const UploadAttachments = "ctl00_MainContent_UploadAttachments_UploadAttachments";
const txtRecipientID = 'ctl00_MainContent_UploadAttachments_txtRecipientID';
const ddlPriorAuthDocType = `ctl00_MainContent_UploadAttachments_ddlPriorAuthDocType`;
const txtComments = 'ctl00_MainContent_UploadAttachments_txtComments';


let fileNameToAPI = '';
let originalFileNameToAPI = '';
let standaloneAttachments = [];


$(document).ready(function () {
    console.log("Populae attachments on page load ");
    // removeUnSentDocumentsOnPageLoad();
    populateStandaloneAttachmentsTable();
});


async function addAttachmentOnButtonClickEvent() {
    console.log("add Attachment On ButtonClickEvent");
    try {
        $(".divUploading").show();

        const isValid = validateStandaloneAttachment();
        if (isValid) {
            console.log("validateStandaloneAttachment successfull ");
            uploadAttachmentNew();
        }
        $(".divUploading").hide();
        return isValid;
    } catch (error) {
        console.error("Error validating file upload:", error);
        $(".divUploading").hide();
        return false;
    }
}

async function uploadAttachmentNew() {
    let timestamp = new Date().toLocaleTimeString('it-IT').replace(':', '').replace(':', '');
    $('#hdnTime').val(timestamp);

    const fileInput = document.getElementById(UploadAttachments);
    const file = fileInput?.files[0];

    const tradingPatnerID = $("#tradingPatnerID").val();

    let fileName = `${attachmentFileName}${$("#hdnTime").val()}--${attachmentFileNameSuffix}--${tradingPatnerID}.${file.name.split('.').pop()}`;
    fileNameToAPI = fileName;
    originalFileNameToAPI = file.name;

    $(".divUploading").show();

    $("#ctl00_MainContent_UploadAttachments_btnSend").prop("disabled", false);

    $("#lblAttachmentErrorMsg").hide();

    try {
        console.log("getAttachmentPresignUrl started ");
        const response = await getAttachmentPresignUrl(fileName, file.type, "PUT");
        console.log(response);

        if (response) {
            console.log("getAttachmentPresignUrl successfull ");
            const isUploaded = await addAttachmentToS3Bucket(response);
            if (isUploaded === '') {
                console.log("addAttachmentToS3Bucket successfull ");
                await saveAttachmentToDB();
            }
        }
        $(".divUploading").hide();
    }
    catch (error) {
        console.error("Failed to get Presigned URL:", error);
        $(".divUploading").hide();
    }
}
function validateStandaloneAttachment() {
    let isValid = true;
    // Hide all error spans
    const errorSpans = [
        "#spanAttachment", "#spantxtRecipientID", "#spantxtRecipientIDMinimum",
        "#spanddlDocumentType", "#spantxtICN", "#spantxtPANumber",
        "#spanddlDestinationPayerID", "#spanddlTransactionTypeID",
        "#spantxtComments", "#spanddlPrimaryDestinationPayer"
    ];
    errorSpans.forEach(span => $(span).hide());

    // Validate dropdowns
    if ($(".ddlPrimaryDestinationPayer :selected").val() === "") {
        $("#spanddlPrimaryDestinationPayer").show();
        isValid = false;
    }

    if ($(".ddlDestinationPayerID :selected").val() === "") {
        $("#spanddlDestinationPayerID").show();
        isValid = false;
    }

    if ($(".ddlPriorAuthDocType :selected").val() === "") {
        $("#spanddlDocumentType").show();
        isValid = false;
    }

    // Validate text inputs
    const recipientID = $(".txtRecipientID").val();
    if (!recipientID) {
        $("#spantxtRecipientID").show();
        isValid = false;
    } else if (recipientID.length < 12) {
        $("#spantxtRecipientIDMinimum").show();
        isValid = false;
    }

    if (!$(".txtComments").val()) {
        $("#spantxtComments").show();
        isValid = false;
    }

    // Validate transaction type-specific fields
    const transactionType = $(".ddlTransactionTypeID :selected").text();
    const transactionVal = $(".ddlTransactionTypeID :selected").val();

    if (transactionVal !== "") {
        if (transactionType === "Prior Auth") {
            const paNumber = $(".txtPANumber").val();
            if (!paNumber) {
                $("#spantxtPANumber").show();
                isValid = false;
            }
        } else {
            const icn = $(".txtICN").val();
            if (!icn) {
                $("#spantxtICN").show();
                isValid = false;
            }
        }
    }

    // Validate file upload
    const fileInput = document.getElementById(UploadAttachments);
    const file = fileInput.files[0];

    if (!file) {
        $("#spanAttachment").show().text("Attachment is required");
        isValid = false;
    } else {
        if (file.size > 10485760) {
            $("#spanAttachment").show().text("File size exceeds max 10MB limit");
            isValid = false;
        } else if (file.size < 1) {
            $("#spanAttachment").show().text("File size cannot be 0kb");
            isValid = false;
        } else {
            const fileExtension = file.name.substring(file.name.lastIndexOf(".")).toLowerCase();
            if (!FileValidator.isExtensionAllowed(fileExtension)) {
                $("#spanAttachment").show().text("Not a valid file type");
                isValid = false;
            }
        }
    }

    return isValid;
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

async function populateStandaloneAttachmentsTable() {
    $(".divUploading").show();

    standaloneAttachments = await getAttachmentsFromDB();

    const tableBody = $("#tblAttachmentBody");
    tableBody.empty();

    const hasAttachments = Array.isArray(standaloneAttachments) && standaloneAttachments.length > 0;
    $("#tblAttachment").toggle(hasAttachments);

    $(".divUploading").hide();

    if (!hasAttachments) {
        $("#ctl00_MainContent_UploadAttachments_pnlFileUpload").show();
        $("#ctl00_MainContent_UploadAttachments_btnSend").hide();
        $("#ctl00_MainContent_UploadAttachments_btnClear").hide();
        return;
    }

    $("#ctl00_MainContent_UploadAttachments_btnSend").show();
    $("#ctl00_MainContent_UploadAttachments_btnClear").show();

    if (standaloneAttachments.length >= 10) {
        $("#ctl00_MainContent_UploadAttachments_pnlFileUpload").hide();
    } else {
        $("#ctl00_MainContent_UploadAttachments_pnlFileUpload").show();
    }

    const transactionType = $(".ddlTransactionTypeID option:selected").text().trim();

    // Show/hide headers based on transaction type
    if (transactionType === "Prior Auth") {
        $("#tblAttachment th:nth-child(2)").hide(); // Hide ICN header
        $("#tblAttachment th:nth-child(3)").show(); // Show PA NUMBER header
    } else {
        $("#tblAttachment th:nth-child(2)").show(); // Show ICN header
        $("#tblAttachment th:nth-child(3)").hide(); // Hide PA NUMBER header
    }

    const rows = standaloneAttachments.map((attachment, index) => {
        const rowClass = index % 2 === 0 ? "gridViewRow" : "gridViewAltRow";

        return `
        <tr class="${rowClass}">
            <td><img src="../Images/file-icon.jpg" alt="File Icon" height="20" width="20" onclick="downloadFile('${attachment.DocumentName}')" style="cursor: pointer;" /></td>
            <td style="${transactionType === 'Prior Auth' ? 'display:none;' : ''}">${attachment.Claim_number}</td>
            <td style="${transactionType !== 'Prior Auth' ? 'display:none;' : ''}">${attachment.PA_NUMBER}</td>
            <td>${attachment.Member_ID}</td>
            <td>${attachment.DOCUMENT_TYPE_DESC}</td>
            <td>${attachment.OUTBOUND_DOCUMENT_UPLOAD_IDENTIFIER}</td>
            <td><button class="btn btn-danger" style="font-weight:bold;width:90px;" onclick="removeAttachment(${attachment.OutBound_Document_Uploads_ID});return false;">Delete</button></td>
        </tr>`;
    });

    tableBody.append(rows.join(""));
}

async function removeUnSentDocumentsOnPageLoad() {

    var pageKey = "PageLoaded";

    $(".divUploading").show();

    try {
        if (!sessionStorage.getItem(pageKey)) {
            const attachments = await getAttachmentsFromDB();
            const hasAttachments = Array.isArray(attachments) && attachments.length > 0;

            if (hasAttachments) {
                const documentIds = attachments.map(item => item.OutBound_Document_Uploads_ID);

                const payload = {
                    DocumentIds: documentIds
                };

                const requestUrl = `${webApiAttachments}DeleteUnSentAttachment`;

                const response = await CommonAjaxCall({
                    url: requestUrl,
                    method: 'POST',
                    data: payload,
                    headers: getHeaders()
                });

                sessionStorage.setItem(pageKey, "true");
            }
            populateStandaloneAttachmentsTable();
        }
    }
    catch (error) {
        console.error("Error removing unsent documents:", error);
    } finally {
        $(".divUploading").hide();
    }
}

async function removeAttachment(documentId) {
    console.log("removeAttachment started ");

    if (!confirm("Are you sure you want to delete this record?")) {
        return;
    }
    $(".divUploading").show();
    try {
        const requestUrl = `${webApiAttachments}DeleteAttachments/${documentId}`;
        const response = await CommonAjaxCall({
            url: requestUrl,
            method: 'DELETE',
            headers: getHeaders()
        });

        populateStandaloneAttachmentsTable();
        $(".divUploading").hide();
        console.log("Delete response:", response);

        console.log("removeAttachment completed ");

    } catch (error) {
        console.error("Failed to delete attachment:", error);
        $(".divUploading").hide();
    }
}

async function removeUnSentAttachment(documentId) {

    console.log("removeUnSentAttachment started ");

    $(".divUploading").show();
    try {
        const requestUrl = `${webApiAttachments}DeleteUnSentAttachment/${documentId}`;
        const response = await CommonAjaxCall({
            url: requestUrl,
            method: 'POST',
            headers: getHeaders()
        });
        $(".divUploading").hide();
        
        console.log("removeUnSentAttachment completed ");

    } catch (error) {
        console.error("Failed to delete attachment:", error);
        $(".divUploading").hide();
    }
}

async function downloadFile(fileName) {
    try {
        $(".divUploading").show();

        const response = await fetch(webApiAttachments + "download?" + new URLSearchParams({ fileName }), {
            method: "GET",
            headers: getHeaders()
        });
        $(".divUploading").hide();

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
        $(".divUploading").hide();
        console.error("Download failed:", error);
        console.error("Failed to download attachment.");
    }
}

async function getAttachmentsFromDB() {

    const medicaidId = $('#ctl00_MainContent_ucRegProgressBar_lblProMedicaidID2').text();
    console.log("getAttachmentsFromDB  started");
    const paNumber = $(".txtPANumber").val() || "0";
    const icn = $(".txtICN").val();
    console.log("PA Number" + paNumber)

    const requestData = { medicaidId, icn, paNumber };
    const requestUrl = `${webApiAttachments}GetAttachmentsByMedicaid`;

    try {
        return await CommonAjaxCall({
            url: requestUrl,
            method: 'GET',
            headers: getHeaders(),
            data: requestData
        });
    } catch (error) {
        $(".divUploading").hide();
        console.error("Failed to retrieve attachments:", error);
        return null;
    }
    console.log("getAttachmentsFromDB  Ended");
}

async function saveAttachmentToDB() {
    console.log("saveAttachmentToDB  started");

    const transactionType = $(".ddlTransactionTypeID :selected").text();
    const transactionVal = $(".ddlTransactionTypeID :selected").val();
    const receiverID = $.trim($("#ddlDestinationPayerID option:selected").text()) ? $("#ddlDestinationPayerID").val() : "";

    const attachmentPayload = {
        MedicaidId: $('#ctl00_MainContent_ucRegProgressBar_lblProMedicaidID2').text(),
        EDITransactionTypeId: transactionType === 'Prior Auth' ? "2" : "1",
        PaNumber: transactionType === 'Prior Auth' ? $(".txtPANumber").val() || "0" : "0",
        MemberId: $(".txtRecipientID").val(),
        ClaimTypeId: transactionVal,
        ClaimNumber: $(".txtICN").val(),
        ProviderId: $('#ctl00_MainContent_ucRegProgressBar_lblProMedicaidID2').text(),
        ReceiverId: receiverID,
        DocumentTypeId: $(".ddlPriorAuthDocType :selected").val(),
        DocumentName: fileNameToAPI,
        OrginalDocumentName: originalFileNameToAPI, // gets filename
        Provider_NPI: $("#ctl00_MainContent_ucRegProgressBar_lblPRONPI2").text(),
        ProviderComments: $(".txtComments").val()
    };
    try {

        const requestUrl = `${webApiAttachments}SaveStandaloneAttachment`;
        const response = await CommonAjaxCall({
            url: requestUrl,
            method: 'POST',
            data: attachmentPayload,
            headers: getHeaders()
        });
        console.log("saveAttachmentToDB successfull ");
        populateStandaloneAttachmentsTable();
        resetData();

    } catch (error) {
        $(".divUploading").hide();
        console.error("Failed to save attachment:", error);
        return null;
    }
}
function resetData() {
    $("#ctl00_MainContent_UploadAttachments_UploadAttachments").val("");
    $(".ddlPriorAuthDocType").prop("selectedIndex", 0);
    $(".txtComments").val('');
    $("#spanAttachment, #spantxtComments, #spanddlDocumentType").hide();
    fileNameToAPI = '';
    originalFileNameToAPI = '';
}

async function updateDocumentStatuses() {

    try {
        $(".divUploading").show();

        const documentIds = standaloneAttachments.map(item => item.OutBound_Document_Uploads_ID);

        const payload = {
            DocumentIds: documentIds
        };

        const requestUrl = `${webApiAttachments}UpdateDocumentStatuses`;

        const response = await CommonAjaxCall({
            url: requestUrl,
            method: 'POST',
            data: payload,
            headers: getHeaders()
        });
        console.log("updateDocumentStatuses Successfull");

        populateStandaloneAttachmentsTable();

        $(".divUploading").hide();

    } catch (error) {
        $(".divUploading").hide();
        console.error("Failed to updateDocumentStatuses:", error);
        return null;
    }
}

async function clearAttachments(event) {

    $(".divUploading").show();

    try {

        const attachments = await getAttachmentsFromDB();
        const hasAttachments = Array.isArray(attachments) && attachments.length > 0;

        if (hasAttachments) {
            const documentIds = attachments.map(item => item.OutBound_Document_Uploads_ID);

            const payload = {
                DocumentIds: documentIds
            };

            const requestUrl = `${webApiAttachments}DeleteUnSentAttachment`;

            const response = await CommonAjaxCall({
                url: requestUrl,
                method: 'POST',
                data: payload,
                headers: getHeaders()
            });

        }
    }
    catch (error) {
        console.error("Error removing unsent documents:", error);
    } finally {
        $(".divUploading").hide();
      
        event.preventDefault();
        var urlpath = window.location.href.substring(0, window.location.href.lastIndexOf("/"));

        const transactionType = $(".ddlTransactionTypeID :selected").text();
        if (transactionType === "Prior Auth") {
            window.location.href = urlpath + "/SearchPriorAuthorization.aspx";
        } else {
            window.location.href = urlpath + "/SearchClaims.aspx";
        }

    }
}
function getHeaders() {
    const apiToken = $("[id*=hdnAccessToken]").val();
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
