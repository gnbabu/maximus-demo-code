<%@ control language="C#" autoeventwireup="true" inherits="UserControls_DashboardItem, App_Web_p4ixifjm" %>

<asp:MultiView ID="mltDashboardItem" runat="server">
    <asp:View ID="vwLabel" runat="server">
        <asp:Label ID="lblQuantity" runat="server" />
    </asp:View>
    <asp:View ID="vwLink" runat="server">
        <asp:LinkButton ID="lnkQuantity" runat="server" OnClick="lnkQuantity_Click" />
        <asp:Label ID="lblQuantityForNonAdmin" runat="server" />
    </asp:View>
</asp:MultiView>