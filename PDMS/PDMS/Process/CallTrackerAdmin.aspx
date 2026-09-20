<%@ Page Language="C#" AutoEventWireup="true" Inherits="Process_CallTrackerAdmin" MasterPageFile="~/MasterPage.master" Title="Call Tracker Reporting and Adminstration" Codebehind="CallTrackerAdmin.aspx.cs" %>
<%@ Register Src="~/UserControls/CallReportingView.ascx" TagName="CallReportingView" TagPrefix="viewRpt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    Call Tracker Administration
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
                <div class="WhiteBox" >

    <viewRpt:CallReportingView ID="ucCallReportingView" runat="server" />
                    </div>
</asp:Content>
