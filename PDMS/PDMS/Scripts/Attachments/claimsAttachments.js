const UploadAttachments = "ctl00_MainContent_uc5SubmitClaim_ucClaimAttachment_UploadAttachments";
const ddlDocumentTypeclaims = `ctl00_MainContent_uc5SubmitClaim_ucClaimAttachment_ddlDocumentTypeclaims`;
const lblInstUploadErrMsg = `ctl00_MainContent_uc5SubmitClaim_ucClaimAttachment_lblInstUploadErrMsg`;
const clmAuthAttachmnts = "ctl00_MainContent_uc5SubmitClaim_ucClaimAttachment_gvClaimAuthAttachment";
const claimTyoperbl = "#ctl00_MainContent_uc5SubmitClaim_rblClaimType";
let claimAttachments = [];

let fileNameToAPI = '';
let originalFileNameToAPI = '';
var claimType;
var claimNumber;
var medicaidId;
var NPI;


$(document).ready(function () {
    populateAttachmentTable();
});

document.addEventListener("DOMContentLoaded", function () {
    claimType = window.claimType;
    claimNumber = window.claimNumber;
    medicaidId = window.medicaidId;
    NPI = window.NPI; 
    UUID = window.UUID;
})

async function addAttachmentOnButtonClickEvent() {
    try {
        
        const isValid = validateClaimsFileUpload();
        if (isValid) {
            uploadAttachment();
        }
        //var isValid = await validateDentalFileAndUploadToS3(claimType === "dental");
        $(".divUploading").hide();
        return isValid;
    } catch (error) {
        console.error("Error validating file upload:", error);
        $(".divUploading").hide();
        return false;
    }
}

async function uploadAttachment() {
    let timestamp = new Date().toLocaleTimeString('it-IT').replace(':', '').replace(':', '');

    const fileInput = document.getElementById(UploadAttachments);
    const file = fileInput?.files[0];
    const tradingPartnerIDval = window.tradingPartnerDval;
    let fileName = `${attachmentFileName}${$("#hdnTime").val()}--${attachmentFileNameSuffix}--${tradingPartnerIDval}.${file.name.split('.').pop()}`;
    originalFileNameToAPI = fileName;
    $(".divUploading").show();

    try {
        const response = await getAttachmentPresignUrl(fileName, file.type, "PUT");
        console.log(response);
        if (response) {
            const isUploaded = await addAttachmentToS3Bucket(response);
            if (isUploaded === '') {
                await saveAttachmentToDB(file,fileName);
            }
        }
    }
    catch (error) {
        console.error("Failed to get Presigned URL:", error);
        $(".divUploading").hide();
    }
}


async function addAttachmentToS3Bucket(response) {

    try {
        var file = document.getElementById(UploadAttachments).files[0];

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
        $(".divUploading").hide();
        console.error("File upload failed:", error);

    }
}

async function getAttachmentPresignUrl(fileName, contentType, method) {

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
        $(".divUploading").hide();
        return null;
    }
}

function validateClaimsFileUpload() {
    resetErrorMessages();

    const $attachmentMessage = $("#spanAttachment");
    $attachmentMessage.hide();

    const fileInput = document.getElementById(UploadAttachments);
    const file = fileInput?.files[0];

    if (!file) {
        $attachmentMessage.text("Please select a file to upload").show();
        return false;
    }

    const recipientName = $('#ctl00_MainContent_uc5SubmitClaim_ucRecipientInformationPanel_lblfrstmi').text().trim();
    if (!recipientName) {
        $attachmentMessage.text("Please enter recipient information to add attachment").show();
        return false;
    }

    const MAX_FILE_SIZE = 10 * 1024 * 1024; // 10MB
    if (file.size > MAX_FILE_SIZE) {
        $attachmentMessage.text("File size exceeds max 10MB limit").show();
        return false;
    }

    const allowedExtensions = [
        ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".pdf", ".zip", ".csv",
        ".xls", ".xlsx", ".ppt", ".pptx", ".doc", ".docx", ".xlsm", ".mdi", ".jpe",
        ".tif", ".pi", ".ec", ".msg", ".acrbak"
    ];

    const fileExtension = file.name.slice(file.name.lastIndexOf(".")).toLowerCase();
    if (!allowedExtensions.includes(fileExtension)) {
        $attachmentMessage.text("Not a valid file type").show();
        return false;
    }

    const tableSelector = "#tblAttachment";
    if ($(`${tableSelector} tbody tr`).length === 10) {
        $("#ctl00_MainContent_uc5SubmitClaim_ucClaimAttachment_lblAttachmentErrorMsg").show().text("Maximum of 10 attachments can be submitted");
        return false;
    }

    return true;
}

function resetErrorMessages() {
    $(`#ctl00_MainContent_uc5SubmitClaim_ucClaimAttachment_lblAttachmentErrorMsg`).text("");
    $("#spanAttachment").text("");
}
function populateAttachmentTable() {

    const storedData = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_hdnPriorAuthAttachments").val();
    const priorAuthAttachments = storedData?.trim() ? JSON.parse(storedData) : [];

    $("#tblAttachment").toggle(priorAuthAttachments.length > 0);

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

async function saveAttachmentToDB(file, fileName) {
    $("#lblInstUploadErrMsg").textContent = "";
    $("#lblInstDocTypeErrMsg").textContent = "";

    const attachmentGrid = document.getElementById(clmAuthAttachmnts);

    const fileInput = document.getElementById(UploadAttachments);
    const uploadErrMsg = document.getElementById(lblInstUploadErrMsg).textContent;

    if (fileInput.files.length > 0) {
        
        const docType = document.getElementById(ddlDocumentTypeclaims).value;
        const destinationPayerId = getDestinationPayerID();


        uploadPriorAuthAttachment(fileInput, docType, uploadErrMsg)
            .then(documentId => {
                if (documentId <= 0) {
                    $("#ctl00_MainContent_uc5SubmitClaim_ucClaimAttachment_lblAttachmentErrorMsg").style.display = "block";
                    return;
                }

                SaveAttachment(fileName);

                document.getElementById(ddlDocumentTypeclaims).selectedIndex = 0;
            });
    }

}

    async function SaveAttachment(fileName) {
        const destinationPayerId = getDestinationPayerID();

        const documentId = await getDocumentId();
        const documentTypeId = await getTypeNumber();
        let EDITransaction_Type_ID = "";

        if (claimType == 0) {
            EDITransaction_Type_ID = "1";
        } else if (claimType = 1) {
            EDITransaction_Type_ID = "3";
        } else if (claimType = 2) {
            EDITransaction_Type_ID = "2";
        }

        const attachmentPayload = {
            MedicaidId: medicaidId,
            EDITransactionTypeId: EDITransaction_Type_ID,
            MemberId: medicaidId,
            ClaimTypeId: claimType,
            ClaimNumber: claimNumber,
            ProviderId: medicaidId,
            PayerRequested: "No",
            PaNumber: null,
            Sender_ID: "MMISODJFS",
            ReceiverId: destinationPayerId.toString(),
            DocumentTypeId: documentTypeId.toString(),
            DocumentName: fileName.toString(),
            UUID: UUID.toString(),
            TO_SEND: "false",
            OrginalDocumentName: originalFileNameToAPI.toString(),
            DocumentId: documentId.toString(),
            Provider_NPI: NPI,
            ProviderComments: ""
        };
        try {

            const requestUrl = `${webApiAttachments}InsertClaimsOutBoundDocumentUploads`;
            const response = await CommonAjaxCall({
                url: webApiAttachments + "InsertClaimsOutBoundDocumentUploads",
                method: 'POST',
                data: attachmentPayload,
                headers: getHeaders()
            });

        } catch (error) {
            $(".divUploading").hide();
            console.error("Failed to save attachment:", error);
            return null;
        }

        populateClaimAttachmentsTable();
        resetData();

    }
    function getDestinationPayerID() {
        let tradingPartnerIDval = "";
        const storedValue = window.tradingPartnerDval;

        if (storedValue !== null) {
            tradingPartnerIDval = storedValue;
        }

        return tradingPartnerIDval;
    }
    async function getTypeNumber() {

        let claimName = "";

        if (claimType == "0") {
            claimName = "Dental";
        } else if (claimType == "1") {
            claimName = "Institutional";
        } else if (claimType == "2") {
            claimName = "Professional";
        }

        const selectedDocumentType = document.getElementById(ddlDocumentTypeclaims).selectedOptions[0].text;
        const requestData = { selectedDocumentType, claimName };

        const requestUrl = `${webApiAttachments}SelectClaimAttachmentTypeId`;
        try {
            return await CommonAjaxCall({
                url: webApiAttachments + "SelectClaimAttachmentTypeId",
                method: 'GET',
                headers: getHeaders(),
                data: { typename: selectedDocumentType, claim: claimName }
            });
        } catch (error) {
            $(".divUploading").hide();
            console.error("Failed to retrieve attachments:", error);
            return null;
        }
    }

    async function getDocumentId() {

        const selectedDocumentType = document.getElementById(ddlDocumentTypeclaims).selectedOptions[0].text;
        const requestData = { medicaidId: medicaidId };
        var APIToken = $("[id*=hdnAccessToken]").val();

        const requestUrl = `${webApiAttachments}SelectProviderByGRPMedicaidID`;
        try {
            return await CommonAjaxCall({
                url: webApiAttachments + "SelectProviderByGRPMedicaidID",
                method: 'GET',
                headers: {
                    "Access-Control-Allow-Origin": "*",
                    "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                    "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                    "Authorization": "Bearer " + APIToken
                },
                data: { medicaidId: medicaidId }
            });
        } catch (error) {
            $(".divUploading").hide();
            console.error("Failed to retrieve attachments:", error);
            return null;
        }
    }
    function resetData() {
        // Clear dropdown selection
        document.getElementById(ddlDocumentTypeclaims).prop("selectedIndex", 0); // Use 0 to reset to first option, or -1 to deselect all


        // Reset file-related variables
        fileNameToAPI = '';
        originalFileNameToAPI = '';

        // Hide validation messages if needed
        $("#spanAttachment").hide();
    }
function uploadPriorAuthAttachment(fileInput, statusLabelId, errorLabel) {
        errorLabel.innerHTML = "";

        let docID = 1;

        if (!fileInput || fileInput.files.length === 0) {
            errorLabel.innerHTML = "Unable to find posted file.";
            return 0;
        }

        const file = fileInput.files[0];

        if (!file.name.trim()) {
            errorLabel.innerHTML = "Select a file for upload.";
            return 0;
        }

        if (!_DestinationPath || _DestinationPath.trim() === "") {
            errorLabel.innerHTML = "ERROR - DestinationPath not defined. Contact system administrator.";
            return 0;
        }

        if (!_ValidFileExtensions || _ValidFileExtensions.trim() === "") {
            errorLabel.innerHTML = "ERROR - ValidFileExtensions must have a value. Contact system administrator.";
            return 0;
        }

        const errMsg = isValidExtension(file);

        if (!errMsg.toString()=="") {
            errorLabel.innerHTML = errMsg;
            return 0;
        }

        if (file.size < 1) {
            errorLabel.innerHTML = "File cannot be 0Kb.";
            return 0;
        }

        const maxFileSizeBytes = MaxFileMegaBytes * 1000 * 1024;
        if (file.size > maxFileSizeBytes) {
            errorLabel.innerHTML = `File cannot be more than ${maxFileSizeBytes.toLocaleString()} bytes (${MaxFileMegaBytes} MB) in size.`;
            return 0;
        }

        if (!/^[\w\-.]+$/.test(file.name)) {
            errorLabel.innerHTML = `The file name can contain letters, numbers, dot(.), underscore(_) and hyphen(-): ${file.name}. Please remove the Special Character from the file Name before upload.`;
            return 0;
        }

        return docID;
    }

    function isValidExtension(file) {
        let isValid = false;
        let errMsg = "";

        if (!file || file.length === 0) {
            errMsg = "No file selected.";
            return { isValid, errMsg };
        }
        let validExtensions = ["doc","docx","pdf","xls","XLS","xlsm","xlsx","ppt","pptx","mdi","jpe","zip","txt","jpg","jpeg","png","gif","bmp","tif","tiff","pi","ec","zip","csv","xlsm","msg","acrbak"];
        const fileName = cleanFilePath(file.name);
        const fileExtension = fileName.split('.').pop().toLowerCase();

        for (const ext of validExtensions) {
            if (fileExtension === ext.toLowerCase()) {
                isValid = true;
                break;
            }
        }

        if (!isValid) {
            errMsg = `Files with extension <b>"${fileExtension}"</b> are not allowed.<br />`;
            errMsg += "You can upload files with the following extensions only:";
            errMsg += " " + validExtensions.map(ext => "." + ext).join(", ");
        }

        return errMsg;
    }

    function cleanFilePath(path, allowRootPath = false, allowUNCPath = false) {
        const pathSeparator = "/";
        const unc = pathSeparator + pathSeparator;
        const driveRootRegex = /^[a-zA-Z]:[\\/]/;
        const invalidChars = /[<>:"|?*\x00-\x1F]/g;
        const wildcards = /[*?]/g;
        const pathTransversal = /(^\.\.\/|(?<=\/)\.\.\/)/g;

        path = path.replace(invalidChars, "").replace(wildcards, "");

        let clean = false;
        let limit = 0;

        if (allowRootPath) {
            do {
                let dr = "";
                let cxPath = path;

                if (!allowUNCPath && cxPath.startsWith(unc)) {
                    cxPath = cxPath.substring(unc.length);
                }

                const drMatch = cxPath.match(driveRootRegex);
                if (drMatch) {
                    cxPath = cxPath.substring(drMatch[0].length);
                    dr = drMatch[0];
                }

                cxPath = dr + cxPath.replace(/:/g, "");

                if (path === cxPath) {
                    clean = true;
                } else {
                    path = cxPath;
                }

                if (++limit === 100) throw new Error("File path contains too many invalid characters");
            } while (!clean);
        } else {
            do {
                let cxPath = path;

                if (!allowUNCPath && cxPath.startsWith(unc)) {
                    cxPath = cxPath.substring(unc.length);
                }

                if (cxPath.startsWith(pathSeparator)) {
                    cxPath = cxPath.substring(1);
                }

                cxPath = cxPath.replace(driveRootRegex, "").replace(/:/g, "");

                if (path === cxPath) {
                    clean = true;
                } else {
                    path = cxPath;
                }

                if (++limit === 100) throw new Error("File path contains too many invalid characters");
            } while (!clean);
        }

        path = path.replace(pathTransversal, "");

        return path;
    }

    async function populateClaimAttachmentsTable() {
        $(".divUploading").show();

        claimsAttachments = await getAttachmentsFromDB();


        const hasAttachments = Array.isArray(claimsAttachments) && claimsAttachments.length > 0;


        $(".divUploading").hide();

        if (!hasAttachments) {
            $("#ctl00_MainContent_uc5SubmitClaim_ucClaimAttachment_UploadAttachments_pnlFileUpload").show();
            $("#ctl00_MainContent_uc5SubmitClaim_ucClaimAttachment_UploadAttachments_btnSend").hide();
            $("#ctl00_MainContent_uc5SubmitClaim_ucClaimAttachment_UploadAttachments_btnClear").hide();
            return;
        }

        $("#ctl00_MainContent_uc5SubmitClaim_ucClaimAttachment_UploadAttachments_btnSend").show();
        $("#ctl00_MainContent_uc5SubmitClaim_ucClaimAttachment_UploadAttachments_btnClear").show();

        if (claimsAttachments.length >= 10) {
            $("#ctl00_MainContent_uc5SubmitClaim_ucClaimAttachment_UploadAttachments_pnlFileUpload").hide();
        } else {
            $("#ctl00_MainContent_uc5SubmitClaim_ucClaimAttachment_UploadAttachments_pnlFileUpload").show();
        }


        const rows = claimsAttachments.map((attachment, index) => {
            const rowClass = index % 2 === 0 ? "gridViewRow" : "gridViewAltRow";

            return `
        <tr class="${rowClass}">
            <td><img src="../Images/file-icon.jpg" alt="File Icon" height="20" width="20" onclick="downloadFile('${attachment.DocumentName}')" style="cursor: pointer;" /></td>
            <td>${attachment.Document_ID}</td>
            <td>${attachment.Document_Service_Name}</td>
            <td>${attachment.OUTBOUND_DOCUMENT_UPLOAD_IDENTIFIER}</td>
            <td><button class="btn btn-danger" style="font-weight:bold;width:90px;" onclick="removeAttachment(${attachment.OutBound_Document_Uploads_ID})">Delete</button></td>
        </tr>`;
        });

        tableBody.append(rows.join(""));
    }

    async function getAttachmentsFromDB() {

        const result = isClmDelted(claimNumber)

        const hasNoData = !result || !result.Tables || result.Tables.length === 0 || result.Tables[0].Rows.length === 0;

        if (hasNoData) {
            const requestData = { medicaidId: medicaidId, claimNumber: claimNumber, claimType : claimType };

            try {
                return await CommonAjaxCall({
                    url: webApiAttachments + "GetAttachmentsByMedicaid",
                    method: 'GET',
                    headers: getHeaders(),
                    data: requestData
                });
            } catch (error) {
                $(".divUploading").hide();
                console.error("Failed to retrieve attachments:", error);
                return null;
            }
        }
    }
async function isClmDelted(claimNumber) {
    const requestData = { claimNumber: claimNumber };

    try {
        return await CommonAjaxCall({
            url: webApiAttachments + "CheckClaimIsDelet",
            method: 'GET',
            headers: getHeaders(),
            data: requestData
        });
    } catch (error) {
        $(".divUploading").hide();
        console.error("Failed to retrieve attachments:", error);
        return null;
    }
}
