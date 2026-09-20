<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ACHBankingInfo" Codebehind="ACHBankingInfo.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<script type="text/javascript">
    function numericOnly(obj) {
        obj.value = obj.value.replace(/[^0-9]/g, '');
    }
    function NumberOnly() {
        var AsciiValue = event.keyCode
        if ((AsciiValue >= 48 && AsciiValue <= 57) || (AsciiValue == 8 || AsciiValue == 127))
            event.returnValue = true;
        else
            event.returnValue = false;
    }
    function AlphaNumericOnly(obj) {
        
        obj.value = obj.value.replace(/[^a-zA-Z 0-9\n\r]+/g, '');
        
    
    }

</script>
<%--<style type="text/css">
     .table>tbody>tr>th,
    .table>tbody>tr>td {
        border:none !important;
        margin-bottom:0px !important;
     }
    .formField, .formDropDown {
        width:200px !important;
    }
   select {
    min-width: 200px !important;
    }
</style>--%>
<div><asp:ValidationSummary ID="vsBankingInfo" runat="server" DisplayMode="List" ValidationGroup="valBankingInfo" CssClass="failureNotification" /><br />
    <asp:ValidationSummary ID="vsRoutingNumber" runat="server" DisplayMode="List" ValidationGroup="valBankingInfo1" CssClass="failureNotification"/>
</div>
<%--<table id="ParentTable" runat="server" class="table" style="text-align:center;margin:20px !important;">
  <tr><td>--%>
    <div id="divAchBankInfo">
         
    <div class="row">
         <div class ="col-sm-12">
        <div class="col-sm-4 text-right"><span class="formLabel200">Financial Institution Name*</span></div>
        <div class="col-sm-8 text-left"><asp:TextBox ID="txtBankName" runat="server" CssClass="formField" MaxLength="80" />
            <asp:RequiredFieldValidator runat="server" ID="reqBankName"
                ControlToValidate="txtBankName" ErrorMessage="*Enter Bank Name" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valBankingInfo" />               
        </div>
        <div class="pdmsLabel" style="display: none;"><asp:Label ID="lblPDMSBankName" runat="server" /></div>  
        </div>
    </div>
    <div class="row">
         <div class ="col-sm-12">
        <div class="col-sm-4"><span class="formLabel200" style="white-space:normal">Financial Institution Routing Number*</span></div>
         <div class="col-sm-8 text-left"><asp:TextBox ID="nbABANumber" runat="server" MaxLength="9" CssClass="formField" onKeyUp="javascript:numericOnly(this);" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="valBankingInfo"
                ControlToValidate="nbABANumber" ErrorMessage="Enter ACH Transit / ABA Number" Text="*" Display="Dynamic" 
                SetFocusOnError="true" />               
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="nbABANumber" 
                ValidationExpression="(?!0{9})(?!9{9})^\d{9}$" ErrorMessage="Enter a 9 digit ACH Transit / ABA Number" Text="*" Display="Dynamic" 
                ValidationGroup="valBankingInfo" />
        </div>
        <div class="pdmsLabel" style="display: none;"><asp:Label ID="lblPDMSABANumber" runat="server" /></div>
     </div>
    </div>
    <div class="row">
         <div class ="col-sm-12">
        <div class="col-sm-4 text-right"><span class="formLabel200" style="white-space:normal !important;">Confirm Financial Institution Routing Number*</span></div>
        <div class="col-sm-8 text-left"><asp:TextBox ID="nbConfirmABANumber" runat="server" MaxLength="9" CssClass="formField" onKeyUp="javascript:numericOnly(this);" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ValidationGroup="valBankingInfo"
                ControlToValidate="nbConfirmABANumber" ErrorMessage="Enter Confirm ACH Transit / ABA Number" Text="*" Display="Dynamic" 
                SetFocusOnError="true" />               
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="nbConfirmABANumber" 
                ValidationExpression="(?!0{9})(?!9{9})^\d{9}$" ErrorMessage="Enter a 9 digit Confirm ACH Transit / ABA Number" Text="*" Display="Dynamic" ValidationGroup="valBankingInfo" />
            <asp:CompareValidator ID="cmpABANumber" runat="server" ControlToCompare="nbABANumber" ControlToValidate="nbConfirmABANumber" 
                Display="Dynamic" ErrorMessage="ACH Transit / ABA Number and Confirm ACH Transit / ABA Number must match."
                Text="*" SetFocusOnError="true" ValidationGroup="valBankingInfo" />               
        </div>
    </div>
        </div> <br />  
    <div class="row">
         <div class ="col-sm-12">
        <div class="col-sm-4 text-right"><span class="formLabel200">Account Number*</span></div>
        <div class="col-sm-8 text-left"><asp:TextBox ID="nbAccountNumber" runat="server" MaxLength="17" CssClass="formField" onKeyUp="javascript:numericOnly(this);" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ValidationGroup="valBankingInfo"
                ControlToValidate="nbAccountNumber" ErrorMessage="Enter Account Number" Text="*" Display="Dynamic" 
                SetFocusOnError="true" />    
            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="nbAccountNumber" 
                ValidationExpression="^(?!0+$)(?!9+$)\d{1,17}$" ErrorMessage="Enter a Account Number" Text="*" Display="Dynamic" ValidationGroup="valBankingInfo" />   
       
        </div>
        <div class="pdmsLabel" style="display: none;"><asp:Label ID="lblPDMSAccountNumber" runat="server" /></div>
      </div>
    </div>
    <div class="row">
         <div class ="col-sm-12">
        <div class="col-sm-4 text-right"><span class="formLabel200">Confirm Account Number*</span></div>
        <div class="col-sm-8 text-left"><asp:TextBox ID="nbConfirmAccountNumber" runat="server" MaxLength="17" CssClass="formField" onKeyUp="javascript:numericOnly(this);" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9" ValidationGroup="valBankingInfo"
                ControlToValidate="nbConfirmAccountNumber" ErrorMessage="Enter Confirm Account Number" Text="*" Display="Dynamic" 
                SetFocusOnError="true" />   
            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="nbConfirmAccountNumber" 
                ValidationExpression="^(?!0+$)(?!9+$)\d{1,17}$" ErrorMessage="Enter a Confirm Account Number" Text="*" Display="Dynamic" ValidationGroup="valBankingInfo" />   
          
            <asp:CompareValidator ID="cmpAccountNumber" runat="server" ControlToCompare="nbAccountNumber" ControlToValidate="nbConfirmAccountNumber" 
                Display="Dynamic" ErrorMessage="Account Number and Confirm Account Number must match."
                Text="*" SetFocusOnError="true" ValidationGroup="valBankingInfo" />               
        </div>
    </div>
        &nbsp;</div>
      <div class="row">
           <div class ="col-sm-12">
        <div class="col-sm-4 text-right"><span class="formLabel200">Account Type*</span></div>
          <%--<div class="col-sm-1"></div>--%>
          <div class="col-sm-8 text-left"><asp:RadioButtonList ID="rblCheckingSavings" RepeatDirection="Horizontal" runat="server" CssClass="formLabel">  
                <asp:ListItem Value="1">Checking</asp:ListItem>  
                <asp:ListItem Value="2">Savings</asp:ListItem>  
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator11" ValidationGroup="valBankingInfo"
                ControlToValidate="rblCheckingSavings" ErrorMessage="Select Checking or Savings" Text="*" Display="Dynamic" 
                SetFocusOnError="true" InitialValue="" />               
        </div>
        <div class="pdmsLabel" style="display: none;"><asp:Label ID="lblPDMSAccountType" runat="server" /></div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right"><span id="lblAccountTypeEntity" class="formLabel200" runat="server">Account Type Entity*</span></div>
        <div class="col-sm-8 text-left"><asp:DropDownList ID="ddlAccountTypeEntity" runat="server" AppendDataBoundItems="True" CssClass="formDropDown"/>
            <asp:RequiredFieldValidator runat="server" ID="RFVAccountEntityType" ValidationGroup="valBankingInfo"
                ControlToValidate="ddlAccountTypeEntity" ErrorMessage="Select Account Type Entity" Text="*" Display="Dynamic" 
                SetFocusOnError="true" InitialValue="" />
        </div>
        </div>
    </div>

        </div>     
   <%-- </td></tr>
</table>--%>

<asp:HiddenField ID="hdnRegAchRequestID" runat="server" />

<!--ACH Contact-->

<!--<div><asp:ValidationSummary ID="vsEftContact" runat="server" DisplayMode="List" ValidationGroup="valEftContact" CssClass="failureNotification"/></div>-->
<%--<table id="ParentTable" runat="server" class="table">
    <tr><td>--%>
        <div id="divAcheft">
    <div class="row">
        <div class="col-sm-4 text-right"><span class="formLabel200">Provider Contact First Name*</span></div>
        <div class="col-sm-8"><asp:TextBox ID="txtEFTContactFirstName" runat="server" CssClass="formField" MaxLength="100" />
            <asp:RequiredFieldValidator runat="server" ID="reqEFTContactFirstName"
                ControlToValidate="txtEFTContactFirstName" ErrorMessage="*Enter EFT Contact First Name" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valBankingInfo" />               
        </div>
        <div class="pdmsLabel" style="display:none;"><asp:Label ID="lblPDMSEFTContactFirstName" runat="server" /></div>
    </div>

    <div class="row">
        <div class="col-sm-4 text-right"><span class="formLabel200">Middle Name</span></div>
        <div class="col-sm-8"><asp:TextBox ID="txtEFTContactMiddleName" runat="server" CssClass="formField" MaxLength="100" />          
        </div>
        <div class="pdmsLabel" style="display:none;"><asp:Label ID="lblPDMSEFTContactMiddleName" runat="server" /></div>
    </div>

    <div class="row">
        <div class="col-sm-4 text-right"><span class="formLabel200">Last Name*</span></div>
        <div class="col-sm-8"><asp:TextBox ID="txtEFTContactLastName" runat="server" CssClass="formField" MaxLength="100" />
            <asp:RequiredFieldValidator runat="server" ID="reqEFTContactLastName"
                ControlToValidate="txtEFTContactLastName" ErrorMessage="*Enter Last Name" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valBankingInfo" />               
        </div>
        <div class="pdmsLabel" style="display:none;"><asp:Label ID="lblPDMSEFTContactLastName" runat="server" /></div>
    </div>

    <div class="row">
        <div class="col-sm-4 text-right"><span class="formLabel200">Phone Number*</span></div>
        <div class="col-sm-8"><asp:TextBox ID="txtPhoneNo" runat="server" CssClass="formField" />
            <ajax:MaskedEditExtender runat="server" ID="meePhoneNumber" AutoComplete="False" 
                ClearMaskOnLostFocus="False" TargetControlID="txtPhoneNo" 
                MaskType="Number" Mask="(999) 999-9999" CultureAMPMPlaceholder="" 
                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ValidationGroup="valBankingInfo"
                ControlToValidate="txtPhoneNo" ErrorMessage="Enter Phone Number" Text="*" Display="Dynamic" 
                SetFocusOnError="true" InitialValue="(___) ___-____" />               
            <asp:CustomValidator ID="cvPhoneNum" runat="server" SetFocusOnError="True" Display="Dynamic" 
                ControlToValidate="txtPhoneNo" ClientValidationFunction="CheckPhoneLength" 
                ErrorMessage="Enter valid Phone Number" Text="*" ValidationGroup="valBankingInfo" />
        </div>
        <div class="pdmsLabel" style="display:none;"><asp:Label ID="lblPDMSPhoneNumber" runat="server" /></div>
    </div>
    <div class="row">
        <div class="col-sm-4 text-right"><span class="formLabel200">Extension</span></div>
        <div class="col-sm-8"><asp:TextBox ID="txtPhoneExt" runat="server" CssClass="formField" MaxLength="5" /></div>
        <div class="pdmsLabel" style="display:none;"><asp:Label ID="lblPDMSPhoneExt" runat="server" /></div>
    </div>
    <div class="row">
        <div class="col-sm-4 text-right"><span class="formLabel200">Email Address*</span></div>
        <div class="col-sm-8"><asp:TextBox ID="txtEmail" CssClass="formField"  runat="server" MaxLength="100" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1"
                ControlToValidate="txtEmail" ErrorMessage="Enter E-mail Address" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valBankingInfo" />               
            <asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="txtEmail" Display="Dynamic" Text="*" ValidationGroup="valBankingInfo"
                ErrorMessage="Enter valid E-mail" SetFocusOnError="true" ValidationExpression="^[A-Z'a-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$" />
        </div>
        <div class="pdmsLabel" style="display:none;"><asp:Label ID="lblPDMSEmail" runat="server" /></div>
    </div>
    <div class="row">
        <div class="col-sm-4 text-right"><span class="formLabel200">Fax Number</span></div>
        <div class="col-sm-8"><asp:TextBox ID="txtFaxNumber" runat="server" CssClass="formField" />
            <ajax:MaskedEditExtender runat="server" ID="MaskedEditExtender1" AutoComplete="False" 
                ClearMaskOnLostFocus="False" TargetControlID="txtFaxNumber" 
                MaskType="Number" Mask="(999) 999-9999" CultureAMPMPlaceholder="" 
                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" 
                CultureDatePlaceholder="" CultureDecimalPlaceholder="" 
                CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                           
            <asp:CustomValidator ID="cvFaxNumber" runat="server" SetFocusOnError="True" Display="Dynamic" 
                ControlToValidate="txtFaxNumber" ClientValidationFunction="CheckPhoneLength" 
                ErrorMessage="Enter valid Fax Number" Text="*" ValidationGroup="valBankingInfo" />
        </div>
        <div class="pdmsLabel" style="display:none;"><asp:Label ID="Label1" runat="server" /></div>
    </div>
            </div>
       <%-- </td></tr>
</table>--%>

<asp:HiddenField ID="hdnRegEftContactID" runat="server" />