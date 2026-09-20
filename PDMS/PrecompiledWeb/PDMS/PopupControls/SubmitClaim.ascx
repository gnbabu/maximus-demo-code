<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SubmitClaim, App_Web_43eentok" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>

<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="wc" %>
<%@ Register Src="~/PopupControls/PriorAuthDiagnosis.ascx" TagPrefix="uc" TagName="PriorAuthDiagnosis" %>
<%--<%@ Register Src="~/PopupControls/UploadDocument.ascx" TagName="UploadDocument" TagPrefix="uc" %>--%>
<%@ Register Src="~/PopupControls/ReviewerNotes.ascx" TagName="ReviewerNotes" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/CARCRARCInformation.ascx" TagName="CARCRARCInformation"
    TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/ClaimsXtenInformation.ascx" TagName="ClaimsXtenInformation"
    TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/RelatedICNScreen.ascx" TagName="RelatedICNScreen"
    TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/PriorAuthProvidersNotes.ascx" TagPrefix="uc" TagName="PriorAuthProvidersNotes" %>

<%@ Register Src="~/PopupControls/PriorAuthDocumentbyMail.ascx" TagPrefix="uc" TagName="PriorAuthDocumentbyMail" %>

<%@ Register Src="~/PopupControls/SubmitClaimAdditionalProviderInformation.ascx"
    TagPrefix="uc" TagName="SubmitClaimAdditionalProviderInformation" %>
<%@ Register Src="~/PopupControls/SubmitClaimNDCDetails.ascx" TagPrefix="uc" TagName="SubmitClaimNDCDetails" %>
<%@ Register Src="~/PopupControls/SubmitClaimOtherPayerPaidAmoutnAdjustment.ascx"
    TagPrefix="uc" TagName="SubmitClaimOtherPayerPaidAmoutnAdjustment" %>
<%@ Register Src="~/PopupControls/SubmitClaimClaimValueCode.ascx" TagPrefix="uc"
    TagName="SubmitClaimClaimValueCode" %>
<%@ Register Src="~/PopupControls/SubmitClaimConditionCode.ascx" TagPrefix="uc" TagName="SubmitClaimConditionCode" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchCCI.ascx" TagPrefix="uc" TagName="SubmitClaimSerchCCI" %>


<%@ Register Src="~/PopupControls/SubmitClaimOccurrenceInformation.ascx" TagPrefix="uc"
    TagName="SubmitClaimOccurrenceInformation" %>
<%@ Register Src="~/PopupControls/SubmitClaimOtherProviderInfo.ascx" TagPrefix="uc"
    TagName="SubmitClaimOtherProviderInfo" %>
<%@ Register Src="~/PopupControls/SubmitClaimICD10ProcedureCodes.ascx" TagPrefix="uc"
    TagName="SubmitClaimICD10ProcedureCodes" %>
<%@ Register Src="~/PopupControls/SubmitClaimOccurrenceSpanInformation.ascx" TagPrefix="uc"
    TagName="SubmitClaimOccurrenceSpanInformation" %>
<%@ Register TagPrefix="jk" Namespace="JK.BootstrapControls" %>

<%@ Register Src="~/PopupControls/SubmitClaimServiceDetails.ascx" TagPrefix="uc"
    TagName="SubmitClaimServiceDetails" %>
<%@ Register Src="~/PopupControls/AdjudicationError.ascx" TagName="AdjudicationErrors"
    TagPrefix="uc" %>

<%@ Register Src="~/PopupControls/ClaimProfessionalServiceDetails.ascx" TagPrefix="uc"
    TagName="ClaimProfessionalServiceDetails" %>
<%@ Register Src="~/PopupControls/ProfessionalServiceLineDetails.ascx" TagPrefix="uc"
    TagName="ProfessionalServiceLineDetails" %>
<%@ Register Src="~/PopupControls/PriorAuthDentalHSCPSCodeSearch.ascx" TagPrefix="uc"
    TagName="PriorAuthDentalHSCPSCodeSearch" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchPage.ascx" TagPrefix="uc" TagName="SubmitClaimSearchPage" %>
<%--<%@ Register Src="~/PopupControls/SubmitClaimSearchPop.ascx" TagPrefix="uc" TagName="SubmitClaimSearchPop" %>--%>
<%@ Register Src="~/PopupControls/ClaimAuthAttachment.ascx" TagPrefix="uc" TagName="ClaimAuthAttachment" %>
<%@ Register Src="~/PopupControls/PriorAuthDiagnosisSeach.ascx" TagPrefix="uc" TagName="PriorAuthDiagnosisSeach" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchReason.ascx" TagPrefix="uc" TagName="SubmitClaimSearchReason" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchNDC.ascx" TagPrefix="uc" TagName="SubmitClaimSearchNDC" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchProc.ascx" TagPrefix="uc" TagName="SubmitClaimSearchProc" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchBill.ascx" TagPrefix="uc" TagName="SubmitClaimSearchBill" %>
<%@ Register Src="~/PopupControls/DelaySubmission.ascx" TagPrefix="uc" TagName="DelaySubmission" %>
<%@ Register Src="~/PopupControls/AssistantSurgeon.ascx" TagPrefix="uc" TagName="AssistantSurgeon" %>
<%@ Register Src="~/PopupControls/ReferringProvider.ascx" TagPrefix="uc" TagName="ReferringProvider" %>
<%@ Register Src="~/PopupControls/OtherPayerPaidAmountServiceDetail.ascx" TagPrefix="uc"
    TagName="OtherPayerPaidAmountServiceDetail" %>
<%@ Register Src="~/PopupControls/OtherPayerAdjustmentServiceDetail.ascx" TagPrefix="uc"
    TagName="OtherPayerAdjustmentServiceDetail" %>
<%@ Register Src="~/PopupControls/RecipientInformationPanel.ascx" TagPrefix="uc"
    TagName="RecipientInformationPanel" %>
<%@ Register Src="~/PopupControls/RenderingProviderInformationPanel.ascx" TagPrefix="uc"
    TagName="RenderingProviderInformationPanel" %>
<%@ Register Src="~/PopupControls/PriorAuthorizationAndReferringPanel.ascx" TagPrefix="uc"
    TagName="PriorAuthorizationAndReferringPanel" %>
<%@ Register Src="~/PopupControls/SupervisingProviderPanel.ascx" TagPrefix="uc" TagName="SupervisingProviderPanel" %>
<%@ Register Src="~/PopupControls/DiagnosisCodePanel.ascx" TagPrefix="uc" TagName="DiagnosisCodePanel" %>
<%@ Register Src="~/PopupControls/AdditionalProviderInfoPanel.ascx" TagPrefix="uc"
    TagName="AdditionalProviderInfoPanel" %>
<%@ Register Src="~/PopupControls/ProviderNotes.ascx" TagPrefix="uc" TagName="ProviderNotes" %>
<%@ Register Src="~/PopupControls/ClaimsProviderBillingNotes.ascx" TagPrefix="uc" TagName="ProviderBillingNotes" %>
<%@ Register Src="~/PopupControls/AccidentInformation.ascx" TagName="AccidentInformation"
    TagPrefix="ucAccident" %>
<%@ Register Src="~/PopupControls/AmbulancePickupDropOff.ascx" TagName="AmbulancePickUpDropOff"
    TagPrefix="ucAmbulance" %>
<%@ Register Src="~/PopupControls/AmbulanceInformation.ascx" TagName="AmbulanceInformation"
    TagPrefix="ucAmbulance" %>
<%@ Register Src="~/PopupControls/ServiceFacilityInformation.ascx" TagName="ServiceFacility"
    TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/OtherPayerInformation.ascx" TagName="OtherPayerInformation"
    TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/ToothQuadrantInfo.ascx" TagPrefix="uc" TagName="ToothQuadrantInfo" %>
<%@ Register Src="~/PopupControls/ServiceInformationDental.ascx" TagPrefix="uc" TagName="ServiceInformationDental" %>
<%@ Register Src="~/PopupControls/ServiceInformationProfessional.ascx" TagPrefix="uc"
    TagName="ServiceInformationProfessional" %>
<%@ Register Src="~/PopupControls/ICDProcedureCode.ascx" TagPrefix="uc" TagName="SubmitClaimICDProcedureCode" %>
<%@ Register Src="~/PopupControls/ServiceInformationInstitutional.ascx" TagPrefix="uc"
    TagName="ServiceInformationInstitutional" %>
<%@ Register Src="~/PopupControls/HeaderOtherPayerAdjustmentMapping.ascx" TagPrefix="uc"
    TagName="SubmitClaimHeaderOtherPayerAdjustmentMapping" %>
<%@ Register Src="~/PopupControls/HeaderOtherPayerAdjustmentMappingProfessional.ascx"
    TagPrefix="uc" TagName="SubmitClaimHeaderOtherPayerAdjustmentMappingProfessional" %>
<%@ Register Src="~/PopupControls/HeaderOtherPayerAdjustmentMappingInstitutional.ascx"
    TagPrefix="uc" TagName="SubmitClaimHeaderOtherPayerAdjustmentMappingInstitutional" %>
<%@ Register Src="~/PopupControls/NDCDetails.ascx"
    TagPrefix="uc" TagName="NDCDetails" %>
<%@ Register Src="~/PopupControls/OutpatientAdjudicationInformation.ascx" TagPrefix="uc"
    TagName="OutpatientAdjudicationInformation" %>
<%@ Register Src="~/PopupControls/InPatientAdjudicationInformation.ascx" TagPrefix="uc"
    TagName="InPatientAdjudicationInformation" %>
<%@ Register Src="~/PopupControls/AttendingPhysicianInformation.ascx" TagPrefix="uc"
    TagName="AttendingPhysicianInformation" %>
<%@ Register Src="~/PopupControls/ServiceDetailsDental.ascx" TagPrefix="uc"
    TagName="ServiceDetailsDental" %>
<%@ Register Src="~/PopupControls/OccurenceSpanInformation.ascx" TagPrefix="uc"
    TagName="OccurenceSpanInformation" %>
<%@ Register Src="~/PopupControls/ConditionCodeInformation.ascx" TagPrefix="uc" TagName="ConditionCodeInformation" %>
<%@ Register Src="~/PopupControls/InstitutionalServiceDetails.ascx" TagPrefix="uc"
    TagName="InstitutionalServiceDetails" %>

<%@ Register Src="~/PopupControls/ValueCodeInformation.ascx" TagPrefix="uc"
    TagName="ValueCodeInformation" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Src="~/PopupControls/MaliciousAttachments.ascx" TagName="MaliciousAttachments" TagPrefix="uc" %>

<%--<link href="../Content/custom-style.css" rel="stylesheet" />

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<link href="<%# Page.ResolveClientUrl("~/App_Themes/Modernization/Modern.css") %>" rel="stylesheet" />--%>

<script src="../Scripts/claimslibrary.js"></script>
<script type="text/javascript"> 
   
    function storevalue() {
        var value = $("#<%=ddlDestinationPayerID.ClientID %> option:selected").text();

        $('#<%= payerID.ClientID %>').val(value);

        if ((value != null || value != "" || value != "undefined") && ($('#ctl00_MainContent_uc5SubmitClaim_txtstatus2').val() == "Pending Submission")) {

            document.getElementById("<%=hdnddlDestinationPayerID.ClientID %>").value = value;
            document.getElementById("<%=ddlDestinationPayerResponsibilitySequence.ClientID %>").disabled = false;
        }
        else {
            if ((value != null || value != "" || value != "undefined")) { document.getElementById("<%=hdnddlDestinationPayerID.ClientID %>").value = value; }
            document.getElementById("<%=ddlDestinationPayerResponsibilitySequence.ClientID %>").disabled = true;
        }

    }
    // Disable Browser Back buttons
    function DisableBackButton() {
        window.history.forward()
    }
    DisableBackButton();
    window.onload = DisableBackButton;
    window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
    window.onunload = function () { void (0) }


    function validatePatientControlNumber() {
        if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_txtPatientControlNumber') != null) {
            if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_txtPatientControlNumber').value == '') {
                document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_patientControlNumberError').innerHTML = '*Patient Control Number is required';
                document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_txtPatientControlNumber').focus();
                return false;
            }
            else {
                document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_patientControlNumberError').innerHTML = '';
                CallBlockUIPostBack();
                return true;
            }
        } else {
            return false;
        }
    }

    function clearTables() {

        localStorage.setItem("diagTable", "");
        localStorage.setItem("dentalServiceDetailTable", "");
        localStorage.setItem("OccSpanTable", "");
        localStorage.setItem("ValueTable", "");
        localStorage.setItem("ICDProcCodeTable", "");
        localStorage.setItem("instiServiceDetailTable", "");
        localStorage.setItem("ConditionTable", "");
        localStorage.setItem("ambulanceDropOffTable", "");
        localStorage.setItem("ToothQuadrantInfoTable", "");
        localStorage.setItem("providerNotesTable", "");
        localStorage.setItem("providerBillingNotesTable", "");
        localStorage.setItem("Professionalservicedetailtable", "");
        localStorage.setItem("HeaderAdjustmentTable", "");
        localStorage.setItem("otherPayerInfoTableDental", "");
        localStorage.setItem("OPPAServiceDetailTable", "");
        localStorage.setItem("OPPAServiceDetailTable", "");
        localStorage.setItem("AdditionalProviderInformationTable", "");
        localStorage.setItem("OccurrenceInfoTable", "");

    }
    function clrtbls() {
        var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();
        if (hdnclaimtype == "0") {
            $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_divDentlOP').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_providerDiagOutput').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_hdnClaimIdDiagnosis').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_providerHeaderAdjustmentdentalOutput').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_providerDentalServicedetailOutput').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_pnlToothQuadrantInfo').innerHTML = "";
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_hdnClaimId').val('');
        }
        if (hdnclaimtype == "1") {
            $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_divDentlOP').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_providerDiagOutput').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_hdnClaimIdDiagnosis').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_hdnValueCode_ClaimID').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucSSubmitClaimHeaderOtherPayerAdjustmentMappingInstitutional_providerHeaderAdjustmentinstiOutput').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ICDProcedureOutput').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_serviceDetailInsti').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_divNDCDetails').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_divAdditionalProviderInfo').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucValueCodeInformation_hdnValueCode_ClaimID').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimOccurrenceInformation1_divOccurrenceInfo').innerHTML = '';
        }
        if (hdnclaimtype == "2") {
            $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_divDentlOP').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_providerDiagOutput').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_hdnClaimIdDiagnosis').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_hdnValueCode_ClaimID').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_upProfServiceLineDetails1').innerHTML = ''
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_upProfServiceLineDetails1').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_divNDCDetails').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_divAdditionalProviderInfo').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMappingProfessional_providerHeaderAdjustmentinstiOutputProf').innerHTML = '';
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMappingProfessional_upnlOtherPayerAdjustmentInformation').innerHTML = '';
        }

    }

    function loadSearchDiagnososCode1() {

        document.getElementById('<%= txtDiagnosisCodeSearch.ClientID %>').disabled = true;
        document.getElementById('<%= ddlICDVersion.ClientID %>').disabled = true;
        document.getElementById('<%= txtDiagnosisCodeDescSearch.ClientID %>').disabled = true;
        document.getElementById('<%= btnloading1.ClientID %>').style.display = 'block';

    }

    function CallBlockUIPostBack() {
        BlockUIPostBack('ctl00_MainContent_pnlBillingAndotherservice');
    }

    $(document).ready(function () {
        var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();

        $("#divSubmitClaimSearchPage").hide();
        $("#btnSearch").click(function () {
            $("#divSubmitClaimSearchPage").show();
        });

        $('input').keypress(function (e) {
            $('input:text[id=' + $(this).attr('id') + ']').css('background-color', '');
        });

        $('.selectdropdown').on('change', function () {
            $('.selectdropdown').css('background-color', '');
        });

        $('#btncloseNPI').click(function () {
            $('#ctl00_MainContent_uc5SubmitClaim_lblnpisearchpopupTxTNPI').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_txtHCPCSCode').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_txtMedicaidid1').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_txtBusinessLastName').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_txtFirstName').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_gvSubmitClaimSearchPage').find('tbody').html('');
            $('#ctl00_MainContent_uc5SubmitClaim_RegularExpressionValidator6').css('display', 'none');
        });
        $('#btncloseNPIPop').click(function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtNPIpop').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_txtMedicaididPop').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_txtlNamedPop').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_txtFNamedPop').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_gvSubmitClaimSearchPagePop').find('tbody').html('');
            $('#ctl00_MainContent_uc5SubmitClaim_RegularExpressionValidator1').css('display', 'none');
            var hdn_SearchId = $('#hdnSearchIdpop').val();
            let hdn_closetextClintID = hdn_SearchId.split("_");
            var hdn_cleanTextBoxID = hdn_closetextClintID[4];


            if (hdn_cleanTextBoxID == 'btnSearch1') {
                $('#ctl00_MainContent_uc5SubmitClaim_txtNPI1').val('');

            }
            else if (hdn_cleanTextBoxID == 'btnSearch2') {
                $('#ctl00_MainContent_uc5SubmitClaim_txtNPI2').val('');

            }
            else if (hdn_cleanTextBoxID == 'btnSearch3') {
                $('#ctl00_MainContent_uc5SubmitClaim_txtNPI1add').val('');

            }
            else if (hdn_cleanTextBoxID == 'btnSearch4') {
                $('#ctl00_MainContent_uc5SubmitClaim_txtNPI2add').val('');

            }
            else if (hdn_cleanTextBoxID == 'btnSearch5') {
                $('#ctl00_MainContent_uc5SubmitClaim_txtNPI3add').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_txtOPMID3add').val(hdn_MedicaidId);
                $('#ctl00_MainContent_uc5SubmitClaim_txtOPName3add').val(hdn_ProviderName);

            }
            else if (hdn_cleanTextBoxID == 'btnSearch6') {

                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI').val(hdn_NPI);
            }
            else if (hdn_cleanTextBoxID == 'txtRenderingProvNPI') {
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI').val('');

            }
            else if (hdn_cleanTextBoxID == 'txtSupervisingProviderNPI') {
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_txtSupervisingProviderNPI').val('');

            }
            else if (hdn_cleanTextBoxID == 'txtNPIServiceFacilityLocation') {
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_errServiceFacilityInfo').css('display', 'none');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_txtNPIServiceFacilityLocation').val('');
            }
            else if (hdn_cleanTextBoxID == 'txtProviderNPI') {
                $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_txtProviderNPI').val('');
            }
            else if (hdn_cleanTextBoxID == 'txtPrimaryCareProviderNPI') {
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').val('');

            }
            else if (hdn_cleanTextBoxID == 'txtReferringProviderNPI') {
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI').val('');

            }
            else if (hdn_cleanTextBoxID == 'txtAssistantSurgeonNPI') {
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_txtAssistantSurgeonNPI').val('');

            }
            else if (hdn_cleanTextBoxID == 'txtAttendingPhysicianNPI') {
                $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI').val('');
            }
        });

     <%--   if ($("#<%=ucReferringProvider.FindControl("txtReferringProviderNPI").ClientID %>").val() !== null && $("#<%=ucReferringProvider.FindControl("txtReferringProviderNPI").ClientID %>").val() !== undefined) {
            localStorage.setItem("ReferingProviderNPI", "" + $("#<%=ucReferringProvider.FindControl("txtReferringProviderNPI").ClientID %>").val() + "");
        }

        if ($("#<%=ucRenderingProviderInformationPanel.FindControl("txtRenderingProvNPI").ClientID %>").val() !== null && $("#<%=ucRenderingProviderInformationPanel.FindControl("txtRenderingProvNPI").ClientID %>").val() !== undefined) {
            localStorage.setItem("RenderingProviderNPI", "" + $("#<%=ucRenderingProviderInformationPanel.FindControl("txtRenderingProvNPI").ClientID %>").val() + "");
         }--%>
        localStorage.setItem("RenderingProviderNPI", "");
        localStorage.setItem("ReferingProviderNPI", "");
        localStorage.setItem("AsstProviderNPI", "");
        //ClearotherPayerInfofields();
        displayOtherPayerInfo();
        
        if (hdnclaimtype == "1") {
            displaytableValueCode();
            //displayInstiServiceDetailtable();
            clearInstiServiceDetailFields();
            displaytableIcdProc();
        }

        DiagClearFields();        
        //displaytable();        
        displaytableOtherPayer();        
        //ProviderNotesClearFields();
        displaytableProviderNote();
        
        if (hdnclaimtype == "0") {
            //displayDentalServiceDetailTable();
            //clearDentalServiceDetailFields();
            binddataSerDet();
            GetServiceLine();
            ClearFieldsTooth();
            //toothdisplaytable();

        }
        if (hdnclaimtype == "2") {
            displayAmbulanceDropOffTable();
            binddataSerDet();
            clearAmbulanceDropOffFields();
        }
        $('#ctl00_MainContent_uc5SubmitClaim_ucClaimAttachment_filesize').val('');
        DisplayInstitutionalHeaderOtherAdjustment();
    });

    $(function () {
        $('#txtProviderNotes').keydown(function (e) {
            if (e.shiftKey || e.ctrlKey || e.altKey) {
                e.preventDefault();
            } else {
                var key = e.keyCode;
                if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
                    e.preventDefault();
                }
            }
        });
    });

    function CollapseExpand(obj) {
        var sp = obj.getElementsByTagName('span')[0];
        var spText = sp.innerHTML;
        var plusIndex = spText.indexOf("+");

        if (plusIndex == 0)
            sp.innerHTML = spText.replace("+", "-");
        else
            sp.innerHTML = spText.replace("-", "+");
    }
    function checkDate(sender, args) {
        if (sender._selectedDate > new Date()) {
            alert("You cannot select a day earlier than today!");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
        }
    }
    function closeCpe() {
        var cpe = $find("cpeServiceInfo");

        if (cpe.get_Collapsed()) {
            cpe._doOpen();
        }
        else {
            cpe._doClose();
        }
    }

    function AddLine() {
        $("#divLine1").show();

    }

    function getbuttondetail(e) {
        if (e.id == "btnDiagSearch2") {
            $("#<%= txtDiagnosisCodeSearch.ClientID %>").first().val("");
            $("#<%= txtDiagnosisCodeDescSearch.ClientID %>").first().val("");
            $("#<%=gvClaimDiagnosisSearch.ClientID %>").html("");
        }
        $("#<%=gvSubmitClaimSearchPage.ClientID %>").html("");

        $('#hdnSearchId').val(e.id);
        localStorage.setItem("btnArgVal", e.id);

        var hdnSearchName = $('#hdnSearchId').val();
        $('#<%=hdnbtnSearch.ClientID %>').val(hdnSearchName);
        $("#divSubmitClaimSearchPage").hide();

    }

    function getTextboxdetail(e) {

        $('#hdnSearchIdpop').val(e.id);
        var hdnSearchName = $('#hdnSearchIdpop').val();
        $('#<%=hdnTxtSearchpop.ClientID %>').val(hdnSearchName);
        $("#divSubmitClaimSearchPage").hide();
    }


    function closemodal() {

        var hdn_NPI = $('#ctl00_MainContent_uc5SubmitClaim_hdnNPI').val();
        var hdn_MedicaidId = $('#ctl00_MainContent_uc5SubmitClaim_hdnMedicaidId').val();
        var hdn_ProviderName = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderName').val();
        var hdn_ProviderLastName = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderLastName').val();
        var hdn_ProviderFirstName = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderFirstName').val();
        var hdn_ProviderAddress = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderAddress').val();
        var hdn_ProviderCity = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderCity').val();
        var hdn_ProviderState = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderState').val();
        var hdn_ProviderZip = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderZip').val();
        var hdn_ProviderAddress2 = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderAddress2').val();
        var hdn_EntityType = $('#ctl00_MainContent_uc5SubmitClaim_hdnEntityType').val();
        var hdn_SearchId = $('#hdnSearchId').val();
        if (hdn_SearchId == 'btnSearch1') {
            $('#ctl00_MainContent_uc5SubmitClaim_txtNPI1').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPMID1').val(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPName1').val(hdn_ProviderName);
        }
        else if (hdn_SearchId == 'btnSearch2') {
            $('#ctl00_MainContent_uc5SubmitClaim_txtNPI2').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPMID2').val(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPName2').val(hdn_ProviderName);
        }
        else if (hdn_SearchId == 'btnSearch3') {
            $('#ctl00_MainContent_uc5SubmitClaim_txtNPI1add').val(hdn_NPI);
            $('#ctl00$MainContent$uc5SubmitClaim$txtOPMID1add').val(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPName1add').val(hdn_ProviderName);
        }
        else if (hdn_SearchId == 'btnSearch4') {
            $('#ctl00_MainContent_uc5SubmitClaim_txtNPI2add').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPMID2add').val(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPName2add').val(hdn_ProviderName);

        }
        else if (hdn_SearchId == 'btnSearch5') {
            $('#ctl00_MainContent_uc5SubmitClaim_txtNPI3add').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPMID3add').val(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPName3add').val(hdn_ProviderName);

        }
        else if (hdn_SearchId == 'btnSearch6') {
            $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblRefProviderMedicaidID').val(hdn_MedicaidId);

            var referringNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI').val();
            var primarycareNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').val();
            var renderingNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI').val();
            var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();
            var RefferingMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblRefProviderMedicaidID').text();
            var PrimaryMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lbPrimaryCareProvMedicaidID').text();
            var RenderingMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingMedicaidID').val();

            if (hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errReferringProviderNPI').text("Non-individual provider cannot be entered as referring provider");
                $('#myModal').modal('hide');
                var result = true;
                if (result) {
                    var checklengtherrorReff = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errReferringProviderNPI').length;

                    if (checklengtherrorReff > 0) {
                        $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_hdnErrorRef').val('true');
                    }
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI').val('');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblRefProviderMedicaidID').text('');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblReffProviderLastName').text('');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblReffProviderFirstName').text('');
                    return;
                }

            }

            if ((referringNPI == renderingNPI) && (RefferingMedID == RenderingMedID)) {
                if ((hdnclaimtype == 0 || hdnclaimtype == 2) && referringNPI != "") {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_divRefErrorMessage').css('display', 'none');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errReferringProviderNPI').text("Rendering provider cannot be same as Referring provider");
                    $('#myModal').modal('hide');
                    var checklengtherrorRef = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errReferringProviderNPI').length;

                    if (checklengtherrorRef > 0) {
                        $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_hdnErrorMessageRef').val('true');
                    }
                    return;
                }
            }
            if ((primarycareNPI == renderingNPI && primarycareNPI.length != 0) || (PrimaryMedID == RenderingMedID && PrimaryMedID.length != 0)) {
                if (hdnclaimtype == 0 || hdnclaimtype == 2) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_divRefErrorMessage').css('display', 'none');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errPrimaryCareProviderNPI').text("Rendering provider cannot be same as Primary care Provider");
                    $('#myModal').modal('hide');
                    var checklengtherrorPri = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errPrimaryCareProviderNPI').length;

                    if (checklengtherrorPri > 0) {
                        $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_hdnErrorMessagePri').val('true');
                    }
                    return;
                }
            }
            if ((referringNPI == primarycareNPI && referringNPI != "") || (RefferingMedID == PrimaryMedID && PrimaryMedID != "")) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblRefProviderMedicaidID').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblReffProviderLastName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblReffProviderFirstName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_divRefErrorMessage').css('display', 'block');
            }
            else {
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_hdnErrorMessageRef').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblReffProviderLastName').text(hdn_ProviderLastName);
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblReffProviderFirstName').text(hdn_ProviderFirstName);
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_divRefErrorMessage').css('display', 'none');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').removeAttr("disabled");
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').css('background', 'white').focus();
            }
        }
        else if (hdn_SearchId == 'btnSearch7') {
            $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingMedicaidID').val(hdn_MedicaidId);

            var result = false;
            var billingnpi = $('#ctl00_MainContent_lblPRONPI2').text();
            var renderNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI').val();
            var renderMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingMedicaidID').val();

            var referNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI').val();
            var referMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblRefProviderMedicaidID').text();

            var primaryNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').val();
            var primaryMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lbPrimaryCareProvMedicaidID').text();

            var supervisingNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_txtSupervisingProviderNPI').val();
            var supervisingMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingProviderMediID').text();

            var assistantSurgeonNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_txtAssistantSurgeonNPI').val();
            var assistantSurgeonMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonMedicaidID').text();

            var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();

            if (hdnclaimtype == 1 && hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_divRenderingErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingErrorMessage').text("Non-individual provider cannot be entered as rendering provider");
                $('#myModal').modal('hide');
                result = true;
            }
            if (assistantSurgeonNPI != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_divRenderingErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingErrorMessage').text("Rendering and Assistant surgeon provider both cannot be entered in the same claim");
                $('#myModal').modal('hide');
                result = true;
            }

            if ((referNPI == renderNPI && renderNPI != "") || (renderMedID == referMedID && renderMedID != "")) {

                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_divRenderingErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingErrorMessage').text("Rendering provider cannot be same as Referring provider");
                $('#myModal').modal('hide');
                result = true;
            }
            if ((primaryNPI == renderNPI && renderNPI != "") || (renderMedID == primaryMedID && renderMedID != "")) {

                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_divRenderingErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingErrorMessage').text("Rendering provider ID should only be entered if it is different than billing provider ID");
                $('#myModal').modal('hide');
                result = true;
            }
            if (billingnpi == renderNPI && renderNPI != "") {

                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_divRenderingErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingErrorMessage').text("Rendering provider ID should only be entered if it is different than billing provider ID");
                $('#myModal').modal('hide');
                result = true;
            }
            if ((supervisingNPI == renderNPI && renderNPI != "") || (renderMedID == supervisingMedID && renderMedID != "")) {
                if (hdnclaimtype == 0 || hdnclaimtype == 2) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_divRenderingErrorMessage').css('display', 'block');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingErrorMessage').text("Rendering provider cannot be same as Supervising provider");
                    $('#myModal').modal('hide');
                    result = true;
                }
            }
            if ((assistantSurgeonNPI == renderNPI && renderNPI != "") || (renderMedID == assistantSurgeonMedID && renderMedID != "")) {
                if (hdnclaimtype == 0 || hdnclaimtype == 2) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_divRenderingErrorMessage').css('display', 'block');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingErrorMessage').text("Rendering Provider cannot be same as Assistant Surgeon");
                    $('#myModal').modal('hide');
                    result = true;
                }
            }
            if (result) {
                var checklengtherrorRend = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingErrorMessage').length;

                if (checklengtherrorRend > 0) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_hdnErrorMessageRender').val('true');
                }
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI').val('');

                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingMedicaidID').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingLastName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingFirstName').text('');
                return;
            }

            $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_divRenderingErrorMessage').css('display', 'none');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_hdnErrorMessageRender').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingMedicaidID').text(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingLastName').text(hdn_ProviderLastName);
            $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingFirstName').text(hdn_ProviderFirstName);

        }
        else if (hdn_SearchId == 'btnSearch8') {
            $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_txtSupervisingProviderNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingProviderMediID').val(hdn_MedicaidId);

            var result = false;
            var supervisingNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_txtSupervisingProviderNPI').val();
            var supervisingMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingProviderMediID').val();

            var assistantNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_txtAssistantSurgeonNPI').val();
            var assistantSurgeonMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonMedicaidID').text();

            var renderingNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI').val();
            var renderMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingMedicaidID').text();

            var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();

            if ((hdnclaimtype == 0 || hdnclaimtype == 2) && hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_divSupervisingErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingError').text("Non - individual provider cannot be entered as supervising provider");
                $('#myModal').modal('hide');
                result = true;
            } if (hdnclaimtype == 1 && hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_divSupervisingErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingError').text("Non - individual provider cannot be entered as operative physician");
                $('#myModal').modal('hide');
                result = true;
            }

            if ((renderingNPI == supervisingNPI && supervisingNPI != "") || (supervisingMedID == renderMedID && supervisingMedID != "")) {
                if (hdnclaimtype == 0 || hdnclaimtype == 2) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_divSupervisingErrorMessage').css('display', 'block');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingError').text("Supervising provider cannot be same as Rendering provider");
                    $('#myModal').modal('hide');
                    result = true;
                }
            }
            if ((assistantNPI == supervisingNPI && supervisingNPI != "") || (supervisingMedID == assistantSurgeonMedID && supervisingMedID != "")) {

                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_divSupervisingErrorMessage').css('display', 'block');
                if (hdnclaimtype == 0 || hdnclaimtype == 2) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingError').text("Assistant surgeon and supervising provider cannot be same");
                }
                if (hdnclaimtype == 1) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingError').text("Operating Physician and Other Operating Physician provider cannot be same");
                }
                $('#myModal').modal('hide');
                result = true;
            }
            if (result) {
                var checklengtherrorSuper = $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingError').length;

                if (checklengtherrorSuper > 0) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_hdnErrorMessageSuper').val('true');
                }
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_txtSupervisingProviderNPI').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingProviderMediID').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingLastName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingFirstName').text('');
                return;
            }
            $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_divSupervisingErrorMessage').css('display', 'none');
            $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_hdnErrorMessageSuper').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingProviderMediID').text(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingLastName').text(hdn_ProviderLastName);
            $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingFirstName').text(hdn_ProviderFirstName);
        }
        else if (hdn_SearchId == 'btnSearch9') {

            if (hdn_EntityType == 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_errServiceFacilityInfo').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_errServiceFacilityInfo').text("Individual provider cannot be entered as facility provider");
                $('#myModal').modal('hide');
                result = true;
            }
            if (result) {

                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_txtNPIServiceFacilityLocation').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationMedicaid').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationAddress1').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationCity').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationState').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationZip').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationAddress2').text('');
                return;
            }
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_errServiceFacilityInfo').css('display', 'none');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_txtNPIServiceFacilityLocation').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationMedicaid').text(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationName').text(hdn_ProviderName);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationAddress1').text(hdn_ProviderAddress);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationCity').text(hdn_ProviderCity);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationState').text(hdn_ProviderState);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationZip').text(hdn_ProviderZip);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationAddress2').text(hdn_ProviderAddress2);

        }
        else if (hdn_SearchId == 'btnDiagSearch2') {

            var hdn_DiagnosisCode = $('#ctl00_MainContent_uc5SubmitClaim_hdnDiagnosisCode').val();
            var hdn_DiagnosisVersion = $('#ctl00_MainContent_uc5SubmitClaim_hdnDiagnosisVersion').val();
            var hdn_DiagnosisDes = $('#ctl00_MainContent_uc5SubmitClaim_hdnDiagnosisDes').val();
            $('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_txtDiagnosisCode').val(hdn_DiagnosisCode);
            $('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlPresentAdmission').val(hdn_DiagnosisVersion);
            $('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_txtDiagnosisCodeDesc').val(hdn_DiagnosisDes);
            $('#myDiagModal').modal('hide'); return;

        }
        else if (hdn_SearchId == 'btnSearchAdditional') {
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_txtProviderNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_lblAdditionalMedicaidID').text(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_lblProviderLastName').text(hdn_ProviderLastName);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_lblProviderFirstName').text(hdn_ProviderFirstName);
        }
        else if (hdn_SearchId == 'btnSearchAdditional1') {
            var hdnadditionalGridSearchRowID = $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_hdnAdditionalGridSearchRowID').val();
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_gvAdditionalProviderinfo_' + hdnadditionalGridSearchRowID + '_txtGridProviderNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_gvAdditionalProviderinfo_' + hdnadditionalGridSearchRowID + '_lblMedId1').text(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_gvAdditionalProviderinfo_' + hdnadditionalGridSearchRowID + '_lbllastname1').text(hdn_ProviderLastName);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_gvAdditionalProviderinfo_' + hdnadditionalGridSearchRowID + '_lblFirstname1').text(hdn_ProviderFirstName);

        }
        else if (hdn_SearchId == 'btnSearchPrimaryCareProvider') {
            $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvMedicaidID').val(hdn_MedicaidId);
            var result = false;
            var refNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI').val();
            var primaryNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').val();
            var primaryMedicaidID = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvMedicaidID').val();
            var RefMedicaidID = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblRefProviderMedicaidID').text();
            var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();

            if ((hdnclaimtype == 0 || hdnclaimtype == 2) && hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errPrimaryCareProviderNPI').text("Non-individual provider cannot be entered as primary care provider");
                $('#myModal').modal('hide');
                result = true;
            }
            if (result) {
                var checklengtherrorPrim = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errPrimaryCareProviderNPI').length;

                if (checklengtherrorPrim > 0) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_hdnErrorPri').val('true');
                }
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvMedicaidID').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvLastName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvFirstName').text('');
                return;
            }

            if ((refNPI == primaryNPI && refNPI != "") || (primaryMedicaidID == RefMedicaidID && primaryMedicaidID != "")) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvMedicaidID').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvLastName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvFirstName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_divRefErrorMessage').css('display', 'block');
                result = true;
            }
            else {
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvLastName').text(hdn_ProviderLastName);
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvFirstName').text(hdn_ProviderFirstName);
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_hdnErrorMessagePri').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_divRefErrorMessage').css('display', 'none');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').removeAttr("disabled");
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').css('background', 'white').focus();
            }
        }
        else if (hdn_SearchId == 'btnAssistantSurgeon') {
            $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_txtAssistantSurgeonNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonMedicaidID').val(hdn_MedicaidId);

            var result = false;
            var assistantSurgeonNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_txtAssistantSurgeonNPI').val();
            var assistantSurgeonMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonMedicaidID').val();

            var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();
            var supervisingNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_txtSupervisingProviderNPI').val();
            var supervisingMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingProviderMediID').text();

            var renderingNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI').val();
            var renderMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingMedicaidID').text();

            if ((hdnclaimtype == 0) && hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_divAssistantSurgeonErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonError').text("Non - individual provider cannot be entered as assistant surgeon");
                $('#myModal').modal('hide');
                result = true;
            }
            if (renderingNPI != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_divAssistantSurgeonErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonError').text("Rendering and Assistant surgeon provider both cannot be entered in the same claim");
                $('#myModal').modal('hide');
                result = true;
            }
            if ((hdnclaimtype == 1) && hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_divAssistantSurgeonErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonError').text("Non - individual provider cannot be entered as other operative physician");
                $('#myModal').modal('hide');
                result = true;
            }

            if ((renderingNPI == assistantSurgeonNPI && assistantSurgeonNPI != "") || (assistantSurgeonMedID == renderMedID && assistantSurgeonMedID != "")) {
                if (hdnclaimtype == 0 || hdnclaimtype == 2) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_divAssistantSurgeonErrorMessage').css('display', 'block');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonError').text("Assistant surgeon and Rendering cannot be same");
                    $('#myModal').modal('hide');
                    result = true;
                }
            }
            if ((assistantSurgeonNPI == supervisingNPI && assistantSurgeonNPI != "") || (assistantSurgeonMedID == supervisingMedID && assistantSurgeonMedID != "")) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_divAssistantSurgeonErrorMessage').css('display', 'block');
                if (hdnclaimtype == 0 || hdnclaimtype == 2) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonError').text("Assistant surgeon and Supervising Provider cannot be same");
                }
                if (hdnclaimtype == 1) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonError').text("Other Operating Physician and Operating Physician Provider cannot be same");
                }
                $('#myModal').modal('hide');
                result = true;
            }
            if (result) {
                var checklengtherrorAssist = $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonError').length;

                if (checklengtherrorAssist > 0) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_hdnErrorMessageAssistant').val('true');
                }
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_txtAssistantSurgeonNPI').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonMedicaidID').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonLastName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonFirstName').text('');
                return;
            }
            $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_divAssistantSurgeonErrorMessage').css('display', 'none');
            $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_hdnErrorMessageAssistant').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonLastName').text(hdn_ProviderLastName);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonFirstName').text(hdn_ProviderFirstName);
        }
        else if (hdn_SearchId == 'btnAttendingPhysician') {
            var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();
            var result = false;
            if ((hdnclaimtype == 1) && hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_divAttendingPhysicianErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianError').text("Non-individual provider cannot be entered as attending provider");
                $('#myModal').modal('hide');
                result = true;
            }
            if (result) {
                var checklengtherror = $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianError').length;

                if (checklengtherror > 0) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_hdnErrorMessageAttending').val('true');
                }
                $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianMedicaidID').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianLastName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianFirstName').text('');
                return;
            }
            $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_divAttendingPhysicianErrorMessage').css('display', 'none');
            $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianError').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianMedicaidID').text(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianLastName').text(hdn_ProviderLastName);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianFirstName').text(hdn_ProviderFirstName);
        }

        $('#myModal').modal('hide');
    }

    function closemodalTOB() {
        var hdn_TOB = $('#ctl00_MainContent_uc5SubmitClaim_hdnTOB').val();
        $('#ctl00_MainContent_uc5SubmitClaim_txtTypeofBill').val(hdn_TOB);
        $('#myModalTOB').modal('hide'); return;

    }

    <%--function FromToValidation(ToDateId) {
         //var txtToDate = document.getElementById("<%= txtToDate.ClientID %>");
        var txtFromDate = $("#" + ToDateId.replace("ToDate", "FromDate"));
        var txtToDate = $("#" + ToDateId);
        if ((txtToDate.val() != "" && txtFromDate.val() == "") || (txtToDate.val() == "" && txtFromDate.val() != "")) {
            txtFromDate.focus();
        }
    }--%>


    function showProgress() {

    }

    function ValidateSubmit() {
        /*show validation list*/
        $('#divErrorList').css('display', 'block');
        /*function for mouse scroll up-down*/
        $(window).bind('mousewheel DOMMouseScroll', function (event) {
            var ScrollTop = $(this).scrollTop();
            if (event.originalEvent.wheelDelta < 0 || event.originalEvent.detail > 0) {
                //scrolldown
                $('#divErrorList').css('top', '0px');
                $('#divErrorList').addClass('fixed-content');
            }
            else {
                //scrollup 
                if (ScrollTop <= 532) {
                    $('#divErrorList').css('top', '0px');
                    $('#divErrorList').removeClass('fixed-content');
                }
            }
        });

        /*function for browser scroll up-down*/
        var lastScrollTop = 0, delta = 5;
        $(window).scroll(function () {
            var nowScrollTop = $(this).scrollTop();
            if (Math.abs(lastScrollTop - nowScrollTop) >= delta) {
                if (nowScrollTop > lastScrollTop) {
                    //scrolldown
                    $('#divErrorList').css('top', '0px');
                    $('#divErrorList').addClass('fixed-content');
                } else {
                    //scrollup
                    if (nowScrollTop <= 532) {
                        $('#divErrorList').css('top', '0px');
                        $('#divErrorList').removeClass('fixed-content');
                    }
                }
                lastScrollTop = nowScrollTop;
            }
        });

        /*to highlight error fields*/
        var val = Page_ClientValidate('validateClaims');
        if (!val) {
            for (var i = 0; i < Page_Validators.length; i++) {
                if (Page_Validators[i].validationGroup == 'validateClaims') {
                    if (!Page_Validators[i].isvalid) {
                        $("#" + Page_Validators[i].controltovalidate).css('background-color', 'gold');
                    }
                    $('input').keypress(function (e) {
                        $('input:text[id=' + $(this).attr('id') + ']').css('background-color', '');
                    });
                }
            }
        }
        //return val;     
    }

    jQuery(document).ready(function ($) {

        localStorage.setItem("RenderingMedicaidID", "");
        localStorage.setItem("AssistantSurgeonMedicaidID", "");
        localStorage.setItem("SupervisingProviderMediID", "");
        localStorage.setItem("ServiceFacilityLocationMedicaid", "");
        localStorage.setItem("RefProviderMedicaidID", "");
        localStorage.setItem("PrimaryCareProvMedicaidID", "");

        localStorage.setItem("RenderingProviderNPI", "");
        localStorage.setItem("ReferingProviderNPI", "");
        localStorage.setItem("AsstProviderNPI", "");
        localStorage.setItem("SuperVisingProviderNPI", "");
        localStorage.setItem("ServiceFacilityNPI", "");
        localStorage.setItem("PrimarycareReferingProviderNPI", "");

        $('a[href^="#"]').click(function () {
            $('#divErrorList').css('top', '');
            var target = this.hash,
                $target = $(target);

            $('html,body').animate({
                scrollTop: $target.offset().top - 470 + 'px'
            }, 'fast', 'linear', function () {
                $target.focus();
                if ($target.is(":focus")) { // Checking if the target was focused                 
                    return false;
                } else {
                    $target.attr('tabindex', '-1'); // Adding tabindex for elements not focusable
                    $target.focus(); // Set focus again
                };
            });
        });
    });
    //$(window).resize(function () {
    //    $('#divErrorList').height($(window).height() - 30);
    //});

    //$(window).trigger('resize');
    function AsyncConfirmYesNo(title, msg, yesFn, noFn) {
        var $confirm = $("#modalConfirmYesNo");
        $confirm.modal('show');
        $("#lblTitleConfirmYesNo").html(title);
        $("#lblMsgConfirmYesNo").html(msg);
        $("#btnYesConfirmYesNo").off('click').click(function () {
            yesFn();
            $confirm.modal("hide");
        });
        $("#btnNoConfirmYesNo").off('click').click(function () {
            noFn();
            $confirm.modal("hide");
        });
        $("#btnModalPopupClose").off('click').click(function () {
            $("#btnSubmit").prop("disabled", false);
            $confirm.modal("hide");
        });
        
    }

    function confirmPriorAuthNumYESClick() {
        CallBlockUIPostBack();
        __doPostBack('<%= btnSubmit.UniqueID%>', '');
    }
    function confirmPriorAuthNumNOClick() {
        $("#btnSubmit").prop("disabled", false);
    }

    function ShowConfirmPriorAuthNum() {
        var txtPriorAuthNum = $('#ctl00_MainContent_uc5SubmitClaim_ucPriorAuthorizationAndReferringPanel_txtPriorAuthNumber').val();
        var flag = true;
        if (txtPriorAuthNum != "") {

            AsyncConfirmYesNo(
                'PRIOR AUTHORIZATION',
                'Are you sure the Prior Authorization Number entered is correct?',
                confirmPriorAuthNumYESClick,
                confirmPriorAuthNumNOClick
            );
            flag = false;

        } else {
            var warnFlag = CheckPriorAuthNumAvailableForServiceLine();

            if (warnFlag) {
                AsyncConfirmYesNo(
                    'PRIOR AUTHORIZATION',
                    'Are you sure the Prior Authorization Number entered is correct?',
                    confirmPriorAuthNumYESClick,
                    confirmPriorAuthNumNOClick
                );
                flag = false;
            } else {
                flag = true;
            }
        }
        return flag;
    }

    function ValidatePortalErrors() {
        var IsCheck = false;
        $('#divErrorList').css('display', 'block');
        $('#divErrorList').empty();
        /* bug-6763*/

        if ($('#divErrorList').is(':visible')) {
            $('#divErrorList').find('.error-message:first').before('<div class="errorlist"><strong>Below errors are created. Please click on the error text to navigate to the panel or field where errors are encountered</strong></div><br/>');
        }

        var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();
        if ($('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errReferringProviderNPI').is(':visible')) {
            $('#divErrorList').find('.errorlist:last').after('<div><a href="#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI" class="errorlist">*Incorrect NPI for Referring provider</a></div>');
            IsCheck = true;
        }

        $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_grdOtherPayer tr').each(function () {
            var DetailorHeader = $(this).find("td").eq(5).html();
            if (($("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_gvOtherPaidAdjudicationInfo tr").length == 0 && hdnclaimtype == 0 && DetailorHeader == 'Detail') && ($("#otherpayerpaiderror1").length == 0)) {
                $('#divErrorList').append('<div id="otherpayerpaiderror1"><a href="#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlDetailId" class="errorlist">*Other payer payment information is required</a></div>');
                IsCheck = true;
            }
        });
        // Attending Physician
        $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI').css('background-color', 'white'); // initially making background white color, if any error occurs then highlights in gold.

        if (($('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI').val() == '') &&
            ($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) == '013' ||
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) == '085')
            && ($('#AttendingError1').length == 0)) {
            var IsCheckErr = new Boolean(false);


            var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "POST",
                url: webApiClaims + "GetInstitutionalServiceDetails?claimid=" + claimid,
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimServiceDetailsInistList = result;
                    if (claimServiceDetailsInistList.length > 0) {
                        var j = 0;
                        for (var i = 0; i < claimServiceDetailsInistList.length; i++) {

                            var Revinucode = result[i].revenue_Code;
                            if ((Revinucode != undefined) && (Revinucode != null) && (Revinucode != "")) {

                                if (Revinucode.substr(0, 3) != '054') {

                                    IsCheckErr = true;
                                    //return false;
                                }


                            };
                            j++;

                        }

                        if (IsCheckErr === true) {
                            $('#divErrorList').append('<div id="AttendingError2"><a href="#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI" class="errorlist">*Attending physician is required</a></div>');
                            $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI').css('background-color', 'gold');
                            IsCheck = true;
                        }

                    }

                }
                //    ,
                //error: function (jqXHR, textStatus, errorThrown) {

                //    }
            });
            //$('table tr').each(function () {
            //    var Revinucode = $(this).find("td").eq(1).html();




        }
        else if (($('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI').val() == '') &&
            (($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) != '013' ||
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) != '085') &&
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val() != '')
            && ($('#AttendingError1').length == 0)) {
            $('#divErrorList').append('<div id="AttendingError2"><a href="#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI" class="errorlist">*Attending physician is required</a></div>');
            $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI').css('background-color', 'gold');
            IsCheck = true;
        }

        //if ($('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_errAttendingPhysicianNPI').is(':visible')) {
        //    $('#divErrorList').append('<div id="AttendingError1"><a href="#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI" class="errorlist">*Incorrect NPI for Attending Physician Information</a></div>');
        //    IsCheck = true;            
        //}
        if (hdnclaimtype == '1') {
            //service info insti
            // error 1
            //if ($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 4) == '0111' &&
            //    ($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtDischargeHr').val() == "" ||
            //        $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtDischargeHr').val() == null) &&
            //    !($('#serviceInformationInstitutionalError1').is(':visible'))
            //) {
            //    $('#divErrorList').append('<div id="serviceInformationInstitutionalError1"><a href="#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtDischargeHr" class="errorlist">*Discharge hour is required</a></div>');
            //    IsCheck = true;
            //    return false
            //}
            const billError = document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblInstErr');

            if (billError && billError.innerText.trim() !== '') {
                $('#divErrorList').append('<div id="serviceInformationInstitutionalError1"><a href="#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill" class="errorlist">*' + billError.innerText + '</a></div>');
                IsCheck = true;
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').css('background-color', 'gold');
            }
            
            // error 2 
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionDate').css('background-color', 'white'); // initially making background white color, if any error occurs then highlights in gold.
            //OHPNM-16159-Institutional Claims- Admission date only required for inpatient claims, not required for outpatient claims.
            if (($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) == '011' ||
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) == '012' ||
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) == '018' ||
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) == '021' ||
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) == '022' ||
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) == '028' ||
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) == '041' ||
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) == '065' ||
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) == '066' ||
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) == '086') &&
                (
                    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionDate').val() == "" ||
                    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionDate').val() == null ||
                    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionHr').val() == "" ||
                    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionHr').val() == null)
            ) {
                $('#divErrorList').append('<div id="serviceInformationInstitutionalError2"><a href="#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionDate" class="errorlist">*Admission date and time is required</a></div>');
                IsCheck = true;
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionDate').css('background-color', 'gold');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionHr').css('background-color', 'gold');
            }

            // error 3  
            if ($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtToDate').val() != "" &&
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtToDate').val() != null &&
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtToDate').val() != undefined) {

                if ((Date.parse($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionDate').val()) >
                    Date.parse($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtToDate').val())) &&
                    ($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionDate').val() != "" &&
                        $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionDate').val() != null)) {

                    $('#divErrorList').append('<div id="serviceInformationInstitutionalError3"><a href="#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionDate" class="errorlist">*Admission date and time is invalid(Greater Than ToDate)</a></div>');
                    IsCheck = true;
                    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionDate').css('background-color', 'gold');
                }
            }
            // error 4 
            //if (($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 4) == '0111' ||
            //    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 4) == '0121' ||
            //    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 4) == '0181' ||
            //    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 4) == '0211' ||
            //    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 4) == '0281' ||
            //    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 4) == '0411' ||
            //    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 4) == '0651' ||
            //    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 4) == '0661' ||
            //    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 4) == '0861') &&
            //    ($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtDischargeHr').val() == "" ||
            //        $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtDischargeHr').val() == null)) {

            //    $('#divErrorList').append('<div id="serviceInformationInstitutionalError4"><a href="#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtDischargeHr" class="errorlist">*Discharge Hour is required for this Type of bill</a></div>');
            //    IsCheck = true;
            //    $('#divErrorList')[0].scrollIntoView(true);
            //    return false
            //}
            // 
            if (($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(3, 1) == '7' ||
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(3, 1) == '8') &&
                ($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val() != "" &&
                    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val() != null)) {
                var resposeFrequencyCode = $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_hdnClaimFrequencyCode').val();
                if ((resposeFrequencyCode !== null) && (resposeFrequencyCode !== "") && (resposeFrequencyCode !== undefined) && (resposeFrequencyCode !== '7') && (resposeFrequencyCode !== '8')) {
                    $('#divErrorList').append('<div id="serviceInformationInstitutionalError1"><a  class="errorlist">Search and inquire the claim to adjust or void</a></div>');
                    IsCheck = true;
                }
            }
        }

        var diagTable = localStorage.getItem("diagTable");

        if ((hdnclaimtype == 2 || hdnclaimtype == 1) && ($('#DiagnosisError1').length == 0)) {

            if ($('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_providerDiagOutput tr').length == 0) {
                $('#divErrorList').append('<div id="DiagnosisError1"><a href="#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_txtDiagnosisCode" class="errorlist">*Diagnosis code is required.</a></div>');
                IsCheck = true;
            }
        }

        if (hdnclaimtype == 1) {
            var PrincipalError = true;
            var HideAdmittingError1 = false;
            var AdmittingError2 = false;
            var PatientReasonForVisitEror = false;
            var count = 0; var admittingCount = 0; var reasonToVisitCount = 0; var extCauseOfInj = 0; var totalCount = 0; var othercount = 0;
            if ($('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_providerDiagOutput tr').length >= 2) {


                $('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_providerDiagOutput tr').each(function () {
                    var spanValue = $(this).closest("tr").find('td').eq(0).find('span').text();


                    if (spanValue != '') {
                        if (spanValue == "Principal") { count++; }
                        if (spanValue == "Admitting") { admittingCount++; }
                        if (spanValue == "External Cause of Injury") { extCauseOfInj++; }
                        if (spanValue == "Patient Reason for Visit") { reasonToVisitCount++; }
                        if (spanValue == "Other") { othercount++; }
                        if ((spanValue == "Principal")) {
                            PrincipalError = false;
                        }
                        if ($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val() != '') {
                            if (($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) == '011' && spanValue == 'Admitting')) {
                                HideAdmittingError1 = true;
                            }
                            if (($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) != '011' && spanValue == 'Admitting')) {
                                AdmittingError2 = true;
                            }
                            if ($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0, 3) != '013' && spanValue == 'Patient Reason for Visit') {
                                PatientReasonForVisitEror = true;
                            }
                        }
                    }
                });

                if (PrincipalError) {
                    $('#divErrorList').append('<div id="DiagnosisError2"><a href="#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription" class="errorlist">*Principal diagnosis code is required</a></div>');
                    IsCheck = true;
                }
                if (count > 1) {
                    $('#divErrorList').append('<div id="DiagnosisError3"><a href="#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription" class="errorlist">*Principal diagnosis code not repeated more than 1</a></div>');
                    IsCheck = true;
                }
                if (admittingCount > 1) {
                    $('#divErrorList').append('<div id="DiagnosisError4"><a href="#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription" class="errorlist">*Admitting diagnosis code not repeated more than 1</a></div>');
                    IsCheck = true;

                }

                if (reasonToVisitCount > 3) {
                    $('#divErrorList').append('<div id="DiagnosisError5"><a href="#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription" class="errorlist">*Patient Reason for Visit diagnosis code not repeated more than 3</a></div>');
                    IsCheck = true;
                }
                if (extCauseOfInj > 12) {
                    $('#divErrorList').append('<div id="DiagnosisError6"><a href="#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription" class="errorlist">*External Cause of Injury diagnosis code not repeated more than 12</a></div>');
                    IsCheck = true;

                }
                if (othercount > 24) {
                    $('#divErrorList').append('<div id="DiagnosisError7"><a href="#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription" class="errorlist">*Other diagnosis code not repeated more than 24</a></div>');
                    IsCheck = true;

                }
                //if ($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(0,3) == '011' && !HideAdmittingError1 && ($('#DiagnosisError3').length == 0)) {
                //    $('#divErrorList').append('<div id="DiagnosisError3"><a href="#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription" class="errorlist">*Admitting diagnosis code is required</a></div>');
                //    IsCheck = true;
                //}

                //if (AdmittingError2 && ($('#DiagnosisError4').length == 0)) {
                //    $('#divErrorList').append('<div id="DiagnosisError4"><a href="#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription" class="errorlist">*Admitting diagnosis code is inappropriate for this type of bill</a></div>');
                //    IsCheck = true;
                //}

                //if (PatientReasonForVisitEror && ($('#DiagnosisError5').length == 0)) {
                //    $('#divErrorList').append('<div id="DiagnosisError5"><a href="#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription" class="errorlist">*Patient reason for visit is inappropriate is this type of bill.</a></div>');
                //    IsCheck = true;
                //}


            }
        }
        document.getElementById('ctl00_MainContent_uc5SubmitClaim_hdnErrorMsg').value = IsCheck;
        //validateportal
        if (IsCheck) {
            $('#divErrorList').css('display', 'block');
            $("#btnSubmit").prop("disabled", false);
            return false;
        }
        else
        {

            //if Prior AuthNum is empty then returns true else Confirm popup will be displayed and ruturns false
            if (ShowConfirmPriorAuthNum()) {
                CallBlockUIPostBack();
                __doPostBack('<%= btnSubmit.UniqueID%>', '');
                return true;
            } else {
                return false;

            }
        }
    }

    function CheckPriorAuthNumAvailableForServiceLine() {
        var flag = false;
        try {
            var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
            var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();

            if (hdnclaimtype == 1) {
                flag = false;
                return flag;

            }

            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                async: false,
                url: webApiClaims + "CheckServiceLinePriorAuthNumberExists?claimid=" + claimid + "&claimType=" + hdnclaimtype,
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    flag= result;
                }
            });

        } catch (e) {
            flag = false;
        } 
        return flag;

    }

    $('#btncloseNPI').click(function () {
        $('#ctl00_MainContent_uc5SubmitClaim_lblnpisearchpopupTxTNPI').text('');
        $('#ctl00_MainContent_uc5SubmitClaim_txtHCPCSCode').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_txtMedicaidid1').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_txtBusinessLastName').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_txtFirstName').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_gvSubmitClaimSearchPage').find('tbody').html('');
        $('#ctl00_MainContent_uc5SubmitClaim_RegularExpressionValidator6').css('display', 'none');
    })

    function GetDestinationPayerIDforDestinationName() {
        document.getElementById("<%=ddlDestinationPayerID.ClientID %>").disabled = false;
        var value = document.getElementById("<%=ddlPrimaryDestinationPayer.ClientID %>").value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetDestinationPayerID?DestnationPayer=" + value,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                ddlDestinationPayerID = document.getElementById("<%=ddlDestinationPayerID.ClientID %>");
                ddlDestinationPayerID.options.length = 0;
                if (result.length > 1) {
                    $("#<%=ddlDestinationPayerID.ClientID %>").append('<option value=" - 1"> </option>');
                    localStorage.setItem("payerdesc", "");

                }

                else {
                    var value = (result[0].Payer_code);
                    document.getElementById("<%=hdnddlDestinationPayerID.ClientID %>").value = value;
                    localStorage.setItem("payerdesc", value);

                }
                for (var i = 0; i < result.length; i++) {
                    $("#<%=ddlDestinationPayerID.ClientID %>").append("<option value" + result[i].Payer_desc + ">" + result[i].Payer_code + "</option>");
                    var payercode = result[i].Payer_desc;
                    $("#<%=hdnDestinationPayerId.ClientID %>").val(payercode)
                };
                loaddata();
            }

        });

    }



    function loaddata() {
        var valuepayer = $("#<%=ddlPrimaryDestinationPayer.ClientID %> option:selected").text();

        var valuepayerid = localStorage.getItem("payerdesc");

        if (valuepayer == null || valuepayer == "") {
            document.getElementById("<%=ddlDestinationPayerID.ClientID %>").disabled = true;
            document.getElementById("<%=ddlDestinationPayerResponsibilitySequence.ClientID %>").disabled = true;
        }
        else {
            document.getElementById("<%=ddlDestinationPayerID.ClientID %>").disabled = false;
            document.getElementById("<%=ddlDestinationPayerResponsibilitySequence.ClientID %>").disabled = true;
        }

        if (valuepayerid != null && valuepayerid != "") {
            document.getElementById("<%=ddlDestinationPayerID.ClientID %>").disabled = false;
            document.getElementById("<%=ddlDestinationPayerResponsibilitySequence.ClientID %>").disabled = false;
        }
    }

    /*Task 7408 */
    function closemodalPop() {

        var hdn_NPI = $('#ctl00_MainContent_uc5SubmitClaim_hdnNPIpop').val();
        var hdn_MedicaidId = $('#ctl00_MainContent_uc5SubmitClaim_hdnMedicaidIdpop').val();
        var hdn_ProviderName = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderNamepop').val();
        var hdn_ProviderLastName = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderLastNamepop').val();
        var hdn_ProviderFirstName = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderFirstNamepop').val();
        var hdn_ProviderAddress = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderAddresspop').val();
        var hdn_ProviderCity = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderCitypop').val();
        var hdn_ProviderState = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderStatepop').val();
        var hdn_ProviderZip = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderZippop').val();
        var hdn_ProviderAddress2 = $('#ctl00_MainContent_uc5SubmitClaim_hdnProviderAddress2pop').val();
        var hdn_EntityType = $('#ctl00_MainContent_uc5SubmitClaim_hdnEntityTypepop').val();
        var hdn_SearchId = $('#hdnSearchIdpop').val();
        let hdn_textClintID = hdn_SearchId.split("_");
        var hdn_SearchTextBoxID = hdn_textClintID[4];

        if (hdn_SearchTextBoxID == 'txtRenderingProvNPI') {
            $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingMedicaidID').val(hdn_MedicaidId);

            var result = false;
            var billingnpi = $('#ctl00_MainContent_lblPRONPI2').text();
            var renderNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI').val();
            var renderMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingMedicaidID').val();

            var referNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI').val();
            var referMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblRefProviderMedicaidID').text();

            var primaryNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').val();
            var primaryMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lbPrimaryCareProvMedicaidID').text();

            var supervisingNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_txtSupervisingProviderNPI').val();
            var supervisingMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingProviderMediID').text();

            var assistantSurgeonNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_txtAssistantSurgeonNPI').val();
            var assistantSurgeonMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonMedicaidID').text();

            var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();

            if (hdnclaimtype == 1 && hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_divRenderingErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingErrorMessage').text("Non-individual provider cannot be entered as rendering provider");
                $('#myModalpop').modal('hide');
                result = true;
            }

            if ((referNPI == renderNPI && renderNPI != "") || (renderMedID == referMedID && renderMedID != "")) {

                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_divRenderingErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingErrorMessage').text("Rendering provider cannot be same as Referring provider");
                $('#myModalpop').modal('hide');
                result = true;
            }
            if ((primaryNPI == renderNPI && renderNPI != "") || (renderMedID == primaryMedID && renderMedID != "")) {

                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_divRenderingErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingErrorMessage').text("Rendering provider cannot be same as and Primary Care provider");
                $('#myModalpop').modal('hide');
                result = true;
            }
            if (billingnpi == renderNPI && renderNPI != "") {

                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_divRenderingErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingErrorMessage').text("Rendering provider ID should only be entered if it is different than billing provider ID");
                $('#myModalpop').modal('hide');
                result = true;
            }
            if ((supervisingNPI == renderNPI && renderNPI != "") || (renderMedID == supervisingMedID && renderMedID != "")) {
                if (hdnclaimtype == 0 || hdnclaimtype == 2) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_divRenderingErrorMessage').css('display', 'block');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingErrorMessage').text("Rendering provider cannot be same as Supervising provider");
                    $('#myModalpop').modal('hide');
                    result = true;
                }
            }
            if ((assistantSurgeonNPI == renderNPI && renderNPI != "") || (renderMedID == assistantSurgeonMedID && renderMedID != "")) {
                if (hdnclaimtype == 0 || hdnclaimtype == 2) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_divRenderingErrorMessage').css('display', 'block');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingErrorMessage').text("Rendering Provider cannot be same as Assistant Surgeon");
                    $('#myModalpop').modal('hide');
                    result = true;
                }
            }
            if (result) {
                var checklengtherrorRend = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingErrorMessage').length;

                if (checklengtherrorRend > 0) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_hdnErrorMessageRender').val('true');
                }
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI').val('');

                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingMedicaidID').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingLastName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingFirstName').text('');
                return;
            }

            $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_divRenderingErrorMessage').css('display', 'none');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_hdnErrorMessageRender').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingMedicaidID').text(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingLastName').text(hdn_ProviderLastName);
            $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingFirstName').text(hdn_ProviderFirstName);

        }
        else if (hdn_SearchTextBoxID == 'txtSupervisingProviderNPI') {
            $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_txtSupervisingProviderNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingProviderMediID').val(hdn_MedicaidId);

            var result = false;
            var supervisingNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_txtSupervisingProviderNPI').val();
            var supervisingMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingProviderMediID').val();

            var assistantNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_txtAssistantSurgeonNPI').val();
            var assistantSurgeonMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonMedicaidID').text();

            var renderingNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI').val();
            var renderMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingMedicaidID').text();

            var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();

            if ((hdnclaimtype == 0 || hdnclaimtype == 2) && hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_divSupervisingErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingError').text("Non - individual provider cannot be entered as supervising provider");
                $('#myModalpop').modal('hide');
                result = true;
            } if (hdnclaimtype == 1 && hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_divSupervisingErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingError').text("Non - individual provider cannot be entered as operative physician");
                $('#myModalpop').modal('hide');
                result = true;
            }

            if ((renderingNPI == supervisingNPI && supervisingNPI != "") || (supervisingMedID == renderMedID && supervisingMedID != "")) {
                if (hdnclaimtype == 0 || hdnclaimtype == 2) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_divSupervisingErrorMessage').css('display', 'block');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingError').text("Supervising provider cannot be same as Rendering provider");
                    $('#myModalpop').modal('hide');
                    result = true;
                }
            }
            if ((assistantNPI == supervisingNPI && supervisingNPI != "") || (supervisingMedID == assistantSurgeonMedID && supervisingMedID != "")) {

                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_divSupervisingErrorMessage').css('display', 'block');
                if (hdnclaimtype == 0 || hdnclaimtype == 2) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingError').text("Assistant surgeon and supervising provider cannot be same");
                }
                if (hdnclaimtype == 1) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingError').text("Operating Physician and Other Operating Physician provider cannot be same");
                }
                $('#myModalpop').modal('hide');
                result = true;
            }
            if (result) {
                var checklengtherrorSuper = $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingError').length;

                if (checklengtherrorSuper > 0) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_hdnErrorMessageSuper').val('true');
                }
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_txtSupervisingProviderNPI').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingProviderMediID').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingLastName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingFirstName').text('');
                return;
            }
            $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_divSupervisingErrorMessage').css('display', 'none');
            $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_hdnErrorMessageSuper').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingProviderMediID').text(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingLastName').text(hdn_ProviderLastName);
            $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingFirstName').text(hdn_ProviderFirstName);
        }
        else if (hdn_SearchTextBoxID == 'txtNPIServiceFacilityLocation') {
            if (hdn_EntityType == 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_errServiceFacilityInfo').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_errServiceFacilityInfo').text("Individual provider cannot be entered as facility provider");
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_txtNPIServiceFacilityLocation').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationMedicaid').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationAddress1').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationCity').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationState').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationZip').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationAddress2').text('');
                $('#myModalpop').modal('hide');
                result = true;
            }
            if (result) {

                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_txtNPIServiceFacilityLocation').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationMedicaid').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationAddress1').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationCity').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationState').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationZip').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationAddress2').text('');
                return;
            }
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_errServiceFacilityInfo').css('display', 'none');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_txtNPIServiceFacilityLocation').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationMedicaid').text(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationName').text(hdn_ProviderName);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationAddress1').text(hdn_ProviderAddress);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationCity').text(hdn_ProviderCity);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationState').text(hdn_ProviderState);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationZip').text(hdn_ProviderZip);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationAddress2').text(hdn_ProviderAddress2);
        }
        else if (hdn_SearchTextBoxID == 'txtProviderNPI') {
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_txtProviderNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_lblAdditionalMedicaidID').text(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_lblProviderLastName').text(hdn_ProviderLastName);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_lblProviderFirstName').text(hdn_ProviderFirstName);
        }
        else if (hdn_SearchTextBoxID == 'txtPrimaryCareProviderNPI') {
            $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvMedicaidID').val(hdn_MedicaidId);
            var result = false;
            var refNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI').val();
            var primaryNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').val();
            var primaryMedicaidID = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvMedicaidID').val();
            var RefMedicaidID = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblRefProviderMedicaidID').text();
            var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();

            if ((hdnclaimtype == 0 || hdnclaimtype == 2) && hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errPrimaryCareProviderNPI').text("Non-individual provider cannot be entered as primary care provider");
                $('#myModalpop').modal('hide');
                result = true;
            }
            if (result) {
                var checklengtherrorPrim = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errPrimaryCareProviderNPI').length;

                if (checklengtherrorPrim > 0) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_hdnErrorPri').val('true');
                }
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvMedicaidID').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvLastName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvFirstName').text('');
                return;
            }

            if ((refNPI == primaryNPI && refNPI != "") || (primaryMedicaidID == RefMedicaidID && primaryMedicaidID != "")) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvMedicaidID').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvLastName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvFirstName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_divRefErrorMessage').css('display', 'block');
                result = true;
            }
            else {
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvLastName').text(hdn_ProviderLastName);
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvFirstName').text(hdn_ProviderFirstName);
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_hdnErrorMessagePri').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_divRefErrorMessage').css('display', 'none');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').removeAttr("disabled");
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').css('background', 'white').focus();
            }
        }

        else if (hdn_SearchTextBoxID == 'txtAssistantSurgeonNPI') {
            $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_txtAssistantSurgeonNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonMedicaidID').val(hdn_MedicaidId);

            var result = false;
            var assistantSurgeonNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_txtAssistantSurgeonNPI').val();
            var assistantSurgeonMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonMedicaidID').val();

            var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();
            var supervisingNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_txtSupervisingProviderNPI').val();
            var supervisingMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingProviderMediID').text();

            var renderingNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI').val();
            var renderMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingMedicaidID').text();

            if ((hdnclaimtype == 0) && hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_divAssistantSurgeonErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonError').text("Non - individual provider cannot be entered as assistant surgeon");
                $('#myModalpop').modal('hide');
                result = true;
            }
            if ((hdnclaimtype == 1) && hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_divAssistantSurgeonErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonError').text("Non - individual provider cannot be entered as other operative physician");
                $('#myModalpop').modal('hide');
                result = true;
            }

            if ((renderingNPI == assistantSurgeonNPI && assistantSurgeonNPI != "") || (assistantSurgeonMedID == renderMedID && assistantSurgeonMedID != "")) {
                if (hdnclaimtype == 0 || hdnclaimtype == 2) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_divAssistantSurgeonErrorMessage').css('display', 'block');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonError').text("Assistant surgeon and Rendering cannot be same");
                    $('#myModalpop').modal('hide');
                    result = true;
                }
            }
            if ((assistantSurgeonNPI == supervisingNPI && assistantSurgeonNPI != "") || (assistantSurgeonMedID == supervisingMedID && assistantSurgeonMedID != "")) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_divAssistantSurgeonErrorMessage').css('display', 'block');
                if (hdnclaimtype == 0 || hdnclaimtype == 2) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonError').text("Assistant surgeon and Supervising Provider cannot be same");
                }
                if (hdnclaimtype == 1) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonError').text("Other Operating Physician and Operating Physician Provider cannot be same");
                }
                $('#myModalpop').modal('hide');
                result = true;
            }
            if (result) {
                var checklengtherrorAssist = $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonError').length;

                if (checklengtherrorAssist > 0) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_hdnErrorMessageAssistant').val('true');
                }
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_txtAssistantSurgeonNPI').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonMedicaidID').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonLastName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonFirstName').text('');
                return;
            }
            $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_divAssistantSurgeonErrorMessage').css('display', 'none');
            $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_hdnErrorMessageAssistant').val('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonLastName').text(hdn_ProviderLastName);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonFirstName').text(hdn_ProviderFirstName);
        }
        else if (hdn_SearchTextBoxID == 'txtAttendingPhysicianNPI') {
            var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();
            var result = false;
            if ((hdnclaimtype == 1) && hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_divAttendingPhysicianErrorMessage').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianError').text("Non-individual provider cannot be entered as attending provider");
                $('#myModalpop').modal('hide');
                result = true;
            }
            if (result) {
                var checklengtherror = $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianError').length;

                if (checklengtherror > 0) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_hdnErrorMessageAttending').val('true');
                }
                $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianMedicaidID').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianLastName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianFirstName').text('');
                return;
            }
            $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_divAttendingPhysicianErrorMessage').css('display', 'none');
            $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianError').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianMedicaidID').text(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianLastName').text(hdn_ProviderLastName);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianFirstName').text(hdn_ProviderFirstName);
        }
        else if (hdn_SearchTextBoxID == 'btnSearchAdditional1') {
            var hdnadditionalGridSearchRowID = $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_hdnAdditionalGridSearchRowID').val();
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_gvAdditionalProviderinfo_' + hdnadditionalGridSearchRowID + '_txtGridProviderNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_gvAdditionalProviderinfo_' + hdnadditionalGridSearchRowID + '_lblMedId1').text(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_gvAdditionalProviderinfo_' + hdnadditionalGridSearchRowID + '_lbllastname1').text(hdn_ProviderLastName);
            $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_gvAdditionalProviderinfo_' + hdnadditionalGridSearchRowID + '_lblFirstname1').text(hdn_ProviderFirstName);

        }
        else if (hdn_SearchTextBoxID == 'btnDiagSearch2') {

            var hdn_DiagnosisCode = $('#ctl00_MainContent_uc5SubmitClaim_hdnDiagnosisCode').val();
            var hdn_DiagnosisVersion = $('#ctl00_MainContent_uc5SubmitClaim_hdnDiagnosisVersion').val();
            var hdn_DiagnosisDes = $('#ctl00_MainContent_uc5SubmitClaim_hdnDiagnosisDes').val();
            $('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_txtDiagnosisCode').val(hdn_DiagnosisCode);
            $('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlPresentAdmission').val(hdn_DiagnosisVersion);
            $('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_txtDiagnosisCodeDesc').val(hdn_DiagnosisDes);
            $('#myDiagModal').modal('hide'); return;

        }
        else if (hdn_SearchTextBoxID == 'btnSearch1') {
            $('#ctl00_MainContent_uc5SubmitClaim_txtNPI1').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPMID1').val(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPName1').val(hdn_ProviderName);
        }
        else if (hdn_SearchTextBoxID == 'btnSearch2') {
            $('#ctl00_MainContent_uc5SubmitClaim_txtNPI2').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPMID2').val(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPName2').val(hdn_ProviderName);
        }
        else if (hdn_SearchTextBoxID == 'btnSearch3') {
            $('#ctl00_MainContent_uc5SubmitClaim_txtNPI1add').val(hdn_NPI);
            $('#ctl00$MainContent$uc5SubmitClaim$txtOPMID1add').val(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPName1add').val(hdn_ProviderName);
        }
        else if (hdn_SearchTextBoxID == 'btnSearch4') {
            $('#ctl00_MainContent_uc5SubmitClaim_txtNPI2add').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPMID2add').val(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPName2add').val(hdn_ProviderName);

        }
        else if (hdn_SearchTextBoxID == 'btnSearch5') {
            $('#ctl00_MainContent_uc5SubmitClaim_txtNPI3add').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPMID3add').val(hdn_MedicaidId);
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPName3add').val(hdn_ProviderName);

        }
        else if (hdn_SearchTextBoxID == 'txtReferringProviderNPI') {
            $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI').val(hdn_NPI);
            $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblRefProviderMedicaidID').val(hdn_MedicaidId);

            var referringNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI').val();
            var primarycareNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').val();
            var renderingNPI = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI').val();
            var hdnclaimtype = $('#ctl00_MainContent_uc5SubmitClaim_rblClaimType input:checked').val();
            var RefferingMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblRefProviderMedicaidID').text();
            var PrimaryMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lbPrimaryCareProvMedicaidID').text();
            var RenderingMedID = $('#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingMedicaidID').val();

            if (hdn_EntityType != 1) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errReferringProviderNPI').text("Non-individual provider cannot be entered as referring provider");
                $('#myModalpop').modal('hide');
                var result = true;
                if (result) {
                    var checklengtherrorReff = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errReferringProviderNPI').length;

                    if (checklengtherrorReff > 0) {
                        $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_hdnErrorRef').val('true');
                    }
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI').val('');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblRefProviderMedicaidID').text('');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblReffProviderLastName').text('');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblReffProviderFirstName').text('');
                    return;
                }

            }

            if ((referringNPI == renderingNPI) && (RefferingMedID == RenderingMedID)) {
                if ((hdnclaimtype == 0 || hdnclaimtype == 2) && referringNPI != "") {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_divRefErrorMessage').css('display', 'none');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errReferringProviderNPI').text("Rendering provider cannot be same as Referring provider");
                    $('#myModal').modal('hide');
                    var checklengtherrorRef = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errReferringProviderNPI').length;

                    if (checklengtherrorRef > 0) {
                        $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_hdnErrorMessageRef').val('true');
                    }
                    return;
                }
            }
            if ((primarycareNPI == renderingNPI && primarycareNPI.length != 0) || (PrimaryMedID == RenderingMedID && PrimaryMedID.length != 0)) {
                if (hdnclaimtype == 0 || hdnclaimtype == 2) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_divRefErrorMessage').css('display', 'none');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errPrimaryCareProviderNPI').text("Rendering provider cannot be same as Primary care Provider");
                    $('#myModalpop').modal('hide');
                    var checklengtherrorPri = $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_errPrimaryCareProviderNPI').length;

                    if (checklengtherrorPri > 0) {
                        $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_hdnErrorMessagePri').val('true');
                    }
                    return;
                }
            }
            if ((referringNPI == primarycareNPI && referringNPI != "") || (RefferingMedID == PrimaryMedID && PrimaryMedID != "")) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblRefProviderMedicaidID').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblReffProviderLastName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblReffProviderFirstName').text('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_divRefErrorMessage').css('display', 'block');
            }
            else {
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_hdnErrorMessageRef').val('');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblReffProviderLastName').text(hdn_ProviderLastName);
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblReffProviderFirstName').text(hdn_ProviderFirstName);
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_divRefErrorMessage').css('display', 'none');
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').removeAttr("disabled");
                $('#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI').css('background', 'white').focus();
            }
        }

        $('#myModalpop').modal('hide');
    }
    function GetDiagnosisCode() {

        var txtDiagnosisCode = $("#<%= txtDiagnosisCodeSearch.ClientID %>").first().val();
        var txtDiagnosisCodedesc = $("#<%= txtDiagnosisCodeDescSearch.ClientID %>").first().val();
        var ddlICDVer = $("#<%=ddlICDVersion.ClientID %>").val();

        $("#<%=gvClaimDiagnosisSearch.ClientID %>").html("");
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetDiagCodeDetails?Code=" + txtDiagnosisCode + "&&DiagVer=" + ddlICDVer + "&&val=" + txtDiagnosisCodedesc,
            //data: '{Code: "' + txtDiagnosisCode + '", DiagVer:"' + ddlICDVer + '", val:"' + txtDiagnosisCodedesc + '"}',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                if (result.length == 0) {

                    $("#<%=gvClaimDiagnosisSearch.ClientID %>").append("<tr><td> No Diagnosis Code Found </td></tr>");
                }
                else {

                    $("#<%=gvClaimDiagnosisSearch.ClientID %>").append("<tr><th>Diagnosis CODE </th><th>Diagnosis Version</th><th>Diagnosis DESCRIPTION </th></tr>");
                    for (var i = 0; i < result.length; i++) {

                        $("#<%=gvClaimDiagnosisSearch.ClientID %>").append("<tr><td><a onClick='GetSelectedRowDiagnosiscode(this); return false;'>" + result[i].Diag_Code + "</a></td><td>" + result[i].Diag_Version + "</td><td>" + result[i].Diag_Desc + "</td></tr>");
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblmessage]").text('Diagnosis Span Code Not Found.');
            }
        });

        return false;
    }

    function GetSelectedRowDiagnosiscode(lnk) {
        $('#myDiagModal').modal('hide');

        $("#<%=hdnDiagnosisCode.ClientID %>").val("");
        $("#<%=hdnDiagnosisVersion.ClientID %>").val("");
        $("#<%=hdnDiagnosisDes.ClientID %>").val("");
        $("#<%=txtDiagnosisCodeSearch.ClientID %>").val("");
        $("#<%=txtDiagnosisCodeDescSearch.ClientID %>").val("");
        var textboxrow = lnk.parentNode.parentNode;
        $("#<%=ucDiagnosisCodePanel.FindControl("txtDiagnosisCode").ClientID %>").val(textboxrow.cells[0].innerText.trim());
        if (textboxrow.cells[1].innerText.trim() == "ICD 9") {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlICDVer").options[2].selected = true;
        } else if (textboxrow.cells[1].innerText.trim() == "ICD 10") {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlICDVer").options[1].selected = true;
        }
        $("#<%=ucDiagnosisCodePanel.FindControl("lblDiagnosisDescription").ClientID %>").html(textboxrow.cells[2].innerText.trim());
        $("#<%=ucDiagnosisCodePanel.FindControl("lblDiagDesc").ClientID %>").val(textboxrow.cells[2].innerText.trim());
        SeqDescChange();
        return false;
    }

    function GetNPIDetails() {
        $("#<%=gvSubmitClaimSearchPage.ClientID %>").html("");
        var txtNPILastName = $("#<%= txtBusinessLastName.ClientID %>").first().val();
        var txtNPIFirstName = $("#<%= txtFirstName.ClientID %>").first().val();
        var txtMedicateId = $("#<%= txtMedicaidid1.ClientID %>").first().val();
        var txtHspscCode = $("#<%= txtHCPCSCode.ClientID %>").first().val();
        var duplicateNpi = localStorage.getItem("DuplicateNpi");
        var search = localStorage.getItem("search");
        var argName = localStorage.getItem("ArgName");

        if (argName == "btnSearchPrimaryCareProvider" && search == "hide") {
            $('#myModal').modal('hide');
            localStorage.setItem("search", "");
            return true;
        }
        else if (duplicateNpi !== null && duplicateNpi !== undefined && duplicateNpi !== "") {

            var arg = localStorage.getItem("ArgName");
            localStorage.setItem("btnArgVal", arg);
            txtHspscCode = duplicateNpi;
            localStorage.setItem("DuplicateNpi", "");
            $('#myModal').modal('show');
        }
        if ($('#myModal').is(':visible')) {
            $("#<%= txtHCPCSCode.ClientID %>").val(duplicateNpi);
            if (txtHspscCode == "" && txtNPIFirstName == "" && txtNPILastName == "" && txtMedicateId == "") {
                $('#ctl00_MainContent_uc5SubmitClaim_lblnpisearchpopupTxTNPI').text("Any one search entry is required");
                return false;
            }
            if (txtHspscCode.length < 10 && txtHspscCode != "" && txtHspscCode != null) {
                $('#ctl00_MainContent_uc5SubmitClaim_lblnpisearchpopupTxTNPI').text("10 digit NPI is required");
                $('#ctl00_MainContent_uc5SubmitClaim_txtHCPCSCode').val('');
                return false;
            }
        }
        if (txtHspscCode.length == 10 || txtNPIFirstName != "" || txtNPILastName != "" || txtMedicateId.length > 0) {
            $("#<%=gvSubmitClaimSearchPage.ClientID %>").html("");
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetNPICodeDetails?firstname=" + txtNPIFirstName + "&&lastname=" + txtNPILastName + "&&Medicateid=" + txtMedicateId + "&&hspsccode=" + txtHspscCode,
                //data: '{firstname: "' + txtNPIFirstName + '" , lastname: "' + txtNPILastName + '" , Medicateid: "' + txtMedicateId + '" , hspsccode: "' + txtHspscCode + '" }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.length == 0) {
                        $("#<%=gvSubmitClaimSearchPage.ClientID %>").append("<tr><td>No Records Found</td></tr>");
                    }
                    else {
                        $("#<%=gvSubmitClaimSearchPage.ClientID %>").append("<tr><th>NPI</th><th>MEDICAID_ID</th><th>BUSINESS/LAST NAME</th><th>FIRST NAME</th><th>ADDRESS LINE 1</th><th>ADDRESS LINE 2</th><th>CITY</th><th>STATE</th><th>ZIP</th></tr>");
                        var medCnt = 0;
                        for (var i = 0; i < result.length; i++) {
                            if (result[i].NPI_Medicateid.length > 0) {
                                medCnt++;
                                $("#<%=gvSubmitClaimSearchPage.ClientID %>").append("<tr><td><a onClick='GetSelectedRowNPIcode(this); return false;'>" + result[i].NPI_Npi + "</asp:LinkButton></td><td><a onClick='GetSelectedRowNPIcode(this); return false;'>" + result[i].NPI_Medicateid + "</asp:LinkButton></td><td>" + result[i].NPI_Lastname + "</td><td>" + result[i].NPI_Firstname + "</td><td>" + result[i].NPI_AddressLine1 + "</td><td>" + result[i].NPI_AddressLine2 + "</td><td>" + result[i].NPI_City + "</td><td>" + result[i].NPI_State + "</td><td>" + result[i].NPI_Zip + "</td></tr>");
                            }
                        };
                        if (medCnt == 0) {
                            $("#<%=gvSubmitClaimSearchPage.ClientID %>").append("<tr><td>No Records Found</td></tr>");
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("#<%=gvSubmitClaimSearchPage.ClientID %>").append("<tr><td>An internal error has occurred, please try your search again.</td></tr>");
                }

            });
            $('#ctl00_MainContent_uc5SubmitClaim_lblnpisearchpopupTxTNPI').text('');
        }

        return false;
    }

    function GetSelectedRowNPIcode(lnk) {
        var textboxrow = lnk.parentNode.parentNode;
        if (textboxrow != null && textboxrow != "" && textboxrow != undefined)
        {
            $('#myModal').modal('hide');
            var btnarg = localStorage.getItem("btnArgVal");
            $("#<%= hdnNPI.ClientID %>").val(textboxrow.cells[0].innerText.trim());
            if ((document.getElementById('<%= hdnNPI.ClientID %>').value == "") && (textboxrow.cells[0].innerText.trim() == "" || textboxrow.cells[0].innerText.trim() == null || textboxrow.cells[0].innerText.trim() == undefined)) {
                $("#<%= hdnMedicaidId.ClientID %>").val(textboxrow.cells[1].innerHTML.trim());
            } else {
                $("#<%= hdnMedicaidId.ClientID %>").val("");
            }
            $("#<%= hdnProviderLastName.ClientID %>").val(textboxrow.cells[2].innerHTML);
            $("#<%= hdnProviderFirstName.ClientID %>").val(textboxrow.cells[3].innerHTML);
            $("#<%= hdnProviderName.ClientID %>").val(textboxrow.cells[3].innerHTML + " " + textboxrow.cells[2].innerHTML);
            $("#<%= hdnProviderAddress.ClientID %>").val(textboxrow.cells[4].innerHTML);
            $("#<%= hdnProviderCity.ClientID %>").val(textboxrow.cells[6].innerHTML);
            $("#<%= hdnProviderState.ClientID %>").val(textboxrow.cells[7].innerHTML);
            $("#<%= hdnProviderZip.ClientID %>").val(textboxrow.cells[8].innerHTML);
            $("#<%= hdnProviderAddress2.ClientID %>").val(textboxrow.cells[5].innerHTML);

            if (btnarg == "btnAttendingPhysician") {
                $("#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI").val(textboxrow.cells[0].innerText.trim());
                if (textboxrow.cells[0].innerText.trim() == '') {
                    $("#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianMedicaidID").html(textboxrow.cells[1].innerText.trim())
                } else {
                    $("#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianMedicaidID").html("")
                }
                $("#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianLastName").html(textboxrow.cells[2].innerHTML)
                $("#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_lblAttendingPhysicianFirstName").html(textboxrow.cells[3].innerHTML)
                $("#<%=ucAttendingPhysicianInformation.FindControl("hdnMedicaidAttendingPhysician").ClientID %>").val(textboxrow.cells[1].innerText.trim())
                $("#<%=ucAttendingPhysicianInformation.FindControl("hdnLastNameAttendingPhysician").ClientID %>").val(textboxrow.cells[2].innerHTML)
                $("#<%=ucAttendingPhysicianInformation.FindControl("hdnFirstNameAttendingPhysician").ClientID %>").val(textboxrow.cells[3].innerHTML)
                $('#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_errAttendingPhysicianNPI').text('');
                clearNdc();
                return false;
            }
            else if (btnarg == "btnSearch6") {
                localStorage.setItem("ReferingProviderNPI", "" + textboxrow.cells[0].innerText.trim() + "");
                $("#<%=ucReferringProvider.FindControl("errReferringProviderNPI").ClientID %>").html("");
                $("#<%=ucReferringProvider.FindControl("txtReferringProviderNPI").ClientID %>").val(textboxrow.cells[0].innerText.trim());
                if (textboxrow.cells[0].innerText.trim() == '')
                {
                    $("#<%=ucReferringProvider.FindControl("lblRefProviderMedicaidID").ClientID %>").text(textboxrow.cells[1].innerText.trim())
                } else
                {
                    $("#<%=ucReferringProvider.FindControl("lblRefProviderMedicaidID").ClientID %>").text("")
                }
                $("#<%=ucReferringProvider.FindControl("hdnRefMedId").ClientID %>").val(textboxrow.cells[1].innerText.trim())
                $("#<%=ucReferringProvider.FindControl("lblReffProviderLastName").ClientID %>").text(textboxrow.cells[2].innerHTML)
                $("#<%=ucReferringProvider.FindControl("lblReffProviderFirstName").ClientID %>").text(textboxrow.cells[3].innerHTML)

                $("#<%=ucReferringProvider.FindControl("hdnRefFirstName").ClientID %>").val(textboxrow.cells[3].innerHTML)
                $("#<%=ucReferringProvider.FindControl("hdnRefLastName").ClientID %>").val(textboxrow.cells[2].innerHTML)
                $("#<%=ucReferringProvider.FindControl("txtPrimaryCareProviderNPI").ClientID %>").prop("disabled", false)
                $("#<%=ucReferringProvider.FindControl("txtPrimaryCareProviderNPI").ClientID %>").css({ "background-color": "" })
                loaderReferringProvider(lnk);
                clearNdc();
                return false;
            }
            else if (btnarg == "btnSearchPrimaryCareProvider") {
                $("#<%=ucReferringProvider.FindControl("txtPrimaryCareProviderNPI").ClientID %>").val(textboxrow.cells[0].innerText.trim());
                if (textboxrow.cells[0].innerText.trim() == '') {
                    $("#<%=ucReferringProvider.FindControl("lbPrimaryCareProvMedicaidID").ClientID %>").html(textboxrow.cells[1].innerText.trim())
                    $("#<%=ucReferringProvider.FindControl("hdnPrimaryRefMedId").ClientID %>").val(textboxrow.cells[1].innerText.trim())
                } else {
                    $("#<%=ucReferringProvider.FindControl("lbPrimaryCareProvMedicaidID").ClientID %>").html("")
                    $("#<%=ucReferringProvider.FindControl("hdnPrimaryRefMedId").ClientID %>").val("")
                }
                $("#<%=ucReferringProvider.FindControl("lblPrimaryCareProvLastName").ClientID %>").html(textboxrow.cells[2].innerText.trim())
                $("#<%=ucReferringProvider.FindControl("lblPrimaryCareProvFirstName").ClientID %>").html(textboxrow.cells[3].innerText.trim())
                $("#<%=ucReferringProvider.FindControl("hdnPrimaryRefFirstName").ClientID %>").val(textboxrow.cells[3].innerHTML)
                $("#<%=ucReferringProvider.FindControl("hdnPrimaryRefLastName").ClientID %>").val(textboxrow.cells[2].innerHTML)
                $("#<%=ucReferringProvider.FindControl("txtPrimaryCareProviderNPI").ClientID %>").prop("disabled", false)
                $("#<%=ucReferringProvider.FindControl("txtPrimaryCareProviderNPI").ClientID %>").css({ "background-color": "" })
                //loaderReferringProvider1(lnk, "false");

                clearNdc();
                return false;
            }
            else if (btnarg == "btnSearch7") {
                localStorage.setItem("RenderingProviderNPI", "" + textboxrow.cells[0].innerText.trim() + "");

                $("#<%=ucRenderingProviderInformationPanel.FindControl("txtRenderingProvNPI").ClientID %>").val(textboxrow.cells[0].innerText.trim());
                if (textboxrow.cells[0].innerText.trim() == '') {
                    $("#<%=ucRenderingProviderInformationPanel.FindControl("lblRenderingMedicaidID").ClientID %>").html(textboxrow.cells[1].innerText.trim())
                    
                } else {
                    $("#<%=ucRenderingProviderInformationPanel.FindControl("lblRenderingMedicaidID").ClientID %>").html("")
                }
                $("#<%=ucRenderingProviderInformationPanel.FindControl("hdnRendMedId").ClientID %>").val(textboxrow.cells[1].innerText.trim())
                $("#<%=ucRenderingProviderInformationPanel.FindControl("lblRenderingLastName").ClientID %>").html(textboxrow.cells[2].innerHTML)
                $("#<%=ucRenderingProviderInformationPanel.FindControl("lblRenderingFirstName").ClientID %>").html(textboxrow.cells[3].innerHTML)
                $("#<%=ucRenderingProviderInformationPanel.FindControl("hdnRendFirstName").ClientID %>").val(textboxrow.cells[3].innerHTML)
                $("#<%=ucRenderingProviderInformationPanel.FindControl("hdnRendLastName").ClientID %>").val(textboxrow.cells[2].innerHTML)
                loaderRenderingProvider(lnk);
                clearNdc();
                return false;
            }

            else if (btnarg == "btnSearch9") {
                $("#<%=ucServiceFacility.FindControl("txtNPIServiceFacilityLocation").ClientID %>").val(textboxrow.cells[0].innerText.trim());
                if (textboxrow.cells[0].innerText.trim() == '') {
                    $("#<%=ucServiceFacility.FindControl("lblServiceFacilityLocationMedicaid").ClientID %>").html(textboxrow.cells[1].innerText.trim())
                    $("#<%=ucServiceFacility.FindControl("hdnServiceFacilityMedId").ClientID %>").val(textboxrow.cells[1].innerText.trim())
                } else {
                    $("#<%=ucServiceFacility.FindControl("lblServiceFacilityLocationMedicaid").ClientID %>").html("")
                    $("#<%=ucServiceFacility.FindControl("hdnServiceFacilityMedId").ClientID %>").val("")
                }
                $("#<%=ucServiceFacility.FindControl("lblServiceFacilityLocationName").ClientID %>").html(textboxrow.cells[2].innerHTML)
                $("#<%=ucServiceFacility.FindControl("lblServiceFacilityLocationAddress1").ClientID %>").html(textboxrow.cells[4].innerHTML)
                $("#<%=ucServiceFacility.FindControl("lblServiceFacilityLocationAddress2").ClientID %>").html(textboxrow.cells[5].innerHTML)
                $("#<%=ucServiceFacility.FindControl("lblServiceFacilityLocationCity").ClientID %>").html(textboxrow.cells[6].innerHTML)
                $("#<%=ucServiceFacility.FindControl("lblServiceFacilityLocationState").ClientID %>").html(textboxrow.cells[7].innerHTML)
                $("#<%=ucServiceFacility.FindControl("lblServiceFacilityLocationZip").ClientID %>").html(textboxrow.cells[8].innerHTML)

                $("#<%=ucServiceFacility.FindControl("hdnServiceFacilityName").ClientID %>").val(textboxrow.cells[2].innerHTML)
                $("#<%=ucServiceFacility.FindControl("hdnServiceFacilityAddress1").ClientID %>").val(textboxrow.cells[4].innerHTML)
                $("#<%=ucServiceFacility.FindControl("hdnServiceFacilityAddress2").ClientID %>").val(textboxrow.cells[5].innerHTML)
                $("#<%=ucServiceFacility.FindControl("hdnServiceFacilityCity").ClientID %>").val(textboxrow.cells[6].innerHTML)
                $("#<%=ucServiceFacility.FindControl("hdnServiceFacilityState").ClientID %>").val(textboxrow.cells[7].innerHTML)
                $("#<%=ucServiceFacility.FindControl("hdnServiceFacilityZip").ClientID %>").val(textboxrow.cells[8].innerHTML)
                clearNdc();
                return false;
            }
            else if (btnarg == "btnAssistantSurgeon") {
                var assSurgNPIvalue = textboxrow.cells[0].innerText.trim();
                localStorage.setItem("AsstProviderNPI", "" + assSurgNPIvalue + "");
                $("#<%=ucAssistantSurgeon.FindControl("txtAssistantSurgeonNPI").ClientID %>").val(assSurgNPIvalue);
                if (textboxrow.cells[0].innerText.trim() == '') {
                    $("#<%=ucAssistantSurgeon.FindControl("lblAssistantSurgeonMedicaidID").ClientID %>").html(textboxrow.cells[1].innerText.trim());
                } else {
                    $("#<%=ucAssistantSurgeon.FindControl("lblAssistantSurgeonMedicaidID").ClientID %>").html("");
                }
                $("#<%=ucAssistantSurgeon.FindControl("lblAssistantSurgeonLastName").ClientID %>").html(textboxrow.cells[2].innerHTML);
                $("#<%=ucAssistantSurgeon.FindControl("lblAssistantSurgeonFirstName").ClientID %>").html(textboxrow.cells[3].innerHTML);
                loaderAssistantSurgeonDental(lnk);
                clearNdc();
                return false;

            }
            else if (btnarg == "btnSearch8") {
                $("#<%=ucSupervisingProviderPanel.FindControl("txtSupervisingProviderNPI").ClientID %>").val(textboxrow.cells[0].innerText.trim());
                if (textboxrow.cells[0].innerText.trim() == '') {
                    $("#<%=ucSupervisingProviderPanel.FindControl("lblSupervisingProviderMediID").ClientID %>").html(textboxrow.cells[1].innerText.trim())
                } else {
                    $("#<%=ucSupervisingProviderPanel.FindControl("lblSupervisingProviderMediID").ClientID %>").html("")
                }
                $("#<%=ucSupervisingProviderPanel.FindControl("lblSupervisingLastName").ClientID %>").html(textboxrow.cells[2].innerHTML)
                $("#<%=ucSupervisingProviderPanel.FindControl("lblSupervisingFirstName").ClientID %>").html(textboxrow.cells[3].innerHTML)

                $("#<%=ucSupervisingProviderPanel.FindControl("hdnMedicaidSuper").ClientID %>").val(textboxrow.cells[1].innerText.trim())
                $("#<%=ucSupervisingProviderPanel.FindControl("hdnLastNameSuper").ClientID %>").val(textboxrow.cells[2].innerHTML)
                $("#<%=ucSupervisingProviderPanel.FindControl("hdnFirstNameSuper").ClientID %>").val(textboxrow.cells[3].innerHTML)
                loaderSupervisingProvider(lnk);
                clearNdc();
                return false;
            }

            else if (btnarg == "btnSearchAdditional") {
                $("#<%=ucAdditionalProviderInfoPanel.FindControl("txtProviderNPI").ClientID %>").val(textboxrow.cells[0].innerText.trim());
                if (textboxrow.cells[0].innerText.trim() == '') {
                    $("#<%=ucAdditionalProviderInfoPanel.FindControl("lblAdditionalMedicaidID").ClientID %>").html(textboxrow.cells[1].innerText.trim())
                } else {
                    $("#<%=ucAdditionalProviderInfoPanel.FindControl("lblAdditionalMedicaidID").ClientID %>").html("")
                }
                $("#<%=ucAdditionalProviderInfoPanel.FindControl("lblProviderLastName").ClientID %>").html(textboxrow.cells[2].innerHTML)
                $("#<%=ucAdditionalProviderInfoPanel.FindControl("lblProviderFirstName").ClientID %>").html(textboxrow.cells[3].innerHTML)
                $("#<%=ucAdditionalProviderInfoPanel.FindControl("hdnAddtionalProviderInfoNPI").ClientID %>").val(textboxrow.cells[1].innerHTML)
                $("#<%=ucAdditionalProviderInfoPanel.FindControl("hdnAdditionalLastName").ClientID %>").val(textboxrow.cells[2].innerHTML)
                $("#<%=ucAdditionalProviderInfoPanel.FindControl("hdnAdditionalFirstName").ClientID %>").val(textboxrow.cells[3].innerHTML)
                clearNdc();
                return false;
            }
        }
    }
    function clearNdc() {
        $('#ctl00_MainContent_uc5SubmitClaim_lblnpisearchpopupTxTNPI').text('');
        $('#ctl00_MainContent_uc5SubmitClaim_txtHCPCSCode').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_txtMedicaidid1').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_txtBusinessLastName').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_txtFirstName').val('');
    }
    function clearGridData() {
        var ddlpayer = $("#<%=ddlDestinationPayerResponsibilitySequence.ClientID %> option:selected").val();
        localStorage.setItem("destinationPayerResponsbilitySequence", "" + ddlpayer + "");
        $('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_providerDiagOutput').innerHTML = '';
        clearTables();
        var value = $("#<%=ddlDestinationPayerID.ClientID %> option:selected").text();

        if ((value != null || value != "" || value != "undefined")) {
            document.getElementById("<%=hdnddlDestinationPayerID.ClientID %>").value = value;
        }
    }
</script>

<style type="text/css">
    .confirm-modal-header{
        padding: 7px;
        border-bottom: 5px solid #25a0da;
        text-align: left;
    }

     .confirm-modal-content {
     -webkit-box-shadow: 0 5px 15px rgba(0,0,0,.5);
     box-shadow: 0 5px 15px rgba(0,0,0,.5);
 }

 .confirm-modal-body {
     position: relative;
     padding: 15px;
 }

 .modal-bodyother {
     height: 185px;
     width: 1337px;
     position: relative;
     padding: 15px;
 }

    .Linkbutton {
        font: bold 17px;
        text-decoration: none !important;
        background-color: #EEEEEE;
        color: #333333;
        padding: 2px 6px 2px 6px;
        border-top: 1px solid #CCCCCC;
        border-right: 1px solid #333333;
        border-bottom: 1px solid #333333;
        border-left: 1px solid #CCCCCC;
    }

    .formField {
        border: 1px solid #ccc;
        background-color: #FFFFFF;
        color: #000;
        width: 274px;
    }

    .formFieldTextBox {
        border: 1px solid #ccc;
        background-color: #FFFFFF;
        color: #000;
        width: 180px !important;
    }

    .formFieldTextBoxSmall {
        border: 1px solid #ccc;
        background-color: #FFFFFF;
        color: #000;
        width: 100px !important;
    }

    .formFieldPos {
        border: 1px solid #ccc;
        background-color: #FFFFFF;
        color: #000;
        width: 90px !important;
    }

    .formFieldcalender {
        border: 1px solid #ccc;
        background-color: #FFFFFF;
        color: #000;
        width: 180px !important;
    }

    .formfieldDate {
        border: 1px solid #ccc;
        background-color: #FFFFFF;
        color: #000;
        width: 110px !important;
    }

    .formfieldMod {
        border: 1px solid #ccc;
        background-color: #FFFFFF;
        color: #000;
        width: 40px !important;
    }





    select {
        min-width: 273px;
        height: 41px;
        border: 1px solid #ccc;
    }

    .formLabel200 {
        width: 178px;
        /*width: 162px;*/
    }

    .col-lg-6 {
        /* width: 50%; */
    }

    .pH2 {
        padding-left: 10px;
        color: white;
    }
    /*.modal-contentprof {
    -webkit-box-shadow: 0 5px 15px rgba(0,0,0,.5);
    box-shadow: 0 5px 15px rgba(0,0,0,.5);
    height: 589px;
    width: max-content;
}*/
    .modal-content {
        -webkit-box-shadow: 0 5px 15px rgba(0,0,0,.5);
        box-shadow: 0 5px 15px rgba(0,0,0,.5);
        height: 589px;
        width: 1300px;
        margin-left: -300px;
    }

    .modal-body {
        height: 385px;
        /*width: 1349px;*/
        position: relative;
        padding: 15px;
    }

    .modal-bodyother {
        height: 185px;
        width: 1337px;
        position: relative;
        padding: 15px;
    }

    .modal-bodydiag {
        height: 204px;
        width: 1334px;
        position: relative;
        padding: 15px;
    }

    .modal-bodytooth {
        height: 182px;
        width: 757px;
        position: relative;
        padding: 15px;
    }

    .modal-header {
        padding: 7px;
        border-bottom: 20px solid #25a0da;
        text-align: -webkit-center;
    }

    #ctl00_MainContent_uc5SubmitClaim_valerrormess br {
        display: none;
    }

    .GvHeaderStyle {
        vertical-align: bottom;
    }

    .popupGridViewOnSearch {
        margin: 20px;
        height: 300px;
        overflow-y: auto;
    }

    .panelTxtboxAlignment {
        background-color: transparent;
        width: 180px
    }

    /*.center {
        margin: 0;
        position: absolute;
        top: 50%;
        left: 50%;
        -ms-transform: translate(-50%, -50%);
        transform: translate(-50%, -50%);
    }*/

    .button {
        font: bold 17px;
        text-decoration: none !important;
        background-color: steelblue;
        color: #FFFFFF;
        padding: 2px 6px 2px 6px;
        border-top: 1px solid #CCCCCC;
        border-right: 1px solid #FFFFFF;
        border-bottom: 1px solid #FFFFFF;
        border-left: 1px solid #CCCCCC;
    }

    .popupGridViewOnSearch {
        margin: 20px;
        height: 300px;
        overflow-y: auto;
    }

    .hideGridColumn {
        display: none;
    }

    .button {
        font: bold 17px;
        text-decoration: none !important;
        background-color: #EEEEEE;
        color: #333333;
        padding: 2px 6px 2px 6px;
        border-top: 1px solid #CCCCCC;
        border-right: 1px solid #333333;
        border-bottom: 1px solid #333333;
        border-left: 1px solid #CCCCCC;
    }

    .box {
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .styleHorizontal {
        overflow-x: hidden;
    }

    .fixed-content {
        height: 10%;
        position: fixed;
        z-index: 1;
        /*top:0;*/
        width: 76%;
        padding: 16px;
        background-color: whitesmoke;
        overflow-y: scroll;
    }

    a.errorlist {
        color: red;
        text-decoration: none;
    }

        a.errorlist:visited {
            color: blue;
            text-decoration: none;
        }
</style>

<div id="modalConfirmYesNo" class="modal fade" data-keyboard="false" data-backdrop="static">
    <div class="modal-dialog">
        <div class="modal-content" style="width:600px;height:175px">
            <div class="modal-header" style="padding:15px;height:50px;text-align:left;border-bottom: 2px solid lightgray;">
                <button type="button"  id="btnModalPopupClose"
                class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
                <h4 id="lblTitleConfirmYesNo" class="modal-title">Confirmation</h4>
            </div>
            <div class="modal-body" style="height:60px;border-bottom: 2px solid lightgray;">
                <p id="lblMsgConfirmYesNo"></p>
            </div>
            <div class="modal-footer">
                <button id="btnYesConfirmYesNo" 
                type="button" style="width:100px;margin-right:20px;" class="btn btn-primary">Yes</button>
                <button id="btnNoConfirmYesNo" style="width:100px;margin-right:20px;"
                type="button" class="btn btn-default">No</button>
            </div>
        </div>
    </div>
</div>

<div>
    <asp:Label runat="server" Text="" ID="adjustInformationMessage" CssClass="failureNotification"></asp:Label>
    <asp:Label runat="server" Text="" ID="lblglobalerror" CssClass="failureNotification"></asp:Label>
</div>
<div class="row">

    <div class="col-sm-8">
        <div class="box">
            <label>Claim&nbsp;Type</label>
        </div>
        <div class="box">

            <asp:RadioButtonList ID="rblClaimType" runat="server" RepeatDirection="Horizontal"
                Style="text-align: center;" aria-label="Claim Type"
                OnSelectedIndexChanged="rblClaimType_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Selected="False" Text="Dental" Value="0"></asp:ListItem>
                <asp:ListItem Selected="False" Text="Institutional" Value="1"></asp:ListItem>
                <asp:ListItem Selected="False" Text="Professional" Value="2"></asp:ListItem>
            </asp:RadioButtonList>

        </div>
    </div>


    <div class="col-sm-4 text-right">
        <div class="row">
            <div class="col-sm-12 text-right">
                <span class="formLabel200" style="text-align: right !important;">Claim Status</span>
                <asp:TextBox ID="txtstatus2" aria-label="Claim Status" runat="server" Style="background-color: lightgrey; width: 44% !important;"
                    ReadOnly="true" />
            </div>
            <div class="col-sm-12 text-right">
                <span class="formLabel200" style="text-align: right !important;">ICN</span>
                <asp:TextBox ID="txtICN2" aria-label="ICN" runat="server" Style="background-color: lightgrey;" CssClass="formField"
                    ReadOnly="true" />
            </div>
            <div class="col-sm-12 text-right">
                <span class="formLabel200" style="text-align: right !important;">Paid Amount</span>
                <asp:TextBox ID="txtPaidAmount" aria-label="Paid Amount" runat="server" Style="background-color: lightgrey;"
                    CssClass="formField" ReadOnly="true" />
            </div>
            <div class="col-sm-12 text-right">
                <span class="formLabel200" style="text-align: right !important;">Adjudication Date</span>
                <asp:TextBox ID="txtAdjDate" runat="server" aria-label="Adjudication Date" Style="background-color: lightgrey;"
                    CssClass="formField" ReadOnly="true" />
            </div>


        </div>
    </div>
</div>
<div id="divErrorList" class="row" style="margin-left: 11px; max-height: 23rem; height: auto; overflow-y: scroll; overflow-x: hidden; display: none">
    <div id="MainErrorContent">
        <asp:ValidationSummary ID="valerrormess" DisplayMode="List" runat="server" CssClass="failureNotification"
            ValidationGroup="validateClaims" />

        <asp:Label runat="server" ID="lblDestinationPayerID" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ddlDestinationPayerID' class='errorlist'>*Destination payer ID is required</a></div>" Visible="false" />
        <asp:Label runat="server" ID="medicaidrequiredError" CssClass="error-message" Text="<div id='msg_txtMedicaidBillingNumber'><a href='#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_txtMedicaidBillingNumber' class='errorlist'>*Medicaid billing number is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="recipientDOB" CssClass="error-message" Text="<div id='msg1_rev_txtBirthDate'><a href='#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_txtBirthDate' class='errorlist'>*Missing Recipient date of birth</div>"
            Visible="false" />
        <asp:Label runat="server" ID="patientControlNoError" CssClass="error-message" Text="<div id='msg2_txtPatientControlNumber'><a href='#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_txtPatientControlNumber' class='errorlist'>*Patient Control Number is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="typeOfBillError" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill' class='errorlist'>*4-digits number is required for Type of Bill</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="releaseOfInfoError" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_ddlReleaseOfInfo' class='errorlist'>* Release of Information is Required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="fromDateRequiredError" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtFromDate' class='errorlist'>*From date is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="toDateRequiredError" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtToDate' class='errorlist'> *Service Information TO date is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="patientStatusError" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_ddlPatientStatus' class='errorlist' >*Patient Status is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="admissionTypeError" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_ddlAdmissionType' class='errorlist'>*Admission Type is required.</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="admitsourceError" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_ddlAdmitSource' class='errorlist'>*Admit source is required.</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="releaseOfInfoErrorDental" CssClass="error-message" Text="<div id='msg3_ddlDentalReleaseofInfo'><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_ddlDentalReleaseofInfo' class='errorlist'>* Release of Information is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="placeOfInformationError" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_txtPlaceofService' class='errorlist'>*Place of Service is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="releaseOfInfoErrorProfessional" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ddlProfessionalDentalReleaseofInfo' class='errorlist'>* Release of Information is required</a></div>"
            Visible="false" />

        <asp:Label runat="server" ID="accidentRelatedToError" CssClass="error-message" Text="<div id='msg_accidentRelatedTo'><a href='#ctl00_MainContent_uc5SubmitClaim_ucAccidentInformation_ddlAccidentrelatedto' class='errorlist'>*Accident related to required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="accidentStateError" CssClass="error-message" Text="<div id='msg_accidentState'><a href='#ctl00_MainContent_uc5SubmitClaim_ucAccidentInformation_ddlAccidentstate' class='errorlist'>*Accident State or Accident Country is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="accidentDateError" CssClass="error-message" Text="<div id='msg_accidentDate'><a href='#ctl00_MainContent_uc5SubmitClaim_ucAccidentInformation_txtAccidentdate' class='errorlist'>*Accident Date required</a></div>"
            Visible="false" />

        <asp:Label runat="server" ID="EPSDTConditionCode" CssClass="error-message" Text="<div id='msg_EPSDTCodeError'><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ddlESPSDTCODEPRofessionalFirst' class='errorlist'>Please select an EPSDT Condition Indicator. If none, choose BLANK.</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="EPSDTConditionIndicator" CssClass="error-message" Text="<div id='msg_EPSDTIndicatorError'><a href='#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationProfessional_ddlProfessionalEPSDTCondition' class='errorlist'>*EPSDT Condition Indicator is required.</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="lblAttending" CssClass="error-message" Text="<div id='msg_AttendingError'><a href='#ctl00_MainContent_uc5SubmitClaim_ucAttendingPhysicianInformation_txtAttendingPhysicianNPI' class='errorlist'>*Attending Physician NPI is required.</a></div>"
            Visible="false" />
        <asp:Label ID="lblServiceDetailsError" runat="server" Text="" CssClass="failureNotification"></asp:Label>
        <asp:Label ID="lblServiceDetailsProfError" runat="server" Text="" CssClass="failureNotification"></asp:Label>
        <asp:Label ID="lblServiceDetailsInsError" runat="server" Text="" CssClass="failureNotification"></asp:Label>
        <asp:Label ID="lblAdditionalPanelError" runat="server" Text="" CssClass="failureNotification"></asp:Label>
        <asp:Label ID="lblServiceInformationErrorFromDOS" runat="server" Text="" CssClass="failureNotification"></asp:Label>
        <asp:Label ID="lblServiceInformationErrorToDOS" runat="server" Text="" CssClass="failureNotification"></asp:Label>
        <asp:Label ID="lblServiceDetailICDError" runat="server" Text="" CssClass="failureNotification"></asp:Label>
        <asp:Label ID="lblServiceDetailICDProcedureError" runat="server" Text="" CssClass="failureNotification"></asp:Label>
        <asp:Label ID="lblDiagnosisRequired" runat="server" Text="" CssClass="failureNotification"></asp:Label>
        <asp:Label runat="server" ID="ErrorMessage1" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence' class='errorlist'>*Other payer with primary payer responsibility sequence is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="ErrorMessage2" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence' class='errorlist'>*Other payer with Secondary payer responsibility sequence is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="ErrorMessage3" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence' class='errorlist'>*Other payer with Tertiary payer responsibility sequence is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="ErrorMessage4" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence' class='errorlist'>*Other payer with Payer Responsibility Four is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="ErrorMessage5" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence' class='errorlist'>*Other payer with Payer Responsibility Five is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="ErrorMessage6" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence' class='errorlist'>*Other payer with Payer Responsibility Six is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="ErrorMessage7" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence' class='errorlist'>*Other payer with Payer Responsibility Seven is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="ErrorMessage8" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence' class='errorlist'>*Other payer with Payer Responsibility Eight is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="ErrorMessage9" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence' class='errorlist'>*Other payer with Payer Responsibility Nine is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="ErrorMessage10" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence' class='errorlist'>*Other payer with Payer Responsibility Ten is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="lblpickupAddressline1" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_txtBoxPickupAddressLine1' class='errorlist'>*Ambulance pick-up address is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="lblPickUpCity" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_txtBoxPickUpCity' class='errorlist'>*Ambulance pick-up city is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="lblPickUpCitylength" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_txtBoxPickUpCity' class='errorlist'>*Pick-up address is invalid</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="lblPickUpState" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_ddlPickupState' class='errorlist'>*Ambulance pick-up State is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="lblPickUPZipCode" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_txtBoxPickUpZip' class='errorlist'>*Ambulance pick-up ZIP is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="lblPickUpZipcodeLength" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_txtBoxPickUpZip' class='errorlist'>*Pick up ZIP code is invalid</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="lblDropupAddressLine1" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_txtBoxDropOffAddressLine1' class='errorlist'>*Ambulance drop-off address is required</a></div>"
            Visible="false" />

        <asp:Label runat="server" ID="lblDropOffCity" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_txtBoxDropOffCity' class='errorlist'>*Ambulance drop-off city is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="lblDropOffCitylength" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_txtBoxDropOffCity' class='errorlist'>*Drop-off address is invalid</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="lblDropOffState" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_ddlDropOffState' class='errorlist'>*Ambulance drop-off state is required</a></div>"
            Visible="false" />

        <asp:Label runat="server" ID="lblDropOffZIP" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_txtBoxDropOffZip' class='errorlist'>*Ambulance drop-off ZIP is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="lblDropOffZIPLength" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_txtBoxDropOffZip' class='errorlist'>*Ambulance drop-off ZIP is invalid</a></div>"
            Visible="false" />

        <asp:Label runat="server" ID="lblTransportDistance" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_txtTransportDistance' class='errorlist'>*Transport distance is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="lblTransportReasonCode" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_ddlTransportReasonCode' class='errorlist'>*Transportation reason code is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="lblConditionIndicator" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_ddlConditionIndicator' class='errorlist'>*Condition indicator is required</a></div>"
            Visible="false" />
        <asp:Label runat="server" ID="lblConditionCode1" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucAmbulanceInformation_ddlConditionCode1' class='errorlist'>*At least one condition code is required</a></div>"
            Visible="false" />
    </div>

    <asp:Label runat="server" ID="lblAdmittingrequiredError" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_lblPresentOnAdmissionEror' class='errorlist'>*Admitting diagnosis code is required</a></div>"
        Visible="false" />
    <asp:Label runat="server" ID="lblAdmittinginappropriateError" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_lblPresentOnAdmissionEror' class='errorlist'>*Admitting diagnosis code is inappropriate for this type of bill</a></div>"
        Visible="false" />
    <asp:Label runat="server" ID="lblPatientrequiredError" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_lblIcdVersionError' class='errorlist'>*Patient reason for visit is required</a></div>"
        Visible="false" />
    <asp:Label runat="server" ID="lblPatientinappropriateError" CssClass="error-message" Text="<div><a href='#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_lblIcdVersionError' class='errorlist'>*Patient reason for visit is inappropriate for this type of bill</a></div>"
        Visible="false" />
</div>
<div class="row" style="margin-left: 11px; max-height: 23rem; height: auto; overflow-y: scroll; overflow-x: hidden;">
    <asp:Label runat="server" ID="lblOtherPayerAdjustmentGridError" CssClass="error-message" Text=""
        Visible="false" />
    <asp:Label runat="server" ID="lblAdmissionDateServiceInfoGridError" CssClass="error-message" Text=""
        Visible="false" />
</div>
<br />
<span style="color: #CC0505; font-size: 14pt !important; font-weight: 100 !important;">An asterisk * indicates a required field</span>
<div class="row">
    <div class="col-sm-4">
        <div class="row" id="DivDestinationPayer" runat="server">
            <div class="col-sm-5" style="text-align: right">
                <span class="hio-field-label GridviewHeaderAsterisk" style="font-size: 16px; text-align: right">Destination Payer Name</span>
            </div>
            <div class="col-sm-7">
                <span style="text-align: left;">
                    <asp:DropDownList ID="ddlPrimaryDestinationPayer" EnableViewState="true" runat="server" Enabled="false"
                        AppendDataBoundItems="False" Style="height: 30px; width: 200px; min-width: 80px;"
                        ValidationGroup="validateClaims" onChange="GetDestinationPayerIDforDestinationName();">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="rfPrimaryDestiantionPayer" SetFocusOnError="true"
                        ValidationGroup="validateClaims" ControlToValidate="ddlPrimaryDestinationPayer"
                        ErrorMessage="<div>*Destination payer is required</div>" Text="" Display="Dynamic"
                        InitialValue="" CssClass="failureNotification" />
                </span>
            </div>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="row" id="divDestinationPayerID" runat="server">
            <div class="col-sm-5" style="text-align: right">
                <span class="hio-field-label GridviewHeaderAsterisk" style="font-size: 16px; text-align: right">Destination Payer ID</span>
            </div>

            <div class="col-sm-7">
                <span style="text-align: left;">
                    <asp:DropDownList ID="ddlDestinationPayerID" EnableViewState="true" runat="server" Enabled="false"
                        AppendDataBoundItems="False" Style="height: 30px; width: 200px; min-width: 80px;"
                        ValidationGroup="validateClaims" OnChange="storevalue();">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="rfdDestinationPayerID" SetFocusOnError="true"
                        ValidationGroup="validateClaims" ControlToValidate="ddlPrimaryDestinationPayer"
                        ErrorMessage="<div>*Destination payer is required</div>" Display="Dynamic"
                        InitialValue="" CssClass="failureNotification" />
                </span>
                <asp:HiddenField ID="hdnddlDestinationPayerID" runat="server" />
            </div>
        </div>
    </div>
    <div class="col-sm-4">
        <div class="row" id="Div8" runat="server">
            <div class="col-sm-5" style="text-align: right">
                <span class="hio-field-label GridviewHeaderAsterisk" style="font-size: 16px; text-align: right">Destination Payer
                    Responsibility Sequence</span>
            </div>


            <div class="col-sm-7">
                <span style="text-align: left;">
                    <asp:DropDownList ID="ddlDestinationPayerResponsibilitySequence" EnableViewState="true"
                        runat="server"
                        AppendDataBoundItems="False" Style="height: 30px; width: 200px; min-width: 80px;"
                        ValidationGroup="validateClaims"
                        OnSelectedIndexChanged="ddlDestinationPayerResponsibilitySequence_SelectedIndexChanged"
                        Enabled="false" onChange="clearGridData();"
                        AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="rfvDestinationPayerRespSeq" SetFocusOnError="true"
                        ValidationGroup="validateClaims" ControlToValidate="ddlDestinationPayerResponsibilitySequence"
                        ErrorMessage="<div>*Destination payer responsibility sequence is required</div>"
                        Text="*"
                        Display="Dynamic" InitialValue="" CssClass="failureNotification" />

                </span>
            </div>
        </div>
    </div>

</div>
<asp:HiddenField ID="hdnClaimId" runat="server" />
<asp:HiddenField ID="hdnPreviousClaimId" runat="server" />
<asp:HiddenField ID="hdnFrequencyCode" runat="server" Value="1" />
<asp:HiddenField ID="hdnDestinationPayerId" runat="server" />
<asp:HiddenField ID="payerID" runat="server" />
<asp:HiddenField ID="payerIDtemp" runat="server" />
<asp:HiddenField ID="hdnErrorMsg" runat="server" />


<asp:HiddenField ID="RegIdTxt" runat="server" />


    <div class="row col-sm-13" style="border: groove; margin-left: 10px" id="mainDiv">
    <div runat="server" id="divallPanels" visible="false">

        <div id="divPanels" style="overflow: hidden">

            <ajax:collapsiblepanelextender id="cpeRecipient" runat="server" collapsed="false"
                targetcontrolid="PanelRecipient_UC" expandcontrolid="pnlsepRecipient2" collapsecontrolid="pnlsepRecipient2" />

            <asp:Panel runat="server" ID="pnlsepRecipient2" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerRecipientInformation2">
                <span id="sepRecipient2" runat="server" class="pageHeader pH2">-
                </span><b class="pageHeader pH2 GridviewHeaderAsterisk">RECIPIENT INFORMATION</b>
            </asp:Panel>
            <asp:Panel ID="PanelRecipient_UC" runat="server" class="styleHorizontal">
                <div class="row">
                    <uc:recipientinformationpanel runat="server" id="ucRecipientInformationPanel" />
                </div>
            </asp:Panel>
            <asp:UpdatePanel ID="UpdatePanelServiceInformationInstitutional" runat="server">
                <ContentTemplate>
                    <ajax:collapsiblepanelextender id="cpeServiceInfo" runat="server" collapsed="false"
                        targetcontrolid="pnlServiceInfo" expandcontrolid="pnlsepServiceInfo" collapsecontrolid="pnlsepServiceInfo"
                        suppresspostback="true" behaviorid="cpeServiceInfo" />
                    <asp:Panel runat="server" ID="pnlsepServiceInfo" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                        Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerServiceInfo">
                        <%--      <span id="sepServiceInfo" runat="server" class="pageHeader pH2">- *SERVICE INFORMATION </span>--%>
                        <span id="sepServiceInfo" runat="server" class="pageHeader pH2">-
                        </span><b class="pageHeader pH2 GridviewHeaderAsterisk">SERVICE INFORMATION </b>
                    </asp:Panel>
                    <asp:Panel ID="pnlServiceInfo" runat="server" Style="min-height: 400px; min-width: 600px; height: 290px; width: auto; max-width: 1200px;">

                        <%-- Institutional claim --%>

                        <uc:serviceinformationinstitutional runat="server" id="ucServiceInformationInstitutional" />

                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

            <%--Institutional TypeofBill Search --%>
            <asp:Panel runat="server" ID="pnlseptypebillsearch" class="CollapsingSeparator" Visible="false"
                onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse"
                CssClass="Ownertypebillsearch">
                <%-- <div class="pageHeader pH2" id="sepServiceDetail" runat="server" style="background-color: cornflowerblue; padding-left: 10px">SERVICE DETAILS</div>--%>
                <span id="septypebillsearch" runat="server" class="pageHeader pH2">Search</span>
            </asp:Panel>
            <asp:Panel ID="pnltypebillsearch" Visible="false" runat="server" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">
                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <div class="row" style="text-align: center;">
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Type of Bill</b>
                                <asp:TextBox ID="txttypeofbillsearch" CssClass="ohio-field-input" runat="server">
                                </asp:TextBox>

                            </span>

                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Type of Bill Description</b>
                                <asp:TextBox ID="txttypeofbillDescsearch" CssClass="ohio-field-input" runat="server">
                                </asp:TextBox>
                                <asp:LinkButton ID="lnktypeofbillsearch" Text="Search" runat="server" ToolTip="Search"
                                    OnClick="lnktypeofbillsearch_Click" Visible="true">

                                </asp:LinkButton>
                            </span>

                        </div>
                    </div>
                    <div class="divtypeofbillsearch" style="padding-top: 10px">
                        <asp:GridView ID="gvtypeofbillsearch" runat="server" Width="80%" AllowSorting="false"
                            CssClass="gridview"
                            EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdtypeofbillsearch_SelectedIndexChanged"
                            Style="margin-right: 100px">

                            <Columns>
                                <asp:BoundField DataField="Code" HeaderText="Code" />
                                <asp:BoundField DataField="Placeofname" HeaderText="Place Of Service Name" />

                            </Columns>
                            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                            <AlternatingRowStyle CssClass="gridViewAltRow" />
                            <RowStyle CssClass="gridViewRow" />
                            <FooterStyle CssClass="gridViewFooter" />
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>

            <div id="dvDentalServiceInformation" runat="server">
                <ajax:collapsiblepanelextender id="cpeDentalServiceInformation" runat="server" collapsed="false"
                    targetcontrolid="pnlDentalServiceInformationSep" expandcontrolid="pnlDentalServiceInformation"
                    collapsecontrolid="pnlDentalServiceInformation" />

                <asp:Panel runat="server" ID="pnlDentalServiceInformation" class="CollapsingSeparator"
                    onclick="javascript:CollapseExpand(this);"
                    Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerServiceInfo">
                    <%--<span id="sepDentalService" runat="server" class="pageHeader pH2">- *Dental SERVICE INFORMATION </span>--%>
                    <span id="Span3" runat="server" class="pageHeader pH2">-</span>
                    <b class="pageHeader pH2 GridviewHeaderAsterisk">SERVICE INFORMATION </b>
                </asp:Panel>
                <asp:Panel ID="pnlDentalServiceInformationSep" runat="server" Style="overflow-x: hidden; height: 80%">
                    <div class="container-fluid">
                        <div class="row">
                            <uc:serviceinformationdental runat="server" id="ucServiceInformationDental" />
                        </div>

                    </div>

                </asp:Panel>

            </div>

            <%-- DenatalSearch --%>
            <asp:Panel runat="server" ID="pnlSepPlaceofServiceSearch" class="CollapsingSeparator"
                Visible="false" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse"
                CssClass="OwnerDiagnosis">

                <span id="SepPlaceofServiceSearch" runat="server" class="pageHeader pH2">Search</span>
            </asp:Panel>
            <asp:Panel ID="pnlPlaceofServiceSearch" Visible="false" runat="server" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">
                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <div class="row" style="text-align: center;">
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Code</b>
                                <asp:TextBox ID="txtCode" CssClass="ohio-field-input" runat="server">
                                </asp:TextBox>

                            </span>

                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Place of Service Name</b>
                                <asp:TextBox ID="txtPlaceOfServiceName" CssClass="ohio-field-input" runat="server">
                                </asp:TextBox>
                                <asp:LinkButton ID="lnkPLaceServiceName" Text="Search" runat="server" ToolTip="Search"
                                    Visible="true">

                                </asp:LinkButton>
                            </span>

                        </div>
                    </div>
                    <div class="divPlaceofServiceSearch" style="padding-top: 10px">
                        <asp:GridView ID="gvPlaceofServiceSearch" runat="server" Width="80%" AllowSorting="false"
                            CssClass="gridview"
                            EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left"
                            Style="margin-right: 100px">

                            <Columns>
                                <asp:BoundField DataField="Code" HeaderText="Code" />
                                <asp:BoundField DataField="Placeofname" HeaderText="Place Of Service Name" />

                            </Columns>
                            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                            <AlternatingRowStyle CssClass="gridViewAltRow" />
                            <RowStyle CssClass="gridViewRow" />
                            <FooterStyle CssClass="gridViewFooter" />
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>
            <ajax:collapsiblepanelextender id="cpeProfessionalclaim" runat="server" collapsed="false"
                targetcontrolid="pnlProfessionalclaim" expandcontrolid="pnlsepProfessionalclaim"
                collapsecontrolid="pnlsepProfessionalclaim" suppresspostback="true" behaviorid="cpeProfessionalclaim" />
            <asp:Panel runat="server" ID="pnlsepProfessionalclaim" class="CollapsingSeparator"
                onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerServiceInfo">
                <span id="sepProfessionalclaim" runat="server" class="pageHeader pH2">-</span><b class="pageHeader pH2 GridviewHeaderAsterisk">SERVICE INFORMATION </b>
            </asp:Panel>
            <asp:Panel ID="pnlProfessionalclaim" runat="server" Style="min-height: 330px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">
                <%-- Professional claim --%>
                <uc:serviceinformationprofessional runat="server" id="ucServiceInformationProfessional" />
            </asp:Panel>

            <div>
                <ucaccident:accidentinformation runat="server" id="ucAccidentInformation" />
            </div>

            <%-- For Every one Dental,Professional claim, Institutional claim --%>
            <ajax:collapsiblepanelextender id="cpePriorAuthInfo" runat="server" collapsed="true"
                targetcontrolid="pnlPriorAuthInfo" expandcontrolid="pnlsepPriorAuthInfo" collapsecontrolid="pnlsepPriorAuthInfo" />
            <asp:Panel runat="server" ID="pnlsepPriorAuthInfo" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerPriorAuthInfo">
                <span id="sepPriorAuthInfo" runat="server" class="pageHeader pH2">+ PRIOR AUTHORIZATION
                & REFERRAL INFORMATION</span>
            </asp:Panel>
            <asp:Panel ID="pnlPriorAuthInfo" runat="server" Style="overflow-x: hidden;">
                <div class="row">
                    <uc:priorauthorizationandreferringpanel runat="server" id="ucPriorAuthorizationAndReferringPanel" />
                </div>
            </asp:Panel>

            <%-- Attending Physician Information Screen - Institutional Claims--%>

            <ajax:collapsiblepanelextender id="cpeAttendingPhysicianInfo" runat="server" collapsed="true"
                targetcontrolid="pnlAttendingPhysicianInfo" expandcontrolid="pnlsepAttendingPhysicianInfo"
                collapsecontrolid="pnlsepAttendingPhysicianInfo" />
            <asp:Panel runat="server" ID="pnlsepAttendingPhysicianInfo" class="CollapsingSeparator"
                onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerRecipientInformation2">
                <span id="sepAttendingPhysicianInfo" runat="server" class="pageHeader pH2">+ ATTENDING
                PHYSICIAN INFORMATION</span>
            </asp:Panel>
            <asp:Panel ID="pnlAttendingPhysicianInfo" runat="server" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; overflow-x: hidden;">
                <div class="container-fluid">
                    <div class="row">
                        <uc:attendingphysicianinformation runat="server" id="ucAttendingPhysicianInformation" />
                    </div>
                </div>
            </asp:Panel>

            <%-- Institutional claim Screen --%>

            <ajax:collapsiblepanelextender id="cpeOtherProviderInfo" runat="server" collapsed="true"
                targetcontrolid="pnlOtherProviderInfo" expandcontrolid="pnlsepOtherProviderInfo"
                collapsecontrolid="pnlsepOtherProviderInfo" />
            <asp:Panel runat="server" ID="pnlsepOtherProviderInfo" class="CollapsingSeparator"
                onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerOtherProviderInfo">
                <span id="sepOtherProviderInfo" runat="server" class="pageHeader pH2">+ OTHER PROVIDER
                INFORMATION </span>
            </asp:Panel>
            <asp:Panel ID="pnlOtherProviderInfo" runat="server" Style="min-height: 180px; min-width: 300px; height: auto; width: auto; max-width: 98%; margin-left: 10px;">
                <div style="text-align: center;" hidden="hidden">
                    <div class="divGrid" style="padding-top: 10px">
                        <asp:GridView ID="gvOtherProviderInfo" runat="server" Width="80%" AllowSorting="false"
                            CssClass="gridview"
                            EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdOtherProviderInfo_SelectedIndexChanged"
                            Style="margin-right: 100px">
                            <Columns>
                                <%--<asp:BoundField DataField="Line" HeaderText="Line" />--%>
                                <asp:BoundField DataField="num_dtl_total" HeaderText="Detail" />
                                <asp:BoundField DataField="" HeaderText="Procedure Code" />
                                <asp:BoundField DataField="" HeaderText="*Health Plan ID" />
                                <asp:BoundField DataField="" HeaderText="*Amount Paid" />
                                <asp:BoundField DataField="" HeaderText="Paid Date" />
                                <asp:BoundField DataField="" HeaderText="Adjustment Group" />
                                <asp:BoundField DataField="" HeaderText="Reason Code" />
                                <asp:BoundField DataField="" HeaderText="Amount" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDeleteOp" runat="server" CommandName="PendingServiceDetails"
                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                            ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');"
                                            AlternateText="Delete"
                                            Visible="<%# CanUserViewDelete()  %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                            <AlternatingRowStyle CssClass="gridViewAltRow" />
                            <RowStyle CssClass="gridViewRow" />
                            <FooterStyle CssClass="gridViewFooter" />
                        </asp:GridView>
                    </div>

                </div>
                <div class="row" style="text-align: center;" hidden="hidden">
                    <asp:ValidationSummary ID="vsOtherProviderInfo" DisplayMode="List" runat="server"
                        CssClass="failureNotification" ValidationGroup="vgOtherProviderInfo" />
                    <div class="col-sm-6">
                        <div class="row" id="OtherProviderInfo" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Other Physician</span>
                            </div>
                            <div class="col-sm-7 text-left">
                                <asp:DropDownList ID="ddlOtherPhysician" CssClass="formField" EnableViewState="true"
                                    runat="server"
                                    AppendDataBoundItems="True">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" SetFocusOnError="true"
                                    ValidationGroup="validateClaims" ControlToValidate="ddlOtherPhysician" ErrorMessage="*"
                                    Text="*" Display="Dynamic" InitialValue="0" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="MedicaidID" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Medicaid ID</span>
                            </div>
                            <div class="col-sm-7 text-left">
                                <asp:TextBox ID="txtMedicaidID" runat="server" CssClass="formField" MaxLength="10"
                                    Style="background-color: lightgrey;" ReadOnly="true" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="ProviderName1" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Name</span>
                            </div>
                            <div class="col-sm-7 text-left">
                                <asp:TextBox ID="txtProviderName1" runat="server" CssClass="formField" MaxLength="35"
                                    Style="background-color: lightgrey;" ReadOnly="true" />
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-6">
                        <div class="div OtherProviderInfoAdd">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">&nbsp;</span>
                            </div>
                            <div class="col-sm-7 text-left">
                                <asp:Button ID="OtherProviderInfoAdd" Text="ADD" runat="server" OnClick="OtherProviderInfoAdds_Click"
                                    CssClass="btn btn-danger" Font-Bold="True" CausesValidation="True" Width="90px" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" style="background-color: lightsteelblue; margin-top: 0px; height: 30px">
                    <div class="col-sm-2"></div>
                    <div class="col-sm-2">
                        <span class="ohio-field" style="font-size: 15px;"><strong>Physician
                Type</strong></span>
                    </div>
                    <div class="col-sm-3">
                        <span class="ohio-field" style="font-size: 15px;"><strong>Provider
                NPI</strong></span>
                    </div>
                    <div class="col-sm-2">
                        <span class="ohio-field" style="font-size: 15px;"><strong>Medicaid
                ID</strong></span>
                    </div>
                    <div class="col-sm-2">
                        <span class="ohio-field" style="font-size: 15px;"><strong>Provider
                Name</strong></span>
                    </div>
                    <div class="col-sm-1">&nbsp;</div>
                </div>

                <div class="row">
                    <asp:UpdatePanel ID="panelNPI1" runat="server">
                        <ContentTemplate>
                            <div class="col-sm-2"></div>
                            <div class="col-sm-2">
                                <span class="ohio-field" style="font-size: 15px;">Atending</span>
                            </div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtNPI1" runat="server" CssClass="formField" MaxLength="10" Style="height: 30px; width: 100px"
                                    OnTextChanged="txtNPI1_TextChanged1" AutoPostBack="True"></asp:TextBox>
                                <button type="button" id="btnSearch1" onclick="getbuttondetail(this)" class="btn btn-link"
                                    data-toggle="modal" data-target="#myModal">
                                    Search
                                </button>

                                <asp:RequiredFieldValidator ID="rfvNPI1" runat="server" ControlToValidate="txtNPI1"
                                    ErrorMessage="*Other provider is unknown" Text="*" Display="Dynamic" ValidationGroup="valReferringProviderNPI"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ID="revNPI1"
                                    Display="Dynamic"
                                    ValidationGroup="valReferringProviderNPI"
                                    ControlToValidate="txtNPI1"
                                    ValidationExpression="^[0-9]{10}$"
                                    ErrorMessage="10-digit number is required">	
                                </asp:RegularExpressionValidator>

                                <asp:Label ID="errNPI1" Style="color: red" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtOPMID1" runat="server" ReadOnly="true" Style="background-color: lightgray; width: 100px;" />
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtOPName1" runat="server" ReadOnly="true" Style="background-color: lightgray"></asp:TextBox>
                            </div>
                            <div class="col-sm-1"></div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <div class="row">
                    <asp:UpdatePanel ID="panelNPI2" runat="server">
                        <ContentTemplate>
                            <div class="col-sm-2"></div>
                            <div class="col-sm-2">
                                <span class="ohio-field" style="font-size: 15px;">Rendering</span>
                            </div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtNPI2" runat="server" CssClass="formField" MaxLength="10" Style="height: 30px; width: 100px"
                                    OnTextChanged="txtNPI2_TextChanged2" AutoPostBack="True"></asp:TextBox>

                                <button type="button" id="btnSearch2" onclick="getbuttondetail(this)" class="btn btn-link"
                                    data-toggle="modal" data-target="#myModal">
                                    Search
                                </button>

                                <asp:RequiredFieldValidator ID="rfvNPI2" runat="server" ControlToValidate="txtNPI1"
                                    ErrorMessage="*NPI is unknown" Text="*" Display="Dynamic" ValidationGroup="valReferringProviderNPI"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator5"
                                    Display="Dynamic"
                                    ValidationGroup="valReferringProviderNPI"
                                    ControlToValidate="txtNPI2"
                                    ValidationExpression="^[0-9]{10}$"
                                    ErrorMessage="10-digit number is required">	
                                </asp:RegularExpressionValidator>

                                <asp:Label ID="errNPI2" Style="color: red" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtOPMID2" runat="server" ReadOnly="true" Style="background-color: lightgray; width: 100px;" />
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtOPName2" runat="server" ReadOnly="true" Style="background-color: lightgray"></asp:TextBox>
                            </div>
                            <div class="col-sm-1"></div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <asp:UpdatePanel ID="panelLine123" runat="server">
                    <ContentTemplate>
                        <div class="row" id="DivLine1" runat="server" visible="false">
                            <div class="col-sm-2" style="text-align: right;">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Other Physician</span>
                            </div>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="ddlOP1" runat="server" Style="min-width: 150px; height: 30px"
                                    Width="150px">
                                    <asp:ListItem Value="0">Referring</asp:ListItem>
                                    <asp:ListItem Value="1">Operating</asp:ListItem>
                                    <asp:ListItem Value="2">Other Operating</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtNPI1add" runat="server" CssClass="formField" MaxLength="10" Style="height: 30px; width: 100px"
                                    OnTextChanged="txtNPI1add_TextChanged1" AutoPostBack="True"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNPI1add" runat="server" ControlToValidate="txtNPI1add"
                                    ErrorMessage="*NPI is unknown" Text="*" Display="Dynamic" ValidationGroup="valReferringProviderNPI"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ID="revNPI1add"
                                    Display="Dynamic"
                                    ValidationGroup="valReferringProviderNPI"
                                    ControlToValidate="txtNPI1add"
                                    ValidationExpression="^[0-9]{10}$"
                                    ErrorMessage="10-digit number is required">	
                                </asp:RegularExpressionValidator>
                                <%--<asp:LinkButton ID="lnkOPNPI1" runat="server" ToolTip="Search" Text="Search" CommandArgument='<%# Eval("CDE_POS") %>' OnClick="lnkSearch_Click" Visible="true" style="font-size: 15px;"/>    --%>
                                <button type="button" class="btn btn-link" id="btnSearch3" onclick="getbuttondetail(this)"
                                    data-toggle="modal" data-target="#myModal">
                                    Search
                                </button>

                                <asp:Label ID="errNPI1add" Style="color: red" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtOPMID1add" runat="server" ReadOnly="true" Style="background-color: lightgray; width: 100px;" />
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtOPName1add" runat="server" ReadOnly="true" Style="background-color: lightgray"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="btnOPdelete1" Text="Delete" runat="server" CssClass="btn btn-danger"
                                    Font-Bold="True" CausesValidation="True" Width="90px" OnClick="btnOPdelete1_Click" />
                            </div>
                        </div>

                        <div class="row" id="DivLine2" runat="server" visible="false">
                            <div class="col-sm-2" style="text-align: right;">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Other Physician</span>
                            </div>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="ddlOP2" runat="server" Style="min-width: 150px; height: 30px"
                                    Width="150">
                                    <asp:ListItem>Referring</asp:ListItem>
                                    <asp:ListItem Selected="True">Operating</asp:ListItem>
                                    <asp:ListItem>Other Operating</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtNPI2add" runat="server" CssClass="formField" MaxLength="10" Style="height: 30px; width: 100px"
                                    OnTextChanged="txtNPI2add_TextChanged2" AutoPostBack="True"></asp:TextBox>
                                <%--                <asp:LinkButton ID="lnkOPNPI2" runat="server" ToolTip="Search" Text="Search" CommandArgument='<%# Eval("CDE_POS") %>' OnClick="lnkSearch_Click" Visible="true" style="font-size: 15px;"/>      
                                --%>
                                <button type="button" class="btn btn-link" id="btnSearch4" onclick="getbuttondetail(this)"
                                    data-toggle="modal" data-target="#myModal">
                                    Search
                                </button>
                                <asp:RequiredFieldValidator ID="rfvNPI2add" runat="server" ControlToValidate="txtNPI1add"
                                    ErrorMessage="*NPI is unknown" Text="*" Display="Dynamic" ValidationGroup="valReferringProviderNPI"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ID="revNPI2add"
                                    Display="Dynamic"
                                    ValidationGroup="valReferringProviderNPI"
                                    ControlToValidate="txtNPI2add"
                                    ValidationExpression="^[0-9]{10}$"
                                    ErrorMessage="10-digit number is required">	
                                </asp:RegularExpressionValidator>

                                <asp:Label ID="errNPI2add" Style="color: red" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtOPMID2add" runat="server" ReadOnly="true" Style="background-color: lightgray; width: 100px;"
                                    AutoPostBack="True" />
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtOPName2add" runat="server" ReadOnly="true" Style="background-color: lightgray"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="btnOPdelete2" Text="Delete" runat="server" CssClass="btn btn-danger"
                                    Font-Bold="True" CausesValidation="True" Width="90px" OnClick="btnOPdelete2_Click" />
                            </div>
                        </div>

                        <div class="row" id="DivLine3" runat="server" visible="false">
                            <div class="col-sm-2" style="text-align: right;">
                                <span class="ohio-field" style="font-size: 15px; text-align: right; height: 30px">Other
                                Physician</span>
                            </div>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="ddlOP3" runat="server" Style="min-width: 150px; height: 30px"
                                    Width="150">
                                    <asp:ListItem>Referring</asp:ListItem>
                                    <asp:ListItem>Operating</asp:ListItem>
                                    <asp:ListItem Selected="True">Other Operating</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtNPI3add" runat="server" CssClass="formField" MaxLength="10" Style="height: 30px; width: 100px"
                                    OnTextChanged="txtNPI3add_TextChanged3" AutoPostBack="True"></asp:TextBox>
                                <%-- <asp:LinkButton ID="lnkOPNPI3" runat="server" ToolTip="Search" Text="Search" CommandArgument='<%# Eval("CDE_POS") %>' OnClick="lnkSearch_Click" Visible="true" style="font-size: 15px;"/>  --%>
                                <button type="button" class="btn btn-link" id="btnSearch5" onclick="getbuttondetail(this)"
                                    data-toggle="modal" data-target="#myModal">
                                    Search
                                </button>
                                <asp:RequiredFieldValidator ID="rfvNPI3add" runat="server" ControlToValidate="txtNPI3add"
                                    ErrorMessage="*NPI is unknown" Text="*" Display="Dynamic" ValidationGroup="valReferringProviderNPI"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator runat="server" ID="revNPI3add"
                                    Display="Dynamic"
                                    ValidationGroup="valReferringProviderNPI"
                                    ControlToValidate="txtNPI3add"
                                    ValidationExpression="^[0-9]{10}$"
                                    ErrorMessage="10-digit number is required">	
                                </asp:RegularExpressionValidator>

                                <asp:Label ID="errNPI3add" Style="color: red" runat="server"></asp:Label>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtOPMID3add" runat="server" ReadOnly="true" Style="background-color: lightgray; width: 100px;" />
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtOPName3add" runat="server" ReadOnly="true" Style="background-color: lightgray"></asp:TextBox>
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="btnOPdelete3" Text="Delete" runat="server" CssClass="btn btn-danger"
                                    Font-Bold="True" CausesValidation="True" Width="90px" OnClick="btnOPdelete3_Click" />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-sm-2"></div>
                            <div class="col-sm-2"></div>
                            <div class="col-sm-3"></div>
                            <div class="col-sm-2"></div>
                            <div class="col-sm-2"></div>
                            <div class="col-sm-1">
                                <asp:Button ID="btnOPadd" Text="Add" runat="server" CssClass="btn btn-primary" Font-Bold="True"
                                    CausesValidation="True" Width="90px" UseSubmitBehavior="False" OnClick="btnOPadd_Click" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>

            <asp:Panel runat="server" ID="pnlsepOtherProviderInfoSearch" class="CollapsingSeparator"
                Visible="false" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerOtherProviderInfoSearch">
                <%-- <div class="pageHeader pH2" id="sepServiceDetail" runat="server" style="background-color: cornflowerblue; padding-left: 10px">SERVICE DETAILS</div>--%>
                <span id="sepOtherProviderInfoSearch" runat="server" class="pageHeader pH2">Search</span>
            </asp:Panel>
            <asp:Panel ID="pnlOtherProviderInfoSearch" Visible="false" runat="server" Style="min-height: 80px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <div class="row" style="text-align: center;">
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><span style="color: red"></span><b>NPI </b>
                                <asp:TextBox ID="txtotherNPi" runat="server" CssClass="formField" MaxLength="10" />
                                <asp:LinkButton ID="lnkotherseacrch" Text="Search" runat="server" ToolTip="Search"
                                    CommandName="OtherProviderInfoSearch" OnClick="lnkOtherProviderInfoSearch_Click"
                                    OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>
                            </span>

                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Medicaid ID </b>
                                <asp:TextBox ID="txtotherMedicaid" runat="server" CssClass="formField" MaxLength="7" />
                            </span>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Business/Last Name </b>
                                <asp:TextBox ID="txtotherbussiness" runat="server" Rows="2" CssClass="formField wd650"
                                    TextMode="MultiLine" MaxLength="70" />
                            </span>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>First Name </b>
                                <asp:TextBox ID="txtotherfirstname" runat="server" CssClass="formField" MaxLength="35" />
                            </span>
                        </div>
                        <%--   <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnOtherProviderInfoSearch" Text="Add" runat="server" OnClick="OtherProviderInfoSearchAdd_Click"
                            CssClass="button" causesValidation="false" />
                        <asp:Button ID="btnOtherProviderInfoSearchCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click"
                            CausesValidation="false" />
                    </div>--%>
                        <div class="divOtherProviderInfoSearch" style="padding-top: 10px">
                            <asp:GridView ID="gvOtherProviderInfoSearch" runat="server" Width="80%" AllowSorting="false"
                                CssClass="gridview"
                                EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdOtherProviderInfoSearch_SelectedIndexChanged"
                                Style="margin-right: 100px">

                                <Columns>
                                    <asp:BoundField DataField="NPI" HeaderText="NPI" />
                                    <asp:BoundField DataField="MedicaidID" HeaderText="Medicaid ID" />
                                    <asp:BoundField DataField="BusinessLastName" HeaderText="Business/Last Name" />
                                    <asp:BoundField DataField="FirstName" HeaderText="First Name" />

                                </Columns>
                                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                                <AlternatingRowStyle CssClass="gridViewAltRow" />
                                <RowStyle CssClass="gridViewRow" />
                                <FooterStyle CssClass="gridViewFooter" />
                            </asp:GridView>
                        </div>
                    </div>


                </div>

            </asp:Panel>

            <ajax:collapsiblepanelextender id="cpeReferringProvider" runat="server" collapsed="true"
                targetcontrolid="pnlReferringProvider" expandcontrolid="pnlsepReferringProvider"
                collapsecontrolid="pnlsepReferringProvider" />
            <asp:Panel runat="server" ID="pnlsepReferringProvider" class="CollapsingSeparator"
                onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerRecipientInformation2">
                <span id="sepReferringProvider" runat="server" class="pageHeader pH2">+ REFERRING PROVIDER
                INFORMATION </span>
            </asp:Panel>
            <asp:Panel ID="pnlReferringProvider" runat="server" Style="overflow-x: hidden;">
                <uc:referringprovider runat="server" id="ucReferringProvider" />
            </asp:Panel>

            <%--<asp:BoundField DataField="Line" HeaderText="Line" />--%>
            <asp:Panel runat="server" ID="pnlsepReferringProvidersearch" class="CollapsingSeparator"
                Visible="false" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerOtherProviderInfoSearch">
                <%-- <div class="pageHeader pH2" id="sepServiceDetail" runat="server" style="background-color: cornflowerblue; padding-left: 10px">SERVICE DETAILS</div>--%>
                <span id="sepReferringProvidersearch" runat="server" class="pageHeader pH2">Search</span>
            </asp:Panel>
            <asp:Panel ID="pnlReferringProvidersearch" Visible="false" runat="server" Style="min-height: 80px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <div class="row" style="text-align: center;">
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><span style="color: red"></span><b>NPI </b>
                                <asp:TextBox ID="txtnpiReferringProvidersearch" runat="server" CssClass="formField"
                                    MaxLength="10" />
                                <asp:LinkButton ID="LinkButton1" Text="Search" runat="server" ToolTip="Search"
                                    CommandName="OtherProviderInfoSearch" OnClick="lnkOtherProviderInfoSearch_Click"
                                    OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>
                            </span>

                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Medicaid ID </b>
                                <asp:TextBox ID="txtmedicaididReferringProvidersearch" runat="server" CssClass="formField"
                                    MaxLength="7" />
                            </span>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Business/Last Name </b>
                                <asp:TextBox ID="txtbussinReferringProvidersearch" runat="server" Rows="2" CssClass="formField wd650"
                                    TextMode="MultiLine" MaxLength="70" />
                            </span>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>First Name </b>
                                <asp:TextBox ID="txtfirstnamerefferingsearch" runat="server" CssClass="formField"
                                    MaxLength="35" />
                            </span>
                        </div>
                        <%--<div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnReferringProvidersearch" Text="Add" runat="server" OnClick="ReferringProvidersearchAdd_Click"
                            CssClass="button" causesValidation="false" />
                        <asp:Button ID="btncancelReferringsearch" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click"
                            CausesValidation="false" />
                    </div>--%>
                        <div class="divReferringProvidersearch" style="padding-top: 10px">
                            <asp:GridView ID="gvReferringProvidersearch" runat="server" Width="80%" AllowSorting="false"
                                CssClass="gridview"
                                EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdReferringProvidersearch_SelectedIndexChanged"
                                Style="margin-right: 100px">

                                <Columns>
                                    <asp:BoundField DataField="NPI" HeaderText="NPI" />
                                    <asp:BoundField DataField="MedicaidID" HeaderText="Medicaid ID" />
                                    <asp:BoundField DataField="BusinessLastName" HeaderText="Business/Last Name" />
                                    <asp:BoundField DataField="FirstName" HeaderText="First Name" />

                                </Columns>
                                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                                <AlternatingRowStyle CssClass="gridViewAltRow" />
                                <RowStyle CssClass="gridViewRow" />
                                <FooterStyle CssClass="gridViewFooter" />
                            </asp:GridView>
                        </div>
                    </div>


                </div>

            </asp:Panel>





            <ajax:collapsiblepanelextender id="cpeRenderingProvider" runat="server" collapsed="true"
                targetcontrolid="pnlRenderingProvider" expandcontrolid="pnlsepRenderingProvider"
                collapsecontrolid="pnlsepRenderingProvider" />
            <asp:Panel runat="server" ID="pnlsepRenderingProvider" class="CollapsingSeparator"
                onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc;"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerRecipientInformation2">
                <span id="sepRenderingProvider" runat="server" class="pageHeader pH2">+ RENDERING PROVIDER
                </span>
            </asp:Panel>
            <asp:Panel ID="pnlRenderingProvider" runat="server" Style="overflow-x: hidden;">
                <div class="container-fluid">
                    <div class="row">
                        <uc:renderingproviderinformationpanel runat="server" id="ucRenderingProviderInformationPanel" />
                    </div>
                </div>
            </asp:Panel>



            <asp:Panel runat="server" ID="pnlsepRenderingProviderSearch" class="CollapsingSeparator"
                Visible="false" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerOtherProviderInfoSearch">
                <span id="sepRenderingProviderSearch" runat="server" class="pageHeader pH2">Search</span>
            </asp:Panel>
            <asp:Panel ID="pnlRenderingProviderSearch" Visible="false" runat="server" Style="min-height: 80px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <div class="row" style="text-align: center;">
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><span style="color: red"></span><b>NPI </b>
                                <asp:TextBox ID="txtnpiRenderingSearch" runat="server" CssClass="formField" MaxLength="10" />
                                <asp:LinkButton ID="lnkRenderingProviderSearch" Text="Search" runat="server" ToolTip="Search"
                                    CommandName="RenderingProviderSearch"
                                    OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>
                            </span>
                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Medicaid ID </b>
                                <asp:TextBox ID="txtmedicaidRenderingSearch" runat="server" CssClass="formField"
                                    MaxLength="7" />
                            </span>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Business/Last Name </b>
                                <asp:TextBox ID="TextBox3" runat="server" Rows="2" CssClass="formField wd650" TextMode="MultiLine"
                                    MaxLength="70" />
                            </span>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>First Name </b>
                                <asp:TextBox ID="txtfirstRenderingSearch" runat="server" CssClass="formField" MaxLength="35" />
                            </span>
                        </div>
                        <%--<div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnRenderingProviderSearch" Text="Add" runat="server" OnClick="RenderingProviderSearchAdd_Click"
                            CssClass="button" causesValidation="false" />
                        <asp:Button ID="btncancelRenderingProviderSearch" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click"
                            CausesValidation="false" />
                    </div>--%>
                        <div class="divRenderingProviderSearch" style="padding-top: 10px">
                            <asp:GridView ID="gvRenderingProviderSearch" runat="server" Width="80%" AllowSorting="false"
                                CssClass="gridview"
                                EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdRenderingProviderSearch_SelectedIndexChanged"
                                Style="margin-right: 100px">

                                <Columns>
                                    <%-- <asp:BoundField DataField="id_perf_prov" HeaderText="NPI" />
                            <asp:BoundField DataField="cde_party_id" HeaderText="Medicaid ID" />
                            <asp:BoundField DataField="nam_last" HeaderText="Business/Last Name" />
                            <%--Not Found in Document--%>
                                    <%--<asp:BoundField DataField=" nam_first" HeaderText="First Name" />--%>
                                    <asp:BoundField DataField="NPI" HeaderText="NPI" />
                                    <asp:BoundField DataField="MedicaidID" HeaderText="Medicaid ID" />
                                    <asp:BoundField DataField="BusinessLastName" HeaderText="Business/Last Name" />
                                    <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                                </Columns>
                                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                                <AlternatingRowStyle CssClass="gridViewAltRow" />
                                <RowStyle CssClass="gridViewRow" />
                                <FooterStyle CssClass="gridViewFooter" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <%-- Service Facility Information Starting --%>

            <div>
                <uc:servicefacility runat="server" id="ucServiceFacility" />
            </div>

            <%-- Service Facility Information Ending --%>
            <ajax:collapsiblepanelextender id="cpeAssistantSurgeon" runat="server" collapsed="true"
                targetcontrolid="pnlAssistantSurgeon" expandcontrolid="pnlsepAssistantSurgeon"
                collapsecontrolid="pnlsepAssistantSurgeon" />
            <asp:Panel runat="server" ID="pnlsepAssistantSurgeon" class="CollapsingSeparator"
                onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerRecipientInformation2">
                <span id="sepAssistantSurgeon" runat="server" class="pageHeader pH2"></span>
            </asp:Panel>
            <asp:Panel ID="pnlAssistantSurgeon" runat="server" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; overflow-x: hidden;">
                <div class="container-fluid">
                    <div class="row">
                        <uc:assistantsurgeon runat="server" id="ucAssistantSurgeon" />
                    </div>
                </div>
            </asp:Panel>


            <ajax:collapsiblepanelextender id="cpeSupervisingProvider" runat="server" collapsed="true"
                targetcontrolid="pnlSupervisingProvider" expandcontrolid="pnlsepSupervisingProvider"
                collapsecontrolid="pnlsepSupervisingProvider" />
            <asp:Panel runat="server" ID="pnlsepSupervisingProvider" class="CollapsingSeparator"
                onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerSupervisingProvider">
                <span id="sepSupervisingProvider" runat="server" class="pageHeader pH2"></span>
            </asp:Panel>
            <asp:Panel ID="pnlSupervisingProvider" runat="server" Style="overflow-x: hidden;">
                <div class="row">
                    <uc:supervisingproviderpanel runat="server" id="ucSupervisingProviderPanel" />
                </div>
            </asp:Panel>

            <asp:Panel runat="server" ID="pnlsepSupervisingProvidersearch" class="CollapsingSeparator"
                Visible="false" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerOtherProviderInfoSearch">
                <span id="sepSupervisingProvidersearch" runat="server" class="pageHeader pH2">Search</span>
            </asp:Panel>
            <asp:Panel ID="pnlSupervisingProvidersearch" Visible="false" runat="server" Style="min-height: 80px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <div class="row" style="text-align: center;">
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><span style="color: red"></span><b>NPI </b>
                                <asp:TextBox ID="txtnpiSupervisingProvidersearch" runat="server" CssClass="formField"
                                    MaxLength="10" />
                                <asp:LinkButton ID="lnkSupervisingProvidersearch" Text="Search" runat="server" ToolTip="Search"
                                    CommandName="SupervisingProvidersearch" OnClick="lnkSupervisingProvidersearch_Click"
                                    OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>
                            </span>
                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Medicaid ID </b>
                                <asp:TextBox ID="txtmedicaidSupervisingProvidersearch" runat="server" CssClass="formField"
                                    MaxLength="7" />
                            </span>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Business/Last Name </b>
                                <asp:TextBox ID="txtbussinessSupervisingProvidersearch" runat="server" Rows="2" CssClass="formField wd650"
                                    TextMode="MultiLine" MaxLength="70" />
                            </span>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>First Name </b>
                                <asp:TextBox ID="txtfirsSupervisingProvidersearch" runat="server" CssClass="formField"
                                    MaxLength="35" />
                            </span>
                        </div>
                        <%-- <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnSupervisingProvidersearch" Text="Add" runat="server" OnClick="SupervisingProvidersearchAdd_Click"
                            CssClass="button" causesValidation="false" />
                        <asp:Button ID="btncanSupervisingProvidersearch" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click"
                            CausesValidation="false" />
                    </div>--%>
                        <div class="divOtherProviderInfoSearch" style="padding-top: 10px">
                            <asp:GridView ID="gvSupervisingProvidersearch" runat="server" Width="80%" AllowSorting="false"
                                CssClass="gridview"
                                EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdSupervisingProvidersearch_SelectedIndexChanged"
                                Style="margin-right: 100px">
                                <Columns>
                                    <%--<asp:BoundField DataField="id_perf_prov" HeaderText="NPI" />
                            <asp:BoundField DataField="cde_party_id" HeaderText="Medicaid ID" />
                            <asp:BoundField DataField="nam_last" HeaderText="Business/Last Name" />
                            <asp:BoundField DataField="nam_first" HeaderText="First Name" />--%>
                                    <asp:BoundField DataField="NPI" HeaderText="NPI" />
                                    <asp:BoundField DataField="MedicaidID" HeaderText="Medicaid ID" />
                                    <asp:BoundField DataField="BusinessLastName" HeaderText="Business/Last Name" />
                                    <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                                </Columns>
                                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                                <AlternatingRowStyle CssClass="gridViewAltRow" />
                                <RowStyle CssClass="gridViewRow" />
                                <FooterStyle CssClass="gridViewFooter" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <%-- Ambulance Information --%>
            <div>
                <ucambulance:ambulanceinformation id="ucAmbulanceInformation" runat="server" />
            </div>

            <%-- Other Payer Information --%>

            <div id="otherPayerAddress">
                <uc:otherpayerinformation runat="server" id="ucOtherPayerInformation" onrefreshotherpayers="OnDeleteOtherPayers_Click" onrefreshotherpayerpaidamountdropdown="OnRefreshOtherPayerPaidAmountDropdown_Click" />
            </div>

            <%-- <ajax:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" Collapsed="false"
        TargetControlID="pnlOtherPayerAdjustmentInformation" ExpandControlID="pnlsepOtherPayerAdjustmentInformation"
        CollapseControlID="pnlsepOtherPayerAdjustmentInformation" />
    <asp:Panel runat="server" ID="Panel10" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
        Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerRecipientInformation2">
        <span id="Span2" runat="server" class="pageHeader pH2">- HEADER-OTHER PAYER ADJUSTMENT
            INFORMATION </span>
    </asp:Panel>
    <asp:Panel ID="Panel25" runat="server" Style="overflow-x: hidden; height: 120px; padding-top: 20px; padding-bottom: 20px;">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>--%>


            <asp:UpdatePanel ID="upnlDiagnosis" runat="server">
                <ContentTemplate>
                    <ajax:collapsiblepanelextender id="cpeDiagnosis" runat="server" collapsed="true"
                        targetcontrolid="pnlDiagnosis" expandcontrolid="pnlsepDiagnosis" collapsecontrolid="pnlsepDiagnosis" />
                    <asp:Panel runat="server" ID="pnlsepDiagnosis" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                        Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerDiagnosis">
                        <span id="sepDiagnosis" runat="server" cssclass="cpBody" class="pageHeader pH2"></span>
                    </asp:Panel>
                    <asp:Panel ID="pnlDiagnosis" runat="server" Style="height: auto; overflow-y: hidden; overflow-x: hidden">
                        <div class="row">
                            <uc:diagnosiscodepanel runat="server" id="ucDiagnosisCodePanel" onrefreshchilddropdown="OnAddDropdown_Click" />
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:Panel runat="server" ID="pnlsepDiagnosissearch" class="CollapsingSeparator"
                Visible="false" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse"
                CssClass="OwnerDiagnosis">
                <span id="sepDiagnosissearch" runat="server" class="pageHeader pH2">Search</span>
            </asp:Panel>
            <asp:Panel ID="pnlDiagnosissearch" Visible="false" runat="server" Style="min-height: 80px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <div class="row" style="text-align: center;">
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><span style="color: red"></span><b>Diagnosis Code </b>
                                <asp:TextBox ID="txtDiagnosisCode1" runat="server" CssClass="formField" />
                                <%-- <asp:LinkButton ID="lnkSearchDiagnosis" runat="server" ToolTip="Search" CommandName="DiagnosisSearch" OnClick="lnkDiagnosisSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>&nbsp;&nbsp;--%>
                            </span>
                            <asp:LinkButton ID="lnkdiag" runat="server" ToolTip="Search" Text="Search" CommandArgument='<%# Eval("DiagnosisCode")%>'
                                OnClick="LinkButton3_Click" Visible="true">
                            </asp:LinkButton>

                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><span style="color: red">*</span><b>ICD Version</b>
                                <%-- <asp:DropDownList ID="ddlICDVersion" CssClass="formField" runat="server">--%>
                                <asp:DropDownList ID="ddlICDVersionClaim" CssClass="formField" EnableViewState="true"
                                    runat="server" AutoPostBack="true"
                                    AppendDataBoundItems="True">
                                    <asp:ListItem Value="ICD 10" Text="1"></asp:ListItem>
                                    <asp:ListItem Value="ICD 9" Text="2"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="reqICDVersionClaim" SetFocusOnError="true"
                                    ValidationGroup="vgCarePlan" ControlToValidate="ddlICDVersionClaim" ErrorMessage="*ICD Version"
                                    Text="*" Display="Dynamic" InitialValue="0" />
                                <span style="color: red; display: none">ICD Version is required</span>
                            </span>
                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Diagnosis Code Description </b>

                                <asp:TextBox ID="txtDiagnosisCodeDesc1" runat="server" Rows="2" CssClass="formField"
                                    TextMode="MultiLine" MaxLength="1000" />
                            </span>
                        </div>
                        <%-- <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnAddDiagnosissearch" Text="Add" runat="server" OnClick="DiagnosissearchAdd_Click"
                            CssClass="button" causesValidation="false" />
                        <asp:Button ID="btnCanelDiagnosissearch" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click"
                            CausesValidation="false" />
                    </div>--%>
                        <div class="divDiagnosisSearch" style="padding-top: 10px">
                            <asp:GridView ID="gvDiagnosisSearch" runat="server" Width="80%" AllowSorting="false"
                                CssClass="gridview"
                                EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdDiagnosisSearch_SelectedIndexChanged"
                                Style="margin-right: 100px">
                                <Columns>
                                    <asp:BoundField DataField="cde_diag_seq" HeaderText="Diagnosis Code" />
                                    <asp:BoundField DataField="qlf_code_list" HeaderText="ICD Version" />
                                    <asp:BoundField DataField="cde_diag" HeaderText="Diagnosis Code Description" />

                                </Columns>
                                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                                <AlternatingRowStyle CssClass="gridViewAltRow" />
                                <RowStyle CssClass="gridViewRow" />
                                <FooterStyle CssClass="gridViewFooter" />
                            </asp:GridView>
                        </div>
                    </div>

                </div>

            </asp:Panel>

            <asp:Panel ID="pnlSection" runat="server">
            </asp:Panel>

            <div id="divOutPatientPanel" runat="server">
                <ajax:collapsiblepanelextender id="cpeOutpatientAdjudication" runat="server" collapsed="true"
                    targetcontrolid="pnlOutpatientAdjudicationInformation" expandcontrolid="pnlsepOutpatientAdjudication"
                    collapsecontrolid="pnlsepOutpatientAdjudication" />
                <asp:Panel ID="pnlsepOutpatientAdjudication" runat="server" class="CollapsingSeparator"
                    onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                    ToolTip="Click to Expand/Collapse" CssClass="OwnerSupervisingProvider">
                    <span id="sepOutpatientAdjudication" runat="server" class="pageHeader pH2">+ OUTPATIENT
                    ADJUDICATION INFORMATION</span>
                </asp:Panel>
                <asp:Panel ID="pnlOutpatientAdjudicationInformation" runat="server" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; max-width: 98%;">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <uc:outpatientadjudicationinformation runat="server" id="ucOutpatientAdjudicationInformation" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div>
                <uc:inpatientadjudicationinformation runat="server" id="ucInPatientAdjudicationInformation" />
            </div>

            <ajax:collapsiblepanelextender id="cpeOtherPayerAdjustmentInformation" runat="server"
                collapsed="true" targetcontrolid="pnlOtherPayerAdjustmentInformation" expandcontrolid="pnlsepOtherPayerAdjustmentInformation"
                collapsecontrolid="pnlsepOtherPayerAdjustmentInformation" />
            <asp:Panel runat="server" ID="pnlsepOtherPayerAdjustmentInformation" class="CollapsingSeparator"
                onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerSupervisingProvider">
                <span id="sepOtherPayerAdjustmentInformation" runat="server" class="pageHeader pH2">+ HEADER OTHER PAYER ADJUSTMENT INFORMATION </span>
            </asp:Panel>
            <asp:Panel ID="pnlOtherPayerAdjustmentInformation" runat="server" Style="height: auto; padding-top: 20px; padding-bottom: 20px;">
                <asp:UpdatePanel ID="upnlOtherPayerAdjustmentInformation" runat="server">
                    <ContentTemplate>
                        <div class="container-fluid">
                            <div class="row">
                                <uc:submitclaimheaderotherpayeradjustmentmapping runat="server" id="ucSubmitClaimHeaderOtherPayerAdjustmentMapping" />
                            </div>
                            <div id="divHeaderOtherPayerProfessional" runat="server" visible="false">
                                <uc:submitclaimheaderotherpayeradjustmentmappingprofessional runat="server" id="ucSubmitClaimHeaderOtherPayerAdjustmentMappingProfessional" />
                            </div>
                            <div id="divHeaderOtherPayerInstitutional" runat="server" visible="false">
                                <uc:submitclaimheaderotherpayeradjustmentmappinginstitutional runat="server" id="ucSSubmitClaimHeaderOtherPayerAdjustmentMappingInstitutional" />

                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:UpdatePanel ID="updatePnlIcd" runat="server">
                <ContentTemplate>
                    <ajax:collapsiblepanelextender id="cpeICD10ProcedureCode" runat="server" collapsed="true"
                        targetcontrolid="pnlICD10ProcedureCode" expandcontrolid="pnlsepICD10ProcedureCode"
                        collapsecontrolid="pnlsepICD10ProcedureCode" />
                    <asp:Panel runat="server" ID="pnlsepICD10ProcedureCode" class="CollapsingSeparator"
                        onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                        ToolTip="Click to Expand/Collapse" CssClass="OwnerICD10ProcedureCode">
                        <span id="sepICD10ProcedureCode" runat="server" class="pageHeader pH2">+ ICD PROCEDURE
                        CODES </span>
                    </asp:Panel>
                    <asp:Panel ID="pnlICD10ProcedureCode" runat="server" Style="min-height: 270px; height: auto; overflow: hidden;">

                        <uc:submitclaimicdprocedurecode runat="server" id="ucSubmitClaimICDProcedureCode" />

                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>


            <ajax:collapsiblepanelextender id="cpeOccurenceInformation" runat="server" collapsed="true"
                targetcontrolid="pnlOccurenceInformation" expandcontrolid="pnlsepOccurenceInformation"
                collapsecontrolid="pnlsepOccurenceInformation" autocollapse="false" autoexpand="false"
                enableviewstate="true" />
            <asp:Panel runat="server" ID="pnlsepOccurenceInformation" class="CollapsingSeparator"
                onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerOccurenceInformation">
                <span id="sepOccurenceInformation" runat="server" class="pageHeader pH2">+ OCCURRENCE
                        INFORMATION </span>
            </asp:Panel>

            <asp:Panel ID="pnlOccurenceInformation" runat="server" Style="height: auto; overflow: hidden;">
                <div class="row">
                    <uc:submitclaimoccurrenceinformation runat="server" id="ucSubmitClaimOccurrenceInformation1" />

                </div>
            </asp:Panel>




            <ajax:collapsiblepanelextender id="cpeOccurenceSpan" runat="server" collapsed="true"
                targetcontrolid="pnlOccurenceSpan" expandcontrolid="pnlOccurenceSpanInformation"
                collapsecontrolid="pnlOccurenceSpanInformation" />
            <asp:Panel runat="server" ID="pnlOccurenceSpanInformation" class="CollapsingSeparator"
                onclick="javascript:CollapseExpand(this);"
                Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerNDCDetails">
                <span id="sepOccurenceSpan" runat="server" class="pageHeader pH2">+ OCCURRENCE SPAN
                        INFORMATION</span>
            </asp:Panel>
            <asp:Panel ID="pnlOccurenceSpan" runat="server" Style="height: 1200px; width: auto; max-width: 100%;">
                <div class="container-fluid">
                    <uc:occurencespaninformation runat="server" id="ucOccurenceSpanInformation" />
                </div>
            </asp:Panel>



            <ajax:collapsiblepanelextender id="cpeConditionCodeInfo" runat="server" collapsed="true"
                targetcontrolid="pnlConditionCodeInfo" expandcontrolid="pnlsepConditionCode"
                collapsecontrolid="pnlsepConditionCode" />
            <asp:Panel runat="server" ID="pnlsepConditionCode" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerConditionCodeInformation">
                <span id="sepConditionCode" runat="server" class="pageHeader pH2">+ CONDITION CODE INFORMATION</span>
            </asp:Panel>
            <asp:Panel ID="pnlConditionCodeInfo" runat="server" Style="min-height: 240px; min-width: 150px; height: auto; width: auto; max-width: 100%;">
                <div class="container-fluid">

                    <uc:conditioncodeinformation runat="server" id="ucConditionCodeInformation" />

                </div>
            </asp:Panel>



            <ajax:collapsiblepanelextender id="cpeValueCodeInformation" runat="server" collapsed="true"
                targetcontrolid="pnlValueCodeInformation" expandcontrolid="pnlsepValueCodeInformation"
                collapsecontrolid="pnlsepValueCodeInformation" />
            <asp:Panel ID="pnlsepValueCodeInformation" runat="server" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerValueCodeInformation">
                <span id="sepValueCodeInformation" runat="server" class="pageHeader pH2">+ VALUE CODE
                INFORMATION
                </span>
            </asp:Panel>
            <asp:Panel ID="pnlValueCodeInformation" runat="server" Style="min-height: 240px; min-width: 150px; height: auto; width: auto; max-width: 100%;">
                <div class="container-fluid">

                    <uc:valuecodeinformation runat="server" id="ucValueCodeInformation" />

                </div>
            </asp:Panel>

            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <ajax:collapsiblepanelextender id="cpeServiceDetail" runat="server" collapsed="false"
                        targetcontrolid="pnlServiceDetail" expandcontrolid="pnlsepServiceDetail" collapsecontrolid="pnlsepServiceDetail" />
                    <asp:Panel runat="server" ID="pnlsepServiceDetail" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                        Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerServiceDetail">
                        <span id="sepServiceDetail" runat="server" class="pageHeader pH2">- <span class="pageHeader pH2 GridviewHeaderAsterisk"></span>SERVICE DETAILS</span>
                    </asp:Panel>
                    <asp:Panel ID="pnlServiceDetail" runat="server" Style="height: auto; overflow: hidden;">

                        <div class="divGrid" style="padding-top: 10px">
                            <uc:institutionalservicedetails runat="server" id="InstitutionalServiceDetails" onrefreshchildgrids="OnDeleteInstitutionalGridBind_Click" />
                            <%--  <asp:GridView ID="gvServiceDetail" runat="server" Height="291px" Style="margin-left: 38px; margin-top: 86px;" Width="446px" AutoGenerateColumns="true" OnDataBound="OnDataBound">--%>
                            <asp:GridView ID="gvServiceDetail" runat="server" Width="80%" AllowSorting="false"
                                CssClass="gridview"
                                EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdServiceDetailSearch_SelectedIndexChanged"
                                Style="margin-right: 200px">
                                <Columns>
                                    <%--<asp:BoundField DataField="Line" HeaderText="Line" />--%>
                                    <asp:BoundField DataField="num_dtl_total" HeaderText="*Detail Item" />
                                    <asp:BoundField DataField="cde_revenue" HeaderText="*Revenue Code" />
                                    <asp:BoundField DataField="cde_proc" HeaderText="Procedure Code" />
                                    <asp:BoundField DataField="" HeaderText="Procedure Modifier" />
                                    <asp:BoundField DataField="" HeaderText="Unit" />
                                    <asp:BoundField DataField="" HeaderText="Unit of Measurement" />
                                    <asp:BoundField DataField="" HeaderText="*From DOS" />
                                    <asp:BoundField DataField="" HeaderText="*To DOS" />
                                    <asp:BoundField DataField="" HeaderText="*Total Charges" />
                                    <asp:BoundField DataField="" HeaderText="Non-Covered Charges" />
                                    <asp:BoundField DataField="" HeaderText="Medicaid Allowed Amount" />
                                    <asp:BoundField DataField="" HeaderText="Paid Amount" />
                                    <asp:BoundField DataField="cde_clm_status" HeaderText="Status" />
                                    <asp:BoundField DataField="" HeaderText="Total Charges" />
                                    <asp:BoundField DataField="" HeaderText="Total Amount Paid" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDeletedsd" runat="server" CommandName="PendingServiceDetails"
                                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');"
                                                AlternateText="Delete"
                                                Visible="<%# CanUserViewDelete()  %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                                <AlternatingRowStyle CssClass="gridViewAltRow" />
                                <RowStyle CssClass="gridViewRow" />
                                <FooterStyle CssClass="gridViewFooter" />
                            </asp:GridView>
                        </div>


                        <div class="row" style="text-align: center;">

                            <div class="row ohio-field">
                                <div class="col-sm-3">
                                </div>
                                <div class="col-sm-1">
                                </div>
                                <div class="col-sm-1"></div>
                                <div class="col-sm-1"></div>
                                <div class="col-sm-1"></div>
                                <div class="col-sm-1"></div>
                                <div class="col-sm-1"></div>
                                <div class="col-sm-1"></div>
                                <div class="col-sm-1"></div>
                                <div class="col-sm-1 text-left"></div>
                            </div>
                        </div>

                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="upnlServiceDetailProfessional" runat="server">
                <ContentTemplate>
                    <ajax:collapsiblepanelextender id="cpeServiceDetailProfessional" runat="server" collapsed="true"
                        targetcontrolid="pnlServiceDetailProfessional" expandcontrolid="pnlsepServiceDetailProfessional"
                        collapsecontrolid="pnlsepServiceDetailProfessional" />
                    <asp:Panel runat="server" ID="pnlsepServiceDetailProfessional" class="CollapsingSeparator"
                        onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                        ToolTip="Click to Expand/Collapse" CssClass="OwnerServiceDetailProfessional">
                        <span id="spanIdProfessional" runat="server" class="pageHeader pH2">-</span> <span id="sepServiceDetailProfessional" runat="server" class="pageHeader pH2 GridviewHeaderAsterisk">SERVICE
                        DETAILS</span>
                    </asp:Panel>
                    <%-- <asp:Panel ID="pnlServiceDetailProfessional" runat="server" Style="min-height: 350px;
                    min-width: 300px; height: auto; width: auto; max-width: 98%;">--%>
                    <asp:Panel ID="pnlServiceDetailProfessional" runat="server" Style="height: auto; overflow: hidden;">
                        <div class="divGrid" style="padding-top: 10px">


                            <uc:professionalservicelinedetails runat="server" id="ProfessionalServiceLineDetails" onrefreshchildgrids="OnDeleteProfessionalGridBind_Click" onrefreshotherpayerpaidamountdropdown="OnRefreshOtherPayerPaidAmountDropdown_Click" />


                        </div>

                        <div class="row" style="text-align: center;">
                            <%--<asp:ValidationSummary ID="ValidationSummary2" DisplayMode="List" runat="server"
                            CssClass="failureNotification" ValidationGroup="valAddProfessionalDetail" />--%>

                            <div class="row ohio-field">
                                <div class="col-sm-3">
                                    <%--<asp:TextBox ID="txtproffProcedureCode" runat="server" CssClass="formField" pattern="([^\s][A-z0-9À-ž\s]+)" Style="height: 30px; width: 120px" />
                            <asp:LinkButton ID="LinkButton4" runat="server" ToolTip="Search" Text="Search" OnClick="lnkProfessionalServiceDetails_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtproffProcedureCode" Text="*" Display="Dynamic" ValidationGroup="valAddProfessionalDetail"></asp:RequiredFieldValidator>--%>
                                </div>
                                <div class="col-sm-1">
                                    <%-- <asp:TextBox ID="txtprofModifier" runat="server" pattern="([^\s][A-z0-9À-ž\s]+)" CssClass="formField" Style="height: 30px; width: 100%" />
                            <asp:RequiredFieldValidator ID="rfvModifier" runat="server"
                                ControlToValidate="txtprofModifier" Text="*" Display="Dynamic" ValidationGroup="valAddProfessionalDetail"></asp:RequiredFieldValidator>--%>
                                </div>
                                <div class="col-sm-1">
                                    <%--  <asp:TextBox ID="txtproffDiagnosisPointer" runat="server" MaxLength="4" CssClass="formField" Style="height: 30px; width: 100%" />
                            <asp:RequiredFieldValidator ID="rfvDiagnosisPointer" runat="server" ControlToValidate="txtproffDiagnosisPointer" Text="*" Display="Dynamic" ValidationGroup="valAddProfessionalDetail"></asp:RequiredFieldValidator>--%>
                                </div>
                                <div class="col-sm-1">
                                    <%-- <asp:TextBox ID="txtprofPlaceofserv" runat="server" CssClass="formField" Style="height: 30px;  width: 100%" />
                            <asp:RequiredFieldValidator ID="rfvPlaceofserv" runat="server" ControlToValidate="txtprofPlaceofserv" Text="*" Display="Dynamic" ValidationGroup="valAddProfessionalDetail"></asp:RequiredFieldValidator> --%>
                                </div>
                                <div class="col-sm-1">
                                    <%-- <asp:DropDownList ID="ddlProffReferralEPSDTService" CssClass="formField" EnableViewState="true" runat="server" style="height: 30px; width: 100%; min-width: 75px;"
                                    AppendDataBoundItems="True">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="reqReferralEPSDTService" SetFocusOnError="true"
                                ValidationGroup="valAddProfessionalDetail" ControlToValidate="ddlProffReferralEPSDTService" ErrorMessage="*Referral EPSDT Service/Family Planning" Text="*" Display="Dynamic" InitialValue="0" />--%>
                                </div>
                                <div class="col-sm-1">
                                    <%--<asp:TextBox ID="txtproffBilledUnits" runat="server" CssClass="formField" Style="height: 30px; width: 100%" MaxLength="1"/>
                            <asp:RequiredFieldValidator runat="server" ID="rfvBilledUnits" SetFocusOnError="true"
                                ValidationGroup="valAddProfessionalDetail" ControlToValidate="txtproffBilledUnits" ErrorMessage="*Billed unit not reported for detail N" Text="*" Display="Dynamic" InitialValue="1" />--%>
                                </div>
                                <div class="col-sm-1">
                                    <%--<asp:DropDownList ID="ddlproffUnitsMea" CssClass="formField" EnableViewState="true" runat="server"  AppendDataBoundItems="True" Width="100%" style="min-width:75px;"/>
                            <asp:RequiredFieldValidator runat="server" ID="rfvddlUnitsMea" SetFocusOnError="true"
                                ValidationGroup="valAddProfessionalDetail" ControlToValidate="ddlproffUnitsMea" ErrorMessage="*Units of measurement is required" Text="*" Display="Dynamic" InitialValue="0" />--%>
                                </div>
                                <div class="col-sm-1">
                                    <%--<asp:TextBox ID="txtProffdateofservice" runat="server" CssClass="formField" style="height: 30px; width: 100%; min-width: 75px;" />
                            <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                            <ajax:CalendarExtender ID="cedateofservice" TargetControlID="txtProffdateofservice" runat="server" />
                            <asp:CompareValidator ID="cvProffdateofservice" runat="server" ErrorMessage="Future date not allowed" Operator="LessThanEqual" ControlToValidate="txtProffdateofservice" Type="Date"></asp:CompareValidator>--%>
                                </div>
                                <div class="col-sm-1">
                                    <%--<asp:TextBox ID="txtproffCharges" runat="server" CssClass="formField" Style="height: 30px; width: 100%" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvcharges" SetFocusOnError="true"
                                ValidationGroup="valAddProfessionalDetail" ControlToValidate="txtproffCharges" ErrorMessage="*Allows $0.00" Text="*" Display="Dynamic" InitialValue="1" />  --%>
                                </div>
                                <div class="col-sm-1 text-left">
                                    <%--<asp:Button ID="btnProfessionalServiceDetailsAdd" Text="ADD" runat="server" OnClick="btnProfessionalServiceDetailsAdd_Click" CssClass="btn btn-danger" Font-Bold="True" ValidationGroup="valProfessionalService" CausesValidation="True" Width="90px" />--%>
                                </div>
                            </div>


                            <div class="col-sm-12">
                                <div class="row" runat="server">
                                    <div class="col-sm-11">
                                        <span class="ohio-field" style="font-size: 15px; text-align: right">&nbsp;</span>
                                    </div>

                                </div>
                            </div>
                            <div class="col-sm-12">
                                <div class="row" runat="server">
                                    <div class="col-sm-11">
                                        <span class="ohio-field" style="font-size: 15px; text-align: right">&nbsp;</span>
                                    </div>

                                </div>
                            </div>


                            <div class="col-sm-6">
                                <div class="row" runat="server">
                                    <div class="col-sm-5">
                                        <span class="ohio-field" style="font-size: 15px; text-align: right">&nbsp;</span>
                                    </div>
                                    <div class="col-sm-7 text-left">
                                        <span class="ohio-field" style="font-size: 15px; text-align: right">&nbsp;</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:Panel runat="server" ID="pnlsepProfessionalServiceDetailsearch" class="CollapsingSeparator"
                Visible="false" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerProfessionalServiceDetailsearch">
                <%-- <div class="pageHeader pH2" id="sepServiceDetail" runat="server" style="background-color: cornflowerblue; padding-left: 10px">SERVICE DETAILS</div>--%>
                <span id="sepProfessionalServiceDetailsSearch" runat="server" class="pageHeader pH2">Search</span>
            </asp:Panel>
            <asp:Panel ID="pnlProfessionalServiceDetailssearch" Visible="false" runat="server"
                Style="min-height: 80px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <div class="row" style="text-align: center;">
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><span style="color: red"></span><b>Procedure Code </b>
                                <asp:TextBox ID="txtProfessionalProcedure" runat="server" CssClass="formField" />
                                <asp:LinkButton ID="lnkProfessionalServiceDetailsSearch" Text="Search" runat="server"
                                    ToolTip="Search"
                                    CommandName="ProfessionalSearch" OnClick="lnkProfessionalSearch_Click"
                                    OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>
                            </span>

                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Procedure Code Description </b>
                                <asp:TextBox ID="txtProfessionalDesc" runat="server" Rows="2" CssClass="formField"
                                    TextMode="MultiLine" MaxLength="1000" />
                            </span>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <asp:Button ID="btnProfessionalServiceDetailsSearch" Text="Add" runat="server" OnClick="ProfessionalServiceDetailsSearchAdd_Click"
                                CssClass="button" CausesValidation="false" />
                            <asp:Button ID="btnProfessionalcancel" runat="server" Text="Cancel" CssClass="button"
                                OnClick="btnCancel_Click"
                                CausesValidation="false" />
                        </div>
                        <div class="divProfessionalServicesearch" style="padding-top: 10px">
                            <asp:GridView ID="gvProfessionalServicesearch" runat="server" Width="80%" AllowSorting="false"
                                CssClass="gridview"
                                EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdProfessionalServicesearch_SelectedIndexChanged"
                                Style="margin-right: 100px">

                                <Columns>
                                    <asp:BoundField DataField="ProcedureCode" HeaderText="Procedure Code" />
                                    <asp:BoundField DataField="ProcedureCodeDesc" HeaderText="Procedure Code Description" />

                                </Columns>
                                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                                <AlternatingRowStyle CssClass="gridViewAltRow" />
                                <RowStyle CssClass="gridViewRow" />
                                <FooterStyle CssClass="gridViewFooter" />
                            </asp:GridView>
                        </div>
                    </div>


                </div>

            </asp:Panel>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <ajax:collapsiblepanelextender id="cpeServiceDetailDental" runat="server" collapsed="true"
                        targetcontrolid="pnlServiceDetailDental" expandcontrolid="pnlsepServiceDetailDental"
                        collapsecontrolid="pnlsepServiceDetailDental" />
                    <asp:Panel runat="server" ID="pnlsepServiceDetailDental" class="CollapsingSeparator"
                        onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                        ToolTip="Click to Expand/Collapse" CssClass="OwnerServiceDetailDental">
                        <span id="sepServiceDetailDental" runat="server" class="pageHeader pH2">-</span><b class="pageHeader pH2 GridviewHeaderAsterisk">SERVICE DETAILS </b>
                    </asp:Panel>
                    <asp:Panel ID="pnlServiceDetailDental" runat="server" Style="min-height: 400px; min-width: 150px; width: auto; height: auto; max-width: 98%"
                        Height="200px">
                        <uc:servicedetailsdental runat="server" id="ucServiceDetailsDental" onrefreshchildgrids="OnDeleteGridBind_Click" />
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>



            <asp:Panel runat="server" ID="pnlsepDentalServiceDetailsearch" class="CollapsingSeparator"
                Visible="false" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerDentalServiceDetailsearch">
                <%-- <div class="pageHeader pH2" id="sepServiceDetail" runat="server" style="background-color: cornflowerblue; padding-left: 10px">SERVICE DETAILS</div>--%>
                <span id="sepDentalServiceDetailsearch" runat="server" class="pageHeader pH2">Search</span>
            </asp:Panel>
            <asp:Panel ID="pnlDentalServiceDetailsearch" Visible="false" runat="server" Style="min-height: 80px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <div class="row" style="text-align: center;">
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><span style="color: red"></span><b>Procedure Code </b>
                                <asp:TextBox ID="txtProcedureCodeSearch" runat="server" CssClass="formField" />
                                <%-- <asp:LinkButton ID="lnkSearchDiagnosis" runat="server" ToolTip="Search" CommandName="DiagnosisSearch" OnClick="lnkDiagnosisSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>&nbsp;&nbsp;--%>
                            </span>

                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Procedure Code Description </b>
                                <asp:TextBox ID="txtProcedureCodeDesc" runat="server" Rows="2" CssClass="formField"
                                    TextMode="MultiLine" MaxLength="1000" />
                            </span>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 " style="background-color: #3E659F">
                            <asp:Button ID="Button4" Text="Add" runat="server" OnClick="DentalServicesearchAdd_Click"
                                CssClass="button" CausesValidation="false" />
                            <asp:Button ID="Button6" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click"
                                CausesValidation="false" />
                        </div>
                        <div class="divDentalServicesearch" style="padding-top: 10px">
                            <asp:GridView ID="gvDentalServicesearch" runat="server" Width="80%" AllowSorting="false"
                                CssClass="gridview"
                                EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdDentalServicesearch_SelectedIndexChanged"
                                Style="margin-right: 100px">

                                <Columns>
                                    <asp:BoundField DataField="" HeaderText="Procedure Code" />
                                    <asp:BoundField DataField="MemberID" HeaderText="Procedure Code Description" />

                                </Columns>
                                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                                <AlternatingRowStyle CssClass="gridViewAltRow" />
                                <RowStyle CssClass="gridViewRow" />
                                <FooterStyle CssClass="gridViewFooter" />
                            </asp:GridView>
                        </div>
                    </div>


                </div>

            </asp:Panel>



            <asp:UpdatePanel ID="upnlNDCDetails" runat="server">
                <ContentTemplate>
                    <ajax:collapsiblepanelextender id="cpeNDCDetails" runat="server" collapsed="true"
                        targetcontrolid="pnlNDCDetail" expandcontrolid="pnlsepNDCDetails" collapsecontrolid="pnlsepNDCDetails" />
                    <asp:Panel runat="server" ID="pnlsepNDCDetails" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                        Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerServiceDetailDental">
                        <span id="sepNDCDetails" runat="server" class="pageHeader pH2">+ NDC DETAILS</span>
                    </asp:Panel>
                    <asp:Panel ID="pnlNDCDetail" runat="server" Style="min-height: 240px; min-width: 150px; height: auto; width: auto; max-width: 98%;">
                        <div class="container-fluid">
                            <div class="row">
                                <uc:ndcdetails runat="server" id="ucNDCDetails" />
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:Panel runat="server" ID="pnlsepNDCDetailsSearch" class="CollapsingSeparator"
                Visible="false" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerNDCDetailsSearch">
                <%-- <div class="pageHeader pH2" id="sepServiceDetail" runat="server" style="background-color: cornflowerblue; padding-left: 10px">SERVICE DETAILS</div>--%>
                <span id="sepNDCDetailsSearch" runat="server" class="pageHeader pH2">Search</span>
            </asp:Panel>
            <asp:Panel ID="pnlNDCDetailsSearch" Visible="false" runat="server" Style="min-height: 80px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <div class="row" style="text-align: center;">
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><span style="color: red"></span><b>NDC Code </b>
                                <asp:TextBox ID="txtNDCCode" runat="server" CssClass="formField" />

                            </span>

                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <span class="ohio-field-label"><b>Trade Name </b>
                                <asp:TextBox ID="txtTradeName" runat="server" Rows="2" CssClass="formField wd650"
                                    TextMode="MultiLine" MaxLength="1000" />
                                <asp:LinkButton ID="lnkNDCDetailSearch" Text="Search" runat="server" ToolTip="Search"
                                    CommandName="NDCDetailSearch" OnClick="lnkNDCDetailSearch_Click"
                                    OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>
                            </span>
                        </div>
                        <%--  <div class="col-sm-6 col-md-4 col-lg-3 ">
                    <asp:Button ID="btnConditionCodeInformationAdd" Text="Add" runat="server" OnClick="ConditionCodeInformationSearchAdd_Click"
                        CssClass="button" causesValidation="false" />
                    <asp:Button ID="btnConditionCodeInformationCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click"
                        CausesValidation="false" />
                </div>--%>
                        <div class="divNDCDetailsSearch" style="padding-top: 10px">
                            <asp:GridView ID="gvNDCDetailsSearch" runat="server" Width="80%" AllowSorting="false"
                                CssClass="gridview"
                                EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdNDCDetailsSearch_SelectedIndexChanged"
                                Style="margin-right: 100px">

                                <Columns>
                                    <asp:BoundField DataField="NDCCode" HeaderText="NDC Code" />
                                    <asp:BoundField DataField="TradeName" HeaderText="Trade Name" />
                                    <%-- <asp:LinkButton ID="lnkConditionCodesearch" Text="Search" runat="server" ToolTip="Search" 
                                        OnClick="lnkConditionCodeSearch_Click" OnClientClick="exportPopup();" 
                                        Visible="true"></asp:LinkButton>
                                    --%>
                                </Columns>
                                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                                <AlternatingRowStyle CssClass="gridViewAltRow" />
                                <RowStyle CssClass="gridViewRow" />
                                <FooterStyle CssClass="gridViewFooter" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <div>
                <uc:toothquadrantinfo runat="server" id="ucToothQuadrant" />
            </div>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <ajax:collapsiblepanelextender id="cpeAdditionalProviderinfo" runat="server" collapsed="true"
                        targetcontrolid="pnlAdditionalProviderinfo" expandcontrolid="pnlsepAdditionalProviderinfo"
                        collapsecontrolid="pnlsepAdditionalProviderinfo" />
                    <asp:Panel runat="server" ID="pnlsepAdditionalProviderinfo" class="CollapsingSeparator"
                        onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                        ToolTip="Click to Expand/Collapse" CssClass="OwnerAdditionalProviderinfo">
                        <span id="sepAdditionalProviderinfo" runat="server" class="pageHeader pH2">+ ADDITIONAL
                        PROVIDER INFORMATION-SERVICE DETAIL  </span>
                    </asp:Panel>
                    <asp:Panel ID="pnlAdditionalProviderinfo" runat="server" Style="overflow-x: hidden;">
                        <div class="row">
                            <uc:additionalproviderinfopanel runat="server" id="ucAdditionalProviderInfoPanel" />
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%-- Ambulance Pick up drop off --%>
            <div>
                <ucambulance:ambulancepickupdropoff runat="server" id="ucAmbulancePickUpDropOff" />
            </div>

            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <ajax:collapsiblepanelextender id="cpeotherPaymentamount" runat="server" collapsed="true"
                        targetcontrolid="pnlotherPaymentamount" expandcontrolid="pnlsepotherPaymentamount"
                        collapsecontrolid="pnlsepotherPaymentamount" />
                    <asp:Panel runat="server" ID="pnlsepotherPaymentamount" class="CollapsingSeparator"
                        onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                        ToolTip="Click to Expand/Collapse" CssClass="OwnerAdditionalProviderinfo">
                        <span id="sepotherPaymentamount" runat="server" class="pageHeader pH2">+ OTHER PAYER
                        PAID AMOUNT-SERVICE DETAIL SCREEN</span>
                    </asp:Panel>
                    <asp:Panel ID="pnlotherPaymentamount" runat="server" Style="min-height: 300px; min-width: 150px; height: auto; width: auto; max-width: 98%;">
                        <div class="container-fluid">
                            <div class="row">
                                <uc:otherpayerpaidamountservicedetail runat="server" id="ucOtherPayerPaidAmountServiceDetail" />
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:UpdatePanel ID="upnlOtherPayerPaidAdjustment" runat="server">
                <ContentTemplate>
                    <ajax:collapsiblepanelextender id="cpeotherpayerpaidadjustment" runat="server" collapsed="true"
                        targetcontrolid="pnlOPPAdjustment" expandcontrolid="pnlsepOPPAdjustment" collapsecontrolid="pnlsepOPPAdjustment" />
                    <asp:Panel runat="server" ID="pnlsepOPPAdjustment" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                        Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerAdditionalProviderinfo">
                        <span id="sepOPPAdjustment" runat="server" class="pageHeader pH2">+ OTHER PAYER ADJUSTMENT
                        INFORMATION-SERVICE DETAIL </span>
                    </asp:Panel>
                    <asp:Panel ID="pnlOPPAdjustment" runat="server" Style="overflow-x: hidden; height: 120px; padding-top: 20px; padding-bottom: 20px;">
                        <div class="container-fluid">
                            <div class="row">
                                <uc:otherpayeradjustmentservicedetail runat="server" id="ucOtherPayerAdjustmentServiceDetail" />
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>


            <ajax:collapsiblepanelextender id="cpeAttachment" runat="server" collapsed="true"
                targetcontrolid="pnlAttachment" expandcontrolid="pnlSepAttachment" collapsecontrolid="pnlSepAttachment" />
            <asp:Panel runat="server" ID="pnlSepAttachment" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerAttachment">
                <span id="sepAttachment" runat="server" class="pageHeader pH2">+ ATTACHMENT </span>
            </asp:Panel>
            <asp:Panel ID="pnlAttachment" runat="server" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; max-width: 98%;">
                <uc:claimauthattachment runat="server" id="ucClaimAttachment" visible="true" enableviewstate="true" />

            </asp:Panel>




            <asp:UpdatePanel ID="upnlProviderNote" runat="server">
                <ContentTemplate>
                    <ajax:collapsiblepanelextender id="Cpeprovidernote" runat="server" collapsed="true"
                        targetcontrolid="pnlProviderNote" expandcontrolid="pnlsepProvidersNotes" collapsecontrolid="pnlsepProvidersNotes" />
                    <asp:Panel runat="server" ID="pnlsepProvidersNotes" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                        Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerProviderNote">
                        <span id="sepProvidersNotes" runat="server" class="pageHeader pH2">+ PROVIDER NOTES
                        </span>
                    </asp:Panel>
                    <asp:Panel ID="pnlProviderNote" runat="server" Style="height: auto; overflow: hidden; min-height: 270px;">

                        <uc:providernotes runat="server" id="ucProviderNotes" visible="true" enableviewstate="true" />

                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

            <div id="divProviderBillingNote" runat="server">
                <asp:UpdatePanel ID="upnlProviderBillingNoteInst" runat="server">
                    <ContentTemplate>
                        <ajax:collapsiblepanelextender id="CollapsiblePanelExtender1" runat="server" collapsed="true"
                            targetcontrolid="pnlProviderBillingNoteInst" expandcontrolid="pnlsepProvidersBillingNotes" collapsecontrolid="pnlsepProvidersBillingNotes" />
                        <asp:Panel runat="server" ID="pnlsepProvidersBillingNotes" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                            Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerProviderNote">
                            <span id="Span2" runat="server" class="pageHeader pH2">+ PROVIDER BILLING NOTES
                            </span>
                        </asp:Panel>
                        <asp:Panel ID="pnlProviderBillingNoteInst" runat="server" Style="height: auto; overflow: hidden; min-height: 270px;">

                            <uc:providerbillingnotes runat="server" id="ucProviderBillingNotes" visible="true" enableviewstate="true" />

                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <%-- <div class="pageHeader pH2" id="sepServiceDetail" runat="server" style="background-color: cornflowerblue; padding-left: 10px">SERVICE DETAILS</div>--%>
            <ajax:collapsiblepanelextender id="cpereviewernoteprovider" runat="server" collapsed="true"
                targetcontrolid="pnlreviewernoteprovider"
                expandcontrolid="pnlsepreviewernoteprovider" collapsecontrolid="pnlsepreviewernoteprovider" />
            <asp:Panel runat="server" ID="pnlsepreviewernoteprovider" class="CollapsingSeparator"
                onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerProviderNote">
                <span id="sepreviewernoteprovider" runat="server" class="pageHeader pH2">+ REVIEWER
                NOTES </span>
            </asp:Panel>
            <asp:Panel ID="pnlreviewernoteprovider" runat="server" Style="height: auto; overflow-y: hidden; overflow-x: hidden">
                <div class="row">
                    <uc:reviewernotes runat="server" id="ReviewNotes" />
                </div>
            </asp:Panel>

            <ajax:collapsiblepanelextender id="cpeDelayedSubReSubinfo" runat="server" collapsed="true"
                targetcontrolid="pnlDelayedSubReSubinfo" expandcontrolid="pnlsepDelayedSubReSubinfo"
                collapsecontrolid="pnlsepDelayedSubReSubinfo" />
            <asp:Panel runat="server" ID="pnlsepDelayedSubReSubinfo" class="CollapsingSeparator"
                onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerDelayedSubReSubinfo">
                <span id="sepDelayedSubReSubinfo" runat="server" class="pageHeader pH2">+ DELAYED SUBMISSION/RESUBMISSION
                INFORMATION </span>
            </asp:Panel>
            <asp:Panel ID="pnlDelayedSubReSubinfo" runat="server" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; max-width: 98%;">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                    <div class="row">
                        <uc:delaysubmission runat="server" id="ucDelaySubmission" />
                    </div>
                </div>
            </asp:Panel>

            <ajax:collapsiblepanelextender id="cpeClaimAdjudication" runat="server" collapsed="true"
                targetcontrolid="pnlClaimAdjudication" expandcontrolid="pnlSepClaimAdjudication"
                collapsecontrolid="pnlSepClaimAdjudication" />
            <asp:Panel runat="server" ID="pnlsepClaimAdjudication" class="CollapsingSeparator"
                onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
                ToolTip="Click to Expand/Collapse" CssClass="OwnerClaimAdjudication">
                <span id="sepClaimAdjudication" runat="server" class="pageHeader pH2">+ CLAIM ADJUDICATION
                </span>
            </asp:Panel>
            <asp:Panel ID="pnlClaimAdjudication" runat="server" Style="min-height: 240px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">
                <div>
                    <asp:Label ID="lblErrorMsg6" runat="server" ForeColor="Red" Style="margin-left: 20px;"></asp:Label>
                </div>
                <div id="divClaimAdjudication" runat="server">
                    <div class="col-sm-6">
                        <div class="row" id="lblCaimStatus" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Claim Status :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtCaimStatus2" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px"
                                        ReadOnly="true" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lblICN" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">ICN :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtICN" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px !important;"
                                        ReadOnly="true" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lblTotalPaidAmount" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Total Paid Amount :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtTotalPaidAmount" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px"
                                        ReadOnly="true" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lblAdjudicationDate" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Adjudication Date :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtAdjudicationDate" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px"
                                        ReadOnly="true" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lblClaimSubmissionDate" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Claim Submission
                            Date :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtClaimSubmissionDate" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px"
                                        ReadOnly="true" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lblTotalCharges1" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Total Charges :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtTotalCharges1" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px"
                                        ReadOnly="true" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lblClaimPaidDate" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Claim Paid Date :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtClaimPaidDate" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px"
                                        ReadOnly="true" />
                                </span>
                            </div>

                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div3" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">CoPay Amount :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtCopayAmount" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px"
                                        ReadOnly="true" />
                                </span>
                            </div>

                        </div>
                    </div>
                </div>
            </asp:Panel>


            <ajax:collapsiblepanelextender id="CollapsibleClaimsXtenInformation" runat="server"
                collapsed="true" targetcontrolid="pnlClaimsXtenInformationInfo"
                expandcontrolid="pnlsepClaimsXtenInformationInfo" collapsecontrolid="pnlsepClaimsXtenInformationInfo" />
            <asp:Panel runat="server" ID="pnlsepClaimsXtenInformationInfo" class="CollapsingSeparator"
                onclick="javascript:CollapseExpand(this);"
                Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerEOBInfo">
                <span id="Span5" runat="server" class="pageHeader pH2">+ CLAIMSXTEN INFORMATION
                </span>
            </asp:Panel>
            <asp:Panel ID="pnlClaimsXtenInformationInfo" runat="server" Style="min-height: 240px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">

                <uc:claimsxteninformation id="ClaimsXtenInformation" runat="server" />

            </asp:Panel>

            <%-- Related ICN Screen  --%>
            <ajax:collapsiblepanelextender id="CollapsibleRelatedICNScreen" runat="server" collapsed="true"
                targetcontrolid="pnlRelatedICNScreenInfo"
                expandcontrolid="pnlsepRelatedICNScreenInfo" collapsecontrolid="pnlsepRelatedICNScreenInfo" />
            <asp:Panel runat="server" ID="pnlsepRelatedICNScreenInfo" class="CollapsingSeparator"
                onclick="javascript:CollapseExpand(this);"
                Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerEOBInfo">
                <span id="Span6" runat="server" class="pageHeader pH2">+ RELATED ICN SCREEN</span>
            </asp:Panel>
            <asp:Panel ID="pnlRelatedICNScreenInfo" runat="server" Style="min-height: 240px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">
                <uc:relatedicnscreen id="RelatedICNScreen" runat="server" />
            </asp:Panel>
            <%-- CARC and RARC --%>
            <ajax:collapsiblepanelextender id="CollapsibleCARCRARC" runat="server" collapsed="true"
                targetcontrolid="pnlCARCRARCInfo"
                expandcontrolid="pnlsepCARCRARCInfo" collapsecontrolid="pnlsepCARCRARCInfo" />
            <asp:Panel runat="server" ID="pnlsepCARCRARCInfo" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerEOBInfo">
                <span id="Span4" runat="server" class="pageHeader pH2">+CARC AND RARC INFORMATION
                </span>
            </asp:Panel>
            <asp:Panel ID="pnlCARCRARCInfo" runat="server" Style="min-height: 240px; min-width: 300px; height: auto; width: auto; max-width: 2200px;">

                <uc:carcrarcinformation id="CARCRARCInformation" runat="server" />

            </asp:Panel>
            <%--ADJUDICATION ERRORS--%>
            <ajax:collapsiblepanelextender id="CollapsibleADJUDICATION" runat="server" collapsed="true"
                targetcontrolid="pnlADJUDICATIONInfo"
                expandcontrolid="pnlSepADJUDICATIONInfo" collapsecontrolid="pnlSepADJUDICATIONInfo" />
            <asp:Panel runat="server" ID="pnlSepADJUDICATIONInfo" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerEOBInfo">
                <span id="Span1" runat="server" class="pageHeader pH2">+ADJUDICATION ERRORS
                </span>
            </asp:Panel>
            <asp:Panel ID="pnlADJUDICATIONInfo" runat="server" Style="min-height: 240px; min-width: 300px; height: auto; width: auto; max-width: 2200px;">

                <uc:adjudicationerrors id="AdjudicationError" runat="server" />

            </asp:Panel>

            <%--Malicious Attachments--%>
            <ajax:collapsiblepanelextender id="CollapsibleMaliciousAttachments" runat="server" collapsed="true"
                targetcontrolid="pnlMaliciousAttachments"
                expandcontrolid="pnlSepMaliciousAttachmentsInfo" collapsecontrolid="pnlSepMaliciousAttachmentsInfo" />
            <asp:Panel runat="server" ID="pnlSepMaliciousAttachmentsInfo" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerEOBInfo">
                <span id="Span7" runat="server" class="pageHeader pH2">+MALICIOUS ATTACHMENTS
                </span>
            </asp:Panel>
            <asp:Panel ID="pnlMaliciousAttachments" runat="server" Style="min-height: 240px; min-width: 300px; height: auto; width: auto; max-width: 2200px;">

                <uc:maliciousattachments id="ClaimMaliciousAttachments" runat="server" />

            </asp:Panel>

            <%--   <ajax:CollapsiblePanelExtender ID="cpeEOBInfo" runat="server" Collapsed="false" TargetControlID="pnlEOBInfo"
        ExpandControlID="pnlsepEOBInfo" CollapseControlID="pnlsepEOBInfo" />
    <asp:Panel runat="server" ID="pnlsepEOBInfo" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
        Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerEOBInfo">
        <span id="sepEOBInfo" runat="server" class="pageHeader pH2">+ EOB INFORMATION </span>
    </asp:Panel>
    <asp:Panel ID="pnlEOBInfo" runat="server" Style="min-height: 240px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">

        <div class="divGrid" style="padding-top: 10px">
            <asp:GridView ID="gvEOBInfo" runat="server" Width="70%" AllowSorting="false" CssClass="gridview"
                EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdEOBInfo_SelectedIndexChanged"
                Style="margin-right: 300px">
                <Columns>
                    <asp:BoundField DataField="" HeaderText="Detail Number" />
                    <asp:BoundField DataField="cde_eob" HeaderText="EOB Code" />
                    <asp:BoundField DataField="" HeaderText="EOB Explanation" />
                    <asp:BoundField DataField="CDE_CLM_ADJ_REASON" HeaderText="CARC" />
                    <asp:BoundField DataField="AMT_ADJUSTMENT" HeaderText="CARC Amount" />
                    <asp:BoundField DataField="" HeaderText="CARC Description" />
                    <asp:BoundField DataField="" HeaderText="RARC" />
                    <asp:BoundField DataField="" HeaderText="RARC Description" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDeleteEob" runat="server" CommandName="EOBInfo" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');"
                                AlternateText="Delete"
                                Visible="<%# CanUserViewDelete()  %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>--%>
            <%-- <div class="div EOBInfo">
            <asp:ImageButton ID="imgEOBInfo" runat="server" ImageUrl="~/Images/add.png" CommandName="EOBInfo" OnCommand="btnAdd_Click" ToolTip="Add" />
        </div>
    </asp:Panel>--%>

            <%--    <ajax:CollapsiblePanelExtender ID="cperelatedclaimsInfo" runat="server"  Collapsed="true"
        TargetControlID="pnlrelatedclaimsInfo" ExpandControlID="pnlseprelatedclaimsInfo"
        CollapseControlID="pnlseprelatedclaimsInfo"  />
    <asp:Panel runat="server" ID="pnlseprelatedclaimsInfo" class="CollapsingSeparator"
        onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc"
        ToolTip="Click to Expand/Collapse" CssClass="OwnerrelatedclaimsInfo">
        <span id="seprelatedclaimsInfo" runat="server" class="pageHeader pH2">+ RELATED CLAIMS
            INFORMATION </span>
    </asp:Panel>
    <asp:Panel ID="pnlrelatedclaimsInfo" runat="server" Style="min-height: 240px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">

        <div class="divrelatedclaimsInfo" style="padding-top: 10px">
            <asp:GridView ID="gvrelatedclaimsInfo" runat="server" Width="70%" AllowSorting="false"
                CssClass="gridview"
                EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdrelatedclaimsInfo_SelectedIndexChanged"
                Style="margin-right: 300px">
                <Columns>
                    <asp:BoundField DataField="" HeaderText="Related ICN" />
                    <asp:BoundField DataField="" HeaderText="Reason" />

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDeleteRelC" runat="server" CommandName="relatedclaimsInfo"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');"
                                AlternateText="Delete"
                                Visible="<%# CanUserViewDelete()  %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>

    </asp:Panel>--%>
            <%--<asp:UpdatePanel runat="server" ID="UpdatePaneSubmitClaimsButton">
        <ContentTemplate>--%>
            <asp:Panel ID="btnSubmitClaim" runat="server" CssClass="Button">
                <div style="text-align: center; padding-top: 4px; width: 95%;">

                    <div class="container-fluid">
                        <div class="row">

                            <%-- <div class="col-sm-6 text-left">
                        <asp:Label ID="lblTitlePS" runat="server" Text="Module Title" Cssclass="pageHeader pH2" />&nbsp;
                        <asp:Label ID="lblRegistrationIdPS" runat="server" Text="" Cssclass="pageHeader pH2" />
                    </div>--%>
                            <div class="col-sm-6 text-left" style="padding-left: 40px;">
                                <asp:CheckBox ID="chkPreDetClaim" Visible="false" runat="server" Text="This is a predetermination claim" />
                            </div>
                            <div class="col-sm-6 text-right" style="padding-right: 5px;">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="false" ValidationGroup="validateClaims"
                                    CssClass="buttonBoxFocus" OnClientClick="return validatePatientControlNumber();" OnClick="btnSave_Click"
                                    ToolTip="Save current screen data" />
                                <%--"this.disabled = true;return ValidatePortalErrors(this);--%> 
                                <asp:Button ID="btnSubmit" runat="server" CausesValidation="false" ValidationGroup="validateClaims" 
                                    Text="Submit" CssClass="buttonBoxFocus" OnClick="btnSubmit_Click" OnClientClick="this.disabled = true;return ValidatePortalErrors(this);" ClientIDMode="Static" />
                                <asp:Button ID="btnReSubmit" runat="server" CausesValidation="true" ValidationGroup="validateClaims"
                                    Text="ReSubmit" CssClass="buttonBoxFocus"
                                    UseSubmitBehavior="false" OnClick="btnReSubmit_Click" OnClientClick="CallBlockUIPostBack()" />
                                <asp:Button ID="btnCopy" runat="server" ValidationGroup="validateClaims" Text="Copy"
                                    CssClass="buttonBoxFocus"
                                    ToolTip="Take action" CausesValidation="true" OnClick="btnCopy_Click" OnClientClick="CallBlockUIPostBack()" />
                                <asp:Button ID="bntAdjust" runat="server" Text="Adjust" CssClass="buttonBoxFocus"
                                    ToolTip="Take action" CausesValidation="true" ValidationGroup="validateClaims"
                                    OnClick="btnAdjust_Click" OnClientClick="CallBlockUIPostBack()" />
                                <asp:Button ID="btnVoid" runat="server" Text="Void" CssClass="buttonBoxFocusred"
                                    ToolTip="Take action" CausesValidation="true" ValidationGroup="validateClaims" OnClick="btnVoid_Click" OnClientClick="CallBlockUIPostBack()" />
                                <asp:Button ID="btncancel" runat="server" CausesValidation="false" Text="Cancel"
                                    CssClass="buttonBoxFocusred" OnClientClick="if(!confirm('All data entry will be lost. Are you sure you want to cancel?')) return false; clrtbls(); clearTables(); CallBlockUIPostBack()"
                                    OnClick="btnCancel_Click"  />
                                <%--   --%>
                            </div>
                        </div>
                    </div>

                </div>
                <asp:Label runat="server" ID="lblErroronSave" ForeColor="Red"></asp:Label>


            </asp:Panel>
            <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
        </div>
        <ajax:modalpopupextender id="mpe" runat="server" popupcontrolid="pnlProNote" targetcontrolid="Button2"
            backgroundcssclass="modalBackground" />
        <asp:Panel ID="pnlProNote" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                <div class="popTitle">
                    <asp:Label ID="lblNoteTitle" CssClass="bodyTextBold" runat="server" Text="Title" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlMain" runat="server">
                <%--  0--%>
                <asp:MultiView ID="mltPopup" runat="server">
                    <asp:View ID="vwPriorAuthProvidersNotes" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:priorauthprovidersnotes runat="server" id="ucPriorAuthProvidersNotes" />
                            </div>
                        </div>

                    </asp:View>
                    <%--  1--%>
                    <asp:View ID="vwClaimProfessionalServiceDetails" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:claimprofessionalservicedetails runat="server" id="ucClaimProfessionalServiceDetails" />
                                <%--  <uc:PriorAuthServiceDetails runat="server" ID="ucPriorAuthServiceDetails" />--%>
                            </div>
                        </div>
                    </asp:View>
                    <%--2--%>
                    <asp:View ID="vwClaimDentalServicesDetails" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <%--<uc:PriorAuthDentalServiceDetails runat="server" ID="ucPriorAuthDentalServiceDetails" />--%>
                                <%--  <uc:ClaimDentalServicesDetails runat="server" ID="ucClaimDentalServicesDetails" />--%>
                            </div>
                        </div>
                    </asp:View>
                    <%--3--%>
                    <asp:View ID="vwSubmitClaimServiceDetails" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:submitclaimservicedetails runat="server" id="ucSubmitClaimServiceDetails" />
                            </div>
                        </div>
                    </asp:View>

                    <%--4--%>
                    <asp:View ID="vwDiagnosis" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:priorauthdiagnosis runat="server" id="ucPriorAuthDiagnosis" />
                            </div>
                        </div>
                    </asp:View>

                </asp:MultiView>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="Button2" Style="display: none" Text="ButtonDummy" />
        <ajax:modalpopupextender id="mpedoc" runat="server" popupcontrolid="pnlDocByMail"
            targetcontrolid="Button5" backgroundcssclass="modalBackground" />
        <asp:Panel ID="pnlDocByMail" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="pnlDocbyMailheader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                <div class="popTitle">
                    <asp:Label ID="lblDocumentbyMail" CssClass="bodyTextBold" runat="server" Text="Title" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlDocumentMail" runat="server">
                <asp:MultiView ID="mltDocumentbyMail" runat="server">
                    <asp:View ID="vwDocumentbyMail" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:priorauthdocumentbymail runat="server" id="ucPriorAuthDocumentbyMail" />
                            </div>
                        </div>
                    </asp:View>
                    <%--  sixth view--%>
                    <asp:View ID="vwAttachment" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:claimauthattachment runat="server" id="ucClaimAuthAttachment" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="Button5" Style="display: none" Text="ButtonDummy5" />

        <ajax:modalpopupextender id="tooth" runat="server" popupcontrolid="pnlToothQuadrantInfo2"
            targetcontrolid="Button1" backgroundcssclass="modalBackground" />
        <asp:Panel ID="pnlToothQuadrantInfo2" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="pnlHeader1" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                <div class="popTitle">
                    <asp:Label ID="lblpnlToothQuadrantInfo" CssClass="bodyTextBold" runat="server" Text="Title" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlTooth" runat="server">
                <asp:MultiView ID="mlttooth" runat="server">

                    <%--1--%>
                    <asp:View ID="vwOtherPayerPaidAmoutnAdjustment" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:submitclaimotherpayerpaidamoutnadjustment runat="server" id="ucSubmitClaimOtherPayerPaidAmoutnAdjustment" />
                            </div>
                        </div>
                    </asp:View>
                    <%--2--%>
                    <asp:View ID="vwNDCDetails" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:submitclaimndcdetails runat="server" id="ucSubmitClaimNDCDetails" />
                            </div>
                        </div>
                    </asp:View>
                    <%--3--%>
                    <asp:View ID="vwAdditionalProviderInformation" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:submitclaimadditionalproviderinformation runat="server" id="ucSubmitClaimAdditionalProviderInformation" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="Button1" Style="display: none" Text="ButtonDummy" />

        <ajax:modalpopupextender id="clam" runat="server" popupcontrolid="pnlSubmitClaimOtherProviderInfo"
            targetcontrolid="Button1" backgroundcssclass="modalBackground" />
        <asp:Panel ID="pnlSubmitClaimOtherProviderInfo" runat="server" CssClass="modalPopup"
            Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="pnlHeader2" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                <div class="popTitle">
                    <asp:Label ID="lblSubmitClaimOtherProviderInfo" CssClass="bodyTextBold" runat="server"
                        Text="Title" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlProviderInfo" runat="server">
                <asp:MultiView ID="mltProviderInfo" runat="server">
                    <%--0--%>
                    <asp:View ID="vwSubmitClaimOtherProviderInfo" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:submitclaimotherproviderinfo runat="server" id="ucSubmitClaimOtherProviderInfo" />
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="vwICD10ProcedureCodes" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:submitclaimicd10procedurecodes runat="server" id="ucSubmitClaimICD10ProcedureCodes" />

                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="Button3" Style="display: none" Text="ButtonDummy" />
        <ajax:modalpopupextender id="mpeSubmitClaimSearchPop" runat="server" popupcontrolid="pnlSubmitClaimSearchPop"
            targetcontrolid="Button9" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseCH9" />

        <asp:Panel ID="pnlSubmitClaimSearchPop" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="pnlSubmitClaimSearchPopHeader" CssClass="popHeader" runat="server"
                HorizontalAlign="Left">
                <asp:Button runat="server" ID="btnCloseCH9" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
                    CausesValidation="false" />
                <div class="popTitle">
                    <asp:Label ID="lblSubmitClaimSearchPop" CssClass="bodyTextBold" runat="server" Text="Title"
                        ForeColor="White" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlSubmitClaimSearch" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                    <div class="row">
                        <%-- <uc:SubmitClaimSearchPop runat="server" ID="ucSubmitClaimSearchPop" Visible="true"
                        EnableViewState="true" />--%>
                    </div>
                </div>
                <%--<asp:MultiView ID="mltSubmitClaimSearchPop" runat="server">
                <asp:View ID="vwSubmitClaimSearchPop" runat="server">
                    
                </asp:View>
            </asp:MultiView>--%>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="Button9" Style="display: none" Text="ButtonDummy9" />


        <ajax:modalpopupextender id="mpeSubmitClaimSearchPage" runat="server" popupcontrolid="pnlSubmitClaimSearchPage"
            targetcontrolid="Button8" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseCH8" />
        <asp:Panel ID="pnlSubmitClaimSearchPage" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="pnlSubmitClaimSearchPageHeader" CssClass="popHeader" runat="server"
                HorizontalAlign="Left">
                <asp:Button runat="server" ID="btnCloseCH8" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
                    CausesValidation="false" />
                <div class="popTitle">
                    <asp:Label ID="lblSubmitClaimSearchPage" CssClass="bodyTextBold" runat="server" Text="Title"
                        ForeColor="White" />
                </div>
            </asp:Panel>
            <asp:Panel ID="Panel6" runat="server">
                <asp:MultiView ID="mltSubmitClaimSearchPage" runat="server">
                    <asp:View ID="vwSubmitClaimSearchPage" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:submitclaimsearchpage runat="server" id="ucSubmitClaimSearchPage" visible="true"
                                    enableviewstate="true" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="Button8" Style="display: none" Text="ButtonDummy8" />


        <ajax:modalpopupextender id="mpeSubmitClaimSearchDiag" runat="server" popupcontrolid="pnlSubmitClaimSearchDiag"
            targetcontrolid="ButtonSearchDiag" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseDiag" />
        <asp:Panel ID="pnlSubmitClaimSearchDiag" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="pnlSubmitClaimSearchDiagHeader" CssClass="popHeader" runat="server"
                HorizontalAlign="Left">
                <asp:Button runat="server" ID="btnCloseDiag" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
                    CausesValidation="false" />
                <div class="popTitle">
                    <asp:Label ID="lblSubmitClaimSearchDiag" CssClass="bodyTextBold" runat="server" Text="Title"
                        ForeColor="White" />
                </div>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server">
                <asp:MultiView ID="mltSubmitClaimSearchDiag" runat="server">
                    <asp:View ID="vwSubmitClaimSearchDiag" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:priorauthdiagnosisseach runat="server" id="ucPriorAuthDiagnosisSeach" visible="true"
                                    enableviewstate="true" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="ButtonSearchDiag" Style="display: none" Text="ButtonSearchDiag" />

        <ajax:modalpopupextender id="mpeSubmitClaimSearchReason" runat="server" popupcontrolid="pnlSubmitClaimSearchReason"
            targetcontrolid="ButtonSearchReason" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseReason" />
        <asp:Panel ID="pnlSubmitClaimSearchReason" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="pnlSubmitClaimSearchReasonHeader" CssClass="popHeader" runat="server"
                HorizontalAlign="Left">
                <asp:Button runat="server" ID="btnCloseReason" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
                    CausesValidation="false" />
                <div class="popTitle">
                    <asp:Label ID="lblSubmitClaimSearchReason" CssClass="bodyTextBold" runat="server"
                        Text="Title" ForeColor="White" />
                </div>
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server">
                <asp:MultiView ID="mltSubmitClaimSearchReason" runat="server">
                    <asp:View ID="vwSubmitClaimSearchReason" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:submitclaimsearchreason runat="server" id="ucSubmitClaimSearchReason" visible="true"
                                    enableviewstate="true" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="ButtonSearchReason" Style="display: none" Text="ButtonSearchReson" />

        <ajax:modalpopupextender id="mpeSubmitClaimSearchProc" runat="server" popupcontrolid="pnlSubmitClaimSearchProcPop"
            targetcontrolid="ButtonSearchProc" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseProc" />
        <asp:Panel ID="pnlSubmitClaimSearchProcPop" runat="server" CssClass="modalPopup"
            Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="pnlSubmitClaimSearchProcHeader" CssClass="popHeader" runat="server"
                HorizontalAlign="Left">
                <asp:Button runat="server" ID="btnCloseProc" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
                    CausesValidation="false" />
                <div class="popTitle">
                    <asp:Label ID="lblSubmitClaimSearchProc" CssClass="bodyTextBold" runat="server" Text="Procedure Code Search"
                        ForeColor="White" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlSubmitClaimSearchProc" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                    <div class="row">
                        <uc:submitclaimsearchproc runat="server" id="ucSubmitClaimSearchProc" visible="true"
                            enableviewstate="true" />
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="ButtonSearchProc" Style="display: none" Text="ButtonSearchReson" />



        <ajax:modalpopupextender id="mpeSubmitClaimSearchNDC" runat="server" popupcontrolid="pnlSubmitClaimSearchNDC"
            targetcontrolid="ButtonSearchNDC" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseNDC" />
        <asp:Panel ID="pnlSubmitClaimSearchNDC" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="pnlSubmitClaimSearchNDCHeader" CssClass="popHeader" runat="server"
                HorizontalAlign="Left">
                <asp:Button runat="server" ID="btnCloseNDC" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
                    CausesValidation="false" />
                <div class="popTitle">
                    <asp:Label ID="lblSubmitClaimSearchNDC" CssClass="bodyTextBold" runat="server" Text="Title"
                        ForeColor="White" />
                </div>
            </asp:Panel>
            <asp:Panel ID="Panel4" runat="server">
                <asp:MultiView ID="mltSubmitClaimSearchNDC" runat="server">
                    <asp:View ID="vwSubmitClaimSearchNDC" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:submitclaimsearchndc runat="server" id="ucSubmitClaimSearchNDC" visible="true"
                                    enableviewstate="true" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="ButtonSearchNDC" Style="display: none" Text="ButtonSearchNDC" />


        <ajax:modalpopupextender id="mpeSubmitClaimSearchBill" runat="server" popupcontrolid="pnlSubmitClaimSearchBill"
            targetcontrolid="ButtonSearchBill" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseBill" />
        <asp:Panel ID="pnlSubmitClaimSearchBill" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="pnlSubmitClaimSearchBillHeader" CssClass="popHeader" runat="server"
                HorizontalAlign="Left">
                <asp:Button runat="server" ID="btnCloseBill" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
                    CausesValidation="false" />
                <div class="popTitle">
                    <asp:Label ID="lblSubmitClaimSearchBill" CssClass="bodyTextBold" runat="server" Text="Title"
                        ForeColor="White" />
                </div>
            </asp:Panel>
            <asp:Panel ID="Panel5" runat="server">
                <asp:MultiView ID="mltSubmitClaimSearchBill" runat="server">
                    <asp:View ID="vwSubmitClaimSearchBill" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:submitclaimsearchbill runat="server" id="ucSubmitClaimSearchBill" visible="true"
                                    enableviewstate="true" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="ButtonSearchBill" Style="display: none" Text="ButtonSearchBill" />
        <ajax:modalpopupextender id="mpeSubmitClaimSearchCCI" runat="server" popupcontrolid="pnlSubmitClaimSearchCCI"
            targetcontrolid="ButtonSearchCCI" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseCCI" />
        <asp:Panel ID="pnlSubmitClaimSearchCCI" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="pnlSubmitClaimSearchCCIHeader" CssClass="popHeader" runat="server"
                HorizontalAlign="Left">
                <asp:Button runat="server" ID="btnCloseCCI" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
                    CausesValidation="false" />
                <div class="popTitle">
                    <asp:Label ID="lblSubmitClaimSearchCCI" CssClass="bodyTextBold" runat="server" Text="Title"
                        ForeColor="White" />
                </div>
            </asp:Panel>
            <asp:Panel ID="Panel7" runat="server">
                <asp:MultiView ID="mltSubmitClaimSearchCCI" runat="server">
                    <asp:View ID="vwSubmitClaimSearchCCI" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">

                                <uc:submitclaimserchcci runat="server" id="ucSubmitClaimSerchCCI" visible="true"
                                    enableviewstate="true" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="ButtonSearchCCI" Style="display: none" Text="ButtonSearchCCI" />

        <asp:Button runat="server" ID="Button12" Style="display: none" Text="ButtonDummy8" />


        <ajax:modalpopupextender id="ModalPopupExtender1" runat="server" popupcontrolid="pnlSubmitClaimSearchDiag"
            targetcontrolid="ButtonSearchDiag" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseDiag" />
        <asp:Panel ID="Panel3" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="Panel8" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                <asp:Button runat="server" ID="Button13" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
                    CausesValidation="false" />
                <div class="popTitle">
                    <asp:Label ID="Label11" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
                </div>
            </asp:Panel>
            <asp:Panel ID="Panel9" runat="server">
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:priorauthdiagnosisseach runat="server" id="PriorAuthDiagnosisSeach1" visible="true"
                                    enableviewstate="true" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="Button19" Style="display: none" Text="ButtonSearchDiag" />

        <ajax:modalpopupextender id="ModalPopupExtender2" runat="server" popupcontrolid="pnlSubmitClaimSearchReason"
            targetcontrolid="ButtonSearchReason" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseReason" />
        <asp:Panel ID="PanelSubmitClaimSearch" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="Panel11" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                <asp:Button runat="server" ID="Button20" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
                    CausesValidation="false" />
                <div class="popTitle">
                    <asp:Label ID="Label12" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
                </div>
            </asp:Panel>
            <asp:Panel ID="Panel12" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                    <div class="row">
                        <uc:submitclaimsearchreason runat="server" id="SubmitClaimSearchReason1" visible="true"
                            enableviewstate="true" />
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="Button21" Style="display: none" Text="ButtonSearchReson" />

        <ajax:modalpopupextender id="ModalPopupExtender3" runat="server" popupcontrolid="pnlSubmitClaimSearchProcPop"
            targetcontrolid="ButtonSearchProc" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseProc" />
        <asp:Panel ID="Panel13" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="Panel14" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                <asp:Button runat="server" ID="Button22" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
                    CausesValidation="false" />
                <div class="popTitle">
                    <asp:Label ID="Label13" CssClass="bodyTextBold" runat="server" Text="Procedure Code Search"
                        ForeColor="White" />
                </div>
            </asp:Panel>
            <asp:Panel ID="Panel15" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                    <div class="row">
                        <uc:submitclaimsearchproc runat="server" id="SubmitClaimSearchProc1" visible="true"
                            enableviewstate="true" />
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="Button23" Style="display: none" Text="ButtonSearchReson" />



        <ajax:modalpopupextender id="ModalPopupExtender4" runat="server" popupcontrolid="pnlSubmitClaimSearchNDC"
            targetcontrolid="ButtonSearchNDC" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseNDC" />
        <asp:Panel ID="Panel16" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="Panel17" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                <asp:Button runat="server" ID="Button24" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
                    CausesValidation="false" />
                <div class="popTitle">
                    <asp:Label ID="Label14" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
                </div>
            </asp:Panel>
            <asp:Panel ID="Panel18" runat="server">
                <asp:MultiView ID="MultiView2" runat="server">
                    <asp:View ID="View2" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:submitclaimsearchndc runat="server" id="SubmitClaimSearchNDC1" visible="true"
                                    enableviewstate="true" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="Button25" Style="display: none" Text="ButtonSearchNDC" />


        <ajax:modalpopupextender id="ModalPopupExtender5" runat="server" popupcontrolid="pnlSubmitClaimSearchBill"
            targetcontrolid="ButtonSearchBill" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseBill" />
        <asp:Panel ID="Panel19" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="Panel20" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                <asp:Button runat="server" ID="Button26" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
                    CausesValidation="false" />
                <div class="popTitle">
                    <asp:Label ID="Label15" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
                </div>
            </asp:Panel>
            <asp:Panel ID="Panel21" runat="server">
                <asp:MultiView ID="MultiView3" runat="server">
                    <asp:View ID="View3" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">
                                <uc:submitclaimsearchbill runat="server" id="SubmitClaimSearchBill1" visible="true"
                                    enableviewstate="true" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="Button27" Style="display: none" Text="ButtonSearchBill" />
        <ajax:modalpopupextender id="ModalPopupExtender6" runat="server" popupcontrolid="pnlSubmitClaimSearchCCI"
            targetcontrolid="ButtonSearchCCI" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseCCI" />
        <asp:Panel ID="Panel22" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="Panel23" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                <asp:Button runat="server" ID="Button28" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
                    CausesValidation="false" />
                <div class="popTitle">
                    <asp:Label ID="Label16" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
                </div>
            </asp:Panel>
            <asp:Panel ID="Panel24" runat="server">
                <asp:MultiView ID="MultiView4" runat="server">
                    <asp:View ID="View4" runat="server">
                        <div style="text-align: left; padding: 15px" class="container-fluid">
                            <div class="row">

                                <uc:submitclaimserchcci runat="server" id="SubmitClaimSerchCCI1" visible="true" enableviewstate="true" />
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="Button29" Style="display: none" Text="ButtonSearchCCI" />
        <!-- The Modal -->
        <div class="modal " id="myModal">
            <div class="modal-dialog">
                <div class="modal-content modalPopup">
                    <input type="hidden" id="hdnSearchId" />
                    <asp:HiddenField ID="hdnbtnSearch" runat="server" />
                    <!-- Modal Header -->
                    <div class="popHeader popUpHeader popTitle">
                        <span class="col-sm-2" style="text-align: center">
                            <asp:Label ID="Label1" Text="NPI" runat="server"></asp:Label></span>
                        <span>
                            <asp:Label ID="lblsearchnpi" Text="MEDICAID ID" runat="server"></asp:Label></span>
                        <span style="padding-left: 150px;">
                            <asp:Label ID="Label2" Text="BUSINESS/LAST NAME" runat="server"></asp:Label></span>
                        <span style="padding-left: 230px;">
                            <asp:Label ID="Label3" Text="FIRST NAME" runat="server"></asp:Label></span>

                        <button type="button" class="close" data-dismiss="modal" id="btncloseNPI">
                            &times;</button>
                    </div>

                    <asp:UpdatePanel runat="server" ID="upmodalBody">
                        <ContentTemplate>
                            <div>
                                <%-- <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                                    CssClass="failureNotification"
                                    ValidationGroup="valSearchNpi" ShowSummary="true" />--%>
                            </div>
                            <div id="divErrorMessageSearchNpi" runat="server" visible="false" class="failureNotification">
                                At least a field is required
                            </div>

                            <div class="row  m-0 popUpSearch-Context d-FlexCenter" style="text-align: center;">
                                <div class="col-sm-6 col-md-4 col-lg-2">
                                    <asp:TextBox ID="txtHCPCSCode" CssClass="ohio-field-input" runat="server" MaxLength="10">
                                    </asp:TextBox>
                                    <asp:Label ID="lblnpisearchpopupTxTNPI" runat="server" CssClass="failureNotification"></asp:Label>
                                    <%--     <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator6"
                                        Display="Dynamic"
                                        ValidationGroup="valSearchNpi"
                                        ControlToValidate="txtHCPCSCode"
                                        ValidationExpression="^[0-9]{10}$"
                                        ErrorMessage="10-digit number is required"
                                        Text="*10-digit number is required"
                                        CssClass="failureNotification">	
                                    </asp:RegularExpressionValidator>--%>
                                    </span>

                                </div>

                                <div class="col-sm-6 col-md-4 col-lg-2 ">
                                    <asp:TextBox ID="txtMedicaidid1" CssClass="ohio-field-input" runat="server">
                                    </asp:TextBox>
                                    <asp:RegularExpressionValidator runat="server" ID="tfvMedicaidId1"
                                        Display="Dynamic"
                                        ValidationGroup="valSearchNpi"
                                        ControlToValidate="txtMedicaidid1"
                                        ValidationExpression="^[0-9]{7}$"
                                        ErrorMessage="7-digit number is required"
                                        Text="*7-digit number is required"
                                        CssClass="failureNotification">	
                                       
                                    </asp:RegularExpressionValidator>
                                    </span>

                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-5 ">
                                    <asp:TextBox ID="txtBusinessLastName" CssClass="ohio-field-input" runat="server"
                                        MaxLength="60">
                                    </asp:TextBox>
                                    </span>

                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-2 ">
                                    <asp:TextBox ID="txtFirstName" CssClass="ohio-field-input" runat="server" MaxLength="35">
                                    </asp:TextBox>
                                    </span>
                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-3 " style="text-align: left; margin-left: 20px;">
                                    <button id="btnloading2" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>

                                    <br />

                                    <asp:Button ID="btnSearch" OnClientClick="return GetNPIDetails()" runat="server" CssClass="buttonBox StepButton buttonBoxFocus" Text="Search" CausesValidation="false" />


                                    <%-- <asp:Button ID="btnSearch"  runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus"
                                        OnClick="btnSearch_Click" ValidationGroup="valSearchNpi"
                                        Style="background-color: darkslateblue !important" />--%>
                                </div>
                            </div>
                            <br />
                            <div class="search-Results">SEARCH RESULTS</div>
                            <asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="Search Result"
                                Visible="false" Style="font-weight: bold;"></asp:Label>
                            <div class="result-Container">
                                <div class="popupGridViewOnSearch">
                                    <mms:sortablepaginggridview
                                        id="gvSubmitClaimSearchPage"
                                        runat="server"
                                        autogeneratecolumns="False"
                                        cssclass="gridViewSmallFont" width="100%"
                                        allowsorting="true"
                                        emptydatatext="An internal error has occurred, please try your search again."
                                        onpageindexchanging="gvSubmitClaimSearchPage_PageIndexChanging"
                                        onsorting="gvHSCPSCodeSearch_Sorting"
                                        rowstyle-verticalalign="Top"
                                        allowpaging="True"
                                        pagesize="15"
                                        gridviewsortcolumn="Code" gridviewsortdirection="Ascending"
                                        datakeynames="MEDICAID_ID, LAST_OR_BUSINESS_NAME,FIRST_NAME">
                                        <columns>
                                        </columns>
                                    </mms:sortablepaginggridview>






                                    <%--  <mms:SortablePagingGridView
                                        ID="gvSubmitClaimSearchPage"
                                        runat="server"
                                        AutoGenerateColumns="False"
                                        CssClass="gridViewSmallFont" Width="100%"
                                        AllowSorting="false"
                                        EmptyDataText="No Providers found."
                                        OnPageIndexChanging="gvSubmitClaimSearchPage_PageIndexChanging"
                                        OnSorting="gvHSCPSCodeSearch_Sorting"
                                        RowStyle-VerticalAlign="Top"
                                        AllowPaging="True"
                                        PageSize="15"
                                        GridViewSortColumn="MEDICAID_ID" GridViewSortDirection="Ascending"
                                        DataKeyNames="MEDICAID_ID, LAST_OR_BUSINESS_NAME,FIRST_NAME">
                                        <Columns>

                                            <asp:TemplateField HeaderText="NPI" ItemStyle-Width="100" ItemStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkNPI" runat="server" ToolTip="Search" Text='<%# Eval("NPI") %>'
                                                        CommandArgument='<%# Eval("NPI") %>' OnClick="lnkNPI_Click" CausesValidation="false">
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MEDICAID_ID" ItemStyle-Width="100" ItemStyle-Wrap="true">
                                                <ItemTemplate>
                                                <asp:LinkButton ID="lnkMEDICAID_ID" runat="server" ToolTip="Search" Text='<%# Eval("MEDICAID_ID") %>'
                                                CommandArgument='<%# Eval("MEDICAID_ID") %>' OnClick="lnkNPI_Click" CausesValidation="false">
                                                </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="MEDICAID_ID" HeaderText="Medicaid ID" SortExpression="MEDICAID_ID" />--%>
                                    <%--<asp:BoundField DataField="LAST_OR_BUSINESS_NAME" HeaderText="Business/Last Name"
                                                SortExpression="LAST_OR_BUSINESS_NAME" />
                                            <asp:BoundField DataField="FIRST_NAME" HeaderText="First Name" SortExpression="FIRST_NAME" />
                                            <asp:BoundField DataField="ADDRESS1" HeaderText="Address Line 1" SortExpression="ADDRESS1">
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ADDRESS2" HeaderText="Address Line 2" SortExpression="ADDRESS2" />
                                            <asp:BoundField DataField="CITY" HeaderText="City" SortExpression="CITY"></asp:BoundField>
                                            <asp:BoundField DataField="STATE" HeaderText="State" SortExpression="STATE"></asp:BoundField>
                                            <asp:BoundField DataField="ZIP" HeaderText="Zip" SortExpression="ZIP"></asp:BoundField>
                                        </Columns>
                                    </mms:SortablePagingGridView>--%>
                                    <asp:HiddenField ID="hdnNPI" runat="server" />
                                    <asp:HiddenField ID="hdnMedicaidId" runat="server" />
                                    <asp:HiddenField ID="hdnProviderName" runat="server" />
                                    <asp:HiddenField ID="hdnProviderLastName" runat="server" />
                                    <asp:HiddenField ID="hdnProviderFirstName" runat="server" />
                                    <asp:HiddenField ID="hdnProviderAddress" runat="server" />
                                    <asp:HiddenField ID="hdnProviderCity" runat="server" />
                                    <asp:HiddenField ID="hdnProviderState" runat="server" />
                                    <asp:HiddenField ID="hdnProviderZip" runat="server" />
                                    <asp:HiddenField ID="hdnProviderAddress2" runat="server" />
                                    <asp:HiddenField ID="hdnEntityType" runat="server" />
                                </div>
                            </div>
                            </div>
                        <asp:Button runat="server" ID="btnsearchnpi" Style="display: none" Text="" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <%-- Search npi popup end --%>
        <div class="modal" id="myDiagModal">
            <div class="modal-dialog">
                <div class="modal-content modalPopup">

                    <!-- Modal Header -->
                    <div class="popHeader popUpHeader popTitle">
                        <span class="col-sm-2" style="text-align: center">
                            <asp:Label ID="Label5" Text="DIAGNOSIS CODE" runat="server"></asp:Label></span>
                        <span style="padding-left: 60px;">
                            <asp:Label ID="Label6" Text="ICD VERSION" runat="server"></asp:Label></span>
                        <span style="padding-left: 190px;">
                            <asp:Label ID="Label7" Text="DIAGNOSIS DESCRIPTION" runat="server"></asp:Label>
                        </span>
                        <asp:Button runat="server" ID="btntestbtn" CssClass="close" Text="X"></asp:Button>
                    </div>
                    <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                        <ContentTemplate>
                            <input type="hidden" id="hdnDiagnosisCode" runat="server" />
                            <input type="hidden" id="hdnDiagnosisVersion" runat="server" />
                            <input type="hidden" id="hdnDiagnosisDes" runat="server" />
                            <div class="modal-body">
                                <div class="m-0 popUpSearch-Context d-FlexCenter" style="text-align: center;">
                                    <div class="col-sm-6 col-md-4 col-lg-2 ">
                                        <asp:TextBox ID="txtDiagnosisCodeSearch" MaxLength="7" CssClass="ohio-field-input"
                                            runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-6 col-md-4 col-lg-2">
                                        <asp:DropDownList ID="ddlICDVersion" runat="server" Style="height: 37px; min-width: 200px">
                                            <asp:ListItem Value="ICD 10" Text="ICD 10"></asp:ListItem>
                                            <asp:ListItem Value="ICD 9" Text="ICD 9"></asp:ListItem>
                                        </asp:DropDownList>
                                        </span>
                                    </div>
                                    <div class="col-sm-6 col-md-4 col-lg-5 " style="margin-left: 20px">
                                        <asp:TextBox ID="txtDiagnosisCodeDescSearch" MaxLength="400" CssClass="ohio-field-input"
                                            runat="server">
                                        </asp:TextBox>
                                        </span>

                                    </div>
                                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                                        <button id="btnloading1" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>

                                        <br />
                                        <asp:Button ID="test1" runat="server" CausesValidation="false" Text="Search" CssClass="buttonBox StepButton buttonBoxFocus" OnClientClick="return GetDiagnosisCode()" />


                                    </div>
                                    <div id="DiagSearchError" class="form-group row" runat="server">
                                        <asp:Label ID="lblSearchEntryError" runat="server" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                                <br />
                                <div class="search-Results">SEARCH RESULTS</div>
                                <asp:Label ID="lblDiagSearch" class="expandcollapse" runat="server" Text="Search Result"
                                    Visible="false"></asp:Label>
                                <div class="result-Container">
                                    <div class="popupGridViewOnSearch">

                                        <mms:sortablepaginggridview
                                            id="gvClaimDiagnosisSearch"
                                            runat="server"
                                            autogeneratecolumns="False"
                                            cssclass="gridViewSmallFont" width="100%"
                                            allowsorting="true"
                                            emptydatatext="No Providers found."
                                            rowstyle-verticalalign="Top"
                                            allowpaging="True"
                                            pagesize="15"
                                            gridviewsortcolumn="Code" gridviewsortdirection="Ascending">
                                            <columns>
                                            </columns>
                                        </mms:sortablepaginggridview>



                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <div class="modal" id="myModalTOB">
            <div class="modal-dialog">
                <div class="modal-content">
                    <input type="hidden" id="hdnSearchIdTOB" />
                    <!-- Modal Header -->
                    <div class="modal-header">
                        <span class="modal-title">Search</span>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <asp:UpdatePanel runat="server" ID="upmodalBodyTOB">
                        <ContentTemplate>
                            <div class="modal-body">
                                <%--                            <uc:SubmitClaimSearchBill runat="server" ID="SubmitClaimSearchBill1" Visible="true" EnableViewState="true" />--%>

                                <div>
                                    <asp:ValidationSummary ID="ValidationSummary8" runat="server" DisplayMode="List"
                                        ValidationGroup="CheckEligibility" ShowSummary="true" />
                                </div>

                                <div class="row" style="text-align: center; margin-left: 10px; margin-right: 10px;">
                                    <div class="row" style="text-align: left;">
                                        <div class="col-sm-2">
                                            <span class="ohio-field-label"><b>&nbsp;Type of Bill</b>
                                                <asp:TextBox ID="txtSearchTOBCode" CssClass="ohio-field-input" runat="server" Width="200px"></asp:TextBox>
                                            </span>
                                        </div>

                                        <div class="col-sm-7">
                                            <span class="ohio-field-label"><b>&nbsp;Type of Bill Description</b>
                                                <asp:TextBox ID="txtSearchTOBDesc" CssClass="ohio-field-input" runat="server"></asp:TextBox>
                                                <%--                                            <asp:LinkButton ID="LinkButton3" Text="Search" runat="server" ToolTip="Search" OnClick="LinkButton8_Click" Visible="true"></asp:LinkButton>--%>
                                            </span>
                                        </div>

                                        <div class="col-sm-2" style="padding-top: 12px;">
                                            <asp:Button ID="Button30" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus"
                                                OnClick="btnSearch_ClickTOB" ValidationGroup="ProviderSearch" OnClientClick="showProgress()"
                                                Style="background-color: darkslateblue !important" />
                                        </div>
                                    </div>

                                </div>
                                <asp:Label ID="Label17" class="expandcollapse" runat="server" Text="Search Result"
                                    Visible="false"></asp:Label>
                                <div style="margin: 20px; width: 95%;">
                                    <mms:sortablepaginggridview
                                        id="gvSubmitClaimSearchPop"
                                        runat="server"
                                        autogeneratecolumns="False"
                                        cssclass="gridViewSmallFont"
                                        allowsorting="true"
                                        emptydatatext="No Providers found."
                                        rowstyle-verticalalign="Top"
                                        allowpaging="True"
                                        pagesize="10"
                                        gridviewsortcolumn="TOBCode"
                                        gridviewsortdirection="Ascending"
                                        datakeynames="TOBCode, TOBDesc" headerstyle-horizontalalign="Center" width="90%">

                                        <pagersettings mode="Numeric" position="Bottom" />

                                        <columns>

                                            <asp:TemplateField HeaderText="Type of Bill" ItemStyle-Wrap="true" ItemStyle-Width="100px">
                                                <itemtemplate>
                                                    <asp:LinkButton ID="lnkTOB" runat="server" ToolTip="Search" Text='<%# Eval("TOBCode") %>'
                                                        CommandArgument='<%# Eval("TOBCode") %>' OnClick="lnkTOB_Click">
                                                    </asp:LinkButton>
                                                </itemtemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="TOBDesc" HeaderText="Type of Bill Description" SortExpression="TOBDesc"
                                                ItemStyle-Width="400px" />

                                        </columns>
                                    </mms:sortablepaginggridview>
                                </div>
                                <asp:HiddenField ID="hdnTOB" runat="server" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <%--Ajax SearchNpi for NPI Text--%>
        <div class="modal " id="myModalpop">
            <div class="modal-dialog">
                <div class="modal-content modalPopup">
                    <input type="hidden" id="hdnSearchIdpop" />
                    <asp:HiddenField ID="hdnTxtSearchpop" runat="server" />
                    <!-- Modal Header -->
                    <div class="popHeader popUpHeader popTitle">
                        <span class="col-sm-2" style="text-align: center">
                            <asp:Label ID="lblNPIPopup" Text="NPI" runat="server"></asp:Label></span>
                        <span>
                            <asp:Label ID="lblMedicadidpop" Text="MEDICAID ID" runat="server"></asp:Label></span>
                        <span style="padding-left: 150px;">
                            <asp:Label ID="lblLNamepop" Text="BUSINESS/LAST NAME" runat="server"></asp:Label></span>
                        <span style="padding-left: 230px;">
                            <asp:Label ID="lblFNamepop" Text="FIRST NAME" runat="server"></asp:Label></span>

                        <button type="button" class="close" data-dismiss="modal" id="btncloseNPIPop">
                            &times;</button>
                    </div>

                    <asp:UpdatePanel runat="server" ID="UpdatePanel6">
                        <ContentTemplate>
                            <div>
                                <%-- <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                                    CssClass="failureNotification"
                                    ValidationGroup="valSearchNpi" ShowSummary="true" />--%>
                            </div>
                            <div id="dvNpiPop" runat="server" visible="false" class="failureNotification">
                                At least a field is required
                            </div>

                            <div class="row  m-0 popUpSearch-Context d-FlexCenter" style="text-align: center;">
                                <div class="col-sm-6 col-md-4 col-lg-2">
                                    <asp:TextBox ID="txtNPIpop" CssClass="ohio-field-input" runat="server" MaxLength="10">
                                    </asp:TextBox>
                                    <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1"
                                        Display="Dynamic"
                                        ValidationGroup="valSearchNpiPop"
                                        ControlToValidate="txtNPIpop"
                                        ValidationExpression="^[0-9]{10}$"
                                        ErrorMessage="10-digit number is required"
                                        Text="*10-digit number is required"
                                        CssClass="failureNotification">	
                                    </asp:RegularExpressionValidator>
                                    </span>

                                </div>

                                <div class="col-sm-6 col-md-4 col-lg-2 ">
                                    <asp:TextBox ID="txtMedicaididPop" CssClass="ohio-field-input" runat="server">
                                    </asp:TextBox>
                                    <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator2"
                                        Display="Dynamic"
                                        ValidationGroup="valSearchNpiPop"
                                        ControlToValidate="txtMedicaididPop"
                                        ValidationExpression="^[0-9]{7}$"
                                        ErrorMessage="7-digit number is required"
                                        Text="*7-digit number is required"
                                        CssClass="failureNotification">	
                                       
                                    </asp:RegularExpressionValidator>
                                    </span>

                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-5 ">
                                    <asp:TextBox ID="txtlNamedPop" CssClass="ohio-field-input" runat="server"
                                        MaxLength="60">
                                    </asp:TextBox>
                                    </span>

                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-2 ">
                                    <asp:TextBox ID="txtFNamedPop" CssClass="ohio-field-input" runat="server" MaxLength="35">
                                    </asp:TextBox>
                                    </span>
                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-3 " style="text-align: left; margin-left: 20px;">
                                    <button id="btnNPILoadPop" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>

                                    <br />
                                    <asp:Button ID="btnNPISearchPop" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus"
                                        OnClick="btnNPISearchPop_Click" ValidationGroup="valSearchNpiPop"
                                        Style="background-color: darkslateblue !important" />

                                </div>
                            </div>
                            <br />
                            <div class="search-Results">SEARCH RESULTS</div>
                            <asp:Label ID="lblNPIPgrdop" class="expandcollapse" runat="server" Text="Search Result"
                                Visible="false" Style="font-weight: bold;"></asp:Label>
                            <div class="result-Container">
                                <div class="popupGridViewOnSearch">
                                    <mms:sortablepaginggridview
                                        id="gvSubmitClaimSearchPagePop"
                                        runat="server"
                                        autogeneratecolumns="False"
                                        cssclass="gridViewSmallFont" width="100%"
                                        allowsorting="false"
                                        emptydatatext="No Providers found."
                                        onpageindexchanging="gvSubmitClaimSearchPagePop_PageIndexChanging"
                                        onsorting="gvSubmitClaimSearchPagePop_Sorting"
                                        rowstyle-verticalalign="Top"
                                        allowpaging="True"
                                        pagesize="15"
                                        gridviewsortcolumn="MEDICAID_ID" gridviewsortdirection="Ascending"
                                        datakeynames="MEDICAID_ID, LAST_OR_BUSINESS_NAME,FIRST_NAME">
                                        <columns>

                                            <asp:TemplateField HeaderText="NPI" ItemStyle-Width="100" ItemStyle-Wrap="true">
                                                <itemtemplate>
                                                    <asp:LinkButton ID="lnkNPIpop" runat="server" ToolTip="Search" Text='<%# Eval("NPI") %>'
                                                        CommandArgument='<%# Eval("NPI") %>' OnClick="lnkNPIpop_Click" CausesValidation="false">
                                                    </asp:LinkButton>
                                                </itemtemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MEDICAID_ID" ItemStyle-Width="100" ItemStyle-Wrap="true">
                                                <itemtemplate>
                                                    <asp:LinkButton ID="lnkMEDICAID_IDpop" runat="server" ToolTip="Search" Text='<%# Eval("MEDICAID_ID") %>'
                                                        CommandArgument='<%# Eval("MEDICAID_ID") %>' OnClick="lnkNPIpop_Click" CausesValidation="false">
                                                    </asp:LinkButton>
                                                </itemtemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="MEDICAID_ID" HeaderText="Medicaid ID" SortExpression="MEDICAID_ID" />--%>
                                            <asp:BoundField DataField="LAST_OR_BUSINESS_NAME" HeaderText="Business/Last Name"
                                                SortExpression="LAST_OR_BUSINESS_NAME" />
                                            <asp:BoundField DataField="FIRST_NAME" HeaderText="First Name" SortExpression="FIRST_NAME" />
                                            <asp:BoundField DataField="ADDRESS1" HeaderText="Address Line 1" SortExpression="ADDRESS1"></asp:BoundField>
                                            <asp:BoundField DataField="ADDRESS2" HeaderText="Address Line 2" SortExpression="ADDRESS2" />
                                            <asp:BoundField DataField="CITY" HeaderText="City" SortExpression="CITY"></asp:BoundField>
                                            <asp:BoundField DataField="STATE" HeaderText="State" SortExpression="STATE"></asp:BoundField>
                                            <asp:BoundField DataField="ZIP" HeaderText="Zip" SortExpression="ZIP"></asp:BoundField>
                                        </columns>
                                    </mms:sortablepaginggridview>
                                    <asp:HiddenField ID="hdnNPIpop" runat="server" />
                                    <asp:HiddenField ID="hdnMedicaidIdpop" runat="server" />
                                    <asp:HiddenField ID="hdnProviderNamepop" runat="server" />
                                    <asp:HiddenField ID="hdnProviderLastNamepop" runat="server" />
                                    <asp:HiddenField ID="hdnProviderFirstNamepop" runat="server" />
                                    <asp:HiddenField ID="hdnProviderAddresspop" runat="server" />
                                    <asp:HiddenField ID="hdnProviderCitypop" runat="server" />
                                    <asp:HiddenField ID="hdnProviderStatepop" runat="server" />
                                    <asp:HiddenField ID="hdnProviderZippop" runat="server" />
                                    <asp:HiddenField ID="hdnProviderAddress2pop" runat="server" />
                                    <asp:HiddenField ID="hdnEntityTypepop" runat="server" />
                                </div>
                            </div>
                            </div>
                        <asp:Button runat="server" ID="btnNIPPop" Style="display: none" Text="" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <%--<ajax:ModalPopupExtender ID="modalFIFailure" runat="server" PopupControlID="pnmFIFailure" TargetControlID="ButtonDummyFIFailure"
                BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnmFIFailure" CancelControlID="btnCloseFilure">
            </ajax:ModalPopupExtender>
            <asp:Panel ID="pnmFIFailure" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 25%;">
                <div class="row">
                    <div style="text-align: left; padding: 15px;color: cornflowerblue" class="container-fluid"  >
                        <%--<asp:Image ID="ImageTXCancel" runat="server" Width="30%" Height="30%" ImageUrl="~/Images/cancel.png" />--%>
            <%-- <asp:Label ID="lblFIResponseID" runat="server" Font-Bold="true" Text=""></asp:Label><br />

                    </div>
                    <div class="col-sm-6 col-md-8 col-lg-9" style="width:100%">
                        <%--<asp:Label ID="Label4" runat="server" Font-Bold="true" Text="Error"></asp:Label><br />--%>
            <%--<asp:GridView ID="grdFIResponse" runat="server"  Width="100%" BorderStyle="None" AllowPaging="true"
                            GridLines="None"  RowStyle-BackColor="White" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField HeaderText="Line Number" ItemStyle-Width="10%">
                 <ItemTemplate>
                     <%# Container.DataItemIndex + 1 %>
                 </ItemTemplate>--%>
            <%-- <ItemStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
		<HeaderStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
             </asp:TemplateField>--%>
            <%--<asp:BoundField DataField="ErrorCode" HeaderText="ErrorCode"  ItemStyle-Width="10%" >
                                    <ItemStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
		<HeaderStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ErrorDescription" HeaderText="ErrorDescp"   ItemStyle-Width="60%" >
                                        <ItemStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
		<HeaderStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
                                    </asp:BoundField>--%>
            <%--</Column
                        </asp:GridView>
                        <asp:Label ID="lblFIErrors" runat="server" Font-Bold="true" Text=""></asp:Label><br />
                        <asp:Label ID="lblSupport" runat="server" Font-Bold="true" Text=""></asp:Label>
                        <a href="mailto:support@OMES.com">support@OMES.com</a> <br /><br />
                    </div>
                </div>
                <asp:LinkButton ID="btnCloseFilure" runat="server" Text="Close" CausesValidation="false" />
            </asp:Panel>
            <asp:Button runat="server" ID="ButtonDummyFIFailure" Style="display: none" />
<asp:Button runat="server" ID="btnplaceofServiceDummy" Style="display: none" Text="ButtonDummy8" />--%>

            <ajax:modalpopupextender id="modalPATXNFailure" runat="server" popupcontrolid="pnmPATXNFailure" targetcontrolid="ButtonDummyPATXNFailure"
                backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnmPATXNFailure" cancelcontrolid="">
            </ajax:modalpopupextender>
            <asp:Panel ID="pnmPATXNFailure" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 25%;">
                <div class="row">
                    <div class="col-sm-6 col-md-4 col-lg-3" style="float: right; padding-left: 10px">
                        <%--<asp:Image ID="ImageTXCancel" runat="server" Width="30%" Height="30%" ImageUrl="~/Images/cancel.png"  ImageAlign="Left"/>--%>
                        <asp:ImageButton ID="btnCloseFailure" Width="30%" Height="30%" ImageUrl="~/Images/cancel.png" CausesValidation="False" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6 col-md-8 col-lg-9" style="width: 100%">
                        <asp:Label ID="lblTXNResponseID" runat="server" Font-Bold="true" Text="" Font-Size="Medium"></asp:Label><br />
                        <br />
                        <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="Error"></asp:Label><br />
                        <br />
                        <asp:GridView ID="grdTXNResponse" runat="server" GridLines="None" Width="100%" BorderStyle="None" RowStyle-BackColor="White" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="ErrorCode" HeaderText="ErrorCode">
                                    <ItemStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ErrorDescription" HeaderText="ErrorDescp">
                                    <ItemStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
                                    <HeaderStyle HorizontalAlign="Left" Font-Names="Arial" Font-Size="10pt" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:Label ID="lblTXNErrors" runat="server" Font-Bold="true" Text=""></asp:Label><br />
                        <asp:Label ID="lblSupportPATXN" runat="server" Font-Bold="true" Text=""></asp:Label>
                        <a href="mailto:support@OMES.com">support@OMES.com</a>
                    </div>
                </div>

            </asp:Panel>
            <asp:Button runat="server" ID="ButtonDummyPATXNFailure" Style="display: none" />




        </div>

        <%-- End ajax pop up for NPI TEXTBOX--%>
    </div>
</div>
<script>
    window.onload = function () {
        document.onkeydown = function (e) {
            return (e.which || e.keyCode) != 116;
        };
    } 
</script>
<cc2:messagebox id="MessageBox2" runat="server" />
