<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ProviderInfoHeader, App_Web_p4ixifjm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc" %>

<div class="UserHeader">
    <table>
        <tr>
            <td><asp:Label runat="server"  CssClass="formLabel300" Text="Provider Name" /></td>
            <td><asp:Label ID="LC125" runat="server" CssClass="formData" /></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label1" runat="server"  CssClass="formLabel300" Text="Medicaid ID" /></td>
            <td><asp:Label ID="LC126" runat="server" CssClass="formData" /></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label2" runat="server"  CssClass="formLabel300" Text="<%$ Resources:BrandingResource , brandStatus %>" /></td>
            <td><asp:Label ID="LC127" runat="server" CssClass="formData" /></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label3" runat="server"  CssClass="formLabel300" Text="PDMS Status" /></td>
            <td><asp:Label ID="LC128" runat="server" CssClass="formData" /></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label4" runat="server"  CssClass="formLabel300" Text="PDMS Status Date" /></td>
            <td><asp:Label ID="LC129" runat="server" CssClass="formData" /></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label5" runat="server"  CssClass="formLabel300" Text="View" /></td>
            <td><asp:Label ID="LC130" runat="server" CssClass="formData" /></td>
        </tr>
        <tr>
            <td><asp:Label ID="lblAssigndTo" runat="server"  CssClass="formLabel300" Text="Assigned To" /></td>
            <td><asp:DropDownList ID="ddlAssignedTo" AutoPostBack="false"  runat="server"></asp:DropDownList></td>
        </tr>
    </table>
    <table>
        <tr>
            <asp:Panel ID="pnlScreenErrors" runat="server" style="border:solid 5px red">
                <asp:ValidationSummary ID="vsProviderInfoHeader" runat="server" DisplayMode="List" />
            </asp:Panel>
        </tr>
        <tr><asp:Panel ID="pnlReturnReasons" runat="server" /></tr>
    </table>
</div>
<asp:TextBox ID="txtRegistrationPageType" runat="server" Visible="false" />
