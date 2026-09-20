<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_AmbulancePickupDropOff" Codebehind="AmbulancePickupDropOff.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<script src="../Scripts/jquery.inputmask.bundle.min.js"></script>
<style type="text/css">
    .RedAsterisk {
        color: red;
    }
</style>
<script>
    function AmbDisableEnableConditionAddButton() {
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucAmbulancePickupDropOff_btnAddAmbulanceService").prop("disabled", true)
        }, 50);
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucAmbulancePickupDropOff_btnAddAmbulanceService").prop('disabled', false);
        }, 1000);
    }

    function displayAmbulanceDropOffTable() {
       
        var hdnClaimstatusAmbulance = document.getElementById("<%=hdnClaimstatusAmbulance.ClientID %>").value;
        if (hdnClaimstatusAmbulance == "Pending Submission") {
            var table = localStorage.getItem("ambulanceDropOffTable");
        }
        else if (hdnClaimstatusAmbulance == "Other") {
            var table = localStorage.removeItem("ambulanceDropOffTable");
        }
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= ambulancedropOffDiv.ClientID %>').innerHTML = table;
        }
    };


    function addAmbulanceDrop() {
        var claimAmbulanceInfoList;
        var validateField = "";
        var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
        var serviceLine = $("#<%=ddlAmbulanceServiceLine.ClientID %> option:selected").text();
        var pickUpAddressLine1 = $("#<%= txtAmbulanceServicePickUpAddressLine1.ClientID %>").first().val();
        var pickUpAddressLine2 = $("#<%= txtAmbulanceServicePickUpAddressLine2.ClientID %>").first().val();
        var pickupcity = $("#<%= txtBoxAmbulanceServicePickUpCity1.ClientID %>").first().val();
        var pickUPState = $("#<%=ddlAmbulanceServicePickUpState1.ClientID %> option:selected").text();

        var pickUPZip = $("#<%= txtBoxAmbulanceServiceZip1.ClientID %>").first().val();
        var dropOffLocationName = $("#<%= txtAmbulanceServiceDropOffLocationName.ClientID %>").first().val();
        var dropOffAddressLine1 = $("#<%= txtAmbulanceServiceDropPickUpAddressLine1.ClientID %>").first().val();
        var dropOffAddressLine2 = $("#<%= txtAmbulanceServiceDropPickUpAddressLine2.ClientID %>").first().val();
        var dropOffCity = $("#<%= txtBoxAmbulanceServicePickUpCity2.ClientID %>").first().val();
        var dropOffState = $("#<%= ddlAmbulanceServicePickUpState2.ClientID %>").first().val();
        var dropOffZip = $("#<%= txtBoxAmbulanceServiceZip2.ClientID %>").first().val();

        validateField = validateFields();
        if (validateField === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "AddAmbulanceInfo?claimid=" + claimid + "&&serviceLine=" + serviceLine + "&&pickUpAddressLine1=" + pickUpAddressLine1 + "&&pickUpAddressLine2=" + pickUpAddressLine2 + "&&pickupcity=" + pickupcity + "&&pickUPState=" + pickUPState + "&&pickUPZip=" + pickUPZip + "&&dropOffLocationName=" + dropOffLocationName + "&&dropOffAddressLine1=" + dropOffAddressLine1 + "&&dropOffAddressLine2=" + dropOffAddressLine2 + "&&dropOffCity=" + dropOffCity + "&&dropOffState=" + dropOffState + "&&dropOffZip=" + dropOffZip + "&&User=<%=HttpContext.Current.User.Identity.Name%>",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimAmbulanceInfoList = result;
                    var table;
                    var ErrorMsg = "";
                    
                    if (claimAmbulanceInfoList.length > 0) {
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Pick Up Address Line 1</th><th style='width:10px; scope='col'>Pick Up City</th><th style='width:10px; scope='col'>Pick Up State</th><th style='width:10px; scope='col'>Pick Up Zip</th><th style='width:10px; scope='col'>Drop Off Address Line 1</th><th style='width:10px; scope='col'>Drop Off City</th><th style='width:10px; scope='col'>Drop Off State</th><th style='width:10px; scope='col'>Drop Off Zip</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        for (var i = 0; i < claimAmbulanceInfoList.length; i++) {
                            var Error = result[i].Error_Message;
                            ErrorMsg = Error;
                        }
                        
                        if (Error == null || Error == undefined || Error == "") {
                            var j = 0;
                            for (var i = 0; i < claimAmbulanceInfoList.length; i++) {
                                var serviceLine = result[i].serviceLine;
                                var pickUpAddressLine1 = result[i].pickUpAddressLine1;
                                var pickUpCity = result[i].pickUpCity;
                                var pickUpState = result[i].pickUpState;
                                var pickUpZip = result[i].pickUpZip;
                                var dropOffLocationAddressline1 = result[i].dropOffLocationAddressline1;
                                var dropOffLocationCity = result[i].dropOffLocationCity;
                                var dropOffLocationZip = result[i].dropOffLocationZip;
                                var dropOffLocationState = result[i].dropOffLocationState;
                                var Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = result[i].Claims_Ambulance_Pick_Up_Drop_Off_Location_ID;

                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + serviceLine + "</span></td><td><span title='Line' class='tNumber'>" + pickUpAddressLine1 + "</span></td><td><span  title='Line' class='tNumber'>" + pickUpCity + "</span></td><td><span title='Line' class='tNumber'>" + pickUpState + "</span></td><td>" + pickUpZip + "</td><td>" + dropOffLocationAddressline1 + "</td><td>" + dropOffLocationCity + "</td><td>" + dropOffLocationState + "</td><td>" + dropOffLocationZip + "</td><td><input type='button' value = 'Edit' onClick = 'return EditAmbulanmceDropOffItem(\"" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteAmbulanmceDropOffLineitem(\"" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                            };

                            table = table + "</tbody></table>";
                            localStorage.setItem("ambulanceDropOffTable", "" + table + "");
                            document.getElementById('<%= ambulancedropOffDiv.ClientID %>').innerHTML = table;
                            clearAmbulanceDropOffFields();
                            $("[id*=lblerrorPanellevelAmbulance]").text('');
                        }
                        else {
                            $("[id*=lblerrorPanellevelAmbulance]").text(ErrorMsg);
                            
                            clearAmbulanceDropOffFields();
                        }

                    }
                },
                error: function (jqXHR, textStatus, errorThrown)
                {;
                    $("[id*=lblmessageAmbulancePickup]").text('try again later..' + textStatus + "<br >" + errorThrown + "<br />");
                }
            });
        }
        return false;
    }


    function validateFields() {
        var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
        var serviceLine = $("#<%=ddlAmbulanceServiceLine.ClientID %> option:selected").text();
        var pickUpAddressLine1 = $("#<%= txtAmbulanceServicePickUpAddressLine1.ClientID %>").first().val();
        var pickUpAddressLine2 = $("#<%= txtAmbulanceServicePickUpAddressLine2.ClientID %>").first().val();
        var pickupcity = $("#<%= txtBoxAmbulanceServicePickUpCity1.ClientID %>").first().val();
        var pickUPState = $("#<%=ddlAmbulanceServicePickUpState1.ClientID %> option:selected").text();

        var pickUPZip = $("#<%= txtBoxAmbulanceServiceZip1.ClientID %>").first().val();
        var dropOffLocationName = $("#<%= txtAmbulanceServiceDropOffLocationName.ClientID %>").first().val();
        var dropOffAddressLine1 = $("#<%= txtAmbulanceServiceDropPickUpAddressLine1.ClientID %>").first().val();
        var dropOffAddressLine2 = $("#<%= txtAmbulanceServiceDropPickUpAddressLine2.ClientID %>").first().val();
        var dropOffCity = $("#<%= txtBoxAmbulanceServicePickUpCity2.ClientID %>").first().val();
        var dropOffState = $("#<%= ddlAmbulanceServicePickUpState2.ClientID %>").first().val();
        var dropOffZip = $("#<%= txtBoxAmbulanceServiceZip2.ClientID %>").first().val();
        if ((serviceLine === null || serviceLine === "" || serviceLine === undefined)) {

            validateField = "false";
            $("[id*=lblServiceLineError]").text('*Service line number is required for ambulance pick-up and drop-off information in service detail');
            return validateField;
        }
        else {
            validateField = "true";
            $("[id*=lblServiceLineError]").text('');
        }

        if ((pickUpAddressLine1 === null || pickUpAddressLine1 === "" || pickUpAddressLine1 === undefined)) {

            validateField = "false";
            $("[id*=pickUpAddressLine1Error]").text('*Ambulance pick-up address is required');
            return validateField;
        }
        else {
            validateField = "true";
            $("[id*=pickUpAddressLine1Error]").text('');
        }

        if ((pickupcity === null || pickupcity === "" || pickupcity === undefined)) {

            validateField = "false";
            $("[id*=lblPickUpCityError]").text('*Ambulance pick-up city is required');
            return validateField;
        }
        else {
            validateField = "true";
            $("[id*=lblPickUpCityError]").text('');
        }

        if ((pickUPState === null || pickUPState === "" || pickUPState === undefined)) {

            validateField = "false";
            $("[id*=lblPickUpStateError]").text('*Ambulance pick up state is required');
            return validateField;
        }
        else {
            validateField = "true";
            $("[id*=lblPickUpStateError]").text('');
        }

        if ((pickUPZip === null || pickUPZip === "" || pickUPZip === undefined)) {

            validateField = "false";
            $("[id*=lblPickUpZipError]").text('*Ambulance pick up zip is required');
            return validateField;
        }
        else {
            validateField = "true";
            $("[id*=lblPickUpZipError]").text('');
        }

        if ((dropOffAddressLine1 === null || dropOffAddressLine1 === "" || dropOffAddressLine1 === undefined)) {

            validateField = "false";
            $("[id*=lblDropAddressLine1Error]").text('*Ambulance drop off address is required');
            return validateField;
        }
        else {
            validateField = "true";
            $("[id*=lblDropAddressLine1Error]").text('');
        }

        if ((dropOffCity === null || dropOffCity === "" || dropOffCity === undefined)) {

            validateField = "false";
            $("[id*=lblDropCityError]").text('*Ambulance drop off city is required');
            return validateField;
        }
        else {
            validateField = "true";
            $("[id*=lblDropCityError]").text('');
        }

        if ((dropOffState === null || dropOffState === "" || dropOffState === undefined)) {

            validateField = "false";
            $("[id*=lblDropStateError]").text('*Ambulance drop off state is required');
            return validateField;
        }
        else {
            validateField = "true";
            $("[id*=lblDropStateError]").text('');
        }

        if ((dropOffZip === null || dropOffZip === "" || dropOffZip === undefined)) {

            validateField = "false";
            $("[id*=lblDropZipError]").text('*Ambulance drop off zip is required');
            return validateField;
        }
        else {
            validateField = "true";
            $("[id*=lblDropZipError]").text('');
        }
        return validateField;
    }

    function EditAmbulanmceDropOffItem(Claims_Ambulance_Pick_Up_Drop_Off_Location_ID,control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        var Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = Claims_Ambulance_Pick_Up_Drop_Off_Location_ID;
        var APIToken = $("[id*=hdnAccessToken]").val();
         $.ajax({
             type: "GET",
             url: webApiClaims + "GetIndAmbulanceInfo?Claims_Ambulance_Pick_Up_Drop_Off_Location_ID=" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID,
             //data: '{Claims_Ambulance_Pick_Up_Drop_Off_Location_ID: "' + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + '"  }',
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
                        var serviceLine = result[i].serviceLine;
                        var pickUpAddressLine1 = result[i].pickUpAddressLine1;
                        var pickUpAddressLine2 = result[i].pickUpAddressLine2;
                        var pickUpCity = result[i].pickUpCity;
                        var pickUpState = result[i].pickUpState;
                        var pickUpZip = result[i].pickUpZip;
                        var dropOffLocationName = result[i].dropOffLocationName;
                        var dropOffLocationAddressline1 = result[i].dropOffLocationAddressline1;
                        var dropOffLocationAddressline2 = result[i].dropOffLocationAddressline2;
                        var dropOffLocationCity = result[i].dropOffLocationCity;
                        var dropOffLocationZip = result[i].dropOffLocationZip;
                        var dropOffLocationState = result[i].dropOffLocationState;
                        var Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = result[i].Claims_Ambulance_Pick_Up_Drop_Off_Location_ID;
                        var Claim_ID = result[i].Claim_ID;

                      
                        $("[id*=txtAmbulanceServicePickUpAddressLine1]").val("" + pickUpAddressLine1 + "");                      
                        $("[id*=txtAmbulanceServicePickUpAddressLine2]").val("" + pickUpAddressLine2 + "");
                        $("[id*=txtBoxAmbulanceServicePickUpCity1]").val("" + pickUpCity + "");
                        $("[id*=txtBoxAmbulanceServiceZip1]").val("" + pickUpZip + "");
                        $("[id*=txtAmbulanceServiceDropOffLocationName]").val("" + dropOffLocationName + "");
                        $("[id*=txtAmbulanceServiceDropPickUpAddressLine1]").val("" + dropOffLocationAddressline1 + "");
                        $("[id*=txtAmbulanceServiceDropPickUpAddressLine2]").val("" + dropOffLocationAddressline2 + "");
                        $("[id*=txtBoxAmbulanceServicePickUpCity2]").val("" + dropOffLocationCity + "");
                        $("[id*=txtBoxAmbulanceServiceZip2]").val("" + dropOffLocationZip + "");
                        $("[id*=Claims_Ambulance_Pick_Up_Drop_Off_Location_ID]").val("" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + "");
                        $('#<%=ddlAmbulanceServiceLine.ClientID%>').val(serviceLine);
                        $('#<%=ddlAmbulanceServicePickUpState1.ClientID%>').val(pickUpState);
                        $('#<%=ddlAmbulanceServicePickUpState2.ClientID%>').val(dropOffLocationState);

                        document.getElementById('<%= btnAddAmbulanceService.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnUpdateAmbulanceService.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnCancelAmbulanceService.ClientID%>').style.visibility = "visible";

                        return false;
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblIcdVersionError]").text('Diagnosis Code is invalid');
            }
        });

    }

    function EditAmbulanceDroOffCode() {
        var claimAmbulanceInfoList;
        var validateField = "";
        var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
        var serviceLine = $("#<%=ddlAmbulanceServiceLine.ClientID %> option:selected").text();
        var pickUpAddressLine1 = $("#<%= txtAmbulanceServicePickUpAddressLine1.ClientID %>").first().val();
        var pickUpAddressLine2 = $("#<%= txtAmbulanceServicePickUpAddressLine2.ClientID %>").first().val();
        var pickupcity = $("#<%= txtBoxAmbulanceServicePickUpCity1.ClientID %>").first().val();
        var pickUPState = $("#<%=ddlAmbulanceServicePickUpState1.ClientID %> option:selected").text();
        var Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = document.getElementById("<%=Claims_Ambulance_Pick_Up_Drop_Off_Location_ID.ClientID %>").value;
        var pickUPZip = $("#<%= txtBoxAmbulanceServiceZip1.ClientID %>").first().val();
        var dropOffLocationName = $("#<%= txtAmbulanceServiceDropOffLocationName.ClientID %>").first().val();
        var dropOffAddressLine1 = $("#<%= txtAmbulanceServiceDropPickUpAddressLine1.ClientID %>").first().val();
        var dropOffAddressLine2 = $("#<%= txtAmbulanceServiceDropPickUpAddressLine2.ClientID %>").first().val();
        var dropOffCity = $("#<%= txtBoxAmbulanceServicePickUpCity2.ClientID %>").first().val();
        var dropOffState = $("#<%= ddlAmbulanceServicePickUpState2.ClientID %>").first().val();
        var dropOffZip = $("#<%= txtBoxAmbulanceServiceZip2.ClientID %>").first().val();

        validateFields();
         if (validateField === "true") {
             var APIToken = $("[id*=hdnAccessToken]").val();
             $.ajax({
                 type: "PUT",

                 url: webApiClaims + "EditAmbulanceDropOffCode?Claims_Ambulance_Pick_Up_Drop_Off_Location_ID=" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + "&&claimid=" + claimid + "&&serviceLine=" + serviceLine + "&&pickUpAddressLine1=" + pickUpAddressLine1 + "&&pickUpAddressLine2=" + pickUpAddressLine2 + "&&pickupcity=" + pickupcity + "&&pickUPState=" + pickUPState + "&&pickUPZip=" + pickUPZip + "&&dropOffLocationName=" + dropOffLocationName + "&&dropOffAddressLine1=" + dropOffAddressLine1 + "&&dropOffAddressLine2=" + dropOffAddressLine2 + "&&dropOffCity=" + dropOffCity + "&&dropOffState=" + dropOffState + "&&dropOffZip=" + dropOffZip + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",

                 //data: '{ Claims_Ambulance_Pick_Up_Drop_Off_Location_ID: "' + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + '" ,  claimid: "' + claimid + '" ,serviceLine: "' + serviceLine + '" , pickUpAddressLine1: "' + pickUpAddressLine1 + '" , pickUpAddressLine2: "' + pickUpAddressLine2 + '" , pickupcity: "' + pickupcity + '" , pickUPState: "' + pickUPState + '" , pickUPZip: "' + pickUPZip + '" , dropOffLocationName: "' + dropOffLocationName + '" , dropOffAddressLine1: "' + dropOffAddressLine1 + '" , dropOffAddressLine2: "' + dropOffAddressLine2 + '" , dropOffCity: "' + dropOffCity + '" , dropOffState: "' + dropOffState + '" , dropOffZip: "' + dropOffZip + '"  }',
                 headers: {
                     "Access-Control-Allow-Origin": "*",
                     "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                     "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                     "Authorization": "Bearer " + APIToken
                 },
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (result) {
                     claimAmbulanceInfoList = result;
                     var table;
                     var totalcharge = 0;
                     var totalpaidamount = 0;
                     var ErrorMsg = "";
                     if (claimAmbulanceInfoList.length > 0) {

                         table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Pick Up Address Line 1</th><th style='width:10px; scope='col'>Pick Up City</th><th style='width:10px; scope='col'>Pick Up State</th><th style='width:10px; scope='col'>Pick Up Zip</th><th style='width:10px; scope='col'>Drop Off Address Line 1</th><th style='width:10px; scope='col'>Drop Off City</th><th style='width:10px; scope='col'>Drop Off State</th><th style='width:10px; scope='col'>Drop Off Zip</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                         for (var i = 0; i < claimAmbulanceInfoList.length; i++) {

                             var Error = result[i].Error_Message;
                             ErrorMsg = Error;
                         }
                         if (Error == null || Error == undefined || Error == "") {
                             var j = 0;

                             for (var i = 0; i < claimAmbulanceInfoList.length; i++) {

                                 var serviceLine = result[i].serviceLine;
                                 var pickUpAddressLine1 = result[i].pickUpAddressLine1;
                                 var pickUpAddressLine2 = result[i].pickUpAddressLine2;
                                 var pickUpCity = result[i].pickUpCity;
                                 var pickUpState = result[i].pickUpState;
                                 var pickUpZip = result[i].pickUpZip;
                                 var dropOffLocationName = result[i].dropOffLocationName;
                                 var dropOffLocationAddressline1 = result[i].dropOffLocationAddressline1;
                                 var dropOffLocationAddressline2 = result[i].dropOffLocationAddressline2;
                                 var dropOffLocationCity = result[i].dropOffLocationCity;
                                 var dropOffLocationZip = result[i].dropOffLocationZip;
                                 var dropOffLocationState = result[i].dropOffLocationState;
                                 var Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = result[i].Claims_Ambulance_Pick_Up_Drop_Off_Location_ID;
                                 var Claim_ID = result[i].Claim_ID;

                                 table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + serviceLine + "</span></td><td><span title='Line' class='tNumber'>" + pickUpAddressLine1 + "</span></td><td><span  title='Line' class='tNumber'>" + pickUpCity + "</span></td><td><span title='Line' class='tNumber'>" + pickUpState + "</span></td><td>" + pickUpZip + "</td><td>" + dropOffLocationAddressline1 + "</td><td>" + dropOffLocationCity + "</td><td>" + dropOffLocationState + "</td><td>" + dropOffLocationZip + "</td><td><input type='button' value = 'Edit' onClick = 'return EditAmbulanmceDropOffItem(\"" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteAmbulanmceDropOffLineitem(\"" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                             };

                             table = table + "</tbody></table>";
                             localStorage.setItem("ambulanceDropOffTable", "" + table + "");
                             document.getElementById('<%= ambulancedropOffDiv.ClientID %>').innerHTML = table;
                             document.getElementById('<%= btnAddAmbulanceService.ClientID%>').style.visibility = "visible";
                             document.getElementById('<%= btnUpdateAmbulanceService.ClientID%>').style.visibility = "hidden";
                             document.getElementById('<%= btnCancelAmbulanceService.ClientID%>').style.visibility = "hidden";

                             clearAmbulanceDropOffFields();
                             $("[id*=lblerrorPanellevelAmbulance]").text('');
                         }
                         else {
                             $("[id*=lblerrorPanellevelAmbulance]").text(ErrorMsg);
                             clearAmbulanceDropOffFields();
                             document.getElementById('<%= btnAddAmbulanceService.ClientID%>').style.visibility = "visible";
                             document.getElementById('<%= btnUpdateAmbulanceService.ClientID%>').style.visibility = "hidden";
                             document.getElementById('<%= btnCancelAmbulanceService.ClientID%>').style.visibility = "hidden";
  
                       }

                     }

                 },
                 error: function (jqXHR, textStatus, errorThrown) {
                     $("[id*=lblmessageAmbulancePickup]").text('try again later..');
                 }
             });
        }
        return false;

        function validateFields() {

            if ((serviceLine === null || serviceLine === "" || serviceLine === undefined)) {

                validateField = "false";
                $("[id*=lblServiceLineError]").text('*Service line number is required for ambulance pick-up and drop-off information in service detail');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=lblServiceLineError]").text('');
            }

            if ((pickUpAddressLine1 === null || pickUpAddressLine1 === "" || pickUpAddressLine1 === undefined)) {

                validateField = "false";
                $("[id*=pickUpAddressLine1Error]").text('*Ambulance pick-up address is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=pickUpAddressLine1Error]").text('');
            }

            if ((pickupcity === null || pickupcity === "" || pickupcity === undefined)) {

                validateField = "false";
                $("[id*=lblPickUpCityError]").text('*Ambulance pick-up city is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=lblPickUpCityError]").text('');
            }

            if ((pickUPState === null || pickUPState === "" || pickUPState === undefined)) {

                validateField = "false";
                $("[id*=lblPickUpStateError]").text('*Ambulance pick up state is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=lblPickUpStateError]").text('');
            }

            if ((pickUPZip === null || pickUPZip === "" || pickUPZip === undefined)) {

                validateField = "false";
                $("[id*=lblPickUpZipError]").text('*Ambulance pick up zip is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=lblPickUpZipError]").text('');
            }

            if ((dropOffAddressLine1 === null || dropOffAddressLine1 === "" || dropOffAddressLine1 === undefined)) {

                validateField = "false";
                $("[id*=lblDropAddressLine1Error]").text('*Ambulance drop off address is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=lblDropAddressLine1Error]").text('');
            }

            if ((dropOffCity === null || dropOffCity === "" || dropOffCity === undefined)) {

                validateField = "false";
                $("[id*=lblDropCityError]").text('*Ambulance drop off city is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=lblDropCityError]").text('');
            }

            if ((dropOffState === null || dropOffState === "" || dropOffState === undefined)) {

                validateField = "false";
                $("[id*=lblDropStateError]").text('*Ambulance drop off state is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=lblDropStateError]").text('');
            }

            if ((dropOffZip === null || dropOffZip === "" || dropOffZip === undefined)) {

                validateField = "false";
                $("[id*=lblDropZipError]").text('*Ambulance drop off zip is required');
                return false;
            }
            else {
                validateField = "true";
                $("[id*=lblDropZipError]").text('');
            }

        }
     }

    function clearAmbulanceDropOffFields() {
        var serviceLine = $("#<%=ddlAmbulanceServiceLine.ClientID %> option:selected").text();      
        var pickUPState = $("#<%=ddlAmbulanceServicePickUpState1.ClientID %> option:selected").text();
        var dropOffState = $("#<%= ddlAmbulanceServicePickUpState2.ClientID %>").first().val();
        $("#<%= txtAmbulanceServicePickUpAddressLine1.ClientID %>").first().val("");
        $("#<%= txtAmbulanceServicePickUpAddressLine2.ClientID %>").first().val("");
        $("#<%= txtBoxAmbulanceServicePickUpCity1.ClientID %>").first().val("");
        $("#<%= txtBoxAmbulanceServiceZip1.ClientID %>").first().val("");
        $("#<%= txtAmbulanceServiceDropOffLocationName.ClientID %>").first().val("");
        $("#<%= txtBoxAmbulanceServicePickUpCity2.ClientID %>").first().val("");
        $("#<%= txtBoxAmbulanceServiceZip2.ClientID %>").first().val("");
        $("#<%= txtAmbulanceServiceDropPickUpAddressLine1.ClientID %>").first().val("");
        $("#<%= txtAmbulanceServiceDropPickUpAddressLine2.ClientID %>").first().val("");        
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucAmbulancePickUpDropOff_ddlAmbulanceServiceLine") != null) {
            if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucAmbulancePickUpDropOff_ddlAmbulanceServiceLine").options.length > 0) {
                document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucAmbulancePickUpDropOff_ddlAmbulanceServiceLine").options[0].selected = true;
            }
        }
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucAmbulancePickUpDropOff_ddlAmbulanceServicePickUpState1") != null) {
            if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucAmbulancePickUpDropOff_ddlAmbulanceServicePickUpState1").options.length > 0) {
                document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucAmbulancePickUpDropOff_ddlAmbulanceServicePickUpState1").options[0].selected = true;
            }
        }
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucAmbulancePickUpDropOff_ddlAmbulanceServicePickUpState2") != null) {
            if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucAmbulancePickUpDropOff_ddlAmbulanceServicePickUpState2").options.length > 0) {
                document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucAmbulancePickUpDropOff_ddlAmbulanceServicePickUpState2").options[0].selected = true;
            }
        }
    }

    function cancelAmbulanceDrop() {

        clearAmbulanceDropOffFields();

        document.getElementById('<%= btnAddAmbulanceService.ClientID%>').style.visibility = "visible";
        document.getElementById('<%= btnUpdateAmbulanceService.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnCancelAmbulanceService.ClientID%>').style.visibility = "hidden";
        var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "AmbulanceServiceBindGrid?claimid=" + claimid,
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
                claimAmbulanceInfoList = result;
                var table;
                var totalcharge = 0;
                var totalpaidamount = 0;
                if (claimAmbulanceInfoList.length > 0) {
                    table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Pick Up Address Line 1</th><th style='width:10px; scope='col'>Pick Up City</th><th style='width:10px; scope='col'>Pick Up State</th><th style='width:10px; scope='col'>Pick Up Zip</th><th style='width:10px; scope='col'>Drop Off Address Line 1</th><th style='width:10px; scope='col'>Drop Off City</th><th style='width:10px; scope='col'>Drop Off State</th><th style='width:10px; scope='col'>Drop Off Zip</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    var j = 0;

                    for (var i = 0; i < claimAmbulanceInfoList.length; i++) {
                        var serviceLine = result[i].serviceLine;
                        var pickUpAddressLine1 = result[i].pickUpAddressLine1;
                        var pickUpAddressLine2 = result[i].pickUpAddressLine2;
                        var pickUpCity = result[i].pickUpCity;
                        var pickUpState = result[i].pickUpState;
                        var pickUpZip = result[i].pickUpZip;
                        var dropOffLocationAddressline1 = result[i].dropOffLocationAddressline1;
                        var dropOffLocationCity = result[i].dropOffLocationCity;
                        var dropOffLocationZip = result[i].dropOffLocationZip;
                        var dropOffLocationState = result[i].dropOffLocationState;
                        var Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = result[i].Claims_Ambulance_Pick_Up_Drop_Off_Location_ID;
                        
                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + serviceLine + "</span></td><td><span title='Line' class='tNumber'>" + pickUpAddressLine1 + "</span></td><td><span  title='Line' class='tNumber'>" + pickUpCity + "</span></td><td><span title='Line' class='tNumber'>" + pickUpState + "</span></td><td>" + pickUpZip + "</td><td>" + dropOffLocationAddressline1 + "</td><td>" + dropOffLocationCity + "</td><td>" + dropOffLocationState + "</td><td>" + dropOffLocationZip + "</td><td><input type='button' value = 'Edit' onClick = 'return EditAmbulanmceDropOffItem(\"" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteAmbulanmceDropOffLineitem(\"" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                    };

                    table = table + "</tbody></table>";
                    localStorage.setItem("ambulanceDropOffTable", "" + table + "");
                    document.getElementById('<%= ambulancedropOffDiv.ClientID %>').innerHTML = table;
                     
                }
                else { document.getElementById('<%= ambulancedropOffDiv.ClientID %>').innerHTML = ""; }

            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblmessageAmbulancePickup]").text('try again later..');
            }
        });
        var table = localStorage.getItem("ambulanceDropOffTable");
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= ambulancedropOffDiv.ClientID %>').innerHTML = table;

        }
    return false;
    }

    function DeleteAmbulanmceDropOffLineitem(Claims_Ambulance_Pick_Up_Drop_Off_Location_ID)
    {
        var result1 = confirm("Are you sure you want to delete?");
        if (result1) {
            var Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = Claims_Ambulance_Pick_Up_Drop_Off_Location_ID;
            var claimid = document.getElementById("<%=hdnClaimId.ClientID %>").value;
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteAmbulanceDropOffCode?Claims_Ambulance_Pick_Up_Drop_Off_Location_ID=" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + "&&claimid=" + claimid,
                //data: '{Claims_Ambulance_Pick_Up_Drop_Off_Location_ID: "' + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + '",  claimid: "' + claimid + '"  }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    claimAmbulanceInfoList = result;
                    var table;
                    var totalcharge = 0;
                    var totalpaidamount = 0;
                    if (claimAmbulanceInfoList.length > 0) {

                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Pick Up Address Line 1</th><th style='width:10px; scope='col'>Pick Up City</th><th style='width:10px; scope='col'>Pick Up State</th><th style='width:10px; scope='col'>Pick Up Zip</th><th style='width:10px; scope='col'>Drop Off Address Line 1</th><th style='width:10px; scope='col'>Drop Off City</th><th style='width:10px; scope='col'>Drop Off State</th><th style='width:10px; scope='col'>Drop Off Zip</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                        var j = 0;

                        for (var i = 0; i < claimAmbulanceInfoList.length; i++) {

                            var serviceLine = result[i].serviceLine;
                            var pickUpAddressLine1 = result[i].pickUpAddressLine1;
                            var pickUpAddressLine2 = result[i].pickUpAddressLine2;
                            var pickUpCity = result[i].pickUpCity;
                            var pickUpState = result[i].pickUpState;
                            var pickUpZip = result[i].pickUpZip;
                            var dropOffLocationName = result[i].dropOffLocationName;
                            var dropOffLocationAddressline1 = result[i].dropOffLocationAddressline1;
                            var dropOffLocationAddressline2 = result[i].dropOffLocationAddressline2;
                            var dropOffLocationCity = result[i].dropOffLocationCity;
                            var dropOffLocationZip = result[i].dropOffLocationZip;
                            var dropOffLocationState = result[i].dropOffLocationState;
                            var Claims_Ambulance_Pick_Up_Drop_Off_Location_ID = result[i].Claims_Ambulance_Pick_Up_Drop_Off_Location_ID;
                            var Claim_ID = result[i].Claim_ID;

                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + serviceLine + "</span></td><td><span title='Line' class='tNumber'>" + pickUpAddressLine1 + "</span></td><td><span  title='Line' class='tNumber'>" + pickUpCity + "</span></td><td><span title='Line' class='tNumber'>" + pickUpState + "</span></td><td>" + pickUpZip + "</td><td>" + dropOffLocationAddressline1 + "</td><td>" + dropOffLocationCity + "</td><td>" + dropOffLocationState + "</td><td>" + dropOffLocationZip + "</td><td><input type='button' value = 'Edit' onClick = 'return EditAmbulanmceDropOffItem(\"" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteAmbulanmceDropOffLineitem(\"" + Claims_Ambulance_Pick_Up_Drop_Off_Location_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                        };

                        table = table + "</tbody></table>";
                        localStorage.setItem("ambulanceDropOffTable", "" + table + "");
                        document.getElementById('<%= ambulancedropOffDiv.ClientID %>').innerHTML = table;
                        clearAmbulanceDropOffFields();

                    }
                    else { document.getElementById('<%= ambulancedropOffDiv.ClientID %>').innerHTML = ""; }

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblmessageAmbulancePickup]").text('try again later..');
                }
            });
        }
    }


</script>

<%-- Ambulance Service Details Starting --%>
<%--Starting of Ambulance Panel>--%>
<ajax:CollapsiblePanelExtender ID="cpeAmbulanceServicePanel" runat="server" Collapsed="true" TargetControlID="pnlAmbulanceServicePanel" ExpandControlID="pnlsepAmbulanceServicePanel" CollapseControlID="pnlsepAmbulanceServicePanel" />
<asp:Panel runat="server" ID="pnlsepAmbulanceServicePanel" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: #2297bc" ToolTip="Click to Expand/Collapse" CssClass="OwnerRecipientInformation2">
    <span id="Span1" runat="server" class="pageHeader pH2">+ AMBULANCE INFORMATION-SERVICE DETAIL </span>
</asp:Panel>



<asp:Panel ID="pnlAmbulanceServicePanel" runat="server" Style="min-height: 120px; height: auto; overflow-x: hidden;">
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
        <ContentTemplate>
            <div>
             <asp:Label ID="lblerrorPanellevelAmbulance" runat="server" ForeColor="Red"></asp:Label>
             <asp:Label ID="lblmessageAmbulancePickup" runat="server" ForeColor="Red"></asp:Label>
            </div>
                
            <div id="ambulancedropOffDiv" runat="server">
            </div>

        
            <div id="divAmbulanceServicePanel" runat="server">
                <div class="row">
                    <asp:HiddenField ID="hdnRowNumberAmbulanceService" runat="server" />
                    <asp:HiddenField ID="Claims_Ambulance_Pick_Up_Drop_Off_Location_ID" runat="server" />
                    <div class="col-sm-6">
                        <div class="row" id="divAmbulanceServiceLineNumber" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right"><span class="RedAsterisk">*</span>Service Line</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlAmbulanceServiceLine" EnableViewState="true" runat="server"
                                        AppendDataBoundItems="True" Style="min-width: 200px; height: 30px; width: 275px" ValidationGroup="valAmbulanceService">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblServiceLineError" runat="server" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                    </div>


                    <div class="col-sm-6">
                        <div class="row" id="Div8" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Drop-off Location Name</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtAmbulanceServiceDropOffLocationName" runat="server" CssClass="formField" MaxLength="10" />

                                </span>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtAmbulanceServiceDropOffLocationName" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="row" id="Div7" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right"><span class="RedAsterisk">*</span>Pick Up Address Line 1</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtAmbulanceServicePickUpAddressLine1" runat="server" CssClass="formField" MaxLength="55" />
                                  <asp:Label ID="pickUpAddressLine1Error" runat="server" ForeColor="Red"></asp:Label>
                                </span>
                              <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtAmbulanceServicePickUpAddressLine1" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div10" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right"><span class="RedAsterisk">*</span>Drop Off Address Line 1</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtAmbulanceServiceDropPickUpAddressLine1" runat="server" CssClass="formField" MaxLength="55" />
                                   <asp:Label ID="lblDropAddressLine1Error" runat="server" ForeColor="Red"></asp:Label>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtAmbulanceServiceDropPickUpAddressLine1" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                </span>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="row" id="Div11" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Pick Up Address Line 2</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtAmbulanceServicePickUpAddressLine2" runat="server" CssClass="formField" MaxLength="55" />
                                </span>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAmbulanceServicePickUpAddressLine2" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div12" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Drop Off Address Line 2</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtAmbulanceServiceDropPickUpAddressLine2" runat="server" CssClass="formField" MaxLength="55" />
                                </span>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtAmbulanceServiceDropPickUpAddressLine2" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="row" id="Div13" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right"><span class="RedAsterisk">*</span>Pick Up City</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtBoxAmbulanceServicePickUpCity1" runat="server" CssClass="formField" MaxLength="55" />
                                  <asp:Label ID="lblPickUpCityError" runat="server" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div14" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right"><span class="RedAsterisk">*</span>Drop Off City</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtBoxAmbulanceServicePickUpCity2" runat="server" CssClass="formField" MaxLength="55" />
                                  <asp:Label ID="lblDropCityError" runat="server" ForeColor="Red"></asp:Label>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtBoxAmbulanceServicePickUpCity2" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                </span>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="row" id="div15" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right"><span class="RedAsterisk">*</span>Pick Up State</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlAmbulanceServicePickUpState1" EnableViewState="true" runat="server"
                                        AppendDataBoundItems="True" Style="min-width: 200px; height: 30px; width: 275px" ValidationGroup="valAmbulanceService">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblPickUpStateError" runat="server" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="div16" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right"><span class="RedAsterisk">*</span>Drop Off State</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlAmbulanceServicePickUpState2" EnableViewState="true" runat="server"
                                        AppendDataBoundItems="True" Style="min-width: 200px; height: 30px; width: 275px" ValidationGroup="valAmbulanceService">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblDropStateError" runat="server" ForeColor="Red"></asp:Label>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <div class="row" id="Div17" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right"><span class="RedAsterisk">*</span>Pick Up Zip</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtBoxAmbulanceServiceZip1" runat="server" CssClass="formField" MaxLength="5" />
                                    <asp:Label ID="lblPickUpZipError" runat="server" ForeColor="Red"></asp:Label>
                                  <asp:RegularExpressionValidator ID="revTxtBoxAmbulanceServiceZip1" ValidationGroup="valAmbulanceService" Display="Dynamic"
                                        ControlToValidate="txtBoxAmbulanceServiceZip1" runat="server" ErrorMessage="*" CssClass="failureNotification"
                                        Text="<div>Drop-off ZIP code is invalid</div>"
                                        SetFocusOnError="True" ValidationExpression="^\d{5}$"></asp:RegularExpressionValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtBoxAmbulanceServiceZip1" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                        ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div18" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right"><span class="RedAsterisk">*</span>Drop Off Zip</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtBoxAmbulanceServiceZip2" runat="server" CssClass="formField" MaxLength="5" />
                                  <asp:Label ID="lblDropZipError" runat="server" ForeColor="Red"></asp:Label>
                                    <asp:RegularExpressionValidator ID="revTxtBoxAmbulanceServiceZip2" ValidationGroup="valAmbulanceService" Display="Dynamic"
                                        ControlToValidate="txtBoxAmbulanceServiceZip2" runat="server" ErrorMessage="*" CssClass="failureNotification"
                                        Text="<div>Drop-off ZIP code is invalid</div>"
                                        SetFocusOnError="True" ValidationExpression="^\d{5}$"></asp:RegularExpressionValidator>
                                </span>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="row">
                    <div style="text-align: right; padding-right: 100px;">
                       
                         <asp:Button ID="btnAddAmbulanceService" Text="ADD" OnClientClick="return addAmbulanceDrop()" runat="server" CssClass="btn btn-primary" Font-Bold="True"  Width="70px" />
                           <br />
                         <asp:Button ID="btnUpdateAmbulanceService" Style="visibility: hidden" Text="Update"  runat="server" CssClass="btn btn-primary" 
                                 Font-Bold="True" ValidationGroup="vgCarePlan" Width="70px" OnClientClick="return EditAmbulanceDroOffCode()" />
                         <br />
                         <asp:Button ID="btnCancelAmbulanceService" Style="visibility: hidden" Text="Cancel"  runat="server" CssClass="btn btn-danger" 
                                 Font-Bold="True"  Width="70px" OnClientClick="return cancelAmbulanceDrop()" />
                        
                    </div>
                    <div>
                        <div runat="server" id="divConfirmAddress" style="display: none; text-align: center">
                            <p style="color: darkgreen">
                                According to the USPS database, the address entered is inaccurate. The following address was found:
                            </p>
                            <p style="color: darkgreen" id="paraUSPSAddress" runat="server"></p>
                            <p style="color: darkgreen">Click on 'Accept' to accept the corrections.</p>
                            <br />
                            <asp:Button ID="btnConfirmAddress" CssClass="buttonBoxFocus" Text="Accept" runat="server" />
                            <asp:Button ID="btnCancelAddressCorrection" CssClass="buttonBox" Text="Cancel" runat="server" />
                        </div>

                        <div runat="server" id="divWSError" style="display: none; text-align: left">
                            <p style="color: darkgreen">
                                An error occurred while validating the entered address.<br />
                                You can continue to work, but any addresses will not be validated by the system.<br />
                                <br />
                                Error details:<br />
                            </p>
                            <p style="color: darkgreen" id="paraWSError" runat="server"></p>
                            <asp:Button ID="btnConfirmWSError" class="buttonBox" Text="Ok" runat="server" />
                        </div>
                    </div>

                </div>
                <asp:CustomValidator ID="cvAddress"
                    ControlToValidate=""
                    OnServerValidate="cvAmbulanceServiceAddress_ServerValidate"
                    Display="None"
                    ErrorMessage=""
                    ValidationGroup="valAmbulanceService"
                    runat="server" />
            </div>
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Panel>




<script type="text/javascript">
    function maxZIndex() {
        var highest = -999;

        $("*").each(function () {
            var current = parseInt($(this).css("z-index"), 10);
            if (current && highest < current)
                highest = current;
        });

        return highest;
    }

    jQuery.expr[':'].contains = function (a, i, m) {
        return jQuery(a).text().toUpperCase()
            .indexOf(m[3].toUpperCase()) >= 0;
    };

    function sendDataAmbulanceService(newStreetAddress, newUnitAddress, floorDept, newAddressLine3, newCity, newState, newCounty, newZip5) {
        $("#<%= txtAmbulanceServiceDropPickUpAddressLine1.ClientID %>").first().val(newStreetAddress);
        $("#<%= txtAmbulanceServiceDropPickUpAddressLine2.ClientID %>").first().val(newUnitAddress);
        $("#<%= txtBoxAmbulanceServicePickUpCity2.ClientID %>").first().val(newCity);
        if ($("#<%= ddlAmbulanceServicePickUpState2.ClientID %>").first().val() !== newState) {
            $("#<%= ddlAmbulanceServicePickUpState2.ClientID %>").first().val(newState);
            $("#<%= ddlAmbulanceServicePickUpState2.ClientID %>").change();
        }
        $('#<%= ddlAmbulanceServicePickUpState2.ClientID %> option:contains(' + newState + ')').attr("selected", "selected");
        $("#<%= txtBoxAmbulanceServiceZip2.ClientID %>").first().val(newZip5);
    }

    function allowSave() {
        $("#<%= SaveButtonClientID %>").prop("disabled", false);
    }

    function setAddressConfirm(confirmValue) {
        var hdnAddressConfirm = document.getElementById("<% = hdnAddressConfirm.ClientID %>");
        hdnAddressConfirm.value = confirmValue;
    }
</script>
<asp:HiddenField ID="hdnAddressConfirm" runat="server" Value="0" />
<asp:HiddenField ID="hdnSaveButtonClientID" runat="server" Value="" />
<asp:HiddenField ID="hdnClaimId" runat="server" />
<asp:HiddenField ID="hdnClaimstatusAmbulance" runat="server" />
