<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_NewTerminationDateView, App_Web_glma3lal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<div style="padding: 5px;">
    <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
    <asp:ValidationSummary ID="vsNewRequest" DisplayMode="List" runat="server" ValidationGroup="NewTermDate" ShowSummary="true" />
    <div class="pg-hint" style="padding-right: 4px;">* Designates a required field</div><br /><br />
    <div style="width: auto;">
        <div class="row">
            <div class="col-sm-4 text-right"><span class="formLabel wd200">Reg ID</span></div>
            <div class="col-sm-8 text-left">
                <asp:Label ID="lblRegID" runat="server" CssClass="formFieldDisplay wd350" /></div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><span class="formLabel wd200">Current Termination Date</span></div>
            <div class="col-sm-8 text-left">
                <asp:Label ID="lblTermDate" runat="server" CssClass="formFieldDisplay wd350" /></div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><span class="formLabel wd200">New Termination Date*</span></div>
            <div class="col-sm-8 text-left">
                <asp:TextBox ID="txtTermDate" runat="server" CssClass="formField wd350" MaxLength="10" />
                <ajax:CalendarExtender ID="ceTermDate" TargetControlID="txtTermDate" runat="server" />
                <asp:RequiredFieldValidator ID="valTermDateReqd" runat="server" SetFocusOnError="true" ValidationGroup="NewTermDate" Text="*"
                    ControlToValidate="txtTermDate" ErrorMessage="* Termination Effective Date is required." Display="Dynamic" Enabled="true" />
                <asp:CompareValidator ID="cvTermDate" runat="server" ValidationGroup="NewTermDate"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtTermDate" Enabled="true"
                    ErrorMessage="* A valid Termination Effective Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> 
                </asp:CompareValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-4 text-right"><span class="formLabel wd200">Comments*</span></div>
            <div class="col-sm-8 text-left">
                <asp:TextBox ID="txtComments" runat="server" Rows="7" CssClass="formField wd350" TextMode="MultiLine" MaxLength="4000" />
                <asp:RequiredFieldValidator ID="valCommentReqd" runat="server" SetFocusOnError="true" ValidationGroup="NewTermDate" Text="*"
                    ControlToValidate="txtComments" ErrorMessage="* Comments are required." Display="Dynamic" Enabled="true" />
            </div>
        </div>
    </div>
    <div class="btnBox" style="padding-top: 10px; padding-right: 10px;">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="DisenrollProvider" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
    </div>
</div>