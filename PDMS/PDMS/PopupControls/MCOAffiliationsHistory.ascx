<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_MCOAffiliationsHistory" Codebehind="MCOAffiliationsHistory.ascx.cs" %>
<br />
<asp:UpdatePanel ID="upGrd" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upGrd">
        <ProgressTemplate>
            <h3 style="background-color:Gray">Processing...</h3>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:GridView runat="server" Width="98%" ID="grd" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found.">
        <Columns>
            <asp:BoundField DataField="NAME" HeaderText="Provider Name" SortExpression="NAME" />
            <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
            <asp:BoundField DataField="TAXONOMY_CODE" HeaderText="Taxonomy Code" SortExpression="TAXONOMY_CODE" Visible="false" />
            <asp:BoundField DataField="START_DATE" HeaderText="Start Date" SortExpression="START_DATE" />
            <asp:BoundField DataField="END_DATE" HeaderText="End Date" SortExpression="END_DATE" />
            <asp:BoundField DataField="DESCRIPTION" HeaderText="Affiliation Status" SortExpression="DESCRIPTION" />
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
    <asp:DataList ID="dlPager" CellPadding="5" RepeatDirection="Horizontal" runat="server" OnItemCommand="dlPager_ItemCommand" RepeatColumns="20">
        <ItemStyle Wrap="true" />
        <ItemTemplate>
            <asp:LinkButton Enabled='<%#Eval("Enabled") %>' runat="server" ID="lnkPageNo" Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo"></asp:LinkButton>
        </ItemTemplate>
    </asp:DataList>
    </ContentTemplate>
</asp:UpdatePanel>
