<%@ Control Language="C#" AutoEventWireup="true" Inherits="Pages_EventHistory" Codebehind="EventHistory.ascx.cs" %>
<asp:GridView runat="server" Width="100%" ID="grdHistory"  AutoGenerateColumns="False" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No event history found.">
    <Columns>
        <asp:BoundField DataField="Date" HeaderText="Date" />
        <asp:BoundField DataField="Status" HeaderText="Status" />
        <asp:BoundField DataField="Action" HeaderText="Action" />
        <asp:BoundField DataField="Reason" HeaderText="Reason" />
        <asp:BoundField DataField="From" HeaderText="From" />
        <asp:BoundField DataField="To" HeaderText="To" />
        <asp:BoundField DataField="Subject" HeaderText="Subject" />
    </Columns>
    <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />  
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" /> 
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>