<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_CertSecondGrid" Codebehind="CertSecondGrid.ascx.cs" %>
<%@ Register Src="~/UserControls/FormField.ascx" TagPrefix="uc" TagName="FormField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/CertSecondGridHistory.ascx" TagPrefix="uc" TagName="CertSecondGridHistory" %>
<%@ Register Src="~/PopupControls/CLIALabCodes.ascx" TagPrefix="uc" TagName="CLIALabCodes" %>
<%@ Register Src="~/PopupControls/CLIACertificationsInfo.ascx" TagPrefix="uc" TagName="CLIACertificationsInfo" %>
<script type="text/javascript" >
</script>


<asp:Panel ID="pnlCertificationsCLIA" runat="server">
    <div class="divGrid">
        <telerik:RadGrid ID="grdCertSecondGrid" EnableAriaSupport="true" FilterMenu-AriaSettings-Label="WorkFlow" allowpaging="false"
            skin="PDMSModern" runat="server" Width="98%" AutoGenerateColumns="False"
            allowsorting="true" PageSize="10" CssClass="gridview" AllowFilteringByColumn="true"
            OnItemCommand="grdCertSecondGrid_ItemCommand" OnItemDataBound="grdCertSecondGrid_ItemDataBound"
            EmptyDataText="No CLIA number found">
            <MasterTableView autogeneratecolumns="False" datakeynames="CLIA_NUMBER, CLIA_EFF_DATE, CLIA_END_DATE, MODIFIED_STATUS_TYPE_ID">
                <Columns>
                    <telerik:GridTemplateColumn DataField="CLIA_NUMBER"  SortExpression="CLIA_NUMBER" HeaderText="CLIA Number" uniquename="CLIANumber" FilterControlWidth="180px">
                        <ItemTemplate>
                            <asp:Label ID="lblCLIANo" runat="server" Text='<%# Eval("CLIA_NUMBER") %>' />
                            <asp:LinkButton ID="lnkBtnCLIA" runat="server" Text='<%# Eval("CLIA_NUMBER") %>'
                                CommandName="ShowCliaLinkage" CommandArgument='<%# Container.ItemIndex %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridBoundColumn DataField="CLIA_EFF_DATE" HeaderText="Effective Date"
                        SortExpression="CLIA_EFF_DATE" DataFormatString="{0:d}" Visible="false" />

                    <telerik:GridTemplateColumn HeaderText="Effective Date" DataField="CLIA_EFF_DATE" SortExpression="CLIA_EFF_DATE" FilterControlWidth="180px">
                        <ItemTemplate>
                            <%# Helper.FormatDate2(Eval("CLIA_EFF_DATE").ToString()) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridBoundColumn DataField="CLIA_END_DATE" HeaderText="Expiration Date"
                        SortExpression="CLIA_END_DATE" DataFormatString="{0:d}" Visible="false" />

                    <telerik:GridTemplateColumn HeaderText="Expiration Date"  DataField="CLIA_END_DATE" FilterControlWidth="180px" SortExpression="CLIA_END_DATE">
                        <ItemTemplate>
                            <%# Helper.FormatDate2(Eval("CLIA_END_DATE").ToString()) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn Visible="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdnMODIFIED_STATUS_TYPE_ID" runat="server"
                                Value='<%# Eval("MODIFIED_STATUS_TYPE_ID") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="<span style='display:none'>Edit</span>" ItemStyle-Width="2%" AllowFiltering="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" runat="server" CommandName="EditCertSecondGridRow"
                                CommandArgument='<%# Container.ItemIndex %>' ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="<span style='display:none'>Delete</span>" ItemStyle-Width="2%" AllowFiltering="false">
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteCertSecondGridRow"
                                CommandArgument='<%# Container.ItemIndex %>' ImageUrl="~/Images/cancel.png"
                                ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');"
                                AlternateText="Delete" Visible='<%# CanUserViewDelete(Container.ItemIndex) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle CssClass="gridpager" mode="NextPrevAndNumeric" alwaysvisible="true" pagesizelabeltext="Page Size: " pagesizes="50,100,500,1000,10000"
                    HorizontalAlign="Right" />
            </MasterTableView>

            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingItemStyle CssClass="gridViewAltRow" />
            <ItemStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </telerik:RadGrid>
    </div>
    <div class="divHistoryAndAdd">
        <asp:ImageButton ID="btnAddCertSecondGrid" runat="server" ImageUrl="~/Images/add.png" AlternateText="Add New" CommandName="CertSecondGrid" OnCommand="lbtnAdd_Click" ToolTip="Add" />
        <br/>
        <asp:linkbutton ID="btnCertSecondGridHistory" CommandName="CertSecondGrid" runat="server" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" Text="History" ToolTip="History" style="color:white; text-decoration: none;">
            <span class="glyphicon glyphicon-book" style="padding-right:7px;"></span>History 
        </asp:linkbutton>
    </div>
</asp:Panel>
<div style="width: 100%; text-align: right; display: none;">
    <asp:HiddenField ID="hdnRowCount" runat="server" />
    <asp:Button ID="lnkExcel" CommandName="Export" OnClick="lnkExcel_Click" runat="server" ToolTip="Excel" Visible="true" Text="Export"/>
    <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
    </div>
    <telerik:RadGrid ID="grdCLIAHistory" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
        AutoGenerateColumns="false" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
            <Excel Format="Biff" />
        </ExportSettings>
        <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
            DataKeyNames="INDEX, DateOfAction" EnableHeaderContextMenu="false" AllowMultiColumnSorting="false">
            <Columns>
                <telerik:GridBoundColumn DataField="Operation" HeaderText="Operation"    SortExpression="Operation" />
                <telerik:GridBoundColumn DataField="CLIA_NUMBER" HeaderText="CLIA Number"    SortExpression="CLIA_NUMBER" />
                <telerik:GridBoundColumn DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                <telerik:GridBoundColumn DataField="CLIA_EFF_DATE" HeaderText="Effective Date" DataFormatString="{0:MM/dd/yyyy}" SortExpression="CLIA_EFF_DATE" />
                <telerik:GridBoundColumn DataField="CLIA_END_DATE" HeaderText="Expiration Date" DataFormatString="{0:MM/dd/yyyy}" SortExpression="CLIA_END_DATE" />
                <telerik:GridBoundColumn DataField="DateOfAction" HeaderText="Update Date" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False" SortExpression="DateOfAction" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>

    <div>
        <asp:ValidationSummary ID="CliaValidationSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="CertSecondGrid" />
    </div>
    <div id="cliaAddDetail" runat="server" visible="false">
    <div class="container-fluid">
              
        <div class="row">
            <div class="col-sm-3 text-right">
                <asp:Label ID="Label1" runat="server" Text="CLIA Number*" CssClass="formLabel170" />
            </div>
            <div class="col-sm-7" style="text-align: left;">
                <asp:TextBox ID="prov_Number" aria-label="CLIA Number" runat="server" CssClass="formField150" MaxLength="10" Enabled="false" OnTextChanged="prov_Number_TextChanged" AutoPostBack="true" onkeyup=""  />
                <asp:CustomValidator ID="cvCLIANumber"
                    ControlToValidate="prov_Number"
                    Display="Dynamic" ValidateEmptyText="true"
                    ErrorMessage="* CLIA Number is required." Text="*"
                    ValidationGroup="CertSecondGrid" SetFocusOnError="True"
                    runat="server" />
                <asp:RegularExpressionValidator ID="valCliaFormat" runat="server" ControlToValidate="prov_Number"
                    ValidationExpression="^\d{2}[Dd]{1}\d{7}$" ErrorMessage="* Enter a valid 10 digit CLIA number in the expected format (2 digits, 'D', 7 digits)."
                    Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="CertSecondGrid" Display="Dynamic" EnableClientScript="true" />
            </div>
            <div class="col-sm-2" style="text-align: left;">
                <div class="pdmsLabel">
                    <asp:Label ID="pdms_Number" runat="server" /></div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-3 text-right">
                <asp:Label ID="lblCLIACertificateType" runat="server" Text="CLIA Certification Type" CssClass="formLabel170" />
            </div>
            <div class="col-sm-7" style="text-align: left;">
                <asp:DropDownList ID="ddlCliaCertType" aria-label="CLIA Certification Type" runat="server" CssClass="formDropDown" />
                </div>
        </div>
    
        <div class="row" style="visibility: visible">
            <div class="col-sm-3 text-right">
                <asp:Label ID="Label2" runat="server" Text="CLIA Effective Date" CssClass="formLabel170" />
            </div>
            <div class="col-sm-7" style="text-align: left;">
                <asp:TextBox ID="prov_Start" runat="server" aria-label="CLIA Effective Date" CssClass="formField formField" /><ajax:CalendarExtender ID="calStart" TargetControlID="prov_Start" runat="server" />
                <%--<asp:RequiredFieldValidator runat="server" ID="rfProv_Start" ControlToValidate="prov_Start" ErrorMessage="" Text="" Display="Dynamic" SetFocusOnError="true" ValidationGroup="CertSecondGrid" />--%>
                <asp:CompareValidator ID="dateValidator" runat="server" ValidationGroup="CertSecondGrid"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="prov_Start"
                    ErrorMessage="Select a valid Effective Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> 
                </asp:CompareValidator>

            </div>
            <div class="col-sm-2">
                <div class="pdmsLabel">
                    <asp:Label ID="pdms_Start" runat="server" /></div>
            </div>
        </div>
        <div class="row" style="visibility: visible">
            <div class="col-sm-3 text-right">
                <asp:Label ID="Label3" runat="server" Text="CLIA Expiration Date" CssClass="formLabel150" />
            </div>
            <div class="col-sm-7" style="text-align: left;">
                <asp:TextBox ID="prov_End" runat="server" aria-label="CLIA Expiration Date" CssClass="formField formField" /><ajax:CalendarExtender ID="calEnd" TargetControlID="prov_End" runat="server" />
                <%--<asp:RequiredFieldValidator runat="server" ID="rfProv_End" ControlToValidate="prov_End" ErrorMessage="" Text="" Display="Dynamic" SetFocusOnError="true" ValidationGroup="CertSecondGrid" />--%>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="CertSecondGrid"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="prov_End"
                    ErrorMessage="Select a valid Expiration Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> 
                </asp:CompareValidator>
            </div>
            <div class="col-sm-2">
                <div class="pdmsLabel">
                    <asp:Label ID="pdms_End" runat="server" /></div>
            </div>
        </div>
    </div>
    <br />
    <uc1:Separator ID="ucSep1" runat="server" Header="Uploaded Documents" />
    <br />
    <asp:PlaceHolder runat="server" ID="PlaceholderUploadCLIA" Visible="false"></asp:PlaceHolder>

    <asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
    <asp:TextBox ID="hidID" runat="server" Visible="false" />
</div>
<style type="text/css">
    .modalPopup
    {
        height: auto;
        left: 20% !important;
        top: 30% !important;
        position: fixed !important;
    }
/* Grid container */
.gridview {
    width: 98%;
    border-collapse: collapse;
    font-family: Arial, sans-serif;
    font-size: 14px;
}

/* Header styling */
.gridViewHeader {
    font-weight: bold;
    text-align: left;
    padding: 8px;
}

/* Row styling */
.rgFilterRow {
    background-color: #d3d3d3;
    padding: 8px;
    border-bottom: 1px solid #ddd;
}

/* Row styling */
.gridViewRow {
    background-color: #f7f7f7;
    padding: 8px;
    border-bottom: 1px solid #ddd;
}

/* Alternating row styling */
.gridViewAltRow {
    background-color: #f9f9f9;
    padding: 8px;
    border-bottom: 1px solid #ddd;
}

/* Footer styling */
.gridViewFooter {
    background-color: #f2f2f2;
    font-weight: bold;
    padding: 8px;
}

/* Pager styling */
.gridpager {
    text-align: right;
    padding: 10px;
    font-size: 13px;
}

/* Link buttons */
.gridLink {
    color: #007acc;
    text-decoration: none;
    font-weight: bold;
}

.gridLink:hover {
    text-decoration: underline;
}

/* Hide header text visually but keep for accessibility */
th span[style*="display:none"] {
    position: absolute;
    left: -9999px;
}

</style>
<asp:Panel ID="upCertSecondGridHistory" runat="server" style="position: fixed; left: 0; top: 0;  width: 60%;">
    <ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" CancelControlID="btnModalOk" TargetControlID="ButtonDummy3"
        BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlModal">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 800px;">
        <asp:Panel ID="pnlHeader" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
            <div align="left">
                &nbsp;&nbsp;
        <asp:Label ID="lblTitle" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlMain1" runat="server" Style="padding: 10px">
            <div>
                <uc:CertSecondGridHistory ID="ucCertSecondGridHistory" runat="server" />
            </div>
        </asp:Panel>
        <div class="btnBox" style="padding: 10px">
            <asp:Button runat="server" ID="btnModalOk" Text="OK" CssClass="buttonBox" aria-Label="Dummy Button" CausesValidation="false" />
            <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkExcel_Click" />
        </div>
    </asp:Panel>
    <asp:Button runat="server" ID="ButtonDummy3" Style="display: none" Text="”ButtonDummy3”" />
</asp:Panel>

<asp:Panel ID="pnlCliaLinkage" runat="server" Visible="false" GroupingText="CLIA Linkage">

<div id="CliaInfo" class="container-fluid theme-blue">
    <div class="row">
        <div class="col-sm-3 text-right">
                    <asp:Label ID="lblLinkCLIANumber" runat="server" Text="CLIA Number:" CssClass="formLabelSmall" />
        </div>
        <div class="col-sm-3 text-left">
                    <asp:Label ID="lbLinkCLIANumber" runat="server" CssClass="formLabelBoldSmall" />
         </div>
        <div class="col-sm-3 text-right">
                    <asp:Label ID="lblLinkCLIACertType" runat="server" Text="CLIA Certification Type:" CssClass="formLabelSmall" />
        </div>
        <div class="col-sm-3 text-left">
                    <asp:Label ID="lblLinkCLIACertTypeRst" runat="server" CssClass="formLabelBoldSmall" />
         </div>
         <div class="col-sm-3 text-right">
                    <asp:Label ID="lblLinkCliaEffective" runat="server" Text="Effective Date:" CssClass="formLabelSmall" />
        </div>
        <div class="col-sm-3 text-left">
                    <asp:Label ID="lblLinkCliaEffectiveDate" runat="server" CssClass="formLabelBoldSmall" />
         </div>
   </div>
    <div class="row">       
        
         <div class="col-sm-9 text-right">
                    <asp:Label ID="lblLinkCliaEnd" runat="server" Text="End Date:" CssClass="formLabelSmall" />
        </div>
        <div class="col-sm-3 text-left">
                    <asp:Label ID="lblLinkCliaEndDate" runat="server" CssClass="formLabelBoldSmall" />
         </div>
        
   </div>
</div>

<div Class="theme-blue">
   <uc:CLIACertificationsInfo ID="CLIACertificationsInfo1" runat="server" />
</div>

<div class="theme-blue">
   <uc:CLIALabCodes ID="CLIALabCodes1" runat="server" />
</div>

</asp:Panel>