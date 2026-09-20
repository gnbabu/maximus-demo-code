<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_AdditionalProviderInfoPanel, App_Web_l5y5araq" %>

<link href="../Content/custom-style.css" rel="stylesheet" />
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">


<script type="text/javascript">
    var validatefields = "true";
    function bindAdditionalProviderInformationserviceDetail() {
        var ClaimType = $("#<%= hdnClaimType_Additional.ClientID %>").first().val();
        var Claim_ID = $("#<%= hdnClaimIdAdditional.ClientID %>").first().val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "FetchAdditionalProviderInformationdata?Claim_ID=" + Claim_ID + "&&ClaimType=" + ClaimType+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                additionalProviderInformationdata = result;
                var table;
                var ErrorMsg = "";
                if (additionalProviderInformationdata.length > 0) {
                    if (ClaimType != "2")
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Provider Type</th><th style='width:10px; scope='col'>Provider NPI</th><th style='width:10px; scope='col'>Last Name</th><th style='width:10px; scope='col'>First Name</th><th style='width:30px; scope='col'>Middle Name</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    else
                        table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Provider Type</th><th style='width:10px; scope='col'>Provider NPI</th><th style='width:10px; scope='col'>Medicaid ID</th><th style='width:10px; scope='col'>Last Name</th><th style='width:10px; scope='col'>First Name</th><th style='width:30px; scope='col'>Middle Name</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                    for (var i = 0; i < additionalProviderInformationdata.length; i++) {
                        var Error = result[i].Error_Msg;
                        ErrorMsg = Error;
                    }
                    if (Error == null || Error == undefined || Error == "") {
                        for (var i = 0; i < additionalProviderInformationdata.length; i++) {
                            var ServiceLine = result[i].ServiceLine;
                            var ProviderType = result[i].ProviderType;
                            var ProviderNPI = result[i].ProviderNPI;
                            var LastName = result[i].LastName;
                            var FirstName = result[i].FirstName;
                            var MiddleName = result[i].MiddleName;
                            var ClaimID = result[i].ClaimID;
                            var MedicaidID = "";
                            if (ClaimType == "2")
                                MedicaidID = result[i].MedicaidID;
                            if (MedicaidID == 'undefined' || MedicaidID == "" || MedicaidID == null)
                                MedicaidID = "";
                            var Claims_Additional_Provider_Information_Service_ID = result[i].Claims_Additional_Provider_Information_Service_ID;
                            $("[id*=hdnAdditionalGridSearchRowID]").val("" + Claims_Additional_Provider_Information_Service_ID + "");

                            if (ClaimType != "2")
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='ServiceLine' class='tNumber'>" + ServiceLine + "</span></td><td><span title='ProviderType' class='tNumber'>" + ProviderType + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + ProviderNPI + "</span></td><td><span title='LastName' class='tNumber'>" + LastName + "</span></td><td><span title='FirstName' class='tNumber'>" + FirstName + "</span></td><td><span title='MiddleName' class='tNumber'>" + MiddleName + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                            else
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='ServiceLine' class='tNumber'>" + ServiceLine + "</span></td><td><span title='ProviderType' class='tNumber'>" + ProviderType + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + ProviderNPI + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + MedicaidID + "</span></td><td><span title='LastName' class='tNumber'>" + LastName + "</span></td><td><span title='FirstName' class='tNumber'>" + FirstName + "</span></td><td><span title='MiddleName' class='tNumber'>" + MiddleName + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";



                        };

                        table = table + "</tbody></table>";                      
                        document.getElementById('<%= divAdditionalProviderInfo.ClientID %>').innerHTML = table;                      
                     }
                     else {
                         
                     }
                 }
                 else {
                    document.getElementById('<%= divAdditionalProviderInfo.ClientID %>').innerHTML = ""; 
                 }
             },
             error: function (jqXHR, textStatus, errorThrown) {
                
             }
         });

    }
    function displayDentalAdditionalTable() {
        var DentalAdditionalclaimstatus = document.getElementById("<%=hdnAdditionalClaimStatusDental.ClientID %>").value;
        if (DentalAdditionalclaimstatus == "Pending Submission") {
            var table = localStorage.getItem("AdditionalProviderInformationTable");
        }
        else if (DentalAdditionalclaimstatus == "Other") {
            var table = localStorage.removeItem("AdditionalProviderInformationTable");
        }

        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= divAdditionalProviderInfo.ClientID %>').innerHTML = table;

        }
    };
    function ddlAdditonalProvidertype_onEditButtonClick(Claim_ID, ClaimType, ddlServiceLine,ClickedProviderType) {
        var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "ddlAdditonalProvidertype_onEditButtonClick?Claim_ID=" + Claim_ID + "&&ClaimType=" + ClaimType + "&&ddlServiceLine=" + ddlServiceLine + "&&ClickedProviderType=" + ClickedProviderType+"",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {

                    var ddResult = result;
                    $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlProviderType").empty();
                    $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlProviderType").append($('<option value=""></option>'));
                    for (var i = 0; i < ddResult.length; i++) {
                        var newOption = "<option value='" + ddResult[i].ID + "'>" + ddResult[i].Desc + "</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlProviderType").append(newOption);
                    }
                    //$('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlProviderType option:has("' + ClickedProviderType + '")').prop('selected', true);
                    $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlProviderType').find('option').filter(function () {
                        return $(this).text() === ClickedProviderType;
                    }).first().attr('selected', 'selected');
                    $("[id*=lblErrorAdditionalPanal]").text('');
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorAdditionalPanal]").text('try again later..');
                }
            });
    }
    function txtAdditionalProviderNPI_TextChangedNPI() {
        var NPIdata = "";
        var Claim_ID = $("#<%= hdnClaimIdAdditional.ClientID %>").first().val();
        var ClaimType = $("#<%= hdnClaimType_Additional.ClientID %>").first().val();
        var ProviderNPI = $("#<%= txtProviderNPI.ClientID %>").first().val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "txtAdditionalProviderNPI_TextChangedNPI?Claim_ID=" + Claim_ID + "&&ClaimType=" + ClaimType + "&&ProviderNPI=" + ProviderNPI,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                NPIdata = result;
                var table;
                var ErrorMsg = "";
                for (var i = 0; i < NPIdata.length; i++) {
                    var Error = result[i].Errormessage;
                    ErrorMsg = Error;
                }
                if (Error == null || Error == undefined || Error == "") {
                    var LastName = NPIdata[0].LastName;
                    var FirstName = NPIdata[0].FirstName;
                    var MedicaidId = NPIdata[0].MedicaidId;
                    $("[id*=lblProviderFirstName]").text(FirstName);
                    $("[id*=lblProviderLastName]").text(LastName);                   
                    if (ClaimType == "2")
                        $("[id*=lblAdditionalMedicaidID]").text(MedicaidId);
                    $("[id*=lblErrorMsgAdditional]").text('');
                }
                else {
                    $("[id*=lblErrorMsgAdditional]").text(ErrorMsg);
                    $("#<%= txtProviderNPI.ClientID %>").val('');
                    $("[id*=lblProviderFirstName]").text('');
                    $("[id*=lblProviderLastName]").text('');
                    if (ClaimType == "2")
                        $("[id*=lblAdditionalMedicaidID]").text('');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                ClearAdditionalProviderInfo();
            }
        });
        return false;
    }
    function ClearAdditionalProviderInfo() {
  
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlAdditonalProviderDetail").options.length > 0)
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlAdditonalProviderDetail").options[0].selected = true;
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlProviderType").options.length > 0)
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlProviderType").options[0].selected = true;
        $("#<%= txtProviderNPI.ClientID %>").val('');
        $("#<%= lblProviderLastName.ClientID %>").html('');
        $("#<%= lblProviderFirstName.ClientID %>").html('');
        $("#<%= lblProviderMI.ClientID %>").html('');
        $("#<%= lblAdditionalMedicaidID.ClientID %>").html('');

        document.getElementById('<%= btnAddAdditionalProInfo.ClientID%>').style.visibility = "visible";
        document.getElementById('<%= btnEditAdditionalProInfo.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnCancelUpdate.ClientID%>').style.visibility = "hidden";
        var table = localStorage.getItem("AdditionalProviderInformationTable");
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= divAdditionalProviderInfo.ClientID %>').innerHTML = table;
            }
        return false;
    }
    function ValidationsforAdditionalProviderInfo() {
        $("[id*=lblErrorMsgAdditional]").text('');
        var Service_Line = $("#<%=ddlAdditonalProviderDetail.ClientID %> option:selected").text();
        var ProviderType = $("#<%=ddlProviderType.ClientID %> option:selected").text();
        var ProviderNPI = $("#<%= txtProviderNPI.ClientID %>").first().val();
        var MedicaidId = $("#<%= lblAdditionalMedicaidID.ClientID %>").first().text();
        if (Service_Line.trim() == undefined || Service_Line.trim() == null || Service_Line.trim() == "") {
            $("[id*=lblErrorMsgAdditional]").text('Service line is required.');
            validatefields = "false";
            return false;
        }
        if (ProviderType.trim() == undefined || ProviderType.trim() == null || ProviderType.trim() == "") {
            $("[id*=lblErrorMsgAdditional]").text('Provider type is required.');
            validatefields = "false";
            return false;
        }
        if (ProviderNPI.trim() == undefined || ProviderNPI.trim() == null || ProviderNPI.trim() == "") {
            if (MedicaidId.trim() == undefined || MedicaidId.trim() == null || MedicaidId.trim() == "") {
                $("[id*=lblErrorMsgAdditional]").text('NPI or Medicaid ID is required.');
                validatefields = "false";
                return false;
            }
        }
    }
    function ddlAdditonalProviderDetail_SelectedIndexChanged() {

        var Claim_ID = $("#<%= hdnClaimIdAdditional.ClientID %>").first().val();
       var ClaimType = $("#<%= hdnClaimType_Additional.ClientID %>").first().val();
        var ddlServiceLine = $("#<%=ddlAdditonalProviderDetail.ClientID %> option:selected").text();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "ddlAdditonalProviderDetail_SelectedIndexChanged?Claim_ID=" + Claim_ID + "&&ClaimType=" + ClaimType + "&&ddlServiceLine=" + ddlServiceLine+"",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                var ddResult = result;
                $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlProviderType").empty();
                $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlProviderType").append($('<option value=""></option>'));
                for (var i = 0; i < ddResult.length; i++) {
                    var newOption = "<option value='" + ddResult[i].ID + "'>" + ddResult[i].Desc + "</option>";
                    $("#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlProviderType").append(newOption);
                }
                $("[id*=lblErrorAdditionalPanal]").text('');
            },
            error: function (jqXHR, textStatus, errorThrown) {
                  $("[id*=lblErrorAdditionalPanal]").text('try again later..');
            }
        });
    }
    function AddAdditionProviderInformationData() {
         validatefields = "true";
        $("[id*=lblErrorMsgAdditional]").text('');
        $("[id*=lblErrorAdditionalPanal]").text('');
        var additionalProviderInformationdata;
        var ServiceLine = $("#<%=ddlAdditonalProviderDetail.ClientID %> option:selected").text();
        var ProviderType = $("#<%= ddlProviderType.ClientID %> option:selected").text();
        var ProviderNPI = $("#<%= txtProviderNPI.ClientID %>").first().val();
        var MedicaidId = $("#<%= lblAdditionalMedicaidID.ClientID %>").first().text();
        var LastName = $("#<%= lblProviderLastName.ClientID %>").first().text();
        var FirstName = $("#<%= lblProviderFirstName.ClientID %>").first().text();
        var MiddleName = $("#<%= lblProviderMI.ClientID %>").first().text();
        var Claim_ID = $("#<%= hdnClaimIdAdditional.ClientID %>").first().val();
        var ClaimType = $("#<%= hdnClaimType_Additional.ClientID %>").first().val();

        var BillingNPI = $("#<%= hdnAdditionalBillingNPI.ClientID %>").first().val();


        var hdnRenderingProviderNPI = $("#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI").val();
        var hdnReferringProviderNPI = $("#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI").val();
        var hdnAssistantProviderNPI = $("#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_txtAssistantSurgeonNPI").val();
        var hdnSupervisingProviderNPI = $("#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_txtSupervisingProviderNPI").val();
        var hdnServiceFacilityProviderNPI = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_txtNPIServiceFacilityLocation").val();
        var hdnPrimaryProviderNPI = $("#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI").val();

        var hdnReferringProvider_Medicaid = $("#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_hdnRefMedId").val();
        var hdnPrimaryProvider_Medicaid = $("#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_hdnPrimaryRefMedId").val();
        var hdnRenderingProvider_MedicaidID = $("#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_hdnRendMedId").val();
        var hdnAssistant_MedicaidID = $("#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_hdnAssistSurMedId").val();
        var hdnSupervisingProvider_Medicaid = $("#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_hdnMedicaidSuper").val();

        var hdnServiceFacilityProvider_Medicaid = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_hdnServiceFacilityMedId").val();
        ValidationsforAdditionalProviderInfo();
        if (validatefields === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            // alert(webApiClaims + "AddAdditionProviderInformationData?ServiceLine=" + ServiceLine + "&&ProviderType=" + ProviderType + "&&ProviderNPI=" + ProviderNPI + "&&MedicaidId=" + MedicaidId + "&&LastName=" + LastName + "&&FirstName=" + FirstName + "&&MiddleName=" + MiddleName + "&&Claim_ID=" + Claim_ID + "&&ClaimType=" + ClaimType + "&&BillingNPI=" + BillingNPI + "&&hdnRenderingProviderNPI=" + hdnRenderingProviderNPI + "&&hdnRenderingProvider_MedicaidID=" + hdnRenderingProvider_MedicaidID + "&&hdnAssistantProviderNPI=" + hdnAssistantProviderNPI + "&&hdnAssistant_MedicaidID=" + hdnAssistant_MedicaidID + "&&hdnSupervisingProviderNPI=" + hdnSupervisingProviderNPI + "&&hdnSupervisingProvider_Medicaid=" + hdnSupervisingProvider_Medicaid + "&&hdnServiceFacilityProviderNPI=" + hdnServiceFacilityProviderNPI + "&&hdnServiceFacilityProvider_Medicaid=" + hdnServiceFacilityProvider_Medicaid + "&&hdnReferringProviderNPI=" + hdnReferringProviderNPI + "&&hdnReferringProvider_Medicaid=" + hdnReferringProvider_Medicaid + "&&hdnPrimaryProviderNPI=" + hdnPrimaryProviderNPI + "hdnPrimaryProvider_Medicaid" + hdnPrimaryProvider_Medicaid + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",);
            $.ajax({
                type: "POST",
                url: webApiClaims + "AddAdditionProviderInformationData?ServiceLine=" + ServiceLine + "&&ProviderType=" + ProviderType + "&&ProviderNPI=" + ProviderNPI + "&&MedicaidId=" + MedicaidId + "&&LastName=" + LastName + "&&FirstName=" + FirstName + "&&MiddleName=" + MiddleName + "&&Claim_ID=" + Claim_ID + "&&ClaimType=" + ClaimType + "&&BillingNPI=" + BillingNPI + "&&hdnRenderingProviderNPI=" + hdnRenderingProviderNPI + "&&hdnRenderingProvider_MedicaidID=" + hdnRenderingProvider_MedicaidID + "&&hdnAssistantProviderNPI=" + hdnAssistantProviderNPI + "&&hdnAssistant_MedicaidID=" + hdnAssistant_MedicaidID + "&&hdnSupervisingProviderNPI=" + hdnSupervisingProviderNPI + "&&hdnSupervisingProvider_Medicaid=" + hdnSupervisingProvider_Medicaid + "&&hdnServiceFacilityProviderNPI=" + hdnServiceFacilityProviderNPI + "&&hdnServiceFacilityProvider_Medicaid=" + hdnServiceFacilityProvider_Medicaid + "&&hdnReferringProviderNPI=" + hdnReferringProviderNPI + "&&hdnReferringProvider_Medicaid=" + hdnReferringProvider_Medicaid + "&&hdnPrimaryProviderNPI=" + hdnPrimaryProviderNPI + "hdnPrimaryProvider_Medicaid" + hdnPrimaryProvider_Medicaid + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                //data: '{ServiceLine: "' + ServiceLine + '" , ProviderType: "' + ProviderType + '" , ProviderNPI: "' + ProviderNPI + '", MedicaidId: "' + MedicaidId + '", LastName: "' + LastName + '", FirstName: "' + FirstName + '", MiddleName: "' + MiddleName + '", Claim_ID: "' + Claim_ID + '" ,ClaimType:"' + ClaimType + '",BillingNPI:"' + BillingNPI + '",hdnRenderingProviderNPI:"' + hdnRenderingProviderNPI + '",hdnRenderingProvider_MedicaidID:"' + hdnRenderingProvider_MedicaidID + '",hdnAssistantProviderNPI:"' + hdnAssistantProviderNPI + '",hdnAssistant_MedicaidID:"' + hdnAssistant_MedicaidID + '",hdnSupervisingProviderNPI:"' + hdnSupervisingProviderNPI + '",hdnSupervisingProvider_Medicaid:"' + hdnSupervisingProvider_Medicaid + '",hdnServiceFacilityProviderNPI:"' + hdnServiceFacilityProviderNPI + '",hdnServiceFacilityProvider_Medicaid:"' + hdnServiceFacilityProvider_Medicaid + '",hdnReferringProviderNPI:"' + hdnReferringProviderNPI + '",hdnReferringProvider_Medicaid:"' + hdnReferringProvider_Medicaid + '",hdnPrimaryProviderNPI:"' + hdnPrimaryProviderNPI + '",hdnPrimaryProvider_Medicaid:"' + hdnPrimaryProvider_Medicaid + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    additionalProviderInformationdata = result;
                    var table;
                    var ErrorMsg = "";
                    if (additionalProviderInformationdata.length > 0) {
                        if (ClaimType != "2")
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Provider Type</th><th style='width:10px; scope='col'>Provider NPI</th><th style='width:10px; scope='col'>Last Name</th><th style='width:10px; scope='col'>First Name</th><th style='width:30px; scope='col'>Middle Name</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        else
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Provider Type</th><th style='width:10px; scope='col'>Provider NPI</th><th style='width:10px; scope='col'>Medicaid ID</th><th style='width:10px; scope='col'>Last Name</th><th style='width:10px; scope='col'>First Name</th><th style='width:30px; scope='col'>Middle Name</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                        for (var i = 0; i < additionalProviderInformationdata.length; i++) {
                            var Error = result[i].Error_Msg;
                            ErrorMsg = Error;
                        }
                        if (Error == null || Error == undefined || Error == "") {
                            for (var i = 0; i < additionalProviderInformationdata.length; i++) {
                                var ServiceLine = result[i].ServiceLine;
                                var ProviderType = result[i].ProviderType;
                                var ProviderNPI = result[i].ProviderNPI;
                                var LastName = result[i].LastName;
                                var FirstName = result[i].FirstName;
                                var MiddleName = result[i].MiddleName;
                                var ClaimID = result[i].ClaimID;
                                var MedicaidID = "";
                                if (ClaimType == "2")
                                    MedicaidID = result[i].MedicaidID;
                                if (MedicaidID == 'undefined' || MedicaidID == "" || MedicaidID == null)
                                    MedicaidID = "";
                                var Claims_Additional_Provider_Information_Service_ID = result[i].Claims_Additional_Provider_Information_Service_ID;
                                $("[id*=hdnAdditionalGridSearchRowID]").val("" + Claims_Additional_Provider_Information_Service_ID + "");

                                if (ClaimType != "2")
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='ServiceLine' class='tNumber'>" + ServiceLine + "</span></td><td><span title='ProviderType' class='tNumber'>" + ProviderType + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + ProviderNPI + "</span></td><td><span title='LastName' class='tNumber'>" + LastName + "</span></td><td><span title='FirstName' class='tNumber'>" + FirstName + "</span></td><td><span title='MiddleName' class='tNumber'>" + MiddleName + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                       
                                else
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='ServiceLine' class='tNumber'>" + ServiceLine + "</span></td><td><span title='ProviderType' class='tNumber'>" + ProviderType + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + ProviderNPI + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + MedicaidID + "</span></td><td><span title='LastName' class='tNumber'>" + LastName + "</span></td><td><span title='FirstName' class='tNumber'>" + FirstName + "</span></td><td><span title='MiddleName' class='tNumber'>" + MiddleName + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        };

                        table = table + "</tbody></table>";
                        localStorage.setItem("AdditionalProviderInformationTable", "" + table + "");
                        document.getElementById('<%= divAdditionalProviderInfo.ClientID %>').innerHTML = table;
                        ClearAdditionalProviderInfo();
                    }
                    else {
                        $("[id*=lblErrorAdditionalPanal]").text(ErrorMsg);
                        ClearAdditionalProviderInfo();
                    }
                }
                else {
                    ClearAdditionalProviderInfo();
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorAdditionalPanal]").text('');
                ClearAdditionalProviderInfo();
            }
        });
        }
        return false;
    }
    function EditAdditionalProviderInfo(Claims_Additional_Provider_Information_Service_ID, ClaimID, ClaimType, control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        var EditAdditionalProData;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "PUT",
            url: webApiClaims + "EditAdditionalProviderInfo?Claims_Additional_Provider_Information_Service_ID=" + Claims_Additional_Provider_Information_Service_ID + "&&Claim_ID=" + ClaimID + "&&ClaimType=" + ClaimType,

            //data: '{Claims_Additional_Provider_Information_Service_ID:"' + Claims_Additional_Provider_Information_Service_ID + '",Claim_ID:"' + ClaimID + '",ClaimType:"' + ClaimType + '"}',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                EditAdditionalProData = result;
                if (EditAdditionalProData.length > 0) {
                    for (var i = 0; i < EditAdditionalProData.length; i++) {
                        var ServiceLine = result[i].ServiceLine;
                        var ProviderType = result[i].ProviderType;
                        var ProviderNPI = result[i].ProviderNPI;
                        var LastName = result[i].LastName;
                        var FirstName = result[i].FirstName;
                        var MiddleName = result[i].MiddleName;
                        var ClaimID = result[i].ClaimID;
                        var MedicaidID = "";
                        if (ClaimType == "2")
                            MedicaidID = result[i].MedicaidID;
                        if (MedicaidID == 'undefined' || MedicaidID == "" || MedicaidID == null)
                            MedicaidID = "";
                        var Claims_Additional_Provider_Information_Service_ID = result[i].Claims_Additional_Provider_Information_Service_ID;

                        //call edit providertype load
                        ddlAdditonalProvidertype_onEditButtonClick(ClaimID, ClaimType, ServiceLine, ProviderType);

                      
                        $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlAdditonalProviderDetail option:contains("' + ServiceLine + '")').prop('selected', true);
                        $("[id*=hdnAdditionalGridSearchRowID]").val("" + Claims_Additional_Provider_Information_Service_ID + "");
                        $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_ddlProviderType option:contains("' + ProviderType + '")').prop('selected', true);
                        $("[id*=txtProviderNPI]").val("" + ProviderNPI + "");
                        if (ClaimType == "2") {
                                $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_lblAdditionalMedicaidID').text(MedicaidID);
                        }
                        $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_lblProviderLastName').text(LastName);
                        $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_lblProviderFirstName').text(FirstName);
                        $('#ctl00_MainContent_uc5SubmitClaim_ucAdditionalProviderInfoPanel_lblProviderMI').text(MiddleName);
                       
                        $("[id*=hdnAdditionalGridSearchRowID]").val("" + Claims_Additional_Provider_Information_Service_ID + "");
                        $("[id*=hdnServiceLine_Additional]").val("" + ServiceLine + "");
                        $("[id*=hdnProviderType_Additional]").val("" + ProviderType + "");
                        
                        $("[id*=hdnAdditionalFirstName]").val("" + FirstName + "");
                        $("[id*=hdnAdditionalLastName]").val("" + LastName + "");
                        document.getElementById('<%= btnAddAdditionalProInfo.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnEditAdditionalProInfo.ClientID%>').style.visibility = "visible";                       
                        document.getElementById('<%= btnCancelUpdate.ClientID%>').style.visibility = "visible"; 
                        $("[id*=lblErrorAdditionalPanal]").text('');
                        return false;
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblErrorAdditionalPanal]").text('');
                ClearAdditionalProviderInfo();
            }
        });
    }
    function DeleteAdditionalProviderInfo(Claims_Additional_Provider_Information_Service_ID, ClaimID, ClaimType) {
        
        var AdditionalProviderInfoData;
        var result1 = confirm("Are you sure you want to delete?");
        if (result1) {
            var Claim_ID = $("#<%= hdnClaimIdAdditional.ClientID %>").val();
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteAdditionalProviderInfo?Claim_ID=" + Claim_ID + "&&Claims_Additional_Provider_Information_Service_ID=" + Claims_Additional_Provider_Information_Service_ID + "&&ClaimType=" + ClaimType,
                //data: '{Claim_ID: "' + Claim_ID + '" , Claims_Additional_Provider_Information_Service_ID: "' + Claims_Additional_Provider_Information_Service_ID + '",ClaimType:"' + ClaimType+'" }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    AdditionalProviderInfoData = result;
                    if (AdditionalProviderInfoData.length > 0) {
                        var table;
                        if (ClaimType != "2")
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Provider Type</th><th style='width:10px; scope='col'>Provider NPI</th><th style='width:10px; scope='col'>Last Name</th><th style='width:10px; scope='col'>First Name</th><th style='width:30px; scope='col'>Middle Name</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        else
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Provider Type</th><th style='width:10px; scope='col'>Provider NPI</th><th style='width:10px; scope='col'>Medicaid ID</th><th style='width:10px; scope='col'>Last Name</th><th style='width:10px; scope='col'>First Name</th><th style='width:30px; scope='col'>Middle Name</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                        for (var i = 0; i < AdditionalProviderInfoData.length; i++) {
                            var ServiceLine = result[i].ServiceLine;
                            var ProviderType = result[i].ProviderType;
                            var ProviderNPI = result[i].ProviderNPI;
                            var LastName = result[i].LastName;
                            var FirstName = result[i].FirstName;
                            var MiddleName = result[i].MiddleName;
                            var ClaimID = result[i].ClaimID;
                            var MedicaidID = "";
                            if (ClaimType == "2")
                                MedicaidID = result[i].MedicaidID;
                            if (MedicaidID == 'undefined' || MedicaidID == "" || MedicaidID == null)
                                MedicaidID = "";
                            var Claims_Additional_Provider_Information_Service_ID = result[i].Claims_Additional_Provider_Information_Service_ID;
                            <%--$("#<%=ddlAdditonalProviderDetail.ClientID %> option[value='" + ServiceLine + "']").remove();--%>

                            if (ClaimType != "2")
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='ServiceLine' class='tNumber'>" + ServiceLine + "</span></td><td><span title='ProviderType' class='tNumber'>" + ProviderType + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + ProviderNPI + "</span></td><td><span title='LastName' class='tNumber'>" + LastName + "</span></td><td><span title='FirstName' class='tNumber'>" + FirstName + "</span></td><td><span title='MiddleName' class='tNumber'>" + MiddleName + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                            else
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='ServiceLine' class='tNumber'>" + ServiceLine + "</span></td><td><span title='ProviderType' class='tNumber'>" + ProviderType + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + ProviderNPI + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + MedicaidID + "</span></td><td><span title='LastName' class='tNumber'>" + LastName + "</span></td><td><span title='FirstName' class='tNumber'>" + FirstName + "</span></td><td><span title='MiddleName' class='tNumber'>" + MiddleName + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                        };
                        table = table + "</tbody></table>";
                        document.getElementById('<%= divAdditionalProviderInfo.ClientID %>').innerHTML = table;
                        localStorage.setItem("AdditionalProviderInformationTable", "" + table + "");
                        ClearAdditionalProviderInfo();
                        $("[id*=lblErrorAdditionalPanal]").text('');
                        return false;

                    }
                    else {
                        document.getElementById('<%= divAdditionalProviderInfo.ClientID %>').innerHTML = "";
                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorAdditionalPanal]").text('');
                    ClearAdditionalProviderInfo();
                }
            });
        }
    }
    function UpdateAdditionalProviderInfo() {
        $("[id*=lblErrorAdditionalPanal]").text('');
        var additionalProviderInformationdata;
        var ServiceLine = $("#<%=ddlAdditonalProviderDetail.ClientID %> option:selected").text();
        var ProviderType = $("#<%= ddlProviderType.ClientID %> option:selected").text();
        var ProviderNPI = $("#<%= txtProviderNPI.ClientID %>").first().val();
        var MedicaidId = $("#<%= lblAdditionalMedicaidID.ClientID %>").first().text();
        var LastName = $("#<%= lblProviderLastName.ClientID %>").first().text();
        var FirstName = $("#<%= lblProviderFirstName.ClientID %>").first().text();
        var MiddleName = $("#<%= lblProviderMI.ClientID %>").first().text();
        var Claim_ID = $("#<%= hdnClaimIdAdditional.ClientID %>").first().val();
        var ClaimType = $("#<%= hdnClaimType_Additional.ClientID %>").first().val();     
        var Claims_Additional_Provider_Information_Service_ID = $("#<%= hdnAdditionalGridSearchRowID.ClientID %>").first().val();

        var renderingNpi = $("#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI").val();

        var BillingNPI = $("#<%= hdnAdditionalBillingNPI.ClientID %>").first().val();
        var hdnRenderingProviderNPI = $("#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_txtRenderingProvNPI").val();
        var hdnReferringProviderNPI = $("#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtReferringProviderNPI").val();
        var hdnAssistantProviderNPI = $("#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_txtAssistantSurgeonNPI").val();
        var hdnSupervisingProviderNPI = $("#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_txtSupervisingProviderNPI").val();
        var hdnServiceFacilityProviderNPI = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_txtNPIServiceFacilityLocation").val();
        var hdnPrimaryProviderNPI = $("#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_txtPrimaryCareProviderNPI").val();

        var hdnReferringProvider_Medicaid = $("#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblReffProMedicaidID").val();
        var hdnPrimaryProvider_Medicaid = $("#ctl00_MainContent_uc5SubmitClaim_ucReferringProvider_lblPrimaryCareProvMedicaidID").val();
        var hdnRenderingProvider_MedicaidID = $("#ctl00_MainContent_uc5SubmitClaim_ucRenderingProviderInformationPanel_lblRenderingMedicaidID").val();
        var hdnAssistant_MedicaidID = $("#ctl00_MainContent_uc5SubmitClaim_ucAssistantSurgeon_lblAssistantSurgeonMedicaidID").val();
        var hdnSupervisingProvider_Medicaid = $("#ctl00_MainContent_uc5SubmitClaim_ucSupervisingProviderPanel_lblSupervisingProviderMediID").val();
        var hdnServiceFacilityProvider_Medicaid = $("#ctl00_MainContent_uc5SubmitClaim_ucServiceFacility_lblServiceFacilityLocationMedicaid").val();
        ValidationsforAdditionalProviderInfo();
        if (validatefields == "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "PUT",

                url: webApiClaims + "UpdateAdditionalProviderInfo?ServiceLine=" + ServiceLine + "&&ProviderType=" + ProviderType + "&&ProviderNPI=" + ProviderNPI + "&&MedicaidId=" + MedicaidId + "&&LastName=" + LastName + "&&FirstName=" + FirstName + "&&MiddleName=" + MiddleName + "&&Claim_ID=" + Claim_ID + "&&ClaimType=" + ClaimType + "&&Claims_Additional_Provider_Information_Service_ID=" + Claims_Additional_Provider_Information_Service_ID + "&&BillingNPI=" + BillingNPI + "&&hdnRenderingProviderNPI=" + hdnRenderingProviderNPI + "&&hdnRenderingProvider_MedicaidID=" + hdnRenderingProvider_MedicaidID + "&&hdnAssistantProviderNPI=" + hdnAssistantProviderNPI + "&&hdnAssistant_MedicaidID=" + hdnAssistant_MedicaidID + "&&hdnSupervisingProviderNPI=" + hdnSupervisingProviderNPI + "&&hdnSupervisingProvider_Medicaid=" + hdnSupervisingProvider_Medicaid + "&&hdnServiceFacilityProviderNPI=" + hdnServiceFacilityProviderNPI + "&&hdnServiceFacilityProvider_Medicaid=" + hdnServiceFacilityProvider_Medicaid + "&&hdnReferringProviderNPI=" + hdnReferringProviderNPI + "&&hdnReferringProvider_Medicaid=" + hdnReferringProvider_Medicaid + "&&hdnPrimaryProviderNPI=" + hdnPrimaryProviderNPI + "&&hdnPrimaryProvider_Medicaid=" + hdnPrimaryProvider_Medicaid + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",

                //data: '{ServiceLine: "' + ServiceLine + '" , ProviderType: "' + ProviderType + '" , ProviderNPI: "' + ProviderNPI + '", MedicaidId: "' + MedicaidId + '", LastName: "' + LastName + '", FirstName: "' + FirstName + '", MiddleName: "' + MiddleName + '", Claim_ID: "' + Claim_ID + '" ,ClaimType:"' + ClaimType + '",Claims_Additional_Provider_Information_Service_ID:"' + Claims_Additional_Provider_Information_Service_ID + '",BillingNPI:"' + BillingNPI + '",hdnRenderingProviderNPI:"' + hdnRenderingProviderNPI + '",hdnRenderingProvider_MedicaidID:"' + hdnRenderingProvider_MedicaidID + '",hdnAssistantProviderNPI:"' + hdnAssistantProviderNPI + '",hdnAssistant_MedicaidID:"' + hdnAssistant_MedicaidID + '",hdnSupervisingProviderNPI:"' + hdnSupervisingProviderNPI + '",hdnSupervisingProvider_Medicaid:"' + hdnSupervisingProvider_Medicaid + '",hdnServiceFacilityProviderNPI:"' + hdnServiceFacilityProviderNPI + '",hdnServiceFacilityProvider_Medicaid:"' + hdnServiceFacilityProvider_Medicaid + '",hdnReferringProviderNPI:"' + hdnReferringProviderNPI + '",hdnReferringProvider_Medicaid:"' + hdnReferringProvider_Medicaid + '",hdnPrimaryProviderNPI:"' + hdnPrimaryProviderNPI + '",hdnPrimaryProvider_Medicaid:"' + hdnPrimaryProvider_Medicaid + '" }',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    additionalProviderInformationdata = result;
                    var table;
                    if (additionalProviderInformationdata.length > 0) {
                        if (ClaimType != "2")
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Provider Type</th><th style='width:10px; scope='col'>Provider NPI</th><th style='width:10px; scope='col'>Last Name</th><th style='width:10px; scope='col'>First Name</th><th style='width:30px; scope='col'>Middle Name</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        else
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Provider Type</th><th style='width:10px; scope='col'>Provider NPI</th><th style='width:10px; scope='col'>Medicaid ID</th><th style='width:10px; scope='col'>Last Name</th><th style='width:10px; scope='col'>First Name</th><th style='width:30px; scope='col'>Middle Name</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        for (var i = 0; i < additionalProviderInformationdata.length; i++) {

                            var Error = result[i].Error_Msg;
                            ErrorMsg = Error;
                        }
                        if (Error == null || Error == undefined || Error == "") {
                            for (var i = 0; i < additionalProviderInformationdata.length; i++) {
                                var ServiceLine = result[i].ServiceLine;
                                var ProviderType = result[i].ProviderType;
                                var ProviderNPI = result[i].ProviderNPI;
                                var LastName = result[i].LastName;
                                var FirstName = result[i].FirstName;
                                var MiddleName = result[i].MiddleName;
                                var ClaimID = result[i].ClaimID;
                                <%--$("#<%=ddlAdditonalProviderDetail.ClientID %> option[value='" + ServiceLine + "']").remove();--%>
                                var MedicaidID = "";
                                if (ClaimType == "2")
                                    MedicaidID = result[i].MedicaidID;
                                if (MedicaidID == 'undefined' || MedicaidID == "" || MedicaidID == null)
                                    MedicaidID = "";
                                var Claims_Additional_Provider_Information_Service_ID = result[i].Claims_Additional_Provider_Information_Service_ID;
                                $("[id*=hdnAdditionalGridSearchRowID]").val("" + Claims_Additional_Provider_Information_Service_ID + "");
                                if (ClaimType != "2")
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='ServiceLine' class='tNumber'>" + ServiceLine + "</span></td><td><span title='ProviderType' class='tNumber'>" + ProviderType + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + ProviderNPI + "</span></td><td><span title='LastName' class='tNumber'>" + LastName + "</span></td><td><span title='FirstName' class='tNumber'>" + FirstName + "</span></td><td><span title='MiddleName' class='tNumber'>" + MiddleName + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                                else
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='ServiceLine' class='tNumber'>" + ServiceLine + "</span></td><td><span title='ProviderType' class='tNumber'>" + ProviderType + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + ProviderNPI + "</span></td><td><span  title='ProviderNPI' class='tNumber'>" + MedicaidID + "</span></td><td><span title='LastName' class='tNumber'>" + LastName + "</span></td><td><span title='FirstName' class='tNumber'>" + FirstName + "</span></td><td><span title='MiddleName' class='tNumber'>" + MiddleName + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteAdditionalProviderInfo(\"" + Claims_Additional_Provider_Information_Service_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";

                            };
                            table = table + "</tbody></table>";
                            document.getElementById('<%= divAdditionalProviderInfo.ClientID %>').innerHTML = table;
                            localStorage.setItem("AdditionalProviderInformationTable", "" + table + "");
                            ClearAdditionalProviderInfo();
                            document.getElementById('<%= btnAddAdditionalProInfo.ClientID%>').style.visibility = "visible";
                            document.getElementById('<%= btnEditAdditionalProInfo.ClientID%>').style.visibility = "hidden";
                            document.getElementById('<%= btnCancelUpdate.ClientID%>').style.visibility = "hidden";  
                            $("[id*=lblErrorAdditionalPanal]").text('');
                        }
                        else {
                            $("[id*=lblErrorAdditionalPanal]").text(ErrorMsg);
                            ClearAdditionalProviderInfo();
                            document.getElementById('<%= btnAddAdditionalProInfo.ClientID%>').style.visibility = "visible";
                            document.getElementById('<%= btnEditAdditionalProInfo.ClientID%>').style.visibility = "hidden";
                            document.getElementById('<%= btnCancelUpdate.ClientID%>').style.visibility = "hidden";    
                        }
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblErrorAdditionalPanal]").text('some Error has occurred');
                    ClearAdditionalProviderInfo();
                }
            });

        }
        return false;
    }
</script>
<asp:Panel ID="pnlAdditionalProviderinfo" runat="server" Style="height: auto; width: auto;">
    <div class="divGrid" style="padding-top: 10px">

        <asp:HiddenField ID="hdnClaimIdAdditional" runat="server" />
        <asp:HiddenField ID="hdnClaimType_Additional" runat="server" />
        <asp:HiddenField ID="hdnServiceLine_Additional" runat="server" />
        <asp:HiddenField ID="hdnProviderType_Additional" runat="server" />      
        <asp:HiddenField ID="hdnAdditionalBillingNPI" runat="server" />
        <asp:HiddenField ID="hdnAdditionalFirstName" runat="server" />
        <asp:HiddenField ID="hdnAdditionalLastName" runat="server" />        

        <div id="divAdditionalProviderInfo" runat="server"></div>
     
    </div>
   
    <asp:HiddenField ID="hdnSequenceidAdditionalPanel" runat="server" />
    <asp:HiddenField ID="hdnServiceLineAdditionalPanel" runat="server" />
    <asp:HiddenField ID="hdnProvidertypeAdditionalPanel" runat="server" />
    <asp:HiddenField ID="hdnAdditionalGridSearchRowID" runat="server" />
    <asp:HiddenField ID="hdnAddtionalProviderInfoNPI" runat="server" />
    <asp:HiddenField ID="hdnAdditionalClaimStatusDental" runat="server" />
        <div class="row form-group" >
            <asp:Label ID="lblErrorMsgAdditional" runat="server" ForeColor="Red" Style="margin-left: 40px;"></asp:Label>
        </div>
    <div class="row" style="text-align: center; padding-left: 20px; padding-right: 20px;" runat="server" id="divAddProv">

        <div class="col-lg-1">
            <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: left; width: 100px; min-width: 100px;">Service Line</span>
            <div style="text-align: center;">
                <asp:DropDownList ID="ddlAdditonalProviderDetail"  runat="server" 
                    AppendDataBoundItems="True" Style="width: 100px; min-width: 100px;" CssClass="formField" class="selectdropdown" onChange="return ddlAdditonalProviderDetail_SelectedIndexChanged()" />  
                <br />
                
         
                <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" SetFocusOnError="true"
                    ValidationGroup="valAddProvider" ControlToValidate="ddlAdditonalProviderDetail" CssClass="failureNotification"
                    ErrorMessage="*" Text="Service line is required" Display="Dynamic"  />--%>

            </div>
        </div>
        <div class="col-lg-2" style="padding-left:50px">
            <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: left">Provider Type</span>
            <div style="text-align: left;">
                <asp:DropDownList ID="ddlProviderType" CssClass="formField" EnableViewState="true" runat="server" ValidationGroup="valAddProvider"
                    AppendDataBoundItems="True" Style="width: 120px; min-width: 120px;" class="selectdropdown">
                </asp:DropDownList>
                <br />
               <%-- <asp:RequiredFieldValidator runat="server" ID="rvProviderType" SetFocusOnError="true"
                    ValidationGroup="valAddProvider" ControlToValidate="ddlProviderType" Text="Provider type is required"
                    Display="Dynamic" CssClass="failureNotification" />--%>

            </div>
        </div>
        <div class="col-lg-2">
            <span class="ohio-field GridviewHeaderAsterisk" style="font-size: 15px; text-align: left">Provider NPI</span>
            <div style="text-align: left;">
                <asp:TextBox ID="txtProviderNPI" runat="server" OnChange ="return txtAdditionalProviderNPI_TextChangedNPI(this)" CssClass="formfieldDate" Style="height: 30px; width: 120px; min-width: 120px;" MaxLength="10"  />
                 <%--<button id="btnloading1" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>--%>
         
                <button type="button" id="btnSearchAdditional" onclick="getbuttondetail(this)" class="btn btn-link" data-toggle="modal" data-target="#myModal">
                    Search
                </button>
                <br />
              <%--  <asp:RequiredFieldValidator ID="rfvProviderNPI" runat="server" ControlToValidate="txtProviderNPI" Text="Npi is required" CssClass="failureNotification"
                     Display="Dynamic" ValidationGroup="valAddProvider"></asp:RequiredFieldValidator>--%>
         <%--       <asp:RegularExpressionValidator runat="server" ID="revProviderNPI"
                    Display="Dynamic"
                    ValidationGroup="valAddProvider"
                    ControlToValidate="txtProviderNPI"
                    ValidationExpression="^[0-9]{10}$" Text=" *Provider NPI 10-digit number is required" CssClass="failureNotification"
                    ErrorMessage="<div id='msg24_rev_txtProviderNPI' > *Provider NPI 10-digit number is required</div>">
                </asp:RegularExpressionValidator>--%>
                <asp:CustomValidator ID="cvProviderNPI" runat="server" ControlToValidate="txtProviderNPI" ErrorMessage="Provider NPI required for detail N in additional provider panel" Display="None" ValidationGroup="validateAdditionalProviderNPI" />



                <asp:Label ID="errProviderNPI" Style="color: red" runat="server"></asp:Label>
            </div>
        </div>
        <div class="col-lg-1" runat="server" visible="false" id="divAdditionalMedicaidIDProf">
            <span class="ohio-field" style="font-size: 15px; text-align: left">Medicaid ID</span>
            <div style="text-align: left;">
                <asp:Label ID="lblAdditionalMedicaidID" runat="server" MaxLength="10" />
            </div>
        </div>
        <div class="col-lg-2">
            <span class="ohio-field" style="font-size: 15px; text-align: left">Last Name</span>
            <div style="text-align: left;">
                <asp:Label ID="lblProviderLastName" runat="server"  />
            </div>
        </div>
        <div class="col-lg-2">
            <span class="ohio-field" style="font-size: 15px; text-align: left">First Name</span>
            <div style="text-align: left;">
                <asp:Label ID="lblProviderFirstName" runat="server" />
            </div>
        </div>
        <div class="col-lg-1">
            <span class="ohio-field" style="font-size: 15px; text-align: left">Middle Name</span>
            <div style="text-align: left;">
                <asp:Label ID="lblProviderMI" runat="server"  />
            </div>
        </div>

        <div class="col-lg-1">
            <span class="ohio-field" style="font-size: 15px; text-align: left">&nbsp;</span>
            <div style="text-align: left;">
                  <%--<button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>--%>
                                  
                <asp:Button ID="btnAddAdditionalProInfo" Text="ADD" OnClientClick="return AddAdditionProviderInformationData()" runat="server" CssClass="btn btn-primary" Font-Bold="True" Width="70px" />
                <asp:Button ID="btnEditAdditionalProInfo" Style="visibility: hidden" Text="Update"  OnClientClick="return UpdateAdditionalProviderInfo()"  runat="server" CssClass="btn btn-primary" Font-Bold="True" Width="70px" />
                <br />
                <asp:Button ID="btnCancelUpdate" Style="visibility: hidden" Text="Cancel" OnClientClick="return ClearAdditionalProviderInfo();" runat="server" CssClass="btn btn-danger" Font-Bold="True" ValidationGroup="valAddProvider" Width="70px" />
            </div>
        </div>
    </div>
    <div class="form-group row">
        <div>
            <asp:Label ID="lblErrorAdditionalPanal" runat="server" ForeColor="Red" Style="margin-left: 40px;"></asp:Label>
        </div>
        <br />
    </div>
</asp:Panel>
