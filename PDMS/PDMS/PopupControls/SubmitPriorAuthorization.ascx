<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_SubmitPriorAuthorization" Codebehind="SubmitPriorAuthorization.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/UploadDocument.ascx" TagName="UploadDocument" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="jk" Namespace="JK.BootstrapControls" Assembly="PDMS" %>
<%@ Register Src="~/PopupControls/PriorAuthProvidersNotes.ascx" TagPrefix="uc" TagName="PriorAuthProvidersNotes" %>
<%@ Register Src="~/PopupControls/PriorAuthServiceDetails.ascx" TagPrefix="uc" TagName="PriorAuthServiceDetails" %>
<%@ Register Src="~/PopupControls/PriorAuthDentalServiceDetails.ascx" TagPrefix="uc" TagName="PriorAuthDentalServiceDetails" %>
<%@ Register Src="~/PopupControls/PriorAuthDiagnosis.ascx" TagPrefix="uc" TagName="PriorAuthDiagnosis" %>
<%@ Register Src="~/PopupControls/PriorAuthDocumentbyMail.ascx" TagPrefix="uc" TagName="PriorAuthDocumentbyMail" %>
<%@ Register Src="~/PopupControls/PriorAuthAttachment.ascx" TagPrefix="uc" TagName="PriorAuthAttachment" %>
<%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>--%>
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery.loadtemplate/1.5.10/jquery.loadTemplate.min.js"></script>
<%--<script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.11.2/jquery-ui.min.js"></script>
<script src="<%# Page.ResolveClientUrl("~/Scripts/jquery.blockUI.js") %>" type="text/javascript"></script>--%>
<script type="text/javascript" src="../Scripts/paging.js"></script>

<%--<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" txtMedicaidBillingNumber --%>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/ServicingProviderInformationSearch.ascx" TagPrefix="uc" TagName="ServicingProviderInformationSearch" %>

<%--<uc:ServicingProviderInformation runat="server" ID="ServicingProviderInformation" />--%>

<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/PriorAuthDiagnosisSeach.ascx" TagPrefix="uc" TagName="PriorAuthDiagnosisSeach" %>
<%@ Register Src="~/PopupControls/PriorAuthDentalHSCPSCodeSearch.ascx" TagPrefix="uc" TagName="PriorAuthDentalHSCPSCodeSearch" %>

<%@ Register Src="~/PopupControls/SubmitClaimSearchProc.ascx" TagPrefix="uc" TagName="SubmitClaimSearchProc" %>
<%@ Register Src="~/PopupControls/ReviewerNotes.ascx" TagName="ReviewerNotes" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Src="~/PopupControls/IntuitivePriorAuthMessageBox.ascx" TagName="IntuitivePriorAuthMessageBox" TagPrefix="msgCC2" %>
<%@ Register Src="~/PopupControls/MaliciousAttachments.ascx" TagName="MaliciousAttachments" TagPrefix="uc" %>

<%--<script src="../Scripts/bootstrap.min.js"></script>--%>

<link href="../Content/custom-style.css" rel="stylesheet" />

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<link href="<%# Page.ResolveClientUrl("~/App_Themes/Modernization/Modern.css") %>" rel="stylesheet" />


<script src="../Scripts/SubmitPA/Attachments/SubmitPAAttachments.js"></script>
<script src="../Scripts/SubmitPA/PADiagnosis.js"></script>



<script>
    var attachmentFileName = "<%= AttachmentFileName %>";
    var attachmentFileNameSuffix = "<%= AttachmentFileNameSuffix %>";    
</script>

<script type="text/javascript">
    $(document).ready(function () {
        function DisableBackButton() {
            window.history.forward()
        };

        DisableBackButton();
        window.onload = DisableBackButton;
        window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
        window.onunload = function () { void (0) }

        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_valSummary").style.display = "";

        CheckDDLAssignment();

        GetNotes();

        //btnSearch6
        if (document.getElementById('<%=txtstatus2.ClientID%>') != null && document.getElementById('<%=txtstatus2.ClientID%>').value == "Pend") {
            document.getElementById('btnSearch6').style.display = "none";
            //
            if (document.getElementById('btnSearch1') != null) {
                document.getElementById('btnSearch1').style.display = "none";
            }
            if (document.getElementById('<%=lnkplaceofservicesrch.ClientID %>') != null) {
                document.getElementById('<%=lnkplaceofservicesrch.ClientID %>').style.visibility = "hidden";
            }
            if (document.getElementById('<%=lnkFacilityType.ClientID %>') != null) {
                document.getElementById('<%=lnkFacilityType.ClientID %>').style.visibility = "hidden";
            }
        }
        //$('#ctl00_MainContent_uc1SubmitPriorAuthorization_divSubmitPriorAuth').style.visibility = "hidden";
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtMedicaidID').val("");
        $('body').on('hidden.bs.modal', '.modal', function () {
            //$(this).find('form').trigger('reset');
            //clear();
            //$(this).removeData('bs.modal');
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFacilityTypeCode').val("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFacilityTypeDescription').val("");
            if (document.getElementById('<%=lblFacilitySResult.ClientID%>') != null) {
                document.getElementById('<%=lblFacilitySResult.ClientID%>').style.visibility = "hidden";
            }
          <%--  document.getElementById('<%=gvPriorAuthFacilitySearchPage.ClientID%>').style.visibility = "hidden";--%>
        })
        $('input[type="text"]').bind('change', function (event) {
            var cleartxt = $(this).val().replace(/[^\p{L}\p{N}\p{P}\p{Z}^$\n]/gu, '');
            $(this).val(cleartxt.trim());
            if (cleartxt.trim() != "") {
                //$(this).style.backgroundColor = "white";
                $(this).css("background-color", "white");
            }
        });


        var ClaimType = $(".rblClaimType input:checked").val();
        if (ClaimType == 'dental') {
            GetDiagnoisServiceDetails();
            GetServiceDetailsDental();
            try {
                OnPlaceOfServiceTextChanged(true);
            }
            catch (err) {
                console.log(err);
            }
        }
        else if (ClaimType == 'Professional') {
            try {
                OnPlaceOfServiceTextChanged(true);
            }
            catch (err) {
                console.log(err);
            }
            GetServiceDetailsProfessional();
            GetDiagnoisServiceDetails();
        }
        else {
            GetServiceDetailsInstitutional();
            GetDiagnoisServiceDetails();
            try {
                OnFacilityTypeTextChanged(true);
                resetsetadminsrcvalue();
            }
            catch (err) {
                console.log(err);
            }
        }

        try {
            getRecipientDetails();
            OnOrderingProviderTextChanged(true);
            OnServiceProviderTextChanged(true);
        }
        catch (err) {
            console.log(err);
        }

    });

    function CallBlockUIPostBack() {
        //alert('test');
        BlockUIPostBack('ctl00_MainContent_pnlBillingAndotherservice');
    };

</script>
<style>


</style>
<script type="text/javascript">

    resetsetadminsrcvalue();

    var btnClickfromserverSide = "";
    function clearPage() {
        var formValue = document.forms[0].elements;
        for (i = 0; i < formValue.length; i++) {
            if (formValue[i].type == 'text') {
                formValue[i].value = "";

            }
            else if (formValue[i].type == 'select-one') {
                formValue[i].value = "";
            }
            else if (formValue[i].type == "radio") {
                var rdbtn = formValue[i];
                rdbtn.disabled = false;

                rdbtn.removeAttribute('checked');
                rdbtn.checked = false;
            }
        }

    }
    $(document).on("click", ".addAttachment", function () {

        var claimType = $(".rblClaimType input:checked").val();
        var isValid = validateFileUpload(claimType === "dental");

        if (isValid) {
            uploadAttachment();
        }

        return isValid;
    });

    function addAttachmentOnButtonClick() {

        console.log('addAttachmentOnButtonClick called');

        var claimType = $(".rblClaimType input:checked").val();
        var isValid = validateFileUpload(claimType === "dental");

        if (isValid) {
            uploadAttachment();
        }

        return isValid;
    }

    function uploadAttachment() {


        let timestamp = new Date().toLocaleTimeString('it-IT').replace(/:/g, '');
        $('#hdnTime').val(timestamp);
        $("#tradingPartnerID").val(document.getElementById('<%=txtDestinationpayerID.ClientID%>').value);

        let claimType = $(".rblClaimType input:checked").val();
        let fileInputId = claimType === 'dental' ? '<%=priorDentalAttachmentUpload.ClientID%>' : '<%=PriorAttachmentUpload.ClientID%>';
        let file = document.getElementById(fileInputId).files[0];

        if (!file) {
            alert('No file selected.');
            return false;
        }

        let fileName = `<%=AttachmentFileName%>${$("#hdnTime").val()}--<%=AttachmentFileNameSuffix%>--${$("#tradingPartnerID").val()}.${file.name.split('.').pop()}`;
        let payLoad = JSON.stringify({ fileName: fileName, contentType: file.type });

        // Show upload progress
        claimType === 'dental' ? $(".divDentalUploading").show() : $(".divUploading").show();

        if (file.size > 10485760) {
            alert('File size exceeds max 10MB limit.');
            claimType === 'dental' ? $(".divDentalUploading").hide() : $(".divUploading").hide();
            return false;
        }

        // Generate presigned URL
        let script = document.createElement("script");
        script.type = "text/javascript";
        script.src = `<%="../Process/SubmitPriorAuthorization.aspx" %>?payloadData=${payLoad}&callback=addAttachment&MedicaidNumber=<%=MedicaidNumber%>&SubmitPAAddAttachment=SubmitPAAttachment`;
        document.getElementsByTagName("head")[0].appendChild(script);

        //  getPresignUrl(fileName, file.type).then(preSignedUrl => {


        //console.log(preSignedUrl);
        //if (preSignedUrl) {
        //    addAttachment(preSignedUrl);
        //}

        //});


        return true;
    }

    function getPresignUrl(fileName, contentType) {

        var APIToken = $("[id*=hdnAccessToken]").val();

        return $.ajax({
            type: "GET",
            url: webApiAttachments + "GetPresignUrl",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            data: { fileName: fileName, contentType: contentType },
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        })
            .then(response => {

                return response;
            })
            .fail(error => {
                console.error("Error fetching Presigned URL:", error);
                return null;
            });
    }

    //Sample
    function uploadFileWithModel() {
        var APIToken = $("[id*=hdnAccessToken]").val();

        var formData = new FormData();
        var fileInput = document.getElementById("fileUpload");
        var uploadedFile = fileInput.files[0];

        if (!uploadedFile) {
            alert("Please select a file.");
            return;
        }

        // Append file
        formData.append("file", uploadedFile);

        // Append custom model properties
        formData.append("UploadedBy", "Vishnu");
        formData.append("Category", "Documents");
        formData.append("Description", "Important Report");

        $.ajax({
            type: "POST",
            url: webApiAttachments + "upload-file",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                console.log("Upload successful:", response);
                alert("File uploaded successfully!");
                UploadPriorAuthAttachment();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("Upload failed:", textStatus, errorThrown);
                alert("Something went wrong!");
            }
        });
    }

    function CheckPhoneLength() {

    }

    function addAttachment(response) {
        var claimType = $(".rblClaimType input:checked").val();
        var fileInputId = claimType === 'dental' ? '<%=priorDentalAttachmentUpload.ClientID%>' : '<%=PriorAttachmentUpload.ClientID%>';
        var file = document.getElementById(fileInputId).files[0];

        if (!file) {
            alert("No file selected. Please choose a file.");
            return;
        }

        var contentType = file.type;

        $.ajax({
            type: 'PUT',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With"
            },
            url: response.preSignedUrl,
            contentType: contentType,
            processData: false,
            async: false,
            data: file,
            crossDomain: true,
            success: function (data, status, xhr) {
                __doPostBack("btnPAPriorAdd", "addAttachment");
                alert("Upload File to S3 Bucket");
            },
            error: function () {
                $(".divUploading").hide();
                alert("File not uploaded, please retry.");
            }
        });
    }

    function validateFileUpload(isDental) {
        var spanId = isDental ? "#spanDentalAttachment" : "#spanAttachment";
        var fileInputId = isDental ? "<%=priorDentalAttachmentUpload.ClientID%>" : "<%=PriorAttachmentUpload.ClientID%>";

        $(spanId).hide();
        var file = document.getElementById(fileInputId).files[0];

        if (!file) {
            $(spanId).show().text("No file selected.");
            return false;
        }

        if (file.size > 10485760) {
            $(spanId).show().text("File size exceeds max 10MB limit.");
            return false;
        }

        var fileExtension = file.name.substring(file.name.lastIndexOf(".")).toLowerCase();
        var allowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".pdf", ".zip", ".csv",
            ".xls", ".xlsx", ".ppt", ".pptx", ".doc", ".docx", ".xlsm", ".mdi",
            ".jpe", ".tif", ".pi", ".ec", ".msg", ".acrbak"];

        if (!allowedExtensions.includes(fileExtension)) {
            $(spanId).show().text("Not a valid file type.");
            return false;
        }

        return true;
    }

    function isNumber(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }
    // Hide validation summary on Cancel click
    function HideValidationSummary(sender) {
        var confirmation = confirm('Are you sure you want to cancel?');
        if (confirmation == "Yes") {
            //Hide all validation errors
            if (window.Page_Validators) {
                for (var vI = 0; vI < Page_Validators.length; vI++) {
                    var vValidator = Page_Validators[vI];
                    vValidator.isvalid = true;
                    ValidatorUpdateDisplay(vValidator);
                }
            }
            //Hide all validaiton summaries
            if (typeof (Page_ValidationSummaries) != "undefined") { //hide the validation summaries
                for (sums = 0; sums < Page_ValidationSummaries.length; sums++) {
                    summary = Page_ValidationSummaries[sums];
                    summary.style.display = "none";
                }
            }
            clearPage();
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtMedicaidID').val("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_lblSvcProviderLName').text("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_lblSvcProviderFName').text("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_lblOrdProviderFName').text("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_lblOrdProviderLName').text("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_divSubmitPriorAuth').hide();
            return true;
        } else {
            return false;
        }
    }

    function SaveButtonVisibility(txtStr) {
        var btnprovNoteEdit = document.getElementById('btnprovNoteEdit').style.display;

        if (txtStr.length > 0 && btnprovNoteEdit == 'none') {
            document.getElementById('btnprovNoteSave').style.display = "block";
        }
        else {
            document.getElementById('btnprovNoteSave').style.display = "none";
        }
        return false;
    }

    function countChar(val) {
        var len = val.value.length;
        var fin;
        if (len >= 262) {
            val.value = val.value.substring(0, 262);
            $('.numbersofChart').text(len + ' / 262');
        } else {
            $('.numbersofChart').text(len + ' / 262');
        }
    };

    function focusError(controlID) {
        event.preventDefault();
        var cpeclientID = Page_Validators[0].controltovalidate.split("_")[0] + "_" + Page_Validators[0].controltovalidate.split("_")[1]
            + "_" + Page_Validators[0].controltovalidate.split("_")[2] + "_" + controlID.toString().split("_")[0] + "_ClientState";
        var pnlClientID = Page_Validators[0].controltovalidate.split("_")[0] + "_" + Page_Validators[0].controltovalidate.split("_")[1]
            + "_" + Page_Validators[0].controltovalidate.split("_")[2] + "_" + controlID.toString().split("_")[1];
        var ctrclientID = Page_Validators[0].controltovalidate.split("_")[0] + "_" + Page_Validators[0].controltovalidate.split("_")[1]
            + "_" + Page_Validators[0].controltovalidate.split("_")[2] + "_" + controlID.toString().split("_")[2];
        if (controlID.toString().split("_")[0] != "noCpe") {
            if (document.getElementById(cpeclientID).value == "true") {
                document.getElementById(pnlClientID).click();
            }
            document.getElementById(pnlClientID).scrollIntoView({ behavior: "smooth" });
        }
        document.getElementById(ctrclientID).focus();
        if (document.getElementById(ctrclientID).value == "" || document.getElementById(ctrclientID).value == "(___) ___-____") {
            document.getElementById(ctrclientID).style.backgroundColor = "yellow";
        }
    }

    function focusPanel(controlID) {
        var cpeclientID = Page_Validators[0].controltovalidate.split("_")[0] + "_" + Page_Validators[0].controltovalidate.split("_")[1]
            + "_" + Page_Validators[0].controltovalidate.split("_")[2] + "_" + controlID.toString().split("_")[0] + "_ClientState";
        var pnlClientID = Page_Validators[0].controltovalidate.split("_")[0] + "_" + Page_Validators[0].controltovalidate.split("_")[1]
            + "_" + Page_Validators[0].controltovalidate.split("_")[2] + "_" + controlID.toString().split("_")[1];
        var ctrclientID = Page_Validators[0].controltovalidate.split("_")[0] + "_" + Page_Validators[0].controltovalidate.split("_")[1]
            + "_" + Page_Validators[0].controltovalidate.split("_")[2] + "_" + controlID.toString().split("_")[2];
        if (controlID.toString().split("_")[0] != "noCpe") {
            if (document.getElementById(cpeclientID).value == "true") {
                document.getElementById(pnlClientID).click();
            }
            document.getElementById(pnlClientID).scrollIntoView({ behavior: "smooth", block: "center" });
            $('html, body').animate({ scrollTop: $('#' + ctrclientID).offset().top }, 2000);
        }
    }

    function ShowLoading() {
        if (document.getElementById('<%=txtMedicaidBillingNumber.ClientID%>').value.length > 0
            && document.getElementById('<%=txtBirthDate.ClientID%>').value.length > 0) {
            document.getElementById('<%= btnRecipientloading1.ClientID %>').style.display = 'block';
        }
    }

    function DentalServiceCodeLoading() {
        document.getElementById('<%= btnDSloading1.ClientID %>').style.display = 'block';
    }

    function ProfessionalServiceCodeLoading() {
        document.getElementById('<%= btnPSloading1.ClientID %>').style.display = 'block';
    }

    function InstServiceCodeLoading() {
        document.getElementById('<%= btnISloading1.ClientID %>').style.display = 'block';
    }

    function validateCSSwithAdminLoader(obj) {

        loadAdminsource(obj.value);
    }

    function validateCSSwithLoader(obj) {
        document.getElementById('<%= btnPAloading1.ClientID %>').style.display = 'block';
        validateCSS(obj);
    }

    function validateCSS(obj) {
        try {
            "<%Session["SelectedPage"] = "SubmitClaimPA"; %>";
            document.getElementById(obj.id).style.backgroundColor = "white";
            var cleartxt = $('#' + obj.id).val().replace(/[^\p{L}\p{N}\p{P}\p{Z}^$\n]/gu, '');
            if (cleartxt != null) {
                $('#' + obj.id).val(cleartxt.trim());
                if (cleartxt.trim() != "") {
                    document.getElementById(obj.id).style.backgroundColor = "white";
                }
            }
        }
        catch {
            console.log("validateCSS error");
        }
    }

    function assignmentchanged(obj) {

        validateCSS(obj);
        validateprocedurefields();
        let span = document.getElementById('spanPlaceOfServiceIndicator');
        let procTypeCodeSpan = document.getElementById('procTypeCodeSpan');
        let spanServiceDetailsHeaderStar = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_spanServiceDetailsHeaderStar');

        let ddlServiceTypeCode = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddlServiceTypeCode');
        let rfvddlServiceTypeCode = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddlServiceTypeCode');
        let rftxtServicecode = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtServicecode');

        if ($('#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlAssignment').val() == '61') {
            span.textContent = "";
        }
        else {
            span.textContent = "*";
        }

        if ($('#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlAssignment').val() == '37') {
            if (rfvddlServiceTypeCode) {
                ValidatorEnable(document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddlServiceTypeCode'), false);
            }
        }

        if ($('#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlAssignment').val() == '37') {

            procTypeCodeSpan.textContent = "";
            spanServiceDetailsHeaderStar.textContent = "";
            if (rfvddlServiceTypeCode) {
                ValidatorEnable(document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddlServiceTypeCode'), false);
            }

            var lblServDetailsProcCode = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode');
            if (lblServDetailsProcCode !== null && lblServDetailsProcCode !== undefined && lblServDetailsProcCode !== '') {
                lblServDetailsProcCode.style.visibility = "hidden";
            }

            changeProcedureCodeType();
        }
        else {
            if (procTypeCodeSpan != null) {
                procTypeCodeSpan.textContent = "*";
            }
            if (spanServiceDetailsHeaderStar != null) {
                spanServiceDetailsHeaderStar.textContent = "*";
            }

            if (rfvddlServiceTypeCode) {
                ValidatorEnable(document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddlServiceTypeCode'), true);
                // rfvddlServiceTypeCode.setAttribute('ValidationGroup', 'valServiceDetails_Institutional');
            }
            var lblServDetailsProcCode = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode');
            if (lblServDetailsProcCode !== null && lblServDetailsProcCode !== undefined && lblServDetailsProcCode !== '') {
                lblServDetailsProcCode.style.visibility = "visible";
            }
        }
    }

    function changeProcedureCodeType() {

        var dropdownn = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddlServiceTypeCode');

        for (var i = 0; i < dropdownn.options.length; i++) {
            if (dropdownn.options[i].text === "--- Please select ---") {
                dropdownn.selectedIndex = i;
                break;
            }
        }
    }

    function validateprocedurefields() {
        let _claimType = $(".rblClaimType input:checked").val();
        let assignmentvalue = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlAssignment').val();
        if (assignmentvalue == '37' && _claimType == 'Institutional') {
            ValidatorEnable(document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtServicecode'), true);
            ValidatorEnable(document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddlServiceTypeCode'), false);
            ValidatorEnable(document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtLnProcedureCode'), false);
        }
        else {
            if (document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtServicecode') != null) {
                ValidatorEnable(document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtServicecode'), false);
            }
            if (document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddlServiceTypeCode') != null) {
                ValidatorEnable(document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddlServiceTypeCode'), true);
            }
            if (document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtLnProcedureCode') != null) {
                ValidatorEnable(document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtLnProcedureCode'), true);
            }
        }
    }

    function CheckDDLAssignment() {
        let span = document.getElementById("spanPlaceOfServiceIndicator");

        if (span !== null && span !== undefined && span !== '') {
            if ($('#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlAssignment').val() == '61') {
                span.textContent = "";
            }
            else {
                span.textContent = "*";
            }
        }

    }

    function setadminsrcvalue(selectedvalue) {

        let _adminSrc = $(selectedvalue).val();
        console.log(_adminSrc);
        $('#<%=hdnAdminSrc.ClientID%>').val(_adminSrc);

    }

    function resetsetadminsrcvalue() {
        let adminTye = $('#<%=ddlAdminType.ClientID%>').val();
        loadAdminsource(adminTye);
        let _selectedvalue = $('#<%=hdnAdminSrc.ClientID%>').val();
        $('#<%=ddlAdminSrc.ClientID%>').val(_selectedvalue);
    }

    function loadAdminsource(selectedvalue) {
        let list1 = [{ SourceValue: '-1', Text: '' }, { SourceValue: '5', Text: '5 - Born Inside Hospital' },
        { SourceValue: '6', Text: '6 - Born Outside Hospital' }];
        let list2 = [{ SourceValue: '-1', Text: '' }, { SourceValue: '1', Text: '1 - Physician Referral' },
        { SourceValue: '2', Text: '2 - Clinic Referral' }, { SourceValue: '3', Text: '3 - HMO Referral' },
        { SourceValue: '4', Text: '4 - Transfer from Hospital' }, { SourceValue: '5', Text: '5 - Transfer from SNF' },
        { SourceValue: '6', Text: '6 - Transfer from Another Health Care Facility' }, { SourceValue: '7', Text: '7 - Emergency Room' },
        { SourceValue: '8', Text: '8 - Court/Law Enforcement' }, { SourceValue: '9', Text: '9 - Information Not Available' },
        { SourceValue: 'D', Text: 'D - Transfer from one Distinct Unit of the Hospital to Another Distinct Unit of the Same Hospital Resulting in a Separate Claim to the Payer' },
        { SourceValue: 'E', Text: 'E - Transfer from Ambulatory Surgery Center' },
        { SourceValue: 'F', Text: 'F - Transfer from a Hospice Facility' },
        { SourceValue: 'G', Text: 'G - Transfer from a Designated Disaster Alternate Care Site' }];

        $('#<%=ddlAdminSrc.ClientID%>').empty();
       <%-- $('#<%=hdnAdminSrc.ClientID%>').val('');--%>
        if (selectedvalue == '-1') {
            $('#<%=ddlAdminSrc.ClientID%>').empty();
        }
        else if (selectedvalue == '4') {
            $.each(list1, function () {
                $('#<%=ddlAdminSrc.ClientID%>').append($("<option></option>").val(this['SourceValue']).html(this['Text']));
            })
        }
        else {
            $.each(list2, function () {
                $('#<%=ddlAdminSrc.ClientID%>').append($("<option></option>").val(this['SourceValue']).html(this['Text']));
            })
        }
       <%-- $('#<%=ddlAdminType%>').removeAttr('disabled');--%>
    }

    function CollapseExpandAttachment() {
        let sp = $('#<%= pnlSepAttachment.ClientID%>' + ' span')[0];
        if (sp != undefined) {
            var spText = sp.innerHTML;
            var plusIndex = spText.indexOf("+");
            if (plusIndex == 0)
                sp.innerHTML = spText.replace("+", "-");
            else
                sp.innerHTML = spText.replace("-", "+");
        }
    }

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
            sender._selectedDate = null;
            sender._textbox.set_Value(null)
        }
    }
    function HideDiagnosisAdd() {
        $("#pnlDiagnosisLine").hide();
    }
    function onlyNumbers(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
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
    function invokeModalPopUp(btnid) {
        btnClickfromserverSide = "Invoked";
        document.getElementById(btnid).click();
    }
    function getbuttondetail(e) {
        $('#hdnSearchId').val(e.id);
        var hdn_SearchId = $('#hdnSearchId').val();

        //if (hdn_SearchId == 'btnSearch1') {
        if (btnClickfromserverSide != "Invoked") {
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtNPI').val("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtProMedicaidID').val("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtBusinessLastName').val("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFirstName').val("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_lblSResult').hide();
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_gvPriorAuthSearchPage').hide();
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFacilityTypeCode').val("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_revNPI').hide();
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtPlaceOfServiceCode').val("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_lblErrorSearch').hide();
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtPlaceOfServiceDesc').val("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtNPIORD').val("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtProMedicaidIDORD').val("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtBusinessLastNameORD').val("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFirstNameORD').val("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_lblSResultORD').hide();
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_gvPriorAuthSearchPageORD').hide();
        }
        else {
            btnClickfromserverSide = "";
        }
    }
    function closeFacModal() {
        var Npi = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnfacilityType').val();
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtfacilityType').val(Npi);
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFacilityTypeCode').val("");
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFacilityTypeDescription').val("");
        //document.getElementById('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFacilityTypeCode').reset();
        //document.getElementById('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFacilityTypeDescription').reset();
        //document.getElementById('#gvPriorAuthFacilitySearchPage').reset();
        $('#FacilityTypeSearchModal').modal('hide');
        $('.modal-backdrop').remove();
    }
    var lastScrollTop = 0, delta = 5;
    $(window).scroll(function () {
        var nowScrollTop = $(this).scrollTop();
        if (Math.abs(lastScrollTop - nowScrollTop) >= delta) {
            if (nowScrollTop > lastScrollTop) {
                //scrolldown
                $('#divErrorList').css('top', '0px');
                $('#divErrorList').addClass('fixed-content');
            }
            else {
                //scrollup
                if (nowScrollTop <= 532) {
                    $('#divErrorList').css('top', '0px');
                    $('#divErrorList').removeClass('fixed-content');
                }
            }
            lastScrollTop = nowScrollTop;
        }
    });
    function closemodal() {
        //var hdn_NPI = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnNPI"').val();
        //var hdn_MedicaidId = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnMedicaidId').val();
        //var hdn_ProviderName = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnProviderName').val();        
        var hdn_SearchId = $('#hdnSearchId').val();
        if (hdn_SearchId == 'btnSearch1') {
            //document.getElementById('hdnProvSearchModalSearchId').value = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtNPIORD').val();
            var Npi = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtNPIORD').val();
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtorderingprovidernpi').val(Npi);
            var Medicaid = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtProMedicaidIDORD').val();
            //$('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtOMID').val(Medicaid);
            var lblMedicaid = document.getElementById('<%=txtOMID.ClientID%>');
            lblMedicaid.innerHTML = Medicaid;
            var FirstName = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFirstNameORD').val();
            var lblFName = document.getElementById('<%=lblOrdProviderFName.ClientID%>');
            lblFName.innerHTML = FirstName;
            var LastName = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtBusinessLastNameORD').val();
            var lblLName = document.getElementById('<%=lblOrdProviderLName.ClientID%>');
            lblLName.innerHTML = LastName;
            $('#ProviderSearchModalORD').modal('hide');
            //$('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
            //alert($('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtorderingprovidernpi').value);
            //document.getElementById('txtorderingprovidernpi').value = "123456";
            //alert(binded);
            // alert(document.getElementById("txtorderingprovidernpi").value);
            // ;            //document.getElementById('txtSPNPI').value = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtNPI').val();
            //$('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtorderingprovidernpi') = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtNPI').val();
            // $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtorderingprovidernpi').val(hdn_NPI);
            //$('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtOMID').val(hdn_MedicaidId);
            //$('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtOPNAME').val(hdn_ProviderName);
        }
        //else if (hdn_SearchId == 'lnkplaceofservicesrch') {
        //    var Npi = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFacilityTypeCode').val();
        //    $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtpalceofservice').val(Npi);
        //    $('#FacilityTypeSearchModal').modal('hide');
        //    $('.modal-backdrop').remove();
        //}
        else if (hdn_SearchId == 'facilitySearch') {
            var Npi = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFacilityTypeCode').val();
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtfacilityType').val(Npi);
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFacilityTypeCode').val("");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFacilityTypeDescription').val("");
            $('#FacilityTypeSearchModal').modal('hide');
            $('.modal-backdrop').remove();
        }
        //else if (hdn_SearchId == 'btnSearch3') {
        //    $('#ctl00_MainContent_uc5SubmitClaim_txtNPI1add').val(hdn_NPI);
        //      $('#ctl00$MainContent$uc5SubmitClaim$txtOPMID1add').val(hdn_MedicaidId);
        //    $('#ctl00_MainContent_uc5SubmitClaim_txtOPName1add').val(hdn_ProviderName);
        //}
        //else if (hdn_SearchId == 'btnSearch4') {
        //    $('#ctl00_MainContent_uc5SubmitClaim_txtNPI2add').val(hdn_NPI);
        //      $('#ctl00_MainContent_uc5SubmitClaim_txtOPMID2add').val(hdn_MedicaidId);
        //    $('#ctl00_MainContent_uc5SubmitClaim_txtOPName2add').val(hdn_ProviderName);
        //}
        //else if (hdn_SearchId == 'btnSearch5') {
        //    $('#ctl00_MainContent_uc5SubmitClaim_txtNPI3add').val(hdn_NPI);
        //      $('#ctl00_MainContent_uc5SubmitClaim_txtOPMID3add').val(hdn_MedicaidId);
        //    $('#ctl00_MainContent_uc5SubmitClaim_txtOPName3add').val(hdn_ProviderName);
        //}
        else if (hdn_SearchId == 'btnSearch6') {
            var Npi = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtNPI').val();
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtSPNPI').css("background-color", "white");
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtSPNPI').val(Npi);
            var Medicaid = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtProMedicaidID').val();
            var lblMedicaid = document.getElementById('<%=txtMedicaidID.ClientID%>');
            //$('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtMedicaidID').val(Medicaid);
            lblMedicaid.innerHTML = Medicaid;
            var FirstName = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFirstName').val();
            var lblFName = document.getElementById('<%=lblSvcProviderFName.ClientID%>');
            lblFName.innerHTML = FirstName;
            var LastName = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtBusinessLastName').val();
            var lblLName = document.getElementById('<%=lblSvcProviderLName.ClientID%>');
            lblLName.innerHTML = LastName;
            $('#ProviderSearchModal').modal('hide');
            $('.modal-backdrop').remove();
        }
        //else if (hdn_SearchId == 'btnSearch7') {
        //    $('#ctl00_MainContent_uc5SubmitClaim_txtRenderingProvNPI').val(hdn_NPI);
        //      $('#ctl00_MainContent_uc5SubmitClaim_txtRenderingMedicaidID').val(hdn_MedicaidId);
        //    $('#ctl00_MainContent_uc5SubmitClaim_txtRenderingProviderName').val(hdn_ProviderName);
        //}
        else if (hdn_SearchId == 'btnDiagSearch2') {
            console.log("Reached the code")
            event.preventDefault();
            //ctl00_MainContent_uc1SubmitPriorAuthorization_txtDiagnosisCodeSearch
            var hdn_DiagnosisCode = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnDiagnosisCode').val();
            var hdn_DiagnosisVersion = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnDiagnosisVersion').val();
            var hdn_DiagnosisDes = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnDiagnosisDes').val();
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtDiagCode').val(hdn_DiagnosisCode);
            //$('#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlDiagnosisCodetype').val(hdn_DiagnosisVersion);
            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtDiagDescription').val(hdn_DiagnosisDes);
            $('#myDiagModal').modal('hide');
            $('.modal-backdrop').remove();
            //return;
        }
        $('#myModal').modal('hide');
    }

    $('#FacilityTypeSearchModal').on('hidden.bs.modal', function () {
        $(this).find('form').trigger('reset');
        clear();

    })
    function ValidateFacilityTypeSearch() {
        var fcode = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFacilityTypeCode').val();
        var fdesc = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtFacilityTypeDescription').val();
        if ((fcode == '' || fcode == undefined) && (fdesc == '' || fdesc == undefined)) {
            $('#FacilitySearchErrorMessage').show();
            return false;
        }
        else {
            $('#FacilitySearchErrorMessage').hide();
            return true;
        }
        return false;
    }
    function minmax(value, min, max) {
        if (parseInt(value) < min || isNaN(parseInt(value)))
            return min;
        else if (parseInt(value) > max)
            return max;
        else return value;
    }
    function ShowConfirmRule6(value) {
        var val = confirm(value);
        if (val && document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_hdnFrontEndEdits_6') != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_hdnFrontEndEdits_6').value = "1";
        }
    }
    function ShowConfirmRule7(value) {
        var val = confirm(value);
        if (val && document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_hdnFrontEndEdits_7') != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_hdnFrontEndEdits_7').value = "1";
        }
    }
    function ShowConfirmRule1(value) {
        var val = confirm(value);
        if (val && document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_hdnFrontEndEdits') != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_hdnFrontEndEdits').value = "1";
        }
    }
    function pageLoad(sender, args) {

    }
    $('*').on('blur change click dblclick error focus focusin focusout hover keydown keypress keyup load mousedown mouseenter mouseleave mousemove mouseout mouseover mouseup resize scroll select submit', function () {

        if (document.getElementById('<%=txtstatus2.ClientID%>') != null && document.getElementById('<%=txtstatus2.ClientID%>').value == "Pend") {
            document.getElementById('btnSearch6').style.visibility = "hidden";
            document.getElementById('btnSearch1').style.visibility = "hidden";
            document.getElementById('<%=lnkplaceofservicesrch.ClientID %>').style.visibility = "hidden";
            document.getElementById('<%=lnkFacilityType.ClientID %>').style.visibility = "hidden";
        }
    });
    //OHPNM-10225
    function RefreshRecipientInfo() {
        document.getElementById('<%= btnRecipientloading1.ClientID %>').style.display = 'none';
        document.getElementById('<%= lblErrorMsg.ClientID %>').style.display = 'none';
        var cpe = $find("ctl00_MainContent_uc1SubmitPriorAuthorization_cpeRecipient");
        if (cpe.get_Collapsed()) {
            cpe._doOpen();
        }
        else {
            cpe._doClose();
        }
        document.getElementById('<%=txtMedicaidBillingNumber.ClientID %>').disabled = false;
        document.getElementById('<%=txtBirthDate.ClientID %>').disabled = false;
        document.getElementById('<%=txtPatientTrckNum.ClientID %>').disabled = false;
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtMedicaidBillingNumber').val('');
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtfrstmi2').val('');
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtLastName2').val('');
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtMiddleName').val('');
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtBirthDate').val('');
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtPatientTrckNum').val('');
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtGender2').val('');
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtAddress1').val('');
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_lblAddress2').val('');
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtCity').val('');
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtState').val('');
        $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtZipCode').val('');
        "<%Session["RecipientEligibilityResponse"] = null; %>";
    }
    $(document).ready(function () {
        validateprocedurefields();
    });
</script>
<style type="text/css">
    table.gridview td, .rgRow td, .rgAltRow td, table.gridViewSmallFont td {
        font-size: 14px;
    }

    a.errMsg {
        color: #CC0505;
        text-decoration: none;
    }

    select {
        min-width: 90%;
        height: 25px;
    }

    div[style="height:221px"] {
        height: auto !important
    }

    .gridViewFooter {
        background-color: white;
    }

    .fixed-content {
        height: 10%;
        position: fixed;
        z-index: 1;
        width: 76%;
        padding: 16px;
        background-color: whitesmoke;
        overflow-y: scroll;
    }

    .gridViewSelected, .gridViewSelected td, .gridViewSelected tr {
        background-color: white;
    }

    .ajaxcalender {
        position: absolute;
        padding: 10px;
    }

    .ajax__calendar_container {
        z-index: 50000 !important;
        position: static;
    }

    .modal-content {
        -webkit-box-shadow: 0 5px 15px rgba(0,0,0,.5);
        box-shadow: 0 5px 15px rgba(0,0,0,.5);
        height: 589px;
        width: max-content;
        margin-left: -300px;
    }

    .modal-body {
        height: auto;
        width: 1349px;
        position: relative;
        padding: 15px;
    }

    .pH2 {
        padding-left: 10px;
        color: white;
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

    .modal-header {
        padding: 7px;
        border-bottom: 20px solid #25a0da;
        text-align: -webkit-center;
    }

    .formField {
        border: 1px solid #ccc;
        color: #000;
        width: 264px;
    }

    .formField150 {
        border: 1px solid #ccc;
        color: #000;
        min-width: 150px;
    }

    .formField200 {
        border: 1px solid #ccc;
        color: #000;
        width: 200px;
        min-width: 200px !important;
        height: 34px;
    }

    .formField_text {
        border: 1px solid #ccc;
        background-color: lightgrey !important;
        color: #000;
        width: 264px;
    }

    .formField_264 {
        border: 1px solid #ccc;
        background-color: lightgrey !important;
        color: #000;
        width: 264px !important;
    }

    .Assignment {
        margin-bottom: 0px;
    }

    select {
        min-width: 273px;
        height: 34px;
        border: 1px solid #ccc;
    }

    .formLabel200 {
        width: 178px;
        /*width: 162px;*/
    }

    .col-lg-6 {
        /* width: 50%; */
    }

    .col-sm-3 {
        width: 23%;
    }

    .col-sm-8 {
        width: 60.666667%;
    }

    #myModal .modalbody {
        font-size: 13px;
        font-weight: bold;
        text-align: center;
        background-color: lightblue;
        width: 100%;
    }

    #myModal .modalInput {
        padding: 0px 0px 0px 10px;
        height: 30px;
        margin-top: 10px;
    }

    .providerSearchResultGrid {
        margin: 0px;
    }

    #gvPriorAuthSearchPage .gridViewHeader {
        font-size: 12px;
        font-weight: bold;
    }

    #gvPriorAuthSearchPageORD .gridViewHeader {
        font-size: 12px;
        font-weight: bold;
    }
    /*AJAX CALENDAR*/
    .QstLTCCalendarCSS .ajax__calendar_container {
        background-color: #DEF1F4;
        border: solid 1px #77D5F7;
        width: 200px !important;
        z-index: 5000 !important;
        top: -18px;
        margin-top: -30px;
    }

    .cetxtEstDOBClass .ajax__calendar_container {
        position: relative !important;
    }

    @media only screen and (max-width: 990px) {
        .dateOfillnessClass .ajax__calendar_container {
            position: relative !important;
            /* top: -150px !important;*/
        }
    }

    @media only screen and (max-width: 990px) {
        .popupcontrolWidth {
            min-width: auto !important;
        }
    }

    .diagnosisDateClass {
        position: relative !important;
        left: 0px !important;
        top: 0px !important;
    }

    .dateOfDischargeClass .ajax__calendar_container {
        position: absolute !important;
        top: -200px !important;
    }

    .drpSplt {
        min-width: 100%;
        width: 50px;
        display: block;
        padding: 0px;
        margin: 0px;
    }

    .QstLTCCalendarCSS .ajax__calendar_header {
        background-color: #ffffff;
        margin-bottom: 4px;
    }

    .QstLTCCalendarCSS .ajax__calendar_title,
    .QstLTCCalendarCSS .ajax__calendar_next,
    .QstLTCCalendarCSS .ajax__calendar_prev {
        color: #004080;
        padding-top: 3px;
    }

    .popupGridViewOnSearch {
        margin: 20px;
        height: 300px;
        overflow-y: auto;
    }

    .radioButtonList {
        list-style: none;
        margin: 0;
        padding: 0;
    }

        .radioButtonList.horizontal li {
            display: inline;
        }

        .radioButtonList label {
            display: inline;
        }

    .PASubPendingInfoStyle {
        background-color: #E7edee;
    }

    .PAApprovedInfoStyle {
        background-color: #Dff7d1;
    }

    .PADeniedInfoStyle {
        background-color: #F1c6c6;
    }

    .PAInfoDenied {
        background-color: #E7edee;
    }

    .all-services span.ohio-field {
        font-weight: bold !important;
        font-size: 16px !important
    }

    .text-red {
        color: #f00 !important
    }

    table.gridview th {
        width: inherit !important;
        line-height: 1
    }

    .watermarked {
        background-color: #F7F6F3;
        border: solid 1px #808080;
        padding: 3px;
        color: Gray;
    }

    .ServiceproviderNPIOutputResponsive {
        width: auto;
        min-width: fit-content
    }

    @media only screen and (max-width: 990px) {
        .ServiceproviderNPIOutputResponsive {
            width: 100%;
            overflow-x: scroll;
            min-width: auto;
        }
    }

    .providerNPIOutputResponsive {
    }

    @media only screen and (max-width: 990px) {
        .providerNPIOutputResponsive {
            width: 100%;
            overflow-x: scroll;
            min-width: auto;
        }
    }

    .buttonBoxFocusgreen {
        color: #FFF;
        background-color: #c25600;
        /*border-color: #eea236 !important;*/
        /*background:none !important;*/
        font-weight: bold;
        padding: 5px 10px;
        line-height: 1.5 !important;
        border-radius: 3px !important;
        text-shadow: none !important;
        border: 1px solid transparent !important;
        width: auto;
        min-width: 100px;
        height: 40px;
    }

    .modal {
        background: rgba(0, 0, 0, 0.5);
    }

    .modal-backdrop {
        display: none;
    }

    .buttonfloatRight {
        float: right;
    }

    .buttonVisible {
        visibility: hidden;
    }

    .defaultGray {
        background-color: #D3D3D3 !important;
    }

    .paDiagnosisAddBtn {
        display: none;
    }

    .paDiagnosisUpdateBtn {
        display: none;
    }

    .attachment-table {
        width: 100%;
        text-align: center;
        border-collapse: collapse;
        margin-top: 0;
        border: 1px solid #ccc;
    }

        .attachment-table thead {
            background-color: #f4f4f4;
            font-weight: bold;
        }

        .attachment-table th,
        .attachment-table td {
            padding: 10px;
            border: 0px solid transparent !important;
        }

        .attachment-table th {
            color: #222222 !important;
        }

        .attachment-table tbody tr:nth-child(even) {
            background-color: #fafafa;
        }

        .attachment-table tbody tr:hover {
            background-color: #f1f1f1;
        }

        .attachment-table .actions-column {
            text-align: center;
        }
</style>
<script type="text/javascript">
    function GetPlaceOfServiceDetails() {
        var txtValueCode = $("#<%= txtPlaceOfServiceCode.ClientID %>").first().val();
        var txtvaluecodedesc = $("#<%= txtPlaceOfServiceDesc.ClientID %>").first().val();
        if (!txtValueCode.match(/\S/) && !txtvaluecodedesc.match(/\S/)) {
            document.getElementById('output').innerHTML = "<p style='color:red'>Either place of code or description is required.</p>";
            return true;
        } else {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetPlaceOfServiceDetails?val=" + txtValueCode + "&&desc=" + txtvaluecodedesc,
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
                        document.getElementById('output').innerHTML = 'No place of service found.';
                    }
                    else {
                        var table = "<table id='placeOfServiceData' class='gridViewSmallFont' cellspacing='0' rules='all' border='1' style='width:100%;border-collapse:collapse;'><tr class='gridViewHeader'><th scope='col'>Procedure Code</th><th scope='col'>Procedure Code Description</th></tr>";
                        for (var i = 0; i < result.length; i++) {
                            table = table + "<tr class='gridViewRow' valign='top'><td style='width:100px;'><a onClick='GetSelectedRow(" + result[i].Placeofservice_Code + "); return false;'>" + result[i].Placeofservice_Code + "</a></td><td>" + result[i].Placeofservice_Desc + "</td></tr>";
                        };
                        table = table + "</table>";
                        document.getElementById('output').innerHTML = table
                        if (result.length > 10) {
                            $('#placeOfServiceData').paging({ limit: 10 });
                        }

                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    document.getElementById('output').innerHTML = 'Something went wrong. Please contact system administrator....!';
                }
            });
            return false;
        }
    }
    function GetSelectedRow(code) {
        var desc = "";
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetPlaceOfServiceDetails?val=" + code + "&&desc=" + desc,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                for (var i = 0; i < result.length; i++) {
                    var strServ = result[i].Placeofservice_Code + "-" + result[i].Placeofservice_Desc;
                    $("[id*=txtpalceofservice]").val(strServ.trim());
                    $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeServiceinfoPlaceofService").hide();
                    return true;
                };
            },
            error: function (jqXHR, textStatus, errorThrown) {
                document.getElementById('output').innerHTML = 'Something went wrong. Please contact system administrator....!';
            }
        });
    }
    function visiblePlaceOfServiceDetails() {
        try {
            $("[id*=txtPlaceOfServiceCode]").val('');
            $("[id*=txtPlaceOfServiceDesc]").val('');
            document.getElementById('output').innerHTML = '';
            $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeServiceinfoPlaceofService").show();
        }
        catch (err) {
            alert(err);
        }
        return false;
    }

    function OnPopulatetxtDestinationpayerID() {
        var ddlDestinationPayerName = $("#<%= ddlSubCapitaPayerIDs.ClientID %>").val();
        document.getElementById('<%=txtDestinationpayerID.ClientID%>').value = ddlDestinationPayerName;
    }

    function OnDestinationPayerNameIndexChanged(code) {

        var ddlDestinationPayerName = $("#<%= ddlAuthorization.ClientID %>").val();

        var ddSubCapitaPayerIDs = document.getElementById("<%=ddlSubCapitaPayerIDs.ClientID %>");
        var option;
        var APIToken = $("[id*=hdnAccessToken]").val();

        if (ddlDestinationPayerName != null) {

            $.ajax({
                type: "GET",
                url: webApiPA + "LoadDestinationPayerIDs?destPayerIDINT=" + ddlDestinationPayerName,

                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    ddSubCapitaPayerIDs.options.length = 0;
                    for (var i = 0; i < result.length; i++) {
                        option = document.createElement('option');
                        option.value = result[i].MCE_ID;
                        option.innerHTML = result[i].PRIOR_AUTH_SUB_DESTINATION_PAYER_DESC_LRG;
                        ddSubCapitaPayerIDs.options.add(option);
                    }
                    var ddlDestinationPayerName2 = $("#<%= ddlSubCapitaPayerIDs.ClientID %>").val();
                    document.getElementById('<%=txtDestinationpayerID.ClientID%>').value = ddlDestinationPayerName2;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    document.getElementById('output').innerHTML = 'Something went wrong. Please contact system administrator....!';
                }
            });
        }
    }

    function OnPlaceOfServiceTextChanged(pageload) {
        var txtValueCode = $("#<%= txtpalceofservice.ClientID %>").first().val();
        txtValueCode = txtValueCode.replace(/(^\s+|\s+$)/g, '');
        var code = '';
        var desc = '';
        if (txtValueCode != '' && txtValueCode != null && txtValueCode != undefined) {
            if (txtValueCode.includes('-')) {
                const placeofservArrya = txtValueCode.split("-");
                code = placeofservArrya[0];
                desc = placeofservArrya[1];
            }
            else {
                $("[id*=txtPlaceOfServiceCode]").val(txtValueCode);
                code = txtValueCode;
            }
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetPlaceOfServiceDetails?val=" + code + "&&desc=" + desc,
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
                        document.getElementById('<%= lblPlaceofErrorSearch.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= lblPlaceofErrorSearch.ClientID%>').innerHTML = 'Place of Service is invalid';
                    }
                    else if (result.length > 1) {
                        $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeServiceinfoPlaceofService").show();
                    }
                    else {
                        var placeofcd = result[0].PlaceOfServiceCode;
                        var placeofdesc = result[0].PlaceOfServiceDesc;
                        var strServ = result[0].Placeofservice_Code + "-" + result[0].Placeofservice_Desc;
                        $("[id*=txtpalceofservice]").val(strServ.trim());
                        document.getElementById('<%= lblPlaceofErrorSearch.ClientID%>').style.visibility = "hidden";
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    document.getElementById('<%= lblPlaceofErrorSearch.ClientID%>').innerHTML = 'Something went wrong. Please contact system administrator....!';
                    document.getElementById('<%= lblPlaceofErrorSearch.ClientID%>').style.visibility = "visible";
                }
            });
        }
        else {
            document.getElementById('<%= lblPlaceofErrorSearch.ClientID%>').style.visibility = "hidden";
        }
    }
    /*PADS Statrted*/




    function getParameterByName(name, url = window.location.href) {

        var medID = $('#<%=hdnMedID.ClientID%>').val();
        return medID;
    }

    function getFormattedDate(dateVal) {
        var year = dateVal.getFullYear();
        var month = (1 + dateVal.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        var day = dateVal.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return month + '/' + day + '/' + year;
    }

    function GetOrderingProviderDetails() {

        var txtNPIORD = $("#<%= txtNPIORD.ClientID %>").first().val();
        var txtProMedicaidIDORD = $("#<%= txtProMedicaidIDORD.ClientID %>").first().val();
        var txtBusinessLastNameORD = $("#<%= txtBusinessLastNameORD.ClientID %>").first().val();
        var txtFirstNameORD = $("#<%= txtFirstNameORD.ClientID %>").first().val();
        document.getElementById('providerNPIOutput').innerHTML = "<img src='../Images/loader.gif'/>";
        if (txtNPIORD.length > 0 && txtNPIORD.length < 10) {
            document.getElementById('providerNPIOutput').innerHTML = "<p style='color:red'>NPI should be 10-digit number.</p>";
            return false;
        }
        else if (txtProMedicaidIDORD.length > 0 && txtProMedicaidIDORD.length < 7) {
            document.getElementById('providerNPIOutput').innerHTML = "<p style='color:red'>Medicaid ID should be 7-digit number.</p>";
            return false;
        }
        else if (txtNPIORD.length <= 0 && txtProMedicaidIDORD.length <= 0 && txtBusinessLastNameORD.length <= 0 && txtFirstNameORD.length <= 0) {
            document.getElementById('providerNPIOutput').innerHTML = "<p style='color:red'>NPI, Medicaid ID, Business/Last Name or First Name is required</p>";
            return false;
        }
        else {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetProviderNPI?npi=" + txtNPIORD + "&&medicaidID=" + txtProMedicaidIDORD + "&&lastName=" + txtBusinessLastNameORD + "&&firstName=" + txtFirstNameORD,
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
                        document.getElementById('providerNPIOutput').innerHTML = 'No Providers found.';
                    }
                    else {
                        var table = "<table class='gridViewSmallFont providerSearchResultGrid' cellspacing='0' rules='all' border='1' style='width:100%;min-width:860px;border-collapse:collapse;'><tbody><tr class='gridViewHeader'><th scope='col'>NPI</th><th scope='col'>Medicaid ID</th><th scope='col'>Business/Last Name</th><th scope='col'>First Name</a></th><th scope='col'>Address Line 1</th><th scope='col'>Address Line 2</th><th scope='col'>City</th><th scope='col'>State</th><th scope='col'>Zip</th></tr>";
                        for (var i = 0; i < result.length; i++) {
                            table = table + "<tr class='gridViewRow' valign='top'><td style='width:100px;'><a onClick='GetRegisteredNPISelectedRow(\"" + escapeSingleQuote(result[i].NPI, false) + "\",\"" + escapeSingleQuote(result[i].MEDICAID_ID, false) + "\",\"" + escapeSingleQuote(result[i].FIRST_NAME, false) + "\",\"" + escapeSingleQuote(result[i].LAST_OR_BUSINESS_NAME, false) + "\"); return true;'>" + result[i].NPI + "</a></td><td style='width:100px;'><a onClick='GetRegisteredNPISelectedRow(\"" + escapeSingleQuote(result[i].NPI, false) + "\",\"" + escapeSingleQuote(result[i].MEDICAID_ID, false) + "\",\"" + escapeSingleQuote(result[i].FIRST_NAME, false) + "\",\"" + escapeSingleQuote(result[i].LAST_OR_BUSINESS_NAME, false) + "\"); return true;'>" + result[i].MEDICAID_ID + "</a></td><td>" + result[i].LAST_OR_BUSINESS_NAME + "</td><td>" + result[i].FIRST_NAME + "</td><td>" + result[i].ADDRESS1 + "</td><td>" + result[i].ADDRESS2 + "</td><td>" + result[i].CITY + "</td><td>" + result[i].STATE + "</td><td>" + result[i].ZIP + "</td></tr>";
                        };
                        table = table + "</tbody></table>";
                        document.getElementById('providerNPIOutput').innerHTML = table;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    document.getElementById('providerNPIOutput').innerHTML = 'Something went wrong. Please contact system administrator....!';
                }
            });
            return false;
        }
    }
    function GetRegisteredNPISelectedRow(npi, medicaid, firstName, lastName) {

        document.getElementById('divorderproviderLoader').innerHTML = "<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
        document.getElementById('<%= lblOrdProviderFName.ClientID%>').innerHTML = escapeSingleQuote(firstName, true);
        document.getElementById('<%= lblOrdProviderLName.ClientID%>').innerHTML = escapeSingleQuote(lastName, true);
        document.getElementById('<%= txtOMID.ClientID%>').innerHTML = medicaid;
        document.getElementById('<%= hdlblOrdProviderFName.ClientID%>').value = escapeSingleQuote(firstName, true);
        document.getElementById('<%= hdlblOrdProviderLName.ClientID%>').value = escapeSingleQuote(lastName, true);
        document.getElementById('<%= hdtxtOMID.ClientID%>').value = medicaid;
        document.getElementById('<%= hdtxtOMID1.ClientID%>').value = medicaid;
        $("[id*=txtorderingprovidernpi]").val(npi);
        $("[id*=txtNPIORD]").val('');
        $("[id*=txtProMedicaidIDORD]").val('');
        $("[id*=txtBusinessLastNameORD]").val('');
        $("[id*=txtFirstNameORD]").val('');
        document.getElementById('divorderproviderLoader').innerHTML = "";
        document.getElementById('providerNPIOutput').innerHTML = '';
        $('#ProviderSearchModalORD').modal('hide');
    }
    function OnOrderingProviderTextChanged(pageLoad) {

        if (pageLoad == false) {
            document.getElementById('<%= lblOrdProviderFName.ClientID%>').innerHTML = "";
            document.getElementById('<%= lblOrdProviderLName.ClientID%>').innerHTML = "";
            document.getElementById('<%= txtOMID.ClientID%>').innerHTML = "";
            document.getElementById('<%= hdlblOrdProviderFName.ClientID%>').value = "";
            document.getElementById('<%= hdlblOrdProviderLName.ClientID%>').value = "";
            document.getElementById('<%= hdtxtOMID.ClientID%>').value = "";
            document.getElementById('<%= hdtxtOMID1.ClientID%>').value = "";
        }
        var txtNPIORD = $("#<%= txtorderingprovidernpi.ClientID %>").first().val();
        document.getElementById('divorderproviderLoader').innerHTML = ""; //"<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
        var txtProMedicaidIDORD = '';
        var txtBusinessLastNameORD = '';
        var txtFirstNameORD = '';
        if (txtNPIORD.length <= 0) {
            if (pageLoad == false) {
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblErrOrderProvidingNPI').innerHTML = "<p style='color:red'>Please Enter valid NPI.</p>";
            }
            document.getElementById('divorderproviderLoader').innerHTML = "";
            return false;
        }
        else if (txtNPIORD.length > 0 && txtNPIORD.length < 10) {
            if (pageLoad == false) {
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblErrOrderProvidingNPI').innerHTML = "<p style='color:red'>10-digit number is required.</p>";
            }
            document.getElementById('divorderproviderLoader').innerHTML = "";
            return false;
        }
        else {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetProviderNPI?npi=" + txtNPIORD + "&&medicaidID=" + txtProMedicaidIDORD + "&&lastName=" + txtBusinessLastNameORD + "&&firstName=" + txtFirstNameORD,
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (pageLoad == false) {
                        if (result.length == 0) {
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblErrOrderProvidingNPI').innerHTML = "<p style='color:red'>No Providers found.</p>";
                            document.getElementById('divorderproviderLoader').innerHTML = "";
                        }
                        else if (result.length > 1) {

                            $("[id*=txtNPIORD]").val(txtNPIORD);
                            $('#ProviderSearchModalORD').modal('show');
                            var table = "<table class='gridViewSmallFont providerSearchResultGrid' cellspacing='0' rules='all' border='1' style='width:100%;min-width:860px;border-collapse:collapse;'><tbody><tr class='gridViewHeader'><th scope='col'>NPI</th><th scope='col'>Medicaid ID</th><th scope='col'>Business/Last Name</th><th scope='col'>First Name</a></th><th scope='col'>Address Line 1</th><th scope='col'>Address Line 2</th><th scope='col'>City</th><th scope='col'>State</th><th scope='col'>Zip</th></tr>";
                            for (var i = 0; i < result.length; i++) {
                                let inputString = result[i].LAST_OR_BUSINESS_NAME;

                                table = table + "<tr class='gridViewRow' valign='top'><td style='width:100px;'><a onClick='GetRegisteredNPISelectedRow(\"" + escapeSingleQuote(result[i].NPI) + "\",\"" + escapeSingleQuote(result[i].MEDICAID_ID) + "\",\"" + escapeSingleQuote(result[i].FIRST_NAME) + "\",\"" + escapeSingleQuote(result[i].LAST_OR_BUSINESS_NAME) + "\"); return true;'>" + result[i].NPI + "</a></td><td style='width:100px;'><a onClick='GetRegisteredNPISelectedRow(\"" + escapeSingleQuote(result[i].NPI) + "\",\"" + escapeSingleQuote(result[i].MEDICAID_ID) + "\",\"" + escapeSingleQuote(result[i].FIRST_NAME) + "\",\"" + escapeSingleQuote(result[i].LAST_OR_BUSINESS_NAME) + "\"); return true;'>" + result[i].MEDICAID_ID + "</a></td><td>" + result[i].LAST_OR_BUSINESS_NAME + "</td><td>" + result[i].FIRST_NAME + "</td><td>" + result[i].ADDRESS1 + "</td><td>" + result[i].ADDRESS2 + "</td><td>" + result[i].CITY + "</td><td>" + result[i].STATE + "</td><td>" + result[i].ZIP + "</td></tr>";
                            };
                            table = table + "</tbody></table>";
                            document.getElementById('providerNPIOutput').innerHTML = table;
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblErrOrderProvidingNPI').innerHTML = '';
                            document.getElementById('divorderproviderLoader').innerHTML = "";
                        }
                        else if (result.length == 1) {
                            document.getElementById('<%= lblOrdProviderFName.ClientID%>').innerHTML = result[0].FIRST_NAME;
                            document.getElementById('<%= lblOrdProviderLName.ClientID%>').innerHTML = result[0].LAST_OR_BUSINESS_NAME;
                            document.getElementById('<%= txtOMID.ClientID%>').innerHTML = result[0].MEDICAID_ID;
                            document.getElementById('<%= hdlblOrdProviderFName.ClientID%>').value = result[0].FIRST_NAME;
                            document.getElementById('<%= hdlblOrdProviderLName.ClientID%>').value = result[0].LAST_OR_BUSINESS_NAME;
                            document.getElementById('<%= hdtxtOMID.ClientID%>').value = result[0].MEDICAID_ID;
                            document.getElementById('<%= hdtxtOMID1.ClientID%>').value = result[0].MEDICAID_ID;
                            $("[id*=txtorderingprovidernpi]").val(result[0].NPI);
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblErrOrderProvidingNPI').innerHTML = "";
                            document.getElementById('divorderproviderLoader').innerHTML = "";
                        }
                    }
                    else {
                        if (result.length == 0) {
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblErrOrderProvidingNPI').innerHTML = "<p style='color:red'>No Providers found.</p>";
                            document.getElementById('divorderproviderLoader').innerHTML = "";
                        }
                        else if (result.length == 1) {
                            document.getElementById('<%= lblOrdProviderFName.ClientID%>').innerHTML = result[0].FIRST_NAME;
                            document.getElementById('<%= lblOrdProviderLName.ClientID%>').innerHTML = result[0].LAST_OR_BUSINESS_NAME;
                            document.getElementById('<%= txtOMID.ClientID%>').innerHTML = result[0].MEDICAID_ID;
                            document.getElementById('<%= hdlblOrdProviderFName.ClientID%>').value = result[0].FIRST_NAME;
                            document.getElementById('<%= hdlblOrdProviderLName.ClientID%>').value = result[0].LAST_OR_BUSINESS_NAME;
                            document.getElementById('<%= hdtxtOMID.ClientID%>').value = result[0].MEDICAID_ID;
                            document.getElementById('<%= hdtxtOMID1.ClientID%>').value = result[0].MEDICAID_ID;
                            $("[id*=txtorderingprovidernpi]").val(result[0].NPI);
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblErrOrderProvidingNPI').innerHTML = "";
                            document.getElementById('divorderproviderLoader').innerHTML = "";
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    document.getElementById('providerNPIOutput').innerHTML = 'Something went wrong. Please contact system administrator....!';
                }
            });
        }
    }

    function escapeSingleQuote(inputString, isReverse) {
        if (!isReverse) {
            if (inputString.includes("'")) {
                inputString = inputString.replace(/'/g, "|");
            }
        }
        else {
            if (inputString.includes("|")) {
                inputString = inputString.replace(/\|/g, "'");
            }
        }
        return inputString;
    }

    function GetServiceProviderDetails() {
        var txtNPI = $("#<%= txtNPI.ClientID %>").first().val();
        var txtProMedicaidID = $("#<%= txtProMedicaidID.ClientID %>").first().val();
        var txtBusinessLastName = $("#<%= txtBusinessLastName.ClientID %>").first().val();
        var txtFirstName = $("#<%= txtFirstName.ClientID %>").first().val();
        document.getElementById('ServiceproviderNPIOutput').innerHTML = "<img src='../Images/loader.gif'/>";
        if (txtNPI.length > 0 && txtNPI.length < 10) {
            document.getElementById('ServiceproviderNPIOutput').innerHTML = "<p style='color:red'>NPI should be 10-digit number.</p>";
            return false;
        }
        else if (txtProMedicaidID.length > 0 && txtProMedicaidID.length < 7) {
            document.getElementById('ServiceproviderNPIOutput').innerHTML = "<p style='color:red'>Medicaid ID should be 7-digit number.</p>";
            return false;
        }
        else if (txtNPI.length <= 0 && txtProMedicaidID.length <= 0 && txtBusinessLastName.length <= 0 && txtFirstName.length <= 0) {
            document.getElementById('ServiceproviderNPIOutput').innerHTML = "<p style='color:red'>NPI, Medicaid ID, Business/Last Name or First Name is required</p>";
            return false;
        }
        else {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetProviderNPI?npi=" + txtNPI + "&&medicaidID=" + txtProMedicaidID + "&&lastName=" + txtBusinessLastName + "&&firstName=" + txtFirstName,
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
                        document.getElementById('ServiceproviderNPIOutput').innerHTML = 'No Providers found.';
                    }
                    else {
                        var table = "<table class='gridViewSmallFont providerSearchResultGrid' cellspacing='0' rules='all' border='1' style='width:100%;border-collapse:collapse;min-width: 865px;'><tbody><tr class='gridViewHeader'><th scope='col'>NPI</th><th scope='col'>Medicaid ID</th><th scope='col'>Business/Last Name</th><th scope='col'>First Name</a></th><th scope='col'>Address Line 1</th><th scope='col'>Address Line 2</th><th scope='col'>City</th><th scope='col'>State</th><th scope='col'>Zip</th></tr>";
                        for (var i = 0; i < result.length; i++) {
                            table = table + "<tr class='gridViewRow' valign='top'><td style='width:100px;'><a onClick='GetServiceRegisteredNPISelectedRow(\"" + result[i].NPI + "\",\"" + result[i].MEDICAID_ID + "\",\"" + escapeSingleQuote(result[i].FIRST_NAME, false) + "\",\"" + escapeSingleQuote(result[i].LAST_OR_BUSINESS_NAME, false) + "\"); return true;'>" + result[i].NPI + "</a></td><td style='width:100px;'><a onClick='GetServiceRegisteredNPISelectedRow(\"" + result[i].NPI + "\",\"" + result[i].MEDICAID_ID + "\",\"" + escapeSingleQuote(result[i].FIRST_NAME, false) + "\",\"" + escapeSingleQuote(result[i].LAST_OR_BUSINESS_NAME, false) + "\"); return true;'>" + result[i].MEDICAID_ID + "</a></td><td>" + result[i].LAST_OR_BUSINESS_NAME + "</td><td>" + result[i].FIRST_NAME + "</td><td>" + result[i].ADDRESS1 + "</td><td>" + result[i].ADDRESS2 + "</td><td>" + result[i].CITY + "</td><td>" + result[i].STATE + "</td><td>" + result[i].ZIP + "</td></tr>";
                        };
                        table = table + "</tbody></table>";
                        document.getElementById('ServiceproviderNPIOutput').innerHTML = table;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    document.getElementById('ServiceproviderNPIOutput').innerHTML = 'Something went wrong. Please contact system administrator....!';
                }
            });
            return false;
        }
    }
    function GetServiceRegisteredNPISelectedRow(npi, medicaid, firstName, lastName) {

        document.getElementById('divServiceproviderLoader').innerHTML = "<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
        document.getElementById('<%= lblSvcProviderFName.ClientID%>').innerHTML = escapeSingleQuote(firstName, true);
        document.getElementById('<%= lblSvcProviderLName.ClientID%>').innerHTML = escapeSingleQuote(lastName, true);
        document.getElementById('<%= txtMedicaidID.ClientID%>').innerHTML = medicaid;
        document.getElementById('<%= hdlblSvcProviderFName.ClientID%>').value = escapeSingleQuote(firstName, true);
        document.getElementById('<%= hdlblSvcProviderLName.ClientID%>').value = escapeSingleQuote(lastName, true);
        document.getElementById('<%= hdtxtMedicaidID.ClientID%>').value = medicaid;
        $("[id*=txtSPNPI]").val(npi);
        $("[id*=txtNPI]").val('');
        $("[id*=txtProMedicaidID]").val('');
        $("[id*=txtBusinessLastName]").val('');
        $("[id*=txtFirstName]").val('');

        var errProviderNPI = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_errProviderNPI');
        if (errProviderNPI) {
            errProviderNPI.innerHTML = "";
        }
        document.getElementById('divServiceproviderLoader').innerHTML = "";
        document.getElementById('ServiceproviderNPIOutput').innerHTML = '';
        $('#ProviderSearchModal').modal('hide');
    }
    function OnServiceProviderTextChanged(pageLoad) {

        if (pageLoad == false) {
            document.getElementById('<%= lblSvcProviderFName.ClientID%>').innerHTML = "";
            document.getElementById('<%= lblSvcProviderLName.ClientID%>').innerHTML = "";
            document.getElementById('<%= txtMedicaidID.ClientID%>').innerHTML = "";
            document.getElementById('<%= hdlblSvcProviderFName.ClientID%>').value = "";
            document.getElementById('<%= hdlblSvcProviderLName.ClientID%>').value = "";
            document.getElementById('<%= hdtxtMedicaidID.ClientID%>').value = "";
        }
        var txtSPNPI = $("#<%= txtSPNPI.ClientID %>").first().val();
        document.getElementById('divServiceproviderLoader').innerHTML = ""; //"<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
        var txtProMedicaidIDSvc = '';
        var txtBusinessLastNameSvc = '';
        var txtFirstNameSvc = '';
        if (txtSPNPI.length <= 0) {
            if (pageLoad == false) {
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_errProviderNPI').innerHTML = "<p style='color:red'>Please Enter valid NPI.</p>";
            }
            document.getElementById('divServiceproviderLoader').innerHTML = "";
            return false;
        }
        else if (txtSPNPI.length > 0 && txtSPNPI.length < 10) {
            if (pageLoad == false) {
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_errProviderNPI').innerHTML = "<p style='color:red'>10-digit number is required.</p>";
            }
            document.getElementById('divServiceproviderLoader').innerHTML = "";
            return false;
        }
        else {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetProviderNPI?npi=" + txtSPNPI + "&&medicaidID=" + txtProMedicaidIDSvc + "&&lastName=" + txtBusinessLastNameSvc + "&&firstName=" + txtFirstNameSvc,
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (pageLoad == false) {
                        if (result.length == 0) {
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_errProviderNPI').innerHTML = "<p style='color:red'>No Providers found.</p>";
                            document.getElementById('divServiceproviderLoader').innerHTML = "";
                        }
                        else if (result.length > 1) {
                            $("[id*=txtNPI]").val(txtSPNPI);
                            $('#ProviderSearchModal').modal('show');
                            var table = "<table class='gridViewSmallFont providerSearchResultGrid' cellspacing='0' rules='all' border='1' style='width:100%;border-collapse:collapse;min-width: 865px;'><tbody><tr class='gridViewHeader'><th scope='col'>NPI</th><th scope='col'>Medicaid ID</th><th scope='col'>Business/Last Name</th><th scope='col'>First Name</a></th><th scope='col'>Address Line 1</th><th scope='col'>Address Line 2</th><th scope='col'>City</th><th scope='col'>State</th><th scope='col'>Zip</th></tr>";
                            for (var i = 0; i < result.length; i++) {
                                table = table + "<tr class='gridViewRow' valign='top'><td style='width:100px;'><a onClick='GetServiceRegisteredNPISelectedRow(\"" + result.d[i].NPI + "\",\"" + result.d[i].MEDICAID_ID + "\",\"" + result.d[i].FIRST_NAME + "\",\"" + result.d[i].LAST_OR_BUSINESS_NAME + "\"); return true;'>" + result.d[i].NPI + "</a></td><td style='width:100px;'><a onClick='GetServiceRegisteredNPISelectedRow(\"" + result.d[i].NPI + "\",\"" + result.d[i].MEDICAID_ID + "\",\"" + result.d[i].FIRST_NAME + "\",\"" + result.d[i].LAST_OR_BUSINESS_NAME + "\"); return true;'>" + result.d[i].MEDICAID_ID + "</a></td><td>" + result.d[i].LAST_OR_BUSINESS_NAME + "</td><td>" + result.d[i].FIRST_NAME + "</td><td>" + result.d[i].ADDRESS1 + "</td><td>" + result.d[i].ADDRESS2 + "</td><td>" + result.d[i].CITY + "</td><td>" + result.d[i].STATE + "</td><td>" + result.d[i].ZIP + "</td></tr>";
                            };
                            table = table + "</tbody></table>";
                            document.getElementById('ServiceproviderNPIOutput').innerHTML = table;
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_errProviderNPI').innerHTML = '';
                            document.getElementById('divServiceproviderLoader').innerHTML = "";
                        }
                        else if (result.length == 1) {

                            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_lblSvcProviderFName').text(result[0].FIRST_NAME);
                            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_lblSvcProviderLName').text(result[0].LAST_OR_BUSINESS_NAME);
                            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtMedicaidID').text(result[0].MEDICAID_ID);
                            document.getElementById('<%= lblSvcProviderFName.ClientID%>').innerHTML = result[0].FIRST_NAME;
                            document.getElementById('<%= lblSvcProviderLName.ClientID%>').innerHTML = result[0].LAST_OR_BUSINESS_NAME;
                            document.getElementById('<%= txtMedicaidID.ClientID%>').innerHTML = result[0].MEDICAID_ID;
                            document.getElementById('<%= hdlblSvcProviderFName.ClientID%>').value = result[0].FIRST_NAME;
                            document.getElementById('<%= hdlblSvcProviderLName.ClientID%>').value = result[0].LAST_OR_BUSINESS_NAME;
                            document.getElementById('<%= hdtxtMedicaidID.ClientID%>').value = result[0].MEDICAID_ID;
                            $("[id*=txtSPNPI]").val(result[0].NPI);
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_errProviderNPI').innerHTML = "";
                            document.getElementById('divServiceproviderLoader').innerHTML = "";
                        }
                    }
                    else {

                        if (result.length == 0) {
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_errProviderNPI').innerHTML = "<p style='color:red'>No Providers found.</p>";
                            document.getElementById('divServiceproviderLoader').innerHTML = "";
                        }
                        else if (result.length == 1) {
                            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_lblSvcProviderFName').text(result[0].FIRST_NAME);
                            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_lblSvcProviderLName').text(result[0].LAST_OR_BUSINESS_NAME);
                            $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtMedicaidID').text(result[0].MEDICAID_ID);
                            document.getElementById('<%= lblSvcProviderFName.ClientID%>').innerHTML = result[0].FIRST_NAME;
                            document.getElementById('<%= lblSvcProviderLName.ClientID%>').innerHTML = result[0].LAST_OR_BUSINESS_NAME;
                            document.getElementById('<%= txtMedicaidID.ClientID%>').innerHTML = result[0].MEDICAID_ID;
                            document.getElementById('<%= hdlblSvcProviderFName.ClientID%>').value = result[0].FIRST_NAME;
                            document.getElementById('<%= hdlblSvcProviderLName.ClientID%>').value = result[0].LAST_OR_BUSINESS_NAME;
                            document.getElementById('<%= hdtxtMedicaidID.ClientID%>').value = result[0].MEDICAID_ID;
                            $("[id*=txtSPNPI]").val(result[0].NPI);
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_errProviderNPI').innerHTML = "";
                            document.getElementById('divServiceproviderLoader').innerHTML = "";
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    document.getElementById('ServiceproviderNPIOutput').innerHTML = 'Something went wrong. Please contact system administrator....!';
                }
            });
        }
    }
    function CloseServiceProviderpopup() {
        $("[id*=txtNPI]").val('');
        $("[id*=txtProMedicaidID]").val('');
        $("[id*=txtBusinessLastName]").val('');
        $("[id*=txtFirstName]").val('');
        document.getElementById('divServiceproviderLoader').innerHTML = "";
        document.getElementById('ServiceproviderNPIOutput').innerHTML = '';
    }
    function visibleFacilityType() {
        try {
            $("[id*=txtFacilityTypeCode]").val('');
            $("[id*=txtFacilityTypeDescription]").val('');
            document.getElementById('facilityTypeOutput').innerHTML = '';
            $('#FacilityTypeSearchModal').modal('show');
        }
        catch (err) {
            console.log(err);
        }
        return false;
    }
    function hideFacilityType() {
        try {
            $("[id*=txtFacilityTypeCode]").val('');
            $("[id*=txtFacilityTypeDescription]").val('');
            document.getElementById('facilityTypeOutput').innerHTML = '';
            $('#FacilityTypeSearchModal').modal('hide');
        }
        catch (err) {
            console.log(err);
        }
        return false;
    }
    function GetFacilityTypes() {
        var txtValueCode = $("#<%= txtFacilityTypeCode.ClientID %>").first().val();
        var txtvaluecodedesc = $("#<%= txtFacilityTypeDescription.ClientID %>").first().val();
        document.getElementById('facilityTypeOutput').innerHTML = "<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
        if (!txtValueCode.match(/\S/) && !txtvaluecodedesc.match(/\S/)) {
            document.getElementById('facilityTypeOutput').innerHTML = "<p style='color:red'>Either Facility Code or Description is Required to perform Search.</p>";
            return false;
        } else {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetFacilityTypeData?facilityCode=" + txtValueCode + "&&facilityDesc=" + txtvaluecodedesc + "&&isTextChangesEvent=" + false,
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var searchResult = "<div style='background-color: darkcyan;'><span class='expandcollapse' style='font-weight: bold; font-size: 14px; color: white; padding-left: 20px;'>SEARCH RESULTS</span></div>";
                    if (result.length == 0) {
                        document.getElementById('facilityTypeOutput').innerHTML = searchResult + 'No data found.';
                    }
                    else {
                        var table = searchResult + "<table class='gridViewSmallFont FacilityTypeSearchResultGrid' cellspacing='0' rules='all' border='1' style='width:100%;border-collapse:collapse;'><tbody><tr class='gridViewHeader' style='color:Black;'><th scope='col'>Facility Type Code</th><th scope='col'>Facility Type Description</th></tr>";
                        for (var i = 0; i < result.length; i++) {
                            table = table + "<tr class='gridViewRow' valign='top'><td style='width:100px;'><a onClick='GetFacilitySelectedRow(\"" + result[i].FacilityTypeCode + "\"); return false;'>" + result[i].FacilityTypeCode + "</a></td><td>" + result[i].FacilityTypeDesc + "</td></tr>";
                        };
                        table = table + "</table>";
                        document.getElementById('facilityTypeOutput').innerHTML = table;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(JSON.stringify(jqXHR));
                    document.getElementById('facilityTypeOutput').innerHTML = 'Something went wrong. Please contact system administrator....!';
                }
            });
            return false;
        }
    }
    function GetFacilitySelectedRow(code) {
        document.getElementById('<%= lblfacilityTypeError.ClientID%>').innerHTML = '';
        var facilityDesc = '';
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetFacilityTypeData?facilityCode=" + code + "&&facilityDesc=" + facilityDesc + "&&isTextChangesEvent=" + true,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                for (var i = 0; i < result.length; i++) {
                    $("[id*=txtfacilityType]").val(result[i].FacilityTypeCode + " - " + result[i].FacilityTypeDesc);
                    $('#FacilityTypeSearchModal').modal('hide');
                    return true;
                };
            },
            error: function (jqXHR, textStatus, errorThrown) {
                document.getElementById('facilityTypeOutput').innerHTML = 'Something went wrong. Please contact system administrator....!';
            }
        });
    }
    function OnFacilityTypeTextChanged(pageload) {
        document.getElementById('divfacilityLoader').innerHTML = "<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
        document.getElementById('<%= lblfacilityTypeError.ClientID%>').innerHTML = '';
        var txtfacilityType = $("#<%= txtfacilityType.ClientID %>").first().val();
        txtfacilityType = txtfacilityType.replace(/(^\s+|\s+$)/g, '');
        var facilityDesc = '';
        if (txtfacilityType != '' && txtfacilityType != null && txtfacilityType != undefined) {
            txtfacilityType = txtfacilityType.length > 3 ? txtfacilityType.substr(0, 4).trim() : txtfacilityType.trim();
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetFacilityTypeData?facilityCode=" + txtfacilityType + "&&facilityDesc=" + facilityDesc + "&&isTextChangesEvent=" + true,
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
                        document.getElementById('divfacilityLoader').innerHTML = "";
                        document.getElementById('<%= lblfacilityTypeError.ClientID%>').innerHTML = 'Facility type is invalid';
                    }
                    else if (result.length > 1) {
                        document.getElementById('divfacilityLoader').innerHTML = "";
                        $("[id*=txtFacilityTypeCode]").val(txtfacilityType);
                        var table = searchResult + "<table class='gridViewSmallFont FacilityTypeSearchResultGrid' cellspacing='0' rules='all' border='1' style='width:100%;border-collapse:collapse;'><tbody><tr class='gridViewHeader' style='color:Black;'><th scope='col'>Facility Type Code</th><th scope='col'>Facility Type Description</th></tr>";
                        for (var i = 0; i < result.length; i++) {
                            table = table + "<tr class='gridViewRow' valign='top'><td style='width:100px;'><a onClick='GetFacilitySelectedRow(\"" + result[i].FacilityTypeCode + "\"); return false;'>" + result[i].FacilityTypeCode + "</a></td><td>" + result[i].FacilityTypeDesc + "</td></tr>";
                        };
                        table = table + "</table>";
                        document.getElementById('facilityTypeOutput').innerHTML = table;
                        $('#FacilityTypeSearchModal').modal('show');
                    }
                    else {
                        document.getElementById('divfacilityLoader').innerHTML = "";
                        $("[id*=txtfacilityType]").val(result[0].FacilityTypeCode + " - " + result[0].FacilityTypeDesc);
                        document.getElementById('<%= lblfacilityTypeError.ClientID%>').innerHTML = '';
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(JSON.stringify(jqXHR));
                }
            });
        }
        else {
            if (pageLoad == false) {
                document.getElementById('divfacilityLoader').innerHTML = "";
                document.getElementById('<%= lblfacilityTypeError.ClientID%>').innerHTML = 'Facility type is invalid';
            }
            document.getElementById('divfacilityLoader').innerHTML = '';
        }
    }

    /*Service Details Client Side*/

    function visibleServiceDetailspopup(popupType, paType) {
        try {
            if (popupType == 'RevenueCode' && paType == 'Institutional') {
                $("[id*=txtHCPCSCode]").val('');
                $("[id*=txtServDetRevCodeDesc]").val('');
                document.getElementById('revenueTypeOutput').innerHTML = '';
                $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeServiceDetailsRevenueCodeSearch").show();
            }
            if (popupType == 'ProcedureCode' && paType == 'Institutional') {
                $("[id*=txtCode]").val('');
                $("[id*=txtPlaceOfServiceName]").val('');
                document.getElementById('procedureCodeOutput').innerHTML = '';
                $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeSubmitPriorAuthSearchProc").show();
            }
            if (popupType == 'ProcedureCode' && paType == 'Dental') {
                $("[id*=txtCode]").val('');
                $("[id*=txtPlaceOfServiceName]").val('');
                document.getElementById('procedureCodeOutput').innerHTML = '';
                $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeSubmitPriorAuthSearchProc").show();
            }
            if (popupType == 'ProcedureCode' && paType == 'Professional') {
                $("[id*=txtCode]").val('');
                $("[id*=txtPlaceOfServiceName]").val('');
                document.getElementById('procedureCodeOutput').innerHTML = '';
                $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeSubmitPriorAuthSearchProc").show();
            }
            //ProcedureCode
        }
        catch (err) {
            console.log(err);
        }
        return false;
    }
    function hideServiceDetailspopup(popupType, paType) {
        try {
            if (popupType == 'RevenueCode' && paType == 'Institutional') {
                $("[id*=txtHCPCSCode]").val('');
                $("[id*=txtServDetRevCodeDesc]").val('');
                document.getElementById('revenueTypeOutput').innerHTML = '';
                $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeServiceDetailsRevenueCodeSearch").hide();
            }
            if (popupType == 'ProcedureCode' && paType == 'Institutional') {
                $("[id*=txtCode]").val('');
                $("[id*=txtPlaceOfServiceName]").val('');
                document.getElementById('procedureCodeOutput').innerHTML = '';
                $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeSubmitPriorAuthSearchProc").hide();
            }
        }
        catch (err) {
            console.log(err);
        }
        return false;
    }
    function OnrevenueCodeTextChanged() {

        if ($('#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlAssignment').val() == '37' && !validateRevenueCodeForPsychiatricIP()) {
            document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = 'Invalid Revenue code for Psychiatric Inpatient.';
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "visible";
        }
        else {
            document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = '';
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "hidden";
        }

        document.getElementById('divRevenueCodeLoader1').innerHTML = "<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
        document.getElementById('<%= lblServDetailsrevenueCode.ClientID%>').innerHTML = '';
        var txtServicecode = $("#<%= txtServicecode.ClientID %>").first().val();
        var revenueCodeDesc = '';
        txtServicecode = txtServicecode.replace(/(^\s+|\s+$)/g, '');
        if (txtServicecode != '' && txtServicecode != null && txtServicecode != undefined) {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetRevenueCodeData?revenueCode=" + txtServicecode + "&&revenueCodeDesc=" + revenueCodeDesc + "&&isTextChangesEvent=" + true,
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
                        document.getElementById('divRevenueCodeLoader1').innerHTML = "";
                        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsrevenueCode').style.visibility = "visible";
                        document.getElementById('<%= lblServDetailsrevenueCode.ClientID%>').innerHTML = 'Revenue code is invalid';
                    }
                    else if (result.length > 1) {
                        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsrevenueCode').style.visibility = "hidden";
                        document.getElementById('divRevenueCodeLoader1').innerHTML = "";
                        $("[id*=txtHCPCSCode]").val(txtServicecode);
                        var table = "<table class='gridViewSmallFont FacilityTypeSearchResultGrid' cellspacing='0' rules='all' border='1' style='width:100%;border-collapse:collapse;'><tbody><tr class='gridViewHeader' style='color:Black;'><th scope='col'>Revenue Code</th><th scope='col'>Revenue Code Description</th></tr>";
                        for (var i = 0; i < result.length; i++) {
                            table = table + "<tr class='gridViewRow' valign='top'><td style='width:100px;'><a onClick='GetRevenueSelectedRow(\"" + result[i].RevenueCode + "\"); return false;'>" + result[i].RevenueCode + "</a></td><td>" + result[i].RevenueCodeDesc + "</td></tr>";
                        };
                        table = table + "</table>";
                        document.getElementById('revenueTypeOutput').innerHTML = table;
                        document.getElementById('divRevenueCodeLoader1').innerHTML = "";
                        $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeServiceDetailsRevenueCodeSearch").show();
                    }
                    else {
                        document.getElementById('divRevenueCodeLoader1').innerHTML = "";
                        $("[id*=txtServicecode]").val(result[0].RevenueCode);
                        document.getElementById('<%= lblServDetailsrevenueCode.ClientID%>').innerHTML = '';
                        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsrevenueCode').style.visibility = "hidden";
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(JSON.stringify(jqXHR));
                }
            });
        }
        else {
            document.getElementById('divRevenueCodeLoader1').innerHTML = "";
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsrevenueCode').style.visibility = "hidden";
            document.getElementById('<%= lblServDetailsrevenueCode.ClientID%>').innerHTML = 'Revenue code is invalid';
        }
    }

    function validateRevenueCodeForPsychiatricIP() {
        var txtServicecode = $("#<%= txtServicecode.ClientID %>").first().val();

        if (txtServicecode != '' && txtServicecode != null && txtServicecode != undefined) {
            const revCodeNum = Number(txtServicecode);

            if (!isNaN(revCodeNum) && (revCodeNum >= 100 && revCodeNum <= 219)) {
                return true;
            }
            else {
                return false;
            }
        }
    }


    function GetrevenueCodes() {
        var txtHCPCSCode = $("#<%= txtHCPCSCode.ClientID %>").first().val();
        var txtServDetRevCodeDesc = $("#<%= txtServDetRevCodeDesc.ClientID %>").first().val();
        document.getElementById('revenueTypeOutput').innerHTML = "<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
        if (!txtHCPCSCode.match(/\S/) && !txtServDetRevCodeDesc.match(/\S/)) {
            document.getElementById('revenueTypeOutput').innerHTML = "<p style='color:red'>Either Revenue Code or Description is Required to perform Search.</p>";
            return false;
        } else {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetRevenueCodeData?revenueCode=" + txtHCPCSCode + "&&revenueCodeDesc=" + txtServDetRevCodeDesc + "&&isTextChangesEvent=" + false,
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var searchResult = "<div style='background-color: darkcyan;'><span class='expandcollapse' style='font-weight: bold; font-size: 14px; color: white; padding-left: 20px;'>SEARCH RESULTS</span></div>";
                    if (result.length == 0) {
                        document.getElementById('revenueTypeOutput').innerHTML = searchResult + 'No data found.';
                    }
                    else {
                        var table = searchResult + "<table id='revenueCodeData' class='gridViewSmallFont FacilityTypeSearchResultGrid' cellspacing='0' rules='all' border='1' style='width:100%;border-collapse:collapse;'><tbody><tr class='gridViewHeader' style='color:Black;'><th scope='col'>Revenue Code</th><th scope='col'>Revenue Code Description</th></tr>";
                        for (var i = 0; i < result.length; i++) {
                            table = table + "<tr class='gridViewRow' valign='top'><td style='width:100px;'><a onClick='GetrevenueCodeSelectedRow(\"" + result[i].RevenueCode + "\"); return false;'>" + result[i].RevenueCode + "</a></td><td>" + result[i].RevenueCodeDesc + "</td></tr>";
                        };
                        table = table + "</table>";
                        document.getElementById('revenueTypeOutput').innerHTML = table;
                        if (result.length > 10) {
                            $('#revenueCodeData').paging({ limit: 10 });
                        }

                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(JSON.stringify(jqXHR));
                    document.getElementById('revenueTypeOutput').innerHTML = 'Something went wrong. Please contact system administrator....!';
                }
            });
            return false;
        }
    }
    function GetrevenueCodeSelectedRow(code) {
        document.getElementById('<%= lblServDetailsrevenueCode.ClientID%>').innerHTML = '';
        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsrevenueCode').style.visibility = "hidden";
        var revenueCodeDesc = '';
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetRevenueCodeData?revenueCode=" + code + "&&revenueCodeDesc=" + revenueCodeDesc + "&&isTextChangesEvent=" + true,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                $("[id*=txtServicecode]").val(result[0].RevenueCode);
                $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeServiceDetailsRevenueCodeSearch").hide();
                return true;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(JSON.stringify(jqXHR));
                document.getElementById('revenueTypeOutput').innerHTML = 'Something went wrong. Please contact system administrator....!';
            }
        });
    }

    function GetprocedureCodes(paType) {

        var txtCode = $("#<%= txtCode.ClientID %>").first().val();
        var txtPlaceOfServiceName = $("#<%= txtPlaceOfServiceName.ClientID %>").first().val();
        document.getElementById('procedureCodeOutput').innerHTML = "<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
        if (!txtCode.match(/\S/) && !txtPlaceOfServiceName.match(/\S/)) {
            document.getElementById('procedureCodeOutput').innerHTML = "<p style='color:red'>Procedure code and/or description is required.</p>";
            return false;
        } else {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetProcedureCodeData?procCode=" + txtCode + "&&procCodeDesc=" + txtPlaceOfServiceName + "&&isTextChangesEvent=" + false,
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var searchResult = "<div style='background-color: darkcyan;'><span class='expandcollapse' style='font-weight: bold; font-size: 14px; color: white; padding-left: 20px;'>SEARCH RESULTS</span></div>";
                    if (result.length == 0) {
                        document.getElementById('procedureCodeOutput').innerHTML = searchResult + 'No data found.';
                    }
                    else {
                        var table = "<table id='tableData' class='gridViewSmallFont FacilityTypeSearchResultGrid' cellspacing='0' rules='all' border='1' style='width:100%;border-collapse:collapse;'><tbody><tr class='gridViewHeader' style='color:Black;'><th scope='col'>Procedure Code</th><th scope='col'>Procedure Code Description</th></tr>";
                        for (var i = 0; i < result.length; i++) {
                            table = table + "<tr class='gridViewRow' valign='top'><td style='width:100px;'><a onClick='GetProcedureCodeSelectedRow(\"" + result[i].ProcedureCode + "\"); return false;'>" + result[i].ProcedureCode + "</a></td><td>" + result[i].ProcedureCodeDesc + "</td></tr>";
                        };
                        table = table + "</table>";
                        document.getElementById('procedureCodeOutput').innerHTML = table;
                        if (result.length > 10) {
                            $('#tableData').paging({ limit: 10 });
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(JSON.stringify(jqXHR));
                    document.getElementById('procedureCodeOutput').innerHTML = 'Something went wrong. Please contact system administrator....!';
                }
            });
            return false;
        }
    }
    function GetProcedureCodeSelectedRow(code) {
        var ClaimType = $(".rblClaimType input:checked").val();
        var procCodeDesc = '';
        // document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = '';
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetProcedureCodeData?procCode=" + code + "&&procCodeDesc=" + procCodeDesc + "&&isTextChangesEvent=" + false,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (ClaimType == 'dental') {
                    document.getElementById('<%= lblDentalProcMessage.ClientID%>').innerHTML = '';
                    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtDentalSDProcCode").style.display = 'none';
                    $("[id*=txtDentalSDProcCode]").val(result[0].ProcedureCode);
                }
                else if (ClaimType == 'Professional') {
                    document.getElementById('<%= lblProfProcMessage.ClientID%>').innerHTML = '';
                    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtProfessionalSDProcCode").style.display = 'none';
                    $("[id*=txtProfessionalSDProcCode]").val(result[0].ProcedureCode);
                }
                else {
                    document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = '';
                    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtLnProcedureCode").style.display = 'none';
                    $("[id*=txtLnProcedureCode]").val(result[0].ProcedureCode);
                }
                $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeSubmitPriorAuthSearchProc").hide();
                return true;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(JSON.stringify(jqXHR));
                document.getElementById('procedureCodeOutput').innerHTML = 'Something went wrong. Please contact system administrator....!';
            }
        });
    }

    function OnServiceTypeCodeIndexChanged(code) {

        var ddlServiceTypeCode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlServiceTypeCode");
        var procCodeType = ddlServiceTypeCode.value;
        var procCodeTypeText = ddlServiceTypeCode.options[ddlServiceTypeCode.selectedIndex].text;

        if (procCodeTypeText !== '--- Please select ---') {
            OnprocedureCodeTextChanged_Institutional();
        }
    }

    function OnprocedureCodeTextChanged_Institutional() {
        var ddlServiceTypeCode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlServiceTypeCode");
        var procCodeType = ddlServiceTypeCode.value;
        var procCodeTypeText = ddlServiceTypeCode.options[ddlServiceTypeCode.selectedIndex].text;

        if ($('#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlAssignment').val() == '37' && procCodeTypeText == '--- Please select ---') {
            /*document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "hidden";*/
            document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = 'Please select Procedure Code Type.';
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "visible";
            document.getElementById('divProcCodeLoader1').innerHTML = "";
        }
        else if (procCodeTypeText == '--- Please select ---') {
            document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = 'Please select Procedure Code Type.';
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "visible";
            document.getElementById('divProcCodeLoader1').innerHTML = "";
        }
        else {
            var ClaimType = $(".rblClaimType input:checked").val();
            document.getElementById('divProcCodeLoader1').innerHTML = "<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
            document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtLnProcedureCode").style.display = 'none';
            document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = '';

            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "hidden";
            txtLnProcedureCode = $("#<%= txtLnProcedureCode.ClientID %>").first().val();

            txtLnProcedureCode = txtLnProcedureCode.replace(/(^\s+|\s+$)/g, '');
            if (txtLnProcedureCode != '' && txtLnProcedureCode != null && txtLnProcedureCode != undefined) {
                var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "GET",
                    url: webApiPA + "GetProcedureCodeData_InstitutionalDynamic?procCode=" + txtLnProcedureCode + "&&procCodeDesc=" + procCodeTypeText,
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
                            if (ClaimType == 'Institutional') {
                                document.getElementById('divProcCodeLoader1').innerHTML = "";
                                document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = 'Procedure Code is invalid for selected Procedure Type';
                                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "visible";
                            }
                        }
                        else if (result.length > 1) {
                            $("[id*=txtHCPCSCode]").val(txtServicecode);
                            var table = "<table class='gridViewSmallFont FacilityTypeSearchResultGrid' cellspacing='0' rules='all' border='1' style='width:100%;border-collapse:collapse;'><tbody><tr class='gridViewHeader' style='color:Black;'><th scope='col'>Procedure Code</th><th scope='col'>Procedure Code Description</th></tr>";
                            for (var i = 0; i < result.length; i++) {
                                table = table + "<tr class='gridViewRow' valign='top'><td style='width:100px;'><a onClick='GetRevenueSelectedRow(\"" + result[i].ProcedureCode + "\"); return false;'>" + result[i].ProcedureCode + "</a></td><td>" + result[i].ProcedureCodeDesc + "</td></tr>";
                            };
                            table = table + "</table>";
                            document.getElementById('procedureCodeOutput').innerHTML = table;
                            if (ClaimType == 'Institutional') {
                                document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = '';
                                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "hidden";
                                document.getElementById('divProcCodeLoader1').innerHTML = "";
                            }

                            $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeSubmitPriorAuthSearchProc").show();
                        }
                        else {
                            if (ClaimType == 'Institutional') {
                                $("[id*=txtLnProcedureCode]").val(result[0].ProcedureCode);
                                document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = '';
                                document.getElementById('divProcCodeLoader1').innerHTML = "";
                                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "hidden";
                            }
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        console.log(JSON.stringify(jqXHR));
                    }
                });
            }
            else {

                if (ClaimType == 'Institutional') {

                    var PAnumberValue = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_txtPANumber2");
                    var isNonConvertedPA = false;
                    if (PAnumberValue && PAnumberValue.value.includes("AUTH")) {
                        isNonConvertedPA = true;
                    }

                    var ddlAssignment = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlAssignment");
                    var assignmentvalue = ddlAssignment.value;
                    var assignmenttext = ddlAssignment.options[ddlAssignment.selectedIndex].text;

                    var isPsychiatricInpatientAssignment = false;
                    if (assignmenttext == 'Psychiatric Inpatient') {
                        isPsychiatricInpatientAssignment = true;
                    }
                    if (!isNonConvertedPA && !isPsychiatricInpatientAssignment) {
                        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtLnProcedureCode").style.display = 'block';
                        document.getElementById('divProcCodeLoader1').innerHTML = "";
                    }
                }
            }
        }
    }

    function OnprocedureCodeTextChanged(paType) {
        var ClaimType = $(".rblClaimType input:checked").val();

        var txtLnProcedureCode = '';
        if (ClaimType == 'Institutional') {
            document.getElementById('divProcCodeLoader1').innerHTML = "<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
            document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtLnProcedureCode").style.display = 'none';
            document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = '';

            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "hidden";
            txtLnProcedureCode = $("#<%= txtLnProcedureCode.ClientID %>").first().val();
        }
        if (ClaimType == 'dental') {
            document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtDentalSDProcCode").style.display = 'none';
            document.getElementById('divProcCodeLoaderDental').innerHTML = "<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
            document.getElementById('<%= lblDentalProcMessage.ClientID%>').innerHTML = '';
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalProcMessage').style.visibility = "hidden";
            txtLnProcedureCode = $("#<%= txtDentalSDProcCode.ClientID %>").first().val();
        }
        if (ClaimType == 'Professional') {
            document.getElementById('divProcCodeLoaderProfessional').innerHTML = "<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
            document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtProfessionalSDProcCode").style.display = 'none';
            document.getElementById('<%= lblProfProcMessage.ClientID%>').innerHTML = '';
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfProcMessage').style.visibility = "hidden";
            txtLnProcedureCode = $("#<%= txtProfessionalSDProcCode.ClientID %>").first().val();
        }


        txtLnProcedureCode = txtLnProcedureCode.replace(/(^\s+|\s+$)/g, '');
        if (txtLnProcedureCode != '' && txtLnProcedureCode != null && txtLnProcedureCode != undefined) {
            var procCodeDesc = '';
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetProcedureCodeData?procCode=" + txtLnProcedureCode + "&&procCodeDesc=" + procCodeDesc + "&&isTextChangesEvent=" + true,
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
                        if (ClaimType == 'Institutional') {
                            document.getElementById('divProcCodeLoader1').innerHTML = "";
                            document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = 'Procedure code is invalid';
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "visible";
                        }
                        if (ClaimType == 'dental') {
                            document.getElementById('divProcCodeLoaderDental').innerHTML = "";
                            document.getElementById('<%= lblDentalProcMessage.ClientID%>').innerHTML = 'Procedure code is invalid';
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalProcMessage').style.visibility = "visible";
                        }
                        if (ClaimType == 'Professional') {
                            document.getElementById('divProcCodeLoaderProfessional').innerHTML = "";
                            document.getElementById('<%= lblProfProcMessage.ClientID%>').innerHTML = 'Procedure code is invalid';
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfProcMessage').style.visibility = "visible";
                        }
                    }
                    else if (result.length > 1) {
                        $("[id*=txtHCPCSCode]").val(txtServicecode);
                        var table = "<table class='gridViewSmallFont FacilityTypeSearchResultGrid' cellspacing='0' rules='all' border='1' style='width:100%;border-collapse:collapse;'><tbody><tr class='gridViewHeader' style='color:Black;'><th scope='col'>Procedure Code</th><th scope='col'>Procedure Code Description</th></tr>";
                        for (var i = 0; i < result.length; i++) {
                            table = table + "<tr class='gridViewRow' valign='top'><td style='width:100px;'><a onClick='GetRevenueSelectedRow(\"" + result[i].ProcedureCode + "\"); return false;'>" + result[i].ProcedureCode + "</a></td><td>" + result[i].ProcedureCodeDesc + "</td></tr>";
                        };
                        table = table + "</table>";
                        document.getElementById('procedureCodeOutput').innerHTML = table;
                        if (ClaimType == 'Institutional') {
                            document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = '';
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "hidden";
                            document.getElementById('divProcCodeLoader1').innerHTML = "";
                        }
                        if (ClaimType == 'dental') {
                            document.getElementById('<%= lblDentalProcMessage.ClientID%>').innerHTML = '';
                            document.getElementById('divProcCodeLoaderDental').innerHTML = "";
                        }
                        if (ClaimType == 'Professional') {
                            document.getElementById('<%= lblProfProcMessage.ClientID%>').innerHTML = '';
                            document.getElementById('divProcCodeLoaderProfessional').innerHTML = "";
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfProcMessage').style.visibility = "hidden";
                        }
                        $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeSubmitPriorAuthSearchProc").show();
                    }
                    else {
                        if (ClaimType == 'Institutional') {
                            $("[id*=txtLnProcedureCode]").val(result[0].ProcedureCode);
                            document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = '';
                            document.getElementById('divProcCodeLoader1').innerHTML = "";
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "hidden";
                        }
                        if (ClaimType == 'dental') {
                            $("[id*=txtDentalSDProcCode]").val(result[0].ProcedureCode);
                            document.getElementById('<%= lblDentalProcMessage.ClientID%>').innerHTML = '';
                            document.getElementById('divProcCodeLoaderDental').innerHTML = "";
                        }
                        if (ClaimType == 'Professional') {
                            $("[id*=txtProfessionalSDProcCode]").val(result[0].ProcedureCode);
                            document.getElementById('<%= lblProfProcMessage.ClientID%>').innerHTML = '';
                            document.getElementById('divProcCodeLoaderProfessional').innerHTML = "";
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfProcMessage').style.visibility = "hidden";
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(JSON.stringify(jqXHR));
                }
            });
        }
        else {

            if (ClaimType == 'Institutional') {

                var PAnumberValue = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_txtPANumber2");
                var isNonConvertedPA = false;
                if (PAnumberValue && PAnumberValue.value.includes("AUTH")) {
                    isNonConvertedPA = true;
                }

                var ddlAssignment = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlAssignment");
                var assignmentvalue = ddlAssignment.value;
                var assignmenttext = ddlAssignment.options[ddlAssignment.selectedIndex].text;

                var isPsychiatricInpatientAssignment = false;
                if (assignmenttext == 'Psychiatric Inpatient') {
                    isPsychiatricInpatientAssignment = true;
                }

                if (!isNonConvertedPA && !isPsychiatricInpatientAssignment) {
                    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtLnProcedureCode").style.display = 'block';
                    document.getElementById('divProcCodeLoader1').innerHTML = "";
                }
            }
            if (ClaimType == 'dental') {
                document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtDentalSDProcCode").style.display = 'block';
                document.getElementById('divProcCodeLoaderDental').innerHTML = "";
            }
            if (ClaimType == 'Professional') {

                document.getElementById('divProcCodeLoaderProfessional').innerHTML = "";
                document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtProfessionalSDProcCode").style.display = 'block';
            }
        }
    }

    /* Provider Notes Client Side*/

    function SaveProviderNotes() {
        var linkId = document.getElementById('<%=tempUniqueId.ClientID%>').value;
        document.getElementById('providerNotesloader').innerHTML = "<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
        var txtProviderNotes = $("#<%= txtProviderNotes.ClientID %>").first().val();
        if (txtProviderNotes == null || txtProviderNotes === "" || txtProviderNotes == undefined) {

        }
        else {
            var ClaimType = $(".rblClaimType input:checked").val();
            var patype;
            if (ClaimType == 'dental') {
                patype = 0;
            }
            else if (ClaimType == 'Professional') {
                patype = 2;
            }
            else {
                patype = 1;
            }
            const hdtxtMedicaidID = getParameterByName('MedicaidNumber');
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "POST",
                url: webApiPA + "SaveNote?note=" + encodeURIComponent(txtProviderNotes) + "&&medicaidID=" + hdtxtMedicaidID + "&&paType=" + patype,
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
                        alert('Something went wrong. Please contact system administrator....!');
                    }
                    else {

                        if (result.status == true) {

                            $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnProvNoteId").val(result.ID);
                            $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnProvNoteText").val(txtProviderNotes);

                            document.getElementById("btnprovNoteEdit").style.display = "block";
                            document.getElementById("btnprovNoteSave").style.display = "none";
                            document.getElementById("btnprovNoteDelete").style.display = "block";
                            document.getElementById('<%=txtProviderNotes.ClientID %>').disabled = true;
                            document.getElementById('providerNotesloader').innerHTML = '';
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    document.getElementById('providerNotesloader').innerHTML = '';
                    console.log(JSON.stringify(jqXHR));

                }
            });
            document.getElementById('providerNotesloader').innerHTML = '';
        }
        return false;
    }

    function EditUpdateProvidernote() {
        document.getElementById('providerNotesloader').innerHTML = "<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
        var text = document.getElementById("btnprovNoteEdit").value;
        if (text == 'Edit') {
            document.getElementById('btnprovNoteEdit').value = 'Update';
            document.getElementById('btnprovNoteEdit').title = 'Update';
            document.getElementById("btnprovNoteSave").style.display = "none";
            document.getElementById('<%=txtProviderNotes.ClientID %>').disabled = false;
        }
        //string noteID, string operation, string noteText, string paType
        else if (text == 'Update') {
            var ClaimType = $(".rblClaimType input:checked").val();
            var patype;
            if (ClaimType == 'dental') {
                patype = 0;
            }
            else if (ClaimType == 'Professional') {
                patype = 2;
            }
            else {
                patype = 1;
            }
            var noteID = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnProvNoteId").val();

            var opearion = 'Update';
            var txtProviderNotes = $("#<%= txtProviderNotes.ClientID %>").first().val();
            var userName = document.getElementById('<%=hdnUserName.ClientID%>').value;
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "POST",
                url: webApiPA + "UpdateNote?noteID=" + noteID + "&&operation=" + opearion + "&&noteText=" + encodeURIComponent(txtProviderNotes) + "&&paType=" + patype + "&&userName=" + userName,
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
                        alert('Something went wrong. Please contact system administrator....!');
                    }
                    else {
                        if (result.status == true) {

                            $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnProvNoteText").val(txtProviderNotes);
                            document.getElementById('providerNotesloader').innerHTML = '';
                            document.getElementById('btnprovNoteEdit').value = 'Edit';
                            document.getElementById('btnprovNoteEdit').title = 'Edit';
                            document.getElementById("btnprovNoteSave").style.display = "none";
                            document.getElementById('<%=txtProviderNotes.ClientID %>').disabled = true;
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    document.getElementById('providerNotesloader').innerHTML = '';
                    console.log(JSON.stringify(jqXHR));
                }
            });
        }
        document.getElementById('providerNotesloader').innerHTML = "";
    }

    function DeleteProviderNotes() {
        var ClaimType = $(".rblClaimType input:checked").val();
        var patype;
        if (ClaimType == 'dental') {
            patype = 0;
        }
        else if (ClaimType == 'Professional') {
            patype = 2;
        }
        else {
            patype = 1;
        }
        document.getElementById('providerNotesloader').innerHTML = "<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
        var noteID = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnProvNoteId").val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "DELETE",
            url: webApiPA + "DeleteNote?noteID=" + noteID + "&&paType=" + patype,
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
                    alert('Something went wrong. Please contact system administrator....!');
                }
                else {

                    if (result.status == true) {
                        $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnProvNoteId").val('');
                        $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnProvNoteText").val('');
                        document.getElementById('<%=txtProviderNotes.ClientID%>').value = '';
                        document.getElementById("btnprovNoteEdit").style.display = "none";
                        document.getElementById("btnprovNoteSave").style.display = "none";
                        document.getElementById("btnprovNoteDelete").style.display = "none";
                        document.getElementById('<%=txtProviderNotes.ClientID %>').disabled = false;
                        document.getElementById('providerNotesloader').innerHTML = '';
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                document.getElementById('providerNotesloader').innerHTML = '';
                console.log(JSON.stringify(jqXHR));

            }
        });
        document.getElementById('providerNotesloader').innerHTML = "";
    }

    function GetNotes() {
        var ClaimType = $(".rblClaimType input:checked").val();
        var patype;
        if (ClaimType == 'dental') {
            patype = 0;
        }
        else if (ClaimType == 'Professional') {
            patype = 2;
        }
        else {
            patype = 1;
        }
        var noteID = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnProvNoteId").val();
        var APIToken = $("[id*=hdnAccessToken]").val();

        $.ajax({
            type: "GET",
            url: webApiPA + "GetNotes?noteID=" + noteID + "&&paType=" + patype,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var notetext = result.Note;
                var noteId = result.noteId;

                if (notetext == null || notetext === "" || notetext == undefined) {

                    $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnProvNoteId").val('');
                    $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnProvNoteText").val('');
                    $("[id*=txtProviderNotes]").val('');
                    if (document.getElementById('<%=txtstatus2.ClientID%>').value == "Approved" ||
                        document.getElementById('<%=txtstatus2.ClientID%>').value == "Partially Approved") {
                        document.getElementById('<%=txtProviderNotes.ClientID %>').disabled = true;
                        document.getElementById("btnprovNoteEdit").style.display = "none";
                        document.getElementById("btnprovNoteDelete").style.display = "none";
                        document.getElementById("btnprovNoteSave").style.display = "none";
                    }
                    else {

                        var textnotes = document.getElementById('<%=txtProviderNotes.ClientID %>');
                        if (textnotes !== null && textnotes !== undefined) {
                            textnotes.disabled = false;
                        }

                        $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnProvNoteId").val('');
                        $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnProvNoteText").val('');

                        var btnprovNoteEdit = document.getElementById("btnprovNoteEdit");
                        if (btnprovNoteEdit !== null && btnprovNoteEdit !== undefined) {
                            btnprovNoteEdit.style.display = "none";
                        }

                        var btnprovNoteSave = document.getElementById("btnprovNoteSave");
                        if (btnprovNoteSave !== null && btnprovNoteSave !== undefined) {
                            btnprovNoteSave.style.display = "none";
                        }

                        var btnprovNoteDelete = document.getElementById("btnprovNoteDelete");
                        if (btnprovNoteDelete !== null && btnprovNoteDelete !== undefined) {
                            btnprovNoteDelete.style.display = "none";
                        }

                    }
                }
                else {

                    $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnProvNoteId").val(noteId);
                    $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnProvNoteText").val(notetext);

                    $("[id*=txtProviderNotes]").val(notetext);
                    document.getElementById('<%=txtProviderNotes.ClientID %>').disabled = true;
                    document.getElementById("btnprovNoteEdit").style.display = "block";
                    document.getElementById("btnprovNoteDelete").style.display = "block";
                    document.getElementById("btnprovNoteSave").style.display = "none";
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(JSON.stringify(jqXHR));

            }
        });
    }

    function visibleCancelPopUp() {
        try {
            $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mdlCancelPARequest1").show();
        }
        catch (err) {
            console.log(err);
        }
        return false;
    }

    function hideCancelPopUp() {
        try {
            $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mdlCancelPARequest1").hide();
        }
        catch (err) {
            console.log(err);
        }
        return false;
    }

    function addInstitutionalLine() {


        var rftxtServicecode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtServicecode");
        if (typeof (rftxtServicecode) != 'undefined' && rftxtServicecode != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtServicecode').style.display = "none";
        }
        var rfvddlServiceTypeCode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddlServiceTypeCode");
        if (typeof (rfvddlServiceTypeCode) != 'undefined' && rfvddlServiceTypeCode != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddlServiceTypeCode').style.display = "none";
        }
        var rfvtxtLnProcedureCode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtLnProcedureCode");
        if (typeof (rfvtxtLnProcedureCode) != 'undefined' && rfvtxtLnProcedureCode != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtLnProcedureCode').style.display = "none";
        }
        var rftxtRequestUnt = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtRequestUnt");
        if (typeof (rftxtRequestUnt) != 'undefined' && rftxtRequestUnt != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtRequestUnt').style.display = "none";
        }
        var rfvddLnRequestUnits = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddLnRequestUnits");
        if (typeof (rfvddLnRequestUnits) != 'undefined' && rfvddLnRequestUnits != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddLnRequestUnits').style.display = "none";
        }
        var rftxtReqFDOS = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtReqFDOS");
        if (typeof (rftxtReqFDOS) != 'undefined' && rftxtReqFDOS != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtReqFDOS').style.display = "none";
        }
        var rftxtReqTDOS = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtReqTDOS");
        if (typeof (rftxtReqTDOS) != 'undefined' && rftxtReqTDOS != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtReqTDOS').style.display = "none";
        }

        document.getElementById("dvSDInstitutionalAddparent").style.height = "auto";
        document.getElementById("dvSDInstitutionalAddparent").style.overflow = "visible";
        document.getElementById("dvSDInstitutionalAddparent").style.visibility = "inherit";
        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "hidden";

        var ClaimType = $(".rblClaimType input:checked").val();
        var ddlServiceTypeCode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlServiceTypeCode");

        for (var i = 0; i < ddlServiceTypeCode.options.length; i++) {
            if (ddlServiceTypeCode.options[i].text == '--- Please select ---') {
                ddlServiceTypeCode.options[i].selected = true;
            }
        }

        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddLnRequestUnits").options[0].selected = true;
        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddLnLevelOfCare").options[0].selected = true;

        var ddlAssignment = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlAssignment");
      <%--  document.getElementById('<%=btnServDetailsUpdateAdd.ClientID%>').value = "Add";--%>

        document.getElementById('btnServDetailsUpdateAdd').value = "Add";

        $("[id*=txtServicecode]").val('');

        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_pnlline").style.visibility = "block";
        var ddlAssignment = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlAssignment");

        var assignmentvalue = ddlAssignment.value;
        var assignmenttext = ddlAssignment.options[ddlAssignment.selectedIndex].text;

        if (assignmentvalue == "34" || assignmentvalue == "35" || assignmentvalue == "55"
            || assignmentvalue == "59" || assignmentvalue == "37") {
            document.getElementById('<%= btnServiceDetailAdd1.ClientID%>').disabled = false;
        }
        if (ClaimType == 'Institutional') {
            if (assignmentvalue == "34") {
                for (var i = 0; i < ddlServiceTypeCode.options.length; i++) {
                    if (ddlServiceTypeCode.options[i].text == 'ICD 10 Procedure') {
                        ddlServiceTypeCode.options[i].selected = true;
                    }
                }
            }
            else if (assignmentvalue == "35" || assignmentvalue == "55") {
                for (var i = 0; i < ddlServiceTypeCode.options.length; i++) {
                    if (ddlServiceTypeCode.options[i].text == 'HCPCS') {
                        ddlServiceTypeCode.options[i].selected = true;
                    }
                }
            }
        }

        if (ClaimType == "Institutional" && assignmentvalue == "59") {
            var txtfacilityType = $("#<%= txtfacilityType.ClientID %>").first().val();
            if (txtfacilityType.includes("011")) {
                for (var i = 0; i < ddlServiceTypeCode.options.length; i++) {
                    if (ddlServiceTypeCode.options[i].text == 'ICD 10 Procedure') {
                        ddlServiceTypeCode.options[i].selected = true;
                    }
                }

            }
            else if (txtfacilityType.includes("013")) {
                for (var i = 0; i < ddlServiceTypeCode.options.length; i++) {
                    if (ddlServiceTypeCode.options[i].text == 'HCPCS') {
                        ddlServiceTypeCode.options[i].selected = true;
                    }
                }
            }
        }
        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_btnUpdate').style.display = "inline";

        return false;
    }

    function GetServiceDetailsInstitutional() {

        var Medicaid = getParameterByName('MedicaidNumber');

        var patype;
        var ClaimType = $(".rblClaimType input:checked").val();
        if (ClaimType == 'dental') {
            patype = 0;
        }
        else if (ClaimType == 'Professional') {
            patype = 2;
        }
        else {
            patype = 1;
        }
        var linkId = document.getElementById('<%=tempUniqueId.ClientID%>').value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetServiceDetailsInstitutionals?paType=" + patype + "&&medicaidID=" + Medicaid + "&&linkId=" + linkId,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var diagnosisCodeTypes = result.diagnosisCodeTypes;
                priorAuthDiagnosisList = result.priorAuthDiagnoses;

                if (result.length > 0) {

                    document.getElementById('divServDetailsNoData').innerHTML = '';
                    var table = '';
                    if ($('#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlAssignment').val() == '37') {
                        table = "<table class='gridview' cellspacing='0' align='Left' rules='rows' border='1' style='width:100%;border-collapse:collapse;margin-right: 0px; margin-top: auto;'><tbody><tr class='gridViewHeader'><th scope='col'>Service<br> Line</th><th scope='col'>Revenue<br> Code</th><th scope='col'>Procedure<br>Code Type</th><th scope='col'>Procedure<br> Code</th><th scope='col'><span class='text-red'>*</span>Requested<br> Units</th><th scope='col'>Requested<br> Dollars</th><th scope='col'><span class='text-red'>*</span>Requested<br> FDOS</th><th scope='col'><span class='text-red'>*</span>Requested<br> TDOS</th><th scope='col'>Status</th><th scope='col'>&nbsp;</th><th scope='col'>&nbsp;</th></tr>";
                    }
                    else {
                        table = "<table class='gridview' cellspacing='0' align='Left' rules='rows' border='1' style='width:100%;border-collapse:collapse;margin-right: 0px; margin-top: auto;'><tbody><tr class='gridViewHeader'><th scope='col'>Service<br> Line</th><th scope='col'>Revenue<br> Code</th><th scope='col'><span class='text-red'>*</span>Procedure<br>Code Type</th><th scope='col'><span class='text-red'>*</span>Procedure<br> Code</th><th scope='col'><span class='text-red'>*</span>Requested<br> Units</th><th scope='col'>Requested<br> Dollars</th><th scope='col'><span class='text-red'>*</span>Requested<br> FDOS</th><th scope='col'><span class='text-red'>*</span>Requested<br> TDOS</th><th scope='col'>Status</th><th scope='col'>&nbsp;</th><th scope='col'>&nbsp;</th></tr>";
                    }


                    for (var i = 0; i < result.length; i++) {

                        var PRIOR_AUTH_SERVICE_DETAIL_ID = result[i].PRIOR_AUTH_SERVICE_DETAIL_ID;
                        var PRIOR_AUTH_SERVICE_REVENUE_CODE = result[i].PRIOR_AUTH_SERVICE_REVENUE_CODE;
                        var PRIOR_AUTH_SERVICE_CODE_TYPE_ID = result[i].PRIOR_AUTH_SERVICE_CODE_TYPE_ID;
                        var PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE = result[i].PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE;
                        //   var PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE = result.d[i].PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE;

                        let PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE = (result[i].PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE != null && result[i].PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE > 0) ? result[i].PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE : '';

                        var PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS = result[i].PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS;
                        var PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS = result[i].PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS;
                        var PRIOR_AUTH_STATUS_ID = result[i].PRIOR_AUTH_STATUS_ID;
                        var PRIOR_AUTH_SERVICE_CODE_TYPE_TEXT = result[i].PRIOR_AUTH_SERVICE_CODE_TYPE_TEXT;
                        var PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS = (result[i].PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS != null && result[i].PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS > 0) ? result[i].PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS : '';
                        let PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR = (result[i].PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR != null && result[i].PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR > 0) ? result[i].PRIOR_AUTH_SERVICE_DETAIL_REQUEST_DOLLAR : '';
                        var sequence = i + 1;

                        const fdos = PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS;

                        if (fdos != '' && fdos != null && fdos != undefined && fdos.length > 0) {
                            const D = new Date(fdos);
                            var fdosDate = getFormattedDate(D);
                            var priorAuthfdosDate = fdosDate;
                        }
                        else {
                            var priorAuthfdosDate = '';
                        }

                        const tdos = PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS;

                        if (tdos != '' && tdos != null && tdos != undefined && tdos.length > 0) {
                            const D = new Date(tdos);
                            var tdosDate = getFormattedDate(D);
                            var priorAuthtdosDate = tdosDate;
                        }
                        else {
                            var priorAuthtdosDate = '';
                        }


                        var status = '';

                        if (PRIOR_AUTH_STATUS_ID == '0') {
                            status = 'Pend';
                        }
                        else if (PRIOR_AUTH_STATUS_ID == '2') {
                            status = 'Approved';
                        }
                        else if (PRIOR_AUTH_STATUS_ID == '3') {
                            status = 'Denied';
                        }
                        else if (PRIOR_AUTH_STATUS_ID == '4') {
                            status = 'Partially Approved';
                        }
                        else if (PRIOR_AUTH_STATUS_ID == '5') {
                            status = 'InProcess';
                        }
                        else if (PRIOR_AUTH_STATUS_ID == '6') {
                            status = 'Pending Addtl Info';
                        }
                        else if (PRIOR_AUTH_STATUS_ID == '7') {
                            status = 'Closed';
                        }
                        else if (PRIOR_AUTH_STATUS_ID == '1') {
                            status = 'Submission Pending';
                        }
                        else {
                            status = PRIOR_AUTH_STATUS_ID;
                        }

                        if (PRIOR_AUTH_SERVICE_CODE_TYPE_TEXT == null) PRIOR_AUTH_SERVICE_CODE_TYPE_TEXT = '';
                        if (document.getElementById('<%=txtstatus2.ClientID%>').value == "Approved" ||
                            document.getElementById('<%=txtstatus2.ClientID%>').value == "Closed") {
                            table = table + "<tr class='gridViewRow'><td><a class='gridLink' onclick='return EditInstitutionalServiceDetails(\"" + PRIOR_AUTH_SERVICE_DETAIL_ID + "\")'>" + sequence + "</a></td><td><input type='text' value='" + PRIOR_AUTH_SERVICE_REVENUE_CODE + "' readonly='readonly'></td><td><span title='Line'>" + PRIOR_AUTH_SERVICE_CODE_TYPE_TEXT + "</span></td><td><input type='text' value='" + PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE + "' readonly='readonly' style='width:200px;background-color:white'></td><td><input type='text' value='" + PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS + "' readonly='readonly' style='width:200px;background-color:white'></td><td><input type='text' value='" + PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE + "' readonly='readonly' style='width:200px;background-color:white'></td><td><input type='text' value='" + priorAuthfdosDate + "' readonly='readonly' style='width:200px;background-color:white'></td><td><input type='text' value='" + priorAuthtdosDate + "' readonly='readonly' style='width:200px;background-color:white'></td><td><input type='text' value='" + status + "' readonly='readonly' disabled='disabled' style='padding:0 5px;' class='aspNetDisabled input-disable' style='width:200px;background-color:white'></td></tr><tr class='gridViewFooter' style='background-color:White;'><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style='white-space:nowrap;'>&nbsp;</td></tr>";
                        }
                        else {
                            table = table + "<tr class='gridViewRow'><td><a class='gridLink' onclick='return EditInstitutionalServiceDetails(\"" + PRIOR_AUTH_SERVICE_DETAIL_ID + "\")'>" + sequence + "</a></td><td><input type='text' value='" + PRIOR_AUTH_SERVICE_REVENUE_CODE + "' readonly='readonly'></td><td><span title='Line'>" + PRIOR_AUTH_SERVICE_CODE_TYPE_TEXT + "</span></td><td><input type='text' value='" + PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE + "' readonly='readonly' style='width:200px;background-color:white'></td><td><input type='text' value='" + PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS + "' readonly='readonly' style='width:200px;background-color:white'></td><td><input type='text' value='" + PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE + "' readonly='readonly' style='width:200px;background-color:white'></td><td><input type='text' value='" + priorAuthfdosDate + "' readonly='readonly' style='width:200px;background-color:white'></td><td><input type='text' value='" + priorAuthtdosDate + "' readonly='readonly' style='width:200px;background-color:white'></td><td><input type='text' value='" + status + "' readonly='readonly' disabled='disabled' style='padding:0 5px;' class='aspNetDisabled input-disable' style='width:200px;background-color:white'></td><td><input type='button' value='Edit' onclick='EditInstitutionalServiceDetails(\"" + PRIOR_AUTH_SERVICE_DETAIL_ID + "\");' class='btn btn-primary' sytle='margin-left:10px' style='font-weight:bold;width:90px;'></td><td><input type='button' value='Delete' onclick='DeleteInstitutionalServiceDetails(\"" + PRIOR_AUTH_SERVICE_DETAIL_ID + "\");' class='btn btn-danger' sytle='margin-left:10px' style='font-weight:bold;width:90px;'></td></tr><tr class='gridViewFooter' style='background-color:White;'><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td style='white-space:nowrap;'>&nbsp;</td></tr>";
                        }
                    }

                    table = table + "</tbody></table>";

                    document.getElementById('institutionalServiceDetails').innerHTML = table;

                    document.getElementById("dvSDInstitutionalAddparent").style.height = "0";
                    document.getElementById("dvSDInstitutionalAddparent").style.overflow = "hidden";
                    document.getElementById("dvSDInstitutionalAddparent").style.visibility = "hidden";
                }
                else {

                    var dvSDInstitutionalAddparent = document.getElementById("dvSDInstitutionalAddparent");
                    if (dvSDInstitutionalAddparent !== null && dvSDInstitutionalAddparent !== undefined) {
                        dvSDInstitutionalAddparent.style.height = "0";
                        dvSDInstitutionalAddparent.style.overflow = "hidden";
                        dvSDInstitutionalAddparent.style.visibility = "hidden";
                    }
                    var institutionalServiceDetails = document.getElementById("institutionalServiceDetails");
                    if (institutionalServiceDetails !== null && institutionalServiceDetails !== undefined) {
                        institutionalServiceDetails.innerHTML = '';
                    }
                    var divServDetailsNoData = document.getElementById("divServDetailsNoData");
                    if (divServDetailsNoData !== null && divServDetailsNoData !== undefined) {
                        divServDetailsNoData.innerHTML = ' <h3 style="color:green"> No Service details found. Please Click on Add button to register. </h1>';
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                // alert("Something went wrong while binding Institutional service details. Please contact system administrator...!");
                console.log("Something went wrong while binding Institutional service details. Please contact system administrator...!");
            }
        });
    }

    function DeleteInstitutionalServiceDetails(Id) {
        var ClaimType = $(".rblClaimType input:checked").val();
        var result = confirm("Are you sure you want to delete?");
        if (result) {
            if (Id != '' && Id != null && Id != undefined) {
                var linkId = document.getElementById('<%=tempUniqueId.ClientID%>').value;
                var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "DELETE",
                    url: webApiPA + "DeleteServiceDetails?lineNumber=" + Id + "&&paType=" + ClaimType + "&&linkId=" + linkId,
                    headers: {
                        "Access-Control-Allow-Origin": "*",
                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                        "Authorization": "Bearer " + APIToken
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        GetServiceDetailsInstitutional();
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        /*  alert('Something went wrong while deleting the service details line item. Please Contact system administrator..!')*/
                        console.log("Something went wrong while deleting the service details line item. Please Contact system administrator..!");
                    }
                });
            }
        }
    }

    function EditInstitutionalServiceDetails(Id) {

        cancelInstitutionalLineAdd();

        var rftxtServicecode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtServicecode");
        if (typeof (rftxtServicecode) != 'undefined' && rftxtServicecode != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtServicecode').style.display = "none";
        }
        var rfvddlServiceTypeCode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddlServiceTypeCode");
        if (typeof (rfvddlServiceTypeCode) != 'undefined' && rfvddlServiceTypeCode != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddlServiceTypeCode').style.display = "none";
        }
        var rfvtxtLnProcedureCode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtLnProcedureCode");
        if (typeof (rfvtxtLnProcedureCode) != 'undefined' && rfvtxtLnProcedureCode != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtLnProcedureCode').style.display = "none";
        }
        var rftxtRequestUnt = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtRequestUnt");
        if (typeof (rftxtRequestUnt) != 'undefined' && rftxtRequestUnt != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtRequestUnt').style.display = "none";
        }
        var rfvddLnRequestUnits = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddLnRequestUnits");
        if (typeof (rfvddLnRequestUnits) != 'undefined' && rfvddLnRequestUnits != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddLnRequestUnits').style.display = "none";
        }
        var rftxtReqFDOS = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtReqFDOS");
        if (typeof (rftxtReqFDOS) != 'undefined' && rftxtReqFDOS != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtReqFDOS').style.display = "none";
        }
        var rftxtReqTDOS = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtReqTDOS");
        if (typeof (rftxtReqTDOS) != 'undefined' && rftxtReqTDOS != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rftxtReqTDOS').style.display = "none";
        }

        var Medicaid = getParameterByName('MedicaidNumber');

        var ClaimType = $(".rblClaimType input:checked").val();
        var patype;
        if (ClaimType == 'dental') {
            patype = 0;
        }
        else if (ClaimType == 'Professional') {
            patype = 2;
        }
        else {
            patype = 1;
        }
        var linkId = document.getElementById('<%=tempUniqueId.ClientID%>').value;
        var ddlServiceTypeCode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlServiceTypeCode");

        for (var i = 0; i < ddlServiceTypeCode.options.length; i++) {
            if (ddlServiceTypeCode.options[i].text == '--- Please select ---') {
                ddlServiceTypeCode.options[i].selected = true;
            }
        }

        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddLnRequestUnits").options[0].selected = true;
        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddLnLevelOfCare").options[0].selected = true;

        var ddlAssignment = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlAssignment");
       <%-- document.getElementById('<%=btnServDetailsUpdateAdd.ClientID%>').value = "Update";--%>
        document.getElementById('btnServDetailsUpdateAdd').value = "Update";
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetServiceDetailsInstitutional?lineNumber=" + Id + "&&MedicaidId=" + Medicaid + "&&patype=" + patype + "&&linkId=" + linkId,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                /*  if (result.d.length > 0) {*/
                $("[id*=txtServicecode]").val(result.PRIOR_AUTH_SERVICE_REVENUE_CODE);


                if (result.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS != null && result.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS > 0) {
                    $("[id*=txtRequestUnt]").val(result.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS);
                }
                else {
                    $("[id*=txtRequestUnt]").val('');
                }

                if (result.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS != null && result.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS > 0) {
                    $("[id*=txtAuthorizedUnits]").val(result.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS);
                }
                else {
                    $("[id*=txtAuthorizedUnits]").val('');
                }


                if (result.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE != null && result.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE > 0) {
                    $("[id*=txtRequestedDollars]").val(result.PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE);
                }
                else {
                    $("[id*=txtRequestedDollars]").val('');
                }


                if (result.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR != null && result.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR > 0) {
                    $("[id*=txtAuthorizedDollars]").val(result.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_DOLLAR);
                }
                else {
                    $("[id*=txtAuthorizedDollars]").val('');
                }



                const dateStringFDOS = result.PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_FDOS;

                if (dateStringFDOS != '' && dateStringFDOS != null && dateStringFDOS != undefined && dateStringFDOS.length > 0) {
                    const D = new Date(dateStringFDOS);
                    var FDOSDate = getFormattedDate(D);
                    $("[id*=txtReqFDOS]").val(FDOSDate);
                }
                else {
                    $("[id*=txtReqFDOS]").val('');
                }

                const dateStringTDOS = result.PRIOR_AUTH_SERVICE_DETAIL_REQUESTED_TDOS;

                if (dateStringTDOS != '' && dateStringTDOS != null && dateStringTDOS != undefined && dateStringTDOS.length > 0) {
                    const D = new Date(dateStringTDOS);
                    var TDOSDate = getFormattedDate(D);
                    $("[id*=txtReqTDOS]").val(TDOSDate);
                }
                else {
                    $("[id*=txtReqTDOS]").val('');
                }

                const dateStringAFDOS = result.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_FROM_DOS;
                // alert(dateStringAFDOS);
                if (dateStringAFDOS != '' && dateStringAFDOS != null && dateStringAFDOS != undefined && dateStringAFDOS.length > 0) {
                    const D = new Date(dateStringAFDOS);
                    var AFDOSDate = getFormattedDate(D);
                    $("[id*=txtAuthorizedFromDOS]").val(AFDOSDate);
                }
                else {
                    $("[id*=txtAuthorizedFromDOS]").val('');
                }

                const dateStringATDOS = result.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_TO_DOS;
                // alert(dateStringATDOS);
                if (dateStringATDOS != '' && dateStringATDOS != null && dateStringATDOS != undefined && dateStringATDOS.length > 0) {
                    const D = new Date(dateStringATDOS);
                    var ATDOSDate = getFormattedDate(D);
                    $("[id*=txtAuthorizedToDOS]").val(ATDOSDate);
                }
                else {
                    $("[id*=txtAuthorizedToDOS]").val('');
                }


                $("[id*=txtLnProcedureCode]").val(result.PRIOR_AUTH_SERVICE_PROCEDUCRE_CODE);

                if (result.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS != null && result.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS > 0) {
                    $("[id*=txtAuthorizedUnits]").val(result.PRIOR_AUTH_SERVICE_DETAIL_AUTHORIZED_UNITS);
                }
                else {
                    $("[id*=txtAuthorizedUnits]").val('');
                }

                if (result.PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS != null && result.PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS > 0) {
                    $("[id*=txtLnRemainingUnits]").val(result.PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS);
                }
                else {
                    $("[id*=txtLnRemainingUnits]").val('');
                }


                $("[id*=txtLnprocCodeDesc]").val(result.PRIOR_AUTH_SERVICE_DETAIL_PROCEDURECODE_DESC);
                $("[id*=txtLnProviderServiceNote]").val(result.PRIOR_AUTH_SERVICE_DETAIL_PROVIDERSERVICE_NOTE);


                if (result.PRIOR_AUTH_STATUS_ID != 'Approved') {
                    $("[id*=txtAuthorizedFromDOS]").val('');
                    $("[id*=txtAuthorizedToDOS]").val('');
                    $("[id*=txtLnRemainingUnits]").val('');
                    $("[id*=txtAuthorizedDollars]").val('');
                    $("[id*=txtAuthorizedUnits]").val('');

                }

                $("[id*=txtLnServiceTrackingNo]").val(result.PRIOR_AUTH_SERVICE_DETAIL_SERVICE_TRACKING_NO);
                $("[id*=txtLnStatus]").val(result.PRIOR_AUTH_STATUS_ID);

                $("[id*=hdLineNumber]").val(result.PRIOR_AUTH_SERVICE_DETAIL_ID);


                var ddlServiceTypeCode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlServiceTypeCode");
                var serviceTypeCode = result.PRIOR_AUTH_SERVICE_CODE_TYPE_ID;
                if (serviceTypeCode != '') {
                    for (var i = 0; i < ddlServiceTypeCode.options.length; i++) {
                        if (ddlServiceTypeCode.options[i].value == serviceTypeCode) {
                            ddlServiceTypeCode.options[i].selected = true;
                        }
                    }
                }

                var ddLnRequestUnits = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddLnRequestUnits");
                var requestedUnitsID = result.PRIOR_AUTH_REQUESTED_UNITS_ID;
                if (requestedUnitsID != '') {
                    for (var i = 0; i < ddLnRequestUnits.options.length; i++) {
                        if (ddLnRequestUnits.options[i].value == requestedUnitsID) {
                            ddLnRequestUnits.options[i].selected = true;
                        }
                    }
                }

                var ddLnLevelOfCare = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddLnLevelOfCare");
                var levelOfCare = result.PRIOR_AUTH_LEVEL_CARE_ID;
                if (levelOfCare != '') {
                    for (var i = 0; i < ddLnLevelOfCare.options.length; i++) {
                        if (ddLnLevelOfCare.options[i].value == levelOfCare) {
                            ddLnLevelOfCare.options[i].selected = true;
                        }
                    }
                }
                /*}*/

                document.getElementById("dvSDInstitutionalAddparent").style.height = "auto";
                document.getElementById("dvSDInstitutionalAddparent").style.overflow = "visible";
                document.getElementById("dvSDInstitutionalAddparent").style.visibility = "inherit";
                //document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_btnUpdate').style.display = "inline";
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(JSON.stringify(jqXHR));
            }
        });

        if (document.getElementById('<%=txtstatus2.ClientID%>').value == "Approved" ||
            document.getElementById('<%=txtstatus2.ClientID%>').value == "Closed") {
            document.getElementById('btnServDetailsUpdateAdd').style.visibility = 'hidden';
            document.getElementById('btnServDetailsUpdateAdd').disabled = true;
        }

    }

    function SaveInstitutionalServiceDetails() {

        if (Page_ClientValidate('valServiceDetails_Institutional')) {

            if (document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').innerText != "")
                return false;

            var ddlServiceTypeCode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlServiceTypeCode");
            var procCodeType = ddlServiceTypeCode.value;
            var procCodeTypeText = ddlServiceTypeCode.options[ddlServiceTypeCode.selectedIndex].text;

            if (procCodeTypeText == '--- Please select ---' && $('#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlAssignment').val() != '37') {
                document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = 'Please select Procedure Code Type.';
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "visible";
                document.getElementById('divProcCodeLoader1').innerHTML = "";
            }
            else {

                var ClaimType = $(".rblClaimType input:checked").val();
                document.getElementById('divProcCodeLoader1').innerHTML = "<img src='../Images/loader.gif' style='height: 38px;width: 35px;'>";
                document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtLnProcedureCode").style.display = 'none';
                document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = '';

                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "hidden";
                txtLnProcedureCode = $("#<%= txtLnProcedureCode.ClientID %>").first().val();

                txtLnProcedureCode = txtLnProcedureCode.replace(/(^\s+|\s+$)/g, '');
                if (txtLnProcedureCode != '' && txtLnProcedureCode != null && txtLnProcedureCode != undefined) {
                    var APIToken = $("[id*=hdnAccessToken]").val();
                    $.ajax({
                        type: "GET",
                        url: webApiPA + "GetProcedureCodeData_InstitutionalDynamic?procCode=" + txtLnProcedureCode + "&&procCodeDesc=" + procCodeTypeText,
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
                                if (ClaimType == 'Institutional') {
                                    document.getElementById('divProcCodeLoader1').innerHTML = "";
                                    document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = 'Procedure Code is invalid for selected Procedure Type';
                                    document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "visible";
                                    //alert("Result: " + document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML);
                                }
                            }
                            else {

                                SaveServiceDetails_Institutional();
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            console.log(JSON.stringify(jqXHR));
                        }
                    });
                }
                else if ($('#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlAssignment').val() == '37') {
                    SaveServiceDetails_Institutional();
                }
            }
        }
        else {

            return false;
        }
    }

    function SaveServiceDetails_Institutional() {

        //alert("Error :" + document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').innerHTML)
        document.getElementById('divProcCodeLoader1').innerHTML = "";
        if (document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').innerText != "") {


            return false;
        }
    <%--  var operation = document.getElementById('<%=btnServDetailsUpdateAdd.ClientID%>').value;--%>
        var operation = document.getElementById('btnServDetailsUpdateAdd').value;
        var lineNumber = $("[id*=hdLineNumber]").val();

        var Medicaid = getParameterByName('MedicaidNumber');
        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsDatesMessage').style.visibility = "hidden";

        var reqUnitsError = document.getElementById('<%=requnitsError.ClientID%>')

        document.getElementById('<%= requnitsError.ClientID%>').style.visibility = "hidden";

        var txtReqFDOS = $("#<%= txtReqFDOS.ClientID %>").first().val();
        var txtReqTDOS = $("#<%= txtReqTDOS.ClientID %>").first().val();

        var txtAuthorizedFromDOS = $("#<%= txtAuthorizedFromDOS.ClientID %>").first().val();
        var txtAuthorizedToDOS = $("#<%= txtAuthorizedToDOS.ClientID %>").first().val();
        var txtRequestUnt = $("#<%= txtRequestUnt.ClientID %>").first().val();


        var message = validateFDOSTDOSDates(txtReqFDOS, txtReqTDOS);

        if (message != '') {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsDatesMessage').style.visibility = "visible";
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsDatesMessage').innerHTML = message;
            return false;
        }
        else {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsDatesMessage').style.visibility = "hidden";
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsDatesMessage').innerHTML = '';
        }

        if (txtAuthorizedFromDOS != '' && txtAuthorizedToDOS != '') {
            var authmessage = validateFDOSTDOSDates(txtAuthorizedFromDOS, txtAuthorizedToDOS);
        }

        if (parseFloat(txtRequestUnt) < 1) {
            document.getElementById('<%= requnitsError.ClientID%>').style.visibility = "visible";
            document.getElementById('<%= requnitsError.ClientID%>').innerHTML = "Invalid requested Units";
            return false;
        }
        var procedureCode = $("#<%= txtLnProcedureCode.ClientID %>").first().val();
        var txtModifier1 = '';
        var txtModifier2 = '';
        var txtModifier3 = '';
        var txtModifier4 = '';
        var claimsorPA = "PA";
        var claimORPAType = "Institutional";
        validateProcedureCodeModifiers(procedureCode, txtModifier1, txtModifier2, txtModifier3, txtModifier4, claimsorPA, claimORPAType);
        if (validateField == false) {
            alert('validated fields are false');
            return false;
        }

        var servDetails = prepareInstitutionalServDetails();
        var imputData = JSON.stringify(servDetails);
        var ClaimType = $(".rblClaimType input:checked").val();
        var patype;
        if (ClaimType == 'dental') {
            patype = 0;
        }
        else if (ClaimType == 'Professional') {
            patype = 2;
        }
        else {
            patype = 1;
        }
        var linkId = document.getElementById('<%=tempUniqueId.ClientID%>').value;
        var userName = document.getElementById('<%=hdnUserName.ClientID%>').value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "POST",
            url: webApiPA + "SaveInstitutionalServiceDetails?codeType=" + servDetails.codeTypeId + "&&procedureCode=" + servDetails.procedureCode + "&&procCodeDesc=" + servDetails.procCodeDesc + "&&providerServiceNote=" + servDetails.providerServiceNote + "&&revenueCode=" + servDetails.revenueCode + "&&levelCareID=" + servDetails.levelofCareId + "&&reqUnits=" + servDetails.reqUnits + "&&unitMeasure=" + servDetails.unitMeasurementId + "&&requnitsfee=" + servDetails.reqUnitsFee + "&&fdos=" + servDetails.txtReqFDOS + "&&tdos=" + servDetails.txtReqTDOS + "&&status=" + servDetails.status + "&&MedicaidId=" + servDetails.Medicaid + "&&trackingNo=" + servDetails.serviceTrackingNo + "&&codeTypeText=" + servDetails.codeTypeText + "&&operation=" + operation + "&&lineNumber=" + lineNumber + "&&patype=" + patype + "&&linkId=" + linkId + "&&userName=" + userName,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                GetServiceDetailsInstitutional();
                cancelInstitutionalLineAdd();
                //OHPNM-11929: UPDATE PA button is not showing after editing service detail panel
                try {
                    if (document.getElementById('<%=txtstatus2.ClientID%>').value == "Pend" || document.getElementById('<%=txtstatus2.ClientID%>').value == "InProcess" || document.getElementById('<%=txtstatus2.ClientID%>').value == "Pending Addtl Info") {
                        document.getElementById('<%=btnUpdate.ClientID %>').style.visibility = "visible";
                        document.getElementById('<%=btnUpdate.ClientID %>').style.display = "inline-block";
                        document.getElementById('<%=btnCancelPARequest.ClientID %>').disabled = true;
                    }
                    else {
                        document.getElementById('<%=btnUpdate.ClientID %>').style.visibility = "hidden";
                        document.getElementById('<%=btnCancelPARequest.ClientID %>').disabled = false;
                    }
                }
                catch {
                    console.log('something went wrong while display Update PA Request...!');
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(JSON.stringify(jqXHR));
            }
        });
    }

    function cancelInstitutionalLineAdd() {

        document.getElementById("dvSDInstitutionalAddparent").style.height = "0";
        document.getElementById("dvSDInstitutionalAddparent").style.overflow = "hidden";
        document.getElementById("dvSDInstitutionalAddparent").style.visibility = "hidden";

        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddLnRequestUnits").options[0].selected = true;
        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddLnLevelOfCare").options[0].selected = true;
        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlServiceTypeCode").options[0].selected = true;

        $("[id*=txtServicecode]").val('');
        $("[id*=txtRequestUnt]").val('');
        $("[id*=txtAuthorizedUnits]").val('');
        $("[id*=txtRequestedDollars]").val('');
        $("[id*=txtAuthorizedDollars]").val('');
        $("[id*=txtReqFDOS]").val('');
        $("[id*=txtReqTDOS]").val('');
        $("[id*=txtAuthorizedFromDOS]").val('');
        $("[id*=txtAuthorizedToDOS]").val('');
        $("[id*=txtLnProcedureCode]").val('');
        $("[id*=txtRequestUnt]").val('');
        $("[id*=txtAuthorizedUnits]").val('');
        $("[id*=txtLnprocCodeDesc]").val('');
        $("[id*=txtLnProviderServiceNote]").val('');
        $("[id*=txtLnRemainingUnits]").val('');
        $("[id*=txtLnServiceTrackingNo]").val('');
        $("[id*=txtLnStatus]").val('');
        $("[id*=hdLineNumber]").val('');

    }

    function prepareInstitutionalServDetails() {

        var codeTypeId = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddlServiceTypeCode').value;
        var codetext = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddlServiceTypeCode').options[document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddlServiceTypeCode').selectedIndex].text;
        var procedureCode = $("#<%= txtLnProcedureCode.ClientID %>").first().val();
        var procCodeDesc = $("#<%= txtLnprocCodeDesc.ClientID %>").first().val();
        var providerServiceNote = $("#<%= txtLnProviderServiceNote.ClientID %>").first().val();
        var revenueCode = $("#<%= txtServicecode.ClientID %>").first().val();
        var levelofCareId = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddLnLevelOfCare').value;
        var reqUnits = $("#<%= txtRequestUnt.ClientID %>").first().val();
        var unitMeasurementId = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddLnRequestUnits').value;
        var reqUnitsFee = $("#<%= txtRequestedDollars.ClientID %>").first().val();

        var txtReqFDOS = $("#<%= txtReqFDOS.ClientID %>").first().val();
        var txtReqTDOS = $("#<%= txtReqTDOS.ClientID %>").first().val();
        var serviceTrackingNo = $("#<%= txtLnServiceTrackingNo.ClientID %>").first().val();
        var status = $("#<%= txtstatus2.ClientID %>").first().val();


        var servDetails = {
            procedureCode: procedureCode,
            procCodeDesc: procCodeDesc,
            providerServiceNote: providerServiceNote,
            revenueCode: revenueCode,
            levelofCareId: levelofCareId,
            reqUnits: reqUnits,
            unitMeasurementId: unitMeasurementId,
            reqUnitsFee: reqUnitsFee,
            txtReqFDOS: txtReqFDOS,
            txtReqTDOS: txtReqTDOS,
            serviceTrackingNo: serviceTrackingNo,
            status: status,
            codeTypeId: codeTypeId,
            codeTypeText: codetext
        }
        return servDetails;
    }


    function validateFDOSTDOSDates(txtReqFDOS, txtReqTDOS) {
        var fdos = new Date(txtReqFDOS);
        var tdos = new Date(txtReqTDOS);

        var message = '';
        if (fdos > tdos) {
            message = "Todos is must be greater than FromDOS";
        }
        var current = new Date();
        const currentDate48months = current.setMonth(-48);
        var date48months = new Date(currentDate48months);

        if (fdos < date48months) {
            message = "* System Allow up to 48 months back";
        }
        return message;
    }

    function addProfessionalLine() {


        var rfvtxtProfessionalSDProcCode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtProfessionalSDProcCode");
        if (typeof (rfvtxtProfessionalSDProcCode) != 'undefined' && rfvtxtProfessionalSDProcCode != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtProfessionalSDProcCode').style.display = "none";
        }
        var rfvtxtProfessionalReqUnits = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtProfessionalReqUnits");
        if (typeof (rfvtxtProfessionalReqUnits) != 'undefined' && rfvtxtProfessionalReqUnits != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtProfessionalReqUnits').style.display = "none";
        }
        var rfvddProffSDMeasurements = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddProffSDMeasurements");
        if (typeof (rfvddProffSDMeasurements) != 'undefined' && rfvddProffSDMeasurements != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddProffSDMeasurements').style.display = "none";
        }
        var RequiredFieldValidator7 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator7");
        if (typeof (RequiredFieldValidator7) != 'undefined' && RequiredFieldValidator7 != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator7').style.display = "none";
        }
        var RequiredFieldValidator2 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator2");
        if (typeof (RequiredFieldValidator2) != 'undefined' && RequiredFieldValidator2 != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator2').style.display = "none";
        }
        var RequiredFieldValidator9 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator9");
        if (typeof (RequiredFieldValidator9) != 'undefined' && RequiredFieldValidator9 != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator9').style.display = "none";
        }
        var RequiredRangeValidator1 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredRangeValidator1");
        if (typeof (RequiredRangeValidator1) != 'undefined' && RequiredRangeValidator1 != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredRangeValidator1').style.display = "none";
        }


        document.getElementById("dvSDProfessionalAddparent").style.height = "auto";
        document.getElementById("dvSDProfessionalAddparent").style.overflow = "visible";
        document.getElementById("dvSDProfessionalAddparent").style.visibility = "visible";

        var ClaimType = $(".rblClaimType input:checked").val();

        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddProffSDMeasurements").options[0].selected = true;
        document.getElementById('btnServProfessionalUpdate').value = "Add";
        document.getElementById('btnServProfessionalUpdate').title = "Add";

        $("[id*=txtProfessionalSDProcCode]").val('');
        $("[id*=lblservProfessionalLineNumber]").val('');

        $("[id*=txtModifier1]").val('');
        $("[id*=txtModifier2]").val('');
        $("[id*=txtModifier3]").val('');
        $("[id*=txtModifier4]").val('');

        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_pnlProfessionalLine").style.visibility = "block";
        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_btnUpdate').style.display = "inline";

        return false;
    }

    function cancelProfessionalLineAdd() {

        document.getElementById("dvSDProfessionalAddparent").style.height = "0";
        document.getElementById("dvSDProfessionalAddparent").style.overflow = "hidden";
        document.getElementById("dvSDProfessionalAddparent").style.visibility = "hidden";

        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddProffSDMeasurements").options[0].selected = true;

        $("[id*=txtProfessionalSDProcCode]").val('');
        $("[id*=txtProfessionalReqUnits]").val('');
        $("[id*=txtProfessionalAuthUnits]").val('');
        $("[id*=txtProfessionalProcCodeDescription]").val('');
        $("[id*=txtProfessionalProvServnote]").val('');
        $("[id*=txtProfessionalReqDollars]").val('');
        $("[id*=txtProfessionalAuthDollars]").val('');
        $("[id*=txtProfessionalReqFDOS]").val('');
        $("[id*=txtProfessionalAuthFDOS]").val('');
        $("[id*=txtProfessionalReqTDOS]").val('');
        $("[id*=txtProfessionalAuthTDOS]").val('');
        $("[id*=txtProfessionalRemainingUnits]").val('');
        $("[id*=txtProfessionalServTrackingNo]").val('');
        $("[id*=txtProfessionalServDetailsStatus]").val('');
    }

    function SaveProfessionalServiceDetails() {
        if (Page_ClientValidate('valServiceDetails_Professional')) {

            var operation = document.getElementById('btnServProfessionalUpdate').value;
            var lineNumber = '';

            var Medicaid = getParameterByName('MedicaidNumber');
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfDatesMessage').style.visibility = "hidden";

            var txtReqFDOS = $("#<%= txtProfessionalReqFDOS.ClientID %>").first().val();
            var txtReqTDOS = $("#<%= txtProfessionalReqTDOS.ClientID %>").first().val();

            var txtProfessionalAuthFDOS = $("#<%= txtProfessionalAuthFDOS.ClientID %>").first().val();
            var txtProfessionalAuthTDOS = $("#<%= txtProfessionalAuthTDOS.ClientID %>").first().val();
            var txtProfessionalReqUnits = $("#<%= txtProfessionalReqUnits.ClientID %>").first().val();

            if (operation == 'Update') {
                lineNumber = $("[id*=hdProfLineNumber]").val();
                opearion = 'Update';
            }
            else {
                lineNumber = '';
                opearion = 'Add';
            }

            var message = validateFDOSTDOSDates(txtReqFDOS, txtReqTDOS);

            if (message != '') {
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfDatesMessage').style.visibility = "visible";
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfDatesMessage').innerHTML = message;
                return false;
            }
            else {
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfDatesMessage').style.visibility = "hidden";
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfDatesMessage').innerHTML = '';
            }

            if (txtProfessionalAuthFDOS != '' && txtProfessionalAuthTDOS != '') {
                var authmessage = validateFDOSTDOSDates(txtProfessionalAuthFDOS, txtProfessionalAuthTDOS);
            }

            if (parseFloat(txtProfessionalReqUnits) < 1) {
                document.getElementById('<%= lblProfessionalReqUnitsError.ClientID%>').style.visibility = "visible";
                document.getElementById('<%= lblProfessionalReqUnitsError.ClientID%>').innerHTML = "Invalid requested Units";
                return false;
            }
            var txtProfessionalSDProcCode = $("#<%= txtProfessionalSDProcCode.ClientID %>").first().val();

            var txtModifier1 = $("#<%= txtModifier1.ClientID %>").first().val();
            var txtModifier2 = $("#<%= txtModifier2.ClientID %>").first().val();
            var txtModifier3 = $("#<%= txtModifier3.ClientID %>").first().val();
            var txtModifier4 = $("#<%= txtModifier4.ClientID %>").first().val();
            var claimsorPA = "PA";
            var claimORPAType = "Professional";
            validateProcedureCodeModifiers(txtProfessionalSDProcCode, txtModifier1, txtModifier2, txtModifier3, txtModifier4, claimsorPA, claimORPAType);
            if (validateField == false) {
                return false;
            }
            var ClaimType = $(".rblClaimType input:checked").val();

            var servDetails = prepareProfessionalServDetails();
            var imputData = JSON.stringify(servDetails);
            var procCodeDesc = '';
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetProcedureCodeData?procCode=" + servDetails.procedureCode + "&&procCodeDesc=" + procCodeDesc + "&&isTextChangesEvent=" + true,
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
                        if (ClaimType == 'Institutional') {
                            document.getElementById('divProcCodeLoader1').innerHTML = "";
                            document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = 'Procedure code is invalid';
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "visible";
                        }
                        if (ClaimType == 'dental') {
                            document.getElementById('divProcCodeLoaderDental').innerHTML = "";
                            document.getElementById('<%= lblDentalProcMessage.ClientID%>').innerHTML = 'Procedure code is invalid';
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalProcMessage').style.visibility = "visible";
                        }
                        if (ClaimType == 'Professional') {
                            document.getElementById('divProcCodeLoaderProfessional').innerHTML = "";
                            document.getElementById('<%= lblProfProcMessage.ClientID%>').innerHTML = 'Procedure code is invalid';
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfProcMessage').style.visibility = "visible";
                        }
                    }
                    else if (result.length > 1) {
                        $("[id*=txtHCPCSCode]").val(txtServicecode);
                        var table = "<table class='gridViewSmallFont FacilityTypeSearchResultGrid' cellspacing='0' rules='all' border='1' style='width:100%;border-collapse:collapse;'><tbody><tr class='gridViewHeader' style='color:Black;'><th scope='col'>Procedure Code</th><th scope='col'>Procedure Code Description</th></tr>";
                        for (var i = 0; i < result.length; i++) {
                            table = table + "<tr class='gridViewRow' valign='top'><td style='width:100px;'><a onClick='GetRevenueSelectedRow(\"" + result[i].ProcedureCode + "\"); return false;'>" + result[i].ProcedureCode + "</a></td><td>" + result[i].ProcedureCodeDesc + "</td></tr>";
                        };
                        table = table + "</table>";
                        document.getElementById('procedureCodeOutput').innerHTML = table;
                        if (ClaimType == 'Institutional') {
                            document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = '';
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "hidden";
                            document.getElementById('divProcCodeLoader1').innerHTML = "";
                        }
                        if (ClaimType == 'dental') {
                            document.getElementById('<%= lblDentalProcMessage.ClientID%>').innerHTML = '';
                            document.getElementById('divProcCodeLoaderDental').innerHTML = "";
                        }
                        if (ClaimType == 'Professional') {
                            document.getElementById('<%= lblProfProcMessage.ClientID%>').innerHTML = '';
                            document.getElementById('divProcCodeLoaderProfessional').innerHTML = "";
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfProcMessage').style.visibility = "hidden";
                        }
                        $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeSubmitPriorAuthSearchProc").show();
                    }
                    else {
                        if (ClaimType == 'Institutional') {
                            $("[id*=txtLnProcedureCode]").val(result[0].ProcedureCode);
                            document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = '';
                            document.getElementById('divProcCodeLoader1').innerHTML = "";
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "hidden";
                        }
                        if (ClaimType == 'dental') {
                            $("[id*=txtDentalSDProcCode]").val(result[0].ProcedureCode);
                            document.getElementById('<%= lblDentalProcMessage.ClientID%>').innerHTML = '';
                            document.getElementById('divProcCodeLoaderDental').innerHTML = "";
                        }
                        if (ClaimType == 'Professional') {
                            $("[id*=txtProfessionalSDProcCode]").val(result[0].ProcedureCode);
                            document.getElementById('<%= lblProfProcMessage.ClientID%>').innerHTML = '';
                            document.getElementById('divProcCodeLoaderProfessional').innerHTML = "";
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfProcMessage').style.visibility = "hidden";
                        }
                        var linkId = document.getElementById('<%=tempUniqueId.ClientID%>').value;
                        var ClaimType = $(".rblClaimType input:checked").val();
                        var patype;
                        if (ClaimType == 'dental') {
                            patype = 0;
                        }
                        else if (ClaimType == 'Professional') {
                            patype = 2;
                        }
                        else {
                            patype = 1;
                        }
                        var userName = document.getElementById('<%=hdnUserName.ClientID%>').value;
                        var APIToken = $("[id*=hdnAccessToken]").val();
                        $.ajax({
                            type: "POST",
                            url: webApiPA + "SaveProfessionalServiceDetails?procedureCode=" + servDetails.procedureCode + "&&procCodeDesc=" + servDetails.procCodeDesc + "&&providerServiceNote=" + servDetails.providerServiceNote + "&&reqUnits=" + servDetails.reqUnits + "&&remainUnits=" + servDetails.remainUnits + "&&requnitsfee=" + servDetails.requnitsfee + "&&fdos=" + servDetails.fdos + "&&tdos=" + servDetails.tdos + "&&status=" + servDetails.status + "&&unitMeasurement=" + servDetails.unitMeasurement + "&&modifier1=" + servDetails.modifier1 + "&&modifier2=" + servDetails.modifier2 + "&&modifier3=" + servDetails.modifier3 + "&&modifier4=" + servDetails.modifier4 + "&&MedicaidId=" + Medicaid + "&&serviceTrackingNo=" + servDetails.serviceTrackingNo + "&&operation=" + operation + "&&lineNumber=" + lineNumber + "&&patype=" + patype + "&&linkId=" + linkId + "&&userName=" + userName,
                            headers: {
                                "Access-Control-Allow-Origin": "*",
                                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                                "Authorization": "Bearer " + APIToken
                            },
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (result) {
                                GetServiceDetailsProfessional();
                                cancelProfessionalLineAdd();
                                //OHPNM-11929: UPDATE PA button is not showing after editing service detail panel
                                try {
                                    if (document.getElementById('<%=txtstatus2.ClientID%>').value == "Pend" || document.getElementById('<%=txtstatus2.ClientID%>').value == "InProcess" || document.getElementById('<%=txtstatus2.ClientID%>').value == "Pending Addtl Info") {
                                        document.getElementById('<%=btnUpdate.ClientID %>').style.visibility = "visible";
                                        document.getElementById('<%=btnUpdate.ClientID %>').style.display = "inline-block";
                                        document.getElementById('<%=btnCancelPARequest.ClientID %>').disabled = true;
                                    }
                                    else {
                                        document.getElementById('<%=btnUpdate.ClientID %>').style.visibility = "hidden";
                                        document.getElementById('<%=btnCancelPARequest.ClientID %>').disabled = false;
                                    }
                                }
                                catch {
                                    console.log('something went wrong while display Update PA Request...!');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                console.log(JSON.stringify(jqXHR));
                            }
                        });

                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(JSON.stringify(jqXHR));
                }
            });
        }
        else {
            return false;
        }
    }

    function DeleteProfessionalServiceDetails(Id) {
        var ClaimType = $(".rblClaimType input:checked").val();
        var result = confirm("Are you sure you want to delete?");
        if (result) {
            if (Id != '' && Id != null && Id != undefined) {
                var linkId = document.getElementById('<%=tempUniqueId.ClientID%>').value;
                var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "DELETE",
                    url: webApiPA + "DeleteServiceDetails?lineNumber=" + Id + "&&paType=" + ClaimType + "&&linkId=" + linkId,
                    headers: {
                        "Access-Control-Allow-Origin": "*",
                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                        "Authorization": "Bearer " + APIToken
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        GetServiceDetailsProfessional();
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        /* alert('Something went wrong while deleting the service details line item. Please Contact system administrator..!')*/
                        console.log("Something went wrong while deleting the service details line item. Please Contact system administrator..!");
                    }
                });
            }
        }
    }

    function GetServiceDetailsProfessional() {

        var Medicaid = getParameterByName('MedicaidNumber');
        var patype;
        var ClaimType = $(".rblClaimType input:checked").val();
        if (ClaimType == 'dental') {
            patype = 0;
        }
        else if (ClaimType == 'Professional') {
            patype = 2;
        }
        else {
            patype = 1;
        }
        var linkId = document.getElementById('<%=tempUniqueId.ClientID%>').value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetProfessionalServiceDetails?MedicaidId=" + Medicaid + "&&patype=" + patype + "&&linkId=" + linkId,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.length > 0) {
                    document.getElementById('divProfServDetailsNoData').innerHTML = '';

                    var table = "<table class='gridview' cellspacing='0' align='Left' rules='rows' border='1' style='width:100%;border-collapse:collapse;margin-right: 0px; margin-top: 0px;'><tbody><tr class='gridViewHeader' style='width:100px;'><th scope='col'>Line</th><th scope='col'>Procedure<br>Code Type</th><th scope='col'>Modifier</th><th scope='col'>Requested<br> Units</th><th scope='col'>Requested<br> Dollars</th><th scope='col'>Requested<br> FDOS</th><th scope='col'>Requested<br> TDOS</th><th scope='col'>Status</th><th scope='col'>&nbsp;</th><th scope='col'>&nbsp;</th></tr>";
                    for (var i = 0; i < result.length; i++) {

                        if (result[i].isErrorOccured != null && result[i].isErrorOccured != undefined && result[i].isErrorOccured != '' && result[i].isErrorOccured == true) {
                            console.log("Something went wrong while binding Professional service details. Exception Message:" + result[i].ErrorMessage);
                        }
                        else {
                            var sequence = i + 1;
                            var PRIOR_AUTH_PROFFSERVICE_DETAIL_ID = result[i].PRIOR_AUTH_PROFFSERVICE_DETAIL_ID;
                            var PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS = (result[i].PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS != null) ? result[i].PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS : '';
                            // var PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID = result.d[i].PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID;
                            var PRIOR_AUTH_PROCEDURE_CODE_ID = result[i].PRIOR_AUTH_PROCEDURE_CODE_ID;
                            var PRIOR_AUTH_STATUS_ID = result[i].PRIOR_AUTH_STATUS_ID;
                            var PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS = result[i].PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS;
                            var PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS = result[i].PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS;
                            var modifier1 = result[i].PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1;
                            var modifier2 = result[i].PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2;
                            var modifier3 = result[i].PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3;
                            var modifier4 = result[i].PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4;
                            var PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR = (result[i].PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR != null && result[i].PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR > 0) ? result[i].PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR : '';
                            var PRIOR_AUTH_REQUESTED_UNITS = (result[i].PRIOR_AUTH_REQUESTED_UNITS != null && result[i].PRIOR_AUTH_REQUESTED_UNITS > 0) ? result[i].PRIOR_AUTH_REQUESTED_UNITS : '';


                            // var Line = result.d[i].Line;

                            const fdos = PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS;


                            if (fdos != '' && fdos != null && fdos != undefined && fdos.length > 0) {
                                const D = new Date(fdos);

                                var fdosDate = getFormattedDate(D);
                                var priorAuthfdosDate = fdosDate;
                            }
                            else {
                                var priorAuthfdosDate = '';
                            };

                            const tdos = PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS;

                            if (tdos != '' && tdos != null && tdos != undefined && tdos.length > 0) {
                                const D = new Date(tdos);
                                var tdosDate = getFormattedDate(D);
                                var priorAuthtdosDate = tdosDate;
                            }
                            else {
                                var priorAuthtdosDate = '';
                            }


                            var status = '';

                            if (PRIOR_AUTH_STATUS_ID == '1') {
                                status = 'Submission Pending';
                            }
                            else if (PRIOR_AUTH_STATUS_ID == '2') {
                                status = 'Approved';
                            }
                            else if (PRIOR_AUTH_STATUS_ID == '3') {
                                status = 'Denied';
                            }
                            else {
                                status = PRIOR_AUTH_STATUS_ID;
                            }

                            if (document.getElementById('<%=txtstatus2.ClientID%>').value == "Approved" ||
                                document.getElementById('<%=txtstatus2.ClientID%>').value == "Closed") {
                                table = table + "<tr class='gridViewRow'><td><a class='gridLink' onclick='return EditProfessionalServiceDetails(\"" + PRIOR_AUTH_PROFFSERVICE_DETAIL_ID + "\")'>" + sequence + "</span></td><td><span title='Procedure Code'>" + PRIOR_AUTH_PROCEDURE_CODE_ID + "</span></td><td><input type='text' value='" + modifier1 + "' readonly='readonly' title='" + modifier1 + "' style='width:30px;float: left'><input type='text' value='" + modifier2 + "' readonly='readonly' title='" + modifier2 + "' style='width:30px;float: left'><input type='text' value='" + modifier3 + "' readonly='readonly' title='" + modifier3 + "' style='width:30px;float: left'><input type='text' value='" + modifier4 + "' readonly='readonly' title='" + modifier4 + "' style='width:30px;float: left'></td><td><input type='text' value='" + PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS + "' readonly='readonly' style='width:35px;'><input type='text' value='" + PRIOR_AUTH_REQUESTED_UNITS + "' readonly='readonly' style='width:80px;'></td><td><input type='text' value='" + PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR + "' readonly='readonly' style='width:120px;'></td><td><input type='text' value='" + priorAuthfdosDate + "' readonly='readonly'></td><td><input type='text' value='" + priorAuthtdosDate + "' readonly='readonly'></td><td><input type='text' value='" + status + "' readonly='readonly' disabled='disabled' style='padding:0 5px;' class='aspNetDisabled input-disable'></td></tr><tr class='gridViewFooter' style='background color:White;'><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>";
                            }
                            else {
                                table = table + "<tr class='gridViewRow'><td><a class='gridLink' onclick='return EditProfessionalServiceDetails(\"" + PRIOR_AUTH_PROFFSERVICE_DETAIL_ID + "\")'>" + sequence + "</span></td><td><span title='Procedure Code'>" + PRIOR_AUTH_PROCEDURE_CODE_ID + "</span></td><td><input type='text' value='" + modifier1 + "' readonly='readonly' title='" + modifier1 + "' style='width:30px;float: left'><input type='text' value='" + modifier2 + "' readonly='readonly' title='" + modifier2 + "' style='width:30px;float: left'><input type='text' value='" + modifier3 + "' readonly='readonly' title='" + modifier3 + "' style='width:30px;float: left'><input type='text' value='" + modifier4 + "' readonly='readonly' title='" + modifier4 + "' style='width:30px;float: left'></td><td><input type='text' value='" + PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS + "' readonly='readonly' style='width:35px;'><input type='text' value='" + PRIOR_AUTH_REQUESTED_UNITS + "' readonly='readonly' style='width:80px;'></td><td><input type='text' value='" + PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR + "' readonly='readonly' style='width:120px;'></td><td><input type='text' value='" + priorAuthfdosDate + "' readonly='readonly'></td><td><input type='text' value='" + priorAuthtdosDate + "' readonly='readonly'></td><td><input type='text' value='" + status + "' readonly='readonly' disabled='disabled' style='padding:0 5px;' class='aspNetDisabled input-disable'></td><td><input type='button' value='Edit' onclick='EditProfessionalServiceDetails(\"" + PRIOR_AUTH_PROFFSERVICE_DETAIL_ID + "\");' class='btn btn-primary' sytle='margin-left:10px' style='font-weight:bold;width:90px;'></td><td><input type='button' value='Delete' onclick='DeleteProfessionalServiceDetails(\"" + PRIOR_AUTH_PROFFSERVICE_DETAIL_ID + "\");' class='btn btn-danger' sytle='margin-left:10px' style='font-weight:bold;width:90px;'></td></tr><tr class='gridViewFooter' style='background color:White;'><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>";
                            }
                        }
                    }
                    var siblingElement = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_pnlsepProfessionalServiceDetail').nextElementSibling;
                    siblingElement.style.height = 'auto';
                    table = table + "</tbody></table>";

                    document.getElementById('professionalServiceDetails').innerHTML = table;


                }
                else {
                    document.getElementById('professionalServiceDetails').innerHTML = '';
                    document.getElementById('divProfServDetailsNoData').innerHTML = ' <h3 style="color:green"> No Service details found. Please Click on Add button to register. </h1>';

                    var siblingElement = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_pnlsepProfessionalServiceDetail').nextElementSibling;
                    siblingElement.style.height = 'auto';


                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("Something went wrong while binding Professional service details. Exception Message:" + JSON.stringify(jqXHR));
            }
        });
    }

    function prepareProfessionalServDetails() {

        var unitMeasurementId = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddProffSDMeasurements').value;
        var reqUnits = $("#<%= txtProfessionalReqUnits.ClientID %>").first().val();
        var reqUnitsFee = $("#<%= txtProfessionalReqDollars.ClientID %>").first().val();
        var txtProfessionalProvServnote = $("#<%= txtProfessionalProvServnote.ClientID %>").first().val();
        var txtProfessionalSDProcCode = $("#<%= txtProfessionalSDProcCode.ClientID %>").first().val();
        var txtProfessionalProcCodeDescription = $("#<%= txtProfessionalProcCodeDescription.ClientID %>").first().val();
        var txtProfessionalRemainingUnits = $("#<%= txtProfessionalRemainingUnits.ClientID %>").first().val();
        var txtProfessionalReqFDOS = $("#<%= txtProfessionalReqFDOS.ClientID %>").first().val();
        var txtProfessionalReqTDOS = $("#<%= txtProfessionalReqTDOS.ClientID %>").first().val();
        var txtProfessionalServDetailsStatus = $("#<%= txtProfessionalServDetailsStatus.ClientID %>").first().val();
        var txtProfessionalServTrackingNo = $("#<%= txtProfessionalServTrackingNo.ClientID %>").first().val();
        var txtModifier1 = $("#<%= txtModifier1.ClientID %>").first().val();
        var txtModifier2 = $("#<%= txtModifier2.ClientID %>").first().val();
        var txtModifier3 = $("#<%= txtModifier3.ClientID %>").first().val();
        var txtModifier4 = $("#<%= txtModifier4.ClientID %>").first().val();

        var servDetails = {

            procedureCode: txtProfessionalSDProcCode,
            procCodeDesc: txtProfessionalProcCodeDescription,
            providerServiceNote: txtProfessionalProvServnote,
            reqUnits: reqUnits,
            remainUnits: txtProfessionalRemainingUnits,
            requnitsfee: reqUnitsFee,
            fdos: txtProfessionalReqFDOS,
            tdos: txtProfessionalReqTDOS,
            status: txtProfessionalServDetailsStatus,
            unitMeasurement: unitMeasurementId,
            modifier1: txtModifier1,
            modifier2: txtModifier2,
            modifier3: txtModifier3,
            modifier4: txtModifier4,
            serviceTrackingNo: txtProfessionalServTrackingNo

        }
        return servDetails;
    }

    function EditProfessionalServiceDetails(Id) {
        cancelProfessionalLineAdd();

        var rfvtxtProfessionalSDProcCode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtProfessionalSDProcCode");
        if (typeof (rfvtxtProfessionalSDProcCode) != 'undefined' && rfvtxtProfessionalSDProcCode != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtProfessionalSDProcCode').style.display = "none";
        }
        var rfvtxtProfessionalReqUnits = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtProfessionalReqUnits");
        if (typeof (rfvtxtProfessionalReqUnits) != 'undefined' && rfvtxtProfessionalReqUnits != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtProfessionalReqUnits').style.display = "none";
        }
        var rfvddProffSDMeasurements = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddProffSDMeasurements");
        if (typeof (rfvddProffSDMeasurements) != 'undefined' && rfvddProffSDMeasurements != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvddProffSDMeasurements').style.display = "none";
        }
        var RequiredFieldValidator7 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator7");
        if (typeof (RequiredFieldValidator7) != 'undefined' && RequiredFieldValidator7 != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator7').style.display = "none";
        }
        var RequiredFieldValidator2 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator2");
        if (typeof (RequiredFieldValidator2) != 'undefined' && RequiredFieldValidator2 != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator2').style.display = "none";
        }
        var RequiredFieldValidator9 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator9");
        if (typeof (RequiredFieldValidator9) != 'undefined' && RequiredFieldValidator9 != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator9').style.display = "none";
        }
        var RequiredRangeValidator1 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredRangeValidator1");
        if (typeof (RequiredRangeValidator1) != 'undefined' && RequiredRangeValidator1 != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredRangeValidator1').style.display = "none";
        }

        var Medicaid = getParameterByName('MedicaidNumber');
        var ClaimType = $(".rblClaimType input:checked").val();
        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddProffSDMeasurements').options[0].selected = true;
        document.getElementById('btnServProfessionalUpdate').value = "Update";
        var ClaimType = $(".rblClaimType input:checked").val();
        var patype;
        if (ClaimType == 'dental') {
            patype = 0;
        }
        else if (ClaimType == 'Professional') {
            patype = 2;
        }
        else {
            patype = 1;
        }
        var linkId = document.getElementById('<%=tempUniqueId.ClientID%>').value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetProfessionalServiceDetail?lineNumber=" + Id + "&&MedicaidId=" + Medicaid + "&&patype=" + patype + "&&linkId=" + linkId,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                const dateStringFDOS = result.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_FDOS;

                if (dateStringFDOS != '' && dateStringFDOS != null && dateStringFDOS != undefined && dateStringFDOS.length > 0) {
                    const D = new Date(dateStringFDOS);
                    var FDOSDate = getFormattedDate(D);
                    $("[id*=txtProfessionalReqFDOS]").val(FDOSDate);
                }
                else {
                    $("[id*=txtProfessionalReqFDOS]").val('');
                }

                const dateStringTDOS = result.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_TDOS;

                if (dateStringTDOS != '' && dateStringTDOS != null && dateStringTDOS != undefined && dateStringTDOS.length > 0) {
                    const D = new Date(dateStringTDOS);
                    var TDOSDate = getFormattedDate(D);
                    $("[id*=txtProfessionalReqTDOS]").val(TDOSDate);
                }
                else {
                    $("[id*=txtProfessionalReqTDOS]").val('');
                }

                const dateStringAFDOS = result.PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_FDOS;

                if (dateStringAFDOS != '' && dateStringAFDOS != null && dateStringAFDOS != undefined && dateStringAFDOS.length > 0) {
                    const D = new Date(dateStringAFDOS);
                    var AFDOSDate = getFormattedDate(D);
                    $("[id*=txtProfessionalAuthFDOS]").val(AFDOSDate);
                }
                else {
                    $("[id*=txtProfessionalAuthFDOS]").val('');
                }

                const dateStringATDOS = result.PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_TDOS;
                // alert(dateStringATDOS);
                if (dateStringATDOS != '' && dateStringATDOS != null && dateStringATDOS != undefined && dateStringATDOS.length > 0) {
                    const D = new Date(dateStringATDOS);
                    var ATDOSDate = getFormattedDate(D);
                    $("[id*=txtProfessionalAuthTDOS]").val(ATDOSDate);
                }
                else {
                    $("[id*=txtProfessionalAuthTDOS]").val('');
                }


                $("[id*=txtModifier1]").val(result.PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER1);
                $("[id*=txtModifier2]").val(result.PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER2);
                $("[id*=txtModifier3]").val(result.PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER3);
                $("[id*=txtModifier4]").val(result.PRIOR_AUTH_PROFFSERVICE_DETAIL_MODIFIER4);
                $("[id*=hdProfLineNumber]").val(Id);


                if (result.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS != null && result.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS > 0) {
                    $("[id*=txtProfessionalReqUnits]").val(result.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_UNITS);
                }
                else {
                    $("[id*=txtProfessionalReqUnits]").val('');
                }

                if (result.PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS != null && result.PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS > 0) {
                    $("[id*=txtProfessionalAuthUnits]").val(result.PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_UNITS);
                }
                else {
                    $("[id*=txtProfessionalAuthUnits]").val('');
                }

                if (result.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR != null && result.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR > 0) {
                    $("[id*=txtProfessionalReqDollars]").val(result.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUEST_DOLLAR);
                }
                else {
                    $("[id*=txtProfessionalReqDollars]").val('');
                }



                if (result.PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR != null || result.PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR > 0) {
                    $("[id*=txtProfessionalAuthDollars]").val(result.PRIOR_AUTH_PROFFSERVICE_DETAIL_AUTHORIZED_DOLLAR);
                }
                else {
                    $("[id*=txtProfessionalAuthDollars]").val('');
                }

                $("[id*=txtProfessionalSDProcCode]").val(result.PRIOR_AUTH_PROCEDURE_CODE_ID);

                if (result.PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS != null && result.PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS > 0) {
                    $("[id*=txtProfessionalRemainingUnits]").val(result.PRIOR_AUTH_PROFFSERVICE_DETAIL_REAMING_UNITS);
                }
                else {
                    $("[id*=txtProfessionalRemainingUnits]").val('');
                }
                if (result.StatusDesc != 'Approved') {
                    $("[id*=txtProfessionalAuthFDOS]").val('');
                    $("[id*=txtProfessionalAuthTDOS]").val('');
                    $("[id*=txtProfessionalAuthUnits]").val('');
                    $("[id*=txtProfessionalRemainingUnits]").val('');
                    $("[id*=txtProfessionalAuthDollars]").val('');

                }
                if (result.StatusDesc == '1') {
                    $("[id*=txtProfessionalServDetailsStatus]").val('');
                }
                else if (result.StatusDesc == '2') { $("[id*=txtProfessionalServDetailsStatus]").val('Approved'); }
                else if (result.StatusDesc == '3') { $("[id*=txtProfessionalServDetailsStatus]").val('Denied'); }
                else { $("[id*=txtProfessionalServDetailsStatus]").val(result.StatusDesc); }

                $("[id*=txtProfessionalServTrackingNo]").val(result.PRIOR_AUTH_PROFFSERVICE_DETAIL_TRACKING_NUM);
                $("[id*=txtProfessionalProcCodeDescription]").val(result.PRIOR_AUTH_PROFFSERVICE_DETAIL_PROCEDURE_DESC);
                $("[id*=txtProfessionalProvServnote]").val(result.PRIOR_AUTH_PROFFSERVICE_DETAIL_PROVIDER_SERVICE_NOTE);

                var ddProffSDMeasurements = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddProffSDMeasurements');
                var requestedUnitsID = result.PRIOR_AUTH_PROFFSERVICE_DETAIL_REQUESTED_UNITS_ID;
                if (requestedUnitsID != '') {
                    for (var i = 0; i < ddProffSDMeasurements.options.length; i++) {
                        if (ddProffSDMeasurements.options[i].value == requestedUnitsID) {
                            ddProffSDMeasurements.options[i].selected = true;
                        }
                    }
                }

                document.getElementById("dvSDProfessionalAddparent").style.height = "auto";
                document.getElementById("dvSDProfessionalAddparent").style.overflow = "visible";
                document.getElementById("dvSDProfessionalAddparent").style.visibility = "visible";
                //if (document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_btnUpdate') != null) {
                //    document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_btnUpdate').style.display = "inline";
                //}                
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(JSON.stringify(jqXHR));
            }
        });

        if (document.getElementById('<%=txtstatus2.ClientID%>').value == "Approved" ||
            document.getElementById('<%=txtstatus2.ClientID%>').value == "Closed") {
            document.getElementById('btnServProfessionalUpdate').style.visibility = 'hidden';
            document.getElementById('btnServProfessionalUpdate').disabled = true;
        }
    }

    function addDentalLine() {


        var rfvtxtDentalSDProcCode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtDentalSDProcCode");



        if (typeof (rfvtxtDentalSDProcCode) != 'undefined' && rfvtxtDentalSDProcCode != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtDentalSDProcCode').style.display = "none";
        }
        var rfvtxtDentalReqUnits = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtDentalReqUnits");
        if (typeof (rfvtxtDentalReqUnits) != 'undefined' && rfvtxtDentalReqUnits != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtDentalReqUnits').style.display = "none";
        }
        var RequiredFieldValidator4 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator4");
        if (typeof (RequiredFieldValidator4) != 'undefined' && RequiredFieldValidator4 != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator4').style.display = "none";
        }
        var RequiredFieldValidator5 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator5");
        if (typeof (RequiredFieldValidator5) != 'undefined' && RequiredFieldValidator5 != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator5').style.display = "none";
        }
        var RequiredFieldValidator6 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator6");
        if (typeof (RequiredFieldValidator6) != 'undefined' && RequiredFieldValidator6 != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator6').style.display = "none";
        }

        var ddDentalToothNumber = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalToothNumber");
        if (typeof (ddDentalToothNumber) != 'undefined' && ddDentalToothNumber != null) {
            document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalToothNumber").options[0].selected = true;
        }
        var RequiredRangeValidator = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredRangeValidator");
        if ((RequiredRangeValidator.style.visibility == "hidden")) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredRangeValidator').style.display = "none";
        }


        var ddDentalProsthsis = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalProsthsis");
        if (typeof (ddDentalProsthsis) != 'undefined' && ddDentalProsthsis != null) {
            document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalProsthsis").options[0].selected = true;
        }



        var ddDentalOralCavity1 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity1");
        if (typeof (ddDentalOralCavity1) != 'undefined' && ddDentalOralCavity1 != null) {
            document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity1").options[0].selected = true;
        }



        var dentalOralCavity2 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity2");  //NO


        console.log(dentalOralCavity2);
        //if (typeof (dentalOralCavity2) != 'undefined' && dentalOralCavity2 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity2").options[0].selected = true;
        //}


        //var dentalOralCavity3 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity3");
        //if (typeof (dentalOralCavity3) != 'undefined' && dentalOralCavity3 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity3").options[0].selected = true;
        //}



        //var dentalOralCavity4 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity4");
        //if (typeof (dentalOralCavity4) != 'undefined' && dentalOralCavity4 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity4").options[0].selected = true;
        //}



        //var dentalOralCavity5 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity5");
        //if (typeof (dentalOralCavity5) != 'undefined' && dentalOralCavity5 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity5").options[0].selected = true;
        //}


        var ddToothSurface1 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface1");
        if (typeof (ddToothSurface1) != 'undefined' && ddToothSurface1 != null) {
            document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface1").options[0].selected = true;
        }




        //var ddToothSurface2 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface2");
        //if (typeof (ddToothSurface2) != 'undefined' && ddToothSurface2 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface2").options[0].selected = true;
        //}


        //var ddToothSurface3 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface3");
        //if (typeof (ddToothSurface3) != 'undefined' && ddToothSurface3 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface3").options[0].selected = true;
        //}


        //var ddToothSurface4 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface4");

        //if (typeof (ddToothSurface4) != 'undefined' && ddToothSurface4 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface4").options[0].selected = true;
        //}


        //var ddToothSurface5 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface5");
        //if (typeof (ddToothSurface5) != 'undefined' && ddToothSurface5 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface5").options[0].selected = true;
        //}


        $("[id*=txtDentalSDProcCode]").val('');
        $("[id*=lblservDentalLineNumber]").val('');
        document.getElementById("dvSDDentalAddparent").style.height = "auto";
        document.getElementById("dvSDDentalAddparent").style.overflow = "visible";
        document.getElementById("dvSDDentalAddparent").style.visibility = "inherit";
        document.getElementById('btnServDentalUpdate').value = "Add";
        document.getElementById('btnServDentalUpdate').title = "Add";
        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_btnUpdate').style.display = "inline";

        $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtDentalProcCodeDescription").attr("maxlength", "80");

        return false;
    }

    function cancelDentalLineAdd() {

        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalToothNumber").options[0].selected = true;
        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalProsthsis").options[0].selected = true;
        document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity1").options[0].selected = true;
        var dentalOralCavity2 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity2");
        //if (dentalOralCavity2 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity2").options[0].selected = true;
        //}

        //var dentalOralCavity3 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity3");
        //if (dentalOralCavity3 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity3").options[0].selected = true;
        //}

        //var dentalOralCavity4 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity4");
        //if (dentalOralCavity4 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity4").options[0].selected = true;
        //}

        //var dentalOralCavity5 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity5");
        //if (dentalOralCavity5 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity5").options[0].selected = true;
        //}

        var ddToothSurface1 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface1");
        if (ddToothSurface1 != null) {
            document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface1").options[0].selected = true;
        }

        //var ddToothSurface2 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface2");
        //if (ddToothSurface2 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface2").options[0].selected = true;
        //}

        //var ddToothSurface3 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface3");
        //if (ddToothSurface3 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface3").options[0].selected = true;
        //}

        //var ddToothSurface4 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface4");

        //if (ddToothSurface4 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface4").options[0].selected = true;
        //}

        //var ddToothSurface5 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface5");
        //if (ddToothSurface5 != null) {
        //    document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface5").options[0].selected = true;
        //}
        $("[id*=txtDentalSDProcCode]").val('');
        $("[id*=txtDentalReqUnits]").val('');
        $("[id*=txtDentalAuthUnits]").val('');
        $("[id*=txtDentalProcCodeDescription]").val('');

        $("[id*=txtDentalProvServnote]").val('');
        $("[id*=txtDentalReqDollars]").val('');
        $("[id*=txtDentalAuthDollars]").val('');
        $("[id*=txtDentalReqFDOS]").val('');
        $("[id*=txtDentalAuthFDOS]").val('');
        $("[id*=txtDentalReqTDOS]").val('');
        $("[id*=txtDentalAuthTDOS]").val('');
        $("[id*=txtDentalRemainingUnits]").val('');
        $("[id*=txtDentalServTrackingNo]").val('');
        $("[id*=txtDentalServDetailsStatus]").val('');
        document.getElementById("dvSDDentalAddparent").style.height = "0";
        document.getElementById("dvSDDentalAddparent").style.overflow = "hidden";
        document.getElementById("dvSDDentalAddparent").style.visibility = "hidden";
    }

    function DeleteDentalServiceDetails(Id) {
        var ClaimType = $(".rblClaimType input:checked").val();
        var result = confirm("Are you sure you want to delete?");
        if (result) {
            if (Id != '' && Id != null && Id != undefined) {
                var linkId = document.getElementById('<%=tempUniqueId.ClientID%>').value;
                var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "DELETE",
                    url: webApiPA + "DeleteServiceDetails?lineNumber=" + Id + "&&paType=" + ClaimType + "&&linkId=" + linkId,
                    headers: {
                        "Access-Control-Allow-Origin": "*",
                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                        "Authorization": "Bearer " + APIToken
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        GetServiceDetailsDental();
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        /*  alert('Something went wrong while deleting the service details line item. Please Contact system administrator..!')*/
                        console.log("Something went wrong while deleting the service details line item. Please Contact system administrator..!");
                    }
                });
            }
        }
    }

    function GetServiceDetailsDental() {
        var Medicaid = getParameterByName('MedicaidNumber');
        var patype;
        var ClaimType = $(".rblClaimType input:checked").val();
        if (ClaimType == 'dental') {
            patype = 0;
        }
        else if (ClaimType == 'Professional') {
            patype = 2;
        }
        else {
            patype = 1;
        }
        var linkId = document.getElementById('<%=tempUniqueId.ClientID%>').value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetDentalServiceDetailsPA?MedicaidId=" + Medicaid + "&&patype=" + patype + "&&linkId=" + linkId,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.length > 0) {
                    document.getElementById('divDentalServDetailsNoData').innerHTML = '';

                    var table = "<table class='gridview' cellspacing='0' align='Left' rules='rows' border='1' style='width:100%;min-width:100%;border-collapse:collapse;margin-right: 0px; margin-top: 0px;'><tbody><tr class='gridViewHeader' style='width:100px;'><th scope='col'>Line</th><th scope='col'>Procedure<br>Code</th><th scope='col'>Tooth<br> Number</th><th scope='col'>Oral<br> Cavity</th><th scope='col'>Requested<br> Units</th><th scope='col'>Requested<br> Dollars</th><th scope='col'>Requested<br> FDOS</th><th scope='col'>Requested<br> TDOS</th><th scope='col'>Status</th><th scope='col'>&nbsp;</th><th scope='col'>&nbsp;</th></tr>";
                    for (var i = 0; i < result.length; i++) {


                        var sequence = i + 1;
                        var PRIOR_AUTH_DENTALSERVICE_DETAIL_ID = result[i].PRIOR_AUTH_DENTALSERVICE_DETAIL_ID;

                        var PRIOR_AUTH_TOOTH_NUMBER_ID;
                        if (result[i].PRIOR_AUTH_TOOTH_NUMBER_CODE != null && result[i].PRIOR_AUTH_TOOTH_NUMBER_CODE != "" && result[i].PRIOR_AUTH_TOOTH_NUMBER_CODE != undefined) {
                            PRIOR_AUTH_TOOTH_NUMBER_ID = result[i].PRIOR_AUTH_TOOTH_NUMBER_CODE;
                        }
                        else {
                            PRIOR_AUTH_TOOTH_NUMBER_ID = "";
                        }


                        var PRIOR_AUTH_PROCEDURE_CODE_ID = result[i].PRIOR_AUTH_PROCEDURE_CODE_ID;
                        var PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS = (result[i].PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS != null && result[i].PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS > 0) ? result[i].PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS : '';
                        var PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR = (result[i].PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR != null && result[i].PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR > 0) ? result[i].PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR : '';
                        var PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS = result[i].PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS;

                        var PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS = result[i].PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS;
                        var PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS = result[i].PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS;
                        var PRIOR_AUTH_STATUS_ID = result[i].PRIOR_AUTH_STATUS_ID;

                        var Line = result[i].Line;
                        const fdos = PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS;

                        if (fdos != '' && fdos != null && fdos != undefined && fdos.length > 0) {
                            const D = new Date(fdos);
                            var fdosDate = getFormattedDate(D);
                            var priorAuthfdosDate = fdosDate;
                        }
                        else {
                            var priorAuthfdosDate = '';
                        }

                        const tdos = PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS;

                        if (tdos != '' && tdos != null && tdos != undefined && tdos.length > 0) {
                            const D = new Date(tdos);
                            var tdosDate = getFormattedDate(D);
                            var priorAuthtdosDate = tdosDate;
                        }
                        else {
                            var priorAuthtdosDate = '';
                        }


                        var status = '';

                        if (PRIOR_AUTH_STATUS_ID == '1') {
                            status = 'Submission Pending';
                        }
                        else if (PRIOR_AUTH_STATUS_ID == '2') {
                            status = 'Approved';
                        }
                        else if (PRIOR_AUTH_STATUS_ID == '3') {
                            status = 'Denied';
                        }
                        else {
                            status = PRIOR_AUTH_STATUS_ID;
                        }

                        if (document.getElementById('<%=txtstatus2.ClientID%>').value == "Approved" ||
                            document.getElementById('<%=txtstatus2.ClientID%>').value == "Closed") {
                            table = table + "<tr class='gridViewRow'><td><a class='gridLink' onclick='return EditDentalServiceDetails(\"" + PRIOR_AUTH_DENTALSERVICE_DETAIL_ID + "\")'>" + sequence + "</a></td><td><span title='Procedure Code'>" + PRIOR_AUTH_PROCEDURE_CODE_ID + "</span></td><td><span title='ToothNumber'>" + PRIOR_AUTH_TOOTH_NUMBER_ID + "</span></td><td><span title='Oral Cavity'>" + PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS + "</span></td><td><input type='text' value='" + PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS + "' readonly='readonly'></td><td><input type='text' value='" + PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR + "' readonly='readonly'></td><td><input type='text' value='" + priorAuthfdosDate + "' readonly='readonly'></td><td><input type='text' value='" + priorAuthtdosDate + "' readonly='readonly'></td><td><input type='text' value='" + status + "' readonly='readonly' disabled='disabled' style='padding: 0 5px !important;' class='aspNetDisabled input-disable'></td></tr><tr class='gridViewFooter' style='background-color:White;'><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>";
                        }
                        else {
                            table = table + "<tr class='gridViewRow'><td><a class='gridLink' onclick='return EditDentalServiceDetails(\"" + PRIOR_AUTH_DENTALSERVICE_DETAIL_ID + "\")'>" + sequence + "</a></td><td><span title='Procedure Code'>" + PRIOR_AUTH_PROCEDURE_CODE_ID + "</span></td><td><span title='ToothNumber'>" + PRIOR_AUTH_TOOTH_NUMBER_ID + "</span></td><td><span title='Oral Cavity'>" + PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS + "</span></td><td><input type='text' value='" + PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS + "' readonly='readonly'></td><td><input type='text' value='" + PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR + "' readonly='readonly'></td><td><input type='text' value='" + priorAuthfdosDate + "' readonly='readonly'></td><td><input type='text' value='" + priorAuthtdosDate + "' readonly='readonly'></td><td><input type='text' value='" + status + "' readonly='readonly' disabled='disabled' style='padding: 0 5px !important;' class='aspNetDisabled input-disable'></td><td><input type='button' value='Edit' onclick='EditDentalServiceDetails(\"" + PRIOR_AUTH_DENTALSERVICE_DETAIL_ID + "\");' class='btn btn-primary' sytle='margin-left:10px' style='font-weight:bold;width:90px;'></td><td><input type='button' value='Delete' onclick='DeleteDentalServiceDetails(\"" + PRIOR_AUTH_DENTALSERVICE_DETAIL_ID + "\");' class='btn btn-danger' sytle='margin-left:10px' style='font-weight:bold;width:90px;'></td></tr><tr class='gridViewFooter' style='background-color:White;'><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr>";
                        }
                    }
                    var siblingElement = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_pnlsepDentalServiceDetail').nextElementSibling;
                    siblingElement.style.height = 'auto';
                    table = table + "</tbody></table>";

                    document.getElementById('dentalServiceDetails').innerHTML = table;
                    document.getElementById("dvSDDentalAddparent").style.height = "0";
                    document.getElementById("dvSDDentalAddparent").style.overflow = "hidden";
                    document.getElementById("dvSDDentalAddparent").style.visibility = "hidden";
                    //alert("Table" + table);
                }
                else {
                    //alert("Table empty");
                    document.getElementById('dentalServiceDetails').innerHTML = '';
                    document.getElementById('divDentalServDetailsNoData').innerHTML = ' <h3 style="color:green"> No Service details found. Please Click on Add button to register. </h3>';
                    var siblingElement = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_pnlsepDentalServiceDetail').nextElementSibling;
                    siblingElement.style.height = 'auto';
                    // document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblprofNoDetailsFound').style.visibility = "visible";

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //alert("Something went wrong while binding while binding Dental Service details. Please contact system administrator...!");
                console.log("Something went wrong while binding while binding Dental Service details. Please contact system administrator...!");
            }
        });
    }

    function SaveDentalServiceDetails() {
        if (Page_ClientValidate('valServiceDetails_Dental')) {
            var ClaimType = $(".rblClaimType input:checked").val();

            if (ClaimType == 'dental') {

                //check length of Proc code desc 80 max
                var procDesc = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtDentalProcCodeDescription").first().val();
                if (procDesc != null && procDesc.length > 80) {
                    $("#srvErrorMessage").text("Procedure Code Description cannot exceed more than 80 characters.");
                    return false;
                }
                else {
                    $("#srvErrorMessage").text("");
                }
                //check length of Proc Note 264 max
                var srvNote = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtDentalProvServnote").first().val();
                if (srvNote != null && srvNote.length > 264) {
                    $("#srvErrorMessage").text("Provider Service Note cannot exceed more than 264 characters.")
                    return false;
                }
                else {
                    $("#srvErrorMessage").text("");
                }
            }

            var operation = document.getElementById('btnServDentalUpdate').value;
            var lineNumber = '';

            var Medicaid = getParameterByName('MedicaidNumber');
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalDatemessage').style.visibility = "hidden";

            var txtReqFDOS = $("#<%= txtDentalReqFDOS.ClientID %>").first().val();
            var txtReqTDOS = $("#<%= txtDentalReqTDOS.ClientID %>").first().val();

            var txtDentalAuthFDOS = $("#<%= txtDentalAuthFDOS.ClientID %>").first().val();
            var txtDentalAuthTDOS = $("#<%= txtDentalAuthTDOS.ClientID %>").first().val();
            var txtDentalRemainingUnits = $("#<%= txtDentalRemainingUnits.ClientID %>").first().val();

            if (operation == 'Update') {
                lineNumber = $("[id*=hdDentalLineNumber]").val();
                opearion = 'Update';
            }
            else {
                lineNumber = '';
                opearion = 'Add';
            }

            var message = validateFDOSTDOSDates(txtReqFDOS, txtReqTDOS);

            if (message != '') {
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalDatemessage').style.visibility = "visible";
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalDatemessage').innerHTML = message;
                return false;
            }
            else {
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalDatemessage').style.visibility = "hidden";
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalDatemessage').innerHTML = '';
            }

            if (txtDentalAuthFDOS != '' && txtDentalAuthTDOS != '') {
                var authmessage = validateFDOSTDOSDates(txtDentalAuthFDOS, txtDentalAuthTDOS);
            }

            if (ClaimType == 'dental') {
                var txtDentalSDProcCode = $("#<%= txtDentalSDProcCode.ClientID %>").first().val();
                var txtModifier1 = '';
                var txtModifier2 = '';
                var txtModifier3 = '';
                var txtModifier4 = '';
                var claimsorPA = "PA";
                var claimORPAType = "Dental";
                validateProcedureCodeModifiers(txtDentalSDProcCode, txtModifier1, txtModifier2, txtModifier3, txtModifier4, claimsorPA, claimORPAType);
            }

            if (validateField == false) {
                return false;
            }

            var servDetails = prepareDentalServDetails();
            var imputData = JSON.stringify(servDetails);
            var procCodeDesc = '';
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetProcedureCodeData?procCode=" + servDetails.procedureCode + "&&procCodeDesc=" + procCodeDesc + "&&isTextChangesEvent=" + true,
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
                        if (ClaimType == 'Institutional') {
                            document.getElementById('divProcCodeLoader1').innerHTML = "";
                            document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = 'Procedure code is invalid';
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "visible";
                        }
                        if (ClaimType == 'dental') {
                            document.getElementById('divProcCodeLoaderDental').innerHTML = "";
                            document.getElementById('<%= lblDentalProcMessage.ClientID%>').innerHTML = 'Procedure code is invalid';
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalProcMessage').style.visibility = "visible";
                        }
                        if (ClaimType == 'Professional') {
                            document.getElementById('divProcCodeLoaderProfessional').innerHTML = "";
                            document.getElementById('<%= lblProfProcMessage.ClientID%>').innerHTML = 'Procedure code is invalid';
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfProcMessage').style.visibility = "visible";
                        }
                    }
                    else if (result.length > 1) {
                        $("[id*=txtHCPCSCode]").val(txtServicecode);
                        var table = "<table class='gridViewSmallFont FacilityTypeSearchResultGrid' cellspacing='0' rules='all' border='1' style='width:100%;border-collapse:collapse;'><tbody><tr class='gridViewHeader' style='color:Black;'><th scope='col'>Procedure Code</th><th scope='col'>Procedure Code Description</th></tr>";
                        for (var i = 0; i < result.length; i++) {
                            table = table + "<tr class='gridViewRow' valign='top'><td style='width:100px;'><a onClick='GetRevenueSelectedRow(\"" + result[i].ProcedureCode + "\"); return false;'>" + result[i].ProcedureCode + "</a></td><td>" + result[i].ProcedureCodeDesc + "</td></tr>";
                        };
                        table = table + "</table>";
                        document.getElementById('procedureCodeOutput').innerHTML = table;
                        if (ClaimType == 'Institutional') {
                            document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = '';
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "hidden";
                            document.getElementById('divProcCodeLoader1').innerHTML = "";
                        }
                        if (ClaimType == 'dental') {
                            document.getElementById('<%= lblDentalProcMessage.ClientID%>').innerHTML = '';
                            document.getElementById('divProcCodeLoaderDental').innerHTML = "";
                        }
                        if (ClaimType == 'Professional') {
                            document.getElementById('<%= lblProfProcMessage.ClientID%>').innerHTML = '';
                            document.getElementById('divProcCodeLoaderProfessional').innerHTML = "";
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfProcMessage').style.visibility = "hidden";
                        }
                        $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeSubmitPriorAuthSearchProc").show();
                    }
                    else {
                        if (ClaimType == 'Institutional') {
                            $("[id*=txtLnProcedureCode]").val(result[0].ProcedureCode);
                            document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = '';
                            document.getElementById('divProcCodeLoader1').innerHTML = "";
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "hidden";
                        }
                        if (ClaimType == 'dental') {
                            $("[id*=txtDentalSDProcCode]").val(result[0].ProcedureCode);
                            document.getElementById('<%= lblDentalProcMessage.ClientID%>').innerHTML = '';
                            document.getElementById('divProcCodeLoaderDental').innerHTML = "";
                        }
                        if (ClaimType == 'Professional') {
                            $("[id*=txtProfessionalSDProcCode]").val(result[0].ProcedureCode);
                            document.getElementById('<%= lblProfProcMessage.ClientID%>').innerHTML = '';
                            document.getElementById('divProcCodeLoaderProfessional').innerHTML = "";
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfProcMessage').style.visibility = "hidden";
                        }
                        var linkId = document.getElementById('<%=tempUniqueId.ClientID%>').value;

                        var ClaimType = $(".rblClaimType input:checked").val();
                        var patype;
                        if (ClaimType == 'dental') {
                            patype = 0;
                        }
                        else if (ClaimType == 'Professional') {
                            patype = 2;
                        }
                        else {
                            patype = 1;
                        }
                        var userName = document.getElementById('<%=hdnUserName.ClientID%>').value;
                        var APIToken = $("[id*=hdnAccessToken]").val();
                        // alert(linkId);
                        $.ajax({
                            type: "POST",
                            url: webApiPA + "SaveDentalServiceDetails?crownInLay=" + servDetails.crownInLay + "&&procedureCode=" + servDetails.procedureCode + "&&procCodeDesc=" + servDetails.procCodeDesc + "&&providerServiceNote=" + servDetails.providerServiceNote + "&&reqUnits=" + servDetails.reqUnits + "&&remainUnits=" + servDetails.remainUnits + "&&requnitsfee=" + servDetails.requnitsfee + "&&fdos=" + servDetails.fdos + "&&tdos=" + servDetails.tdos + "&&status=" + servDetails.status + "&&toothNum=" + servDetails.toothNum + "&&prosthesisNum=" + servDetails.prosthesisNum + "&&cavity1=" + servDetails.cavity1 + "&&cavity2=" + servDetails.cavity2 + "&&cavity3=" + servDetails.cavity3 + "&&cavity4=" + servDetails.cavity4 + "&&cavity5=" + servDetails.cavity5 + "&&surface1=" + servDetails.surface1 + "&&surface2=" + servDetails.surface2 + "&&surface3=" + servDetails.surface3 + "&&surface4=" + servDetails.surface4 + "&&surface5=" + servDetails.surface5 + "&&MedicaidId=" + Medicaid + "&&serviceTrackingNo=" + servDetails.serviceTrackingNo + "&&operation=" + operation + "&&lineNumber=" + lineNumber + "&&patype=" + patype + "&&linkId=" + linkId + "&&userName=" + userName,
                            headers: {
                                "Access-Control-Allow-Origin": "*",
                                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                                "Authorization": "Bearer " + APIToken
                            },
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (result) {
                                /*  alert('Before GetServiceDetailsDental');*/
                                GetServiceDetailsDental();
                                cancelDentalLineAdd();
                                //OHPNM-11929: UPDATE PA button is not showing after editing service detail panel
                                try {
                                    if (document.getElementById('<%=txtstatus2.ClientID%>').value == "Pend" || document.getElementById('<%=txtstatus2.ClientID%>').value == "InProcess" || document.getElementById('<%=txtstatus2.ClientID%>').value == "Pending Addtl Info") {
                                        document.getElementById('<%=btnUpdate.ClientID %>').style.visibility = "visible";
                                        document.getElementById('<%=btnUpdate.ClientID %>').style.display = "inline-block";
                                        document.getElementById('<%=btnCancelPARequest.ClientID %>').disabled = true;
                                    }
                                    else {
                                        document.getElementById('<%=btnUpdate.ClientID %>').style.visibility = "hidden";
                                        document.getElementById('<%=btnCancelPARequest.ClientID %>').disabled = false;
                                    }
                                }
                                catch {
                                    console.log('something went wrong while display Update PA Request...!');
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                /*   alert(JSON.stringify(jqXHR));*/
                                console.log(JSON.stringify(jqXHR));
                            }
                        });
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(JSON.stringify(jqXHR));
                }
            });
        }
        else {
            return false;
        }
        var selectedoption = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_txtDentalSDProcCode').value;
        if (selectedoption === "") {
            document.getElementById("reqmessage").style.display = "block";
            return false; // prevent form submission
        }
    }

    function validateProcedureCodeModifiers(procedureCode, txtModifier1, txtModifier2, txtModifier3, txtModifier4, claimsorPA, claimORPAType) {
        var txtProcCode = procedureCode;
        var APIToken = $("[id*=hdnAccessToken]").val();
        var providerTypeId = $("[id*=hdnProviderTypeId]").val();

        $.ajax({
            type: "GET",
            url: webApiClaims + "validateProcedureCodeModifiers?provider_type_id=" + providerTypeId + "&&procedure_code=" + txtProcCode + "&&M1=" + txtModifier1 + "&&M2=" + txtModifier2 + "&&M3=" + txtModifier3 + "&&M4=" + txtModifier4 + "&&claimsorPA=" + claimsorPA + "&&claimORPAType=" + claimORPAType,
            //data: '{desc: "" , val: "' + txtProcCode + '", includeICDCodes: false }',
            async: false,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if ((result.length > 0)) {
                    validateField = false;
                    if (claimORPAType == 'Institutional') {
                        document.getElementById('<%= lblServDetailsProcCode.ClientID%>').innerHTML = result;
                        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblServDetailsProcCode').style.visibility = "visible";
                    }
                    if (claimORPAType == 'Dental') {
                        document.getElementById('<%= lblDentalProcMessage.ClientID%>').innerHTML = result;
                        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalProcMessage').style.visibility = "visible";
                    }
                    if (claimORPAType == 'Professional') {
                        document.getElementById('<%= lblProfProcMessage.ClientID%>').innerHTML = result;
                        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblProfProcMessage').style.visibility = "visible";
                    }

                    return false;

                } else { validateField = true; }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMessageonTop]").text(result);
                validateField = "false";
            }
        });

    }
    function prepareDentalServDetails() {

        var procedureCode = $("#<%= txtDentalSDProcCode.ClientID %>").first().val();
        var procCodeDesc = $("#<%= txtDentalProcCodeDescription.ClientID %>").first().val();
        var providerServiceNote = $("#<%= txtDentalProvServnote.ClientID %>").first().val();
        var toothNumber = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalToothNumber').value;
        var prosthesisNum = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalProsthsis').value;
        var cavity1 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity1').value;

        let cavity2;
        var cav2 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity2');
        if (cav2 != null) {
            cavity2 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity2').value;
        }

        let cavity3;
        var cav3 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity3');
        if (cav3 != null) {
            cavity3 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity3').value;
        }

        let cavity4;
        var cav4 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity4');
        if (cav4 != null) {
            cavity4 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity4').value;
        }

        let cavity5
        var cav5 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity5');
        if (cav5 != null) {
            cavity5 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity5').value;
        }

        let surface2;
        let surface3;
        let surface4;
        let surface5;

        var surface1 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface1').value;

        var surf2 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface2');
        if (surf2 != null) {
            surface2 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface2').value;
        }

        var surf3 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface3');
        if (surf3 != null) {
            surface3 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface3').value;
        }

        var surf4 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface4');
        if (surf4 != null) {
            surface4 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface4').value;
        }

        var surf5 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface5');
        if (surf5 != null) {
            surface5 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface5').value;
        }

        var reqUnits = $("#<%= txtDentalReqUnits.ClientID %>").first().val();
        var reqUnitsFee = $("#<%= txtDentalReqDollars.ClientID %>").first().val();
        var txtReqFDOS = $("#<%= txtDentalReqFDOS.ClientID %>").first().val();
        var txtReqTDOS = $("#<%= txtDentalReqTDOS.ClientID %>").first().val();
        var serviceTrackingNo = $("#<%= txtDentalServTrackingNo.ClientID %>").first().val();
        var remainingUnits = $("#<%= txtDentalRemainingUnits.ClientID %>").first().val(); //
        var txtDentalServDetailsStatus = $("#<%= txtstatus2.ClientID %>").first().val();


        var servDetails = {

            procedureCode: procedureCode,
            procCodeDesc: procCodeDesc,
            providerServiceNote: providerServiceNote,
            reqUnits: reqUnits,
            remainUnits: remainingUnits,
            requnitsfee: reqUnitsFee,
            fdos: txtReqFDOS,
            tdos: txtReqTDOS,
            toothNum: toothNumber,
            prosthesisNum: prosthesisNum,
            cavity1: cavity1,
            cavity2: cavity2,
            cavity3: cavity3,
            cavity4: cavity4,
            cavity5: cavity5,
            surface1: surface1,
            surface2: surface2,
            surface3: surface3,
            surface4: surface4,
            surface5: surface5,
            serviceTrackingNo: serviceTrackingNo,
            status: txtDentalServDetailsStatus
        }
        return servDetails;
    }

    function EditDentalServiceDetails(Id) {
        //Cancel the Edit Panel
        //Clear all controls for the panel

        cancelDentalLineAdd();

        var rfvtxtDentalSDProcCode = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtDentalSDProcCode");
        if (typeof (rfvtxtDentalSDProcCode) != 'undefined' && rfvtxtDentalSDProcCode != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtDentalSDProcCode').style.display = "none";
        }
        var rfvtxtDentalReqUnits = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtDentalReqUnits");
        if (typeof (rfvtxtDentalReqUnits) != 'undefined' && rfvtxtDentalReqUnits != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_rfvtxtDentalReqUnits').style.display = "none";
        }
        var RequiredFieldValidator4 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator4");
        if (typeof (RequiredFieldValidator4) != 'undefined' && RequiredFieldValidator4 != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator4').style.display = "none";
        }
        var RequiredFieldValidator5 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator5");
        if (typeof (RequiredFieldValidator5) != 'undefined' && RequiredFieldValidator5 != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator5').style.display = "none";
        }
        var RequiredFieldValidator6 = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator6");
        if (typeof (RequiredFieldValidator6) != 'undefined' && RequiredFieldValidator6 != null) {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredFieldValidator6').style.display = "none";
        }
        var RequiredRangeValidator = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredRangeValidator");
        if (RequiredRangeValidator.style.visibility == "hidden") {
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_RequiredRangeValidator').style.display = "none";
        }
        var Medicaid = getParameterByName('MedicaidNumber');
        var ClaimType = $(".rblClaimType input:checked").val();

        document.getElementById('btnServDentalUpdate').value = "Update";
        var linkId = document.getElementById('<%=tempUniqueId.ClientID%>').value;
        var ClaimType = $(".rblClaimType input:checked").val();
        var patype;
        if (ClaimType == 'dental') {
            patype = 0;
        }
        else if (ClaimType == 'Professional') {
            patype = 2;
        }
        else {
            patype = 1;
        }
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetDentalServiceDetail?lineNumber=" + Id + "&&MedicaidId=" + Medicaid + "&&patype=" + patype + "&&linkId=" + linkId,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                const dateStringFDOS = result.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS;

                if (dateStringFDOS != '' && dateStringFDOS != null && dateStringFDOS != undefined && dateStringFDOS.length > 0) {
                    const D = new Date(dateStringFDOS);
                    var FDOSDate = getFormattedDate(D);
                    $("[id*=txtDentalReqFDOS]").val(FDOSDate);
                }
                else {
                    $("[id*=txtDentalReqFDOS]").val('');
                }

                const dateStringTDOS = result.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS;

                if (dateStringTDOS != '' && dateStringTDOS != null && dateStringTDOS != undefined && dateStringTDOS.length > 0) {
                    const D = new Date(dateStringTDOS);
                    var TDOSDate = getFormattedDate(D);
                    $("[id*=txtDentalReqTDOS]").val(TDOSDate);
                }
                else {
                    $("[id*=txtDentalReqTDOS]").val('');
                }

                const dateStringAFDOS = result.PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS;
                // alert(dateStringAFDOS);
                if (dateStringAFDOS != '' && dateStringAFDOS != null && dateStringAFDOS != undefined && dateStringAFDOS.length > 0) {
                    const D = new Date(dateStringAFDOS);
                    var AFDOSDate = getFormattedDate(D);
                    $("[id*=txtDentalAuthFDOS]").val(AFDOSDate);
                }
                else {
                    $("[id*=txtDentalAuthFDOS]").val('');
                }

                const dateStringATDOS = result.PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS;
                // alert(dateStringATDOS);
                if (dateStringATDOS != '' && dateStringATDOS != null && dateStringATDOS != undefined && dateStringATDOS.length > 0) {
                    const D = new Date(dateStringATDOS);
                    var ATDOSDate = getFormattedDate(D);
                    $("[id*=txtDentalAuthTDOS]").val(ATDOSDate);
                }
                else {
                    $("[id*=txtDentalAuthTDOS]").val('');
                }
                $("[id*=hdDentalLineNumber]").val(Id);


                if (result.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS != null && result.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS > 0) {
                    $("[id*=txtDentalReqUnits]").val(result.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_UNITS);
                }
                else {
                    $("[id*=txtDentalReqUnits]").val('');
                }


                if (result.PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS != null && result.PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS > 0) {
                    $("[id*=txtDentalAuthUnits]").val(result.PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_UNITS);
                }
                else {
                    $("[id*=txtDentalAuthUnits]").val('');
                }




                if (result.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR != null && result.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR > 0) {
                    $("[id*=txtDentalReqDollars]").val(result.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUEST_DOLLAR);
                }
                else {
                    $("[id*=txtDentalAuthDollars]").val('');
                }


                if (result.PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR != null && result.PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR > 0) {
                    $("[id*=txtDentalAuthDollars]").val(result.PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_DOLLAR);
                }
                else {
                    $("[id*=txtDentalAuthDollars]").val('');
                }

                $("[id*=txtDentalReqFDOS]").val(result.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_FDOS);
                $("[id*=txtDentalReqTDOS]").val(result.PRIOR_AUTH_DENTALSERVICE_DETAIL_REQUESTED_TDOS);
                //$("[id*=txtDentalAuthFDOS]").val(result.d.PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_FDOS);
                //$("[id*=txtDentalAuthTDOS]").val(result.d.PRIOR_AUTH_DENTALSERVICE_DETAIL_AUTHORIZED_TDOS);

                $("[id*=txtDentalSDProcCode]").val(result.PRIOR_AUTH_PROCEDURE_CODE_ID);

                if (result.PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS != null || result.PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS > 0) {
                    $("[id*=txtDentalRemainingUnits]").val(result.PRIOR_AUTH_SERVICE_DETAIL_REAMING_UNITS);
                }
                else {
                    $("[id*=txtDentalRemainingUnits]").val('');
                }

                if (result.StatusDesc != 'Approved') {
                    $("[id*=txtDentalAuthFDOS]").val('');
                    $("[id*=txtDentalAuthTDOS]").val('');

                    $("[id*=txtDentalAuthDollars]").val('');
                    $("[id*=txtDentalAuthUnits]").val('');
                    $("[id*=txtDentalRemainingUnits]").val('');
                }
                $("[id*=txtDentalServTrackingNo]").val(result.PRIOR_AUTH_DENTALSERVICE_DETAIL_TRACKING_NUM);
                $("[id*=txtDentalProcCodeDescription]").val(result.PRIOR_AUTH_DENTALSERVICE_DETAIL_PROCEDURE_DESC);
                $("[id*=txtDentalProvServnote]").val(result.PRIOR_AUTH_DENTALSERVICE_DETAIL_PROVIDER_SERVICE_NOTE);
                $("[id*=txtDentalServDetailsStatus]").val(result.StatusDesc);


                var ddDentalToothNumber = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalToothNumber');
                var teethNumber = result.PRIOR_AUTH_TOOTH_NUMBER_ID;
                if (teethNumber != '') {
                    for (var i = 0; i < ddDentalToothNumber.options.length; i++) {
                        if (ddDentalToothNumber.options[i].value == teethNumber) {
                            ddDentalToothNumber.options[i].selected = true;
                        }
                    }
                }
                var ddDentalOralCavity1 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity1');
                var cavity1 = result.PRIOR_AUTH_DENTAL_ORAL_CAVITY1_MMIS;
                if (cavity1 != '') {
                    for (var i = 0; i < ddDentalOralCavity1.options.length; i++) {
                        if (ddDentalOralCavity1.options[i].value == cavity1) {
                            ddDentalOralCavity1.options[i].selected = true;
                        }
                    }
                }
                var ddDentalOralCavity2 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity2');
                if (ddDentalOralCavity2 != null || ddDentalOralCavity2 != '' || ddDentalOralCavity2 != undefined) {
                    var cavity2 = result.PRIOR_AUTH_DENTAL_ORAL_CAVITY2_MMIS;
                    if (cavity2 != '') {
                        for (var i = 0; i < ddDentalOralCavity2.options.length; i++) {
                            if (ddDentalOralCavity2.options[i].value == cavity2) {
                                ddDentalOralCavity2.options[i].selected = true;
                            }
                        }
                    }
                }

                var ddDentalOralCavity3 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity3');
                if (ddDentalOralCavity3 != null || ddDentalOralCavity3 != '' || ddDentalOralCavity3 != undefined) {
                    var cavity3 = result.PRIOR_AUTH_DENTAL_ORAL_CAVITY3_MMIS;
                    if (cavity3 != '') {
                        for (var i = 0; i < ddDentalOralCavity3.options.length; i++) {
                            if (ddDentalOralCavity3.options[i].value == cavity3) {
                                ddDentalOralCavity3.options[i].selected = true;
                            }
                        }
                    }
                }

                var ddDentalOralCavity4 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity4');
                if (ddDentalOralCavity4 != null || ddDentalOralCavity4 != '' || ddDentalOralCavity4 != undefined) {
                    var cavity4 = result.PRIOR_AUTH_DENTAL_ORAL_CAVITY4_MMIS;
                    if (cavity4 != '') {
                        for (var i = 0; i < ddDentalOralCavity4.options.length; i++) {
                            if (ddDentalOralCavity4.options[i].value == cavity4) {
                                ddDentalOralCavity4.options[i].selected = true;
                            }
                        }
                    }
                }

                var ddDentalOralCavity5 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalOralCavity5');
                if (ddDentalOralCavity5 != null || ddDentalOralCavity5 != '' || ddDentalOralCavity5 != undefined) {
                    var cavity5 = result.PRIOR_AUTH_DENTAL_ORAL_CAVITY5_MMIS;
                    if (cavity5 != '') {
                        for (var i = 0; i < ddDentalOralCavity5.options.length; i++) {
                            if (ddDentalOralCavity5.options[i].value == cavity5) {
                                ddDentalOralCavity5.options[i].selected = true;
                            }
                        }
                    }
                }

                var ddToothSurface1 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface1');
                if (ddToothSurface1 != null || ddToothSurface1 != '' || ddToothSurface1 != undefined) {
                    var Surface1 = result.PRIOR_AUTH_DENTALTOOTH_SURFACE1_CODE;
                    if (Surface1 != '') {
                        for (var i = 0; i < ddToothSurface1.options.length; i++) {
                            if (ddToothSurface1.options[i].value == Surface1) {
                                ddToothSurface1.options[i].selected = true;
                            }
                        }
                    }
                }

                var ddToothSurface2 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface2');
                if (ddToothSurface2 != null || ddToothSurface2 != '' || ddToothSurface2 != undefined) {
                    var Surface2 = result.PRIOR_AUTH_DENTALTOOTH_SURFACE2_CODE;
                    if (Surface2 != '') {
                        for (var i = 0; i < ddToothSurface2.options.length; i++) {
                            if (ddToothSurface2.options[i].value == Surface2) {
                                ddToothSurface2.options[i].selected = true;
                            }
                        }
                    }
                }
                var ddToothSurface3 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface3');
                if (ddToothSurface3 != null || ddToothSurface3 != '' || ddToothSurface3 != undefined) {
                    var Surface3 = result.PRIOR_AUTH_DENTALTOOTH_SURFACE3_CODE;
                    if (Surface3 != '') {
                        for (var i = 0; i < ddToothSurface3.options.length; i++) {
                            if (ddToothSurface3.options[i].value == Surface3) {
                                ddToothSurface3.options[i].selected = true;
                            }
                        }
                    }
                }
                var ddToothSurface4 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface4');
                if (ddToothSurface4 != null || ddToothSurface4 != '' || ddToothSurface4 != undefined) {
                    var Surface4 = result.PRIOR_AUTH_DENTALTOOTH_SURFACE4_CODE;
                    if (Surface4 != '') {
                        for (var i = 0; i < ddToothSurface4.options.length; i++) {
                            if (ddToothSurface4.options[i].value == Surface4) {
                                ddToothSurface4.options[i].selected = true;
                            }
                        }
                    }
                }

                var ddToothSurface5 = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddToothSurface5');
                if (ddToothSurface5 != null || ddToothSurface5 != '' || ddToothSurface5 != undefined) {
                    var Surface5 = result.PRIOR_AUTH_DENTALTOOTH_SURFACE5_CODE;
                    if (Surface5 != '') {
                        for (var i = 0; i < ddToothSurface5.options.length; i++) {
                            if (ddToothSurface5.options[i].value == Surface5) {
                                ddToothSurface5.options[i].selected = true;
                            }
                        }
                    }
                }

                var crownInLay = '';
                if (result.PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID != null & result.PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID != '' & result.PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID != undefined) {
                    crownInLay = result.PRIOR_AUTH_DENTALPROSTHESIS_CROWN_INLAY_ID;
                }

                var ddDentalProsthsis = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_ddDentalProsthsis');

                for (var i = 0; i < ddDentalProsthsis.options.length; i++) {
                    if (ddDentalProsthsis.options[i].value == crownInLay) {
                        ddDentalProsthsis.options[i].selected = true;
                    }
                }

                document.getElementById("dvSDDentalAddparent").style.height = "auto";
                document.getElementById("dvSDDentalAddparent").style.overflow = "visible";
                document.getElementById("dvSDDentalAddparent").style.visibility = "inherit";
                //if (document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_btnUpdate') != null) {
                //    document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_btnUpdate').style.display = "inline";
                //}               
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(JSON.stringify(jqXHR));
            }
        });

        if (document.getElementById('<%=txtstatus2.ClientID%>').value == "Approved" ||
            document.getElementById('<%=txtstatus2.ClientID%>').value == "Closed") {
            document.getElementById('btnServDentalUpdate').style.visibility = 'hidden';
            document.getElementById('btnServDentalUpdate').disabled = true;
        }
    }

    /*RECIPIENT INFORMATION*/

    function ClearRecipientInfo() {
        $("[id*=txtfrstmi2]").val('');
        $("[id*=txtLastName2]").val('');
        $("[id*=txtGender2]").val('');
        $("[id*=txtAddress1]").val('');
        $("[id*=lblAddress2]").val('');
        $("[id*=txtCity]").val('');
        $("[id*=txtState]").val('');
        $("[id*=txtZipCode]").val('');
        $("[id*=txtMiddleName]").val('');
    }

    function ValidateBirthDate() {
        var txtBirthDate = $("#<%= txtBirthDate.ClientID %>").first().val();

        if (txtBirthDate == null || txtBirthDate == '' || txtBirthDate == undefined) {
            document.getElementById('<%= lblDOBErrMsg.ClientID%>').innerHTML = 'Recipient date of birth is required.';
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDOBErrMsg').style.visibility = "visible";
            return false;
        }
        else if (isValidDate(txtBirthDate)) {
            return true;
        }
        return false;
    }

    function isValidDate(dateObject) {
        return new Date(dateObject).toString() !== 'Invalid Date';
    }

    function ValidateMedicaidBillingNumber() {
        var txtMedicaidBillingNumber = $("#<%= txtMedicaidBillingNumber.ClientID %>").first().val();

        if (txtMedicaidBillingNumber == null || txtMedicaidBillingNumber == '' || txtMedicaidBillingNumber == undefined) {
            document.getElementById('<%= lblMBErrorMSG.ClientID%>').innerHTML = '*12-digit number is required';
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblMBErrorMSG').style.visibility = "visible";
            return false;
        }
        else if (txtMedicaidBillingNumber.length != 12) {
            document.getElementById('<%= lblMBErrorMSG.ClientID%>').innerHTML = '*12-digit number is required';
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblMBErrorMSG').style.visibility = "visible";
            return false;
        }
        return true;
    }

    function getRecipientDetails() {
        $("[id*=hdnRecipientCallStatus]").val("false");
        document.getElementById('divpnlRecipientLoader').style.visibility = "visible";
        document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = '';
        document.getElementById('<%= lblMBErrorMSG.ClientID%>').innerHTML = '';
        document.getElementById('<%= lblDOBErrMsg.ClientID%>').innerHTML = '';
        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblErrorMsg').style.visibility = "hidden";
        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblMBErrorMSG').style.visibility = "hidden";
        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDOBErrMsg').style.visibility = "hidden";

        ClearRecipientInfo();
        var billingNumber = $("#<%= txtMedicaidBillingNumber.ClientID %>").first().val();
        var dob = $("#<%= txtBirthDate.ClientID %>").first().val();

        if (billingNumber == null || billingNumber == '' || billingNumber == undefined) {
            document.getElementById('divpnlRecipientLoader').style.visibility = "hidden";
            return false;
        }
        if (dob == null || dob == '' || dob == undefined) {
            document.getElementById('divpnlRecipientLoader').style.visibility = "hidden";
            return false;
        }
        if (!ValidateMedicaidBillingNumber()) {
            document.getElementById('divpnlRecipientLoader').style.visibility = "hidden";
            return false;
        }
        if (!ValidateBirthDate()) {
            document.getElementById('divpnlRecipientLoader').style.visibility = "hidden";
            return false;
        }

        var Medicaid = getParameterByName('MedicaidNumber');
        var ClaimType = $(".rblClaimType input:checked").val();
        var mbPrev = $("[id*=txtMedicaidBillingNumberPrevious]").val();
        var dobPrev = $("[id*=txtBirthDatePrevious]").val();


        $("[id*=hdnModified]").val("true");

        var APIToken = $("[id*=hdnAccessToken]").val();
        var username = $("[id*=hdnUserName]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetPARecipientInformation?medicaidId=" + Medicaid + "&&Username=" + username + "&&medicaidbillingnumber=" + billingNumber + "&&dateofBirth=" + dob + "&&medicalBillingPrev=" + mbPrev + "&&dateofBirthPrev=" + dobPrev,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var recipientInfo = result;
                if (recipientInfo !== null && recipientInfo.Errors === null) {

                    $("[id*=txtMedicaidBillingNumberPrevious]").val(billingNumber);
                    $("[id*=txtBirthDatePrevious]").val(dob);
                    $("[id*=txtLastName2]").val(recipientInfo.LastName);
                    $("[id*=txtfrstmi2]").val(recipientInfo.FirstName);
                    $("[id*=txtGender2]").val(recipientInfo.Gender);
                    $("[id*=txtAddress1]").val(recipientInfo.AddressLine1);
                    $("[id*=lblAddress2]").val(recipientInfo.AddressLine2);

                    if (recipientInfo.FirstName != null && recipientInfo.LastName != null && recipientInfo.AddressLine1 != null) {
                        $("[id*=hdnRecipientCallStatus]").val("true");
                    }

                    if (recipientInfo.MiddleName != null && recipientInfo.MiddleName != '' && recipientInfo.MiddleName != undefined)
                        $("[id*=txtMiddleName]").val(recipientInfo.MiddleName);
                    else
                        $("[id*=txtMiddleName]").val('');

                    $("[id*=txtCity]").val(recipientInfo.City);
                    $("[id*=txtState]").val(recipientInfo.StateCode);

                    $("[id*=txtZipCode]").val(recipientInfo.ZipCode5);
                    document.getElementById('divpnlRecipientLoader').style.visibility = "hidden";
                }
                else if (recipientInfo != null && recipientInfo.Errors != null && recipientInfo.Errors.length > 0) {
                    var errorMessage = '';
                    for (var i = 0; i < recipientInfo.Errors.length; i++) {
                        errorMessage = errorMessage + ";" + recipientInfo.Errors[i].Code + " : " + recipientInfo.Errors[i].Description;
                    }
                    document.getElementById('divpnlRecipientLoader').style.visibility = "hidden";
                    document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = errorMessage;
                    document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblErrorMsg').style.visibility = "visible";
                    $("[id*=hdnRecipientCallStatus]").val("false");
                }
                else {
                    document.getElementById('divpnlRecipientLoader').style.visibility = "hidden";
                    document.getElementById('<%= lblErrorMsg.ClientID%>').innerHTML = 'Error: An error occurred while processing the request';
                    document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblErrorMsg').style.visibility = "visible";
                    $("[id*=hdnRecipientCallStatus]").val("false");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                document.getElementById('divpnlRecipientLoader').style.visibility = "hidden";
                console.log(JSON.stringify(jqXHR));
            }
        });
    }

</script>
<script type="text/javascript">
    $(document).ready(function () {
        $('#tableData').paging({ limit: 5 });
    });
</script>
<style type="text/css">
    .failureNotification_PA {
        color: #D10000 !important;
        display: block !important;
    }

    table {
        background: #fff;
    }

    .paging-nav {
        text-align: right;
        padding-top: 2px;
    }

        .paging-nav a {
            margin: auto 1px;
            text-decoration: none;
            display: inline-block;
            padding: 1px 7px;
            background: #91b9e6;
            color: white;
            border-radius: 3px;
        }

        .paging-nav .selected-page {
            background: #187ed5;
            font-weight: bold;
        }

    .paging-nav,
    #tableData {
        width: 400px;
        margin: 0 auto;
        font-family: Arial, sans-serif;
    }
</style>

<div class="row">
    <asp:HiddenField ID="hdnUserName" runat="server" />
    <asp:HiddenField ID="tempUniqueId" runat="server" />
    <asp:HiddenField ID="hdnModified" runat="server" />
    <asp:HiddenField ID="hdnRecipientCallStatus" runat="server" Value="false" />
    <asp:HiddenField ID="hdnPriorAuthAttachments" runat="server" />

    <asp:HiddenField ID="hdnServiceDetailEdited" runat="server" />
    <asp:HiddenField ID="hdnTime" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="tradingPartnerID" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="txtDestinationpayerID" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnMedID" runat="server" />
    <asp:HiddenField ID="hdnProviderTypeId" runat="server" />
    <div class="col-sm-6" style="display: flex; align-content: flex-end; justify-content: flex-end; align-items: flex-end; flex-wrap: wrap;">
        <div style="text-align: center; margin-top: 20px; width: 430px">
            <label>Prior&nbsp;Authorization&nbsp;Type</label>
            <asp:RadioButtonList ID="rblClaimType" CssClass="radioButtonList rblClaimType" aria-label="Prior Authorization Type"
                runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                OnSelectedIndexChanged="rblClaimType_SelectedIndexChanged" Width="425px" RepeatLayout="Table" Style="background: none;">
                <asp:ListItem Selected="False" Text="Dental" Value="dental"></asp:ListItem>
                <asp:ListItem Selected="False" Text="Professional" Value="Professional"></asp:ListItem>
                <asp:ListItem Selected="False" Text="Institutional" Value="Institutional"></asp:ListItem>
            </asp:RadioButtonList>

        </div>

    </div>
    <div class="col-sm-6 text-left" id="PAInfo" runat="server">
        <div class="row" style="padding-top: 0.1in;">
            <div class="col-sm-12 text-right">
                <span class="formLabel200">PA Status:</span>
                <asp:TextBox ID="txtstatus2" runat="server" aria-label="PA Status:" CssClass="formField_264" ReadOnly="true" />
            </div>
            <div class="col-sm-12 text-right">
                <span class="formLabel200">PA Number:</span>
                <asp:TextBox ID="txtPANumber2" runat="server" aria-label="PA Number" CssClass="formField_264" ReadOnly="true" />
            </div>
            <div class="col-sm-12 text-right">
                <span class="formLabel200">PA Submission Date:</span>
                <asp:TextBox ID="txtPACreation2" runat="server" aria-label="PA subission date" CssClass="formField_264" ReadOnly="true" />
            </div>
            <div class="col-sm-12 text-right">
                <span class="formLabel200">PA Effective Date:</span>
                <asp:TextBox ID="txtEffDate2" runat="server" aria-label="PA Effective date" CssClass="formField_264" ReadOnly="true" />
            </div>
            <div class="col-sm-12 text-right">
                <span class="formLabel200">PA Expiration Date:</span>
                <asp:TextBox ID="txtExpDate2" runat="server" aria-label="PA expiration date" CssClass="formField_264" ReadOnly="true" />
            </div>
        </div>
    </div>
</div>

<div id="divErrorList" class="row" style="margin-left: 11px; max-height: 23rem; height: auto; overflow-y: scroll; overflow-x: hidden;">
    <div id="MainErrorContent">
        <asp:ValidationSummary ID="valSummary" DisplayMode="List" runat="server" CssClass="failureNotification_PA" ValidationGroup="valProviderInfoHeader" />
    </div>
</div>

<asp:HiddenField ID="hdnFrontEndEdits" runat="server" />
<asp:HiddenField ID="hdnFrontEndEdits_6" runat="server" />
<asp:HiddenField ID="hdnFrontEndEdits_7" runat="server" />
<asp:HiddenField ID="hdnPAInquiryTrn" runat="server" />

<div id="divSubmitPriorAuth" runat="server" visible="false">
    <span style="color: #CC0505; font-size: 14pt !important; font-weight: 100 !important;">An asterisk * indicates a required field</span>


    <asp:UpdatePanel ID="updatepanelddl" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-4 text-left">
                    <asp:Label ID="lblpriorautherror" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6 col-lg-6">
                    <div class="row">
                        <div class="col-lg-5">
                            <span class="ohio-field" style="font-size: 15px; text-align: right; font-weight: bold;"><span style="color: #CC0505">*</span>Destination Payer Name:</span>
                        </div>
                        <div class="col-lg-7">
                            <span style="text-align: left;">
                                <asp:DropDownList ID="ddlAuthorization" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="false"
                                    AppendDataBoundItems="True" onchange="javascript:OnDestinationPayerNameIndexChanged(this);">
                                </asp:DropDownList>
                                <button id="btnPAloading1" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 col-lg-6">
                    <div class="row">
                        <div class="col-lg-5">
                            <span class="ohio-field" style="font-size: 15px; text-align: right; font-weight: bold;"><span style="color: #CC0505">*</span>Destination Payer ID:</span>
                        </div>
                        <div class="col-lg-7">
                            <span style="text-align: left;">
                                <asp:DropDownList ID="ddlSubCapitaPayerIDs" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="false"
                                    AppendDataBoundItems="True" onchange="OnPopulatetxtDestinationpayerID(this); javascript:validateCSS(this);">
                                </asp:DropDownList>

                            </span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6 col-lg-6">
                    <div class="row">
                        <div class="col-lg-5">
                            <span class="ohio-field" style="font-size: 15px; text-align: right; font-weight: bold;"><span style="color: #CC0505">*</span>Assignment:</span>
                        </div>
                        <div class="col-lg-7">
                            <span style="text-align: left;">
                                <asp:DropDownList ID="ddlAssignment" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="false"
                                    AppendDataBoundItems="True" OnSelectedIndexChanged="ddlAssignment_SelectedIndexChanged" onchange="javascript:assignmentchanged(this);">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvAssignment" runat="server" ControlToValidate="ddlAssignment" ErrorMessage="*" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 col-lg-6">
                    <div class="row">
                        <div class="col-lg-5">
                            <span class="ohio-field" style="font-size: 15px; text-align: right; font-weight: bold;"><span style="color: #CC0505">*</span>Service Type:</span>
                        </div>
                        <div style="left: -1px" class="col-lg-7">
                            <span style="text-align: left;">
                                <asp:DropDownList ID="ddlServiceType" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="false"
                                    AppendDataBoundItems="true" onchange="javascript:validateCSS(this);" OnSelectedIndexChanged="ddlServiceType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>

        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlAuthorization" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

    <div class="row" style="display: none">

        <div class="col-sm-19 col-md-16 col-lg-12 text-right">
            <asp:Label ID="lblPAStatus" class="formLabel200" Text="" runat="server" />
            <span class="formLabel200">PA Status:<span></span>
            </span>
            <asp:Label ID="lblstatus2" class="formLabelAuto" Text="" runat="server" />
            <span class="formLabel200"><span></span>
            </span>
        </div>
        <div class="col-sm-19 col-md-16 col-lg-12 text-right">
            <asp:Label ID="lblPANumber" class="formLabel200" Text="" runat="server" />
            <span class="formLabel200">PA Number:<span></span>
            </span>
            <asp:Label ID="lblPANumber2" class="formLabelAuto" Text="" runat="server" />
            <span class="formLabel200"><span></span>
            </span>
        </div>
        <div class="col-sm-19 col-md-16 col-lg-12 text-right">
            <asp:Label ID="lblPACreation" class="formLabel200" Text="" runat="server" />
            <span class="formLabel200">PA Submission Date:<span></span>
            </span>
            <asp:Label ID="lblPACreation2" class="formLabelAuto" Text="" runat="server" />
            <span class="formLabel200"><span></span>
            </span>
        </div>
        <div class="col-sm-19 col-md-16 col-lg-12 text-right">
            <asp:Label ID="lblEffDate" class="formLabel200" Text="" runat="server" />
            <span class="formLabel200">PA Effective Date:<span></span>
            </span>
            <asp:Label ID="lblEffDate2" class="formLabelAuto" Text="" runat="server" />
            <span class="formLabel200"><span></span>
            </span>
        </div>
        <div class="col-sm-19 col-md-16 col-lg-12 text-right">
            <asp:Label ID="lblExpDate" class="formLabel200" Text="" runat="server" />
            <span class="formLabel200">PA Expiry Date:<span></span>
            </span>
            <asp:Label ID="lblExpDate2" class="formLabelAuto" Text="" runat="server" />
            <span class="formLabel200"><span></span>
            </span>
        </div>
    </div>
    <asp:HiddenField ID="RegIdTxt" runat="server" />
    <asp:HiddenField ID="hdnTrackingNo" runat="server" />
    <div class="row col-sm-15" style="border: groove;">
        <asp:UpdatePanel ID="updatevalidatepacle" runat="server">
            <ContentTemplate>
                <asp:ValidationSummary ID="valerrormess" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valProviderHeader" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upRecipientInfo" runat="server">
            <ContentTemplate>
                <ajax:collapsiblepanelextender id="cpeRecipient" runat="server" collapsed="false" targetcontrolid="pnlRecipient"
                    expandcontrolid="pnlsepRecipient" collapsecontrolid="pnlsepRecipient"
                    expandedtext="-" collapsedsize="0" scrollcontents="false"
                    collapsedtext="+" expanddirection="Vertical"
                    textlabelid="lblsepRecipient"
                    imagecontrolid="pnlSepRecImg" suppresspostback="true" />

                <asp:Panel runat="server" ID="pnlsepRecipient" class="CollapsingSeparator"
                    CssClass="CollapsingSeparator">

                    <asp:Label ID="lblsepRecipient" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                    <span runat="server" class="pageHeader pH2" style="color: #CC0505; padding-left: 0">*</span>
                    <span runat="server" tabindex="0" class="pageHeader pH2" style="padding-left: 0">Recipient Information</span>
                    <asp:ImageButton ID="btnRefreshRecipientInfo" runat="server" Style="float: right; margin: 2px; height: 21px" ImageUrl="../Images/refresh.png" OnClientClick="RefreshRecipientInfo();" ToolTip="Refresh" />
                </asp:Panel>

                <asp:Panel ID="pnlRecipient" runat="server" Style="min-width: 300px; padding: 0 15px; width: auto; max-width: 100%; overflow-x: hidden;">
                    <div>
                        <div id="divpnlRecipientLoader" style="visibility: hidden">
                            <img src='../Images/loader.gif' style='height: 38px; width: 35px;'>
                        </div>
                        <asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Style="margin-left: 20px;"></asp:Label>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 p-0">
                            <div class="row" id="lblMedicaidBillingNumber" runat="server">
                                <div class="col-sm-5">
                                    <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: #CC0505">*</span>Medicaid Billing Number</span>
                                </div>
                                <div class="col-sm-7">
                                    <span style="text-align: left;">
                                        <asp:TextBox ID="txtMedicaidBillingNumber" runat="server" CssClass="formField" MaxLength="12" Style="height: 30px; width: 200px" onchange="getRecipientDetails();"
                                            onkeypress="return onlyNumbers(event)" />
                                        <asp:RequiredFieldValidator ID="rfvMEDBillNum" runat="server" ControlToValidate="txtMedicaidBillingNumber" ErrorMessage="*Enter Medicaid Billing Number" Text="*"
                                            Display="Dynamic" ValidationGroup="valProviderHeader" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <asp:Label ID="lblMBErrorMSG" runat="server" Style="visibility: hidden" ForeColor="Red"></asp:Label>
                                        <button id="btnRecipientloading1" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading...</button>
                                        <asp:HiddenField ID="txtMedicaidBillingNumberPrevious" runat="server" />
                                    </span>
                                </div>
                            </div>
                            <div class="row" id="lblLastName" runat="server">
                                <div class="col-sm-5">
                                    <span class="ohio-field" style="font-size: 15px; text-align: right">Last Name</span>
                                </div>
                                <div class="col-sm-7">
                                    <span style="text-align: left;">
                                        <asp:TextBox ID="txtLastName2" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px" ReadOnly="true" />
                                    </span>
                                </div>
                            </div>
                            <div class="row" id="lblfrstmi" runat="server">
                                <div class="col-sm-5">
                                    <span class="ohio-field" style="font-size: 15px; text-align: right">First Name:</span>
                                </div>
                                <div class="col-sm-7">
                                    <span style="text-align: left;">
                                        <asp:TextBox ID="txtfrstmi2" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px" ReadOnly="true" />
                                    </span>
                                </div>
                            </div>
                            <div class="row" id="lblmiddleName" runat="server">
                                <div class="col-sm-5">
                                    <span class="ohio-field" style="font-size: 15px; text-align: right">Middle Name:</span>
                                </div>
                                <div class="col-sm-7">
                                    <span style="text-align: left;">
                                        <asp:TextBox ID="txtMiddleName" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px" ReadOnly="true" />
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4 p-0">
                            <div class="row" id="lblBirthDate" runat="server">
                                <div class="col-sm-5">
                                    <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: #CC0505">*</span>Date of Birth</span>
                                </div>
                                <div class="col-sm-7">
                                    <span style="text-align: left;">
                                        <asp:TextBox ID="txtBirthDate" runat="server" CssClass="formField" Style="height: 30px; width: 200px" onchange="getRecipientDetails();" />

                                        <ajax:calendarextender id="ceBirthdate" runat="server" format="MM/dd/yyyy" targetcontrolid="txtBirthDate"
                                            popupposition="BottomRight" popupbuttonid="imgBirthDate" />

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBirthDate"
                                            ErrorMessage="*Recipient date of birth is required" Text="*" Display="Dynamic" ValidationGroup="valProviderHeader" ForeColor="Red" />

                                        <asp:Label ID="lblDOBErrMsg" runat="server" Style="visibility: hidden" ForeColor="Red"></asp:Label>
                                        <asp:HiddenField ID="txtBirthDatePrevious" runat="server" />
                                    </span>
                                </div>
                            </div>
                            <div class="row" id="Div24" runat="server">
                                <div class="col-sm-5">
                                    <span class="ohio-field" style="font-size: 15px; text-align: right">Patient Tracking Number</span>
                                </div>
                                <div class="col-sm-7">
                                    <span style="text-align: left;">
                                        <asp:TextBox ID="txtPatientTrckNum" runat="server" CssClass="formField" Style="height: 30px; width: 200px" MaxLength="50" onchange="javascript:validateCSS(this);" />
                                        <asp:RequiredFieldValidator ID="RfvPatientTrckNum" runat="server" ControlToValidate="txtPatientTrckNum" ErrorMessage="*Patient Tracking Number is required to save your prior authorization" Text="*"
                                            Display="Dynamic" ValidationGroup="valProviderHeader" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </span>
                                </div>
                            </div>
                            <div class="row" id="lblGender" runat="server">
                                <div class="col-sm-5">
                                    <span class="ohio-field" style="font-size: 15px; text-align: right">Gender:</span>
                                </div>
                                <div class="col-sm-7">
                                    <span style="text-align: left;">
                                        <asp:TextBox ID="txtGender2" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px" ReadOnly="true" />
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-4 p-0">
                            <div class="row" id="Div23" runat="server">
                                <div class="col-sm-5">
                                    <span class="ohio-field" style="font-size: 15px; text-align: right">Address 1:</span>
                                </div>
                                <div class="col-sm-7">
                                    <span style="text-align: left;">
                                        <asp:TextBox ID="txtAddress1" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px" ReadOnly="true" />
                                    </span>
                                </div>
                            </div>
                            <div class="row" id="Div25" runat="server">
                                <div class="col-sm-5">
                                    <span class="ohio-field" style="font-size: 15px; text-align: right">Address 2:</span>
                                </div>
                                <div class="col-sm-7">
                                    <span style="text-align: left;">
                                        <asp:TextBox ID="lblAddress2" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px" ReadOnly="true" />
                                    </span>
                                </div>
                            </div>
                            <div class="row" id="Div26" runat="server">
                                <div class="col-sm-5">
                                    <span class="ohio-field" style="font-size: 15px; text-align: right">City :</span>
                                </div>
                                <div class="col-sm-7">
                                    <span style="text-align: left;">
                                        <asp:TextBox ID="txtCity" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px" ReadOnly="true" />
                                    </span>
                                </div>
                            </div>
                            <div class="row" id="Div27" runat="server">
                                <div class="col-sm-5">
                                    <span class="ohio-field" style="font-size: 15px; text-align: right">State :</span>
                                </div>
                                <div class="col-sm-7">
                                    <span style="text-align: left;">
                                        <asp:TextBox ID="txtState" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px" ReadOnly="true" />
                                    </span>
                                </div>
                            </div>
                            <div class="row" id="Div28" runat="server">
                                <div class="col-sm-5">
                                    <span class="ohio-field" style="font-size: 15px; text-align: right">Zip Code:</span>
                                </div>
                                <div class="col-sm-7">
                                    <span style="text-align: left;">
                                        <asp:TextBox ID="txtZipCode" runat="server" CssClass="formField" Style="background-color: lightgrey; height: 30px; width: 200px" ReadOnly="true" />
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6" style="display: none">
                        <div class="row" id="lblCBMM" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">*Covered by Medicaid Managed CarePlan?</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlCBMMXP" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlCBMMXP_SelectedIndexChanged" Style="height: 30px; width: 200px; min-width: 80px;">
                                        <asp:ListItem Value="1" Text=""></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="No" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="reqCBMMXP" SetFocusOnError="true"
                                        ValidationGroup="valProviderHeader" ControlToValidate="ddlCBMMXP" ErrorMessage="*Recipient is covered by managed care program" Text="*" Display="Dynamic" InitialValue="0" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6" style="display: none">
                        <div class="row" id="PlanName" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Plan Name</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlplanname" EnableViewState="true" runat="server" AutoPostBack="true"
                                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlplanname_SelectedIndexChanged" CssClass="formField" Style="height: 30px; width: 200px; min-width: 80px;">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="reqplanname" SetFocusOnError="true"
                                        ValidationGroup="valProviderHeader" ControlToValidate="ddlplanname" ErrorMessage="*Plan Name" Text="*" Display="Dynamic" InitialValue="0" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6" style="display: none">
                        <div class="row" id="AdmissionDate" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Admission Date</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtAdmissionDate" runat="server" CssClass="formField" Style="height: 30px; width: 200px" AutoPostBack="false" ValidationGroup="valProviderHeader" OnTextChanged="txtAdmissionDate_TextChanged" />
                                    <ajax:calendarextender id="ceAdmissionDate" runat="server" format="MM/dd/yyyy" targetcontrolid="txtAdmissionDate"
                                        popupposition="TopRight" popupbuttonid="imgBirthDate" enabledonclient="true" />

                                    <br />
                                    <asp:Label ID="lblAdmissionDateErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6" style="display: none">
                        <div class="row" id="SpecialIndecator" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Special Indicator</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlSpecialIndicator" EnableViewState="true" runat="server" AutoPostBack="true"
                                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlSpecialIndicator_SelectedIndexChanged" CssClass="formField" Style="height: 30px; width: 200px; min-width: 80px;">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvSpecialIndicator" SetFocusOnError="true"
                                        ValidationGroup="valProviderHeader" ControlToValidate="ddlSpecialIndicator" ErrorMessage="*Special Indicator" Text="*" Display="Dynamic" InitialValue="0" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6" style="display: none">
                        <div class="row" id="LTCFDischargedate" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">LTCF Discharge Date</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtDischargeDate" runat="server" CssClass="formField" Style="height: 30px; width: 200px" />
                                    <ajax:calendarextender id="ceDischargeDate" runat="server" format="MM/dd/yyyy" targetcontrolid="txtDischargeDate"
                                        popupposition="TopRight" cssclass="QstLTCCalendarCSS" popupbuttonid="imgBirthDate" enabledonclient="true" />
                                </span>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtMedicaidBillingNumber" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="txtBirthDate" EventName="TextChanged" />
            </Triggers>
        </asp:UpdatePanel>
        <div id="DivPriorAuth" runat="server">
            <!--REQUESTER CONTACT INFORMATION -->
            <ajax:collapsiblepanelextender id="cpeContact" runat="server" collapsed="false" targetcontrolid="pnlContact"
                expandcontrolid="pnlSepContact" collapsecontrolid="pnlSepContact"
                expandedtext="-" collapsedsize="0" scrollcontents="false" collapsedtext="+" expanddirection="Vertical"
                imagecontrolid="ImgpnlSepContact" suppresspostback="true" textlabelid="lblsepContact" />
            <asp:Panel runat="server" ID="pnlSepContact" class="CollapsingSeparator" CssClass="CollapsingSeparator">

                <asp:Label ID="lblsepContact" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                <span runat="server" class="pageHeader pH2" style="color: #CC0505; padding-left: 0">*</span>
                <span runat="server" tabindex="0" class="pageHeader pH2" style="padding-left:0">Requester Contact Information</span>
            </asp:Panel>
            <asp:Panel ID="pnlContact" runat="server" Style="width: auto; max-width: 100%; overflow-x: hidden;">

                <div class="row">
                    <div class="col-sm-6 col-lg-6">
                        <div class="row" id="ContactName" runat="server">
                            <div class="col-lg-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right;"><span style="color: #CC0505">*</span>Contact First Name</span>
                            </div>
                            <div class="col-lg-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtContactName" runat="server" CssClass="formField" Width="200" MaxLength="30" onkeypress="javascript:validateCSS(this);" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-lg-6">
                        <div class="row" id="ContactLastName" runat="server">
                            <div class="col-lg-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right;"><span style="color: #CC0505">*</span>Contact Last Name</span>
                            </div>
                            <div class="col-lg-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtContactLastName" runat="server" CssClass="formField" MaxLength="30" Width="200" onkeypress="javascript:validateCSS(this);" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6 col-lg-6">
                        <div class="row" id="Contactnumber" runat="server">
                            <div class="col-lg-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right;"><span style="color: #CC0505">*</span>Contact Number</span>
                            </div>
                            <div class="col-lg-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtContactNumber" runat="server" CssClass="formField" Width="200" MaxLength="15" onkeypress="javascript:validateCSS(this);" />
                                    <ajax:maskededitextender id="meeContactnoext" runat="server" autocomplete="False" clearmaskonlostfocus="False"
                                        cultureampmplaceholder="" culturecurrencysymbolplaceholder="" culturedateformat="" culturedateplaceholder=""
                                        culturedecimalplaceholder="" culturethousandsplaceholder="" culturetimeplaceholder=""
                                        enabled="True" mask="(999) 999-9999" masktype="Number" targetcontrolid="txtContactNumber" />
                                    <asp:RequiredFieldValidator runat="server" ID="rfvContactNumber" ValidationGroup="valProviderHeader" Style="white-space: nowrap"
                                        ControlToValidate="txtContactNumber" ErrorMessage="Contact Number is required" Text="Contact Number is required" Display="Dynamic"
                                        SetFocusOnError="true" InitialValue="(___) ___-____,___" ForeColor="Red" />
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                                        ControlToValidate="txtContactNumber" ClientValidationFunction="CheckPhoneLength" ForeColor="Red"
                                        ErrorMessage="Contact phone number is invalid" Text="Contact phone number is invalid" ValidationGroup="valProviderHeader" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-lg-6">
                        <div class="row" id="ContactExtension" runat="server">
                            <div class="col-lg-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right;">Contact Extension</span>
                            </div>
                            <div class="col-lg-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtExt" runat="server" CssClass="formField" Width="200" MaxLength="6" onkeypress="return onlyNumbers(event)" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!--SERVICE INFORMATION -->
            <ajax:collapsiblepanelextender id="cpeServiceInformation" runat="server" collapsed="false" targetcontrolid="pnlServiceInformation"
                expandcontrolid="pnlsepServiceInformation" collapsecontrolid="pnlsepServiceInformation"
                expandedtext="-" collapsedsize="0" scrollcontents="false" collapsedtext="+" expanddirection="Vertical"
                imagecontrolid="ImpnlsepServiceInformation" textlabelid="lblServiceInformation" suppresspostback="true" />
            <asp:Panel runat="server" ID="pnlsepServiceInformation" class="CollapsingSeparator" CssClass="CollapsingSeparator">
                <asp:Label ID="lblServiceInformation" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                <span runat="server" class="pageHeader pH2" style="color: #CC0505; padding-left: 0">*</span>
                <span runat="server" tabindex="0" class="pageHeader pH2" style="padding-left:0">Service Information</span>

            </asp:Panel>
            <asp:Panel ID="pnlServiceInformation" runat="server" Style="min-width: 300px; overflow-x: hidden; width: auto; max-width: 100%;">
                <div class="row" id="InstitService" runat="server">
                    <div class="col-sm-6">
                        <div class="row" id="facilityType" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: #CC0505">*</span>Facility Type</span>
                            </div>
                            <asp:UpdatePanel ID="upServiceInformation" runat="server">
                                <ContentTemplate>
                                    <div class="col-sm-4">
                                        <span style="text-align: left;">
                                            <asp:TextBox ID="txtfacilityType" runat="server" CssClass="formField" MaxLength="12" Style="height: 30px; width: 200px" EnableViewState="true"
                                                onblur="OnFacilityTypeTextChanged(false);" />
                                            <div id="divfacilityLoader"></div>
                                            <asp:Label ID="lblfacilityTypeError" runat="server" Visible="true" ForeColor="Red"></asp:Label>
                                        </span>
                                    </div>
                                    <div class="col-sm-2 text-left">
                                        <span style="text-align: left; font-size: 15px;">
                                            <asp:LinkButton ID="lnkFacilityType" runat="server" Text="Search" OnClientClick="return visibleFacilityType()" />
                                        </span>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="row" id="adminssionDate" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: #CC0505">*</span>Admission Date</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtFAdmissionDate" runat="server" CssClass="formField" ReadOnly="false" Style="height: 30px; width: 200px"
                                        OnTextChanged="txtFAdmissionDate_TextChanged" onchange="javascript:validateCSS(this);" AutoPostBack="false" />
                                    <ajax:calendarextender id="cetxtFAdmissionDate" targetcontrolid="txtFAdmissionDate" runat="server" popupposition="TopRight" format="MM/dd/yyyy" />

                                    <asp:Label ID="lblAdmissionErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                </span>
                                <asp:RequiredFieldValidator ID="reqValAdmission" runat="server" ControlToValidate="txtFAdmissionDate" Display="Dynamic" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="valProviderHeader">*</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row" id="dischargeDate" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Discharge Date:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtFDischargeDate" runat="server" CssClass="formField" Style="height: 30px; width: 200px"
                                        OnTextChanged="txtFDischargeDate_TextChanged" AutoPostBack="false" />
                                    <ajax:calendarextender id="cetxtFDischargeDate" targetcontrolid="txtFDischargeDate" popupposition="TopRight" runat="server" format="MM/dd/yyyy" />
                                    <asp:CompareValidator ID="cvtxtFDischargeDate" runat="server" ValidationGroup="valProviderHeader"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtFDischargeDate"
                                        ErrorMessage="Select a valid smaller date than today" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                    <asp:Label ID="lblDischargeErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <div class="row" id="dischargeStatus" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: #CC0505">*</span>Discharge Status:</span>
                            </div>
                            <div class="col-sm-4">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlDischargeStatus" CssClass="formField200" EnableViewState="true" runat="server" AutoPostBack="false"
                                        AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </span>
                                <asp:CustomValidator ID="PatientCustomValidator" runat="server" ErrorMessage="Patient discharge status is required" ControlToValidate="ddlDischargeStatus" ForeColor="Red">*</asp:CustomValidator>
                            </div>
                        </div>
                        <div class="row" id="lastMensPeriod" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Date of Last Menstrual Period:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtLstMensPeriod" runat="server" CssClass="formField" Style="height: 30px; width: 200px"
                                        OnTextChanged="txtLstMensPeriod_TextChanged" AutoPostBack="false" />
                                    <ajax:calendarextender id="cetxtLstMensPeriod" targetcontrolid="txtLstMensPeriod" cssclass="QstLTCCalendarCSS" popupposition="TopRight" runat="server" format="MM/dd/yyyy" />
                                    <asp:CompareValidator ID="cvtxtLstMensPeriod" runat="server" ValidationGroup="valProviderHeader"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtLstMensPeriod"
                                        ErrorMessage="Select a valid smaller date than today" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                    <br />
                                    <asp:Label ID="lblMensErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <div class="row" id="estDateBirth" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Estimated Date of Birth:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtEstBirthDate" runat="server" CssClass="formField" Style="height: 30px; width: 200px"
                                        OnTextChanged="txtEstBirthDate_TextChanged" AutoPostBack="false" />
                                    <ajax:calendarextender id="cetxtEstBirthDate" targetcontrolid="txtEstBirthDate" cssclass="QstLTCCalendarCSS" popupposition="TopRight" runat="server" format="MM/dd/yyyy" />
                                    <asp:CompareValidator ID="cvtxtEstBirthDate" runat="server" ValidationGroup="valProviderHeader"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEstBirthDate"
                                        ErrorMessage="Select a valid smaller date than today" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                    <br />
                                    <asp:Label ID="lblEstErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <div class="row" id="onsetIllness" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Date of Onset of Illness:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtOnsetIllness" runat="server" CssClass="formField" Style="height: 30px; width: 200px"
                                        OnTextChanged="txtOnsetIllness_TextChanged" AutoPostBack="false" />
                                    <ajax:calendarextender id="cetxtOnsetIllness" targetcontrolid="txtOnsetIllness" cssclass="QstLTCCalendarCSS" popupposition="TopRight" runat="server" format="MM/dd/yyyy" />
                                    <asp:CompareValidator ID="cvtxtOnsetIllness" runat="server" ValidationGroup="valProviderHeader"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtOnsetIllness"
                                        ErrorMessage="Select a valid smaller date than today" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                    <br />
                                    <asp:Label ID="lblIllnessErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <div class="row" id="accidentDt" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Accident Date:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left; position: relative">
                                    <asp:TextBox ID="txtAccidentDate" runat="server" CssClass="formField" AutoPostBack="false"
                                        Style="height: 30px; width: 200px" ValidationGroup="valOrgInfo" OnTextChanged="txtAccidentDate_TextChanged" />
                                    <ajax:calendarextender id="cetxtAccidentDate" targetcontrolid="txtAccidentDate" cssclass="cetxtEstDOBClass" runat="server" popupposition="TopRight" format="MM/dd/yyyy" />
                                    <asp:CompareValidator ID="cvtxtAccidentDate" runat="server" ValidationGroup="valOrgInfo"
                                        Type="Date" Operator="LessThan" ControlToValidate="txtAccidentDate"
                                        ErrorMessage="Select a valid smaller date than today" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                    <asp:Label ID="lblAccidentErrMsg" runat="server" Visible="true" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lvlOfService" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: #CC0505">*</span>Level Of Service:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlLevelService" CssClass="formField200" runat="server" onchange="javascript:validateCSS(this);">
                                        <asp:ListItem Text="" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="E - Elective" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="U - Urgent" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </div>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <div class="row" id="adminType" runat="server">
                                    <div class="col-sm-5">
                                        <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: #CC0505">*</span>Admission Type:</span>
                                    </div>

                                    <div class="col-sm-7">
                                        <span style="text-align: left;">
                                            <asp:DropDownList ID="ddlAdminType" CssClass="formField200" runat="server"
                                                AppendDataBoundItems="false" onchange="javascript:validateCSSwithAdminLoader(this);">
                                                <asp:ListItem Text="" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="1 - Emergency" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="2 - Urgent" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="3 - Elective" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="4 - Newborn" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="5 - Trauma" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="9 - Information Not Available" Value="9"></asp:ListItem>
                                            </asp:DropDownList>
                                        </span>
                                    </div>

                                </div>
                                <div class="row" id="adminSrc" runat="server">
                                    <div class="col-sm-5">
                                        <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: #CC0505">*</span>Admission Source:</span>
                                    </div>
                                    <div class="col-sm-7">
                                        <span style="text-align: left;">
                                            <asp:HiddenField ID="hdnAdminSrc" Value="" runat="server" />
                                            <asp:DropDownList ID="ddlAdminSrc" CssClass="formField200" runat="server" AutoPostBack="false" onchange="javascript:setadminsrcvalue(this);"
                                                AppendDataBoundItems="True">
                                            </asp:DropDownList>
                                        </span>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="row" id="dlyReason" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Delay Reason:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlDlyReason" CssClass="formField200" EnableViewState="false" runat="server" AutoPostBack="false"
                                        AppendDataBoundItems="false">
                                        <asp:ListItem Text="" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="1 - Proof of Eligibility Unknown or Unavailable" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2 - Litigation" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3 - Authorization Delays" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4 - Delay in Certifying Provider" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="7 - Third Party Processing Delay" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8 - Delay in Eligibility Determination" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="10 - Administration Delay in the Prior Approval Process" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11 - Other" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="15 - Natural Disaster" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="16 - Lack of Information" Value="16"></asp:ListItem>
                                        <asp:ListItem Text="17 - No Response to Initial Request" Value="17"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </div>
                        </div>
                        <div class="row" id="associatePAnum" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Associate PA No:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtPaNum" runat="server" CssClass="formField200" Style="height: 30px;" MaxLength="50" />
                                    <ajax:filteredtextboxextender id="FilteredTextBoxExtender1" runat="server" enabled="True" targetcontrolid="txtPaNum" filtertype="Numbers, UppercaseLetters, LowercaseLetters" filtermode="ValidChars"></ajax:filteredtextboxextender>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" id="ProfService" style="display: none" runat="server">
                    <div class="col-sm-6">
                        <div class="row" id="Service" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right"><span id="spanPlaceOfServiceIndicator" style="color: #CC0505">*</span>Place of Service:</span>
                            </div>
                            <asp:UpdatePanel ID="UpdPlaceOfServices" runat="server" class="col-sm-7">
                                <ContentTemplate>
                                    <div class="row" style="display: inline-flex;">
                                        <div class="col-sm-8">
                                            <span style="text-align: left;">
                                                <asp:TextBox ID="txtpalceofservice" runat="server" CssClass="formField200" MaxLength="12" Style="height: 30px;"
                                                    onchange="OnPlaceOfServiceTextChanged(false);" />
                                                <asp:RequiredFieldValidator ID="reqtxtpalceofservice" runat="server" ControlToValidate="txtpalceofservice" ErrorMessage="*Enter Place of service" Text="*"
                                                    Display="Dynamic" ValidationGroup="valProviderHeader"></asp:RequiredFieldValidator>
                                                <asp:Label ID="lblPlaceofErrorSearch" runat="server" Visible="true" ForeColor="Red"></asp:Label>
                                            </span>
                                        </div>
                                        <div class="col-sm-4 text-center">
                                            <span style="font-size: 15px;">
                                                <asp:LinkButton ID="lnkplaceofservicesrch" style="margin-left: 15px !important;" runat="server" Text="Search" OnClientClick="return visiblePlaceOfServiceDetails()" />
                                            </span>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="row" id="AccidentDate" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Accident Date</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtAccDtService" runat="server" CssClass="formField" Style="height: 30px; width: 200px"
                                        OnTextChanged="txtAccDtService_TextChanged" AutoPostBack="false" />
                                    <ajax:calendarextender id="ceAccDtService" targetcontrolid="txtAccDtService" popupposition="BottomLeft" runat="server" format="MM/dd/yyyy" />
                                    <asp:CompareValidator ID="cvAccDtService" runat="server" ValidationGroup="valOrgInfo"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtAccDtService"
                                        ErrorMessage="Select a valid smaller date than today" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                    <br />
                                    <asp:Label ID="lblAccidentServiceErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <div class="row" id="PateintDate" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Date Of Patient Event:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="TextBox3" runat="server" CssClass="formField" Style="height: 30px; width: 200px"
                                        OnTextChanged="TextBox3_TextChanged" AutoPostBack="false" />
                                    <ajax:calendarextender id="ceTextBox3" targetcontrolid="TextBox3" popupposition="BottomLeft" runat="server" format="MM/dd/yyyy" />
                                    <asp:CompareValidator ID="cvTextBox3" runat="server" ValidationGroup="valOrgInfo"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="TextBox3"
                                        ErrorMessage="Select a valid smaller date than today" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                    <br />
                                    <asp:Label ID="lblTextBoxErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <div class="row" id="profOnsetIllness" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Date of Onset of Illness:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtProfOnsetIllness" runat="server" CssClass="formField" Style="height: 30px; width: 200px"
                                        OnTextChanged="txtProfOnsetIllness_TextChanged" AutoPostBack="false" />
                                    <ajax:calendarextender id="cetxtProfOnsetIllness" targetcontrolid="txtProfOnsetIllness" runat="server" popupposition="BottomLeft" format="MM/dd/yyyy" />
                                    <asp:CompareValidator ID="cvtxtProfOnsetIllness" runat="server" ValidationGroup="valOrgInfo"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtProfOnsetIllness"
                                        ErrorMessage="Select a valid smaller date than today" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                    <br />
                                    <asp:Label ID="lblOnsetIllnessErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <div class="row" id="MenDtInst" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Date Of Last Menstrual Period:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtMenDtInst" runat="server" CssClass="formField" Style="height: 30px; width: 200px"
                                        OnTextChanged="txtMenDtInst_TextChanged" AutoPostBack="false" />
                                    <ajax:calendarextender id="cetxtMenDtInst" targetcontrolid="txtMenDtInst" popupposition="TopRight" runat="server" format="MM/dd/yyyy" />
                                    <asp:CompareValidator ID="cvtxtMenDtInst" runat="server" ValidationGroup="valOrgInfo"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtMenDtInst"
                                        ErrorMessage="Select a valid smaller date than today" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                    <br />
                                    <asp:Label ID="lblMenErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <div class="row" id="EstDtBirth" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Estimated Date Of Birth:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtEstDOB" runat="server" CssClass="formField" Style="height: 30px; width: 200px"
                                        OnTextChanged="txtEstDOB_TextChanged" AutoPostBack="false" />
                                    <ajax:calendarextender id="cetxtEstDOB" targetcontrolid="txtEstDOB" popupposition="TopRight" runat="server" format="MM/dd/yyyy" />
                                    <asp:CompareValidator ID="cvtxtEstDOB" runat="server" ValidationGroup="valOrgInfo"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEstDOB"
                                        ErrorMessage="Select a valid smaller date than today" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                    <br />
                                    <asp:Label ID="lblEstDOBErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lvlServiceInst" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Level Of Service:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlLvlServiceInst" CssClass="formField200" EnableViewState="false" runat="server" AutoPostBack="false"
                                        AppendDataBoundItems="false">
                                        <asp:ListItem Text="" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="E - Elective" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="U - Urgent" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </div>
                        </div>
                        <div class="row" id="dlyedInst" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Delay Reason:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlDelayedInst" CssClass="formField200" EnableViewState="false" runat="server" AutoPostBack="false"
                                        AppendDataBoundItems="false">
                                        <asp:ListItem Text="" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="1 - Proof of Eligibility Unknown or Unavailable" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2 - Litigation" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3 - Authorization Delays" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4 - Delay in Certifying Provider" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="7 - Third Party Processing Delay" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8 - Delay in Eligibility Determination" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="10 - Administration Delay in the Prior Approval Process" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11 - Other" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="15 - Natural Disaster" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="16 - Lack of Information" Value="16"></asp:ListItem>
                                        <asp:ListItem Text="17 - No Response to Initial Request" Value="17"></asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                            </div>
                        </div>
                        <div class="row" id="panumInst" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Associate PA No:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtPANumInst" runat="server" CssClass="formField200" Style="height: 30px;" MaxLength="50" />

                                    <ajax:filteredtextboxextender id="FilteredTextBoxExtender2" runat="server" enabled="True" targetcontrolid="txtPANumInst" filtertype="Numbers, UppercaseLetters, LowercaseLetters" filtermode="ValidChars"></ajax:filteredtextboxextender>

                                </span>
                            </div>
                        </div>
                    </div>
                    <asp:Label ID="lblDateError" runat="server" Font-Bold="true" ForeColor="Red" Visible="false"></asp:Label>
                </div>
            </asp:Panel>

            <!--PATIENT ACCOUNT NUMBER -->
            <ajax:collapsiblepanelextender id="cpeTrackingNumber" runat="server" collapsed="true" targetcontrolid="pnlTrackingNumber"
                expandcontrolid="pnlsepTrackingNumber" collapsecontrolid="pnlsepTrackingNumber"
                expandedtext="-" collapsedsize="0" scrollcontents="true" collapsedtext="+" expanddirection="Vertical"
                imagecontrolid="ImgpnlsepTrackingNumber" textlabelid="lblsepTrackingNumber" suppresspostback="true" />
            <asp:Panel runat="server" ID="pnlSepTrackingNumber" class="CollapsingSeparator" CssClass="CollapsingSeparator">

                <asp:Label ID="lblsepTrackingNumber" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                <span runat="server" class="pageHeader pH2" style="color: #CC0505; padding-left: 0">*</span>
                <span runat="server" class="pageHeader pH2" style="padding-left: 0">Patient Account Number</span>

            </asp:Panel>
            <asp:Panel ID="pnlTrackingNumber" runat="server" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">
                <div class="col-sm-6">
                    <div class="row" id="TrackingNumber" runat="server">
                        <div class="col-sm-5">
                            <span class="ohio-field" style="font-size: 15px; text-align: right">Patient Account Number</span>
                        </div>
                        <div class="col-sm-7">
                            <span style="text-align: left;">
                                <asp:TextBox ID="txtTrackingNumber" runat="server" CssClass="formField" MaxLength="20" />
                                <asp:RequiredFieldValidator ID="reqTrackingNumber" runat="server" ControlToValidate="txtTrackingNumber" ErrorMessage="* Missing Patient Event Tracking Number" Text="*"
                                    Display="Dynamic" ValidationGroup="valProviderHeader"></asp:RequiredFieldValidator>
                            </span>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!--SERVICE PROVIDER INFORMATION -->

            <asp:UpdatePanel ID="UpdatePanelSP" runat="server">
                <ContentTemplate>
                    <ajax:collapsiblepanelextender id="CPEService" runat="server" collapsed="false"
                        targetcontrolid="pnlService" expandcontrolid="pnlSepService" collapsecontrolid="pnlSepService"
                        collapsedsize="0" scrollcontents="false" expanddirection="Vertical" expandedtext="-" collapsedtext="+"
                        imagecontrolid="ImgpnlService" textlabelid="lblsepService" suppresspostback="true" />
                    <asp:Panel runat="server" ID="pnlSepService" class="CollapsingSeparator" CssClass="CollapsingSeparator">
                        <asp:Label ID="lblsepService" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                        <span runat="server" class="pageHeader pH2" style="color: #CC0505; padding-left: 0">*</span>
                        <span runat="server" tabindex="0" class="pageHeader pH2" style="padding-left:0">Service Provider Information</span>
                    </asp:Panel>

                    <asp:Panel ID="pnlService" runat="server" Style="min-width: 150px; height: 300px; width: auto;">
                        <span style="color: #CC0505">When entering the service (rendering) provider, this is the individual or servicing organization (such as a Laboratory, DME or Pharmacy) performing the service.</span>
                        <table style="width: 100%; font-weight: bold">
                            <tr style="background-color: lightBlue;">
                                <td style="width: 55%;"><span style="color: #CC0505">*</span>NPI</td>
                                <td style="width: 15%;">Medicaid ID</td>
                                <td style="width: 15%;">Last Name</td>
                                <td style="width: 15%;">First Name</td>
                            </tr>
                            <tr style="font-size: 15px;">
                                <td>
                                    <span style="text-align: left;">
                                        <span style="color: #CC0505">*</span>  Service Provider : 
                                <asp:TextBox ID="txtSPNPI" runat="server" CssClass="formField" Width="264px" MaxLength="10" onkeypress="return onlyNumbers(event)"
                                    onblur="OnServiceProviderTextChanged(false);" ValidationGroup="valProviderHeader" onchange="javascript:validateCSS(this);" />
                                        <asp:RequiredFieldValidator ID="reqSPNPI" runat="server" ControlToValidate="txtSPNPI" ErrorMessage="*Invalid NPI" Text="*" Display="Dynamic"
                                            ValidationGroup="valProviderHeader">
                                        </asp:RequiredFieldValidator>
                                        <button type="button" id="btnSearch6" onclick="getbuttondetail(this)" style="font-size: 15px;" class="btn btn-link" data-toggle="modal" data-target="#ProviderSearchModal">Search</button>
                                        <div id="divServiceproviderLoader"></div>
                                        <asp:Label ID="errProviderNPI" Style="color: #CC0505" runat="server"></asp:Label>

                                    </span>
                                </td>
                                <td>
                                    <span style="text-align: left;">
                                        <asp:Label runat="server" ID="txtMedicaidID" Font-Size="Large"></asp:Label>
                                        <asp:HiddenField ID="hdtxtMedicaidID" runat="server" />
                                    </span>
                                </td>
                                <td>
                                    <span style="text-align: left;">
                                        <asp:Label runat="server" ID="lblSvcProviderLName" Font-Size="Large"></asp:Label>
                                        <asp:HiddenField ID="hdlblSvcProviderLName" runat="server" />
                                    </span>
                                </td>
                                <td>
                                    <span style="text-align: left;">
                                        <asp:Label runat="server" ID="lblSvcProviderFName" Font-Size="Large"></asp:Label>
                                        <asp:HiddenField ID="hdlblSvcProviderFName" runat="server" />
                                    </span>
                                </td>

                            </tr>
                        </table>
                    </asp:Panel>

                    <asp:Panel runat="server" ID="pnlSepServiceProviderInfo" class="CollapsingSeparator" Visible="false" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerDiagnosis">
                        <span id="SepServiceProviderInfo" runat="server" class="pageHeader">Search</span>
                    </asp:Panel>
                    <asp:Panel ID="pnlServiceProviderInfo" Visible="false" runat="server" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">
                        <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                            <div class="row" style="text-align: center;">
                                <div class="col-sm-6 col-md-4 col-lg-3 ">
                                    <span class="ohio-field-label"><b>NPI</b>
                                        <asp:TextBox ID="txtserviceNPI" CssClass="ohio-field-input" runat="server">
                                        </asp:TextBox>
                                    </span>
                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-3 ">
                                    <span class="ohio-field-label"><b>Medicaid ID</b>
                                        <asp:TextBox ID="txtServiceMedicaidID" CssClass="ohio-field-input" runat="server">
                                        </asp:TextBox>
                                    </span>
                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-3 ">
                                    <span class="ohio-field-label"><b>Business/Last Name</b>
                                        <asp:TextBox ID="txtservicebussiness" CssClass="ohio-field-input" runat="server">
                                        </asp:TextBox>
                                    </span>
                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-3 ">
                                    <span class="ohio-field-label"><b>First Name</b>
                                        <asp:TextBox ID="txtserviceFirstName" CssClass="ohio-field-input" runat="server">
                                        </asp:TextBox>
                                    </span>
                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-3 ">
                                    <asp:Button ID="Button4" Text="Cancel" runat="server" OnClick="btnCancel_Click"
                                        CssClass="button" CausesValidation="true" />
                                </div>
                            </div>
                            <div class="divServiceProviderInfoSearch" style="padding-top: 10px">
                                <asp:GridView ID="gvServiceProviderInfoSearch" runat="server" Width="80%" AllowSorting="false" CssClass="gridview"
                                    EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdServiceProviderInfoSearch_SelectedIndexChanged" Style="margin-right: 100px">
                                    <Columns>
                                        <asp:BoundField DataField="RenderingProviderID" HeaderText="NPI" />
                                        <asp:BoundField DataField="MemberID" HeaderText="Medicaid ID" />
                                        <asp:BoundField DataField="" HeaderText="Business/Last Name" />
                                        <asp:BoundField DataField="" HeaderText="First Name" />
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
                </ContentTemplate>
            </asp:UpdatePanel>

            <!--ORDERING PROVIDER INFORMATION -->
            <asp:UpdatePanel ID="UpdatePanelOPI" runat="server">
                <ContentTemplate>
                    <ajax:collapsiblepanelextender id="cpeorderproviderinfo" runat="server" collapsed="true" targetcontrolid="pnlorderproviderinfo"
                        expandcontrolid="pnlSeporderproviderinfo" collapsecontrolid="pnlseporderproviderinfo"
                        collapsedsize="0" scrollcontents="false" expanddirection="Vertical" expandedtext="-" collapsedtext="+"
                        imagecontrolid="Imgpnlseporderproviderinfo" textlabelid="lblseporderproviderinfo" suppresspostback="true" />

                    <asp:Panel runat="server" ID="pnlseporderproviderinfo" class="CollapsingSeparator" CssClass="CollapsingSeparator">
                        <asp:Label ID="lblseporderproviderinfo" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                        <span runat="server" tabindex="0" class="pageHeader pH2" style="padding-left:0">Ordering Provider Information</span>
                    </asp:Panel>
                    <asp:Panel ID="pnlorderproviderinfo" runat="server" Style="min-width: 150px; height: auto; width: auto;">
                        <table style="width: 100%; font-weight: bold">
                            <tr style="background-color: lightBlue;">
                                <td style="width: 55%;"><span style="color: #CC0505">*</span>NPI</td>
                                <td style="width: 15%;">Medicaid ID</td>
                                <td style="width: 15%;">Last Name</td>
                                <td style="width: 15%;">First Name</td>
                            </tr>
                            <tr style="font-size: 15px;">
                                <td>
                                    <span style="text-align: left;">
                                        <span style="color: #CC0505">*</span>  Ordering Provider : 
                            <asp:TextBox ID="txtorderingprovidernpi" runat="server" CssClass="formField" MaxLength="10"
                                onkeypress="return onlyNumbers(event)" ValidationGroup="valProviderHeader" onblur="OnOrderingProviderTextChanged(false);"
                                onchange="javascript:validateCSS(this);" />
                                        <asp:RequiredFieldValidator ID="reqorderingprovidernpi" runat="server" ControlToValidate="txtorderingprovidernpi"
                                            ErrorMessage="* Incorrect NPI number" Text="*" Display="Dynamic" ValidationGroup="valProviderHeader"></asp:RequiredFieldValidator>
                                        <button type="button" id="btnSearch1" style="font-size: 15px;" onclick="getbuttondetail(this)" class="btn btn-link" data-toggle="modal" data-target="#ProviderSearchModalORD">
                                            Search
                                        </button>
                                        <div id="divorderproviderLoader"></div>
                                        <asp:Label ID="lblErrOrderProvidingNPI" Style="color: #CC0505" runat="server"></asp:Label>
                                    </span>
                                </td>
                                <td>
                                    <span style="text-align: left;">
                                        <asp:Label runat="server" ID="txtOMID" Font-Size="Large"></asp:Label>
                                        <asp:HiddenField ID="hdtxtOMID" runat="server" />
                                        <asp:HiddenField ID="hdtxtOMID1" runat="server" />
                                    </span>
                                </td>

                                <td>
                                    <span style="text-align: left;">
                                        <asp:Label runat="server" ID="lblOrdProviderLName" Font-Size="Large"></asp:Label>
                                        <asp:HiddenField ID="hdlblOrdProviderLName" runat="server" />
                                    </span>
                                </td>
                                <td>
                                    <span style="text-align: left;">
                                        <asp:Label runat="server" ID="lblOrdProviderFName" Font-Size="Large"></asp:Label>
                                        <asp:HiddenField ID="hdlblOrdProviderFName" runat="server" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <!--DIAGNOSIS -->
            <asp:Panel ID="uptPnlDiagnosis" runat="server">
                <ajax:collapsiblepanelextender id="cpeDiagnosis" runat="server" collapsed="false" targetcontrolid="pnlDiagnosis"
                    expandcontrolid="pnlsepDiagnosis" collapsecontrolid="pnlsepDiagnosis"
                    collapsedsize="0" expanddirection="Vertical" expandedtext="-" collapsedtext="+" scrollcontents="false"
                    imagecontrolid="ImgpnlsepDiagnosis" textlabelid="lblsepDiagnosis" suppresspostback="true" />
                <asp:Panel runat="server" ID="pnlsepDiagnosis" class="CollapsingSeparator"
                     Style="height: auto" CssClass="CollapsingSeparator">
                    <asp:Label ID="lblsepDiagnosis" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                    <span runat="server" id="diMandatoryInst" class="pageHeader pH2" style="color: #CC0505; padding-left: 0">*</span>
                    <span runat="server" tabindex="0" class="pageHeader pH2" style="padding-left: 0">Diagnosis Information</span>
                </asp:Panel>

                <asp:Panel ID="pnlDiagnosis" runat="server" Style="width: 100%; min-width: fit-content; overflow-x: hidden; height: auto;">
                    <div id="DiagnosisLineOutput" style="height: auto;"></div>

                    <div style="padding: 0px 20px;">
                        <div class="row">
                            <div class="col-sm-7">
                                <div id="divDiagnosisNoData"></div>
                            </div>
                            <div class="col-sm-5 text-right">

                                <asp:HiddenField ID="hdnDiagnosiAddCopy" runat="server" />
                                <asp:Button ID="btnDiagnosisAdd" Text="Add"
                                    runat="server" CssClass="btn btn-primary" Font-Bold="True" OnClientClick="return AddNewdiagnosisLineItem('','')"
                                    ValidationGroup="" CausesValidation="True" Width="90px" Sytle="margin-left:10px" />

                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div id="dvDiagnosisAddparent" style="height: 0; overflow: hidden">
                    <asp:Panel ID="pnlDiagnosisLine" runat="server" Visible="true" Style="padding: 20px">
                        <div id="dvDiagnosisAdd">
                            <div class="row">
                                <div class="col-sm-6">
                                    <asp:Label ID="lblDiagnosisErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6 col-lg-6">
                                    <div class="row">
                                        <asp:HiddenField ID="isDiagnosisLineOpened" runat="server" />
                                        <div class="col-lg-5 col-sm-6 text-right" style="font-size: 15px; padding: 0 10px 10px 10px;"><span class="text-red">*</span> Diagnosis Code Type</div>
                                        <div class="col-lg-6 col-sm-6" style="padding: 0 10px 10px 10px;">
                                            <asp:DropDownList ID="ddlDiagnosisCodeType" runat="server" ValidationGroup="valDiagnosis" CssClass="formField200">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvddlDiagnosisCodeType" runat="server"
                                                ControlToValidate="ddlDiagnosisCodeType" SetFocusOnError="true"
                                                ErrorMessage="Diagnosis Code Type is required" Text="Diagnosis Code Type is required" Display="Dynamic"
                                                ForeColor="Red"
                                                ValidationGroup="valDiagnosis"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6 col-lg-6">
                                    <div class="row">
                                        <div class="col-lg-4 col-sm-6 text-right" style="font-size: 15px; padding: 0 10px 10px 10px;">Diagnosis Code</div>
                                        <div class="col-lg-6 col-sm-6" style="padding: 0 10px 10px 10px;">
                                            <div class="row" style="display: inline-flex;">
                                                <div class="col-lg-8">
                                                    <asp:HiddenField ID="hdDiagLineNum" runat="server" />
                                                    <asp:TextBox ID="txtLnDiagnosisCode" runat="server" onchange="OnDiagnosisTextChanged('');" MaxLength="7" CssClass="formField200" ValidationGroup="valDiagnosis" />
                                                    <asp:RequiredFieldValidator ID="rfvtxtLnDiagnosisCode" runat="server"
                                                        ControlToValidate="txtLnDiagnosisCode" SetFocusOnError="true"
                                                        ErrorMessage="Diagnosis Code is required" Text="Diagnosis Code is required" Display="Dynamic"
                                                        ForeColor="Red"
                                                        ValidationGroup="valDiagnosis"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtLnDiagnosisCode" ID="revtxtLnDiagnosisCode" ForeColor="Red"
                                                        ValidationExpression="^[\s\S]{0,7}$" runat="server" ValidationGroup="valDiagnosis"
                                                        ErrorMessage="Procedure code modifier must be equal or less than 7 characters"></asp:RegularExpressionValidator>
                                                </div>
                                                <div class="col-lg-4 text-left">
                                                    <asp:LinkButton ID="lnkDiagnosisSearch" Style="font-size: 15px;" OnClientClick="return visibleDiagnosisPanel()" runat="server" ToolTip="Search" Text="Search" Visible="true">
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-6 col-lg-6">
                                    <div class="row">
                                        <div class="col-lg-5 col-sm-6 text-right" style="font-size: 15px; padding: 0 10px 10px 10px;">Diagnosis Code Description</div>
                                        <div class="col-lg-6 col-sm-6" style="padding: 0 10px 10px 10px;">
                                            <asp:TextBox ID="txtDiagnosisCodeDescription" Enabled="false" runat="server" Style="background-color: lightgrey;" CssClass="formField200" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm-6 col-lg-6">
                                    <div class="row">
                                        <div class="col-lg-4 col-sm-6 text-right" style="font-size: 15px; padding: 0 10px 10px 10px;">Diagnosis Date</div>
                                        <div class="col-lg-6 col-sm-6" style="padding: 0 10px 10px 10px;">
                                            <asp:TextBox ID="txtDiagnosisDate" runat="server" CssClass="formField200" ValidationGroup="valDiagnosis" />
                                            <ajax:calendarextender id="ceDiagnosisDate" targetcontrolid="txtDiagnosisDate" runat="server" format="MM/dd/yyyy" />

                                            <br />
                                            <asp:Label ID="lblDiagnosisDateErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row" style="display: flex; flex-direction: row; justify-content: flex-end;">
                                <div class="col-sm-2 col-md-2 col-lg-2 text-right">
                                    <div id="btnDiagnosisAddLineDiv" hidden="hidden">

                                        <input type="button" id="btnDiagnosisAddLine" class="buttonBoxFocus btn btn-primary" value="Add" title="Add" onclick="SaveDiagnosisLineItem('Add');" />

                                    </div>
                                    <div id="btnDiagnosisUpdateDiv" hidden="hidden">
                                        <input type="button" id="btnDiagnosisUpdate" class="buttonBoxFocus btn btn-primary" value="Update" title="Update" onclick="SaveDiagnosisLineItem('Update');" />
                                    </div>
                                </div>
                                <div class="col-sm-2 col-md-2 col-lg-2 text-right">

                                    <input type="button" id="btnDiagnosisEditCancel" style="margin: 0;" class="custom-btn btn btn-danger" value="Cancel" title="Cancel" onclick="DiagnosisLineCancel();" />

                                </div>
                                <div>
                                    <div id="diagLoader"></div>
                                </div>
                            </div>

                            <div class="form-group row" style="padding-left: 250px">
                                <asp:Label ID="lblIcdVersionError" runat="server" ForeColor="Red"></asp:Label>
                                <asp:Label ID="lblDiagnosisSaveError" Visible="false" runat="server" ForeColor="Red"></asp:Label>
                            </div>

                        </div>
                    </asp:Panel>
                </div>


            </asp:Panel>
            <asp:Panel ID="uptPnlServiceDetails" runat="server">
                <ajax:collapsiblepanelextender id="cpeServiceDetail" runat="server" collapsed="false"
                    targetcontrolid="pnlServiceDetail"
                    expandcontrolid="pnlsepServiceDetail" collapsecontrolid="pnlsepServiceDetail"
                    expandedtext="-" suppresspostback="true" textlabelid="lblsepServiceDetail"
                    collapsedtext="+" collapsedsize="0" scrollcontents="false" expanddirection="Vertical"
                    imagecontrolid="ImgpnlServiceDetail" />
                <asp:Panel runat="server" ID="pnlsepServiceDetail" class="CollapsingSeparator" CssClass="CollapsingSeparator">
                    <asp:Label ID="lblsepServiceDetail" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                    <span id="spanServiceDetailsHeaderStar" runat="server" class="pageHeader pH2" style="color: #CC0505; padding-left: 0">*</span>
                    <span runat="server" class="pageHeader pH2" style="padding-left: 0">Service Details</span>
                </asp:Panel>
                <asp:Panel ID="pnlServiceDetail" runat="server" ScrollBars="Horizontal" Style="width: 100%; min-width: fit-content; overflow: scroll" Height="200px">
                    <div id="institutionalServiceDetails" style="height: auto"></div>
                    <div style="padding: 0 20px;">
                        <div class="row">
                            <div class="col-sm-7">
                                <div id="divServDetailsNoData"></div>

                            </div>
                            <div class="col-sm-5 text-right">
                                <asp:Button ID="btnServiceDetailAdd1" Text="Add" UseSubmitBehavior="false"
                                    runat="server" CssClass="btn btn-primary" Font-Bold="True" OnClientClick="return addInstitutionalLine()"
                                    ValidationGroup="valDentalService" CausesValidation="True" Width="90px" Sytle="margin-left:10px" />
                            </div>
                        </div>
                    </div>

                    <asp:Panel runat="server" ID="pnlsepline" class="CollapsingSeparator" Visible="false" BackColor="LightBlue" CssClass="OwnerServiceDetail">
                        <span id="sepline" runat="server" class="pageHeader pH2" style="color: black; font-weight: bold">Line</span>
                        <asp:Label ID="lblservDetlbl" runat="server" Text="04"></asp:Label>
                    </asp:Panel>

                </asp:Panel>
                <div id="dvSDInstitutionalAddparent" style="height: 0; overflow: hidden">

                    <asp:Panel ID="pnlline" runat="server" Visible="true" Style="min-height: 100px; min-width: 150px; height: auto; width: auto;">
                        <div style="padding: 0 15px">
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="link-form clearfix row">
                                        <div class="col-lg-4">
                                            <div class="row" id="Servicecode" runat="server">
                                                <div class="col-sm-5">
                                                    <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Revenue Code</span>
                                                </div>
                                                <div class="col-sm-7">
                                                    <div class="row">
                                                        <div class="col-sm-7">
                                                            <span style="text-align: left;">
                                                                <asp:HiddenField ID="hdLineNumber" runat="server" />
                                                                <asp:TextBox ID="txtServicecode" runat="server" CssClass="formField" Style="width: 100%;" onblur="OnrevenueCodeTextChanged();" MaxLength="4" />
                                                                <asp:RequiredFieldValidator ID="rftxtServicecode" runat="server" ControlToValidate="txtServicecode" Text="Revenue code is required" Display="Dynamic"
                                                                    ValidationGroup="valServiceDetails_Institutional" ErrorMessage="Revenue code is required" ForeColor="Red"
                                                                    SetFocusOnError="true" Style="white-space: nowrap"></asp:RequiredFieldValidator>



                                                                <ajax:filteredtextboxextender id="ftetxtServicecode" runat="server" targetcontrolid="txtServicecode" validchars="1234567890" />
                                                            </span>
                                                            <div id="divRevenueCodeLoader1"></div>
                                                        </div>
                                                        <div class="col-sm-5 text-left">
                                                            <asp:LinkButton ID="lnkServiceCode" Style="font-size: 15px;" runat="server" ToolTip="Search" Text="Search"
                                                                CommandArgument='<%# Eval("ServiceCode") %>' OnClientClick="return visibleServiceDetailspopup('RevenueCode','Institutional')" Visible="true">
                                                            </asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-8">
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div class="row" id="ServiceTypeCode" runat="server">
                                                        <div class="col-lg-5 col-sm-6">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">
                                                                <span id="procTypeCodeSpan" style="color: #CC0505">*</span>Procedure Type Code</span>
                                                        </div>
                                                        <div class="col-lg-7">
                                                            <span style="text-align: left;">
                                                                <asp:DropDownList ID="ddlServiceTypeCode" runat="server" CssClass="formField200" onchange="javascript:OnServiceTypeCodeIndexChanged(this);">
                                                                </asp:DropDownList>
                                                                <asp:RequiredFieldValidator ID="rfvddlServiceTypeCode" runat="server"
                                                                    ControlToValidate="ddlServiceTypeCode" SetFocusOnError="true"
                                                                    ErrorMessage="Required." Text="Required." Display="Dynamic"
                                                                    ForeColor="Red"
                                                                    ValidationGroup="valServiceDetails_Institutional"></asp:RequiredFieldValidator>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="row" id="AuthorizedUnits" runat="server">
                                                        <div class="col-lg-5 col-sm-6">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Authorized Units</span>
                                                        </div>
                                                        <div class="col-lg-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtAuthorizedUnits" TextMode="Number" Enabled="false" runat="server" Style="background-color: lightgrey; width: 100%;" CssClass="formField" />
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-4">
                                            <div class="row" id="Div10" runat="server">
                                                <div class="col-sm-5 ">
                                                    <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Procedure Code</span>
                                                </div>
                                                <div class="col-sm-7">
                                                    <div class="row">
                                                        <div class="col-sm-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtLnProcedureCode" runat="server" CssClass="formField" Width="100%" onblur="OnprocedureCodeTextChanged_Institutional();" />
                                                                <asp:RequiredFieldValidator ID="rfvtxtLnProcedureCode" runat="server" ControlToValidate="txtLnProcedureCode"
                                                                    Text="Procedure code is required" ErrorMessage="Procedure code is required" SetFocusOnError="true" Style="white-space: nowrap"
                                                                    Display="Dynamic" ForeColor="Red" ValidationGroup="valServiceDetails_Institutional"></asp:RequiredFieldValidator>
                                                                <ajax:filteredtextboxextender id="ftetxtLnProcedureCode" runat="server" targetcontrolid="txtLnProcedureCode" validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />
                                                                <button id="btnISloading1" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading...</button>
                                                            </span>
                                                        </div>

                                                        <div id="divProcCodeLoader1"></div>
                                                        <div class="col-sm-5 text-left">
                                                            <asp:LinkButton ID="lnkPlaceofServiceSearchDetail1" OnClientClick="return visibleServiceDetailspopup('ProcedureCode','Institutional')" Text="Search" runat="server" ToolTip="Search"
                                                                CommandArgument='<%# Eval("CDE_POS") %>' Visible="true" Style="font-size: 15px"></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-8">
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div class="row" id="RequestUnt" runat="server">
                                                        <div class="col-lg-5 col-sm-6">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;"><span class="text-red">*</span>Requested Units</span>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <div style="float: left; width: 100%">
                                                                <span style="text-align: left; display: flex;">
                                                                    <asp:TextBox ID="txtRequestUnt" runat="server" MaxLength="15" AutoPostBack="false" Style="min-width: 70px;"
                                                                        ValidationGroup="valServiceDetails_Institutional" OnTextChanged="txtRequestUnt_TextChanged" CssClass="formField" />
                                                                    <asp:Label ID="requnitsError" runat="server" ForeColor="Red" Style="visibility: hidden"></asp:Label>

                                                                    <asp:RequiredFieldValidator ID="rftxtRequestUnt" runat="server" ControlToValidate="txtRequestUnt"
                                                                        Text="Requested unit is required" ErrorMessage="Requested unit is required" SetFocusOnError="true"
                                                                        Display="Dynamic" ForeColor="Red" ValidationGroup="valServiceDetails_Institutional"></asp:RequiredFieldValidator>
                                                                    <asp:DropDownList ID="ddLnRequestUnits" Style="min-width: 70px;" CssClass="formField150" ValidationGroup="valServiceDetails_Institutional"
                                                                        EnableViewState="true" runat="server">
                                                                    </asp:DropDownList>
                                                                    <div style="float: right; position: relative; top: 10px; right: -5px;">
                                                                        <span class="text-red">*</span>
                                                                    </div>
                                                                    <asp:RequiredFieldValidator ID="rfvddLnRequestUnits" runat="server"
                                                                        ControlToValidate="ddLnRequestUnits" SetFocusOnError="true"
                                                                        ErrorMessage="Select Requested Units type." Text="Select Requested Units type." Display="Dynamic"
                                                                        ForeColor="Red"
                                                                        ValidationGroup="valServiceDetails_Institutional"></asp:RequiredFieldValidator>
                                                                    <ajax:filteredtextboxextender id="ftbtxtRequestUnt" runat="server" targetcontrolid="txtRequestUnt" validchars="1234567890" />

                                                                </span>

                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="row" id="AuthorizedDollars" runat="server">
                                                        <div class="col-lg-5 col-sm-6">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Authorized Dollars</span>
                                                        </div>
                                                        <div class="col-lg-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtAuthorizedDollars" TextMode="Number" Enabled="false" runat="server" Style="background-color: lightgrey; width: 100%" CssClass="formField" />
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-4">
                                            <div class="row" id="Div11" runat="server">
                                                <div class="col-lg-5">
                                                    <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Procedure Code Description </span>
                                                </div>
                                                <div class="col-lg-7 col-sm-12">
                                                    <span style="text-align: left;">
                                                        <asp:TextBox ID="txtLnprocCodeDesc" Height="70px" MaxLength="80" Width="100%" TextMode="MultiLine" runat="server" Rows="6"
                                                            CssClass="formField" />

                                                        <asp:RegularExpressionValidator ID="rev_txtLnprocCodeDesc" runat="server" ControlToValidate="txtLnprocCodeDesc"
                                                            Display="Dynamic" ForeColor="Red" ValidationGroup="valServiceDetails_Institutional"
                                                            ErrorMessage="Please enter a maximum of 80 characters" SetFocusOnError="true"
                                                            ValidationExpression="[\s\S]{0,80}"></asp:RegularExpressionValidator>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="row" id="Div12" runat="server">
                                                <div class="col-lg-5">
                                                    <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Provider Service note</span>
                                                </div>
                                                <div class="col-lg-7 col-sm-12">
                                                    <span style="text-align: left;">
                                                        <asp:TextBox ID="txtLnProviderServiceNote" Height="70px" MaxLength="264" Width="100%" TextMode="MultiLine" Rows="6" runat="server"
                                                            CssClass="formField" />
                                                        <asp:RegularExpressionValidator ID="rev_txtLnProviderServiceNote" runat="server" ControlToValidate="txtLnProviderServiceNote"
                                                            Display="Dynamic" ForeColor="Red" ValidationGroup="valServiceDetails_Institutional"
                                                            ErrorMessage="Please enter a maximum of 264 characters" SetFocusOnError="true"
                                                            ValidationExpression="[\s\S]{0,264}"></asp:RegularExpressionValidator>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="row" id="Div14" runat="server">
                                                <div class="col-lg-5">
                                                    <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Level Of Care</span>
                                                </div>
                                                <div class="col-lg-7 col-sm-12">
                                                    <span style="text-align: left;">
                                                        <asp:DropDownList ID="ddLnLevelOfCare" CssClass="formField drpSplt" Width="100%" EnableViewState="true" runat="server">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                            </div>


                                        </div>
                                        <div class="col-lg-8 link-form">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="row" id="RequestedDollars" runat="server">
                                                        <div class="col-lg-5 col-sm-6">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Requested Dollars</span>
                                                        </div>
                                                        <div class="col-lg-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtRequestedDollars" runat="server" CssClass="formField" MaxLength="11" onchange="javascript:validateCSS(this);" />
                                                                <asp:RangeValidator runat="server" ControlToValidate="txtRequestedDollars" Display="Dynamic"
                                                                    ErrorMessage="Invalid requested dollars(should be max 8 digits)." MaximumValue="99999999.99" Type="Double"
                                                                    MinimumValue="0.00" ForeColor="Red"></asp:RangeValidator>
                                                                <ajax:filteredtextboxextender id="ajxtxtRequestedDollars" runat="server" enabled="True"
                                                                    targetcontrolid="txtRequestedDollars" filtertype="Numbers, Custom" validchars=".">
                                                                </ajax:filteredtextboxextender>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="row" id="AuthorizedFromDOS" runat="server">
                                                        <div class="col-lg-5 col-sm-6">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Authorized FDOS</span>
                                                        </div>
                                                        <div class="col-lg-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtAuthorizedFromDOS" runat="server" Enabled="false" Style="background-color: lightgrey;" CssClass="formField" />
                                                                <ajax:calendarextender id="CalendarExtender7" targetcontrolid="txtAuthorizedFromDOS" runat="server" />
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="row" id="ReqFDOS" runat="server">
                                                        <div class="col-lg-5 col-sm-6">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;"><span class="text-red">*</span>Requested FDOS</span>
                                                        </div>
                                                        <div class="col-lg-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtReqFDOS" title="From Date of Service (FDOS) is the start date of service date range."
                                                                    runat="server" CssClass="formField" ValidationGroup="valOrgInfo" />
                                                                <ajax:calendarextender id="ceReqFDOS" targetcontrolid="txtReqFDOS" runat="server" format="MM/dd/yyyy" />
                                                                <asp:RequiredFieldValidator ID="rftxtReqFDOS" runat="server" ControlToValidate="txtReqFDOS"
                                                                    Text="Requested FDOS is required" ErrorMessage="Requested FDOS is required"
                                                                    ForeColor="Red" Display="Dynamic" ValidationGroup="valServiceDetails_Institutional" Style="white-space: nowrap"></asp:RequiredFieldValidator>

                                                                <br />
                                                                <asp:Label ID="lblReqFDOSErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="row" id="AuthorizedToDOS" runat="server">
                                                        <div class="col-lg-5 col-sm-6">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Authorized TDOS</span>
                                                        </div>
                                                        <div class="col-lg-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtAuthorizedToDOS" runat="server" Enabled="false" Style="background-color: lightgrey;" CssClass="formField" />
                                                                <ajax:calendarextender id="ceAuthorizedToDOS" targetcontrolid="txtAuthorizedToDOS" runat="server" />

                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div class="row" id="RequestedTDOS" runat="server">
                                                        <div class="col-lg-5 col-sm-6">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;"><span class="text-red">*</span>Requested TDOS</span>
                                                        </div>
                                                        <div class="col-lg-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtReqTDOS" title="To Date of Service (TDOS) is the enc date of service date range." runat="server" CssClass="formField" ValidationGroup="valOrgInfo" />
                                                                <ajax:calendarextender id="CalendarExtender6" targetcontrolid="txtReqTDOS" runat="server" format="MM/dd/yyyy" />
                                                                <asp:RequiredFieldValidator ID="rftxtReqTDOS" runat="server" ControlToValidate="txtReqTDOS"
                                                                    ErrorMessage="Requested TDOS is required" ForeColor="Red" Style="white-space: nowrap"
                                                                    Display="Dynamic" ValidationGroup="valServiceDetails_Institutional"></asp:RequiredFieldValidator>

                                                                <br />
                                                                <asp:Label ID="lblReqTDOSErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6" style="padding: 0;">
                                                    <div class="col-lg-5 col-sm-6">
                                                        <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Remaining Units</span>
                                                    </div>
                                                    <div class="col-lg-7">
                                                        <span style="text-align: left;">
                                                            <asp:TextBox ID="txtLnRemainingUnits" TextMode="Number" Enabled="false" runat="server" Style="background-color: lightgrey;" CssClass="formField" />
                                                        </span>
                                                    </div>

                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="row">
                                                        <div class="col-lg-5">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Service Tracking No</span>
                                                        </div>
                                                        <div class="col-lg-7 col-sm-12">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtLnServiceTrackingNo" runat="server" MaxLength="50" CssClass="formField" />
                                                                <ajax:filteredtextboxextender id="ftetxtLnServiceTrackingNo" runat="server" targetcontrolid="txtLnServiceTrackingNo" validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />

                                                            </span>
                                                        </div>

                                                    </div>
                                                </div>
                                                <div class="col-sm-6" style="padding: 0;">
                                                    <div class="col-lg-5 col-sm-6">
                                                        <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Status</span>
                                                    </div>
                                                    <div class="col-lg-7">
                                                        <span style="text-align: left;">
                                                            <asp:TextBox ID="txtLnStatus" Enabled="false" runat="server" Style="background-color: lightgrey;" CssClass="formField" />
                                                            <ajax:filteredtextboxextender id="ftetxtLnStatus" runat="server" targetcontrolid="txtLnStatus" validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />

                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div class="row" runat="server">
                                                        <div class="col-sm-4">
                                                            <asp:Label ID="lblServDetailsDatesMessage" runat="server" Style="visibility: hidden; white-space: nowrap" ForeColor="Red" Font-Bold="true"></asp:Label>
                                                            <asp:Label ID="lblServDetailsrevenueCode" runat="server" Style="visibility: hidden; white-space: nowrap" ForeColor="Red" Font-Bold="true"></asp:Label>
                                                            <asp:Label ID="lblServDetailsProcCode" runat="server" Style="visibility: hidden; white-space: nowrap" ForeColor="Red" Font-Bold="true"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row " style="display: flex; flex-direction: row; justify-content: flex-end;">
                                        <div class="col-sm-2 col-md-2 col-lg-2 text-right">
                                            <input type="button" onclick="SaveInstitutionalServiceDetails()" class="buttonBoxFocus btn btn-primary" id="btnServDetailsUpdateAdd" title="Add" value="Add" />
                                        </div>
                                        <div class="col-sm-2 col-md-2 col-lg-2 text-right">
                                            <asp:Button ID="btnServDetAddEditCancel" Visible="false"
                                                runat="server"
                                                CausesValidation="true"
                                                Text="Cancel"
                                                CssClass="custom-btn btn btn-danger" OnClientClick="return cancelInstitutionalLineAdd()" />
                                            <input type="button" onclick="cancelInstitutionalLineAdd()" class="custom-btn btn btn-danger" style="margin: 0;" title="Cancel" value="Cancel" />

                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>

                    </asp:Panel>
                </div>


            </asp:Panel>
            <asp:Panel ID="upDentalServiceDetail" runat="server">
                <ajax:collapsiblepanelextender id="cpeDentalServiceDetails" runat="server"
                    collapsed="false"
                    targetcontrolid="pnlDentalServiceDetail"
                    expandcontrolid="pnlsepDentalServiceDetail"
                    collapsecontrolid="pnlsepDentalServiceDetail"
                    expandedtext="-"
                    collapsedtext="+" collapsedsize="0" scrollcontents="false" expanddirection="Vertical"
                    imagecontrolid="ImgpnlDentalServiceDetail" textlabelid="lblsepDentalServiceDetail" suppresspostback="true" />
                <asp:Panel runat="server" ID="pnlsepDentalServiceDetail" class="CollapsingSeparator" CssClass="CollapsingSeparator">
                    <asp:Label ID="lblsepDentalServiceDetail" runat="server" Text="" class="pageHeader pH2"></asp:Label>

                    <span runat="server" class="pageHeader pH2" style="color: #CC0505; padding-left: 0">*</span>
                    <span runat="server" tabindex="0" class="pageHeader pH2" style="padding-left: 0">Service Details</span>


                </asp:Panel>
                <asp:Panel ID="pnlDentalServiceDetail" runat="server" Style="width: 100%; min-width: fit-content;" Height="200px">
                    <div id="dentalServiceDetails" style="height: auto"></div>
                    <div style="padding: 0 20px; background: #ffffff">
                        <div class="row">
                            <div class="col-sm-7">
                                <div id="divDentalServDetailsNoData"></div>

                            </div>
                            <div class="col-sm-5 text-right">

                                <asp:Button ID="btnDentalServiceDetailAdd1" Text="Add" UseSubmitBehavior="false" OnClientClick="return addDentalLine()"
                                    runat="server" CssClass="btn btn-primary" Font-Bold="True"
                                    ValidationGroup="valDentalService" CausesValidation="True" Width="90px" Sytle="margin-left:10px" />

                            </div>
                        </div>
                    </div>

                </asp:Panel>
                <asp:Panel runat="server" ID="pnlsepDentalLine" class="CollapsingSeparator" Visible="false" BackColor="LightBlue" CssClass="OwnerServiceDetail">
                    <span id="sepDentalLine" runat="server" class="pageHeader pH2">Line</span>
                    <asp:Label ID="lblservDentalLineNumber" runat="server"></asp:Label>
                </asp:Panel>
                <div id="dvSDDentalAddparent" style="height: 0; overflow: hidden">
                    <asp:Panel ID="pnlDentalLine" runat="server" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; max-width: 100%;">
                        <span id="reqmessage" role="alert" aria-live="assertive" style="color: #CC0505; font-size: 14pt !important; font-weight: 100 !important">An asterisk * indicates a required field</span><br />
                        <span id="srvErrorMessage" role="alert" aria-live="assertive" style="color: #CC0505; font-size: 14pt !important; font-weight: 100 !important"></span>
                        <div style="padding: 0 15px">
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="row link-form clearfix ">
                                        <div class="col-lg-4">
                                            <div class="row" runat="server">
                                                <div class="col-lg-5">
                                                    <span class="ohio-field text-right" style="font-size: 15px; text-align: right;"><span class="text-red">*</span>Procedure Code</span>
                                                </div>
                                                <div class="col-lg-7">
                                                    <div class="row">
                                                        <div class="col-lg-8">
                                                            <span style="text-align: left;">
                                                                <asp:HiddenField ID="hdDentalLineNumber" runat="server" />
                                                                <asp:TextBox ID="txtDentalSDProcCode" aria-label="*Procedure Code" onblur="OnprocedureCodeTextChanged('Dental');"
                                                                    runat="server" CssClass="formField" />
                                                                <asp:RequiredFieldValidator ID="rfvtxtDentalSDProcCode" runat="server" ControlToValidate="txtDentalSDProcCode" Width="200px"
                                                                    Text="Procedure code is required" ErrorMessage="Procedure code is required" SetFocusOnError="true" ForeColor="Red"
                                                                    Display="Dynamic" ValidationGroup="valServiceDetails_Dental"></asp:RequiredFieldValidator>
                                                                <ajax:filteredtextboxextender id="ftetxtDentalSDProcCode" runat="server" targetcontrolid="txtDentalSDProcCode"
                                                                    validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />
                                                                <button id="btnDSloading1" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading...</button>
                                                            </span>
                                                        </div>
                                                        <div id="divProcCodeLoaderDental"></div>
                                                        <div class="col-lg-4 text-left">
                                                            <asp:LinkButton ID="lnkDentalSDProcCodeSearchLink" OnClientClick="return visibleServiceDetailspopup('ProcedureCode','Dental')"
                                                                Text="Search" runat="server" ToolTip="Search"
                                                                CommandArgument='<%# Eval("CDE_POS") %>' Visible="true"
                                                                Style="font-size: 15px;"></asp:LinkButton>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-4">
                                            <div class="row" runat="server">
                                                <div class="col-sm-5">
                                                    <span class="ohio-field text-right" style="font-size: 15px; text-align: right;"><span class="text-red">*</span>Requested Units</span>
                                                </div>
                                                <div class="col-sm-7">
                                                    <div style="display: flex">
                                                        <div style="float: left; width: 100%">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtDentalReqUnits" aria-label="*Requested Units" MaxLength="15" runat="server"
                                                                    CssClass="formField" Style="width: 200px" />
                                                                <asp:Label ID="lblDentalReqUnitsError" runat="server" ForeColor="Red" Visible="false"></asp:Label>

                                                                <ajax:filteredtextboxextender id="ftbe" runat="server" targetcontrolid="txtDentalReqUnits" validchars="1234567890" />

                                                                <asp:RequiredFieldValidator ID="rfvtxtDentalReqUnits" runat="server" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtDentalReqUnits" Text="Requested unit is required"
                                                                    ErrorMessage="Requested unit is required" Display="Dynamic" ValidationGroup="valServiceDetails_Dental"></asp:RequiredFieldValidator>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <div class="col-lg-4">
                                            <div class="row" runat="server">
                                                <div class="col-sm-5">
                                                    <span class="ohio-field text-right" style="font-size: 15px; text-align: right;">Authorized Units</span>
                                                </div>
                                                <div class="col-sm-7">
                                                    <span style="text-align: left;">
                                                        <asp:TextBox ID="txtDentalAuthUnits" runat="server" TextMode="Number" Style="background-color: lightgrey; width: 200px" Enabled="false" CssClass="formField defaultGray" />
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>


                                    <div class="row">
                                        <div class="col-lg-4">
                                            <div class="row" runat="server">
                                                <div class="col-sm-5">
                                                    <span class="ohio-field text-right" style="font-size: 15px; text-align: right">Procedure Code Description</span>
                                                </div>
                                                <div class="col-sm-7">
                                                    <span style="text-align: left;">
                                                        <asp:TextBox ID="txtDentalProcCodeDescription" aria-label="Procedure Code Description" Height="70px" MaxLength="80" Width="100%" TextMode="MultiLine"
                                                            runat="server" Rows="6" CssClass="formField" />
                                                        <ajax:filteredtextboxextender id="ftetxtDentalProcCodeDescription" runat="server" targetcontrolid="txtDentalProcCodeDescription" validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="row" runat="server">
                                                <div class="col-sm-5">
                                                    <span class="ohio-field text-right" style="font-size: 15px; text-align: right;">Tooth Number</span>
                                                </div>
                                                <div class="col-sm-7">
                                                    <span style="text-align: left;">
                                                        <asp:DropDownList ID="ddDentalToothNumber" aria-label="Tooth Number" CssClass="formField drpSplt" EnableViewState="true" Width="60px" Style="min-width: 100%; max-width: 100%" runat="server">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="row" runat="server">
                                                <div class="col-sm-5">
                                                    <span class="ohio-field text-right" style="font-size: 15px; text-align: right;">Oral Cavity</span>
                                                </div>
                                                <div class="col-sm-7">
                                                    <!-- <div class="row"  style="display:-webkit-box">-->
                                                    <span style="text-align: left;">
                                                        <asp:DropDownList ID="ddDentalOralCavity1" aria-label="Oral Cavity" CssClass="formField drpSplt" EnableViewState="true" runat="server">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div class="col-sm-12" style="visibility: hidden; display: none">
                                                        <asp:DropDownList ID="ddDentalOralCavity2" CssClass="formField drpSplt" runat="server" Style="visibility: hidden; display: none;">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-sm-12" style="visibility: hidden; display: none">
                                                        <asp:DropDownList ID="ddDentalOralCavity3" CssClass="formField drpSplt" runat="server" Style="visibility: hidden; display: none;">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-sm-12" style="visibility: hidden; display: none">
                                                        <asp:DropDownList ID="ddDentalOralCavity4" CssClass="formField drpSplt" runat="server" Style="visibility: hidden; display: none;">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-sm-12" style="visibility: hidden; display: none">
                                                        <asp:DropDownList ID="ddDentalOralCavity5" CssClass="formField drpSplt" runat="server" Style="visibility: hidden; display: none;">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <!--</div>-->

                                                </div>
                                            </div>
                                            <div class="row" runat="server">
                                                <div class="col-sm-5">
                                                    <span class="ohio-field text-right" style="font-size: 15px; text-align: right;">Tooth Surface</span>
                                                </div>
                                                <div class="col-sm-7">
                                                    <!-- <div class="row" style="display:-webkit-box">-->
                                                    <span style="text-align: left;">
                                                        <asp:DropDownList ID="ddToothSurface1" aria-label="Tooth Surface" CssClass="formField drpSplt" Style="min-width: 100%" EnableViewState="true" runat="server">
                                                        </asp:DropDownList>
                                                    </span>
                                                    <div class="col-sm-12" style="visibility: hidden; display: none">
                                                        <asp:DropDownList ID="ddToothSurface2" CssClass="formField drpSplt" EnableViewState="true" runat="server" Style="visibility: hidden; display: none;" Enabled="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-sm-12" style="visibility: hidden; display: none">
                                                        <asp:DropDownList ID="ddToothSurface3" CssClass="formField drpSplt" EnableViewState="true" runat="server" Style="visibility: hidden; display: none;" Enabled="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-sm-12" style="visibility: hidden; display: none">
                                                        <asp:DropDownList ID="ddToothSurface4" CssClass="formField drpSplt" EnableViewState="true" runat="server" Style="visibility: hidden; display: none;" Enabled="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-sm-12" style="visibility: hidden; display: none">
                                                        <asp:DropDownList ID="ddToothSurface5" CssClass="formField drpSplt" EnableViewState="true" runat="server" Style="visibility: hidden; display: none;" Enabled="true">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <!-- </div>-->
                                                </div>
                                            </div>
                                            <div class="row" runat="server">
                                                <div class="col-sm-5">
                                                    <span class="ohio-field text-right" style="font-size: 15px; text-align: right;">Provider Service Note:</span>
                                                </div>
                                                <div class="col-sm-7">
                                                    <span style="text-align: left;">
                                                        <asp:TextBox ID="txtDentalProvServnote" aria-label="Provider Service Note" Height="70px" MaxLength="264" Width="100%" TextMode="MultiLine"
                                                            runat="server" Rows="6" CssClass="formField" />
                                                        <asp:RegularExpressionValidator ID="rev_txtDentalProvServnote" runat="server" ControlToValidate="txtDentalProvServnote"
                                                            Display="Dynamic" ForeColor="Red" ValidationGroup="valServiceDetails_Institutional"
                                                            ErrorMessage="Please enter a maximum of 264 characters" SetFocusOnError="true"
                                                            ValidationExpression="[\s\S]{0,264}"></asp:RegularExpressionValidator>
                                                        <ajax:filteredtextboxextender id="ftetxtDentalProvServnote" runat="server" targetcontrolid="txtDentalProvServnote" validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="row" runat="server">
                                                <div class="col-sm-5">
                                                    <span class="ohio-field text-right" style="font-size: 15px; text-align: right;">Prosthesis,Crown or Inlay:</span>
                                                </div>
                                                <div class="col-sm-7">
                                                    <span style="text-align: left;">
                                                        <asp:DropDownList ID="ddDentalProsthsis" aria-label="Prosthesis,Crown or Inlay" CssClass="formField drpSplt" EnableViewState="true" Width="60px" Style="width: 100%; min-width: 40px; max-width: 100%" runat="server">
                                                        </asp:DropDownList>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-8 link-form">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="row" runat="server">
                                                        <div class="col-sm-5">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right;"><span class="text-red">*</span>Requested Dollars</span>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtDentalReqDollars" aria-label="*Requested Dollars" runat="server" CssClass="formField" MaxLength="11" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red" ValidationGroup="valServiceDetails_Dental" ControlToValidate="txtDentalReqDollars"
                                                                    Text="Requested dollar amount is required" ErrorMessage="Requested dollar amount is required" SetFocusOnError="true" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                <asp:RangeValidator ID="RequiredRangeValidator" runat="server" ControlToValidate="txtDentalReqDollars" ValidationGroup="valServiceDetails_Dental"
                                                                    ErrorMessage="Invalid requested dollars(Should be max 8 digits)." MaximumValue="99999999.99" Type="Double" SetFocusOnError="true"
                                                                    MinimumValue="0.00" ForeColor="Red" Display="Dynamic"></asp:RangeValidator>
                                                                <ajax:filteredtextboxextender id="ajxtxtDentalReqDollars" runat="server" enabled="True" targetcontrolid="txtDentalReqDollars"
                                                                    filtertype="Numbers, Custom" validchars=".">
                                                                </ajax:filteredtextboxextender>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="row" runat="server">
                                                        <div class="col-sm-5">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right;">Authorized Dollars</span>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtDentalAuthDollars" runat="server" TextMode="Number" Style="background-color: lightgrey;" Enabled="false" CssClass="formField defaultGray" />
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="row" runat="server">
                                                        <div class="col-sm-5">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right;"><span class="text-red">*</span>Requested FDOS</span>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <span style="text-align: left;">

                                                                <asp:TextBox ID="txtDentalReqFDOS" aria-label="*Requested FDOS" runat="server" title="From Date of Service (FDOS) is the start date of service date range." onmouseover="this.title=this.value;"
                                                                    CssClass="formField" />
                                                                <ajax:calendarextender id="CalendarExtender1" targetcontrolid="txtDentalReqFDOS" runat="server" format="MM/dd/yyyy" />
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDentalReqFDOS" ForeColor="Red" SetFocusOnError="true"
                                                                    Text="Requested FDOS is required" ErrorMessage="Requested FDOS is required" Display="Dynamic" ValidationGroup="valServiceDetails_Dental"></asp:RequiredFieldValidator>

                                                                <br />
                                                                <asp:Label ID="lblDentalReqFDOSErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>

                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="row" runat="server">
                                                        <div class="col-sm-5">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right;">Authorized FDOS</span>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtDentalAuthFDOS" runat="server" Style="background-color: lightgrey;" Enabled="false" CssClass="formField defaultGray" />
                                                                <ajax:calendarextender id="CalendarExtender2" targetcontrolid="txtDentalAuthFDOS" runat="server" />

                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                    <div class="row" runat="server">
                                                        <div class="col-sm-5">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right;"><span class="text-red">*</span>Requested TDOS</span>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtDentalReqTDOS" aria-label="*Requested TDOS" title="To Date of Service (TDOS) is the enc date of service date range." runat="server" CssClass="formField" />
                                                                <ajax:calendarextender id="CalendarExtender3" targetcontrolid="txtDentalReqTDOS" runat="server" format="MM/dd/yyyy" />

                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtDentalReqTDOS" SetFocusOnError="true"
                                                                    Text="Requested TDOS is required" ErrorMessage="Requested TDOS is required" ForeColor="Red" Display="Dynamic" ValidationGroup="valServiceDetails_Dental"></asp:RequiredFieldValidator>
                                                                <br />
                                                                <asp:Label ID="lblDentalReqTDOSErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="row" runat="server">
                                                        <div class="col-sm-5">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right;">Authorized TDOS</span>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtDentalAuthTDOS" runat="server" Style="background-color: lightgrey;" Enabled="false" CssClass="formField defaultGray" />
                                                                <ajax:calendarextender id="CalendarExtender4" targetcontrolid="txtDentalAuthTDOS" runat="server" />

                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <div class="row" runat="server">
                                                        <div class="col-sm-5">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right;">Service Tracking No</span>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtDentalServTrackingNo" aria-label="Service Tracking No" MaxLength="50" runat="server" CssClass="formField" />
                                                                <ajax:filteredtextboxextender id="ftetxtDentalServTrackingNo" runat="server" targetcontrolid="txtDentalServTrackingNo" validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-sm-6">
                                                    <div class="row" runat="server">
                                                        <div class="col-sm-5">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right;">Remaining Units</span>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtDentalRemainingUnits" TextMode="Number" runat="server" Style="background-color: lightgrey;" Enabled="false" CssClass="formField defaultGray" />
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-sm-6">
                                                </div>


                                                <div class="col-sm-6">
                                                    <div class="row" runat="server">
                                                        <div class="col-sm-5">
                                                            <span class="ohio-field text-right" style="font-size: 15px; text-align: right;">Status</span>
                                                        </div>
                                                        <div class="col-sm-7">
                                                            <span style="text-align: left;">
                                                                <asp:TextBox ID="txtDentalServDetailsStatus" Enabled="false" runat="server" Style="background-color: lightgrey;"
                                                                    CssClass="formField defaultGray" />
                                                                <ajax:filteredtextboxextender id="ftetxtDentalServDetailsStatus" runat="server" targetcontrolid="txtDentalServDetailsStatus" validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-sm-6">
                                                    <div class="row" runat="server">
                                                        <div class="col-sm-4">
                                                            <asp:Label ID="lblDentalDatemessage" runat="server" Style="white-space: nowrap; visibility: hidden" ForeColor="Red" Font-Bold="true"></asp:Label>
                                                            <asp:Label ID="lblDentalProcMessage" runat="server" Style="white-space: nowrap; visibility: hidden" ForeColor="Red" Font-Bold="true"></asp:Label>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                            <div class="row " style="display: flex; flex-direction: row; justify-content: flex-end;">
                                <div class="col-sm-2 text-right">
                                    <input type="button" id="btnServDentalUpdate" onclick="SaveDentalServiceDetails()" style="background-color: darkblue !important" class="buttonBoxFocus btn btn-primary" title="Update" value="Update" />
                                </div>
                                <div class="col-sm-2 text-right">
                                    <input type="button" onclick="cancelDentalLineAdd()" class="custom-btn btn btn-danger" style="margin: 0 !important;" title="Cancel" value="Cancel" />
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </asp:Panel>


            <asp:Panel ID="upProfessionalServiceDetail" runat="server">
                <ajax:collapsiblepanelextender id="cpeProfessionalServiceDetail" runat="server"
                    collapsed="false"
                    targetcontrolid="pnlProfessionalServiceDetail"
                    expandcontrolid="pnlsepProfessionalServiceDetail"
                    collapsecontrolid="pnlsepProfessionalServiceDetail"
                    expandedtext="-"
                    collapsedtext="+" collapsedsize="0" scrollcontents="false" expanddirection="Vertical"
                    imagecontrolid="ImgsepProfessionalServiceDetail" textlabelid="lblsepProfessionalServiceDetail" suppresspostback="true" />

                <asp:Panel runat="server" ID="pnlsepProfessionalServiceDetail" class="CollapsingSeparator" CssClass="CollapsingSeparator">
                    <asp:Label ID="lblsepProfessionalServiceDetail" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                    <span runat="server" class="pageHeader pH2" style="color: #CC0505; padding-left: 0">*</span>
                    <span runat="server" class="pageHeader pH2" style="padding-left: 0">Service Details</span>

                </asp:Panel>

                <asp:Panel ID="pnlProfessionalServiceDetail" runat="server" Visible="true" Style="width: 100%;" Height="200px">
                    <div id="professionalServiceDetails" style="height: auto"></div>
                    <div style="padding: 0 20px;">
                        <div class="row">
                            <div class="col-sm-7">
                                <div id="divProfServDetailsNoData"></div>

                            </div>
                            <div class="col-sm-5 text-right">

                                <asp:Button ID="btnProfessionalServiceDetailAdd" Text="Add" UseSubmitBehavior="false"
                                    runat="server" CssClass="btn btn-primary" Font-Bold="True" OnClientClick="return addProfessionalLine()"
                                    ValidationGroup="valProfessionalService" CausesValidation="True" Width="90px" Sytle="margin-left:10px" />
                            </div>
                        </div>
                    </div>

                </asp:Panel>
                <asp:Panel runat="server" ID="pnlsepProfessionalLine" class="CollapsingSeparator" Visible="false" BackColor="LightBlue" CssClass="OwnerServiceDetail">
                    <span id="sepProfessionalLine" runat="server" class="pageHeader pH2" style="color: black; font-weight: bold">Line</span>
                    <asp:Label ID="lblservProfessionalLineNumber" runat="server" Text="04"></asp:Label>
                </asp:Panel>
                <div id="dvSDProfessionalAddparent" style="height: 0; overflow: hidden">
                    <asp:Panel ID="pnlProfessionalLine" runat="server" Style="min-height: 150px; min-width: 150px; height: auto; width: auto; max-width: 100%;">
                        <div style="padding: 0 15px">
                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="link-form clearfix">
                                        <div class="row">
                                            <div class="col-lg-4">
                                                <div class="row" runat="server">
                                                    <div class="col-lg-5">
                                                        <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;"><span class="text-red">*</span>Procedure Code</span>
                                                    </div>
                                                    <div class="col-lg-7">
                                                        <div class="row">
                                                            <div class="col-lg-8">
                                                                <span style="text-align: left;">
                                                                    <asp:HiddenField ID="hdProfLineNumber" runat="server" />
                                                                    <asp:TextBox ID="txtProfessionalSDProcCode" onblur="OnprocedureCodeTextChanged('Professional');"
                                                                        runat="server" CssClass="formField" ValidationGroup="valOrgInfo" />
                                                                    <asp:RequiredFieldValidator ID="rfvtxtProfessionalSDProcCode" runat="server" ControlToValidate="txtProfessionalSDProcCode"
                                                                        Text="Procedure code is required" ErrorMessage="Procedure code is required" SetFocusOnError="true" ForeColor="Red" Style="white-space: nowrap"
                                                                        Display="Dynamic" ValidationGroup="valServiceDetails_Professional"></asp:RequiredFieldValidator>
                                                                    <ajax:filteredtextboxextender id="ftetxtProfessionalSDProcCode" runat="server" targetcontrolid="txtProfessionalSDProcCode"
                                                                        validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />

                                                                    <button id="btnPSloading1" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading...</button>
                                                                </span>
                                                                <div id="divProcCodeLoaderProfessional"></div>
                                                            </div>
                                                            <div class="col-lg-4 text-left">
                                                                <asp:LinkButton ID="lnkProfessionalSDProcCodeSearchLink" OnClientClick="return visibleServiceDetailspopup('ProcedureCode','Professional')"
                                                                    Text="Search" runat="server" ToolTip="Search"
                                                                    CommandArgument='<%# Eval("CDE_POS") %>' Visible="true"
                                                                    Style="font-size: 15px;"></asp:LinkButton>

                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-8">
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="row" runat="server">
                                                            <div class="col-lg-5 col-sm-6">
                                                                <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;"><span class="text-red">*</span>Requested Units</span>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <div class="row" style="display: flex; padding: 0 15px">
                                                                    <div style="float: left; width: 45%; margin-right: 2px;">
                                                                        <span style="text-align: left;">
                                                                            <asp:TextBox ID="txtProfessionalReqUnits" runat="server" CssClass="formField" MaxLength="15"
                                                                                ValidationGroup="valOrgInfo" />
                                                                            <ajax:filteredtextboxextender id="ftbtxtProfessionalReqUnits" runat="server" targetcontrolid="txtProfessionalReqUnits" validchars="1234567890" />


                                                                            <asp:Label ID="lblProfessionalReqUnitsError" runat="server" ForeColor="Red" Visible="false"></asp:Label>


                                                                        </span>
                                                                    </div>
                                                                    <div style="float: left; width: 55%">
                                                                        <span style="text-align: right;">
                                                                            <asp:DropDownList ID="ddProffSDMeasurements" CssClass="formField" ValidationGroup="valOrgInfo" EnableViewState="true" runat="server">
                                                                            </asp:DropDownList>
                                                                            <asp:RequiredFieldValidator ID="rfvddProffSDMeasurements" runat="server"
                                                                                ControlToValidate="ddLnRequestUnits" SetFocusOnError="true"
                                                                                ErrorMessage="Select Requested Units type." Text="Select Requested Units type." Display="Dynamic"
                                                                                ForeColor="Red"
                                                                                ValidationGroup="valServiceDetails_Professional"></asp:RequiredFieldValidator>
                                                                        </span>
                                                                    </div>
                                                                    <div style="float: right; position: relative; top: 10px;">
                                                                        <span class="text-red">*</span>
                                                                    </div>
                                                                </div>
                                                                <div>
                                                                    <asp:RequiredFieldValidator ID="rfvtxtProfessionalReqUnits" runat="server" ControlToValidate="txtProfessionalReqUnits" ErrorMessage="Requested unit is required"
                                                                        Text="Requested unit is required" Display="Dynamic" ForeColor="Red" ValidationGroup="valServiceDetails_Professional"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row" runat="server">
                                                            <div class="col-lg-5 col-sm-6">
                                                                <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Authorized Units</span>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <span style="text-align: left;">
                                                                    <asp:TextBox ID="txtProfessionalAuthUnits" TextMode="Number" runat="server" Enabled="false" Style="background-color: lightgrey;" CssClass="formField" />
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-lg-4">
                                                <div class="row" runat="server">
                                                    <div class="col-lg-5">
                                                        <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important">Procedure Code Description</span>
                                                    </div>
                                                    <div class="col-lg-7 col-sm-12">
                                                        <span style="text-align: left;">
                                                            <asp:TextBox ID="txtProfessionalProcCodeDescription" Height="70px" MaxLength="80" Width="100%" TextMode="MultiLine"
                                                                runat="server" Rows="6" CssClass="formField" />
                                                            <ajax:filteredtextboxextender id="ftetxtProfessionalProcCodeDescription" runat="server" targetcontrolid="txtProfessionalProcCodeDescription" validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="row" runat="server">
                                                    <div class="col-lg-5">
                                                        <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important">Modifier:</span>
                                                    </div>
                                                    <div class="col-lg-7 col-sm-12">
                                                        <div class="row" style="display: flex; justify-content: space-around; margin: 0;">
                                                            <div class="col-sm-3" style="padding: 0 2px;">
                                                                <asp:TextBox ID="txtModifier1" Style="width: 100%; min-width: 40px" Height="40px" runat="server" CssClass="formField" ValidationGroup="valOrgInfo" MaxLength="2" />
                                                                <ajax:filteredtextboxextender id="ftetxtModifier1" runat="server" targetcontrolid="txtModifier1" validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />
                                                            </div>

                                                            <div class="col-sm-3" style="padding: 0 2px;">
                                                                <asp:TextBox ID="txtModifier2" Style="width: 100%; min-width: 40px" Height="40px" runat="server" CssClass="formField" ValidationGroup="valOrgInfo" MaxLength="2" />

                                                                <ajax:filteredtextboxextender id="ftetxtModifier2" runat="server" targetcontrolid="txtModifier2" validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />
                                                            </div>
                                                            <div class="col-sm-3" style="padding: 0 2px;">
                                                                <asp:TextBox ID="txtModifier3" Style="width: 100%; min-width: 40px" Height="40px" runat="server" CssClass="formField" ValidationGroup="valOrgInfo" MaxLength="2" />

                                                                <ajax:filteredtextboxextender id="ftetxtModifier3" runat="server" targetcontrolid="txtModifier3" validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />
                                                            </div>
                                                            <div class="col-sm-3" style="padding: 0 2px;">
                                                                <asp:TextBox ID="txtModifier4" Style="width: 100%; min-width: 40px" Height="40px" runat="server" CssClass="formField" ValidationGroup="valOrgInfo" MaxLength="2" />

                                                                <ajax:filteredtextboxextender id="ftetxtModifier4" runat="server" targetcontrolid="txtModifier4" validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div style="margin: 0 20px;">
                                                                <div>
                                                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtModifier1" ID="RegularExpressionValidator2" ForeColor="Red"
                                                                        ValidationExpression="^[\s\S]{2,2}$" runat="server" ValidationGroup="valServiceDetails_Professional"
                                                                        ErrorMessage="Procedure code modifier must be 2 characters"></asp:RegularExpressionValidator>
                                                                </div>
                                                                <div>
                                                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtModifier2" ID="RegularExpressionValidator3"
                                                                        ValidationExpression="^[\s\S]{2,2}$" runat="server" ValidationGroup="valServiceDetails_Professional" ForeColor="Red"
                                                                        ErrorMessage="Procedure code modifier must be 2 characters"></asp:RegularExpressionValidator>
                                                                </div>
                                                                <div>
                                                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtModifier3" ID="RegularExpressionValidator4" ForeColor="Red"
                                                                        ValidationExpression="^[\s\S]{2,2}$" runat="server" ValidationGroup="valServiceDetails_Professional"
                                                                        ErrorMessage="Procedure code modifier must be 2 characters"></asp:RegularExpressionValidator>
                                                                </div>
                                                                <div>
                                                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtModifier4" ID="RegularExpressionValidator5" ForeColor="Red"
                                                                        ValidationExpression="^[\s\S]{2,2}$" runat="server" ValidationGroup="valServiceDetails_Professional"
                                                                        ErrorMessage="Procedure code modifier must be 2 characters"></asp:RegularExpressionValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row" runat="server">
                                                    <div class="col-lg-5">
                                                        <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important">Provider Service Note:</span>
                                                    </div>
                                                    <div class="col-lg-7 col-sm-12">
                                                        <span style="text-align: left;">
                                                            <asp:TextBox ID="txtProfessionalProvServnote" Height="70px" MaxLength="264" Width="100%" TextMode="MultiLine"
                                                                runat="server" Rows="6" CssClass="formField" />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtLnProviderServiceNote"
                                                                Display="Dynamic" ForeColor="Red" ValidationGroup="valServiceDetails_Institutional"
                                                                ErrorMessage="Please enter a maximum of 264 characters" SetFocusOnError="true"
                                                                ValidationExpression="[\s\S]{0,264}"></asp:RegularExpressionValidator>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-lg-8 link-form">
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="row" runat="server">
                                                            <div class="col-lg-5 col-sm-6">
                                                                <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;"><span class="text-red">*</span>Requested Dollars</span>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <span style="text-align: left;">
                                                                    <asp:TextBox ID="txtProfessionalReqDollars" runat="server" CssClass="formField" ValidationGroup="valOrgInfo" MaxLength="11" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtProfessionalReqDollars" ForeColor="Red"
                                                                        Text="Requested dollar amount is required" ErrorMessage="Requested dollar required" Style="white-space: nowrap" Display="Dynamic"
                                                                        ValidationGroup="valServiceDetails_Professional"></asp:RequiredFieldValidator>
                                                                    <asp:RangeValidator ID="RequiredRangeValidator1" runat="server" ControlToValidate="txtProfessionalReqDollars"
                                                                        ErrorMessage="Invalid requested dollars(Should be max 8 digits)." MaximumValue="99999999.99" Type="Double"
                                                                        MinimumValue="0.00" ForeColor="Red" Display="Dynamic"></asp:RangeValidator>
                                                                    <ajax:filteredtextboxextender id="ajxtxtProfessionalReqDollars" runat="server" enabled="True"
                                                                        targetcontrolid="txtProfessionalReqDollars" filtertype="Numbers, Custom" validchars=".">
                                                                    </ajax:filteredtextboxextender>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row" runat="server">
                                                            <div class="col-lg-5 col-sm-6">
                                                                <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Authorized Dollars</span>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <span style="text-align: left;">
                                                                    <asp:TextBox ID="txtProfessionalAuthDollars" runat="server" Enabled="false" Style="background-color: lightgrey;" CssClass="formField" />

                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="row" runat="server">
                                                            <div class="col-lg-5 col-sm-6">
                                                                <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;"><span class="text-red">*</span>Requested FDOS</span>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <span style="text-align: left;">
                                                                    <asp:TextBox ID="txtProfessionalReqFDOS" title="From Date of Service (FDOS) is the start date of service date range." runat="server" CssClass="formField" ValidationGroup="valOrgInfo" />
                                                                    <ajax:calendarextender id="CalendarExtender5" targetcontrolid="txtProfessionalReqFDOS" runat="server" format="MM/dd/yyyy" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtProfessionalReqFDOS" ForeColor="Red" Style="white-space: nowrap"
                                                                        Text="Requested FDOS is required" ErrorMessage="Requested FDOS is required" Display="Dynamic" ValidationGroup="valServiceDetails_Professional"></asp:RequiredFieldValidator>


                                                                    <asp:Label ID="lblProfessionalReqFDOSErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <div class="row" runat="server">
                                                            <div class="col-lg-5 col-sm-6">
                                                                <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Authorized FDOS</span>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <span style="text-align: left;">
                                                                    <asp:TextBox ID="txtProfessionalAuthFDOS" runat="server" Enabled="false" Style="background-color: lightgrey;" CssClass="formField" />
                                                                    <ajax:calendarextender id="CalendarExtender9" targetcontrolid="txtProfessionalAuthFDOS" runat="server" />
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-6">
                                                        <div class="row" runat="server">
                                                            <div class="col-lg-5 col-sm-6">
                                                                <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;"><span class="text-red">*</span>Requested TDOS</span>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <span style="text-align: left;">
                                                                    <asp:TextBox ID="txtProfessionalReqTDOS" runat="server" CssClass="formField" ValidationGroup="valOrgInfo" />
                                                                    <ajax:calendarextender id="CalendarExtender10" targetcontrolid="txtProfessionalReqTDOS" runat="server" format="MM/dd/yyyy" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtProfessionalReqTDOS" ForeColor="Red"
                                                                        ErrorMessage="Requested TDOS is required" Style="white-space: nowrap" Display="Dynamic" ValidationGroup="valServiceDetails_Professional"></asp:RequiredFieldValidator>

                                                                    <br />
                                                                    <asp:Label ID="lblProfessionalReqTDOSErrMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>

                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <div class="row" runat="server">
                                                            <div class="col-lg-5 col-sm-6">
                                                                <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Authorized TDOS</span>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <span style="text-align: left;">
                                                                    <asp:TextBox ID="txtProfessionalAuthTDOS" runat="server" Enabled="false" Style="background-color: lightgrey;" CssClass="formField" />
                                                                    <ajax:calendarextender id="CalendarExtender8" targetcontrolid="txtProfessionalAuthTDOS" runat="server" />

                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="row" runat="server">
                                                            <div class="col-lg-5 col-sm-6">
                                                                <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Service Tracking No</span>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <span style="text-align: left;">
                                                                    <asp:TextBox ID="txtProfessionalServTrackingNo" runat="server" MaxLength="50" CssClass="formField" />
                                                                    <ajax:filteredtextboxextender id="ftetxtProfessionalServTrackingNo" runat="server" targetcontrolid="txtProfessionalServTrackingNo" validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-sm-6">
                                                        <div class="row" runat="server">
                                                            <div class="col-lg-5 col-sm-6">
                                                                <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Remaining Units</span>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <span style="text-align: left;">
                                                                    <asp:TextBox ID="txtProfessionalRemainingUnits" runat="server" Enabled="false" TextMode="Number" Style="background-color: lightgrey;" CssClass="formField" />
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-6">
                                                    </div>


                                                    <div class="col-sm-6">
                                                        <div class="row" runat="server">
                                                            <div class="col-lg-5 col-sm-6">
                                                                <span class="ohio-field text-right" style="font-size: 15px; text-align: right; margin-bottom: 0 !important;">Status</span>
                                                            </div>
                                                            <div class="col-lg-7">
                                                                <span style="text-align: left;">
                                                                    <asp:TextBox ID="txtProfessionalServDetailsStatus" Enabled="false" runat="server" Style="background-color: lightgrey;"
                                                                        CssClass="formField" />
                                                                    <ajax:filteredtextboxextender id="ftetxtProfessionalServDetailsStatus" runat="server" targetcontrolid="txtProfessionalServDetailsStatus" validchars="1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ " />

                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-sm-6">
                                                        <div class="row" runat="server">
                                                            <div class="col-sm-4">
                                                                <asp:Label ID="lblProfDatesMessage" runat="server" Style="white-space: nowrap; visibility: hidden" ForeColor="Red" Font-Bold="true"></asp:Label>
                                                                <asp:Label ID="lblProfProcMessage" runat="server" Style="white-space: nowrap; visibility: hidden" ForeColor="Red" Font-Bold="true"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                                <div class="row" style="display: flex; flex-direction: row; justify-content: flex-end;">
                                    <div class="col-sm-2 col-md-2 col-lg-2 text-right">


                                        <input type="button" onclick="SaveProfessionalServiceDetails()" class="buttonBoxFocus btn btn-primary" id="btnServProfessionalUpdate" title="Update" value="Update" />
                                    </div>
                                    <div class="col-sm-2 col-md-2 col-lg-2 text-right">
                                        <asp:Button ID="btnServProfessionalAddEditCancel"
                                            runat="server" Visible="false"
                                            Text="Cancel"
                                            CssClass="custom-btn btn btn-danger" OnClientClick="return cancelProfessionalLineAdd()" />
                                        <input type="button" onclick="cancelProfessionalLineAdd()" class="custom-btn btn btn-danger" title="Cancel" value="Cancel" />

                                    </div>
                                </div>
                            </div>
                    </asp:Panel>
                </div>

            </asp:Panel>

            <ajax:collapsiblepanelextender id="cpeCertHospital" runat="server" collapsed="false" targetcontrolid="pnlCertHospital"
                expandcontrolid="pnlsepCertHospital" collapsecontrolid="pnlsepCertHospital"
                expandedtext="-"
                collapsedtext="+" collapsedsize="0" scrollcontents="true" expanddirection="Vertical"
                imagecontrolid="ImgsepCertHospital" textlabelid="lblsepCertHospital" suppresspostback="true" />

            <asp:Panel runat="server" ID="pnlsepCertHospital" class="CollapsingSeparator" Visible="false" CssClass="CollapsingSeparator">

                <asp:Label ID="lblsepCertHospital" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                <span runat="server" class="pageHeader pH2" style="padding-left: 0">Service Details</span>
            </asp:Panel>
            <asp:Panel ID="pnlCertHospital" runat="server" Visible="false" Style="min-height: 100px; min-width: 150px; width: auto; max-width: 1200px;">
                <div class="divGrid" style="padding-top: 10px">
                    <asp:GridView ID="gvCertHospital" runat="server" Width="80%" AllowSorting="false" CssClass="gridview" PageSize="10"
                        EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left"
                        OnSelectedIndexChanged="grdCertHospital_SelectedIndexChanged" Style="margin-right: 200px">
                        <Columns>
                            <asp:BoundField DataField="TypeOfServiceCode" HeaderText="Service Code Type" />
                            <asp:BoundField DataField="ServiceCode" HeaderText="Service Code" />
                            <asp:BoundField DataField="RequestedUnits" HeaderText="Requested Unit" />
                            <asp:BoundField DataField="" HeaderText="Approved Units" />
                            <asp:BoundField DataField="" HeaderText="Requested Unit Fee" />
                            <asp:BoundField DataField="" HeaderText="Approved Unit Fee" />
                            <asp:BoundField DataField="ServiceStartDate" HeaderText="Requested_FDOS" />
                            <asp:BoundField DataField="ServiceEndDate" HeaderText="Requested TDOS" />
                            <asp:BoundField DataField="ServiceStatusCode" HeaderText="Status" />
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" Width="100px" />

                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label"><b>Total Fees: </b>
                            <asp:TextBox ID="txtTotal" CssClass="ohio-field-input" runat="server" />
                        </span>
                    </div>

                </div>
            </asp:Panel>

            <!--PROVIDER NOTES -->
            <ajax:collapsiblepanelextender id="Cpeprovidernote" runat="server" collapsed="true" targetcontrolid="pnlProviderNote"
                expandcontrolid="pnlsepProvidersNotes" collapsecontrolid="pnlsepProvidersNotes"
                expandedtext="-"
                collapsedtext="+" collapsedsize="0" scrollcontents="false" expanddirection="Vertical"
                imagecontrolid="ImgpnlsepProvidersNotes" suppresspostback="true" textlabelid="lblsepProvidersNotes" />
            <asp:Panel runat="server" ID="pnlsepProvidersNotes" class="CollapsingSeparator" CssClass="CollapsingSeparator">

                <asp:Label ID="lblsepProvidersNotes" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                <span runat="server" tabindex="0" class="pageHeader pH2" style="padding-left: 0">Provider Notes</span>
            </asp:Panel>
            <asp:Panel ID="pnlProviderNote" runat="server" Style="min-width: 150px; height: auto; width: auto;">
                <div id="Div17" runat="server" class="pageHeader pH2 clearfix" style="color: black; font-weight: bold; font-size: 20px; background-color: lightblue; width: 100%">Note</div>
                <div class="px15 ">
                    <div class="row py15">
                        <div class="col-md-10 text-left">
                            <asp:HiddenField ID="hdnProvNoteId" runat="server" />
                            <asp:HiddenField ID="hdnProvNoteText" runat="server" />
                            <asp:TextBox ID="txtProviderNotes" Height="140px" MaxLength="262" Enabled="false" ValidationGroup="valProviderNotes" onkeyup="SaveButtonVisibility(this.value); countChar(this)"
                                TextMode="MultiLine" Rows="6" runat="server"
                                CssClass="formField w-100" />
                            <asp:RequiredFieldValidator ID="rfvprovnotes" runat="server" ControlToValidate="txtProviderNotes"
                                SetFocusOnError="true" ForeColor="Red"
                                ErrorMessage="Please enter notes." Text="Please enter notes." Display="Dynamic" ValidationGroup="valProviderNotes" />
                            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtProviderNotes" ID="revtxtProviderNotes"
                                ValidationExpression="^[\s\S]{1,262}$" runat="server" ValidationGroup="valProviderNotes" ForeColor="Red"
                                ErrorMessage="notes cannot be more than 262 chars."></asp:RegularExpressionValidator>
                            <div>
                                <span class="ohio-field" style="font-size: 15px; text-align: left">Max 262 characters. If note exceeds 262 characters, a word document can be added in the attachment section.<span class="numbersofChart" style="font-size: 15px; float: right"></span></span>

                            </div>
                            <div id="providerNotesloader" style="margin-top: 67px"></div>

                        </div>
                        <div class="col-md-2 text-center provider-action" style="display: flex; flex-direction: row; flex-wrap: wrap; align-content: center; justify-content: space-between;">
                            <div>
                                <input type="button" id="btnprovNoteEdit" class="buttonBoxFocus btn btn-primary" value="Edit" title="Edit" onclick="EditUpdateProvidernote();" />
                            </div>
                            <div>
                                <input type="button" id="btnprovNoteSave" class="buttonBoxFocus btn btn-primary" style="display: none" value="Save" title="Save" onclick="SaveProviderNotes();" />
                            </div>
                            <div>
                                <input type="button" id="btnprovNoteDelete" class="custom-btn btn btn-danger" value="Delete" title="Delete" onclick="DeleteProviderNotes();" />
                            </div>
                            <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/Images/add.png" Visible="false" CommandName="ProvidersNotes" OnCommand="btnAdd_Click" ToolTip="Add" />
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!--REVIEWER NOTES -->
            <ajax:collapsiblepanelextender id="cpePnlReviewerNotesExt" runat="server" collapsed="true" targetcontrolid="pnlProvidermainPnl"
                expandcontrolid="pnlReviewerNotes" collapsecontrolid="pnlReviewerNotes"
                expandedtext="-"
                collapsedtext="+" collapsedsize="0" scrollcontents="false" expanddirection="Vertical"
                imagecontrolid="ImgpnlReviewerNotes" textlabelid="lblsepProsthodontics" suppresspostback="true" />
            <asp:Panel runat="server" ID="pnlReviewerNotes" class="CollapsingSeparator">
     <asp:Label ID="lblsepProsthodontics" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                <span runat="server" tabindex="0" class="pageHeader pH2" style="padding-left: 0">Reviewer Notes</span>
            </asp:Panel>

            <asp:Panel ID="pnlProvidermainPnl" runat="server" Style="min-width: 150px; min-height: 100px; height: 200px; width: auto;">
                <div id="Div18" runat="server" class="pageHeader pH2 clearfix d-flex" style="color: black; font-weight: bold; background-color: lightblue; width: 100%">
                    <div class="w-150px" style="color: black; font-weight: bold; width: 150px; font-size: 20px;">Line</div>
                    <div class="w-150px" style="width: 97px; font-size: 20px">Note</div>
                </div>
                <table class="revieser-notes w-100" id="reviewerNotesTbl" runat="server">
                </table>
            </asp:Panel>

            <!-- OUTCOME OF REVIEW -->
            <ajax:collapsiblepanelextender id="cpeOutcomeOfReview" runat="server" collapsed="true" targetcontrolid="pnlOutcomeOfReview"
                expandcontrolid="pnlsepOutcomeofReview" collapsecontrolid="pnlsepOutcomeofReview"
                expandedtext="-"
                collapsedtext="+" collapsedsize="0" scrollcontents="false" expanddirection="Vertical"
                imagecontrolid="ImgpnlsepOutcomeofReview" textlabelid="lblOutcome" suppresspostback="true" />
            <asp:Panel runat="server" ID="pnlsepOutcomeofReview" class="CollapsingSeparator" CssClass="CollapsingSeparator">
                <asp:Label ID="lblOutcome" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                <span runat="server" tabindex="0" class="pageHeader pH2" style="padding-left: 0">Outcome Of Review</span>
            </asp:Panel>
            <asp:Panel ID="pnlOutcomeOfReview" runat="server" Style="min-width: 150px; min-height: 200px; height: 200px; width: auto;">
                <div id="divOutcomeofReview" runat="server" class="pageHeader pH2 clearfix d-flex" style="color: black; font-weight: bold; background-color: lightblue; width: 100%">
                    <div class="w-150px" style="color: black; font-weight: bold; width: 150px; font-size: 20px;">Line</div>
                    <div class="w-150px" style="color: black; font-weight: bold; width: 150px; font-size: 20px;">Reason Code</div>
                    <div class="w-150px" style="color: black; font-weight: bold; width: 97px; font-size: 20px;">Reason Description</div>
                </div>
                <table class="revieser-notes w-100" id="outcometbl" runat="server">
                </table>
            </asp:Panel>

            <!--ATTACHMENT -->
            <ajax:collapsiblepanelextender id="cpeAttachment" runat="server" collapsed="false" targetcontrolid="pnlAttachment"
                expandcontrolid="pnlSepAttachment" collapsecontrolid="pnlSepAttachment"
                expandedtext="-"
                collapsedtext="+" collapsedsize="0" scrollcontents="false" expanddirection="Vertical"
                imagecontrolid="ImgpnlSepAttachment" suppresspostback="true" textlabelid="lblsepAttachment" />
            <asp:Panel runat="server" ID="pnlSepAttachment" class="CollapsingSeparator" CssClass="CollapsingSeparator">
                <asp:Label ID="lblsepAttachment" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                <asp:Label runat="server" ID="AttchmentMandatory" Text="*" class="pageHeader pH2" Style="color: #CC0505; padding-left: 0"></asp:Label>
                <span runat="server" class="pageHeader pH2" style="padding-left: 0">Attachment</span>
            </asp:Panel>
            <asp:Panel ID="pnlAttachment" runat="server" Style="height: auto; width: 100%; overflow-x: hidden;">
                <div class="row" style="margin: 0;">
                    <table id="tblAttachment" class="gridView attachment-table" cellspacing='0' align='Left' rules='rows' border='1'
                        style="width: 100%; text-align: center; border-collapse: collapse; margin-top: 0;">
                        <thead>
                            <tr class="gridViewHeader">
                                <th style="width: 5%;"></th>
                                <th style="width: 15%;">Line Item</th>
                                <th style="width: 20%;">DOCUMENT ID</th>
                                <th style="width: 10%;">Patient Tracking Number</th>
                                <th style="width: 15%;">Document Type</th>
                                <th style="width: 25%">Note</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody id="tblAttachmentBody">
                        </tbody>
                    </table>

                </div>
                <div class="row" style="margin-left: 1%;">
                    <span style="color: #CC0505" id="spnShow" runat="server" visible="false">If the Prior Authorization status is In Process or Pend, please upload the attachment (s) on the Upload Attachments tab.</span>
                </div>
                <div class="row" style="text-align: left; padding: 16px">

                    <div class="col-md-3" style="text-align: left; font-weight: bold">
                        <span class="ohio-fiel"><span style="color: #CC0505; text-align: left">*</span> <b>Upload attachment: </b>
                            <br />
                            <br />

                            <mms:encryptedfileupload runat="server" id="PriorAttachmentUpload" viewstatemode="Enabled" cssclass="fileControl" style="width: 100%; min-width: 100%" onchange="javascript:validateCSS(this);" />
                            <asp:TextBox ID="txtAttachmentName" Visible="false" runat="server" CssClass="formField" Style="width: 100%; max-width: fit-content" MaxLength="100" ReadOnly="true" ValidationGroup="valUploadAttachements" />
                            <span style="color: #CC0505; display: none" id="spanAttachment">
                                <br />
                                Document is required</span>
                        </span>
                        <asp:Label ID="lblInstUploadErrMsg" Visible="false" runat="server" ForeColor="Red" />
                    </div>
                    <div class="col-md-3" style="text-align: left; font-weight: bold">
                        <span class="ohio-field"><span style="color: #CC0505">*</span><b>Document Type: </b>
                            <br />
                            <br />
                            <asp:DropDownList ID="ddlPriorAuthDocType" runat="server" CssClass="formField ddlPriorAuthDocType" ValidationGroup="valUploadAttachements" Style="width: inherit; max-width: fit-content;"
                                onchange="javascript:validateCSS(this);">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvpriorAuthDocType" runat="server" ControlToValidate="ddlPriorAuthDocType" SetFocusOnError="true"
                                ErrorMessage="Document type is required" Text="Document type is required." Display="Dynamic" ForeColor="Red"
                                ValidationGroup="valUploadAttachements"></asp:RequiredFieldValidator>
                            <span style="color: #CC0505; display: none" id="spanddlDocumentType">
                                <br />
                                Document type is required</span>
                        </span>
                        <asp:Label ID="lblInstDocTypeErrMsg" Visible="false" runat="server" ForeColor="Red" />
                    </div>
                    <div class="col-md-4" style="text-align: left; font-weight: bold">
                        <span class="ohio-field"><b>Note: </b>
                            <br />
                            <br />
                            <asp:TextBox ID="txtPriorAttachmentNote" CssClass="ohio-field-input" MaxLength="80" runat="server"
                                onchange="javascript:validateCSS(this);" TextMode="MultiLine" Rows="1" />
                            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtPriorAttachmentNote" ID="rxvtxtPriorAttachmentNote"
                                ValidationExpression="^[\s\S]{1,80}$" runat="server" ValidationGroup="valUploadAttachements" ForeColor="Red"
                                ErrorMessage="notes cannot be more than 80 chars."></asp:RegularExpressionValidator>
                        </span>
                    </div>

                    <div class="col-md-2" style="padding-left: 70px; text-align: center;">
                        <br />
                        <br />

                        <button id="btnClientAttachments" class="btn btn-primary" style="width: 90px; font-weight: bold;" onclick="addAttachmentOnButtonClickEvent(); return false;">Add</button>

                    </div>
                </div>
                <div style="display: flex; flex-direction: row; justify-content: space-between; display: none; padding-top: 10px" class="divUploading">
                    <span style="padding-left: 20px;" class="uploading">Uploading please wait...</span>
                    <span style="margin-right: 10px; margin-left: 10px; margin-bottom: 10px" class="spinner"></span>
                </div>
                <p>

                    <asp:Label ID="lblAttachmentStatusMsg" Font-Bold="true" runat="server" />
                </p>
                <p>

                    <asp:Label ID="lblAttachmentErrorMsg" Font-Bold="true" runat="server" ForeColor="Red" />
                </p>
            </asp:Panel>

            <%--Dental Attachments Begin--%>
            <ajax:collapsiblepanelextender id="cpeDentalAttachment" runat="server" collapsed="true" targetcontrolid="pnlDentalAttachment"
                expandcontrolid="pnlSepDentalAttachment" collapsecontrolid="pnlSepDentalAttachment"
                expandedtext="-"
                collapsedtext="+" collapsedsize="0" scrollcontents="false" expanddirection="Vertical"
                imagecontrolid="ImgsepDentalAttachment" suppresspostback="true" textlabelid="lblsepDentalAttachment" />
            <asp:Panel runat="server" ID="pnlSepDentalAttachment" class="CollapsingSeparator" CssClass="CollapsingSeparator">
                <asp:Label ID="lblsepDentalAttachment" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                <span runat="server" class="pageHeader pH2" style="color: #CC0505; padding-left: 0">*</span>
                <span runat="server" tabindex="0" class="pageHeader pH2" style="padding-left: 0">Attachment</span>
            </asp:Panel>
            <asp:Panel ID="pnlDentalAttachment" runat="server" Style="height: auto; width: 100%; overflow-x: hidden;">
                <div class="row" style="margin: 0;">
                    <table id="tblDentalAttachment" class="gridView attachment-table" cellspacing='0' align='Left' rules='rows' border='1'
                        style="width: 100%; text-align: center; border-collapse: collapse; margin-top: 0;">
                        <thead>
                            <tr class="gridViewHeader">
                                <th style="width: 5%;"></th>
                                <th style="width: 15%;">Line Item</th>
                                <th style="width: 20%;">DOCUMENT ID</th>
                                <th style="width: 10%;">Patient Tracking Number</th>
                                <th style="width: 15%;">Document Type</th>
                                <th style="width: 25%">Note</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody id="tblDentalAttachmentBody">
                        </tbody>
                    </table>

                </div>
                <div class="row" style="text-align: left; padding: 16px">
                    <div class="col-md-3 ">
                        <span class="ohio-field"><span style="color: #CC0505">*</span> <b>Upload attachment: </b>
                            <br />
                            <br />
                            <mms:encryptedfileupload runat="server" id="priorDentalAttachmentUpload" style="width: 100%; min-width: 100%" viewstatemode="Enabled" cssclass="fileControl"
                                onchange="javascript:validateCSS(this);" />
                            <asp:TextBox ID="TextBox1" Visible="false" runat="server" CssClass="formField wd500" Style="width: 100%; max-width: fit-content" MaxLength="100" ReadOnly="true" />
                            <span style="color: #CC0505; display: none" id="spanDentalAttachment">
                                <br />
                                Document is required</span>

                        </span>
                        <asp:Label ID="lblDentalUploadErrMsg" Visible="false" runat="server" ForeColor="Red" />

                    </div>
                    <div class="col-md-3">
                        <span class="ohio-field"><span style="color: #CC0505">*</span><b>Document Type: </b>
                            <br />
                            <br />
                            <asp:DropDownList ID="ddlPriorDentalAuthDocType" runat="server" CssClass="formField ddlPriorDentalAuthDocType" Style="width: 100%; max-width: fit-content" ValidationGroup="valdentalAttach_attachment" onchange="javascript:validateCSS(this);">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvddlPriorDentalAuthDocType" runat="server"
                                ControlToValidate="ddlPriorDentalAuthDocType" SetFocusOnError="true"
                                ErrorMessage="Document type is required." Text="Document type is required." Display="Dynamic" ForeColor="Red"
                                ValidationGroup="valdentalAttach_attachment"></asp:RequiredFieldValidator>
                            <span style="color: #CC0505; display: none" id="spanddlDentalDocumentType">
                                <br />
                                Document type is required</span>
                        </span>
                        <asp:Label ID="lblDentalDocTypeErrMsg" Visible="false" runat="server" ForeColor="Red" />

                    </div>
                    <div class="col-md-4">
                        <span class="ohio-field"><b>Note: </b>
                            <br />
                            <br />
                            <asp:TextBox ID="txtPriorDentalAttachmentNote" CssClass="ohio-field-input" MaxLength="80" runat="server"
                                onchange="javascript:validateCSS(this);" TextMode="MultiLine" Rows="1" />
                        </span>
                    </div>
                    <div class="col-md-2" style="padding-left: 70px; text-align: center;">
                        <br />
                        <br />
                        <button id="btnAddDentalAttachment1" class="btn btn-primary" style="width: 90px; font-weight: bold;" onclick="addAttachmentOnButtonClickEvent(); return false;">Add</button>
                    </div>

                </div>
                <div style="display: flex; flex-direction: row; justify-content: space-between; display: none; padding-top: 10px" class="divDentalUploading">
                    <span style="padding-left: 20px;" class="uploading">Uploading please wait...</span>
                    <span style="margin-right: 10px; margin-left: 10px; margin-bottom: 10px" class="spinner"></span>
                </div>

                <p>
                    <b>
                        <asp:Label ID="lblDentalAttachmentStatusMsg" runat="server" /></b>
                </p>
                <p>
                    <b>
                        <asp:Label ID="lblDentalAttachmentErrorMsg" runat="server" ForeColor="Red" /></b>
                </p>
            </asp:Panel>

            <%--Dental Attachments End--%>

            <ajax:collapsiblepanelextender id="cpemissingtooth" runat="server" collapsed="true" targetcontrolid="pnlmissingtooth"
                expandcontrolid="pnlSepmissingtooth" collapsecontrolid="pnlSepmissingtooth"
                expandedtext="-"
                collapsedtext="+" collapsedsize="0" scrollcontents="true" expanddirection="Vertical"
                imagecontrolid="Imgsepmissingtooth" suppresspostback="true" textlabelid="lblsepmissingtooth" />
            <asp:Panel runat="server" ID="pnlSepmissingtooth" class="CollapsingSeparator" Visible="false" CssClass="CollapsingSeparator">
                <asp:Label ID="lblsepmissingtooth" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                <span runat="server" class="pageHeader pH2" style="padding-left: 0">Missing Tooth</span>
            </asp:Panel>
            <asp:Panel ID="pnlmissingtooth" runat="server" Visible="false" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">
                <span id="spnPrimaryMissingTooth" runat="server" class="pageHeader">Select Primary Missing Teeth </span>
                <br />
                <div class="row PrimaryMissingTooth" runat="server">
                    <div class="col-sm-4 text-right">
                        <asp:CheckBox ID="chkMissingToothA" runat="server" Enabled="true" Text="A" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothB" runat="server" Enabled="true" Text="B" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothC" runat="server" Enabled="true" Text="C" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothD" runat="server" Enabled="true" Text="D" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothE" runat="server" Enabled="true" Text="E" CssClass="missingCheckBox font-weight:bold" />
                    </div>
                    <div class="col-sm-3 text-left">
                        <asp:CheckBox ID="chkMissingToothF" runat="server" Enabled="true" Text="F" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothG" runat="server" Enabled="true" Text="G" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothH" runat="server" Enabled="true" Text="H" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothI" runat="server" Enabled="true" Text="I" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothJ" runat="server" Enabled="true" Text="J" CssClass="missingCheckBox font-weight:bold" />
                    </div>
                    <br />
                    <div class="col-sm-4 text-right">
                        <asp:CheckBox ID="chkMissingToothT" runat="server" Enabled="true" Text="T" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothS" runat="server" Enabled="true" Text="S" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothR" runat="server" Enabled="true" Text="R" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothQ" runat="server" Enabled="true" Text="Q" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothP" runat="server" Enabled="true" Text="P" CssClass="missingCheckBox font-weight:bold" />
                    </div>
                    <div class="col-sm-3 text-left">
                        <asp:CheckBox ID="chkMissingToothO" runat="server" Enabled="true" Text="O" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothN" runat="server" Enabled="true" Text="N" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothM" runat="server" Enabled="true" Text="M" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothL" runat="server" Enabled="true" Text="L" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingToothK" runat="server" Enabled="true" Text="K" CssClass="missingCheckBox font-weight:bold" />
                    </div>
                </div>
                <span id="Span4" runat="server" class="pageHeader">Select Permanent Missing Teeth </span>
                <br />
                <div class="row  PermanentMissingTeeth" runat="server">
                    <div class="col-sm-5 text-right">
                        <asp:CheckBox ID="chkMissingTeeth1" runat="server" Enabled="true" Text="1" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth2" runat="server" Enabled="true" Text="2" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth3" runat="server" Enabled="true" Text="3" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth4" runat="server" Enabled="true" Text="4" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth5" runat="server" Enabled="true" Text="5" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth6" runat="server" Enabled="true" Text="6" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth7" runat="server" Enabled="true" Text="7" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth8" runat="server" Enabled="true" Text="8" CssClass="missingCheckBox font-weight:bold" />
                    </div>
                    <div class="col-sm-4 text-right">
                        <asp:CheckBox ID="chkMissingTeeth9" runat="server" Enabled="true" Text="9" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth10" runat="server" Enabled="true" Text="10" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth11" runat="server" Enabled="true" Text="11" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth12" runat="server" Enabled="true" Text="12" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth13" runat="server" Enabled="true" Text="13" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth14" runat="server" Enabled="true" Text="14" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth15" runat="server" Enabled="true" Text="15" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth16" runat="server" Enabled="true" Text="16" CssClass="missingCheckBox font-weight:bold" />
                    </div>
                    <br />
                    <div class="col-sm-5 text-right">
                        <asp:CheckBox ID="chkMissingTeeth32" runat="server" Enabled="true" Text="32" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth31" runat="server" Enabled="true" Text="31" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth30" runat="server" Enabled="true" Text="30" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth29" runat="server" Enabled="true" Text="29" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth28" runat="server" Enabled="true" Text="28" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth27" runat="server" Enabled="true" Text="27" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth26" runat="server" Enabled="true" Text="26" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth25" runat="server" Enabled="true" Text="25" CssClass="missingCheckBox font-weight:bold" />
                    </div>
                    <div class="col-sm-4 text-right">
                        <asp:CheckBox ID="chkMissingTeeth24" runat="server" Enabled="true" Text="24" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth23" runat="server" Enabled="true" Text="23" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth22" runat="server" Enabled="true" Text="22" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth21" runat="server" Enabled="true" Text="21" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth20" runat="server" Enabled="true" Text="20" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth19" runat="server" Enabled="true" Text="19" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth18" runat="server" Enabled="true" Text="18" CssClass="missingCheckBox font-weight:bold" />
                        <asp:CheckBox ID="chkMissingTeeth17" runat="server" Enabled="true" Text="17" CssClass="missingCheckBox font-weight:bold" />
                    </div>
                </div>
            </asp:Panel>

            <!--DOCUMENT BY MAIL -->
            <ajax:collapsiblepanelextender id="cpeDocumentbyMail" runat="server" collapsed="true" targetcontrolid="pnlDocumentbyMail"
                expandcontrolid="pnlSepDocumentbyMail" collapsecontrolid="pnlSepDocumentbyMail"
                expandedtext="-"
                collapsedtext="+" collapsedsize="0" scrollcontents="true" expanddirection="Vertical"
                imagecontrolid="ImgsepDocumentbyMail" suppresspostback="true" textlabelid="lblsepDocumentbyMail" />
            <asp:Panel runat="server" ID="pnlSepDocumentbyMail" class="CollapsingSeparator" CssClass="CollapsingSeparator">
                <asp:Label ID="lblsepDocumentbyMail" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                <span runat="server" class="pageHeader pH2" style="padding-left: 0">Document By Mail</span>
            </asp:Panel>
            <asp:Panel ID="pnlDocumentbyMail" runat="server" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">
                <div class="divGrid" style="padding-top: 10px">
                    <asp:GridView ID="gvDocumentbyMail" runat="server" Width="80%" AllowSorting="false" CssClass="gridview"
                        EmptyDataText="No Data Found" DataKeyNames="PRIOR_AUTH_DOCUMENT_MAIL_ID" AutoGenerateColumns="false" OnRowCommand="gvDocumentbyMail_RowCommand"
                        HorizontalAlign="Left" OnSelectedIndexChanged="grdDocumentbyMail_SelectedIndexChanged" Style="margin-right: 200px">
                        <Columns>
                            <asp:TemplateField HeaderText="Line No">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DOCUMENT_TYPE_DESC" HeaderText="Document Type" />
                            <asp:BoundField DataField="PRIOR_AUTH_DOCUMENT_MAIL_DESC" HeaderText="Note" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnPrintCover" runat="server" Text="Print Cover" CommandName="PrintCoverpage" OnCommand="btnPrintCoverAdd_Click" CssClass="btn btn-info" OnClientClick="javascript:window.open('../Documents/MITSEDMS_Cover_IT4.pdf'); return false;" ToolTip="PrintCover" Width="90px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteDocumentMail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CssClass="btn btn-danger" Font-Bold="True" CausesValidation="True" Width="90px"
                                        ToolTip="Delete" AlternateText="Delete"
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
                    <div class="col-md-4">
                        <span class="ohio-field-label"><span style="color: #CC0505">*</span><b>Document Type: </b>
                            <asp:DropDownList ID="ddlDoctype" CssClass="formField" runat="server">
                            </asp:DropDownList>
                        </span>
                    </div>
                    <div class="col-md-4">
                        <span class="ohio-field-label"><b>Note: </b>
                            <asp:TextBox ID="txtDocNote" runat="server" Rows="2" CssClass="formField wd650" TextMode="MultiLine" MaxLength="1000" />
                        </span>
                    </div>
                    <div class="div DocumentBy MailAdd">
                        <asp:Button ID="btnAddDocument" Text="ADD" runat="server" OnClick="btnDocumentMail_Click" CssClass="btn btn-info" Font-Bold="True" CausesValidation="True" Width="90px" />
                    </div>
                </div>
            </asp:Panel>



            <asp:Panel ID="pnlreviewernoteprovider" runat="server" Style="min-height: 0px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">
                <div class="divGrid" style="padding-top: 10px">
                    <asp:GridView ID="gvOutcomeofreview" runat="server" Width="70%" AllowSorting="false" CssClass="gridview"
                        EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="gvOutcomeofreview_SelectedIndexChanged" Style="margin-right: 300px">
                        <Columns>
                            <asp:BoundField DataField="ODSProviderNoteID" HeaderText="Service Line" />
                            <asp:BoundField DataField="ReasonCode" HeaderText="Reason Code" />
                            <asp:BoundField DataField="Note" HeaderText="Reason Code Description" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImageButton1" runat="server" CommandName="DocumentMail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                        ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="if(!confirm('Are you sure you want to delete?')) return false;" AlternateText="Delete"
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
            </asp:Panel>


            <!--REASON FOR DENIAL -->
            <ajax:collapsiblepanelextender id="cpeReasonforDenial" runat="server" collapsed="true" targetcontrolid="pnlReasonforDenial"
                expandcontrolid="pnlsepReasonforDenial" collapsecontrolid="pnlsepReasonforDenial"
                expandedtext="-"
                collapsedtext="+" collapsedsize="0" scrollcontents="true" expanddirection="Vertical"
                imagecontrolid="ImgsepReasonforDenial" suppresspostback="true" textlabelid="lblsepReasonforDenial" />
            <asp:Panel runat="server" ID="pnlsepReasonforDenial" class="CollapsingSeparator" Visible="false"
                  CssClass="CollapsingSeparator">
                <asp:Label ID="lblsepReasonforDenial" runat="server" Text="" class="pageHeader pH2"></asp:Label>
                <span runat="server" class="pageHeader pH2" style="padding-left: 0">Reason For Denial</span>
            </asp:Panel>
            <asp:Panel ID="pnlReasonforDenial" runat="server" Visible="false" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">
                <div class="divGrid" style="padding-top: 10px">
                    <asp:GridView ID="gvReDenial" runat="server" Width="80%" AllowSorting="false" CssClass="gridview"
                        EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdReDenial_SelectedIndexChanged" Style="margin-right: 200px">
                        <Columns>

                            <asp:BoundField DataField="ServiceStatusReasonCode" HeaderText="Reason Code" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            <asp:BoundField DataField="" HeaderText="Note" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>

            </asp:Panel>


            <ajax:collapsiblepanelextender id="CollapsibleMaliciousAttachments" runat="server" collapsed="true"
                targetcontrolid="pnlMaliciousAttachments"
                expandcontrolid="pnlSepMaliciousAttachmentsInfo" collapsecontrolid="pnlSepMaliciousAttachmentsInfo" />
            <asp:Panel runat="server" ID="pnlSepMaliciousAttachmentsInfo" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);"
                ToolTip="Click to Expand/Collapse" CssClass="CollapsingSeparator">
                <span id="Span7" runat="server" class="pageHeader pH2">+Malicious Attachments
                </span>
            </asp:Panel>
            <asp:Panel ID="pnlMaliciousAttachments" runat="server" Style="min-height: 240px; min-width: 300px; height: auto; width: auto; max-width: 2200px;">

                <uc:maliciousattachments id="PAMaliciousAttachments" runat="server" />

            </asp:Panel>

            <asp:Panel ID="btnPriorAuth" runat="server" CssClass="Button">
                <div style="text-align: center; padding-top: 4px; display: inline-flex; width: 100%;">
                    <div class="container-fluid">
                        <div class="row">
                            <asp:Button ID="btnUpdate" runat="server" CausesValidation="true" Style="display: block; margin: 0 2px;" Text="Update PA Request" CssClass="buttonBoxFocus buttonVisible"
                                OnClick="btnUpdate_Click" OnClientClick="CallBlockUIPostBack()" />
                            <asp:Button ID="btnReSubmit" runat="server" Text="Resubmit" CssClass="buttonBoxFocusgreen" Style="margin: 0 2px;"
                                UseSubmitBehavior="false" Visible="false" OnClick="btnReSubmit_Click" OnClientClick="CallBlockUIPostBack()" />

                            <asp:Button ID="btnSave" runat="server" CausesValidation="true" Style="margin: 0 2px;" Text="Save" CssClass="buttonBoxFocusBlue"
                                OnClick="btnSave_Click" OnClientClick="if(!confirm('Your unsubmitted prior authorization will be saved in the system for 72 hours.')) return false; CallBlockUIPostBack();"
                                ToolTip="Save current screen data" />
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="buttonBoxFocusgreen" Style="margin: 0 2px;"
                                OnClick="btnSubmit_Click" OnClientClick="this.disabled=true; CallBlockUIPostBack();" UseSubmitBehavior="false" Visible="false" />

                            <asp:Button ID="btnCancel_Revert" runat="server" Text="Cancel" CssClass="buttonBoxFocusred" Style="margin: 0 2px;"
                                Visible="false" AutoPostBack="true" OnClick="btnCancel_Revert_Click" OnClientClick="CallBlockUIPostBack()" />
                            <asp:Button ID="btnClearAll" runat="server" Text="Cancel" CssClass="buttonBoxFocusred" Style="margin: 0 2px;"
                                OnClientClick="if(!confirm('Are you sure you want to cancel?')) return false; CallBlockUIPostBack()" OnClick="btnClearAll_Click" />
                            <asp:Button ID="btnCancelPARequest" runat="server" Text="Cancel PA Request" CssClass="buttonBoxFocusgreen" Style="margin: 0 2px;"
                                OnClick="btnCancelPARequest_Click" Visible="false" OnClientClick="CallBlockUIPostBack()" />
                            <asp:Button ID="btnCopy" runat="server" Text="Copy" CssClass="buttonBoxFocus buttonfloatRight" Style="margin: 0 2px;"
                                ToolTip="Take action" OnClick="btnCopy_Click" OnClientClick="CallBlockUIPostBack()" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <ajax:modalpopupextender id="mpe" runat="server" popupcontrolid="pnlProNote" targetcontrolid="Button1" backgroundcssclass="modalBackground" />
            <asp:Panel ID="pnlProNote" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
                <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                    <div class="popTitle">
                        <asp:Label ID="lblNoteTitle" CssClass="bodyTextBold" runat="server" Text="Title" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlMain" runat="server">
                    <asp:MultiView ID="mltPopup" runat="server">
                        <asp:View ID="vwPriorAuthProvidersNotes" runat="server">
                            <div style="text-align: left; padding: 15px" class="container-fluid">
                                <div class="row">
                                    <uc:priorauthprovidersnotes runat="server" id="ucPriorAuthProvidersNotes" />
                                </div>
                            </div>
                        </asp:View>

                        <asp:View ID="vwServiceDetails" runat="server">
                            <div style="text-align: left; padding: 15px" class="container-fluid">
                                <div class="row">
                                    <uc:priorauthservicedetails runat="server" id="ucPriorAuthServiceDetails" />
                                </div>
                            </div>
                        </asp:View>

                        <asp:View ID="vwDentalServiceDetail" runat="server">
                            <div style="text-align: left; padding: 15px" class="container-fluid">
                                <div class="row">
                                    <uc:priorauthdentalservicedetails runat="server" id="ucPriorAuthDentalServiceDetails" />
                                </div>
                            </div>
                        </asp:View>

                        <asp:View ID="vwProDiagnosis" runat="server">
                            <div style="text-align: left; padding: 15px" class="container-fluid">
                                <div class="row">
                                    <uc:priorauthdiagnosis runat="server" id="ucPriorAuthDiagnosis" />
                                </div>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </asp:Panel>
            </asp:Panel>



            <asp:Button runat="server" ID="Button1" Style="display: none" Text="ButtonDummy" />
            <ajax:modalpopupextender id="mpedoc" runat="server" popupcontrolid="pnlDocByMail" targetcontrolid="Button5" backgroundcssclass="modalBackground" />
            <asp:Panel ID="pnlDocByMail" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
                <asp:Panel ID="pnlDocbyMailheader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                    <div class="popTitle">
                        <asp:Label ID="lblDocumentbyMail" CssClass="bodyTextBold" runat="server" Text="Title" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="Panel1" runat="server">
                    <asp:MultiView ID="mltDocumentbyMail" runat="server">
                        <asp:View ID="vwDocumentbyMail" runat="server">
                            <div style="text-align: left; padding: 15px" class="container-fluid">
                                <div class="row">
                                    <uc:priorauthdocumentbymail runat="server" id="ucPriorAuthDocumentbyMail" />
                                </div>
                            </div>
                        </asp:View>

                        <asp:View ID="vwAttachment" runat="server">
                            <div style="text-align: left; padding: 15px" class="container-fluid">
                                <div class="row">
                                    <uc:priorauthattachment runat="server" id="ucPriorAuthAttachment" />
                                </div>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </asp:Panel>
            </asp:Panel>
            <asp:Button runat="server" ID="Button5" Style="display: none" Text="ButtonDummy5" />
            <ajax:modalpopupextender id="mpesearch" runat="server" popupcontrolid="pnlsearch" targetcontrolid="Button6" backgroundcssclass="modalBackground" />
            <asp:Panel ID="pnlsearch" runat="server" CssClass="modalPopup" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; max-width: 1200px;">
                <asp:Panel ID="pnlSearchHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                    <div class="popTitle">
                        <asp:Label ID="lblServiceSearch" CssClass="bodyTextBold" runat="server" Text="Title" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="Panel4" runat="server">
                    <asp:MultiView ID="mltservicesearch" runat="server">
                        <asp:View ID="vwServicingProviderinfo" runat="server">
                            <div style="text-align: left; padding: 15px" class="container-fluid">
                                <div class="row">
                                    <uc:servicingproviderinformationsearch id="ucServicingProviderInformationSearch" runat="server" visible="true" enableviewstate="true" />
                                </div>
                            </div>
                        </asp:View>

                    </asp:MultiView>
                </asp:Panel>
            </asp:Panel>
            <asp:Button runat="server" ID="Button6" Style="display: none" Text="ButtonDummy6" />
            <ajax:modalpopupextender id="mpediagnosissearch" runat="server" popupcontrolid="pnlDiagnosisSearchS" targetcontrolid="Button7" backgroundcssclass="modalBackground" />
            <asp:Panel ID="pnlDiagnosisSearchS" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
                <asp:Panel ID="pnlDiagnosisSearchSHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                    <div class="popTitle">
                        <asp:Label ID="lbldiagnosissearch" CssClass="bodyTextBold" runat="server" Text="Title" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="Panel5" runat="server">
                    <asp:MultiView ID="mltdiagnosissearch" runat="server">
                        <asp:View ID="vwDiagnosisSearch" runat="server">
                            <div style="text-align: left; padding: 15px" class="container-fluid">
                                <div class="row">
                                    <uc:priorauthdiagnosisseach id="ucPriorAuthDiagnosisSeach" runat="server" visible="true" enableviewstate="true" />
                                </div>
                            </div>
                        </asp:View>

                    </asp:MultiView>
                </asp:Panel>
            </asp:Panel>
            <asp:Button runat="server" ID="Button7" Style="display: none" Text="ButtonDummy7" />
            <ajax:modalpopupextender id="mpeDentalHSCPSCodeSearch" runat="server" popupcontrolid="pnlDentalHSCPSCodeSearch" targetcontrolid="Button8" backgroundcssclass="modalBackground" />
            <asp:Panel ID="pnlDentalHSCPSCodeSearch" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
                <asp:Panel ID="pnlDentalHSCPSCodeSearchHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                    <div class="popTitle">
                        <asp:Label ID="lblDentalHSCPSCodeSearch" CssClass="bodyTextBold" runat="server" Text="Title" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="Panel6" runat="server">
                    <asp:MultiView ID="mltDentalHSCPSCodeSearch" runat="server">
                        <asp:View ID="vwDentalHSCPSCodeSearch" runat="server">
                            <div style="text-align: left; padding: 15px" class="container-fluid">
                                <div class="row">
                                    <uc:priorauthdentalhscpscodesearch runat="server" id="PriorAuthDentalHSCPSCodeSearch" visible="true" enableviewstate="true" />
                                </div>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </asp:Panel>
            </asp:Panel>
            <asp:Button runat="server" ID="Button8" Style="display: none" Text="ButtonDummy8" />
            <!-- The Modal -->
            <div class="modal" id="myModal">
                <div class="modal-dialog">
                    <div class="modal-content" style="height: auto; background-color: lightblue">
                        <input type="hidden" id="hdnSearchId" />
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                            <ContentTemplate>
                                <div style="padding-bottom: 30px; background-color: lightblue">
                                    <table class="modalbody">
                                        <tr style="background-color: darkcyan; color: white; font-size: 14px;">
                                            <td>NPI</td>
                                            <td>MEDICAID ID</td>
                                            <td>BUSINESS/LAST NAME</td>
                                            <td>FIRST NAME</td>
                                            <td>
                                                <button type="button" class="close" data-dismiss="modal">&times;</button></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="TextBox2" CssClass="modalInput" runat="server"></asp:TextBox>

                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox4" CssClass="modalInput" runat="server">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox5" CssClass="modalInput" runat="server">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="TextBox6" CssClass="modalInput" runat="server">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="Button11" runat="server" CausesValidation="true" Text="Search"
                                                    CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="ProviderSearch"
                                                    OnClientClick="return showProgress()" Style="background-color: darkslateblue !important; height: 30px; margin-top: 10px;" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div style="background-color: darkcyan;">
                                    <asp:Label ID="Label5" class="expandcollapse" runat="server" Text="SEARCH RESULTS"
                                        Visible="false" Style="font-weight: bold; font-size: 14px; color: white; padding-left: 20px;"></asp:Label>
                                </div>
                                <div>
                                    <mms:sortablepaginggridview id="SortablePagingGridView1" runat="server"
                                        autogeneratecolumns="False" cssclass="gridViewSmallFont providerSearchResultGrid" width="100%"
                                        allowsorting="true" emptydatatext="No Providers found."
                                        onpageindexchanging="gvPriorAuthSearchPage_PageIndexChanging"
                                        onsorting="gvNPICodeSearch_Sorting" rowstyle-verticalalign="Top" allowpaging="True"
                                        pagesize="15" gridviewsortcolumn="MEDICAID_ID" gridviewsortdirection="Ascending"
                                        datakeynames="MEDICAID_ID, LAST_OR_BUSINESS_NAME,FIRST_NAME,ADDRESS1,ADDRESS2,CITY,STATE,ZIP">
                                        <columns>
                                            <asp:TemplateField HeaderText="NPI" ItemStyle-Width="100" ItemStyle-Wrap="true">
                                                <itemtemplate>
                                                    <asp:LinkButton ID="lnkNPI" runat="server" ToolTip="Search"
                                                        Text='<%# Eval("NPI") %>' CommandArgument='<%# Eval("NPI") %>'
                                                        OnClick="lnkNPI_Click">
                                                    </asp:LinkButton>
                                                </itemtemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Medicaid ID" ItemStyle-Width="100" ItemStyle-Wrap="true">
                                                <itemtemplate>
                                                    <asp:LinkButton ID="lnkMedicaidId" runat="server" ToolTip="Search"
                                                        Text='<%# Eval("MEDICAID_ID") %>' CommandArgument='<%# Eval("MEDICAID_ID") %>'
                                                        OnClick="lnkMedicaidId_Click">
                                                    </asp:LinkButton>
                                                </itemtemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="LAST_OR_BUSINESS_NAME" HeaderText="Business/Last Name"
                                                SortExpression="LAST_OR_BUSINESS_NAME" />
                                            <asp:BoundField DataField="FIRST_NAME" HeaderText="First Name"
                                                SortExpression="FIRST_NAME" />
                                            <asp:BoundField DataField="ADDRESS1" HeaderText="Address Line 1"
                                                SortExpression="ADDRESS1" />
                                            <asp:BoundField DataField="ADDRESS2" HeaderText="Address Line 2"
                                                SortExpression="ADDRESS2" />
                                            <asp:BoundField DataField="CITY" HeaderText="City"
                                                SortExpression="CITY" />
                                            <asp:BoundField DataField="STATE" HeaderText="State"
                                                SortExpression="STATE" />
                                            <asp:BoundField DataField="ZIP" HeaderText="Zip"
                                                SortExpression="ZIP" />
                                        </columns>
                                    </mms:sortablepaginggridview>
                                    <asp:HiddenField ID="hdnNPI" runat="server" />
                                    <asp:HiddenField ID="hdnMedicaidId" runat="server" />
                                    <asp:HiddenField ID="hdnProviderName" runat="server" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <!-- Pop up for Diagnosis search -->
            <!-- The Diagnosis Modal -->
            <div class="modal" id="myDiagModal">
                <div class="modal-dialog">
                    <div class="modal-content">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Diagnosis Search</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                            <ContentTemplate>
                                <asp:HiddenField ID="hdnDiagnosisCode" runat="server" />
                                <asp:HiddenField ID="hdnDiagnosisVersion" runat="server" />
                                <asp:HiddenField ID="hdnDiagnosisDes" runat="server" />
                                <div class="modal-body">
                                    <div class="row" style="text-align: center;">
                                        <div class="col-sm-6 col-md-4 col-lg-3">
                                            <span class="ohio-field-label"><span style="color: #CC0505"></span><b>Diagnosis Code</b>
                                                <asp:TextBox ID="txtDiagnosisCodeSearch" MaxLength="7" CssClass="ohio-field-input" runat="server">
                                                </asp:TextBox>
                                            </span>
                                        </div>
                                        <div class="col-sm-6 col-md-4 col-lg-3">
                                            <span class="ohio-field-label"><span style="color: #CC0505">*</span><b>ICD Version </b>
                                                <asp:DropDownList ID="DropDownList1" CssClass="ohio-field-label" runat="server">
                                                    <asp:ListItem Value="ICD 10" Text="ICD 10"></asp:ListItem>
                                                    <asp:ListItem Value="ICD 9" Text="ICD 9"></asp:ListItem>
                                                </asp:DropDownList>
                                            </span>
                                        </div>
                                        <div class="col-sm-6 col-md-4 col-lg-3">
                                            <span class="ohio-field-label"><span style="color: #CC0505"></span><b>Diagnosis Code Description</b>
                                                <asp:TextBox ID="txtDiagnosisCodeDescSearch" MaxLength="400" CssClass="ohio-field-input" runat="server">
                                                </asp:TextBox>
                                            </span>
                                        </div>
                                        <div class="col-sm-6 col-md-4 col-lg-3">
                                            <asp:Button ID="btnDiagSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnDiagSearch_Click" ValidationGroup="ProviderSearch" OnClientClick="return showProgress()" Style="background-color: darkslateblue !important" />
                                        </div>
                                    </div>
                                    <asp:Label ID="lblDiagSearch" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>
                                    <div class="popupGridViewOnSearch">
                                        <mms:sortablepaginggridview
                                            id="gvClaimDiagnosisSearch"
                                            runat="server"
                                            autogeneratecolumns="False"
                                            cssclass="gridViewSmallFont" width="100%"
                                            allowsorting="true"
                                            showheaderwhenempty="true"
                                            emptydatatext="No Diagnosis Found."
                                            rowstyle-verticalalign="Top"
                                            alternatingrowstyle-backcolor="White" gridlines="Horizontal"
                                            gridviewsortcolumn="ICD10Diag" gridviewsortdirection="Ascending">
                                            <columns>
                                                <asp:TemplateField HeaderText="Diagnosis Code" ItemStyle-Width="100" ItemStyle-Wrap="true">
                                                    <itemtemplate>
                                                        <asp:LinkButton ID="lnkICD10Diag" runat="server" ToolTip="Search" Text='<%# Eval("ICD10Diag") %>' CommandArgument='<%# Eval("ICD10Diag") %>' OnClick="lnkICD10Diag_Click">
                                                        </asp:LinkButton>
                                                    </itemtemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ICDVersion" HeaderText="ICD Version" SortExpression="ICDVersion" />
                                                <asp:BoundField DataField="DiagDesc" HeaderText="Diagnosis Code Description" SortExpression="DiagDesc" />
                                            </columns>
                                        </mms:sortablepaginggridview>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <!--PROVIDER SEARCH MODAL SERVICE PROVIDER -->
            <div class="modal" id="ProviderSearchModal">
                <div class="modal-dialog">
                    <div style="height: auto; background-color: lightblue">
                        <input type="hidden" id="hdnProvSearchModalSearchId" />
                        <asp:UpdatePanel runat="server" ID="upmodalProvSearchModalBody">
                            <ContentTemplate>
                                <div class="popupcontrolWidth" style="padding-bottom: 30px; background-color: lightblue; width: auto; min-width: fit-content;">
                                    <table class="modalbody" style="width: 100%">
                                        <tr style="background-color: darkcyan; color: white; font-size: 14px;">
                                            <td>NPI</td>
                                            <td>MEDICAID ID</td>
                                            <td>BUSINESS/LAST NAME</td>
                                            <td>FIRST NAME</td>
                                            <td>
                                                <button type="button" onclick="CloseServiceProviderpopup();" class="close" data-dismiss="modal">&times;</button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtNPI" MaxLength="10" CssClass="modalInput" runat="server" CausesValidation="true"
                                                    onkeypress="return isNumber(event)" onchange="javascript:validateCSS(this);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtProMedicaidID" MaxLength="7" CssClass="modalInput" runat="server"
                                                    onkeypress="return isNumber(event)" onchange="javascript:validateCSS(this);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBusinessLastName" MaxLength="60" CssClass="modalInput" runat="server" onchange="javascript:validateCSS(this);">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFirstName" MaxLength="35" CssClass="modalInput" runat="server" onchange="javascript:validateCSS(this);">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search"
                                                    CssClass="buttonBoxFocus" ValidationGroup="ProviderSearch"
                                                    OnClientClick="return GetServiceProviderDetails()" Style="background-color: darkslateblue !important; height: 30px; margin-top: 7px; width: 55px" />

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <asp:Label ID="lblErrorSearch" runat="server"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvNPI" runat="server" ControlToValidate="txtNPI"
                                                    ErrorMessage="*Other provider is unknown" Text="*" Display="Dynamic"
                                                    ValidationGroup="valProviderHeader"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator runat="server" ID="revNPI" Display="Dynamic"
                                                    ValidationGroup="valProviderHeader" ControlToValidate="txtNPI" SetFocusOnError="true"
                                                    ValidationExpression="^[0-9]{10}$">
                                                </asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                    </table>

                                    <div style="background-color: darkcyan; width: 845px">
                                        <asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="SEARCH RESULTS"
                                            Visible="false" Style="font-weight: bold; font-size: 14px; color: white; padding-left: 20px;"></asp:Label>
                                    </div>
                                    <div class="ServiceproviderNPIOutputResponsive">
                                        <div id="ServiceproviderNPIOutput"></div>
                                        <asp:HiddenField ID="hdnProvSearchModalNPI" runat="server" />
                                        <asp:HiddenField ID="hdnProvSearchModalMedicaidId" runat="server" />
                                        <asp:HiddenField ID="hdnProvSearchModalProviderName" runat="server" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <!--PROVIDER SEARCH MODAL ORDERING PROVIDER -->
            <div class="modal" id="ProviderSearchModalORD" style="width: 100%">
                <div class="modal-dialog">
                    <div style="height: auto; width: 100%; background-color: lightblue">
                        <input type="hidden" id="hdnProvSearchModalSearchIdORD" />
                        <asp:UpdatePanel runat="server" ID="upmodalProvSearchModalBodyORD">
                            <ContentTemplate>
                                <div class="popupcontrolWidth" style="padding-bottom: 30px; background-color: lightblue; width: 100%; min-width: fit-content;">
                                    <table class="modalbody" style="width: 100%">
                                        <tr style="background-color: darkcyan; color: white; font-size: 14px;">
                                            <td>NPI</td>
                                            <td>MEDICAID ID</td>
                                            <td>BUSINESS/LAST NAME</td>
                                            <td>FIRST NAME</td>
                                            <td>
                                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtNPIORD" MaxLength="10" CssClass="modalInput" runat="server" CausesValidation="true"
                                                    onkeypress="return isNumber(event)" onchange="javascript:validateCSS(this);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtProMedicaidIDORD" MaxLength="7" CssClass="modalInput" runat="server"
                                                    onkeypress="return isNumber(event)" onchange="javascript:validateCSS(this);"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBusinessLastNameORD" MaxLength="60" CssClass="modalInput" runat="server" onchange="javascript:validateCSS(this);">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtFirstNameORD" MaxLength="35" CssClass="modalInput" runat="server" onchange="javascript:validateCSS(this);">
                                                </asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnSearchORD" runat="server" CausesValidation="true" Text="Search"
                                                    CssClass="buttonBoxFocus" ValidationGroup="ProviderSearch"
                                                    OnClientClick="return GetOrderingProviderDetails()" Style="background-color: darkslateblue !important; height: 30px; margin-top: 7px; width: 55px" />


                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5">
                                                <asp:Label ID="lblErrorSearchORD" runat="server"></asp:Label>
                                                <asp:RequiredFieldValidator ID="rfvNPIORD" runat="server" ControlToValidate="txtNPI"
                                                    ErrorMessage="*Other provider is unknown" Text="*" Display="Dynamic"
                                                    ValidationGroup="valProviderHeader"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator runat="server" ID="revNPIORD" Display="Dynamic"
                                                    ValidationGroup="valProviderHeader" ControlToValidate="txtNPI" SetFocusOnError="true"
                                                    ValidationExpression="^[0-9]{10}$">
                                                </asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="background-color: darkcyan; width: 845px">
                                        <asp:Label ID="lblSResultORD" class="expandcollapse" runat="server" Text="SEARCH RESULTS"
                                            Visible="false" Style="font-weight: bold; font-size: 14px; color: white; padding-left: 20px;"></asp:Label>
                                    </div>
                                    <div class="providerNPIOutputResponsive">
                                        <div id="providerNPIOutput"></div>
                                        <asp:HiddenField ID="hdnProvSearchModalNPIORD" runat="server" />
                                        <asp:HiddenField ID="hdnProvSearchModalMedicaidIdORD" runat="server" />
                                        <asp:HiddenField ID="hdnProvSearchModalProviderNameORD" runat="server" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <!-- FACILITY TYPE SEARCH MODAL -->
            <div class="modal" id="FacilityTypeSearchModal" style="width: 100%;">
                <div class="modal-dialog">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div style="padding-bottom: 30px; background-color: lightblue">
                                <table class="modalbody" style="text-align: center; min-width: 100%;">
                                    <tr style="background-color: darkcyan; color: white; font-size: 14px; font-weight: bold; text-align: center;">
                                        <td>FACILITY TYPE CODE</td>
                                        <td>FACILITY TYPE DESCRIPTION</td>
                                        <td>
                                            <button id="facCloseModalId" type="button" class="close" data-dismiss="modal" runat="server" onclientclick="return hideFacilityType()">&times;</button></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFacilityTypeCode" CssClass="modalInput" runat="server"></asp:TextBox>
                                        </td>
                                        <td>

                                            <asp:TextBox ID="txtFacilityTypeDescription" MaxLength="100" CssClass="modalInput" runat="server"></asp:TextBox>


                                        </td>
                                        <td>
                                            <asp:Button ID="btnSearchFacilityType" runat="server" CausesValidation="true" Text="Search"
                                                CssClass="buttonBoxFocus" ValidationGroup="FacilityTypeSearch"
                                                OnClientClick="return GetFacilityTypes();" Style="background-color: darkslateblue !important; height: 40px;" />

                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <label id="FacilitySearchErrorMessage" style="font-weight: bold; color: #CC0505; display: none; font-size: 12px;">Either Facility Code or Description is Required to perform Search.</label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="background-color: darkcyan;">
                                <asp:Label ID="lblFacilitySResult" class="expandcollapse" runat="server" Text="SEARCH RESULTS"
                                    Visible="false" Style="font-weight: bold; font-size: 14px; color: white; padding-left: 20px;"></asp:Label>
                            </div>
                            <div style="background-color: lightblue;">
                                <div id="facilityTypeOutput"></div>

                            </div>
                            <asp:HiddenField ID="hdnfacilityType" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>


            <ajax:modalpopupextender id="mpeSubmitPriorAuthSearchProc" runat="server" popupcontrolid="pnlSubmitPriorAuthSearchProcPop" targetcontrolid="ButtonSearchProc" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseProc" />
            <asp:Panel ID="pnlSubmitPriorAuthSearchProcPop" runat="server" CssClass="modalPopup " Style="display: none; height: 50px; width: auto; min-height: 250px; min-width: 920px; overflow-y: auto">
                <asp:Panel ID="pnlSubmitPriorAuthSearchProcHeader" CssClass="popHeader popUpHeader" runat="server" HorizontalAlign="Left">
                    <asp:Button runat="server" ID="btnCloseProc" Text="X" CssClass="popUpClose" />
                    <div class="popTitle">
                        <asp:Label ID="lblSubmitPriorAuthSearchProc" CssClass="bodyTextBold" runat="server" Text="Procedure Code Search" ForeColor="White" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlSubmitPriorAuthSearchProc" runat="server">


                    <div>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ValidationGroup="CheckEligibility" ShowSummary="true" />
                    </div>

                    <div class="row m-0 popUpSearch-Context d-FlexCenter">
                        <div class="col-sm-6 col-md-4 col-lg-2 ">
                            <span class="ohio-field-label"><b>Procedure Code</b>

                                <asp:TextBox ID="txtCode" CssClass="ohio-field-input" MaxLength="7" runat="server"></asp:TextBox>

                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtCode" ID="revtxtCode" ForeColor="Red"
                                    ValidationExpression="^[\s\S]{0,7}$" runat="server" ValidationGroup="valprocedureCodeSearch"
                                    ErrorMessage="Allowed Max 7 characters"></asp:RegularExpressionValidator>
                            </span>
                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-8 ">
                            <span class="ohio-field-label"><b>Procedure Code Description</b>

                                <asp:TextBox ID="txtPlaceOfServiceName" CssClass="ohio-field-input" MaxLength="100" runat="server">
 
                                </asp:TextBox>
                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtPlaceOfServiceName" ID="revtxtPlaceOfServiceName" ForeColor="Red"
                                    ValidationExpression="^[\s\S]{0,100}$" runat="server" ValidationGroup="valprocedureCodeSearch"
                                    ErrorMessage="Max 100 characters"></asp:RegularExpressionValidator>
                            </span>
                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-2 text-center ">
                            <asp:Button ID="Button9" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus"
                                ValidationGroup="valprocedureCodeSearch" OnClientClick="return GetprocedureCodes('Institutional');" Style="background-color: darkslateblue !important" />

                        </div>
                        <div>
                            <asp:Label ID="lblProcCodeError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        </div>
                    </div>
                    <div class="search-Results">SEARCH RESULTS</div>
                    <asp:Label ID="Label2" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>

                    <div class="result-Container">
                        <div id="procedureCodeOutput"></div>

                    </div>
                </asp:Panel>
            </asp:Panel>
            <asp:Button runat="server" ID="ButtonSearchProc" Style="display: none" Text="ButtonSearchReson" />



            <ajax:modalpopupextender id="mpeServiceDetailsRevenueCodeSearch" runat="server" popupcontrolid="pnlDentalHSCPSCodeSearch1"
                targetcontrolid="button10" backgroundcssclass="modalBackground" />
            <asp:Panel ID="pnlDentalHSCPSCodeSearch1" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 920px; height: auto; width: auto;">
                <asp:Panel ID="Panel3" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                    <asp:Button runat="server" OnClientClick="return hideServiceDetailspopup('RevenueCode','Institutional')" ID="htnRevenueCodeClosePopup" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;" />
                    <div class="popTitle">
                        <asp:Label ID="Label3" CssClass="bodyTextBold" runat="server" Text="Revenue Code Search" ForeColor="White" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="Panel7" runat="server">

                    <div>
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ValidationGroup="CheckEligibility" ShowSummary="true" />
                    </div>
                    <div class="row m-0 popUpSearch-Context d-FlexCenter">
                        <div class="col-sm-6 col-md-4 col-lg-2 ">
                            <span class="ohio-field-label"><b>Revenue Code</b>

                                <asp:TextBox ID="txtHCPCSCode" MaxLength="4" CssClass="ohio-field-input" runat="server">
 
                                </asp:TextBox>

                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtHCPCSCode" ID="rxvtxtHCPCSCode" ForeColor="Red"
                                    ValidationExpression="^[\s\S]{0,7}$" runat="server" ValidationGroup="valRevenueCodeSearch"
                                    ErrorMessage="Allowed Max 7 characters"></asp:RegularExpressionValidator>
                            </span>
                            <div id="divRevenueCodeLoader"></div>
                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-8 ">
                            <span class="ohio-field-label"><b>Revenue Code Description</b>

                                <asp:TextBox ID="txtServDetRevCodeDesc" MaxLength="100" CssClass="ohio-field-input" runat="server">
 
                                </asp:TextBox>
                                <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtServDetRevCodeDesc" ID="rxvtxtServDetRevCodeDesc" ForeColor="Red"
                                    ValidationExpression="^[\s\S]{0,100}$" runat="server" ValidationGroup="valRevenueCodeSearch"
                                    ErrorMessage="Max 100 characters"></asp:RegularExpressionValidator>
                            </span>
                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-2 text-center ">
                            <asp:Button ID="btnRevCodeSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" ValidationGroup="valRevenueCodeSearch"
                                OnClientClick="return GetrevenueCodes();" Style="background-color: darkslateblue !important" />

                        </div>
                        <div>
                            <asp:Label ID="lblRevenueCodeError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        </div>
                        <br />
                    </div>
                    <div class="search-Results">SEARCH RESULTS</div>
                    <asp:Label ID="Label4" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>

                </asp:Panel>
            </asp:Panel>
            <asp:Button runat="server" ID="button10" Style="display: none" Text="ButtonDummy8" />



            <ajax:modalpopupextender id="mpeDiagnosisSearch1" runat="server" popupcontrolid="pnlDiagnosisSearch1"
                cancelcontrolid="htnDiagnosisClosePopup" targetcontrolid="button12" backgroundcssclass="modalBackground" />
            <asp:Panel ID="pnlDiagnosisSearch1" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 920px; height: 50px; width: auto; overflow-y: auto">
                <asp:Panel ID="Panel2" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                    <asp:Button runat="server" ID="htnDiagnosisClosePopup" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;" />
                    <div class="popTitle">
                        <asp:Label ID="Label6" CssClass="bodyTextBold" runat="server" Text="Diagnosis Code Search" ForeColor="White" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlDiagnosispopupBody" runat="server">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                        <ContentTemplate>
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <asp:HiddenField ID="HiddenField2" runat="server" />
                            <asp:HiddenField ID="HiddenField3" runat="server" />
                            <div class="modal-body" style="width: 100%;">
                                <div class="row">
                                    <div class="col-sm-6 col-md-4 col-lg-3">
                                        <span class="ohio-field-label" style="margin: 0;"><span style="color: #CC0505"></span><b>Diagnosis Code</b>
                                            <asp:TextBox ID="txtDiagnosisCodeSearch1" MaxLength="7" CssClass="ohio-field-input" onchange="javascript:validateCSS(this);" runat="server">
                                            </asp:TextBox>
                                        </span>
                                    </div>
                                    <div class="col-sm-6 col-md-4 col-lg-3">
                                        <span class="ohio-field-label" style="margin: 0;"><b>ICD Version </b>

                                        </span>
                                        <asp:Label ID="lblICDVersion" CssClass="ohio-field-label" runat="server" Style="margin: 0;">ICD 10</asp:Label>
                                    </div>
                                    <div class="col-sm-6 col-md-4 col-lg-3">
                                        <span class="ohio-field-label" style="margin: 0;"><span style="color: #CC0505"></span><b>Diagnosis Code Description</b>

                                            <asp:TextBox ID="txtDiagnosisCodeDescSearch1" MaxLength="100" CssClass="ohio-field-input" runat="server" onchange="javascript:validateCSS(this);">

                                            </asp:TextBox>
                                            <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtDiagnosisCodeDescSearch1" ID="RegularExpressionValidator6" ForeColor="Red"
                                                ValidationExpression="^[\s\S]{0,100}$" runat="server" ValidationGroup="valDiagnosisCodeSearch"
                                                ErrorMessage="Max 100 characters"></asp:RegularExpressionValidator>
                                        </span>
                                    </div>
                                    <div class="col-sm-6 col-md-4 col-lg-3">
                                        <div style="height: 20px;"></div>

                                        <asp:Button ID="btndiagnosiscodeSearch" runat="server" CausesValidation="true" Text="Search" Visible="true" CssClass="buttonBoxFocus"
                                            OnClientClick="return GetDiagnosisDetails()" Style="background-color: darkslateblue !important" />
                                    </div>
                                    <div>
                                        <asp:Label ID="lbldiagnosiscodeError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                    </div>
                                </div>
                                <asp:Label ID="Label7" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>
                                <div style="margin: 20px;">
                                    <div id="DiagnosisOutput">
                                    </div>

                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </asp:Panel>

            <asp:Button runat="server" ID="button12" Style="display: none" Text="ButtonDummy8" />

            <ajax:modalpopupextender id="modalPATXNSuccess" runat="server" popupcontrolid="pnmPATXNSuccess" targetcontrolid="ButtonDummyPATXNSuccess"
                backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnmPATXNSuccess" cancelcontrolid="hdnCloseSuccess">
            </ajax:modalpopupextender>
            <asp:Panel ID="pnmPATXNSuccess" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 25%;">
                <div class="row">
                    <div class="col-sm-6 col-md-4 col-lg-3">
                        <asp:Image ID="ImageTXCheck" runat="server" Width="30%" Height="30%" ImageUrl="~/Images/check.png" />
                    </div>
                    <div class="col-sm-6 col-md-8 col-lg-9">
                        <asp:Label ID="lblTXNResponse" runat="server" Font-Bold="true"></asp:Label>
                    </div>
                </div>
                <asp:LinkButton ID="btnCloseSuccess" runat="server" Text="Close" OnClick="btnCloseSuccess_Click" />
            </asp:Panel>
            <asp:Button runat="server" ID="ButtonDummyPATXNSuccess" Style="display: none" />
            <asp:HiddenField runat="server" ID="hdnCloseSuccess" />

            <ajax:modalpopupextender id="mdlpnlReSubmitConfirm" runat="server" popupcontrolid="pnlReSubmitConfirm" targetcontrolid="btnDumReSubmitConfirm"
                backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnlReSubmitConfirm" cancelcontrolid="btnReSubmitConfirmNo">
            </ajax:modalpopupextender>
            <asp:Panel ID="pnlReSubmitConfirm" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 25%;">
                <div class="row">
                    <asp:Label ID="Label8" runat="server" Font-Bold="true" Text="You are sending an update to the original request. Are you sure you want to update this request?"></asp:Label>
                </div>
                <asp:Button ID="btnReSubmitConfirmYes" OnClick="btnReSubmitConfirmYes_Click" runat="server" Text="Yes" CssClass="buttonBoxFocusgreen" />
                <asp:Button ID="btnReSubmitConfirmNo" OnClick="btnReSubmitConfirmNo_Click" runat="server" Text="No" CssClass="buttonBoxFocusred" />
            </asp:Panel>
            <asp:Button runat="server" ID="btnDumReSubmitConfirm" Style="display: none" />


            <ajax:modalpopupextender id="mdlCancelPARequest1" runat="server" popupcontrolid="pnlCancelPARequ1" targetcontrolid="btnDumpnlCancelPARequ1"
                backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnlCancelPARequ1" cancelcontrolid="btnCancelPARequNo1">
            </ajax:modalpopupextender>
            <asp:Panel ID="pnlCancelPARequ1" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 25%;">
                <div class="row">
                    <asp:Label ID="Label11" runat="server" Font-Bold="true" Text="Cancelling will permanently close this request. Are you sure you want to cancel this request?"></asp:Label>
                </div>
                <asp:Button ID="btnCancelPARequYes1" OnClick="btnCancelPARequYes1_Click" runat="server" Text="Yes" CssClass="buttonBoxFocusgreen" />
                <asp:Button ID="btnCancelPARequNo1" runat="server" Text="No" CssClass="buttonBoxFocusred" OnClientClick="return hideCancelPopUp();" />
            </asp:Panel>
            <asp:Button runat="server" ID="btnDumpnlCancelPARequ1" Style="display: none" />


            <ajax:modalpopupextender id="mdlCancelPARequest" runat="server" popupcontrolid="pnlCancelPARequ" targetcontrolid="btnDumpnlCancelPARequ"
                backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnlCancelPARequ" cancelcontrolid="btnCancelPARequNo">
            </ajax:modalpopupextender>
            <asp:Panel ID="pnlCancelPARequ" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 25%;">
                <div class="row">
                    <asp:Label ID="Label9" runat="server" Font-Bold="true" Text="Cancelling will permanently close this request. Are you sure you want to cancel this request?"></asp:Label>
                </div>
                <asp:Button ID="btnCancelPARequYes" OnClick="btnCancelPARequYes_Click" runat="server" Text="Yes" CssClass="buttonBoxFocusgreen" />
                <asp:Button ID="btnCancelPARequNo" OnClick="btnCancelPARequNo_Click" runat="server" Text="No" CssClass="buttonBoxFocusred" />
            </asp:Panel>
            <asp:Button runat="server" ID="btnDumpnlCancelPARequ" Style="display: none" />


            <ajax:modalpopupextender id="mdlpnlUpdatePARequest" runat="server" popupcontrolid="pnlUpdatePARequest" targetcontrolid="btlDumlUpdatePARequest"
                backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnlUpdatePARequest" cancelcontrolid="btnUpdatePARequestNo">
            </ajax:modalpopupextender>
            <asp:Panel ID="pnlUpdatePARequest" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 25%;">
                <div class="row">
                    <asp:Label ID="Label10" runat="server" Font-Bold="true" Text="You are sending an update to the original request. Are you sure you want to update this request?"></asp:Label>
                </div>
                <asp:Button ID="btnUpdatePARequestYes" OnClick="btnUpdatePARequestYes_Click" runat="server" Text="Yes" CssClass="buttonBoxFocusgreen" />
                <asp:Button ID="btnUpdatePARequestNo" OnClick="btnUpdatePARequestNo_Click" runat="server" Text="No" CssClass="buttonBoxFocusred" />
            </asp:Panel>
            <asp:Button runat="server" ID="btlDumlUpdatePARequest" Style="display: none" />


            <ajax:modalpopupextender id="mdlWarningAcknowledgment" runat="server" popupcontrolid="pnlWarningAcknowledgment" targetcontrolid="btnDumWarningAcknowledgment"
                backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnlWarningAcknowledgment" cancelcontrolid="btnWarningAcknowledgmentNo">
            </ajax:modalpopupextender>
            <asp:Panel ID="pnlWarningAcknowledgment" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 40%;">
                <div class="row">
                    <asp:Label ID="lblWarningAcknowledgment" runat="server" Font-Bold="true" Text=""></asp:Label>
                </div>
                <br />
                <br />
                <asp:Button ID="btnWarningAcknowledgmentYes" OnClick="btnWarningAcknowledgmentYes_Click" runat="server" Text="Acknowledge" CssClass="buttonBoxFocusgreen" />
                <asp:Button ID="btnWarningAcknowledgmentNo" OnClick="btnWarningAcknowledgmentNo_Click" runat="server" Text="Cancel" CssClass="buttonBoxFocusred" />
            </asp:Panel>
            <asp:Button runat="server" ID="btnDumWarningAcknowledgment" Style="display: none" />


            <ajax:modalpopupextender id="mpeServiceinfoPlaceofService" runat="server" popupcontrolid="pnlplaceofServiceSearch"
                cancelcontrolid="htnplaceofserviceClosePopup" targetcontrolid="btnplaceofServiceDummy" backgroundcssclass="modalBackground" />
            <asp:Panel ID="pnlplaceofServiceSearch" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 920px; height: auto; width: auto;">
                <asp:Panel ID="Panel8" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                    <asp:Button runat="server" ID="htnplaceofserviceClosePopup" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;" />
                    <div class="popTitle">
                        <asp:Label ID="Label12" CssClass="bodyTextBold" runat="server" Text="Place of Service Search" ForeColor="White" />
                    </div>
                </asp:Panel>
                <asp:UpdatePanel runat="server" ID="upServiceInfo">
                    <ContentTemplate>
                        <asp:Panel ID="Panel9" runat="server">
                            <div class="row m-0 popUpSearch-Context d-FlexCenter">
                                <div class="col-sm-6 col-md-4 col-lg-2 ">
                                    <span class="ohio-field-label"><b>Place Of Service Code</b>
                                        <asp:TextBox ID="txtPlaceOfServiceCode" CssClass="ohio-field-input" runat="server" onchange="javascript:validateCSS(this);">
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtPlaceOfServiceCode" ID="RegularExpressionValidator8" ForeColor="Red"
                                            ValidationExpression="^[\s\S]{0,7}$" runat="server" ValidationGroup="valPlaceOfServiceSearch"
                                            ErrorMessage="Allowed Max 7 characters"></asp:RegularExpressionValidator>
                                    </span>
                                </div>

                                <div class="col-sm-6 col-md-4 col-lg-8 ">
                                    <span class="ohio-field-label"><b>Place Of Service Description</b>

                                        <asp:TextBox ID="txtPlaceOfServiceDesc" MaxLength="100" CssClass="ohio-field-input" runat="server" onchange="javascript:validateCSS(this);">
 
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtPlaceOfServiceDesc" ID="RegularExpressionValidator9" ForeColor="Red"
                                            ValidationExpression="^[\s\S]{0,100}$" runat="server" ValidationGroup="valPlaceOfServiceSearch"
                                            ErrorMessage="Max 100 characters"></asp:RegularExpressionValidator>
                                    </span>
                                </div>

                                <div class="col-sm-6 col-md-4 col-lg-2 text-center ">
                                    <asp:Button ID="btnPlaceOfServiceSearch" runat="server" Text="Search" Visible="false" CssClass="buttonBoxFocus"
                                        OnClientClick="return GetPlaceOfServiceDetails()" Style="background-color: darkslateblue !important" />
                                    <button type="button" class="buttonBoxFocus" onclick="GetPlaceOfServiceDetails()">Search</button>
                                </div>
                                <div>
                                    <asp:Label ID="lblPlaceOfServiceSearchError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                </div>
                                <br />
                            </div>
                            <div style="background-color: darkcyan;">
                                <asp:Label ID="lblSrchPlc" class="expandcollapse" runat="server" Text="SEARCH RESULTS"
                                    Visible="false" Style="font-weight: bold; font-size: 14px; color: white; padding-left: 20px;"></asp:Label>
                            </div>

                            <div class="result-Container">
                                <div id="output"></div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:Button runat="server" ID="btnplaceofServiceDummy" Style="display: none" Text="ButtonDummy8" />

            <ajax:modalpopupextender id="modalPATXNFailure" runat="server" popupcontrolid="pnmPATXNFailure" targetcontrolid="ButtonDummyPATXNFailure"
                backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnmPATXNFailure" cancelcontrolid="btnCloseFailure">
            </ajax:modalpopupextender>
            <asp:Panel ID="pnmPATXNFailure" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 25%;">
                <div class="row">
                    <div class="col-sm-6 col-md-4 col-lg-3">
                        <asp:Image ID="ImageTXCancel" runat="server" Width="30%" Height="30%" ImageUrl="~/Images/cancel.png" />
                    </div>
                    <div class="col-sm-6 col-md-8 col-lg-9">
                        <asp:Label ID="lblTXNResponseID" runat="server" Font-Bold="true" Text=""></asp:Label><br />
                        <asp:Label ID="Label15" runat="server" Font-Bold="true" Text="Error"></asp:Label><br />
                        <asp:GridView ID="grdTXNResponse" runat="server" ShowHeader="false" GridLines="None" BorderStyle="None" RowStyle-BackColor="White" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="ErrorCode" HeaderText="ErrorCode" />
                                <asp:BoundField DataField="ErrorDescription" HeaderText="ErrorDescp" />
                            </Columns>
                        </asp:GridView>
                        <asp:Label ID="lblTXNErrors" runat="server" Font-Bold="true" Text=""></asp:Label><br />
                        <asp:Label ID="lblSupport" runat="server" Font-Bold="true" Text=""></asp:Label>
                    </div>
                </div>
                <asp:LinkButton ID="btnCloseFailure" runat="server" Text="Close" />
            </asp:Panel>
            <asp:Button runat="server" ID="ButtonDummyPATXNFailure" Style="display: none" />
        </div>
    </div>
</div>
<cc2:messagebox id="MessageBox2" runat="server" />
<msgcc2:intuitivepriorauthmessagebox id="IntuitivePriorAuthMessageBoxID" runat="server" />
