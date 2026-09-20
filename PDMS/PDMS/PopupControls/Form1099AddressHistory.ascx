<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_Form1099AddressHistory" Codebehind="Form1099AddressHistory.ascx.cs" %>
<br />
<asp:GridView runat="server" Width="98%" ID="grdForm1099AddressHistory" AutoGenerateColumns="False" HorizontalAlign="Left"  CssClass="gridview" 
    EmptyDataText="No entries found." onrowdatabound="grdForm1099AddressHistory_RowDataBound"
    AllowPaging="true" AllowSorting="true" PageSize="10"
    OnPageIndexChanging="grdForm1099AddressHistory_PageIndexChanging" OnSorting="grdForm1099AddressHistory_Sorting">
    <Columns>
        <asp:BoundField DataField="OPERATION"           HeaderText="Operation"      SortExpression="Operation" />
        <asp:BoundField DataField="ADDRESS1"            HeaderText="Address"        SortExpression="ADDRESS1" />
        <asp:BoundField DataField="EMAIL1"              HeaderText="Email"          SortExpression="EMAIL1" />
        <asp:BoundField DataField="PHONE1"              HeaderText="Phone"          SortExpression="PHONE1" />
        <asp:BoundField DataField="ADR_EFFECTIVE_DATE"  HeaderText="Effective Date" SortExpression="ADR_EFFECTIVE_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
        <asp:BoundField DataField="ADR_END_DATE"        HeaderText="End Date"       SortExpression="ADR_END_DATE"       DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
        <asp:BoundField DataField="TAX_ID_TYPE"         HeaderText="Tax ID Type"    SortExpression="TAX_ID_TYPE" />
        <asp:BoundField DataField="IRS_TAX_ID"          HeaderText="Tax ID"         SortExpression="IRS_TAX_ID" />
        <asp:BoundField DataField="IS_TAX_EXEMPT"       HeaderText="Exempt"         SortExpression="IS_TAX_EXEMPT" />
        <asp:BoundField DataField="IS_FORM_W9"          HeaderText="Form W9"        SortExpression="IS_FORM_W9" />
        <asp:BoundField DataField="IS_FORM_147"         HeaderText="Form 147"       SortExpression="IS_FORM_147" />
        <asp:BoundField DataField="UserName"            HeaderText="User Name"      SortExpression="UserName" />
        <asp:BoundField DataField="DateOfAction"        HeaderText="Update Date"    SortExpression="DateOfAction"       DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False" />
    </Columns>
    <PagerStyle             CssClass="gridpager"        HorizontalAlign="Right" />
    <HeaderStyle            CssClass="gridViewHeader"   Width="100px" />
    <AlternatingRowStyle    CssClass="gridViewAltRow" />
    <RowStyle               CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>