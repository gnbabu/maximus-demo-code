<%@ page language="C#" autoeventwireup="true" inherits="Process_ProviderDetailsNew, App_Web_unbhbgmw" masterpagefile="~/MasterPage.master" title="Provider Management Details" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Src="~/PopupControls/ProviderManagementView.ascx" TagName="ProviderManagementView" TagPrefix="viewDetail" %>
<%@ Register Src="~/PopupControls/ProviderAddView.ascx" TagName="ProviderAddView" TagPrefix="viewAddProvider" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    Provider Management Details
</asp:Content>--%>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server" >
   
    <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
    <h2>Provider Management Home</h2>
    <viewDetail:ProviderManagementView ID="ucProviderManagementView" runat="server" />

     
    <ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlNewRegistration" TargetControlID="btnDummy"
                    RepositionMode="RepositionOnWindowScroll"  BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlNewRegistration"  >
                </ajax:ModalPopupExtender>
                <asp:Panel ID="pnlNewRegistration" runat="server" CssClass="modalPopup"  Style="display: none; min-height: 340px; min-width:300px; height: auto; width: auto; max-width:660px;">
                    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
                        <div class="popTitle"><asp:Label ID="lblNewRegistration" runat="server" Text="New Registration"></asp:Label></div>
                    </asp:Panel>
                    <viewAddProvider:ProviderAddView ID="ucProviderAddView" runat="server"/>
                </asp:Panel>
                <asp:Button runat="server" ID="btnDummy" Style="display: none" Text="btnDummy"/>
</asp:Content>
