<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_Form1099Address, App_Web_yvhxe4ml" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/Address.ascx" TagName="Address" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/Form1099AddressHistory.ascx" TagPrefix="sh" TagName="History" %>

<script type="text/javascript">
    function CheckPhoneLength(sender, args) {
        var re = /\D/g; // Remove any characters that are not numbers
        var test = args.Value.replace(re, "");
        if (test === "") return true;

        var len = test.length;
        if (len !== 10)
            args.IsValid = false;
        if (test[0] === 0 || test[0] === 1 || test[3] === 0 || test[3] === 1)
            args.IsValid = false;
        return false;
    }
    function numericOnly(obj) {
        obj.value = obj.value.replace(/[^0-9]/g, '');
    }

    function CheckSSNLength(sender, args) {
        var re = /\D/g; // Remove any characters that are not numbers
        var test = args.Value.replace(re, "");
        if (test === "") return true;

        var len = test.length;
        if (len !== 9)
            args.IsValid = false;
        return false;
    }

    function ignoreValidation() {
        if (typeof Page_ClientValidate != 'undefined') {
            Page_ClientValidate('reset-validation');
            Page_BlockSubmit = false;
        }
        return true;
    }

    function removeDisabled() {
        $("#<%= btnCloseHistory.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    }
</script>
<div onmouseover="removeDisabled();">
<asp:ValidationSummary ID="vsForm1099Address" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valForm1099Address"/>
<asp:UpdatePanel ID="upHistory" runat="server">
    <ContentTemplate>
        <div role="dialog" aria-live="assertive" aria-labelledby="dialog1Title" aria-hidden="true" id="divDialog1">
            <ajax:modalpopupextender id="mpe" runat="server" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseHistory" popupcontrolid="pnlModal" targetcontrolid="ButtonDummy2" behaviorid="mpeSplHitory" />
            <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 60%;">
                <asp:Panel ID="pnlHeader" runat="server" CssClass="pnlHeader" HorizontalAlign="Left">
                    <div align="left">
                        &nbsp;&nbsp;
                     <h2 id="dialog1Title">
                         <asp:Label ID="lblSpHistoryTitle" runat="server" CssClass="history" ForeColor="White" Text="History" /></h2>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px">
                    <div class="container-fluid" style="text-align: left; padding: 15px;">
                        <div class="row">
                            <sh:history id="ucForm1099AddressHistory" runat="server" />
                        </div>
                        <div class="row">
                            <div class="btnBox" style="text-align: right;">
                                <asp:Button ID="btnCloseHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="OK" /><br />
                                <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkExcel_Click" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <asp:Button runat="server" ID="ButtonDummy2" Style="display: none" Text="”ButtonDummy2”" />
        </div>

    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExportHistory" />
    </Triggers>
</asp:UpdatePanel>

<div class="divHistoryAndAdd">
    <span aria-label="History">
        <asp:LinkButton TabIndex="0" ID="btnHistory" CommandName="History" runat="server" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" Text="History" ToolTip="History" Style="color: white; text-decoration: none;">
        History</asp:LinkButton>
    </span>
</div>
<asp:UpdatePanel ID="upProv" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        
        <div class="row">
                  <div class="col-sm-3  text-right"><asp:Label runat="server" AssociatedControlID="prov_billing" class="formLabel200">Same as Billing Location</asp:Label></div>
                <div class="col-sm-9" id="div2" runat="server">
               <asp:CheckBox ID="prov_billing" runat="server" Checked="false" aria-label="Same as Billing Location" CssClass="formFieldCheckBox" OnCheckedChanged="prov_Billing_CheckedChanged" AutoPostBack="true" style="border:none" />
                     </div>
                 </div>
        <div class="row">
                  <div class="col-sm-3  text-right"><asp:Label runat="server" AssociatedControlID="prov_override" class="formLabel200">Override Address Validation</asp:Label></div>
                <div class="col-sm-9" id="div1" runat="server">
               <asp:CheckBox ID="prov_override" runat="server" Checked="false" aria-label="Override Address Validation" CssClass="formFieldCheckBox" OnCheckedChanged="prov_Override_CheckedChanged" AutoPostBack="true" style="border:none" />
                     </div>
                 </div>
        <div class="row">
                <div class="col-sm-3  text-right"><asp:Label runat="server" AssociatedControlID="prov_Same" class="formLabel200">Same as Practice Location</asp:Label></div>
                <div class="col-sm-9" id="divchkchanged" runat="server">    
                <asp:CheckBox ID="prov_Same" runat="server" CssClass="formFieldCheckBox" OnCheckedChanged="prov_Same_CheckedChanged" AutoPostBack="true" style="border:none" />
                 </div>
            </div>
        <div id="ParentTable" runat="server">
            <uc:Address id="ucAddress" runat="server" ValidationGroup="valForm1099Address"></uc:Address>                   
        </div>

        <div class="row" id="divTaxArea" runat="server" style="width: 100%">
            <div class="col-sm-3  text-right">
                <asp:Label ID="Label1" runat="server" Text="IRS Tax Type" CssClass="formLabel200" />

            </div>
            <div class="col-sm-9">
                <fieldset>
                    <legend>
                         <asp:RadioButtonList ID="rbTaxTypeId" runat="server" CssClass="QstRadioList" RepeatDirection="Horizontal" Width="250px" AutoPostBack="false" AppendDataBoundItems="true">
                    <asp:ListItem Text="SSN" Value="15" selected="True"></asp:ListItem>
                    <asp:ListItem Text="FEIN" Value="16"></asp:ListItem>
                </asp:RadioButtonList>
                    </legend>
                </fieldset>
            </div>

            <div class="col-sm-3  text-right" runat="server">
                <asp:Label ID="lblTaxID" runat="server" CssClass="formLabel200" Text="IRS Tax ID"></asp:Label>
            </div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtTaxId" aria-Label="Tax Id" runat="server" CssClass="formField" onKeyUp="javascript:numericOnly(this);" MaxLength="9" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ValidationGroup="valForm1099Address"
                    ControlToValidate="txtTaxId" ErrorMessage="* Tax ID is required" Text="*" Display="Dynamic"
                    SetFocusOnError="true" Enabled="true" />
                <asp:CustomValidator ID="cvSSN" runat="server" SetFocusOnError="True" Display="Dynamic"
                    ControlToValidate="txtTaxId" ClientValidationFunction="CheckSSNLength"
                    ErrorMessage="* Tax ID is invalid" Text="*" ValidationGroup="valForm1099Address" />
            </div>
            <div style="display: none;">
                <asp:Label ID="Label10" runat="server" />
            </div>

            <div id="divDateDisplay" runat="server">
                <div class="col-sm-3 text-right">
                    <span class="formLabel150">
                        <asp:Label ID="lblEffectiveDate" runat="server" AssociatedControlID="txtEffectiveDate" Text="Effective Date" /></span>&nbsp;&nbsp;
                </div>

                <div class="col-sm-9 text-left" id="t">
                    <ajax:CalendarExtender
                        ID="calEffectiveDate"
                        runat="server"
                        Format="MM/dd/yyyy"
                        TargetControlID="txtEffectiveDate"
                        PopupPosition="BottomLeft"
                        CssClass="QstCalendarCSS"
                        PopupButtonID="imgPDMSStatusDate"
                        EnabledOnClient="true" />
                    <asp:TextBox ID="txtEffectiveDate" runat="server" CssClass="formField" AutoCompleteType="Disabled" />
                    <asp:CompareValidator
                        ID="dateValidator"
                        runat="server"
                        Type="Date"
                        Operator="DataTypeCheck"
                        ControlToValidate="txtEffectiveDate"
                        ErrorMessage="Select a valid Effective Date"
                        Text="*"
                        Display="Dynamic"
                        ValueToCompare="MM/dd/yyyy"
                        ValidationGroup="valForm1099Address"
                        SetFocusOnError="true"> 
                    </asp:CompareValidator>
                    <asp:Image ID="imgPDMSStatusDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="18px" AlternateText="Effective Date Calendar" />
                </div>


                <div class="col-sm-3 text-right">
                    <span class="formLabel150">
                        <asp:Label ID="lblEndDate" runat="server" Text="End Date" /></span>&nbsp;&nbsp;
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField" MaxLength="100" />
                </div>
            </div>
            

            <div class="col-sm-3  text-right">
                <asp:Label ID="Label17" runat="server" Text="Tax Exempt" CssClass="formLabel200" />
            </div>
            <div class="col-sm-9">
                <fieldset>
                    <legend>
                         <asp:RadioButtonList ID="rblTaxExempt" runat="server" CssClass="QstRadioList" RepeatDirection="Horizontal" Width="250px" AutoPostBack="false" AppendDataBoundItems="true">
                    <asp:ListItem Text="Yes" Value="1" selected="True"></asp:ListItem>
                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
                    </legend>
                </fieldset>
               
            </div>


            <div class="col-sm-3  text-right">
                <asp:Label ID="Label3" runat="server" Text="W9 Form" CssClass="formLabel200" />
            </div>
            <div class="col-sm-9">
                <fieldset>
                    <legend>
                         <asp:RadioButtonList ID="rbFormW9" runat="server" CssClass="QstRadioList" RepeatDirection="Horizontal" Width="250px" AutoPostBack="false" AppendDataBoundItems="true">
                    <asp:ListItem Text="Yes" Value="1" selected="True"></asp:ListItem>
                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
                    </legend>
                </fieldset>
               
            </div>

            <div class="col-sm-3  text-right">
                <asp:Label ID="Label4" runat="server" Text="Form 147" CssClass="formLabel200" />
            </div>
            <div class="col-sm-9">
                <fieldset>
                    <legend>
                        <asp:RadioButtonList ID="rblForm147" runat="server" CssClass="QstRadioList" RepeatDirection="Horizontal" Width="250px" AutoPostBack="false" AppendDataBoundItems="true">
                    <asp:ListItem Text="Yes" Value="1" selected="True"></asp:ListItem>
                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
                    </legend>
                </fieldset>
                
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

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
                <telerik:GridBoundColumn DataField="ADDRESS1"            HeaderText="Address"        SortExpression="ADDRESS1" />
                <telerik:GridBoundColumn DataField="EMAIL1"              HeaderText="Email"          SortExpression="EMAIL1" />
                <telerik:GridBoundColumn DataField="PHONE1"              HeaderText="Phone"          SortExpression="PHONE1" />
                <telerik:GridBoundColumn DataField="ADR_EFFECTIVE_DATE"  HeaderText="Effective Date" SortExpression="ADR_EFFECTIVE_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
                <telerik:GridBoundColumn DataField="ADR_END_DATE"        HeaderText="End Date"       SortExpression="ADR_END_DATE"       DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
                <telerik:GridBoundColumn DataField="TAX_ID_TYPE"         HeaderText="Tax ID Type"    SortExpression="TAX_ID_TYPE" />
                <telerik:GridBoundColumn DataField="IRS_TAX_ID"          HeaderText="Tax ID"         SortExpression="IRS_TAX_ID" />
                <telerik:GridBoundColumn DataField="IS_TAX_EXEMPT"       HeaderText="Exempt"         SortExpression="IS_TAX_EXEMPT" />
                <telerik:GridBoundColumn DataField="IS_FORM_W9"          HeaderText="Form W9"        SortExpression="IS_FORM_W9" />
                <telerik:GridBoundColumn DataField="IS_FORM_147"         HeaderText="Form 147"       SortExpression="IS_FORM_147" />
                <telerik:GridBoundColumn DataField="UserName"            HeaderText="User Name"      SortExpression="UserName" />
                <telerik:GridBoundColumn DataField="DateOfAction"        HeaderText="Update Date"    SortExpression="DateOfAction"       DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
</div>