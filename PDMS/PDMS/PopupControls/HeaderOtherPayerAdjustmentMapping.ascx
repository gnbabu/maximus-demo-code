<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_HeaderOtherPayerAdjustmentMapping" Codebehind="HeaderOtherPayerAdjustmentMapping.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchReason.ascx" TagPrefix="uc" TagName="SubmitClaimSearchReason" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">
<script type="text/javascript">



    function otherPayerHeaderDetailBind() {
         var Claim_ID = $("#<%= hdnClaimId.ClientID %>").first().val();
        var claimHeaderOtherAdjustmentInstiList;
        var APIToken = $("[id*=hdnAccessToken]").val();
         $.ajax({
             type: "GET",
             //data: '{ClaimId: "' + Claim_ID + '"}',
             url: webApiClaims + "getHeaderOtherPayerBind?ClaimId=" + Claim_ID,
             headers: {
                 "Access-Control-Allow-Origin": "*",
                 "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                 "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                 "Authorization": "Bearer " + APIToken
             },
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             success: function (result) {
                 claimHeaderOtherAdjustmentInstiList = result;
                 var table;

                 if (claimHeaderOtherAdjustmentInstiList.length > 0) {
                     table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:10px;' scope='col'>Reason Code</th><th style='width:10px;' scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                     for (var i = 0; i < claimHeaderOtherAdjustmentInstiList.length; i++) {
                         var HealthPlanID = result[i].cde_health_plan_id;
                         var AdjustmentGroup = result[i].cde_adjustment_group;
                         var ReasonCode = result[i].cde_reason_code;
                         var PayerAmount = result[i].cde_amount;
                         var PayerQuantity = result[i].cde_quantity;
                         var Claim_ID = result[i].Claim_ID;
                         var Claims_Header_Other_Payer_Adjustment_Information_ID = result[i].Claims_Header_Other_Payer_Adjustment_Information_ID;
                         table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + HealthPlanID + "</span></td><td><span title='Line' class='tNumber'>" + AdjustmentGroup + "</span></td><td><span  title='Line' class='tNumber'>" + ReasonCode + "</span></td><td><span title='Line' class='tNumber'>" + PayerAmount + "</span></td><td>" + PayerQuantity + "</td><td><input type='button' value = 'Edit' onClick = 'return EditFieldsHeaderAdjustmentInstitutional(\"" + Claim_ID + "\",\"" + Claims_Header_Other_Payer_Adjustment_Information_ID + "\",this);return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteHeaderAdjustmentOtherPayerInfo_Institutional(\"" + Claim_ID + "\",\"" + Claims_Header_Other_Payer_Adjustment_Information_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                     };
                     table = table + "</tbody></table>";
                     localStorage.setItem("HeaderAdjustmentTable", "" + table + "");
                     document.getElementById('<%= providerHeaderAdjustmentdentalOutput.ClientID %>').innerHTML = table;
                      HeaderAdjInstiClearFields();
                      BindHeaderOtherAdjustmentGroupDropdown();
                  }
                  else {
                     document.getElementById('<%= providerHeaderAdjustmentdentalOutput.ClientID %>').innerHTML = "";
                  }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
     }
    function loadAddHeaderOtherPayerDental() {
        var HealthPlanID = $("#<%=ddlOtherPayerHealthPlanID.ClientID %> option:selected").text();
        var AdjustmentGroup = $("#<%=ddlOtherPayerAdjustmentGroup.ClientID %> option:selected").text();
       
        var ReasonCode = $("#<%= txtOtherPayerReasonCode.ClientID %>").first().val();
        var PayerAmount = $("#<%= txtOtherPayerAmount.ClientID %>").first().val();
        var PayerQuantity = $("#<%= txtOtherPayerQuantity.ClientID %>").first().val();

        var displayTabDentalHeadAdj = localStorage.getItem("HeaderAdjustmentTable");
         <%--   var grid = document.getElementById("<%= gvOtherPayerAdjustmentInfo.ClientID%>");--%>
        var inputs = displayTabDentalHeadAdj.rows[gridindexReasoncode].getElementsByTagName("INPUT");

        if (HealthPlanID != "" && AdjustmentGroup != "" && ReasonCode != "" && PayerAmount != "" && PayerQuantity != "") {
            document.getElementById('<%= ddlOtherPayerHealthPlanID.ClientID %>').disabled = true;
            document.getElementById('<%= ddlOtherPayerAdjustmentGroup.ClientID %>').disabled = true;
            document.getElementById('<%= txtOtherPayerReasonCode.ClientID %>').disabled = true;
            document.getElementById('<%= txtOtherPayerAmount.ClientID %>').disabled = true;
            document.getElementById('<%= txtOtherPayerQuantity.ClientID %>').disabled = true;
            document.getElementById('<%= btnOtherPayerAdjustmentInfoAdd.ClientID %>').style.display = 'none';
            document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';
        }
    }

    function HeaderDisableEnableConditionAddButton() {
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMappingProfessional_btnOtherPayerAdjustmentInfoAdd").prop("disabled", true)
        }, 50);
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMappingProfessional_btnOtherPayerAdjustmentInfoAdd").prop('disabled', false);
        }, 1000);
    }

    function onlyDotsAndNumbers(txt, event) {
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

        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;

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

    function DentalReasonCodetextChange() {
        var txtOtherPayerReasonCode = $("#<%= txtOtherPayerReasonCode.ClientID %>").val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetReasonCode?desc=" + txtOtherPayerReasonCode,
            //data: '{desc: "' + txtOtherPayerReasonCode + '" }',
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
                    $("[id*=txtOtherPayerReasonCode]").val('');
                    $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_lblHeaderDentalerror").text('Reason code not found.');
                }
                else {
                    $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_lblHeaderDentalerror").text('');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_lblHeaderDentalerror").text('Reason code not found.');
            }
        });
    }
    function AddButtonDentalHeaderOtherAdjustment() {
        var claimHeaderOtherAdjustmentInstiList;
        var ddlOtherPayerHealthPlanID = $("#<%=ddlOtherPayerHealthPlanID.ClientID %> option:selected").text();
        var ddlOtherPayerAdjustmentGroup = $("#<%=ddlOtherPayerAdjustmentGroup.ClientID %> option:selected").text();
        var txtOtherPayerReasonCode = $("#<%= txtOtherPayerReasonCode.ClientID %>").val();
        var txtOtherPayerAmount = $("#<%= txtOtherPayerAmount.ClientID %>").val();
        var txtOtherPayerQuantity = $("#<%= txtOtherPayerQuantity.ClientID %>").val();
        validateFields();
        var hdnClaimIdInsti = $("#<%= hdnClaimId.ClientID %>").val();
        var lblErrorMessageHeaderOtherPayerDental = $("#<%= lblErrorMessageHeaderOtherPayerDental.ClientID %>").text();

        if (validateField == "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "POST",
                url: webApiClaims + "AddHeaderAdjustmentOtherPayerInfo_Institutional?hdnClaimId=" + hdnClaimIdInsti + "&&lblErrorMessageHeaderOtherAdjustment=" + lblErrorMessageHeaderOtherPayerDental + "&&ddlOtherPayerHealthPlanID=" + ddlOtherPayerHealthPlanID + "&&ddlOtherPayerAdjustmentGroup=" + ddlOtherPayerAdjustmentGroup + "&&txtOtherPayerReasonCode=" + txtOtherPayerReasonCode + "&&txtOtherPayerAmount=" + txtOtherPayerAmount + "&&txtOtherPayerQuantity=" + txtOtherPayerQuantity + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                //data: '{hdnClaimId: "' + hdnClaimIdInsti + '" , lblErrorMessageHeaderOtherAdjustment: "' + lblErrorMessageHeaderOtherPayerDental + '" , ddlOtherPayerHealthPlanID: "' + ddlOtherPayerHealthPlanID + '" , ddlOtherPayerAdjustmentGroup: "' + ddlOtherPayerAdjustmentGroup + '" , txtOtherPayerReasonCode: "' + txtOtherPayerReasonCode + '" , txtOtherPayerAmount: "' + txtOtherPayerAmount + '" , txtOtherPayerQuantity: "' + txtOtherPayerQuantity + '"  }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimHeaderOtherAdjustmentInstiList = result;
                    if (result[0].lblErrorMessageHeaderOtherAdjustment) {
                        $("[id*=lblErrorMessageHeaderOtherPayerDental]").text(result[0].lblErrorMessageHeaderOtherAdjustment);
                    }
                    else {
                        var table;
                        if (claimHeaderOtherAdjustmentInstiList.length > 0) {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:10px;' scope='col'>Reason Code</th><th style='width:10px;' scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                            for (var i = 0; i < claimHeaderOtherAdjustmentInstiList.length; i++) {
                                var HealthPlanID = result[i].cde_health_plan_id;
                                var AdjustmentGroup = result[i].cde_adjustment_group;
                                var ReasonCode = result[i].cde_reason_code;
                                var PayerAmount = result[i].cde_amount;
                                var PayerQuantity = result[i].cde_quantity;
                                var Claim_ID = result[i].Claim_ID;
                                var Claims_Header_Other_Payer_Adjustment_Information_ID = result[i].Claims_Header_Other_Payer_Adjustment_Information_ID;
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + HealthPlanID + "</span></td><td><span title='Line' class='tNumber'>" + AdjustmentGroup + "</span></td><td><span  title='Line' class='tNumber'>" + ReasonCode + "</span></td><td><span title='Line' class='tNumber'>" + PayerAmount + "</span></td><td>" + PayerQuantity + "</td><td><input type='button' value = 'Edit' onClick = 'return EditFieldsHeaderAdjustmentInstitutional(\"" + Claim_ID + "\",\"" + Claims_Header_Other_Payer_Adjustment_Information_ID + "\",this);return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteHeaderAdjustmentOtherPayerInfo_Institutional(\"" + Claim_ID + "\",\"" + Claims_Header_Other_Payer_Adjustment_Information_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            };
                            table = table + "</tbody></table>";
                            localStorage.setItem("HeaderAdjustmentTable", "" + table + "");
                            document.getElementById('<%= providerHeaderAdjustmentdentalOutput.ClientID %>').innerHTML = table;
                            HeaderAdjInstiClearFields();
                            BindHeaderOtherAdjustmentGroupDropdown();
                          }
                          else {
                              HeaderAdjInstiClearFields();
                          }
                      }
                  },
                  error: function (jqXHR, textStatus, errorThrown) {
                      $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('no data found');
                  }

              });
        }
        return false;
        function validateFields() {
            if ((ddlOtherPayerHealthPlanID == null || ddlOtherPayerHealthPlanID == "" || ddlOtherPayerHealthPlanID == undefined)) {
                validateField = "false";
                $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('Health plan ID is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('');
            }

            if ((ddlOtherPayerAdjustmentGroup == null || ddlOtherPayerAdjustmentGroup == "" || ddlOtherPayerAdjustmentGroup == undefined)) {

                validateField = "false";
                $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('Adjustment group is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('');
            }
            if ((txtOtherPayerReasonCode == null || txtOtherPayerReasonCode == "" || txtOtherPayerReasonCode == undefined)) {
                validateField = "false";
                $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('Reason code is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('');
            }
            if ((txtOtherPayerAmount == null || txtOtherPayerAmount == "" || txtOtherPayerAmount == undefined)) {
                validateField = "false";
                $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('Amount is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('');
            }
            if ((txtOtherPayerQuantity != null || txtOtherPayerQuantity != "" || txtOtherPayerAmount != undefined)) {
                if (txtOtherPayerQuantity.charAt(0) === "0") {
                    validateField = "false";
                    $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('Invalid Quantity');
                    return false;
                }
            }
            else {
                validateField = "true";
                $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('');
            }
            var Claim_ID = $("#<%= hdnClaimId.ClientID %>").first().val();
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                //data: '{ClaimId: "' + Claim_ID + '"}',
                url: webApiClaims + "getHeaderOtherPayerBind?ClaimId=" + Claim_ID,
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimHeaderOtherAdjustmentInstiList = result;
                    for (var i = 0; i < claimHeaderOtherAdjustmentInstiList.length; i++) {
                        var HealthPlanID = result[i].cde_health_plan_id;
                        var AdjustmentGroup = result[i].cde_adjustment_group;
                        var ReasonCode = result[i].cde_reason_code;
                        var PayerAmount = result[i].cde_amount;
                        var PayerQuantity = result[i].cde_quantity;
                        var Claim_ID = result[i].Claim_ID;
                        var Claims_Header_Other_Payer_Adjustment_Information_ID = result[i].Claims_Header_Other_Payer_Adjustment_Information_ID;

                        if ((ddlOtherPayerHealthPlanID == HealthPlanID) && (ddlOtherPayerAdjustmentGroup == AdjustmentGroup) && (txtOtherPayerReasonCode == ReasonCode)) {
                            $("[id*=lblErrorMessageHeaderOtherAdjustment]").text('Health Plan, Adjustment Group, and Reason Code must be unique');
                            validateField = "false";
                            return false;
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorMessageHeaderOtherAdjustment]").text('Reason code not found.');
                }
            });

        }
    }
    function EditHeaderAdjustmentOtherPayerInfo_Institutional() {
        $("[id*=lblErrorMessageHeaderOtherAdjustment]").text('');
        $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('');
        var claimHeaderOtherAdjustmentInstiList;
        var ddlOtherPayerHealthPlanID = $("#<%=ddlOtherPayerHealthPlanID.ClientID %> option:selected").text();
        var ddlOtherPayerAdjustmentGroup = $("#<%=ddlOtherPayerAdjustmentGroup.ClientID %> option:selected").text();
        var txtOtherPayerReasonCode = $("#<%= txtOtherPayerReasonCode.ClientID %>").first().val();
        var txtOtherPayerAmount = $("#<%= txtOtherPayerAmount.ClientID %>").first().val();
        var txtOtherPayerQuantity = $("#<%= txtOtherPayerQuantity.ClientID %>").first().val();
        var hdnClaimId = $("#<%= hdnClaimId.ClientID %>").first().val();
        var hdnPlanId = $("#<%= hdnPlanId.ClientID %>").first().val();
        var hdnAdjustgrp = $("#<%= hdnAdjustgrp.ClientID %>").first().val();
        var hdnreasoncode = $("#<%= hdnreasoncode.ClientID %>").first().val();
        var Claims_Header_Other_Payer_Adjustment_Information_ID = $("#<%= hdnClaims_Header_Other_Payer_Adjustment_Information_ID.ClientID %>").val();
        var lblErrorMessageHeaderOtherPayerDental = $("#<%= lblErrorMessageHeaderOtherPayerDental.ClientID %>").text();
        validateFields();
        if (validateField === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "PUT",
                url: webApiClaims + "UpdateHeaderAdjustmentOtherPayerInfo_Institutional?hdnClaimId=" + hdnClaimId + "&&lblErrorMessageHeaderOtherAdjustment=" + lblErrorMessageHeaderOtherPayerDental + "&&Claims_Header_Other_Payer_Adjustment_Information_ID=" + Claims_Header_Other_Payer_Adjustment_Information_ID + "&&ddlOtherPayerHealthPlanID=" + ddlOtherPayerHealthPlanID + "&&ddlOtherPayerAdjustmentGroup=" + ddlOtherPayerAdjustmentGroup + "&&txtOtherPayerReasonCode=" + txtOtherPayerReasonCode + "&&txtOtherPayerAmount=" + txtOtherPayerAmount + "&&txtOtherPayerQuantity=" + txtOtherPayerQuantity + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                //data: '{hdnClaimId: "' + hdnClaimId + '",lblErrorMessageHeaderOtherAdjustment:"' + lblErrorMessageHeaderOtherPayerDental + '" , Claims_Header_Other_Payer_Adjustment_Information_ID: "' + Claims_Header_Other_Payer_Adjustment_Information_ID + '" , ddlOtherPayerHealthPlanID: "' + ddlOtherPayerHealthPlanID + '" , ddlOtherPayerAdjustmentGroup: "' + ddlOtherPayerAdjustmentGroup + '" , txtOtherPayerReasonCode: "' + txtOtherPayerReasonCode + '" , txtOtherPayerAmount: "' + txtOtherPayerAmount + '" , txtOtherPayerQuantity: "' + txtOtherPayerQuantity + '"  }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimHeaderOtherAdjustmentInstiList = result;
                    if (result[0].lblErrorMessageHeaderOtherAdjustment) {
                        if ((hdnPlanId != ddlOtherPayerHealthPlanID) && (ddlOtherPayerAdjustmentGroup != hdnAdjustgrp) && (hdnreasoncode != txtOtherPayerReasonCode)) {
                            $("[id*=lblErrorMessageHeaderOtherPayerDental]").text(result[0].lblErrorMessageHeaderOtherAdjustment);
                        }
                        else {
                            HeaderAdjInstiClearFields();
                            document.getElementById('<%= btnOtherPayerAdjustmentInfoAdd.ClientID%>').style.visibility = "visible";
                            document.getElementById('<%= btnOtherPayerAdjustmentInfoEdit.ClientID%>').style.visibility = "hidden";
                            document.getElementById('<%= btnCancelUpdate.ClientID%>').style.visibility = "hidden"; }
                    }
                    else {
                        if (claimHeaderOtherAdjustmentInstiList.length > 0) {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:10px;' scope='col'>Reason Code</th><th style='width:10px;' scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                            for (var i = 0; i < claimHeaderOtherAdjustmentInstiList.length; i++) {
                                var HealthPlanID = result[i].cde_health_plan_id;
                                var AdjustmentGroup = result[i].cde_adjustment_group;
                                var ReasonCode = result[i].cde_reason_code;
                                var PayerAmount = result[i].cde_amount;
                                var PayerQuantity = result[i].cde_quantity;
                                var Claim_ID = result[i].Claim_ID;
                                var Claims_Header_Other_Payer_Adjustment_Information_ID = result[i].Claims_Header_Other_Payer_Adjustment_Information_ID;
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + HealthPlanID + "</span></td><td><span title='Line' class='tNumber'>" + AdjustmentGroup + "</span></td><td><span  title='Line' class='tNumber'>" + ReasonCode + "</span></td><td><span title='Line' class='tNumber'>" + PayerAmount + "</span></td><td>" + PayerQuantity + "</td><td><input type='button' value = 'Edit' onClick = 'return EditFieldsHeaderAdjustmentInstitutional(\"" + Claim_ID + "\",\"" + Claims_Header_Other_Payer_Adjustment_Information_ID + "\",this);return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteHeaderAdjustmentOtherPayerInfo_Institutional(\"" + Claim_ID + "\",\"" + Claims_Header_Other_Payer_Adjustment_Information_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            };
                            table = table + "</tbody></table>";
                            localStorage.setItem("HeaderAdjustmentTable", "" + table + "");
                            document.getElementById('<%= providerHeaderAdjustmentdentalOutput.ClientID %>').innerHTML = table;
                            HeaderAdjInstiClearFields();
                            document.getElementById('<%= btnOtherPayerAdjustmentInfoAdd.ClientID%>').style.visibility = "visible";
                            document.getElementById('<%= btnOtherPayerAdjustmentInfoEdit.ClientID%>').style.visibility = "hidden";
                            document.getElementById('<%= btnCancelUpdate.ClientID%>').style.visibility = "hidden";
                            return false;
                        }
                        else {
                            HeaderAdjInstiClearFields();
                            return false;
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('Data not found.');
                }
            });
        }
            return false;
            function validateFields() {
                if ((ddlOtherPayerHealthPlanID === null || ddlOtherPayerHealthPlanID === "" || ddlOtherPayerHealthPlanID === undefined)) {
                    validateField = "false";
                    $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('Health plan ID is required');
                    return false;
                }
                else {
                    validateField = "true";
                    $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('');
                }
                if ((ddlOtherPayerAdjustmentGroup === null || ddlOtherPayerAdjustmentGroup === "" || ddlOtherPayerAdjustmentGroup === undefined)) {
                    validateField = "false";
                    $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('Adjustment group is required');
                    return false;
                }
                else {
                    validateField = "true";
                    $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('');
                }
                if ((txtOtherPayerReasonCode === null || txtOtherPayerReasonCode === "" || txtOtherPayerReasonCode === undefined)) {
                    validateField = "false";
                    $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('Reason code is required');
                    return false;
                }
                else {
                    validateField = "true";
                    $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('');
                }
                if ((txtOtherPayerAmount === null || txtOtherPayerAmount === "" || txtOtherPayerAmount === undefined)) {
                    validateField = "false";
                    $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('Amount is required');
                    return false;
                }
                else {
                    validateField = "true";
                    $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('');
                }
                if ((txtOtherPayerQuantity !== null || txtOtherPayerQuantity !== "" || txtOtherPayerAmount !== undefined)) {
                    if (txtOtherPayerQuantity.charAt(0) === "0") {
                        validateField = "false";
                        $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('Invalid Quantity');
                        return false;
                    }
                }
                else {
                    validateField = "true";
                    $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('');
                }
            }
    }
    function EditFieldsHeaderAdjustmentInstitutional(claim_id, Claims_Header_Other_Payer_Adjustment_Information_ID, control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        $("[id*=lblErrorMessageHeaderOtherAdjustment]").text('');
        $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('');
        var claimHeaderOtherAdjustmentInstiList;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetEdittedFieldsHeaderAdjustmentInstitutional?claim_id=" + claim_id + "&&Claims_Header_Other_Payer_Adjustment_Information_ID=" + Claims_Header_Other_Payer_Adjustment_Information_ID+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                claimHeaderOtherAdjustmentInstiList = result;
                if (claimHeaderOtherAdjustmentInstiList.length > 0) {
                   
                    for (var i = 0; i < claimHeaderOtherAdjustmentInstiList.length; i++) {
                        var HealthPlanID = result[i].cde_health_plan_id;
                        var AdjustmentGroup = result[i].cde_adjustment_group;
                        var ReasonCode = result[i].cde_reason_code;
                        var PayerAmount = result[i].cde_amount;
                        var PayerQuantity = result[i].cde_quantity;
                        var Claim_ID = result[i].Claim_ID;
                        var Claims_Header_Other_Payer_Adjustment_Information_ID = result[i].Claims_Header_Other_Payer_Adjustment_Information_ID;
                        
                        $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerHealthPlanID option:contains("' + HealthPlanID + '")').prop('selected', true);

                        ReloadAdjustmentGrouponEditClick(Claim_ID, HealthPlanID, AdjustmentGroup);
                        $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerHealthPlanID option:contains("' + AdjustmentGroup + '")').prop('selected', true);
                                                       
                        $("[id*=txtOtherPayerReasonCode]").val("" + ReasonCode + "");
                        $("[id*=hdnPlanId]").val("" + HealthPlanID + "");
                        $("[id*=hdnAdjustgrp]").val("" + AdjustmentGroup + "");
                        $("[id*=hdnreasoncode]").val("" + ReasonCode + "");

                        $("[id*=txtOtherPayerAmount]").val("" + PayerAmount + "");
                        $("[id*=txtOtherPayerQuantity]").val("" + PayerQuantity + "");
                        $("[id*=hdnClaimId]").val("" + Claim_ID + "");
                        $("[id*=hdnClaims_Header_Other_Payer_Adjustment_Information_ID]").val("" + Claims_Header_Other_Payer_Adjustment_Information_ID + "");
                        document.getElementById('<%= btnOtherPayerAdjustmentInfoAdd.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnOtherPayerAdjustmentInfoEdit.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnCancelUpdate.ClientID%>').style.visibility = "visible";
                        return false;
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('try again..');
            }
        });
    }
    function DisplayInstitutionalHeaderOtherAdjustment() {
        var displayTabInstiHeadAdj = localStorage.getItem("HeaderAdjustmentTable");
        if (displayTabInstiHeadAdj != null && displayTabInstiHeadAdj != undefined && displayTabInstiHeadAdj != "") {
            var claimHeaderOtherAdjustmentInstiList;
            var hdnClaimIdInsti = $("#<%= hdnClaimId.ClientID %>").val();
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "DisplayInstitutionalHeaderAdjustmentPanel?hdnClaimId=" + hdnClaimIdInsti+"",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimHeaderOtherAdjustmentInstiList = result;
                    var table;
                    if (claimHeaderOtherAdjustmentInstiList.length > 0) {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:10px;' scope='col'>Reason Code</th><th style='width:10px;' scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        for (var i = 0; i < claimHeaderOtherAdjustmentInstiList.length; i++) {
                            var HealthPlanID = result[i].cde_health_plan_id;
                            var AdjustmentGroup = result[i].cde_adjustment_group;
                            var ReasonCode = result[i].cde_reason_code;
                            var PayerAmount = result[i].cde_amount;
                            var PayerQuantity = result[i].cde_quantity;
                            var Claim_ID = result[i].Claim_ID;
                            var Claims_Header_Other_Payer_Adjustment_Information_ID = result[i].Claims_Header_Other_Payer_Adjustment_Information_ID;
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + HealthPlanID + "</span></td><td><span title='Line' class='tNumber'>" + AdjustmentGroup + "</span></td><td><span  title='Line' class='tNumber'>" + ReasonCode + "</span></td><td><span title='Line' class='tNumber'>" + PayerAmount + "</span></td><td>" + PayerQuantity + "</td><td><input type='button' value = 'Edit' onClick = 'return EditFieldsHeaderAdjustmentInstitutional(\"" + Claim_ID + "\",\"" + Claims_Header_Other_Payer_Adjustment_Information_ID + "\",this);return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteHeaderAdjustmentOtherPayerInfo_Institutional(\"" + Claim_ID + "\",\"" + Claims_Header_Other_Payer_Adjustment_Information_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        };
                        table = table + "</tbody></table>";
                        localStorage.setItem("HeaderAdjustmentTable", "" + table + "");
                        document.getElementById('<%= providerHeaderAdjustmentdentalOutput.ClientID %>').innerHTML = table;
                        HeaderAdjInstiClearFields();
                        return false;
                    }
                    else {
                        HeaderAdjInstiClearFields();
                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('try again..');
                }
            });
        }
    }
    function DeleteHeaderAdjustmentOtherPayerInfo_Institutional(Claim_ID, Claims_Header_Other_Payer_Adjustment_Information_ID) {
        var claimHeaderOtherAdjustmentInstiList;
        var result1 = confirm("Are you sure you want to delete?");
        if (result1) {
            var Claim_ID = $("#<%= hdnClaimId.ClientID %>").val();
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteHeaderAdjustmentOtherPayerInfo_Institutional?Claim_ID=" + Claim_ID + "&&Claims_Header_Other_Payer_Adjustment_Information_ID=" + Claims_Header_Other_Payer_Adjustment_Information_ID,
                //data: '{Claim_ID: "' + Claim_ID + '" , Claims_Header_Other_Payer_Adjustment_Information_ID: "' + Claims_Header_Other_Payer_Adjustment_Information_ID + '" }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimHeaderOtherAdjustmentInstiList = result;
                    if (claimHeaderOtherAdjustmentInstiList.length > 0) {
                        var table;
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:10px;' scope='col'>Reason Code</th><th style='width:10px;' scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        for (var i = 0; i < claimHeaderOtherAdjustmentInstiList.length; i++) {
                            var HealthPlanID = result[i].cde_health_plan_id;
                            var AdjustmentGroup = result[i].cde_adjustment_group;
                            var ReasonCode = result[i].cde_reason_code;
                            var PayerAmount = result[i].cde_amount;
                            var PayerQuantity = result[i].cde_quantity;
                            var Claim_ID = result[i].Claim_ID;
                            var Claims_Header_Other_Payer_Adjustment_Information_ID = result[i].Claims_Header_Other_Payer_Adjustment_Information_ID;
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + HealthPlanID + "</span></td><td><span title='Line' class='tNumber'>" + AdjustmentGroup + "</span></td><td><span  title='Line' class='tNumber'>" + ReasonCode + "</span></td><td><span title='Line' class='tNumber'>" + PayerAmount + "</span></td><td>" + PayerQuantity + "</td><td><input type='button' value = 'Edit' onClick = 'return EditFieldsHeaderAdjustmentInstitutional(\"" + Claim_ID + "\",\"" + Claims_Header_Other_Payer_Adjustment_Information_ID + "\",this);return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteHeaderAdjustmentOtherPayerInfo_Institutional(\"" + Claim_ID + "\",\"" + Claims_Header_Other_Payer_Adjustment_Information_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        };
                        table = table + "</tbody></table>";
                        localStorage.setItem("HeaderAdjustmentTable", "" + table + "");
                        document.getElementById('<%= providerHeaderAdjustmentdentalOutput.ClientID %>').innerHTML = table;
                        HeaderAdjInstiClearFields();
                        BindHeaderOtherAdjustmentGroupDropdown();
                        return false;
                    }
                    else {
                        document.getElementById('<%= providerHeaderAdjustmentdentalOutput.ClientID %>').innerHTML = "";
                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('try again..');
                }
            });
          }
      }
    function HeaderAdjInstiClearFields() {
        //ctl00$MainContent$uc5SubmitClaim$ucSubmitClaimHeaderOtherPayerAdjustmentMapping$ddlOtherPayerHealthPlanID        
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerHealthPlanID").options.length > 0) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerHealthPlanID").options[0].selected = true;
        }
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerAdjustmentGroup").options.length > 0) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerAdjustmentGroup").options[0].selected = true;
        }
        $("#<%= txtOtherPayerReasonCode.ClientID %>").first().val("");
        $("#<%= txtOtherPayerAmount.ClientID %>").first().val("");
        $("#<%= txtOtherPayerQuantity.ClientID %>").first().val("");
        document.getElementById('<%= lblErrorMessageHeaderOtherPayerDental.ClientID%>').innerHTML = '';
        $("#<%= lblErrorMessageHeaderOtherPayerDental.ClientID %>").html("");
        document.getElementById('<%= btnOtherPayerAdjustmentInfoAdd.ClientID%>').style.visibility = "visible";
        document.getElementById('<%= btnOtherPayerAdjustmentInfoEdit.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnCancelUpdate.ClientID%>').style.visibility = "hidden";
        var table = localStorage.getItem("HeaderAdjustmentTable");
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= providerHeaderAdjustmentdentalOutput.ClientID %>').innerHTML = table;
        }
        return false;


    }

    function visibleReasonCode() {
        $("#<%=ucSubmitClaimSearchReason.FindControl("gvSubmitClaimSearchPop").ClientID %>").html("");
        $("#<%=ucSubmitClaimSearchReason.FindControl("txtCode").ClientID %>").val("");
        $("#<%=ucSubmitClaimSearchReason.FindControl("txtPlaceOfServiceName").ClientID %>").val("");
        localStorage.setItem("indexReasoncode", "");
        localStorage.setItem("OtherPayerAdjustmentServiceDet", "false");
        $find("mpeReasonCode1").show();
        return false;

    }
    function GetHeaderAndOtherPayerDetails() {
        var OtherPayerAdjustmentServiceDet = localStorage.getItem("OtherPayerAdjustmentServiceDet");
        if (OtherPayerAdjustmentServiceDet == "true") {
            OtherPayerAdjSerDet();
            return false;
        }
        var txtReasonProcCode = $("#<%= ucSubmitClaimSearchReason.FindControl("txtCode").ClientID %>").first().val();

        var txtReasonProccodedesc = $("#<%= ucSubmitClaimSearchReason.FindControl("txtPlaceOfServiceName").ClientID %>").first().val();

        $("#<%=ucSubmitClaimSearchReason.FindControl("gvSubmitClaimSearchPop").ClientID %>").html("");
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetHeaderAndOtherPayerDetail?desc=" + txtReasonProccodedesc + "&&val=" + txtReasonProcCode,
            //data: '{desc: "' + txtReasonProccodedesc + '" , val: "' + txtReasonProcCode + '" }',
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
                    $("#<%=ucSubmitClaimSearchReason.FindControl("gvSubmitClaimSearchPop").ClientID %>").append("<tr><td> No Records Found </td></tr>");
                 }
                 else {
                     $("#<%=ucSubmitClaimSearchReason.FindControl("gvSubmitClaimSearchPop").ClientID %>").append("<tr><th>Reason Code </th><th>Reason Code Description </th></tr>");
                     for (var i = 0; i < result.length; i++) {                         
                        $("#<%=ucSubmitClaimSearchReason.FindControl("gvSubmitClaimSearchPop").ClientID %>").append("<tr><td><a onClick='GetHeaderAndOtherPayercode(this); return false;'>" + result[i].Reason_Code + "</asp:LinkButton></td><td>" + result[i].Reason_Desc + "</td></tr>");
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblmessage]").text('Reason code not found.');
            }
        });

        return false;
    }
    function GetHeaderAndOtherPayercode(lnk) {
        $find("mpeReasonCode1").hide();

        var gridindexReasoncode = localStorage.getItem("indexReasoncode");

        if (!(gridindexReasoncode == "")) {

            var row = lnk.parentNode.parentNode;
            var displayTabInstiHeadAdj = localStorage.getItem("HeaderAdjustmentTable");
         <%--  var grid = document.getElementById("<%= gvOtherPayerAdjustmentInfo.ClientID%>");--%>
            var inputs = displayTabInstiHeadAdj.rows[gridindexReasoncode].getElementsByTagName("INPUT");
            inputs[2].value = row.cells[0].innerText;

            return false;
        }
        else {
            var textboxrow = lnk.parentNode.parentNode;
            $("[id*=ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_txtOtherPayerReasonCode]").val(textboxrow.cells[0].innerText.trim());

            return false;
        }
    }

    function ddlHeaderOtherPayerHealthPlanID_SelectedIndexChanged() {

        var Claim_ID = $("#<%= hdnClaimId.ClientID %>").first().val();
        var ddlHeaderOtherHealthPlanID = $("#<%=ddlOtherPayerHealthPlanID.ClientID %> option:selected").text();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "ddlHeaderOtherPayerHealthPlanID_SelectedIndexChanged?Claim_ID=" + Claim_ID + "&&ddlHeaderOtherHealthPlanID=" + ddlHeaderOtherHealthPlanID+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var ddAdjustmentCodeResult = result;
                $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerAdjustmentGroup").empty();
                $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerAdjustmentGroup").append($('<option value=""></option>'));
                for (var i = 0; i < ddAdjustmentCodeResult.length; i++) {
                    var newOption = "<option value='" + ddAdjustmentCodeResult[i].cde_adjustment_group + "'>" + ddAdjustmentCodeResult[i].cde_adjustment_group + "</option>";
                    $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerAdjustmentGroup").append(newOption);
                }
                $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMessageHeaderOtherPayerDental]").text('try again later..');
            }
        });
    }

    function BindHeaderOtherAdjustmentGroupDropdown() {
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "BindAdjustmentGroupDropdown",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                data = result;
                $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerAdjustmentGroup").empty();
                $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerAdjustmentGroup").append($('<option value=""></option>'));
                for (var i = 0; i < data.length; i++) {
                    $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerAdjustmentGroup").append($('<option></option>').attr("value", data[i].PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_ID).text(data[i].PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_DESC));
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }

    function ReloadAdjustmentGrouponEditClick(Claim_ID, HealthPlanID, ClickedAdjustmentGroup) {
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "ReloadAdjustmentGrouponEditClick?Claim_ID=" + Claim_ID + "&&HealthPlanID=" + HealthPlanID + "&&ClickedAdjustmentGroup=" + ClickedAdjustmentGroup,
            //data: '{Claim_ID: "' + Claim_ID + '",HealthPlanID:"' + HealthPlanID + '",ClickedAdjustmentGroup:"' + ClickedAdjustmentGroup + '"}',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var FilteredDropdownValues= result;
                $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerAdjustmentGroup").empty();
                $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerAdjustmentGroup").append($('<option value=""></option>'));
                for (var i = 0; i < FilteredDropdownValues.length; i++) {
                    $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerAdjustmentGroup").append($('<option></option>').attr("value", FilteredDropdownValues[i].cde_adjustment_group).text(FilteredDropdownValues[i].cde_adjustment_group));
                }
                $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimHeaderOtherPayerAdjustmentMapping_ddlOtherPayerAdjustmentGroup').find('option').filter(function () {
                    return $(this).text() === ClickedAdjustmentGroup;
                }).first().attr('selected', 'selected');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                
            }
        });
    }

</script>
<asp:Panel ID="pnlOtherPayerAdjustmentInformation" runat="server" style="text-align: center; padding-left: 20px; padding-right: 20px;">
<asp:UpdatePanel ID="upICD10ProcedureCodes" runat="server" UpdateMode="Conditional" >
    <ContentTemplate>
          
        <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                <div class="divGrid" style="padding-right: 20px;">
                                <asp:HiddenField ID="hdnPlanId"  runat="server" />
                                <asp:HiddenField ID="hdnAdjustgrp"  runat="server" />
                                <asp:HiddenField ID="hdnreasoncode"  runat="server" />

                    <div id="providerHeaderAdjustmentdentalOutput" runat="server"></div>
                    <%--<asp:GridView ID="gvOtherPayerAdjustmentInfo" runat="server" AllowSorting="false" CssClass="gridview"
                        DataKeyNames="Claims_Header_Other_Payer_Adjustment_Information_ID"
                        AutoGenerateColumns="false"
                        OnRowEditing="gvOtherPayerAdjustmentInfo_RowEditing"
                        OnRowCancelingEdit="gvOtherPayerAdjustmentInfo_RowCancelingEdit"
                        OnRowUpdating="gvOtherPayerAdjustmentInfo_RowUpdating"
                        OnRowDeleting="gvOtherPayerAdjustmentInfo_RowDeleting"
                        OnRowDataBound="OnRowDataBound"                         
                        HorizontalAlign="Center" Width="100%" 
                        EnableViewState="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Health Plan ID" >
                            <ItemTemplate>
                                <asp:Label ID="lblHealthPlanID" runat="server" Text='<%# Eval("Health_Plan_ID")%>'></asp:Label>
                                <asp:HiddenField ID="hdnOtherPayerAdjustmentInfo" Value='<%# Bind("Claims_Header_Other_Payer_Adjustment_Information_ID") %>' runat="server" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlOtherPayerHealthPlanGVID" runat="server" Style="height: 25px; width: 200px; min-width: 200px;">
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdnOtherPayerAdjustmentInfo" Value='<%# Bind("Claims_Header_Other_Payer_Adjustment_Information_ID") %>' runat="server" />
                                <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="ddlOtherPayerHealthPlanGVID" InitialValue="" Display="Dynamic"
                                 ErrorMessage="Please select Health Plan ID" Text="*select Health Plan ID" CssClass="failureNotification"  ValidationGroup="vgBilledUnits"/>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Adjustment Group">
                            <ItemTemplate>
                                <asp:Label ID="lblOtherPayerAdjustmentGroup" runat="server" Text='<%# Eval("cde_adjustment_group")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="hdnAdjustgrp" Value='<%# Bind("cde_adjustment_group") %>' runat="server" />
                                <asp:DropDownList ID="ddlOtherPayerAdjustmentGroup" runat="server" Style="height: 25px; width: 200px; min-width: 200px;"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfv2" runat="server" ControlToValidate="ddlOtherPayerAdjustmentGroup" InitialValue="" Display="Dynamic"
                                     ErrorMessage="Please select Adjustment Group" Text="*select Adjustment Group" CssClass="failureNotification" ValidationGroup="vgBilledUnits"/>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reason Code" >
                                <ItemTemplate>
                                    <asp:Label ID="lblReasonCode" runat="server" Text='<%# Eval("cde_reason_code")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                <asp:HiddenField ID="hdnreasoncode" Value='<%# Bind("cde_reason_code") %>' runat="server" />
                                   <asp:TextBox ID="txtHeaderreasonCode"  Text='<%# Eval("cde_reason_code")%>' runat="server" MaxLength="5"  CssClass="formFieldTextBoxSmall" Style="height: 30px; width: 100px" />

                                </EditItemTemplate>
                            </asp:TemplateField>
                        <asp:BoundField DataField="cde_amount" HeaderText="Amount" />
                        <asp:BoundField DataField="cde_quantity" HeaderText="Quantity"  ReadOnly="true"/>
                        <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" Text="Edit" runat="server" CommandName="Edit"  CausesValidation="false" CssClass="btn btn-primary" />                                 
                                    <asp:Button ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" CssClass="btn btn-danger" OnClientClick='return confirm("Are you sure you want to delete this record?");' CausesValidation="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button ID="btnUpdate" Text="Update" runat="server" CommandName="Update" CausesValidation="true" ValidationGroup="vgBilledUnits" CssClass="btn btn-primary" />                                   
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger" CausesValidation="false" />
                                </EditItemTemplate>
                               
                            </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>--%>
            </div>
        </div>
        <div runat="server" id="divHead">
             
                    
        <div class="row" style="padding-left: 70px;">
            <div class="col-md-2">
                <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color: red">* </span>Health Plan ID</span>
            </div>
            <div class="col-md-2">
                <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color: red">* </span>Adjustment Group</span>
            </div>
            <div class="col-md-2">
                <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color: red">* </span>Reason Code</span>
                       <asp:Label ID="lblHeaderDentalerror" ForeColor="Red" Style="font-size: 14px;" runat="server" Text=""></asp:Label>

            </div>
            <div class="col-md-3">
                <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style="color: red">* </span>Amount</span>
            </div>
            <div class="col-md-2">
                <span class="ohio-field-label" style="font-size: 17px;font-weight: bold;padding-right: 120px;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Quantity</span>
            </div>
            <div class="col-md-1">
            </div>
        </div>
        <div class="row" style="padding-left: 70px;">
            <div class="col-sm-2">
                <asp:DropDownList ID="ddlOtherPayerHealthPlanID" CssClass="formFieldTextBoxSmall" EnableViewState="true" runat="server"
                    AppendDataBoundItems="True" Style="height: 30px; width: 180px; min-width: 180px;" class="selectdropdown" onChange="return ddlHeaderOtherPayerHealthPlanID_SelectedIndexChanged()">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator runat="server" ID="rfvOtherPayerHealthPlanID" SetFocusOnError="true"
                    ValidationGroup="validateClaimsDental" ControlToValidate="ddlOtherPayerHealthPlanID" CssClass="failureNotification"
                    Text="*Health plan ID is required" Display="Dynamic" InitialValue="" />
            </div>
            <div class="col-sm-2">
                <asp:DropDownList ID="ddlOtherPayerAdjustmentGroup" CssClass="formFieldTextBoxSmall" EnableViewState="true" runat="server"
                    AppendDataBoundItems="true" Style="height: 30px; width: 100px; min-width: 100px;" class="selectdropdown">
                </asp:DropDownList><br />
                <asp:RequiredFieldValidator runat="server" ID="rfvOtherPayerAdjustmentGroup" SetFocusOnError="true"
                    ValidationGroup="validateClaimsDental" ControlToValidate="ddlOtherPayerAdjustmentGroup" CssClass="failureNotification"
                    Text="*Select Adjustment Group" Display="Dynamic" InitialValue="" />
            </div>
            <div class="col-sm-3">
               <asp:TextBox ID="txtOtherPayerReasonCode" runat="server" MaxLength="5" CssClass="formFieldTextBoxSmall" Style="height: 30px; width: 170px" Height="30px" ValidationGroup="validateClaimsDental" OnChange="return DentalReasonCodetextChange()"  />
                    
                <asp:LinkButton ID="lnkOtherPayerReasonSearch" runat="server" ToolTip="Search" Text="Search"  style="font-size:14px;"
                    CommandArgument='<%# Eval("ID_PROVIDER") %>'   Visible="true" OnClientClick="return visibleReasonCode()"></asp:LinkButton>&nbsp;&nbsp;
               <%-- OnClientClick="return visibleDentalReasonCode()"--%>
                <br />
                <asp:RequiredFieldValidator runat="server" ID="rqfOtherPayerReasonCode" SetFocusOnError="true"
                    ValidationGroup="validateClaimsDental" ControlToValidate="txtOtherPayerReasonCode" CssClass="failureNotification"
                    Text="*Enter Reason Code"  Display="Dynamic" InitialValue="" />
               
            </div>
            <div class="col-sm-2">
                <asp:TextBox ID="txtOtherPayerAmount" onkeypress="return onlyDotsAndNumbersWithNegetive(this,event);"
                    runat="server" CssClass="formFieldTextBoxSmall" MaxLength="18" Style="height: 30px; width: 200px;" /><br />
                <asp:RequiredFieldValidator runat="server" ID="rqfOtherPayerAmount" SetFocusOnError="true"
                    ValidationGroup="validateClaimsDental" ControlToValidate="txtOtherPayerAmount" CssClass="failureNotification"
                    Text="*Enter Amount" Display="Dynamic" InitialValue="" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtOtherPayerAmount" ValidationExpression="^-?(0|[1-9]\d*)(\.\d+)?$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
            </div>
            <div class="col-sm-2">
                <asp:TextBox ID="txtOtherPayerQuantity" onkeypress="return onlyDotsAndNumbersheader(this,event);"
                    runat="server" CssClass="formFieldTextBoxSmall" MaxLength="15" Style="height: 30px; width: 200px;" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtOtherPayerQuantity" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
            </div>
            <div class="col-sm-1">
                <asp:Button ID="btnOtherPayerAdjustmentInfoAdd" Text="Add" OnClientClick="return AddButtonDentalHeaderOtherAdjustment();" runat="server" CssClass="btn btn-primary" ValidationGroup="validateClaimsDental" 
                         Font-Bold="True" Width="60px" Height="30px"/>
                     <asp:Button ID="btnOtherPayerAdjustmentInfoEdit" Style="visibility: hidden" Text="Update" OnClientClick="return EditHeaderAdjustmentOtherPayerInfo_Institutional();" runat="server" CssClass="btn btn-primary" Font-Bold="True" ValidationGroup="vgCarePlan" Width="70px" />
                     <asp:Button ID="btnCancelUpdate" Style="visibility: hidden" Text="Cancel" OnClientClick="return HeaderAdjInstiClearFields();" runat="server" CssClass="btn btn-danger" Font-Bold="True" ValidationGroup="vgCarePlan" Width="70px" />
                <button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                     
                </div>
        </div>
        <br />
            <div style="text-align: left;">
            <asp:Label runat="server" CssClass="failureNotification" ID="lblErrorMessageHeaderOtherPayerDental"> </asp:Label>
        </div>
        <asp:HiddenField ID="hdnClaimId" runat="server" />
        <asp:HiddenField runat="server" ID="hdnClaims_Header_Other_Payer_Adjustment_Information_ID" />

</div>
    </ContentTemplate>
</asp:UpdatePanel>
    </asp:Panel>
<ajax:ModalPopupExtender BehaviorID="mpeReasonCode1" ID="mpeSubmitClaimSearchReason" runat="server" PopupControlID="pnlSubmitClaimSearchReason" TargetControlID="ButtonSearchReason" BackgroundCssClass="modalBackground" CancelControlID="btnCloseReason" />
<asp:Panel ID="pnlSubmitClaimSearchReason" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
    <asp:Panel ID="pnlSubmitClaimSearchReasonHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
        <asp:Button runat="server" ID="btnCloseReason" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;" CausesValidation="false" OnClientClick="CloseReasonPopupDental();" />
         <div class="search-Results" style="font-size: 25px; background-color: #4da5c3"><b>Reason Code Search</b></div>
        <div></div>
        <div></div>
        <div class="search-Results">
            <span style="padding-left: 10px; font-size: 17px;">Reason Code</span>
            <span style="padding-left: 105px; font-size: 17px;">Reason Code Description</span>
        </div>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server">
        <div style="text-align: left; padding: 15px" class="container-fluid">
            <div class="row">
                <uc:SubmitClaimSearchReason runat="server" ID="ucSubmitClaimSearchReason" Visible="true" EnableViewState="true" />
            </div>
        </div>

    </asp:Panel>
</asp:Panel>
<asp:Button runat="server" ID="ButtonSearchReason" Style="display: none" Text="ButtonSearchReson" />