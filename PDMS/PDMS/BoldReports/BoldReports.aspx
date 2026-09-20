<%@ Page Title="Bold Reports"
    Language="C#"
    AutoEventWireup="true"
    MasterPageFile="~/MasterPage.master"
    CodeBehind="BoldReports.aspx.cs"
    Inherits="BoldReports_BoldReports" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Styles -->
    <link href="https://cdn.boldreports.com/11.1.10/content/v2.0/tailwind-light/bold.report-viewer.min.css"
          rel="stylesheet" />

    <style>
        .report-viewer-container {
            height: 700px;
            width: 100%;
            border: 1px solid #dee2e6;
            border-radius: 4px;
        }
    </style>

    <!-- Title -->
    <h4>
        <asp:Literal ID="litReportTitle" runat="server" Text=""></asp:Literal>
    </h4>

    <!-- Error Panel -->
    <asp:Panel ID="pnlViewerError" runat="server" Visible="false" CssClass="alert alert-danger">
        <asp:Literal ID="litViewerError" runat="server" Text=""></asp:Literal>
    </asp:Panel>

    <!-- Hidden Config -->
    <asp:HiddenField ID="hfReportPath" runat="server" />
    <asp:HiddenField ID="hfAuthToken" runat="server" />
    <asp:HiddenField ID="hfReportServiceUrl" runat="server" />
    <asp:HiddenField ID="hfReportServerUrl" runat="server" />

    <!-- ✅ Report List -->
    <asp:Panel ID="pnlReportList" runat="server" Visible="false">

        <h4>Available Reports</h4>

        <asp:GridView ID="gvReports"
            runat="server"
            AutoGenerateColumns="false"
            CssClass="table table-striped"
            OnRowCommand="gvReports_RowCommand">

            <Columns>
                <asp:BoundField DataField="ReportName" HeaderText="Report Name" />

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton
                            ID="btnView"
                            runat="server"
                            CommandName="ViewReport"
                            CommandArgument='<%# Eval("ReportPath") %>'
                            Text="View" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>

    </asp:Panel>

    <!-- Viewer -->
    <asp:Panel ID="pnlViewer" runat="server" Visible="false">
        <div class="report-viewer-container">
            <div id="report-viewer" style="height:100%; width:100%;"></div>
        </div>
    </asp:Panel>

    <!-- Scripts -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

    <script src="https://cdn.boldreports.com/11.1.10/scripts/v2.0/common/bold.reports.common.min.js"></script>
    <script src="https://cdn.boldreports.com/11.1.10/scripts/v2.0/common/bold.reports.widgets.min.js"></script>
    <script src="https://cdn.boldreports.com/11.1.10/scripts/v2.0/bold.report-viewer.min.js"></script>

    <script type="text/javascript">
        $(function () {

            var reportPath = $("#<%= hfReportPath.ClientID %>").val();
            var authToken = $("#<%= hfAuthToken.ClientID %>").val();
            var reportServiceUrl = $("#<%= hfReportServiceUrl.ClientID %>").val();

            if (!reportPath) {
                console.log("View-all mode, skipping report viewer init");
                return;
            }

            if (!authToken || !reportServiceUrl) {
                console.warn("Missing viewer configuration.");
                return;
            }

            try {
                $("#report-viewer").boldReportViewer({
                    reportServiceUrl: reportServiceUrl,
                    reportPath: reportPath,
                    serviceAuthorizationToken: authToken,
                    processingMode: ej.ReportViewer.ProcessingMode.Remote,

                    ajaxBeforeLoad: function (args) {
                        if (!args.headers) args.headers = [];
                        args.headers.push({
                            Key: "Authorization",
                            Value: authToken
                        });
                    }
                });

            } catch (e) {
                console.error("Report Viewer Error:", e);
                alert("Error loading report: " + (e.message || e));
            }

        });
    </script>

</asp:Content>