<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_Correspondence" Codebehind="Correspondence.ascx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>


<script type="text/javascript">
    function checkDate(sender, args) {
        if (sender._selectedDate > new Date()) {
            alert("You cannot select a day earlier than today!");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
        }
    }
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

    function correspondenceTypeChange() {
        document.getElementById("<%= txtDateAvailableFrom.ClientID%>").value = ''
        document.getElementById("<%= txtDateAvailableTo.ClientID%>").value = ''
        $("#<%= gvCorrespondencesearch.ClientID%>").remove();
        $("#<%= gvConvertedCorrespondencesearch.ClientID%>").remove();

        var endDate = new Date();
        $("#<%= calExtDateAvailableFrom.ClientID%>").datepicker("destroy");
        $("#<%= calExtDateAvailableTo.ClientID%>").datepicker("destroy");

        $("#<%= calExtDateAvailableFrom.ClientID%>").datepicker("refresh");
        $("#<%= calExtDateAvailableTo.ClientID%>").datepicker("refresh");

        $("#<%= calExtDateAvailableFrom.ClientID%>").datepicker('option', 'maxDate', endDate);
        $("#<%= calExtDateAvailableTo.ClientID%>").datepicker('option', 'maxDate', endDate);

    }

    function printPartOfPage(elementId) {
        var printContent = document.getElementById(elementId);
        var windowUrl = '';
        var uniqueName = new Date();
        var windowName = '';
        var printWindow = window.open('left=50000,top=50000,width=0,height=0');
        printWindow.document.write(printContent.innerHTML);
        printWindow.document.close();
        printWindow.focus();
        printWindow.print();
        printWindow.close();
    }
    function btnSearch_Click() {
        var selectedOption = document.getElementById("ddlCorrespondenceType").value;
        if (selectedOption === "") {
            document.getElementById("requiredMessage").style.display = "block";
            return false; // Prevent form submission
        }
    }


</script>
<style type="text/css">
    .headRowSortable {
        background: url('../App_Themes/Default/Grid/SortAsc.gif') left center no-repeat;
        background-size: contain;
        text-decoration: none;
        vertical-align: central;
    }
</style>




<div class="row col-sm-13" style="border: groove; margin-left: 10px">

    <ajax:collapsiblepanelextender id="cpeCorrespondence" runat="server" collapsed="false" targetcontrolid="pnlCorrespondence"
        expandcontrolid="pnlsepCorrespondence" collapsecontrolid="pnlSepCorrespondence" />
    <asp:Panel runat="server" ID="pnlSepCorrespondence" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: cornflowerblue;" ToolTip="Click to Expand/Collapse">
        <h1><span id="sepCorrespondence" runat="server" class="pageHeader">- *  Search Correspondence</span></h1>
    </asp:Panel>
    <span id="requiredMessage" role="alert" aria-live="assertive" style="color: #CC0505; font-size: 14pt !important; font-weight: 100 !important">An asterisk * indicates a required field</span>
    <%--    <asp:Panel runat="server" ID="pnlInstructions" class="OwnerBackground">--%>
    <asp:Panel ID="pnlCorrespondence" runat="server" Style="min-height: 340px; min-width: 300px; height: 238px; width: auto; max-width: 1497px;">
        <div class="row">
            <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" Style="margin-left: 20px;" ValidationGroup="valCorrespondence" ShowSummary="true" CssClass="failureNotification" />

            <div id="CorrespondenceType" class="col-sm-5">
                <span class="ohio-field-label" style="margin-top: 8px;"><span style="color: red">*</span>Correspondence TYPE
                          <%--  <asp:Label ID="lblAttDocType" runat="server" Text="Document Type" CssClass="formLabel200" />--%>
                    <asp:DropDownList ID="ddlCorrespondenceType" aria-label="*Correspondence TYPE" CssClass="formField" Style="margin-top: 8px;" EnableViewState="true" runat="server" onchange="correspondenceTypeChange()"
                        AppendDataBoundItems="True">
                    </asp:DropDownList>

                    <span style="color: red; display: none">Correspondence Type is required</span>
                </span>

            </div>
            <div class="col-sm-3">
                <asp:Label ID="Label2" CssClass="ohio-field" AssociatedControlID="txtDateAvailableFrom" runat="server">
                    <span class="ohio-field-label">Date Available From: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right"
                        aria-hidden="true"></span>
                    </span>
                    <span class="input-group">
                        <ajax:calendarextender
                            id="calExtDateAvailableFrom"
                            runat="server"
                            format="MM/dd/yyyy"
                            targetcontrolid="txtDateAvailableFrom"
                            popupposition="BottomLeft"
                            cssclass=""
                            popupbuttonid=""
                            enabledonclient="true" />
                        <asp:TextBox ID="txtDateAvailableFrom" CssClass="ohio-field-input" runat="server" autocomplete="off" />
                        <asp:CompareValidator
                            ID="CompareValidator1"
                            runat="server"
                            Type="Date"
                            Operator="DataTypeCheck"
                            ControlToValidate="txtDateAvailableFrom"
                            ErrorMessage="Select a valid PNM Date Available From"
                            Display="Dynamic"
                            ValueToCompare="MM/dd/yyyy"
                            ValidationGroup="valCorrespondence"
                            Style="position: absolute"
                            SetFocusOnError="true">
                        </asp:CompareValidator>
                        <span class="input-group-addon">
                            <span class="fa-date" aria-hidden="true"></span>
                        </span>
                    </span>
                </asp:Label>
            </div>
            <div class="col-sm-3">
                <asp:Label ID="lblDateAvailableTo" CssClass="ohio-field" AssociatedControlID="txtDateAvailableTo" runat="server">
                    <span class="ohio-field-label">Date Available To: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right"
                        aria-hidden="true"></span>
                    </span>
                    <span class="input-group">
                        <ajax:calendarextender
                            id="calExtDateAvailableTo"
                            runat="server"
                            format="MM/dd/yyyy"
                            targetcontrolid="txtDateAvailableTo"
                            popupposition="BottomLeft"
                            cssclass=""
                            popupbuttonid=""
                            enabledonclient="true" />
                        <asp:TextBox ID="txtDateAvailableTo" CssClass="ohio-field-input" runat="server" autocomplete="off" />
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
                            ValidationGroup="valCorrespondence"
                            SetFocusOnError="true">
                        </asp:CompareValidator>
                        <span class="input-group-addon">
                            <span class="fa-date" aria-hidden="true"></span>
                        </span>
                    </span>
                    <asp:CompareValidator
                        ID="cvDateRange"
                        runat="server"
                        Type="Date"
                        Operator="LessThanEqual"
                        ForeColor="red"
                        ControlToValidate="txtDateAvailableFrom"
                        ControlToCompare="txtDateAvailableTo"
                        ErrorMessage="Date Available To should always be greater than Date Available From"
                        Display="Dynamic"
                        Style="position: absolute"
                        ValidationGroup="valCorrespondence"
                        SetFocusOnError="true">
                    </asp:CompareValidator>
                </asp:Label>
            </div>
        </div>
        <br />
        <br />
         <div class="btnBox btnBoxCenter">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocusBlue" OnClick="btnSearch_Click" ValidationGroup="valProviderHeader" OnClientClick="showProgress()" />
                    <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBoxFocusred" OnClick="btnClear_Click" OnClientClick="showProgress()" />

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSearch" />
                    <asp:PostBackTrigger ControlID="btnClear" />
                </Triggers>
            </asp:UpdatePanel>
        </div>


        <asp:UpdateProgress ID="UpdateProgress" runat="server" DisplayAfter="1">
            <ProgressTemplate>
                <div style="padding-right: 30px">
                    <img src="../Images/ajax-loader.gif" alt="" />
                    Loading ...
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </asp:Panel>
    <ajax:collapsiblepanelextender id="cpeCorrespondencetype" runat="server" collapsed="false" targetcontrolid="pnlCorrespondencetype" expandcontrolid="pnlSepCorrespondencetype" collapsecontrolid="pnlSepCorrespondencetype" />
    <asp:Panel runat="server" ID="pnlsepCorrespondencetype" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: cornflowerblue;" ToolTip="Click to Expand/Collapse">
        <h1><span id="sepCorrespondencetype" runat="server" class="pageHeader">- Correspondence Search Result</span></h1>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlCorrespondencetype">
        <div style="width: 100%; text-align: right">
            <asp:HiddenField ID="hdnRowCount" runat="server" />
            <telerik:radgrid id="RadGridExport" runat="server" visible="true">
                <exportsettings ignorepaging="true" openinnewwindow="true">
                    <pdf pageheight="8.5in" pagewidth="11in" pagetitle="CORRESPONDENCE SEARCH RESULT">
                        <pagefooter>
                            <rightcell text="Page <?page-number?>" />
                        </pagefooter>
                    </pdf>
                </exportsettings>
                <mastertableview autogeneratecolumns="false">
                    <columns>
                        <telerik:gridboundcolumn uniquename="col1" headertext="Correspondence Subject" datafield="SUBJECT"></telerik:gridboundcolumn>
                        <telerik:gridboundcolumn uniquename="col2" headertext="Correspondence Type" datafield="CORRESPODENCE_TYPE"></telerik:gridboundcolumn>
                        <telerik:gridboundcolumn uniquename="col3" headertext="Prior Authorization Number" datafield="PA_NUMBER" visible="false"></telerik:gridboundcolumn>
                        <telerik:gridboundcolumn uniquename="col3" headertext="Hospice Tracking Number" datafield="HTN_NUMBER" visible="false"></telerik:gridboundcolumn>
                        <telerik:gridboundcolumn uniquename="col4" headertext="Recipient ID" datafield="RECIPIENT_ID" visible="false"></telerik:gridboundcolumn>
                        <telerik:gridboundcolumn uniquename="col5" headertext="Date Sent" datafield="DATE_SENT"></telerik:gridboundcolumn>
                        <telerik:gridboundcolumn uniquename="col6" headertext="Dave Viewed" datafield="DATE_VIEWED"></telerik:gridboundcolumn>
                        <telerik:gridboundcolumn uniquename="col7" headertext="Printed"></telerik:gridboundcolumn>
                    </columns>
                </mastertableview>
            </telerik:radgrid>


            <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
                <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10,000 results from the search will be exported.</p>
            </div>
        </div>
        <asp:GridView
            ID="gvCorrespondencesearch"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="gridViewSmallFont" Width="100%"
            AllowSorting="true"
            EmptyDataText="No Email found."
            OnRowDataBound="gvCorrespondencesearch_RowDataBound"
            OnRowCommand="gvCorrespondencesearch_RowCommand"
            OnPageIndexChanging="gvCorrespondencesearch_PageIndexChanging"
            OnSorting="gvCorrespondencesearch_Sorting"
            currentsortfield="DATE_SENT"
            currentsortdirection="DESC"
            RowStyle-VerticalAlign="Top"
            AllowPaging="True"
            PageSize="10"
            DataKeyNames="COMMUNICATION_EVENT_ID, SUBJECT,CORRESPODENCE_TYPE, PA_NUMBER, HTN_NUMBER, RECIPIENT_ID, DATE_SENT,DATE_VIEWED, BODY, EMAIL_TO, DOCUMENT_ID, ELIGIBILITY, DOCUMENT_TYPE, DOCUMENT_ID_PDF,  ONBASE_DOCUMENT_ID,FILE_NAME_PDF, FILE_NAME, DOCUMENT_ATTACHMENT_XREF_ID">
            <Columns>
                <asp:TemplateField HeaderText="Correspondence Subject" SortExpression="SUBJECT">
                    <ItemTemplate>
                        <asp:LinkButton
                            ID="LnkButtonEnrollment"
                            runat="server"
                            CausesValidation="false"
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Visible='<%# Eval("DOCUMENT_ID").ToString() == "" ? true : false %>'
                            CommandName="OpenEnrollment"
                            Text='<%# Eval("SUBJECT") %>'
                            CssClass="gridLink" />
                        <asp:LinkButton
                            ID="LnkButtonDownloadpdf1"
                            runat="server"
                            CausesValidation="false"
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Visible='<%# Eval("document_type").ToString() == "pdf" ? true : false %>'
                            CommandName="OnFileDownloadPDF"
                            OnClientClick="openInNewTab();"
                            Text='<%# Eval("SUBJECT") %>'
                            CssClass="gridLink" />
                        <asp:LinkButton
                            ID="LnkButtonDownloadpdfReal"
                            runat="server"
                            CausesValidation="false"
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Visible='<%# Eval("document_type").ToString() == "zip" ? true : false %>'
                            CommandName="OnFileDownloadPDF_Zip"
                            OnClientClick="openInNewTab();"
                            Text='<%# Eval("SUBJECT") %>'
                            CssClass="gridLink" />
                        <asp:LinkButton
                            ID="LnkButtonDownloadpdf"
                            runat="server"
                            CausesValidation="false"
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Visible='<%# (Eval("document_type").ToString() == "zip" && Eval("DOCUMENT_ID_PDF").ToString() != "0") ? true : false %>'
                            CommandName="OnFileDownloadPDF"
                            OnClientClick="openInNewTab();"
                            Text='pdf'
                            CssClass="gridLink" />
                        <asp:LinkButton
                            ID="LnkButtonDownloadNonPdf"
                            runat="server"
                            CausesValidation="false"
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                            Visible='<%# (Eval("DOCUMENT_ID").ToString() != "" && Eval("document_type").ToString() != "zip" && Eval("document_type").ToString() != "pdf") ? true : false %>'
                            CommandName="OnFileDownload"
                            OnClientClick="openInNewTab();"
                            Text='<%# Eval("SUBJECT") %>'
                            CssClass="gridLink" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CORRESPODENCE_TYPE" HeaderText="Correspondence Type" SortExpression="CORRESPODENCE_TYPE" />
                <asp:BoundField DataField="PA_NUMBER" HeaderText="Prior Authorization Number" SortExpression="PA_NUMBER" Visible="false" />
                <asp:BoundField DataField="HTN_NUMBER" HeaderText="Hospice Tracking Number" SortExpression="HTN_NUMBER" Visible="false" />
                <asp:BoundField DataField="RECIPIENT_ID" HeaderText="Recipient ID" SortExpression="RECIPIENT_ID" Visible="false" />
                <asp:BoundField DataField="DATE_SENT" HeaderText="Date Sent" SortExpression="DATE_SENT" />
                <asp:BoundField DataField="DATE_VIEWED" HeaderText="Date Viewed" SortExpression="DATE_VIEWED" />



            </Columns>
        </asp:GridView>
    </asp:Panel>


    <ajax:collapsiblepanelextender id="cpeConvertedCorrespondence" runat="server" collapsed="false" targetcontrolid="pnlConvertedCorrespondence" expandcontrolid="pnlSepConvertedCorrespondence" collapsecontrolid="pnlSepConvertedCorrespondence" />
    <asp:Panel runat="server" ID="pnlsepConvertedCorrespondence" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: cornflowerblue;" ToolTip="Click to Expand/Collapse">
        <h1><span id="sepConvertedCorrespondence" runat="server" class="pageHeader">- Converted Correspondence</span></h1>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlConvertedCorrespondence">
        <div style="width: 100%; text-align: right">
            <asp:HiddenField ID="hdnROwncont2" runat="server" />
            <telerik:radgrid id="RadGridExport1" runat="server" visible="true">
                <exportsettings ignorepaging="true" openinnewwindow="true">
                    <pdf pageheight="8.5in" pagewidth="11in" pagetitle="Converted Correspondence">
                        <pagefooter>
                            <rightcell text="Page <?page-number?>" />
                        </pagefooter>
                    </pdf>
                </exportsettings>
                <mastertableview autogeneratecolumns="false">
                    <columns>
                        <telerik:gridboundcolumn uniquename="col1" headertext="Correspondence Subject" datafield="DOCUMENT_SERVICE_NAME"></telerik:gridboundcolumn>

                    </columns>
                </mastertableview>
            </telerik:radgrid>


            <div id="divRowCount2" title="High Row Count" style="display: none; text-align: center">
                <p style="color: darkgreen" id="p1" runat="server">Only the first 10,000 results from the search will be exported.</p>
            </div>
        </div>
        <div id="LoadingPanel" style="display: none; color: red; text-align: center;">Converted Correspondence...</div>
        <mms:sortablepaginggridview
            id="gvConvertedCorrespondencesearch"
            runat="server"
            autogeneratecolumns="False"
            cssclass="gridViewSmallFont" width="100%"
            allowsorting="true"
            emptydatatext="No Converted Correspondence found."
            onrowcommand="gvConvertedCorrespondencesearch_RowCommand"
            onpageindexchanging="gvConvertedCorrespondencesearch_PageIndexChanging"
            onsorting="gvConvertedCorrespondencesearch_Sorting"
            currentsortfield="DOCUMENT_TYPE_DESC"
            currentsortdirection="DESC"
            rowstyle-verticalalign="Top"
            allowpaging="True"
            pagesize="10"
            datakeynames="DOCUMENT_TYPE_DESC,BODY,EMAIL_TO,DOCUMENT_ID,ELIGIBILITY,DATE_VIEWED">
            <columns>
                <asp:templatefield headertext="Correspondence Subject" sortexpression="DOCUMENT_TYPE_DESC">
                    <itemtemplate>
                        <asp:linkbutton id="lbtnSubject" runat="server" commandname="subject" commandargument="<%# ((GridViewRow) Container).RowIndex %>" text='<%# Eval("DOCUMENT_TYPE_DESC") %>'></asp:linkbutton>
                    </itemtemplate>
                </asp:templatefield>

            </columns>
        </mms:sortablepaginggridview>
    </asp:Panel>


    <cc2:messagebox id="MessageBox2" runat="server" />

</div>
<ajax:modalpopupextender id="mpeEmailPreview" runat="server" popupcontrolid="pnlPreview"
    targetcontrolid="ButtonDummy" backgroundcssclass="modalBackground" cancelcontrolid="btnCancel">
</ajax:modalpopupextender>
<asp:Panel ID="pnlPreview" runat="server" CssClass="modalPopup" Style="margin-right: 5% !important;">
    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
        <table style="width: 100%; cursor: pointer">
            <tr>
                <td>
                    <div class="popTitle">Provider Communication</div>
                </td>
                <td style="text-align: right">
                    <asp:ImageButton ID="imgClose" runat="server" ImageUrl="~/Images/cancel.png" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>

    <asp:UpdatePanel ID="upPreview" runat="server" UpdateMode="Conditional" style="margin-left: auto; margin-right: auto;">
        <ContentTemplate>
            <div id="divPreview" style="overflow: scroll; height: 500px; padding-right: 50px;">
                <table id="tblPreview" style="padding-top: 20px; width: 100%;">
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 20%;">
                            <%-- <asp:Label ID="lblSubject" runat="server" CssClass="formLabel300" Text="Subject" AssociatedControlID="txtSubject" /></td>--%>
                        <td>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="formField" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--                 <asp:Label ID="lblSendTo" runat="server" CssClass="formLabel300" Text="Send To" AssociatedControlID="lboxSendTo" /></td>
                            --%>
                        <td>
                            <asp:ListBox ID="lboxSendTo" runat="server" CssClass="formField" SelectionMode="Multiple" />
                            <asp:CustomValidator ID="cvSendTo" runat="server" ControlToValidate="lboxSendTo" OnServerValidate="SendToRequired" Display="Static"
                                ValidationGroup="Emails" ErrorMessage="* Select at least one Send To email address." Text="*" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%--    <asp:Label ID="lblBody" runat="server" CssClass="formLabel300" Text="Body" AssociatedControlID="txtBody" /></td>--%>
                        <td>
                            <asp:TextBox ID="txtBody" Text="txt" runat="server" CssClass="formField" ReadOnly="true" TextMode="MultiLine" Columns="100" Rows="20" />
                            <div runat="server" id="divBody"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblAttach" runat="server" CssClass="formLabel300" Text="Attachment" />
                        </td>
                        <td>
                            <asp:Panel ID="pnlAttachments" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="btnBox">
                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="buttonBoxFocus" OnClientClick="javascript:printPartOfPage('divPreview');" />
                <asp:Button ID="btnCancel" runat="server" CausesValidation="false" Text="Close" CssClass="buttonBox" OnClick="btnCancel_Click" Style="margin-right: 20px; margin-bottom: 15px !important;" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" />

<%--  GridViewSortColumn="" GridViewSortDirection="Descending"--%>