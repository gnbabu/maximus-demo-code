<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_AdditionalTaxonomyCodeHistory, App_Web_l5y5araq" %>
<br />
<asp:GridView runat="server" Width="98%" ID="grd" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found.">
    <Columns>
        <asp:BoundField DataField="TAXONOMY_CODE" HeaderText="Primary Taxonomy Code" SortExpression="TAXONOMY_CODE" />
        <asp:BoundField DataField="TAXONOMY_NAME" HeaderText="Taxonomy Code Description" SortExpression="TAXONOMY_NAME" />
        <asp:BoundField DataField="START_DATE" HeaderText="Start Date" DataFormatString="{0:d}" Visible="false" SortExpression="START_DATE" />
        <asp:BoundField DataField="END_DATE" HeaderText="EndDate" DataFormatString="{0:d}" Visible="false" SortExpression="END_DATE" />
        <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
        <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" HeaderText="Update Date" SortExpression="LAST_MODIFIED_DATE_TIME" />
    </Columns>
    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>
