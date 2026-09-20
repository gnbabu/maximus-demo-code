<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_CARCRARCINformation, App_Web_c4une0e1" %>
<div class="divGrid" style="padding-top: 10px; padding-bottom: 20px;" >
    <div style="overflow-y:scroll;height:200px">
    <asp:GridView ID="gvCARCandRARCDetails" runat="server" Width="100%" AllowSorting="false"
        CssClass="gridview"
        EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" style="padding:10px">
        <Columns>
            <asp:BoundField DataField="Service_Line" HeaderText="Service Line" />
            <asp:BoundField DataField="CARC_NO" HeaderText="CARC" />
            <asp:BoundField DataField="CARC_Amount" HeaderText="CARC Amount" />
            <asp:BoundField DataField="CARC_Description" HeaderText="CARC Description" />
            <asp:BoundField DataField="RARC_No" HeaderText="RARC" />
            <asp:BoundField DataField="RARC_Description" HeaderText="RARC Description" />
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
        </div>
</div>
