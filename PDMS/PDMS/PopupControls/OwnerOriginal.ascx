<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_OwnerOriginal" Codebehind="OwnerOriginal.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<div><asp:ValidationSummary ID="vsOwnerOriginal" runat="server" DisplayMode="List" ValidationGroup="valOwnerOriginal" /></div>
<table id="ParentTable" runat="server">
    <colgroup>
        <col width="33%" />
        <col width="33%" />
        <col width="33%" />
    </colgroup>
    <tr>
        <td>&nbsp;</td>
        <td></td>
        <td></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Name of Original Owner*</span></td>
        <td><asp:TextBox ID="txtOwnerName" runat="server" CssClass="formField" MaxLength="80" />
            <asp:RequiredFieldValidator runat="server" ID="reqOwnerName"
                ControlToValidate="txtOwnerName" ErrorMessage="*Enter Name of Original Owner" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valOwnerOriginal" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSOwnerName" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">SSN/Tax ID of Original Owner*</span></td>
        <td><ew:NumericBox ID="nbTaxID" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="9" CssClass="formField" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="nbTaxID" 
                ValidationExpression="^\d{9}$" ErrorMessage="Please enter 9 digit SSN or Tax Id" Text="*" Display="Dynamic" ValidationGroup="valOwnerOriginal" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1"
                ControlToValidate="nbTaxID" ErrorMessage="*Enter SSN/Tax ID of Original Owner" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valOwnerOriginal" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSTaxID" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Place of Transfer</span></td>
        <td><asp:TextBox ID="txtTransferPlace" runat="server" CssClass="formField" MaxLength="80" /></td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSTransferPlace" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Date of Transfer*</span></td>
        <td><asp:TextBox ID="txtTransferDate" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="calStart" TargetControlID="txtTransferDate" runat="server" />
            <asp:CompareValidator id="dateValidator" runat="server" ValidationGroup="valOwnerOriginal"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtTransferDate"  
                ErrorMessage="Select a valid Date of Transfer" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true"> 
            </asp:CompareValidator>
            <asp:RequiredFieldValidator ID="rfvTransferDate" runat="server" SetFocusOnError="true" ValidationGroup="valOwnerOriginal" Text="*"
                ControlToValidate="txtTransferDate" ErrorMessage="Enter Date of Transfer" Display="Dynamic" />
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSTransferDate" runat="server" /></div></td>
    </tr>
</table>

<asp:HiddenField ID="hdnRegOriginalOwnerID" runat="server" />