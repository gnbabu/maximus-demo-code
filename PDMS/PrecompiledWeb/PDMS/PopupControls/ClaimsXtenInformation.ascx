<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ClaimsXtenInformation, App_Web_glma3lal" %>
<div class="divGrid" style="padding-top: 10px; padding-bottom: 20px;">
    <asp:GridView ID="gvClaimXtenInfo" runat="server" Width="100%" AllowSorting="false"
        CssClass="gridview"
        EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" Style="margin-right: 300px">
        <Columns>
            <asp:BoundField DataField="RelatedHistroyNumber" HeaderText="Related Histroy Number" />
            <asp:BoundField DataField="ClaimsXtenAuditResult" HeaderText="ClaimsXten Audit Result" />
            <asp:BoundField DataField="DatePosted" HeaderText="Date Posted" />
            <asp:BoundField DataField="RelatedICN" HeaderText="Related ICN" />
            <asp:BoundField DataField="ErrorCode" HeaderText="Error Code" />
            <asp:BoundField DataField="ErrorCodeDescription" HeaderText="Error Code Description" />
            <asp:BoundField DataField="ClaimsXtenAuditMessage1" HeaderText="ClaimsXten Audit Message1" />
            <asp:BoundField DataField="ClaimsXtenAuditMessage2" HeaderText="ClaimsXten Audit Message2" />
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
</div>
