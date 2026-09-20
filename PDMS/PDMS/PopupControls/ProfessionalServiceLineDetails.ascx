<%@ Control Language="C#" AutoEventWireup="true" Inherits="ProfessionalServiceLineDetails" Codebehind="ProfessionalServiceLineDetails.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchProc.ascx" TagPrefix="uc" TagName="SubmitClaimSearchProc" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchPop.ascx" TagPrefix="uc" TagName="SubmitClaimSearchPop" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">

<script type="text/javascript">
    var billedUnitsErrMsg = "*Prior Authorization Number from Prior Autorization & Referral Information panel is required when total Billed Units is greater than 48.";
    var maxBilledUnits = 48;
    const duolaProcCodes = ["T1032", "T1033"];
    $(document).ready(function () {
        var clmstatus = $("#ctl00_MainContent_uc5SubmitClaim_txtstatus2").val();
        if (clmstatus != null && clmstatus != undefined && clmstatus != "") {
            $("#<%= lblProfStatus.ClientID %>").html(clmstatus);
        }
        $("#<%=ddlDiagnosisPointer1.ClientID %>").attr("disabled", true);
        $("#<%=ddlDiagnosisPointer2.ClientID %>").attr("disabled", true);
        $("#<%=ddlDiagnosisPointer3.ClientID %>").attr("disabled", true);
        $("#<%=ddlDiagnosisPointer4.ClientID %>").attr("disabled", true);
    });

    var codeType = "PlaceofService";
    var claimsOrPA = "Claims";
    var claimType = "Professional";
    var NpiElement = document.getElementById("ctl00_MainContent_ucRegProgressBar_lblPRONPI2");
    var MedIdElement = document.getElementById("ctl00_MainContent_ucRegProgressBar_lblProMedicaidID2");
    var providerTypeId = $("[id*=hdnProviderTypeId]").val();
    // Retrieve the text content inside the span
    var Npi = NpiElement.textContent || NpiElement.innerText;
    var MedId = MedIdElement.textContent || MedIdElement.innerText;
    var isValidPlaceCode = true;

    function displayprofessionalservicedetailtable(copied_Service_Line = 0, action="Add", control) {
        var claimid = document.getElementById("<%=hdnValueCode_ClaimID.ClientID %>").value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetClaimProfessionalServiceDetail?claimid=" + claimid + "",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                servicedetailprofessionalList = result;
                var table;
                var totalcharge = 0;
                var totalpaidamount = 0;
                var rowcount = 0;
                var total_BilledUnits = 0;
                if (servicedetailprofessionalList.length > 0) {
                    $("#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ddlNDCServiceLine").empty();
                    $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlAdditonalProviderDetail").empty();
                    $("#ctl00_MainContent_uc5SubmitClaim_ucAmbulancePickUpDropOff_ddlAmbulanceServiceLine").empty();
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlDetailId").empty();
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlServiceLine").empty();

                    $("#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ddlNDCServiceLine").append($("<option> </option>"));
                    $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlAdditonalProviderDetail").append($("<option> </option>"));
                    $("#ctl00_MainContent_uc5SubmitClaim_ucAmbulancePickUpDropOff_ddlAmbulanceServiceLine").append($("<option> </option>"));
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlDetailId").append($("<option> </option>"));
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlServiceLine").append($("<option> </option>"));
                    table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>*Procedure Code</th><th style='width:10px; scope='col'>Place Of Service</th><th style='width:10px; scope='col'>*Billed Units</th><th style='width:10px; scope='col'>Paid Units</th><th style='width:10px; scope='col'>Date Of Service</th><th style='width:10px; scope='col'>Charges</th><th style='width:10px; scope='col'>Paid Amount</th><th style='width:10px; scope='col'>Status</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    var j = 0;
                    for (var i = 0; i < servicedetailprofessionalList.length; i++) {
                        var sequence = i;
                        sequence++;
                        rowcount++;
                        document.getElementById('<%= hdnServiceLines.ClientID %>').value = rowcount;
                        var cde_proc = result[i].cde_proc;
                        var plc_service = result[i].plc_service;
                        var bil_unt = result[i].bil_unt;
                        total_BilledUnits = total_BilledUnits + Number(bil_unt);
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
                        pad_amnt = HandleDecimalValue(pad_amnt).toString();
                        var cde_clm_status = result[i].cde_clm_status;
                        var Claim_service_Id = result[i].Claim_service_Id;
                        var Claim_Id = result[i].Claim_Id;
                        var service_line = result[i].service_line;
                        totalcharge = totalcharge + parseFloat(cde_clm_chrge);
                        totalpaidamount = totalpaidamount + parseFloat(pad_amnt);
                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + cde_proc + "</span></td><td><span  title='Line' class='tNumber'>" + plc_service + "</span></td><td><span title='Line' class='tNumber'>" + bil_unt + "</span></td><td>" + pad_unt + "</td><td>" + ServiceDate + "</td><td>" + cde_clm_chrge + "</td><td>" + pad_amnt + "</td><td>" + cde_clm_status + "</td><td><input type='button' value = 'Edit' onClick = 'return EditProfessionalServiceLineItem(\"" + Claim_service_Id + "\",\"" + Claim_Id + "\",\"" + service_line + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value = 'Copy' onClick = 'return EditProfessionalServiceLineItem(\"" + Claim_service_Id + "\",\"" + Claim_Id + "\",\"" + service_line + "\",this, 1); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteProfessionalServiceDetailLineitem(\"" + Claim_Id + "\",\"" + service_line + "\",this);' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        // add service line to Ambulance PickUp/Drop Off panel, no method in panel itself
                        $("#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ddlNDCServiceLine").append($("<option></option>").val(service_line).html(service_line));
                        $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlAdditonalProviderDetail").append($("<option></option>").val(service_line).html(service_line));
                        $("#ctl00_MainContent_uc5SubmitClaim_ucAmbulancePickUpDropOff_ddlAmbulanceServiceLine").append($("<option></option>").val(service_line).html(service_line));
                        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlDetailId").append($("<option></option>").val(service_line).html(service_line));
                        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlServiceLine").append($("<option></option>").val(service_line).html(service_line));
                        j = sequence;

                    };
                    j++;
                    $("#<%= lblDetailsItemProf.ClientID %>").html("" + j + "");

                    table = table + "</tbody></table>";
                    localStorage.setItem("ProfessionalServiceDetailTable", "" + table + "");
                    document.getElementById('<%= upProfServiceLineDetails1.ClientID %>').innerHTML = table;

                    $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_hdnTotalBilledUnits").val(total_BilledUnits);

                    clearallfieldsProfessionalSetrvicedetail();
                    if (document.getElementById('<%= ProfInfoAdd.ClientID%>') != null) {
                        if (rowcount >= 49) {
                            document.getElementById('<%= ProfInfoAdd.ClientID%>').style.visibility = "hidden";
                        }
                        else {
                            document.getElementById('<%= ProfInfoAdd.ClientID%>').style.visibility = "visible";
                        }
                    }
                    $("#<%= lblProfTotalCharges.ClientID %>").html("" + totalcharge.toFixed(2) + "");
                    if (isNaN(totalpaidamount)) {
                        $("#<%= lblProfTotalAmountPaid.ClientID %>").html("" + 0.00 + "");
                    }
                    else {
                        $("#<%= lblProfTotalAmountPaid.ClientID %>").html("" + totalpaidamount + "");
                    }
                }
                else {
                    document.getElementById('<%= upProfServiceLineDetails1.ClientID %>').innerHTML = "";
                    $("#<%= lblDetailsItemProf.ClientID %>").html("" + 01 + "");
                    $("#<%= lblProfTotalCharges.ClientID %>").html("" + totalcharge.toFixed(2) + "");
                    if (isNaN(totalpaidamount)) {
                        $("#<%= lblProfTotalAmountPaid.ClientID %>").html("" + 0.00 + "");
                    }
                    else {
                        $("#<%= lblProfTotalAmountPaid.ClientID %>").html("" + 0.00 + "");
                    }
                    $("#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ddlNDCServiceLine").empty();
                    $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlAdditonalProviderDetail").empty();
                    $("#ctl00_MainContent_uc5SubmitClaim_ucAmbulancePickUpDropOff_ddlAmbulanceServiceLine").empty();
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlDetailId").empty();
                    $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlServiceLine").empty();
                }
                bindAdditionalProviderInformationserviceDetail();
                GetOtherPayerPaidAmountBind();
                otherPayerAdjSerDetailBind();
                ndcPanelBind();
                if (document.getElementById('<%= hdnServiceLines.ClientID %>').value < 50) {
                    if (action == "Add") {
                        if (copied_Service_Line == 0) {
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML = "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$ProfInfoAdd' value='ADD' onclick='return ProfInfoAdd_Click();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProfInfoAdd' class='btn btn-primary' style='font-weight:bold;width:90px; visibility: visible;'>";
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$btnUpdateServiceDetailProfessional' value='Update' onclick='return UpdateProfessionalServiceDetail();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_btnUpdateServiceDetailProfessional' class='btn btn-primary' style='font-weight: bold; width: 90px; visibility: hidden;'>";
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$btncancelProfessional' value='Cancel' onclick='clearallfieldsProfessionalSetrvicedetail(); return displayprofessionalservicedetailtable();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_btncancelProfessional' class='btn btn-primary' style='font-weight: bold; width: 90px; visibility: visible;'>";
                        } else {
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML = "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$ProfInfoAdd' value='ADD' onclick='return ProfInfoAdd_Click();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProfInfoAdd' class='btn btn-primary' style='font-weight:bold;width:90px; visibility: visible;'>";
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$btnUpdateServiceDetailProfessional' value='Update' onclick='return UpdateProfessionalServiceDetail();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_btnUpdateServiceDetailProfessional' class='btn btn-primary' style='font-weight: bold; width: 90px; visibility: hidden;'>";
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$btncancelProfessional' value='Cancel' onclick='clearallfieldsProfessionalSetrvicedetail(); return displayprofessionalservicedetailtable();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_btncancelProfessional' class='btn btn-primary' style='font-weight: bold; width: 90px; visibility: visible;'>";
                            EditProfessionalServiceLineItem(1, claimid, copied_Service_Line, control);
                        }
                    } else {
                        if (copied_Service_Line == 0) {
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML = "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$ProfInfoAdd' value='ADD' onclick='return ProfInfoAdd_Click();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProfInfoAdd' class='btn btn-primary' style='font-weight:bold;width:90px; visibility: visible;'>";
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$btnUpdateServiceDetailProfessional' value='Update' onclick='return UpdateProfessionalServiceDetail();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_btnUpdateServiceDetailProfessional' class='btn btn-primary' style='font-weight: bold; width: 90px; visibility: hidden;'>";
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$btncancelProfessional' value='Cancel' onclick='clearallfieldsProfessionalSetrvicedetail(); return displayprofessionalservicedetailtable();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_btncancelProfessional' class='btn btn-primary' style='font-weight: bold; width: 90px; visibility: visible;'>";
                        } else {
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML = "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$ProfInfoAdd' value='ADD' onclick='return ProfInfoAdd_Click();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProfInfoAdd' class='btn btn-primary' style='font-weight:bold;width:90px; visibility: visible;'>";
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$btnUpdateServiceDetailProfessional' value='Update' onclick='return UpdateProfessionalServiceDetail();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_btnUpdateServiceDetailProfessional' class='btn btn-primary' style='font-weight: bold; width: 90px; visibility: hidden;'>";
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$btncancelProfessional' value='Cancel' onclick='clearallfieldsProfessionalSetrvicedetail(); return displayprofessionalservicedetailtable();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_btncancelProfessional' class='btn btn-primary' style='font-weight: bold; width: 90px; visibility: visible;'>";
                            EditProfessionalServiceLineItem(1, claimid, copied_Service_Line, control);
                        }
                    }
                } else {
                    document.getElementById("<%= profSerDiv.ClientID%>").innerHTML = "";
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblmessage]").text('Service line code not found.');
                alert('Failed Loading GetProfessionalServiceDetail.  Error: ' + errorThrown)
            }
        });

        return false;
    };

    function ReadonlyFields() {
        document.getElementById('<%= txtServiceProcedureCode.ClientID %>').disabled = true;
        document.getElementById('<%= txtModifier1.ClientID %>').disabled = true;
        document.getElementById('<%= txtModifier2.ClientID %>').disabled = true;
        document.getElementById('<%= txtModifier3.ClientID %>').disabled = true;
        document.getElementById('<%= txtModifier4.ClientID %>').disabled = true;
        document.getElementById('<%= txtProfPlaceOfService.ClientID %>').disabled = true;
        document.getElementById('<%= txtProffdateofservice.ClientID %>').disabled = true;
        document.getElementById('<%= txtProffdateofservice.ClientID %>').disabled = true;
        document.getElementById('<%= txtCharges.ClientID %>').disabled = true;
        document.getElementById('<%= txtBilledUnits.ClientID %>').disabled = true;
        document.getElementById('<%= txtLineCntrlNumber.ClientID %>').disabled = true;
        document.getElementById('<%= ddlDiagnosisPointer1.ClientID %>').disabled = true;
        document.getElementById('<%= ddlDiagnosisPointer2.ClientID %>').disabled = true;
        document.getElementById('<%= ddlDiagnosisPointer3.ClientID %>').disabled = true;
        document.getElementById('<%= ddlDiagnosisPointer4.ClientID %>').disabled = true;
        document.getElementById('<%= txtPriorAuthNumber.ClientID %>').disabled = true;
        document.getElementById('<%= ddlEpsdtService.ClientID %>').disabled = true;
        document.getElementById('<%= txtReferralNumber.ClientID %>').disabled = true;
        document.getElementById('<%= ddlFamilyPlanning.ClientID %>').disabled = true;
        document.getElementById('<%= ddlDMECertType.ClientID %>').disabled = true;
        document.getElementById('<%= ddlEmergency.ClientID %>').disabled = true;
        document.getElementById('<%= txtDuration.ClientID %>').disabled = true;
    }


    function clearProfessionalServiceDetailFields() {
        $("#<%= txtServiceProcedureCode.ClientID %>").first().val("");
        $("#<%= txtProfPlaceOfService.ClientID %>").first().val("");
        $("#<%= txtProffdateofservice.ClientID %>").first().val("");
        $("#<%= txtCharges.ClientID %>").first().val("");
        $("#<%= txtBilledUnits.ClientID %>").first().val("");
        $("#<%= txtDuration.ClientID %>").first().val("");
        $("#<%= txtLineCntrlNumber.ClientID %>").first().val("");
        $("#<%= txtPriorAuthNumber.ClientID %>").first().val("");
        $("#<%= txtReferralNumber.ClientID %>").first().val("");
        $("#<%= txtCertRev.ClientID %>").first().val("");
        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblBilledUnitsMsgError").text('');
        document.getElementById("ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDMECertType").options[0].selected = true;
        document.getElementById("ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlUnitsOfMeasurement").options[0].selected = true;
        document.getElementById("ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlEpsdtService").options[0].selected = true;
        document.getElementById("ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlFamilyPlanning").options[0].selected = true;
        document.getElementById("ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlEmergency").options[0].selected = true;
        var diag1Length = $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1 option").length;
        if (diag1Length > 1) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").options[0].selected = true;
        }
        diag2Length = $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2 option").length;
        if (diag2Length > 1) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").options[0].selected = true;
        }
        diag3Length = $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3 option").length;
        if (diag3Length > 1) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").options[0].selected = true;
        }
        diag4Length = $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4 option").length;
        if (diag4Length > 1) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").options[0].selected = true;
        }
        $("#<%= txtModifier1.ClientID %>").first().val("");
        $("#<%= txtModifier2.ClientID %>").first().val("");
        $("#<%= txtModifier3.ClientID %>").first().val("");
        $("#<%= txtModifier4.ClientID %>").first().val("");
        $("#<%= lblProfTotalAmountPaid.ClientID %>").html("");
        $("#<%= lblProfPaidUnits.ClientID %>").html("");
        $("#hdnoldDiagnosisPointerValue").val("");
        $("#hdnoldDiagnosisPointerValue2").val("");
        $("#hdnoldDiagnosisPointerValue3").val("");

        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").attr("disabled", false);
        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").attr("disabled", true);
        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").attr("disabled", true);
        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").attr("disabled", true);

        oldDiagnosisPointerValue = "";
        oldDiagnosisPointerValue2 = "";
        oldDiagnosisPointerValue3 = "";
        oldDiagnosisPointerValue4 = "";
        ReloadDiagnosisPointersProf();
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
                number = dec_value[0] + "." + dec_value[1];
                return number;
            }
        }
    }

    function validateProfessionalProcedureCodeModifiers() {
        var txtProcCode = $("#<%= txtServiceProcedureCode.ClientID %>").first().val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        var providerTypeId = $("[id*=hdnProviderTypeId]").val();
        var Modifier1 = $("#<%= txtModifier1.ClientID %>").first().val();
        var Modifier2 = $("#<%= txtModifier2.ClientID %>").first().val();
        var Modifier3 = $("#<%= txtModifier3.ClientID %>").first().val();
        var Modifier4 = $("#<%= txtModifier4.ClientID %>").first().val();
        var validationResult = true;

        $.ajax({
            type: "GET",
            url: webApiClaims + "validateProcedureCodeModifiers?provider_type_id=" + providerTypeId + "&&procedure_code=" + txtProcCode + "&&M1=" + Modifier1 + "&&M2=" + Modifier2 + "&&M3=" + Modifier3 + "&&M4=" + Modifier4 + "&&claimsorPA=claims&&claimORPAType=Professional",
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

                if (result.length > 0) {
                    $("[id*=lblErrorMessageonTop]").text(result);
                    validationResult = false;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMessageonTop]").text(result);
                validationResult = false;
            }
        });
        return validationResult;
    }

    function validateProfessionalServiceDetails() {

        var ProcedureCode = $("#<%= txtServiceProcedureCode.ClientID %>").first().val();
        var DateOfService = $("#<%= txtProffdateofservice.ClientID %>").first().val();
        var DMECertType = $("#<%=ddlDMECertType.ClientID %> option:selected").text();
        var DMEDuration = $("#<%= txtDuration.ClientID %>").first().val();
        var ProfPlaceOfService = $("#<%= txtProfPlaceOfService.ClientID %>").first().val();
        var Modifier1 = $("#<%= txtModifier1.ClientID %>").first().val();
        var Modifier2 = $("#<%= txtModifier2.ClientID %>").first().val();
        var Modifier3 = $("#<%= txtModifier3.ClientID %>").first().val();
        var Modifier4 = $("#<%= txtModifier4.ClientID %>").first().val();
        var DiagnosisPointer1 = $("#<%=ddlDiagnosisPointer1.ClientID %> option:selected").text();
        var DiagnosisPointer2 = $("#<%=ddlDiagnosisPointer2.ClientID %> option:selected").text();
        var DiagnosisPointer3 = $("#<%=ddlDiagnosisPointer3.ClientID %> option:selected").text();
        var DiagnosisPointer4 = $("#<%=ddlDiagnosisPointer4.ClientID %> option:selected").text();
        var PriorAuth = $("#<%= txtPriorAuthNumber.ClientID %>").first().val();
        var Refralnumber = $("#<%= txtReferralNumber.ClientID %>").first().val();
        var Charges = $("#<%= txtCharges.ClientID %>").first().val();
        var Linenumber = $("#<%= txtLineCntrlNumber.ClientID %>").first().val();
        var UnitsOfMeasurement = $("#<%=ddlUnitsOfMeasurement.ClientID %> option:selected").text();
        var BilledUnits = $("#<%= txtBilledUnits.ClientID %>").first().val();
        var lblDetailsItemProf = $("#<%= lblDetailsItemProf.ClientID %>").html();
        var paidunits = $("#<%=lblProfPaidUnits.ClientID %>").html();
        var paidAmount = $("#<%=lblProfTotalAmountPaid.ClientID %>").html();
        var status = $("#<%=lblProfStatus.ClientID %>").html();
        var Referred_EPSDT_Service = $("#<%=ddlEpsdtService.ClientID %> option:selected").text();
        var Family_Planning = $("#<%=ddlFamilyPlanning.ClientID %> option:selected").text();
        var Emergency = $("#<%=ddlEmergency.ClientID %> option:selected").text();
        var Final_EAPG = $("#<%=lblProfFinalEAPG.ClientID %>").html();
        var Payment_Action = $("#<%=lblProfPaymentAction.ClientID %>").html();
        var Paid_Amount = $("#<%=lblProfPaidAmount.ClientID %>").html();
        var Total_Charges = $("#<%=lblProfTotalCharges.ClientID %>").html();
        var Cert_Revision = $("#<%= txtCertRev.ClientID %>").first().val();

        var validationResult = true;
        if ((ProcedureCode === null || ProcedureCode === "" || ProcedureCode === undefined)) {
            $("[id*=lblErrorMessageonTop]").text('*Procedure code is required');
            validationResult = false;
        }
        else {
            var validation = false;
            validation = ProcedureCode.startsWith("MR");
            if (validation != true) {
                validation = ProcedureCode.startsWith("DD");
            }
            if (validation != true) {
                validation = ProcedureCode.startsWith("PNM");
            }
            if (validation === true) {
                $("[id*=lblErrorMessageonTop]").text('*Procedure code is invalid');
                validationResult = false;
            }
        }
        if ((DateOfService === null || DateOfService === "" || DateOfService === undefined)) {
            $("#<%= lblErrorDOS.ClientID %>").html("*Date Of Service is required");
            validationResult = false;
        }
        else {
            $("#<%= lblErrorDOS.ClientID %>").html("");
        }
        if ((ProfPlaceOfService === null || ProfPlaceOfService === "" || ProfPlaceOfService === undefined)) {
            $("#<%= errPlaceOfService.ClientID %>").html("*Place Of Service is required");
            validationResult = false;
        }
        else {
            $("#<%= errPlaceOfService.ClientID %>").html("");
        }
        if ((DiagnosisPointer1 === null || DiagnosisPointer1 === "" || DiagnosisPointer1 === undefined)
            && (DiagnosisPointer2 === null || DiagnosisPointer2 === "" || DiagnosisPointer2 === undefined)
            && (DiagnosisPointer3 === null || DiagnosisPointer3 === "" || DiagnosisPointer3 === undefined)
            && (DiagnosisPointer4 === null || DiagnosisPointer4 === "" || DiagnosisPointer4 === undefined)) {
            $("#<%= lblpointerError.ClientID %>").html("*Please select diagnosis pointer's first dropdown.");
            validationResult = false;
        }
        else {
            $("#<%= lblpointerError.ClientID %>").html("");
        }

        if (Charges === null || Charges === "" || Charges === undefined) {
            $("#<%= txtchargeserror.ClientID %>").html("*Charges is required");
            validationResult = false;
        }
        else {
            $("#<%= txtchargeserror.ClientID %>").html("");
        }

        if (BilledUnits === null || BilledUnits === "" || BilledUnits === undefined) {
            $("#<%= lblBilledUnitsError.ClientID %>").html("*Billed unit is required for service line N*");
            validationResult = false;
        }
        else if ($('#ctl00_MainContent_uc5SubmitClaim_ucPriorAuthorizationAndReferringPanel_txtPriorAuthNumber').val() === "") {
            var duolaCodefound = $.inArray(ProcedureCode, duolaProcCodes) > -1;
            if (duolaCodefound) {

                var totalBilledUnits = $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_hdnTotalBilledUnits").val();
                var totalUnits = Number(BilledUnits);
                if (totalBilledUnits !== "") {
                    totalUnits = Number(BilledUnits) + Number(totalBilledUnits);
                }
                if (BilledUnits > maxBilledUnits || (totalBilledUnits !== "" && totalUnits > maxBilledUnits)) {
                    $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblBilledUnitsMsgError").text(billedUnitsErrMsg);
                    validationResult = false;
                }
                else {
                    $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblBilledUnitsMsgError").text('');
                }
            }
        }
        else {
            $("#<%= lblBilledUnitsError.ClientID %>").html("");
        }
        if (UnitsOfMeasurement === null || UnitsOfMeasurement === "" || UnitsOfMeasurement === undefined) {
            $("#<%= lblUnitOfMeasurementError.ClientID %>").html("*Unit of Measurement is required");
            validationResult = false;
        }
        else {
            $("#<%= lblUnitOfMeasurementError.ClientID %>").html("");
        }
        if ((Modifier1 != null && Modifier1 != "" && Modifier1 != undefined)) {
            if (Modifier1.length != 2) {
                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorMessageonTop").text('*Procedure code modifier must be 2 characters.');
                validationResult = false;
            }
        }
        if ((Modifier2 != null && Modifier2 != "" && Modifier2 != undefined)) {
            if (Modifier2.length != 2) {
                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorMessageonTop1").text('*Procedure code modifier must be 2 characters.');
                validationResult = false;
            }
        }
        if ((Modifier3 != null && Modifier3 != "" && Modifier3 != undefined)) {
            if (Modifier3.length != 2) {
                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorMessageonTop1").text('*Procedure code modifier must be 2 characters.');
                validationResult = false;
            }
        }
        if ((Modifier4 != null && Modifier4 != "" && Modifier4 != undefined)) {
            if (Modifier4.length != 2) {
                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorMessageonTop1").text('*Procedure code modifier must be 2 characters.');
                validationResult = false;
            }
        }
        if ((BilledUnits != null && BilledUnits != "" && BilledUnits != undefined)) {
            if (BilledUnits <= 0) {
                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorMessageonTop1").text('*Billed units should be greater than zero.');
                validationResult = false;
            }
        }
        else {
            var topErr = $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorMessageonTop1").text();
            if (topErr == "" || topErr == null || topErr == undefined) {
                validationResult = false;

            } else {
                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorMessageonTop1").text('');
                $("#<%= lblBilledUnitsError.ClientID %>").html("");
            }
        }
        return validationResult;
    }

    function ProfInfoAdd_Click() {
        var claimStatus = $("#ctl00_MainContent_uc5SubmitClaim_txtstatus2").val();
        var claimid = document.getElementById("<%=hdnValueCode_ClaimID.ClientID %>").value;
        var ProcedureCode = $("#<%= txtServiceProcedureCode.ClientID %>").first().val();
        var DateOfService = $("#<%= txtProffdateofservice.ClientID %>").first().val();
        var DMECertType = $("#<%=ddlDMECertType.ClientID %> option:selected").text();
        var DMEDuration = $("#<%= txtDuration.ClientID %>").first().val();
        var ProfPlaceOfService = $("#<%= txtProfPlaceOfService.ClientID %>").first().val();
        var Modifier1 = $("#<%= txtModifier1.ClientID %>").first().val();
        var Modifier2 = $("#<%= txtModifier2.ClientID %>").first().val();
        var Modifier3 = $("#<%= txtModifier3.ClientID %>").first().val();
        var Modifier4 = $("#<%= txtModifier4.ClientID %>").first().val();
        var DiagnosisPointer1 = $("#<%=ddlDiagnosisPointer1.ClientID %> option:selected").text();
        var DiagnosisPointer2 = $("#<%=ddlDiagnosisPointer2.ClientID %> option:selected").text();
        var DiagnosisPointer3 = $("#<%=ddlDiagnosisPointer3.ClientID %> option:selected").text();
        var DiagnosisPointer4 = $("#<%=ddlDiagnosisPointer4.ClientID %> option:selected").text();
        var PriorAuth = $("#<%= txtPriorAuthNumber.ClientID %>").first().val();
        var Refralnumber = $("#<%= txtReferralNumber.ClientID %>").first().val();
        var Charges = $("#<%= txtCharges.ClientID %>").first().val();
        var storedcharge = Charges;

        Charges = HandleDecimalValue(Charges).toString();
        if (Charges == null || Charges == "" || Charges == undefined) {
            if (storedcharge == "0" || storedcharge == "0.0" || storedcharge == "0.00" || storedcharge == "0.000") {
                Charges = "0.00";
            }
        }
        var Linenumber = $("#<%= txtLineCntrlNumber.ClientID %>").first().val();
        var UnitsOfMeasurement = $("#<%=ddlUnitsOfMeasurement.ClientID %> option:selected").text();
        var BilledUnits = $("#<%= txtBilledUnits.ClientID %>").first().val();
        var lblDetailsItemProf = $("#<%= lblDetailsItemProf.ClientID %>").html();
        var paidunits = $("#<%=lblProfPaidUnits.ClientID %>").html();
        var paidAmount = $("#<%=lblProfTotalAmountPaid.ClientID %>").html();
        var status = $("#<%=lblProfStatus.ClientID %>").html();
        var Referred_EPSDT_Service = $("#<%=ddlEpsdtService.ClientID %> option:selected").text();
        var Family_Planning = $("#<%=ddlFamilyPlanning.ClientID %> option:selected").text();
        var Emergency = $("#<%=ddlEmergency.ClientID %> option:selected").text();
        var Final_EAPG = $("#<%=lblProfFinalEAPG.ClientID %>").html();
        var Payment_Action = $("#<%=lblProfPaymentAction.ClientID %>").html();
        var Paid_Amount = $("#<%=lblProfPaidAmount.ClientID %>").html();
        var Total_Charges = $("#<%=lblProfTotalCharges.ClientID %>").html();
        var Cert_Revision = $("#<%= txtCertRev.ClientID %>").first().val();
        var validationResult = validateProfessionalServiceDetails();

        if (validationResult && validateProfessionalProcedureCodeModifiers()) {
            $("[id*=lblErrorMessageonTop]").text('');

            if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_revtxtCharges') != null) {
                if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_revtxtCharges').style.display != 'none') {
                    return false;
                }
            }
            var txtProcCode = $("#<%= txtServiceProcedureCode.ClientID %>").first().val();
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
                    if ((result.length == 0) || (result.length > 1)) {
                        $("[id*=txtServiceProcedureCode]").val('');
                        $("[id*=lblErrorMessageonTop]").text('Procedure code is invalid.');
                    }
                    else {
                        var txtPlaceCode = $("#<%= txtProfPlaceOfService.ClientID %>").first().val();
                        if (txtPlaceCode == "00" || txtPlaceCode == "0") { $("[id*=txtProfPlaceOfService]").val(''); return false; }
                        var APIToken = $("[id*=hdnAccessToken]").val();
                        $.ajax({
                            type: "GET",
                            url: webApiPA + "GetPlaceOfServiceCodeDetails?val=" + txtPlaceCode,
                            //data: '{desc: "" , val: "' + txtPlaceCode + '" }',
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
                                    $("#<%= errPlaceOfService.ClientID %>").html("*Place Of Service is invalid");
                                    return false;
                                }
                                else {
                                    var APIToken = $("[id*=hdnAccessToken]").val();
                                    $.ajax({
                                        type: "POST",
                                        url: webApiClaims + "AddProfessionalServiceDetails?claimid=" + claimid + "&&ProcedureCode=" + ProcedureCode + "&&DateOfService=" + DateOfService + "&&DMECertType=" + DMECertType + "&&DMEDuration=" + DMEDuration + "&&ProfPlaceOfService=" + ProfPlaceOfService + "&&Modifier1=" + Modifier1 + "&&Modifier2=" + Modifier2 + "&&Modifier3=" + Modifier3 + "&&Modifier4=" + Modifier4 + "&&DiagnosisPointer1=" + DiagnosisPointer1 + "&&DiagnosisPointer2=" + DiagnosisPointer2 + "&&DiagnosisPointer3=" + DiagnosisPointer3 + "&&DiagnosisPointer4=" + DiagnosisPointer4 + "&&Charges=" + Charges + "&&UnitsOfMeasurement=" + UnitsOfMeasurement + "&&BilledUnits=" + BilledUnits + "&&lblDetailsItemProf=" + lblDetailsItemProf + "&&paidunits=" + paidunits + "&&paidAmount=" + paidAmount + "&&status=" + status + "&&PriorAuth=" + PriorAuth + "&&Refralnumber=" + Refralnumber + "&&Linenumber=" + Linenumber + "&&Referred_EPSDT_Service=" + Referred_EPSDT_Service + "&&Family_Planning=" + Family_Planning + "&&Emergency=" + Emergency + "&&Final_EAPG=" + Final_EAPG + "&&Payment_Action=" + Payment_Action + "&&Paid_Amount=" + Paid_Amount + "&&Total_Charges=" + Total_Charges + "&&Cert_Revision=" + Cert_Revision + "&&user=<%=HttpContext.Current.User.Identity.Name%>",
                                        //data: '{claimid: "' + claimid + '" ,ProcedureCode: "' + ProcedureCode + '" , DateOfService: "' + DateOfService + '" , DMECertType: "' + DMECertType + '" , DMEDuration: "' + DMEDuration + '" , ProfPlaceOfService: "' + ProfPlaceOfService + '" , Modifier1: "' + Modifier1 + '" , Modifier2: "' + Modifier2 + '" , Modifier3: "' + Modifier3 + '" , Modifier4: "' + Modifier4 + '" , DiagnosisPointer1: "' + DiagnosisPointer1 + '" , DiagnosisPointer2: "' + DiagnosisPointer2 + '" , DiagnosisPointer3: "' + DiagnosisPointer3 + '" , DiagnosisPointer4: "' + DiagnosisPointer4 + '" , Charges: "' + Charges + '", UnitsOfMeasurement: "' + UnitsOfMeasurement + '", BilledUnits: "' + BilledUnits + '", lblDetailsItemProf: "' + lblDetailsItemProf + '", paidunits: "' + paidunits + '", paidAmount: "' + paidAmount + '", status: "' + status + '", PriorAuth: "' + PriorAuth + '", Refralnumber: "' + Refralnumber + '", Linenumber: "' + Linenumber + '", Referred_EPSDT_Service: "' + Referred_EPSDT_Service + '", Family_Planning: "' + Family_Planning + '", Emergency: "' + Emergency + '", Final_EAPG: "' + Final_EAPG + '", Payment_Action: "' + Payment_Action + '", Paid_Amount: "' + Paid_Amount + '", Total_Charges: "' + Total_Charges + '", Cert_Revision: "' + Cert_Revision + '"}',
                                        headers: {
                                            "Access-Control-Allow-Origin": "*",
                                            "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                                            "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                                            "Authorization": "Bearer " + APIToken
                                        },
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function () {
                                            displayprofessionalservicedetailtable(0, 'Add');
                                        },
                                        error: function (jqXHR, textStatus, errorThrown) {
                                            $("[id*=lblmessage]").text('Error Adding Service Line');
                                        }
                                    });
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                $("[id*=lblmessage]").text('Place Of Service code not found.');
                            }
                        });
                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=txtServiceProcedureCode]").val('');
                    $("[id*=lblErrorMessageonTop]").text('Procedure code not found.');
                }
            });
        }
        return false;
    }

    function CopyProfessionalServiceLineItem(Claim_service_Id, claim_id, serviceline, control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        var hdnHidePaidAmounts = document.getElementById("<%=hdnHidePaidAmounts.ClientID %>").value;
        $("[id*=lblProcedureCodeError]").text('');
        $("#<%= lblErrorDOS.ClientID %>").html("");
        $("#<%= errPlaceOfService.ClientID %>").html("");
        $("#<%= lblpointerError.ClientID %>").html("");
        $("#<%= txtchargeserror.ClientID %>").html("");
        $("#<%= lblBilledUnitsError.ClientID %>").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorMessageonTop1").text('');
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
                displayprofessionalservicedetailtable(serviceline, 'Add', control);
                $(control).closest('tr').find('input[type=button]').prop('disabled', true);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblIcdVersionError]").text('Error Occurred in Service Line Copy');
            }
        });
    }


    function EditProfessionalServiceLineItem(Claim_service_Id, claim_id, service_line, control, isCopy = 0) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        var hdnHidePaidAmounts = document.getElementById("<%=hdnHidePaidAmounts.ClientID %>").value;
        $("[id*=lblProcedureCodeError]").text('');
        $("#<%= lblErrorDOS.ClientID %>").html("");
        $("#<%= errPlaceOfService.ClientID %>").html("");
        $("#<%= lblpointerError.ClientID %>").html("");
        $("#<%= txtchargeserror.ClientID %>").html("");
        $("#<%= lblBilledUnitsError.ClientID %>").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorMessageonTop1").text('');
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetIndProfessionalServiceDetail?Claim_service_Id=" + Claim_service_Id + "&&claim_id=" + claim_id + "&&serviceline=" + service_line,
            //data: '{Claim_service_Id: "' + Claim_service_Id + '", claim_id:"' + claim_id + '", serviceline:"' + service_line + '"}',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                claimProfessionalServiceDetailList = result;
                if (claimProfessionalServiceDetailList.length > 0) {
                    for (var i = 0; i < claimProfessionalServiceDetailList.length; i++) {
                        var sequence = i;
                        sequence++;
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
                        pad_amnt = HandleDecimalValue(pad_amnt).toString();
                        if (hdnHidePaidAmounts == "1") {
                            pad_amnt = "";
                            pad_unt = "";
                        }

                        var cde_clm_status = result[i].cde_clm_status;
                        var mdf_first = result[i].mdf_first;
                        var mdf_secnd = result[i].mdf_secnd;
                        var mdf_thrd = result[i].mdf_thrd;
                        var mdf_forth = result[i].mdf_forth;
                        var digno_first = result[i].digno_first;
                        var digno_sec = result[i].digno_sec;
                        var digno_third = result[i].digno_third;
                        var digno_forth = result[i].digno_forth;
                        var unt_of_measurment = result[i].unt_of_measurment;
                        var Referred_EPSDT_Service = result[i].Referred_EPSDT_Service;
                        var Family_Planning = result[i].Family_Planning;
                        var Emergency = result[i].Emergency;
                        var Final_EAPG = result[i].Final_EAPG;
                        var Payment_Action = result[i].Payment_Action;
                        var Total_Charges = result[i].Total_Charges;

                        var dme_cert_type = result[i].dme_cert_type;
                        var dme_duration = result[i].dme_duration;
                        var prior_auth = result[i].prior_auth;
                        var ref_num = result[i].ref_num;
                        var Line_Control_Number = result[i].Line_Control_Number;
                        var Cert_Revision = result[i].Cert_Revision;
                        if (Cert_Revision == '1/1/1900 12:00:00 AM') { Cert_Revision = ""; }
                        if (Cert_Revision != "") {
                            Cert_Revision = new Date(Cert_Revision).toLocaleDateString();
                            $("#<%= txtCertRev.ClientID %>").first().val(Cert_Revision);
                        }

                        $("#<%= txtServiceProcedureCode.ClientID %>").first().val(cde_proc);
                        $("#<%= txtProffdateofservice.ClientID %>").first().val(ServiceDate);
                        $("#<%= txtProfPlaceOfService.ClientID %>").first().val(plc_service);
                        $("#<%= txtModifier1.ClientID %>").first().val(mdf_first);
                        $("#<%= txtModifier2.ClientID %>").first().val(mdf_secnd);
                        $("#<%= txtModifier3.ClientID %>").first().val(mdf_thrd);
                        $("#<%= txtModifier4.ClientID %>").first().val(mdf_forth);
                        $("[id*=hdnProfessionalServiceDetails]").val("" + claim_id + "");
                        $("[id*=hdnProfessionalClaimServiceId]").val("" + Claim_service_Id + "");
                        $("#<%= lblDetailsItemProf.ClientID %>").html("" + service_line + "");
                        $('#<%=ddlDiagnosisPointer1.ClientID%>').val(digno_first);
                        $('#<%=ddlDiagnosisPointer2.ClientID%>').val(digno_sec);
                        $('#<%=ddlDiagnosisPointer3.ClientID%>').val(digno_third);
                        $('#<%=ddlDiagnosisPointer4.ClientID%>').val(digno_forth);
                        $('#<%=ddlUnitsOfMeasurement.ClientID%>').val(unt_of_measurment);
                        if (cde_clm_status != null && cde_clm_status != "" && cde_clm_status != undefined) {
                            $('#<%=lblProfStatus.ClientID%>').text(cde_clm_status);
                        }
                        if (cde_clm_chrge == "" || cde_clm_chrge == undefined || cde_clm_chrge == null) { $("[id*=txtCharges]").val("0.00"); }
                        else { $("[id*=txtCharges]").val("" + cde_clm_chrge + ""); }
                        if (pad_amnt != null && pad_amnt != undefined && pad_amnt != "") {
                            $('#<%=lblProfTotalAmountPaid.ClientID%>').text(pad_amnt);
                        }

                        $("[id*=txtBilledUnits]").val("" + bil_unt + "");
                        if (pad_unt != null && pad_unt != "" && pad_unt != undefined) {
                            $('#<%=lblProfPaidUnits.ClientID%>').text(pad_unt);
                        }

                        $('#<%=ddlEpsdtService.ClientID%>').val(Referred_EPSDT_Service);
                        $('#<%=ddlFamilyPlanning.ClientID%>').val(Family_Planning);
                        $('#<%=ddlEmergency.ClientID%>').val(Emergency);
                        if (Final_EAPG != null && Final_EAPG != "" && Final_EAPG != undefined) {
                            $('#<%=lblProfFinalEAPG.ClientID%>').text(Final_EAPG);
                        }
                        if (Total_Charges != null && Total_Charges != "" && Total_Charges != undefined) {
                            var totalchargeabc = parseFloat(Total_Charges);
                            $('#<%=lblProfTotalCharges.ClientID%>').text(totalchargeabc.toFixed(2));
                        }
                        //Update total billed units
                        var tbu = $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_hdnTotalBilledUnits").val();
                        if (tbu != "" && isCopy == 0) {
                            tbu = Number(tbu) - Number(bil_unt);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_hdnTotalBilledUnits").val(tbu);
                        }
                       
                        $('#<%=ddlDMECertType.ClientID%>').val(dme_cert_type);
                        $("[id*=txtDuration]").val("" + dme_duration + "");
                        $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_txtReferralNumber').val("" + ref_num + "");
                        $("[id*=txtLineCntrlNumber]").val("" + Line_Control_Number + "");
                        $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_txtPriorAuthNumber').val("" + prior_auth + "");
                        if (isCopy != 1) {
                            $("#<%= lblDetailsItemProf.ClientID %>").html("" + service_line + "");
                        }
                        if (digno_first.trim() != null && digno_first.trim() != "" && digno_first.trim() != "undefined") {
                            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1 option:contains("' + digno_first + '")').prop('selected', true);
                            loader1Change();
                        }
                        if (digno_sec.trim() != null && digno_sec.trim() != "" && digno_sec.trim() != "undefined") {
                            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2 option:contains("' + digno_sec + '")').prop('selected', true);
                            loader2Change();
                        }
                        if (digno_third.trim() != null && digno_third.trim() != "" && digno_third.trim() != "undefined") {
                            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3 option:contains("' + digno_third + '")').prop('selected', true);
                            loader3Change();
                        }
                        if (digno_forth.trim() != null && digno_forth.trim() != "" && digno_forth.trim() != "undefined") {
                            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4 option:contains("' + digno_forth + '")').prop('selected', true);
                        }

                        if ((digno_first.trim() == null || digno_first.trim() == "" || digno_first.trim() == "undefined") &&
                            (digno_sec.trim() == null || digno_sec.trim() == "" || digno_sec.trim() == "undefined") &&
                            (digno_third.trim() == null || digno_third.trim() == "" || digno_third.trim() == "undefined") &&
                            (digno_forth.trim() == null || digno_forth.trim() == "" || digno_forth.trim() == "undefined")) {

                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").attr("disabled", false);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").attr("disabled", true);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").attr("disabled", true);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").attr("disabled", true);

                            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1').val(0);
                        }

                        if (isCopy) {
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML = "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$ProfInfoAdd' value='ADD' onclick='return ProfInfoAdd_Click();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProfInfoAdd;' class='btn btn-primary' style='font-weight:bold;width:90px; visibility: visible;'>";
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$btnUpdateServiceDetailProfessional' value='Update' onclick='return UpdateProfessionalServiceDetail();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_btnUpdateServiceDetailProfessional' class='btn btn-primary' style='font-weight: bold; width: 90px; visibility: hidden;'>";
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$btncancelProfessional' value='Cancel' onclick='clearallfieldsProfessionalSetrvicedetail(); return displayprofessionalservicedetailtable();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_btncancelProfessional' class='btn btn-primary' style='font-weight: bold; width: 90px; visibility: visible;'>";
                        } else {
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML = "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$ProfInfoAdd' value='ADD' onclick='return ProfInfoAdd_Click();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProfInfoAdd;' class='btn btn-primary' style='font-weight:bold;width:90px; visibility: hidden;'>";
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$btnUpdateServiceDetailProfessional' value='Update' onclick='return UpdateProfessionalServiceDetail();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_btnUpdateServiceDetailProfessional' class='btn btn-primary' style='font-weight: bold; width: 90px; visibility: visible;'>";
                            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$btncancelProfessional' value='Cancel' onclick='clearallfieldsProfessionalSetrvicedetail(); return displayprofessionalservicedetailtable();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_btncancelProfessional' class='btn btn-primary' style='font-weight: bold; width: 90px; visibility: visible;'>";
                        }

                        return false;
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblIcdVersionError]").text('Error Occurred in Service Line Edit');
            }
        });
    }
    function UpdateProfessionalServiceDetail() {
        var claimStatus = $("#ctl00_MainContent_uc5SubmitClaim_txtstatus2").val();
        var servicedetailprofessionalList;
        var claimid = document.getElementById("<%=hdnValueCode_ClaimID.ClientID %>").value;
        var ProcedureCode = $("#<%= txtServiceProcedureCode.ClientID %>").first().val();
        var ProfPlaceOfService = $("#<%= txtProfPlaceOfService.ClientID %>").first().val();
        var DateOfService = $("#<%= txtProffdateofservice.ClientID %>").first().val();
        var DMECertType = $("#<%=ddlDMECertType.ClientID %> option:selected").text();
        var DMEDuration = $("#<%= txtDuration.ClientID %>").first().val();
        var Charges = $("#<%= txtCharges.ClientID %>").first().val();
        var storedcharge = Charges;
        Charges = HandleDecimalValue(Charges).toString();
        if (Charges == null || Charges == "" || Charges == undefined) {
            if (storedcharge == "0" || storedcharge == "0.0" || storedcharge == "0.00" || storedcharge == "0.000") {
                Charges = "0.00";
            }
        }
        var UnitsOfMeasurement = $("#<%=ddlUnitsOfMeasurement.ClientID %> option:selected").text();
        var BilledUnits = $("#<%= txtBilledUnits.ClientID %>").first().val();
        var DiagnosisPointer1 = $("#<%=ddlDiagnosisPointer1.ClientID %> option:selected").text();
        var DiagnosisPointer2 = $("#<%=ddlDiagnosisPointer2.ClientID %> option:selected").text();
        var DiagnosisPointer3 = $("#<%=ddlDiagnosisPointer3.ClientID %> option:selected").text();
        var DiagnosisPointer4 = $("#<%=ddlDiagnosisPointer4.ClientID %> option:selected").text();
        var Modifier1 = $("#<%= txtModifier1.ClientID %>").first().val();
        var Modifier2 = $("#<%= txtModifier2.ClientID %>").first().val();
        var Modifier3 = $("#<%= txtModifier3.ClientID %>").first().val();
        var Modifier4 = $("#<%= txtModifier4.ClientID %>").first().val();
        var lblDetailsItemProf = $("#<%= lblDetailsItemProf.ClientID %>").html();
        var paidAmount = $("#<%= lblProfTotalAmountPaid.ClientID %>").html();
        var paidunits = $("#<%= lblProfPaidUnits.ClientID %>").html();
        var status = $("#<%= lblProfStatus.ClientID %>").html();
        var hdnProfessionalServiceDetails = $("#<%= hdnProfessionalServiceDetails.ClientID %>").val();
        var hdnProfessionalClaimServiceId = $("#<%= hdnProfessionalClaimServiceId.ClientID %>").val();
        var Referred_EPSDT_Service = $("#<%=ddlEpsdtService.ClientID %> option:selected").text();
        var Family_Planning = $("#<%=ddlFamilyPlanning.ClientID %> option:selected").text();
        var Emergency = $("#<%=ddlEmergency.ClientID %> option:selected").text();
        var Final_EAPG = $("#<%=lblProfFinalEAPG.ClientID %>").html();
        var Payment_Action = $("#<%=lblProfPaymentAction.ClientID %>").html();
        var Paid_Amount = $("#<%=lblProfPaidAmount.ClientID %>").html();
        var Total_Charges = $("#<%=lblProfTotalCharges.ClientID %>").html();
        var Cert_Revision = $("#<%= txtCertRev.ClientID %>").first().val();
        var Linenumber = $("#<%= txtLineCntrlNumber.ClientID %>").first().val();
        var PriorAuth = $("#<%= txtPriorAuthNumber.ClientID %>").first().val();
        var Refralnumber = $("#<%= txtReferralNumber.ClientID %>").first().val();

        if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_revtxtCharges') != null) {
            if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_revtxtCharges').style.display != 'none') {
                return false;
            }
        }
        if (validateProfessionalServiceDetails() && validateProfessionalProcedureCodeModifiers()) {
            $("[id*=lblErrorMessageonTop]").text('');
            var txtProcCode = $("#<%= txtServiceProcedureCode.ClientID %>").first().val();
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
                    if ((result.length == 0) || (result.length > 1)) {
                        $("[id*=txtServiceProcedureCode]").val('');
                        $("[id*=lblErrorMessageonTop]").text('Procedure code is invalid.');
                        return false;
                    }
                    else {
                        var txtPlaceCode = $("#<%= txtProfPlaceOfService.ClientID %>").first().val();
                        var APIToken = $("[id*=hdnAccessToken]").val();
                        $.ajax({
                            type: "GET",
                            url: webApiPA + "GetPlaceOfServiceCodeDetails?val=" + txtPlaceCode,
                            //data: '{desc: "" , val: "' + txtPlaceCode + '" }',
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
                                    $("#<%= errPlaceOfService.ClientID %>").html("*Place Of Service is invalid");
                                    return false;
                                }
                                else {
                                    var APIToken = $("[id*=hdnAccessToken]").val();
                                    $.ajax({
                                        type: "PUT",
                                        url: webApiClaims + "EditProfessionalServiceDetailCode?claimid=" + claimid + "&&ProcedureCode=" + ProcedureCode + "&&DateOfService=" + DateOfService + "&&DMECertType=" + DMECertType + "&&DMEDuration=" + DMEDuration + "&&ProfPlaceOfService=" + ProfPlaceOfService + "&&Modifier1=" + Modifier1 + "&&Modifier2=" + Modifier2 + "&&Modifier3=" + Modifier3 + "&&Modifier4=" + Modifier4 + "&&DiagnosisPointer1=" + DiagnosisPointer1 + "&&DiagnosisPointer2=" + DiagnosisPointer2 + "&&DiagnosisPointer3=" + DiagnosisPointer3 + "&&DiagnosisPointer4=" + DiagnosisPointer4 + "&&Charges=" + Charges + "&&UnitsOfMeasurement=" + UnitsOfMeasurement + "&&BilledUnits=" + BilledUnits + "&&lblDetailsItemProf=" + lblDetailsItemProf + "&&paidunits=" + paidunits + "&&paidAmount=" + paidAmount + "&&status=" + status + "&&hdnProfessionalClaimServiceId=" + hdnProfessionalClaimServiceId + "&&hdnProfessionalServiceDetails=" + hdnProfessionalServiceDetails + "&&Referred_EPSDT_Service=" + Referred_EPSDT_Service + "&&Family_Planning=" + Family_Planning + "&&Emergency=" + Emergency + "&&Final_EAPG=" + Final_EAPG + "&&Payment_Action=" + Payment_Action + "&&Paid_Amount=" + Paid_Amount + "&&Total_Charges=" + Total_Charges + "&&Cert_Revision=" + Cert_Revision + "&&Linenumber=" + Linenumber + "&&PriorAuth=" + PriorAuth + "&&Refralnumber=" + Refralnumber + "&&user=<%=HttpContext.Current.User.Identity.Name%>",
                                        //data: '{claimid: "' + claimid + '" ,ProcedureCode: "' + ProcedureCode + '" , DateOfService: "' + DateOfService + '" , DMECertType: "' + DMECertType + '" , DMEDuration: "' + DMEDuration + '" , ProfPlaceOfService: "' + ProfPlaceOfService + '" , Modifier1: "' + Modifier1 + '" , Modifier2: "' + Modifier2 + '" , Modifier3: "' + Modifier3 + '" , Modifier4: "' + Modifier4 + '" , DiagnosisPointer1: "' + DiagnosisPointer1 + '" , DiagnosisPointer2: "' + DiagnosisPointer2 + '" , DiagnosisPointer3: "' + DiagnosisPointer3 + '" , DiagnosisPointer4: "' + DiagnosisPointer4 + '" , Charges: "' + Charges + '", UnitsOfMeasurement: "' + UnitsOfMeasurement + '", BilledUnits: "' + BilledUnits + '", lblDetailsItemProf: "' + lblDetailsItemProf + '", paidunits: "' + paidunits + '", paidAmount: "' + paidAmount + '", status: "' + status + '",hdnProfessionalClaimServiceId: "' + hdnProfessionalClaimServiceId + '",hdnProfessionalServiceDetails: "' + hdnProfessionalServiceDetails + '", Referred_EPSDT_Service: "' + Referred_EPSDT_Service + '", Family_Planning: "' + Family_Planning + '", Emergency: "' + Emergency + '", Final_EAPG: "' + Final_EAPG + '", Payment_Action: "' + Payment_Action + '", Paid_Amount: "' + Paid_Amount + '", Total_Charges: "' + Total_Charges + '", Cert_Revision: "' + Cert_Revision + '", Linenumber: "' + Linenumber + '", PriorAuth: "' + PriorAuth + '", Refralnumber: "' + Refralnumber + '" }',
                                        headers: {
                                            "Access-Control-Allow-Origin": "*",
                                            "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                                            "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                                            "Authorization": "Bearer " + APIToken
                                        },
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        success: function () {
                                            displayprofessionalservicedetailtable(0, 'Update');
                                        },
                                        error: function (jqXHR, textStatus, errorThrown) {
                                            $("[id*=lblmessage]").text('Error Updating Service Line');
                                        }
                                    });
                                }
                            },
                            error: function (jqXHR, textStatus, errorThrown) {
                                $("[id*=lblmessage]").text('Place Of Service code not found.');
                            }
                        });
                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=txtServiceProcedureCode]").val('');
                    $("[id*=lblErrorMessageonTop]").text('Procedure code not found.');
                }
            });
        }
        return false;
    }

    function DeleteProfessionalServiceDetailLineitem(claimid, service_line, control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
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
                    displayprofessionalservicedetailtable(0, 'Add');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblIcdVersionError]").text('Error Occurred in Service Line Delete');
                }
            });
        }
    }

    function clearallfieldsProfessionalSetrvicedetail() {
        clearProfessionalServiceDetailFields();
        var claimid = document.getElementById("<%=hdnValueCode_ClaimID.ClientID %>").value;
        if (document.getElementById('<%= hdnServiceLines.ClientID %>').value < 50) {
            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML = "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$ProfInfoAdd' value='ADD' onclick='return ProfInfoAdd_Click();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProfInfoAdd;' class='btn btn-primary' style='font-weight:bold;width:90px; visibility: visible;'>";
            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$btnUpdateServiceDetailProfessional' value='Update' onclick='return UpdateProfessionalServiceDetail();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_btnUpdateServiceDetailProfessional' class='btn btn-primary' style='font-weight: bold; width: 90px; visibility: hidden;'>";
            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$ProfessionalServiceLineDetails$btncancelProfessional' value='Cancel' onclick='return clearallfieldsProfessionalSetrvicedetail();' id='ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_btncancelProfessional' class='btn btn-primary' style='font-weight: bold; width: 90px; visibility: hidden;'>";
        } else {
            document.getElementById("<%= profSerDiv.ClientID%>").innerHTML = "";
        }
        // displayprofessionalservicedetailtable();
    }

    function loadPlaceOfService() {
        ReadonlyFields();
        document.getElementById('<%= btnloading2.ClientID %>').style.display = 'block';
        document.getElementById('<%= lnkPlaceofServiceSearch.ClientID %>').style.display = 'none';
    }
    /*Diagnosis Pointer Dropdowns code starts here*/
    /*Global Variables for diagnosis pointers Professional */
    var oldDiagnosisPointerValue;
    var oldDiagnosisPointerValue2;
    var oldDiagnosisPointerValue3;
    var oldDiagnosisPointerValue4;

    /*Functions for Add scenario*/
    function loader1Change() {
        var diagPointer1 = $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1 option:selected").val();

        if (diagPointer1 == oldDiagnosisPointerValue) {
            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").attr("disabled", false);
            if (oldDiagnosisPointerValue != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2 > option[value=' + oldDiagnosisPointerValue + ']').remove();
                $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3 > option[value=' + oldDiagnosisPointerValue + ']').remove();
                $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4 > option[value=' + oldDiagnosisPointerValue + ']').remove();
            }

            //sorting dropdown2
            SortDiagnosisPointer2();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2').val(0);

            //sorting dropdown3
            SortDiagnosisPointer3();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3').val(0);

            //sorting Dropdown4
            SortDiagnosisPointer4();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').val(0);
        }

        if ((diagPointer1 != null || diagPointer1 != "" || diagPointer1 != undefined) &&
            (oldDiagnosisPointerValue == "" || oldDiagnosisPointerValue == null || oldDiagnosisPointerValue == 'undefined')) {

            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2 > option[value=' + diagPointer1 + ']').remove();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3 > option[value=' + diagPointer1 + ']').remove();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4 > option[value=' + diagPointer1 + ']').remove();

            $('#hdnoldDiagnosisPointerValue').val(diagPointer1);
            oldDiagnosisPointerValue = $('#hdnoldDiagnosisPointerValue').val();
            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").attr("disabled", false);
            return;
        }
        else if ((diagPointer1 != oldDiagnosisPointerValue) && (diagPointer1 != "" || diagPointer1 != 'undefined' || diagPointer1 != null) && (diagPointer1.length > 0)) {

            var addPointer = "<option value='" + oldDiagnosisPointerValue + "'>" + oldDiagnosisPointerValue + "</option>";
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2').append(addPointer);
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3').append(addPointer);
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').append(addPointer);


            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2 > option[value=' + diagPointer1 + ']').remove();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3 > option[value=' + diagPointer1 + ']').remove();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4 > option[value=' + diagPointer1 + ']').remove();

            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").attr("disabled", false);

            //sorting dropdown2
            SortDiagnosisPointer2();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2').val(0);

            //sorting dropdown3 if not disabled
            if (!$('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3').is(':disabled')) {
                SortDiagnosisPointer3();
                $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3').val(0);
                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").attr("disabled", true);
            }

            //sorting dropdown4 if not disabled
            if (!$('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').is(':disabled')) {
                SortDiagnosisPointer4();
                $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').val(0);
                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").attr("disabled", true);
            }
            $('#hdnoldDiagnosisPointerValue').val(diagPointer1);
            oldDiagnosisPointerValue = $('#hdnoldDiagnosisPointerValue').val();

            return;
        }
        else if (diagPointer1.length == 0) {
            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").attr("disabled", true);
            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").attr("disabled", true);
            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").attr("disabled", true);

            SortDiagnosisPointer2();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2').val(0);
            SortDiagnosisPointer3();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3').val(0);
            SortDiagnosisPointer4();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').val(0);
            return;
        }
        return false;
    }

    function loader2Change() {
        var diagPointer2 = $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2 option:selected").val();
        if (diagPointer2 == oldDiagnosisPointerValue2) {
            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").attr("disabled", false);
            if (oldDiagnosisPointerValue != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3 > option[value=' + oldDiagnosisPointerValue + ']').remove();
            }
            //sorting dropdown3
            SortDiagnosisPointer3();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3').val(0);

            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").attr("disabled", true);
            //sorting Dropdown4
            SortDiagnosisPointer4();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').val(0);
        }

        if ((diagPointer2 != null && diagPointer2 != "" && diagPointer2 != undefined) &&
            (oldDiagnosisPointerValue2 == "" || oldDiagnosisPointerValue2 == null || oldDiagnosisPointerValue2 == 'undefined')) {
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3 > option[value=' + diagPointer2 + ']').remove();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4 > option[value=' + diagPointer2 + ']').remove();

            $('#hdnoldDiagnosisPointerValue2').val(diagPointer2);
            oldDiagnosisPointerValue2 = $('#hdnoldDiagnosisPointerValue2').val();

            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").attr("disabled", false);
            return;
        }
        else if ((diagPointer2 != oldDiagnosisPointerValue2) && (diagPointer2 != "" || diagPointer2 != 'undefined' || diagPointer2 != null) && (diagPointer2.length > 0)) {
            var addPointer2 = "<option value='" + oldDiagnosisPointerValue2 + "'>" + oldDiagnosisPointerValue2 + "</option>";

            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3').append(addPointer2);
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').append(addPointer2);

            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3 > option[value=' + diagPointer2 + ']').remove();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4 > option[value=' + diagPointer2 + ']').remove();

            if (oldDiagnosisPointerValue != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3 > option[value=' + oldDiagnosisPointerValue + ']').remove();
            }

            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").attr("disabled", false);

            //sorting dropdown3
            SortDiagnosisPointer3();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3').val(0);

            //sorting dropdown4 if not disabled
            if (!$('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').is(':disabled')) {
                SortDiagnosisPointer4();
                $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').val(0);
                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").attr("disabled", true);
            }

            $('#hdnoldDiagnosisPointerValue2').val(diagPointer2);
            oldDiagnosisPointerValue2 = $('#hdnoldDiagnosisPointerValue2').val();

            return;
        }
        else if (diagPointer2.length == 0) {
            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").attr("disabled", true);
            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").attr("disabled", true);

            SortDiagnosisPointer3();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3').val(0);
            SortDiagnosisPointer4();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').val(0);
            return;
        }
        return false;
    }

    function loader3Change() {
        var diagPointer3 = $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3 option:selected").val();
        if (diagPointer3 == oldDiagnosisPointerValue3) {
            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").attr("disabled", false);
            if (oldDiagnosisPointerValue != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4 > option[value=' + oldDiagnosisPointerValue + ']').remove();
            }
            if (oldDiagnosisPointerValue2 != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4 > option[value=' + oldDiagnosisPointerValue2 + ']').remove();
            }
            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").attr("disabled", false);

            //sorting Dropdown4
            SortDiagnosisPointer4();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').val(0);

            $('#hdnoldDiagnosisPointerValue3').val(diagPointer3);
            oldDiagnosisPointerValue3 = $('#hdnoldDiagnosisPointerValue3').val();
        }
        if ((diagPointer3 != null && diagPointer3 != "" && diagPointer3 != undefined) &&
            (oldDiagnosisPointerValue3 == "" || oldDiagnosisPointerValue3 == null || oldDiagnosisPointerValue3 == 'undefined')) {
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4 > option[value=' + diagPointer3 + ']').remove();

            $('#hdnoldDiagnosisPointerValue3').val(diagPointer3);
            oldDiagnosisPointerValue3 = $('#hdnoldDiagnosisPointerValue3').val();
            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").attr("disabled", false);
            return;

        }
        else if ((diagPointer3 != oldDiagnosisPointerValue3) && (diagPointer3 != "" || diagPointer3 != 'undefined' || diagPointer3 != null) && (diagPointer3.length > 0)) {
            var addPointer3 = "<option value='" + oldDiagnosisPointerValue3 + "'>" + oldDiagnosisPointerValue3 + "</option>";

            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').append(addPointer3);

            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4 > option[value=' + diagPointer3 + ']').remove();

            if (oldDiagnosisPointerValue != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4 > option[value=' + oldDiagnosisPointerValue + ']').remove();
            }
            if (oldDiagnosisPointerValue2 != "") {
                $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4 > option[value=' + oldDiagnosisPointerValue2 + ']').remove();
            }
            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").attr("disabled", false);
            //sorting Dropdown4
            SortDiagnosisPointer4();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').val(0);


            $('#hdnoldDiagnosisPointerValue3').val(diagPointer3);
            oldDiagnosisPointerValue3 = $('#hdnoldDiagnosisPointerValue3').val();

            return;
        }
        else if (diagPointer3.length == 0) {
            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").attr("disabled", true);

            SortDiagnosisPointer4();
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').val(0);
            return;
        }
        return false;
    }

    /*Functions for sorting dropdown values in ascending order*/
    function SortDiagnosisPointer2() {
        var selectList = $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2 option");
        selectList.sort(function (a, b) {
            a = a.value;
            b = b.value;

            return a - b;
        });
        $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2').html(selectList);
    }

    function SortDiagnosisPointer3() {
        var selectList = $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3 option");
        selectList.sort(function (a, b) {
            a = a.value;
            b = b.value;

            return a - b;
        });
        $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3').html(selectList);
    }

    function SortDiagnosisPointer4() {
        var selectList = $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4 option");
        selectList.sort(function (a, b) {
            a = a.value;
            b = b.value;

            return a - b;
        });
        $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').html(selectList);
    }

    function ReloadDiagnosisPointersProf() {
        $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2').html($('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1').html());
        $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3').html($('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1').html());
        $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4').html($('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1').html());
    }

    /*Diagnosis Pointer Dropdowns code Ends here*/

    function ModifierChanged(e) {
        var variable = "";
        var modifierNo = "";
        if (e.id === "ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_txtModifier1") {
            variable = $("#<%= txtModifier1.ClientID %>").first().val();
            modifierNo = "1";
            if (variable == "00" || variable == "0") { $("[id*=txtModifier1]").val(''); return false; }
        }
        if (e.id === "ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_txtModifier2") {
            variable = $("#<%= txtModifier2.ClientID %>").first().val();
            modifierNo = "2";
            if (variable == "00" || variable == "0") { $("[id*=txtModifier2]").val(''); return false; }
        }
        if (e.id === "ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_txtModifier3") {
            variable = $("#<%= txtModifier3.ClientID %>").first().val();
            modifierNo = "3";
            if (variable == "00" || variable == "0") { $("[id*=txtModifier3]").val(''); return false; }
        }
        if (e.id === "ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_txtModifier4") {
            variable = $("#<%= txtModifier4.ClientID %>").first().val();
            modifierNo = "4";
            if (variable == "00" || variable == "0") { $("[id*=txtModifier4]").val(''); return false; }
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
                    $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorMessageonTop1").text('');
                } else if (result === "false") {
                    $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorMessageonTop1").text("*Procedure modifier" + modifierNo + " is invalid.");
                    if (modifierNo == "1") { $("[id*=txtModifier1]").val(''); }
                    if (modifierNo == "2") { $("[id*=txtModifier2]").val(''); }
                    if (modifierNo == "3") { $("[id*=txtModifier3]").val(''); }
                    if (modifierNo == "4") { $("[id*=txtModifier4]").val(''); }
                    return false;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblIcdVersionError]").text('Diagnosis Code is invalid');
            }
        });
    }

    function DentalPriorAuthNumberChanged() {
        var priorAuthNumberProfessional = $("#<%= txtPriorAuthNumber.ClientID %>").first().val();
        var PriorAuthNumber = $("#ctl00_MainContent_uc5SubmitClaim_ucPriorAuthorizationAndReferringPanel_txtPriorAuthNumber").first().val();
        if (priorAuthNumberProfessional === PriorAuthNumber) {
            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_lblErrorMessageonTop").text('*PA number in the service detail should only be entered if it is different than the header');
            $("#<%= txtPriorAuthNumber.ClientID %>").first().val("");
        }
        else { $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorMessageonTop").text(''); }
        return false;
    }

    function loadddl() {

        ReadonlyFields();
        document.getElementById('<%= btnloading3.ClientID %>').style.display = 'block';

    }

    function visibleProcCode() {
        $find("mpeProcCode").show();
        return false;
    }

    function visiblePlcOfService() {
        $("#<%= ucSubmitClaimSearchPop.FindControl("txtCode").ClientID %>").first().val("");
        $("#<%= ucSubmitClaimSearchPop.FindControl("txtPlaceOfServiceName").ClientID %>").first().val("");
        $("#<%=ucSubmitClaimSearchPop.FindControl("lblSResultPlaceofService").ClientID %>").html("");
        $("#<%=ucSubmitClaimSearchPop.FindControl("gvSubmitClaimSearchPop").ClientID %>").html("");
        $find("mpePlcOfService").show();
        return false;
    }

    function GetProcedureCodeInfo() {
        var txtProcCode = $("#<%= ucSubmitClaimSearchProc.FindControl("txtCode").ClientID %>").first().val();

        var txtProccodedesc = $("#<%= ucSubmitClaimSearchProc.FindControl("txtPlaceOfServiceName").ClientID %>").first().val();

        $("#<%=ucSubmitClaimSearchProc.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").html("");
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetProcCodeDetails?desc=" + txtProccodedesc + "&&val=" + txtProcCode +"&&includeICDCodes=false",
            //data: '{desc: "' + txtProccodedesc + '" , val: "' + txtProcCode + '", includeICDCodes: false }',
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
                    $("#<%=ucSubmitClaimSearchProc.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").append("<tr><td> No Records Found </td></tr>");
                }
                else {
                    $("#<%=ucSubmitClaimSearchProc.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").append("<tr><th>Procedure Code </th><th>Procedure Code Description </th></tr>");
                    for (var i = 0; i < result.length; i++) {
                        $("#<%=ucSubmitClaimSearchProc.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").append("<tr><td><a onClick='GetProcCodeProf(this); return false;'>" + result[i].Proc_Code + "</asp:LinkButton></td><td>" + result[i].Proc_Desc + "</td></tr>");
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblmessage]").text('Procedure code not found.');
            }
        });

        return false;
    }

    function GetProcCodeProf(lnk) {

        $find("mpeProcCode").hide();

        var gridindexProccode = localStorage.getItem("indexProccode");

        if (!(gridindexProccode == "" || gridindexProccode == null || gridindexProccode == undefined)) {

            var row = lnk.parentNode.parentNode;
            <%--var grid = document.getElementById("<%= gvProfServiceLineDetails.ClientID%>");--%>
            var inputs = grid.rows[gridindexReasoncode].getElementsByTagName("INPUT");
            inputs[2].value = row.cells[0].innerText;
            $("#<%=ucSubmitClaimSearchProc.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").html("");
            $("#<%=ucSubmitClaimSearchProc.FindControl("txtCode").ClientID %>").val("");
            $("#<%=ucSubmitClaimSearchProc.FindControl("txtPlaceOfServiceName").ClientID %>").val("");
            return false;
        }
        else {
            var textboxrow = lnk.parentNode.parentNode;
            $("[id*=txtServiceProcedureCode]").val(textboxrow.cells[0].innerText.trim());
            $("#<%=ucSubmitClaimSearchProc.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").html("");
            $("#<%=ucSubmitClaimSearchProc.FindControl("txtCode").ClientID %>").val("");
            $("#<%=ucSubmitClaimSearchProc.FindControl("txtPlaceOfServiceName").ClientID %>").val("");
            $("[id*=lblErrorMessageonTop]").text('');
            $("[id*=valerrormess]").text('');
            return false;
        }
    }

    function GetPlaceOfServiceCodeInfo() {
        $("#<%=ucSubmitClaimSearchPop.FindControl("gvSubmitClaimSearchPop").ClientID %>").html("");
        $("#<%=ucSubmitClaimSearchPop.FindControl("lblSResultPlaceofService").ClientID %>").html("");
        var txtPlaceCode = $("#<%= ucSubmitClaimSearchPop.FindControl("txtCode").ClientID %>").first().val();
        var txtPiacecodedesc = $("#<%= ucSubmitClaimSearchPop.FindControl("txtPlaceOfServiceName").ClientID %>").first().val();
        if (txtPlaceCode == "" && txtPiacecodedesc == "") {
            $("#<%=ucSubmitClaimSearchPop.FindControl("lblSResultPlaceofService").ClientID %>").html("Place of service code or name is required");
            return false;
        }
        if (txtPlaceCode.length >= 1) {
            if (txtPlaceCode != undefined && txtPlaceCode.length != 2 && txtPlaceCode.length < 2) {
                $("#<%=ucSubmitClaimSearchPop.FindControl("lblSResultPlaceofService").ClientID %>").html("*2-Digit Place of service code is required");
                return false;
            }
        }
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
                        $("#<%=ucSubmitClaimSearchPop.FindControl("gvSubmitClaimSearchPop").ClientID %>").append("<tr><td><a onClick='return GetPlaceOfserviceCodeProf(this)'>" + result[i].Placeofservice_Code + "</asp:LinkButton></td><td>" + result[i].Placeofservice_Desc + "</td></tr>");
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblmessage]").text('Place Of Service code not found.');
            }
        });

        return false;
    }

    function GetPlaceOfserviceCodeProf(lnk) {

        var textboxrow = lnk.parentNode.parentNode;
        $("[id*=txtProfPlaceOfService]").val(textboxrow.cells[0].innerText.trim());
        $find("mpePlcOfService").hide();
        return false;
    }

    function DisableEnable_Add_ServiceDetail_Prof() {
        $('ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProfInfoAdd').one('submit', function () {
            $(this).find('input[type="submit"]').attr('disabled', 'disabled');
        });
    }

    function validateDate11() {
        var daterequested = document.getElementById("<%= txtCertRev.ClientID%>").value;

        var tdate = new Date();
        var dd = tdate.getDate();
        var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
        var yyyy = tdate.getFullYear();
        var currentDate = (MM) + "/" + dd + "/" + yyyy;

        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {

            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProffServiceLineDetailsDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProffServiceLineDetailsDateRequiredError1').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_txtCertRev').val('');
        }
        else {
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProffServiceLineDetailsDateRequiredError1').css('display', 'none');

        }

        if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProffServiceLineDetailsDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProffServiceLineDetailsDateRequiredError1').text('Future Date not allowed.');
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_txtCertRev').val('');
        }

    }

    function validateDate12() {
        var daterequested = document.getElementById("<%= txtProffdateofservice.ClientID%>").value;

        var tdate = new Date();
        var dd = tdate.getDate();
        var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
        var yyyy = tdate.getFullYear();
        var currentDate = (MM) + "/" + dd + "/" + yyyy;

        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {

            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProfServiceLineDetailsDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProfServiceLineDetailsDateRequiredError1').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_txtProffdateofservice').val('');
        }
        else {
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProfServiceLineDetailsDateRequiredError1').css('display', 'none');

        }

        if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProfServiceLineDetailsDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ProfServiceLineDetailsDateRequiredError1').text('Future Date not allowed.');
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_txtProffdateofservice').val('');
        }

    }

    function ServiceProcCodeChange() {

        var txtProcCode = $("#<%= txtServiceProcedureCode.ClientID %>").first().val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetProcCodeDetails?val=" + txtProcCode +"&&includeICDCodes=false",
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
                    $("[id*=txtServiceProcedureCode]").val('');
                    $("[id*=lblErrorMessageonTop]").text('Procedure code is invalid.');
                }
                else if (result.length > 1) {
                    $("[id*=txtServiceProcedureCode]").val('');
                    $("[id*=lblErrorMessageonTop]").text('Procedure code is invalid.');
                }
                else {
                    $("[id*=lblErrorMessageonTop]").text('');
                    $("[id*=valerrormess]").text('');

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=txtServiceProcedureCode]").val('');
                $("[id*=lblErrorMessageonTop]").text('Procedure code not found.');
            }
        });

        return false;
    }

    function servicePlaceOfServiceCodeChange() {
        var txtPlaceCode = $("#<%= txtProfPlaceOfService.ClientID %>").first().val();
        var providerTypeId = $("[id*=hdnProviderTypeId]").val();
        isValidPlaceCode = true;
        if (txtPlaceCode == "00" || txtPlaceCode == "0") { $("[id*=txtProfPlaceOfService]").val(''); return false; }
        var apiToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetPlaceOfServiceCodeDetails?val=" + txtPlaceCode,
            //data: '{desc: "" , val: "' + txtPlaceCode + '" }',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + apiToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.length == 0) {
                    $("[id*=txtProfPlaceOfService]").val('');
                    $("[id*=lblErrorMessageonTop1]").text('Place Of Service code is not valid.');
                }
                else {

                    //if Place of Service code is valid then
                    //Get SpecialtyTypes and check the rules for each specialty type
                    
                    ValidatePlaceOfServiceCode();

                    if (isValidPlaceCode == true) {
                        $("[id*=lblErrorMessageonTop1]").text('');
                    }                            

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMessageonTop1]").text('Place Of Service code not found.');
            }
        });

        return false;
    }

    function ValidatePlaceOfServiceCode() {
        //alert('In Validate: NPI=' + Npi);
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
                        if (isValidPlaceCode == true) {
                            //alert('i: ' + i);
                            GetCodeSetRule(SpecialtyTypeId);
                        }
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMessageonTop1]").text('Error validating Place Of Service code.');
            }
        });
    }

    function GetCodeSetRule(SpecialtyTypeId) {
        var txtCode = $("#<%= txtProfPlaceOfService.ClientID %>").first().val();
        var providerTypeId = $("[id*=hdnProviderTypeId]").val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetCodeSetRule?providerTypeId=" + providerTypeId + "&&specialtyTypeId=" + SpecialtyTypeId + "&&code=" + txtCode + "&&codeType=" + codeType + "&&claimsorPA=" + claimsOrPA + "&&claimORPAType=" + claimType,
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
                        isValidPlaceCode = false;
                        $("[id*=txtProfPlaceOfService]").val('');
                        $("[id*=lblErrorMessageonTop1]").text(errorMsg);
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMessageonTop1]").text('Error validating Place Of Service code.');
            }
        });
    }

    function HideLabel() {
        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_valerrormess").text('');
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
            if ((txtlen - dotpos) > 2)
                return false;
        }
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        else
            return true;
    }

    function validateDate2Prof() {
        var daterequested = document.getElementById("<%= txtProffdateofservice.ClientID%>").value;
        var tdate = new Date();
        var dd = tdate.getDate();
        var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
        var yyyy = tdate.getFullYear();
        var currentDate = (MM) + "/" + dd + "/" + yyyy;
        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorDOS').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorDOS').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_txtProffdateofservice').val('');
        }
        else {
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorDOS').css('display', 'none');
        }

        if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorDOS').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_lblErrorDOS').text('Future Date not allowed.');
            $('#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_txtProffdateofservice').val('');
        }
    }


</script>
<asp:HiddenField runat="server"  ID="hdnHidePaidAmounts" Value="0"/>
<asp:HiddenField runat="server"  ID="hdnServiceLines" Value="0" />
<asp:UpdatePanel ID="upProfServiceLineDetails" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="div_instiServiceDetail">
            <asp:Label runat="server" ID="lblErrorMessageonTop" CssClass="failureNotification"></asp:Label>
            <div id="upProfServiceLineDetails1" runat="server">
                <asp:Label runat="server" ID="lblErrorMessageonTop1" CssClass="failureNotification"></asp:Label>
            </div>
        </div>
        <div style="padding-left: 750px;">
            <div class="col-sm-5" style="height: 20px;">
                <span class="ohio-field" style="font-size: 15px; text-align: right">Total Charges:</span>
            </div>
            <div class="col-sm-7">
                <asp:Label ID="lblProfTotalCharges" runat="server"></asp:Label>
            </div>
            <div class="col-sm-5">
                <span class="ohio-field" style="font-size: 15px; text-align: right">Total Amount Paid:</span>
            </div>
            <div class="col-sm-7">
                <asp:Label ID="lblProfTotalAmountPaid" runat="server"></asp:Label>
            </div>
        </div>
        <div runat="server" id="divProf">
            <div class="clearfix"></div> 
            <div class="row"><span style="color:red;text-align:left !important; margin-left:50px !important;margin-bottom:15px !important">If a prior authorization number is entered, please ensure it is a valid one. Inaccurate entry will result in delay or denial of Claim Processing</span></div>
            <div class="row" style="background-color: lightblue;">
                <div class="col-sm-2" style="font-size: 15px; text-align: right; padding-left: 50px; padding-top: 2px; height: 25px;">
                    <span class="ohio-field" style="font-size: 14px;">Service Line:
                        <asp:Label ID="lblDetailsItemProf" runat="server" Height="16px" ReadOnly="true"
                            Width="52px" />
                    </span>
                </div>
            </div>
            <div>
                <div>
                    <asp:Label runat="server" ID="lblBilledUnitsMsgError" CssClass="failureNotification"></asp:Label>
                </div>
            </div>
            <asp:HiddenField ID="hdnProfessionalServiceDetails" runat="server" />
            <asp:HiddenField ID="hdnProfessionalClaimServiceId" runat="server" />
            <asp:HiddenField ID="hdnProfServiceDetailClaimStatus" runat="server" />
            <asp:HiddenField ID="hdnTotalBilledUnits" runat="server" />
            <div>
                <div style="float: left; width: 100%; height: 50px">
                    <div style="padding-top: 500px; padding-left: 10px; font-size: 16px;">
                        <asp:ValidationSummary ID="valerrormess" DisplayMode="List" runat="server" CssClass="failureNotification"
                            ValidationGroup="valProfServiceLineInfo" />
                    </div>
                </div>
            </div>
            <div>
                <div style="float: left; width: 35%; height: 50px;">
                    <div class="col-sm-5" style="padding-left: 75px">
                        <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Procedure code:</span>
                    </div>
                    <div class="col-sm-7">
                        <span style="text-align: left;">
                            <asp:TextBox ID="txtServiceProcedureCode" OnChange="return ServiceProcCodeChange()" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 140px; min-width: 100px;" />
                            <asp:LinkButton ID="lnkInsProcCode" runat="server" Text="Search" ToolTip="Search" OnClientClick="return visibleProcCode()" Visible="true" Style="font-size: 13px;"></asp:LinkButton>
                            <br />

                          <%--   <asp:RequiredFieldValidator runat="server" ID="rfvProcedureCode" SetFocusOnError="true" ForeColor="Red"
                                ValidationGroup="valProfServiceLineInfo" ControlToValidate="txtServiceProcedureCode" Text="*Procedure code is Required" Display="Dynamic" />--%>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ControlToValidate="txtServiceProcedureCode" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ControlToValidate="txtProfPlaceOfService" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                        </span>
                    </div>

                </div>
                <div style="float: left; width: 35%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 50px">
                        <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Place of Service:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtProfPlaceOfService" OnChange="return servicePlaceOfServiceCodeChange()" runat="server" CssClass="formFieldPos" Style="height: 30px; width: 140px; min-width: 100px;" />
                        <button id="btnloading2" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>
                        <asp:Label ID="errPlaceOfService" runat="server" ForeColor="Red"></asp:Label>
                        <%--<asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Search" Text="Search" Visible="true" Style="font-size: 13px;" />--%>
                        <%--<asp:LinkButton ID="lnkPlaceofServiceSearchDetail" Text="Search" runat="server" ToolTip="Search" OnClick="lnkSubClaimSepopSearch_Click" CommandArgument='<%# Eval("CDE_POS") %>' Visible="true"></asp:LinkButton>--%>
                        <asp:LinkButton ID="lnkPlaceofServiceSearch" Text="Search" runat="server" Style="font-size: 15px;" ToolTip="Search" OnClientClick="return visiblePlcOfService()" Visible="true"></asp:LinkButton>
                        <asp:Label runat="server" ID="lblError" Text="" ForeColor="Red" />
                        <br />
                        <asp:RequiredFieldValidator ID="rfvPlaceofserv" runat="server" ControlToValidate="txtProfPlaceOfService" Text="*Place of service is required" ForeColor="Red" Display="Dynamic" ValidationGroup="valProfServiceLineInfo"></asp:RequiredFieldValidator>

                    </div>
                </div>

                <div style="float: left; width: 25%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 30px">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Status:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:Label ID="lblProfStatus" Font-Size="Medium" runat="server"></asp:Label>
                    </div>
                </div>

            </div>

            <div style="padding-top: 60px;">

                <div style="float: left; width: 35%; height: 50px;">
                    <div class="col-sm-5" style="padding-left: 30px">
                        <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Date of Service:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtProffdateofservice" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 140px; min-width: 100px;" OnChange="validateDate2Prof()" />
                        <asp:Image ID="imgServiceToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                        <ajax:CalendarExtender ID="cedateofservice" runat="server" Format="MM/dd/yyyy" TargetControlID="txtProffdateofservice" PopupPosition="BottomLeft" CssClass="QstCalendarCSS" EnabledOnClient="true" />
                        <br />
                        <%-- <asp:CompareValidator ID="cvdateofservice" runat="server" ValidationGroup="valProfServiceLineInfo"
                            Type="Date" Operator="LessThanEqual" ControlToValidate="txtProffdateofservice"
                            ErrorMessage="*Select Date of service valid smaller date than today" Text="*Future Date not Allowed" ForeColor="Red"
                            SetFocusOnError="true"></asp:CompareValidator>--%>
                        <asp:Label ID="ProfServiceLineDetailsDateRequiredError1" runat="server" Text="" CssClass="error-message" Style="display: none;"></asp:Label>
                        <asp:Label ID="lblErrorDOS" runat="server" ForeColor="Red"></asp:Label>

                        <asp:RequiredFieldValidator runat="server" ID="revdateofservice" SetFocusOnError="true" ForeColor="Red"
                            ValidationGroup="valProfServiceLineInfo" ControlToValidate="txtProffdateofservice" Text="*Date of service is required." Display="Dynamic" />

                    </div>

                </div>
                <div style="float: left; width: 35%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 30px">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Modifier:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtModifier1" onChange="return ModifierChanged(this)" runat="server" CssClass="formfieldMod" Style="height: 30px; width: 2%; min-width: 45px;" MaxLength="2"
                            onKeyUp="javascript:alphanumericOnly(this);" onKeyDown="HideLabel()" />
                        <asp:TextBox ID="txtModifier2" onChange="return ModifierChanged(this)" runat="server" CssClass="formfieldMod" Style="height: 30px; width: 2%; min-width: 45px;" MaxLength="2"
                            onKeyUp="javascript:alphanumericOnly(this);" onKeyDown="HideLabel()" />
                        <asp:TextBox ID="txtModifier3" onChange="return ModifierChanged(this)" runat="server" CssClass="formfieldMod" Style="height: 30px; width: 2%; min-width: 45px;" MaxLength="2"
                            onKeyUp="javascript:alphanumericOnly(this);" onKeyDown="HideLabel()" />
                        <asp:TextBox ID="txtModifier4" onChange="return ModifierChanged(this)" runat="server" CssClass="formfieldMod" Style="height: 30px; width: 2%; min-width: 45px;" MaxLength="2"
                            onKeyUp="javascript:alphanumericOnly(this);" onKeyDown="HideLabel()" />
                    </div>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtModifier1" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtModifier2" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtModifier3" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txtModifier4" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                </div>


                <div style="float: left; width: 25%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 30px">
                        <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Charges:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtCharges" runat="server" MaxLength="18" CssClass="formfieldDate" Style="height: 30px; width: 140px; min-width: 100px;" />
                        <asp:Label ID="txtchargeserror" runat="server" ForeColor="Red"></asp:Label>
                        <asp:RequiredFieldValidator runat="server" ID="revCharges" SetFocusOnError="true" ForeColor="Red"
                            ValidationGroup="valProfServiceLineInfo" ControlToValidate="txtCharges" Text="*Charges is required" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revtxtCharges" runat="server" ControlToValidate="txtCharges" ValidationExpression="^(-?\d+\.)?-?\d+$"
                            ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                    </div>
                </div>
            </div>

            <div style="padding-top: 100px">

                <div style="float: left; width: 35%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 45px">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Line Control Number:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtLineCntrlNumber" runat="server" MaxLength="50" CssClass="formfieldDate" Style="height: 30px; width: 140px; min-width: 100px;" />
                    </div>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtLineCntrlNumber" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                </div>
                <div style="float: left; width: 35%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 50px">
                        <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Diagnosis Pointer:</span>
                    </div>
                    <div class="col-sm-7">

                        <asp:DropDownList onChange="loader1Change()" ID="ddlDiagnosisPointer1" ViewStateMode="Enabled" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True" Style="height: 30px; width: 10px; min-width: 53px;">
                        </asp:DropDownList>
                        <asp:DropDownList onChange="loader2Change()" ID="ddlDiagnosisPointer2" ViewStateMode="Enabled" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True" Style="height: 30px; width: 10px; min-width: 52px;">
                        </asp:DropDownList>
                        <asp:DropDownList onChange="loader3Change()" ID="ddlDiagnosisPointer3" ViewStateMode="Enabled" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True" Style="height: 30px; width: 10px; min-width: 52px;">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlDiagnosisPointer4" EnableViewState="true" runat="server" ViewStateMode="Enabled"
                            AppendDataBoundItems="True" Style="height: 30px; width: 10px; min-width: 52px;">
                        </asp:DropDownList>
                        <br />
                        <asp:Label ID="lblpointerError" Text="" ForeColor="Red" runat="server"></asp:Label>
                        <asp:Label ID="lblpointerErr" ForeColor="Red" runat="server"></asp:Label>

                    </div>
                    <div>
                        <button id="btnloading3" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>
                    </div>
                </div>
                <div style="float: left; width: 25%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 60px">
                        <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Billed Units:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtBilledUnits" onkeypress="return event.charCode == 46 || (event.charCode >= 48 && event.charCode <= 57)" runat="server" MaxLength="12" CssClass="formfieldDate" Style="height: 30px; width: 140px; min-width: 100px;" />
                        <asp:Label ID="lblBilledUnitsError" ForeColor="Red" runat="server"></asp:Label>
                        <br />
                        <asp:RequiredFieldValidator runat="server" ID="revBilledUnits" SetFocusOnError="true" ForeColor="Red"
                            ValidationGroup="valProfServiceLineInfo" ControlToValidate="txtBilledUnits" Text="*Billed Units is required" Display="Dynamic" />
                        <div runat="server" style="text-align: right">
                            <asp:RegularExpressionValidator ID="revtxtBilledUnits" ValidationGroup="valProfServiceLineInfo" Display="Dynamic"
                                ControlToValidate="txtBilledUnits" runat="server" ErrorMessage="*Max 8 digit before decimal, 3 digit after decimal allowed in billed units" ForeColor="Red"
                                Text="Max 8 digit<br>before decimal<br>& 3 digit<br>after decimal"
                                SetFocusOnError="True" ValidationExpression="^[0-9]{1,8}(\.[0-9]{1,3})*$"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                </div>

            </div>

            <div style="padding-top: 70px">
                <div style="float: left; width: 35%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 5px">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Prior Authorization Number:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtPriorAuthNumber" onChange="return DentalPriorAuthNumberChanged()" MaxLength="50" onKeyUp="javascript:alphanumericOnly(this);" runat="server" CssClass="formFieldTextBoxSmall" Style="height: 30px; width: 140px; min-width: 100px;" />
                    </div>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtPriorAuthNumber" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                </div>
                <div style="float: left; width: 35%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 50px">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Referred EPSDT Service:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:DropDownList ID="ddlEpsdtService" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True" Style="height: 30px; width: 10px; min-width: 60px;">
                            <asp:ListItem Value="" Text=""></asp:ListItem>
                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                            <%--<asp:ListItem Value="N" Text="No" />--%>
                        </asp:DropDownList>
                    </div>
                </div>

                <div style="float: left; width: 25%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 50px">
                        <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Unit of Measurement:</span>
                    </div>
                    <div class="col-sm-5">
                        <asp:DropDownList ID="ddlUnitsOfMeasurement" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True" Style="height: 30px; width: 10px; min-width: 55px;" class="selectdropdown">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="rfvDropdownUnitofMeasurement" SetFocusOnError="true" ForeColor="Red"
                            ValidationGroup="valProfServiceLineInfo" ControlToValidate="ddlUnitsOfMeasurement"
                            Text="*Unit of Measurement is Required." Display="Dynamic" />
                    </div>
                     <asp:Label ID="lblUnitOfMeasurementError" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>

            <div>

                <div style="float: left; width: 35%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 75px">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Referral Number:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtReferralNumber" MaxLength="20" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 140px; min-width: 100px;" />
                    </div>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtReferralNumber" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                </div>
                <div style="float: left; width: 35%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 10px">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Family Planning:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:DropDownList ID="ddlFamilyPlanning" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True" Style="height: 30px; width: 10px; min-width: 60px;">
                            <asp:ListItem Value="" Text=""></asp:ListItem>
                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                            <%-- <asp:ListItem Value="N" Text="No" />--%>
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="float: left; width: 25%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 50px">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Paid Units:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:Label ID="lblProfPaidUnits" runat="server"></asp:Label>
                    </div>
                </div>
            </div>

            <div>

                <div style="float: left; width: 35%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 35px">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">DME Certification Type:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:DropDownList ID="ddlDMECertType" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True" Style="height: 30px; width: 10px; min-width: 110px;">
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="float: left; width: 35%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 97px">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Emergency:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:DropDownList ID="ddlEmergency" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True" Style="height: 30px; width: 10px; min-width: 60px;">
                            <asp:ListItem Value="" Text=""></asp:ListItem>
                            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
                            <%--  <asp:ListItem Value="N" Text="No" />--%>
                        </asp:DropDownList>
                    </div>
                </div>


                <div style="float: left; width: 25%; height: 50px" id="divPaidAmount" runat="server">
                    <div class="col-sm-5" style="padding-left: 30px">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Paid Amount:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:Label ID="lblProfPaidAmount" runat="server"></asp:Label>
                    </div>
                </div>
            </div>

            <div>

                <div style="float: left; width: 35%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 34px">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">DME Duration (month):</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:TextBox ID="txtDuration" runat="server" MaxLength="15" CssClass="formfieldDate" Style="height: 30px; width: 140px; min-width: 100px;" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtDuration" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                            ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                        <%--   <asp:RequiredFieldValidator runat="server" ID="revDuration" SetFocusOnError="true"
                ValidationGroup="valProfServiceLineInfo" ControlToValidate="txtDuration" ErrorMessage="DME Duration (month) is required." Text="*" Display="Dynamic" InitialValue="0" />
            <asp:RangeValidator ID="revDurationMonth" runat="server" ErrorMessage="RangeValidator" ControlToValidate="txtDuration" MinimumValue="0" Type="Double"></asp:RangeValidator>--%>
                    </div>
                </div>
                <div style="float: left; width: 35%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 93px">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Final EAPG:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:Label ID="lblProfFinalEAPG" runat="server"></asp:Label>
                    </div>
                </div>
                <div style="float: left; width: 25%; height: 50px" id="profSerDiv" runat="server">
                    <button id="btnloading" runat="server" style="display: none;" cssclass="buttonbox stepbutton buttonboxfocus"><i class="fa fa-spinner fa-spin"></i>loading</button>
                    <asp:Button ID="ProfInfoAdd" Text="ADD" OnClientClick="return ProfInfoAdd_Click();" runat="server" CssClass="btn btn-primary" Font-Bold="True" Width="90px" EnableViewState="False" CausesValidation="false"  />
                    <asp:Button ID="btnUpdateServiceDetailProfessional" Style="visibility: hidden" OnClientClick="return UpdateProfessionalServiceDetail();" Text="Update" CssClass="btn btn-primary" runat="server" Font-Bold="True" ValidationGroup="valProfServiceLineInfo" EnableViewState="false" CausesValidation="false" Width="90px" />
                    <asp:Button ID="btncancelProfessional" Style="visibility: hidden" OnClientClick="clearallfieldsProfessionalSetrvicedetail(); return displayprofessionalservicedetailtable();" Text="Cancel" CssClass="btn btn-primary" runat="server" Font-Bold="True" ValidationGroup="valProfServiceLineInfo" EnableViewState="false" CausesValidation="false" Width="90px" />
                </div>


            </div>

            <div>
                <div style="float: left; width: 35%; height: 50px">
                    <div class="col-sm-5" style="padding-left: 25px">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Certification Revision or Recertification Date:</span>
                    </div>
                    <div class="col-sm-7">
                        <%-- <div style="padding-left: 5px">--%>
                        <asp:TextBox ID="txtCertRev" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 140px; min-width: 100px;"  />
                        <asp:Image ID="imgProfServiceToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                        <ajax:CalendarExtender ID="ceCertRev" runat="server" Format="MM/dd/yyyy" TargetControlID="txtCertRev" PopupPosition="BottomLeft" CssClass="QstCalendarCSS" EnabledOnClient="true" />
                        <asp:Label ID="ProffServiceLineDetailsDateRequiredError1" runat="server" Text="" CssClass="error-message" Style="display: none;"></asp:Label>
                        <asp:Label ID="lblErrorCertificationRev" runat="server" ForeColor="Red" Width="500px"></asp:Label>
                        <%-- </div>--%>
                    </div>
                </div>

                <div style="float: left; width: 35%; height: 50px" id="divPaymentAction" runat="server">
                    <div class="col-sm-5" style="padding-left: 10px">
                        <span class="ohio-field" style="font-size: 15px; text-align: right">Payment Action:</span>
                    </div>
                    <div class="col-sm-7">
                        <asp:Label ID="lblProfPaymentAction" runat="server"></asp:Label>
                    </div>
                </div>

            </div>
        </div>

        <asp:HiddenField ID="hdnValueCode_ClaimID" runat="server" />
        <asp:HiddenField ID="hdnProviderTypeId" runat="server" />

    </ContentTemplate>
</asp:UpdatePanel>


<ajax:ModalPopupExtender BehaviorID="mpeProcCode" ID="mpeSubmitClaimSearchProc" runat="server" PopupControlID="pnlSubmitClaimSearchProcPop"
    TargetControlID="ButtonSearchProc" BackgroundCssClass="modalBackground" CancelControlID="btnCloseProc" />
<asp:Panel ID="pnlSubmitClaimSearchProcPop" runat="server" CssClass="modalPopup"
    Style="display: none; min-height: 100px; min-width: 900px; height: auto; width: auto;">
    <asp:Panel ID="pnlSubmitClaimSearchProcHeader" CssClass="popHeader" runat="server"
        HorizontalAlign="Left">
        <asp:Button runat="server" ID="btnCloseProc" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
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
                <uc:SubmitClaimSearchProc runat="server" ID="ucSubmitClaimSearchProc" Visible="true" EnableViewState="true" />
            </div>
        </div>
    </asp:Panel>
</asp:Panel>
<asp:Button runat="server" ID="ButtonSearchProc" Style="display: none" Text="ButtonSearchReson" />

<asp:Button runat="server" ID="Button3" Style="display: none" Text="ButtonDummy" />
<ajax:ModalPopupExtender BehaviorID="mpePlcOfService" ID="mpeSubmitClaimSearchPop" runat="server" PopupControlID="pnlSubmitClaimSearchPop"
    TargetControlID="Button9" BackgroundCssClass="modalBackground" CancelControlID="btnCloseCH9" />

<asp:Panel ID="pnlSubmitClaimSearchPop" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
    <asp:Panel ID="pnlSubmitClaimSearchPopHeader" runat="server" HorizontalAlign="Left">
        <asp:Button runat="server" ID="btnCloseCH9" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;" CausesValidation="false" />
        <div class="search-Results">
            <span style="padding-left: 10px; font-size: 17px;">CODE</span>
            <span style="padding-left: 280px; font-size: 17px;">PLACE OF SERVICE</span>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlSubmitClaimSearch" runat="server">
        <div style="text-align: left; padding: 15px" class="container-fluid">
            <div class="row">
                <uc:SubmitClaimSearchPop runat="server" ID="ucSubmitClaimSearchPop" Visible="true" EnableViewState="true" />
            </div>
        </div>
        <%--<asp:MultiView ID="mltSubmitClaimSearchPop" runat="server">
                <asp:View ID="vwSubmitClaimSearchPop" runat="server">
                    
                </asp:View>
            </asp:MultiView>--%>
    </asp:Panel>
</asp:Panel>
<asp:Button runat="server" ID="Button9" Style="display: none" Text="ButtonDummy9" />
<input type="hidden" value="" id="hdnoldDiagnosisPointerValue">
<input type="hidden" value="" id="hdnoldDiagnosisPointerValue2">
<input type="hidden" value="" id="hdnoldDiagnosisPointerValue3">
