<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="UserControls_OtherPayerInformation" Codebehind="OtherPayerInformation.ascx.cs" %>

<%@ Register Src="~/PopupControls/HeaderOtherPayerAdjustmentMappingProfessional.ascx"
    TagPrefix="uc" TagName="HeaderOtherPayerAdjustmentMappingProfessional" %>
<%@ Register Src="~/PopupControls/HeaderOtherPayerAdjustmentMapping.ascx" TagPrefix="uc"
    TagName="HeaderOtherPayerAdjustmentMapping" %>
<%@ Register Src="~/PopupControls/HeaderOtherPayerAdjustmentMappingInstitutional.ascx"
    TagPrefix="uc" TagName="HeaderOtherPayerAdjustmentMappingInstitutional" %>

<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<meta name="viewport" content="width=device-width, initial-scale=1">



<%--<style>
    .gridview {
        table-layout: auto;
        width:100%;
    }
</style>--%>

<script>
    function displayOtherPayerInfo() {
        var OtherPayerInfoclaimstatus = document.getElementById("<%=hdnOtherPayerInfoClaimStatus.ClientID %>").value;
        if (OtherPayerInfoclaimstatus == "Pending Submission") {
            var table = localStorage.getItem("otherPayerInfoTableDental");
        }
        else {
            var table = localStorage.removeItem("otherPayerInfoTableDental");
        }
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= divDentlOP.ClientID %>').innerHTML = table;
        }
    }

    function claimAdjLevelChange() {
        var ddlAdjudicationLevel = $("#<%=ddlClaimAdjudicationLevel.ClientID %> option:selected").val();
        if (ddlAdjudicationLevel != null && ddlAdjudicationLevel != undefined && ddlAdjudicationLevel != "") {
            $("[id*=ErrClaimAdjudication]").text('');
        }
        if (ddlAdjudicationLevel == "1") {
            $("#<%= txtPaidAmount1.ClientID %>").prop("disabled", false);
            $("#<%= txtNonCoveredAmount.ClientID %>").prop("disabled", false);
        } else if (ddlAdjudicationLevel == "2") {
            $("#<%= txtPaidAmount1.ClientID %>").prop("disabled", true);
            $("#<%= txtNonCoveredAmount.ClientID %>").prop("disabled", true);
        }
        return false;
    }
    function otherPayerDisablefieldsWithClear() {
        var destinationPayerResponsbilitySequence = $('#ctl00_MainContent_uc5SubmitClaim_ddlDestinationPayerResponsibilitySequence').val();
        localStorage.setItem("destinationPayerResponsbilitySequence", "" + destinationPayerResponsbilitySequence + "");
        var destinationPayerResponsibilitySeq = localStorage.getItem("destinationPayerResponsbilitySequence");
        var payerresponsibilitySeq = $("#<%=ddlPayerSequence.ClientID %> option:selected").val();

        if (payerresponsibilitySeq != "" && payerresponsibilitySeq != undefined && payerresponsibilitySeq != null) {
            $("[id*=errorPayerSeq]").text('');
            if (parseInt(destinationPayerResponsibilitySeq) > parseInt(payerresponsibilitySeq)) {
                Enablefields();
            } else { Disablefields(); }
        } else {
            Disablefields();
            if (payerresponsibilitySeq != 12) {
                $("#<%= txtPaidDate1.ClientID %>").prop("disabled", false);

            }
            else {
                $("#<%= txtPaidDate1.ClientID %>").prop("disabled", true);

            }
        }

        function Disablefields() {
            $("#<%= ddlClaimAdjudicationLevel.ClientID %>").prop("disabled", true);
            $("#<%= txtClaimNumber.ClientID %>").prop("disabled", true);
            $("#<%= txtPaidDate1.ClientID %>").prop("disabled", true);
            $("#<%= txtClaimNumber.ClientID %>").first().val("");
            $("#<%= txtPaidDate1.ClientID %>").first().val("");
            $("#<%= txtPaidAmount1.ClientID %>").first().val("");
            $("#<%= txtNonCoveredAmount.ClientID %>").first().val("");
            if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlClaimAdjudicationLevel") != null) {
                document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlClaimAdjudicationLevel").options[0].selected = true;
            }

        }
        function Enablefields() {
            $("#<%= ddlClaimAdjudicationLevel.ClientID %>").prop("disabled", false);
            $("#<%= txtClaimNumber.ClientID %>").prop("disabled", false);
            $("#<%= txtPaidDate1.ClientID %>").prop("disabled", false);
            $("#<%= txtClaimNumber.ClientID %>").first().val("");
            $("#<%= txtPaidDate1.ClientID %>").first().val("");
            $("#<%= txtPaidAmount1.ClientID %>").first().val("");
            $("#<%= txtNonCoveredAmount.ClientID %>").first().val("");
            if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlClaimAdjudicationLevel") != null) {
                document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlClaimAdjudicationLevel").options[0].selected = true;
            }
        }
        return false;
    }
    function otherPayerDisablefields() {
        var destinationPayerResponsbilitySequence = $('#ctl00_MainContent_uc5SubmitClaim_ddlDestinationPayerResponsibilitySequence').val();
        localStorage.setItem("destinationPayerResponsbilitySequence", "" + destinationPayerResponsbilitySequence + "");
        var destinationPayerResponsibilitySeq = localStorage.getItem("destinationPayerResponsbilitySequence");
        var payerresponsibilitySeq = $("#<%=ddlPayerSequence.ClientID %> option:selected").val();

        if (payerresponsibilitySeq != "" && payerresponsibilitySeq != undefined && payerresponsibilitySeq != null) {
            $("[id*=errorPayerSeq]").text('');
            if (parseInt(destinationPayerResponsibilitySeq) > parseInt(payerresponsibilitySeq)) {
                Enablefields();
            } else { Disablefields(); }
        } else { Disablefields(); }

        function Disablefields() {
            $("#<%= ddlClaimAdjudicationLevel.ClientID %>").prop("disabled", true);
            $("#<%= txtClaimNumber.ClientID %>").prop("disabled", true);
            $("#<%= txtPaidDate1.ClientID %>").prop("disabled", true);
            $("#<%= txtPaidAmount1.ClientID %>").prop("disabled", true);
            $("#<%= txtNonCoveredAmount.ClientID %>").prop("disabled", true);

        }
        function Enablefields() {
            $("#<%= ddlClaimAdjudicationLevel.ClientID %>").prop("disabled", false);
            $("#<%= txtClaimNumber.ClientID %>").prop("disabled", false);
            $("#<%= txtPaidDate1.ClientID %>").prop("disabled", false);
            $("#<%= txtPaidAmount1.ClientID %>").prop("disabled", false);
            $("#<%= txtNonCoveredAmount.ClientID %>").prop("disabled", false);
        }
        return false;
    }
    function validatehpId() {
        var healthPlanID = $("#<%= txtHealthPlanID.ClientID %>").first().val();
        if (healthPlanID != null && healthPlanID != undefined && healthPlanID != "") {
            $("[id*=healthPlanIdError]").text('');

            var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "GetDentalOtherPayerLineitem?claimid=" + claimid,
                //data: '{claimid: "' + claimid + '"  }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var claimOtherPayerInfoList = result;
                    if (claimOtherPayerInfoList.length > 0) {
                        for (var i = 0; i < claimOtherPayerInfoList.length; i++) {
                            var healthPlanID1 = result[i].healthPlanID;
                            if (healthPlanID1 == healthPlanID) {
                                $("[id*=healthPlanIdError]").text('*Duplicate Health Plan ID is Not Allowed');
                                $("#<%= txtHealthPlanID.ClientID %>").first().val("");
                                return false;
                            } else {
                                $("[id*=healthPlanIdError]").text('');
                            }
                        };
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {

                }
            });
        }

    }
    function addOtherPayerInfoCode() {
        var claimOtherPayerList;
        $("[id*=lblOthPayerNameError]").text('');
        $("[id*=healthPlanIdError]").text('');
        var validateField = "";
        var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
        var claimtype = document.getElementById("<%=hdnotherPayer_ClaimType.ClientID %>").value;
        var otherPayerName = $("#<%= txtOtherPayerInformation.ClientID %>").first().val();
        var healthPlanID = $("#<%= txtHealthPlanID.ClientID %>").first().val();
        var claimFilingIndicator = $("#<%=ddlClaimFilingIndicator.ClientID %> option:selected").val();
        var payerResponsibilitySequence = $("#<%=ddlPayerSequence.ClientID %> option:selected").val();
        var subscriberNumber = $("#<%= txtSubscriptionNumber.ClientID %>").first().val();
        var policyNumber = $("#<%= txtPolicyNumber.ClientID %>").first().val();
        var groupName = $("#<%= txtGroupName.ClientID %>").first().val();
        var insuranceTypeCode = $("#<%=ddlInsuranceTypeCode.ClientID %> option:selected").val();
        var patientRelationshipSuscriber = $("#<%=ddlPatientRelationship.ClientID %> option:selected").val();
        var subscriberFirstName = $("#<%= txtInsuredfirstName.ClientID %>").first().val();
        var subscriberLastName = $("#<%= txtInsuredLastName.ClientID %>").first().val();
        var subscriberMiddleName = $("#<%= txtOtherPayerMiddleName.ClientID %>").first().val();
        var subscriberAddressLine1 = $("#<%= txtSubcribersAddressLine1.ClientID %>").first().val();
        var subscriberAddressLine2 = $("#<%= txtSubscribersAddressLine2.ClientID %>").first().val();
        var subscriberCity = $("#<%= txtSubcribersCity.ClientID %>").first().val();
        var subscriberState = $("#<%=ddlSubcribersState.ClientID %> option:selected").val();
        var subscriberZip = $("#<%= txtSubscribersZip.ClientID %>").first().val();
        var claimAdjudicationLevel = $("#<%=ddlClaimAdjudicationLevel.ClientID %> option:selected").val();
        var claimNumber = $("#<%= txtClaimNumber.ClientID %>").first().val();
        var paidDate = $("#<%= txtPaidDate1.ClientID %>").first().val();
        var paidAmount = $("#<%= txtPaidAmount1.ClientID %>").first().val();
        var nonCoveredAmoount = $("#<%= txtNonCoveredAmount.ClientID %>").first().val();
        Errorlabelclear();
        if (hasValue()) {
            validateAddressFields();
        }
        else {
            validateField = "true";
        }
        validateFields();

        if (validateField === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "POST",
                url: webApiClaims + "AddOtherPayerDetails?claimid=" + claimid + "&&otherPayerName=" + otherPayerName + "&&healthPlanID=" + healthPlanID + "&&claimFilingIndicator=" + claimFilingIndicator + "&&payerResponsibilitySequence=" + payerResponsibilitySequence + "&&subscriberNumber=" + subscriberNumber + "&&policyNumber=" + policyNumber + "&&groupName=" + groupName + "&&insuranceTypeCode=" + insuranceTypeCode + "&&patientRelationshipSuscriber=" + patientRelationshipSuscriber + "&&subscriberFirstName=" + subscriberFirstName + "&&subscriberLastName=" + subscriberLastName + "&&subscriberMiddleName=" + subscriberMiddleName + "&&subscriberAddressLine1=" + subscriberAddressLine1 + "&&subscriberAddressLine2=" + subscriberAddressLine2 + "&&subscriberCity=" + subscriberCity + "&&subscriberState=" + subscriberState + "&&subscriberZip=" + subscriberZip + "&&claimAdjudicationLevel=" + claimAdjudicationLevel + "&&claimNumber=" + claimNumber + "&&paidDate=" + paidDate + "&&paidAmount=" + paidAmount + "&&nonCoveredAmoount=" + nonCoveredAmoount + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                //data: '{claimid: "' + claimid + '" ,otherPayerName: "' + otherPayerName + '" , healthPlanID: "' + healthPlanID + '" , claimFilingIndicator: "' + claimFilingIndicator + '" , payerResponsibilitySequence: "' + payerResponsibilitySequence + '" , subscriberNumber: "' + subscriberNumber + '" , policyNumber: "' + policyNumber + '" , groupName: "' + groupName + '" , insuranceTypeCode: "' + insuranceTypeCode + '" , patientRelationshipSuscriber: "' + patientRelationshipSuscriber + '" , subscriberFirstName: "' + subscriberFirstName + '" , subscriberLastName: "' + subscriberLastName + '" , subscriberMiddleName: "' + subscriberMiddleName + '" , subscriberAddressLine1: "' + subscriberAddressLine1 + '" , subscriberAddressLine2: "' + subscriberAddressLine2 + '" , subscriberCity: "' + subscriberCity + '" , subscriberState: "' + subscriberState + '" , subscriberZip: "' + subscriberZip + '" , claimAdjudicationLevel: "' + claimAdjudicationLevel + '" , claimNumber: "' + claimNumber + '" , paidDate: "' + paidDate + '" , paidAmount: "' + paidAmount + '" , nonCoveredAmoount: "' + nonCoveredAmoount + '"  }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var claimOtherPayerInfoList = result;
                    var table;
                    if (claimtype == "0") {
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerHealthPlanID").empty();
                    }
                    if (claimtype == "1") {
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSSubmitClaimHeaderOtherPayerAdjustmentMappingInstitutional_ddlOtherPayerHealthPlanID").empty();
                    }

                    if (claimtype == "2") {
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMappingProfessional_ddlOtherPayerHealthPlanID").empty();
                    }
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId").empty();
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentHealthPlanID").empty();
                    var newOption = "<option value=''></option>";
                    if (claimtype == "0") {
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerHealthPlanID").append(newOption);
                    }
                    if (claimtype == "1") {
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSSubmitClaimHeaderOtherPayerAdjustmentMappingInstitutional_ddlOtherPayerHealthPlanID").append(newOption);
                    }

                    if (claimtype == "2") {
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMappingProfessional_ddlOtherPayerHealthPlanID").append(newOption);
                    }

                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId").append(newOption);
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentHealthPlanID").append(newOption);
                    if (claimOtherPayerInfoList.length > 0) {
                        loadDropDownList();
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-bottom:50px;margin-left: 70px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:15px; scope='col'>*Other Payer Name</th><th style='width:15px; scope='col'>*Health Plan Id</th><th style='width:15px; scope='col'>Insured Last Name</th><th style='width:15px; scope='col'>Insured First Name</th><th style='width:15px; scope='col'>*Payer Sequence</th><th style='width:15px; scope='col'>*Adjudication Level</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                        for (var i = 0; i < claimOtherPayerInfoList.length; i++) {

                            var otherPayerName = result[i].otherPayerName;
                            var healthPlanID = result[i].healthPlanID;
                            var subscriberLastName = result[i].subscriberLastName;
                            var subscriberFirstName = result[i].subscriberFirstName;
                            var payerResponsibilitySequence = result[i].payerResponsibilitySequence;
                            removeOptions(payerResponsibilitySequence);
                            if (payerResponsibilitySequence == '1') payerResponsibilitySequence = "Primary";
                            if (payerResponsibilitySequence == '2') payerResponsibilitySequence = "Secondary";
                            if (payerResponsibilitySequence == '3') payerResponsibilitySequence = "Tertiary";
                            if (payerResponsibilitySequence == '4') payerResponsibilitySequence = "Payer Responsibility Four";
                            if (payerResponsibilitySequence == '5') payerResponsibilitySequence = "Payer Responsibility Five";
                            if (payerResponsibilitySequence == '6') payerResponsibilitySequence = "Payer Responsibility Six";
                            if (payerResponsibilitySequence == '7') payerResponsibilitySequence = "Payer Responsibility Seven";
                            if (payerResponsibilitySequence == '8') payerResponsibilitySequence = "Payer Responsibility Eight";
                            if (payerResponsibilitySequence == '9') payerResponsibilitySequence = "Payer Responsibility Nine";
                            if (payerResponsibilitySequence == '10') payerResponsibilitySequence = "Payer Responsibility Ten";
                            if (payerResponsibilitySequence == '11') payerResponsibilitySequence = "Payer Responsibility Eleven";
                            if (payerResponsibilitySequence == '12') payerResponsibilitySequence = "Unknown";
                            var claimAdjudicationLevel = result[i].claimAdjudicationLevel;
                            var otherpayerInfoID = result[i].otherpayerInfoID;

                            if (claimAdjudicationLevel == "1") {

                                var newOption = "<option value='" + otherpayerInfoID + "'>" + healthPlanID + "</option>";
                                if (claimtype == "0") {
                                    $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerHealthPlanID").append(newOption);
                                }
                                if (claimtype == "1") {
                                    $("#ctl00_MainContent_uc5SubmitClaim_ucSSubmitClaimHeaderOtherPayerAdjustmentMappingInstitutional_ddlOtherPayerHealthPlanID").append(newOption);
                                }

                                if (claimtype == "2") {
                                    $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMappingProfessional_ddlOtherPayerHealthPlanID").append(newOption);
                                }
                                claimAdjudicationLevel = "Header";
                            }
                            if (claimAdjudicationLevel == "2") {

                                var newOption = "<option value='" + otherpayerInfoID + "'>" + healthPlanID + "</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentHealthPlanID").append(newOption);
                                claimAdjudicationLevel = "Detail";
                            }

                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + otherPayerName + "</span></td><td ><span title='Line' class='tNumber'>" + healthPlanID + "</span></td><td><span  title='Line' class='tNumber'>" + subscriberLastName + "</span></td><td><span title='Line' class='tNumber'>" + subscriberFirstName + "</span></td><td>" + payerResponsibilitySequence + "</td><td>" + claimAdjudicationLevel + "</td><td><input type='button' value = 'Edit' onClick = 'return EditDentalOtherPayerLineItem(\"" + otherpayerInfoID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteDentalOtherPayerLineitem(\"" + otherpayerInfoID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                          
                        };

                        table = table + "</tbody></table>";
                        localStorage.setItem("otherPayerInfoTableDental", "" + table + "");
                        document.getElementById('<%= divDentlOP.ClientID %>').innerHTML = table;
                            ClearotherPayerInfofields();
                            if (claimFilingIndicator != null && claimFilingIndicator != undefined && claimFilingIndicator != " " && (claimFilingIndicator === "MA" || claimFilingIndicator === "MB")) {

                                removeOptionsForIndicator(claimFilingIndicator);
                            }
                        }

                    },
                    error: function (jqXHR, textStatus, errorThrown) {

                    }
                });
        }

        function hasValue() {
            var rtn = false;
            if (!(subscriberAddressLine1 === null || subscriberAddressLine1 === "") || 
                !(subscriberCity === null || subscriberCity === "") ||
                !(subscriberState === null || subscriberState === "") ||
                !(subscriberZip === null || subscriberZip === "")) {
                rtn = true;
            }
            return rtn;
        }

        function Errorlabelclear() {
            $("[id*=lblAddress1error]").text('');
            $("[id*=lblCityError]").text('');
            $("[id*=lblStateError]").text('');
            $("[id*=lblZipError]").text('');

        }

        function validateAddressFields() {
            
            if (subscriberAddressLine1 === null || subscriberAddressLine1 === "") {
                validateField = "false";
                $("[id*=lblAddress1error]").text('*Address Line 1 is required');
                return false;
            }
            if (subscriberCity === null || subscriberCity === "") {
                validateField = "false";
                $("[id*=lblCityError]").text('*City is required');
                return false;
            }
            if (subscriberState === null || subscriberState === "") {
                validateField = "false";
                $("[id*=lblStateError]").text('*State is required');
                return false;
            }
            if (subscriberZip === null || subscriberZip === "") {
                validateField = "false";
                $("[id*=lblZipError]").text('*Zip  is required');
                return false;
            }
            
        }

        function validateFields() {

                if ((otherPayerName === null || otherPayerName === "" || otherPayerName === undefined)) {

                validateField = "false";
                $("[id*=lblOthPayerNameError]").text('*Other Payer Name is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=lblOthPayerNameError]").text('');
            }

            if ((healthPlanID === null || healthPlanID === "" || healthPlanID === undefined)) {

                validateField = "false";
                $("[id*=healthPlanIdError]").text('*Health Plan ID is Required');
                return false;
            }

            else {
                validateField = "true";
                $("[id*=healthPlanIdError]").text('');
            }



            if ((claimFilingIndicator === null || claimFilingIndicator === "" || claimFilingIndicator === undefined)) {

                validateField = "false";
                $("[id*=errorClaimFilingIndicator]").text('*Claim Filing Indicator missing');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=errorClaimFilingIndicator]").text('');
            }


            if ((payerResponsibilitySequence === null || payerResponsibilitySequence === "" || payerResponsibilitySequence === undefined)) {

                validateField = "false";
                $("[id*=errorPayerSeq]").text('*Payer sequence is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=errorPayerSeq]").text('');
            }

            if ((subscriberNumber === null || subscriberNumber === "" || subscriberNumber === undefined)) {

                validateField = "false";
                $("[id*=errSubScNum]").text('*Subscription Number is Required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=errSubScNum]").text('');
            }


            if ((patientRelationshipSuscriber === null || patientRelationshipSuscriber === "" || patientRelationshipSuscriber === undefined)) {

                validateField = "false";
                $("[id*=errPatientRelation]").text('*Patient relationship is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=errPatientRelation]").text('');
            }

            if ((subscriberFirstName === null || subscriberFirstName === "" || subscriberFirstName === undefined)) {

                validateField = "false";
                $("[id*=errFirstName]").text('*Insured’s First Name is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=errFirstName]").text('');
            }

            if ((subscriberLastName === null || subscriberLastName === "" || subscriberLastName === undefined)) {

                validateField = "false";
                $("[id*=errLastName]").text('*Insured’s Last Name is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=errLastName]").text('');
            }

            if ((policyNumber == null || policyNumber == "" || policyNumber == undefined) && (groupName == null || groupName == "" || groupName == undefined)) {

                validateField = "false";
                $("[id*=lblPolicynoandgroupnameErrormsg]").text('*Either group name or policy number is required');
                return false;
            }

            else {
                validateField = "true";
                $("[id*=lblPolicynoandgroupnameErrormsg]").text('');
            }
            if ((policyNumber != null && policyNumber != "" && policyNumber != undefined) && (groupName != null && groupName != "" && groupName != undefined)) {

                validateField = "false";
                $("[id*=lblPolicynoandgroupnameErrormsg]").text('*Group name and policy number both cannot be entered');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=lblPolicynoandgroupnameErrormsg]").text('');
            }

            var destinationPayerResponsbilitySequence = $('#ctl00_MainContent_uc5SubmitClaim_ddlDestinationPayerResponsibilitySequence').val();
            localStorage.setItem("destinationPayerResponsbilitySequence", "" + destinationPayerResponsbilitySequence + "");
            var destinationPayerResponsibilitySeq = localStorage.getItem("destinationPayerResponsbilitySequence");
            var payerresponsibilitySeq = $("#<%=ddlPayerSequence.ClientID %> option:selected").val();
            if (parseInt(destinationPayerResponsibilitySeq) > parseInt(payerresponsibilitySeq)) {

                if ((claimAdjudicationLevel === null || claimAdjudicationLevel === "" || claimAdjudicationLevel === undefined)) {

                    validateField = "false";
                    $("[id*=ErrClaimAdjudication]").text('*Claim Adjudication Level is requiered');
                    return false;
                }
                else {
                    validateField = "true";
                    $("[id*=ErrClaimAdjudication]").text('');
                }
                if ((claimNumber === null || claimNumber === "" || claimNumber === undefined)) {

                    validateField = "false";
                    $("[id*=errClaimNo]").text('*Claim Number is Required');
                    return false;
                }
                else {
                    validateField = "true";
                    $("[id*=errClaimNo]").text('');
                }
                if ((paidDate === null || paidDate === "" || paidDate === undefined)) {

                    validateField = "false";
                    $("[id*=errPaidDate]").text('*Paid Date is required');
                    return false;
                }
                else {
                    validateField = "true";
                    $("[id*=errPaidDate]").text('');
                }
                if (claimAdjudicationLevel == "1") {
                    if ((paidAmount === null || paidAmount === "" || paidAmount === undefined)) {

                        validateField = "false";
                        $("[id*=errPaidAmount]").text('*Paid Amount is required');
                        return false;
                    }
                    else {
                        validateField = "true";
                        $("[id*=errPaidAmount]").text('');
                    }
                }
            }
        }

        return false;
    }

    function loadDropDownList() {
        $("#<%=ddlPayerSequence.ClientID %>").empty();
        var newOption = "<option value=''></option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
        var newOption = "<option value='1'>Primary</option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
        var newOption = "<option value='2'>Secondary</option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
        var newOption = "<option value='3'>Tertiary</option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
        var newOption = "<option value='4'>Payer Responsibility Four</option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
        var newOption = "<option value='5'>Payer Responsibility Five</option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
        var newOption = "<option value='6'>Payer Responsibility Six</option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
        var newOption = "<option value='7'>Payer Responsibility Seven</option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
        var newOption = "<option value='8'>Payer Responsibility Eight</option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
        var newOption = "<option value='9'>Payer Responsibility Nine</option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
        var newOption = "<option value='10'>Payer Responsibility Ten</option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
        var newOption = "<option value='11'>Payer Responsibility Eleven</option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
        var newOption = "<option value='12'>Unknown</option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
        var destinationPayerResponsbilitySequence = $('#ctl00_MainContent_uc5SubmitClaim_ddlDestinationPayerResponsibilitySequence').val();
        localStorage.setItem("destinationPayerResponsbilitySequence", "" + destinationPayerResponsbilitySequence + "");
        var destinationPayerResponsibilitySeq = localStorage.getItem("destinationPayerResponsbilitySequence");
        $("#<%=ddlPayerSequence.ClientID %> option[value='" + destinationPayerResponsibilitySeq + "']").remove();
    }
    function loadddlClaimFilingIndicator() {
        var Claim_ID = $("#<%= hdnClaimId.ClientID %>").first().val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetFilteredClaimIndicator?Claim_ID=" + Claim_ID+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var ddResult = result;
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlClaimFilingIndicator").empty();
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlClaimFilingIndicator").append($('<option value=""></option>'));
                for (var i = 0; i < ddResult.length; i++) {
                    var newOption = "<option value='" + ddResult[i].ID + "'>" + ddResult[i].Desc + "</option>";
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlClaimFilingIndicator").append(newOption);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }

    function removeOptions(payerResponsibilitySequence) {

        $("#<%=ddlPayerSequence.ClientID %> option[value='" + payerResponsibilitySequence + "']").remove();

    }
    function removeOptionsForIndicator(claimFilingIndicator) {

        $("#<%=ddlClaimFilingIndicator.ClientID %> option[value='" + claimFilingIndicator + "']").remove();
    }
    function updateOptionsForIndicator(claimFilingIndicator) {

        $("#<%=ddlClaimFilingIndicator.ClientID %> option[value='" + claimFilingIndicator + "']").remove();
        if (claimFilingIndicator === 'MA') {
            var newOption = "<option value='MA'>MA - Medicare Part A</option>";
            $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlClaimFilingIndicator").append(newOption);
        }
        if (claimFilingIndicator === 'MB') {
            var newOption = "<option value='MB'>MB - Medicare Part B</option>";
            $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlClaimFilingIndicator").append(newOption);
        }
    }

    function EditDentalOtherPayerLineItem(otherpayerInfoID,control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        $("[id*=hdnOtherPayerInfoId]").val("" + otherpayerInfoID + "");
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetIndDentalOtherPayerDetail?otherpayerInfoID=" + otherpayerInfoID,
            //data: '{otherpayerInfoID: "' + otherpayerInfoID + '"}',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var claimOtherPayerList = result;
                if (claimOtherPayerList.length > 0) {

                    for (var i = 0; i < claimOtherPayerList.length; i++) {

                        var otherpayerInfoID = result[i].otherpayerInfoID;
                        var otherPayerName = result[i].otherPayerName;
                        var healthPlanID = result[i].healthPlanID;
                        var claimFilingIndicator = result[i].claimFilingIndicator;
                        var payerResponsibilitySequence = result[i].payerResponsibilitySequence;
                        debugger;
                        if ($('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence option:contains("' + payerResponsibilitySequence + '")')) {
                            removeOptions(payerResponsibilitySequence);
                        }
                        
                            if (payerResponsibilitySequence === '1') {
                                var newOption = "<option value='1'>Primary</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
                            }
                            if (payerResponsibilitySequence === '2') {
                                var newOption = "<option value='2'>Secondary</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
                            }
                            if (payerResponsibilitySequence === '3') {
                                var newOption = "<option value='3'>Tertiary</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
                            }
                            if (payerResponsibilitySequence === '4') {
                                var newOption = "<option value='4'>Payer Responsibility Four</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
                            }
                            if (payerResponsibilitySequence === '5') {
                                var newOption = "<option value='5'>Payer Responsibility Five</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
                            }
                            if (payerResponsibilitySequence === '6') {
                                var newOption = "<option value='6'>Payer Responsibility Six</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
                            }
                            if (payerResponsibilitySequence === '7') {
                                var newOption = "<option value='7'>Payer Responsibility Seven</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
                            }
                            if (payerResponsibilitySequence === '8') {
                                var newOption = "<option value='8'>Payer Responsibility Eight</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
                            }
                            if (payerResponsibilitySequence === '9') {
                                var newOption = "<option value='9'>Payer Responsibility Nine</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
                            }
                            if (payerResponsibilitySequence === '10') {
                                var newOption = "<option value='10'>Payer Responsibility Ten</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
                            }
                            if (payerResponsibilitySequence === '11') {
                                var newOption = "<option value='11'>Payer Responsibility Eleven</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
                            }
                            if (payerResponsibilitySequence === '12') {
                                var newOption = "<option value='12'>Unknown</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").append(newOption);
                            }
                        
                        if (claimFilingIndicator != null && claimFilingIndicator != undefined && claimFilingIndicator != " " && (claimFilingIndicator === "MA" || claimFilingIndicator === "MB")) {

                            updateOptionsForIndicator(claimFilingIndicator);
                        }
                        var subscriberNumber = result[i].subscriberNumber;
                        var policyNumber = result[i].policyNumber;
                        var groupName = result[i].groupName;
                        var insuranceTypeCode = result[i].insuranceTypeCode;
                        var patientRelationshipSuscriber = result[i].patientRelationshipSuscriber;
                        var subscriberFirstName = result[i].subscriberFirstName;
                        var subscriberLastName = result[i].subscriberLastName;
                        var subscriberMiddleName = result[i].subscriberMiddleName;
                        var subscriberAddressLine1 = result[i].subscriberAddressLine1;
                        var subscriberAddressLine2 = result[i].subscriberAddressLine2;
                        var subscriberCity = result[i].subscriberCity;
                        var subscriberState = result[i].subscriberState;
                        var subscriberZip = result[i].subscriberZip;
                        var claimAdjudicationLevel = result[i].claimAdjudicationLevel;
                        var claimNumber = result[i].claimNumber;
                        var value = new Date
                            (
                                result[i].paidDate
                            );
                        var month = parseInt(parseInt(value.getMonth()) + parseInt(1));
                        if (parseInt(month) < 10) { month = '0' + parseInt(month); }
                        var day = value.getDate();
                        if (parseInt(day) < 10) { day = '0' + day; }
                        var year = value.getFullYear();
                        var paidDateOrginal = month + "/" + day + "/" + year;
                        var paidAmount = result[i].paidAmount;
                        var nonCoveredAmoount = result[i].nonCoveredAmoount;

                        $("#<%= txtOtherPayerInformation.ClientID %>").first().val(otherPayerName);
                        $("#<%= txtHealthPlanID.ClientID %>").first().val(healthPlanID);
                        $('#<%=ddlClaimFilingIndicator.ClientID%>').val(claimFilingIndicator);
                        $('#<%=ddlPayerSequence.ClientID%>').val(payerResponsibilitySequence);
                        $("#<%= txtSubscriptionNumber.ClientID %>").first().val(subscriberNumber);
                        $("#<%= txtPolicyNumber.ClientID %>").first().val(policyNumber);
                        $("#<%= txtGroupName.ClientID %>").first().val(groupName);
                        $('#<%=ddlInsuranceTypeCode.ClientID%>').val(insuranceTypeCode);
                        $('#<%=ddlPatientRelationship.ClientID%>').val(patientRelationshipSuscriber);
                        $("#<%= txtInsuredfirstName.ClientID %>").first().val(subscriberFirstName);
                        $("#<%= txtInsuredLastName.ClientID %>").first().val(subscriberLastName);
                        $("#<%= txtOtherPayerMiddleName.ClientID %>").first().val(subscriberMiddleName);
                        $("#<%= txtSubcribersAddressLine1.ClientID %>").first().val(subscriberAddressLine1);
                        $("#<%= txtSubscribersAddressLine2.ClientID %>").first().val(subscriberAddressLine2);
                        $("#<%= txtSubcribersCity.ClientID %>").first().val(subscriberCity);
                        $('#<%=ddlSubcribersState.ClientID%>').val(subscriberState);
                        $("#<%= txtSubscribersZip.ClientID %>").first().val(subscriberZip);
                        $('#<%=ddlClaimAdjudicationLevel.ClientID%>').val(claimAdjudicationLevel);
                        $("#<%= txtClaimNumber.ClientID %>").first().val(claimNumber);
                        if (paidDateOrginal != 'NaN/NaN/NaN') {
                            $("#<%= txtPaidDate1.ClientID %>").first().val(paidDateOrginal);
                        }
                        $("#<%= txtPaidAmount1.ClientID %>").first().val(paidAmount);
                        $("#<%= txtNonCoveredAmount.ClientID %>").first().val(nonCoveredAmoount);

                        document.getElementById('<%= btnOtherPayerAdd.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnOtherPayerUpdate.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnOtherPayerCancel.ClientID%>').style.visibility = "visible";
                        otherPayerDisablefields();
                        claimAdjLevelChange();
                        return false;
                    };
                }


            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });


    }

    function updateOtherPayerInfoCode() {
        var claimOtherPayerList;
        $("[id*=ErrClaimAdjudication]").text('');
        var validateField = "";
        var otherpayerInfoId = document.getElementById("<%=hdnOtherPayerInfoId.ClientID %>").value;
        var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
        var claimtype = document.getElementById("<%=hdnotherPayer_ClaimType.ClientID %>").value;
        var otherPayerName = $("#<%= txtOtherPayerInformation.ClientID %>").first().val();
        var healthPlanID = $("#<%= txtHealthPlanID.ClientID %>").first().val();
        var claimFilingIndicator = $("#<%=ddlClaimFilingIndicator.ClientID %> option:selected").val();
        var payerResponsibilitySequence = $("#<%=ddlPayerSequence.ClientID %> option:selected").val();
        var subscriberNumber = $("#<%= txtSubscriptionNumber.ClientID %>").first().val();
        var policyNumber = $("#<%= txtPolicyNumber.ClientID %>").first().val();
        var groupName = $("#<%= txtGroupName.ClientID %>").first().val();
        var insuranceTypeCode = $("#<%=ddlInsuranceTypeCode.ClientID %> option:selected").val();
        var patientRelationshipSuscriber = $("#<%=ddlPatientRelationship.ClientID %> option:selected").val();
        var subscriberFirstName = $("#<%= txtInsuredfirstName.ClientID %>").first().val();
        var subscriberLastName = $("#<%= txtInsuredLastName.ClientID %>").first().val();
        var subscriberMiddleName = $("#<%= txtOtherPayerMiddleName.ClientID %>").first().val();
        var subscriberAddressLine1 = $("#<%= txtSubcribersAddressLine1.ClientID %>").first().val();
        var subscriberAddressLine2 = $("#<%= txtSubscribersAddressLine2.ClientID %>").first().val();
        var subscriberCity = $("#<%= txtSubcribersCity.ClientID %>").first().val();
        var subscriberState = $("#<%=ddlSubcribersState.ClientID %> option:selected").val();
        var subscriberZip = $("#<%= txtSubscribersZip.ClientID %>").first().val();
        var claimAdjudicationLevel = $("#<%=ddlClaimAdjudicationLevel.ClientID %> option:selected").val();
        var claimNumber = $("#<%= txtClaimNumber.ClientID %>").first().val();
        var paidDate = $("#<%= txtPaidDate1.ClientID %>").first().val();
        var paidAmount = $("#<%= txtPaidAmount1.ClientID %>").first().val();
        var nonCoveredAmoount = $("#<%= txtNonCoveredAmount.ClientID %>").first().val();
        validateFields();
        if (validateField === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "PUT",
                url: webApiClaims + "UpdateOtherPayerDetails?otherpayerInfoId=" + otherpayerInfoId + "&&claimid=" + claimid + "&&otherPayerName=" + otherPayerName + "&&healthPlanID=" + healthPlanID + "&&claimFilingIndicator=" + claimFilingIndicator + "&&payerResponsibilitySequence=" + payerResponsibilitySequence + "&&subscriberNumber=" + subscriberNumber + "&&policyNumber=" + policyNumber + "&&groupName=" + groupName + "&&insuranceTypeCode=" + insuranceTypeCode + "&&patientRelationshipSuscriber=" + patientRelationshipSuscriber + "&&subscriberFirstName=" + subscriberFirstName + "&&subscriberLastName=" + subscriberLastName + "&&subscriberMiddleName=" + subscriberMiddleName + "&&subscriberAddressLine1=" + subscriberAddressLine1 + "&&subscriberAddressLine2=" + subscriberAddressLine2 + "&&subscriberCity=" + subscriberCity + "&&subscriberState=" + subscriberState + "&&subscriberZip=" + subscriberZip + "&&claimAdjudicationLevel=" + claimAdjudicationLevel + "&&claimNumber=" + claimNumber + "&&paidDate=" + paidDate + "&&paidAmount=" + paidAmount + "&&nonCoveredAmoount=" + nonCoveredAmoount + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var claimOtherPayerInfoList = result;
                    var table;
                    var newOption = "<option value=''></option>";
                    if (claimtype == "0") {
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerHealthPlanID").empty();
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerHealthPlanID").append(newOption);
                    }
                    if (claimtype == "1") {
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSSubmitClaimHeaderOtherPayerAdjustmentMappingInstitutional_ddlOtherPayerHealthPlanID").empty();
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSSubmitClaimHeaderOtherPayerAdjustmentMappingInstitutional_ddlOtherPayerHealthPlanID").append(newOption);
                    }
                    if (claimtype == "2") {
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMappingProfessional_ddlOtherPayerHealthPlanID").empty();
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMappingProfessional_ddlOtherPayerHealthPlanID").append(newOption);
                    }
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId").empty();
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId").append(newOption);

                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentHealthPlanID").empty();
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentHealthPlanID").append(newOption);

                    if (claimOtherPayerInfoList.length > 0) {
                        loadDropDownList();
                        loadddlClaimFilingIndicator();

                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-bottom:50px;margin-left: 70px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:15px; scope='col'>*Other Payer Name</th><th style='width:15px; scope='col'>*Health Plan Id</th><th style='width:15px; scope='col'>Insured Last Name</th><th style='width:10px; scope='col'>Insured First Name</th><th style='width:15px; scope='col'>*Payer Sequence</th><th style='width:15px; scope='col'>*Adjudication Level</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                        for (var i = 0; i < claimOtherPayerInfoList.length; i++) {

                            var otherPayerName = result[i].otherPayerName;
                            var healthPlanID = result[i].healthPlanID;
                            var subscriberLastName = result[i].subscriberLastName;
                            var subscriberFirstName = result[i].subscriberFirstName;
                            var payerResponsibilitySequence = result[i].payerResponsibilitySequence;
                            removeOptions(payerResponsibilitySequence);
                            if (payerResponsibilitySequence == '1') payerResponsibilitySequence = "Primary";
                            if (payerResponsibilitySequence == '2') payerResponsibilitySequence = "Secondary";
                            if (payerResponsibilitySequence == '3') payerResponsibilitySequence = "Tertiary";
                            if (payerResponsibilitySequence == '4') payerResponsibilitySequence = "Payer Responsibility Four";
                            if (payerResponsibilitySequence == '5') payerResponsibilitySequence = "Payer Responsibility Five";
                            if (payerResponsibilitySequence == '6') payerResponsibilitySequence = "Payer Responsibility Six";
                            if (payerResponsibilitySequence == '7') payerResponsibilitySequence = "Payer Responsibility Seven";
                            if (payerResponsibilitySequence == '8') payerResponsibilitySequence = "Payer Responsibility Eight";
                            if (payerResponsibilitySequence == '9') payerResponsibilitySequence = "Payer Responsibility Nine";
                            if (payerResponsibilitySequence == '10') payerResponsibilitySequence = "Payer Responsibility Ten";
                            if (payerResponsibilitySequence == '11') payerResponsibilitySequence = "Payer Responsibility Eleven";
                            if (payerResponsibilitySequence == '12') payerResponsibilitySequence = "Unknown";
                            var claimAdjudicationLevel = result[i].claimAdjudicationLevel;
                            var otherpayerInfoID = result[i].otherpayerInfoID;

                            if (claimAdjudicationLevel == "1") { 
                                var newOption = "<option value='" + otherpayerInfoID + "'>" + healthPlanID + "</option>";
                                if (claimtype == "0") {
                                    $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerHealthPlanID").append(newOption);
                                }
                                if (claimtype == "1") {
                                    $("#ctl00_MainContent_uc5SubmitClaim_ucSSubmitClaimHeaderOtherPayerAdjustmentMappingInstitutional_ddlOtherPayerHealthPlanID").append(newOption);
                                }
                                if (claimtype == "2") {
                                    var ddlOtherPayerHealthPlanID = document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMappingProfessional_ddlOtherPayerHealthPlanID");
                                    var option = document.createElement("option");
                                    option.text = healthPlanID;
                                    option.value = otherpayerInfoID;
                                    ddlOtherPayerHealthPlanID.add(option, ddlOtherPayerHealthPlanID[i+1]);

                                    //$("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMappingProfessional_ddlOtherPayerHealthPlanID").append(newOption);
                                }
                                claimAdjudicationLevel = "Header";
                            }
                            if (claimAdjudicationLevel == "2") {
                                var newOption = "<option value='" + otherpayerInfoID + "'>" + healthPlanID + "</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentHealthPlanID").append(newOption);
                                claimAdjudicationLevel = "Detail";
                            }
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + otherPayerName + "</span></td><td><span title='Line' class='tNumber'>" + healthPlanID + "</span></td><td><span  title='Line' class='tNumber'>" + subscriberLastName + "</span></td><td><span title='Line' class='tNumber'>" + subscriberFirstName + "</span></td><td>" + payerResponsibilitySequence + "</td><td>" + claimAdjudicationLevel + "</td><td><input type='button' value = 'Edit' onClick = 'return EditDentalOtherPayerLineItem(\"" + otherpayerInfoID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteDentalOtherPayerLineitem(\"" + otherpayerInfoID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                        };

                        table = table + "</tbody></table>";
                        localStorage.setItem("otherPayerInfoTableDental", "" + table + "");
                        document.getElementById('<%= divDentlOP.ClientID %>').innerHTML = table;
                    document.getElementById('<%= btnOtherPayerAdd.ClientID%>').style.visibility = "visible";
                    document.getElementById('<%= btnOtherPayerUpdate.ClientID%>').style.visibility = "hidden";
                    document.getElementById('<%= btnOtherPayerCancel.ClientID%>').style.visibility = "hidden";
                    ClearotherPayerInfofields();
                    if (claimtype == "0") { otherPayerHeaderDetailBind(); }
                    if (claimtype == "1") { otherPayerInstHeaderDetailBind(); }
                    if (claimtype == "2") { otherPayerProfHeaderDetailBind(); }
                    reloadChildPanel();

                }

            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });
        }


        function validateFields() {
            if ((otherPayerName === null || otherPayerName === "" || otherPayerName === undefined)) {
                validateField = "false";
                $("[id*=lblOthPayerNameError]").text('*Other Payer Name is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=lblOthPayerNameError]").text('');
            }

            if ((healthPlanID === null || healthPlanID === "" || healthPlanID === undefined)) {

                validateField = "false";
                $("[id*=healthPlanIdError]").text('*Health Plan ID is Required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=healthPlanIdError]").text('');
            }
            if ((policyNumber == null || policyNumber == "" || policyNumber == undefined) && (groupName == null || groupName == "" || groupName == undefined)) {

                validateField = "false";
                $("[id*=lblPolicynoandgroupnameErrormsg]").text('*Either group name or policy number is required');
                return false;
            }

            else {
                validateField = "true";
                $("[id*=lblPolicynoandgroupnameErrormsg]").text('');
            }

            if ((claimFilingIndicator === null || claimFilingIndicator === "" || claimFilingIndicator === undefined)) {

                validateField = "false";
                $("[id*=errorClaimFilingIndicator]").text('*Claim Filing Indicator missing');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=errorClaimFilingIndicator]").text('');
            }


            if ((payerResponsibilitySequence === null || payerResponsibilitySequence === "" || payerResponsibilitySequence === undefined)) {

                validateField = "false";
                $("[id*=errorPayerSeq]").text('*Payer sequence is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=errorPayerSeq]").text('');
            }

            if ((subscriberNumber === null || subscriberNumber === "" || subscriberNumber === undefined)) {

                validateField = "false";
                $("[id*=errSubScNum]").text('*Subscription Number is Required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=errSubScNum]").text('');
            }


            if ((patientRelationshipSuscriber === null || patientRelationshipSuscriber === "" || patientRelationshipSuscriber === undefined)) {

                validateField = "false";
                $("[id*=errPatientRelation]").text('*Patient relationship is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=errPatientRelation]").text('');
            }

            if ((subscriberFirstName === null || subscriberFirstName === "" || subscriberFirstName === undefined)) {

                validateField = "false";
                $("[id*=errFirstName]").text('*Insured’s First Name is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=errFirstName]").text('');
            }

            if ((subscriberLastName === null || subscriberLastName === "" || subscriberLastName === undefined)) {

                validateField = "false";
                $("[id*=errLastName]").text('*Insured’s Last Name is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=errLastName]").text('');
            }
            var destinationPayerResponsbilitySequence = $('#ctl00_MainContent_uc5SubmitClaim_ddlDestinationPayerResponsibilitySequence').val();
            localStorage.setItem("destinationPayerResponsbilitySequence", "" + destinationPayerResponsbilitySequence + "");
            var destinationPayerResponsibilitySeq = localStorage.getItem("destinationPayerResponsbilitySequence");
            var payerresponsibilitySeq = $("#<%=ddlPayerSequence.ClientID %> option:selected").val();
            if (parseInt(destinationPayerResponsibilitySeq) > parseInt(payerresponsibilitySeq)) {

                if ((claimAdjudicationLevel === null || claimAdjudicationLevel === "" || claimAdjudicationLevel === undefined)) {

                    validateField = "false";
                    $("[id*=ErrClaimAdjudication]").text('*Claim Adjudication Level is requiered');
                    return false;
                }
                else {
                    validateField = "true";
                    $("[id*=ErrClaimAdjudication]").text('');
                }
                if ((claimNumber === null || claimNumber === "" || claimNumber === undefined)) {

                    validateField = "false";
                    $("[id*=errClaimNo]").text('*Claim Number is Required');
                    return false;
                }
                else {
                    validateField = "true";
                    $("[id*=errClaimNo]").text('');
                }
                if ((paidDate === null || paidDate === "" || paidDate === undefined)) {

                    validateField = "false";
                    $("[id*=errPaidDate]").text('*Paid Date is required');
                    return false;
                }
                else {
                    validateField = "true";
                    $("[id*=errPaidDate]").text('');
                }
                if (claimAdjudicationLevel == "1") {

                    if ((paidAmount === null || paidAmount === "" || paidAmount === undefined)) {

                        validateField = "false";
                        $("[id*=errPaidAmount]").text('*Paid Amount is required');
                        return false;
                    }
                    else {
                        validateField = "true";
                        $("[id*=errPaidAmount]").text('');
                    }

                }
            }

        }

        return false;
    }

    function DeleteDentalOtherPayerLineitem(otherpayerInfoID) {
        var result = confirm("Are you sure you want to delete?");
        if (result) {
            var claimtype = document.getElementById("<%=hdnotherPayer_ClaimType.ClientID %>").value;
            var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
            if (claimtype == "0") {
                $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerHealthPlanID").empty();
            }
            if (claimtype == "1") {
                $("#ctl00_MainContent_uc5SubmitClaim_ucSSubmitClaimHeaderOtherPayerAdjustmentMappingInstitutional_ddlOtherPayerHealthPlanID").empty();
            }

            if (claimtype == "2") {
                $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMappingProfessional_ddlOtherPayerHealthPlanID").empty();
            }
            $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId").empty();
            $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentHealthPlanID").empty();
            var newOption = "<option value=''></option>";
            if (claimtype == "0") {
                $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerHealthPlanID").append(newOption);
            }
            if (claimtype == "1") {
                $("#ctl00_MainContent_uc5SubmitClaim_ucSSubmitClaimHeaderOtherPayerAdjustmentMappingInstitutional_ddlOtherPayerHealthPlanID").append(newOption);
            }

            if (claimtype == "2") {
                $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMappingProfessional_ddlOtherPayerHealthPlanID").append(newOption);
            }

            $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId").append(newOption);
            $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentHealthPlanID").append(newOption);
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteDentalOtherPayerLineitem?otherpayerInfoID=" + otherpayerInfoID + "&&claimid=" + claimid,
                //data: '{otherpayerInfoID: "' + otherpayerInfoID + '", claimid: "' + claimid + '"  }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var claimOtherPayerInfoList = result;
                    var table;

                    if (claimOtherPayerInfoList.length > 0) {
                        loadDropDownList();
                        loadddlClaimFilingIndicator();
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 70px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:15px; scope='col'>*Other Payer Name</th><th style='width:15px; scope='col'>*Health Plan Id</th><th style='width:15px; scope='col'>Insured Last Name</th><th style='width:10px; scope='col'>Insured First Name</th><th style='width:15px; scope='col'>*Payer Sequence</th><th style='width:15px; scope='col'>*Adjudication Level</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                        for (var i = 0; i < claimOtherPayerInfoList.length; i++) {

                            var otherPayerName = result[i].otherPayerName;
                            var healthPlanID = result[i].healthPlanID;
                            var subscriberLastName = result[i].subscriberLastName;
                            var subscriberFirstName = result[i].subscriberFirstName;
                            var payerResponsibilitySequence = result[i].payerResponsibilitySequence;
                            removeOptions(payerResponsibilitySequence);
                            if (payerResponsibilitySequence == '1') payerResponsibilitySequence = "Primary";
                            if (payerResponsibilitySequence == '2') payerResponsibilitySequence = "Secondary";
                            if (payerResponsibilitySequence == '3') payerResponsibilitySequence = "Tertiary";
                            if (payerResponsibilitySequence == '4') payerResponsibilitySequence = "Payer Responsibility Four";
                            if (payerResponsibilitySequence == '5') payerResponsibilitySequence = "Payer Responsibility Five";
                            if (payerResponsibilitySequence == '6') payerResponsibilitySequence = "Payer Responsibility Six";
                            if (payerResponsibilitySequence == '7') payerResponsibilitySequence = "Payer Responsibility Seven";
                            if (payerResponsibilitySequence == '8') payerResponsibilitySequence = "Payer Responsibility Eight";
                            if (payerResponsibilitySequence == '9') payerResponsibilitySequence = "Payer Responsibility Nine";
                            if (payerResponsibilitySequence == '10') payerResponsibilitySequence = "Payer Responsibility Ten";
                            if (payerResponsibilitySequence == '11') payerResponsibilitySequence = "Payer Responsibility Eleven";
                            if (payerResponsibilitySequence == '12') payerResponsibilitySequence = "Unknown";
                            var claimAdjudicationLevel = result[i].claimAdjudicationLevel;
                            var otherpayerInfoID = result[i].otherpayerInfoID;
                            var claimFilingIndicator = result[i].claimFilingIndicator;

                            //if (claimFilingIndicator != null && claimFilingIndicator != undefined && claimFilingIndicator != " " && (claimFilingIndicator === "MA" || claimFilingIndicator === "MB")) {

                            //    removeOptionsForIndicator(claimFilingIndicator);
                            //}
                            if (claimAdjudicationLevel == "1") {

                                var newOption = "<option value='" + otherpayerInfoID + "'>" + healthPlanID + "</option>";
                                if (claimtype == "0") {
                                    $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerHealthPlanID").append(newOption);
                                }
                                if (claimtype == "1") {
                                    $("#ctl00_MainContent_uc5SubmitClaim_ucSSubmitClaimHeaderOtherPayerAdjustmentMappingInstitutional_ddlOtherPayerHealthPlanID").append(newOption);
                                }

                                if (claimtype == "2") {
                                    $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMappingProfessional_ddlOtherPayerHealthPlanID").append(newOption);
                                }
                                claimAdjudicationLevel = "Header";
                            }
                            if (claimAdjudicationLevel == "2") {

                                var newOption = "<option value='" + otherpayerInfoID + "'>" + healthPlanID + "</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentHealthPlanID").append(newOption);
                                claimAdjudicationLevel = "Detail";
                            }

                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + otherPayerName + "</span></td><td><span title='Line' class='tNumber'>" + healthPlanID + "</span></td><td><span  title='Line' class='tNumber'>" + subscriberLastName + "</span></td><td><span title='Line' class='tNumber'>" + subscriberFirstName + "</span></td><td>" + payerResponsibilitySequence + "</td><td>" + claimAdjudicationLevel + "</td><td><input type='button' value = 'Edit' onClick = 'return EditDentalOtherPayerLineItem(\"" + otherpayerInfoID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteDentalOtherPayerLineitem(\"" + otherpayerInfoID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                        };

                        table = table + "</tbody></table>";
                        localStorage.setItem("otherPayerInfoTableDental", "" + table + "");
                        document.getElementById('<%= divDentlOP.ClientID %>').innerHTML = table;
                        ClearotherPayerInfofields();
                        if (claimtype == "0") { otherPayerHeaderDetailBind(); }
                        if (claimtype == "1") { otherPayerInstHeaderDetailBind(); }
                        if (claimtype == "2") { otherPayerProfHeaderDetailBind(); }
                        reloadChildPanel();
                    }
                    else {
                        document.getElementById('<%= divDentlOP.ClientID %>').innerHTML = "";
                        ClearotherPayerInfofields();
                        loadDropDownList();
                        loadddlClaimFilingIndicator();
                        if (claimtype == "0") { otherPayerHeaderDetailBind(); }
                        if (claimtype == "1") { otherPayerInstHeaderDetailBind(); }
                        if (claimtype == "2") { otherPayerProfHeaderDetailBind(); }
                        reloadChildPanel();

                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {

                }
            });
        }
    }
    function reloadChildPanel() {
        GetOtherPayerPaidAmountBind();
        otherPayerAdjSerDetailBind();
    }

    function ClearotherPayerInfofields() {
        $("#<%= txtOtherPayerInformation.ClientID %>").first().val("");
        $("#<%= txtHealthPlanID.ClientID %>").first().val("");
        $("#<%= txtSubscriptionNumber.ClientID %>").first().val("");
        $("#<%= txtPolicyNumber.ClientID %>").first().val("");
        $("#<%= txtGroupName.ClientID %>").first().val("");
        $("#<%= txtInsuredfirstName.ClientID %>").first().val("");
        $("#<%= txtInsuredLastName.ClientID %>").first().val("");
        $("#<%= txtOtherPayerMiddleName.ClientID %>").first().val("");
        $("#<%= txtSubcribersAddressLine1.ClientID %>").first().val("");
        $("#<%= txtSubscribersAddressLine2.ClientID %>").first().val("");
        $("#<%= txtSubcribersCity.ClientID %>").first().val("");
        $("#<%= txtSubscribersZip.ClientID %>").first().val("");
        $("#<%= txtClaimNumber.ClientID %>").first().val("");
        $("#<%= txtPaidDate1.ClientID %>").first().val("");
        $("#<%= txtPaidAmount1.ClientID %>").first().val("");
        $("#<%= txtNonCoveredAmount.ClientID %>").first().val("");
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlClaimFilingIndicator") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlClaimFilingIndicator").options[0].selected = true;
        }
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPayerSequence").options[0].selected = true;
        }
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlInsuranceTypeCode") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlInsuranceTypeCode").options[0].selected = true;
        }
       
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlClaimAdjudicationLevel") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlClaimAdjudicationLevel").options[0].selected = true;
        }
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPatientRelationship") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPatientRelationship").options[0].selected = true;
        }
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlSubcribersState") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlSubcribersState").options[0].selected = true;
        }

    }

    function cancelOtherPayer() {
        ClearotherPayerInfofields();
        document.getElementById('<%= btnOtherPayerAdd.ClientID%>').style.visibility = "visible";
        document.getElementById('<%= btnOtherPayerUpdate.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnOtherPayerCancel.ClientID%>').style.visibility = "hidden";
        var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetDentalOtherPayerLineitem?claimid=" + claimid,
            //data: '{claimid: "' + claimid + '"  }',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var claimOtherPayerInfoList = result;
                if (claimOtherPayerInfoList.length > 0) {
                    loadddlClaimFilingIndicator();
                    for (var i = 0; i < claimOtherPayerInfoList.length; i++) {
                        var payerResponsibilitySequence = result[i].payerResponsibilitySequence;
                        removeOptions(payerResponsibilitySequence);
                        if (payerResponsibilitySequence == '1') payerResponsibilitySequence = "Primary";
                        if (payerResponsibilitySequence == '2') payerResponsibilitySequence = "Secondary";
                        if (payerResponsibilitySequence == '3') payerResponsibilitySequence = "Tertiary";
                        if (payerResponsibilitySequence == '4') payerResponsibilitySequence = "Payer Responsibility Four";
                        if (payerResponsibilitySequence == '5') payerResponsibilitySequence = "Payer Responsibility Five";
                        if (payerResponsibilitySequence == '6') payerResponsibilitySequence = "Payer Responsibility Six";
                        if (payerResponsibilitySequence == '7') payerResponsibilitySequence = "Payer Responsibility Seven";
                        if (payerResponsibilitySequence == '8') payerResponsibilitySequence = "Payer Responsibility Eight";
                        if (payerResponsibilitySequence == '9') payerResponsibilitySequence = "Payer Responsibility Nine";
                        if (payerResponsibilitySequence == '10') payerResponsibilitySequence = "Payer Responsibility Ten";
                        if (payerResponsibilitySequence == '11') payerResponsibilitySequence = "Payer Responsibility Eleven";
                        if (payerResponsibilitySequence == '12') payerResponsibilitySequence = "Unknown";
                        var claimFilingIndicator = result[i].claimFilingIndicator;
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });
        var table = localStorage.getItem("otherPayerInfoTableDental");
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= divDentlOP.ClientID %>').innerHTML = table;
        }

        return false;
    }

    function paidDateChange() {

        DisableFields();
        document.getElementById('<%= btnLoading3.ClientID %>').style.display = 'block';

    }

    function DisableFields() {
       
    }

    function loadAddOtherPayer() {

        var OtherPayerInformation = $("#<%= txtOtherPayerInformation.ClientID %>").first().val();
        var patientrelationship = $("#<%=ddlPatientRelationship.ClientID %> option:selected").text();
        var HealthPlanID = $("#<%= txtHealthPlanID.ClientID %>").first().val();
        var InsuredFirstName = $("#<%= txtInsuredfirstName.ClientID %>").first().val();
        var ClaimFilingIndicator = $("#<%=ddlClaimFilingIndicator.ClientID %> option:selected").text();
        var insuredLastName = $("#<%= txtInsuredLastName.ClientID %>").first().val();
        var ddlPayerSeq = $("#<%=ddlPayerSequence.ClientID %> option:selected").text();
        var Subscriptionnumber = $("#<%= txtSubscriptionNumber.ClientID %>").first().val();
        if (OtherPayerInformation != "" && patientrelationship != "" && HealthPlanID != "" && InsuredFirstName != "" && ClaimFilingIndicator != "" && insuredLastName != "" && ddlPayerSeq != "" && OtherPayerInformation != "" && Subscriptionnumber != "") {
            DisableFields();
            document.getElementById('<%= btnOtherPayerAdd.ClientID %>').style.display = 'none';
            document.getElementById('<%= btnloading1.ClientID %>').style.display = 'block';
        }
    }


    function loadOtherPayer() {
        DisableFields();
        document.getElementById('<%= ddlPayerSequence.ClientID %>').style.display = 'none';
        document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';

    }
    function ddllevel() {
        DisableFields();
        document.getElementById('<%= btnloading2.ClientID %>').style.display = 'block';
    }

    function PDisableEnableConditionAddButton() {
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_btnOtherPayerAdd").prop("disabled", true)
        }, 50);
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_btnOtherPayerAdd").prop('disabled', false);
        }, 1000);
    }
    function validateDate17() {
        var daterequested = document.getElementById("<%= txtPaidDate1.ClientID%>").value;

        var tdate = new Date();
        var dd = tdate.getDate();
        var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
        var yyyy = tdate.getFullYear();
        var currentDate = (MM) + "/" + dd + "/" + yyyy;

        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_OtherprayerInforDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_OtherprayerInforDateRequiredError1').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_txtPaidDate1').val('');
        }
        else {
            $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_OtherprayerInforDateRequiredError1').css('display', 'none');
        }

        if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_OtherprayerInforDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_OtherprayerInforDateRequiredError1').text('Future Date not allowed.');
            $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_txtPaidDate1').val('');
        }

    }

    function recipentData() {
        var patientRelationship = $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlPatientRelationship option:selected").val();
        if (patientRelationship != null && patientRelationship != undefined && patientRelationship != "") {
            $("[id*=errPatientRelation]").text('');
            if (patientRelationship == '18') {

                var lblAddreess = $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblAddress').text();
                var lbladdress2 = $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblAddressLine2').text();
                var lblcity = $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblCity').text();
                var lblzipcode = $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblZipcode').text();
                var lblstate = $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblState').text();
                var lblfrstname = $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblfrstmi').text();
                var lblmiddlename = $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblMiddleName').text();
                var lbllastname = $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblLastName').text();
                if (lblfrstname != null && lblfrstname != undefined && lblfrstname != "") {

                    $("#<%= txtInsuredfirstName.ClientID %>").first().val(lblfrstname);
                }
                if (lbllastname != null && lbllastname != undefined && lbllastname != "") {

                    $("#<%= txtInsuredLastName.ClientID %>").first().val(lbllastname);
                }
                if (lblmiddlename != null && lblmiddlename != undefined && lblmiddlename != "") {

                    $("#<%= txtOtherPayerMiddleName.ClientID %>").first().val(lbllastname);
                }
                if (lblmiddlename != null && lblmiddlename != undefined && lblmiddlename != "") {

                    $("#<%= txtOtherPayerMiddleName.ClientID %>").first().val(lblmiddlename);
                }
                if (lblAddreess != null && lblAddreess != undefined && lblAddreess != "") {

                    $("#<%= txtSubcribersAddressLine1.ClientID %>").first().val(lblAddreess);
                }
                if (lbladdress2 != null && lbladdress2 != undefined && lbladdress2 != "") {

                    $("#<%= txtSubscribersAddressLine2.ClientID %>").first().val(lbladdress2);
                }
                if (lblcity != null && lblcity != undefined && lblcity != "") {

                    $("#<%= txtSubcribersCity.ClientID %>").first().val(lblcity);
                }
                if (lblstate != null && lblstate != undefined && lblstate != "") {

                    <%--////$("#<%= ddlSubcribersState.ClientID %>").first().text(lblsta--%>

                    $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlSubcribersState option:contains("' + lblstate + '")').prop('selected', true);

                }
                if (lblzipcode != null && lblzipcode != undefined && lblzipcode != "") {

                    $("#<%= txtSubscribersZip.ClientID %>").first().val(lblzipcode);
                }
            }


            else {
                    $("#<%= txtInsuredfirstName.ClientID %>").first().val("");
                    $("#<%= txtInsuredLastName.ClientID %>").first().val("");
                    $("#<%= txtOtherPayerMiddleName.ClientID %>").first().val("");
                    $("#<%= txtOtherPayerMiddleName.ClientID %>").first().val("");
                    $("#<%= txtSubcribersAddressLine1.ClientID %>").first().val("");
                    $("#<%= txtSubscribersAddressLine2.ClientID %>").first().val("");
                    $("#<%= txtSubcribersCity.ClientID %>").first().val(lblcity);
                    $("#<%= txtSubscribersZip.ClientID %>").first().val("");
                if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlSubcribersState") != null) {
                    document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlSubcribersState").options[0].selected = true;
                }
 }
        }
        else {
            $("#<%= txtInsuredfirstName.ClientID %>").first().val("");
            $("#<%= txtInsuredLastName.ClientID %>").first().val("");
            $("#<%= txtOtherPayerMiddleName.ClientID %>").first().val("");
            $("#<%= txtOtherPayerMiddleName.ClientID %>").first().val("");
                    $("#<%= txtSubcribersAddressLine1.ClientID %>").first().val("");
                    $("#<%= txtSubscribersAddressLine2.ClientID %>").first().val("");
                    $("#<%= txtSubcribersCity.ClientID %>").first().val(lblcity);
            $("#<%= txtSubscribersZip.ClientID %>").first().val("");
            if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlSubcribersState") != null) {
                document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerInformation_ddlSubcribersState").options[0].selected = true;
            }
        }
        return false;
    }
    function ClearOtherPayerName() {
        var otherPayerName = $("#<%= txtOtherPayerInformation.ClientID %>").first().val();
        if (otherPayerName != null && otherPayerName != undefined && otherPayerName!="")
            { $("[id*=lblOthPayerNameError]").text(''); }
    }
    function validateSubscription() {
        var txtSubscriptionNumber = $("#<%= txtSubscriptionNumber.ClientID %>").first().val();
        if (txtSubscriptionNumber != null && txtSubscriptionNumber != undefined && txtSubscriptionNumber != "") { $("[id*=errSubScNum]").text(''); }
    }
    
</script>
<script src="../Scripts/jquery.inputmask.bundle.min.js">
    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9\x20]/g, '');
    }
      function allowOnlyNumber(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }   
    function onlyDotsAndNumbers(txt, event) {
        var charCode = (event.which) ? event.which : event.keyCode
        if (charCode == 46) {
            if (txt.value.indexOf(".") < 0)
                return true;
            else
                return false;;
        }
        if (txt.value.indexOf(".") > 0) {
            var txtlen = txt.value.length;
            var dotpos = txt.value.indexOf(".");
            if ((txtlen - dotpos) > 2)
                return false;
        }
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        else
            return true;
    }
    function onlyDotsAndNumbersWithNegetive(txt, event) {
        debugger;
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

        if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 45)
            return false;

        return true;
    }
   
</script>

<ajax:CollapsiblePanelExtender ID="cpeOtherPayerInformation" runat="server" Collapsed="true"
    TargetControlID="pnlOtherPayerInformations" ExpandControlID="pnlsepOtherPayerInformation"
    CollapseControlID="pnlsepOtherPayerInformation" />
<asp:Panel runat="server" ID="pnlsepOtherPayerInformation" class="CollapsingSeparator"
    onclick="javascript:CollapseExpand(this);" 
    ToolTip="Click to Expand/Collapse" CssClass="OwnerToothQuadrantInfo CollapsingSeparator">
    <span id="sepOtherPayerInformation" runat="server" class="pageHeader pH2">+ Other Payer
        Information </span>
</asp:Panel>
<asp:Panel ID="pnlOtherPayerInformations" runat="server" Style="overflow-x: hidden;">
    <asp:UpdatePanel ID="upnlOtherPayerInformations" runat="server">
        <ContentTemplate>

            <asp:HiddenField ID="hdnAddressConfirm" runat="server" Value="0" />
            <asp:HiddenField ID="hdnSaveButtonClientID" runat="server" Value="" />
            <asp:HiddenField ID="hdnClaimId" runat="server" />
            <asp:HiddenField ID="hdnFrstName" runat="server" />
            <asp:HiddenField ID="hdnLastName" runat="server" />
            <asp:HiddenField ID="hdnMiddlename" runat="server" />
            <asp:HiddenField ID="hdnPrimaryPayerSequence" runat="server" />
            <asp:HiddenField ID="hdnOtherPayerInfoId" runat="server" />
            <asp:HiddenField ID="hdnOtherPayerInfoClaimStatus" runat="server" />

            <div id="errorMessagePrimarySequence" runat="server" visible="false" class="failureNotification">
                *Please enter  Destination Payer and  Destination Payer Responsibility Sequence
            </div>
            <asp:Label ID="lblPolicynoandgroupnameErrormsg" runat="server" CssClass="failureNotification"></asp:Label>
            <asp:Label ID="lblWrongAddress" runat="server" CssClass="failureNotification">*Address validation failed. Could not find a valid destination for a mailing or package.</asp:Label>
            <asp:Label ID="lblStreetNormalized" runat="server" CssClass="failureNotification">*Street name normalized, but no matching address was found.</asp:Label>
            <asp:Label ID="lblMultipleAddress" runat="server" CssClass="failureNotification">*Multiple possible addresses, but no exact match made. Number of possible addresses is {0}.</asp:Label>
            <asp:Label ID="lblNoAddress" runat="server" CssClass="failureNotification">*Address not found. Details:</asp:Label>
            <asp:Label ID="lblPoBox" runat="server" CssClass="failureNotification">*If the address is a PO Box, it must start with 'P.O. Box</asp:Label>
            <br />
            <div>
                <div>
                    <div id="divDentlOP" runat="server">
                    </div>
                   
                   
                    <%-- <asp:GridView runat="server"  ID="grdOtherPayer" AutoGenerateColumns="False" AllowSorting="false"  CssClass="gridview" 
                    Width="100%"  
                    HeaderStyle-CssClass="header-center" BorderWidth="0px" 
                    GridLines="None" BorderStyle="None"
                    HeaderStyle-HorizontalAlign="Center"
                    HorizontalAlign="Center" Style="margin-left: 100px; float:left;margin-bottom:20px"
                    OnRowCommand="grd_OtherPayer_RowCommand"
                    OnRowDataBound="grd_OtherPayer_RowDataBound"
                    DataKeyNames="Claims_Other_Payer_Information_ID">
           
                    <Columns>
                        <asp:BoundField DataField="Other_Payer_Name" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderText="Other Payer Name" SortExpression="Other_Payer_Name" >
                            <ItemStyle Width="10%" HorizontalAlign="Center"/> 
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Health_Plan_ID" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderText="Health Plan Id" SortExpression="Health_Plan_ID"   >
                            <ItemStyle Width="10%" HorizontalAlign="Center"/> 
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Subscriber_Last_Name" HeaderText="Insured Last Name" SortExpression="Subscriber_Last_Name"  >
                            <ItemStyle Width="10%" HorizontalAlign="Center"/> 
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Subscriber_First_Name" SortExpression="Subscriber_First_Name" 
                            HeaderText="Insured First Name" >
                            <ItemStyle Width="10%" HorizontalAlign="Center"/> 
                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PRIOR_AUTH_SUBMIT_CLAIM_PAYERSEQUENCE_DESC" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderText="Payer Sequence" 
                            SortExpression="PRIOR_AUTH_SUBMIT_CLAIM_PAYERSEQUENCE_DESC">
                            <ItemStyle Width="20%" HorizontalAlign="Center"/> 
                            <HeaderStyle Width="20%" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PRIOR_AUTH_SUBMIT_CLAIM_ADJUDICATION_LEVEL_DESC" SortExpression="PRIOR_AUTH_SUBMIT_CLAIM_ADJUDICATION_LEVEL_DESC" 
                            HeaderText="Adjudication Level" HeaderStyle-CssClass="GridviewHeaderAsterisk" >
                            <ItemStyle Width="20%" HorizontalAlign="Center" /> 
                            <HeaderStyle Width="20%" HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField ItemStyle-Width="10%" HeaderStyle-Width="10%">
                            <ItemTemplate >
                                <asp:Button ID="btnEdit" runat="server" CausesValidation="false" CommandName="btnOtherPayerEdit"
                                    CssClass="btn btn-primary" Font-Bold="True" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    ToolTip="Edit" AlternateText="Edit" Height="40px" Width="80px" Font-Size="Medium"
                                    Visible="true" Text="Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-Width="10%" HeaderStyle-Width="10%">                           
                            <ItemTemplate >
                                <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-danger" Font-Bold="True"
                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');"
                                    AlternateText="Delete" Height="40px" Width="80px" Font-Size="Medium"
                                    Visible="true" Text="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader"  />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>--%>
                </div>
            </div>

            <%--</div>--%>
            <div>
                <uc:HeaderOtherPayerAdjustmentMappingProfessional runat="server" ID="ucHeaderOtherPayerAdjustmentMappingProfessional"
                    Visible="false" />
                <uc:HeaderOtherPayerAdjustmentMapping runat="server" ID="ucHeaderOtherPayerAdjustmentMapping"
                    Visible="false" />
                <uc:HeaderOtherPayerAdjustmentMappingInstitutional runat="server" ID="ucHeaderOtherPayerAdjustmentMappingInstitutional"
                    Visible="false" />

            </div>
            <div id="divOther" runat="server">
             

                <div class="row">
                    <asp:HiddenField ID="hdnRowNumberOtherPayer" runat="server" />
                    <asp:HiddenField ID="hdnHealthPlanIdOnEdit" runat="server" />     
                    <asp:HiddenField ID="hdnotherPayer_ClaimType" runat="server" />
                    <div class="col-sm-4">
                        <div class="row" id="lblOtherPayerInformation" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Other Payer Name
                                :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtOtherPayerInformation" runat="server" CssClass="formFieldTextBox" MaxLength="60" onKeyUp="javascript:alphanumericOnly(this);"  OnChange="ClearOtherPayerName()"
                                        Style="height: 30px; width: 210px"
                                        ValidationGroup="valOtherPayerInformation" /><br />
                                     <asp:Label ID="lblOthPayerNameError" runat="server" ForeColor="Red"></asp:Label>
                                   <%-- <asp:RequiredFieldValidator ID="rfvOtherPayerInformation" runat="server" ControlToValidate="txtOtherPayerInformation" CssClass="failureNotification"
                                        Text="*Other Payer Name is required" Display="Dynamic" ValidationGroup="valOtherPayerInformation" SetFocusOnError="true">
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtOtherPayerInformation" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row" id="lblPatientRelationship" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Patient Relationship
                                To Subscriber :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlPatientRelationship"  onChange="recipentData()" EnableViewState="true" runat="server"  
                                        AppendDataBoundItems="True" Style="min-width: 180px; height: 30px" ValidationGroup="valOtherPayerInformation" class="selectdropdown">
                                    </asp:DropDownList><br />
                                    <asp:Label ID="errPatientRelation" runat="server" ForeColor="Red"></asp:Label>
                                   <%-- <asp:RequiredFieldValidator runat="server" ID="rfvPatientRelationshipToSubscriber"
                                        SetFocusOnError="true"
                                        ValidationGroup="valOtherPayerInformation" ControlToValidate="ddlPatientRelationship" CssClass="failureNotification"
                                        Text="*Patient relationship is required" Display="Dynamic" InitialValue="" />--%>

                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row" id="lblClaimAdjudicationLevel" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Claim Adjudication
                                Level :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlClaimAdjudicationLevel" EnableViewState="true" runat="server" 
                                        AppendDataBoundItems="True" Style="min-width: 180px; height: 30px;" ValidationGroup="valOtherPayerInformation" OnSelectedIndexChanged="ddlClaimAdjudicationLevel_OnSelectedIndexChanged"
                                        Enabled="true" onChange="return claimAdjLevelChange()">
                                    </asp:DropDownList>
                                    <button id="btnloading2" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>

                                    <br />
                                    <asp:Label ID="ErrClaimAdjudication" runat="server" ForeColor="Red"></asp:Label>
                                    <%--<asp:RequiredFieldValidator runat="server" ID="rfvAdjudication" SetFocusOnError="true"
                                        Enabled="false"
                                        ValidationGroup="valOtherPayerInformation" ControlToValidate="ddlClaimAdjudicationLevel" CssClass="failureNotification"
                                        Text="*Claim Adjudication Level is requiered" Display="Dynamic"
                                        InitialValue="" />--%>

                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4">
                        <div class="row" id="lblHealthPlanID" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Health Plan ID
                                :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">

                                    <asp:TextBox ID="txtHealthPlanID" runat="server" CssClass="formFieldTextBox" MaxLength="80" onKeyUp="javascript:alphanumericOnly(this);" 
                                        Style="height: 30px; width: 210px" onChange="return validatehpId()" ValidationGroup="valOtherPayerInformation" /><br />
                                      <asp:Label ID="healthPlanIdError" runat="server" ForeColor="Red"></asp:Label>
                                    <%--<asp:RequiredFieldValidator ID="rfvHealthPlanID" runat="server" ControlToValidate="txtHealthPlanID" CssClass="failureNotification"
                                        Text="*Health Plan ID is Required" Display="Dynamic" ValidationGroup="valOtherPayerInformation" SetFocusOnError="true">
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtHealthPlanID" CssClass="failureNotification"
                                        ID="rfvMinimumHealthPlanId"
                                        ValidationExpression="^[\s\S]{2,}$"
                                        runat="server" ErrorMessage="<div>*Minimum 2 characters is required.</div>">
                                    </asp:RegularExpressionValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ControlToValidate="txtHealthPlanID" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                    <asp:Label runat="server" ID="lblHealthPlanIdDuplicate" CssClass="failureNotification" Text="Same health plan ID cannot be used for more than one payer" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row" id="lblInsuredfirstName" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Subscribers First
                                Name :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtInsuredfirstName" runat="server" CssClass="formFieldTextBox" MaxLength="35" onKeyUp="javascript:alphanumericOnly(this);"
                                        Style="height: 30px; width: 210px" ValidationGroup="valOtherPayerInformation" /><br />
                                 <asp:Label ID="errFirstName" runat="server" ForeColor="Red"></asp:Label>
                                     <%--<asp:RequiredFieldValidator ID="rfvInsuredfirstName" runat="server" ControlToValidate="txtInsuredfirstName" CssClass="failureNotification" SetFocusOnError="true"
                                        Text="*Insured’s First Name is required" Display="Dynamic" ValidationGroup="valOtherPayerInformation">
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtInsuredfirstName" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row" id="lblClaimNumber" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Claim Number :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtClaimNumber" runat="server" CssClass="formFieldTextBox" MaxLength="50" onKeyUp="javascript:alphanumericOnly(this);"  
                                        Style="height: 30px; width: 210px;" Enabled="true"  ValidationGroup="valOtherPayerInformation" />
                                    <asp:Label ID="errClaimNo" runat="server" ForeColor="Red"></asp:Label>
                                   <%-- <asp:RequiredFieldValidator ID="rfvClaimNumber" runat="server" ControlToValidate="txtClaimNumber" CssClass="failureNotification"
                                        Enabled="false" SetFocusOnError="true"
                                        ErrorMessage="<div>*Claim Number is Required</div>" Display="Dynamic" ValidationGroup="valOtherPayerInformation">
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtClaimNumber" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4">
                        <div class="row" id="lblClaimFilingIndicator" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Claim Filing Indicator
                                :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlClaimFilingIndicator" EnableViewState="true" runat="server"
                                        AppendDataBoundItems="True" Style="height: 30px; width: 180px; min-width: 80px;"
                                        ValidationGroup="valOtherPayerInformation" class="selectdropdown">
                                    </asp:DropDownList><br />
                                    <asp:Label ID="errorClaimFilingIndicator" runat="server" ForeColor="Red"></asp:Label>
                                    <%--<asp:RequiredFieldValidator runat="server" ID="rfvClaimFilingIndicator" SetFocusOnError="true" CssClass="failureNotification"
                                        ValidationGroup="valOtherPayerInformation" ControlToValidate="ddlClaimFilingIndicator"
                                        Text="*Claim Filing Indicator missing" Display="Dynamic" InitialValue="" />--%>

                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row" id="lblInsuredLastName" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Subscribers Last Name :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtInsuredLastName" runat="server" CssClass="formFieldTextBox" MaxLength="60" onKeyUp="javascript:alphanumericOnly(this);"
                                        Style="height: 30px; width: 210px" ValidationGroup="valOtherPayerInformation" /><br />
                                    <asp:Label ID="errLastName" runat="server" ForeColor="Red"></asp:Label>
                                    <%--<asp:RequiredFieldValidator ID="rfvInsuredLastName" runat="server" ControlToValidate="txtInsuredLastName" CssClass="failureNotification" SetFocusOnError="true"
                                        Text="*Insured’s Last Name is required" Display="Dynamic" ValidationGroup="valOtherPayerInformation">
                                    </asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtInsuredLastName" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row" id="lblPaidDate" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Paid Date :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtPaidDate1" Enabled="true" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 210px;"
                                         ValidationGroup="valOtherPayerInformation"  OnChange="return validateDate17()" />
                                    <asp:Image ID="ImagePaidDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp"
                                        Height="16px" AlternateText="Calendar Icon" />
                                    <ajax:CalendarExtender ID="cePaidDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtPaidDate1" PopupPosition="BottomLeft" CssClass="QstCalendarCSS" EnabledOnClient="true" />
                                    <%--<ajax:CalendarExtender ID="ceProfessi" runat="server" Format="MM/dd/yyyy" TargetControlID="txtProfessi" PopupPosition="BottomLeft" CssClass="QstCalendarCSS" EnabledOnClient="true" />--%>
                                    <asp:Label ID="errPaidDate" runat="server" ForeColor="Red"></asp:Label>
                                   <%-- <asp:RequiredFieldValidator ID="rfvPaidDate" runat="server" ControlToValidate="txtPaidDate" CssClass="failureNotification"
                                       
                                        ErrorMessage="<div>*Paid Date is required</div>" Text="*Paid Date is required" Display="Dynamic" ValidationGroup="valOtherPayerInformation" />--%>
                                    <button id="btnLoading3" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>

                                    <%--<asp:CompareValidator ID="cvtPaidDate" runat="server" Type="Date" Operator="LessThanEqual"
                                    ControlToValidate="txtPaidDate" ValidationGroup="valOtherPayerInformation" ForeColor="Red"
                                    ErrorMessage="*Paid date cannot be greater than today’s date"
                                    Display="Dynamic" SetFocusOnError="true" />--%>
                                    <asp:Label ID="OtherprayerInforDateRequiredError1" runat="server" Text="" CssClass="error-message" Style="display: none;"></asp:Label>

                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4">
                        <div class="row" id="lblPayerSequence" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Payer Responsibility
                                Sequence :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <button id="btnloading" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>


                                    <asp:DropDownList ID="ddlPayerSequence" EnableViewState="true" runat="server"
                                        AppendDataBoundItems="True" Style="width: 180px; min-width: 80px; height: 30px"
                                        ValidationGroup="valOtherPayerInformation" onChange="return otherPayerDisablefieldsWithClear()"
                                        OnSelectedIndexChanged="OtherPayerResponsibilitySequence_OnSelectedIndexChanged" class="selectdropdown">
                                    </asp:DropDownList><br />
                                    <asp:Label ID="errorPayerSeq" runat="server" ForeColor="Red"></asp:Label>
                                    <%--<asp:RequiredFieldValidator runat="server" ID="rfvPayerSequence" SetFocusOnError="true" CssClass="failureNotification"
                                        ValidationGroup="valOtherPayerInformation" ControlToValidate="ddlPayerSequence"
                                        Text="*Payer sequence is required"
                                        Display="Dynamic" InitialValue="" />--%>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row" id="lblSubscribersMiddleName" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Subscribers Middle
                                Name :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtOtherPayerMiddleName" runat="server" CssClass="formFieldTextBox" MaxLength="25" onKeyUp="javascript:alphanumericOnly(this);"
                                        Style="height: 30px; width: 210px" />
                                </span>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="txtOtherPayerMiddleName" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row" id="lblPaidAmount1" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Paid Amount :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtPaidAmount1" runat="server" CssClass="formFieldTextBox" MaxLength="18" onkeypress="return onlyDotsAndNumbersWithNegetive(this,event);"
                                         Style="height: 30px; width: 210px;" Enabled="true" ValidationGroup="valOtherPayerInformation" />
                                  <asp:Label ID="errPaidAmount" runat="server" ForeColor="Red"></asp:Label>
                                    <%-- <asp:RequiredFieldValidator runat="server" ID="rfPaidAmount1" Enabled="false"
                                        ControlToValidate="txtPaidAmount1" ErrorMessage="<div>*Paid Amount is required</div>"
                                        Text="" CssClass="failureNotification"
                                        Display="Dynamic"
                                        SetFocusOnError="true" ValidationGroup="valOtherPayerInformation" />--%>
                                    <asp:RegularExpressionValidator ID="RegularExpressionemojiValidatorPaidAmount" runat="server"
                                        ControlToValidate="txtPaidAmount1" ValidationExpression="^[A-Za-z0-9?/./d ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input"
                                        Display="Dynamic" ForeColor="Red" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">

                    <div class="col-sm-4">
                        <div class="row" id="lblSubscriptionNumber" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Subscribers Number:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtSubscriptionNumber" runat="server" CssClass="formFieldTextBox" MaxLength="80" onKeyUp="javascript:alphanumericOnly(this);" onChange="return validateSubscription()"
                                        Style="height: 30px; width: 210px" ValidationGroup="valOtherPayerInformation" /><br />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtSubscriptionNumber" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                    <asp:Label ID="errSubScNum" runat="server" ForeColor="Red"></asp:Label>
                                    <%--<asp:RequiredFieldValidator ID="rfvSubscriptionNumner" runat="server" ControlToValidate="txtSubscriptionNumber" CssClass="failureNotification"
                                        Text="*Subscription Number is Required" Display="Dynamic" ValidationGroup="valOtherPayerInformation"></asp:RequiredFieldValidator>--%>
                                    <asp:RegularExpressionValidator runat="server"
                                        Display="Dynamic" CssClass="failureNotification"
                                        ValidationGroup="valOtherPayerInformation"
                                        ControlToValidate="txtSubscriptionNumber"
                                        ValidationExpression="^[\s\S]{2,80}$"
                                        ErrorMessage="<div>*Subscriber number field length should be in between 2-80 characters</div>">
                                    </asp:RegularExpressionValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row" id="lblSubscribersAddress" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Subscribers Address Line 1:</span>

                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtSubcribersAddressLine1" runat="server" CssClass="formFieldTextBox" MaxLength="55" onKeyUp="javascript:alphanumericOnly(this);"
                                        Style="height: 30px; width: 210px" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtSubcribersAddressLine1" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                    <asp:Label runat="server" CssClass="failureNotification" ID="lblAddress1">*Address Line 1 is required</asp:Label>
                                  <asp:Label ID="lblAddress1error" runat="server" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row" id="lblNonCoveredAmount" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Non Covered Amount:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtNonCoveredAmount" runat="server" Enabled="true" CssClass="formFieldTextBox" MaxLength="35" onkeypress="return onlyDotsAndNumbers();"
                                         Style="height: 30px; width: 210px;" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorNonCovered" runat="server"
                                        ControlToValidate="txtNonCoveredAmount" ValidationExpression="^[0-9?/./d ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input"
                                        Display="Dynamic" ForeColor="Red" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4">
                        <div class="row" id="lblPolicyNumber" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Policy Number:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtPolicyNumber" runat="server" CssClass="formFieldTextBox" MaxLength="50" onKeyUp="javascript:alphanumericOnly(this);"
                                        Style="height: 30px; width: 210px" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtPolicyNumber" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row" id="divSubscribersAddress2" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Subscribers Address Line 2:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtSubscribersAddressLine2" runat="server" CssClass="formFieldTextBox" onKeyUp="javascript:alphanumericOnly(this);"
                                        MaxLength="55" Style="height: 30px; width: 210px" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txtSubscribersAddressLine2" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">&nbsp;</div>

                </div>
                <div class="row">
                    <div class="col-sm-4">
                        <div class="row" id="lblGroupName" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Group Name:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtGroupName" runat="server" CssClass="formFieldTextBox" MaxLength="60" onKeyUp="javascript:alphanumericOnly(this);"
                                        Style="height: 30px; width: 210px" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtGroupName" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="row" id="lblSubcribersCity" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Subscribers City:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtSubcribersCity" runat="server" CssClass="formFieldTextBox" MaxLength="35" onKeyUp="javascript:alphanumericOnly(this);"
                                        Style="height: 30px; width: 210px" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ControlToValidate="txtSubcribersCity" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                    <asp:Label runat="server" ID="lblCity" CssClass="failureNotification">*City is required</asp:Label>
                                    <asp:Label ID="lblCityError" runat="server" ForeColor="Red"></asp:Label>
                                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtSubcribersCity" CssClass="failureNotification"
                                        ID="rgvCity"
                                        ValidationExpression="^[\s\S]{2,}$"
                                        runat="server" ErrorMessage="<div>*Subscribers city is invalid.</div>">
                                    </asp:RegularExpressionValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-4">
                        <div class="col-sm-6" style="text-align: right">
                            <button id="btnloading1" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>

                             <asp:Button ID="btnOtherPayerAdd" Text="ADD"  
                                 runat="server" CssClass="btn btn-primary" Font-Bold="True" Width="90px" OnClientClick="return addOtherPayerInfoCode()"/>
                        
                           <%-- <asp:Button ID="btnOtherPayerAdd" Text="ADD" runat="server" CssClass="btn btn-primary"
                                Font-Bold="True"
                                ValidationGroup="valOtherPayerInformation" CausesValidation="true" Width="90px"
                                 />--%>
                        </div>
                        <div class="col-sm-6" style="text-align: left">
                             <asp:Button ID="btnOtherPayerUpdate" Style="visibility: hidden" Text="Update"  
                                 runat="server" CssClass="btn btn-primary" 
                                 Font-Bold="True" ValidationGroup="vgCarePlan" Width="90px" 
                                 OnClientClick="return updateOtherPayerInfoCode()" />
                         
                         <asp:Button ID="btnOtherPayerCancel" Style="visibility: hidden" Text="Cancel"  
                             runat="server"
                             CssClass="btn btn-primary" 
                                 Font-Bold="True"  Width="90px" OnClientClick="return cancelOtherPayer()" />
                        

                           <%-- <asp:Button ID="btnOtherPayerUpdate" Text="Update" runat="server" CommandName="Update"
                                CssClass="btn btn-primary" Font-Bold="True"
                                ValidationGroup="valOtherPayerInformation" CausesValidation="true" Width="90px"
                                Visible="false" OnClick="btnOtherPayerUpdate_Click" />--%>
                          <%--  <asp:Button ID="btnOtherPayerCancel" Text="Cancel" runat="server" CommandName="Cancel"
                                CssClass="btn btn-danger" Font-Bold="True"
                                ValidationGroup="valOtherPayerInformation" CausesValidation="false" Width="90px"
                                Visible="False"
                                OnClick="btnOtherPayerCancel_Click" />--%>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div class="col-sm-4">
                        <div class="row" id="lblInsuranceTypeCode" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Insurance Type Code
                                :</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlInsuranceTypeCode" EnableViewState="true" runat="server"
                                        AppendDataBoundItems="True" Style="height: 30px; width: 180px; min-width: 80px;">
                                    </asp:DropDownList>

                                </span>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-4">
                        <div class="col-sm-6">
                            <div class="row" id="lblSubcribersState" runat="server" style="padding-left: 30px;">
                                <div class="col-sm-5">
                                    <span class="ohio-field" style="font-size: 15px; text-align: left">Subscribers State :</span>
                                </div>
                                <div class="col-sm-7">
                                    <span style="text-align: left;">
                                        <asp:DropDownList ID="ddlSubcribersState" EnableViewState="true" runat="server"
                                            AppendDataBoundItems="True" Style="height: 30px; width: 80px; min-width: 80px;">
                                        </asp:DropDownList>
                                        <asp:Label runat="server" ID="lblState" CssClass="failureNotification">*State is required</asp:Label>
                                        <asp:Label ID="lblStateError" runat="server" ForeColor="Red"></asp:Label>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="row" id="divSubZip" runat="server" style="padding-right: 50px;">
                                <div class="col-sm-5">
                                    <span class="ohio-field" style="font-size: 15px; text-align: left">Subcribers Zip :</span>
                                </div>
                                <div class="col-sm-7">
                                    <span style="text-align: left;">
                                        <asp:TextBox ID="txtSubscribersZip" runat="server" CssClass="formFieldTextBox" MaxLength="35"
                                            Style="height: 30px; width: 80px !important;" ValidationGroup="valOtherPayerInformation" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ControlToValidate="txtSubscribersZip" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                            ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                        <asp:Label CssClass="failureNotification" ID="lblZip" runat="server">*Zip  is required</asp:Label>
                                        <asp:Label ID="lblZipError" runat="server" ForeColor="Red"></asp:Label>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div runat="server" id="divConfirmAddress" style="display: none; text-align: center">
                        <p style="color: darkgreen">
                            According to the USPS database, the address entered is inaccurate. The following
                        address was found:
                        </p>
                        <p style="color: darkgreen" id="paraUSPSAddress" runat="server"></p>
                        <p style="color: darkgreen">Click on 'Accept' to accept the corrections.</p>
                        <br />
                        <asp:Button ID="btnConfirmAddress" CssClass="buttonBoxFocus" Text="Accept" runat="server" />
                        <asp:Button ID="btnCancelAddressCorrection" CssClass="buttonBox" Text="Cancel" runat="server" />
                    </div>
                    <div runat="server" id="divWSError" style="display: none; text-align: left">
                        <p style="color: darkgreen">
                            An error occurred while validating the entered address.<br />
                            You can continue to work, but any addresses will not be validated by the system.<br />
                            <br />
                            Error details:<br />
                        </p>
                        <p style="color: darkgreen" id="paraWSError" runat="server"></p>
                        <asp:Button ID="btnConfirmWSError" class="buttonBox" Text="Ok" runat="server" />
                    </div>


                    <asp:CustomValidator ID="cvOtherPayerAddress"
                        ControlToValidate=""
                        OnServerValidate="cvAOtherPayerAddress_ServerValidate"
                        Display="None"
                        ErrorMessage=""
                        ValidationGroup="valOtherPayerInformation"
                        runat="server" />
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Panel>
<script src="../Scripts/jquery.inputmask.bundle.min.js"></script>
<script type="text/javascript">
    function maxZIndex() {
        var highest = -999;

        $("*").each(function () {
            var current = parseInt($(this).css("z-index"), 10);
            if (current && highest < current)
                highest = current;
        });

        return highest;
    }

    jQuery.expr[':'].contains = function (a, i, m) {
        return jQuery(a).text().toUpperCase()
            .indexOf(m[3].toUpperCase()) >= 0;
    };

    function sendDataOtherPayer(newStreetAddress, newUnitAddress, floorDept, newAddressLine3, city, state, county, zip4, zip5) {

        $("#<%= txtSubcribersAddressLine1.ClientID %>").first().val(newStreetAddress);
        $("#<%= txtSubscribersAddressLine2.ClientID %>").first().val(newUnitAddress);
        $("#<%= txtSubcribersCity.ClientID %>").first().val(city);
        if ($("#<%= ddlSubcribersState.ClientID %>").first().val() !== state) {
            $("#<%= ddlSubcribersState.ClientID %>").first().val(state);
            $("#<%= ddlSubcribersState.ClientID %>").change();
        }
        $('#<%= ddlSubcribersState.ClientID %> option:contains(' + state + ')').attr("selected", "selected");



        $("#<%= txtSubscribersZip.ClientID %>").first().val(zip5);
    }
    function allowSave() {
        $("#<%= SaveButtonClientID %>").prop("disabled", false);
    }

    function setAddressConfirm(confirmValue) {
        var hdnAddressConfirm = document.getElementById("<% = hdnAddressConfirm.ClientID %>");
        hdnAddressConfirm.value = confirmValue;
    }
</script>

