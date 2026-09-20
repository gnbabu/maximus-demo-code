<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_UploadGlobalAdminChange" Codebehind="UploadGlobalAdminChange.ascx.cs" %>

<asp:Panel ID="pnl" runat="server" CssClass="modal-panel">
    <h4>Upload Global Administrator Change Document</h4>

    <asp:Label ID="lblInfo" runat="server" Text="Please upload the approved form, then click Save." />
    <br /><br />

    <asp:FileUpload ID="fuDoc" runat="server" />
    <asp:RequiredFieldValidator ID="rfvFile" runat="server"
        ControlToValidate="fuDoc" InitialValue=""
        ErrorMessage="Please choose a document before saving."
        Display="Dynamic" />
    <br /><br />

    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
</asp:Panel>
