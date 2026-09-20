<%@ page language="C#" autoeventwireup="true" inherits="Process_CallTrackerAdmin, App_Web_rnw0hezi" masterpagefile="~/MasterPage.master" title="Call Tracker Reporting and Adminstration" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Src="~/UserControls/CallReportingView.ascx" TagName="CallReportingView" TagPrefix="viewRpt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    Call Tracker Administration
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
                <div class="WhiteBox" >

    <viewRpt:CallReportingView ID="ucCallReportingView" runat="server" />
                    </div>
</asp:Content>
