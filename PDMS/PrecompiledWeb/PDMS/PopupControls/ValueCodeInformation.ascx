<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ValueCodeInformation, App_Web_l5y5araq" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<script language="javaScript">
    var TotalCountinValueCodeGrid;
    
    function CancelValueCodeFields() {
        $("#<%= txtValueCode.ClientID %>").first().val("");
        $("#<%= txtAmount.ClientID %>").first().val("");
        $("#<%= lblValueCodeDesc.ClientID %>").html("");
        $("[id*=lblErrorValueCodePanel]").text('');
        if (TotalCountinValueCodeGrid >= 24) {
            document.getElementById('<%= btnAddValueCodeInformation.ClientID%>').style.visibility = "hidden";
        }
        else {
            document.getElementById('<%= btnAddValueCodeInformation.ClientID%>').style.visibility = "visible";
        }
        document.getElementById('<%= btnCancelValueCode.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnEditValueCode.ClientID%>').style.visibility = "hidden";
        var table = localStorage.getItem("ValueTable");
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= divValueCodeInformation.ClientID %>').innerHTML = table;
        }
        return false;
    }
    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^ a-zA-Z0-9]/g, '');
    }

    function displaytableValueCode() {

        var table = localStorage.getItem("ValueTable");
        var ValueCodeInfoclaimstatus = document.getElementById("<%=hdnValueCodeClaimStatus.ClientID %>").value;
        if (ValueCodeInfoclaimstatus == "Pending Submission") {
            var table = localStorage.getItem("ValueTable");
        }
        else {
            var table = localStorage.removeItem("ValueTable");
        }
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= divValueCodeInformation.ClientID %>').innerHTML = table;
        }

    };

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

    function visibleValueCode() {
        $("#<%=gvValueCodeSearchPop.ClientID %>").html("");
        localStorage.setItem("index", "");
        $find("mpeValueCode").show();
        return false; Onupload

    }

    function visibleValueCodeGridview(lnk) {

        lnk.parentNode.parentNode.rowIndex;
        var index = lnk.parentNode.parentNode.rowIndex;
        localStorage.setItem("index", index);

        $find("mpeValueCode").show();
        return false;

    }

    function GetSelectedRowvaluecode(lnk) {

        $find("mpeValueCode").hide();

        var gridindex = localStorage.getItem("index");

        if (!(gridindex == "")) {

            var row = lnk.parentNode.parentNode;
            var grid = "";
            var inputs = grid.rows[gridindex].getElementsByTagName("INPUT");

            inputs[1].value = row.cells[0].innerText;
            grid.rows[gridindex].cells[4].innerText = row.cells[1].innerHTML;
            return false;
        }
        else {
            var textboxrow = lnk.parentNode.parentNode;
            $("[id*=txtValueCode]").val(textboxrow.cells[0].innerText.trim());
            $("[id*=lblValueCodeDesc]").html(textboxrow.cells[1].innerHTML);
            return false;
        }
    }

    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^ a-zA-Z0-9]/g, '');
    }
    function gidViewValueCodetextChange(lnk) {

        var gridindex = lnk.parentNode.parentNode.rowIndex;
        var grid = "";
        var inputs = grid.rows[gridindex].getElementsByTagName("INPUT");

        var txtValueCode = inputs[1].value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetValueCodeDescription?desc=" + txtValueCode+"",
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
                        .find("input[type=text][id*=lblValCode]").text("");
                    $("[id*=lblErrorValueCodePanel]").text('Value code not found.');
                }
                else {
                    var row = $(lnk).closest("tr");
                    row.find("[id*=lblValCode]").text(result[0]["CLAIMS_VALUE_CODE_DESC"]);
                    $("[id*=lblErrorValueCodePanel]").text('');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorValueCodePanel]").text('Value code not found.');
            }
        });

    }
    function ValueCodetextChange() {        
        <%--if ($("#<%= txtValueCode.ClientID %>").first().val() <= 9 && $("#<%= txtValueCode.ClientID %>").first().val() > 0) {
            $("#<%= txtValueCode.ClientID %>").first().val(String($("#<%= txtValueCode.ClientID %>").first().val()).padStart(2, '0'));
        }
        else --%>
        if ($("#<%= txtValueCode.ClientID %>").first().val() == 0) {
            $("#<%= txtValueCode.ClientID %>").first().val("");
            $("[id*=lblValueCodeDesc]").empty("");
        }
        var txtValueCode = $("#<%= txtValueCode.ClientID %>").first().val();

        if ($("#<%= txtValueCode.ClientID %>").first().val() == "" && $("#<%= txtValueCode.ClientID %>").first().val() != "00") {
            var txtValueCode = $("#<%= txtValueCode.ClientID %>").first().val();
            $("#<%= txtValueCode.ClientID %>").first().val("");
            $("[id*=lblValueCodeDesc]").empty("");
        }
        else {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "GetValueCodeDescription?desc=" + txtValueCode + "",
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
                        $("[id*=lblValueCodeDesc]").empty("");
                        $("[id*=lblErrorValueCodePanel]").text('Value code not found.');
                    }
                    else {
                        $("[id*=lblValueCodeDesc]").text(result[0]["CLAIMS_VALUE_CODE_DESC"]);
                        $("[id*=lblErrorValueCodePanel]").text('');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblValueCodeDesc]").empty("");
                    $("[id*=lblErrorValueCodePanel]").text('Value code not found.');
                }
            });
        }
    }

    function GetValueCodeDetails() {
        $("[id*=fieldRequireError]").text('');
        var txtValueCode = $("#<%= txtCode.ClientID %>").first().val();
        var txtvaluecodedesc = $("#<%= txtValueDesc.ClientID %>").first().val();
        $("#<%=gvValueCodeSearchPop.ClientID %>").html("");
        if ((txtValueCode == "" || txtValueCode == null || txtValueCode == undefined) && (txtvaluecodedesc == "" || txtvaluecodedesc == null || txtvaluecodedesc == undefined)) {
            $("[id*=fieldRequireError]").text('Value Code or Value Code Description is required.');
            return false;
        }
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetValueCodeDetails?desc=" + txtValueCode + "&&val=" + txtvaluecodedesc,
            //data: '{desc: "' + txtValueCode + '" , val: "' + txtvaluecodedesc + '" }',
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
                    $("#<%=gvValueCodeSearchPop.ClientID %>").append("<tr><td> No Records Found </td></tr>");
                }
                else {
                    $("#<%=gvValueCodeSearchPop.ClientID %>").append("<tr><th>VALUE CODE </th><th>VALUE CODE DESCRIPTION </th></tr>");
                    for (var i = 0; i < result.length; i++) {
                        $("#<%=gvValueCodeSearchPop.ClientID %>").append("<tr><td><a onClick='GetSelectedRowvaluecode(this); return false;'>" + result[i].Value_Code + "</asp:LinkButton></td><td>" + result[i].Value_Desc + "</td></tr>");
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorValueCodePanel]").text('Value code not found.');
            }
        });

        return false;
    }

    function UpdateValuecode() {
        
        validateValueDetail = "true";
        var ClaimID = $("#<%= hdnValueCode_ClaimID.ClientID %>").first().val();
        var Claims_Value_Code_Information_ID = $("#<%= hdnValue_Code.ClientID %>").first().val();
        var Value_Code = $("#<%= txtValueCode.ClientID %>").first().val();
        var Amount = $("#<%= txtAmount.ClientID %>").first().val();
        var ValueCodeDesc = $("#<%= lblValueCodeDesc.ClientID %>").html();
        var Line = $("#<%= hdnLine.ClientID %>").first().val();
        var claimValueCodeData;
        validateValueDetails();
        if (validateValueDetail === "true") {         
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "PUT",
                url: webApiClaims + "EditValueCodeData?ClaimID=" + ClaimID + "&&Claims_Value_Code_Information_ID=" + Claims_Value_Code_Information_ID + "&&Value_Code=" + Value_Code + "&&Amount=" + Amount + "&&ValueCodeDesc=" + ValueCodeDesc + "&&Line=" + Line + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {                    
                    claimValueCodeData = result;
                    var totalCount = 0;
                    var Error = null;
                    for (var i = 0; i < claimValueCodeData.length; i++) {
                        if (result[i].ErrorMessage != null) {
                            Error = result[i].ErrorMessage;
                        }
                    }
                    if (Error == null || Error == undefined || Error == "") {
                        if (claimValueCodeData.length > 0) {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Value Code</th><th style='width:10px; scope='col'>Amount</th><th style='width:30px; scope='col'>Value Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                            for (var i = 0; i < claimValueCodeData.length; i++) {
                                totalCount++;
                                var Line = result[i].Line;
                                var Value_Code = result[i].Value_Code;
                                var Amount = result[i].Amount;
                                var Value_Desc = result[i].ValueCodeDesc;
                                var Claims_Value_Code_Information_ID = result[i].hdnValue_Code;
                                var ClaimID = result[i].ClaimID;
                                $("[id*=hdnLine]").val("" + Line + "");

                                TotalCountinValueCodeGrid = totalCount;
                                if (totalCount >= 24) { document.getElementById('<%= btnAddValueCodeInformation.ClientID%>').style.visibility = "hidden"; }
                                else { document.getElementById('<%= btnAddValueCodeInformation.ClientID%>').style.visibility = "visible"; }
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span  title='Value Code ' class='tNumber'>" + Value_Code + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Value Code Description;' class='tNumber'>" + Value_Desc + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditValuecode(\"" + Claims_Value_Code_Information_ID + "\",\"" + ClaimID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteValuecode(\"" + Claims_Value_Code_Information_ID + "\",\"" + ClaimID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                            };
                            table = table + "</tbody></table>";
                            document.getElementById('<%= divValueCodeInformation.ClientID %>').innerHTML = table;
                            localStorage.setItem("ValueTable", "" + table + "");
                            document.getElementById('<%= btnAddValueCodeInformation.ClientID%>').style.visibility = "visible";
                            document.getElementById('<%= btnEditValueCode.ClientID%>').style.visibility = "hidden";
                            document.getElementById('<%= btnCancelValueCode.ClientID%>').style.visibility = "hidden";
                            clearvaluecode();
                        }
                    }
                    else {
                        $("[id*=lblErrorValueCodePanel]").text('Duplicate Value Code is not allowed. ');
                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorValueCodePanel]").text('Value Code not found.');
                }
            });
           
            return false;
          
        }
        return false;
        function validateValueDetails() {
            if (Value_Code == "") {
                validateValueDetail = "false";
                $("[id*=lblErrorValueCodePanel]").text('Value Code is required');
                return false;
            }
            else {
                validateValueDetail = "true";
                $("[id*=lblErrorValueCodePanel]").text('');
            }

            if (Amount == "") {
                validateValueDetail = "false";
                $("[id*=lblErrorAmount]").text('Amount is required');
                return false;
            }
            else {
                validateValueDetail = "true";
                $("[id*=lblErrorAmount]").text('');
            }

        }
        return false;
    }
    function EditValuecode(valuecodeInfoID, ClaimID, control) {
        
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        var claimValueCodeList;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
                type: "GET",
            
            url: webApiClaims + "GetValueCodeData?valuecodeInfoID=" + valuecodeInfoID + "&&ClaimID=" + ClaimID+"",
            //data: '{valuecodeInfoID:"' + valuecodeInfoID + '",ClaimID:"' + ClaimID + '"}',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
            success: function (result) {
                
                    claimValueCodeList = result;
                    if (claimValueCodeList.length > 0) {
                        for (var i = 0; i < claimValueCodeList.length; i++) {
                            var Line = result[i].Line;
                            var Value_Code = result[i].Value_Code;
                            var Amount = result[i].Amount;
                            var Value_Desc = result[i].ValueCodeDesc;
                            var Claims_Value_Code_Information_ID = result[i].hdnValue_Code;                           
                            var ClaimID = result[i].ClaimID;                         
                            $("[id*=txtValueCode]").val("" + Value_Code + "");
                            $("[id*=txtAmount]").val("" + Amount + "");                        
                            $("[id*=hdnValueCode_ClaimID]").val("" + ClaimID + "");
                            $("[id*=hdnValue_Code]").val("" + Claims_Value_Code_Information_ID + "");
                            $("[id*=hdnLine]").val("" + Line + "");
                            document.getElementById('<%= lblValueCodeDesc.ClientID%>').innerHTML = Value_Desc;
                            document.getElementById('<%= btnAddValueCodeInformation.ClientID%>').style.visibility = "hidden";
                            document.getElementById('<%= btnEditValueCode.ClientID%>').style.visibility = "visible";
                            document.getElementById('<%= btnCancelValueCode.ClientID%>').style.visibility = "visible";
                        return false;
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorValueCodePanel]").text('Value Code is invalid');
            }
        });


        }
    function DeleteValuecode(Claims_Value_Code_Information_ID) { 
        var claimValueCodeDat;
        var result = confirm("Are you sure you want to delete?");
        if (result) {
            var ClaimID = $("#<%= hdnValueCode_ClaimID.ClientID %>").first().val();
            var APIToken = $("[id*=hdnAccessToken]").val();
           $.ajax({
                type: "DELETE",
               url: webApiClaims + "DeleteValueCodeData?ClaimID=" + ClaimID + "&&Claims_Value_Code_Information_ID=" + Claims_Value_Code_Information_ID+"",
               headers: {
                   "Access-Control-Allow-Origin": "*",
                   "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                   "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                   "Authorization": "Bearer " + APIToken
               },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimValueCodeDat = result;
                    var totalCount = 0;
                    if (claimValueCodeDat.length > 0) {
                        var table;
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Value Code</th><th style='width:10px; scope='col'>Amount</th><th style='width:30px; scope='col'>Value Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                        for (var i = 0; i < claimValueCodeDat.length; i++) {
                            totalCount++;
                            var sequence = i;
                            sequence++;

                            var Line = result[i].Line;
                            var Value_Code = result[i].Value_Code;
                            var Amount = result[i].Amount;
                            var Value_Desc = result[i].ValueCodeDesc;
                            var Claims_Value_Code_Information_ID = result[i].hdnValue_Code;
                            var ClaimID = result[i].ClaimID;
                            $("[id*=hdnLine]").val("" + Line + "");

                            TotalCountinValueCodeGrid = totalCount;
                            if (totalCount >= 24) { document.getElementById('<%= btnAddValueCodeInformation.ClientID%>').style.visibility = "hidden"; }
                             else { document.getElementById('<%= btnAddValueCodeInformation.ClientID%>').style.visibility = "visible"; }
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span  title='Value Code ' class='tNumber'>" + Value_Code + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Value Code Description;' class='tNumber'>" + Value_Desc  + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditValuecode(\"" + Claims_Value_Code_Information_ID + "\",\"" + ClaimID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteValuecode(\"" + Claims_Value_Code_Information_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                       };
                        table = table + "</tbody></table>";
                        document.getElementById('<%= divValueCodeInformation.ClientID %>').innerHTML = table;
                        localStorage.setItem("ValueTable", "" + table + "");
                        clearvaluecode();
                        document.getElementById('<%= btnAddValueCodeInformation.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnEditValueCode.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnCancelValueCode.ClientID%>').style.visibility = "hidden";
                        return false;

                    }
                    else {
                        document.getElementById('<%= divValueCodeInformation.ClientID %>').innerHTML = "";
                        clearvaluecode();
                        document.getElementById('<%= btnAddValueCodeInformation.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnEditValueCode.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnCancelValueCode.ClientID%>').style.visibility = "hidden";
                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorValueCodePanel]").text('Value Code not found.');
                }
            });

        }
       
      }
    function AddValueCode() {
        var claimValuecodeData;
        var Value_Code = $("#<%= txtValueCode.ClientID %>").first().val();
        var Amount = $("#<%= txtAmount.ClientID %>").first().val();
        var ValueCodeDesc = $("#<%= lblValueCodeDesc.ClientID %>").first().val();
        var ClaimID = $("#<%= hdnValueCode_ClaimID.ClientID %>").first().val();
        var hdnValue_Code = $("#<%= hdnValue_Code.ClientID %>").first().val();
   
        validateValueDetail();
        if (validateValueDetail === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "POST",
                url: webApiClaims + "AddValueCodeData?ClaimID=" + ClaimID + "&&Value_Code=" + Value_Code + "&&Amount=" + Amount + "&&ValueCodeDesc=" + ValueCodeDesc + "&&hdnValue_Code=" + hdnValue_Code + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                //data: '{ClaimID: "' + ClaimID + '" , Value_Code: "' + Value_Code + '" , Amount: "' + Amount + '", ValueCodeDesc: "' + ValueCodeDesc + '" , hdnValue_Code: "' + hdnValue_Code + '"}',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimValuecodeData = result;
                    var table;
                    var Error = null;
                    if (claimValuecodeData.length > 0) {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Value Code</th><th style='width:10px; scope='col'>Amount</th><th style='width:30px; scope='col'>Value Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        var totalCount = 0;                        
                        for (var i = 0; i < claimValuecodeData.length; i++) {
                            if (result[i].ErrorMessage != null) {
                                 Error = result[i].ErrorMessage;
                            }
                        }
                        if (Error == null || Error == undefined || Error == "") {
                            for (var i = 0; i < claimValuecodeData.length; i++) {
                                totalCount++;
                                var sequence = i;
                                sequence++;

                                var Line = result[i].Line;
                                var Value_Code = result[i].Value_Code;
                                var Amount = result[i].Amount;
                                var ValueCodeDesc = result[i].ValueCodeDesc;
                                var ClaimID = result[i].ClaimID;
                                var Claims_Value_Code_Information_ID = result[i].hdnValue_Code;
                                $("[id*=hdnLine]").val("" + Line + "");
                                if (totalCount >= 24) { document.getElementById('<%= btnAddValueCodeInformation.ClientID%>').style.visibility = "hidden"; }
                                else { document.getElementById('<%= btnAddValueCodeInformation.ClientID%>').style.visibility = "visible"; }
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span  title='Value Code ' class='tNumber'>" + Value_Code + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Value Code Description;' class='tNumber'>" + ValueCodeDesc + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditValuecode(\"" + Claims_Value_Code_Information_ID + "\",\"" + ClaimID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteValuecode(\"" + Claims_Value_Code_Information_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            };
                            table = table + "</tbody></table>";
                            localStorage.setItem("ValueTable", "" + table + "");
                            document.getElementById('<%= divValueCodeInformation.ClientID %>').innerHTML = table;
                        }
                        else {
                            $("[id*=lblErrorValueCodePanel]").text('Duplicate Value Code is not allowed. ');
                            return false;
                        }
                    }

                    else {
                        
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorValueCodePanel]").text('Value Code not found.');
                }
            });

           clearvaluecode();
        }

    
        function validateValueDetail() {
            if (Value_Code == "")
                {
                validateValueDetail = "false";
                $("[id*=lblErrorValueCodePanel]").text('Value Code is required');
                return false;
            }
            else
                {
                validateValueDetail = "true";
                $("[id*=lblErrorValueCodePanel]").text('');
            }

            if (Amount == "") 
               {
                validateValueDetail = "false";
                $("[id*=lblErrorAmount]").text('Amount is required');
                return false;
            }
            else
               {
                validateValueDetail = "true";
                $("[id*=lblErrorAmount]").text('');
             }
            
        }
        return false;
    }


    function clearvaluecode(){

        $("#<%= txtValueCode.ClientID %>").first().val("");
        $("#<%= txtAmount.ClientID %>").first().val("");
        $("#<%= lblValueCodeDesc.ClientID %>").html("");
    }
</script>


         
        <asp:Panel ID="pnlValueCodeInformation" runat="server"  Style="overflow: hidden;min-height:270px;height:auto;">
            <div class="row">
                <div style="padding-top: 10px; padding-left: 15px">
                    <asp:Label ID="hdn" runat="server" Style="display: none"></asp:Label>
                    <div id="divValueCodeInformation" runat="server" />          
                </div>
            </div>
                <div id="divValue" runat="server">
                    <div id="dataEnteringRow" runat="server">
                        <div class="row" style="padding-left: 40px;">
                           <div class="col-sm-6 col-md-4 col-lg-3">
                            <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color: red">*</span>Value Code</span>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 p-0 m-0">
                            <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">
                                <span style="color: red">*</span>Amount</span>
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 p-0 m-0">
                            <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">Value Code
                                    Description</span>
                            </div>
                            <div class="col-md-3">
                            </div>
                        </div>
                            <div class="row" style="padding-left: 40px;">

                                <div class="col-sm-6 col-md-4 col-lg-3">
                                    <div class="row">

                                        <div style="text-align: left;">
                                            <asp:TextBox ValidationGroup="validateValueCode" ID="txtValueCode" runat="server"
                                               OnChange="return ValueCodetextChange()" onKeyUp="javascript:alphanumericOnly(this);" MaxLength="2"
                                                CssClass="formField"
                                                Style="height: 30px; width: 200px; margin-left:30px" />
                                             <asp:LinkButton ID="searchLink" runat="server" OnClientClick="return visibleValueCode()" Text="Search"></asp:LinkButton>
                                        <asp:Label ID="lblErrorValueCodePanel" runat="server" ForeColor="Red" Style="margin-left: 40px;"></asp:Label>
                                        </div>                                  

                                    </div>
                                </div>

                                <div class="col-sm-6 col-md-4 col-lg-3">
                                    <div style="text-align: left;">
                                       <asp:TextBox ID="txtAmount" runat="server" CssClass="formField" MaxLength="11" Style="height: 30px; width: 170px;" onkeypress="return onlyDotsAndNumbers(this,event);"/>
                                         <br /> <asp:Label ID="lblErrorAmount" runat="server" ForeColor="Red" ></asp:Label>                                   
                                    </div>
                                  
                                </div>

                               <div class="col-sm-6 col-md-4 col-lg-3">
                                    <div style="text-align: left;">
                                        <asp:Label ID="lblValueCodeDesc" runat="server" Style="font-size: 12px;
                                            text-align: justify;"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-sm-3" style="padding-left: 100px;">
                                    <asp:Button ID="btnAddValueCodeInformation" Text="ADD" runat="server" OnClientClick="return AddValueCode()"
                                        CssClass="btn btn-primary"  Font-Bold="True" Width="90px" />
                                  <br />   <asp:Button ID="btnEditValueCode" Text="Update" runat="server" OnClientClick="return UpdateValuecode()"
                                        CssClass="btn btn-primary" Style="visibility: hidden"  Font-Bold="True" Width="90px" />
                                    <asp:Button ID="btnCancelValueCode" Style="visibility: hidden" Text="Cancel" OnClientClick="return CancelValueCodeFields();"
                                            runat="server" CssClass="btn btn-danger" Font-Bold="True"  Width="70px" />
                                </div>
                            </div>              
                    </div>                    
                    <ajax:ModalPopupExtender BehaviorID="mpeValueCode" ID="mpeSubmitClaimSearchValueCode"
                        runat="server" PopupControlID="pnlSubmitClaimSearchValueCode"
                        TargetControlID="ButtonSearchValueCode" BackgroundCssClass="modalBackground"
                        CancelControlID="btnCloseValueCode" />
                </div>
                <asp:Panel ID="pnlSubmitClaimSearchValueCode" runat="server" CssClass="modalPopup"
                    Style="display: none; min-height: 350px; min-width: 1500px; height: auto; width: auto;">

                    <asp:Panel ID="pnlSubmitClaimSearchValueCodeHeader" CssClass="popHeader popUpHeader"
                        runat="server"
                        HorizontalAlign="Left" Style="height: auto; width: auto; min-height: 40px; min-width: 100px;" >
                        <asp:Button runat="server" ID="btnCloseValueCode" CssClass="popUpClose" Text="X"
                            CausesValidation="false" />
                            <div class="search-Results">
                <span style="padding-left: 110px; font-size: 17px;">VALUE CODE</span>
                <span style="padding-left: 140px; font-size: 17px;">VALUE CODE DESCRIPTION</span>
            
                        </div>
                    </asp:Panel>

                    <div class="container-fluid">                       

                         <asp:Panel ID="pnlValueCodeInfoSearch" runat="server">
                               
                                     <div>
                                            <asp:Label ID="fieldRequireError" runat="server" ForeColor="Red"></asp:Label>
                                        </div>
                                    <div class="row m-0 popUpSearch-Context d-FlexCenter">                                       
                                        <div class="col-sm-6 col-md-4 col-lg-2 ">
                                            <span class="ohio-field-label" style="padding-left:10px;font-size:17px;">
                                                <asp:TextBox MaxLength="2" ID="txtCode" CssClass="ohio-field-input" runat="server"></asp:TextBox>
                                            </span>
                                        </div>
                                        <div class="col-sm-6 col-md-4 col-lg-8 ">
                                            <span class="ohio-field-label">
                                                <asp:TextBox MaxLength="100" ID="txtValueDesc" CssClass="ohio-field-input" runat="server">
                                                </asp:TextBox>
                                            </span>
                                        </div>
                                        <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="col-sm-6 col-md-4 col-lg-3 " style="padding-top: 12px;">
                            <asp:Button ID="btnSearch2" OnClientClick="return GetValueCodeDetails()" runat="server" CssClass="buttonBox StepButton buttonBoxFocus"  Text="Search" CausesValidation="false" />



                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
       
        <asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>
       
         <div class="search-Results">SEARCH RESULTS</div>
                <asp:Label ID="Label1" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>

        <div class="result-Container">
             <div class="popupGridViewOnSearch">
                <mms:SortablePagingGridView
                    ID="gvValueCodeSearchPop"
                    runat="server"
                    AutoGenerateColumns="False"
                    CssClass="gridViewSmallFont" Width="100%"
                    AllowSorting="true"
                    EmptyDataText="No Providers found."
                    OnPageIndexChanging="gvValueCodeSearchPop_PageIndexChanging"
                    OnSorting="gvValueCodeSearchPop_Sorting"
                    RowStyle-VerticalAlign="Top"
                    AllowPaging="True"
                    PageSize="15"
                    GridViewSortColumn="Code" GridViewSortDirection="Ascending"
                    DataKeyNames="CLAIMS_VALUE_CODE, CLAIMS_VALUE_CODE_DESC">
                    <Columns>

                    
                    </Columns>
                </mms:SortablePagingGridView>
                 </div>
        </div>
                                <asp:HiddenField ID="hdnValueCode" runat="server" />

                                <asp:HiddenField ID="hdnValueCodeDesc" runat="server" />
                                <asp:HiddenField ID="hdnValueCodeClaimStatus" runat="server" />


                           </asp:Panel>
                    </div>
                   
                </asp:Panel>
                <asp:Button runat="server" ID="ButtonSearchValueCode" Style="display: none" Text="ButtonSearchValueCode" />
        </asp:Panel>
    


<asp:HiddenField ID="hdnValueCode_ClaimID" runat="server" />
<asp:HiddenField ID="hdnValueCode_ClaimType" runat="server" />
<asp:HiddenField ID="hdnValue_Code" runat="server" />
<asp:HiddenField ID="hdnLine" runat="server" />
