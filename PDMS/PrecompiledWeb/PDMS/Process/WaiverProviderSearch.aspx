<%@ page language="C#" autoeventwireup="true" masterpagefile="~/MasterPage.master" inherits="Process_DIDDProviderSearch, App_Web_unbhbgmw" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/DIDDProviderSearch.ascx" TagName="DIDDProviderSearch" TagPrefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    Services Provider Search
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <style type="text/css">

.RightBox
{
    width: 860px;
}
.UserHeader
{
    
    width: 1120px;
    
}
</style>
    <center><span   class="bodyTextRed" style="text-align:center">To Avoid Duplicates, Search for Existing Referral Before Proceeding</span></center>
    <uc1:DIDDProviderSearch ID="ucDIDDProviderSearch" runat="server" />
</asp:Content>