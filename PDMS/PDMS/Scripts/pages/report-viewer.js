const containerId = '#report-grid-container';
let currentRequest = null;

$(function () {

    const params = new URLSearchParams(window.location.search);
    console.log(params);
    const reportName = params.get('reportName');
    const startDateString = params.get('startDate');
    const endDateString = params.get('endDate');


    if (!reportName || !startDateString || !endDateString) {
        console.error('Missing report parameters');
        return;
    }

    $("#report-title").text(reportName);
    const today = moment().format("MM/DD/YYYY");
    $("#run-date").text(today);
    // Set from/to dates
    $("#from-date").text(startDateString || "N/A");
    $("#to-date").text(endDateString || "N/A");



    const request = {
        reportName: reportName,
        startDate: convertToIso(startDateString),
        endDate: convertToIso(endDateString, true)
    };

    currentRequest = request;
    loadReports(request);
});


function loadReports(request) {
    $(containerId).empty();
    showClientLoader();
    switch (request.reportName) {

        case 'Credentialing Clean File Report':
            loadProviderCredentialingCleanFileReport(request);
            break;

        case 'Credentialing Flagged Files Report':
            loadProviderCredentialingFlaggedFilesReport(request);
            break;

        case 'Provider Re-credentialing Due Report':
            loadProviderReCredentialingDueReport(request);
            break;

        case 'Credentialing – Provider Contracting > 36 Months Report':
            loadProviderCredentialingGreaterThan36MonthsReport(request);
            break;

        case 'Credentialing – Clean and Flagged Files Summary Report':
            loadCredentialingCleanAndFlaggedFilesReport(request);
            break;

        default:
            console.error('Unknown report name:', request.reportName);
            break;
    }
}

function loadProviderReCredentialingDueReport(request) {

    ReportsService.getProviderReCredentialingDueReport(
        request.startDate,
        request.endDate,
        function (response) {
            renderProviderReCredentialingDueReport(response);
            hideClientLoader();
        }
    );
}

function renderProviderReCredentialingDueReport(data) {

    $(containerId)
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'ProviderName', title: 'Provider Name', type: 'text', sortable: true },
                { key: 'ProviderSpecialty', title: 'Provider Specialty', type: 'text', sortable: true },
                { key: 'NPI', title: 'Provider NPI', type: 'text', sortable: true },
                { key: 'ReCredentialingDueDate', title: 'Provider Re - Cred Due Date', type: 'date', sortable: true },
                { key: 'Status', title: 'Status', type: 'text', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No re‑credentialing due providers found.',
            idProperty: 'NPI',
            enableRowSelection: false,
            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,
            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });
}

function loadProviderCredentialingCleanFileReport(request) {

    ReportsService.getProviderCredentialingCleanFile(
        request.startDate,
        request.endDate,
        function (response) {
            renderProviderCredentialingCleanFileReport(response);
            hideClientLoader();
        }
    );
}

function renderProviderCredentialingCleanFileReport(data) {

    $(containerId)
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'CredentialingTaskType', title: 'Credentialing Task Type', type: 'text', sortable: true },
                { key: 'StartDate', title: 'Start Date', type: 'date', sortable: true },
                { key: 'ProviderName', title: 'Provider Name', type: 'text', sortable: true },
                { key: 'ProviderType', title: 'Provider Type', type: 'text', sortable: true },
                { key: 'ProviderSpecialty', title: 'Provider Specialty', type: 'text', sortable: true },
                { key: 'NPI', title: 'Provider NPI', type: 'text', sortable: true },
                { key: 'MedicaidId', title: 'Provider MED ID', type: 'text', sortable: true },
                { key: 'PrimaryCity', title: 'Primary Practice City', type: 'text', sortable: true },
                { key: 'PrimaryState', title: 'Primary Practice State', type: 'text', sortable: true },
                { key: 'PrimaryCounty', title: 'Primary Practice County', type: 'text', sortable: true },
                { key: 'ApprovedDate', title: 'Date Approved', type: 'date', sortable: true },
                { key: 'ApprovedBy', title: 'Approved By', type: 'text', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No credentialing clean file records found.',
            idProperty: 'MedicaidId',
            enableRowSelection: false,
            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,
            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });
}

function loadProviderCredentialingFlaggedFilesReport(request) {

    ReportsService.getProviderCredentialingFlaggedFiles(
        request.startDate,
        request.endDate,
        function (response) {
            renderProviderCredentialingFlaggedFilesReport(response);
            hideClientLoader();
        }
    );
}

function renderProviderCredentialingFlaggedFilesReport(data) {

    $(containerId)
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'CredentialingTaskType', title: 'Credentialing Task Type', type: 'text', sortable: true },
                { key: 'StartDate', title: 'Start Date', type: 'date', sortable: true },
                { key: 'ProviderType', title: 'Provider Type', type: 'text', sortable: true },
                { key: 'ProviderName', title: 'Provider Name', type: 'text', sortable: true },
                { key: 'ProviderSpecialty', title: 'Provider Specialty', type: 'text', sortable: true },
                { key: 'NPI', title: 'Provider NPI', type: 'text', sortable: true },
                { key: 'MedicaidId', title: 'Provider Medicaid ID', type: 'text', sortable: true },
                { key: 'PrimaryCity', title: 'Primary Practice City', type: 'text', sortable: true },
                { key: 'PrimaryState', title: 'Primary Practice State', type: 'text', sortable: true },
                { key: 'PrimaryCounty', title: 'Primary Practice County', type: 'text', sortable: true },
                { key: 'CommitteeDecision', title: 'Committee Decision', type: 'text', sortable: true },
                { key: 'DecisionDate', title: 'Decision Date', type: 'date', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No credentialing flagged files found.',
            idProperty: 'MedicaidId',
            enableRowSelection: false,
            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,
            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });
}

function loadProviderCredentialingGreaterThan36MonthsReport(request) {

    ReportsService.getProviderCredentialinggraterthan36MonthsReport(
        request.startDate,
        request.endDate,
        function (response) {
            renderProviderCredentialingGreaterThan36MonthsReport(response);
            hideClientLoader();
        }
    );
}

function renderProviderCredentialingGreaterThan36MonthsReport(data) {

    $(containerId)
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'ProviderName', title: 'Provider Name', type: 'text', sortable: true },
                { key: 'ProviderType', title: 'Provider Type', type: 'text', sortable: true },
                { key: 'NPI', title: 'Provider NPI', type: 'text', sortable: true },
                { key: 'ReCredentialingDueDate', title: 'Provider Re- Cred Due Date', type: 'date', sortable: true },
                { key: 'MostRecentDateApproved', title: 'Most Recent Date Approved', type: 'date', sortable: true }
            ],
            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No providers found with re‑credentialing overdue more than 36 months.',
            idProperty: 'NPI',
            enableRowSelection: false,
            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,
            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });
}

function loadCredentialingCleanAndFlaggedFilesReport(request) {

    ReportsService.getCredentialngCleanandFlaggedfiles(
        request.startDate,
        request.endDate,
        function (response) {
            renderCredentialingCleanAndFlaggedFilesReport(response);
            hideClientLoader();
        }
    );
}

function renderCredentialingCleanAndFlaggedFilesReport(data) {

    $(containerId)
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'CSName', title: 'Credentialing Specialist Name', type: 'text', sortable: true },
                { key: 'TotalFilesProcessed', title: 'Total Files Processed', type: 'number', sortable: true },
                { key: 'FlaggedFiles', title: 'Total # of Flagged Files', type: 'number', sortable: true },
                { key: 'CleanFiles', title: 'Total # of Clean Files', type: 'number', sortable: true },
                { key: 'PercentFlagged', title: 'Percent of Flagged Processed', type: 'text', sortable: true },
                { key: 'PercentCleaned', title: 'Percent of Clean Processed', type: 'text', sortable: true }

            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No credentialing file processing summary data found.',
            idProperty: 'CSName',
            enableRowSelection: false,
            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,
            includeTime: false
        });
}

function showClientLoader() {
    $("#client-loader").removeClass("d-none");
}
function hideClientLoader() {
    $("#client-loader").addClass("d-none");
}

function convertToIso(dateStr, endOfDay = false) {
    const m = moment(dateStr, 'MM/DD/YYYY');
    if (endOfDay) {
        m.endOf('day');
    } else {
        m.startOf('day');
    }
    return m.toISOString();
}

$('#btnMyDrafts').click(function () {
    const query = $.param({
        mydrafts: true,
    });

    const href = appHref(`MesCred/Reports/Reports.aspx?${query}`);

    window.location.href = href;
});

const APP_BASE_PATH = (function () {
    const baseEl = document.querySelector('base[href]');
    if (baseEl) {
        const u = new URL(baseEl.getAttribute('href'), window.location.origin);
        return u.pathname.endsWith('/') ? u.pathname : (u.pathname + '/');
    }
    const m = window.location.pathname.match(/^\/MES_CRED(\/|$)/i);
    return m ? '/MES_CRED/' : '/';
})();

function appHref(pathAndQuery) {
    const base = APP_BASE_PATH.endsWith('/') ? APP_BASE_PATH : (APP_BASE_PATH + '/');
    const rel = pathAndQuery.startsWith('/') ? pathAndQuery.slice(1) : pathAndQuery;
    return new URL(base + rel, window.location.origin).href;
}

function downloadReport(request, format) {

    switch (request.reportName) {

        case 'Provider Re-credentialing Due Report':
            ReportsService.downloadProviderReCredentialingDueReport(
                request.startDate,
                request.endDate,
                format
            );
            break;

        case 'Credentialing Clean File Report':
            ReportsService.downloadProviderCredentialingCleanFile(
                request.startDate,
                request.endDate,
                format
            );
            break;

        case 'Credentialing Flagged Files Report':
            ReportsService.downloadProviderCredentialingFlaggedFiles(
                request.startDate,
                request.endDate,
                format
            );
            break;

        case 'Credentialing – Provider Contracting > 36 Months Report':
            ReportsService.downloadProviderCredentialinggraterthan36MonthsReport(
                request.startDate,
                request.endDate,
                format
            );
            break;

        case 'Credentialing – Clean and Flagged Files Summary Report':
            ReportsService.downloadCredentialngCleanandFlaggedfiles(
                request.startDate,
                request.endDate,
                format
            );
            break;

        default:
            alert('Download not supported for this report');
            console.warn('No download handler for:', request.reportName);
            break;
    }
}

$(document).on('click', '.download-btn', function () {

    const format = $(this).data('format');

    if (!currentRequest) {
        alert('No report loaded');
        return;
    }

    downloadReport(currentRequest, format);
});