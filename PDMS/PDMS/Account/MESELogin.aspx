<%@ Page Language="C#" AutoEventWireup="true" Inherits="Account_MESELogin" Codebehind="MESELogin.aspx.cs" %>

<!DOCTYPE html>
<html lang="en">
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
            background-color: #f9f9f9;
        }

        .page-wrapper {
            display: flex;
            height: 100vh;
            overflow: hidden;
        }

        /* Sidebar */
        .sidebar {
            width: 300px;
            background-color: #002d72;
            color: #fff;
            flex-shrink: 0;
            transition: width 0.3s ease;
            overflow: hidden; /* hides content when collapsed */
            display: flex;
            flex-direction: column;
            padding-top: 50px;
        }

            /* Fully closed */
            .sidebar.collapsed {
                width: 0;
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
            }

                .sidebar .nav-link:hover {
                    background-color: #003a82;
                }

            .sidebar .bi {
                margin-right: 8px;
            }

        /* Main Content */



        .main-section {
            flex-grow: 1;
            display: flex;
            flex-direction: column;
            height: 100vh;
        }

        .header {
            background-color: #004aad;
            color: #fff;
            padding: 10px 20px;
            font-size: 30px;
            justify-content: space-between;
            align-items: center;
            font-weight: 600;
        }

        .toggle-btn {
            background: none;
            border: none;
            color: #fff;
            font-size: 25px;
            display: inline-flex;
            align-items: center;
            gap: 5px;
            cursor: pointer;
        }

        .navbar-custom {
            background-color: #002d72;
        }

            .navbar-custom .nav-link {
                color: #fff;
                font-weight: 600;
                font-size: 14pt;
                margin-left: 12px;
                text-decoration: none;
            }

                .navbar-custom .nav-link:hover {
                    background-color: #003a82;
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
            border: 1px solid #ddd;
            padding: 20px;
            margin-bottom: 20px;
        }

            .panel-custom h3,
            .panel-custom h4 {
                font-weight: bold;
                margin-top: 0;
            }

        .btn-primary {
            background-color: #004aad;
            border-color: #004aad;
        }

            .btn-primary:hover {
                background-color: #003a82;
                border-color: #003a82;
            }

        .govt-footer {
            background-color: #bbb;
            color: #111;
            text-align: center;
            font-size: 12px;
            padding: 10px 15px;
        }

            .govt-footer a {
                color: #111 !important;
                text-decoration: none;
            }

        .forgot-link {
            float: right;
            margin-top: 5px;
            font-size: 16px;
        }

        @media (max-width: 767.98px) {
            .forgot-link {
                float: none;
                display: block;
                text-align: right;
            }
            /* Hide sidebar completely on mobile regardless of state */
            .sidebar {
                display: none !important;
            }
        }

        .form-help a {
            font-size: 16px;
        }


        /* Footer styles */
        #main-footer {
            background-color: #111;
            color: #fff;
            padding-right: 2rem;
            padding-top: 0.5rem;
            border-top: 1px solid #B6C7E1;
        }

        footer a {
            color: #fff !important;
            display: inline-block;
            padding: 0.3rem 0;
            text-decoration: none !important
        }

        .footer-grid {
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between;
            margin: auto;
            gap: 30px;
            max-width: 1600px;
        }

        .footer-column {
            flex: 1 1 250px;
            text-align: justify;
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
            width: 120px;
            margin-bottom: 10px;
        }

        .footer-header {
            color: #fff;
            font-size: 40px;
            justify-content: space-between;
            align-items: center;
            font-weight: 800;
        }

        .return-to-top {
            text-align: right;
            margin-bottom: 10px;
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="page-wrapper">
            <!-- Sidebar (starts collapsed via class below) -->
            <nav id="sidebar" class="sidebar collapsed">
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
                <div class="header">
                    <button id="sidebarToggleBtn" class="toggle-btn" type="button" aria-expanded="false" aria-controls="sidebar">
                        <i class="bi bi-list" id="toggleIcon"></i>
                    </button>
                    <span>maximus</span>

                </div>

                <!-- Top Navbar -->
                <nav class="navbar navbar-expand-lg navbar-custom">
                    <div class="container-fluid">
                        <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                            <li class="nav-item"><a class="nav-link" href="#">Provider Network Management</a></li>
                            <li class="nav-item"><a class="nav-link" href="#">Medicaid Home</a></li>
                            <li class="nav-item"><a class="nav-link" href="#">Learning</a></li>
                            <li class="nav-item"><a class="nav-link" href="#">Contact</a></li>
                            <li class="nav-item"><a class="nav-link" href="#">Fee Schedule</a></li>
                        </ul>
                    </div>
                </nav>

                <!-- Main Body -->
                <main class="flex-main">
                    <div class="main-container">
                        <div class="row">
                            <!-- Sign In -->
                            <div class="col-md-6">
                                <div class="panel-custom">
                                    <h3>Sign in</h3>
                                    <h4>Access your account</h4>
                                    <strong>
                                        <asp:Label ID="lblErrormessage" Style="color: red" runat="server" Text="* User ID and/or Password does not match our records." Visible="false"></asp:Label>
                                    </strong>

                                    <div class="mb-3" style="margin-bottom: 0rem !important">
                                        <label for="email" class="form-label">User ID</label>
                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" ToolTip="User ID"></asp:TextBox>
                                        <small class="text-danger">
                                            <div tabindex="0" class="error-wrapper">
                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="txtUserName" class="failureNotification"
                                                    ErrorMessage="* User ID is required." ToolTip="User ID is required"
                                                    ValidationGroup="LoginUserValidationGroup" SetFocusOnError="false" aria-hidden="true">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </small>
                                    </div>
                                    <div class="mb-3" style="margin-bottom: 0rem !important">
                                        <label for="password" class="form-label">Password</label>
                                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" ToolTip="Password"></asp:TextBox>
                                        <a href="~/Account/PasswordReset.aspx" runat="server" class="forgot-link">Forgot password?</a>
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
                                        OnClick="btnLogin_Click" CssClass="btn btn-primary" Visible="true" />

                                    <div class="form-help mt-2">
                                        <a href="https://mesc.mpc-release.maximus.com/STATE_SSO/create-account">Don't have an account?</a>
                                    </div>
                                </div>
                            </div>

                            <!-- News -->
                            <div class="col-md-6">
                                <div class="panel-custom">
                                    <h1>Latest News</h1>
                                    <p>
                                        <strong>New Medicaid Self-Service Portal</strong><br>
                                        Your state has launched a new mobile-friendly portal to help members manage their Medicaid benefits, check eligibility, and upload documents securely.
                                    </p>
                                    <p>
                                        <strong>Emergency Assistance for Medicaid Members</strong><br>
                                        Resources are now available for Medicaid recipients affected by recent natural disasters. Visit the Disaster Relief page for help with prescriptions, transportation, and temporary housing.
                                    </p>
                                    <p>
                                        <strong>Telehealth Expansion for Rural Communities</strong><br>
                                        Medicaid has expanded telehealth coverage, making it easier for members in rural areas to access primary care, mental health services, and specialists from home.
                                    </p>
                                </div>
                            </div>
                        </div>

                        <!-- SSO -->
                        <div class="row">
                            <div class="col-12">
                                <div class="panel-custom">
                                    <h5><strong>Need a different way to sign in?</strong></h5>
                                    <p>Use our secure SSO option if you have been provided alternate access credentials.</p>
                                      <asp:Button ID="btnGotoIOP" runat="server" Text="Launch secondary SSO"
                                     OnClick="btnGotoIOP_Click" CssClass="btn btn-secondary" Visible="true" />
                                    
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
                            <div class="footer-column">
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
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

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
        </script>
    </form>
</body>
</html>
