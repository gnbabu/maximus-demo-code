<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_TotalNumberBeds, App_Web_c4une0e1" %>
<%@ Register Src="~/UserControls/FormField.ascx" TagPrefix="uc" TagName="FormField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

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
    
</style>
<div><asp:ValidationSummary ID="DeaValidationSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="TotalBeds" /></div>
<table id="ParentTable" runat="server" style="margin-left:20px;margin-right:20px;margin-top:10px;">
     
    <tr>
        <td style="text-align:right">
            
            <asp:Label ID="Label1" runat="server" Text="Total Number of Beds" CssClass="formLabel200" />
        </td>
         <td  style="text-align:left;"  >
            <asp:TextBox runat="server" ID="txtTotalBeds" Width="50px" >   </asp:TextBox>
              <asp:RequiredFieldValidator runat="server" ID="reqNumBeds" ControlToValidate="txtTotalBeds" ErrorMessage="* No. Of Beds is required." Display="static"  Text="*" SetFocusOnError="true" ValidationGroup="TotalBeds" />
             <asp:CustomValidator ID="CustVal_LC59" runat="server" ControlToValidate="txtTotalBeds" OnServerValidate="ValidateLC59" Display="static" ValidationGroup="TotalBeds" Text="*" ErrorMessage="* Please enter No. Of Beds"  />  
         
            <%--<ew:NumericBox ID="prov_TotalBeds" runat="server" DecimalPlaces="0" PositiveNumber="True" MaxLength="4" CssClass="formField formField" />--%>
        
          </td>
          <td></td>
         
    </tr>
    <tr>
         <td colspan="3">
             <div class="pdmsLabel"><asp:Label ID="pdms_Beds" runat="server"   CssClass="failureNotification"/></div></td>
    </tr>
 </table>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
