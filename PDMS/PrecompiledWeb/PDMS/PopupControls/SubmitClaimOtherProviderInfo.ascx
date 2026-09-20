<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SubmitClaimOtherProviderInfo, App_Web_wenzyumt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>


<asp:UpdatePanel ID="upOtherProviderInfo" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlOtherProviderInfo" runat="server" DefaultButton="btnSave" Style="padding: 10px;">
            <div style="width: 600px;">
                <asp:ValidationSummary ID="vsOtherProviderInfo" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="vgOtherProviderInfo"/>
                <div class="wdAuto">
                    <div class="row OtherProviderInfo" runat="server">
                        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                    <asp:Label ID="lblOtherPhysician" Text="Other Physician" runat="server" class="formLabel200" />
                </div>
                <div class="col-sm-9 text-left">
                    <asp:DropDownList ID="ddlOtherPhysician" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlOtherPhysician_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" SetFocusOnError="true"
                        ValidationGroup="valOwnerInfo" ControlToValidate="ddlOtherPhysician" ErrorMessage="*" Text="*" Display="Dynamic" InitialValue="0" />
                </div>
                <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                    <asp:Label ID="lblMedicaidID" class="formLabel200" Text="Medicaid ID" runat="server" />
                </div>
                <div class="col-sm-9 text-left">
                    <asp:TextBox ID="txtMedicaidID" runat="server" MaxLength="35" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true" />
                </div>
                <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                    <asp:Label ID="lblProviderName1" class="formLabel200" Text="Name" runat="server" />
                </div>
                <div class="col-sm-9 text-left">
                    <asp:TextBox ID="txtProviderName1" runat="server" MaxLength="35" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true" />
                </div>
                    </div>
                </div>
                <asp:UpdateProgress runat="server" ID="upSubmitClaim" DisplayAfter="0" AssociatedUpdatePanelID="upOtherProviderInfo">
                    <ProgressTemplate>
                        <div class="loading">

                            <asp:Image ID="imgOtherProviderInfo" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="PriorAuthNotes" /></td>
                        <td>
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" /></td>
                    </tr>
                </table>
            </div>

        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
