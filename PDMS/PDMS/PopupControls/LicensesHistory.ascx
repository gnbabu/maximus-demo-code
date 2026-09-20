<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_LicensesHistory" Codebehind="LicensesHistory.ascx.cs" %>
<br />
<asp:ValidationSummary ID="LicensesHistoryValidationID" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="LicensesHistoryValidationGP" />
<asp:GridView runat="server" Width="100%" ID="grd" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" 
    PageSize="10" AllowPaging="true" AllowSorting="true" EmptyDataText="No entries found." OnPageIndexChanging="grd_PageIndexChanging" OnSorting="grd_Sorting">
    <Columns>
        <asp:BoundField DataField="Operation"               HeaderText="Operation"              SortExpression="Operation" />
        <asp:BoundField DataField="LICENSE_NUMBER"          HeaderText="License Number"         SortExpression="LICENSE_NUMBER" />
        <asp:BoundField DataField="UserName"                HeaderText="User Name"              SortExpression="UserName" />
        <asp:BoundField DataField="LICENSE_BOARD_NAME"      HeaderText="License Board"          SortExpression="LICENSE_BOARD_NAME" />
        <asp:BoundField DataField="LICENSE_STATE"           HeaderText="License Issuing State"  SortExpression="LICENSE_STATE" />
        <asp:Boundfield Datafield="LICENSE_STATUS"          HeaderText="License Status"         SortExpression="LICENSE_STATUS" />
        <asp:Boundfield Datafield="LICENSE_SUBSTATUS"       HeaderText="License Sub-Status"     SortExpression="LICENSE_SUBSTATUS" />
        <asp:Boundfield Datafield="ELICENSE_VERIFIED"       HeaderText="eLicense Verified"      SortExpression="ELICENSE_VERIFIED" />
        <asp:BoundField DataField="LICENSE_EFF_DATE"        HeaderText="Effective Date" DataFormatString="{0:MM/dd/yyyy}"    SortExpression="LICENSE_EFF_DATE" />
        <asp:BoundField DataField="LICENSE_END_DATE"        HeaderText="Expiration Date" DataFormatString="{0:MM/dd/yyyy}"   SortExpression="LICENSE_END_DATE" />
        <asp:BoundField DataField="DateOfAction"            HeaderText="Update Date"    SortExpression="DateOfAction"   DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False"  />
<%--         <asp:TemplateField HeaderText="Address" ItemStyle-Width="20%">
                            <ItemTemplate>
                                <asp:Literal ID="litAddress" runat="server" Text='<%# FormatAddress(Eval("REG_ADDRESSID"))%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                <asp:TemplateField HeaderText="Endorsement">
                            <ItemTemplate>
                                <asp:Literal ID="litEndorsement" runat="server" Text='<%# FormatSpecialtyFocus(Eval("REG_LICENSURE_ID"))%>'  />
                            </ItemTemplate>
                </asp:TemplateField>--%>
    </Columns>
    <PagerStyle             CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle            CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle    CssClass="gridViewAltRow" />
    <RowStyle               CssClass="gridViewRow" />
    <FooterStyle            CssClass="gridViewFooter" />
</asp:GridView>
