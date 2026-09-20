<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_SubmitClaimOccurrenceInformation" Codebehind="SubmitClaimOccurrenceInformation.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%--<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>--%>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<link href="../Content/custom-style.css" rel="stylesheet" />
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">
<script language="JavaScript">
    var TotalCountintheGrid;
    function CancelOccurrenceInfoDetails() {
        $("#<%= txtOccurrenceCode.ClientID %>").val('');
        $("#<%= txtOccurenceDate.ClientID %>").val('');
        $("#<%= lblOccurrenceDesc.ClientID %>").html('');
        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnCancelOccurrence.ClientID%>').style.visibility = "hidden";

        if (TotalCountintheGrid < 24) {
            document.getElementById('<%= btnAddOccurrence.ClientID%>').style.visibility = "visible";
        }
        else {
            document.getElementById('<%= btnAddOccurrence.ClientID%>').style.visibility = "hidden";
        }
        var table = localStorage.getItem("OccurrenceInfoTable");
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= divOccurrenceInfo.ClientID %>').innerHTML = table;
        }
         return false;
    }
    function AddOccurrenceInfoData() {        
        var OccurrnceInfodata;        
        var hdnClaimId = $("#<%= hdnClaimId.ClientID %>").first().val();
        var Occurrence_Code = $("#<%= txtOccurrenceCode.ClientID %>").first().val();
        var OccurrenceDate = $("#<%= txtOccurenceDate.ClientID %>").first().val();
        var OccurrenceDesc = document.getElementById('<%= lblOccurrenceDesc.ClientID%>').innerHTML;
        var hdnClaimsOccurrenceInformationID = $("#<%= hdnClaimsOccurrenceInformationID.ClientID %>").first().val();
        var ValOccurrenceInfo = "";
        ValidateOccurrenceInfoDetail();        
        if (ValOccurrenceInfo === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "POST",
                url: webApiClaims + "AddOccurrenceInfoData?Claim_ID=" + hdnClaimId + "&&Occurrence_Code=" + Occurrence_Code + "&&OccurrenceDate=" + OccurrenceDate + "&&OccurrenceDescription=" + OccurrenceDesc + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {                    
                    OccurrnceInfodata = result;
                    var table;
                    var Error = null;
                    if (OccurrnceInfodata.length > 0) {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Occurrence Code</th><th style='width:10px; scope='col'>Occurrence Date</th><th style='width:30px; scope='col'>Occurrance Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        var totalCount = 0;
                        for (var i = 0; i < OccurrnceInfodata.length; i++) {
                            if (result[i].Error != null) {
                                Error = result[i].Error;
                            }
                        }
                        if (Error == null || Error == undefined || Error == "") {
                            for (var i = 0; i < OccurrnceInfodata.length; i++) {
                                totalCount++;

                                var Line = result[i].Line;
                                var OccurrenceInfo_Code = result[i].OccurrenceInfoCode;
                                var OccurrenceInfoDate = result[i].OccurrenceDate;
                                var OccurrenceInfoDesc = result[i].OccurrenceDesc;
                                var ClaimsOccurrenceInformationID = result[i].ClaimsOccurrenceInformationID;
                                var Claim_ID = result[i].ClaimId;

                                TotalCountintheGrid = totalCount;
                                if (totalCount >= 24) { document.getElementById('<%= btnAddOccurrence.ClientID%>').style.visibility = "hidden"; }
                                else { document.getElementById('<%= btnAddOccurrence.ClientID%>').style.visibility = "visible"; }
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span title='Occurrence Code' class='tNumber'>" + OccurrenceInfo_Code + "</span></td><td><span  title='OccurrenceDate' class='tNumber'>" + OccurrenceInfoDate + "</span></td><td><span title='OccurenceCode Description' class='tNumber'>" + OccurrenceInfoDesc + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOccurrenceItem(\"" + ClaimsOccurrenceInformationID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOccurrenceItem(\"" + ClaimsOccurrenceInformationID + "\",\"" + Claim_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            };
                            table = table + "</tbody></table>";
                            localStorage.setItem("OccurrenceInfoTable", "" + table + "");
                            document.getElementById('<%= divOccurrenceInfo.ClientID %>').innerHTML = table;
                            OccurrenceInfoClearFields();

                        }
                        else {
                            $("[id*=lblOccurrenceInfomessage]").text('Duplicate occurrence Code is not allowed. ');
                        }
                    }
                    else {
                        OccurrenceInfoClearFields();
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblOccurrenceInfomessage]").text('Occurrence Code not found.');
                }
            });
        }
        function ValidateOccurrenceInfoDetail() {            
            if (Occurrence_Code == null || Occurrence_Code == undefined || Occurrence_Code == "") {
                ValOccurrenceInfo = "false";
                $("[id*=lbltxtOccurrenceCode]").text('Occurrecne Code is required');
                return false;
            }
            else {
                ValOccurrenceInfo = "true";
                $("[id*=lbltxtOccurrenceCode]").text('');
            }
            if (OccurrenceDate == null || OccurrenceDate == undefined || OccurrenceDate == "") {
                ValOccurrenceInfo = "false";
                $("[id*=lbltxtOccurenceDate]").text('Occurrecne Date is required');
                return false;
            }
            else {
                ValOccurrenceInfo = "true";
                $("[id*=lbltxtOccurenceDate]").text('');
            }
            if (OccurrenceDesc == null || OccurrenceDesc == undefined || OccurrenceDesc == "") {
                ValOccurrenceInfo = "false"; 
                $("[id*=ErrlblOccurrenceDesc]").text('Occurrence Description is required');
                return false;
            }
            else {
                ValOccurrenceInfo = "true";
                $("[id*=ErrlblOccurrenceDesc]").text('');
            }
            if (validateDate7() === true) {
                ValOccurrenceInfo = "true";

            }
            else {
                ValOccurrenceInfo = "false";
                return false;
            }
        
        }
        return false;
    }
    function OccurrenceInfoClearFields() {        
        $("#<%= txtOccurrenceCode.ClientID %>").val('');
        $("#<%= txtOccurenceDate.ClientID %>").val('');       
        $("#<%= lblOccurrenceDesc.ClientID %>").html('');
    }
    function loadAddOccurrenceCodeInformation() {
        var txtOccurenceDate = $("#<%= txtOccurrenceCode.ClientID %>").first().val();
        var occurrencedesc = $("#<%= txtOccurenceDate.ClientID %>").first().val();
        if (txtOccurenceDate != "" && occurrencedesc != "") {
        <%--document.getElementById('<%= btnAdd.ClientID %>').style.display = 'none';--%>
       
        document.getElementById('<%= txtOccurrenceCode.ClientID %>').disabled = true;
        document.getElementById('<%= txtOccurenceDate.ClientID %>').disabled = true;
    }
    }
    function EditOccurrenceItem(ClaimsOccurrenceInformationID, Claim_ID,control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        var claimOccurrenceCodeList;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetOccurrenceInfoData?ClaimsOccurrenceInformationID=" + ClaimsOccurrenceInformationID + "&&Claim_ID=" + Claim_ID,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                claimOccurrenceCodeList = result;
                if (claimOccurrenceCodeList.length > 0) {                    
                    for (var i = 0; i < claimOccurrenceCodeList.length; i++) {
                        var Line = result[i].Line;
                        var OccurrenceInfo_Code = result[i].OccurrenceInfoCode;
                        var OccurrenceInfoDate = result[i].OccurrenceDate;
                        var OccurrenceInfoDesc = result[i].OccurrenceDesc;
                        var ClaimsOccurrenceInformationID = result[i].ClaimsOccurrenceInformationID;
                        var Claim_ID = result[i].ClaimId;

                        $("[id*=hdnOccLine]").val("" + Line + "");
                        $("[id*=txtOccurrenceCode]").val("" + OccurrenceInfo_Code + "");
                        $("[id*=txtOccurenceDate]").val("" + OccurrenceInfoDate + "");
                        $("[id*=lblOccurrenceDesc]").val("" + OccurrenceInfoDesc + "");
                        $("[id*=hdnClaimId]").val("" + Claim_ID + "");//
                        $("[id*=hdnClaimsOccurrenceInformationID]").val("" + ClaimsOccurrenceInformationID + "");

                        document.getElementById('<%= lblOccurrenceDesc.ClientID%>').innerHTML = OccurrenceInfoDesc;
                        document.getElementById('<%= btnAddOccurrence.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnCancelOccurrence.ClientID%>').style.visibility = "visible";
                        return false;
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblOccurrenceInfomessage]").text('Occurrence Code is invalid');
            }
        });
       
    }
    function DeleteOccurrenceItem(ClaimsOccurrenceInformationID, Claim_ID) {
        var claimOccurrenceCodeList;
        var result = confirm("Are you sure you want to delete?");
        if (result) {

            var Claim_ID = $("#<%= hdnClaimId.ClientID %>").val();
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteOccurrencInfoCode?Claim_ID=" + Claim_ID + "&&ClaimsOccurrenceInformationID=" + ClaimsOccurrenceInformationID+"",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimOccurrenceSpanData = result;
                    var totalCount = 0;
                    if (claimOccurrenceSpanData.length > 0) {
                        var table;
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Occurrence Code</th><th style='width:10px; scope='col'>Occurrence Date</th><th style='width:30px; scope='col'>Occurrance Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                        for (var i = 0; i < claimOccurrenceSpanData.length; i++) {
                            totalCount++;
                            var Line = result[i].Line;
                            var OccurrenceInfo_Code = result[i].OccurrenceInfoCode;
                            var OccurrenceInfoDate = result[i].OccurrenceDate;
                            var OccurrenceInfoDesc = result[i].OccurrenceDesc;
                            var ClaimsOccurrenceInformationID = result[i].ClaimsOccurrenceInformationID;
                            var Claim_ID = result[i].ClaimId;

                            TotalCountintheGrid = totalCount;
                            if (totalCount >= 24) { document.getElementById('<%= btnAddOccurrence.ClientID%>').style.visibility = "hidden"; }
                                else { document.getElementById('<%= btnAddOccurrence.ClientID%>').style.visibility = "visible"; }
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span title='Occurrence Code' class='tNumber'>" + OccurrenceInfo_Code + "</span></td><td><span  title='OccurrenceDate' class='tNumber'>" + OccurrenceInfoDate + "</span></td><td><span title='OccurenceCode Description' class='tNumber'>" + OccurrenceInfoDesc + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOccurrenceItem(\"" + ClaimsOccurrenceInformationID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOccurrenceItem(\"" + ClaimsOccurrenceInformationID + "\",\"" + Claim_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        };
                        table = table + "</tbody></table>";
                        document.getElementById('<%= divOccurrenceInfo.ClientID %>').innerHTML = table;
                        localStorage.setItem("OccurrenceInfoTable", "" + table + "");
                        OccurrenceInfoClearFields();
                        return false;

                    }
                    else {
                        document.getElementById('<%= divOccurrenceInfo.ClientID %>').innerHTML = "";
                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblOccurrenceInfomessage]").text('Occurrence Code not found.');
                }
            });
        }
    }
    function UpdateOccurrenceInfo() {        
        var claimOccurrenceSpanData;
        var OccurrenceCode = $("#<%= txtOccurrenceCode.ClientID %>").first().val();
        var OccurrecneDate = $("#<%= txtOccurenceDate.ClientID %>").first().val();        
        var OccurrenceDesc = document.getElementById('<%= lblOccurrenceDesc.ClientID%>').innerHTML;
        var ClaimID = $("#<%= hdnClaimId.ClientID %>").first().val();
        var ClaimsOccurrenceInformationID = $("#<%= hdnClaimsOccurrenceInformationID.ClientID %>").val();
        var OccLine = $("#<%= hdnOccLine.ClientID %>").val();
        var ValOccurrenceInfo = "";
        ValidateOccurrenceInfoDetail();
        if (ValOccurrenceInfo === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "POST",
                url: webApiClaims + "EditDataOfOccurrenceInfo?ClaimID=" + ClaimID + "&&ClaimsOccurrenceInformationID=" + ClaimsOccurrenceInformationID + "&&OccurrenceCode=" + OccurrenceCode + "&&OccurrecneDate=" + OccurrecneDate + "&&OccurrenceDesc=" + OccurrenceDesc + "&&OccLine=" + OccLine + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                //data: '{ClaimID: "' + ClaimID + '" , ClaimsOccurrenceInformationID: "' + ClaimsOccurrenceInformationID + '" , OccurrenceCode: "' + OccurrenceCode + '" , OccurrecneDate: "' + OccurrecneDate + '" , OccurrenceDesc: "' + OccurrenceDesc + '",OccLine:"' + OccLine + '" }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimOccurrenceSpanData = result;
                    var totalCount = 0;
                    var Error = null;
                    if (claimOccurrenceSpanData.length > 0) {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Occurrence Code</th><th style='width:10px; scope='col'>Occurrence Date</th><th style='width:30px; scope='col'>Occurrance Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        for (var i = 0; i < claimOccurrenceSpanData.length; i++) {
                            if (result[i].Error != null) {
                                Error = result[i].Error;
                            }
                        }
                        if (Error == null || Error == undefined || Error == "") {
                            for (var i = 0; i < claimOccurrenceSpanData.length; i++) {
                                totalCount++;
                                var Line = result[i].Line;
                                var OccurrenceInfo_Code = result[i].OccurrenceInfoCode;
                                var OccurrenceInfoDate = result[i].OccurrenceDate;
                                var OccurrenceInfoDesc = result[i].OccurrenceDesc;
                                var ClaimsOccurrenceInformationID = result[i].ClaimsOccurrenceInformationID;
                                var Claim_ID = result[i].ClaimId;

                                TotalCountintheGrid = totalCount;
                                if (totalCount >= 24) { document.getElementById('<%= btnAddOccurrence.ClientID%>').style.visibility = "hidden"; }
                                else {
                                    document.getElementById('<%= btnAddOccurrence.ClientID%>').style.visibility = "visible";
                                }
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span title='Occurrence Code' class='tNumber'>" + OccurrenceInfo_Code + "</span></td><td><span  title='OccurrenceDate' class='tNumber'>" + OccurrenceInfoDate + "</span></td><td><span title='OccurenceCode Description' class='tNumber'>" + OccurrenceInfoDesc + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOccurrenceItem(\"" + ClaimsOccurrenceInformationID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOccurrenceItem(\"" + ClaimsOccurrenceInformationID + "\",\"" + Claim_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                            };
                            table = table + "</tbody></table>";
                            document.getElementById('<%= divOccurrenceInfo.ClientID %>').innerHTML = table;
                            localStorage.setItem("OccurrenceInfoTable", "" + table + "");
                            document.getElementById('<%= btnAddOccurrence.ClientID%>').style.visibility = "visible";
                            document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
                            document.getElementById('<%= btnCancelOccurrence.ClientID%>').style.visibility = "hidden";
                            OccurrenceInfoClearFields();
                        }
                        else {
                            $("[id*=lblOccurrenceInfomessage]").text('Duplicate occurrence Code is not allowed. ');
                        }
                    }
                   
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblOccurrenceInfomessage]").text('Occurrence Code not found.');
                }
            });
        }

        return false;
        function ValidateOccurrenceInfoDetail() {
            if (OccurrenceCode == null || OccurrenceCode == undefined || OccurrenceCode == "") {
                ValOccurrenceInfo = "false";
                $("[id*=lbltxtOccurrenceCode]").text('Occurrecne Code is required');
                return false;
            }
            else {
                ValOccurrenceInfo = "true";
                $("[id*=lbltxtOccurrenceCode]").text('');
            }
            if (OccurrecneDate == null || OccurrecneDate == undefined || OccurrecneDate == "") {
                ValOccurrenceInfo = "false";
                $("[id*=lbltxtOccurenceDate]").text('Occurrecne Date is required');
                return false;
            }
            else {
                ValOccurrenceInfo = "true";
                $("[id*=lbltxtOccurenceDate]").text('');
            }
            if (OccurrenceDesc == null || OccurrenceDesc == undefined || OccurrenceDesc == "") {
                ValOccurrenceInfo = "false";
                $("[id*=ErrlblOccurrenceDesc]").text('Occurrence Description is required');
                return false;
            }
            else {
                ValOccurrenceInfo = "true";
                $("[id*=ErrlblOccurrenceDesc]").text('');
            }
            if (validateDate7() === true) {
                ValOccurrenceInfo = "true";

            }
            else {
                ValOccurrenceInfo = "false";
                return false;
            }

        }
        return false;
    }
    function loadSearchOccurrenceCodeInformation() {
        document.getElementById('<%= BtnSearchOccurrence.ClientID %>').style.display = 'none';        
        document.getElementById('<%= txtSearchoOccuCode.ClientID %>').disabled = true;
         document.getElementById('<%= txtSearchOccurrenceDesc.ClientID %>').disabled = true;
     }
    function visibleOccurrenceCode()
{
    $("#<%=gvOccurrenceCodeSearch.ClientID %>").html("");
    $("#<%=txtSearchoOccuCode.ClientID %>").val("");
    $("#<%=txtSearchOccurrenceDesc.ClientID %>").val("");
    localStorage.setItem("indexoccurence", "");
    $find("mpeOccurrenceCode").show();
    return false;
}
    function OccurrenceCodetextChange() {
        $("[id*=lblOccurrenceInfomessage]").text("");
        $("[id*=lbltxtOccurrenceCode]").text('');
        $("[id*=lbltxtOccurenceDate]").text('');
        $("[id*=ErrlblOccurrenceDesc]").text('');

        var txtOccurrenceCode = $("#<%= txtOccurrenceCode.ClientID %>").first().val();
        if (txtOccurrenceCode.length > 1) {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "GetClaimOccurenceInfoDescription?desc=" + txtOccurrenceCode,
                //data: '{desc: "' + txtOccurrenceCode + '" }',
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
                        $("[id*=lblOccurrenceDesc]").text("");
                        $("[id*=lblOccurrenceInfomessage]").text('Occurrence Code not found.');
                    }
                    else {
                        $("[id*=lblOccurrenceDesc]").text(result[0]["CLAIMS_OCCURRENCE_CODE_DESC"]);
                        $("[id*=lblOccurrenceInfomessage]").text("");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblOccurrenceInfomessage]").text('Occurrence Code not found.');
                    $("[id*=lblOccurrenceInfomessage]").text("");
                }
            });
        }
        else {
            $("[id*=lblOccurrenceDesc]").text("");
            $("[id*=lblOccurrenceInfomessage]").text('Occurrence Code not found.');
        }
    }

    function validateDate7() {        
        var daterequested = document.getElementById("<%= txtOccurenceDate.ClientID%>").value;

         var tdate = new Date();
         var dd = tdate.getDate();
         var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
         var yyyy = tdate.getFullYear();
         var currentDate = (MM) + "/" + dd + "/" + yyyy;

        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;

        if (!(date_regex.test(daterequested))) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimOccurrenceInformation1_OccInfoDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimOccurrenceInformation1_OccInfoDateRequiredError1').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimOccurrenceInformation1_txtOccurenceDate').val('');
            return false;
        }
        else if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimOccurrenceInformation1_OccInfoDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimOccurrenceInformation1_OccInfoDateRequiredError1').text('Future Date not allowed.');
            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimOccurrenceInformation1_txtOccurenceDate').val('');
            return false;
        }
        else {

            $('#ctl00_MainContent_uc5SubmitClaim_ucSubmitClaimOccurrenceInformation1_OccInfoDateRequiredError1').css('display', 'none');
            return true;
        }

        

    }

    function GetOccurenceCodeDetails() {
        
        $("[id*=lblError]").text('');
        var txtOccurenceCode = $("#<%= txtSearchoOccuCode.ClientID %>").first().val();
        var txtOccurencecodedesc = $("#<%= txtSearchOccurrenceDesc.ClientID %>").first().val();
        if ((txtOccurenceCode == "" || txtOccurenceCode == null || txtOccurenceCode == undefined) && (txtOccurencecodedesc == "" || txtOccurencecodedesc == null || txtOccurencecodedesc == undefined)) {
            $("[id*=lblError]").text('Occurrence Code or Occurrence Description is required.');
            return false;
        }
        
        $("#<%=gvOccurrenceCodeSearch.ClientID %>").html("");
        var APIToken = $("[id*=hdnAccessToken]").val();
         $.ajax({
             type: "GET",
             url: webApiClaims + "GetOccurenceDetails?desc=" + txtOccurencecodedesc + "&&val=" + txtOccurenceCode,
             //data: '{desc: "' + txtOccurencecodedesc + '" , val: "' + txtOccurenceCode + '" }',
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
                     $("#<%=gvOccurrenceCodeSearch.ClientID %>").append("<tr><td> No Records Found </td></tr>");
                 }
                 else {
                     $("#<%=gvOccurrenceCodeSearch.ClientID %>").append("<tr><th>OCCURRENCE CODE </th><th>OCCURRENCE CODE DESCRIPTION </th></tr>");
                     for (var i = 0; i < result.length; i++) {                         
                         $("#<%=gvOccurrenceCodeSearch.ClientID %>").append("<tr><td><a onClick='GetSelectedRowOccurencecodedetails(this); return false;'>" + result[i].occur_Code + "</a></td><td>" + result[i].occur_Desc + "</td></tr>");
                     };
                 }
             },
             error: function (jqXHR, textStatus, errorThrown) {
                 $("[id*=lblOccurrenceInfomessage]").text('Occurence Code Not Found.');
             }
         });
          return false;
    }

    function GetSelectedRowOccurencecodedetails(lnk) {       
            $find("mpeOccurrenceCode").hide();
        var gridindex = localStorage.getItem("indexoccurence");
      
        var textboxrow = lnk.parentNode.parentNode;
        $("[id*=txtOccurrenceCode]").val(textboxrow.cells[0].innerText.trim());
        $("[id*=lblOccurrenceDesc]").html(textboxrow.cells[1].innerHTML);
        return false;
   
    }

    function visibleOccurenceCodeGridView(lnk) {
        $("#<%=gvOccurrenceCodeSearch.ClientID %>").html("");
         $("#<%=txtSearchoOccuCode.ClientID %>").val("");
        $("#<%=txtSearchOccurrenceDesc.ClientID %>").val("");

        var index = lnk.parentNode.parentNode.rowIndex;
        localStorage.setItem("indexoccurence", index);

        $find("mpeOccurrenceCode").show();
        return false;

    }

</script>
<asp:HiddenField ID="hdnClaimId" runat="server"  />
<asp:HiddenField ID="hdnOccLine" runat="server" />
<asp:HiddenField ID="hdnClaimsOccurrenceInformationID" runat="server"  />

 

                   
               
   <asp:Panel ID="OccurrenceInfoDataEntry" runat="server" Visible="true">
         <asp:HiddenField ID="hdnValueOccuranceCode" runat="server" />
                     <asp:HiddenField ID="hdnValueOccuranceCodeDesc" runat="server" />
        <div runat="server" id="divOccurrenceInfo"></div>
       <div runat="server" id="OccurrenceIn">

       <div class="row" style="padding-left: 40px;">
           <div class="col-sm-6 col-md-4 col-lg-3">
               <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color: red">* </span>Occurrence Code</span>
           </div>
           <div class="col-sm-6 col-md-4 col-lg-3">
               <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color: red">* </span>Occurrence Date</span>
           </div>
           <div class="col-sm-6 col-md-4 col-lg-3">
               <span class="ohio-field-label" style="font-size: 17px; font-weight: bold; padding-left: 30px;">Occurrence Description</span>
           </div>
           <div class="col-sm-6 col-md-4 col-lg-3">
               <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"></span>
           </div>
         
       </div>

        <div class="col-sm-12"  style="padding-left: 40px;" >
           

            <div class="col-sm-6 col-md-4 col-lg-3">

                <div class="row">
                    <span style="text-align: left;">
                       <p><asp:TextBox ID="txtOccurrenceCode" runat="server" MaxLength="2" CssClass="formFieldTextBox" Style="height: 30px; width: 200px"  onChange="return OccurrenceCodetextChange(this)" EnableViewState="true" />
                          <asp:LinkButton ID="lnkOccurencesrch" runat="server" Text="Search" Style="font-size: 14px;" OnClientClick="return visibleOccurrenceCode()"  CausesValidation="false" /></p></span>  <br />                   
                          
                    <asp:Label ID="lbltxtOccurrenceCode" runat="server" ForeColor="Red"></asp:Label>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtOccurrenceCode" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                <asp:Label ID="lblOccurrenceInfomessage"  runat="server"  ForeColor="Red" Text=""></asp:Label>
                </div>
            </div>

            <div class="col-sm-6 col-md-4 col-lg-3">               
                <div>
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtOccurenceDate" runat="server" CssClass="formFieldTextBox" Style="height: 30px; width: 200px"  ValidationGroup="newOccurrence"/>
                        <ajax:CalendarExtender ID="ceOccurenceDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtOccurenceDate"
                            PopupPosition="TopRight" CssClass="QstCalendarCSS" PopupButtonID="imgDate" EnabledOnClient="true" />

                       
                        
                        <asp:Image ID="imgOccurenceDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" /><br />
                        
                        <asp:Label ID="lbltxtOccurenceDate" runat="server" ForeColor="Red"></asp:Label>
                     
                         <asp:Label ID="OccInfoDateRequiredError1" runat="server" Text="" CssClass="error-message" style="display:none;"></asp:Label>
                        
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtOccurenceDate" ErrorMessage="Select a Valid Occurrence Date."
                            Display="Dynamic" Text="*" ValidationGroup="newOccurrence" OnServerValidate="ReportOccurenceDate_ServerValidate" />
                    </span>

                </div>
            </div>

            <div class="col-sm-6 col-md-4 col-lg-3">                
                <span style="text-align: left;">
                <asp:Label runat="server" ID="lblOccurrenceDesc"></asp:Label>
                  <%--  <asp:Label runat="server" ID="ErrlblOccurrenceDesc"></asp:Label>--%>
                </span>
            </div>

            <div class="col-sm-6 col-md-4 col-lg-3">
                               <br />
                <asp:Button ID="btnAddOccurrence" Text="Add" OnClientClick="return AddOccurrenceInfoData()" runat="server" CssClass="btn btn-primary"  CausesValidation="true"  />   
                 <br /> <asp:Button ID="btnEdit" Style="visibility: hidden" Text="Update" OnClientClick="return UpdateOccurrenceInfo()"   runat="server" CssClass="btn btn-primary" Font-Bold="True" Width="70px" />
            <asp:Button ID="btnCancelOccurrence" Style="visibility: hidden" Text="Cancel" OnClientClick="return CancelOccurrenceInfoDetails();" runat="server" CssClass="btn btn-danger" Font-Bold="True" ValidationGroup="newOccurrence" Width="70px" />
            </div>
        </div>
        <div class="form-group row" style="padding-left: 250px">
            <asp:Label ID="lblOccurCodeError" runat="server" ForeColor="Red"></asp:Label>
        </div>

       <br />
      

           </div>
       
       
    </asp:Panel>
    
    <asp:UpdatePanel runat="server"  ID="updateOCSearch">

    <ContentTemplate>
         <ajax:ModalPopupExtender BehaviorID="mpeOccurrenceCode" ID="mpeSubmitOccuurenceSearch" runat="server" PopupControlID="pnlOccurrencePop" TargetControlID="ButtonSearchOccurrence" BackgroundCssClass="modalBackground" CancelControlID="btnCloseProc" />
        <asp:Panel ID="pnlOccurrencePop" runat="server" CssClass="modalPopup " Style="display: none; height: auto; width: auto; min-height: 350px; min-width: 1500px;">
            <asp:Panel ID="pnlOccurrenceSearchHeader" CssClass="popHeader popUpHeader" runat="server" HorizontalAlign="Left" Style="height: auto; width: auto; min-height: 40px; min-width: 100px;">
                <asp:Button runat="server" ID="btnCloseProc" Text="X" CssClass="popUpClose" CausesValidation="false"  />
                <%--<div class="popTitle">
                    <asp:Label ID="lblOccurrenceCodeSearch" CssClass="bodyTextBold" runat="server" Text="OCCURRENCE CODE SEARCH" ForeColor="White" />
                </div>--%>
                 <div class="search-Results">
                <span style="padding-left: 40px; font-size: 17px;">OCCURRENCE CODE</span>
                <span style="padding-left: 80px; font-size: 17px;">OCCURRENCE CODE DESCRIPTION</span>
            </div>
            </asp:Panel>
            <asp:Panel ID="pnlOccurrencePopSearch" runat="server">

                <asp:HiddenField ID="hdnEditSearch" runat="server" />
                <%--<div>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ValidationGroup="SearchOccurrence" ShowSummary="true" />
                </div>--%>
                  <div>
                    <asp:Label runat="server" ID="lblError" ForeColor="Red" />
                </div>
                <div class="row m-0 popUpSearch-Context d-FlexCenter">
                   
                    <div class="col-sm-6 col-md-4 col-lg-2 ">
                        <span class="ohio-field-label" style="padding-left:10px;font-size:17px;" >
                            <asp:TextBox ID="txtSearchoOccuCode" CssClass="ohio-field-input" runat="server"></asp:TextBox>
                        </span>
                    </div>

                    <div class="col-sm-6 col-md-4 col-lg-8 ">
                        <span class="ohio-field-label">
                            <asp:TextBox ID="txtSearchOccurrenceDesc" CssClass="ohio-field-input" runat="server">
                            </asp:TextBox>
                        </span>
                    </div>

                    <div class="col-sm-6 col-md-4 col-lg-2 text-center ">                       
                     
                               <br />
                        <asp:Button ID="BtnSearchOccurrence" runat="server" CausesValidation="false" Text="Search" CssClass="buttonBoxFocus"  OnClientClick="return GetOccurenceCodeDetails(this);" Style="background-color: darkslateblue !important" />
                    </div>
                   
                </div>
                <div class="search-Results">SEARCH RESULTS</div>
                <asp:Label ID="lblSearchMsg" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>

                <div class="result-Container">
                     <div class="popupGridViewOnSearch">
                      <mms:SortablePagingGridView
                ID="gvOccurrenceCodeSearch"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="gridViewSmallFont" Width="100%"
                AllowSorting="true"
                EmptyDataText="No Providers found."
                OnPageIndexChanging="gvOccurrenceCodeSearch_PageIndexChanging"            
                RowStyle-VerticalAlign="Top"
                AllowPaging="True"
                PageSize="15"
                GridViewSortColumn="Code" GridViewSortDirection="Ascending"
                DataKeyNames="CLAIMS_OCCURRENCE_CODE, CLAIMS_OCCURRENCE_CODE_DESC">
                <Columns>

                    
                </Columns>
            </mms:SortablePagingGridView>

</div>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="ButtonSearchOccurrence" Style="display: none" Text="" />

   
   </ContentTemplate>
    </asp:UpdatePanel>

 

                 
