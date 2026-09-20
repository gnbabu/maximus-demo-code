<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_HospiceEnrollment, App_Web_qtcaivva" enableEventValidation="false" stylesheettheme="Default" %>

<%@ PreviousPageType TypeName="RegistrationProvider" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/SelfServiceProgressBar.ascx" TagPrefix="uc7" TagName="SelfServiceProgressBar" %>
<%@ Register Src="~/PopupControls/HospiceEnrollSearch.ascx" TagPrefix="uc8" TagName="HospiceEnrollSearch" %>

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

            <div id="divHospiceEnrollSearch" runat="server">
                <div class="enrollment">
                </div>
                <asp:Panel ID="pnlHospiceEnrollSearch" runat="server">
                    <uc8:HospiceEnrollSearch runat="server" ID="HospiceEnrollSearch" Visible="true" EnableViewState="true" />
                </asp:Panel>
            </div>
          
          </asp:Panel>
    </div>
      
</asp:Content>

