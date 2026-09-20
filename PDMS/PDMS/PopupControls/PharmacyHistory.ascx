<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_PharmacyHistory" Codebehind="PharmacyHistory.ascx.cs" %>
<br />
<asp:GridView runat="server" Width="98%" ID="grd" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found.">
    <Columns>
        <asp:BoundField DataField="PHARMACY_NAME" HeaderText="Pharmacy Name" />
        <asp:BoundField DataField="CHIEF_PHARMACIST_NAME" HeaderText="Chief Pharmacist" />
        <asp:BoundField DataField="OCCUPANCY_PERMIT_NUMBER" HeaderText="Occupancy Permit Number" DataFormatString="{0:d}" />
        <asp:BoundField DataField="UserName" HeaderText="User Name" />
        <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" HeaderText="Update Date" />
    </Columns>
    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>
