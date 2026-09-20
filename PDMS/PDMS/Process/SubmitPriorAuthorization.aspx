<%@ page language="C#" autoeventwireup="true" masterpagefile="~/MasterPage.master" inherits="Process_SubmitPriorAuthorization" Codebehind="SubmitPriorAuthorization.aspx.cs" %>

<%@ previouspagetype typename="RegistrationProvider" %>

<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ register src="~/UserControls/SelfServiceProgressBar.ascx" tagprefix="uc7" tagname="SelfServiceProgressBar" %>
<%@ register src="~/PopupControls/SubmitPriorAuthorization.ascx" tagprefix="uc1" tagname="SubmitPriorAuthorization" %>



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
                            <uc7:selfserviceprogressbar id="ucRegProgressBar" runat="server" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <br />
            <div id="divPriorAuthSearch" runat="server">
                <div class="paSearch">
                </div>
                <asp:Panel ID="pnlPASearch" runat="server">
                    <uc1:submitpriorauthorization runat="server" id="uc1SubmitPriorAuthorization" visible="true" enableviewstate="true" />
                </asp:Panel>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

