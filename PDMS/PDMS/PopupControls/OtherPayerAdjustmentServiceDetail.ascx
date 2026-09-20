<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="PopupControls_OtherPayerAdjustmentServiceDetail" Codebehind="OtherPayerAdjustmentServiceDetail.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchReason.ascx" TagPrefix="uc" TagName="SubmitClaimSearchReason" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">


<script type="text/javascript"> 

    function otherPayerAdjSerDetailBind() {
        var ClaimType = $("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val();
        var Claim_ID = $("#<%= hdnOPAdjustmentClaimID.ClientID %>").first().val();
        var OtherPayerAdjustmentDetails;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "getOtherPayerAdjSerDetailBind?ClaimId=" + Claim_ID + "&&ClaimType=" + ClaimType,
            //data: '{ClaimId: "' + Claim_ID + '",ClaimType: "' + ClaimType + '"  }',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                OtherPayerAdjustmentDetails = result;               
                    var table;
                   
                    if (OtherPayerAdjustmentDetails.length > 0) {
                        if (ClaimType == "1" && serviceline != "") {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px;' scope='col'>Service Line</th><th style='width:10px;' scope='col'>Revenue Code</th><th style='width:10px;' scope='col'>Procedure Code</th><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:30px; scope='col'>Reason Code</th><th style='width:30px; scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        } else {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px;' scope='col'>Service Line</th><th style='width:10px;' scope='col'>Procedure Code</th><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:30px; scope='col'>Reason Code</th><th style='width:30px; scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        }
                    
                            for (var i = 0; i < OtherPayerAdjustmentDetails.length; i++) {

                                var serviceline = result[i].Service_Line;
                                var Procedure_Code = result[i].Procedure_Code;
                                var Health_Plan_ID = result[i].Health_Plan_ID;
                                var Adjustment_Group = result[i].Adjustment_Group;
                                var Reason_Code = result[i].Reason_Code;
                                var Amount = result[i].Amount;
                                var Quantity = result[i].Quantity;
                                var Claim_ID = result[i].Claim_ID;
                                var Revenue_Code = result[i].Revenue_Code;
                                var Other_Payer_Adjustment_Service_Detail_ID = result[i].Other_Payer_Adjustment_Service_Detail_ID;

                                document.getElementById('<%= btnAddOtherPayerAdjustment.ClientID%>').style.visibility = "visible";
                                    if (ClaimType == "1") {
                                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Service Line' class='tNumber'>" + serviceline + "</span></td><td><span title='Reason Code' class='tNumber'>" + Revenue_Code + "</span></td><td><span  title='Procedure Code' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Helath Plan ID' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Adjustment Group' class='tNumber'>" + Adjustment_Group + "</span></td><td><span title='Reason Code' class='tNumber'>" + Reason_Code + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Quntity' class='tNumber'>" + Quantity + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                                    }
                                    else {
                                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Service Line' class='tNumber'>" + serviceline + "</span></td><td><span  title='Procedure Code' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Helath Plan ID' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Adjustment Group' class='tNumber'>" + Adjustment_Group + "</span></td><td><span title='Reason Code' class='tNumber'>" + Reason_Code + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Quntity' class='tNumber'>" + Quantity + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                                    }
                                };
                                table = table + "</tbody></table>";
                                localStorage.setItem("OtherPayerAdjustmentTable", "" + table + "");
                                document.getElementById('<%= divOtherPayerAdjustment.ClientID %>').innerHTML = table;
                                ClearOtherPayerAdjustmetServiceDetail();
                                $("#<%= lblOtherPayerAdjustmenterrormsg.ClientID %>").text("");
                          
                        }
                        else {
                        document.getElementById('<%= divOtherPayerAdjustment.ClientID %>').innerHTML = "";
                     }
                 
             },
             error: function (jqXHR, textStatus, errorThrown) {

             }
         });
    }



    var ValidateAdjustmentDetails = "";
    function ValidationsforOtherPayerAdjustmentSD() {
        /*$("[id*=lblddlServiceLine]").text('');*/
        var serviceline = $("#<%=ddlServiceLine.ClientID %> option:selected").text();
        var ddlOPPAdjustmentHealthPlanID = $("#<%=ddlOPPAdjustmentHealthPlanID.ClientID %> option:selected").text();
        var txtOtherPayerReasonCode = $("#<%= txtOtherPayerReasonCode.ClientID %>").first().val();
        var ddlOPPAdjustmentGroup = $("#<%=ddlOPPAdjustmentGroup.ClientID %> option:selected").text();
        var txtOPPAdjustmentAmount = $("#<%= txtOPPAdjustmentAmount.ClientID %>").first().val();
        if (serviceline.trim() == "" || serviceline.trim() == undefined || serviceline.trim() == null) {
            ValidateAdjustmentDetails = "false";
            $("[id*=lblddlServiceLine]").text('*Service Line Number is required');
            return false;
        }
        else {
            ValidateAdjustmentDetails = "true";
            $("[id*=lblddlServiceLine]").text('');
        }
        if (ddlOPPAdjustmentHealthPlanID.trim() == "" || ddlOPPAdjustmentHealthPlanID.trim() == undefined || ddlOPPAdjustmentHealthPlanID.trim() == null) {
            ValidateAdjustmentDetails = "false";
            $("[id*=lblddlOPPAdjustmentHealthPlanID]").text('*Health Plan ID is required');
            return false;
        }
        else {
            ValidateAdjustmentDetails = "true";
            $("[id*=lblddlOPPAdjustmentHealthPlanID]").text('');
        }
        if (ddlOPPAdjustmentGroup.trim() == "" || ddlOPPAdjustmentGroup.trim() == null || ddlOPPAdjustmentGroup.trim() == undefined) {
            ValidateAdjustmentDetails = "false";
            $("[id*=lblddlOPPAdjustmentGroup]").text('*Adjustment Group is required');
            return false;
        }
        else {
            ValidateAdjustmentDetails = "true";
            $("[id*=lblddlOPPAdjustmentGroup]").text('');
        }
        if (txtOtherPayerReasonCode.trim() == "" || txtOtherPayerReasonCode.trim() == null || txtOtherPayerReasonCode.trim() == undefined) {
            ValidateAdjustmentDetails = "false";
            $("[id*=lbltxtOtherPayerReasonCode]").text('*Enter Reason Code');
            return false;
        }
        else {
            ValidateAdjustmentDetails = "true";
            $("[id*=lbltxtOtherPayerReasonCode]").text('');
        }
        if (txtOPPAdjustmentAmount.trim() == "" || txtOPPAdjustmentAmount.trim() == null || txtOPPAdjustmentAmount.trim() == undefined) {
            ValidateAdjustmentDetails = "false";
            $("[id*=lbltxtOPPAdjustmentAmount]").text('Amount is required');
            return false;
        }
        else {
            ValidateAdjustmentDetails = "true";
            $("[id*=lbltxtOPPAdjustmentAmount]").text('');
        }

    }
    function AddOtherPayerAdjustmentDetails() {
        $("[id*=lblmessage]").text('');
        var OtherPayerAdjustmentDetails;
        var ClaimType = $("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val();
        var serviceline = $("#<%=ddlServiceLine.ClientID %> option:selected").text();
        if (ClaimType == "1") {
            var lblAdjRevenueCode = document.getElementById('<%= lblAdjRevenueCode.ClientID%>').innerHTML;
            var lblAdjProc_Code = document.getElementById('<%= lblAdjProc_Code.ClientID%>').innerHTML;
        } else {
            var lblAdjProc_Code = document.getElementById('<%= lblAdjProc_Code.ClientID%>').innerHTML;
        }
        var ddlOPPAdjustmentHealthPlanID = $("#<%=ddlOPPAdjustmentHealthPlanID.ClientID %> option:selected").text();
        var ddlOPPAdjustmentGroup = $("#<%=ddlOPPAdjustmentGroup.ClientID %> option:selected").text();
        var txtOPPAdjustmentAmount = $("#<%= txtOPPAdjustmentAmount.ClientID %>").first().val();
        var txtOPPAdjustmentQuantity = $("#<%= txtOPPAdjustmentQuantity.ClientID %>").first().val();
        var Claim_ID = $("#<%= hdnOPAdjustmentClaimID.ClientID %>").first().val();
        var txtOtherPayerReasonCode = $("#<%= txtOtherPayerReasonCode.ClientID %>").first().val();
        var lblOtherPayerAdjustmenterrormsg = $("#<%= lblOtherPayerAdjustmenterrormsg.ClientID %>").text();
        ValidationsforOtherPayerAdjustmentSD();
        if (ValidateAdjustmentDetails === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "POST",
                url: webApiClaims + "AddOtherPayerAdjustmentDetails?Claim_ID=" + Claim_ID + "&&serviceline=" + serviceline + "&&lblAdjRevenueCode=" + lblAdjRevenueCode + "&&lblAdjProc_Code=" + lblAdjProc_Code + "&&ddlOPPAdjustmentHealthPlanID=" + ddlOPPAdjustmentHealthPlanID + "&&ddlOPPAdjustmentGroup=" + ddlOPPAdjustmentGroup + "&&txtOtherPayerReasonCode=" + txtOtherPayerReasonCode + "&&txtOPPAdjustmentAmount=" + txtOPPAdjustmentAmount + "&&txtOPPAdjustmentQuantity=" + txtOPPAdjustmentQuantity + "&&ClaimType=" + ClaimType + "&&lblOtherPayerAdjustmenterrormsg=" + lblOtherPayerAdjustmenterrormsg + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    OtherPayerAdjustmentDetails = result;
                    if (result[0].Error_Message) {
                        $("[id*=lblOtherPayerAdjustmenterrormsg]").text(result[0].Error_Message);
                    }
                    else {
                        var table;
                        if (OtherPayerAdjustmentDetails.length > 0) {
                            if (ClaimType == "1" && serviceline != "") {
                                table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px;' scope='col'>Service Line</th><th style='width:10px;' scope='col'>Revenue Code</th><th style='width:10px;' scope='col'>Procedure Code</th><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:30px; scope='col'>Reason Code</th><th style='width:30px; scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                            } else {
                                table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px;' scope='col'>Service Line</th><th style='width:10px;' scope='col'>Procedure Code</th><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:30px; scope='col'>Reason Code</th><th style='width:30px; scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                            }
                            for (var i = 0; i < OtherPayerAdjustmentDetails.length; i++) {
                                var Error = result[i].Error;
                            }
                            if (Error == null || Error == undefined || Error == "") {
                                for (var i = 0; i < OtherPayerAdjustmentDetails.length; i++) {

                                    var serviceline = result[i].Service_Line;
                                    var Procedure_Code = result[i].Procedure_Code;
                                    var Health_Plan_ID = result[i].Health_Plan_ID;
                                    var Adjustment_Group = result[i].Adjustment_Group;
                                    var Reason_Code = result[i].Reason_Code;
                                    var Amount = result[i].Amount;
                                    var Quantity = result[i].Quantity;
                                    var Claim_ID = result[i].Claim_ID;
                                    var Revenue_Code = result[i].Revenue_Code;
                                    var Other_Payer_Adjustment_Service_Detail_ID = result[i].Other_Payer_Adjustment_Service_Detail_ID;

                                    document.getElementById('<%= btnAddOtherPayerAdjustment.ClientID%>').style.visibility = "visible";
                                    if (ClaimType == "1") {
                                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Service Line' class='tNumber'>" + serviceline + "</span></td><td><span title='Reason Code' class='tNumber'>" + Revenue_Code + "</span></td><td><span  title='Procedure Code' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Helath Plan ID' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Adjustment Group' class='tNumber'>" + Adjustment_Group + "</span></td><td><span title='Reason Code' class='tNumber'>" + Reason_Code + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Quntity' class='tNumber'>" + Quantity + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                                    }
                                    else {
                                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Service Line' class='tNumber'>" + serviceline + "</span></td><td><span  title='Procedure Code' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Helath Plan ID' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Adjustment Group' class='tNumber'>" + Adjustment_Group + "</span></td><td><span title='Reason Code' class='tNumber'>" + Reason_Code + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Quntity' class='tNumber'>" + Quantity + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                                    }
                                };
                                table = table + "</tbody></table>";
                                localStorage.setItem("OtherPayerAdjustmentTable", "" + table + "");
                                document.getElementById('<%= divOtherPayerAdjustment.ClientID %>').innerHTML = table;
                                ClearOtherPayerAdjustmetServiceDetail();
                                BindOtherPayerAdjustmentGroupDropdown();
                                $("#<%= lblOtherPayerAdjustmenterrormsg.ClientID %>").text("");
                            }
                        }
                        else {
                            ClearOtherPayerAdjustmetServiceDetail();
                            $("#<%= lblOtherPayerAdjustmenterrormsg.ClientID %>").text("");
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    
                }
            });

        }
        return false;
    }
    function ClearOtherPayerAdjustmetServiceDetail() {
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlServiceLine") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlServiceLine").options[0].selected = true;
        }
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentHealthPlanID") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentHealthPlanID").options[0].selected = true;
        }
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentGroup") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentGroup").options[0].selected = true;
        }
        <%--$("#<%=ddlServiceLine.ClientID %> option:selected").text('');--%>
        <%--$("#<%=ddlOPPAdjustmentHealthPlanID.ClientID %> option:selected").text('');
        $("#<%=ddlOPPAdjustmentGroup.ClientID %> option:selected").text('');--%>
        $("#<%= lblAdjRevenueCode.ClientID %>").html('');
        $("#<%= lblAdjProc_Code.ClientID %>").html('');
        $("#<%= txtOtherPayerReasonCode.ClientID %>").val("");
        $("#<%= txtOPPAdjustmentAmount.ClientID %>").val("");
        $("#<%= txtOPPAdjustmentQuantity.ClientID %>").val("");
        
    }
    function EditOtherPayerAdjustmentInfo(Other_Payer_Adjustment_Service_Detail_ID, Claim_ID, ClaimType, control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        $("#<%= lblOtherPayerAdjustmenterrormsg.ClientID %>").text("");
        var AdjustmentServiceDetailData;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "PUT",
            url: webApiClaims + "EditOtherPayerAdjustmentServiceDetail?Other_Payer_Adjustment_Service_Detail_ID=" + Other_Payer_Adjustment_Service_Detail_ID + "&&Claim_ID=" + Claim_ID + "&&ClaimType=" + ClaimType,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                AdjustmentServiceDetailData = result;
                if (AdjustmentServiceDetailData.length > 0) {
                    for (var i = 0; i < AdjustmentServiceDetailData.length; i++) {
                        var serviceline = result[i].Service_Line;
                        var Procedure_Code = result[i].Procedure_Code;
                        var Health_Plan_ID = result[i].Health_Plan_ID;
                        var Adjustment_Group = result[i].Adjustment_Group;
                        var Reason_Code = result[i].Reason_Code;
                        var Amount = result[i].Amount;
                        var Quantity = result[i].Quantity;
                        var Claim_ID = result[i].Claim_ID;
                        var Revenue_Code = result[i].Revenue_Code;
                        var Other_Payer_Adjustment_Service_Detail_ID = result[i].Other_Payer_Adjustment_Service_Detail_ID;

                        ReloadAdjustmentGrouponOtherPayerAdjustmentEditClick(Claim_ID, serviceline, Health_Plan_ID, Adjustment_Group);
                        $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlServiceLine option:contains("' + serviceline + '")').prop('selected', true);
                        $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentHealthPlanID option:contains("' + Health_Plan_ID + '")').prop('selected', true);
                        $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentGroup option:contains("' + Adjustment_Group + '")').prop('selected', true);

                        $("#<%=ddlServiceLine.ClientID %> option:selected").text(serviceline);
                        
                        if (ClaimType == "1") {
                            $("#<%= lblAdjRevenueCode.ClientID %>").html(Revenue_Code);
                            $("#<%= lblAdjProc_Code.ClientID %>").html(Procedure_Code);
                        }
                        else {
                            $("#<%= lblAdjProc_Code.ClientID %>").html(Procedure_Code);
                        }
                        /*$("[id*=lblAdjProc_Code]").val("" + Procedure_Code + "");*/
                        $("#<%=ddlOPPAdjustmentHealthPlanID.ClientID %> option:selected").text(Health_Plan_ID);
                        $("#<%=ddlOPPAdjustmentGroup.ClientID %> option:selected").text(Adjustment_Group);
                        $("[id*=txtOtherPayerReasonCode]").val("" + Reason_Code + "");
                        $("[id*=txtOPPAdjustmentAmount]").val("" + Amount + "");
                        $("[id*=txtOPPAdjustmentQuantity]").val("" + Quantity + "");
                        $("[id*=hdnOPAdjustmentClaimID]").val("" + Claim_ID + "");
                        $("[id*=hdnOtherPayerAdjustmentServiceDetail_ID]").val("" + Other_Payer_Adjustment_Service_Detail_ID + "");

                        document.getElementById('<%= btnAddOtherPayerAdjustment.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnEditOtherPayerAdjustment.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnCancelOtherPayerAdjustment.ClientID%>').style.visibility = "visible";

                        return false;
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                
            }
        });
        return false;
    }
    function DeleteOtherPayerAdjustmentInfo(Other_Payer_Adjustment_Service_Detail_ID, Claim_ID) {
        var PayerAdjustmentInfo;
        var result = confirm("Are you sure you want to delete?");
        if (result) {
            var ClaimType = $("#<%= hdnOPAdjustmentClaimType.ClientID %>").val();
            var Claim_ID = $("#<%= hdnOPAdjustmentClaimID.ClientID %>").val();
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteOtherPayerAdjustmentServiceDetail?Claim_ID=" + Claim_ID + "&&Other_Payer_Adjustment_Service_Detail_ID=" + Other_Payer_Adjustment_Service_Detail_ID+"",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    PayerAdjustmentInfo = result;
                    var table = "";
                    if (PayerAdjustmentInfo.length > 0) {                 
                        if (ClaimType == "1") {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px;' scope='col'>Service Line</th><th style='width:10px;' scope='col'>Revenue Code</th><th style='width:10px;' scope='col'>Procedure Code</th><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:30px; scope='col'>Reason Code</th><th style='width:30px; scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        } else {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px;' scope='col'>Service Line</th><th style='width:10px;' scope='col'>Procedure Code</th><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:30px; scope='col'>Reason Code</th><th style='width:30px; scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        }
                        for (var i = 0; i < PayerAdjustmentInfo.length; i++) {

                            var serviceline = result[i].Service_Line;
                            var Procedure_Code = result[i].Procedure_Code;
                            var Health_Plan_ID = result[i].Health_Plan_ID;
                            var Adjustment_Group = result[i].Adjustment_Group;
                            var Reason_Code = result[i].Reason_Code;
                            var Amount = result[i].Amount;
                            var Quantity = result[i].Quantity;
                            var Claim_ID = result[i].Claim_ID;
                            var Revenue_Code = result[i].Revenue_Code;
                            var Other_Payer_Adjustment_Service_Detail_ID = result[i].Other_Payer_Adjustment_Service_Detail_ID;
                            if (ClaimType == "1") {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Service Line' class='tNumber'>" + serviceline + "</span></td><td><span title='Revenue Code' class='tNumber'>" + Revenue_Code + "</span></td><td><span  title='Procedure Code' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Helath Plan ID' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Adjustment Group' class='tNumber'>" + Adjustment_Group + "</span></td><td><span title='Reason Code' class='tNumber'>" + Reason_Code + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Quntity' class='tNumber'>" + Quantity + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            } else {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Service Line' class='tNumber'>" + serviceline + "</span></td><td><span  title='Procedure Code' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Helath Plan ID' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Adjustment Group' class='tNumber'>" + Adjustment_Group + "</span></td><td><span title='Reason Code' class='tNumber'>" + Reason_Code + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Quntity' class='tNumber'>" + Quantity + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            }
                        };
                        table = table + "</tbody></table>";
                        localStorage.setItem("OtherPayerAdjustmentTable", "" + table + "");
                        document.getElementById('<%= divOtherPayerAdjustment.ClientID %>').innerHTML = table;
                        
                        ClearOtherPayerAdjustmetServiceDetail();
                        BindOtherPayerAdjustmentGroupDropdown();
                        $("#<%= lblOtherPayerAdjustmenterrormsg.ClientID %>").text("");
                        return false;

                    }
                    else {
                        document.getElementById('<%= divOtherPayerAdjustment.ClientID %>').innerHTML = "";
                        $("#<%= lblOtherPayerAdjustmenterrormsg.ClientID %>").text("");
                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    
                }
            });
        }
    }
    function UpdateOtherPayerAdjustmentServiceDetail() {
        //var ValidateAdjustmentDetails = "";
        $("[id*=lblmessage]").text('');
        var OtherPayerAdjustmentDetails;
        var ClaimType = $("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val();
        var serviceline = $("#<%=ddlServiceLine.ClientID %> option:selected").text();
        if (ClaimType == "1") {
            var lblAdjRevenueCode = document.getElementById('<%= lblAdjRevenueCode.ClientID%>').innerHTML;
            var lblAdjProc_Code = document.getElementById('<%= lblAdjProc_Code.ClientID%>').innerHTML;
        } else {
            var lblAdjProc_Code = document.getElementById('<%= lblAdjProc_Code.ClientID%>').innerHTML;
        }
        var ddlOPPAdjustmentHealthPlanID = $("#<%=ddlOPPAdjustmentHealthPlanID.ClientID %> option:selected").text();
        var ddlOPPAdjustmentGroup = $("#<%=ddlOPPAdjustmentGroup.ClientID %> option:selected").text();
        var txtOPPAdjustmentAmount = $("#<%= txtOPPAdjustmentAmount.ClientID %>").first().val();
        var txtOPPAdjustmentQuantity = $("#<%= txtOPPAdjustmentQuantity.ClientID %>").first().val();
        var Claim_ID = $("#<%= hdnOPAdjustmentClaimID.ClientID %>").first().val();
        var txtOtherPayerReasonCode = $("#<%= txtOtherPayerReasonCode.ClientID %>").first().val();
        var hdnOtherPayerAdjustmentServiceDetail_ID = $("#<%= hdnOtherPayerAdjustmentServiceDetail_ID.ClientID %>").first().val();
        var lblOtherPayerAdjustmenterrormsg = $("#<%= lblOtherPayerAdjustmenterrormsg.ClientID %>").text();
        ValidationsforOtherPayerAdjustmentSD();
        if (ValidateAdjustmentDetails === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "PUT",
                url: webApiClaims + "UpdateOtherPayerAdjustmentServiceDetailPanel?OtherPayerAdjustmentServiceDetail_ID=" + hdnOtherPayerAdjustmentServiceDetail_ID + "&&Claim_ID=" + Claim_ID + "&&serviceline=" + serviceline + "&&lblAdjRevenueCode=" + lblAdjRevenueCode + "&&lblAdjProc_Code=" + lblAdjProc_Code + "&&ddlOPPAdjustmentHealthPlanID=" + ddlOPPAdjustmentHealthPlanID + "&&ddlOPPAdjustmentGroup=" + ddlOPPAdjustmentGroup + "&&txtOtherPayerReasonCode=" + txtOtherPayerReasonCode + "&&txtOPPAdjustmentAmount=" + txtOPPAdjustmentAmount + "&&txtOPPAdjustmentQuantity=" + txtOPPAdjustmentQuantity + "&&ClaimType=" + ClaimType + "&&lblOtherPayerAdjustmenterrormsg=" + lblOtherPayerAdjustmenterrormsg + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    OtherPayerAdjustmentDetails = result;
                    if (result[0].Error_Message) {
                        $("[id*=lblOtherPayerAdjustmenterrormsg]").text(result[0].Error_Message);
                        ClearOtherPayerAdjustmetServiceDetail();
                    }
                    else {
                        var table;
                        if (OtherPayerAdjustmentDetails.length > 0) {
                            if (ClaimType == "1") {
                                table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px;' scope='col'>Service Line</th><th style='width:10px;' scope='col'>Revenue Code</th><th style='width:10px;' scope='col'>Procedure Code</th><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:30px; scope='col'>Reason Code</th><th style='width:30px; scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                            } else {
                                table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px;' scope='col'>Service Line</th><th style='width:10px;' scope='col'>Procedure Code</th><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:30px; scope='col'>Reason Code</th><th style='width:30px; scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                            }
                            for (var i = 0; i < OtherPayerAdjustmentDetails.length; i++) {
                                var Error = result[i].Error;
                            }
                            if (Error == null || Error == undefined || Error == "") {
                                for (var i = 0; i < OtherPayerAdjustmentDetails.length; i++) {

                                    var serviceline = result[i].Service_Line;
                                    var Procedure_Code = result[i].Procedure_Code;
                                    var Health_Plan_ID = result[i].Health_Plan_ID;
                                    var Adjustment_Group = result[i].Adjustment_Group;
                                    var Reason_Code = result[i].Reason_Code;
                                    var Amount = result[i].Amount;
                                    var Quantity = result[i].Quantity;
                                    var Claim_ID = result[i].Claim_ID;
                                    var Revenue_Code = result[i].Revenue_Code;
                                    var Other_Payer_Adjustment_Service_Detail_ID = result[i].Other_Payer_Adjustment_Service_Detail_ID;

                                    document.getElementById('<%= btnAddOtherPayerAdjustment.ClientID%>').style.visibility = "visible";

                                    if (ClaimType == "1") {
                                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Service Line' class='tNumber'>" + serviceline + "</span></td><td><span title='Reason Code' class='tNumber'>" + Revenue_Code + "</span></td><td><span  title='Procedure Code' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Helath Plan ID' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Adjustment Group' class='tNumber'>" + Adjustment_Group + "</span></td><td><span title='Reason Code' class='tNumber'>" + Reason_Code + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Quntity' class='tNumber'>" + Quantity + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                                    }
                                    else {
                                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Service Line' class='tNumber'>" + serviceline + "</span></td><td><span  title='Procedure Code' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Helath Plan ID' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Adjustment Group' class='tNumber'>" + Adjustment_Group + "</span></td><td><span title='Reason Code' class='tNumber'>" + Reason_Code + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Quntity' class='tNumber'>" + Quantity + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                                    }
                                };
                                table = table + "</tbody></table>";
                                document.getElementById('<%= divOtherPayerAdjustment.ClientID %>').innerHTML = table;
                                localStorage.setItem("OtherPayerAdjustmentTable", "" + table + "");
                                ClearOtherPayerAdjustmetServiceDetail();
                                $("#<%= lblOtherPayerAdjustmenterrormsg.ClientID %>").text("");
                            }
                            else {
                                $("#<%= lblOtherPayerAdjustmenterrormsg.ClientID %>").text(Error);
                            <%--document.getElementById('<%= lblErrorMessage.ClientID%>').innerHTML = Error;--%>
                            }
                        }
                        else {
                            ClearOtherPayerAdjustmetServiceDetail();
                        }
                    }
                    
                },
                error: function (jqXHR, textStatus, errorThrown) {

                }

            });
        }
        document.getElementById('<%= btnAddOtherPayerAdjustment.ClientID%>').style.visibility = "visible";
        document.getElementById('<%= btnEditOtherPayerAdjustment.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnCancelOtherPayerAdjustment.ClientID%>').style.visibility = "hidden";
        return false;
    }
    function ddlServiceLine_SelectedIndexChanged() {
        $("[id*=lblddlServiceLine]").text("");
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentHealthPlanID") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentHealthPlanID").options[0].selected = true;
        }
        BindOtherPayerAdjustmentGroupDropdown();
        var hdnOPAdjustmentClaimType = $("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val();
        var hdnOPAdjustmentClaimID = $("#<%= hdnOPAdjustmentClaimID.ClientID %>").first().val();
        var ddlServiceLine = $("#<%=ddlServiceLine.ClientID %> option:selected").text();
        if (ddlServiceLine != null && ddlServiceLine != " " && ddlServiceLine != undefined) {
            if (hdnOPAdjustmentClaimID != "" || hdnOPAdjustmentClaimID != null || hdnOPAdjustmentClaimID != undefined) {
                var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "GET",
                    url: webApiClaims + "ddlServiceLine_SelectedIndexChanged?ServiceLine=" + ddlServiceLine + "&&ClaimType=" + hdnOPAdjustmentClaimType + "&&ClaimId=" + hdnOPAdjustmentClaimID+"",
                    headers: {
                        "Access-Control-Allow-Origin": "*",
                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                        "Authorization": "Bearer " + APIToken
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        if (result.d != "") {
                            $("[id*=lblddlServiceLine]").text('');
                            if (result[0]["cde_proc"] != null) {
                                $("[id*=lblAdjProc_Code]").text(result[0]["cde_proc"]);
                            }
                            else {
                                $("[id*=lblAdjProc_Code]").text("");
                            }

                            if (hdnOPAdjustmentClaimType == "1") {
                                $("[id*=lblAdjRevenueCode]").text(result[0]["Revenue_Code"]);
                            }
                        }
                        else {
                            $("[id*=lblAdjProc_Code]").text("");
                            $("[id*=lblddlServiceLine]").text('*Service Line Number is required');
                        }

                    },
                    error: function (jqXHR, textStatus, errorThrown) {

                    }
                });
            }
        }
        else {
            $("[id*=lblAdjProc_Code]").text(' ');
        }
    }
    function txtOtherPayerReasonCode_TextChanged() {
        $("[id*=lbltxtOtherPayerReasonCode]").text("");
        var txtOtherPayerReasonCode = $("#<%= txtOtherPayerReasonCode.ClientID %>").first().val();
        if (txtOtherPayerReasonCode != "" || txtOtherPayerReasonCode != null || txtOtherPayerReasonCode != undefined) {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "txtOtherPayerReasonCode_TextChanged?OtherPayerReasonCode=" + txtOtherPayerReasonCode+"",
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
                        $("[id*=txtOtherPayerReasonCode]").val('');
                        $("[id*=lbltxtOtherPayerReasonCode]").text("Reason code is invalid");

                        return;
                    } else {
                        $("[id*=lbltxtOtherPayerReasonCode]").text("");
                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {

                }
            });
        }

    }
    function loaderOtherPayerAdjustmentServiceDetails() {

        if ($("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val() == "1") {

            var serviceline = $("#<%=ddlServiceLine.ClientID %> option:selected").text();
            var OPPAdjustmentHealthPlanID = $("#<%=ddlOPPAdjustmentHealthPlanID.ClientID %> option:selected").text();
            var OPPAdjustmentGroup = $("#<%=ddlOPPAdjustmentGroup.ClientID %> option:selected").text();
            var AdjRevenueCode = $("#<%= lblAdjRevenueCode.ClientID %>").text();
            var OtherPayerReasonCode = $("#<%= txtOtherPayerReasonCode.ClientID %>").first().val();
            var OPPAdjustmentAmount = $("#<%= txtOPPAdjustmentAmount.ClientID %>").first().val();
            var OPPAdjustmentQuantity = $("#<%= txtOPPAdjustmentQuantity.ClientID %>").first().val();


            if (serviceline != "" && OPPAdjustmentHealthPlanID != "" && OPPAdjustmentGroup != "" && AdjRevenueCode != "" && OtherPayerReasonCode != "" && OPPAdjustmentAmount != "" && OPPAdjustmentQuantity != "") {

                document.getElementById('<%= ddlServiceLine.ClientID %>').disabled = true;
                document.getElementById('<%= lblAdjRevenueCode.ClientID %>').disabled = true;
                document.getElementById('<%= ddlOPPAdjustmentHealthPlanID.ClientID %>').disabled = true;
                document.getElementById('<%= ddlOPPAdjustmentGroup.ClientID %>').disabled = true;
                document.getElementById('<%= txtOtherPayerReasonCode.ClientID %>').disabled = true;
                document.getElementById('<%= txtOPPAdjustmentAmount.ClientID %>').disabled = true;
                document.getElementById('<%= txtOPPAdjustmentQuantity.ClientID %>').disabled = true;

                document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';
               <%-- document.getElementById('<%= btnOPPAdjustmentAdd.ClientID %>').style.display = 'none';--%>
            }
        }
        if ($("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val() == "0") {
            var serviceline = $("#<%=ddlServiceLine.ClientID %> option:selected").text();
            var OPPAdjustmentHealthPlanID = $("#<%=ddlOPPAdjustmentHealthPlanID.ClientID %> option:selected").text();
            var OPPAdjustmentGroup = $("#<%=ddlOPPAdjustmentGroup.ClientID %> option:selected").text();

            var OtherPayerReasonCode = $("#<%= txtOtherPayerReasonCode.ClientID %>").first().val();
            var OPPAdjustmentAmount = $("#<%= txtOPPAdjustmentAmount.ClientID %>").first().val();
            var OPPAdjustmentQuantity = $("#<%= txtOPPAdjustmentQuantity.ClientID %>").first().val();


            if (serviceline != "" && OPPAdjustmentHealthPlanID != "" && OPPAdjustmentGroup != "" && OtherPayerReasonCode != "" && OPPAdjustmentAmount != "" && OPPAdjustmentQuantity != "") {

                document.getElementById('<%= ddlServiceLine.ClientID %>').disabled = true;

                document.getElementById('<%= ddlOPPAdjustmentHealthPlanID.ClientID %>').disabled = true;
                document.getElementById('<%= ddlOPPAdjustmentGroup.ClientID %>').disabled = true;
                document.getElementById('<%= txtOtherPayerReasonCode.ClientID %>').disabled = true;
                document.getElementById('<%= txtOPPAdjustmentAmount.ClientID %>').disabled = true;
                document.getElementById('<%= txtOPPAdjustmentQuantity.ClientID %>').disabled = true;

                document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';
                <%--document.getElementById('<%= btnOPPAdjustmentAdd.ClientID %>').style.display = 'none';--%>
            }
        }
        if ($("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val() == "2") {
            var serviceline = $("#<%=ddlServiceLine.ClientID %> option:selected").text();
            var OPPAdjustmentHealthPlanID = $("#<%=ddlOPPAdjustmentHealthPlanID.ClientID %> option:selected").text();
            var OPPAdjustmentGroup = $("#<%=ddlOPPAdjustmentGroup.ClientID %> option:selected").text();

            var OtherPayerReasonCode = $("#<%= txtOtherPayerReasonCode.ClientID %>").first().val();
            var OPPAdjustmentAmount = $("#<%= txtOPPAdjustmentAmount.ClientID %>").first().val();
            var OPPAdjustmentQuantity = $("#<%= txtOPPAdjustmentQuantity.ClientID %>").first().val();


            if (serviceline != "" && OPPAdjustmentHealthPlanID != "" && OPPAdjustmentGroup != "" && OtherPayerReasonCode != "" && OPPAdjustmentAmount != "" && OPPAdjustmentQuantity != "") {

                document.getElementById('<%= ddlServiceLine.ClientID %>').disabled = true;

                  document.getElementById('<%= ddlOPPAdjustmentHealthPlanID.ClientID %>').disabled = true;
                  document.getElementById('<%= ddlOPPAdjustmentGroup.ClientID %>').disabled = true;
                document.getElementById('<%= txtOtherPayerReasonCode.ClientID %>').disabled = true;
                document.getElementById('<%= txtOPPAdjustmentAmount.ClientID %>').disabled = true;
                document.getElementById('<%= txtOPPAdjustmentQuantity.ClientID %>').disabled = true;

                document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';
                  document.getElementById('<%= btnAddOtherPayerAdjustment.ClientID %>').style.display = 'none';
              }
          }
    }

    function loadersearch() {

        if ($("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val() == "1") {
            document.getElementById('<%= ddlServiceLine.ClientID %>').disabled = true;
            document.getElementById('<%= lblAdjRevenueCode.ClientID %>').disabled = true;
            document.getElementById('<%= ddlOPPAdjustmentHealthPlanID.ClientID %>').disabled = true;
            document.getElementById('<%= ddlOPPAdjustmentGroup.ClientID %>').disabled = true;
            document.getElementById('<%= txtOtherPayerReasonCode.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentAmount.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentQuantity.ClientID %>').disabled = true;

            <%--document.getElementById('<%= btnloading1.ClientID %>').style.display = 'block';--%>
            document.getElementById('<%= lnkOtherPayerReasonSearch.ClientID %>').style.display = 'none';
        }
        if ($("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val() == "0") {
            document.getElementById('<%= ddlServiceLine.ClientID %>').disabled = true;
         
            document.getElementById('<%= ddlOPPAdjustmentHealthPlanID.ClientID %>').disabled = true;
            document.getElementById('<%= ddlOPPAdjustmentGroup.ClientID %>').disabled = true;
            document.getElementById('<%= txtOtherPayerReasonCode.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentAmount.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentQuantity.ClientID %>').disabled = true;

           <%-- document.getElementById('<%= btnloading1.ClientID %>').style.display = 'block';--%>
            document.getElementById('<%= lnkOtherPayerReasonSearch.ClientID %>').style.display = 'none';
        }
        if ($("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val() == "2") {
            document.getElementById('<%= ddlServiceLine.ClientID %>').disabled = true;

             document.getElementById('<%= ddlOPPAdjustmentHealthPlanID.ClientID %>').disabled = true;
             document.getElementById('<%= ddlOPPAdjustmentGroup.ClientID %>').disabled = true;
             document.getElementById('<%= txtOtherPayerReasonCode.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentAmount.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentQuantity.ClientID %>').disabled = true;

            <%--document.getElementById('<%= btnloading1.ClientID %>').style.display = 'block';--%>
             document.getElementById('<%= lnkOtherPayerReasonSearch.ClientID %>').style.display = 'none';
         }
        }

    function loaderDropDown1() {

        if ($("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val() == "1") {
            document.getElementById('<%= ddlServiceLine.ClientID %>').disabled = true;
            document.getElementById('<%= lblAdjRevenueCode.ClientID %>').disabled = true;
            document.getElementById('<%= ddlOPPAdjustmentHealthPlanID.ClientID %>').disabled = true;
            document.getElementById('<%= ddlOPPAdjustmentGroup.ClientID %>').disabled = true;
            document.getElementById('<%= txtOtherPayerReasonCode.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentAmount.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentQuantity.ClientID %>').disabled = true;

            <%--document.getElementById('<%= btnloading2.ClientID %>').style.display = 'block';--%>

        }
        if ($("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val() == "0") {
            document.getElementById('<%= ddlServiceLine.ClientID %>').disabled = true;

            document.getElementById('<%= ddlOPPAdjustmentHealthPlanID.ClientID %>').disabled = true;
            document.getElementById('<%= ddlOPPAdjustmentGroup.ClientID %>').disabled = true;
            document.getElementById('<%= txtOtherPayerReasonCode.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentAmount.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentQuantity.ClientID %>').disabled = true;

           <%-- document.getElementById('<%= btnloading2.ClientID %>').style.display = 'block';--%>

        }
        if ($("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val() == "2") {
            document.getElementById('<%= ddlServiceLine.ClientID %>').disabled = true;

              document.getElementById('<%= ddlOPPAdjustmentHealthPlanID.ClientID %>').disabled = true;
              document.getElementById('<%= ddlOPPAdjustmentGroup.ClientID %>').disabled = true;
            document.getElementById('<%= txtOtherPayerReasonCode.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentAmount.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentQuantity.ClientID %>').disabled = true;

             <%-- document.getElementById('<%= btnloading2.ClientID %>').style.display = 'block';--%>

          }
    }

    function loaderDropDown2() {

        if ($("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val() == "1") {
            document.getElementById('<%= ddlServiceLine.ClientID %>').disabled = true;
            document.getElementById('<%= lblAdjRevenueCode.ClientID %>').disabled = true;
            document.getElementById('<%= ddlOPPAdjustmentHealthPlanID.ClientID %>').disabled = true;
            document.getElementById('<%= ddlOPPAdjustmentGroup.ClientID %>').disabled = true;
            document.getElementById('<%= txtOtherPayerReasonCode.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentAmount.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentQuantity.ClientID %>').disabled = true;

           <%-- document.getElementById('<%= btnloading3.ClientID %>').style.display = 'block';--%>

        }
        if ($("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val() == "0") {
            document.getElementById('<%= ddlServiceLine.ClientID %>').disabled = true;
          
            document.getElementById('<%= ddlOPPAdjustmentHealthPlanID.ClientID %>').disabled = true;
            document.getElementById('<%= ddlOPPAdjustmentGroup.ClientID %>').disabled = true;
            document.getElementById('<%= txtOtherPayerReasonCode.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentAmount.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentQuantity.ClientID %>').disabled = true;

           <%-- document.getElementById('<%= btnloading3.ClientID %>').style.display = 'block';--%>

        }
        if ($("#<%= hdnOPAdjustmentClaimType.ClientID %>").first().val() == "2") {
            document.getElementById('<%= ddlServiceLine.ClientID %>').disabled = true;

             document.getElementById('<%= ddlOPPAdjustmentHealthPlanID.ClientID %>').disabled = true;
             document.getElementById('<%= ddlOPPAdjustmentGroup.ClientID %>').disabled = true;
            document.getElementById('<%= txtOtherPayerReasonCode.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentAmount.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAdjustmentQuantity.ClientID %>').disabled = true;

           <%--  document.getElementById('<%= btnloading3.ClientID %>').style.display = 'block';--%>

         }
    }


function visibleDentalReasonCode()
{
    localStorage.setItem("OtherPayerAdjustmentServiceDet", "true");
    $find("mpeDentalReasonCodeOther").show();
        return false;   
    }

    function CloseReasonPopup() {
        $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ucSubmitClaimSearchReason_txtCode').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ucSubmitClaimSearchReason_txtPlaceOfServiceName').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ucSubmitClaimSearchReason_gvSubmitClaimSearchPop').find('tbody').html('');
    }
    function OtherDisableEnableConditionAddButton() {
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_btnOPPAdd").prop("disabled", true)
        }, 50);
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_btnOPPAdd").prop('disabled', false);
        }, 1000);
    }

    function OtherPayerAdjSerDet() {
       
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
                        $("#<%=ucSubmitClaimSearchReason.FindControl("gvSubmitClaimSearchPop").ClientID %>").append("<tr><td><a onClick='GetOtherPayerAdjSerDet(this); return false;'>" + result[i].Reason_Code + "</asp:LinkButton></td><td>" + result[i].Reason_Desc + "</td></tr>");
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
               
            }
        });

        return false;
    }

    function GetOtherPayerAdjSerDet(lnk) {
        $find("mpeDentalReasonCodeOther").hide();
        var textboxrow = lnk.parentNode.parentNode;
        $("[id*=ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_txtOtherPayerReasonCode]").val(textboxrow.cells[0].innerText.trim());
        $("[id*=hdnReasonCode12]").val(textboxrow.cells[0].innerText.trim());

        return false;
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
    function CancelOtherPayerAdjustmentServiceDetail() {
        $("#<%=ddlServiceLine.ClientID %>").prop('selectedIndex', 0);
        $("#<%=ddlOPPAdjustmentHealthPlanID.ClientID %>").prop('selectedIndex', 0);
        $("#<%=ddlOPPAdjustmentGroup.ClientID %>").prop('selectedIndex', 0);
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_lblAdjProc_Code").text('');
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_lblAdjRevenueCode").text('');
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_txtOtherPayerReasonCode").val('');
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_txtOPPAdjustmentAmount").val('');
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_txtOPPAdjustmentQuantity").val('');
        document.getElementById('<%= btnAddOtherPayerAdjustment.ClientID%>').style.visibility = "visible";
        document.getElementById('<%= btnEditOtherPayerAdjustment.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnCancelOtherPayerAdjustment.ClientID%>').style.visibility = "hidden";
        $("#<%= lblOtherPayerAdjustmenterrormsg.ClientID %>").text("");
        var table = localStorage.getItem("OtherPayerAdjustmentTable");
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= divOtherPayerAdjustment.ClientID %>').innerHTML = table;
        }
    }

    function BindOtherPayerAdjustmentGroupDropdown() {
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "POST",
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
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentGroup").empty();
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentGroup").append($('<option value=""></option>'));
                for (var i = 0; i < data.length; i++) {
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentGroup").append($('<option></option>').attr("value", data[i].PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_ID).text(data[i].PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_DESC));
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }

    function ddlOtherPayerAdjustmentServiceDetailHealthPlanID_SelectedIndexChanged() {
        var Claim_ID = $("#<%= hdnOPAdjustmentClaimID.ClientID %>").first().val();
        var ddlOPAHealthPlanID = $("#<%=ddlOPPAdjustmentHealthPlanID.ClientID %> option:selected").text();
        var ddlOPAServiceLineNo = $("#<%=ddlServiceLine.ClientID %> option:selected").text();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "ddlOtherPayerAdjustmentServiceDetailHealthPlanID_SelectedIndexChanged?Claim_ID=" + Claim_ID + "&&ddlOPAHealthPlanID=" + ddlOPAHealthPlanID + "&&ddlOPAServiceLineNo=" + ddlOPAServiceLineNo+"",
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
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentGroup").empty();
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentGroup").append($('<option value=""></option>'));
                for (var i = 0; i < ddAdjustmentCodeResult.length; i++) {
                    var newOption = "<option value='" + ddAdjustmentCodeResult[i].Adjustment_Group + "'>" + ddAdjustmentCodeResult[i].Adjustment_Group + "</option>";
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentGroup").append(newOption);
                }
                $("[id*=lblOtherPayerAdjustmenterrormsg]").text('');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblOtherPayerAdjustmenterrormsg]").text('try again later..');
            }
        });
    }

    function ReloadAdjustmentGrouponOtherPayerAdjustmentEditClick(Claim_ID,ServiceLineNo,HealthPlanID,ClickedAdjustmentGroup) {
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "ReloadAdjustmentGrouponOtherPayerAdjustmentEditClick?Claim_ID=" + Claim_ID + "&&ServiceLineNo=" + ServiceLineNo + "&&HealthPlanID=" + HealthPlanID + "&&ClickedAdjustmentGroup=" + ClickedAdjustmentGroup,
            //data: '{Claim_ID: "' + Claim_ID + '",ServiceLineNo:"' + ServiceLineNo + '",HealthPlanID:"' + HealthPlanID + '",ClickedAdjustmentGroup:"' + ClickedAdjustmentGroup + '"}',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var FilteredDropdownValues = result;
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentGroup").empty();
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentGroup").append($('<option value=""></option>'));
                for (var i = 0; i < FilteredDropdownValues.length; i++) {
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentGroup").append($('<option></option>').attr("value", FilteredDropdownValues[i].Adjustment_Group).text(FilteredDropdownValues[i].Adjustment_Group));
                }
                $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlOPPAdjustmentGroup').find('option').filter(function () {
                    return $(this).text() === ClickedAdjustmentGroup;
                }).first().attr('selected', 'selected');
            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });
    }
</script>
<div class="row">
    <div style="text-align: left;padding-left: 6rem;">
            <asp:Label runat="server" CssClass="failureNotification" ID="lblErrorMessage"> </asp:Label>
        </div>
    <div runat="server" id="divOtherPayerAdjustment"></div>
    <%--<div class="divGrid" style="padding-right: 60px;">
        <asp:GridView ID="gvOPPAdjustmentInfo" runat="server" AllowSorting="false" CssClass="gridview"
            AutoGenerateColumns="false"            
            DataKeyNames="Other_Payer_Adjustment_Service_Detail_ID"
            OnRowEditing="gvOPPAdjustmentInfo_RowEditing"
            OnRowCancelingEdit="gvOPPAdjustmentInfo_RowCancelingEdit"
            OnRowUpdating="gvOPPAdjustmentInfo_RowUpdating"
            OnRowDeleting="gvOPPAdjustmentInfo_RowDeleting"
            OnRowDataBound="gvOPPAdjustmentInfo_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Other_Payer_Adjustment_Service_Detail_ID" HeaderText="ID"
                    HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:TemplateField HeaderText="Service Line" HeaderStyle-CssClass="GridviewHeaderAsterisk">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceLine" runat="server" Text='<%# Eval("Service_Line") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlServiceLine" runat="server" Style="height: 25px; width: 200px;
                            min-width: 200px;">
                        </asp:DropDownList>
                                <asp:HiddenField ID="hdnServiceline" Value='<%# Bind("Service_Line") %>' runat="server" />

                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Revenue_Code" HeaderText="Revenue Code" ReadOnly="true"
                    Visible="false" />
                <asp:BoundField DataField="Procedure_Code" HeaderText="Procedure Code" ReadOnly="true" />
                <asp:TemplateField HeaderText="Health Plan ID" HeaderStyle-CssClass="GridviewHeaderAsterisk">
                    <ItemTemplate>
                        <asp:Label ID="lblOPPAdjustmentHealthPlanID" runat="server" Text='<%# Eval("Health_Plan_ID")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlOPPAdjustmentHealthPlanID" runat="server" Style="height: 25px;
                            width: 200px; min-width: 200px;">
                        </asp:DropDownList>
                                <asp:HiddenField ID="hdnHealthId" Value='<%# Bind("Health_Plan_ID") %>' runat="server" />

                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Adjustment Group" HeaderStyle-CssClass="GridviewHeaderAsterisk">
                    <ItemTemplate>
                        <asp:Label ID="lblOPPAdjustmentGroup" runat="server" Text='<%# Eval("Adjustment_Group")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlOPPAdjustmentGroup" runat="server" Style="height: 25px;
                            width: 200px; min-width: 200px;"></asp:DropDownList>
                                <asp:HiddenField ID="hdnAdjustmentGrp" Value='<%# Bind("Adjustment_Group") %>' runat="server" />

                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reason Code" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblReasonCode" runat="server" Text='<%# Eval("Reason_Code")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                <asp:HiddenField ID="hdnReasoncode" Value='<%# Bind("Reason_Code") %>' runat="server" />
                                   <asp:TextBox ID="txtHeaderreasonCode"  Text='<%# Eval("Reason_Code")%>' runat="server" MaxLength="5" Font-Size="Small" Style="height: 25px; width: 100px;
                            min-width: 150px;"/>
                                    <asp:LinkButton ID="gridlnkOtherPayerReasonSearch" runat="server" ToolTip="Search" Text="Search"  style="font-size:14px;width:30px"
                                      OnClientClick="return visibleDentalReasonCode()" Visible="true"></asp:LinkButton>
                                </EditItemTemplate> 
                </asp:TemplateField>
                <asp:BoundField DataField="Amount" HeaderText="Amount" HeaderStyle-CssClass="GridviewHeaderAsterisk" />
                <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                <asp:BoundField DataField="Claim_ID" HeaderText="ClaimID" HeaderStyle-CssClass="hideGridColumn"
                    ItemStyle-CssClass="hideGridColumn" />
                 <%--<asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" Text="Edit " Width="50px" runat="server" CommandName="Edit"  CausesValidation="false" CssClass="btn btn-primary" />                                 
                            <asp:Button ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" CssClass="btn btn-danger" OnClientClick='return confirm("Are you sure you want to delete this record?");' CausesValidation="false" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Button ID="btnUpdate" Text="Update" runat="server" CommandName="Update" CssClass="btn btn-primary" CausesValidation="false"/>                                   
                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger" CausesValidation="false" />
                        </EditItemTemplate>                               
                </asp:TemplateField>--%>
               <%-- <asp:CommandField ButtonType="Link" ShowEditButton="true" CausesValidation="false" ControlStyle-CssClass="btn btn-primary"
                    ControlStyle-ForeColor="White" ControlStyle-BorderStyle="Solid" />
                <asp:CommandField ButtonType="Link" ShowDeleteButton="true" CausesValidation="false" ControlStyle-CssClass="btn btn-danger"
                    ControlStyle-ForeColor="White" ControlStyle-BorderStyle="Solid" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>--%>
</div>
<br />
<div runat="server" id="divOtherPayer">


<div class="row" style="padding-left: 4rem;" >
    <div class="col-md-1">
        <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color:red">*</span>Service
            Line</span>
    </div>
    <div runat="server" id="HeaderOPARevenue">
        <div class="col-md-1">
            <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">Revenue Code</span>
        </div>
    </div>
    <div class="col-md-1">
        <span class="ohio-field-label" style="font-size: 17px; font-weight: bold; padding-left: 10px;">Procedure
            Code</span>
    </div>
    <div class="col-md-2">
        <span class="ohio-field-label" style="font-size: 17px; font-weight: bold; padding-left: 10px;">
            <span style="color:red">*</span>Health Plan ID</span>
    </div>
    <div class="col-md-1">
        <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color:red">*</span>Adjustment
            Group</span>
    </div>
    <div class="col-md-2">
        <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;text-align:center"><span style="color:red">*</span>Reason Code</span>
    </div>
    <div class="col-md-1" style="padding-left:50px">
        <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color:red">*</span>Amount</span>
    </div>
    <div class="col-md-1" style="padding-left:80px">
        <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">Quantity</span>
    </div>
    
</div>

<div class="row" style="padding-left: 4rem;">
    <div class="col-sm-1" style="padding-left:1px;">
        <div style="text-align: left;">
            <asp:DropDownList ID="ddlServiceLine" CssClass="formfieldDate" EnableViewState="true"
                runat="server" onChange="return ddlServiceLine_SelectedIndexChanged(this)" 
                 Style="min-width: 20px; height: 30px" Width="100px" class="selectdropdown">
            </asp:DropDownList>
            <br />
            <%-- <button id="btnloading2" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>--%>
             
            <%--<asp:RequiredFieldValidator runat="server" ID="rfvServiceLine" SetFocusOnError="true"
                ValidationGroup="validateOPAdjustment" ControlToValidate="ddlServiceLine"
                Text="*Service Line Number is required" Display="Dynamic" InitialValue="" CssClass="failureNotification" />--%>
             <asp:Label runat="server" ID="lblddlServiceLine" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div runat="server" id="divAdjRevenueCode">
        <div class="col-sm-1">
            <div style="text-align: left;">
                <asp:Label ID="lblAdjRevenueCode" runat="server" 
                    Style="height: 30px; width: 100px;"/>
            </div>
        </div>
    </div>
    <div class="col-sm-1">
        <div style="text-align: left;">
            <asp:Label ID="lblAdjProc_Code" runat="server"
                Style="height: 30px; width: 100px"/>
        </div>
    </div>
    <div class="col-sm-2">
        <div style="text-align: center;">
            <asp:DropDownList ID="ddlOPPAdjustmentHealthPlanID" CssClass="formfieldDate" EnableViewState="true"
                runat="server"  Style="min-width: 30px;
                height: 30px" Width="130px" class="selectdropdown" onChange="return ddlOtherPayerAdjustmentServiceDetailHealthPlanID_SelectedIndexChanged()">
            </asp:DropDownList>
            <br />
              <%--<button id="btnloading3" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>--%>
             
            <%--<asp:RequiredFieldValidator runat="server" ID="rfvOPPAdjustmentHealthPlanID" SetFocusOnError="true"
                ValidationGroup="validateOPAdjustment" ControlToValidate="ddlOPPAdjustmentHealthPlanID"
                Text="*Health Plan ID is required" Display="Dynamic" InitialValue="" CssClass="failureNotification" />--%>
            <asp:Label runat="server" ID="lblddlOPPAdjustmentHealthPlanID" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div class="col-sm-1" style="padding-left:1px;">
        <div style="text-align: left;">
            <asp:DropDownList ID="ddlOPPAdjustmentGroup" CssClass="formfieldDate" EnableViewState="true"
                runat="server" AppendDataBoundItems="True" Style="min-width: 30px; height: 30px"
                Width="130px" class="selectdropdown"></asp:DropDownList>
            <br />
            <%--<asp:RequiredFieldValidator runat="server" ID="rfvOPPAdjustmentGroup" SetFocusOnError="true"
                ValidationGroup="validateOPAdjustment" ControlToValidate="ddlOPPAdjustmentGroup"
                Text="*Adjustment Group is required" Display="Dynamic" InitialValue="" CssClass="failureNotification" />--%>
            <asp:Label runat="server" ID="lblddlOPPAdjustmentGroup" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div class="col-sm-2" style="padding-left:20px">        
        <div style="text-align: right;width:170px">
            <asp:TextBox ID="txtOtherPayerReasonCode" runat="server" MaxLength="5" CssClass="formfieldDate" Style="height: 30px; width: 90px" onChange="return txtOtherPayerReasonCode_TextChanged(this)" />
            <asp:LinkButton ID="lnkOtherPayerReasonSearch" runat="server" ToolTip="Search" Text="Search"  style="font-size:14px;width:50px"
                    CommandArgument='<%# Eval("ID_PROVIDER") %>'  OnClientClick="return visibleDentalReasonCode()" Visible="true"></asp:LinkButton>
           <%-- <button id="btnloading1" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>--%>
                  
            <br />
            <%--<asp:RequiredFieldValidator runat="server" ID="rfvOPPReasonCode" SetFocusOnError="true"
                    ValidationGroup="validateOPAdjustment" ControlToValidate="txtOtherPayerReasonCode" CssClass="failureNotification"
                    Text="*Enter Reason Code"  Display="Dynamic" InitialValue="" />--%>
              <asp:Label runat="server" ID="lbltxtOtherPayerReasonCode" ForeColor="Red"></asp:Label>
         </div>
    </div>
    <div class="col-sm-1" style="padding-left:1px;">
        <div style="text-align: right;">
            <asp:TextBox ID="txtOPPAdjustmentAmount" runat="server" CssClass="formfieldDate" onkeypress="return onlyDotsAndNumbersWithNegetive(this,event);" Style="height: 30px; 
                width: 100px" />
            <br />
           <%-- <asp:RequiredFieldValidator runat="server" ID="rfvOPPAdjustmentAmount" SetFocusOnError="true"
                ValidationGroup="validateOPAdjustment" ControlToValidate="txtOPPAdjustmentAmount"
                Text="*Amount is required" Display="Dynamic" InitialValue="" CssClass="failureNotification"/>--%>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtOPPAdjustmentAmount" ValidationExpression="^-?(0|[1-9]\d*)(\.\d+)?$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
             <asp:Label runat="server" ID="lbltxtOPPAdjustmentAmount" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div class="col-sm-1" style="padding-left:30px">
        <div style="text-align: left;">
            <asp:TextBox ID="txtOPPAdjustmentQuantity" runat="server" CssClass="formfieldDate" Style="height: 30px;
                width: 100px" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtOPPAdjustmentQuantity" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
        </div>
    </div>
    <div class="col-sm-2" style="padding-left:30px;text-align:center;">
         <button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                   
       <%-- <asp:Button ID="btnOPPAdjustmentAdd"  Text="Add" runat="server" OnClick="btnOPPAdjustmentAdd_Click"
            CssClass="btn btn-primary" Font-Bold="True" ValidationGroup="validateOPAdjustment" CausesValidation="true"
            Width="60px" Height="30px" OnClientClick="loaderOtherPayerAdjustmentServiceDetails()" />--%>

         <asp:Button ID="btnAddOtherPayerAdjustment" Text="ADD" OnClientClick="return AddOtherPayerAdjustmentDetails()" runat="server" CssClass="btn btn-primary" Font-Bold="True" Width="70px" Height="30px" />
        <br /> <asp:Button ID="btnEditOtherPayerAdjustment" OnClientClick="return UpdateOtherPayerAdjustmentServiceDetail()" Style="visibility: hidden" Text="Update" runat="server" CssClass="btn btn-primary" Font-Bold="True"  Width="70px" />
        <br /> <asp:Button ID="btnCancelOtherPayerAdjustment" OnClientClick="return CancelOtherPayerAdjustmentServiceDetail()" Style="visibility: hidden" Text="Cancel" runat="server" CssClass="btn btn-primary" Font-Bold="True"  Width="70px" />
    </div>
</div>
    <div style="padding-left:30px">
        <asp:Label ID="lblOtherPayerAdjustmenterrormsg" ForeColor="Red" Style="font-size:medium" runat="server" Text=""></asp:Label>
    </div>
    </div>

<asp:HiddenField ID="hdnOPAdjustmentClaimID" runat="server" />
<asp:HiddenField ID="hdnOPAdjustmentClaimType" runat="server" />
<asp:HiddenField ID="hdnAddedReasonCodetoGrid" runat="server" />
<asp:HiddenField ID="hdnReasonCode12" runat="server" />
<asp:HiddenField ID="hdnOtherPayerAdjustmentServiceDetail_ID" runat="server" />
<asp:HiddenField ID="hdnOtherPayerAdjustmentClaimStatus" runat="server" />
<ajax:ModalPopupExtender BehaviorID="mpeDentalReasonCodeOther" ID="mpeSubmitClaimSearchReason" runat="server" PopupControlID="pnlSubmitClaimSearchReason" TargetControlID="ButtonSearchReason" BackgroundCssClass="modalBackground" CancelControlID="btnCloseReason" />
<asp:Panel ID="pnlSubmitClaimSearchReason" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
    <asp:Panel ID="pnlSubmitClaimSearchReasonHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
        <asp:Button runat="server" ID="btnCloseReason" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;" CausesValidation="false" OnClientClick="CloseReasonPopup();"/>
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
