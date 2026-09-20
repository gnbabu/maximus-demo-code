<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Process_ClaimSearch" Codebehind="ClaimSearch.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/SelfServiceProgressBar.ascx" TagPrefix="uc7" TagName="SelfServiceProgressBar" %>
<%@ Register Src="~/PopupControls/ClaimSearch.ascx" TagPrefix="uc6" TagName="ClaimSearch" %>

<asp:Content ID="AUTH" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="WhiteBox">
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
            <div id="divClaimSearch" runat="server">
                <div class="enrollment"></div>
                <asp:Panel ID="pnlClaimSearch" runat="server">
                    <uc6:ClaimSearch runat="server" ID="uc6ClaimSearch" Visible="true" EnableViewState="true" />
                </asp:Panel>
            </div>        
          </asp:Panel>
    </div>    
</asp:Content>