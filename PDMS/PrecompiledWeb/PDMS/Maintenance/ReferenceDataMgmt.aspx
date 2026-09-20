<%@ page language="C#" autoeventwireup="true" masterpagefile="~/MasterPage.master" inherits="Maintenance_ReferenceDataMgmt, App_Web_nqybrob4" enableEventValidation="false" stylesheettheme="Default" %>

<%@ register src="../UserControls/ReferenceData/RDMCodeSets.ascx" tagname="RDMCodeSets" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/WPCCodeSets.ascx" tagname="WPCCodeSets" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/C4CodeSets.ascx" tagname="C4CodeSets" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/CMSCodeSets.ascx" tagname="CMSCodeSets" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/NUBCCodeSets.ascx" tagname="NUBCCodeSets" tagprefix="uc" %>
<%@ register src="../UserControls/ReferenceData/PAAssignProcedureGrp.ascx" tagname="PAProcedureGrp" tagprefix="uc" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="WhiteBox">

        <asp:Panel ID="pnlUserSelect" runat="server">
            <span class="formLabelAuto">Reference Data:</span>
            <asp:DropDownList ID="ddlReferenceDataSelect" runat="server" CssClass="formDropDown" AutoPostBack="true" aria-label="ReferenceDataSelect" AppendDataBoundItems="True"
                onselectedindexchanged="ddlReferenceDataSelect_SelectedIndexChanged" />
            <asp:Button ID="btnBack" runat="server" Text="Back to Reference Data" CssClass="buttonBox" CausesValidation="false" OnClick="btnBack_Click" />
            <br />
            <br />
        </asp:Panel>
        <asp:MultiView ID="mltEnrollment" runat="server" ActiveViewIndex="0" EnableViewState="true">
            <asp:View ID="vwPAProcedureGrp" runat="server">
                <uc:PAProcedureGrp id="PAProcedureGrp" runat="server" />
            </asp:View>
            <asp:View ID="vwRDMCodeSets" runat="server">
                <uc:RDMCodeSets id="ucRDMCodeSets" runat="server" />
            </asp:View>
            <asp:View ID="vwWPCCodeSets" runat="server">
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
            </asp:View>
        </asp:MultiView>

    </div>
</asp:Content>