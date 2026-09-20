<%@ page language="C#" autoeventwireup="true" inherits="Account_Logout, App_Web_zwaluqkv" masterpagefile="~/MasterPage.master" enableEventValidation="false" stylesheettheme="Default" %>


<%@ Register TagPrefix="jk" Namespace="JK.BootstrapControls" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="PageLabelContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="container-fluid">
        <div class="row"> 
        <cc1:ModalPopupExtender ID="mpeLogoutMsg" runat="server" PopupControlID="pnlLogoutMsg" TargetControlID="btnlogoutDummy"
                BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnlLogoutMsg" runat="server" CssClass="modalPopup" align="center" Style="display: none; width: 70%; height: auto;">
                <asp:Panel ID="Panel3" CssClass="popHeader" runat="server">
                    <div class="popTitle">Logout Message</div>
                </asp:Panel>
                <asp:Panel ID="Panel1" runat="server" Style="margin-right: 10px">
                    <div style="text-align: left; padding: 15px; margin-left: 30px !important" class="container-fluid">
                        <p>You will now be navigated to IOP. Please log out of IOP to end your session.</p>
                    </div>
                </asp:Panel>
                <div class="btnBox btnBoxCenter" style="padding-top: 10px; width: 93%;">
                    <asp:Button ID="btnLogoutOK" runat="server" Text="Ok" CssClass="buttonBox" OnClick="btnLogoutOk_Clicked" />
                </div>
            </asp:Panel>

            <asp:Button runat="server" ID="btnlogoutDummy" aria-Label="DummyButton" Style="display: none" />
          </div>
    </div>
  </asp:Content>
