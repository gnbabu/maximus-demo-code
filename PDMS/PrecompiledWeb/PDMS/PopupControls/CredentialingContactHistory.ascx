<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_CredentialingContactHistory, App_Web_l5y5araq" %>
<br />
<asp:GridView runat="server" ID="grdCredentialingContact" AllowPaging="true" PageSize="10" AutoGenerateColumns="False"
    HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No records found" OnPageIndexChanging="grdCredentialingContact_PageIndexChanging" OnSorting="grdCredentialingContact_Sorting" >
    <Columns>
        <asp:BoundField DataField="OPERATION"       HeaderText="Operation"      SortExpression="Operation" />
        <asp:BoundField DataField="CONTACT_NAME"    HeaderText="Contact Name"   SortExpression="CONTACT_NAME" />
        <asp:BoundField DataField="PRACTICE_NAME"   HeaderText="Practice Name"  SortExpression="PRACTICE_NAME" />
        <asp:BoundField DataField="CONTACT_NUMBER"  HeaderText="Phone"          SortExpression="CONTACT_NUMBER" />
        <asp:BoundField DataField="EMAIL_ID"        HeaderText="Email"          SortExpression="EMAIL_ID" />
        <asp:BoundField DataField="UserName"        HeaderText="User Name"      SortExpression="UserName" />
        <asp:BoundField DataField="DateOfAction"    HeaderText="Update Date"    SortExpression="DateOfAction"   DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False"  />
    </Columns>
    <PagerStyle             CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle            CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle    CssClass="gridViewAltRow" />
    <RowStyle               CssClass="gridViewRow" />
    <FooterStyle            CssClass="gridViewFooter" />
</asp:GridView>
