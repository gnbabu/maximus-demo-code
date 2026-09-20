<%@ page title="Provider Credentialing Reports" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="BoldReports_Reports" codebehind="Reports.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
</asp:Content>
<asp:Content ID="MainContent" runat="server" ContentPlaceHolderID="MainContent">

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/services/credentialing.service.js") %>"></script>
    <!-- Icons -->
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons|Material+Icons+Outlined" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Open+Sans:wght@400;500;600;700&display=swap" rel="stylesheet" />

    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet" />
    <link href="<%# Page.ResolveClientUrl("~/Styles/accordion/maximus-accordion.css") %>" rel="stylesheet" />
    <link href="<%# Page.ResolveClientUrl("~/Styles/modelPopup/modal.maximus.css") %>" rel="stylesheet" />
    <link href="<%# Page.ResolveClientUrl("~/Styles/datepicker/datepicker.maximus.css") %>" rel="stylesheet" />

    <style>
        :root {
            --purple-dark: #1b4c79;
            --purple-main: #4a4a90;
            --purple-light: #56337f;
            --purple-lighter: #F7F4FA;
            --purple-footer: #1b4c79;
            --text-dark: #333;
            --text-muted: #666;
            --bg-page: #f0f0f0;
            --border-color: #e0e0e0;
        }

        #reportsPageWrapper {
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



        /* Button styling in header */
        .header-buttons .btn {
            background-color: #4a4a90;
            color: white;
            font-size: 14px;
            padding: 8px 15px;
            border-radius: 5px;
            margin-left: 10px;
            display: flex;
            align-items: center;
        }

            .header-buttons .btn:hover {
                background-color: #123a5e;
            }

        .header-buttons .material-icons {
            margin-right: 8px;
        }

        /* Card header styles */
        .card-header {
            background-color: #4a4a90;
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
            background-color: #4a4a90;
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
            color: #4a4a90;
        }

        /* Action buttons */
        .btn-action-outline {
            border: 1.5px solid #4a4a90;
            color: #4a4a90;
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
                color: #4a4a90;
                border-color: #4a4a90;
            }

        .btn-action-filled {
            background-color: #4a4a90;
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
                background-color: #4a4a90;
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
                color: #4a4a90;
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
                color: #333333;
                padding: 2px 6px;
                cursor: pointer;
                border-radius: 4px;
                transition: background 0.2s;
                flex-shrink: 0;
                display: flex;
                align-items: center;
            }

                .report-item .btn-more .material-icons-outlined {
                    color: #333333;
                    font-size: 22px;
                }

                .report-item .btn-more:hover {
                    background-color: #f5f5f5;
                    color: #000000;
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
            color: #4a4a90;
            padding: 4px;
            cursor: pointer;
            border-radius: 4px;
            transition: background 0.2s;
            display: flex;
            align-items: center;
        }



        .unsetPublicSearchDDLLength {
            min-width: 0;
            appearance: revert;
        }

        .action-buttons {
            display: flex;
        }

        .report-actions {
            position: relative;
        }

        .report-dropdown {
            position: absolute;
            top: 100%;
            right: 0;
            background: #fff;
            border: 1px solid #e6e4f2;
            box-shadow: 0 8px 24px rgba(0,0,0,0.18); /* elevated look */
            border-radius: 8px;
            min-width: 140px;
            display: none;
            z-index: 9999;
            padding: 6px 0;
            width: 160px; /* ✅ fixed width */
            max-width: 160px;
        }

            .report-dropdown .dropdown-item {
                display: flex;
                align-items: center;
                gap: 8px;
                padding: 8px 12px;
                font-size: 13px;
                cursor: pointer;
                color: #333;
            }

                .report-dropdown .dropdown-item:hover {
                    background-color: #F7F4FA;
                    color: #4a4a90;
                }

            .report-dropdown .material-icons-outlined {
                font-size: 18px;
            }

        .report-item.active {
            background-color: #F7F4FA; /* soft purple */
            border-color: #4a4a90;
            box-shadow: 0 2px 6px rgba(92, 26, 140, 0.15);
        }


        .form-control {
            border: 1px solid #603d98;
        }

        #reportLoader {
            position: fixed;
            inset: 0;
            z-index: 9999;
        }

        .loader-backdrop {
            position: absolute;
            inset: 0;
            background: rgba(0,0,0,0.5);
        }

        .loader-content {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            text-align: center;
            color: white;
        }

        .spinner {
            width: 50px;
            height: 50px;
            border: 5px solid #ccc;
            border-top-color: #2F80ED;
            border-radius: 50%;
            animation: spin 1s linear infinite;
            margin: 0 auto 15px;
        }

        @keyframes spin {
            to {
                transform: rotate(360deg);
            }
        }

        .loader-text {
            font-size: 16px;
            font-weight: 500;
        }

        input[type="text"], input::placeholder {
            color: #424242 !important;
        }
        /* =========================================================
   BOOTSTRAP 3 PAGE HEADER OVERRIDES
   Add this at the end of the existing style block
   ========================================================= */

        .page-container {
            display: flex;
            align-items: center;
            justify-content: space-between;
            flex-wrap: wrap;
            gap: 12px;
            width: 100%;
            padding: 20px 15px;
            margin-bottom: 20px;
            background-color: #f7f4fa;
            border: 1px solid #e6e4f2;
            border-radius: 6px;
        }

        /* Left-side heading */

        .page-heading {
            display: table;
        }

            .page-heading .page-title-icon,
            .page-heading .page-title {
                display: table-cell;
                vertical-align: middle;
            }

            .page-heading .page-title-icon {
                width: 36px;
                height: 36px;
                color: #ffffff;
                text-align: center;
                background-color: #4a4a90;
                border-radius: 50%;
            }

                .page-heading .page-title-icon .material-icons-outlined {
                    display: block;
                    font-size: 20px;
                    line-height: 36px;
                }

            .page-heading .page-title {
                padding-left: 12px;
                margin: 0;
                color: #4a4a90;
                font-size: 20px;
                font-weight: 700;
                line-height: 36px;
            }

        /* Right-side buttons */

        .action-buttons {
            display: flex;
            align-items: center;
            gap: 8px;
            flex-wrap: wrap;
            white-space: nowrap;
        }

            .action-buttons .page-button {
                display: inline-flex;
                align-items: center;
                vertical-align: middle;
                text-decoration: none;
            }

            .action-buttons .material-icons-outlined {
                display: inline-block;
                margin-right: 5px;
                font-size: 18px;
                line-height: 1;
                vertical-align: middle;
            }

            /* Outline button */

            .action-buttons .btn-action-outline {
                padding: 7px 16px;
                color: #4a4a90;
                background-color: #ffffff;
                border: 1px solid #4a4a90;
                border-radius: 6px;
                font-size: 13px;
                font-weight: 600;
            }

                .action-buttons .btn-action-outline:hover,
                .action-buttons .btn-action-outline:focus,
                .action-buttons .btn-action-outline:active {
                    color: #4a4a90;
                    background-color: #F7F4FA;
                    border-color: #4a4a90;
                    text-decoration: none;
                    outline: none;
                }

            /* Filled button */

            .action-buttons .btn-action-filled {
                padding: 8px 18px;
                color: #ffffff;
                background-color: #4a4a90;
                border: 0;
                border-radius: 6px;
                font-size: 13px;
                font-weight: 600;
            }

                .action-buttons .btn-action-filled:hover,
                .action-buttons .btn-action-filled:focus,
                .action-buttons .btn-action-filled:active {
                    color: #ffffff;
                    background-color: #123a5e;
                    text-decoration: none;
                    outline: none;
                }

        /* Tablet and mobile */

        @media (max-width: 767px) {

            .page-container {
                padding: 15px;
            }

            .page-heading,
            .action-buttons {
                float: none !important;
                width: 100%;
            }

            .action-buttons {
                padding-top: 0;
                margin-top: 15px;
                white-space: normal;
            }

                .action-buttons .page-button {
                    display: block;
                    width: 100%;
                    margin: 0 0 10px;
                    text-align: center;
                }

                    .action-buttons .page-button:last-child {
                        margin-bottom: 0;
                    }
        }
        /* =========================================================
   PAGE LAYOUT AND FOOTER OVERLAP FIX
   ========================================================= */

        html {
            position: relative;
            min-height: 100%;
        }

        body {
            min-height: 100%;
        }

        /*
   Main wrapper for this page.
   Increase padding-bottom if the footer in MasterPage.master
   uses position: fixed.
*/
        .reports-page-wrapper {
            position: relative;
            width: 100%;
            min-height: 400px;
            padding-top: 15px;
            padding-bottom: 80px;
            clear: both;
        }

            .reports-page-wrapper:before,
            .reports-page-wrapper:after {
                display: table;
                content: " ";
            }

            .reports-page-wrapper:after {
                clear: both;
            }

        /* Main report cards */

        #reportManagement,
        #myDrafts {
            position: relative;
            float: none;
            min-height: 1px;
            margin-right: auto;
            margin-bottom: 30px;
            margin-left: auto;
            padding: 0;
            overflow: visible;
            clear: both;
        }

            #reportManagement:before,
            #reportManagement:after,
            #myDrafts:before,
            #myDrafts:after {
                display: table;
                content: " ";
            }

            #reportManagement:after,
            #myDrafts:after {
                clear: both;
            }

        .content-card {
            background-color: #ffffff;
            border: 1px solid #e0e0e0;
            border-radius: 12px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
        }

        /* Page header */

        .page-container {
            display: flex;
            align-items: center;
            justify-content: space-between;
            flex-wrap: wrap;
            gap: 12px;
            width: 100%;
            margin: 0 0 20px;
            padding: 20px 15px;
            background-color: #f7f4fa;
            border: 1px solid #e6e4f2;
            border-radius: 10px 10px 0 0;
        }

        .page-heading {
            display: table;
        }

            .page-heading .page-title-icon,
            .page-heading .page-title {
                display: table-cell;
                vertical-align: middle;
            }

            .page-heading .page-title-icon {
                width: 36px;
                height: 36px;
                color: #ffffff;
                text-align: center;
                background-color: #4a4a90;
                border-radius: 50%;
            }

                .page-heading .page-title-icon .material-icons-outlined {
                    display: block;
                    font-size: 20px;
                    line-height: 36px;
                }

            .page-heading .page-title {
                margin: 0;
                padding-left: 12px;
                color: #4a4a90;
                font-size: 20px;
                font-weight: 700;
                line-height: 36px;
            }

        /* Header buttons */

        .action-buttons {
            display: flex;
            align-items: center;
            gap: 8px;
            flex-wrap: wrap;
            white-space: nowrap;
        }

            .action-buttons .page-button {
                display: inline-flex;
                align-items: center;
                vertical-align: middle;
                text-decoration: none;
            }

            .action-buttons .material-icons-outlined {
                display: inline-block;
                margin-right: 5px;
                font-size: 18px;
                line-height: 1;
                vertical-align: middle;
            }

            .action-buttons .btn-action-outline {
                padding: 7px 16px;
                color: #4a4a90;
                background-color: #ffffff;
                border: 1px solid #4a4a90;
                border-radius: 6px;
                font-size: 13px;
                font-weight: 600;
            }

                .action-buttons .btn-action-outline:hover,
                .action-buttons .btn-action-outline:focus,
                .action-buttons .btn-action-outline:active {
                    color: #4a4a90;
                    background-color: #F7F4FA;
                    border-color: #4a4a90;
                    text-decoration: none;
                    outline: none;
                }

            .action-buttons .btn-action-filled {
                padding: 8px 18px;
                color: #ffffff;
                background-color: #4a4a90;
                border: 1px solid #4a4a90;
                border-radius: 6px;
                font-size: 13px;
                font-weight: 600;
            }

                .action-buttons .btn-action-filled:hover,
                .action-buttons .btn-action-filled:focus,
                .action-buttons .btn-action-filled:active {
                    color: #ffffff;
                    background-color: #123a5e;
                    border-color: #123a5e;
                    text-decoration: none;
                    outline: none;
                }

        /* Accordion spacing */

        #standardReportsAccordion,
        #customReportsAccordion,
        #draftReportsAccordion {
            margin-right: 15px;
            margin-bottom: 20px;
            margin-left: 15px;
        }

        /* Report items: Bootstrap 3-compatible flex fallback */

        .report-item {
            position: relative;
            display: table;
            width: 100%;
            margin-bottom: 8px;
            padding: 12px 16px;
            background-color: #ffffff;
            border: 1px solid #e0e0e0;
            border-radius: 8px;
        }

            .report-item .report-icon,
            .report-item .report-name,
            .report-item .report-actions-wrapper {
                display: table-cell;
                vertical-align: middle;
            }

            .report-item .report-icon {
                width: 34px;
                color: #4a4a90;
            }

            .report-item .report-name {
                width: auto;
                color: #333333;
                font-size: 13px;
            }

            .report-item .report-actions-wrapper {
                width: 40px;
                text-align: right;
            }

        /* Bootstrap 3 collapse uses .in, not .show */

        .report-list.collapse {
            display: none;
        }

            .report-list.collapse.in {
                display: block;
            }

        /* Help strip must remain in normal page flow */

        .help-strip {
            position: relative;
            float: none;
            width: 100%;
            margin-top: 20px;
            margin-bottom: 30px;
            clear: both;
        }

        /* Loader compatibility */

        #reportLoader {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            z-index: 99999;
        }

        .loader-backdrop {
            position: absolute;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            background-color: rgba(0, 0, 0, 0.5);
        }

        /* Mobile */

        @media (max-width: 767px) {
            .reports-page-wrapper {
                padding-right: 10px;
                padding-bottom: 100px;
                padding-left: 10px;
            }

            #reportManagement,
            #myDrafts {
                width: 100%;
            }

            .page-container {
                padding: 15px;
            }

            .page-heading,
            .action-buttons {
                float: none !important;
                width: 100%;
            }

            .action-buttons {
                margin-top: 15px;
                padding-top: 0;
                white-space: normal;
            }

                .action-buttons .page-button {
                    display: block;
                    width: 100%;
                    margin: 0 0 10px;
                    text-align: center;
                }

                    .action-buttons .page-button:last-child {
                        margin-bottom: 0;
                    }

            #standardReportsAccordion,
            #customReportsAccordion,
            #draftReportsAccordion {
                margin-right: 10px;
                margin-left: 10px;
            }
        }
    </style>


    <div id="reportsPageWrapper" class="reports-page-wrapper clearfix">

        <%--Report Management--%>
        <div id="reportManagement" class="container content-card clearfix">


            <!-- Page Title Row -->
            <div class="page-container clearfix">

                <div class="page-heading pull-left">
                    <div class="page-title-icon">
                        <span class="material-icons-outlined">assessment</span>
                    </div>

                    <h1 class="page-title">Report Management</h1>
                </div>

                <div class="action-buttons pull-right">

                    <a href="javascript:void(0);"
                        id="btnAdhocQuery"
                        class="btn btn-action-filled page-button"
                        role="button">
                        <span class="material-icons-outlined">search</span>
                        <span>Ad-hoc Query</span>
                    </a>

                    <a href="javascript:void(0);"
                        id="btnMyDrafts"
                        class="btn btn-action-outline page-button"
                        role="button">
                        <span class="material-icons-outlined">drafts</span>
                        <span>My Drafts</span>
                    </a>

                    <a href="javascript:void(0);"
                        id="newReportBtn"
                        class="btn btn-action-filled page-button"
                        role="button">
                        <span class="material-icons-outlined">note_add</span>
                        <span>New Report</span>
                    </a>

                </div>

            </div>


            <!-- Standard Reports -->
            <div id="standardReportsAccordion" class="maximus-accordion">
                <div class="maximus-accordion-item">
                    <div class="maximus-accordion-header">
                        <span class="maximus-accordion-bar"></span>
                        <span class="maximus-accordion-title">Standard Reports</span>
                        <span class="maximus-accordion-arrow">
                            <span class="material-icons">expand_more</span>
                        </span>
                    </div>


                    <div class="maximus-accordion-content">
                        <div id="standardReportsByCategory"></div>
                        <%--                    <div class="p-3" id="standardReportsList">
                        <!-- We will inject items here -->
                    </div>--%>
                    </div>
                </div>
            </div>

            <!-- Custom Reports -->

            <div id="customReportsAccordion" class="maximus-accordion">
                <div class="maximus-accordion-item">
                    <div class="maximus-accordion-header">
                        <span class="maximus-accordion-bar"></span>
                        <span class="maximus-accordion-title">Custom Reports</span>
                        <span class="maximus-accordion-arrow">
                            <span class="material-icons">expand_more</span>
                        </span>
                    </div>
                    <div class="maximus-accordion-content">
                        <div id="customReportsByCategory"></div>
                        <%--<div class="p-3" id="customReportsList">
                        <!-- Items injected here -->
                    </div>--%>
                    </div>
                </div>
            </div>


        </div>

        <%--My Drafts--%>
        <div id="myDrafts" class="container content-card clearfix" style="display: none;">

            <!-- Page Title Row -->
            <!-- My Drafts Page Title Row -->
            <div class="page-container clearfix">

                <div class="page-heading pull-left">
                    <div class="page-title-icon">
                        <span class="material-icons-outlined">drafts</span>
                    </div>

                    <h1 class="page-title">My Drafts</h1>
                </div>

                <div class="action-buttons pull-right">

                    <a href="javascript:void(0);"
                        id="btnAdhocQuery2"
                        class="btn btn-action-filled page-button"
                        role="button">
                        <span class="material-icons-outlined">search</span>
                        <span>Ad-hoc Query</span>
                    </a>

                    <a href="javascript:void(0);"
                        id="btnReportManagement"
                        class="btn btn-action-outline page-button"
                        role="button">
                        <span class="material-icons-outlined">assessment</span>
                        <span>Report Management</span>
                    </a>

                    <a href="javascript:void(0);"
                        id="newReportBtn2"
                        class="btn btn-action-filled page-button"
                        role="button">
                        <span class="material-icons-outlined">note_add</span>
                        <span>New Report</span>
                    </a>

                </div>

            </div>


            <!-- Draft Reports -->
            <div id="draftReportsAccordion" class="maximus-accordion">
                <div class="maximus-accordion-item">
                    <div class="maximus-accordion-header">
                        <span class="maximus-accordion-bar"></span>
                        <span class="maximus-accordion-title">Draft Reports</span>
                        <span class="maximus-accordion-arrow">
                            <span class="material-icons">expand_more</span>
                        </span>
                    </div>

                    <div class="maximus-accordion-content">
                        <div id="draftReportsByCategory"></div>
                    </div>
                </div>
            </div>

        </div>

        <!-- Modal 2: Select Template -->
        <div class="modal fade" id="selectTemplateModal" tabindex="-1" aria-labelledby="selectTemplateLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h3 class="modal-title" id="selectTemplateLabel">Select Template</h3>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="templateSelect" class="control-label">Template*</label>
                            <select class="form-control" style="height: 42px;" id="templateSelect" required="required">
                                <option value="" selected="selected">Select Template</option>
                            </select>
                            <div id="templateError" class="text-danger small" style="display: none;">
                                Template is required
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="reportTypeSelect" class="control-label">Report Type*</label>
                            <select class="form-control" style="height: 42px;" id="reportTypeSelect" required="required">
                                <option value="" selected="selected">Select Report Type</option>
                                <option value="Standard Reports">Standard Reports</option>
                                <option value="Custom Reports">Custom Reports</option>
                                <option value="Draft Reports">Draft Reports</option>
                            </select>
                            <div id="reportTypeError" class="text-danger small" style="display: none;">
                                Report Type is required
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="reportName" class="control-label">Report Name*</label>
                            <input type="text" class="form-control" style="color: #424242 !important;" id="reportName" maxlength="100" placeholder="Enter Report Name" required="required" />
                            <div id="reportNameError" class="text-danger small" style="display: none;">
                                Report Name is required
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <!-- Cancel button with Material Icon -->
                        <button type="button" class="btn btn-secondary popup-button" id="cancelSelectTemplate" data-dismiss="modal">
                            <span class="material-icons" style="margin-right: 8px;">cancel</span> Cancel
                        </button>
                        <!-- Save and Close button with Material Icon -->
                        <button type="button" class="btn btn-secondary popup-button" id="saveAndCloseTemplate">
                            <span class="material-icons" style="margin-right: 8px;">check_circle</span> Confirm
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="reportLoader" style="display: none;">
            <div class="loader-backdrop"></div>
            <div class="loader-content">
                <div class="spinner"></div>
                <div class="loader-text">
                    Creating your report…<br />
                    Redirecting you to the editor
                </div>
            </div>
        </div>

        <!-- Help Links Strip -->
        <%--<section class="help-strip" style="margin-top: 20px;" aria-label="Help Links">
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
                <asp:HyperLink runat="server" ID="lnkPoliciesURLID" class="help-tile" NavigateUrl="~/Policies.aspx" aria-label="close_fullscreen Policies Guidelines View Maximus Policies Guidelines.">
                    <span class="material-symbols-outlined2 help-tile__icon">close_fullscreen
                    </span>
                    <span class="help-tile__content">
                        <span class="help-tile__title">Policies Guidelines</span>
                        <span class="help-tile__subtitle">View Maximus Policies Guidelines</span>
                    </span>
                </asp:HyperLink>

                <!-- Tile: Need Help? -->
                <a class="help-tile help-tile-need-help" href="/help" aria-label="help Need Help? Here’s a step-by-step guide to help you understand the process.">
                    <span class="material-symbols-outlined2 help-tile__icon">help
                    </span>
                    <span class="help-tile__content">
                        <span class="help-tile__title">Need Help?</span>
                        <span class="help-tile__subtitle">Here’s a step-by-step guide to help you understand the process.
                        </span>
                    </span>
                </a>

                <!-- ✅ ✅ CUSTOM HELP MODAL (REPLACES RADWINDOW) -->
                <div id="helpModal" class="custom-modal">
                    <div id="helpDialog" class="custom-modal-content">

                        <div class="custom-modal-header" id="helpModalHeader">
                            <span id="helpModalTitle">Help</span>
                            <button type="button" class="custom-close"
                                aria-label="Close help dialog"
                                onclick="closeHelpModal()">
                                ×</button>
                        </div>

                        <div id="helpModalBody" class="custom-modal-body"></div>

                    </div>
                </div>
            </div>
        </section>--%>
        <asp:HiddenField ID="hfIsView" runat="server" Value='<%# (User.IsInRole("ODMCredentialingSupervisor") || User.IsInRole("CredentialingSupervisor") || User.IsInRole("ODMStateAdministrator")) %>' />
        <asp:HiddenField ID="hfIsEdit" runat="server" Value='<%# User.IsInRole("ODMStateAdministrator") %>' />
    </div>
    <script>

        $('#reportName').on('input', function () {
            $('#reportNameError').toggle(!$(this).val().trim());
        });

        $('#templateSelect').on('change', function () {
            $('#templateError').toggle(!$(this).val());
        });

        $('#reportTypeSelect').on('change', function () {
            $('#reportTypeError').toggle(!$(this).val());
        });

        // Allow only letters, numbers, spaces, and dashes
        $('#reportName').on('keypress', function (e) {
            const char = String.fromCharCode(e.which);

            if (!/[A-Za-z0-9\- ]/.test(char)) {
                e.preventDefault();
            }
        });

        // Handle paste
        $('#reportName').on('paste', function (e) {
            e.preventDefault();

            let pastedText = (e.originalEvent || e).clipboardData.getData('text');

            // Remove invalid characters
            pastedText = pastedText.replace(/[^A-Za-z0-9\- ]/g, '');

            // Limit to 100 characters
            pastedText = pastedText.substring(0, 100);

            const currentValue = $(this).val();
            const remaining = 100 - currentValue.length;

            $(this).val(currentValue + pastedText.substring(0, remaining));
        });

        // Safety net for typing, drag/drop, autofill, etc.
        $('#reportName').on('input', function () {
            let value = $(this).val();

            value = value
                .replace(/[^A-Za-z0-9\- ]/g, '') // only alphanumeric, space, dash
                .substring(0, 100);             // max 100 chars

            $(this).val(value);
        });

        const canEdit = $('#<%= hfIsEdit.ClientID %>').val() === "True";
        const canView = $('#<%= hfIsView.ClientID %>').val() === "True";

        console.log(canView);
        console.log(canEdit);

        $('#selectTemplateModal').on('show.bs.modal', function () {

            // If already populated, skip reload
            if ($('#templateSelect option').length <= 1) {
                loadBoldReportsByCategory();  // optional reload
            }

        });

        // ===============================
        // ✅ Bold Reports Integration
        // ===============================

        const API_BASE = window.WEB_API_URL;
        let cachedToken = null;

        // ✅ TOKEN FETCH
        async function getBoldToken() {

            if (cachedToken) return cachedToken;

            const res = await fetch(`${API_BASE}reports/api/token`);

            if (!res.ok) {
                throw new Error("Token fetch failed");
            }

            const tokenObj = await res.json();
            cachedToken = tokenObj.access_token;

            return cachedToken;
        }

        // ✅ GENERIC API CALL
        async function boldApiGet(url) {

            const token = await getBoldToken();

            const res = await fetch(url, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            });

            if (!res.ok) {
                throw new Error(await res.text());
            }

            return await res.json();
        }

        // ✅ NEW: Category-based loader (parallel)
        async function loadBoldReportsByCategory() {

            try {

                const categories = await boldApiGet(`${API_BASE}reports/api/UserCategories`);

                // clear containers
                $('#standardReportsByCategory').html('');
                $('#customReportsByCategory').html('');
                $('#draftReportsByCategory').html('');

                for (let cat of categories) {

                    const categoryName = cat.Name || "";

                    const reports = await boldApiGet(
                        `${API_BASE}reports/api/UserReports?category=${encodeURIComponent(categoryName)}`
                    );

                    // ✅ HANDLE TEMPLATE DROPDOWN HERE
                    if (categoryName === "Templates") {

                        const ddl = $('#templateSelect');

                        ddl.empty();
                        ddl.append(`<option value="" selected="selected">Select Template</option>`);

                        if (!reports || reports.length === 0) {

                            ddl.append(`<option disabled>No templates available</option>`);

                        } else {

                            for (let rpt of reports) {

                                ddl.append(`
                                    <option value="${rpt.Id}">
                                        ${rpt.Name}
                                    </option>
                                `);
                            }
                        }
                    }

                    let categoryHtml = '';

                    for (let rpt of reports) {

                        categoryHtml += `
    <div class="report-item">

        <span class="report-icon">
            <span class="material-icons-outlined">description</span>
        </span>

        <span class="report-name">${rpt.Name}</span>

        <div class="report-actions-wrapper">
            <div class="report-actions">

                <button type="button"
                        class="btn-more report-menu-toggle"
                        aria-label="Report actions">
                    <span class="material-icons-outlined">more_vert</span>
                </button>

                <div class="report-dropdown">

                    ${rpt.CanRead && canView ? `
                        <div class="dropdown-item"
                             onclick="handleReportAction(this, 'view', '${rpt.Id}', '${categoryName}', '${rpt.Name}', '${rpt.IsDraft}')">
                            <span class="material-icons-outlined">visibility</span>
                            View
                        </div>
                    ` : ""}

                    ${rpt.CanWrite && canEdit ? `
                        <div class="dropdown-item"
                             onclick="handleReportAction(this, 'edit', '${rpt.Id}', '${categoryName}', '${rpt.Name}', '${rpt.IsDraft}')">
                            <span class="material-icons-outlined">edit</span>
                            Edit
                        </div>

                        <div class="dropdown-item"
                             onclick="handleReportAction(this, 'delete', '${rpt.Id}', '${categoryName}', '${rpt.Name}', '${rpt.IsDraft}')">
                            <span class="material-icons-outlined">delete</span>
                            Delete
                        </div>
                    ` : ""}

                </div>
            </div>
        </div>

    </div>
`;
                    }

                    // ✅ ✅ FIXED ROUTING
                    const categoryMap = {
                        "Standard Reports": "#standardReportsByCategory",
                        "Custom Reports": "#customReportsByCategory",
                        "Draft Reports": "#draftReportsByCategory"
                    };

                    const target = categoryMap[categoryName];

                    if (target) {
                        $(target).append(categoryHtml);
                    } else {
                        console.warn("Unknown category:", categoryName);
                    }

                }



                // ✅ FORCE CHECK BASED ON DOM (more reliable)

                if ($('#standardReportsByCategory').children().length === 0) {
                    $('#standardReportsByCategory').html(`
                            <div class="text-center text-muted" style="padding: 20px 0;">
                                There are no reports to display
                            </div>
                        `);
                }

                if ($('#customReportsByCategory').children().length === 0) {
                    $('#customReportsByCategory').html(`
                            <div class="text-center text-muted" style="padding: 20px 0;">
                                There are no reports to display
                            </div>
                        `);
                }

                if ($('#draftReportsByCategory').children().length === 0) {
                    $('#draftReportsByCategory').html(`
                            <div class="text-center text-muted" style="padding: 20px 0;">
                                There are no reports to display
                            </div>
                        `);
                }

            } catch (err) {
                console.error("Category load failed:", err);
            }
        }

        function handleReportAction(el, action, id, category, name, isDraft) {

            // ✅ Remove highlight from all rows
            $('.report-item').removeClass('active');

            // ✅ Find parent report row and highlight it
            const row = $(el).closest('.report-item');
            row.addClass('active');

            // ✅ Close dropdown
            row.find('.report-dropdown').hide();

            // ✅ Navigate (small delay helps UX)
            setTimeout(() => {
                if (action === 'view') {
                    viewBoldReport(id, category, name, isDraft);
                } else if (action === 'edit') {
                    editBoldReport(id, category, name, isDraft);
                } else if (action === 'delete') {
                    deleteBoldReport(id, category, name, isDraft);
                }
            }, 150);
        }

        // ✅ NAVIGATION (KEEPING YOUR WebForms ROUTING)
        function viewBoldReport(id, category, name, isDraft) {

            window.location.href =
                `ViewReport.aspx?id=${encodeURIComponent(id)}`
                + `&category=${encodeURIComponent(category)}`
                + `&name=${encodeURIComponent(name)}`
                + `&isDraft=${isDraft}`;
        }

        function editBoldReport(id, category, name, isDraft) {

            window.location.href =
                `EditReport.aspx?id=${encodeURIComponent(id)}`
                + `&category=${encodeURIComponent(category)}`
                + `&name=${encodeURIComponent(name)}`
                + `&isDraft=${isDraft}`;
        }

        async function deleteBoldReport(id, category, name, isDraft) {

            if (!confirm(`Are you sure you want to delete "${name}"?`)) {
                return;
            }

            try {

                const token = await getBoldToken();

                const response = await fetch(
                    `${API_BASE}reports/api/DeleteReport`,
                    {
                        method: 'POST',
                        headers: {
                            'Authorization': `Bearer ${token}`,
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify({
                            id: id
                        })
                    });

                if (!response.ok) {
                    throw new Error(await response.text());
                }

                alert('Report deleted successfully.');

                loadBoldReportsByCategory();

            } catch (err) {

                console.error(err);
                alert('Failed to delete report.');
            }
        }

        // ✅ INIT
        $(document).ready(function () {
          try {
            loadBoldReportsByCategory();


            if (!canEdit) {
                $('#newReportBtn').hide();
                $('#newReportBtn2').hide();
            }
          } catch (e) {
            console.error('[Reports.aspx] init error:', e);
          }
        });

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
          try {
            $("#helpDialog").draggable({
                handle: "#helpModalHeader",   // ✅ drag only by header
                containment: "window",        // ✅ stay inside screen
                scroll: false
            });
          } catch (e) {
            console.error('[Reports.aspx] draggable init error:', e);
          }
        });

        window.onclick = function (e) {
            var modal = document.getElementById("helpModal");
            if (e.target === modal) {
                modal.style.display = "none";
            }
        };

        // ✅ Reliable click binding (fixes your issue)
        $(document).ready(function () {
          try {
            $('.help-tile-need-help').on('click', function (e) {
                e.preventDefault();

                console.log("Help tile clicked");

                showHelp();
            });
          } catch (e) {
            console.error('[Reports.aspx] help-tile click binding error:', e);
          }
        });

        $(document).on('click', '.report-menu-toggle', function (e) {

            e.stopPropagation();

            const btn = $(this);
            const dropdown = btn.siblings('.report-dropdown');
            const parent = btn.closest('.report-actions'); // ✅ capture correct parent

            // ✅ reset others (send them back properly)
            $('.report-dropdown').each(function () {
                const el = $(this);

                // restore only if moved to body
                if (el.parent()[0] === document.body) {
                    el.hide().appendTo(el.data('parent'));
                } else {
                    el.hide();
                }
            });

            // ✅ store original parent (only once)
            if (!dropdown.data('parent')) {
                dropdown.data('parent', parent);
            }

            // ✅ temp show for width calc
            dropdown.css({
                display: 'block',
                visibility: 'hidden'
            }).appendTo('body');

            const offset = btn.offset();
            const btnWidth = btn.outerWidth();
            const dropdownWidth = dropdown.outerWidth();

            let left = offset.left + btnWidth - dropdownWidth;
            let top = offset.top + btn.outerHeight();

            const screenWidth = $(window).width();

            if (left + dropdownWidth > screenWidth - 10) {
                left = screenWidth - dropdownWidth - 10;
            }

            if (left < 10) {
                left = 10;
            }

            dropdown.css({
                position: 'absolute',
                top: top,
                left: left,
                visibility: 'visible',
                display: 'block',
                zIndex: 999999
            });

        });

        // ✅ Close when clicking outside
        $(document).on('click', function () {

            $('.report-dropdown').each(function () {

                const el = $(this);

                if (el.parent()[0] === document.body && el.data('parent')) {
                    el.hide().appendTo(el.data('parent'));
                } else {
                    el.hide();
                }

            });

        });

        function showLoader(message) {
            if (message) {
                $('#reportLoader .loader-text').html(message);
            }
            $('#reportLoader').fadeIn(150);
        }

        function hideLoader() {
            $('#reportLoader').fadeOut(150);
        }

        $('#btnAdhocQuery').on('click', function () {

            showLoader("Loading Query Designer...<br/>Please wait");

            setTimeout(function () {
                window.location.href = "QueryDesigner.aspx";
            }, 300);
        });

        $('#btnAdhocQuery2').on('click', function () {

            showLoader("Loading Query Designer...<br/>Please wait");

            setTimeout(function () {
                window.location.href = "QueryDesigner.aspx";
            }, 300);
        });

    </script>

    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/services/reports.service.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/pages/report-management.js") %>"></script>

</asp:Content>
