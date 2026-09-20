<%@ control language="C#" autoeventwireup="true" inherits="UserControls_TransactionMonitor, App_Web_wenzyumt" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">
    .rgEdit {
        width: 15px;
        height: 15px;
        display: inline-block;
        text-indent: -17px !important;
    }
</style>

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

    function addscope(sender, args) {
        var gridTable = sender.get_masterTableView().get_element();
        var rows = gridTable.getElementsByTagName("tr");
        for (var i = 0; i < rows.length; i++) {
            $(rows[i]).attr("scope", "col");
        }
        var cells = gridTable.getElementsByTagName("td");
        for (var i = 0; i < cells.length; i++) {
            $(cells[i]).attr("scope", "col");
        }
        var headers = gridTable.getElementsByTagName("th");
        for (var i = 0; i < headers.length; i++) {
            $(headers[i]).attr("scope", "col");
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

    .identModalPopup {
        width: 520px !important;
    }

    .RadGrid .rgFilter {
        width: 13px;
        height: 13px;
        margin: 0 0 0 2px;
    }
</style>
<div>


    <div>


        <cc1:groupbox id="gbSearch" captionstyle-cssclass="bodyTextBold" style="float: left; font-weight: bold; margin-top: 40px;" horizontalalign="Center" width="60%" runat="server">
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
                        <asp:TextBox ID="txtCurrentTransactionID" runat="server" CssClass="textEntry" MaxLength="30" Style="display: none" />
                        <asp:TextBox ID="txtCurrenntAssignedStatus" runat="server" CssClass="textEntry" MaxLength="30" Style="display: none" />

                        <div class="row">
                            <div class="col-sm-2 text-right">
                                <span class="formLabel150">
                                    <asp:Label ID="lblRegId" runat="server" AssociatedControlID="txtRegId" Text="Reg ID" /></span>&nbsp;&nbsp;
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:TextBox ID="txtRegId" runat="server" CssClass="textEntry" TextMode="Number" Attributes="maxlength=10" />
                                <asp:RegularExpressionValidator ID="revtxtRegID" runat="server" ControlToValidate="txtRegId"
                                    ValidationExpression="\d{0,10}" ErrorMessage="* Enter a numeric Reg ID Number with max 10 digits."
                                    Enabled="true" SetFocusOnError="true"
                                    ValidationGroup="TransactionSearch" Display="Dynamic" />
                            </div>
                            <div class="col-sm-2 text-right">
                                <span class="formLabel150">
                                    <asp:Label ID="lblMedicaidId" runat="server" AssociatedControlID="txtMedicaidId" Text="Medicaid ID" /></span>&nbsp;&nbsp;
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:TextBox ID="txtMedicaidId" runat="server" MaxLength="9" CssClass="textEntry" />
                            </div>
                        </div>
                        <div class="row" style="margin-top: 1px">
                            <div class="col-sm-2 text-right">
                                <span class="formLabel150">
                                    <asp:Label ID="lblFromDate" runat="server" AssociatedControlID="txtFromDate" Text="Start Date" /></span>&nbsp;&nbsp;
                            </div>
                            <div class="col-sm-4 text-left">
                                <span class="input-group">
                                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="ohio-field-input" autocomplete="off" />
                                    <span class="input-group-addon">
                                        <span aria-hidden="true">
                                            <asp:Image ID="Image31" Height="22" Width="22" runat="server" AlternateText="calender" ImageUrl="~/Images/Calendaricon.png" />
                                        </span>
                                    </span>
                                    <ajax:calendarextender id="CalendarExtender1" format="MM/dd/yyyy"
                                        targetcontrolid="txtFromDate" runat="server" />

                                    <asp:CompareValidator
                                        ID="cvtxtDateReceived"
                                        runat="server"
                                        Type="Date"
                                        Operator="DataTypeCheck"
                                        ControlToValidate="txtFromDate"
                                        ErrorMessage="Select a valid Start Date"
                                        Display="Dynamic"
                                        ValueToCompare="MM/dd/yyyy"
                                        ValidationGroup="TransactionSearch"
                                        SetFocusOnError="true"> 
                                    </asp:CompareValidator>
                                </span>
                            </div>

                            <div class="col-sm-2 text-right">
                                <span class="formLabel150">
                                    <asp:Label ID="lblToDate" runat="server" AssociatedControlID="txtToDate" Text="End Date" /></span>&nbsp;&nbsp;
                            </div>
                            <div class="col-sm-4 text-left">
                                <span class="input-group">
                                    <asp:TextBox ID="txtToDate" runat="server" CssClass="ohio-field-input" autocomplete="off" />
                                    <span class="input-group-addon">
                                        <span aria-hidden="true">
                                            <asp:Image ID="Image1" Height="22" Width="22" runat="server" AlternateText="calender" ImageUrl="~/Images/Calendaricon.png" />
                                        </span>
                                    </span>
                                    <ajax:calendarextender id="CalendarExtender2" format="MM/dd/yyyy"
                                        targetcontrolid="txtToDate" runat="server" />

                                    <asp:CompareValidator
                                        ID="CompareValidator1"
                                        runat="server"
                                        Type="Date"
                                        Operator="DataTypeCheck"
                                        ControlToValidate="txtToDate"
                                        ErrorMessage="Select a valid END Date"
                                        Display="Dynamic"
                                        ValueToCompare="MM/dd/yyyy"
                                        ValidationGroup="TransactionSearch"
                                        SetFocusOnError="true"> 
                                    </asp:CompareValidator>

                                </span>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 1px">

                            <div class="col-sm-2 text-right">
                                <span class="formLabel150">
                                    <asp:label id="lblDisplayHistoricNotes" runat="server" associatedcontrolid="chkHistoricNotes" text="Display Historic Notes" />
                                </span>&nbsp;&nbsp;
                            </div>
                            <div class="col-sm-4 text-left">
                                <asp:checkbox id="chkHistoricNotes" runat="server" />
                            </div>
                            <div class="col-md-4 text-right">
                                <span class="formLabel150">
                                    <asp:label id="lblNWFFailures" runat="server" associatedcontrolid="chkNWFFailures" text="Display Non-WF Failures Only" />
                                </span>&nbsp;&nbsp;
                            </div>
                            <div class="col-md-1 text-right">
                                <asp:checkbox id="chkNWFFailures" runat="server" />
                            </div>
                        </div>


                        <div id="divAdditionalSearchCriteria" runat="server" visible="false"></div>
                        <div class="btnBox btnBoxCenter">
                            <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch1_Click" ValidationGroup="TransactionSearch" />
                            <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBox" OnClick="btnClear1_Click" />
                        </div>
                        <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
                    </div>
                    <!--<div class="pg-hint3Center pg-hint3">Medicaid ID is exact match search fields.</div>-->
                    <div class="row"></div>
                    <legend class="bodyTextBold" style="font-size: 14pt; font-weight: bold" tabindex="0">Resend Transaction</legend>
                    <div class="row" style="display:none">
                        <div class="col-sm-2 text-right">
                            <span class="formLabel150">
                                <asp:Label ID="Label9" runat="server" AssociatedControlID="txtRegId2" Text="Reg ID" /></span>&nbsp;&nbsp;
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:TextBox ID="txtRegId2" runat="server" CssClass="textEntry" MaxLength="10" />
                            <asp:RegularExpressionValidator ID="txtRegId2Validator" runat="server" ControlToValidate="txtRegId2"
                                ValidationExpression="\d{0,10}" ErrorMessage="* Enter a numeric Reg ID Number with max 10 digits."
                                Enabled="true" SetFocusOnError="true"
                                ValidationGroup="TransactionSend" Display="Dynamic" />
                        </div>

                        <div class="col-sm-2 text-right">
                            <span class="formLabel150">
                                <asp:Label ID="Label10" runat="server" AssociatedControlID="ddlTransactionType" Text="Transaction type" /></span>&nbsp;&nbsp;
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="textEntry">
                                <asp:ListItem Text="MITS Update" Value="1" />
                                <asp:ListItem Text="MITS Enroll" Value="2" />
                                <asp:ListItem Text="PSM Partial" Value="3" />
                                <asp:ListItem Text="PCW Partial" Value="4" />
                                <asp:ListItem Text="PSM Full" Value="5" />
                                <asp:ListItem Text="PCW Full" Value="6" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="btnBox btnBoxCenter">
                        <asp:Button ID="btnTransactionSend" runat="server" CausesValidation="true" Text="Resend" OnClick="btnTransactionSend_Click" CssClass="buttonBoxFocus" ValidationGroup="TransactionSend" />
                    </div>
                </asp:Panel>
            </div>
        </cc1:groupbox>

        <cc1:groupbox id="GroupBox1" captionstyle-cssclass="bodyTextBold" style="float: left; font-weight: bold; margin-top: 40px;" horizontalalign="Center" width="30%" runat="server">
            <legend class="bodyTextBold" style="font-size: 14pt; font-weight: bold" tabindex="0">Status & Stats</legend>

            <div style="text-align: center;">
                <asp:Panel ID="Panel1" runat="server" DefaultButton="btnSearch">
                    <div class="row">
                        <div class="col-sm-2 text-right">
                            <span class="formLabel150">
                                <asp:Label ID="lblcurrentTMTotal" runat="server" Text="Current TM Total" /></span>&nbsp;&nbsp;
                        </div>
                        <div class="col-sm-2 text-right">
                            <span class="formLabel150">
                                <asp:Label ID="lblcurrentTMTotalCount" runat="server" Text="" /></span>&nbsp;&nbsp;
                        </div>
                     </div>
                        <%--<div class="col-sm-4 text-right">
                    </div>--%>
<%--                    <div class="row">
                        <div class="col-sm-4 text-right">
                            <span class="formLabel150">
                                <asp:Label ID="Label1" runat="server" Text="Current Stats by Status" visible="false" /></span>&nbsp;&nbsp;
                        </div>
                        <div class="col-sm-4 text-right"></div>
                    </div>--%>
                    <div class="row">
                        <div class="col-sm-2 text-right">
                            <span class="formLabel150">
                                <asp:Label ID="lblPreviousWeekTotal" runat="server" Text="Previous Week Total" /></span>&nbsp;&nbsp;
                        </div>

                        <div class="col-sm-2 text-right">
                            <span class="formLabel150">
                                <asp:Label ID="lblPreviousWeekTotalCount" runat="server" Text="" /></span>&nbsp;&nbsp;
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2 text-right">
                            <span class="formLabel150">
                                <asp:Label ID="lblPreviousMonthTotal" runat="server" Text="Previous Month Total" /></span>&nbsp;&nbsp;
                        </div>

                        <div class="col-sm-2 text-right">
                            <span class="formLabel150">
                                <asp:Label ID="lblPreviousMonthTotalCount" runat="server" Text="" /></span>&nbsp;&nbsp;
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-2 text-right">
                            <span class="formLabel150">
                                <asp:Label ID="lblAssignedStatus" runat="server" Text="Assigned Status" visible="false" /></span>&nbsp;&nbsp;
                        </div>
                        <div class="col-sm-2 text-right">
                            <span class="formLabel150">
                                <asp:Label ID="lblCount" runat="server" Text="Count" visible="false" /></span>&nbsp;&nbsp;
                        </div>
                    </div>
                    
                    <div class="col-sm-4 text-right">
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="Label4" runat="server" Text="DODD" visible="false" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblDODDCount" runat="server" Text="" visible="false" /></span>&nbsp;&nbsp;
                    </div>

                    <div class="col-sm-2 text-right">
                    </div>

                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-4 text-right">
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="Label5" runat="server" Text="Fix in UI/Runbook" visible="false" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblRunBookCount" runat="server" Text="" visible="false" /></span>&nbsp;&nbsp;
                    </div>

                    <div class="col-sm-2 text-right">
                    </div>

                    <div class="col-sm-2 text-right"></div>

                    <div class="col-sm-4 text-right">
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="Label8" runat="server" Text="Jira Ticket" visible="false" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblJiraCount" runat="server" Text="" visible="false" /></span>&nbsp;&nbsp;
                    </div>

                    <div class="col-sm-2 text-right">
                    </div>

                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-4 text-right">
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="Label3" runat="server" Text="ODA" visible="false" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblODACount" runat="server" Text="" visible="false" /></span>&nbsp;&nbsp;
                    </div>

                    <div class="col-sm-2 text-right">
                    </div>

                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-4 text-right">
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="Label6" runat="server" Text="ODM Other" visible="false" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblODMOtherCount" runat="server" Text="" visible="false"/></span>&nbsp;&nbsp;
                    </div>

                    <div class="col-sm-2 text-right">
                    </div>

                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-4 text-right">
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="Label11" runat="server" Text="ODM to RTP" visible="false" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblODMtoRTPCount" runat="server" Text="" visible="false" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-2 text-right">
                    </div>

                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-4 text-right">
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="Label7" runat="server" Text="Other" visible="false" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblOtherCount" runat="server" Text="" visible="false" /></span>&nbsp;&nbsp;
                    </div>

                    <div class="col-sm-2 text-right">
                    </div>

                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-4 text-right">
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="Label14" runat="server" Text="Triage" visible="false" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblTriageCount" runat="server" Text="" visible="false" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-2 text-right">
                    </div>

                    <div class="col-sm-2 text-right">
                    </div>
                    <div class="col-sm-4 text-right">
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="Label2" runat="server" Text="Unassigned" visible="false" /></span>&nbsp;&nbsp;
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">
                            <asp:Label ID="lblUnassignedCount" runat="server" Text="" visible="false" /></span>&nbsp;&nbsp;
                    </div>
                </asp:Panel>
            </div>
        </cc1:groupbox>
    </div>

</div>



<br />

<asp:Panel ID="pnlResultHeader" runat="server" CssClass="ResultHeader">
    <asp:Label ID="lblResultHeader" runat="server" />
</asp:Panel>

<div style="width: 100%; text-align: right">

    <asp:HiddenField ID="hdnRowCount" runat="server" />
    <telerik:radgrid id="RadGridExport" runat="server" visible="true" rendermode="Lightweight">
        <exportsettings ignorepaging="true" openinnewwindow="true">
            <pdf pageheight="8.5in" pagewidth="11in" pagetitle="Transaction Search">
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
                <telerik:gridboundcolumn uniquename="col4" headertext="Transaction Type" datafield="TRANSACTION_TYPE"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col5" headertext="Assigned To" datafield="AssignedTo"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col6" headertext="Submit Date" datafield="SUBMIT_DATE_TIME"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col7" headertext="SI Message" datafield="SI_RESPONSE_MESSAGE"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col7" headertext="SI Ack Message" datafield="SI_ACK_RESPONSE_MESSAGE"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col8" headertext="MITS Message" datafield="MITS_ACK_MESSAGE"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col9" headertext="FI Message" datafield="FI_ACK_MESSAGE"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col10" headertext="SPBM Message" datafield="SPBM_ACK_MESSAGE"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col11" headertext="EVV Message" datafield="EVV_ACK_MESSAGE"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col12" headertext="PSM Message" datafield="PSM_Message"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col13" headertext="PCW Message" datafield="PCW_Message"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col14" headertext="SI KEY" datafield="SI_TRANSACTION_KEY"></telerik:gridboundcolumn>
                <telerik:gridboundcolumn uniquename="col15" headertext="Historic TM Notes" datafield="HistoricNotes"></telerik:gridboundcolumn>
            </columns>
        </mastertableview>
    </telerik:radgrid>


    <div id="divRowCounthigh" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
    </div>
</div>

<div id="LoadingPanelHigh" style="display: none; color: red; text-align: center;">Setting up workflow for provider...</div>
<br />
<table style="width: 100%">
    <tr>

        <td>
            <asp:Label ID="lnkRowCount" Visible="false" runat="server" Style="float: left" CssClass="formLabelAuto">Count: <%= hdnRowCount.Value.ToString() %></asp:Label>
        </td>
        <td></td>

        <td>
            <div style="text-align: right; padding-right: 30px">
                <asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/Excel_24x24.png" alt="PDF" /></asp:LinkButton>
            </div>
            <br />
            <br />
        </td>
    </tr>
</table>

<telerik:radgrid
    id="gvTransactions"
    runat="server"
    allowpaging="True"
    enableariasupport="true"
    allowsorting="true"
    allowfilteringbycolumn="true"
    onitemcommand="gvTransactions_RowCommand"
    onitemdatabound="gvTransactions_RowDataBound"
    allowmultirowselection="true"
    onpageindexchanged="gvTransactions_PageIndexChanging"
    skin="PDMSModern"
    onsortcommand="gvTransactions_Sorting"
    OnNeedDataSource ="gvTransactions_NeedDataSource"
    style="width: 100%; border-style: none" cssclass="gridViewSmallFont grd-x-scroll">
    <GroupingSettings CaseSensitive="false" />
    <mastertableview autogeneratecolumns="False" allowsorting="true" allowpaging="true" PageSize="10" TableLayout="Auto" EnableHeaderContextMenu="true"
        datakeynames="REG_ID,MEDICAID_ID,TRANSACTION_QUEUE_ID,SUBMIT_DATE_TIME,TRANSACTION_TYPE,Status,SI_RESPONSE_MESSAGE, SI_ACK_RESPONSE_MESSAGE,MITS_ACK_MESSAGE,SPBM_ACK_MESSAGE,EVV_ACK_MESSAGE,FI_ACK_MESSAGE,Subscriber,SI_TRANSACTION_KEY,AssignedStatus,Process_ID,PSM_Message,PCW_Message,AssignedTo,Action_Name">
        <columns>
               <telerik:gridtemplatecolumn Visible="true" allowfiltering="false">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkAssign" runat="server" />
                        </ItemTemplate>
                        <HeaderTemplate>
                            <input id="chkAll" title="Select All" onclick="javascript: SelectAllCheckboxes(this);" runat="server" type="checkbox" />
                        </HeaderTemplate>
                    </telerik:gridtemplatecolumn>

       <%-- <telerik:gridclientselectcolumn uniquename="ClientSelectColumn">
            
          </telerik:gridclientselectcolumn>--%>
           <telerik:gridboundcolumn datafield="TRANSACTION_QUEUE_ID" headertext="Transaction ID" sortexpression="TRANSACTION_QUEUE_ID" visible="false" />
            <telerik:gridboundcolumn datafield="Process_ID" headertext="Process_ID" sortexpression="Process_ID" visible="false" />
            <telerik:gridboundcolumn datafield="Action_Name" headertext="Action_Name" sortexpression="Action_Name" visible="false" />

            <telerik:gridtemplatecolumn headertext="Reg ID" allowfiltering="false">
                <itemtemplate>
                    <asp:LinkButton
                        ID="lnkREGID"
                        runat="server"
                        CausesValidation="false"
                        CommandArgument='<%# ((GridItem)Container).RowIndex %>'
                        CommandName="RouteToRegScreen"
                        Text='<%# Eval("REG_ID") %>'
                        CssClass="gridLink"
                        PostBackUrl="~/Process/TransactionSearch.aspx" />
                    <!--PostBackUrl="~/Process/Registration.aspx" />-->
                </itemtemplate>
            </telerik:gridtemplatecolumn>


            <telerik:gridboundcolumn datafield="MEDICAID_ID" headertext="Medicaid ID" visible="true" sortexpression="MEDICAID_ID" allowfiltering="false" />
            <telerik:gridboundcolumn datafield="TRANSACTION_TYPE" headertext="Transaction Type" sortexpression="TRANSACTION_TYPE" showfiltericon="true" visible="true" />
            <telerik:gridboundcolumn datafield="AssignedTo" headertext="Assigned To" sortexpression="AssignedTo" visible="true" />
            <telerik:gridboundcolumn datafield="SUBMIT_DATE_TIME" headertext="Submit Date" sortexpression="SUBMIT_DATE_TIME" visible="true" />
            <telerik:gridboundcolumn datafield="SI_RESPONSE_MESSAGE" headertext="SI Message" sortexpression="SI_RESPONSE_MESSAGE" visible="true" />
            <telerik:gridboundcolumn datafield="SI_ACK_RESPONSE_MESSAGE" headertext="SI Ack Message" sortexpression="SI_ACK_RESPONSE_MESSAGE" visible="true" />
            <telerik:gridboundcolumn datafield="MITS_ACK_MESSAGE" headertext="MITS Message" sortexpression="MITS_ACK_MESSAGE" visible="true" />
            <telerik:gridboundcolumn datafield="FI_ACK_MESSAGE" headertext="FI Message" sortexpression="FI_ACK_MESSAGE" visible="true" />
            <telerik:gridboundcolumn datafield="SPBM_ACK_MESSAGE" headertext="SPBM Message" sortexpression="SPBM_ACK_MESSAGE" visible="true" />
            <telerik:gridboundcolumn datafield="EVV_ACK_MESSAGE" headertext="EVV Message" sortexpression="EVV_ACK_MESSAGE" visible="true" />
            <telerik:gridboundcolumn datafield="PSM_Message" headertext="PSM Message" sortexpression="PSM_Message" visible="true" />
            <telerik:gridboundcolumn datafield="PCW_Message" headertext="PCW Message" sortexpression="PCW_Message" visible="true" />
            <telerik:gridboundcolumn datafield="SI_TRANSACTION_KEY" headertext="SI Key" sortexpression="SI_TRANSACTION_KEY" visible="true" allowfiltering="false" />

            <telerik:gridtemplatecolumn headertext="Resend" allowfiltering="false">
                <itemtemplate>
                    <asp:LinkButton
                        ID="lnkResend"
                        runat="server"
                        CausesValidation="false"
                        CommandArgument='<%# ((GridItem)Container).RowIndex %>'
                        CommandName="ResendTransaction"
                        Text="Resend"
                        CssClass="gridLink"
                        OnClick="lnkResend_Click"
                        PostBackUrl="~/Process/TransactionSearch.aspx" />
                    <!--PostBackUrl="~/Process/Registration.aspx" />-->
                </itemtemplate>
            </telerik:gridtemplatecolumn>

            <telerik:gridtemplatecolumn headertext="Return to Review" allowfiltering="false">
                <itemtemplate>
                    <asp:LinkButton
                        ID="lnkRTR"
                        runat="server"
                        CausesValidation="false"
                        CommandArgument='<%# ((GridItem)Container).RowIndex %>'
                        CommandName="RTRTransaction"
                        Text="Return to Review"
                        CssClass="gridLink"
                        PostBackUrl="~/Process/TransactionSearch.aspx" />
                    <!--PostBackUrl="~/Process/Registration.aspx" />-->
                </itemtemplate>
            </telerik:gridtemplatecolumn>

            <telerik:gridtemplatecolumn headertext="Assigned Status" allowfiltering="false">
                <itemtemplate>
                    <asp:LinkButton
                        ID="UpdateAssignedStatus"
                        runat="server"
                        CausesValidation="false"
                        CommandArgument='<%# ((GridItem)Container).RowIndex %>'
                        CommandName="UpdateAssignedStatus"
                        Text='<%# Eval("AssignedStatus") %>'
                        CssClass="gridLink"
                        OnClick="lnkUpdateAssignedStatus_Click"
                        PostBackUrl="~/Process/TransactionSearch.aspx" />
                    <!--PostBackUrl="~/Process/Registration.aspx" />-->
                </itemtemplate>
            </telerik:gridtemplatecolumn>

            <telerik:gridtemplatecolumn headertext="New Note">
                <itemtemplate>
                    <asp:LinkButton
                        ID="lnkAddNewNote"
                        runat="server"
                        CausesValidation="false"
                        CommandArgument='<%# ((GridItem)Container).RowIndex %>'
                        CommandName="AddNewNote"
                        Text="Add Note"
                        CssClass="gridLink"
                        OnClick="lnkAddNewNote_Click"
                        PostBackUrl="~/Process/TransactionSearch.aspx" />
                    <!--PostBackUrl="~/Process/Registration.aspx" />-->
                </itemtemplate>
            </telerik:gridtemplatecolumn>

            <telerik:gridboundcolumn datafield="HistoricNotes" headertext="Historic TM Notes" sortexpression="HistoricNotes" htmlencode="false" />

        </columns>
        <pagerstyle mode="NextPrevAndNumeric" alwaysvisible="true" pagesizelabeltext="Page Size: " pagesizes="10,20,50,100,500,1000,10000" />
    </mastertableview>
    <clientsettings>
        <scrolling allowscroll="True" usestaticheaders="True" scrollheight="500px" />
        <selecting allowrowselect="false"></selecting>
        <ClientEvents OnFilterMenuShowing="filterMenuShowing" OnGridCreated="addscope" />
     </clientsettings>
    <filtermenu onclientshowing="MenuShowing" cssclass="gridviewFilter" />

</telerik:radgrid>


<div aria-live="assertive">
    <ajax:modalpopupextender id="actionModalController" runat="server" popupcontrolid="actionModal" targetcontrolid="ButtonDummy"
        backgroundcssclass="identModalBackground" cancelcontrolid="btnModalCancel" popupdraghandlecontrolid="actionModalHeader">
    </ajax:modalpopupextender>
    <asp:Panel ID="actionModal" runat="server" CssClass="identModalPopup" Style="display: none; height: 300px; width: 1000px">
        <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
            <div class="popTitle">
                <div>
                    <span class="formLabel150">
                        <asp:Label ID="ModalHeader" runat="server" Text="" /></span>&nbsp;&nbsp;
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="plnTextArea" runat="server" Style="padding: 10px">
            <div>
                <asp:TextBox ID="notesTxtBoxID" runat="server" aria-Label="Notes" ValidationGroup="NotesGroup" TextMode="MultiLine" Style="height: 160px" Rows="4" Cols="10" CssClass="formFieldLarge" />
                <div>
                    <asp:RequiredFieldValidator ID="vatNotesVld" runat="server" ControlToValidate="notesTxtBoxID"
                        Enabled="true" SetFocusOnError="true" Display="Dynamic"
                        ValidationGroup="NotesGroup" ErrorMessage="* Notes is required."></asp:RequiredFieldValidator>
                </div>

            </div>

            <div id="assignedStausSection">
                <div>
                    <span class="formLabel150">
                        <asp:Label ID="assignedStatusLableID" runat="server" AssociatedControlID="assignedStatusDropdownId" Text="Assigned Status" /></span>&nbsp;&nbsp;
                </div>
                <div>
                    <asp:DropDownList runat="server" ID="assignedStatusDropdownId">
                        <asp:ListItem Text="Unassigned" Value="Unassigned"></asp:ListItem>
                        <asp:ListItem Text="DODD" Value="DODD"></asp:ListItem>
                        <asp:ListItem Text="FIX IN UI/RUNBOOK" Value="FIX IN UI/RUNBOOK"></asp:ListItem>
                        <asp:ListItem Text="JIRA TICKET" Value="JIRA TICKET"></asp:ListItem>
                        <asp:ListItem Text="ODA" Value="ODA"></asp:ListItem>
                        <asp:ListItem Text="ODM OTHER" Value="ODM OTHER"></asp:ListItem>
                        <asp:ListItem Text="ODM TO RTP" Value="ODM TO RTP"></asp:ListItem>
                        <asp:ListItem Text="OTHER" Value="OTHER"></asp:ListItem>
                        <asp:ListItem Text="TRIAGE" Value="TRIAGE"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="center" style="margin-top: 2px">
                <asp:Button runat="server" ID="btnModalOk" Text="Save" CssClass="buttonBox" OnClick="btnModalOk_Click" ValidationGroup="NotesGroup" CausesValidation="true" />
                <asp:Button runat="server" ID="btnModalCancel" Text="Cancel" CssClass="buttonBox" CausesValidation="false" />
            </div>
            <div>
            </div>

        </asp:Panel>
    </asp:Panel>
    <asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />

</div>





<cc2:messagebox id="MessageBox2" runat="server" />


