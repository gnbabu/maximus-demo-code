<%@ Page Language="C#" AutoEventWireup="true" masterpagefile="~/MasterPage.master" Inherits="Process_ORProviderSearchMenu" Codebehind="ORProviderSearchMenu.aspx.cs" %>

<%@ register tagprefix="jk" namespace="JK.BootstrapControls" Assembly="PDMS" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ register src="~/PopupControls/ORProviderSearch.ascx" tagprefix="uc2" tagname="ORPProviderSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="text-align: left !important"></div>
</asp:Content>
<asp:Content ID="cntORPProviderSearch" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnlRecipientEligibility" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto;">
        <div id="divORPProviderSearch" runat="server">
            <asp:Panel ID="pnlORPProviderSearch" runat="server">
                <uc2:orpproviderSearch runat="server" id="ucORPProviderSearch" visible="true" enableviewstate="true" />
            </asp:Panel>
        </div>
    </asp:Panel>
</asp:Content>
