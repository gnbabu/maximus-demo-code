<%@ page language="C#" autoeventwireup="true" masterpagefile="~/MasterPage.master" inherits="Process_RedirectToCMCDashboardLeftMenu, App_Web_sdbcnqyo" enableEventValidation="false" stylesheettheme="Default" %>

<%@ register tagprefix="jk" namespace="JK.BootstrapControls" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="text-align: left !important"></div>
</asp:Content>
<asp:Content ID="cntCMCdashboardRedirect" ContentPlaceHolderID="MainContent" runat="Server">
    
<style>
   #cmccenter { position: absolute; top: 50%; width: 100%; height: 1px; overflow: visible }
   #cmcmain { position: absolute; left: 40%; width: 720px; margin-left: -360px; height: 540px; top: -270px;font-size:large;font-weight:bold; } 
</style>

  <div id="cmccenter">
   <div id="cmcmain">
      <p>You are being redirected to CMC Quality Metrics Dashboard...</p>
       <div>
          <asp:Label ID="lblErrorMessage" CssClass="failureNotification" runat="server" Visible="false" Text="Report/Dashboard service is not available, please try again later."></asp:Label>
       </div>
   </div>
</div>
</asp:Content>





