<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="PopupControls_ServiceDetailsDental" Codebehind="ServiceDetailsDental.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchProc.ascx" TagPrefix="uc" TagName="SubmitClaimSearchProc" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchPop.ascx" TagPrefix="uc" TagName="SubmitClaimSearchPop" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">

<br />
<script type="text/javascript">
    $(document).ready(function () {
        var clmstatus = $("#ctl00_MainContent_uc5SubmitClaim_txtstatus2").val();
        if (clmstatus != null && clmstatus != undefined && clmstatus != "")
        {
            $("#<%= lblStatusServiceDetails.ClientID %>").html(clmstatus);
        } 
        $("#<%=ddlDiagnosisDentalFirst.ClientID %>").attr("disabled", true);
        $("#<%=ddlDignosisDentalSecond.ClientID %>").attr("disabled", true);
        $("#<%=ddlDignosisDentalThird.ClientID %>").attr("disabled", true);
        $("#<%=ddlDignosisDentalFourth.ClientID %>").attr("disabled", true);
       
    });
   
    function displayDentalServiceDetailTable(copied_Service_Line = 0, control) {
        var claimStatus = $("#ctl00_MainContent_uc5SubmitClaim_txtstatus2").val();
        var DentalServiceLineclaimstatus = document.getElementById("<%=hdnDentalServiecLineClaimStatus.ClientID %>").value;
        if (DentalServiceLineclaimstatus == "Pending Submission") {
            var table = localStorage.getItem("dentalServiceDetailTable");
        }
        else if (DentalServiceLineclaimstatus == "Other") {
            var table = localStorage.removeItem("dentalServiceDetailTable");
        }

        var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "getDentalServiceDetails?claimid=" + claimid,
            //data: '{claimid: "' + claimid + '" }',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                claimServiceDetailsDentalList = result;
                var table;
                var totalcharge = 0;
                var rowcount = 0;
                var totalpaidamount = 0;

                $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothServiceLine").empty();
                $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlAdditonalProviderDetail").empty();
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlDetailId").empty();
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlServiceLine").empty();

                $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothServiceLine").append($("<option> </option>"));
                $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlAdditonalProviderDetail").append($("<option> </option>"));
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlDetailId").append($("<option> </option>"));
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlServiceLine").append($("<option> </option>"));

                if (claimServiceDetailsDentalList.length > 0) {
                    var FirstDateOfService = "";
                    var LastDateOfService = "";
                    table = "<table class=\"gridview\"  cellspacing=\"0\" align=\"Middle\" rules=\"rows\" border=\"1\" style=\"margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;\"><tbody><tr class=\"gridViewHeader\"><th style=\"width:10px; scope=\"col\">Service Line</th><th style=\"width:10px; scope=\"col\">*Procedure Code</th><th style=\"width:10px; scope=\"col\">Place Of Service</th><th style=\"width:10px; scope=\"col\">*Billed Units</th><th style=\"width:10px; scope=\"col\">Paid Units</th><th style=\"width:10px; scope=\"col\">Date Of Service</th><th style=\"width:10px; scope=\"col\">Charges</th><th style=\"width:10px; scope=\"col\">Paid Amount</th><th style=\"width:10px; scope=\"col\">Status</th><th style=\"width:10px;\" scope=\"col\">&nbsp;</th><th style=\"width:10px;\" scope=\"col\">&nbsp;</th><th style=\"width:10px;\" scope=\"col\">&nbsp;</th></tr>";
                    var j = 0;
                    for (var i = 0; i < claimServiceDetailsDentalList.length; i++) {
                        var sequence = i;
                        sequence++;
                        rowcount++;
                        document.getElementById('<%=hdnServiceLines.ClientID %>').value = rowcount;
                        var cde_proc = result[i].cde_proc;
                        var plc_service = result[i].plc_service;
                        if (plc_service == "0") {
                            plc_service = "";
                        }
                        var bil_unt = result[i].bil_unt;
                        var pad_unt = result[i].pad_unt;
                        var ServiceDate = new Date(result[i].ServiceDate);
                        ServiceDate = formatDate(ServiceDate);

                        if (FirstDateOfService == "") {
                            FirstDateOfService = ServiceDate;
                        } else if (ServiceDate < FirstDateOfService) {
                            FirstDateOfService = ServiceDate;
                        }

                        if (LastDateOfService == "") {
                            LastDateOfService = ServiceDate;
                        } else if (ServiceDate > LastDateOfService) {
                            LastDateOfService = ServiceDate;
                        }

                        var cde_clm_chrge = result[i].cde_clm_chrge;
                        var cde_clm_chrge1 = cde_clm_chrge;
                        cde_clm_chrge = HandleDecimalValue(cde_clm_chrge).toString();
                        if (cde_clm_chrge == null || cde_clm_chrge == "" || cde_clm_chrge == undefined) {
                            if (cde_clm_chrge1 == "0" || cde_clm_chrge1 == "0.0" || cde_clm_chrge1 == "0.00" || cde_clm_chrge1 == "0.000") {
                                cde_clm_chrge = "0.00";
                            }
                        }

                        var pad_amnt = result[i].pad_amnt;
                        pad_amnt = HandleDecimalValue(pad_amnt).toString();
                        var cde_clm_status = claimStatus;
                        var Claim_service_Id = result[i].Claim_service_Id;
                        var Claim_Id = result[i].Claim_Id;
                        var serviceline = result[i].service_Line;
                        totalcharge = totalcharge + parseFloat(cde_clm_chrge);
                        totalpaidamount = totalpaidamount + parseFloat(pad_amnt);
                        table = table + "<tr style=\"border: 1px solid black; border-collapse: collapse;\" class=\"gridViewRow\"><td><span title=\"Line\" class=\"tNumber\">" + sequence + "</span></td><td><span title=\"Line\" class=\"tNumber\">" + cde_proc + "</span></td><td><span  title=\"Line\" class=\"tNumber\">" + plc_service + "</span></td><td><span title=\"Line\" class=\"tNumber\">" + bil_unt + "</span></td><td>" + pad_unt + "</td><td>" + ServiceDate + "</td><td>" + cde_clm_chrge + "</td><td>" + pad_amnt + "</td><td>" + cde_clm_status + "</td><td><input type=\"button\" value = \"Edit\" onClick = \"return EditDentalServiceLineItem('" + Claim_service_Id + "','" + Claim_Id + "','" + serviceline + "',this); return true;\" class=\"btn btn-primary\" sytle = \"margin-left:3px\"></td><td><input type=\"button\" value = \"Copy\" onClick = \"return EditDentalServiceLineItem('" + Claim_service_Id + "','" + Claim_Id + "','" + serviceline + "',this, 1); return true;\" class=\"btn btn-primary\" sytle = \"margin-left:3px\"></td><td><input type=\"button\" value=\"Delete\" onclick=\"return DeleteDentalServiceDetailLineitem('" + Claim_Id + "','" + serviceline + "');\" class=\"btn btn-danger\" sytle=\"margin-left:3px\"></td></tr >";
                        j = sequence;
                        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothServiceLine").append($("<option></option>").val(serviceline).html(serviceline));
                        $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlAdditonalProviderDetail").append($("<option></option>").val(serviceline).html(serviceline));
                        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlDetailId").append($("<option></option>").val(serviceline).html(serviceline));
                        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlServiceLine").append($("<option></option>").val(serviceline).html(serviceline));
                    }

                    j++;
                    $("#<%= lblDetailsItemDental.ClientID %>").html("" + j + "");

                    table = table + "</tbody></table>";
                    localStorage.setItem("dentalServiceDetailTable", "" + table + "");
                    document.getElementById('<%= providerDentalServicedetailOutput.ClientID %>').innerHTML = table;
                    clearDentalServiceDetailFields();

                    if (rowcount >= 50) {
                        if (document.getElementById('<%= btnServiceDetailsDentailAdd.ClientID%>') != null) {
                            document.getElementById('<%= btnServiceDetailsDentailAdd.ClientID%>').style.visibility = "hidden";
                        }
                        if (document.getElementById('<%= btnUpdateServiceDetailDental.ClientID%>') != null) {
                            document.getElementById('<%= btnUpdateServiceDetailDental.ClientID%>').style.visibility = "hidden";
                        }

                    } else {
                        if (document.getElementById('<%= btnServiceDetailsDentailAdd.ClientID%>') != null) {
                            document.getElementById('<%= btnServiceDetailsDentailAdd.ClientID%>').style.visibility = "visible";
                        }
                        if (document.getElementById('<%= btnUpdateServiceDetailDental.ClientID%>') != null) {
                            document.getElementById('<%= btnUpdateServiceDetailDental.ClientID%>').style.visibility = "hidden";
                        }
                    }
                    if (document.getElementById('<%= btnCancelDental.ClientID%>') != null) {
                        document.getElementById('<%= btnCancelDental.ClientID%>').style.visibility = "visible";
                    }

                    if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_lblServiceDateInformation') != null) {
                        if (FirstDateOfService != LastDateOfService) {
                            document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_lblServiceDateInformation').innerText = FirstDateOfService + ' - ' + LastDateOfService;
                        } else {
                            document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_lblServiceDateInformation').innerText = FirstDateOfService;
                        }
                    }

                    if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_hdnStartDate') != null) {
                        document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_hdnStartDate').value = FirstDateOfService;
                    }

                    if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_hdnEndDate') != null) {
                        document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_hdnEndDate').value = LastDateOfService;
                    }
                    if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_hdnDateOfService') != null) {
                        if (FirstDateOfService != LastDateOfService) {
                            document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_hdnDateOfService').value = FirstDateOfService;
                        } else {
                            document.getElementById('ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_hdnDateOfService').value = FirstDateOfService + ' - ' + LastDateOfService;
                        }
                    }

                    $("#<%= lblDenTotalCharges.ClientID %>").html("" + totalcharge.toFixed(2) + "");
                    if (isNaN(totalpaidamount)) {
                        $("#<%= lblDenTotalAmountPaid.ClientID %>").html("" + 0.00 + "");

                    }
                    else {
                        $("#<%= lblDenTotalAmountPaid.ClientID %>").html("" + totalpaidamount + "");
                    }
                }
                else {
                    document.getElementById('<%= providerDentalServicedetailOutput.ClientID %>').innerHTML = "";
                    $("#<%= lblDenTotalAmountPaid.ClientID %>").html("" + 0.00 + "");
                    $("#<%= lblDenTotalCharges.ClientID %>").html("" + 0.00 + "");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblmessage]").text('Error Populating Dental Service Details Table');
            }
        });

        bindtoothsurfacedata();
        bindAdditionalProviderInformationserviceDetail();
        GetOtherPayerPaidAmountBind();
        otherPayerAdjSerDetailBind();
    }

    function functionclear()
    {
       
        document.getElementById('<%= txtProducerCodeDental.ClientID %>').disabled = true;
        document.getElementById('<%= lnkProducerCodeDental.ClientID %>').disabled = true;
        document.getElementById('<%= txtdateofserviceDental.ClientID %>').disabled = true;
        document.getElementById('<%= txtLineControllerNoDental.ClientID %>').disabled = true;
        document.getElementById('<%= txtPriorAuthNumberDental.ClientID %>').disabled = true;
        document.getElementById('<%= txtReferralDental.ClientID %>').disabled = true;
        document.getElementById('<%= txtplaceofserviceDental.ClientID %>').disabled = true;
        document.getElementById('<%= txtModifierDentalFirst.ClientID %>').disabled = true;
        document.getElementById('<%= txtModifierDentalSecond.ClientID %>').disabled = true;
        document.getElementById('<%= txtModifierDentalThird.ClientID %>').disabled = true;
        document.getElementById('<%= txtModifierDentalFourth.ClientID %>').disabled = true;
        document.getElementById('<%= ddlDiagnosisDentalFirst.ClientID %>').disabled = true;
        document.getElementById('<%= ddlDignosisDentalSecond.ClientID %>').disabled = true;
        document.getElementById('<%= ddlDignosisDentalThird.ClientID %>').disabled = true;
        document.getElementById('<%= ddlDignosisDentalFourth.ClientID %>').disabled = true;
        document.getElementById('<%= ddlOralCavityDentalFirst.ClientID %>').disabled = true;
        document.getElementById('<%= ddlOralCavityDentalSecond.ClientID %>').disabled = true;
        document.getElementById('<%= ddlOralCavityDentalThird.ClientID %>').disabled = true;
        document.getElementById('<%= ddlOralCavityDentalFourth.ClientID %>').disabled = true;
        document.getElementById('<%= ddlOralCavityDentalFifth.ClientID %>').disabled = true;
        document.getElementById('<%= ddlInlyneCodeDental.ClientID %>').disabled = true;
        document.getElementById('<%= txtChargesDental.ClientID %>').disabled = true;
        document.getElementById('<%= txtBillUnitDental.ClientID %>').disabled = true;
    }

    function HandleDecimalValue(value) {
        var number = value;
        number = number.replace(/^0+/, '');
        dec_value = number.split('.');
        if (dec_value[1] == null || dec_value[1] == undefined) {
            return number;
        }
        else {
            dec_value[1] = dec_value[1].slice(0, 2);
            if (dec_value[1].length > 1) {
                if (dec_value[1] == "00") {
                    number = dec_value[0];
                    return number;
                }
                else {
                    number = dec_value[0] + "." + dec_value[1];
                    return number;
                }
            }
            else {
                if (dec_value[1] == "0") {
                    number = dec_value[0];
                    return number;
                }
                else {

                }
                number = dec_value[0] + "." + dec_value[1];
                return number;
            }
        }
    }

    function validateFields() {
        var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
        var procedureCode = $("#<%= txtProducerCodeDental.ClientID %>").first().val();
        var placeOfService = $("#<%= txtplaceofserviceDental.ClientID %>").first().val();
        var dateOfService = $("#<%= txtdateofserviceDental.ClientID %>").first().val();
        var charges = $("#<%= txtChargesDental.ClientID %>").first().val();
        charges = HandleDecimalValue(charges).toString();
        if (charges == null || charges == "" || charges == undefined) {
            if (storedcharge == "0" || storedcharge == "0.0" || storedcharge == "0.00" || storedcharge == "0.000") {
                charges = "0.00";
            }
        }
        var billedUnits = $("#<%= txtBillUnitDental.ClientID %>").first().val();

        var lineControlNumber = $("#<%= txtLineControllerNoDental.ClientID %>").first().val();
        var priorAuthNumber = $("#<%= txtPriorAuthNumberDental.ClientID %>").first().val();
        var referralNumber = $("#<%= txtReferralDental.ClientID %>").first().val();
        var ProsthesisCode = $("#<%=ddlInlyneCodeDental.ClientID %> option:selected").text();
        var oralCavityDentalFirst = $("#<%=ddlOralCavityDentalFirst.ClientID %> option:selected").text();
        var oralCavityDentalSecond = $("#<%=ddlOralCavityDentalSecond.ClientID %> option:selected").text();
        var oralCavityDentalThird = $("#<%=ddlOralCavityDentalThird.ClientID %> option:selected").text();
        var oralCavityDentalFourth = $("#<%=ddlOralCavityDentalFourth.ClientID %> option:selected").text();
        var oralCavityDentalFifth = $("#<%=ddlOralCavityDentalFifth.ClientID %> option:selected").text();
        var diagnosisPointerFirst = $("#<%=ddlDiagnosisDentalFirst.ClientID %> option:selected").text();
        var diagnosisPointerSecond = $("#<%=ddlDignosisDentalSecond.ClientID %> option:selected").text();
        var diagnosisPointerThird = $("#<%=ddlDignosisDentalThird.ClientID %> option:selected").text();
        var diagnosisPointerFourth = $("#<%=ddlDignosisDentalFourth.ClientID %> option:selected").text();
        var modifierDentalFirst = $("#<%= txtModifierDentalFirst.ClientID %>").first().val(); 
        var modifierDentalSecond = $("#<%= txtModifierDentalSecond.ClientID %>").first().val();
        var modifierDentalThird = $("#<%= txtModifierDentalThird.ClientID %>").first().val();
        var modifierDentalFourth = $("#<%= txtModifierDentalFourth.ClientID %>").first().val();
        var lblDetailsItemDental = $("#<%= lblDetailsItemDental.ClientID %>").html(); 
        var lblpaidAmountDental = $("#<%= lblpaidAmountDental.ClientID %>").html();
        var lblPaidUnitDental = $("#<%= lblPaidUnitDental.ClientID %>").html(); 
        var lblStatusServiceDetails = $("#<%= lblStatusServiceDetails.ClientID %>").html();
        var procCodeErr = $("#<%= lblProcCodeError.ClientID %>").html(); 
        var placeOfServiceErr = $("#<%= lblPlaceOfServiceErr.ClientID %>").html(); 

        var validationResult = true;
        $("[id*=lblProcCodeError]").text('');
        $("#<%= lblDateOfServiceError.ClientID %>").html("");
        $("#<%= lblChargesError.ClientID %>").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('');
        $("#<%= lblErrorBilledUnits.ClientID %>").html("");
        if ((procedureCode === null || procedureCode === "" || procedureCode === undefined)) {
            validationResult = false;
            $("[id*=lblProcCodeError]").text('*Procedure code is required');
        }
        else {
            var validation = false;
            validation = procedureCode.startsWith("MR");
            if (validation != true) {
                validation = procedureCode.startsWith("DD");
            }
            if (validation != true) {
                validation = procedureCode.startsWith("PNM");
            }
            if (validation === true) {
                validationRetule = false;
                $("[id*=lblProcCodeError]").text('*Procedure code is invalid');
            }
        }
        if ((dateOfService === null || dateOfService === "" || dateOfService === undefined)) {
            validationResult = false;
            $("#<%= lblDateOfServiceError.ClientID %>").html("*Date Of Service is required");
        }
        if (charges === null || charges === "" || charges === undefined) {
            validationResult = false;
            $("#<%= lblChargesError.ClientID %>").html("*Charges are required");
        }

        if (billedUnits === null || billedUnits === "" || billedUnits === undefined) {
            validationResult = false;
            $("#<%= lblErrorBilledUnits.ClientID %>").html("*Billed unit is required for service line N*");
        }
   
        if ((modifierDentalFirst != null && modifierDentalFirst != "" && modifierDentalFirst != undefined)) {
            if (modifierDentalFirst.length != 2) {
                validationResult = false;
                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('*Procedure code modifier must be 2 characters.');
            }
        }
        if ((modifierDentalSecond != null && modifierDentalSecond != "" && modifierDentalSecond != undefined)) {
            if (modifierDentalSecond.length != 2) {
                validationResult = false;
                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('*Procedure code modifier must be 2 characters.');
            }
        }
        if ((modifierDentalThird != null && modifierDentalThird != "" && modifierDentalThird != undefined)) {
            if (modifierDentalThird.length != 2) {
                validationResult = false;
                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('*Procedure code modifier must be 2 characters.');
            }
        }
        if ((modifierDentalFourth != null && modifierDentalFourth != "" && modifierDentalFourth != undefined)) {
            if (modifierDentalFourth.length != 2) {
                validationResult = false;
                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('*Procedure code modifier must be 2 characters.');
            }
        } 
        if ((billedUnits != null && billedUnits != "" && billedUnits != undefined)) {
            if (billedUnits <= 0) {
                validationResult = false;
                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('*Billed units should be greater than zero.');
            }
        }
        if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_ServiceDetailsDental_revtxtChargesDental') != null) {
            if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_ServiceDetailsDental_revtxtChargesDental').style.display != 'none') {
                validationResult = false;
            }
        }
        if (procedureCode.length < 5) {
            validationResult = false;
            $("[id*=lblProcCodeError]").text('*  5-character value is required');
        }
        return validationResult;
    }

    function validateProcedureCodeModifiers() {
        var txtProcCode = $("#<%= txtProducerCodeDental.ClientID %>").first().val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        var providerTypeId = $("[id*=hdnProviderTypeId]").val();
        var modifierDentalFirst = $("#<%= txtModifierDentalFirst.ClientID %>").first().val();
        var modifierDentalSecond = $("#<%= txtModifierDentalSecond.ClientID %>").first().val();
        var modifierDentalThird = $("#<%= txtModifierDentalThird.ClientID %>").first().val();
        var modifierDentalFourth = $("#<%= txtModifierDentalFourth.ClientID %>").first().val();
        var validationResult = true;
        $.ajax({
            type: "GET",
            url: webApiClaims + "validateProcedureCodeModifiers?provider_type_id=" + providerTypeId + "&&procedure_code=" + txtProcCode + "&&M1=" + modifierDentalFirst + "&&M2=" + modifierDentalSecond + "&&M3=" + modifierDentalThird + "&&M4=" + modifierDentalFourth + "&&claimsorPA=claims&&claimORPAType=Dental",
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
                if (result.length > 0) {
                    validationResult = false;
                    $("[id*=lblErrorMessageonTop]").text(result);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                validationResult = false;
                $("[id*=lblErrorMessageonTop]").text(result);
            }
        });
        return validationResult;
    }

    function addDentalServiceDetailCode() {
        var claimServiceDetailsDentalList;
        var claimStatus = $("#ctl00_MainContent_uc5SubmitClaim_txtstatus2").val();

        var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
        var procedureCode = $("#<%= txtProducerCodeDental.ClientID %>").first().val();
        var placeOfService = $("#<%= txtplaceofserviceDental.ClientID %>").first().val();
        var dateOfService = $("#<%= txtdateofserviceDental.ClientID %>").first().val();
        var charges = $("#<%= txtChargesDental.ClientID %>").first().val();
        var storedcharge = charges;
        charges = HandleDecimalValue(charges).toString();
        if (charges == null || charges == "" || charges == undefined) {
            if (storedcharge == "0" || storedcharge == "0.0" || storedcharge == "0.00" || storedcharge == "0.000") {
                charges = "0.00";
            }
        }
        var billedUnits = $("#<%= txtBillUnitDental.ClientID %>").first().val();

        var lineControlNumber = $("#<%= txtLineControllerNoDental.ClientID %>").first().val();
        var priorAuthNumber = $("#<%= txtPriorAuthNumberDental.ClientID %>").first().val();
        var referralNumber = $("#<%= txtReferralDental.ClientID %>").first().val();
        var ProsthesisCode = $("#<%=ddlInlyneCodeDental.ClientID %> option:selected").text();
        var oralCavityDentalFirst = $("#<%=ddlOralCavityDentalFirst.ClientID %> option:selected").text();
        var oralCavityDentalSecond = $("#<%=ddlOralCavityDentalSecond.ClientID %> option:selected").text();
        var oralCavityDentalThird = $("#<%=ddlOralCavityDentalThird.ClientID %> option:selected").text();
        var oralCavityDentalFourth = $("#<%=ddlOralCavityDentalFourth.ClientID %> option:selected").text();
        var oralCavityDentalFifth = $("#<%=ddlOralCavityDentalFifth.ClientID %> option:selected").text();
        var diagnosisPointerFirst = $("#<%=ddlDiagnosisDentalFirst.ClientID %> option:selected").text();
        var diagnosisPointerSecond = $("#<%=ddlDignosisDentalSecond.ClientID %> option:selected").text();
        var diagnosisPointerThird = $("#<%=ddlDignosisDentalThird.ClientID %> option:selected").text();
        var diagnosisPointerFourth = $("#<%=ddlDignosisDentalFourth.ClientID %> option:selected").text();
        var modifierDentalFirst = $("#<%= txtModifierDentalFirst.ClientID %>").first().val();
        var modifierDentalSecond = $("#<%= txtModifierDentalSecond.ClientID %>").first().val();
        var modifierDentalThird = $("#<%= txtModifierDentalThird.ClientID %>").first().val();
        var modifierDentalFourth = $("#<%= txtModifierDentalFourth.ClientID %>").first().val();
        var lblDetailsItemDental = $("#<%= lblDetailsItemDental.ClientID %>").html();
        var lblpaidAmountDental = $("#<%= lblpaidAmountDental.ClientID %>").html();
        var lblPaidUnitDental = $("#<%= lblPaidUnitDental.ClientID %>").html();
        var lblStatusServiceDetails = $("#<%= lblStatusServiceDetails.ClientID %>").html();
        var procCodeErr = $("#<%= lblProcCodeError.ClientID %>").html();
        var placeOfServiceErr = $("#<%= lblPlaceOfServiceErr.ClientID %>").html();
        ProcedureCodetextChange();
        validateFields();
        validateProcedureCodeModifiers();

        if (validateFields() && validateProcedureCodeModifiers()) {
            $("[id*=valerrormessServ]").text('');
            $("[id*=lblProcCodeError]").text('');

            var txtProcCode = document.getElementById("<%=txtProducerCodeDental.ClientID %>").value;
            if ((txtProcCode != undefined) && (txtProcCode != "")) {
                var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "GET",
                    url: webApiPA + "GetProcCodeDetails?val=" + txtProcCode + "&&includeICDCodes=false",
                    //data: '{desc: "" , val: "' + txtProcCode + '", includeICDCodes: false }',
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
                            $("[id*=txtProducerCodeDental]").val('');
                            $("[id*=lblProcCodeError]").text('Procedure code is invalid.');
                        }
                        else {
                            var serviveInfoPlaceOfService = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_txtPlaceofService").first().val();
                            var txtPlaceofService = document.getElementById("<%=txtplaceofserviceDental.ClientID %>").value;
                            if (txtPlaceofService == "" || txtPlaceofService == null || txtPlaceofService == undefined) {

                                $("[id*=lblPlaceOfServiceErr]").html("");
                                addDentalSerDet();
                                return false;
                            }
                            if (serviveInfoPlaceOfService === txtPlaceofService) {
                                $("#<%= txtplaceofserviceDental.ClientID %>").first().val("");
                            }
                            else {
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('');
                            }
                            var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
                            var APIToken = $("[id*=hdnAccessToken]").val();
                            $.ajax({
                                type: "GET",
                                url: webApiPA + "GetPlaceOfServiceCodeDetails?val=" + txtPlaceofService,
                                headers: {
                                    "Access-Control-Allow-Origin": "*",
                                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                                    "Authorization": "Bearer " + APIToken
                                },
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (result) {
                                    if ((result.length == 0) || (result.length > 1)) {
                                        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('* Place of Service Code is invalid');
                                        return false;
                                    } else {
                                        addDentalSerDet();
                                    }
                                }
                            });
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        $("[id*=lblProcCodeError]").text('Procedure code not found.');
                    }
                });
            }
        }
        return false;
    }
    function addDentalSerDet() {
        var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
        var procedureCode = $("#<%= txtProducerCodeDental.ClientID %>").first().val();
        var placeOfService = $("#<%= txtplaceofserviceDental.ClientID %>").first().val();
        var dateOfService = $("#<%= txtdateofserviceDental.ClientID %>").first().val();
        var charges = $("#<%= txtChargesDental.ClientID %>").first().val();
        var storedcharge = charges;
        charges = HandleDecimalValue(charges).toString();
        if (charges == null || charges == "" || charges == undefined) {
            if (storedcharge == "0" || storedcharge == "0.0" || storedcharge == "0.00" || storedcharge == "0.000") {
                charges = "0.00";
            }
        }
        var billedUnits = $("#<%= txtBillUnitDental.ClientID %>").first().val();

        var lineControlNumber = $("#<%= txtLineControllerNoDental.ClientID %>").first().val();
        var priorAuthNumber = $("#<%= txtPriorAuthNumberDental.ClientID %>").first().val();
        var referralNumber = $("#<%= txtReferralDental.ClientID %>").first().val();
        var ProsthesisCode = $("#<%=ddlInlyneCodeDental.ClientID %> option:selected").text();
        var oralCavityDentalFirst = $("#<%=ddlOralCavityDentalFirst.ClientID %> option:selected").text();
        var oralCavityDentalSecond = $("#<%=ddlOralCavityDentalSecond.ClientID %> option:selected").text();
        var oralCavityDentalThird = $("#<%=ddlOralCavityDentalThird.ClientID %> option:selected").text();
        var oralCavityDentalFourth = $("#<%=ddlOralCavityDentalFourth.ClientID %> option:selected").text();
        var oralCavityDentalFifth = $("#<%=ddlOralCavityDentalFifth.ClientID %> option:selected").text();
        var diagnosisPointerFirst = $("#<%=ddlDiagnosisDentalFirst.ClientID %> option:selected").text();
        var diagnosisPointerSecond = $("#<%=ddlDignosisDentalSecond.ClientID %> option:selected").text();
        var diagnosisPointerThird = $("#<%=ddlDignosisDentalThird.ClientID %> option:selected").text();
        var diagnosisPointerFourth = $("#<%=ddlDignosisDentalFourth.ClientID %> option:selected").text();
        var modifierDentalFirst = $("#<%= txtModifierDentalFirst.ClientID %>").first().val();
        var modifierDentalSecond = $("#<%= txtModifierDentalSecond.ClientID %>").first().val();
        var modifierDentalThird = $("#<%= txtModifierDentalThird.ClientID %>").first().val();
        var modifierDentalFourth = $("#<%= txtModifierDentalFourth.ClientID %>").first().val();
        var lblDetailsItemDental = $("#<%= lblDetailsItemDental.ClientID %>").html();
        var lblpaidAmountDental = $("#<%= lblpaidAmountDental.ClientID %>").html();
        var lblPaidUnitDental = $("#<%= lblPaidUnitDental.ClientID %>").html();
        var lblStatusServiceDetails = $("#<%= lblStatusServiceDetails.ClientID %>").html();
        var procCodeErr = $("#<%= lblProcCodeError.ClientID %>").html();
        var placeOfServiceErr = $("#<%= lblPlaceOfServiceErr.ClientID %>").html();

        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "POST",
            url: webApiClaims + "AddDentalServiceDetails?claimid=" + claimid + "&&procedureCode=" + procedureCode + "&&placeOfService=" + placeOfService + "&&dateOfService=" + dateOfService + "&&charges=" + charges + "&&billedUnits=" + billedUnits + "&&lineControlNumber=" + lineControlNumber + "&&priorAuthNumber=" + priorAuthNumber + "&&referralNumber=" + referralNumber + "&&ProsthesisCode=" + ProsthesisCode + "&&oralCavityDentalFirst=" + oralCavityDentalFirst + "&&oralCavityDentalSecond=" + oralCavityDentalSecond + "&&oralCavityDentalThird=" + oralCavityDentalThird + "&&oralCavityDentalFourth=" + oralCavityDentalFourth + "&&oralCavityDentalFifth=" + oralCavityDentalFifth + "&&diagnosisPointerFirst=" + diagnosisPointerFirst + "&&diagnosisPointerSecond=" + diagnosisPointerSecond + "&&diagnosisPointerThird=" + diagnosisPointerThird + "&&diagnosisPointerFourth=" + diagnosisPointerFourth + "&&modifierDentalFirst=" + modifierDentalFirst + "&&modifierDentalSecond=" + modifierDentalSecond + "&&modifierDentalThird=" + modifierDentalThird + "&&modifierDentalFourth=" + modifierDentalFourth + "&&lblDetailsItemDental=" + lblDetailsItemDental + "&&lblpaidAmountDental=" + lblpaidAmountDental + "&&lblPaidUnitDental=" + lblPaidUnitDental + "&&lblStatusServiceDetails=" + lblStatusServiceDetails + "&&user=<%=HttpContext.Current.User.Identity.Name%>",
            //data: '{claimid: "' + claimid + '" ,procedureCode: "' + procedureCode + '" , placeOfService: "' + placeOfService + '" , dateOfService: "' + dateOfService + '" , charges: "' + charges + '" , billedUnits: "' + billedUnits + '" , lineControlNumber: "' + lineControlNumber + '" , priorAuthNumber: "' + priorAuthNumber + '" , referralNumber: "' + referralNumber + '" , ProsthesisCode: "' + ProsthesisCode + '" , oralCavityDentalFirst: "' + oralCavityDentalFirst + '" , oralCavityDentalSecond: "' + oralCavityDentalSecond + '" , oralCavityDentalThird: "' + oralCavityDentalThird + '" , oralCavityDentalFourth: "' + oralCavityDentalFourth + '" , oralCavityDentalFifth: "' + oralCavityDentalFifth + '" , diagnosisPointerFirst: "' + diagnosisPointerFirst + '" , diagnosisPointerSecond: "' + diagnosisPointerSecond + '" , diagnosisPointerThird: "' + diagnosisPointerThird + '" , diagnosisPointerFourth: "' + diagnosisPointerFourth + '" , modifierDentalFirst: "' + modifierDentalFirst + '" , modifierDentalSecond: "' + modifierDentalSecond + '" , modifierDentalThird: "' + modifierDentalThird + '" , modifierDentalFourth: "' + modifierDentalFourth + '" , lblDetailsItemDental: "' + lblDetailsItemDental + '" , lblpaidAmountDental: "' + lblpaidAmountDental + '" , lblPaidUnitDental: "' + lblPaidUnitDental + '", lblStatusServiceDetails: "' + lblStatusServiceDetails + '"  }',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function () {
                displayDentalServiceDetailTable();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblmessage]").text('Error Adding Service Line');
                }
        });
    }

    function CopyDentalServiceLineItem(Claim_service_Id, claim_id, serviceline, control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        var hdnHidePaidAmounts = document.getElementById("<%=hdnHidePaidAmounts.ClientID %>").value;
        $("[id*=lblProcCodeError]").text('');
        $("#<%= lblDateOfServiceError.ClientID %>").html("");
        $("#<%= lblChargesError.ClientID %>").html("");
        $("#<%= lblErrorBilledUnits.ClientID %>").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('');
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "CopyServiceDetail?Claim_service_Id=" + Claim_service_Id + "&&claim_id=" + claim_id + "&&serviceline=" + serviceline,
            //data: '{Claim_service_Id: "' + Claim_service_Id + '", claim_id:"' + claim_id + '", serviceline:"' + serviceline + '"}',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function () {
                displayDentalServiceDetailTable(serviceline);
                $(control).closest('tr').find('input[type=button]').prop('disabled', true);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblIcdVersionError]").text('Error Copying Service Line');
            }
        });
    }

    function EditDentalServiceLineItem(Claim_service_Id, claim_id, serviceline, control, isCopy = 0)
    {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        var hdnHidePaidAmounts = document.getElementById("<%=hdnHidePaidAmounts.ClientID %>").value;
        $("[id*=lblProcCodeError]").text('');                               
        $("#<%= lblDateOfServiceError.ClientID %>").html("");            
        $("#<%= lblChargesError.ClientID %>").html("");            
        $("#<%= lblErrorBilledUnits.ClientID %>").html("");                
        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('');                
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetIndDentalServiceDetail?Claim_service_Id=" + Claim_service_Id + "&&claim_id=" + claim_id + "&&serviceline=" + serviceline,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var claimServiceList = result;
                if (claimServiceList.length > 0) {
                    for (var i = 0; i < claimServiceList.length; i++) {
                        var sequence = i;
                        sequence++;

                        var cde_proc = result[i].cde_proc;
                        var plc_service = result[i].plc_service;
                        var bil_unt = result[i].bil_unt;
                        var pad_unt = result[i].pad_unt;
                        var ServiceDate = new Date(result[i].ServiceDate);
                        var ServiceDate = formatDate(ServiceDate);

                        var cde_clm_chrge = result[i].cde_clm_chrge;
                        var cde_clm_chrge1 = cde_clm_chrge;
                        cde_clm_chrge = HandleDecimalValue(cde_clm_chrge).toString();
                        if (cde_clm_chrge == null || cde_clm_chrge == "" || cde_clm_chrge == undefined) {
                            if (cde_clm_chrge1 == "0" || cde_clm_chrge1 == "0.0" || cde_clm_chrge1 == "0.00" || cde_clm_chrge1 == "0.000") {
                                cde_clm_chrge = "0.00";
                            }
                        }
                        var pad_amnt = result[i].pad_amnt;
                        pad_amnt = HandleDecimalValue(pad_amnt).toString();
                        if (pad_amnt == 'undefined' || pad_amnt == null || pad_amnt == "") {
                            pad_amnt = "0";
                        }
                        else {
                            pad_amnt = HandleDecimalValue(pad_amnt).toString();
                        }
                        if (hdnHidePaidAmounts == "1") {
                            pad_amnt = "";
                        }
                        var cde_clm_status = result[i].cde_clm_status;                         
                        var line_ctr_num = result[i].line_ctr_num;
                        var prior_auth = result[i].prior_auth;
                        var ref_num = result[i].ref_num;
                        var mdf_first = result[i].mdf_first;
                        var mdf_secnd = result[i].mdf_secnd;
                        var mdf_thrd = result[i].mdf_thrd;
                        var mdf_forth = result[i].mdf_forth;
                        var digno_first = result[i].digno_first;
                        var digno_sec = result[i].digno_sec;
                        var digno_third = result[i].digno_third;
                        var digno_forth = result[i].digno_forth;                       
                        var orl_cvt_first = result[i].orl_cvt_first;
                        if (orl_cvt_first == "0" || orl_cvt_first == "1" || orl_cvt_first == "2") {
                            orl_cvt_first = "0" + orl_cvt_first;
                        }
                        var orl_cvt_sec = result[i].orl_cvt_sec;
                        if (orl_cvt_sec == "0" || orl_cvt_sec == "1" || orl_cvt_sec == "2") {
                            orl_cvt_sec = "0" + orl_cvt_sec;
                        }
                        var orl_cvt_third = result[i].orl_cvt_third;
                        if (orl_cvt_third == "0" || orl_cvt_third == "1" || orl_cvt_third == "2") {
                            orl_cvt_third = "0" + orl_cvt_third;
                        }
                        var orl_cvt_forth = result[i].orl_cvt_forth;
                        if (orl_cvt_forth == "0" || orl_cvt_forth == "1" || orl_cvt_forth == "2") {
                            orl_cvt_forth = "0" + orl_cvt_forth;
                        }
                        var orl_cvt_fifth = result[i].orl_cvt_fifth;
                        if (orl_cvt_fifth == "0" || orl_cvt_fifth == "1" || orl_cvt_fifth == "2") {
                            orl_cvt_fifth = "0" + orl_cvt_fifth;
                        }
                        var prosthesis_cd = result[i].prosthesis_cd;

                        $("#<%= txtProducerCodeDental.ClientID %>").first().val(cde_proc);
                        $("#<%= txtdateofserviceDental.ClientID %>").first().val(ServiceDate);
                        $("#<%= txtLineControllerNoDental.ClientID %>").first().val(line_ctr_num);
                        $("#<%= txtPriorAuthNumberDental.ClientID %>").first().val(prior_auth);
                        $("#<%= txtReferralDental.ClientID %>").first().val(ref_num);
                        if (plc_service == "0") {
                            plc_service = "";
                        }
                        $("#<%= txtplaceofserviceDental.ClientID %>").first().val(plc_service);
                        $("#<%= txtModifierDentalFirst.ClientID %>").first().val(mdf_first);
                        $("#<%= txtModifierDentalSecond.ClientID %>").first().val(mdf_secnd);
                        $("#<%= txtModifierDentalThird.ClientID %>").first().val(mdf_thrd);
                        $("#<%= txtModifierDentalFourth.ClientID %>").first().val(mdf_forth);
                        $("[id*=hdnDentalServiceDetails]").val("" + claim_id + "");
                        $("[id*=hdnDentalClaimServiceId]").val("" + Claim_service_Id + "");
                        
                        $('#<%=ddlDiagnosisDentalFirst.ClientID%>').val(digno_first);
                        $('#<%=ddlDignosisDentalSecond.ClientID%>').val(digno_sec);
                        $('#<%=ddlDignosisDentalThird.ClientID%>').val(digno_third);
                        $('#<%=ddlDignosisDentalFourth.ClientID%>').val(digno_forth);
                        $('#<%=ddlOralCavityDentalFirst.ClientID%>').val(orl_cvt_first);
                        $('#<%=ddlOralCavityDentalSecond.ClientID%>').val(orl_cvt_sec);
                        $('#<%=ddlOralCavityDentalThird.ClientID%>').val(orl_cvt_third);
                        $('#<%=ddlOralCavityDentalFourth.ClientID%>').val(orl_cvt_forth);
                        $('#<%=ddlOralCavityDentalFifth.ClientID%>').val(orl_cvt_fifth);
                        if (prosthesis_cd == "Initial Placement") {
                            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlInlyneCodeDental").options[1].selected = true;
                        }
                        if (prosthesis_cd == "Replacement") {
                            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlInlyneCodeDental").options[2].selected = true;
                        }
                        if (prosthesis_cd == "" || prosthesis_cd == null || prosthesis_cd == undefined) {
                            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlInlyneCodeDental").options[0].selected = true;
                        }                         
                        var clmstatus = $("#ctl00_MainContent_uc5SubmitClaim_txtstatus2").val();
                        $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblStatusServiceDetails').text(clmstatus);
                        $("[id*=txtChargesDental]").val("" + cde_clm_chrge + "");
                        $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblpaidAmountDental').text(pad_amnt);
                        $("[id*=txtBillUnitDental]").val("" + bil_unt + ""); 
                        $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblPaidUnitDental').text(pad_unt);

                        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").attr("disabled", false);
                        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").attr("disabled", false);
                        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").attr("disabled", false);
                        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").attr("disabled", false);
                        if (!(isCopy)) {
                            $("#<%= lblDetailsItemDental.ClientID %>").html("" + serviceline + "");
                        }

                        if (digno_first.trim() != null && digno_first.trim() != "" && digno_first.trim() != "undefined")
                        {
                            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst option:contains("' + digno_first + '")').prop('selected', true);
                            Dental_loader1Change();
                        }
                        if (digno_sec.trim() != null && digno_sec.trim() != "" && digno_sec.trim() != "undefined")
                        {
                            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond option:contains("' + digno_sec + '")').prop('selected', true);
                            Dental_loader2Change();
                        }
                        if (digno_third.trim() != null && digno_third.trim() != "" && digno_third.trim() != "undefined")
                        {
                            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird option:contains("' + digno_third + '")').prop('selected', true);
                            Dental_loader3Change();
                        }
                        if (digno_forth.trim() != null && digno_forth.trim() != "" && digno_forth.trim() != "undefined")
                        {
                            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth option:contains("' + digno_forth + '")').prop('selected', true);
                        }

                        if ((digno_first.trim() == null || digno_first.trim() == "" || digno_first.trim() == "undefined") && (digno_sec.trim() == null || digno_sec.trim() == "" || digno_sec.trim() == "undefined") && (digno_third.trim() == null || digno_third.trim() == "" || digno_third.trim() == "undefined") && (digno_forth.trim() == null || digno_forth.trim() == "" || digno_forth.trim() == "undefined"))
                        {
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").attr("disabled", false);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").attr("disabled", true);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").attr("disabled", true);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").attr("disabled", true);
                            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst').val(0);
                        }
                    };
                }
                //buttons visibilty
                if (document.getElementById('<%= hdnServiceLines.ClientID %>').value < 50) {
                    if (isCopy) {
                        document.getElementById("<%= dentalSerDiv.ClientID%>").innerHTML = " <input type = 'submit' name = 'ctl00$MainContent$uc5SubmitClaim$ucServiceDetailsDental$btnAddServiceDetailDental' value = 'Add' onclick = 'return addDentalServiceDetailCode();' id='ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_btnAddServiceDetailDental' class='btn btn-primary' style = 'font-weight: bold; width: 70px; visibility: visible;'>";
                        document.getElementById("<%= dentalSerDiv.ClientID%>").innerHTML += "<input type = 'submit' name = 'ctl00$MainContent$uc5SubmitClaim$ucServiceDetailsDental$btnCancelDental' value = 'Cancel' onclick = 'clearallfieldsDentalSetrvicedetail(); displayDentalServiceDetailTable();' id='ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_btnCancelDental' class='btn btn-primary' style = 'font-weight: bold; width: 70px; visibility: visible;'>";
                    } else {
                        document.getElementById("<%= dentalSerDiv.ClientID%>").innerHTML = " <input type = 'submit' name = 'ctl00$MainContent$uc5SubmitClaim$ucServiceDetailsDental$btnUpdateServiceDetailDental' value = 'Update' onclick = 'return UpdatedentalServiceDetail();' id='ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_btnUpdateServiceDetailDental' class='btn btn-primary' style = 'font-weight: bold; width: 70px; visibility: visible;'>";
                        document.getElementById("<%= dentalSerDiv.ClientID%>").innerHTML += "<input type = 'submit' name = 'ctl00$MainContent$uc5SubmitClaim$ucServiceDetailsDental$btnCancelDental' value = 'Cancel' onclick = 'clearallfieldsDentalSetrvicedetail(); displayDentalServiceDetailTable();' id='ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_btnCancelDental' class='btn btn-primary' style = 'font-weight: bold; width: 70px; visibility: visible;'>";
                    }
                } else {
                    document.getElementById("<%= dentalSerDiv.ClientID%>").innerHTML = "";
                }
                return false;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblIcdVersionError]").text('Error Loading Service Lines');
            }
        });
    }

    /*Diagnosis Pointer Dropdowns code starts here*/
    /*Global Variables for diagnosis pointers Dental */
    var oldDiagnosisPointerDentalValue;
    var oldDiagnosisPointerDentalValue2;
    var oldDiagnosisPointerDentalValue3;
    var oldDiagnosisPointerDentalValue4;

    /*Functions for Add scenario*/
    function Dental_loader1Change() {
        var diagPointerDental1 = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst option:selected").val();

        if (diagPointerDental1 == oldDiagnosisPointerDentalValue) {
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").attr("disabled", false);
            if (oldDiagnosisPointerDentalValue != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond > option[value=' + oldDiagnosisPointerDentalValue + ']').remove();
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird > option[value=' + oldDiagnosisPointerDentalValue + ']').remove();
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth > option[value=' + oldDiagnosisPointerDentalValue + ']').remove();
            }

            //sorting dropdown2
            SortDiagnosisPointerDental2();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond').val(0);

            //sorting dropdown3
            SortDiagnosisPointerDental3();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird').val(0);

            //sorting Dropdown4
            SortDiagnosisPointerDental4();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').val(0);
        }

        if ((diagPointerDental1 != null || diagPointerDental1 != "" || diagPointerDental1 != undefined) &&
            (oldDiagnosisPointerDentalValue == "" || oldDiagnosisPointerDentalValue == null || oldDiagnosisPointerDentalValue == 'undefined')) {

            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond > option[value=' + diagPointerDental1 + ']').remove();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird > option[value=' + diagPointerDental1 + ']').remove();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth > option[value=' + diagPointerDental1 + ']').remove();

            $('#hdnoldDiagnosisPointerDentalValue').val(diagPointerDental1);
            oldDiagnosisPointerDentalValue = $('#hdnoldDiagnosisPointerDentalValue').val();
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").attr("disabled", false);            
            return;
        }
        else if ((diagPointerDental1 != oldDiagnosisPointerDentalValue) && (diagPointerDental1 != "" || diagPointerDental1 != 'undefined' || diagPointerDental1 != null) && (diagPointerDental1.length>0)) {

            var addDentalPointer1 = "<option value='" + oldDiagnosisPointerDentalValue + "'>" + oldDiagnosisPointerDentalValue + "</option>";
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond').append(addDentalPointer1);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird').append(addDentalPointer1);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').append(addDentalPointer1);

            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond > option[value=' + diagPointerDental1 + ']').remove();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird > option[value=' + diagPointerDental1 + ']').remove();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth > option[value=' + diagPointerDental1 + ']').remove();

            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").attr("disabled", false);

            //sorting dropdown2
            SortDiagnosisPointerDental2();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond').val(0);

            //sorting dropdown3 if not disabled
            if (!$('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird').is(':disabled')) {
                SortDiagnosisPointerDental3();
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird').val(0);
                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").attr("disabled", true);
            }

            //sorting dropdown4 if not disabled
            if (!$('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').is(':disabled')) {
                SortDiagnosisPointerDental4();
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').val(0);
                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").attr("disabled", true);
            }
            $('#hdnoldDiagnosisPointerDentalValue').val(diagPointerDental1);
            oldDiagnosisPointerDentalValue = $('#hdnoldDiagnosisPointerDentalValue').val();

            return;
        }
        else if (diagPointerDental1.length == 0) {
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").attr("disabled", true);
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").attr("disabled", true);
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").attr("disabled", true);

            SortDiagnosisPointerDental2();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond').val(0);
            SortDiagnosisPointerDental3();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird').val(0);
            SortDiagnosisPointerDental4();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').val(0);
            return;
        }
        return false;
    }

    function Dental_loader2Change() {
        var diagPointerDental2 = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond option:selected").val();

        if (diagPointerDental2 == oldDiagnosisPointerDentalValue2) {
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").attr("disabled", false);
            if (oldDiagnosisPointerDentalValue != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird > option[value=' + oldDiagnosisPointerDentalValue + ']').remove();
            }
            //sorting dropdown3
            SortDiagnosisPointerDental3();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird').val(0);

            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").attr("disabled", true);
            //sorting Dropdown4
            SortDiagnosisPointerDental4();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').val(0);
        }

        if ((diagPointerDental2 != null && diagPointerDental2 != "" && diagPointerDental2 != undefined) &&
            (oldDiagnosisPointerDentalValue2 == "" || oldDiagnosisPointerDentalValue2 == null || oldDiagnosisPointerDentalValue2 == 'undefined')) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird > option[value=' + diagPointerDental2 + ']').remove();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth > option[value=' + diagPointerDental2 + ']').remove();

            $('#hdnoldDiagnosisPointerDentalValue2').val(diagPointerDental2);
            oldDiagnosisPointerDentalValue2 = $('#hdnoldDiagnosisPointerDentalValue2').val();

            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").attr("disabled", false);            
            return;
        }
        else if ((diagPointerDental2 != oldDiagnosisPointerDentalValue2) && (diagPointerDental2 != "" || diagPointerDental2 != 'undefined' || diagPointerDental2 != null) && (diagPointerDental2.length>0)) {
            var addDentalPointer2 = "<option value='" + oldDiagnosisPointerDentalValue2 + "'>" + oldDiagnosisPointerDentalValue2 + "</option>";

            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird').append(addDentalPointer2);
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').append(addDentalPointer2);

            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird > option[value=' + diagPointerDental2 + ']').remove();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth > option[value=' + diagPointerDental2 + ']').remove();

            if (oldDiagnosisPointerDentalValue != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird > option[value=' + oldDiagnosisPointerDentalValue + ']').remove();
            }

            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").attr("disabled", false);

            //sorting dropdown3
            SortDiagnosisPointerDental3();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird').val(0);

            //sorting dropdown4 if not disabled
            if (!$('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').is(':disabled')) {
                SortDiagnosisPointerDental4();
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').val(0);
                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").attr("disabled", true);
            }

            $('#hdnoldDiagnosisPointerDentalValue2').val(diagPointerDental2);
            oldDiagnosisPointerDentalValue2 = $('#hdnoldDiagnosisPointerDentalValue2').val();

            return;
        }
        else if (diagPointerDental2.length == 0) {
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").attr("disabled", true);
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").attr("disabled", true);

            SortDiagnosisPointerDental3();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird').val(0);
            SortDiagnosisPointerDental4();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').val(0);
            return;
        }
        return false;
    }

    function Dental_loader3Change() {
        var diagPointerDental3 = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird option:selected").val();

        if (diagPointerDental3 == oldDiagnosisPointerDentalValue3) {
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").attr("disabled", false);
            if (oldDiagnosisPointerDentalValue != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth > option[value=' + oldDiagnosisPointerDentalValue + ']').remove();
            }
            if (oldDiagnosisPointerDentalValue2 != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth > option[value=' + oldDiagnosisPointerDentalValue2 + ']').remove();
            }
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").attr("disabled", false);

            //sorting Dropdown4
            SortDiagnosisPointerDental4();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').val(0);

            $('#hdnoldDiagnosisPointerDentalValue3').val(diagPointerDental3);
            oldDiagnosisPointerDentalValue3 = $('#hdnoldDiagnosisPointerDentalValue3').val();
        }
        if ((diagPointerDental3 != null && diagPointerDental3 != "" && diagPointerDental3 != undefined) &&
            (oldDiagnosisPointerDentalValue3 == "" || oldDiagnosisPointerDentalValue3 == null || oldDiagnosisPointerDentalValue3 == 'undefined')) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth > option[value=' + diagPointerDental3 + ']').remove();

            $('#hdnoldDiagnosisPointerDentalValue3').val(diagPointerDental3);
            oldDiagnosisPointerDentalValue3 = $('#hdnoldDiagnosisPointerDentalValue3').val();
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").attr("disabled", false);          
            return;

        }
        else if ((diagPointerDental3 != oldDiagnosisPointerDentalValue3) && (diagPointerDental3 != "" || diagPointerDental3 != 'undefined' || diagPointerDental3 != null) && (diagPointerDental3.length>0)) {
            var addDentalPointer3 = "<option value='" + oldDiagnosisPointerDentalValue3 + "'>" + oldDiagnosisPointerDentalValue3 + "</option>";

            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').append(addDentalPointer3);

            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth > option[value=' + diagPointerDental3 + ']').remove();

            if (oldDiagnosisPointerDentalValue != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth > option[value=' + oldDiagnosisPointerDentalValue + ']').remove();
            }
            if (oldDiagnosisPointerDentalValue2 != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth > option[value=' + oldDiagnosisPointerDentalValue2 + ']').remove();
            }
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").attr("disabled", false);
            //sorting Dropdown4
            SortDiagnosisPointerDental4();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').val(0);


            $('#hdnoldDiagnosisPointerDentalValue3').val(diagPointerDental3);
            oldDiagnosisPointerDentalValue3 = $('#hdnoldDiagnosisPointerDentalValue3').val();

            return;
        }
        else if (diagPointerDental3.length == 0) {
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").attr("disabled", true);

            SortDiagnosisPointerDental4();
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').val(0);
            return;
        }
        return false;
     }

    /*Functions for sorting dropdown values in ascending order*/
    function SortDiagnosisPointerDental2() {
        var selectList = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond option");
        selectList.sort(function (a, b) {
            a = a.value;
            b = b.value;

            return a - b;
        });
        $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond').html(selectList);
    }

    function SortDiagnosisPointerDental3() {
        var selectList = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird option");
        selectList.sort(function (a, b) {
            a = a.value;
            b = b.value;

            return a - b;
        });
        $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird').html(selectList);
    }

    function SortDiagnosisPointerDental4() {
        var selectList = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth option");
        selectList.sort(function (a, b) {
            a = a.value;
            b = b.value;

            return a - b;
        });
        $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').html(selectList);
    }

    function ReloadDiagnosisPointers() {
        $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond').html($('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst').html());
        $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird').html($('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst').html());
        $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth').html($('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst').html());
    }

    function ClearDiagnosisPointersDropdownVariables() {
        oldDiagnosisPointerDentalValue = "";
        oldDiagnosisPointerDentalValue2 = "";
        oldDiagnosisPointerDentalValue3 = "";
        oldDiagnosisPointerDentalValue4 = "";
        ReloadDiagnosisPointers();
    }

    /*Diagnosis Pointer Dropdowns code Ends here*/

    function LaoderDate() {
        functionclear();

        document.getElementById('<%= btnloading6.ClientID %>').style.display = 'block';

    }
    function Loader1() {
       
            functionclear();
       
        document.getElementById('<%= btnloading1.ClientID %>').style.display = 'block'; 
         
    }
    function Loader2() {

        functionclear();

        document.getElementById('<%= btnloading2.ClientID %>').style.display = 'block';

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

    var specialKeys = new Array();
    specialKeys.push(8); //Backspace
    function IsNumeric(e) {
        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
        document.getElementById("error").style.display = ret ? "none" : "inline";
        return ret;
    }
    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9- ]/g, '');
    }
   
    function DisableEnable_Add_ServiceDetail_Dental() {
        setTimeout(function () {
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_btnServiceDetailsDentailAdd").prop("disabled", true)
        }, 50);

        setTimeout(function () {
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_btnServiceDetailsDentailAdd").prop('disabled', false);
        }, 1000);
    }

    function validateDate2() {
        var daterequested = document.getElementById("<%= txtdateofserviceDental.ClientID%>").value;
        var tdate = new Date();
        var dd = tdate.getDate();
        var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
        var yyyy = tdate.getFullYear();
        var currentDate = (MM) + "/" + dd + "/" + yyyy;
        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_birthDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_birthDateRequiredError1').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_txtdateofserviceDental').val('');
        }
        else {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_birthDateRequiredError1').css('display', 'none');
              }

        if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_birthDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_birthDateRequiredError1').text('Future Date not allowed.');
            $('#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_txtdateofserviceDental').val('');
        }
    }


    function visibleProcCode() {
        $("#<%=ucSubmitClaimSearchProc.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").html("");
        $("#<%=ucSubmitClaimSearchProc.FindControl("txtCode").ClientID %>").val("");
        $("#<%=ucSubmitClaimSearchProc.FindControl("txtPlaceOfServiceName").ClientID %>").val(""); 
        $("#<%=ucSubmitClaimSearchProc.FindControl("lbltxtCodeError").ClientID %>").html("");
        localStorage.setItem("indexPrcode", "");
        $find("mperProcCode").show();
        return false;
    }

    function visiblePlaceOfServiceCode() {
        $("#<%= ucSubmitClaimSearchPop.FindControl("txtCode").ClientID %>").val("");
        $("#<%= ucSubmitClaimSearchPop.FindControl("txtPlaceOfServiceName").ClientID %>").val("");
        $("#<%=ucSubmitClaimSearchPop.FindControl("lblSResultPlaceofService").ClientID %>").html("");
        localStorage.setItem("indexPlaceOfServicecodeserviceDetailPanel", "false");
        $("#<%=ucSubmitClaimSearchPop.FindControl("gvSubmitClaimSearchPop").ClientID %>").html("");
        localStorage.setItem("indexPlaceOfServicecode", "");
        $find("mperPlaceOfServiceCode").show();
        return false;
    }
    function GetProcedureCodeInfo() {
        var txtProcedureCode = $("#<%= ucSubmitClaimSearchProc.FindControl("txtCode").ClientID %>").first().val();
        var txtProcodedesc = $("#<%= ucSubmitClaimSearchProc.FindControl("txtPlaceOfServiceName").ClientID %>").first().val();
        $("#<%= ucSubmitClaimSearchProc.FindControl("lbltxtCodeError").ClientID %>").first().val("");
        if (txtProcedureCode != undefined) {
            var txtProcCode = $("#<%= ucSubmitClaimSearchProc.FindControl("txtCode").ClientID %>").first().val();
            var txtProccodedesc = $("#<%= ucSubmitClaimSearchProc.FindControl("txtPlaceOfServiceName").ClientID %>").first().val();
            $("#<%=ucSubmitClaimSearchProc.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").html("");
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetProcCodeDetails?desc=" + txtProccodedesc + "&&val=" + txtProcCode + "&&includeICDCodes=false",
                //data: '{desc: "' + txtProccodedesc + '" , val: "' + txtProcCode + '", includeICDCodes: false }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result)
                {
                    if (result.length == 0)
                    {
                        $("#<%=ucSubmitClaimSearchProc.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").append("<tr><td> No Records Found </td></tr>");
                    }
                    else
                    {
                        $("#<%=ucSubmitClaimSearchProc.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").append("<tr><th>Procedure Code </th><th>Procedure Code Description </th></tr>");
                        for (var i = 0; i < result.length; i++)
                        {
                            $("#<%=ucSubmitClaimSearchProc.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").append("<tr><td><a onClick='GetProcCodeDental(this); return false;'>" + result[i].Proc_Code + "</asp:LinkButton></td><td>" + result[i].Proc_Desc + "</td></tr>");
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblmessage]").text('Procedure code not found.');
                }

            });
        }
        return false;
    }

    function GetProcCodeDental(lnk) {
        $find("mperProcCode").hide();
        var gridindexProccode = localStorage.getItem("indexProccode");      
             var textboxrow = lnk.parentNode.parentNode;
            $("[id*=txtProducerCodeDental]").val(textboxrow.cells[0].innerText.trim());
             return false;
         
    }

    function GetPlaceOfServiceCodeInfo() {
        $("#<%=ucSubmitClaimSearchPop.FindControl("lblSResultPlaceofService").ClientID %>").html("");
        $("#<%=ucSubmitClaimSearchPop.FindControl("gvSubmitClaimSearchPop").ClientID %>").html("");
        var sample = localStorage.getItem("indexPlaceOfServicecodeserviceDetailPanel");
        var PlaceCode = $("#<%= ucSubmitClaimSearchPop.FindControl("txtCode").ClientID %>").first().val();
        var Piacecodedesc = $("#<%= ucSubmitClaimSearchPop.FindControl("txtPlaceOfServiceName").ClientID %>").first().val();
        if (sample == "false") {
        if (PlaceCode == "" && Piacecodedesc == "") {
            $("#<%=ucSubmitClaimSearchPop.FindControl("lblSResultPlaceofService").ClientID %>").html("Place of service code or name is required");
            return false;
            }
            if (PlaceCode.length >= 1) {
                if (PlaceCode != undefined && PlaceCode.length != 2 && PlaceCode.length < 2) {
                    $("#<%=ucSubmitClaimSearchPop.FindControl("lblSResultPlaceofService").ClientID %>").html("*2-Digit Place of service code is required");
                    return false;
                }
            }
           
        }
        if (sample == "false") {

            var txtPlaceCode = $("#<%= ucSubmitClaimSearchPop.FindControl("txtCode").ClientID %>").first().val();

            var txtPiacecodedesc = $("#<%= ucSubmitClaimSearchPop.FindControl("txtPlaceOfServiceName").ClientID %>").first().val();

            $("#<%=ucSubmitClaimSearchPop.FindControl("gvSubmitClaimSearchPop").ClientID %>").html("");
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetPlaceOfServiceCodeDetails?desc=" + txtPiacecodedesc + "&&val=" + txtPlaceCode,
                //data: '{desc: "' + txtPiacecodedesc + '" , val: "' + txtPlaceCode + '" }',
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
                        $("#<%=ucSubmitClaimSearchPop.FindControl("gvSubmitClaimSearchPop").ClientID %>").append("<tr><td> No Records Found </td></tr>");
                    }
                    else {
                        $("#<%=ucSubmitClaimSearchPop.FindControl("gvSubmitClaimSearchPop").ClientID %>").append("<tr><th>Place Of Service Code </th><th>Place Of Service Code Description </th></tr>");
                        for (var i = 0; i < result.length; i++) {
                            $("#<%=ucSubmitClaimSearchPop.FindControl("gvSubmitClaimSearchPop").ClientID %>").append("<tr><td><a onClick='GetPlaceOfserviceCodeDental(this); return false;'>" + result[i].Placeofservice_Code + "</asp:LinkButton></td><td>" + result[i].Placeofservice_Desc + "</td></tr>");
                        };
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblmessage]").text('Place Of Service code not found.');
                }
            });

            return false;
        }
        else if (sample == "true") {
            GetPlaceOfServiceCodeInfoDentalPlaceOfService();
            return false;
        }
    }

    function GetPlaceOfserviceCodeDental(lnk) {
        $find("mperPlaceOfServiceCode").hide();

        var gridindexProccode = localStorage.getItem("indexPlaceOfServicecode");

        if (!(gridindexProccode == "")) {

            var row = lnk.parentNode.parentNode;
            <%--var grid = document.getElementById("<%= gvServiceDetailDental.ClientID%>");--%>
            var inputs = grid.rows[gridindexReasoncode].getElementsByTagName("INPUT");
            inputs[2].value = row.cells[0].innerText;

            return false;
        }
        else {
            var textboxrow = lnk.parentNode.parentNode;
            $("[id*=txtplaceofserviceDental]").val(textboxrow.cells[0].innerText.trim());

            return false;
        }
    }

    function ProcedureCodetextChange() {
        $("[id*=valerrormessServ]").text('');
        $("[id*=lblProcCodeError]").text('');

        var txtProcCode = document.getElementById("<%=txtProducerCodeDental.ClientID %>").value;
        if (txtProcCode.length < 5) {
            $("[id*=lblProcCodeError]").text('*  5-character value is required');
            return false;
        }
        if ((txtProcCode != undefined) && (txtProcCode != "")) {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiPA + "GetProcCodeDetails?val=" + txtProcCode + "&&includeICDCodes=false",
                //data: '{desc: "" , val: "' + txtProcCode + '", includeICDCodes: false }',
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
                        $("[id*=txtProducerCodeDental]").val('');
                        $("[id*=lblProcCodeError]").text('Procedure code is invalid.');
                    }
                    else {
                        $("[id*=lblProcCodeError]").text('');
                        $("[id*=valerrormessServ]").text('');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblProcCodeError]").text('Procedure code not found.');
                    return false;
                }
            });
        }
    }


    function ValidatePlceofService() {
        var serviveInfoPlaceOfService = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_txtPlaceofService").first().val();       
        var txtPlaceofService = document.getElementById("<%=txtplaceofserviceDental.ClientID %>").value;
        if (txtPlaceofService == "" || txtPlaceofService == "" || txtPlaceofService == undefined) {
            $("[id*=lblPlaceOfServiceErr]").html("");
            return false;
        }
        if (serviveInfoPlaceOfService === txtPlaceofService) {
            // $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('* Place of service in service detail should only be entered if different than claim level');
            $("#<%= txtplaceofserviceDental.ClientID %>").first().val("");
            // return false;
        } else { $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text(''); }
        var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
        if (txtPlaceofService == "0") { txtPlaceofService = "00"; }
        var APIToken = $("[id*=hdnAccessToken]").val();
         $.ajax({
             type: "GET",
             url: webApiPA + "PlaceofServiceTextChanged?txtPlaceofService=" + txtPlaceofService + "&&ClaimId=" + claimid,
             //data: '{txtPlaceofService: "' + txtPlaceofService + '",ClaimId:"' + claimid +'" }',
             headers: {
                 "Access-Control-Allow-Origin": "*",
                 "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                 "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                 "Authorization": "Bearer " + APIToken
             },
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             success: function (result) {
                 if (result == "") {
                     $("[id*=lblPlaceOfServiceErr]").html("" + result + "");
                 }
                 else {
                     $("[id*=lblPlaceOfServiceErr]").html("" + result + "");
                 }
             }
         });
     }

    function HideLabel() {
        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_valerrormessServ").text('');
    }


    function UpdatedentalServiceDetail() {
        var claimStatus = $("#ctl00_MainContent_uc5SubmitClaim_txtstatus2").val();
        var claimServiceDetailsDentalList;
        var validateField = "";
        var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
        var procedureCode = $("#<%= txtProducerCodeDental.ClientID %>").first().val();
        var placeOfService = $("#<%= txtplaceofserviceDental.ClientID %>").first().val();
        var dateOfService = $("#<%= txtdateofserviceDental.ClientID %>").first().val();
        var charges = $("#<%= txtChargesDental.ClientID %>").first().val();
        var storedcharge = charges;
        charges = HandleDecimalValue(charges).toString();
        if (charges == null || charges == "" || charges == undefined) {
            if (storedcharge == "0" || storedcharge == "0.0" || storedcharge == "0.00" || storedcharge == "0.000") {
                charges = "0.00";
            }
        }
        var billedUnits = $("#<%= txtBillUnitDental.ClientID %>").first().val();
        var lineControlNumber = $("#<%= txtLineControllerNoDental.ClientID %>").first().val();
        var priorAuthNumber = $("#<%= txtPriorAuthNumberDental.ClientID %>").first().val();
        var referralNumber = $("#<%= txtReferralDental.ClientID %>").first().val();
        var ProsthesisCode = $("#<%=ddlInlyneCodeDental.ClientID %> option:selected").text();
        var oralCavityDentalFirst = $("#<%=ddlOralCavityDentalFirst.ClientID %> option:selected").text();
        var oralCavityDentalSecond = $("#<%=ddlOralCavityDentalSecond.ClientID %> option:selected").text();
        var oralCavityDentalThird = $("#<%=ddlOralCavityDentalThird.ClientID %> option:selected").text();
        var oralCavityDentalFourth = $("#<%=ddlOralCavityDentalFourth.ClientID %> option:selected").text();
        var oralCavityDentalFifth = $("#<%=ddlOralCavityDentalFifth.ClientID %> option:selected").text();
        var diagnosisPointerFirst = $("#<%=ddlDiagnosisDentalFirst.ClientID %> option:selected").text();
        var diagnosisPointerSecond = $("#<%=ddlDignosisDentalSecond.ClientID %> option:selected").text();
        var diagnosisPointerThird = $("#<%=ddlDignosisDentalThird.ClientID %> option:selected").text();
        var diagnosisPointerFourth = $("#<%=ddlDignosisDentalFourth.ClientID %> option:selected").text();
        var modifierDentalFirst = $("#<%= txtModifierDentalFirst.ClientID %>").first().val();
        var modifierDentalSecond = $("#<%= txtModifierDentalSecond.ClientID %>").first().val();
        var modifierDentalThird = $("#<%= txtModifierDentalThird.ClientID %>").first().val();
        var modifierDentalFourth = $("#<%= txtModifierDentalFourth.ClientID %>").first().val();
        var lblDetailsItemDental = $("#<%= lblDetailsItemDental.ClientID %>").html();
        var lblpaidAmountDental = $("#<%= lblpaidAmountDental.ClientID %>").html();
        var lblPaidUnitDental = $("#<%= lblPaidUnitDental.ClientID %>").html();
        var lblStatusServiceDetails = $("#<%= lblStatusServiceDetails.ClientID %>").html();
        var hdnDentalServiceDetails = $("#<%= hdnDentalServiceDetails.ClientID %>").val();
        var hdnDentalClaimServiceId = $("#<%= hdnDentalClaimServiceId.ClientID %>").val();
        var procCodeErr = $("#<%= lblProcCodeError.ClientID %>").html();
        var placeOfServiceErr = $("#<%= lblPlaceOfServiceErr.ClientID %>").html();

        if (validateFields() && validateProcedureCodeModifiers()) {
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('');
            $("[id*=valerrormessServ]").text('');
            $("[id*=lblProcCodeError]").text('');

            var txtProcCode = document.getElementById("<%=txtProducerCodeDental.ClientID %>").value;
            if ((txtProcCode != undefined) && (txtProcCode != "")) {
                var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "GET",
                    url: webApiPA + "GetProcCodeDetails?val=" + txtProcCode + "&&includeICDCodes=false",
                    //data: '{desc: "" , val: "' + txtProcCode + '", includeICDCodes: false }',
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
                            $("[id*=txtProducerCodeDental]").val('');
                            $("[id*=lblProcCodeError]").text('Procedure code is invalid.');
                        }
                        else {
                            var serviveInfoPlaceOfService = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationDental_txtPlaceofService").first().val();
                            var txtPlaceofService = document.getElementById("<%=txtplaceofserviceDental.ClientID %>").value;

                            if (txtPlaceofService == "" || txtPlaceofService == null || txtPlaceofService == undefined) {
                                $("[id*=lblPlaceOfServiceErr]").html("");
                                updateSerDet();
                                return false;
                            }
                            if (serviveInfoPlaceOfService === txtPlaceofService) {
                                $("#<%= txtplaceofserviceDental.ClientID %>").first().val("");
                            } else {
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('');
                            }
                            var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
                            if (txtPlaceofService == "0") { txtPlaceofService = "00"; }
                            var APIToken = $("[id*=hdnAccessToken]").val();
                            $.ajax({
                                type: "GET",
                                url: webApiPA + "GetPlaceOfServiceCodeDetails?val=" + txtPlaceofService,
                                //data: '{desc: "" , val: "' + txtPlaceofService + '" }',
                                headers: {
                                    "Access-Control-Allow-Origin": "*",
                                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                                    "Authorization": "Bearer " + APIToken
                                },
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (result) {
                                    if ((result.length == 0) || (result.length > 1)) {
                                        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('* Place of Service Code is invalid');
                                        return false;
                                    } else {
                                        updateSerDet();
                                    }
                                }

                            });
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert("hello4");
                        $("[id*=lblProcCodeError]").text('Procedure code not found.');
                    }
                });
            }
        }
        return false;
    }

    function updateSerDet() {
        var claimStatus = $("#ctl00_MainContent_uc5SubmitClaim_txtstatus2").val();
        var claimServiceDetailsDentalList;
        var validateField = "";
        var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
        var procedureCode = $("#<%= txtProducerCodeDental.ClientID %>").first().val();
        var placeOfService = $("#<%= txtplaceofserviceDental.ClientID %>").first().val();
        var dateOfService = $("#<%= txtdateofserviceDental.ClientID %>").first().val();
        var charges = $("#<%= txtChargesDental.ClientID %>").first().val();
        var storedcharge = charges;
        charges = HandleDecimalValue(charges).toString();
        if (charges == null || charges == "" || charges == undefined) {
            if (storedcharge == "0" || storedcharge == "0.0" || storedcharge == "0.00" || storedcharge == "0.000") {
                charges = "0.00";
            }
        }
        var billedUnits = $("#<%= txtBillUnitDental.ClientID %>").first().val();
        var lineControlNumber = $("#<%= txtLineControllerNoDental.ClientID %>").first().val();
        var priorAuthNumber = $("#<%= txtPriorAuthNumberDental.ClientID %>").first().val();
        var referralNumber = $("#<%= txtReferralDental.ClientID %>").first().val();
        var ProsthesisCode = $("#<%=ddlInlyneCodeDental.ClientID %> option:selected").text();
        var oralCavityDentalFirst = $("#<%=ddlOralCavityDentalFirst.ClientID %> option:selected").text();
        var oralCavityDentalSecond = $("#<%=ddlOralCavityDentalSecond.ClientID %> option:selected").text();
        var oralCavityDentalThird = $("#<%=ddlOralCavityDentalThird.ClientID %> option:selected").text();
        var oralCavityDentalFourth = $("#<%=ddlOralCavityDentalFourth.ClientID %> option:selected").text();
        var oralCavityDentalFifth = $("#<%=ddlOralCavityDentalFifth.ClientID %> option:selected").text();
        var diagnosisPointerFirst = $("#<%=ddlDiagnosisDentalFirst.ClientID %> option:selected").text();
        var diagnosisPointerSecond = $("#<%=ddlDignosisDentalSecond.ClientID %> option:selected").text();
        var diagnosisPointerThird = $("#<%=ddlDignosisDentalThird.ClientID %> option:selected").text();
        var diagnosisPointerFourth = $("#<%=ddlDignosisDentalFourth.ClientID %> option:selected").text();
        var modifierDentalFirst = $("#<%= txtModifierDentalFirst.ClientID %>").first().val();
        var modifierDentalSecond = $("#<%= txtModifierDentalSecond.ClientID %>").first().val();
        var modifierDentalThird = $("#<%= txtModifierDentalThird.ClientID %>").first().val();
        var modifierDentalFourth = $("#<%= txtModifierDentalFourth.ClientID %>").first().val();
        var lblDetailsItemDental = $("#<%= lblDetailsItemDental.ClientID %>").html();
        var lblpaidAmountDental = $("#<%= lblpaidAmountDental.ClientID %>").html();
        var lblPaidUnitDental = $("#<%= lblPaidUnitDental.ClientID %>").html();
        var lblStatusServiceDetails = $("#<%= lblStatusServiceDetails.ClientID %>").html();
        var hdnDentalServiceDetails = $("#<%= hdnDentalServiceDetails.ClientID %>").val();
        var hdnDentalClaimServiceId = $("#<%= hdnDentalClaimServiceId.ClientID %>").val();
        var procCodeErr = $("#<%= lblProcCodeError.ClientID %>").html();
        var placeOfServiceErr = $("#<%= lblPlaceOfServiceErr.ClientID %>").html();
        var hdnHidePaidAmounts = document.getElementById("<%=hdnHidePaidAmounts.ClientID %>").value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "PUT",
            url: webApiClaims + "EditDentalServiceDetailCode?claimid=" + claimid + "&&procedureCode=" + procedureCode + "&&placeOfService=" + placeOfService + "&&dateOfService=" + dateOfService + "&&charges=" + charges + "&&billedUnits=" + billedUnits + "&&lineControlNumber=" + lineControlNumber + "&&priorAuthNumber=" + priorAuthNumber + "&&referralNumber=" + referralNumber + "&&ProsthesisCode=" + ProsthesisCode + "&&oralCavityDentalFirst=" + oralCavityDentalFirst + "&&oralCavityDentalSecond=" + oralCavityDentalSecond + "&&oralCavityDentalThird=" + oralCavityDentalThird + "&&oralCavityDentalFourth=" + oralCavityDentalFourth + "&&oralCavityDentalFifth=" + oralCavityDentalFifth + "&&diagnosisPointerFirst=" + diagnosisPointerFirst + "&&diagnosisPointerSecond=" + diagnosisPointerSecond + "&&diagnosisPointerThird=" + diagnosisPointerThird + "&&diagnosisPointerFourth=" + diagnosisPointerFourth + "&&modifierDentalFirst=" + modifierDentalFirst + "&&modifierDentalSecond=" + modifierDentalSecond + "&&modifierDentalThird=" + modifierDentalThird + "&&modifierDentalFourth=" + modifierDentalFourth + "&&lblDetailsItemDental=" + lblDetailsItemDental + "&&lblpaidAmountDental=" + lblpaidAmountDental + "&&lblPaidUnitDental=" + lblPaidUnitDental + "&&lblStatusServiceDetails=" + lblStatusServiceDetails + "&&hdnDentalServiceDetails=" + hdnDentalServiceDetails + "&&hdnDentalClaimServiceId=" + hdnDentalClaimServiceId + "&&user=<%=HttpContext.Current.User.Identity.Name%>",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function () {
                displayDentalServiceDetailTable();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblmessage]").text('Error Updating Service Line');
            }
        });
    }

    function clearDentalServiceDetailFields(){
        $("#<%= txtProducerCodeDental.ClientID %>").first().val("");
        $("#<%= txtplaceofserviceDental.ClientID %>").first().val("");
        $("#<%= txtdateofserviceDental.ClientID %>").first().val("");
        $("#<%= txtChargesDental.ClientID %>").first().val("");
        $("#<%= txtBillUnitDental.ClientID %>").first().val("");
        $("#<%= txtLineControllerNoDental.ClientID %>").first().val("");
        $("#<%= txtPriorAuthNumberDental.ClientID %>").first().val("");
        $("#<%= txtReferralDental.ClientID %>").first().val("");
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlInlyneCodeDental") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlInlyneCodeDental").options[0].selected = true;
        }
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlOralCavityDentalFirst") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlOralCavityDentalFirst").options[0].selected = true;
        }
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlOralCavityDentalSecond") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlOralCavityDentalSecond").options[0].selected = true;
        }
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlOralCavityDentalThird") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlOralCavityDentalThird").options[0].selected = true;
        }
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlOralCavityDentalFourth") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlOralCavityDentalFourth").options[0].selected = true;
        }
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlOralCavityDentalFifth") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlOralCavityDentalFifth").options[0].selected = true;
        }
        var Dentaldiag1Length = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst option").length; 
        if (Dentaldiag1Length > 0) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").options[0].selected = true;
        }
        var Dentaldiag2Length = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond option").length;
        if (Dentaldiag2Length > 0) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").options[0].selected = true;
        }
        var Dentaldiag3Length = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird option").length;
        if (Dentaldiag3Length > 0) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").options[0].selected = true;
        }
        var Dentaldiag4Length = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth option").length;
        if (Dentaldiag4Length > 0) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").options[0].selected = true;
        }
        $("#<%= txtModifierDentalFirst.ClientID %>").first().val(""); 
        $("#<%= txtModifierDentalSecond.ClientID %>").first().val("");
        $("#<%= txtModifierDentalThird.ClientID %>").first().val("");
        $("#<%= txtModifierDentalFourth.ClientID %>").first().val("");          

        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").attr("disabled", false);
        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").attr("disabled", true);
        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").attr("disabled", true);
        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").attr("disabled", true);
        ClearDiagnosisPointersDropdownVariables();
    }

    function DeleteDentalServiceDetailLineitem(claimid, service_line) {
        var claimStatus = $("#ctl00_MainContent_uc5SubmitClaim_txtstatus2").val();
        var result = confirm("Are you sure you want to delete service line: " + service_line + "?");
        if (result) {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteClaimServiceLine?service_line=" + service_line + "&&claimid=" + claimid,
                //data: '{service_line: "' + service_line + '" , claimid: "' + claimid + '" }', 
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    $("[id*=lblErrorMessageonTop]").text('');
                    displayDentalServiceDetailTable();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblIcdVersionError]").text('Error Deleting Service Line');
                }
            });
        }
    }

    function clearallfieldsDentalSetrvicedetail() {

        clearDentalServiceDetailFields();

        document.getElementById("<%= dentalSerDiv.ClientID%>").innerHTML = "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ucServiceDetailsDental$btnServiceDetailsDentailAdd' value='ADD' onclick='return addDentalServiceDetailCode();' id='ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_btnServiceDetailsDentailAdd' class='btn btn-primary' style='font-weight:bold;width:70px;'>";
        document.getElementById("<%= dentalSerDiv.ClientID%>").innerHTML += "<input type = 'submit' name = 'ctl00$MainContent$uc5SubmitClaim$ucServiceDetailsDental$btnUpdateServiceDetailDental' value = 'Update' onclick = 'return UpdatedentalServiceDetail();' id='ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_btnUpdateServiceDetailDental' class='btn btn-primary' style = 'font-weight: bold; width: 70px; visibility: hidden;'>";
        document.getElementById("<%= dentalSerDiv.ClientID%>").innerHTML += "<input type = 'submit' name = 'ctl00$MainContent$uc5SubmitClaim$ucServiceDetailsDental$btnCancelDental' value = 'Cancel' onclick = 'clearallfieldsDentalSetrvicedetail(); displayDentalServiceDetailTable();' id='ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_btnCancelDental' class='btn btn-primary' style = 'font-weight: bold; width: 70px; visibility: hidden;'>";
        var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "DentalServiceDetailBindGrid?claimid=" + claimid,
            //data: '{claimid: "' + claimid + '" }',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                claimServiceDetailsDentalList = result;
                var table;
                var totalcharge = 0;
                var totalpaidamount = 0;
                var rowcount = 0;
                
                if (claimServiceDetailsDentalList.length > 0) {

                    table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>*Procedure Code</th><th style='width:10px; scope='col'>Place Of Service</th><th style='width:10px; scope='col'>*Billed Units</th><th style='width:10px; scope='col'>Paid Units</th><th style='width:10px; scope='col'>Date Of Service</th><th style='width:10px; scope='col'>Charges</th><th style='width:10px; scope='col'>Paid Amount</th><th style='width:10px; scope='col'>Status</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                    var j = 0;
                    
                    for (var i = 0; i < claimServiceDetailsDentalList.length; i++) {
                        var sequence = i;
                        sequence++;
                        rowcount++;
                        var cde_proc = result[i].cde_proc;
                        var plc_service = result[i].plc_service;
                        var bil_unt = result[i].bil_unt;
                        var pad_unt = result[i].pad_unt;
                        var ServiceDate = new Date(result[i].ServiceDate);
                        ServiceDate = formatDate(ServiceDate);

                        var cde_clm_chrge = result[i].cde_clm_chrge;
                        var cde_clm_chrge1 = cde_clm_chrge;
                        cde_clm_chrge = HandleDecimalValue(cde_clm_chrge).toString();
                        if (cde_clm_chrge == null || cde_clm_chrge == "" || cde_clm_chrge == undefined) {
                            if (cde_clm_chrge1 == "0" || cde_clm_chrge1 == "0.0" || cde_clm_chrge1 == "0.00" || cde_clm_chrge1 == "0.000") {
                                cde_clm_chrge = "0.00";
                            }
                        }
                        var pad_amnt = result[i].pad_amnt;
                        var cde_clm_status = result[i].cde_clm_status;
                        var Claim_service_Id = result[i].Claim_service_Id;
                        var Claim_Id = result[i].Claim_Id;
                        var serviceline = result[i].service_Line;
                        totalcharge = totalcharge + parseFloat(cde_clm_chrge);
                        totalpaidamount = totalpaidamount + parseFloat(pad_amnt);
                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + cde_proc + "</span></td><td><span  title='Line' class='tNumber'>" + plc_service + "</span></td><td><span title='Line' class='tNumber'>" + bil_unt + "</span></td><td>" + pad_unt + "</td><td>" + ServiceDate + "</td><td>" + cde_clm_chrge + "</td><td>" + pad_amnt + "</td><td>" + cde_clm_status + "</td><td><input type='button' value = 'Edit' onClick = 'return EditDentalServiceLineItem('" + Claim_service_Id + "','" + Claim_Id + "','" + serviceline + "',this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value = 'Copy' onClick = 'return EditDentalServiceLineItem('" + Claim_service_Id + "','" + Claim_Id + "','" + serviceline + "',this, 1); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteDentalServiceDetailLineitem('" + Claim_Id + "','" + serviceline + "');' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        j = sequence;
                    };
                    j++; 
                    $("#<%= lblDetailsItemDental.ClientID %>").html("" + j + "");
                    //table = table + "<tr style='border:none;' class='gridViewRow'><td>Total Charges: " + totalcharge + "</td></tr >";

                    table = table + "</tbody></table>";
                    if (rowcount >= 50) {
                        document.getElementById('<%= btnServiceDetailsDentailAdd.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnUpdateServiceDetailDental.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnServiceDetailsDentailAdd.ClientID%>').style.display = "none";
                        document.getElementById('<%= btnUpdateServiceDetailDental.ClientID%>').style.display = "none";
                    } else {
                        document.getElementById('<%= btnServiceDetailsDentailAdd.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnUpdateServiceDetailDental.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnServiceDetailsDentailAdd.ClientID%>').style.display = "block";
                        document.getElementById('<%= btnUpdateServiceDetailDental.ClientID%>').style.display = "block";
                    }

                    localStorage.setItem("dentalServiceDetailTable", "" + table + "");
                    document.getElementById('<%= providerDentalServicedetailOutput.ClientID %>').innerHTML = table;
                    $("#<%= lblDenTotalCharges.ClientID %>").html("" + totalcharge.toFixed(2) + "");
                    if (isNaN(totalpaidamount)) {
                        $("#<%= lblDenTotalAmountPaid.ClientID %>").html("" + 0.00 + "");

                        }
                        else {
                        $("#<%= lblDenTotalAmountPaid.ClientID %>").html("" + totalpaidamount + "");
                    }

                   
                    }
                    else {
                    document.getElementById('<%= providerDentalServicedetailOutput.ClientID %>').innerHTML = "";
                    $("#<%= lblDenTotalCharges.ClientID %>").html("" + totalcharge.toFixed(2) + "");
                    if (isNaN(totalpaidamount)) {
                        $("#<%= lblDenTotalAmountPaid.ClientID %>").html("" + 0.00 + "");

                        }
                        else {
                        $("#<%= lblDenTotalAmountPaid.ClientID %>").html("" + totalpaidamount + "");
                    }

                    return false;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblIcdVersionError]").text('Diagnosis Code is invalid');
            }
        });
        return false;
    }

    function DentalPriorAuthNumberChanged() {
        var priorAuthNumberDental = $("#<%= txtPriorAuthNumberDental.ClientID %>").first().val();       
        var PriorAuthNumber = $("#ctl00_MainContent_uc5SubmitClaim_ucPriorAuthorizationAndReferringPanel_txtPriorAuthNumber").first().val();        
        if (priorAuthNumberDental === PriorAuthNumber) {           
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('*PA number in the service detail should only be entered if it is different than the header');
            $("#<%= txtPriorAuthNumberDental.ClientID %>").first().val("");
        }
        else { $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text(''); }
        return false;
    }
   
    function DentalReferralChanged() {
        var priorAuthNumberDental = $("#<%= txtReferralDental.ClientID %>").first().val();       
        var PriorAuthNumber = $("#ctl00_MainContent_uc5SubmitClaim_ucPriorAuthorizationAndReferringPanel_txtReferralNumber").first().val();        
        if (priorAuthNumberDental === PriorAuthNumber) {           
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('*Referral number in the service detail should only be entered if it is different than the header');
             $("#<%= txtReferralDental.ClientID %>").first().val("");
         }
         else { $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text(''); }
         return false;
    }

    function ModifierChanged(e) {
        var variable = "";
        var modifierNo = "";
        if (e.id === "ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_txtModifierDentalFirst") {
            variable = $("#<%= txtModifierDentalFirst.ClientID %>").first().val();
            modifierNo = "1";
            if (variable == "00" || variable == "0") { $("[id*=txtModifierDentalFirst]").val(''); return false; }
        }
        if (e.id === "ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_txtModifierDentalSecond") {
            variable = $("#<%= txtModifierDentalSecond.ClientID %>").first().val();
            modifierNo = "2";
            if (variable == "00" || variable == "0") { $("[id*=txtModifierDentalSecond]").val(''); return false; }
        }
        if (e.id === "ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_txtModifierDentalThird") {
            variable = $("#<%= txtModifierDentalThird.ClientID %>").first().val();
            modifierNo = "3";
            if (variable == "00" || variable == "0") { $("[id*=txtModifierDentalThird]").val(''); return false; }
        }
        if (e.id === "ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_txtModifierDentalFourth") {
            variable = $("#<%= txtModifierDentalFourth.ClientID %>").first().val();
            modifierNo = "4";
            if (variable == "00" || variable == "0") { $("[id*=txtModifierDentalFourth]").val(''); return false; }
        }
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "CheckModifierText?variable=" + variable,
            //data: '{variable: "' + variable + '" }',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                claimServiceDetailsDentalList = result;
                if (result === "true") {
                    $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('');
                } else if (result === "false") {
                    $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text("*Procedure modifier" + modifierNo + " is invalid.");
                    if (modifierNo == "1") { $("[id*=txtModifierDentalFirst]").val(''); }
                    if (modifierNo == "2") { $("[id*=txtModifierDentalSecond]").val(''); }
                    if (modifierNo == "3") { $("[id*=txtModifierDentalThird]").val(''); }
                    if (modifierNo == "4") { $("[id*=txtModifierDentalFourth]").val(''); }
                    
                    return false;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblIcdVersionError]").text('Diagnosis Code is invalid');
            }
        });
       
}
   
</script>
<asp:HiddenField runat="server"  ID="hdnHidePaidAmounts" Value="0"/>
<asp:HiddenField runat="server"  ID="hdnServiceLines" Value="0" />
<asp:UpdatePanel ID="upServiceDetailsS" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div style="float: left; width: 100%; height: 50px;">
                     <div style="padding-top: 20px; padding-left: 10px; font-size: 16px;overflow-x:hidden">
                    <asp:ValidationSummary ID="valerrormessServ" DisplayMode="List" runat="server" CssClass="failureNotification"
                        ValidationGroup="validateDentalServiceCus" />
                         <asp:Label runat="server" ID="lblErrorMessageonTop" CssClass="failureNotification"></asp:Label>
                     </div>
                </div>
        <div class="divGrid" style="padding-top: 10px; padding-left: 50px;">             
            <br />
                <div id="providerDentalServicedetailOutput" runat="server"></div>
            <div style="float:right">
                <div class="col-sm-5" style="height: 20px;">
                    <span class="ohio-field" style="font-size: 15px; text-align: right">Total Charges:</span>
                </div>
                <div class="col-sm-7">
                    <asp:Label ID="lblDenTotalCharges" runat="server"></asp:Label>
                </div>
                <div class="col-sm-5">
                    <span class="ohio-field" style="font-size: 15px; text-align: right">Total Amount Paid:</span>
                </div>
                <div class="col-sm-7">
                    <asp:Label ID="lblDenTotalAmountPaid" runat="server"></asp:Label>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <asp:HiddenField ID="hdnClaimId" runat="server" />
        <asp:HiddenField ID="hdnClaimType" runat="server" />
        <asp:HiddenField ID="hdnDentalServiecLineClaimStatus" runat="server" />
        <div runat="server" id="divService" style="height:auto;overflow-x:hidden">
            <br />   
                <div class="row"><span style="color:red;text-align:left !important; margin-left:50px !important;margin-bottom:15px !important">If a prior authorization number is entered, please ensure it is a valid one. Inaccurate entry will result in delay or denial of Claim Processing</span></div>
            <div style="background-color: lightblue;padding:8px 5px !important" class="col-sm-12 row">
                <div class="col-sm-2" style="font-size: 15px; text-align: right; padding-left: 50px;
                    padding-top: 2px; height: 25px;">
                    <span class="ohio-field" style="font-size: 17px;font-weight:bold">Service Line:
                        <asp:Label ID="lblDetailsItemDental" runat="server" Height="16px" ReadOnly="true"
                            Width="52px" />
                    </span>
                </div>
                <div class="col-sm-10" style="font-size: 14px;">

                    <asp:HiddenField ID="hdnDentalServiceDetails" runat="server" />
                    <asp:HiddenField ID="hdnDentalClaimServiceId" runat="server" />
                </div>
            </div>
            <div class="row">
                <!--base coloumns for the dental service detail start-->
                <div class="col-sm-4"  style="margin-left: 50px;">
                    <div class="row">
                        <div class="col-sm-7">
                            <span class="ohio-field" style="font-size: 14px; text-align: left; padding: 0;"><span style="color: red">* </span>Procedure
                                Code:</span>
                        </div>
                        <div class="col-sm-3" style="padding-left: 0px;">
                            <asp:TextBox ID="txtProducerCodeDental" OnChange="return ProcedureCodetextChange()" runat="server" CssClass="formFieldTextBoxSmall" Width="70px" Height="30px" onKeyUp="javascript:alphanumericOnly(this);"
                                MaxLength="5" /><br />     
                            <asp:Label ID="lblProcCodeError" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                        <div class="col-sm-2" style="padding-left: 0px;">
                            <asp:LinkButton ID="lnkProducerCodeDental" Style="font-size: 14px;padding-left:30px;" runat="server" CausesValidation="false"
                                Text="Search" ToolTip="Search" OnClientClick="return visibleProcCode();" Visible="true"></asp:LinkButton>&nbsp;&nbsp;
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-7">
                            <span class="ohio-field" style="font-size: 14px; text-align: left"><span style="color: red">* </span>Date of Services:</span>
                        </div>
                        <div class="col-sm-5" style="text-align: left; padding-left: 0px;">
                            <span style="text-align: left;">
                                <asp:TextBox ID="txtdateofserviceDental" runat="server" CssClass="formFieldPos" Style="width: 100px !important;height:30px;" OnChange="validateDate2()"  Text=""   />
                                 <asp:Image ID="imgoccToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp"
                                    Height="16px" AlternateText="Calendar Icon" />
                                <ajax:CalendarExtender ID="clExtender" runat="server" Format="MM/dd/yyyy" TargetControlID="txtdateofserviceDental"
                                     PopupPosition="TopRight" CssClass="QstCalendarCSS" EnabledOnClient="true" />
                                 <button id="btnloading6" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
        
                                <br />   
                                 <asp:Label ID="lblDateOfServiceError" runat="server" ForeColor="Red"></asp:Label>
                                 <asp:Label ID="birthDateRequiredError1" runat="server" Text="" CssClass="error-message" style="display:none;"></asp:Label>
                                <asp:Label ID="lblErrorDOS" runat="server" ForeColor="Red"></asp:Label>
                            </span>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-7">
                            <span class="ohio-field" style="font-size: 14px; text-align: left">Line Control Number:</span>
                        </div>
                        <div class="col-sm-5" style="text-align: left; padding-left: 0px;">
                            <asp:TextBox ID="txtLineControllerNoDental" runat="server" CssClass="formFieldTextBoxSmall" MaxLength="50" onKeyUp="javascript:alphanumericOnly(this);"
                                Style="width: 110px;height:30px;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-7">
                            <span class="ohio-field" style="font-size: 14px; text-align: left">Prior Authorization
                                Number:</span>
                        </div>
                        <div class="col-sm-5" style="text-align: left; padding-left: 0px;">
                            <asp:TextBox ID="txtPriorAuthNumberDental" onChange="return DentalPriorAuthNumberChanged()" runat="server" CssClass="formFieldTextBoxSmall" MaxLength="50" onKeyUp="javascript:alphanumericOnly(this);"
                                Style="height: 30px; width: 110px;" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPriorAuthNumberDental" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-7">
                            <span class="ohio-field" style="font-size: 14px; text-align: left">Referral Number:</span>
                        </div>
                        <div class="col-sm-4" style="text-align: left; padding-left: 0px">
                            <asp:TextBox ID="txtReferralDental" onChange="return DentalReferralChanged()" runat="server" CssClass="formFieldTextBoxSmall" MaxLength="50" onKeyUp="javascript:alphanumericOnly(this);"
                                Style="width: 110px;height:30px;" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtReferralDental" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-5">
                    <div class="row">
                        <div class="col-sm-4">
                            <span class="ohio-field" style="font-size: 14px; text-align: left;">Place of Service:</span>
                        </div>
                        <div class="col-sm-2" style="padding-left: 0px;">
                            <span style="text-align: left;">
                                <asp:TextBox ID="txtplaceofserviceDental" runat="server" CssClass="formFieldPos" MaxLength="2" onkeydown = "return (!(event.keyCode>=65 && event.keyCode <=90) && event.keyCode!=32);"
                                   OnChange="return ValidatePlceofService()"  Style="height: 30px; width: 70px"  />
                                 <button id="btnloading1" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
        
                                <div class="form-group row" >
                            <asp:Label ID="lblPlaceOfServiceErr" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                            </span>

                        </div>
                        <div class="col-sm-2" style="padding-left: 0px;">
                            <asp:LinkButton ID="lnkPlaceDental" Text="Search" Style="font-size: 15px;padding-left:30px;" runat="server" CausesValidation="false"
                                ToolTip="Search" OnClientClick="return visiblePlaceOfServiceCode()"
                                Visible="true"></asp:LinkButton>

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <span class="ohio-field" style="font-size: 14px; text-align: left;">Modifier:</span>
                        </div>
                        <div class="col-sm-8" style="padding-left: 0px;">
                            <asp:TextBox ID="txtModifierDentalFirst" onChange="return ModifierChanged(this)" runat="server" CssClass="formfieldMod" MaxLength="2" onKeyUp="javascript:alphanumericOnly(this);"
                                Style="height: 30px; width: 50px;" onKeyDown="HideLabel()"/>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtModifierDentalFirst" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            
                            <asp:TextBox ID="txtModifierDentalSecond" runat="server" onChange="return ModifierChanged(this)" CssClass="formfieldMod" MaxLength="2" onKeyUp="javascript:alphanumericOnly(this);"
                                Style="height: 30px; width: 50px;" onKeyDown="HideLabel()" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtModifierDentalSecond" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            
                            <asp:TextBox ID="txtModifierDentalThird" runat="server" onChange="return ModifierChanged(this)" CssClass="formfieldMod" MaxLength="2" onKeyUp="javascript:alphanumericOnly(this);"
                                Style="height: 30px; width: 50px;" onKeyDown="HideLabel()"/>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtModifierDentalThird" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            
                            <asp:TextBox ID="txtModifierDentalFourth" runat="server" onChange="return ModifierChanged(this)" CssClass="formfieldMod" MaxLength="2" onKeyUp="javascript:alphanumericOnly(this);"
                                Style="height: 30px; width: 50px;" onKeyDown="HideLabel()" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txtModifierDentalFourth" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                          
                        </div>

                    </div>
                    <div class="row" id="divDentalD" runat="server">
                        <div class="col-sm-4">
                            <span class="ohio-field" style="font-size: 14px; text-align: left;">Diagnosis Pointer:</span>
                        </div>
                        <div class="col-sm-8" style="padding-left: 0px;">
                            <asp:DropDownList ID="ddlDiagnosisDentalFirst" runat="server" Style="height: 30px;
                                 width: 20px; min-width: 60px;" onChange="return Dental_loader1Change()" >
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlDignosisDentalSecond" runat="server" Style="height: 30px;
                                 width: 20px; min-width: 60px;" onChange="return Dental_loader2Change()" >
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlDignosisDentalThird" runat="server" Style="height: 30px;
                                 width: 20px; min-width: 60px;" onChange="return Dental_loader3Change()" >
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlDignosisDentalFourth" runat="server" Style="height: 30px;
                                 width: 20px; min-width: 60px;" >
                            </asp:DropDownList>
                        </div>
                         <button id="btnloading2" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
        
                        <div class="form-group row" style="padding-left: 250px">
                            <asp:Label ID="lblpointerError" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <span class="ohio-field" style="font-size: 14px; text-align: left;">Oral Cavity:</span>
                        </div>
                        <div class="col-sm-8" style="padding-left: 0px;">
                            <asp:DropDownList ID="ddlOralCavityDentalFirst" runat="server"  AppendDataBoundItems="true" Style="height: 30px;width: 20px; min-width: 70px;">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlOralCavityDentalSecond" runat="server" Style="height: 30px;
                                width: 20px; min-width: 70px; display:none">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlOralCavityDentalThird" runat="server" Style="height: 30px;
                                width: 20px; min-width: 70px; display:none">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlOralCavityDentalFourth" runat="server" Style="height: 30px;
                                width: 20px; min-width: 70px; display:none">
                                                           </asp:DropDownList>
                            <asp:DropDownList ID="ddlOralCavityDentalFifth" runat="server" Style="height: 30px;
                                width: 20px; min-width: 70px; display:none">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <span class="ohio-field" style="font-size: 14px; text-align: left;">Prosthesis, Crown
                                or Inlay Code:</span>

                        </div>
                        <div class="col-sm-4" style="padding-left: 0px;">
                            <asp:DropDownList ID="ddlInlyneCodeDental" runat="server" Style="height: 30px; 
                                width: 100px; min-width: 150px;" EnableViewState="true" AppendDataBoundItems="True" class="selectdropdown">
                               
                                  <asp:ListItem Value="0" Text=""></asp:ListItem>
                                <asp:ListItem Value="I" Text="Initial Placement"></asp:ListItem>
                                <asp:ListItem Value="R" Text="Replacement"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="col-sm-2">
                    <div class="row">
                        <div class="col-sm-7">
                            <span class="ohio-field" style="font-size: 14px; text-align: left;">Status:</span>
                        </div>
                        <div class="col-sm-5" style="text-align: left; padding-left: 0;">
                            <asp:Label ID="lblStatusServiceDetails" Style="font-size: 14px;" runat="server" Text="Pending Submission"></asp:Label>

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-7">
                            <span class="ohio-field" style="font-size: 14px; text-align: left;"><span style="color: red">* </span>Charges:</span>
                        </div>
                        <div class="col-sm-5" style="text-align: left; padding-left: 0">
                            <asp:TextBox ID="txtChargesDental" runat="server" CssClass="formFieldTextBoxSmall" Style="height: 30px;
                                width: 70px; min-width: 70px;" /><br />
                            <asp:Label ID="lblChargesError" runat="server" ForeColor="Red"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4"
                                MaxLength="5"
                                ValidationGroup="validateDentalServiceControl" ControlToValidate="txtChargesDental"
                                CssClass="failureNotification"
                                Text="*Charges are required" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="revtxtChargesDental" runat="server" ControlToValidate="txtChargesDental" ValidationExpression="^(-?\d+\.)?-?\d+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />

                        </div>
                    </div>
                    <div class="row" id="divPaidAmount" runat="server">
                        <div class="col-sm-7">
                            <span class="ohio-field" style="font-size: 14px; text-align:left;">Paid Amount:</span>
                        </div>
                        <div class="col-sm-5" style="text-align: left; padding-left: 0;">
                            <asp:Label ID="lblpaidAmountDental" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-7" style="text-align: right;">
                            <span class="ohio-field" style="font-size: 14px; text-align: left;"><span style="color: red">* </span>Billed Units:</span>
                        </div>
                        <div class="col-sm-5" style="text-align: left; padding-left: 0;">
                            <asp:TextBox ID="txtBillUnitDental" runat="server" CssClass="formFieldTextBoxSmall" Style="height: 30px;
                                width: 30px; min-width: 70px;"
                                onkeypress="return event.charCode == 46 || (event.charCode >= 48 && event.charCode <= 57)" /><br />   
                             <asp:Label ID="lblErrorBilledUnits" runat="server" ForeColor="Red"></asp:Label>
                          <%--  <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator11"
                                MaxLength="5"
                                ValidationGroup="validateDentalServiceControl" ControlToValidate="txtBillUnitDental"
                                CssClass="failureNotification"
                                Text="*Billed unit is required for service line N*" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtBillUnitDental" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />--%>

                        </div>
                    </div>
                    <div class="row" id="divPaidUnit" runat="server">
                        <div class="col-sm-7" style="text-align: right;">
                            <span class="ohio-field" style="font-size: 14px; padding-left: 10px; text-align: left;">
                                Paid Units:</span>

                        </div>
                        <div class="col-sm-5" style="text-align: left; padding-left: 0;">
                            <asp:Label ID="lblPaidUnitDental" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-sm-1">
                    <!--put all buttons here -->
                    <div class="row">
                        <div class="col-sm-12" id="dentalSerDiv" runat="server">
                            <asp:Button ID="btnServiceDetailsDentailAdd" Text="ADD" OnClientClick="return addDentalServiceDetailCode()" runat="server" CssClass="btn btn-primary" Font-Bold="True"  Width="70px" />
                           <br />
                            <asp:Button ID="btnUpdateServiceDetailDental" Enabled="true" Style="visibility: hidden" Text="Update" OnClientClick="return UpdatedentalServiceDetail()" runat="server" CssClass="btn btn-primary" Font-Bold="True"  Width="70px" />
                             <br />
                            <asp:Button ID="btnCancelDental" Enabled="true" Style="visibility: hidden" Text="Cancel" OnClientClick="return clearallfieldsDentalSetrvicedetail()" runat="server" CssClass="btn btn-primary" Font-Bold="True"  Width="70px" />
                             <button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
        
                            <%--CausesValidation="true" ValidationGroup="validateDentalServiceControl"--%>
                            <br />

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                             
                           <br />
                            <br />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <%-- 
                             --%>
                            
                           <%-- <asp:Button ID="btnCancelDental" Text="Cancel" runat="server" CssClass="btn btn-danger"
                                Font-Bold="True" OnClick="btnCancelDental_Click" Width="70px" Visible="false" CausesValidation="false" />--%>
                            <br />
                            <br />
                        </div>
                    </div>
                </div>
                <!--base coloumns for the dental service detail end-->
            </div>



            <br />
            <div class="row" style="background-color: lightsteelblue; margin-top: 0px; height: 35px;
                margin-left: 5px;"
                hidden="hidden">


                <div class="col-sm-1">
                    <span class="ohio-field" style="font-size: 15px; text-align: left"><strong>Detail</strong></span>
                    <div style="text-align: left;">
                        <asp:TextBox ID="txtDentalDetail" runat="server" CssClass="formFieldTextBox" Style="background-color: lightgrey;
                            height: 28px; width: 90px; min-width: 90px;"
                            ReadOnly="True" />
                    </div>
                </div>

                <div class="col-sm-2">
                    <span class="ohio-field" style="font-size: 15px; text-align: left"><strong>*Procedure
                        Code</strong></span>
                    <div class="text-left">
                        <asp:TextBox ID="txtProcedureCode" runat="server" CssClass="formFieldTextBoxSmall" Style="height: 30px;
                            width: 90px; min-width: 90px;" />


                    </div>

                </div>


                <div class="col-sm-1">
                    <span class="ohio-field" style="font-size: 15px; text-align: left"><strong>&nbsp;*Billed&nbsp;Units</strong></span>
                    <div style="text-align: left;">
                        <asp:TextBox ID="txtBilledUnits" runat="server" CssClass="formFieldTextBox" Style="height: 30px;
                            width: 90px; min-width: 90px;" />

                    </div>
                </div>

                <div class="col-sm-1">
                    <span class="ohio-field" style="font-size: 15px; text-align: left"><strong>Paid Units</strong></span>
                    <div style="text-align: left;">
                        <asp:TextBox ID="txtPaidUnits" runat="server" CssClass="formFieldTextBox" Style="background-color: lightgrey;
                            height: 30px; width: 90px; min-width: 90px;"
                            ReadOnly="True" />
                    </div>
                </div>

                <div class="col-sm-2">
                    <span class="ohio-field" style="font-size: 15px; text-align: left"><strong>*Date of
                        Services</strong></span>
                    <div class="text-left">
                        <asp:TextBox ID="txtdateofserviceold" runat="server" CssClass="formFieldTextBox" Style="height: 30px;
                            width: 100px; min-width: 100px;" />
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp"
                            Height="16px" AlternateText="Calendar Icon" />
                        <ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="txtdateofserviceold"
                            runat="server" />

                    </div>
                </div>

                <div class="col-sm-1">
                    <span class="ohio-field" style="font-size: 15px; text-align: left"><strong>*Charges</strong></span>
                    <div style="text-align: left;">
                        <asp:TextBox ID="txtChargesold" runat="server" Style="height: 30px; width: 90px; min-width: 90px;"
                            CssClass="formFieldTextBox" />

                    </div>
                </div>

                <div class="col-sm-1">
                    <span class="ohio-field" style="font-size: 15px; text-align: left"><strong>Paid&nbsp;Amount</strong></span>
                    <div style="text-align: left;">
                        <asp:TextBox ID="txtdentalPaidAmount" runat="server" Style="background-color: lightgrey;
                            height: 30px; width: 90px; min-width: 90px;"
                            CssClass="formFieldTextBox" ReadOnly="true" />
                    </div>
                </div>

                <div class="col-sm-1">
                    <span class="ohio-field" style="font-size: 15px; text-align: left"><strong>Status</strong></span>
                    <div style="text-align: left;">
                        <asp:TextBox ID="txtclaimsStatus" runat="server" Style="background-color: lightgrey;
                            height: 30px; width: 90px; min-width: 90px;"
                            CssClass="formFieldTextBox" ReadOnly="true" />
                    </div>
                </div>

                <div class="col-sm-1">
                    <span class="ohio-field" style="font-size: 15px; text-align: left">&nbsp;</span>
                    <div style="text-align: left;">
                    </div>
                </div>
            </div>
            <div class="row" style="height: 30px; margin-left: 5px;" hidden="hidden">
                <div class="col-sm-7">&nbsp;</div>
                <div class="col-sm-1">
                    <span class="ohio-field" style="font-size: 15px; text-align: left"><strong>Total&nbsp;Charges</strong>
                        <asp:TextBox ID="txttotalCharges" runat="server" Style="background-color: lightgrey;
                            height: 30px; width: 90px; min-width: 90px;"
                            CssClass="formFieldTextBox" ReadOnly="true" />
                    </span>
                </div>
                <div class="col-sm-1">
                    <span class="ohio-field" style="font-size: 15px; text-align: left"><strong>Total&nbsp;Paid</strong>
                        <asp:TextBox ID="txttotalamountpaid" runat="server" Style="background-color: lightgrey;
                            height: 30px; width: 90px; min-width: 90px;"
                            CssClass="formFieldTextBox" ReadOnly="true" />
                    </span>
                </div>
                <div class="col-sm-1">&nbsp;</div>
                <div class="col-sm-1">&nbsp;</div>
            </div>
            <div>
                
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<ajax:ModalPopupExtender ID="mpeSubmitClaimSearchProc" BehaviorID="mperProcCode" runat="server" PopupControlID="pnlSubmitClaimSearchProcPop"
    TargetControlID="ButtonSearchProc" BackgroundCssClass="modalBackground" CancelControlID="btnCloseProc" />
<asp:Panel ID="pnlSubmitClaimSearchProcPop" runat="server" CssClass="modalPopup"
    Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
    <asp:Panel ID="pnlSubmitClaimSearchProcHeader" CssClass="popHeader" runat="server"
        HorizontalAlign="Left">
        <asp:Button runat="server" ID="btnCloseProc" Text="X" Style="float: right; background-image: none;
            border: 0px; border-radius: 0px;"
            OnClick="btnCloseProc_Click"
            CausesValidation="false" />
        <div class="search-Results">
            <span style="padding-left: 10px; font-size: 17px;">PROCEDURE CODE</span>
            <span style="padding-left: 70px; font-size: 17px;">PROCEDURE CODE DESCRIPTION</span>
        </div>

    </asp:Panel>
    <asp:Panel ID="pnlSubmitClaimSearchProc" runat="server">
        <div style="text-align: left; padding: 15px" class="container-fluid">
            <div class="row">
                <uc:SubmitClaimSearchProc runat="server" ID="ucSubmitClaimSearchProc" Visible="true"
                    EnableViewState="true" />

            </div>
        </div>
    </asp:Panel>
</asp:Panel>
<asp:Button runat="server" ID="ButtonSearchProc" Style="display: none" Text="ButtonSearchReson" />

<ajax:ModalPopupExtender ID="mpeSubmitClaimSearchPop" BehaviorID="mperPlaceOfServiceCode" runat="server" PopupControlID="pnlSubmitClaimSearchPop"
    TargetControlID="Button9" BackgroundCssClass="modalBackground" CancelControlID="btnCloseCH9" />

<asp:Panel ID="pnlSubmitClaimSearchPop" runat="server" CssClass="modalPopup" Style="display: none;
    min-height: 250px; min-width: 610px; height: auto; width: auto;">
    <asp:Panel ID="pnlSubmitClaimSearchPopHeader" CssClass="popHeader" runat="server"
        HorizontalAlign="Left">
        <asp:Button runat="server" ID="btnCloseCH9" Text="X" Style="float: right; background-image: none;
            border: 0px; border-radius: 0px;"
            CausesValidation="false" />
        <div class="search-Results">
            <span style="padding-left: 10px; font-size: 17px;">CODE</span>
            <span style="padding-left: 250px; font-size: 17px;">PLACE OF SERVICE</span>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlSubmitClaimSearch" runat="server">
        <div style="text-align: left; padding: 15px" class="container-fluid">
            <div class="row">
                <uc:SubmitClaimSearchPop runat="server" ID="ucSubmitClaimSearchPop" Visible="true"
                    EnableViewState="true" />
            </div>
        </div>

    </asp:Panel>
</asp:Panel>
<asp:Button runat="server" ID="Button9" Style="display: none" Text="ButtonDummy9" />
<input type="hidden" value="" id="hdnoldDiagnosisPointerDentalValue">
<input type="hidden" value="" id="hdnoldDiagnosisPointerDentalValue2">
<input type="hidden" value="" id="hdnoldDiagnosisPointerDentalValue3">
<asp:HiddenField ID="hdnProviderTypeId" runat="server" />
