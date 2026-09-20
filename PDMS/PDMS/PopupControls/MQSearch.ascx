<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_MQSearch" Codebehind="MQSearch.ascx.cs" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<script type="text/javascript">
    function openNewtab(pagename) {
        window.open(window.origin + '/Process/' + pagename);
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
.Blink {
  animation: blinker 1.5s cubic-bezier(.5, 0, 1, 1) infinite alternate;  
}
@keyframes blinker {  
  from { opacity: 1; }
  to { opacity: 0; }
}
</style>

<br />

<cc1:GroupBox ID="gbSearch" Caption="Search Criteria" CaptionStyle-CssClass="bodyTextBold" Style="float: left; font-weight: bold; margin-top: 40px;" HorizontalAlign="Center" Width="100%" runat="server">
    <asp:Label runat="server" ID="lblMessages" CssClass="error-message" />
    <div>
        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="TransactionSearch" ShowSummary="true" />
    </div>
    <asp:UpdatePanel runat="server" ID="TimedPanel" UpdateMode="Conditional" EnableViewState="true">
        <ContentTemplate>
            <div runat="server" style="float: right">
                <div runat="server" class="Blink" style="display: inline;">
                    <asp:Label ID="checkHealthIconID" runat="server" Text=""></asp:Label>
                </div>
                <div runat="server" style="display: inline;">
                    <asp:Label ID="checkHealthID" runat="server" Text=""></asp:Label>
                    <asp:Label ID="checkHealthDTID" runat="server" Text=""></asp:Label>
                </div>

            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="text-align: center; margin-top: 50px;">
        <asp:Panel ID="pnlFilter" runat="server" DefaultButton="btnSearch">
            <div style="width: 100%;" class="table">
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
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="formField" style="width:90%;"  /><ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtFromDate" runat="server" />
                    </div>

                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblToDate" runat="server" AssociatedControlID="txtToDate" Text="To Date" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-4 text-left">
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="formField" style="width:90%;" /><ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="txtToDate" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblQueuename" runat="server" AssociatedControlID="ddlQueuename" Text="Queue Name" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-4 text-left">
                        <asp:DropDownList ID="ddlQueuename" runat="server" CssClass="DropDownList" />
                    </div>
                </div>
            </div>
            <!--<div class="pg-hint3Center pg-hint3">Medicaid ID is exact match search fields.</div>-->
            <div class="btnBoxCenter">
                <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="mq" />
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
    <telerik:RadGrid ID="RadGridExport" runat="server" Visible="false">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true">
            <Pdf PageHeight="8.5in" PageWidth="11in" PageTitle="MQ Search">
                <PageFooter>
                    <RightCell Text="Page <?page-number?>" />
                </PageFooter>
            </Pdf>
        </ExportSettings>
        <MasterTableView AutoGenerateColumns="false">
            <Columns>
                <telerik:GridBoundColumn UniqueName="col1" HeaderText="Transaction ID" DataField="MQ_MSG_LOG_ID" Visible="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col2" HeaderText="Reg ID" DataField="REG_ID"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col3" HeaderText="Medicaid ID" DataField="MEDICAID_ID"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col2" HeaderText="Queue" DataField="QUEUENAME"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col3" HeaderText="Date Received" DataField="RECEIVEDATE"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col4" HeaderText="Message"></telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <asp:Label ID="lnkRowCount" Visible="false" runat="server" Style="float: left" CssClass="formLabelAuto">Row Count: <%= hdnRowCount.Value.ToString() %></asp:Label>
    <asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/Excel_24x24.png" alt="PDF" /></asp:LinkButton>&nbsp;&nbsp;
    <asp:LinkButton ID="lnkPDF" runat="server" ToolTip="PDF" OnClick="lnkPDF_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/PDF_24x24.png" alt="PDF" /></asp:LinkButton>

    <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
    </div>
</div>

<div id="LoadingPanel" style="display: none; color: red; text-align: center;">Setting up workflow for provider...</div>
<br />

<mms:SortablePagingGridView
    ID="gvMQSearch"
    runat="server"
    AutoGenerateColumns="False"
    CssClass="gridViewSmallFont" Width="100%"
    AllowSorting="false"
    EmptyDataText="No MQ Search found."
    OnPageIndexChanging="gvMQSearch_PageIndexChanging"
    OnRowCommand="gvMQSearch_RowCommand"
    RowStyle-VerticalAlign="Top"
    AllowPaging="True"
    PageSize="15"
    DataKeyNames="REG_ID,MEDICAID_ID,QUEUENAME,RECEIVEDATE,MQ_MSG_LOG_ID">
    <Columns>
        <asp:BoundField DataField="MQ_MSG_LOG_ID" HeaderText="MQ ID" SortExpression="MQ_MSG_LOG_ID" Visible="false" />
        <asp:BoundField DataField="REG_ID" HeaderText="Reg ID" SortExpression="REG_ID" />
        <asp:BoundField DataField="MEDICAID_ID" HeaderText="Medicaid ID" SortExpression="MEDICAID_ID" />
        <asp:BoundField DataField="QUEUENAME" HeaderText="Queue" SortExpression="QUEUENAME" />
        <asp:BoundField DataField="RECEIVEDATE" HeaderText="Date Received" SortExpression="RECEIVEDATE" />
        <asp:TemplateField ShowHeader="true" HeaderText="MESSAGE">
            <ItemTemplate>
                <itemtemplate>
                    <asp:Literal ID="ltMessage" runat="server" Visible="false" Text='<%# HttpUtility.HtmlEncode(Eval("MESSAGE")) %>' />
                </itemtemplate>
                <asp:LinkButton
                    ID="lbtnMessage"
                    runat="server"
                    CausesValidation="false"
                    CommandArgument='<%# Container.DataItemIndex %>'
                    CommandName="showmessage"
                    Text="Transaction Received"
                    CssClass="gridLink" />

            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <%-- CommandArgument='<%# Eval("MQ_MSG_LOG_ID") %>'--%>
    <%-- <PagerStyle cssClass="gridViewPager" HorizontalAlign="Right" />  --%>
</mms:SortablePagingGridView>

<cc2:MessageBox ID="MessageBox2" runat="server" />
