<%@ page title="Provider Directory" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_PublicSearchAPI, App_Web_rnw0hezi" enableEventValidation="false" stylesheettheme="Default" %>

<%@ register assembly="SCS.WebControls.GroupBox" namespace="SCS.WebControls" tagprefix="cc1" %>
<%@ register src="~/PopupControls/MessageBox.ascx" tagname="MessageBox" tagprefix="cc2" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <%-- Provider Directory--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        table {
            text-align: center;
        }

        th, td {
            padding: 5px;
            border: inset; /* Adds border to table cells */

                       
        }

        .align-checkbox {
            text-align: center;
        }

        .popover {
            width: 240px;
        }

        @media only screen and (max-width: 760px) {
            input[type=text], input[type=password], textarea, select {
                width: 100% !important;
                min-width: 100px !important;
            }

            .col-sm-12 {
                padding-right: 0px;
                padding-left: 0px;
            }

            .WhiteBox {
                padding: 0px;
            }
        }

        @media only screen and (max-width: 990px) {
            input[type=text], input[type=password], textarea, select {
                width: 100% !important;
                min-width: 100px !important;
            }
        }

        @media only screen and (max-width: 400px) {
            .form-control {
                font-size: 11px;
            }

            .WhiteBox {
                padding: 0px;
            }
        }

        .rddlPopup .rddlItem {
            background-color: white;
        }
        .RedAsterisk
        {
            color:#e00;
        }
        .RadComboBorder {
            border-width: 1px;
            border-style: solid;
            border-color: #ccc;
            }
        



legend {
display: none !important;
}


    </style>
    <script type="text/javascript">
        function pageLoad() {
            try {
                $('[data-toggle="popover"]').popover()
            }
            catch (err) {
                console.log('that popover method does not exist at this point: ' + err);
            }
        }
        function validateFields() {

            var isValid = true;

            if ($('#ctl00_MainContent_ddlHealthPlan').val() === '') {
                $('#ctl00_MainContent_rfvddlHealthPlan').css('display', 'block');
                isValid = false;
            } else {
                $('#ctl00_MainContent_rfvddlHealthPlan').css('display', 'none');
            }

            // Provider Type Validation (Telerik RadComboBox)
            var providerCombo = $find("<%=rcbProviderType.ClientID %>");
            var providerTypeCollection = providerCombo.get_checkedItems();
            if (providerTypeCollection.length === 0) {
                $('#ctl00_MainContent_rfvRcbProviderType').css('display', 'block');
                isValid = false;
            } else {
                $('#ctl00_MainContent_rfvRcbProviderType').css('display', 'none');
            }

            // State validation
            if ($('#ctl00_MainContent_ddlState').prop('selectedIndex') === 0) {
                $('#ctl00_MainContent_rfvddlState').css('display', 'block');
                isValid = false;
            } else {
                $('#ctl00_MainContent_rfvddlState').css('display', 'none');
            }

            // Zip, City, and County validation (at least one must be filled)
            if ($('#ctl00_MainContent_txtZip').val().trim() === '' &&
                $('#ctl00_MainContent_txtCity').val().trim() === '' &&
                !isCountySelected()) {

                $('#ctl00_MainContent_rfvtxtZip').css('display', 'block');
                isValid = false;
            } else {
                $('#ctl00_MainContent_rfvtxtZip').css('display', 'none');
            }

            if (isValid) {
                var state = $('#ctl00_MainContent_ddlState').val().trim();
                var zip = $('#ctl00_MainContent_txtZip').val().trim();

                if (state.toUpperCase() !== "OH" && zip === "") {
                    $('#ctl00_MainContent_rfvtxtZip').text("Zip is required when State is not OH.").css('display', 'block');

                    isValid = false;
                } else {
                    $('#ctl00_MainContent_rfvtxtZip').text('').css('display', 'none');
                }
            }

            var zip = $('#ctl00_MainContent_txtZip').val().trim();
            var radiusMiles = $('#ctl00_MainContent_ddlRadiusMiles').val().trim();

            if (zip !== "" && radiusMiles === "") {
                $('#ctl00_MainContent_rfvddlRadiusMiles').css('display', 'block');
                isValid = false;
            } else {
                $('#ctl00_MainContent_rfvddlRadiusMiles').css('display', 'none');
            }

            return isValid;
        }

        function isCountySelected() {
            var countyCombo = $find("<%=rcbCounty.ClientID %>");
            var selectedItems = countyCombo.get_checkedItems();

            if (selectedItems.length > 0) {
                return true;
            } else {
                return false;
            }
        }

        function CreateAPIURL() {

            if (!validateFields()) {
                return false;
            }

            var APIURLParms = "";
            var APIURL = document.getElementById('ctl00_MainContent_hdnpublicsearchapi').value;
            /*"https://localhost:7055/SearchParameter/PublicSearch?"*/
            APIURLParms += APIURL;

            var state = "";
            state = document.getElementById('ctl00_MainContent_ddlState').value;
            APIURLParms += "state=" + state;

            var zip = "";
            zip = document.getElementById('ctl00_MainContent_txtZip').value;
            APIURLParms += "&zip=" + zip;

            var healthplan = "";
            healthplan = document.getElementById('ctl00_MainContent_ddlHealthPlan').value;
            APIURLParms += "&healthplan=" + healthplan;

            var providerTypeSelectedValues = "";
            var combo = $find("<%=rcbProviderType.ClientID %>");
            var providerTypeCollection = combo.get_checkedItems();
            if (providerTypeCollection.length != 0) {
                for (var i = 0; i < providerTypeCollection.length; i++) {
                    providerTypeSelectedValues += providerTypeCollection[i].get_value() + ",";
                }
                providerTypeSelectedValues = providerTypeSelectedValues.replace(/,(?=[^,]*$)/, "");
            }
            APIURLParms += "&ProviderTypeIDsDelimited=" + providerTypeSelectedValues;

            var radius = document.getElementById('ctl00_MainContent_ddlRadiusMiles').value;
            APIURLParms += "&radius=" + radius;

            var specialtyTypeSelectedValues = "";
            var combo = $find("<%=rcbProviderSpeciality.ClientID %>");
            var specialtyTypeCollection = combo.get_checkedItems();
            if (specialtyTypeCollection.length != 0) {
                for (var i = 0; i < specialtyTypeCollection.length; i++) {
                    specialtyTypeSelectedValues += specialtyTypeCollection[i].get_value() + ",";
                }
                specialtyTypeSelectedValues = specialtyTypeSelectedValues.replace(/,(?=[^,]*$)/, "");
            }
            if (specialtyTypeSelectedValues != "")
                APIURLParms += "&SpecialtyTypeIDsDelimited=" + specialtyTypeSelectedValues;

            var Program = "";
            Program = document.getElementById('ctl00_MainContent_ddlProgram').value;
            if (Program != "")
                APIURLParms += "&Program=" + Program;

            var FacilityType = "";
            FacilityType = document.getElementById('ctl00_MainContent_ddlFacilityType').value;
            if (FacilityType != "")
                APIURLParms += "&FacilityType=" + FacilityType;

            var PrimaryCareProviders = "";
            PrimaryCareProviders = document.getElementById('ctl00_MainContent_ddlPrimaryCareProviders').value;
            if (PrimaryCareProviders != "")
                APIURLParms += "&PrimaryCareProviders=" + PrimaryCareProviders;

            var OrgName = "";
            OrgName = document.getElementById('ctl00_MainContent_txtOrgName').value;
            if (OrgName != "")
                APIURLParms += "&OrgName=" + OrgName;

            var DMEProductsServicesSelectedValues = "";
            var combo = $find("<%=rcbDMEProductsServices.ClientID %>");
            var DMEProductsServicesCollection = combo.get_checkedItems();
            if (DMEProductsServicesCollection.length != 0) {
                for (var i = 0; i < DMEProductsServicesCollection.length; i++) {
                    DMEProductsServicesSelectedValues += DMEProductsServicesCollection[i].get_value() + ",";
                }
                DMEProductsServicesSelectedValues = DMEProductsServicesSelectedValues.replace(/,(?=[^,]*$)/, "");
            }
            if (DMEProductsServicesSelectedValues != "")
                APIURLParms += "&DMEProductsServices=" + DMEProductsServicesSelectedValues;

            var AcceptsPatientsAsYoungAs = "";
            AcceptsPatientsAsYoungAs = document.getElementById('ctl00_MainContent_txtAcceptsPatientsAsYoungAs').value;
            if (AcceptsPatientsAsYoungAs != "")
                APIURLParms += "&AcceptsPatientsAsYoungAs=" + AcceptsPatientsAsYoungAs;

            var AcceptsPatientsAsOldAs = "";
            AcceptsPatientsAsOldAs = document.getElementById('ctl00_MainContent_txtAcceptsPatientsAsOldAs').value;
            if (AcceptsPatientsAsOldAs != "")
                APIURLParms += "&AcceptsPatientsAsOldAs=" + AcceptsPatientsAsOldAs;

            var AcceptsPatientsofGender = "";
            AcceptsPatientsofGender = document.getElementById('ctl00_MainContent_ddlAcceptsPatientsofGender').value;
            if (AcceptsPatientsofGender != "")
                APIURLParms += "&AcceptsPatientsofGender=" + AcceptsPatientsofGender;

            var AcceptsNewPatients = "";
            AcceptsNewPatients = document.getElementById('ctl00_MainContent_ddlAcceptsNewPatients').value;
            if (AcceptsNewPatients != "")
                APIURLParms += "&AcceptsNewPatients=" + AcceptsNewPatients;

            var AcceptsNewborns = "";
            AcceptsNewborns = document.getElementById('ctl00_MainContent_ddlAcceptsNewborns').value;
            if (AcceptsNewborns != "")
                APIURLParms += "&AcceptsNewborns=" + AcceptsNewborns;

            var AcceptsPregnantWomen = "";
            AcceptsPregnantWomen = document.getElementById('ctl00_MainContent_ddlAcceptsPregnantWomen').value;
            if (AcceptsPregnantWomen != "")
                APIURLParms += "&AcceptsPregnantWomen=" + AcceptsPregnantWomen;

            var CountySelectedValues = "";
            var combo = $find("<%=rcbCounty.ClientID %>");
            var CountyCollection = combo.get_checkedItems();
            if (CountyCollection.length != 0) {
                for (var i = 0; i < CountyCollection.length; i++) {
                    CountySelectedValues += CountyCollection[i].get_value() + ",";
                }
                CountySelectedValues = CountySelectedValues.replace(/,(?=[^,]*$)/, "");
            }
            if (CountySelectedValues != "")
                APIURLParms += "&County=" + CountySelectedValues;

            var City = "";
            City = document.getElementById('ctl00_MainContent_txtCity').value;
            if (City != "")
                APIURLParms += "&City=" + City;

            var ProviderGender = "";
            ProviderGender = document.getElementById('ctl00_MainContent_ddlProviderGender').value;
            if (ProviderGender != "")
                APIURLParms += "&ProviderGender=" + ProviderGender;

            var HospitalAffiliationSelectedValues = "";
            var combo = $find("<%=rcbHospitalAffiliation.ClientID %>");
            var HospitalAffiliationCollection = combo.get_checkedItems();
            if (HospitalAffiliationCollection.length != 0) {
                for (var i = 0; i < HospitalAffiliationCollection.length; i++) {
                    HospitalAffiliationSelectedValues += HospitalAffiliationCollection[i].get_value() + ",";
                }
                HospitalAffiliationSelectedValues = HospitalAffiliationSelectedValues.replace(/,(?=[^,]*$)/, "");
            }
            if (HospitalAffiliationSelectedValues != "")
                APIURLParms += "&HospitalAffiliation=" + HospitalAffiliationSelectedValues;

            var LanguagesSpokenListSelectedValues = "";
            var combo = $find("<%=rcbLanguagesSpoken.ClientID %>");
            var LanguagesSpokenListCollection = combo.get_checkedItems();
            if (LanguagesSpokenListCollection.length != 0) {
                for (var i = 0; i < LanguagesSpokenListCollection.length; i++) {
                    LanguagesSpokenListSelectedValues += LanguagesSpokenListCollection[i].get_value() + ",";
                }
                LanguagesSpokenListSelectedValues = LanguagesSpokenListSelectedValues.replace(/,(?=[^,]*$)/, "");
            }
            if (LanguagesSpokenListSelectedValues != "")
                APIURLParms += "&LanguagesSpokenList=" + LanguagesSpokenListSelectedValues;

            var SpecializedTrainingSelectedValues = "";
            var combo = $find("<%=rcbSpecializedTraining.ClientID %>");
            var SpecializedTrainingCollection = combo.get_checkedItems();
            if (SpecializedTrainingCollection.length != 0) {
                for (var i = 0; i < SpecializedTrainingCollection.length; i++) {
                    SpecializedTrainingSelectedValues += SpecializedTrainingCollection[i].get_value() + ",";
                }
                SpecializedTrainingSelectedValues = SpecializedTrainingSelectedValues.replace(/,(?=[^,]*$)/, "");
            }
            if (SpecializedTrainingSelectedValues != "")
                APIURLParms += "&SpecializedTraining=" + SpecializedTrainingSelectedValues;

            var CulturalCompetenciesSelectedValues = "";
            var combo = $find("<%=rcbCulturalCompetencies.ClientID %>");
            var CulturalCompetenciesCollection = combo.get_checkedItems();
            if (CulturalCompetenciesCollection.length != 0) {
                for (var i = 0; i < CulturalCompetenciesCollection.length; i++) {
                    CulturalCompetenciesSelectedValues += CulturalCompetenciesCollection[i].get_value() + ",";
                }
                CulturalCompetenciesSelectedValues = CulturalCompetenciesSelectedValues.replace(/,(?=[^,]*$)/, "");
            }
            if (CulturalCompetenciesSelectedValues != "")
                APIURLParms += "&CulturalCompetencies=" + CulturalCompetenciesSelectedValues;

            var ADAAccommodationsSelectedValues = "";
            var combo = $find("<%=rcbADAAccommodations.ClientID %>");
            var ADAAccommodationsCollection = combo.get_checkedItems();
            if (ADAAccommodationsCollection.length != 0) {
                for (var i = 0; i < ADAAccommodationsCollection.length; i++) {
                    ADAAccommodationsSelectedValues += ADAAccommodationsCollection[i].get_value() + ",";
                }
                ADAAccommodationsSelectedValues = ADAAccommodationsSelectedValues.replace(/,(?=[^,]*$)/, "");
            }
            if (ADAAccommodationsSelectedValues != "")
                APIURLParms += "&ADAAccommodations=" + ADAAccommodationsSelectedValues;

            var BoardCertificationsSelectedValues = "";
            var combo = $find("<%=rcbBoardCertifications.ClientID %>");
            var BoardCertificationsCollection = combo.get_checkedItems();
            if (BoardCertificationsCollection.length != 0) {
                for (var i = 0; i < BoardCertificationsCollection.length; i++) {
                    BoardCertificationsSelectedValues += BoardCertificationsCollection[i].get_value() + ",";
                }
                BoardCertificationsSelectedValues = BoardCertificationsSelectedValues.replace(/,(?=[^,]*$)/, "");
            }
            if (BoardCertificationsSelectedValues != "")
                APIURLParms += "&BoardCertifications=" + BoardCertificationsSelectedValues;

            var Telehealth = "";
            Telehealth = document.getElementById('ctl00_MainContent_ddlTelehealth').value;
            if (Telehealth != "")
                APIURLParms += "&Telehealth=" + Telehealth;

            var CHIP = "";
            CHIP = document.getElementById('ctl00_MainContent_ddlCHIP').value;
            if (CHIP != "")
                APIURLParms += "&CHIP=" + CHIP;

            var NewMedicaid = "";
            NewMedicaid = document.getElementById('ctl00_MainContent_ddlNewMedicaid').value;
            if (NewMedicaid != "")
                APIURLParms += "&NewMedicaid=" + NewMedicaid;


            getPublicSearchResult(APIURLParms)

        }
        function getPublicSearchResult(APIURLParms) {
            //$.ajax({
            //    type: 'GET',
            //    headers: {
            //        "Access-Control-Allow-Origin": "*",
            //        "Access-Control-Allow-Methods": "POST, GET, PUT, OPTIONS",
            //        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With"
            //    },
            //    url: APIURLParms,
            //    async: false,
            //    contentType: "application/fhir+json; charset=utf-8",
            //    dataType: "json",
            //    success: function (result) {
                    document.getElementById('ctl00_MainContent_txtresponse').value = APIURLParms;//JSON.stringify(result)

            //    },
            //    error: function (error) {
            //        console.log(error);
            //        alert('please retry');
            //    },
            //});
        }
        function handleStateChange() {
            var element = $find("<%= rcbCounty.ClientID %>");
            if (document.getElementById('ctl00_MainContent_ddlState').value == 'OH') {
                element.enable();
            } else {
                element.disable();
            }

            if (document.getElementById('ctl00_MainContent_ddlState').selectedIndex == 0) {
                document.getElementById('ctl00_MainContent_rfvddlState').style.display = 'inline';
            } else {
                document.getElementById('ctl00_MainContent_rfvddlState').style.display = 'hidden';
            }
        }

        function scrollToElement(elementId) {
            var element = document.getElementById(elementId);
            if (element) {
                element.scrollIntoView({ behavior: 'smooth', block: 'start' });
            } else {
                console.error('Element not found: ' + elementId);
            }
        }


        function OnClientFocusHandler(sender, eventArgs) {
            if (!sender.get_dropDownVisible()) {
                sender.showDropDown();
            }
        }

        $(document).ready(function () {
            var $table = $(".RadComboBox").find("table");
            $table.removeAttr("summary");
            $table.attr("role", "presentation");

        })
    </script>

    <div class="WhiteBox">
        <div id="divPublicSearchDemo">
            <div id="divPublicSearchG1G2Demo" class="col-lg-12">

                <div class="row">
                    <div class="col-sm-12 text-left">
                        <span class="formLabelSmall" style="color: #545487; font-weight: 700;">Application Programming Interface (API)</span>
                        <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 text-left">
                        <span class="pg-hint3Center">The API is a new and faster alternative to the Provider Directory search. It allows vendors to access public data from the PNM Provider Directory in real time. The API retrieves data from PNM daily. The demo function allows for testing and generating valid queries.</span>
                        
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 text-left">
                        <span class="pg-hint3Center">The Demo function will allow for testing and receiving valid queries.</span><br />
                        <br/>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-12 text-left">
                        <span class="formLabelSmall" style="color: #545487; font-weight: 700;">Demo API and Live API</span>
                        <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                        <br />
                        <table>
                            <tr>
                                <th><b>API Version</b></th>
                                <th><b>Interactive Demo</b></th>
                                <th><b>Planned Phase Out Date</b></th>
                                <th><b>API Link</b></th>
                            </tr>
                            <tr>
                                <td>Version 1.0</td>
                                <td><a id="demolink" text="Demo API" onclick="scrollToElement('divdemoAPI')" href="#">Demo API</a> </td>
                                <td>
                                    <asp:Label ID="lblPlannedPhaseOutDate" runat="server"></asp:Label>
                                </td>
                                <td><a id="apilink" text="API" href="" runat="server" target="_blank">API</a></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 text-left">
                        <br/>
                        <span class="formLabelSmall" style="color: #545487; font-weight: 700;">API Request Parameters</span>
                        <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                        <span class="pg-hint3Center">The Demo and API link contain field descriptions and information to ease entry.</span><br />
                        <span class="pg-hint3Center" id="spnexample" runat="server"><i></i></span>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 text-left">
                        <br/>
                        <span class="formLabelSmall" style="color: #545487; font-weight: 700;">Request Parameters</span>
                        <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                        
                        <table style="border-block-color: black">
                            <tr>
                                <th><b>Data Element</b></th>
                                <th><b>Type</b></th>
                                <th><b>Description</b></th>
                                <th><b>Field Name</b></th>
                            </tr>
                            <tr>
                                <td>ProviderTypeIDsDelimited </td>
                                <td>string </td>
                                <td>Dropdown multi-select list of provider types, including "Select All." Refer to the Provider Directory Search interface for examples. Output will be the MMIS PT Code.</td>
                                <td>Provider Type </td>
                            </tr>
                            <tr>
                                <td>SpecialtyTypeIDsDelimited </td>
                                <td>string </td>
                                <td>Drop down multi-select list of Provider Specialties including Select All. See user interface Provider Directory Search for example. Output will be MMIS PS Code. </td>
                                <td>Specialty </td>
                            </tr>
                            <tr>
                                <td>City </td>
                                <td>string </td>
                                <td>Name of the city </td>
                                <td>City </td>
                            </tr>
                            <tr>
                                <td>State </td>
                                <td>string </td>
                                <td>Name of the state in 2 letter code </td>
                                <td>State </td>
                            </tr>
                            <tr>
                                <td>County </td>
                                <td>string </td>
                                <td>Name of the Ohio county if state is equal to OH. Drop down multi-select list of Ohio Counties including Select All. See user interface Provider Directory Search for example. Output will be County Code. </td>
                                <td>County </td>
                            </tr>
                            <tr>
                                <td>Zip </td>
                                <td>string </td>
                                <td>ZIP code </td>
                                <td>Zip </td>
                            </tr>
                            <tr>
                                <td>Radius</td>
                                <td>string </td>
                                <td>Dropdown of defined radius parameters including Select All. See user interface Provider Directory Search for example. </td>
                                <td>Radius </td>
                            </tr>
                            <tr>
                                <td>OrgName </td>
                                <td>string </td>
                                <td>Name of the provider. If not an exact match, the search will return results that contain the input string.</td>
                                <td>Provider Name </td>
                            </tr>

                            <tr>
                                <td>HealthPlan </td>
                                <td>string </td>
                                <td>Drop down multi-select list of defined Health Plan including Select All. See user interface Provider Directory Search for example. </td>
                                <td>Health Plan </td>
                            </tr>
                            <tr>
                                <td>Program </td>
                                <td>string </td>
                                <td>Drop down multi-select list of defined Program including Select All. See user interface Provider Directory Search for example. </td>
                                <td>Program </td>
                            </tr>
                            <tr>
                                <td>FacilityType </td>
                                <td>string </td>
                                <td>Dropdown multi-select list of defined facility types, including "Select All." Refer to the Provider Directory Search interface for examples.</td>
                                <td>Facility Type </td>
                            </tr>
                            <tr>
                                <td>PrimaryCareProviders </td>
                                <td>string </td>
                                <td>Indicates if the provider is a primary care provider. </td>
                                <td>PCP </td>
                            </tr>

                            <tr>
                                <td>AcceptsPatientsofGender </td>
                                <td>bool </td>
                                <td>Dropdown multi-select list of defined patient genders, including "Select All." Refer to the Provider Directory Search interface for examples.</td>
                                <td>Patient Gender </td>
                            </tr>
                            <tr>
                                <td>AcceptsNewPatients </td>
                                <td>bool </td>
                                <td>Indicates whether the provider is accepting new patients.</td>
                                <td>New Patients </td>
                            </tr>

                            <tr>
                                <td>AcceptsNewborns </td>
                                <td>bool </td>
                                <td>Indicates if the provider accepts newborns </td>
                                <td>Newborn </td>
                            </tr>

                            <tr>
                                <td>AcceptsPregnantWomen </td>
                                <td>bool </td>
                                <td>Indicates if the provider accepts pregnant women. </td>
                                <td>Pregnant Women </td>
                            </tr>

                            <tr>
                                <td>ProviderGender </td>
                                <td>string </td>
                                <td>Dropdown multi-select list of defined provider genders, including "Select All." Refer to the Provider Directory Search interface for examples.</td>
                                <td>Provider Gender </td>
                            </tr>


                            <tr>
                                <td>SpecializedTraining </td>
                                <td>string </td>
                                <td>Drop down multi-select list of defined Specialized Training including Select All. See user interface Provider Directory Search for example. </td>
                                <td>Specialized Training </td>
                            </tr>
                            <tr>
                                <td>CulturalCompetencies </td>
                                <td>string </td>
                                <td>Drop down multi-select list of defined Cultural Competencies including Select All. See user interface Provider Directory Search for example. </td>
                                <td>Cultural Competencies </td>
                            </tr>
                            <tr>
                                <td>ADAAccommodations </td>
                                <td>string </td>
                                <td>Drop down multi-select list of defined ADA Accommodations including Select All. See user interface Provider Directory Search for example. </td>
                                <td>ADA  </td>
                            </tr>
                            <tr>
                                <td>Telehealth </td>
                                <td>bool </td>
                                <td>Indicates if the provider offers telehealth services </td>
                                <td>Telehealth </td>
                            </tr>

                            <tr>
                                <td>Pagination </td>
                                <td>int </td>
                                <td>Number of records to retrieve. Default is 10; maximum allowed is 100.</td>
                                <td>Number of Records </td>
                            </tr>
                            <tr>
                                <td>Index </td>
                                <td>int </td>
                                <td>Records to be returned, for example: 1-100, 101-200. If nothing is entered, first 100 records will return. </td>
                                <td>Index </td>
                            </tr>


                        </table>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-12 text-left">
                        <br />
                        <span class="formLabelSmall" style="color: #545487; font-weight: 700;">API Response Parameters</span>
                        <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                        
                        <table style="border-block-color: black">
                            <tr>
                                <th><b>Data Element</b></th>
                                <th><b>Type</b></th>
                                <th><b>Description</b></th>
                                <th><b>Field Name</b></th>
                            </tr>
                            <tr>
                                <td>Provider Name </td>
                                <td>string </td>
                                <td>Name of the provider. </td>
                                <td>Name </td>
                            </tr>
                            <tr>
                                <td>Provider Type </td>
                                <td>string </td>
                                <td>Type of provider. </td>
                                <td>PROVIDER_TYPE_ABBREVIATION </td>
                            </tr>
                            <tr>
                                <td>Specialty </td>
                                <td>string </td>
                                <td>Specialties of the provider, separated by comma. </td>
                                <td>SpecialtyTypeName </td>
                            </tr>
                            <tr>
                                <td>Address 1 </td>
                                <td>string </td>
                                <td>Primary service address, line 1. </td>
                                <td>ADDRESS_1 </td>
                            </tr>
                            <tr>
                                <td>Address 2 </td>
                                <td>string </td>
                                <td>Primary service address, line 2. </td>
                                <td>ADDRESS_2 </td>
                            </tr>
                            <tr>
                                <td>City </td>
                                <td>string </td>
                                <td>Primary service city. </td>
                                <td>ContactCity </td>
                            </tr>
                            <tr>
                                <td>State </td>
                                <td>string </td>
                                <td>Primary service state. </td>
                                <td>ContactState </td>
                            </tr>
                            <tr>
                                <td>Zip </td>
                                <td>string </td>
                                <td>Primary service ZIP code. </td>
                                <td>ContactZip </td>
                            </tr>
                            <tr>
                                <td>County </td>
                                <td>string </td>
                                <td>County code. </td>
                                <td>County </td>
                            </tr>
                            <tr>
                                <td>Phone </td>
                                <td>string </td>
                                <td>Primary service phone number. </td>
                                <td>Phone </td>
                            </tr>
                            <tr>
                                <td>Website </td>
                                <td>string </td>
                                <td>Primary service website URL. </td>
                                <td>Website </td>
                            </tr>
                            <tr>
                                <td>Affiliated Health Plans </td>
                                <td>string </td>
                                <td>Health plans accepted by the provider, separated by commas. </td>
                                <td>HealthPlans </td>
                            </tr>
                            <tr>
                                <td>Affiliated Programs </td>
                                <td>string </td>
                                <td>Programs in which the provider participates, separated by commas. </td>
                                <td>Program </td>
                            </tr>
                            <tr>
                                <td>Index </td>
                                <td>int </td>
                                <td>Range of records returned, for example: 1–100 or 101–200.  </td>
                                <td>Index </td>
                            </tr>
                            <tr>
                                <td>Total Count </td>
                                <td>int </td>
                                <td>Total number of records available on the server. </td>
                                <td>Total Count </td>
                            </tr>

                        </table>
                    </div>

                </div>
                <div id="divdemoAPI" class="row">
                    <div class="col-sm-12 text-left">
                        <br />
                        <span class="formLabelSmall" style="color: #545487; font-weight: 700;">Demo API</span>
                        <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                        <span class="pg-hint3Center">Use the following form to test generating valid queries. After selecting Submit, the query will display below.</span><br /><br />
                    </div>
                </div>
            </div>


        </div>
    </div>
    <cc1:groupbox id="gbSearch" horizontalalign="Center" width="98%" caption="<span style='display:none'>Public Search</span>" runat="server">
        <asp:Label runat="server" ID="lblMessages" CssClass="error-message" />
        <div>
            <%--<asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="PublicSearch" ShowSummary="true" />--%>
        </div>
        <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">
            <asp:UpdateProgress ID="updateProgress" runat="server">
                <progresstemplate>
                    <div style="padding-right: 30px">
                        <img src="../Images/ajax-loader.gif" alt="" />
                        Loading ...
                    </div>
                </progresstemplate>
            </asp:UpdateProgress>
            <div id="divPublicSearch">
                <div id="divPublicSearchG1G2" class="col-sm-12 col-md-12">
                    <div id="divPublicSearchGroup1" class="col-sm-12 col-md-12 col-lg-12">

                        <div class="row">
                            <!--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblHealthPlan" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlHealthPlan">Health Plan <span class="RedAsterisk">*</span></asp:Label>
                                </div>-->
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblHealthPlanelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlHealthPlan">HealthPlan <span class="RedAsterisk">*</span></asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:DropDownList ID="ddlHealthPlan" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Health Plan" />
                                <asp:RequiredFieldValidator ID="rfvddlHealthPlan"
                                    InitialValue="-1"
                                    ValidationGroup="PublicSearch"
                                    runat="server"
                                    ControlToValidate="ddlHealthPlan"
                                    ErrorMessage="* Health Plan is required"
                                    Display="Dynamic" CssClass="redAsterisk" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblProgram" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlProgram">Program</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblProgramelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlProgram">Program</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:DropDownList ID="ddlProgram" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Program" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblProviderType" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbProviderType">Provider Type</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblProviderTypeelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbProviderType">ProviderTypeIDsDelimited
                                        <span class="RedAsterisk">*</span>
                                </asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <telerik:radcombobox rendermode="Classic"  borderwidth="1px" id="rcbProviderType" runat="server" checkboxes="true" allowcustomtext="true" tooltip="Provider Type"
                                    enablecheckallitemscheckbox="false" filter="Contains" backcolor="White" onitemchecked="rcbProviderType_ItemChecked" autopostback="false"
                                    skin="PDMSModern" cssclass="unsetPublicSearchRCBLength RadComboBorder" onclientfocus="OnClientFocusHandler"
                                    width="100%" >
                                </telerik:radcombobox>
                                <asp:RequiredFieldValidator
                                    ID="rfvRcbProviderType"
                                    runat="server"
                                    ControlToValidate="rcbProviderType"
                                    ErrorMessage="* Please select at least one Provider Type."
                                    ValidationGroup="PublicSearch"
                                    Display="Dynamic" CssClass="redAsterisk" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblFacilityType" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlFacilityType">Facility Type</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblFacilityTypeelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlFacilityType">FacilityType</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:DropDownList ID="ddlFacilityType" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Facility Type" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblPrimaryCareProvidersOnly" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlPrimaryCareProviders">Primary Care Providers Only</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblPrimaryCareProvidersOnlyelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlPrimaryCareProviders">PrimaryCareProviders</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:DropDownList ID="ddlPrimaryCareProviders" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Primary Care Providers Only" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblProviderNameFullPath" runat="server" CssClass="publicSearchLabels" AssociatedControlID="txtOrgName">Provider Name (Full or Partial)</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblProviderNameFullPathelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="txtOrgName">OrgName</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                
                                    <asp:TextBox ID="txtOrgName" class="form-control" aria-label="..." runat="server" />
                                    <%--                                        <span class="input-group-btn">
                                            <asp:DropDownList ID="ddlOrgNameSearch" Style="min-width: 100px; margin: 0px; padding: 0px" CssClass="btn btn-default active" runat="server" ToolTip="Provider Name (Full or Partial)">
                                                <asp:ListItem Value="equals" Text="Equal to" Selected="True" />
                                                <asp:ListItem Value="begins" Text="Begins with" />
                                                <asp:ListItem Value="contains" Text="Contains" />
                                                <asp:ListItem Value="ends" Text="Ends with" />
                                            </asp:DropDownList>
                                        </span>--%>
                                
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblDMEProductsServices" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbDMEProductsServices">DME Products & Services</asp:Label>

                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblDMEProductsServiceselement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbDMEProductsServices">DMEProductsServices</asp:Label>

                            </div>
                            <div class="col-sm-4 text-left">
                                <telerik:radcombobox rendermode="Classic"  borderwidth="1px" id="rcbDMEProductsServices" runat="server" checkboxes="true" allowcustomtext="true" tooltip="DME Products & Services"
                                    enablecheckallitemscheckbox="true" skin="PDMSModern" cssclass="unsetPublicSearchRCBLength RadComboBorder" backcolor="White"
                                    width="100%" onclientfocus="OnClientFocusHandler">
                                </telerik:radcombobox>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblAcceptsPatientsAsYoungAs" runat="server" CssClass="publicSearchLabels" AssociatedControlID="txtAcceptsPatientsAsYoungAs">Accepts Patients As Young As</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblAcceptsPatientsAsYoungAselement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="txtAcceptsPatientsAsYoungAs">AcceptsPatientsAsYoungAs</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:TextBox ID="txtAcceptsPatientsAsYoungAs" class="form-control" aria-label="..." runat="server" ToolTip="Accepts Patients As Young As" />
                                <asp:RegularExpressionValidator ID="revtxtAcceptsPatientsAsYoungAs" runat="server" ControlToValidate="txtAcceptsPatientsAsYoungAs"
                                    ValidationExpression="^[0-9]\d*$" ErrorMessage="* Enter a numeric value for Accepts Patients As Young As."
                                    Enabled="true" SetFocusOnError="true" Text=""
                                    ValidationGroup="PublicSearch" Display="None" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblAcceptsPatientsAsOldAs" runat="server" CssClass="publicSearchLabels" AssociatedControlID="txtAcceptsPatientsAsOldAs">Accepts Patients As Old As</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblAcceptsPatientsAsOldAselement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="txtAcceptsPatientsAsOldAs">AcceptsPatientsAsOldAs</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:TextBox ID="txtAcceptsPatientsAsOldAs" class="form-control" aria-label="..." runat="server" CausesValidation="true" ValidationGroup="PublicSearch" ToolTip="Accepts Patients As Old As" />
                                <asp:RegularExpressionValidator ID="revtxtAcceptsPatientsAsOldAs" runat="server" ControlToValidate="txtAcceptsPatientsAsOldAs"
                                    ValidationExpression="^[0-9]\d*$" ErrorMessage="* Enter a numeric value for Accepts Patients As Old As."
                                    Enabled="true" SetFocusOnError="true" Text=""
                                    ValidationGroup="PublicSearch" Display="None" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblAcceptsPatientsofGender" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlAcceptsPatientsofGender">Accepts Patients of Gender</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblAcceptsPatientsofGenderelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlAcceptsPatientsofGender">AcceptsPatientsofGender</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:DropDownList ID="ddlAcceptsPatientsofGender" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Accepts Patients of Gender" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblAcceptsNewPatients" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlAcceptsNewPatients">Accepts New Patients</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblAcceptsNewPatientselement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlAcceptsNewPatients">AcceptsNewPatients</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:DropDownList ID="ddlAcceptsNewPatients" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Accepts New Patients" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblAcceptsNewborns" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlAcceptsNewborns">Accepts Newborns</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblAcceptsNewbornselement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlAcceptsNewborns">AcceptsNewborns</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:DropDownList ID="ddlAcceptsNewborns" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Accepts Newborns" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblAcceptsPregnantWomen" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlAcceptsPregnantWomen">Accepts Pregnant Women</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblAcceptsPregnantWomenelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlAcceptsPregnantWomen">AcceptsPregnantWomen</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:DropDownList ID="ddlAcceptsPregnantWomen" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Accepts Pregnant Women" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblrcbCounty" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbCounty">County</asp:Label>  <asp:RequiredFieldValidator ID="rfvrcbCounty" runat="server" ControlToValidate="rcbCounty" InitialValue="*" ErrorMessage="County is required" Display="Dynamic" />
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblrcbCountyelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbCounty">County</asp:Label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rcbCounty" InitialValue="*" ErrorMessage="County is required" Display="Dynamic" CssClass="redAsterisk" />
                            </div>
                            <div class="col-sm-4 text-left">
                                <telerik:radcombobox rendermode="Classic"  borderwidth="1px" id="rcbCounty" runat="server" checkboxes="true" allowcustomtext="false" tooltip="County"
                                    enablecheckallitemscheckbox="true" filter="Contains" backcolor="White" autopostback="false"
                                    skin="PDMSModern" cssclass="unsetPublicSearchRCBLength RadComboBorder" onclientfocus="OnClientFocusHandler"
                                    width="100%">
                                </telerik:radcombobox>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lbltxtCity" runat="server" CssClass="publicSearchLabels" AssociatedControlID="txtCity">City</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lbltxtCityelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="txtCity">City</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                
                                    <asp:TextBox ID="txtCity" class="form-control" aria-label="..." runat="server" ToolTip="City" />
                                    <asp:RegularExpressionValidator ID="revtxtCity" runat="server" ControlToValidate="txtCity"
                                        ValidationExpression="^[a-zA-Z]+(?:[\s-][a-zA-Z]+)*$" ErrorMessage="* Enter only aphabetic characters for City."
                                        Enabled="true" SetFocusOnError="true" Text=""
                                        ValidationGroup="PublicSearch" Display="None" />
                                    <%--                                        <span class="input-group-btn" style="padding-right: 0px; margin-left: 0px">

                                            <asp:DropDownList ID="ddlCitySearch" Style="min-width: 100px; margin: 0px; padding: 0px" CssClass="btn btn-default active" runat="server" ToolTip="City Search Type" >
                                                <asp:ListItem Value="equals" Text="Equal to" Selected="True" />
                                                <asp:ListItem Value="begins" Text="Begins with" />
                                                <asp:ListItem Value="contains" Text="Contains" />
                                                <asp:ListItem Value="ends" Text="Ends with" />
                                            </asp:DropDownList>
                                        </span>--%>
                                
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlState" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlState">State <span class="RedAsterisk">*</span></asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblddlStateelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlState">State <span class="RedAsterisk">*</span></asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:DropDownList ID="ddlState" runat="server" OnSelectedIndexChanged="SelectCounty_OnSelectedStateIndexChanged" ToolTip="State"
                                    CssClass="form-control unsetPublicSearchDDLLength">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator
                                    ID="rfvddlState" runat="server"
                                    ControlToValidate="ddlState"
                                    InitialValue="*"
                                    ErrorMessage="State is required."
                                    Enabled="true"
                                    SetFocusOnError="true"
                                    ValidationGroup="PublicSearch"
                                    Display="Dynamic" CssClass="redAsterisk" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lbltxtZip" runat="server" CssClass="publicSearchLabels" AssociatedControlID="txtZip">
                                     
                                        <span tabindex="0" class="publicSearchLabels">Zip Code <span class="ohio-tooltip" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_DIRECTORY_POPUP_ZIP' runat='server' Text='Radius searches are only available if a Zip Code is entered' />" ><asp:Image ID="imgInfoIcon" AlternateText="INFO" Height="13" Width="13" runat="server" ImageUrl="~/Images/Infoicon.png" /></span></span>


                                    </asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lbltxtZipelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="txtZip">Zip</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" ToolTip="Zip Code" />
                                <asp:RegularExpressionValidator ID="revtxtZip" runat="server" ControlToValidate="txtZip"
                                    ValidationExpression="^[0-9]\d*$" ErrorMessage="* Enter a numeric value for ZipCode."
                                    Enabled="true" SetFocusOnError="true" Text=""
                                    ValidationGroup="PublicSearch" Display="Dynamic" />
                                <asp:RequiredFieldValidator
                                    ID="rfvtxtZip"
                                    runat="server"
                                    ControlToValidate="txtZip"
                                    InitialValue=""
                                    ErrorMessage="You must enter a County, City or Zip Code to complete the search."
                                    Enabled="true" S
                                    etFocusOnError="true"
                                    ValidationGroup="PublicSearch"
                                    Display="Dynamic" CssClass="redAsterisk" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlRadiusMiles" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlRadiusMiles">Radius (Miles) <span class="RedAsterisk">*</span></asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblddlRadiusMileselement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlRadiusMiles">Radius<span class="RedAsterisk">*</span></asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:DropDownList ID="ddlRadiusMiles" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Radius (Miles)" />
                                <asp:RequiredFieldValidator
                                    ID="rfvddlRadiusMiles"
                                    runat="server"
                                    ControlToValidate="ddlRadiusMiles"
                                    InitialValue="*"
                                    ErrorMessage="* RadiusMiles is required when Zip is provided."
                                    Enabled="true"
                                    SetFocusOnError="true"
                                    ValidationGroup="PublicSearch"
                                    Display="Dynamic" CssClass="redAsterisk" />
                            </div>
                        </div>

                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlProviderSpeciality" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbProviderSpeciality">Provider Speciality</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblddlProviderSpecialityelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbProviderSpeciality">SpecialtyTypeIDsDelimited</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <telerik:radcombobox rendermode="Classic"  borderwidth="1px" id="rcbProviderSpeciality" runat="server" checkboxes="true" allowcustomtext="true" tooltip="Provider Speciality"
                                    enablecheckallitemscheckbox="true" skin="PDMSModern" cssclass="unsetPublicSearchRCBLength RadComboBorder" filter="Contains" backcolor="White"
                                    width="100%" >
                                </telerik:radcombobox>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlProviderGender" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlProviderGender">Provider Gender</asp:Label>

                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblddlProviderGenderelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlProviderGender">Gender</asp:Label>

                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:DropDownList ID="ddlProviderGender" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Provider Gender" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlHospitalAffiliation" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbHospitalAffiliation">Hospital Affiliation</asp:Label>

                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblddlHospitalAffiliationelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbHospitalAffiliation">HospitalAffiliation</asp:Label>

                            </div>
                            <div class="col-sm-4 text-left">
                                <telerik:radcombobox rendermode="Classic"  borderwidth="1px" id="rcbHospitalAffiliation" runat="server" checkboxes="true" allowcustomtext="true" tooltip="Hospital Affiliation"
                                    enablecheckallitemscheckbox="true" skin="PDMSModern" cssclass="unsetPublicSearchRCBLength RadComboBorder" filter="Contains" backcolor="White"
                                    width="100%" onclientfocus="OnClientFocusHandler" >
                                </telerik:radcombobox>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlLanguagesSpoken" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbLanguagesSpoken">Languages Spoken</asp:Label>

                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblddlLanguagesSpokenelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbLanguagesSpoken">LanguagesSpoken</asp:Label>

                            </div>
                            <div class="col-sm-4 text-left">
                                <telerik:radcombobox rendermode="Classic"  borderwidth="1px" id="rcbLanguagesSpoken" runat="server" checkboxes="true" allowcustomtext="true" tooltip="Languages Spoken"
                                    enablecheckallitemscheckbox="true" skin="PDMSModern" cssclass="unsetPublicSearchRCBLength RadComboBorder" filter="Contains" backcolor="White"
                                    width="100%" onclientfocus="OnClientFocusHandler" >
                                </telerik:radcombobox>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlSpecializedTraining" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbSpecializedTraining">Specialized Training</asp:Label>

                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblddlSpecializedTrainingelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbSpecializedTraining">SpecializedTraining</asp:Label>

                            </div>
                            <div class="col-sm-4 text-left">
                                <telerik:radcombobox rendermode="Classic"  borderwidth="1px" id="rcbSpecializedTraining" runat="server" checkboxes="true" allowcustomtext="true" tooltip="Specialized Training"
                                    enablecheckallitemscheckbox="true" skin="PDMSModern" cssclass="unsetPublicSearchRCBLength RadComboBorder" backcolor="White"
                                    width="100%" onclientfocus="OnClientFocusHandler">
                                </telerik:radcombobox>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlCulturalCompetencies" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbCulturalCompetencies">Cultural Competencies</asp:Label>

                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblddlCulturalCompetencieselement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbCulturalCompetencies">CulturalCompetencies</asp:Label>

                            </div>
                            <div class="col-sm-4 text-left">
                                <telerik:radcombobox rendermode="Classic"  borderwidth="1px" id="rcbCulturalCompetencies" runat="server" checkboxes="true" allowcustomtext="true" tooltip="Cultural Competencies"
                                    enablecheckallitemscheckbox="true" skin="PDMSModern" cssclass="unsetPublicSearchRCBLength RadComboBorder" backcolor="White"
                                    width="100%" onclientfocus="OnClientFocusHandler">
                                </telerik:radcombobox>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlADAAccommodations" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbADAAccommodations">ADA Accommodations</asp:Label>

                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblddlADAAccommodationselement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbADAAccommodations">ADAAccommodations</asp:Label>

                            </div>
                            <div class="col-sm-4 text-left">
                                <telerik:radcombobox rendermode="Classic"  borderwidth="1px" id="rcbADAAccommodations" runat="server" checkboxes="true" allowcustomtext="true" tooltip="ADA Accommodations"
                                    enablecheckallitemscheckbox="true" skin="PDMSModern" cssclass="unsetPublicSearchRCBLength RadComboBorder" backcolor="White"
                                    width="100%" onclientfocus="OnClientFocusHandler">
                                </telerik:radcombobox>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlBoardCertifications" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbBoardCertifications">Board Certifications</asp:Label>

                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblddlBoardCertificationselement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="rcbBoardCertifications">BoardCertifications</asp:Label>

                            </div>
                            <div class="col-sm-4 text-left">
                                <telerik:radcombobox rendermode="Classic"  borderwidth="1px" id="rcbBoardCertifications" runat="server" checkboxes="true" allowcustomtext="true" tooltip="Board Certifications"
                                    enablecheckallitemscheckbox="true" skin="PDMSModern" cssclass="unsetPublicSearchRCBLength RadComboBorder" backcolor="White"
                                    width="100%" onclientfocus="OnClientFocusHandler">
                                </telerik:radcombobox>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlTelehealth" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlTelehealth">Telehealth</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblddlTelehealthelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlTelehealth">Telehealth</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:DropDownList ID="ddlTelehealth" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Telehealth" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlCHIP" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlCHIP">CHIP</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblddlCHIPelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlCHIP">CHIP</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:DropDownList ID="ddlCHIP" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="CHIP" />
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-sm-4 text-right">
                                    <asp:Label ID="lblddlNewMedicaid" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlNewMedicaid">New Medicaid Patients</asp:Label>
                                </div>--%>
                            <div class="col-sm-4 text-right">
                                <asp:Label ID="lblddlNewMedicaidelement" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlNewMedicaid">NewMedicaidPatients</asp:Label>
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:DropDownList ID="ddlNewMedicaid" runat="server" CssClass="form-control unsetPublicSearchDDLLength" ToolTip="Accepts New Medicaid Patients" />
                                <br />
                            </div>
                        </div>
                    </div>

                </div>

            </div>
      
            <div class="row" style="text-align: center">
                <br />
                <br />
                
               <asp:Button ID="btnSearch" 
                    runat="server" 
                    Text="Submit" 
                    CssClass="buttonBoxFocus" 
                    OnClientClick="return CreateAPIURL();" 
                    CausesValidation="true" 
                    ValidationGroup="PublicSearch" 
                    ToolTip="Submit" style="margin-right:10px;" UseSubmitBehavior="false" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" ToolTip="Clear" />
                <%-- <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" />--%>
            </div>
            <br />
            <div class="row">
                <div class="col-sm-4 text-right" style="padding-top: 40px;">
                    <asp:Label ID="lblresponse" runat="server" CssClass="publicSearchLabels" AssociatedControlID="ddlNewMedicaid">Request URL with Parameters</asp:Label>
                </div>

                <div class="col-sm-8 text-right" style="text-align: left;">
                    <asp:TextBox ID="txtresponse" runat="server" TextMode="MultiLine" ToolTip="Request URL" style="height: 200px; width: 800px;" BorderStyle="Solid"  Columns="200" Rows="15"></asp:TextBox>
                </div>
            </div>
        </asp:Panel>
    </cc1:groupbox>
    <br />

    <cc2:messagebox id="MessageBox2" runat="server" />

    <ajax:modalpopupextender id="mpeReviewProvider" runat="server" popupcontrolid="pnlViewProvider" targetcontrolid="btnDummy"
        repositionmode="RepositionOnWindowScroll" backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnlViewProvider">
    </ajax:modalpopupextender>
    <asp:Panel ID="pnlViewProvider" runat="server" CssClass="modalPopup" align="center" Style="display: none; width: 55%; height: 80%;" ScrollBars="Auto">
        <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
            <div class="popTitle">Provider Information</div>
        </asp:Panel>
        <asp:Panel ID="pnlAdminMaintenance" runat="server" Style="margin-right: 10px; margin-left: 20px;">
            <asp:MultiView ID="mltViewProvider" runat="server" ActiveViewIndex="0" EnableViewState="true">
                <asp:View ID="vwProvider" runat="server">
                    <div>
                        <%--<asp:ValidationSummary ID="valExpressSummary" runat="server" DisplayMode="List" ValidationGroup="ExpressMaintSelection" ShowSummary="true" />--%>
                    </div>
                    <br />
                    <%-- <div style="text-align: left; padding: 15px; margin-left:30px !important" class="container-fluid">
                 <div class="row">--%>
                    <div id="divPublicSearchProviderInfoPopUp" class="row">
                        <div class="col-sm-10 text-left">
                            <div class="row">
                                <div class="col-sm-12 text-left">
                                    <span class="formLabel200">Provider Information</span>
                                    <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">
                                    <span class="formLabel wd150">Provider name</span>
                                </div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblProviderName" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">
                                    <span class="formLabel wd150">Address</span>
                                </div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblAddress" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right">
                                    <span class="formLabel wd150">City, State, Zip</span>
                                </div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblCityStateZip" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">County</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblCounty" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Specialty</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblSpecialty" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Accepting New Patients</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblAcceptingNewPatients" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Gender</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblGender" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Hospital Affiliation(s)</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblHospitalAffiliations" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Board Certification</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblBoardCertification" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-12 text-left">
                                    <br />
                                    <span class="formLabel200">Office Information</span>
                                    <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Monday</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblOfficeMon" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Tuesday</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblOfficeTue" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Wednesday</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblOfficeWed" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Thursday</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblOfficeThu" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Friday</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblOfficeFri" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Saturday</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblOfficeSat" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Sunday</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblOfficeSun" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Office Phone</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblOfficePhone" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-12 text-left">
                                    <br />
                                    <span class="formLabel200">Other Information</span>
                                    <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Cultural Competencies</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblCulturalCompetenciesLBL" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">ADA Accommodations</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblOfficeAccommodationsLBL" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Languages Spoken</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblLanguagesSpokenLBL" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">DME Products and Services</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblDMEProductsLBL" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Telehealth</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblTelehealthLBL" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">CHIP</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblCHIPLBL" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel wd150">Accepts New Medicaid Patients</span></div>
                                <div class="col-sm-9 text-left" style="white-space: pre-wrap;">
                                    <asp:Label ID="lblNewMedicaidLBL" runat="server" CssClass="formLabelPublicSearch"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-2">
                            <asp:HyperLink ID="hlGoogleMaps" runat="server" Text="Get Directions" Target="_blank"></asp:HyperLink>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
        </asp:Panel>
        <div class="row text-center" style="padding-top: 20px; padding-right: 10px;">
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
        </div>
        <br />
        <br />
    </asp:Panel>
    <asp:Button runat="server" ID="btnDummy" Style="display: none" Text="btnDummy" />
    <asp:HiddenField ID="hdnpublicsearchapi" runat="server" />
    <script>
        $(document).ready(function () {
            // Get the button elementvar 
            button = $('.rcbActionButton'); // Set the name attribute
            button.attr('title', 'ProviderType'); // Output the button element with the name attribute console.log(button[0].outerHTML);
            button.attr('aria-label', 'Provider Type');
        });
    </script>

</asp:Content>

