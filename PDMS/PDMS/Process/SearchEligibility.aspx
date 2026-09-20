<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_SearchEligibility" Codebehind="SearchEligibility.aspx.cs" %>

<%@ PreviousPageType TypeName="RegistrationProvider" %>
<%@ register tagprefix="jk" namespace="JK.BootstrapControls" Assembly="PDMS" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ register src="~/PopupControls/SearchMemberEligibility.ascx" tagprefix="uc2" tagname="RecipientEligibilitySearchV2" %>
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
        <div id="divRecipientEligibilitySearchv2" runat="server">
            <asp:Panel ID="pnlRecipientEligibilitySearchv2" runat="server">
                <uc2:recipienteligibilitysearchv2 runat="server" id="ucRecipientEligibilitySearchv2" visible="true" enableviewstate="true" />
            </asp:Panel>
        </div>
    </asp:Panel>
</asp:Content>
