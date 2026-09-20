<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_DelegatesUsers, App_Web_iq0r534d" %>
<%@ register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajx" %>

<script>
    $(document).ready(function () {
        Text = $('#ctl00_MainContent_DelegatesID_rgDelegates_ctl00_ctl03_ctl01_PageSizeComboBox_Input'); // Set the name attribute
        Text.attr('aria-label', 'ProviderType'); // Output the button element with the name attribute console.log(button[0].outerHTML);
    });
</script>

<asp:panel id="pnlPageHeader" runat="server" cssclass="pageHeader">
    <p style="text-align: center">
        <asp:literal id="pagelabel" runat="server" text="Reference Data Management: Delegates"></asp:literal>
    </p>
</asp:panel>
<asp:validationsummary id="valDelegateUserIDValidation" displaymode="List" runat="server" cssclass="failureNotification val-summary" validationgroup="DelegateUserIDValidation" visible="true" enabled="true" showsummary="true" />
<br />
<telerik:radgrid id="rgDelegates" runat="server" rendermode="Lightweight" allowpaging="True" allowsorting="True"
    onneeddatasource="rgDelegates_NeedDataSource" allowfilteringbycolumn="False" skin="PDMSModern"
    cellspacing="0" gridlines="None" oninsertcommand="rgDelegates_InsertCommand" onupdatecommand="rgDelegates_UpdateCommand"
    autogeneratecolumns="false" autogenerateeditcolumn="False" onitemdatabound="rgDelegates_ItemDataBound">
    <validationsettings enablevalidation="true" validationgroup="BackgroundValidation" commandstovalidate="rgDelegates_InsertCommand,rgDelegates_UpdateCommand" />
    <mastertableview commanditemdisplay="Top" gridlines="None"
        datakeynames="USERNAME,CONTACT_NAME,EMAIL,ACTIVE_DELEGATE,PHONE_MATCH">
        <columns>
            <telerik:grideditcommandcolumn headertext="Edit">
            </telerik:grideditcommandcolumn>
            <telerik:gridboundcolumn datafield="USERNAME" headertext="User ID" uniquename="USERNAMEID">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="CONTACT_NAME" headertext="Contact Name" uniquename="CONTACT_NAME">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="EMAIL" headertext="Email Address" uniquename="EMAIL">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="ACTIVE_DELEGATE" headertext="Active Delegate" uniquename="ACTIVE_DELEGATE">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="PHONE_MATCH" headertext="Phone Exact Match" uniquename="PHONE_MATCH" Display="false" >
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="PHONE_MATCH_VALUE" headertext="Phone Exact Match" uniquename="PHONE_MATCH_VALUE" >
            </telerik:gridboundcolumn>
        </columns>
        <editformsettings editformtype="Template">
            <editcolumn uniquename="EditCol">
            </editcolumn>
            <formtemplate>
                <div class="WhiteBox">
                    <div class="gridEditTable">
                        <div class="row">
                            <div class="col-sm-3 text-right">User ID </div>
                            <div class="col-sm-9 text-left">
                                <asp:textbox maxlength="80" id="txtUserName" runat="server" text='<%# Bind("USERNAME") %>' cssclass="formField">
                                </asp:textbox>
                                <asp:requiredfieldvalidator id="rfvUserName" runat="server" errormessage="* User ID is required"
                                    controltovalidate="txtUserName">
                                </asp:requiredfieldvalidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Contact Name </div>
                            <div class="col-sm-9 text-left">
                                <asp:textbox maxlength="256" id="txtContact" runat="server" text='<%# Bind("CONTACT_NAME") %>' cssclass="formField">
                                </asp:textbox>
                                <asp:requiredfieldvalidator id="rfvContactName" runat="server" errormessage="* Contact Name is required"
                                    controltovalidate="txtContact">
                                </asp:requiredfieldvalidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Email Address</div>
                            <div class="col-sm-9 text-left">
                                <asp:textbox maxlength="256" id="txtEmail" runat="server" text='<%# Bind("EMAIL") %>' cssclass="formField">
                                </asp:textbox>
                                <asp:requiredfieldvalidator id="rfvEmailAddr" runat="server" errormessage="* Email Address is required"
                                    controltovalidate="txtEmail">
                                </asp:requiredfieldvalidator>
                                <asp:regularexpressionvalidator id="regexEmailValid" runat="server" validationexpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" controltovalidate="txtEmail" errormessage="Invalid Email Format"></asp:regularexpressionvalidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Active Delegate</div>
                            <div class="col-sm-9 text-left">
                                <asp:radiobuttonlist id="rblActiveDelegate" runat="server" cssclass="formLabel300 rblYesNo" textalign="Right" repeatdirection="Horizontal" selectedvalue='<%# Bind("ACTIVE_DELEGATE") %>'>
                                    <asp:listitem text="Yes" value="True" />
                                    <asp:listitem text="No" value="False" />
                                    <asp:listitem value="" text="" style="display: none" />
                                </asp:radiobuttonlist>
                                <asp:requiredfieldvalidator id="rfvActiveDelegate" runat="server" errormessage="* Active Delegate selection is required"
                                    controltovalidate="rblActiveDelegate">
                                </asp:requiredfieldvalidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Phone Exact Match</div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="ckbPhoneMatch" textalign="Right" repeatdirection="Horizontal" Checked='<%# Bind("PHONE_MATCH") %>' />
                            </div>
                        </div>
                        <div class="row text-center">
                            <asp:button id="btnUpdate" text='<%# (Container is GridEditFormInsertItem) ? "Save" : "Save" %>'
                                runat="server" commandname='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' cssclass="buttonBox buttonBoxFocus"></asp:button>
                            <asp:button id="btnCancel" text="Cancel" runat="server" causesvalidation="False"
                                commandname="Cancel" cssclass="buttonBox"></asp:button>
                        </div>
                    </div>
                </div>
            </formtemplate>
            <popupsettings scrollbars="None" />
        </editformsettings>
    </mastertableview>
</telerik:radgrid>
