let startDateValue = null;
let cleanReportDatesInitialized = false;
let selectedReportName = null;


$(function () {
    const params = new URLSearchParams(window.location.search);
    console.log(params);
    const mydrafts = params.get('mydrafts');

    if (mydrafts !== null && mydrafts !== '' && mydrafts !== undefined && mydrafts === 'true') {
        $('#myDrafts').show();
        $('#reportManagement').hide();
    }
    else {
        $('#reportManagement').show();
        $('#myDrafts').hide();
    }


    $('#btnReportManagement').click(function () {

        $('#reportManagement').show();
        $('#myDrafts').hide();

        // ✅ Show loader
        showLoader("Refreshing reports...");

        // ✅ Clear UI before reload (prevents duplicate rendering)
        $('#standardReportsByCategory').html('');
        $('#customReportsByCategory').html('');
        $('#draftReportsByCategory').html('');

        // ✅ Reload ALL categories
        loadBoldReportsByCategory()
            .finally(() => {
                hideLoader();
            });

        // ✅ Clean URL
        const url = new URL(window.location.href);
        url.searchParams.delete("mydrafts");
        history.replaceState({}, document.title, url.toString());
    });

    $('#btnMyDrafts').click(function () {

        $('#myDrafts').show();
        $('#reportManagement').hide();

        // ✅ OPTIONAL: show loader
        showLoader("Refreshing drafts...");

        // ✅ Clear old drafts before reloading
        $('#draftReportsByCategory').html('');

        // ✅ Reload all categories (includes Draft Reports)
        loadBoldReportsByCategory()
            .finally(() => {
                hideLoader();
            });
    });

    $("#standardReportsAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    $("#draftReportsAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    $("#customReportsAccordion").maximusAccordion({
        allowMultiple: false
    });

    $(document).on('click', '.maximus-date-wrapper .maximus-icon', function () {
        $(this).siblings('.maximus-date-input').focus();
    });

    var selectTemplateModal = $('#selectTemplateModal').modalPlugin({
        modalId: '#selectTemplateModal',
        modalWidth: '500px',

        onSave: async function ($modal) {   // ✅ make async

            const $ddl = $modal.find('#templateSelect');
            const $ddl2 = $modal.find('#reportTypeSelect');
            const reportName = $modal.find('#reportName').val().trim();

            let isValid = true;

            if (!reportName) {
                $('#reportNameError').show();
                isValid = false;
            } else {
                $('#reportNameError').hide();
            }

            if ($ddl.prop('selectedIndex') === 0) {
                $('#templateError').show();
                isValid = false;
            } else {
                $('#templateError').hide();
            }

            if ($ddl2.prop('selectedIndex') === 0) {
                $('#reportTypeError').show();
                isValid = false;
            } else {
                $('#reportTypeError').hide();
            }

            if (!isValid) return false;

            try {
                const templateId = $ddl.val();
                const reportTypeId = $ddl2.val();

                // ✅ Get token
                const token = await getToken();

                // ✅ CHECK NAME UNIQUENESS FIRST
                const isUnique = await $.ajax({
                    url: API_BASE + "reports/api/IsReportNameUnique",
                    type: "GET",

                    data: {
                        category: "Templates",
                        name: reportName
                    },

                    headers: {
                        "ServiceAuthorizationToken": "Bearer " + token
                    }
                });

                if (!isUnique) {
                    $('#reportNameError')
                        .text('Report name already exists ❌')
                        .show();

                    return;
                }

                showLoader("Creating your report…<br/>Redirecting you to the editor");

                // ✅ If unique → proceed to clone
                $.ajax({
                    url: API_BASE + 'reports/api/CloneReport',
                    type: 'POST',
                    data: {
                        SrcId: templateId,
                        tgtCategory: reportTypeId,
                        tgtName: reportName
                    },
                    headers: {
                        "ServiceAuthorizationToken": "Bearer " + token
                    },

                    success: function (res) {

                        console.log("FULL RESPONSE:", res);

                        const reportId = res.Value || res.value || res;

                        console.log("Resolved reportId:", reportId);

                        if (!reportId) {
                            alert("Report created but ID missing ❌");
                            return;
                        }

                        const editorUrl = appHref(
                            `MesCred/Reports/EditReport.aspx?id=${reportId}&isDraft=true`
                        );

                        window.location.href = editorUrl;
                    },

                    error: function (err) {
                        console.error("Clone failed ❌", err);
                        alert("Failed to create report");
                    }
                });

            } catch (e) {
                console.error("Error ❌", e);
            }
        },

        onClose: function ($modal) {

            $modal.find('#reportName').val('');
            $modal.find('#templateSelect').prop('selectedIndex', 0);

            $('#reportNameError').hide();
            $('#templateError').hide();
            $('#reportTypeError').hide();

            $modal.find('#reportName').removeClass('is-invalid');

            console.log('Select Template modal reset ✅');
        }
    });

    async function getToken() {
        const res = await fetch(API_BASE + "reports/api/token");
        const data = await res.json();
        return data.access_token;
    }

    $('#newReportBtn').on('click', function () {
        selectTemplateModal.open();
    });

    $('#newReportBtn2').on('click', function () {
        selectTemplateModal.open();
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

    function renderStandardReports(listContainerSelector) {
        var $container = $(listContainerSelector);
        $container.html('<div class="text-muted p-2">Loading…</div>');
        ReportsService.getStandardReports(function (response) {
            var items = (response && response.Data) ? response.Data : [];

            if (!items.length) {
                $container.html('<div class="text-muted p-2">No standard reports available.</div>');
                return;
            }

            var frag = $(document.createDocumentFragment());

            items.forEach(function (r) {
                var id = r.ReportId ?? '';
                var name = r.ReportName ?? '';

                var $item = $(`
    <div class="report-item" data-report-id="${id}">
        <span class="report-icon">
            <span class="material-icons-outlined">description</span>
        </span>
        <span class="report-name"></span>
        <button class="btn-more" type="button" title="More">
            <span class="material-icons-outlined">more_vert</span>
        </button>
    </div>
    `);
                $item.find('.report-name').text(name);
                $item.on('click', function (e) {

                    if (!$(e.target).closest('.btn-more').length) return;
                    var reportId = $(this).data('report-id');
                    var reportName = $(this).find('.report-name').text();
                    console.log('Clicked report:', reportId, reportName);
                    selectedReportName = $(this).find('.report-name').text().trim();
                });

                frag.append($item);
            });

            $container.empty().append(frag);

        }, function (xhr) {
            console.error('Failed to load standard reports', xhr);
            $container.html('<div class="text-danger p-2">Failed to load standard reports.</div>');
        });
    }

    function renderCustomReports(listContainerSelector) {
        var $container = $(listContainerSelector);
        $container.html('<div class="text-muted p-2">Loading…</div>');

        ReportsService.getCustomReports(function (response) {
            var items = (response && response.Data) ? response.Data : [];

            if (!items.length) {
                $container.html('<div class="text-muted p-2">No custom reports available.</div>');
                return;
            }

            var frag = $(document.createDocumentFragment());

            items.forEach(function (r) {
                var id = r.ReportId ?? '';
                var name = r.ReportName ?? '';

                var $item = $(`
        <div class="report-item" data-report-id="${id}">
          <span class="report-icon">
            <span class="material-icons-outlined">description</span>
          </span>
          <span class="report-name"></span>
          <button class="btn-more" title="More">
            <span class="material-icons-outlined">more_vert</span>
          </button>
        </div>
      `);
                $item.find('.report-name').text(name);
                $item.on('click', function (e) {
                    if ($(e.target).closest('.btn-more').length) {
                        alert('returning');
                        return;
                    }                    // ignore 'more' clicks
                    var reportId = $(this).data('report-id');
                    var reportName = $(this).find('.report-name').text();
                    console.log('Open custom report:', reportId, reportName);
                });


                frag.append($item);
            });

            $container.empty().append(frag);

        }, function (xhr) {
            console.error('Failed to load custom reports', xhr);
            $container.html('<div class="text-danger p-2">Failed to load custom reports.</div>');
        });
    }

    renderStandardReports('#standardReportsList');
    renderCustomReports('#customReportsList');

    $('#saveAndCloseTemplate').on('click', function () {
        selectTemplateModal.save();
    });

    $('#cancelSelectTemplate').on('click', function () {
        selectTemplateModal.close();
    });

    $('#templateSelect').on('change', function () {
        $('#templateError').hide();
    });

    $('#reportTypeSelect').on('change', function () {
        $('#reportTypeError').hide();
    });

    $('#selectTemplateModal').on('hidden.bs.modal', function () {

        const $modal = $(this);

        // ✅ Clear values
        $modal.find('#reportName').val('');
        $modal.find('#templateSelect').prop('selectedIndex', 0);

        // ✅ Clear errors
        $modal.find('#reportNameError').hide();
        $modal.find('#templateError').hide();
        $modal.find('#reportTypeError').hide();

        // ✅ Remove red border
        $modal.find('#reportName').removeClass('is-invalid');

    });
});
