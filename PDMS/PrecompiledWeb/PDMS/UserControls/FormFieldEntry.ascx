<%@ control language="C#" autoeventwireup="true" inherits="UserControls_FormFieldEntry, App_Web_p4ixifjm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<asp:MultiView ID="mltFormFieldEntry" runat="server" ActiveViewIndex="0">
    <asp:View ID="vwLabel" runat="server">
        <!-- Label -->
        <asp:Label ID="lblDisplay" CssClass="formFieldDisplay"  runat="server" />
    </asp:View>
    <asp:View ID="vwTextFreeForm" runat="server">
        <!-- Free Form Text -->
        <asp:TextBox ID="txtEdit" runat="server" CssClass="formField" MaxLength="200" />
    </asp:View>
    <asp:View ID="vwDropDown" runat="server">
        <!-- Dropdown list -->
        <asp:DropDownList ID="ddlEdit" runat="server" CssClass="formField" AutoPostBack="true" 
            onselectedindexchanged="ddlEdit_SelectedIndexChanged" />
    </asp:View>
    <asp:View ID="vwNumeric" runat="server">
        <!-- Numeric Box -->
        <ew:NumericBox ID="nbEdit" runat="server" DecimalPlaces="0" PositiveNumber="True" CssClass="formField" />                
    </asp:View>
    <asp:View ID="vwDateEntry" runat="server">
        <!-- Date Entry -->
        <asp:TextBox ID="txtEditDate" runat="server" CssClass="formField" MaxLength="12" />
        <ajax:CalendarExtender ID="calEditDate" TargetControlID="txtEditDate" runat="server" Format="MM/dd/yyyy" />
        <asp:CompareValidator id="cmpValEditDate" runat="server" SetFocusOnError="true"
            Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEditDate"  
            Text="* Select a valid Effective Date" Display="Dynamic" ValueToCompare="MM/dd/yyyy" />
    </asp:View>
    <asp:View ID="vwLicenseNo" runat="server">
        <!-- License Number Entry -->
        <asp:DropDownList ID="ddlLicenseNo" runat="server" CssClass="formFieldAuto" />
        <asp:TextBox ID="txtLicenseNo" runat="server" CssClass="formField100" MaxLength="20" />
    </asp:View>
    <asp:View ID="vwStateZip" runat="server">
        <!-- State/Zip Entry -->
        <asp:DropDownList ID="ddlState" runat="server" CssClass="formFieldAuto" />
        <ew:NumericBox ID="nbZip" runat="server" DecimalPlaces="0" PositiveNumber="True" CssClass="formField100" MaxLength="9" />                
    </asp:View>
    <asp:View ID="vwPhoneNo" runat="server">
        <!-- Phone No Entry -->
        <asp:TextBox ID="txtPhoneNo" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="10" CssClass="formField" />
        <ajax:MaskedEditExtender runat="server" ID="meePhoneNumber" AutoComplete="False" ClearMaskOnLostFocus="False" 
            TargetControlID="txtPhoneNo" MaskType="Number" Mask="(999) 999-9999" CultureAMPMPlaceholder="" 
            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
            CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
    </asp:View>
</asp:MultiView>