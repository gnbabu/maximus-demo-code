<%@ Page Language="C#" AutoEventWireup="true" Inherits="Process_CallTracker"  Title="Call Tracker" MasterPageFile="~/NonModal.master"  Codebehind="CallTracker.aspx.cs" %>
<%@ Register Src="~/UserControls/CallAddView.ascx" TagName="CallAddView" TagPrefix="viewAddCall" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
   Call Tracker
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <viewAddCall:CallAddView ID="ucCallAddView" runat="server" />
</asp:Content>