<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_Recipient_Eligibility, App_Web_rnw0hezi" enableEventValidation="false" stylesheettheme="Default" %>

<%@ PreviousPageType TypeName="RegistrationProvider" %>
<%@ register tagprefix="jk" namespace="JK.BootstrapControls" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ register src="~/PopupControls/RecipientEligibilitySearch.ascx" tagprefix="uc1" tagname="RecipientEligibilitySearch" %>
<%@ register src="~/UserControls/SelfServiceProgressBar.ascx" tagprefix="uc1" tagname="SearchEligibilityProgressBar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="text-align: left !important"></div>
</asp:Content>
<asp:Content ID="RecipientEligibility" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnlRecipientEligibility" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto;">
        <div class="row">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <triggers>
                    <asp:PostBackTrigger ControlID="ucRegProgressBar" />
                </triggers>
                <contenttemplate>
                    <div id="DivProgressBar" runat="server">
                        <uc1:searcheligibilityprogressbar id="ucRegProgressBar" runat="server" />
                    </div>
                </contenttemplate>
            </asp:UpdatePanel>
        </div>
        <div id="DivRecipientEligibilitySearch" runat="server">
            <asp:Panel ID="pnlRecipientEligibilitySearch" runat="server">
                <uc1:recipienteligibilitysearch runat="server" id="ucRecipientEligibilitySearch" visible="true" enableviewstate="true" />
            </asp:Panel>
        </div>
    </asp:Panel>
</asp:Content>
