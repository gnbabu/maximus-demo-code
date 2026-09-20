<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="Process_ProviderAdminAgentAccounts" Codebehind="ProviderAdminAgentAccounts.aspx.cs" %>

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
		.stickyHeader
		{
			position:absolute;
		}
    </style>
  <div class="WhiteBox">
       <asp:ValidationSummary ID="ManageAgentValidationSummary" runat="server" CssClass="failureNotification" ValidationGroup="ManageAgentValidationSummary"/>
      <asp:Label ID="URP03_ERR" runat="server" Visible="false" Text="" CssClass="failureNotification" Style="color: Red" /> 
      <div id="divChangeOwner" runat="server">
        <%--<div style="text-align: left; margin-left: 5%;">Change Account admin</div>--%>
        <div id="divChangeOwner1">
            <asp:Panel ID="pnlChangeOwner" runat="server">
                <div style="text-align: left; margin-left: 5%;" class="tablepad">
                    <table >
                    <tr><td>
                    <div class="row">
                        <div class="col-sm-4 text-right">
                            <span class="formLabel">Medicaid ID:</span>
                        </div>
                        <div class="col-sm-8 text-left">                            
                            <asp:DropDownList runat="server" ID="ddlMedicaidID" AutoPostBack="true" AppendDataBoundItems="True" OnSelectedIndexChanged ="ddlMedicaidID_SelectedIndexChanged"> </asp:DropDownList>
                        </div>
                    </div>
                    </td>
                    <td>
                        <div class="row" id="divChangeAdmin" runat ="server">
                        <div class="col-sm-8 text-left" >
                            <span class="formLabel">Change admin to:</span>
                        </div>
                        <div class="col-sm-8 text-left fieldValue wd80">
                            <asp:Textbox ID="txtChgAdmin" CssClass="formField" runat="server"/>
                        </div>
                    </div>
                    </td>
                    </tr>
                    <tr><td>
                    <div class="row">
                        <div class="col-sm-4 text-right">
                            <span class="formLabel">Name:</span>
                        </div>
                        <div class="col-sm-8 text-left">
                            <asp:Textbox ID="lblName" CssClass="formField" runat="server" Enabled ="false"/>
                        </div>
                    </div>
                    </td>
                    <td>
                        <div class="row">
                        <div class="col-sm-8 text-right">
                            <span class="formLabel">&nbsp;</span>
                        </div>
                        <div class="col-sm-8 text-left fieldValue wd80">
                            <asp:Button ID="btnChangeOwner" runat="server" Text="Change Admin" CssClass="buttonBox" OnClick="btnChangeOwner_Click" />
                        </div>
                    </div>
                    </td>
                    </tr>
                    <tr><td>
                    <div class="row">
                        <div class="col-sm-4 text-right">
                            <span class="formLabel">Select Agent:</span>
                        </div>
                        <div class="col-sm-8 text-left fieldValue wd300">
                            <asp:Textbox ID="txtSeltAgent" CssClass="formField" runat="server"/>
                        </div>
                    </div>
                    </td></tr>
                    <tr><td>
                    <div class="row">
                        <div class="col-sm-4 text-right">
                            <span class="formLabel">&nbsp;</span>
                        </div>
                        <div class="col-sm-8 text-left">
                            <asp:Button ID="btnSearchAgent" runat="server" Text="Search" CssClass="buttonBox" OnClick="btnSearchAgent_Click" />
                        </div>
                    </div>
                    </td></tr>                   
                   </table>
                </div>    
                
            </asp:Panel>
        </div>
    </div>
    <br />
    <br />
    <asp:Panel runat="server" ID="panel" Width="100%" HorizontalAlign="Center">
        <div style="text-align: left; margin-left: 0%; margin-right: 0%">
			<div style="overflow-y:scroll">
            <asp:GridView runat="server" ID="grdDeactivateUser" AutoGenerateColumns="false"  ShowHeader="true" Visible="true" Width="100%" CellContentClick ="GridView_MyCellCententClicked"
                EmptyDataText="No users to activate/de-activate." ShowHeaderWhenEmpty="true" OnRowDataBound="grdDeactivateUser_RowDataBound" CellPadding="5">
                <Columns></Columns>
            </asp:GridView>
			</div>
			<div style="height:500px; overflow:scroll">
            <asp:GridView runat="server" ID="grdAgentRoles" AutoGenerateColumns="false" ShowHeader="true" Visible="true" Width="100%"
                EmptyDataText="No matching records found." ShowHeaderWhenEmpty="true" OnRowDataBound="grdAgentRoles_RowDataBound" CellPadding="5" >
                <Columns>
					<asp:TemplateField HeaderStyle-CssClass="stickyHeader"/>
                </Columns>
            </asp:GridView>
			</div>
        </div>
        <br /> 
        <div style="text-align: right; margin-left: 70%;">
                    <table>
                        <tr>
                            <td style="align-content:center">
                                <asp:LinkButton ID="lbFirst" runat="server" OnClick="lbFirst_Click"><<</asp:LinkButton>
                            </td>
                            <td style="align-content:center">
                                <asp:LinkButton ID="lbPrevious" runat="server" OnClick="lbPrevious_Click"><</asp:LinkButton>
                            </td>
                            <td>
                                <asp:DataList ID="rptPaging" runat="server"
                                    OnItemCommand="rptPaging_ItemCommand"
                                    OnItemDataBound="rptPaging_ItemDataBound" RepeatDirection="Horizontal">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbPaging" runat="server"
                                            CommandArgument='<%# Eval("PageIndex") %>' CommandName="newPage"
                                            Text='<%# Eval("PageText") %> ' Width="20px">
						</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                            <td>
                                <asp:LinkButton ID="lbNext" runat="server"  style="align-content:center"
				OnClick="lbNext_Click">></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lbLast" runat="server" style="align-content:center"
				OnClick="lbLast_Click">>></asp:LinkButton>
                            </td>
                            <td>
                                <asp:Label ID="lblpage" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>

                </div>
        <br />
        <div style="text-align: center;margin-left: 0%;">

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
                    <span class="formLabel" style="text-align: left; padding-left: 12px">OHID*</span>
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
                    <span class="formLabel" style="text-align: left; padding-left: 12px">Email Address*</span>
                </div>
                <div class="col-sm-8 text-left">
                   <asp:TextBox runat="server" ID="Email" CssClass="formField" />
                   <asp:RequiredFieldValidator ID="valEmailRequired" runat="server" ControlToValidate="Email"
                    ValidationGroup="AddAgentValidationSummary" ErrorMessage="* Email is required." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                   <asp:RegularExpressionValidator ID="valEmailFormat" runat="server" ControlToValidate="Email" Display="Dynamic" Text="*"
                    ValidationExpression="^([0-9a-zA-Z+]([-.\w]*[0-9a-zA-Z+])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                    ErrorMessage="* Invalid email format." ValidationGroup="AddAgentValidationSummary"></asp:RegularExpressionValidator>        
                </div>
                <br /><div class="col-sm-4 text-right"></div><span class="col-sm-8 text-left">This is the email address used to create the OH|ID</span>

            </div>
            <div class="row">
                <div class="col-sm-4 text-right">
                    <span class="formLabel" style="text-align: left; padding-left: 12px">Confirm Email*</span>
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
      <ajax:ModalPopupExtender ID="Mod_UserConfirm" runat="server" PopupControlID="Pnl_UserConfirm" TargetControlID="ButtonDummy" 
         CancelControlID="Btn_No" BackgroundCssClass="addUserModalBackground" PopupDragHandleControlID="Pnl_UserConfirm">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="Pnl_UserConfirm" Width="20%" Height="15%" runat="server" CssClass="addUserModalPopup" align="left" Style="display: none;">
        <asp:Panel ID="Panel2" Style="cursor: move; padding: 5px;" BackColor="#205794" runat="server" HorizontalAlign="Left">
            <div style="text-align: left">
                &nbsp;&nbsp;
                <asp:Label ID="Label1" CssClass="bodyTextBold" runat="server" Text="User Confirmation" ForeColor="White" />
            </div>
        </asp:Panel>
        <br /> 
        <div style="text-align: left;"  class="tablepad">
            <div class="row">
                <div class="col-sm-12 text-left" style="margin-left:20%">
                    <span class="formLabel">There are unsaved changes, do you want to continue...</span>
                </div>
            </div>
        </div>
        <div class="btnBox" style="margin-right:30%;margin-top:5%" >
                <asp:Button ID="Btn_Yes" runat="server" Text="Ok" CssClass="buttonBoxFocus" OnClick="Btn_Yes_Click"  />
                <asp:Button ID="Btn_No" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="Btn_No_Click" />
        </div>
    </asp:Panel>
    <asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy"/>

  </div>
     <asp:HiddenField ID="hdnRegID" runat="server" />
     <asp:HiddenField ID="hdnProvAdminUserID" runat="server" />
</asp:Content>
