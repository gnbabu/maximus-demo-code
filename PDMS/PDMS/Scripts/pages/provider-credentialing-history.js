$(function () {

    $("#credentialingNotesAccordionID2").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    $("#CredentialCommitteeID").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    $("#providerCredentialingDocumentsAccordionID").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    $("#providerCredentialingHistoryAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    loadProviderCredentialActivities();
    loadProviderHistoryDocuments();
});

function getSelectedCredentialingId() {
    return $("[id*=hdnSelectedCredentialingId]").val();
}


function loadProviderCredentialActivities() {
    $("#providerCredentialActivitiesAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    var regId = $("[id*=hdnRegId]").val();
    var credId = $("[id*=hdnSelectedCredentialingId]").val();

    ProviderService.getProviderCredentialActivitiesHistoryByCredRegId(regId, credId, function (response) {
        renderProviderCredentialActivitiesGrid(response);
    });

    ProviderService.getProviderCredentialHistoryByRegId(function (response) {
        renderCredentialActivityHistoryGrid(response);
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
function renderProviderCredentialActivitiesGrid(data) {

    $("#provider-credential-activities-grid-container")
        .removeClass("d-none")
        .dataGrid({

            data: data,

            columns: [
                {
                    key: "ScreeningActivityTypeName",
                    title: "Credentialing Verification",
                    type: "text",
                    sortable: true
                },
                {
                    key: "DataRankName",
                    title: "Status",
                    type: "text",
                    sortable: true,
                    cellTemplate: function (row) {

                        if (!row.DataRankName) return "";

                        const activityId = row.CredentialActivityId;
                        const activityTypeId = row.ActivityTypeId;
                        const dataRankId = row.DataRankTypeId;
                        const screeningName = encodeURIComponent(
                            row.ScreeningActivityTypeName || ""
                        );
                        var credId = $("[id*=hdnSelectedCredentialingId]").val();
                        var regId = $("[id*=hdnRegId]").val();

                        const href = appHref(
                            `MesCred/ProviderCredentialingDetails/ProviderVerificationResultsHistory.aspx` +
                            `?ActivityId=${activityId}` +
                            `&ActivityTypeId=${activityTypeId}` +
                            `&CredentialingId=${credId}` +
                            `&RegID=${regId}` +
                            `&DataRankId=${dataRankId}` +
                            `&ScreeningName=${screeningName}`
                        );

                        return `
                            <a href="${href}"
                               class="text-primary fw-bold"
                               aria-label="${row.DataRankName} - Open details for activityId ${activityId}"
                               title="View Verification Results">
                                ${row.DataRankName}
                            </a>`;
                    }
                },
                {
                    key: "VerificationSourceDisplayName",
                    title: "Verification Source",
                    type: "text",
                    sortable: true
                },
                {
                    key: "LastActionDate",
                    title: "Verification Date",
                    type: "date",
                    sortable: true
                },
                {
                    key: "VerifiedBy",
                    title: "Verified By",
                    type: "text",
                    sortable: true
                },
                {
                    key: "Notes",
                    title: "Notes",
                    sortable: false,
                    cellTemplate: function (row) {

                        return `
                            <span class="material-symbols-outlined notes-icon"
                                  title="${row.Notes || ''}">
                                sticky_note_2
                            </span>`;
                    }
                }
            ],

            tableClass: "maximus-base-table",
            gridTitle: "",
            noDataMessage: "No credential activities found",
            idProperty: "CredentialActivityId",
            enableRowSelection: false,
            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,
            enableStatusLabel: true,
            dateFormat: "MM-DD-YYYY",
            includeTime: false
        });
}

function renderCredentialActivityHistoryGrid(data) {

    $("#credential-activity-history-grid-container")
        .removeClass("d-none")
        .dataGrid({

            data: data,


            rowClass: function (row) {
                const selectedId = getSelectedCredentialingId();

                if (selectedId && row.CredentialingId == selectedId) {
                    return "highlight-row";
                }

                return "";
            },

            columns: [
                {
                    key: "CredentialingType",
                    title: "Credentialing Type",
                    type: "text",
                    sortable: true
                },
                {
                    key: "CredentialingStartDate",
                    title: "Credentialing Start",
                    type: "date",
                    sortable: true
                },
                {
                    key: "CredentialingEndDate",
                    title: "Credentialing End",
                    type: "date",
                    sortable: true
                },
                {
                    key: "CredentialingStatusName",
                    title: "Status",
                    type: "text",
                    sortable: true,
                    cellTemplate: function (row) {

                        if (!row.CredentialingStatusName)
                            return "";

                        const credentialingId = row.CredentialingId;
                        const regId = $("[id*=hdnRegId]").val();
                        const latestCredentialingId = $("[id*=hdnLatestCredentialingId]").val();
                        const currentCredId = $("[id*=hdnSelectedCredentialingId]").val();


                        // ✅ LIVE RECORD → goes back to ProviderInformationBar
                        if (!row.CredentialingEndDate) {

                            const liveHref = appHref(
                                `MesCred/ProviderCredentialingDetails/ProviderInformationBar.aspx?RegID=${regId}`
                            );

                            return `
                                <a href="${liveHref}"
                                   class="fw-bold text-success"
                                   title="Return to Current Credential">
                                    🔄 ${row.CredentialingStatusName} (Current)
                                </a>`;
                        }

                        // ✅ CURRENT RECORD → NO LINK
                        if (currentCredId && String(credentialingId) === String(currentCredId)) {
                            return `
                            <span class="fw-bold text-dark"
                                  title="Current Credentialing Record">
                                ${row.CredentialingStatusName}
                            </span>`;
                        }


                        const href = appHref(
                            `MesCred/ProviderCredentialingDetails/ProviderCredentialingHistory.aspx` +
                            `?CredentialingId=${credentialingId}` +
                            `&RegID=${regId}`
                        );

                        return `
                            <a href="${href}"
                               class="text-primary fw-bold"
                               title="View Credentialing History">
                                ${row.CredentialingStatusName}
                            </a>`;
                    }
                },
                {
                    key: "CredentialingResultName",
                    title: "Result",
                    type: "text",
                    sortable: true
                },
                {
                    key: "CredentialingRiskLevel",
                    title: "Credentialing Risk Level",
                    type: "text",
                    sortable: true
                }
            ],

            tableClass: "maximus-base-table",
            gridTitle: "",
            noDataMessage: "No Credential history found",
            idProperty: "CredentialingId",

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: "MM-DD-YYYY",
            includeTime: false
        });
    renderBottomCheckbox(data);
}

function renderBottomCheckbox(data) {

    const checkboxContainer = $("#credential-activity-history-grid-container-checkbox");

    checkboxContainer.empty();

    if (!data || data.length === 0) {
        checkboxContainer.addClass("d-none");
        return;
    }

    const credentialStatusID = parseInt($("[id*=hdnCredentialStatusID]").val());

    const STATUS = {
        PendingProcess: 3
    };

    // ✅ Equivalent to C# Checked = (status == 3)
    const isChecked = credentialStatusID === STATUS.PendingProcess;
    const userDetail = $("[id*=hdnUserDetail]").val().split("|");
    const roles = userDetail.length > 1 ? userDetail[1].split(",") : [];
    const ALLOWED_ROLES = ["CredentialingSpecialist", "ODMCredentialingSpecialist"];
    const hasAccess = roles.some(r => ALLOWED_ROLES.includes(r));

    if (!hasAccess) {
        checkboxContainer.addClass("d-none");
        return;
    }

    const html = `
        <table class="mt-2" role="presentation">
            <tbody>
                <tr>
                    <td>
                        <label>
                            <input type="checkbox" 
                                   id="updateCredentialStatusCheckbox"
                                   ${isChecked ? "checked" : ""} />
                            Pending Verification
                        </label>
                    </td>
                </tr>
            </tbody>
        </table>
    `;



    checkboxContainer
        .removeClass("d-none")
        .html(html);
}

function loadProviderHistoryDocuments() {
    var providerDocumentRequest = {
        regId: parseInt($("[id*=hdnRegId]").val(), 10),
        regPageTypeId: parseInt($("[id*=hdnRegStepId]").val(), 10),
        docSectionName: "", // optional, set when needed
        exclusions: $("[id*=hdnExclusions]").val(),
        showEducationWorkDocs: String($("[id*=hdnshowEducationWorkDocs]").val()).toLowerCase() === "true",
        userRole: $("[id*=hdnUserRole]").val()
    };

    CredentialingActivityService.getProviderDocuments(providerDocumentRequest, function (response) {
        renderProviderHistoryDocumentsGrid(response);
    });
}

function renderProviderHistoryDocumentsGrid(data) {

    $('#provider-history-documents-grid-container')
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

$(document).ready(function () {


    $("#ctl00_MainContent_ucProviderDetails_btnShowProviderView").hide();


    const regId = $("[id*=hdnRegId]").val();
    const credentialingId = $("[id*=hdnSelectedCredentialingId]").val();

    // Populate checkbox from database
    CredentialingService.getCredentialingPendingVerification(
        regId,
        credentialingId,
        function (response) {
            $("#updateCredentialStatusCheckbox").prop(
                "checked",
                response.isInPendingVerification
            );
        });

    $(document).on("change", "#updateCredentialStatusCheckbox", function () {
        debugger;
        const isChecked = $(this).is(":checked");

        const regId = $("[id*=hdnRegId]").val();
        const credentialingId = $("[id*=hdnSelectedCredentialingId]").val();

        const userDetail = $("[id*=hdnUserDetail]").val().split("|");
        const userId = userDetail[0];

        const STATUS = {
            InProcess: 2,
            PendingProcess: 3
        };

        const newStatusId = isChecked
            ? STATUS.PendingProcess
            : STATUS.InProcess;

        // Save Pending Verification Flag
        CredentialingService.saveCredentialingPendingVerification(
            {
                RegID: parseInt(regId),
                CredentialingID: parseInt(credentialingId),
                IsInPendingVerification: isChecked,
                UserID: userId
            },
            function () {

                // Update Credential Status
                ProviderService.updateCredentialStatus(
                    regId,
                    newStatusId,
                    userId,
                    function () {

                        $("[id*=hdnCredentialStatusID]").val(newStatusId);
                        loadProviderCredentialActivities();
                    });
            });
    });
});