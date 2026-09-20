<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CostReportsStub.aspx.cs" Inherits="StateSingleSignOn.CostReportsStub" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>Projects</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width" />
    <!--[if lt IE 9]>
            <meta http-equiv="X-UA-Compatible" content="IE=8" />
            <script src="~/Scripts/json2.js"></script>
        <![endif]-->
    <!--[if (gte IE 9) | (!IE) | IE 9]><!-->
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <!--<![endif]-->
    <link rel='icon' href='/favicon.ico' type='image/x-icon'>
    <link rel='shortcut icon' href='/favicon.ico' type='image/x-icon' />
    <script src="https://ohiofi.mslc.com/Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="https://ohiofi.mslc.com/Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="https://ohiofi.mslc.com/Scripts/jquery.validate.unobtrusive.min.js" type="text/javascript"></script>
    <script src="https://ohiofi.mslc.com/Scripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="https://ohiofi.mslc.com/Scripts/site.js" type="text/javascript"></script>
    <script src="https://ohiofi.mslc.com/Scripts/modernizr-2.8.3.js" type="text/javascript"></script>
    <script src="https://ohiofi.mslc.com/Scripts/bootstrap-datepicker.js" type="text/javascript"></script>
    <script src="https://ohiofi.mslc.com/Scripts/date.js" type="text/javascript"></script>
    
    <script src="https://ohiofi.mslc.com/Scripts/jquery.cookie.js" type="text/javascript"></script>
    <script src="https://ohiofi.mslc.com/Scripts/Pages/Index.js" type="text/javascript"></script>
    <script src="https://ohiofi.mslc.com/Scripts/Pages/LogOut.js" type="text/javascript"></script>
    <script src="https://ohiofi.mslc.com/Scripts/moment.min.js" type="text/javascript"></script>
      
    <script src="https://ohiofi.mslc.com/Scripts/jqGrid/grid.locale-en.js"></script>
    <script src="https://ohiofi.mslc.com/Scripts/jqGrid/jquery.jqGrid.min.js"></script>
    <script src="https://ohiofi.mslc.com/Scripts/jqGrid/grid.custom.js"></script>
    <script src="https://ohiofi.mslc.com/Content/feather-icons/dist/feather.min.js"></script>  
    <link href="https://ohiofi.mslc.com/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="https://ohiofi.mslc.com/Content/datepicker.css" rel="stylesheet" type="text/css" />
    
    <link href="https://ohiofi.mslc.com/Content/ui.jqgrid.css" rel="stylesheet" type="text/css" />
    <script src="https://ohiofi.mslc.com/Scripts/jsrender.min.js"></script>

    <script src="https://ohiofi.mslc.com/Scripts/ej/web/ej.web.all.min.js"></script>

    
    <link href="https://ohiofi.mslc.com/Content/buttons.css" rel="stylesheet" type="text/css" />
    <link href="https://ohiofi.mslc.com/Content/south-street/jquery-ui-1.9.2.custom.css" rel="stylesheet" type="text/css" />
    <link href="https://ohiofi.mslc.com/Content/ej/web/bootstrap-theme/ej.web.all.min.css" rel="stylesheet"/>

    <link href="https://ohiofi.mslc.com/Content/default.css" rel="stylesheet" type="text/css" />
    <link href="https://ohiofi.mslc.com/Content/buttons.css" rel="stylesheet" type="text/css" />
    <link href="https://ohiofi.mslc.com/Content/site.css" rel="stylesheet" type="text/css" />
    <link href="https://ohiofi.mslc.com/Content/StatLog.css" rel="stylesheet" type="text/css" />
    <script>
        $(document).ready(function () {
            $("input").attr("autocomplete", "off");
        });
    </script>
</head>
<body style="color: #696969">
    <div class="overlay">
        <!-- This is the "spinner" area to show that we are doing something in the background. -->
        <div class="overlay-inset"></div>
        <div class="overlay-banner">
            <p></p>
            <img src="https://ohiofi.mslc.com/Images/loading.gif" />
        </div>
    </div>

    <input id="logoutURL" type="hidden" value="https://ohiofi.mslc.com/Projects/LogOut" />
    <input id="hdnHeartBeat" type="hidden" value="https://ohiofi.mslc.com/Home/HeartBeat" />
    <input id="hdnWarningSeconds" type="hidden" value="12933" />
    <input id="hdnTimeOutSeconds" type="hidden" value="14370" />
    <a id="aLogOutWarning" data-toggle="modal" href="#window_LogOutWarning"></a>

    <div id="window_LogOutWarning" class="modal fade">
        <div class="modal-dialog" role="document">
            <div class="modal-content">

                <div class="modal-header">
                    <h3>Session Warning.</h3>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div>
                        Your session is about to expire. Time:  <span id="SecondsTilTimeOut"></span>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" id="aContinueSession" class="btn btn-success" data-dismiss="modal">Continue Session</button>
                    <a href="https://ohiofi.mslc.com/Projects/LogOut" class="btn btn-success">Log Out</a>
                </div>
            </div>
        </div>
    </div>

    <div id="window_HelpNumber" class="modal fade">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h3>Need Assistance?</h3>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div>
                        Please contact Customer Support at OMESCR.RSwebportal@mslc.com
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div id="dialogDiv" class="MSLCModal modal hide fade" tabindex="-1" aria-labelledby="windowTitleLabel" aria-hidden="true">
        <div id="modal-dialog" class="modal-dialog" role="document">
            <div id="dialogContent" style="font-size: 14px; width:100%"></div>
            <div id="dialogMessage" style="font-size: 14px; width:100%"></div>
        </div>
    </div>

    <div class="page-layout">
        <div style="background-color: #74c141; height: 40px;"></div>
        <div style="border-bottom: 1px dashed #ccc; position: relative;">
            <img src="https://ohiofi.mslc.com/images/logo.png" width="500" height="80">
            <div class="pull-right" style="position:absolute; bottom: 0; right: 1rem;">
                <div id="dvApplicationName">
                    <b>OMES Cost Report and Rate Setting</b>
                </div>
                <div id="dvEmail">
                    <b>Email: MES@MEDICAID.GOV</b>
                </div>
            </div>
        </div>
        <div class="d-flex">
            <div id="cssmenu" class="d-flex justify-content-between flex-grow-1">
                <ul>
                    <li class='has-sub'>
                        <a href="https://ohiofi.mslc.com/Projects/LogOut">Log Out</a>
                    </li>
                </ul>
            </div>
            <div style="border-bottom: 2px solid #74c141;" class="d-flex align-items-center pl-1">
                <a data-toggle="modal" href="#window_HelpNumber" data-backdrop="static" class="pr-1"><img style="border-radius: 8px;" src="https://ohiofi.mslc.com/Images/Messaging.png" alt="Need Help?" title="Need Help?" width="40" height="40" /></a>
            </div>
        </div>
    </div>

    <div id="contentPanel" style="width: 1280px; margin-left: auto; margin-right: auto;">
        <div class="container">
            <div class="clearFixedFloatingDiv"></div>
        </div>

        
        


<div id="Projects" class="View" style="max-width: 500px; margin: auto;">
    <fieldset>
        <table class="table table-bordered table-hover">
            <thead>
                <tr>
                    <th><legend><b>Select a Project</b></legend></th>
                </tr>
            </thead>
                <tbody>
                    <tr>
                        <td>
                            <a href="https://ohiofi.mslc.com/Projects/SetProject?cID=TnVyc2luZyBGYWNpbGl0eSAoTkYpTVNMQ1NhbHRBbmRQZXBwZXI%3D">Nursing Facility (NF)</a>
                        </td>
                    </tr>
                </tbody>
        </table>
    </fieldset>
</div>
    </div>
    <div class="footer_new" style="color:white">
        <div class="d-flex flex-row justify-content-between container footer_text" style="max-width:1280px; width:1280px;">
            <div style="float:left; width:60%; color:white;">
Legal Notice <br />
This system is for authorized users only, and its use may be monitored. Unauthorized or improper use may result in disciplinary action, civil/criminal penalties, and sanctions. By using this system, you consent to the terms and conditions of use. <br />

            </div>
            <div style="float:left; width:40%;">
                
                <div style="padding-left:175px;color:white;">
                    Copyright © 2013–2024 Myers and Stauffer LC. All rights reserved.<br />

                </div>
                <div style="padding-right: 15px; text-align:right; color:white;">
                    Version: 0.5.0.2066
                </div>
            </div>
        </div>
    </div>
    
    <div id="SyncFusionScript">
        <script src="https://ohiofi.mslc.com/Scripts/ej/common/ej.unobtrusive.min.js"></script>

    </div>
    <noscript>
        <div>
            <h2>This site requires JavaScript</h2>
        </div>
    </noscript>




</body>
</html>

