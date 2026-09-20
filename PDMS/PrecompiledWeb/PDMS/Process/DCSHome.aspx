<%@ page language="C#" autoeventwireup="true" inherits="Process_DCSHome, App_Web_rnw0hezi" masterpagefile="~/MasterPage.master" title="DCS Home" enableEventValidation="false" stylesheettheme="Default" %>
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