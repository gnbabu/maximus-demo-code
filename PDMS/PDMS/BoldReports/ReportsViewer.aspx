<%@ Page Title="Provider Credentialing Reports Viewer" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="ReportsViewer.aspx.cs" Inherits="BoldReports_ReportsViewer" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/services/credentialing.service.js") %>"></script>

    <!-- Icons -->
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons|Material+Icons+Outlined" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Open+Sans:wght@400;500;600;700&display=swap" rel="stylesheet" />



    <style>
        :root {
            --purple-dark: #3a0e6e;
            --purple-main: #5c1a8c;
            --purple-light: #6a1b9a;
            --purple-lighter: #f3e5f5;
            --purple-footer: #3a0e6e;
            --text-dark: #333;
            --text-muted: #666;
            --bg-page: #f0f0f0;
            --border-color: #e0e0e0;
        }


        body {
            background: #f4f5fa;
            font-size: 14px;
        }

        /* ===== CARDS ===== */
        .page-card {
            background: #fff;
            border: 1px solid #ded9eb;
            border-radius: 8px;
        }

        .page-card-header {
            padding: 12px 16px;
            font-weight: 600;
            color: #56337f;
            border-bottom: 1px solid #e6e1f0;
            display: flex;
            justify-content: space-between;
        }

        /* Container styling */


        /* Header styles */
        h1 {
            color: #6f42c1;
            font-size: 24px;
            font-weight: bold;
        }

        /* Button styling in header */
        .header-buttons .btn {
            background-color: #6f42c1;
            color: white;
            font-size: 14px;
            padding: 8px 15px;
            border-radius: 5px;
            margin-left: 10px;
            display: flex;
            align-items: center;
        }

            .header-buttons .btn:hover {
                background-color: #5a2c9f;
            }

        .header-buttons .material-icons {
            margin-right: 8px;
        }

        /* Card header styles */
        .card-header {
            background-color: #6f42c1;
            color: white;
            font-size: 16px;
            font-weight: 600;
            padding: 15px;
            display: flex;
            align-items: center;
        }

            .card-header .material-icons {
                margin-right: 10px;
                font-size: 22px;
            }

        /* List item styles */
        .list-group-item {
            font-size: 14px;
            padding: 15px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            border: 1px solid #e3e3e3;
            background-color: #fff;
        }

            .list-group-item:hover {
                background-color: #f1f1f1;
            }

        .material-icons {
            /*font-size: 20px;*/
        }


        .content-card {
            background-color: #fff;
            border-radius: 12px;
            border: 1px solid var(--border-color);
            box-shadow: 0 2px 8px rgba(0,0,0,0.06);
            padding: 0px;
        }
        /* Title area */
        .page-container {
            padding: 20px 10px;
            background: #F7F4FA;
            margin-bottom: 20px;
            border: 1px solid #e6e4f2;
        }

        .page-title-icon {
            width: 36px;
            height: 36px;
            border-radius: 50%;
            background-color: #5c1a8c;
            color: #fff;
            display: flex;
            align-items: center;
            justify-content: center;
        }

            .page-title-icon .material-icons-outlined {
                font-size: 20px;
            }



        .page-title {
            font-size: 20px;
            font-weight: 700;
            color: #5c1a8c;
        }

        /* Action buttons */
        .btn-action-outline {
            border: 1.5px solid #5c1a8c;
            color: #5c1a8c;
            background: #fff;
            border-radius: 8px;
            font-size: 13px;
            font-weight: 600;
            padding: 6px 16px;
            transition: all 0.2s;
            display: inline-flex;
            align-items: center;
            gap: 6px;
        }

        .btn-action-outline:hover {
            background-color: var(--purple-lighter);
            color: #5c1a8c;
            border-color: #5c1a8c;
        }

        .btn-action-filled {
            background-color: #5c1a8c;
            color: #fff;
            border: none;
            border-radius: 8px;
            font-size: 13px;
            font-weight: 600;
            padding: 7px 18px;
            transition: all 0.2s;
            display: inline-flex;
            align-items: center;
            gap: 6px;
        }

        .btn-action-filled:hover {
            background-color: var(--purple-dark);
            color: #fff;
        }

        .btn-action-outline .material-icons-outlined,
        .btn-action-filled .material-icons-outlined {
            font-size: 18px;
        }

        /* Report list items */
        .report-item {
            display: flex;
            align-items: center;
            background-color: #fff;
            border: 1px solid var(--border-color);
            border-radius: 8px;
            padding: 12px 16px;
            margin-bottom: 8px;
            transition: box-shadow 0.2s;
            gap: 12px;
        }

            .report-item:hover {
                box-shadow: 0 2px 6px rgba(0,0,0,0.07);
            }

            .report-item .report-icon {
                color: #5c1a8c;
                flex-shrink: 0;
                display: flex;
                align-items: center;
            }

                .report-item .report-icon .material-icons-outlined {
                    font-size: 22px;
                }

            .report-item .report-name {
                font-size: 13px;
                color: var(--text-dark);
                font-weight: 400;
                flex: 1;
            }

            .report-item .btn-more {
                background: none;
                border: none;
                color: var(--text-muted);
                padding: 2px 6px;
                cursor: pointer;
                border-radius: 4px;
                transition: background 0.2s;
                flex-shrink: 0;
                display: flex;
                align-items: center;
            }

                .report-item .btn-more .material-icons-outlined {
                    font-size: 22px;
                }

                .report-item .btn-more:hover {
                    background-color: #f5f5f5;
                }

        .report-list.collapse:not(.show) {
            display: none;
        }

        .report-list.show {
            display: block;
        }

        .btn-icon {
            background: none;
            border: none;
            color: #5c1a8c;
            padding: 4px;
            cursor: pointer;
            border-radius: 4px;
            transition: background 0.2s;
            display: flex;
            align-items: center;
        }

        .report-card {
            background-color: #F7F4FA;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding-left: 25px;
            border: 1px solid #e6e4f2;
        }

        /* Left section: Title and Subtitle */
        .report-left {
            max-width: 60%;
        }

            .report-left h1 {
                font-size: 24px;
                color: #6f42c1; /* Purple color */
                margin: 0;
            }

            .report-left p {
                font-size: 16px;
                color: #6c757d; /* Dark gray */
            }

        /* Right section: Report details with dates */
        .report-right {
            background-color: #ffffff;
            border-radius: 8px;
            padding: 15px;
            margin: 15px 100px;
            min-width: 400px;
            font-weight: 700;
        }

            .report-right h2 {
                font-size: 20px;
                color: #6f42c1; /* Purple color */
                margin-bottom: 10px;
            }

            .report-right .date-label {
                font-size: 12px;
                color: #6f42c1;
                margin-bottom: 0 !important;
            }

            .report-right .date-value {
                font-size: 10px;
                color: #6f42c1;
                margin-bottom: 1px;
            }

        /* Material Icon Styling */
        .material-icons-outlined {
            font-size: 18px;
            vertical-align: middle;
        }

        /* Responsive Design */
        @media (max-width: 768px) {
            .report-card {
                flex-direction: column;
                align-items: center;
            }

            .report-left {
                max-width: 100%;
                text-align: center;
                margin-bottom: 20px;
            }

            .report-right {
                min-width: auto;
            }
        }

        .action-buttons {
            display: flex;
        }

        
        .btn:focus,
        .btn:active:focus,
        .btn-action-filled:focus,
        .btn-action-outline:focus {
            outline: none !important;
            box-shadow: none !important;
        }

    </style>

    <div id="myDrafts" class="container content-card">

        <!-- Page Title Row -->
        <div class="d-flex flex-column flex-sm-row align-items-start align-items-sm-center justify-content-between gap-3 page-container">
            <div class="d-flex align-items-center gap-3">
                <div class="page-title-icon">
                    <span class="material-icons-outlined">assessment</span>
                </div>
                <h1 class="page-title mb-0">Standalone Reports</h1>
            </div>
            <div class="gap-3 action-buttons">
                
                <button type="button" class="btn btn-action-outline download-btn" data-format="pdf">
                    <span class="material-icons-outlined">picture_as_pdf</span> PDF
                </button>
                <button class="btn btn-action-outline" type="button" id="btnMyDrafts">
                    <span class="material-icons-outlined">drafts</span> My Drafts
                </button>
                <button class="btn btn-action-filled" type="button">
                    <span class="material-icons-outlined">note_add</span> New Report
           
                </button>
            </div>
        </div>

        <div>

            <div class="report-card">
                <!-- Left Section: Title and Subtitle -->
                <div class="report-left">
                    <h1>Maximus Credentialing</h1>
                    <p>Operational Insight Reports</p>
                </div>

                <!-- Right Section: Report Details -->
                <div class="report-right">
                    <h2 id="report-title"></h2>

                    <p class="date-label">Report Run Date:</p>
                    <p class="date-value" id="run-date"></p>

                    <p class="date-label">From Date:</p>
                    <p class="date-value" id="from-date"></p>

                    <p class="date-label">To Date:</p>
                    <p class="date-value" id="to-date"></p>
                </div>

            </div>
            <div style="margin: 15px;">
                <div id="report-grid-container"></div>
            </div>

        </div>

    </div>
    <div id="client-loader" class="d-none"
        style="position: fixed; inset: 0; background: rgba(255,255,255,0.6); z-index: 1060; display: flex; align-items: center; justify-content: center;">
        <div style="display: flex; align-items: center; gap: 10px; font-weight: 600; color: #533591;">
            <img src="<%= ResolveUrl("~/Images/ajax-loader.gif") %>" alt="Loading" />
            <span>Loading...</span>
        </div>
    </div>

    
    <!-- Help Links Strip -->
    <section class="help-strip mt-4" aria-label="Help Links">
        <div class="help-strip__inner">
            <!-- Left header -->
            <div class="help-strip__label">
                <span>Help Links</span>
            </div>
            <!-- Tile: We are Hiring -->
            <a class="help-tile" target="_blank" href="https://maximus.avature.net/careers/USHome" title="We are Hiring! (opens new window)" aria-label="work We are Hiring! Click here to know more.">
                <span class="material-symbols-outlined2 help-tile__icon">work</span>
                <span class="help-tile__content">
                    <span class="help-tile__title">We are Hiring!</span>
                    <span class="help-tile__subtitle">Click here to know more.</span>
                </span>
            </a>
            <!-- Tile: Policies & Guidelines -->
            <asp:HyperLink runat="server" ID="lnkPoliciesURLID" class="help-tile" NavigateUrl="~/MesCred/Policies.aspx" aria-label="close_fullscreen Policies Guidelines View Maximus Policies Guidelines.">
                <span class="material-symbols-outlined2 help-tile__icon" >
                    close_fullscreen
                </span>
                <span class="help-tile__content">
                    <span class="help-tile__title">Policies Guidelines</span>
                    <span class="help-tile__subtitle">View Maximus Policies Guidelines</span>
                </span>
            </asp:HyperLink>

            <!-- Tile: Need Help? -->
            <a class="help-tile help-tile-need-help" href="/help" aria-label="help Need Help? Here’s a step-by-step guide to help you understand the process.">
                <span class="material-symbols-outlined2 help-tile__icon" >
                    help
                </span>
                <span class="help-tile__content">
                    <span class="help-tile__title">Need Help?</span>
                    <span class="help-tile__subtitle">
                        Here’s a step-by-step guide to help you understand the process.
                    </span>
                </span>
            </a>

            <!-- ✅ ✅ CUSTOM HELP MODAL (REPLACES RADWINDOW) -->
            <div id="helpModal" class="custom-modal">
                <div id="helpDialog" class="custom-modal-content">

                    <div class="custom-modal-header" id="helpModalHeader">
                        <span id="helpModalTitle">Help</span>
                        <button type="button" class="custom-close" onclick="closeHelpModal()">×</button>
                    </div>

                    <div id="helpModalBody" class="custom-modal-body"></div>

                </div>
            </div>
        </div>
    </section>


    <!-- Vendor JS -->
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.4/moment.min.js"></script>
    
     <script>


         function showHelp() {

             var section = "REPORTS_LANDING_PAGE";
             CredentialingService.getHelpText(section, function (response) {

                 response = response || [];

                 if (response.length === 0) {
                     alert("Help not available.");
                     return;
                 }

                 // ✅ find pdf + text
                 var pdfItem = response.find(x => (x.Mode || "").toLowerCase() === "pdf");
                 var textItem = response.find(x => ["text", "popup"].includes((x.Mode || "").toLowerCase()));

                 // ✅ PDF → open new tab
                 if (pdfItem && pdfItem.PdfUrl) {
                     window.open(pdfItem.PdfUrl, "_blank");
                     return;
                 }

                 // ✅ TEXT / POPUP → show modal
                 if (textItem && textItem.Content) {

                     document.getElementById("helpModalTitle").textContent =
                         textItem.Title || "Help";

                     document.getElementById("helpModalBody").innerHTML =
                         textItem.Content;

                     document.getElementById("helpModal").style.display = "block";
                     return;
                 }

                 alert("Help not available.");

             }, function (err) {

                 console.error("Failed to load help:", err);
                 alert("Failed to load help.");

             });
         }

         function closeHelpModal() {
             document.getElementById("helpModal").style.display = "none";
         }

         $(document).ready(function () {

             $("#helpDialog").draggable({
                 handle: "#helpModalHeader",   // ✅ drag only by header
                 containment: "window",        // ✅ stay inside screen
                 scroll: false
             });

         });

         window.onclick = function (e) {
             var modal = document.getElementById("helpModal");
             if (e.target === modal) {
                 modal.style.display = "none";
             }
         };

         // ✅ Reliable click binding (fixes your issue)
         $(document).ready(function () {

             $('.help-tile-need-help').on('click', function (e) {
                 e.preventDefault();

                 console.log("Help tile clicked");

                 showHelp();
             });

         });

     </script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/services/reports.service.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/pages/report-viewer.js") %>"></script>
</asp:Content>
