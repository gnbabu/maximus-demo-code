<%@ page language="C#" autoeventwireup="true" inherits="Process_CallTracker, App_Web_rnw0hezi" title="Call Tracker" masterpagefile="~/NonModal.master" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Src="~/UserControls/CallAddView.ascx" TagName="CallAddView" TagPrefix="viewAddCall" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
   Call Tracker
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <viewAddCall:CallAddView ID="ucCallAddView" runat="server" />
</asp:Content>