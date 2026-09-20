<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_PrimaryPracticeLocationHistory, App_Web_glma3lal" %>
<br />
<asp:GridView runat="server" Width="98%"  ID="grd" AutoGenerateColumns="False" HorizontalAlign="Left"  EmptyDataText="No entries found." CssClass="gridview"
      AllowPaging="true" AllowSorting="True" PageSize="10"
        OnPageIndexChanging="grd_PageIndexChanging" OnSorting="grd_Sorting">
    <Columns>
        <asp:BoundField DataField="OPERATION"           HeaderText="Operation"      SortExpression="Operation" />
        <asp:BoundField DataField="NAME"            HeaderText="Name"        SortExpression="NAME" />
        <asp:BoundField DataField="ADDRESS1"            HeaderText="Address"        SortExpression="ADDRESS1" />
        <asp:BoundField DataField="EMAIL1"              HeaderText="Email"          SortExpression="EMAIL1" />
        <asp:BoundField DataField="PHONE1"              HeaderText="Phone"          SortExpression="PHONE1" />
        <asp:BoundField DataField="ADR_EFFECTIVE_DATE"  HeaderText="Effective Date" SortExpression="ADR_EFFECTIVE_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
        <asp:BoundField DataField="ADR_END_DATE"        HeaderText="End Date"       SortExpression="ADR_END_DATE"       DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
        <asp:BoundField DataField="UserName"            HeaderText="User Name"      SortExpression="UserName" />
        <asp:BoundField DataField="DateOfAction"        HeaderText="Update Date"    SortExpression="DateOfAction"       DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False" />
    </Columns>
    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>