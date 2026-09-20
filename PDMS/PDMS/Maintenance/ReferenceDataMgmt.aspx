<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="Maintenance_ReferenceDataMgmt" Codebehind="ReferenceDataMgmt.aspx.cs" %>

<%@ Register Src="../UserControls/ReferenceData/RDMCodeSets.ascx" TagName="RDMCodeSets" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/WPCCodeSets.ascx" TagName="WPCCodeSets" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/C4CodeSets.ascx" TagName="C4CodeSets" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/CMSCodeSets.ascx" TagName="CMSCodeSets" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/NUBCCodeSets.ascx" TagName="NUBCCodeSets" TagPrefix="uc" %>
<%@ Register Src="../UserControls/ReferenceData/PAAssignProcedureGrp.ascx" TagName="PAProcedureGrp" TagPrefix="uc" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .page-main-header {
            font-family: 'Merriweather', serif !important;
            font-size: 28px !important;
        }

        .WhiteBox {
            font-family: 'Source Sans Pro', sans-serif !important;
        }

        .form-control {
            font-size: 17px;
            height: 44px;
            color: #000;
        }

        .ohio-field-input {
            height: 44px;
            color: #000;
        }

        .row-gap {
            padding-top: 10px !important;
        }
    </style>
    <div class="WhiteBox">

        <asp:Panel ID="pnlUserSelect" runat="server">
            <div class="row">
                <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                    <span class="ohio-field" style="margin-right: 10px;">Configuration Management:</span>
                    <asp:DropDownList ID="ddlReferenceDataSelect" runat="server" CssClass="form-control" AutoPostBack="true" aria-label="ReferenceDataSelect" AppendDataBoundItems="True"
                        OnSelectedIndexChanged="ddlReferenceDataSelect_SelectedIndexChanged" Style="margin-top: 10px;" />
                </div>
                <div class="col-sm-6 col-md-4 col-lg-3 outerName" style="float: right; text-align: right;">
                    <%-- <asp:Button ID="btnBack" runat="server" Text="Back to Reference Data" CssClass="buttonBox" CausesValidation="false" OnClick="btnBack_Click" />--%>
                    <asp:LinkButton ID="btnBack" runat="server" CausesValidation="false" OnClick="btnBack_Click" Style="text-decoration: underline !important; font-weight: 700;">Back to Reference Data</asp:LinkButton>

                </div>
            </div>
            <br />
            <br />
        </asp:Panel>
        <asp:MultiView ID="mltEnrollment" runat="server" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vwPAProcedureGrp" runat="server">
                <uc:paproceduregrp id="PAProcedureGrp" runat="server" />
            </asp:View>
            <asp:View ID="vwRDMCodeSets" runat="server">
                <uc:rdmcodesets id="ucRDMCodeSets" runat="server" />
            </asp:View>
            <asp:View ID="vwWPCCodeSets" runat="server">
                <uc:wpccodesets id="ucWPCCodeSets" runat="server" />
            </asp:View>
            <asp:View ID="vwC4CodeSets" runat="server">
                <uc:c4codesets id="ucC4CodeSets" runat="server" />
            </asp:View>
            <asp:View ID="vwCMSCodeSets" runat="server">
                <uc:cmscodesets id="ucCMSCodeSets" runat="server" />
            </asp:View>
            <asp:View ID="vwNUBCCodeSets" runat="server">
                <uc:nubccodesets id="ucNUBCCodeSets" runat="server" />
            </asp:View>
        </asp:MultiView>

    </div>
</asp:Content>
