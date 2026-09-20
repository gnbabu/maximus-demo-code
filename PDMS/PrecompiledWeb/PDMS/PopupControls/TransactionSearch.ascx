<%@ control language="C#" autoeventwireup="true" inherits="UserControls_TransactionSearch, App_Web_wenzyumt" %>
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

<cc1:GroupBox ID="gbSearch"  CaptionStyle-CssClass="bodyTextBold" style="float:left;font-weight:bold;margin-top:40px;" HorizontalAlign="Center" Width="100%" runat="server"> 
    <legend class="bodyTextBold" style="font-size:14pt;font-weight:bold" tabindex="0">Search Criteria</legend>
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
                          <ajax:CalendarExtender ID="CalendarExtender1" Format="MM/dd/yyyy"
                                    TargetControlID="txtFromDate" runat="server" />
                    </div>

                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblToDate" runat="server" AssociatedControlID="txtToDate" Text="To Date" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-4 text-left">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="textEntry" autocomplete="off" />
                       <ajax:CalendarExtender ID="CalendarExtender2" Format="MM/dd/yyyy"
                                    TargetControlID="txtToDate" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" Text="Status" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-4 text-left">
                        <asp:DropDownList ID="ddlStatus" runat="server"   CssClass="DropDownList" />
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblSubscriberFails" runat="server" AssociatedControlID="ddlSubscriberFails" Text="Subscriber Fails" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-4 text-left">
                        <asp:DropDownList ID="ddlSubscriberFails" runat="server"   CssClass="DropDownList" />
                    </div>
                </div>

                        <div id="divAdditionalSearchCriteria" runat="server" visible="false"></div>
                        <div class="btnBox btnBoxCenter">
                        <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="TransactionSearch" />
                        <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" />
                        </div>
                   <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
            </div>
            <!--<div class="pg-hint3Center pg-hint3">Medicaid ID is exact match search fields.</div>-->
            
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
            <Pdf PageHeight="8.5in" PageWidth="11in" PageTitle="Transaction Search">
                <PageFooter>
                    <RightCell Text="Page <?page-number?>" />
                </PageFooter>
            </Pdf>
        </ExportSettings>
        <MasterTableView AutoGenerateColumns="false">
            <Columns>
                <telerik:GridBoundColumn UniqueName="col1" HeaderText="Transaction ID" DataField="TRANSACTION_QUEUE_ID" Visible="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col2" HeaderText="Reg ID" DataField="REG_ID"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col3" HeaderText="Medicaid ID" DataField="MEDICAID_ID"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col4" HeaderText="Submit Date" DataField="SUBMIT_DATE_TIME"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col5" HeaderText="Process Date" DataField="PROCESS_DATE_TIME"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col6" HeaderText="Transaction Type" DataField="TRANSACTION_TYPE"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col7" HeaderText="Status" DataField="Status"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col8" HeaderText="SI Message" DataField="SI_RESPONSE_MESSAGE"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col9" HeaderText="SI Ack Message" DataField="SI_ACK_RESPONSE_MESSAGE"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col10" HeaderText="MITS Ack Message" DataField="MITS_ACK_MESSAGE"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col11" HeaderText="SPBM Ack Message" DataField="SPBM_ACK_MESSAGE"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col12" HeaderText="FI Ack Message" DataField="FI_ACK_MESSAGE"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col13" HeaderText="EVV Message" DataField="EVV_ACK_MESSAGE"></telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
        <asp:Label ID="lnkRowCount" Visible="false" runat="server" style="float:left" CssClass="formLabelAuto">Row Count: <%= System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(hdnRowCount.Value.ToString(), true) %></asp:Label>
    <asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/Excel_24x24.png" alt="PDF" /></asp:LinkButton>&nbsp;&nbsp;
    <asp:LinkButton ID="lnkPDF" runat="server" ToolTip="PDF" OnClick="lnkPDF_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/PDF_24x24.png" alt="PDF" /></asp:LinkButton>
    
    <div id="divRowCounthigh" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
    </div>
</div>

<div id="LoadingPanelHigh" style="display: none; color: red; text-align: center;">Setting up workflow for provider...</div>
<br />
<mms:SortablePagingGridView
    ID="gvTransactions"
    runat="server"
    AutoGenerateColumns="False"
    CssClass="gridViewSmallFont grd-x-scroll" Width="100%"
    AllowSorting="true"
    EmptyDataText="No Providers found."
    OnRowCommand="gvTransactions_RowCommand"
    OnRowDataBound="gvTransactions_RowDataBound"
    OnPageIndexChanging="gvTransactions_PageIndexChanging"
    OnSorting="gvTransactions_Sorting"
    RowStyle-VerticalAlign="Top"
    AllowPaging="True"
    PageSize="15"
    GridViewSortColumn="ProcessedDate" GridViewSortDirection="Ascending"
    DataKeyNames="REG_ID,MEDICAID_ID,TRANSACTION_QUEUE_ID,SUBMIT_DATE_TIME,PROCESS_DATE_TIME,TRANSACTION_TYPE,Status,SI_RESPONSE_MESSAGE,SI_ACK_RESPONSE_MESSAGE,MITS_ACK_MESSAGE,SPBM_ACK_MESSAGE,FI_ACK_MESSAGE,Subscriber,SI_TRANSACTION_KEY,EVV_ACK_Message,PSM_Message,PCW_Message">
    <Columns>
        <asp:TemplateField Visible="false">
            <ItemTemplate>
                <asp:CheckBox ID="chkAssign" runat="server" />
            </ItemTemplate>
            <HeaderTemplate>
                <input id="chkAll" onclick="javascript: SelectAllCheckboxes(this);" runat="server" type="checkbox" />
            </HeaderTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="TRANSACTION_QUEUE_ID" HeaderText="Transaction ID" SortExpression="TRANSACTION_QUEUE_ID"  Visible="false" />
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
             <ItemTemplate>
                  <asp:LinkButton
                    ID="lnkResubmitTransaction"
                    runat="server"
                    CausesValidation="false"
                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                    CommandName="ResubmitTransaction"
                    Text="Resubmit"
                    CssClass="gridLink"
                    PostBackUrl="~/Process/TransactionSearch.aspx"/><!--PostBackUrl="~/Process/Registration.aspx" />-->
                <asp:LinkButton
                    ID="lnkCancelTransaction"
                    runat="server"
                    CausesValidation="false"
                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                    CommandName="CancelTransaction"
                    Text="Cancel"
                    CssClass="gridLink" 
                    PostBackUrl="~/Process/TransactionSearch.aspx"/> <!--PostBackUrl="~/Process/Registration.aspx" />-->
                <asp:Label 
                    runat="server"
                    Text =" " 
                    />
            </ItemTemplate>           
        </asp:TemplateField>
   </Columns> 

</mms:SortablePagingGridView>
<cc2:MessageBox ID="MessageBox2" runat="server" />