<%@ Control Language="C#" AutoEventWireup="true" Inherits="Process_FormField" Codebehind="FormField.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccuctf" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<style type="text/css">
    .formLabel300
    {
        font-weight:bold;
    }
    
    .rblYesNo
    {
        width:100px;
        text-align:left;
    }
    
    .rbl_Vertical
    {
        text-align:left;
    }
    
    .fieldChk
    {
        text-align:left;
    }
    
    .formField
     {
         font-weight:normal!important;
         border:none!important;
     }
    
    .fieldTable
    {
        width:100%;
    }
    
    .fieldTable>tbody>tr>td:nth-child(1) .formLabel300
    {
        float:right;
    }
    
    .fieldTable>tbody>tr>td:nth-child(2) .formLabel300, .fieldTable>tbody>tr>td:nth-child(2)
    {
        float:left;
    }
    
    .fieldTableStacked label
    {
        width:75%;
        text-align:left;
    }

    .fieldTableStacked>tbody>tr>td
    {
        text-align:left;
    }  
    
    .fieldTableStacked>tbody>tr>td span
    {
        margin-left:50px;
    }
    
    .fieldTable input[type='text']
    {
        border:solid 1px black!important;
        width:150px!important
    }

</style>
<div id="divFieldTextBox" runat="server" style="display:none;">
    <table class="fieldTable">
        <tr>
            <td><asp:Label AssociatedControlID="fieldTextBox" ID="lblTextBox" runat="server" CssClass="formLabel300" /></td>
            <td>
                <asp:TextBox ID="fieldTextBox" runat="server" CssClass="formField formField" CausesValidation="true" />
                <asp:Label ID="lblTextBoxOptLabel" runat="server" Visible="false" />
                <asp:RequiredFieldValidator ID="valFieldTextBox" runat="server" ControlToValidate="fieldTextBox" CssClass="failureNotification" Enabled="false" />
                <asp:RegularExpressionValidator ID="regValFieldTextBox" ControlToValidate="fieldTextBox" CssClass="failureNotification" runat="server" Enabled="false" />
                <asp:CustomValidator ID="custValFieldTextBox" ValidateEmptyText="true" CssClass="failureNotification" runat="server" Enabled="true" ControlToValidate="fieldTextBox" OnServerValidate="custValFieldTextBox_ServerValidate" ErrorMessage="WHAT??" />
            </td>
        </tr>
    </table>
</div>

<div id="divFieldNumericBox" runat="server" style="display:none;">
    <table class="fieldTable">
        <tr>
            <td><asp:Label AssociatedControlID="fieldNumericBox" ID="lblNumericBox" runat="server" CssClass="formLabel300" /></td>
            <td>
                <ew:NumericBox ID="fieldNumericBox" runat="server" CssClass="formField formField" CausesValidation="true" />
                <asp:Label ID="lblNumbericBoxOptLabel" runat="server" Visible="false" />
            </td>
        </tr>
    </table>
</div>


<div id="divFieldCalendar" runat="server" style="display:none;">
    <table class="fieldTable">
        <tr>
            <td><asp:Label AssociatedControlID="fieldCalendar" ID="lblCalendar" runat="server" CssClass="formLabel300" /></td>
            <td>
                <asp:TextBox ID="fieldCalendar" runat="server" CssClass="formField formField" />
                <ccuctf:CalendarExtender ID="calExtend" TargetControlID="fieldCalendar" runat="server" /> 
            </td>
        </tr>
    </table>
</div>

<div id="divFieldLabel" runat="server" style="display:none">
    <table class="fieldTable">
        <tr>
            <td><asp:Label AssociatedControlID="fieldLabel" ID="lblLabel" runat="server" CssClass="formLabel300" /></td>
            <td><asp:Label ID="fieldLabel" runat="server" CssClass="formLabel300 formField" style="text-align:left" /></td>
        </tr>
    </table>
    
</div>

<div id="divFieldStackedLabel" runat="server" style="display:none">
    <table class="fieldTableStacked">
        <tr>
            <td><asp:Label AssociatedControlID="fieldStackedLabel" ID="lblStackedLabel" runat="server" CssClass="formLabel300" /></td>
        </tr>
        <tr>
            <td><asp:Label ID="fieldStackedLabel" runat="server" CssClass="formLabel300 formField" style="text-align:left; width:500px;" /></td>
        </tr>
    </table>
    
</div>

<div id="divFieldLinkButton" runat="server" style="display:none">
    <table class="fieldTable">
        <tr>
            <td><asp:Label AssociatedControlID="fieldLinkButton" ID="lblLinkButton" runat="server" CssClass="formLabel300" /></td>
            <td><asp:LinkButton ID="fieldLinkButton" runat="server" CssClass="formLabel300 formField" style="text-align:left" /></td>
        </tr>
    </table>
</div>

<div id="divFieldComboBox" runat="server" style="display:none">
    <div class="fieldTable">
        <div class="row">
            <div class="col-sm-4 text-right"><asp:Label AssociatedControlID="fieldComboBox" ID="lblComboBox" runat="server" CssClass="formLabel300" /></div>
            <div class="col=sm-8 text-left"><asp:DropDownList ID="fieldComboBox" AutoPostBack="false"  runat="server"></asp:DropDownList></div>
        </div>
    </div>
</div>

<div id="divFieldYesNo" runat="server" style="display:none;">
    <table class="fieldTable">
        <tr>
            <td><asp:Label AssociatedControlID="fieldYesNo" ID="lblYesNo" runat="server" CssClass="formLabel300" /></td>
            <td>
                <asp:RadioButtonList ID="fieldYesNo" runat="server" CssClass="formLabel300 rblYesNo formField" TextAlign="Right" RepeatDirection="Horizontal" >
                    <asp:ListItem Text="Yes" />
                    <asp:ListItem Text="No" />
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
</div>

<div id="divFieldRadioButtonList" runat="server" style="display:none">
    <table class="fieldTable">
        <tr>
            <td><asp:Label AssociatedControlID="fieldRadioButtonList" ID="lblRadioButtonList" runat="server" CssClass="formLabel300" style="text-align:top;" /></td>
            <td><asp:RadioButtonList ID="fieldRadioButtonList" runat="server" CssClass="formLabel300 rbl_Vertical formField" RepeatDirection="Vertical" TextAlign="Right" /></td>
        </tr>
    </table>
</div>

<div id="divFieldCheckBox" runat="server" style="display:none">
    <table class="fieldTable">
        <tr>
            <td><asp:Label AssociatedControlID="fieldCheckBox" ID="lblCheckBox" runat="server" CssClass="formLabel300" /></td>
            <td><asp:CheckBox ID="fieldCheckBox" runat="server" CssClass="formLabel300 fieldChk formField" /></td>
        </tr>
    </table>
</div>

<div id="divFieldNoSuchField" runat="server" style="display:none">
    <asp:Label ID="fieldNoSuchField" runat="server" CssClass="formLabel300" />
</div>

