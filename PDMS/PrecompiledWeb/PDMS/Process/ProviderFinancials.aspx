<%@ page language="C#" autoeventwireup="true" masterpagefile="~/MasterPage.master" inherits="Process_ProviderFinancials, App_Web_rnw0hezi" enableEventValidation="false" stylesheettheme="Default" %>

<%@ PreviousPageType TypeName="RegistrationProvider" %>
<%@ register tagprefix="jk" namespace="JK.BootstrapControls" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ register src="~/PopupControls/ProviderFinancial.ascx" tagprefix="ucPF" tagname="ProviderFinancial" %>
<%@ register src="~/UserControls/SelfServiceProgressBar.ascx" tagprefix="uc1" tagname="SearchEligibilityProgressBar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="text-align: left !important"></div>
</asp:Content>
<asp:Content ID="providerFinancialID" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnlProviderFinancial" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto;">
        <div class="row">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <triggers>
                    <asp:PostBackTrigger ControlID="ucRegProgressBar" />
                </triggers>
                <contenttemplate>
                    <div id="divProgressBar" runat="server">
                        <uc1:searcheligibilityprogressbar id="ucRegProgressBar" runat="server" />
                    </div>
                </contenttemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divProviderFinancialSearch" runat="server">
            <asp:Panel ID="pnlProviderFinancialSearch" runat="server">
                <ucPF:providerFinancial runat="server" id="ucProviderFinancial" visible="true" enableviewstate="true" />
            </asp:Panel>
        </div>
    </asp:Panel>
</asp:Content>