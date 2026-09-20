var validFileExtensions = "doc,docx,pdf,xls,XLS,xlsm,xlsx,ppt,pptx,mdi,jpe,zip,txt,jpg,jpeg,png,gif,bmp,tif,tiff,pi,ec,zip,csv,xlsm,msg,acrbak";
var PriorAttachmentUpload = "ctl00_MainContent_uc1SubmitPriorAuthorization_PriorAttachmentUpload";
var priorDentalAttachmentUpload = "ctl00_MainContent_uc1SubmitPriorAuthorization_priorDentalAttachmentUpload";
var ddlPriorDentalAuthDocType = `ctl00_MainContent_uc1SubmitPriorAuthorization_ddlPriorDentalAuthDocType`;
var txtPriorDentalAttachmentNote = `ctl00_MainContent_uc1SubmitPriorAuthorization_txtPriorDentalAttachmentNote`;
var priorAuthDentalAttachments = [];

let fileNameToAPI = '';
let originalFileNameToAPI = '';

$(document).ready(function () {
    populateAttachmentTable();
});

async function addAttachmentOnButtonClickEvent() {

    console.log('addDentalAttachmentOnButtonClick called');

    var claimType = $(".rblClaimType input:checked").val();



    try {

        const isPAValidate = validateClaim();
        if (isPAValidate !== undefined && !isPAValidate) {
            console.log("Validation failed: Too many attachments.");
            return;
        }

        var isValid = await validateDentalFileAndUploadToS3(claimType === "dental");
        claimType === 'dental' ? $(".divDentalUploading").hide() : $(".divUploading").hide();
        return isValid;
    } catch (error) {
        console.error("Error validating file upload:", error);
        claimType === 'dental' ? $(".divDentalUploading").hide() : $(".divUploading").hide();
        return false;
    }
}
function validateDocumentType(isDental) {
    const docTypeSelector = isDental
        ? "#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlPriorDentalAuthDocType"
        : "#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlPriorAuthDocType";

    const spanSelector = isDental
        ? "#spanddlDentalDocumentType"
        : "#spanddlDocumentType";

    const selectedValue = $(docTypeSelector).val();

    if (!selectedValue || selectedValue.trim() === "") {
        $(spanSelector).show();
        return false;
    } else {
        $(spanSelector).hide();
        return true;
    }
}

async function validateDentalFileAndUploadToS3(isDental) {

    var spanId = isDental ? "#spanDentalAttachment" : "#spanAttachment";
    var fileInputId = isDental ? priorDentalAttachmentUpload : PriorAttachmentUpload;
    var claimType = $(".rblClaimType input:checked").val();

    $(spanId).hide();
    var file = document.getElementById(fileInputId).files[0];

    if (!file) {
        $(spanId).show().text("No file selected.");
        return false;
    }

    if (file.size > 10485760) {
        $(spanId).show().text("File size exceeds max 10MB limit.");
        return false;
    }

    var fileExtension = file.name.substring(file.name.lastIndexOf(".")).toLowerCase();
    var allowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".pdf", ".zip", ".csv",
        ".xls", ".xlsx", ".ppt", ".pptx", ".doc", ".docx", ".xlsm", ".mdi",
        ".jpe", ".tif", ".pi", ".ec", ".msg", ".acrbak"];

    if (!allowedExtensions.includes(fileExtension)) {
        $(spanId).show().text("Not a valid file type.");
        return false;
    }

    if (!validateDocumentType(isDental)) {
        return false;
    }


    let timestamp = new Date().toLocaleTimeString('it-IT').replace(/:/g, '');
    $('#hdnTime').val(timestamp);
    var destinationPayerID = $("#txtDestinationpayerID").val();
    $("#tradingPartnerID").val(destinationPayerID);


    let fileName = `${attachmentFileName}${$("#hdnTime").val()}--${attachmentFileNameSuffix}--${$("#tradingPartnerID").val()}.${file.name.split('.').pop()}`;
    fileNameToAPI = fileName;
    originalFileNameToAPI = file.name;

    claimType === 'dental' ? $(".divDentalUploading").show() : $(".divUploading").show();

    try {

        const response = await getAttachmentPresignUrl(fileName, file.type, "PUT");
        console.log(response);

        if (response) {

            const isUploaded = await addAttachmentToS3Bucket(response);

            if (isUploaded === '') {
                await uploadAttachments();
            }
        }
    } catch (error) {
        console.error("Failed to get Presigned URL:", error);
        claimType === 'dental' ? $(".divDentalUploading").hide() : $(".divUploading").hide();
    }

    return true;
}

async function getAttachmentPresignUrl(fileName, contentType, method) {

    var claimType = $(".rblClaimType input:checked").val();
    var APIToken = $("[id*=hdnAccessToken]").val();

    try {
        const response = await CommonAjaxCall({
            url: webApiAttachments + "GetPresignUrl",
            method: "GET",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            data: { fileName: fileName, contentType: contentType, method: method }
        });

        return response;
    } catch (error) {
        console.error("Error fetching Presigned URL:", error);
        claimType === 'dental' ? $(".divDentalUploading").hide() : $(".divUploading").hide();
        return null;
    }
}

async function addAttachmentToS3Bucket(response) {
    var claimType = $(".rblClaimType input:checked").val();

    try {
        let fileInputId = claimType === 'dental' ? priorDentalAttachmentUpload : PriorAttachmentUpload;
        var file = document.getElementById(fileInputId).files[0];

        if (!file) {
            console.log("No file selected. Please choose a file.");
            return;
        }

        var contentType = file.type;

        let uploadResponse = await $.ajax({
            type: 'PUT',
            url: response.preSignedUrl,
            contentType: contentType,
            processData: false,
            data: file,
            crossDomain: true
        });
        return uploadResponse;
    } catch (error) {
        claimType === 'dental' ? $(".divDentalUploading").hide() : $(".divUploading").hide();
        console.error("File upload failed:", error);

    }
}

async function uploadAttachments() {

    var claimType = $(".rblClaimType input:checked").val();

    try {

        const paytype = claimType === 'dental' ? "DentalProf" : "Institutional";
        var validationResult = await validateAttachment(paytype);

        if (validationResult) {
            console.log("Validation failed: Too many attachments.");
            return;
        }

        console.log("Validation passed: You can add attachments.");

        const response = await performUploadDentalAttachment();

        console.log("Upload Response:", response);

        if (response && response.DocumentId) {
            console.log("File uploaded successfully! Document ID: " + response.DocumentId);
            saveRecordToPriorAuthAttachment(response.DocumentId);

            if (claimType === 'dental') {
                $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtPriorDentalAttachmentNote").val("");
                $("#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlPriorDentalAuthDocType").prop("selectedIndex", 0);
                $("#ctl00_MainContent_uc1SubmitPriorAuthorization_priorDentalAttachmentUpload").val("");
            }
            else {
                $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtPriorAttachmentNote").val("");
                $("#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlPriorAuthDocType").prop("selectedIndex", 0);
                $("#ctl00_MainContent_uc1SubmitPriorAuthorization_PriorAttachmentUpload").val("");
            }

            claimType === 'dental' ? $(".divDentalUploading").hide() : $(".divUploading").hide();

        } else {
            console.error("Upload failed or missing document ID.");
        }
    } catch (error) {
        claimType === 'dental' ? $(".divDentalUploading").hide() : $(".divUploading").hide();
        console.error("Error during attachment validation/upload:", error);
    }
}

async function performUploadDentalAttachment() {

    var claimType = $(".rblClaimType input:checked").val();
    var attachmentPayload = prepareSubmitAttachmentPayload();

    console.log("Uploading attachment...");

    try {
        const response = await CommonAjaxCall({
            url: webApiAttachments + "SavePAUploadedFileData",
            method: "POST",
            headers: getHeaders(),
            data: attachmentPayload,
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });

        console.log("File Uploaded to document store " + response.DocumentId);

        return response;
    } catch (error) {
        console.error("Upload failed:", error);
        claimType === 'dental' ? $(".divDentalUploading").hide() : $(".divUploading").hide();
        console.error("Something went wrong!");
    }
}

async function validateAttachment(paType) {
    const claimType = $(".rblClaimType input:checked").val();

    try {
        const medicaidId = $('#ctl00_MainContent_ucRegProgressBar_lblProMedicaidID2').text();

        const response = await getPriorAuthAttachmentCount(claimType, medicaidId);

        if (!response) return false;

        const attachmentCount = response.attachmentCount;
        const status = attachmentCount >= 10;

        const errorMsgSelector = paType === "Institutional"
            ? "#ctl00_MainContent_uc1SubmitPriorAuthorization_lblAttachmentErrorMsg"
            : "#ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalAttachmentErrorMsg";

        $(errorMsgSelector).text(status ? "Maximum 10 attachments can be submitted." : "").toggle(status);

        return status;
    } catch (error) {
        claimType === 'dental' ? $(".divDentalUploading").hide() : $(".divUploading").hide();
        console.error("Error fetching attachment count:", error);
        return false;
    }
}

async function getPriorAuthAttachmentCount(claimType, medicaidId) {
    try {
        const response = await CommonAjaxCall({
            url: webApiAttachments + "PriorAuthAttachmentCount",
            method: "GET",
            headers: getHeaders(),
            data: { claimType, medicaidId },
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });

        return response;
    } catch (error) {
        console.error("Error fetching PriorAuthAttachment Count:", error);
        return null;
    }
}
function prepareSubmitAttachmentPayload() {

    const claimType = $(".rblClaimType input:checked").val();
    var fileInputId = claimType === 'dental' ? priorDentalAttachmentUpload : PriorAttachmentUpload;
    var file = document.getElementById(fileInputId).files[0];

    var originalDocumentName = file.name;


    var claimTypeData = getClaimTypeData();

    const data = {
        fileName: file.name,
        tradingPartnerID: $("#txtDestinationpayerID").val(),
        uuid: crypto.randomUUID(),
        payerRequested: "Yes",
        paNumber: $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtPANumber2").val(),
        memberId: $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtMedicaidBillingNumber").val(),
        providerId: $('#ctl00_MainContent_ucRegProgressBar_lblProMedicaidID2').text(),
        providerNPI: $('#ctl00_MainContent_ucRegProgressBar_lblPRONPI2').text(),
        claimNumber: "",
        originalDocumentName: originalDocumentName,
        toSend: false,
        receiverId: $("#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlSubCapitaPayerIDs").val() || '',
        claimTypeId: claimTypeData.claimTypeId,
        documentTypeId: claimTypeData.documentTypeId,
        claimType: claimType,
        timestamp: $('#hdnTime').val(),
        medicaidId: $('#ctl00_MainContent_ucRegProgressBar_lblProMedicaidID2').text(),
        ApiFileName: fileNameToAPI
    };
    return data;


}
function getClaimTypeData() {

    const claimType = $(".rblClaimType input:checked").val().toUpperCase();

    switch (claimType) {
        case "DENTAL":
            return { claimTypeId: 1, documentTypeId: parseInt($("#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlPriorDentalAuthDocType").val()) };
        case "PROFESSIONAL":
            return { claimTypeId: 2, documentTypeId: parseInt($("#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlPriorAuthDocType").val()) };
        case "INSTITUTIONAL":
            return { claimTypeId: 3, documentTypeId: parseInt($("#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlPriorAuthDocType").val()) };
        default:
            return { claimTypeId: 0, documentTypeId: 0 };
    }
}
function saveRecordToPriorAuthAttachment(documentId) {
    const claimType = $(".rblClaimType input:checked").val();
    const isDental = claimType === "dental";

    const storedData = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnPriorAuthAttachments").val();
    const priorAuthAttachments = storedData?.trim() ? JSON.parse(storedData) : [];

    // Selectors for Dental vs Institutional
    const docTypeSelector = isDental
        ? "#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlPriorDentalAuthDocType"
        : "#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlPriorAuthDocType";

    const noteSelector = isDental
        ? "#ctl00_MainContent_uc1SubmitPriorAuthorization_txtPriorDentalAttachmentNote"
        : "#ctl00_MainContent_uc1SubmitPriorAuthorization_txtPriorAttachmentNote";

    const newAttachment = {
        PRIOR_AUTH_SUB_DOCUMENT_TYPE_ID: $(docTypeSelector).val(),
        PRIOR_AUTH_SUB_TRACKING_NUMBER: $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtPatientTrckNum").val(),
        PRIOR_AUTH_SUB_DOCUMENT_ID: documentId,
        PRIOR_AUTH_SUB_Note: $(noteSelector).val(),
        PRIOR_AUTH_SUB_ATTACHMENT_AUTH_TYPE: getClaimTypeData().claimTypeId,
        PRIOR_AUTH_SUB_DOCUMENT_TYPE_DESC: $(docTypeSelector + " option:selected").text(),
        DOCUMENT_ID: documentId?.toString().trim() ? documentId : 0,
        PRIOR_AUTH_SUB_ATTACHMENT_STATUS: "1",
        MedicaidId: $("#ctl00_MainContent_ucRegProgressBar_lblProMedicaidID2").text(),
        PRIOR_AUTH_SUB_ATTACHMENT_ID: Math.floor(Math.random() * 1000000),
        DOCUMENT_NAME: fileNameToAPI,
        ORIGINAL_DOCUMENT_NAME: originalFileNameToAPI,
    };

    priorAuthAttachments.push(newAttachment);

    $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnPriorAuthAttachments").val(JSON.stringify(priorAuthAttachments));

    console.log("Updated attachments:", priorAuthAttachments);
    populateAttachmentTable();
}
function populateAttachmentTable() {
    const claimType = $(".rblClaimType input:checked").val();
    const isDental = claimType === "dental";

    const tableBody = $(isDental ? "#tblDentalAttachmentBody" : "#tblAttachmentBody");
    tableBody.empty();

    const storedData = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnPriorAuthAttachments").val();
    const priorAuthAttachments = storedData?.trim() ? JSON.parse(storedData) : [];

    $(isDental ? "#tblDentalAttachment" : "#tblAttachment").toggle(priorAuthAttachments.length > 0);

    if (priorAuthAttachments.length === 0) return;

    const rows = priorAuthAttachments.map((attachment, index) => {
        const rowClass = index % 2 === 0 ? "gridViewRow" : "gridViewAltRow";

        return `
        <tr class="${rowClass}" style="height:65px;background-color: transparent !important;;">
          <td><img src="../Images/file-icon.jpg" alt="File Icon" height="20" width="20" onclick="downloadFile('${attachment.DOCUMENT_NAME}')" style="cursor: pointer;" /></td>
          <td>${index + 1}</td>
          <td>${attachment.DOCUMENT_ID}</td>
          <td>${attachment.PRIOR_AUTH_SUB_TRACKING_NUMBER}</td>
          <td>${attachment.PRIOR_AUTH_SUB_DOCUMENT_TYPE_DESC}</td>
          <td>${attachment.PRIOR_AUTH_SUB_Note}</td>
          <td><button class="btn btn-danger" style="font-weight:bold;width:90px;" onclick="removeAttachment(${attachment.PRIOR_AUTH_SUB_ATTACHMENT_ID})">Delete</button></td>
        </tr>`;
    });
    tableBody.append(rows.join(""));
}
function removeAttachment(attachmentId) {

    if (!confirm("Are you sure you want to delete this record?")) {
        return;
    }

    var storedData = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnPriorAuthAttachments").val();
    var priorAuthAttachments = storedData && storedData.trim() !== "" ? JSON.parse(storedData) : [];

    priorAuthAttachments = priorAuthAttachments.filter(attachment => attachment.PRIOR_AUTH_SUB_ATTACHMENT_ID !== attachmentId);

    $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnPriorAuthAttachments").val(JSON.stringify(priorAuthAttachments));

    populateAttachmentTable();
}

async function downloadFile(fileName) {
    try {
        const response = await fetch(webApiAttachments + "download?" + new URLSearchParams({ fileName }), {
            method: "GET",
            headers: getHeaders()
        });

        if (!response.ok) throw new Error(`Failed to download. Status: ${response.status}`);

        const blob = await response.blob();
        const url = URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = url;
        a.download = fileName;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        URL.revokeObjectURL(url);
    } catch (error) {
        console.error("Download failed:", error);
        console.error("Failed to download attachment.");
    }
}

async function downloadS3FileClient(fileName) {
    try {

        if (!fileName) throw new Error("Invalid file name.");

        const contentType = 'application/octet-stream';

        const response = await getDentalAttachmentPresignUrl(fileName, contentType, "GET");

        if (!response || !response.preSignedUrl) {
            throw new Error("Failed to retrieve pre-signed URL.");
        }


        const blob = await CommonAjaxCall({
            url: response.preSignedUrl,
            method: 'GET',
            crossDomain: true,
            dataType: '',
            xhrFields: { responseType: 'blob' }
        });

        if (!blob || blob.size === 0) {
            throw new Error("Failed to retrieve file data or file is empty.");
        }

        const blobUrl = URL.createObjectURL(blob);
        const a = document.createElement("a");
        a.href = blobUrl;
        a.download = fileName;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        URL.revokeObjectURL(blobUrl);
    } catch (err) {
        console.error("Download failed:", err);
        console.error(`Download failed: ${err.message || err.statusText}`);
    }
}
function resetErrorMessages(isDental) {
    const prefix = isDental ? "Dental" : "Inst";
    $(`#ctl00_MainContent_uc1SubmitPriorAuthorization_lbl${prefix}UploadErrMsg`).text("");
    $(`#ctl00_MainContent_uc1SubmitPriorAuthorization_lbl${prefix}DocTypeErrMsg`).text("");
    $(`#ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalAttachmentErrorMsg`).text("");
    $(`#ctl00_MainContent_uc1SubmitPriorAuthorization_lblAttachmentErrorMsg`).text("");
}
function validateAttachmentLimit(isDental) {
    const tableSelector = isDental ? "#tblDentalAttachment" : "#tblAttachment";
    const errorMsgSelector = isDental
        ? "#ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalAttachmentErrorMsg"
        : "#ctl00_MainContent_uc1SubmitPriorAuthorization_lblAttachmentErrorMsg";

    if ($(`${tableSelector} tbody tr`).length === 10) {
        $(errorMsgSelector).show().text("Maximum of 10 attachments can be submitted");
        return false;
    }
    return true;
}
function validateMedicaidBillingNumber(isDental) {
    const errorMsgSelector = isDental
        ? "#ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalAttachmentErrorMsg"
        : "#ctl00_MainContent_uc1SubmitPriorAuthorization_lblAttachmentErrorMsg";

    if ($("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtMedicaidBillingNumber").val().trim() === "") {
        $(errorMsgSelector).show().text("Medicaid Billing Number is required");
        return false;
    }
    return true;
}
function validateDocType(isDental) {
    const docTypeSelector = isDental
        ? "#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlPriorDentalAuthDocType"
        : "#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlPriorAuthDocType";

    const errorMsgSelector = isDental
        ? "#ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalDocTypeErrMsg"
        : "#ctl00_MainContent_uc1SubmitPriorAuthorization_lblInstDocTypeErrMsg";

    if ($(docTypeSelector).prop("selectedIndex") === 0) {
        $(errorMsgSelector).show().text("Document type is required");
    } else {
        $(errorMsgSelector).hide().text("");
    }
}
function validateClaim() {

    const isDental = $(".rblClaimType input:checked").val() === "dental";

    resetErrorMessages(isDental);

    if (!validateAttachmentLimit(isDental) || !validateMedicaidBillingNumber(isDental)) return false;

    validateDocType(isDental);

    const prefix = isDental ? "Dental" : "Inst";
    if ($(`#ctl00_MainContent_uc1SubmitPriorAuthorization_lbl${prefix}DocTypeErrMsg`).is(":visible") ||
        $(`#ctl00_MainContent_uc1SubmitPriorAuthorization_lbl${prefix}UploadErrMsg`).is(":visible")) {
        return false;
    }
    return true;
}
function getHeaders() {
    var apiToken = $("[id*=hdnAccessToken]").val();
    return {
        "Access-Control-Allow-Origin": "*",
        "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
        "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
        "Authorization": "Bearer " + apiToken
    };
}
function CommonAjaxCall(options) {

    var defaults = {
        url: '',
        method: 'GET',
        data: {},
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        returnStatus: false,
        headers: {}, // Default empty headers
        success: function (response, textStatus, xhr) {
            console.log("Request succeeded:", response);
        },
        error: function (xhr, status, error) {
            console.error("Request failed:", error);
        }
    };

    var settings = $.extend({}, defaults, options);

    if (settings.method.toUpperCase() !== 'GET' && typeof settings.data === 'object') {
        settings.data = JSON.stringify(settings.data);
    }

    var dfd = $.Deferred();
    var jqXHRRequest = null;

    // Make AJAX request
    jqXHRRequest = $.ajax({
        url: settings.url,
        type: settings.method,
        data: settings.data,
        dataType: settings.dataType,
        contentType: settings.contentType,
        headers: settings.headers,
        success: function (response, textStatus, xhr) {
            settings.success(response, textStatus, xhr);
            if (settings.returnStatus && (response === undefined || response === null || response === '')) {
                dfd.resolve({ status: xhr.status });
            } else {
                dfd.resolve(response);
            }
        },
        error: function (xhr, status, error) {
            settings.error(xhr, status, error);
            dfd.reject(xhr, status, error);
        }
    });

    var promise = dfd.promise();
    promise.abort = function () {
        if (jqXHRRequest) {
            jqXHRRequest.abort();
        }
    };

    return promise;
}