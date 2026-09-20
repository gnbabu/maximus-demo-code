<%@ page title="Log In" language="C#" autoeventwireup="true" inherits="Account_Login1" Codebehind="Login.aspx.cs" %>

<%@ register src="../UserControls/UserProfile.ascx" tagname="UserProfile" tagprefix="uc" %>
<%@ register src="~/UserControls/BreakingNews.ascx" tagname="BreakingNews" tagprefix="uc1" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head runat="server">

    <meta charset="UTF-8" />
    <title>Maximus Sign In</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap 5 + Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" rel="stylesheet" />

    <style>
        html, body {
            height: 100%;
            margin: 0;
            background-color: #f0f0f0;
            font-size: 12.5pt !important;
        }

        .page-wrapper {
            min-height: 100vh;
            display: flex;
            flex-direction: column;
        }

        /* Sidebar */
        .sidebar {
            width: 300px;
            background-color:#004e9a;/* #004e9a;*/
            color: #fff;
            transition: width 0.3s ease;
            /*display: flex;*/
            /*flex-direction: column;*/
            padding-top: 50px;
            min-height: 100vh;
        }

            /* Fully closed */
            .sidebar.collapsed {
                width: 0;
                display: none;
            }

            .sidebar h5 {
                padding: 15px;
                text-align: center;
                font-size: 18px;
                border-bottom: 1px solid rgba(255, 255, 255, 0.1);
                margin: 0;
                white-space: nowrap;
            }

            .sidebar .nav-link {
                color: #fff;
                padding: 12px 30px;
                white-space: nowrap;
                font-size: 12pt;
                text-decoration: none !important;
                font-family:'Noto Sans Regular',arial, helvetica, sans-serif !important;
                /* Add smooth transition for all properties */
            }

                .sidebar .nav-link:hover {
                    background-color: #28a0cb;
                    color: #fff;
                    /* Font & Visual Effects */
                    padding-left: 40px; /* Subtle slide-to-right effect */
                    letter-spacing: 0.5px; /* Slightly expands the text */
                    text-shadow: 0 0 8px rgba(255, 255, 255, 0.4); /* Soft glow effect */
                    box-shadow: inset 4px 0 0 0 #00bde3;
                }

            .sidebar .bi {
                margin-right: 8px;
            }

        /* Main Content */
        .main-section {
            flex-grow: 1;
            display: flex;
            flex-direction: column;
        }

        .header {
            background-color: #ffffff;
            color: black;
            padding: 10px 20px;
            font-size: 30px;
            justify-content: space-between;
            align-items: center;
            font-weight: 600;
            /*display: flex;*/
            user-select: none;
        }

        .toggle-btn {
            background: none;
            border: none;
            color: black;
            font-size: 25px;
            display: inline-flex;
            align-items: center;
            gap: 5px;
            cursor: pointer;
        }

        .navbar-custom {
            background-color: #004e9a;
        }

            .navbar-custom .nav-link {
                color: #fff;
                font-weight: 600;
                font-size: 14pt;
                margin-left: 12px;
                text-decoration: none;
                border-bottom: 4px solid transparent;
            }

                .navbar-custom .nav-link:hover {
                    border-bottom: 4px solid #28a0cb
                }



        .flex-main {
            flex: 1;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 30px 15px;
            overflow-y: auto;
        }

        .main-container {
            width: 100%;
            max-width: 1100px;
        }

        .panel-custom {
            background: #fff;
            border-left: 1px solid #ddd;
            padding: 20px;
            margin-bottom: 20px;
            display: flex;
            flex-direction: column;
        }

            .panel-custom h1 {
                font-size: 1.5rem;
                font-weight: bold;
                margin-top: 0;
            }

            .panel-custom h2 {
                font-size: 1rem;
                font-weight: bold;
                margin-top: 0;
            }

            .panel-custom h3 {
                font-size: 1.2rem;
            }

            .panel-custom h3,
            .panel-custom h4 {
                font-weight: bold;
                margin-top: 0;
            }

        .panel-custom-login {
            background: #fff;
            padding: 20px;
            margin-bottom: 20px;
            display: flex;
            flex-direction: column;
        }

        .panel-custom-sso {
            padding: 10px 20px 20px 20px;
            margin-bottom: 20px;
            display: flex;
            flex-direction: column;
        }

        .usa-input {
            border-width: 1px;
            border-color: #565c65;
            border-style: solid;
            border-radius: 0;
            color: #1b1b1b;
            display: block;
            height: 3.3rem !important;
            margin-top: 0.5rem;
            /*max-width: 30rem;*/
            padding: 0.5rem;
            width: 100%;
            transition: border-color 0.3s ease, box-shadow 0.3s ease;
        }

            .usa-input:focus {
                border-color: #0d6efd; /* Bootstrap primary color */
                outline: none;
                box-shadow: 0 0 8px 4px rgba(13, 110, 253, 0.6);
                background-color: #fff;
                z-index: 1;
                position: relative;
            }

        .usa-label {
            font-family: Source Sans Pro Web, Helvetica Neue, Helvetica, Roboto, Arial, sans-serif;
            font-size: 1.15rem !important;
            line-height: 1.3;
            display: block;
            font-weight: 400;
            margin-top: 1.0rem;
            max-width: 30rem;
        }

        .createaccount {
            font-size: 1.15rem !important;
            border-bottom: 2px #dad7d9 solid;
            padding-bottom: 20px;
        }

        .usa-show-password {
            font-size: 1.2rem !important;
            line-height: 1.3;
            float: right;
            margin: .25rem 0 1rem !important;
            color: #005ea2 !important;
            -webkit-text-decoration: underline;
            text-decoration: underline;
            background-color: transparent;
            border: 0;
            border-radius: 0;
            box-shadow: none;
            font-weight: 400;
            justify-content: normal;
            text-align: left;
            width: auto;
            cursor: pointer;
        }

        .usa-forgot-password {
            font-size: 1.2rem !important;
            line-height: 1.3;
            float: left;
            margin: .25rem 0 1rem !important;
            color: #005ea2 !important;
            -webkit-text-decoration: underline;
            text-decoration: underline;
            background-color: transparent;
            border: 0;
            border-radius: 0;
            box-shadow: none;
            font-weight: 400;
            justify-content: normal;
            text-align: left;
            width: auto;
            cursor: pointer;
        }

        .usa-link {
            color: #005ea2 !important;
            -webkit-text-decoration: underline;
            text-decoration: underline;
        }

        .btn-primary {
            background-color: #004aad;
            border-color: #004aad;
            font-size: 17px;
            font-weight: 600;
            width: 120px;
        }

            .btn-primary:hover {
                background-color: #003a82;
                border-color: #003a82;
            }

        .btn-outline-primary {
            color: #004aad;
            background-color: #fff;
            border: 2px solid #004aad;
            font-size: 17px;
            font-weight: 600;
            width: 250px;
        }

            .btn-outline-primary:hover {
                background-color: #004aad;
                border: 2px solid #004aad;
            }

        .govt-footer {
            background-color: rgb(135 146 157 / 2%);
            color: #162e51;
            text-align: center;
            font-size: 12px;
            padding: 10px 15px;
        }

            .govt-footer a {
                color: #162e51 !important;
                text-decoration: none;
            }

        .forgot-link {
            float: right;
            margin-top: 5px;
            font-size: 16px;
        }

        .form-control {
            background-color: #ffffff !important;
            border: 1px solid #757575;
            outline: none; /* optional: suppress default focus glow */
        }

            /* Add a different border color when focused */
            .form-control:focus {
            }

        .h1SignIn {
            font-family: Source Sans Pro Web,Helvetica Neue,Helvetica,Roboto,Arial,sans-serif !important;
            font-size: 2em !important;
            margin-top: 1rem !important;
        }

        .h2AccAccess {
            font-family: Source Sans Pro Web, Helvetica Neue, Helvetica, Roboto, Arial, sans-serif !important;
            font-size: 2.13rem !important;
            font-weight: 700 !important;
        }

        .h2diffSignIn {
            font-family: Source Sans Pro Web, Helvetica Neue, Helvetica, Roboto, Arial, sans-serif !important;
            display: block;
            font-size: 1.5em !important;
            margin-block-start: 0.83em;
            margin-block-end: 0.83em;
            margin-inline-start: 0px;
            margin-inline-end: 0px;
            font-weight: bold !important;
            unicode-bidi: isolate;
        }

        .usa-button {
            font-family: Source Sans Pro Web, Helvetica Neue, Helvetica, Roboto, Arial, sans-serif;
            font-size: 1.10rem;
            line-height: .9;
            color: #fff;
            background-color: #005ea2;
            -webkit-appearance: none;
            -moz-appearance: none;
            appearance: none;
            align-items: center;
            border: 0;
            border-radius: .25rem;
            cursor: pointer;
            -moz-column-gap: .5rem;
            column-gap: .5rem;
            display: inline-flex;
            font-weight: 700;
            justify-content: center;
            margin-right: .5rem;
            padding: 0.85rem 1.50rem;
            text-align: center;
            -webkit-text-decoration: none;
            text-decoration: none;
        }

        .usa-button--outline {
            background-color: transparent;
            box-shadow: inset 0 0 0 2px #005ea2;
            color: #005ea2;
            width: 50%;
        }

            .usa-button--outline:visited {
                color: #005ea2;
            }


        /* Responsive behavior */

        /* On small devices, sidebar is hidden, toggle is hidden */
        @media (max-width: 767.98px) {
            .sidebar {
                display: none !important;
                min-height: auto;
            }

            .toggle-btn {
                display: none;
            }

            main.flex-main {
                padding: 1rem 1rem 2rem;
            }
        }

        /* On medium and up: sidebar visible */
        @media (min-width: 768px) {
            .page-wrapper {
                flex-direction: row;
            }

            .sidebar {
                width: 300px;
                flex-shrink: 0;
            }

            .main-section {
                flex-grow: 1;
                min-height: 100vh;
                display: flex;
                flex-direction: column;
            }

            main.flex-main {
                padding: 2rem 2rem;
            }
        }

        .form-help a {
            font-size: 16px;
        }


        /* Footer styles */
        #main-footer {
            background-color:#ECEFF1;   /* #f5f0ca4f;*/
            color: black;
            padding-right: 2rem;
            padding-top: 0.5rem;
            border-top: 1px solid #B6C7E1;
        }

        footer a {
            color: black !important;
            display: inline-block;
            padding: 0.3rem 0;
            text-decoration: none !important
        }

        .footer-grid {
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between;
            margin-left: 10px;
            margin-right: auto;
            gap: 2rem;
            user-select: none;
        }

        .footer-column {
            flex: 1 1 250px;
            text-align: justify;
            color:black !important;
        }

            .footer-column h4 {
                font-size: 16px;
                margin-bottom: 10px;
                color: var(--white);
            }

            .footer-column p,
            .footer-column ul {
                font-size: 14px;
                line-height: 1.6;
            }

            .footer-column ul {
                list-style: none;
                padding: 0;
            }

                .footer-column ul li {
                    margin-bottom: 6px;
                }

            .footer-column a:hover {
                text-decoration: underline;
            }

        .footer-bottom {
            text-align: center;
            margin-top: 30px;
            font-size: 13px;
            color: #cccccc;
        }

        .footer-logo {
            /*   width: 120px;
            height: 24px;
            margin-top: -7px;*/
        }

        .footer-header {
            color: black;
            font-size: 40px;
            justify-content: space-between;
            align-items: center;
            font-weight: 800;
        }

        .return-to-top {
            text-align: right;
            margin-bottom: 5px;
        }

            .return-to-top a {
                color: #fff;
                text-decoration: none;
                font-size: 14px;
                text-decoration: none !important
            }

                .return-to-top a:hover {
                    text-decoration: underline;
                }

        #divRender {
            max-height: 743px;
            overflow-y: auto;
            overflow-x: hidden;
        }
    </style>
</head>
<body>



    <form id="form1" runat="server">
        <div class="page-wrapper">
            <!-- Sidebar (starts collapsed via class below) -->
            <nav id="sidebar" class="sidebar collapsed" aria-label="Sidebar navigation">
                <ul class="nav flex-column">
                    <li class="nav-item">
                        <a href="~/Process/FeeScheduleContact.aspx" class="nav-link" runat="server">Skip To Main Content</a>
                    </li>
                    <li class="nav-item">
                        <a href="https://www.dhcs.ca.gov/Medi-Cal/Pages/home.aspx" class="nav-link" runat="server">Medicaid Home</a>
                    </li>
                    <li class="nav-item">
                        <a href="~/Process/Default.aspx" class="nav-link" runat="server">Home</a>
                    </li>
                    <li class="nav-item">
                        <a href="~/Process/PublicSearch.aspx" class="nav-link" runat="server">Provider Directory</a>
                    </li>
                    <li class="nav-item">
                        <a href="~/Process/GISSearch.aspx" class="nav-link" runat="server">Provider Search - GIS</a>
                    </li>
                    <li class="nav-item">
                        <a href="~/Process/Resources.aspx" class="nav-link" runat="server">Provider Ed & Training Resources</a>
                    </li>
                    <li class="nav-item">
                        <a href="~/Process/ContactUs.aspx" class="nav-link" runat="server">Contact Us</a>
                    </li>
                    <li class="nav-item">
                        <a href="~/Process/FeeScheduleContact.aspx" class="nav-link" runat="server">Fee Schedule</a>
                    </li>
                </ul>
            </nav>

            <!-- Main Area -->
            <div class="main-section">
                <!-- Header -->
                <header>
                    <div class="header">
                        <%--      <button id="sidebarToggleBtn" class="toggle-btn" type="button" aria-expanded="false" aria-controls="sidebar">
                        <i class="bi bi-list" id="toggleIcon"></i>
                    </button>--%>
                        <button id="sidebarToggleBtn" class="toggle-btn" type="button" aria-expanded="false" aria-controls="sidebar" aria-label="Toggle sidebar">
                            <i class="bi bi-list" id="toggleIcon"></i>
                        </button>
                        <img src='<%= ResolveUrl("~/Images/MESC/oklahomalogo.png") %>' alt="MES" class="footer-logo" style="width: 12rem;" />
                    </div>
                </header>
                <!-- Top Navbar -->
                <nav class="navbar navbar-expand-lg navbar-custom">
                    <div class="container-fluid">
                        <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                            <li class="nav-item"><a class="nav-link" href="~/Default.aspx" runat="server">Provider Network Management</a></li>
                            <li class="nav-item"><a class="nav-link" href="https://www.dhcs.ca.gov/Medi-Cal/Pages/home.aspx" target="_blank" runat="server">Medicaid Home</a></li>
                            <li class="nav-item"><a class="nav-link" href="~/Resources.aspx" runat="server">Learning</a></li>
                            <li class="nav-item"><a class="nav-link" href="~/Process/ContactUs.aspx" runat="server">Contact</a></li>
                            <li class="nav-item"><a class="nav-link" href="~/Process/FeeScheduleContact.aspx" runat="server">Fee Schedule</a></li>
                        </ul>
                    </div>
                </nav>

                <!-- Main Body -->
                <main class="flex-main" style="background-color: #ffffff">
                    <div class="container">
                        <div class="row align-items-stretch">
                            <!-- Left Column: Sign In + SSO -->
                            <div class="col-md-6 d-flex flex-column">
                                <div class="panel-custom-login flex-fill mb-3">
                                    <h2 class="fw-bold mb-0">Sign in</h2>
                                    <h2 class="fw-bold mb-3">Access your account</h2>
                                    <strong>
                                        <asp:Label ID="lblErrormessage" Style="color: red" runat="server" Text="* User ID and/or Password does not match our records." Visible="false"></asp:Label>
                                    </strong>

                                    <div class="mb-3">
                                        <label for="txtUserName" class="form-label usa-label">Email address</label>
                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control usa-input" ToolTip="Email address"></asp:TextBox>
                                        <small class="text-danger">
                                            <div tabindex="0" class="error-wrapper">
                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="txtUserName" class="failureNotification"
                                                    ErrorMessage="* User ID is required." ToolTip="User ID is required"
                                                    ValidationGroup="LoginUserValidationGroup" SetFocusOnError="false" aria-hidden="true">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </small>
                                    </div>
                                    <div class="mb-3">
                                        <label for="txtPassword" class="form-label usa-label">Password</label>
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control usa-input" ToolTip="Password"></asp:TextBox>
                                        <a href="#" id="togglePassword" class="usa-show-password">Show password</a>

                                        <small class="text-danger">
                                            <div tabindex="0" class="error-wrapper">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPassword" class="failureNotification"
                                                    ErrorMessage="* Password is required." ToolTip="Password is required"
                                                    ValidationGroup="LoginUserValidationGroup" SetFocusOnError="false" aria-hidden="true">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </small>
                                    </div>
                                    <asp:Button ID="btnLogin" runat="server" Text="Sign in" ValidationGroup="LoginUserValidationGroup"
                                        OnClick="btnLogin_Click" CssClass="btn btn-primary usa-button" Visible="true" />

                                    <div class="form-help mt-2">
                                        <a href="~/Account/PasswordReset.aspx" runat="server" class="usa-forgot-password">Forgot password?</a>
                                    </div>
                                </div>
                                <div class="panel-custom-sso">

                                    <p class="text-center createaccount">
                                        Don't have an account?
                                       
                                        <a href="https://mesc.mpc-release.maximus.com/STATE_SSO/create-account" class="usa-link">Create your account now</a>

                                    </p>
                                    <h2 class="h2diffSignIn">Need a different way to sign in?</h2>
                                    <p>Use our secure SSO option if you have been provided alternate access credentials.</p>
                                    <asp:Button ID="btnGotoIOP" runat="server" Text="Launch secondary SSO"
                                        OnClick="btnGotoIOP_Click" CssClass="usa-button usa-button--outline" Visible="true" />
                                </div>
                            </div>

                            <!-- Right Column: News -->
                            <div class="col-md-6">
                                <div class="panel-custom flex-fill">
                                    <h1>Latest News</h1>
                                    <div id="divRender" runat="server">
                                        <%--<asp:PlaceHolder ID="phAutoGenerate" runat="server"></asp:PlaceHolder>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </main>

                <!-- Footer -->
                <footer>
                    <div class="govt-footer">
                        &copy; 2026 Maximus Provider Portal. For demonstration purposes only.
                    </div>
                    <div id="main-footer">
                        <div class="return-to-top">
                            <a href="#">Return to top</a>
                        </div>
                        <div class="footer-grid">
                            <!-- Logo and Description -->
                            <div class="footer-column text-center">
                                <div class="footer-header">
                                    <span>MES</span>
                                </div>
                            </div>

                            <!-- Contact Info -->
                            <div class="footer-column">
                                <h4>Contact</h4>
                                <p>
                                    1600 Tysons Blvd<br />
                                    Suite 1400<br />
                                    McLean, VA 22102
                                </p>
                                <p>Phone: <a href="tel:18006861516">1-800-686-1516</a></p>
                            </div>

                            <!-- Links -->
                            <div class="footer-column">
                                <h4>Quick Links</h4>
                                <ul>
                                    <li><a href="https://maximus.com/">Home</a></li>
                                    <li><a href="#">Contact Us</a></li>
                                    <li><a href="https://ohio.gov/wps/portal/gov/site/home/privacy-notice-and-policies" target="_blank">Privacy & Accessibility</a></li>
                                    <li><a href="https://maximus.com/Terms">Terms of Use</a></li>
                                </ul>
                            </div>

                            <!-- System Info -->
                            <div class="footer-column">
                                <h4>System Info</h4>
                                <p>Version: v3.0.109</p>
                                <p>Updated: September 20th, 2026</p>
                            </div>
                        </div>
                    </div>


                </footer>


            </div>
        </div>

        <!-- Bootstrap JS -->
        <script id="ze-snippet" src='<%="https://static.zdassets.com/ekr/snippet.js?key=" + SnippetKey %>'></script>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
        <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

        <script>
            (function () {
                const sidebar = document.getElementById('sidebar');
                const btn = document.getElementById('sidebarToggleBtn');
                const icon = document.getElementById('toggleIcon');

                function toggleSidebar() {
                    const isCollapsed = sidebar.classList.toggle('collapsed');
                    btn.setAttribute('aria-expanded', (!isCollapsed).toString());
                    icon.classList.toggle('bi-list', isCollapsed);
                    icon.classList.toggle('bi-x', !isCollapsed);
                }

                btn.addEventListener('click', toggleSidebar);

                // Ensure collapsed on load and correct icon set
                sidebar.classList.add('collapsed');
                btn.setAttribute('aria-expanded', 'false');
                icon.classList.remove('bi-x');
                icon.classList.add('bi-list');
            })();

            $(document).ready(function () {
                $('#togglePassword').on('click', function (e) {
                    e.preventDefault();
                    var passwordInput = $('#<%= txtPassword.ClientID %>');
                    var isPassword = passwordInput.attr('type') === 'password';
                    passwordInput.attr('type', isPassword ? 'text' : 'password');
                    $(this).text(isPassword ? 'Hide password' : 'Show password');
                });
            });
        </script>
    </form>
</body>
</html>

