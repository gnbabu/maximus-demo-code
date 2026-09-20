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
    $("#providerCredentialActivitiesAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    loadProviderCredentialActivities();
});

function loadProviderCredentialActivities() {
    
    var regId = $("[id*=hdnRegId]").val();

    ProviderService.getProviderCredentialActivitiesByRegId(function (response) {
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

                        const href = appHref(
                            `MesCred/ProviderCredentialingDetails/ProviderVerificationResults.aspx` +
                            `?ActivityId=${activityId}` +
                            `&ActivityTypeId=${activityTypeId}` +
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
                        const regId = $("[id*=hdnRegId]").val();   // ✅ get regId
                        const currentId = $("[id*=hdnCurrentCredentialingId]").val();


                        // Current record = no end date
                        if (!row.CredentialingEndDate) {
                            return `
                            <span class="fw-bold text-dark"
                                  title="Current Credentialing Record">
                                ${row.CredentialingStatusName}
                            </span>`;
                        }


                        const href = appHref(
                            `MesCred/ProviderCredentialingDetails/ProviderCredentialingHistory.aspx` +
                            `?CredentialingId=${credentialingId}` +
                            `&RegID=${regId}` +
                            `&LatestCredentialingId=${currentId}`
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
                                   id="updateCredentialStatusCheckbox" />
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

    // Populate checkbox AFTER rendering
    const regId = $("[id*=hdnRegId]").val();
    const credentialingId = $("[id*=hdnCurrentCredentialingId]").val();

    CredentialingService.getCredentialingPendingVerification(
        regId,
        credentialingId,
        function (response) {

            $("#updateCredentialStatusCheckbox").prop(
                "checked",
                response.IsInPendingVerification
            );
        }
    );
}

$(document).ready(function () {
  
    $(document).on("change", "#updateCredentialStatusCheckbox", function () {


        const isChecked = $(this).is(":checked");
        const regId = $("[id*=hdnRegId]").val();
        const credentialingId = $("[id*=hdnCurrentCredentialingId]").val();

        const userDetail = $("[id*=hdnUserDetail]").val().split("|");
        const userId = userDetail[0];

        const STATUS = {
            InProcess: 2,
            PendingProcess: 3
        };

        const newStatusId = isChecked
            ? STATUS.PendingProcess
            : STATUS.InProcess;

        // Save Pending Verification flag
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
                    }
                );
            }
        );
    });

});