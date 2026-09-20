<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_MedicaidHistory" Codebehind="MedicaidHistory.ascx.cs" %>
<br />
<asp:GridView runat="server" Width="98%" ID="grdMedicaid" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found.">
    <Columns>
        <asp:BoundField DataField="MEDICAID_NUMBER" HeaderText="Other State Medicaid ID" SortExpression="MEDICAID_NUMBER" />
        <asp:BoundField DataField="MEDICAID_STATE" HeaderText="State" SortExpression="MEDICAID_STATE" />
        <asp:BoundField DataField="MEDICAID_EFF_DATE" HeaderText="Issue Date" SortExpression="MEDICAID_EFF_DATE" DataFormatString="{0:d}" />
        <asp:BoundField DataField="MEDICAID_END_DATE" HeaderText="End Date" SortExpression="MEDICAID_END_DATE" DataFormatString="{0:d}" />
        <asp:BoundField DataField="UserName" HeaderText="User Name" />
        <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" HeaderText="Update Date" />
    </Columns>
    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>