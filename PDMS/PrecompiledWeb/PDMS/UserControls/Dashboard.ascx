<%@ control language="C#" autoeventwireup="true" inherits="UserControls_Dashboard, App_Web_ejpeldiy" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Src="DashboardItem.ascx" TagName="DashboardItem" TagPrefix="cc2" %>

<asp:GridView ID="gvDashboard" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" Width="100%"
    CssClass="gridViewSmallFont dashboard" EmptyDataText="No dashboard found." RowHeaderColumn="Status">
    <Columns>
        <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="30%" />   
        <asp:TemplateField HeaderText="0-30 Days">
            <ItemTemplate>
                <cc2:DashboardItem ID="ucDashboard0" runat="server" TableId='<%# Bind("TableId") %>' Quantity='<%# Bind("Days0_30") %>'
                    StatusID='<%# Bind("StatusID") %>' Ordinal="0" DashboardLabel='<%# System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(DataBinder.Eval(Container.DataItem,"Status") + "/0-30 Days", true) %>' IsAssigned='<%# Convert.ToBoolean(Eval("IsAssigned")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="31-60 Days">
            <ItemTemplate>
                <cc2:DashboardItem ID="ucDashboard1" runat="server" TableId='<%# Bind("TableId") %>' Quantity='<%# Bind("Days31_60") %>'
                    StatusID='<%# Bind("StatusID") %>' Ordinal="1" DashboardLabel='<%# System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(DataBinder.Eval(Container.DataItem,"Status") + "/31-60 Days", true) %>' IsAssigned='<%# Convert.ToBoolean(Eval("IsAssigned")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="61-90 Days">
            <ItemTemplate>
                <cc2:DashboardItem ID="ucDashboard2" runat="server" TableId='<%# Bind("TableId") %>' Quantity='<%# Bind("Days61_90") %>'
                    StatusID='<%# Bind("StatusID") %>' Ordinal="2" DashboardLabel='<%# System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(DataBinder.Eval(Container.DataItem,"Status") + "/61-90 Days", true) %>' IsAssigned='<%# Convert.ToBoolean(Eval("IsAssigned")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="91+ Days">
            <ItemTemplate>
                <cc2:DashboardItem ID="ucDashboard3" runat="server" TableId='<%# Bind("TableId") %>' Quantity='<%# Bind("Days90Plus") %>'
                    StatusID='<%# Bind("StatusID") %>' Ordinal="3" DashboardLabel='<%# System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(DataBinder.Eval(Container.DataItem,"Status") + "/91 Plus Days", true) %>' IsAssigned='<%# Convert.ToBoolean(Eval("IsAssigned")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Total">
            <ItemTemplate>
                <cc2:DashboardItem ID="ucDashboard4" runat="server" TableId='<%# Bind("TableId") %>' Quantity='<%# Bind("Total") %>'
                    StatusID='<%# Bind("StatusID") %>' Ordinal="99" DashboardLabel='<%# System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(DataBinder.Eval(Container.DataItem,"Status") + "/Total", true) %>' IsAssigned='<%# Convert.ToBoolean(Eval("IsAssigned")) %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle CssClass="gridViewHeader" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>
