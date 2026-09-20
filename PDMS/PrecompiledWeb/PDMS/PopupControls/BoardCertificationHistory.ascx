<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_BoardCertificationHistory, App_Web_c4une0e1" %>
<br />
<asp:GridView runat="server" Width="98%" ID="grdBoardCertificationHistory" AutoGenerateColumns="False" HorizontalAlign="Left"  CssClass="gridview" EmptyDataText="No entries found.">
    <Columns>
        <asp:BoundField DataField="BOARD_CERTIFICATION_NAME" HeaderText="Board Certification name" SortExpression="BOARD_CERTIFICATION_NAME" />
        <asp:BoundField DataField="BOARD_SPECIALTY_NAME"    HeaderText="Board Specialty"        SortExpression="BOARD_SPECIALTY_NAME" />
        <asp:BoundField DataField="CERTIFICATION_NUMBER"    HeaderText="Certification Number"   SortExpression="CERTIFICATION_NUMBER" />
        <asp:BoundField DataField="EffectiveDate"           HeaderText="Effective Date"         SortExpression="EffectiveDate" />
        <asp:BoundField DataField="ExpirationDate"          HeaderText="Expiration Date"        SortExpression="ExpirationDate" />
        <asp:BoundField DataField="UserName"                HeaderText="User Name"              SortExpression="UserName" />
        <asp:BoundField DataField="UpdateDate"              HeaderText="Update Date"            SortExpression="UpdateDate" />
    </Columns>
    <PagerStyle             CssClass="gridpager"        HorizontalAlign="Right" />
    <HeaderStyle            CssClass="gridViewHeader"   Width="100px" />
    <AlternatingRowStyle    CssClass="gridViewAltRow" />
    <RowStyle               CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>