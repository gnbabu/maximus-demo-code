<%@ control language="C#" autoeventwireup="true" inherits="Views_CallAddView, App_Web_p4ixifjm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="wc" %>
<script type="text/javascript">
    //$(document).ready(function () {
    //    var chk = document.getElementById("<%=chkNoRegID.ClientID%>");
    //    ToggleValidator(chk);
    //});

    //function ToggleValidator(chk) {
    //    var valName = document.getElementById("<%=valRegID.ClientID%>");
    //    if (chk.checked)
    //        ValidatorEnable(valName, false);
    //    else
    //        ValidatorEnable(valName, true);
    //}
</script>
<style type="text/css">
    .CallText {
        width:98% !important;
    }
</style>
<div style="padding: 5px; height: auto;">
    <div style="text-align: left;">
        <asp:Label runat="server" ID="lblSuccessMessage" CssClass="error-message" Text="Call Successfully Inserted" Visible="false" />
        <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
        <asp:ValidationSummary ID="vsNewCall" DisplayMode="List" runat="server" ValidationGroup="AddCall" ShowSummary="true" Enabled="true" CssClass="error-message" />
    </div>

    <div class="pg-hint" style="padding-right: 4px;">* Designates a required field</div>
    <br />
    <br />
    <div style="text-align: left; padding-top: 10px; display: inline; width: auto;">
        <asp:Button ID="btnStart" runat="server" Text="Start Timer" CssClass="buttonBoxFocus" OnClick="btnStart_Click" CausesValidation="false" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel Timer" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
        <asp:UpdatePanel ID="upTimer" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <asp:Label ID="lblForTimer" runat="server" CssClass="fieldLabel wd100" Text="Time(MM:SS)"></asp:Label><asp:Label ID="lblTimer" runat="server" CssClass="fieldValue bodyTextBold" Text="00:00"></asp:Label>
                <asp:Timer ID="tmrTimer" Interval="100" runat="server" OnTick="tmrTimer_Tick" Enabled="false" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="tmrTimer" EventName="Tick" />
            </Triggers>
        </asp:UpdatePanel>

    </div>
    <br />
    <br />
    <div style="width: auto;">
        <div class="row">
            <div class="col-sm-6"><span class="formLabel150"><asp:Label  ID="lblCallID"  runat="server" AssociatedControlID="txtCallID" Text="Call ID"></asp:Label></span>&nbsp;&nbsp;
                <asp:TextBox ID="txtCallID" runat="server" CssClass="formField" MaxLength="20" Enabled="false" /></div>
            <div class="col-sm-6"></div>
        </div>
        <div class="row">
            <div class="col-sm-6"><span class="formLabel150"><asp:Label  ID="lblRegID"  runat="server" AssociatedControlID="txtRegID" Text="Reg ID*"></asp:Label></span>&nbsp;&nbsp;
                <asp:TextBox ID="txtRegID" runat="server" CssClass="formField" MaxLength="20" />
                <asp:RequiredFieldValidator ID="valRegID" runat="server" ControlToValidate="txtRegID" 
                        ErrorMessage="Reg ID is required." Enabled="true" ValidationGroup="AddCall" Text="*" 
                        Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator></div>
                <div class="col-sm-4 text-left"><asp:CheckBox ID="chkNoRegID" runat="server" CssClass="fieldChk" Text="No Registration On File" AutoPostBack="true" OnCheckedChanged="chkNoRegID_CheckedChanged" />
                <%--<input type="checkbox" id="chkNoRegID" runat="server" class="fieldChk" onclick="ToggleValidator(this)" />No Registration On File--%>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6"><span class="formLabel150"><asp:Label  ID="lblNPI"  runat="server" AssociatedControlID="txtNPI" Text="NPI"></asp:Label></span>&nbsp;&nbsp;
            <asp:TextBox ID="txtNPI" runat="server" CssClass="formField" MaxLength="10" />
                <asp:RegularExpressionValidator ID="valNPI" runat="server" ControlToValidate="txtNPI" ErrorMessage="NPI must be 10 digits and start with 1 or 2."
                    ValidationGroup="AddCall" ValidationExpression="^([1-2][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$" Display="Dynamic" 
                    SetFocusOnError="true" Text="*"></asp:RegularExpressionValidator>
            </div>
            <div class="col-sm-6"><span class="formLabel150"><asp:Label  ID="lblMedicaidID"  runat="server" AssociatedControlID="txtMedicaidID" Text="Medicaid ID"></asp:Label></span>&nbsp;&nbsp;
            <asp:TextBox ID="txtMedicaidID" runat="server" CssClass="formField" MaxLength="9" />
                <asp:RegularExpressionValidator ID="valMedicaidID" runat="server" ControlToValidate="txtMedicaidID" ErrorMessage="Medicaid ID must be 9 digits and start with 0."
                    ValidationGroup="AddCall" ValidationExpression="^([0][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$" Display="Dynamic" 
                    SetFocusOnError="true" Text="*"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6"><span class="formLabel150"><asp:Label  ID="lblSource"  runat="server" AssociatedControlID="ddlSource" Text="Who is the Caller?*"></asp:Label></span>&nbsp;&nbsp;
                <asp:DropDownList ID="ddlSource" runat="server" CssClass="formField" /><asp:CompareValidator runat="server" ID="valSourceReqd" ControlToValidate="ddlSource"
                    ValueToCompare="0" Type="Integer" ErrorMessage="A value for Who is the Caller is required." Enabled="true"
                    Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="AddCall" />
            </div>
            <div class="col-sm-6"><span class="formLabel150"><asp:Label  ID="lblReason"  runat="server" AssociatedControlID="ddlReason" Text="Reason for Calling*"></asp:Label></span>&nbsp;&nbsp;
                <asp:DropDownList ID="ddlReason" runat="server" CssClass="formField" /><asp:CompareValidator runat="server" ID="vallReasonReqd" ControlToValidate="ddlReason"
                    ValueToCompare="0" Type="Integer" ErrorMessage="A value for Reason for Calling is required." Enabled="true"
                    Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="AddCall" /></div>
        </div>
        <div class="row">
            <%--                Can make these fields as large as need to, db size is max.  Making 200 based on visual pulled over from demo--%>
            <div class="col-sm-6"><span class="formLabel150"><asp:Label  ID="lblCallerOther"  runat="server" AssociatedControlID="txtCallerOther" Text="Caller Other"></asp:Label></span>&nbsp;&nbsp;
                <asp:TextBox ID="txtCallerOther" runat="server" CssClass="formField" MaxLength="200" /></div>
            <div class="col-sm-6"><span class="formLabel150"><asp:Label  ID="lblReasonOther"  runat="server" AssociatedControlID="txtReasonOther" Text="Reason Other"></asp:Label></span>&nbsp;&nbsp;
                <asp:TextBox ID="txtReasonOther" runat="server" CssClass="formField" MaxLength="200" /></div>
        </div>
        <div class="row">
            <div class="col-sm-6"><span class="formLabel150"><asp:Label  ID="lblResolution"  runat="server" AssociatedControlID="ddlResolution" Text="Action Taken*"></asp:Label></span>&nbsp;&nbsp;
                <asp:DropDownList ID="ddlResolution" runat="server" CssClass="formField" /><asp:CompareValidator runat="server" ID="valResolutionReqd" ControlToValidate="ddlResolution"
                    ValueToCompare="0" Type="Integer" ErrorMessage="A value for Action Taken is required."
                    Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="AddCall" /></div>     
        </div>
        <div class="row">
            <div class="col-sm-1"></div>
            <div class="col-sm-9">
                <wc:GroupBox ID="grpCallDetails" runat="server" Caption="Call Details" ToolTip="Call Details">
                    <asp:RequiredFieldValidator ID="valCallDetails" runat="server" ControlToValidate="txtCallDetails" 
                        ErrorMessage="Call Details are required." Enabled="true" ValidationGroup="AddCall" Text="*" 
                        Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtCallDetails" runat="server" TextMode="MultiLine" Rows="5" CssClass="CallText" title="Call Details"></asp:TextBox>
                </wc:GroupBox>
            </div>
        </div>
    </div>
    <div class="btnBox" style="margin-left:auto; margin-right:auto;text-align:center">
        <asp:Button ID="btnSaveAndEnd" runat="server" Text="Save and End Call" CssClass="buttonBoxFocus" OnClick="btnSaveAndEnd_Click" CausesValidation="true" ValidationGroup="AddCall" Enabled="false" />
        <asp:Button ID="btnSaveAndCont" runat="server" Text="Save and Continue" CssClass="buttonBox" OnClick="btnSaveAndCont_Click" CausesValidation="true" ValidationGroup="AddCall" Enabled="false" />
    </div>
    <br />
    <br />
    <br />
</div>
