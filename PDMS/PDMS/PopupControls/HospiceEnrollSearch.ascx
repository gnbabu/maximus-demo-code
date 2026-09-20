<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_HospiceEnrollSearch" Codebehind="HospiceEnrollSearch.ascx.cs" %>

<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/HospiceEnrollment.ascx" TagPrefix="uc1" TagName="HospiceEnrollment" %>
<script type="text/javascript">
    $(function () {
        $("[id*=btnSearch]").click(function () {
            var txtMBillingNumber = $("[id*=txtMBillingNumber]").val();
            $("[id*=spanChopMBillingNumber]").hide();
            $("[id*=spanMBillingNumberLength]").hide();
            if ($('#<%= chlChangeofProvider.ClientID %>').is(':checked')) {
                if (txtMBillingNumber === "") {
                    $("[id*=spanChopMBillingNumber]").show();
                    return false;
                }
            }
            if (txtMBillingNumber != null && txtMBillingNumber !== "") {
                if (txtMBillingNumber.length < 12) {
                    $("[id*=spanMBillingNumberLength]").show();
                    return false;
                }
            }
            return true;
        });
        $("[id*=btnAdd]").click(function () {
            $("[id*=spanMBillingNumber]").hide();
            var txtMBillingNumber = $("[id*=txtMBillingNumber]").val();
            if (txtMBillingNumber == null || txtMBillingNumber === "") {
                $("[id*=spanMBillingNumber]").show();
                return false;
            }
            else {
                if (txtMBillingNumber.length < 12) {
                    $("[id*=spanMBillingNumberLength]").show();
                    return false;
                }
            }
            return true;
        });
        $("[id*=btnSave]").click(function () {
            $("[id*=spanMBillingNumber]").hide();
            $("[id*=spanDateofBirth]").hide();
            var txtDateofBirth = $("[id*=txtDateofBirth]").val();
            $("[id*=hdDateOfBirth]").val(txtDateofBirth);
           
            if (txtDateofBirth === "") {
                $("[id*=spanDateofBirth]").show();
                return false;
             }
              var txtMBillingNumber = $("[id*=txtMBillingNumber]").val();
            if (txtMBillingNumber === "") {
                $("[id*=spanMBillingNumber]").show();
                return false;
            }
            else {
                if (txtMBillingNumber.length < 12) {
                    $("[id*=spanMBillingNumberLength]").show();
                    return false;
                }
            }
            return true;
        });
        $("[id*=txtMBillingNumber]").keypress(function () {
            var billingNumber = $("[id*=txtMBillingNumber]").val();
            if (billingNumber !== "") {
                if (billingNumber.length < 12) {
                    $("[id*=spanMBillingNumberLength]").show();
                    $('#ctl00_MainContent_HospiceEnrollSearch_btnAdd').attr('disabled', 'disabled');
                }
                else if (billingNumber.length === 12) {
                    $("[id*=spanMBillingNumberLength]").hide();
                    $('#ctl00_MainContent_HospiceEnrollSearch_btnAdd').removeAttr('disabled');
                }
            }
            else
            {
                $('#ctl00_MainContent_HospiceEnrollSearch_btnAdd').attr('disabled', 'disabled');
            }
        });
        $("[id*=txtMBillingNumber]").keydown(function () {
            var billingNumber = $("[id*=txtMBillingNumber]").val();
            if (billingNumber !== "") {
                if (billingNumber.length < 12) {
                    $("[id*=spanMBillingNumberLength]").show();
                    $('#ctl00_MainContent_HospiceEnrollSearch_btnAdd').attr('disabled', 'disabled');
                }
                else if (billingNumber.length === 12) {
                    $("[id*=spanMBillingNumberLength]").hide();
                    $('#ctl00_MainContent_HospiceEnrollSearch_btnAdd').removeAttr('disabled');
                }
            }
            else {
                $('#ctl00_MainContent_HospiceEnrollSearch_btnAdd').attr('disabled', 'disabled');
            }
        });
        $("[id*=txtMBillingNumber]").blur(function () {
            var billingNumber = $("[id*=txtMBillingNumber]").val();
            if (billingNumber !== "") {
                if (billingNumber.length < 12) {
                    $("[id*=spanMBillingNumberLength]").show();
                    $('#ctl00_MainContent_HospiceEnrollSearch_btnAdd').attr('disabled', 'disabled');
                }
                else if (billingNumber.length === 12) {
                    $("[id*=spanMBillingNumberLength]").hide();
                    $('#ctl00_MainContent_HospiceEnrollSearch_btnAdd').removeAttr('disabled');
                }
            }
            else {
                $('#ctl00_MainContent_HospiceEnrollSearch_btnAdd').attr('disabled', 'disabled');
            }
        });
    });
    function setDOBBlankValue() {
        $('#<%=txtDateofBirth.ClientID%>').val("");
     }
</script>
<asp:HiddenField ID="hdMedicaidBillingNumber" runat="server" />
<asp:HiddenField ID="hdHospiceTrackNo" runat="server" />
<asp:HiddenField ID="hdDateOfBirth" runat="server" />
<asp:Panel ID="pnlHospiceSearch" runat="server">
    <cc1:GroupBox ID="gbSearch"  HorizontalAlign="Center" Width="100%" runat="server">
        <h1 style="font-weight:bold;">Hospice Enrollment Search</h1>
        <asp:Label runat="server" ID="lblMessages" CssClass="error-message" />
        <%--<div>
            <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="ProviderSearch" ShowSummary="true" />
        </div>--%>
        <div style="text-align: center;">
            <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">
                <div class="container-fluid">
                    <asp:UpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-sm-12 col-md-4 col-lg-3 outerName">
                                    <asp:Label ID="lblHTrackingNumber" CssClass="ohio-field" AssociatedControlID="txtHTrackingNumber" runat="server"><span class="ohio-field-label">Hospice Tracking Number <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='HOSPICE_SEARCH_POPUP_1' runat='server' Text='<%$ Resources:BrandingResource , HOSPICE_SEARCH_POPUP_TRACKINGNUMBER %>' />" aria-hidden="true"></span></span>
                                        <asp:TextBox ID="txtHTrackingNumber" CssClass="ohio-field-input" runat="server" MaxLength="9" />
                                       <%-- <span id="spanHTrackingNumber" style="color: red; display: none">
                                            <br />
                                            * Hospice Tracking Number is required.</span>--%>
                                    </asp:Label></div><div class="col-sm-6 col-md-4 col-lg-3 outerName">
                                    <asp:Label ID="lblMBillingNumber" CssClass="ohio-field" AssociatedControlID="txtMBillingNumber" runat="server"><span class="ohio-field-label">Medicaid Billing Number <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='HOSPICE_SEARCH_POPUP_2' runat='server' Text='<%$ Resources:BrandingResource , HOSPICE_SEARCH_POPUP_BILLINGNUMBER %>' />" aria-hidden="true"></span></span>
                                        <asp:TextBox ID="txtMBillingNumber" CssClass="ohio-field-input" aria-label="Medicaid Billing Number 12-digit number is required" runat="server" MaxLength="12" />
                                        <span id="spanMBillingNumber" style="color: #A30000; display: none">
                                            <br />
                                            Medicaid Billing Number is required.</span>
                                        <span id="spanMBillingNumberLength" style="color: #A30000; display: none">
                                           
                                            Medicaid Billing Number : 12-digit number is required.</span>
                                         <span id="spanChopMBillingNumber" style="color : #A30000 ; display: none">
                                            <br />
                                             Medicaid Billing Number is required when this is a change of Hospice provider</span>
                                    </asp:Label></div><br /><asp:CheckBox ID="chlChangeofProvider" Enabled="True" runat="server" Text="This is a change of hospice provider." CssClass="ChkBoxClass" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="col-sm-3" style="padding-top:8px;">
                        <span class="ohio-field-label" style="font-size: 15px; text-align: right">Max Records</span>
                    </div>
                    <div class="col-sm-3" style="padding-top:8px;">
                        <span style="text-align: left; width:350px">
                            <asp:DropDownList ID="ddlPageSize" runat="server" aria-label="Max Records" AutoPostBack="true" OnSelectedIndexChanged="PageSize_Changed" Style="min-width: 80px; height: 30px">
                                <asp:ListItem Text="5" Value="5" />
                                <asp:ListItem Text="10" Value="10" />
                                <asp:ListItem Text="20" Value="20" />
                                <asp:ListItem Text="30" Value="30" />
                                <asp:ListItem Text="40" Value="40" />
                                <asp:ListItem Text="50" Value="50" />
                            </asp:DropDownList>
                        </span>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">

                        <ContentTemplate>
                            <br /><br />
                            <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocusBlue" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBoxFocusred" OnClick="btnClear_Click" />
                            <asp:Button ID="btnAdd" runat="server" CausesValidation="false" Text="Add" Enabled="false" CssClass="buttonBox" style="background-color:#28a0cb !important;" />
                            <ajax:ModalPopupExtender ID="DialpnlDateofBirth" runat="server" BehaviorID="modelDataOfBirth" 
                                        PopupControlID="pnlDataOfBirth" TargetControlID="btnAdd"
                                        BackgroundCssClass="modalBackground" CancelControlID="btnCloseDB"  />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSearch" />
                            <asp:PostBackTrigger ControlID="btnClear" />
                            <asp:PostBackTrigger ControlID="btnAdd" />
                        </Triggers>
                    </asp:UpdatePanel>

                    <asp:Panel ID="pnlDataOfBirth" runat="server" CssClass="modalPopup" Style="display: none; min-height: 350px; min-width: 610px; height: auto; width: auto;">
        <asp:Panel ID="pnlCE1" runat="server">
            <asp:Button runat="server" ID="btnCloseDB" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;" CausesValidation="false" OnClientClick="javascript: setDOBBlankValue();" />
            <div style="text-align: left; padding: 42px;" class="container-fluid">
                <div class="row" style="text-align: center;">
                    <div class="col-sm-6 col-md-6 col-lg-6 ">
                    <span class="ohio-field-label"><strong>Please enter DOB of the Recipient</strong> <br>
                        <asp:TextBox ID="txtDateofBirth"  CssClass="ohio-field-input" aria-label="Date of Birth" runat="server" autocomplete="off"></asp:TextBox><ajax:CalendarExtender ID="clDateofBirth" TargetControlID="txtDateofBirth" runat="server" EnabledOnClient="true" />
                        <span id="spanDateofBirth" style="color:#A30000; display:none"><br />Date of Birth is required</span> <asp:CompareValidator ID="cvttxtDateofBirth" runat="server" Operator="LessThanEqual"
                        ControlToValidate="txtDateofBirth" ValidationGroup="validateHospice" CssClass="failureNotification" ForeColor="Red" ErrorMessage="Future date not allowed"
                        Display="Dynamic" SetFocusOnError="true" Type="Date" />
                    </span>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnSave" runat="server" OnClick="btnAdd_Click" CausesValidation="true" Text="Save" CssClass="buttonBoxFocus"  Style="background-color: darkslateblue !important" />
                    </div>
                </div>
                <div style="text-align: center">
                    <span id="TerminalSearchErrorMessage" style="color: #A30000;"></span>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>

                </div>
                <label id="lblMessage" aria-label="errormessage" runat="server" style="color: #A30000;"></label>
            </asp:Panel>
            <asp:UpdateProgress ID="UpdateProgress" runat="server" DisplayAfter="1">
                <ProgressTemplate>
                    <div style="padding-right: 30px">
                        <img src="../Images/ajax-loader.gif" alt="" />Loading ... </div></ProgressTemplate></asp:UpdateProgress></div></cc1:GroupBox><br /><div style="width: 100%; text-align: right">
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
                    <telerik:GridBoundColumn UniqueName="col1" HeaderText="Hospice Tracking Number" DataField="HospiceTrackNo"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="col2" HeaderText="Medicaid Billing Number" DataField="RecipID"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="col3" HeaderText="Name" DataField="RecipName"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="col4" HeaderText="Date Recieved" DataField="SubmissionDate" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="col5" HeaderText="Status" DataField="Status"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn UniqueName="col6" HeaderText="Denial Reason" DataField="DenialReason"></telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
       <asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/Excel_24x24.png" alt="PDF" /></asp:LinkButton>&nbsp;&nbsp; 
       <asp:LinkButton ID="lnkPDF" runat="server" ToolTip="PDF" OnClick="lnkPDF_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/PDF_24x24.png" alt="PDF" /></asp:LinkButton>
       <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
            <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10,000 results from the search will be exported.will be exported.> be exported.>ted.will be exported.> be exported.></p></div>
        </div>
        <div id="LoadingPanel" style="display: none; color: #AD0000; text-align: center;">Setting up workflow for provider...</div><br />
        <asp:GridView
        ID="gvHospice"
        runat="server"
        Caption='<table border="1" width="100%" cellpadding="0" cellspacing="0" style="background-color: #7d9fc2;"><tr><td style="font-weight:800;color: #FFFFFF;">SEARCH RESULTS</td></tr></table>' HorizontalAlign="Center"
        AutoGenerateColumns="False"
        CssClass="gridViewSmallFont" Width="100%"
        AllowSorting="true"
        OnPageIndexChanging="gvHospice_PageIndexChanging"
        OnSorting="gvHospice_Sorting"
        RowStyle-VerticalAlign="Top"
        AllowPaging="True"
        PageSize="15"
        GridViewSortColumn="HospiceTrackNo" GridViewSortDirection="Ascending"
        DataKeyNames="HospiceTrackNo,RecipID,RecipName,SubmissionDate,Status,DenialReason"
        OnRowCommand="gvHospice_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Hospice Tracking Number">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lnkBtnHosNo" CommandName="ShowHosLinkage" Text='<%# Eval("HospiceTrackNo") %>' CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="HospiceTrackNo" HeaderText="Hospice Track No" SortExpression="HospiceTrackNo" Visible="false" />
            <asp:BoundField DataField="RecipID" HeaderText="Medicaid Billing Number" SortExpression="RecipID" />
            <asp:BoundField DataField="RecipName" HeaderText="Name" SortExpression="RecipName" />
            <asp:BoundField DataField="SubmissionDate" HeaderText="Date Recieved" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" SortExpression="SubmissionDate" />
            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
            <asp:BoundField DataField="DenialReason" HeaderText="Denial Reason" SortExpression="DenialReason" />
        </Columns>
    </asp:GridView>

</asp:Panel>

<asp:Panel ID="pnlHospiceEnrollment" runat="server" Visible="false">
    <%--<uc1:HospiceEnrollment ID="uc1HospiceEnrollment" runat="server" Visible="true" EnableViewState="true" />--%>
    <%-- <iframe id="iframeHospiceEnrollment" runat="server"></iframe>--%>
    <asp:PlaceHolder runat="server" ID="HospicePlaceholderInitial"></asp:PlaceHolder>

</asp:Panel>