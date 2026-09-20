<%@ page language="C#" autoeventwireup="true" masterpagefile="~/MasterPage.master" inherits="Process_ProviderOperatorHome, App_Web_rnw0hezi" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/ProviderOperatorView.ascx" TagName="ProviderOperatorView" TagPrefix="viewSummary" %>
<%@ Register Src="~/PopupControls/ProviderManagementView.ascx" TagName="ProviderManagementView" TagPrefix="viewDetail" %>
<%@ Register Src="~/PopupControls/ProviderAddView.ascx" TagName="ProviderKFEView" TagPrefix="viewKFE" %> 



<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    <asp:Literal ID="pagelabel" runat="server" text="My Queue - Paper Applications"></asp:Literal>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
                 <div class="WhiteBox">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Always" >

        <ContentTemplate>
       <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
        <viewSummary:ProviderOperatorView ID="ucProviderOperatorView" runat="server"/>

        <asp:UpdateProgress runat="server"  ID="upProgress" DisplayAfter="0"  >
            <ProgressTemplate>
                <div class="loading">
                    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/Images/ajax-loader.gif" />Loading...
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <br />

        <viewDetail:ProviderManagementView ID="ucProviderManagementView" runat="server" Visible="false" />

            <%--    KEY FIELD EDIT POP UP--%>
                <ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlKFE" TargetControlID="btnDummy"
                    RepositionMode="RepositionOnWindowScroll"  BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlKFE"  >
                </ajax:ModalPopupExtender>
                <asp:Panel ID="pnlKFE" runat="server" CssClass="modalPopup"  Style="display: none; min-height: 340px; min-width:300px; height: auto; width: auto; max-width:660px;">
                    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
                        <div class="popTitle"><asp:Label ID="lblKFETitle" runat="server" Text="Edit Key Registration Fields"></asp:Label></div>
                    </asp:Panel>
                    <viewKFE:ProviderKFEView ID="ucProviderAddView" runat="server"/>
                   
                </asp:Panel>
                <asp:Button runat="server" ID="btnDummy" Style="display: none" Text="btnDummy" />
            <br />
            <br />

             </ContentTemplate>
    </asp:UpdatePanel>
                     </div>

</asp:Content>