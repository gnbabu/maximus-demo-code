<%@ page language="C#" autoeventwireup="true" masterpagefile="~/MasterPage.master" inherits="Maintenance_ReferenceData, App_Web_nqybrob4" maintainscrollpositiononpostback="true" enableEventValidation="false" stylesheettheme="Default" %>

<%@ register src="../UserControls/ReferenceData/ApplicationFeePaymentType.ascx" tagname="ApplicationFeePaymentType" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/ApplicationType.ascx" tagname="ApplicationType" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/PaperRequestDocumentType.ascx" tagname="PaperRequestDocumentType" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/ProviderCategoryType.ascx" tagname="ProviderCategoryType" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/ProviderType.ascx" tagname="ProviderType" tagprefix="uc" %>
<%--<%@ register src="../UserControls/ReferenceData/PAAssignProcedureGrp.ascx" tagname="PAProcedureGrp" tagprefix="uc" %>--%>
<%@ register src="../UserControls/ReferenceData/ProviderTypeFee.ascx" tagname="ProviderTypeFee" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/RegPageSetting.ascx" tagname="RegPageSetting" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/RegPageSettingAction.ascx" tagname="RegPageSettingAction" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/RegPageType.ascx" tagname="RegPageType" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/RegSectionUploadControl.ascx" tagname="RegSectionUploadControl" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/SpecialtyType.ascx" tagname="SpecialtyType" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/TaxonomyType.ascx" tagname="TaxonomyType" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/AppSettings.ascx" tagname="AppSettings" tagprefix="ucAppS" %>
<%@ register src="../UserControls/ReferenceData/DelegatesUsers.ascx" tagname="Delegates" tagprefix="ucDel" %>
<%@ register src="../UserControls/ReferenceData/DataFixTables.ascx" tagname="DataFixTables" tagprefix="ucDFT" %>
<%@ register src="../UserControls/ReferenceData/WebAPITesting.ascx" tagname="WebAPITesting" tagprefix="ucWAT" %>
<%@ register src="../UserControls/ReferenceData/RDMCodeSets.ascx" tagname="RDMCodeSets" tagprefix="uc" %>
<%--<%@ register src="../UserControls/ReferenceData/WPCCodeSets.ascx" tagname="WPCCodeSets" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/C4CodeSets.ascx" tagname="C4CodeSets" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/CMSCodeSets.ascx" tagname="CMSCodeSets" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/NUBCCodeSets.ascx" tagname="NUBCCodeSets" tagprefix="uc" %>--%>
<%@ register src="../UserControls/ReferenceData/AutomatedReports.ascx" tagname="AutomatedReports" tagprefix="ucAR" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="WhiteBox">

        <asp:Panel ID="pnlUserSelect" runat="server">
            <span class="formLabelAuto">Reference Data:</span>
            <asp:DropDownList ID="ddlReferenceDataSelect" runat="server" CssClass="formDropDown" AutoPostBack="true" aria-label="ReferenceDataSelect" AppendDataBoundItems="True"
                onselectedindexchanged="ddlReferenceDataSelect_SelectedIndexChanged" />
            <br />
            <br />
        </asp:Panel>
        <asp:MultiView ID="mltEnrollment" runat="server" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vwTaxonomyType" runat="server">
                <uc:taxonomytype id="ucTaxonomyType" runat="server" />
            </asp:View>
            <asp:View ID="vwApplicationType" runat="server">
                <uc:applicationtype id="ApplicationType" runat="server" />
            </asp:View>
            <asp:View ID="vwPaperRequestDocumentType" runat="server">
                <uc:paperrequestdocumenttype id="PaperRequestDocumentType" runat="server" />
            </asp:View>
            <asp:View ID="vwProviderCategoryType" runat="server">
                <uc:providercategorytype id="ProviderCategoryType" runat="server" />
            </asp:View>
            <asp:View ID="vwProviderType" runat="server">
                <uc:providertype id="ProviderType" runat="server" />
            </asp:View>
<%--            <asp:View ID="vwPAProcedureGrp" runat="server">
                <uc:PAProcedureGrp id="PAProcedureGrp" runat="server" />
            </asp:View>--%>
            <asp:View ID="vwProviderTypeFee" runat="server">
                <uc:providertypefee id="ProviderTypeFee" runat="server" />
            </asp:View>
            <asp:View ID="vwRegPageSetting" runat="server">
                <uc:regpagesetting id="RegPageSetting" runat="server" />
            </asp:View>
            <asp:View ID="vwRegPageSettingAction" runat="server">
                <uc:regpagesettingaction id="RegPageSettingAction" runat="server" />
            </asp:View>
            <asp:View ID="vwRegPageType" runat="server">
                <uc:regpagetype id="RegPageType" runat="server" />
            </asp:View>
            <asp:View ID="vwRegSectionUploadControl" runat="server">
                <uc:regsectionuploadcontrol id="RegSectionUploadControl" runat="server" />
            </asp:View>
            <asp:View ID="vwSpecialtyType" runat="server">
                <uc:specialtytype id="SpecialtyType" runat="server" />
            </asp:View>
            <asp:View ID="vwDelegates" runat="server">
                <ucDel:delegates id="DelegatesID" runat="server" />
            </asp:View>
            <asp:View ID="vwAppSettings" runat="server">
                <ucAppS:appsettings id="AppSettingsID" runat="server" />
            </asp:View>
            <asp:View ID="vwDataFixTables" runat="server">
                <ucDFT:DataFixTables id="DataFixTablesID" runat="server" />
            </asp:View>
            <asp:View ID="vwWebAPITesting" runat="server">
                <ucWAT:WebAPITesting id="WebAPITestingID" runat="server" />
            </asp:View>
            <asp:View ID="vwRDMCodeSets" runat="server">
                <uc:RDMCodeSets id="ucRDMCodeSets" runat="server" />
            </asp:View>
<%--            <asp:View ID="vwWPCCodeSets" runat="server">
                <uc:WPCCodeSets id="ucWPCCodeSets" runat="server" />
            </asp:View>
            <asp:View ID="vwC4CodeSets" runat="server">
                <uc:C4CodeSets id="ucC4CodeSets" runat="server" />
            </asp:View>
            <asp:View ID="vwCMSCodeSets" runat="server">
                <uc:CMSCodeSets id="ucCMSCodeSets" runat="server" />
            </asp:View>
            <asp:View ID="vwNUBCCodeSets" runat="server">
                <uc:NUBCCodeSets id="ucNUBCCodeSets" runat="server" />
            </asp:View>--%>
            <asp:View ID="vwAutomatedReports" runat="server">
                <ucAR:AutomatedReports id="ucARAutomatedReports" runat="server" />
            </asp:View>
        </asp:MultiView>

    </div>
</asp:Content>
