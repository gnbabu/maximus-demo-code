$(function () {
    loadCvoQueue();
});

function loadCvoQueue() {
    var userId =   $("[id*=hdnUserId]").val();
    var roleName = $("[id*=hdnRole]").val();

    CredentialingService.getCvoQueue(userId, roleName, function (response) {
        renderCvoQueueGrid(response);
    });
}


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
function renderCvoQueueGrid(data) {

    $("#cvo-queue-grid-container")
        .removeClass("d-none")
        .dataGrid({

            data: data,

            columns: [
                {
                    key: "CredentialingId",
                    title: "Credentialing ID",
                    type: "text",
                    sortable: true,
                    cellTemplate: function (row) {
                        const regId = String(row.RegistrationId ?? '').trim();
                        const credId = String(row.CredentialingId ?? '').trim();
                        const href = appHref(`MesCred/ProviderCredentialingDetails/ProviderInformationBar.aspx?RegID=${encodeURIComponent(regId)}`);
                        return `<a href="${href}" class="regid-link" data-id="${credId}">${credId}</a>`;
                    }
                },
                { key: "ProviderName", title: "Name", type: "text", sortable: true },
                { key: "Npi", title: "NPI", type: "text", sortable: true },
                { key: "ProviderType", title: "Provider Type", type: "text", sortable: true }, 
                { key: "SpecialtyTypeName", title: "Specialty", type: "text", sortable: true },
                { key: "TaskName", title: "Task", type: "text", sortable: true },
                { key: "WorkflowName", title: "Workflow", type: "text", sortable: true },
                { key: "AgingDays", title: "Aging", type: "number", sortable: true }
            ],

            tableClass: "maximus-base-table",
            gridTitle: "",
            noDataMessage: "No queue items found",
            idProperty: "TaskId",

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: "MM-DD-YYYY",
            includeTime: false
        });
}

function showClientLoader() {
    $("#client-loader").removeClass("d-none");
}
function hideClientLoader() {
    $("#client-loader").addClass("d-none");
}
