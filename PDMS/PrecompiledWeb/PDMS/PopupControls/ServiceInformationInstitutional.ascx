<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ServiceInformationInstitutional, App_Web_tiu3g34i" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/TypeOfBill.ascx" TagPrefix="uc" TagName="TypeOfBill" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">

   

<script type="text/javascript">
    var codeTypeB = "TypeOfBill";
    var claimTypeB = "Institutional";
    var isValidBillCode = true;

    function frmDataChange(){

        loader2ServiceInformationInstitutional();
        document.getElementById('<%= btnloading1.ClientID %>').style.display = 'block';

    }

    function admDateChange() {

        var test1 = $("#<%= ddlPatientStatus.ClientID %>").first().val();
        
        if ((test1 !== null) && (test1 !== "")) {
            
            $("#<%= releaseInformationError.ClientID %>").text(""); 
            $("#<%= ddlReleaseOfInfo.ClientID %>").css({ "background-color": "white" });
        }

        if (($("#<%= txtFromDate.ClientID %>").val() !== null) && ($("#<%= txtFromDate.ClientID %>").val() !== "")) {
            $("#<%= fromDateRequiredError.ClientID %>").text("");
            $("#<%= txtFromDate.ClientID %>").css({ "background-color": "white" });            
        }
        if (($("#<%= txtToDate.ClientID %>").val() !== null) && ($("#<%= txtToDate.ClientID %>").val() !== "")) {
            $("#<%= toDateRequiredError.ClientID %>").text("");
            $("#<%= txtToDate.ClientID %>").css({ "background-color": "white" });           
        }
        if (($("#<%= ddlPatientStatus.ClientID %>").first().val() !== null) && ($("#<%= ddlPatientStatus.ClientID %>").first().val() !== "")) {
            $("#<%= patientStatusError.ClientID %>").text("");
            $("#<%= ddlPatientStatus.ClientID %>").css({ "background-color": "white" });          
        }
        if (($("#<%= ddlAdmissionType.ClientID %>").first().val() !== null) && ($("#<%= ddlAdmissionType.ClientID %>").first().val() !== "")) {
            $("#<%= admissionTypeError.ClientID %>").text("");
            $("#<%= ddlAdmissionType.ClientID %>").css({ "background-color": "white" });
        }
        if (($("#<%= ddlAdmitSource.ClientID %>").first().val() !== null) && ($("#<%= ddlAdmitSource.ClientID %>").first().val() !== "")) {
            $("#<%= admitsourceError.ClientID %>").text("");
            $("#<%= ddlAdmitSource.ClientID %>").css({ "background-color": "white" }); 
        }
        if ((($("#<%= txtAdmissionDate.ClientID %>").val() !== null) && ($("#<%= txtAdmissionDate.ClientID %>").val() !== "")) && (($("#<%= txtAdmissionHr.ClientID %>").val() !== null) && ($("#<%= txtAdmissionHr.ClientID %>").val() !== ""))) {
            $("#<%= lblServiceInfoError.ClientID %>").text("");
        }
        if (($("#<%= txtAdmissionDate.ClientID %>").val() !== null) && ($("#<%= txtAdmissionDate.ClientID %>").val() !== "")) {             
            validateDate22();
            return false;
        }
        else {
            $("#<%= lblFromToDate %>").css('display', 'none');           
        }

    }

    function validateFields() {

        var txtAdmissionDate = document.getElementById("<%=txtAdmissionDate.ClientID %>").value;
        var txtAdmissionHr = document.getElementById("<%=txtAdmissionHr.ClientID %>").value;
      
        if ((txtAdmissionDate != null) && (txtAdmissionDate != "") && (txtAdmissionHr != null) && (txtAdmissionHr != ""))
        {
          
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblServiceInfoError').text('');

        }
    }
    function ddlAdmissionTypeSelectedIndexChanged() {
        var ddlAdmissionType = $("#<%=ddlAdmissionType.ClientID %> option:selected").text();

        if ((ddlAdmissionType != "") && (ddlAdmissionType != null)) {
            document.getElementById('<%= ddlAdmitSource.ClientID %>').disabled = false;

            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_admissionTypeError').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_ddlAdmissionType').css("background-color", "white");
        }
        else if ((ddlAdmissionType === "") || (ddlAdmissionType === null)) {
            document.getElementById('<%= ddlAdmitSource.ClientID %>').disabled = true;
        }
        var value = document.getElementById("<%=ddlAdmissionType.ClientID %>").value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetAdmissionType?AdmissionType=" + value,
            //data: '{AdmissionType: "' + value + '" }',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var ddlAdmitSource = document.getElementById("<%=ddlAdmitSource.ClientID %>");
                ddlAdmitSource.options.length = 0;
                $("#<%=ddlAdmitSource.ClientID %>").append('<option value ="-1"> </option>');
                for (var i = 0; i < result.length; i++) {
                    var code = result[i].Payer_code.length > 125 ? result[i].Payer_code.substr(0, 125) + "..." : result[i].Payer_code;
                    $("#<%=ddlAdmitSource.ClientID %>").append("<option value" + result[i].Payer_desc + ">" + code + "</option>");
                    var AdmitSourceDesc = result[i].Payer_desc;
                    var AdmitSourceCode = result[i].Payer_code;
                    $("#<%=hdnAdmissionTypeText.ClientID %>").val(AdmitSourceDesc);
                    $("#<%=hdnAdmissionTypeValue.ClientID %>").val(AdmitSourceCode);
                };
             }

         });
    }
    function admitSrcStoreval() {
        var admitSrc = $("#<%=ddlAdmitSource.ClientID %> option:selected").text();
        $("#<%= hdnAdmitSrc.ClientID %>").val(admitSrc);
        if ((admitSrc != "") && (admitSrc != null)) {

            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_admitsourceError').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_ddlAdmitSource').css("background-color", "white");
        }
       
    }
    function loader2ServiceInformationInstitutional() {

        document.getElementById('<%= txtTypeofBill.ClientID %>').disabled = true;
         document.getElementById('<%= ddlReleaseOfInfo.ClientID %>').disabled = true;
         document.getElementById('<%= txtFromDate.ClientID %>').disabled = true;
         document.getElementById('<%= txtToDate.ClientID %>').disabled = true;
         document.getElementById('<%= ddlPatientStatus.ClientID %>').disabled = true;
         document.getElementById('<%= txtAdmissionDate.ClientID %>').disabled = true;
         document.getElementById('<%= txtDischargeHr.ClientID %>').disabled = true;
        document.getElementById('<%= ddlAdmissionType.ClientID %>').disabled = true;
        document.getElementById('<%= ddlAdmitSource.ClientID %>').disabled = true;
        document.getElementById('<%= txtPatPaidAmt.ClientID %>').disabled = true;
        document.getElementById('<%= txtSubmittedDRG.ClientID %>').disabled = true;
        document.getElementById('<%= lblFinalDRG.ClientID %>').disabled = true;
        
        
     }


    function onlyDotsAndNumbers(txt, event) {
        var charCode = (event.which) ? event.which : event.keyCode
        if (charCode == 46) {
            if (txt.value.indexOf(".") < 0)
                return true;
            else
                return false;
        }
        if (txt.value.indexOf(".") > 0) {
            var txtlen = txt.value.length;
            var dotpos = txt.value.indexOf(".");
            //Change the number here to allow more decimal points than 2
            if ((txtlen - dotpos) > 2)
                return false;
        }
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }
    function visibleTypeOfBillCode() {
        $("#<%=ucTypeOfBill.FindControl("gvSubmitClaimSearchPop").ClientID %>").html("");
        localStorage.setItem("typeOfBill", "");
        $find("mpeTypeOfBillCode").show();
        return false;

    }

    function GetTypeOfBillInfo() {
        var txtTypeOfBillCode = $("#<%= ucTypeOfBill.FindControl("txtSearchTOBCode").ClientID %>").first().val();

        var txtTypeOfBilldesc = $("#<%= ucTypeOfBill.FindControl("txtSearchTOBDesc").ClientID %>").first().val();

        $("#<%=ucTypeOfBill.FindControl("gvSubmitClaimSearchPop").ClientID %>").html("");
        var APIToken = $("[id*=hdnAccessToken]").val();
         $.ajax({
             type: "GET",
             url: webApiClaims + "GetTypeOfBillDetails?desc=" + txtTypeOfBilldesc + "&&val=" + txtTypeOfBillCode,
             //data: '{desc: "' + txtTypeOfBilldesc + '" , val: "' + txtTypeOfBillCode + '" }',
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
                     $("#<%=ucTypeOfBill.FindControl("gvSubmitClaimSearchPop").ClientID %>").append("<tr><td> No Records Found </td></tr>");
                }
                else {
                     $("#<%=ucTypeOfBill.FindControl("gvSubmitClaimSearchPop").ClientID %>").append("<tr><th>Type Of Bill</th><th>Type Of Bill Description </th></tr>");
                     for (var i = 0; i < result.length; i++) {                         
                         $("#<%=ucTypeOfBill.FindControl("gvSubmitClaimSearchPop").ClientID %>").append("<tr><td><a onClick='GetTypeOfBillCode(this); return false;'>" + result[i].TypeOfBill_Code + "</asp:LinkButton></td><td>" + result[i].TypeOfBill_Desc + "</td></tr>");
                     };
                 }
             },
             error: function (jqXHR, textStatus, errorThrown) {
                 $("[id*=lblmessage]").text('TypeOfBill code not found.');
             }
         });

         return false;
    }

    function GetTypeOfBillCode(lnk) {

        $find("mpeTypeOfBillCode").hide();

        var gridindexProccode = localStorage.getItem("typeOfBill");

            var textboxrow = lnk.parentNode.parentNode;
            $("[id*=txtTypeofBill]").val(textboxrow.cells[0].innerText.trim());

            return false;
    }

    function ddlReleaseOfInfoChanged() {
        var ddlReleaseOfInfo = $("#<%=ddlReleaseOfInfo.ClientID %> option:selected").text();
     
        if ((ddlReleaseOfInfo != "") && (ddlReleaseOfInfo != null)) {
           
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_releaseInformationError').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_ddlReleaseOfInfo').css("background-color", "white");
        }
    }

    
    function patientStatusChanged() {
        var ddlPatientStatus = $("#<%=ddlPatientStatus.ClientID %> option:selected").text();

        if ((ddlPatientStatus != "") && (ddlPatientStatus != null)) {
            
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_patientStatusError').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_ddlPatientStatus').css("background-color", "white");
         }
     }

    function CheckTypeBill()
    {
        $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblInstErr').text('');
         if (($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().length == 4)
            && ($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(3, 4) == '7' ||
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtTypeofBill').val().substr(3, 4) == '8'))
         {
             if (($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_ClaimFrequencyCode').val() !== '7')
                 && ($('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_ClaimFrequencyCode').val() !== '8')) {

                 $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblInstTypeBill').text('Search and inquire the claim to adjust or void.');
             }
 }
        else {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblInstTypeBill').text('');
        }
    }

    function toDateVerify() {
        var daterequested = document.getElementById("<%= txtToDate.ClientID%>").value;
        if ((document.getElementById("<%= txtToDate.ClientID%>") != null) || (document.getElementById("<%= txtToDate.ClientID%>") != ""))
        { 
        var Todate = document.getElementById("<%= txtToDate.ClientID%>");
        if ((ToDate != null) || (ToDate != ""))
        {
           
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_toDateRequiredError').text('');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtToDate').css("background-color", "white");

        }

        var today = new Date();
        var dd = today.getDate().toString().padStart(2, '0');
        var mm = (today.getMonth()+1).toString().padStart(2, '0');
        var yyyy = today.getFullYear();
        var todaydate = mm + '/' + dd + '/' + yyyy;

        var FromDate = $("#<%= txtFromDate.ClientID %>").first().val();
        var ToDate = $("#<%= txtToDate.ClientID %>").first().val();

        if (Date.parse(ToDate) > Date.parse(todaydate)) {

            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_toDateFutureDateError').text('Future date not allowed');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtToDate').val('');
        }
        else { $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_toDateFutureDateError').text(''); }

            if (Date.parse(FromDate) > Date.parse(ToDate)) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_toDateCompareError').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_toDateCompareError').text('To Date must be greater or equal From Date');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtToDate').val('');

        }
        else {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_toDateCompareError').text('');

        }
       
            var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_InvalidError').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_InvalidError').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtToDate').val('');
        }
        else {

            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_InvalidError').css('display', 'none');

            }
            var daterequested = document.getElementById("<%= txtAdmissionDate.ClientID%>").value;
            if (daterequested != null && daterequested != '' && daterequested != undefined) {
                if (new Date(daterequested) > new Date(ToDate)) {
                    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblFromToDate').css('display', 'block');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblFromToDate').text("*Admission Date and hour is Invalid(Greater Than ToDate)");
                    $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionDate').val('');
                }
            }
        }
    }

    function validateDate() {
        var daterequested = document.getElementById("<%= txtFromDate.ClientID%>").value;
        var ToDate = $("#<%= txtToDate.ClientID %>").first().val();
        var tdate = new Date();
        var dd = tdate.getDate();
        var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
        var yyyy = tdate.getFullYear();
        var currentDate = (MM) + "/" + dd + "/" + yyyy;
       
        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_fromDateFutureDateError').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_fromDateFutureDateError').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtFromDate').val('');
        }
        else {

            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_fromDateFutureDateError').css('display', 'none');

              }
        if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_fromDateFutureDateError').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_fromDateFutureDateError').text("*Future date not allowed");
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtFromDate').val('');
        }
        if ((ToDate != null) && (ToDate != "") && (ToDate != undefined)) {
            if (Date.parse(daterequested) > Date.parse(ToDate)) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_toDateCompareError').css('display', 'block');

                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_toDateCompareError').text('To Date must be greater or equal From Date');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtFromDate').val('');

            }
            else {
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_toDateCompareError').text('');

            }
        }
        var daterequestedAdmissionDate = document.getElementById("<%= txtAdmissionDate.ClientID%>").value;
        if (daterequestedAdmissionDate != null && daterequestedAdmissionDate != '' && daterequestedAdmissionDate != undefined) {
            if (new Date(daterequestedAdmissionDate) > new Date(ToDate)) {
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblFromToDate').css('display', 'block');
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblFromToDate').text("*Admission Date and hour is Invalid(Greater Than ToDate)");
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionDate').val('');
            }
        }
        
    }
    function validateDate22() {
        var daterequested = document.getElementById("<%= txtAdmissionDate.ClientID%>").value;
        var Fromdate = document.getElementById("<%= txtFromDate.ClientID%>").value;
        var Todate = document.getElementById("<%= txtToDate.ClientID%>").value;
        var tdate = new Date();
        var dd = tdate.getDate();
        var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
        var yyyy = tdate.getFullYear();
        var currentDate = (MM) + "/" + dd + "/" + yyyy;

        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblFromToDate').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblFromToDate').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionDate').val('');
        }
        else {

            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblFromToDate').css('display', 'none');

        }
        /*if (new Date(daterequested) > new Date(Fromdate) || new Date(daterequested) > new Date(Todate)) {*/
        if ( new Date(daterequested) > new Date(Todate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblFromToDate').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblFromToDate').text("*Admission Date and hour is Invalid(Greater Than ToDate)");
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionDate').val('');
        }
        if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblFromToDate').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_lblFromToDate').text("*Future date not allowed");
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtAdmissionDate').val('');
        }
        
    }
    function txtTypeofBillTextChanged(){
        var TypeOfBill = document.getElementById("<%=txtTypeofBill.ClientID %>").value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        isValidBillCode = true;
        $.ajax({
            type: "GET",
            url: webApiClaims + "ValidationForTypeOfBill?TypeOfBill=" + TypeOfBill,
            //data: '{TypeOfBill: "' + TypeOfBill + '" }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {               
                if (result === "") {
                    $("[id*=lblInstErr]").text(""); 

                    //if Place of Service code is valid then
                    //Get SpecialtyTypes and check the rules for each specialty type
                    //alert('Before Validate');
                    ValidateTypeOfBillCode();

                    if (isValidBillCode == true) {
                        $("[id*=lblInstErr]").text('');
                    }              

                }
                else {
                    $("[id*=lblInstErr]").text(result);
                    
                    if (TypeOfBill.length < 4) {
                        document.getElementById('<%= typeOfBillNumberOfDigitsError.ClientID %>').visible = false;
                    }
                    else {
                        if (document.getElementById('<%= typeOfBillNumberOfDigitsError.ClientID %>') != null) 
                        document.getElementById('<%= typeOfBillNumberOfDigitsError.ClientID %>').visible = true;
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }

    function ValidateTypeOfBillCode() {
        //alert('In Validate: NPI=' + Npi);
        //alert('In Validate: MedId=' + MedId);
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetSpecialtyTypes?medId=" + MedId + "&&npi=" + Npi,
            //data: '{desc: "" , val: "' + txtRevenueCode + '" }',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result1) {
                if (result1.length == 0) {
                    //Don't do anything as there're no specialties
                }
                else if (result1.length > 0) {
                    //Check the CodeSet Rules here for Place Of Service Code, if is valid for one then stop otherwise check all specialties
                    //alert('In loop' + result1.length);
                    for (var i = 0; i < result1.length; i++) {
                        var SpecialtyTypeId = result1[i].SpecialtyTypeId;
                        if (isValidBillCode == true) {
                            //alert('i: ' + i);
                            GetBillCodeSetRule(SpecialtyTypeId);
                        }
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblInstErr]").text('Error validating Type of Bill code.');
            }
        });
    }

    function GetBillCodeSetRule(SpecialtyTypeId) {
        var TypeOfBill = document.getElementById("<%=txtTypeofBill.ClientID %>").value;
        var providerTypeId = $("[id*=hdnProviderTypeId]").val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        //alert("CodeType: " + codeTypeB);
        //alert("claimTypeB: " + claimTypeB);
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetCodeSetRule?providerTypeId=" + providerTypeId + "&&specialtyTypeId=" + SpecialtyTypeId + "&&code=" + TypeOfBill + "&&codeType=" + codeTypeB + "&&claimsorPA=" + claimsOrPA + "&&claimORPAType=" + claimTypeB,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result2) {
                //alert("Result Length: " + result.length)
                if (result2.length == 0) {
                    //No rule then it is not good for Place of Service
                }
                else if (result2.length == 1) {
                    //Check if isAllows
                    var isAllowed = result2[0].Allowed;
                    var errorMsg = result2[0].ErrorMessage;
                    //alert('isAllowed: ' + isAllowed);
                    if (isAllowed == "False") {
                        isValidBillCode = false;
                        $("[id*=lblInstErr]").text(errorMsg);
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblInstErr]").text('Error validating Type of Bill code.');
            }
        });
    }

    $(document).ready(function () {
        $("option").text(function (i, t) {
            //Optional set full text on title attr
            $(this).attr("title", t);
            return t.length > 100 ? t.substr(0, 100) + "..." : t;
        });
    });
</script>

<div class="row">
     <div>
     <asp:Label ID="lblErrorMsg3" runat="server" ForeColor="Red" Style="margin-left: 20px;"></asp:Label>
     </div>
    <div class="col-sm-4">
        <div class="row">
            <div class="col-sm-5">
                <asp:HiddenField ID="hdnClaimId" runat="server" />
                <asp:HiddenField runat="server" ID="hdnAdmissionTypeText" />
                <asp:HiddenField runat="server" ID="hdnAdmissionTypeValue" />
                <asp:HiddenField runat="server" ID="hdnClaimFrequencyCode" />

                

                <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: red">* </span>Type of Bill</span>
            </div>
           
            <div class="col-sm-7">
                <span style="text-align: right;">
                    <asp:TextBox ID="txtTypeofBill" runat="server" CssClass="formfieldDate" MaxLength="4" Style="height: 30px; width: 150px !important"   onChange="CheckTypeBill();txtTypeofBillTextChanged()" onKeydown="return (!(event.keyCode>=65 && event.keyCode <=90) && event.keyCode!=32);" />
                     
                     
                   <asp:LinkButton ID="lnkPlaceofServiceSearch" Text="Search"   style="font-size: 14px; " runat="server" ToolTip="Search" OnClientClick="return visibleTypeOfBillCode()" CommandArgument='<%# Eval("CDE_POS") %>' Visible="true" CausesValidation="False"></asp:LinkButton>
                    <asp:Label ID="typeOfBillNumberOfDigitsError" runat="server" Text="4-digits number is required for Type of Bill" CssClass="error-message" Visible="false"></asp:Label>
                  <br />
                    <asp:Label runat="server" ID="lblInstErr" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="lblInstTypeBill" ForeColor="Red"></asp:Label>

                    
                    


                </span>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-5">
                <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: red">* </span> Release of Information</span>
            </div>
            <div class="col-sm-7">
                <span style="text-align: left;">
                    <asp:DropDownList ID="ddlReleaseOfInfo" EnableViewState="true" runat="server"
                        AppendDataBoundItems="True" Style="min-width: 150px; height: 30px" OnChange="ddlReleaseOfInfoChanged()">
                        <asp:ListItem Value="0" Text=""></asp:ListItem>
                        <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                        <asp:ListItem Value="N" Text="No" />
                    </asp:DropDownList>
                     <asp:Label ID="releaseInformationError" runat="server" Text="" CssClass="error-message"></asp:Label>
                  
                </span>
            </div>

        </div>
        <div class="row">
            <div class="col-sm-5">
                <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: red">* </span>From Date</span>
            </div>
           <div class="col-sm-7">
                <span style="text-align: left;">
                    <asp:TextBox ID="txtFromDate" OnChange="validateDate()" AutoComplete="off"  runat="server" CssClass="formfieldDate" Style="height: 30px;  width: 150px !important"  />
                    <asp:Image ID="imgSericeFromDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                    <ajax:CalendarExtender ID="ceFromDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtFromDate" PopupPosition="BottomLeft" CssClass="QstCalendarCSS" EnabledOnClient="true" />
                     <asp:Label ID="fromDateRequiredError" runat="server" Text="" CssClass="error-message" ></asp:Label>
                    <br />
 
   <button id="btnloading1" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                     
              <asp:Label ID="fromDateFutureDateError" runat="server" CssClass="error-message" Text="" ></asp:Label>
           
                </span>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-5">
                <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: red">* </span>To Date</span>
            </div>
           <div class="col-sm-7">
                <span style="text-align: left;">
                     <asp:TextBox ID="txtToDate" runat="server" AutoComplete="off" CssClass="formfieldDate" Style="height: 30px; width: 150px !important"  onChange="toDateVerify();"></asp:TextBox>   
                    <asp:Image ID="imgServiceToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                    <ajax:CalendarExtender ID="ceToDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtToDate" PopupPosition="BottomRight" CssClass="QstCalendarCSS" EnabledOnClient="true" />
                   
                     <asp:Label ID="toDateRequiredError" runat="server" Text="" CssClass="error-message"></asp:Label>
                    <br />
                   
                     <asp:Label ID="toDateFutureDateError" runat="server" CssClass="error-message" ></asp:Label>
                     <asp:Label ID="InvalidError" runat="server" Text="" CssClass="error-message" ></asp:Label>
                    <br />
                     <asp:Label ID="toDateCompareError" runat="server" Text="" CssClass="error-message"></asp:Label>
                    
                    
                  
                </span>
            </div>


        </div>
       
    </div>
    <div class="col-sm-4">
        <div class="row">
            <div class="col-sm-5">
                <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: red">* </span>Patient Status</span>
            </div>
            <div class="col-sm-7">
                <span style="text-align: left;">
                    <asp:DropDownList ID="ddlPatientStatus" EnableViewState="true" runat="server"
                        OnChange="patientStatusChanged()" AppendDataBoundItems="True" Style="width: 80px; height: 30px"  >
                        
                    </asp:DropDownList>
                    <asp:Label  ID="patientStatusError" runat="server" Text="" CssClass="error-message"></asp:Label>
                    <asp:HiddenField ID="hdnPatientStatusDisplay" runat="server" />

                </span>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-5">
                <span class="ohio-field" style="font-size: 15px; text-align: right">Admission  Date and Hour</span>
            </div>
            <div class="col-sm-7">
                <span style="text-align: left;">
                    <asp:TextBox ID="txtAdmissionDate" runat="server" AutoComplete="off" CssClass="formfieldDate" MaxLength="10" Style="height: 30px; width: 100px" OnChange="return admDateChange()" />
                    <asp:Image ID="imgAdmissiondate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" Visible="false" />
                    <ajax:CalendarExtender ID="CalendarExtender1" runat="server" Format="MM/dd/yyyy" TargetControlID="txtAdmissionDate" PopupPosition="BottomRight" CssClass="QstCalendarCSS" EnabledOnClient="true"  />
                    <asp:TextBox OnChange="validateFields()" ID="txtAdmissionHr" runat="server" CssClass="formfieldDate" MaxLength="4" Style="height: 30px; width: 80px !important"  onKeydown="return (!(event.keyCode>=65 && event.keyCode <=90) && event.keyCode!=32);"/>
                   <asp:RegularExpressionValidator ID="valAdmissionHr" runat="server" ControlToValidate="txtAdmissionHr"
                            ValidationExpression="^([0-1][0-9]|[2][0-3])([0-5][0-9])$" ErrorMessage="* Please enter hours Incorrect Admission Hour Format, must be HHMM" ForeColor="Red"
                            Enabled="true" SetFocusOnError="true" Text="*Please enter hours Incorrect Admission Hour Format, must be HHMM" 
                            ValidationGroup="validateClaims" Display="Dynamic" />
                   <%--  <asp:CompareValidator ID="cvAdmissionDate2" runat="server" Type="Date" Operator="GreaterThanEqual" ControlToValidate="txtFromDate" ControlToCompare="txtAdmissionDate" ForeColor="Red"  ValidationGroup="VldGrpFromDate" ErrorMessage="Admission date and time is invalid" Display="Dynamic" SetFocusOnError="true"   />--%>
                </span>
                  <button id="btnloading2" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
     
                <asp:Label ID="lblFromToDate" runat="server" CssClass="error-message" Text="" ></asp:Label>
                <asp:Label ID="lblServiceInfoError" runat="server" CssClass="error-message" Text="" ></asp:Label>
            </div>

        </div>
        <div class="row">
            <div class="col-sm-5">
                <span class="ohio-field" style="font-size: 15px; text-align: right">Discharge Hour</span>
            </div>
           <div class="col-sm-7">
                <span style="text-align: left;">
                    <asp:TextBox ID="txtDischargeHr" runat="server" CssClass="formfieldDate" MaxLength="4" Style="height: 30px; width: 200px" onKeydown="return (!(event.keyCode>=65 && event.keyCode <=90) && event.keyCode!=32);"
                        />
                    <%--OnTextChanged="txtDischargeHr_TextChanged" AutoPostBack="True" --%>
                    
                   <asp:RegularExpressionValidator ID="valDischargeHr" runat="server" ControlToValidate="txtDischargeHr" ForeColor="Red"
                            ValidationExpression="^([0-1][0-9]|[2][0-3])([0-5][0-9])$" ErrorMessage="* Please enter hours Incorrect Discharge Hour Format, must be HHMM"
                            Enabled="true" SetFocusOnError="true" Text="*Discharge Hour Format is Invalid, must be HHMM"
                            ValidationGroup="validateClaims" Display="Dynamic" />
                </span>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-5">
                <span class="ohio-field" style="font-size: 15px; text-align: right"><span style="color: red">* </span>Admission type</span>
            </div>
            <div class="col-sm-7">
                <span style="text-align: left;">
                    <asp:DropDownList ID="ddlAdmissionType" EnableViewState="true" runat="server"
                        AppendDataBoundItems="True"  Style="min-width: 200px; height: 30px" onchange="ddlAdmissionTypeSelectedIndexChanged()" >
                    </asp:DropDownList>                     
                     
                     <asp:Label ID="admissionTypeError" runat="server" Text="" CssClass="error-message"></asp:Label>
                       

                </span>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-5">
                <span class="ohio-field" style="font-size: 15px; text-align: right"><span  style="color:red">*</span>Admit Source</span>
            </div>
           <div class="col-sm-7">
                <span style="text-align: left;">
                    <asp:DropDownList ID="ddlAdmitSource" EnableViewState="true" runat="server" onchange="admitSrcStoreval()" Enabled="false"
                        AppendDataBoundItems="True"  Style="width: 80px; height: 30px">

                    </asp:DropDownList>
                    <asp:Label ID="admitsourceError" runat="server" Text="" CssClass="error-message"></asp:Label>
                    <asp:HiddenField id="hdnAdmitSrc" runat="server" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvAdmitSource" SetFocusOnError="true" ForeColor="Red"
                            ValidationGroup="validateClaims" ControlToValidate="ddlAdmitSource" ErrorMessage="*Admit Source" Text="*" Display="Dynamic" InitialValue="0" />
                </span>
            </div>

        </div>
    </div>
    <div class="col-sm-4">
        <div class="row">
            <div class="col-sm-5">
                <span class="ohio-field" style="font-size: 15px; text-align: right">Patient Paid Amount</span>
            </div>
            <div class="col-sm-7">
                <span style="text-align: left;">
                    <asp:TextBox ID="txtPatPaidAmt" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 200px" onkeypress="return onlyDotsAndNumbers(this,event);" MaxLength="18"  />

                </span>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtPatPaidAmt" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-5">
                <span class="ohio-field" style="font-size: 15px; text-align: right">Submitted DRG</span>
            </div>
            <div class="col-sm-7">
                <span style="text-align: left;">
                    <asp:TextBox ID="txtSubmittedDRG" runat="server" CssClass="formfieldDate" MaxLength="4" Style="height: 30px; width: 200px" />

                </span>
            </div>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtSubmittedDRG" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
        </div>
        <div class="row" id="divFinalDRG" runat="server">
            <div class="col-sm-5">
                <span class="ohio-field" style="font-size: 15px; text-align: right">Final DRG</span>
            </div>
            <div class="col-sm-7">
                <asp:Label ID="lblFinalDRG" runat="server"></asp:Label>              

            </div>
        </div>
    </div>

</div>
   

<ajax:ModalPopupExtender BehaviorID="mpeTypeOfBillCode" ID="mpeSubmitClaimSearchPop" runat="server" PopupControlID="pnlSubmitClaimSearchPop"
    TargetControlID="Button9" BackgroundCssClass="modalBackground" CancelControlID="btnCloseCH9" />

<asp:Panel ID="pnlSubmitClaimSearchPop" runat="server" CssClass="modalPopup" Style="display: none; min-height: 1px; min-width: 800px; height: auto; width: auto;">
    <asp:Panel ID="pnlSubmitClaimSearchPopHeader"  runat="server" HorizontalAlign="Left">
        <asp:Button runat="server" ID="btnCloseCH9"  Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
            CausesValidation="false" />
        <div class="search-Results">
            <span style="padding-left:50px">Type of Bill</span>
            <span style="padding-left:150px">Type of Bill Description</span>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlSubmitClaimSearch" runat="server">
        <div class="result-Container">
            <div class="popupGridViewOnSearch">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                    <div class="row">
                        <uc:TypeOfBill runat="server" ID="ucTypeOfBill" Visible="true" EnableViewState="true" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Panel>
<asp:Button runat="server" ID="Button9" Style="display: none" Text="ButtonDummy9" />


