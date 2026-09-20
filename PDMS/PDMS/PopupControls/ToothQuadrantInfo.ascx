<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ToothQuadrantInfo" Codebehind="ToothQuadrantInfo.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<meta name="viewport" content="width=device-width, initial-scale=1">
<script type="text/javascript">
    
    function bindtoothsurfacedata() {

        var hdnClaimId = $("#<%= hdnClaimId.ClientID %>").first().val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetToothQuadrtantPanelInfodata?hdnClaimId=" + hdnClaimId+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var ToothQuadrantInfoList = result;
                var table = "";
                if (ToothQuadrantInfoList.length > 0) {
                    table = "<table class='gridview' cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Tooth Number</th > <th style='width:10px; scope=' col'> Tooth Surface</th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th ><th style='width: 10px; ' scope='col'>&nbsp;</th><th style='width: 10px; ' scope='col'>&nbsp;</th></tr > ";
                    for (var i = 0; i < ToothQuadrantInfoList.length; i++) {
                        var j = 0;
                        var ToothServiceLine = result[i].ToothServiceLine;

                        var ToothNumber = result[i].ToothNumber;
                        var ToothSurface1 = result[i].ToothSurface1;
                        var ToothSurface2 = result[i].ToothSurface2;
                        var ToothSurface3 = result[i].ToothSurface3;
                        var ToothSurface4 = result[i].ToothSurface4;
                        var ToothSurface5 = result[i].ToothSurface5;
                        var Claims_Tooth_and_Surface_Information_ID = result[i].Claims_Tooth_and_Surface_Information_ID;
                        $("#<%=ddlToothServiceLine.ClientID %> option[value='" + ToothServiceLine + "']").remove();
                            j++;
                            if (j >= 50) { document.getElementById('<%= btnAdd.ClientID%>').style.visibility = "hidden"; }
                            else { document.getElementById('<%= btnAdd.ClientID%>').style.visibility = "visible"; }
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + ToothServiceLine + "</span></td><td><span title='Line' class='tNumber'>" + ToothNumber + "</span></td><td><span  title='Line' class='tNumber'>" + ToothSurface1 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface2 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface3 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface4 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface5 + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditToothQuadrantInfoLineItem(\"" + Claims_Tooth_and_Surface_Information_ID + "\",\"" + hdnClaimId + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteToothQuadrantInfoItem(\"" + Claims_Tooth_and_Surface_Information_ID + "\",\"" + ToothServiceLine + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr > ";
                        };
                        table = table + "</tbody></table>";
                        localStorage.setItem("ToothQuadrantInfoTable", "" + table + "");
                        document.getElementById('<%= ToothQuadrantInfoOutput.ClientID %>').innerHTML = table;
                        ClearFieldsTooth();
                        return false;
                    }
                    else {
                        document.getElementById('<%= ToothQuadrantInfoOutput.ClientID %>').innerHTML = "";
                        document.getElementById('<%= btnAdd.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden"; 
                        document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "hidden";

                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblToothSurfaceError]").text('Tooth code not found(1).');
                }
            });


    }

    function clearToothQuadrantInfoTableContent() {

        document.getElementById('<%= ToothQuadrantInfoOutput.ClientID %>').innerHTML = "";

    }
    function toothdisplaytable() {
        var abc = localStorage.getItem("ToothQuadrantInfoTable");

        if (abc != null && abc != undefined && abc != "") {
            document.getElementById('<%= ToothQuadrantInfoOutput.ClientID %>').innerHTML = abc;

        }
    };

    
    function loadTooth() {

        
<%--        document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';--%>

    }
    <%--function loadAddTooth() {

        disableFields();
        document.getElementById('<%= btnAddToothQuadrantInfo.ClientID %>').style.display = 'none';
        document.getElementById('<%= btnloading1.ClientID %>').style.display = 'block'; 

    }--%>
    function loadAddToothddl() {
       
        <%--document.getElementById('<%= btnloading2.ClientID %>').style.display = 'block';--%>
    }

    function GetToothSurfaceOnEdit() {

         $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface1").html("");
         $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface2").html("");
         $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface3").html("");
         $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface4").html("");
         $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5").html("");
         var newOption = "<option value=''></option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface1").append(newOption);
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface2").append(newOption);
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface3").append(newOption);
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface4").append(newOption);
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5").append(newOption);
        var APIToken = $("[id*=hdnAccessToken]").val();
         $.ajax({
             type: "GET",
             url: webApiClaims+"GetPriorToothSurfaceList",
             headers: {
                 "Access-Control-Allow-Origin": "*",
                 "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                 "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                 "Authorization": "Bearer " + APIToken
             },
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             success: function (result) {
                 var toothsurface = result;
                 if (toothsurface.length > 0) {
                     for (var i = 0; i < toothsurface.length; i++) {
                         var newOption = "<option value='" + result[i].TOOTHSURFACE_ID + "'>" + result[i].TOOTHSURFACE + "</option>";
                         $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface1").append(newOption);
                         $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface2").append(newOption);
                         $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface3").append(newOption);
                         $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface4").append(newOption);
                         $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5").append(newOption);
                     };
                    
                 }
             },

             error: function (jqXHR, textStatus, errorThrown) {
                 //  $("[id*=lblToothSurfaceError]").text('Tooth code not found.');
             }
         });

     }

    function GetToothSurface() {

        //disableFields();
       <%-- document.getElementById('<%= ddlToothSurface1.ClientID %>').disabled = false;
        document.getElementById('<%= ddlToothSurface2.ClientID %>').disabled = false;
        document.getElementById('<%= ddlToothSurface3.ClientID %>').disabled = false;
        document.getElementById('<%= ddlToothSurface4.ClientID %>').disabled = false;
        document.getElementById('<%= ddlToothSurface5.ClientID %>').disabled = false;--%>
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface1").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface2").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface3").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface4").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5").html("");
        var newOption = "<option value=''></option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface1").append(newOption);
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims+"GetPriorToothSurfaceList",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var toothsurface = result;
                if (toothsurface.length > 0) {
                    for (var i = 0; i < toothsurface.length; i++) {
                        var newOption = "<option value='" + result[i].TOOTHSURFACE_ID + "'>" + result[i].TOOTHSURFACE + "</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface1").append(newOption);
                    };
                    return false;
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                //  $("[id*=lblToothSurfaceError]").text('Tooth code not found.');
            }
        });

    }

    function GetServiceLine() {  
        
        var ClaimId = $("#<%= hdnClaimId.ClientID %>").first().val();        
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothServiceLine").html("");
        var newOption = "<option value=''></option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothServiceLine").append(newOption);
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            
            url: webApiClaims + "GetToothServiceLine?claimId=" + ClaimId+"",
            //data: '{claimId: "' + ClaimId + '"}',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (res) {
                
                var claimData = res;
                $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothServiceLine").empty();

                if (claimData.length > 0) {
                    
                    $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothServiceLine").append($("<option> </option>"));
                    for (var i = 0; i < claimData.length; i++) {
                        
                        var serviceline = res[i].Service_Line;

                        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothServiceLine").append($("<option></option>").val(serviceline).html(serviceline));
                    };
                }
                else {
                    
                    $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothServiceLine").empty();

                }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblmessage]").text('Value code not found.');
            }
        });

    }



    function GetToothSurface1() {

        //disableFields();
        var ToothSurface1 = $("#<%=ddlToothSurface1.ClientID %> option:selected").val();
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface2").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface3").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface4").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5").html("");
        var newOption = "<option value=''></option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface2").append(newOption);
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface3").append(newOption);
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface4").append(newOption);
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5").append(newOption);
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetToothSurface1List?ToothSurface1=" + ToothSurface1+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var toothsurface = result;
                if (toothsurface.length > 0) {
                    for (var i = 0; i < toothsurface.length; i++) {
                        var newOption = "<option value='" + result[i].TOOTHSURFACE_ID + "'>" + result[i].TOOTHSURFACE + "</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface2").append(newOption);
                    };
                    return false;
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                //  $("[id*=lblToothSurfaceError]").text('Tooth code not found.');
            }
        });

    }

    function GetToothSurface2() {

        //disableFields();
        var ToothSurface1 = $("#<%=ddlToothSurface1.ClientID %> option:selected").val();
        var ToothSurface2 = $("#<%=ddlToothSurface2.ClientID %> option:selected").val();

        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface3").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface4").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5").html("");
        var newOption = "<option value=''></option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface3").append(newOption);
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface4").append(newOption);
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5").append(newOption);
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetToothSurface2?ToothSurface1=" + ToothSurface1 + "&&ToothSurface2=" + ToothSurface2+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var toothsurface = result;
                if (toothsurface.length > 0) {
                    for (var i = 0; i < toothsurface.length; i++) {
                        var newOption = "<option value='" + result[i].TOOTHSURFACE_ID + "'>" + result[i].TOOTHSURFACE + "</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface3").append(newOption);
                    };
                    return false;
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                //  $("[id*=lblToothSurfaceError]").text('Tooth code not found.');
            }
        });

    }

    function GetToothSurface3() {

        //disableFields();
        var ToothSurface1 = $("#<%=ddlToothSurface1.ClientID %> option:selected").val();
        var ToothSurface2 = $("#<%=ddlToothSurface2.ClientID %> option:selected").val();
        var ToothSurface3 = $("#<%=ddlToothSurface3.ClientID %> option:selected").val();
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface4").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5").html("");

        var newOption = "<option value=''></option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface4").append(newOption);
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5").append(newOption);
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetToothSurface3?ToothSurface1=" + ToothSurface1 + "&&ToothSurface2=" + ToothSurface2 + "&&ToothSurface3=" + ToothSurface3+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var toothsurface = result;
                if (toothsurface.length > 0) {
                    for (var i = 0; i < toothsurface.length; i++) {
                        var newOption = "<option value='" + result[i].TOOTHSURFACE_ID + "'>" + result[i].TOOTHSURFACE + "</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface4").append(newOption);
                    };
                    return false;
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                //  $("[id*=lblToothSurfaceError]").text('Tooth code not found.');
            }
        });

    }

    function GetToothSurface4() {

        //disableFields();
        var ToothSurface1 = $("#<%=ddlToothSurface1.ClientID %> option:selected").val();
        var ToothSurface2 = $("#<%=ddlToothSurface2.ClientID %> option:selected").val();
        var ToothSurface3 = $("#<%=ddlToothSurface3.ClientID %> option:selected").val();
        var ToothSurface4 = $("#<%=ddlToothSurface4.ClientID %> option:selected").val();

        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5").html("");
        var newOption = "<option value=''></option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5").append(newOption);
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetToothSurface4?ToothSurface1=" + ToothSurface1 + "&&ToothSurface2=" + ToothSurface2 + "&&ToothSurface3=" + ToothSurface3 + "&&ToothSurface4=" + ToothSurface4+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var toothsurface = result;
                if (toothsurface.length > 0) {
                    for (var i = 0; i < toothsurface.length; i++) {
                        var newOption = "<option value='" + result[i].TOOTHSURFACE_ID + "'>" + result[i].TOOTHSURFACE + "</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5").append(newOption);
                    };
                    return false;
                }
            },

            error: function (jqXHR, textStatus, errorThrown) {
                //  $("[id*=lblToothSurfaceError]").text('Tooth code not found.');
            }
        });

    }

    function AddToothQuadrantInfo() {
        $("[id*=lblToothSurfaceError]").text('');
        var ToothNumber = $("#<%= txtToothNumber.ClientID %>").first().val();
        var ToothServiceLine = $("#<%=ddlToothServiceLine.ClientID %> option:selected").text();
        var ToothSurface1 = $("#<%=ddlToothSurface1.ClientID %> option:selected").val();
        var ToothSurface2 = $("#<%=ddlToothSurface2.ClientID %> option:selected").val();
        var ToothSurface3 = $("#<%=ddlToothSurface3.ClientID %> option:selected").val();
        var ToothSurface4 = $("#<%=ddlToothSurface4.ClientID %> option:selected").val();
        var ToothSurface5 = $("#<%=ddlToothSurface5.ClientID %> option:selected").val();
        var hdnClaimId = $("#<%= hdnClaimId.ClientID %>").first().val();

        var validate = "";
        if (hdnClaimId != null && hdnClaimId != "undefined" && hdnClaimId != "") {
            if (ToothServiceLine == null || ToothServiceLine == "" || ToothServiceLine == " " || ToothServiceLine == undefined) {
                $("[id*=lblToothSurfaceError]").text('*Service Line is required');
                validate = "false";
                return false;
            }
            else {
                validate = "true";
                $("#<%= lblToothSurfaceError.ClientID %>").html("");
            }
            if (ToothNumber == null || ToothNumber == "" || ToothNumber == undefined) {
                $("[id*=lblToothSurfaceError]").text('*Tooth Number is required');
                validate = "false";
                return false;
            }
            else {
                validate = "true";
                $("#<%= lblToothSurfaceError.ClientID %>").html("");
            }
            var duplicateCount = CheckDuplicates(ToothNumber, $("#<%=ddlToothSurface1.ClientID %> option:selected").text(), $("#<%=ddlToothSurface2.ClientID %> option:selected").text(), $("#<%=ddlToothSurface3.ClientID %> option:selected").text(), $("#<%=ddlToothSurface4.ClientID %> option:selected").text(), $("#<%=ddlToothSurface5.ClientID %> option:selected").text(),0);
            if (duplicateCount > 0) {
                $("[id*=lblToothSurfaceError]").text('*Please enter unique Toothnumber and ToothSurface');
                validate = "false";
                return false;
            }
            else {
                validate = "true";
                $("#<%= lblToothSurfaceError.ClientID %>").html("");
             }
            if (validate == "true") {
                var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "POST",
                    url: webApiClaims + "AddToothQuadrantInfo?hdnClaimId=" + hdnClaimId + "&&ToothNumber=" + ToothNumber + "&&ToothServiceLine=" + ToothServiceLine + "&&ToothSurface1=" + ToothSurface1 + "&&ToothSurface2=" + ToothSurface2 + "&&ToothSurface3=" + ToothSurface3 + "&&ToothSurface4=" + ToothSurface4 + "&&ToothSurface5=" + ToothSurface5 + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                    //data: '{hdnClaimId: "' + hdnClaimId + '",ToothNumber: "' + ToothNumber + '",ToothServiceLine: "' + ToothServiceLine + '",ToothSurface1: "' + ToothSurface1 + '",ToothSurface2: "' + ToothSurface2 + '",ToothSurface3: "' + ToothSurface3 + '",ToothSurface4: "' + ToothSurface4 + '",ToothSurface5: "' + ToothSurface5 + '"}',
                    headers: {
                        "Access-Control-Allow-Origin": "*",
                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                        "Authorization": "Bearer " + APIToken
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var ToothQuadrantInfoList = result;
                        
                        var table = "";
                        if (ToothQuadrantInfoList.length > 0) {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; margin-left: 10px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:20px; scope='col'>Service Line</th><th style='width:20px; scope='col'>Tooth Number</th > <th style='width:20px; scope=' col'> Tooth Surface</th > <th style='width: 20px; scope = ' col' ></th > <th style='width: 20px; scope = ' col' ></th > <th style='width: 20px; scope = ' col' ></th > <th style='width: 20px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > </tr > ";
                            for (var i = 0; i < ToothQuadrantInfoList.length; i++) {
                                var j = 0;
                                var ToothServiceLine = result[i].ToothServiceLine;                               
                                var ToothNumber = result[i].ToothNumber;
                                var ToothSurface1 = result[i].ToothSurface1;
                                var ToothSurface2 = result[i].ToothSurface2;
                                var ToothSurface3 = result[i].ToothSurface3;
                                var ToothSurface4 = result[i].ToothSurface4;
                                var ToothSurface5 = result[i].ToothSurface5;
                                var Claims_Tooth_and_Surface_Information_ID = result[i].Claims_Tooth_and_Surface_Information_ID;
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + ToothServiceLine + "</span></td><td><span title='Line' class='tNumber'>" + ToothNumber + "</span></td><td><span  title='Line' class='tNumber'>" + ToothSurface1 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface2 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface3 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface4 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface5 + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditToothQuadrantInfoLineItem(\"" + Claims_Tooth_and_Surface_Information_ID + "\",\"" + hdnClaimId + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteToothQuadrantInfoItem(\"" + Claims_Tooth_and_Surface_Information_ID + "\",\"" + ToothServiceLine + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr > ";
                                $("#<%=ddlToothServiceLine.ClientID %> option[value='" + ToothServiceLine + "']").remove();
                                j++;
                                if (j >= 50) { document.getElementById('<%= btnAdd.ClientID%>').style.visibility = "hidden"; }
                                else { document.getElementById('<%= btnAdd.ClientID%>').style.visibility = "visible"; }
                            };
                            table = table + "</tbody></table>";
                            localStorage.setItem("ToothQuadrantInfoTable", "" + table + "");
                            document.getElementById('<%= ToothQuadrantInfoOutput.ClientID %>').innerHTML = table;
                           
                           
                        }
                    },

                    error: function (jqXHR, textStatus, errorThrown) {
                        $("[id*=lblToothSurfaceError]").text('Tooth code not found(2).');
                    }
                });
            }

        }       
        GetToothSurface();       
        ClearFieldsTooth();       
        return false;

    }
    function CheckDuplicates(ToothNumber, ToothSurface1, ToothSurface2, ToothSurface3, ToothSurface4, ToothSurface5, Claims_Tooth_and_Surface_Information_ID) {
        var count = 0;
        var hdnClaimId = $("#<%= hdnClaimId.ClientID %>").first().val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            async: false,
            url: webApiClaims + "GetToothQuadrtantPanelInfodata?hdnClaimId=" + hdnClaimId + "",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var ToothQuadrantInfoList = result;
                if (ToothQuadrantInfoList.length > 0) {
                    for (var i = 0; i < ToothQuadrantInfoList.length; i++) {
                         if (result[i].ToothNumber == ToothNumber && result[i].ToothSurface1 == ToothSurface1 && result[i].ToothSurface2 == ToothSurface2
                             && result[i].ToothSurface3 == ToothSurface3 && result[i].ToothSurface4 == ToothSurface4
                             && result[i].ToothSurface5 == ToothSurface5) {
                             if (Claims_Tooth_and_Surface_Information_ID == 0 || (Claims_Tooth_and_Surface_Information_ID != 0 && Claims_Tooth_and_Surface_Information_ID != result[i].Claims_Tooth_and_Surface_Information_ID)) { 
                                 count = count + 1;
                              }
                         }
                    }; 
                }
                return count;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblToothSurfaceError]").text('Tooth code not found(1).');
            }
        });
        return count;
    }
    function ClearFieldsTooth() {
          $("#<%= txtToothNumber.ClientID %>").first().val("");        
        if ($('#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothServiceLine').children('option').length> 0) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothServiceLine").options[0].selected = true;
        }
        $("#<%=ddlToothSurface1.ClientID %> option:selected").text('');
        $("#<%=ddlToothSurface2.ClientID %> option:selected").text('');
        $("#<%=ddlToothSurface3.ClientID %> option:selected").text('');
        $("#<%=ddlToothSurface4.ClientID %> option:selected").text('');
        $("#<%=ddlToothSurface5.ClientID %> option:selected").text('');
             
    }

    function EditToothQuadrantInfoLineItem(Claims_Tooth_and_Surface_Information_ID, Claim_ID,control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        GetToothSurfaceOnEdit();
        $("[id*=lblToothSurfaceError]").text('');
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "Get_ToothQuadrantInfoDetails?Claim_ID=" + Claim_ID + "&&Claims_Tooth_and_Surface_Information_ID=" + Claims_Tooth_and_Surface_Information_ID+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {                
                var ToothQuadrantInfo = result;
                if (ToothQuadrantInfo.length > 0) {
                   
                    for (var i = 0; i < ToothQuadrantInfo.length; i++) {
                        debugger;
                        var hdnClaimId = result[i].hdnClaimId;
                        var Claims_Tooth_and_Surface_Information_ID = result[i].Claims_Tooth_and_Surface_Information_ID;
                        var ToothServiceLine = result[i].ToothServiceLine;
                        var ToothNumber = result[i].ToothNumber;
                        var ToothSurface1 = result[i].ToothSurface1;
                        var ToothSurface2 = result[i].ToothSurface2;
                        var ToothSurface3 = result[i].ToothSurface3;
                        var ToothSurface4 = result[i].ToothSurface4;
                        var ToothSurface5 = result[i].ToothSurface5;                        
                        var newOption = "<option value='" + ToothServiceLine + "'>" + ToothServiceLine + "</option>";                       
                        $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothServiceLine").append(newOption);                                        
                        $("[id*=Claims_Tooth_and_Surface_Information_ID]").val("" + Claims_Tooth_and_Surface_Information_ID + "");
                        $("[id*=hdnClaimId]").val("" + hdnClaimId + "");
                        $('#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_txtToothNumber').val(ToothNumber);                      
                        
                        $('#<%=ddlToothServiceLine.ClientID%>').val(ToothServiceLine);
                        if (ToothSurface1 != null && ToothSurface1 != "" && ToothSurface1 != undefined) {
                            var value = "";
                            if (ToothSurface1 == "B") { value = "1"; }
                            if (ToothSurface1 == "D") { value = "2"; }
                            if (ToothSurface1 == "F") { value = "3"; }
                            if (ToothSurface1 == "I") { value = "4"; }
                            if (ToothSurface1 == "L") { value = "5"; }
                            if (ToothSurface1 == "M") { value = "6"; }
                            if (ToothSurface1 == "O") { value = "7"; }
                            $('#<%=ddlToothSurface1.ClientID%>').val(value);
                            $('#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface2 > option[value=' + value + ']').remove();
                            $('#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface3 > option[value=' + value + ']').remove();
                            $('#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface4 > option[value=' + value + ']').remove();
                            $('#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5 > option[value=' + value + ']').remove();
                        } 
                        if (ToothSurface2 != null && ToothSurface2 != "" && ToothSurface2 != undefined) {
                            var value = "";
                            if (ToothSurface2 == "B") { value = "1"; }
                            if (ToothSurface2 == "D") { value = "2"; }
                            if (ToothSurface2 == "F") { value = "3"; }
                            if (ToothSurface2 == "I") { value = "4"; }
                            if (ToothSurface2 == "L") { value = "5"; }
                            if (ToothSurface2 == "M") { value = "6"; }
                            if (ToothSurface2 == "O") { value = "7"; }
                            $('#<%=ddlToothSurface2.ClientID%>').val(value);                            
                            $('#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface3 > option[value=' + value + ']').remove();
                            $('#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface4 > option[value=' + value + ']').remove();
                            $('#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5 > option[value=' + value + ']').remove();
                        } else { $('#<%=ddlToothSurface2.ClientID%>').val(""); }
                        if (ToothSurface3 != null && ToothSurface3 != "" && ToothSurface3 != undefined) {
                            var value = "";
                            if (ToothSurface3 == "B") { value = "1"; }
                            if (ToothSurface3 == "D") { value = "2"; }
                            if (ToothSurface3 == "F") { value = "3"; }
                            if (ToothSurface3 == "I") { value = "4"; }
                            if (ToothSurface3 == "L") { value = "5"; }
                            if (ToothSurface3 == "M") { value = "6"; }
                            if (ToothSurface3 == "O") { value = "7"; }
                            $('#<%=ddlToothSurface3.ClientID%>').val(value);                            
                            $('#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface4 > option[value=' + value + ']').remove();
                            $('#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5 > option[value=' + value + ']').remove();
                        } else { $('#<%=ddlToothSurface3.ClientID%>').val(""); }
                        if (ToothSurface4 != null && ToothSurface4 != "" && ToothSurface4 != undefined) {
                            var value = "";
                            if (ToothSurface4 == "B") { value = "1"; }
                            if (ToothSurface4 == "D") { value = "2"; }
                            if (ToothSurface4 == "F") { value = "3"; }
                            if (ToothSurface4 == "I") { value = "4"; }
                            if (ToothSurface4 == "L") { value = "5"; }
                            if (ToothSurface4 == "M") { value = "6"; }
                            if (ToothSurface4 == "O") { value = "7"; }
                            $('#<%=ddlToothSurface4.ClientID%>').val(value);  
                            $('#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothSurface5 > option[value=' + value + ']').remove();
                        } else { $('#<%=ddlToothSurface4.ClientID%>').val(""); }
                        if (ToothSurface5 != null && ToothSurface5 != "" && ToothSurface5 != undefined) {
                            var value = "";
                            if (ToothSurface5 == "B") { value = "1"; }
                            if (ToothSurface5 == "D") { value = "2"; }
                            if (ToothSurface5 == "F") { value = "3"; }
                            if (ToothSurface5 == "I") { value = "4"; }
                            if (ToothSurface5 == "L") { value = "5"; }
                            if (ToothSurface5 == "M") { value = "6"; }
                            if (ToothSurface5 == "O") { value = "7"; }
                            $('#<%=ddlToothSurface5.ClientID%>').val(value);
                                } else {
                            $('#<%=ddlToothSurface5.ClientID%>').val("");
                        }                       
                      <%--  $("#<%=ddlToothServiceLine.ClientID %> option:selected").text(ToothServiceLine);--%>
                        document.getElementById('<%= btnAdd.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "visible";
                        return false;
                    };

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
              
                $("[id*=lblToothSurfaceError]").text('Tooth code not found(3).');
            }
        });
       // ClearFields();
        //GetServiceLine();
        //GetToothSurface();
       <%-- document.getElementById('<%= ddlToothServiceLine.ClientID %>').disabled = true;--%>
        return false;
    }

    function DeleteToothQuadrantInfoItem(Claims_Tooth_and_Surface_Information_ID,ToothServiceLine) {
        var result = confirm("Are you sure you want to delete?");

        if (result) {
            var hdnClaimId = $("#<%= hdnClaimId.ClientID %>").first().val();
            
            if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothServiceLine").options.length==0) {
                var newOption = "<option value=''></option>";
                $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothServiceLine").append(newOption);
            }
            var newOption = "<option value='" + ToothServiceLine + "'>" + ToothServiceLine + "</option>";
            $("#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_ddlToothServiceLine").append(newOption);
            $("[id*=lblToothSurfaceError]").text('');
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteToothQuadrantInfo?hdnClaimId=" + hdnClaimId + "&&Claims_Tooth_and_Surface_Information_ID=" + Claims_Tooth_and_Surface_Information_ID+"",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    var ToothQuadrantInfoList = result;
                    var table = "";
                    if (ToothQuadrantInfoList.length > 0) {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; margin-left: 10px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:20px; scope='col'>Service Line</th><th style='width:20px; scope='col'>Tooth Number</th > <th style='width:20px; scope=' col'> Tooth Surface</th > <th style='width: 20px; scope = ' col' ></th > <th style='width: 20px; scope = ' col' ></th > <th style='width: 20px; scope = ' col' ></th > <th style='width: 20px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > </tr > ";
                        for (var i = 0; i < ToothQuadrantInfoList.length; i++) {
                            var j = 0;
                            var ToothServiceLine = result[i].ToothServiceLine;
                           
                            var ToothNumber = result[i].ToothNumber;
                            var ToothSurface1 = result[i].ToothSurface1;
                            var ToothSurface2 = result[i].ToothSurface2;
                            var ToothSurface3 = result[i].ToothSurface3;
                            var ToothSurface4 = result[i].ToothSurface4;
                            var ToothSurface5 = result[i].ToothSurface5;
                            var Claims_Tooth_and_Surface_Information_ID = result[i].Claims_Tooth_and_Surface_Information_ID;
                            $("#<%=ddlToothServiceLine.ClientID %> option[value='" + ToothServiceLine + "']").remove();
                            j++;
                            if (j >= 50) { document.getElementById('<%= btnAdd.ClientID%>').style.visibility = "hidden"; }
                          else { document.getElementById('<%= btnAdd.ClientID%>').style.visibility = "visible"; }
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + ToothServiceLine + "</span></td><td><span title='Line' class='tNumber'>" + ToothNumber + "</span></td><td><span  title='Line' class='tNumber'>" + ToothSurface1 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface2 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface3 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface4 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface5 + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditToothQuadrantInfoLineItem(\"" + Claims_Tooth_and_Surface_Information_ID + "\",\"" + hdnClaimId + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteToothQuadrantInfoItem(\"" + Claims_Tooth_and_Surface_Information_ID + "\",\"" + ToothServiceLine + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr > ";
                        };
                        table = table + "</tbody></table>";
                        localStorage.setItem("ToothQuadrantInfoTable", "" + table + "");
                        document.getElementById('<%= ToothQuadrantInfoOutput.ClientID %>').innerHTML = table;
                        ClearFieldsTooth();
                        return false;
                    }
                    else {
                        document.getElementById('<%= ToothQuadrantInfoOutput.ClientID %>').innerHTML = "";
                        document.getElementById('<%= btnAdd.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden"; 
                        document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "hidden";

                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblToothSurfaceError]").text('Tooth code not found(4).');
                }
            });
        }
        document.getElementById('<%= btnAdd.ClientID%>').style.visibility = "visible";
        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "hidden";
        
       <%-- document.getElementById('<%= ddlToothServiceLine.ClientID %>').disabled = false;--%>
        GetToothSurface();
       /* GetServiceLine();*/
        ClearFieldsTooth();
        return false;
    }
    //CancelToothQuadrantInfo
    function CancelToothQuadrantInfo() {
        $("[id*=lblToothSurfaceError]").text('');
        ClearFieldsTooth();
        GetToothSurface();
       /* GetServiceLine();*/
        document.getElementById('<%= btnAdd.ClientID%>').style.visibility = "visible";
        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "hidden";
      <%--    document.getElementById('<%= ddlToothServiceLine.ClientID %>').disabled = false;--%> 
        toothdisplaytable();
        return false;

    }
    function UpdateToothQuadrantInfo() {
        $("[id*=lblToothSurfaceError]").text('');
        var Tooth_Number = $("#<%= txtToothNumber.ClientID %>").first().val();
        var ToothServiceLine = $("#<%=ddlToothServiceLine.ClientID %> option:selected").val();
        var Tooth_Surface1 = $("#<%=ddlToothSurface1.ClientID %> option:selected").val();
        var Tooth_Surface2 = $("#<%=ddlToothSurface2.ClientID %> option:selected").val();
        var Tooth_Surface3 = $("#<%=ddlToothSurface3.ClientID %> option:selected").val();
        var Tooth_Surface4 = $("#<%=ddlToothSurface4.ClientID %> option:selected").val();
        var Tooth_Surface5 = $("#<%=ddlToothSurface5.ClientID %> option:selected").val();
        var hdnClaimId = $("#<%= hdnClaimId.ClientID %>").first().val();
        var Claims_Tooth_and_Surface_Information_ID = $("#<%= Claims_Tooth_and_Surface_Information_ID.ClientID %>").first().val();
        var validate = "";
        if (hdnClaimId != null && hdnClaimId != "undefined" && hdnClaimId != "") {
            if (ToothServiceLine == null || ToothServiceLine == "" || ToothServiceLine == " " || ToothServiceLine == undefined) {
                $("[id*=lblToothSurfaceError]").text('*Service Line is required');
                validate = "false";
                return false;
            }
            else {
                validate = "true";
                $("#<%= lblToothSurfaceError.ClientID %>").html("");
             }
            if (Tooth_Number == null || Tooth_Number == "" || Tooth_Number == undefined) {
                $("[id*=lblToothSurfaceError]").text('*Tooth Number is required');
                validate = "false";
                return false;
            }
            else {
                validate = "true";
                $("#<%= lblToothSurfaceError.ClientID %>").html("");
            }
            var duplicateCount = CheckDuplicates(Tooth_Number, $("#<%=ddlToothSurface1.ClientID %> option:selected").text(), $("#<%=ddlToothSurface2.ClientID %> option:selected").text(), $("#<%=ddlToothSurface3.ClientID %> option:selected").text(), $("#<%=ddlToothSurface4.ClientID %> option:selected").text(), $("#<%=ddlToothSurface5.ClientID %> option:selected").text(), Claims_Tooth_and_Surface_Information_ID);
            if (duplicateCount > 0) {
                $("[id*=lblToothSurfaceError]").text('*Please enter unique Toothnumber and ToothSurface');
                validate = "false";
                return false;
            }
            else {
                validate = "true";
                $("#<%= lblToothSurfaceError.ClientID %>").html("");
            }
            if (validate == "true") {
                var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "POST",
                    url: webApiClaims + "UpdateToothQuadrantInfo?hdnClaimId=" + hdnClaimId + "&&ToothServiceLine=" + ToothServiceLine + "&&Claims_Tooth_and_Surface_Information_ID=" + Claims_Tooth_and_Surface_Information_ID + "&&Tooth_Number=" + Tooth_Number + "&&Tooth_Surface1=" + Tooth_Surface1 + "&&Tooth_Surface2=" + Tooth_Surface2 + "&&Tooth_Surface3=" + Tooth_Surface3 + "&&Tooth_Surface4=" + Tooth_Surface4 + "&&Tooth_Surface5=" + Tooth_Surface5 + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                    //data: '{hdnClaimId: "' + hdnClaimId + '" ,ToothServiceLine: "' + ToothServiceLine + '" , Claims_Tooth_and_Surface_Information_ID: "' + Claims_Tooth_and_Surface_Information_ID + '" , Tooth_Number: "' + Tooth_Number + '" , Tooth_Surface1: "' + Tooth_Surface1 + '" , Tooth_Surface2: "' + Tooth_Surface2 + '" , Tooth_Surface3: "' + Tooth_Surface3 + '" , Tooth_Surface4: "' + Tooth_Surface4 + '", Tooth_Surface5: "' + Tooth_Surface5 + '"    }',
                    headers: {
                        "Access-Control-Allow-Origin": "*",
                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                        "Authorization": "Bearer " + APIToken
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var ToothQuadrantInfoList = result;
                        console.log(ToothQuadrantInfoList);
                        var table = "";
                        if (ToothQuadrantInfoList.length > 0) {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; margin-left: 10px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:20px; scope='col'>Service Line</th><th style='width:20px; scope='col'>Tooth Number</th > <th style='width:20px; scope=' col'> Tooth Surface</th > <th style='width: 20px; scope = ' col' ></th > <th style='width: 20px; scope = ' col' ></th > <th style='width: 20px; scope = ' col' ></th > <th style='width: 20px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > </tr > ";
                            for (var i = 0; i < ToothQuadrantInfoList.length; i++) {
                                var j = 0;
                                var ToothServiceLine = result[i].ToothServiceLine;                               
                                var ToothNumber = result[i].ToothNumber;
                                var ToothSurface1 = result[i].ToothSurface1;
                                var ToothSurface2 = result[i].ToothSurface2;
                                var ToothSurface3 = result[i].ToothSurface3;
                                var ToothSurface4 = result[i].ToothSurface4;
                                var ToothSurface5 = result[i].ToothSurface5;
                                var Claims_Tooth_and_Surface_Information_ID = result[i].Claims_Tooth_and_Surface_Information_ID;
                                $("#<%=ddlToothServiceLine.ClientID %> option[value='" + ToothServiceLine + "']").remove();
                                j++;
                                if (j >= 50) { document.getElementById('<%= btnAdd.ClientID%>').style.visibility = "hidden"; }
                                else { document.getElementById('<%= btnAdd.ClientID%>').style.visibility = "visible"; }
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + ToothServiceLine + "</span></td><td><span title='Line' class='tNumber'>" + ToothNumber + "</span></td><td><span  title='Line' class='tNumber'>" + ToothSurface1 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface2 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface3 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface4 + "</span></td><td><span title='Line' class='tNumber'>" + ToothSurface5 + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditToothQuadrantInfoLineItem(\"" + Claims_Tooth_and_Surface_Information_ID + "\",\"" + hdnClaimId + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteToothQuadrantInfoItem(\"" + Claims_Tooth_and_Surface_Information_ID + "\",\"" + ToothServiceLine + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr > ";
                            };
                            table = table + "</tbody></table>";
                            localStorage.setItem("ToothQuadrantInfoTable", "" + table + "");
                            document.getElementById('<%= ToothQuadrantInfoOutput.ClientID %>').innerHTML = table;

                            document.getElementById('<%= btnAdd.ClientID%>').style.visibility = "visible";
                            document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
                            document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "hidden";
                            ClearFieldsTooth();
                            return false;
                        }
                    },

                    error: function (jqXHR, textStatus, errorThrown) {
                        $("[id*=lblToothSurfaceError]").text('Tooth code not found(5).');
                    }
                });
            } 

            document.getElementById('<%= btnAdd.ClientID%>').style.visibility = "visible";
            document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
            document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "hidden";

        }       
        GetToothSurface();
        ClearFieldsTooth();       
        return false;
    }

    function toothNumberChange() {
        var ToothNumber = $("#<%= txtToothNumber.ClientID %>").first().val();
        // if (ToothNumber == "1" || ToothNumber == "2" || ToothNumber == "3" || ToothNumber == "4" || ToothNumber == "5" || ToothNumber == "6" || ToothNumber == "7" || ToothNumber == "8" || ToothNumber == "9") { ToothNumber = "0" + ToothNumber; }
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "validateToothNo?ToothNumber=" + ToothNumber+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
               
                if (result === "false") {
                    $("[id*=lblToothNumberInvalid]").text('*Tooth number is invalid.');
                    $('#ctl00_MainContent_uc5SubmitClaim_ucToothQuadrant_txtToothNumber').val('');
                }
                else { $("[id*=lblToothNumberInvalid]").text('');
                }
             },

             error: function (jqXHR, textStatus, errorThrown) {
                 $("[id*=lblToothSurfaceError]").text('Tooth code not found(6).');
             }
         });

        return false;
    }
      
</script>
<script src="../Scripts/jquery.inputmask.bundle.min.js">
    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9\x20]/g, '');
    }
   
    <%-- function TDisableEnableConditionAddButton() {
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucToothQuadrantInfo_btnAddToothQuadrantInfo").prop("disabled", true)
        }, 50);
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucToothQuadrantInfo_btnAddToothQuadrantInfo").prop('disabled', false);
        }, 1000);--%>
    }
</script>

<asp:UpdatePanel ID="updatePanelToothQuadrant" runat="server"  UpdateMode="Conditional">
    <ContentTemplate>
        <ajax:CollapsiblePanelExtender ID="cpeToothQuadrantInfo" runat="server" Collapsed="true" TargetControlID="pnlToothQuadrantInfo" ExpandControlID="pnlsepToothQuadrantInfo" CollapseControlID="pnlsepToothQuadrantInfo" />
        <asp:Panel runat="server" ID="pnlsepToothQuadrantInfo" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" 
            CssClass="OwnerToothQuadrantInfo CollapsingSeparator">
            <span id="sepToothQuadrantInfo" runat="server" class="pageHeader pH2">+ Tooth & Tooth Surface Information </span>
        </asp:Panel>
         
        <asp:Panel ID="pnlToothQuadrantInfo" runat="server" Style="min-height: 240px;  height: auto;   padding-bottom: 50px;">
            <div>
          <asp:Label runat="server" ID="toothValidator" CssClass="error-message" Text="Tooth number is invalid" Visible="false" /></div>
           
            <div id="ToothQuadrantInfoOutput" runat="server"></div>
           <%-- <div class="divGrid">
                <asp:GridView ID="gvToothQuadrantInfo" runat="server"  AllowSorting="false" CssClass="gridview" Width="100%"
                    AutoGenerateColumns="false"  
                    OnRowEditing="gvToothQuadrantInfo_RowEditing"
                    OnRowDataBound="gvToothQuadrantInfo_RowDataBound "
                    OnRowUpdating="gvToothQuadrantInfo_RowUpdating"
                    OnRowCancelingEdit="gvToothQuadrantInfo_RowCancelingEdit"
                    OnRowDeleting="gvToothQuadrantInfo_RowDeleting"
                    DataKeyNames="Claims_Tooth_and_Surface_Information_Id"
                    >

                    <Columns>
                        <asp:BoundField DataField="Service_Line" HeaderText="Service Line" ReadOnly="true" HeaderStyle-CssClass="GridviewHeaderAsterisk"/>
                         <asp:TemplateField HeaderText="Tooth Number"  HeaderStyle-CssClass="GridviewHeaderAsterisk">
                    <ItemTemplate>
                        <asp:Label ID="lblToothNumber" runat="server" Text='<%# Eval("Tooth_Number") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditToothNumber" runat="server" Style="height: 30px; width: 200px; min-width: 200px;">
                        </asp:TextBox>
                         <asp:RequiredFieldValidator ID="rfvEditToothNumber" runat="server" ControlToValidate="txtEditToothNumber"
                                        ErrorMessage="<div>*Tooth Number is required</div>" CssClass="failureNotification"
                                        Display="Dynamic" ValidationGroup="valToothQuadEdit"></asp:RequiredFieldValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tooth Surface" HeaderStyle-Width="50%">
                            <ItemTemplate>
                                <asp:Label ID="lblToothSurface1" runat="server" Text='<%# Eval("Toothsurface1") %>'></asp:Label>
                                <asp:Label ID="lblToothSurface2" runat="server" Text='<%# Eval("Toothsurface2") %>'></asp:Label>
                                <asp:Label ID="lblToothSurface3" runat="server" Text='<%# Eval("Toothsurface3") %>'></asp:Label>
                                <asp:Label ID="lblToothSurface4" runat="server" Text='<%# Eval("Toothsurface4") %>'></asp:Label>
                                <asp:Label ID="lblToothSurface5" runat="server" Text='<%# Eval("Toothsurface5") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlToothSurface1" runat="server"
                                    Style="min-width: 90px; width: 90px;"
                                    CssClass="formFieldTextBoxSmall" OnSelectedIndexChanged ="ddlToothSurface1_Selected_IndexChangedEdit" 
                                    AppendDataBoundItems="true" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlToothSurface2" runat="server"
                                    Style="min-width: 90px; width: 90px;"
                                    CssClass="formFieldTextBoxSmall" OnSelectedIndexChanged ="ddlToothSurface2_Selected_IndexChangedEdit" 
                                    AppendDataBoundItems="true">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlToothSurface3" runat="server"
                                    Style="min-width: 90px; width: 90px;"
                                    CssClass="formFieldTextBoxSmall" OnSelectedIndexChanged ="ddlToothSurface3_Selected_IndexChangedEdit" 
                                    AppendDataBoundItems="true">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlToothSurface4" runat="server"
                                    Style="min-width: 90px; width: 90px;"
                                    CssClass="formFieldTextBoxSmall" OnSelectedIndexChanged ="ddlToothSurface4_Selected_IndexChangedEdit" 
                                    AppendDataBoundItems="true">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlToothSurface5" runat="server"
                                    Style="min-width: 90px; width: 90px;"
                                    CssClass="formFieldTextBoxSmall" AppendDataBoundItems="true">
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="25%">
                                <EditItemTemplate>
                                    <asp:Button ID="btnUpdate" Text="Update" runat="server" CommandName="Update" CausesValidation="true" ValidationGroup="valToothQuadEdit"  CssClass="btn btn-primary" />                                   
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" CssClass="btn btn-danger" CausesValidation="false" />
                                </EditItemTemplate>
                               
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" CausesValidation="false" CommandName="Edit" CssClass="btn btn-primary" Text="Edit" />
                                    <asp:Button ID="btnDelete" runat="server" CausesValidation="false" CommandName="Delete" CssClass="btn btn-danger" OnClientClick="return confirm(&quot;Are you sure you want to delete this record?&quot;);" Text="Delete" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="200px" />
                               
                            </asp:TemplateField>
                    </Columns>


                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>

            </div>--%>
           <div class="row" style="text-align: center; padding-left: 5px;height:auto;" runat="server" id="divInsertTooth">


                <div class="col-sm-2">
                    <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: left;"><b>Service Line</b></span>
                    <div style="text-align: left;">
                        <asp:HiddenField ID="hdnToothPanelId" runat="server" />
                        <asp:DropDownList ID="ddlToothServiceLine" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True"
                            Style="min-width: 90px; width: 90px;"
                            CssClass="formFieldTextBoxSmall"
                            ReadOnly="true" class="selectdropdown"/>
                        <%--<asp:TextBox ID="TextBox1" runat="server"  ReadOnly="true"/>--%>
                                                   <%-- <br />
                        <asp:RequiredFieldValidator ID="rfvDropdownlist1" runat="server"
                            ControlToValidate="ddlToothServiceLine"
                            Text="*Service Line is required"
                            Display="Dynamic"
                            CssClass="failureNotification"
                            ValidationGroup="valToothQuad">
                        </asp:RequiredFieldValidator>--%>
<br />
                        
                    </div>
                    
                    
                </div>
                <div class="col-sm-2">
                    <span class="ohio-field  GridviewHeaderAsterisk" style="font-size: 15px; text-align: left"><b>Tooth Number</b></span>
                    <div style="text-align: left;">
                        <asp:TextBox ID="txtToothNumber" runat="server"
                            CssClass="formFieldTextBoxSmall" 
                            MaxLength="12"
                            onKeyUp="javascript:alphanumericOnly(this);"
                            Style="height: 30px; min-width: 90px;" Width="90px" ValidationGroup="valToothQuad" onChange="return toothNumberChange()" /><br />
                         <button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                   
                       <%-- <asp:RequiredFieldValidator ID="rfvToothNumber" runat="server"
                            ControlToValidate="txtToothNumber"
                            Text="*Tooth Number is required" 
                            Display="Dynamic"
                            CssClass="failureNotification"
                            ValidationGroup="valToothQuad">
                        </asp:RequiredFieldValidator>--%>

                        <asp:Label ID="lblToothNumberInvalid" runat="server"  Text="" CssClass="failureNotification"></asp:Label>
                    </div>
                </div>
                <div class="col-sm-6">
                    <span class="ohio-field" style="font-size: 15px; text-align: left"><b>Tooth Surface</b></span>
                    <div style="text-align: left;">
                        <asp:DropDownList ID="ddlToothSurface1" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True"
                            Style=" min-width: 90px; width: 90px;"
                            CssClass="formFieldTextBoxSmall" OnChange="return GetToothSurface1()"
                            ReadOnly="true" 
                            >
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlToothSurface2" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True"
                            Style=" min-width: 90px; width: 90px;"
                            CssClass="formFieldTextBoxSmall"  OnChange="return GetToothSurface2()"                            
                            ReadOnly="true">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlToothSurface3" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True"
                            Style=" min-width: 90px; width: 90px;"
                            CssClass="formFieldTextBoxSmall"
                            ReadOnly="true"  OnChange="return GetToothSurface3()">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlToothSurface4" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True"
                            Style=" min-width: 90px; width: 90px;"
                            CssClass="formFieldTextBoxSmall"
                            ReadOnly="true"  OnChange="return GetToothSurface4()">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlToothSurface5" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True"
                            Style=" min-width: 90px; width: 90px;"
                            CssClass="formFieldTextBoxSmall"  OnChange="loadAddToothddl()"
                            ReadOnly="true">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <%--<button id="btnloading2" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>--%>
                   </div>
                </div>
                <div class="col-sm-2">
                     <span class="ohio-field" style="font-size: 15px; text-align: left">&nbsp;</span>
                     <div class="row">
                        <div class="col-sm-12">
                             <asp:Button ID="btnAdd" Text="ADD" OnClientClick="return AddToothQuadrantInfo()" runat="server"  CssClass="btn btn-primary" Font-Bold="True" Width="90px" />
                            </div>
                        </div>
                     <div class="row">
                        <div class="col-sm-12">
                                <asp:Button ID="btnEdit" Style="visibility: hidden" Text="Update" OnClientClick="return UpdateToothQuadrantInfo()" runat="server" CssClass="btn btn-primary" Font-Bold="True"  Width="70px" />
                            </div>
                         </div>
                     <div class="row">
                        <div class="col-sm-12">
                         <asp:Button ID="btnCancel" Style="visibility: hidden" Text="Cancel" OnClientClick="return CancelToothQuadrantInfo()" runat="server" CssClass="btn btn-primary" Font-Bold="True"  Width="70px" />
                        </div>
                    </div>
                </div>

            </div>

            <div class="form-group" style="float: left; width: 100%; padding-top:35px">
                <asp:Label ID="lblToothSurfaceError" runat="server" ForeColor="Red"></asp:Label>
            </div>

        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="hdnClaimId" runat="server" />
<asp:HiddenField ID="Claims_Tooth_and_Surface_Information_ID" runat="server" />