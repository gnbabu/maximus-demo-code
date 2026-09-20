<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_Certifications, App_Web_wbqq1lcm" %>
<%@ Register Src="~/UserControls/FormField.ascx" TagPrefix="uc" TagName="FormField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<%@ register src="~/PopupControls/CertificationsHistory.ascx"      tagprefix="uc"     tagname="CertificationsHistory" %>
<script  type="text/javascript">

    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^ a-zA-Z0-9]/g, '');
    }
    function removeDisabled() {
        $("#<%= btnModalOk.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    }
</script>
<style type="text/css">
    .formLabel {
        width: 140px !important;
    }
    /*input[type='text']
    {
        border:solid 1px black!important;
        width:250px;
    }*/


    .modalPopup, #ctl00_MainContent_ucLicensesClassifications_ucMessageModal_pnlModal {
        height: 320px;
        width: 785px;
    }

    #labelTable > tbody > tr > td > span {
        width: 150px;
    }

    #provTable > tbody > tr > td > input[type='text'], #pdmsTable > tbody > tr > td > input[type='text'] {
        width: 250px;
    }

    #provTable > tbody > tr > td > span {
        border: none;
    }

    .dataTable > tbody > tr > td {
        height: 25px;
    }

    .pdmsLabel {
        width: 250px;
    }

    table {
        text-align: left !important;
    }

    .failureNotification {
        text-align: left !important;
    }
</style>
<div onmouseover="removeDisabled();">
<div>
    <asp:ValidationSummary ID="DeaValidationSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="Certifications" />
    <asp:ValidationSummary ID="DeaADDValidationSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="ADDCertifications" />
</div>
<asp:Label ID="lblErrorMsg" runat="server" Text="" ForeColor="red" />
<div id="ParentTable" runat="server">

    <br />
            <h2 class="pageHeader" style="text-decoration: underline">DEA Question</h2>
            <br />
            <br />
            <br />
     <div class="row">
        <div class="col-sm-4 text-right">
            <asp:Label ID="Label6" runat="server" Text="Do you have a current DEA registration?" CssClass="formLabel300" />
        </div>
        <div class="col-sm-8" style="display: flex; flex-wrap: wrap; justify-content: space-evenly">
            <fieldset>
                <legend>
                      <asp:RadioButtonList ID="rblCurrentDEARegistration" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblCurrentDEARegistration_SelectedIndexChanged">
                      <asp:ListItem Value="Y">Yes</asp:ListItem>
                      <asp:ListItem Value="N">No</asp:ListItem>
                  </asp:RadioButtonList>
                </legend>
            </fieldset>
            <asp:RequiredFieldValidator runat="server" ID="rfvCurrentDEARegistration" Visible="false" ControlToValidate="rblCurrentDEARegistration" ErrorMessage="* Current DEA Registration is required." Text="*" Display="None" ValidationGroup="Certifications" />
            
        </div>
    </div>
    
    <span style="font:bold; font-weight:700;">If Yes, make selection and Add New for each DEA and waiver including Waiver 2000.</span>
    <br />
    <span>If No, make selection and fill in remaining information. </span>
    <br />
   <div id="divAddFederalDEA" runat="server" visible="false">
        <div class="row">
        <div class="col-sm-4">
            <asp:Label ID="Label1" runat="server" Text="DEA Number" CssClass="formLabel300" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox ID="prov_Number" runat="server" aria-Label="DEA Number" CssClass="formField" MaxLength="9" AutoPostBack="true"  onKeyUp="javascript:alphanumericOnly(this);" OnTextChanged="prov_Number_TextChanged" />
            <asp:requiredfieldvalidator runat="server" id="rvdeanumber" controltovalidate="prov_number" errormessage="* DEA number is required." text="*" display="dynamic" setfocusonerror="true" validationgroup="Certifications" />
            <%--<asp:customvalidator id="custval_lc72" runat="server" controltovalidate="prov_number" onservervalidate="validatelc72" display="dynamic" validationgroup="certifications" errormessage="* all fields must be filled out" text="*" />--%>
        </div>
        <div style="display:none;">
            <div class="pdmsLabel">
                <asp:Label ID="pdms_Number" runat="server" /></div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4">
            <asp:Label ID="Label4" runat="server" Text="DEA State" CssClass="formLabel300" />
        </div>
        <div class="col-sm-8">
            <asp:DropDownList ID="prov_State" style="min-width:333px;" AutoPostBack="false" aria-label="DEA State" ValidationGroup="Certifications" runat="server" CssClass="formDropDown"></asp:DropDownList>
            <asp:RequiredFieldValidator runat="server" ID="req2" ControlToValidate="prov_State" ErrorMessage="* State is required." Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="Certifications" />
        </div>
        <div style="display:none;">
            <div class="pdmsLabel">
                <asp:Label ID="pdms_State" runat="server" /></div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4">
            <asp:Label ID="Label2" runat="server" Text="Issue Date" CssClass="formLabel300" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox ID="prov_Start" runat="server" aria-Label="Issue Date" CssClass="formField" />
            <ajax:CalendarExtender ID="calStart" TargetControlID="prov_Start" runat="server" />
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="prov_Start" ErrorMessage="* Issue Date is required." Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="Certifications" />
            <asp:CompareValidator ID="startDateValidator" runat="server" ValidationGroup="Certifications" Type="Date" Operator="DataTypeCheck" ControlToValidate="prov_Start" ErrorMessage="Select a valid Issue Date" Text="*" ValueToCompare="MM/dd/yyyy" SetFocusOnError="true"> 
            </asp:CompareValidator>
            <asp:CustomValidator ID="CustVal_LC74" runat="server" ControlToValidate="prov_Start" OnServerValidate="ValidateLC74" Display="Static" ValidationGroup="Certifications" ErrorMessage="* Future dates not allowed." Text="*" />
        </div>
        <div style="display:none;">
            <div class="pdmsLabel">
                <asp:Label ID="pdms_Start" runat="server" /></div>
        </div>
    </div>
    <div class="row">
        <div class="col-sm-4">
            <asp:Label ID="Label3" runat="server" Text="Expiration Date" CssClass="formLabel300" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox ID="prov_End" runat="server" aria-Label="Expiration Date" CssClass="formField" />
            <ajax:CalendarExtender ID="calEnd" TargetControlID="prov_End" runat="server" />
            <asp:CompareValidator ID="endDateValidator" runat="server" ValidationGroup="Certifications" Type="Date" Operator="DataTypeCheck" ControlToValidate="prov_End"
                ErrorMessage="Select a valid Expiration Date" Text="*" ValueToCompare="MM/dd/yyyy" SetFocusOnError="true"> 
            </asp:CompareValidator>
            <asp:CompareValidator ID="CompareStartAndEndDateValidator" runat="server" ValidationGroup="Certifications" Type="Date" Operator="LessThan" ControlToValidate="prov_Start" ControlToCompare="prov_End"
            ErrorMessage="* Expiration Date should be greater than Issue Date" Text="*" SetFocusOnError="true"> 
            </asp:CompareValidator>

        </div>
        <div style="display:none;">
            <div class="pdmsLabel">
                <asp:Label ID="pdms_End" runat="server" /></div>
        </div>
    </div>
        <div class="row">
        <div class="col-sm-4">
            <asp:Label ID="Label8" runat="server" Text="DEA Status" CssClass="formLabel300" />
        </div>
        <div class="col-sm-8">
           <asp:DropDownList ID="ddldeastatus" aria-Label="DEA Status" runat="server" CssClass="formDropDown" style="min-width:333px;">
               <asp:ListItem Value="Active" Text="Active" Selected="True">Active</asp:ListItem>
               <asp:ListItem Value="Inactive" Text="Inactive">Inactive</asp:ListItem>
           </asp:DropDownList>
        
        </div>
        <div style="display:none;">
            <div class="pdmsLabel">
                <asp:Label ID="Label10" runat="server" /></div>
        </div>
    </div>

   </div>
    <div id="divDEAQuestionMapping" runat="server" visible="false">
    <div class="row">
        <div class="col-sm-4">
            <asp:Label ID="Label5" runat="server" Text="Name of Provider that prescribes on your behalf" CssClass="formLabel300" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox ID="txtPrescribeProvider" runat="server" onKeyUp="javascript:alphanumericOnly(this);"  CssClass="formField"/>
            <asp:requiredfieldvalidator runat="server" id="reqPrescribeProvider" controltovalidate="txtPrescribeProvider" errormessage="* Name of Provider is required." text="*" display="dynamic" setfocusonerror="true" validationgroup="Certifications" />
            

        </div>
        
    </div>
    <div class="row">
        <div class="col-sm-4">
            <asp:Label ID="Label7" runat="server" Text="DEA Number of the prescribing Provider" CssClass="formLabel300" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox ID="txtPrescribeProviderDEA" runat="server"  onKeyUp="javascript:alphanumericOnly(this);" CssClass="formField"/>
            <asp:requiredfieldvalidator runat="server" id="reqPrescribeProviderDEA" controltovalidate="txtPrescribeProviderDEA" errormessage="* DEA Number is required." text="*" display="dynamic" setfocusonerror="true" validationgroup="Certifications" />
        </div>
        
    </div>
    <div class="row">
        <div class="col-sm-4">
            <asp:Label ID="Label9" runat="server" Text="DEA State of the prescribing Provider" CssClass="formLabel300" />
        </div>
        <div class="col-sm-8">
           <asp:DropDownList ID="ddlPrecribeProviderState" AutoPostBack="false"  runat="server" CssClass="formDropDown" style="min-width:333px"></asp:DropDownList>
           <asp:requiredfieldvalidator runat="server" id="reqPrecribeProviderState" controltovalidate="ddlPrecribeProviderState" errormessage="* DEA State is required." text="*" display="dynamic" setfocusonerror="true" validationgroup="Certifications" />
        </div>
        
    </div>
    <div class="row">
        <div class="col-sm-4">
            <asp:Label ID="Label11" runat="server" Text="Prescribing Comments" CssClass="formLabel300" />
        </div>
        <div class="col-sm-8">
            <asp:TextBox ID="txtPrecibeComments" TextMode="MultiLine" MaxLength="150" runat ="server" onKeyUp="javascript:alphanumericOnly(this);"  CssClass="formField" />
            <%--OHPNM-4099 On selection on NO, prescribe comments should be optional--%>
            <%--<asp:requiredfieldvalidator runat="server" id="reqPrecibeComments" controltovalidate="txtPrecibeComments" errormessage="* Prescribing Comments is required." text="*" display="dynamic" setfocusonerror="true" validationgroup="Certifications" />--%>
        </div>
       
    </div>
</div>
</div>

 <div class="divGrid">
        <asp:GridView runat="server" ID="grdCertification" AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No records found" OnRowCommand="grdCertificationk_RowCommand" >
            <Columns>
                <%--<asp:BoundField DataField="TITLE" HeaderText="Title" />--%>
                <asp:BoundField DataField="DEA_NUMBER" HeaderText="DEA Number" />

                <asp:BoundField DataField="DEA_STATE" HeaderText="DEA State" />
                <asp:BoundField DataField="DEA_EFF_DATE" HeaderText="Issue Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="DEA_END_DATE" HeaderText="Expiration Date" DataFormatString="{0:MM/dd/yyyy}" />
                  
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnReg_DEA_ID" runat="server" Value='<%# Eval("REG_DEA_ID")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnMODIFIED_STATUS_TYPE_ID" runat="server" Value='<%# Eval("MODIFIED_STATUS_TYPE_ID")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:templatefield headertext="Contact Details">
            <itemtemplate>
              <asp:label id="lblContact" text= '<%# GetContactDetails(Eval("CONTACT_NAME"),Eval("CONTACT_EMAIL_ADDRESS"),Eval("CONTACT_PHONE_NUMBER"))%>' runat="server"/> 
              
            </itemtemplate>
          </asp:templatefield>--%>
                <asp:TemplateField ItemStyle-Width="2%" HeaderText="Edit">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditCertificationDetails" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteCertificationDetails" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" AlternateText="Delete"
                            Visible="<%# CanUserViewDelete(((GridViewRow) Container).RowIndex)  %>" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
     <div style="text-align:right; clear: both;">
        <asp:ImageButton ID="btnAddCertification" runat="server" AlternateText="Add Certification" Visible="false" ImageUrl="~/Images/add.png" CommandName="WorkHistoryAdd" OnCommand="lbtnAdd_Click" ToolTip="Add" />
        <br /><br /><span aria-label="Licenses History">
            <asp:LinkButton TabIndex="0" ID="btnCertificationsHistory" CommandName="Certifications" runat="server" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" Text="History" ToolTip="History" Style="color: white; text-decoration: none;">
                <span class="glyphicon glyphicon-book" style="padding-right:7px;"></span>History 
            </asp:LinkButton></span>
        
    </div>
    </div>
<div style="width: 100%; text-align: right; display: none;">
    <asp:HiddenField ID="hdnRowCount" runat="server" />
    <asp:Button ID="lnkExcel" CommandName="Export" OnClick="lnkExcel_Click" runat="server" ToolTip="Excel" Visible="true" Text="Export"/>
    <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
    </div>
    <telerik:RadGrid ID="grdDEAHistory" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
        AutoGenerateColumns="False" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
            <Excel Format="Biff" />
        </ExportSettings>
        <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
            DataKeyNames="INDEX, DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
            <Columns>
                <telerik:GridBoundColumn DataField="Operation" HeaderText="Operation"    SortExpression="Operation" />
                <telerik:GridBoundColumn DataField="DEA_NUMBER" HeaderText="DEA Number"  SortExpression="DEA_NUMBER" />
                <telerik:GridBoundColumn DataField="UserName" HeaderText="User Name"     SortExpression="UserName" />
                <telerik:GridBoundColumn DataField="DEA_STATE" HeaderText="DEA State"    SortExpression="DEA_STATE" />
                <telerik:GridBoundColumn DataField="DEA_EFF_DATE" HeaderText="Effective Date" DataFormatString="{0:MM/dd/yyyy}"   SortExpression="DEA_EFF_DATE" />
                <telerik:GridBoundColumn DataField="DEA_END_DATE" HeaderText="Expiration Date" DataFormatString="{0:MM/dd/yyyy}"  SortExpression="DEA_END_DATE" />
                <telerik:GridBoundColumn DataField="DateOfAction" HeaderText="Update Date" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" SortExpression="DateOfAction" HtmlEncode="False" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>

<style type="text/css">
    .modalPopup
    {
        height: auto;
        left: 20% !important;
        top: 30% !important;
        position: fixed !important;
    }
</style>
<asp:panel id="upDEAHistory" runat="server">
    <ajax:modalpopupextender id="mpe" runat="server" popupcontrolid="pnlModal" targetcontrolid="ButtonDummy2"
        backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnlModal" CancelControlID="btnModalOk">
    </ajax:modalpopupextender>
    <asp:panel id="pnlModal" runat="server" cssclass="modalPopup" style="display: none; padding: 20px; width: 1000px;">
        <asp:panel id="pnlHeader" cssclass="pnlHeader" runat="server" horizontalalign="Left" style="width: 950px">
            <div align="left">
                &nbsp;&nbsp;
                <asp:label id="lblTitle" cssclass="bodyTextBold" runat="server" text="Title" forecolor="White" />
            </div>
        </asp:panel>
        <asp:panel id="pnlMain1" runat="server" style="padding: 10px; width: 950px !important; margin-left: 10px;">
            <div>
                <uc:certificationshistory id="ucCertificationsHistory" runat="server" />
            </div>
        </asp:panel>
        <div class="btnBox" style="padding: 10px">
            <asp:button runat="server" id="btnModalOk" text="OK" cssclass="buttonBox" causesvalidation="false" />
            <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkExcel_Click" />
        </div>
    </asp:panel>
    <asp:button runat="server" id="ButtonDummy2" style="display: none" text="”ButtonDummy2”" />
</asp:panel>
   
<asp:HiddenField ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
<asp:PlaceHolder runat="server" ID="PlaceholderUploadDEA"></asp:PlaceHolder>
</div>
