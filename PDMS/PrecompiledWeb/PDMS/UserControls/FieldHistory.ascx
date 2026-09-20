<%@ control language="C#" autoeventwireup="true" inherits="UserControls_FieldHistory, App_Web_p4ixifjm" %>
<asp:GridView runat="server" Width="100%" ID="grdHistory"  AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No entries found.">
    <Columns>
        <asp:BoundField ReadOnly="true" DataField="FieldValue" HeaderText="Value" />
        <asp:BoundField ReadOnly="true" DataField="UserID" HeaderText="Submitted By User ID" />
        <asp:BoundField ReadOnly="true" DataField="DateSubmitted" HeaderText="Date Submitted" />
        <asp:BoundField ReadOnly="true" DataField="ProviderStatus" HeaderText="Provider Status" />
    </Columns>
    <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />  
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" /> 
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>