<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ACHReliaCardAuthorization, App_Web_c4une0e1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<script type="text/javascript">
    function NumberOnly() {
        var AsciiValue = event.keyCode
        if ((AsciiValue >= 48 && AsciiValue <= 57) || (AsciiValue == 8 || AsciiValue == 127))
            event.returnValue = true;
        else
            event.returnValue = false;
    }
    </script>
<div><asp:ValidationSummary ID="vsReliaCardInfo" runat="server" DisplayMode="List" ValidationGroup="valReliaCard" /></div>
<table id="ParentTable" runat="server">
    <colgroup>
        <col width="33%" />
        <col width="33%" />
    </colgroup>
    <tr>
        <td>&nbsp;</td>
        <td><h3>Provider</h3></td>
    </tr>
    <tr>
        <td><span class="formLabel200">First Name*</span></td>
        <td><asp:TextBox ID="txtFirstName" runat="server" CssClass="formField" MaxLength="80" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3"
                    ControlToValidate="txtFirstName" ErrorMessage="*Enter First Name" Text="*" Display="Dynamic" 
                    SetFocusOnError="true" ValidationGroup="valReliaCard" />                 
        </td>
    </tr>
    <tr>
        <td><span class="formLabel200">Middle Initial</span></td>
        <td><asp:TextBox ID="txtMiddleInitial" runat="server" CssClass="formField" MaxLength="80" />
                           
        </td>
    </tr>
    <tr>
        <td><span class="formLabel200">Last Name*</span></td>
        <td><asp:TextBox ID="txtLastName" runat="server" CssClass="formField" MaxLength="80" />
             <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1"
                    ControlToValidate="txtLastName" ErrorMessage="*Enter Last Name" Text="*" Display="Dynamic" 
                    SetFocusOnError="true" ValidationGroup="valReliaCard" />              
        </td>
    </tr>
    <tr>
        <td><span class="formLabel200">Street*</span></td>
        <td><asp:TextBox ID="txtStreet" runat="server" CssClass="formField" MaxLength="100" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6"
                    ControlToValidate="txtStreet" ErrorMessage="*Enter Street" Text="*" Display="Dynamic" 
                    SetFocusOnError="true" ValidationGroup="valReliaCard" /> 
        </td>
    </tr>
    <tr>
        <td><span class="formLabel200">City*</span></td>
        <td><asp:TextBox ID="txtCity" runat="server" CssClass="formField" MaxLength="50" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2"
                    ControlToValidate="txtCity" ErrorMessage="*Enter City" Text="*" Display="Dynamic" 
                    SetFocusOnError="true" ValidationGroup="valReliaCard" />                 
        </td>
    </tr>
    <tr>
        <td><span class="formLabel200">State*</span></td>
        <td><asp:DropDownList ID="ddlState" runat="server" CssClass="formDropDown" AppendDataBoundItems="True"/>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4"
                    ControlToValidate="ddlState" ErrorMessage="*Enter State" Text="*" Display="Dynamic" 
                    SetFocusOnError="true" ValidationGroup="valReliaCard" />               
        </td>
    </tr>
    <tr>
        <td><span class="formLabel200">Zip Code/Postal Code*</span></td>
        <td><asp:TextBox ID="txtZipCode" runat="server" CssClass="formField" MaxLength="5"  CausesValidation="true" onkeypress="return NumberOnly()"/>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5"
                    ControlToValidate="txtZipCode" ErrorMessage="*Enter Zip Code" Text="*" Display="Dynamic" 
                    SetFocusOnError="true" ValidationGroup="valReliaCard" />
            <asp:RegularExpressionValidator runat="server" ID="regexZip" ControlToValidate="txtZipCode" ValidationExpression="(?!0{5})(?!9{5})\d{5}" ErrorMessage="* Zip must be 5 digits" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valReliaCard" />               
        </td>
    </tr>
</table>
<asp:HiddenField ID="hdnRegACHReliaCardID" runat="server" />