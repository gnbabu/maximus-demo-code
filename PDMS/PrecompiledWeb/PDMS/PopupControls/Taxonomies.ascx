<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_Taxonomies, App_Web_yvhxe4ml" enableviewstate="true" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/TaxonomiesHistory.ascx" TagPrefix="uc" TagName="TaxonomiesHistory" %>
<script type="text/javascript">
    function removeDisabled() {
        $("#<%= btnCloseHistory.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    }
</script>
<div onmouseover="removeDisabled();">
<div>
    <asp:ValidationSummary ID="OverallErrorReportingForThisPage" runat="server" DisplayMode="List" ValidationGroup="valTaxonomiesOverall" CssClass="failureNotification"/>
</div>
<asp:Panel ID="pnlTaxonomies" runat="server" Style="display: inline-block; width: 100%;">
    <div class="divGrid">
        <asp:GridView runat="server" Width="98%" ID="grdTaxonomies"
            AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No records found" OnRowCommand="grd_RowCommand">
            <Columns>
                <asp:BoundField DataField="TAXONOMY_CODE" HeaderText="Taxonomy" />
                <asp:BoundField DataField="TAXONOMY_NAME" HeaderText="Taxonomy Description" />
                <asp:TemplateField HeaderText="Primary">
                    <ItemTemplate>
                        <asp:Label ID="lblIsPrimary" runat="server" Text='<%# (Convert.ToBoolean(Eval("PRIMARY_FLAG")) == true) ? "Yes" : "No" %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="START_DATE" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="END_DATE" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:TemplateField ItemStyle-Width="2%" HeaderText="<span style='display:none'>Edit</span>">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditTaxonomiesRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<span style='display:none'>Delete</span>">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteTaxonomiesRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" AlternateText="Delete"
                            Visible="<%# CanUserViewDelete()  %>" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>

    <div class="divHistoryAndAdd">
        <asp:ImageButton ID="btnAddTaxonomies" AlternateText="Add New" runat="server" ImageUrl="~/Images/add.png" CommandName="Taxonomies" OnCommand="lbtnAdd_Click" ToolTip="Add" /><br />
        <asp:LinkButton TabIndex="0" ID="btnHistory" CommandName="History" runat="server" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" Text="History" ToolTip="History" Style="color: white; text-decoration: none;">
            History</asp:LinkButton>
        <div id="Div2" class="help-kfe" style="cursor: pointer; display: inline-block;" runat="server">
            <div class="help-parent-name" style="display: none;">Primary Taxonomy</div>
            
        </div>
    </div>
    <br />
</asp:Panel>
<div id="taxonomyDetail" runat="server" visible="false">
    <div>
        <asp:ValidationSummary ID="vsTaxonomies" runat="server" DisplayMode="List" ValidationGroup="valTaxonomies" CssClass="failureNotification"/>
    </div>
    <asp:Label ID="lblDuplicate" runat="server" Text="The taxonomy has already been added to this registration. Please select a different one" ForeColor="Red" Visible="false" />
    <asp:UpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="ParentTable" runat="server">
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Taxonomy*</span></div>
                    <div class="col-sm-9 text-left">
                        <asp:DropDownList ID="ddlTaxonomy" aria-label="Taxonomy" runat="server" EnableViewState="true"></asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="reqCategory" ValidationGroup="valTaxonomies"
                            ControlToValidate="ddlTaxonomy" ErrorMessage="*Select a Taxonomy" Text="*" Display="Dynamic"
                            SetFocusOnError="true" InitialValue="" />
                    </div>
                    <div style="display: none;">
                        <asp:Label ID="lblPDMSTaxonomy" runat="server" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel"></span></div>
                    <div class="col-sm-9 text-left">
                        <asp:CheckBox ID="chkIsPrimary" runat="server" Enabled="true" Checked="false" Text=" Is Primary Taxonomy" />
                    </div>
                    <div style="display: none;">
                        <asp:Label ID="lblPDSMTaxonomyIsPrimary" runat="server" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">Start Date*</span></div>
                    <div class="col-sm-9 text-left">
                        <asp:TextBox ID="txtTaxonomyStart" runat="server" aria-label="Taxonomy start" CssClass="formDropDownLarge" />
                        <ajax:CalendarExtender ID="calStart" TargetControlID="txtTaxonomyStart" runat="server" />
                        <asp:RequiredFieldValidator runat="server" ID="reqStartDate" ValidationGroup="valTaxonomies"
                            ControlToValidate="txtTaxonomyStart" ErrorMessage="*Enter a Start Date" Text="*" Display="Dynamic"
                            SetFocusOnError="true" InitialValue="" />
                    </div>
                    <div style="display: none;">
                        <asp:Label ID="Label1" runat="server" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel">End Date</span></div>
                    <div class="col-sm-9 text-left">
                        <asp:TextBox ID="txtTaxonomyEnd" aria-label="taxnomy end" runat="server" CssClass="formDropDownLarge" />
                        <ajax:CalendarExtender ID="calEnd" TargetControlID="txtTaxonomyEnd" runat="server" />
                        <%--  <asp:RequiredFieldValidator ID="reqEndDate" runat="server" ValidationGroup="valTaxonomies"
                            ControlToValidate="txtTaxonomyEnd" ErrorMessage="*Enter an End Date" Text="*" Display="Dynamic" 
                            SetFocusOnError="true" InitialValue="" /> --%>
                    </div>
                    <div style="display: none;">
                        <asp:Label ID="Label2" runat="server" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right">&nbsp;</div>
                    <div class="col-sm-9 text-left">
                        <asp:UpdateProgress ID="updateProgress" runat="server" AssociatedUpdatePanelID="pnlUpdate">
                            <ProgressTemplate>
                                <div style="padding-right: 30px">
                                    <img alt="Loading" src="../Images/ajax-loader.gif" />
                                    Loading ...
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                    <div style="display: none;">&nbsp;</div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:HiddenField ID="hdnRegTaxonomyID" runat="server" />
</div>

<style type="text/css">
    .modalPopup
    {
        height: auto;
        left: 20% !important;
        top: 30% !important;
        position: fixed !important;
    }
</style>
<asp:Panel ID="upTaxonomyHistory" runat="server">
    <ajax:ModalPopupExtender ID="mpe" runat="server" BackgroundCssClass="modalBackground" CancelControlID="btnCloseHistory" PopupControlID="pnlModal" PopupDragHandleControlID="pnlModal" TargetControlID="ButtonDummy3" />
    <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 1000px;">
        <asp:Panel ID="pnlHeader" runat="server" CssClass="pnlHeader" HorizontalAlign="Left">
            <div align="left">
                &nbsp;&nbsp;<asp:Label ID="lblTitle" runat="server" CssClass="bodyTextBold" ForeColor="White" Text="Title" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px">
            <div class="container-fluid" style="text-align: left; padding: 15px;">
                <div class="row">
                    <uc:TaxonomiesHistory ID="ucTaxonomiesHistory" runat="server" />
                </div>
                <div class="row">
                    <div class="btnBox" style="text-align: right;">
                        <asp:Button ID="btnCloseHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="OK" />
                        <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkExcel_Click" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Button runat="server" aria-Label="Dummy Button" ID="ButtonDummy3" Style="display: none" Text=”ButtonDummy3” />
</asp:Panel>
<div style="width: 100%; text-align: right; display: none;">
    <asp:HiddenField ID="hdnRowCount" runat="server" />
    <asp:Button ID="lnkExcel" CommandName="Export" OnClick="lnkExcel_Click" runat="server" ToolTip="Excel" Visible="true" Text="Export"/>
    <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
    </div>
    <telerik:RadGrid ID="grd" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
        AutoGenerateColumns="true" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
            <Excel Format="Biff" />
        </ExportSettings>
        <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
            DataKeyNames="INDEX, DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
            <Columns>
                <telerik:GridBoundColumn DataField="OPERATION"           HeaderText="Operation"      SortExpression="Operation" />
                <telerik:GridBoundColumn DataField="Operation"           HeaderText="Operation"              SortExpression="Operation" />
                <telerik:GridBoundColumn DataField="TAXONOMY_CODE"       HeaderText="Taxonomy"               SortExpression="TAXONOMY_CODE" />
                <telerik:GridBoundColumn DataField="TAXONOMY_NAME"       HeaderText="Taxonomy Description"   SortExpression="TAXONOMY_NAME" />
                <telerik:GridBoundColumn DataField="PRIMARY_FLAG"   HeaderText="Primary"                SortExpression="PRIMARY_FLAG" />
                <telerik:GridBoundColumn DataField="START_DATE"          HeaderText="Start"                  SortExpression="START_DATE"     DataFormatString="{0:MM/dd/yyyy}" />
                <telerik:GridBoundColumn DataField="END_DATE"            HeaderText="End"                    SortExpression="END_DATE"       DataFormatString="{0:MM/dd/yyyy}" />
                <telerik:GridBoundColumn DataField="UserName"            HeaderText="User Name"              SortExpression="UserName" />
                <telerik:GridBoundColumn DataField="DateOfAction"        HeaderText="Update Date"            SortExpression="DateOfAction"   DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False"  />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>
</div>