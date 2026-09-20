<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_FeeInformation" Codebehind="FeeInformation.ascx.cs" %>
<%@ Register Src="~/UserControls/FormField.ascx" TagPrefix="uc" TagName="FormField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<div>
    <asp:ValidationSummary ID="vsFeeInformation" DisplayMode="List" runat="server" ValidationGroup="valFeeInformation" />
</div>
<table id="ParentTable" runat="server">
    <tr>
        <td> <asp:Label ID="lblDepositDate" runat="server" Text="Deposit Date*" CssClass="formLabel" />
        </td>
        <td>
            <asp:TextBox ID="txtPaymentDate" runat="server" CssClass="formField"  />
            <ajax:CalendarExtender ID="calPayment" runat="server" TargetControlID="txtPaymentDate"></ajax:CalendarExtender>
            <asp:RequiredFieldValidator runat="server" ID="valPaymentDateReq" ControlToValidate="txtPaymentDate" ErrorMessage="* Payment Received Date is required." Text="*" SetFocusOnError="true" ValidationGroup="valFeeInformation" />
            <asp:CompareValidator ID="cvFutureDate" ControlToValidate="txtPaymentDate" Operator="LessThan" Type="Date"
                runat="server" ErrorMessage="Payment recieved date cannot be future date" Text="*" ValidationGroup="valFeeInformation"
                SetFocusOnError="true" ValueToCompare="MM/dd/yyyy" />
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="lblDepositID" runat="server" Text="Deposit ID*" CssClass="formLabel" /></td>
        <td>
            <asp:TextBox ID="txtDepositID" runat="server" CssClass="formField" MaxLength="15" />
            <asp:RequiredFieldValidator runat="server" ID="valDepositIdReq" ControlToValidate="txtDepositID" ErrorMessage="* Deposit ID is required." Text="*" SetFocusOnError="true" ValidationGroup="valFeeInformation" />
        </td>
    </tr>
    <tr>
        <td> <asp:Label ID="lblPaymentID" runat="server" Text="Payment ID" CssClass="formLabel" /></td>
        <td> <asp:TextBox ID="txtPaymentID" runat="server" CssClass="formField" MaxLength="15" /> </td>
    </tr>
</table>
