<%@ Page Language="C#" AutoEventWireup="true" Inherits="Process_DCSHome"  MasterPageFile="~/MasterPage.master" Title="DCS Home" Codebehind="DCSHome.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    DCS Home Page
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="boxPanelFull2">
        <div class="boxPanelHeader">My Dashboard</div>
        <div class="boxPanelData">
                <asp:HyperLink ID="lnkAdmin" runat="server" Text="DCS Administration" Target="_self" NavigateUrl="~/Process/AdminDCS.aspx" ToolTip="DCS Administration"></asp:HyperLink>
        </div>
    </div>

</asp:Content>