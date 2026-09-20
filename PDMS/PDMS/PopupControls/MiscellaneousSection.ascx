<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_MiscellaneousSection" Codebehind="MiscellaneousSection.ascx.cs" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/Medicare.ascx" TagName="Medicare" TagPrefix="uc5" %>
<%@ Register Src="~/PopupControls/Medicaid.ascx" TagName="Medicaid" TagPrefix="uc6" %>
<div id="divMiscellaneous">
    <span class="pageHeader">Medicare Number</span>
    <uc1:Separator runat="server" ID="test" />
    <uc5:Medicare ID="ucMedicare" runat="server" />
    <br />
    <span class="pageHeader">Medicaid</span>
    <br />
    <uc6:Medicaid ID="ucMedicaid" runat="server" />
</div>
