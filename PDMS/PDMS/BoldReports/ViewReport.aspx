<%@ Page Title="Provider Credentialing Reports Viewer" Language="C#" MasterPageFile="~/MasterPage.master" 
    AutoEventWireup="true" CodeBehind="ViewReport.aspx.cs" Inherits="BoldReports_ReportsViewReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <link href="<%= ResolveUrl("~/Content/bold-reports/v2.0/tailwind-light/bold.report-designer.min.css") %>" rel="stylesheet" />

    <!-- The Bold Reports SDK requires jQuery 3.x internally. Load it here, let the
         SDK bind its plugins to it, then hand control back to the MasterPage's own
         jQuery 2.0.3 (which Bootstrap 3 / modalPlugin / the hamburger menu depend
         on) via noConflict so the two versions never clash on this page. -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="<%= ResolveUrl("~/Scripts/bold-reports/v2.0/common/bold.reports.common.min.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/bold-reports/v2.0/common/bold.reports.widgets.min.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/bold-reports/v2.0/bold.report-viewer.min.js") %>"></script>
    <script>
        // Restore window.$ / jQuery to the MasterPage's jQuery 2.0.3; jq3 keeps
        // the jQuery 3.6 instance the Bold Reports SDK above just bound itself to.
        var jq3 = jQuery.noConflict(true);
    </script>

    <style>
        #viewer {
            height: 800px;
        }

        .debug {
            margin-top: 10px;
            padding: 10px;
            background: #f5f5f5;
            border: 1px solid #ccc;
            font-size: 12px;
            max-height: 250px;
            overflow-y: auto;
        }

        .error {
            color: red;
        }
    </style>

    <div id="viewer"></div>
    <div id="error" class="error"></div>

<script>

    const API_BASE = window.WEB_API_URL;   // ✅ same as list page
    const REPORT_SERVICE_URL = API_BASE + "reports/api";  // ✅ adjust if needed

    let cachedToken = null;

    // ✅ LOGGER
    function log(msg, data = null) {
        console.log(msg, data || "");
    }

    // ✅ GET QUERY PARAMS
    function getQueryParam(name) {
        const urlParams = new URLSearchParams(window.location.search);
        return urlParams.get(name);
    }

    const category = getQueryParam("category");
    const name = getQueryParam("name");

    log("Category:", category);
    log("Report Name:", name);

    // ✅ TOKEN
    async function getToken() {

        if (cachedToken) {
            log("Using cached token");
            return cachedToken;
        }

        const url = `${API_BASE}reports/api/token`;
        log("Fetching token from:", url);

        const response = await fetch(url);
        const text = await response.text();

        log("Token response:", text);

        if (!response.ok) {
            throw new Error(text);
        }

        const tokenObj = JSON.parse(text);
        cachedToken = tokenObj.access_token;

        log("Token fetched ✅");

        return cachedToken;
    }

    function waitForViewerAndInit(token, reportPath) {

        let attempts = 0;

        const interval = setInterval(() => {

            if (typeof jq3.fn.boldReportViewer === "function") {

                clearInterval(interval);

                log("✅ Viewer ready, initializing...");

                jq3("#viewer").boldReportViewer({
                    reportServiceUrl: REPORT_SERVICE_URL,
                    reportPath: reportPath,
                    serviceAuthorizationToken: "bearer " + token,

                    // ✅ Fires when report is loaded
                    reportLoaded: function () {
                        adjustViewerHeight();
                    },

                    // ✅ Fires after rendering finishes
                    renderComplete: function () {
                        adjustViewerHeight();
                    }

                });

            } else {
                log("⏳ Waiting for viewer script...");
            }

            if (attempts++ > 20) {
                clearInterval(interval);
                log("❌ Viewer script never initialized");
            }

        }, 200);
    }

    function adjustViewerHeight() {

        setTimeout(() => {

            // Try to find the actual report page container
            const reportContent = document.querySelector(".e-reportviewer-page");

            if (reportContent) {
                const contentHeight = reportContent.scrollHeight;

                const finalHeight = contentHeight + 50; // padding buffer

                document.getElementById("viewer").style.height = finalHeight + "px";

                console.log("✅ Viewer resized to:", finalHeight);
            } else {
                console.log("⚠️ Report content not found, fallback height used");
            }

        }, 300); // wait for DOM paint
    }

    // ✅ LOAD VIEWER
    async function loadViewer() {

        try {

            const token = await getToken();

            const reportPath = `${category}/${name}`;

            log("Initializing viewer");
            log("Report Path:", reportPath);
            log("Report Service URL:", REPORT_SERVICE_URL);

            waitForViewerAndInit(token, reportPath);

        } catch (err) {
            log("Viewer ERROR:", err.message);
            document.getElementById("error").innerText = err.message;
        }
    }

    loadViewer();

</script>

</asp:Content>

