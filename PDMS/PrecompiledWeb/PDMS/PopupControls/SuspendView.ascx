<%@ control language="C#" autoeventwireup="true" inherits="Views_SuspendView, App_Web_rqhgepvh" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<div style="padding:5px;">
    <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
    <asp:ValidationSummary ID="vsNewRequest" DisplayMode="List" runat="server"  ValidationGroup="DisenrollProvider" ShowSummary="true"  />
    <div class="pg-hint" style="padding-right:4px;">* Designates a required field</div><br /><br />
    <div style="width:auto;">
            <div class="row">
                <div class="col-sm-6 text-right"><span class="formLabel wd200">Effective Date</span></div>
                <div class="col-sm-6 text-left"><asp:Label ID="lblEffectiveDate" runat="server" CssClass="formFieldDisplay wd200" /></div>
            </div>
            <div class="row">
                <div class="col-sm-6 text-right"><span class="formLabel wd200">Enrollment Status</span></div>
                <div class="col-sm-6 text-left"><asp:Label ID="lblEnrollmentStatus" runat="server" CssClass="formFieldDisplay wd200" /></div>  
            </div>
            <div class="row">
                <div class="col-sm-6 text-right"><span class="formLabel wd200">Re-Validation Due Date</span></div>
                <div class="col-sm-6 text-left"><asp:Label ID="lblRevalDueDate" runat="server" CssClass="formFieldDisplay wd200" /></div>
            </div>
            <div class="row">
                <div class="col-sm-6 text-right"><asp:Label CssClass="formLabel wd200" runat="server" ID="lblDisenrollEffectiveDate">Suspend Claims Effective Date*</asp:Label></div>
                <div class="col-sm-6 text-left"><asp:TextBox ID="txtTermDate" runat="server" CssClass="formField wd200" MaxLength="10" />
                    <ajax:CalendarExtender ID="ceTermDate" TargetControlID="txtTermDate" runat="server" />
                    <asp:RequiredFieldValidator ID="valTermDateReqd" runat="server" SetFocusOnError="true" ValidationGroup="DisenrollProvider" Text="*"
                        ControlToValidate="txtTermDate" ErrorMessage="* Disenrollment Effective Date is required." Display="Dynamic"  Enabled="true" />
                    <asp:CompareValidator id="cvTermDate" runat="server" ValidationGroup="DisenrollProvider"   
                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtTermDate"   Enabled="true"
                        ErrorMessage="* A valid Disenrollment Effective Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                        SetFocusOnError="true"> 
                    </asp:CompareValidator>
                </div>
        </div>
        <div id="divsuspend" visible="true" class="row">
            <div class="col-sm-6 text-right">
                <asp:Label CssClass="formLabel wd200" runat="server" ID="lblEnrollstatusreason">Enrollment Status Reason Code*</asp:Label>

            </div>
            <div class="col-sm-6 text-left">
                <asp:DropDownList ID="ddlEnrollreason" runat="server" EnableViewState="true"></asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6 text-right"><span class="formLabel wd200">Comments*</span></div>
            <div class="col-sm-6 text-left"><asp:TextBox ID="txtComments" runat="server" Rows="7" CssClass="formField wd200" TextMode="MultiLine" MaxLength="4000" />
                <asp:RequiredFieldValidator ID="valCommentReqd" runat="server" SetFocusOnError="true" ValidationGroup="DisenrollProvider" Text="*"
                    ControlToValidate="txtComments" ErrorMessage="* Comments are required." Display="Dynamic"  Enabled="true" />
            </div>
        </div>
    </div>
    <div class="btnBox" style="padding-top:10px;padding-right:10px;">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="DisenrollProvider" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
    </div>
    <br />
</div>