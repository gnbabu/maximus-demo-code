<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Process_Recipient_Eligibility" Codebehind="RecipientEligibility.aspx.cs" %>

<%@ PreviousPageType TypeName="RegistrationProvider" %>
<%@ Register TagPrefix="jk" Namespace="JK.BootstrapControls" Assembly="PDMS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/RecipientEligibilitySearch.ascx" TagPrefix="uc1" TagName="RecipientEligibilitySearch" %>
<%@ Register Src="~/UserControls/SelfServiceProgressBar.ascx" TagPrefix="uc1" TagName="SearchEligibilityProgressBar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="text-align: left !important"></div>
</asp:Content>
<asp:Content ID="RecipientEligibility" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="WhiteBox">


        <asp:Panel ID="pnlRecipientEligibility" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto;">
            <div class="row">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="ucRegProgressBar" />
                    </Triggers>
                    <ContentTemplate>
                        <div id="DivProgressBar" runat="server">
                            <uc1:SearchEligibilityProgressBar ID="ucRegProgressBar" runat="server" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="DivRecipientEligibilitySearch" runat="server">
                <asp:Panel ID="pnlRecipientEligibilitySearch" runat="server">
                    <uc1:RecipientEligibilitySearch runat="server" ID="ucRecipientEligibilitySearch" Visible="true" EnableViewState="true" />
                </asp:Panel>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
