<%@ page title="ProviderCorrespondence" language="C#" autoeventwireup="true" inherits="Process_ProviderCorrespondence, App_Web_rnw0hezi" masterpagefile="~/MasterPage.master" enableEventValidation="false" stylesheettheme="Default" %>

<%@ PreviousPageType TypeName="RegistrationProvider" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/SelfServiceProgressBar.ascx" TagPrefix="uc7" TagName="SelfServiceProgressBar" %>
<%@ Register Src="~/PopupControls/Correspondence.ascx" TagPrefix="uc9" TagName="Correspondence" %>

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

            <div id="divCorrespondence" runat="server">
                <div class="enrollment">
                </div>
                <asp:Panel ID="pnlCorrespondence" runat="server">
                    <uc9:Correspondence runat="server" ID="uc9Correspondence" Visible="true" EnableViewState="true" />
                </asp:Panel>
            </div>
          
          </asp:Panel>
    </div>

</asp:Content>