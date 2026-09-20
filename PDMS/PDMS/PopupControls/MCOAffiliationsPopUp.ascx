<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_MCOAffiliationsPopUp" Codebehind="MCOAffiliationsPopUp.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<style>
    .wd120Custom  {
            padding-right: 300px !important;
    }
</style>

<div id="AddressTable">
    <asp:UpdatePanel ID="upRegAffil" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" >
                <div class="popTitle">
                    <asp:Label ID="lbl_title"  runat="server" Text="MCP Affliation Details"  />
                </div> 
            </asp:Panel> 
            <asp:Panel ID="pnlPage" runat="server" DefaultButton="btnSave" Style="padding: 10px;">
                <div style="width: 900px; overflow: scroll; height: 750px;">
                    <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
                    <asp:ValidationSummary ID="MCOAffiliations" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="MCOAffiliations" />
                    <asp:ValidationSummary ID="ConfirmAffiliations" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="ConfirmAffiliations" />
                    <table>
                        <tr>
                            <td class="formLabel wd120Custom">Plan Name</td>
                            <td class="wd225 alignLeft">
                                <asp:TextBox ID="txtPlanName" runat="server"  CssClass="formField "  ReadOnly="true" />
                            </td>

                        </tr>

                        <tr>
                            <td class="checkclass formLabel wd120Custom">Start Date*</td>
                            <td class="wd225 alignLeft">
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="formField " ReadOnly="true" /><ajax:CalendarExtender ID="calStart" TargetControlID="txtStartDate" runat="server" />

                            </td>

                        </tr>
                        <tr>
                            <td id="tdEndDateLbl" runat="server" class="formLabel wd120Custom">End Date</td>
                            <td id="tdEndDateFld" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="formField " ReadOnly="true" /><ajax:CalendarExtender ID="calEnd" TargetControlID="txtEndDate" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td id="td3" runat="server" class="formLabel wd120Custom">Provider Type</td>
                            <td id="td4" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtProviderType" runat="server" CssClass="formField " ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td id="td5" runat="server" class="formLabel wd120Custom">MCPN Plan Specialities</td>
                            <td id="td6" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtSpecialities" runat="server" CssClass="formField " ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td id="td7" runat="server" class="formLabel wd120Custom">MITS Specialities</td>
                            <td id="td8" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtMitsSpecialiaties" runat="server" CssClass="formField " ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td id="td9" runat="server" class="formLabel wd120Custom">Group Affiliation</td>
                            <td id="td10" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtgoupAffliation" runat="server" CssClass="formField " ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td id="td11" runat="server" class="formLabel wd120Custom">Tracking Number</td>
                            <td id="td12" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtTracingNumber" runat="server" CssClass="formField " ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td id="td13" runat="server" class="formLabel wd120Custom">Program Code</td>
                            <td id="td14" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtProgramCode" runat="server" CssClass="formField " ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td id="td15" runat="server" class="formLabel wd120Custom">PCP Indicator</td>
                            <td id="td16" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtPCPIndicator" runat="server" CssClass="formField " ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td id="td17" runat="server" class="formLabel wd120Custom">Panel Capacity</td>
                            <td id="td18" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtPanelCapacity" runat="server" CssClass="formField " ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td id="td19" runat="server" class="formLabel wd120Custom">Existing Patients Only</td>
                            <td id="td20" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtExtPatientsOnly" runat="server" CssClass="formField " ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td id="td21" runat="server" class="formLabel wd120Custom">Gender Accepted</td>
                            <td id="td22" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtGenederAccepted" runat="server" CssClass="formField " ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td id="td23" runat="server" class="formLabel wd120Custom">Age Limit Low</td>
                            <td id="td24" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtAgeLimitLow" runat="server" CssClass="formField " ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td id="td25" runat="server" class="formLabel wd120Custom">Age Limit High</td>
                            <td id="td26" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtAgeLimitHigh" runat="server" CssClass="formField " ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td id="td27" runat="server" class="formLabel wd120Custom">Accept Newborns</td>
                            <td id="td28" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtNewborns" runat="server" CssClass="formField " ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td id="td29" runat="server" class="formLabel wd120Custom">Accept Family Members</td>
                            <td id="td30" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtFamMembers" runat="server" CssClass="formField " />
                            </td>
                        </tr>
                        <tr>
                            <td id="td31" runat="server" class="formLabel wd120Custom">Accept Pregnant Woman</td>
                            <td id="td32" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtPregWoman" runat="server" CssClass="formField " />
                            </td>
                        </tr>
                        <tr>
                            <td id="td33" runat="server" class="formLabel wd120Custom">Languages Spoken</td>
                            <td id="td34" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtLanSpoken" runat="server" CssClass="formField " />
                            </td>
                        </tr>
                        <tr>
                            <td id="td35" runat="server" class="formLabel wd120Custom">TPA Name</td>
                            <td id="td36" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtTPA" runat="server" CssClass="formField " />
                            </td>
                        </tr>
                        <tr>
                            <td id="td37" runat="server" class="formLabel wd120Custom">Plan Comments</td>
                            <td id="td38" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtComments" runat="server" CssClass="formField " />
                            </td>
                        </tr>

                        <%-- <tr id="trEndDateHint" runat="server">
                            <td colspan="4" class="pg-hint3"><b>Only enter the End Date</b> when the individual provider has left your group; <b>otherwise, leave blank.</b></td>
                        </tr>--%>
                    </table>
                    <table class="wdAuto" style="display: none;">
                        <tr>

                            <td id="td1" runat="server" class="formLabel wd120Custom">Medicaid ID</td>
                            <td id="td2" runat="server" class="wd225 alignLeft">
                                <asp:TextBox ID="txtMedicaidID" runat="server" CssClass="formField  wd120" MaxLength="9" />

                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtMedicaidID" ValidationExpression="^([0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$"
                                    ErrorMessage="* Enter a 9 digit Medicaid ID." Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="MCOAffiliations" Display="Dynamic" />

                            </td>
                        </tr>
                    </table>
                    

                    <asp:UpdateProgress runat="server" ID="upSaveProgress" DisplayAfter="0" AssociatedUpdatePanelID="upRegAffil">
                        <ProgressTemplate>
                            <div class="loading">
                                <asp:Image ID="imgSaving" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>

                    <div id="divAffiliationSaveBox" class="btnBox" runat="server">
                        <asp:Button ID="btnSave" runat="server" Text="Save" Visible="false" CssClass="buttonBox" CausesValidation="true" ValidationGroup="MCOAffiliations"/>
                        <asp:Button ID="Button1" runat="server" Text="Cancel" Visible="false" CssClass="buttonBox" CausesValidation="false" />
                        <asp:Button ID="btnSaveConfirm" runat="server" Visible="false" Text="Confirm Association" CssClass="buttonBox"  ValidationGroup="ConfirmAffiliations" CausesValidation="true" />
                        <asp:Button ID="btnSaveEndDate" runat="server" Visible="false" Text="End Association" CssClass="buttonBox" ValidationGroup="MCOAffiliations" CausesValidation="true" />
                        <asp:Button ID="btnCancel" runat="server" Text="Close" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />

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
                    ValidationGroup="MCOAffiliations"
                    runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</div>
