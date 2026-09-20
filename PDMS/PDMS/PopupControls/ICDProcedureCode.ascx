<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ICDProcedureCode" Codebehind="ICDProcedureCode.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/SubmitClaimICD10ProcedureCodes.ascx" TagPrefix="uc" TagName="SubmitClaimICD10ProcedureCodes" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">


<script type="text/javascript">
    function ClearICDProcedureCode() {
        clearICDPanelFields();
        $("#<%= lblICDProcCodeError.ClientID %>").html("");
        document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "visible";
        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "hidden";
        var table = localStorage.getItem("ICDProcCodeTable");
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= ICDProcedureOutput.ClientID %>').innerHTML = table;
            return false;
        }
    }
    function clearICDProcCodeTableContent() {
        document.getElementById('<%= ICDProcedureOutput.ClientID %>').innerHTML = "";
    }
    function displaytableIcdProc() {
        var ICDTable = localStorage.getItem("ICDProcCodeTable");


        var ICDProcedureCodeClaimStatus;
        if (document.getElementById("<%=hdnICDProcedureCodeClaimStatus.ClientID %>") != null)
        {
            ICDProcedureCodeClaimStatus = document.getElementById("<%=hdnICDProcedureCodeClaimStatus.ClientID %>").value;
        }
        if (ICDProcedureCodeClaimStatus == "Pending Submission") {
            ICDTable = localStorage.getItem("ICDProcCodeTable");
        }
        else {
            ICDTable = localStorage.removeItem("ICDProcCodeTable");
        }
        
        if (ICDTable != null && ICDTable != undefined && ICDTable != "") {
            if (document.getElementById('<%= ICDProcedureOutput.ClientID %>') != null) {
                document.getElementById('<%= ICDProcedureOutput.ClientID %>').innerHTML = ICDTable;
            }
        }
    }

    function AddICDProcCode() {
        $("[id*=lblICDProcCodeError]").text('');
        var ddlSequenceText = $("#<%=ddlSequenceICD.ClientID %> option:selected").val();
        var txtICD10Procedurecode = $("#<%= txtICD10Procedurecode.ClientID %>").first().val();
        var hdnClaimId = $("#<%= hdnClaimIDInstitutional.ClientID %>").first().val();
        var txtICDPrCdDate = $("#<%= txtICDPrCdDate.ClientID %>").first().val();
        var ddlICDVersion = $("#<%=ddlICDPrCodeInstiutional.ClientID %> option:selected").val();
        var lblICDDesc = $("#<%= txtICD10ProcedureCodeDec1.ClientID %>").html();

        var validate = "";

        if (hdnClaimId != null && hdnClaimId != "undefined" && hdnClaimId != "") {
            if (ddlSequenceText == null || ddlSequenceText == "" || ddlSequenceText == undefined) {
                $("[id*=lblICDProcCodeError]").text('*Select sequence');
                validate = "false";
                return false;
            }
            else {
                validate = "true";
                $("#<%= lblICDProcCodeError.ClientID %>").html("");
            }
            if (txtICD10Procedurecode == null || txtICD10Procedurecode == "" || txtICD10Procedurecode == undefined) {
                $("[id*=lblICDProcCodeError]").text('*ICD 10 procedure code is required');
                validate = "false";
                return false;
            }
            else {
                validate = "true";
                $("#<%= lblICDProcCodeError.ClientID %>").html("");
            }

            if (txtICDPrCdDate == null || txtICDPrCdDate == "" || txtICDPrCdDate == undefined) {
                $("[id*=lblICDProcCodeError]").text('*Date is required');
                validate = "false";
                return false;
            }
            else {
                validate = "true";
                $("#<%= lblICDProcCodeError.ClientID %>").html("");
            }
            if (validate == "true") {
                var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "POST",
                    url: webApiClaims + "addicd10procedurecode?hdnClaimId=" + hdnClaimId + "&&ddlSequenceText=" + ddlSequenceText + "&&txtICD10Procedurecode=" + txtICD10Procedurecode + "&&ddlICDVersion=" + ddlICDVersion + "&&txtICDPrCdDate=" + txtICDPrCdDate + "&&lblICDDesc=" + lblICDDesc + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                    headers: {
                        "Access-Control-Allow-Origin": "*",
                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                        "Authorization": "Bearer " + APIToken
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var ICD10procedurecodeList = result;

<%--                        if (result.d[0].SequeneCount_Principal == "already added") {
                            $("[id*=lblICDProcCodeError]").text('Already principal sequence is added');
                            document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "Visible";

                        }
                        else {--%>
<%--                            if (result.d[0].SequeneCount_Other >= 24) {
                                document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "hidden";
                            }
                            else {
                                document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "Visible";
                            }--%>

                            var table = "";
                            var count = 0;
                            if (ICD10procedurecodeList.length > 0) {
                                table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>ICD Procedure Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:10px; scope='col'>Date</th><th style='width:30px; scope='col'>ICD Procedure Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                                var totalCount = 0;
                                for (var i = 0; i < ICD10procedurecodeList.length; i++) {
                                    staus = "Success";
                                    totalCount++;
                                    var Sequence = result[i].Sequence;
                                    var IcdProcedureCode = result[i].IcdProcedureCode;
                                    var ICDVersion = result[i].ICDVersion;
                                    var IcdProcedureCodeDescription = result[i].IcdProcedureCodeDescription;
                                    var Claims_ICD_Procedure_Code_Sequence_ID = result[i].Claims_ICD_Procedure_Code_Sequence_ID;
                                    var Claim_ID = result[i].Claim_ID;
                                    var ICD_Date = result[i].Date;
                                    if (Sequence == "Principal") { count++; }
                                    if (totalCount >= 25) { document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "hidden"; }
                                    else { document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "visible"; }
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Sequence + "</span></td><td><span title='Line' class='tNumber'>" + IcdProcedureCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + ICD_Date + "</span></td><td>" + IcdProcedureCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditICDProcdureCodeLineItem(\"" + Claims_ICD_Procedure_Code_Sequence_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteICDProcedureCodeItem(\"" + Claims_ICD_Procedure_Code_Sequence_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                                    $("#<%=ddlSequenceICD.ClientID %>").empty();
                                    var newOption = "<option value=''></option>";
                                    $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").append(newOption);

                                    if (count < 1) {
                                        newOption = "<option value='3'>Principal</option>";
                                        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").append(newOption);

                                    }

                                    newOption = "<option value='2'>Other</option>";
                                    $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").append(newOption);

                                };
                                table = table + "</tbody></table>";
                                localStorage.setItem("ICDProcCodeTable", "" + table + "");
                                document.getElementById('<%= ICDProcedureOutput.ClientID %>').innerHTML = table;
                                document.getElementById('<%= ICDProcedureOutput.ClientID %>').style.display = 'block';
                                
                                clearICDPanelFields();
                                return false;
                            }

                            else {
                                document.getElementById('<%= ICDProcedureOutput.ClientID %>').innerHTML = "";
                                return false;
                            }
                        //}
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        $("[id*=lblICDProcCodeError]").text('ICD Procedure code not found.');
                    }
                });
            }
        }
        clearICDPanelFields();       
        return false;

    }
    function clearICDPanelFields() {
        $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_txtICD10Procedurecode').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_txtICDPrCdDate').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_txtICD10ProcedureCodeDec').val('');
        document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlICDPrCodeInstiutional").options[0].selected = true;
        document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").options[0].selected = true;
        $("#<%= txtICD10ProcedureCodeDec1.ClientID %>").html("");
    }
    function EditICDProcdureCodeLineItem(Claims_ICD_Procedure_Code_Sequence_ID, Claim_ID, control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        $("[id*=lblICDProcCodeError]").text('');
        $("#<%=ddlSequenceICD.ClientID %>").empty();
        var newOption = "<option value=''></option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").append(newOption);
        newOption = "<option value='2'>Other</option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").append(newOption);
        newOption = "<option value='3'>Principal</option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").append(newOption);
        $("[id*=lblICDProcCodeError]").text('');
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetICD_Procedure_CodeDetails?Claim_ID=" + Claim_ID + "&&Claims_ICD_Procedure_Code_Sequence_ID=" + Claims_ICD_Procedure_Code_Sequence_ID+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var ICD10procedurecodeList = result;
                if (ICD10procedurecodeList.length > 0) {

                    for (var i = 0; i < ICD10procedurecodeList.length; i++) {

                        var Sequence = result[i].Sequence;
                        var IcdProcedureCode = result[i].IcdProcedureCode;
                        var ICDVersion = result[i].ICDVersion;
                        var IcdProcedureCodeDescription = result[i].IcdProcedureCodeDescription;
                        var Claims_ICD_Procedure_Code_Sequence_ID = result[i].Claims_ICD_Procedure_Code_Sequence_ID;
                        var Claim_ID = result[i].Claim_ID;
                        var ICD_Date = result[i].Date;
                        if (Sequence == "Other") { document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").options[1].selected = true; }
                        if (Sequence == "Principal") { document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").options[2].selected = true; }

                        if (ICDVersion == "ICD 10") { document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlICDPrCodeInstiutional").options[0].selected = true; }
                        if (ICDVersion == "ICD 9") { document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlICDPrCodeInstiutional").options[1].selected = true; }
                        $("[id*=txtICD10Procedurecode]").val("" + IcdProcedureCode + "");
                        $("[id*=hdnClaimsICDProcedureCodeId]").val("" + Claims_ICD_Procedure_Code_Sequence_ID + "");
                        $("[id*=txtICDPrCdDate]").val("" + ICD_Date + "");
                        document.getElementById('<%= txtICD10ProcedureCodeDec1.ClientID%>').innerHTML = IcdProcedureCodeDescription;
                        document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "visible";
                        return false;
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblICDProcCodeError]").text('ICD Procedure code not found.');
            }
        });

    }
    function DeleteICDProcedureCodeItem(Claims_ICD_Procedure_Code_ID) {
        var result = confirm("Are you sure you want to delete?");

        if (result) {
            var hdnClaimId = $("#<%= hdnClaimIDInstitutional.ClientID %>").first().val();
            $("[id*=lblICDProcCodeError]").text('');
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteICDProcedeCodeSequence?hdnClaimId=" + hdnClaimId + "&&Claims_ICD_Procedure_Code_ID=" + Claims_ICD_Procedure_Code_ID+"",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var count = 0;
                    var ICD10procedurecodeList = result;
                    var table = "";
                    var totalCount = 0;
                    if (ICD10procedurecodeList.length > 0) {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Icd Procedure Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:10px; scope='col'>Date</th><th style='width:30px; scope='col'>ICD Procedure Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        for (var i = 0; i < ICD10procedurecodeList.length; i++) {
                            totalCount++;
                            var Sequence = result[i].Sequence;
                            var IcdProcedureCode = result[i].IcdProcedureCode;
                            var ICDVersion = result[i].ICDVersion;
                            var IcdProcedureCodeDescription = result[i].IcdProcedureCodeDescription;
                            var Claims_ICD_Procedure_Code_Sequence_ID = result[i].Claims_ICD_Procedure_Code_Sequence_ID;
                            var Claim_ID = result[i].Claim_ID;
                            var ICD_Date = result[i].Date;
                            if (Sequence == "Principal") {
                                count++;
                            }
                            if (totalCount >= 25) { document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "hidden"; }
                            else { document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "visible"; }
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Sequence + "</span></td><td><span title='Line' class='tNumber'>" + IcdProcedureCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + ICD_Date + "</span></td><td>" + IcdProcedureCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditICDProcdureCodeLineItem(\"" + Claims_ICD_Procedure_Code_Sequence_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteICDProcedureCodeItem(\"" + Claims_ICD_Procedure_Code_Sequence_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        };
                        table = table + "</tbody></table>";
                        localStorage.setItem("ICDProcCodeTable", "" + table + "");
                        document.getElementById('<%= ICDProcedureOutput.ClientID %>').innerHTML = table;
                        clearICDPanelFields();
                        if (result[0].SequeneCount_Other >= 24) {
                            document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "hidden";
                        }
                        else {
                            document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "visible";
                        }
                        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";

                        $("#<%=ddlSequenceICD.ClientID %>").empty();
                        var newOption = "<option value=''></option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").append(newOption);
                        if (count < 1) {
                            newOption = "<option value='3'>Principal</option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").append(newOption);

                        }

                        newOption = "<option value='2'>Other</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").append(newOption);


                    }
                    else {
                        $("#<%=ddlSequenceICD.ClientID %>").empty();
                        var newOption = "<option value=''></option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").append(newOption);
                        newOption = "<option value='2'>Other</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").append(newOption);
                        newOption = "<option value='3'>Principal</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").append(newOption);
                        document.getElementById('<%= ICDProcedureOutput.ClientID %>').innerHTML = "";
                        document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblICDProcCodeError]").text('ICD Procedure code not found.');
                }
            });
        }
        clearICDPanelFields();
        return false;
    }
    function EditICDProcedureCode() {
        var hdnClaimId = $("#<%= hdnClaimIDInstitutional.ClientID %>").first().val();
        var Claims_ICDProc_Code_Sequence = $("#<%= hdnClaimsICDProcedureCodeId.ClientID %>").first().val();
        var Sequence = $("#<%=ddlSequenceICD.ClientID %> option:selected").val();
        var Sequence = $("#<%=ddlSequenceICD.ClientID%>").val();
        var ICD10Procedurecode = $("#<%= txtICD10Procedurecode.ClientID %>").first().val();
        var ddlICDVersion = $("#<%=ddlICDPrCodeInstiutional.ClientID %> option:selected").val();
        var txtICDPrCdDate = $("#<%= txtICDPrCdDate.ClientID %>").first().val();
        var ICDDescription = $("#<%= txtICD10ProcedureCodeDec1.ClientID %>").html();

        if (Sequence == null || Sequence == "" || Sequence == undefined) {
            $("[id*=lblICDProcCodeError]").text('*Select sequence');
            validate = "false";
            return false;
        }
        else {
            validate = "true";
            $("#<%= lblICDProcCodeError.ClientID %>").html("");
        }
        if (ICD10Procedurecode == null || ICD10Procedurecode == "" || ICD10Procedurecode == undefined) {
            $("[id*=lblICDProcCodeError]").text('*ICD 10 procedure code is required');
            validate = "false";
            return false;
        }
        else {
            validate = "true";
            $("#<%= lblICDProcCodeError.ClientID %>").html("");
        }

        if (txtICDPrCdDate == null || txtICDPrCdDate == "" || txtICDPrCdDate == undefined) {
            $("[id*=lblICDProcCodeError]").text('*Date is required');
            validate = "false";
            return false;
        }
        else {
            validate = "true";
            $("#<%= lblICDProcCodeError.ClientID %>").html("");
        }
        if (validate == "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "PUT",
                url: webApiClaims + "EditICD10ProcedureCode?hdnClaimId=" + hdnClaimId + "&&Claims_ICDProc_Code_Sequence=" + Claims_ICDProc_Code_Sequence + "&&Sequence=" + Sequence + "&&ICD10Procedurecode=" + ICD10Procedurecode + "&&ddlICDVersion=" + ddlICDVersion + "&&txtICDPrCdDate=" + txtICDPrCdDate + "&&ICDDescription=" + ICDDescription + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var ICD10procedurecodeList = result;
                    if (result[0].SequeneCount_Principal == "already added") {
                        $("[id*=lblICDProcCodeError]").text('Already principal sequence is added');
                    } else {
                        if (ICD10procedurecodeList.length > 0) {
                            var count = 0;
                            var totalCount = 0;
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Icd Procedure Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:10px; scope='col'>Date</th><th style='width:30px; scope='col'>ICD Procedure Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                            for (var i = 0; i < ICD10procedurecodeList.length; i++) {
                                totalCount++;
                                var Sequence = result[i].Sequence;
                                var IcdProcedureCode = result[i].IcdProcedureCode;
                                var ICDVersion = result[i].ICDVersion;
                                var IcdProcedureCodeDescription = result[i].IcdProcedureCodeDescription;
                                var Claims_ICD_Procedure_Code_Sequence_ID = result[i].Claims_ICD_Procedure_Code_Sequence_ID;
                                var Claim_ID = result[i].Claim_ID;
                                var ICD_Date = result[i].Date;
                                if (Sequence == "Principal") {
                                    count++;
                                }
                                if (totalCount >= 25) { document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "hidden"; }
                                else {
                                    document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "visible";
                                    document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "hidden";}
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Sequence + "</span></td><td><span title='Line' class='tNumber'>" + IcdProcedureCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + ICD_Date + "</span></td><td>" + IcdProcedureCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditICDProcdureCodeLineItem(\"" + Claims_ICD_Procedure_Code_Sequence_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteICDProcedureCodeItem(\"" + Claims_ICD_Procedure_Code_Sequence_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            };
                            table = table + "</tbody></table>";
                            localStorage.setItem("ICDProcCodeTable", "" + table + "");
                            document.getElementById('<%= ICDProcedureOutput.ClientID %>').innerHTML = table;
                            document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
                            clearICDPanelFields();
                            $("#<%=ddlSequenceICD.ClientID %>").empty();
                            var newOption = "<option value=''></option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").append(newOption);

                            if (count < 1) {

                                newOption = "<option value='3'>Principal</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").append(newOption);

                            }

                            newOption = "<option value='2'>Other</option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlSequenceICD").append(newOption);
                        }
                        if (result.d[0].SequeneCount_Other >= 24) {

                            document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "hidden";

                        } else {
                            document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "visible";

                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblICDProcCodeError]").text('ICD Procedure code not found.');
                }
            });

            document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
            document.getElementById('<%= btnAddICDCode.ClientID%>').style.visibility = "visible";
        }

        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
        clearICDPanelFields();
        return false;
    }
    function dateChanged() {
        var txtICDPrCdDate = $("#<%= txtICDPrCdDate.ClientID %>").first().val();
        if (txtICDPrCdDate != "" && txtICDPrCdDate != undefined && txtICDPrCdDate != null) {
            validateDate5();
        }
        else {
            ICDDateRequiredError1.Visible = true;
        }




    }
    $(function () {
        $("[id*=ClaimDentserviceAdd]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["ddlDocumentType"];
            return RequiredFieldsValidations(row, requiredControles);
        });
    });
    function RequiredFieldsValidations(row, requiredControles) {
        var isValid = true;
        $.each(requiredControles, function (index, Id) {
            var label = row.find("[id*=" + Id + "]").next("SPAN");
            label.hide();
            if ($.trim(row.find("[id*=" + Id + "]").val()) === "") {
                label.show();
                isValid = false;
            }
        });

        return isValid;
    }


        function visibleICDCode() {
            $("#<%=ucSubmitClaimICD10ProcedureCodes.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").html("");
            $("#<%=ucSubmitClaimICD10ProcedureCodes.FindControl("txtCode").ClientID %>").val("");
            $("#<%=ucSubmitClaimICD10ProcedureCodes.FindControl("txtPlaceOfServiceName").ClientID %>").val("");
            localStorage.setItem("indexIcdProcedurecode", "");

            $find("mpeICDCode").show();
            return false;
        }


    function ICDProcedureCodetextChange() {

        var txtICDProcedureCode = $("#<%= txtICD10Procedurecode.ClientID %>").first().val();
        var txtdropdown = $('#<%= ddlICDPrCodeInstiutional.ClientID %>').val();
        $("[id*=lblICDProcCodeError]").text('');
        $('#<%= txtICD10ProcedureCodeDec.ClientID %>').val('');
        $('#<%= txtICD10ProcedureCodeDec1.ClientID %>').html('');


        if (txtICDProcedureCode.length != 0) {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "GetICDProcedureCodeDescription?desc=" + txtICDProcedureCode + "&&dropdownvalue=" + txtdropdown,
                //data: '{desc: "' + txtICDProcedureCode + '", dropdownvalue: "' + txtdropdown + '" }',
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
                        $("[id*=lblICDProcCodeError]").text('ICD Procedure code  is Invalid.');
                    }
                    else {
                        $('#<%= txtICD10ProcedureCodeDec.ClientID %>').val(result[0]["LONG_DESC"]);
                        $('#<%= txtICD10ProcedureCodeDec1.ClientID %>').html(result[0]["LONG_DESC"]);
                        $("[id*=lblICDProcCodeError]").text('');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblICDProcCodeError]").text('ICD Procedure Code is Invalid');
                }
            });
        }
    }
    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9- ]/g, '');
    }
    function validateDate5() {
        var daterequested = document.getElementById("<%= txtICDPrCdDate.ClientID%>").value;

        var tdate = new Date();
        var dd = tdate.getDate();
        var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
        var yyyy = tdate.getFullYear();
        var currentDate = (MM) + "/" + dd + "/" + yyyy;

        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ICDDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ICDDateRequiredError1').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_txtICDPrCdDate').val('');
        }
        else {

            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ICDDateRequiredError1').css('display', 'none');

        }

        if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ICDDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ICDDateRequiredError1').text('Future Date not allowed.');
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_txtICDPrCdDate').val('');
        }

    }

    function GetIcdProcCodeDetails() {
        var txtIcdProcCode = $("#<%= ucSubmitClaimICD10ProcedureCodes.FindControl("txtCode").ClientID %>").first().val();
        var txtIcdProccodedesc = $("#<%= ucSubmitClaimICD10ProcedureCodes.FindControl("txtPlaceOfServiceName").ClientID %>").first().val();
        var txtdropdown = $("#<%= ucSubmitClaimICD10ProcedureCodes.FindControl("ddlICDPrCodeInstiutional").ClientID %>").first().val();
        $("#<%=ucSubmitClaimICD10ProcedureCodes.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").html("");
        if (txtIcdProcCode == "" && txtIcdProccodedesc == "") {
            $("#<%=ucSubmitClaimICD10ProcedureCodes.FindControl("lblSResultICD").ClientID %>").append("<tr><td> Either ICD procedure code or description is required</td></tr>");
            return false;
        }
        else {
            $("#<%=ucSubmitClaimICD10ProcedureCodes.FindControl("lblSResultICD").ClientID %>").html("");
        }
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetICDProcedureCodeDetails?desc=" + txtIcdProccodedesc + "&&val=" + txtIcdProcCode + "&&dropdownvalue=" + txtdropdown,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            //data: '{desc: "' + txtIcdProccodedesc + '" , val: "' + txtIcdProcCode + '", dropdownvalue: "' + txtdropdown + '" }',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result.length == 0) {
                    $("#<%=ucSubmitClaimICD10ProcedureCodes.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").append("<tr><td> No Records Found </td></tr>");
                }
                else {
                    $("#<%=ucSubmitClaimICD10ProcedureCodes.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").append("<tr><th>ICD Procedure Code </th><th>ICD Version</th><th>ICD Procedure Code Description</th></tr>");
                    for (var i = 0; i < result.length; i++) {

                        $("#<%=ucSubmitClaimICD10ProcedureCodes.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").append("<tr><td><a onClick='GetIcdProcedurecode(this); return false;'>" + result[i].ICD_Code + "</a></td><td>" + result[i].ICD_Desc + "</td><td>" + result[i].ICD_Version + "</td></tr>");

                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblICDProcCodeError]").text('ICD Procedure code not found.');
            }
        });
        return false;
    }

    function GetIcdProcedurecode(lnk) {
        $find("mpeICDCode").hide();
        var gridindexICDcode = localStorage.getItem("indexIcdProcedurecode");

        if (!(gridindexICDcode == "")) {

            var row = lnk.parentNode.parentNode;
            var inputs = grid.rows[gridindexICDcode].getElementsByTagName("INPUT");
            inputs[1].value = row.cells[0].innerText;
            grid.rows[gridindexICDcode].cells[4].innerText = row.cells[2].innerHTML;
            return false;
        }
        else {
            var textboxrow = lnk.parentNode.parentNode;
            var ICDVersion = textboxrow.cells[1].innerText.trim();
            if (ICDVersion == "ICD 10") {
                document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlICDPrCodeInstiutional").options[0].selected = true;
            }
            if (ICDVersion == "ICD 9") {
                document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimICDProcedureCode_ddlICDPrCodeInstiutional").options[1].selected = true;
            }
            $("[id*=ddlICDPrCodeInstiutional]").val(textboxrow.cells[1].innerText.trim());
            $("[id*=txtICD10Procedurecode]").val(textboxrow.cells[0].innerText.trim());
            $("[id*=txtICD10ProcedureCodeDec1]").html(textboxrow.cells[2].innerHTML);
            $("[id*=txtICD10ProcedureCodeDec]").val(textboxrow.cells[2].innerHTML);

            return false;
        }
    }

    function visibleIcdProcedureCodeGridView(lnk) {
        $("#<%=ucSubmitClaimICD10ProcedureCodes.FindControl("gvSubmitClaimSearchProcPop").ClientID %>").html("");
        $("#<%=ucSubmitClaimICD10ProcedureCodes.FindControl("txtCode").ClientID %>").val("");
        $("#<%=ucSubmitClaimICD10ProcedureCodes.FindControl("txtPlaceOfServiceName").ClientID %>").val("");

        var index = lnk.parentNode.parentNode.rowIndex;
        localStorage.setItem("indexIcdProcedurecode", index);

        $find("mpeICDCode").show();
        return false;

    }

</script>



<asp:UpdatePanel ID="upICD10ProcedureCodes" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hdnClaimsICDProcedureCodeId" runat="server" />

        <div class="row" style="text-align: center; width: 100%;">

            <div id="ICDProcedureOutput" runat="server"></div>            
        </div>
        <div>
            <asp:Label runat="server" ID="lblError" Text="" ForeColor="Red" Visible="false" />
        </div>
        <div class="row" style="text-align: center; padding-left: 20px;" id="divICD" runat="server">
            <div class="col-sm-2">
                <span class="ohio-field" style="font-size: 14px; text-align: left"><span style="color: red">* </span>Sequence</span>
                <div style="text-align: left;">
                    <asp:DropDownList ID="ddlSequenceICD" CssClass="formfieldDate" Style="min-width: 150px; height: 30px" EnableViewState="true" runat="server" AppendDataBoundItems="True" Width="150px"
                        ValidationGroup="vgICDProcedureCodes">
                    </asp:DropDownList>
                </div>
            </div>

            <div class="col-sm-2">
                <span class="ohio-field" style="font-size: 14px; text-align: left"><span style="color: red">* </span>ICD Procedure Code</span>
                <div style="text-align: left;">
                    <asp:TextBox ID="txtICD10Procedurecode" onKeyUp="javascript:alphanumericOnly(this);" onChange="return ICDProcedureCodetextChange()" runat="server" MaxLength="7" CssClass="formfieldDate" Style="height: 30px; width: 150px" ValidationGroup="vgICDProcedureCodes" />
                    <asp:LinkButton ID="lnkICD10Procedurecode" Style="font-size: 14px; text-align: left" Text="Search" runat="server" ToolTip="Search" CommandArgument='<%# Eval("CDE_POS") %>' Visible="true" CausesValidation="False" OnClientClick="return visibleICDCode()"></asp:LinkButton>
                </div>
            </div>
            <div class="col-sm-2">
                <span class="ohio-field" style="font-size: 14px; text-align: left"><span style="color: red">* </span>ICD Version</span>
                <div style="text-align: left;">
                    <asp:DropDownList ID="ddlICDPrCodeInstiutional" Style="min-width: 150px; height: 30px" Width="150px" CssClass="formfieldDate" runat="server" ValidationGroup="vldheaderInstitution">
                        <asp:ListItem Value="ICD 10" Text="ICD 10"></asp:ListItem>
                        <asp:ListItem Value="ICD 9" Text="ICD 9"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-sm-3">
                <span class="ohio-field" style="font-size: 14px; text-align: left"><span style="color: red">* </span>Date</span>
                <div style="text-align: left;">
                    <asp:TextBox ID="txtICDPrCdDate" OnChange="dateChanged()" ValidationGroup="vgICDProcedureCodes" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 150px; min-width: 150px;" />
                    <asp:Image ID="imgProcedureDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                    <ajax:CalendarExtender ID="ceOccurenceDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtICDPrCdDate" PopupPosition="BottomRight" CssClass="QstCalendarCSS" PopupButtonID="imgDate" EnabledOnClient="true" />
                    <asp:Label ID="ICDDateRequiredError1" runat="server" Text="" CssClass="error-message" Style="display: none;"></asp:Label>
                    <button id="btnloading2" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>

                </div>
            </div>
            <div class="col-sm-2 text-centre">
                <span class="ohio-field" style="font-size: 14px; text-align: left">ICD Procedure Code Description</span>
                <%--<div style="text-align: left;">--%>
                <span>
                    <asp:Label ID="txtICD10ProcedureCodeDec1" runat="server" Style="height: 30px; width: 200px; font-size: 14px;"></asp:Label>

                    <asp:HiddenField ID="txtICD10ProcedureCodeDec" runat="server" Value="" />


                </span>
                <%--</div>--%>
            </div>

            <div class="col-sm-1">
                <span class="ohio-field" style="font-size: 15px; text-align: left">&nbsp;</span>
                <div style="text-align: left;">
                    <button id="btnloading" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>

                    <br />
                    <%--<asp:Button ID="btnAddICD10ProcedureCode" Text="ADD" runat="server" OnClick="btnAddICDCode_Click" OnClientClick="loadICDProcCode()"
                        CssClass="btn btn-primary" Font-Bold="True" Width="90px" ValidationGroup="vgICDProcedureCodes" CausesValidation="true" />--%>

                    <asp:Button ID="btnAddICDCode" Text="ADD" OnClientClick="return AddICDProcCode()" runat="server" CssClass="btn btn-primary" Font-Bold="True" Width="90px" />
                    <br />
                    <asp:Button ID="btnEdit" Style="visibility: hidden" Text="Update" OnClientClick="return EditICDProcedureCode()" runat="server" CssClass="btn btn-primary" Font-Bold="True" Width="70px" />
                    <asp:Button ID="btnCancel" Style="visibility: hidden" Text="Cancel" OnClientClick="return ClearICDProcedureCode()" runat="server" CssClass="btn btn-primary" Font-Bold="True" Width="70px" />

                </div>
            </div>
            <asp:HiddenField ID="hdnClaimIDInstitutional" runat="server" />
            <asp:HiddenField ID="hdnICDProcedureCodeClaimStatus" runat="server" />
        </div>

        <div class="form-group" style="float: left; width: 100%; padding-top: 35px">
            <asp:Label ID="lblICDProcCodeError" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<ajax:ModalPopupExtender BehaviorID="mpeICDCode" ID="mpeSubmitClaimSearchPop" runat="server" PopupControlID="pnlSubmitClaimSearchPop"
    TargetControlID="Button9" BackgroundCssClass="modalBackground" CancelControlID="btnCloseCH9" />

<asp:Panel ID="pnlSubmitClaimSearchPop" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 800px; height: auto; width: auto;">
    <asp:Panel ID="pnlSubmitClaimSearchPopHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
        <asp:Button runat="server" ID="btnCloseCH9" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;" CausesValidation="false" />
        <div class="search-Results">
            <span style="padding-left: 10px; font-size: 17px;">ICD Procedure Code</span>
            <span style="padding-left: 230px; font-size: 17px;">ICD Version</span>
            <span style="padding-left: 270px; font-size: 17px;">ICD Procedure Code Decsription</span>

        </div>
    </asp:Panel>
    <asp:Panel ID="pnlSubmitClaimSearch" runat="server">
        <div style="text-align: left; padding: 15px" class="container-fluid">
            <div class="row">
                <uc:SubmitClaimICD10ProcedureCodes runat="server" ID="ucSubmitClaimICD10ProcedureCodes" Visible="true" EnableViewState="true" />

            </div>
        </div>

    </asp:Panel>
</asp:Panel>
<asp:Button runat="server" ID="Button9" Style="display: none" Text="ButtonDummy9" />
