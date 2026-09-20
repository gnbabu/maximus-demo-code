<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ConditionCodeInformation, App_Web_wbqq1lcm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/ConditionCodeSearch.ascx" TagPrefix="uc" TagName="ConditionCodeSearch" %>
 <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">

    
<script lang="javaScript">
    var TotalCountinConditionCodegrid;
    function CancelConditionCodeFields() {
        $("#<%= txtConditionCode.ClientID %>").val('');
        $("#<%= lblConditionDescription.ClientID %>").html('');
        $("[id*=lblConditionCodemessage]").text('');
        if (TotalCountinConditionCodegrid >= 24) {
            document.getElementById('<%= btnConditionCodeAdd.ClientID%>').style.visibility = "hidden";
        }
        else {
            document.getElementById('<%= btnConditionCodeAdd.ClientID%>').style.visibility = "visible";
        }
        document.getElementById('<%= btnCancelConditionCode.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnEditConditionCode.ClientID%>').style.visibility = "hidden";
        var table = localStorage.getItem("ConditionTable");
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= divConditionCodeInformation.ClientID %>').innerHTML = table;
        }
        return false;
    }
    function EditConditioncode(conditioncodeInfoID, ClaimID,control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);  
        var claimConditionCodeList;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetConditionCode?conditioncodeInfoID=" + conditioncodeInfoID + "&&ClaimID=" + ClaimID+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                claimConditionCodeList = result;
                if (claimConditionCodeList.length > 0) {
                    for (var i = 0; i < claimConditionCodeList.length; i++) {
                        var Line = result[i].Line;
                        var ConditionCode = result[i].ConditionCode;
                        var ConditionDescription = result[i].ConditionCodeDesc;
                        var ClaimID = result[i].ClaimID;
                        var Claims_Condition_Code_Information_ID = result[i].hdnConditionCode;
                        $("[id*=txtConditionCode]").val("" + ConditionCode + "");
                        $("[id*=hdnConditionClaimID]").val("" + ClaimID + "");
                        $("[id*=hdnConditionCode]").val("" + Claims_Condition_Code_Information_ID + "");
                        $("[id*=hdnLine]").val("" + Line + "");
                        document.getElementById('<%= lblConditionDescription.ClientID%>').innerHTML = ConditionDescription;
                        document.getElementById('<%= btnConditionCodeAdd.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnEditConditionCode.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnCancelConditionCode.ClientID%>').style.visibility = "visible";
                        return false;
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblConditionCodemessage]").text('Condition Code is invalid');
                ClearCondtionCodeFields();
            }
        });


    }
    function UpdateConditioncode() {
        var validateConditiondetail = true;
        var ClaimID = $("#<%= hdnConditionClaimID.ClientID %>").first().val();
        var Claims_Condition_Code_Information_ID = $("#<%= hdnConditionCode.ClientID %>").first().val();
        var ConditionCode = $("#<%= txtConditionCode.ClientID %>").first().val();
        var ConditionDescription = document.getElementById('<%= lblConditionDescription.ClientID %>').innerHTML;
        var Line = $("#<%= hdnLine.ClientID %>").first().val();
        var claimConditionCodeData;
        validateConditiondetails();
        if (validateConditiondetail === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "PUT",
                url: webApiClaims + "EditConditionCodeData?ClaimID=" + ClaimID + "&&Claims_Condition_Code_Information_ID=" + Claims_Condition_Code_Information_ID + "&&ConditionCode=" + ConditionCode + "&&ConditionCodeDesc=" + ConditionDescription + "&&Line=" + Line + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimConditionCodeData = result;
                    var totalCount = 0;
                    var Error = null;
                    if (claimConditionCodeData.length > 0) {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Condition Code</th><th style='width:10px; scope='col'>Condition Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        for (var i = 0; i < claimConditionCodeData.length; i++) {
                            if (result[i].ErrorMsg != null) {
                                Error = result[i].ErrorMsg;
                            }
                        }
                        if (Error == null || Error == undefined || Error == "") {
                            for (var i = 0; i < claimConditionCodeData.length; i++) {
                                totalCount++;
                                var sequence = i;
                                sequence++;

                                var ConditionCode = result[i].ConditionCode;
                                var ConditionDescription = result[i].ConditionCodeDesc;
                                var Claims_Condition_Code_Information_ID = result[i].hdnConditionCode;
                                var ClaimID = result[i].ClaimID;

                                TotalCountinConditionCodegrid = totalCount;
                                if (totalCount >= 24) { document.getElementById('<%= btnConditionCodeAdd.ClientID%>').style.visibility = "hidden"; }
                                else { document.getElementById('<%= btnConditionCodeAdd.ClientID%>').style.visibility = "visible"; }
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span  title='Condition Code' class='tNumber'>" + ConditionCode + "</span></td><td><span title='Condition Description' class='tNumber'>" + ConditionDescription + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditConditioncode(\"" + Claims_Condition_Code_Information_ID + "\",\"" + ClaimID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteConditioncode(\"" + Claims_Condition_Code_Information_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                            };
                            table = table + "</tbody></table>";
                            document.getElementById('<%= divConditionCodeInformation.ClientID %>').innerHTML = table;
                            localStorage.setItem("ConditionTable", "" + table + "");
                            document.getElementById('<%= btnConditionCodeAdd.ClientID%>').style.visibility = "visible";
                            document.getElementById('<%= btnEditConditionCode.ClientID%>').style.visibility = "hidden";
                            document.getElementById('<%= btnCancelConditionCode.ClientID%>').style.visibility = "hidden";
                            ClearCondtionCodeFields();

                        }
                        else {
                            $("[id*=lblConditionCodemessage]").text('Duplicate Condition Code is not allowed. ');
                            return false;
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblConditionCodemessage]").text('Condition Code not found.');
                }
            });
            ClearCondtionCodeFields();
        }
        function validateConditiondetails() {
            if (ConditionCode == "") {
                validateConditiondetail = "false";
                $("[id*=lblConditionCodemessage]").text('Condition code is required');
                return false;
            }
            else {
                validateConditiondetail = "true";
                $("[id*=lblConditionCodemessage]").text('');
            }


        }
        return false;

    }
    function DeleteConditioncode(Claims_Condition_Code_Information_ID) {
        
        var claimConditionCodeDat;
        var result = confirm("Are you sure you want to delete?");
        if (result) {
            var ClaimID = $("#<%= hdnConditionClaimID.ClientID %>").first().val();
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteConditionCodeData?ClaimID=" + ClaimID + "&&Claims_Condition_Code_Information_ID=" + Claims_Condition_Code_Information_ID+"",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimConditionCodeDat = result;
                    if (claimConditionCodeDat.length > 0) {
                        var table;
                        var totalCount = 0;
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Condition Code</th><th style='width:10px; scope='col'>Condition Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                      for (var i = 0; i < claimConditionCodeDat.length; i++) {
                          totalCount++;
                          var Line = result[i].Line;
                          var ConditionCode = result[i].ConditionCode;
                          var ConditionDescription = result[i].ConditionCodeDesc;
                          var ClaimID = result[i].ClaimID;
                          var Claims_Condition_Code_Information_ID = result[i].hdnConditionCode;
                          $("[id*=hdnLine]").val("" + Line + "");

                          TotalCountinConditionCodegrid = totalCount;
                          if (totalCount >= 24) { document.getElementById('<%= btnConditionCodeAdd.ClientID%>').style.visibility = "hidden"; }
                          else { document.getElementById('<%= btnConditionCodeAdd.ClientID%>').style.visibility = "visible"; }
                          table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span  title='Condition Code' class='tNumber'>" + ConditionCode + "</span></td><td><span title='Condition Description' class='tNumber'>" + ConditionDescription + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditConditioncode(\"" + Claims_Condition_Code_Information_ID + "\",\"" + ClaimID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteConditioncode(\"" + Claims_Condition_Code_Information_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        };
                        table = table + "</tbody></table>";
                        document.getElementById('<%= divConditionCodeInformation.ClientID %>').innerHTML = table;
                        localStorage.setItem("ConditionTable", "" + table + "");
                        ClearCondtionCodeFields();
                        document.getElementById('<%= btnConditionCodeAdd.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnEditConditionCode.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnCancelConditionCode.ClientID%>').style.visibility = "hidden";
                        return false;

                    }
                    else {
                        document.getElementById('<%= divConditionCodeInformation.ClientID %>').innerHTML = "";
                        ClearCondtionCodeFields();
                        document.getElementById('<%= btnConditionCodeAdd.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnEditConditionCode.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnCancelConditionCode.ClientID%>').style.visibility = "hidden";
                       return false;
                   }
               },
               error: function (jqXHR, textStatus, errorThrown) {
                   $("[id*=lblConditionCodemessage]").text('Condition Code not found.');
                   ClearCondtionCodeFields();
               }
           });

        }

    }

    function ClearCondtionCodeFields() {
        
        $("#<%= txtConditionCode.ClientID %>").val('');
        $("#<%= lblConditionDescription.ClientID %>").html('');
        $("[id*=lblConditionCodemessage]").text('');
    }

    function AddConditionCode() {
        var validateConditiondetail = true;
        var claimConditioncodeData;
        var ConditionCode = $("#<%= txtConditionCode.ClientID %>").first().val();
        var ConditionDescription = $("#<%= lblConditionDescription.ClientID %>").first().val();
        var ClaimID = $("#<%= hdnConditionClaimID.ClientID %>").first().val();
        var hdnConditionCode = $("#<%= hdnConditionCode.ClientID %>").first().val();        
        /*if (ConditionCode.length != 0) {*/
            validateConditiondetails();
        if (validateConditiondetail === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "POST",
                    url: webApiClaims + "AddConditionCodeData?ClaimID=" + ClaimID + "&&ConditionCode=" + ConditionCode + "&&ConditionDescription=" + ConditionDescription + "&&hdnConditionCode=" + hdnConditionCode + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                    headers: {
                        "Access-Control-Allow-Origin": "*",
                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                        "Authorization": "Bearer " + APIToken
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {                        
                        claimConditioncodeData = result;
                        var table;
                        var totalCount = 0;
                        var Error = null;
                        if (claimConditioncodeData.length > 0) {

                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Condition Code</th><th style='width:10px; scope='col'>Condition Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";                            
                            for (var i = 0; i < claimConditioncodeData.length; i++) {
                                if (result[i].ErrorMsg != null) {
                                    Error = result[i].ErrorMsg;
                                }
                            }
                            if (Error == null || Error == undefined || Error == "") {                                
                                for (var i = 0; i < claimConditioncodeData.length; i++) {
                                    totalCount++;
                                    var Line = result[i].Line;
                                    var ConditionCode = result[i].ConditionCode;
                                    var ConditionDescription = result[i].ConditionCodeDesc;
                                    var ClaimID = result[i].ClaimID;
                                    var Claims_Condition_Code_Information_ID = result[i].hdnConditionCode;
                                    $("[id*=hdnLine]").val("" + Line + "");

                                    TotalCountinConditionCodegrid = totalCount;
                                    if (totalCount >= 24) { document.getElementById('<%= btnConditionCodeAdd.ClientID%>').style.visibility = "hidden"; }
                                    else { document.getElementById('<%= btnConditionCodeAdd.ClientID%>').style.visibility = "visible"; }
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span  title='Condition Code' class='tNumber'>" + ConditionCode + "</span></td><td><span title='Condition Description' class='tNumber'>" + ConditionDescription + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditConditioncode(\"" + Claims_Condition_Code_Information_ID + "\",\"" + ClaimID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteConditioncode(\"" + Claims_Condition_Code_Information_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                                };
                                table = table + "</tbody></table>";
                                localStorage.setItem("ConditionTable", "" + table + "");

                                document.getElementById('<%= divConditionCodeInformation.ClientID %>').innerHTML = table;
                                ClearCondtionCodeFields();
                            }
                            else {
                                $("[id*=lblConditionCodemessage]").text('Duplicate Condition Code is not allowed. ');
                                return false;
                            }
                        }

                        else {
                            ClearCondtionCodeFields();
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        $("[id*=lblConditionCodemessage]").text('Condition Code not found.');
                        ClearCondtionCodeFields();
                    }
                });
                ClearCondtionCodeFields();
            }


            function validateConditiondetails() {
                if (ConditionCode == "") {
                    validateConditiondetail = "false";
                    $("[id*=lblConditionCodemessage]").text('Condition code is required');
                    return false;
                }
                else {
                    validateConditiondetail = "true";
                    $("[id*=lblConditionCodemessage]").text('');
                }


            }
        //}
        return false;
    }
   
    function visibleConditionCode() {
        $("#<%=ucConditionCodeSearch.FindControl("gvConditionCodeSearch").ClientID %>").html("");
        $("#<%=ucConditionCodeSearch.FindControl("txtConditonCode").ClientID %>").val("");
        $("#<%=ucConditionCodeSearch.FindControl("txtConditionCodeDesc").ClientID %>").val("");
        localStorage.setItem("indexconditioncode", "");
        $find("mpeConditionCode").show();
        return false;

    }

    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9- ]/g, '');
    }

    function ConditionCodetextChange() {
        $("[id*=lblConditionDescription]").text("");
        $("[id*=lblConditionCodemessage]").text('');
        var txtConditionCode = $("#<%= txtConditionCode.ClientID %>").first().val();
        if (txtConditionCode.length != 0) {
            if (txtConditionCode.length > 2) {
                $("[id*=lblConditionDescription]").text("");
                $("[id*=lblConditionCodemessage]").text('Condition code not found.');
            }
            else {
                var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "GET",
                    url: webApiClaims + "GetClaimConditionCodeDescription?desc=" + txtConditionCode,
                    //data: '{desc: "' + txtConditionCode + '" }',
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
                            $("[id*=lblConditionDescription]").text("");
                            $("[id*=lblConditionCodemessage]").text('Condition code not found.');
                        }
                        else {
                            $("[id*=lblConditionDescription]").text(result[0]["CLAIMS_CONDITION_CODE_DESCRIPTION"]);
                            $("[id*=lblConditionCodemessage]").text("");
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        $("[id*=lblConditionCodemessage]").text('Condition code not found.');
                    }
                });
            }
        }
        
    }

    function gidViewConditionCodetextChange(lnk) {

        var gridindexconditioncode = lnk.parentNode.parentNode.rowIndex;
        var grid = ""; <%--document.getElementById("<%= gvConditionCodeInfo.ClientID%>");--%>
        var inputs = grid.rows[gridindexconditioncode].getElementsByTagName("INPUT");


        var txtSpanCode = inputs[1].value;
        var APIToken = $("[id*=hdnAccessToken]").val();
    $.ajax({
        type: "GET",
        url: webApiClaims + "GetClaimConditionCodeDescription?desc=" + txtSpanCode,
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
                    .find("input[type=text][id*=lblConditionCodeDesc]").text("");
                $("[id*=lblConditionCodemessage]").text('Condition code not found.');
            }
            else {
                var row = $(lnk).closest("tr");
                row.find("[id*=lblConditionCodeDesc]").text(result[0]["CLAIMS_CONDITION_CODE_DESCRIPTION"]);
                $("[id*=lblConditionCodemessage]").text("");
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $("[id*=lblConditionCodemessage]").text('Condition code not found.');
        }
    });

    }   

    function GetConditionCodeDetails() {
        var lblSearchResult = $("#<%= ucConditionCodeSearch.FindControl("fieldRequireError").ClientID %>").first();
        lblSearchResult.text('');
        var txtConditionCode = $("#<%= ucConditionCodeSearch.FindControl("txtConditonCode").ClientID %>").first().val();
         var txtConditioncodedesc = $("#<%= ucConditionCodeSearch.FindControl("txtConditionCodeDesc").ClientID %>").first().val();
        $("#<%=ucConditionCodeSearch.FindControl("gvConditionCodeSearch").ClientID %>").html("");
        if ((txtConditionCode == "" || txtConditionCode == null || txtConditionCode == undefined) && (txtConditioncodedesc == "" || txtConditioncodedesc == null || txtConditioncodedesc == undefined)) {
            lblSearchResult.text('Condition Code or Condition Code Description is required.');
            return false;
        }
        var APIToken = $("[id*=hdnAccessToken]").val();
         $.ajax({
             type: "GET",
             url: webApiClaims + "GetConditionCodeDetails?desc=" + txtConditioncodedesc + "&&val=" + txtConditionCode,
             //data: '{desc: "' + txtConditioncodedesc + '" , val: "' + txtConditionCode + '" }',
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

                     $("#<%=ucConditionCodeSearch.FindControl("gvConditionCodeSearch").ClientID %>").append("<tr><td> No Records Found </td></tr>");
                  }
                  else {
                     $("#<%=ucConditionCodeSearch.FindControl("gvConditionCodeSearch").ClientID %>").append("<tr><th>CONDITION CODE </th><th>CONDITION CODE DESCRIPTION </th></tr>");
                      for (var i = 0; i < result.length; i++) {
                          $("#<%=ucConditionCodeSearch.FindControl("gvConditionCodeSearch").ClientID %>").append("<tr><td><a onClick='GetSelectedRowConditioncode(this); return false;'>" + result[i].condition_Code + "</a></td><td>" + result[i].condition_Desc + "</td></tr>");

                     };
                 }
             },
             error: function (jqXHR, textStatus, errorThrown) {
                 $("[id*=lblConditionCodemessage]").text('Condition code not found.');
             }
         });
          return false;
    }

    function GetSelectedRowConditioncode(lnk) {

        $find("mpeConditionCode").hide();

        var gridindexconditioncode = localStorage.getItem("indexconditioncode");
        
        if(!(gridindexconditioncode == "")) {

            var row = lnk.parentNode.parentNode;
            var grid = ""; <%--document.getElementById("<%= gvConditionCodeInfo.ClientID%>");--%>
            var inputs = grid.rows[gridindexconditioncode].getElementsByTagName("INPUT");
            inputs[1].value = row.cells[0].innerText;
            grid.rows[gridindexconditioncode].cells[3].innerText = row.cells[1].innerHTML;
            return false;
        }
        else {
            var textboxrow = lnk.parentNode.parentNode;
            $("[id*=txtConditionCode]").val(textboxrow.cells[0].innerText.trim());
            $("[id*=lblConditionDescription]").html(textboxrow.cells[1].innerHTML);
            return false;
        }
    }

    function visibleConditionCodeGridView(lnk) {
        $("#<%=ucConditionCodeSearch.FindControl("gvConditionCodeSearch").ClientID %>").html("");
        $("#<%=ucConditionCodeSearch.FindControl("txtConditonCode").ClientID %>").val("");
        $("#<%=ucConditionCodeSearch.FindControl("txtConditionCodeDesc").ClientID %>").val("");
        
        var index = lnk.parentNode.parentNode.rowIndex;
        localStorage.setItem("indexconditioncode", index);

        $find("mpeConditionCode").show();
        return false;

    }

</script>
<div class="row">
    <div class="divGrid">
        <div id="divConditionCodeInformation" runat="server"></div>      
    </div>
</div>
<div id="divcon" runat="server">
<div class="row">
    <div class="col-md-1">
    </div>
    <div class="col-md-3">
        <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color:red">*</span>Condition
            Code</span>
    </div>
    <div class="col-md-6">
        <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">Condition
            Description</span>
    </div>
    <div class="col-md-2">
    </div>
</div>

<div class="row">
    <div class="col-sm-1">
    </div>
    <div class="col-sm-3">
        <div style="text-align: left;">
            <asp:TextBox ID="txtConditionCode" runat="server"  CssClass="formFieldTextBox"
                MaxLength="2" Style="height: 30px; width: 150px" onChange="return ConditionCodetextChange(this)" onKeyUp="javascript:alphanumericOnly(this);"/>
            <asp:LinkButton ID="lnkConditionCodeSearch" Style="font-size: 14px;" runat="server"
                Text="Search" ToolTip="Search" OnClientClick="return visibleConditionCode()"
                Visible="true" CausesValidation="false"></asp:LinkButton>&nbsp;&nbsp;<br />            
            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtConditionCode" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />           
            <asp:Label ID="lblConditionCodemessage" runat="server" ForeColor="Red" ></asp:Label>
        </div>
    </div>
    <div class="col-sm-5">
        <div style="text-align: left;">
            <asp:Label ID="lblConditionDescription" runat="server" Style="height: 30px; max-width: auto" />
        </div>
    </div>
    <div class="col-sm-3" style="padding-left: 80px;">
        <asp:Button ID="btnConditionCodeAdd" Text="Add" OnClientClick="return AddConditionCode()" runat="server" 
            CssClass="btn btn-primary" Font-Bold="True" Width="60px" Height="30px" />
       <br /> <asp:Button ID="btnEditConditionCode" Text="Update" OnClientClick="return UpdateConditioncode()" runat="server" 
            CssClass="btn btn-primary" Font-Bold="True"  Style="visibility:hidden" Width="60px" Height="30px" />
         <asp:Button ID="btnCancelConditionCode" Style="visibility: hidden" Text="Cancel" OnClientClick="return CancelConditionCodeFields();"
             runat="server" CssClass="btn btn-danger" Font-Bold="True"  Width="70px" />
    </div>
</div>
</div>
<ajax:ModalPopupExtender BehaviorID="mpeConditionCode" ID="mpeConditionCodeSearch"
    runat="server" PopupControlID="pnlConditionCodeSearch"
    TargetControlID="ButtonConditionCodeSearch" BackgroundCssClass="modalBackground"
    CancelControlID="btnCloseConditionCode" />
<asp:Panel ID="pnlConditionCodeSearch" runat="server" CssClass="modalPopup" Style="display: none;
    min-height: 350px; min-width: 1500px; height: auto; width: auto;">
    <asp:Panel ID="pnlConditionCodeSearchHeader" runat="server" HorizontalAlign="Left" Style="height: auto; width: auto; min-height: 40px; min-width: 100px;">
        <asp:Button runat="server" ID="btnCloseConditionCode" Text="X" Style="float: right;
            background-image: none; border: 0px; border-radius: 0px;"
            CausesValidation="false" />
        <div class="search-Results">
            <span style="padding-left: 60px">CONDITION CODE</span>
            <span style="padding-left: 200px">CONDITION CODE DESCRIPTION</span>
        </div>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server">
        <uc:ConditionCodeSearch runat="server" ID="ucConditionCodeSearch" Visible="true"
            EnableViewState="true" />
    </asp:Panel>
</asp:Panel>
<asp:Button runat="server" ID="ButtonConditionCodeSearch" Style="display: none" Text="ButtonConditionCodeSearch" />


<asp:HiddenField ID="hdnConditionClaimID" runat="server" />
<asp:HiddenField ID="hdnConditionCodeClaimType" runat="server" />
<asp:HiddenField ID="hdnConditionCode" runat="server" />
<asp:HiddenField ID="hdnConditionCodeDesc" runat="server" />
<asp:HiddenField ID="hdnLine" runat="server" />


