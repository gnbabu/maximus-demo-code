<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_SubmitClaim, App_Web_unbhbgmw" enableEventValidation="false" stylesheettheme="Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/SelfServiceProgressBar.ascx" TagPrefix="uc7" TagName="SelfServiceProgressBar" %>
<%@ Register Src="~/PopupControls/SubmitClaim.ascx" TagPrefix="uc5" TagName="SubmitClaim" %>

<asp:Content ID="AUTH" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="WhiteBox">
        <div>
            <asp:Label runat="server" Text="" ID="lblGeneralErr" CssClass="failureNotification"></asp:Label>
        </div>
        <asp:Panel ID="pnlBillingAndotherservice" runat="server">
            <div class="row">
    <%--                <asp:HiddenField ID="HasUnsavedDataPA" runat="server" />--%>
            <asp:UpdatePanel ID="claimSearchUpdatePanel" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="ucRegProgressBar" />
                </Triggers>
                <ContentTemplate>
                    <div id="DivProgressBar" runat="server">
                        <uc7:SelfServiceProgressBar ID="ucRegProgressBar" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>            
        <br />
        <div id="DivSubmitClaims" runat="server">
            <div class="enrollment"></div>
            <asp:Panel ID="pnlSubmitClaims" runat="server">
                <%--        Note:  these views must be in order of reg_page_type_id--%>
                <uc5:SubmitClaim ID="uc5SubmitClaim" runat="server" Visible="true" EnableViewState="true" />

            </asp:Panel>
        </div>    
        </asp:Panel>
    </div>    
</asp:Content>