 <%@ Control Language="C#" AutoEventWireup="true" Inherits="Pages_ACHAuthorization" Codebehind="ACHAuthorization.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc2" %>
<%@ Register Src="~/PopupControls/ACHBankingInfo.ascx" TagPrefix="uc" TagName="BankingInfo" %>
<%@ Register Src="~/PopupControls/FeeInformation.ascx" TagPrefix="uc" TagName="FeeInformation" %>
<%@ Register Src="~/PopupControls/ACHEftContact.ascx" TagPrefix="uc" TagName="EftContact" %>
<%@ Register Src="~/PopupControls/ACHVendorInfo.ascx" TagPrefix="uc" TagName="VendorInfo" %>
<%@ Register Src="~/PopupControls/ACHHistory.ascx" TagPrefix="uc" TagName="History" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/ACHReliaCardAuthorization.ascx" TagPrefix="uc" TagName="ReliaCard" %>
<%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
<style type="text/css">
    .cpHeader {
        background-color: White;
        color: #0000FF;
        cursor: pointer;
        font-size: 9pt;
        font-weight: bold;
    }

        .cpHeader a {
            color: #0000FF;
            text-decoration: underline;
        }

    .cpBody {
        width: 100%;
    }

    .divHistoryAndAdd {
        width: 100%;
        text-align: right;
    }

    .divGrid {
        width: 100%;
    }

    .tab {
        margin-left: 40px;
    }

    .checkBox {
        padding-left: 120px;
        font-weight: bold;
    }

    .gridview {
        float: right;
    }
    .confirm {
        color: red;
    }

    #ctl00_MainContent_225_upShowNames1 {
        display: none;
    }
</style>

<script  type="text/javascript">
    function ACHAnswerIsYes(rblId) {
        if (document.getElementById(rblId) != null) {
            var oElem = document.getElementById(rblId);
            var radio = oElem.getElementsByTagName("input");
            return radio[0].checked;
        }
        return false;
    }

    function ACHTogglePanelYes(rblId, pnlId) {
        var remit = document.getElementById('<%=pnlRemit.ClientID %>');
        if (ACHAnswerIsYes(rblId)) {
            document.getElementById(pnlId).style.display = "block";
            remit.style.display = "block";
        }
        else {
            document.getElementById(pnlId).style.display = "none";
            remit.style.display = "none";
        }
    }
    function ToggleVisible(ctrlID, divClientID) {
        
        var HideMe = document.getElementById(ctrlID).checked;
        if (HideMe) {
            document.getElementById(divClientID).style.display = "none";
        }
        else {
            document.getElementById(divClientID).style.display = "block";
        }
    }
    
    function ToggleWaiverVisible(bankInfo, waiverBankInfo, chlClientID, confirmClientID) {
        
        if (chlClientID == "1") {
            document.getElementById(bankInfo).style.display = "block";
            document.getElementById(waiverBankInfo).style.display = "none";
            document.getElementById(confirmClientID).style.display = "block";
        }
        else if (chlClientID == "2")
        {
            document.getElementById(bankInfo).style.display = "none";
            document.getElementById(waiverBankInfo).style.display = "block";
            document.getElementById(confirmClientID).style.display = "block";
        }
        
    }

    //OHPNM-13757 The UploadSecionControl is being added, and has the upShowNames1, which is the Proof of Fee -- if that exists on the page, hide it
    function removeFeeDocuments() {
        var elements = document.querySelector('upShowNames1');
        for (var x = 0; x < elements.length; x++) {
            element[x].style.display = 'none;'
        }
        var element = document.getElementById('ctl00_MainContent_225_upShowNames1');
        if (element != null) {
            element.style.display = 'none';
        }
    }

    window.onload = removeFeeDocuments();
 
</script>

<div>
    <uc:MessageBox ID="ucMessageBox" runat="server" />
</div>
<div ID="cantDisplayWarning" class="row" runat="server" visible="false">
    This information is private and not available for review.
</div>

<div class="boxPanelFull" style="height: auto; display: inline-block;" runat="server" ID="mainPanel">
    <br />
    <div style="font-weight:bold">
     
        <asp:Literal ID="res_paymentError" runat="server" Text =" <%$ Resources:BrandingResource , paymentError %> " /> 
    </div>
        <asp:RadioButtonList ID="rblINTEND_TO_RECEIVE_MCC" BorderStyle="None" CellPadding="0" CellSpacing="0"
        RepeatDirection="Horizontal" runat="server" RepeatLayout="Table" CssClass="QstRadioList" OnSelectedIndexChanged="rblIntendToReceive_Changed" AutoPostBack="true">
        <asp:ListItem Value="1">Yes</asp:ListItem>
        <asp:ListItem Value="2">No</asp:ListItem>
    </asp:RadioButtonList>

    <asp:Panel ID="pnlWISH_TO_CONTINUE_ACH" runat="server">
        <br />
        <span class="formLabelAuto">Do you wish to cancel this EFT authorization?</span>
        <asp:RadioButtonList ID="rblWISH_TO_CONTINUE_ACH" BorderStyle="None" CellPadding="0" CellSpacing="0"
            RepeatDirection="Horizontal" runat="server" RepeatLayout="Table" CssClass="QstRadioList">
            <asp:ListItem Value="1">Yes</asp:ListItem>
            <asp:ListItem Value="2">No</asp:ListItem>
        </asp:RadioButtonList>
        <br />
        <br />
    </asp:Panel>

    <asp:Panel ID="pnlWaiverPaymentType" runat="server">
        <br />
        <span>Please mark you choice:</span>
        <asp:RadioButtonList ID="rdlWaiverPaymentType" runat="server">
            <asp:ListItem Value="1">Direct Deposit</asp:ListItem>
            <asp:ListItem Value="2">ReliaCard</asp:ListItem>
        </asp:RadioButtonList>
    </asp:Panel>
    <div class="clearfix"></div>

    <div id="divBankingInfo" runat="server" style="display:none">
    <asp:Panel ID="pnlBankingInfo" runat="server">
        <%--DR66 wants this label--%>
        <uc2:Separator ID="Separator4" runat="server" Header="Instructions" />
        <div style="padding-top: 10px;">
            <b>READ INSTRUCTIONS BEFORE COMPLETING </b>
            <ul>
                <li>
                    Electronic Fund Transfer (EFT) enrollment is required for a provider to enroll with the State Medicaid Program.  
                    <br />
                </li>
                
                <li>
                    Medicaid providers must submit this form to receive payment via EFT (Electronic Fund Transfer). It is also the responsibility of the Medicaid provider to ensure this information is updated, as necessary. 
                    <br />
                </li>
                <li>
                    The State Medicaid Program transmits the EFT via the NACHA standard CCD + format.
                    <br />
                </li>
                <li>
                    It is the responsibility of the Provider to contact their financial institution to request the receipt of all data contained within the ACH information field (including the RTN Reassociation Trace Number) of the CCD + Addenda Record. This Trace Number uniquely identifies the transaction set and aids in reassociating payments and remittance advices.
                    <br />
                </li>

            </ul>
        </div>
        
            <asp:CheckBox ID ="chkBankInUS" runat="server" OnCheckedChanged="chkBankInUS_CheckedChanged" AutoPostBack="true" />
            <span style="padding-bottom:10px">Check here if the bank is outside of the United States. Per 1902(a)(80) of the Social Security Act, the State shall not provide any payment to any financial institution or entity located outside the United States.</span>
        <div id="bankInfo" runat="server">
            <br />
        <i>Please enter your banking information below.</i><br />
        <uc2:Separator ID="Separator1" runat="server" Header="Banking and EFT Contact Information" />
        <br />
        <div class="divGrid" style="padding-top: 10px;">
            <asp:GridView runat="server" Width="98%" ID="grdBankingInfo"
                AutoGenerateColumns="False" HorizontalAlign="Left"
                CssClass="gridview" EmptyDataText="No Banking and EFT Contact information found."
                OnRowCommand="grd_RowCommand" OnRowDataBound="grdBankingInfo_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="BANK_NAME" HeaderText="Financial Institution Name" />
                    <asp:BoundField DataField="ACCOUNT_NUMBER" HeaderText="Account Number" Visible="false" />
                    <asp:TemplateField HeaderText="Account Number">
                        <ItemTemplate>
                            <asp:Label ID="lblAccountNumber" runat="server" Text='<%# MaskAccountNumber(Eval("ACCOUNT_NUMBER")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ACH_ACCOUNT_TYPE" HeaderText="Account Type" />

                    <asp:TemplateField HeaderText="Provider Contact Name">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("FIRST_NAME")+ " " + Eval("MIDDLE_NAME") + " " + Eval("LAST_NAME")%>' ></asp:Label>
                            </ItemTemplate>
                    </asp:TemplateField>
                        <asp:BoundField DataField="PHONE_NUMBER" HeaderText="Phone Number" />
                        <asp:BoundField DataField="PHONE_EXTENSION" HeaderText="Ext" />
                        <asp:BoundField DataField="EMAIL_ADDRESS" HeaderText="E-mail Address" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="BankingInfo" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
        <div class="divHistoryAndAdd">
            <asp:ImageButton ID="btnAddBankingInfo" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="BankingInfo" ToolTip="Add" />
            <asp:ImageButton ID="btnHistoryBankingInfo" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="BankingInfo" ToolTip="History" />
        </div>
        
        <div id="divEFTInfo"  runat="server" style="display:none">
            <br /><uc2:Separator ID="Separator3" runat="server" Header="EFT Contact" /><br />
            <div class="divGrid" style="padding-top: 10px">
                <asp:GridView runat="server" Width="98%" ID="grdEftContact" 
                    AutoGenerateColumns="False" HorizontalAlign="Left" 
                    CssClass="gridview" EmptyDataText="No EFT contact found." 
                    OnRowCommand="grd_RowCommand" onrowdatabound="grdEftContact_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Provider Contact Name">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%#Eval("FIRST_NAME")+ " " + Eval("MIDDLE_NAME") + " " + Eval("LAST_NAME")%>' ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PHONE_NUMBER" HeaderText="Phone Number" />
                        <asp:BoundField DataField="PHONE_EXTENSION" HeaderText="Ext" />
                        <asp:BoundField DataField="EMAIL_ADDRESS" HeaderText="E-mail Address" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EftContact" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" 
                                    ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
            </div>
            <div class="divHistoryAndAdd">
                <asp:ImageButton ID="btnAddEftContact" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="EftContact" ToolTip="Add" />
                <asp:ImageButton ID="btnHistoryEftContact" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="EftContact" ToolTip="History" />
            </div>
        </div>

    </div>
    </asp:Panel>
    </div>
    <div id="divWaiverBankingInfo" runat="server">
    <asp:Panel ID="pnlWaiverBankingInfo" runat="server" >
        <uc2:Separator ID="Separator5" runat="server" Header="Instructions" /><br />
            <b>READ INSTRUCTIONS BEFORE COMPLETING </b>
        <div style="background-color:white; padding-top: 10px; width:100%;">
           <ul style="list-style:none">
               <li>The District of Columbia Department of Health and Human Services is hereby authorized to initiate credit entries for deposit of state payments and to initiate, if necessary, debit entries and adjustments for any credit entries in error to my account indicated below and the financial institution named below. I acknowledge that the designation of direct deposit transactions to my account must comply with the provisions of U.S. law.<br /></li>
               
               <li>There are new processing requirements for electronic vendor payments that are being sent to a financial institution outside of the United States. If our payments to you are being forwarded from a U.S. financial institution to a financial institution in another country, please notify Claims Processing, (402) 471-9170. (Section 1902(a) of the Social Security Act and 2011 NACHA Operating Rules & Guidelines, Article Two, SUBSECTION 2.5.8 Specific Provisions for IAT Entries (International ACH Transaction), page OR 13.).<br /></li>
               
               <li>Click(opens new window)&nbsp;<a href ="http://dhhs.dc.gov/medicaid/Documents/LateorMissingEFTResolutionProcedures.pdf" target="_blank" title="http://dhhs.dc.gov/medicaid/Documents/LateorMissingEFTResolutionProcedures.pdf" ><b>here</b></a>&nbsp;for Late/Missing EFT Resolution Procedures.
                    
                </li>
               <li>Go to(opens new window)  <a href="http://dhhs.dc.gov/Pages/fis_claimsprocessing.aspx" target="_blank">http://dhhs.dc.gov/Pages/fis_claimsprocessing.aspx</a>  for information about these payment methods.<br /></li>
           </ul>
        </div>
        <uc2:Separator ID="Separator6" runat="server" Header="ReliaCard Authorization" />
        <br />
        <div class="divGrid" style="padding-top: 10px;">
            <asp:GridView runat="server" Width="98%" ID="grdReliaCard"
                AutoGenerateColumns="False" HorizontalAlign="Left"
                CssClass="gridview" EmptyDataText="No Personal ReliaCard Information found."
                OnRowCommand="grd_RowCommand" OnRowDataBound="grdReliaCard_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="LAST_NAME" HeaderText="Last Name (As appears on account)" />
                    <asp:BoundField DataField="FIRST_NAME" HeaderText="First Name (As appears on account)" />
                    <asp:BoundField DataField="CITY" HeaderText="City" />
                    <asp:BoundField DataField="STATE" HeaderText="State" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="ReliaCard" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
        <div class="divHistoryAndAdd">
            <asp:ImageButton ID="btnAddReliaCard" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="ReliaCard" ToolTip="Add" />
            <asp:ImageButton ID="btnHistoryReliaCard" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="ReliaCard" ToolTip="History" />
        </div>
    </asp:Panel>
    <div id="bankInfoExtra">
    <asp:Panel ID="pnlFeeInfo" runat="server">
          <uc2:Separator ID="ucSeparator5" runat="server" Header="Fee Information" />
            <br />
        <asp:Panel ID="pnlProviderFeeInfo" runat="server">
            <div style="margin-left: 20px; margin-right: 20px;">
            <i>Please indicate if the fee has been submitted.</i><br /><br />
            <asp:CheckBox  runat="server" ID="chkFeeInformation" Text=""  /> 
                I confirm the Provider Application Fee required by provisions of the Affordable Care Act due every three years has been paid in accordance with TennCare Policy PRO 11-001 found at:
                <asp:HyperLink ID="lnkFeeForm" runat="server" NavigateUrl="http://www.tn.gov/tenncare/forms/pro11001.pdf" Text="http://www.tn.gov/tenncare/forms/pro11001.pdf(opens new window)"  Target="_blank" ToolTip="TennCare Policy PRO 11-001 Form"  />.
                <div style="padding-left: 40px; padding-top:10px;">New providers should send payments to: <br /><br />
                HCFA<br />
                310 Great Circle Road<br />
                Nashville, TN 37243<br />
                Attn: Accounting
                </div>
                </div>
        </asp:Panel>

        



        <br />
        <asp:Panel ID="pnlReviewFeeInfo" runat="server" CssClass="NERemove">
            <div class="divGrid" style="padding-top: 10px">
                <asp:GridView ID="grdFeeInformation" runat="server" Width="98%" AllowSorting="false" CssClass="gridview" 
                    EmptyDataText="No fee information found." AutoGenerateColumns="false" HorizontalAlign="Left" OnRowCommand="grd_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="PAYMENT_DATE" HeaderText="Deposit Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False"   />
                        <asp:BoundField DataField="EDISON_NUMBER" HeaderText="Deposit ID"  />
                        <asp:BoundField DataField="PAYMENT_NUMBER" HeaderText="Payment ID"  />
                        <asp:TemplateField ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="FeeInformation" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="REG_ACH_FEE_INFORMATION_ID"  Visible="false"   />
                        <asp:BoundField DataField="CREATE_DATE_TIME"  Visible="false"   />
                        <asp:BoundField DataField="CREATE_USER"  Visible="false"   />
                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
            </div>
            <div class="divHistoryAndAdd">
                <asp:ImageButton ID="btnAddFeeInfo" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="FeeInformation" ToolTip="Add" />
            </div>
            <br />
        </asp:Panel>   
    </asp:Panel>
    <asp:Panel ID="pnlRemit" runat="server" Style="display: none">
        <br />
        <uc2:Separator ID="ucSep2" runat="server" Header="Remittance Information" />
        <br />
        <i>The remittance information will be sent to the address below.
            <br />
            If corrections need to be made to the information below, please return to the Practice Location Billing/Payment section.</i>
        <br />
        <br />
        <div>
            <span class="formLabel300">Billing Contact Name</span>
            <asp:Label ID="lblBillingContactName" runat="server" CssClass="formFieldReadOnly" />
        </div>
        <div>
            <span class="formLabel300">Pay To / Check Payable To Name</span>
            <asp:Label ID="lblPayToName" runat="server" CssClass="formFieldReadOnly" />
        </div>
        <div>
            <span class="formLabel300">Billing Address</span>
            <asp:Label ID="lblAddress" runat="server" CssClass="formFieldReadOnly" />
        </div>
        <div>
            <span class="formLabel300">Address 2</span>
            <asp:Label ID="lblAddress2" runat="server" CssClass="formFieldReadOnly" />
        </div>
        <div>
            <span class="formLabel300">City</span>
            <asp:Label ID="lblCity" runat="server" CssClass="formFieldReadOnly" />
        </div>
        <div>
            <span class="formLabel300">State</span>
            <asp:Label ID="lblState" runat="server" CssClass="formFieldReadOnly" />
        </div>
        <div>
            <span class="formLabel300">Zip</span>
            <asp:Label ID="lblZip" runat="server" CssClass="formFieldReadOnly" />
        </div>
        <div>
            <span class="formLabel300">Ext Zip</span>
            <asp:Label ID="lblExtZip" runat="server" CssClass="formFieldReadOnly" />
        </div>
        <div>
            <span class="formLabel300">Email Address</span>
            <asp:Label ID="lblBPA56" runat="server" CssClass="formFieldReadOnly" />
        </div>
        <div>
            <span class="formLabel300">&nbsp;</span>
            <asp:CheckBox ID="chkConfirmCorrect" runat="server" Text="I confirm this remittance address is correct." />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlVendorInfo" runat="server" CssClass="NERemove">
        <br />
        <uc2:Separator ID="Separator2" runat="server" Header="Vendor Information" />
        <br />
        <div class="divGrid" style="padding-top: 10px">
            <asp:GridView runat="server" Width="98%" ID="grdVendorInfo" AutoGenerateColumns="False" HorizontalAlign="Left"
                CssClass="gridview" EmptyDataText="No vendor information found." OnRowCommand="grd_RowCommand">
                <Columns>
                    <asp:BoundField DataField="VENDOR_NUMBER" HeaderText="Vendor Number" />
                    <asp:BoundField DataField="LOCATION_CODE" HeaderText="Location Code" />
                    <asp:BoundField DataField="SEQUENCE_NUMBER" HeaderText="Sequence Number" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="VendorInfo" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
        <div class="divHistoryAndAdd">
            <asp:ImageButton ID="btnAddVendorInfo" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="VendorInfo" ToolTip="Add" />
            <asp:ImageButton ID="btnHistoryVendorInfo" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="VendorInfo" ToolTip="History" />
        </div>
        <br />
        <br />
    </asp:Panel>
    </div>
</div>
<br />
    </div>
    <div id="divConfirm" style="padding-top:10px; display:none;" runat="server">
<br />
            <uc2:Separator ID="Separator7" runat="server" Header="Confirm" />
            <asp:Panel ID="pnlConfirmAttestation" runat="server">
                <br />   <%-- Remove District of Columbia As per new design --%>
            <span>By selecting the confirmation box below, the submitting individual is attesting and acknowledging on behalf of the Medicaid Provider listed above that: 
            </span>
            <ul>
                <li> He or she is authorized to complete and submit this Enrollment Form. </li>
                <li> The information provided is accurate and true. </li>
            </ul>
            <asp:CheckBox ID="chkConfirm" runat="server" Text="I confirm the information provided is true and accurate." CssClass="confirm"/>
            </asp:Panel>
        </div>

<asp:HiddenField ID="hdnRegAchRequestID" runat="server" />
<asp:HiddenField ID="hdnRegServiceLocationID" runat="server" />


<ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
    CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="ownerModalPopup" align="center" Style="display: none">
    <asp:Panel ID="pnlHeaderMpe" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
        <div align="left">
            &nbsp;&nbsp;
            <asp:Label ID="lblTitle" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px" DefaultButton="btnSave">
        <asp:MultiView ID="mltPopup" runat="server">
            <asp:View ID="vwBankingInfo" runat="server">
                <div style="text-align: left; padding: 15px; margin-left:30px !important" class="container-fluid">
                 <div class="row">
                <uc:BankingInfo ID="ucBankingInfo" runat="server" />
                       </div>
              </div>
            </asp:View>
            <asp:View ID="vwFeeInformation" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                 <div class="row">
                <uc:FeeInformation ID="ucFeeinformation" runat="server" />
                       </div>
              </div>
            </asp:View>
            <asp:View ID="vwEftContact" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                 <div class="row">
                <uc:EftContact ID="ucEftContact" runat="server" />
                       </div>
              </div>
            </asp:View>
            <asp:View ID="vwVendorInfo" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                 <div class="row">
                <uc:VendorInfo ID="ucVendorInfo" runat="server" />
                       </div>
              </div>
            </asp:View>
            <asp:View ID="vwReliaCard" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                 <div class="row">
                <uc:ReliaCard ID="ucReliaCard" runat="server" />
                       </div>
              </div>
            </asp:View>          
            <asp:View ID="vwHistory" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                 <div class="row">
                <uc:History ID="ucHistory" runat="server" />
                       </div>
              </div>
            </asp:View>           
        </asp:MultiView>
    </asp:Panel>
    <div class="row text-center" style="padding-right: 10px;">
         <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox"
                    CausesValidation="false" />
           </div>
    <br />
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />


