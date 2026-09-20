<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_TaxonomiesHistory" Codebehind="TaxonomiesHistory.ascx.cs" %>
<br />
<asp:GridView runat="server" Width="98%" ID="grdTaxonomiesHistory" AutoGenerateColumns="False" HorizontalAlign="Left" AllowPaging="True" AllowSorting="True"       
    CssClass="gridview" EmptyDataText="No entries found." OOnPageIndexChanging="grdTaxonomiesHistory_PageIndexChanging" OnSorting="grdTaxonomiesHistory_Sorting">
    <Columns>
        <asp:BoundField DataField="Operation"           HeaderText="Operation"              SortExpression="Operation" />
        <asp:BoundField DataField="TAXONOMY_CODE"       HeaderText="Taxonomy"               SortExpression="TAXONOMY_CODE" />
        <asp:BoundField DataField="TAXONOMY_NAME"       HeaderText="Taxonomy Description"   SortExpression="TAXONOMY_NAME" />
        <asp:BoundField DataField="PRIMARY_FLAG"   HeaderText="Primary"                SortExpression="PRIMARY_FLAG" />
        <asp:BoundField DataField="START_DATE"          HeaderText="Start"                  SortExpression="START_DATE"     DataFormatString="{0:MM/dd/yyyy}" />
        <asp:BoundField DataField="END_DATE"            HeaderText="End"                    SortExpression="END_DATE"       DataFormatString="{0:MM/dd/yyyy}" />
        <asp:BoundField DataField="UserName"            HeaderText="User Name"              SortExpression="UserName" />
        <asp:BoundField DataField="DateOfAction"        HeaderText="Update Date"            SortExpression="DateOfAction"   DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False"  />
    </Columns>
    <PagerStyle             CssClass="gridpager"        HorizontalAlign="Right" />
    <HeaderStyle            CssClass="gridViewHeader"   Width="100px" />
    <AlternatingRowStyle    CssClass="gridViewAltRow" />
    <RowStyle               CssClass="gridViewRow" />
    <FooterStyle            CssClass="gridViewFooter" />
</asp:GridView>