<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_PriorAuthorizationAndReferringPanel" Codebehind="PriorAuthorizationAndReferringPanel.ascx.cs" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">

<br />
<script type="text/javascript">
    function loaderPriorAuthorization() {

        <%--document.getElementById('<%= txtReferralNumber.ClientID %>').disabled = true;
        document.getElementById('<%= txtPriorAuthNumber.ClientID %>').disabled = true;
        document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';--%>

    }
    function loaderPriorAuthorization1() {

        <%--document.getElementById('<%= txtPriorAuthNumber.ClientID %>').disabled = true;
        document.getElementById('<%= txtReferralNumber.ClientID %>').disabled = true;
         document.getElementById('<%= btnloading1.ClientID %>').style.display = 'block';--%>

     }

    function alphanumericOnly(obj) {
      obj.value = obj.value.replace(/[^a-zA-Z0-9- ]/g, '');
    }
</script>
<asp:Panel ID="pnlPriorAuthInfo" runat="server" Style="min-height: 61px; min-width: 66px; height: auto; width: auto; max-width: 98%;" ScrollBars="None">
    <asp:UpdatePanel id="UpdatePanel2" runat="server" >
    <contenttemplate>
    <asp:HiddenField ID="hdnClaimIdPriorAuthRef" runat="server" />

    <div >
    <div class="row"><span style="color:red;text-align:left !important; margin-left:50px !important;margin-bottom:15px !important">Please ensure a Valid Auth Number is entered. Inaccurate entry will result in delay or denial of Claim Processing</span></div>
    <div class="row PriorAuthNumber" style="text-align: center;margin-top:10px" runat="server">

            <div class="col-sm-6">
                <div class="col-sm-5 text-right">
                    <span class="ohio-field" style="font-size: 16px; text-align: right">Prior Authorization Number</span>
                </div>
                <div class="col-sm-7 text-left">
                    
                    <asp:TextBox ID="txtPriorAuthNumber" onChange="loaderPriorAuthorization1()" runat="server" CssClass="formField" MaxLength="50" Width="200px" Height="30" onKeyUp="javascript:alphanumericOnly(this);" AutoPostBack="False" />
                     <button id="btnloading1" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                     
                    <asp:RegularExpressionValidator ID="EmojiValidation" runat="server" ControlToValidate="txtPriorAuthNumber" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                     
                </div>
            </div>

            <div class="col-sm-6">
                <div class="col-sm-5 text-right">
                    <span class="ohio-field" style="font-size: 16px; text-align: right">Referral Number</span>
                </div>
                <div class="col-sm-7 text-left">
                    <asp:TextBox ID="txtReferralNumber" runat="server" CssClass="formField" MaxLength="50" Width="200px" Height="30" onKeyUp="javascript:alphanumericOnly(this);" onChange="loaderPriorAuthorization()" AutoPostBack="False"/>
                     <button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                     
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtReferralNumber" ValidationExpression="^[A-Za-z0-9? ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                </div>
            </div>

        </div>
    </div>
       </contenttemplate>       
        </asp:UpdatePanel>
</asp:Panel>
