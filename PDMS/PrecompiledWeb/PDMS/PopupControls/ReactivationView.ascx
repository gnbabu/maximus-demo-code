<%@ control language="C#" autoeventwireup="true" inherits="Views_ReactivationView, App_Web_rqhgepvh" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<div style="padding: 5px;">
    <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
    <asp:ValidationSummary ID="vsNewRequest" DisplayMode="List" runat="server" ValidationGroup="ReactivateProvider" ShowSummary="true" />

    
    <div id="divReactivate">
        <div class="row">
            <div class="pg-hint" style="padding-right: 4px;margin-right:20px !important;">* Designates a required field</div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><span class="formLabel wd200">Current Effective Date</span></div>
            <div class="col-sm-7">
                <asp:Label ID="lblEffectiveDate" runat="server" CssClass="formFieldDisplay wd250" /></div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><span class="formLabel wd200">Current Enrollment Status</span></div>
            <div class="col-sm-7">
                <asp:Label ID="lblEnrollmentStatus" runat="server" CssClass="formFieldDisplay wd250" /></div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><span class="formLabel wd200">Current Status End Date</span></div>
            <div class="col-sm-7">
                <asp:Label ID="lblRevalDueDate" runat="server" CssClass="formFieldDisplay wd250" /></div>
        </div>

        <div class="row">
           <div class="col-sm-4 text-right"> <span class="formLabel wd200">New Enrollment Status</span></div>
            <div class="col-sm-7">
                <asp:Label ID="lblNewEnrollmentStatus" runat="server" CssClass="formFieldDisplay wd250" />
				<asp:HiddenField ID="hdnNewEnrollmentStatusCode" runat="server" />
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><span class="formLabel wd200">New Revalidation Due Date*</span></div>
            <div class="col-sm-7">
                <asp:TextBox ID="txtReEnrollmentDueDate" runat="server" CssClass="formField wd250" MaxLength="10" />
                <ajax:CalendarExtender ID="ceReEnrollmentDueDate" TargetControlID="txtReEnrollmentDueDate" runat="server" />
                <asp:RequiredFieldValidator ID="valReEnrollmentDueDateReqd" runat="server" SetFocusOnError="true" ValidationGroup="ReactivateProvider" Text="*"
                    ControlToValidate="txtReEnrollmentDueDate" ErrorMessage="* New Revalidation Due Date is required." Display="Dynamic" Enabled="true" />
                <asp:CompareValidator ID="cvReEnrollmentDueDate" runat="server" ValidationGroup="ReactivateProvider"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtReEnrollmentDueDate" Enabled="true"
                    ErrorMessage="* A valid New Revalidation Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> 
                </asp:CompareValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><span class="formLabel wd200">Comments*</span></div>
            <div class="col-sm-7">
                <asp:TextBox ID="txtComments" runat="server" Rows="7" CssClass="formField wd250" TextMode="MultiLine" MaxLength="4000" />
                <asp:RequiredFieldValidator ID="valCommentReqd" runat="server" SetFocusOnError="true" ValidationGroup="TerminateProvider" Text="*"
                    ControlToValidate="txtComments" ErrorMessage="* Comments are required." Display="Dynamic" Enabled="true" />
            </div>
        </div>
    </div>
    <br />
    <div class="btnBox" style="padding-top: 10px; padding-right: 10px;">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="DisenrollProvider" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
    </div>
</div>
