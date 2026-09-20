<%@ page language="C#" autoeventwireup="true" masterpagefile="~/MasterPage.master" inherits="Process_PasswordReset, App_Web_1rnu513f" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Src="FormField.ascx" TagName="FormField" TagPrefix="uc" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="ms" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccuctf" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="PageLabelContent">
    Password Reset
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>
   <script type="text/javascript">
       $(function () {
          var input = document.getElementById('<%= RP02.ClientID %>');
           $('<%= RP02.ClientID %>').keyup(function () {
              $('#<%= RP02.ClientID %>').blur();
           });
           input.addEventListener("keyup", function (event) {
               if (event.keyCode === 13) {
                   event.preventDefault();
                  <%-- $('<%= RP02.ClientID %>').blur();--%>
                   $('#<%= btnStep1.ClientID %>').click();                   
               }
           });
       });
       
   </script>
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
        .failureNotification
        {
            color:Red!important;
        }
        
        #ctl00_MainContent_lblRP02,
        #ctl00_MainContent_lblRP03
        {
            width: 250px; 
            float: right;
        }
    </style>
    <br /><br />
    <asp:UpdatePanel runat="server" ID="upReset" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="WhiteBox" style="text-align:center;">
        <asp:ValidationSummary ID="Step1ValidationSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="Step1" />
        <asp:ValidationSummary ID="Step2ValidationSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="Step2" />
            <%--<asp:Panel ID="pnlValidationSummary" runat="server" style="width:100%" Visible="false">
                <asp:Label ID="RP02_ERR" runat="server" Visible="false" Text="Unable to locate a user account with the User Name and/or Email Address entered." CssClass="failureNotification" /><br />
                <asp:Label ID="RP03_ERR05" runat="server" Visible="false" Text="Email address format incorrect: example johnsmith@mymd.com." CssClass="failureNotification" /><br />
                <asp:Label ID="RP03_ERR13" runat="server" Visible="false" Text="Unable to locate a user account with the User Name and/or Email Address entered." CssClass="failureNotification" /><br />
                <asp:Label ID="RP06_ERR" runat="server" Visible="false" Text="Password must be at least # characters long, must contain at least one capital letter and at least one special character." CssClass="failureNotification" /><br />
                <asp:Label ID="RP07_ERR" runat="server" Visible="false" Text="Passwords do not match." CssClass="failureNotification" /><br />
                <asp:Label ID="RP09_ERR" runat="server" Visible="false" Text="CAPTCH characters do not match." CssClass="failureNotification" /><br />
            </asp:Panel>--%>
            <asp:Panel ID="pnlStep1" runat="server">
                <div class="boxPanelData">
                    <div class="row">
                        <div class="col-sm-4 text-right">
                            <asp:Label ID="lblRP02" runat="server" Text="User ID" CssClass="formLabel300" />
                        </div>
                        <div class="col-sm-8 text-left">
                            <asp:TextBox ID="RP02" runat="server" CssClass="formField formField" MaxLength="80" ToolTip="User ID" />
                            <asp:RequiredFieldValidator ID="reqRP02" runat="server" ControlToValidate="RP02" Display="Dynamic" ValidationGroup="Step1" Text="*" ErrorMessage="* User ID is required" />
                            <asp:CustomValidator ID="cvRP02" runat="server" ControlToValidate="RP02" OnServerValidate="cvRP02_ServerValidate" Display="Static" ValidationGroup="Step1" Text="*" ErrorMessage="* User ID not found" />
                        </div>
                    </div>
                    <div>
                        <div class="col-sm-4 text-right">
                            <asp:Label ID="lblRP03" runat="server" Text="Email Address" CssClass="formLabel300" />
                        </div>
                        <div class="col-sm-8 text-left">
                            <asp:TextBox ID="RP03" runat="server" CssClass="formField formField" MaxLength="80" ToolTip="Account Email Address" />
                            <asp:RequiredFieldValidator ID="reqRP03" runat="server" ControlToValidate="RP03" Display="Dynamic" ValidationGroup="Step1" Text="*" ErrorMessage="* Email Address is required" />
                            <asp:CustomValidator ID="cvRP03" runat="server" ControlToValidate="RP03" OnServerValidate="RP03_Validating" Display="Static" ValidationGroup="Step1" Text="*" ErrorMessage="* Email Address not associated with the User ID" />
                        </div>
                    </div>
                </div>
                <div class="StepDiv"><asp:Button ID="btnStep1" runat="server" CssClass="buttonBox StepButton buttonBoxFocus" OnClick="btnStep1_Click" Text="Continue" CausesValidation="true" /></div>
            </asp:Panel>
            <asp:Panel ID="pnlStep2" runat="server" visible="false">
                <asp:Label ID="lblHidTry" runat="server" Visible="false" />
                <div class="boxPanelData">
               
                     <div class="row">
                        <div class="col-sm-4 text-right">
                            <asp:Label ID="lblRP05" runat="server" CssClass="formLabel300" />
                        </div>
                        <div class="col-sm-8 text-left">
                            <asp:TextBox ID="RP05" runat="server" CssClass="formField formField" MaxLength="80" />
                            <asp:RequiredFieldValidator ID="reqRP05" runat="server" ControlToValidate="RP05" Display="Dynamic" ValidationGroup="Step2" Text="*" ErrorMessage="* Security Answer is required" />
                            <asp:CustomValidator ID="cvRP05" runat="server" ControlToValidate="RP05" OnServerValidate="RP05_Validating" Display="Static" ValidationGroup="Step2" Text="*" ErrorMessage="* Security question answer does not match our records" />
                        </div>
                    </div>                 
              </div>
                <div class="StepDiv">
                    <div style="width:205px;height:40px;margin-left:auto; margin-right:auto;" runat="server" id="div1">
                         <div class="StepDiv"><asp:Button ID="btnSendEmail" runat="server" CssClass="buttonBox StepButton buttonBoxFocus" OnClick="btnSendEmail_Click" Text="Send Email" CausesValidation="true" /></div>
                    </div>
                </div>
            </asp:Panel>
                   
            <asp:Panel ID="PanelNewPassword" runat="server" visible="false">
               
                <div class="boxPanelData">
                    <div class="row">
                        <asp:Label ID="lblPasswordExists" runat="server" CssClass="failureNotification" Text ="" Visible ="false" />
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right">
                            <asp:Label ID="lblRP06" runat="server" Text="New Password" CssClass="formLabel300" />
                        </div>
                        <div class="col-sm-8 text-left">
                            <asp:TextBox ID="RP06" runat="server" CssClass="formField formField" MaxLength="80" TextMode="Password" />
                            <asp:RequiredFieldValidator ID="reqRP06" runat="server" ControlToValidate="RP06" Display="Dynamic" ValidationGroup="Step2" Text="*" ErrorMessage="* New Password is required" />
                            <asp:CustomValidator ID="cvRP06" runat="server" ControlToValidate="RP06" OnServerValidate="RP06_Validating" Display="Static" ValidationGroup="Step2" Text="*" ErrorMessage="* Password requires:<ul style=&quot;margin-top:0px!important;margin-bottom:-15px!important;&quot;><li>at least 8 characters</li><li>at most 10 characters</li><li>at least one lowercase letter</li><li>at least one uppercase letter</li><li>at least one number</li><li>at least one symbol or space !?.*</li></ul>" />
                            
                        </div>
                       <%-- <td>
                            <asp:Label ID="lblPasswordExists" runat="server" CssClass="failureNotification" Text ="" Visible ="false" />

                        </td>--%>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right">
                            <asp:Label ID="lblRP07" runat="server" Text="Confirm New Password" CssClass="formLabel300" />
                        </div>
                        <div class="col-sm-8 text-left">
                            <asp:TextBox ID="RP07" runat="server" CssClass="formField formField" MaxLength="80" TextMode="Password" />
                            <asp:RequiredFieldValidator ID="reqRP07" runat="server" ControlToValidate="RP07" Display="Dynamic" ValidationGroup="Step2" Text="*" ErrorMessage="* Confirm Password is required" />
                            <asp:CustomValidator ID="cvRP07" runat="server" ControlToValidate="RP07" OnServerValidate="RP07_Validating" Display="Static" ValidationGroup="Step2" Text="*" ErrorMessage="* Password fields must match" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4"></div>
                        <div class="col-sm-4">
                            <div class="StepDiv">
                                <ms:CaptchaControl style="margin-left:auto; margin-right:auto;" ID="Captcha1" runat="server" CaptchaBackgroundNoise="Low" CaptchaLength="5" CaptchaHeight="60" CaptchaWidth="200" CaptchaLineNoise="None" CaptchaMinTimeout="5" CaptchaMaxTimeout="240" FontColor = "#529E00" ToolTip="Captcha Control Type Text in Image" />
                            </div>
                        </div>
                        <div class="col-sm-4"></div>
                    </div><br />
                    <div class="row">
                        <div class="col-sm-4 text-right">
                            <asp:Label ID="lblRP09" runat="server" Text="Enter the characters you see above" CssClass="formLabel300" />
                        </div>
                        <div class="col-sm-8 text-left">
                            <asp:TextBox ID="RP09" runat="server" CssClass="formField formField" MaxLength="80" />
                            <asp:RequiredFieldValidator ID="reqRP09" runat="server" ControlToValidate="RP09" Display="Dynamic" ValidationGroup="Step2" Text="*" ErrorMessage="* Captcha is required" />
                            <asp:CustomValidator ID="cvRP09" runat="server" ControlToValidate="RP09" OnServerValidate="RP09_Validating" Display="Static" ValidationGroup="Step2" Text="*" ErrorMessage="* Captcha is incorrect" />
                        </div>
                    </div>
                </div>
                <div class="StepDiv">
                    <div style="width:205px;height:40px;margin-left:auto; margin-right:auto;" runat="server" id="divBtnContinueCancel">
                        <div style="width:100px; float:left;"><asp:Button id="btnCancel"  runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" /></div>
                        <div style="width:100px; float:left;"><asp:Button id="btnStep2"  runat="server" Text="Continue" CssClass="buttonBox buttonBoxFocus" onclick="btnContinue_Click" CausesValidation="true" /></div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlSuccess" Visible="false">
                <div style="margin-left:auto; margin-right:auto;text-align:center;">
                    <p>You have successfully changed your password.<br />Click LOG IN below to proceed to the Log In page.</p>
                </div>
                <div class="StepDiv"><asp:Button ID="btnLogin" runat="server" CssClass="buttonBox StepButton buttonBoxFocus" Text="LOG IN" OnClick="btnContinue_Click"  /></div>
            </asp:Panel>
            <asp:Panel ID="pnlLockedOut" runat="server" Visible="false">
                <p>
                    <asp:Literal ID="ltrlLockedOut" runat="server" Text="<%$ Resources:BrandingResource , pnlLockedOut %>" />
                </p>
            </asp:Panel>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <ccuctf:ModalPopupExtender ID="mpeChangesSaved" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy" CancelControlID="btnModalCancel" BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlModal">
    </ccuctf:ModalPopupExtender>
    <asp:Panel ID="pnlModal" runat="server" CssClass="identModalPopup" align="center" style="display:none; padding:20px;width:200px;">
        <p><asp:Label ID="lblModal" runat="server" Text="Are you sure you want to cancel? Your information/changes will not be saved. Click YES to confirm." /></p>
        <asp:Button runat="server" ID="btnModalOk" Text="YES" CssClass="buttonBox" OnClick="btnModalOk_Click" CausesValidation="false" />
        <asp:Button runat="server" ID="btnModalCancel" Text="Cancel" CssClass="buttonBox" CausesValidation="false" />
    </asp:Panel>
    <asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />
</asp:Content>