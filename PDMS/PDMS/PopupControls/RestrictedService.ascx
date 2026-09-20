<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_RestrictedService" Codebehind="RestrictedService.ascx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/PopupControls/RestrictedServiceHistory.ascx" TagPrefix="uc" TagName="RestrictedServiceHistory" %>
<script type="text/javascript">
    function removeDisabled() {
        $("#<%= btnCloseHistory.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    }
</script>
<div onmouseover="removeDisabled();">
<div id ="AddressTable" >
<%-- <asp:UpdatePanel ID="updatepanelDummy" runat="server"> 
 <ContentTemplate>--%>

    
<asp:UpdatePanel ID="upRestrictedService" runat="server" UpdateMode="Conditional">

    <ContentTemplate>
        
 


<div style="padding:5px;">
    <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
    <asp:ValidationSummary ID="vsNewRestrcitedService" DisplayMode="List" runat="server" CssClass="failureNotification" validationGroup="RestrcitedService" ShowSummary="true"  />
    <div class="pg-hint" style="padding-right:4px;">* Designates a required field</div><br /><br />
    <div style="width:auto;">
           
            <div class="row">
                <div class="col-sm-4 text-right"><asp:Label CssClass="formLabel wd200" runat="server" ID="lblRSEffectiveDate">Effective Date*</asp:Label></div>
                <div class="col-sm-8 text-left"><asp:TextBox ID="txtEffectiveDate" runat="server" CssClass="formField wd200" MaxLength="10" />
                    <ajax:CalendarExtender ID="ceEffectiveDate" TargetControlID="txtEffectiveDate" runat="server" />
                    <asp:RequiredFieldValidator ID="valEffectiveDateReqd" runat="server" SetFocusOnError="true" ValidationGroup="RestrcitedService" Text="*"
                        ControlToValidate="txtEffectiveDate" ErrorMessage="* Effective Date is required." Display="Dynamic"  Enabled="true" />
                    <asp:CompareValidator id="cvEffectiveDate" runat="server" ValidationGroup="RestrcitedService"   
                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEffectiveDate"   Enabled="true"
                        ErrorMessage="* A valid Effective Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                        SetFocusOnError="true"> 
                    </asp:CompareValidator>
                </div>

            </div>
             <div class="row">
                <div class="col-sm-4 text-right"><asp:Label CssClass="formLabel wd200" runat="server" ID="lblRSEndDate">End Date</asp:Label></div>
                <div class="col-sm-8 text-left"><asp:TextBox ID="txtRSEndDate" runat="server" CssClass="formField wd200" MaxLength="10" />
                    <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtRSEndDate" runat="server" />
                   
                    <asp:CompareValidator id="cvEndDate" runat="server" ValidationGroup="RestrcitedService"   
                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtRSEndDate"   Enabled="true"
                        ErrorMessage="* A valid End Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                        SetFocusOnError="true"> 
                    </asp:CompareValidator>
                </div>
             </div>
         <div class="row">
        <div class="col-sm-4  text-right">
            <asp:Label ID="lblRSStatus" runat="server" Text="* Restricted Status:" CssClass="formLabel wd200"></asp:Label>
            </div>
        <div class="col-sm-8">
          <asp:DropDownList runat="server" ID="ddlRSStatus" CssClass="formDropDown"  EnableViewState="true" 
                AutoPostBack="true" AppendDataBoundItems="True" >
            </asp:DropDownList>
           <asp:RequiredFieldValidator runat="server" ID="RfvddlRSStatus" SetFocusOnError="true" 
            ValidationGroup="RestrcitedService" ControlToValidate="ddlRSStatus" ErrorMessage="* Restricted Status is required" Text="*" Display="Dynamic" InitialValue="0" />
         </div>
    </div>
    <div class="row">
        <div class="col-sm-4  text-right">
            <asp:Label ID="lblReviewType" runat="server" Text="* Review Type:" CssClass="formLabel wd200"></asp:Label>
            </div>
        <div class="col-sm-8">
          <asp:DropDownList runat="server" ID="ddlReviewType" CssClass="formDropDown" AutoPostBack="true" AppendDataBoundItems="True"  EnableViewState="true">
            </asp:DropDownList>
           <asp:RequiredFieldValidator runat="server" ID="RfvddlRSReviewType" SetFocusOnError="true" 
            ValidationGroup="RestrcitedService" ControlToValidate="ddlReviewType" ErrorMessage="* Review type is required" Text="*" Display="Dynamic" InitialValue="0" />
         </div>
    </div>
         <div class="row">
        <div class="col-sm-4  text-right">
            <asp:Label ID="Label1" runat="server" Text="* Include/Exclude:" CssClass="formLabel wd200"></asp:Label>
            </div>
        <div class="col-sm-8">
          <asp:DropDownList runat="server" ID="ddlIncludeExclude" CssClass="formDropDown"
                AutoPostBack="true" AppendDataBoundItems="True"  EnableViewState="true" >
            </asp:DropDownList>
           <asp:RequiredFieldValidator runat="server" ID="RfvIncludeExclude" SetFocusOnError="true" 
            ValidationGroup="RestrcitedService" ControlToValidate="ddlIncludeExclude" ErrorMessage="* Include/Exclude is required" Text="*" Display="Dynamic" InitialValue="0" />
         </div>
    </div>
    <div class="row">
        <div class="col-sm-4  text-right">
            <asp:Label ID="lblRestrict" runat="server" Text="* Restrict:" CssClass="formLabel wd200"></asp:Label>
            </div>
        <div class="col-sm-8">
          <asp:DropDownList runat="server" ID="ddlRestrict" CssClass="formDropDown"
                AutoPostBack="true" AppendDataBoundItems="True"  EnableViewState="true">
                <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                <asp:ListItem Text="No" Value="False"></asp:ListItem>
            </asp:DropDownList>
           <asp:RequiredFieldValidator runat="server" ID="RfvddlRestrict" SetFocusOnError="true" 
            ValidationGroup="RestrcitedService" ControlToValidate="ddlRestrict" ErrorMessage="* Restrict is required" Text="*" Display="Dynamic" InitialValue="0" />
         </div>
    </div>
    <div class="row">
        <div class="col-sm-4  text-right">
            <asp:Label ID="Label2" runat="server" Text="* Review Reason:" CssClass="formLabel wd200"></asp:Label>
            </div>
        <div class="col-sm-8">
          <asp:DropDownList runat="server" ID="ddlReviewReason" CssClass="formDropDown" EnableViewState="true"
                AutoPostBack="true" AppendDataBoundItems="True">
            </asp:DropDownList>
           <asp:RequiredFieldValidator runat="server" ID="RfvReviewReason" SetFocusOnError="true" 
            ValidationGroup="RestrcitedService" ControlToValidate="ddlReviewReason" ErrorMessage="* Review reason is required" Text="*" Display="Dynamic" InitialValue="0" />
         </div>
    </div>
  
        </div>
        
    </div>
     <p style="color: red"><asp:Label runat="server" ID="lblmessage" Text=""></asp:Label></p>
    <br />
        <div id="divSaveBox" class="btnBox" runat="server" style="padding-right:10px" >
                <asp:Button id="btnSave"  runat="server" Text="Save" CssClass="buttonBox" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="RestrcitedService" />
                <asp:Button id="btnCancel"  runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
                         
            </div> 


    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlReviewType" />
         <asp:AsyncPostBackTrigger ControlID="ddlRestrict" />
         <asp:AsyncPostBackTrigger ControlID="ddlReviewReason" />
         <asp:AsyncPostBackTrigger ControlID="ddlIncludeExclude" />
        <asp:AsyncPostBackTrigger ControlID="ddlRSStatus" />
    </Triggers>
</asp:UpdatePanel>
<asp:Panel ID="upRestrictedServiceHistory" runat="server">
    <ajax:ModalPopupExtender ID="mpe" runat="server" BackgroundCssClass="modalBackground" CancelControlID="btnCloseHistory" PopupControlID="pnlModal" PopupDragHandleControlID="pnlModal" TargetControlID="ButtonDummy3" />
    <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 1000px;">
        <asp:Panel ID="pnlHeader" runat="server" CssClass="pnlHeader" HorizontalAlign="Left">
            <div align="left">
                &nbsp;&nbsp;<asp:Label ID="lblTitle" runat="server" CssClass="bodyTextBold" ForeColor="White" Text="Title" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px">
            <div class="container-fluid" style="text-align: left; padding: 15px;">
                <div class="row">
                    <uc:RestrictedServiceHistory ID="ucRestrictedServiceHistory" runat="server" />
                </div>
                <div class="row">
                    <div class="btnBox" style="text-align: right;">
                        <asp:Button ID="btnCloseHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="OK" />
                        <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkExcel_Click" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Button runat="server" aria-Label="Dummy Button" ID="ButtonDummy3" Style="display: none" Text=”ButtonDummy3” />
</asp:Panel>
    <div style="width: 100%; text-align: right; display: none;">
    <asp:HiddenField ID="hdnRowCount" runat="server" />
    <asp:Button ID="lnkExcel" CommandName="Export" OnClick="lnkExcel_Click" runat="server" ToolTip="Excel" Visible="true" Text="Export"/>
    <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
    </div>
    <telerik:RadGrid ID="grd" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
        AutoGenerateColumns="true" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
            <Excel Format="Biff" />
        </ExportSettings>
        <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
            DataKeyNames="INDEX, DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
            <Columns>
                <telerik:GridBoundColumn DataField="OPERATION"           HeaderText="Operation"      SortExpression="Operation" />
                <telerik:GridBoundColumn DataField="IS_EXCLUDE"          HeaderText="Exclude/Include"    SortExpression="IS_EXCLUDE" />
                <telerik:GridBoundColumn DataField="REVIEWTYPE"          HeaderText="Review Type"        SortExpression="REVIEWTYPE" />
                <telerik:GridBoundColumn DataField="REVIEWREASON"        HeaderText="Review Reason"      SortExpression="REVIEWREASON" />
                <telerik:GridBoundColumn DataField="IS_RESTRICT"         HeaderText="Restrict"           SortExpression="IS_RESTRICT" />
                <telerik:GridBoundColumn DataField="EFFECTIVE_DATE"      HeaderText="Effective Date"     SortExpression="EFFECTIVE_DATE" DataFormatString="{0:MM/dd/yyyy}" />
                <telerik:GridBoundColumn DataField="END_DATE"            HeaderText="End Date"           SortExpression="END_DATE" DataFormatString="{0:MM/dd/yyyy}" />
                <telerik:GridBoundColumn DataField="STATUS"              HeaderText="Status"             SortExpression="STATUS" />
                <telerik:GridBoundColumn DataField="UserName" HeaderText="LastModifiedUser" SortExpression="UserName" />
                <telerik:GridBoundColumn DataField="LAST_MODIFIED_DATE_TIME" HeaderText="LastModifiedDateTime" SortExpression="LAST_MODIFIED_DATE_TIME" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />

            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>

<%--</ContentTemplate> 
</asp:UpdatePanel>--%>

    </div>
    </div>