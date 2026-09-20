<%@ control language="C#" autoeventwireup="true" inherits="Pages_SubstituteW4Form, App_Web_av5ll3zk" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/Pages/ContactEntry.ascx" TagName="ContactEntry" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccuctf" %>

<style type="text/css">   
    .divHistoryAndAdd
    {
        width: 100%;
        text-align: right;
    }
    
    .divGrid
    {
        width: 100%;
    }
    
    .gridview
    {
        float: right;
    }
    .formFieldReadOnly100l
    {
        width:auto!important;
    }
    
    .gridViewHeader>th>a
    {
        color:White!important;
    }
    
    .rbl_Vertical>tbody>tr>td:nth-child(1)
    {
        width:75px;
    }
</style>

<div class="boxPanelFull">
    <b>Information from the Identification page displayed below.</b><br />
    <i>Corrections to this information must be made in the Organization/Individual Identification and Primary Contact sections of the Identification page.</i><br /><br />
    <table class="wdAuto">
        <tr>
            <td class="formLabel wd170">Legal Business Name</td>
            <td><asp:Label ID="RS01" runat="server" CssClass="formFieldDisplay" /></td>
        </tr>
        <tr>
            <td  class="formLabel  wd170">Tax ID</td>
            <td><asp:Label ID="RS03" runat="server" CssClass="formFieldDisplay" /></td>
        </tr>
        <tr>
            <td  class="formLabel wd170">DBA</td>
            <td><asp:Label ID="RS02" runat="server" CssClass="formFieldDisplay" /></td>

        </tr>
        </table>
        <br /><div class="pg-hint2">**Please visit (opens new window)<a href="http://www.irs.gov" title="http://www.irs.gov"  target="_blank" >http://www.irs.gov</a> to obtain a copy of the W4 with instructions.</div>
</div>
<br />
<div>
    <table>
            <tr><td class="formLabel" style="width:90px;">Marital Status</td><td><asp:DropDownList ID="ddlMaritalStatus" runat="server"/></td></tr>
        <tr><td></td><td><span class="field-hint">Note: If married, but legally separated, or spouse is a nonresident alien, select “Single”.</span> </td></tr>
    </table>
    <table>
        <tr>
    <td>
        
        <span class="formLabel" style="width:auto; text-align:left">If your last name differs from that shown on your social security card, check here. You must call 1-800-772-1213 for a replacement card. </span></td>
        <td><asp:CheckBox ID="chkDifferentLastName" runat="server" />
    </td>
    </tr>
        <tr style="height:auto"><td>
    
        <span class="formLabel" style="width:auto">Total number of allowances you are claiming</span></td><td><asp:TextBox ID="txtNoofAllowances" runat="server" /></td><td><span></span></td>
        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="valW4Info"
                ControlToValidate="txtNoofAllowances" ErrorMessage="Enter total number of allowances" Text="*" Display="Dynamic" 
                SetFocusOnError="true" />               
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNoofAllowances" 
                ValidationExpression="^\d+$" ErrorMessage="Enter total number of allowances" Text="*" Display="Dynamic" 
                ValidationGroup="valW4Info" />
        </tr>
    <tr style="height:auto">
        <td><span class="formLabel" style="width:auto">Additional amount, if any, you want withheld from each paycheck</span></td><td><asp:TextBox ID="txtAdditionalAmount" runat="server" /></td><td><span></span></td>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtAdditionalAmount" 
                ValidationExpression="^\d+$" ErrorMessage="Enter additional amount" Text="*" Display="Dynamic" 
                ValidationGroup="valW4Info" />
        <ccuctf:MaskedEditExtender runat="server" ID="meeAdditionalAmount" AutoComplete="False" ClearMaskOnLostFocus="False" TargetControlID="txtAdditionalAmount" MaskType="Number" Mask="999\,999.99" CultureAMPMPlaceholder="" 
                                            CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" InputDirection="RightToLeft"/>
    </tr>
        
    <tr style="height:75px"><td>
        
        <span class="formLabel" style="width:auto; text-align:left">I claim exemption from withholding for 2015, and I certify that I meet both of the following conditions for exemption.
<br />
• Last year I had a right to a refund of all federal income tax withheld because I had no tax liability, and
<br />
• This year I expect a refund of all federal income tax withheld because I expect to have no tax liability.
            <br />
If you meet both conditions, indicate “Exempt” here.</span></td><td style="vertical-align:bottom"><asp:CheckBox ID="chkExempt" Text="Exempt" runat="server"/>
    </td>
        </tr>
</table>
</div>
<uc1:MessageBox ID="MessageBox2" runat="server" />