<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_RemittanceInformation" Codebehind="RemittanceInformation.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/UserControls/AddressValidation.ascx" TagName="AddressValidation" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Address.ascx" TagName="Address" TagPrefix="uc" %>
<style type="text/css">
    .auto-style2 {
        width: 327px;
    }
</style>
<script type="text/javascript">
    function CheckPhoneLength(sender, args) {
        var re = /\D/g; // Remove any characters that are not numbers
        var test = args.Value.replace(re, "");
        if (test == "") return true;

        var len = test.length;
        if (len != 10)
            args.IsValid = false;
        if (test[0] == 0 || test[0] == 1 || test[3] == 0 || test[3] == 1)
            args.IsValid = false;
        return;
    }

   

                    
</script>

<asp:ValidationSummary ID="valSumRemittanceInformation" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valOwnerInfo" />
<asp:UpdatePanel ID="upProv" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:UpdateProgress runat="server" ID="upProgress" DisplayAfter="0">
            <ProgressTemplate>
                <div class="loading">
                    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/Images/ajax-loader.gif" />Loading...
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
          <div id="ParentTable" runat="server">
            <div class="row">          
                <div class="col-sm-3  text-right">
                    <span class="formLabel200">Same as Practice Location</span>
                </div>
                <div class="col-sm-9">
                    <asp:CheckBox ID="prov_Same" runat="server" CssClass="formFieldCheckBox" OnCheckedChanged="prov_Same_CheckedChanged" AutoPostBack="true" style="border:none" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3  text-right">
                     <asp:Label ID="Label3" runat="server" Text="Address Type" CssClass="formLabel200" />
                    
                </div>
                <div class="col-sm-9" style="text-align:left !important">
                    <asp:RadioButtonList ID="rblContactType" runat="server"  CssClass="QstRadioList" Width="250px" OnSelectedIndexChanged="rblContactType_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" >
                        <asp:ListItem Text="Individual" Value="P" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Organization" Value="B"></asp:ListItem>
                    </asp:RadioButtonList>
                   <%-- <asp:CustomValidator ID="cvContactType" ClientValidationFunction="TextBoxValidation" runat="server"
                ErrorMessage="Please fill TextBox" ControltoValidate="rblContactType" >* </asp:CustomValidator>
                --%>   </div>
               
            </div>
              <div class="row organization" runat="server" id="trOrg" visible="false">
                <div class="col-sm-3  text-right">
                     <asp:Label ID="Label4" runat="server" Text="Organization Address Name" CssClass="formLabel200" />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="prov_OrgName" runat="server" CssClass="formField" />                    
                        <asp:RequiredFieldValidator ID="rfvOrgName" runat="server"   ControlToValidate="prov_OrgName" ErrorMessage="* Please enter organization name" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
               
                     </div>
            </div>
             <div class="row individual" runat="server" id="trFirst" visible="false">
                <div class="col-sm-3  text-right">
                     <asp:Label ID="Label5" runat="server" Text="Address First Name" CssClass="formLabel200" />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtAFirstName" runat="server" CssClass="formField" />                    
                        <asp:RequiredFieldValidator ID="rfvAFirstName" runat="server"   ControlToValidate="txtAFirstName" ErrorMessage="* Please enter first name" Text="*" Display="Dynamic"  ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row individual" runat="server" id="trMiddle" visible="false">
                <div class="col-sm-3  text-right">
                     <asp:Label ID="Label6" runat="server" Text="Address Middle Name" CssClass="formLabel200" />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtAMiddleName" runat="server" CssClass="formField" />                    
                       <%-- <asp:RequiredFieldValidator ID="rfvAMiddleName" runat="server"   ControlToValidate="prov_OrgName" ErrorMessage="Please enter middle name" Enabled="false"></asp:RequiredFieldValidator>--%>
                </div>
            </div>
            <div class="row individual" runat="server" id="trLast" visible="false">
                <div class="col-sm-3  text-right">
                     <asp:Label ID="Label7" runat="server" Text="Address Last Name" CssClass="formLabel200" />
              
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtALastName" runat="server" CssClass="formField" />                    
                        <asp:RequiredFieldValidator ID="rfvALastName" runat="server"   ControlToValidate="txtALastName" ErrorMessage="* Please enter last name" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row individual" runat="server" id="trTitle" visible="false">
                <div class="col-sm-3  text-right">
                     <asp:Label ID="Label8" runat="server" Text="Title/Suffix" CssClass="formLabel200" />                  
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="formField" />                    
                        <%--<asp:RequiredFieldValidator ID="rfvTitle" runat="server"   ControlToValidate="prov_OrgName" ErrorMessage="Please enter title" Enabled="false"></asp:RequiredFieldValidator>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3  text-right">
                     <span class="formLabel200">Remittance Advice Name</span>
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="prov_AdviceName" runat="server" CssClass="formField" />
                </div>
            </div>
        </div>
         <uc:Address id="ucAddress" runat="server" ValidationGroup="valOwnerInfo"></uc:Address>
<div id="ParentTable2" runat="server">
            <div class="row">
                <div class="col-sm-3  text-right">
                    <span class="formLabel200">Address Phone Number*</span>
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="prov_address_phone" runat="server" CssClass="formField" CausesValidation="true" MaxLength="10" />
                    <ajax:MaskedEditExtender runat="server" ID="meePhoneNumber" AutoComplete="False" ClearMaskOnLostFocus="False" TargetControlID="prov_address_phone" MaskType="Number" Mask="(999) 999-9999" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                    <asp:RegularExpressionValidator runat="server" ID="revPhone" ControlToValidate="prov_address_phone" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" ErrorMessage="* Phone number is 10 digits with area code" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valOwnerInfo" Enabled="false"/>
                    <%--<asp:RequiredFieldValidator runat="server" ID="reqPhone" ControlToValidate="prov_address_phone" ErrorMessage="* Practice Phone Number is required." Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valOwnerInfo" />--%>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3  text-right">
                    <span class="formLabel200">Fax Number</span>
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="prov_Fax" runat="server" CssClass="formField" CausesValidation="true" MaxLength="10" />
                    <ajax:MaskedEditExtender runat="server" ID="MaskedEditExtender1" AutoComplete="False" ClearMaskOnLostFocus="False" TargetControlID="prov_Fax" MaskType="Number" Mask="(999) 999-9999" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
              <%--      <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="prov_Fax" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" ErrorMessage="* Fax number is 10 digits with area code" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valOwnerInfo" />
       --%>      
                     <asp:CustomValidator ID="cvFax" runat="server" SetFocusOnError="True" Display="Dynamic"
                        ControlToValidate="prov_Fax" ClientValidationFunction="CheckPhoneLength"
                        ErrorMessage="Enter valid Fax Number" Text="*" ValidationGroup="valOwnerInfo" />       
                </div>
            </div>
             <div class="row">
                <div class="col-sm-3  text-right">
                    <span class="formLabel200">Contact Name</span>
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="prov_ContactName" runat="server" CssClass="formField" />
                                             <%--<asp:CustomValidator runat="server" ID="cvtxtContact" ValidateEmptyText="true" ClientValidationFunction="ValidateContactTextBox"
                        ErrorMessage="Please enter contact name" ></asp:CustomValidator> --%>    
                   <%-- <asp:RequiredFieldValidator ID="rfvContactname" runat="server"   ControlToValidate="prov_ContactName" ErrorMessage="* Please enter contact name"  Text="*" Display="Dynamic"  ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>--%>
</div> 
             </div>
            <div class="row">
                <div class="col-sm-3  text-right">
                    <span class="formLabel200">Contact Phone Number*</span>
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="prov_Phone" runat="server" CssClass="formField" CausesValidation="true" MaxLength="10" />
                    <ajax:MaskedEditExtender runat="server" ID="MaskedEditExtender2" AutoComplete="False" ClearMaskOnLostFocus="False" TargetControlID="prov_Phone" MaskType="Number" Mask="(999) 999-9999" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                    <asp:RegularExpressionValidator runat="server" ID="rgvContactPhone" ControlToValidate="prov_Phone" ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" ErrorMessage="* Contact Phone number is 10 digits with area code" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valOwnerInfo" Enabled="false"/>
                  <%--  <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="prov_Phone" ErrorMessage="* Contact Phone Number is required." Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valOwnerInfo" />--%>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3  text-right">
                  <span class="formLabel200">Email Address</span><br />
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="prov_Email" runat="server" CssClass="formField" />
                     <%--<asp:RequiredFieldValidator ID="rfvEmailRequired" runat="server" ControlToValidate="prov_Email" 
                      ValidationGroup="valOwnerInfo" ErrorMessage="* Email is required." Display="Dynamic" Text="*"></asp:RequiredFieldValidator>   --%>                                     
                     <asp:RegularExpressionValidator ID="valEmailFormat" runat="server" ControlToValidate="prov_Email" Display="Dynamic" Text="*"
                      ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$" 
                      ErrorMessage="* Invalid email format." ValidationGroup="valOwnerInfo"></asp:RegularExpressionValidator>
                                 
                </div>
            </div>
        </div>
<%--        <uc:AddressValidation ID="valAddress" ValidationGroup="valOwnerInfo" runat="server" UnitAddressControl="prov_Suite" StreetAddressControl="prov_Address1"
            CityControl="prov_City" StateControl="ddlState" Zip5Control="prov_Zip" Zip4Control="prov_ExtZip" />--%>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
