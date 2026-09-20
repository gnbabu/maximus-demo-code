<%@ Page Title="" Language="C#" MasterPageFile="~/MasterWorkflowPage.master" AutoEventWireup="true" Inherits="Process_AddAffiliations" Codebehind="AddAffiliations.aspx.cs" %>
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

