<%@ page title="" language="C#" masterpagefile="~/MasterWorkflowPage.master" autoeventwireup="true" inherits="Process_AddAffiliations, App_Web_unbhbgmw" enableEventValidation="false" stylesheettheme="Default" %>
<%@ MasterType TypeName="MasterWorkflowPage" %>
<%@ Register Src="~/Pages/GroupAffiliations.ascx" TagPrefix="uc" TagName="GroupAffiliations" %>


<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    Add Affiliations
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    
    <uc:GroupAffiliations ID="ucGroupAffiliations" runat="server" DisableOnSave="true" />
    <div>
        <asp:Label ID="lblRegistrationId" runat="server" />
    </div>
    <!--<asp:HiddenField id="dirty" value="" runat="server" EnableViewState="true"/>-->
</asp:Content>

