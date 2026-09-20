<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_DataLookup" Codebehind="DataLookup.ascx.cs" %>
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

<legend class="bodyTextBold" style="font-size: 14pt; font-weight: bold; margin-top: 20px;" tabindex="0">Search Criteria</legend>
<div class="WhiteBox">
    <div>
        <div class="row" style="margin-top: 10px">
            <div class="col-sm-2 text-right">
                <span class="formLabel150">
                    <asp:Label ID="lblRegId" runat="server" AssociatedControlID="txtRegId" Text="Reg ID" />
                </span>&nbsp;&nbsp;
            </div>
            <div class="col-sm-4 text-left">
                <asp:TextBox ID="txtRegId" runat="server" CssClass="textEntry" MaxLength="30" />
            </div>
            <div class="col-sm-2 text-right">
                <span class="formLabel150">
                    <asp:Label ID="lblMedicaidId" runat="server" AssociatedControlID="txtMedicaidId" Text="Med ID" />
                </span>&nbsp;&nbsp;
            </div>
            <div class="col-sm-4 text-left">
                <asp:TextBox ID="txtMedicaidId" runat="server" MaxLength="9" CssClass="textEntry" />
            </div>
        </div>
        <div class="row" style="margin-top: 25px">
            <div class="col-sm-2 text-right">
                <span class="formLabel150"></span>
            </div>
            <div class="col-sm-4 text-left">
                <asp:DropDownList runat="server" ID="ddlDataLookupTableId" CssClass="DropDownList">
                    <asp:ListItem Text="MC Facility Number" Value="MC_FACILITY_NUMBER"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="btnBoxCenter" style="margin-top: 25px">
            <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="TransactionSearch" />
            <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" />
        </div>
    </div>
</div>

<legend class="bodyTextBold" style="font-size: 14pt; font-weight: bold" tabindex="0">Search Results</legend>
<div style="margin-top: 20px">
    <div style="display: none">
        <telerik:radgrid id="RadGridExport" runat="server" visible="true">
            <exportsettings ignorepaging="true" openinnewwindow="true">
                <pdf pageheight="8.5in" pagewidth="11in" pagetitle="Operations & Transaction Hub">
                    <pagefooter>
                        <rightcell text="Page <?page-number?>" />
                    </pagefooter>
                </pdf>
            </exportsettings>
            <mastertableview autogeneratecolumns="false">
                <columns>
                    <telerik:gridboundcolumn uniquename="col1" headertext="Reg ID" datafield="REG_ID"></telerik:gridboundcolumn>
                    <telerik:gridboundcolumn uniquename="col2" headertext="Medicaid ID" datafield="MED_ID" dataformatstring="&nbsp;{0}"></telerik:gridboundcolumn>
                    <telerik:gridboundcolumn uniquename="col3" headertext="Data Lookup" datafield="DATA_LOOKUP"></telerik:gridboundcolumn>
                    <telerik:gridboundcolumn uniquename="col4" headertext="MCPN Facility Number" datafield="MCPN_FACILITY_NUMBER"></telerik:gridboundcolumn>
                </columns>
            </mastertableview>
        </telerik:radgrid>
    </div>
    <asp:LinkButton ID="lnkExcel" runat="server" Style="float: right" ToolTip="Excel" OnClick="lnkExcel_Click" Visible="false">
        <img src="../Images/Excel_24x24.png" alt="PDF" /></asp:LinkButton>
    &nbsp;&nbsp;
    
    <mms:sortablepaginggridview
        id="gvDataLookup"
        runat="server"
        autogeneratecolumns="False"
        cssclass="gridViewSmallFont" width="100%"
        allowsorting="false"
        emptydatatext="No records found."
        onrowcommand="gvDataLookup_RowCommand" onrowdatabound="gvDataLookup_RowDataBound"
        rowstyle-verticalalign="Top"
        allowpaging="True"
        pagesize="15"
        gridviewsortcolumn="REG_ID" gridviewsortdirection="Ascending"
        datakeynames="REG_ID,MED_ID,FACILITY_NO_TYPE,DATA_LOOKUP,MCPN_FACILITY_NUMBER">
        <columns>
            <asp:templatefield showheader="False" headertext="Reg ID">
                <itemtemplate>
                    <asp:linkbutton
                        id="lnkReview"
                        runat="server"
                        causesvalidation="false"
                        commandargument='<%# ((GridViewRow)Container).RowIndex %>'
                        commandname="ProviderRecord"
                        text='<%# Eval("REG_ID") %>'
                        cssclass="gridLink" postbackurl="~/Process/Registration.aspx" />
                </itemtemplate>
            </asp:templatefield>
            <asp:boundfield datafield="MED_ID" headertext="Med ID" />
            <asp:boundfield datafield="FACILITY_NO_TYPE" headertext="" visible="false" />
            <asp:boundfield datafield="DATA_LOOKUP" headertext="Data Lookup" />
            <asp:boundfield datafield="MCPN_Facility_Number" headertext="MCPN Facility Number" headerstyle-width="520" headerstyle-wrap="true" />
            <asp:templatefield headertext="Add New">
                <itemtemplate>
                    <asp:linkbutton
                        id="btnAddNew"
                        runat="server"
                        commandname="AddNewFacilityNumber"
                        text="Add New"
                        cssclass="gridLink" />
                </itemtemplate>
            </asp:templatefield>
        </columns>
    </mms:sortablepaginggridview>
</div>
