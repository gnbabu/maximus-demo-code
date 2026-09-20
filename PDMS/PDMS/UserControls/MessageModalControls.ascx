<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_MessageModalControls" Codebehind="MessageModalControls.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%--<%@ Register Src="~/PopupControls/EventHistory.ascx" TagPrefix="ucEH" TagName="EventHistory" %>--%>

<%@ Register Src="~/PopupControls/Certifications.ascx" TagPrefix="uc" TagName="Certifications" %>
<%@ Register Src="~/PopupControls/CertificationsHistory.ascx" TagPrefix="uc" TagName="CertificationsHistory" %>

<%@ Register Src="~/PopupControls/CertSecondGrid.ascx" TagPrefix="uc" TagName="CertSecondGrid" %>
<%@ Register Src="~/PopupControls/CertSecondGridHistory.ascx" TagPrefix="uc" TagName="CertSecondGridHistory" %>

<%@ Register Src="~/PopupControls/Licenses.ascx" TagPrefix="uc" TagName="Licenses" %>
<%@ Register Src="~/PopupControls/LicensesHistory.ascx" TagPrefix="uc" TagName="LicensesHistory" %>

<%@ Register Src="~/PopupControls/Pharmacy.ascx" TagPrefix="uc" TagName="Pharmacy" %>
<%@ Register Src="~/PopupControls/PharmacyHistory.ascx" TagPrefix="uc" TagName="PharmacyHistory" %>

<%@ Register Src="~/PopupControls/Medicare.ascx" TagPrefix="uc" TagName="Medicare" %>
<%@ Register Src="~/PopupControls/MedicareHistory.ascx" TagPrefix="uc" TagName="MedicareHistory" %>

<%@ Register Src="~/PopupControls/PrimarySpecialty.ascx" TagPrefix="uc" TagName="PrimarySpecialty" %>
<%@ Register Src="~/PopupControls/PrimarySpecialtyHistory.ascx" TagPrefix="uc" TagName="PrimarySpecialtyHistory" %>

<%@ Register Src="~/PopupControls/AdditionalSpecialties.ascx" TagPrefix="uc" TagName="AdditionalSpecialties" %>
<%@ Register Src="~/PopupControls/AdditionalSpecialtiesHistory.ascx" TagPrefix="uc" TagName="AdditionalSpecialtiesHistory" %>

<%@ Register Src="~/PopupControls/TaxonomyCode.ascx" TagPrefix="uc" TagName="TaxonomyCode" %>
<%@ Register Src="~/PopupControls/TaxonomyCodeHistory.ascx" TagPrefix="uc" TagName="TaxonomyCodeHistory" %>

<%@ Register Src="~/PopupControls/AdditionalTaxonomyCode.ascx" TagPrefix="uc" TagName="AdditionalTaxonomyCode" %>
<%@ Register Src="~/PopupControls/AdditionalTaxonomyCodeHistory.ascx" TagPrefix="uc" TagName="AdditionalTaxonomyCodeHistory" %>

<%--<%@ Register Src="~/PopupControls/PrimaryPracticeLocation.ascx" TagPrefix="uc" TagName="PrimaryPracticeLocation" %>--%>
<%--<%@ Register Src="~/PopupControls/PrimaryPracticeLocationHistory.ascx" TagPrefix="uc" TagName="PrimaryPracticeLocationHistory" %>--%>

<%--<%@ Register Src="~/PopupControls/BillingPaymentContactInfo.ascx" TagPrefix="uc" TagName="BillingPaymentContactInfo" %>--%>
<%--<%@ Register Src="~/PopupControls/BillingPaymentContactInfoHistory.ascx" TagPrefix="uc" TagName="BillingPaymentContactInfoHistory" %>--%>

<%--<%@ Register Src="~/PopupControls/CorrespondenceInformation.ascx" TagPrefix="uc" TagName="CorrespondenceInformation" %>--%>
<%--<%@ Register Src="~/PopupControls/CorrespondenceInformationHistory.ascx" TagPrefix="uc" TagName="CorrespondenceInformationHistory" %>--%>

<%--<%@ Register Src="~/PopupControls/SatellitePracticeLocations.ascx" TagPrefix="uc" TagName="SatellitePracticeLocations" %>--%>
<%--<%@ Register Src="~/PopupControls/SatellitePracticeLocationsHistory.ascx" TagPrefix="uc" TagName="SatellitePracticeLocationsHistory" %>--%>

<%--<%@ Register Src="~/PopupControls/OrgInfo.ascx" TagPrefix="uc" TagName="OrgInfo" %>--%>
<%--<%@ Register Src="~/PopupControls/OrgInfoHistory.ascx" TagPrefix="uc" TagName="OrgInfoHistory" %>--%>

<%--<%@ Register Src="~/PopupControls/PrimaryContactInfo.ascx" TagPrefix="uc" TagName="PrimaryContactInfo" %>--%>
<%--<%@ Register Src="~/PopupControls/PrimaryContactInfoHistory.ascx" TagPrefix="uc" TagName="PrimaryContactInfoHistory" %>--%>

<%--<%@ Register Src="~/PopupControls/BankingInformation.ascx" TagPrefix="uc" TagName="BankingInformation" %>--%>
<%--<%@ Register Src="~/PopupControls/BankingInformationHistory.ascx" TagPrefix="uc" TagName="BankingInformationHistory" %>--%>

<%--<%@ Register Src="~/PopupControls/GroupAffiliations.ascx" TagPrefix="uc" TagName="GroupAffiliations" %>--%>

<%--<%@ Register Src="~/PopupControls/VendorNumber.ascx" TagPrefix="uc" TagName="VendorNumber" %>--%>

<style type="text/css">
    .modalBackground
    {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.2;
    }
    .modalPopup
    {
        background-color: #FFFFFF;
        border-width: 1px;
        border-style: solid;
        border-color: black;
        padding: 0px;
        width: 50%;
    }
        
        
</style>

<!-- ModalPopupExtender -->
<cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
    CancelControlID="btnCancel" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlModal">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" align="center" style="display:none">
    <asp:Panel ID="pnlHeader" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
        <div style="text-align: left;">&nbsp;&nbsp;
        <asp:Label ID="lblTitle" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
        </div>
    </asp:Panel>  
    <asp:Panel ID="pnlLabel" runat="server">
        <asp:Panel runat="server" ID="pnlDefault">
            <div style="text-align: left;padding: 15px;">
                <asp:Label id="lblMessage" runat="server" />
            </div>
        </asp:Panel>
        <%--<asp:Panel runat="server" ID="pnlEventHistory" Visible="false" style="margin:20px">
            <ucEH:EventHistory ID="ucEventHistory" runat="server" />
        </asp:Panel>--%>
        <asp:Panel runat="server" ID="pnlCertifications" Visible="false" style="margin:20px">
            <uc:Certifications ID="Certifications" runat="server" />
        </asp:Panel>
        <%--<asp:Panel runat="server" ID="pnlCertificationsHistory" Visible="false" style="margin:20px">
            <uc:CertificationsHistory ID="CertificationsHistory" runat="server" />
        </asp:Panel>--%>
        <asp:Panel runat="server" ID="pnlCertSecondGrid" Visible="false" style="margin:20px">
            <uc:CertSecondGrid ID="CertSecondGrid" runat="server" />
        </asp:Panel>
       <%-- <asp:Panel runat="server" ID="pnlCertSecondGridHistory" Visible="false" style="margin:20px">
            <uc:CertSecondGridHistory ID="CertSecondGridHistory" runat="server" />
        </asp:Panel>--%>
        <asp:Panel runat="server" ID="pnlLicenses" Visible="false" style="margin:20px">
            <uc:Licenses ID="Licenses" runat="server" />
        </asp:Panel>
       <%-- <asp:Panel runat="server" ID="pnlLicensesHistory" Visible="false" style="margin:20px">
            <uc:LicensesHistory ID="LicensesHistory" runat="server" />
        </asp:Panel>--%>
        <asp:Panel runat="server" ID="pnlPharmacy" Visible="false" style="margin:20px">
            <uc:Pharmacy ID="Pharmacy" runat="server" />
        </asp:Panel>
        <%--<asp:Panel runat="server" ID="pnlPharmacyHistory" Visible="false" style="margin:20px">
            <uc:PharmacyHistory ID="PharmacyHistory" runat="server" />
        </asp:Panel>--%>
        <asp:Panel runat="server" ID="pnlMedicare" Visible="false" style="margin:20px">
            <uc:Medicare ID="Medicare" runat="server" />
        </asp:Panel>
        <%--<asp:Panel runat="server" ID="pnlMedicareHistory" Visible="false" style="margin:20px">
            <uc:MedicareHistory ID="MedicareHistory" runat="server" />
        </asp:Panel>--%>
        <asp:Panel runat="server" ID="pnlPrimarySpecialty" Visible="false" style="margin:20px" >
            <uc:PrimarySpecialty ID="ucPrimarySpecialty" runat="server" />
        </asp:Panel>
        <%--<asp:Panel runat="server" ID="pnlPrimarySpecialtyHistory" Visible="false" style="margin:20px">
            <uc:PrimarySpecialtyHistory ID="PrimarySpecialtyHistory" runat="server" />
        </asp:Panel>--%>
        <asp:Panel runat="server" ID="pnlAdditionalSpecialties" Visible="false" style="margin:20px">
            <uc:AdditionalSpecialties ID="AdditionalSpecialties" runat="server" />
        </asp:Panel>
        <%--<asp:Panel runat="server" ID="pnlAdditionalSpecialtiesHistory" Visible="false" style="margin:20px">
            <uc:AdditionalSpecialtiesHistory ID="AdditionalSpecialtiesHistory" runat="server" />
        </asp:Panel>--%>
        <asp:Panel runat="server" ID="pnlTaxonomyCode" Visible="false" style="margin:20px">
            <uc:TaxonomyCode ID="TaxonomyCode" runat="server" />
        </asp:Panel>
        <%--<asp:Panel runat="server" ID="pnlTaxonomyCodeHistory" Visible="false" style="margin:20px">
            <uc:TaxonomyCodeHistory ID="TaxonomyCodeHistory" runat="server" />
        </asp:Panel>--%>
        <asp:Panel runat="server" ID="pnlAdditionalTaxonomyCode" Visible="false" style="margin:20px">
            <uc:AdditionalTaxonomyCode ID="AdditionalTaxonomyCode" runat="server" />
        </asp:Panel>
        <%--<asp:Panel runat="server" ID="pnlAdditionalTaxonomyCodeHistory" Visible="false" style="margin:20px">
            <uc:AdditionalTaxonomyCodeHistory ID="AdditionalTaxonomyCodeHistory" runat="server" />
        </asp:Panel>--%>
        <%--<asp:Panel runat="server" ID="pnlPrimaryPracticeLocation" Visible="false" style="margin:20px">
            <uc:PrimaryPracticeLocation ID="PrimaryPracticeLocation" runat="server" />
        </asp:Panel>--%>
        <%--<asp:Panel runat="server" ID="pnlPrimaryPracticeLocationHistory" Visible="false" style="margin:20px">
            <uc:PrimaryPracticeLocationHistory ID="PrimaryPracticeLocationHistory" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBillingPaymentContactInfo" Visible="false" style="margin:20px">
            <uc:BillingPaymentContactInfo ID="BillingPaymentContactInfo" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBillingPaymentContactInfoHistory" Visible="false" style="margin:20px">
            <uc:BillingPaymentContactInfoHistory ID="BillingPaymentContactInfoHistory" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlCorrespondenceInformation" Visible="false" style="margin:20px">
            <uc:CorrespondenceInformation ID="CorrespondenceInformation" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlCorrespondenceInformationHistory" Visible="false" style="margin:20px">
            <uc:CorrespondenceInformationHistory ID="CorrespondenceInformationHistory" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlSatellitePracticeLocations" Visible="false" style="margin:20px">
            <uc:SatellitePracticeLocations ID="SatellitePracticeLocations" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlSatellitePracticeLocationsHistory" Visible="false" style="margin:20px">
            <uc:SatellitePracticeLocationsHistory ID="SatellitePracticeLocationsHistory" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlOrgInfo" Visible="false" style="margin:20px">
            <uc:OrgInfo ID="OrgInfo" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlOrgInfoHistory" Visible="false" style="margin:20px">
            <uc:OrgInfoHistory ID="OrgInfoHistory" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlPrimaryContactInfo" Visible="false" style="margin:20px">
            <uc:PrimaryContactInfo ID="PrimaryContactInfo" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlPrimaryContactInfoHistory" Visible="false" style="margin:20px">
            <uc:PrimaryContactInfoHistory ID="PrimaryContactInfoHistory" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlGroupAffiliations" Visible="false" style="margin:20px">
            <uc:GroupAffiliations ID="GroupAffiliations" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlVendorNumber" Visible="false" style="margin:20px">
            <uc:VendorNumber ID="VendorNumber" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBankingInformation" Visible="false" style="margin:20px">
            <uc:BankingInformation ID="BankingInformation" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBankingInformationHistory" Visible="false" style="margin:20px">
            <uc:BankingInformationHistory ID="BankingInformationHistory" runat="server" />
        </asp:Panel>--%>
    </asp:Panel>
    <table border="0" style="padding-bottom: 10px;text-align: center;padding: 0px;border-collapse: separate; border-spacing: 5px;" role="presentation">
        <tr>
            <td>
                <asp:Button id="btnContinue"  runat="server" Text="Continue" CssClass="buttonBox" CausesValidation="false" onclick="btnContinue_Click" />
            </td>
            <td>
                <asp:Button id="btnCancel"  runat="server" Text="Cancel" CssClass="buttonBox" 
                    CausesValidation="false" />
            </td>
        </tr>
    </table> 
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />
