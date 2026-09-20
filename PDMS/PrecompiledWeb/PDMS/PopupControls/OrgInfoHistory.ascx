<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_OrgInfoHistory, App_Web_c4une0e1" %>

<br />
<asp:GridView runat="server" Width="98%" ID="grdOrgInfoHistory" AutoGenerateColumns="False" HorizontalAlign="Left" 
    CssClass="gridview" EmptyDataText="No entries found.">
    <Columns>
        <asp:BoundField DataField="NAME" HeaderText="Legal Name" />
        <asp:BoundField DataField="DBA" HeaderText="DBA" />
        <asp:BoundField DataField="NPI" HeaderText="NPI" />
        <asp:BoundField DataField="TAX_ID" HeaderText="Tax ID" />
        <asp:BoundField DataField="PROVIDER_TYPE_NAME" HeaderText="Provider Type" />
        <asp:BoundField DataField="CHANGE_EFFECTIVE_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False"
            HeaderText="Effective Date" />
        <asp:BoundField DataField="LAST_MODIFIED_USERNAME" HeaderText="Last Modified User" />
        <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
            HeaderText="Last Modified Date" />
    </Columns>
    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>
