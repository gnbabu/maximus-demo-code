$(function () {
    HideButtonsByActionType($("[id*=ddlHospiceApplicationType]").val());
    if ($("[id*=ddlHospiceApplicationType]").val() === "MAINT") {
        $("[id*=gvHospiceBenefitPeriod] tbody tr").each(function () {
            if (!this.rowIndex) return;
            var lblReasonForUpdate = $(this).find("td span[id*=lblReasonForUpdate]").html();
            if (lblReasonForUpdate === "Death"
                || lblReasonForUpdate === "Individual no longer meets the enrollment criteria"
                || lblReasonForUpdate === "Individual is no longer terminally ill"
                || lblReasonForUpdate === "Individual entered a non-contracted facility"
                || lblReasonForUpdate === "Individual moved out the service area"
                || lblReasonForUpdate === "Individual revoked the Medicaid hospice benefit") {

                $("[id*=txtDisenrollment]").prop('disabled', false);
            }
        });
    }
    var lblMessage = $("[id*=lblMessage]").text();
    if (lblMessage.indexOf("Hospice application is successfully submitted") != -1) {
        HideButtonsByActionType("", true);
    }
    $("[id*=ddlHospiceApplicationType]").change(function () {
        //if (confirm("Changing the section will result to loose unsaved data?")) {

        //}
        var selectedValue = $(this).val();
        HideButtonsByActionType(selectedValue);

    });
    $("[id*=btnHospiceSubmit]").click(function () {
        var isvalid = true;
        var strErrorMessage = "";
        if ($("[id*=txtElectionDate]").val() === "") {
            strErrorMessage = strErrorMessage + ' ' + '<a  onclick="SetBackgroundColor(\'txtElectionDate\')" Style="color: red;">Enrollment date is required.</a> <br/>';
            isvalid = false;
        }
        else {
            if (!isDate($("[id*=txtElectionDate]").val())) {
                strErrorMessage = strErrorMessage + ' ' + '<a  onclick="SetBackgroundColor(\'txtElectionDate\')" Style="color: red;">Enrollment date is not a valid date.</a> <br/>';
                isvalid = false;
            }
        }
        var actionType = $("[id*=ddlHospiceApplicationType]").val();
        //var isReportedDeath = false;
        //$("[id*=gvHospiceBenefitPeriod] tbody tr").each(function () {
        //    if (!this.rowIndex) return;
        //    var lblReasonForUpdate = $(this).find("td span[id*=lblReasonForUpdate]").html();
        //    if (lblReasonForUpdate !== null &&
        //        lblReasonForUpdate !== undefined) {
        //        if (lblReasonForUpdate === "Death") {
        //            isReportedDeath = true;
        //        }
        //    }
        //});

        var isDisenrollmentRequired = false;
        if ($("[id*=txtDisenrollment]").val() === "" && actionType === "MAINT") {
            $("[id*=gvHospiceBenefitPeriod] tbody tr").each(function () {
                if (!this.rowIndex) return;
                var lblReasonForUpdate = $(this).find("td span[id*=lblReasonForUpdate]").html();
                if (lblReasonForUpdate === "Death"
                    || lblReasonForUpdate === "Individual no longer meets the enrollment criteria"
                    || lblReasonForUpdate === "Individual is no longer terminally ill"
                    || lblReasonForUpdate === "Individual entered a non-contracted facility"
                    || lblReasonForUpdate === "Individual moved out the service area"
                    || lblReasonForUpdate === "Individual revoked the Medicaid hospice benefit") {

                    isDisenrollmentRequired = true;
                }
            });
        }
        if (actionType === "REVOC" || actionType === "BFTRM" || isDisenrollmentRequired === true) {
            if ($("[id*=txtDisenrollment]").val() === "") {
                strErrorMessage = strErrorMessage + ' ' + '<a  onclick="SetBackgroundColor(\'txtDisenrollment\')" Style="color: red;">Disenrollment date is required.</a> <br/>';
                isvalid = false;
            }
        }
        if ($("[id*=txtDisenrollment]").val() != "") {
            if (Date.parse($("[id*=txtDisenrollment]").val()) < Date.parse($("[id*=txtElectionDate]").val())) {
                strErrorMessage = strErrorMessage + ' ' + '<a  onclick="SetBackgroundColor(\'txtDisenrollment\')" Style="color: red;">Disenrollment date cannot be smaller than election date.</a> <br/>';
                isvalid = false;
            }
        }
        var hdBenefitPeriodEffDate = $("[id*=HdBenefitPeriodEffDate]").val();
        if (Date.parse($("[id*=txtElectionDate]").val()) > Date.parse(hdBenefitPeriodEffDate)) {
            strErrorMessage = strErrorMessage + ' ' + '<a  onclick="SetBackgroundColor(\'txtElectionDate\')" Style="color: red;">Election date is greater than effective date of First 90-day period.</a> <br/>';
            isvalid = false;
        }

        var benefitPeriodGridRowsCount = 0;
        for (var i = 0; i <= $("[id*=gvHospiceBenefitPeriod] tbody tr").length - 1; i++) {
            if ($("[id*=gvHospiceBenefitPeriod] tbody tr")[i].getElementsByTagName('td').length > 1) {
                benefitPeriodGridRowsCount++;
            }
        }
        if (benefitPeriodGridRowsCount <= 1) {
            strErrorMessage = strErrorMessage + ' ' + '<a  onclick="SetBackgroundColor(\'gvHospiceBenefitPeriod\')" Style="color: red;">Hospice BenefitPeriod is required information panel.</a> <br/>';
            isvalid = false;
        }
        var countyGridRowsCount = 0;
        for (var i = 0; i <= $("[id*=gvRecipentServiceLocation] tbody tr").length - 1; i++) {
            if ($("[id*=gvRecipentServiceLocation] tbody tr")[i].getElementsByTagName('td').length > 1) {
                countyGridRowsCount++;
            }
        }
        if (countyGridRowsCount <= 1) {
            strErrorMessage = strErrorMessage + ' ' + '<a  onclick="SetBackgroundColor(\'gvRecipentServiceLocation\')" Style="color: red;">County and State of Recipient’s Hospice Service Location is required information panel.</a> <br/>';
            isvalid = false;
        }
        var gvHospiceIDGPhysicianGridRowsCount = 0;
        for (var i = 0; i <= $("[id*=gvHospiceIDGPhysician] tbody tr").length - 1; i++) {
            if ($("[id*=gvHospiceIDGPhysician] tbody tr")[i].getElementsByTagName('td').length > 1) {
                gvHospiceIDGPhysicianGridRowsCount++;
            }
        }
        if (gvHospiceIDGPhysicianGridRowsCount <= 1) {
            strErrorMessage = strErrorMessage + ' ' + '<a  onclick="SetBackgroundColor(\'gvHospiceIDGPhysician\')" Style="color: red;">Hospice IDG Physician is required information panel.</a> <br/>';
            isvalid = false;
        }
        var gvHospiceAttendingPhysicianGridRowsCount = 0;
        for (var i = 0; i <= $("[id*=gvHospiceAttendingPhysician] tbody tr").length - 1; i++) {
            if ($("[id*=gvHospiceAttendingPhysician] tbody tr")[i].getElementsByTagName('td').length > 1) {
                gvHospiceAttendingPhysicianGridRowsCount++;
            }
        }
        if (gvHospiceAttendingPhysicianGridRowsCount <= 1) {
            strErrorMessage = strErrorMessage + ' ' + '<a  onclick="SetBackgroundColor(\'gvHospiceAttendingPhysician\')" Style="color: red;">Hospice Attending Physician is required information panel.</a> <br/>';
            isvalid = false;
        }
        //debugger;
        var gvHospiceTerminalIllnessDiagnosisGridRowsCount = 0;
        for (var i = 0; i <= $("[id*=gvHospiceTerminalIllnessDiagnosis] tbody tr").length - 1; i++) {
            if ($("[id*=gvHospiceTerminalIllnessDiagnosis] tbody tr")[i].getElementsByTagName('td').length > 1) {
                gvHospiceTerminalIllnessDiagnosisGridRowsCount++;
            }
        }
        if (gvHospiceTerminalIllnessDiagnosisGridRowsCount <= 2) {
            strErrorMessage = strErrorMessage + ' ' + '<a  onclick="SetBackgroundColor(\'gvHospiceTerminalIllnessDiagnosis\')" Style="color: red;">Hospice Terminal Illness Diagnosis is required information panel.</a> <br/>';
            isvalid = false;
        }
        var gvHospiceProviderServiceSpanGridRowsCount = 0;
        for (var i = 0; i <= $("[id*=gvHospiceProviderServiceSpan] tbody tr").length - 1; i++) {
            if ($("[id*=gvHospiceProviderServiceSpan] tbody tr")[i].getElementsByTagName('td').length > 1) {
                gvHospiceProviderServiceSpanGridRowsCount++;
            }
        }
        if (gvHospiceProviderServiceSpanGridRowsCount <= 1) {
            strErrorMessage = strErrorMessage + ' ' + '<a  onclick="SetBackgroundColor(\'gvHospiceProviderServiceSpan\')" Style="color: red;">Hospice Provider Span is required information panel.</a> <br/>';
            isvalid = false;
        }

        if (isvalid === true) {
            var errorMessage = CheckDateswithInBenefitPeriod("gvRecipentServiceLocation", "LocationErrorMessage", "COUNTY AND STATE OF RECIPIENT’S HOSPICE SERVICE LOCATION");
            if (errorMessage != "") {
                strErrorMessage = strErrorMessage + ' ' + errorMessage;
                isvalid = false;
            }
            errorMessage = CheckDateswithInBenefitPeriod("gvHospiceTerminalIllnessDiagnosis", "TerminalErrorMessage", "HOSPICE TERMINAL ILLNESS DIAGNOSIS");
            if (errorMessage != "") {
                strErrorMessage = strErrorMessage + ' ' + errorMessage;
                isvalid = false;
            }
            errorMessage = CheckDateswithInBenefitPeriod("gvHospiceHLTCFProviderService", "HltcPhyErrorMessage", "HLTCF PROVIDER SERVICE");
            if (errorMessage != "") {
                strErrorMessage = strErrorMessage + ' ' + errorMessage;
                isvalid = false;
            }
        }

        if (strErrorMessage != "" && isvalid === false) {
            $("[id*=divPortalErrors]").css("background-color", "#999");
            $("[id*=divPortalErrors]").html(strErrorMessage);
        }
        return isvalid;
    });

});
function SetBackgroundColor(Id) {
    $("[id*=" + Id + "]").css("background-color", "yellow");
    $("[id*=" + Id + "]").focus();
}
function HideButtonsByActionType(selectedValue, isHospiceSubmited = false) {

    var status = $("[id*=hdnHospiceStatus]").val();
    var isHospiceMaintenanceSubRole = $("[id*=hdHospiceMaintenanceSubRole]").val();
    var isDateoverlapped = $("[id*=hdIsDateOverlapped]").val();
    var actionType = $("[id*=ddlHospiceApplicationType]");
    var btnSubmit = $("[id*=btnHospiceSubmit]");
    var hdIsAllownewbenPeriod = $("[id*=hdIsAllownewbenPeriod]").val();
    var gridControles = ["gvRecipentServiceLocation", "gvHospiceBenefitPeriod", "gvHospiceIDGPhysician", "gvHospiceAttendingPhysician", "gvHospiceOtherPayerSpan", "gvHospiceEpisodeofCare", "gvHospiceTerminalIllnessDiagnosis", "gvHospiceProviderServiceSpan", "gvHospiceHLTCFProviderService", "gvHospiceAttachment"];
    var reasonForUpdate = $("[id*=gvHospiceBenefitPeriod] [id*=ftnAdd]").closest("tr").find("[id*=fddlReasonForUpdate]");
    var gvHospiceProviderServiceSpanGridRowsCount = 0;
    for (var i = 0; i <= $("[id*=gvHospiceProviderServiceSpan] tbody tr").length - 1; i++) {
        if ($("[id*=gvHospiceProviderServiceSpan] tbody tr")[i].getElementsByTagName('td').length > 1) {
            gvHospiceProviderServiceSpanGridRowsCount++;
        }
    }
    $.each(gridControles, function (index, Id) {
        var btnEdit = $("[id*=" + Id + "]").closest('tr').find('td input[id*="btnEdit"]');
        var btnDelete = $("[id*=" + Id + "]").closest('tr').find('td input[id*="btnDelete"]');
        var hdnIsDifferentProdvider = $("[id*=" + Id + "]").closest('tr').find('td input[id*="hdnIsDifferentProdvider"]').val();
        var footerAdd = $("[id*=" + Id + "]").find('tr:last');
        btnEdit.show();
        btnDelete.show();
        footerAdd.show();
        if (isHospiceMaintenanceSubRole === "false") {
            btnEdit.hide();
            btnDelete.hide();
            footerAdd.hide();
        }
        else {
            if (selectedValue === "REVOC") {
                btnEdit.hide();
                btnDelete.hide();
                footerAdd.hide();
            }
            else {
                //Maintain hospice record
                if (selectedValue !== "MAINT") {
                    btnEdit.hide();
                }
                if (selectedValue === "NEWEN") {
                    btnEdit.show();
                }
                if (selectedValue === "MAINT") {
                    reasonForUpdate.prop('disabled', false);
                }
                if (selectedValue === "CLSPR" || selectedValue === "BFTRM") {
                    footerAdd.hide();
                    btnDelete.hide();
                }
            }


            if (status === "D" && selectedValue != "NEWEN") {
                btnEdit.hide();
                btnDelete.hide();
                footerAdd.hide();
            }
            if (status === "D" && selectedValue === "NEWEN") {
                btnEdit.show();
                btnDelete.show();
                footerAdd.show();
            }
            //if (selectedValue === "CHGPR" && Id != "gvHospiceBenefitPeriod" && gvHospiceProviderServiceSpanGridRowsCount <= 1) {
            //    btnEdit.hide();
            //    btnDelete.hide();
            //    footerAdd.hide();
            //}
            if (selectedValue === "CHGPR" && Id != "gvHospiceBenefitPeriod") {
                btnEdit.show();
                btnDelete.show();
                footerAdd.show();
            }
            if (selectedValue === "CHGPR" && Id === "gvHospiceProviderServiceSpan") {
                btnEdit.show();
                btnDelete.show();
                footerAdd.show();
            }
            if (selectedValue === "CLSPR" && Id === "gvHospiceProviderServiceSpan") {
                btnEdit.show();
            }

            if (Id === "gvHospiceBenefitPeriod") {
                if (status === "C" && selectedValue === "MAINT") {
                    footerAdd.hide();
                }
                else if (hdIsAllownewbenPeriod === "False" || hdIsAllownewbenPeriod === "false") {
                    footerAdd.hide();
                }
                if (hdnIsDifferentProdvider === "true" || hdnIsDifferentProdvider === "True") {
                    btnEdit.hide();
                }
                if (selectedValue === "CHGPR") {
                    footerAdd.hide();
                }
            }
            if (status === "C") {
                btnEdit.hide();
                btnDelete.hide();
                footerAdd.hide();
                actionType.prop('disabled', true);
                btnSubmit.prop('disabled', true);
            }
            if (selectedValue === "") {
                btnEdit.hide();
                btnDelete.hide();
                footerAdd.hide();
            }
        }
    });
    if (isHospiceSubmited || isHospiceMaintenanceSubRole === "false") {
        actionType.prop('disabled', true);
        btnSubmit.prop('disabled', true);
    }
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
function RequiredFieldsValidations(row, requiredControles, errorMessageId) {
    debugger;
    var isValid = true;
    $("[id*=" + errorMessageId + "]").text("");
    $.each(requiredControles, function (index, Id) {
        debugger;
        var label = row.find("[id*=" + Id + "]").next("SPAN");
        row.find("[id*=" + Id + "]").removeClass("focus");
        label.hide();
        if ($.trim(row.find("[id*=" + Id + "]").val()) === "") {
            row.find("[id*=" + Id + "]").addClass("focus");
            label.show();
            isValid = false;
        }
    });

    return isValid;
}
function DateValidations(effectiveDate, endDate, errorMessageId) {
    if (Date.parse(effectiveDate) > Date.parse(endDate)) {
        $("[id*=" + errorMessageId + "]").text("Effective Date Should be less than or equal to End date.");
        return false;
    }
    if (Date.parse(endDate) < Date.parse(effectiveDate)) {
        $("[id*=" + errorMessageId + "]").text("End Date Should be greater than or equal to Effectvie Data.");
        return false;
    }
    return true;
}
function DateValidationsOverlap(effectiveDate, endDate, gridId, errorMessageId) {
    var EffectiveDates = [];
    var EndDates = [];
    // Run function for each tbody tr for overrlap date validations
    $("[id*=" + gridId + "] tbody tr").each(function () {
        if (!this.rowIndex) return;
        if ($(this).find("td span[id*=lblEffectiveDate]").html() !== null && $(this).find("td span[id*=lblEffectiveDate]").html() !== undefined) {
            EffectiveDates.push(new Date($(this).find("td span[id*=lblEffectiveDate]").html()));
        }
        if ($(this).find("td span[id*=lblEndDate]").html() !== null && $(this).find("td span[id*=lblEndDate]").html() !== undefined) {
            EndDates.push(new Date($(this).find("td span[id*=lblEndDate]").html()));
        }
    });
    var minEffDate = new Date(Math.min.apply(null, EffectiveDates));
    var maxEndDate = new Date(Math.max.apply(null, EndDates));
    var effDate = new Date(effectiveDate);
    var endDate = new Date(endDate);
    if (effDate >= minEffDate && effDate <= maxEndDate || endDate >= minEffDate && endDate <= maxEndDate) {
        $("[id*=" + errorMessageId + "]").text("Effective date or End date  should not fall in between previously entered effective end date.");
        return false;
    }
    return true;
}
function CheckDaysGap(effectiveDate, endDate, gridId, errorMessageId, benfitLineNumber, errorMessage) {
    var EffectiveDates = [];
    var EndDates = [];
    var benCount = 0;
    // Run function for each tbody tr for overrlap date validations
    $("[id*=" + gridId + "] tbody tr").each(function () {
        if (!this.rowIndex) return;
        var benLineno = $(this).find("td span[id*=lblBenefitLineNo]").html();
        if (benLineno === benfitLineNumber) {
            if ($(this).find("td span[id*=lblEffectiveDate]").html() !== null && $(this).find("td span[id*=lblEffectiveDate]").html() !== undefined) {
                EffectiveDates.push(new Date($(this).find("td span[id*=lblEffectiveDate]").html()));
            }
            if ($(this).find("td span[id*=lblEndDate]").html() !== null && $(this).find("td span[id*=lblEndDate]").html() !== undefined) {
                EndDates.push(new Date($(this).find("td span[id*=lblEndDate]").html()));
            }
            benCount++;
        }
    });
    if (benCount > 0) {
        var maxEndDate = new Date(Math.max.apply(null, EndDates));
        var effDate = new Date(effectiveDate);
        var days = Math.round((effDate - maxEndDate) / (1000 * 60 * 60 * 24));
        if (days !== 1) {
            $("[id*=" + errorMessageId + "]").text(errorMessage);
            return false;
        }
    }
    return true;
}
function CompareWithBenefitPeriod(effectiveDate, endDate, benefitSegment, gridId, errorMessageId) {
    // Compare with BenefitPeriod Effdata & EndDate
    var effDate = new Date(effectiveDate);
    var endDate = new Date(endDate);
    var hdBenefitPeriodEffDate = new Date($("[id*=HdBenefitPeriodEffDate]").val());
    var hdBenefitPeriodEndDate = new Date($("[id*=HdBenefitPeriodEndDate]").val());
    //TBD
    //if (effDate < hdBenefitPeriodEffDate) {
    //    $("[id*=" + errorMessageId + "]").text("Effective date must match with benefit period effective date");
    //    return false;
    //}
    //if (endDate > hdBenefitPeriodEndDate) {
    //    $("[id*=" + errorMessageId + "]").text("End date must match with benefit period effective date");
    //    return false;
    //}
    //return BenefitSegment90DaysCheck(effectiveDate, endDate, benefitSegment, gridId, errorMessageId);

    return true;
}
function CompareWithBenefitPeriodLineNo(effectiveDate, endDate, errorMessageId, bEffDate, bEndDate, benPeriod) {
    // Compare with BenefitPeriod Effdata & EndDate
    var effDate = Date.parse(effectiveDate);
    var endDate = Date.parse(endDate);
    var hdBenefitPeriodEffDate = Date.parse(bEffDate);
    var hdBenefitPeriodEndDate = Date.parse(bEndDate);
    var actionType = $("[id*=ddlHospiceApplicationType]").val();

    if (actionType === "CHGPR") {
        var provEffDate;
        var provEndDate;
        // Run function for each tbody tr for overrlap date validations
        $("[id*=gvHospiceProviderServiceSpan] tbody tr").each(function () {
            if (!this.rowIndex) return;
            if ($(this).find("td span[id*=lblBenefitLineNo]").html() !== null && $(this).find("td span[id*=lblBenefitLineNo]").html() !== undefined) {
                if ($(this).find("td span[id*=lblBenefitLineNo]").html() === benPeriod) {
                    provEffDate = new Date($(this).find("td span[id*=lblEffectiveDate]").html());
                    provEndDate = new Date($(this).find("td span[id*=lblEndDate]").html());
                }
            }
        });

        if (effDate < provEffDate) {
            $("[id*=" + errorMessageId + "]").text("Effective date must match with provider service span effective date");
            return false;
        }
        if (endDate > provEndDate) {
            $("[id*=" + errorMessageId + "]").text("End date must match with provider service span end date");
            return false;
        }
    }
    else {
        if (effDate < hdBenefitPeriodEffDate) {
            $("[id*=" + errorMessageId + "]").text("Effective date must match with benefit period effective date");
            return false;
        }
        if (endDate > hdBenefitPeriodEndDate) {
            $("[id*=" + errorMessageId + "]").text("End date must match with benefit period end date");
            return false;
        }
    }

    return true;
}
function BenefitSegment90DaysCheck(effectiveDate, endDate, benefitSegment, gridId, errorMessageId) {
    // Compare with BenefitPeriod Effdata & EndDate
    var effDate = new Date(effectiveDate);
    var endDate = new Date(endDate);

    var First90Days = 0;
    var Second90Days = 0;
    $("[id*=" + gridId + "] tbody tr").each(function () {
        if (!this.rowIndex) return;
        var grdEffDate = $(this).find("td span[id*=lblEffectiveDate]").html();
        var grdEndDate = $(this).find("td span[id*=lblEndDate]").html();
        var grdBenefitSegmentIndicator = $(this).find("td span[id*=lblBenefitSegmentIndicator]").html();
        if (grdEffDate !== null && grdEndDate !== null && grdBenefitSegmentIndicator !== null &&
            grdEffDate !== undefined && grdEndDate !== undefined && grdBenefitSegmentIndicator !== undefined) {
            var days = Math.round((new Date(grdEndDate) - new Date(grdEffDate)) / (1000 * 60 * 60 * 24));
            if (grdBenefitSegmentIndicator === "First 90 days period") {
                First90Days = First90Days + days;
            }
            if (grdBenefitSegmentIndicator === "Second 90 days period") {
                Second90Days = Second90Days + days;
            }
        }
    });
    var enteredDay = Math.round((endDate - effDate) / (1000 * 60 * 60 * 24));
    if (benefitSegment === "First 90 days period") {
        First90Days = First90Days + enteredDay;
    }
    if (benefitSegment === "Second 90 days period") {
        Second90Days = Second90Days + enteredDay;
    }

    if (First90Days > 90 || Second90Days > 90) {
        $("[id*=" + errorMessageId + "]").text("Total Days of Effective Date & End Date should not greater than 90 days.");
        return false;
    }
    return true;
}
function CheckDateswithInBenefitPeriod(gridId, errorMessageId, customMessage) {
    var errorMessage = "";
    var data = [];
    $("[id*=" + gridId + "] tbody tr").each(function () {
        if (!this.rowIndex) return;
        var grdEffDate = $(this).find("td span[id*=lblEffectiveDate]").html();
        var grdEndDate = $(this).find("td span[id*=lblEndDate]").html();
        /* var benfitLinoValue = $(this).find("td span[id*=lblBenefitPeriod]").html();*/
        var benLineno = $(this).find("td span[id*=lblBenefitLineNo]").html();
        var benfitLinoValue = $(this).find("td input[id*=hdnBFPeriodDates]").val();
        if (benLineno !== null && benfitLinoValue !== null && grdEffDate !== null && grdEndDate !== null &&
            benLineno !== undefined && benfitLinoValue !== undefined && grdEffDate !== undefined && grdEndDate !== undefined) {
            var benefit = benfitLinoValue.split('-');
            var provEffDate;
            var provEndDate;
            // Run function for each tbody tr for overrlap date validations
            $("[id*=gvHospiceProviderServiceSpan] tbody tr").each(function () {
                if (!this.rowIndex) return;
                if ($(this).find("td span[id*=lblBenefitLineNo]").html() !== null && $(this).find("td span[id*=lblBenefitLineNo]").html() !== undefined) {
                    if ($(this).find("td span[id*=lblBenefitLineNo]").html() === benLineno) {
                        provEffDate = new Date($(this).find("td span[id*=lblEffectiveDate]").html());
                        provEndDate = new Date($(this).find("td span[id*=lblEndDate]").html());
                    }
                }
            });
            if (data.length == 0) {
                data.push({ BenefitLineNo: benLineno, EffDate: grdEffDate, EndDate: grdEndDate, BenEffDate: benefit[0], BenEndDate: benefit[1], ProvEffDate: provEffDate, ProvEndDate: provEndDate });
            }
            else {
                var isExisting = false;
                for (var i = 0; i < data.length; i++) {
                    if (data[i].BenefitLineNo == benLineno) {
                        data[i].EndDate = grdEndDate;
                        isExisting = true;
                    }
                }
                if (!isExisting) {
                    data.push({ BenefitLineNo: benLineno, EffDate: grdEffDate, EndDate: grdEndDate, BenEffDate: benefit[0], BenEndDate: benefit[1], ProvEffDate: provEffDate, ProvEndDate: provEndDate });
                }
            }
        }
    });
    var actionType = $("[id*=ddlHospiceApplicationType]").val();
    for (var i = 0; i < data.length; i++) {
        var benLineNo = data[i].BenefitLineNo;
        var BenefitPeriodEffDate = new Date(data[i].BenEffDate);
        var BenefitPeriodEndDate = new Date(data[i].BenEndDate);
        var gridPeriodEffDate = new Date(data[i].EffDate);
        var gridPeriodEndDate = new Date(data[i].EndDate);
        if (actionType === "CHGPR") {
            var provEffDate = data[i].ProvEffDate;
            var provEndDate = data[i].ProvEndDate;
            if (gridPeriodEffDate < provEffDate) {
                errorMessage = errorMessage + '' + "'" + customMessage + "' Effective date must match with provider service span effective date for Benefit Line No: " + benLineNo;
                errorMessage += " <br/>";
            }
            if (gridPeriodEndDate > provEndDate) {
                errorMessage = errorMessage + '' + "'" + customMessage + "' end date must match with provider service span end date  for Benefit Line No:" + benLineNo;
                errorMessage += " <br/>";
            }
            var days = Math.round((gridPeriodEndDate - gridPeriodEffDate) / (1000 * 60 * 60 * 24));
            var benDays = Math.round((provEndDate - provEffDate) / (1000 * 60 * 60 * 24));
            if (days + 1 !== benDays + 1) {
                errorMessage = errorMessage + '' + "'" + customMessage + "' must be assigned for every day within the provider service span period for Benefit Line No:" + benLineNo;
                errorMessage += " <br/>";
            }
        }
        else {
            if (gridPeriodEffDate < BenefitPeriodEffDate) {
                errorMessage = errorMessage + '' + "'" + customMessage + "' Effective date must match benefit period effective date for Benefit Line No: " + benLineNo;
                errorMessage += " <br/>";
            }
            if (gridPeriodEndDate > BenefitPeriodEndDate) {
                errorMessage = errorMessage + '' + "'" + customMessage + "' end date must match benefit period end date  for Benefit Line No:" + benLineNo;
                errorMessage += " <br/>";
            }
            var days = Math.round((gridPeriodEndDate - gridPeriodEffDate) / (1000 * 60 * 60 * 24));
            var benDays = Math.round((BenefitPeriodEndDate - BenefitPeriodEffDate) / (1000 * 60 * 60 * 24));
            if (days + 1 !== benDays + 1) {
                errorMessage = errorMessage + '' + "'" + customMessage + "' must be assigned for every day within the benefit period for Benefit Line No:" + benLineNo;
                errorMessage += " <br/>";
            }
        }
    }
    return errorMessage;
}

function DisplayPortalErrors(Errors) {
    var strErrors = "";
    var errorMgs = Errors.split(",");
    $.each(errorMgs, function (i, errorMsg) {
        var error = errorMsg.split("-");
        var errorCode = error[0];
        var errorDes = error[1];
        if (errorCode.startsWith("B2")) {
            if (errorCode === "B2001") {
                strErrors += + errorDes + "<br/>";
            }
            if (errorCode === "B2002") {
                strErrors += + errorDes + "<br/>";
            }
            if (errorCode === "B2003") {
                strErrors += '<a  onclick="SetBackgroundColor(\'ddlHospiceApplicationType\')" Style="color: red;">' + errorDes + '</a> <br/>';
            }
            if (errorCode === "B2004") {
                strErrors += '<a  onclick="SetBackgroundColor(\'gvRecipentServiceLocation\')" Style="color: red;">' + errorDes + '</a> <br/>';
            }
            if (errorCode === "B2005") {
                strErrors += '<a  onclick="SetBackgroundColor(\'gvRecipentServiceLocation\')" Style="color: red;">' + errorDes + '</a> <br/>';
            }
            if (errorCode === "B2006") {
                strErrors += '<a  onclick="SetBackgroundColor(\'gvHospiceHLTCFProviderService\')" Style="color: red;">' + errorDes + '</a> <br/>';
            }
            if (errorCode === "B2007") {
                strErrors += '<a  onclick="SetBackgroundColor(\'gvHospiceBenefitPeriod\')" Style="color: red;">' + errorDes + '</a> <br/>';
            }
            if (errorCode === "B2008") {
                strErrors += '<a  onclick="SetBackgroundColor(\'gvHospiceBenefitPeriod\')" Style="color: red;">' + errorDes + '</a> <br/>';
            }
            if (errorCode === "B2009") {
                strErrors += '<a  onclick="SetBackgroundColor(\'gvHospiceTerminalIllnessDiagnosis\')" Style="color: red;">' + errorDes + '</a> <br/>';
            }
            if (errorCode === "B2010") {
                strErrors += '<a  onclick="SetBackgroundColor(\'gvHospiceTerminalIllnessDiagnosis\')" Style="color: red;">' + errorDes + '</a> <br/>';
            }
            if (errorCode === "B2011") {
                strErrors += '<a  onclick="SetBackgroundColor(\'gvHospiceTerminalIllnessDiagnosis\')" Style="color: red;">' + errorDes + '</a> <br/>';
            }
            if (errorCode === "B2012") {
                strErrors += errorDes + "<br/>";
            }
            if (errorCode === "B2013") {
                strErrors += '<a  onclick="SetBackgroundColor(\'gvHospiceAttachment\')" Style="color: red;">' + errorDes + '</a> <br/>';
            }
            if (errorCode === "B2014") {
                strErrors += '<a  onclick="SetBackgroundColor(\'ddlHospiceApplicationType\')" Style="color: red;">' + errorDes + '</a> <br/>';
            }
            if (errorCode === "B2015") {
                strErrors += '<a  onclick="SetBackgroundColor(\'gvHospiceIDGPhysician\')" Style="color: red;">' + errorDes + '</a> <br/>';
            }
            if (errorCode === "B2016") {
                strErrors += '<a  onclick="SetBackgroundColor(\'gvHospiceAttendingPhysician\')" Style="color: red;">' + errorDes + '</a> <br/>';
            }
            if (errorCode === "B2017") {
                strErrors += errorDes + "<br/>";
            }
        }
        else {
            strErrors += errorDes + "<br/>";
        }

    });
    $("[id*=divPortalErrors]").css("background-color", "#999");
    $("[id*=divPortalErrors]").html(strErrors);
}