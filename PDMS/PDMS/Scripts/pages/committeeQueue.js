const APP_BASE_PATH = (function () {
    const baseEl = document.querySelector('base[href]');
    if (baseEl) {
        const u = new URL(baseEl.getAttribute('href'), window.location.origin);
        return u.pathname.endsWith('/') ? u.pathname : (u.pathname + '/');
    }
    const m = window.location.pathname.match(/^\/MES_CRED(\/|$)/i);
    return m ? '/MES_CRED/' : '/';
})();


$(function () {
    loadCvoCommitteeQueue();

    var credentialApproveModal = $('#credentialApproveModal').modalPlugin({
        modalId: '#credentialApproveModal',
        modalWidth: '600px',

        onOpen: function ($modal) {
            console.log('Approve Selected Providers modal opened');

            // Optional: show selected count
            var selectedCount = getSelectedProvidersFromCommitteeQueueGrid().length;
            $modal.find('.modal-body p:first')
                .text(`Are you sure you want to approve ${selectedCount} selected provider(s)?`);
        },

        onSave: function ($modal) {
            approveSelectedProviders();
            credentialApproveModal.close();
        },

        onClose: function ($modal) {

        }
    });

    $('#btnApprove').on('click', function () {
        credentialApproveModal.save();
    });


    $('#btnCancel').on('click', function () {
        credentialApproveModal.close();
    });

    $('#btnApproveSelectedProviders').on('click', function (e) {

        if ($(this).hasClass('disabled')) {
            e.preventDefault();
            return;
        }

        const selectedProviders = getSelectedProvidersFromCommitteeQueueGrid();

        if (!selectedProviders || selectedProviders.length === 0) {
            alert('Please select at least one provider to approve.');
            return;
        }

        credentialApproveModal.open();
    });

    $(document).on(
        'change',
        '#committee-queue-grid-container .dg-row-select, #committee-queue-grid-container .dg-select-all',
        function () {
            updateApproveSelectedButtonState();
        }
    );

});

function loadCvoCommitteeQueue() {
    var userId = $("[id*=hdnUserId]").val();
    var roleName = $("[id*=hdnRole]").val();
    var includeOtherSteps = false;

    CredentialingService.getCommitteeQueue(userId, roleName, includeOtherSteps, function (response) {
        renderCommitteeQueueGrid(response);
    });
}

function renderCommitteeQueueGrid(data) {


    const isUserInRole =
        ($("[id*=hdnIsUserInRole]").val() || "")
            .toLowerCase() === "true";

    $("#committee-queue-grid-container")
        .removeClass("d-none")
        .dataGrid({

            data: data,

            columns: [
                {
                    key: "RegistrationId", title: "Credentialing ID", type: "text", sortable: true,
                    cellTemplate: function (row) {
                        const regId = String(row.RegistrationId ?? "").trim();
                        const credId = String(row.CredentialingId ?? "").trim();
                        const href = appHref(`MesCred/ProviderCredentialingDetails/ProviderInformationBar.aspx?RegID=${encodeURIComponent(regId)}`);
                        return `<a href="${href}" class="gridLink" data-regid="${regId}">${credId}</a>`;
                    }
                },
                { key: "ProviderName", title: "Provider Name", type: "text", sortable: true },
                { key: "Npi", title: "NPI", type: "text", sortable: true },
                { key: "ProviderType", title: "Provider Type", type: "text", sortable: true },
                { key: "CredentialRiskLevelName", title: "Data Rank", type: "text", sortable: true },
                { key: "AgingDays", title: "Aging", type: "number", sortable: true },
                { key: "TaskName", title: "Task", type: "text", sortable: true },
                { key: "WorkflowName", title: "Workflow", type: "text", sortable: true },
                { key: "SPECIALTY_TYPE_NAME", title: "Specialty", type: "text", sortable: true }
            ],

            tableClass: "maximus-base-table",
            gridTitle: "",
            noDataMessage: "No records found.",
            idProperty: "TaskId",
            enableRowSelection: isUserInRole,
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

function approveSelectedProviders() {

    // Get selected providers from the grid plugin
    var selectedProviders = getSelectedProvidersFromCommitteeQueueGrid();

    if (!selectedProviders || selectedProviders.length === 0) {
        alert("Please select at least one provider to approve.");
        return;
    }

    var isUserInPSRoles = $("[id*=hdnIsUserInPSRoles]").val() === 'True';
    var currentUserId = $("[id*=hdnUserId]").val();
    var isUserInRole = $("[id*=hdnIsUserInRole]").val() === 'True';

    // Build ProviderApprovalRequest list
    var requests = $.map(selectedProviders, function (row) {
        return {
            regId: row.RegistrationId,
            credentialingId: row.credentialing_id,
            processId: row.ProcessId,
            userId: currentUserId,
            isUserInPSRoles: isUserInPSRoles || false,
            isUserInCredentialingRole: isUserInRole || false
        };
    });

    // Call API using CredentialingService
    CredentialingService.approveProviders(requests, function (response) {

        if (!response || !response.Results) {
            alert("Approval failed. Please try again.");
            return;
        }

        var failed = $.grep(response.Results, function (r) {
            return r.Success === false;
        });

        if (failed.length > 0) {
            alert(
                (response.Results.length - failed.length) +
                " provider(s) approved successfully.\n" +
                failed.length +
                " provider(s) failed. Please check logs."
            );
            console.error("Approval failures:", failed);
        } else {
            alert("Selected providers approved successfully.");
        }

        // Refresh Committee Queue grid
        loadCvoCommitteeQueue();
    });
}

function getSelectedProvidersFromCommitteeQueueGrid() {

    var $grid = $("#committee-queue-grid-container");

    if ($grid.length === 0) {
        console.error("Committee Queue grid container not found.");
        return [];
    }

    var gridDom = $grid.get(0);

    if (typeof gridDom.getSelectedRowObjects !== "function") {
        console.error("getSelectedRowObjects method not available on grid.");
        return [];
    }

    // ✅ Get ALL rows from plugin (current behavior)
    var allRows = gridDom.getSelectedRowObjects();

    // ✅ Get checked checkbox row indexes
    var selectedIndexes = [];
    $grid.find(".dg-row-select:checked").each(function () {
        var rowIndex = $(this).closest("tr").index();
        selectedIndexes.push(rowIndex);
    });

    // ✅ Map only checked rows
    var selectedRows = selectedIndexes.map(function (i) {
        return allRows[i];
    }).filter(Boolean);

    return selectedRows;
}

function updateApproveSelectedButtonState() {

    const selectedCount =
        getSelectedProvidersFromCommitteeQueueGrid().length;

    const $btn = $('#btnApproveSelectedProviders');

    if (selectedCount > 0) {
        $btn
            .removeClass('disabled')
            .attr('aria-disabled', 'false');
    } else {
        $btn
            .addClass('disabled')
            .attr('aria-disabled', 'true');
    }
}

function appHref(pathAndQuery) {
    const base = APP_BASE_PATH.endsWith('/') ? APP_BASE_PATH : (APP_BASE_PATH + '/');
    const rel = pathAndQuery.startsWith('/') ? pathAndQuery.slice(1) : pathAndQuery;
    return new URL(base + rel, window.location.origin).href;
}