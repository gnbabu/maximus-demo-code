<%@ Page Language="C#" AutoEventWireup="true" Inherits="Process_ProviderRetrieveReports" MasterPageFile="~/MasterPage.master" Codebehind="ProviderRetrieveReports.aspx.cs" %>

<%@ PreviousPageType TypeName="RegistrationProvider" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/SelfServiceProgressBar.ascx" TagPrefix="uc7" TagName="SelfServiceProgressBar" %>
<%@ Register Src="~/PopupControls/RetrieveReports.ascx" TagPrefix="uc9" TagName="RetrieveReports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="text-align: left !important"></div>
    <!--  <asp:Label id="lblPgTitle" runat="server" CssClass="pageHeader" />-->

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

           <div id="divRetrieveReports" runat="server">
                <div class="enrollment">
                </div>
                <asp:Panel ID="Panel1" runat="server">
                    <uc9:RetrieveReports runat="server" ID="ucRetrieveReports" Visible="true" EnableViewState="true" />
                </asp:Panel>
            </div>
          
          </asp:Panel>
    </div>

</asp:Content>
