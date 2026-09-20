<%@ page title="User Account Creation" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Account_AccountCreation, App_Web_1rnu513f" enableEventValidation="false" stylesheettheme="Default" %>

<%@ Register Src="~/UserControls/SelectedControl.ascx" TagName="SelectedControl" TagPrefix="uc1" %>
<%@ Register Src="FormField.ascx" TagName="FormField" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccuctf" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="PageLabelContent">
    Create User Account
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //Update Panel needs after async postback, else event handlers are lost.
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_pageLoaded(setupEventHandlers);
            prm.add_pageLoaded(setupAccEventHandlers);
        });

        function setupEventHandlers() {
            $("#<%= txtConfirmTaxID.ClientID %>").bind("cut copy paste", function (e) {
                e.preventDefault();
            });
        }

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

        function setupAccEventHandlers() {
            $(".help-zipext").mouseover(function () {
                // .position() uses position relative to the offset parent, 
                var pos = $(this).position();

                // .outerWidth() takes into account border and padding.
                var width = $(this).outerWidth();

                //show the menu directly over the placeholder
                $("#helpZIPExtInfo").css({
                    position: "absolute",
                    top: pos.top + "px",
                    left: (pos.left + width) + "px"
                }).show();

            });

            $("#helpZIPExtInfo").mouseleave(function () {
                $("#helpZIPExtInfo").hide();
            });

        }
    </script>

    <div id="createAccount" class="WhiteBox">
        <asp:UpdatePanel runat="server" ID="upCreateAccount" UpdateMode="Conditional">
            <ContentTemplate>
                <table class="createAccountSteps" border="1" role="presentation">
                    <tr>
                        <td style="margin: 0px;">
                            <uc1:SelectedControl ID="ucStep1" runat="server" Text="Enter Provider Info" Selected="true" />
                        </td>
                        <td>
                            <uc1:SelectedControl ID="ucStep2" runat="server" Text="Create User ID & Password" Selected="false" />
                        </td>
                        <td>
                            <uc1:SelectedControl ID="ucStep3" runat="server" Text="Confirmation" Selected="false" />
                        </td>
                    </tr>
                </table>
                <asp:CreateUserWizard ID="wizCreateUser" DisplaySideBar="false" runat="server" OnCancelButtonClick="wizCreateUser_CancelButtonClick"
                    OnCreatedUser="wizCreateUser_CreatedUser" OnCreatingUser="wizCreateUser_CreatingUser"
                    NavigationButtonStyle-CssClass="buttonBox" OnActiveStepChanged="wizCreateUser_ActiveStepChanged"
                    StartNextButtonStyle-CssClass="buttonBox buttonBoxFocus margintoFive"
                    OnNextButtonClick="wizCreateUser_NextButtonClick" OnPreviousButtonClick="wizCreateUser_PreviousButtonClick"
                    CssClass="createUserForm" LoginCreatedUser="false" Width="100%"
                    CancelButtonStyle-CssClass="buttonBox" CancelButtonText="Cancel" DisplayCancelButton="true">
                    <WizardSteps>
                        <asp:WizardStep ID="wizStepIDInfo" runat="server" Title="Enter">
                            <div>
                                <br />
                                <div class="boxContainer"><asp:Label ID="lblHeader2" Text="Get started by filling out the form below" runat="server" CssClass="boxLabel" /></div>
                                <div class="pg-hint">* Designates a required field</div>
                                <br />
                                <br />
                                <asp:ValidationSummary ID="valRegister" DisplayMode="List" runat="server" CssClass="failureNotification val-summary"
                                    Visible="true" Enabled="true" ValidationGroup="Register" ShowSummary="true" />
                                <table class="tablepad" role="presentation">
                                    <tr>
                                        <td colspan="2">
                                            <p style="padding-left: 20px; padding-bottom: 10px;">The Tax ID you establish during your account setup process cannot be changed once your account is created. Please be careful to enter the correct Tax ID (EIN or SSN) when establishing your account. If you are an Individual Provider, you must enter your SSN as your primary tax identifier. You will have the opportunity to add your EIN (if applicable) later in the enrollment/re-enrollment process.  If you are a Group, Institution, or Facility you should enter your EIN. However, you may use your SSN as your primary Tax Identifier if you do not have an EIN. Existing providers that are intending to do a re-enrollment or an update will be required to enter additional identifying information before establishing an account. Once the Tax ID is established for your account, you will be able to enroll, re-enroll, and update other providers that may use this same Tax ID.</p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblTaxID" runat="server" Text="Tax ID*" CssClass="formLabel150" /></td>
                                        <td>
                                            <asp:TextBox ID="txtTaxID" runat="server" MaxLength="9" CssClass="formField" ToolTip="Tax ID" />
                                            <asp:RequiredFieldValidator ID="valTaxIDReqd" runat="server" ControlToValidate="txtTaxID"
                                                Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                                                ValidationGroup="Register" ErrorMessage="* Tax ID is required."></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="valTaxIdFormat" runat="server" ControlToValidate="txtTaxID"
                                                ValidationExpression="(?!0{9})(?!9{9})^\d{9}$" ErrorMessage="* Enter a 9 digit Tax ID."
                                                Enabled="true" SetFocusOnError="true" Text="*"
                                                ValidationGroup="Register" Display="Dynamic" />
                                            <asp:CustomValidator ID="cvConvertedFound" runat="server" OnServerValidate="ValidateConvertedFound" Enabled="false"
                                                Display="Static" ValidationGroup="Register" ErrorMessage="* The Tax ID is associated with an existing provider, however the additional information supplied does not match to an existing provider in the our system.  Please contact the call center for support."
                                                Text="*" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblConfirmTaxID" runat="server" Text="Confirm Tax ID*" CssClass="formLabel150" /></td>
                                        <td>
                                            <asp:TextBox ID="txtConfirmTaxID" runat="server" MaxLength="9" CssClass="formField" ToolTip="Confirm Tax ID" />
                                            <asp:RequiredFieldValidator ID="valTaxIDCReqd" runat="server" ControlToValidate="txtConfirmTaxID"
                                                Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                                                ValidationGroup="Register" ErrorMessage="* Confirm Tax ID is required."></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="valConfirmTaxIdFormat" runat="server" ControlToValidate="txtConfirmTaxID"
                                                ValidationExpression="(?!0{9})(?!9{9})^\d{9}$" ErrorMessage="* Enter a 9 digit Tax ID in Confirm Tax ID."
                                                Enabled="true" SetFocusOnError="true" Text="*"
                                                ValidationGroup="Register" Display="Dynamic" />
                                            <asp:CompareValidator ID="valTaxIDCompare" runat="server" ControlToCompare="txtTaxID"
                                                ControlToValidate="txtConfirmTaxID" Display="Dynamic" Text="*"
                                                ErrorMessage="* The Tax ID and Confirm Tax ID do not match.  Please verify you have entered the correct Tax ID."
                                                ValidationGroup="Register" Operator="Equal" />
                                        </td>

                                    </tr>

                                    <tr>
                                        <td></td>
                                        <td colspan="2">
                                            <fieldset>
                                                <legend>Tax ID Type*</legend>
                                                <asp:RadioButtonList ID="rblTaxIDType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                                    <asp:ListItem Selected="False" Text="EIN" Value="16"></asp:ListItem>
                                                    <asp:ListItem Selected="False" Text="SSN" Value="15"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </fieldset>
                                            <asp:RequiredFieldValidator ID="valTaxIDType" runat="server" ControlToValidate="rblTaxIDType" Enabled="true"
                                                SetFocusOnError="true" Display="Static" Text="*" ValidationGroup="Register" ErrorMessage="* Tax ID Type is required."></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                </table>
                                <div id="divExistingFound" runat="server" class="wdAll error-message" style="padding-left: 20px; padding-bottom: 10px;">
                                    <asp:Literal ID="ltlExistingFound" runat="server" Text=" <%$ Resources:BrandingResource , ACCOUNT_CREATION_EXISTING_FOUND %> " />
                                </div>
                                <table class="tablepad" style="margin-left: 17.5%;" role="presentation">
                                    <tr id="trNPI" runat="server">
                                        <td style="text-align: right">NPI</td>
                                        <td>
                                            <asp:TextBox ID="txtNPI" runat="server" MaxLength="10" CssClass="formField" /><asp:Literal ID="ltlNPIApplicable" runat="server" Text="(if applicable)"></asp:Literal>
                                            <asp:RegularExpressionValidator ID="valNPI" runat="server" ControlToValidate="txtNPI"
                                                ValidationExpression="^([1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$" ErrorMessage="* Enter a 10 digit NPI that does not begin with 0."
                                                Enabled="true" SetFocusOnError="true" Text="*"
                                                ValidationGroup="Register" Display="Dynamic" />
                                            <asp:CustomValidator ID="cvNPI" runat="server" OnServerValidate="ValidateConvertedFoundOnNPI" Enabled="false"
                                                Display="Static" ValidationGroup="Register" ErrorMessage="* The NPI does not match the SSN you have entered."
                                                Text="*" />
                                            <asp:CustomValidator ID="cvRequiredNPI" runat="server" OnServerValidate="ValidateRequiredNPI" Enabled="false"
                                                Display="Static" ValidationGroup="Register" ErrorMessage="* NPI is required."
                                                Text="*" />
                                             <asp:CustomValidator ID="cvValidateType1NPI" runat="server" OnServerValidate="ValidateType1NPI" Enabled="false"
                                                Display="Static" ValidationGroup="Register" ErrorMessage="* The NPI entered must be a Type 1 NPI."
                                                Text="*" />
                                        </td>
                                    </tr>
                                    <tr id="trTaxonomy" runat="server">
                                        <td style="text-align: right">Taxonomy Code</td>
                                        <td>
                                            <asp:TextBox ID="txtTaxonomyCode" runat="server" MaxLength="80" CssClass="formField" />(if applicable) </td>
                                    </tr>
                                    <tr id="trZip" runat="server">
                                        <td style="text-align: right">Zip Code*</td>
                                        <td>
                                            <asp:TextBox ID="txtZipCode" runat="server" MaxLength="5" CssClass="formField" />
                                            <asp:RequiredFieldValidator ID="valZipReqd" runat="server" ControlToValidate="txtZipCode"
                                                Enabled="false" SetFocusOnError="true" Display="Dynamic" Text="*"
                                                ValidationGroup="Register" ErrorMessage="* Zip Code is required."></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="valZipFormat" runat="server" Text="*"
                                                ErrorMessage="* Enter 5 digits for zip code" ControlToValidate="txtZipCode" SetFocusOnError="true"
                                                Display="Dynamic" ValidationExpression="\d{5}$" ValidationGroup="Register" />
                                        </td>
                                    </tr>
                                    <tr id="trZipExt" runat="server">
                                        <td style="text-align: right">Zip Code Extension*</td>
                                        <td>
                                            <asp:TextBox ID="txtZipCodeExt" runat="server" MaxLength="4" CssClass="formField" />
                                            <asp:RequiredFieldValidator ID="valZipExtRqd" runat="server" ControlToValidate="txtZipCodeExt"
                                                Enabled="false" SetFocusOnError="true" Display="Dynamic" Text="*"
                                                ValidationGroup="Register" ErrorMessage="* Zip Code Extension is required."></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="valZipExtFormat" runat="server" Text="*"
                                                ErrorMessage="* Enter 4 digits for zip code extension" ControlToValidate="txtZipCodeExt" SetFocusOnError="true"
                                                Display="Dynamic" ValidationExpression="\d{4}$" ValidationGroup="Register" />
                                            <div id="helpZIPExt" class="help-zipext" style="cursor: pointer; display: inline-block;">
                                                <asp:Image ID="imgHelpTaxID" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" />
                                            </div>
                                            <div id="helpZIPExtInfo" class="infoBox" style="top: 0; right: 0; width: 480px">

                                                <div class="infoContent">
                                                    <asp:Literal ID="ltlhelpZIPExtHelp" runat="server" Text="<%$ Resources:BrandingResource , KEY_FIELD_ZIP_EXT_HELPTEXT %>"></asp:Literal>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr id="trMedicaidID" runat="server">
                                        <td style="text-align: right">Medicaid ID*</td>
                                        <td>
                                            <asp:TextBox ID="txtMedicaidID" runat="server" MaxLength="13" CssClass="formField" />
                                            <asp:RequiredFieldValidator ID="valMediaidIDReqd" runat="server" ControlToValidate="txtMedicaidID"
                                                Enabled="false" SetFocusOnError="true" Display="Dynamic" Text="*"
                                                ValidationGroup="Register" ErrorMessage="* Medicaid ID is required. Please include leading zeroes as applicable."></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr id="trMedicaidInfo" runat="server">
                                        <td></td>
                                        <td class="pg-hint2" style="text-align: left;">(* Please enter the exact number provided including leading zeroes.)</td>
                                    </tr>

                                </table>
                            </div>
                            </div>
                                <customnavigationtemplate>
                                <br />
                                <asp:UpdateProgress id="updateProgress" runat="server">
                                    <ProgressTemplate>
                                        <div style="padding-left:30px;text-align:right;">
                                            <img src="../Images/ajax-loader.gif" alt="AJAX Loader" /></div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                </customnavigationtemplate>
                        </asp:WizardStep>

                        <asp:CreateUserWizardStep ID="wizStepProfileInfo" runat="server" Title="Create Profile">
                            <ContentTemplate>
                                <br />
                                <div class="boxContainer"><asp:Label ID="lblHeader2" Text="Please enter your contact information" runat="server" CssClass="boxLabel" /></div>
                                <div class="pg-hint">* Designates a required field</div>
                                <br />
                                <div style="text-align: center;">
                                    <asp:ValidationSummary ID="valRegisterUserValidationSummary" runat="server" CssClass="failureNotification val-summary"
                                        ValidationGroup="RegisterUserValidationGroup" DisplayMode="List" />
                                    <table border="0" style="text-align: left; margin-left: 20%;" class="tablepad" role="presentation">
                                        <tr>
                                            <td>
                                                <span class="formLabel">Contact Name*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtContactName" runat="server" MaxLength="50" CssClass="formField"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="valContactRequired" runat="server" ControlToValidate="txtContactName"
                                                    ValidationGroup="RegisterUserValidationGroup" Display="Dynamic" Text="*"
                                                    ErrorMessage="* Contact Name is required"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="valContactFormat" runat="server" Display="Dynamic" Text="*"
                                                    ErrorMessage="* Contact Name: Invalid character found." ControlToValidate="txtContactName"
                                                    SetFocusOnError="true" ValidationExpression="^[0-9a-zA-Z''-'\s]{1,50}$"
                                                    ValidationGroup="RegisterUserValidationGroup" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="formLabel">Title*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTitle" runat="server" MaxLength="50" CssClass="formField"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="valTitleRequired" runat="server" ControlToValidate="txtTitle"
                                                    ValidationGroup="RegisterUserValidationGroup" ErrorMessage="* Title is required." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="valTitleFormat" runat="server"
                                                    ErrorMessage="* Title: Invalid character found." ControlToValidate="txtTitle"
                                                    SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="RegisterUserValidationGroup"
                                                    ValidationExpression="^[a-zA-Z''-'\s]{1,50}$" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="formLabel">Phone Number*</span>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="10" CssClass="formField" />
                                                <ccuctf:MaskedEditExtender runat="server" ID="meePhoneNumber" AutoComplete="False" ClearMaskOnLostFocus="False"
                                                    TargetControlID="txtPhone" MaskType="Number" Mask="(999) 999-9999" CultureAMPMPlaceholder=""
                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                                                <asp:CustomValidator ID="cvPhoneNum" runat="server" SetFocusOnError="True" Display="Dynamic"
                                                    ControlToValidate="txtPhone" ClientValidationFunction="CheckPhoneLength"
                                                    ErrorMessage="Enter valid Phone Number" Text="*" ValidationGroup="RegisterUserValidationGroup" />
                                                <asp:RegularExpressionValidator ID="valPhoneFormat" runat="server" ControlToValidate="txtPhone"
                                                    Display="Dynamic" Text="*" ValidationExpression="^((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}$"
                                                    ErrorMessage="* Invalid phone format.  Format should be (999) 999-9999." ValidationGroup="RegisterUserValidationGroup"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="formLabel">Extension</span></td>
                                            <td>
                                                <asp:TextBox ID="txtPhoneExt" runat="server" MaxLength="10" CssClass="formField"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="formLabel">Email Address*</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="Email" CssClass="formField300" />
                                                <asp:RequiredFieldValidator ID="valEmailRequired" runat="server" ControlToValidate="Email"
                                                    ValidationGroup="RegisterUserValidationGroup" ErrorMessage="* Email is required." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="valEmailFormat" runat="server" ControlToValidate="Email" Display="Dynamic" Text="*"
                                                    ValidationExpression="^([0-9a-zA-Z+]([-.\w]*[0-9a-zA-Z+])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                                    ErrorMessage="* Invalid email format." ValidationGroup="RegisterUserValidationGroup"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <span class="formLabel">Confirm Email*</span></td>
                                            <td>
                                                <asp:TextBox runat="server" ID="ConfirmEmail" CssClass="formField300" />
                                                <asp:RequiredFieldValidator ID="valConfirmEmailRequired" runat="server" ControlToValidate="ConfirmEmail"
                                                    ValidationGroup="RegisterUserValidationGroup" ErrorMessage="* Confirm Email is required." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                                                <asp:CompareValidator ID="valEmailsCompare" runat="server" ControlToValidate="ConfirmEmail" ControlToCompare="Email"
                                                    Type="String" Operator="Equal" Display="Dynamic" Text="*"
                                                    ValidationGroup="RegisterUserValidationGroup" ErrorMessage="* Email addresses must match."></asp:CompareValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <div class="boxContainer"><asp:Label ID="Label1" Text="Create your user id and password" runat="server" CssClass="boxLabel" /></div>
                                <br />
                                <table border="0" style="text-align: left; margin-left: 20%;" class="tablepad">
                                    <tr>
                                        <td>
                                            <span class="formLabel">User ID*</span>
                                        </td>
                                        <td>

                                            <asp:TextBox runat="server" ID="UserName" CssClass="formField" MaxLength="50" />
                                            <asp:RequiredFieldValidator ID="valUserIDRequired" runat="server" ControlToValidate="UserName"
                                                ValidationGroup="RegisterUserValidationGroup" ErrorMessage="* User ID is required." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="valUserIDExists" runat="server" OnServerValidate="Validate_UserNameExists"
                                                ControlToValidate="UserName" Display="Dynamic" Text="*"
                                                ValidationGroup="RegisterUserValidationGroup" ErrorMessage="* User ID is not available." />
                                            <asp:RegularExpressionValidator ID="valUserIDFormat" runat="server" ControlToValidate="UserName"
                                                ValidationExpression="^[a-zA-Z0-9@.]+$"
                                                ErrorMessage="* User ID:<ul style=&quot;margin-top:-5px!important&quot;><li>May contain uppercase letters</li><li>May contain lowercase letters</li><li>May contain numbers</li><li>May contain @ symbols and periods</li><li>At least one character long</li><li>At most 50 characters long</li></ul>"
                                                ValidationGroup="RegisterUserValidationGroup" Text="*" Display="Dynamic"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="formLabel">Password*</span>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="formField" MaxLength="10" />
                                            <asp:RequiredFieldValidator ID="valPasswordRequired" runat="server" ControlToValidate="Password"
                                                ErrorMessage="* Password is required." Display="Dynamic" Text="*"
                                                ValidationGroup="RegisterUserValidationGroup" />
                                            <asp:RegularExpressionValidator ID="valPasswordFormat" runat="server" ValidationGroup="RegisterUserValidationGroup"
                                                ControlToValidate="Password" Display="Dynamic"
                                                ValidationExpression="^(?=.*[0-9])(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[!@#$%\^&*\(\)\-_+=;:'\/\[\]{},.<>|`]).{8,20}$"
                                                ErrorMessage="<div class='pwd-val-error'>*Password is invalid. Password requirements:<ul><li>Between 8 and 20 characters</li><li>Contain at least one non-alphanumeric character</li><li>Contain at least one lowercase letter</li><li>Contain at least one uppercase letter</li></ul></div>" Text="*" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span class="formLabel">Confirm Password*</span></td>
                                        <td>
                                            <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" MaxLength="10" CssClass="formField" />
                                            <asp:RequiredFieldValidator ID="valConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword"
                                                Display="Dynamic" Text="*" ErrorMessage="* Confirm Password is required."
                                                ValidationGroup="RegisterUserValidationGroup" />
                                            <asp:CompareValidator ID="valPasswordCompare" runat="server" ControlToCompare="Password"
                                                ControlToValidate="ConfirmPassword" Display="Dynamic" Text="*"
                                                ErrorMessage="* The Password and Confirmation Password must match."
                                                ValidationGroup="RegisterUserValidationGroup" Operator="Equal" />

                                        </td>
                                    </tr>
                                </table>

                                <div class="boxContainer"><asp:Label ID="Label2" Text="Answer your security question" runat="server" CssClass="boxLabel" /></div>
                                <br />
                                <table border="0" style="text-align: left; margin-left: 20%;" class="tablepad">
                                    <tr>
                                        <td><span class="formLabel">Security Question*</span></td>
                                        <td>
                                            <asp:UpdatePanel ID="upSecurity" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlPasswordQuestion1" CssClass="formDropDown"
                                                        OnSelectedIndexChanged="ddlPasswordQuestion1_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true" >
                                                    </asp:DropDownList>
                                                    <asp:CompareValidator runat="server" ID="valQuestion1Required" ControlToValidate="ddlPasswordQuestion1"
                                                        ValueToCompare="0" Type="Integer" ErrorMessage="* Both security questions are required."
                                                        Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*"
                                                        ValidationGroup="RegisterUserValidationGroup" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><span class="formLabel">Answer*</span></td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtAnswer1" CssClass="formField" />
                                            <asp:RequiredFieldValidator ID="valAnswer1Required" runat="server" ControlToValidate="txtAnswer1"
                                                Display="Dynamic" Text="*" SetFocusOnError="true"
                                                ErrorMessage="* Both security answers are required." ValidationGroup="RegisterUserValidationGroup" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><span class="formLabel">Security Question*</span></td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList runat="server" ID="ddlPasswordQuestion2" CssClass="formDropDown"
                                                        OnSelectedIndexChanged="ddlPasswordQuestion2_SelectedIndexChanged" AutoPostBack="true"  AppendDataBoundItems="true" >
                                                    </asp:DropDownList>
                                                    <asp:CompareValidator runat="server" ID="valQuestion2Required" ControlToValidate="ddlPasswordQuestion2"
                                                        ValueToCompare="0" Type="Integer" ErrorMessage="* Both security questions are required."
                                                        Operator="NotEqual" ValidationGroup="RegisterUserValidationGroup"
                                                        SetFocusOnError="true" Display="Dynamic" Text="*" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><span class="formLabel">Answer*</span></td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtAnswer2" CssClass="formField" />
                                            <asp:RequiredFieldValidator runat="server" ID="valAnswer2Required" ControlToValidate="txtAnswer2"
                                                Display="Dynamic" SetFocusOnError="true" Text="*"
                                                ErrorMessage="* Both security answers are required." ValidationGroup="RegisterUserValidationGroup" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <CustomNavigationTemplate>
                                <br />
                                <asp:UpdateProgress ID="updateProgress" runat="server">
                                    <ProgressTemplate>
                                        <div style="padding-right: 30px">
                                            <img src="../Images/ajax-loader.gif" />
                                            Saving...
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <asp:Button ID="StepPreviousButton" runat="server" CommandName="MovePrevious" Text="Previous"
                                    CssClass="buttonBox" ToolTip="Save and move to the Previous Section" UseSubmitBehavior="false" />
                                <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" Text="Register"
                                    CssClass="buttonBox buttonBoxFocus" ToolTip="Create User and move to Confirmation" CausesValidation="true"
                                    ValidationGroup="RegisterUserValidationGroup" UseSubmitBehavior="true" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonBox" CommandName="Cancel" ToolTip="Cancel and return to the Log In page" Text="Cancel" />
                            </CustomNavigationTemplate>
                        </asp:CreateUserWizardStep>

                        <asp:CompleteWizardStep ID="wzStepComplete" runat="server">
                            <ContentTemplate>
                                <div style="width: 600px">
                                    <br />
                                    <div class="boxContainer"><asp:Label ID="lblHeader2" Text="Confirmation - Next Steps" runat="server" CssClass="boxLabel" /></div>
                                    <br />
                                    Your online account creation was successful.
                                    <br />
                                    <br />
                                    <asp:Label ID="lblEmailSent" runat="server" Text="A confirmation email was sent to the email address used during account creation.<br><br>Please refer to the email for instructions on activating your account."></asp:Label>
                                    <asp:Label ID="lblEmailFailed" runat="server" Text="However, sending an email to the email address used during account creation failed.  Please contact user support to activate your account."></asp:Label>
                                    <br />
                                    <br />
                                </div>
                                <div style="width: 100%">
                                    <div style="float: right;">
                                        <asp:Button ID="btnReturn" runat="server" CssClass="buttonBox buttonBoxFocus" Text="Return to Home Page" OnClick="btnReturn_Click" />
                                    </div>
                                </div>
                                </div>
                            </ContentTemplate>
                        </asp:CompleteWizardStep>
                    </WizardSteps>
                    <CancelButtonStyle CssClass="buttonBox" />
                    <NavigationButtonStyle CssClass="buttonBox margintoFive"></NavigationButtonStyle>
                    <StartNextButtonStyle CssClass="buttonBox buttonBoxFocus margintoFive" />
                </asp:CreateUserWizard>
                <uc:MessageBox ID="MessageBox2" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <ccuctf:ModalPopupExtender ID="mpeChangesSaved" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy" CancelControlID="btnModalCancel" BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlModal">
    </ccuctf:ModalPopupExtender>
    <asp:Panel ID="pnlModal" runat="server" CssClass="identModalPopup" align="center" Style="display: none; padding: 20px; width: 200px;">
        <p>
            <asp:Label ID="lblModal" runat="server" Text="Are you sure you want to cancel? Your information/changes will not be saved. Click YES to confirm." />
        </p>
        <asp:Button runat="server" ID="btnModalOk" Text="YES" CssClass="buttonBox" OnClick="btnModalOk_Click" CausesValidation="false" />
        <asp:Button runat="server" ID="btnModalCancel" Text="Cancel" CssClass="buttonBox" CausesValidation="false" />
    </asp:Panel>
    <asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />
</asp:Content>
