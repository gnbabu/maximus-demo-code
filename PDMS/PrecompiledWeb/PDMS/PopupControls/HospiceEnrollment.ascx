<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_HospiceEnrollment, App_Web_43eentok" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/HospiceRecipientInformation.ascx" TagPrefix="uc1" TagName="HospiceRecipientInformation" %>
<%@ Register Src="~/PopupControls/HospiceRecipientServiceLocation.ascx" TagPrefix="uc1" TagName="HospiceRecipientServiceLocation" %>
<%@ Register Src="~/PopupControls/HospiceEnrollmentAndDisenrollment.ascx" TagPrefix="uc2" TagName="HospiceEnrollmentAndDisenrollment" %>
<%@ Register Src="~/PopupControls/HospiceBenefitPeriod.ascx" TagPrefix="uc3" TagName="HospiceBenefitPeriod" %>
<%--<script src="../Scripts/HospiceCommon.js"></script>--%>
<script type="text/javascript">
    function BenefitLineNumberChange(ctrl, action) {
        var selectedItem = $(ctrl).find('option:selected').text();
        var hdCntrl = $(ctrl).closest('tr').find('td input[id*="hdnBenefitLineNo"]');
        hdCntrl.val(selectedItem);
        var selectedVal = $(ctrl).find('option:selected').val();
        var benefit = selectedVal.split(';');
        var flblSegmentBenefitType = (action === 'Edit') ? $(ctrl).closest('tr').find("[id*=elblSegmentBenefitType]") : $(ctrl).closest('tr').find(" [id*=flblSegmentBenefitType]");
        var flblDateBenefitPeriod = (action === 'Edit') ? $(ctrl).closest('tr').find("[id*=elblDateBenefitPeriod]") : $(ctrl).closest('tr').find(" [id*=flblDateBenefitPeriod]");
        if (benefit.length > 1) {
            flblSegmentBenefitType.html(benefit[0]);
            flblDateBenefitPeriod.html(benefit[1]);
        }   
        else {
            flblSegmentBenefitType.html("");
            flblDateBenefitPeriod.html("");
        }
    }


    $(function () {

        SetBenefitLineNumber();
        function SetBenefitLineNumber() {
            console.log('2');
            let ctrl = "[id*=fddlBenefitLineNo]";
            let selectedItem = $(ctrl).find('option:selected').text();
            console.log(selectedItem);
            if (selectedItem != null && selectedItem != '' && selectedItem != undefined) {
                let action = "Add";
                var hdCntrl = $(ctrl).closest('tr').find('td input[id*="hdnBenefitLineNo"]');
                hdCntrl.val(selectedItem);
                var selectedVal = $(ctrl).find('option:selected').val();
                var benefit = selectedVal.split(';');
                var flblSegmentBenefitType = (action === 'Edit') ? $(ctrl).closest('tr').find("[id*=elblSegmentBenefitType]") : $(ctrl).closest('tr').find(" [id*=flblSegmentBenefitType]");
                var flblDateBenefitPeriod = (action === 'Edit') ? $(ctrl).closest('tr').find("[id*=elblDateBenefitPeriod]") : $(ctrl).closest('tr').find(" [id*=flblDateBenefitPeriod]");
                if (benefit.length > 1) {
                    flblSegmentBenefitType.html(benefit[0]);
                    flblDateBenefitPeriod.html(benefit[1]);
                }
                else {
                    flblSegmentBenefitType.html("");
                    flblDateBenefitPeriod.html("");
                }
            }
        }
        // Stick the #nav to the top of the window
        var nav = $('#divPortalErrors');
        var navHomeY = nav?.offset()?.top;
        var isFixed = false;
        var $w = $(window);
        $w.scroll(function () {
            var scrollTop = $w.scrollTop();
            var shouldBeFixed = scrollTop > navHomeY;
            if (shouldBeFixed && !isFixed) {
                nav.css({
                    position: 'fixed',
                    top: 0,
                    left: nav.offset().left,
                    width: nav.width()
                });
                isFixed = true;
            }
            else if (!shouldBeFixed && isFixed) {
                nav.css({
                    position: 'static'
                });
                isFixed = false;
            }
        });


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
            var selectedValue = $(this).val();
            HideButtonsByActionType(selectedValue);

        });
    });


    function IsValidHospiceSubmit() {
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
        debugger;
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
        debugger;
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
    }

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
        var isValid = true;
        $("[id*=" + errorMessageId + "]").text("");
        $.each(requiredControles, function (index, Id) {            
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
        // Run function for each tbody tr for overrlap date validations

        var effDate = new Date(effectiveDate);
        var endDate = new Date(endDate);
        var isValid = true;

        $("[id*=" + gridId + "] tbody tr").each(function () {            
            if (!this.rowIndex) return;

            var rowEffectiveDate = new Date($(this).find("td span[id*=lblEffectiveDate]").html());
            var rowEndDate = new Date($(this).find("td span[id*=lblEndDate]").html());           

            if (effDate >= rowEffectiveDate && effDate <= rowEndDate || endDate >= rowEffectiveDate && endDate <= rowEndDate) {
                $("[id*=" + errorMessageId + "]").text("Effective date or End date should not fall in between previously entered effective end date.");
                isValid = false;
                return;
            }
        });
        return isValid;
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
            var provEffDate = data[i].ProvEffDate;
            var provEndDate = data[i].ProvEndDate;
            if (i == 0) {
                if (provEffDate > BenefitPeriodEffDate) {
                    var isChoppedMain = true;
                }
            }
            if (actionType === "CHGPR" || isChoppedMain) {
                if (gridPeriodEffDate < provEffDate) {
                    errorMessage = errorMessage + '' + "'" + customMessage + "' Effective date must match with provider service span effective date for Benefit Line No: " + benLineNo;
                    errorMessage += " <br/>";
                }
                if (gridPeriodEndDate > provEndDate) {
                    errorMessage = errorMessage + '' + "'" + customMessage + "' end date must match with provider service span end date  for Benefit Line No:" + benLineNo;
                    errorMessage += " <br/>";
                }
                if (gridId !== "gvHospiceHLTCFProviderService") {
                    var days = Math.round((gridPeriodEndDate - gridPeriodEffDate) / (1000 * 60 * 60 * 24));
                    var benDays = Math.round((provEndDate - provEffDate) / (1000 * 60 * 60 * 24));
                    if (days + 1 !== benDays + 1) {
                        errorMessage = errorMessage + '' + "'" + customMessage + "' must be assigned for every day within the provider service span period for Benefit Line No:" + benLineNo;
                        errorMessage += " <br/>";
                    }
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
                if (gridId !== "gvHospiceHLTCFProviderService") {
                    var days = Math.round((gridPeriodEndDate - gridPeriodEffDate) / (1000 * 60 * 60 * 24));
                    var benDays = Math.round((BenefitPeriodEndDate - BenefitPeriodEffDate) / (1000 * 60 * 60 * 24));
                    if (days + 1 !== benDays + 1) {
                        errorMessage = errorMessage + '' + "'" + customMessage + "' must be assigned for every day within the benefit period for Benefit Line No:" + benLineNo;
                        errorMessage += " <br/>";
                    }
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
</script>

<style type="text/css">
    .divGrid {
        width: 100%;
    }

    .gridview {
        float: right;
    }

    .gridViewHeader > th > a {
        color: White !important;
    }

    .panelOwnerInfo {
        padding: 20px 20px 0px 20px;
    }

    td, th {
        padding: 4px !important;
    }

    .row {
        margin-top: 4px;
        margin-bottom: 4px;
    }

    .col-sm-3 {
        width: 23%;
    }
</style>
 <asp:HiddenField ID="hdnHospiceStatus" runat="server" />
 <asp:HiddenField ID="hdHospiceMaintenanceSubRole" runat="server" />
 <asp:HiddenField ID="hdLastSubmittedDate" runat="server" />
<asp:HiddenField ID="hdIsAllownewbenPeriod" runat="server" />
<div>
   <a id="DisenrollmentErrorMessagelink" style="color: red;"> <span id="DisenrollmentErrorMessage" style="color: red;"></span></a> <br />
    <div id ="divPortalErrors" style="padding:5px;" runat="server"></div>
     <asp:Label ID="lblMessage" class="expandcollapse" runat="server" Style="color: red;"></asp:Label>
</div>
<br />
<div>
    <table>
        <tr>
            <td>
                <strong><asp:Label ID="lblApplicationType" runat="server" CssClass="formLabel300" Text="Hospice Application Action Type" /></strong></td>
            <td>
                <asp:DropDownList ID="ddlHospiceApplicationType" AutoPostBack="true" OnSelectedIndexChanged="ddlHospiceApplicationType_Change"  runat="server"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="valApplicationType" runat="server" ControlToValidate="ddlHospiceApplicationType"
                    Enabled="false" SetFocusOnError="true" Display="Dynamic" Text="*"
                    ValidationGroup="ProviderSearch" ErrorMessage="* Hospice Application Action Type is required."></asp:RequiredFieldValidator>

            </td>
            <td>
                <p style="color:red">(Changing this selection will result in the loss of unsaved data.)</p> 
            </td>
        </tr>
    </table>
</div>
<div id="DivHospicEnrollSearch" runat="server">
    <div class="enrollment">
    </div>
    <asp:Panel ID="pnldetails" runat="server">
        <uc1:HospiceRecipientInformation ID="uc1HospiceEnrollment" runat="server" Visible="true" EnableViewState="true" />
    </asp:Panel>
    <ajax:Accordion ID="Accordion1" runat="Server" SelectedIndex="0" EnableViewState="false"
            HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
            SuppressHeaderPostbacks="true">
            <Panes>
                <ajax:AccordionPane ID="AccordionPane1" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                    ContentCssClass="accordionContent">
                    <Header>
                        <asp:Label ID="Label1" class="expandcollapse" runat="server" Text="- * ENROLLMENT AND DISENROLLMENT"></asp:Label>
                    </Header>
                    <Content>

                        <div class="row" style="width: 97%; padding: 10px;">
                            <div class="col-md-2" style="text-align: right;">
                                <span class="ohio-field-label"><span style="color: red">*</span> <b>Election Date:</b>  </span>
                            </div>
                            <div class="col-md-2" style="position: static;">
                                <asp:TextBox ID="txtElectionDate" Width="80%" autocomplete="off" ClientIDMode="static" runat="server">
                                </asp:TextBox>
                                <asp:HiddenField ID="hdnElectionDate" runat="server" />
                                <ajax:CalendarExtender ID="clElectionDate" TargetControlID="txtElectionDate" runat="server" Format="MM/dd/yyyy" />
                                <asp:RequiredFieldValidator ID="rfvtxtElectionDate" runat="server" ControlToValidate="txtElectionDate" isplay="Dynamic" ForeColor="Red" ErrorMessage="Please select election date" SetFocusOnError="True" ValidationGroup="vgOnSubmit"></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-3" style="text-align: right;">
                                <span class="ohio-field-label"><b>Disenrollment Date:</b>
                                </span>
                            </div>
                            <div class="col-md-2" style="position: static;">
                                <asp:TextBox ID="txtDisenrollment" Width="80%" autocomplete="off" ClientIDMode="static" runat="server"></asp:TextBox>
                                <asp:HiddenField ID="hdnDisenrollment"  runat="server" />
                                <ajax:CalendarExtender ID="ClDisenrollment" TargetControlID="txtDisenrollment" runat="server" Format="MM/dd/yyyy" />
                               <%-- <asp:RequiredFieldValidator ID="rfvtxtDisenrollment" runat="server" ControlToValidate="txtDisenrollment" ErrorMessage="Please select disenrollment date"
                                    Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="vgOnSubmit"></asp:RequiredFieldValidator>--%>
                            </div>
                            <div class="col-md-1">
                            </div>
                        </div>
                        <div style="text-align: center">
                            <span id="EnrollementErrorMessage" style="color: red;"></span>
                        </div>
                    </Content>
                </ajax:AccordionPane>
            </Panes>
        </ajax:Accordion>
</div>
<div>
    <div class="WhiteBox" style="padding: 0px !important;">
         <uc3:HospiceBenefitPeriod ID="uc3HospiceBenefitPeriod" runat="server" Visible="true" EnableViewState="true" />
        <%--<uc1:HospiceRecipientServiceLocation ID="uc1HospiceRecipientServiceLocation" runat="server" Visible="true" EnableViewState="true" />--%>
        
       
        <%--<uc4:HospiceIDGPhysician ID="uc4HospiceIDGPhysician" runat="server" Visible="true" EnableViewState="true" />--%>
        <%--<uc5:HospiceAttendingPhysician ID="uc5HospiceAttendingPhysician" runat="server" Visible="true" EnableViewState="true" />--%>
      <%--  <uc6:HospiceOtherPayerSpan ID="uc6HospiceOtherPayerSpan" runat="server" Visible="true" EnableViewState="true" />
        <uc7:HospiceEpisodeofCare ID="uc7HospiceEpisodeofCare" runat="server" Visible="true" EnableViewState="true" />
        <uc8:HospiceTerminalIllnessDiagnosis ID="uc8HospiceTerminalIllnessDiagnosis" runat="server" Visible="true" EnableViewState="true" />

        <uc9:HospiceProviderServiceSpan ID="uc9HospiceProviderServiceSpan" runat="server" Visible="true" EnableViewState="true" />
        <uc10:HospiceHLTCFProviderService ID="uc10HospiceHLTCFProviderService" runat="server" Visible="true" EnableViewState="true" />
        <uc11:HospiceAttachment ID="uc11HospiceAttachment" runat="server" Visible="true" EnableViewState="true" />--%>
        <%--<uc12:HospiceDocumentsByMail ID="uc12HospiceDocumentsByMail" runat="server" Visible="true" EnableViewState="true" />--%>
        <ajax:Accordion ID="AccordionHospiceConfirmation" runat="Server" SelectedIndex="0" EnableViewState="false"
            HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
            AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
            SuppressHeaderPostbacks="true">
            <Panes>
                <ajax:AccordionPane ID="AccordionPaneHospiceConfirmation" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                    ContentCssClass="accordionContent">
                    <Header>
                        <asp:Label ID="lblEnrollmentAndDisEnrollmentExpansion" class="expandcollapse" runat="server" Text="- CONFIRMATION"></asp:Label>
                    </Header>
                    <Content>
                        <div class="row" style="text-align: center;">
                            <div class="col-sm-6 col-md-4 col-lg-3 ">
                                <span class="ohio-field-label"><span style="color: red"></span><b>Application Submission Date : </b>
                                    <asp:Label ID="lblDateofApplicationUpdate" runat="server">
                                    </asp:Label>
                                </span>

                            </div>
                            <div class="col-sm-6 col-md-4 col-lg-3 ">
                                <span class="ohio-field-label"><span style="color: red"></span><b>Hospice Tracking Number : </b>
                                    <asp:Label ID="lblHospiceTrackingNumber" runat="server"></asp:Label>
                                </span>

                            </div>
                        </div>
                        <hr />

                        <div style="font-size: 12pt; color: red; padding-left: 25px; padding-right: 25px; text-align: left;">
                            IMPORTANT - This Hospice Tracking Number (HTN) is necessary for accessing the status of submitted enrollments. Please write this number down or print this page and keep it for your records PRIOR TO EXITING. Application submitted after 4 PM will not processed until next business day.
                     <br />
                        </div>
                        <div style="padding-top: 20px; text-align: center">
                            <asp:Button ID="btnHospiceSubmit" runat="server" Text="Submit" OnClientClick ="if (!IsValidHospiceSubmit()) return false;" OnClick="btnHospiceSubmit_Click" Enabled="false" 
                                CssClass="buttonBoxFocus" />&nbsp;&nbsp;
                            <asp:Button ID="btnHospiceCancel" runat="server" Text="Cancel" OnClientClick="return confirm('Are you sure you want to cancel?');" OnClick="btnHospiceCancel_Click" CssClass="buttonBox" />
                            <br />
                           
                        </div>

                    </Content>
                </ajax:AccordionPane>

            </Panes>
        </ajax:Accordion>
    </div>
</div>