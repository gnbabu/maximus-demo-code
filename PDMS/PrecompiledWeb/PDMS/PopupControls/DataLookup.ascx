<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_DataLookup, App_Web_l5y5araq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<script>
    $(document).ready(function () {
        $('#ctl00_MainContent_ucDataLookup_txtRegId').keyup(function (e) {
            if (/\D/g.test(this.value)) {
                this.value = this.value.replace(/\D/g, '');
            }
        });
        $('#ctl00_MainContent_ucDataLookup_txtMedicaidId').keyup(function (e) {
            if (/\D/g.test(this.value)) {
                this.value = this.value.replace(/\D/g, '');
            }
        });
    });
</script>

<legend class="bodyTextBold" style="font-size:14pt;font-weight:bold; margin-top:20px;" tabindex="0">Search Criteria</legend>
<div class="WhiteBox">
        <div>
            <div class="row" style="margin-top:10px">
                <div class="col-sm-2 text-right">
                    <span class="formLabel150">
                        <asp:Label ID="lblRegId" runat="server" AssociatedControlID="txtRegId" Text="Reg ID" />
                    </span>&nbsp;&nbsp;
                </div>
                <div class="col-sm-3 text-left">
                    <asp:TextBox ID="txtRegId" runat="server" CssClass="textEntry" MaxLength="30" />
                </div>
                <div class="col-sm-2 text-right">
                    <span class="formLabel150">
                        <asp:Label ID="lblMedicaidId" runat="server" AssociatedControlID="txtMedicaidId" Text="Med ID" />
                    </span>&nbsp;&nbsp;
                </div>
                <div class="col-sm-3 text-left">
                    <asp:TextBox ID="txtMedicaidId" runat="server" MaxLength="9" CssClass="textEntry" />
                </div>
            </div>
            <div class="row" style="margin-top:25px" >
                <div class="col-sm-4 text-left">
                </div>
                <div class="col-sm-4 text-left">
                    <asp:DropDownList  runat="server" id="ddlDataLookupTableId">
                        <asp:ListItem Text="MC Facility Number" Value="MC_FACILITY_NUMBER"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="btnBox btnBoxCenter" style="margin-top:25px" >
                <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="TransactionSearch" />
                <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" />
            </div>
        </div>
</div>

<legend class="bodyTextBold" style="font-size:14pt;font-weight:bold" tabindex="0">Search Results</legend>
<div style="margin-top:20px">
    <div style="display:none">
        <telerik:RadGrid ID="RadGridExport" runat="server" Visible="true">
            <ExportSettings IgnorePaging="true" OpenInNewWindow="true">
                <Pdf PageHeight="8.5in" PageWidth="11in" PageTitle="Transaction Search">
                    <PageFooter>
                        <RightCell Text="Page <?page-number?>" />
                    </PageFooter>
                </Pdf>
            </ExportSettings>
            <MasterTableView AutoGenerateColumns="false">
                <Columns>
                    <telerik:GridBoundColumn UniqueName="col1" HeaderText="Reg ID" DataField="REG_ID"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="col2" HeaderText="Medicaid ID" DataField="MED_ID" DataFormatString="&nbsp;{0}"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="col3" HeaderText="Data Lookup" DataField="DATA_LOOKUP"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="col4" HeaderText="MCPN Facility Number" DataField="MCPN_FACILITY_NUMBER"></telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>
    <asp:LinkButton ID="lnkExcel" runat="server" style="float:right" ToolTip="Excel" OnClick="lnkExcel_Click" Visible="false"><img src="../Images/Excel_24x24.png" alt="PDF" /></asp:LinkButton>
    &nbsp;&nbsp;
    
    <mms:SortablePagingGridView
        ID="gvDataLookup"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="gridViewSmallFont" Width="100%"
        AllowSorting="false"
        EmptyDataText="No records found."
        OnRowCommand="gvDataLookup_RowCommand" OnRowDataBound="gvDataLookup_RowDataBound"
        RowStyle-VerticalAlign="Top"
        AllowPaging="True"
        PageSize="15"
        GridViewSortColumn="REG_ID" GridViewSortDirection="Ascending"
        DataKeyNames="REG_ID,MED_ID,FACILITY_NO_TYPE,DATA_LOOKUP,MCPN_FACILITY_NUMBER">
        <Columns>
            <asp:TemplateField ShowHeader="False" HeaderText="Reg ID">
                <ItemTemplate>
                    <asp:LinkButton
                        ID="lnkReview"
                        runat="server"
                        CausesValidation="false"
                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                        CommandName="ProviderRecord"
                        Text='<%# Eval("REG_ID") %>'
                        CssClass="gridLink" PostBackUrl="~/Process/Registration.aspx" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="MED_ID" HeaderText="Med ID"  />
            <asp:BoundField DataField="FACILITY_NO_TYPE" HeaderText="" visible="false" />
            <asp:BoundField DataField="DATA_LOOKUP" HeaderText="Data Lookup" />
            <asp:BoundField DataField="MCPN_Facility_Number" HeaderText="MCPN Facility Number" HeaderStyle-Width="520" HeaderStyle-Wrap="true" />
            <asp:TemplateField  HeaderText="Add New">
                <ItemTemplate>
                    <asp:LinkButton
                        ID="btnAddNew"
                        runat="server"
                        CommandName="AddNewFacilityNumber"
                        Text="Add New"
                        CssClass="gridLink" />
                </ItemTemplate>
            </asp:TemplateField>
       </Columns> 
    </mms:SortablePagingGridView>
</div>