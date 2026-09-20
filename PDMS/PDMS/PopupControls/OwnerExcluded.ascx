<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_OwnerExcluded" Codebehind="OwnerExcluded.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<div><asp:ValidationSummary ID="vsOwnerExcluded" runat="server" DisplayMode="List" ValidationGroup="valOwnerExcluded" /></div>
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
        <td><span class="formLabel300">Person or Entity*</span></td>
        <td><asp:DropDownList ID="ddlOwner" runat="server" CssClass="formDropDown" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="valOwnerExcluded"
                ControlToValidate="ddlOwner" ErrorMessage="*Select an Owner" Text="*" Display="Dynamic" 
                SetFocusOnError="true" InitialValue="" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSPersonEntityNo" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel300">Exclusion Begin Date*</span></td>
        <td><asp:TextBox ID="txtExclusionBeginDate" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="calStart" TargetControlID="txtExclusionBeginDate" runat="server" />
            <asp:CompareValidator id="dateValidator" runat="server" ValidationGroup="valOwnerExcluded"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtExclusionBeginDate"  
                ErrorMessage="Select a valid Exclusion Begin Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true"> 
            </asp:CompareValidator>
            <asp:RequiredFieldValidator ID="rfvExclusionBeginDate" runat="server" SetFocusOnError="true" ValidationGroup="valOwnerExcluded" Text="*"
                ControlToValidate="txtExclusionBeginDate" ErrorMessage="Enter Exclusion Begin Date" Display="Dynamic" />
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSExclusionBeginDate" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel300">Exclusion End Date*</span></td>
        <td><asp:TextBox ID="txtExclusionEndDate" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtExclusionEndDate" runat="server" />
            <asp:CompareValidator id="CompareValidator1" runat="server" ValidationGroup="valOwnerExcluded"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtExclusionEndDate"  
                ErrorMessage="Select a valid Exclusion End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true"> 
            </asp:CompareValidator>
            <asp:RequiredFieldValidator ID="rfvExclusionEndDate" runat="server" SetFocusOnError="true" ValidationGroup="valOwnerExcluded" Text="*"
                ControlToValidate="txtExclusionEndDate" ErrorMessage="Enter Exclusion End Date" Display="Dynamic" />
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSExclusionEndDate" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel300">Reason for Exclusion/Termination*</span></td>
        <td><asp:TextBox ID="txtExclusionReason" runat="server" CssClass="formField" MaxLength="255" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5"
                ControlToValidate="txtExclusionReason" ErrorMessage="*Enter the Reason for Exclusion" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valOwnerExcluded" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSExclusionReason" runat="server" /></div></td>
    </tr>
</table>

<asp:HiddenField ID="hdnRegOwnerExcludedID" runat="server" />
