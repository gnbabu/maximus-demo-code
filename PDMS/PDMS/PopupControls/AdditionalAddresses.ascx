<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_AdditionalAddressControl" Codebehind="AdditionalAddresses.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/UserControls/Address.ascx" TagName="Address" TagPrefix="uc" %>
<%--<style type="text/css">
       .table>tbody>tr>th,
    .table>tbody>tr>td {
        border:none !important;
        margin-bottom:0px !important;
    }
</style>--%>
<script type="text/javascript">
    function checkChar(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z]/g, '');
    }
    function NumberOnly() {
        var AsciiValue = event.keyCode
        if ((AsciiValue >= 48 && AsciiValue <= 57) || (AsciiValue == 8 || AsciiValue == 127))
            event.returnValue = true;
        else
            event.returnValue = false;
    }
</script><asp:ValidationSummary ID="SpecialtyValidationSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="AdditionalAddresses" />
<asp:updatePanel ID="PnlAdditionalAddresses" runat="server" UpdateMode="Conditional">
<ContentTemplate>
  <%--<table id="Table1" runat="server" class="table">
           <tr><td>--%>
<div id="ParentTable" runat="server">
    <%--<colgroup>
        <col width="20%" />
        <col width="50%" />
        <col width="40%" />
    </colgroup>--%>
    <div class="row">
        <div class="pg-hint2"><i style="float:right;">Please add any additional addresses.</i></div>
    </div>    
</div>
<uc:Address ID="ucAddress" runat="server" ValidationGroup="AdditionalAddresses" GetGeocode="false" />
<div id="ParentTable2" runat="server">
   <div class="row">
        <div class="col-sm-3 text-right">
            <asp:Label ID="Label10" runat="server" Text="Phone Number*" CssClass="formLabel200" />
        </div>
        <div class="col-sm-9">
            <asp:TextBox ID="prov_Phone" runat="server" CssClass="formField" CausesValidation="true" MaxLength="10" />
            <ajax:MaskedEditExtender runat="server" ID="meePhoneNumber" AutoComplete="False" 
                ClearMaskOnLostFocus="False" TargetControlID="prov_Phone" 
                MaskType="Number" Mask="(999) 999-9999" CultureAMPMPlaceholder="" 
                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" 
                CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" 
                CultureTimePlaceholder="" Enabled="True" />
            <asp:RegularExpressionValidator runat="server" ID="regexPhone" ControlToValidate="prov_Phone" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" ErrorMessage="* Phone is 10 digits including area code" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="AdditionalAddresses" />
            <asp:RequiredFieldValidator runat="server" ID="reqPhone" ControlToValidate="prov_Phone" ErrorMessage="* Phone Number is required" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="AdditionalAddresses" />
            <asp:CustomValidator ID="cvprov_PhoneAddl" runat="server" SetFocusOnError="True" Display="Dynamic" 
                        ControlToValidate="prov_Phone" ClientValidationFunction="CheckPhoneLength" 
                        ErrorMessage="Enter valid Phone Number" Text="*" ValidationGroup="AdditionalAddresses" />
        </div>
        <div style="display:none;">
            <asp:Label ID="pdms_Phone" runat="server"/>       </div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right">
            <asp:Label ID="Label4" runat="server" Text="Ext" CssClass="formLabel200" />
        </div>
        <div class="col-sm-9">
            <asp:TextBox ID="prov_PhoneExt" runat="server" CssClass="formField" CausesValidation="true" MaxLength="6" />
        </div>
        <div style="display:none;">
            <asp:Label ID="pdms_PhoneExt" runat="server"/>
        </div>
    </div>
</div>
               <%-- </td></tr>
</table>--%>
</ContentTemplate></asp:updatePanel>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
<asp:HiddenField ID="hdnRegAdditionalAddressID" runat="server" />