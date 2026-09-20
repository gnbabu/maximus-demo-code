var assignToUserModal;
var adminMaintenanceModal;
var suppressAdvancedSearchToggle = false;

$(function () {
    toggleSearchButtonState();

    $("#advSearchDiv").addClass("d-none");
    // Initialize modal plugin ONCE
    assignToUserModal = $('#assignToUserModal').modalPlugin({
        modalId: '#assignToUserModal',
        modalWidth: '700px',

        onOpen: function ($modal) {
            console.log('Assign To User modal opened');

            // Reset dropdown selection only
            $modal.find('#ddlUnAssignedTo').val('-1');
            $modal.find('#ddlUnAssignedTo').removeClass('is-invalid');
            $modal.find('#assignUserError').addClass('d-none');
        },

        onSave: function () {
            assignUser();
            assignToUserModal.close();
        },

        onClose: function () {
            // Optional cleanup if needed
        }
    });

    // Delegated click (grid is dynamic)
    $(document).on('click', '.js-action-assign', function (e) {

        e.preventDefault();

        const $el = $(this);

        openAssignToUserModal(
            $el.data('regid'),
            $el.data('currentstepid'),
            $el.data('assignedto')
        );

    });

    $('#ddlUnAssignedTo').on('change', function () {
        $(this).removeClass('is-invalid');
        $('#assignUserError').addClass('d-none');
    });


    // Modal footer buttons
    $('#btnAssignConfirm').on('click', function (e) {
        e.preventDefault();

        //Validate BEFORE saving
        if (!validateAssignUser()) {
            return;
        }

        // Proceed to save only if valid
        assignToUserModal.save();
    });

    $('#btnAssignCancel').on('click', function () {
        assignToUserModal.close();
    });

    adminMaintenanceModal = $('#adminMaintenanceModal').modalPlugin({
        modalId: '#adminMaintenanceModal',
        modalWidth: '700px',

        onOpen: function ($modal) {
            console.log('Admin Maintenance modal opened');

            // Reset dropdown

            $modal.find('#ddlAdminActions').val('-1');
            $modal.find('#ddlAdminActions').removeClass('is-invalid');
            $modal.find('#adminActionError').addClass('d-none');

        },

        onSave: function () {
            submitAdminAction();
        },

        onClose: function () {
            // Optional cleanup
        }
    });

    // Confirm button
    $('#btnAdminConfirm').on('click', function (e) {
        e.preventDefault();

        // Validate BEFORE save
        if (!validateAdminAction()) {
            return; //stop here, modal stays open
        }

        // Proceed only when valid
        adminMaintenanceModal.save();
    });

    // Cancel button
    $('#btnAdminCancel').on('click', function () {
        adminMaintenanceModal.close();
    });

    $('#ddlAdminActions').on('change', function () {
        $(this).removeClass('is-invalid');
        $('#adminActionError').addClass('d-none');
    });

    // Delegated click from grid
    $(document).on('click', '.js-action-admin', function (e) {
        e.preventDefault();

        const $el = $(this);

        openAdminMaintenanceModal({
            regId: $el.data('regid'),
            providerName: $el.data('providername'),
            npi: $el.data('npi')
        });
    });

    $(document).on('input change', '#advancedSearchPanel input, #advancedSearchPanel select', function () {
        toggleSearchButtonState();
    });

    $(document).on('click', '.js-search-btn', function (e) {

        if ($(this).prop('disabled')) {
            e.preventDefault();
            return false;
        }
    });

    $(document).on(
        'change',
        '#provider-grid-container .dg-row-select, #provider-grid-container .dg-select-all',
        function () {

            // WAIT for plugin to update selectedRows
            setTimeout(function () {
                toggleAssignButtonState();
            }, 0); // minimal delay works
        }
    );


    $(document).on('click', '.js-assign-btn', function (e) {


        const selectedRows = getSelectedProvidersGrid();

        if (!selectedRows || selectedRows.length === 0) {
            e.preventDefault();
            return false;
        }

        // Build comma-separated RegIds
        const regIds = selectedRows.map(r => r.RegID).join(',');


        // Take FIRST row details
        const firstRow = selectedRows[0];

        openAssignToUserModal(
            regIds,
            firstRow.CurrentStepID,
            firstRow.AssignedTo
        );

    });

});


function openAssignToUserModal(regId, currentStepId, assignedTo) {

    console.log('Opening Assign modal for:', regId);

    regId = String(regId || '');

    // Store comma-separated RegIds
    $("[id$='hdnAssignedId']").val(regId);

    // detect multi-select
    const isMulti = regId.includes(',');

    // pick first RegId for API call
    const firstRegId = isMulti ? regId.split(',')[0] : regId;

    // skip step validation ONLY for multi
    if (!isMulti) {
        if (!currentStepId || currentStepId == 0) {

            $.modalPlugin.alert({
                title: 'Info',
                message: 'Unable to assign user because the workflow step is missing.'
            });
            return;
        }

        $("[id$='hfCurrentStepID']").val(currentStepId);
        $('#txtCurrentlyAssignedTo').val(assignedTo || '');
    } else {
        // multi case → clear display
        $('#txtCurrentlyAssignedTo').val('');
    }

    // ALWAYS CALL API
    ProviderService.getAssignedToData(
        firstRegId,
        currentStepId || 0,  // safe fallback
        true,
        function (response) {

            // populate current user (single only)
            if (!isMulti) {
                $('#txtCurrentlyAssignedTo').val(
                    response.CurrentlyAssignedTo || ''
                );
            }

            // populate dropdown for BOTH
            var ddl = $('#ddlUnAssignedTo');
            ddl.empty();
            ddl.append('<option value="-1">-- Enter --</option>');

            if (response.Users && response.Users.length > 0) {
                response.Users.forEach(function (u) {
                    ddl.append(
                        `<option value="${u.UserId}">${u.UserName}</option>`
                    );
                });
            }

            // open modal AFTER dropdown ready
            assignToUserModal.open();
        }
    );
}


function assignUser() {

    var regId = $("[id$='hdnAssignedId']").val(); 
    var assignedUserId = $('#ddlUnAssignedTo').val();
    var assignedUser = $('#ddlUnAssignedTo option:selected').text();
    var userId = $("[id$='hdnLoggedInUser']").val();

    var request = {
        RegId: regId,
        AssignedUser: assignedUser,
        AssignedUserId: assignedUserId,
        UserId: userId
    };

    console.log('Assign user request:', request);

    ProviderService.assignedToUser(request, function (response) {

        if (response && response.message && response.message !== "Success") {

            $.modalPlugin.alert({
                title: 'Info',
                message: response.message
            });

            return;
        }

        assignToUserModal.close();

        suppressAdvancedSearchToggle = true;
        performProviderSearch();
    });
}

function validateAssignUser() {

    var ddl = $('#ddlUnAssignedTo');
    var errorDiv = $('#assignUserError');
    var assignedUserId = ddl.val();

    // Reset validation
    ddl.removeClass('is-invalid');
    errorDiv.addClass('d-none');

    if (!assignedUserId || assignedUserId === '-1') {
        ddl.addClass('is-invalid');
        errorDiv.removeClass('d-none');
        return false;
    }

    return true;
}

function openAdminMaintenanceModal(data) {

    console.log('Opening Admin modal:', data);

    // Set header fields
    $('#lblCredentialingID').text(data.regId);
    $('#lblProviderName').text(data.providerName);
    $('#hdnNPI').val(data.npi);

    var ddl = $('#ddlAdminActions');
    ddl.empty();
    ddl.append('<option value="-1">-- Select --</option>');

    // Call API to get admin actions
    ProviderService.getAdminActions(function (actions) {

        if (actions && actions.length > 0) {
            actions.forEach(function (item) {
                ddl.append(
                    '<option value="' + item.Action + '">' +
                    item.Action +
                    '</option>'
                );
            });
        }

        // Open modal AFTER actions are loaded
        adminMaintenanceModal.open();
    });
}

function submitAdminAction() {

    var ddl = $('#ddlAdminActions');
    var errorDiv = $('#adminActionError2');
    var successDiv = $('#adminSuccessMessage');

    // Reset messages
    errorDiv.addClass('d-none');
    successDiv.addClass('d-none');

    var action = ddl.val();
    var npi = $('#hdnNPI').val(); 
    var loggedUser = $("#" + hdnLoggedInUserId).val();
    

    // ✅ Basic validation
    if (action === "-1") {
        errorDiv.removeClass('d-none');
        ddl.addClass('is-invalid');
        return;
    } else {
        ddl.removeClass('is-invalid');
    }

    var request = {
        Action: action,
        Npi: npi,
        loggedUser: loggedUser
    };

    console.log('Submitting admin action:', request);

    ProviderService.executeAdminAction(request, function(response) {

        let message = response?.Message || "Action completed.";

        // reset styles
        errorDiv.removeClass().addClass('d-none');
        successDiv.removeClass().addClass('d-none');

        
        if (response && response.isError) {
            errorDiv
                .text(message)
                .removeClass('d-none')
                .addClass('text-error');
            return;
        }


        // ✅ GREEN SUCCESS (you can keep alert or change similarly)
        successDiv
            .text(message)
            .removeClass('d-none')
            .addClass('text-success');

        setTimeout(() => {
            successDiv.addClass('d-none');
        }, 3000);

        setTimeout(() => {
            $('#adminMaintenanceModal').modal('hide');
            suppressAdvancedSearchToggle = true;
            performProviderSearch();
        }, 1500);

    }, function() {

        // ❌ HARD ERROR
        errorDiv
            .text("Something went wrong. Please try again.")
            .removeClass('d-none')
            .css({
                color: 'red',
                fontWeight: 'bold'
            });

    });
}


function validateAdminAction() {

    var ddl = $('#ddlAdminActions');
    var errorDiv = $('#adminActionError');
    var action = ddl.val();

    // Reset validation state
    ddl.removeClass('is-invalid');
    errorDiv.addClass('d-none');

    // Validation
    if (!action || action === '-1') {
        ddl.addClass('is-invalid');
        errorDiv.removeClass('d-none');
        return false;
    }

    return true;
}

function performProviderSearch() {

    if (typeof Page_ClientValidate === "function") {
        if (!Page_ClientValidate("ProviderSearch")) {
            hideClientLoader();
            return false;
        }
    }
    $("#divCardBody").removeClass("d-none");
    const searchRequest = buildProviderSearchRequest();
    loadProviderPage(searchRequest);
    $("#advSearchDiv").removeClass("d-none");
    return false;
}

function buildProviderSearchRequest() {

    const $scope = $("#advancedSearchPanel");
    const $ctl = suffix => $scope.find("[id$='" + suffix + "']");

    const v = val => {
        if (val === undefined || val === null) return '';

        const s = String(val).trim();

        if (
            s === '' ||
            s === 'MM/DD/YYYY' ||
            /^[-\s]*enter[-\s]*$/i.test(s)
        ) {
            return '';
        }

        return s;
    };

    return {
        baseMedicaidID: v($ctl("txtMedicaidID").val()),
        groupName: v($ctl("txtGroupName").val()),
        dbaName: v($ctl("txtDBAName").val()),
        taxID: v($('#hdnTaxID').val()),
        npi: v($ctl("txtNPI").val()),
        applicationTypeId: v($ctl("ddlApplicationType").val()),
        providerCategoryTypeId: '',
        waiverType: '',
        providerTypeId: v($ctl("ddlProviderType").val()),
        specialtyId: v($ctl("ddlSpecialty").val()),
        enrollmentStatusReason: '',
        doddContractNumber: '',
        county: v($ctl("txtCounty").val()),
        city: v($ctl("txtCity").val()),
        regID: v($ctl("txtRegID").val()),
        credId: v($ctl("txtCredentialingId").val()),

        tennCareStatus: '',
        pnmEnrollmentStatus: '',
        pnmApplicationStatus: '',

        mcp: '',
        program: '',
        taxonomy: v($ctl("ddlTaxonomy").val()),
        pdmsStatus: v($ctl("ddlPDMSStatus").val()),
        PDMSStatusDate: v($ctl("txtPDMSStatusDate").val()),
        dateReceived: v($ctl("txtDateReceived").val()),
        medicareNumber: v($ctl("txtMedicareNumber").val()),
        area: '',
        odaRegistrationStatus: '',
        doddRegistrationStatus: '',
        
        ContractType: '',
        ContractStatus: '',

        roleName: v($("[id$='hdnRoleName']").val()),
        pageNumber: 1,
        pageSize: 999,
        sortExpression: "RegID DESC",
        getTotalResultCount: true

    };
}

function loadProviderPage(searchRequest) {
    showClientLoader();

    ProviderService.providerSearch(
        searchRequest,
        function (res) {
            renderProviderGrid(res.Items || []);
            toggleAssignButtonState();
            if (!suppressAdvancedSearchToggle) {
                togglePanelNew();
            }
            $("#provider-grid-container").removeClass("d-none");
            hideClientLoader();
            suppressAdvancedSearchToggle = false;
        },
        function (err) {
            console.error(err);
            hideClientLoader();
        }
    );
}

function renderProviderGrid(data) {

    $("#provider-grid-container")
        .empty()
        .dataGrid({
            data: data,
            columns: [
                {
                    key: 'CredId',
                    title: 'Credentialing ID',
                    type: 'text',
                    sortable: true,
                    cellTemplate: function (row) {
                        const regId = String(row.RegID ?? '').trim();
                        const credId = String(row.CredId ?? '').trim();
                        const href = appHref(`MesCred/ProviderCredentialingDetails/ProviderInformationBar.aspx?RegID=${encodeURIComponent(regId)}`);
                        return `<a href="${href}" class="regid-link" data-id="${credId}">${credId}</a>`;
                    }
                },
                { key: 'OrganizationName', title: 'Provider Name', type: 'text', sortable: true },
                { key: 'ProviderTypeName', title: 'Provider Type', type: 'text', sortable: true },
                { key: 'TaxId', title: 'Tax ID', type: 'text', sortable: true },
                { key: 'NPI', title: 'NPI', type: 'text', sortable: true },
                { key: 'SpecialtyTypeName', title: 'Specialty', type: 'text', sortable: true },
                { key: 'BaseMedicaidID', title: 'Medicaid ID', type: 'text', sortable: true },
                { key: 'AssignedTo', title: 'Assigned To', type: 'text', sortable: true },
                {
                    key: '_actions',
                    title: '',
                    type: 'text',
                    sortable: false,
                    width: 48,
                    cellTemplate: function (row) {
                        const regId = String(row.RegID ?? '').trim();
                        const currStepID = String(row.CurrentStepID ?? '').trim();
                        // a unique-ish id for aria-controls (optional)
                        const uid = `dd-${regId.replace(/[^a-zA-Z0-9_\-:.]/g, '_')}`;
                        return `
                                <div class="dropdown pd-dropdown">
                                  <button class="dropdown-button border-0 three-dots-btn"
                                          id="${uid}"
                                          data-bs-toggle="dropdown"
                                          data-bs-auto-close="outside"
                                          aria-expanded="false"
                                          type="button"
                                          data-regid="${regId}"
                                          title="Actions">
                                    <span class="material-symbols-outlined">more_vert</span>
                                  </button>
                                  <ul class="dropdown-menu" aria-labelledby="${uid}">
                                    <li>
                                      <a href="javascript:void(0)" class="dropdown-item js-action-review" data-regid="${regId}">Review</a>
                                    </li>
                                    <li>
                                      <a href="javascript:void(0)"
                                        class="dropdown-item js-action-admin"
                                        data-regid="${regId}"
                                        data-providername="${row.OrganizationName ?? ''}"
                                        data-npi="${row.NPI ?? ''}">
                                        Admin
                                    </a>
                                    </li>
                                    <li>
                                      <a href="javascript:void(0)"
                                           class="dropdown-item js-action-assign"
                                           data-regid="${regId}"
                                           data-currentstepid="${currStepID}"
                                           data-assignedto="${row.AssignedTo ?? ''}">
                                           Assign To
                                     </a>

                                    </li>
                                  </ul>
                                </div>`;
                    }
                }
            ],
            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No providers found.',
            idProperty: 'RegID',
            enableRowSelection: true,
            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,
            enableClientSideSortNoReset: false,
            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

    if (data && data.length > 0) {
        $('#workbenchSectionHeader').text('Provider Search Results');
    } else {
        $('#workbenchSectionHeader').text('Provider Search');
    }
}

function showClientLoader() {
    $("#client-loader").removeClass("d-none");
}
function hideClientLoader() {
    $("#client-loader").addClass("d-none");
}


function clearProviderSearchFields() {
    const $scope = $("#advancedSearchPanel");
    const $ctl = (suffix) => $scope.find("[id$='" + suffix + "']");

    const fieldsToClear = [
        "txtMedicaidID", "txtGroupName", "txtDBAName", "txtTaxID", "hdnTaxID", "txtNPI",
        "ddlApplicationType", "ddlProviderType",
        "ddlSpecialty", "txtCounty", "txtCity", "txtRegID",
        "ddlTaxonomy", "ddlPDMSStatus", "txtMedicareNumber",
        "ddlPageSize"
    ];

    fieldsToClear.forEach(id => $ctl(id).val(""));

    $ctl("txtDateReceived").val("");
    $ctl("txtPDMSStatusDate").val("");

    $ctl("txtDateReceived").attr("placeholder", "MM/DD/YYYY");
    $ctl("txtPDMSStatusDate").attr("placeholder", "MM/DD/YYYY");

    $ctl("hdnPageNumber").val("1");


    window.sortColWithDirection = "RegID DESC";


    $scope.find("select").prop("selectedIndex", 0);
    toggleSearchButtonState();
    renderProviderGrid([]);
    hideClientLoader();

    $('#workbenchSectionHeader').text('Provider Search');
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

function toggleSearchButtonState() {

    const $scope = $("#advancedSearchPanel");
    const $ctl = suffix => $scope.find("[id$='" + suffix + "']");

    let hasValue = false;

    const fields = [
        $ctl("txtMedicaidID"),
        $ctl("txtGroupName"),
        $ctl("txtDBAName"),
        $ctl("txtTaxID"),
        $ctl("txtNPI"),
        $ctl("ddlApplicationType"),
        $ctl("ddlProviderType"),
        $ctl("ddlSpecialty"),
        $ctl("txtCounty"),
        $ctl("txtCity"),
        $ctl("txtRegID"),
        $ctl("txtCredentialingId"),
        $ctl("ddlTaxonomy"),
        $ctl("ddlPDMSStatus"),
        $ctl("txtMedicareNumber"),
        $ctl("txtDateReceived"),
        $ctl("txtPDMSStatusDate")
    ];

    fields.forEach(function ($el) {

        if (!$el.length) return;

        let val = ($el.val() || "").toString().trim();

        if ($el.is("input")) {

            if (
                val.length > 0 &&                    //  must have real chars
                !/^[-\s]*enter[-\s]*$/i.test(val) && //  ignore "- Enter -"
                val !== "MM/DD/YYYY"                //  ignore watermark
            ) {
                hasValue = true;
                return false; //  break loop early
            }
        }

        if ($el.is("select")) {

            let text = $el.find("option:selected").text().trim();

            if (val !== "" && val !== "-1" && !/^[-\s]*enter[-\s]*$/i.test(text.toLowerCase())) {
                hasValue = true;
                return false;
            }
        }

    });

    const $btn = $(".js-search-btn");

    $btn.prop('disabled', !hasValue)
        .toggleClass('disabled', !hasValue);
}


function getSelectedProvidersGrid() {

    var $grid = $("#provider-grid-container");
    if ($grid.length === 0) {
        console.error("Provider grid container not found.");
        return [];
    }

    var gridDom = $grid.get(0);

    if (typeof gridDom.getSelectedRowObjects !== "function") {
        console.error("getSelectedRowObjects method not available on provider grid.");
        return [];
    }

    return gridDom.getSelectedRowObjects();
}

function toggleAssignButtonState() {

    const selectedRows = getSelectedProvidersGrid();
    const $btn = $(".js-assign-btn");

    const hasSelection = selectedRows && selectedRows.length > 0;

    $btn.prop("disabled", !hasSelection)
        .toggleClass("disabled", !hasSelection);
}

function formatDateInput(el) {

    // ✅ remove all non-digits
    let val = el.value.replace(/\D/g, '');

    // ✅ format MM/DD/YYYY
    if (val.length >= 3 && val.length <= 4)
        val = val.slice(0, 2) + '/' + val.slice(2);

    else if (val.length > 4)
        val = val.slice(0, 2) + '/' + val.slice(2, 4) + '/' + val.slice(4, 8);

    el.value = val;
}