<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_AdditionalSpecialties, App_Web_c4une0e1" %>
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
<asp:ValidationSummary ID="AdditionalSpecialtiesValidationSummary" runat="server" DisplayMode="List" CssClass="failureNotification" ValidationGroup="AdditionalSpecialties" />
<table id="ParentTable" runat="server">
    <colgroup>
        <col width="30%" />
        <col width="40%" />
        <col width="40%" />
    </colgroup>
    <tr>
        <td>
            <%--<h3>&nbsp;</h3>--%>
            <table id="labelTable" class="dataTable">
                <tr><td><asp:Label ID="Label1" runat="server" Text="Description" CssClass="formLabel300" /></td></tr>
                <%--<tr><td><asp:Label ID="Label2" runat="server" Text="Start Date" CssClass="formLabel300" /></td></tr>
                <tr><td><asp:Label ID="Label3" runat="server" Text="End Date" CssClass="formLabel300" /></td></tr>
                <tr><td><asp:Label ID="Label4" runat="server" Text="Board Certified" CssClass="formLabel300" /></td></tr>--%>
            </table>
        </td>
        <td>
            <%--<h3>Provider</h3>--%>
            <table id="provTable" class="dataTable">
                <tr><td>
                    <asp:DropDownList ID="prov_Description" AutoPostBack="false" runat="server" AppendDataBoundItems="True"></asp:DropDownList></td>
                    <asp:CustomValidator ID="cv1" runat="server" OnServerValidate="Validate_DescriptionRequired" Display="Static" ValidationGroup="AdditionalSpecialties" ErrorMessage="* Description is required." Text="*" />
                </td></tr>
                <%--<tr><td>
                    <asp:TextBox ID="prov_Start" DataFormatString="{0:d}" runat="server" CssClass="formField formField" /><ajax:CalendarExtender ID="calStart" TargetControlID="prov_Start" runat="server" />
                    <asp:RequiredFieldValidator runat="server" ID="reqStart" ControlToValidate="prov_Start" ErrorMessage="* Enter Start Date" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="AdditionalSpecialties" />
                    <asp:CustomValidator ID="CustVal_LC23_ERR23" runat="server" ControlToValidate="prov_Start" OnServerValidate="ValidateLC23_ERR23" Display="Static" ValidationGroup="AdditionalSpecialties" ErrorMessage="* Duplicate entries - Check date spans." Text="*" />
                    <asp:CustomValidator ID="CustVal_LC24" runat="server" ControlToValidate="prov_Start" OnServerValidate="ValidateLC24" Display="Static" ValidationGroup="AdditionalSpecialties" ErrorMessage="* Start Date is required." Text="*" /><%--when code/description is selected
                </td></tr>
                <tr><td><asp:TextBox ID="prov_End" DataFormatString="{0:d}" runat="server" CssClass="formField formField" /><ajax:CalendarExtender ID="calEnd" TargetControlID="prov_End" runat="server" /></td></tr>
                <tr><td><asp:CheckBox ID="prov_Certified" runat="server" CssClass="formLabel300 fieldChk formField" /></td></tr>
                <asp:CustomValidator ID="CustVal_LC26" runat="server" OnServerValidate="ValidateLC26" Display="Static" ValidationGroup="AdditionalSpecialties" ErrorMessage="* If Specialty Description is entered, Board Certified is Required." Text="*" />--%>
            </table>
        </td>
        <td style="display:block">
            <%--<h3>PDMS</h3>--%>
            <table id="pdmsTable" class="dataTable">
                <tr><td><div class="pdmsLabel"><asp:Label ID="pdms_Description" runat="server" Text="debug" /></div></td></tr>
                <%--<tr><td><div class="pdmsLabel"><asp:Label ID="pdms_Start" runat="server" Text="1/1/2013" /></div></td></tr>
                <tr><td><div class="pdmsLabel"><asp:Label ID="pdms_End" runat="server" Text="1/1/2013" /></div></td></tr>
                <tr><td><div class="pdmsLabel"><asp:Label ID="pdms_Certified" runat="server" Text="Y" /></div></td></tr>--%>
            </table>
        </td>
    </tr>
</table>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />

<%-- Start and End dates are removed by request but may be added later. So, setting to not display. Values are min/max for start/end  --%>
<asp:TextBox ID="prov_Start" runat="server" style="display:none" />
<asp:TextBox ID="prov_End" runat="server" style="display:none" />
<asp:CheckBox ID="prov_Certified" runat="server" style="display:none" />