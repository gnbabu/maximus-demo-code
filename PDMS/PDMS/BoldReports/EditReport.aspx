<%@ Page Title="Provider Credentialing Reports Editor" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeBehind="EditReport.aspx.cs" Inherits="BoldReports_ReportsEditReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <link href="<%= ResolveUrl("~/Content/bold-reports/v2.0/tailwind-light/bold.report-designer.min.css") %>" rel="stylesheet" />

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="<%= ResolveUrl("~/Scripts/bold-reports/v2.0/common/bold.reports.common.min.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/bold-reports/v2.0/common/bold.reports.widgets.min.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/bold-reports/v2.0/bold.report-designer.min.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/bold-reports/v2.0/bold.report-viewer.min.js") %>"></script>

    <div id="designer" style="height: 800px;"></div>

    <div class="modal fade" id="saveSuccessModal" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 class="modal-title">Success</h3>
                </div>
                <div class="modal-body">
                    Report saved successfully.
                </div>
                <div class="modal-footer">
                    <button type="button"
                        class="btn btn-primary"
                        data-bs-dismiss="modal">
                        OK
                    </button>
                </div>
            </div>
        </div>
    </div>

    <style>
        #designer_saveconfigmenu li.e-menu-caret-icon {
            display: none !important;
        }
    </style>

    <script>

        function getQueryParam(name) {
            const urlParams = new URLSearchParams(window.location.search);
            return urlParams.get(name);
        }

        const API_BASE = window.WEB_API_URL;   // ✅ MUST end with /
        const REPORT_SERVICE_URL = API_BASE + "reports/api";

        // ✅ Passed from code-behind
        console.log("REPORT_SERVICE_URL:", REPORT_SERVICE_URL);
        const category = getQueryParam("category") || "";
        const name = getQueryParam("name") || "";

        const reportId = getQueryParam("id") || "";
        const isDraft = getQueryParam("isDraft") === "true";

        const REPORT_PATH = `${category}/${name}`;
        const SAFE_REPORT_PATH = encodeURI(REPORT_PATH);

        console.log("REPORT_PATH (JS):", SAFE_REPORT_PATH);

        async function getToken() {
            const res = await fetch(API_BASE + "reports/api/token");
            const data = await res.json();
            return data.access_token;
        }

        function initDesigner(token) {

            $("#designer").boldReportDesigner({

                serviceUrl: REPORT_SERVICE_URL,
                reportServiceUrl: REPORT_SERVICE_URL,

                serviceAuthorizationToken: "Bearer " + token,

                createNew: true,

                reportSaved: function () {

                    const modal =
                        new bootstrap.Modal(
                            document.getElementById('saveSuccessModal')
                        );

                    modal.show();
                },

                ajaxBeforeLoad: function (args) {

                    console.log("ActionType:", args.actionType);

                    args.headers = args.headers || [];

                    if (reportId) {
                        args.headers.push({
                            Key: "ReportId",
                            Value: reportId
                        });
                    }

                    args.headers.push({
                        Key: "ServiceAuthorizationToken",
                        Value: "bearer " + token
                    });

                    if (
                        args.actionType === "saveServerReport" ||
                        args.actionType === "openServerReport" ||
                        args.actionType === "createServerReport" ||
                        args.actionType === "HasDraftReport"
                    ) {

                        args.data = {
                            category: category,
                            reportName: name,
                            description: "",
                            isPublic: false,
                            isDraft: false,
                            isEdit: true,
                            tags: []
                        };

                        console.log("Save Payload:", args.data);
                    }
                }
            });

            // ✅ IMPORTANT: open after init
            setTimeout(function () {

                const designerObj = $("#designer").data("boldReportDesigner");

                if (!designerObj) return;

                if (reportId && !isDraft) {
                    designerObj.openReport(reportId);
                }
                else if (reportId && isDraft) {
                    designerObj.openReport(reportId);
                }
                else {
                    designerObj.newServerReport("New_Report");
                }

            }, 800);
        }

        async function loadDesigner() {
            try {
                const token = await getToken();
                initDesigner(token);
            } catch (e) {
                console.error("Edit ERROR:", e);
            }
        }

        loadDesigner();

    </script>

</asp:Content>
