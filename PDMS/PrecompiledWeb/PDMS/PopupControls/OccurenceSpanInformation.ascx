<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_OccurenceSpanInformation, App_Web_qlfnt5yf" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/SearchOccurrenceSpan.ascx" TagPrefix="uc"
    TagName="SearchOccurrenceSpan" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<link href="../Content/custom-style.css" rel="stylesheet" />
 <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">

<script language="JavaScript">
    var TotalCountinOccSpanGrid;
    function CancelOccurrencespanInfo() {
        $("#<%= txtOccurrenceSpanCode.ClientID %>").val('');
        $("#<%= txtoccFromDate.ClientID %>").val('');
        $("#<%= txtoccToDate.ClientID %>").val('');
        $("#<%= lblspanOccurrenceDesc.ClientID %>").html('');
        $("[id*=OccSpanInfoDateRequiredError1]").text('');
        $("[id*=lblOccurrenceSpanCode]").text("");
        $("[id*=OccSpanDateRequiredError1]").text("");
        $("[id*=lblmessage]").text("");
        if (TotalCountinOccSpanGrid >= 24) {
            document.getElementById('<%= btnAddOcc.ClientID%>').style.visibility = "hidden";
        }
        else {
            document.getElementById('<%= btnAddOcc.ClientID%>').style.visibility = "visible";
        }
        document.getElementById('<%= btnCancelOccurrenceSpan.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnEditOccurrenceSpan.ClientID%>').style.visibility = "hidden";
        var table = localStorage.getItem("OccSpanTable");
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= divOccurrenceSpan.ClientID %>').innerHTML = table;
        }
        return false;
    }
    function AddOccurrenceSpan() {        
        var validateOccurrenceSpanDetails = "";
        $("[id*=lblmessage]").text('');
        var claimOccurrenceSpanData;
        var Occurrence_Code_Span = $("#<%= txtOccurrenceSpanCode.ClientID %>").first().val();
        var FromDate = $("#<%= txtoccFromDate.ClientID %>").first().val();
        var ToDate = $("#<%= txtoccToDate.ClientID %>").first().val();
        var OccurrenceDesc = $("#<%= lblspanOccurrenceDesc.ClientID %>").first().val();
        var Claim_ID = $("#<%= hdnClaimOccurenceSpanInformation.ClientID %>").first().val();
        validateOccurrenceSpanDetail();        
        if (validateOccurrenceSpanDetails === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "POST",
                url: webApiClaims + "AddOccurrenceSpanData?Claim_ID=" + Claim_ID + "&&Occurrence_Code_Span=" + Occurrence_Code_Span + "&&FromDate=" + FromDate + "&&ToDate=" + ToDate + "&&OccurrenceDescription=" + OccurrenceDesc + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                //data: '{Claim_ID: "' + Claim_ID + '" , Occurrence_Code_Span: "' + Occurrence_Code_Span + '" , FromDate: "' + FromDate + '", ToDate: "' + ToDate + '", OccurrenceDescription: "' + OccurrenceDesc + '" }',
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
                    var table;
                    var Error = null;
                    if (claimOccurrenceSpanData.length > 0) {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Occurrence Span Code</th><th style='width:10px; scope='col'>From Date</th><th style='width:10px; scope='col'>To Date</th><th style='width:30px; scope='col'>Occurrance Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        var totalCount = 0;                        
                        for (var i = 0; i < claimOccurrenceSpanData.length; i++) {
                            if (result[i].Error != null) {
                                Error = result[i].Error;
                            }
                        }
                        if (Error == null || Error == undefined || Error == "") {
                            for (var i = 0; i < claimOccurrenceSpanData.length; i++) {
                                totalCount++;

                                var Line = result[i].Line;
                                var OccurrenceSpanCode = result[i].OccurrenceSpanCode;
                                var FromDate = result[i].FromDate;
                                var ToDate = result[i].ToDate;
                                var OccurenceCodeDescription = result[i].OccurrenceDesc;
                                var Claims_Occurrence_Code_Span_Information_ID = result[i].Claims_Occurrence_Code_Span_Information_ID;
                                var Claim_ID = result[i].Claim_ID;

                                TotalCountinOccSpanGrid = totalCount;
                                if (totalCount >= 24) { document.getElementById('<%= btnAddOcc.ClientID%>').style.visibility = "hidden"; }
                                else { document.getElementById('<%= btnAddOcc.ClientID%>').style.visibility = "visible"; }
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span title='Occurrence Span Code' class='tNumber'>" + OccurrenceSpanCode + "</span></td><td><span  title='FromDate' class='tNumber'>" + FromDate + "</span></td><td><span title='ToDate' class='tNumber'>" + ToDate + "</span></td><td><span title='ToDate' class='tNumber'>" + OccurenceCodeDescription + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOccurrenceSpanItem(\"" + Claims_Occurrence_Code_Span_Information_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOccurrenceSpanItem(\"" + Claims_Occurrence_Code_Span_Information_ID + "\",\"" + Claim_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            };
                            table = table + "</tbody></table>";
                            localStorage.setItem("OccSpanTable", "" + table + "");
                            document.getElementById('<%= divOccurrenceSpan.ClientID %>').innerHTML = table;
                            OccurrencePanelClearFields();

                        }
                        else {
                            $("[id*=lblmessage]").text('Duplicate occurrence span code is not allowed. ');
                        }
                    }
                    else {
                        OccurrencePanelClearFields();
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblmessage]").text('Occurrence Span Code not found.');
                }
            });
           
        }
        
        function validateOccurrenceSpanDetail() {
            
            $("[id*=OccSpanInfoDateRequiredError1]").text('');
            $("[id*=lblOccurrenceSpanCode]").text("");
            $("[id*=OccSpanDateRequiredError1]").text("");
            $("[id*=lblmessage]").text("");          
            if (Occurrence_Code_Span == "" || Occurrence_Code_Span == null || Occurrence_Code_Span == undefined) {
                validateOccurrenceSpanDetails = "false";
                $("[id*=lblOccurrenceSpanCode]").text('Occurrence Span Code is required');
                return false;
            }
            else {

                validateOccurrenceSpanDetails = "true";
                $("[id*=lblOccurrenceSpanCode]").text('');
            }
            if (FromDate == "" || FromDate == undefined || FromDate == null) {
                validateOccurrenceSpanDetails = "false";
                $("[id*=OccSpanInfoDateRequiredError1]").text('From date is required');
                $("[id*=OccSpanInfoDateRequiredError1]").show();
                return false;
            }
            else {
                validateOccurrenceSpanDetails = "true";
                $("[id*=OccSpanInfoDateRequiredError1]").text('');               
            }
            if (ToDate == "" || ToDate == undefined || ToDate == null) {
                validateOccurrenceSpanDetails = "false";
                $("[id*=OccSpanDateRequiredError1]").text('To Date is required');
                $("[id*=OccSpanDateRequiredError1]").show();
                return false;
            }
            else {
                validateOccurrenceSpanDetails = "true";
                $("[id*=OccSpanDateRequiredError1]").text('');
                
            }           
            if (validateDateFromDate() === true) {
                validateOccurrenceSpanDetails = "true";
                
            }
            else {
                validateOccurrenceSpanDetails = "false";
                return false;
            }
            if (validateDateToDate() === true) {
                validateOccurrenceSpanDetails = "true";
                
            } else {
                validateOccurrenceSpanDetails = "false";
                return false;
            }
            
            if (FromDate != "" && ToDate != "") {                
                var From = new Date(FromDate);
                var To = new Date(ToDate);
                if (From > To) {
                    validateOccurrenceSpanDetails = "false";
                    $("[id*=lblmessage]").text('From date cannot be greater than To date');
                    return false;
                }
                else {
                    validateOccurrenceSpanDetails = "true";
                    $("[id*=lblmessage]").text('');                  
                }
            }
           
            
        }
        return false;
    }
    
    function EditOccurrenceSpanInfoCode() {        
        var claimOccurrenceSpanData;
        var Occurrence_Code_Span = $("#<%= txtOccurrenceSpanCode.ClientID %>").first().val();
        var FromDate = $("#<%= txtoccFromDate.ClientID %>").first().val();
        var ToDate = $("#<%= txtoccToDate.ClientID %>").first().val();
        var OccurrenceDesc = $("#<%= lblspanOccurrenceDesc.ClientID %>").first().val();
        var Claim_ID = $("#<%= hdnClaimOccurenceSpanInformation.ClientID %>").first().val();
        var Claims_Occurrence_Code_Span_Information_ID = $("#<%= hdnClaimsOccurrenceSpanInformationID.ClientID %>").val();
        var Line = $("#<%= hdnLine.ClientID %>").val();
        validateOccurrenceSpanDetail();
        if (validateOccurrenceSpanDetails === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "PUT",
                url: webApiClaims + "EditOccurrenceSpanInfoData?Claim_ID=" + Claim_ID + "&&Claims_Occurrence_Code_Span_Information_ID=" + Claims_Occurrence_Code_Span_Information_ID + "&&Occurrence_Code_Span=" + Occurrence_Code_Span + "&&FromDate=" + FromDate + "&&ToDate=" + ToDate + "&&OccurrenceDesc=" + OccurrenceDesc + "&&Line=" + Line + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                //data: '{Claim_ID: "' + Claim_ID + '" , Claims_Occurrence_Code_Span_Information_ID: "' + Claims_Occurrence_Code_Span_Information_ID + '" , Occurrence_Code_Span: "' + Occurrence_Code_Span + '" , FromDate: "' + FromDate + '" , ToDate: "' + ToDate + '" , OccurrenceDesc: "' + OccurrenceDesc + '",Line:"' + Line + '" }',
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
                    for (var i = 0; i < claimOccurrenceSpanData.length; i++) {
                        if (result[i].Error != null) {
                            Error = result[i].Error;
                        }
                    }
                    if (Error == null || Error == undefined || Error == "") {
                        if (claimOccurrenceSpanData.length > 0) {

                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Occurrence Span Code</th><th style='width:10px; scope='col'>From Date</th><th style='width:10px; scope='col'>To Date</th><th style='width:30px; scope='col'>Occurrance Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                            for (var i = 0; i < claimOccurrenceSpanData.length; i++) {
                                totalCount++;
                                var Line = result[i].Line;
                                var OccurrenceSpanCode = result[i].OccurrenceSpanCode;
                                var FromDate = result[i].FromDate;
                                var ToDate = result[i].ToDate;
                                var OccurenceCodeDescription = result[i].OccurrenceDesc;
                                var Claims_Occurrence_Code_Span_Information_ID = result[i].Claims_Occurrence_Code_Span_Information_ID;
                                var Claim_ID = result[i].Claim_ID;

                                TotalCountinOccSpanGrid = totalCount;
                                if (totalCount >= 24) { document.getElementById('<%= btnAddOcc.ClientID%>').style.visibility = "hidden"; }
                                else { document.getElementById('<%= btnAddOcc.ClientID%>').style.visibility = "visible"; }
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span title='Occurrence Span Code' class='tNumber'>" + OccurrenceSpanCode + "</span></td><td><span  title='FromDate' class='tNumber'>" + FromDate + "</span></td><td><span title='ToDate' class='tNumber'>" + ToDate + "</span></td><td><span title='ToDate' class='tNumber'>" + OccurenceCodeDescription + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOccurrenceSpanItem(\"" + Claims_Occurrence_Code_Span_Information_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOccurrenceSpanItem(\"" + Claims_Occurrence_Code_Span_Information_ID + "\",\"" + Claim_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                            };
                            table = table + "</tbody></table>";
                            document.getElementById('<%= divOccurrenceSpan.ClientID %>').innerHTML = table;
                            localStorage.setItem("OccSpanTable", "" + table + "");
                            document.getElementById('<%= btnAddOcc.ClientID%>').style.visibility = "visible";
                            document.getElementById('<%= btnEditOccurrenceSpan.ClientID%>').style.visibility = "hidden";
                            document.getElementById('<%= btnCancelOccurrenceSpan.ClientID%>').style.visibility = "hidden";
                            OccurrencePanelClearFields();
                        }                       
                    }
                    else {
                        $("[id*=lblmessage]").text('Duplicate occurrence span code is not allowed. ');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblmessage]").text('Occurrence Span Code not found.');
                }
            });            
        }        
        return false;
        function validateOccurrenceSpanDetail() {
            $("[id*=OccSpanInfoDateRequiredError1]").text('');
            $("[id*=lblOccurrenceSpanCode]").text("");
            $("[id*=OccSpanDateRequiredError1]").text("");
            $("[id*=lblmessage]").text("");
            if (Occurrence_Code_Span == "" || Occurrence_Code_Span == null || Occurrence_Code_Span == undefined) {
                validateOccurrenceSpanDetails = "false";
                $("[id*=lblOccurrenceSpanCode]").text('Occurrence Span Code is required');
                return false;
            }
            else if (Occurrence_Code_Span.length <= 1)
            {
                validateOccurrenceSpanDetails = "false";
                $("[id*=lblOccurrenceSpanCode]").text('Occurrence Span Code not found');
                return false;
            }
            else {
                validateOccurrenceSpanDetails = "true";
                $("[id*=lblOccurrenceSpanCode]").text('');
            }
            if (FromDate == "" || FromDate == undefined || FromDate == null) {
                validateOccurrenceSpanDetails = "false";
                $("[id*=OccSpanInfoDateRequiredError1]").text('From date is required');
                $("[id*=OccSpanInfoDateRequiredError1]").show();
                return false;
            }
            else {
                validateOccurrenceSpanDetails = "true";
                $("[id*=OccSpanInfoDateRequiredError1]").text('');
            }
            if (ToDate == "" || ToDate == undefined || ToDate == null) {
                validateOccurrenceSpanDetails = "false";
                $("[id*=OccSpanDateRequiredError1]").text('To Date is required');
                $("[id*=OccSpanDateRequiredError1]").show();
                return false;
            }
            else {
                validateOccurrenceSpanDetails = "true";
                $("[id*=OccSpanDateRequiredError1]").text('');

            }
            if (validateDateFromDate() === true) {
                validateOccurrenceSpanDetails = "true";

            }
            else {
                validateOccurrenceSpanDetails = "false";
                return false;
            }
            if (validateDateToDate() === true) {
                validateOccurrenceSpanDetails = "true";

            } else {
                validateOccurrenceSpanDetails = "false";
                return false;
            }

            if (FromDate != "" && ToDate != "") {
                var From = new Date(FromDate);
                var To = new Date(ToDate);
                if (From > To) {
                    validateOccurrenceSpanDetails = "false";
                    $("[id*=lblmessage]").text('From date cannot be greater than To date');
                    return false;
                }
                else {
                    validateOccurrenceSpanDetails = "true";
                    $("[id*=lblmessage]").text('');
                }
            }
          

        }
        return false;
    }
    function EditOccurrenceSpanItem(Claims_Occurrence_Code_Span_Information_ID, Claim_ID,control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        var claimOccurrenceSpanCodeList;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetOccurrenceSpanData?Claims_Occurrence_Code_Span_Information_ID=" + Claims_Occurrence_Code_Span_Information_ID + "&&Claim_ID=" + Claim_ID + "",
            //data: '{Claims_Occurrence_Code_Span_Information_ID:"' + Claims_Occurrence_Code_Span_Information_ID + '",Claim_ID:"' + Claim_ID + '"}',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {                
                claimOccurrenceSpanCodeList = result;
                if (claimOccurrenceSpanCodeList.length > 0) {

                    for (var i = 0; i < claimOccurrenceSpanCodeList.length; i++) {
                        var Line = result[i].Line;
                        var OccurrenceSpanCode = result[i].OccurrenceSpanCode;
                        var FromDate = result[i].FromDate;
                        var ToDate = result[i].ToDate;
                        var OccurenceCodeDescription = result[i].OccurrenceDesc;
                        var Claims_Occurrence_Code_Span_Information_ID = result[i].Claims_Occurrence_Code_Span_Information_ID;
                        var Claim_ID = result[i].Claim_ID;

                        $("[id*=hdnLine]").val("" + Line + "");
                        $("[id*=txtOccurrenceSpanCode]").val("" + OccurrenceSpanCode + "");
                        $("[id*=txtoccFromDate]").val("" + FromDate + "");
                        $("[id*=txtoccToDate]").val("" + ToDate + "");
                        $("[id*=hdnClaimOccurenceSpanInformation]").val("" + Claim_ID + "");//
                        $("[id*=hdnClaimsOccurrenceSpanInformationID]").val("" + Claims_Occurrence_Code_Span_Information_ID + "");

                        document.getElementById('<%= lblspanOccurrenceDesc.ClientID%>').innerHTML = OccurenceCodeDescription;
                        document.getElementById('<%= btnAddOcc.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnEditOccurrenceSpan.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnCancelOccurrenceSpan.ClientID%>').style.visibility = "visible";
                            return false;
                        };
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblmessage]").text('Occurrence Span Code is invalid');
                }
            });


    }
    function DeleteOccurrenceSpanItem(Claims_Occurrence_Code_Span_Information_ID) {     
            var claimOccurrenceSpanData;
            var result = confirm("Are you sure you want to delete?");
        if (result) {

            var Claim_ID = $("#<%= hdnClaimOccurenceSpanInformation.ClientID %>").val();
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteOccurrencSPanCode?Claim_ID=" + Claim_ID + "&&Claims_Occurrence_Code_Span_Information_ID=" + Claims_Occurrence_Code_Span_Information_ID,
                //data: '{Claim_ID: "' + Claim_ID + '" , Claims_Occurrence_Code_Span_Information_ID: "' + Claims_Occurrence_Code_Span_Information_ID + '" }',
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
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Occurrence Span Code</th><th style='width:10px; scope='col'>From Date</th><th style='width:10px; scope='col'>To Date</th><th style='width:30px; scope='col'>Occurrance Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                        for (var i = 0; i < claimOccurrenceSpanData.length; i++) {
                            totalCount++;
                            var Line = result[i].Line;
                            var OccurrenceSpanCode = result[i].OccurrenceSpanCode;
                            var FromDate = result[i].FromDate;
                            var ToDate = result[i].ToDate;
                            var OccurenceCodeDescription = result[i].OccurrenceDesc;
                            var Claims_Occurrence_Code_Span_Information_ID = result[i].Claims_Occurrence_Code_Span_Information_ID;
                            var Claim_ID = result[i].Claim_ID;

                            TotalCountinOccSpanGrid = totalCount;
                            if (totalCount >= 24) { document.getElementById('<%= btnAddOcc.ClientID%>').style.visibility = "hidden"; }
                                else { document.getElementById('<%= btnAddOcc.ClientID%>').style.visibility = "visible"; }
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span title='Occurrence Span Code' class='tNumber'>" + OccurrenceSpanCode + "</span></td><td><span  title='FromDate' class='tNumber'>" + FromDate + "</span></td><td><span title='ToDate' class='tNumber'>" + ToDate + "</span></td><td><span title='ToDate' class='tNumber'>" + OccurenceCodeDescription + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOccurrenceSpanItem(\"" + Claims_Occurrence_Code_Span_Information_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOccurrenceSpanItem(\"" + Claims_Occurrence_Code_Span_Information_ID + "\",\"" + Claim_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        };
                        table = table + "</tbody></table>";
                        document.getElementById('<%= divOccurrenceSpan.ClientID %>').innerHTML = table;
                        localStorage.setItem("OccSpanTable", "" + table + "");
                        OccurrencePanelClearFields();
                        return false;

                    }
                    else {
                        document.getElementById('<%= divOccurrenceSpan.ClientID %>').innerHTML = "";
                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblmessage]").text('Occurrence Span Code not found.');
                }
            });
        }
    }
    function OccurrencePanelClearFields() {    

        $("#<%= txtOccurrenceSpanCode.ClientID %>").val('');
        $("#<%= txtoccFromDate.ClientID %>").val('');
        $("#<%= txtoccToDate.ClientID %>").val('');
        $("#<%= lblspanOccurrenceDesc.ClientID %>").html('');
        $("[id*=OccSpanInfoDateRequiredError1]").text('');
        $("[id*=lblOccurrenceSpanCode]").text("");
        $("[id*=OccSpanDateRequiredError1]").text("");
        $("[id*=lblmessage]").text("");  
    }
    function loadAddSearchOccuranceSpan() {
        var OccSpanCode = $("#<%= txtOccurrenceSpanCode.ClientID %>").first().val();
        var OccFromDate = $("#<%= txtoccFromDate.ClientID %>").first().val();
        var OccToDate = $("#<%= txtoccToDate.ClientID %>").first().val();

        if (OccSpanCode != "" && OccFromDate != "" && OccToDate != "") {
            document.getElementById('<%= btnAddOcc.ClientID %>').style.display = 'none';
            document.getElementById('<%= txtOccurrenceSpanCode.ClientID %>').disabled = true;
        document.getElementById('<%= txtoccFromDate.ClientID %>').disabled = true;
            document.getElementById('<%= txtoccToDate.ClientID %>').disabled = true;
        }
    }
    function visibleSpanCode() {
        $("#<%=ucSearchOccurrenceSpan.FindControl("gvOccurenceCodeSpanSearchPop").ClientID %>").html("");
        $("#<%=ucSearchOccurrenceSpan.FindControl("txtCode").ClientID %>").val("");
        $("#<%=ucSearchOccurrenceSpan.FindControl("txtCodeDescription").ClientID %>").val("");
        localStorage.setItem("indexoccurencecode", "");
        $find("mpeSpanCode").show();
        return false; Onupload
    }
    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^ a-zA-Z0-9]/g, '');
    }
    function OccurrenceSpanCodetextChange() {        
        $("[id*=lblspanOccurrenceDesc]").text("");
        $("[id*=OccSpanInfoDateRequiredError1]").text('');
        $("[id*=lblOccurrenceSpanCode]").text("");
        $("[id*=OccSpanDateRequiredError1]").text("");
        $("[id*=lblmessage]").text("");  
        var txtOccurrenceSpanCode = $("#<%= txtOccurrenceSpanCode.ClientID %>").first().val();
        if (txtOccurrenceSpanCode.length > 1) {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "GetClaimOccurenceDescription?desc=" + txtOccurrenceSpanCode,
                //data: '{desc: "' + txtOccurrenceSpanCode + '" }',
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
                        $("[id*=lblspanOccurrenceDesc]").text("");
                        $("[id*=lblmessage]").text('Occurrence span code not found.');
                        $("#<%= txtOccurrenceSpanCode.ClientID %>").val('');
                    }
                    else {
                        $("[id*=lblspanOccurrenceDesc]").text(result[0]["CLAIMS_OCCURRENCE_CODE_DESC"]);
                        $("[id*=lblmessage]").text('');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblmessage]").text('Occurrence span code not found.');
                    $("[id*=lblspanOccurrenceDesc]").text("");
                    $("#<%= txtOccurrenceSpanCode.ClientID %>").val('');
                }
            });
        }
        else {
            $("[id*=lblspanOccurrenceDesc]").text("");
            $("[id*=lblmessage]").text('Occurrence span code not found.');
        }
    }

    function gidViewOccurrenceSpanCodetextChange(lnk) {

        var gridindex = lnk.parentNode.parentNode.rowIndex;       
         var inputs = grid.rows[gridindex].getElementsByTagName("INPUT");

        var txtSpanCode = inputs[1].value;
        var APIToken = $("[id*=hdnAccessToken]").val();
         $.ajax({
             type: "GET",
             url: webApiClaims + "GetClaimOccurenceDescription?desc=" + txtSpanCode,
             //data: '{desc: "' + txtSpanCode + '" }',
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
                     var row = $(this).closest("tr");
                     $(lnk).parent().parent()
                         .find("input[type=text][id*=lblOccSpanCodeDesc]").text("");
                     $("[id*=lblmessage]").text('Span code not found.');
                 }
                 else {
                     var row = $(lnk).closest("tr");

                     row.find("[id*=lblOccSpanCodeDesc]").text(result[0]["CLAIMS_OCCURRENCE_CODE_DESC"]);
                     $("[id*=lblmessage]").text('');
                 }
             },
             error: function (jqXHR, textStatus, errorThrown) {
                 $("[id*=lblmessage]").text('Value code not found.');
             }
         });
    }   
    function validateDateToDate() {
        
        var daterequested = document.getElementById("<%= txtoccToDate.ClientID%>").value;

         var tdate = new Date();
         var dd = tdate.getDate();
         var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
         var yyyy = tdate.getFullYear();
         var currentDate = (MM) + "/" + dd + "/" + yyyy;

        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;

        if (!(date_regex.test(daterequested))) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucOccurenceSpanInformation_OccSpanDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucOccurenceSpanInformation_OccSpanDateRequiredError1').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_ucOccurenceSpanInformation_txtoccToDate').val('');
            return false;

        }
        else if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucOccurenceSpanInformation_OccSpanDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucOccurenceSpanInformation_OccSpanDateRequiredError1').text('Future Date not allowed.');
            $('#ctl00_MainContent_uc5SubmitClaim_ucOccurenceSpanInformation_txtoccToDate').val('');
            return false;
        }
        else {
            $('#ctl00_MainContent_uc5SubmitClaim_ucOccurenceSpanInformation_OccSpanDateRequiredError1').css('display', 'none');
            return true;

        }

        
    }
    function validateDateFromDate() {        
        var daterequested = document.getElementById("<%= txtoccFromDate.ClientID%>").value;

        var tdate = new Date();
        var dd = tdate.getDate();
        var MM = ((tdate.getMonth() + 1) < 10 ? '0' : '') + (tdate.getMonth() + 1);
        var yyyy = tdate.getFullYear();
        var currentDate = (MM) + "/" + dd + "/" + yyyy;

        var date_regex = /^(0[1-9]|1[0-2])\/(0[1-9]|1\d|2\d|3[01])\/(19|20)\d{2}$/;
        if (!(date_regex.test(daterequested))) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucOccurenceSpanInformation_OccSpanInfoDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucOccurenceSpanInformation_OccSpanInfoDateRequiredError1').text('Please Enter in MM/DD/YYYY Format');
            $('#ctl00_MainContent_uc5SubmitClaim_ucOccurenceSpanInformation_txtoccFromDate').val('');
            return false;

        }
        else if (new Date(daterequested) > new Date(currentDate)) {
            $('#ctl00_MainContent_uc5SubmitClaim_ucOccurenceSpanInformation_OccSpanInfoDateRequiredError1').css('display', 'block');
            $('#ctl00_MainContent_uc5SubmitClaim_ucOccurenceSpanInformation_OccSpanInfoDateRequiredError1').text('Future Date not allowed.');
            $('#ctl00_MainContent_uc5SubmitClaim_ucOccurenceSpanInformation_txtoccFromDate').val('');
            return false;
        }
        else {
            $('#ctl00_MainContent_uc5SubmitClaim_ucOccurenceSpanInformation_OccSpanInfoDateRequiredError1').css('display', 'none');
            return true;

        }

       

    }
    function GetOccurenceSpanDetails() {
        
        var lblSResult = $("#<%= ucSearchOccurrenceSpan.FindControl("lblSResult").ClientID %>").first();
        lblSResult.text('');
        var txtOccurenceSpanCode = $("#<%= ucSearchOccurrenceSpan.FindControl("txtCode").ClientID %>").first().val();
        var txtOccurenceSpancodedesc = $("#<%= ucSearchOccurrenceSpan.FindControl("txtCodeDescription").ClientID %>").first().val();
        $("#<%=ucSearchOccurrenceSpan.FindControl("gvOccurenceCodeSpanSearchPop").ClientID %>").html("");
        if ((txtOccurenceSpanCode == "" || txtOccurenceSpanCode == null || txtOccurenceSpanCode == undefined) && (txtOccurenceSpancodedesc == "" || txtOccurenceSpancodedesc == null || txtOccurenceSpancodedesc == undefined)) {
            lblSResult.text('Occurrence Span Code or Occurrence Span Description is required.');
            return false;
        }
        var APIToken = $("[id*=hdnAccessToken]").val();
         $.ajax({
             type: "GET",
             url: webApiClaims + "GetOccurenceCodeDetails?desc=" + txtOccurenceSpancodedesc + "&&val=" + txtOccurenceSpanCode,
             //data: '{desc: "' + txtOccurenceSpancodedesc + '" , val: "' + txtOccurenceSpanCode + '" }',
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
                     $("#<%=ucSearchOccurrenceSpan.FindControl("gvOccurenceCodeSpanSearchPop").ClientID %>").append("<tr><td> No Records Found </td></tr>");
                  }
                 else {                    
                     $("#<%=ucSearchOccurrenceSpan.FindControl("gvOccurenceCodeSpanSearchPop").ClientID %>").append("<tr><th>OCCURENCE SPAN CODE </th><th>OCCURENCE SPAN CODE DESCRIPTION </th></tr>");
                     for (var i = 0; i < result.length; i++) {                         
                          $("#<%=ucSearchOccurrenceSpan.FindControl("gvOccurenceCodeSpanSearchPop").ClientID %>").append("<tr><td><a onClick='GetSelectedRowOccurencecode(this); return false;'>" + result[i].occurence_Code + "</a></td><td>" + result[i].occurence_Desc + "</td></tr>");
                     };
                 }
             },
             error: function (jqXHR, textStatus, errorThrown) {
                 $("[id*=lblmessage]").text('Occurence Span Code Not Found.');
             }
         });
          return false;
    }
    function GetSelectedRowOccurencecode(lnk) {

        $find("mpeSpanCode").hide();
        var gridindex = localStorage.getItem("indexoccurencecode");

            var textboxrow = lnk.parentNode.parentNode;
            $("[id*=txtOccurrenceSpanCode]").val(textboxrow.cells[0].innerText.trim());
            $("[id*=lblspanOccurrenceDesc]").html(textboxrow.cells[1].innerHTML);
            return false;   
    }
    function visibleOccurencespanCodeGridView(lnk) {
        $("#<%=ucSearchOccurrenceSpan.FindControl("gvOccurenceCodeSpanSearchPop").ClientID %>").html("");
         $("#<%=ucSearchOccurrenceSpan.FindControl("txtCode").ClientID %>").val("");
         $("#<%=ucSearchOccurrenceSpan.FindControl("txtCodeDescription").ClientID %>").val("");

        var index = lnk.parentNode.parentNode.rowIndex;
        localStorage.setItem("indexoccurencecode", index);

        $find("mpeSpanCode").show();
        return false;

    }
</script>
<style>
    .gridview {
        table-layout: auto;
    }
</style>
<asp:Panel ID="pnlOccurrenceSpanInfo" runat="server" Style="height: auto; width: 100%; min-height: 200px">
<asp:HiddenField runat="server" ID="hdnClaimOccurenceSpanInformation" />
<asp:HiddenField runat="server" ID="hdnClaimsOccurrenceSpanInformationID" />
<asp:HiddenField runat="server" ID="hdnClaimType" />
    <asp:HiddenField runat="server" ID="hdnLine" />


<div class="row" style="margin-left: 10px;">
    <div style="text-align: left;">
    <p style="color: red">
        <asp:Label runat="server" class="failureNotification" Style="font-size: 17px;" ID="lblmessage"
            Text=""></asp:Label>
    </p>
</div>
    <div id="divOccurrenceSpan" runat="server"></div>
</div>
<div id="Occuerance" runat="server" style ="height: auto;min-height:270px">
    <div class="row" style="margin-left: 10px">

        <div class="col-sm-4 col-md-2 col-lg-2">
            <span class="ohio-field-label GridviewHeaderAsterisk" style="font-size: 17px; font-weight: bold;">Occurrence
                Span Code</span>
        </div>
        <div class="col-sm-4 col-md-2 col-lg-2">
            <span class="ohio-field-label GridviewHeaderAsterisk" style="font-size: 17px; font-weight: bold;">From Date</span>
        </div>
        <div class="col-sm-4 col-md-2 col-lg-2">
            <span class="ohio-field-label GridviewHeaderAsterisk" style="font-size: 17px; font-weight: bold;">To Date</span>
        </div>
        <div class="col-sm-4 col-md-3 col-lg-4">
            <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">Occurrence Description</span>
        </div>
        <div class="col-sm-8 col-md-3 col-lg-2">
            <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"></span>
        </div>

    </div>



    <div class="row" style="margin-left: 15px">
        <div class="col-sm-4 col-md-2 col-lg-2">

            <div style="text-align: left;">
                <asp:TextBox ID="txtOccurrenceSpanCode" runat="server" MaxLength="2" CssClass="formfieldDate" onKeyUp="javascript:alphanumericOnly(this);" onChange="return OccurrenceSpanCodetextChange(this)"
                    Style="height: 30px; width:100px !important" />               
               
                <asp:LinkButton ID="lnkOccurenceSpanInfoSearch" Text="Search" runat="server" ToolTip="Search" OnClientClick="return visibleSpanCode()" CausesValidation="false"></asp:LinkButton>
            </div>         
            <asp:Label runat="server" ID="lblOccurrenceSpanCode" ForeColor="Red"></asp:Label>
        </div>
        <div class="col-sm-4 col-md-2 col-lg-2">
            <div>
                <asp:TextBox ID="txtoccFromDate" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 130px" />
                <asp:Image ID="imgoccFromDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp"
                    Height="16px" AlternateText="Calendar Icon" />
                <ajax:CalendarExtender ID="ceoccFromDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtoccFromDate" PopupPosition="TopRight" CssClass="QstCalendarCSS" EnabledOnClient="true" />
              
                
                 <asp:Label runat="server" ID="OccSpanInfoDateRequiredError1" ForeColor="Red"></asp:Label>
            </div>
        </div>
        <div class="col-sm-4 col-md-2 col-lg-2">
            <div>
                <asp:TextBox ID="txtoccToDate" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 130px" />
                <asp:Image ID="imgoccToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp"
                    Height="16px" AlternateText="Calendar Icon" />
                <ajax:CalendarExtender ID="cecoccToDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtoccToDate" PopupPosition="TopRight" CssClass="QstCalendarCSS" EnabledOnClient="true" />              
               
                <asp:Label runat="server" ID="OccSpanDateRequiredError1" ForeColor="Red"></asp:Label>
            </div>
        </div>
            <div class="col-sm-6 col-md-4 col-lg-3">                

            <div style="text-align: left;">
                <asp:Label ID="lblspanOccurrenceDesc" runat="server"></asp:Label>
            </div>
        </div>

       <div class="col-sm-3 col-md-2 col-lg-1.5">
           <asp:Button ID="btnAddOcc" Text="ADD" OnClientClick="return AddOccurrenceSpan()" runat="server" CssClass="btn btn-primary" Font-Bold="True" Width="70px" />
                   <br /> <asp:Button ID="btnEditOccurrenceSpan" Style="visibility: hidden" Text="Update"  OnClientClick="return EditOccurrenceSpanInfoCode()"  runat="server" CssClass="btn btn-primary" Font-Bold="True" ValidationGroup="valOccurenceSpanCode" Width="70px" />                
                      <asp:Button ID="btnCancelOccurrenceSpan" Style="visibility: hidden" Text="Cancel" OnClientClick="return CancelOccurrencespanInfo();" runat="server" CssClass="btn btn-danger" Font-Bold="True" ValidationGroup="valOccurenceSpanCode" Width="70px" />  
            </div>
         <div class="col-sm-3 col-md-2 col-lg-1.5">
          
               
            
                     
            </div>

        
        <br />

    </div>
    <ajax:ModalPopupExtender BehaviorID="mpeSpanCode" ID="mpeOccurenceSPanSearchPop"
        runat="server" PopupControlID="pnlOccurenceSPanSearchPop"
        TargetControlID="ButtonSearchOccurenceSpan" BackgroundCssClass="modalBackground"
        CancelControlID="btnCloseOccurenceSpan" />
    <asp:Panel ID="pnlOccurenceSPanSearchPop" runat="server" CssClass="modalPopup" Style="display: none; height: auto; width: auto; min-height: 350px; min-width: 1500px;">
        <asp:Panel ID="pnlSubmitClaimSearchOccurenceSpanHeader" CssClass="popHeader" runat="server"
            HorizontalAlign="Left">
            <asp:Button runat="server" ID="btnCloseOccurenceSpan" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;"
                CausesValidation="false" />
            <div class="search-Results">
                <span style="padding-left: 40px; font-size: 17px;">OCCURRENCE SPAN CODE</span>
                <span style="padding-left: 60px; font-size: 17px;">OCCURRENCE SPAN CODE DESCRIPTION</span>
            </div>
        </asp:Panel>
        <asp:Panel ID="Panel4" runat="server">
           
                <uc:SearchOccurrenceSpan runat="server" ID="ucSearchOccurrenceSpan" Visible="true"
                    EnableViewState="true" />
           
        </asp:Panel>
    </asp:Panel>
</div>

</asp:Panel>
<asp:Button runat="server" ID="ButtonSearchOccurenceSpan" Style="display: none" Text="ButtonSearchOccurenceSpan" />
