<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ProviderFeed" Codebehind="ProviderFeed.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="ms" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="mbox" %>
<%@ Register Src="~/PopupControls/AddProviderFeed.ascx" TagName="AddProviderFeed" TagPrefix="uc7" %>
<%@ Register Src="~/PopupControls/WorkflowEventInfo.ascx" TagName="workflowevent" TagPrefix="uc8" %>
<%@ Register Src="~/PopupControls/WorkflowEventHistoryInfo.ascx" TagName="workfloweventHist" TagPrefix="uc9" %>

<div id="divProviderFeed" runat="server">
    <uc8:workflowevent id="ucAddProviderFeed" runat="server" mode="Grid" />
    <br />
    <uc9:workfloweventhist id="Workflowevent1" runat="server" mode="Grid" />
</div>
