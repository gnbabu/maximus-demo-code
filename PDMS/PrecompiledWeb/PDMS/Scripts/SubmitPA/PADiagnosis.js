function visibleDiagnosisPanel() {
    try {
        $("[id*=txtDiagnosisCodeSearch1]").val('');
        $("[id*=txtDiagnosisCodeDescSearch1]").val('');
        document.getElementById('DiagnosisOutput').innerHTML = '';
        $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeDiagnosisSearch1").show();
    }
    catch (err) {
        console.log(err);
    }
    return false;
}
function GetDiagnosisDetails() {
    
    var txtValueCode = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtDiagnosisCodeSearch1").first().val();
    var txtvaluecodedesc = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtDiagnosisCodeDescSearch1").first().val();
    var lblICDValue = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_lblICDVersion").innerHTML;
    var input = 'Code ::' + txtValueCode + ' and Description ::' + txtvaluecodedesc + ' and ICD value ::' + lblICDValue;
    if (!txtValueCode.match(/\S/) && !txtvaluecodedesc.match(/\S/)) {
        document.getElementById('DiagnosisOutput').innerHTML = "<p style='color:red'>Either diagnosis code or description is required.</p>";
        return false;
    } else {
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetICDDiagnosis?code=" + txtValueCode + "&&icdVersion=" + lblICDValue + "&&diagnosisDes=" + txtvaluecodedesc,
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
                    document.getElementById('DiagnosisOutput').innerHTML = 'No Diagnosis Found.';
                }
                else {
                    var table = "<table class='gridViewSmallFont' cellspacing='0' rules='rows' border='1' style='width:100%;border-collapse:collapse;'><tbody><tr class='gridViewHeader'><th scope='col'>Diagnosis Code</th><th scope='col'>ICD Version</th><th scope='col'>Diagnosis Code Description</th></tr>";
                    for (var i = 0; i < result.length; i++) {
                        var code = result[i].ICD10Diag;
                        var desc = result[i].DiagDesc;
                        table = table + "<tr class='gridViewRow' valign='top'><td style='width: 100px;'><a onClick='GetDiagnosisSelectedRow(\"" + code.trimEnd() + "\"); return true;'>" + code.trimEnd() + "</a></td><td>ICD 10</td><td>" + desc.trimEnd() + "</td></tr>";
                    };
                    table = table + "</tbody></table>";
                    document.getElementById('DiagnosisOutput').innerHTML = table;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                document.getElementById('DiagnosisOutput').innerHTML = 'Something went wrong. Please contact system administrator....!';
            }
        });
        return false;
    }
}
function GetDiagnosisSelectedRow(code) {
    var lblICDValue = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_lblICDVersion").innerHTML;
    var diagnosisDes = '';
    var APIToken = $("[id*=hdnAccessToken]").val();
    $.ajax({
        type: "GET",
        url: webApiPA + "GetICDDiagnosis?code=" + code + "&&icdVersion=" + lblICDValue + "&&diagnosisDes=" + diagnosisDes,
        headers: {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
            "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
            "Authorization": "Bearer " + APIToken
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            for (var i = 0; i < result.length; i++) {
                $("[id*=txtDiagnosisCodeDescription]").val(result[0].DiagDesc.trimEnd());
                $("[id*=txtLnDiagnosisCode]").val(result[0].ICD10Diag.trimEnd());
                $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeDiagnosisSearch1").hide();
                return true;
            };
        },
        error: function (jqXHR, textStatus, errorThrown) {
            document.getElementById('DiagnosisOutput').innerHTML = 'Something went wrong. Please contact system administrator....!';
        }
    });
}
function OnDiagnosisTextChanged(action) {

    var txtDiagnosisCode = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtLnDiagnosisCode").first().val();
    txtDiagnosisCode = txtDiagnosisCode.replace(/(^\s+|\s+$)/g, '');
    var lblICDValue = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_lblICDVersion").innerHTML;
    var code = '';
    var desc = '';
    var diagnosisDes = '';
    var isvalidDiagCode = true;
    if (txtDiagnosisCode.indexOf('.') >= 0) {

        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblIcdVersionError').innerHTML = 'Incorrect Diagnosis Code (Enter diagnosis code without a decimal)';
        isvalidDiagCode = false;
    }

    if (txtDiagnosisCode != '' && txtDiagnosisCode != null && txtDiagnosisCode != undefined && isvalidDiagCode) {
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiPA + "GetICDDiagnosis?code=" + txtDiagnosisCode + "&&icdVersion=" + lblICDValue + "&&diagnosisDes=" + diagnosisDes,
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

                    if ($("[id*=txtLnDiagnosisCode]").val().indexOf('.') >= 0) {
                        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblIcdVersionError').innerHTML = 'Incorrect Diagnosis Code (Enter diagnosis code without a decimal)';
                    }
                    else {
                        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblIcdVersionError').innerHTML = 'Incorrect Diagnosis Code (Enter diagnosis code without a decimal)';
                    }

                    //$("[id*=txtLnDiagnosisCode]").val("");
                    $("[id*=txtDiagnosisCodeDescription]").val("");
                    return false;
                }
                else if (result.length > 1) {
                    $find("ctl00_MainContent_uc1SubmitPriorAuthorization_mpeDiagnosisSearch1").show();
                }
                else {
                    var placeofcd = result[0].ICD10Diag;
                    var placeofdesc = result[0].DiagDesc;
                    $("[id*=txtDiagnosisCodeDescription]").val(result[0].DiagDesc.trimEnd());
                    $("[id*=txtLnDiagnosisCode]").val(result[0].ICD10Diag.trimEnd());
                    var strMsg = "It is a valid Diagnosis code. Please click on Add button."
                    if (action == "Update")
                        strMsg = "It is a valid Diagnosis code. Please click on Update button."
                    document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblIcdVersionError').innerHTML = strMsg;

                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblIcdVersionError').innerHTML = 'Something went wrong. Please contact system administrator....!';
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblIcdVersionError').style.visibility = "visible";
            }
        });
    }
    else if (!isvalidDiagCode) {
        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblIcdVersionError').innerHTML = 'Incorrect Diagnosis Code (Enter diagnosis code without a decimal)';
        $("[id*=txtDiagnosisCodeDescription]").val("");
        return false;
    }
    else {
        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblICDVersion').innerHTML = 'ICD 10';
    }

}
function ValidateDiagnosisSave() {

    var PAnumberValue = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_txtPANumber2");
    var isNonConvertedPA = false;
    if (PAnumberValue && PAnumberValue.value.includes("AUTH")) {
        isNonConvertedPA = true;
    }
    var ddlDiagnosisCodeType = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlDiagnosisCodeType");
    var value = ddlDiagnosisCodeType.value;
    var text = ddlDiagnosisCodeType.options[ddlDiagnosisCodeType.selectedIndex].text;
    if (text == '--- Please select ---' && !isNonConvertedPA) {
        alert('Please select DiagnosisCode Type...!');
        document.getElementById("dvDiagnosisAddparent").style.visibility = "visible";
        return false;
    }
    return true;
}
function DeleteDiagnosisLineitem(diagnosisID) {
    var result = confirm("Are you sure you want to delete?");
    var linkId = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_tempUniqueId').value;
    if (result) {
        if (diagnosisID != '' && diagnosisID != null && diagnosisID != undefined) {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "DELETE",
                url: webApiPA + "DeleteDiagnosis?diagnosisID=" + diagnosisID + "&&linkId=" + linkId,
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    GetDiagnoisServiceDetails();
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    /* alert('Something went wrong while deleting the Diagnosis Panel. Please Contact system administrator..!')*/
                    console.log('Something went wrong while deleting the Diagnosis Panel. Please Contact system administrator..!');
                }
            });
        }
    }
}
function AddNewdiagnosisLineItem(code, index) {
    document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDiagnosisErrorMessage').innerHTML = '';
    document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDiagnosisErrorMessage').style.visibility = "none";

    var isPrincipalAdded = false;
    var isAdmittingAdded = false;
    var priorAuthDiagnosisCodeTypeDesc;
    var priorAuthDiagnosisList;
    var diagnosisTypeCode;
    var diagnosisCode;
    var diagnosisDesc;
    var ClaimType = $(".rblClaimType input:checked").val();
    var patype;
    if (ClaimType == 'dental') {
        patype = 0;
    }
    else if (ClaimType == 'Professional') {
        patype = 2;
    }
    else {
        patype = 1;
    }
    var linkId = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_tempUniqueId').value;
    var Medicaid = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtProMedicaidIDORD').val();
    var APIToken = $("[id*=hdnAccessToken]").val();
    $.ajax({
        type: "GET",
        url: webApiPA + "GetDiagnosisNewLineAdd?paType=" + patype + "&&medicaidID=" + Medicaid + "&&linkId=" + linkId,
        headers: {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
            "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
            "Authorization": "Bearer " + APIToken
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            console.log(result);
            var diagnosisCodeTypes = result.diagnosisCodeTypes;
            priorAuthDiagnosisList = result.priorAuthDiagnoses;

            if (priorAuthDiagnosisList.length > 0) {
                for (var i = 0; i < priorAuthDiagnosisList.length; i++) {
                    if (priorAuthDiagnosisList[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC == 'Principal') {
                        isPrincipalAdded = true;
                        // alert('Principal Got added...!')
                    }
                    if (priorAuthDiagnosisList[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC == 'Admitting') {
                        isAdmittingAdded = true;
                        //alert('Admitting Got added...!')
                    }
                }
            }

            if (typeof code === 'string' && code.length == 0) {
                var ddlDiagnosisCodeType = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlDiagnosisCodeType");
                var i, L = ddlDiagnosisCodeType.options.length - 1;
                for (i = L; i >= 0; i--) {
                    ddlDiagnosisCodeType.remove(i);
                }
                ddlDiagnosisCodeType.options.length == 0;
                if (priorAuthDiagnosisList.length == 0) {
                    for (var i = 0; i < diagnosisCodeTypes.length; i++) {
                        if (diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC == 'Principal') {
                            AddOption(ddlDiagnosisCodeType, diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC, diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID);
                        }
                    }
                }
                if (priorAuthDiagnosisList.length == 1) {
                    if (!isPrincipalAdded) {
                        for (var i = 0; i < diagnosisCodeTypes.length; i++) {
                            if (diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC == 'Principal') {
                                AddOption(ddlDiagnosisCodeType, diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC, diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID);
                            }
                        }
                    }
                    else {
                        AddOption(ddlDiagnosisCodeType, "--- Please select ---", "0");
                        for (var i = 0; i < diagnosisCodeTypes.length; i++) {
                            if (diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC != 'Principal') {
                                AddOption(ddlDiagnosisCodeType, diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC, diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID);
                            }
                        }
                    }
                }
                if (priorAuthDiagnosisList.length >= 2 && isAdmittingAdded && isPrincipalAdded) {
                    AddOption(ddlDiagnosisCodeType, "--- Please select ---", "0");
                    for (var i = 0; i < diagnosisCodeTypes.length; i++) {
                        if (diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC != 'Principal' && diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC != 'Admitting') {
                            AddOption(ddlDiagnosisCodeType, diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC, diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID);
                        }
                    }
                    for (var i = 0; i < ddlDiagnosisCodeType.options.length; i++) {
                        if (ddlDiagnosisCodeType.options[i].text == '--- Please select ---') {
                            ddlDiagnosisCodeType.options[i].selected = true;
                        }
                    }
                }
                else if (priorAuthDiagnosisList.length >= 2 && !isPrincipalAdded) {
                    for (var i = 0; i < diagnosisCodeTypes.length; i++) {
                        if (diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC == 'Principal') {
                            AddOption(ddlDiagnosisCodeType, diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC, diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID);
                        }
                    }
                }
                else if (priorAuthDiagnosisList.length >= 2 && !isAdmittingAdded) {
                    AddOption(ddlDiagnosisCodeType, "--- Please select ---", "0");
                    for (var i = 0; i < diagnosisCodeTypes.length; i++) {
                        if (diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC != 'Principal') {
                            AddOption(ddlDiagnosisCodeType, diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC, diagnosisCodeTypes[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID);
                        }
                    }
                }

                document.getElementById("btnDiagnosisAddLineDiv").hidden = false;
                document.getElementById("btnDiagnosisUpdateDiv").hidden = true;
            }
            if (typeof code === 'string' && code.length != 0) {

                document.getElementById("btnDiagnosisAddLineDiv").hidden = true;
                document.getElementById("btnDiagnosisUpdateDiv").hidden = false;
                var ddlDiagnosisCodeType = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlDiagnosisCodeType");
                for (var i = 0; i < priorAuthDiagnosisList.length; i++) {
                    if (priorAuthDiagnosisList[i].PRIOR_AUTH_DIAGNOSIS_ID == code) {

                        priorAuthDiagnosisCodeTypeDesc = priorAuthDiagnosisList[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC;

                        diagnosisTypeCode = priorAuthDiagnosisList[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID;
                        diagnosisCode = priorAuthDiagnosisList[i].PRIOR_AUTH_DIAGNOSIS_CODE;
                        diagnosisDesc = priorAuthDiagnosisList[i].PRIOR_AUTH_DIAGNOSIS_DESC;
                        const dateString = priorAuthDiagnosisList[i].PRIOR_AUTH_DIAGNOSIS_DATE;

                        if (dateString.length > 0) {
                            const D = new Date(dateString);
                            var diagDate = getFormattedDate(D);
                            $("[id*=txtDiagnosisDate]").val(diagDate);
                        }
                        else {
                            $("[id*=txtDiagnosisDate]").val('');
                        }

                        var ii, Li = ddlDiagnosisCodeType.options.length - 1;
                        for (ii = Li; ii >= 0; ii--) {
                            ddlDiagnosisCodeType.remove(ii);
                        }
                        ddlDiagnosisCodeType.options.length == 0;
                        var diagnosisTypename;
                        for (var j = 0; j < diagnosisCodeTypes.length; j++) {
                            if (diagnosisCodeTypes[j].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID == diagnosisTypeCode) {
                                diagnosisTypename = diagnosisCodeTypes[j].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC;
                            }
                        }
                        $("[id*=txtDiagnosisCodeDescription]").val(diagnosisDesc);
                        $("[id*=txtLnDiagnosisCode]").val(diagnosisCode);

                        $("[id*=hdDiagLineNum]").val(code);
                        console.log(document.getElementById("hdDiagLineNum"));
                        if (diagnosisTypename == 'Principal') {
                            for (var k = 0; k < diagnosisCodeTypes.length; k++) {
                                if (diagnosisCodeTypes[k].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC == 'Principal') {
                                    AddOption(ddlDiagnosisCodeType, diagnosisCodeTypes[k].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC, diagnosisCodeTypes[k].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID);
                                }
                            }
                        }
                        else if (diagnosisTypename == 'Admitting') {
                            AddOption(ddlDiagnosisCodeType, "--- Please select ---", "0");
                            for (var l = 0; l < diagnosisCodeTypes.length; l++) {
                                if (diagnosisCodeTypes[l].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC != 'Principal') {
                                    AddOption(ddlDiagnosisCodeType, diagnosisCodeTypes[l].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC, diagnosisCodeTypes[l].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID);
                                }
                            }
                        }
                        else if (index == 2) {
                            AddOption(ddlDiagnosisCodeType, "--- Please select ---", "0");
                            for (var m = 0; m < diagnosisCodeTypes.length; m++) {
                                if (diagnosisCodeTypes[m].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC != 'Principal') {
                                    AddOption(ddlDiagnosisCodeType, diagnosisCodeTypes[m].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC, diagnosisCodeTypes[m].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID);
                                }
                            }
                        }
                        else if (priorAuthDiagnosisList.length >= 2 && diagnosisTypename != 'Admitting') {
                            AddOption(ddlDiagnosisCodeType, "--- Please select ---", "0");
                            for (var n = 0; n < diagnosisCodeTypes.length; n++) {
                                if (diagnosisCodeTypes[n].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC != 'Principal' && diagnosisCodeTypes[n].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC != 'Admitting') {
                                    AddOption(ddlDiagnosisCodeType, diagnosisCodeTypes[n].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC, diagnosisCodeTypes[n].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID);
                                }
                            }
                        }
                        else if (!isPAConvertedType()) {
                            if (priorAuthDiagnosisCodeTypeDesc != 'Principal' && priorAuthDiagnosisCodeTypeDesc != 'Admitting') {
                                AddOption(ddlDiagnosisCodeType, priorAuthDiagnosisCodeTypeDesc, diagnosisTypeCode);
                                if (!isPrincipalAdded) {
                                    for (var o = 0; o < diagnosisCodeTypes.length; o++) {
                                        if (diagnosisCodeTypes[o].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC == 'Principal') {
                                            AddOption(ddlDiagnosisCodeType, diagnosisCodeTypes[o].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC, diagnosisCodeTypes[o].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID);
                                        }
                                    }
                                }
                                else {
                                    AddOption(ddlDiagnosisCodeType, "--- Please select ---", "0");
                                    for (var p = 0; p < diagnosisCodeTypes.length; p++) {
                                        if (diagnosisCodeTypes[p].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC != 'Principal') {
                                            AddOption(ddlDiagnosisCodeType, diagnosisCodeTypes[p].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC, diagnosisCodeTypes[p].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID);
                                        }
                                    }
                                }
                            }
                            else {
                                AddOption(ddlDiagnosisCodeType, priorAuthDiagnosisCodeTypeDesc, diagnosisTypeCode);
                            }
                        }
                        for (var q = 0; q < ddlDiagnosisCodeType.options.length; q++) {
                            if (ddlDiagnosisCodeType.options[q].value == diagnosisTypeCode) {
                                ddlDiagnosisCodeType.options[q].selected = true;
                            }
                        }
                    }
                }
            }

            var pnlDiagnosisLine = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_pnlDiagnosisLine');
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_pnlDiagnosisLine').style.visibility = "block";
            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_isDiagnosisLineOpened').value = "true";

            // isDiagnosisExist();

            document.getElementById("dvDiagnosisAddparent").style.height = "auto";
            document.getElementById("dvDiagnosisAddparent").style.overflow = "visible";
            document.getElementById("dvDiagnosisAddparent").style.visibility = "visible";


            return true;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(JSON.stringify(jqXHR));
        }
    });
    return false;
}
function SaveDiagnosisLineItem(action) {
 
    OnDiagnosisTextChanged(action);
    if (document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblIcdVersionError').innerHTML != "") {
        if ($("[id*=txtDiagnosisCodeDescription]").val() != "") {
            if (Page_ClientValidate('valDiagnosis')) {

                document.getElementById('diagLoader').innerHTML = "<img src='../Images/loader.gif'/>";
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDiagnosisErrorMessage').innerHTML = '';
                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDiagnosisErrorMessage').style.visibility = "none";
                if (ValidateDiagnosisSave()) {
                    var ClaimType = $(".rblClaimType input:checked").val();
                    if (ClaimType == '' || ClaimType == null || ClaimType == undefined) {
                        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDiagnosisErrorMessage').innerHTML = 'Something went wrong. Please contact system administrator....!';
                        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDiagnosisErrorMessage').style.visibility = "block";
                        return true;
                    }
                   
                    var txtDiagnosisDate = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtDiagnosisDate").first().val();

                    //Need to Check
                    var Medicaid = getParameterByName('MedicaidNumber');


                    var CurrentDate = new Date();
                    var GivenDate = new Date(txtDiagnosisDate);

                    if (txtDiagnosisDate != null && txtDiagnosisDate != '' && txtDiagnosisDate != undefined && GivenDate > CurrentDate) {

                        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDiagnosisErrorMessage').innerHTML = 'Dbiagnosis date should not be future date.';
                        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDiagnosisErrorMessage').style.visibility = "block";

                        return false;
                    }
                    var diagnosisCode = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtLnDiagnosisCode").first().val();
                    var diagnosisDesc = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtDiagnosisCodeDescription").first().val();
                    var ddlDiagnosisCodeType = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlDiagnosisCodeType");
                    var codevalue = ddlDiagnosisCodeType.value;
                    var codetext = ddlDiagnosisCodeType.options[ddlDiagnosisCodeType.selectedIndex].text;

                    var patype;
                    if (ClaimType == 'dental') {
                        patype = 0;
                    }
                    else if (ClaimType == 'Professional') {
                        patype = 2;
                    }
                    else {
                        patype = 1;
                    }
                    var diagId;
                    var opearion;
                    if (action == 'Update') {
                        diagId = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_hdDiagLineNum').value;
                        opearion = 'Update';
                    }
                    else {
                        diagId = '';
                        opearion = 'Add';
                    }
                    var linkId = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_tempUniqueId').value;
                    var userName = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_hdnUserName').value;
                    var APIToken = $("[id*=hdnAccessToken]").val();
                    $.ajax({
                        type: "POST",
                        url: webApiPA + "DiagnosisSave?claimType=" + patype + "&&diagnosisCode=" + diagnosisCode + "&&diagnosisCodeDesc=" + diagnosisDesc + "&&diagonsisType=" + codevalue + "&&diagnosisTypeText=" + codetext + "&&diagUpdateDate=" + txtDiagnosisDate + "&&MedicaidId=" + Medicaid + "&&action=" + opearion + "&&diagId=" + diagId + "&&linkId=" + linkId + "&&userName=" + userName,
                        headers: {
                            "Access-Control-Allow-Origin": "*",
                            "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                            "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                            "Authorization": "Bearer " + APIToken
                        },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (result) {
                            /* alert(result);*/
                            /* alert(result.d.length);*/
                            if (result !== true) {

                                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDiagnosisErrorMessage').innerHTML = result;
                                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDiagnosisErrorMessage').style.visibility = "block";
                            }
                            else if (result === true) {
                                /*  alert('Came to else condition....!');*/
                                $("[id*=txtDiagnosisDate]").val('');
                                $("[id*=txtLnDiagnosisCode]").val('');
                                $("[id*=txtDiagnosisCodeDescription]").val('');
                                $("[id*=txtLnDiagnosisCode]").val('');
                                var ddlDiagnosisCodeType = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlDiagnosisCodeType");
                                ddlDiagnosisCodeType.options[0].selected = true;
                                document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlDiagnosisCodeType").disabled = false;
                                document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_isDiagnosisLineOpened').value = "false";
                                document.getElementById("dvDiagnosisAddparent").style.visibility = "hidden";
                                document.getElementById("dvDiagnosisAddparent").style.overflow = "hidden";
                                document.getElementById("dvDiagnosisAddparent").style.height = "0";
                                document.getElementById("btnDiagnosisAddLineDiv").hidden = false;
                                document.getElementById("btnDiagnosisUpdateDiv").hidden = true;

                                GetDiagnoisServiceDetails();
                                document.getElementById('diagLoader').innerHTML = "";
                                return false;
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDiagnosisErrorMessage').innerHTML = 'Something went wrong. Please contact system administrator....!';
                            document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblDiagnosisErrorMessage').style.visibility = "visible";
                        }
                    });
                }
                else {
                    document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_lblICDVersion').innerHTML = 'ICD 10';
                }
            }
            else {
                return false;
            }
            document.getElementById('diagLoader').innerHTML = "";
        }
    }
}
function GetDiagnoisServiceDetails() {
    //alert("In GetDiagnoisServiceDetails");
    var Medicaid = getParameterByName('MedicaidNumber');
    var btnDiagnosisAdd = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_btnDiagnosisAdd");

    var patype;
    var ClaimType = $(".rblClaimType input:checked").val();
    if (ClaimType == 'dental') {
        patype = 0;
    }
    else if (ClaimType == 'Professional') {
        patype = 2;
    }
    else {
        patype = 1;
    }
    var linkId = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_tempUniqueId').value;
    var APIToken = $("[id*=hdnAccessToken]").val();
    $.ajax({
        type: "GET",
        url: webApiPA + "GetDiagnosisNewItems?paType=" + patype + "&&medicaidID=" + Medicaid + "&&linkId=" + linkId,
        headers: {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
            "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
            "Authorization": "Bearer " + APIToken
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            var diagnosisCodeTypes = result.diagnosisCodeTypes;
            priorAuthDiagnosisList = result.priorAuthDiagnoses;
            // alert(priorAuthDiagnosisList.length);
            if (priorAuthDiagnosisList.length > 0) {
                document.getElementById('divDiagnosisNoData').innerHTML = '';

                if (priorAuthDiagnosisList.length >= 12) {
                    if (typeof (btnDiagnosisAdd) != 'undefined' && btnDiagnosisAdd != null) {
                        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_btnDiagnosisAdd').style.visibility = "hidden";
                    }
                }
                else {
                    if (typeof (btnDiagnosisAdd) != 'undefined' && btnDiagnosisAdd != null) {
                        var hdnDiagnosiAdd = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnDiagnosiAddCopy').val();
                        if (hdnDiagnosiAdd != null && hdnDiagnosiAdd != undefined && hdnDiagnosiAdd == "true") {

                            document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_btnDiagnosisAdd").disabled = false;

                            document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_ddlDiagnosisCodeType").disabled = false;
                            document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_txtLnDiagnosisCode").disabled = false;
                            document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_txtDiagnosisDate").disabled = false;
                            document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_btnDiagnosisAdd").onclick = function () { AddNewdiagnosisLineItem('', ''); };
                            document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_btnDiagnosisAdd").type = "button";

                        }

                        document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_btnDiagnosisAdd').style.visibility = "visible";

                    }
                }
                // alert('Preparing Table...!');
                var table = "<table class='gridview' cellspacing='0' align='Left' rules='rows' border='1' style='width:100%;min-width:100%;border-collapse:collapse;margin-right: 0px; margin-top: 0px;'><tbody><tr class='gridViewHeader'><th scope='col'>Sequence</th><th scope='col'>*Diagnosis Code Type</th><th scope='col'>*Diagnosis Code</th><th scope='col'>Diagnosis Code Description</th><th scope='col'>Diagnosis Date</th><th scope='col'> </th><th scope='col'> </th></tr>";

                for (var i = 0; i < priorAuthDiagnosisList.length; i++) {
                    //  alert('Looping table....!');
                    var diagnosisCodeType = priorAuthDiagnosisList[i].PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC;
                    var PRIOR_AUTH_DIAGNOSIS_ID = priorAuthDiagnosisList[i].PRIOR_AUTH_DIAGNOSIS_ID;
                    var diagnosisCode = priorAuthDiagnosisList[i].PRIOR_AUTH_DIAGNOSIS_CODE;
                    var diagnosisDesc = priorAuthDiagnosisList[i].PRIOR_AUTH_DIAGNOSIS_DESC;
                    var sequence = i + 1;

                    const dateString = priorAuthDiagnosisList[i].PRIOR_AUTH_DIAGNOSIS_DATE;

                    if (dateString.length > 0) {
                        const D = new Date(dateString);
                        var diagDate = getFormattedDate(D);
                        var diagnosisDate = diagDate;
                    }
                    else {
                        var diagnosisDate = '';
                    }
                    var siblingElement = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_pnlsepDiagnosis').nextElementSibling;
                    siblingElement.style.height = 'auto';
                    table = table + "<tr class='gridViewRow'><td><span title='Line' class='tNumber' style='display:inline-block;width:30px;'>" + sequence + "</span></td><td><span title='Line' class='tNumber' style='display:inline-block;width:80px;'>" + diagnosisCodeType + "</span></td><td><span  title='Line' class='tNumber'>" + diagnosisCode + "</span></td><td><span title='Line' class='tNumber' style='display:inline-block;width:120px;'>" + diagnosisDesc + "</span></td><td>" + diagnosisDate + "</td><td><input type='button' value = 'Edit' onClick = 'EditdiagnosisLineItem(\"" + PRIOR_AUTH_DIAGNOSIS_ID + "\",\"" + sequence + "\");' class='btn btn-primary' sytle = 'margin-left:3px' style = 'font-weight:bold;width:50px;' /></td><td><input type='button' value='Delete' onclick='DeleteDiagnosisLineitem(\"" + PRIOR_AUTH_DIAGNOSIS_ID + "\");' class='btn btn-danger' sytle='margin-left:3px' style='font-weight:bold;width:50px;' /></td></tr>";
                };
                //  alert('Out of loop....!');
                table += `
                        </tbody>
                        </table>
                    `;
                document.getElementById('DiagnosisLineOutput').innerHTML = table;



                // alert('data binded to table...!');
            }
            else {
                var PAnumberValue = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_txtPANumber2");
                var isNonConvertedPA = false;
                if (PAnumberValue && PAnumberValue.value.includes("AUTH")) {
                    isNonConvertedPA = true;
                }

                if (!isNonConvertedPA) {

                    var DiagnosisLineOutput = document.getElementById('DiagnosisLineOutput');
                    if (DiagnosisLineOutput !== null && DiagnosisLineOutput !== undefined) {
                        DiagnosisLineOutput.innerHTML = '';
                    }
                    var divDiagnosisNoData = document.getElementById('divDiagnosisNoData');
                    if (divDiagnosisNoData !== null && divDiagnosisNoData !== undefined) {
                        divDiagnosisNoData.innerHTML = ' <h3 style="color:green"> No Diagnosis Data found. Please Click on Add to add Diagnosis. </h1>';
                    }
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log('Something went wrong while binding Diagnosis Grid. Please contact system administrator...!');
            /*  alert("Something went wrong while binding Diagnosis Grid. Please contact system administrator...!");*/
        }
    });
}
function isDiagnosisExist() {
    var Medicaid = getParameterByName('MedicaidNumber');
    var patype;
    var ClaimType = $(".rblClaimType input:checked").val();
    if (ClaimType == 'dental') {
        patype = 0;
    }
    else if (ClaimType == 'Professional') {
        patype = 2;
    }
    else {
        patype = 1;
    }
    var linkId = document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_tempUniqueId').value;
    var APIToken = $("[id*=hdnAccessToken]").val();
    $.ajax({
        type: "GET",
        url: webApiPA + "CheckDiagnosisDataExist?paType=" + patype + "&&medicaidID=" + Medicaid + "&&linkId=" + linkId,
        headers: {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
            "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
            "Authorization": "Bearer " + APIToken
        },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            if (result) {
                GetDiagnoisServiceDetails();
            }
            else {
                return false;
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            /* alert("Something went wrong while binding Diagnosis Grid. Please contact system administrator...!");*/
            console.log("Something went wrong while binding Diagnosis Grid. Please contact system administrator...!");
        }
    });
}
function AddOption(ddlDiagnosisCodeType, text, value) {
    var option = document.createElement('option');
    option.value = value;
    option.innerHTML = text;
    ddlDiagnosisCodeType.options.add(option);
}
function EditdiagnosisLineItem(code, index) {
 
    AddNewdiagnosisLineItem(code, index);
    document.getElementById("dvDiagnosisAddparent").style.height = "auto";
    document.getElementById("dvDiagnosisAddparent").style.overflow = "visible";
    document.getElementById("dvDiagnosisAddparent").style.visibility = "visible";
    return false;
}
function DiagnosisLineCancel() {
    $("[id*=txtDiagnosisCodeDescription]").val('');
    $("[id*=txtLnDiagnosisCode]").val('');
    $("[id*=txtDiagnosisDate]").val('');
    $("[id*=hdLineNumber]").val('');
    $("[id*=lblIcdVersionError]").val('');


    document.getElementById("dvDiagnosisAddparent").style.height = "0";
    document.getElementById("dvDiagnosisAddparent").style.overflow = "hidden";
    document.getElementById("dvDiagnosisAddparent").style.visibility = "hidden";

    document.getElementById("btnDiagnosisAddLineDiv").hidden = false;
    document.getElementById("btnDiagnosisUpdateDiv").hidden = true;
    document.getElementById('ctl00_MainContent_uc1SubmitPriorAuthorization_isDiagnosisLineOpened').value = "false";
    return true;
}
function isPAConvertedType() {
    var PAnumberValue = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_txtPANumber2");
    var isNonConvertedPA = false;
    if (PAnumberValue && PAnumberValue.value.includes("AUTH")) {
        isNonConvertedPA = true;
    }
}