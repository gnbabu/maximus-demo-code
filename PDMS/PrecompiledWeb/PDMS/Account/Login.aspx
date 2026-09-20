<%@ page title="Log In" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Account_Login, App_Web_zwaluqkv" enableEventValidation="false" stylesheettheme="Default" %>

<%@ Register Src="../UserControls/UserProfile.ascx" TagName="UserProfile" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/BreakingNews.ascx" TagName="BreakingNews" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="PageLabelContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
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
                    event.keyCode = 0
                }
            }
        }

      //function checkchanged(obj) {            
      //      var chk = document.getElementById(obj);
      //      var chkbox = chk.getElementsByTagName("input");
      //      document.getElementById("btnCancelTerms").disabled = chk.checked ? true : false;
      //  }

    </script>
    <style>
        .ohLoginButton {
    color: #fff;
    background-color: #435363 !important;
    border-color: #435363;
    background-image: none;
    font-weight: bold;
    padding: 5px 10px;
    line-height: 1.5 !important;
    border-radius: 3px !important;
    text-shadow: none !important;
    width: auto;
    min-width: 300px;
    height: 50px;
}

    .ohLoginButton:hover {
        opacity: 0.5;
        background-image: none;
        background-color: #435363;
    }

    .ohLoginButton:disabled, .ohLoginButton input[type=submit]:disabled, .ohLoginButton input[type=submit][disabled=disabled], .ohLoginButton button[disabled=disabled] {
        opacity: 0.2;
    }

    .ohLoginButton:active {
        color: #fff;
        background-color: #435363;
        border-color: #435363;
        line-height: 1.5 !important;
        padding: 5px 10px;
        background: none #31b0d5 !important;
    }

        .ohLoginButton:active:focus, .ohLoginButton.active:focus, .ohLoginButton.active:hover, .ohLoginButton:active.focus, .ohLoginButton:active:focus, .ohLoginButton:active:hover {
            color: #fff;
            background-color: #435363;
            border-color: #435363;
            line-height: 1.5 !important;
            padding: 5px 10px;
            background: none #269abc !important;
        }
        main h1{font-size:1.5em;margin:0.25em 0 0 0;padding:0}
        main h2{color:#000;}
        main .attentionBox {
            font-size:14pt;
            background-color:#f9a02d;
            color:#000;
            padding:0.25em;
            text-align:left;
            margin-top:1em;
            font-weight:bold;
        }
        .lblLegalAgreementCSS{
            word-wrap: break-word;
        }
    </style>
    <div class="container-fluid">
        <div class="row">            
            <div class="col-sm-12">
                <div class="row">
                    <div class="col-sm-4"></div>
                    <div class="col-sm-8">
                        <asp:Panel ID="pnlFailureText" runat="server" Visible="false">
                            <span class="error-message" style="font-size:18pt;font-weight:bold">
                                <asp:Literal ID="FailureTextInvalid" runat="server"></asp:Literal>
                            </span>
                        </asp:Panel>
                    </div>
                </div>
                <div class="LoginBox alignCenter" id="divLogin" runat="server">

                    <div id="newLogin" runat="server">
                        <asp:Login ID="Login1" runat="server" FailureTextStyle-CssClass="failureNotification" RenderOuterTable="false" OnLoggingIn="LoginUser_LoggingIn" OnLoginError="Login1_LoginError" OnLoggedIn="Login1_LoggedIn">
                            <LayoutTemplate>
                                <asp:Panel ID="pnlLogin" runat="server" CssClass="accountInfo" DefaultButton="LoginButton" Visible="false">
                                    <section class="login" aria-label="Login Information">
                                        <br />
                                        <div class="row">
                                            <div class="col-sm-2">
                                                <strong><h1>Login</h1></strong>
                                            </div>
                                        </div>
                                        <br />
                                        <div>
<%--                                            <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification" ValidationGroup="LoginUserValidationGroup" DisplayMode="List" />--%>
                                            <span class="failureNotification" role="alert" aria-atomic="true">
                                                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                                            </span>
                                        </div>
                                        <div style="margin-left: auto; margin-right: auto; text-align: center;" aria-live="polite" >
                                            <%-- <span class="formLabel" style="width: 150px;">User ID</span>--%>
                                           <strong><asp:Label ID="lblUserName" runat="server" Text="Please enter your User ID" CssClass="formLabel300" AssociatedControlID="UserName"></asp:Label></strong>
                                            <asp:TextBox ID="UserName" runat="server" CssClass="formField" ToolTip="User ID"></asp:TextBox>
                                            <div tabindex="0" class="error-wrapper">
                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" class="failureNotification"
                                                    ErrorMessage="* User ID is required." ToolTip="User ID is required"
                                                    ValidationGroup="LoginUserValidationGroup" SetFocusOnError="false" aria-hidden="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <asp:Panel ID="pnlpswd" runat="server" Visible="false" aria-live="polite">
                                            <%--<span class="formLabel" style="width: 150px;">Password</span>--%>
                                            <asp:Label ID="lblPassowrd" runat="server" CssClass="formLabel300" Text="Please enter your Password" AssociatedControlID="Password"></asp:Label>
                                            <asp:TextBox ID="Password" runat="server" CssClass="formField" TextMode="Password" ToolTip="Password"></asp:TextBox>
                                            <div tabindex="0" class="error-wrapper" aria-label="Password is required">
                                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" class="failureNotification"
                                                    ErrorMessage="* Password is required." ToolTip="Password is required"
                                                    ValidationGroup="LoginUserValidationGroup" SetFocusOnError="false" aria-hidden="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </asp:Panel>
                                    </section>
                                    <br />
                                    <br />
                                    <%--<table style="width:50%; margin:auto;">--%>
                                    <div class="row">
                                        <div class="col-sm-3"></div>
                                        <div class="col-sm-6">
                                            <%--<asp:LinkButton ID ="LoginButton" runat="server" CommandName="Login" ValidationGroup="LoginUserValidationGroup" CssClass="buttonBox buttonBoxFocus" Visible="false">
                                            <i class="glyphicon glyphicon-lock" aria-hidden="true"></i>Log In
                                        </asp:LinkButton>--%>
                                            <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="LoginUserValidationGroup" CssClass="buttonBox buttonBoxFocus" Visible="false" />
                                            <asp:Button ID="btnNext" runat="server" Text="Next" ValidationGroup="LoginUserValidationGroup" CssClass="buttonBox buttonBoxFocus" OnClick="btnNext_Click" />
                                            <asp:Button ID="btnGotoIOP" runat="server" Text="Go to IOP" ValidationGroup="LoginUserValidationGroup" CssClass="buttonBox buttonBoxFocus" Visible="false" OnClick="btnGotoIOP_Click" />

                                        </div>
                                        <%--<div class="col-sm-3" >
                                        <asp:Button ID="btnUnlockUser" runat="server" Text="Unlock User" OnClick="btnUnlockUser_Click" CssClass="buttonBox" />

                                    </div>--%>
                                        <div class="col-sm-3"></div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-sm-1"></div>
                                        <div class="col-sm-4">
                                          <strong><asp:Panel ID="pnlNewUser" runat="server"><span class="loginlink">Don't have an Account?</span>&nbsp;<asp:LinkButton ID="lnkCreateAccount" runat="server" Text="Click here"  CssClass="loginlink"  OnClick="lnkCreateAccount_Clicked" /></asp:Panel>
                                            <asp:HyperLink ID="hyperPasswordReset" CssClass="loginlink" runat="server" Text="Forgot/Reset Password?" NavigateUrl="~/Account/PasswordReset.aspx" ToolTip="This can only be used if you have not established an OH|ID" Visible="false" /></strong>
                                        </div>
                                        <div class="col-sm-3"></div>
                                        <div class="col-sm-3">
                                          <strong><asp:HyperLink ID="hyperForgotUserName" CssClass="loginlink" runat="server" Text="Forgot User ID?" NavigateUrl="~/Account/UserNameRecovery.aspx" ToolTip="This can only be used if you have not established an OH|ID" /></strong>
                                        </div>
                                        <div class="col-sm-1"></div>
                                        <%--<div class="col-sm-3"></div>--%>
                                    </div>
                                    <br />
                                </asp:Panel>
                                <asp:Panel ID="pnlOHNewLogin" runat ="server" CssClass="accountInfo" DefaultButton="OHLoginButton">
                                   <section class="login" aria-label="Login Information">
                                        <div class="row">
                                            <div class="col-sm-8 text-left">
                                                <h1>Log in</h1>
                                                <span style="font-size:medium;">All users must log in on the OH|ID portal using their single sign on ID.</span>
                                            </div>
                                        </div>
                                       <br />
                                       <div class="row">
                                           <div class="col-sm-5 text-left">
                                                <asp:Button ID="OHLoginButton" runat="server" Text="Log in with OH|ID" ValidationGroup="LoginUserValidationGroup" CssClass="ohLoginButton" OnClick="btnOHLogin_Click" />
                                           </div>
                                       </div>    
                                       <div class="attentionBox">
                                           Attention Providers: if you need assistance signing in or acquiring your OH|ID, please contact the ODM Integrated Help Desk at 800-686-1516 or email <a href="mail:ihd@medicaid.ohio.gov">ihd@medicaid.ohio.gov</a>
                                       </div>
                                   </section>
                                </asp:Panel>
                            </LayoutTemplate>
                        </asp:Login>
                    </div>
                    <asp:Panel ID="pnlLockedOut" runat="server" Visible="false">
                        <div style="margin: inherit; padding-top: 10px; text-align: center; font-size: 18px; color: red;">
                            <p>
                                <asp:Literal ID="ltrlLockedOut" runat="server" Text="<%$ Resources:BrandingResource , pnlLockedOut %>" />
                            </p>
                        </div>
                    </asp:Panel>
                    <br />
                                    </div>



                <uc:userprofile id="ucUserProfile" runat="server" visible="false" />
                <asp:Panel ID="pnlNewsBox" runat="server">
                    <div class="NewsBox">
                        <h2>Latest News 
                        </h2>
                        <div style="min-height: 100px">
                            <div id="divNewsItems" enableviewstate="false" runat="server">
                            </div>
                            <div id="divRender" runat="server">
                                <asp:PlaceHolder ID="phAutoGenerate" runat="server"></asp:PlaceHolder>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:HiddenField ID="hdnPswd" runat="server" />
            </div>
        </div>
        <!-- ModalPopupExtender -->
        <div aria-live="assertive">
        <ajax:ModalPopupExtender ID="mpelegalAgreement" runat="server" PopupControlID="pnllegalAgreement" TargetControlID="ButtonDummy"
            BackgroundCssClass="modalBackground">
        </ajax:ModalPopupExtender>
        <asp:Panel ID="pnllegalAgreement" runat="server" CssClass="modalPopup" Style="display: none; width: 50%; height: auto; overflow : auto;" TabIndex="0">
            <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
                <div class="popTitle">
                    <h1>Terms</h1>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlMain" CssClass="login-popup-scroll" runat="server" Style="margin-right: 10px">
                <div style="text-align: left; padding: 15px; margin-left: 30px !important" class="container-fluid">
                    <div>
                        <asp:Label ID="lblLegalAgreementNew" runat="server" CssClass="lblLegalAgreementCSS" Text="" />
                    </div>
                    <div> 
                        <asp:CheckBox ID="chkTerms" runat="server" style="color:black" Text="Yes, I have read the agreement" ToolTip="Agree to Policy and Terms of Service." OnCheckedChanged="chkTerms_CheckedChanged" AutoPostBack="true" />
                    </div>
                </div>
            </asp:Panel>
            <div class="btnBoxCancelLogin" style="width: 93%;">
                <asp:Button runat="server" ID="btnCancelTerms" Text="Cancel" CssClass="buttonBox" style="margin-right: 5%;" OnClick="btnCancelTerms_Click" />
            </div>
        </asp:Panel>
        <asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />
        </div>
    </div>
</asp:Content>
