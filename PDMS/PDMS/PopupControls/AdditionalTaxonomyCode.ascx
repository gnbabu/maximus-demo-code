<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_AdditionalTaxonomyCode" Codebehind="AdditionalTaxonomyCode.ascx.cs" %>
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
    .atcTableTitle
    {
        height:50px!important;
    }
</style>
<asp:ValidationSummary ID="TaxonomyCodeValidationSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="AdditionalTaxonomyCode" />
<asp:UpdatePanel runat="server" ID="upProv" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upProv">
            <ProgressTemplate>
                <h3 style="background-color:Gray;">Processing...</h3>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <table id="ParentTable" runat="server">
            <colgroup>
                <col width="30%" />
                <col width="40%" />
                <col width="40%" />
            </colgroup>
            <%--<tr>
                <td>
                </td>
                <td>
                    Provider
                </td>
                <td>
                    PDMS
                </td>
            </tr>--%>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Code" CssClass="formLabel300" />
                </td>
                <td>
                    <asp:DropDownList ID="prov_Code" AutoPostBack="true" AppendDataBoundItems="True" runat="server" OnTextChanged="prov_Code_TextChanged"></asp:DropDownList>
                    <asp:CustomValidator ID="cv1" runat="server" OnServerValidate="Validate_CodeRequired" Display="Static" ValidationGroup="AdditionalTaxonomyCode" ErrorMessage="* Taxonomy code is required." Text="*" />
                </td>
                <td>
                    <div class="pdmsLabel"><asp:Label ID="pdms_Code" runat="server" /></div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Description" CssClass="formLabel300" />
                </td>
                <td><asp:DropDownList ID="prov_Description" AutoPostBack="true" AppendDataBoundItems="True" runat="server" OnTextChanged="prov_Description_TextChanged"></asp:DropDownList></td>
                <td><div class="pdmsLabel"><asp:Label ID="pdms_Description" runat="server" /></div></td>
            </tr>
            <%--<tr>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Start Date" CssClass="formLabel300" />
                </td>
                <td>
                    <asp:TextBox ID="prov_Start" runat="server" CssClass="formField formField" /><ajax:CalendarExtender ID="calStart" TargetControlID="prov_Start" runat="server" />
                    <asp:RequiredFieldValidator runat="server" ID="reqStart" ControlToValidate="prov_Start" ErrorMessage="* Start Date is required." Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="AdditionalTaxonomyCode" />
                    <asp:CustomValidator ID="CustVal_LC46_ERR23" runat="server" ControlToValidate="prov_Start" OnServerValidate="ValidateLC46_ERR23" Display="Static" ValidationGroup="AdditionalTaxonomyCode" ErrorMessage="* Duplicate entries - Check date spans." Text="*" />
                    <asp:CustomValidator ID="CustVal_LC37" runat="server" ControlToValidate="prov_Start" OnServerValidate="ValidateLC37" Display="Static" ValidationGroup="AdditionalTaxonomyCode" ErrorMessage="* Start Date is required." Text="*" />               
                </td>
                <td>
                    <div class="pdmsLabel"><asp:Label ID="pdms_Start" runat="server" /></div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="End Date" CssClass="formLabel300" />
                </td>
                <td>
                    <asp:TextBox ID="prov_End" runat="server" CssClass="formField formField" /><ajax:CalendarExtender ID="calEnd" TargetControlID="prov_End" runat="server" />
                </td>
                <td>
                    <div class="pdmsLabel"><asp:Label ID="pdms_End" runat="server" /></div>
                </td>
            </tr>--%>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />

<%-- Start and End dates are removed by request but may be added later. So, setting to not display. Values are min/max for start/end  --%>
<asp:TextBox ID="prov_Start" runat="server" style="display:none" />
<asp:TextBox ID="prov_End" runat="server" style="display:none" />
