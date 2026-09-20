 <%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Process_SearchPriorAuthorization" Codebehind="SearchPriorAuthorization.aspx.cs" %>

 <%@ PreviousPageType TypeName="RegistrationProvider" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/SelfServiceProgressBar.ascx" TagPrefix="uc7" TagName="SelfServiceProgressBar" %>
<%@ Register Src="~/PopupControls/SearchPriorAuthorization.ascx" TagPrefix="uc8" TagName="SearchPriorAuthorization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <script src="../Scripts/HospiceCommon.js"></script>     
</asp:Content>
<asp:Content ID="AUTH" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="WhiteBox">
          <asp:Panel ID="pnlBillingAndotherservice" runat="server">
              <div class="row">
                <asp:HiddenField ID="HasUnsavedDataPA" runat="server" />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
            <div id="divPriorAuthSearch" runat="server">
                <div class="paSearch">
                </div>
                <asp:Panel ID="pnlPASearch" runat="server">
                    <uc8:SearchPriorAuthorization runat="server" ID="SearchPriorAuthorization" Visible="true" EnableViewState="true" />
                </asp:Panel>
            </div>        
          </asp:Panel>
    </div>    
</asp:Content>

