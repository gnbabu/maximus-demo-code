$(document).ready(function () {
   
    var providerDocumentRequest = {
        regId: parseInt($("[id*=hdnRegId]").val(), 10),
        regPageTypeId: parseInt($("[id*=hdnRegStepId]").val(), 10),
        docSectionName: "", // optional, set when needed
        exclusions: $("[id*=hdnExclusions]").val(),
        showEducationWorkDocs: String($("[id*=hdnshowEducationWorkDocs]").val()).toLowerCase() === "true",
        userRole: $("[id*=hdnUserRole]").val()
    };

    CredentialingActivityService.getProviderDocuments(providerDocumentRequest, function (response) {
        renderProviderDocumentsGrid(response);
    });
});

function refreshProviderDocumentsGrid() {

    var providerDocumentRequest = {
        regId: parseInt($("[id*=hdnRegId]").val(), 10),
        regPageTypeId: parseInt($("[id*=hdnRegStepId]").val(), 10),
        docSectionName: "",
        exclusions: $("[id*=hdnExclusions]").val(),
        showEducationWorkDocs:
            String($("[id*=hdnshowEducationWorkDocs]").val()).toLowerCase() === "true",
        userRole: $("[id*=hdnUserRole]").val()
    };

    CredentialingActivityService.getProviderDocuments(
        providerDocumentRequest,
        function (data) {
            renderProviderDocumentsGrid(data);
        });
}


$(document).on("click", "#btnUploadAjax", function () {

    const fileInput = document.querySelector(".file-upload-input");

    if (!fileInput || !fileInput.files.length) {
        alert("Select a file");
        return;
    }

    const formData = new FormData();

    formData.append("file", fileInput.files[0]);
    formData.append("regId", $("[id*=hdnRegId]").val());
    formData.append("userId", $("[id*=hdnUserId]").val());

    $.ajax({

        url: window.WEB_API_URL + "Credentialing/UploadDocument",
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,

        success: function (response) {

            setMessage(
                "Your document has been uploaded successfully.",
                false);

            const fileInput = document.getElementById(
                "ctl00_MainContent_UploadDocProv_filUploadFile"
            );

            if (fileInput) {
                fileInput.value = null;
            }

            const uploadBtn = document.getElementById("btnUploadAjax");

            if (uploadBtn) {
                uploadBtn.disabled = true;
                uploadBtn.classList.add("disabled");
            }

            setTimeout(function () {
                setMessage("", false);
            }, 5000);

            refreshProviderDocumentsGrid();
        },

        error: function (xhr) {
            console.log(xhr.responseText);
            setMessage("Upload failed.", true);
        }
    });
});

function deleteProviderDocument(documentId) {

    if (!documentId) return;

    $.modalPlugin.confirm({
        title: 'Delete Document',
        message: 'Are you sure you want to delete this document?',
        okText: 'Yes',
        cancelText: 'No',
        width: '450px',

        onConfirm: function () {

            var deleteRequest = {
                documentId: documentId,
                userId: $("[id*=hdnUserId]").val()
            };

            CredentialingActivityService.deleteProviderDocument(deleteRequest, function (response) {

                var providerDocumentRequest = {
                    regId: parseInt($("[id*=hdnRegId]").val(), 10),
                    regPageTypeId: parseInt($("[id*=hdnRegStepId]").val(), 10),
                    docSectionName: "",
                    exclusions: $("[id*=hdnExclusions]").val(),
                    showEducationWorkDocs: String($("[id*=hdnshowEducationWorkDocs]").val()).toLowerCase() === "true",
                    userRole: $("[id*=hdnUserRole]").val()
                };

                CredentialingActivityService.getProviderDocuments(providerDocumentRequest, function (data) {
                    renderProviderDocumentsGrid(data);
                });

            }, function (error) {
                $("#<%= lblStatusMsg.ClientID %>").text("Error deleting document.");
                console.error("Delete failed:", error);
            });
        },

        onCancel: function () {
            console.log("User cancelled delete.");
        }
    });
}


function renderProviderDocumentsGrid(data) {

    $('#provider-documents-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data || [],

            columns: [
                {
                    key: 'Name',
                    title: 'Name',
                    type: 'text',
                    sortable: true,
                    cellTemplate: function (row) {
                        if (!row.OnBaseDocumentId) {
                            return '<span class="text-danger">Not Found</span>';
                        }
                        return 'Credentialing';
                    }
                },
                {
                    key: 'FileName',
                    title: 'File Name',
                    type: 'text',
                    sortable: true,
                    // Using cellTemplate to render link or 'Not Found'
                    cellTemplate: function (row) {
                        if (!row.OnBaseDocumentId) {
                            return '<span class="text-danger">Not Found</span>';
                        }
                        var docId = row.OnBaseDocumentId;

                        var url = window.SHOW_FILES_URL.showFilesUrl +
                            '?docId=' + encodeURIComponent(docId) +
                            '&fileName=' + encodeURIComponent(row.FileName) +
                            '&mode=inline';

                        return '<a href="' + url + '" ' +
                            'target="_blank" ' +
                            'rel="noopener noreferrer" ' +
                            'aria-label="' + row.FileName + ' (opens in a new tab)" ' +
                            'class="text-primary">' +
                            row.FileName +
                            '</a>';
                    }
                },
                {
                    key: 'LastModifiedDateTime',
                    title: 'Date',
                    type: 'date',
                    sortable: true
                },
                {
                    key: 'Username',
                    title: 'Username',
                    type: 'text',
                    sortable: true,
                    render: function (value) {
                        return value || '-';
                    }
                },
                {
                    key: 'RoleName',
                    title: 'Role',
                    type: 'text',
                    sortable: true,
                    visible: false
                },
                {
                    title: 'Delete',
                    sortable: false,
                    cellTemplate: function (row) {
                        return `
                            <a href="javascript:void(0);"
                               class="delete-icon"
                               title="Delete"
                               onclick="deleteProviderDocument(${row.DocumentId})">
                                <span class="material-symbols-outlined"
                                      style="font-size:16px; color:#000;">
                                    delete
                                </span>
                            </a>`;
                    }
                }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No uploaded documents found.',
            idProperty: 'DocumentId',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });
}