<%@ Control Language="C#" AutoEventWireup="true" Inherits="UIControls_MenuTop" Codebehind="MenuTop.ascx.cs" %>

<asp:Menu ID="mnuTop" runat="server" Orientation="Horizontal" CssClass="mainMenu" 
    DataSourceID="SiteMapDataSource1" StaticEnableDefaultPopOutImage="false" 
    DynamicEnableDefaultPopOutImage="false" onprerender="mnuTop_PreRender">
    <DynamicHoverStyle CssClass="subMenuItemHover" />
    <DynamicMenuStyle CssClass="subMenuItem" />
    <DynamicMenuItemStyle CssClass="subMenuItem" />
</asp:Menu>

<asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" ShowStartingNode="false" 
    SiteMapProvider="XmlSiteMapProviderData" />