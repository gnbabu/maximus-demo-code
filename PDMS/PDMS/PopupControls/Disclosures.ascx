<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_Disclosures" codebehind="Disclosures.ascx.cs" %>
<%@ register assembly="eWorld.UI" namespace="eWorld.UI" tagprefix="ew" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>
<%@ register src="~/PopupControls/DisclosureQuestion.ascx" tagname="Question" tagprefix="uc" %>

<asp:panel id="pnlQuestions" runat="server" cssclass="OwnerBackground">
    <div class="panelOwnerInfo">
        <uc:question id="ucD01" runat="server" option="1" questiontypeid="D01" section="Questions" />
        <br />

        <uc:question id="ucD02" runat="server" option="2" questiontypeid="D02" section="Questions" />
        <br />
        <uc:question id="ucD03" runat="server" option="3" questiontypeid="D03" section="Questions" />
        <br />
        <uc:question id="ucD04" runat="server" option="4" questiontypeid="D04" section="Questions" />
        <br />
        <uc:question id="ucD05" runat="server" option="5" questiontypeid="D05" section="Questions" />
        <br />
        <uc:question id="ucD06" runat="server" option="6" questiontypeid="D06" section="Questions" />
        <br />
        <uc:question id="ucD07" runat="server" option="7" questiontypeid="D07" section="Questions" />
        <br />
        <uc:question id="ucD09" runat="server" option="9" questiontypeid="D09" section="Questions" />
        <br />
        <uc:question id="ucD10" runat="server" option="10" questiontypeid="D10" section="Questions" />
        <br />
        <uc:question id="ucD11" runat="server" option="11" questiontypeid="D11" section="Questions" />
        <br />
        <uc:question id="ucD12" runat="server" option="12" questiontypeid="D12" section="Questions" />
        <br />
        <uc:question id="ucD13" runat="server" option="13" questiontypeid="D13" section="Questions" />
        <br />
        <uc:question id="ucD14" runat="server" option="14" questiontypeid="D14" section="Questions" />
        <br />
        <uc:question id="ucD15" runat="server" option="15" questiontypeid="D15" section="Questions" />
        <br />
        <uc:question id="ucD16" runat="server" option="16" questiontypeid="D16" section="Questions" />
        <br />
        <%--<asp:hiddenfield id="hdnAccessToken" runat="server" />
        <asp:hiddenfield id="hdnWebAPIURL" runat="server" />--%>        
    </div>
</asp:panel>
<asp:hiddenfield id="hdnRegId" runat="server" />
