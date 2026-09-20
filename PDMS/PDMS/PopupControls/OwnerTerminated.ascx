<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_OwnerTerminated" Codebehind="OwnerTerminated.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<div><asp:ValidationSummary ID="vsOwnerTerminated" runat="server" DisplayMode="List" ValidationGroup="valOwnerTerminated" /></div>
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
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="valOwnerTerminated"
                ControlToValidate="ddlOwner" ErrorMessage="*Select an Owner" Text="*" Display="Dynamic" 
                SetFocusOnError="true" InitialValue="" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSPersonEntityNo" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">State where practicing when Terminated*</span></td>
        <td><asp:DropDownList ID="ddlState" runat="server" CssClass="formDropDown" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="valOwnerTerminated"
                ControlToValidate="ddlState" ErrorMessage="*Select a State" Text="*" Display="Dynamic" 
                SetFocusOnError="true" InitialValue="" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSState" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Reason for Termination*</span></td>
        <td><asp:TextBox ID="txtTerminationReason" runat="server" CssClass="formField" MaxLength="255" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3"
                ControlToValidate="txtTerminationReason" ErrorMessage="*Enter the Reason for Termination" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valOwnerTerminated" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSTerminationReason" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Termination Begin Date*</span></td>
        <td><asp:TextBox ID="txtTerminationBeginDate" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="calStart" TargetControlID="txtTerminationBeginDate" runat="server" />
            <asp:CompareValidator id="dateValidator" runat="server" ValidationGroup="valOwnerTerminated"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtTerminationBeginDate"  
                ErrorMessage="Select a valid Termination Begin Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true"> 
            </asp:CompareValidator>
            <asp:RequiredFieldValidator ID="rfvTerminationBeginDate" runat="server" SetFocusOnError="true" ValidationGroup="valOwnerTerminated" Text="*"
                ControlToValidate="txtTerminationBeginDate" ErrorMessage="Enter Termination Begin Date" Display="Dynamic" />
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSTerminationBeginDate" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Termination End Date*</span></td>
        <td><asp:TextBox ID="txtTerminationEndDate" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtTerminationEndDate" runat="server" />
            <asp:CompareValidator id="CompareValidator1" runat="server" ValidationGroup="valOwnerTerminated"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtTerminationEndDate"  
                ErrorMessage="Select a valid Termination End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true"> 
            </asp:CompareValidator>
            <asp:RequiredFieldValidator ID="rfvTerminationEndDate" runat="server" SetFocusOnError="true" ValidationGroup="valOwnerTerminated" Text="*"
                ControlToValidate="txtTerminationEndDate" ErrorMessage="Enter Termination End Date" Display="Dynamic" />
            <asp:CompareValidator id="cmpValStartEndDates" ControlToValidate="txtTerminationBeginDate" ControlToCompare="txtTerminationEndDate" Operator="LessThanEqual" Type="Date"
                runat="server" ErrorMessage="Termination Begin Date cannot be greater than End Date" Text="*" Display="Dynamic" CultureInvariantValues="false"  
                SetFocusOnError="true" ValidationGroup="valOwnerTerminated" />
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSTerminationEndDate" runat="server" /></div></td>
    </tr>
</table>

<asp:HiddenField ID="hdnRegOwnerTerminatedID" runat="server" />