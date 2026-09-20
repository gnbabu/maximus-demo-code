<%@ Control Language="C#" AutoEventWireup="true" Inherits="Views_ExpressTerminationView" Codebehind="ExpressTerminationView.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<div style="padding: 5px;">
    <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
    <asp:ValidationSummary ID="vsNewRequest" DisplayMode="List" runat="server" ValidationGroup="TerminateProvider" ShowSummary="true" />
    <div class="pg-hint" style="padding-right: 4px;">* Designates a required field</div><br /><br />
    <div style="width: auto;">
        <div class="row">
            <div class="col-sm-4 text-right"><span class="formLabel wd200">Effective Date</span></div>
            <div class="col-sm-8 text-left">
                <asp:Label ID="lblEffectiveDate" runat="server" CssClass="formFieldDisplay wd450" /></div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><span class="formLabel wd200">Enrollment Status</span></div>
            <div class="col-sm-8 text-left">
                <asp:Label ID="lblEnrollmentStatus" runat="server" CssClass="formFieldDisplay wd450" /></div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><span class="formLabel wd200">Revalidation Due Date</span></div>
            <div class="col-sm-8 text-left">
                <asp:Label ID="lblRevalDueDate" runat="server" CssClass="formFieldDisplay wd450" /></div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><span class="formLabel wd200">Termination Effective Date*</span></div>
            <div class="col-sm-8 text-left">
                <asp:TextBox ID="txtTermDate" runat="server" CssClass="formField" MaxLength="10" />
                <ajax:CalendarExtender ID="ceTermDate" TargetControlID="txtTermDate" runat="server" />
                <asp:RequiredFieldValidator ID="valTermDateReqd" runat="server" SetFocusOnError="true" ValidationGroup="TerminateProvider" Text="*"
                    ControlToValidate="txtTermDate" ErrorMessage="* Termination Effective Date is required." Display="Dynamic" Enabled="true" />
                <asp:CompareValidator ID="cvTermDate" runat="server" ValidationGroup="TerminateProvider"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtTermDate" Enabled="true"
                    ErrorMessage="* A valid Termination Effective Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> 
                </asp:CompareValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><span class="formLabel wd200">New Enrollment Status*</span></div>
            <div class="col-sm-8 text-left">
                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="formDropDown" MaxLength="10" />
                <asp:RequiredFieldValidator ID="valStatusReqd" runat="server" SetFocusOnError="true" ValidationGroup="TerminateProvider" Text="*"
                    ControlToValidate="ddlStatus" ErrorMessage="* New Enrollment Status is required." Display="Dynamic" Enabled="true" InitialValue="" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><span class="formLabel wd200">Comments*</span></div>
            <div class="col-sm-8 text-left">
                <asp:TextBox ID="txtComments" runat="server" Rows="7" CssClass="formField" TextMode="MultiLine" MaxLength="4000" />
                <asp:RequiredFieldValidator ID="valCommentReqd" runat="server" SetFocusOnError="true" ValidationGroup="TerminateProvider" Text="*"
                    ControlToValidate="txtComments" ErrorMessage="* Comments are required." Display="Dynamic" Enabled="true" />
            </div>
        </div>
    </div>
    <div class="btnBox" style="padding-top: 10px; padding-right: 10px;">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="DisenrollProvider" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
    </div>
</div>
