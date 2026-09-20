<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_PharmacySection" Codebehind="PharmacySection.ascx.cs" %>
<%@ Register Src="~/PopupControls/PharmacyProviders.ascx" TagName="PharmacyProviders" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/PharmacyPharmacist.ascx" TagName="PharmacyPharmacist" TagPrefix="uc2" %>

<asp:Panel ID="pnlPharmacyProviders" runat="server" Style="display: inline-block; width: 100%; padding-bottom: 20px; margin-bottom: 0px;">
    <uc1:Separator ID="Separator15" runat="server" Header="Pharmacy Providers" />
    <br />
    <div style="text-align: right">
        <%--<asp:ImageButton ID="btnPharmacyHistory" runat="server" Text="History" CommandName="PharmacyProviders" ImageUrl="~/Images/history_icon.jpg" ToolTip="History" Visible="false" OnCommand="btnHistory_Click" />--%>
    </div>
    <uc:PharmacyProviders ID="ucPharmacyProviders" runat="server" />
    <br />
    <br />

    <uc2:PharmacyPharmacist id="ucPharmacyPharmacist" runat="server" />
</asp:Panel>
