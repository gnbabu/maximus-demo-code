<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SSO.aspx.cs" Inherits="StateSingleSignOn.SSO" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta3/dist/js/bootstrap.bundle.min.js" integrity="sha384-JEW9xMcG8R+pH31jmWH6WWP0WintQrMb4s7ZOdauHnUtxwoG2vI5DkLtS3qm9Ekf" crossorigin="anonymous"></script>
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous">
    <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&display=swap" rel="stylesheet">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>

    <link href="<%# Page.ResolveClientUrl("~/Styles/MESC/styles.css") %>" rel="stylesheet" />
    <title>MES</title>
    <style type="text/css">
        div[class^='container'], div[class*='container'] {
            padding: 0 8px !important;
        }

        div[class^='row'], div[class*='row'] {
            margin: 0 -8px !important;
        }

        div[class^='col'], div[class*='col'] {
            padding: 0 8px !important;
        }

        :root {
            --theme_color: #502e91;
            --lightgrey: #F3F7FE;
            --midGrey: #9192C4;
            --white: #ffffff;
            --very-dark-grey: #26262E;
            --dark-blue: #5152B7;
            --greylight-pink: #F4EEEE;
            --dark-grey: #26262E;
            --sky-light-blue: #B6C7E1;
        }

        @font-face {
            font-family: 'Noto Sans Regular';
            font-style: normal;
            font-weight: normal;
            src: local('Noto Sans Regular'), url('../fonts/NotoSans-Regular.woff') format('woff');
        }


        @font-face {
            font-family: 'Noto Sans Regular';
            font-style: normal;
            font-weight: normal;
            src: local('Noto Sans Regular'), url('../fonts/NotoSans[wdth,wght].woff') format('woff');
        }


        @font-face {
            font-family: 'Noto Sans Italic';
            font-style: normal;
            font-weight: normal;
            src: local('Noto Sans Italic'), url('../fonts/NotoSans-Italic.woff') format('woff');
        }


        @font-face {
            font-family: 'Noto Sans Italic';
            font-style: normal;
            font-weight: normal;
            src: local('Noto Sans Italic'), url('../fonts/NotoSans-Italic[wdth,wght].woff') format('woff');
        }


        @font-face {
            font-family: 'Noto Sans Bold';
            font-style: normal;
            font-weight: normal;
            src: local('Noto Sans Bold'), url('../fonts/NotoSans-Bold.woff') format('woff');
        }


        @font-face {
            font-family: 'Noto Sans Bold Italic';
            font-style: normal;
            font-weight: normal;
            src: local('Noto Sans Bold Italic'), url('../fonts/NotoSans-BoldItalic.woff') format('woff');
        }

        body {
            font-family: 'Noto Sans Regular',arial, helvetica, sans-serif !important;
            font-optical-sizing: auto;
            font-style: normal;
            overflow-x: hidden;
            text-align: left;
        }

        a, a:link, a:visited, a:active, a:hover {
            text-decoration: none !important;
            color: inherit
        }

        .comp-body {
            background-color: var(--lightgrey);
            position: relative;
            text-align: left;
        }

        .app-header {
            padding: 0.8rem;
        }

        .comp-body:after {
            content: '';
            position: absolute;
            width: 100vw;
            height: 100vh;
            top: 0;
            left: 0;
            background-image: url('../Images/MESC/body_bg.jpg');
            z-index: -1;
            opacity: 0.1;
        }
        /* menu */
        #comMenu {
            width: 28px;
            height: 22px;
            cursor: pointer;
            display: flex;
            flex-direction: column;
            flex-wrap: wrap;
            justify-content: space-between;
        }

            #comMenu div {
                width: 100%;
                height: 3px;
                background: var(--very-dark-grey);
                transition: all 0.3s;
                backface-visibility: hidden;
            }

            #comMenu.on .one {
                transform: rotate(45deg) translate(5px, 5px);
            }

            #comMenu.on .two {
                opacity: 0;
            }

            #comMenu.on .three {
                transform: rotate(-45deg) translate(7px, -8px);
            }
        /* END MENU */
        .bg-white {
            background-color: var(--white);
        }

        .branding {
            width: 120px;
        }

        .main_nav a {
            display: inline-block;
            padding: 0.7rem !important;
            border-radius: 4px;
            margin: 0 .5rem;
            text-decoration: none;
            font-weight: 600;
            color: var(--theme_color) !important;
        }

            .main_nav a:hover {
                background-color: var(--theme_color);
                color: var(--white) !important
            }

        .login-container {
            height: calc(100vh - 240px);
            display: flex;
            align-content: center;
            align-items: center;
            width: 90%;
            margin: auto;
        }

        .login_welcome {
            max-height: 520px
        }

        .login-box {
            padding: 2rem;
        }

            .login-box input {
                padding: 0.7rem;
                background-color: var(--greylight-pink);
            }

                .login-box input:focus {
                    border: 1px solid var(--dark-blue);
                    background-color: var(--greylight-pink);
                    box-shadow: inset 0 -1px 0 #ddd;
                }

        .singin_title {
            padding: 0.5rem 0;
            font-size: 1.2rem;
            border-bottom: 1px solid var(--midGrey);
        }

        .fl-icon {
            position: absolute;
            z-index: 99;
            right: 14px;
            top: 10px;
        }

        .login-settings a {
            text-decoration: none;
            display: inline-block;
            position: relative;
            padding: .5rem;
            font-size: 0.92rem;
        }

            .login-settings a::after {
                content: "";
                position: absolute;
                width: 30%;
                height: 2px;
                background-color: var(--midGrey);
                z-index: 9;
                left: 0;
                bottom: 0;
                transition: all 400ms ease-in-out;
                opacity: 0;
            }

            .login-settings a:hover::after {
                width: 100%;
                transition: all 400ms ease-in-out;
                opacity: 0.3;
            }

        input.login-btn {
            padding: 0.7rem;
            background-color: var(--dark-grey);
            color: var(--white);
        }

            input.login-btn:hover {
                color: var(--white);
                background-color: var(--very-dark-grey);
            }

        a {
            text-decoration: none;
        }

        .welcome-bg {
            background-color: var(--sky-light-blue);
        }

        .slider_box {
            padding: 2rem;
            color: var(--white);
        }

            .slider_box h1 {
                font-weight: 700;
            }

                .slider_box h1 span {
                    color: var(--dark-blue);
                }

        .login-carosole p {
            color: var(--white);
            font-size: 1.1rem;
            line-height: 1.4;
            padding: 2rem 0;
        }

        .welcome-news {
            min-height: 300px;
        }


        .dropdown-menu hr.dropdown-divider {
            border-top-color: var(--midGrey);
        }

        .btn-primary:hover {
            background-color: #03026d;
            border-color: #004393;
            color: #3b87ff;
            box-shadow: 0 7px 17px 1px rgba(3 2 109 / 0.2);
        }

        .btn-primary {
            margin-bottom: 30px;
            display: inline-block;
            color: #FFFFFF;
            min-width: 160px;
            background-color: #03026d !important;
            border-radius: 30px;
            padding: 0 24px;
            height: 52px;
            font-size: 18px;
            line-height: 19px;
            border: 0;
            font-weight: 600;
            box-shadow: 0 0px 0 0px rgba(0 0 0 / .25);
            transition: all .2s cubic-bezier(0.46, 0.03, 0.12, 0.99);
            cursor: pointer;
            background-image: none;
        }

        .leftMenu {
            position: fixed;
            width: 320px;
            background-color: var(--white);
            height: 100vh;
            left: -340px;
            z-index: 9;
            top: 0;
            padding: 2rem 0;
            transition: all 400ms ease-in-out;
        }

        .left-brandin {
            height: 80px;
            background-color: var(--theme_color);
            margin-bottom: 3rem;
        }

        .leftMenu.active {
            left: 0;
            transition: all 400ms ease-in-out;
        }

        .left_menu_list a {
            display: block;
            margin: 0.2rem .5rem !important;
            padding: .8rem .8rem !important;
            color: var(--theme_color) !important;
            background-color: transparent;
            border-radius: 14px;
            transition: all 400ms ease-in-out;
        }

            .left_menu_list a:hover {
                background-color: var(--theme_color) !important;
                color: var(--white) !important;
                transition: all 400ms ease-in-out;
            }

        .container_login {
            max-width: 1440px !important;
            margin: auto;
        }

        body {
            transition: all 400ms ease-in-out;
            padding-left: 0px !important;
        }

            body.active {
                overflow-x: hidden;
                padding-left: 320px !important;
                transition: all 400ms ease-in-out;
            }

        .menutoggle.active .two {
            display: none
        }

        .menutoggle.active .one {
            transform: rotate(50deg) translateY(7px) translateX(10px)
        }

        .menutoggle.active .three {
            transform: rotate(-50deg) translateY(-3px) translateX(6px)
        }

        #Footer {
            background-color: var(--theme_color) !important;
            color: var(--white) !important;
            padding: 3rem !important;
            border-top: 1px solid #B6C7E1 !important;
            margin-top: 0px !important;
            height: auto !important
        }

        footer a {
            color: var(--white) !important;
            display: inline-block;
            padding: 0.3rem 0;
        }

        .btn-secondary:hover {
            font-weight: 600;
            color: #ffffff !important;
            background: #0163ff;
        }

        #createAccountButton {
            margin-top: 24px !important;
        }

        .btn-secondary {
            margin-top: 35px;
            border: 3px solid #0163ff;
            border-radius: 30px;
            padding: 12px 40px;
            background-color: #ffffff;
            font-size: 18px;
            line-height: 19px;
            font-weight: 600;
            color: #0163ff !important;
            transition: all .2s cubic-bezier(0.46, 0.03, 0.12, 0.99);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="form-container container">
                <div class="row">
                    <div>
                        <h1 class="text-center sr-only">STATEID</h1>

                        <div class="center-block logo-container_image" style="background-image: url(/wps/wcm/connect/gov/3bb51acb-f2f6-4ba9-82a3-3fea906321a3/ohid_logo.png?MOD=AJPERES&amp;CACHEID=ROOTWORKSPACE.Z18_J146I0G0O8K440Q411BSNT10A1-3bb51acb-f2f6-4ba9-82a3-3fea906321a3-oNz-tEW);"></div>
                        </a>
                            <div class="margin-top-xs text-center h3-semi-bold ohid-login-title">State's Digital Identity. One State. One Account.</div>
                        <p class="margin-top-xs text-center ohid-login-subtitle">
                            Register once, use across many State websites
                        </p>

                    </div>
                </div>
            </div>
        </div>
        <div class="btn-wrapper text-center">
            <a id="createAccountButton" class="btn btn-secondary" tabindex="0" role="button" href="/create-account">Create account
            </a>
        </div>
        <div class="login_welcome bg-white">
            <div class="row">
                <div class="col-md-4"></div>
                <div class="col-md-4">
                    <div class="login-box">
                        <div class="singin_title">
                            <p class="m-0">Sign In With <strong>State ID</strong></p>
                        </div>
                        <p class="p-4 ps-0">
                            <strong>
                                <asp:Label ID="lblErrormessage" Style="color: red" runat="server" Text="* User ID and/or Password does not match our records." Visible="false"></asp:Label>
                            </strong>
                        </p>
                        <asp:Label ID="lblUserName" runat="server" Style="border: 0px;" CssClass="form-control" Text="State ID"></asp:Label>
                        <div class="mb-3 position-relative">
                            <%--<input type="text" class="form-control pe-5" id="userID" placeholder="Enter UserID">--%>

                            <asp:TextBox ID="UserName" runat="server" CssClass="form-control pe-5" ToolTip="Enter UserID"></asp:TextBox>
                            <span class="fl-icon">
                                <img src="../STATE_SSO/Images/MESC/user.svg" width="24" height="24" alt="user" />
                            </span>
                            <small class="text-danger">
                                <div tabindex="0" class="error-wrapper">
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" class="failureNotification"
                                        ErrorMessage="* User ID is required." ToolTip="User ID is required"
                                        ValidationGroup="LoginUserValidationGroup" SetFocusOnError="false" aria-hidden="true">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </small>
                        </div>
                        <asp:Label ID="lblPassword" runat="server" Style="border: 0px;" CssClass="form-control" Text="Password"></asp:Label>
                        <div class="mb-3 position-relative">
                            <%--<input type="password" class="form-control pe-5" id="password" placeholder="Enter Password">--%>

                            <asp:TextBox ID="password" runat="server" CssClass="form-control pe-5" TextMode="Password" ToolTip="Enter Password"></asp:TextBox>

                            <span class="fl-icon">
                                <img src="../STATE_SSO/Images/MESC/password.svg" width="24" height="24" alt="user" />
                            </span>
                            <small class="text-danger">
                                <div tabindex="0" class="error-wrapper" aria-label="Password is required">
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="password" class="failureNotification"
                                        ErrorMessage="* Password is required." ToolTip="Password is required"
                                        ValidationGroup="LoginUserValidationGroup" SetFocusOnError="false" aria-hidden="true">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </small>
                        </div>
                        <div class="mb-3 d-flex justify-content-md-between login-settings">
                            <div>
                                <a href="#" runat="server">Forgot User ID</a>/<a href="#">Unlock User</a>
                            </div>
                            <div>
                                <a href="#" runat="server">Forgot / Reset Password</a>
                            </div>
                        </div>
                        <div>
                            <%-- <button type="button" class="btn login-btn btn-md w-100 text-center">Sign In</button>--%>
                            <asp:Button ID="btnNext" runat="server" Text="Sign In" ValidationGroup="LoginUserValidationGroup" CssClass="btn-primary btn-md w-100 text-center"
                                OnClick="LoginUser_LoggingIn" />

                            <%--btnNext_Click--%>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    </
                </div>
            </div>
    </form>
</body>
</html>

