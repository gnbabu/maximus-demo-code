using Corp.Core.Libraries.ProviderManagementReference;
using MAXIMUS.Models.Data.PDMS;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PDMSRestServices.Models
{
    public class ProviderDetails
    {
        public string SourceSystem { get; set; }
        public string Correlation_Id { get; set; }
        public string TargetSystem { get; set; }
        [ValidateNever]
        public List<Provider> Providers { get; set; }
    }
    public class Provider
    {
        public string Segment_Id { get; set; }
        public string Enr_Prv_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }

        public string NPI { get; set; }
        public string SSN { get; set; }
        public string FEIN { get; set; }
        public string? NPI_StartDate { get; set; }
        public string? NPI_EndDate { get; set; }
        public string CAQH { get; set; }
        public string MCARE { get; set; }

        public string? MCARE_StartDate { get; set; }
        public string? MCARE_EndDate { get; set; }
        public string Attestation_Indicator { get; set; }
        public List<WorkGap> WorkGaps { get; set; }
        public ProviderDemographics ProviderDemographics { get; set; }
        public ProviderEnrollment ProviderEnrollment { get; set; }
        public ProviderAddress ProviderAddress { get; set; }
        public ProviderContact ProviderContact { get; set; }
        public CredentialingContact CredentialingContact { get; set; }
        public List<PracticeLocation> PracticeLocations { get; set; }
        public List<ProviderClassification> ProviderClassifications { get; set; }
        public List<Taxonomy> ProviderTaxonomies { get; set; }
        public Medicare Medicare { get; set; }
        public List<BoardCertification> BoardCertifications { get; set; }
        public List<License> Licenses { get; set; }
        public DEALicense DEALicense { get; set; }
        public CDSLicense CDSLicense { get; set; }
        public List<Education> Educations { get; set; }
        public List<WorkHistory> WorkHistories { get; set; }
        public List<Insurance> Insurances { get; set; }
        public List<MalpracticeClaim> MalpracticeClaims { get; set; }
        public List<McoAffiliation> McoAffiliations { get; set; }
        public List<GroupAffiliation> GroupAffiliations { get; set; }
        public List<HospitalPrivilege> HospitalPrivileges { get; set; }
        public List<OtherDocumentation> OtherDocumentations { get; set; }
        public List<CLIACertificate> CliaCertificates { get; set; }
    }

    public class Provider_ID
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Prv_Id { get; set; }
        public string Provider_Id_Type { get; set; }
        public string Provider_Id_Value { get; set; }
        public string? Pro_Start_Date { get; set; }
        public string? Pro_End_Date { get; set; }
        public string Attestation_Indicator { get; set; }

        public int Is_API_Request { get; set; }

        public int Staging_Cred_Provider_Header_Id { get; set; }
    }
    public class Provider_Header
    {
        public string Segment_Id { get; set; }
        public string Enrollment_File_Id { get; set; }
        public string Enrollment_File_Name { get; set; }
        public string Source_System { get; set; }
        public string Target_System { get; set; }
        public string Record_Count { get; set; }
    }

    public class WorkGap
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_WorkGaps_Id { get; set; }
        public string Enr_Insurance_Internal_Note { get; set; }
        public string Gap_Start_Date { get; set; }
        public string Gap_End_Date { get; set; }
        public string Gap_Reason { get; set; }
    }

    public class ProviderDemographics
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Prv_Demo_Id { get; set; }
        public string Provider_Name_First { get; set; }
        public string Provider_Name_Middle_Initial { get; set; }
        public string Provider_Name_Last { get; set; }
        public string Org_Name { get; set; }
        public string Dba_Name { get; set; }
        public string Provider_Dob { get; set; }
        public string Provider_Gender { get; set; }
        public string Birth_Country { get; set; }
        public string Birth_State { get; set; }
        public string Birth_City { get; set; }
        public string Provider_Title { get; set; }
        public string Provider_Office_Manager { get; set; }
        public string Residing_Ohio_Last_5_Years { get; set; }
        public string Ownership_Type { get; set; }
        public string Previously_Delegated_Provider { get; set; }
    }

    public class ProviderEnrollment
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Enr_Id { get; set; }
        public string Enrollment_Effective_Date { get; set; }
        public string Enrollment_End_Date { get; set; }
        public string Enumeration_Type { get; set; }
        public string Enrollment_Status { get; set; }
        public string Enrollment_Status_Reason { get; set; }
        public string Revalidation_Date { get; set; }
        public string Recredential_Date { get; set; }
        public string Application_Status { get; set; }
    }

    public class ProviderAddress
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Prv_Addr_Id { get; set; }
        public string Address_Type { get; set; }
        public string Address_Line_1 { get; set; }
        public string Address_Line_2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public string Zip { get; set; }
        public string Zip_Ext { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
    }

    public class ProviderContact
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Prv_Contact_Id { get; set; }
        public string Contact_Type { get; set; }
        public string Contact_Name { get; set; }
        public string Contact_Title { get; set; }
        public string Contact_Phone { get; set; }
        public string Contact_Phone_Ext { get; set; }
        public string Receive_Text_Number_1 { get; set; }
        public string Contact_Fax { get; set; }
        public string Contact_Email { get; set; }
        public string Contact_Address_1 { get; set; }
        public string Contact_Address_2 { get; set; }
        public string Contact_City { get; set; }
        public string Contact_County { get; set; }
        public string Contact_State { get; set; }
        public string Contact_Zip { get; set; }
        public string Contact_Ext_Zip { get; set; }
        public string Contact_Phone_2 { get; set; }
        public string Receive_Text_Number_2 { get; set; }
        public string Contact_Fax_2 { get; set; }
        public string Contact_Email_2 { get; set; }
        public string Office_Manager { get; set; }
        public string Contact_Phone_2_Ext { get; set; }
    }

    public class CredentialingContact
    {
        public string? Segment_Id { get; set; }
        public string? Provider_Sequence_Number { get; set; }
        public string? Enr_Prv_Contact_Id { get; set; }
        public string? Contact_Type { get; set; }
        public string? Contact_Name { get; set; }
        public string? Practice_Name { get; set; }
        public string? Contact_Phone { get; set; }
        public string? Contact_Phone_Ext { get; set; }
        public string? Contact_Fax { get; set; }
        public string? Contact_Email { get; set; }
        public string? Contact_Title { get; set; }
        public string? Receive_Text_Number_1 { get; set; }
        public string? Contact_Address_1 { get; set; }
        public string? Contact_Address_2 { get; set; }
        public string? Contact_City { get; set; }
        public string? Contact_County { get; set; }
        public string? Contact_State { get; set; }
        public string? Contact_Zip { get; set; }
        public string? Contact_Ext_Zip { get; set; }
        public string? Contact_Phone_2 { get; set; }
        public string? Receive_Text_Number_2 { get; set; }
        public string? Contact_Fax_2 { get; set; }
        public string? Contact_Email_2 { get; set; }
        public string? Office_Manager { get; set; }
    }

    public class PracticeLocation
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Pract_Loc_Id { get; set; }
        public string Practice_Location_Id { get; set; }
        public string Practice_Location_Name { get; set; }
        public string Address_Type { get; set; }
        public string Address_Line_1 { get; set; }
        public string Address_Line_2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public string Zip { get; set; }
        public string Zip_Ext { get; set; }
        public string Phone_1 { get; set; }
        public string Phone_1_Ext { get; set; }
        public string Phone_Number_2 { get; set; }
        public string Phone_Ext_2 { get; set; }
        public string Fax_1 { get; set; }
        public string Fax_2 { get; set; }
        public string Email { get; set; }
        public string Contact_Name { get; set; }
        public string Practice_Start_Date { get; set; }
        public string Practice_End_Date { get; set; }
    }

    public class ProviderClassification
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Prv_Class_Id { get; set; }
        public string Provider_Specialty_Code { get; set; }
        public string Provider_Type_Code { get; set; }
        public string Primary_Flag { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
    }

    public class Taxonomy
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Taxonomy_Id { get; set; }
        public string Taxonomy_Code { get; set; }
        public string Primary_Flag { get; set; }
    }

    public class Medicare
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Medicare_Id { get; set; }
        public string Enr_Medicare_Internal_Note { get; set; }
        public string Medicare_Number { get; set; }
        public string Medicare_Number_Type { get; set; }
        public string Medicare_State { get; set; }
        public string Medicare_Enrollment_Date { get; set; }
        public string Medicare_Status { get; set; }
    }

    public class BoardCertification
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Board_Cert_Aff_Id { get; set; }
        public string Enr_Board_Cert_Internal_Note { get; set; }
        public string Effective_Date { get; set; }
        public string Expiration_Date { get; set; }
        public string Certification_Number { get; set; }
        public string Board_Specialty { get; set; }
        public string Board_Certification_Url { get; set; }
        public string Board_Certification_Url_Document_Name { get; set; }
        public string Certifying_Board_Name { get; set; }
    }

    public class License
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_License_Id { get; set; }
        public string Enr_License_Internal_Note { get; set; }
        public string License_Type { get; set; }
        public string License_Number { get; set; }
        public string License_State { get; set; }
        public string License_Start_Date { get; set; }
        public string License_End_Date { get; set; }
        public string License_Issue_Date { get; set; }
        public string License_Board_Name { get; set; }
        public string License_Url { get; set; }
        public string License_Url_Document_Name { get; set; }
        public string Facility_License_Url { get; set; }
        public string Facility_License_Url_Document_Name { get; set; }
    }

    public class DEALicense
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Dea_Id { get; set; }
        public string Enr_Dea_Internal_Note { get; set; }
        public string Dea_Number { get; set; }
        public string Dea_State { get; set; }
        public string Dea_Eff_Date { get; set; }
        public string Dea_End_Date { get; set; }
        public string Dea_Url { get; set; }
        public string Dea_Url_Document_Name { get; set; }
        public string Dea_Status { get; set; }
    }

    public class CDSLicense
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Cds_Id { get; set; }
        public string Enr_Cds_Internal_Note { get; set; }
        public string Cds_Number { get; set; }
        public string State { get; set; }
        public string Cds_Date_Issued { get; set; }
        public string Cds_Expiration_Date { get; set; }
        public string Cds_Url { get; set; }
        public string Cds_Url_Document_Name { get; set; }
    }

    public class Education
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Educ_Id { get; set; }
        public string Enr_Educ_Internal_Note { get; set; }
        public string School_Name { get; set; }
        public string Field_Of_Study { get; set; }
        public string Start_Year { get; set; }
        public string End_Year { get; set; }
        public string Degree_Awarded_Type { get; set; }
        public string Education_Url_School_Diploma { get; set; }
        public string Education_Url_School_Diploma_Document_Name { get; set; }
        public string Education_Type { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Zip_Ext { get; set; }
        public string Country { get; set; }
        public string Phone_Number { get; set; }
        public string Phone_Number_Ext { get; set; }
        public string Graduation_Year { get; set; }
    }

    public class WorkHistory
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Work_Hist_Id { get; set; }
        public string Enr_Work_Hist_Internal_Note { get; set; }
        public string Title { get; set; }
        public string Employer_Name_Name_Of_Organization { get; set; }
        public string Worked_From { get; set; }
        public string Worked_To { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Ext_Zip { get; set; }
        public string Country { get; set; }
        public string Contact_Name { get; set; }
        public string Contact_Phone { get; set; }
        public string Contact_Phone_Ext { get; set; }
        public string Contact_Email_Address { get; set; }
        public string Workhistory_Url { get; set; }
        public string Workhistory_Url_Document_Name { get; set; }
        public string Reason_For_Departure { get; set; }
        public string Is_Current_Employer { get; set; }
        public string Military_Reserve_Active_Military_Duty { get; set; }
    }

    public class Insurance
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Insurance_Id { get; set; }
        public string Enr_Insurance_Internal_Note { get; set; }
        public string Policy_Number { get; set; }
        public string Policy_Holder { get; set; }
        public string Effective_Date { get; set; }
        public string Expiration_Date { get; set; }
        public string Policy_Url { get; set; }
        public string Policy_Url_Document_Name { get; set; }
        public string Carrier_Name { get; set; }
        public string Coverage_Amount_Per_Occurrence { get; set; }
        public string Coverage_Amount_Per_Aggregate { get; set; }
        public string Coverage_Type { get; set; }
    }
    public class MalpracticeClaim
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Malpractice_Clm_Id { get; set; }
        public string Enr_Malpractice_Clm_Internal_Note { get; set; }
        public string Is_Malpractice { get; set; }
        public string Date_Occurrence { get; set; }
        public string Date_Claim_Filed { get; set; }
        public string Professional_Carrier { get; set; }
        public string Date_Filed { get; set; }
        public string Claim_Status { get; set; }
        public string Policy_Number { get; set; }
        public string Your_Role_Case { get; set; }
        public string Alleged_Injury_To_Patient_Description { get; set; }
        public string Claim_Settled_Date { get; set; }
        public string Settlement_Amount { get; set; }
        public string Method_Of_Resolution { get; set; }
    }

    // ---------------- PROVIDER AFFILIATED PROGRAMS ----------------
    public class Provider_Affiliated_Programs
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Prv_Pgm_Aff_Id { get; set; }
        public string Enr_Prov_Affil_Pgms_Internal_Note { get; set; }
        public string Program { get; set; }
        public string Program_Type_Code { get; set; }
        public string Program_Effective_Date { get; set; }
        public string Program_End_Date { get; set; }
    }

    // ---------------- GROUP AFFILIATIONS ----------------
    public class GroupAffiliation
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Grp_Aff_Id { get; set; }
        public string Enr_Grp_Aff_Internal_Note { get; set; }
        public string Group_Name { get; set; }
        public string Npi { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public string Status { get; set; }
    }

    // ---------------- HOSPITAL PRIVILEGE ----------------
    public class HospitalPrivilege
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Hosp_Aff_Id { get; set; }
        public string Enr_Hosp_Aff_Internal_Note { get; set; }
        public string Hospital_Id { get; set; }
        public string Hospital_Name { get; set; }
        public string Is_Required { get; set; }
        public string Hospital_Privilege { get; set; }
        public string Is_Primary_Hospital { get; set; }
        public string Admitting_Privilege_Status { get; set; }
        public string Staff_Category { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
    }

    // ---------------- CLIA CERTIFICATE ----------------
    public class CLIACertificate
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Clia_Id { get; set; }
        public string Enr_Clia_Internal_Note { get; set; }
        public string Clia_Certification_Number { get; set; }
        public string Clia_Eff_Date { get; set; }
        public string Clia_Expiration_Date { get; set; }
        public string Clia_Url { get; set; }
        public string Clia_Url_Document_Name { get; set; }
        public string Clia_Certification_Type { get; set; }
    }

    // ---------------- MCO AFFILIATION ----------------
    public class McoAffiliation
    {
        public string Segment_Id { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Mco_Aff_Id { get; set; }
        public string Enr_Mco_Internal_Note { get; set; }
        public string Plan_Code { get; set; }
        public string Plan_Name { get; set; }
        public string Effective_Date { get; set; }
        public string End_Date { get; set; }
    }

    // ---------------- OTHER DOCUMENTATION ----------------
    public class OtherDocumentation
    {
        public string Segment_ID { get; set; }
        public string Provider_Sequence_Number { get; set; }
        public string Enr_Other_Documentation_ID { get; set; }
        public string Documentation_Type { get; set; }
        public string Documentation_Url { get; set; }
        public string Documentation_Url_Document_Name { get; set; }
    }

}