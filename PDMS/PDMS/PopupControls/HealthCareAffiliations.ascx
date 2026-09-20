<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_HealthCareAffiliations" Codebehind="HealthCareAffiliations.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:UpdatePanel ID="upRegAffil" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:Panel ID="pnlPage" runat="server" DefaultButton="btnSave" Style="padding: 10px;">
            <div style="width: 750px;">
                <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
                <asp:ValidationSummary ID="HealthCareAffiliations" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="HealthCareAffiliations" />
                <asp:ValidationSummary ID="ConfirmAffiliations" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="ConfirmAffiliations" />
                <div class="wdAuto">
                    <div class="row">
                        <div class="col-sm-7">
                            <asp:Label ID="Label1" runat="server" Text="Do you practice exclusively within the Inpatient Setting?*" CssClass="formLabel" />
                        </div>
                        <div class="col-sm-4">
                            <asp:RadioButtonList ID="rblInpatientSetting" aria-label="Do you practice exclusively within the Inpatient Setting" runat="server" RepeatDirection="Horizontal" CssClass="QstRadioList">
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                                <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div id="divWhatIsHTInpatientSetting" aria-label="Inpatient Setting" tabindex="0" class="col-sm-1 bodyTextSmall what-is-this-link-InpatientSetting" style="color: blue; cursor: pointer; display: inline-block; text-decoration: underline" runat="server">
                            <asp:Image ID="Image3" AlternateText="Inpatient Setting"  runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" />
                        </div>
                        <div id="divHTInpatientSetting" class="infoBox" style="left: 120px; top: 100px;">
                            <div class="infoTitle">Inpatient Setting</div>
                            <div class="infoContent">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:BrandingResource , KEY_FIELD_HOSPITAL_AFFILIATION_INPATIENTSETTING_HELPTEXT %>"></asp:Literal>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-5">
                            <asp:Label ID="Label3" runat="server" Text="Do you have hospital privileges?*" CssClass="formLabel" />
                        </div>
                        <div class="col-sm-7">
                            <asp:RadioButtonList ID="rblHospitalPrivileges" aria-label="Do you have hospital privileges" runat="server" RepeatDirection="Horizontal" CssClass="QstRadioList" OnSelectedIndexChanged="rblHospitalPrivileges_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="True" Selected="True">Yes</asp:ListItem>
                                <asp:ListItem Value="False">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right"><span class="formLabel">If 'No', please specify</span></div>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtHospitalPrivilegesReason" runat="server" Rows="2" CssClass="formField"  aria-label="If 'No', please specify" TextMode="MultiLine" MaxLength="150" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvrfvHospitalPrivileges" ControlToValidate="txtHospitalPrivilegesReason" 
                                ErrorMessage="* what kind of admitting arrangements comment required." Text="*" Display="Dynamic" 
                                SetFocusOnError="true" ValidationGroup="HealthCareAffiliations" Enabled="false" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right">
                            <asp:Label ID="lblIsPrimaryFacility" runat="server" Text="This is my Primary Facility" class="formLabel" />
                        </div>
                        <div class="col-sm-7">
                            <asp:CheckBox ID="chkIsPrimaryFacility" runat="server" />
                        </div>
                        <div id="divWhatIsReqPrimaryFacility" aria-label="Primary Facility" tabindex="0" class="col-sm-1 bodyTextSmall what-is-this-link-primaryFacility" style="color: blue; cursor: pointer; display: inline-block; text-decoration: underline" runat="server">
                            <asp:Image ID="imgHelpTaxID" AlternateText="Primary Facility" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" />
                        </div>
                        <div id="divRequestedPrimaryFacility" class="infoBox" style="left: 120px; top: 100px;">
                            <div class="infoTitle">Primary Facility</div>
                            <div class="infoContent">
                                <asp:Literal ID="ltlRequestedEffectiveDateInfo1" runat="server" Text="<%$ Resources:BrandingResource , KEY_FIELD_HOSPITAL_AFFILIATION_PRIMARY_FACILITY_HELPTEXT %>"></asp:Literal>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right"><span class="formLabel">Ohio Medicaid ID*</span></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtMedicaidIDSearch" aria-label="Ohio Medicaid ID" runat="server" MaxLength="7" CssClass="formField" OnTextChanged="txtMedicaidIDSearch_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valMedReq" runat="server" ControlToValidate="txtMedicaidIDSearch" Enabled="true" SetFocusOnError="true"
                                Display="Dynamic" Text="*" ValidationGroup="HealthCareAffiliations" ErrorMessage="* Please enter the Medicaid ID for the Facility"></asp:RequiredFieldValidator>
                            <asp:CustomValidator runat="server" Display="Dynamic" ID="customValidator1" ControlToValidate="txtMedicaidIDSearch" ForeColor="Red"
                                ErrorMessage="You must enter the Medicaid ID of a hospital."></asp:CustomValidator>
                        </div>
                        <div id="divWhatIsReqMedicaidID" aria-label="Search Facility using Medicaid ID" tabindex="0" class="col-sm-1 bodyTextSmall what-is-this-link-MedicaidID" style="color: blue; cursor: pointer; display: inline-block; text-decoration: underline" runat="server">
                            <asp:Image ID="Image2" AlternateText="Search Facility using Medicaid"  runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" />
                        </div>
                        <div id="divRequestedMedicaidID" class="infoBox" style="left: 120px; top: 100px;">
                            <div class="infoTitle">Search Facility using Medicaid ID</div>
                            <div class="infoContent">
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:BrandingResource , KEY_FIELD_HOSPITAL_AFFILIATION_MEDICAIDID_HELPTEXT %>"></asp:Literal>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right"><span class="formLabel">Facility Name*</span></div>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtFacilityName" aria-label="Facility Name"  runat="server" MaxLength="35" CssClass="formField" ReadOnly="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="valFNReqd" runat="server" ControlToValidate="txtFacilityName" Enabled="true" SetFocusOnError="true"
                                Display="Dynamic" Text="*" ValidationGroup="HealthCareAffiliations" ErrorMessage="* Facility Name is required."></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right"><span class="formLabel">Staff Category* </span></div>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlStaffCategoryID" runat="server" CssClass="formDropDown" aria-label="select Staff Category">
                                <asp:ListItem Text="" Value="" Selected="True" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="ddlAffiliationPrivilegesStatusRequired" ControlToValidate="ddlStaffCategoryID"
                                Display="Dynamic" SetFocusOnError="true" Text="*" InitialValue=""
                                ErrorMessage="* Staff Category is required." ValidationGroup="HealthCareAffiliations" />
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right"><span class="formLabel">Status of Privileges* </span></div>
                        <div class="col-sm-8">
                            <asp:DropDownList ID="ddlAffiliationPrivilegesStatus" runat="server" CssClass="formDropDown" aria-label="select Status of Privileges">
                                <asp:ListItem Text="" Value="" Selected="True" />
                              
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlAffiliationPrivilegesStatus"
                                Display="Dynamic" SetFocusOnError="true" Text="*" InitialValue=""
                                ErrorMessage="* Status of Privileges is required." ValidationGroup="HealthCareAffiliations" />
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right"><span class="formLabel">Start Date*</span></div>
                        <div class="col-sm-7">
                            <asp:TextBox ID="txtStart_Date" runat="server" aria-label="Start Date" CssClass="formField" />
                            <ajax:CalendarExtender ID="calStart" TargetControlID="txtStart_Date" runat="server" />
                            <asp:RequiredFieldValidator runat="server" ID="reqEffective" ControlToValidate="txtStart_Date" 
                                ErrorMessage="* Start Date is required." Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="HealthCareAffiliations" />
                            <asp:CompareValidator id="cvStartDate" runat="server" ValidationGroup="HealthCareAffiliations"   
                                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtStart_Date"  
                                ErrorMessage="Select a valid Start Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                SetFocusOnError="true" /> 
                        </div>
                        <div id="divWhatIsReqEffectivestartDate" aria-label="Start Date" tabindex="0" class="col-sm-1 bodyTextSmall what-is-this-link-startDate" style="color: blue; cursor: pointer; display: inline-block; text-decoration: underline" runat="server">
                            <asp:Image ID="Image1" AlternateText="Start Date"  runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" />
                        </div>
                        <div id="divRequestedEffectivestartDate" class="infoBox" style="left: 120px; top: 100px;">
                            <div class="infoTitle">Start Date</div>
                            <div class="infoContent">
                                <asp:Literal ID="ltrStartDateHA" runat="server" Text="<%$ Resources:BrandingResource , HOSPITAL_AFFILIATIONS_START_DATE_HT %>"></asp:Literal>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right"><span class="formLabel">End Date</span></div>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="formFieldReadOnly" ReadOnly="true" Enabled="false" Text="12/31/2299" /><ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtEndDate" runat="server" />
                        </div>

                    </div>
                    <div class="row">
                        <div class="col-sm-5">
                            <asp:Label ID="Label2" runat="server" Text="Any past or present restriction of privileges?*" CssClass="formLabel" />
                        </div>
                        <div class="col-sm-7">
                            <asp:RadioButtonList ID="rblRestrictedPrivileges" aria-label="Any past or present restriction of privileges" runat="server" RepeatDirection="Horizontal" CssClass="QstRadioList" OnSelectedIndexChanged="rblRestrictedPrivileges_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="True">Yes</asp:ListItem>
                                <asp:ListItem Value="False" Selected="True">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4 text-right"><span class="formLabel">If 'Yes', please specify</span></div>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtResponseComment" aria-label="If 'Yes', please specify" runat="server" Rows="2" CssClass="formField" TextMode="MultiLine" MaxLength="150" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvResponseComment" ControlToValidate="txtResponseComment" 
                                ErrorMessage="* Any past or present restrictions of privileges comment required." Text="*" Display="Dynamic" 
                                SetFocusOnError="true" ValidationGroup="HealthCareAffiliations" Enabled="false" />
                        </div>
                    </div>
                </div>
                <asp:UpdateProgress runat="server" ID="upSaveProgress" DisplayAfter="0" AssociatedUpdatePanelID="upRegAffil">
                    <ProgressTemplate>
                        <div class="loading">
                            <asp:Image ID="imgSaving" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="PendingGroupAffiliations" /></td>
                        <td>
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" /></td>
                    </tr>
                </table>
            </div>
            <br />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
