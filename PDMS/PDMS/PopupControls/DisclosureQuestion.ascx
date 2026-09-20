<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_DisclosureQuestion" codebehind="DisclosureQuestion.ascx.cs" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>
<%@ register src="~/PopupControls/OwnerTransaction.ascx" tagprefix="uc" tagname="OwnerTransaction" %>

<%@ register src="~/PopupControls/OwnerRelationships.ascx" tagprefix="uc" tagname="OwnerRelationships" %>

<%@ register src="~/PopupControls/OwnerOtherInfo.ascx" tagprefix="uc" tagname="OwnerOtherInfo" %>
<%@ register src="~/PopupControls/OwnerSubcontractor.ascx" tagprefix="uc" tagname="OwnerSubcontractor" %>
<%@ register src="~/PopupControls/OwnerConviction.ascx" tagprefix="uc" tagname="OwnerConviction" %>
<%@ register src="~/PopupControls/OwnerDebarred.ascx" tagprefix="uc" tagname="OwnerDebarred" %>
<%@ register src="~/PopupControls/OwnerExcluded.ascx" tagprefix="uc" tagname="OwnerExcluded" %>
<%@ register src="~/PopupControls/OwnerTerminated.ascx" tagprefix="uc" tagname="OwnerTerminated" %>
<%@ register src="~/PopupControls/OwnerOriginal.ascx" tagprefix="uc" tagname="OwnerOriginal" %>
<%@ register src="~/PopupControls/OwnerSubcontractorOwner.ascx" tagprefix="uc" tagname="OwnerSubcontractorOwner" %>
<%@ register src="~/PopupControls/OwnerSupplier.ascx" tagprefix="uc" tagname="OwnerSupplier" %>
<%@ register src="~/PopupControls/OwnerHistory.ascx" tagprefix="uc" tagname="OwnerHistory" %>
<%@ register src="~/PopupControls/OwnerConvictionOnBehalf.ascx" tagprefix="uc" tagname="OwnerConvictionOnBehalf" %>
<%@ register src="~/PopupControls/OwnerPenalty.ascx" tagprefix="uc" tagname="OwnerPenalty" %>
<%@ register src="~/PopupControls/OwnerResidency.ascx" tagprefix="uc" tagname="OwnerResidency" %>

<script src="<%# Page.ResolveClientUrl("~/Scripts/jquery-2.0.3.js") %>" type="text/javascript"></script>

<%--<asp:ScriptManager runat="server" EnablePageMethods="true"></asp:ScriptManager>--%>



<%--<uc: OwnerResidency runat="server" id="OwnerResidency" />--%>

<script type="text/javascript">
    function QuestionAnswerIsYes(rblId) {
        if (document.getElementById(rblId) != null) {
            var oElem = document.getElementById(rblId);
            var radio = oElem.getElementsByTagName("input");
            return radio[0].checked;
        }
        return false;
    }

    function QuestionTogglePanel(rblId, pnlId, changedId) {
        if (QuestionAnswerIsYes(rblId)) document.getElementById(pnlId).style.display = "block";
        else document.getElementById(pnlId).style.display = "none";
        var oElem = document.getElementById(changedId);
        oElem.value = "CHANGED";
    }
</script>
<%--
<script>
    //D01
    if (!('TBL_BODY_VWOPT1_ID' in window)) {
        // declare once
        window.TBL_BODY_VWOPT1_ID = 'vwOpt1_tblBody'; // use window to avoid block-scope const/let collisions
    } else {
        // optionally reassign
        window.TBL_BODY_VWOPT1_ID = window.TBL_BODY_VWOPT1_ID || 'vwOpt1_tblBody';
    }

    //D02
    if (!('TBL_BODY_VWOPT2_ID' in window)) {
        // declare once
        window.TBL_BODY_VWOPT2_ID = 'vwOpt2_tblBody'; // use window to avoid block-scope const/let collisions
    } else {
        // optionally reassign
        window.TBL_BODY_VWOPT2_ID = window.TBL_BODY_VWOPT2_ID || 'vwOpt2_tblBody';
    }

    //D03
    if (!('TBL_BODY_VWOPT3_ID' in window)) {
        // declare once
        window.TBL_BODY_VWOPT3_ID = 'vwOpt3_tblBody'; // use window to avoid block-scope const/let collisions
    } else {
        // optionally reassign
        window.TBL_BODY_VWOPT3_ID = window.TBL_BODY_VWOPT3_ID || 'vwOpt3_tblBody';
    }

    //D04
    if (!('TBL_BODY_VWOPT4_ID' in window)) {
        // declare once
        window.TBL_BODY_VWOPT4_ID = 'vwOpt4_tblBody'; // use window to avoid block-scope const/let collisions
    } else {
        // optionally reassign
        window.TBL_BODY_VWOPT4_ID = window.TBL_BODY_VWOPT4_ID || 'vwOpt4_tblBody';
    }

    //D05
    if (!('TBL_BODY_VWOPT5_ID' in window)) {
        // declare once
        window.TBL_BODY_VWOPT5_ID = 'vwOpt5_tblBody'; // use window to avoid block-scope const/let collisions
    } else {
        // optionally reassign
        window.TBL_BODY_VWOPT5_ID = window.TBL_BODY_VWOPT5_ID || 'vwOpt5_tblBody';
    }

    //D06
    if (!('TBL_BODY_VWOPT6_ID' in window)) {
        // declare once
        window.TBL_BODY_VWOPT6_ID = 'vwOpt6_tblBody'; // use window to avoid block-scope const/let collisions
    } else {
        // optionally reassign
        window.TBL_BODY_VWOPT6_ID = window.TBL_BODY_VWOPT6_ID || 'vwOpt6_tblBody';
    }

    //D07
    if (!('TBL_BODY_VWOPT7_ID' in window)) {
        // declare once
        window.TBL_BODY_VWOPT7_ID = 'vwOpt7_tblBody'; // use window to avoid block-scope const/let collisions
    } else {
        // optionally reassign
        window.TBL_BODY_VWOPT7_ID = window.TBL_BODY_VWOPT7_ID || 'vwOpt7_tblBody';
    }

    //D08
    if (!('TBL_BODY_VWOPT8_ID' in window)) {
        // declare once
        window.TBL_BODY_VWOPT8_ID = 'vwOpt8_tblBody'; // use window to avoid block-scope const/let collisions
    } else {
        // optionally reassign
        window.TBL_BODY_VWOPT8_ID = window.TBL_BODY_VWOPT8_ID || 'vwOpt8_tblBody';
    }

    //D09
    if (!('TBL_BODY_VWOPT9_ID' in window)) {
        // declare once
        window.TBL_BODY_VWOPT9_ID = 'vwOpt9_tblBody'; // use window to avoid block-scope const/let collisions
    } else {
        // optionally reassign
        window.TBL_BODY_VWOPT9ID = window.TBL_BODY_VWOPT9_ID || 'vwOpt9_tblBody';
    }

    //D10
    if (!('TBL_BODY_VWOPT10_ID' in window)) {
        // declare once
        window.TBL_BODY_VWOPT10_ID = 'vwOpt10_tblBody'; // use window to avoid block-scope const/let collisions
    } else {
        // optionally reassign
        window.TBL_BODY_VWOPT10ID = window.TBL_BODY_VWOPT10_ID || 'vwOpt10_tblBody';
    }

    //D11
    if (!('TBL_BODY_VWOPT11_ID' in window)) {
        // declare once
        window.TBL_BODY_VWOPT11_ID = 'vwOpt11_tblBody'; // use window to avoid block-scope const/let collisions
    } else {
        // optionally reassign
        window.TBL_BODY_VWOPT11_ID = window.TBL_BODY_VWOPT11_ID || 'vwOpt11_tblBody';
    }

    //D12
    if (!('TBL_BODY_VWOPT12_ID' in window)) {
        // declare once
        window.TBL_BODY_VWOPT12_ID = 'vwOpt12_tblBody'; // use window to avoid block-scope const/let collisions
    } else {
        // optionally reassign
        window.TBL_BODY_VWOPT12ID = window.TBL_BODY_VWOPT12_ID || 'vwOpt12_tblBody';
    }

    //D13
    if (!('TBL_BODY_VWOPT13_ID' in window)) {
        // declare once
        window.TBL_BODY_VWOPT13_ID = 'vwOpt13_tblBody'; // use window to avoid block-scope const/let collisions
    } else {
        // optionally reassign
        window.TBL_BODY_VWOPT13_ID = window.TBL_BODY_VWOPT13_ID || 'vwOpt13_tblBody';
    }

    //D14
    if (!('TBL_BODY_VWOPT14_ID' in window)) {
        // declare once
        window.TBL_BODY_VWOPT14_ID = 'vwOpt14_tblBody'; // use window to avoid block-scope const/let collisions
    } else {
        // optionally reassign
        window.TBL_BODY_VWOPT14_ID = window.TBL_BODY_VWOPT14_ID || 'vwOpt14_tblBody';
    }

    //D15
    if (!('TBL_BODY_VWOPT15_ID' in window)) {
        // declare once
        window.TBL_BODY_VWOPT15_ID = 'vwOpt15_tblBody'; // use window to avoid block-scope const/let collisions
    } else {
        // optionally reassign
        window.TBL_BODY_VWOPT15_ID = window.TBL_BODY_VWOPT15_ID || 'vwOpt15_tblBody';
    }

    // same idea for other constants
    window.API_BASE ??= '/api/disclosures'; // only assigns if undefined or null
</script>--%>


<%--<script>

    function $(id) { return document.getElementById(id); }

    function toDtoFromForm(QuetionTypeID) {
        if (QuetionTypeID == 'D01') {
            return {
                QUESTION_TYPE_ID: "D01",
                INCIDENT_DATE: $('vwOpt1_txtDate').value.trim(),  // "MM/dd/yyyy"
                STATE_CODE: $('vwOpt1_ddlState').value.trim(),
                PROGRAM_AFFECTED: $('vwOpt1_txtProgram').value.trim(),
                AGENCY_TAKING_ACTION: $('vwOpt1_txtAgency').value.trim(),
                ACTION_TAKEN: $('vwOpt1_txtAction').value.trim(),
                EXPLANATION_DETAILS: $('vwOpt1_txtExplanation').value.trim(),
                COURT_ID: '',
                COUNTRY_ID: '',
                COUNTY_ID: '',
                CHARGE: '',
                AGREEMENT: '',
                CASE_NUMBER: '',
                IS_SELECTED: '1'
            };
        }
        else if (QuetionTypeID == 'D02') {
            return {
                QUESTION_TYPE_ID: "D02",
                INCIDENT_DATE: $('vwOpt2_txtDate').value.trim(),  // "MM/dd/yyyy"
                STATE_CODE: $('vwOpt2_ddlState').value.trim(),
                PROGRAM_AFFECTED: $('vwOpt2_txtProgram').value.trim(),
                AGENCY_TAKING_ACTION: $('vwOpt2_txtAgency').value.trim(),
                ACTION_TAKEN: $('vwOpt2_txtAction').value.trim(),
                EXPLANATION_DETAILS: $('vwOpt2_txtExplanation').value.trim(),
                COURT_ID: '',
                COUNTRY_ID: '',
                COUNTY_ID: '',
                CHARGE: '',
                AGREEMENT: '',
                CASE_NUMBER: '',
                IS_SELECTED: '1'
            };
        }
        else if (QuetionTypeID == 'D03') {
            return {
                QUESTION_TYPE_ID: "D03",
                INCIDENT_DATE: $('vwOpt3_txtDate').value.trim(),  // "MM/dd/yyyy"
                STATE_CODE: $('vwOpt3_ddlState').value.trim(),
                PROGRAM_AFFECTED: $('vwOpt3_txtProgram').value.trim(),
                AGENCY_TAKING_ACTION: $('vwOpt3_txtAgency').value.trim(),
                ACTION_TAKEN: $('vwOpt3_txtAction').value.trim(),
                EXPLANATION_DETAILS: $('vwOpt3_txtExplanation').value.trim(),
                COURT_ID: '',
                COUNTRY_ID: '',
                COUNTY_ID: '',
                CHARGE: '',
                AGREEMENT: '',
                CASE_NUMBER: '',
                IS_SELECTED: '1'
            };
        }
        else if (QuetionTypeID == 'D04') {
            return {
                QUESTION_TYPE_ID: "D04",
                INCIDENT_DATE: $('vwOpt4_txtDate').value.trim(),  // "MM/dd/yyyy"
                STATE_CODE: $('vwOpt4_ddlState').value.trim(),
                PROGRAM_AFFECTED: $('vwOpt4_txtProgram').value.trim(),
                AGENCY_TAKING_ACTION: $('vwOpt4_txtAgency').value.trim(),
                ACTION_TAKEN: $('vwOpt4_txtAction').value.trim(),
                EXPLANATION_DETAILS: $('vwOpt4_txtExplanation').value.trim(),
                COURT_ID: '',
                COUNTRY_ID: '',
                COUNTY_ID: '',
                CHARGE: '',
                AGREEMENT: '',
                CASE_NUMBER: '',
                IS_SELECTED: '1'
            };
        }
        else if (QuetionTypeID == 'D05') {
            return {
                QUESTION_TYPE_ID: "D05",
                INCIDENT_DATE: $('vwOpt5_txtDate').value.trim(),  // "MM/dd/yyyy"
                STATE_CODE: $('vwOpt5_ddlState').value.trim(),
                PROGRAM_AFFECTED: $('vwOpt5_txtProgram').value.trim(),
                AGENCY_TAKING_ACTION: $('vwOpt5_txtAgency').value.trim(),
                ACTION_TAKEN: $('vwOpt5_txtAction').value.trim(),
                EXPLANATION_DETAILS: $('vwOpt5_txtExplanation').value.trim(),
                COURT_ID: '',
                COUNTRY_ID: '',
                COUNTY_ID: '',
                CHARGE: '',
                AGREEMENT: '',
                CASE_NUMBER: '',
                IS_SELECTED: '1'
            };
        }
        else if (QuetionTypeID == 'D06') {
            return {
                QUESTION_TYPE_ID: "D06",
                INCIDENT_DATE: $('vwOpt6_txtDate').value.trim(),  // "MM/dd/yyyy"
                STATE_CODE: $('vwOpt6_ddlState').value.trim(),
                PROGRAM_AFFECTED: $('vwOpt6_txtProgram').value.trim(),
                EXPLANATION_DETAILS: $('vwOpt6_txtExplanation').value.trim(),
                AGENCY_TAKING_ACTION: '',
                ACTION_TAKEN: '',
                COURT_ID: '',
                COUNTRY_ID: '',
                COUNTY_ID: '',
                CHARGE: '',
                AGREEMENT: '',
                CASE_NUMBER: '',
                IS_SELECTED: '1'
            };
        }
        else if (QuetionTypeID == 'D07') {
            return {
                QUESTION_TYPE_ID: "D07",
                INCIDENT_DATE: $('vwOpt7_txtDate').value.trim(),  // "MM/dd/yyyy"
                STATE_CODE: $('vwOpt7_ddlState').value.trim(),
                PROGRAM_AFFECTED: $('vwOpt7_txtProgram').value.trim(),
                AGREEMENT: $('vwOpt7_txtAgreement').value.trim(),
                EXPLANATION_DETAILS: $('vwOpt7_txtExplanation').value.trim(),
                AGENCY_TAKING_ACTION:'',
                ACTION_TAKEN: '',
                COURT_ID: '',
                COUNTRY_ID: '',
                COUNTY_ID: '',
                CHARGE: '',                
                CASE_NUMBER: '',
                IS_SELECTED: '1'
            };
        }
        else if (QuetionTypeID == 'D08') {
            return {
                QUESTION_TYPE_ID: "D08",
                INCIDENT_DATE: $('vwOpt8_txtDate').value.trim(),  // "MM/dd/yyyy"
                STATE_CODE: $('vwOpt8_ddlState').value.trim(),
                PROGRAM_AFFECTED: $('vwOpt8_txtProgram').value.trim(),
                EXPLANATION_DETAILS: $('vwOpt8_txtExplanation').value.trim(),
                AGENCY_TAKING_ACTION: '',
                ACTION_TAKEN: '',
                COURT_ID: '',
                COUNTRY_ID: '',
                COUNTY_ID: '',
                CHARGE: '',
                CASE_NUMBER: '',
                AGREEMENT: '',
                IS_SELECTED: '1'
            };
        }
        else if (QuetionTypeID == 'D09') {
            return {
                QUESTION_TYPE_ID: "D09",
                INCIDENT_DATE: $('vwOpt9_txtDate').value.trim(),  // "MM/dd/yyyy"
                STATE_CODE: $('vwOpt9_ddlState').value.trim(),
                COUNTY_ID: $('vwOpt9_ddlCounty').value.trim(),
                COURT_ID: $('vwOpt9_txtCourt').value.trim(),
                CASE_NUMBER: $('vwOpt9_txtCauseNumber').value.trim(),
                CHARGE: $('vwOpt9_txtCharge').value.trim(),
                EXPLANATION_DETAILS: $('vwOpt9_txtExplanation').value.trim(),
                AGENCY_TAKING_ACTION: '',
                PROGRAM_AFFECTED:'',
                ACTION_TAKEN: '',
                COUNTRY_ID: '',
                AGREEMENT: '',
                IS_SELECTED: '1'
            };
        }
        else if (QuetionTypeID == 'D10') {
            return {
                QUESTION_TYPE_ID: "D10",
                INCIDENT_DATE: $('vwOpt10_txtDate').value.trim(),  // "MM/dd/yyyy"
                STATE_CODE: $('vwOpt10_ddlState').value.trim(),
                COUNTY_ID: $('vwOpt10_ddlCounty').value.trim(),
                COURT_ID: $('vwOpt10_txtCourt').value.trim(),
                CASE_NUMBER: $('vwOpt10_txtCauseNumber').value.trim(),
                CHARGE: $('vwOpt10_txtCharge').value.trim(),
                EXPLANATION_DETAILS: $('vwOpt10_txtExplanation').value.trim(),
                AGENCY_TAKING_ACTION: '',
                PROGRAM_AFFECTED: '',
                ACTION_TAKEN: '',
                COUNTRY_ID: '',               
                AGREEMENT: '',
                IS_SELECTED: '1'
            };
        }
        else if (QuetionTypeID == 'D11') {
            return {
                QUESTION_TYPE_ID: "D11",
                INCIDENT_DATE: $('vwOpt11_txtDate').value.trim(),  // "MM/dd/yyyy"
                STATE_CODE: $('vwOpt11_ddlState').value.trim(),
                COUNTY_ID: $('vwOpt11_ddlCounty').value.trim(),
                COURT_ID: $('vwOpt11_txtCourt').value.trim(),
                CASE_NUMBER: $('vwOpt11_txtCauseNumber').value.trim(),
                CHARGE: $('vwOpt11_txtCharge').value.trim(),
                EXPLANATION_DETAILS: $('vwOpt11_txtExplanation').value.trim(),
                AGENCY_TAKING_ACTION: '',
                PROGRAM_AFFECTED: '',
                ACTION_TAKEN: '',                
                COUNTRY_ID: '',
                AGREEMENT: '',
                IS_SELECTED: '1'
            };
        }
        else if (QuetionTypeID == 'D12') {
            return {
                QUESTION_TYPE_ID: "D12",
                INCIDENT_DATE: $('vwOpt12_txtDate').value.trim(),  // "MM/dd/yyyy"
                STATE_CODE: $('vwOpt12_ddlState').value.trim(),
                COUNTY_ID: $('vwOpt12_ddlCounty').value.trim(),
                COURT_ID: $('vwOpt12_txtCourt').value.trim(),
                CASE_NUMBER: $('vwOpt12_txtCauseNumber').value.trim(),
                CHARGE: $('vwOpt12_txtCharge').value.trim(),
                EXPLANATION_DETAILS: $('vwOpt12_txtExplanation').value.trim(),
                AGENCY_TAKING_ACTION: '',
                PROGRAM_AFFECTED: '',
                ACTION_TAKEN: '',               
                COUNTRY_ID: '',
                AGREEMENT: '',
                IS_SELECTED: '1'
            };
        }
        else if (QuetionTypeID == 'D13') {
            return {
                QUESTION_TYPE_ID: "D13",
                INCIDENT_DATE: $('vwOpt13_txtDate').value.trim(),  // "MM/dd/yyyy"
                STATE_CODE: $('vwOpt13_ddlState').value.trim(),
                COUNTY_ID: $('vwOpt13_ddlCounty').value.trim(),
                COURT_ID: $('vwOpt13_txtCourt').value.trim(),
                CASE_NUMBER: $('vwOpt13_txtCauseNumber').value.trim(),
                CHARGE: $('vwOpt13_txtCharge').value.trim(),
                EXPLANATION_DETAILS: $('vwOpt13_txtExplanation').value.trim(),
                AGENCY_TAKING_ACTION: '',
                PROGRAM_AFFECTED: '',
                ACTION_TAKEN: '',               
                COUNTRY_ID: '',
                AGREEMENT: '',
                IS_SELECTED: '1'
            };
        }
        else if (QuetionTypeID == 'D14') {
            return {
                QUESTION_TYPE_ID: "D14",
                INCIDENT_DATE: $('vwOpt14_txtDate').value.trim(),  // "MM/dd/yyyy"
                STATE_CODE: $('vwOpt14_ddlState').value.trim(),
                PROGRAM_AFFECTED: $('vwOpt14_txtProgram').value.trim(),
                AGENCY_TAKING_ACTION: $('vwOpt14_txtAgency').value.trim(),
                ACTION_TAKEN: $('vwOpt14_txtAction').value.trim(),
                EXPLANATION_DETAILS: $('vwOpt14_txtExplanation').value.trim(),
                COURT_ID: '',
                COUNTRY_ID: '',
                COUNTY_ID: '',
                CHARGE: '',
                CASE_NUMBER: '',
                AGREEMENT: '',
                IS_SELECTED: '1'
            };
        }
        else if (QuetionTypeID == 'D15') {
            return {
                QUESTION_TYPE_ID: "D14",
                INCIDENT_DATE: '',  // "MM/dd/yyyy"
                STATE_CODE: '',
                PROGRAM_AFFECTED: '',
                AGENCY_TAKING_ACTION: '',
                ACTION_TAKEN: '',
                EXPLANATION_DETAILS: $('vwOpt15_txtExplanation').value.trim(),
                COURT_ID: '',
                COUNTRY_ID: '',
                COUNTY_ID: '',
                CHARGE: '',
                CASE_NUMBER: '',
                AGREEMENT: '',
                IS_SELECTED: '1'
            };
        }
        else {
            return {
                QUESTION_TYPE_ID: "D01",
                INCIDENT_DATE: $('vwOpt1_txtDate').value.trim(),  // "MM/dd/yyyy"
                STATE_CODE: $('vwOpt1_ddlState').value.trim(),
                PROGRAM_AFFECTED: $('vwOpt1_txtProgram').value.trim(),
                AGENCY_TAKING_ACTION: $('vwOpt1_txtAgency').value.trim(),
                ACTION_TAKEN: $('vwOpt1_txtAction').value.trim(),
                EXPLANATION_DETAILS: $('vwOpt1_txtExplanation').value.trim(),
                COURT_ID: '',
                COUNTRY_ID: '',
                COUNTY_ID: '',
                CHARGE: '',
                CASE_NUMBER: '',               
                AGREEMENT: '',
                IS_SELECTED: '1'
            };
        }
    }

    function validate(dto, key) {
        const errs = [];

        //if (!dto.INCIDENT_DATE) errs.push('Date is required.');
        //if (!dto.STATE_CODE) errs.push('State is required.');
        //if (!dto.PROGRAM_AFFECTED) errs.push('Program is required.');
        //if (!dto.AGENCY_TAKING_ACTION) errs.push('Agency is required.');
        //if (!dto.ACTION_TAKEN) errs.push('Action is required.');
        //if (!dto.EXPLANATION_DETAILS) errs.push('Explanation is required.');

        return errs;
    }

    //function callPageMethod(method, payloadObj) {
    //    var hiddenRegIDObj = $("[id*=hdnRegId]");
    //    if (hiddenRegIDObj != undefined) {
    //        var hiddernRegVal = hiddenRegIDObj.value;
    //    }
    //    var APIToken = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiMmUyZmQ4MzAtOTk0ZC00YTE4LWEzMDktNGMxMmU4ODE3MTgzIiwiZXhwIjoxNzY3MzI0NjYzLCJpc3MiOiJsb2NhbGhvc3QiLCJhdWQiOiJsb2NhbGhvc3QifQ.iL2rK4VTZimvjbDEwAb7ovzybzknhnGxUy4WVwYOYvE' //$("[id*=hdnAccessToken]").val();
    //    var regId = '472364'
    //    try {
    //        var url = webApiEnrollment + method;            
    //        if (method == 'AddDisclosure' || method == 'UpdateDisclosure') {
    //            jQuery.ajax({
    //                type: "POST",
    //                url: url,//method,
    //                async: true,
    //                headers: {
    //                    "Access-Control-Allow-Origin": "*",
    //                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
    //                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
    //                    "Authorization": "Bearer " + APIToken,
    //                },
    //                contentType: "application/json; charset=utf-8",
    //                dataType: "json",
    //                data: JSON.stringify(payloadObj),
    //                success: function (result) {
    //                    loadDisclosures();
    //                },
    //                error: function (jqXHR, textStatus, errorThrown) {
    //                    console.error(errorThrown);
    //                }
    //            });
    //        } else if (method == 'GetDisclosures') {
    //            //TODO: Loop through Question Types and bind all the grids
    //            url = url + "?regId=" + payloadObj;
    //            jQuery.ajax({
    //                type: "GET",
    //                url: url,//method,
    //                async: true,
    //                headers: {
    //                    "Access-Control-Allow-Origin": "*",
    //                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
    //                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
    //                    "Authorization": "Bearer " + APIToken,
    //                },
    //                contentType: "application/json; charset=utf-8",
    //                dataType: "json",
    //                success: function (result) {
    //                    // Validate result before proceeding 
    //                    if (!result || !Array.isArray(result) || result.length === 0)
    //                    {
    //                        //console.log("No valid disclosures returned");
    //                        return;
    //                    }

    //                    bindDisclosures(result);
    //                },
    //                error: function (jqXHR, textStatus, errorThrown) {                        
    //                    console.error('LoadDisclosure: '+ errorThrown);
    //                }
    //            });

    //        } else if (method == 'DeleteDisclosure') {                
    //            //TODO: Loop through Question Types and bind all the grids
    //            url = url + "?regDisclosureId=" + payloadObj;
    //            jQuery.ajax({
    //                type: "POST",
    //                url: url,//method,
    //                async: true,
    //                headers: {
    //                    "Access-Control-Allow-Origin": "*",
    //                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
    //                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
    //                    "Authorization": "Bearer " + APIToken,
    //                },
    //                contentType: "application/json; charset=utf-8",
    //                dataType: "json",
    //                success: function (result) {                        
    //                    loadDisclosures();
    //                },
    //                error: function (jqXHR, textStatus, errorThrown) {                        
    //                    console.error('DeleteDisclosure: ' + errorThrown);
    //                }
    //            });
    //        }

    //    } catch (err) {            
    //        console.error('method:' + method + ' Error:' + err);
    //    }
    //}

    function escapeHtml(s) {
        return (s ?? '').replace(/[&<>"']/g, c => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', '\'': '&#39;' }[c]));
    }

    function formatDate(dateString) {
        const d = new Date(dateString);
        const year = d.getFullYear();
        const month = String(d.getMonth() + 1).padStart(2, '0');
        const day = String(d.getDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    }

    function escapeAttr(s) { return (s ?? '').replace(/"/g, '&quot;'); }

    //function bindDisclosures(rows) {
    //    const tbody = $(TBL_BODY_VWOPT1_ID);
    //    tbody.innerHTML = '';
    //    rows.forEach(r => {
    //        const tr = document.createElement('tr');
    //        tr.dataset.key = r.QUESTION_TYPE_ID;
    //        tr.innerHTML = `
    //  <td>${formatDate(r.INCIDENT_DATE)}</td>
    //  <td>${escapeHtml(r.STATE_CODE)}</td>
    //  <td>${escapeHtml(r.PROGRAM_AFFECTED)}</td>
    //  <td>${escapeHtml(r.AGENCY_TAKING_ACTION)}</td>
    //  <td>${escapeHtml(r.ACTION_TAKEN)}</td>
    //  <td>${escapeHtml(r.EXPLANATION_DETAILS)}</td>
    //  <td>
    //    <button type="button" class="btn btn-link" title="Edit" onclick="editRow('${r.REG_DISCLOSURES_ID}')">
    //     <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Images/edit.png" AlternateText="Edit" />

    //    </button>
    //    <button type="button" class="btn btn-link" title="Delete" onclick="deleteRow('${r.REG_DISCLOSURES_ID}')">
    //     <asp:Image ID="imgCancel" runat="server" ImageUrl="~/Images/cancel.png" AlternateText="Cancel" />

    //    </button>
    //  </td>
    //`;
    //        tbody.appendChild(tr);
    //    });
    //}
    //function bindDisclosures(rows) {
    //    const tbody1 = $(TBL_BODY_VWOPT1_ID);
    //    const tbody2 = $(TBL_BODY_VWOPT2_ID);
    //    const tbody3 = $(TBL_BODY_VWOPT3_ID);
    //    const tbody4 = $(TBL_BODY_VWOPT4_ID);
    //    const tbody5 = $(TBL_BODY_VWOPT5_ID);
    //    const tbody6 = $(TBL_BODY_VWOPT6_ID);
    //    const tbody7 = $(TBL_BODY_VWOPT7_ID);
    //    const tbody8 = $(TBL_BODY_VWOPT8_ID);
    //    const tbody9 = $(TBL_BODY_VWOPT9_ID);
    //    const tbody10 = $(TBL_BODY_VWOPT10_ID);
    //    const tbody11 = $(TBL_BODY_VWOPT11_ID);
    //    const tbody12 = $(TBL_BODY_VWOPT12_ID);
    //    const tbody13 = $(TBL_BODY_VWOPT13_ID);
    //    const tbody14 = $(TBL_BODY_VWOPT14_ID);


    //    if (tbody1 != undefined) {
    //        tbody1.innerHTML = '';
    //    }
    //    if (tbody2 != undefined) {
    //        tbody2.innerHTML = '';
    //    }
    //    if (tbody3 != undefined) {
    //        tbody3.innerHTML = '';
    //    }
    //    if (tbody4 != undefined) {
    //        tbody4.innerHTML = '';
    //    }
    //    if (tbody5 != undefined) {
    //        tbody5.innerHTML = '';
    //    }
    //    if (tbody6 != undefined) {
    //        tbody6.innerHTML = '';
    //    }
    //    if (tbody7 != undefined) {
    //        tbody7.innerHTML = '';
    //    }
    //    if (tbody8 != undefined) {
    //        tbody8.innerHTML = '';
    //    }
    //    if (tbody9 != undefined) {
    //        tbody9.innerHTML = '';
    //    }
    //    if (tbody10 != undefined) {
    //        tbody10.innerHTML = '';
    //    }
    //    if (tbody10 != undefined) {
    //        tbody11.innerHTML = '';
    //    }
    //    if (tbody12 != undefined) {
    //        tbody12.innerHTML = '';
    //    }
    //    if (tbody13 != undefined) {
    //        tbody13.innerHTML = '';
    //    }
    //    if (tbody14 != undefined) {
    //        tbody14.innerHTML = '';
    //    }

    //    rows.forEach(r => {
    //        const tr = document.createElement('tr');
    //        tr.dataset.key = r.QUESTION_TYPE_ID;

            
    //        // Route row using switch
    //        switch (r.QUESTION_TYPE_ID) {
    //            case 'D01':
    //                tr.innerHTML = `
    //               <td>${formatDate(r.INCIDENT_DATE)}</td>
    //               <td>${escapeHtml(r.STATE_CODE)}</td>
    //               <td>${escapeHtml(r.PROGRAM_AFFECTED)}</td>
    //               <td>${escapeHtml(r.AGENCY_TAKING_ACTION)}</td>
    //               <td>${escapeHtml(r.ACTION_TAKEN)}</td>
    //               <td>${escapeHtml(r.EXPLANATION_DETAILS)}</td>
    //               <td>
    //                 <button type="button" class="btn btn-link" title="Edit" onclick="editRow('${r.REG_DISCLOSURES_ID}')">
    //                   <img src="/Images/edit.png" alt="Edit">
    //                 </button>
    //                 <button type="button" class="btn btn-link" title="Delete" onclick="deleteRow('${r.REG_DISCLOSURES_ID}')">
    //                   <img src="/Images/cancel.png" alt="Delete">
    //                 </button>
    //               </td>
    //             `;
    //                tbody1 && tbody1.appendChild(tr);
    //                break;
    //            case 'D02':
    //                tr.innerHTML = `
    //               <td>${formatDate(r.INCIDENT_DATE)}</td>
    //               <td>${escapeHtml(r.STATE_CODE)}</td>
    //               <td>${escapeHtml(r.PROGRAM_AFFECTED)}</td>
    //               <td>${escapeHtml(r.AGENCY_TAKING_ACTION)}</td>
    //               <td>${escapeHtml(r.ACTION_TAKEN)}</td>
    //               <td>${escapeHtml(r.EXPLANATION_DETAILS)}</td>
    //               <td>
    //                 <button type="button" class="btn btn-link" title="Edit" onclick="editRow('${r.REG_DISCLOSURES_ID}')">
    //                   <img src="/Images/edit.png" alt="Edit">
    //                 </button>
    //                 <button type="button" class="btn btn-link" title="Delete" onclick="deleteRow('${r.REG_DISCLOSURES_ID}')">
    //                   <img src="/Images/cancel.png" alt="Delete">
    //                 </button>
    //               </td>
    //             `;
    //                tbody2 && tbody2.appendChild(tr);
    //                break;
    //            case 'D03':
    //                tr.innerHTML = `
    //               <td>${formatDate(r.INCIDENT_DATE)}</td>
    //               <td>${escapeHtml(r.STATE_CODE)}</td>
    //               <td>${escapeHtml(r.PROGRAM_AFFECTED)}</td>
    //               <td>${escapeHtml(r.AGENCY_TAKING_ACTION)}</td>
    //               <td>${escapeHtml(r.ACTION_TAKEN)}</td>
    //               <td>${escapeHtml(r.EXPLANATION_DETAILS)}</td>
    //               <td>
    //                 <button type="button" class="btn btn-link" title="Edit" onclick="editRow('${r.REG_DISCLOSURES_ID}')">
    //                   <img src="/Images/edit.png" alt="Edit">
    //                 </button>
    //                 <button type="button" class="btn btn-link" title="Delete" onclick="deleteRow('${r.REG_DISCLOSURES_ID}')">
    //                   <img src="/Images/cancel.png" alt="Delete">
    //                 </button>
    //               </td>
    //             `;
    //                tbody3 && tbody3.appendChild(tr);
    //                break;
    //            case 'D04':
    //                tr.innerHTML = `
    //               <td>${formatDate(r.INCIDENT_DATE)}</td>
    //               <td>${escapeHtml(r.STATE_CODE)}</td>
    //               <td>${escapeHtml(r.PROGRAM_AFFECTED)}</td>
    //               <td>${escapeHtml(r.AGENCY_TAKING_ACTION)}</td>
    //               <td>${escapeHtml(r.ACTION_TAKEN)}</td>
    //               <td>${escapeHtml(r.EXPLANATION_DETAILS)}</td>
    //               <td>
    //                 <button type="button" class="btn btn-link" title="Edit" onclick="editRow('${r.REG_DISCLOSURES_ID}')">
    //                   <img src="/Images/edit.png" alt="Edit">
    //                 </button>
    //                 <button type="button" class="btn btn-link" title="Delete" onclick="deleteRow('${r.REG_DISCLOSURES_ID}')">
    //                   <img src="/Images/cancel.png" alt="Delete">
    //                 </button>
    //               </td>
    //             `;
    //                tbody4 && tbody4.appendChild(tr);
    //                break;
    //            case 'D05':
    //                tr.innerHTML = `
    //               <td>${formatDate(r.INCIDENT_DATE)}</td>
    //               <td>${escapeHtml(r.STATE_CODE)}</td>
    //               <td>${escapeHtml(r.PROGRAM_AFFECTED)}</td>
    //               <td>${escapeHtml(r.AGENCY_TAKING_ACTION)}</td>
    //               <td>${escapeHtml(r.ACTION_TAKEN)}</td>
    //               <td>${escapeHtml(r.EXPLANATION_DETAILS)}</td>
    //               <td>
    //                 <button type="button" class="btn btn-link" title="Edit" onclick="editRow('${r.REG_DISCLOSURES_ID}')">
    //                   <img src="/Images/edit.png" alt="Edit">
    //                 </button>
    //                 <button type="button" class="btn btn-link" title="Delete" onclick="deleteRow('${r.REG_DISCLOSURES_ID}')">
    //                   <img src="/Images/cancel.png" alt="Delete">
    //                 </button>
    //               </td>
    //             `;
    //                tbody5 && tbody5.appendChild(tr);
    //                break;
    //            case 'D06':
    //                tr.innerHTML = `
    //                <td>${formatDate(r.INCIDENT_DATE)}</td>
    //                <td>${escapeHtml(r.STATE_CODE)}</td>
    //                <td>${escapeHtml(r.PROGRAM_AFFECTED)}</td>                   
    //                <td>${escapeHtml(r.EXPLANATION_DETAILS)}</td>
    //                <td>
    //                    <button type="button" class="btn btn-link" title="Edit" onclick="editRow('${r.REG_DISCLOSURES_ID}')">
    //                    <img src="/Images/edit.png" alt="Edit">
    //                    </button>
    //                    <button type="button" class="btn btn-link" title="Delete" onclick="deleteRow('${r.REG_DISCLOSURES_ID}')">
    //                    <img src="/Images/cancel.png" alt="Delete">
    //                    </button>
    //                </td>
    //                `;
    //                tbody6 && tbody6.appendChild(tr);
    //                break;
    //            case 'D07':
    //                tr.innerHTML = `
    //                 <td>${formatDate(r.INCIDENT_DATE)}</td>
    //                 <td>${escapeHtml(r.STATE_CODE)}</td>
    //                 <td>${escapeHtml(r.PROGRAM_AFFECTED)}</td>    
    //                 <td>${escapeHtml(r.AGREEMENT)}</td>    
    //                 <td>${escapeHtml(r.EXPLANATION_DETAILS)}</td>
    //                 <td>
    //                     <button type="button" class="btn btn-link" title="Edit" onclick="editRow('${r.REG_DISCLOSURES_ID}')">
    //                     <img src="/Images/edit.png" alt="Edit">
    //                     </button>
    //                     <button type="button" class="btn btn-link" title="Delete" onclick="deleteRow('${r.REG_DISCLOSURES_ID}')">
    //                     <img src="/Images/cancel.png" alt="Delete">
    //                     </button>
    //                 </td>
    //                 `;
    //                tbody7 && tbody7.appendChild(tr);
    //                break;
    //            case 'D08':
    //                tr.innerHTML = `
    //                <td>${formatDate(r.INCIDENT_DATE)}</td>
    //                <td>${escapeHtml(r.STATE_CODE)}</td>
    //                <td>${escapeHtml(r.PROGRAM_AFFECTED)}</td>                   
    //                <td>${escapeHtml(r.EXPLANATION_DETAILS)}</td>
    //                <td>
    //                    <button type="button" class="btn btn-link" title="Edit" onclick="editRow('${r.REG_DISCLOSURES_ID}')">
    //                    <img src="/Images/edit.png" alt="Edit">
    //                    </button>
    //                    <button type="button" class="btn btn-link" title="Delete" onclick="deleteRow('${r.REG_DISCLOSURES_ID}')">
    //                    <img src="/Images/cancel.png" alt="Delete">
    //                    </button>
    //                </td>
    //                `;
    //                tbody8 && tbody8.appendChild(tr);
    //                break;
    //            case 'D09':                    
    //                tr.innerHTML = `
    //                <td>${formatDate(r.INCIDENT_DATE)}</td>
    //                <td>${escapeHtml(r.STATE_CODE)}</td>
    //                <td>${escapeHtml(r.COUNTY_ID)}</td>                   
    //                <td>${escapeHtml(r.COURT_ID)}</td>
    //                <td>${escapeHtml(r.CASE_NUMBER)}</td>
    //                <td>${escapeHtml(r.CHARGE)}</td>
    //                <td>${escapeHtml(r.EXPLANATION_DETAILS)}</td>
    //                <td>
    //                    <button type="button" class="btn btn-link" title="Edit" onclick="editRow('${r.REG_DISCLOSURES_ID}')">
    //                    <img src="/Images/edit.png" alt="Edit">
    //                    </button>
    //                    <button type="button" class="btn btn-link" title="Delete" onclick="deleteRow('${r.REG_DISCLOSURES_ID}')">
    //                    <img src="/Images/cancel.png" alt="Delete">
    //                    </button>
    //                </td>
    //                `;
    //                tbody9 && tbody9.appendChild(tr);
    //                break;
    //            case 'D10':                    
    //                tr.innerHTML = `
    //                <td>${formatDate(r.INCIDENT_DATE)}</td>
    //                <td>${escapeHtml(r.STATE_CODE)}</td>
    //                <td>${escapeHtml(r.COUNTY_ID)}</td>                   
    //                <td>${escapeHtml(r.COURT_ID)}</td>
    //                <td>${escapeHtml(r.CASE_NUMBER)}</td>
    //                <td>${escapeHtml(r.CHARGE)}</td>
    //                <td>${escapeHtml(r.EXPLANATION_DETAILS)}</td>
    //                <td>
    //                    <button type="button" class="btn btn-link" title="Edit" onclick="editRow('${r.REG_DISCLOSURES_ID}')">
    //                    <img src="/Images/edit.png" alt="Edit">
    //                    </button>
    //                    <button type="button" class="btn btn-link" title="Delete" onclick="deleteRow('${r.REG_DISCLOSURES_ID}')">
    //                    <img src="/Images/cancel.png" alt="Delete">
    //                    </button>
    //                </td>
    //                `;
    //                tbody10 && tbody10.appendChild(tr);
    //                break;
    //            case 'D11':                    
    //                tr.innerHTML = `
    //                <td>${formatDate(r.INCIDENT_DATE)}</td>
    //                <td>${escapeHtml(r.STATE_CODE)}</td>
    //                <td>${escapeHtml(r.COUNTY_ID)}</td>                   
    //                <td>${escapeHtml(r.COURT_ID)}</td>
    //                <td>${escapeHtml(r.CASE_NUMBER)}</td>
    //                <td>${escapeHtml(r.CHARGE)}</td>
    //                <td>${escapeHtml(r.EXPLANATION_DETAILS)}</td>
    //                <td>
    //                    <button type="button" class="btn btn-link" title="Edit" onclick="editRow('${r.REG_DISCLOSURES_ID}')">
    //                    <img src="/Images/edit.png" alt="Edit">
    //                    </button>
    //                    <button type="button" class="btn btn-link" title="Delete" onclick="deleteRow('${r.REG_DISCLOSURES_ID}')">
    //                    <img src="/Images/cancel.png" alt="Delete">
    //                    </button>
    //                </td>
    //                `;
    //                tbody11 && tbody11.appendChild(tr);
    //                break;
    //            case 'D12':                    
    //                tr.innerHTML = `
    //                <td>${formatDate(r.INCIDENT_DATE)}</td>
    //                <td>${escapeHtml(r.STATE_CODE)}</td>
    //                <td>${escapeHtml(r.COUNTY_ID)}</td>                   
    //                <td>${escapeHtml(r.COURT_ID)}</td>
    //                <td>${escapeHtml(r.CASE_NUMBER)}</td>
    //                <td>${escapeHtml(r.CHARGE)}</td>
    //                <td>${escapeHtml(r.EXPLANATION_DETAILS)}</td>
    //                <td>
    //                    <button type="button" class="btn btn-link" title="Edit" onclick="editRow('${r.REG_DISCLOSURES_ID}')">
    //                    <img src="/Images/edit.png" alt="Edit">
    //                    </button>
    //                    <button type="button" class="btn btn-link" title="Delete" onclick="deleteRow('${r.REG_DISCLOSURES_ID}')">
    //                    <img src="/Images/cancel.png" alt="Delete">
    //                    </button>
    //                </td>
    //                `;
    //                tbody12 && tbody12.appendChild(tr);
    //                break;
    //            case 'D13':                    
    //                tr.innerHTML = `
    //                <td>${formatDate(r.INCIDENT_DATE)}</td>
    //                <td>${escapeHtml(r.STATE_CODE)}</td>
    //                <td>${escapeHtml(r.COUNTY_ID)}</td>                   
    //                <td>${escapeHtml(r.COURT_ID)}</td>
    //                <td>${escapeHtml(r.CASE_NUMBER)}</td>
    //                <td>${escapeHtml(r.CHARGE)}</td>
    //                <td>${escapeHtml(r.EXPLANATION_DETAILS)}</td>
    //                <td>
    //                    <button type="button" class="btn btn-link" title="Edit" onclick="editRow('${r.REG_DISCLOSURES_ID}')">
    //                    <img src="/Images/edit.png" alt="Edit">
    //                    </button>
    //                    <button type="button" class="btn btn-link" title="Delete" onclick="deleteRow('${r.REG_DISCLOSURES_ID}')">
    //                    <img src="/Images/cancel.png" alt="Delete">
    //                    </button>
    //                </td>
    //                `;
    //                tbody13 && tbody13.appendChild(tr);
    //                break;
    //            case 'D14':
    //                tr.innerHTML = `
    //               <td>${formatDate(r.INCIDENT_DATE)}</td>
    //               <td>${escapeHtml(r.STATE_CODE)}</td>
    //               <td>${escapeHtml(r.PROGRAM_AFFECTED)}</td>
    //               <td>${escapeHtml(r.AGENCY_TAKING_ACTION)}</td>
    //               <td>${escapeHtml(r.ACTION_TAKEN)}</td>
    //               <td>${escapeHtml(r.EXPLANATION_DETAILS)}</td>
    //               <td>
    //                 <button type="button" class="btn btn-link" title="Edit" onclick="editRow('${r.REG_DISCLOSURES_ID}')">
    //                   <img src="/Images/edit.png" alt="Edit">
    //                 </button>
    //                 <button type="button" class="btn btn-link" title="Delete" onclick="deleteRow('${r.REG_DISCLOSURES_ID}')">
    //                   <img src="/Images/cancel.png" alt="Delete">
    //                 </button>
    //               </td>
    //             `;
    //                tbody14 && tbody14.appendChild(tr);
    //                break;

    //            default:
    //                // If unexpected type, choose where it should go
    //                tbody1.appendChild(tr);
    //                break;
    //        }
    //    });
    //}

    //function loadDisclosures() {
    //    try {
    //        callPageMethod('GetDisclosures', 597862);
    //    } catch (err) {
    //        console.error(err);
    //    }
    //}

    //function addDisclosure(QuetionTypeID, RegQuestionID) {
    //    const dto = toDtoFromForm(QuetionTypeID);
    //    dto.REG_ID = 597862;
    //    dto.REG_DISCLOSURES_ID = 0;
    //    dto.REG_QUESTION_ID = RegQuestionID;
    //    dto.QUESTION_TYPE_ID = QuetionTypeID;

    //    const errs = validate(dto, QuetionTypeID);
    //    if (errs.length) return alert(errs.join('\n'));
    //    try {
    //        callPageMethod('AddDisclosure', dto);
    //        clearForm(QuetionTypeID);
    //        loadDisclosures();
    //    } catch (err) {
    //        console.error(err);
    //    }
    //}

    function clearForm(QuetionTypeID) {
        if (QuetionTypeID = 'D01') {
            ['vwOpt1_txtDate', 'vwOpt1_ddlState', 'vwOpt1_txtProgram', 'vwOpt1_txtAgency', 'vwOpt1_txtAction', 'vwOpt1_txtExplanation']
                .forEach(id => { const el = $(id); if (el) el.value = ''; });
        }

        if (QuetionTypeID = 'D02') {
            ['vwOpt2_txtDate', 'vwOpt2_ddlState', 'vwOpt2_txtProgram', 'vwOpt2_txtAgency', 'vwOpt2_txtAction', 'vwOpt2_txtExplanation']
                .forEach(id => { const el = $(id); if (el) el.value = ''; });
        }

        if (QuetionTypeID = 'D03') {
            ['vwOpt3_txtDate', 'vwOpt3_ddlState', 'vwOpt3_txtProgram', 'vwOpt3_txtAgency', 'vwOpt3_txtAction', 'vwOpt3_txtExplanation']
                .forEach(id => { const el = $(id); if (el) el.value = ''; });
        }

        if (QuetionTypeID = 'D04') {
            ['vwOpt4_txtDate', 'vwOpt4_ddlState', 'vwOpt4_txtProgram', 'vwOpt4_txtAgency', 'vwOpt4_txtAction', 'vwOpt4_txtExplanation']
                .forEach(id => { const el = $(id); if (el) el.value = ''; });
        }

        if (QuetionTypeID = 'D05') {
            ['vwOpt5_txtDate', 'vwOpt5_ddlState', 'vwOpt5_txtProgram', 'vwOpt5_txtAgency', 'vwOpt5_txtAction', 'vwOpt5_txtExplanation']
                .forEach(id => { const el = $(id); if (el) el.value = ''; });
        }

        if (QuetionTypeID = 'D06') {
            ['vwOpt6_txtDate', 'vwOpt6_ddlState', 'vwOpt6_txtProgram', 'vwOpt6_txtExplanation']
                .forEach(id => { const el = $(id); if (el) el.value = ''; });
        }

        if (QuetionTypeID = 'D07') {
            ['vwOpt7_txtDate', 'vwOpt7_ddlState', 'vwOpt7_txtProgram', 'vwOpt7_txtExplanation']
                .forEach(id => { const el = $(id); if (el) el.value = ''; });
        }

        if (QuetionTypeID = 'D08') {
            ['vwOpt8_txtDate', 'vwOpt8_ddlState', 'vwOpt8_txtProgram', 'vwOpt8_txtExplanation']
                .forEach(id => { const el = $(id); if (el) el.value = ''; });
        }

        if (QuetionTypeID = 'D09') {
            ['vwOpt9_txtDate', 'vwOpt9_ddlState', 'vwOpt9_txtExplanation']
                .forEach(id => { const el = $(id); if (el) el.value = ''; });
        }
    }

    // Inline edit
    function editRow(key) {
        const tr = [...$(TBL_BODY_VWOPT1_ID).children].find(r => r.dataset.key == key);
        if (!tr) return;
        const tds = tr.querySelectorAll('td');
        const curr = {
            INCIDENT_DATE: tds[0].innerText.trim(),
            STATE_CODE: tds[1].innerText.trim(),
            PROGRAM_AFFECTED: tds[2].innerText.trim(),
            AGENCY_TAKING_ACTION: tds[3].innerText.trim(),
            ACTION_TAKEN: tds[4].innerText.trim(),
            EXPLANATION_DETAILS: tds[5].innerText.trim()
        };

        tr.innerHTML = `
    <td><input type="text" value="${escapeAttr(curr.INCIDENT_DATE)}" /></td>
    <td>
      <select>
        <option value="">Select State</option>
        <option value="OH" ${curr.STATE_CODE === 'OH' ? 'selected' : ''}>Ohio</option>
        <option value="CA" ${curr.STATE_CODE === 'CA' ? 'selected' : ''}>California</option>
        <option value="TX" ${curr.STATE_CODE === 'TX' ? 'selected' : ''}>Texas</option>
      </select>
    </td>
    <td><input type="text" value="${escapeAttr(curr.PROGRAM_AFFECTED)}" /></td>
    <td><input type="text" value="${escapeAttr(curr.AGENCY_TAKING_ACTION)}" /></td>
    <td><input type="text" value="${escapeAttr(curr.ACTION_TAKEN)}" /></td>
    <td><textarea rows="3">${escapeAttr(curr.EXPLANATION_DETAILS)}</textarea></td>
    <td>
      <button type="button" class="btn btn-sm btn-success">Save</button>
      <button type="button" class="btn btn-sm btn-secondary">Cancel</button>
    </td>
  `;
    }

    //function saveRow(key, btn) {
    //    const tr = btn.closest('tr');
    //    const dto = {
    //        QUESTION_TYPE_ID: key,
    //        INCIDENT_DATE: tr.querySelector('td:nth-child(1) input').value.trim(),
    //        STATE_CODE: tr.querySelector('td:nth-child(2) select').value.trim(),
    //        PROGRAM_AFFECTED: tr.querySelector('td:nth-child(3) input').value.trim(),
    //        AGENCY_TAKING_ACTION: tr.querySelector('td:nth-child(4) input').value.trim(),
    //        ACTION_TAKEN: tr.querySelector('td:nth-child(5) input').value.trim(),
    //        EXPLANATION_DETAILS: tr.querySelector('td:nth-child(6) textarea').value.trim()
    //    };
    //    const errs = validate(dto, key);
    //    if (errs.length) return alert(errs.join('\n'));
    //    try {
    //        const ok = callPageMethod('UpdateDisclosure', { dto });
    //        if (!ok) throw new Error('Update returned false.');
    //        loadDisclosures();
    //    } catch (err) {
    //        console.error(err);
    //    }
    //}

    //function deleteRow(key) {
    //    try {
    //        const ok = callPageMethod('DeleteDisclosure', key);
    //        /*if (!ok) throw new Error('Delete returned false.');*/
    //        loadDisclosures();
    //    } catch (err) {
    //        console.error(err);
    //    }
    //}

    //document.addEventListener('DOMContentLoaded', () => {
    //    loadDisclosures();
    //});
</script>--%>

<div>


    <table style="border: 1px solid gray; padding: 3px; width: 100%">
        <!-- YES/NO radio button list -->
        <tr>
            <td>
                <asp:literal id="lblQuestion" runat="server" mode="PassThrough" />
            </td>
            <td style="min-width: 150px;">
                <asp:radiobuttonlist id="rblYesNo" borderstyle="None" cellpadding="0" cellspacing="0"
                    repeatdirection="Horizontal" runat="server" repeatlayout="Table"  style="display:inline-block; float:right;">
                    <asp:listitem value="1">Yes</asp:listitem>
                    <asp:listitem value="2">No</asp:listitem>
                </asp:radiobuttonlist>
            </td>
        </tr>
        <tr></tr>
        <tr>
            <td>
                <asp:panel id="pnlQuestion" runat="server">
                    <br />
                    <asp:label id="lblInnerMessage" runat="server" />
                    <asp:multiview id="mltQuestion" runat="server" clientidmode="Static">
                        <asp:view id="vwDefaultQuestion" runat="server"></asp:view>

                        <!-- ====================== vwOpt1 ====================== -->
                        <asp:view id="vwOpt1" runat="server">

                            <div class="form-section" style="display: flex; flex-wrap: wrap; gap: 20px;">
                                <!-- DATE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt1_txtDate"><b>DATE (Approximate Date Allowed) *</b></label><br />
                                    <input id="vwOpt1_txtDate" class="form-control" placeholder="MM/DD/YYYY" type="text" />
                                </div>

                                <!-- STATE -->
                                <div style="flex: 1;">
                                    <label for="vwOpt1_ddlState"><b>STATE WHERE THE INCIDENT OCCURRED *</b></label><br />
                                    <select id="vwOpt1_ddlState" class="form-control">
                                        <option value="">Select State</option>
                                        <option value="OH">Ohio</option>
                                        <option value="CA">California</option>
                                        <option value="TX">Texas</option>
                                    </select>
                                </div>

                                <!-- PROGRAM -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt1_txtProgram"><b>PROGRAM AFFECTED *</b></label><br />
                                    <input id="vwOpt1_txtProgram" class="form-control" placeholder="Enter Program" type="text" />
                                </div>

                                <!-- AGENCY -->
                                <div style="flex: 10; min-width: 250px;">
                                    <label for="vwOpt1_txtAgency"><b>AGENCY TAKING THE ACTION *</b></label><br />
                                    <input id="vwOpt1_txtAgency" class="form-control" placeholder="Enter Agency" type="text" />
                                </div>

                                <!-- ACTION -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt1_txtAction"><b>ACTION TAKEN *</b></label><br />
                                    <input id="vwOpt1_txtAction" class="form-control" placeholder="Enter Action taken" type="text" />
                                </div>

                                <!-- Info -->
                                <div style="flex: 1; min-width: 250px;">
                                    <p style="font-size: 12px; color: #333;">
                                        If disclosure is for a prior Exclusion, provide Reinstatement documentation from OIG. Attach to this page.
                                    </p>
                                </div>

                                <!-- EXPLANATION -->
                                <div style="flex: 100%; margin-top: 20px;">
                                    <label for="vwOpt1_txtExplanation"><b>EXPLANATION/DETAILS *</b></label><br />
                                    <p style="font-size: 12px; color: #333;">
                                        Please provide a detailed explanation and attach all relevant documentation. If documentation is not available,
        please explain why and where it can be obtained.
                                    </p>
                                    <textarea id="vwOpt1_txtExplanation" class="form-control" rows="5" style="width: 100%;"></textarea>
                                </div>
                            </div>

                            <!-- Add button: client-side only -->
                            <div class="divHistoryAndAdd" style="margin-top: 10px;">
                                <button id="vwOpt1_btnAdd" type="button" title="Add New" class="btn btn-primary" onclick="addDisclosure('D01',1)">
                                    <%-- <asp:Image ID="imgAdd" runat="server" ImageUrl="~/Images/add.png" AlternateText="Add" />--%>Add New
                                </button>
                            </div>
                            <br />

                            <!-- Client-rendered HTML table -->
                            <%--<table id="vwOpt1_tblDisclosures" class="gridview" style="width: 98%;">
                                <thead class="gridViewHeader">
                                    <tr>
                                        <th>INCIDENT DATE</th>
                                        <th>STATE CODE</th>
                                        <th>PROGRAM AFFECTED</th>
                                        <th>AGENCY TAKING ACTION</th>
                                        <th>ACTION TAKEN</th>
                                        <th>EXPLANATION DETAILS</th>
                                        <th style="width: 110px;">Actions</th>
                                    </tr>
                                </thead>
                                <tbody id="vwOpt1_tblBody">
                                    <!-- rows injected via JS -->
                                </tbody>
                            </table>--%>
                        </asp:view>


                        <!-- ====================== vwOpt2 ====================== -->
                        <asp:view id="vwOpt2" runat="server">
                            <div class="form-section" style="display: flex; flex-wrap: wrap; gap: 20px;">
                                <!-- DATE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt2_txtDate"><b>DATE (Approximate Date Allowed) *</b></label><br />
                                    <input id="vwOpt2_txtDate" class="form-control" placeholder="MM/DD/YYYY" type="text" />
                                </div>

                                <!-- STATE -->
                                <div style="flex: 1;">
                                    <label for="vwOpt2_ddlState"><b>STATE WHERE THE INCIDENT OCCURRED *</b></label><br />
                                    <select id="vwOpt2_ddlState" class="form-control">
                                        <option value="">Select State</option>
                                        <option value="OH">Ohio</option>
                                        <option value="CA">California</option>
                                        <option value="TX">Texas</option>
                                    </select>
                                </div>

                                <!-- PROGRAM -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt2_txtProgram"><b>PROGRAM AFFECTED *</b></label><br />
                                    <input id="vwOpt2_txtProgram" class="form-control" placeholder="Enter Program" type="text" />
                                </div>

                                <!-- AGENCY -->
                                <div style="flex: 10; min-width: 250px;">
                                    <label for="vwOpt2_txtAgency"><b>AGENCY TAKING THE ACTION *</b></label><br />
                                    <input id="vwOpt2_txtAgency" class="form-control" placeholder="Enter Agency" type="text" />
                                </div>

                                <!-- ACTION -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt2_txtAction"><b>ACTION TAKEN *</b></label><br />
                                    <input id="vwOpt2_txtAction" class="form-control" placeholder="Enter Action taken" type="text" />
                                </div>

                                <!-- Info -->
                                <div style="flex: 1; min-width: 250px;">
                                    <p style="font-size: 12px; color: #333;">
                                        If disclosure is for a prior Exclusion, provide Reinstatement documentation from OIG. Attach to this page.
                                    </p>
                                </div>

                                <!-- EXPLANATION -->
                                <div style="flex: 100%; margin-top: 20px;">
                                    <label for="vwOpt2_txtExplanation"><b>EXPLANATION/DETAILS *</b></label><br />
                                    <p style="font-size: 12px; color: #333;">
                                        Please provide a detailed explanation and attach all relevant documentation. If documentation is not available,
        please explain why and where it can be obtained.
                                    </p>
                                    <textarea id="vwOpt2_txtExplanation" class="form-control" rows="5" style="width: 100%;"></textarea>
                                </div>
                            </div>

                            <div class="divHistoryAndAdd" style="margin-top: 10px;">
                                <%-- <asp:imagebutton id="vwOpt2_btnAddRelationships" runat="server" imageurl="~/Images/add.png"
                                    oncommand="btnAdd_Click" commandname="vwOpt2" />--%>
                                <button id="vwOpt2_btnAdd" type="button" title="Add New" class="btn btn-primary" onclick="addDisclosure('D02',1)">Add New </button>
                            </div>
                            <br />

                            <!-- Client-rendered HTML table -->
                            <%--<table id="vwOpt2_tblDisclosures" class="gridview" style="width: 98%;">
                                <thead class="gridViewHeader">
                                    <tr>
                                        <th>INCIDENT DATE</th>
                                        <th>STATE CODE</th>
                                        <th>PROGRAM AFFECTED</th>
                                        <th>AGENCY TAKING ACTION</th>
                                        <th>ACTION TAKEN</th>
                                        <th>EXPLANATION DETAILS</th>
                                        <th style="width: 110px;">Actions</th>
                                    </tr>
                                </thead>
                                <tbody id="vwOpt2_tblBody">
                                    <!-- rows injected via JS -->
                                </tbody>
                            </table>--%>
                        </asp:view>

                        <!-- ====================== vwOpt3 ====================== -->
                        <asp:view id="vwOpt3" runat="server">
                            <div class="form-section" style="display: flex; flex-wrap: wrap; gap: 20px;">

                                <!-- DATE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt3_txtDate"><b>DATE (Approximate Date Allowed) *</b></label><br />
                                    <input id="vwOpt3_txtDate" class="form-control" placeholder="MM/DD/YYYY" type="text" />
                                </div>

                                <!-- STATE -->
                                <div style="flex: 1;">
                                    <label for="vwOpt3_ddlState"><b>STATE WHERE THE INCIDENT OCCURRED *</b></label><br />
                                    <select id="vwOpt3_ddlState" class="form-control">
                                        <option value="">Select State</option>
                                        <option value="OH">Ohio</option>
                                        <option value="CA">California</option>
                                        <option value="TX">Texas</option>
                                    </select>
                                </div>

                                <!-- PROGRAM -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt3_txtProgram"><b>PROGRAM AFFECTED *</b></label><br />
                                    <input id="vwOpt3_txtProgram" class="form-control" placeholder="Enter Program" type="text" />
                                </div>

                                <!-- AGENCY -->
                                <div style="flex: 10; min-width: 250px;">
                                    <label for="vwOpt3_txtAgency"><b>AGENCY TAKING THE ACTION *</b></label><br />
                                    <input id="vwOpt3_txtAgency" class="form-control" placeholder="Enter Agency" type="text" />
                                </div>

                                <!-- ACTION -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt3_txtAction"><b>ACTION TAKEN *</b></label><br />
                                    <input id="vwOpt3_txtAction" class="form-control" placeholder="Enter Action taken" type="text" />
                                </div>

                                <!-- EXPLANATION -->
                                <div style="flex: 100%; margin-top: 20px;">
                                    <label for="vwOpt3_txtExplanation"><b>EXPLANATION/DETAILS *</b></label><br />
                                    <p style="font-size: 12px; color: #333;">
                                        Please provide a detailed explanation and attach all relevant documentation. If documentation is not available,
please explain why and where it can be obtained.
                                    </p>
                                    <textarea id="vwOpt3_txtExplanation" class="form-control" rows="5" style="width: 100%;"></textarea>
                                </div>
                            </div>
                            <div class="divHistoryAndAdd" style="margin-top: 10px;">
                                <%-- <asp:imagebutton id="vwOpt2_btnAddRelationships" runat="server" imageurl="~/Images/add.png"
         oncommand="btnAdd_Click" commandname="vwOpt2" />--%>
                                <button id="vwOpt3_btnAdd" type="button" title="Add New" class="btn btn-primary" onclick="addDisclosure('D03',1)">Add New </button>
                            </div>
                            <br />

                            <!-- Client-rendered HTML table -->
                            <%--<table id="vwOpt3_tblDisclosures" class="gridview" style="width: 98%;">
                                <thead class="gridViewHeader">
                                    <tr>
                                        <th>INCIDENT DATE</th>
                                        <th>STATE CODE</th>
                                        <th>PROGRAM AFFECTED</th>
                                        <th>AGENCY TAKING ACTION</th>
                                        <th>ACTION TAKEN</th>
                                        <th>EXPLANATION DETAILS</th>
                                        <th style="width: 110px;">Actions</th>
                                    </tr>
                                </thead>
                                <tbody id="vwOpt3_tblBody">
                                    <!-- rows injected via JS -->
                                </tbody>
                            </table>--%>
                        </asp:view>

                        <!-- ====================== vwOpt4 ====================== -->
                        <asp:view id="vwOpt4" runat="server">
                            <div class="form-section" style="display: flex; flex-wrap: wrap; gap: 20px;">
                                <!-- DATE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt4_txtDate"><b>DATE (Approximate Date Allowed) *</b></label><br />
                                    <input id="vwOpt4_txtDate" class="form-control" placeholder="MM/DD/YYYY" type="text" />
                                </div>

                                <!-- STATE -->
                                <div style="flex: 1;">
                                    <label for="vwOpt4_ddlState"><b>STATE WHERE THE INCIDENT OCCURRED *</b></label><br />
                                    <select id="vwOpt4_ddlState" class="form-control">
                                        <option value="">Select State</option>
                                        <option value="OH">Ohio</option>
                                        <option value="CA">California</option>
                                        <option value="TX">Texas</option>
                                    </select>
                                </div>

                                <!-- PROGRAM -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt4_txtProgram"><b>PROGRAM AFFECTED *</b></label><br />
                                    <input id="vwOpt4_txtProgram" class="form-control" placeholder="Enter Program" type="text" />
                                </div>

                                <!-- AGENCY -->
                                <div style="flex: 10; min-width: 250px;">
                                    <label for="vwOpt4_txtAgency"><b>AGENCY TAKING THE ACTION *</b></label><br />
                                    <input id="vwOpt4_txtAgency" class="form-control" placeholder="Enter Agency" type="text" />
                                </div>

                                <!-- ACTION -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt4_txtAction"><b>ACTION TAKEN *</b></label><br />
                                    <input id="vwOpt4_txtAction" class="form-control" placeholder="Enter Action taken" type="text" />
                                </div>

                                <!-- Info -->
                                <div style="flex: 1; min-width: 250px;">
                                    <p style="font-size: 12px; color: #333;">
                                        If disclosure is for a prior Exclusion, provide Reinstatement documentation from OIG. Attach to this page.
                                    </p>
                                </div>

                                <!-- EXPLANATION -->
                                <div style="flex: 100%; margin-top: 20px;">
                                    <label for="vwOpt4_txtExplanation"><b>EXPLANATION/DETAILS *</b></label><br />
                                    <p style="font-size: 12px; color: #333;">
                                        Please provide a detailed explanation and attach all relevant documentation. If documentation is not available,
please explain why and where it can be obtained.
                                    </p>
                                    <textarea id="vwOpt4_txtExplanation" class="form-control" rows="5" style="width: 100%;"></textarea>
                                </div>
                            </div>
                            <div class="divHistoryAndAdd" style="margin-top: 10px;">
                                <button id="vwOpt4_btnAdd" type="button" title="Add New" class="btn btn-primary" onclick="addDisclosure('D04',1)">Add New </button>
                            </div>
                            <br />

                            <!-- Client-rendered HTML table -->
                            <%--<table id="vwOpt4_tblDisclosures" class="gridview" style="width: 98%;">
                                <thead class="gridViewHeader">
                                    <tr>
                                        <th>INCIDENT DATE</th>
                                        <th>STATE CODE</th>
                                        <th>PROGRAM AFFECTED</th>
                                        <th>AGENCY TAKING ACTION</th>
                                        <th>ACTION TAKEN</th>
                                        <th>EXPLANATION DETAILS</th>
                                        <th style="width: 110px;">Actions</th>
                                    </tr>
                                </thead>
                                <tbody id="vwOpt4_tblBody">
                                    <!-- rows injected via JS -->
                                </tbody>
                            </table>--%>
                        </asp:view>

                        <!-- ====================== vwOpt5 ====================== -->
                        <asp:view id="vwOpt5" runat="server">
                            <div class="form-section" style="display: flex; flex-wrap: wrap; gap: 20px;">
                                <!-- DATE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt5_txtDate"><b>DATE (Approximate Date Allowed) *</b></label><br />
                                    <input id="vwOpt5_txtDate" class="form-control" placeholder="MM/DD/YYYY" type="text" />
                                </div>

                                <!-- STATE -->
                                <div style="flex: 1;">
                                    <label for="vwOpt5_ddlState"><b>STATE WHERE THE INCIDENT OCCURRED *</b></label><br />
                                    <select id="vwOpt5_ddlState" class="form-control">
                                        <option value="">Select State</option>
                                        <option value="OH">Ohio</option>
                                        <option value="CA">California</option>
                                        <option value="TX">Texas</option>
                                    </select>
                                </div>

                                <!-- PROGRAM -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt5_txtProgram"><b>PROGRAM AFFECTED *</b></label><br />
                                    <input id="vwOpt5_txtProgram" class="form-control" placeholder="Enter Program" type="text" />
                                </div>

                                <!-- AGENCY -->
                                <div style="flex: 10; min-width: 250px;">
                                    <label for="vwOpt5_txtAgency"><b>AGENCY TAKING THE ACTION *</b></label><br />
                                    <input id="vwOpt5_txtAgency" class="form-control" placeholder="Enter Agency" type="text" />
                                </div>

                                <!-- ACTION -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt5_txtAction"><b>ACTION TAKEN *</b></label><br />
                                    <input id="vwOpt5_txtAction" class="form-control" placeholder="Enter Action taken" type="text" />
                                </div>

                                <!-- Info -->
                                <div style="flex: 1; min-width: 250px;">
                                    <p style="font-size: 12px; color: #333;">
                                        If disclosure is for a prior Exclusion, provide Reinstatement documentation from OIG. Attach to this page.
                                    </p>
                                </div>

                                <!-- EXPLANATION -->
                                <div style="flex: 100%; margin-top: 20px;">
                                    <label for="vwOpt5_txtExplanation"><b>EXPLANATION/DETAILS *</b></label><br />
                                    <p style="font-size: 12px; color: #333;">
                                        Please provide a detailed explanation and attach all relevant documentation. If documentation is not available,
please explain why and where it can be obtained.
                                    </p>
                                    <textarea id="vwOpt5_txtExplanation" class="form-control" rows="5" style="width: 100%;"></textarea>
                                </div>
                            </div>

                            <div class="divHistoryAndAdd" style="margin-top: 10px;">
                                <%-- <asp:imagebutton id="vwOpt2_btnAddRelationships" runat="server" imageurl="~/Images/add.png"
                            oncommand="btnAdd_Click" commandname="vwOpt2" />--%>
                                <button id="vwOpt5_btnAdd" type="button" title="Add New" class="btn btn-primary" onclick="addDisclosure('D05',1)">Add New </button>
                            </div>
                            <br />

                            <!-- Client-rendered HTML table -->
                            <%--<table id="vwOpt5_tblDisclosures" class="gridview" style="width: 98%;">
                                <thead class="gridViewHeader">
                                    <tr>
                                        <th>INCIDENT DATE</th>
                                        <th>STATE CODE</th>
                                        <th>PROGRAM AFFECTED</th>
                                        <th>AGENCY TAKING ACTION</th>
                                        <th>ACTION TAKEN</th>
                                        <th>EXPLANATION DETAILS</th>
                                        <th style="width: 110px;">Actions</th>
                                    </tr>
                                </thead>
                                <tbody id="vwOpt5_tblBody">
                                    <!-- rows injected via JS -->
                                </tbody>
                            </table>--%>
                        </asp:view>

                        <!-- ====================== vwOpt6 ====================== -->
                        <asp:view id="vwOpt6" runat="server">
                            <div class="form-section" style="display: flex; flex-wrap: wrap; gap: 20px;">
                                <!-- DATE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt6_txtDate"><b>DATE (Approximate Date Allowed) *</b></label><br />
                                    <input id="vwOpt6_txtDate" class="form-control" placeholder="MM/DD/YYYY" type="text" />
                                </div>

                                <!-- STATE -->
                                <div style="flex: 1;">
                                    <label for="vwOpt6_ddlState"><b>STATE WHERE THE INCIDENT OCCURRED *</b></label><br />
                                    <select id="vwOpt6_ddlState" class="form-control">
                                        <option value="">Select State</option>
                                        <option value="OH">Ohio</option>
                                        <option value="CA">California</option>
                                        <option value="TX">Texas</option>
                                    </select>
                                </div>

                                <!-- PROGRAM -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt6_txtProgram"><b>PROGRAM AFFECTED *</b></label><br />
                                    <input id="vwOpt6_txtProgram" class="form-control" placeholder="Enter Program" type="text" />
                                </div>

                                <!-- EXPLANATION -->
                                <div style="flex: 100%; margin-top: 20px;">
                                    <label for="vwOpt6_txtExplanation"><b>EXPLANATION/DETAILS *</b></label><br />
                                    <p style="font-size: 12px; color: #333;">
                                        Please provide a detailed explanation and attach all relevant documentation. If documentation is not available,
please explain why and where it can be obtained.
                                    </p>
                                    <textarea id="vwOpt6_txtExplanation" class="form-control" rows="5" style="width: 100%;"></textarea>
                                </div>
                            </div>
                            <div class="divHistoryAndAdd" style="margin-top: 10px;">
                                <%-- <asp:imagebutton id="vwOpt2_btnAddRelationships" runat="server" imageurl="~/Images/add.png"
 oncommand="btnAdd_Click" commandname="vwOpt2" />--%>
                                <button id="vwOpt6_btnAdd" type="button" title="Add New" class="btn btn-primary" onclick="addDisclosure('D06',1)">Add New </button>
                            </div>
                            <br />

                            <!-- Client-rendered HTML table -->
                            <%--<table id="vwOpt6_tblDisclosures" class="gridview" style="width: 98%;">
                                <thead class="gridViewHeader">
                                    <tr>
                                        <th>INCIDENT DATE</th>
                                        <th>STATE CODE</th>
                                        <th>PROGRAM AFFECTED</th>
                                        <th>EXPLANATION DETAILS</th>
                                        <th style="width: 110px;">Actions</th>
                                    </tr>
                                </thead>
                                <tbody id="vwOpt6_tblBody">
                                    <!-- rows injected via JS -->
                                </tbody>
                            </table>--%>
                        </asp:view>

                        <!-- ====================== vwOpt7 ====================== -->
                        <asp:view id="vwOpt7" runat="server">
                            <div class="form-section" style="display: flex; flex-wrap: wrap; gap: 20px;">
                                <!-- DATE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt7_txtDate"><b>DATE (Approximate Date Allowed) *</b></label><br />
                                    <input id="vwOpt7_txtDate" class="form-control" placeholder="MM/DD/YYYY" type="text" />
                                </div>

                                <!-- STATE -->
                                <div style="flex: 1;">
                                    <label for="vwOpt7_ddlState"><b>STATE WHERE THE INCIDENT OCCURRED *</b></label><br />
                                    <select id="vwOpt7_ddlState" class="form-control">
                                        <option value="">Select State</option>
                                        <option value="OH">Ohio</option>
                                        <option value="CA">California</option>
                                        <option value="TX">Texas</option>
                                    </select>
                                </div>

                                <!-- PROGRAM -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt7_txtProgram"><b>PROGRAM AFFECTED *</b></label><br />
                                    <input id="vwOpt7_txtProgram" class="form-control" placeholder="Enter Program" type="text" />
                                </div>

                                <!-- AGREEMENT -->
                                <div style="flex: 10; min-width: 250px;">
                                    <label for="vwOpt7_txtAgreement"><b>AGREEMENT *</b></label><br />
                                    <input id="vwOpt7_txtAgreement" class="form-control" placeholder="Enter Agreement" type="text" />
                                </div>

                                <!-- EXPLANATION -->
                                <div style="flex: 100%; margin-top: 20px;">
                                    <label for="vwOpt7_txtExplanation"><b>EXPLANATION/DETAILS *</b></label><br />
                                    <p style="font-size: 12px; color: #333;">
                                        Please provide a detailed explanation and attach all relevant documentation. If documentation is not available,
please explain why and where it can be obtained.
                                    </p>
                                    <textarea id="vwOpt7_txtExplanation" class="form-control" rows="5" style="width: 100%;"></textarea>
                                </div>
                            </div>
                            <div class="divHistoryAndAdd" style="margin-top: 10px;">
                                <button id="vwOpt7_btnAdd" type="button" title="Add New" class="btn btn-primary" onclick="addDisclosure('D07',1)">Add New </button>
                            </div>
                            <br />

                            <!-- Client-rendered HTML table -->
                            <%--<table id="vwOpt7_tblDisclosures" class="gridview" style="width: 98%;">
                                <thead class="gridViewHeader">
                                    <tr>
                                        <th>INCIDENT DATE</th>
                                        <th>STATE CODE</th>
                                        <th>PROGRAM AFFECTED</th>
                                        <th>AGREEMENT</th>
                                        <th>EXPLANATION DETAILS</th>
                                        <th style="width: 110px;">Actions</th>
                                    </tr>
                                </thead>
                                <tbody id="vwOpt7_tblBody">
                                    <!-- rows injected via JS -->
                                </tbody>
                            </table>--%>
                        </asp:view>

                        <!-- ====================== vwOpt8 ====================== -->
                        <asp:view id="vwOpt8" runat="server">
                            <div class="form-section" style="display: flex; flex-wrap: wrap; gap: 20px;">
                                <!-- DATE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt8_txtDate"><b>DATE (Approximate Date Allowed) *</b></label><br />
                                    <input id="vwOpt8_txtDate" class="form-control" placeholder="MM/DD/YYYY" type="text" />
                                </div>

                                <!-- STATE -->
                                <div style="flex: 1;">
                                    <label for="vwOpt8_ddlState"><b>STATE WHERE THE INCIDENT OCCURRED *</b></label><br />
                                    <select id="vwOpt8_ddlState" class="form-control">
                                        <option value="">Select State</option>
                                        <option value="OH">Ohio</option>
                                        <option value="CA">California</option>
                                        <option value="TX">Texas</option>
                                    </select>
                                </div>

                                <!-- PROGRAM -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt8_txtProgram"><b>PROGRAM AFFECTED *</b></label><br />
                                    <input id="vwOpt8_txtProgram" class="form-control" placeholder="Enter Program" type="text" />
                                </div>

                                <!-- EXPLANATION -->
                                <div style="flex: 100%; margin-top: 20px;">
                                    <label for="vwOpt8_txtExplanation"><b>EXPLANATION/DETAILS *</b></label><br />
                                    <p style="font-size: 12px; color: #333;">
                                        Please provide a detailed explanation and attach all relevant documentation. If documentation is not available,
please explain why and where it can be obtained.
                                    </p>
                                    <textarea id="vwOpt8_txtExplanation" class="form-control" rows="5" style="width: 100%;"></textarea>
                                </div>
                            </div>
                            <div class="divHistoryAndAdd" style="margin-top: 10px;">
                                <button id="vwOpt8_btnAdd" type="button" title="Add New" class="btn btn-primary" onclick="addDisclosure('D08',1)">Add New </button>
                            </div>
                            <br />

                            <!-- Client-rendered HTML table -->
                            <%--<table id="vwOpt8_tblDisclosures" class="gridview" style="width: 98%;">
                                <thead class="gridViewHeader">
                                    <tr>
                                        <th>INCIDENT DATE</th>
                                        <th>STATE CODE</th>
                                        <th>PROGRAM AFFECTED</th>
                                        <th>EXPLANATION DETAILS</th>
                                        <th style="width: 110px;">Actions</th>
                                    </tr>
                                </thead>
                                 <tbody id="vwOpt8_tblBody">
                                    <!-- rows injected via JS -->
                                </tbody>
                            </table>--%>
                        </asp:view>

                        <!-- ====================== vwOpt9 ====================== -->
                        <asp:view id="vwOpt9" runat="server">
                            <div class="form-section" style="display: flex; flex-wrap: wrap; gap: 20px;">
                                <!-- DATE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt9_txtDate"><b>DATE (Approximate Date Allowed) *</b></label><br />
                                    <input id="vwOpt9_txtDate" class="form-control" placeholder="MM/DD/YYYY" type="text" />
                                </div>

                                <!-- STATE -->
                                <div style="flex: 1;">
                                    <label for="vwOpt9_ddlState"><b>STATE WHERE THE INCIDENT OCCURRED *</b></label><br />
                                    <select id="vwOpt9_ddlState" class="form-control">
                                        <option value="">Select State</option>
                                        <option value="OH">Ohio</option>
                                        <option value="CA">California</option>
                                        <option value="TX">Texas</option>
                                    </select>
                                </div>

                                <!-- COUNTY -->
                                <div style="flex: 1;">
                                    <label for="vwOpt9_ddlCounty"><b>COUNTY *</b></label><br />
                                    <select id="vwOpt9_ddlCounty" class="form-control">
                                        <option value="">Select County</option>
                                        <option value="1">Cuyahoga</option>
                                        <option value="2">Franklin</option>
                                        <option value="3">Hamilton</option>
                                    </select>
                                </div>

                                <!-- COURT -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt9_txtCourt"><b>COURT *</b></label><br />
                                    <input id="vwOpt9_txtCourt" class="form-control" placeholder="Enter Court" type="text" />
                                </div>

                                <!-- CASE NUMBER -->
                                <div style="flex: 10; min-width: 250px;">
                                    <label for="vwOpt9_txtCauseNumber"><b>CAUSE/CASE NUMBER *</b></label><br />
                                    <input id="vwOpt9_txtCauseNumber" class="form-control" placeholder="Enter Cause/Case Number" type="text" />
                                </div>

                                <!-- CHARGE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt9_txtCharge"><b>CHARGE *</b></label><br />
                                    <input id="vwOpt9_txtCharge" class="form-control" placeholder="Enter Charge" type="text" />
                                </div>

                                <!-- EXPLANATION -->
                                <div style="flex: 100%; margin-top: 20px;">
                                    <label for="vwOpt9_txtExplanation"><b>EXPLANATION/DETAILS *</b></label><br />
                                    <p style="font-size: 12px; color: #333;">
                                        Please provide a detailed explanation and attach all relevant documentation. If documentation is not available,
please explain why and where it can be obtained.
                                    </p>
                                    <textarea id="vwOpt9_txtExplanation" class="form-control" rows="5" style="width: 100%;"></textarea>
                                </div>
                            </div>
                            <div class="divHistoryAndAdd" style="margin-top: 10px;">
                                <button id="vwOpt9_btnAdd" type="button" title="Add New" class="btn btn-primary" onclick="addDisclosure('D09',1)">Add New </button>
                            </div>
                            <br />

                            <!-- Client-rendered HTML table -->
                            <%--<table id="vwOpt9_tblDisclosures" class="gridview" style="width: 98%;">
                                <thead class="gridViewHeader">
                                    <tr>
                                        <th>INCIDENT DATE</th>
                                        <th>STATE CODE</th>
                                        <th>COUNTY</th>
                                        <th>COURT</th>
                                        <th>CASE NUMBER</th>
                                        <th>CHARGE</th>
                                        <th>EXPLANATION DETAILS</th>
                                        <th style="width: 110px;">Actions</th>
                                    </tr>
                                </thead>
                                <tbody id="vwOpt9_tblBody">
                                    <!-- rows injected via JS -->
                                </tbody>
                            </table>--%>
                        </asp:view>

                        <!-- ====================== vwOpt10 ====================== -->
                        <asp:view id="vwOpt10" runat="server">
                            <div class="form-section" style="display: flex; flex-wrap: wrap; gap: 20px;">
                                <!-- DATE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt10_txtDate"><b>DATE (Approximate Date Allowed) *</b></label><br />
                                    <input id="vwOpt10_txtDate" class="form-control" placeholder="MM/DD/YYYY" type="text" />
                                </div>

                                <!-- STATE -->
                                <div style="flex: 1;">
                                    <label for="vwOpt10_ddlState"><b>STATE WHERE THE INCIDENT OCCURRED *</b></label><br />
                                    <select id="vwOpt10_ddlState" class="form-control">
                                        <option value="">Select State</option>
                                        <option value="OH">Ohio</option>
                                        <option value="CA">California</option>
                                        <option value="TX">Texas</option>
                                    </select>
                                </div>

                                <!-- COUNTY -->
                                <div style="flex: 1;">
                                    <label for="vwOpt10_ddlCounty"><b>COUNTY *</b></label><br />
                                    <select id="vwOpt10_ddlCounty" class="form-control">
                                        <option value="">Select County</option>
                                        <option value="1">Cuyahoga</option>
                                        <option value="2">Franklin</option>
                                        <option value="3">Hamilton</option>
                                    </select>
                                </div>

                                <!-- COURT -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt10_txtCourt"><b>COURT *</b></label><br />
                                    <input id="vwOpt10_txtCourt" class="form-control" placeholder="Enter Court" type="text" />
                                </div>

                                <!-- CASE NUMBER -->
                                <div style="flex: 10; min-width: 250px;">
                                    <label for="vwOpt10_txtCauseNumber"><b>CAUSE/CASE NUMBER *</b></label><br />
                                    <input id="vwOpt10_txtCauseNumber" class="form-control" placeholder="Enter Cause/Case Number" type="text" />
                                </div>

                                <!-- CHARGE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt10_txtCharge"><b>CHARGE *</b></label><br />
                                    <input id="vwOpt10_txtCharge" class="form-control" placeholder="Enter Charge" type="text" />
                                </div>

                                <!-- EXPLANATION -->
                                <div style="flex: 100%; margin-top: 20px;">
                                    <label for="vwOpt10_txtExplanation"><b>EXPLANATION/DETAILS *</b></label><br />
                                    <p style="font-size: 12px; color: #333;">
                                        Please provide a detailed explanation and attach all relevant documentation. If documentation is not available,
please explain why and where it can be obtained.
                                    </p>
                                    <textarea id="vwOpt10_txtExplanation" class="form-control" rows="5" style="width: 100%;"></textarea>
                                </div>
                            </div>
                            <div class="divHistoryAndAdd" style="margin-top: 10px;">
                                <button id="vwOpt10_btnAdd" type="button" title="Add New" class="btn btn-primary" onclick="addDisclosure('D10',1)">Add New </button>
                            </div>
                            <br />

                            <!-- Client-rendered HTML table -->
                            <%--<table id="vwOpt10_tblDisclosures" class="gridview" style="width: 98%;">
                                <thead class="gridViewHeader">
                                    <tr>
                                        <th>INCIDENT DATE</th>
                                        <th>STATE CODE</th>
                                        <th>COUNTY</th>
                                        <th>COURT</th>
                                        <th>CASE NUMBER</th>
                                        <th>CHARGE</th>
                                        <th>EXPLANATION DETAILS</th>
                                        <th style="width: 110px;">Actions</th>
                                    </tr>
                                </thead>
                                <tbody id="vwOpt10_tblBody">
                                    <!-- rows injected via JS -->
                                </tbody>
                            </table>--%>
                        </asp:view>

                        <!-- ====================== vwOpt11 ====================== -->
                        <asp:view id="vwOpt11" runat="server">
                            <div class="form-section" style="display: flex; flex-wrap: wrap; gap: 20px;">
                                <!-- DATE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt11_txtDate"><b>DATE (Approximate Date Allowed) *</b></label><br />
                                    <input id="vwOpt11_txtDate" class="form-control" placeholder="MM/DD/YYYY" type="text" />
                                </div>

                                <!-- STATE -->
                                <div style="flex: 1;">
                                    <label for="vwOpt11_ddlState"><b>STATE WHERE THE INCIDENT OCCURRED *</b></label><br />
                                    <select id="vwOpt11_ddlState" class="form-control">
                                        <option value="">Select State</option>
                                        <option value="OH">Ohio</option>
                                        <option value="CA">California</option>
                                        <option value="TX">Texas</option>
                                    </select>
                                </div>

                                <!-- COUNTY -->
                                <div style="flex: 1;">
                                    <label for="vwOpt11_ddlCounty"><b>COUNTY *</b></label><br />
                                    <select id="vwOpt11_ddlCounty" class="form-control">
                                        <option value="">Select County</option>
                                        <option value="1">Cuyahoga</option>
                                        <option value="2">Franklin</option>
                                        <option value="3">Hamilton</option>
                                    </select>
                                </div>

                                <!-- COURT -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt11_txtCourt"><b>COURT *</b></label><br />
                                    <input id="vwOpt11_txtCourt" class="form-control" placeholder="Enter Court" type="text" />
                                </div>

                                <!-- CASE NUMBER -->
                                <div style="flex: 10; min-width: 250px;">
                                    <label for="vwOpt11_txtCauseNumber"><b>CAUSE/CASE NUMBER *</b></label><br />
                                    <input id="vwOpt11_txtCauseNumber" class="form-control" placeholder="Enter Cause/Case Number" type="text" />
                                </div>

                                <!-- CHARGE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt11_txtCharge"><b>CHARGE *</b></label><br />
                                    <input id="vwOpt11_txtCharge" class="form-control" placeholder="Enter Charge" type="text" />
                                </div>

                                <!-- EXPLANATION -->
                                <div style="flex: 100%; margin-top: 20px;">
                                    <label for="vwOpt11_txtExplanation"><b>EXPLANATION/DETAILS *</b></label><br />
                                    <p style="font-size: 12px; color: #333;">
                                        Please provide a detailed explanation and attach all relevant documentation. If documentation is not available,
please explain why and where it can be obtained.
                                    </p>
                                    <textarea id="vwOpt11_txtExplanation" class="form-control" rows="5" style="width: 100%;"></textarea>
                                </div>
                            </div>
                            <div class="divHistoryAndAdd" style="margin-top: 10px;">
                                <button id="vwOpt11_btnAdd" type="button" title="Add New" class="btn btn-primary" onclick="addDisclosure('D11',1)">Add New </button>
                            </div>
                            <br />

                            <!-- Client-rendered HTML table -->
                            <%--<table id="vwOpt11_tblDisclosures" class="gridview" style="width: 98%;">
                                <thead class="gridViewHeader">
                                    <tr>
                                        <th>INCIDENT DATE</th>
                                        <th>STATE CODE</th>
                                        <th>COUNTY</th>
                                        <th>COURT</th>
                                        <th>CASE NUMBER</th>
                                        <th>CHARGE</th>
                                        <th>EXPLANATION DETAILS</th>
                                        <th style="width: 110px;">Actions</th>
                                    </tr>
                                </thead>
                                <tbody id="vwOpt11_tblBody">
                                    <!-- rows injected via JS -->
                                </tbody>
                            </table>--%>
                        </asp:view>

                        <!-- ====================== vwOpt12 ====================== -->
                        <asp:view id="vwOpt12" runat="server">
                            <div class="form-section" style="display: flex; flex-wrap: wrap; gap: 20px;">
                                <!-- DATE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt12_txtDate"><b>DATE (Approximate Date Allowed) *</b></label><br />
                                    <input id="vwOpt12_txtDate" class="form-control" placeholder="MM/DD/YYYY" type="text" />
                                </div>

                                <!-- STATE -->
                                <div style="flex: 1;">
                                    <label for="vwOpt12_ddlState"><b>STATE WHERE THE INCIDENT OCCURRED *</b></label><br />
                                    <select id="vwOpt12_ddlState" class="form-control">
                                        <option value="">Select State</option>
                                        <option value="OH">Ohio</option>
                                        <option value="CA">California</option>
                                        <option value="TX">Texas</option>
                                    </select>
                                </div>

                                <!-- COUNTY -->
                                <div style="flex: 1;">
                                    <label for="vwOpt12_ddlCounty"><b>COUNTY *</b></label><br />
                                    <select id="vwOpt12_ddlCounty" class="form-control">
                                        <option value="">Select County</option>
                                        <option value="1">Cuyahoga</option>
                                        <option value="2">Franklin</option>
                                        <option value="3">Hamilton</option>
                                    </select>
                                </div>

                                <!-- COURT -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt12_txtCourt"><b>COURT *</b></label><br />
                                    <input id="vwOpt12_txtCourt" class="form-control" placeholder="Enter Court" type="text" />
                                </div>

                                <!-- CASE NUMBER -->
                                <div style="flex: 10; min-width: 250px;">
                                    <label for="vwOpt12_txtCauseNumber"><b>CAUSE/CASE NUMBER *</b></label><br />
                                    <input id="vwOpt12_txtCauseNumber" class="form-control" placeholder="Enter Cause/Case Number" type="text" />
                                </div>

                                <!-- CHARGE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt12_txtCharge"><b>CHARGE *</b></label><br />
                                    <input id="vwOpt12_txtCharge" class="form-control" placeholder="Enter Charge" type="text" />
                                </div>

                                <!-- EXPLANATION -->
                                <div style="flex: 100%; margin-top: 20px;">
                                    <label for="vwOpt12_txtExplanation"><b>EXPLANATION/DETAILS *</b></label><br />
                                    <p style="font-size: 12px; color: #333;">
                                        Please provide a detailed explanation and attach all relevant documentation. If documentation is not available,
please explain why and where it can be obtained.
                                    </p>
                                    <textarea id="vwOpt12_txtExplanation" class="form-control" rows="5" style="width: 100%;"></textarea>
                                </div>
                            </div>
                            <div class="divHistoryAndAdd" style="margin-top: 10px;">
                                <button id="vwOpt12_btnAdd" type="button" title="Add New" class="btn btn-primary" onclick="addDisclosure('D12',1)">Add New </button>
                            </div>
                            <br />

                            <!-- Client-rendered HTML table -->
                            <%--<table id="vwOpt12_tblDisclosures" class="gridview" style="width: 98%;">
                                <thead class="gridViewHeader">
                                    <tr>
                                        <th>INCIDENT DATE</th>
                                        <th>STATE CODE</th>
                                        <th>COUNTY</th>
                                        <th>COURT</th>
                                        <th>CASE NUMBER</th>
                                        <th>CHARGE</th>
                                        <th>EXPLANATION DETAILS</th>
                                        <th style="width: 110px;">Actions</th>
                                    </tr>
                                </thead>
                                <tbody id="vwOpt12_tblBody">
                                    <!-- rows injected via JS -->
                                </tbody>
                            </table>--%>
                        </asp:view>

                        <!-- ====================== vwOpt13 ====================== -->
                        <asp:view id="vwOpt13" runat="server">
                            <div class="form-section" style="display: flex; flex-wrap: wrap; gap: 20px;">
                                <!-- DATE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt13_txtDate"><b>DATE (Approximate Date Allowed) *</b></label><br />
                                    <input id="vwOpt13_txtDate" class="form-control" placeholder="MM/DD/YYYY" type="text" />
                                </div>

                                <!-- STATE -->
                                <div style="flex: 1;">
                                    <label for="vwOpt13_ddlState"><b>STATE WHERE THE INCIDENT OCCURRED *</b></label><br />
                                    <select id="vwOpt13_ddlState" class="form-control">
                                        <option value="">Select State</option>
                                        <option value="OH">Ohio</option>
                                        <option value="CA">California</option>
                                        <option value="TX">Texas</option>
                                    </select>
                                </div>

                                <!-- COUNTY -->
                                <div style="flex: 1;">
                                    <label for="vwOpt13_ddlCounty"><b>COUNTY *</b></label><br />
                                    <select id="vwOpt13_ddlCounty" class="form-control">
                                        <option value="">Select County</option>
                                        <option value="1">Cuyahoga</option>
                                        <option value="2">Franklin</option>
                                        <option value="3">Hamilton</option>
                                    </select>
                                </div>

                                <!-- COURT -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt13_txtCourt"><b>COURT *</b></label><br />
                                    <input id="vwOpt13_txtCourt" class="form-control" placeholder="Enter Court" type="text" />
                                </div>

                                <!-- CASE NUMBER -->
                                <div style="flex: 10; min-width: 250px;">
                                    <label for="vwOpt13_txtCauseNumber"><b>CAUSE/CASE NUMBER *</b></label><br />
                                    <input id="vwOpt13_txtCauseNumber" class="form-control" placeholder="Enter Cause/Case Number" type="text" />
                                </div>

                                <!-- CHARGE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt13_txtCharge"><b>CHARGE *</b></label><br />
                                    <input id="vwOpt13_txtCharge" class="form-control" placeholder="Enter Charge" type="text" />
                                </div>

                                <!-- EXPLANATION -->
                                <div style="flex: 100%; margin-top: 20px;">
                                    <label for="vwOpt13_txtExplanation"><b>EXPLANATION/DETAILS *</b></label><br />
                                    <p style="font-size: 12px; color: #333;">
                                        Please provide a detailed explanation and attach all relevant documentation. If documentation is not available,
please explain why and where it can be obtained.
                                    </p>
                                    <textarea id="vwOpt13_txtExplanation" class="form-control" rows="5" style="width: 100%;"></textarea>
                                </div>
                            </div>
                            <div class="divHistoryAndAdd" style="margin-top: 10px;">
                                <button id="vwOpt13_btnAdd" type="button" title="Add New" class="btn btn-primary" onclick="addDisclosure('D13',1)">Add New </button>
                            </div>
                            <br />

                            <!-- Client-rendered HTML table -->
                            <%--<table id="vwOpt13_tblDisclosures" class="gridview" style="width: 98%;">
                                <thead class="gridViewHeader">
                                    <tr>
                                        <th>INCIDENT DATE</th>
                                        <th>STATE CODE</th>
                                        <th>COUNTY</th>
                                        <th>COURT</th>
                                        <th>CASE NUMBER</th>
                                        <th>CHARGE</th>
                                        <th>EXPLANATION DETAILS</th>
                                        <th style="width: 110px;">Actions</th>
                                    </tr>
                                </thead>
                                <tbody id="vwOpt13_tblBody">
                                    <!-- rows injected via JS -->
                                </tbody>
                            </table>--%>
                        </asp:view>

                        <!-- ====================== vwOpt14 ====================== -->
                        <asp:view id="vwOpt14" runat="server">
                            <div class="form-section" style="display: flex; flex-wrap: wrap; gap: 20px;">
                                <!-- DATE -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt14_txtDate"><b>DATE (Approximate Date Allowed) *</b></label><br />
                                    <input id="vwOpt14_txtDate" class="form-control" placeholder="MM/DD/YYYY" type="text" />
                                </div>

                                <!-- STATE -->
                                <div style="flex: 1;">
                                    <label for="vwOpt14_ddlState"><b>STATE WHERE THE INCIDENT OCCURRED *</b></label><br />
                                    <select id="vwOpt14_ddlState" class="form-control">
                                        <option value="">Select State</option>
                                        <option value="OH">Ohio</option>
                                        <option value="CA">California</option>
                                        <option value="TX">Texas</option>
                                    </select>
                                </div>

                                <!-- PROGRAM -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt14_txtProgram"><b>PROGRAM AFFECTED *</b></label><br />
                                    <input id="vwOpt14_txtProgram" class="form-control" placeholder="Enter Program" type="text" />
                                </div>

                                <!-- AGENCY -->
                                <div style="flex: 10; min-width: 250px;">
                                    <label for="vwOpt14_txtAgency"><b>AGENCY TAKING THE ACTION *</b></label><br />
                                    <input id="vwOpt14_txtAgency" class="form-control" placeholder="Enter Agency" type="text" />
                                </div>

                                <!-- ACTION -->
                                <div style="flex: 1; min-width: 250px;">
                                    <label for="vwOpt14_txtAction"><b>ACTION TAKEN *</b></label><br />
                                    <input id="vwOpt14_txtAction" class="form-control" placeholder="Enter Action taken" type="text" />
                                </div>

                                <!-- EXPLANATION -->
                                <div style="flex: 100%; margin-top: 20px;">
                                    <label for="vwOpt14_txtExplanation"><b>EXPLANATION/DETAILS *</b></label><br />
                                    <p style="font-size: 12px; color: #333;">
                                        Please provide a detailed explanation and attach all relevant documentation. If documentation is not available,
please explain why and where it can be obtained.
                                    </p>
                                    <textarea id="vwOpt14_txtExplanation" class="form-control" rows="5" style="width: 100%;"></textarea>
                                </div>
                            </div>

                            <div class="divHistoryAndAdd" style="margin-top: 10px;">
                                <%-- <asp:imagebutton id="vwOpt2_btnAddRelationships" runat="server" imageurl="~/Images/add.png"
                            oncommand="btnAdd_Click" commandname="vwOpt2" />--%>
                                <button id="vwOpt14_btnAdd" type="button" title="Add New" class="btn btn-primary" onclick="addDisclosure('D14',1)">Add New </button>
                            </div>
                            <br />

                            <!-- Client-rendered HTML table -->
                            <%--<table id="vwOpt14_tblDisclosures" class="gridview" style="width: 98%;">
                                <thead class="gridViewHeader">
                                    <tr>
                                        <th>INCIDENT DATE</th>
                                        <th>STATE CODE</th>
                                        <th>PROGRAM AFFECTED</th>
                                        <th>AGENCY TAKING ACTION</th>
                                        <th>ACTION TAKEN</th>
                                        <th>EXPLANATION DETAILS</th>
                                        <th style="width: 110px;">Actions</th>
                                    </tr>
                                </thead>
                                <tbody id="vwOpt14_tblBody">
                                    <!-- rows injected via JS -->
                                </tbody>
                            </table>--%>
                        </asp:view>

                        <!-- ====================== vwOpt15 ====================== -->
                        <asp:view id="vwOpt15" runat="server">
                            <div class="form-section" style="display: flex; flex-wrap: wrap; gap: 20px;">
                                <!-- EXPLANATION/DETAILS -->                                                               
                                <div style="flex: 100%; margin-top: 20px;">
                                    <label for="vwOpt16_txtExplanation"><b>EXPLANATION/DETAILS *</b></label><br />
                                    <p style="font-size: 12px; color: #333;">
                                        Please provide a detailed explanation and attach all relevant documentation. If documentation is not available,
please explain why and where it can be obtained.
                                    </p>
                                    <textarea id="vwOpt16_txtExplanation" class="form-control" rows="5" style="width: 100%;"></textarea>
                                </div>
                            </div>
                        </asp:view>

                        <!-- ====================== vwOpt16 ====================== -->
                        <asp:view id="vwOpt16" runat="server">
                        </asp:view>



                    </asp:multiview>
                </asp:panel>
            </td>
        </tr>
    </table>

    <%-- <asp:hiddenfield id="hdnAccessToken" runat="server" />
    <asp:hiddenfield id="hdnWebAPIURL" runat="server" />
    <asp:hiddenfield id="hdnRegId" runat="server" />--%>
</div>

<asp:hiddenfield id="hdnChanged" runat="server" />

