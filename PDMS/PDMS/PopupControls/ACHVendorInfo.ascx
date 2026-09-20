<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ACHVendorInfo" Codebehind="ACHVendorInfo.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<div><asp:ValidationSummary ID="vsVendorInfo" runat="server" DisplayMode="List" ValidationGroup="valVendorInfo" /></div>
<table id="ParentTable" runat="server">
    <colgroup>
        <col width="50%" />
        <col width="50%" />
    </colgroup>
    <tr>
        <td colspan="2">&nbsp;</td>
    </tr>
    <tr>
        <td><span class="formLabel200">Vendor Number*</span></td>
        <td><asp:TextBox ID="txtVendorNumber" runat="server" CssClass="formField" MaxLength="10" />
            <asp:RequiredFieldValidator runat="server" ID="reqVendorNumber"
                ControlToValidate="txtVendorNumber" ErrorMessage="*Enter Vendor Number" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valVendorInfo" />               
        </td>
    </tr>
    <tr>
        <td><span class="formLabel200">Location Code*</span></td>
        <td><asp:TextBox ID="txtLocationCode" runat="server" CssClass="formField" MaxLength="10" />
            <asp:RequiredFieldValidator runat="server" ID="reqLocationCode"
                ControlToValidate="txtLocationCode" ErrorMessage="*Enter Location Code" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valVendorInfo" />               
        </td>
    </tr>
    <tr>
        <td><span class="formLabel200">Sequence Number*</span></td>
        <td><ew:NumericBox ID="nbSequenceNumber" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="3" CssClass="formField" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1"
                ControlToValidate="nbSequenceNumber" ErrorMessage="*Enter Sequence Number" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valVendorInfo" />               
        </td>
    </tr>
</table>

<asp:HiddenField ID="hdnRegVendorInfoID" runat="server" />