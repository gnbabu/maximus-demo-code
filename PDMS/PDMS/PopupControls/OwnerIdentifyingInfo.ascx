<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_OwnerIdentifyingInfo" Codebehind="OwnerIdentifyingInfo.ascx.cs" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>
<style type="text/css">

    .RadioList tr 
    {     
        margin: 0px;
        padding: 0 5px 0 0; 
        vertical-align: middle;
        margin: 0 50px 0 0;
    }
</style>
<script type="text/javascript">
    $(document).ready(function () {

        //Update Panel needs after async postback, else event handlers are lost.
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        //prm.add_pageLoaded(setupOrgEventHandlers);
    });



    
</script>

<div>
    <asp:ValidationSummary ID="vsOwnerIdentifyingInfo" runat="server" DisplayMode="List" ValidationGroup="valOwnerIdentifyingInfo" />
    
</div>
    



<br />
<table  id="ParentTable" runat="server" style="width:auto;">

    <tr id="trBusinessName" runat="server">
        <td class="formLabel wd170"><asp:Label runat="server" ID="lblname" Text="Entity Name"></asp:Label>
           </td>
        <td  style="text-align:left;">
            <asp:TextBox ID="txtLegalBusinessName" runat="server" MaxLength="100" CssClass="formFieldReadOnly" ReadOnly="true" Enabled="false" />
      <%--      <asp:RequiredFieldValidator runat="server" ID="reqLegalBusinessName"
                ControlToValidate="txtLegalBusinessName" ErrorMessage="*Enter Legal Business Name" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valOwnerIdentifyingInfo" />  --%>             
        </td>
    </tr>

    <tr id="trDBA" runat="server">
        <td class="formLabel wd170">DBA Name</td>
        <td  style="text-align:left;">
            <asp:TextBox ID="txtDBA" runat="server" MaxLength="100" CssClass="formFieldReadOnly" Enabled="false" />
            <%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator10"
                ControlToValidate="txtLegalBusinessName" ErrorMessage="*Enter Legal Business Name" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valOwnerIdentifyingInfo" /> --%>            
        </td>
    </tr>
        <tr id="trBirthDate" runat="server">
             <td class="formLabel wd170">Birth Date</td>
      
        <td  style="text-align:left;"><asp:TextBox ID="txtBirthDate" runat="server" CssClass="formFieldReadOnly" Enabled="false" />
            <%--<ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtBirthDate" runat="server" />--%>
          <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" SetFocusOnError="true" ValidationGroup="valOwnerIdentifyingInfo" Text="*"
                ControlToValidate="txtBirthDate" ErrorMessage="Enter Date of Birth" Display="Dynamic" />
            <asp:CompareValidator id="CompareValidator3" runat="server" ValidationGroup="valOwnerIdentifyingInfo"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtBirthDate"  
                ErrorMessage="Select a valid Date of Birth" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true"> 
            </asp:CompareValidator>--%>
        </td>
    </tr>
        <tr id="trSSN" runat="server">
        <td><span class="formLabel wd170">SSN</span></td>
        <td  style="text-align:left;"><ew:NumericBox ID="txtSSN" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="9" CssClass="formFieldReadOnly" Enabled="false" />
            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtSSN" 
                ValidationExpression="^\d{9}$" ErrorMessage="Enter a 9 digit SSN" Text="*" Display="Dynamic" ValidationGroup="valOwnerIdentifyingInfo" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1"
                ControlToValidate="nbTaxID" ErrorMessage="*Enter SSN" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valOwnerIdentifyingInfo" />      --%>         

     

        </td>
    </tr>

  <tr id="trTaxID" runat="server">
        <td><span class="formLabel wd170">Tax ID</span></td>
        <td  style="text-align:left;"><ew:NumericBox ID="nbTaxID" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="9" CssClass="formFieldReadOnly" Enabled="false" />
           <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="nbTaxID" 
                ValidationExpression="^\d{9}$" ErrorMessage="Enter a 9 digit Tax ID" Text="*" Display="Dynamic" ValidationGroup="valOwnerIdentifyingInfo" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2"
                ControlToValidate="nbTaxID" ErrorMessage="*Enter Tax ID" Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valOwnerIdentifyingInfo" />              --%> 
    

        </td>
    </tr>


    <tr id="trNPI">
        <td><span class="formLabel wd170">NPI</span></td>
        <td  style="text-align:left;"><ew:NumericBox ID="nbNPI" runat="server" DecimalPlaces="0" MaxLength="10" PositiveNumber="true" CssClass="formFieldReadOnly" Enabled="false" />
            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="nbNPI" 
                ValidationExpression="^[1-9]\d{9}$" ErrorMessage="*NPI requires 10 digits and cannot start with 0" Text="*" Display="Dynamic" 
                ValidationGroup="valOwnerIdentifyingInfo" />--%>


        </td>
    </tr>

        <tr id="trProviderNumber">
        <td class="formLabel wd170"><asp:Label ID="lblProviderNumber" runat="server" Text="Medicaid ID" CssClass="formLabel wd170" /></td>
        <td  style="text-align:left;"><ew:NumericBox ID="nbProviderNumber" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="11" CssClass="formFieldReadOnly" Enabled="false" />
            <%--<asp:RegularExpressionValidator ID="rfeProviderNumber11Digit" runat="server" ControlToValidate="nbProviderNumber" 
                ValidationExpression="^\d{11}$" ErrorMessage="Enter an 11 digit Provider Number" Text="*" Display="Dynamic" ValidationGroup="valOwnerIdentifyingInfo" />--%>
        </td>
    </tr>
        <tr id="trCategoryType" >
        <td class="formLabel wd170"  style="height:20px;margin-top:7px;">Select the most appropriate category below:</td>
        <td>
            <asp:RadioButtonList ID="rblEntityType" runat="server" RepeatDirection="Vertical" RepeatLayout ="Table" CellPadding="0" CellSpacing ="0" CssClass="RadioList">

            </asp:RadioButtonList>
              <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4"
                ControlToValidate="rblEntityType" ErrorMessage="* Select the most appropriate category " Text="*" Display="Dynamic" 
                SetFocusOnError="true" ValidationGroup="valOwnerIdentifyingInfo" />               

        </td>
    </tr>
    </table>



<asp:HiddenField ID="hdnProviderCategoryId" runat="server" />
<asp:HiddenField ID="hdnProviderTypeId" runat="server" />
<asp:HiddenField ID="hdnRegOwnerIdentifyingInfoID" runat="server" />