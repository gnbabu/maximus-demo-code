<%@ Page Title="Recover User ID" Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="UserNameRecovery" Codebehind="UserNameRecovery.aspx.cs" %>
<%@ Register Src="FormField.ascx" TagName="FormField" TagPrefix="uc" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="ms" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccuctf" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="PageLabelContent">
    <br /><br />
    Recover User ID
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <style type="text/css">

        .fieldTable>tbody>tr>td:nth-child(1)
        {
            width:50%;
        }
        
        .fieldTable>tbody>tr>td>.formField
        {
            border:solid 1px black!important;
        }
    
        .StepDiv
        {
            width:100%; 
            text-align:center; 
            margin-top:20px;
        }
        
        .StepButton
        {
            margin-left:auto; 
            margin-right:auto;
            text-align:center;
        }
</style>
    <script type="text/javascript">
        function numericOnly(obj) {
            obj.value = obj.value.replace(/[^0-9]/g, '');
        }
        function alphanumericOnly(obj) {
            obj.value = obj.value.replace(/[^a-zA-Z0-9]/g, '');
        } 
    </script>
    <br />
    <br />
    <asp:UpdatePanel runat="server" ID="upReset" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="WhiteBox">
             <span style="color: #df2012; font-size: 14pt !important; font-weight: 100 !important;">An asterisk * indicates a required field</span>
            <asp:ValidationSummary ID="Step1ValidationSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="Step1" />
            <asp:Panel ID="pnlValidationSummary" runat="server" style="width:100%" Visible="false">
                <%--<asp:Label ID="UNR02_ERR" runat="server" Visible="false" Text="Email address format incorrect: example johnsmith@mymd.com" CssClass="failureNotification" />--%><%--<asp:Label ID="UNR0406_ERR" runat="server" Visible="false" Text="Either the Group NPI or the Medicaid ID must be entered." CssClass="failureNotification" />--%><%--<asp:Label ID="UNR09_ERR" runat="server" Visible="false" Text="Must match captcha characters." CssClass="failureNotification" />--%>
                <asp:Label ID="UNR11_ERR" runat="server" style="color:Red!important;" Visible="false" Text="* We are unable to locate your user account with the information entered." CssClass="failureNotification" />
            </asp:Panel>
            <asp:Panel ID="pnlStep1" runat="server">
                <div class="boxPanelData">
                  <div class="row">
                    <div class="col-sm-4 text-right">
                        <span class="formLabel"><asp:Label ID="lblMedID" runat="server" Text="Medicaid ID*"/></span>
                    </div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtMedicaidID" runat="server" MaxLength="13" CssClass="formField" aria-required="true" aria-label="medicaid id" ToolTip="Medicaid Id" />
                        <asp:RequiredFieldValidator ID="valMediaidIDReqd" runat="server" ControlToValidate="txtMedicaidID"
                            Enabled="false" SetFocusOnError="true" Display="Dynamic" Text="*"
                            ValidationGroup="flValGroup" ErrorMessage="* Medicaid ID is required. Please include leading zeroes as applicable."></asp:RequiredFieldValidator>
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-sm-4 text-right">
                        <span class="formLabel"><asp:Label ID="lblEmail" runat="server" Text="Email Address*"/></span>
                    </div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox runat="server" ID="txtEmail" CssClass="formField" aria-required="true" aria-label="email" Tooltip="Email Address" />
                        <asp:RequiredFieldValidator ID="valEmailRequired" runat="server" ControlToValidate="txtEmail"
                            ValidationGroup="flValGroup" ErrorMessage="* Email is required." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="valEmailFormat" runat="server" ControlToValidate="txtEmail" Display="Dynamic" Text="*"
                            ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                            ErrorMessage="* Invalid email format." ValidationGroup="flValGroup"></asp:RegularExpressionValidator>
                    </div>
                  </div>
                  <div class="row">
                    <div class="col-sm-4  text-right">
                    <span class="formLabel"><asp:Label ID="lblconfirmEmail" runat="server" Text="Confirm Email" /></span>
                    </div>
                    <div class="col-sm-8 text-left">
                    <asp:TextBox runat="server" ID="txtConfirmEmail" CssClass="formField" Tooltip="Confirm Email Address" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtConfirmEmail"
                    ValidationGroup="flValGroup" ErrorMessage="*Confirm Email is required." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtConfirmEmail" Display="Dynamic" Text="*"
                            ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                            ErrorMessage="* Invalid email format." ValidationGroup="flValGroup"></asp:RegularExpressionValidator>
                    <asp:CustomValidator ID="cvCEmailMatch" runat="server" OnServerValidate="Validate_cvCEmailMatch"
                        Display="Dynamic" ValidationGroup="flValGroup" ErrorMessage="* Email addresses must match."
                        Text="* Email addresses must match." />
                        </div>
                  </div>       
                    <div class="row">
                        <div class="col-sm-4"></div>
                        <div class="col-sm-4">
                            <div class="StepDiv" tabindex="0">
                                <ms:CaptchaControl style="margin-left:auto; margin-right:auto;" ID="Captcha1" runat="server" 
                                        CaptchaBackgroundNoise="Low" CaptchaLength="5" 
                                        CaptchaHeight="60" CaptchaWidth="200" 
                                        CaptchaLineNoise="None" CaptchaMinTimeout="5" 
                                        CaptchaMaxTimeout="240" FontColor = "#529E00" ToolTip="Captcha Control Type Text in Image" />
                            </div>
                        </div>
                        <div class="col-sm-4"></div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-4  text-right">
                            <asp:Label ID="lblUNR09" runat="server" Text="Enter the characters you see above*" CssClass="formLabel300" />
                        </div>
                        <div class="col-sm-8 text-left">
                            <asp:TextBox ID="UNR09" runat="server" CssClass="formField formField" aria-required="true"  ToolTip="Enter the characters you see above" />
                            <asp:RequiredFieldValidator ID="reqUNR09" runat="server" ControlToValidate="UNR09" Display="Dynamic" ValidationGroup="Step1" Text="*" ErrorMessage="* Captcha is required" />
                            <asp:CustomValidator ID="cvUNR09" runat="server" ControlToValidate="UNR09" OnServerValidate="ValidateUNR09" Display="Static" ValidationGroup="Step1" Text="*" ErrorMessage="* Must match captcha characters" />
                        </div>
                    </div>
                     <div class="btnBox btnBoxCenter" id="divBtnContinueCancel" runat="server">
                                <asp:Button ID="btnSend" runat="server" Text="Submit" CssClass="buttonBox buttonBoxFocus" CausesValidation="false" OnClick="btnSend_Click" />
                                <asp:Button id="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel" CssClass="buttonBox" CausesValidation="false" />

                    </div>
              </div>
                <%--</table>--%>
                <br /> <br /> <br />
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlSuccess" style="width:100%;">
                <div style="margin-left:auto; margin-right:auto;text-align:center;">
                    <p>
                Your user name has been emailed to the email address we have on record.<br />
                
                    </p>
                </div>
                <div class="StepDiv">
                    <asp:Button ID="btnLogin" runat="server" CssClass="buttonBox StepButton" Text="LOG IN" OnClick="btnLogin_Click" />
                </div>
            </asp:Panel>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <ccuctf:ModalPopupExtender ID="mpeChangesSaved" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy" CancelControlID="btnModalCancel" BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlModal">
    </ccuctf:ModalPopupExtender>
    <asp:Panel ID="pnlModal" runat="server" CssClass="identModalPopup" align="center" style="display:none; padding:20px;width:200px;">
        <p>
            <asp:Label ID="lblModal" runat="server" Text="Are you sure you want to cancel? Your information/changes will not be saved. Click YES to confirm." />
        </p>
        <div class="btnBox">
        <asp:Button runat="server" ID="btnModalOk" Text="YES" CssClass="buttonBox" OnClick="btnModalOk_Click" CausesValidation="false" />
        <asp:Button runat="server" ID="btnModalCancel" Text="Cancel" CssClass="buttonBox" CausesValidation="false" />
        </div>
    </asp:Panel>
    <asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy"/>
</asp:Content>