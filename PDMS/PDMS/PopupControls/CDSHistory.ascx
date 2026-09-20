<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_CDSHistory" Codebehind="CDSHistory.ascx.cs" %>
<br />
<asp:GridView runat="server" Width="98%" ID="grdCDSHistory" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found." style="margin-top:0px">
    <Columns>
        <asp:BoundField DataField="STATE_CDS_Number"        HeaderText="CDS Number"     SortExpression="STATE_CDS_Number" />
        <asp:BoundField DataField="STATE"                   HeaderText="State"          SortExpression="STATE" />
        <asp:BoundField DataField="UserName"                HeaderText="User Name"      SortExpression="UserName" />
        <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" HeaderText="Update Date"    SortExpression="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:d}" />
    </Columns>
    <PagerStyle          CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle         CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle            CssClass="gridViewRow" />
    <FooterStyle         CssClass="gridViewFooter" />
</asp:GridView>