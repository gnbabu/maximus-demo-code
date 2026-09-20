<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_OwnerSubcontractorOwner, App_Web_c4une0e1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<div><asp:ValidationSummary ID="vsSubcontractorOwner" runat="server" DisplayMode="List" ValidationGroup="valSubcontractorOwner" /></div>
<table id="ParentTable" runat="server">
    <colgroup>
        <col width="33%" />
        <col width="33%" />
        <col width="33%" />
    </colgroup>
    <tr>
        <td>&nbsp;</td>
        <td><h3>Provider</h3></td>
        <td><h3>PDMS</h3></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Subcontractor*</span></td>
        <td><asp:DropDownList ID="ddlSubcontractor" runat="server" CssClass="formDropDown" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="valSubcontractorOwner"
                ControlToValidate="ddlSubcontractor" ErrorMessage="*Select an Owner" Text="*" Display="Dynamic" 
                SetFocusOnError="true" InitialValue="" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSSubcontractor" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Name*</span></td>
        <td><asp:TextBox ID="txtName" runat="server" CssClass="formField" MaxLength="100" />
            <asp:RequiredFieldValidator runat="server" ID="reqName"
                ControlToValidate="txtName" ErrorMessage="*Enter Name" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valSubcontractorOwner" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSName" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Birth Date</span></td>
        <td><asp:TextBox ID="txtBirthDate" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="calStart" TargetControlID="txtBirthDate" runat="server" />
            <asp:CompareValidator id="dateValidator" runat="server" ValidationGroup="valSubcontractorOwner"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtBirthDate"  
                ErrorMessage="Select a valid Birth Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true"> 
            </asp:CompareValidator>
            <asp:CompareValidator id="cmpValBirthDateFuture" ControlToValidate="txtBirthDate" Operator="LessThan" Type="Date"
                runat="server" ErrorMessage="Birth Date cannot be future date" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valSubcontractorOwner" />                
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSRequestedEffectiveDate" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">SSN or Tax ID*</span></td>
        <td><ew:NumericBox ID="nbTaxID" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="9" CssClass="formField" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="nbTaxID" 
                ValidationExpression="^\d{9}$" ErrorMessage="Enter a 9 digit Tax ID" Text="*" Display="Dynamic" ValidationGroup="valSubcontractorOwner" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3"
                ControlToValidate="nbTaxID" ErrorMessage="*Enter SSN or Tax ID" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valSubcontractorOwner" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSTaxID" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Percentage of Ownership</span></td>
        <td><ew:NumericBox ID="nbPercent" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="3" CssClass="formField" />
            <asp:CompareValidator id="cmpValPercentage" ControlToValidate="nbPercent" Operator="LessThanEqual" Type="Integer" ValueToCompare="100"
                runat="server" ErrorMessage="Percentage cannot be greater than 100" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valSubcontractorOwner" />                
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSPercentage" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Title</span></td>
        <td><asp:TextBox ID="txtTitle" runat="server" CssClass="formField" MaxLength="80" /></td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSTitle" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Address*</span></td>
        <td><asp:TextBox ID="txtAddress1" runat="server" CssClass="formField" MaxLength="100" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5"
                ControlToValidate="txtAddress1" ErrorMessage="*Enter the Address" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valSubcontractorOwner" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSStreet1" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Address 2</span></td>
        <td><asp:TextBox ID="txtAddress2" runat="server" CssClass="formField" MaxLength="100" /></td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSStreet2" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">City*</span></td>
        <td><asp:TextBox ID="txtCity" runat="server" CssClass="formField" MaxLength="50" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6"
                ControlToValidate="txtCity" ErrorMessage="*Enter the City" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valSubcontractorOwner" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSCity" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">State*</span></td>
        <td><asp:DropDownList ID="ddlState" runat="server" CssClass="formDropDown" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="valSubcontractorOwner"
                ControlToValidate="ddlState" ErrorMessage="*Select a State" Text="*" Display="Dynamic" 
                SetFocusOnError="true" InitialValue="" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSState" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Zip*</span></td>
        <td><ew:NumericBox ID="nbZipFirst5" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="5" CssClass="formField" />                
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ValidationGroup="valSubcontractorOwner"
                ControlToValidate="nbZipFirst5" ErrorMessage="Enter Zip (First 5 digits)" Text="*" Display="Dynamic" 
                SetFocusOnError="true" />               
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="nbZipFirst5" ValidationExpression="^\d{5}$" 
                ErrorMessage="Enter 5 digits for the Zip (First 5)" Text="*" Display="Dynamic" ValidationGroup="valSubcontractorOwner" />
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSZip" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Ext Zip</span></td>
        <td><ew:NumericBox ID="nbZipLast4" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="4" CssClass="formField" />                
            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="nbZipLast4" 
                ValidationExpression="^\d{4}$" ErrorMessage="Enter 4 digits for the Zip (Last 4)" Text="*" 
                Display="Dynamic" ValidationGroup="valSubcontractorOwner" />
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSExtZip" runat="server" /></div></td>
    </tr>
</table>

<asp:HiddenField ID="hdnRegSubcontractorOwnerID" runat="server" />