<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_RetrieveReports" Codebehind="RetrieveReports.ascx.cs" %>

<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>

<script type="text/javascript">
    function pageLoad() {
        $('[data-toggle="popover"]').popover()
    }
    function SelectAll(id) {
        var frm = document.forms[0];
        for (i = 0; i < frm.elements.length; i++) {
            var thisName = frm.elements[i].name;
            if (frm.elements[i].type == "checkbox" && thisName.indexOf("chkSelect") != -1) {
                frm.elements[i].checked = document.getElementById(id).checked;
            }
        }
    }

    function CollapseExpand(obj) {
        var sp = obj.getElementsByTagName('span')[0];
        var spText = sp.innerHTML;
        var plusIndex = spText.indexOf("+");

        if (plusIndex == 0)
            sp.innerHTML = spText.replace("+", "-");
        else
            sp.innerHTML = spText.replace("-", "+");
    }
    function exportPopup() {
        var rowCount = $("#<%= hdnRowCount.ClientID %>").first().val();
        if (rowCount > 10000) {
            $("#divRowCount").dialog();
        }
    }

</script>

<style type="text/css">
    .formDropdownPF {
        border: 1px solid #ccc;
        background-color: #FFFFFF;
        color: #000;
        min-width: 130px;
        font-family: "Arial Narrow";
        font-size: 14pt !important;
        vertical-align: top;
        height: 40px;
    }

    .formField {
        border: 1px solid #ccc;
        background-color: #FFFFFF;
        color: #000;
        width: 274px;
    }

    select {
        min-width: 273px;
        height: 41px;
        border: 1px solid #ccc;
    }

    .formLabel200 {
        width: 178px;
        /*width: 162px;*/
    }

    .col-lg-6 {
        /* width: 50%; */
    }

    .pnl-seach {
        min-height: 530px;
        min-width: 300px;
        height: auto;
        width: auto;
    }

    .watermarked {
        background-color: #F7F6F3;
        border: solid 1px #808080;
        padding: 3px;
        color: #717171;
    }

    .unwatermarked {
        border: solid 1px #808080;
        padding: 3px;
        color: Gray;
    }

    .pagePnlSearchHeader {
        color: white;
        font-size: 24px;
        font-weight: bold;
    }

    .pagePnlTextField {
        font-size: 16px;
    }

    .mySearchButton {
        box-shadow: inset 0px 1px 0px 0px #9acc85;
        background: linear-gradient(to bottom, #288128 5%, #288128 100%);
        background-color: #288128;
        border: 1px solid #288128;
        display: inline-block;
        cursor: pointer;
        color: #ffffff;
        font-family: Arial;
        font-size: 18px;
        font-weight: bold;
        padding: 6px 12px;
        text-decoration: none;
    }

    .myPrintButton {
        box-shadow: inset 0px 1px 0px 0px #9acc85;
        background: linear-gradient(to bottom, #438fff 5%, #438fff 100%);
        background-color: #438fff;
        border: 1px solid #438fff;
        display: inline-block;
        cursor: pointer;
        color: #ffffff;
        font-family: Arial;
        font-size: 18px;
        font-weight: bold;
        padding: 6px 12px;
        text-decoration: none;
    }

    .collapsiblePanelContainer {
        height: 0;
        overflow: hidden;
    }

    .style3 {
        color: black;
        font-weight: bold;
    }

    td, h1, h2 {
        margin: 3px !important;
        padding-left: 5px !important;
    }

    .panelHeaderStyle {
        background-color: #2297bc;
        border-style: none;
    }

    .col-sm-3 {
        text-align: right;
    }
</style>

<cc1:GroupBox ID="gbSearch" HorizontalAlign="Center" Width="100%" runat="server">
    <ajax:collapsiblepanelextender id="cpeFinancialSearch" runat="server" collapsed="false" targetcontrolid="pnlFilter"
    expandcontrolid="pnlFSCPESearch" collapsecontrolid="pnlFSCPESearch"
    expandedtext="-" collapsedsize="0" scrollcontents="true" collapsedtext="+" expanddirection="Vertical"
    suppresspostback="true" textlabelid="lblsepContact" />
    <asp:panel runat="server" id="pnlFSCPESearch" class="CollapsingSeparator" style="background-color: #2197bb;"
        tooltip="Click to Expand/Collapse" cssclass="OwnerAuthSearch CollapsingSeparator">

        <div class="pageHeader pH2">
            <table width="100%" class="collaps-table">
                <tr>
                    <td align="left" style="color: white;">
                        <h1><span style="color: white; font-weight: bold;">
                            <button type="button" class="panelHeaderStyle" tabindex="0" id="searchReports" aria-expanded="true" >Search Reports</button></span></h1>
                        <asp:label runat="server" id="lblsepContact1" />
                    </td>
                    <td style="padding-right: 10px; margin: 30px; width: 35px">
                        <asp:label runat="server" id="lblsepContact" style="color: white" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:panel>
    <asp:Label runat="server" ID="lblMessages" CssClass="error-message" />
    <div style="text-align: left">
        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="valRetrieveReports" ShowSummary="true" />
    </div>
    <div style="text-align: center;">
        <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">
            <div class="container-fluid">
                <asp:UpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-sm-12 col-md-12 col-lg-4 outerName">
                                <asp:Label ID="Label1" class="ohio-select" AssociatedControlID="" runat="server">
                                    <span class="ohio-select-label">Document Type: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right"
                                        data-content="<asp:Literal ID='popupRetrieveReportsDTy' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_RETRIEVEREPORTS_POPUP_DocumentType %>' />"
                                        aria-hidden="true"></span>
                                    </span>
                                    <span class="ohio-select-select fa" aria-hidden="true">
                                        <asp:DropDownList ID="ddlDocumentType" CssClass="ohio-select-select-el" runat="server" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </span>
                                </asp:Label>
                            </div>
                            <div class="col-sm-12 col-md-6 col-lg-3 outerName">
                                <asp:Label ID="Label2" CssClass="ohio-field" AssociatedControlID="txtDateAvailableFrom" runat="server">
                                    <span class="ohio-field-label">Date Avaliable From: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right"
                                        data-content="<asp:Literal ID='popupRetrieveReportsDF' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_RETRIEVEREPORTS_POPUP_DateFrom %>' />"
                                        aria-hidden="true"></span>
                                    </span>
                                    <span class="input-group">
                                        <ajax:CalendarExtender
                                            ID="calExDateAvaliableFrom"
                                            runat="server"
                                            Format="MM/dd/yyyy"
                                            TargetControlID="txtDateAvailableFrom"
                                            PopupPosition="BottomLeft"
                                            CssClass=""
                                            PopupButtonID=""
                                            EnabledOnClient="true" />
                                        <asp:TextBox ID="txtDateAvailableFrom" CssClass="ohio-field-input" runat="server" />
                                        <asp:CompareValidator
                                            ID="CompareValidator1"
                                            runat="server"
                                            Type="Date"
                                            Operator="DataTypeCheck"
                                            ControlToValidate="txtDateAvailableFrom"
                                            ErrorMessage="Select a valid PNM Date Available From"
                                            Display="Dynamic"
                                            ValueToCompare="MM/dd/yyyy"
                                            ValidationGroup="valRetrieveReports"
                                            Style="position: absolute"
                                            SetFocusOnError="true"> 
                                        </asp:CompareValidator>
                                        <span class="input-group-addon">
                                            <span class="fa-date" aria-hidden="true"></span>
                                        </span>
                                    </span>
                                </asp:Label>
                            </div>
                            <div class="col-sm-6 col-md-6 col-lg-3 outerName">
                                <asp:Label ID="lblDateAvailableTo" CssClass="ohio-field" AssociatedControlID="txtDateAvailableTo" runat="server">
                                    <span class="ohio-field-label">Date Avaliable To: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right"
                                        data-content="<asp:Literal ID='popupRetrieveReportsDTo' runat='server' Text='<%$ Resources:BrandingResource , PROVIDER_RETRIEVEREPORTS_POPUP_DateTo %>' />"
                                        aria-hidden="true"></span>
                                    </span>
                                    <span class="input-group">
                                        <ajax:CalendarExtender
                                            ID="CalendarExtender2"
                                            runat="server"
                                            Format="MM/dd/yyyy"
                                            TargetControlID="txtDateAvailableTo"
                                            PopupPosition="BottomLeft"
                                            CssClass=""
                                            PopupButtonID=""
                                            EnabledOnClient="true" />
                                        <asp:TextBox ID="txtDateAvailableTo" CssClass="ohio-field-input" runat="server" />
                                        <asp:CompareValidator
                                            ID="cvDateAvailableTo"
                                            runat="server"
                                            Type="Date"
                                            Operator="DataTypeCheck"
                                            ControlToValidate="txtDateAvailableTo"
                                            ErrorMessage="Select a valid PNM Date Available To"
                                            Display="Dynamic"
                                            ValueToCompare="MM/dd/yyyy"
                                            Style="position: absolute"
                                            ValidationGroup="valRetrieveReports"
                                            SetFocusOnError="true"> 
                                        </asp:CompareValidator>
                                        <span class="input-group-addon">
                                            <span class="fa-date" aria-hidden="true"></span>
                                        </span>
                                    </span>
                                </asp:Label>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="pg-hint3Center pg-hint3">When Date Avaliable From and To fields are blank, search results will not include previously downloaded files.</div>
                <%--<div class="btnBox btnBoxCenter">--%>
                <div class="btnBox btnBoxCenter">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocusBlue" OnClick="btnSearch_Click" ValidationGroup="valRetrieveReports" OnClientClick="showProgress()" />
                            <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBoxFocusred" OnClick="btnClear_Click" OnClientClick="showProgress()" />

                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSearch" />
                            <asp:PostBackTrigger ControlID="btnClear" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <%-- </div>--%>
        </asp:Panel>
        <asp:UpdateProgress ID="UpdateProgress" runat="server" DisplayAfter="1">
            <ProgressTemplate>
                <div style="padding-right: 30px">
                    <img src="../Images/ajax-loader.gif" alt="" />
                    Loading ...
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
</cc1:GroupBox>
<br />
<div style="width: 100%; text-align: right">
    <asp:HiddenField ID="hdnRowCount" runat="server" />
    <telerik:RadGrid ID="RadGridExport" runat="server" Visible="true">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true">
            <Pdf PageHeight="8.5in" PageWidth="11in" PageTitle="Trade File Search Results">
                <PageFooter>
                    <RightCell Text="Page <?page-number?>" />
                </PageFooter>
            </Pdf>
        </ExportSettings>
        <MasterTableView AutoGenerateColumns="false">
            <Columns>
                <telerik:GridBoundColumn UniqueName="col1" HeaderText="File Name" DataField="FileName"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col2" HeaderText="Document Type" DataField="DocumentType"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col3" HeaderText="Date Available" DataField="DateAvailable"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col4" HeaderText="Date Downloaded" DataField="DateDownloaded"></telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <asp:Label ID="lnkRowCount" Visible="false" runat="server" Style="float: left" CssClass="formLabelAuto">Row Count: <%= hdnRowCount.Value.ToString() %></asp:Label><asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/Excel_24x24.png" alt="PDF" /></asp:LinkButton>&nbsp;&nbsp;
    <asp:LinkButton ID="lnkPDF" runat="server" ToolTip="PDF" OnClick="lnkPDF_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/PDF_24x24.png" alt="PDF" /></asp:LinkButton><div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10,000 results from the search will be exported.</p>
    </div>

</div>

<div id="LoadingPanel" style="display: none; color: red; text-align: center;">Retrieving Reports...</div>
<br />

<div class="row" style="border: groove; margin-left: 10px">
<asp:Panel ID="pnlrReportResult" runat="server" Visible="false">
    <div id="divRRSearchHeader" class="RetrieveReportsSearchHeader" style="background-color:#2297bc;height:52px;text-align:left;font-size:1.5vw" runat="server" visible="false">
         
    </div>
    <mms:SortablePagingGridView
        ID="gvRetrieveReports"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="gridViewSmallFont" Width="100%"
        AllowSorting="true"
        EmptyDataText="No Reports found."
        OnPageIndexChanging="gvRetrieveReports_PageIndexChanging"
        OnRowCommand="gvRetrieveReports_RowCommand"
        OnSorting="gvRetrieveReports_Sorting"
        RowStyle-VerticalAlign="Top"
        AllowPaging="True"
        PageSize="15"
        GridViewSortColumn="" GridViewSortDirection="Ascending"
        DataKeyNames="DOCUMENT_ATTACHMENT_XREF_ID, FileName">
        <Columns>
            <asp:BoundField DataField="FileName" HeaderText="File Name" SortExpression="FileName" />
            <asp:BoundField DataField="DocumentType" HeaderText="Document Type" SortExpression="DocumentType" />
            <asp:BoundField DataField="DateAvailable" HeaderText="Date Available" SortExpression="DateAvailable" DataFormatString="{0:d}" />
            <asp:BoundField DataField="DateDownloaded" HeaderText="Date Downloaded" SortExpression="DateDownloaded" DataFormatString="{0:d}" />
            <asp:TemplateField ShowHeader="False" HeaderText="">
                <ItemTemplate>
                    <asp:LinkButton
                        ID="LnkButtonDownload"
                        runat="server"
                        CausesValidation="false"
                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                        CommandName="OnFileDownload"
                        Text="Download Report"
                         Visible='<%# (! Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsDownloaded"))) %>'
                        CssClass="gridLink" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </mms:SortablePagingGridView>
</asp:Panel>
<asp:Panel ID="pnlReportResult277" runat="server" Visible="false">
    <div id="div1" class="RetrieveReportsSearchHeader" runat="server"  style="background-color:#ABBAEA;height:52px;text-align:left;font-size:1.5vw">
          277 Claim Submission Response Search Results 
       </div>
    <mms:SortablePagingGridView
        ID="gvRetrieveReports277"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="gridViewSmallFont" Width="100%"
        AllowSorting="true"
        EmptyDataText="No Reports found."
        OnPageIndexChanging="gvRetrieveReports277_PageIndexChanging"
        OnRowCommand="gvRetrieveReports277_RowCommand"
        RowStyle-VerticalAlign="Top"
        AllowPaging="True"
        PageSize="15"
        GridViewSortColumn="" GridViewSortDirection="Ascending"
        DataKeyNames="DOCUMENT_ATTACHMENT_XREF_ID, FileName, Name, HTML_BODY">
        <Columns>
            <asp:BoundField DataField="StatusDate" HeaderText="Status Date" DataFormatString="{0:d}" />
            <asp:BoundField DataField="PayerName" HeaderText="Payer Name" />
            <asp:BoundField DataField="PatientControlNumber" HeaderText="Patient Control Number" DataFormatString="{0:d}" />
            <asp:BoundField DataField="ICN" HeaderText="ICN" SortExpression="ICN" />
            <asp:BoundField DataField="DateAvailable" HeaderText="Date Available" DataFormatString="{0:d}" />
            <asp:BoundField DataField="DateDownloaded" HeaderText="Date Downloaded"  DataFormatString="{0:d}" />
            <asp:TemplateField ShowHeader="False" HeaderText="">
                <ItemTemplate>
                    <asp:LinkButton
                        ID="LnkButtonDownload11"
                        runat="server"
                        CausesValidation="false"
                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                        CommandName="OnFileDownload"
                        Text="Download Report"
                        Visible='<%# (! Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsDownloaded"))) %>'
                        CssClass="gridLink" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </mms:SortablePagingGridView>
</asp:Panel>

<asp:Panel ID="pnlReportResult278" runat="server" Visible="false">
      <div id="div2" class="RetrieveReportsSearchHeader" runat="server" style="background-color:#ABBAEA;height:52px;text-align:left;font-size:1.5vw">
          278 Prior Authorization Submission Response Results 
      </div>
    <mms:SortablePagingGridView
        ID="gvRetrieveReports278"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="gridViewSmallFont" Width="100%"
        AllowSorting="true"
        EmptyDataText="No Reports found."
        OnPageIndexChanging="gvRetrieveReports278_PageIndexChanging"
        OnRowCommand="gvRetrieveReports278_RowCommand"
        RowStyle-VerticalAlign="Top"
        AllowPaging="True"
        PageSize="15"
        GridViewSortColumn="" GridViewSortDirection="Ascending"
        DataKeyNames="DOCUMENT_ATTACHMENT_XREF_ID, FileName, Name, HTML_BODY">
        <Columns>
           <asp:BoundField DataField="StatusDate" HeaderText="Status Date" DataFormatString="{0:d}" />
            <asp:BoundField DataField="PayerName" HeaderText="Payer Name" />
            <asp:BoundField DataField="PatientTrackingNumber" HeaderText="Patient Tracking Number" />
            <asp:BoundField DataField="Prior_Auth_Number" HeaderText="Prior Auth Number" />
            <asp:BoundField DataField="DateAvailable" HeaderText="Date Available" DataFormatString="{0:d}" />
            <asp:BoundField DataField="DateDownloaded" HeaderText="Date Downloaded" DataFormatString="{0:d}" />
            <asp:TemplateField ShowHeader="False" HeaderText="">
                <ItemTemplate>
                    <asp:LinkButton
                        ID="LnkButtonDownload1"
                        runat="server"
                        CausesValidation="false"
                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                        CommandName="OnFileDownload"
                        Text="Download Report"
                         Visible='<%# (! Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "IsDownloaded"))) %>'
                        CssClass="gridLink" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </mms:SortablePagingGridView>
</asp:Panel>
    <asp:Panel ID="pnlPASRRReports" runat="server" Visible="false">
    <div id="div3" class="RetrieveReportsSearchHeader" runat="server"  style="background-color:#ABBAEA;height:52px;text-align:left;font-size:1.5vw">
          PASRR Reports Search Results 
       </div>
    <mms:SortablePagingGridView
        ID="gvPASRRReports"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="gridViewSmallFont" Width="100%"
        AllowSorting="true"
        EmptyDataText="No Reports found."
        OnPageIndexChanging="gvPASRRReports_PageIndexChanging"
        OnRowCommand="gvPASRRReports_RowCommand"
        RowStyle-VerticalAlign="Top"
        AllowPaging="True"
        PageSize="15"
        GridViewSortColumn="" GridViewSortDirection="Ascending"
        DataKeyNames="DOCUMENT_ATTACHMENT_XREF_ID, FileName, DOCUMENT_ID">
        <Columns>
            <asp:BoundField DataField="FileName" HeaderText="Document Title" DataFormatString="{0:d}" />

            
            <asp:BoundField DataField="DateAvailable" HeaderText="Posted Date" DataFormatString="{0:d}" />
            <asp:BoundField DataField="DateDownloaded" HeaderText="Last Accessed Date"  DataFormatString="{0:d}" />
            <asp:TemplateField ShowHeader="False" HeaderText="">
                <ItemTemplate>
                    <asp:LinkButton
                        ID="lnkPASRR"
                        runat="server"
                        CausesValidation="false"
                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                        CommandName="OnFileDownload"
                        Text="Download Report"
                        
                        CssClass="gridLink" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </mms:SortablePagingGridView>
</asp:Panel>
<cc2:MessageBox ID="MessageBox2" runat="server" />
</div>