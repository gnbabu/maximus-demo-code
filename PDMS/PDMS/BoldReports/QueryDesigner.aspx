<%@ Page Title="Provider Credentialing Query Designer" Language="C#"
    MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeBehind="QueryDesigner.aspx.cs"
    Inherits="BoldReports_QueryDesigner" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <link href="<%= ResolveUrl("~/Content/bold-reports/v2.0/tailwind-light/bold.report-designer.min.css") %>" rel="stylesheet" />

    <script src="<%= ResolveUrl("~/Scripts/bold-reports/v2.0/common/bold.reports.common.min.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/bold-reports/v2.0/common/bold.reports.widgets.min.js") %>"></script>
    <script src="<%= ResolveUrl("~/Scripts/bold-reports/v2.0/bold.report-designer.min.js") %>"></script>

    <div id="queryDesigner" style="height: 800px;"></div>

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

        function waitForQueryDesigner(token) {

            let attempts = 0;

            const interval = setInterval(() => {

                if (typeof $.fn.boldReportQueryDesigner === "function") {

                    clearInterval(interval);

                    log("✅ Query Designer ready");

                    $("#queryDesigner").boldReportQueryDesigner({

                        serviceUrl: REPORT_SERVICE_URL,

                        serviceAuthorizationToken: "bearer " + token,

                        // ✅ Called when initialized
                        create: async function () {

                            console.log("✅ Query Designer initialized");

                            const obj = $("#queryDesigner").data("boldReportQueryDesigner");

                            try {

                                const res = await fetch(REPORT_SERVICE_URL + "/PostDesignerAction", {
                                    method: "POST",
                                    headers: {
                                        "Content-Type": "application/json",
                                        "serviceauthorizationtoken": "bearer " + token,
                                        "Accept": "application/json, text/javascript, */*; q=0.01",
                                        "ServerUrl": API_BASE + "reporting/api/site/dev"
                                    },
                                    body: JSON.stringify({
                                        designerAction: "dataSourceCatalog",
                                        actionType: "dataSourceCatalog",
                                        itemId: null,
                                        itemType: null
                                    })
                                });

                                const result = await res.json();

                                console.log("✅ DataSources:", result);

                                let dataSources = [];

                                for (let ds of result.Data) {

                                    let dataSource = window.ej.ReportUtil.createDataSource();

                                    dataSource.DataSourceReference = ds.Id;
                                    dataSource.Name = ds.Name;

                                    dataSources.push(dataSource);
                                }

                                // ✅ THIS is the magic line
                                obj.newDataSet("CRED", dataSources);

                            } catch (e) {
                                console.error("❌ DataSource load failed", e);
                            }

                            // ✅ Preview hook (keep your existing)
                            obj._renderingGridData = obj.renderingGridData;

                            obj.renderingGridData = function (data, headers) {

                                console.log("Preview Data:", data);

                                obj._renderingGridData(data, headers);
                            };
                        },


                        ajaxBeforeLoad: function (args) {

                            // ✅ SAFETY CHECK (critical fix)
                            if (args.url && !args.url.startsWith("http")) {
                                args.url = REPORT_SERVICE_URL + "/" + args.url.replace(/^\/+/, "");
                            }

                            // ✅ Ensure POST
                            if (!args.type) {
                                args.type = "POST";
                            }

                            // ✅ Attach token
                            args.headers = args.headers || [];
                            args.headers.push({
                                Key: "serviceauthorizationtoken",
                                Value: "bearer " + token
                            });

                            console.log("✅ Query API:", args.url, args.actionType);
                        },

                        // ✅ Cleaner toolbar (optional)
                        toolbarSettings: {
                            items: 131071 & ~4 & ~1 // remove Save + New
                        }

                    });

                } else {
                    log("⏳ Waiting for Query Designer...");
                }

                if (attempts++ > 30) {
                    clearInterval(interval);
                    console.error("❌ Query Designer failed to initialize");
                }

            }, 200);
        }

        async function loadQueryDesigner() {

            try {

                const token = await getToken();
                waitForQueryDesigner(token);

            } catch (e) {

                console.error("Query Designer ERROR:", e);

            }
        }

        $(document).ready(function () {
            loadQueryDesigner();
        });

    </script>

</asp:Content>
