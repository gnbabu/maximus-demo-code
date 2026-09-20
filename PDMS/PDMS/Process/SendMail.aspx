<%@ Page Title="Send Email" Language="C#" MasterPageFile="~/MasterPage.master" ValidateRequest="false" AutoEventWireup="true" Inherits="Process.Process_SendMail" Codebehind="SendMail.aspx.cs" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/EmailTemplateMaintenance.ascx" TagName="EmailTemplateMaintenance" TagPrefix="EmailTemplateMaintenance" %>
<%@ Register Src="~/UserControls/EmailQueueListing.ascx" TagPrefix="EmailQueue" TagName="EmailQueueListing" %>
<%@ Register Src="~/UserControls/EmailHistory.ascx" TagPrefix="History" TagName="EmailHistory" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageLabelContent" runat="Server">
    Email Management
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        /*.modal {
            position: fixed;
            top: 0;
            left: 0;
            background-color: black;
            z-index: 99;
            opacity: 0.8;
            filter: alpha(opacity=80);
            -moz-opacity: 0.8;
            min-height: 100%;
            width: 100%;
        }*/

        #DocumentsAndReportsLoading {
            display: none;
            font-family: Arial;
            font-size: 10pt;
            height: 100px;
            position: fixed;
            width: 200px;
            z-index: 999;
        }

        .failureNotification {
            color: Red !important;
            text-align: left;
        }

        .buttonLinkBox {
            -moz-border-radius: 6px;
            /* Old button background-color was #3d94f6 replaced with #07457E */
            -moz-box-shadow: inset 0px 1px 0px 0px #97c4fe;
            -webkit-border-radius: 6px;
            -webkit-box-shadow: inset 0px 1px 0px 0px #97c4fe;
            /*background: -webkit-gradient( linear, left top, left bottom, color-stop(0.05, #07457E), color-stop(1, #1e62d0) );*/
            background: -moz-linear-gradient(center top, #07457E 5%, #1e62d0 100%);
            background-color: #07457E;
            border: 1px solid #337fed;
            border-radius: 6px;
            box-shadow: inset 0px 1px 0px 0px #97c4fe;
            color: #ffffff;
            display: inline-block;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#07457E', endColorstr='#1e62d0');
            font-family: arial;
            font-size: 13px;
            font-weight: bold;
            padding: 1px 6px;
            text-decoration: none;
            text-shadow: 1px 1px 0px #1570cd;
        }

        #test a {
            color: #ffffff !important;
            text-decoration: none;
        }

        #test a:active {
            color: #ffffff !important;
            text-decoration: none;
        }

        #test a:visited {
            color: #ffffff !important;
            text-decoration: none;
        }

        #test a:hover {
            color: #ffffff !important;
            text-decoration: none;
        }

        .radTabStrip { border: none; }

        .radTabStrip .tab {
            background-color: #ebebe0;
            border-right: 1px solid #666;
            color: #000;
                
            text-align: center;
            vertical-align: middle;
            width: 15%;
        }

        .radTabStrip .tab.overviewTab { border: none; }

        .radTabStrip .tab.selectedTab {
            background-color: #259dd9;
            border-color: #666;
            color: #000;
        }



        .radTabStrip .tab.hoveredTab {
            background-color: #ccccb3;
            border-color: #666;
            color: #000;
        }

        .radTabStrip .tab.hoveredTab.selectedTab {
            background-color: #259dd9;
            border-color: #666;
            color: #000;
        }

        .radTabStrip .tab .rtsLink { border: 1px solid #ccc; }

        .sectionArea { background-color: #99ddff; }

        .title-banner {
            background-color: #923931;
            color: #fff;
            padding: 3px;
            text-align: center;
        }

        .title-banner-content {
            margin-bottom: 15px;
            margin-left: 20px;
            margin-top: 15px;
        }

        .label-center { text-align: center; }

        .tRequired { color: red; }

        table { width: 100%; }

        table.grid {
            border-collapse: separate;
            border-spacing: 0px;
            font-size: 12pt;
        }

        table.grid tr {
            font-size: 12pt;
            text-align: left;
        }

        table.grid td { font-size: 12pt; }

        .formLabelEM, .formLabel300EM, .formLabel150EM, .formLabel170EM, .formLabel200EM {
            display: inline-block;
            font-weight: bold;
            padding-right: 3px;
            text-align: left;
            vertical-align: top;
            width: 130px;
        }

        .formLabel150EM { width: 150px; }

        .formLabel170EM { width: 170px; }

        .formLabel200EM { width: 215px; }

        .formLabel300EM { width: 300px; }



    </style>
   
    <cc1:GroupBox ID="gbSendMail" Caption="Send Email" CaptionStyle-CssClass="bodyTextBold" HorizontalAlign="Center" Width="100%" runat="server">

    <telerik:RadTabStrip RenderMode="Lightweight" runat="server" ID="RadTabStrip1" AutoPostBack="true" OnTabClick="RadTabStrip1_TabClick" CssClass="radTabStrip" MultiPageID="RadMultiPage1" SelectedIndex="0" EnableEmbeddedSkins="false" Skin="">
        <Tabs>
            <telerik:RadTab Text="Setup Bulk Job"
                CssClass="tab overviewTab"
                SelectedCssClass="selectedTab"
                HoveredCssClass="hoveredTab">
            </telerik:RadTab>
            <telerik:RadTab Text="Email Queue"
                CssClass="tab overviewTab"
                SelectedCssClass="selectedTab"
                HoveredCssClass="hoveredTab">
            </telerik:RadTab>
            <telerik:RadTab Text="Email Job History"
                CssClass="tab overviewTab"
                SelectedCssClass="selectedTab"
                HoveredCssClass="hoveredTab">
            </telerik:RadTab>
        </Tabs>
    </telerik:RadTabStrip>

    <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0" CssClass="multiPage">


        <telerik:RadPageView runat="server" ID="RadPageView1">
            <EmailTemplateMaintenance:EmailTemplateMaintenance runat="server" id="EmailTemplateMaintenance" />
        </telerik:RadPageView>

        <telerik:RadPageView runat="server" ID="RadPageView2">
            <EmailQueue:EmailQueueListing runat="server" ID="EmailQueueListing" />
        </telerik:RadPageView>

        <telerik:RadPageView runat="server" ID="RadPageView3">
            <History:EmailHistory runat="server" ID="HistoryControl" />
        </telerik:RadPageView>
    </telerik:RadMultiPage>
        </cc1:GroupBox>
</asp:Content>

