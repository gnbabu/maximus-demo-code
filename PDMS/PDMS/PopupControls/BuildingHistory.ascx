<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_BuildingHistory" Codebehind="BuildingHistory.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<script type="text/javascript">
     function exportPopup() {
        var rowCount = $("#<%= hdnRowCount.ClientID %>").first().val();
        if (rowCount > 10000) {
            $("#divRowCount").dialog();
        }
    }
</script>

<uc1:Separator ID="Separator2" runat="server" Header="Operators Associated with the Building" />

<div style="width: 100%; text-align: right">
    <asp:HiddenField ID="hdnRowCount" runat="server" />
    <telerik:RadGrid ID="RadGridExport" runat="server" Visible="true">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true">
            <Pdf PageHeight="8.5in" PageWidth="11in" PageTitle="Provider Search">
                <PageFooter>
                    <RightCell Text="Page <?page-number?>" />
                </PageFooter>
            </Pdf>
        </ExportSettings>
        <MasterTableView AutoGenerateColumns="false">
            <Columns>
                <telerik:GridBoundColumn UniqueName="col1" DataField="name" HeaderText="Name"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col2" DataField="MaskTaxID" HeaderText="Tax Id"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col3" HeaderText="NPI" DataField="NPI"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col4"  DataField="START_DATE" HeaderText="Start Date" DataFormatString="{0:d}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col5" DataField="END_DATE" HeaderText="End Date" DataFormatString="{0:d}"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col7"  DataField="Status" HeaderText="Status"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col8" DataField="OperatorMedicaid" HeaderText="Medicaid ID" ></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col9" DataField="ProviderDesc" HeaderText="Provider Type" ></telerik:GridBoundColumn>
               
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>

    

       <asp:LinkButton ID="lnkPDF" runat="server" ToolTip="PDF" OnClick="lnkPDF_Click" OnClientClick="exportPopup();" Visible="true"><img src="../Images/PDF_24x24.png" alt="PDF" /></asp:LinkButton>
    <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10,000 results from the search will be exported.</p>
    </div>
</div>
 
<asp:Panel ID="pnlOperatonAssociatedBuilding" runat="server" Style="display: inline-block; width: 100%; padding-bottom: 20px">
    <%--<uc1:Separator ID="Separator1" runat="server" Header="Operaton Associated with the Building" />--%>
    <div class="container" style="width: 90%">
       <%-- <uc1:Separator ID="Separator12" runat="server" Header="Building History" />--%>
        <div class="row">
            <div class="col-sm-2">
                <asp:Label ID="lblMedicaid" runat="server" CssClass="formLabel" Text="Medicaid ID: "></asp:Label>
            </div>
            <div class="col-sm-2 text-right">
                <asp:Label ID="lblMedText" runat="server" CssClass="formLabelAuto"></asp:Label>
            </div>
            <div class="col-sm-2">
                <asp:Label ID="lblHomeNumber" runat="server" CssClass="formLabel" Text="Home Number: "></asp:Label>
            </div>
            <div class="col-sm-2 text-right">
                <asp:Label ID="lblHomeText" runat="server" CssClass="formLabelAuto"></asp:Label>
            </div>
            <div class="col-sm-2">
                <asp:Label ID="lblIIDFacilityNumber" runat="server" CssClass="formLabel" Text="IID Facility Number: "></asp:Label>
            </div>
            <div class="col-sm-2 text-right">
                <asp:Label ID="lblIIDText" runat="server" CssClass="formLabelAuto"></asp:Label>
            </div>
        </div>

        <asp:GridView ID="BuildingHistoryGrid" runat="server" Style="margin-left: 38px; margin-top: 86px;" Width="98%" AutoGenerateColumns="False" EmptyDataText="No Building History details available."
            CssClass="gridViewSmallFont">
            <Columns>
                <asp:BoundField DataField="name" HeaderText="Name" SortExpression="Name"></asp:BoundField>
                <asp:BoundField DataField="MaskTaxID" HeaderText="Tax Id" SortExpression="MaskTaxID"></asp:BoundField>
                <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI"></asp:BoundField>
                 <asp:BoundField DataField="START_DATE" HeaderText="Start Date" SortExpression="START_DATE" DataFormatString="{0:d}"></asp:BoundField>
                <asp:BoundField DataField="END_DATE" HeaderText="End Date" SortExpression="End Date" DataFormatString="{0:d}"></asp:BoundField>
                 <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status"></asp:BoundField>
                <asp:BoundField DataField="OperatorMedicaid" HeaderText="Medicaid ID" SortExpression="OperatorMedicaid"></asp:BoundField>
                <asp:BoundField DataField="ProviderDesc" HeaderText="Provider Type" SortExpression="ProviderDesc"></asp:BoundField>
                 <asp:TemplateField ItemStyle-Width="2%">
                <ItemTemplate>
                    <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditContractMaintenance" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                        ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                </ItemTemplate>
            </asp:TemplateField>
               
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
        <br />

    </div>
</asp:Panel>
