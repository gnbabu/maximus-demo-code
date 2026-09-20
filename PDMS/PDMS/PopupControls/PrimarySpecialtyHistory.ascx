<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_PrimarySpecialtyHistory" Codebehind="PrimarySpecialtyHistory.ascx.cs" %>
<br />
<asp:GridView runat="server" Width="98%" ID="grd" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found." style="margin-top:0px">
    <Columns>
        <asp:BoundField DataField="SPECIALTY_TYPE_NAME" HeaderText="Primary Specialty" SortExpression="SPECIALTY_TYPE_NAME" />
        <asp:BoundField DataField="START_DATE" HeaderText="Start Date" SortExpression="START_DATE" DataFormatString="{0:d}" Visible="false" />
        <asp:BoundField DataField="END_DATE" HeaderText="End Date" SortExpression="END_DATE" DataFormatString="{0:d}" Visible="false" />
        <asp:BoundField DataField="SPECIALTY_BOARD_CERTIFIED" SortExpression="SPECIALTY_BOARD_CERTIFIED" HeaderText="Board Certified" Visible="false" />
        <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
        <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" SortExpression="LAST_MODIFIED_DATE_TIME" HeaderText="Update Date" />
    </Columns>
    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>
