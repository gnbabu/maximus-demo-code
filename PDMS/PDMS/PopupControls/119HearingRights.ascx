<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_119HearingRights" Codebehind="119HearingRights.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uss" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<script type="text/javascript">

</script>
<div style="width: 750px;">
    <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
    <asp:ValidationSummary ID="vs119HearingRights" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="val119HearingRights" />
</div>
<br />
<asp:Panel ID="pnlProposedAdjuidicationOrder" runat="server">
    <span class="pageHeader">Proposed Adjudication Order</span>
    <br />
    <div class="row">
        <div class="col-sm-6 text-right">
            <asp:Label ID="lblDateOfProposedAdjudication" AssociatedControlID="txtDateOfProposedAdjudication" runat="server" CssClass="formLabel300">Date of Proposed Adjudication Order</asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtDateOfProposedAdjudication" runat="server" CssClass="formField" Enabled="true" ValidationGroup="val119HearingRights" CauseValidation="true" />
            <ajax:CalendarExtender ID="calDateOfProposedAdjudication" TargetControlID="txtDateOfProposedAdjudication" runat="server" />
            <asp:CompareValidator ID="cvDateOfProposedAdjudication" runat="server" ValidationGroup="val119HearingRights"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateOfProposedAdjudication" Enabled="true"
                ErrorMessage="Select a valid Date for Proposed Adjudication" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true" />
            <asp:CustomValidator ID="cusvDateOfProposedAdjudication" ValidateEmptyText="true" runat="server" ControlToValidate="txtDateOfProposedAdjudication"
                OnServerValidate="ValidatePAODate" Display="Dynamic" ValidationGroup="val119HearingRights"
                ErrorMessage="*Date of Adjudication should be between today's date and submission date." Text="*" />
        </div>
    </div>
    <br />
    <asp:PlaceHolder runat="server" ID="PlaceholderUploadProposedAdjudicationOrder" Visible ="true"></asp:PlaceHolder>
    <div id="divPAO">
        <div class="row">
            <div class="col-sm-6 text-right">
                <asp:Label ID="lblChkPAOReturnedUndelivered" AssociatedControlID="chkPAOReturnedUndelivered" runat="server" CssClass="formLabel300">PAO Returned Undeliverable / Unclaimed?</asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:CheckBox ID="chkPAOReturnedUndelivered" runat="server" Enabled="true" TextAlign="Left" Checked="false" Text="" CssClass="hearingCheckbox" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6 text-right">
                <asp:Label ID="lblPAODateReSent" AssociatedControlID="txtPAODateReturned" runat="server" CssClass="formLabel200">Date Returned</asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:TextBox ID="txtPAODateReturned" runat="server" CssClass="formField" Enabled="true" ValidationGroup="val119HearingRights" CauseValidation="true" />
                <ajax:CalendarExtender ID="calPAODateReturned" TargetControlID="txtPAODateReturned" runat="server" />
                <asp:CompareValidator ID="cvtxtPAODateReturned" runat="server" ValidationGroup="val119HearingRights"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtPAODateReturned" Enabled="true"
                    ErrorMessage="Select a valid Date for PAO Date Returned" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6 text-right">
                <asp:Label ID="lblDateReSent" AssociatedControlID="txtPAODateReSent" runat="server" CssClass="formLabel200">Date Re-Sent</asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:TextBox ID="txtPAODateReSent" runat="server" CssClass="formField" Enabled="true" ValidationGroup="val119HearingRights" CauseValidation="true" />
                <ajax:CalendarExtender ID="calPAODateReSent" TargetControlID="txtPAODateReSent" runat="server" />
                <asp:CompareValidator ID="cvtxtPAODateReSent" runat="server" ValidationGroup="val119HearingRights"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtPAODateReSent" Enabled="true"
                    ErrorMessage="Select a valid Date for PAO Date Re-Sent" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true" />
            </div>
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="pnlHearingStatus" runat="server">
    <span class="pageHeader">Hearing Status</span>
    <br />
    <div class="row">
        <div class="col-sm-6 text-right">
            <asp:Label ID="lblHearingStatus" AssociatedControlID="ddlHearingStatus" runat="server" CssClass="formLabel200">Hearing Status</asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:DropDownList ID="ddlHearingStatus" runat="server" AutoPostBack="true" AppendDataBoundItems="True" CssClass="formDropDown" OnInit="ddlHearingStatus_Init" OnSelectedIndexChanged="ddlHearingStatus_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
    </div>
    <asp:Panel ID="pnlHearingStatusHR" runat="server" Enabled="false" Visible="false">
        <div class="row">
            <div class="col-sm-6 text-right">
                <asp:Label ID="lblDateOfHearingRequest" AssociatedControlID="txtDateOfHearingRequest" runat="server" CssClass="formLabel200">Date of Hearing Request</asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:TextBox ID="txtDateOfHearingRequest" runat="server" CssClass="formField" Enabled="true" ValidationGroup="val119HearingRights" CauseValidation="true" />
                <ajax:CalendarExtender ID="calDateOfHearingRequest" TargetControlID="txtDateOfHearingRequest" runat="server" />
                <asp:CompareValidator ID="cvtxtDateOfHearingRequest" runat="server" ValidationGroup="val119HearingRights"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateOfHearingRequest" Enabled="true"
                    ErrorMessage="Select a valid Date for Hearing Request" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true" />
                <asp:CustomValidator ID="cusDateOfHearingRequest" ValidateEmptyText="true" runat="server" ControlToValidate="txtDateOfHearingRequest"
                    OnServerValidate="ValidateHearingRequestDate" Display="Dynamic" ValidationGroup="val119HearingRights"
                    ErrorMessage="*Date of Hearing Request must be after Denial/Termination Date." Text="*" />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlHearingStatusSR" runat="server" Enabled="false" Visible="false">
        <div class="row">
            <div class="col-sm-6 text-right">
                <asp:Label ID="lblSettlementDate" AssociatedControlID="txtSettlementDate" runat="server" CssClass="formLabel200">Settlement Date</asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:TextBox ID="txtSettlementDate" runat="server" CssClass="formField" Enabled="true" ValidationGroup="val119HearingRights" CauseValidation="true" />
                <ajax:CalendarExtender ID="calSettlementDate" TargetControlID="txtSettlementDate" runat="server" />
                <asp:CompareValidator ID="cvtxtSettlementDate" runat="server" ValidationGroup="val119HearingRights"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtSettlementDate" Enabled="true"
                    ErrorMessage="Select a valid Date for Settlement Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true" />
                <asp:CustomValidator ID="cusSettlementDate" ValidateEmptyText="true" runat="server" ControlToValidate="txtSettlementDate"
                    OnServerValidate="ValidateSettlementDate" Display="Dynamic" ValidationGroup="val119HearingRights"
                    ErrorMessage="*Date of Settlement must be after Denial/Termination Date." Text="*" />
            </div>
        </div>
        <br />
        <asp:PlaceHolder runat="server" ID="PlaceholderUploadSettlementAgreements"  Visible ="true"></asp:PlaceHolder>
    </asp:Panel>
</asp:Panel>
<asp:Panel ID="pnlAdjudicationOrder" runat="server">
    <span class="pageHeader">Adjudication Order</span>
    <br />
    <div class="row">
        <div class="col-sm-6 text-right">
            <asp:Label ID="lblDateOfAdjudicationOrder" AssociatedControlID="txtDateOfAdjudicationOrder" runat="server" CssClass="formLabel200">Date of Adjudication Order</asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtDateOfAdjudicationOrder" runat="server" CssClass="formField" Enabled="true" ValidationGroup="val119HearingRights" CauseValidation="true" />
            <ajax:CalendarExtender ID="calDateOfAdjudicationOrder" TargetControlID="txtDateOfAdjudicationOrder" runat="server" />
            <asp:CompareValidator ID="cvtxtDateOfAdjudicationOrder" runat="server" ValidationGroup="val119HearingRights"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateOfAdjudicationOrder" Enabled="true"
                ErrorMessage="Select a valid Date for Adjudication Order" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true" />
            <asp:CustomValidator ID="cusDateOfAdjudicationOrder" ValidateEmptyText="true" runat="server" ControlToValidate="txtDateOfAdjudicationOrder"
                OnServerValidate="ValidateDateOfAdjudicationOrder" Display="Dynamic" ValidationGroup="val119HearingRights"
                ErrorMessage="*Date of Adjudication Order must be between todays date and date of initial notice." Text="*" />
        </div>
    </div>
    <br />
    <asp:Panel ID="pnlAdjudicationOrderUploadControl" runat="server"  Visible="true">
        <asp:PlaceHolder runat="server" ID="PlaceholderUploadAdjudicationOrder"></asp:PlaceHolder>
    </asp:Panel>
</asp:Panel>
<asp:Panel ID="pnlDenialTerminationInfo" runat="server">
    <span class="pageHeader">Denial / Termination Information</span>
    <br />
    <div class="row">
        <div class="col-sm-6 text-right">
            <asp:Label ID="lblDateOfDenialTermination" AssociatedControlID="txtDateOfDenialTermination" runat="server" CssClass="formLabel200">Date of Denial / Termination</asp:Label>
        </div>
        <div class="col-sm-6">
            <asp:TextBox ID="txtDateOfDenialTermination" runat="server" CssClass="formField" Enabled="true" ValidationGroup="val119HearingRights" CauseValidation="true" />
            <ajax:CalendarExtender ID="calDateOfDenialTermination" TargetControlID="txtDateOfDenialTermination" runat="server" />
            <asp:CompareValidator ID="cvtxtDateOfDenialTermination" runat="server" ValidationGroup="val119HearingRights"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateOfDenialTermination" Enabled="true"
                ErrorMessage="Select a valid Date for Denial of Termination" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true" />
        </div>
    </div>
    <div class="row">
        <div class="col-sm-6 text-right">
            <asp:Label ID="lblTerminationReasons" AssociatedControlID="ddlTerminationReasons" runat="server" CssClass="formLabel200" Text="Termination Reason"></asp:Label>

        </div>
        <div class="col-sm-6">
            <asp:DropDownList ID="ddlTerminationReasons" runat="server" AutoPostBack="true" AppendDataBoundItems="True" CssClass="formDropDown" OnInit="ddlTerminationReasons_Init">
            </asp:DropDownList>
            <%--<asp:CustomValidator ID="cusTerminationReasons" ValidateEmptyText="true" runat="server" ControlToValidate="ddlTerminationReasons"
                OnServerValidate="ValidateTerminationReasons" Display="Dynamic" ValidationGroup="val119HearingRights"
                ErrorMessage="*A valid Termination Reason is required" Text="*" />--%>
        </div>
    </div>
</asp:Panel>
<asp:HiddenField ID="hidID" runat="server" />
<cc2:MessageBox ID="MessageBox2" runat="server" />
