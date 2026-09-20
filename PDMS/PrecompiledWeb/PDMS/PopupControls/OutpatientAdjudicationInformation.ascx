<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_OutpatientAdjudicationInformation, App_Web_l5y5araq" %>
<script>
    function onlyDotsAndNumbers(txt, event) {        
        var charCode = (event.which) ? event.which : event.keyCode
        if (charCode == 46) {
            if (txt.value.indexOf(".") < 0)
                return true;
            else
                return false;;
        }
        if (txt.value.indexOf(".") > 0) {
            var txtlen = txt.value.length;
            var dotpos = txt.value.indexOf(".");
            if ((txtlen - dotpos) > 2)
                return false;
        }
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        else
            return true;
    }



</script>

<asp:UpdatePanel ID="upnlOutpatientAdjudication" runat="server">
    <ContentTemplate>
        <div class="row col-sm-12 " runat="server">
            <div class="col-sm-6">
                <asp:HiddenField ID="hdnAdjudicationInformationID" runat="server" />
                <asp:HiddenField ID="hdnOutpatientAdjudicationInfo" runat="server" />
                <div class="col-sm-5 text-right">
                    <span class="ohio-field" style="font-size: 16px; text-align: right">Reimbursement Rate(Percentage as decimal):</span>
                </div>
                <div class="col-sm-7 text-left">
                    <asp:TextBox ID="txtReimbursementRate" runat="server" CssClass="formField" MaxLength="10" Width="200px" Height="30" onkeypress="return onlyDotsAndNumbers(this,event);" />
                </div>
                <asp:RangeValidator ID="RangevalReimbursement" runat="server" ControlToValidate="txtReimbursementRate"
                    ErrorMessage="Reimbursement rate cannot be greater than 1" ForeColor="Red" MaximumValue="1.0" MinimumValue="0.0"
                    SetFocusOnError="True" Type="Double"></asp:RangeValidator>
            </div>
            <div class="col-sm-6">
                <div class="col-sm-5 text-right">
                    <span class="ohio-field" style="font-size: 16px; text-align: right">Claim Remark Code(MOA 03):</span>
                </div>
                <div class="col-sm-7 text-left">
                    <asp:TextBox ID="txtClaimRemarkCodeMOA03" runat="server" CssClass="formField" MaxLength="5" Width="200px" Height="30" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtClaimRemarkCodeMOA03" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                </div>
            </div>
        </div>
        <div class="row col-sm-12 " runat="server">
            <div class="col-sm-6">
                <div class="col-sm-5 text-right">
                    <span class="ohio-field" style="font-size: 16px; text-align: right">HCPCS Payable Amount:</span>
                </div>
                <div class="col-sm-7 text-left">
                    <asp:TextBox ID="txtHCPCSPayableAmt" runat="server" CssClass="formField" onkeypress="return onlyDotsAndNumbers(this,event);" MaxLength="18" Width="200px" Height="30" />
                </div>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtHCPCSPayableAmt" ValidationExpression="^[0-9]+\.[0-9]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
            </div>
            <div class="col-sm-6">
                <div class="col-sm-5 text-right">
                    <span class="ohio-field" style="font-size: 16px; text-align: right">Claim Remark Code(MOA 04):</span>
                </div>
                <div class="col-sm-7 text-left">
                    <asp:TextBox ID="txtClaimRemarkCodeMOA04" runat="server" CssClass="formField" MaxLength="5" Width="200px" Height="30" />
                </div>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtClaimRemarkCodeMOA04" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
            </div>
        </div>
        <div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
