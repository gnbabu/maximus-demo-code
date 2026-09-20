#!perl
use strict; 
use warnings;
use Text::ParseWords;
no warnings 'uninitialized';

# OHPNM-3821 - this script can be run as-is w/o inputs; it'll prompt you and provider instructions
# This script takes data from the 2.05 PNM Spreadsheet and analyzes a live system against it to see where there are discrepancies between the requirements and implementation

my $tab_to_process_now = ''; # options are orp_provider_types or provider_types; select one, then run perl script to get results, or leave it blank and it'll prompt for one
my $ignored_pages = ""; # list of pages that were found that aren't listed below in %pnm_pages hash
my @provider_type_names_columns = (); # list of all provider type names from spreadsheet (mostly used for MMIS provider type #19)
my @provider_type_columns = (); # list of all provider types
my @specialty_type_columns = (); # list of all specialty types
my @pages_in_spreadsheet_table_in_order = (); # list of pages in the order they are in 2.05 spreadsheet
my @possibly_bad = (); # list of pages that don't match the spreadsheet
my %entity_types = ('Individual' => 1, 'Group' => 2, 'Organization' => 3, 'Facility' => 4, 'Pharmacy' => 5); # for provider type #19
my $conditionals_that_need_confirmation = "";
my $conditionals_that_are_required = "";
my %superfluous_provider_types_in_live_system = ();
my %page_actual;
my %tabs_to_process = ('provider_types' => {
							'csv_to_load' => 'provider_types.csv',
							'number_of_provider_type_columns' => 123,
							'app_type' => 'all_but_2',
							'tab_name' => 'Provider Types'
						},
						'orp_provider_types' => {
							'csv_to_load' => 'orp_provider_types.csv',
							'number_of_provider_type_columns' => 27,
							'app_type' => '2',
							'tab_name' => 'ORP Provider Types'
						});
# define all pages, so we don't have to store this type of data (page id, section id, etc) in 2.05 spreadsheet
my $file_name = "FILE_NAME";
my $reg_page_type_id = "REG_PAGE_TYPE_ID";
my $reg_section_type_id = "REG_SECTION_TYPE_ID";
# OHPNM-4953 -- removed Office Information page since it's struck-through
my %pnm_pages = (
					'Provider Information' => {												$file_name => 'OrgInfo.ascx', 								$reg_page_type_id => 1, 	$reg_section_type_id => 1},
					'Primary Contact Information' => {										$file_name => 'PrimaryContactAddress.ascx', 				$reg_page_type_id => 1, 	$reg_section_type_id => 2},
					'Credentialing Contact' => {											$file_name => 'CredentialingContact.ascx', 					$reg_page_type_id => 29, 	$reg_section_type_id => 71},
					'CPC Contact' => {														$file_name => 'CPCContactInformation.ascx', 				$reg_page_type_id => 1, 	$reg_section_type_id => 73},
					'Primary Service Address' => {											$file_name => 'PrimaryServiceAddress.ascx', 				$reg_page_type_id => 3, 	$reg_section_type_id => 27},
					'Billing Payment Address' => {											$file_name => 'BillingPaymentAddress.ascx', 				$reg_page_type_id => 3, 	$reg_section_type_id => 28},
					'Correspondence Address' => {											$file_name => 'CorrespondenceAddress.ascx', 				$reg_page_type_id => 3, 	$reg_section_type_id => 29},
					'Other Service Locations' => {											$file_name => 'SatellitePracticeLocations.ascx', 			$reg_page_type_id => 3, 	$reg_section_type_id => 46},
					'1099 Address' => {														$file_name => 'Form1099Address.ascx', 						$reg_page_type_id => 3, 	$reg_section_type_id => 55},
					'Home Office Address' => {												$file_name => 'HomeOfficeAddress.ascx', 					$reg_page_type_id => 3, 	$reg_section_type_id => 53},
					'Long Term Care Addresses' => {											$file_name => 'NursingFacilityAddress.ascx', 				$reg_page_type_id => 3, 	$reg_section_type_id => 54},
					'Hospital Addresses' => {												$file_name => 'HospitalAddress.ascx', 						$reg_page_type_id => 3, 	$reg_section_type_id => 56},
					'Provider Specialties Page' => {										$file_name => 'Specialties.ascx', 							$reg_page_type_id => 2, 	$reg_section_type_id => 3},
					'Provider Taxonomies Page' => {											$file_name => 'Taxonomies.ascx', 							$reg_page_type_id => 2, 	$reg_section_type_id => 4},
					'Professional License Page' => {										$file_name => 'Licenses.ascx', 								$reg_page_type_id => 2, 	$reg_section_type_id => 7},
					'Board Certifications' => {												$file_name => 'BoardCertification.ascx', 					$reg_page_type_id => 2, 	$reg_section_type_id => 52},
					'CLIA Certifications' => {												$file_name => 'CertSecondGrid.ascx', 						$reg_page_type_id => 2, 	$reg_section_type_id => 5},
					'Medicare Page' => {													$file_name => 'MiscellaneousSection.ascx/Medicare.ascx', 	$reg_page_type_id => 2, 	$reg_section_type_id => 26},
					'Provider Type Behavioral Health' => {									$file_name => 'BehavioralHealthInfo.ascx', 					$reg_page_type_id => 2, 	$reg_section_type_id => 21},
					'Pharmacy Provider' => {												$file_name => 'PharmacySection.ascx', 						$reg_page_type_id => 2, 	$reg_section_type_id => 24},
					'CPR and First Aid' => {												$file_name => 'CPRCertification.ascx', 						$reg_page_type_id => 2, 	$reg_section_type_id => 11},
					'DME Providers Page' => {												$file_name => 'DME.ascx', 									$reg_page_type_id => 2, 	$reg_section_type_id => 37},
					'Provider Group Facility Hospital for Group Affiliations' => {			$file_name => 'GroupAffiliations.ascx', 					$reg_page_type_id => 4, 	$reg_section_type_id => 34},
					'Provider Group Facility Hospital for Individual Affiliations' => {		$file_name => 'GroupAndFacilityAffiliationsView.ascx', 		$reg_page_type_id => 21, 	$reg_section_type_id => 33},
					'MCP Affiliation Page' => {												$file_name => 'MCOAffiliations.ascx', 						$reg_page_type_id => 27, 	$reg_section_type_id => 47},
					'Nursing Facility Ventilator' => {										$file_name => 'NursingFacilityVentilator.ascx', 			$reg_page_type_id => 2, 	$reg_section_type_id => 64},
					'Controlled Dangerous Substance (CDS)' => {								$file_name => 'CDS.ascx', 									$reg_page_type_id => 29, 	$reg_section_type_id => 43},
					'Federal Drug Enforcement Agency (DEA) Registration' => {				$file_name => 'Certifications.ascx', 						$reg_page_type_id => 2, 	$reg_section_type_id => 42},
					'Professional Liability Insurance' => {									$file_name => 'Insurance.ascx', 							$reg_page_type_id => 29, 	$reg_section_type_id => 6},
					'Education and Training' => {											$file_name => 'Education.ascx', 							$reg_page_type_id => 2, 	$reg_section_type_id => 48},
					'Malpractice Claims History' => {										$file_name => 'MalpracticeClaim.ascx', 						$reg_page_type_id => 29, 	$reg_section_type_id => 39},
					'Work History' => {														$file_name => 'WorkHistoryDetails.ascx', 					$reg_page_type_id => 29, 	$reg_section_type_id => 49},
					'W9 Form' => {															$file_name => 'FormW9.ascx', 								$reg_page_type_id => 17, 	$reg_section_type_id => 36},
					'ACH Authorization Page' => {											$file_name => 'ACHAuthorization.ascx', 						$reg_page_type_id => 17, 	$reg_section_type_id => 45},
					'Application Fee' => {													$file_name => 'ApplicationFee.ascx', 						$reg_page_type_id => 17, 	$reg_section_type_id => 17},
					'Ownership Disclosure' => {												$file_name => 'OwnerInformation.ascx', 						$reg_page_type_id => 5, 	$reg_section_type_id => 35},
					'Document Upload' => {													$file_name => 'OtherDocUploadSectionControl.ascx', 			$reg_page_type_id => 8, 	$reg_section_type_id => 44},
					'Provider Agreements' => {												$file_name => 'Agreements.ascx', 							$reg_page_type_id => 8, 	$reg_section_type_id => 8},
					'CPC Specialites' => {													$file_name => 'CPCSpecialties.ascx', 						$reg_page_type_id => 2, 	$reg_section_type_id => 74},
					'CPC Attestation' => {													$file_name => 'CPCAttestation.ascx', 						$reg_page_type_id => 8, 	$reg_section_type_id => 76},
					'CPC Practice Partnership' => {											$file_name => 'PracticePartnership.ascx', 					$reg_page_type_id => 4, 	$reg_section_type_id => 75}
				);

my $instructions = "
To analyze the 2.05 spreadsheet requirements against what's in a live system, do the following (save all files locally next to the perl script).

1) Save 2.05 'Provider Types' tab as CSV to file 'provider_types.csv' and save 'ORP Provider Types' tab as csv to file 'orp_provider_types.csv'

2) Save results of the following command to file 'list_of_provider_types.txt':

SELECT MMIS_PROVIDER_TYPE_ID, APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID 
FROM PROVIDER_TYPE 
WHERE IS_USED_IN_MMIS = 'Y' 
ORDER BY MMIS_PROVIDER_TYPE_ID 

3) Save results of the following command to file 'all_reg_page_setting_data.txt':

SELECT providerType.MMIS_PROVIDER_TYPE_ID, 
	regPageSetting.IS_VISIBLE,
	regPageSetting.IS_REQUIRED, 
	regPageSetting.APPLICATION_TYPE_ID, 
  	regPageSetting.ENTITY_TYPE_ID,
  	regPageSetting.REG_PAGE_TYPE_ID,
  	regPageSetting.REG_SECTION_TYPE_ID 
FROM REG_PAGE_SETTING regPageSetting
JOIN PROVIDER_TYPE providerType on providerType.PROVIDER_TYPE_ID = regPageSetting.PROVIDER_TYPE_ID 
ORDER BY providerType.MMIS_PROVIDER_TYPE_ID

4) Save results of the following command to file 'specialty_overrides.txt':

SELECT pt.MMIS_PROVIDER_TYPE_ID, st.MMIS_SPECIALTY_TYPE_ID, reg_section_type_id, pt.PROVIDER_TYPE_ID, st.SPECIALTY_TYPE_ID, is_visible, IS_REQUIRED 
FROM REG_SECTION_SETTING_Provider_Specialty_Type override 
JOIN provider_type pt on pt.PROVIDER_TYPE_ID = override.Provider_TYPE_ID join SPECIALTY_TYPE st on st.SPECIALTY_TYPE_ID = override.Specialty_type_id
WHERE pt.IS_USED_IN_MMIS = 'Y'
ORDER BY pt.MMIS_PROVIDER_TYPE_ID
";

# if $tab_to_process_now is empty, give them instructions and prompt for what tab to analyze
if ($tab_to_process_now eq "") {
	print $instructions . "\n";
	print "Which tab in the 2.05 spreadsheet would you like to analyze?\n\n";
	print "1) Provider Types\n";
	print "2) ORP Provider Types\n\n";
	print "Type '1' or '2' and hit 'Enter': ";
	my $answer = <STDIN>;
	chomp($answer);
	if ($answer == 2) {
		$tab_to_process_now = "orp_provider_types";	
	}
	else {
		$tab_to_process_now = "provider_types";	
	}
}

my $number_of_provider_type_cols = $tabs_to_process{$tab_to_process_now}{"number_of_provider_type_columns"};

open(PROVIDER_TYPES, $tabs_to_process{$tab_to_process_now}{"csv_to_load"});
my @provider_types = <PROVIDER_TYPES>;
chomp(@provider_types);
close(PROVIDER_TYPES);

my $found_first_page = 0;
my $each_col = 0;
my %page_requirements;
for (my $i = 0; $i < @provider_types; $i++) {
	my @split_line;
	if ($provider_types[$i] =~ /Page Name/) {
		# have to have a special split for the page name row
		@split_line = split(',', $provider_types[$i]);
	}
	else {
		@split_line = parse_line(q{,}, 0, $provider_types[$i]);
	}

	if ($split_line[1] eq "Page Name") {
		# Found provider type name
		for ($each_col = 2; $each_col <= $number_of_provider_type_cols; $each_col++) {
			push(@provider_type_names_columns, $split_line[$each_col]);
		}
		next;
	}
	
	if ($split_line[0] eq "" && $found_first_page) {
		# at end of file
		last;
	}
	
	if ($split_line[0] eq "" && $split_line[1] eq "PROVIDER TYPE") {
		# Found provider types row	
		for ($each_col = 2; $each_col <= $number_of_provider_type_cols; $each_col++) {
			push(@provider_type_columns, $split_line[$each_col]);
		}
		next;
	}
	
	if ($split_line[0] eq "" && $split_line[1] eq "Yes") {
		# Found specialty types row
		for ($each_col = 2; $each_col <= $number_of_provider_type_cols; $each_col++) {
			push(@specialty_type_columns, $split_line[$each_col]);
		}
		next;
	}

	if ($split_line[0] eq "Provider Entry/Provider Access Pages") {
		$found_first_page = 1;
		
		my $current_page = $split_line[1];
		$current_page =~ s/\s+$//; # remove extra space from end of strings
		
		if ($pnm_pages{$current_page} eq undef) {
			$ignored_pages .= $current_page . "\n";
			next;
		}
		else {
			push(@pages_in_spreadsheet_table_in_order, $current_page);
		}
		
		for ($each_col = 2; $each_col <= $number_of_provider_type_cols; $each_col++) {
			my $provider_type = $provider_type_columns[($each_col - 2)];
		
			my $current_value = $split_line[$each_col];
			$current_value =~ s/\s+$//;
	
			if ($current_value eq "Optional") {
				# OHPNM-4953 -- they are using "Optional" instead of "O" in some fields
				$current_value = "O";
			}
			
			my $specialty = $specialty_type_columns[($each_col - 2)];
			if ($specialty eq "" || $specialty =~ /,/) {
				$specialty = "ALL";
			}
			else {
				# deal with spreadsheet having short specialty type ids, like "23" instead of "023"
				if (length($specialty) == 2) {
					$specialty = "0" . $specialty;
				}
			}
			
			if ($provider_type eq "19") {
				# have to deal with PT 19 differently; #19's specialty types are really entity types (1=Individual, 2=Group, 3=Organization, 4=Facility, 5=Pharmacy)
				$provider_type_names_columns[($each_col - 2)] =~ /.*\((.*?)\)/;
				$specialty = $entity_types{$1};
			}
			
			if ($page_requirements{$pnm_pages{$current_page}{$reg_section_type_id}}{$provider_type}{$specialty} eq undef) { 
				# print "Store page_requirements{" . $pnm_pages{$current_page}{$reg_section_type_id} . "}{" . $provider_type_columns[($each_col - 2)] . "}{" . $specialty . "} = " . $current_value . "\n";
				$page_requirements{$pnm_pages{$current_page}{$reg_section_type_id}}{$provider_type}{$specialty} = $current_value;
			}
		}
	}
}

# get list of all provider types, then organize by app type [1, 3, 4, 5, 6, 7]="Provider Types" tab and [2]="ORP Provider Types"
# select MMIS_PROVIDER_TYPE_ID, APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID from provider_type where IS_USED_IN_MMIS = 'Y' order by MMIS_PROVIDER_TYPE_ID 
my @all_provider_types;
my @provider_types_on_provider_types_tab = (); # 1, 3, 4, 5, 6, 7
my @provider_types_on_orp_provider_types_tab = (); # just # 2

open(LIST_OF_PROVIDER_TYPES, "list_of_provider_types.txt");
@all_provider_types = <LIST_OF_PROVIDER_TYPES>;
chomp(@all_provider_types);
close(LIST_OF_PROVIDER_TYPES);

for (my $each_provider_type = 0; $each_provider_type < @all_provider_types; $each_provider_type++) {
	my @split_line = split(/\t/, $all_provider_types[$each_provider_type]);
	
	if ($split_line[1] == 2) {
		push(@provider_types_on_orp_provider_types_tab, $split_line[0]);
	}
	else {
		push(@provider_types_on_provider_types_tab, $split_line[0]);
	}
	
	if (($split_line[1] == 2 && $tab_to_process_now eq "orp_provider_types") || ($split_line[1] != 2 && $tab_to_process_now eq "provider_types")) {
		# see if this provider type is on our spreadsheet
		my $found_provider_type = 0;
		for (my $i = 0; $i < @provider_type_columns; $i++) {
			if ($split_line[0] eq $provider_type_columns[$i] ) {
				$found_provider_type = 1;
				last;
			}
		}
		
		if ($found_provider_type == 0) {
			$superfluous_provider_types_in_live_system{$split_line[0]} = 1;
		}	
	}
}

my @live_reg_page_setting_data;
open(LIVE_REG_PAGE_SETTING_DATA, "all_reg_page_setting_data.txt");
@live_reg_page_setting_data = <LIVE_REG_PAGE_SETTING_DATA>;
chomp(@live_reg_page_setting_data);
close(LIVE_REG_PAGE_SETTING_DATA);

for (my $each_row = 0; $each_row < @live_reg_page_setting_data; $each_row++) {
	my @split_line = split(/\t/, $live_reg_page_setting_data[$each_row]); # MMIS_PROVIDER_TYPE_ID, IS_VISIBLE, IS_REQUIRED, APPLICATION_TYPE_ID, ENTITY_TYPE_ID, REG_PAGE_TYPE_ID, REG_SECTION_TYPE_ID 
	
	my $specialty = "ALL";
	my $provider_type = $split_line[0];
	my $entity_type = $split_line[4]; # for PT #19
	
	if ($tab_to_process_now eq 'orp_provider_types' && $split_line[3] != 2) {
			next;
	}
	
	if ($tab_to_process_now eq 'provider_types' && $split_line[3] == 2) {
			next;
	}

	my $current_value = "";
	# check "is visible"
	if ($split_line[1] eq "1") {
		# check "is required"
		if ($split_line[2] eq "1") {
			$current_value = "R";
		}
		else {
			$current_value = "O";
		}
	}
	else {
		$current_value = "N"
	}
	
	if ($provider_type eq "19") {
		# Processing live data on 19, change specialty = $entity_type
		$specialty = $entity_type;
	}

	# store off actual value
	$page_actual{$split_line[6]}{$provider_type}{$specialty} = $current_value;
	# print "SETTING page_actual{" . $split_line[6] . "}{" . $split_line[0] . "}{$specialty} = " . $current_value . " // " . $page_actual{$split_line[6]}{$split_line[0]}{$specialty} . "\n";
}


# overrides -> these are overrides in the live system in table "REG_SECTION_SETTING_Provider_Specialty_Type"
my @live_override_data;
open(LIVE_OVERRIDE_DATA, "specialty_overrides.txt");
@live_override_data = <LIVE_OVERRIDE_DATA>;
chomp(@live_override_data);
close(LIVE_OVERRIDE_DATA);

my %override_actual;
for (my $each_row = 0; $each_row < @live_override_data; $each_row++) {
	my @split_line = split(/\t/, $live_override_data[$each_row]); # MMIS_PROVIDER_TYPE_ID MMIS_SPECIALTY_TYPE_ID reg_section_type_id PROVIDER_TYPE_ID SPECIALTY_TYPE_ID is_visible, IS_REQUIRED
	
	my $current_value = "";
	# check "is visible"
	if ($split_line[5] eq "1") {
		# check "is required"
		if ($split_line[6] eq "1") {
			$current_value = "R";
		}
		else {
			$current_value = "O";
		}
	}
	else {
		$current_value = "N"
	}
	
	$override_actual{$split_line[2]}{$split_line[0]}{$split_line[1]} = $current_value; # override_actual{SECTION_ID}{PT}{ST}
	# print "SETTING override_actual{" . $split_line[2] . "}{" . $split_line[0] . "}{" . $split_line[1] . "} = " . $current_value . " // " . $override_actual{$split_line[2]}{$split_line[0]}{$split_line[1]} . "\n";
}

print "\nAnalyzing '" . $tabs_to_process{$tab_to_process_now}{"tab_name"} . "' tab in 2.05 spreadsheet.\n\n";
print "NOTE: 'ALL' just means the specialty type that is not specified for a provider type (specialty is blank in the spreadsheet).\n\n";

# Go through all pages and cols and check requirements versus actual
# go through all pages in order [ROW]
# go through each COLUMN and grab PT/ST/VALUE for each
# If spreadsheet ST eq "ALL" compare spreadsheet (page_requirements) with value of page_actual{PT}{ST}{ALL}
# If ST is a number, see if there is an override_actual; if not, use value of page_actual{PT}{ST}{ALL}
my $bad_cnt = 0;
for (my $page = 0; $page < @pages_in_spreadsheet_table_in_order; $page++) {
	for (my $col = 0; $col < @provider_type_columns; $col++) {
		
		my $specialty = $specialty_type_columns[$col];
		if ($specialty eq "" || $specialty =~ /,/) {
			$specialty = "ALL";
		}
		else {
			# deal with spreadsheet having short specialty type ids, like "23" instead of "023"
			if (length($specialty) == 2) {
				$specialty = "0" . $specialty;
			}
		}
		
		my $section_id = $pnm_pages{$pages_in_spreadsheet_table_in_order[$page]}{$reg_section_type_id};
		my $provider_type = $provider_type_columns[$col];
		my $expected_value = "";
		my $actual_value = "";
		
		if ($provider_type eq "19") {
			# have to deal with PT 19 differently; #19's specialty types are really entity types (1=Individual, 2=Group, 3=Organization, 4=Facility, 5=Pharmacy)
			$provider_type_names_columns[$col] =~ /.*\((.*?)\)/;
			$specialty = $entity_types{$1};
		}

		if ($specialty eq "ALL") {
			$expected_value = $page_requirements{$section_id}{$provider_type}{$specialty};
			$actual_value = $page_actual{$section_id}{$provider_type}{$specialty};
		}
		else {
			$expected_value = $page_requirements{$section_id}{$provider_type}{$specialty};
			
			# found something in the override table
			if ($override_actual{$section_id}{$provider_type}{$specialty} ne undef) {
				$actual_value = $override_actual{$section_id}{$provider_type}{$specialty};
			}
			# found specialty data in live system (from PT #19 probably)
			elsif ($page_actual{$section_id}{$provider_type}{$specialty} ne undef) {
				$actual_value = $page_actual{$section_id}{$provider_type}{$specialty}
			}
			# couldn't find specialty live data, just use live data for "ALL" for this provider type
			else {
				$actual_value = $page_actual{$section_id}{$provider_type}{"ALL"};
			}
		}

		# if expected value is N and are actual value is either "N" or is not there, then this is good
		if ($expected_value eq "N" && ($actual_value eq "N" || $actual_value eq undef)) {
			# this is good
		}
		elsif ($expected_value eq "R" && $actual_value eq "R") {
			# this is good
		}
		elsif ($expected_value eq "O" && $actual_value eq "O") {
			# this is good
		}
		elsif ($expected_value eq "Conditional" && $actual_value eq "O") {
			$conditionals_that_need_confirmation .= $pages_in_spreadsheet_table_in_order[$page] . " (section_id=" . $section_id . ")(pt=" . $provider_type . ")(st=$specialty)\n";
		}
		elsif ($expected_value eq "Conditional" && $actual_value eq "R") {
			$conditionals_that_are_required .= $pages_in_spreadsheet_table_in_order[$page] . " (section_id=" . $section_id . ")(pt=" . $provider_type . ")(st=$specialty)\n";
		}
		else {
			print "THESE SEEM WRONG: " . $pages_in_spreadsheet_table_in_order[$page] . " (section_id=" . $section_id . ")(pt=" . $provider_type . ")(st=$specialty): " . $expected_value . " != " . $actual_value . "\n";
			$bad_cnt++;
		}
	}
	
}

print "\n" . $bad_cnt . " possibly bad\n";

print "\nThe following pages were ignored since they don't have a page/section ID:\n\n" . $ignored_pages;

if ($conditionals_that_need_confirmation ne "") {
	print "\nThese are marked as 'conditional' in 2.05, and they are 'optional' in the system, but someone needs to manually confirm the conditional code:\n\n" . $conditionals_that_need_confirmation . "\n";
}

if ($conditionals_that_are_required ne "") {
	print "These are marked as 'conditional' in 2.05, and they are 'required' in the system, but that doesn't make sense; they need to be looked into:\n\n" . $conditionals_that_are_required . "\n";
}

if (keys %superfluous_provider_types_in_live_system > 0) {
	# print out provider types that are in system but aren't in either tab on the spreadsheet
	print "The following provider types are in the live system, but aren't on this spreadsheet tab:\n\n";
	foreach my $key (sort (keys(%superfluous_provider_types_in_live_system))) {
	   print "$key\n";
	}
}

