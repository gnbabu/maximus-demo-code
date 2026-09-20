<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_DiagnosisCodePanel, App_Web_l5y5araq" %>

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">
<script type="text/javascript">
    var TotalCountintheGrid;
    function clearDiagTableContent() {

        document.getElementById('<%= providerDiagOutput.ClientID %>').innerHTML = "";
      
    }
    function displaytable() {
        
        var abc = localStorage.getItem("diagTable");
       
        if (abc != null && abc != undefined && abc != "") {
           
            var ClaimType = $("#<%= hdnClaimType_Diagnosis.ClientID %>").val();
            var hdnClaimType_Diag = $("#<%= hdnClaimType_Diagnosis.ClientID %>").val();
            var hdnClaimIdDiag = $("#<%= hdnClaimIdDiagnosis.ClientID %>").val();
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "PUT",
                url: webApiClaims + "BindDiagCode?hdnClaimIdDiag=" + hdnClaimIdDiag + "&&hdnClaimType_Diag=" + hdnClaimType_Diag,
                //data: '{hdnClaimIdDiag: "' + hdnClaimIdDiag + '" , hdnClaimType_Diag: "' + hdnClaimType_Diag + '" }',
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
                        var table;
                        if (ClaimType == '1') {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Diagnosis Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:10px; scope='col'>Present On Admission</th><th style='width:30px; scope='col'>Diagnosis Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        } else { table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Diagnosis Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:30px; scope='col'>Diagnosis Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>"; }

                        if ((ClaimType == '0')) {
                            clearServiceDetailsOptionsDental();
                            var newOption = "<option value=''></option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").append(newOption);
                        }
                        if ((ClaimType == '2')) {
                            clearServiceDetailsOptionsProf();
                            var newOption = "<option value=''></option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").append(newOption);
                        }
                        var count = 0; var admittingCount = 0; var reasonToVisitCount = 0; var extCauseOfInj = 0; var totalCount = 0; var othercount = 0;
                        for (var i = 0; i < claimDiagnosisList.length; i++) {
                            totalCount++;
                            var sequence = result[i].sequence;
                            var DiagnosisCode = result[i].DiagnosisCode;
                            var ICDVersion = result[i].ICDVersion;
                            var PresentOnAdmission = result[i].PreasentOnAdmission;
                            var DianosisCodeDescription = result[i].DianosisCodeDescription;
                            var Claims_DIAGNOSIS_ID = result[i].Claims_Diagnosis_ID;
                            var Claim_ID = result[i].Claim_ID;
                            if (sequence == "Principal") { count++; }
                            if (sequence == "Admitting") { admittingCount++; }
                            if (sequence == "External Cause of Injury") { extCauseOfInj++; }
                            if (sequence == "Patient Reason for Visit") { reasonToVisitCount++; }
                            if (sequence == "Other") { othercount++; }
                            if ((ClaimType == '0')) {
                                var newOption = "<option value='" + sequence + "'>" + sequence + "</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").append(newOption);
                            }
                            if ((ClaimType == '2')) {
                                var newOption = "<option value='" + sequence + "'>" + sequence + "</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").append(newOption);
                            }

                            if (ClaimType == '1')
                            {
                                if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_txtstatus2').value == 'Pending Submission') {
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + PresentOnAdmission + "</span></td><td>" + DianosisCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditClaimsdiagnosisLineItem(\"" + Claims_DIAGNOSIS_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimDiagnosisLineitem(\"" + Claims_DIAGNOSIS_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                                } else {
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + PresentOnAdmission + "</span></td><td>" + DianosisCodeDescription + "</td>";
                                }
                            } else
                            {
                                if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_txtstatus2').value == 'Pending Submission') {
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td>" + DianosisCodeDescription + "</td><td>&nbsp;</td><td>&nbsp;</td><td><input type='button' value = 'Edit' onClick = 'return EditClaimsdiagnosisLineItem(\"" + Claims_DIAGNOSIS_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimDiagnosisLineitem(\"" + Claims_DIAGNOSIS_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                                } else {
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td>" + DianosisCodeDescription + "</td><td>&nbsp;</td><td>&nbsp;</td></tr >";
                                }
                            }
                        };
                        TotalCountintheGrid = totalCount;
                        if( ClaimType == '1') {
                            if (totalCount >= 41) { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden"; }
                            else {                                if (document.getElementById('<%= btnDiagnosis.ClientID%>') != null) {                                    document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible";
                                }

                            }
                        }
                        else if (ClaimType == '2') {
                            if (totalCount >= 12) { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden"; }
                            else {
                                if (document.getElementById('<%= btnDiagnosis.ClientID%>') != null) {
                                    document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible";
                                }
                            }
                        }
                        else if (ClaimType == '0') {
                            if (totalCount >= 4) { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden"; }
                            else {
                                if (document.getElementById('<%= btnDiagnosis.ClientID%>') != null) {
                                    document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible";
                                }
                            }
                        }
                        table = table + "</tbody></table>";
                        $("#<%=ddlDiagnosisSequenceDescription.ClientID %>").empty();
                        var newOption = "<option value=''></option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);

                        newOption = "<option value='1'>Principal</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                        if (count < 1) {
                            $("#<%= ddl1.ClientID %>").val("1");
                        } else { $("#<%= ddl1.ClientID %>").val("0"); }
                        newOption = "<option value='2'>Admitting</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                        if (admittingCount < 1) {
                            $("#<%= ddl2.ClientID %>").val("1");
                        } else { $("#<%= ddl2.ClientID %>").val("0"); }
                        newOption = "<option value='3'>Other</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                        if (othercount < 24) {
                            $("#<%= ddl5.ClientID %>").val("1");
                        } else {
                            $("#<%= ddl5.ClientID %>").val("0");
                        }
                        newOption = "<option value='4'>Patient Reason for Visit</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                        if (reasonToVisitCount < 3) {
                            $("#<%= ddl3.ClientID %>").val("1");
                        } else { $("#<%= ddl3.ClientID %>").val("0"); }
                        newOption = "<option value='5'>External Cause of Injury</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                        if (extCauseOfInj < 12) {
                            $("#<%= ddl4.ClientID %>").val("1");
                        } else {
                            $("#<%= ddl4.ClientID %>").val("0");
                        }
                        if (document.getElementById('<%= providerDiagOutput.ClientID %>') != null) {
                            document.getElementById('<%= providerDiagOutput.ClientID %>').innerHTML = table;
                        }
                        localStorage.setItem("diagTable", "" + table + "");
                        DiagClearFields();
                        return false;

                    }
                    else {
                        if (document.getElementById('<%= providerDiagOutput.ClientID %>') != null) {
                            document.getElementById('<%= providerDiagOutput.ClientID %>').innerHTML = "";
                        }
                        return false;
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=rfvDiagnosiscode]").text('Diagnosis Code is invalid');
                }
            });

        }

    };

    function binddataSerDet() {
          var ClaimType = $("#<%= hdnClaimType_Diagnosis.ClientID %>").val();
        var hdnClaimType_Diag = $("#<%= hdnClaimType_Diagnosis.ClientID %>").val();
        var hdnClaimIdDiag = $("#<%= hdnClaimIdDiagnosis.ClientID %>").val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "PUT",
            url: webApiClaims + "BindDiagCode?hdnClaimIdDiag=" + hdnClaimIdDiag + "&&hdnClaimType_Diag=" + hdnClaimType_Diag,
            //data: '{hdnClaimIdDiag: "' + hdnClaimIdDiag + '" , hdnClaimType_Diag: "' + hdnClaimType_Diag + '" }',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                var claimDiagnosisList = result; { 
                 
                if (claimDiagnosisList.length > 0) 
                    var table;                    
                    if ((ClaimType == '0')) {
                        clearServiceDetailsOptionsDental();
                        var newOption = "<option value=''></option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").append(newOption);
                        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").append(newOption);
                        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").append(newOption);
                        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").append(newOption);
                    }
                    if ((ClaimType == '2')) {
                        clearServiceDetailsOptionsProf();
                        var newOption = "<option value=''></option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").append(newOption);
                        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").append(newOption);
                        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").append(newOption);
                        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").append(newOption);
                    }
                    var count = 0; var admittingCount = 0; var reasonToVisitCount = 0; var extCauseOfInj = 0; var totalCount = 0; var othercount = 0;
                    for (var i = 0; i < claimDiagnosisList.length; i++) {
                        totalCount++;
                        var sequence = result[i].sequence;
                        var DiagnosisCode = result[i].DiagnosisCode;
                        var ICDVersion = result[i].ICDVersion;
                        var PresentOnAdmission = result[i].PreasentOnAdmission;
                        var DianosisCodeDescription = result[i].DianosisCodeDescription;
                        var Claims_DIAGNOSIS_ID = result[i].Claims_Diagnosis_ID;
                        var Claim_ID = result[i].Claim_ID;
                        if (sequence == "Principal") { count++; }
                        if (sequence == "Admitting") { admittingCount++; }
                        if (sequence == "External Cause of Injury") { extCauseOfInj++; }
                        if (sequence == "Patient Reason for Visit") { reasonToVisitCount++; }
                        if (sequence == "Other") { othercount++; }
                        if ((ClaimType == '0')) {
                            var newOption = "<option value='" + sequence + "'>" + sequence + "</option>";

                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").attr("disabled", false);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").append(newOption);
                        }
                        if ((ClaimType == '2')) {
                            var newOption = "<option value='" + sequence + "'>" + sequence + "</option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").attr("disabled", false);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").append(newOption);
                        }
                       
                       
                    };                   
                        return false;

                    }
                   
                },
                
            });

    }; 

 
    function disablefielda() {
        document.getElementById('<%= ddlPresentAdmission.ClientID %>').disabled = true;
        document.getElementById('<%= ddlICDVer.ClientID %>').disabled = true;
        document.getElementById('<%= txtDiagnosisCode.ClientID %>').disabled = true;
        document.getElementById('<%= ddlDiagnosisSequenceDescription.ClientID %>').disabled = true;
    }
    function SeqDescChange() {
        var ClaimType = $("#<%= hdnClaimType_Diagnosis.ClientID %>").val();       
        if (ClaimType == '1') {
            var ddlSeqDesc = $("#<%=ddlDiagnosisSequenceDescription.ClientID %> option:selected").text();
            if ((ddlSeqDesc == "Admitting") || (ddlSeqDesc == "Patient Reason for Visit")) {
                $("#<%=ddlPresentAdmission.ClientID %>").attr("disabled", true);
                document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlPresentAdmission").options[0].selected = true;
            } else {

                $("#<%=ddlPresentAdmission.ClientID %>").attr("disabled", false);
            }
            return false;
        }
    }
    function loadAddDiagnososCode() {
        if ($("#<%= hdnClaimType_Diagnosis.ClientID %>").first().val() == "1") {
            var SequenceDescription = $("#<%=ddlDiagnosisSequenceDescription.ClientID %> option:selected").text();
            var ICDVer = $("#<%=ddlICDVer.ClientID %> option:selected").text();
            var PresentAdmission = $("#<%=ddlPresentAdmission.ClientID %> option:selected").text();
            var DiagCode = $("#<%= txtDiagnosisCode.ClientID %>").first().val();

            if (SequenceDescription == "Patient Reason for Visit") {
                if (ICDVer != "" && DiagCode != "") {
                    document.getElementById('<%= ddlDiagnosisSequenceDescription.ClientID %>').disabled = true;
                    document.getElementById('<%= txtDiagnosisCode.ClientID %>').disabled = true;
                    document.getElementById('<%= ddlICDVer.ClientID %>').disabled = true;
                    document.getElementById('<%= ddlPresentAdmission.ClientID %>').disabled = true;
                    document.getElementById('<%= btnDiagnosis.ClientID %>').style.display = 'none';
                   <%-- document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';--%>
                }
            }
            if (SequenceDescription != "Patient Reason for Visit") {
                if (ICDVer != "" && DiagCode != "" && SequenceDescription != "" && PresentAdmission != "") {
                    document.getElementById('<%= ddlDiagnosisSequenceDescription.ClientID %>').disabled = true;
                    document.getElementById('<%= txtDiagnosisCode.ClientID %>').disabled = true;
                    document.getElementById('<%= ddlICDVer.ClientID %>').disabled = true;
                    document.getElementById('<%= ddlPresentAdmission.ClientID %>').disabled = true;
                    document.getElementById('<%= btnDiagnosis.ClientID %>').style.display = 'none';
                   <%-- document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';--%>
                }
            }
        }
        if (($("#<%= hdnClaimType_Diagnosis.ClientID %>").first().val() == "0") || ($("#<%= hdnClaimType_Diagnosis.ClientID %>").first().val() == "2")) {
            var ICDVer = $("#<%=ddlICDVer.ClientID %> option:selected").text();
            var DiagCode = $("#<%= txtDiagnosisCode.ClientID %>").first().val();
            if (ICDVer != "" && DiagCode != "") {
                document.getElementById('<%= txtDiagnosisCode.ClientID %>').disabled = true;
                    document.getElementById('<%= ddlICDVer.ClientID %>').disabled = true;
                    document.getElementById('<%= btnDiagnosis.ClientID %>').style.display = 'none';
                 <%--   document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';--%>
            }
        }
    }

    function DiaDisableEnableConditionAddButton() {
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_btnDiagnosisAdd").prop("disabled", true)
        }, 50);
        setTimeout(function () {
            $("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_btnDiagnosisAdd").prop('disabled', false);
        }, 1000);
    }
    function DiagnosisCodetextChange() {
        $("[id*=rfvDiagnosiscode]").text("");
        $("[id*=lblDiagnosisDescription]").text("");
        var DiagnosisCode = $("#<%= txtDiagnosisCode.ClientID %>").first().val();
        var ddlICDVer = $("#<%=ddlICDVer.ClientID %>").val();
        if ((DiagnosisCode != undefined) && (DiagnosisCode != "")) {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "GetDiagnosisCodeDescrption?Code=" + DiagnosisCode + "&&lICDVer=" + ddlICDVer,
                //data: '{Code: "' + DiagnosisCode + '", lICDVer:"' + ddlICDVer + '"}',
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
                        $("[id*=lblDiagnosisDescription]").text("");
                        $("[id*=rfvDiagnosiscode]").text('Diagnosis Code is invalid');
                    }
                    else {
                        $("[id*=lblDiagnosisDescription]").text(result[0]["DiagDesc"]);
                        $("[id*=lblDiagDesc]").val(result[0]["DiagDesc"]);
                        $("[id*=rfvDiagnosiscode]").text("");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=rfvDiagnosiscode]").text('Diagnosis Code is invalid');
                }
            });
        }
    }
    function gridViewDiagnosisCodetextChange(lnk) {
        var gridindex = lnk.parentNode.parentNode.rowIndex;       
        var inputs = grid.rows[gridindex].getElementsByTagName("INPUT");
        var DiagnosisCode = inputs[0].value;
        var ddldropdown = $("[id*=drpdownIcdVersion]");
        var selectedText = ddldropdown.find("option:selected").text();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetDiagnosisCodeDescrption?Code=" + DiagnosisCode + "&&lICDVer=" + selectedText,
            //data: '{Code: "' + DiagnosisCode + '", lICDVer:"' + selectedText + '"}',
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
                    var row = $(lnk).closest("tr");

                    $("[id*=rfvDiagnosiscode]").text('Diagnosis Code is invalid');
                }
                else {
                    var row = $(lnk).closest("tr");
                    row.find("[id*=lblEditDiagnosisDescription]").text(result[0]["DiagDesc"]);
                    $("[id*=rfvDiagnosiscode]").text('');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=rfvDiagnosiscode]").text('Diagnosis Code is invalid');
            }
        });
    }

    function addDiagnosisCode() {
        
        var ClaimType = $("#<%= hdnClaimType_Diagnosis.ClientID %>").val();        
        var claimDiagnosisList;
        var validateField = "";
        if (ClaimType == '1') {
            var sequence = $("#<%=ddlDiagnosisSequenceDescription.ClientID %> option:selected").val();
        } else { var sequence = null; }
        var DiagCode = $("#<%= txtDiagnosisCode.ClientID %>").first().val();        
        var ICDVer = $("#<%=ddlICDVer.ClientID %> option:selected").text();
        if (ClaimType == '1') {
            var presentOnAdmission = $("#<%=ddlPresentAdmission.ClientID %> option:selected").text();
        } else { var presentOnAdmission = null; }
        var DiagDesc = $("#<%= lblDiagnosisDescription.ClientID %>").html();
        validateFields();
        var hdnClaimIdDiag = $("#<%= hdnClaimIdDiagnosis.ClientID %>").val();
        var hdnClaimType_Diag = $("#<%= hdnClaimType_Diagnosis.ClientID %>").val();
        var hdnDiagnosispanel = $("#<%= hdnDiagnosispanel.ClientID %>").val();        
        if (validateField === "true") {
            // debugger;
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "GET",
                url: webApiClaims + "AddDiagnosisCode?hdnClaimIdDiag=" + hdnClaimIdDiag + "&&hdnClaimType_Diag=" + hdnClaimType_Diag + "&&sequence=" + sequence + "&&DiagCode=" + DiagCode + "&&ICDVer=" + ICDVer + "&&presentOnAdmission=" + presentOnAdmission + "&&DiagDesc=" + DiagDesc + "&&User=<%=HttpContext.Current.User.Identity.Name%>",
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
                    if (result[0].lblErrorMessageDiagnosis) {
                        $("[id*=seqDescError]").text(result[0].lblErrorMessageDiagnosis);
                    }
                    else {
                    var table;
                    if (claimDiagnosisList.length > 0) {

                        if (ClaimType == '1') {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Diagnosis Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:10px; scope='col'>Present On Admission</th><th style='width:30px; scope='col'>Diagnosis Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        } else { table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Diagnosis Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:30px; scope='col'>Diagnosis Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>"; }
                        if ((ClaimType == '0')) {
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").attr("disabled", false);
                            clearServiceDetailsOptionsDental();
                            var newOption = "<option value=''></option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").append(newOption);
                        }
                        if ((ClaimType == '2')) {
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").attr("disabled", false);
                            clearServiceDetailsOptionsProf();
                            var newOption = "<option value=''></option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").append(newOption);
                        }
                        var count = 0; var admittingCount = 0; var reasonToVisitCount = 0; var extCauseOfInj = 0; var totalCount = 0; var othercount = 0;
                        for (var i = 0; i < claimDiagnosisList.length; i++) {
                            totalCount++;
                            var sequence = result[i].sequence;
                            var DiagnosisCode = result[i].DiagnosisCode;
                            var ICDVersion = result[i].ICDVersion;
                            var PresentOnAdmission = result[i].PreasentOnAdmission;
                            var DianosisCodeDescription = result[i].DianosisCodeDescription;
                            var Claims_DIAGNOSIS_ID = result[i].Claims_Diagnosis_ID;
                            var Claim_ID = result[i].Claim_ID;
                            if (sequence == "Principal") { count++; }
                            if (sequence == "Admitting") { admittingCount++; }
                            if (sequence == "External Cause of Injury") { extCauseOfInj++; }
                            if (sequence == "Patient Reason for Visit") { reasonToVisitCount++; }
                            if (sequence == "Other") { othercount++; }
                            if ((ClaimType == '0')) {
                                var newOption = "<option value='" + sequence + "'>" + sequence + "</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").append(newOption);
                            }
                            if ((ClaimType == '2')) {
                                var newOption = "<option value='" + sequence + "'>" + sequence + "</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").append(newOption);
                            }
                            if (ClaimType == '1') {
                                if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_txtstatus2').value == 'Pending Submission') {
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + PresentOnAdmission + "</span></td><td>" + DianosisCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditClaimsdiagnosisLineItem(\"" + Claims_DIAGNOSIS_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimDiagnosisLineitem(\"" + Claims_DIAGNOSIS_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr>";
                                }
                                else {
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + PresentOnAdmission + "</span></td><td>" + DianosisCodeDescription + "</td></tr>";
                                }
                            }
                            else
                            {
                                if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_txtstatus2').value == 'Pending Submission') {
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td>" + DianosisCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditClaimsdiagnosisLineItem(\"" + Claims_DIAGNOSIS_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimDiagnosisLineitem(\"" + Claims_DIAGNOSIS_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr>"
                                } else {
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td>" + DianosisCodeDescription + "</td></tr>";
                                }
                            }
                        };
                        table = table + "</tbody></table>";
                        localStorage.setItem("diagTable", "" + table + "");
                        TotalCountintheGrid = totalCount;
                        if (ClaimType == '1') {
                            if (totalCount >= 41) { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden"; }
                            else { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible"; }
                        }
                        else if (ClaimType == '2') {
                            if (totalCount >= 12) { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden"; }
                            else { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible"; }
                        }
                        else if (ClaimType == '0') {
                            if (totalCount >= 4) { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden"; }
                            else { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible"; }
                        }

                        $("#<%=ddlDiagnosisSequenceDescription.ClientID %>").empty();
                        var newOption = "<option value=''></option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);

                        newOption = "<option value='1'>Principal</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                        if (count < 1) {
                            $("#<%= ddl1.ClientID %>").val("0");
                        } else { $("#<%= ddl1.ClientID %>").val("1"); }
                        newOption = "<option value='2'>Admitting</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                        if (admittingCount < 1) {
                            $("#<%= ddl2.ClientID %>").val("0");
                        } else { $("#<%= ddl2.ClientID %>").val("1"); }
                        newOption = "<option value='3'>Other</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                        if (othercount < 24) {
                            $("#<%= ddl5.ClientID %>").val("1");
                        } else { $("#<%= ddl5.ClientID %>").val("0"); }
                        newOption = "<option value='4'>Patient Reason for Visit</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                        if (reasonToVisitCount < 3) {
                            $("#<%= ddl3.ClientID %>").val("0");
                        } else { $("#<%= ddl3.ClientID %>").val("1"); }
                        newOption = "<option value='5'>External Cause of Injury</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                        if (extCauseOfInj < 12) {
                            $("#<%= ddl4.ClientID %>").val("0");
                        } else { $("#<%= ddl4.ClientID %>").val("1"); }
                        document.getElementById('<%= providerDiagOutput.ClientID %>').innerHTML = table;
                        DiagClearFields();
                    }
                    else {
                        DiagClearFields();
                    }
                }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=seqDescError]").text('Diagnosis code not found.');
                }
            });
        }
        return false;

        function validateFields() {
            if ((sequence === null || sequence === "" || sequence === undefined) && (ClaimType == '1')) {

                validateField = "false";
                $("#<%= seqDescError.ClientID %>").html("*Sequence description is required");
                    return false;
                
            }
            else {
                validateField = "true";
                $("#<%= seqDescError.ClientID %>").html("");
            }
            if ((DiagDesc === null || DiagDesc === "" || DiagDesc === undefined)) {

                validateField = "false";
                $("[id*=rfvDiagnosiscode]").text('Diagnosis Code is invalid');
                    return false;
                
            }
            else {
                validateField = "true";
                $("[id*=rfvDiagnosiscode]").text('');
             }           
            
            if (DiagCode === null || DiagCode === "" || DiagCode === undefined) {
                validateField = "false";
                $("#<%= rfvDiagnosiscode.ClientID %>").html("*Diagnosis Code is required");
                return false;
            }
            else {
                validateField = "true";
                $("#<%= rfvDiagnosiscode.ClientID %>").html("");
            }
            
            if (ICDVer === null || ICDVer === "" || ICDVer === undefined) {
                validateField = "false";
                $("#<%= reqICDVersion.ClientID %>").html("*ICD version is required");
                return false;
            }
            else {
                validateField = "true";
                $("#<%= reqICDVersion.ClientID %>").html("");
            }
         
            if ((presentOnAdmission === null || presentOnAdmission === "" || presentOnAdmission === undefined) && (ClaimType == '1')) {
                var ddlSeqDesc = $("#<%=ddlDiagnosisSequenceDescription.ClientID %> option:selected").text();
                if ((ddlSeqDesc != "Admitting") && (ddlSeqDesc != "Patient Reason for Visit")) {
                    validateField = "false";
                    $("#<%= lblPresentOnAdmissionEror.ClientID %>").html("*Present on Admission is required");
                    return false;
                }
            }
            else {
                $("#<%= lblPresentOnAdmissionEror.ClientID %>").html("");
                validateField = "true";
            }

         
        }
    }

    function EditClaimsdiagnosisLineItem(claim_diag_info_id, claim_id,control) {
        $(control).closest('tr').find('input[type=button]').prop('disabled', true);
        /*bindDropDown();*/
        var claim_type = $("#<%= hdnClaimType_Diagnosis.ClientID %>").val();
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiClaims + "GetIndDiagnosisCode?claim_id=" + claim_id + "&&claim_diag_info_id=" + claim_diag_info_id + "&&claim_type=" + claim_type, 
            //data: '{claim_id: "' + claim_id + '", claim_diag_info_id:"' + claim_diag_info_id + '", claim_type:"' + claim_type + '"}',
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
                    $("#<%=ddlDiagnosisSequenceDescription.ClientID %>").empty();
                    var newOption = "<option value=''></option>";                   
                    $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                    if ($("#<%= ddl1.ClientID %>").val() != "1") {
                        newOption = "<option value='1'>Principal</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                    }
                    if ($("#<%= ddl2.ClientID %>").val() != "1") {
                     
                        newOption = "<option value='2'>Admitting</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                    }
                    // always display "other"
                    newOption = "<option value='3'>Other</option>";
                    $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                    if ($("#<%= ddl3.ClientID %>").val() != "1") {
                        newOption = "<option value='4'>Patient Reason for Visit</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                    }
                    if ($("#<%= ddl4.ClientID %>").val() != "1") {
                        newOption = "<option value='5'>External Cause of Injury</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                    }
                            
                    for (var i = 0; i < claimDiagnosisList.length; i++) {                       
                        var sequence = result[i].sequence;
                        var DiagnosisCode = result[i].DiagnosisCode;
                        var ICDVersion = result[i].ICDVersion;
                        var PresentOnAdmission = result[i].PreasentOnAdmission;
                        var DianosisCodeDescription = result[i].DianosisCodeDescription;
                        var Claims_DIAGNOSIS_ID = result[i].Claims_Diagnosis_ID;
                        var Claim_ID = result[i].Claim_ID;
                        if (sequence == "Principal") {
                            newOption = "<option value='1'>Principal</option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                            $('#<%=ddlDiagnosisSequenceDescription.ClientID%>').val('1');
                        }
                        if (sequence == "Admitting") {                           
                            newOption = "<option value='2'>Admitting</option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                            $('#<%=ddlDiagnosisSequenceDescription.ClientID%>').val('2');
                        }
                        if (sequence == "Other") {
                            if ($('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription option:contains("' + sequence + '")')) {
                                $('#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription option:contains("' + sequence + '")').prop('selected', true);
                            }
                            else {
                                newOption = "<option value='3'>Other</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                                $('#<%=ddlDiagnosisSequenceDescription.ClientID%>').val('3');
                            }
                        }
                        if (sequence == "Patient Reason for Visit") {
                            if ($("#<%= ddl3.ClientID %>").val() == "1") {

                                newOption = "<option value='4'>Patient Reason for Visit</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                            }                            
                            $('#<%=ddlDiagnosisSequenceDescription.ClientID%>').val('4');
                        }
                        if (sequence == "External Cause of Injury") {
                            if ($("#<%= ddl4.ClientID %>").val() == "1") {

                                newOption = "<option value='5'>External Cause of Injury</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                            }
                            $('#<%=ddlDiagnosisSequenceDescription.ClientID%>').val('5');
                        }
                        $("[id*=txtDiagnosisCode]").val("" + DiagnosisCode + "");
                        $("[id*=hdnDiagnosisCliamInfoId]").val("" + Claims_DIAGNOSIS_ID + "");
                        if (ICDVersion == "ICD 10") { document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlICDVer").options[1].selected = true; }
                        if (ICDVersion == "ICD 9") { document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlICDVer").options[2].selected = true; }
                        if (PresentOnAdmission == "Yes" ) { document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlPresentAdmission").options[1].selected = true; }
                        if (PresentOnAdmission == "No" ) { document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlPresentAdmission").options[2].selected = true; }
                        if (PresentOnAdmission == "Unknown" ) { document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlPresentAdmission").options[3].selected = true; }
                        if (PresentOnAdmission == "Not Applicable") { document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlPresentAdmission").options[4].selected = true; }
                        document.getElementById('<%= lblDiagnosisDescription.ClientID%>').innerHTML = DianosisCodeDescription;
                        document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden";
                        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "visible";
                        document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "visible";
                        return false;
                    }; 
                   
                }               
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=rfvDiagnosiscode]").text('Diagnosis Code is invalid');
            }
        });

    }

    function DeleteClaimDiagnosisLineitem(claim_diag_info_id) {        
        var ClaimType = $("#<%= hdnClaimType_Diagnosis.ClientID %>").val();
        var result1 = confirm("Are you sure you want to delete?");
        if (result1) {
            var ClaimType = $("#<%= hdnClaimType_Diagnosis.ClientID %>").val();
            var hdnClaimType_Diag = $("#<%= hdnClaimType_Diagnosis.ClientID %>").val();
            var hdnClaimIdDiag = $("#<%= hdnClaimIdDiagnosis.ClientID %>").val();
            if (ClaimType == '0' || ClaimType == '2')
            {
                var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "GET",                    
                    url: webApiClaims + "CheckDiagnosisCodeUse?claimid=" + hdnClaimIdDiag + "&&diagnosisCode=" + claim_diag_info_id + "",
                    headers: {
                        "Access-Control-Allow-Origin": "*",
                        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                        "Authorization": "Bearer " + APIToken
                    },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result2) {
                        if (result2 == "") {
                            //result = result;
                            $("[id*=rfvDiagnosiscode]").html("" + result2 + "");
                            var APIToken = $("[id*=hdnAccessToken]").val();
                            $.ajax({
                                type: "DELETE",
                                url: webApiClaims + "DeleteDiagnosisCode?hdnClaimIdDiag=" + hdnClaimIdDiag + "&&claimDiag_Info_Id=" + claim_diag_info_id + "&&hdnClaimType_Diag=" + hdnClaimType_Diag,
                                //data: '{hdnClaimIdDiag: "' + hdnClaimIdDiag + '" , claimDiag_Info_Id: "' + claim_diag_info_id + '" , hdnClaimType_Diag: "' + hdnClaimType_Diag + '" }',
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
                                        var table;
                                        if (ClaimType == '1') {
                                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Diagnosis Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:10px; scope='col'>Present On Admission</th><th style='width:30px; scope='col'>Diagnosis Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                                        } else { table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Diagnosis Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:30px; scope='col'>Diagnosis Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>"; }

                                        if ((ClaimType == '0')) {
                                            clearServiceDetailsOptionsDental();
                                            var newOption = "<option value=''></option>";
                                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").append(newOption);
                                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").append(newOption);
                                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").append(newOption);
                                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").append(newOption);
                                        }
                                        if ((ClaimType == '2')) {
                                            clearServiceDetailsOptionsProf();
                                            var newOption = "<option value=''></option>";
                                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").append(newOption);
                                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").append(newOption);
                                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").append(newOption);
                                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").append(newOption);
                                        }
                                        var count = 0; var admittingCount = 0; var reasonToVisitCount = 0; var extCauseOfInj = 0; var totalCount = 0; var othercount = 0;
                                        for (var i = 0; i < claimDiagnosisList.length; i++) {
                                            totalCount++;
                                            var sequence = result[i].sequence;
                                            var DiagnosisCode = result[i].DiagnosisCode;
                                            var ICDVersion = result[i].ICDVersion;
                                            var PresentOnAdmission = result[i].PreasentOnAdmission;
                                            var DianosisCodeDescription = result[i].DianosisCodeDescription;
                                            var Claims_DIAGNOSIS_ID = result[i].Claims_Diagnosis_ID;
                                            var Claim_ID = result[i].Claim_ID;
                                            if (sequence == "Principal") { count++; }
                                            if (sequence == "Admitting") { admittingCount++; }
                                            if (sequence == "External Cause of Injury") { extCauseOfInj++; }
                                            if (sequence == "Patient Reason for Visit") { reasonToVisitCount++; }
                                            if (sequence == "Other") { othercount++; }
                                            if ((ClaimType == '0')) {
                                                var newOption = "<option value='" + sequence + "'>" + sequence + "</option>";
                                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").append(newOption);
                                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").append(newOption);
                                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").append(newOption);
                                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").append(newOption);
                                            }
                                            if ((ClaimType == '2')) {
                                                var newOption = "<option value='" + sequence + "'>" + sequence + "</option>";
                                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").append(newOption);
                                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").append(newOption);
                                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").append(newOption);
                                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").append(newOption);
                                            }

                                            if (ClaimType == '1') {
                                                if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_txtstatus2').value == 'Pending Submission') {
                                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + PresentOnAdmission + "</span></td><td>" + DianosisCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditClaimsdiagnosisLineItem(\"" + Claims_DIAGNOSIS_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimDiagnosisLineitem(\"" + Claims_DIAGNOSIS_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr>";
                                                }
                                                else {
                                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + PresentOnAdmission + "</span></td><td>" + DianosisCodeDescription + "</td></tr>";
                                                }
                                            } else {
                                                if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_txtstatus2').value == 'Pending Submission') {
                                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td>" + DianosisCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditClaimsdiagnosisLineItem(\"" + Claims_DIAGNOSIS_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimDiagnosisLineitem(\"" + Claims_DIAGNOSIS_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                                                } else {
                                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td>" + DianosisCodeDescription + "</td></tr>";
                                                }
                                            }
                                        };
                                        TotalCountintheGrid = totalCount;
                                        if (ClaimType == '1') {
                                            if (totalCount >= 41) { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden"; }
                                            else { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible"; }
                                        }
                                        else if (ClaimType == '2') {
                                            if (totalCount >= 12) { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden"; }
                                            else { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible"; }
                                        }
                                        else if (ClaimType == '0') {
                                            if (totalCount >= 4) { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden"; }
                                            else { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible"; }
                                        }
                                        table = table + "</tbody></table>";
                                        $("#<%=ddlDiagnosisSequenceDescription.ClientID %>").empty();
                                        var newOption = "<option value=''></option>";
                                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);

                                        newOption = "<option value='1'>Principal</option>";
                                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                                        if (count < 1) {
                                            $("#<%= ddl1.ClientID %>").val("0");
                                        } else { $("#<%= ddl1.ClientID %>").val("1"); }
                                        newOption = "<option value='2'>Admitting</option>";
                                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                                        if (admittingCount < 1) {
                                            $("#<%= ddl2.ClientID %>").val("0");
                                        } else { $("#<%= ddl2.ClientID %>").val("1"); }
                                        newOption = "<option value='3'>Other</option>";
                                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                                        if (othercount < 24) {
                                            $("#<%= ddl5.ClientID %>").val("0");
                                        }
                                        newOption = "<option value='4'>Patient Reason for Visit</option>";
                                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                                        if (reasonToVisitCount < 3) {
                                            $("#<%= ddl3.ClientID %>").val("0");
                                        } else { $("#<%= ddl3.ClientID %>").val("1"); }
                                        newOption = "<option value='5'>External Cause of Injury</option>";
                                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                                        if (extCauseOfInj < 12) {
                                            $("#<%= ddl4.ClientID %>").val("0");
                                        } else { $("#<%= ddl4.ClientID %>").val("1"); }
                                        document.getElementById('<%= providerDiagOutput.ClientID %>').innerHTML = table;
                                        localStorage.setItem("diagTable", "" + table + "");
                                        DiagClearFields();
                                        return false;

                                    }
                                    else {
                                        if (ClaimType == '1') {
                                            $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").empty();
                                            var newOptions = "<option value=''></option>";
                                            $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOptions);
                                            newOptions = "<option value='1'>Principal</option>";
                                            $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOptions);
                                            newOptions = "<option value='2'>Admitting</option>";
                                            $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOptions);
                                            newOptions = "<option value='3'>Other</option>";
                                            $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOptions);
                                            newOptions = "<option value='4'>Patient Reason for Visit</option>";
                                            $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOptions);
                                            newOptions = "<option value='5'>External Cause of Injury</option>";
                                            $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOptions);
                                        }
                                        document.getElementById('<%= providerDiagOutput.ClientID %>').innerHTML = "";
                                        return false;
                                    }
                                },
                                error: function (jqXHR, textStatus, errorThrown) {
                                    $("[id*=rfvDiagnosiscode]").text('Diagnosis Code is invalid');
                                }
                            });
                        }
                        else {
                            //result = result.d;
                            $("[id*=rfvDiagnosiscode]").html("" + result2 + "");
                        }
                    },

                    error: function (jqXHR, textStatus, errorThrown) {
                    }
                });
            }
            else
            {
                var APIToken = $("[id*=hdnAccessToken]").val();
                $.ajax({
                    type: "DELETE",
                    url: webApiClaims + "DeleteDiagnosisCode?hdnClaimIdDiag=" + hdnClaimIdDiag + "&&claimDiag_Info_Id=" + claim_diag_info_id + "&&hdnClaimType_Diag=" + hdnClaimType_Diag,
                    //data: '{hdnClaimIdDiag: "' + hdnClaimIdDiag + '" , claimDiag_Info_Id: "' + claim_diag_info_id + '" , hdnClaimType_Diag: "' + hdnClaimType_Diag + '" }',
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
                            var table;
                            if (ClaimType == '1') {
                                table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Diagnosis Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:10px; scope='col'>Present On Admission</th><th style='width:30px; scope='col'>Diagnosis Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                            } else { table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Diagnosis Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:30px; scope='col'>Diagnosis Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>"; }

                            if ((ClaimType == '0')) {
                                clearServiceDetailsOptionsDental();
                                var newOption = "<option value=''></option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").append(newOption);
                            }
                            if ((ClaimType == '2')) {
                                clearServiceDetailsOptionsProf();
                                var newOption = "<option value=''></option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").append(newOption);
                            }
                            var count = 0; var admittingCount = 0; var reasonToVisitCount = 0; var extCauseOfInj = 0; var totalCount = 0; var othercount = 0;
                            for (var i = 0; i < claimDiagnosisList.length; i++) {
                                totalCount++;
                                var sequence = result[i].sequence;
                                var DiagnosisCode = result[i].DiagnosisCode;
                                var ICDVersion = result[i].ICDVersion;
                                var PresentOnAdmission = result[i].PreasentOnAdmission;
                                var DianosisCodeDescription = result[i].DianosisCodeDescription;
                                var Claims_DIAGNOSIS_ID = result[i].Claims_Diagnosis_ID;
                                var Claim_ID = result[i].Claim_ID;
                                if (sequence == "Principal") { count++; }
                                if (sequence == "Admitting") { admittingCount++; }
                                if (sequence == "External Cause of Injury") { extCauseOfInj++; }
                                if (sequence == "Patient Reason for Visit") { reasonToVisitCount++; }
                                if (sequence == "Other") { othercount++; }
                                if ((ClaimType == '0')) {
                                    var newOption = "<option value='" + sequence + "'>" + sequence + "</option>";
                                    $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").append(newOption);
                                    $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").append(newOption);
                                    $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").append(newOption);
                                    $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").append(newOption);
                                }
                                if ((ClaimType == '2')) {
                                    var newOption = "<option value='" + sequence + "'>" + sequence + "</option>";
                                    $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").append(newOption);
                                    $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").append(newOption);
                                    $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").append(newOption);
                                    $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").append(newOption);
                                }

                                if (ClaimType == '1') {
                                    if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_txtstatus2').value == 'Pending Submission') {
                                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + PresentOnAdmission + "</span></td><td>" + DianosisCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditClaimsdiagnosisLineItem(\"" + Claims_DIAGNOSIS_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimDiagnosisLineitem(\"" + Claims_DIAGNOSIS_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr>";
                                    } else {
                                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + PresentOnAdmission + "</span></td><td>" + DianosisCodeDescription + "</td></tr>";
                                    }
                                } else {
                                    if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_txtstatus2').value == 'Pending Submission') {
                                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td>" + DianosisCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditClaimsdiagnosisLineItem(\"" + Claims_DIAGNOSIS_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimDiagnosisLineitem(\"" + Claims_DIAGNOSIS_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr>";
                                    } else {
                                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td>" + DianosisCodeDescription + "</td></tr>";
                                    }
                                }
                            };
                            TotalCountintheGrid = totalCount;
                            if (ClaimType == '1') {
                                if (totalCount >= 41) { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden"; }
                                else { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible"; }
                            }
                            else if (ClaimType == '2') {
                                if (totalCount >= 12) { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden"; }
                                else { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible"; }
                            }
                            else if (ClaimType == '0') {
                                if (totalCount >= 4) { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden"; }
                                else { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible"; }
                            }
                            table = table + "</tbody></table>";
                            $("#<%=ddlDiagnosisSequenceDescription.ClientID %>").empty();
                            var newOption = "<option value=''></option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);

                            newOption = "<option value='1'>Principal</option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                            if (count < 1) {
                                $("#<%= ddl1.ClientID %>").val("0");
                            } else { $("#<%= ddl1.ClientID %>").val("1"); }
                            newOption = "<option value='2'>Admitting</option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                            if (admittingCount < 1) {
                                $("#<%= ddl2.ClientID %>").val("0");
                            } else { $("#<%= ddl2.ClientID %>").val("1"); }
                            newOption = "<option value='3'>Other</option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                            if (othercount < 24) {
                                $("#<%= ddl5.ClientID %>").val("1");
                            } else { $("#<%= ddl5.ClientID %>").val("0"); }
                            newOption = "<option value='4'>Patient Reason for Visit</option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                            if (reasonToVisitCount < 3) {
                                $("#<%= ddl3.ClientID %>").val("0");
                            } else { $("#<%= ddl3.ClientID %>").val("1"); }
                            newOption = "<option value='5'>External Cause of Injury</option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                            if (extCauseOfInj < 12) {
                                $("#<%= ddl4.ClientID %>").val("0");
                            } else { $("#<%= ddl4.ClientID %>").val("1"); }

                            document.getElementById('<%= providerDiagOutput.ClientID %>').innerHTML = table;
                            localStorage.setItem("diagTable", "" + table + "");
                            DiagClearFields();
                            return false;

                        }
                        else {
                            if (ClaimType == '1') {
                                $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").empty();
                                var newOptions = "<option value=''></option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOptions);
                                newOptions = "<option value='1'>Principal</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOptions);
                                newOptions = "<option value='2'>Admitting</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOptions);
                                newOptions = "<option value='3'>Other</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOptions);
                                newOptions = "<option value='4'>Patient Reason for Visit</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOptions);
                                newOptions = "<option value='5'>External Cause of Injury</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOptions);
                            }
                            document.getElementById('<%= providerDiagOutput.ClientID %>').innerHTML = "";
                            return false;
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        $("[id*=rfvDiagnosiscode]").text('Diagnosis Code is invalid');
                    }
                });
            }
        }
     }

    function EditDiagnosisCode() {
        var ClaimType = $("#<%= hdnClaimType_Diagnosis.ClientID %>").val();
        var claimDiagnosisList;
        var validateField = "";
        if (ClaimType == '1') {
            var sequence = $("#<%=ddlDiagnosisSequenceDescription.ClientID %> option:selected").val();
        } else { sequence = null;}
          var DiagCode = $("#<%= txtDiagnosisCode.ClientID %>").first().val();
        var ICDVer = $("#<%=ddlICDVer.ClientID %> option:selected").text();
        if (ClaimType == '1') {
            var presentOnAdmission = $("#<%=ddlPresentAdmission.ClientID %> option:selected").text();
        } else { presentOnAdmission = null; }
          var DiagDesc = $("#<%= lblDiagnosisDescription.ClientID %>").html();
          validateFields();
        var hdnClaimIdDiag = $("#<%= hdnClaimIdDiagnosis.ClientID %>").val();
        var claimDiag_Info_Id = $("#<%= hdnDiagnosisCliamInfoId.ClientID %>").val();
          var hdnClaimType_Diag = $("#<%= hdnClaimType_Diagnosis.ClientID %>").val();
        var hdnDiagnosispanel = $("#<%= hdnDiagnosispanel.ClientID %>").val();
    
        if (validateField === "true") {
            var APIToken = $("[id*=hdnAccessToken]").val();
            $.ajax({
                type: "PUT",
                url: webApiClaims + "EditDiagnosisCode?hdnClaimIdDiag=" + hdnClaimIdDiag + "&&claimDiag_Info_Id=" + claimDiag_Info_Id + "&&hdnClaimType_Diag=" + hdnClaimType_Diag + "&&sequence=" + sequence + "&&DiagCode=" + DiagCode + "&&ICDVer=" + ICDVer + "&&presentOnAdmission=" + presentOnAdmission + "&&DiagDesc=" + DiagDesc + "&&userid=<%=HttpContext.Current.User.Identity.Name%>",
                //data: '{hdnClaimIdDiag: "' + hdnClaimIdDiag + '" , claimDiag_Info_Id: "' + claimDiag_Info_Id + '" , hdnClaimType_Diag: "' + hdnClaimType_Diag + '" , sequence: "' + sequence + '" , DiagCode: "' + DiagCode + '" , ICDVer: "' + ICDVer + '" , presentOnAdmission: "' + presentOnAdmission + '" , DiagDesc: "' + DiagDesc + '"  }',
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

                        var table;
                        if (ClaimType == '1') {
                            table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Diagnosis Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:10px; scope='col'>Present On Admission</th><th style='width:30px; scope='col'>Diagnosis Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                        } else { table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Sequence</th><th style='width:10px; scope='col'>Diagnosis Code</th><th style='width:10px; scope='col'>ICD Version</th><th style='width:30px; scope='col'>Diagnosis Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>"; }
                        if ((ClaimType == '0')) {
                            clearServiceDetailsOptionsDental();
                            var newOption = "<option value=''></option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").append(newOption);
                        }
                        if ((ClaimType == '2')) {
                            clearServiceDetailsOptionsProf();
                            var newOption = "<option value=''></option>";
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").append(newOption);
                            $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").append(newOption);
                        }
                        var count = 0; var admittingCount = 0; var reasonToVisitCount = 0; var extCauseOfInj = 0; var totalCount = 0; var othercount = 0;
                        for (var i = 0; i < claimDiagnosisList.length; i++) {
                            totalCount++;
                            var sequence = result[i].sequence;
                            var DiagnosisCode = result[i].DiagnosisCode;
                            var ICDVersion = result[i].ICDVersion;
                            var PresentOnAdmission = result[i].PreasentOnAdmission;
                            var DianosisCodeDescription = result[i].DianosisCodeDescription;
                            var Claims_DIAGNOSIS_ID = result[i].Claims_Diagnosis_ID;
                            var Claim_ID = result[i].Claim_ID;
                            if (sequence == "Principal") { count++; }
                            if (sequence == "Admitting") { admittingCount++; }
                            if (sequence == "External Cause of Injury") { extCauseOfInj++; }
                            if (sequence == "Patient Reason for Visit") { reasonToVisitCount++; }
                            if (sequence == "Other") { othercount++; }
                            if ((ClaimType == '0')) {
                                var newOption = "<option value='" + sequence + "'>" + sequence + "</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").append(newOption);
                            }
                            if ((ClaimType == '2')) {
                                var newOption = "<option value='" + sequence + "'>" + sequence + "</option>";
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").append(newOption);
                                $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").append(newOption);
                            }
                            if (ClaimType == '1') {
                                if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_txtstatus2').value == 'Pending Submission') {
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + PresentOnAdmission + "</span></td><td>" + DianosisCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditClaimsdiagnosisLineItem(\"" + Claims_DIAGNOSIS_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimDiagnosisLineitem(\"" + Claims_DIAGNOSIS_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr>";
                                } else {
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td><span title='Line' class='tNumber'>" + PresentOnAdmission + "</span></td><td>" + DianosisCodeDescription + "</td></tr>";
                                }
                            } else {
                                if (document.getElementById('ctl00_MainContent_uc5SubmitClaim_txtstatus2').value == 'Pending Submission') {
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td>" + DianosisCodeDescription + "</td><td><input type='button' value = 'Edit' onClick = 'return EditClaimsdiagnosisLineItem(\"" + Claims_DIAGNOSIS_ID + "\",\"" + Claim_ID + "\",this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteClaimDiagnosisLineitem(\"" + Claims_DIAGNOSIS_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr>";
                                } else {
                                    table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequence + "</span></td><td><span title='Line' class='tNumber'>" + DiagnosisCode + "</span></td><td><span  title='Line' class='tNumber'>" + ICDVersion + "</span></td><td>" + DianosisCodeDescription + "</td></tr>";
                                }
                            }
                        };
                        table = table + "</tbody></table>";
                        document.getElementById('<%= providerDiagOutput.ClientID %>').innerHTML = table;
                        localStorage.setItem("diagTable", "" + table + "");
                        TotalCountintheGrid = totalCount;
                        if (ClaimType == '1') {
                            if (totalCount >= 41) { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden"; }
                            else { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible"; }
                        }
                        else if (ClaimType == '2') {
                            if (totalCount >= 12) { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden"; }
                            else { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible"; }
                        }
                        else if (ClaimType == '0') {
                            if (totalCount >= 4) { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "hidden"; }
                            else { document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible"; }
                        }
                        $("#<%=ddlDiagnosisSequenceDescription.ClientID %>").empty();
                        var newOption = "<option value=''></option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);

                        newOption = "<option value='1'>Principal</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                        if (count < 1) {
                            $("#<%= ddl1.ClientID %>").val("0");
                        } else { $("#<%= ddl1.ClientID %>").val("1"); }
                        newOption = "<option value='2'>Admitting</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                        if (admittingCount < 1) {
                            $("#<%= ddl2.ClientID %>").val("0");
                        } else { $("#<%= ddl2.ClientID %>").val("1"); }
                        newOption = "<option value='3'>Other</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                        if (othercount < 24) {
                            $("#<%= ddl5.ClientID %>").val("1");
                        } else {
                            $("#<%= ddl5.ClientID %>").val("0");
                        }
                        newOption = "<option value='4'>Patient Reason for Visit</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                        if (reasonToVisitCount < 3) {
                            $("#<%= ddl3.ClientID %>").val("0");
                        } else { $("#<%= ddl3.ClientID %>").val("1"); }
                        newOption = "<option value='5'>External Cause of Injury</option>";
                        $("#ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").append(newOption);
                        if (extCauseOfInj < 12) {
                            $("#<%= ddl4.ClientID %>").val("0");
                        } else { $("#<%= ddl4.ClientID %>").val("1"); }
                    }
                    else {

                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("[id*=seqDescError]").text('Diagnosis code not found.');
                }
            });

            document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible";
            document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
            document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "hidden";
            DiagClearFields();
        }
        return false;

        function validateFields() {           
              if ((sequence === null || sequence === "" || sequence === undefined) && (ClaimType == '1')) {
                  validateField = "false";
                  $("#<%= seqDescError.ClientID %>").html("*Sequence description is required");
                return false;
            }
            else {
                validateField = "true";
                $("#<%= seqDescError.ClientID %>").html("");
            }
            if (DiagCode === null || DiagCode === "" || DiagCode === undefined) {
                validateField = "false";
                $("#<%= rfvDiagnosiscode.ClientID %>").html("*Diagnosis Code is required");
                return false;
            }
            else {
                validateField = "true";
                $("#<%= rfvDiagnosiscode.ClientID %>").html("");
            }
            if (ICDVer === null || ICDVer === "" || ICDVer === undefined) {
                validateField = "false";
                $("#<%= reqICDVersion.ClientID %>").html("*ICD version is required");
                return false;
            }
            else {
                validateField = "true";
                $("#<%= reqICDVersion.ClientID %>").html("");
            }
              if((presentOnAdmission === null || presentOnAdmission === "" || presentOnAdmission === undefined) && (ClaimType == '1')) {
                var ddlSeqDesc = $("#<%=ddlDiagnosisSequenceDescription.ClientID %> option:selected").text();
                if ((ddlSeqDesc != "Admitting") && (ddlSeqDesc != "Patient Reason for Visit")) {
                    validateField = "false";
                    $("#<%= lblPresentOnAdmissionEror.ClientID %>").html("*Present on Admission is required");
                    return false;
                }
              } 
            else {

                  $("#<%= lblPresentOnAdmissionEror.ClientID %>").html("");
                  validateField = "true";
              }

              if ((DiagDesc === null || DiagDesc === "" || DiagDesc === undefined)) {

                  validateField = "false";
                  $("[id*=rfvDiagnosiscode]").text('Diagnosis Code is invalid');
                  return false;

              }
              else {
                  validateField = "true";
                  $("[id*=rfvDiagnosiscode]").text('');
              }

          }
    }
    function CancelDiagFields() {
        
        var ClaimType = $("#<%= hdnClaimType_Diagnosis.ClientID %>").val();

        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlICDVer") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlICDVer").options[1].selected = true;
        }
        if (ClaimType == '1') {
            if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription") != null) {
                document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").options[0].selected = true;
            }
            if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlPresentAdmission") != null) {
                document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlPresentAdmission").options[0].selected = true;
            }
        }
        $("#<%= txtDiagnosisCode.ClientID %>").first().val("");
        if (document.getElementById('<%= lblDiagnosisDescription.ClientID%>') != null) {
            document.getElementById('<%= lblDiagnosisDescription.ClientID%>').innerHTML = '';
        }
        $("#<%= seqDescError.ClientID %>").html("");
        $("#<%= rfvDiagnosiscode.ClientID %>").html("");
        $("#<%= reqICDVersion.ClientID %>").html("");
        $("#<%= lblPresentOnAdmissionEror.ClientID %>").html("");        
        document.getElementById('<%= btnEdit.ClientID%>').style.visibility = "hidden";
        document.getElementById('<%= btnCancel.ClientID%>').style.visibility = "hidden";
        if (ClaimType == '0') {
            if (TotalCountintheGrid < 4) {
                document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible";
            }
        }
        if (ClaimType == '2') {
            if (TotalCountintheGrid < 12) {
                document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible";
            }
        }
        if (ClaimType == '1') {
            if (TotalCountintheGrid < 41) {
                document.getElementById('<%= btnDiagnosis.ClientID%>').style.visibility = "visible";
            }
        }
        var table = localStorage.getItem("diagTable");
        if (table != null && table != undefined && table != "") {
            document.getElementById('<%= providerDiagOutput.ClientID %>').innerHTML = table;
        }
        return false;
    }

    function DiagClearFields() {
        
        var ClaimType = $("#<%= hdnClaimType_Diagnosis.ClientID %>").val();

        if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlICDVer") != null) {
            document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlICDVer").options[1].selected = true;
        }
        if (ClaimType == '1') {
            if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription") != null) {
                document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlDiagnosisSequenceDescription").options[0].selected = true;
            }
            if (document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlPresentAdmission") != null) {
                document.getElementById("ctl00_MainContent_uc5SubmitClaim_ucDiagnosisCodePanel_ddlPresentAdmission").options[0].selected = true;
            }
        }
        $("#<%= txtDiagnosisCode.ClientID %>").first().val("");
        if (document.getElementById('<%= lblDiagnosisDescription.ClientID%>') != null) {
            document.getElementById('<%= lblDiagnosisDescription.ClientID%>').innerHTML = '';
        }
        $("#<%= seqDescError.ClientID %>").html("");
        $("#<%= rfvDiagnosiscode.ClientID %>").html("");
        $("#<%= reqICDVersion.ClientID %>").html("");
        $("#<%= lblPresentOnAdmissionEror.ClientID %>").html("");       
    }

    function clearServiceDetailsOptionsDental() {
        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDiagnosisDentalFirst").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalSecond").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalThird").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ucServiceDetailsDental_ddlDignosisDentalFourth").html("");
       
    }
    function clearServiceDetailsOptionsProf() {       
        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer1").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer2").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer3").html("");
        $("#ctl00_MainContent_uc5SubmitClaim_ProfessionalServiceLineDetails_ddlDiagnosisPointer4").html("");
      
    }

</script>
<asp:Panel ID="pnlDiagnosis" runat="server" Style="height: auto; width: 100%; min-height: 200px">
    <asp:HiddenField ID="hdnDiagnosispanel" runat="server" />
    <asp:HiddenField ID="hdnClaimIdDiagnosis" runat="server" />
    <asp:HiddenField ID="hdnClaimType_Diagnosis" runat="server" />
    <asp:HiddenField ID="hdnICN_Diagnosis" runat="server" />
    <asp:HiddenField ID="hdnDiagnosisCliamInfoId" runat="server" />
     <asp:HiddenField ID="ddl1" runat="server" />
     <asp:HiddenField ID="ddl2" runat="server" />
     <asp:HiddenField ID="ddl3" runat="server" />
     <asp:HiddenField ID="ddl4" runat="server" />
     <asp:HiddenField ID="ddl5" runat="server" />
    <div class="divGrid" style="padding-top: 10px">
        <div id="dvDeleteError" runat="server" style="color: red; padding-left: 30px;">
            <asp:Label ID="lblDeleteError" runat="server" ForeColor="Red"></asp:Label>

        </div>
        <div id="providerDiagOutput" runat="server"></div>

        <%-- <asp:GridView ID="gvDiagnosis" runat="server" Width="90%" AllowSorting="false" CssClass="gridview"  DataKeyNames="Claims_Diagnosis_Information_ID"
            AutoGenerateColumns="false" HorizontalAlign="Left" Style="margin-left: 40px; float: left"
            OnRowDeleting="gvDiagnosis_RowDeleting"
            OnRowEditing="gvDiagnosis_RowEditing"
            OnRowUpdating="gvDiagnosis_RowUpdating"
            OnRowCancelingEdit="gvDiagnosis_RowCancelingEdit"
            OnRowDataBound="gvDiagnosis_RowDataBound">
            <Columns>
                <asp:BoundField DataField="diag_seq" HeaderText="Sequence " ReadOnly="true" />

                <asp:TemplateField HeaderText="Sequence" HeaderStyle-CssClass="GridviewHeaderAsterisk">
                    <ItemTemplate>
                        <asp:Label ID="lblSequenceDescription" runat="server" Text='<%# Eval("seq_Desc")%>'></asp:Label>
                        <asp:HiddenField ID="hdnClaims_Diagnosis_Information_ID" Value='<%# Eval("Claims_Diagnosis_Information_ID")%>' runat="server" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="dropDownSeqDesc" runat="server" OnSelectedIndexChanged="dropDownSeqDesc_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Diagnosis Code" HeaderStyle-CssClass="GridviewHeaderAsterisk">
                    <ItemTemplate>
                        <asp:Label ID="lblDiagnosisCode" runat="server" Text='<%# Eval("diag_code")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtGridDiagnosisCode" runat="server" onChange="return gridViewDiagnosisCodetextChange(this)" AutoPostBack="true"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="ICD Version" HeaderStyle-CssClass="GridviewHeaderAsterisk">
                    <ItemTemplate>
                        <asp:Label ID="lblIcdVersion" runat="server" Text='<%# Eval("ICDVersion")%>'></asp:Label>
                    </ItemTemplate>

                    <EditItemTemplate>
                        <asp:DropDownList ID="drpdownIcdVersion" runat="server" onChange="return gridViewDiagnosisCodetextChange(this)" AutoPostBack="true">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Present On Admission" HeaderStyle-CssClass="GridviewHeaderAsterisk">
                    <ItemTemplate>
                        <asp:Label ID="lblPresentOnadmission" runat="server" Text='<%# Eval("Present_On_Admission")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="drpdownPresentOnAdmission" runat="server" OnSelectedIndexChanged="drpdownPresentOnAdmission_SelectedOndexChanged">
                        </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Diagnosis Code Description">
                    <ItemTemplate>
                        <asp:Label ID="lblGridDiagnosisDescription" runat="server" Text='<%# Eval("diagnosisDescription")%>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="lblEditDiagnosisDescription" runat="server" Enabled="false" />
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:CommandField ShowEditButton="true" ControlStyle-CssClass="btn btn-primary" ControlStyle-Font-Size="Medium" ControlStyle-Font-Underline="false"
                    ControlStyle-ForeColor="White" ControlStyle-BorderStyle="Solid" ControlStyle-Font-Bold="true" CausesValidation="false" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Button ID="btnDiagnosisDlt" Text="Delete" runat="server" CommandName="Delete" CssClass="btn btn-danger" Font-Bold="True"
                            OnClientClick='return confirm("Are you sure you want to delete this record?");' CausesValidation="false" />
                    </ItemTemplate>

                </asp:TemplateField>
                
                <%--<asp:CommandField ButtonType="Link" ShowDeleteButton="true" CausesValidation="false" ControlStyle-CssClass="btn btn-danger"
                    ControlStyle-ForeColor="White" ControlStyle-BorderStyle="Solid" />
            </Columns>
            <pagerstyle cssclass="gridpager" horizontalalign="Right" />
        <headerstyle cssclass="gridViewHeader" width="100px" />
        <alternatingrowstyle cssclass="gridViewAltRow" />
        <rowstyle cssclass="gridViewRow" />
        <footerstyle cssclass="gridViewFooter" />
        </asp:GridView>--%>
    </div>

    <div class="form-group row" style="padding-left: 20px">
        <div class="col-sm-12" runat="server" id="divInsert">

            <asp:HiddenField ID="hdnSequenceid" runat="server" />

            <div id="divSequencedescription" runat="server" class="col-sm-2 ">
                <span class="ohio-field-label"><span style="color: red" id="Seqdiv" runat="server">*</span><b>Sequence</b></span>
                <div>
                    <asp:DropDownList ID="ddlDiagnosisSequenceDescription" CssClass="formField" EnableViewState="true"
                        runat="server" AppendDataBoundItems="True" Visible="true"
                        Style="height: 30px; width: 100px; min-width: 100px;" OnChange="return SeqDescChange()">
                    </asp:DropDownList>
                    <%--<button id="btnloading1" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>--%>
                    <asp:Label runat="server" ID="seqDescError" ForeColor="Red"></asp:Label>
                    <%--<asp:RequiredFieldValidator runat="server" ID="rvSequencedescription" SetFocusOnError="true"
                        ValidationGroup="vgCarePlan" ControlToValidate="ddlDiagnosisSequenceDescription" ErrorMessage="*Sequence description is required" Text="*" Display="Dynamic" InitialValue="0" />--%>
                </div>
            </div>


            <div class="col-sm-2" id="divDiagnosisCode" runat="server">
                <div id="divDiagErrormsg" runat="server" style="color: red;"></div>
                <span class="ohio-field-label"><span runat="server" style="color: red">*</span><b>Diagnosis Code</b></span>
                <div>

                    <asp:TextBox ID="txtDiagnosisCode" onChange="return DiagnosisCodetextChange()" runat="server" CssClass="formFieldTextBoxSmall" Width="100px" CausesValidation="true" />
                    <button type="button" class="btn btn-link" id="btnDiagSearch2" onclick="getbuttondetail(this)" data-toggle="modal" data-target="#myDiagModal">
                        Search
                    </button>
                    <br />
                    <asp:Label runat="server" ID="rfvDiagnosiscode" ForeColor="Red"></asp:Label>
                    <%-- <asp:RequiredFieldValidator runat="server" ID="rfvDiagnosiscode" SetFocusOnError="true" ForeColor="Red"
                        ValidationGroup="vgCarePlan" ControlToValidate="txtDiagnosisCode" Text="*Diagnosis Code is required" Display="Dynamic" />--%>
                </div>
            </div>

            <div class="col-sm-2" id="divICDVersion" runat="server">
                <span class="ohio-field-label"><span runat="server" style="color: red">*</span><b>ICD Version</b></span>
                <div>
                    <asp:DropDownList ID="ddlICDVer" CssClass="formField" EnableViewState="true" runat="server"  onChange="return DiagnosisCodetextChange(this)" AppendDataBoundItems="True" Style="height: 30px; width: 100px; min-width: 100px;"></asp:DropDownList>
                    <button type="button" runat="server" class="btn btn-link" id="btnICDVersionSearch" onclick="getbuttondetail(this)" data-toggle="modal" data-target="#myDiagModal">
                        Search
                    </button>
                    <asp:Label runat="server" ID="reqICDVersion" ForeColor="Red"></asp:Label>
                    <%-- <asp:RequiredFieldValidator runat="server" ID="reqICDVersion" SetFocusOnError="true"
                        ValidationGroup="vgCarePlan" ControlToValidate="ddlICDVer" ErrorMessage="*ICD version is required" Text="*" Display="Dynamic" InitialValue="0" />--%>
                </div>
            </div>

            <div id="divPresetOnAdmission" runat="server" class="col-sm-2">
                <span class="ohio-field-label"><span runat="server" style="color: red">*</span><b>Present on Admission</b></span>
                <div>
                    <asp:DropDownList ID="ddlPresentAdmission" CssClass="formField" EnableViewState="true" runat="server" AppendDataBoundItems="True" Style="height: 30px; width: 150px; min-width: 150px;"></asp:DropDownList>
                 <asp:Label ID="lblPresentOnAdmissionEror" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>

            <div id="divDiagnosisDescription" class="col-sm-2 text-centre" runat="server">
                <span class="ohio-field-label"><b>Diagnosis Description </b></span>
                <span>
                    <asp:Label ID="lblDiagnosisDescription" runat="server" Style="height: 30px; width: 200px; font-size: 14px;" />
                    <asp:HiddenField ID="lblDiagDesc" runat="server" />

                </span>
            </div>

            <div id="divBtndiagnosis" runat="server" class="col-sm-2">
                <span class="ohio-field-label">&nbsp;</span>
                <div class="col-sm-12" style="margin-left: 30px">
                    <%--<button id="btnloading" runat="server" style="display: none;" cssclass="buttonBox StepButton buttonBoxFocus"><i class="fa fa-spinner fa-spin"></i>Loading</button>--%>

                    <asp:Button ID="btnDiagnosis" Text="ADD" OnClientClick="return addDiagnosisCode()" runat="server" CssClass="btn btn-primary" Font-Bold="True" ValidationGroup="vgCarePlan" Width="70px" />
                   <br /> <asp:Button ID="btnEdit" Style="visibility: hidden" Text="Update" OnClientClick="return EditDiagnosisCode()" runat="server" CssClass="btn btn-primary" Font-Bold="True" ValidationGroup="vgCarePlan" Width="70px" />
                    <asp:Button ID="btnCancel" Style="visibility: hidden" Text="Cancel" OnClientClick="return CancelDiagFields();" runat="server" CssClass="btn btn-danger" Font-Bold="True" ValidationGroup="vgCarePlan" Width="70px" />
                </div>

            </div>
        </div>
    </div>
    <div id="ICDErrorMsg" runat="server" class="form-group row" style="padding-left: 75px">
        <asp:Label ID="lblIcdVersionError" runat="server" ForeColor="Red"></asp:Label>
    </div>
   <%-- <div class="form-group row" style="padding-left: 250px">
       
    </div>--%>
</asp:Panel>
