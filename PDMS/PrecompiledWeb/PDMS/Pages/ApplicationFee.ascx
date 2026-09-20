<%@ control language="C#" autoeventwireup="true" inherits="Pages_ApplicationFee, App_Web_roucadzr" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>

<script type="text/javascript">
    $(document).ready(function(){

    });
    function TogglePaymentType(divID, divPayByECheck, divPayByPaperCheck, divRequestWaiver,divComments,tbl) {
        //$("#divPayByECheck").hide();
        //$("#divPayByPaperCheck").hide();
      //  $("#divRequestWaiver").hide();
        //$("#divComments").hide();
        document.getElementById(divPayByECheck).style.display = "none";
        document.getElementById(divPayByPaperCheck).style.display = "none";
        document.getElementById(divRequestWaiver).style.display = "none";
        document.getElementById(divComments).style.display = "none";
        document.getElementById(tbl).style.display = "none";
       // $("#tblPaperCheck").hide();
        if (divID == "divPayByECheck") {
           // $("#divPayByECheck").show();
            //$("#divComments").show();
            document.getElementById(divPayByECheck).style.display = "block";
            document.getElementById(divComments).style.display = "block";
        }
        else if(divID == "divPayByPaperCheck") {
           // $("#divPayByPaperCheck").show();
           // $("#tblPaperCheck").show();
            //$("#divComments").show();
            document.getElementById(divPayByPaperCheck).style.display = "block";
            document.getElementById(divComments).style.display = "block";
            document.getElementById(tbl).style.display = "block";
        }
        else if (divID == "divRequestWaiver") {
            //$("#divRequestWaiver").show();
            //$("#divComments").show();
            document.getElementById(divRequestWaiver).style.display = "block";
            document.getElementById(divComments).style.display = "block";
        }
    }
    function openLink(url) {
        window.open(url, 'newWindow');
    }
</script>

<script>
    
    function resultCallback(result, account, errors) {
        
        if (result == "success") {
            
             // Properties available in account parameter:
             //
             // account.identifier: The identifier of account that was selected from the wallet.
             // account.singleuse: If true, then the account was not stored in wallet and may only be used for a single payment authorization via the transaction service.
             // account.name: The name of the account holder.
             // account.type: The account type (amex, diners, discover, jcb, mastercard, visa, checking. savings).
             // account.lastfourdigits: The last four digits of the account number.
             // account.expirationmonth: The expiration month account (credit card only).
             // account.expirationyear: The expiration year account (credit card only).
             if (account != null) {
                 
                var typeDictionary = {
                    amex: "American Express", diners: "Diners Club",
                    discover: "Discover", jcb: "JCB", mastercard: "MasterCard",
                    visa: "Visa", checking: "Checking", savings: "Savings"
                };
 
                $("#selectedWalletAccount").removeClass("d-none");
                $("#selectedWalletAccount").text(typeDictionary[account.type] + " ... " + account.lastfourdigits);
                
                $("#hdnAccountNumber").val(account.number);

                $("#hdnAccountIdentifier").val(account.identifier);
                //$("#hdnAccountNumber").value = account 
                $("#hdnAccountFirstName").val(account.name);
                $("#hdnLastName").val(account.name);
                $("#hdnAccountType").val(account.type);
                $("#hdnlastfourdigits").val(account.lastfourdigits);
                $("#hdnExpirationMonth").val(account.expirationmonth); 
                 $("#hdnExpirationYear").val(account.expirationyear);
                 document.getElementById('<%= btnCompletePayment.ClientID %>').disabled = false;
            }
        }
         else if (result == "error") {
            alert("There was an issue selecting an account: " + errors);
        }
     }
    
</script>
<%-- Note This following script is to show/hide sections baesd upon what was selected for payment type. Since it was not triggering document .ready() in the popup control, it is writtern in the containing control --%>
<script type="text/javascript">
    $(document).ready(function () {
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(setupPaymentSection);
    });

    function setupPaymentSection() {
        var value = $(".payment-type input[type='radio']:checked").val();

        $("#divPayByECheck").hide();
        $("#divPayByPaperCheck").hide();
        $("#divRequestWaiver").hide();
        $("#divComments").hide();
        if (value != null) {
            if (value.toLowerCase() == "1") {
                $("#divPayByECheck").show();
                $("#divComments").show();
            }
            else if (value.toLowerCase() == "2") {
                $("#divPayByPaperCheck").show();
                $("#tblPaperCheck").show();
                $("#divComments").show();
            }
            else if (value.toLowerCase() == "3") {
                $("#divRequestWaiver").show();
                $("#divComments").show();
            }
        }
    }
</script>

<span class="pageHeader">Application Fee</span>
<br />

    <div class="boxPanelFull">
        <div>All prospective, re-enrolling, and reactivating institutional providers are required to pay an application fee.  
You may request a waiver of the fee if you are already enrolled in Medicare and have already paid the application fee to Medicare.
You may also request a waiver of the fee if you have paid the fee to another State Medicaid program.  
The current amount of the fee is <asp:Label ID="FeeAmount1" runat="server"></asp:Label></div><br />
        You may also request a waiver of the fee if you have paid within the past 5 years.  
        <div>
            <div class="row">
                <div class="col-sm-3 text-right"><asp:Label ID="lbl4FeeAmount" runat="server" CssClass="formLabel"> Fee Amount</asp:Label></div>
                <div class="col-sm-9"><asp:Label ID="lblFeeAmount" runat="server" CssClass="formLabelData"> </asp:Label></div>
            </div> 
            <div class="row">
                <div class="col-sm-3 text-right"><asp:Label ID="lbl4FeeStatus" runat="server" CssClass="formLabel"> Fee Status</asp:Label></div>
                <div class="col-sm-9"><asp:Label ID="lblFeeStatus" runat="server" CssClass="formLabelData" Text="Pending" /></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"  style="height:20px;padding-top:7px;vertical-align:top;"><asp:Label ID="lbl4PaymentType" runat="server" CssClass="formLabel"> Payment Type</asp:Label></div>
                <div class="col-sm-4 text-left" style="vertical-align:bottom;padding:0px; ">
                    <fieldset>
                        <legend>
                         <label>
                             <asp:RadioButtonList ID="rblPaymentType" runat="server" CssClass="radioButtonList" RepeatDirection="Vertical" OnDataBound="rblPaymentType_DataBound" OnSelectedIndexChanged="rblPaymentType_SelectedIndexChanged" AutoPostBack="true"/>
                        </label>
                        </legend>
                    </fieldset>
                </div>
                <div class="col-sm-3">
<%--                    Only show this when payment type selected is paper check--%>
                    <div id="tblPaperCheck" style="padding-left: 10px; width:350px;display:none;" runat="server">
                        <div class="row">
                            <b>Send Payment To:</b>
                        </div>
                        <div class="row">
                            <asp:Literal id= "ltlPaperCheckSendToAddr" runat="server" Text="<%$ Resources:BrandingResource , APPLICATION_FEE_PAPER_CHECK_SENDTO_ADDRESS %>" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        
       <div id="divPayByECheck" class="show" style="padding-top:10px;" runat="server"> 

           
           <table style="padding-top:10px;">
               <tr>
                   <td style="padding-right:10px;">
                       
                        <asp:Button ID="btnCompletePayment" runat="server" Text="Authorize Payment" CssClass="buttonBoxAppFee" OnClick="btnAuthorizePayment_ServerClick" CausesValidation="true"/>
                   </td>
                   <td>
                       
                        
                       <Button id = "btnSelectPayment" type="button" runat="server" data-target="#Wallet" Class="buttonBoxFocus"  OnClick="cbossWallet.showModal(this, resultCallback);" Text="Select Payment" >Select Payment
                       </Button>
                       <span id="selectedWalletAccount" class="u-badge-v1 g-rounded-3 d-none"></span>
                   </td>
               </tr>
           </table>
       </div>
        <div id="divPayByPaperCheck" class="show" style="padding-top:10px;" runat="server">
            
            <asp:Label ID="lblAmount" runat="server" CssClass="formLabel">Amount*</asp:Label>
            <asp:TextBox ID="txtAmount" runat="server" aria-Label="Amount" MaxLength="5"  CssClass="formField"/>
            <%--<asp:RequiredFieldValidator ID="rfvAmount" runat="server" SetFocusOnError="true" ValidationGroup="valProviderInfoHeader" Text="*"
                ControlToValidate="txtAmount" ErrorMessage="Enter Fee amount" Display="Dynamic" />
            <asp:RegularExpressionValidator ID="revAmount" ControlToValidate="txtAmount" runat="server" ValidationExpression="^[0-9]*$" />--%>
        </div>
        <div id="divRequestWaiver" style="padding-top:10px;" runat="server">
            <asp:Label ID="lblWaiverReason" runat="server" text="Waiver Reason" CssClass="formLabel"/>
            <asp:DropDownList ID="ddlWaiverReason" runat="server" aria-Label="Waiver Reason" CssClass="formDropDown" AutoPostBack="false" />
        </div>
        
        <asp:Panel ID="pnlVerifyPECOS"  runat="server" style="padding-top:10px;" Visible ="false">
        <div ID="divVerifyPECOS"  style="padding-top:10px;">
            <asp:CheckBox ID="chkVerifiedPECOS" runat="server" Text="I confirm that payment information has been verified in PECOS." OnCheckedChanged="chkVerifiedPECOS_CheckedChanged" AutoPostBack="true"  />
            <asp:LinkButton ID="lnkVerifyPECOS" runat="server"  OnClick="Verify_PECOS_Click">Search PECOS.</asp:LinkButton>
        </div>
        </asp:Panel>

        <div id="divComments"  style="padding-top:10px;" runat="server">
            <asp:Label ID="lblComments" runat="server" Text="Comments" AssociatedControlID="txtComments" CssClass="formLabel"></asp:Label>
            <asp:TextBox ID="txtComments" runat="server" aria-Label="Comments" TextMode="MultiLine"  Rows="4" CssClass="formFieldLarge" />
        </div>
    </div>
<br />
    <div id="divgrdFeePaymentHistory" class="show" style="padding-bottom:10px;">
        <span class="pageHeader">Fee Payment History</span><br />
        <asp:GridView ID="grdFeePaymentHistory" runat="server" AutoGenerateColumns="False" EmptyDataText="No payment information found."  HorizontalAlign="Left" CssClass="gridview" ShowHeader="true" ShowHeaderWhenEmpty="true">
            <Columns>
            <asp:BoundField DataField="FEE_AMOUNT" HeaderText="Fee Amount"  />
            <asp:BoundField DataField="FEE_STATUS_NAME" HeaderText="Fee Status" />
            <asp:BoundField DataField="FEE_STATUS_DATE" HeaderText="Status Date"  />
            <asp:BoundField DataField="APPLICATION_FEE_WAIVER_REASON_ID" HeaderText="Waiver Reason"  />
                <asp:BoundField DataField="TRANSACTION_ID" HeaderText="Transaction ID"  />
        </Columns>
            <PagerStyle CssClass="gridViewPager"  />  
            <HeaderStyle CssClass="gridViewHeader"/>
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" /> 
            <FooterStyle CssClass="gridViewFooter" />
            <SelectedRowStyle CssClass="gridViewSelected" />
        </asp:GridView><br />
    </div>
    <asp:HiddenField ID="hdnRegAppFeeID" runat="server" ClientIDMode="Static"/>
<asp:HiddenField ID="hdnAccountIdentifier" runat="server" ClientIDMode="Static"/>
<asp:HiddenField ID="hdnAccountNumber" runat="server" ClientIDMode="Static"/>
<asp:HiddenField ID="hdnAccountFirstName" runat="server" ClientIDMode="Static"/>
<asp:HiddenField ID="hdnLastName" runat="server" ClientIDMode="Static"/>
<asp:HiddenField ID="hdnAccountType" runat="server" ClientIDMode="Static"/>
<asp:HiddenField ID="hdnlastfourdigits" runat="server" ClientIDMode="Static"/>
<asp:HiddenField ID="hdnExpirationMonth" runat="server" ClientIDMode="Static"/>
<asp:HiddenField ID="hdnExpirationYear" runat="server" ClientIDMode="Static"/>
<br />
