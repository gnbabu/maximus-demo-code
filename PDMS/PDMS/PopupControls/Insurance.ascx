<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_Insurance" Codebehind="Insurance.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/UserControls/Address.ascx" TagName="Address" TagPrefix="uc" %>
<%--<%@ Register Src="~/UserControls/UploadControl.ascx" TagName="UploadControl" TagPrefix="uc" %>--%>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/InsuranceHistory.ascx" TagPrefix="uc" TagName="InsuranceHistory" %>

<script type="text/javascript">
    function numericOnly(obj) {
        obj.value = obj.value.replace(/[^0-9]/g, '');
    }
    function removeDisabled() {
        $("#<%= btnModalOk.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    }

</script>
<div onmouseover="removeDisabled();">
<div>
    <asp:ValidationSummary ID="vsInsurance" runat="server" DisplayMode="List" ValidationGroup="valInsurance" CssClass="failureNotification"/>
</div>
<br />
<div id="pnlInsuranceGridDetails" runat="server">
    <div class="divGrid">
        <asp:GridView runat="server" ID="grdInsurance" AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No records found" OnRowCommand="grdInsurance_RowCommand" OnRowDataBound="grdInsurance_RowDataBound">
            <Columns>
                
                 <asp:TemplateField HeaderText="Carrying malpractice insurance?">
                    <ItemTemplate>
                        <asp:Label ID="lblIsMalpracticeClaimed" runat="server" Text='<%# Eval("IsMalpracticeClaimed")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="POLICY_NUMBER" HeaderText="Policy Number" />
                <asp:BoundField DataField="EFFECTIVE_DATE" HeaderText="Effective Date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField DataField="EXPIRATION_DATE" HeaderText="Expiration Date" DataFormatString="{0:MM/dd/yyyy}" />
               
                <asp:BoundField DataField="PolicyHolder" HeaderText="Policy Holder" />
                <asp:BoundField DataField="CoverageAmountPerOccurance" HeaderText="Coverage Account Per Occurence" />
                <asp:BoundField DataField="CoverageAmountPerAggregate" HeaderText="Coverage Account Per Aggregate" />
                <asp:BoundField DataField="MalpracticeInsuranceReason" HeaderText="Explanation regarding malpractice insurance" />
                <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnRegAddressId" runat="server" Value='<%# Eval("REG_ADDRESS_ID")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:templatefield headertext="Contact Details">
            <itemtemplate>
              <asp:label id="lblContact" text= '<%# GetContactDetails(Eval("CONTACT_NAME"),Eval("CONTACT_EMAIL_ADDRESS"),Eval("CONTACT_PHONE_NUMBER"))%>' runat="server"/> 
              
            </itemtemplate>
          </asp:templatefield>--%>
                <asp:TemplateField HeaderText="EDIT" ItemStyle-Width="2%" >
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditWorkDetails" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    <div class="divHistoryAndAdd">
        <asp:ImageButton ID="btnAddWorkItem" AlternateText="add new" runat="server" ImageUrl="~/Images/add.png" CommandName="WorkHistoryAdd" OnCommand="lbtnAdd_Click" ToolTip="Add" />
        <%--<asp:ImageButton ID="btnWorkHistory" CommandName="WorkHistory" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" ToolTip="History" />--%>
        <br />
        <span aria-label="Insurance History">
            <asp:LinkButton TabIndex="0" ID="btnHistory" CommandName="History" runat="server" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" Text="History" ToolTip="History" Style="color: white; text-decoration: none;">
                <span class="glyphicon glyphicon-book" style="padding-right:7px;"></span>History 
            </asp:LinkButton></span>
    </div>
    <br />
</div>
<div id="divMalPracticeClaims" runat="server" visible="false">
    <div class="row">
        <div class="col-sm-4"><span class="formLabel200">Do you carry malpractice insurance?</span></div>
        <div class="col-sm-4  text-left">
            <asp:RadioButtonList ID="rblMalpracticeInsurace" aria-label="Do you carry malpractice insurance"  runat="server" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblMalpracticeInsurace_SelectedIndexChanged" AppendDataBoundItems="true">
                <asp:ListItem Value="1">Yes</asp:ListItem>
                <asp:ListItem Value="0">No</asp:ListItem>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator runat="server" ID="rfvMalpraciceClaims" ControlToValidate="rblMalpracticeInsurace" ErrorMessage="* Please select malpractice insurance." Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valInsurance" />
        </div>

    </div>
    <div class="row" runat="server" id="divMalpracticeReason" visible="false">
        <br />
        <div class="col-sm-offset-2"><span class="formLabel200" style="font-style: italic">If No, please provide explanation below.</span></div>
        <br />
        <br />
        <div class="col-sm-5 "><span class="formLabel200">Please provide an explanation regarding malpractice insurance</span></div>
        <div class="col-sm-7">
            <asp:TextBox ID="txtMalpracticeReason" runat="server" MaxLength="200" CssClass="formField" TextMode="MultiLine" Columns="100" Rows="7" />
            <asp:RequiredFieldValidator runat="server" ID="rfvMalpracticrReason" ControlToValidate="txtMalpracticeReason" ErrorMessage="* Enter an explanation regarding malpractice insurance." Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valInsurance" />
        </div>

    </div>
</div>
<br />

<div id="divProfessionalInsurance" runat="server" style="width: 100%; margin-left: 10%;" visible="false">

    <div id="ParentTable" runat="server">

        <div class="row">
            <div class="col-sm-3  text-right"><span class="formLabel200">Self Insured?</span></div>
            <div class="col-sm-9">
                <asp:DropDownList ID="ddlSelfInsured" aria-label="Self Insured" runat="server" CssClass="formField">
                    <asp:ListItem Value="1" Selected="True">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:DropDownList>
                
            </div>
           
        </div>
        <div class="row">
            <div class="col-sm-3  text-right"><span class="formLabel200">Policy Number*</span></div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtPolicyNumber" aria-label="Policy Number"  runat="server" CssClass="formField" MaxLength="80" />
                <asp:RequiredFieldValidator runat="server" ID="reqPolicyNumber"
                    ControlToValidate="txtPolicyNumber" ErrorMessage="*Enter Policy Number" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valInsurance" />
            </div>
            <%--        <td>effe
            <div class="pdmsLabel">
                <asp:Label ID="lblPDMSPolicyNumber" runat="server" /></div>
        </td>--%>
        </div>
        <div class="row">
            <div class="col-sm-3  text-right"><span class="formLabel200">Effective Date*</span></div>
            <div class="col-sm-9" style="text-align: left;">
                <asp:TextBox ID="txtEffectiveDate" aria-label="Effective Date" runat="server" CssClass="formField" />
                <ajax:CalendarExtender ID="calEffectiveDate" TargetControlID="txtEffectiveDate" runat="server" />
                <asp:RequiredFieldValidator ID="rfvEffectiveDate" runat="server" SetFocusOnError="true" ValidationGroup="valInsurance" Text="*"
                    ControlToValidate="txtEffectiveDate" ErrorMessage="*Enter Effective Date" Display="Dynamic" />
                <asp:CompareValidator ID="dateValidator" runat="server" ValidationGroup="valInsurance"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEffectiveDate"
                    ErrorMessage="Select a valid Effective Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> 
                </asp:CompareValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3  text-right"><span class="formLabel200">Original Effective Date*</span></div>
            <div class="col-sm-9" style="text-align: left;">
                <asp:TextBox ID="txtOrginaEffDate" aria-label="Original Effective Date"  runat="server" CssClass="formField" />
                <ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="txtOrginaEffDate" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" SetFocusOnError="true" ValidationGroup="valInsurance" Text="*"
                    ControlToValidate="txtOrginaEffDate" ErrorMessage="*Enter Orginal Effective Date" Display="Dynamic" />
                <asp:CompareValidator ID="CompareValidator2" runat="server" ValidationGroup="valInsurance"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtOrginaEffDate"
                    ErrorMessage="Select a valid Orginal Effective Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> 
                </asp:CompareValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3  text-right"><span class="formLabel200">Expiration Date*</span></div>
            <div class="col-sm-9" style="text-align: left;">
                <asp:TextBox ID="txtExpirationDate" aria-label="Expiration Date" runat="server" CssClass="formField" />
                <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtExpirationDate" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" SetFocusOnError="true" ValidationGroup="valInsurance" Text="*"
                    ControlToValidate="txtExpirationDate" ErrorMessage="*Enter Expiration Date" Display="Dynamic" />
                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="valInsurance"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtExpirationDate"
                    ErrorMessage="Select a valid Expiration Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> 
                </asp:CompareValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3 text-right">
                <span class="formLabel wd200">Type of Coverage*</span>
            </div>
            <div class="col-sm-9">
                <asp:DropDownList ID="ddlTypeofCoverage" aria-label="Type of Coverage" runat="server" CssClass="formField" />
                <asp:RequiredFieldValidator ID="rfvTypeofCoverage" runat="server" SetFocusOnError="true" ValidationGroup="valInsurance" Text="*"
                    ControlToValidate="ddlTypeofCoverage" ErrorMessage="*Type of coverage is required." Display="Dynamic" Enabled="true" InitialValue="" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3 text-center">
                <span class="formLabel wd200">&nbsp;&nbsp;Do you have unlimited coverage?*</span>
            </div>
            <div class="col-sm-9">
                <asp:DropDownList ID="ddlIsUnlimitedCoverage" aria-label="Do you have unlimited coverage" runat="server" CssClass="formField">
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvIsUnlimitedCoverage" runat="server" SetFocusOnError="true" ValidationGroup="valInsurance" Text="*"
                    ControlToValidate="ddlIsUnlimitedCoverage" ErrorMessage="*Unlimited coverage is required." Display="Dynamic" Enabled="true" InitialValue="" />
            </div>
        </div>
          <div class="row">
            <div class="col-sm-3 text-right">
                <span class="formLabel wd200">Policy includes tail coverage*</span>
            </div>
            <div class="col-sm-9">
                <asp:DropDownList ID="ddlIsPolicyTailCoverageInclude" aria-label="Policy includes tail coverage"  runat="server" CssClass="formField">
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" SetFocusOnError="true" ValidationGroup="valInsurance" Text="*"
                    ControlToValidate="ddlIsPolicyTailCoverageInclude" ErrorMessage="*Policy includes tail coverage is required." Display="Dynamic" Enabled="true" InitialValue="" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3  text-right"><span class="formLabel200">Carrier or Self-Insured Name*</span></div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtCarrierName" runat="server" aria-label="Carrier or Self-Insured Name" CssClass="formField" MaxLength="80" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1"
                    ControlToValidate="txtCarrierName" ErrorMessage="*Enter Carrier or Self-Insured Name" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valInsurance" />
            </div>
        </div>
            <div class="row">
            <div class="col-sm-3  text-right"><span class="formLabel200"></span></div>
            <div class="col-sm-9">
            <asp:CheckBox  runat="server" class="pg-hint4" ID="chkFTCA" Text="Check here if insurance is through Federal Tort Claims Act (FTCA) "  /> 
            
            </div>
        </div>
    </div>
    <uc:Address ID="ucAddress" runat="server" ValidationGroup="valInsurance" OnStateChangedEvent="ucAddress_StateChangedEvent"></uc:Address>
  <%--  <div id="Table1" runat="server">
        <div class="row">
            <div class="col-sm-3  text-right"><span class="formLabel200">Agent Name*</span></div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtAgentName" runat="server" CssClass="formField" MaxLength="80" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4"
                    ControlToValidate="txtAgentName" ErrorMessage="*Enter Agent Name" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valInsurance" />
            </div>
        </div>--%>
        <div class="row">
            <div class="col-sm-3  text-right">
                <asp:Label ID="lblPhone" runat="server" CssClass="formLabel200">Agent Telephone Number*</asp:Label>
            </div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="formField" MaxLength="80" />
                <ajax:MaskedEditExtender runat="server" ID="meePhoneNumber" AutoComplete="False"
                    ClearMaskOnLostFocus="False" TargetControlID="txtPhoneNumber"
                    MaskType="Number" Mask="(999) 999-9999" CultureAMPMPlaceholder=""
                    CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                    CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="valInsurance"
                    ControlToValidate="txtPhoneNumber" ErrorMessage="*Enter Phone Number" Text="*" Display="Dynamic"
                    SetFocusOnError="true" InitialValue="(___) ___-____" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3  text-right">
                <asp:Label ID="lblFax" runat="server" class="formLabel200">Agent Fax Number*</asp:Label>
            </div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtFaxNumber" runat="server" CssClass="formField" MaxLength="80" />
                <ajax:MaskedEditExtender runat="server" ID="MaskedEditExtender1" AutoComplete="False"
                    ClearMaskOnLostFocus="False" TargetControlID="txtFaxNumber"
                    MaskType="Number" Mask="(999) 999-9999" CultureAMPMPlaceholder=""
                    CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                    CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                    CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ValidationGroup="valInsurance"
                    ControlToValidate="txtFaxNumber" ErrorMessage="*Enter Fax Number" Text="*" Display="Dynamic"
                    SetFocusOnError="true" InitialValue="(___) ___-____" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3  text-right">
                <asp:Label ID="lblemail" runat="server" class="formLabel200" Visible="false">Agent Email Address*</asp:Label>
            </div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtemail" runat="server" CssClass="formField" MaxLength="80" Visible="false" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7"
                    ControlToValidate="txtemail" ErrorMessage="*Enter valid Email" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valInsurance" Enabled="False" />
                <asp:RegularExpressionValidator ID="valEmailFormat" runat="server" ControlToValidate="txtemail" Display="Dynamic" Text="*"
                    ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                    ErrorMessage="* Invalid email format." ValidationGroup="valInsurance"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3  text-right">
                <asp:Label ID="lblpolicy" runat="server" class="formLabel200" Visible="true">Policy Holder*</asp:Label>
            </div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtPolicyHolder" runat="server" aria-label="Policy Holder" CssClass="formField" MaxLength="80" Visible="true" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8"
                    ControlToValidate="txtPolicyHolder" ErrorMessage="*Enter Policy Holder" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valInsurance" Enabled="False" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3 text-center"><span class="formLabel200">&nbsp;&nbsp;Coverage Amount Per Occurrence*</span></div>
            <div class="col-sm-9">
                <%--<telerik:RadMaskedTextBox  ID="txtAmtPerOccurrence" runat="server" Mask="$#,###,###,###,###" ></telerik:RadMaskedTextBox>--%>
                <asp:TextBox ID="txtAmtPerOccurrence" aria-label="Coverage Amount Per Occurrence"  runat="server" CssClass="formField" MaxLength="15" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9"
                    ControlToValidate="txtAmtPerOccurrence" ErrorMessage="*Enter Coverage Amount Per Occurrence" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valInsurance" />
                <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtAmtPerOccurrence" 
                ValidationExpression="^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$" ErrorMessage="*Enter Coverage Amount Per Occurrence" Text="*" Display="Dynamic" 
                ValidationGroup="valInsurance" />--%>
                <%--        <ajax:MaskedEditExtender runat="server" ID="meeAdditionalAmount" AutoComplete="False" ClearMaskOnLostFocus="false" 
            TargetControlID="txtAmtPerOccurrence" MaskType="Number" Mask="999\,999\,999" CultureAMPMPlaceholder="" 
            DisplayMoney="Left" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" 
            CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" InputDirection="RightToLeft"/>               --%>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-3 text-center"><span class="formLabel200">&nbsp;&nbsp;Coverage Amount Per Aggregate*</span></div>
            <div class="col-sm-9">
                <asp:TextBox ID="txtAmtPerAggregate" aria-label="Coverage Amount Per Aggregate" runat="server" CssClass="formField" MaxLength="15" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator10"
                    ControlToValidate="txtAmtPerAggregate" ErrorMessage="*Enter Coverage Amount Per Aggregate" Text="*" Display="Dynamic"
                    SetFocusOnError="true" ValidationGroup="valInsurance" />
                <%--      <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAmtPerAggregate" 
                ValidationExpression="^[+-]?[0-9]{1,3}(?:[0-9]*(?:[.,][0-9]{2})?|(?:,[0-9]{3})*(?:\.[0-9]{2})?|(?:\.[0-9]{3})*(?:,[0-9]{2})?)$" ErrorMessage="*Enter Coverage Amount Per Aggregate" Text="*" Display="Dynamic" 
                ValidationGroup="valInsurance" />--%>
                <%--<ajax:MaskedEditExtender runat="server" ID="MaskedEditExtender2" AutoComplete="False" ClearMaskOnLostFocus="false" TargetControlID="txtAmtPerAggregate" MaskType="Number" Mask="999\,999\,999" CultureAMPMPlaceholder="" 
                            DisplayMoney="Left"                CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" InputDirection="RightToLeft"/>               --%>
            </div>
        </div>
    </div>
</div>
<asp:panel id="upInsuranceHistory" runat="server">
    <ajax:modalpopupextender id="mpe" runat="server" popupcontrolid="pnlModal" targetcontrolid="ButtonDummy2"
        backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnlModal" CancelControlID="btnModalOk">
    </ajax:modalpopupextender>
    <asp:panel id="pnlModal" runat="server" cssclass="modalPopup" style="display: none; padding: 20px; width: auto; left: 122px !important; top: 13px !important">
        <asp:panel id="pnlHeader" cssclass="pnlHeader" runat="server" horizontalalign="Left">
            <div align="left">
                &nbsp;&nbsp;
                <asp:label id="lblTitle" cssclass="bodyTextBold" runat="server" text="Title" forecolor="White" />
            </div>
        </asp:panel>
        <asp:panel id="pnlMain1" runat="server" style="padding: 10px; margin-left: 10px;">
            <div>
                <uc:insurancehistory id="ucInsuranceHistory" runat="server" />
                
            </div>
        </asp:panel>
        <div class="btnBox" style="padding: 10px">
            <asp:button runat="server" id="btnModalOk" text="OK" cssclass="buttonBox" causesvalidation="false" />
            <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkExcel_Click" />
        </div>
    </asp:panel>
    <asp:button runat="server" id="ButtonDummy2" style="display: none" text="”ButtonDummy2”" />
</asp:panel>
<div style="width: 100%; text-align: right; display: none;">
    <asp:HiddenField ID="hdnRowCount" runat="server" />
    <asp:Button ID="lnkExcel" CommandName="Export" OnClick="lnkExcel_Click" runat="server" ToolTip="Excel" Visible="true" Text="Export"/>
    <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
    </div>
    <telerik:RadGrid ID="grd" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
        AutoGenerateColumns="false" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
            <Excel Format="Biff" />
        </ExportSettings>
        <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
            DataKeyNames="INDEX, DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
            <Columns>
                <telerik:GridBoundColumn DataField="Operation"               HeaderText="Operation"              SortExpression="Operation" />
                <telerik:GridBoundColumn DataField="isMalpracticeClaimed"           HeaderText="Carrying malpractice insurance?"                    SortExpression="isMalpracticeClaimed" />
                <telerik:GridBoundColumn DataField="IS_SELF_INSURED"                    HeaderText="Self Insured?"                                      SortExpression="IS_SELF_INSURED" />
                <telerik:GridBoundColumn DataField="POLICY_NUMBER"                   HeaderText="Policy Number"                                      SortExpression="POLICY_NUMBER" />
                <telerik:GridBoundColumn DataField="EFFECTIVE_DATE"                  HeaderText="Eff Date" DataFormatString="{0:MM/dd/yyyy}"         SortExpression="EFFECTIVE_DATE" />
                <telerik:GridBoundColumn DataField="ORIGINAL_EFFECTIVE_DATE"                   HeaderText="Orig Eff Date" DataFormatString="{0:MM/dd/yyyy}"    SortExpression="ORIGINAL_EFFECTIVE_DATE"  />
                <telerik:GridBoundColumn DataField="EXPIRATION_DATE"                 HeaderText="Expiration Date" DataFormatString="{0:MM/dd/yyyy}"  SortExpression="EXPIRATION_DATE" />
                <telerik:GridBoundColumn DataField="TYPE_OF_COVERAGE_NAME"                   HeaderText="Type of Coverage"                                   SortExpression="TYPE_OF_COVERAGE_NAME" />
                <telerik:GridBoundColumn DataField="IS_UNLIMITED_COVERAGE"           HeaderText="Do you have unlimited coverage?"                    SortExpression="IS_UNLIMITED_COVERAGE" />
                <telerik:GridBoundColumn DataField="TAIL_NOSE_COVERAGE"                    HeaderText="Policy includes tail coverage?"                     SortExpression="TAIL_NOSE_COVERAGE" />
                <telerik:GridBoundColumn DataField="CarrierName"                    HeaderText="Carrier or Self-Insured Name"                       SortExpression="CarrierName" />
                <telerik:GridBoundColumn DataField="CarrierAddress"                 HeaderText="Carrier Address"                                    SortExpression="CarrierAddress" />
                <telerik:GridBoundColumn DataField="PolicyHolder"                    HeaderText="Policy Holder"                                      SortExpression="PolicyHolder" />
                <telerik:GridBoundColumn DataField="CoverageAmountPerOccurance"      HeaderText="Coverage Account Per Occurence"                     SortExpression="CoverageAmountPerOccurance" />
                <telerik:GridBoundColumn DataField="CoverageAmountPerAggregate"      HeaderText="Coverage Account Per Aggregate"                     SortExpression="CoverageAmountPerAggregate" />
                <telerik:GridBoundColumn DataField="MalpracticeInsuranceReason"      HeaderText="Explanation regarding malpractice insurance"        SortExpression="MalpracticeInsuranceReason" />
                <telerik:GridBoundColumn DataField="UserName"                        HeaderText="User Name"                                          SortExpression="UserName" />
                <telerik:GridBoundColumn DataField="DateOfAction"                    HeaderText="Update Date"                                        SortExpression="DateOfAction"   DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False"  />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>

<br />
<br />
</div>

<%--<uc:UploadControl ID="insUploadControl" runat="server" IsRequired="true" UploadValidationError="Please upload proof of insurance" UploadDocumentMessage="Upload the proof of insurance details here" />--%>
<asp:HiddenField ID="hidID" runat="server" Visible="false" />
<asp:HiddenField ID="hdnRegInsuranceID" runat="server" />
<asp:HiddenField ID="hdnPolicyUploaded" runat="server" />
<asp:PlaceHolder runat="server" ID="PlaceholderUploadInsurance"></asp:PlaceHolder>
<script>
    $(function () {
        $('#<%= txtAmtPerOccurrence.ClientID %>').maskMoney({ precision: '0' });
        $('#<%= txtAmtPerAggregate.ClientID %>').maskMoney({ precision: '0' });
    })
</script>
