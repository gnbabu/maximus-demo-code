<%@ control language="C#" autoeventwireup="true" inherits="UserControls_RegistrationNavigation, App_Web_yvhxe4ml" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageModal" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/ProcessAppeal.ascx" TagName="ProcessAppeal" TagPrefix="uc2" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/UploadDocument.ascx" TagName="UploadDocument" TagPrefix="uc1" %>
<style type="text/css">
    .auto-style2 {
        height: 42px;
    }
</style>
<%--<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>--%>

<script type="text/javascript">
    $(document).ready(function () {

        //Update Panel needs after async postback, else event handlers are lost.
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(setupEventHandlers);


        const chkConfirmSpecialties = $('#<%= chkSpecialityMP.ClientID %>');
        chkConfirmSpecialties.on('click', function () {
            if (this.checked) {
                document.getElementById('<%= btnSpecialityConfirmMP.ClientID %>').disabled = false;

            } else {
                document.getElementById('<%= btnSpecialityConfirmMP.ClientID %>').disabled = true;
            }

        });

        const cancelConfirmSpecialties = $('#<%= btnSpecialityCancelMP.ClientID %>');
        cancelConfirmSpecialties.on('click', function () {
            var chkbox = document.getElementById('<%= chkSpecialityMP.ClientID %>');
            chkbox.checked = false;

        });

    });
    function setupEventHandlers() {
        $(".decision-options input[type='radio']").click(function (evt) {
            var target = $(evt.target);

            var newDate = "";
            if (target.val().toLowerCase() == "approve provider request") {
                newDate = $(".retro-req-date").text();

            }
            else if (target.val().toLowerCase() == "use application submission date") {
                newDate = $(".retro-submit-date").text();
            }

            $(".retro-approved-date").val(newDate);
        })

    }
    function SetPageChanged() {
        var storage = window.sessionStorage;
        storage.setItem("isSpecialityChanged", "True");
        /*  alert("Selected Text: " + selectedText + " Value: " + selectedValue);*/
        return true;
    }

</script>
<div>
    <asp:ValidationSummary ID="vsRegistrationNavigation" runat="server" DisplayMode="List" ValidationGroup="valRegistrationNavigation" />
</div>

<div>
    <asp:MultiView ID="mltNavigation" runat="server" ActiveViewIndex="0">
        <asp:View runat="server" ID="vwProvider">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-4 text-left">
                        <h1> <asp:Label ID="lblTitle" runat="server" Text="Module Title" CssClass="pageHeader" />&nbsp;</h1>
                       
                        <asp:Label ID="lblRegistrationId" runat="server" Text="" CssClass="pageHeader" />
                        <br />
                        <asp:Label ID="lblRequired" runat="server" Text="" CssClass="failureNotification" />
                    </div>
                    <div class="col-sm-4 text-center">
                        <asp:Label ID="lblCurrentStepText" runat="server" Text="" CssClass="pageHeader2" />&nbsp;
                    </div>
                    <div class="col-sm-4 text-right">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClientClick="SetPageChanged();" OnClick="btnSave_Click"
                            ToolTip="Save current screen data" CausesValidation="true" />
                        <asp:Button ID="btnCancelChanges" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancelChanges_Click"
                            ToolTip="Save and move to the next screen" />
                        <asp:Button ID="btnPrevious" runat="server" Text="Previous" CssClass="buttonBox" OnClick="btnPrevious_Click" ToolTip="Save and move to the previous screen" />
                        <asp:Button ID="btnSaveNext" runat="server" Text="Next" CssClass="buttonBox" OnClientClick="SetPageChanged();" OnClick="btnSaveNext_Click"
                            ToolTip="Save and move to next section" CausesValidation="false" />

                    </div>

                </div>

            </div>
        </asp:View>
        <asp:View runat="server" ID="vwProviderServices">
            <div class="container-fluid">
                <div class="row">
                    <%-- <tr><td colspan="3" class="auto-style2">
                                            <asp:panel ID="pnlTakeAction" runat="server"  Visible="true" CssClass="UserHeader"  style="height:20px;">
                                <table width="100%" border="0" cellpadding="0" cellspacing="0" align="right">
                                    <tr>
                                        
                                        <td align="right"><%--add section for generate pdf -
                                            <asp:Button ID="btnTakeActionApprove" runat="server"  CausesValidation="false" Text="Approve" CssClass="buttonBox" OnClick="btnTakeActionApprove_Click" />
                                                                                
                                            <asp:Button ID="btnTakeActionReject" runat="server"  CausesValidation="false" Text="Reject" CssClass="buttonBox" UseSubmitBehavior="false" /></td>
                                    </tr>
                                </table>
                            </asp:panel>
                    </td>
                    </tr>--%>
                    <div class="col-sm-6 text-left">
                        <h1><asp:Label ID="lblTitlePS" runat="server" Text="Module Title" CssClass="pageHeader" /></h1>&nbsp;
                        <asp:Label ID="lblRegistrationIdPS" runat="server" Text="" CssClass="pageHeader" />
                    </div>
                    <div class="col-sm-6 text-right" style="padding-right:5px;">
                        <asp:Button ID="btnTakeActionApprove" runat="server" CausesValidation="false" Text="Approve" CssClass="buttonBoxFocus" OnClick="btnTakeActionApprove_Click" />
                        <asp:Button ID="btnTakeActionReject" runat="server" CausesValidation="false" Text="Reject" CssClass="buttonBox" UseSubmitBehavior="false" />
                        <asp:Button ID="btnSavePS" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click"
                            ToolTip="Save current screen data" />
                        <asp:Button ID="btnTakeAction" runat="server" Text="Take Action" CssClass="buttonBoxFocus" Visible="false"
                            ToolTip="Take action" OnClick="btnTakeAction_Click" />
                        <asp:Button ID="btnPreviousPS" runat="server" Text="Previous" CssClass="buttonBox" OnClick="btnPreviousPS_Click" ToolTip="Save and move to the previous screen" />
                        <asp:Button ID="btnNextPS" runat="server" Text="Next" CssClass="buttonBox" OnClick="btnNextPS_Click" ToolTip="Save and move to the next screen" />
                        <asp:Button ID="btnCancelPS" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancelChanges_Click"
                            ToolTip="Save and move to the next screen" visible ="false" />
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="vwAdministration" EnableViewState="false">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-4 text-left">

                        <asp:Label ID="lblTitleAdmin" runat="server" Text="Administration" CssClass="pageHeader" />&nbsp;
                        <asp:Label ID="lblRegistrationIdAdmin" runat="server" Text="" CssClass="pageHeader" />
                    </div>
                    <div class="col-sm-4 text-center"></div>

                    <div class="col-sm-4 text-right" >
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="vwStateAdministration">
            <div class="container-fluid">
                <div class="row">

                    <div class="col-sm-4 text-left">
                        <asp:Label ID="lblScreeningReviewTitle" runat="server" Text="" CssClass="pageHeader" />&nbsp;
                        <asp:Label ID="lblRegID" runat="server" Text="" CssClass="pageHeader" />
                    </div>
                    <div class="col-sm-4 text-center"></div>
                    <div class="col-sm-4 text-right">
                        <asp:Button ID="btnSATakeAction" runat="server" Text="Take Action" CssClass="buttonBoxFocus" Visible="true"
                            ToolTip="Take action" OnClick="btnTakeAction_Click" />

                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="vwProcessAppealButton">
            <div class="container-fluid">
                <div class="row">

                    <div class="col-sm-6 text-left">
                        <asp:Label ID="lblProcessAppealTitle" runat="server" Text="" CssClass="pageHeader" />&nbsp;
                        <asp:Label ID="lblRegID1" runat="server" Text="" CssClass="pageHeader" />
                    </div>
                    <div class="col-sm-6 text-right">
                        <asp:Button ID="btnProcessAppeal" runat="server" Text="Process Appeal" CssClass="buttonBox" Visible="true"
                            ToolTip="Process Appeal" OnClick="btnProcessAppeal_Click" />
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="vwFinancialReview">
            <div class="container-fluid">
                <div class="row">

                    <div class="col-sm-6 text-left">
                        <asp:Label ID="lblFinancialReviewTitle" runat="server" Text="Financial Review" CssClass="pageHeader" />&nbsp;
                        <asp:Label ID="lblREGIDFinancialReview" runat="server" Text="" CssClass="pageHeader" />
                    </div>
                    <div class="col-sm-6 text-right">
                        <asp:Button ID="btnSaveFinancialReview" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click"
                            ToolTip="Save current screen data" />
                        <asp:Button ID="btnFinancialReviewTakeAction" runat="server" Text="Take Action" CssClass="buttonBoxFocus" Visible="true"
                            ToolTip="Take action" OnClick="btnTakeAction_Click" />
                        <asp:Button ID="btnPreviousFinancialReview" runat="server" Text="Previous" CssClass="buttonBox" OnClick="btnPreviousPS_Click" ToolTip="Save and move to the previous screen" />
                        <asp:Button ID="btnNExtPSFinancialReview" runat="server" Text="Next" CssClass="buttonBox" OnClick="btnNextPS_Click" ToolTip="Save and move to the next screen" />
                    </div>
                </div>
            </div>
        </asp:View>
        <asp:View runat="server" ID="vwApplicationDispositionTakeAction">
            <div class="container-fluid">
                <div class="row">

                    <div class="col-sm-6 text-left">
                        <asp:Label ID="Label1" runat="server" Text="Application Disposition" CssClass="pageHeader" />&nbsp;
                        <asp:Label ID="Label2" runat="server" Text="" CssClass="pageHeader" />
                    </div>
                    <div class="col-sm-6 text-right">
                        <asp:Button ID="btnSaveApplicationDisposition" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click"
                            ToolTip="Save current screen data" />
                        <asp:Button ID="btnApplicationDisposition" runat="server" Text="Take Action" CssClass="buttonBoxFocus" Visible="true"
                            ToolTip="Take action" OnClick="btnTakeAction_Click" />
                        <asp:Button ID="btnPrevApplicationDisposition" runat="server" Text="Previous" CssClass="buttonBox" OnClick="btnPreviousPS_Click" ToolTip="Save and move to the previous screen" />
                        <asp:Button ID="btnNextApplicationDisposition" runat="server" Text="Next" CssClass="buttonBox" OnClick="btnNextPS_Click" ToolTip="Save and move to the next screen" />
                    </div>
                </div>
            </div>
        </asp:View>
          <asp:View runat="server" ID="vwRiskAlertClosure">
            <div class="container-fluid">
                <div class="row">
                </div>
            </div>
        </asp:View>

    </asp:MultiView>
</div>
<div>
    <asp:Panel ID="pnlAppealProcessStatus" runat="server" Visible="false">
        <div class="row">
            <div class="col-sm-3">
                <asp:Label ID="lblAppealBeginDate" CssClass="bodyTextBold" runat="server" Text="Appeal Begin Date:" />
            </div>
            <div class="col-sm-3">
                <asp:Label ID="lblAppealBeginDateValue" CssClass="formData" runat="server" Text="" />
            </div>
            <div class="col-sm-3">
                <asp:Label ID="lblAppealEndDate" CssClass="bodyTextBold" runat="server" Text="Appeal End Date:" />
            </div>
            <div class="col-sm-3">
                <asp:Label ID="lblAppealEndDateValue" CssClass="formData" runat="server" Text="" />
            </div>
            <div class="col-sm-3">
                <asp:Label ID="lblAppealStatus" CssClass="bodyTextBold" runat="server" Text="Appeal Status:" />
            </div>
            <div class="col-sm-3">
                <asp:Label ID="lblAppealStatusValue" CssClass="formData" runat="server" Text="" />
            </div>

        </div>
    </asp:Panel>
</div>
<script language="javascript" type="text/javascript">
    function AnswerIsReject(rblId) {
        if (document.getElementById(rblId) != null) {
            var oElem = document.getElementById(rblId);
            var radio = oElem.getElementsByTagName("input");
            return radio[1].checked;
        }
        return false;
    }

    function TogglePanel(rblId, pnlId) {
        if (AnswerIsReject(rblId)) {
            document.getElementById(pnlId).style.display = "block";
        }
        else {
            document.getElementById(pnlId).style.display = "none";
        }
    }
    function AnswerIsReject1(rblId) {
        if (document.getElementById(rblId) != null) {
            var oElem = document.getElementById(rblId);
            var radio = oElem.getElementsByTagName("input");
            return radio[0].checked || radio[1].checked;
        }
        return false;
    }

    function TogglePanel1(rblId, pnlId, lblID) {
        if (AnswerIsReject1(rblId)) {
            document.getElementById(pnlId).style.display = "block";
            document.getElementById(lblID).style.display = "block";
        }
        else {
            document.getElementById(pnlId).style.display = "none";
            document.getElementById(lblID).style.display = "none";
        }
    }
    function doClick(buttonName, e) {
        // The purpose of this function is to allow the enter key to 
        // point to the correct button to click.
        var key;

        if (window.event) key = window.event.keyCode;   // IE
        else key = e.which;                             // Firefox

        if (key == 13) {
            //Get the button the user wants to have clicked
            var btn = document.getElementById(buttonName);
            if (btn != null) { //If we find the button click it
                btn.click();
                event.keyCode = 0;
            }
        }
    }
    function showPopup(clientID) {
        var bhmpeID = clientID;
        var mPopup = $find(bhmpeID);
        if (mPopup) mPopup.show();
        $find(bhmpeID).show();
        return false;
    }

    
    function showTakeActionPopup(clientID, titleId, SaveId) {
        var bhmpeID = clientID;
        $('[id=' + titleId + ']')[0].innerHTML = "Take Action - Application Disposition";
        var mPopup = $find(bhmpeID);
        if (mPopup) mPopup.show();
        return false;
    }

</script>

<!-- ModalPopupExtender -->
<cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
    CancelControlID="btnCancelmpe" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlHeader">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="ownerModalPopup" align="center" Style="display: none; height: auto;">
    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
        <div class="popTitle">
            <asp:Label ID="lblMpeTitle" runat="server" Text="Take Action" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlLabel" runat="server">
        <div style="text-align: left; padding: 15px">
            <asp:MultiView ID="mltTakeAction" runat="server" ActiveViewIndex="0">
                <asp:View ID="vwPSReview" runat="server">
                    <div class="container-fluid">
                        <div class="row">
                            <asp:Label Visible="false" ID="valAlert" Text="" runat="server" ForeColor="OrangeRed"></asp:Label>
                        </div>
                        <div class="row">
                            <asp:ValidationSummary ID="vsTakeAction" runat="server" DisplayMode="List" ValidationGroup="valTakeAction" />
                        </div>
                        <div class="row">
                            <div class="col-sm-12 text-center">
                                <asp:RadioButtonList ID="rblPSReview" BorderStyle="None" CellPadding="0" CellSpacing="0"
                                    RepeatDirection="Horizontal" runat="server" RepeatLayout="Table" CssClass="QstRadioList" Style="display: none;float:none;">
                                    <asp:ListItem Value="1">Approve</asp:ListItem>
                                    <asp:ListItem Value="2" Selected="True">Return to Provider</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                                <asp:Panel ID="pnlPSReview" runat="server" class="row">
                                    <div class="col-sm-12" id="trReturnReason" runat="server">
                                        <div class="row">
                                            <div class="col-sm-3 text-right" style="padding-right: 0px;"><span>Return Reason</span></div>
                                            <div class="col-sm-9 text-left">
                                                <asp:Panel ID="Panel1" ScrollBars="Vertical" Height="200" runat="server" CssClass="listFullWidth">
                                                    <asp:ListView ID="lsvReturnReasons" runat="server">
                                                        <LayoutTemplate>
                                                            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <div style="display: table-row;">
                                                                <asp:CheckBox ID="chkReason" runat="server" Text=" " Style="margin-left: 5px;" /><asp:Label ID="lblReason" runat="server" aria-label="Reasons" Text='<%# DataBinder.Eval(Container.DataItem, "ERROR_NAME") %>' />
                                                                <asp:HiddenField ID="hdnErrorTypeID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ERROR_TYPE_ID") %>' />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:ListView>
                                                </asp:Panel>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            <div class="row">
                                <div class="col-sm-3 text-right" style="padding-right: 0px;"><span>Notes to Provider</span></div>
                                <div class="col-sm-9 text-left">
                                    <asp:TextBox ID="txtComments" runat="server" aria-label="Notes to Provider" MaxLength="1000" CssClass="modalFormField" TextMode="MultiLine" Columns="100" Rows="7" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right" style="padding-right: 0px;"><span>Internal Comments</span></div>
                                <div class="col-sm-9 text-left">
                                    <asp:TextBox ID="txtInternalComments" aria-label="Internal Comments" runat="server" MaxLength="1000" CssClass="modalFormField" TextMode="MultiLine" Columns="100" Rows="7" />
                                </div>
                            </div>
                            <div class="row" id="divChkBCIText" runat="server" visible ="false">
                                <div class="col-sm-3 text-right" style="padding-right: 0px;"></div>
                                <div class="col-sm-9 text-left" style="padding-right: 0px;">
                                    <asp:CheckBox ID="chkBCIText" runat="server" Text="Click here to add BCI information to notice" />
                                </div>
                            </div>
                        </div>
                </asp:View>
                <asp:View ID="vwSignature" runat="server">
                    <div style="text-align: center">
                        <asp:ValidationSummary ID="vsContractSignature" runat="server" DisplayMode="List" ValidationGroup="valSignature" />
                        <div id="pnlAdminInfo" class="center wd400" runat="server">
                            <div id="pnlAdminName" class="row center wd400" runat="server">
                                <div class="col-sm-12">
                                    <div class="row">
                                        <p>Please enter the name of your Administrator and the contract dates provided to you in your registration notification.</p>
                                    </div>
                                    <div class="col-sm-3 text-right"><span class="formLabel formLabel170">Administrator Name*</span></div>
                                    <div class="col-sm-9 wd200 formEntry">
                                        <asp:TextBox ID="txtAdministrator" runat="server" MaxLength="100" CssClass="formField"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="valAdminReqd" ControlToValidate="txtAdministrator" ErrorMessage="* Administrator is required." Text="*"
                                            Display="Dynamic" SetFocusOnError="true" ValidationGroup="valSignature" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel formLabel170">Contract Begin Date*</span></div>
                                <div class="col-sm-9 wd200 formEntry">
                                    <asp:TextBox ID="txtContractStartDate" runat="server" CssClass="formField" /><cc1:CalendarExtender ID="calContractStart"
                                        TargetControlID="txtContractStartDate" runat="server" />
                                    <asp:RequiredFieldValidator runat="server" ID="valContractStartReqd" ControlToValidate="txtContractStartDate" ErrorMessage="* Contract Start Date is required." Text="*"
                                        Display="Dynamic" SetFocusOnError="true" ValidationGroup="valSignature" />
                                    <asp:RegularExpressionValidator ID="vaContractStartFormat" runat="server" ControlToValidate="txtContractStartDate" Text="*" Width="2"
                                        ValidationGroup="valSignature" Display="Dynamic"
                                        ErrorMessage="* Select/Enter a valid Contract Start Date (mm/dd/yyyy)" SetFocusOnError="true" ValidationExpression="^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel formLabel170">Contract End Date*</span></div>
                                <div class="col-sm-9 wd200 formEntry">
                                    <asp:TextBox ID="txtContractEndDate" runat="server" CssClass="formField" /><cc1:CalendarExtender ID="calContractEnd"
                                        TargetControlID="txtContractEndDate" runat="server" />
                                    <asp:RequiredFieldValidator runat="server" ID="valContractEndReqd" ControlToValidate="txtContractEndDate" ErrorMessage="* Contract End Date is required."
                                        Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valSignature" />
                                    <asp:RegularExpressionValidator ID="valContractEndFormat" runat="server" ControlToValidate="txtContractEndDate" Text="*" Width="2"
                                        ValidationGroup="valSignature" Display="Dynamic"
                                        ErrorMessage="* Select/Enter a valid Contract End Date (mm/dd/yyyy)" SetFocusOnError="true" ValidationExpression="^(0[1-9]|1[012])[- /.](0[1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d$">
                                    </asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <asp:CheckBox ID="chkSignature" runat="server" Text="Sign Contract" ValidationGroup="valSignature" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwScreeningComplete" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <asp:ValidationSummary ID="valScreeningComplete" runat="server" DisplayMode="List" ValidationGroup="valScreeningComplete" />
                        </div>
                        <div class="row text-center">
                            <asp:CheckBox ID="chkScreeningComplete" runat="server" Text="Screening Complete" /><br />
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel formLabelSmall verticalAlignTop">Comments</span></div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtScreeningComments" aria-Label="Screening Comments" runat="server" MaxLength="1000" CssClass="formFieldLarge" TextMode="MultiLine" Columns="2000" Rows="7" /> 
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwScreeningReview" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <asp:ValidationSummary ID="vsStateAdminReview" runat="server" DisplayMode="List" ValidationGroup="valAppealProcess" />
                        </div>
                        <div class="row">
                            <div class="col-sm-12 text-center">
                            <asp:RadioButtonList ID="rblScreeningReview" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" CssClass="QstRadioList" Style="float:none;">
                                <asp:ListItem Value="1">Confirm</asp:ListItem>
                                <asp:ListItem Value="2">Override Screening</asp:ListItem>
                            </asp:RadioButtonList>
                                </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 verticalAlignTop text-right">
                                <asp:Label ID="lblAppealProcessComments" runat="server" Text="Comments" CssClass="formLabelSmall verticalAlignTop" />

                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtAppealProcessComments" runat="server" MaxLength="1000" CssClass="formFieldLarge" TextMode="MultiLine" Columns="2000" Rows="7" />
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwRetroEffectiveDateReview" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ValidationGroup="valRetro" />
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel wd200">Requested Effective Date </span></div>
                            <div class="col-sm-9">
                                <asp:Label ID="lblRetroRequestedEffectiveDate" runat="server" CssClass="formFieldDisplay retro-req-date" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel wd200">Application Submission Date</span></div>
                            <div class="col-sm-9">
                                <asp:Label ID="lblRetroSubmitDate" runat="server" CssClass="formFieldDisplay retro-submit-date" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel wd200">Decision*</span></div>
                            <div class="col-sm-9">
                                <asp:RadioButtonList ID="rblRetroReviewDecision" runat="server" CssClass="decision-options" RepeatDirection="Vertical" />
                                <asp:CompareValidator runat="server" ID="valDecisionCmp" ControlToValidate="rblRetroReviewDecision" ValueToCompare="" Type="String" ErrorMessage="* A Decision is required."
                                    Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="valTakeAction" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel wd200">Approved Effective Date*</span></div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtRetroApprovedEffectiveDate" runat="server" CssClass="formField retro-approved-date" MaxLength="10" />
                                <cc1:CalendarExtender ID="ceApprovedEfectiveDate" TargetControlID="txtRetroApprovedEffectiveDate" runat="server" />
                                <asp:RequiredFieldValidator ID="valApprovedEfectiveReqd" runat="server" SetFocusOnError="true" Text="*" ValidationGroup="valTakeAction"
                                    ControlToValidate="txtRetroApprovedEffectiveDate" ErrorMessage="* Approved Effective Date is required ." Display="Dynamic" Enabled="true" />
                                <asp:CompareValidator ID="valApprovedEfectiveDateFormat" runat="server" ValidationGroup="valTakeAction"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtRetroApprovedEffectiveDate" Enabled="true"
                                    ErrorMessage="* A valid Approved Effective Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel wd200">Comments*></span></div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtRetroComments" runat="server" Rows="7" CssClass="formField wd250" TextMode="MultiLine" MaxLength="4000" />
                                <asp:RequiredFieldValidator ID="valCommentReqd" runat="server" SetFocusOnError="true" Text="*" ValidationGroup="valTakeAction"
                                    ControlToValidate="txtRetroComments" ErrorMessage="* Comments are required." Display="Dynamic" Enabled="true" />
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwMemberRetroEffectiveDateReview" runat="server">
                    <asp:UpdatePanel ID="upMemberRetro" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div>
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ValidationGroup="valGroupReviewRetro" />
                            </div>
                            <asp:GridView runat="server" Width="98%" ID="grdMemberRetroEffectiveDateReview" DataKeyNames="REG_AFFILIATION_ID,SUBMIT_DATE_TIME,START_DATE"
                                AutoGenerateColumns="False" HorizontalAlign="Left"
                                CssClass="gridview" EmptyDataText="No Members found."
                                OnRowCommand="grdMemberRetroEffectiveDateReview_RowCommand" OnRowDataBound="grdMemberRetroEffectiveDateReview_RowDataBound" OnRowEditing="grdMemberRetroEffectiveDateReview_RowEditing">
                                <Columns>
                                    <asp:BoundField DataField="NAME" HeaderText="Provider Name" />
                                    <asp:BoundField DataField="NPI" HeaderText="NPI" />
                                    <asp:BoundField DataField="START_DATE" HeaderText="Start Date" />

                                    <asp:TemplateField HeaderText="Approved Effective Date">
                                        <ItemTemplate>
                                            <cc1:CalendarExtender
                                                ID="calFromDate"
                                                runat="server"
                                                Format="MM/dd/yyyy"
                                                TargetControlID="txtApprovedEffectiveDate"
                                                PopupPosition="BottomRight"
                                                CssClass="QstCalendarCSS"
                                                PopupButtonID="imgFromDate"
                                                EnabledOnClient="true" />
                                            <asp:TextBox ID="txtApprovedEffectiveDate" runat="server" Text="" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Decision">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlDecision" runat="server" OnSelectedIndexChanged="ddlDecision_SelectedIndexChanged" CausesValidation="true" AutoPostBack="true" AppendDataBoundItems="True">
                                                <asp:ListItem Text="" Value="" />
                                                <asp:ListItem Text="Approve Provider Request" Value="ApproveProviderRequest" />
                                                <asp:ListItem Text="Use Application Submission Date" Value="UseSubmissionDate" />
                                                <asp:ListItem Text="Override Effective Date" Value="OverrideEffectiveDate" />
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comments">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtComments" runat="server" Text="" TextMode="MultiLine" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                                <AlternatingRowStyle CssClass="gridViewAltRow" />
                                <RowStyle CssClass="gridViewRow" />
                                <FooterStyle CssClass="gridViewFooter" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:View>
                <asp:View ID="vwFinancialReviewComplete" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" ValidationGroup="valFinancialReviewComplete" />
                        </div>
                        <div class="row">
                            <asp:CheckBox ID="chkFinancialReviewComplete" runat="server" Text=" Financial Review Complete" />
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel formLabelSmall verticalAlignTop">Comments</span></div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtFinancialReviewComplete" runat="server" MaxLength="1000" CssClass="formFieldLarge" TextMode="MultiLine" Columns="2000" Rows="7" />
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwApplicationDisposition" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <asp:ValidationSummary ID="vsApplicationDisposition" runat="server" DisplayMode="List" ValidationGroup="valApplicationDisposition" />
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                            <asp:RadioButtonList ID="rblApproveOrDeny" runat="server">
                                <asp:ListItem Text="Approve Application" Value="ApproveApplication"></asp:ListItem>
                                <asp:ListItem Text="Deny Application" Value="DenyApplication"></asp:ListItem>
                            </asp:RadioButtonList>
                                </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel formLabelSmall verticalAlignTop">Comments</span></div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtApplicationDispositionComments" runat="server" MaxLength="1000" CssClass="formFieldLarge" TextMode="MultiLine" Columns="2000" Rows="7" />
                            </div>



                            <br />
                        </div>
                        <div class="row">
                            <uc1:Separator ID="ucSepUploadAppDisposition" runat="server" Header="Uploaded Documents" Mode="1" Visible="false" />
                            <br />

                            <uc1:UploadDocument ID="ucUploadDocumentDBH" runat="server" CssClassUploadButton="buttonBox" DocumentSection="DBH Review"
                                ValidFileExtensions="doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt" Visible="false" />
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwDHCFRecommendation" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <asp:ValidationSummary ID="vsDHCFRecommendation" runat="server" DisplayMode="List" ValidationGroup="valDHCFRecommendation" />
                        </div>
                        <div class="row">
                            <div class="col-sm-12 text-center">
                            <asp:RadioButtonList ID="rblDHCFRecommendation" runat="server" CssClass="QstRadioList" style="float:none;">
                                <asp:ListItem Text="Reviewed and Recommend Approval" Value="ApproveApplication"></asp:ListItem>
                                <asp:ListItem Text="Reviewed and Recommend Denial" Value="DenyApplication"></asp:ListItem>
                            </asp:RadioButtonList>
                                </div>

                            <br />
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel formLabelSmall verticalAlignTop">Comments</span></div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtDHCFRecommendation" runat="server" MaxLength="1000" CssClass="formFieldLarge" TextMode="MultiLine" Columns="2000" Rows="7" />
                            </div>
                             <br />
                        </div>
                        <div class="row">
                        <uc1:Separator ID="ucSep1" runat="server" Header="Uploaded Documents" />
                        <br />

                        <uc1:UploadDocument ID="ucUploadDocumentDHCF" runat="server" CssClassUploadButton="buttonBox" DocumentSection="DHCF Review"
                            ValidFileExtensions="doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt" Visible="false" />
                            </div>
                    </div>

                </asp:View>
                <asp:View ID="vwDHCFApplicationFeeRecommendation" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <asp:ValidationSummary ID="vsDHCFApplicationFeeRecommendation" runat="server" DisplayMode="List" ValidationGroup="valDHCFApplicationFeeRecommendation" />
                        </div>
                         <div class="row">
                             <div class="col-sm-12 text-center">
                        <asp:RadioButtonList ID="rblDHCFApplicationFeeRecommendation" runat="server" CssClass="QstRadioList">
                            <asp:ListItem Text="Reviewed and Recommend Hardship Approval" Value="ApproveApplication"></asp:ListItem>
                            <asp:ListItem Text="Reviewed and Recommend Hardship Denial" Value="DenyApplication"></asp:ListItem>
                        </asp:RadioButtonList>
                                 </div>
                             </div>
                         <div class="row">
                                <div class="col-sm-3 text-right"><span class="formLabel formLabelSmall verticalAlignTop">Comments</span></div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtDHCFApplicationFeeRecommendation" runat="server" MaxLength="1000" CssClass="formFieldLarge" TextMode="MultiLine" Columns="2000" Rows="7" /></div>
                           </div>
                    </div>
                </asp:View>
                <asp:View ID="vwStateApplicationFeeRecommendation" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <asp:ValidationSummary ID="vsStateApplicationFeeRecommendation" runat="server" DisplayMode="List" ValidationGroup="valStateApplicationFeeRecommendation" />
                        </div>
                        <div class="row">
                            <div class="col-sm-12 text-center">
                        <asp:RadioButtonList ID="rblStateApplicationFeeRecommendation" runat="server">
                            <asp:ListItem Text="Approve" Value="ApproveApplication"></asp:ListItem>
                            <asp:ListItem Text="Deny" Value="DenyApplication"></asp:ListItem>
                        </asp:RadioButtonList>
                                </div>
                            </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel formLabelSmall verticalAlignTop">Comments</span></div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtStateApplicationFeeRecommendation" runat="server" MaxLength="1000" CssClass="formFieldLarge" TextMode="MultiLine" Columns="2000" Rows="7" /></td>
                           </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwDBHRecommendation" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <asp:ValidationSummary ID="vsDBHRecommendation" runat="server" DisplayMode="List" ValidationGroup="valDBHRecommendation" />
                        </div>
                         <div class="row">
                             <div class="col-sm-12 text-center">
                        <asp:RadioButtonList ID="rblDBHRecommendation" runat="server">
                            <asp:ListItem Text="Reviewed and Recommend Approval" Value="ApproveApplication"></asp:ListItem>
                            <asp:ListItem Text="Reviewed and Recommend Denial" Value="DenyApplication"></asp:ListItem>
                        </asp:RadioButtonList>
                                 </div>
                         </div>
                       <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel formLabelSmall verticalAlignTop">Comments</span></div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtDBHRecommendation" runat="server" MaxLength="1000" CssClass="formFieldLarge" TextMode="MultiLine" Columns="2000" Rows="7" /></td>
                          </div>
                        </div>
                   <div class="row">
                    <br />
                    <uc1:Separator ID="ucSepDBHReview" runat="server" Header="Uploaded Documents"  />
                    <br />

                    <uc1:UploadDocument ID="ucUploadDocumentDBHReview" runat="server" CssClassUploadButton="buttonBox" DocumentSection="DBH Review"
                        ValidFileExtensions="doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt" Visible="false" />
                         </div>
                        </div>
                </asp:View>
                <asp:View ID="vwOperatorTerminate" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <asp:ValidationSummary ID="vsOperatorTermintate" runat="server" DisplayMode="List" ValidationGroup="valOperatorTerminate" />
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                    <asp:Label ID="lblEffectiveDate" runat="server" class="formLabel wd200">Effective Date</asp:Label>
                            </div>
                             <div class="col-sm-9">
                                    <asp:TextBox ID="txtEffectiveDate" runat="server" MaxLength="10" CssClass="formFieldDisplay" />
                            </div>
                        </div>
                         <div class="row">
                            <div class="col-sm-3 text-right">
                                    <asp:Label ID="lblEnrollmentStatus" runat="server" class="formLabel wd200">Enrollment Status</asp:Label> 
                            </div>
                             <div class="col-sm-9">
                                    <asp:TextBox ID="txtEnrollmentStatus" runat="server" MaxLength="20" CssClass="formFieldDisplay" /> 
                           </div>
                        </div>
                         <div class="row">
                            <div class="col-sm-3 text-right">
                                    <asp:Label ID="lblReenrollmentDueDate" runat="server" class="formLabel wd200">Re-Enrollment Due Date</asp:Label> 
                            </div>
                             <div class="col-sm-9">
                                    <asp:TextBox ID="txtReenrollmentDueDate" runat="server" MaxLength="20" CssClass="formFieldDisplay" />
                                  </div>
                        </div>
                            <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel wd200">Termination Effective Date*</span> 
                            </div>
                             <div class="col-sm-9">
                                    <asp:TextBox ID="txtTermDate" runat="server" CssClass="formField" MaxLength="10" />
                                    <cc1:CalendarExtender ID="ceTermDate" TargetControlID="txtTermDate" runat="server" />
                                    <%--                                    <asp:RequiredFieldValidator ID="valTermDateReqd" runat="server" SetFocusOnError="true" ValidationGroup="valOperatorTerminate" Text="*"
                                        ControlToValidate="txtTermDate" ErrorMessage="* Termination Effective Date is required." Display="Dynamic" Enabled="true" />
                                    <asp:CompareValidator ID="cvTermDate" runat="server" ValidationGroup="valOperatorTerminate"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtTermDate" Enabled="true"
                                        ErrorMessage="* A valid Termination Effective Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="true"> 
                                    </asp:CompareValidator>--%>
                                </div>
                        </div>
                          <div class="row">
                            <div class="col-sm-3 text-right"> <span class="formLabel wd200">New Enrollment Status*</span>
                              </div>
                             <div class="col-sm-9">
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="formField" MaxLength="10" />
                                    <%--                                    <asp:RequiredFieldValidator ID="valStatusReqd" runat="server" SetFocusOnError="true" ValidationGroup="valOperatorTerminate" Text="*"
                                        ControlToValidate="ddlStatus" ErrorMessage="* New Enrollment Status is required." Display="Dynamic" Enabled="true" InitialValue="" />--%>
                               </div>
                            </div>
                           <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel wd200">Comments*</span>
                              </div>
                             <div class="col-sm-9">
                                    <asp:TextBox ID="txtTermComments" runat="server" Rows="7" CssClass="formField wd250" TextMode="MultiLine" MaxLength="4000" />
                                    <%--                                    <asp:RequiredFieldValidator ID="valTermCommentsReqd" runat="server" SetFocusOnError="true" ValidationGroup="valOperatorTerminate" Text="*"
                                        ControlToValidate="txtTermComments" ErrorMessage="* Comments are required." Display="Dynamic" Enabled="true" EnableClientScript="true" />--%>
                             </div>
                               </div>
                   <div class="row">
                    <br />
                    <uc1:Separator ID="Separator1" runat="server" Header="Uploaded Documents" />
                    <br />

                    <uc1:UploadDocument ID="UploadDocument1" runat="server" CssClassUploadButton="buttonBox" DocumentSection="DBH Review"
                        ValidFileExtensions="doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt" Visible="false" />
                       </div>      
                    </div>
                </asp:View>
                <asp:View ID="vwDDSRecommendation" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <asp:ValidationSummary ID="vsDDSRecommendation" runat="server" DisplayMode="List" ValidationGroup="valDDSRecommendation" />
                        </div>
                          <div class="row">
                              <div class="col-sm-12 text-center">
                        <asp:RadioButtonList ID="rblDDSRecommendation" runat="server">
                            <asp:ListItem Text="Reviewed and Recommend Approval" Value="ApproveApplication"></asp:ListItem>
                            <asp:ListItem Text="Reviewed and Recommend Denial" Value="DenyApplication"></asp:ListItem>
                        </asp:RadioButtonList>
                                  </div>
                   </div>
                       <div class="row">
                                <div  class="col-sm-3 text-right"><span class="formLabel formLabelSmall verticalAlignTop">Comments</span></div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtDDSRecommendation" runat="server" MaxLength="1000" CssClass="formFieldLarge" TextMode="MultiLine" Columns="2000" Rows="7" /></div>
                         </div>
                   <div class="row">
                    <br />
                    <uc1:Separator ID="ucSepDDSReview" runat="server" Header="Uploaded Documents" />
                    <br />

                    <uc1:UploadDocument ID="ucUploadDocumentDDSReview" runat="server" CssClassUploadButton="buttonBox" DocumentSection="DDS Review"
                        ValidFileExtensions="doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt" Visible="false" />
                          </div>
                        </div>
                </asp:View>
                <asp:View ID="vwCredScreeningComplete" runat="server">
                    <div style="text-align: left; padding: 15px" class="container-fluid">
                        <div class="row">
                            <asp:ValidationSummary ID="ValidationSummary4" runat="server" DisplayMode="List" ValidationGroup="valCredScreeningComplete" />
                        </div>
                        <div class="row">
                           <div class="col-sm-3 text-right"><span class="formLabel formLabelSmall verticalAlignTop">Data Rank</span></div>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddlcredRiskLevel" runat="server" CssClass="formField300" AutoPostBack="false" >
                                    <asp:ListItem Text="" Value="0" />
                                    <asp:ListItem Text="Low" Value="1" />
                                    <asp:ListItem Text="Medium" Value="2" />
                                    <asp:ListItem Text ="High" Value="3" />
                                </asp:DropDownList>
                            </div> 
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="formLabel formLabelSmall verticalAlignTop">Verified By</span></div>
                            <div class="col-sm-9">
                                <asp:Label ID="lblVerifiedby" runat="server" /> 
                            </div>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="vwRiskAlertClosureComplete" runat="server">
                </asp:View>
            </asp:MultiView>
        </div>
    </asp:Panel>
    <div class="row btnBox" style="padding-right: 10px;">
        <asp:Button ID="btnSavempe" runat="server" Text="Save" CssClass="buttonBoxFocus" CausesValidation="true" ValidationGroup="valTakeAction"
            OnClick="btnSavempe_Click" />
        <asp:Button ID="btnCancelmpe" runat="server" Text="Cancel" CssClass="buttonBox" CausesValidation="false" />
    </div>
    <br />
</asp:Panel>
<cc1:ModalPopupExtender ID="mpe1" runat="server" PopupControlID="pnlModal1" TargetControlID="ButtonDummy"
    BackgroundCssClass="navigationModalBackground" PopupDragHandleControlID="pnlHeader1" CancelControlID="btnCancelmpe1">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlModal1" runat="server" CssClass="navigationModalPopup" align="center" Style="display: none">
    <asp:Panel ID="pnlHeader1" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
        <div class="row">
            <div class="col-sm-12 text-left">
            &nbsp;&nbsp;
            <asp:Label ID="lblMpeTitle1" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
                </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px" DefaultButton="btnSave">
        <asp:MultiView ID="mltPopup" runat="server">
            <asp:View ID="vwProcessAppeal" runat="server">
                <div class="row">
                <uc2:ProcessAppeal ID="ucProcessAppeal" runat="server" />
                    </div>
            </asp:View>
        </asp:MultiView>
       <div class="row btnBox" >
                    <asp:Button ID="btnSaveProcessAppeal" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSaveProcessAppeal_Click"
                        CausesValidation="true" />
               
                    <asp:Button ID="btnCancelmpe1" runat="server" Text="Cancel" CssClass="buttonBox"
                        CausesValidation="false" />
             </div>
    </asp:Panel>

    <br />
</asp:Panel>



<cc1:ModalPopupExtender ID="specialityConfirmPopup" runat="server" PopupControlID="pnlspecialityConfirmMP" TargetControlID="ButtonDummy"
    BackgroundCssClass="navigationModalBackground" PopupDragHandleControlID="pnlspecialityConfirmMPHeader" CancelControlID="btnSpecialityCancelMP">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlspecialityConfirmMP" runat="server" CssClass="navigationModalPopup" align="center" Style="display: none; min-width:450px!important;">
    <asp:Panel ID="pnlspecialityConfirmMPHeader" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
        <div class="row">
            <div class="col-sm-12 text-left">
            &nbsp;&nbsp;
            <asp:Label ID="Label3" CssClass="bodyTextBold" runat="server" Text="Specialties Confirm" ForeColor="White" />
                </div>
        </div>
        </asp:Panel>

        <asp:Panel ID="Panel2"  runat="server" Style="margin-right: 10px">
                <div style="text-align: left; padding: 15px; margin-left: 30px !important" class="container-fluid">
                    <div style="text-align:center">
                        <p> By checking the box, you are confirming that</p>
                        <p>you have selected the correct Specialties and </p>
                        <p>Specialty Effective dates for the provider.</p>
                    </div>
                    <div style="text-align:center;align-content:center;align-items:center;">
                        <br /> 
                        <label>Specialties Confirmed</label> 
                        <asp:CheckBox ID="chkSpecialityMP"  runat="server" style="color:black" ToolTip="Specialties Confirmed"   />
                    </div>
                </div>
        </asp:Panel>
          <div class="row btnBox" style="text-align:center;align-content:center;align-items:center;" >
               
            <asp:Button ID="btnSpecialityCancelMP" runat="server" Text="Cancel" CssClass="buttonBox" 
                CausesValidation="false" />
            <asp:Button ID="btnSpecialityConfirmMP" runat="server" Text="Confirm" Enabled="false" CssClass="buttonBoxFocus" OnClick="btnSpecialityConfirmMP_Click"
                CausesValidation="true" />
    <br />
         </div>
    <br />
    </asp:Panel>


<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />

<uc1:MessageModal ID="ucMessageModal" runat="server" />
<uc1:MessageBox ID="MessageBox2" runat="server" />


<div id="SectionHeading" class="row SectionHeading" runat="server">
    <div id="SectionHeader" class="SectionHeader" runat="server">
    </div>

    <div id="SectionHeaderSub" class="SectionHeaderSub" runat="server">
    </div>

    <br />
    <div id="SectionBody" class="SectionBody" runat="server"></div>


</div>
