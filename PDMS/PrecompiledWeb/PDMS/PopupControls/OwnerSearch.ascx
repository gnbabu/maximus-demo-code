<%@ control language="C#" autoeventwireup="true" inherits="UserControls_OwnerSearch, App_Web_2k5drnu4" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/DisenrollmentView.ascx" TagName="DisenrollmentView" TagPrefix="viewDisenroll" %>
<%@ Register Src="~/PopupControls/RetroEffectiveView.ascx" TagName="RetroView" TagPrefix="viewRetro" %>
<%@ Register Src="~/PopupControls/ExpressTerminationView.ascx" TagName="TermView" TagPrefix="viewTerm" %>
<%@ Register Src="~/PopupControls/ReactivationView.ascx" TagName="ReactivationView" TagPrefix="viewReactivate" %>
<%@ Register Src="~/PopupControls/ProviderManagementView.ascx" TagName="ProviderManagementView" TagPrefix="viewProviderManagement" %>
<%@ Register Src="~/PopupControls/ProviderSummaryView.ascx" TagName="ProviderSummaryView" TagPrefix="viewSummary" %>
<%@ Register Src="~/PopupControls/ProviderAddView.ascx" TagName="ProviderAddView" TagPrefix="viewAddProvider" %>
<script type="text/javascript">
    function SelectAll(id) {
        var frm = document.forms[0];
        for (i = 0; i < frm.elements.length; i++) {
            var thisName = frm.elements[i].name;
            if (frm.elements[i].type == "checkbox" && thisName.indexOf("chkSelect") != -1) {
                frm.elements[i].checked = document.getElementById(id).checked;
            }
        }
    }

    function SetPageNumber(id, pageNumber, idClicked) {
        if (document.getElementById(id) != null) {
            document.getElementById(id).value = pageNumber;
        }
        if (document.getElementById(idClicked) != null) {
            document.getElementById(idClicked).value = "TRUE";
        }
    }

    function exportPopup() {
        var rowCount = $("#<%= hdnRowCount.ClientID %>").first().val();
        if (rowCount > 10000) {
            $("#divRowCount").dialog();
        }
    }
    function SelectAllCheckboxes(spanChk) {

        // Added as ASPX uses SPAN for checkbox
        var oItem = spanChk.children;
        var theBox = (spanChk.type == "checkbox") ?
            spanChk : spanChk.children.item[0];
        xState = theBox.checked;
        elm = theBox.form.elements;

        for (i = 0; i < elm.length; i++)
            if (elm[i].type == "checkbox" &&
                elm[i].id != theBox.id) {
                //elm[i].click();
                if (elm[i].checked != xState)
                    elm[i].click();
                //elm[i].checked=xState;
            }
    }
</script>
<style type="text/css">
    select {
        min-width: 90%;
    }
</style>

<cc1:GroupBox ID="gbSearch" CaptionStyle-CssClass="bodyTextBold" HorizontalAlign="Center" Width="100%" runat="server">
   <legend class="bodyTextBold" style="font-size:14pt;font-weight:bold" tabindex="0">Search Criteria</legend>
        <asp:Label runat="server" ID="lblMessages" CssClass="error-message" />
  
    <div>
        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="OwnerSearch" ShowSummary="true" />
    </div>
    <div style="text-align: center;">
        <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">
            <div style="width: 100%;" class="table">
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="txtFirstName" Text="First Name" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-4 text-left" aria-label="firstname" role="textbox">
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="textEntry" MaxLength="30" />
                    </div>

                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblLastName" runat="server" AssociatedControlID="txtLastName" Text="Last Name" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-4 text-left" aria-label="LastName" role="textbox">
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="textEntry" MaxLength="30" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblGroupName" runat="server" AssociatedControlID="txtGroupName" Text="Organization Name" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-4 text-left" aria-label="Organization Name" role="textbox">
                        <asp:TextBox ID="txtGroupName" runat="server" CssClass="textEntry" MaxLength="30" />
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblTaxID" runat="server" AssociatedControlID="txtTaxID" Text="Tax ID" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-4 text-left" aria-label="TaxID" role="textbox">
                        <asp:TextBox ID="txtTaxID" runat="server" MaxLength="9" CssClass="textEntry" />
                        <asp:RegularExpressionValidator ID="valTaxIdFormat" runat="server" ControlToValidate="txtTaxID"
                            ValidationExpression="^\d{9}$" ErrorMessage="* Enter a 9 digit Tax ID."
                            Enabled="true" SetFocusOnError="true" Text="*"
                            ValidationGroup="OwnerSearch" Display="Dynamic" />
                    </div>
                </div>

                <asp:UpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>


                        <div id="divAdditionalSearchCriteria" runat="server" visible="false"></div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="pg-hint3Center pg-hint3"><span class="text-center" tabindex="0">Tax ID is exact match search fields.</span></div>
            <div class="btnBox btnBoxCenter">
                <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="OwnerSearch" />
                <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" />
            </div>
        </asp:Panel>
    </div>
</cc1:GroupBox>
<br />

<asp:Panel ID="pnlResultHeader" runat="server" CssClass="ResultHeader">
    <asp:Label ID="lblResultHeader" runat="server" />
</asp:Panel>

    <div style="width: 100%; text-align: right">
        
    <asp:HiddenField ID="hdnRowCount" runat="server" />
    <telerik:RadGrid ID="RadGridExport" runat="server" Visible="true">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true">
            <Pdf PageHeight="8.5in" PageWidth="11in" PageTitle="Owner Search">
                <PageFooter>
                    <RightCell Text="Page <?page-number?>" />
                </PageFooter>
            </Pdf>
        </ExportSettings>
        <MasterTableView AutoGenerateColumns="false">
            <Columns>
                <telerik:GridBoundColumn UniqueName="col1" HeaderText="Provider Name" DataField="OrganizationName"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col2" HeaderText="Provider Type" DataField="PROVIDER_TYPE_NAME"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col3" HeaderText="Tax ID" DataField="TaxId"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col4" HeaderText="NPI" DataField="NPI"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col5" HeaderText="PNM Status" DataField="PDMSStatus"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col6" HeaderText="Assigned To" DataField="AssignedTo"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col7" HeaderText="Reg ID" DataField="RegID"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col8" HeaderText="Status" DataField="PDMSStatusDate"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col9" HeaderText="Medicaid ID" DataField="BaseMedicaidID"></telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
        <asp:Label ID="lnkRowCount" Visible="false" runat="server" style="float:left" CssClass="formLabelAuto">Row Count: <%= System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(hdnRowCount.Value.ToString(), true) %></asp:Label>
    <asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/Excel_24x24.png" alt="PDF" /></asp:LinkButton>&nbsp;&nbsp;
    <asp:LinkButton ID="lnkPDF" runat="server" ToolTip="PDF" OnClick="lnkPDF_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/PDF_24x24.png" alt="PDF" /></asp:LinkButton>
    
    <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
    </div>
</div>

<div id="LoadingPanel" style="display: none; color: red; text-align: center;">Setting up workflow for provider...</div>
<br />

<mms:SortablePagingGridView
    ID="gvProviders"
    runat="server"
    AutoGenerateColumns="False"
    CssClass="gridViewSmallFont" Width="100%"
    AllowSorting="true"
    EmptyDataText="No Providers found."
    OnRowCommand="gvProviders_RowCommand"
    OnRowDataBound="gvProviders_RowDataBound"
    OnPageIndexChanging="gvProviders_PageIndexChanging"
    OnSorting="gvProviders_Sorting"
    RowStyle-VerticalAlign="Top"
    AllowPaging="True"
    PageSize="15"
    GridViewSortColumn="OrganizationName" GridViewSortDirection="Ascending"
    DataKeyNames="RegID, CurrentStepID,UserID, BaseMedicaidID,PROCESS_ID">
    <Columns>
        <asp:TemplateField Visible="false">
            <ItemTemplate>
                <asp:CheckBox ID="chkAssign" runat="server" />
            </ItemTemplate>
            <HeaderTemplate>
                <input id="chkAll" onclick="javascript: SelectAllCheckboxes(this);" runat="server" type="checkbox" />
            </HeaderTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False" HeaderText="">
            <ItemTemplate>
                <asp:LinkButton
                    ID="lnkReview"
                    runat="server"
                    CausesValidation="false"
                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                    CommandName="ReviewRow"
                    Text="Review"
                    CssClass="gridLink" PostBackUrl="~/Process/Registration.aspx" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="OrganizationName" HeaderText="Provider Name" SortExpression="OrganizationName" HtmlEncode="false" />
        <asp:BoundField DataField="PROVIDER_TYPE_NAME" HeaderText="Provider Type" SortExpression="PROVIDER_TYPE_NAME" />
        <asp:BoundField DataField="TaxId" HeaderText="Tax ID" SortExpression="TaxId" />
        <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
        <asp:BoundField DataField="PDMSStatus" HeaderText="PNM Status" SortExpression="PDMSStatus" />
        <asp:BoundField DataField="AssignedTo" HeaderText="Assigned To" SortExpression="AssignedTo" />
                <asp:BoundField DataField="RegID" HeaderText="Reg ID" SortExpression="RegID" />
        <asp:BoundField DataField="PDMSStatusDate" HeaderText="Status" SortExpression="PDMSStatusDate" />
        <asp:BoundField DataField="BaseMedicaidID" HeaderText="Medicaid ID" SortExpression="BaseMedicaidID" />
    </Columns>
</mms:SortablePagingGridView>
<cc2:MessageBox ID="MessageBox2" runat="server" />