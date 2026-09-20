<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_Appeals_ascx, App_Web_tiu3g34i" %>
<%@ Register Src="~/UserControls/FormField.ascx" TagPrefix="uc" TagName="FormField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<%--<%@ Register Src="~/UserControls/Separator.ascx" TagName="Separator" TagPrefix="ucSep" %>--%>

<div><asp:ValidationSummary ID="vsAppeals" runat="server" DisplayMode="List" ValidationGroup="valAppeals" /></div>

<br />
<%--<ucSep:Separator ID="SepInitialNotice" runat="server" Header="Initial Notice" Mode="1" />--%>
<span class="pageHeader">Initial Notice</span>
<div id="ParentTable" runat="server" style="width:70%;">
    <div class="row">
        <div class="col-sm-4 text-right">
            <asp:Label ID="Label6" runat="server" Text="Date Of Initial Notice" CssClass="formLabel" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox ID="txtDateOfInitialNotice" runat="server" CssClass="formField formField" /><ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtDateOfInitialNotice" runat="server" />
            <asp:RequiredFieldValidator runat="server" ID="reqEffective" ControlToValidate="txtDateOfInitialNotice" ErrorMessage="*Enter an Date Of Initial Notice" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valAppeals" />
            <asp:CompareValidator id="CompareValidator2" runat="server" ValidationGroup="valAppeals"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateOfInitialNotice"  
                ErrorMessage="Select a valid Date Of Initial Notice" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true" /> 
        </div>
        <%--<td>
            <div class="pdmsLabel"><asp:Label ID="pdms_Effective" runat="server" /></div>
        </td>--%>
    </div>
    </div>

<br />
<asp:PlaceHolder runat="server" 
               ID="PlaceholderInitial"></asp:PlaceHolder>
<br />
<span class="pageHeader">Final Notice</span>
<div id="Table1" runat="server" style="width:70%;">
     <div class="row">
        <div class="col-sm-4 text-right">
            <asp:Label ID="Label1" runat="server" Text="Date Of Final Notice" CssClass="formLabel" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox ID="txtDateOfFinalNotice" runat="server" CssClass="formField formField" /><ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="txtDateOfFinalNotice" runat="server" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtDateOfFinalNotice" ErrorMessage="*Enter an Date Of Final Notice" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valAppeals" />
            <asp:CompareValidator id="CompareValidator1" runat="server" ValidationGroup="valAppeals"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateOfFinalNotice"  
                ErrorMessage="Select a valid Date Of Final Notice" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true" /> 
        </div>
        
    </div>

</div>
<br />
<asp:PlaceHolder runat="server" 
               ID="PlaceholderFinal"></asp:PlaceHolder>
<br />
<br />
<span class="pageHeader">Denial/Termination Information</span>
<div id="Table2" runat="server" style="width:70%;">
    <div class="row">
        <div class="col-sm-4 text-right">
            <asp:Label ID="Label3" runat="server" Text="Date Of Denial/Termination" CssClass="formLabel" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox ID="txtDateOfDenialOrTermination" runat="server" CssClass="formField formField" /><ajax:CalendarExtender ID="CalendarExtender3" TargetControlID="txtDateOfDenialOrTermination" runat="server" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtDateOfDenialOrTermination" ErrorMessage="*Enter an Date Of Denial/Termination Notice" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valAppeals" />
            <asp:CompareValidator id="CompareValidator3" runat="server" ValidationGroup="valAppeals"   
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateOfDenialOrTermination"  
                ErrorMessage="Select a valid Date Of Denial/Termination" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true" /> 
        </div>
       
    </div>
        <div class="row">
        <div class="col-sm-4 text-right">
            <asp:Label ID="Label5" runat="server" Text="Termination Reason" CssClass="formLabel" />
        </div>
        <div class="col-sm-8">
            <asp:DropDownList ID="ddlTermReason" runat="server"  AppendDataBoundItems="True" CssClass="formField formField" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="ddlTermReason" ErrorMessage="*Select Termination Reason" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valAppeals" />

        </div>
       
    </div>
    </div>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
