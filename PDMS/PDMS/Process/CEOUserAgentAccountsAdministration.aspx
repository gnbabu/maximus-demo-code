<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="Process_CEOUserAgentAccountsAdministration" Codebehind="CEOUserAgentAccountsAdministration.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="PageLabelContent">
    Provider Account Administration
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style type="text/css">
        table {
            text-align: center;
        }

        th, td {
            padding: 5px;
        }

        .form-control {
            font-family: "Arial Narrow";
            font-size: 14pt !important;
        }

        .rcbInner {
            border-style: none !important;
            display: inline !important;
        }

        .addUserModalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.2;
            height: auto;
        }

        .addUserModalPopup
        {
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding: 0px;
            width: auto;
            height: auto;
        }
    </style>
  <div class="WhiteBox">
       <asp:ValidationSummary ID="ManageAgentValidationSummary" runat="server" CssClass="failureNotification" ValidationGroup="ManageAgentValidationSummary"/>
      <asp:Label ID="URP03_ERR" runat="server" Visible="false" Text="" CssClass="failureNotification" Style="color: Red" /> 
     <br />
    <asp:Panel runat="server" ID="panel" Width="90%" HorizontalAlign="Center">
        <div style="text-align: left; margin-left: 20%;overflow:auto">
            <asp:GridView runat="server" ID="grdDeactivateUser" AutoGenerateColumns="false"  ShowHeader="true" Visible="true" Width="90%"
                EmptyDataText="No Secondary users to activate/de-activate." ShowHeaderWhenEmpty="true" OnRowDataBound="grdDeactivateUser_RowDataBound" CellPadding="5">
                <Columns></Columns>
            </asp:GridView>
            <asp:GridView runat="server" ID="grdSecondaryUserFacility" AutoGenerateColumns="false" ShowHeader="true" Visible="true" Width="90%"
                EmptyDataText="No matching records found." ShowHeaderWhenEmpty="true" OnRowDataBound="grdSecondaryUserFacility_RowDataBound" CellPadding="5">
                <Columns></Columns>
            </asp:GridView>
            <asp:GridView runat="server" ID="grdSecondaryUserContract" AutoGenerateColumns="false" ShowHeader="true" Visible="true" Width="90%"
                EmptyDataText="No matching records found." ShowHeaderWhenEmpty="true" OnRowDataBound="grdSecondaryUserContract_RowDataBound" CellPadding="5">
                <Columns></Columns>
            </asp:GridView>
        </div>
        <br /> 
        <br />
        <div style="text-align: center;margin-left: 20%;">

            <asp:Button ID="btnAddUser" runat="server" CssClass="buttonBox" OnClick="btnAddUser_Click" Text="Add User" />
            <asp:Button ID="btnSave" runat="server" CssClass="buttonBox" OnClick="btnSave_Click" Text="Save" />
             <asp:Button ID="btnReturn" runat="server" CssClass="buttonBox" OnClick="btnReturn_Click" Text="Cancel"  />
        </div>
        <br />
    </asp:Panel>
     <ajax:ModalPopupExtender ID="mpeAddAgent" runat="server" PopupControlID="pnlAddAgent" TargetControlID="ButtonDummy" 
         CancelControlID="btnCancel" BackgroundCssClass="addUserModalBackground" PopupDragHandleControlID="pnlAddAgent">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlAddAgent" runat="server" CssClass="addUserModalPopup" align="left" Style="display: none;">
        <asp:Panel ID="pnlHeaderMpe" Style="cursor: move; padding: 5px;" BackColor="#205794" runat="server" HorizontalAlign="Left">
            <div style="text-align: left">
                &nbsp;&nbsp;
                <asp:Label ID="lblTitle" CssClass="bodyTextBold" runat="server" Text="User Information" ForeColor="White" />
            </div>
        </asp:Panel>
        <br />
       <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="failureNotification" ValidationGroup="AddAgentValidationSummary"/>
         <asp:Label ID="lblmpeError" runat="server" Visible="false" Text="" CssClass="failureNotification" Style="color: Red" /> 
        <br /> 
        <div style="text-align: center; margin-right:10%;" class="tablepad">
            <div class="row">
                <div class="col-sm-4 text-right">
                    <span class="formLabel">OHID*</span>
                </div>
                <div class="col-sm-8 text-left">
                    <asp:TextBox runat="server" ID="UserName" CssClass="formField" MaxLength="50" />                                   
                    <asp:RequiredFieldValidator runat="server" ID="reqUserID" ControlToValidate="UserName" Display="Dynamic" 
					ErrorMessage="* Agent User ID is required" SetFocusOnError="true" Text="*" ValidationGroup="AddAgentValidationSummary" />
					<asp:CustomValidator ID="cvUserIDExists" runat="server" OnServerValidate="Validate_UserNameExists"
                                ControlToValidate="UserName" Display="Dynamic" Text="*"
                                ValidationGroup="AddAgentValidationSummary" ErrorMessage="* Agent is not created in the System." />       
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4 text-right">
                    <span class="formLabel">Email Address*</span>
                </div>
                <div class="col-sm-8 text-left">
                   <asp:TextBox runat="server" ID="Email" CssClass="formField" />
                   <asp:RequiredFieldValidator ID="valEmailRequired" runat="server" ControlToValidate="Email"
                    ValidationGroup="AddAgentValidationSummary" ErrorMessage="* Email is required." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                   <asp:RegularExpressionValidator ID="valEmailFormat" runat="server" ControlToValidate="Email" Display="Dynamic" Text="*"
                    ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                    ErrorMessage="* Invalid email format." ValidationGroup="AddAgentValidationSummary"></asp:RegularExpressionValidator>        
                </div>
                <br /><div class="col-sm-4 text-right"></div><span class="col-sm-8 text-left">This is the email address used to create the OH|ID</span>
            </div>
            <div class="row">
                <div class="col-sm-4 text-right">
                    <span class="formLabel">Confirm Email*</span>
                </div>
                <div class="col-sm-8 text-left">
                    <asp:TextBox runat="server" ID="ConfirmEmail" CssClass="formField" />
                     <asp:RequiredFieldValidator ID="valConfirmEmailRequired" runat="server" ControlToValidate="ConfirmEmail"
                        ValidationGroup="AddAgentValidationSummary" ErrorMessage="* Confirm Email is required." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                     <asp:CompareValidator ID="valEmailsCompare" runat="server" ControlToValidate="ConfirmEmail" ControlToCompare="Email"
                        Type="String" Operator="Equal" Display="Dynamic" Text="*"
                        ValidationGroup="AddAgentValidationSummary" ErrorMessage="* Email addresses must match."></asp:CompareValidator>        
                </div>
            </div>
        </div>
        <div class="btnBox" style="margin-right:10%;">
                <asp:Button ID="btnSaveAddAgent" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSaveAddAgent_Click" CausesValidation="true" ValidationGroup="AddNewProvider" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" CausesValidation="false" />
        </div>
    </asp:Panel>
    <asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy"/>

  </div>
     <asp:HiddenField ID="hdnRegID" runat="server" />
</asp:Content>
