var validFileExtensions = "doc,docx,pdf,xls,XLS,xlsm,xlsx,ppt,pptx,mdi,jpe,zip,txt,jpg,jpeg,png,gif,bmp,tif,tiff,pi,ec,zip,csv,xlsm,msg,acrbak";

var PriorAttachmentUpload = "ctl00_MainContent_uc1SubmitPriorAuthorization_PriorAttachmentUpload";
var priorDentalAttachmentUpload = "ctl00_MainContent_uc1SubmitPriorAuthorization_priorDentalAttachmentUpload";
var ddlPriorDentalAuthDocType = `ctl00_MainContent_uc1SubmitPriorAuthorization_ddlPriorDentalAuthDocType`;
var txtPriorDentalAttachmentNote = `ctl00_MainContent_uc1SubmitPriorAuthorization_txtPriorDentalAttachmentNote`;

let fileNameToAPI = '';

$(document).ready(function () {
    alert("Attchments JS file Loaded")
});

function addDentalAttachmentOnButtonClick() {

    debugger;
    console.log('addDentalAttachmentOnButtonClick called');

    var claimType = $(".rblClaimType input:checked").val();
    var isValid = validateDentalFileAndUploadToS3(claimType === "dental");

    //if (isValid) {
    //    uploadDentalAttachment();
    //}

    return isValid;
}


function validateDentalFileAndUploadToS3(isDental) {
    var spanId = isDental ? "#spanDentalAttachment" : "#spanAttachment";
    var fileInputId = isDental ? priorDentalAttachmentUpload : PriorAttachmentUpload;

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

    var selectedValue = $("#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlPriorDentalAuthDocType").val();
    if (!selectedValue || selectedValue.trim() === "") {
        $("#spanddlDentalDocumentType").show();
        return false;
    } else {
        $("#spanddlDentalDocumentType").hide();
    }

    let timestamp = new Date().toLocaleTimeString('it-IT').replace(/:/g, '');
    $('#hdnTime').val(timestamp);
    var destinationPayerID = $("#txtDestinationpayerID").val();
    $("#tradingPartnerID").val(destinationPayerID);

    let fileName = `${attachmentFileName}${$("#hdnTime").val()}--${attachmentFileNameSuffix}--${$("#tradingPartnerID").val()}.${file.name.split('.').pop()}`;
    fileNameToAPI = fileName;


    claimType === 'dental' ? $(".divDentalUploading").show() : $(".divUploading").show();

    getDentalAttachmentPresignUrl(fileName, file.type).then(response => {
        console.log(response);
        if (response) {
            addAttachmentToS3(response);
        }
    });

    return true;
}

function getDentalAttachmentPresignUrl(fileName, contentType) {

    var APIToken = $("[id*=hdnAccessToken]").val();

    return $.ajax({
        type: "GET",
        url: webApiAttachments + "GetPresignUrl",
        headers: {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
            "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
            "Authorization": "Bearer " + APIToken
        },
        data: { fileName: fileName, contentType: contentType },
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    })
        .then(response => {
            return response;
        })
        .fail(error => {
            console.error("Error fetching Presigned URL:", error);
            return null;
        });

}

function addAttachmentToS3(response) {

    debugger;

    var claimType = $(".rblClaimType input:checked").val();

    let fileInputId = claimType === 'dental' ? priorDentalAttachmentUpload : PriorAttachmentUpload;
    var file = document.getElementById(fileInputId).files[0];

    if (!file) {
        alert("No file selected. Please choose a file.");
        return;
    }

    var contentType = file.type;

    $.ajax({
        type: 'PUT',
        headers: {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Methods": "POST, GET, PUT, OPTIONS",
            "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With"
        },
        url: response.preSignedUrl,
        contentType: contentType,
        processData: false,
        async: false,
        data: file,
        crossDomain: true,
        success: function (data, status, xhr) {

            alert("Data:" + JSON.stringify(data));
            alert("status:" + status);
            console.log("Upload success");
            alert("Upload File to S3 Bucket");

            //__doPostBack("btnPAPriorAdd", "addAttachment");
            if (claimType === 'dental') {
                uploadDentalAttachment();
            }
        },
        error: function () {
            $(".divUploading").hide();
            alert("File not uploaded, please retry.");
        }
    });
}


//Check this TODO
function uploadDentalAttachment() {
    $("#ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalUploadErrMsg").text("");
    $("#ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalDocTypeErrMsg").text("");

    // Check for attachment limit
    if ($("#tblDentalAttachment tbody tr").length === 10) {
        $("#ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalAttachmentErrorMsg").show().text("Maximum of 10 attachments can be submitted");
        return;
    }

    // Medicaid Billing Number validation
    if ($("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtMedicaidBillingNumber").val().trim() === "") {
        $("#ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalAttachmentErrorMsg").show().text("Medicaid Billing Number is required");
        return;
    }

    // Document type validation
    if ($("#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlPriorDentalAuthDocType").prop("selectedIndex") === 0) {
        $("#ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalDocTypeErrMsg").show().text("Document type is required");
    } else {
        $("#ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalDocTypeErrMsg").hide().text("");
    }

    // Stop execution if errors are visible
    if ($("#ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalDocTypeErrMsg").is(":visible") || $("#ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalUploadErrMsg").is(":visible")) {
        return;
    }

    // Validate attachment count asynchronously
    validateAttachment("DentalProf", function (result) {
        if (result) {
            console.log("Validation failed: Too many attachments.");
            return;
        } else {
            console.log("Validation passed: You can add attachments.");

            // Proceed with the upload process AFTER validation success
            performUploadDentalAttachment();
        }
    });
}

// Function to handle actual upload after validation
function performUploadDentalAttachment() {
    debugger;
    var attachmentPayload = prepareSubmitAttachmentPayload();

    console.log("Uploading attachment...");

    $.ajax({
        type: "POST",
        url: webApiAttachments + "SavePAUploadedFileData",
        headers: getHeaders(),
        data: attachmentPayload,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            debugger;
            alert("File Uploaded to document store" + response.documentId);
            //Display in table and add to hidden field to use it for saving to PDMS DB
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Upload failed:", textStatus, errorThrown);
            alert("Something went wrong!");
        }
    });
}




function validateAttachment(paType, callback) {
    var claimType = $(".rblClaimType input:checked").val();
    var medicaidId = $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtProMedicaidIDORD').val();

    getPriorAuthAttachmentCount(claimType, medicaidId).then(function (response) {
        if (response) {
            let attachmentCount = response.attachmentCount;
            let status = false;

            if (attachmentCount >= 10) {
                if (paType === "Institutional") {
                    $("#ctl00_MainContent_uc1SubmitPriorAuthorization_lblAttachmentErrorMsg").show().text("Maximum 10 attachments can be submitted.");
                    status = true;
                } else if (paType === "DentalProf") {
                    $("#ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalAttachmentErrorMsg").show().text("Maximum 10 attachments can be submitted.");
                    status = true;
                }
            } else {
                if (paType === "Institutional") {
                    $("#ctl00_MainContent_uc1SubmitPriorAuthorization_lblAttachmentErrorMsg").hide().text("");
                } else if (paType === "DentalProf") {
                    $("#ctl00_MainContent_uc1SubmitPriorAuthorization_lblDentalAttachmentErrorMsg").hide().text("");
                }
                status = false;
            }

            // Execute the callback with the result
            if (typeof callback === "function") {
                callback(status);
            }
        }
    }).catch(function (error) {
        console.error("Error fetching attachment count:", error);
        if (typeof callback === "function") {
            callback(false);
        }
    });
}

function getPriorAuthAttachmentCount(claimType, medicaidId) {
    return $.ajax({
        type: "GET",
        url: webApiAttachments + "PriorAuthAttachmentCount",
        headers: getHeaders(),
        data: { claimType: claimType, medicaidId: medicaidId },
        contentType: "application/json; charset=utf-8",
        dataType: "json"
    })
        .then(response => {
            debugger;
            return response;
        })
        .fail(error => {
            console.error("Error fetching PriorAuthAttachment Count:", error);
            return null;
        });
}

function UploadPriorAuthAttachment() {

    debugger;

    $("#lblAttStatusMsg").text("");
    $("#lblAttErrMsg").text("");

    let docID = 0;

    let fileInput = document.getElementById("ctl00_MainContent_uc1SubmitPriorAuthorization_priorDentalAttachmentUpload");
    let file = fileInput.files[0]; // Get the first selected file

    if (!file) {
        $("#lblAttErrMsg").text("Unable to find posted file.");
        return docID;
    }


    if (!file.name.trim()) {
        $("#lblAttErrMsg").text("Select a file for upload.");
        return docID;
    }

    //if (!destinationPath || destinationPath.trim() === "") {
    //    $("#lblAttErrMsg").text("ERROR - DestinationPath not defined. Contact system administrator.");
    //    return docID;
    //}

    if (!validFileExtensions || validFileExtensions.trim() === "") {
        $("#lblAttErrMsg").text("ERROR - ValidFileExtensions must have a value. Contact system administrator.");
        return docID;
    }

    // Validate file extension
    let fileExtension = file.name.split('.').pop().toLowerCase();
    if (!validFileExtensions.includes(fileExtension)) {
        $("#lblAttErrMsg").text("Invalid file extension. Allowed extensions: " + validFileExtensions);
        return docID;
    }

    // Validate file size
    let maxFileSizeBytes = maxFileSizeMB * 1000 * 1024;
    if (file.size < 1) {
        $("#lblAttErrMsg").text("File cannot be 0Kb.");
        return docID;
    }

    if (file.size > maxFileSizeBytes) {
        $("#lblAttErrMsg").text(`File cannot be more than ${maxFileSizeBytes.toLocaleString()} bytes (${maxFileSizeMB} MB) in size.`);
        return docID;
    }

    // Validate special characters in filename
    let validFileNamePattern = /^[a-zA-Z0-9_.-]+$/;
    if (!validFileNamePattern.test(file.name)) {
        $("#lblAttErrMsg").text(`The file name can contain letters, numbers, dot(.), underscore(_) and hyphen(-): ${file.name}. Please remove special characters before upload.`);
        return docID;
    }


}



function saveRecordToPriorAuthAttachment() {
    //our DB
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

function prepareSubmitAttachmentPayload() {

    const claimType = $(".rblClaimType input:checked").val();
    var fileInputId = claimType === 'Dental' ? priorDentalAttachmentUpload : PriorAttachmentUpload;
    var file = document.getElementById(fileInputId).files[0];

    var originalDocumentName = file.name;

    if (claimType === "Dental") {
        var claimTypeData = getClaimTypeData();

        const data = {
            fileName: fileNameToAPI,
            tradingPartnerID: $("#txtDestinationpayerID").val(),
            uuid: crypto.randomUUID(), // Generates a unique ID similar to C#'s Guid.NewGuid()
            payerRequested: "Yes",
            paNumber: $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtPANumber2").val(),
            memberId: $("#ctl00_MainContent_uc1SubmitPriorAuthorization_txtMedicaidBillingNumber").val(),
            providerId: $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtProMedicaidIDORD').val(),
            providerNPI: $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtNPI').val(),
            claimNumber: "",
            originalDocumentName: originalDocumentName,
            toSend: false,
            receiverId: $("#ctl00_MainContent_uc1SubmitPriorAuthorization_ddlSubCapitaPayerIDs").val() || '',
            claimTypeId: claimTypeData.claimTypeId,
            documentTypeId: claimTypeData.documentTypeId,
            claimType: claimType,
            timestamp: $('#hdnTime').val(timestamp),
            medicaidId: $('#ctl00_MainContent_uc1SubmitPriorAuthorization_txtProMedicaidIDORD').val()
        };
    }
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

function addAttachmentRow(attachment) {
    const attachments = JSON.parse($("#hfAttachmentsJson").val() || "[]");
    attachments.push(attachment);
    $("#hfAttachmentsJson").val(JSON.stringify(attachments));

    const row = `
        <tr>
            <td>${attachment.fileName}</td>
            <td>${attachment.fileType}</td>
            <td>${attachment.notes}</td>
            <td>
                <a href="${attachment.s3Url}" target="_blank">Download</a>
                <a href="#" onclick="deleteAttachment('${attachment.fileName}'); return false;">Delete</a>
            </td>
        </tr>`;
    $("#tblAttachments tbody").append(row);
}

//function uploadDentalAttachment() {

//    debugger;
//    let timestamp = new Date().toLocaleTimeString('it-IT').replace(/:/g, '');
//    $('#hdnTime').val(timestamp);
//    var destinationPayerID = $("#txtDestinationpayerID").val();
//    $("#tradingPartnerID").val(destinationPayerID);
//    //$("#tradingPartnerID").val(document.getElementById('<%=txtDestinationpayerID.ClientID%>').value);

//    let claimType = $(".rblClaimType input:checked").val();
//    let fileInputId = claimType === 'dental' ? priorDentalAttachmentUpload : PriorAttachmentUpload;
//    let file = document.getElementById(fileInputId).files[0];

//    if (!file) {
//        alert('No file selected.');
//        return false;
//    }
//    debugger;
//    //let fileName = `<%=AttachmentFileName%>${$("#hdnTime").val()}--<%=AttachmentFileNameSuffix%>--${$("#tradingPartnerID").val()}.${file.name.split('.').pop()}`;
//    let fileName = `${attachmentFileName}${$("#hdnTime").val()}--${attachmentFileNameSuffix}--${$("#tradingPartnerID").val()}.${file.name.split('.').pop()}`;
//    let payLoad = JSON.stringify({ fileName: fileName, contentType: file.type });

//    // Show upload progress
//    claimType === 'dental' ? $(".divDentalUploading").show() : $(".divUploading").show();

//    if (file.size > 10485760) {
//        alert('File size exceeds max 10MB limit.');
//        claimType === 'dental' ? $(".divDentalUploading").hide() : $(".divUploading").hide();
//        return false;
//    }

//    // Generate presigned URL

//    //getDentalAttachmentPresignUrl(fileName, file.type).then(preSignedUrl => {
//    //    console.log(preSignedUrl);
//    //    if (preSignedUrl) {
//    //        addAttachment(preSignedUrl);
//    //    }
//    //});


//    return false;
//}