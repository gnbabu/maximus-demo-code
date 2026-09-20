<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_OtherPayerPaidAmountServiceDetail, App_Web_rqhgepvh" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">


<script>
    function GetOtherPayerPaidAmountBind()
        {
        var ClaimID = $("#<%= hdnOPPAmountClaimID.ClientID %>").first().val();
        var ClaimType = $("#<%= hdnOPPAmountClaimType.ClientID %>").first().val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetOtherPayerPaidAmountBind?ClaimId=" + ClaimID + "&&ClaimType=" + ClaimType+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var AddOPPAServiceDetailsList = result;
                var table = "";
                if (AddOPPAServiceDetailsList.length > 0) {
                    if (ClaimType == 1) {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Revenue Code</th><th style='width:10px; scope='col'>Procedure Code</th > <th style='width:10px; scope=' col'> Health Plan ID</th ><th style='width:10px; scope=' col'> Amount Paid</th ><th style='width:10px; scope=' col'> Paid Date</th ><th style='width:10px; scope=' col'> Paid Service Unit Count</th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th ><th style='width: 10px; ' scope='col'>&nbsp;</th><th style='width: 10px; ' scope='col'>&nbsp;</th></tr > ";
                    }
                    else {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Procedure Code</th > <th style='width:10px; scope=' col'> Health Plan ID</th ><th style='width:10px; scope=' col'> Amount Paid</th ><th style='width:10px; scope=' col'> Paid Date</th ><th style='width:10px; scope=' col'> Paid Service Unit Count</th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th ><th style='width: 10px; ' scope='col'>&nbsp;</th><th style='width: 10px; ' scope='col'>&nbsp;</th></tr > ";
                    }
                    for (var i = 0; i < AddOPPAServiceDetailsList.length; i++) {
                        var Service_Line = parseInt(result[i].Service_Line);

                        var Revenue_Code = result[i].Revenue_Code;
                        var Procedure_Code = result[i].Procedure_Code;
                        var Health_Plan_ID = result[i].Health_Plan_ID;
                        var Amount_Paid = result[i].Amount_Paid;
                        var Paid_Date = result[i].Paid_Date;
                        var Paid_unit_Count = result[i].Paid_unit_Count;
                        var OPPAServiceDetailId = result[i].OPPAServiceDetailId;
                        var ClaimId = result[i].ClaimId;
                        if (ClaimType == 1) {
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Service_Line + "</span></td><td><span title='Line' class='tNumber'>" + Revenue_Code + "</span></td><td><span  title='Line' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Line' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Line' class='tNumber'>" + Amount_Paid + "</span></td><td><span title='Line' class='tNumber'>" + Paid_Date + "</span></td><td><span title='Line' class='tNumber'>" + Paid_unit_Count + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOPPAServiceDetailLineItem(\"" + OPPAServiceDetailId + "\",\"" + ClaimId + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOPPAServiceDetailLItem(\"" + OPPAServiceDetailId + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr > ";
                        }
                        else {
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Service_Line + "</span></td><td><span  title='Line' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Line' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Line' class='tNumber'>" + Amount_Paid + "</span></td><td><span title='Line' class='tNumber'>" + Paid_Date + "</span></td><td><span title='Line' class='tNumber'>" + Paid_unit_Count + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOPPAServiceDetailLineItem(\"" + OPPAServiceDetailId + "\",\"" + ClaimId + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOPPAServiceDetailLItem(\"" + OPPAServiceDetailId + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr > ";
                        }
                    };
                    table = table + "</tbody></table>";
                    document.getElementById('<%= OPPAServiceDetailOutput.ClientID %>').innerHTML = table;

                }
                else { document.getElementById('<%= OPPAServiceDetailOutput.ClientID %>').innerHTML = ""; }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMsgOtherPayerPaid]").text('Other Payer paid amount code not found.');
            }
        });
    }


    function clearOPPAServiceDetailTableContent() {
        document.getElementById('<%= OPPAServiceDetailOutput.ClientID %>').innerHTML = "";
    }
    function displaytableOtherPayer() {
        var OtherPayerPaidclaimstatus = document.getElementById("<%=hdnOtherPayerPaidClaimStatus.ClientID %>").value;
        if (OtherPayerPaidclaimstatus == "Pending Submission") {
            var table = localStorage.getItem("OPPAServiceDetailTable");
        }
        else {
            var table = localStorage.removeItem("OPPAServiceDetailTable");
        }

        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= OPPAServiceDetailOutput.ClientID %>').innerHTML = table;

        }
    };

    function loaderOtherPaidAmountServiceDetails() {

        var detailsId = $("#<%=ddlDetailId.ClientID %> option:selected").text(); 
        var HealthPlanId = $("#<%=ddlOPPHealhPlanId.ClientID %> option:selected").text();
        var OppAmount = $("#<%= txtOPPAmount.ClientID %>").first().val();
        var PaidServiceCount = $("#<%= txtOtherPaidServiceCount.ClientID %>").first().val();
       

        if (detailsId != "" && HealthPlanId != "" && OppAmount != "" && PaidServiceCount != "") {

            document.getElementById('<%= ddlDetailId.ClientID %>').disabled = true;
            document.getElementById('<%= ddlOPPHealhPlanId.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAmount.ClientID %>').disabled = true;
            document.getElementById('<%= txtOtherPaidServiceCount.ClientID %>').disabled = true;

            document.getElementById('<%= btnOPPAdd.ClientID %>').style.display = 'none';
        }
    }

    function ClearFields() {
        $("#<%=ddlDetailId.ClientID %>").prop('selectedIndex', 0);
        $("#<%=ddlOPPHealhPlanId.ClientID %>").prop('selectedIndex', 0);
        $("#<%= lbltOPPProcedureCode.ClientID %>").html('');
        $("#<%= lblOPPPaidDate.ClientID %>").html('');
        $("#<%= lbltOPPRevenueCode.ClientID %>").html('');
        $("#<%= txtOPPAmount.ClientID %>").first().val("");
        $("#<%= txtOtherPaidServiceCount.ClientID %>").first().val("");
    }
    function AddOPPAserviceDetails()
    {
        var validate = "true";
        $("[id*=lblErrorMsgOtherPayerPaid]").text('');
        if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_lblOtherPayerAdjustmentGridError') != null)
        {
            document.getElementById('ctl00_MainContent_uc5SubmitClaim_lblOtherPayerAdjustmentGridError').innerHTML = '';
        }

        var Service_Line = $("#<%=ddlDetailId.ClientID %> option:selected").text();
        var Health_Plan_ID = $("#<%=ddlOPPHealhPlanId.ClientID %> option:selected").text();
        var Amount_Paid = $("#<%= txtOPPAmount.ClientID %>").first().val();
        var Paid_unit_Count;
        if ($("#<%= txtOtherPaidServiceCount.ClientID %>").first().val() != null && $("#<%= txtOtherPaidServiceCount.ClientID %>").first().val() != "") {
            Paid_unit_Count = parseInt($("#<%= txtOtherPaidServiceCount.ClientID %>").first().val());
        }
        var hdnOPPBillUnit;
        if ($("#<%= hdnOPPBillUnit.ClientID %>").first().val() != null && $("#<%= hdnOPPBillUnit.ClientID %>").first().val() != "") {
            hdnOPPBillUnit = parseInt($("#<%= hdnOPPBillUnit.ClientID %>").first().val());
        }
        var ClaimID = $("#<%= hdnOPPAmountClaimID.ClientID %>").first().val();
        var ClaimType = $("#<%= hdnOPPAmountClaimType.ClientID %>").first().val();
        var Procedure_Code = $("#<%= lbltOPPProcedureCode.ClientID %>").html();
        var Paid_Date = $("#<%= lblOPPPaidDate.ClientID %>").html();
        var Revenue_Code = $("#<%= lbltOPPRevenueCode.ClientID %>").html();
        if (Service_Line == null || Service_Line == " " || Service_Line == undefined) {
            $("[id*=lblErrorMsgOtherPayerPaid]").text('Service line number is required for other payer paid amount.');
            validate = "false";
            return false;
        }
        if (Health_Plan_ID == null || Health_Plan_ID == "" || Health_Plan_ID == undefined) {
            $("[id*=lblErrorMsgOtherPayerPaid]").text('Health Plan ID is required.');
            validate = "false";
            return false;
        }
        if (Amount_Paid == null || Amount_Paid == "" || Amount_Paid == undefined) {
            $("[id*=lblErrorMsgOtherPayerPaid]").text('Amount paid is required.');
            validate = "false";
            return false;
        }
        if (Paid_unit_Count == null || Paid_unit_Count == "" || Paid_unit_Count == undefined) {
            $("[id*=lblErrorMsgOtherPayerPaid]").text('Other payer paid service unit count is required.');
            validate = "false";
            return false;
        }
        if ((Paid_unit_Count) > (hdnOPPBillUnit)) {
            $("[id*=lblErrorMsgOtherPayerPaid]").text('Other payer paid service unit count cannot be greater than billed unit');
            validate = "false";
            return false;
        }
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetOtherPayerPaidAmountBind?ClaimId=" + ClaimID + "&&ClaimType=" + ClaimType+"",
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
                for (var i = 0; i < OtherPayerAdjustmentDetails.length; i++) {
                    var aService_Line = parseInt(result[i].Service_Line);
                    var aRevenue_Code = result[i].Revenue_Code;
                    var aProcedure_Code = result[i].Procedure_Code;
                    var aHealth_Plan_ID = result[i].Health_Plan_ID;

                    if ((Service_Line == aService_Line) && (aRevenue_Code == Revenue_Code) && (aHealth_Plan_ID == Health_Plan_ID) && (aProcedure_Code == Procedure_Code)) {
                        $("[id*=lblErrorMsgOtherPayerPaid]").text('Service Line, Revenue Code, Health Plan, and Procedure Code must be unique');
                        validate = "false";
                        return false;
                    }

                    if ((Service_Line == aService_Line) && (aHealth_Plan_ID == Health_Plan_ID))
                    {
                        $("[id*=lblErrorMsgOtherPayerPaid]").text('Only one payer may be listed for each service line');
                        validate = "false";
                        return false;
                    }
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMsgOtherPayerPaid]").text('Only one payer may be listed for each service line');
            }
        });

        if (validate == "true")
        {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "POST",
                url: webApiClaims + "AddOPPAServiceDetails?ClaimID=" + ClaimID + "&&Service_Line=" + Service_Line + "&&Procedure_Code=" + Procedure_Code + "&&Health_Plan_ID=" + Health_Plan_ID + "&&Amount_Paid=" + Amount_Paid + "&&Paid_Date=" + Paid_Date + "&&Paid_unit_Count=" + Paid_unit_Count + "&&ClaimType=" + ClaimType + "&&Revenue_Code=" + Revenue_Code + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var AddOPPAServiceDetailsList = result;
                        var table = "";
                    if (AddOPPAServiceDetailsList.length > 0) {
                        if (ClaimType == 1) {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Revenue Code</th><th style='width:10px; scope='col'>Procedure Code</th > <th style='width:10px; scope=' col'> Health Plan ID</th ><th style='width:10px; scope=' col'> Amount Paid</th ><th style='width:10px; scope=' col'> Paid Date</th ><th style='width:10px; scope=' col'> Paid Service Unit Count</th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th ><th style='width: 10px; ' scope='col'>&nbsp;</th><th style='width: 10px; ' scope='col'>&nbsp;</th></tr > ";
                        }
                        else {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Procedure Code</th > <th style='width:10px; scope=' col'> Health Plan ID</th ><th style='width:10px; scope=' col'> Amount Paid</th ><th style='width:10px; scope=' col'> Paid Date</th ><th style='width:10px; scope=' col'> Paid Service Unit Count</th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th ><th style='width: 10px; ' scope='col'>&nbsp;</th><th style='width: 10px; ' scope='col'>&nbsp;</th></tr > ";
                        }
                        for (var i = 0; i < AddOPPAServiceDetailsList.length; i++) {
                            var Service_Line = parseInt(result[i].Service_Line);
                           
                            var Revenue_Code = result[i].Revenue_Code;
                            var Procedure_Code = result[i].Procedure_Code;
                            var Health_Plan_ID = result[i].Health_Plan_ID;
                            var Amount_Paid = result[i].Amount_Paid;
                            var Paid_Date = result[i].Paid_Date;
                            var Paid_unit_Count = result[i].Paid_unit_Count;
                            var OPPAServiceDetailId = result[i].OPPAServiceDetailId;
                            var ClaimId = result[i].ClaimId;
                            if (ClaimType == 1) {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Service_Line + "</span></td><td><span title='Line' class='tNumber'>" + Revenue_Code + "</span></td><td><span  title='Line' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Line' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Line' class='tNumber'>" + Amount_Paid + "</span></td><td><span title='Line' class='tNumber'>" + Paid_Date + "</span></td><td><span title='Line' class='tNumber'>" + Paid_unit_Count + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOPPAServiceDetailLineItem(\"" + OPPAServiceDetailId + "\",\"" + ClaimId + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOPPAServiceDetailLItem(\"" + OPPAServiceDetailId + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr > ";
                            }
                            else {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Service_Line + "</span></td><td><span  title='Line' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Line' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Line' class='tNumber'>" + Amount_Paid + "</span></td><td><span title='Line' class='tNumber'>" + Paid_Date + "</span></td><td><span title='Line' class='tNumber'>" + Paid_unit_Count + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOPPAServiceDetailLineItem(\"" + OPPAServiceDetailId + "\",\"" + ClaimId + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOPPAServiceDetailLItem(\"" + OPPAServiceDetailId + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr > ";
                            }
                            };
                            table = table + "</tbody></table>";
                            localStorage.setItem("OPPAServiceDetailTable", "" + table + "");
                            document.getElementById('<%= OPPAServiceDetailOutput.ClientID %>').innerHTML = table;
                        
                        
                        }
                    },

                    error: function (jqXHR, textStatus, errorThrown) {
                        $("[id*=lblErrorMsgOtherPayerPaid]").text('Other Payer paid amount code not found.');
                    }
                });
        }
        //GetServiceLineList();
        GetHealthPlanId();
        ClearFields();
        return false;
    }
    function EditOPPAServiceDetailLineItem(OPPAServiceDetailId, ClaimId, control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        $("[id*=lblErrorMsgOtherPayerPaid]").text('');
        var ClaimType = $("#<%= hdnOPPAmountClaimType.ClientID %>").first().val();
        var EditOPPAServiceDetailsList;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "POST",
            url: webApiClaims + "EditOPPAServiceDetailLineItem?ClaimId=" + ClaimId + "&&OPPAServiceDetailId=" + OPPAServiceDetailId,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                EditOPPAServiceDetailsList = result;
                if (EditOPPAServiceDetailsList.length > 0) {                    
                    for (var i = 0; i < EditOPPAServiceDetailsList.length; i++) {
                        var Service_Line = parseInt(result[i].Service_Line);
                        var Revenue_Code = result[i].Revenue_Code;
                        var Procedure_Code = result[i].Procedure_Code;
                        var Health_Plan_ID = result[i].Health_Plan_ID;
                        var Amount_Paid = result[i].Amount_Paid;
                        var Paid_Date = result[i].Paid_Date;
                        var Paid_unit_Count = result[i].Paid_unit_Count;
                        var OPPAServiceDetailId = result[i].OPPAServiceDetailId;
                        var ClaimId = result[i].ClaimId;
                        $("[id*=Other_Payer_Adjudication_Information_Service_Detail_ID]").val("" + OPPAServiceDetailId + "");                      

                        $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlDetailId option:contains("' + Service_Line + '")').prop('selected', true);
                        $('#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId option:contains("' + Health_Plan_ID + '")').prop('selected', true);

                        document.getElementById('<%= lbltOPPProcedureCode.ClientID%>').innerHTML = Procedure_Code;
                        document.getElementById('<%= lblOPPPaidDate.ClientID%>').innerHTML = Paid_Date;
                        $("[id*=txtOPPAmount]").val("" + Amount_Paid + "");
                        $("[id*=txtOtherPaidServiceCount]").val("" + Paid_unit_Count + "");
                        if (ClaimType == 1) {
                            document.getElementById('<%= lbltOPPRevenueCode.ClientID%>').innerHTML = Revenue_Code;
                        }
                        document.getElementById('<%= btnOPPAdd.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "visible";
                        
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMsgOtherPayerPaid]").text('Error occurred while editing a record.');
            }
        });
    }

    function DeleteOPPAServiceDetailLItem(OPPAServiceDetailId) {
        var ClaimID = $("#<%= hdnOPPAmountClaimID.ClientID %>").first().val();
        var ClaimType = $("#<%= hdnOPPAmountClaimType.ClientID %>").val();
        var result = confirm("Are you sure you want to delete?");
        $("[id*=lblErrorMsgOtherPayerPaid]").text('');
        if (result) {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteOPPAServiceDetail?ClaimID=" + ClaimID + "&&OPPAServiceDetailId=" + OPPAServiceDetailId,
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var OPPAServiceDetailsList = result;
                    var table = "";
                    if (OPPAServiceDetailsList.length > 0) {
                        if (ClaimType == 1) {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Revenue Code</th><th style='width:10px; scope='col'>Procedure Code</th > <th style='width:10px; scope=' col'> Health Plan ID</th ><th style='width:10px; scope=' col'> Amount Paid</th ><th style='width:10px; scope=' col'> Paid Date</th ><th style='width:10px; scope=' col'> Paid Service Unit Count</th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th ><th style='width: 10px; ' scope='col'>&nbsp;</th><th style='width: 10px; ' scope='col'>&nbsp;</th></tr > ";
                        }
                        else {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Procedure Code</th > <th style='width:10px; scope=' col'> Health Plan ID</th ><th style='width:10px; scope=' col'> Amount Paid</th ><th style='width:10px; scope=' col'> Paid Date</th ><th style='width:10px; scope=' col'> Paid Service Unit Count</th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th ><th style='width: 10px; ' scope='col'>&nbsp;</th><th style='width: 10px; ' scope='col'>&nbsp;</th></tr > ";
                        }
                        for (var i = 0; i < OPPAServiceDetailsList.length; i++) {

                            var Service_Line = parseInt(result[i].Service_Line);
                            var Revenue_Code = result[i].Revenue_Code;
                            var Procedure_Code = result[i].Procedure_Code;
                            var Health_Plan_ID = result[i].Health_Plan_ID;
                            var Amount_Paid = result[i].Amount_Paid;
                            var Paid_Date = result[i].Paid_Date;
                            var Paid_unit_Count = result[i].Paid_unit_Count;
                            var OPPAServiceDetailId = result[i].OPPAServiceDetailId;
                            var ClaimId = result[i].ClaimId;
                            if (ClaimType == 1) {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Service_Line + "</span></td><td><span title='Line' class='tNumber'>" + Revenue_Code + "</span></td><td><span  title='Line' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Line' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Line' class='tNumber'>" + Amount_Paid + "</span></td><td><span title='Line' class='tNumber'>" + Paid_Date + "</span></td><td><span title='Line' class='tNumber'>" + Paid_unit_Count + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOPPAServiceDetailLineItem(\"" + OPPAServiceDetailId + "\",\"" + ClaimId + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOPPAServiceDetailLItem(\"" + OPPAServiceDetailId + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr > ";

                            } else {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Service_Line + "</span></td><td><span  title='Line' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Line' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Line' class='tNumber'>" + Amount_Paid + "</span></td><td><span title='Line' class='tNumber'>" + Paid_Date + "</span></td><td><span title='Line' class='tNumber'>" + Paid_unit_Count + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOPPAServiceDetailLineItem(\"" + OPPAServiceDetailId + "\",\"" + ClaimId + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOPPAServiceDetailLItem(\"" + OPPAServiceDetailId + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr > ";
                            }
                        };
                        table = table + "</tbody></table>";
                        localStorage.setItem("OPPAServiceDetailTable", "" + table + "");
                        document.getElementById('<%= OPPAServiceDetailOutput.ClientID %>').innerHTML = table;
                       
                    }
                    else {
                        document.getElementById('<%= OPPAServiceDetailOutput.ClientID %>').innerHTML = "";
                        document.getElementById('<%= btnOPPAdd.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden"; 
                        document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "hidden";

                        return false;
                    }
                   
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorMsgOtherPayerPaid]").text('Other payer paid amount code not found.');
                }
            });
        }

        ClearFields();
        document.getElementById('<%= btnOPPAdd.ClientID%>').style.visibility = "visible";
        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "hidden";
        return false;
    }

    function onlyDotsAndNumbersWithNegetive(txt, event) {
        
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
    function UpdateOPPServiceDetails() {
        $("[id*=lblErrorMsgOtherPayerPaid]").text('');
        var OPPAServiceDetailID = $("#<%= Other_Payer_Adjudication_Information_Service_Detail_ID.ClientID %>").first().val();
        var Service_Line = $("#<%=ddlDetailId.ClientID %> option:selected").text();
        var Health_Plan_ID = $("#<%=ddlOPPHealhPlanId.ClientID %> option:selected").text();
        var Amount_Paid = $("#<%= txtOPPAmount.ClientID %>").first().val();
        var Paid_unit_Count = $("#<%= txtOtherPaidServiceCount.ClientID %>").first().val();
        var hdnOPPBillUnit = $("#<%= hdnOPPBillUnit.ClientID %>").first().val();
        var ClaimID = $("#<%= hdnOPPAmountClaimID.ClientID %>").first().val();
        var ClaimType = $("#<%= hdnOPPAmountClaimType.ClientID %>").first().val();
        var Procedure_Code = $("#<%= lbltOPPProcedureCode.ClientID %>").html();
        var Paid_Date = $("#<%= lblOPPPaidDate.ClientID %>").html();
        var Revenue_Code = $("#<%= lbltOPPRevenueCode.ClientID %>").html();
        var validate = "true";
        if (Service_Line == null || Service_Line == "" || Service_Line == undefined) {
            $("[id*=lblErrorMsgOtherPayerPaid]").text('Service Line Number is required.');
            validate = "false";
            return false;
        }
        if (Health_Plan_ID == null || Health_Plan_ID == "" || Health_Plan_ID == undefined) {
            $("[id*=lblErrorMsgOtherPayerPaid]").text('Health Plan ID is required.');
            validate = "false";
            return false;
        }
        if (Amount_Paid == null || Amount_Paid == "" || Amount_Paid == undefined) {
            $("[id*=lblErrorMsgOtherPayerPaid]").text('Amount paid is required.');
            validate = "false";
            return false;
        }
        if (Paid_unit_Count == null || Paid_unit_Count == "" || Paid_unit_Count == undefined) {
            $("[id*=lblErrorMsgOtherPayerPaid]").text('Other payer paid service unit count is required.');
            validate = "false";
            return false;
        }
        if (ClaimID != null && ClaimID != "undefined" && ClaimID != "") {
           
            if (validate == "true") {
                var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "POST",
                    url: webApiClaims + "UpdateOPPAserviceDeatils?ClaimID=" + ClaimID + "&&OPPAServiceDetailID=" + OPPAServiceDetailID + "&&Service_Line=" + Service_Line + "&&Health_Plan_ID=" + Health_Plan_ID + "&&Amount_Paid=" + Amount_Paid + "&&Paid_unit_Count=" + Paid_unit_Count + "&&Revenue_Code=" + Revenue_Code + "&&ClaimType=" + ClaimType + "&&Procedure_Code=" + Procedure_Code + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                    headers: {
                        "Access-Control-Allow-Origin": "*",
                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                        "Authorization": "Bearer " + APIToken
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var OPPAServiceDetailsList = result;
                        var table = "";
                        if (result[0].ErrorMsg != null) {
                            $("[id*=lblErrorMsgOtherPayerPaid]").text(result[0].ErrorMsg);
                            return false;
                        }
                        else {
                            if (OPPAServiceDetailsList.length > 0) {
                                if (ClaimType == 1) {
                                    table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Revenue Code</th><th style='width:10px; scope='col'>Procedure Code</th > <th style='width:10px; scope=' col'> Health Plan ID</th ><th style='width:10px; scope=' col'> Amount Paid</th ><th style='width:10px; scope=' col'> Paid Date</th ><th style='width:10px; scope=' col'> Paid Service Unit Count</th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th ><th style='width: 10px; ' scope='col'>&nbsp;</th><th style='width: 10px; ' scope='col'>&nbsp;</th></tr > ";
                                }
                                else {
                                    table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Procedure Code</th > <th style='width:10px; scope=' col'> Health Plan ID</th ><th style='width:10px; scope=' col'> Amount Paid</th ><th style='width:10px; scope=' col'> Paid Date</th ><th style='width:10px; scope=' col'> Paid Service Unit Count</th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th ><th style='width: 10px; ' scope='col'>&nbsp;</th><th style='width: 10px; ' scope='col'>&nbsp;</th></tr > ";
                                }
                                for (var i = 0; i < OPPAServiceDetailsList.length; i++) {

                                    var Service_Line = parseInt(result[i].Service_Line);
                                    var Revenue_Code = result[i].Revenue_Code;
                                    var Procedure_Code = result[i].Procedure_Code;
                                    var Health_Plan_ID = result[i].Health_Plan_ID;
                                    var Amount_Paid = result[i].Amount_Paid;
                                    var Paid_Date = result[i].Paid_Date;
                                    var Paid_unit_Count = result[i].Paid_unit_Count;
                                    var OPPAServiceDetailId = result[i].OPPAServiceDetailId;
                                    var ClaimId = result[i].ClaimId;
                                    if (ClaimType == 1) {
                                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Service_Line + "</span></td><td><span title='Line' class='tNumber'>" + Revenue_Code + "</span></td><td><span  title='Line' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Line' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Line' class='tNumber'>" + Amount_Paid + "</span></td><td><span title='Line' class='tNumber'>" + Paid_Date + "</span></td><td><span title='Line' class='tNumber'>" + Paid_unit_Count + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOPPAServiceDetailLineItem(\"" + OPPAServiceDetailId + "\",\"" + ClaimId + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOPPAServiceDetailLItem(\"" + OPPAServiceDetailId + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr > ";

                                    } else {
                                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Service_Line + "</span></td><td><span  title='Line' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Line' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Line' class='tNumber'>" + Amount_Paid + "</span></td><td><span title='Line' class='tNumber'>" + Paid_Date + "</span></td><td><span title='Line' class='tNumber'>" + Paid_unit_Count + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOPPAServiceDetailLineItem(\"" + OPPAServiceDetailId + "\",\"" + ClaimId + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOPPAServiceDetailLItem(\"" + OPPAServiceDetailId + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr > ";
                                    }
                                };
                                table = table + "</tbody></table>";
                                localStorage.setItem("OPPAServiceDetailTable", "" + table + "");
                                document.getElementById('<%= OPPAServiceDetailOutput.ClientID %>').innerHTML = table;

                                document.getElementById('<%= btnOPPAdd.ClientID%>').style.visibility = "visible";
                                document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
                                document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "hidden";
                                ClearFields();
                                return false;
                            }
                            if (OPPAServiceDetailsList.length > 0) {
                                GetHealthplanId();
                            }
                        }
                    },

                    error: function (jqXHR, textStatus, errorThrown) {
                        $("[id*=lblErrorMsgOtherPayerPaid]").text('Other payer paid amount code not found.');
                    }
                });
            }

            document.getElementById('<%= btnOPPAdd.ClientID%>').style.visibility = "visible";
            document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
            document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "hidden";

        }
        GetServiceLineList();
        GetHealthPlanId();
        ClearFields();
        document.getElementById('<%= OPPAServiceDetailOutput.ClientID %>').disabled = false;
        return false;
    }

    function GetHealthPlanId() {
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId").html("");
        var newOption = "<option value=''></option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId").append(newOption);
        var ClaimID = $("#<%= hdnOPPAmountClaimID.ClientID %>").first().val();
        $("[id*=lblErrorMsgOtherPayerPaid]").text('');
        var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "GetHealthPlanIDForOPPAmountServiceDetail?ClaimId=" + ClaimID+"",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    var HealthPlanList = result;
                    if (HealthPlanList.length > 0) {
                        for (var i = 0; i < HealthPlanList.length; i++) {

                            var Health_Plan_ID = result[i].Health_Plan_ID;
                            var Claims_Other_Payer_Information_ID = result[i].Claims_Other_Payer_Information_ID;
                            var newOption = "<option value='" + Claims_Other_Payer_Information_ID + "'>" + Health_Plan_ID + "</option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId").append(newOption);

                        };
                      
                        return false;
                    }
                 

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorMsgOtherPayerPaid]").text('Other payer paid amount code not found.');
                }
            });
        


        return false;
    }

    function GetServiceLineList() {
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlDetailId").html("");
        var newOption = "<option value=''></option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlDetailId").append(newOption);
        var ClaimID = $("#<%= hdnOPPAmountClaimID.ClientID %>").first().val();
          $("[id*=lblErrorMsgOtherPayerPaid]").text('');
        var APIToken = $("[id*=hdnAccessToken]").val();
          $.ajax({
              type: "GET",
              url: webApiClaims + "GetServiceLineNoFromServiceDetailPanel?ClaimID=" + ClaimID,
              headers: {
                  "Access-Control-Allow-Origin": "*",
                  "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                  "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                  "Authorization": "Bearer " + APIToken
              },
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (result) {

                  var HealthPlanList = result;
                  if (HealthPlanList.length > 0) {
                      for (var i = 0; i < HealthPlanList.length; i++) {
                          var Service_Line = parseInt(result[i].Service_Line);
                         
                          var Claims_Service_Details_ID = result[i].Claims_Service_Details_ID;
                          var newOption = "<option value='" + Claims_Service_Details_ID + "'>" + Service_Line + "</option>";
                          $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlDetailId").append(newOption);

                      };

                      
                  }

                  return false;
              },
              error: function (jqXHR, textStatus, errorThrown) {
                  $("[id*=lblErrorMsgOtherPayerPaid]").text('Other payer paid amount code not found.');
              }
          });

          return false;
      }

    function loaderDropdown1() {

           <%-- document.getElementById('<%= ddlDetailId.ClientID %>').disabled = true;
            document.getElementById('<%= ddlOPPHealhPlanId.ClientID %>').disabled = true;
            document.getElementById('<%= txtOPPAmount.ClientID %>').disabled = true;
            document.getElementById('<%= txtOtherPaidServiceCount.ClientID %>').disabled = true;--%>
        
    }
    function loaderServiceLine() {
        $("[id*=lblErrorMsgOtherPayerPaid]").text(' ');
        $("[id*=lbltOPPProcedureCode]").text(' ');
        var ClaimID = $("#<%= hdnOPPAmountClaimID.ClientID %>").first().val();
        var Service_Line = $("#<%=ddlDetailId.ClientID %> option:selected").text();
        var ClaimType = $("#<%= hdnOPPAmountClaimType.ClientID %>").first().val();
        if ((Service_Line != " ") && (Service_Line != null) && (Service_Line != undefined)) {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "ddlDetailId_SelectedIndexChanged?Claim_ID=" + ClaimID + "&&Service_Line=" + Service_Line + "&&ClaimType=" + ClaimType+"",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result != "") {
                        $("[id*=hdnOPPBillUnit]").val(result[0]["bil_unt"]);
                        if (result[0]["cde_proc"] != null) {
                            $("[id*=lbltOPPProcedureCode]").text(result[0]["cde_proc"]);
                        }
                        else {
                            $("[id*=lbltOPPProcedureCode]").text("");
                        }
                        if (ClaimType == 1) {
                            $("[id*=lbltOPPRevenueCode]").text(result[0]["Revenue_Code"]);
                        }
                    }
                    else {
                        $("[id*=lbltOPPProcedureCode]").text("");
                    }
                },

                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorMsgOtherPayerPaid]").text('');
                }
            });
            RemoveAlreadyAddedHealthPlanID();
        }
        else {
            $("[id*=lblErrorMsgOtherPayerPaid]").text(' ');
            $("[id*=lbltOPPProcedureCode]").text(' ');

        }
        return false;
    }

    function RemoveAlreadyAddedHealthPlanID() {
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId").html("");
        var newOption = "<option value=''></option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId").append(newOption);
       
        var ClaimID = $("#<%= hdnOPPAmountClaimID.ClientID %>").first().val();
        var Service_Line = $("#<%=ddlDetailId.ClientID %> option:selected").text();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "DELETE",
            url: webApiClaims + "RemoveAlreadyAddedHealthPlanID?ClaimID=" + ClaimID + "&&serviceLine=" + Service_Line,
            //data: '{ClaimID: "' + ClaimID + '" ,serviceLine: "' + Service_Line + '" }',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var HealthPlanList = result;
                if (HealthPlanList.length > 0) {
                    for (var i = 0; i < HealthPlanList.length; i++) {

                        var Health_Plan_ID = result[i].Health_Plan_ID;
                        var Claims_Other_Payer_Information_ID = result[i].Claims_Other_Payer_Information_ID;
                        var newOption = "<option value='" + Claims_Other_Payer_Information_ID + "'>" + Health_Plan_ID + "</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_ddlOPPHealhPlanId").append(newOption);

                    };
                    return false;
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMsgOtherPayerPaid]").text('Other payer paid amount code not found.');
            }
        });
    }

    function loaderHealthPlanId() {
        $("[id*=lblErrorMsgOtherPayerPaid]").text(' ');
         var ClaimID = $("#<%= hdnOPPAmountClaimID.ClientID %>").first().val();
        var Health_Plan_ID = $("#<%=ddlOPPHealhPlanId.ClientID %> option:selected").text();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "ddlOPPHealhPlanId_SelectedIndexChanged?ClaimID=" + ClaimID + "&&HealthplanId=" + Health_Plan_ID+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result != "true") {
                    document.getElementById('<%= lblOPPPaidDate.ClientID%>').innerHTML = result;
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorMsgOtherPayerPaid]").text('Other payer paid amount code not found.');
            }
        });
     }
    function loaderDropdown2() {

<%--        document.getElementById('<%= ddlDetailId.ClientID %>').disabled = true;
          document.getElementById('<%= ddlOPPHealhPlanId.ClientID %>').disabled = true;
          document.getElementById('<%= txtOPPAmount.ClientID %>').disabled = true;
            document.getElementById('<%= txtOtherPaidServiceCount.ClientID %>').disabled = true;

          document.getElementById('<%= btnloading2.ClientID %>').style.display = 'block';--%>

    }

    function CancelOPPAServiceInfo() {
        $("[id*=lblErrorMsgOtherPayerPaid]").text('');
        ClearFields();
        GetServiceLineList();
        GetHealthPlanId();
        document.getElementById('<%= btnOPPAdd.ClientID%>').style.visibility = "visible";
        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "hidden";
        var table = localStorage.getItem("OPPAServiceDetailTable");
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= OPPAServiceDetailOutput.ClientID %>').innerHTML = table;
        }

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
    function ServiceDisableEnableConditionAddButton() {
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_btnOPPAdd").prop("disabled", true)
        }, 50);
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucOtherPayerPaidAmountServiceDetail_btnOPPAdd").prop('disabled', false);
        }, 1000);
    }
</script>
<div class="row">
                <div id="OPPAServiceDetailOutput" runat="server"></div>
    <div class="divGrid" style="padding-right: 70px;">
<%--        <asp:GridView ID="gvOtherPaidAdjudicationInfo" runat="server" AllowSorting="false"
            CssClass="gridview"
            AutoGenerateColumns="false"
            DataKeyNames="Other_Payer_Adjudication_Information_Service_Detail_ID"
            OnRowEditing="gvOtherPaidAdjudicationInfo_RowEditing"
            OnRowCancelingEdit="gvOtherPaidAdjudicationInfo_RowCancelingEdit"
            OnRowUpdating="gvOtherPaidAdjudicationInfo_RowUpdating"
            OnRowDeleting="gvOtherPaidAdjudicationInfo_RowDeleting"            
            OnRowDataBound="gvOtherPaidAdjudicationInfo_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Other_Payer_Adjudication_Information_Service_Detail_ID"
                    HeaderText="ID" HeaderStyle-CssClass="hideGridColumn" ItemStyle-CssClass="hideGridColumn" />
                <asp:TemplateField HeaderText="Service Line" HeaderStyle-CssClass="GridviewHeaderAsterisk">
                    <ItemTemplate>
                        <asp:Label ID="lblServiceLine" runat="server" Text='<%# Eval("Service_Line") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlDetailId" runat="server" Style="height: 25px; width: 200px;
                            min-width: 200px;">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdnServiceline" Value='<%# Bind("Service_Line") %>' runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Revenue_Code" HeaderText="Revenue Code" ReadOnly="true"
                    Visible="false" />
                <asp:BoundField DataField="Procedure_Code" HeaderText="Procedure Code " ReadOnly="true" />
                <asp:TemplateField HeaderText="Health Plan ID" HeaderStyle-CssClass="GridviewHeaderAsterisk">
                    <ItemTemplate>
                        <asp:Label ID="lblOtherPaidHealthPlanID" runat="server" Text='<%# Eval("Health_Plan_ID")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlOPPHealhPlanId" runat="server" Style="height: 25px; width: 200px;
                            min-width: 200px;">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdnHealthPlanID" Value='<%# Bind("Health_Plan_ID") %>' runat="server" />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Amount_Paid" HeaderText="Amount Paid" HeaderStyle-CssClass="GridviewHeaderAsterisk" />
                <asp:BoundField DataField="Paid_Date" HeaderText="Paid Date" ReadOnly="true" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:TemplateField HeaderText="Paid Service Unit Count" HeaderStyle-CssClass="GridviewHeaderAsterisk">
                    <ItemTemplate>
                        <asp:Label ID="lblPaidServiceUnitCount" runat="server" Text='<%# Eval("Paid_unit_Count")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Textbox ID="txtOtherPaidServiceCount" Text='<%# Eval("Paid_unit_Count")%>' runat="server" Style="height: 25px; width: 200px;
                            min-width: 200px;" Maxlength="15" onkeypress="return onlyDotsAndNumbers(this,event);">
                        </asp:Textbox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Claim_ID" HeaderText="ClaimID" HeaderStyle-CssClass="hideGridColumn"
                    ItemStyle-CssClass="hideGridColumn" />
                <asp:CommandField ButtonType="Link" ShowEditButton="true" CausesValidation="false" ControlStyle-CssClass="btn btn-primary"
                    ControlStyle-ForeColor="White" ControlStyle-BorderStyle="Solid" />
                <asp:CommandField ButtonType="Link" ShowDeleteButton="true" CausesValidation="false" ControlStyle-CssClass="btn btn-danger"
                    ControlStyle-ForeColor="White" ControlStyle-BorderStyle="Solid" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>--%>
    </div>
</div>
<br />
<div runat="server" id="divotherp">
    <div class="row" style="padding-left: 6rem;">
        <div class="col-md-1">
            <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color:red">*</span>Service
                Line</span>
        </div>
        <div runat="server" id="HeaderRevenue">
            <div class="col-md-1">
                <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">Revenue Code</span>
            </div>
        </div>
        <div class="col-md-1">
            <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">Procedure
                Code</span>
        </div>
        <div class="col-md-2">
            <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color:red">*</span>Health Plan
                ID</span>
        </div>
        <div class="col-md-2">
            <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color:red">*</span>Amount Paid</span>
        </div>
        <div class="col-md-2">
            <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">Paid Date</span>
        </div>
        <div class="col-md-2">
            <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color:red">*</span>Paid Service
                Unit Count</span>
        </div>
        <div class="col-md-1">
        </div>
    </div>
    <div class="row" style="padding-left:6rem;">
        <div class="col-sm-1">
            <div style="text-align: left;">
                 <asp:DropDownList ID="ddlDetailId" CssClass="formField" EnableViewState="true"
                    runat="server" OnChange="return loaderServiceLine()"  AppendDataBoundItems="True"
                    Style="min-width: 30px; height: 30px" Width="100px" class="selectdropdown">
                </asp:DropDownList>
                <br />
                 <button id="btnloading1" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                  
               <%-- <asp:RequiredFieldValidator runat="server" ID="rfvDetailId" SetFocusOnError="true"
                    ValidationGroup="validateOPPAmount" ControlToValidate="ddlDetailId"
                    Text="*Service Line Number is required" Display="Dynamic" InitialValue="" CssClass="failureNotification"/>--%>
            </div>
        </div>
        <div runat="server" id="divRevenueCode">
            <div class="col-sm-1">
                <div style="text-align: left;">
                    <asp:Label runat="server" ID="lbltOPPRevenueCode" Style="width: 110px;"></asp:Label>
                </div>
            </div>
        </div>
        <div class="col-sm-1">
            <div style="text-align: left;">
                <asp:Label runat="server" ID="lbltOPPProcedureCode" Style="width: 110px;"></asp:Label>
            </div>
        </div>
        <div class="col-sm-2">

            <div style="text-align: left;">
                <asp:DropDownList ID="ddlOPPHealhPlanId" CssClass="formField" EnableViewState="true"
                    runat="server"  AppendDataBoundItems="True" Style="min-width: 30px;
                    height: 30px" OnChange="return loaderHealthPlanId()"
                    Width="170px" class="selectdropdown">
                </asp:DropDownList>
                <br />
                 <button id="btnloading2" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                   
                <asp:RequiredFieldValidator runat="server" ID="rfvOPPHealhPlanId" SetFocusOnError="true"
                    ValidationGroup="validateOPPAmount" ControlToValidate="ddlOPPHealhPlanId"
                    Text="*Health Plan ID is required" Display="Dynamic" InitialValue="" CssClass="failureNotification" />
            </div>
        </div>
        <div class="col-sm-2">
            <div style="text-align: left;">
                <asp:TextBox ID="txtOPPAmount" runat="server" CssClass="formFieldTextBox" Style="height: 30px;
                    width: 150px" MaxLength="18" onkeypress="return onlyDotsAndNumbersWithNegetive(this,event);"/>
                <br />
                <asp:RequiredFieldValidator runat="server" ID="rfvOPPAmount" SetFocusOnError="true"
                    ValidationGroup="validateOPPAmount" ControlToValidate="txtOPPAmount"
                    Text="*Amount Paid is required" Display="Dynamic" InitialValue="" CssClass="failureNotification" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtOPPAmount" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
            </div>
        </div>
        <div class="col-sm-2">
            <div style="text-align: left;">
                <asp:Label ID="lblOPPPaidDate" runat="server" Style="height: 30px; width: 130px"></asp:Label>
            </div>
        </div>
        <div class="col-sm-2">
            <div style="text-align: left;">
                <asp:TextBox ID="txtOtherPaidServiceCount" runat="server" CssClass="formFieldTextBox" Style="height: 30px;
                    width: 170px" Maxlength="15" onkeypress="return onlyDotsAndNumbers(this,event);"/>
                <br />
                <asp:RequiredFieldValidator runat="server" ID="rfvOtherPaidServiceCount" SetFocusOnError="true"
                    ValidationGroup="validateOPPAmount" ControlToValidate="txtOtherPaidServiceCount"
                    Text="*Other payer paid service unit count is required" Display="Dynamic" InitialValue="" CssClass="failureNotification" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtOtherPaidServiceCount" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
            </div>
        </div>
        <div class="col-sm-1">
              <span class="ohio-field" style="font-size: 15px; text-align: left">&nbsp;</span>
                     <div class="row">
                            <div class="col-sm-12">
                             <asp:Button ID="btnOPPAdd" Text="ADD" OnClientClick="return AddOPPAserviceDetails()" runat="server"  CssClass="btn btn-primary" Font-Bold="True" Width="90px" />
                            </div>
                     </div>
                     <div class="row">
                            <div class="col-sm-12">
                                <asp:Button ID="btnEdit" Style="visibility: hidden" Text="Update" OnClientClick="return UpdateOPPServiceDetails()" runat="server" CssClass="btn btn-primary" Font-Bold="True"  Width="70px" />
                            </div>
                     </div>
                     <div class="row">
                        <div class="col-sm-12">
                         <asp:Button ID="btnCancel" Style="visibility: hidden" Text="Cancel" OnClientClick="return CancelOPPAServiceInfo()" runat="server" CssClass="btn btn-danger" Font-Bold="True"  Width="70px" />
                        </div>
                     </div>
        </div>
    </div>
    <div class="form-group" style="float: left; width: 100%; padding-top:35px">
                <asp:Label ID="lblErrorMsgOtherPayerPaid" runat="server" ForeColor="Red"></asp:Label>
            </div>
</div>

<asp:HiddenField ID="hdnOPPAmountClaimID" runat="server" />
<asp:HiddenField ID="hdnOPPAmountClaimType" runat="server" />
<asp:HiddenField ID="hdnOPPBillUnit" runat="server" />
<asp:HiddenField ID="hdnOtherPayerPaidClaimStatus" runat="server" />
<asp:HiddenField ID="Other_Payer_Adjudication_Information_Service_Detail_ID" runat="server" />
