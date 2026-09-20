<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_Pharmacy" Codebehind="Pharmacy.ascx.cs" %>
<%@ Register Src="~/UserControls/FormField.ascx" TagPrefix="uc" TagName="FormField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<script type="text/javascript">
    
    function NumberOnly() {
        var AsciiValue = event.keyCode
        if ((AsciiValue >= 48 && AsciiValue <= 57) || (AsciiValue == 8 || AsciiValue == 127))
            event.returnValue = true;
        else
            event.returnValue = false;
    }
    
</script>
<style type="text/css">
    .pdmsLabel
    {
        width:120px;
    }
    table
    {
        text-align:left!important;
    }
</style>
<asp:ValidationSummary ID="MiscValidationSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="Pharmacy" />
<table id="ParentTable" runat="server">
    <tr>
        <td>
            <asp:Label ID="Label4" runat="server" Text="NCPDP Number*" CssClass="formLabel wd200" />
        </td>
        <td>
            <asp:TextBox ID="txtNCPDPNumber" runat="server" CssClass="formField wd100" MaxLength="7" MinLength="7" CausesValidation="true" onkeypress="return NumberOnly()"/>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="txtNCPDPNumber" ErrorMessage="* NCPDP Number is required." Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="Pharmacy" />
            <asp:RegularExpressionValidator ID="valNCPDPFormat" runat="server" Text="*" ErrorMessage="* Enter 7 digits for NCPDP number" ControlToValidate="txtNCPDPNumber" SetFocusOnError="true"
                                                    Display="Dynamic" ValidationExpression="\d{7}$" ValidationGroup="Register" />
        </td>
        <td>
            <div class="pdmsLabel"><asp:Label ID="pdms_txtNCPDPNumber" runat="server" Text="" /></div>
        </td>
    </tr>
        <tr>
        <td>
            <asp:Label ID="Label6" runat="server" Text="NCPDP Start Date*" CssClass="formLabel wd200" />
        </td>
        <td>
            <asp:TextBox ID="txtNCPDPStartDate" runat="server" CssClass="formField wd100" /><ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtNCPDPStartDate" runat="server" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtNCPDPStartDate" ErrorMessage="* NCPDP Start Date is required." Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="Pharmacy" />
            <asp:CompareValidator id="dateValidator" runat="server" ValidationGroup="Pharmacy"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtNCPDPStartDate"  
                ErrorMessage="Select a valid NCPDP Start Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true" />
            <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtNCPDPStartDate" OnServerValidate="NCPDPStart_Future" Display="Static" ValidationGroup="Pharmacy" ErrorMessage="* Future dates not allowed." Text="*" />
            <asp:CustomValidator ID="CustomValidator3" runat="server" ControlToValidate="txtNCPDPStartDate" OnServerValidate="NCPDPStart_Less" Display="Static" ValidationGroup="Pharmacy" ErrorMessage="* Invalid date span." Text="*" />
            <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtNCPDPStartDate" OnServerValidate="NCPDPStart_Valid" Display="Static" ValidationGroup="Pharmacy" ErrorMessage="* NCPDP start date cannot be before 1750." Text="*" />
            <asp:CustomValidator ID="CustomValidator4" runat="server" ControlToValidate="txtNCPDPStartDate" OnServerValidate="NCPDPStartRequired" Display="Static" ValidationGroup="Pharmacy" ErrorMessage="* Issue date is required." Text="*" />
        </td>
        <td>
            <%-- BZ #4225 (PSR) - Setting a label used for test to an empty string, as it likely should be assigned text in the code.                                                                                         --%>
            <%--<div class="pdmsLabel"><asp:Label ID="pdms_Start" runat="server" Text="1/1/2013" /></div>--%>
            <div class="pdmsLabel"><asp:Label ID="pdms_txtNCPDPStartDate" runat="server" Text="" /></div>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label8" runat="server" Text="NCPDP End Date" CssClass="formLabel wd200" />
        </td>
        <td>
            <asp:TextBox ID="txtNCPDPEndDate" runat="server" CssClass="formField wd100" /><ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="txtNCPDPEndDate" runat="server" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtNCPDPEndDate" ErrorMessage="* NCPDP End Date is required." Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="Pharmacy" />
            <asp:CompareValidator id="CompareValidator1" runat="server" ValidationGroup="Pharmacy"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtNCPDPEndDate"  
                ErrorMessage="Select a valid NCPDP End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true" />
            <asp:CustomValidator ID="CustomValidator6" runat="server" ControlToValidate="txtNCPDPEndDate" OnServerValidate="ValidateLC_Greater" Display="Static" ValidationGroup="Pharmacy" ErrorMessage="* Invalid date span." Text="*" />
            <asp:CustomValidator ID="CustomValidator10" runat="server" ControlToValidate="txtNCPDPEndDate" OnServerValidate="NCPDPEnd_Valid" Display="Static" ValidationGroup="Pharmacy" ErrorMessage="* NCPDP end date cannot be after 2150." Text="*" />
        </td>
        <td>
            <%-- BZ #4225 (PSR) - Setting a label used for test to an empty string, as it likely should be assigned text in the code.                                                                                         --%>
            <%--<div class="pdmsLabel"><asp:Label ID="pdms_Start" runat="server" Text="1/1/2013" /></div>--%>
            <div class="pdmsLabel"><asp:Label ID="pdms_txtNCPDPEndDate" runat="server" Text="" /></div>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label10" runat="server" Text="Rebate Exemption Start Date" CssClass="formLabel wd200" />
        </td>
        <td>
            <asp:TextBox ID="txtRebateExemptionStartDate" runat="server" CssClass="formField wd100" /><ajax:CalendarExtender ID="CalendarExtender3" TargetControlID="txtRebateExemptionStartDate" runat="server" />
            <asp:CompareValidator id="CompareValidator2" runat="server" ValidationGroup="Pharmacy"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtRebateExemptionStartDate"  
                ErrorMessage="Select a valid Rebate Exemption Start Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true" />
            <asp:CustomValidator ID="CustomValidator5" runat="server" ControlToValidate="txtRebateExemptionStartDate" OnServerValidate="RebateExemptionStart_Future" Display="Static" ValidationGroup="Pharmacy" ErrorMessage="* Future dates not allowed." Text="*" />
            <asp:CustomValidator ID="CustomValidator7" runat="server" ControlToValidate="txtRebateExemptionStartDate" OnServerValidate="RebateExemptionStart_Less" Display="Static" ValidationGroup="Pharmacy" ErrorMessage="* Invalid date span." Text="*" />
            <asp:CustomValidator ID="CustomValidator8" runat="server" ControlToValidate="txtRebateExemptionStartDate" OnServerValidate="RebateExemptionStartRequired" Display="Static" ValidationGroup="Pharmacy" ErrorMessage="* Issue date is required." Text="*" />
        </td>
        <td>
            <%-- BZ #4225 (PSR) - Setting a label used for test to an empty string, as it likely should be assigned text in the code.                                                                                         --%>
            <%--<div class="pdmsLabel"><asp:Label ID="pdms_Start" runat="server" Text="1/1/2013" /></div>--%>
            <div class="pdmsLabel"><asp:Label ID="pdms_txtRebateExemptionStartDate" runat="server" Text="" /></div>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label12" runat="server" Text="Rebate Exemption End Date" CssClass="formLabel wd200" />
        </td>
        <td>
            <asp:TextBox ID="txtRebateExemptionEndDate" runat="server" CssClass="formField wd100" /><ajax:CalendarExtender ID="CalendarExtender4" TargetControlID="txtRebateExemptionEndDate" runat="server" />
            <asp:CompareValidator id="CompareValidator3" runat="server" ValidationGroup="Pharmacy"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtRebateExemptionEndDate"  
                ErrorMessage="Select a valid Rebate Exemption End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true" />
            
            <asp:CustomValidator ID="CustomValidator9" runat="server" ControlToValidate="txtRebateExemptionEndDate" OnServerValidate="RebateExemptionValidateLC_Greater" Display="Static" ValidationGroup="Pharmacy" ErrorMessage="* Invalid date span." Text="*" />
            
        </td>
        <td>
            <%-- BZ #4225 (PSR) - Setting a label used for test to an empty string, as it likely should be assigned text in the code.                                                                                         --%>
            <%--<div class="pdmsLabel"><asp:Label ID="pdms_Start" runat="server" Text="1/1/2013" /></div>--%>
            <div class="pdmsLabel"><asp:Label ID="pdms_txtRebateExemptionEndDate" runat="server" Text="" /></div>
        </td>
    </tr>
    <tr>
        <td><asp:Label ID="Label2" runat="server" Text="340 B Participant" CssClass="formLabel wd200" /></td>
        <td><asp:RadioButtonList ID="B340Participant" runat="server" RepeatDirection="Horizontal">
            <asp:ListItem Value="1">Yes</asp:ListItem>
        <asp:ListItem Value="2">No</asp:ListItem>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="B340Participant" ErrorMessage="* 340 B Participant is required." Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="Pharmacy" /></td>
    </tr>
</table>            
            
            
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
