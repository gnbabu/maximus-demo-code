<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_HospiceBenefitPeriod" Codebehind="HospiceBenefitPeriod.ascx.cs" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>
<%@ register tagprefix="telerik" namespace="Telerik.Web.UI" assembly="Telerik.Web.UI" %>
<%@ register src="~/PopupControls/HospiceCheckEligibility.ascx" tagprefix="uc1" tagname="HospiceCheckEligibility" %>
<%@ register assembly="eWorld.UI" namespace="eWorld.UI" tagprefix="ew" %>
<%@ Register Src="~/PopupControls/RecipientEligibilitySearch.ascx" TagPrefix="uc4" TagName="RecipientEligibilitySearch" %>
<%@ Register Src="~/PopupControls/HospiceRecipientServiceLocation.ascx" TagPrefix="uc1" TagName="HospiceRecipientServiceLocation" %>

<%@ Register Src="~/PopupControls/HospiceRecipientInformation.ascx" TagPrefix="uc1" TagName="HospiceRecipientInformation" %>
<%@ Register Src="~/PopupControls/HospiceEnrollmentAndDisenrollment.ascx" TagPrefix="uc2" TagName="HospiceEnrollmentAndDisenrollment" %>
<%@ Register Src="~/PopupControls/HospiceOtherPayerSpan.ascx" TagPrefix="uc6" TagName="HospiceOtherPayerSpan" %>
<%@ Register Src="~/PopupControls/HospiceEpisodeofCare.ascx" TagPrefix="uc7" TagName="HospiceEpisodeofCare" %>
<%@ Register Src="~/PopupControls/HospiceTerminalIllnessDiagnosis.ascx" TagPrefix="uc8" TagName="HospiceTerminalIllnessDiagnosis" %>
<%@ Register Src="~/PopupControls/HospiceProviderServiceSpan.ascx" TagPrefix="uc9" TagName="HospiceProviderServiceSpan" %>
<%@ Register Src="~/PopupControls/HospiceHLTCFProviderService.ascx" TagPrefix="uc10" TagName="HospiceHLTCFProviderService" %>
<%@ Register Src="~/PopupControls/HospiceAttachment.ascx" TagPrefix="uc11" TagName="HospiceAttachment" %>
<%@ Register Src="~/PopupControls/HospiceDocumentsByMail.ascx" TagPrefix="uc12" TagName="HospiceDocumentsByMail" %>
<%--<script src="../Scripts/jquery-1.4.1.min.js"></script>
<script src="../Scripts/jquery-1.4.1.js"></script>
<link href="../Styles/jquery-ui.css" rel="Stylesheet" type="text/css" />--%>
<script type="text/javascript">
    $(function () {
        $("[id*=gvHospiceBenefitPeriod] [id*=ftnAdd]").click(function () {
            var isValid = true;
            try {
                var row = $(this).closest("tr");
                var requiredControles = ["ftxtEffectiveDate", "ftxtEndDate"];
                var effectvieData = $.trim(row.find("[id*=ftxtEffectiveDate]").val());
                var endData = $.trim(row.find("[id*=ftxtEndDate]").val());
                var segmentIndecator = $.trim(row.find("[id*=fddlBenefitSegmentIndicator]").html());
                isValid = RequiredFieldsValidations(row, requiredControles);
                var actionType = $("[id*=ddlHospiceApplicationType]").val();
                if (isValid === true) {
                    isValid = DateValidations(effectvieData, endData, "BFErrorMessage");

                    /// added a logic to validate 90/60 days
                    if (endData != '') {
                        var someeffectvieData = new Date(effectvieData);
                        var someendData = new Date(endData);
                        if (segmentIndecator.toUpperCase() === "FIRST 90 DAY PERIOD" || segmentIndecator.toUpperCase() === "SECOND 90 DAY PERIOD") {
                            var days = 90;
                            days = days - 1;
                            var reseffectvieData = someeffectvieData.setDate(someeffectvieData.getDate() + days);
                            if (someendData > reseffectvieData) {

                                $("[id*=BFErrorMessage]").text("End Date cannot be greater than 90 days.");
                                isValid = false;
                            } else {
                                $("[id*=BFErrorMessage]").text("");
                                isValid = true;
                            }
                        }
                        if (segmentIndecator.toUpperCase() === "SUBSEQUENT 60 DAY PERIOD") {
                            var days = 59;
                            var reseffectvieData = someeffectvieData.setDate(someeffectvieData.getDate() + days);
                            if (someendData > reseffectvieData) {
                                $("[id*=BFErrorMessage]").text("End Date cannot be greater than 60 days.");
                                isValid = false;
                            } else {
                                $("[id*=BFErrorMessage]").text("");
                                isValid = true;
                            }
                        }
                    }
                }

                var actionType = $("[id*=ddlHospiceApplicationType]").val();
                if (isValid === true && actionType !== "CHGPR") {
                    isValid = DateValidationsOverlap(effectvieData, endData, "gvHospiceBenefitPeriod", "BFErrorMessage");
                }
                if (isValid === true) {
                    isValid = BenefitSegment90DaysCheck(effectvieData, endData, segmentIndecator, "gvHospiceBenefitPeriod", "BFErrorMessage");
                }
            } catch (error) {

                console.error('validate required controls error', error.message);
                isValid = false;
            }
            return isValid;
        });
        $("[id*=gvHospiceBenefitPeriod] [id*=btnUpdate]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["etxtEffectiveDate", "etxtEndDate"];
            var effectvieData = $.trim(row.find("[id*=etxtEffectiveDate]").val());
            var endData = $.trim(row.find("[id*=etxtEndDate]").val());
            var segmentIndecator = $.trim(row.find("[id*=eddlBenefitSegmentIndicator]").html());
            var isValid = RequiredFieldsValidations(row, requiredControles);
            var actionType = $("[id*=ddlHospiceApplicationType]").val();
            
            if (isValid === true && actionType === "MAINT") {
                var reqControles = ["eddlReasonForUpdate"];
                isValid = RequiredFieldsValidations(row, reqControles);
            }
            if (isValid === true) {
                isValid = DateValidations(effectvieData, endData, "BFErrorMessage");

                if (endData != '') {
                    var someeffectvieData = new Date(effectvieData);
                    var someendData = new Date(endData);
                    if (segmentIndecator.toUpperCase() === "FIRST 90 DAY PERIOD") {
                        var days = 89;
                        var reseffectvieData = someeffectvieData.setDate(someeffectvieData.getDate() + days);
                        if (someendData > reseffectvieData) {

                            $("[id*=BFErrorMessage]").text("End Date cannot be greater than 90 days.");
                            isValid = false;
                        } else {
                            $("[id*=BFErrorMessage]").text("");
                            isValid = true;
                        }
                    }
                    if (segmentIndecator.toUpperCase() === "SECOND 90 DAY PERIOD") {
                        var days = 89;
                        var reseffectvieData = someeffectvieData.setDate(someeffectvieData.getDate() + days);
                        if (someendData > reseffectvieData) {
                            $("[id*=BFErrorMessage]").text("End Date cannot be greater than 90 days.");
                            isValid = false;
                        } else {
                            $("[id*=BFErrorMessage]").text("");
                            isValid = true;
                        }
                    }
                    if (segmentIndecator.toUpperCase() === "SUBSEQUENT 60 DAY PERIOD") {
                        var days = 59;
                        var reseffectvieData = someeffectvieData.setDate(someeffectvieData.getDate() + days);
                        if (someendData > reseffectvieData) {
                            $("[id*=BFErrorMessage]").text("End Date cannot be greater than 60 days.");
                            isValid = false;
                        } else {
                            $("[id*=BFErrorMessage]").text("");
                            isValid = true;
                        }
                    }
                }
            }


            if (isValid === true) {
                isValid = BenefitSegment90DaysCheck(effectvieData, endData, segmentIndecator, "gvHospiceBenefitPeriod", "BFErrorMessage");
            }
            return isValid;
        });

        $("[id*=gvHospiceAttendingPhysician] [id*=ftnAdd]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["fddlBenefitLineNo", "ftxtNPI", "ftxtWrittenCertificationDate"];
            var isValid = RequiredFieldsValidations(row, requiredControles);
            if (isValid == true) {
                var ftxtNPI = $.trim(row.find("[id*=ftxtNPI]").val());
                if (ftxtNPI.length != 10) {
                    $("[id*=AttendingPhyErrorMessage]").text("NPI Should be 10 Digit number.");
                    isValid = false;
                }
            }

            var ftxtOralCertificationDate = $.trim(row.find("[id*=ftxtOralCertificationDate]").val());
            var ftxtWrittenCertificationDate = $.trim(row.find("[id*=ftxtWrittenCertificationDate]").val());
            if (isValid === true) {
                isValid = DateValidations(ftxtOralCertificationDate, ftxtWrittenCertificationDate, "AttendingPhyErrorMessage");
            }
            if (isValid == true) {
                if (isFutureDate(ftxtOralCertificationDate)) {
                    $("[id*=AttendingPhyErrorMessage]").text("OralCertificationDate - Future date is not allowed.");
                    return false;
                }

                if (isFutureDate(ftxtWrittenCertificationDate)) {
                    $("[id*=AttendingPhyErrorMessage]").text("WrittenCertificationDate - Future date is not allowed.");
                    return false;
                }
            }
            return isValid;
        });
        $("[id*=gvHospiceAttendingPhysician] [id*=btnUpdate]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["eddlBenefitLineNo", "etxtNPI", "etxtWrittenCertificationDate"];
            var isValid = RequiredFieldsValidations(row, requiredControles);
            if (isValid == true) {
                var etxtNPI = $.trim(row.find("[id*=etxtNPI]").val());
                if (etxtNPI.length != 10) {
                    $("[id*=AttendingPhyErrorMessage]").text("NPI Should be 10 Digit number.");
                    isValid = false;
                }
            }
            var etxtOralCertificationDate = $.trim(row.find("[id*=etxtOralCertificationDate]").val());
            var etxtWrittenCertificationDate = $.trim(row.find("[id*=etxtWrittenCertificationDate]").val());
            if (isValid === true) {
                isValid = DateValidations(etxtOralCertificationDate, etxtWrittenCertificationDate, "AttendingPhyErrorMessage");
            }
            if (isValid == true) {
                if (isFutureDate(etxtOralCertificationDate)) {
                    $("[id*=AttendingPhyErrorMessage]").text("OralCertificationDate - Future date is not allowed.");
                    return false;
                }

                if (isFutureDate(etxtWrittenCertificationDate)) {
                    $("[id*=AttendingPhyErrorMessage]").text("WrittenCertificationDate - Future date is not allowed.");
                    return false;
                }
            }
            return isValid;
        });

        $("[id*=gvHospiceIDGPhysician] [id*=ftnAdd]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["fddlBenefitLineNo", "ftxtNPI", "ftxtWrittenCertificationDate"];
            var isValid = RequiredFieldsValidations(row, requiredControles);
            if (isValid == true) {
                var ftxtNPI = $.trim(row.find("[id*=ftxtNPI]").val());
                if (ftxtNPI.length != 10) {
                    $("[id*=IDGPhyErrorMessage]").text("10 Digit number required.");
                    isValid = false;
                }
            }
            var ftxtOralCertificationDate = $.trim(row.find("[id*=ftxtOralCertificationDate]").val());
            var ftxtWrittenCertificationDate = $.trim(row.find("[id*=ftxtWrittenCertificationDate]").val());
            if (isValid === true) {
                isValid = DateValidations(ftxtOralCertificationDate, ftxtWrittenCertificationDate, "IDGPhyErrorMessage");
            }
            if (isValid == true) {
                if (isFutureDate(ftxtOralCertificationDate)) {
                    $("[id*=IDGPhyErrorMessage]").text("OralCertificationDate - Future date is not allowed.");
                    return false;
                }
                if (isFutureDate(ftxtWrittenCertificationDate)) {
                    $("[id*=IDGPhyErrorMessage]").text("WrittenCertificationDate - Future date is not allowed.");
                    return false;
                }
            }
            return isValid;
        });
        $("[id*=gvHospiceIDGPhysician] [id*=btnUpdate]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["eddlBenefitLineNo", "etxtNPI", "etxtWrittenCertificationDate"];
            var isValid = RequiredFieldsValidations(row, requiredControles);
            if (isValid == true) {
                var etxtNPI = $.trim(row.find("[id*=etxtNPI]").val());
                if (etxtNPI.length != 10) {
                    $("[id*=IDGPhyErrorMessage]").text("10 Digit number required.");
                    isValid = false;
                }
            }
            var etxtOralCertificationDate = $.trim(row.find("[id*=etxtOralCertificationDate]").val());
            var etxtWrittenCertificationDate = $.trim(row.find("[id*=etxtWrittenCertificationDate]").val());
            if (isValid === true) {
                isValid = DateValidations(etxtOralCertificationDate, etxtWrittenCertificationDate, "IDGPhyErrorMessage");
            }
            if (isValid == true) {
                if (isFutureDate(etxtOralCertificationDate)) {
                    $("[id*=IDGPhyErrorMessage]").text("Oral Certification Date - Future date is not allowed.");
                    return false;
                }
                if (isFutureDate(etxtWrittenCertificationDate)) {
                    $("[id*=IDGPhyErrorMessage]").text("Written Certification Date - Future date is not allowed.");
                    return false;
                }
            }
            return isValid;
        });
    });
    function isFutureDate(idate) {
        var today = new Date().getTime(),
            idate = idate.split("/");

        idate = new Date(idate[2], idate[0] - 1, idate[1]).getTime();
        return (today - idate) < 0;
    }
    function ReloadHospicePage() {
    }
    function RequiredFieldsValidations(row, requiredControles) {
        var isValid = true;
        $.each(requiredControles, function (index, Id) {
            var label = row.find("[id*=" + Id + "]").next("SPAN");
            label.hide();
            if ($.trim(row.find("[id*=" + Id + "]").val())) {
                label.show();
                isValid = false;
            }
        });

        return isValid;
    }
    

    function DateValidations(effectiveDate, endDate, errControl) {
        if (effectiveDate != '' && !isDate(effectiveDate)) {
            if (errControl == "AttendingPhyErrorMessage" || errControl == "IDGPhyErrorMessage") {
                $("[id*=" + errControl + "]").text("Oral Certification Date " + effectiveDate + " is not a valid date.");
            }
            else {
                $("[id*=" + errControl + "]").text("Effective Date " + effectiveDate + " is not a valid date.");
            }
            return false;
        }
        if (!isDate(endDate)) {
            if (errControl == "AttendingPhyErrorMessage" || errControl == "IDGPhyErrorMessage") {
                $("[id*=" + errControl + "]").text("Written Certification Date " + endDate + " is not a valid date.");
            }
            else {
                $("[id*=" + errControl + "]").text("End Date " + endDate + " is not a valid date.");
            }
            return false;
        }
        //No need to check the enddate validation for Oral Certification and Written Certification Date
        if (errControl == "AttendingPhyErrorMessage" || errControl == "IDGPhyErrorMessage") {
            return true;
        }
        if (effectiveDate != '' && (Date.parse(effectiveDate) > Date.parse(endDate))) {
            if (errControl == "AttendingPhyErrorMessage" || errControl == "IDGPhyErrorMessage") {
                $("[id*=" + errControl + "]").text("Oral Certification Date Should be less than or equal to End date.");
            }
            else {
                $("[id*=" + errControl + "]").text("Effective Date Should be less than or equal to End date.");
            }
            return false;
        }
        if (effectiveDate != '' && (Date.parse(endDate) < Date.parse(effectiveDate))) {
            if (errControl == "AttendingPhyErrorMessage" || errControl == "IDGPhyErrorMessage") {
                $("[id*=" + errControl + "]").text("Written Certification Date Should be less than or equal to End date.");
            }
            else {
                $("[id*=" + errControl + "]").text("EndDate Should be grater than or equal to EffectvieData.");
            }

            return false;
        }
        return true;
    }
    function isDate(txtDate) {
        var currVal = txtDate;
        var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/;
        var dtArray = currVal.match(rxDatePattern);
        if (dtArray == null)
            return false;
        dtMonth = dtArray[1];
        dtDay = dtArray[3];
        dtYear = dtArray[5];

        if (dtMonth < 1 || dtMonth > 12)
            return false;
        else if (dtDay < 1 || dtDay > 31)
            return false;
        else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
            return false;
        else if (dtMonth == 2) {
            var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
            if (dtDay > 29 || (dtDay == 29 && !isleap))
                return false;
        }
        return true;
    }
    function DuplicateCheck(segmentIndecator) {
        var isduplicate = false;
        $("[id*=gvHospiceBenefitPeriod] tbody tr").each(function () {
            if (!this.rowIndex) return;
            var lblBenefitSegmentIndicator = $(this).find("td span[id*=lblBenefitSegmentIndicator]").html();
            if (lblBenefitSegmentIndicator !== null &&
                lblBenefitSegmentIndicator !== undefined) {
                if (lblBenefitSegmentIndicator === segmentIndecator) {
                    isduplicate = true;
                    $("[id*=BFErrorMessage]").text("Repetition of SegmentIndicator not allowed.");
                }
            }
        });

        return isduplicate;
    }
    function BenefitSegmentIndicatorChange(ctrl) {
        var selectedItem = $(ctrl).find('option:selected').text();
        var hdCntrl = $(ctrl).closest('tr').find('td input[id*="hdnBenefitSegmentIndicator"]');
        hdCntrl.val(selectedItem);
    }
    function ReasonForUpdateChange(ctrl) {
        var selectedItem = $(ctrl).find('option:selected').val();
        var hdCntrl = $(ctrl).closest('tr').find('td input[id*="hdnReasonForUpdate"]');
        hdCntrl.val(selectedItem);
    }
    function BindSegmentIndicatorWithBefetPeriods(data, gridIds) {
        if (data != '') {
            var ids = gridIds.split(",");
            data = JSON.parse(data);
            $.each(ids, function (index, Id) {
                var ddResult = data;
                var i = 0;
                var dropdownId = $("[id*=" + Id + "] [id*=ftnAdd]").closest("tr").find("[id*=fddlBenefitSegmentIndicator]");
                dropdownId.empty();
                dropdownId.append("<option value='0'></option>");
                for (; i < ddResult.length; i++) {
                    dropdownId.append($('<option></option>').attr("value", ddResult[i].INDICATOR_TYPE_VALUE).text(ddResult[i].INDICATOR_TYPE_DESC));
                }
            });
        }
    }
    function BindBenefitPeriodLineNos(data, gridIds) {
        if (data != '') {
            var ids = gridIds.split(",");
            data = JSON.parse(data);
            $.each(ids, function (index, Id) {
                var ddResult = data;
                var i = 0;
                var dropdownId = $("[id*=" + Id + "] [id*=ftnAdd]").closest("tr").find("[id*=fddlBenefitLineNo]");
                dropdownId.empty();
                dropdownId.append("<option value='0'></option>");
                for (; i < ddResult.length; i++) {
                    dropdownId.append($('<option></option>').attr("value", ddResult[i].BenefitPeriod).text(ddResult[i].LineNo));
                }
            });
        }
    }
    function ValidProviderNPI(npi) {
        var isvalid = true;
        //var post_data = JSON.stringify({ "npi": npi });
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            url: webApiClaims + "ValidProviderNPI?npi=" + npi,
            type: 'GET',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: 'application/json;charset=utf-8',
            dataType: 'json',
            //data: post_data,
            success: function (data) {
                data = JSON.parse(data);
                if (data === false) {
                    $("[id*=IDGPhyErrorMessage]").text("Incorrect NPI for Hospice IDG physician.");
                    isvalid = false;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('<p>status code: ' + jqXHR.status + '</p><p>errorThrown: ' + errorThrown + '</p><p>jqXHR.responseText:</p><div>' + jqXHR.responseText + '</div>');
            }
        });

        return isvalid;
    }
    function Add90DaystoEndDate(ctrl, action) {
        $("[id*=BFErrorMessage]").text("");
        var txtEffectiveDate = (action === 'Edit') ? $(ctrl).closest('tr').find('td input[id*="etxtEffectiveDate"]') : $(ctrl).closest('tr').find('td input[id*="ftxtEffectiveDate"]');
        var txtEndDate = (action === 'Edit') ? $(ctrl).closest('tr').find('td input[id*="etxtEndDate"]') : $(ctrl).closest('tr').find('td input[id*="ftxtEndDate"]');
        var effDate = new Date(txtEffectiveDate.val());
        var days = 90;
        var segmentIndecator = (action === 'Edit') ? $.trim($(ctrl).closest('tr').find('td span[id*="eddlBenefitSegmentIndicator"]').html()) : $.trim($(ctrl).closest('tr').find('td span[id*="fddlBenefitSegmentIndicator"]').html());
        if (segmentIndecator.toLowerCase() === "subsequent 60 day period") {
            days = 60;
        }
        if (!isNaN(effDate.getTime())) {
            days = days - 1;
            effDate.setDate(effDate.getDate() + days);
            txtEndDate.val(effDate.toInputFormat());
        }
    }
    function Validate90Or60DaysLogic(ctrl){
        var effectvieData = $(ctrl).closest('tr').find('td input[id*="ftxtEffectiveDate"]').val();
        var endData = $(ctrl).closest('tr').find('td input[id*="ftxtEndDate"]').val();
        var segmentIndecator = $.trim($(ctrl).closest('tr').find('td span[id*="fddlBenefitSegmentIndicator"]').html());
        var isValid = DateValidations(effectvieData, endData, "BFErrorMessage");
        if (isValid && endData != '') {
            var someeffectvieData = new Date(effectvieData);
            var someendData = new Date(endData);
            if (segmentIndecator.toUpperCase() === "FIRST 90 DAY PERIOD" || segmentIndecator.toUpperCase() === "SECOND 90 DAY PERIOD") {
                var days = 89;
                var reseffectvieData = someeffectvieData.setDate(someeffectvieData.getDate() + days);
                if (someendData > reseffectvieData) {
                    
                    $("[id*=BFErrorMessage]").text("Days count exceeded the allowed range.");
                    isValid = false;
                } else {
                    $("[id*=BFErrorMessage]").text("");
                    isValid = true;
                }
            }

            if (segmentIndecator.toUpperCase() === "SUBSEQUENT 60 DAY PERIOD") {
                var days = 59;
                var reseffectvieData = someeffectvieData.setDate(someeffectvieData.getDate() + days);
                if (someendData > reseffectvieData) {
                    $("[id*=BFErrorMessage]").text("Days count exceeded the allowed range.");
                    isValid = false;
                } else {
                    $("[id*=BFErrorMessage]").text("");
                    isValid = true;
                }
            }
        }
}
    Date.prototype.toInputFormat = function () {
        var yyyy = this.getFullYear().toString();
        var mm = (this.getMonth() + 1).toString(); // getMonth() is zero-based
        var dd = this.getDate().toString();
        return mm + "/" + dd + "/" + yyyy; // padding
    }
    function SearchProviderNpiinfo() {
        $('#gvHospiceProviderNPISearch').empty();
        var npi = $('#<%=txtNPI.ClientID%>').val();
     var medicaidid = $('#<%=txtMedicaidID.ClientID%>').val();
     var lastName = $('#<%=txtBusinessLastName.ClientID%>').val();
     var firstName = $('#<%=txtFirstName.ClientID%>').val();
        var etxtProviderNPI = $.trim(npi);
        if (etxtProviderNPI === "" && medicaidid === "" && lastName === "" && firstName === "") {
            $("[id*=HospiceProviderSearchErrorMessage]").text("Please Enter at least one search criteria.");
            return;
        }
        if (etxtProviderNPI.length > 0 && etxtProviderNPI.length != 10) {
            $("[id*=HospiceProviderSearchErrorMessage]").text("NPI Should be 10 Digit number.");
            return;
        }
        if (medicaidid.length > 0 && medicaidid.length != 7) {
            $("[id*=HospiceProviderSearchErrorMessage]").text("7 digits Medicaid ID is required.");
            return;
        }
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiHospice + "Search?npi=" + npi + "&&medicaidid=" + medicaidid + "&&lastName=" + lastName + "&&firstName=" + firstName,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: 'application/json;charset=utf-8',
            dataType: 'json',
            success: function (result) {
                if (result === "") {
                    $("[id*=HospiceProviderSearchErrorMessage]").text("No data found.");
                    $("[id*=gvHospiceProviderNPISearch] tr").not($("[id*=gvHospiceProviderNPISearch] tr:first-child")).css('visibility', 'hidden');
                }
                else {
                    $("[id*=gvHospiceProviderNPISearch] tr").not($("[id*=gvHospiceProviderNPISearch] tr:first-child")).css('visibility', 'visible');
                    var row = $("[id*=gvHospiceProviderNPISearch] tr:last-child").clone(true);
                    $("[id*=gvHospiceProviderNPISearch] tr").not($("[id*=gvHospiceProviderNPISearch] tr:first-child")).remove();
                    if (result.length > 0) {
                        for (var i = 0; i < result.length; i++) {
                            $("td", row).eq(0).html(result[i].NPI);
                            $("td", row).eq(1).html(result[i].MEDICAID_ID);
                            $("td", row).eq(2).html(result[i].LAST_OR_BUSINESS_NAME);
                            $("td", row).eq(3).html(result[i].FIRST_NAME);
                            $("[id*=gvHospiceProviderNPISearch]").append(row);
                            row = $("[id*=gvHospiceProviderNPISearch] tr:last-child").clone(true);
                        }
                        $("[id*=HospiceProviderSearchErrorMessage]").text("");
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=HospiceProviderSearchErrorMessage]").text(jqXHR.responseText);
                //alert('<p>status code: ' + jqXHR.status + '</p><p>errorThrown: ' + errorThrown + '</p><p>jqXHR.responseText:</p><div>' + jqXHR.responseText + '</div>');
            }
        });

    }
    function FillSelectedProviderNPIInfo(ctrl) {
        var row = $(ctrl).closest("tr");
        var action = $('#hdSelectedSearchNpiButtonAction').val();
        var gridId = $('#hdSelectedSearchNpiGridId').val();
        var behaviorId = $('#hdSelectedSearchNpiBehaviorId').val();
        if (action == 'Add') {
            var fillRow = $("[id*=" + gridId + "] [id*=ftnAdd]").closest("tr");
            fillRow.find("[id*=ftxtNPI]").val(row.find("td").eq(0).html());
        }
        else {
            var fillRow = $("[id*=" + gridId + "] [id*=btnUpdate]").closest("tr");
            fillRow.find("[id*=etxtNPI]").val(row.find("td").eq(0).html());
        }
     
        if (behaviorId === 'modelProviderSearch1' || behaviorId === 'modelProviderSearch2') {
            var value = row.find("td").eq(1).html();
            $("[id*=hdnAttendingMedID]").val(value);
          <%--  alert($('#<%=hdnAttendingMedID.ClientID%>').val());--%>
        }
        if (behaviorId === 'modelProviderSearch3' || behaviorId === 'modelProviderSearch4') {           
            var value = row.find("td").eq(1).html();
            $("[id*=hdnIDGMedID]").val(value);
        }
        $find("" + behaviorId + "").hide();
    }
    function SearchNpiClickEvent(action, gridId, behaviorId) {
        $("[id*=HospiceProviderSearchErrorMessage]").text("");
        $("[id*=gvHospiceProviderNPISearch] tr").not($("[id*=gvHospiceProviderNPISearch] tr:first-child")).css('visibility', 'hidden');
        $('#hdSelectedSearchNpiGridId').val(gridId);
        $('#hdSelectedSearchNpiButtonAction').val(action);
        $('#hdSelectedSearchNpiBehaviorId').val(behaviorId);

    }
    function setBlankNPISearchValue() {
        $('#<%=txtNPI.ClientID%>').val("");
        $('#<%=txtMedicaidID.ClientID%>').val("");
        $('#<%=txtBusinessLastName.ClientID%>').val("");
        $('#<%=txtFirstName.ClientID%>').val("");
    }    
</script>
<style>
    select, option {
        width: 150px;
    }

    table.gridview td, .rgRow td, .rgAltRow td, table.gridViewSmallFont td {
        font-size: 14px;
    }

    select {
        min-width: 90%;
        height: 25px;
    }

    .gridViewFooter {
        background-color: white;
    }

    .gridViewSelected, .gridViewSelected td, .gridViewSelected tr {
        background-color: white;
    }
</style>

<ajax:accordion id="AccordionHospiceBenefitPeriod" runat="Server" selectedindex="0" enableviewstate="false"
    headercssclass="accordionHeader" headerselectedcssclass="accordionHeaderSelected" contentcssclass="accordionContent"
    autosize="None" fadetransitions="true" transitionduration="250" framespersecond="40" requireopenedpane="false"
    suppressheaderpostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionPaneHospiceBenefitPeriod" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblHospiceBenefitPeriod" class="expandcollapse" runat="server" Text="- * HOSPICE BENEFIT PERIOD"></asp:Label>
            </Header>
            <Content>
                 <asp:HiddenField ID="HdBenefitPeriodEffDate" runat="server"  />
                <asp:HiddenField ID="HdBenefitPeriodEndDate" runat="server" />
                <asp:HiddenField ID="hdnMedicaidID" runat="server"  />
                <asp:HiddenField ID="hdnNPI" runat="server"  />
                <asp:HiddenField ID="hdnAttendingMedID" runat="server"  />
                <asp:HiddenField ID="hdnIDGMedID" runat="server"  />
                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvHospiceBenefitPeriod" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvHospiceBenefitPeriod_PageIndexChanging"
                        OnRowEditing="gvHospiceBenefitPeriod_RowEditing" OnRowUpdating="gvHospiceBenefitPeriod_RowUpdating" OnRowCancelingEdit="gvHospiceBenefitPeriod_RowCancelingEdit"
                        OnRowDeleting="gvHospiceBenefitPeriod_RowDeleting" OnRowDataBound="gvHospiceBenefitPeriod_RowDataBound" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>
                            <asp:TemplateField HeaderText="Line No" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lbllineNo" runat="server" Text='<%# Bind("BenPeriod") %>'></asp:Label>
                                     <asp:HiddenField ID="hdnIsDifferentProdvider" runat="server" Value='<%#Bind("IsDifferentProdvider") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Benefit Period Type" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenefitSegmentIndicator" runat="server" Text='<%# Bind("BenPeriodType") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="hdnBenefitSegmentIndicator" runat="server" Value='<%#Bind("BenPeriodType") %>' />
                                     <asp:Label ID="eddlBenefitSegmentIndicator" runat="server"></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                     <asp:Label ID="fddlBenefitSegmentIndicator" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective Date" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Bind("BenPeriodEffDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtEffectiveDate" autocomplete="off"  runat="server" Text='<%# Bind("BenPeriodEffDate", "{0:MM/dd/yyyy}") %>' onChange="Add90DaystoEndDate(this,'Edit')"></asp:TextBox> 
                                    <ajax:CalendarExtender ID="cleEffecDate" TargetControlID="etxtEffectiveDate" runat="server"  />
                                    <span style="color:red; display:none"><br />Effective date is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtEffectiveDate" autocomplete="off" runat="server" onChange="Add90DaystoEndDate(this,'Add')"></asp:TextBox> 
                                    <ajax:CalendarExtender ID="clEffecDate" TargetControlID="ftxtEffectiveDate" runat="server" />
                                    <span style="color:red; display:none"><br />Effective date is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EndDate" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("BenPeriodEndDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtEndDate" autocomplete="off" runat="server" Text='<%# Bind("BenPeriodEndDate", "{0:MM/dd/yyyy}") %>' onChange="Validate90Or60DaysLogic(this)"></asp:TextBox> 
                                    <ajax:CalendarExtender ID="cleEndDate" TargetControlID="etxtEndDate" runat="server" />
                                    <span style="color:red; display:none"><br />End date is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtEndDate" autocomplete="off" runat="server" onChange="Validate90Or60DaysLogic(this)"></asp:TextBox> 
                                    <ajax:CalendarExtender ID="clEndDate" TargetControlID="ftxtEndDate" runat="server" />
                                    <span style="color:red; display:none"><br />End date is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate> 
                                    <asp:TextBox ID="etxtStatus" runat="server" Text='<%# Bind("Status") %>' Enabled="false"></asp:TextBox>                     
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtStatus" runat="server" Text="Complete" Enabled="false"></asp:TextBox> 
                                </FooterTemplate>                                
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Reason For Update" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblReasonForUpdate" runat="server" Text='<%# Bind("BenUpdateReason") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="hdnReasonForUpdate" runat="server" Value='<%#Bind("BenUpdateReason") %>' />
                                    <asp:DropDownList ID="eddlReasonForUpdate" runat="server" onchange="javascript:ReasonForUpdateChange(this);" ></asp:DropDownList>
                                    <span style="color:red; display:none"><br />Reason for update is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="fddlReasonForUpdate" runat="server" Enabled="false"></asp:DropDownList>  
                                      <span style="color:red; display:none"><br />Reason for update is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CssClass="button" CausesValidation="false" />
                                    &nbsp;
                <asp:Button ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" Visible='<%# Convert.ToBoolean(Eval("IsFromInquiry")) == true ? false : true %>'
                    CssClass="button" OnClientClick='return confirm("Are you sure you want to delete this record?");' CausesValidation="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button ID="btnUpdate" Text="Update" runat="server" CommandName="Update" CssClass="button"  />
                                    &nbsp;
                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" CssClass="button" CausesValidation="false" />
                                </EditItemTemplate>
                                 <FooterTemplate>
                                            <asp:Button ID="ftnAdd" runat="server" Text="Add New" OnClick="fbtnAdd_Click"  CssClass="button" Style="width: auto !important;"  />
                                        </FooterTemplate>                               
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>

                    <asp:Button runat="server" ID="btnCheckEligibility" OnClick="btnCheckEligibility_Click" Text="Check Eligibility" Style="background-color: darkslateblue !important; margin-right: 100%" CssClass="buttonBox" />
                     <asp:Button runat="server" ID="btnShowCheckEligibility"  Style="display:none"   />
                    <ajax:ModalPopupExtender ID="ShowPopupCheckEligibility"  runat="server" PopupControlID="pnlCEsearch" TargetControlID="btnShowCheckEligibility" BackgroundCssClass="modalBackground" />
                    
                    <asp:Panel ID="pnlCEsearch" runat="server" CssClass="modalPopup" Style="display: none; max-height: 70%; min-width: 610px; overflow:auto">
                        <asp:Panel ID="pnlCE" runat="server">
                           <asp:Button runat="server" ID="btnCloseCH" OnClick="btnCloseCH_Click"  Text="X" style="float: right;background-image: none;border: 0px;border-radius: 0px;" CausesValidation="false" />
                                    <div style="text-align: left; padding: 42px;overflow-x: auto;overflow-y: auto" class="container-fluid";>
                                        <div class="row">
                                           <%-- <uc1:HospiceCheckEligibility ID="HospiceCheckEligibility1" runat="server" Visible="true" EnableViewState="true" />--%>
                                            <%-- <uc4:RecipientEligibilitySearch ID="uc4RecipientEligibilitySearch" runat="server" Visible="true" EnableViewState="true" />--%>
                                             <asp:PlaceHolder runat="server" ID="HospicePlaceholderInitial"></asp:PlaceHolder>
                                        </div>
                                    </div>
                           <%-- <div style="text-align: center; padding: 42px;" class="container-fluid";>
                          <asp:Button ID="btnClose" runat="server" Text="Close" />
                                 </div>--%>
                        </asp:Panel>
                       
                    </asp:Panel>
                    <div style="text-align: center">
                                <span id="BFErrorMessage" runat="server" style="color: red;"></span>
                            </div>
                </div>

            </Content>

        </ajax:AccordionPane>
    </Panes>

</ajax:accordion>
 <uc1:HospiceRecipientServiceLocation ID="uc1HospiceRecipientServiceLocation" runat="server" Visible="true" EnableViewState="true" />
<ajax:accordion id="AccordionHospiceAttendingPhysician" runat="Server" selectedindex="0" enableviewstate="false"
    headercssclass="accordionHeader" headerselectedcssclass="accordionHeaderSelected" contentcssclass="accordionContent"
    autosize="None" fadetransitions="true" transitionduration="250" framespersecond="40" requireopenedpane="false"
    suppressheaderpostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionPaneHospiceAttendingPhysician" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblHospiceAttendingPhysician" class="expandcollapse" runat="server" Text="- * ATTENDING PHYSICIAN INFORMATION"></asp:Label>
            </Header>
            <Content>

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvHospiceAttendingPhysician" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true" 
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvHospiceAttendingPhysician_PageIndexChanging"
                        OnRowEditing="gvHospiceAttendingPhysician_RowEditing" OnRowUpdating="gvHospiceAttendingPhysician_RowUpdating" OnRowCancelingEdit="gvHospiceAttendingPhysician_RowCancelingEdit"
                        OnRowDeleting="gvHospiceAttendingPhysician_RowDeleting" OnRowDataBound="gvHospiceAttendingPhysician_RowDataBound" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>
                            <asp:TemplateField HeaderText="Benefit Line No" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenefitLineNo" runat="server" Text='<%# Bind("BenPeriod") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="hdnBenefitLineNo" runat="server" Value='<%#Bind("BenPeriod") %>'/>
                                    <asp:DropDownList ID="eddlBenefitLineNo" runat="server" onchange="javascript:BenefitLineNumberChange(this,'Edit');"></asp:DropDownList>
                                     <span style="color:red; display:none"><br />Benefit Line No is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:HiddenField ID="hdnBenefitLineNo" runat="server"/>
                                    <asp:DropDownList ID="fddlBenefitLineNo" runat="server" onchange="javascript:BenefitLineNumberChange(this,'Add');"></asp:DropDownList>
                                     <span style="color:red; display:none"><br />Benefit Line No is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Benefit Period Type" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblPhySegmentBenefitType" runat="server" ></asp:Label>
                                </ItemTemplate>
                                 <EditItemTemplate>
                                   <asp:Label ID="elblSegmentBenefitType" runat="server" ></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                  <asp:Label ID="flblSegmentBenefitType" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Benefit Period" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblPhyBenefitPeriod" runat="server"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                   <asp:Label ID="elblDateBenefitPeriod" runat="server" ></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                  <asp:Label ID="flblDateBenefitPeriod" runat="server"  ></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="NPI" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblNPI" runat="server" Text='<%# Bind("PhyNPI") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtNPI" runat="server" Text='<%# Bind("PhyNPI") %>'></asp:TextBox>
                                    <span style="color:red; display:none"> <br />NPI is required <br /></span>
                                     <asp:LinkButton ID="lnkSearch1" runat="server" Text="Search" OnClientClick="SearchNpiClickEvent('Update','gvHospiceAttendingPhysician','modelProviderSearch1')" />
                                    <ajax:ModalPopupExtender ID="DiaProviderSearch" runat="server" BehaviorID="modelProviderSearch1"
                                        PopupControlID="pnlProviderSearch" TargetControlID="lnkSearch1"
                                        BackgroundCssClass="modalBackground" CancelControlID="btnCloseNpiSearch" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtNPI" runat="server"></asp:TextBox>
                                     <span style="color:red; display:none"> <br />NPI is required <br /></span>
                                    <asp:LinkButton ID="lnkSearch2" runat="server" Text="Search" OnClientClick="SearchNpiClickEvent('Add','gvHospiceAttendingPhysician','modelProviderSearch2')" />
                                    <ajax:ModalPopupExtender ID="DiaProviderSearch" runat="server" BehaviorID="modelProviderSearch2"
                                        PopupControlID="pnlProviderSearch" TargetControlID="lnkSearch2"
                                        BackgroundCssClass="modalBackground" CancelControlID="btnCloseNpiSearch" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Oral Certification Date" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblOralCertificationDate" runat="server" Text='<%# Bind("PhyOralCertDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtOralCertificationDate" runat="server" autocomplete="off" Text='<%# Bind("PhyOralCertDate", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                    <ajax:CalendarExtender ID="cleOralDate" TargetControlID="etxtOralCertificationDate" runat="server" />
                                     <span style="color:red; display:none"><br />Oral Certification Date is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtOralCertificationDate" autocomplete="off" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="clOralDate" TargetControlID="ftxtOralCertificationDate" runat="server" />
                                    <span style="color:red; display:none"><br />Oral Certification Date is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Written Certification Date" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblWrittenCertificationDate" runat="server" Text='<%# Bind("PhyWritCertDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtWrittenCertificationDate" autocomplete="off" runat="server" Text='<%# Bind("PhyWritCertDate", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                    <ajax:CalendarExtender ID="cleWrittenCertDate" TargetControlID="etxtWrittenCertificationDate" runat="server" />
                                    <span style="color:red; display:none"><br />Written Certification Date is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtWrittenCertificationDate" autocomplete="off" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="clWrittenCertDate" TargetControlID="ftxtWrittenCertificationDate" runat="server" />
                                    <span style="color:red; display:none"><br />Written Certification Date is required</span>
                                </FooterTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CssClass="button" CausesValidation="false" />
                                    &nbsp;
                <asp:Button ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" Visible='<%# Convert.ToBoolean(Eval("IsFromInquiry")) == true ? false : true %>'
                    CssClass="button" OnClientClick='return confirm("Are you sure you want to delete this record?");' CausesValidation="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button ID="btnUpdate" Text="Update" runat="server" CommandName="Update" CssClass="button" CausesValidation="true" />
                                    &nbsp;
                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" CssClass="button" CausesValidation="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="ftnAdd" runat="server" Text="Add New" OnClick="AttendingfbtnAdd_Click" CssClass="button" Style="width: auto !important;"
                                     CausesValidation="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>
                <div style="text-align: center">
                    <span id="AttendingPhyErrorMessage" runat="server" style="color: red;"></span>
                </div>
            </Content>

        </ajax:AccordionPane>
    </Panes>

</ajax:accordion>

<ajax:accordion id="AccordionHospiceIDGPhysician" runat="Server" selectedindex="0" enableviewstate="false"
    headercssclass="accordionHeader" headerselectedcssclass="accordionHeaderSelected" contentcssclass="accordionContent"
    autosize="None" fadetransitions="true" transitionduration="250" framespersecond="40" requireopenedpane="false"
    suppressheaderpostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionPaneHospiceIDGPhysician" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblHospiceIDGPhysician" class="expandcollapse" runat="server" Text="- * HOSPICE IDG PHYSICIAN INFORMATION"></asp:Label>
            </Header>
            <Content>
               
                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                   <%-- <asp:UpdatePanel ID="panel1"  runat="server" UpdateMode="Always" >
                        <ContentTemplate>--%>
                            <asp:GridView ID="gvHospiceIDGPhysician" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                                HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true" 
                                CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvHospiceIDGPhysician_PageIndexChanging"
                                OnRowEditing="gvHospiceIDGPhysician_RowEditing" OnRowUpdating="gvHospiceIDGPhysician_RowUpdating" OnRowCancelingEdit="gvHospiceIDGPhysician_RowCancelingEdit"
                                OnRowDeleting="gvHospiceIDGPhysician_RowDeleting" OnRowDataBound="gvHospiceIDGPhysician_RowDataBound" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                                <Columns>
                                    <asp:TemplateField HeaderText="Benefit Line No" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenefitLineNo" runat="server" Text='<%# Bind("BenPeriod") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="hdnBenefitLineNo" runat="server" Value='<%#Bind("BenPeriod") %>'/>
                                    <asp:DropDownList ID="eddlBenefitLineNo" runat="server" onchange="javascript:BenefitLineNumberChange(this,'Edit');"></asp:DropDownList>
                                     <span style="color:red; display:none"><br />Benefit Line No is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:HiddenField ID="hdnBenefitLineNo" runat="server"/>
                                    <asp:DropDownList ID="fddlBenefitLineNo" runat="server" onchange="javascript:BenefitLineNumberChange(this, 'Add');"></asp:DropDownList>
                                     <span style="color:red; display:none"><br />Benefit Line No is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Benefit Period Type" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblIDGBenefitPeriodType" runat="server" ></asp:Label>
                                </ItemTemplate>
                                 <EditItemTemplate>
                                   <asp:Label ID="elblSegmentBenefitType" runat="server" ></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                  <asp:Label ID="flblSegmentBenefitType" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Benefit Period" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblIDGBenefitPeriod" runat="server"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                   <asp:Label ID="elblDateBenefitPeriod" runat="server" ></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                  <asp:Label ID="flblDateBenefitPeriod" runat="server"  ></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                                    <asp:TemplateField HeaderText="NPI" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNPI" runat="server" Text='<%# Bind("IDGPhyNPI") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="etxtNPI" runat="server" Text='<%# Bind("IDGPhyNPI") %>'></asp:TextBox>
                                            <span style="color:red; display:none"> <br />NPI is required <br /></span>
                                            <asp:LinkButton ID="lnkSearch3" runat="server" Text="Search" OnClientClick="SearchNpiClickEvent('Update','gvHospiceIDGPhysician','modelProviderSearch3')" />
                                    <ajax:ModalPopupExtender ID="DiaProviderSearch" runat="server" BehaviorID="modelProviderSearch3"
                                        PopupControlID="pnlProviderSearch" TargetControlID="lnkSearch3"
                                        BackgroundCssClass="modalBackground" CancelControlID="btnCloseNpiSearch" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="ftxtNPI" runat="server"></asp:TextBox>
                                            <span style="color:red; display:none"> <br />NPI is required <br /></span>
                                             <asp:LinkButton ID="lnkSearch4" runat="server" Text="Search" OnClientClick="SearchNpiClickEvent('Add','gvHospiceIDGPhysician','modelProviderSearch4')" />
                                    <ajax:ModalPopupExtender ID="DiaProviderSearch" runat="server" BehaviorID="modelProviderSearch4"
                                        PopupControlID="pnlProviderSearch" TargetControlID="lnkSearch4"
                                        BackgroundCssClass="modalBackground" CancelControlID="btnCloseNpiSearch" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Oral Certification Date" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOralCertificationDate" runat="server" Text='<%# Bind("IDGPhyOralCertDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="etxtOralCertificationDate" autocomplete="off" runat="server" Text='<%# Bind("IDGPhyOralCertDate", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                            <ajax:CalendarExtender ID="cleOralCertfDate" TargetControlID="etxtOralCertificationDate" runat="server" EnabledOnClient="true" />
                                            <span style="color: red; display: none">
                                                <br />
                                                Oral Certification Date is required</span>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="ftxtOralCertificationDate" autocomplete="off" runat="server"></asp:TextBox>
                                            <ajax:CalendarExtender ID="clOralCertfDate" TargetControlID="ftxtOralCertificationDate" runat="server" EnabledOnClient="true" />
                                            <span style="color: red; display: none">
                                                <br />
                                                Oral Certification Date is required</span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Written Certification Date" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWrittenCertificationDate" runat="server" Text='<%# Bind("IDGPhyWritCertDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="etxtWrittenCertificationDate" autocomplete="off" runat="server" Text='<%# Bind("IDGPhyWritCertDate", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                            <ajax:CalendarExtender ID="cleWrittenDate" TargetControlID="etxtWrittenCertificationDate" runat="server" EnabledOnClient="true" />
                                            <span style="color: red; display: none">
                                                <br />
                                                Written Certification Date is required</span>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="ftxtWrittenCertificationDate" autocomplete="off" runat="server"></asp:TextBox>
                                            <ajax:CalendarExtender ID="clWrittenDate" TargetControlID="ftxtWrittenCertificationDate" runat="server" EnabledOnClient="true" />
                                            <span style="color: red; display: none">
                                                <br />
                                                Written Certification Date is required</span>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CssClass="button" CausesValidation="false" />
                                            &nbsp;
                <asp:Button ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" Visible='<%# Convert.ToBoolean(Eval("IsFromInquiry")) == true ? false : true %>'
                    CssClass="button" OnClientClick='return confirm("Are you sure you want to delete this record?");' CausesValidation="false" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnUpdate" Text="Update" runat="server" CommandName="Update" CssClass="button" CausesValidation="true" />
                                            &nbsp;
                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" CssClass="button" CausesValidation="false" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Button ID="ftnAdd" runat="server"   Text="Add New" OnClick="IDGfbtnAdd_Click" CssClass="button" Style="width: auto !important;"  />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="gridViewHeader" />
                                <AlternatingRowStyle CssClass="gridViewAltRow" />
                                <RowStyle CssClass="gridViewRow" />
                                <FooterStyle CssClass="gridViewFooter" />
                            </asp:GridView>
                       <%-- </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ftnAdd"/>
                        </Triggers>
                    </asp:UpdatePanel>--%>
                </div>
                <div style="text-align: center">
                    <span id="IDGPhyErrorMessage" runat="server" style="color: red;"></span>
                </div>
                 
            </Content>

        </ajax:AccordionPane>
    </Panes>

</ajax:accordion>
  <asp:HiddenField ID="hdSelectedSearchNpiGridId" runat="server" ClientIDMode="Static" />
                 <asp:HiddenField ID="hdSelectedSearchNpiButtonAction" runat="server" ClientIDMode="Static" />
                 <asp:HiddenField ID="hdSelectedSearchNpiBehaviorId" runat="server" ClientIDMode="Static" />
<asp:Panel ID="pnlProviderSearch" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
                    <asp:Panel ID="pnlCE1" runat="server">
                        <asp:Button runat="server" ID="btnCloseNpiSearch" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;" CausesValidation="false" OnClientClick="javascript: setBlankNPISearchValue();" />
                        <div style="text-align: left; padding: 42px; overflow-x: auto; overflow-y: auto" class="container-fluid">
                            <div class="row" style="text-align: center;">
                                <div class="col-sm-6 col-md-4 col-lg-3 ">
                                    <span class="ohio-field-label"><b>NPI</b>
                                        <asp:TextBox ID="txtNPI" CssClass="ohio-field-input" runat="server" MaxLength="10">
                                        </asp:TextBox>
                                    </span>
                                    <asp:RegularExpressionValidator runat="server" ID="revtxtNPI" ControlToValidate="txtNPI" ErrorMessage="10 Digit number required" ValidationExpression="^\d{10}$" ForeColor="Red"></asp:RegularExpressionValidator>
                                </div>

                                <div class="col-sm-6 col-md-4 col-lg-3 ">
                                    <span class="ohio-field-label"><b>Medicaid ID</b>
                                        <asp:TextBox ID="txtMedicaidID" CssClass="ohio-field-input" runat="server" MaxLength="7">
                                        </asp:TextBox>
                                    </span>

                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-3 ">
                                    <span class="ohio-field-label"><b>Business/Last Name</b>
                                        <asp:TextBox ID="txtBusinessLastName" CssClass="ohio-field-input" runat="server" MaxLength="70">
                                        </asp:TextBox>
                                    </span>

                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-3 ">
                                    <span class="ohio-field-label"><b>First Name</b>
                                        <asp:TextBox ID="txtFirstName" CssClass="ohio-field-input" runat="server" MaxLength="35">
                                        </asp:TextBox>
                                    </span>

                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-3 ">
                                    <input type="button" id="btnSearch" onclick="SearchProviderNpiinfo();" value="Search" class="buttonBoxFocus" style="background-color: darkslateblue !important" />
                                </div>
                            </div>
                            <div style="text-align: center">
                                <span id="HospiceProviderSearchErrorMessage" runat="server" style="color: red;"></span>
                            </div>
                            <div style="width: 100%; height: 400px; overflow: scroll">
                                <mms:SortablePagingGridView
                                    ID="gvHospiceProviderNPISearch"
                                    runat="server"
                                    AutoGenerateColumns="False"
                                    CssClass="gridViewSmallFont" Width="100%"
                                    AllowSorting="true"
                                    ShowHeaderWhenEmpty="true"
                                    EmptyDataText=" "
                                    RowStyle-VerticalAlign="Top"
                                    AlternatingRowStyle-BackColor="White" GridLines="Horizontal"
                                    AllowPaging="True"
                                    PageSize="15"
                                    GridViewSortColumn="NPI" GridViewSortDirection="Ascending">
                                    <Columns>
                                        <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
                                        <asp:BoundField DataField="MEDICAID_ID" HeaderText="Medicaid ID" SortExpression="MedicaidID" />
                                        <asp:BoundField DataField="LAST_OR_BUSINESS_NAME" HeaderText="Business/Last Name" SortExpression="BusinessLastName" />
                                        <asp:BoundField DataField="FIRST_NAME" HeaderText="First Name" SortExpression="FirstName" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <input type="button" id="lnkSelect" onclick="FillSelectedProviderNPIInfo(this); setBlankNPISearchValue();" value="Select" class="buttonBoxFocus" style="background-color: darkslateblue !important" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </mms:SortablePagingGridView>
                            </div>
                        </div>
                    </asp:Panel>
                </asp:Panel>

 <uc6:HospiceOtherPayerSpan ID="uc6HospiceOtherPayerSpan" runat="server" Visible="true" EnableViewState="true" />
        <uc7:HospiceEpisodeofCare ID="uc7HospiceEpisodeofCare" runat="server" Visible="true" EnableViewState="true" />
        <uc8:HospiceTerminalIllnessDiagnosis ID="uc8HospiceTerminalIllnessDiagnosis" runat="server" Visible="true" EnableViewState="true" />

        <uc9:HospiceProviderServiceSpan ID="uc9HospiceProviderServiceSpan" runat="server" Visible="true" EnableViewState="true" />
        <uc10:HospiceHLTCFProviderService ID="uc10HospiceHLTCFProviderService" runat="server" Visible="true" EnableViewState="true" />
        <uc11:HospiceAttachment ID="uc11HospiceAttachment" runat="server" Visible="true" EnableViewState="true" />