<%@ page title="Credentialing Committee Review" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_CredentialQueue, App_Web_rnw0hezi" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/CredentialReviewSearch.ascx" TagName="CredentialReviewSearch" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    ODM Chair Review/Credentials Committee Review
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="WhiteBox">
        <script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.min.js" type="text/javascript"></script>

        <style type="text/css">
            .col-sm-7 {
                width: 52% !important;
            }
        </style>
        <br />
        <asp:Label runat="server" ID="lblMessage" CssClass="error-message" Text="" Visible="false" />
        <asp:Panel ID="pnlSearch" runat="server">
            <uc:CredentialReviewSearch ID="credentialSearch" runat="server" />
            <div class="btnBox btnBoxCenter">
                <asp:Button ID="btnCredentialSearch" runat="server" Text="Search" CssClass="buttonBoxFocus" OnClick="btnCredentialSearch_Click" />
                <asp:Button ID="btnCredentialClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBox" OnClick="btnCredentialClear_Click" />
            </div><br /><br /><br />
        </asp:Panel>
        <div>
            <div class="boxPanelHeader">Work Items</div>
            <asp:CheckBox ID="chkAllCredential" runat="server" Text="<b>Approve all displayed providers</b>" AutoPostBack="true" OnCheckedChanged="chkAllCredential_CheckedChanged" />
            <asp:Panel ID="pnlCredentialQueue" runat="server">
                <div class="boxPanelData">
                    <asp:GridView
                        runat="server"
                        Width="100%"
                        ID="gvCredentialProviders"
                        AutoGenerateColumns="False"
                        HorizontalAlign="Center"
                        CssClass="gridViewSmallFont"
                        EmptyDataText="No records found."
                        DataKeyNames="REG_ID,TASK_ID,WORKFLOW_ID,PROCESS_ID"
                        OnRowCommand="gvCredentialProviders_RowCommand"
                        OnSelectedIndexChanged="gvCredentialProviders_SelectedIndexChanged"
                        OnPageIndexChanged="gvCredentialProviders_PageIndexChanged" OnRowDataBound="gvCredentialProviders_RowDataBound" OnRowCreated ="gvCredentialProviders_OnRowCreated"
                        PageSize="15">
                        <Columns>
                            <asp:TemplateField ShowHeader="False" HeaderText="Recommended Risk Level">
                                <ItemTemplate>
                                    <asp:Label ID="lblDataRank" runat="server" Text='<%# Eval("CredentialRiskLevelName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderText="Name">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LnkBtnName" runat="server" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' Text='<%# Eval("PROVIDER_NAME") %>'
                                        CommandName="GoToRegistration" PostBackUrl="~/Process/Registration.aspx" CssClass="gridLink"></asp:LinkButton>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("PROVIDER_NAME") %>' Visible="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ReadOnly="true" DataField="TASK_NAME" HeaderText="Task" />
                            <asp:BoundField ReadOnly="true" DataField="WORKFLOW_NAME" HeaderText="Workflow" />
                            <asp:BoundField ReadOnly="true" DataField="NPI" HeaderText="NPI" />
                            <asp:BoundField ReadOnly="true" DataField="PROVIDER_TYPE_NAME" HeaderText="Provider Type" />
                            <asp:BoundField ReadOnly="true" DataField="AGING" HeaderText="Aging" />
                            <asp:BoundField ReadOnly="true" DataField="PROVIDER_RISK_LEVEL_NAME" HeaderText="Provider Risk Level" />
                            <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderText="">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnPage" runat="server" Value='<%# Eval("CLASS_NAME") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ReadOnly="true" DataField="credentialing_id" HeaderText="Credential" Visible="false" />
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>
                <br />
            </asp:Panel>
        </div>
        <asp:Button runat="server" ID="btnPopupDummy" Style="display: none" />
        <ajax:ModalPopupExtender ID="mpeCredentialApprove" runat="server" PopupControlID="pnlCredentialApprove" TargetControlID="btnPopupDummy"
            RepositionMode="RepositionOnWindowScroll" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlCredentialApprove">
        </ajax:ModalPopupExtender>
        <asp:Panel runat="server" ID="pnlCredentialApprove" CssClass="modalPopup" align="center" Style="display: none; width: 50%; height: auto;">
            <asp:UpdatePanel ID="upPanelApprove" runat="server">
                <ContentTemplate>
                    <div style="padding: 5px;">
                        <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
                            <div class="popTitle">
                                <asp:Label ID="lblMpeCommitteeTitle" runat="server" Text="Take Action" />
                            </div>
                        </asp:Panel>
                        <asp:Label runat="server" ID="Label1" CssClass="error-message" />
                        <div>
                            <span style="color: red; font-weight: bold">By clicking approve, you are approving all providers returned in this search.</span>
                        </div>
                        <br />
                    </div>
                    <asp:UpdateProgress runat="server" ID="upProgressApprove" DisplayAfter="0">
                        <ProgressTemplate>
                            <div class="loading">
                                <asp:Image ID="imgLoading" runat="server" ImageUrl="~/Images/ajax-loader.gif" />Loading...
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="btnBox" style="padding-top: 10px; padding-right: 10px;">
                <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="buttonBoxFocus" CausesValidation="false" OnClick="btnApprove_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>

