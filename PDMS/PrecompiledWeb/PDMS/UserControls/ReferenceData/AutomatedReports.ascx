<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_AutomatedReports, App_Web_iq0r534d" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
    <p style="text-align: center">
        <asp:Literal ID="pagelabel" runat="server" Text="Configuration Management: Automated Reports"></asp:Literal>
    </p>
</asp:Panel>
<script type="text/javascript">
</script>

<asp:ValidationSummary ID="ARValidationSummarySuccessID" DisplayMode="List" runat="server" CssClass="successNotification" ValidationGroup="ARValidationSummarySuccessGP" />
<asp:ValidationSummary ID="ARValidationSummaryID" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="ARValidationSummaryGP" />
<telerik:radgrid id="rgAutomatedReports" runat="server" rendermode="Lightweight" allowpaging="True" allowsorting="True"
    onneeddatasource="rgAutomatedReports_NeedDataSource" ondeletecommand="rgAutomatedReports_DeleteCommand" allowfilteringbycolumn="True" skin="PDMSModern"
    cellspacing="0" gridlines="None" oninsertcommand="rgAutomatedReports_InsertCommand" onupdatecommand="rgAutomatedReports_UpdateCommand"
    autogeneratecolumns="false" autogenerateeditcolumn="False">
    <clientsettings>

        <scrolling allowscroll="True" scrollheight="" usestaticheaders="True" savescrollposition="true"></scrolling>

    </clientsettings>
    <mastertableview commanditemdisplay="Top" gridlines="None">
        <columns>
            <telerik:grideditcommandcolumn headertext="<span style='display:none'>Edit</span>">
            </telerik:grideditcommandcolumn>
            <telerik:gridboundcolumn datafield="ID" headertext="ID" uniquename="ID">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="REPORT_NAME" headertext="Report Name" uniquename="REPORT_NAME">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="REPORT_ID" headertext="Report ID" uniquename="REPORT_ID">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="REPORT_SUBJECT" headertext="Report Subject Line" uniquename="REPORT_SUBJECT">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="REPORT_FROM" headertext="Report Emailed From" uniquename="REPORT_FROM">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="REPORT_TO" headertext="Report Emailed To" uniquename="REPORT_TO">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="REPORT_BCC" headertext="Report Emailed Bcc" uniquename="REPORT_BCC">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="REPORT_SQL" headertext="Report SQL Name" uniquename="REPORT_SQL">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="REPORT_TEMPLATE" headertext="Report Template Name" uniquename="REPORT_TEMPLATE">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="REPORT_ATTACHMENT" headertext="Report Attachment Name" uniquename="REPORT_ATTACHMENT">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="CRON_EXP" headertext="CRON Expression" uniquename="CRON_EXP">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="ACTIVE" headertext="Enable/Disable Report" uniquename="ACTIVE" datatype="System.Boolean">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="ENVIRONMENT" headertext="Environment" uniquename="ENVIRONMENT">
            </telerik:gridboundcolumn>
        </columns>
        <editformsettings editformtype="Template">
            <editcolumn uniquename="EditCol">
            </editcolumn>
            <formtemplate>
                <div class="WhiteBox">
                    <div class="gridEditTable">
                        <div class="row">
                            <div class="col-sm-3 text-right">ID</div>
                            <div class="col-sm-9 text-left">
                                <asp:Label ID="txtID" runat="server" Text='<%# Bind("ID") %>' Style="width: 450px" Enabled="false"></asp:Label>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-3 text-right">Report Name</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtReportName" runat="server" Text='<%# Bind("REPORT_NAME") %>' Style="width: 450px">
                                </asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-3 text-right">Report ID</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtReportId" runat="server" Text='<%# Bind("REPORT_ID") %>' Style="width: 450px">
                                </asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-3 text-right">Report Subject Line</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtReportSubject" runat="server" Text='<%# Bind("REPORT_SUBJECT") %>' Style="width: 450px">
                                </asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-3 text-right">Report Emailed From</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtReportFrom" runat="server" Text='<%# Bind("REPORT_FROM") %>' TextMode="MultiLine" CssClass="formFieldMultiline">
                                </asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-3 text-right">Report Emailed To</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtReportTo" runat="server" Text='<%# Bind("REPORT_TO") %>' TextMode="MultiLine" CssClass="formFieldMultiline">
                                </asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-3 text-right">Report Emailed Bcc</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtReportBcc" runat="server" Text='<%# Bind("REPORT_BCC") %>' TextMode="MultiLine" CssClass="formFieldMultiline">
                                </asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-3 text-right">Report SQL Name</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtReportSQL" runat="server" Text='<%# Bind("REPORT_SQL") %>' Style="width: 450px">
                                </asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-3 text-right">Report Template Name</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtReportTemplate" runat="server" Text='<%# Bind("REPORT_TEMPLATE") %>' Style="width: 450px">
                                </asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-3 text-right">Report Attachment Name</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtReportAttachment" runat="server" Text='<%# Bind("REPORT_ATTACHMENT") %>' Style="width: 450px">
                                </asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-3 text-right">CRON Expression</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="100" ID="txtCRON" runat="server" Text='<%# Bind("CRON_EXP") %>' Style="width: 450px">
                                </asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-3 text-right">Enable/Disable Report</div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="chkActiveReport" Checked='<%# Bind("ACTIVE") %>' />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-sm-3 text-right">Environment</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="100" ID="txtReportEnvironment" runat="server" Text='<%# Bind("ENVIRONMENT") %>' Style="width: 450px">
                                </asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row text-center">
                            <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CssClass="buttonBox"
                                CommandName="Cancel"></asp:Button>
                            <asp:Button ID="btnDelete" Text="Delete" runat="server" CausesValidation="False" CssClass="buttonBox buttonBoxFocus"
                                CommandName="Delete"></asp:Button>
                        </div>
                        <br />
                    </div>
                </div>
            </formtemplate>
            <popupsettings scrollbars="None" />
        </editformsettings>
    </mastertableview>
</telerik:radgrid>

<asp:Panel runat="server" ID="pnlAutomatedReportSubGrid">
    <br />
    <div class="row">
        <span>&nbsp;&nbsp;&nbsp;Report ID : </span>
            <asp:TextBox MaxLength="100" ID="txtReportIDGrid" Style="width: 450px" runat="server">
            </asp:TextBox>&nbsp;&nbsp;
            <asp:Button ID="btnGridResults" runat="server" OnClick="btnGridResults_Click" Text="Search"></asp:Button>&nbsp;&nbsp;
            <asp:Button ID="btnDeleteGrid" runat="server" OnClick="btnDeleteGrid_Click" Text="Delete"></asp:Button>
    </div>
    <br />
    <div class="divGrid">
        <asp:GridView runat="server" ID="grdAutomatedReportSub" AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No records found">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" />
                <asp:BoundField DataField="REPORT_ID" HeaderText="Report ID" />
                <asp:BoundField DataField="REPORT_STATUS" HeaderText="Report Status" />
                <asp:BoundField DataField="REPORT_MESSAGE" HeaderText="Report Message" />
                <asp:BoundField DataField="EMAIL_SENT" HeaderText="Email Sent" />
                <asp:BoundField DataField="TOPROCESS_DATE" HeaderText="To Process Date" DataFormatString="{0:MM/dd/yy H:mm:ss}" />
                <asp:BoundField DataField="EMAIL_SENT_DATE" HeaderText="Email Sent Date" DataFormatString="{0:MM/dd/yy H:mm:ss}" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    <br />
</asp:Panel>
