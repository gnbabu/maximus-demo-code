<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_CorrespondenceAddressHistory" Codebehind="CorrespondenceAddressHistory.ascx.cs" %>
<br />
<asp:GridView runat="server" Width="98%" ID="grd" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found." style="margin-top:0px"
    AllowPaging="true" PageSize="10" AllowSorting="true" OnPageIndexChanging="grd_PageIndexChanging" OnSorting="grd_Sorting">
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
    <PagerStyle             CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle            CssClass="gridViewHeader" Width="120px" />
    <AlternatingRowStyle    CssClass="gridViewAltRow" />
    <RowStyle               CssClass="gridViewRow" />
    <FooterStyle            CssClass="gridViewFooter" />
</asp:GridView>