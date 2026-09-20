<%@ control language="C#" autoeventwireup="true" inherits="UserControls_RegEnrollment" Codebehind="NPIandMedIdControl.ascx.cs" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>

<asp:ValidationSummary ID="vsRegEnrollment" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="NPIandMedIdControl" />
<asp:UpdatePanel ID="upProv" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <h2><span class="pageHeader">NPI Span</span></h2>
        <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
        <div>History of the NPI in PNM is listed below.</div>
        <br>
        <div id="divNote" style="font-weight: bold; margin-top: 5px; margin-bottom: 3px; font-style: italic">
            Note: Only dates with a calendar icon are editable for the NPI span by Provider Type.
        </div>
        <br>
        <asp:RadioButtonList ID="rblEnrollmentSpanActions" runat="server" RepeatDirection="Vertical" OnSelectedIndexChanged="rblEnrollmentSpanActions_SelectedIndexChanged" AutoPostBack="true" Style="margin-left: 0" />
        <br>
        <asp:Label ID="lblRequestedEffectiveDate" runat="server" Text="" Style="font-weight: bold;" />
        <br>
        <br>

        <asp:Label ID="lblFailureMsg" runat="server" Text="" CssClass="failureNotification" />
        <asp:HiddenField ID="hdnEnrollIdORP" runat="server" />
        <asp:HiddenField ID="hdnEnrollIdORPInactive" runat="server" />
        <telerik:radgrid id="rgApplicationType" runat="server" rendermode="Lightweight" mastertableview-caption="Enrollment Spans" allowpaging="True" allowsorting="False"
            onneeddatasource="rgApplicationType_NeedDataSource" allowfilteringbycolumn="False" skin="PDMSModern"
            cellspacing="0" gridlines="None" onupdatecommand="rgApplicationType_UpdateCommand"
            onitemdatabound="rgApplicationType_ItemDataBound"
            autogeneratecolumns="false" autogenerateeditcolumn="False" onprerender="rgApplicationType_PreRender">

            <mastertableview commanditemdisplay="None" gridlines="None" datakeynames="REG_NPI_MEDID_ENROLLMENT_SPAN_ID">
                <columns>

                    <telerik:grideditcommandcolumn uniquename="EditCommandColumn" buttontype="ImageButton" edittext="Edit" updatetext="Update" canceltext="Cancel" editimageurl="~/Images/edit.png" updateimageurl="../App_Themes/Default/Grid/Update.gif" cancelimageurl="../App_Themes/Default/Grid/Cancel.gif">
                    </telerik:grideditcommandcolumn>

                    <telerik:gridboundcolumn datafield="NPI" headertext="NPI Number       " uniquename="EnrollmentNpiNumber">
                    </telerik:gridboundcolumn>
                    <telerik:gridboundcolumn datafield="MMIS_PROVIDER_TYPE_ID" headertext="PT Type      " uniquename="EnrollmentPTType">
                    </telerik:gridboundcolumn>
                    <telerik:gridboundcolumn datafield="REG_ID" headertext="Reg ID" uniquename="EnrollmentRegId">
                    </telerik:gridboundcolumn>
                    <telerik:gridboundcolumn datafield="MEDICAID_ID" headertext="Med ID" uniquename="EnrollmentMedId">
                    </telerik:gridboundcolumn>

                    <telerik:gridtemplatecolumn datafield="ENROLL_START_DATE_TIME" headertext="Provider Effective Date" uniquename="EnrollmentEffDate" datatype="System.DateTime">
                        <itemtemplate>
                            <asp:Image ID="imgEffDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="18px" AlternateText="Effective Date Calendar" />
                            <asp:Label ID="lblEffDate" runat="server" CssClass="formLabelGrid" Text='<%#Eval("ENROLL_START_DATE_TIME", "{0:MM/dd/yyyy}") %>'></asp:Label>
                        </itemtemplate>
                    </telerik:gridtemplatecolumn>

                    <telerik:gridtemplatecolumn datafield="ENROLL_END_DATE_TIME" headertext="End Date" uniquename="EnrollmentEndDate" datatype="System.DateTime">
                        <itemtemplate>
                            <asp:Image ID="imgEndDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="18px" AlternateText="End Date Calendar" />
                            <asp:Label ID="lblEndDate" runat="server" CssClass="formLabelGrid" Text='<%#Eval("ENROLL_END_DATE_TIME", "{0:MM/dd/yyyy}") %>'></asp:Label>
                        </itemtemplate>
                    </telerik:gridtemplatecolumn>

                    <telerik:gridboundcolumn datafield="UserName" headertext="User/History" uniquename="EnrollmentUsername">
                    </telerik:gridboundcolumn>
                    <telerik:gridboundcolumn datafield="ENROLL_STATUS_DESC" headertext="Active/Inactive" uniquename="EnrollentStatus">
                    </telerik:gridboundcolumn>
                    <telerik:gridboundcolumn datafield="REG_NPI_MEDID_ENROLLMENT_SPAN_ID" uniquename="REG_NPI_MEDID_ENROLLMENT_SPAN_ID" display="false">
                    </telerik:gridboundcolumn>
                </columns>
                <editformsettings editformtype="Template">
                    <formtemplate>
                        <div class="WhiteBox">
                            <div class="gridEditTable">
                                <div class="row">
                                    <div class="col-sm-3 text-right">NPI</div>
                                    <div class="col-sm-9 text-left">
                                        <asp:TextBox MaxLength="50" ID="txtNpi" aria-label="NPI" runat="server" Text='<%# Bind("NPI") %>' CssClass="formField"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right">PT Type</div>
                                    <div class="col-sm-9 text-left">
                                        <asp:TextBox MaxLength="50" ID="txtPTType" aria-label="PT Type" runat="server" Text='<%# Bind("MMIS_PROVIDER_TYPE_ID") %>' CssClass="formField"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right">Reg ID</div>
                                    <div class="col-sm-9 text-left">
                                        <asp:TextBox MaxLength="50" ID="txtRegId" aria-label="Reg ID" runat="server" Text='<%# Bind("REG_ID") %>' CssClass="formField"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right">Medicaid ID</div>
                                    <div class="col-sm-9 text-left">
                                        <asp:TextBox MaxLength="50" ID="txtMedicaidId" aria-label="Medicaid ID" runat="server" Text='<%# Bind("MEDICAID_ID") %>' CssClass="formField"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right">Provider Effective Date</div>
                                    <div class="col-sm-9 text-left">
                                        <ajax:calendarextender id="calProviderEffectiveDate" runat="server"
                                            format="MM/dd/yyyy" targetcontrolid="txtProviderEffectiveDate" popupposition="BottomRight"
                                            cssclass="QstCalendarCSS" popupbuttonid="imgExpirationDate" enabledonclient="true" />
                                        <asp:TextBox ID="txtProviderEffectiveDate" runat="server" aria-label="Provider Effective Date" Text='<%# Bind("ENROLL_START_DATE_TIME","{0:MM/dd/yyyy}") %>' CssClass="formField" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right">End Date</div>
                                    <div class="col-sm-9 text-left">
                                        <ajax:calendarextender id="calProviderEndDate" runat="server"
                                            format="MM/dd/yyyy" targetcontrolid="txtProviderEndDate" popupposition="BottomRight"
                                            cssclass="QstCalendarCSS" popupbuttonid="imgExpirationDate" enabledonclient="true" />
                                        <asp:TextBox ID="txtProviderEndDate" runat="server" aria-label="End Date" Text='<%# Bind("ENROLL_END_DATE_TIME","{0:MM/dd/yyyy}") %>' CssClass="formField" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right">User/History</div>
                                    <div class="col-sm-9 text-left">
                                        <asp:TextBox MaxLength="50" ID="txtUsername" aria-label="Username" runat="server" Text='<%# Bind("UserName") %>' CssClass="formField"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-3 text-right">Active/Inactive</div>
                                    <div class="col-sm-9 text-left">
                                        <asp:TextBox MaxLength="50" ID="txtStatus" aria-label="Active/Inactive" runat="server" Text='<%# Bind("ENROLL_STATUS_DESC") %>' CssClass="formField"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row" hidden>
                                    <div class="col-sm-3 text-right">Active/Inactive</div>
                                    <div class="col-sm-9 text-left">
                                        <asp:TextBox MaxLength="50" ID="txtRegEnrollmentId" aria-label="Reg Enrollment ID" runat="server" Text='<%# Bind("REG_NPI_MEDID_ENROLLMENT_SPAN_ID") %>' CssClass="formField"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-9"></div>
                                    <div class="col-sm-3 text-right">
                                        <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                            runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBoxFocus"></asp:Button>&nbsp;
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                                        CommandName="Cancel" CssClass="buttonBox"></asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </formtemplate>
                    <popupsettings scrollbars="None" />
                </editformsettings>
            </mastertableview>
        </telerik:radgrid>
    </ContentTemplate>
</asp:UpdatePanel>
