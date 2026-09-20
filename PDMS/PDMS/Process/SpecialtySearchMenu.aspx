<%@ Page Language="C#" AutoEventWireup="true" masterpagefile="~/MasterPage.master" Inherits="Process_SpecialtySearchMenu" Codebehind="SpecialtySearchMenu.aspx.cs" %>

<%@ register tagprefix="jk" namespace="JK.BootstrapControls" Assembly="PDMS" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ register src="~/PopupControls/ProviderSpecialtySearch1.ascx" tagprefix="uc2" tagname="ProvSpecialtySearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <%--<div style="text-align: left !important"></div>--%>   
    <div class="row" style="text-align:center;padding-top:20px;padding-bottom:20px;">
        <span>Provider Specialty Search</span>
    </div>
</asp:Content>
<asp:Content ID="cntProvSpecialtySearch" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnlProvSpecialtySearchMenu" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto;">
        <div id="divProvSpecialtySearch" runat="server">
            <asp:Panel ID="pnlProvSpecialtySearch" runat="server">
                <uc2:ProvSpecialtySearch runat="server" id="ucProvSpecialtySearch" visible="true" enableviewstate="true" />
            </asp:Panel>
        </div>
    </asp:Panel>
</asp:Content>
