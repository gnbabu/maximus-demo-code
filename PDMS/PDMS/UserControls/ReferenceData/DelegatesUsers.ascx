<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_DelegatesUsers" Codebehind="DelegatesUsers.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<style>
    .formDropDown {
        font-size: 17px;
        height: 44px;
        color: #000;
    }

    .formField {
        height: 44px;
        color: #000;
    }
</style>
<script>
    $(document).ready(function () {
        Text = $('#ctl00_MainContent_DelegatesID_rgDelegates_ctl00_ctl03_ctl01_PageSizeComboBox_Input'); // Set the name attribute
        Text.attr('aria-label', 'ProviderType'); // Output the button element with the name attribute console.log(button[0].outerHTML);
    });
</script>

<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
    <p style="text-align: center" class="page-main-header">
        <asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management: Delegates"></asp:Literal>
    </p>
</asp:Panel>
<asp:ValidationSummary ID="valDelegateUserIDValidation" DisplayMode="List" runat="server" CssClass="failureNotification val-summary" ValidationGroup="DelegateUserIDValidation" Visible="true" Enabled="true" ShowSummary="true" />
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
            <telerik:gridboundcolumn datafield="PHONE_MATCH" headertext="Phone Exact Match" uniquename="PHONE_MATCH" display="false">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="PHONE_MATCH_VALUE" headertext="Phone Exact Match" uniquename="PHONE_MATCH_VALUE">
            </telerik:gridboundcolumn>
        </columns>
        <editformsettings editformtype="Template">
            <editcolumn uniquename="EditCol">
            </editcolumn>
            <formtemplate>
                <div class="WhiteBox">
                    <div class="gridEditTable">
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                <span class="ohio-field">User ID</span>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="80" ID="txtUserName" runat="server" Text='<%# Bind("USERNAME") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ErrorMessage="* User ID is required"
                                    ControlToValidate="txtUserName">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                <span class="ohio-field">Contact Name</span>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="256" ID="txtContact" runat="server" Text='<%# Bind("CONTACT_NAME") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvContactName" runat="server" ErrorMessage="* Contact Name is required"
                                    ControlToValidate="txtContact">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                <span class="ohio-field">Email Address</span>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="256" ID="txtEmail" runat="server" Text='<%# Bind("EMAIL") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEmailAddr" runat="server" ErrorMessage="* Email Address is required"
                                    ControlToValidate="txtEmail">
                                </asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" ErrorMessage="Invalid Email Format"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                <span class="ohio-field">Active Delegate</span>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:RadioButtonList ID="rblActiveDelegate" runat="server" CssClass="formLabel300 rblYesNo" TextAlign="Right" RepeatDirection="Horizontal" SelectedValue='<%# Bind("ACTIVE_DELEGATE") %>'>
                                    <asp:ListItem Text="Yes" Value="True" />
                                    <asp:ListItem Text="No" Value="False" />
                                    <asp:ListItem Value="" Text="" style="display: none" />
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="rfvActiveDelegate" runat="server" ErrorMessage="* Active Delegate selection is required"
                                    ControlToValidate="rblActiveDelegate">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                <span class="ohio-field">Phone Exact Match</span>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="ckbPhoneMatch" TextAlign="Right" repeatdirection="Horizontal" Checked='<%# Bind("PHONE_MATCH") %>' />
                            </div>
                        </div>
                        <div class="row" style="padding-top: 20px;">
                            <div class="col-sm-3 text-right">
                                <span class="ohio-field"></span>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Save" : "Save" %>'
                                    runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
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
