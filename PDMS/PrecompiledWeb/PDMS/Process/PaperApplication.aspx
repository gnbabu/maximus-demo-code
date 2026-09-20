<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_PaperApplication, App_Web_sdbcnqyo" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/PaperRequestAddView.ascx" TagName="PaperRequestAddView" TagPrefix="viewQ" %>
<%@ Register Src="~/PopupControls/ProviderAddView.ascx" TagName="ProviderAddView" TagPrefix="viewAddProvider" %>
<%@ Register Src="~/PopupControls/ProviderManagementView.ascx" TagName="ProviderManagementView" TagPrefix="viewDetail" %>
<%@ Register Src="~/PopupControls/PaperDocumentView.ascx" TagName="PaperDocumentView" TagPrefix="vewDoc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    <asp:Label ID="lblPageTitle" runat="server" Text="New Paper Request"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="WhiteBox">
 <script src="../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
<%--    <asp:UpdatePanel ID="upProviderType" runat="server"  UpdateMode="Conditional" >
        <ContentTemplate>--%>
           <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
            <asp:Label runat="server" ID="lblInformationalMessage" CssClass="error-message" />
            <viewQ:PaperRequestAddView ID="ucPaperRequestAddView" runat="server"/>
<%--                <asp:UpdateProgress runat="server"  ID="upChildGetProgress" DisplayAfter="0"  >
                    <ProgressTemplate>
                        <div class="loading">
                            <asp:Image ID="imgLoading" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>--%>
    <vewDoc:PaperDocumentView ID="ucPaperDocView" runat="server" />
    <viewDetail:ProviderManagementView ID="ucProviderManagementView" runat="server" Visible="false" />

    <ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlNewRegistration" TargetControlID="btnDummy"
                    RepositionMode="RepositionOnWindowScroll"  
                        BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlNewRegistration"  >
                </ajax:ModalPopupExtender>
       <asp:Panel ID="pnlNewRegistration" runat="server" CssClass="modalPopup"  Style="display: none; min-height: 340px; min-width:300px; height: auto; width: auto;">
                    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
                        <div class="popTitle"><asp:Label ID="lblNewRegTitle" runat="server" Text="New Registration"></asp:Label></div>
                    </asp:Panel>
                    <viewAddProvider:ProviderAddView ID="ucProviderAddView" runat="server"/>
                </asp:Panel>
    <asp:Button runat="server" ID="btnDummy" Style="display: none" Text="BtnDummy" />
        </div>
</asp:Content>

