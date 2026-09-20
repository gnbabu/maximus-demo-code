<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="PopupControls_ReviewerNotes" Codebehind="ReviewerNotes.ascx.cs" %>
<div class="divGrid" style="padding-top: 10px; padding-bottom: 20px;height:auto">
    <asp:GridView ID="gvRevNotesProvider" runat="server" Width="70%" AllowSorting="false"
        CssClass="gridview"
        EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" Style="margin-right: 300px">
        <Columns>
            <asp:BoundField DataField="num_dtl" HeaderText="Line" />
            <asp:BoundField DataField="dsc_note" HeaderText="Note" />
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
</div>
