<%@ control language="C#" autoeventwireup="true" inherits="UserControls_DashboardProviderSummary, App_Web_p4ixifjm" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>

    <asp:GridView ID="gvDashboard" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center" Width="100%" 
            CssClass="gridViewSmallFont dashboard" EmptyDataText="No dashboard found." OnRowDataBound="gvDashboard_RowDataBound" RowHeaderColumn="ProviderType">
        <Columns>
            <asp:BoundField DataField="ProviderType" HeaderText="Provider Type" ItemStyle-Width="20%" />            
            <asp:BoundField DataField="Active" HeaderText="Active" ItemStyle-Width="7%" />
            <asp:BoundField DataField="Maintenance" HeaderText="Maintenance" ItemStyle-Width="7%" />
            <asp:BoundField DataField="Conversion" HeaderText="Conversion" ItemStyle-Width="7%" />
            <asp:BoundField DataField="RevalidationDue" HeaderText="Revalidation Due < 30 Days" ItemStyle-Width="12%" />
            <asp:BoundField DataField="RevalidationInProgress" HeaderText="Revalidation In Progress" ItemStyle-Width="12%" />
            <asp:BoundField DataField="Updates" HeaderText="Update in Progress" ItemStyle-Width="7%" />
            <asp:BoundField DataField="New" HeaderText="New In Progress" ItemStyle-Width="7%" />
            <asp:BoundField DataField="Inactive" HeaderText="Inactive" ItemStyle-Width="7%" />
            <asp:BoundField DataField="Terminated" HeaderText="Terminated" ItemStyle-Width="7%" />
            <asp:BoundField DataField="Denied" HeaderText="Denied" ItemStyle-Width="7%" />
        </Columns>   
        <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />  
        <HeaderStyle CssClass="gridViewHeader" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" /> 
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
