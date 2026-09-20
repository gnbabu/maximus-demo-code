<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_MedicareHistory, App_Web_l5y5araq" %>
<br />
<asp:GridView runat="server" Width="98%" ID="grdMedicare" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found.">
    <Columns>
        <asp:BoundField DataField="MEDICARE_NUMBER" HeaderText="Other State Medicaid ID" SortExpression="MEDICAID_NUMBER" />
        <asp:BoundField DataField="MEDICARE_EFF_DATE" HeaderText="Issue Date" SortExpression="MEDICARE_EFF_DATE" DataFormatString="{0:d}" />
        <asp:BoundField DataField="MEDICARE_END_DATE" HeaderText="End Date" SortExpression="MEDICARE_END_DATE" DataFormatString="{0:d}" />
        <asp:BoundField DataField="UserName" HeaderText="User Name" />
        <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" HeaderText="Update Date" />
    </Columns>
    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>