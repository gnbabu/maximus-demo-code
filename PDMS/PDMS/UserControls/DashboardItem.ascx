<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_DashboardItem" Codebehind="DashboardItem.ascx.cs" %>

<asp:MultiView ID="mltDashboardItem" runat="server">
    <asp:View ID="vwLabel" runat="server">
        <asp:Label ID="lblQuantity" runat="server" />
    </asp:View>
    <asp:View ID="vwLink" runat="server">
        <asp:LinkButton ID="lnkQuantity" runat="server" OnClick="lnkQuantity_Click" />
        <asp:Label ID="lblQuantityForNonAdmin" runat="server" />
    </asp:View>
</asp:MultiView>