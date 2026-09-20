<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_OwnerDebarred" Codebehind="OwnerDebarred.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<div><asp:ValidationSummary ID="vsOwnerDebarred" runat="server" DisplayMode="List" ValidationGroup="valOwnerDebarred" /></div>
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
        <td><span class="formLabel200">Person or Entity*</span></td>
        <td><asp:DropDownList ID="ddlOwner" runat="server" CssClass="formDropDown" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="valOwnerDebarred"
                ControlToValidate="ddlOwner" ErrorMessage="*Select an Owner" Text="*" Display="Dynamic" 
                SetFocusOnError="true" InitialValue="" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSPersonEntityNo" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Debarment Date*</span></td>
        <td><asp:TextBox ID="txtDebarmentDate" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="calStart" TargetControlID="txtDebarmentDate" runat="server" />
            <asp:CompareValidator id="dateValidator" runat="server" ValidationGroup="valOwnerDebarred"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDebarmentDate"  
                ErrorMessage="Select a valid Debarment Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true"> 
            </asp:CompareValidator>
            <asp:RequiredFieldValidator ID="rfvDebarmentDate" runat="server" SetFocusOnError="true" ValidationGroup="valOwnerDebarred" Text="*"
                ControlToValidate="txtDebarmentDate" ErrorMessage="Enter Debarment Date" Display="Dynamic" />
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSDebarmentDate" runat="server" /></div></td>
    </tr>

    <tr>
        <td><span class="formLabel200">Length of Debarment*</span></td>
        <td><asp:TextBox ID="txtDebarmentDuration" runat="server" CssClass="formField" MaxLength="100" />
            <asp:RequiredFieldValidator runat="server" ID="reqDebarmentDuration"
                ControlToValidate="txtDebarmentDuration" ErrorMessage="*Enter Length of Debarment" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valOwnerDebarred" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSDebarmentDuration" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Reason for Debarment*</span></td>
        <td><asp:TextBox ID="txtDebarmentReason" runat="server" CssClass="formField" MaxLength="100" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5"
                ControlToValidate="txtDebarmentReason" ErrorMessage="*Enter the Reason for Debarment" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valOwnerDebarred" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSDebarmentReason" runat="server" /></div></td>
    </tr>
</table>

<asp:HiddenField ID="hdnRegOwnerDebarredID" runat="server" />