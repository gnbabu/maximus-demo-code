<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ACHEftContact" Codebehind="ACHEftContact.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%--<style type="text/css">
     .table>tbody>tr>th,
    .table>tbody>tr>td {
        border:none !important;
        margin-bottom:0px !important;
     }
    .formField {
        width:200px !important;
    }    
</style>--%>
<div><asp:ValidationSummary ID="vsEftContact" runat="server" DisplayMode="List" ValidationGroup="valEftContact" CssClass="failureNotification"/></div>
<%--<table id="ParentTable" runat="server" class="table">
    <tr><td>--%>
        <div id="divAcheft">
    <div class="row">
        <div class="col-sm-4 text-right"><span class="formLabel200">Provider Contact First Name*</span></div>
        <div class="col-sm-8"><asp:TextBox ID="txtEFTContactFirstName" runat="server" CssClass="formField" MaxLength="100" />
            <asp:RequiredFieldValidator runat="server" ID="reqEFTContactFirstName"
                ControlToValidate="txtEFTContactFirstName" ErrorMessage="*Enter EFT Contact First Name" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valEftContact" />               
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
                SetFocusOnError="true" ValidationGroup="valEftContact" />               
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
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ValidationGroup="valEftContact"
                ControlToValidate="txtPhoneNo" ErrorMessage="Enter Phone Number" Text="*" Display="Dynamic" 
                SetFocusOnError="true" InitialValue="(___) ___-____" />               
            <asp:CustomValidator ID="cvPhoneNum" runat="server" SetFocusOnError="True" Display="Dynamic" 
                ControlToValidate="txtPhoneNo" ClientValidationFunction="CheckPhoneLength" 
                ErrorMessage="Enter valid Phone Number" Text="*" ValidationGroup="valEftContact" />
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
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator11"
                ControlToValidate="txtEmail" ErrorMessage="Enter E-mail Address" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valEftContact" />               
            <asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="txtEmail" Display="Dynamic" Text="*" ValidationGroup="valEftContact"
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
                ErrorMessage="Enter valid Fax Number" Text="*" ValidationGroup="valEftContact" />
        </div>
        <div class="pdmsLabel" style="display:none;"><asp:Label ID="Label1" runat="server" /></div>
    </div>
            </div>
       <%-- </td></tr>
</table>--%>

<asp:HiddenField ID="hdnRegEftContactID" runat="server" />