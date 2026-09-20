<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_CertificationTransmittal, App_Web_glma3lal" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<div><asp:ValidationSummary ID="vsCertTrans" DisplayMode="List" runat="server" CssClass="failureNotification" 
    ValidationGroup="valCertTrans" /></div>
<table>
    <tr><td colspan="4">&nbsp;</td></tr>
    <tr>
        <td><span class="formLabel170">Type of Action*</span></td>
        <td><asp:DropDownList ID="ddlTypeOfAction" runat="server" CssClass="formDropDown" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="valCertTrans"
                ControlToValidate="ddlTypeOfAction" ErrorMessage="* Select a Type of Action" Text="*" Display="Dynamic" 
                SetFocusOnError="true" InitialValue="" />               
        </td>
        <td><span class="formLabel170">Ownership Change Date</span></td>
        <td align="left"><asp:TextBox ID="txtOwnershipChangeDate" runat="server" CssClass="formField" />
            <ajax:CalendarExtender ID="calOwnershipChangeDate" TargetControlID="txtOwnershipChangeDate" runat="server" />
            <asp:CompareValidator id="dateValidator" runat="server" ValidationGroup="valCertTrans"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtOwnershipChangeDate"  
                ErrorMessage="Select a valid Ownership Change Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true"> 
            </asp:CompareValidator>
        </td>
    </tr>
    <tr>
        <td><span class="formLabel170">Survey Date</span></td>
        <td align="left"><asp:TextBox ID="txtSurveyDate" runat="server" CssClass="formField" />
            <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtSurveyDate" runat="server" />
            <asp:CompareValidator id="CompareValidator1" runat="server" ValidationGroup="valCertTrans"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtSurveyDate"  
                ErrorMessage="Select a valid Survey Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true"> 
            </asp:CompareValidator>
        </td>
        <td><span class="formLabel170">Accreditation Status</span></td>
        <td><asp:DropDownList ID="ddlAccreditationStatus" runat="server" CssClass="formDropDown" /></td>
    </tr>
    <tr>
        <td><span class="formLabel170">Cert Eff Date</span></td>
        <td align="left"><asp:TextBox ID="txtCertEffDate" runat="server" CssClass="formField" />
            <ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="txtCertEffDate" runat="server" />
            <asp:CompareValidator id="CompareValidator2" runat="server" ValidationGroup="valCertTrans"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtCertEffDate"  
                ErrorMessage="Select a valid Certification Effective Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true"> 
            </asp:CompareValidator>
        </td>
        <td><span class="formLabel170">Total # Facility Beds</span></td>
        <td align="left"><ew:NumericBox ID="nbTotalFacBeds" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="10" CssClass="formField" /></td>
    </tr>
    <tr>
        <td><span class="formLabel170">Cert End Date</span></td>
        <td align="left"><asp:TextBox ID="txtCertEndDate" runat="server" CssClass="formField" />
            <ajax:CalendarExtender ID="CalendarExtender3" TargetControlID="txtCertEndDate" runat="server" />
            <asp:CompareValidator id="CompareValidator3" runat="server" ValidationGroup="valCertTrans"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtCertEndDate"  
                ErrorMessage="Select a valid Certification End Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true"> 
            </asp:CompareValidator>
        </td>
        <td><span class="formLabel170">Total # Certified Beds</span></td>
        <td align="left"><ew:NumericBox ID="nbTotalCertBeds" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="10" CssClass="formField" /></td>
    </tr>
    <tr>
        <td><span class="formLabel170">Eligibility</span></td>
        <td><asp:DropDownList ID="ddlEligibility" runat="server" CssClass="formDropDown" /></td>
        <td><span class="formLabel170">LTC Bed Breakdown</span></td>
        <td><asp:DropDownList ID="ddlLTCBedBreakdown" runat="server" CssClass="formDropDown" /></td>
    </tr>
    <tr valign="top">
        <td><span class="formLabel170">Comments</span></td>
        <td colspan="3" align="left">
            <asp:TextBox ID="txtComments" runat="server" Rows="10" CssClass="formFieldLarge" TextMode="MultiLine" MaxLength="4000" 
                ControlToValidate="txtComments" />
        </td>
    </tr>
</table>

<asp:HiddenField ID="hdnCertificationId" runat="server" />
