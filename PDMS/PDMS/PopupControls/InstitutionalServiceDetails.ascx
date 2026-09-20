<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_InstitutionalServiceDetails" Codebehind="InstitutionalServiceDetails.ascx.cs" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchProc.ascx" TagPrefix="uc" TagName="SubmitClaimSearchProc" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchRevenueCode.ascx" TagPrefix="uc" TagName="SubmitClaimSearchRevenueCode" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%--<script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>--%>

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">

<br />
<script type="text/javascript">
    $(document).ready(function () {
        var clmstatus = $("#ctl00_MainContent_uc5SubmitClaim_txtstatus2").val();
        if (clmstatus != null && clmstatus != undefined && clmstatus != "") {
            $("#<%= lblInstStatus.ClientID %>").html(clmstatus);
        }


    });
    var codeType = "RevenueCode";
    var claimsOrPA = "Claims";
    var claimType = "Institutional";
    var NpiElement = document.getElementById("ctl00_MainContent_ucRegProgressBar_lblPRONPI2");
    var MedIdElement = document.getElementById("ctl00_MainContent_ucRegProgressBar_lblProMedicaidID2");
    var providerTypeId = $("[id*=hdnProviderTypeId]").val();
    // Retrieve the text content inside the span
    var Npi = NpiElement.textContent || NpiElement.innerText;
    var MedId = MedIdElement.textContent || MedIdElement.innerText;
    var isValidRevenueCode = true;
    //alert('NPI: ' + Npi);
    //alert('MedId:' + MedId);

    function displayInstiServiceDetailtable(copied_Service_Line = 0, action="Add", control) {
        var InstiServiceLineclaimstatus = document.getElementById("<%=hdnInstiServiceLineClaimStatus.ClientID %>").value;
        if (InstiServiceLineclaimstatus == "Pending Submission") {
            var table = localStorage.getItem("instiServiceDetailTable");
        }
        else if (InstiServiceLineclaimstatus == "Other") {
            var table = localStorage.removeItem("instiServiceDetailTable");
        }

        var claimid = document.getElementById("<%=hdnValueCode_ClaimID.ClientID %>").value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetinstitutionalServiceDetails?claimid=" + claimid,
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
                var table;
                var totalcharge = 0;
                var totalpaidamount = 0;
                var rowcount = 0;

                $("#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ddlNDCServiceLine").empty();
                $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlAdditonalProviderDetail").empty();
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlDetailId").empty();
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlServiceLine").empty();
                $("#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ddlNDCServiceLine").append($("<option> </option>"));
                $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlAdditonalProviderDetail").append($("<option> </option>"));
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlDetailId").append($("<option> </option>"));
                $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlServiceLine").append($("<option> </option>"));
                if (claimServiceDetailsInistList.length > 0) {

                    table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>*Revenue Code</th><th style='width:10px; scope='col'>Procedure Type</th><th style='width:10px; scope='col'>Procedure Code</th><th style='width:10px; scope='col'>*Unit</th><th style='width:10px; scope='col'>Unit Of Measurement</th><th style='width:10px; scope='col'>*From DOS</th><th style='width:10px; scope='col'>To DOS</th><th style='width:10px; scope='col'>*Total Charges</th><th style='width:10px; scope='col'>Paid Amount</th><th style='width:10px; scope='col'>Status</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    var j = 0;

                    for (var i = 0; i < claimServiceDetailsInistList.length; i++) {
                        var service_Line = result[i].service_Line;
                        var sequence = service_Line;
                        rowcount++;

                        var revenue_Code = result[i].revenue_Code;

                        var cde_proc = result[i].cde_proc;
                        var procedure_type = result[i].procedure_type;
                        var unit = result[i].unit;
                        var unit_of_measurement = result[i].unit_of_measurement;
                        var fromDOS = new Date(result[i].fromDOS);
                        fromDOS = formatDate(fromDOS);
                        var toDOS = result[i].toDOS;

                        if (toDOS != null && toDOS != "" && toDOS != undefined) {
                            toDOS = formatDate(new Date(toDOS));
                        } else { toDOS = ""; }
                        var total_charges = result[i].total_charges;
                        var total_charges1 = total_charges;
                        total_charges = HandleDecimalValue(total_charges).toString();
                        if (total_charges == null || total_charges == "" || total_charges == undefined) {
                            if (total_charges1 == "0" || total_charges1 == "0.0" || total_charges1 == "0.00" || total_charges1 == "0.000") {
                                total_charges = "0.00";
                            }
                        }
                        var paid_amount = result[i].paid_amount;
                        paid_amount = HandleDecimalValue(paid_amount).toString();
                        var status = result[i].status;
                        var Claim_service_Id = result[i].Claim_service_Id;
                        var Claim_Id = result[i].Claim_Id;

                        totalcharge = totalcharge + parseFloat(total_charges);
                        totalpaidamount = totalpaidamount + parseFloat(paid_amount);
                        j = sequence;
                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + revenue_Code + "</span></td><td><span  title='Line' class='tNumber'>" + procedure_type + "</span></td><td><span title='Line' class='tNumber'>" + cde_proc + "</span></td><td>" + unit + "</td><td>" + unit_of_measurement + "</td><td>" + fromDOS + "</td><td>" + toDOS + "</td><td>" + total_charges + "</td><td>" + paid_amount + "</td><td>" + status + "</td><td><input type='button' value = 'Edit' onClick = 'return EditInstiServiceLineItem(\"" + Claim_service_Id + "\",\"" + Claim_Id + "\",\"" + service_Line + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Copy' onclick='return EditInstiServiceLineItem(\"" + Claim_service_Id + "\",\"" + Claim_Id + "\",\"" + service_Line + "\",this, 1); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteInstiServiceDetailLineitem(\"" + Claim_Id + "\",\"" + service_Line + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ddlNDCServiceLine").append($("<option></option>").val(sequence).html(sequence));
                        $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlAdditonalProviderDetail").append($("<option></option>").val(sequence).html(sequence));
                        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlDetailId").append($("<option></option>").val(sequence).html(sequence));
                        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerAdjustmentServiceDetail_ddlServiceLine").append($("<option></option>").val(sequence).html(sequence));

                    };
                    j++;

                    $("#<%= lblDetailsItemInst.ClientID %>").html("" + j + "");

                    table = table + "</tbody></table>";
                    localStorage.setItem("instiServiceDetailTable", "" + table + "");
                    if (document.getElementById('<%= serviceDetailInsti.ClientID %>') != null)
                    {
                        document.getElementById('<%= serviceDetailInsti.ClientID %>').innerHTML = table;
                    }

                    $("#<%= lblInsTotalAmountPaid.ClientID %>").html("");
                    $("#<%= lblInsTotalAmountBilled.ClientID %>").html("");
                    $("#<%= lblInsTotalAmountBilled.ClientID %>").html("" + totalcharge.toFixed(2) + "");
                    if (isNaN(totalpaidamount)) {
                        $("#<%= lblInsTotalAmountPaid.ClientID %>").html("" + 0.00 + "");

                    }
                    else {
                        $("#<%= lblInsTotalAmountPaid.ClientID %>").html("" + totalpaidamount + "");
                    }
                    if (rowcount >= 999) {
                        document.getElementById('<%= InstInfoAdd.ClientID%>').style.visibility = "hidden";
                    } else { document.getElementById('<%= InstInfoAdd.ClientID%>').style.visibility = "visible"; }

                    bindAdditionalProviderInformationserviceDetail();
                    GetOtherPayerPaidAmountBind();
                    otherPayerAdjSerDetailBind();
                    ndcPanelBind();
                }
                else {
                    document.getElementById('<%= serviceDetailInsti.ClientID %>').innerHTML = "";
                    $("#<%= lblInsTotalAmountBilled.ClientID %>").html("");
                    $("#<%= lblInsTotalAmountPaid.ClientID %>").html("" + 0.00 + "");
                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMessageInstitutional]").text('Error Displaying Service Lines');
            }
        });

        if (copied_Service_Line == 0) {
            if (document.getElementById("<%= instiSerDiv.ClientID%>") != null) {
                document.getElementById("<%= instiSerDiv.ClientID%>").innerHTML = " <input type='submit' name='ctl00$MainContent$uc5SubmitClaim$InstitutionalServiceDetails$InstInfoAdd' value='ADD' onclick='return addInstitutionalServiceDetailCode();' id='ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstInfoAdd' class='btn btn-primary' style='font-weight:bold;width:70px; visibility: visible;'>";
                document.getElementById("<%= instiSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$InstitutionalServiceDetails$btnUpdateServiceDetailInsti' value = 'Update' onclick = 'return UpdateInstiServiceDetail();' id='ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_btnUpdateServiceDetailInsti' class='btn btn-primary' style='font-weight: bold; width: 70px; visibility: hidden;'>";
                document.getElementById("<%= instiSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$InstitutionalServiceDetails$btnCancelInsti' value='Cancel' onclick='clearInstiServiceDetailFieldsOnClickOfCancel(); displayInstiServiceDetailtable();' id='ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_btnCancelInsti' class='btn btn-primary' style='font-weight: bold; width: 70px; visibility: visible;'>";
            }
        } else {
            if (document.getElementById("<%= instiSerDiv.ClientID%>") != null) {
                document.getElementById("<%= instiSerDiv.ClientID%>").innerHTML = " <input type='submit' name='ctl00$MainContent$uc5SubmitClaim$InstitutionalServiceDetails$InstInfoAdd' value='ADD' onclick='return addInstitutionalServiceDetailCode();' id='ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstInfoAdd' class='btn btn-primary' style='font-weight:bold;width:70px; visibility: hidden;'>";
                document.getElementById("<%= instiSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$InstitutionalServiceDetails$btnUpdateServiceDetailInsti' value = 'Update' onclick = 'return UpdateInstiServiceDetail();' id='ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_btnUpdateServiceDetailInsti' class='btn btn-primary' style='font-weight: bold; width: 70px; visibility: visible;'>";
                document.getElementById("<%= instiSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$InstitutionalServiceDetails$btnCancelInsti' value='Cancel' onclick='clearInstiServiceDetailFieldsOnClickOfCancel(); displayInstiServiceDetailtable();' id='ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_btnCancelInsti' class='btn btn-primary' style='font-weight: bold; width: 70px; visibility: visible;'>";
            }
            EditInstiServiceLineItem(1, claimid, copied_Service_Line, control);
        }
    };
    function DisableFields() {
        document.getElementById('<%= txtInstRevenueCode.ClientID %>').disabled = true;
        document.getElementById('<%= txtInsFromDOS.ClientID %>').disabled = true;
        document.getElementById('<%= txtInsToDOS.ClientID %>').disabled = true;
        document.getElementById('<%= txtInsUnit.ClientID %>').disabled = true;
        document.getElementById('<%= txtInsProcCode.ClientID %>').disabled = true;
        document.getElementById('<%= ddlInsUnitsOfMeasurement.ClientID %>').disabled = true;
        document.getElementById('<%= txtInsModifier1.ClientID %>').disabled = true;
        document.getElementById('<%= txtInsModifier2.ClientID %>').disabled = true;
        document.getElementById('<%= txtInsModifier3.ClientID %>').disabled = true;
        document.getElementById('<%= txtInsModifier4.ClientID %>').disabled = true;
        document.getElementById('<%= txtInsTotalCharges.ClientID %>').disabled = true;
        document.getElementById('<%= txtInsLineCntrlNumber.ClientID %>').disabled = true;
        document.getElementById('<%= tblInsNonCovCharges.ClientID %>').disabled = true;

    }
    function InsFrmDOSChange() {
        DisableFields();
        document.getElementById('<%= btnloading1.ClientID %>').style.display = 'block';

    }
    function InsToDOSChange() {
        DisableFields();
        document.getElementById('<%= btnloading2.ClientID %>').style.display = 'block';

    }
    function loaderInstitutionalServiceDetails() {

        var RevenueCode = $("#<%= txtInstRevenueCode.ClientID %>").first().val();
        var FromDos = $("#<%= txtInsFromDOS.ClientID %>").first().val();
        var Unit = $("#<%= txtInsUnit.ClientID %>").first().val();
        var UnitofMeasure = $("#<%=ddlInsUnitsOfMeasurement.ClientID %> option:selected").text();
        var TotalCharges = $("#<%= txtInsTotalCharges.ClientID %>").first().val();

        if (RevenueCode != "" && FromDos != "" && Unit != "" && TotalCharges != "" && UnitofMeasure != "") {
            document.getElementById('<%= txtInstRevenueCode.ClientID %>').disabled = true;
            document.getElementById('<%= txtInsFromDOS.ClientID %>').disabled = true;
            document.getElementById('<%= txtInsToDOS.ClientID %>').disabled = true;
            document.getElementById('<%= txtInsUnit.ClientID %>').disabled = true;
            document.getElementById('<%= txtInsProcCode.ClientID %>').disabled = true;
            document.getElementById('<%= ddlInsUnitsOfMeasurement.ClientID %>').disabled = true;
            document.getElementById('<%= txtInsModifier1.ClientID %>').disabled = true;
            document.getElementById('<%= txtInsModifier2.ClientID %>').disabled = true;
            document.getElementById('<%= txtInsModifier3.ClientID %>').disabled = true;
            document.getElementById('<%= txtInsModifier4.ClientID %>').disabled = true;
            document.getElementById('<%= txtInsTotalCharges.ClientID %>').disabled = true;
            document.getElementById('<%= txtInsLineCntrlNumber.ClientID %>').disabled = true;
            document.getElementById('<%= tblInsNonCovCharges.ClientID %>').disabled = true;

            document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';
            document.getElementById('<%= InstInfoAdd.ClientID %>').style.display = 'none';
        }

    }


    function CheckProp() {
        if (<%= ShowServiceDetails %> == True) {
            document.getElementById("ServiceLine").style.visibility = "visible";
        }
        else {
            document.getElementById("ServiceLine").style.visibility = "hidden";
        }
    }
    function visibleRevenueCode() {
        $("#<%=ucSubmitClaimSearchRevenueCode.FindControl("lblRevenueError").ClientID %>").html("");
        $("#<%= ucSubmitClaimSearchRevenueCode.FindControl("txtCode").ClientID %>").first().val("");
        $("#<%= ucSubmitClaimSearchRevenueCode.FindControl("txtPlaceOfServiceName").ClientID %>").first().val("");
        $("#<%=ucSubmitClaimSearchRevenueCode.FindControl("gvSubmitClaimSearchRevPop").ClientID %>").html("");
        localStorage.setItem("indexRevencode", "");
        $find("mperRevenueCode").show();
        return false;

    }
    function visibleProcedureCode() {
        $("#<%=ucSubmitClaimSearchProc.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").html("");
        localStorage.setItem("indexProccode", "");
        $find("mperProcedureCode").show();
        return false;

    }
    function DisableEnable_Add_ServiceDetail_Insti() {
        setTimeout(function () {
            $("#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstInfoAdd").prop("disabled", true)
        }, 999);

        setTimeout(function () {
            $("#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstInfoAdd").prop('disabled', false);
        }, 1000);
    }
    function CloseProcedureCodePopup() {
        $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_ucSubmitClaimSearchProc_txtCode').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_ucSubmitClaimSearchProc_txtPlaceOfServiceName').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_ucSubmitClaimSearchProc_gvSubmitClaimSearchProcPop').find('tbody').html('');
    }

    function validateDate9() {
        var daterequested = document.getElementById("<%= txtInsFromDOS.ClientID%>").value;

        var tdate = new Date();
        var dd = tdate.getDate();
        var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
        var yyyy = tdate.getFullYear();
        var currentDate = (MM) + "/" + dd + "/" + yyyy;

        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstServiceDetailDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstServiceDetailDateRequiredError1').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_txtInsFromDOS').val('');

        }
        else {
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstServiceDetailDateRequiredError1').css('display', 'none');

        }

        if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstServiceDetailDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstServiceDetailDateRequiredError1').text('Future Date not allowed.');
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_txtInsFromDOS').val('');
        }
    }
    function validateDate10() {
        var daterequested = document.getElementById("<%= txtInsToDOS.ClientID%>").value;

        var tdate = new Date();
        var dd = tdate.getDate();
        var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
        var yyyy = tdate.getFullYear();
        var currentDate = (MM) + "/" + dd + "/" + yyyy;

        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstServiceDetailToDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstServiceDetailToDateRequiredError1').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_txtInsToDOS').val('');
        }
        else {
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstServiceDetailToDateRequiredError1').css('display', 'none');


        }

        if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstServiceDetailToDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstServiceDetailToDateRequiredError1').text('Future Date not allowed.');
            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_txtInsToDOS').val('');
        }
    }


    function GetServiceDetailsInfo() {
        $("#<%=ucSubmitClaimSearchRevenueCode.FindControl("lblRevenueError").ClientID %>").html("");
        $("#<%=ucSubmitClaimSearchRevenueCode.FindControl("gvSubmitClaimSearchRevPop").ClientID %>").html("");
        var txtRevenueCode = $("#<%= ucSubmitClaimSearchRevenueCode.FindControl("txtCode").ClientID %>").first().val();

        var txtRevenuecodedesc = $("#<%= ucSubmitClaimSearchRevenueCode.FindControl("txtPlaceOfServiceName").ClientID %>").first().val();
        if (txtRevenueCode == "" && txtRevenuecodedesc == "") {
            $("#<%=ucSubmitClaimSearchRevenueCode.FindControl("lblRevenueError").ClientID %>").html("Revenue Code or Revenue Code Description  is required for Search.");
            return false;
        }
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetRevenueCodeDetails?desc=" + txtRevenuecodedesc + "&&val=" + txtRevenueCode,
            //data: '{desc: "' + txtRevenuecodedesc + '" , val: "' + txtRevenueCode + '" }',
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
                    $("#<%=ucSubmitClaimSearchRevenueCode.FindControl("gvSubmitClaimSearchRevPop").ClientID %>").append("<tr><td> No Records Found </td></tr>");
                }
                else {
                    $("#<%=ucSubmitClaimSearchRevenueCode.FindControl("gvSubmitClaimSearchRevPop").ClientID %>").append("<tr><th>Revenue Code </th><th>Revenue Code Description </th></tr>");
                    for (var i = 0; i < result.length; i++) {
                        $("#<%=ucSubmitClaimSearchRevenueCode.FindControl("gvSubmitClaimSearchRevPop").ClientID %>").append("<tr><td><a onClick='GetRevenueCode(this); return false;'>" + result[i].Reven_Code + "</asp:LinkButton></td><td>" + result[i].Reven_Desc + "</td></tr>");
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMessageonTop]").text('Revenue code not found.');
            }
        });

        return false;
    }

    function GetProcedureCodeInfo() {
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
                else {
                    $("#<%=ucSubmitClaimSearchProc.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").append("<tr><th>Procedure Code </th><th>Procedure Code Description </th></tr>");
                    for (var i = 0; i < result.length; i++)
                    {
                        $("#<%=ucSubmitClaimSearchProc.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").append("<tr><td><a onClick='GetProcCode(this); return false;'>" + result[i].Proc_Code + "</asp:LinkButton></td><td>" + result[i].Proc_Desc + "</td></tr>");
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                $("[id*=lblErrorProcedureCodeMsg]").text('Procedure code not found.');
            }
        });

        return false;
    }

    function GetRevenueCode(lnk) {

        $find("mperRevenueCode").hide();

        var gridindexRevencode = localStorage.getItem("indexRevencode");

        if (!(gridindexRevencode == "")) {
            return false;
        }
        else {
            var textboxrow = lnk.parentNode.parentNode;
            $("[id*=txtInstRevenueCode]").val(textboxrow.cells[0].innerText.trim());

            return false;
        }
    }

    function visibleServiceDetailsGridView(lnk) {
        $("#<%=ucSubmitClaimSearchRevenueCode.FindControl("lblRevenueError").ClientID %>").html("");
        $("#<%=ucSubmitClaimSearchRevenueCode.FindControl("gvSubmitClaimSearchRevPop").ClientID %>").html("");
        $("#<%=ucSubmitClaimSearchRevenueCode.FindControl("txtCode").ClientID %>").val("");
        $("#<%=ucSubmitClaimSearchRevenueCode.FindControl("txtPlaceOfServiceName").ClientID %>").val("");

        var index = lnk.parentNode.parentNode.rowIndex;
        localStorage.setItem("indexReasoncode", index);

        $find("mperRevenueCode").show();
        return false;

    }

    function GetProcCode(lnk) {

        $find("mperProcedureCode").hide();

        var gridindexProccode = localStorage.getItem("indexProccode");

        if (!(gridindexProccode == "")) {
            return false;
        }
        else {
            var textboxrow = lnk.parentNode.parentNode;
            $("[id*=txtInsProcCode]").val(textboxrow.cells[0].innerText.trim());

            return false;
        }
    }

    function renuecodechange() {
        //alert('Npi: ' + Npi);
        var txtRevenueCode = $("#<%= txtInstRevenueCode.ClientID %>").first().val();
        var providerTypeId = $("[id*=hdnProviderTypeId]").val();
        isValidRevenueCode = true;
        $("[id*=lblErrorMessageonTop]").text('');
        if ((txtRevenueCode != undefined) && (txtRevenueCode != null) && (txtRevenueCode != "")) {
            if (txtRevenueCode.length < 4) {
                $("[id*=txtInstRevenueCode]").val('');
                $("[id*=lblErrorMessageonTop]").text('4-character value is required'); 
            }
            else {
                var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "GET",
                    url: webApiClaims + "GetRevenueCodeDetails?val=" + txtRevenueCode,
                    //data: '{desc: "" , val: "' + txtRevenueCode + '" }',
                    headers: {
                        "Access-Control-Allow-Origin": "*",
                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                        "Authorization": "Bearer " + APIToken
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        //alert('result: ' + result.length);
                        if (result.length == 0) {
                            $("[id*=txtInstRevenueCode]").val('');
                            $("[id*=lblErrorMessageonTop]").text('Revenue code is invalid.');
                        }
                        else if (result.length > 1) {
                            $("[id*=txtInstRevenueCode]").val('');
                            $("[id*=lblErrorMessageonTop]").text('Revenue code is invalid.');
                        }
                        else {
                            //if Revenue code is valid then
                            //Get SpecialtyTypes and check the rules for each specialty type
                            //alert('Before Validate');
                            ValidateRevenueCode()
                            
                            if (isValidRevenueCode == true) {
                                $("[id*=lblErrorMessageonTop]").text('');
                                $("[id*=valerrormess]").text('');
                            }                            
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        $("[id*=lblErrorMessageonTop]").text('Revenue code not found.');
                    }
                });
            }
        }
        return false;
    }

    function ValidateRevenueCode() {
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
                    //isValidRevenueCode = true;
                }
                else if (result1.length > 0) {
                    //Check the CodeSet Rules here for Revenue Code, if is valid for one then stop otherwise check all specialties
                    //alert('In loop' + result1.length);
                    for (var i = 0; i < result1.length; i++) {
                        var SpecialtyTypeId = result1[i].SpecialtyTypeId;
                        if (isValidRevenueCode == true) {
                            //alert('i: ' + i);
                            GetCodeSetRule(SpecialtyTypeId);
                        }
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMessageonTop]").text('Error validating Revenue code.');
            }
        });
    }

    function GetCodeSetRule(SpecialtyTypeId) {
        var txtRevenueCode = $("#<%= txtInstRevenueCode.ClientID %>").first().val();
        var providerTypeId = $("[id*=hdnProviderTypeId]").val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetCodeSetRule?providerTypeId=" + providerTypeId + "&&specialtyTypeId=" + SpecialtyTypeId + "&&code=" + txtRevenueCode + "&&codeType=" + codeType + "&&claimsorPA=" + claimsOrPA + "&&claimORPAType=" + claimType,
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
                    //No rule then it is good
                }
                else if (result2.length == 1) {
                    //Check if isAllows
                    var isAllowed = result2[0].Allowed;
                    var errorMsg = result2[0].ErrorMessage;
                    //alert('isAllowed: ' + isAllowed);
                    if (isAllowed == "False") {
                        isValidRevenueCode = false;
                        $("[id*=txtInstRevenueCode]").val('');
                        $("[id*=lblErrorMessageonTop]").text('Revenue code is invalid. ' + errorMsg);
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMessageonTop]").text('Error validating Revenue code.');
            }
        });
    }

    function proccodechange() {
        var txtProcCode = $("#<%= txtInsProcCode.ClientID %>").first().val();
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
                    /*$("[id*=txtInsProcCode]").val('');*/
                    $("[id*=lblErrorProcedureCodeMsg]").text('Procedure code is invalid.');
                }
                else {
                    $("[id*=lblErrorProcedureCodeMsg]").text('');
                    $("[id*=valerrormess]").text('');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                /*$("[id*=txtInsProcCode]").val('');*/
                $("[id*=lblErrorProcedureCodeMsg]").text('Procedure code not found.');
            }
        });

        return false;
    }

    function HideLabel() {
        $("#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_valerrormess").text('');
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

    function validateInstitutionalServiceLineFields() {

        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth() + 1;
        var yyyy = today.getFullYear();
        var todaydate = mm + '/' + dd + '/' + yyyy;
        var validationResult = true;

        var claimid = document.getElementById("<%=hdnValueCode_ClaimID.ClientID %>").value;
        var revenueCode = $("#<%= txtInstRevenueCode.ClientID %>").first().val();
        var procedureType = $("#<%= lblInstProcType.ClientID %>").html();
        var procedureCode = $("#<%= txtInsProcCode.ClientID %>").first().val();
        var modifierDentalFirst = $("#<%= txtInsModifier1.ClientID %>").first().val();
        var modifierDentalSecond = $("#<%= txtInsModifier2.ClientID %>").first().val();
        var modifierDentalThird = $("#<%= txtInsModifier3.ClientID %>").first().val();
        var modifierDentalFourth = $("#<%= txtInsModifier4.ClientID %>").first().val();
        var lineControlNumber = $("#<%= txtInsLineCntrlNumber.ClientID %>").first().val();
        var fromDOB = $("#<%= txtInsFromDOS.ClientID %>").first().val();
        var toDOB = $("#<%= txtInsToDOS.ClientID %>").first().val();
        var nonCoveredCharges = $("#<%= tblInsNonCovCharges.ClientID %>").first().val();
        var status = $("#<%= lblInstStatus.ClientID %>").html();
        var Unit = $("#<%= txtInsUnit.ClientID %>").first().val();
        var UnitOfMeasurement = $("#<%=ddlInsUnitsOfMeasurement.ClientID %> option:selected").text();
        var totalCharges = $("#<%= txtInsTotalCharges.ClientID %>").first().val();
        var storedcharge = totalCharges;
        totalCharges = HandleDecimalValue(totalCharges).toString();
        if (totalCharges == null || totalCharges == "" || totalCharges == undefined) {
            if (storedcharge == "0" || storedcharge == "0.0" || storedcharge == "0.00" || storedcharge == "0.000") {
                totalCharges = "0.00";
            }
        }
        var paidAmt = $("#<%= lblInsPaidAmount.ClientID %>").html();
        var InsFinalEAPG = $("#<%= lblInsFinalEAPG.ClientID %>").html();
        var InsPaymentAction = $("#<%= lblInsPaymentAction.ClientID %>").html();
        var InsTotalAmountPaid = $("#<%= lblInsTotalAmountPaid.ClientID %>").html();
        var InsTotalAmountBilled = $("#<%= lblInsTotalAmountBilled.ClientID %>").html();


        if ((revenueCode === null || revenueCode === "" || revenueCode === undefined)) {
            $("[id*=lblErrorMessageonTop]").text('');
            $("[id*=rfvtxtInstRevenueCode]").text('*Revenue code is required.');
            validationResult = false;
        }
        else {
            $("[id*=rfvtxtInstRevenueCode]").text('');
        }

        if ((fromDOB === null || fromDOB === "" || fromDOB === undefined)) {
            $("#<%= revInsFromDOS.ClientID %>").html("*From DOS is required.");
            validationResult = false;
        }
        else {
            $("#<%= revInsFromDOS.ClientID %>").html("");
        }
        if (Unit === null || Unit === "" || Unit === undefined) {
            $("#<%= valInstServiceLineInfo.ClientID %>").html("*Unit is required");
            validationResult = false;
        }
        else {
            $("#<%= valInstServiceLineInfo.ClientID %>").html("");
        }

        if (UnitOfMeasurement === null || UnitOfMeasurement === "" || UnitOfMeasurement === undefined) {
            $("#<%= lblUnitOfMeasurementError.ClientID %>").html("*Unit of Measurement is required");
            validationResult = false;
        }
        else {
            $("#<%= lblUnitOfMeasurementError.ClientID %>").html("");
        }

        if (totalCharges === null || totalCharges === "" || totalCharges === undefined) {
            $("#<%= revInsTotalCharges.ClientID %>").html("*Total Charges is required.");
            validationResult = false;
        }
        else {
            $("#<%= revInsTotalCharges.ClientID %>").html("");
        }

        var serviceFrmDate = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtFromDate").val();
        var serviceToDate = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceInformationInstitutional_txtToDate").val();
        $("#<%= errorMessageValid.ClientID %>").html("");
        if (!(serviceFrmDate != null && serviceFrmDate != "" && serviceFrmDate != undefined && serviceToDate != null && serviceToDate != "" && serviceToDate != undefined)) {
            $("#<%= errorMessageValid.ClientID %>").append("<br />Please enter From Date and To Date in the service information panel before entering service details.");
            validationResult = false;
        }
        if (toDOB != null && toDOB != "" && toDOB != undefined) {
            if ((Date.parse(toDOB) > Date.parse(serviceToDate)) || (Date.parse(toDOB) <= Date.parse(serviceFrmDate))) {
                if ((Date.parse(toDOB) != Date.parse(serviceToDate)) && (Date.parse(toDOB) != Date.parse(serviceFrmDate))) {
                    $("#<%= errorMessageValid.ClientID %>").append("<br />To DOS should be reported be within the statement period");
                    validationResult = false;
                }
            }
        }
        if (fromDOB != null && fromDOB != "" && fromDOB != undefined) {
            if ((Date.parse(fromDOB) < Date.parse(serviceFrmDate)) || (Date.parse(fromDOB) > Date.parse(serviceToDate))) {
                if (((Date.parse(fromDOB) != Date.parse(serviceFrmDate)) || (Date.parse(fromDOB) != Date.parse(serviceToDate)))) {
                    $("#<%= errorMessageValid.ClientID %>").append("<br />From DOS should be reported be within the statement period");
                    validationResult = false;
                }
            }
        }
        if (((modifierDentalFirst != null && modifierDentalFirst != "" && modifierDentalFirst != undefined) ||
            (modifierDentalSecond != null && modifierDentalSecond != "" && modifierDentalSecond != undefined) ||
            (modifierDentalThird != null && modifierDentalThird != "" && modifierDentalThird != undefined) ||
            (modifierDentalFourth != null && modifierDentalFourth != "" && modifierDentalFourth != undefined)) &&
            (procedureCode == null || procedureCode == "" || procedureCode == undefined)) {
            $("#<%= errorMessageValid.ClientID %>").append("<br />*Modifiers can only be entered if procedure code is reported.");
            validationResult = false;
        }
        if (toDOB != null && toDOB != "" && toDOB != undefined) {
            if (Date.parse(toDOB) > Date.parse(todaydate)) {
                $("#<%= errorMessageValid.ClientID %>").append("<br />*To date cannot be future date.");
                validationResult = false;
            }

            if (Date.parse(fromDOB) > Date.parse(toDOB)) {
                $("#<%= errorMessageValid.ClientID %>").append("<br />*From date cannot be greater than To date.");
                validationResult = false;
            }
        }
        if (Date.parse(fromDOB) > Date.parse(todaydate)) {
            $("#<%= errorMessageValid.ClientID %>").append("<br />*From date cannot be future date");
            validationResult = false;
        }
        if (Unit != null && Unit != "" && Unit != undefined) {
            if (Unit <= 0) {
                $("#<%= errorMessageValid.ClientID %>").append("<br />* Unit should be greater than zero");
                validationResult = false;
            }
        }
        if (nonCoveredCharges == null && totalCharges == null && nonCoveredCharges == "" && totalCharges == "" && nonCoveredCharges == undefined && totalCharges == undefined) {
            $("#<%= errorMessageValid.ClientID %>").append("<br />*Either the Total Charges Amount or Non-Covered Charges Amount must be greater than $0.00");
            validationResult = false;
        }
        if (nonCoveredCharges != null && totalCharges != null && nonCoveredCharges != "" && totalCharges != "" && nonCoveredCharges != undefined && totalCharges != undefined) {
            if (nonCoveredCharges > totalCharges) {
                $("#<%= errorMessageValid.ClientID %>").append("<br />*Non covered charges cannot be greater than total charges.");
                validationResult = false;
            }
        }
        return validationResult;
    }

    function validateInstitutionalProcedureCodeModifiers() {
        var txtProcCode = $("#<%= txtInsProcCode.ClientID %>").first().val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        var providerTypeId = $("[id*=hdnProviderTypeId]").val();
        var validationResult = true;

        var claimid = document.getElementById("<%=hdnValueCode_ClaimID.ClientID %>").value;
        var procedureCode = $("#<%= txtInsProcCode.ClientID %>").first().val();
        var modifierDentalFirst = $("#<%= txtInsModifier1.ClientID %>").first().val();
        var modifierDentalSecond = $("#<%= txtInsModifier2.ClientID %>").first().val();
        var modifierDentalThird = $("#<%= txtInsModifier3.ClientID %>").first().val();
        var modifierDentalFourth = $("#<%= txtInsModifier4.ClientID %>").first().val();

        $.ajax({
            type: "GET",
            url: webApiClaims + "validateProcedureCodeModifiers?provider_type_id=" + providerTypeId + "&&procedure_code=" + txtProcCode + "&&M1=" + modifierDentalFirst + "&&M2=" + modifierDentalSecond + "&&M3=" + modifierDentalThird + "&&M4=" + modifierDentalFourth +"&&claimsorPA=claims&&claimORPAType=Institutional",
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

    function addInstitutionalServiceDetailCode() {
        var claimStatus = $("#ctl00_MainContent_uc5SubmitClaim_txtstatus2").val();
        $("[id*=lblErrorMessageonTop]").text('');
        $("[id*=valerrormess]").text('');
        var claimServiceDetailsInistList;
        var claimid = document.getElementById("<%=hdnValueCode_ClaimID.ClientID %>").value;
        var revenueCode = $("#<%= txtInstRevenueCode.ClientID %>").first().val();
        var procedureType = $("#<%= lblInstProcType.ClientID %>").html();
        var procedureCode = $("#<%= txtInsProcCode.ClientID %>").first().val();
        var modifierDentalFirst = $("#<%= txtInsModifier1.ClientID %>").first().val();
        var modifierDentalSecond = $("#<%= txtInsModifier2.ClientID %>").first().val();
        var modifierDentalThird = $("#<%= txtInsModifier3.ClientID %>").first().val();
        var modifierDentalFourth = $("#<%= txtInsModifier4.ClientID %>").first().val();
        var lineControlNumber = $("#<%= txtInsLineCntrlNumber.ClientID %>").first().val();
        var fromDOB = $("#<%= txtInsFromDOS.ClientID %>").first().val();
        var toDOB = $("#<%= txtInsToDOS.ClientID %>").first().val();
        var nonCoveredCharges = $("#<%= tblInsNonCovCharges.ClientID %>").first().val();
        var status = $("#<%= lblInstStatus.ClientID %>").html();
        var Unit = $("#<%= txtInsUnit.ClientID %>").first().val();
        var UnitOfMeasurement = $("#<%=ddlInsUnitsOfMeasurement.ClientID %> option:selected").text();
        var totalCharges = $("#<%= txtInsTotalCharges.ClientID %>").first().val();
        var storedcharge = totalCharges;
        totalCharges = HandleDecimalValue(totalCharges).toString();
        if (totalCharges == null || totalCharges == "" || totalCharges == undefined) {
            if (storedcharge == "0" || storedcharge == "0.0" || storedcharge == "0.00" || storedcharge == "0.000") {
                totalCharges = "0.00";
            }
        }
        var paidAmt = $("#<%= lblInsPaidAmount.ClientID %>").html();
        var InsFinalEAPG = $("#<%= lblInsFinalEAPG.ClientID %>").html();
        var InsPaymentAction = $("#<%= lblInsPaymentAction.ClientID %>").html();
        var InsTotalAmountPaid = $("#<%= lblInsTotalAmountPaid.ClientID %>").html();
        var InsTotalAmountBilled = $("#<%= lblInsTotalAmountBilled.ClientID %>").html();

        if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_revtxtInsTotalCharges') != null) {
            if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_revtxtInsTotalCharges').style.display != 'none') {
                return false;
            }
        }

        if (validateInstitutionalServiceLineFields() && validateInstitutionalProcedureCodeModifiers()) {
            $("[id*=lblErrorMessageonTop]").text('');
            var txtRevenueCode = $("#<%= txtInstRevenueCode.ClientID %>").first().val();
            $("[id*=lblErrorMessageonTop]").text('');
            if ((txtRevenueCode != undefined) && (txtRevenueCode != null) && (txtRevenueCode != "")) {
                if (txtRevenueCode.length < 4) {
                    $("[id*=txtInstRevenueCode]").val('');
                    $("[id*=lblErrorMessageonTop]").text('4-character value is required');
                }
                else {
                    var APIToken = $("[id*=hdnAccessToken]").val();
                    $.ajax({
                        type: "GET",
                        url: webApiClaims + "GetRevenueCodeDetails?val=" + txtRevenueCode,
                        //data: '{desc: "" , val: "' + txtRevenueCode + '" }',
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
                                $("[id*=txtInstRevenueCode]").val('');
                                $("[id*=lblErrorMessageonTop]").text('Revenue code is invalid.');
                            }
                            else if (result.length > 1) {
                                $("[id*=txtInstRevenueCode]").val('');
                                $("[id*=lblErrorMessageonTop]").text('Revenue code is invalid.');
                            }
                            else {
                                var txtProcCode = $("#<%= txtInsProcCode.ClientID %>").first().val();
                                if (txtProcCode != null && txtProcCode != undefined && txtProcCode != "") {
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
                                                $("[id*=txtInsProcCode]").val('');
                                                $("[id*=lblErrorProcedureCodeMsg]").text('Procedure code is invalid.');
                                                return false;
                                            }
                                            else {
                                                $("[id*=lblErrorProcedureCodeMsg]").text('');
                                                $("[id*=valerrormess]").text('');
                                                addInstiServiceDetail();
                                            }
                                        },
                                        error: function (jqXHR, textStatus, errorThrown) {
                                            $("[id*=txtInsProcCode]").val('');
                                            $("[id*=lblErrorProcedureCodeMsg]").text('Procedure code not found.');
                                        }
                                    });
                                }
                                else { addInstiServiceDetail(); }
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            $("[id*=lblErrorMessageonTop]").text('Revenue code not found.');
                        }
                    });
                }
            }
            return false;
        } else {
            return false;
        }

        function addInstiServiceDetail() {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "POST",
                url: webApiClaims + "AddInstitutionalServiceDetails?claimid=" + claimid + "&&InsTotalAmountBilled=" + InsTotalAmountBilled + "&&InsTotalAmountPaid=" + InsTotalAmountPaid + "&&InsFinalEAPG=" + InsFinalEAPG + "&&InsPaymentAction=" + InsPaymentAction + "&&paidAmt=" + paidAmt + "&&revenueCode=" + revenueCode + "&&procedureType=" + procedureType + "&&procedureCode=" + procedureCode + "&&modifierDentalFirst=" + modifierDentalFirst + "&&modifierDentalSecond=" + modifierDentalSecond + "&&modifierDentalThird=" + modifierDentalThird + "&&modifierDentalFourth=" + modifierDentalFourth + "&&lineControlNumber=" + lineControlNumber + "&&fromDOB=" + fromDOB + "&&toDOB=" + toDOB + "&&nonCoveredCharges=" + nonCoveredCharges + "&&status=" + status + "&&Unit=" + Unit + "&&UnitOfMeasurement=" + UnitOfMeasurement + "&&totalCharges=" + totalCharges + "&&user=<%=HttpContext.Current.User.Identity.Name%>",
                //data: '{claimid: "' + claimid + '" ,InsTotalAmountBilled: "' + InsTotalAmountBilled + '",InsTotalAmountPaid: "' + InsTotalAmountPaid + '",InsFinalEAPG: "' + InsFinalEAPG + '" ,InsPaymentAction: "' + InsPaymentAction + '"  ,paidAmt: "' + paidAmt + '" ,revenueCode: "' + revenueCode + '" , procedureType: "' + procedureType + '" , procedureCode: "' + procedureCode + '" , modifierDentalFirst: "' + modifierDentalFirst + '" , modifierDentalSecond: "' + modifierDentalSecond + '" , modifierDentalThird: "' + modifierDentalThird + '" , modifierDentalFourth: "' + modifierDentalFourth + '" , lineControlNumber: "' + lineControlNumber + '" , fromDOB: "' + fromDOB + '" , toDOB: "' + toDOB + '" , nonCoveredCharges: "' + nonCoveredCharges + '" , status: "' + status + '" , Unit: "' + Unit + '" , UnitOfMeasurement: "' + UnitOfMeasurement + '" , totalCharges: "' + totalCharges + '"  }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    clearInstiServiceDetailFieldsOnClickOfCancel();
                    displayInstiServiceDetailtable();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorMessageInstitutional]").text('Error Adding Service Line');
                }
            });
        }
    }

    function CopyInstiServiceLineItem(Claim_service_Id, claim_id, service_Line, control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        var hdnHidePaidAmounts = document.getElementById("<%=hdnHidePaidAmounts.ClientID %>").value;
        $("[id*=rfvtxtInstRevenueCode]").text('');
        $("#<%= revInsFromDOS.ClientID %>").html("");
        $("#<%= valInstServiceLineInfo.ClientID %>").html("");
        $("#<%= lblUnitOfMeasurementError.ClientID %>").html("");
        $("#<%= revInsTotalCharges.ClientID %>").html("");
        $("#<%= errorMessageValid.ClientID %>").html("");
        binddataSerDet();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "CopyServiceDetail?Claim_service_Id=" + Claim_service_Id + "&&claim_id=" + claim_id + "&&serviceline=" + service_Line,
            //data: '{Claim_service_Id: "' + Claim_service_Id + '", claim_id:"' + claim_id + '", serviceline:"' + service_Line + '"}',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function () {
                displayInstiServiceDetailtable(service_Line, control);
                $(control).closest('tr').find('input[type=button]').prop('disabled', true);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblIcdVersionError]").text('Diagnosis Code is invalid');
            }
        });
    }

    function EditInstiServiceLineItem(Claim_service_Id, claim_id, service_Line, control, isCopy=0) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        var hdnHidePaidAmounts = document.getElementById("<%=hdnHidePaidAmounts.ClientID %>").value;
        $("[id*=rfvtxtInstRevenueCode]").text('');
        $("#<%= revInsFromDOS.ClientID %>").html("");
        $("#<%= valInstServiceLineInfo.ClientID %>").html("");
        $("#<%= lblUnitOfMeasurementError.ClientID %>").html("");
        $("#<%= revInsTotalCharges.ClientID %>").html("");
        $("#<%= errorMessageValid.ClientID %>").html("");
        binddataSerDet();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetIndInstiServiceDetail?Claim_service_Id=" + Claim_service_Id + "&&claim_id=" + claim_id + "&&service_Line=" + service_Line,
            //data: '{Claim_service_Id: "' + Claim_service_Id + '", claim_id:"' + claim_id + '", service_Line:"' + service_Line + '"}',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                claimDiagnosisList = result;
                if (claimDiagnosisList.length > 0) {
                    for (var i = 0; i < claimDiagnosisList.length; i++) {
                        var service_Line = result[i].service_Line;
                        var sequence = service_Line;
                        if (isCopy != 1) {
                            $("#<%= lblDetailsItemInst.ClientID %>").html("" + sequence + "");
                        }                     

                        var revenue_Code = result[i].revenue_Code;
                        var cde_proc = result[i].cde_proc;
                        var procedure_type = result[i].procedure_type;
                        var unit = result[i].unit;
                        var unit_of_measurement = result[i].unit_of_measurement;
                        var fromDOS = new Date(result[i].fromDOS);
                        fromDOS = formatDate(fromDOS);

                        var toDOS = result[i].toDOS;

                        if (toDOS != null && toDOS != "" && toDOS != undefined) {
                            toDOS = formatDate(new Date(toDOS));
                        } else { toDOS = ""; }

                        var total_charges = result[i].total_charges;
                        var total_charges1 = total_charges;
                        total_charges = HandleDecimalValue(total_charges).toString();
                        if (total_charges == null || total_charges == "" || total_charges == undefined) {
                            if (total_charges1 == "0" || total_charges1 == "0.0" || total_charges1 == "0.00" || total_charges1 == "0.000") {
                                total_charges = "0.00";
                            }
                        }
                        var paid_amount = result[i].paid_amount;
                        paid_amount = HandleDecimalValue(paid_amount).toString();
                        if (paid_amount == 'undefined' || paid_amount == null || paid_amount == "") {
                            paid_amount = "0";
                        }
                        else {
                            paid_amount = HandleDecimalValue(paid_amount).toString();
                        }
                        
                        var status = result[i].status;
                        var Claim_service_Id = result[i].Claim_service_Id;
                        var Claim_Id = result[i].Claim_Id;
                        var mdf_first = result[i].mdf_first;
                        var mdf_secnd = result[i].mdf_secnd;
                        var mdf_thrd = result[i].mdf_thrd;
                        var mdf_forth = result[i].mdf_forth;
                        var line_ctr_num = result[i].line_ctr_num;
                        var final_EAPG = result[i].final_EAPG;
                        var payment_Action = result[i].payment_Action;
                        var non_Covered_Charges = result[i].non_Covered_Charges;
                        var total_amount_billed = result[i].total_amount_billed;
                        var total_amount_paid = result[i].total_amount_paid;
                        if (hdnHidePaidAmounts == "1") {
                            paid_amount = "";
                            total_amount_paid = "";
                        }
                        $("[id*=hdnServiceLine]").val("" + service_Line + "");
                        $("[id*=hdnValueCode_ClaimID]").val("" + Claim_Id + "");
                        $("[id*=hdnDentalClaimServiceId]").val("" + Claim_service_Id + "");
                        if (isCopy == 1) {
                            document.getElementById("<%= instiSerDiv.ClientID%>").innerHTML = " <input type='submit' name='ctl00$MainContent$uc5SubmitClaim$InstitutionalServiceDetails$InstInfoAdd' value='ADD' onclick='return addInstitutionalServiceDetailCode();' id='ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstInfoAdd' class='btn btn-primary' style='font-weight:bold;width:70px; visibility: visible;'>";
                            document.getElementById("<%= instiSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$InstitutionalServiceDetails$btnUpdateServiceDetailInsti' value = 'Update' onclick = 'return UpdateInstiServiceDetail();' id='ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_btnUpdateServiceDetailInsti' class='btn btn-primary' style = 'font-weight: bold; width: 70px; visibility: hidden;'>";
                            document.getElementById("<%= instiSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$InstitutionalServiceDetails$btnCancelInsti' value='Cancel' onclick='clearInstiServiceDetailFieldsOnClickOfCancel(); displayInstiServiceDetailtable();' id='ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_btnCancelInsti' class='btn btn-primary' style='font-weight: bold; width: 70px; visibility: visible;'>";
                        } else {
                            document.getElementById("<%= instiSerDiv.ClientID%>").innerHTML = " <input type='submit' name='ctl00$MainContent$uc5SubmitClaim$InstitutionalServiceDetails$InstInfoAdd' value='ADD' onclick='return addInstitutionalServiceDetailCode();' id='ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstInfoAdd' class='btn btn-primary' style='font-weight:bold;width:70px; visibility: hidden;'>";
                            document.getElementById("<%= instiSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$InstitutionalServiceDetails$btnUpdateServiceDetailInsti' value = 'Update' onclick = 'return UpdateInstiServiceDetail();' id='ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_btnUpdateServiceDetailInsti' class='btn btn-primary' style = 'font-weight: bold; width: 70px; visibility: visible;'>";
                            document.getElementById("<%= instiSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$InstitutionalServiceDetails$btnCancelInsti' value='Cancel' onclick='clearInstiServiceDetailFieldsOnClickOfCancel(); displayInstiServiceDetailtable();' id='ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_btnCancelInsti' class='btn btn-primary' style='font-weight: bold; width: 70px; visibility: visible;'>";
                        }
                        $("#<%= txtInstRevenueCode.ClientID %>").first().val(revenue_Code);
                        document.getElementById('<%= lblInstProcType.ClientID%>').innerHTML = procedure_type;
                        $("#<%= txtInsProcCode.ClientID %>").first().val(cde_proc);
                        $("#<%= txtInsModifier1.ClientID %>").first().val(mdf_first);
                        $("#<%= txtInsModifier2.ClientID %>").first().val(mdf_secnd);
                        $("#<%= txtInsModifier3.ClientID %>").first().val(mdf_thrd);
                        $("#<%= txtInsModifier4.ClientID %>").first().val(mdf_forth);
                        $("#<%= txtInsLineCntrlNumber.ClientID %>").first().val(line_ctr_num);
                        $("#<%= txtInsFromDOS.ClientID %>").first().val(fromDOS);
                        $("#<%= txtInsToDOS.ClientID %>").first().val(toDOS);
                        $("#<%= tblInsNonCovCharges.ClientID %>").first().val(non_Covered_Charges);
                        $("#<%= txtInsUnit.ClientID %>").first().val(unit);
                        $('#<%=ddlInsUnitsOfMeasurement.ClientID%>').val(unit_of_measurement);
                        $("#<%= txtInsTotalCharges.ClientID %>").first().val(total_charges);
                        $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_lblInsPaidAmount').text(paid_amount);
                        $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_lblInsFinalEAPG').text(final_EAPG);
                        $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_lblInsPaymentAction').text(payment_Action);
                        if (total_amount_paid != null && total_amount_paid != "" && total_amount_paid != undefined) {
                            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_lblInsTotalAmountPaid').text(total_amount_paid);
                        }
                        if (total_amount_billed != null && total_amount_billed != "" && total_amount_billed != undefined) {
                            $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_lblInsTotalAmountBilled').text(total_amount_billed.toFixed(2));
                        }
                        $('#ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_lblInstStatus').text(status);

                        return false;
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblIcdVersionError]").text('Diagnosis Code is invalid');
            }
        });
        return false;
    }


    function clearInstiServiceDetailFieldsOnClickOfCancel() {
        var claimid = document.getElementById("<%=hdnValueCode_ClaimID.ClientID %>").value;
        $("#<%= lblInsTotalAmountPaid.ClientID %>").html("");
        $("#<%= lblInsTotalAmountBilled.ClientID %>").html("");
        clearInstiServiceDetailFields();
        document.getElementById("<%= instiSerDiv.ClientID%>").innerHTML = "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$InstitutionalServiceDetails$InstInfoAdd' value='ADD' onclick='return addInstitutionalServiceDetailCode();' id='ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_InstInfoAdd' class='btn btn-primary' style='font-weight:bold;width:70px; visibility: visible;'>";
        document.getElementById("<%= instiSerDiv.ClientID%>").innerHTML += "<input type = 'submit' name = 'ctl00$MainContent$uc5SubmitClaim$InstitutionalServiceDetails$btnUpdateServiceDetailInsti' value = 'Update' onclick = 'return UpdateInstiServiceDetail();' id='ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_btnUpdateServiceDetailInsti' class='btn btn-primary' style = 'font-weight: bold; width: 70px; visibility: hidden;'>";
        document.getElementById("<%= instiSerDiv.ClientID%>").innerHTML += "<input type='submit' name='ctl00$MainContent$uc5SubmitClaim$InstitutionalServiceDetails$btnCancelInsti' value='Cancel' onclick='clearInstiServiceDetailFieldsOnClickOfCancel(); displayInstiServiceDetailtable();' id='ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_btnCancelInsti' class='btn btn-primary' style='font-weight: bold; width: 70px; visibility: visible;'>";
        return false;
    }
    function clearInstiServiceDetailFields() {
        $("#<%= txtInstRevenueCode.ClientID %>").first().val("");
        $("#<%= txtInsProcCode.ClientID %>").first().val("");
        $("#<%= txtInsModifier1.ClientID %>").first().val("");
        $("#<%= txtInsModifier2.ClientID %>").first().val("");
        $("#<%= txtInsModifier3.ClientID %>").first().val("");
        $("#<%= txtInsModifier4.ClientID %>").first().val("");
        $("#<%= txtInsLineCntrlNumber.ClientID %>").first().val("");
        $("#<%= txtInsFromDOS.ClientID %>").first().val("");
        $("#<%= txtInsToDOS.ClientID %>").first().val("");
        $("#<%= tblInsNonCovCharges.ClientID %>").first().val("");
        $("#<%= txtInsUnit.ClientID %>").first().val("");
        $("#<%= txtInsTotalCharges.ClientID %>").first().val("");
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_ddlInsUnitsOfMeasurement") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_ddlInsUnitsOfMeasurement").options[0].selected = true;
        }
        $("#<%= lblInsPaidAmount.ClientID %>").html("");
        $("#<%= lblInsFinalEAPG.ClientID %>").html("");
        $("#<%= lblInsPaymentAction.ClientID %>").html("");
    }


    function UpdateInstiServiceDetail() {
        var claimStatus = $("#ctl00_MainContent_uc5SubmitClaim_txtstatus2").val();
        var claimServiceDetailsInistList;
        var claimid = document.getElementById("<%=hdnValueCode_ClaimID.ClientID %>").value;
        var revenueCode = $("#<%= txtInstRevenueCode.ClientID %>").first().val();
        var procedureType = $("#<%= lblInstProcType.ClientID %>").html();
        var procedureCode = $("#<%= txtInsProcCode.ClientID %>").first().val();
        var modifierDentalFirst = $("#<%= txtInsModifier1.ClientID %>").first().val();
        var modifierDentalSecond = $("#<%= txtInsModifier2.ClientID %>").first().val();
        var modifierDentalThird = $("#<%= txtInsModifier3.ClientID %>").first().val();
        var modifierDentalFourth = $("#<%= txtInsModifier4.ClientID %>").first().val();
        var lineControlNumber = $("#<%= txtInsLineCntrlNumber.ClientID %>").first().val();
        var fromDOB = $("#<%= txtInsFromDOS.ClientID %>").first().val();
        var toDOB = $("#<%= txtInsToDOS.ClientID %>").first().val();
        var nonCoveredCharges = $("#<%= tblInsNonCovCharges.ClientID %>").first().val();
        var status = $("#<%= lblInstStatus.ClientID %>").html();
        var Unit = $("#<%= txtInsUnit.ClientID %>").first().val();
        var UnitOfMeasurement = $("#<%=ddlInsUnitsOfMeasurement.ClientID %> option:selected").text();
        var total_charges = $("#<%= txtInsTotalCharges.ClientID %>").first().val();
        var storedcharge = total_charges;
        total_charges = HandleDecimalValue(total_charges).toString();
        if (total_charges == null || total_charges == "" || total_charges == undefined) {
            if (storedcharge == "0" || storedcharge == "0.0" || storedcharge == "0.00" || storedcharge == "0.000") {
                total_charges = "0.00";
            }
        }
        var paidAmt = $("#<%= lblInsPaidAmount.ClientID %>").html();
        var InsFinalEAPG = $("#<%= lblInsFinalEAPG.ClientID %>").html();
        var InsPaymentAction = $("#<%= lblInsPaymentAction.ClientID %>").html();
        var InsTotalAmountPaid = $("#<%= lblInsTotalAmountPaid.ClientID %>").html();
        var InsTotalAmountBilled = $("#<%= lblInsTotalAmountBilled.ClientID %>").html();
        var service_line = document.getElementById("<%=hdnServiceLine.ClientID %>").value;
        


        if (validateInstitutionalServiceLineFields() && validateInstitutionalProcedureCodeModifiers()) {
            var txtRevenueCode = $("#<%= txtInstRevenueCode.ClientID %>").first().val();
            $("[id*=lblErrorMessageonTop]").text('');
            if ((txtRevenueCode != undefined) && (txtRevenueCode != null) && (txtRevenueCode != "")) {
                if (txtRevenueCode.length < 4) {
                    $("[id*=txtInstRevenueCode]").val('');
                    $("[id*=lblErrorMessageonTop]").text('4-character value is required');
                }
                else {
                    var APIToken = $("[id*=hdnAccessToken]").val();
                    $.ajax({
                        type: "GET",
                        url: webApiClaims + "GetRevenueCodeDetails?val=" + txtRevenueCode,
                        //data: '{desc: "" , val: "' + txtRevenueCode + '" }',
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

                                $("[id*=txtInstRevenueCode]").val('');
                                $("[id*=lblErrorMessageonTop]").text('Revenue code is invalid.');
                            }
                            else if (result.length > 1) {
                                $("[id*=txtInstRevenueCode]").val('');
                                $("[id*=lblErrorMessageonTop]").text('Revenue code is invalid.');
                            }
                            else {
                                var txtProcCode = $("#<%= txtInsProcCode.ClientID %>").first().val();
                                if (txtProcCode != null && txtProcCode != undefined && txtProcCode != "") {
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
                                                $("[id*=txtInsProcCode]").val('');
                                                $("[id*=lblErrorProcedureCodeMsg]").text('Procedure code is invalid.');
                                                return false;
                                            }
                                            else {
                                                $("[id*=lblErrorProcedureCodeMsg]").text('');
                                                $("[id*=valerrormess]").text('');
                                                updateServiceInstiCode();
                                                clearInstiServiceDetailFieldsOnClickOfCancel();
                                            }
                                        },
                                        error: function (jqXHR, textStatus, errorThrown) {
                                            $("[id*=txtInsProcCode]").val('');
                                            $("[id*=lblErrorProcedureCodeMsg]").text('Procedure code not found.');
                                        }
                                    });
                                }
                                else {
                                    updateServiceInstiCode();
                                    clearInstiServiceDetailFieldsOnClickOfCancel();
                                }
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            $("[id*=lblErrorMessageonTop]").text('Revenue code not found.');
                        }
                    });
                }
            }
        }
        function updateServiceInstiCode() {
            var hdnHidePaidAmounts = document.getElementById("<%=hdnHidePaidAmounts.ClientID %>").value;
            if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_revtxtInsTotalCharges') != null) {
                if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_revtxtInsTotalCharges').style.display != 'none') {
                    return false;
                }
            }
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "PUT",
                url: webApiClaims + "EditInstiServiceDetailCode?claimid=" + claimid + "&&service_line=" + service_line + "&&InsTotalAmountBilled=" + InsTotalAmountBilled + "&&InsTotalAmountPaid=" + InsTotalAmountPaid + "&&InsFinalEAPG=" + InsFinalEAPG + "&&InsPaymentAction=" + InsPaymentAction + "&&paidAmt=" + paidAmt + "&&revenueCode=" + revenueCode + "&&procedureType=" + procedureType + "&&procedureCode=" + procedureCode + "&&modifierDentalFirst=" + modifierDentalFirst + "&&modifierDentalSecond=" + modifierDentalSecond + "&&modifierDentalThird=" + modifierDentalThird + "&&modifierDentalFourth=" + modifierDentalFourth + "&&lineControlNumber=" + lineControlNumber + "&&fromDOB=" + fromDOB + "&&toDOB=" + toDOB + "&&nonCoveredCharges=" + nonCoveredCharges + "&&status=" + status + "&&Unit=" + Unit + "&&UnitOfMeasurement=" + UnitOfMeasurement + "&&totalCharges=" + total_charges + "&&user=<%=HttpContext.Current.User.Identity.Name%>",
                //data: '{claimid: "' + claimid + '" ,service_line: "' + service_line + '" ,InsTotalAmountBilled: "' + InsTotalAmountBilled + '",InsTotalAmountPaid: "' + InsTotalAmountPaid + '",InsFinalEAPG: "' + InsFinalEAPG + '" ,InsPaymentAction: "' + InsPaymentAction + '"  ,paidAmt: "' + paidAmt + '" ,revenueCode: "' + revenueCode + '" , procedureType: "' + procedureType + '" , procedureCode: "' + procedureCode + '" , modifierDentalFirst: "' + modifierDentalFirst + '" , modifierDentalSecond: "' + modifierDentalSecond + '" , modifierDentalThird: "' + modifierDentalThird + '" , modifierDentalFourth: "' + modifierDentalFourth + '" , lineControlNumber: "' + lineControlNumber + '" , fromDOB: "' + fromDOB + '" , toDOB: "' + toDOB + '" , nonCoveredCharges: "' + nonCoveredCharges + '" , status: "' + status + '" , Unit: "' + Unit + '" , UnitOfMeasurement: "' + UnitOfMeasurement + '" , totalCharges: "' + total_charges + '"  }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    displayInstiServiceDetailtable();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorMessageInstitutional]").text('Error Updating Service Line');
                }
            });
        }
        return false;
    }

    function DeleteInstiServiceDetailLineitem(Claim_Id, service_Line) {
        var claimServiceDetailsDentalList;
        <%--var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;--%>
        var claimStatus = $("#ctl00_MainContent_uc5SubmitClaim_txtstatus2").val();
        var result = confirm("Are you sure you want to delete claim line: " + service_Line + "?");
        if (result) {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteClaimServiceLine?service_line=" + service_Line + "&&claimid=" + Claim_Id,
                //data: '{service_line: "' + service_Line + '" , claimid: "' + Claim_Id + '" }',
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
                    displayInstiServiceDetailtable();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblIcdVersionError]").text('Error in Deleting Claim Line');
                }
            });
        }
    }


    function InstiServiceDetailModifierChanged(e) {
        var variable = "";
        var modifierNo = "";
        if (e.id === "ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_txtInsModifier1") {
            variable = $("#<%= txtInsModifier1.ClientID %>").first().val();
            modifierNo = "1";
        }
        if (e.id === "ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_txtInsModifier2") {
            variable = $("#<%= txtInsModifier2.ClientID %>").first().val();
            modifierNo = "2";
        }
        if (e.id === "ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_txtInsModifier3") {
            variable = $("#<%= txtInsModifier3.ClientID %>").first().val();
            modifierNo = "3";
        }
        if (e.id === "ctl00_MainContent_uc5SubmitClaim_InstitutionalServiceDetails_txtInsModifier4") {
            variable = $("#<%= txtInsModifier4.ClientID %>").first().val();
            modifierNo = "4";
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
                    $("#<%= errorMessageValid.ClientID %>").html("");

              } else if (result === "false") {
                  $("#<%= errorMessageValid.ClientID %>").html("*Procedure modifier" + modifierNo + " is invalid.");
                  return false;
              }
          },
          error: function (jqXHR, textStatus, errorThrown) {
              $("[id*=lblIcdVersionError]").text('Diagnosis Code is invalid');
          }
      });

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
</script>
<style>
    .header-center {
        text-align: center;
    }

    .gridview {
        width: 100%;
        word-wrap: break-word;
        table-layout: fixed;
    }

    .cssPager td {
        padding-left: 4px;
        padding-right: 4px;
    }
</style>
<asp:HiddenField runat="server"  ID="hdnHidePaidAmounts" Value="0"/>
<asp:UpdatePanel ID="upInstServiceLineDetails" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="div_instiServiceDetail">
            <asp:Label runat="server" ID="lblErrorMessageonTop" CssClass="failureNotification"></asp:Label>
            <div id="serviceDetailInsti" runat="server">
                <%--<asp:GridView ID="gvInstServiceLineDetails" CssClass="gridview" runat="server" Width="100%" BorderColor="Transparent" RowStyle-Height="50px" PagerStyle-HorizontalAlign="Center" OnRowDataBound="gvInstServiceLineDetails_RowDataBound"
                AutoGenerateColumns="false" AllowPaging="true" ShowHeaderWhenEmpty="true" OnRowCancelingEdit="gvInstServiceLineDetails_RowCancelingEdit" OnRowEditing="gvInstServiceLineDetails_RowEditing" OnRowDeleting="gvInstServiceLineDetails_RowDeleting" OnRowUpdating="gvInstServiceLineDetails_RowUpdating" OnPageIndexChanging="gvInstServiceLineDetails_PageIndexChanging"
                AlternatingRowStyle-BackColor="White" PageSize="5" HeaderStyle-CssClass="header-center" HorizontalAlign="Center" BorderWidth="0px" GridLines="None" BorderStyle="None" HeaderStyle-HorizontalAlign="Center" Style="margin-bottom: 14px">
                <EditRowStyle HorizontalAlign="Center" />
                <PagerStyle HorizontalAlign="Center" CssClass="cssPager" />
                <Columns>
                    <asp:BoundField DataField="Service_Line" ItemStyle-HorizontalAlign="Center" ReadOnly="true" HeaderText="Service Line">
                        <ItemStyle Width="80px" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle Width="80px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Revenue_Code" ItemStyle-HorizontalAlign="Center" ReadOnly="true" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderText="Revenue Code">
                        <ItemStyle Width="100px" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle Width="100px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Procedure_Type" ItemStyle-HorizontalAlign="Center" ReadOnly="true" HeaderText="Procedure Type">
                        <ItemStyle Width="80px" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle Width="80px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Procedure_code" ItemStyle-HorizontalAlign="Center" ReadOnly="true" HeaderText="Procedure Code">
                        <ItemStyle Width="80px" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle Width="80px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Billed_Units" ItemStyle-HorizontalAlign="Center" ReadOnly="true" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderText="Unit">
                        <ItemStyle Width="60px" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Unit_Of_Measurement" ItemStyle-HorizontalAlign="Center" ReadOnly="true" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderText="Unit of Measurement">
                        <ItemStyle Width="80px" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle Width="80px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Date_of_Service" ItemStyle-HorizontalAlign="Center" ReadOnly="true" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderText="From DOS">
                       <ItemStyle Width="80px" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle Width="80px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="To_Date_of_Service" ItemStyle-HorizontalAlign="Center" ReadOnly="true" HeaderText="To DOS">
                        <ItemStyle Width="90px" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle Width="90px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Total_Charges" ItemStyle-HorizontalAlign="Center" ReadOnly="true" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderText="Total Charges">
                       <ItemStyle Width="90px" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle Width="90px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Paid_Amount" ItemStyle-HorizontalAlign="Center" ReadOnly="true" HeaderText="Paid Amount">
                       <ItemStyle Width="50px" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle Width="50px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="Status" ItemStyle-HorizontalAlign="Center" ReadOnly="true" HeaderText="Status">
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:BoundField>
                    <asp:BoundField DataField="Status" ItemStyle-HorizontalAlign="Center" ReadOnly="true" HeaderText="Status">
                        <ItemStyle Width="70px" HorizontalAlign="Center"></ItemStyle>
                        <HeaderStyle Width="70px" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:CommandField HeaderStyle-Width="160px" ShowEditButton="true" ItemStyle-HorizontalAlign="Center" ShowDeleteButton="true" ControlStyle-Font-Size="Medium" ControlStyle-Font-Underline="false"
                        ControlStyle-ForeColor="White" ControlStyle-BorderStyle="Solid" ControlStyle-Font-Bold="true" CausesValidation="false">
                        <ControlStyle BorderStyle="Solid" Font-Bold="True" Font-Size="Medium" Font-Underline="False" ForeColor="White"></ControlStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                    </asp:CommandField>
                </Columns>

                <PagerSettings PageButtonCount="5" />
                <PagerStyle HorizontalAlign="Center" />

                <RowStyle Height="50px"></RowStyle>
            </asp:GridView>--%>
            </div>
        </div>
        <div id="ServiceLine1" runat="server">

            <div id="ServiceLine" onload="CheckProp()">
                <div style="float: right">
                    <div id="instibillSec" runat="server">
                        <div class="col-sm-5" style="height: 20px;">
                            <span class="ohio-field" style="font-size: 15px; text-align: right">Total Amount Billed:</span>
                        </div>
                        <div class="col-sm-7">
                            <span style="text-align: left; font-size: 15px">
                                <asp:Label ID="lblInsTotalAmountBilled" runat="server"></asp:Label>
                            </span>
                        </div>
                        <div class="col-sm-5">
                            <span class="ohio-field" style="font-size: 15px; text-align: right">Total Amount Paid:</span>
                        </div>
                        <div class="col-sm-3">
                            <asp:Label ID="lblInsTotalAmountPaid" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div style="float: left; width: 55%;" id="instiSerDiv" runat="server">
                        <button id="btnloading" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>

                        <asp:Button ID="InstInfoAdd" Text="ADD" OnClientClick="return addInstitutionalServiceDetailCode();"
                            runat="server" CssClass="btn btn-primary" Font-Bold="True" Width="70px" EnableViewState="False" CausesValidation="false" />
                        <br />
                        <asp:Button ID="btnUpdateServiceDetailInsti" Style="visibility: hidden" Text="Update" runat="server" CssClass="btn btn-primary"
                            Font-Bold="True" Width="70px" OnClientClick="return UpdateInstiServiceDetail();" EnableViewState="False" CausesValidation="false" />
                        <br />
                        <asp:Button ID="btnCancelInsti" Style="visibility: hidden" Text="Cancel" runat="server" CssClass="btn btn-primary"
                            Font-Bold="True" Width="70px" OnClientClick="return clearInstiServiceDetailFieldsOnClickOfCancel();" EnableViewState="False" CausesValidation="false" />

                    </div>

                </div>
                <div class="clearfix"></div>
                <div style="padding-left: 40px;">
                    <asp:Label ID="errorMessageValid" runat="server" ForeColor="Red"></asp:Label>
                </div>
                <div id="divLightblueHeader" class="row" style="background-color: lightblue;">

                    <div class="col-sm-2" style="font-size: 15px; text-align: right; padding-left: 50px; padding-top: 2px; height: 25px;">
                        <span class="ohio-field" style="font-size: 14px;">Service Line:
                        <asp:Label ID="lblDetailsItemInst" runat="server" Height="16px" ReadOnly="true"
                            Width="52px" />
                        </span>
                    </div>
                </div>
                <div id="divInnerInstiFields">
                    <div>
                        <div style="float: left; width: 35%; height: 50px;">
                            <div class="col-sm-5" style="padding-left: 75px">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Revenue code:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtInstRevenueCode" OnChange="return renuecodechange()" MaxLength="4" ValidationGroup="valInstServiceLineInfo" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 100px; min-width: 100px;" />
                                    <asp:LinkButton ID="lnkOPNPI3" runat="server" Text="Search" ToolTip="Search" OnClientClick="return visibleRevenueCode()" Visible="true" Style="font-size: 13px;"></asp:LinkButton><br />
                                   
                                    <asp:Label runat="server" ID="lblErrorMessageInstitutional" CssClass="failureNotification"></asp:Label>
                                    <asp:Label ID="rfvtxtInstRevenueCode" runat="server" ForeColor="Red"></asp:Label>
                                </span>
                            </div>

                        </div>
                        <div style="float: left; width: 35%; height: 50px">
                            <div class="col-sm-5" style="padding-left: 30px">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">From DOS:</span>
                            </div>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtInsFromDOS" OnChange="validateDate9()" ValidationGroup="valInstServiceLineInfo" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 100px; min-width: 100px;" />
                                <button id="btnloading1" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>

                                <asp:Image ID="imgServiceToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                                <ajax:CalendarExtender ID="ceInsFromDOS" runat="server" Format="MM/dd/yyyy" TargetControlID="txtInsFromDOS" PopupPosition="BottomRight" CssClass="QstCalendarCSS" EnabledOnClient="true" />
                                <br />
                                <asp:Label ID="revInsFromDOS" runat="server" ForeColor="Red"></asp:Label>
                                <asp:Label ID="InstServiceDetailDateRequiredError1" runat="server" Text="" CssClass="error-message" Style="display: none;"></asp:Label>
                                <asp:Label ID="lblErrorFromDOS" runat="server" ForeColor="Red" Width="500px"></asp:Label>

                            </div>
                        </div>
                        <div style="float: left; width: 25%; height: 50px">
                            <div class="col-sm-5" style="padding-left: 30px">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Status:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left; font-size: 15px">
                                    <asp:Label ID="lblInstStatus" Text="Pending Submission" runat="server"></asp:Label>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div>

                        <div style="float: left; width: 35%; height: 50px">
                            <div class="col-sm-5" style="padding-left: 30px">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Procedure Type:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left; font-size: 15px">
                                    <asp:Label ID="lblInstProcType" Text="HCPCS" runat="server"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <div style="float: left; width: 35%; height: 50px">
                            <div class="col-sm-5" style="padding-left: 30px">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">To DOS:</span>
                            </div>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtInsToDOS" OnChange="validateDate10()" ValidationGroup="valInstServiceLineInfo" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 100px; min-width: 100px;" />
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                                <ajax:CalendarExtender ID="ceInsToDOS" runat="server" Format="MM/dd/yyyy" TargetControlID="txtInsToDOS" PopupPosition="BottomRight" PopupButtonID="imgDate" CssClass="QstCalendarCSS" EnabledOnClient="true" />
                                <button id="btnloading2" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>
                                <%-- <asp:CompareValidator ID="InsFutureToDOS" runat="server" ValidationGroup="valInstServiceLineInfo"
                                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtInsToDOS"
                                ErrorMessage="To date cannot be a future date" Text="*" ValueToCompare="MM/dd/yyyy"
                                SetFocusOnError="true"></asp:CompareValidator>
                            

                            <asp:CompareValidator ID="InsFutureToDOS2" runat="server" Operator="LessThanEqual"
                                ControlToValidate="txtInsToDOS" ValidationGroup="VldGrpToDate"  ForeColor="Red"  ErrorMessage="<div>Future date not allowed</div>"
                                Display="Dynamic"  SetFocusOnError="true" Type="Date" />--%>
                                <asp:Label ID="InstServiceDetailToDateRequiredError1" runat="server" Text="" CssClass="error-message" Style="display: none;"></asp:Label>

                                <asp:CompareValidator runat="server" ID="cmtxtInsToDOS"
                                    ErrorMessage="<div>To DOS must be greater than or equal to From DOS</div>" Display="Dynamic"
                                    ControlToValidate="txtInsToDOS" ForeColor="Red" Type="Date" Operator="GreaterThanEqual" ControlToCompare="txtInsFromDOS" />
                                <asp:Label ID="lblErrorToDOS" runat="server" ForeColor="Red"></asp:Label>


                                <%--<asp:RequiredFieldValidator runat="server" ID="revInsToDOS" SetFocusOnError="true" ForeColor="Red"
                    ValidationGroup="valInstServiceLineInfo" ControlToValidate="txtInsFromDOS" ErrorMessage="*To DOS is required." Text="*" Display="Dynamic" />--%>
                            </div>
                        </div>
                        <div style="float: left; width: 25%; height: 50px;">
                            <div class="col-sm-5" style="padding-left: 75px">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Unit:</span>
                            </div>

                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtInsUnit" MaxLength="18" onkeypress="return event.charCode == 46 || (event.charCode >= 48 && event.charCode <= 57)" ValidationGroup="valInstServiceLineInfo" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 100px; min-width: 100px;"></asp:TextBox>
                                    <asp:Label ID="valInstServiceLineInfo" runat="server" ForeColor="Red"></asp:Label>
                                </span>
                            </div>

                        </div>
                    </div>

                    <div>

                        <div style="float: left; width: 35%; height: 50px;">
                            <div class="col-sm-5" style="padding-left: 75px">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Procedure code:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtInsProcCode" OnChange="return proccodechange()" MaxLength="5" ValidationGroup="valInstServiceLineInfo" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 100px; min-width: 100px;" />
                                    <asp:LinkButton ID="lnkInsProcCode" runat="server" Text="Search" ToolTip="Search" OnClientClick="return visibleProcedureCode()" Visible="true" Style="font-size: 13px;"></asp:LinkButton>
                                    <asp:RegularExpressionValidator runat="server" ID="revProcDentail"
                                        Display="Dynamic" ValidationGroup="validateDentalServiceControl"
                                        ControlToValidate="txtInsProcCode"
                                        ValidationExpression="^[\s\S]{5,}$" ForeColor="Red"
                                        ErrorMessage="<div id='msg4_rev_txtPlaceofService' >*  5-character value is required</div>"> 
                                    </asp:RegularExpressionValidator>
                                    <asp:Label runat="server" ID="lblErrorProcedureCodeMsg" CssClass="failureNotification"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <div style="float: left; width: 35%; height: 50px" id="divFinalEAPG" runat="server">
                            <div class="col-sm-5" style="padding-left: 93px">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Final EAPG:</span>
                            </div>
                            <div class="col-sm-7">
                                <asp:Label ID="lblInsFinalEAPG" runat="server"></asp:Label>
                            </div>
                        </div>

                        <div style="float: left; width: 25%; height: 50px">
                            <div class="col-sm-5" style="padding-left: 50px">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Unit of Measurement:</span>
                            </div>
                            <div class="col-sm-7">
                                <asp:DropDownList ID="ddlInsUnitsOfMeasurement" EnableViewState="true" runat="server"
                                    AppendDataBoundItems="True" Style="height: 30px; width: 10px; min-width: 60px;">
                                </asp:DropDownList>
                            </div>
                            <asp:Label ID="lblUnitOfMeasurementError" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <div>

                        <div style="float: left; width: 35%; height: 50px">
                            <div class="col-sm-5" style="padding-left: 30px">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Procedure Modifier:</span>
                            </div>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtInsModifier1" onChange="return InstiServiceDetailModifierChanged(this)" MaxLength="2" runat="server" CssClass="formfieldMod" Style="height: 30px; width: 2%; min-width: 40px;" onKeyDown="HideLabel()" />
                                <asp:TextBox ID="txtInsModifier2" onChange="return InstiServiceDetailModifierChanged(this)" MaxLength="2" runat="server" CssClass="formfieldMod" Style="height: 30px; width: 2%; min-width: 40px;" onKeyDown="HideLabel()" />
                                <asp:TextBox ID="txtInsModifier3" onChange="return InstiServiceDetailModifierChanged(this)" MaxLength="2" runat="server" CssClass="formfieldMod" Style="height: 30px; width: 2%; min-width: 40px;" onKeyDown="HideLabel()" />
                                <asp:TextBox ID="txtInsModifier4" onChange="return InstiServiceDetailModifierChanged(this)" MaxLength="2" runat="server" CssClass="formfieldMod" Style="height: 30px; width: 2%; min-width: 40px;" onKeyDown="HideLabel()" />
                            </div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtInsModifier1" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtInsModifier2" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtInsModifier3" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtInsModifier4" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                        </div>
                        <div style="float: left; width: 35%; height: 50px" id="divPaymentAction" runat="server">
                            <div class="col-sm-5" style="padding-left: 10px">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Payment Action:</span>
                            </div>
                            <div class="col-sm-7">
                                <asp:Label ID="lblInsPaymentAction" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div style="float: left; width: 25%; height: 50px;">
                            <div class="col-sm-5" style="padding-left: 75px">
                                <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: right">Total Charges:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtInsTotalCharges" MaxLength="18" ValidationGroup="valInstServiceLineInfo" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 100px; min-width: 100px;" /><br />
                                    <asp:Label ID="revInsTotalCharges" runat="server" ForeColor="Red"></asp:Label>
                                </span>
                                <asp:RegularExpressionValidator ID="revtxtInsTotalCharges" runat="server" ControlToValidate="txtInsTotalCharges" ValidationExpression="^(-?\d+\.)?-?\d+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            </div>

                        </div>
                    </div>

                    <div>

                        <div style="float: left; width: 35%; height: 50px">
                            <div class="col-sm-5" style="padding-left: 45px">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Line Control Number:</span>
                            </div>
                            <div class="col-sm-7">
                                <asp:TextBox ID="txtInsLineCntrlNumber" runat="server" MaxLength="50" CssClass="formfieldDate" Style="height: 30px; width: 100px; min-width: 100px;" />
                            </div>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtInsLineCntrlNumber" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                        </div>

                        <div style="float: left; width: 35%; height: 50px; visibility: hidden;">
                            <div class="col-sm-5" style="padding-left: 75px">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Non Covered Charges:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="tblInsNonCovCharges" MaxLength="18" ValidationGroup="valInstServiceLineInfo" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 100px; min-width: 100px;" />
                                </span>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="tblInsNonCovCharges" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            </div>

                        </div>
                    </div>
                    <div>
                        <div style="float: left; width: 25%; height: 50px" id="divPaidAmount" runat="server">
                            <div class="col-sm-5" style="padding-left: 30px">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Paid Amount:</span>
                            </div>
                            <div class="col-sm-7">
                                <asp:Label ID="lblInsPaidAmount" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div style="float: left; width: 100%;">
            <asp:ValidationSummary ID="valerrormess" DisplayMode="List" runat="server" CssClass="failureNotification"
                ValidationGroup="valInstServiceLineInfo" />


        </div>

        <asp:HiddenField ID="hdnValueCode_ClaimID" runat="server" />
        <asp:HiddenField ID="hdnServiceLine" runat="server" />
        <asp:HiddenField ID="hdnInstiServiceLineClaimStatus" runat="server" />
        <asp:HiddenField ID="hdnProviderTypeId" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

<ajax:ModalPopupExtender BehaviorID="mperProcedureCode" ID="mpeSubmitClaimSearchProc" runat="server" PopupControlID="pnlSubmitClaimSearchProcPop"
    TargetControlID="ButtonSearchProc" BackgroundCssClass="modalBackground" CancelControlID="btnCloseProc" />
<asp:Panel ID="pnlSubmitClaimSearchProcPop" runat="server" CssClass="modalPopup"
    Style="display: none; min-height: 100px; min-width: 900px; height: auto; width: auto;">
    <asp:Panel ID="pnlSubmitClaimSearchProcHeader" CssClass="popHeader" runat="server"
        HorizontalAlign="Left">
        <asp:Button runat="server" ID="btnCloseProc" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
            OnClick="btnCloseProc_Click" OnClientClick="CloseProcedureCodePopup();"
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


<ajax:ModalPopupExtender BehaviorID="mperRevenueCode" ID="mpeSubmitClaimSearchRevenueCode" runat="server" PopupControlID="pnlSubmitClaimSearchRevenueCodePop"
    TargetControlID="ButtonSearchRevCode" BackgroundCssClass="modalBackground" CancelControlID="btnCloseRevCode" />
<asp:Panel ID="pnlSubmitClaimSearchRevenueCodePop" runat="server" CssClass="modalPopup" Style="display: none; min-height: 500px; min-width: 800px; height: auto; width: auto;">
    <asp:Panel ID="pnlSubmitClaimSearchRevenueCodeHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
        <asp:Button runat="server" ID="btnCloseRevCode" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;" OnClick="btnCloseRevCode_Click" CausesValidation="false" />
        <div class="search-Results">
            <span style="padding-left: 10px; font-size: 17px;">Revenue Code</span>
            <span style="padding-left: 90px; font-size: 17px;">Revenue Code Description</span>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlSubmitClaimSearchRevenueCode" runat="server">
        <div style="text-align: left; padding: 15px" class="container-fluid">
            <div class="row">
                <uc:SubmitClaimSearchRevenueCode runat="server" ID="ucSubmitClaimSearchRevenueCode" Visible="true" EnableViewState="true" />
            </div>
        </div>
    </asp:Panel>
</asp:Panel>
<asp:Button runat="server" ID="ButtonSearchRevCode" Style="display: none" Text="ButtonSearchReson" />