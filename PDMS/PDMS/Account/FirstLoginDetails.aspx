<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="Account_FirstLoginDetails" Codebehind="FirstLoginDetails.aspx.cs" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="PageLabelContent">
    <br />
    User Information
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

     <div class="WhiteBox" style="text-align:center;">
    <asp:ValidationSummary ID="flValidationSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="flValGroup" />
     <asp:UpdatePanel ID="upValidationSummary" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlValidationSummary" runat="server" Style="width: 100%" Visible="false">
                <asp:Label ID="lblError" runat="server" Visible="false" Text="" CssClass="failureNotification" Style="color: Red" />

            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlftlogin" runat="server">
      <div class="boxPanelData">
          <div class="row">
            <div class="col-sm-4 text-right">
                <span class="formLabel"><asp:Label ID="lblMedID" runat="server" Text="Medicaid ID*"/></span>
            </div>
            <div class="col-sm-8 text-left">
                <asp:TextBox ID="txtMedicaidID" runat="server" MaxLength="13" CssClass="formField" />
                <asp:RequiredFieldValidator ID="valMediaidIDReqd" runat="server" ControlToValidate="txtMedicaidID"
                    Enabled="false" SetFocusOnError="true" Display="Dynamic" Text="*"
                    ValidationGroup="flValGroup" ErrorMessage="* Medicaid ID is required. Please include leading zeroes as applicable."></asp:RequiredFieldValidator>
            </div>
          </div>
          <div class="row">
            <div class="col-sm-4 text-right">
                <span class="formLabel"><asp:Label ID="lblEmail" runat="server" Text="Email Address*"/></span>
            </div>
            <div class="col-sm-8 text-left">
                <asp:TextBox runat="server" ID="txtEmail" CssClass="formField" />
                <asp:RequiredFieldValidator ID="valEmailRequired" runat="server" ControlToValidate="txtEmail"
                    ValidationGroup="flValGroup" ErrorMessage="* Email is required." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="valEmailFormat" runat="server" ControlToValidate="txtEmail" Display="Dynamic" Text="*"
                    ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                    ErrorMessage="* Invalid email format." ValidationGroup="flValGroup"></asp:RegularExpressionValidator>
            </div>
          </div>
          <div class="row">
            <div class="col-sm-4  text-right">
            <span class="formLabel"><asp:Label ID="lblconfirmEmail" runat="server" Text="Confirm Email" /></span>
            </div>
            <div class="col-sm-8 text-left">
            <asp:TextBox runat="server" ID="txtConfirmEmail" CssClass="formField" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtConfirmEmail"
            ValidationGroup="flValGroup" ErrorMessage="*Confirm Email is required." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtConfirmEmail" Display="Dynamic" Text="*"
                    ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                    ErrorMessage="* Invalid email format." ValidationGroup="flValGroup"></asp:RegularExpressionValidator>
            <asp:CustomValidator ID="cvCEmailMatch" runat="server" OnServerValidate="Validate_cvCEmailMatch"
                Display="Dynamic" ValidationGroup="flValGroup" ErrorMessage="* Email addresses must match."
                Text="* Email addresses must match." />
                </div>
          </div>
          <div class="btnBox" style="margin-right:10%;">
                <asp:Button ID="btnSendEmail" runat="server" Text="Send Email" CssClass="buttonBoxFocus" OnClick="btnSendEmail_Click" CausesValidation="true" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" CausesValidation="false" OnClick="btnCancel_Click" />
          </div>
      </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlSuccess" Visible="false">
        <div style="margin-left:auto; margin-right:auto;text-align:center;">
            <p>An Email with a temporary password is sent to the email you entered.<br />Login with this temporary Password to create your profile.</p>
        </div>       
     </asp:Panel>
  </div>
  <asp:HiddenField ID="hdnUname" runat="server" />
</asp:Content>