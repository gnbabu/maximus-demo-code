<%@ Page Title="Provider Credentialing Reports Designer" Language="C#"     
    MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeBehind="DesignReport.aspx.cs" Inherits="BoldReports_ReportsDesignReport" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <link href="<%= ResolveUrl("~/Content/bold-reports/v2.0/tailwind-light/bold.report-designer.min.css") %>" rel="stylesheet" />

    <!-- The Bold Reports SDK requires jQuery 3.x internally. Load it here, let the
         SDK bind its plugins to it, then hand control back to the MasterPage's own
         jQuery 2.0.3 (which Bootstrap 3 / modalPlugin / the hamburger menu depend
         on) via noConflict so the two versions never clash on this page. -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="<%= ResolveUrl("~/Scripts/bold-reports/v2.0/common/bold.reports.common.min.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/bold-reports/v2.0/common/bold.reports.widgets.min.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/bold-reports/v2.0/bold.report-designer.min.js") %>"></script>
    <script>
        // Restore window.$ / jQuery to the MasterPage's jQuery 2.0.3; jq3 keeps
        // the jQuery 3.6 instance the Bold Reports SDK above just bound itself to.
        var jq3 = jQuery.noConflict(true);
    </script>

    <div id="designer" style="height: 800px;"></div>

    <script>

        const API_BASE = window.WEB_API_URL;
        const REPORT_SERVICE_URL = API_BASE + "reports/api";

        function log(label, value) {
            console.log(label + ":", value);
        }

        async function getToken() {
            const res = await fetch(API_BASE + "reports/api/token");
            const data = await res.json();

            log("Token fetched", data);

            return data.access_token;
        }

        function waitForDesigner(token) {

            let attempts = 0;

            const interval = setInterval(() => {

                if (typeof jq3.fn.boldReportDesigner === "function") {

                    clearInterval(interval);

                    log("✅ Designer ready");

                    jq3("#designer").boldReportDesigner({

                        reportServiceUrl: REPORT_SERVICE_URL,

                        serviceAuthorizationToken: "bearer " + token,

                        ajaxBeforeLoad: function (args) {

                            // ✅ FIX 1: Ensure URL is never null
                            if (!args.url.startsWith("http")) {
                                args.url = REPORT_SERVICE_URL + "/" + args.url.replace(/^\/+/, "");
                            }

                            // ✅ FIX 2: Force POST for API calls
                            if (!args.type) {
                                args.type = "POST";
                            }

                            // ✅ FIX 3: Always send token
                            args.headers = args.headers || [];
                            args.headers.push({
                                Key: "serviceauthorizationtoken",
                                Value: "bearer " + token
                            });

                            console.log("✅ Designer Request:", args.url);
                        },

                        createNew: true
                    });

                } else {
                    log("⏳ Waiting for designer...");
                }

                if (attempts++ > 30) {
                    clearInterval(interval);
                    console.error("❌ Designer failed to initialize");
                }

            }, 200);
        }

        async function loadDesigner() {
            try {
                const token = await getToken();

                waitForDesigner(token);

            } catch (e) {
                console.error("Designer ERROR:", e);
            }
        }


        jq3(document).ready(function () {
            loadDesigner();
        });


    </script>

</asp:Content>
