<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_OwnerSupplier, App_Web_l5y5araq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<div><asp:ValidationSummary ID="vsOwnerSupplier" runat="server" DisplayMode="List" ValidationGroup="valOwnerSupplier" /></div>
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
        <td><span class="formLabel200">Name of Supplier*</span></td>
        <td><asp:TextBox ID="txtName" runat="server" CssClass="formField" MaxLength="100" />
            <asp:RequiredFieldValidator runat="server" ID="reqName"
                ControlToValidate="txtName" ErrorMessage="*Enter Name of Supplier" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valOwnerSupplier" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSName" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Address*</span></td>
        <td><asp:TextBox ID="txtAddress1" runat="server" CssClass="formField" MaxLength="100" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5"
                ControlToValidate="txtAddress1" ErrorMessage="*Enter the Address" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valOwnerSupplier" />               
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
                SetFocusOnError="true" ValidationGroup="valOwnerSupplier" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSCity" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">State*</span></td>
        <td><asp:DropDownList ID="ddlState" runat="server" CssClass="formDropDown" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="valOwnerSupplier"
                ControlToValidate="ddlState" ErrorMessage="*Select a State" Text="*" Display="Dynamic" 
                SetFocusOnError="true" InitialValue="" />               
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSState" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Zip*</span></td>
        <td><ew:NumericBox ID="nbZipFirst5" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="5" CssClass="formField" />                
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ValidationGroup="valOwnerSupplier"
                ControlToValidate="nbZipFirst5" ErrorMessage="Enter Zip (First 5 digits)" Text="*" Display="Dynamic" 
                SetFocusOnError="true" />               
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="nbZipFirst5" ValidationExpression="^\d{5}$" 
                ErrorMessage="Enter 5 digits for the Zip (First 5)" Text="*" Display="Dynamic" ValidationGroup="valOwnerSupplier" />
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSZip" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Ext Zip</span></td>
        <td><ew:NumericBox ID="nbZipLast4" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="4" CssClass="formField" />                
            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="nbZipLast4" 
                ValidationExpression="^\d{4}$" ErrorMessage="Enter 4 digits for the Zip (Last 4)" Text="*" 
                Display="Dynamic" ValidationGroup="valOwnerSupplier" />
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSExtZip" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">NPI*</span></td>
        <td><ew:NumericBox ID="nbNPI" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="10" CssClass="formField" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="valOwnerSupplier"
                ControlToValidate="nbNPI" ErrorMessage="Enter NPI" Text="*" Display="Dynamic" 
                SetFocusOnError="true" />               
            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="nbNPI" 
                ValidationExpression="^\d{10}$" ErrorMessage="Enter a 10 digit NPI" Text="*" Display="Dynamic" ValidationGroup="valOwnerSupplier" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="nbNPI" 
                ValidationExpression="^[1-9][0-9]+$" ErrorMessage="NPI requires 10 digits and cannot start with 0" Text="*" Display="Dynamic" ValidationGroup="valOwnerSupplier" />
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSNPI" runat="server" /></div></td>
    </tr>
    <tr>
        <td><span class="formLabel200">Tax ID*</span></td>
        <td><ew:NumericBox ID="nbTaxID" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="9" CssClass="formField" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="valOwnerSupplier"
                ControlToValidate="nbTaxID" ErrorMessage="Enter Tax ID" Text="*" Display="Dynamic" 
                SetFocusOnError="true" />               
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="nbTaxID" 
                ValidationExpression="^\d{9}$" ErrorMessage="Enter a 9 digit Tax ID" Text="*" Display="Dynamic" ValidationGroup="valOwnerSupplier" />
        </td>
        <td><div class="pdmsLabel"><asp:Label ID="lblPDMSTaxID" runat="server" /></div></td>
    </tr>
</table>

<asp:HiddenField ID="hdnRegSupplierID" runat="server" />