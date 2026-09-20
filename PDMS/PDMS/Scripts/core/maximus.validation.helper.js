
/*

    place all new validation types / concepts here as well in the code behind helper class.




*/


$.validator.addMethod("dependentrequired", function (value, element, params) {
    var otherName = params.other;
    var targetValue = (params.target || "").toLowerCase();

    var other = $('[name="' + otherName + '"], #' + otherName);
    if (!other.length) return true;

    var actualValue;

    if (other.is(':checkbox')) {
        actualValue = other.is(':checked').toString().toLowerCase();
    } else if (other.is(':radio')) {
        actualValue = $('[name="' + otherName + '"]:checked').val();
        actualValue = actualValue ? actualValue.toLowerCase() : "";
    } else {
        actualValue = (other.val() || "").toLowerCase();
    }

    var shouldRequire = actualValue === targetValue;
    if (!shouldRequire) return true;

    return $.trim(value).length > 0;
});

$.validator.unobtrusive.adapters.add("dependentrequired", ["other", "target"], function (options) {
    options.rules["dependentrequired"] = {
        other: options.params.other,
        target: options.params.target
    };
    options.messages["dependentrequired"] = options.message;
});



/**
 *  DATE FORMAT AND LOGIC CHECK ATTRIBTUES
 */

$.validator.addMethod("startdateistodayorlater", function (value) {
    if (!value) return true;
    const today = new Date();
    const inputStartDate = new Date(value); //getting the value of the input date selected
    today.setHours(0, 0, 0, 0); //default preperation to just comapre dates
    return inputStartDate >= today; //make sure date is a future date
});

$.validator.unobtrusive.adapters.addBool("startdateistodayorlater");


$.validator.unobtrusive.adapters.add("enddateisafterstartdate", ["other"], function (options) {
    options.rules["enddateisafterstartdate"] = options.params.other; //
    options.messages["enddateisafterstartdate"] = options.message;
});

$.validator.addMethod("enddateisafterstartdate", function (value, element, param) {
    const startDateVal = $("#" + param).val();
    if (!value || !startDateVal) return true;
    const start = new Date(startDateVal);
    const end = new Date(value);
    return end >= start;
});


$(function () {
    $.validator.unobtrusive.parse("form:visible"); //if hididng elements from other views or wrappers not wanting then use this.
    console.log("validation helper running...");

    $("input").on("blur", function () {

        $(this).valid();
    });
});

function clearAllValidationErrorsForContainer(conatinerSelector) {

    let $container = $(conatinerSelector);

    // Remove error classes from inputs
    $container.find(".input-validation-error").removeClass("input-validation-error");

    // Clear validation summary (if any)
    $container.find(".validation-summary-errors").removeClass("validation-summary-errors").addClass("validation-summary-valid");

    // Clear field-specific error messages
    $container.find(".field-validation-error")
        .removeClass("field-validation-error")
        .addClass("field-validation-valid")
        .empty();
}

function clearAllValidationErrorsForForm(formSelector) {
        var form = $(formSelector);

        // Reset validation state
        form.validate().resetForm();

        // Remove error classes from inputs
        form.find(".input-validation-error").removeClass("input-validation-error");

        // Clear validation summary (if any)
        form.find(".validation-summary-errors").removeClass("validation-summary-errors").addClass("validation-summary-valid");

        // Clear field-specific error messages
        form.find(".field-validation-error")
            .removeClass("field-validation-error")
            .addClass("field-validation-valid")
            .empty();
}


function reparsedFormValidation(formSelector) {
    let $form = $(formSelector);
    $form.removeData("validator");
    $form.removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse($form);
}
