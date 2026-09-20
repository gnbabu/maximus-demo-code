<%@ control language="C#" autoeventwireup="true" inherits="Pages_SiteVisit, App_Web_2k5drnu4" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Src="~/PopupControls/SiteVisits.ascx" TagName="SiteVisits" TagPrefix="uc" %>

<%@ Register Src="~/PopupControls/SiteVisitAttempt.ascx" TagName="AttemptDetails" TagPrefix="uc" %>


<br />
<uc:SiteVisits runat="server" ID="ucSiteVisits" OnSiteVisitSelected="ucSiteVisits_SiteVisitSelected" OnSiteVisitUpdated="ucSiteVisits_SiteVisitUpdated"/>
<br />

<br />
<uc:AttemptDetails runat="server" ID="ucAttemptDetails" OnSiteVisitAttemptCancel="ucAttemptDetails_SiteVisitAttemptCancel" OnSiteVisitAttemptUpdated="ucAttemptDetails_SiteVisitAttemptUpdated"  />
