<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="Process_AdminUserAccounts" Codebehind="AdminUserAccounts.aspx.cs" %>

<%@ Register Src="~/UserControls/SelectedControl.ascx" TagName="SelectedControl" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc2" %>
<%@ Register Src="FormField.ascx" TagName="FormField" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/ManyToManySelector.ascx" TagPrefix="uc" TagName="ManyToManySelector" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="PageLabelContent">
    User Accounts
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">


    <!--[if gt IE 7]>
        <style type="text/css">
        </style>
    <![endif]-->
    <script type="text/javascript">
        function CheckPhoneLength(sender, args) {
            var re = /\D/g; // Remove any characters that are not numbers
            var test = args.Value.replace(re, "");
            if (test == "") return true;

            var len = test.length;
            if (len != 10)
                args.IsValid = false;
            if (test[0] == 0 || test[0] == 1 || test[3] == 0 || test[3] == 1)
                args.IsValid = false;
            return;
        }

        function AnswerIsYes(rblId) {
            if (document.getElementById(rblId) != null) {
                var oElem = document.getElementById(rblId);
                var radio = oElem.getElementsByTagName("input");
                return radio[0].checked;
            }
            return false;
        }

        function TogglePanel(rbl1_Id, pnl, pnl2) {
            if (AnswerIsYes(rbl1_Id)) {
                document.getElementById(pnl).style.display = "block";
                document.getElementById(pnl2).style.display = "none";
            }
            else {
                document.getElementById(pnl).style.display = "none";
                document.getElementById(pnl2).style.display = "block";
            }
        }
    </script>
    <script type="text/javascript">
        function doClick(buttonName, e) {
            // The purpose of this function is to allow the enter key to 
            // point to the correct button to click.
            var key;

            if (window.event) key = window.event.keyCode;   // IE
            else key = e.which;                             // Firefox

            if (key == 13) {
                //Get the button the user wants to have clicked
                var btn = document.getElementById(buttonName);
                if (btn != null) { //If we find the button click it
                    btn.click();
                    event.keyCode = 0;
                }
            }
        }
    </script>

    <div class="WhiteBox">
        <div id="userCreateAccount">
            <asp:Panel ID="pnlStepControl" runat="server">
                <div style="text-align: center;">
                    <div class="row">
                        <div class="col-sm-6">
                            <uc1:SelectedControl ID="ucStep1" runat="server" Text="Create User ID & Password" Selected="false" />
                        </div>
                        <div class="col-sm-6">
                            <uc1:SelectedControl ID="ucStepPermissions" Text="Permissions" runat="server" Selected="false" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <br />
            <div style="float: right;">
                <table class="tblButtons" role="presentation">
                    <div class="row">
                         <th style="visibility:hidden">table</th>
                        <td>
                             <asp:Button ID="btnDelete" CssClass="buttonBox buttonBoxFocus" runat="server" Text="Delete User" OnClick="btnDelete_Click" ToolTip="Delete User" OnClientClick="return confirm('Are you sure you want to delete?');" CausesValidation="false" />
                        </td>
                        <td>
                             <asp:Button ID="btnReactivate" CssClass="buttonBox buttonBoxFocus" runat="server" Text="Reactivate User" OnClick="btnReactivate_Click" ToolTip="Reactivate User" OnClientClick="return confirm('Are you sure you want to Reactivate?');" CausesValidation="false" />
                        </td>
                        <td>
                            <asp:Button ID="btnPrevTop" CssClass="buttonBox" runat="server" Text="Previous" OnClick="btnPrev" CausesValidation="false" ToolTip="Previous" /></td>
                        <td>
                            <asp:Button ID="btnSaveTopUser" CssClass="buttonBox buttonBoxFocus" runat="server" Text="Save" OnClick="btnSubmit_Click" ToolTip="Save User" /></td>
                        <td>
                            <asp:Button ID="btnSaveTopPerm" CssClass="buttonBox buttonBoxFocus" runat="server" Text="Save" OnClick="btnSubmitPermissions_Click" ToolTip="Save Permissions" /></td>
                        <td>
                            <asp:Button ID="btnCancTop" CssClass="buttonBox" runat="server" Text="Cancel" OnClick="btnCancel" CausesValidation="false" ToolTip="Cancel" /></td>
                    </div>
                </table>
            </div>
            <asp:ValidationSummary ID="valAdminUserAccounts" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="AdminUserAccounts" />
            <asp:Panel ID="pnlPermissions" runat="server" CssClass="pnlWidth">
                <asp:UpdatePanel ID="upValidationSummary" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlValidationSummary" runat="server" Style="width: 100%" Visible="false">
                            <asp:Label ID="URP03_ERR" runat="server" Visible="false" Text="" CssClass="failureNotification" Style="color: Red" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <uc2:Separator ID="Separator2" runat="server" Header="User Role" />
                <br />
                <div class="row">
                    <div class="col-sm-5 text-right">
                        <asp:Label ID="URP02" runat="server" CssClass="formLabel170" Text="User Name: " />
                    </div>
                    <div class="col-sm-7 text-left">
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="formField" ToolTip="User Name" />
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtUserName" Display="Dynamic"
                            ErrorMessage="* Username is required" SetFocusOnError="true" Text="*" ValidationGroup="AdminUserAccounts" />
                        <asp:CustomValidator ID="CustomValidator1" runat="server" OnServerValidate="Validate_UserNameExists"
                            ControlToValidate="txtUserName" Display="Dynamic" Text="*"
                            ValidationGroup="AdminUserAccounts" ErrorMessage="* User ID already Exists." />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-5 text-right">
                        <span class="formLabel">
                            <asp:Label ID="lblOHID" runat="server" Text="OH|ID: " /></span>
                    </div>
                    <div class="col-sm-7 text-left">
                        <asp:TextBox runat="server" ID="txtOHID" CssClass="formField" Aria-label="OH|ID(SOUID)"/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-5 text-right">
                        <span class="formLabel">
                            <asp:Label ID="lblIOPEmail" runat="server" Text="Email Address: " /></span>
                    </div>
                    <div class="col-sm-7 text-left">
                        <asp:TextBox runat="server" ID="txtIOPEmail" CssClass="formField" aria-label="Email Address"/>
                    </div>
                </div>  
                <div class="row">
                    <div class="col-sm-5 text-right">
                        <span class="formLabel">
                            <asp:Label ID="lblContactName" runat="server" Text="Contact Name: " /></span>
                    </div>
                    <div class="col-sm-7 text-left">
                        <asp:TextBox runat="server" ID="txtIOPContactName" MaxLength="50" CssClass="formField" aria-label="Contact Name" />
                    </div>
                </div>    
                <br />
                <br />
                <uc:ManyToManySelector runat="server" ID="mmsUserRoles" />
                <br />
                <asp:Panel ID="pnlProviderTypes" runat="server">
                    <uc2:Separator ID="Separator3" runat="server" Header="User Expertise" />
                    <br />
                    <uc:FormField ID="ProviderTypeFilter" runat="server" LabelText="Filter By Provider Type" FieldType="ComboBox" ValidationToolTip="Select a Provider Type" />
                    <br />
                    <uc:ManyToManySelector runat="server" ID="mmsProviderTypes" />
                </asp:Panel>
                <asp:Panel ID="pnlQueueAssignment" runat="server">
                    <br />
                    <uc2:Separator ID="Separator4" runat="server" Header="Queue Assignment" />
                    <br />
                    <table role="presentation">
                        <div class="row">
                            <tr>
                                <td>
                                    <table>
                                        <div class="row">
                                            <tr>
                                                <td>
                                                    <asp:ListBox ID="lbAppFrom" runat="server" Height="250" SelectionMode="Multiple" Width="250" />
                                                </td>
                                            </tr>
                                        </div>
                                        <div class="row">
                                        </div>
                                        <div class="row">
                                            <tr>
                                                <td align="right">
                                                    <br />
                                                    <asp:Button ID="btnAppFromAll" runat="server" class="listButtonsBelow btn btn-default btn-sm active" CommandName="AppFromAll" OnCommand="ListViewButtons_Click" Text="All" ToolTip="All" />
                                                    <asp:Button ID="btnAppFromNone" runat="server" class="listButtonsBelow btn btn-default btn-sm active" CommandName="AppFromNone" OnCommand="ListViewButtons_Click" Text="None" ToolTip="None" />
                                            </tr>
                                        </div>
                                    </table>
                                </td>
                                <td align="center" width="25%">
                                    <asp:Button ID="btnAppAdd" runat="server" class="listButtons btn btn-default btn-md active" CommandName="AppAdd" OnCommand="ListViewButtons_Click" Text="Add &gt;&gt;&gt;" ToolTip="Add App" />
                                    <br />
                                    <br />
                                    <asp:Button ID="btnAppRemove" runat="server" class="listButtons btn btn-default btn-md active" CommandName="AppRemove" OnCommand="ListViewButtons_Click" Text="&lt;&lt;&lt; Remove" ToolTip="Remove" />
                                </td>
                                <td>
                                    <table role="presentation">
                                        <div class="row">
                                            <tr>
                                                <td>
                                                    <asp:ListBox ID="lbAppTo" runat="server" Height="250" SelectionMode="Multiple" Width="250" />
                                                </td>
                                            </tr>
                                        </div>
                                        <div class="row">
                                            <tr>
                                                <td align="right">
                                                    <br />
                                                    <asp:Button ID="btnAppToAll" runat="server" class="listButtonsBelow btn btn-default btn-sm active" CommandName="AppToAll" OnCommand="ListViewButtons_Click" Text="All" ToolTip="All" />
                                                    <asp:Button ID="btnAppToNone" runat="server" class="listButtonsBelow btn btn-default btn-sm active" CommandName="AppToNone" OnCommand="ListViewButtons_Click" Text="None" ToolTip="None" />
                                            </tr>
                                        </div>
                                    </table>
                                </td>
                            </tr>
                        </div>
                    </table>
                </asp:Panel>
                <div style="display: none;">
                    <br />
                    <uc2:Separator ID="Separator5" runat="server" Header="Statuses" />
                    <br />
                    <table role="presentation">
                        <div class="row">
                            <tr>
                                <td>
                                    <table role="presentation">
                                        <div class="row">
                                            <tr>
                                                <td>
                                                    <asp:ListBox ID="lbStatFrom" runat="server" Height="250" SelectionMode="Multiple" Width="250" />
                                                </td>
                                            </tr>
                                        </div>
                                        <div class="row">
                                            <tr>
                                                <td align="right">
                                                    <br />
                                                    <asp:Button ID="btnStatFromAll" runat="server" class="listButtonsBelow btn btn-default btn-sm active" CommandName="StatFromAll" OnCommand="ListViewButtons_Click" Text="All" ToolTip="All" />
                                                    <asp:Button ID="btnStatFromNone" runat="server" class="listButtonsBelow btn btn-default btn-sm active" CommandName="StatFromNone" OnCommand="ListViewButtons_Click" Text="None" ToolTip="None" />
                                                </td>
                                            </tr>
                                        </div>
                                    </table>
                                </td>
                                <td align="center" width="25%">
                                    <asp:Button ID="btnStatAdd" runat="server" class="listButtons btn btn-default btn-md active" CommandName="StatAdd" OnCommand="ListViewButtons_Click" Text="Add &gt;&gt;&gt;" ToolTip="Add" />
                                    <br />
                                    <br />
                                    <asp:Button ID="btnStatRemove" runat="server" class="listButtons btn btn-default btn-md active" CommandName="StatRemove" OnCommand="ListViewButtons_Click" Style="font-size: 8pt; width: 100px;" Text="&lt;&lt;&lt; Remove" ToolTip="Remove" />
                                </td>
                                <td>
                                    <table role="presentation">
                                        <div class="row">
                                            <tr>
                                                <td>
                                                    <asp:ListBox ID="lbStatTo" runat="server" Height="250" SelectionMode="Multiple" Width="250" />
                                                </td>
                                            </tr>
                                        </div>
                                        <div class="row">
                                            <tr>
                                                <td align="right">
                                                    <br />
                                                    <asp:Button ID="btnStatToAll" runat="server" class="listButtonsBelow btn btn-default btn-sm active" CommandName="StatToAll" OnCommand="ListViewButtons_Click" Text="All" ToolTip="All" />
                                                    <asp:Button ID="btnStatToNone" runat="server" class="listButtonsBelow btn btn-default btn-sm active" CommandName="StatToNone" OnCommand="ListViewButtons_Click" Text="None" ToolTip="None" />
                                                </td>
                                            </tr>
                                        </div>
                                    </table>
                                </td>
                            </tr>
                        </div>
                    </table>
                </div>
                <div style="width: 100%; text-align: right; display: none;">
                    <asp:Button ID="buttonBox" runat="server" OnClick="btnSubmitPermissions_Click" CssClass="buttonBox" Text="Submit" CausesValidation="false" ToolTip="Submit Permissions" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlCreateUser" runat="server">
                <asp:UpdatePanel ID="upUser" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <br />
                        <div class="boxContainer">
                            <asp:Label ID="lblHeader2" Text="Please enter your contact information" runat="server" CssClass="boxLabel" /></div>
                        <br />
                        <div style="text-align: left; margin-left: 20%;" class="tablepad">
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <span class="formLabel">
                                        <asp:Label ID="lblCname" runat="server" Text="Contact Name*" /></span>
                                </div>

                                <div class="col-sm-8 text-left">
                                    <asp:TextBox ID="txtContactName" runat="server" MaxLength="50" CssClass="formField" aria-label="Contact Name"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtContactName" ErrorMessage="* Contact Name is required" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="AdminUserAccounts" />
                                   <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Text="*" ErrorMessage="* Contact Name: Invalid character found" ControlToValidate="txtContactName" SetFocusOnError="true" Display="Dynamic" ValidationExpression="^[0-9a-zA-Z,.''-'\s]{1,50}$" ValidationGroup="AdminUserAccounts" />--%>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <span class="formLabel">
                                        <asp:Label ID="lblTitle" runat="server" Text="Title*" /></span>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="50" CssClass="formField" aria-label="Title"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" Text="*" ControlToValidate="txtTitle" ErrorMessage="* Title is required" Display="Dynamic" SetFocusOnError="true" ValidationGroup="AdminUserAccounts" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Text="*" ErrorMessage="* Title: Invalid character found" ControlToValidate="txtTitle" SetFocusOnError="true" Display="Dynamic" ValidationExpression="^[a-zA-Z,.''-'\s]{1,50}$" ValidationGroup="AdminUserAccounts" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <span class="formLabel">
                                        <asp:Label ID="lblPhNum" runat="server" Text="Phone Number*" /></span>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="formField"  Aria-label="Phone" />
                                    <ajax:MaskedEditExtender runat="server" ID="meePhoneNumber" AutoComplete="False" ClearMaskOnLostFocus="False" TargetControlID="txtPhone" MaskType="Number" Mask="(999) 999-9999" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                    <asp:CustomValidator ID="cvPhone" runat="server" OnServerValidate="Validate_cvPhoneRequired" Display="Dynamic" ValidationGroup="AdminUserAccounts" ErrorMessage="* Phone Number is required" Text="*" />
                                    <asp:CustomValidator ID="cvPhoneNum" runat="server" SetFocusOnError="True" ControlToValidate="txtPhone" ClientValidationFunction="CheckPhoneLength" OnServerValidate="Validate_cvPhoneNumber" Display="Dynamic" ErrorMessage="* Enter valid Phone Number" Text="*" ValidationGroup="AdminUserAccounts" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <span class="formLabel">
                                        <asp:Label ID="lblemail" runat="server" Text="Email Address*" /></span>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:TextBox runat="server" ID="Email" CssClass="formField" aria-label="Email Address" />
                                    <asp:CustomValidator ID="cvEmailRequired" runat="server" OnServerValidate="Validate_cvEmailRequired" ControlToValidate ="Email"
                                        Display="Static" ValidationGroup="AdminUserAccounts" ErrorMessage="* Email is required."
                                        Text="*" />
                                    <asp:CustomValidator ID="cvEmailFormat" runat="server" OnServerValidate="Validate_cvEmailFormat" ControlToValidate ="Email" 
                                        Display="Static" ValidationGroup="AdminUserAccounts" ErrorMessage="* Invalid email format."
                                        Text="*" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <span class="formLabel">
                                        <asp:Label ID="lblconfirmEmail" runat="server" Text="Confirm Email" />
                                    </span>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:TextBox runat="server" ID="ConfirmEmail" CssClass="formField" aria-label="Confirm Email Address" />
                                    <asp:CustomValidator ID="cvCEmailMatch" runat="server" OnServerValidate="Validate_cvCEmailMatch"
                                        Display="Static" ValidationGroup="AdminUserAccounts" ErrorMessage="* Email addresses must match."
                                        Text="*" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <span class="formLabel">
                                        <asp:Label ID="lblActive" runat="server" Text="Active" />
                                    </span>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <fieldset style="border:none !important">
                                    <legend style="visibility:hidden !important;margin-bottom:0px!important;height:0px">Active</legend>
                                    <asp:RadioButtonList ID="rblIsApproved" BorderStyle="None" CellPadding="0" CellSpacing="0" ToolTip="Active"
                                        RepeatDirection="Horizontal" runat="server" RepeatLayout="Table" CssClass="RadioList">
                                        <asp:ListItem Value="true">True</asp:ListItem>
                                        <asp:ListItem Value="false">False</asp:ListItem>
                                    </asp:RadioButtonList>
                                    </fieldset>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-8">
                                    <div style="float: right;">
                                        <asp:Button ID="btnUnlockUser" runat="server" Text="Unlock User" OnClick="btnUnlockUser_Click" CssClass="buttonBox" Style="margin-top: 20px;" ToolTip="Unlock User" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <uc2:Separator ID="ucSep3" runat="server" Header="Create your user id and password." />
                        <br />
                        <div style="text-align: left; margin-left: 20%;" class="tablepad">
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <span class="formLabel">
                                        <asp:Label ID="lbluserId" runat="server" Text="User ID*" /></span>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:TextBox runat="server" ID="UserName" CssClass="formField" MaxLength="50" aria-label="User ID" />
                                    <asp:RequiredFieldValidator runat="server" ID="reqUserID" ControlToValidate="UserName" Display="Dynamic"
                                        ErrorMessage="* Username is required" SetFocusOnError="true" Text="*" ValidationGroup="AdminUserAccounts" />
                                    <asp:CustomValidator ID="cvUserIDExists" runat="server" OnServerValidate="Validate_UserNameExists"
                                        ControlToValidate="UserName" Display="Dynamic" Text="*"
                                        ValidationGroup="AdminUserAccounts" ErrorMessage="* User ID is not available." />
                                   <%-- <asp:RegularExpressionValidator ID="valUserIDFormat" runat="server" ControlToValidate="UserName"
                                        ValidationExpression="^[a-zA-Z0-9@.]+$"
                                        ErrorMessage="* User ID:<ul style=&quot;margin-top:-5px!important&quot;><li>May contain uppercase letters</li><li>May contain lowercase letters</li><li>May contain numbers</li><li>May contain @ symbols and periods</li><li>At least one character long</li><li>At most 50 characters long</li></ul>"
                                        ValidationGroup="AdminUserAccounts" Text="*" Display="Dynamic"></asp:RegularExpressionValidator>--%>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <span class="formLabel">
                                        <asp:Label ID="lblOldPwd" runat="server" Text="Old Password*" /></span>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:TextBox runat="server" ID="OldPassword" TextMode="Password" CssClass="formField" MaxLength="10" aria-label="Please enter old password" />
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredOldPassword" ControlToValidate="OldPassword" Text="*" ErrorMessage="*Old Password is required" SetFocusOnError="true" ValidationGroup="AdminUserAccounts" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <span class="formLabel">
                                        <asp:Label ID="lblpwd" runat="server" Text="Password*" /></span>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="formField" MaxLength="20" aria-label="Password" />
                                    <asp:Label ID="PasswordExists" runat="server" Visible="false" Text="" CssClass="failureNotification" Style="color: Red" />
                                    <asp:RequiredFieldValidator runat="server" ID="RequirePassword" ControlToValidate="Password" Text="*" ErrorMessage="* Password is required" SetFocusOnError="true" ValidationGroup="AdminUserAccounts" />
                                    <asp:RegularExpressionValidator ID="valPasswordFormat" runat="server" ValidationGroup="AdminUserAccounts"
                                        ControlToValidate="Password"
                                        ValidationExpression="^(?=.*[0-9])(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[!@#$%\^&*\(\)\-_+=;:'\/\[\]{},.<>|`]).{8,20}$"
                                        ErrorMessage="<div class='pwd-val-error'>*Password is invalid. Password requirements:<ul><li>Between 8 and 20 characters</li><li>Contain at least one non-alphanumeric character</li><li>Contain at least one lowercase letter</li><li>Contain at least one uppercase letter</li><li>at least one number</li><li>at least one symbol or space !@#$%^&*()-_+=;:'&quot;/[]{},.<>|`</li></ul></div>" Text="*" Display="Dynamic" />
                                    <asp:CustomValidator ID="cvPassword" runat="server" ControlToValidate="Password" OnServerValidate="Password_Validating" Display="Static" ValidationGroup="AdminUserAccounts" Text="*" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right">
                                    <span class="formLabel">
                                        <asp:Label ID="lblconfirmpwd" runat="server" Text="Confirm Password" /></span>
                                </div>
                                <div class="col-sm-8 text-left">
                                    <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" MaxLength="20" CssClass="formField" aria-label="Confirm Password" />
                                    <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="* The Password and Confirmation Password must match" SetFocusOnError="true" Text="*" ValidationGroup="AdminUserAccounts" />
                                </div>
                            </div>
                        </div>
                        <br />
                        <uc2:Separator ID="ucSep4" runat="server" Header="Answer your security question." />
                        <br />
                        <div style="text-align: left; margin-left: 20%;" class="tablepad">
                            <div class="row">
                                <div class="col-sm-4 text-right"><span class="formLabel">
                                    <asp:Label ID="lblPasswordQuestion1" runat="server" AssociatedControlID="ddlPasswordQuestion1" Text="Security Question*" /></span></div>
                                <div class="col-sm-8 text-left fieldValue wd300">
                                    <asp:UpdatePanel ID="upSQ1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="ddlPasswordQuestion1" OnSelectedIndexChanged="ddlPasswordQuestion1_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True" aria-label="Choose first security question">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:CompareValidator runat="server" ID="CompareValidator1" ControlToValidate="ddlPasswordQuestion1"
                                        ValueToCompare="0" Type="String" ErrorMessage="* Security Question is required" Operator="NotEqual"
                                        SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="AdminUserAccounts" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right"><span class="formLabel">
                                    <asp:Label ID="lblAnswer1" runat="server" AssociatedControlID="Answer1" Text="Answer*" /></span></div>
                                <div class="col-sm-8 text-left">
                                    <asp:TextBox runat="server" ID="Answer1" CssClass="formField" aria-label="Enter first security answer" />
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator14" ControlToValidate="Answer1" ErrorMessage="* Answer is required" SetFocusOnError="true" Text="*" ValidationGroup="AdminUserAccounts" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right"><span class="formLabel">
                                    <asp:Label ID="lblPasswordQuestion2" runat="server" AssociatedControlID="ddlPasswordQuestion2" Text="Security Question*" /></span></div>
                                <div class="col-sm-8 text-left fieldValue wd300">
                                    <asp:UpdatePanel ID="upSQ2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList runat="server" ID="ddlPasswordQuestion2" OnSelectedIndexChanged="ddlPasswordQuestion2_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True" aria-label="Choose second security question">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:CompareValidator runat="server" ID="CompareValidator2" ControlToValidate="ddlPasswordQuestion2"
                                        ValueToCompare="0" Type="String" ErrorMessage="* Security Question is required" Operator="NotEqual"
                                        SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="AdminUserAccounts" />
                                    <asp:TextBox ID="TextBox3" runat="server" Visible="false" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-4 text-right"><span class="formLabel">
                                    <asp:Label ID="lblAnswer2" runat="server" AssociatedControlID="Answer2" Text="Answer*" ToolTip="Enter second security answer" /></span></div>
                                <div class="col-sm-8 text-left">
                                    <asp:TextBox runat="server" ID="Answer2" CssClass="formField" />
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="Answer2" ErrorMessage="* Answer is required" SetFocusOnError="true" Text="*" ValidationGroup="AdminUserAccounts" />
                                </div>
                            </div>
                        </div>
                        </div>
                        <div style="width: 100%; text-align: right; display: none;">
                            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="buttonBox" Text="Submit" CausesValidation="false" aria-label="Submit" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:Panel ID="pnlConfirmation" runat="server">
                <br />
                <uc2:Separator ID="ucSep5" runat="server" Header="Submission completed." />
                <br />
                Your online account registration was successful.
                        <br />
                <br />
                A confirmation email was sent to the email address used during registration.
                        <br />
                <br />
                Please refer to the email for instructions on activating your account.
                        <br />
                <br />
                <div style="width: 100%; height: 25px;">
                    <div style="border: 1px solid Black; width: auto; background-color: White; padding: 3px; float: right;">
                        <asp:HyperLink ID="lnkReturn" runat="server" NavigateUrl="~/Process/AdminHome.aspx" Text="Return to Provider Services Portal Home Page" ToolTip="Return to home page" />
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlChangesSaved" runat="server">
                <br />
                <uc2:Separator ID="Separator1" runat="server" Header="Saved" />
                <br />
                <asp:Label ID="lblchangesMsg" runat="server" Text="Your changes have been saved." />
                <br />
                <br />
                <div style="width: 100%; height: 25px;">
                    <div style="border: 1px solid Black; width: auto; background-color: White; padding: 3px; float: right;">
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Process/AdminHome.aspx" Text="Return to Provider Services Portal Home Page" ToolTip="Return to home page" />
                    </div>
                </div>
            </asp:Panel>
            <br />
            <div style="float: right;">
                <table class="tblButtons" role="presentation">
                    <div class="row">
                       <th style="visibility:hidden">table</th>
                        <td>
                            <asp:Button ID="btnPrevBtm" CssClass="buttonBox" runat="server" Text="Previous" OnClick="btnPrev" CausesValidation="false" ToolTip="Previous" /></td>
                        <td>
                            <asp:Button ID="btnSaveBtmUser" CssClass="buttonBox buttonBoxFocus" runat="server" Text="Save" OnClick="btnSubmit_Click" ToolTip="Save User" /></td>
                        <td>
                            <asp:Button ID="btnSaveBtmPerm" CssClass="buttonBox buttonBoxFocus" runat="server" Text="Save" OnClick="btnSubmitPermissions_Click" ToolTip="Save Permissions" /></td>
                        <td>
                            <asp:Button ID="btnCancBtm" CssClass="buttonBox" runat="server" Text="Cancel" OnClick="btnCancel" CausesValidation="false" ToolTip="Cancel" /></td>
                    </div>
                </table>
            </div>
        </div>
        <asp:TextBox ID="AddOrEdit" runat="server" Visible="false" ToolTip="hidden" />
        <asp:TextBox ID="EditUserName" runat="server" Visible="false" ToolTip="hidden" />
    </div>
    <ajax:ModalPopupExtender ID="mpeChangesSaved" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy" BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlModal">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlModal" runat="server" CssClass="identModalPopup" align="center" Style="display: none; padding: 20px; width: 200px;">
        <p><asp:Label ID="lblModal" runat="server" /></p>
        <br />
        <asp:Button runat="server" ID="btnModalOk" Text="OK" CssClass="buttonBox" OnClick="btnModalOk_Click" CausesValidation="false" ToolTip="OK" />
        <asp:Button runat="server" ID="btnModalCancel" Text="Cancel" CssClass="buttonBox" CausesValidation="false" ToolTip="Cancel" OnClick="btnModalOk_Click" />
    </asp:Panel>
    <asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" ToolTip="hidden" />
    <uc1:MessageBox ID="MessageBox2" runat="server" />
    <asp:HiddenField ID="hdnIsOHid" runat="server" />
</asp:Content>
