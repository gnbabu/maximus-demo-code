<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_PrimarySpecialty, App_Web_rqhgepvh" %>
<%@ Register Src="~/UserControls/FormField.ascx" TagPrefix="uc" TagName="FormField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<style type="text/css">
    .formLabel
    {
        width:140px!important;
    }
    input[type='text']
    {
        border:solid 1px black!important;
        width:250px;
    }
    input[type='checkbox']
    {
        float:left;
    }    
    .modalPopup, #ctl00_MainContent_ucLicensesClassifications_ucMessageModal_pnlModal
    {
        height:320px;
        width:785px;
    }
    #labelTable>tbody>tr>td>span
    {
        width:150px;
    }
    #provTable>tbody>tr>td>input[type='text'], #pdmsTable>tbody>tr>td>input[type='text']
    {
        width:250px;
    }
    #provTable>tbody>tr>td>span
    {
        border:none;
    }
 .dataTable>tbody>tr>td
    {
        height:25px;
    }
    .pdmsLabel
    {
        width:250px;
    }
    table
    {
        text-align:left!important;
    }
    .failureNotification
    {
        text-align:left!important;
    }
</style>
<asp:ValidationSummary ID="SpecialtyValidationSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="Specialty" />
<table id="ParentTable" runat="server">
    <colgroup>
        <col width="30%" />
        <col width="40%" />
        <col width="40%" />
    </colgroup>
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" Text="Description" CssClass="formLabel300" />
        </td>
        <td><asp:DropDownList ID="prov_Description" AutoPostBack="false" runat="server"></asp:DropDownList>
                <asp:CustomValidator ID="cv_prov_Description" runat="server" OnServerValidate="Validate_RequireDescription" Display="Dynamic" SetFocusOnError="true" ValidationGroup="Specialty" ErrorMessage="Description is required." Text="*" />
        </td>
        <td>
            <div class="pdmsLabel"><asp:Label ID="pdms_Description" runat="server" Text="debug" /></div>
        </td>
    </tr>
</table>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
<asp:CheckBox ID="LC116" runat="server" Text="Invalid" OnCheckedChanged="LC116_CheckedChanged" Visible="false" />

<%-- Start and End dates are removed by request but may be added later. So, setting to not display. Values are min/max for start/end  --%>
<asp:TextBox ID="prov_Start" runat="server" style="display:none" />
<asp:TextBox ID="prov_End" runat="server" style="display:none" />
<asp:CheckBox ID="prov_Certified" runat="server" style="display:none" Checked="true" />