<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="PopupControls_RelatedICNScreen" Codebehind="RelatedICNScreen.ascx.cs" %>
<div class="divGrid" style="padding-top: 10px; padding-bottom: 20px;">
    <asp:GridView ID="gvRelatedICNs" runat="server" Width="70%" AllowSorting="false"
        CssClass="gridview"
        EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" Style="margin-right: 300px">
        <Columns>
            <asp:BoundField DataField="RelatedICN" HeaderText="RelatedICN" />
            <asp:BoundField DataField="Reason" HeaderText="Reason" />
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
</div>
