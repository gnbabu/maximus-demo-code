<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_OwnerTransaction" Codebehind="OwnerTransaction.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<style>
	.formLabel200 {
		right: -8px;
		position: absolute;
	}
</style>

<asp:UpdatePanel ID="upSSN" runat="server" UpdateMode="Conditional">
	<ContentTemplate>
		<div>
			<asp:ValidationSummary ID="vsOwnerTransaction" runat="server" DisplayMode="List" ValidationGroup="valOwnerTransaction" />
		</div>
		<div>
			<asp:UpdateProgress ID="updateProgress" runat="server">
				<ProgressTemplate>
					<div>
						<img src="../Images/ajax-loader.gif" alt="AJAX Loader" />
					</div>
				</ProgressTemplate>
			</asp:UpdateProgress>
		</div>
		<div id="divOwnerTransaction" runat="server">
			<div class="row">
				<div class="col-sm-3 text-right"><span class="formLabel200">Person or Entity*</span></div>
				<div class="col-sm-9">
					<asp:DropDownList ID="ddlOwner" runat="server" CssClass="formDropDown" OnSelectedIndexChanged="ddlOwner_SelectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="True" />
					<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="valOwnerTransaction"
						ControlToValidate="ddlOwner" ErrorMessage="*Select an Owner" Text="*" Display="Dynamic"
						SetFocusOnError="true" InitialValue="" />
				</div>
				<div class="pdmsLabel" style="display: none;">
					<asp:Label ID="lblPDMSPersonEntityNo" runat="server" />
				</div>
			</div>

			<div class="row">
				<div class="col-sm-3 text-right"><span class="formLabel200">Amount of Transaction*</span></div>
				<div class="col-sm-9">
					<ew:NumericBox ID="nbAmount" runat="server" CssClass="formField"></ew:NumericBox>
				</div>
				<div class="col-sm-9 text-right">
					<asp:RegularExpressionValidator ID="AmountValidator" runat="server" ErrorMessage="Only Decimals With Precision Less Than 2" ControlToValidate="nbAmount"
						ValidationExpression="^\d+(\.\d{1,2})?$" ValidationGroup="valOwnerTransaction" Display="Dynamic"></asp:RegularExpressionValidator>
					<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" ValidationGroup="valOwnerTransaction" Text="*"
						ControlToValidate="nbAmount" ErrorMessage="Enter Amount of Transaction" Display="Dynamic" />
				</div>
				<div class="pdmsLabel" style="display: none;">
					<asp:Label ID="lblPDMSAmount" runat="server" />

				</div>
			</div>
			<div class="row">
				<div class="col-sm-3 text-right"><span class="formLabel200">Date of Transaction*</span></div>
				<div class="col-sm-9">
					<asp:TextBox ID="txtCMPDate" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="calStart" TargetControlID="txtCMPDate" runat="server" />
					<asp:CompareValidator ID="dateValidator" runat="server" ValidationGroup="valOwnerTransaction"
						Type="Date" Operator="DataTypeCheck" ControlToValidate="txtCMPDate"
						ErrorMessage="Select a valid Transaction Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
						SetFocusOnError="true"> 
					</asp:CompareValidator>
					<asp:RequiredFieldValidator ID="rfvCMPDate" runat="server" SetFocusOnError="true" ValidationGroup="valOwnerTransaction" Text="*"
						ControlToValidate="txtCMPDate" ErrorMessage="Enter Date of Transaction" Display="Dynamic" />
				</div>
				<div class="pdmsLabel" style="display: none;">
					<asp:Label ID="lblPDMSCMPDate" runat="server" />
				</div>
			</div>

		</div>
	</ContentTemplate>
	<Triggers>
		<asp:AsyncPostBackTrigger ControlID="ddlOwner" EventName="SelectedIndexChanged" />
	</Triggers>
</asp:UpdatePanel>

<asp:HiddenField ID="hdnRegOwnerTransactionID" runat="server" />
