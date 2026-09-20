$(function () {  
    loadProfessionalLicenses();
    loadBoardCertifications();
    loadCliaCertifications();
    loadDeaCertificates();
    loadCdsCertificates();
});

function loadProfessionalLicenses() {

    $("#medicairIdaccordian").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    $("#professionalLicenseAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    CredentialingService.getProfessionalLicensesByRegID(function (response) {
        renderProfessionalLicensesGrid(response);
    });
}

function renderProfessionalLicensesGrid(data) {

    $('#professional-license-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'LicenseTypeName', title: 'License Type', type: 'text', sortable: true },
                { key: 'LicenseNumber', title: 'License Number', type: 'text', sortable: true },
                { key: 'LicenseState', title: 'State', type: 'text', sortable: true },
                { key: 'IssueDate', title: 'Effective Date', type: 'date', sortable: true },
                { key: 'ExpirationDate', title: 'Expiration Date', type: 'date', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No Professional Licenses found.',
            idProperty: 'LicenseNumber',  // unique property

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

}

function loadBoardCertifications() {
    
    $("#boardCertificationAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    CredentialingService.getBoardCertificationsByRegID(function (response) {
        renderBoardCertificationsGrid(response);
    });
}

function renderBoardCertificationsGrid(data) {

    $('#board-certification-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,
            columns: [
                { key: 'BoardCertificationName', title: 'Board Certification', type: 'text', sortable: true },
                { key: 'BoardSpecialtyName', title: 'Board Speciality', type: 'text', sortable: true },
                { key: 'CertificationNumber', title: 'Certification Number', type: 'text', sortable: true },
                { key: 'EffectiveDate', title: 'Effective Date', type: 'date', sortable: true },
                { key: 'ExpirationDate', title: 'Expiration Date', type: 'date', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No Board Certifications found.',
            idProperty: 'CertificationNumber', // assumes unique; adjust if your data can be empty

            enableAllColumnSearch: false
            , enableColumnFilters: true
            , enableSorting: true

            , dateFormat: 'MM-DD-YYYY'
            , includeTime: false
        });
}

function loadCliaCertifications() {

    $("#cliaCertificationAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    CredentialingService.getCliaCertificationsByRegID(function (response) {
        renderCliaCertificationsGrid(response);
    });
}

function renderCliaCertificationsGrid(data) {

    $('#clia-certification-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'CliaNumber', title: 'CLIA Number', type: 'text', sortable: true },
                { key: 'CliaCertificationName', title: 'CLIA Certification Type', type: 'text', sortable: true },
                { key: 'EffectiveDate', title: 'Effective Date', type: 'date', sortable: true },
                { key: 'ExpirationDate', title: 'Expiration Date', type: 'date', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No CLIA Certifications found.',
            idProperty: 'CliaNumber',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

}

function loadDeaCertificates() {

    $("#deaCertificationAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    CredentialingService.getDeaCertificatesByRegID(function (response) {
        renderDeaCertificatesGrid(response);
    });
}
function renderDeaCertificatesGrid(data) {

    $('#dea-certificates-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'DeaNumber', title: 'DEA Number', type: 'text', sortable: true },
                { key: 'State', title: 'State', type: 'text', sortable: true },
                { key: 'EffectiveDate', title: 'Effective Date', type: 'date', sortable: true },
                { key: 'ExpirationDate', title: 'Expiration Date', type: 'date', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No DEA Certificates found.',
            idProperty: 'DeaNumber',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

}

function loadCdsCertificates() {

    $("#cdsLicenseAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    CredentialingService.getCdsCertificatesByRegID(function (response) {
        renderCdsCertificatesGrid(response);
    });
}
function renderCdsCertificatesGrid(data) {

    $('#cds-certificates-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'CdsNumber', title: 'CDS Number', type: 'text', sortable: true },
                { key: 'State', title: 'State', type: 'text', sortable: true },
                { key: 'EffectiveDate', title: 'Effective Date', type: 'date', sortable: true },
                { key: 'ExpirationDate', title: 'Expiration Date', type: 'date', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No CDS Certificates found.',
            idProperty: 'CdsNumber',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

}