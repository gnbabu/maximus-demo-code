<%@ page language="C#" autoeventwireup="true" masterpagefile="~/MasterPage.master" inherits="Process_ORProviderSearch, App_Web_rnw0hezi" enableEventValidation="false" stylesheettheme="Default" %>

<%@ PreviousPageType TypeName="RegistrationProvider" %>
<%@ register tagprefix="jk" namespace="JK.BootstrapControls" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ register src="~/PopupControls/ORProviderSearch.ascx" tagprefix="uc2" tagname="ORPProviderSearch" %>
<%@ register src="~/UserControls/SelfServiceProgressBar.ascx" tagprefix="uc1" tagname="SearchEligibilityProgressBar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="text-align: left !important"></div>
</asp:Content>
<asp:Content ID="cntORPProviderSearch" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnlRecipientEligibility" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto;">
        <div class="row">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <triggers>
                    <asp:PostBackTrigger ControlID="ucRegProgressBar" />
                </triggers>
                <contenttemplate>
                    <asp:Panel id="ProgressBarPanel" runat="server">
                        <div id="DivProgressBar" runat="server">
                            <uc1:searcheligibilityprogressbar id="ucRegProgressBar" runat="server" />
                        </div>
                    </asp:Panel>
                </contenttemplate>                
            </asp:UpdatePanel>
        </div>
        <div id="divORPProviderSearch" runat="server">
            <asp:Panel ID="pnlORPProviderSearch" runat="server">
                <uc2:orpproviderSearch runat="server" id="ucORPProviderSearch" visible="true" enableviewstate="true" />
            </asp:Panel>
        </div>
    </asp:Panel>
</asp:Content>

