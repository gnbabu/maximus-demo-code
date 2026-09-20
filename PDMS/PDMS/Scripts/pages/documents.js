$(function () {


    // Show upload section with animation
    $('#btnShowUpload').on('click', function () {
        $('#uploadButtonWrapper').fadeOut(150, function () {
            $('#uploadSection')
                .removeClass('d-none')
                .hide()
                .slideDown(200);
        });
    });

    // Trigger file picker
    $('#btnUpload').on('click', function () {
        $('#fileUpload').trigger('click');
    });

    // Hide upload section with animation
    $('#btnCancelUpload').on('click', function () {
        $('#uploadSection').slideUp(200, function () {
            $(this).addClass('d-none');
            $('#uploadButtonWrapper').fadeIn(150);
            $('#fileUpload').val(''); // reset file input
        });
    });




    // Initial load for the "professional" section
    $("#professionalLicenseAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    loadDocuments('Licenses', '#professional-license-documents-grid-container');

    $("#deaLicenseAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    loadDocuments('FederalDEA', '#dea-license-documents-grid-container');

    $("#cdsLicenseAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    loadDocuments('StateCDSNumber', '#cds-license-documents-grid-container');

    $("#boardCertificationAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    loadDocuments('BoardCertification', '#board-certification-documents-grid-container');

    $("#ecfmgCertificationAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    loadDocuments('ECFMGCertificate', '#ecfmg-certification-documents-grid-container');

    $("#w9Accordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    loadDocuments('W9Form', '#w9-documents-grid-container');

    $("#schoolDiplomaAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    loadDocuments('Education', '#school-diploma-documents-grid-container');

    $("#facilityLicenseAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    loadDocuments('NursingFacilityVentilator', '#facility-license-documents-grid-container');

    $("#insuranceAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    loadDocuments('Insurance', '#insurance-documents-grid-container');

    $("#workhistoryAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });
    loadDocuments('EmploymentHistory', '#work-history-documents-grid-container');
});


function loadDocuments(section, containerId) {
    CredentialingService.getDocuments(section, function (response) {

        response = response || [];
        for (var i = 0; i < response.length; i++) {
            response[i].section = section;
        }

        renderDocuments(containerId, response);

    }, function (err) {
        console.error('Failed to load documents:', err);
        renderDocuments(containerId, []);
    });
}

function renderDocuments(containerId, data) {
    // Keep a reference to the current data set to resolve actions by index
    $(containerId)
        .removeClass('d-none')
        .dataGrid({
            data: data,

            columns: [
                {
                    key: "DocumentType",
                    title: "Document Type",
                    type: "text",
                    sortable: false
                },
                {
                    key: "FileName",
                    title: "File Name",
                    type: "text",
                    // Using cellTemplate to render link or 'Not Found'
                    cellTemplate: function (row) {
                        if (!row.FileName) {
                            return '<span class="text-danger-accessible">Not Found</span>';
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
                    },
                    sortable: false
                },
                {
                    key: "Source",
                    title: "Source",
                    type: "text",
                    sortable: false
                },
                {
                    key: "UploadedOn",
                    title: "Uploaded On",
                    type: "date",
                    sortable: false
                },
                {
                    key: "Status",
                    title: "Status",
                    type: "text",
                    sortable: false,
                    // Render a badge based on status
                    cellTemplate: function (row) {
                        var cls = '';

                        if (row.Status === 'Required') {
                            cls = 'text-danger-accessible'; // red text only for Required
                        }

                        return '<span class="' + cls + '">' + (row.Status || '-') + '</span>';
                    }
                }
            ],

            // Match your education grid configuration
            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No document records found',
            idProperty: 'DocumentId', // assuming unique within a section

            enableAllColumnSearch: false,
            enableColumnFilters: false,
            enableSorting: false,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });



    // Delete
    $(containerId).on('click.action-delete', '[data-action="delete"]', function (e) {
        e.preventDefault();
        var documentId = $(this).data('document-id');
        var documentType = $(this).data('document-type')
        console.log('Delete documentId:', documentId);
        console.log('Document Type:', documentType);
    });
}
