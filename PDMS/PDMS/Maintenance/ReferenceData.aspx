<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="Maintenance_ReferenceData" MaintainScrollPositionOnPostback="true" Codebehind="ReferenceData.aspx.cs" %>

<%@ Register Src="../UserControls/ReferenceData/ApplicationFeePaymentType.ascx" TagName="ApplicationFeePaymentType" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/ApplicationType.ascx" TagName="ApplicationType" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/PaperRequestDocumentType.ascx" TagName="PaperRequestDocumentType" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/ProviderCategoryType.ascx" TagName="ProviderCategoryType" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/ProviderType.ascx" TagName="ProviderType" TagPrefix="uc" %>
<%--<%@ register src="../UserControls/ReferenceData/PAAssignProcedureGrp.ascx" tagname="PAProcedureGrp" tagprefix="uc" %>--%>
<%@ Register Src="../UserControls/ReferenceData/ProviderTypeFee.ascx" TagName="ProviderTypeFee" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/RegPageSetting.ascx" TagName="RegPageSetting" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/RegPageSettingAction.ascx" TagName="RegPageSettingAction" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/RegPageType.ascx" TagName="RegPageType" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/RegSectionUploadControl.ascx" TagName="RegSectionUploadControl" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/SpecialtyType.ascx" TagName="SpecialtyType" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/TaxonomyType.ascx" TagName="TaxonomyType" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/AppSettings.ascx" TagName="AppSettings" TagPrefix="ucAppS" %>
<%@ Register Src="../UserControls/ReferenceData/DelegatesUsers.ascx" TagName="Delegates" TagPrefix="ucDel" %>
<%@ Register Src="../UserControls/ReferenceData/DataFixTables.ascx" TagName="DataFixTables" TagPrefix="ucDFT" %>
<%@ Register Src="../UserControls/ReferenceData/WebAPITesting.ascx" TagName="WebAPITesting" TagPrefix="ucWAT" %>
<%@ Register Src="../UserControls/ReferenceData/RDMCodeSets.ascx" TagName="RDMCodeSets" TagPrefix="uc" %>
<%--<%@ register src="../UserControls/ReferenceData/WPCCodeSets.ascx" tagname="WPCCodeSets" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/C4CodeSets.ascx" tagname="C4CodeSets" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/CMSCodeSets.ascx" tagname="CMSCodeSets" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/NUBCCodeSets.ascx" tagname="NUBCCodeSets" tagprefix="uc" %>--%>
<%@ Register Src="../UserControls/ReferenceData/AutomatedReports.ascx" TagName="AutomatedReports" TagPrefix="ucAR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .page-main-header {
            font-family: 'Merriweather', serif !important;
            font-size: 28px !important;
        }

        .form-control {
            margin-top: 10px;
            font-size: 17px;
            height: 44px;
            color: #000;
        }
    </style>
    <div class="WhiteBox">

        <asp:Panel ID="pnlUserSelect" runat="server">
            <div class="row">
                <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                    <span class="ohio-field">Configuration Management:</span>
                    <asp:DropDownList ID="ddlReferenceDataSelect" runat="server" CssClass="form-control" AutoPostBack="true" aria-label="ReferenceDataSelect" AppendDataBoundItems="True"
                        OnSelectedIndexChanged="ddlReferenceDataSelect_SelectedIndexChanged" />
                </div>
            </div>
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
                <ucdel:delegates id="DelegatesID" runat="server" />
            </asp:View>
            <asp:View ID="vwAppSettings" runat="server">
                <ucapps:appsettings id="AppSettingsID" runat="server" />
            </asp:View>
            <asp:View ID="vwDataFixTables" runat="server">
                <ucdft:datafixtables id="DataFixTablesID" runat="server" />
            </asp:View>
            <asp:View ID="vwWebAPITesting" runat="server">
                <ucwat:webapitesting id="WebAPITestingID" runat="server" />
            </asp:View>
            <asp:View ID="vwRDMCodeSets" runat="server">
                <uc:rdmcodesets id="ucRDMCodeSets" runat="server" />
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
                <ucar:automatedreports id="ucARAutomatedReports" runat="server" />
            </asp:View>
        </asp:MultiView>

    </div>
</asp:Content>
