<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_GroupAffiliationsCtrl" Codebehind="GroupAffiliationsCtrl.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>


<div id="AddressTable">
    <asp:UpdatePanel ID="upRegAffil" runat="server" UpdateMode="Conditional">

        <ContentTemplate>

            <asp:Panel ID="pnlPage" runat="server" DefaultButton="btnSave" Style="padding: 10px;">
                <div>
                    <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
                    <asp:ValidationSummary ID="GroupAffiliations" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="GroupAffiliations" />
                    <asp:ValidationSummary ID="ConfirmAffiliations" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="ConfirmAffiliations" />
                    <div class="wdAuto">
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel wd120">First Name*</span></div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox ID="txtFirstName" runat="server" MaxLength="35" CssClass="formField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valFNReqd" runat="server" ControlToValidate="txtFirstName" Enabled="true" SetFocusOnError="true"
                                    Display="Dynamic" Text="*" ValidationGroup="GroupAffiliations" ErrorMessage="* First Name is required."></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel wd120">Last Name*</span></div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox ID="txtLastName" runat="server" MaxLength="35" CssClass="formField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valLNReqd" runat="server" ControlToValidate="txtLastName" Enabled="true" SetFocusOnError="true"
                                    Display="Dynamic" Text="*" ValidationGroup="GroupAffiliations" ErrorMessage="* Last Name is required."></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel wd120">NPI*</span></div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox ID="txtNPI" runat="server" MaxLength="10" CssClass="formField" />
                                <asp:RequiredFieldValidator ID="rfvNPI" runat="server" ControlToValidate="txtNPI" Text="*"
                                    Enabled="true" Display="Dynamic" ValidationGroup="GroupAffiliations" ErrorMessage="* A valid NPI is required."></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="valNPIFormat" runat="server" ControlToValidate="txtNPI" ValidationExpression="^([1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$"
                                    ErrorMessage="* Enter a 10 digit NPI that does not begin with 0." Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="GroupAffiliations" Display="Dynamic" />
                             
                            </div>
                        </div>
                        <asp:Panel ID="pnlRenderingLocations" runat="server">
                            <div class="row" id="divRenderingLocations" runat="server">
                                <div class="col-sm-3 text-right"><span class="formLabel wd120">Rendering Location*</span></div>
                                <div class="col-sm-9 text-left">
                                    <asp:DropDownList ID="ddlRenderingLocations" runat="server" CssClass="formField"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rgvRenderingLocations" runat="server" ControlToValidate="ddlRenderingLocations" Enabled="true" SetFocusOnError="true"
                                        Display="Dynamic" Text="*" ValidationGroup="GroupAffiliations" ErrorMessage="* Rendering Locations."></asp:RequiredFieldValidator>
                                    <br />
                                     <asp:CheckBox ID="chkDirectoryOptOut" runat="server" Text="Click here to NOT include this provider in directory for this location." /> 
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="checkclass formLabel wd120">Start Date*</span></div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="calStart" TargetControlID="txtStartDate" runat="server" />
                                <asp:RequiredFieldValidator runat="server" ID="valStartReqd" ControlToValidate="txtStartDate" ErrorMessage="* Start Date is required." Text="*" Display="Dynamic"
                                    SetFocusOnError="true" ValidationGroup="GroupAffiliations" /><div id="divWhatIsReqEffectiveDate1" class="bodyTextSmall what-is-this-link1" style="color: blue; cursor: pointer; display: inline-block; text-decoration: underline" runat="server">What is this?</div>
                                <div id="divRequestedEffectiveDateInfo1" class="infoBox" style="left: 120px; top: 100px;">
                                    <div class="infoTitle">Start Date</div>
                                    <div class="infoContent">
                                        <asp:Literal ID="ltlRequestedEffectiveDateInfo1" runat="server" Text="<%$ Resources:BrandingResource , REQUESTED_EFFECTIVE_DATE_HELPTEXT %>"></asp:Literal>
                                    </div>
                                </div>
                                <asp:CompareValidator ID="cvEndDate" runat="server" ValidationGroup="GroupAffiliations"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtStartDate" Enabled="true"
                                    ErrorMessage="* A valid Start Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true" />

                                <asp:CustomValidator ID="valStartRetroCheck" runat="server" OnServerValidate="Validate_RetroStartDate" ControlToValidate="txtStartDate" Display="Static" ValidationGroup="GroupAffiliations"
                                    ErrorMessage="* Start date for the Group Member cannot be prior to your Group effective date" Text="*" />
                            </div>
                        </div>
                        <asp:Panel ID="pnlEndDate" runat="server" Enabled="false">
                            <div class="row" id="divEndDate" runat="server">
                                <div class="col-sm-3 text-right"><span class="checkclass formLabel wd120">End Date</span></div>
                                <div class="col-sm-9 text-left">
                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField" />
                                    <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtEndDate" runat="server" />
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="GroupAffiliations"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate" Enabled="true"
                                        ErrorMessage="* A valid End Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="true" />
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span id="td1" runat="server" class="formLabel wd120">Medicaid ID</span></div>
                            <div id="td2" runat="server" class="col-sm-9 text-left">
                                <asp:TextBox ID="txtMedicaidID" runat="server" CssClass="formField" MaxLength="9" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMedicaidID" Text="*"
                                    Enabled="false" Display="Dynamic" ValidationGroup="ConfirmAffiliations" ErrorMessage="* Medicaid ID is required."></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator1" ValidationGroup="GroupAffiliations" runat="server"
                                    ErrorMessage="* The Medicaid ID does not match the NPI on file." ControlToValidate="txtMedicaidID" OnServerValidate="Validate_MedicaidID"
                                    Text="*" />
                            </div>
                        </div>
                    </div>
                    <div>
                        <div class="row" id="trStatus" runat="server">
                            <div class="col-sm-3 text-right"><span class="formLabel wd120">Affiliation Status</span></div>
                            <div class="col-sm-9 text-left">
                                <asp:Label ID="lblAffiliationStatus" runat="server" CssClass="formFieldDisplayAuto" />
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

                    <div id="divAffiliationSaveBox" class="btnBox text-center" runat="server">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="GroupAffiliations" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
                        <asp:Button ID="btnSaveConfirm" runat="server" Text="Confirm Association" CssClass="buttonBox" OnClick="btnSaveConfirm_Click" ValidationGroup="ConfirmAffiliations" CausesValidation="true" />
                    </div>
                </div>
                <br />

                <div runat="server" id="divConfirmGroupAffiliation" style="text-align: center">
                    <p style="color: red">
                        <asp:Label runat="server" ID="lblmessage" Text=""></asp:Label>
                    </p>
                </div>
                <asp:HiddenField ID="hdnGroupAffiliationConfirm" runat="server" Value="0" />
                <asp:CustomValidator ID="cvGroupAffiliation"
                    ControlToValidate=""
                    OnServerValidate="cvGroupAffiliation_ServerValidate"
                    Display="None"
                    ErrorMessage=""
                    ValidationGroup="GroupAffiliations"
                    runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</div>
