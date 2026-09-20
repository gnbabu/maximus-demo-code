<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_NDCDetails, App_Web_wbqq1lcm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/SubmitClaimSearchNDC.ascx" TagPrefix="uc" TagName="SubmitClaimSearchNDC" %>
 <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
        <meta name="viewport" content="width=device-width, initial-scale=1">

<script type="text/javascript">
    var TotalCountinNDCDetailsGrid;
    function CancelNDCCodeDetails() {
        $("[id*=lblNDCDetailsmessage]").text('');
        $("#<%=ddlNDCServiceLine.ClientID %> option:selected").text('');
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ddlUnitOfMeasure") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ddlUnitOfMeasure").options[0].selected = true;
        }
        $("#<%= txtNDCCode.ClientID %>").first().val("");
        $("#<%= txtTotalUnit.ClientID %>").first().val("");
        $("#<%= txtPrescriptionNumber.ClientID %>").first().val("");

        if (TotalCountinNDCDetailsGrid > 30) {
            document.getElementById('<%= btnNDCAddDetails.ClientID%>').style.visibility = "hidden";
        }
        else {
            document.getElementById('<%= btnNDCAddDetails.ClientID%>').style.visibility = "visible";
        }
        document.getElementById('<%= btnCancelNDCDetails.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnEditNDCDetails.ClientID%>').style.visibility = "hidden";
        var table = localStorage.getItem("NDCDetailsTable");
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= divNDCDetails.ClientID %>').innerHTML = table;
        }
        return false;
    }
    function ndcPanelBind() {
        var ClaimID = $("#<%= hdnNDC_ClaimID.ClientID %>").first().val();
        var ClaimType = $("#<%= hdnNDC_ClaimType.ClientID %>").first().val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "getNDCDetailsData?ClaimID=" + ClaimID + "&&ClaimType=" + ClaimType,
            //data: '{ClaimID: "' + ClaimID + '" ,ClaimType:"' + ClaimType + '" }',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var NDCdetailsData;
                NDCdetailsData = result;
                var table;
                if (NDCdetailsData.length > 0) {
                    table = '<table class=\"gridview\"  cellspacing=\"0\" align=\"Middle\" rules=\"rows\" border=\"1\" style=\"margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;\"><tbody><tr class=\"gridViewHeader\"><th scope=\"col\">Service Line</th><th scope=\"col\">NDC</th><th scope=\"col\">Unit Of Measure</th><th scope=\"col\">Prescription Number</th><th scope=\"col\">Total Unit</th><th scope=\"col\">&nbsp;</th><th scope=\"col\">&nbsp;</th></tr>';
                    var Count = 0;
                    for (var i = 0; i < NDCdetailsData.length; i++) {
                        Count++;
                        var ServiceLine = result[i].ServiceLine;
                        var NDCCode = result[i].NDCCode;
                        var UnitMeasure = result[i].UnitMeasure;
                        var PrescriptionNuber = result[i].PrescriptionNuber;
                        var TotalUnit = result[i].TotalUnit;
                        var Claims_NDC_Details_Screen_ID = result[i].Claims_NDC_Details_Screen_ID;
                        var Claim_ID = result[i].Claim_ID;

                        TotalCountinNDCDetailsGrid = Count;
                        if (Count > 30) { document.getElementById('<%= btnNDCAddDetails.ClientID%>').style.visibility = "hidden"; }
                            else { document.getElementById('<%= btnNDCAddDetails.ClientID%>').style.visibility = "visible"; }
                            table = table + '<tr style=\"border: 1px solid black; border-collapse: collapse;\" class=\"gridViewRow\"><td><span title=\"Service Line\" class=\"tNumber\">' + ServiceLine + '</span></td><td><span  title=\"NDC\" class=\"tNumber\">' + NDCCode + '</span></td><td><span title=\"Unit Of Measure\" class=\"tNumber\">' + UnitMeasure + '</span></td><td><span title=\"Prescription Number\">' + PrescriptionNuber + '</span></td><td><span title=\"Total Unit\" class=\"tNumber\">' + TotalUnit + '</span></td><td><input type=\"button\" value = \"Edit\" onClick = \"return EditNDCDetailsItem(\'' + Claims_NDC_Details_Screen_ID + '\',\'' + Claim_ID + '\',this); return true;\" class=\"btn btn-primary\" sytle = \"margin-left:3px\"></td><td><input type=\"button\" value=\"Delete\" onclick=\"return DeleteNDCDetailsItem(\'' + Claims_NDC_Details_Screen_ID + '\');\" class=\"btn btn-danger\" sytle=\"margin-left:3px\"></td></tr >';
                        };
                        table = table + "</tbody></table>";
                        localStorage.setItem("NDCDetailsTable", "" + table + "");
                     document.getElementById('<%= divNDCDetails.ClientID %>').innerHTML = table;                     
                     GetServiceLineNoForNDC();
                 }
                else {
                    document.getElementById('<%= divNDCDetails.ClientID %>').innerHTML = "";  
                 }
             },
             error: function (jqXHR, textStatus, errorThrown) {
                 $("[id*=lblNDCDetailsmessage]").text('Something Went Wrong..Try again');
             }
         });

    }

    function AddNDCDetailsData() {
        var NDCdetailsData;
        $("[id*=lblNDCDetailsmessage]").text('');
        var validatedata = "";
        var serviceline = $("#<%=ddlNDCServiceLine.ClientID %> option:selected").text();
        var NDCCode = $("#<%= txtNDCCode.ClientID %>").first().val();
        var totalUnit = $("#<%= txtTotalUnit.ClientID %>").first().val();
        var PrescriptionNumber = $("#<%= txtPrescriptionNumber.ClientID %>").first().val();
        var UnitMeasure = $("#<%= ddlUnitOfMeasure.ClientID %>").first().val();
        var ClaimID = $("#<%= hdnNDC_ClaimID.ClientID %>").first().val();
        var ClaimType = $("#<%= hdnNDC_ClaimType.ClientID %>").first().val();
        var hdnNDC_Code = $("#<%= hdnNDC_Code.ClientID %>").first().val();
        
        ValidateNDCDetailsForClaims();
        if (validatedata === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "POST",
                url: webApiClaims + "AddNDCDetailsData?ClaimID=" + ClaimID + "&&ClaimType=" + ClaimType + "&&hdnNDC_Code=" + hdnNDC_Code + "&&serviceline=" + serviceline + "&&NDCCode=" + NDCCode + "&&totalUnit=" + totalUnit + "&&PrescriptionNumber=" + PrescriptionNumber + "&&UnitMeasure=" + UnitMeasure + "&&userid=<%=HttpContext.Current.User.Identity.Name%>" ,
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                
                    NDCdetailsData = result;
                    var table;
                    if (NDCdetailsData.length > 0) {
                        table = '<table class=\"gridview\"  cellspacing=\"0\" align=\"Middle\" rules=\"rows\" border=\"1\" style=\"margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;\"><tbody><tr class=\"gridViewHeader\"><th scope=\"col\">Service Line</th><th scope=\"col\">NDC</th><th scope=\"col\">Unit Of Measure</th><th scope=\"col\">Prescription Number</th><th scope=\"col\">Total Unit</th><th scope=\"col\">&nbsp;</th><th scope=\"col\">&nbsp;</th></tr>';
                        var Count = 0;
                        for (var i = 0; i < NDCdetailsData.length; i++) {
                            Count++;
                            var ServiceLine = result[i].ServiceLine;
                            var NDCCode = result[i].NDCCode;
                            var UnitMeasure = result[i].UnitMeasure;
                            var PrescriptionNuber = result[i].PrescriptionNuber;
                            var TotalUnit = result[i].TotalUnit;
                            var Claims_NDC_Details_Screen_ID = result[i].Claims_NDC_Details_Screen_ID;
                            var Claim_ID = result[i].Claim_ID;

                            TotalCountinNDCDetailsGrid = Count;
                            if (Count > 30) { document.getElementById('<%= btnNDCAddDetails.ClientID%>').style.visibility = "hidden"; }
                            else { document.getElementById('<%= btnNDCAddDetails.ClientID%>').style.visibility = "visible"; }
                            table = table + '<tr style=\"border: 1px solid black; border-collapse: collapse;\" class=\"gridViewRow\"><td><span title=\"Service Line\" class=\"tNumber\">' + ServiceLine + '</span></td><td><span  title=\"NDC\" class=\"tNumber\">' + NDCCode + '</span></td><td><span title=\"Unit Of Measure\" class=\"tNumber\">' + UnitMeasure + '</span></td><td><span title=\"Prescription Number\">' + PrescriptionNuber + '</span></td><td><span title=\"Total Unit\" class=\"tNumber\">' + TotalUnit + '</span></td><td><input type=\"button\" value = \"Edit\" onClick=\"return EditNDCDetailsItem(\'' + Claims_NDC_Details_Screen_ID + '\',\'' + Claim_ID + '\',this); return true;\" class=\"btn btn-primary\" sytle=\"margin-left:3px\"></td><td><input type=\"button\" value=\"Delete\" onclick=\"return DeleteNDCDetailsItem(\'' + Claims_NDC_Details_Screen_ID + '\');\" class=\"btn btn-danger\" sytle=\"margin-left:3px\"></td></tr >';
                        };
                        table = table + "</tbody></table>";
                        localStorage.setItem("NDCDetailsTable", "" + table + "");
                        document.getElementById('<%= divNDCDetails.ClientID %>').innerHTML = table;
                        NDCDetailsClearFields();
                        GetServiceLineNoForNDC();
                    }
                    else {
                        NDCDetailsClearFields();
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblNDCDetailsmessage]").text('Something Went Wrong..Try again');
                }
            });
        }
        
        return false;
        function ValidateNDCDetailsForClaims() {
         
            $("[id*=lblddlNDCServiceLine]").text('');
            $("[id*=lbltxtNDCCode]").text('');
            $("[id*=lblddlUnitOfMeasure]").text('');
            $("[id*=lbltxtTotalUnit]").text('');
            if (serviceline.trim() == null || serviceline.trim() == undefined || serviceline.trim() == "") {
                validatedata = "false";
                $("[id*=lblddlNDCServiceLine]").text('Service Line Number is required');
                return false;
            }
            else {
                validatedata = "true";
                $("[id*=lblddlNDCServiceLine]").text('');
            }
            if (NDCCode.trim() == "" || NDCCode.trim() == undefined || NDCCode.trim() == null) {
                validatedata = "false";
                $("[id*=lbltxtNDCCode]").text('NDC is required');
                return false;
            }
            else {
                validatedata = "true";
                $("[id*=lbltxtNDCCode]").text('');
            }
            if (UnitMeasure.length == 0 || UnitMeasure == "" || UnitMeasure == undefined || UnitMeasure == null) {
                validatedata = "false";
                $("[id*=lblddlUnitOfMeasure]").text('Unit Of Measure is required');
                return false;
            }
            else {
                validatedata = "true";
                $("[id*=lblddlUnitOfMeasure]").text('');
            }
            if (totalUnit == "" || totalUnit == undefined || totalUnit == null) {
                validatedata = "false";
                $("[id*=lbltxtTotalUnit]").text('Total Unit is required');
                return false;
            }
            else {
                validatedata = "true";
                $("[id*=lbltxtTotalUnit]").text('');
            }
            //validations on business logics
            if (NDCCode.length != 11) {
                $("[id*=txtNDCCode]").val('');
                validatedata = "false";
                $("[id*=lbltxtNDCCode]").text('11 digits NDC Code is required');
                return false;
            }
            if (parseInt(totalUnit) < 1) {
                validatedata = "false";
                $("[id*=lbltxtTotalUnit]").text('Total Unit should be greater than zero');
                return false;
            }
        }

    }

    function NDCDetailsClearFields() {
        $("[id*=lblNDCDetailsmessage]").text('');
        $("#<%=ddlNDCServiceLine.ClientID %> option:selected").text('');
        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ddlUnitOfMeasure") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ddlUnitOfMeasure").options[0].selected = true;
        }
        $("#<%= txtNDCCode.ClientID %>").first().val("");
        $("#<%= txtTotalUnit.ClientID %>").first().val("");
        $("#<%= txtPrescriptionNumber.ClientID %>").first().val("");
    }
    function DeleteNDCDetailsItem(Claims_NDC_Details_Screen_ID) {
        var NDCdetailsData;
        $("[id*=lblNDCDetailsmessage]").text('');
        var result = confirm("Are you sure you want to delete?");
        if (result) {

            var Claim_ID = $("#<%= hdnNDC_ClaimID.ClientID %>").val();
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiClaims + "DeleteNDCDetails?Claim_ID=" + Claim_ID + "&&Claims_NDC_Details_Screen_ID=" + Claims_NDC_Details_Screen_ID+"",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    NDCdetailsData = result;
                    if (NDCdetailsData.length > 0) {
                        var table;
                        var Count = 0;
                        table = '<table class=\"gridview\"  cellspacing=\"0\" align=\"Middle\" rules=\"rows\" border=\"1\" style=\"margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;\"><tbody><tr class=\"gridViewHeader\"><th scope=\"col\">Service Line</th><th scope=\"col\">NDC</th><th scope=\"col\">Unit Of Measure</th><th scope=\"col\">Prescription Number</th><th scope=\"col\">Total Unit</th><th scope=\"col\">&nbsp;</th><th scope=\"col\">&nbsp;</th></tr>';

                        for (var i = 0; i < NDCdetailsData.length; i++) {
                            Count++;
                            var ServiceLine = result[i].ServiceLine;
                            var NDCCode = result[i].NDCCode;
                            var UnitMeasure = result[i].UnitMeasure;
                            var PrescriptionNuber = result[i].PrescriptionNuber;
                            var TotalUnit = result[i].TotalUnit;
                            var Claims_NDC_Details_Screen_ID = result[i].Claims_NDC_Details_Screen_ID;
                            var Claim_ID = result[i].Claim_ID;

                            TotalCountinNDCDetailsGrid = Count;
                            if (Count > 30) { document.getElementById('<%= btnNDCAddDetails.ClientID%>').style.visibility = "hidden"; }
                            else { document.getElementById('<%= btnNDCAddDetails.ClientID%>').style.visibility = "visible"; }
                            table = table + '<tr style=\"border: 1px solid black; border-collapse: collapse;\" class=\"gridViewRow\"><td><span title=\"Service Line\" class=\"tNumber\">' + ServiceLine + '</span></td><td><span  title=\"NDC\" class=\"tNumber\">' + NDCCode + '</span></td><td><span title=\"Unit Of Measure\" class=\"tNumber\">' + UnitMeasure + '</span></td><td><span title=\"Prescription Number\">' + PrescriptionNuber + '</span></td><td><span title=\"Total Unit\" class=\"tNumber\">' + TotalUnit + '</span></td><td><input type=\"button\" value = \"Edit\" onClick=\"return EditNDCDetailsItem(\'' + Claims_NDC_Details_Screen_ID + '\',\'' + Claim_ID + '\',this); return true;\" class=\"btn btn-primary\" sytle=\"margin-left:3px\"></td><td><input type=\"button\" value=\"Delete\" onclick=\"return DeleteNDCDetailsItem(\'' + Claims_NDC_Details_Screen_ID + '\');\" class=\"btn btn-danger\" sytle=\"margin-left:3px\"></td></tr >';
                        };
                        table = table + "</tbody></table>";
                        document.getElementById('<%= divNDCDetails.ClientID %>').innerHTML = table;
                        localStorage.setItem("NDCDetailsTable", "" + table + "");
                        NDCDetailsClearFields();
                    }
                    else {
                        document.getElementById('<%= divNDCDetails.ClientID %>').innerHTML = "";
                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblNDCDetailsmessage]").text('Something Went Wrong..Try again');
                }
            });
        }
        GetServiceLineNoForNDC();
        NDCDetailsClearFields();
        document.getElementById('<%= btnNDCAddDetails.ClientID%>').style.visibility = "visible";
        document.getElementById('<%= btnEditNDCDetails.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnCancelNDCDetails.ClientID%>').style.visibility = "hidden";
        return false;
    }
    function EditNDCDetailsItem(Claims_NDC_Details_Screen_ID, ClaimId,control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        var NDCdetailsData;
        $("[id*=lblNDCDetailsmessage]").text('');
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "POST",
            url: webApiClaims + "EditNDCDetails?Claims_NDC_Details_Screen_ID=" + Claims_NDC_Details_Screen_ID + "&&Claim_ID=" + ClaimId,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                NDCdetailsData = result;
                if (NDCdetailsData.length > 0) {
                    for (var i = 0; i < NDCdetailsData.length; i++) {
                        var ServiceLine = result[i].ServiceLine;
                        var NDCCode = result[i].NDCCode;
                        var UnitMeasure = result[i].UnitMeasure;
                        var PrescriptionNuber = result[i].PrescriptionNuber;
                        var TotalUnit = result[i].TotalUnit;
                        var Claims_NDC_Details_Screen_ID = result[i].Claims_NDC_Details_Screen_ID;
                        var Claim_ID = result[i].Claim_ID;
                        <%--$('#<%=ddlNDCServiceLine.ClientID%>').text(ServiceLine);--%>
                        $("#<%=ddlNDCServiceLine.ClientID %> option:selected").text(ServiceLine);
                        $("[id*=txtNDCCode]").val("" + NDCCode + "");
                        $('#<%=ddlUnitOfMeasure.ClientID%>').val(UnitMeasure);
                        $("[id*=txtPrescriptionNumber]").val("" + PrescriptionNuber + "");
                        $("[id*=txtTotalUnit]").val("" + TotalUnit + "");
                        $("[id*=hdnNDC_ClaimID]").val("" + ClaimId + "");
                        $("[id*=hdnNDC_Code]").val("" + Claims_NDC_Details_Screen_ID + "");
                        document.getElementById('<%= btnNDCAddDetails.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnEditNDCDetails.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnCancelNDCDetails.ClientID%>').style.visibility = "visible";
                        return false;
                    };
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblNDCDetailsmessage]").text('NDC Code is invalid');
            }
        });
        GetServiceLineNoForNDC();
        return false;
    }
    function EditNDCCodeDetails() {

        $("[id*=lblNDCDetailsmessage]").text('');
        var NDCdetailsData;
        var serviceline = $("#<%=ddlNDCServiceLine.ClientID %> option:selected").text();
        var NDCCode = $("#<%= txtNDCCode.ClientID %>").first().val();
        var totalUnit = $("#<%= txtTotalUnit.ClientID %>").first().val();
        var PrescriptionNumber = $("#<%= txtPrescriptionNumber.ClientID %>").first().val();
        var UnitMeasure = $("#<%= ddlUnitOfMeasure.ClientID %>").first().val();
        var ClaimID = $("#<%= hdnNDC_ClaimID.ClientID %>").first().val();
        var ClaimType = $("#<%= hdnNDC_ClaimType.ClientID %>").first().val();
        var hdnNDC_Code = $("#<%= hdnNDC_Code.ClientID %>").first().val();
        ValidateNDCDetailsData();
        if (validatedata === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "POST",
                url: webApiClaims + "EditNDCDetailsData?ClaimID=" + ClaimID + "&&ClaimType=" + ClaimType + "&&hdnNDC_Code=" + hdnNDC_Code + "&&serviceline=" + serviceline + "&&NDCCode=" + NDCCode + "&&totalUnit=" + totalUnit + "&&PrescriptionNumber=" + PrescriptionNumber + "&&UnitMeasure=" + UnitMeasure + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {                    
                    NDCdetailsData = result;
                    var Count = 0;
                    if (NDCdetailsData.length > 0) {
                        table = '<table class=\"gridview\"  cellspacing=\"0\" align=\"Middle\" rules=\"rows\" border=\"1\" style=\"margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;\"><tbody><tr class=\"gridViewHeader\"><th scope=\"col\">Service Line</th><th scope=\"col\">NDC</th><th scope=\"col\">Unit Of Measure</th><th scope=\"col\">Prescription Number</th><th scope=\"col\">Total Unit</th><th scope=\"col\">&nbsp;</th><th scope=\"col\">&nbsp;</th></tr>';
                        for (var i = 0; i < NDCdetailsData.length; i++) {
                            Count++;
                            var ServiceLine = result[i].ServiceLine;
                            var NDCCode = result[i].NDCCode;
                            var UnitMeasure = result[i].UnitMeasure;
                            var PrescriptionNuber = result[i].PrescriptionNuber;
                            var TotalUnit = result[i].TotalUnit;
                            var Claims_NDC_Details_Screen_ID = result[i].Claims_NDC_Details_Screen_ID;
                            var Claim_ID = result[i].Claim_ID;

                            TotalCountinNDCDetailsGrid = Count;
                            if (Count > 30) { document.getElementById('<%= btnNDCAddDetails.ClientID%>').style.visibility = "hidden"; }
                             else { document.getElementById('<%= btnNDCAddDetails.ClientID%>').style.visibility = "visible"; }
                            table = table + '<tr style=\"border: 1px solid black; border-collapse: collapse;\" class=\"gridViewRow\"><td><span title=\"Service Line\" class=\"tNumber\">' + ServiceLine + '</span></td><td><span  title=\"NDC\" class=\"tNumber\">' + NDCCode + '</span></td><td><span title=\"Unit Of Measure\" class=\"tNumber\">' + UnitMeasure + '</span></td><td><span title=\"Prescription Number\">' + PrescriptionNuber + '</span></td><td><span title=\"Total Unit\" class=\"tNumber\">' + TotalUnit + '</span></td><td><input type=\"button\" value = \"Edit\" onClick=\"return EditNDCDetailsItem(\'' + Claims_NDC_Details_Screen_ID + '\',\'' + Claim_ID + '\',this); return true;\" class=\"btn btn-primary\" sytle=\"margin-left:3px\"></td><td><input type=\"button\" value=\"Delete\" onclick=\"return DeleteNDCDetailsItem(\'' + Claims_NDC_Details_Screen_ID + '\');\" class=\"btn btn-danger\" sytle=\"margin-left:3px\"></td></tr >';

                        };
                        table = table + "</tbody></table>";
                        document.getElementById('<%= divNDCDetails.ClientID %>').innerHTML = table;
                        localStorage.setItem("NDCDetailsTable", "" + table + "");
                        NDCDetailsClearFields();
                        GetServiceLineNoForNDC();
                    }
                    else {

                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=lblNDCDetailsmessage]").text('Something Went Wrong..Try again');
                }
            });


            document.getElementById('<%= btnNDCAddDetails.ClientID%>').style.visibility = "visible";
            document.getElementById('<%= btnEditNDCDetails.ClientID%>').style.visibility = "hidden";
            document.getElementById('<%= btnCancelNDCDetails.ClientID%>').style.visibility = "hidden";
        }
        
        return false;
        function ValidateNDCDetailsData() {            
            $("[id*=lblddlNDCServiceLine]").text('');
            $("[id*=lbltxtNDCCode]").text('');
            $("[id*=lblddlUnitOfMeasure]").text('');
            $("[id*=lbltxtTotalUnit]").text('');
            if (serviceline.trim() == undefined || serviceline.trim() == null || serviceline.trim() == "") {                
                validatedata = "false";
                $("[id*=lblddlNDCServiceLine]").text('Service Line Number is required');
                return false;
            }
            else {
                validatedata = "true";
                $("[id*=lblddlNDCServiceLine]").text('');
            }
            if (NDCCode.trim() == "" || NDCCode.trim() == undefined || NDCCode.trim() == null) {
                validatedata = "false";
                $("[id*=lbltxtNDCCode]").text('NDC is required');
                return false;
            }
            else {
                validatedata = "true";
                $("[id*=lbltxtNDCCode]").text('');
            }
            if (UnitMeasure.length == 0 || UnitMeasure == "" || UnitMeasure == undefined || UnitMeasure == null) {
                validatedata = "false";
                $("[id*=lblddlUnitOfMeasure]").text('Unit Of Measure is required');
                return false;
            }
            else {
                validatedata = "true";
                $("[id*=lblddlUnitOfMeasure]").text('');
            }
            if (totalUnit == "" || totalUnit == undefined || totalUnit == null) {
                validatedata = "false";
                $("[id*=lbltxtTotalUnit]").text('Total Unit is required');
                return false;
            }
            else {
                validatedata = "true";
                $("[id*=lbltxtTotalUnit]").text('');
            }
            //validations on business logics
            if (NDCCode.length != 11) {
                $("[id*=txtNDCCode]").val('');
                validatedata = "false";
                $("[id*=lbltxtNDCCode]").text('11 digits NDC Code is required');
                return false;
            }
            if (parseInt(totalUnit) < 1) {
                validatedata = "false";
                $("[id*=lbltxtTotalUnit]").text('Total Unit should be greater than zero');
                return false;
            }
        }
    }
    function loaderNDCDetail() {

        var NDCCode = $("#<%= txtNDCCode.ClientID %>").first().val();
        var totalUnit = $("#<%= txtTotalUnit.ClientID %>").first().val();
        var serviceline = $("#<%=ddlNDCServiceLine.ClientID %> option:selected").text();
        var unitmeasure = $("#<%= ddlUnitOfMeasure.ClientID %>").first().val();

        if (NDCCode != "" && totalUnit != "" && serviceline != "" && unitmeasure != "") {
            document.getElementById('<%= ddlNDCServiceLine.ClientID %>').disabled = true;
            document.getElementById('<%= txtNDCCode.ClientID %>').disabled = true;
            document.getElementById('<%= txtPrescriptionNumber.ClientID %>').disabled = true;
            document.getElementById('<%= ddlUnitOfMeasure.ClientID %>').disabled = true;
            document.getElementById('<%= txtTotalUnit.ClientID %>').disabled = true;

            document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';
                <%--document.getElementById('<%= btnNDCAdd.ClientID %>').style.display = 'none';--%>
        }
    }
    function CloseNDCPopup() {      
        $('#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ucSubmitClaimSearchNDC_txtCode').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ucSubmitClaimSearchNDC_txtTradeName').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ucSubmitClaimSearchNDC_gvSubmitClaimSearchNDCPop').find('tbody').html('');
        $("#<%=ucSubmitClaimSearchNDC.FindControl("divSearchNDCDetails").ClientID %>").html("");
        $("[id*=lblNDCDetailsmessage]").text('');
    }

    function visibleNDCCode()
    {
        $('#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ucSubmitClaimSearchNDC_txtCode').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ucSubmitClaimSearchNDC_txtTradeName').val('');
        $('#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ucSubmitClaimSearchNDC_gvSubmitClaimSearchNDCPop').find('tbody').html('');
        $("#<%= ucSubmitClaimSearchNDC.FindControl("fieldRequireError").ClientID %>").text('');
   
    localStorage.setItem("indexNDCcode", "");
    $find("mpeNDCCode").show();
    return false;
   
    }

    function GetNDCDetails() {
        $("[id*=lblNDCDetailsmessage]").text('');
        var txtNDCCode = $("#<%= ucSubmitClaimSearchNDC.FindControl("txtCode").ClientID %>").first().val();
        var txtTradeName = $("#<%= ucSubmitClaimSearchNDC.FindControl("txtTradeName").ClientID %>").first().val();
        $("#<%=ucSubmitClaimSearchNDC.FindControl("gvSubmitClaimSearchNDCPop").ClientID %>").html("");
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetNDCode?NDCCode=" + txtNDCCode + "&&TradeName=" + txtTradeName + "",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {

                if (txtNDCCode.trim().length == 0 && txtTradeName.trim().length == 0) {
                    $("#<%= ucSubmitClaimSearchNDC.FindControl("fieldRequireError").ClientID %>").text('NDC code or Trade name is required');
                    return false;
                }
                if (result.length == 0) {//divSearchNDCDetails
                    table = "";
                    table = table + "<table><tbody><tr><td> No Records Found </td></tr></tbody></table>";

                }
                else {
                    table = '<table class=\"gridview\"  cellspacing=\"0\" align=\"Middle\" rules=\"rows\" border=\"1\" style=\"margin-left: 40px; float: left;width:50%;border-collapse:collapse;margin-right: 20px;margin-top: 0px;\"><tbody><tr class=\"gridViewHeader\"><th scope=\"col\">NDC</th><th scope=\"col\">Trade Name </th></tr>';
                    for (var i = 0; i < result.length; i++) {
                        table = table + '<tr><td><a onClick=\"GetNDCcodeList(\'' + result[i].CLAIMS_NDC_CODE + '\');\">' + result[i].CLAIMS_NDC_CODE + '</a></td><td>' + result[i].LAY_DESC + '</td></tr>';

                        //$('#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ucSubmitClaimSearchNDC_gvSubmitClaimSearchNDCPop').append("<tr><td><a onClick='GetNDCcodeList(\"" + result.d[i].CLAIMS_NDC_CODE + "\");'>" + result.d[i].CLAIMS_NDC_CODE + "</a></td><td>" + result.d[i].LAY_DESC + "</td></tr>");
                    };
                }
                $("#<%=ucSubmitClaimSearchNDC.FindControl("divSearchNDCDetails").ClientID %>").html(table);
                $("#<%= ucSubmitClaimSearchNDC.FindControl("fieldRequireError").ClientID %>").text('');
                $("[id*=lbltxtNDCCode]").text('');
             },
             error: function (jqXHR, textStatus, errorThrown) {
                 $("#<%= ucSubmitClaimSearchNDC.FindControl("fieldRequireError").ClientID %>").text('Something Went Wrong..Try again');
             }
         });
          return false;
    }

    function GetNDCcodeList(NDCCode) {
        $find("mpeNDCCode").hide();
        // $("[id*=lbltxtNDCCode]").val(NDCCodeDescription.replace(/'/g, "\'"));
        $("[id*=txtNDCCode]").val("" + NDCCode + "");
        $("#<%=ucSubmitClaimSearchNDC.FindControl("divSearchNDCDetails").ClientID %>").html("");
    }
    
  
    function NDCCodetextChange() {
        $("[id*=lblNDCDetailsmessage]").text('');  
        var txtNDCCode = $("#<%= txtNDCCode.ClientID %>").first().val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetNDCCodeDescription?desc=" + txtNDCCode,
            //data: '{desc: "' + txtNDCCode + '" }',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (txtNDCCode.length != 11) {
                    $("[id*=txtNDCCode]").val('');
                    $("[id*=lbltxtNDCCode]").text('11 digits NDC Code is required');                 
                    return false;
                }

                if (result === "" && txtNDCCode.length == 11) {
                    $("[id*=txtNDCCode]").val('');
                    $("[id*=lbltxtNDCCode]").text('NDC code not found.');
                    return false;
                }
                else {
                    $("[id*=lbltxtNDCCode]").text('');

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblNDCDetailsmessage]").text('Something Went Wrong..Try again');
            }
        });
       /* GetServiceLineNoForNDC();*/
    }
    function GetServiceLineNoForNDC() {
        $("[id*=lblNDCDetailsmessage]").text('');
        $("#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ddlNDCServiceLine").html("");
        var newOption = "<option value=''></option>";
        $("#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ddlNDCServiceLine").append(newOption);
        var ClaimID = $("#<%= hdnNDC_ClaimID.ClientID %>").first().val();
        var APIToken = $("[id*=hdnAccessToken]").val();

        $.ajax({
            type: "GET",
            url: webApiClaims + "GetServiceLineNoForNDCDetails?ClaimID=" + ClaimID + "",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var ServiceLineList = result;
                if (ServiceLineList.length > 0) {
                    for (var i = 0; i < ServiceLineList.length; i++) {
                        var Service_Line = result[i].Service_Line;

                        var Claims_Service_Details_ID = result[i].Claims_Service_Details_ID;
                        var newOption = "<option value='" + Service_Line + "'>" + Service_Line + "</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucNDCDetails_ddlNDCServiceLine").append(newOption);
                    };

                }
                return false;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=lblNDCDetailsmessage]").text('Something Went Wrong..Try again');
            }
        });
        return false;
    }

</script>
  <div style="text-align: left;">
   
        <asp:Label runat="server" class="failureNotification" Style="font-size: 17px;" ID="lblNDCDetailsmessage" Text=""></asp:Label>
</div>
<div class="row">
    <div id="divNDCDetails" runat="server"></div>
</div>
<br />
<div runat="server" id="divNDC">
<div class="row" style="padding-left: 6rem;">
    <div class="col-md-2">
        <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color:red">*</span>Service Line</span>
    </div>
    <div class="col-md-2">
        <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color:red">*</span>NDC</span>
    </div>
    <div class="col-md-2">
        <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color:red">*</span>Unit Of Measure</span>
    </div>
    <div class="col-md-2">
        <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;">Prescription Number</span>
    </div>
    <div class="col-md-2">
        <span class="ohio-field-label" style="font-size: 17px; font-weight: bold;"><span style="color:red">*</span>Total Unit</span>
    </div>
    <div class="col-md-2">
    </div>
</div>

<div class="row" style="padding-left:6rem;">
    <div class="col-sm-2">
        <div style="text-align: left;">
            <asp:DropDownList ID="ddlNDCServiceLine" CssClass="formFieldTextBox" EnableViewState="true" runat="server" AppendDataBoundItems="True" Style="min-width: 20px; height: 30px" Width="170px" class="selectdropdown">
            </asp:DropDownList>
            <br />
            <asp:Label ID="lblddlNDCServiceLine" runat="server" Text="" CssClass="error-message"></asp:Label>
        </div>
    </div>
    <div class="col-sm-2">
        <div style="text-align: left;">
          <asp:TextBox ID="txtNDCCode" runat="server" CssClass="formfieldDate" MaxLength="11" Style="height: 30px; width: 140px" onKeydown="return (!(event.keyCode>=65 && event.keyCode <=90) && event.keyCode!=32);" onChange="return NDCCodetextChange()" />
            <asp:LinkButton ID="lnkNDCCodeSearch" CausesValidation="false" Style="font-size: 14px;" runat="server" Text="Search" ToolTip="Search" OnClientClick="return visibleNDCCode()" Visible="true"></asp:LinkButton>&nbsp;&nbsp;
            <asp:Label ID="lbltxtNDCCode" runat="server" Text="" CssClass="error-message" ></asp:Label>
            <br />
        </div>
    </div>
    <div class="col-sm-2">
        <div style="text-align: left;">
            <asp:DropDownList ID="ddlUnitOfMeasure" CssClass="formFieldTextBox" EnableViewState="true" runat="server" AppendDataBoundItems="True" Style="min-width: 30px; height: 30px" Width="170px">
            </asp:DropDownList>
             <asp:Label ID="lblddlUnitOfMeasure" runat="server" Text="" CssClass="error-message" ></asp:Label>
        </div>
    </div>
    <div class="col-sm-2">
        <div style="text-align: left;">
            <asp:TextBox ID="txtPrescriptionNumber" runat="server" CssClass="formFieldTextBox" Style="height: 30px; width: 170px" MaxLength="50" onKeydown="return (!(event.keyCode>=65 && event.keyCode <=90) && event.keyCode!=32);"  />
        </div>
        
 <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtPrescriptionNumber" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
    </div>
    <div class="col-sm-2">
        <div style="text-align: left;">
            <asp:TextBox ID="txtTotalUnit" runat="server" CssClass="formFieldTextBox" Style="height: 30px; width: 170px" MaxLength="15" onKeydown="return (!(event.keyCode>=65 && event.keyCode <=90) && event.keyCode!=32);" />
            <br />
             <asp:Label ID="lbltxtTotalUnit" runat="server" Text="" CssClass="error-message" ></asp:Label>
        </div>
    </div>
    <div class="col-sm-2">
         <button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                     
        <asp:Button ID="btnNDCAddDetails" Text="Add" OnClientClick=" return AddNDCDetailsData()" runat="server" CssClass="btn btn-primary" Font-Bold="True"  Width="60px" Height="30px"  />
        <br /> <asp:Button ID="btnEditNDCDetails" Style="visibility: hidden" Text="Update" runat="server" CssClass="btn btn-primary" Font-Bold="True"  Width="70px" OnClientClick="return EditNDCCodeDetails()" />
        <asp:Button ID="btnCancelNDCDetails" Style="visibility: hidden" Text="Cancel" runat="server" CssClass="btn btn-primary" Font-Bold="True"  Width="70px" OnClientClick="return CancelNDCCodeDetails()" />
    </div>
</div>
  
</div>
<ajax:ModalPopupExtender  BehaviorID="mpeNDCCode" ID="mpeSubmitClaimSearchNDC" runat="server" PopupControlID="pnlSubmitClaimSearchNDC"
    TargetControlID="ButtonSearchNDC" BackgroundCssClass="modalBackground" CancelControlID="btnCloseNDC" />
<asp:Panel ID="pnlSubmitClaimSearchNDC" runat="server" CssClass="modalPopup" Style=" min-height: 300px; min-width: 800px; max-height: 500px; max-width: 1000px;">
    <asp:Panel ID="pnlSubmitClaimSearchNDCHeader" runat="server" HorizontalAlign="Left">
        <asp:Button runat="server" ID="btnCloseNDC" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;" OnClientClick="CloseNDCPopup();"/>
        <div class="search-Results">
            <span style="padding-left:50px">NDC</span>
            <span style="padding-left:200px">TRADE NAME</span>
        </div>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server">
        <uc:SubmitClaimSearchNDC runat="server" ID="ucSubmitClaimSearchNDC" Visible="true"
            EnableViewState="true" />
    </asp:Panel>
</asp:Panel>
<asp:Button runat="server" ID="ButtonSearchNDC" Style="display: none" Text="ButtonSearchNDC" />


<asp:HiddenField ID="hdnNDC_ClaimID" runat="server" />
<asp:HiddenField ID="hdnNDC_ClaimType" runat="server" />
<asp:HiddenField ID="hdnNDC_Code" runat="server" />
