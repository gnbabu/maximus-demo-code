<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_TransactionSearch" Codebehind="TransactionSearch.ascx.cs" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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
            $("#divRowCounthigh").dialog();
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
    $(document).ready(function () {
        $('#ctl00_MainContent_ucTransactionSearch_txtRegId').keyup(function (e) {
            if (/\D/g.test(this.value)) {
                this.value = this.value.replace(/\D/g, '');
            }
        });
    });
</script>
<style type="text/css">
    select {
        min-width: 90%;
    }

    .grd-x-scroll {
        overflow-x: auto;
        display: block;
        white-space: nowrap;
    }
</style>

<cc1:groupbox id="gbSearch" captionstyle-cssclass="bodyTextBold" style="float: left; font-weight: bold; margin-top: 40px;" horizontalalign="Center" width="100%" runat="server">
    <legend class="bodyTextBold" style="font-size: 14pt; font-weight: bold" tabindex="0">Search Criteria</legend>
    <asp:Label runat="server" ID="lblMessages" CssClass="error-message" />
    <div>
        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="TransactionSearch" ShowSummary="true" />
    </div>
    <div style="text-align: center;">
        <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">
            <div style="width: 100%;" class="table">

                <%-- <asp:UpdatePanel ID="pnlUpdate" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblRegId" runat="server" AssociatedControlID="txtRegId" Text="Reg ID" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-4 text-left">
                        <asp:TextBox ID="txtRegId" runat="server" CssClass="textEntry" MaxLength="30" />
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblMedicaidId" runat="server" AssociatedControlID="txtMedicaidId" Text="Medicaid ID" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-4 text-left">
                        <asp:TextBox ID="txtMedicaidId" runat="server" MaxLength="9" CssClass="textEntry" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblFromDate" runat="server" AssociatedControlID="txtFromDate" Text="From Date" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-4 text-left">
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="textEntry" autocomplete="off" />
                        <ajax:calendarextender id="CalendarExtender1" format="MM/dd/yyyy"
                            targetcontrolid="txtFromDate" runat="server" />
                    </div>

                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblToDate" runat="server" AssociatedControlID="txtToDate" Text="To Date" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-4 text-left">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="textEntry" autocomplete="off" />
                        <ajax:calendarextender id="CalendarExtender2" format="MM/dd/yyyy"
                            targetcontrolid="txtToDate" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" Text="Status" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-4 text-left">
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="DropDownList" />
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblSubscriberFails" runat="server" AssociatedControlID="ddlSubscriberFails" Text="Subscriber Fails" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-4 text-left">
                        <asp:DropDownList ID="ddlSubscriberFails" runat="server" CssClass="DropDownList" />
                    </div>
                </div>

                <div id="divAdditionalSearchCriteria" runat="server" visible="false"></div>
                <br />
                <div class="btnBoxCenter">
                    <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="TransactionSearch" />
                    <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" />
                </div>
                <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
            <!--<div class="pg-hint3Center pg-hint3">Medicaid ID is exact match search fields.</div>-->

        </asp:Panel>
    </div>
</cc1:groupbox>
<br />

<asp:Panel ID="pnlResultHeader" runat="server" CssClass="ResultHeader">
    <asp:Label ID="lblResultHeader" runat="server" />
</asp:Panel>

<div style="width: 100%; text-align: right">

    <asp:HiddenField ID="hdnRowCount" runat="server" />
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
                <telerik:gridboundcolumn uniquename="col1" headertext="Transaction ID" datafield="TRANSACTION_QUEUE_ID" visible="false"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col2" headertext="Reg ID" datafield="REG_ID"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col3" headertext="Medicaid ID" datafield="MEDICAID_ID"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col4" headertext="Submit Date" datafield="SUBMIT_DATE_TIME"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col5" headertext="Process Date" datafield="PROCESS_DATE_TIME"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col6" headertext="Transaction Type" datafield="TRANSACTION_TYPE"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col7" headertext="Status" datafield="Status"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col8" headertext="SI Message" datafield="SI_RESPONSE_MESSAGE"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col9" headertext="SI Ack Message" datafield="SI_ACK_RESPONSE_MESSAGE"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col10" headertext="SI Key" datafield="SI_TRANSACTION_KEY"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col11" headertext="MITS Ack Message" datafield="MITS_ACK_MESSAGE"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col12" headertext="SPBM Ack Message" datafield="SPBM_ACK_MESSAGE"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col13" headertext="FI Ack Message" datafield="FI_ACK_MESSAGE"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col14" headertext="EVV Message" datafield="EVV_ACK_MESSAGE"></telerik:gridboundcolumn>
            </columns>
        </mastertableview>
    </telerik:radgrid>
    <asp:Label ID="lnkRowCount" Visible="false" runat="server" Style="float: left" CssClass="formLabelAuto">Row Count: <%= System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(hdnRowCount.Value.ToString(), true) %></asp:Label>
    <asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/Excel_24x24.png" alt="PDF" /></asp:LinkButton>&nbsp;&nbsp;
    <asp:LinkButton ID="lnkPDF" runat="server" ToolTip="PDF" OnClick="lnkPDF_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/PDF_24x24.png" alt="PDF" /></asp:LinkButton>

    <div id="divRowCounthigh" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
    </div>
</div>

<div id="LoadingPanelHigh" style="display: none; color: red; text-align: center;">Setting up workflow for provider...</div>
<br />
<mms:sortablepaginggridview
    id="gvTransactions"
    runat="server"
    autogeneratecolumns="False"
    cssclass="gridViewSmallFont grd-x-scroll" width="100%"
    allowsorting="true"
    emptydatatext="No Providers found."
    onrowcommand="gvTransactions_RowCommand"
    onrowdatabound="gvTransactions_RowDataBound"
    onpageindexchanging="gvTransactions_PageIndexChanging"
    onsorting="gvTransactions_Sorting"
    rowstyle-verticalalign="Top"
    allowpaging="True"
    pagesize="15"
    gridviewsortcolumn="ProcessedDate" gridviewsortdirection="Ascending"
    datakeynames="REG_ID,MEDICAID_ID,TRANSACTION_QUEUE_ID,SUBMIT_DATE_TIME,PROCESS_DATE_TIME,TRANSACTION_TYPE,Status,SI_RESPONSE_MESSAGE,SI_ACK_RESPONSE_MESSAGE,MITS_ACK_MESSAGE,SPBM_ACK_MESSAGE,FI_ACK_MESSAGE,Subscriber,SI_TRANSACTION_KEY,EVV_ACK_Message,PSM_Message,PCW_Message">
    <columns>
        <asp:TemplateField Visible="false">
            <itemtemplate>
                <asp:CheckBox ID="chkAssign" runat="server" />
            </itemtemplate>
            <headertemplate>
                <input id="chkAll" onclick="javascript: SelectAllCheckboxes(this);" runat="server" type="checkbox" />
            </headertemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="TRANSACTION_QUEUE_ID" HeaderText="Transaction ID" SortExpression="TRANSACTION_QUEUE_ID" Visible="false" />
        <asp:BoundField DataField="REG_ID" HeaderText="Reg ID" SortExpression="REG_ID" />
        <asp:BoundField DataField="MEDICAID_ID" HeaderText="Medicaid ID" SortExpression="MEDICAID_ID" />
        <asp:BoundField DataField="TRANSACTION_TYPE" HeaderText="Transaction Type" SortExpression="TRANSACTION_TYPE" />
        <asp:BoundField DataField="SUBMIT_DATE_TIME" HeaderText="Submit Date" SortExpression="SUBMIT_DATE_TIME" HtmlEncode="false" />
        <asp:BoundField DataField="PROCESS_DATE_TIME" HeaderText="Process Date" SortExpression="PROCESS_DATE_TIME" HtmlEncode="false" />
        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
        <asp:BoundField DataField="SI_RESPONSE_MESSAGE" HeaderText="SI Message" SortExpression="SI_RESPONSE_MESSAGE" HtmlEncode="false" />
        <asp:BoundField DataField="SI_ACK_RESPONSE_MESSAGE" HeaderText="SI Ack Message" SortExpression="SI_ACK_RESPONSE_MESSAGE" HtmlEncode="false" />
        <asp:BoundField DataField="MITS_ACK_MESSAGE" HeaderText="MITS Ack Message" SortExpression="MITS_ACK_MESSAGE" HtmlEncode="false" />
        <asp:BoundField DataField="SPBM_ACK_MESSAGE" HeaderText="SPBM Ack Message" SortExpression="SPBM_ACK_MESSAGE" HtmlEncode="false" />
        <asp:BoundField DataField="FI_ACK_MESSAGE" HeaderText="FI Ack Message" SortExpression="FI_ACK_MESSAGE" HtmlEncode="false" />
        <asp:BoundField DataField="EVV_ACK_Message" HeaderText="EVV" SortExpression="EVV_ACK_Message" HtmlEncode="false" />
        <asp:BoundField DataField="PSM_Message" HeaderText="PSM" SortExpression="PSM_Message" HtmlEncode="false" />
        <asp:BoundField DataField="PCW_Message" HeaderText="PCW" SortExpression="PCW_Message" HtmlEncode="false" />
        <asp:BoundField DataField="SI_TRANSACTION_KEY" HeaderText="SI KEY" SortExpression="SI_TRANSACTION_KEY" HtmlEncode="false" />
        <asp:TemplateField ShowHeader="False" HeaderText="Transaction Action">
            <itemtemplate>
                <asp:LinkButton
                    ID="lnkResubmitTransaction"
                    runat="server"
                    CausesValidation="false"
                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                    CommandName="ResubmitTransaction"
                    Text="Resubmit"
                    CssClass="gridLink"
                    PostBackUrl="~/Process/TransactionSearch.aspx" />
                <!--PostBackUrl="~/Process/Registration.aspx" />-->
                <asp:LinkButton
                    ID="lnkCancelTransaction"
                    runat="server"
                    CausesValidation="false"
                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                    CommandName="CancelTransaction"
                    Text="Cancel"
                    CssClass="gridLink"
                    PostBackUrl="~/Process/TransactionSearch.aspx" />
                <!--PostBackUrl="~/Process/Registration.aspx" />-->
                <asp:Label
                    runat="server"
                    Text=" " />
            </itemtemplate>
        </asp:TemplateField>
    </columns>

</mms:sortablepaginggridview>
<cc2:messagebox id="MessageBox2" runat="server" />
