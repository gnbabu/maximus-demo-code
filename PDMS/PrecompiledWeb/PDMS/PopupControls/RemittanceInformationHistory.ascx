<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_RemittanceInformationHistory, App_Web_l5y5araq" %>
<br />
<asp:GridView runat="server" Width="98%" ID="grd" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found." style="margin-top:0px">
    <Columns>
        <asp:BoundField DataField="PAYTO_ADDRESS_NAME" HeaderText="Name" SortExpression="PAYTO_ADDRESS_NAME" />
        <asp:BoundField DataField="CHECK_PAYABLE_TO_NAME" HeaderText="Pay To / Check Payable To Name" SortExpression="CHECK_PAYABLE_TO_NAME" />
        <asp:BoundField DataField="PAYTO_EMAIL_ADDRESS" HeaderText="Email Address" SortExpression="PAYTO_EMAIL_ADDRESS" />
        <asp:BoundField DataField="PAYTO_PHONE_NUMBER" HeaderText="Phone Number" SortExpression="PAYTO_PHONE_NUMBER" />
        <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
        <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" HeaderText="Update Date" SortExpression="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:d}" />
    </Columns>
    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>
