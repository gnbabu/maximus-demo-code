<%@ Control Language="C#" AutoEventWireup="true" Inherits="Pages_GroupAndFacilityAffiliationsView" Codebehind="GroupAndFacilityAffiliationsView.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/PendingGroupAffiliations.ascx" TagPrefix="uc" TagName="PendingGroupAffiliations" %>
<%@ Register Src="~/PopupControls/HealthCareAffiliations.ascx" TagPrefix="uc" TagName="HealthCareAffiliations" %>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>

<script type="text/javascript">
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
                event.keyCode = 0
            }
        }
    }

    function numericOnly(obj) {
        obj.value = obj.value.replace(/[^0-9]/g, '');
    }

    $(document).keydown(function (e) {
        // ESCAPE key pressed
        if (e.keyCode == 27) {
            $find("mpecancel").hide();
            var variable = localStorage.getItem("testGF");
            if (variable == "true") {
                $find("mpecancel").hide();
                document.getElementById("<%=btnAdd.ClientID %>").focus();
                return false;
            }
        }
        if (e.keyCode == 27) {
            $find("mpe2cancel").hide();
            var variable = localStorage.getItem("testGF");
            if (variable == "false") {
                $find("mpe2cancel").hide();
                document.getElementById("<%=ImageButton1.ClientID %>").focus();
                return false;
            }
        }
        var target = e.target;
        var shiftPressed = e.shiftKey;
        // If TAB key pressed
        if (e.keyCode == 9) {                 // If inside a Modal dialog (determined by attribute role="dialog")
            if ($(target).parents('[role=dialog]').length) {
                // Find first or last input element in the dialog parent (depending on whether Shift was pressed). 
                // Input elements must be visible, and can be Input/Select/Button/Textarea.
                var borderElem = shiftPressed ?
                    $(target).closest('[role=dialog]').find('input:visible,select:visible,button:visible,textarea:visible').first()
                    :
                    $(target).closest('[role=dialog]').find('input:visible,select:visible,button:visible,textarea:visible').last();
                if ($(borderElem).length) {
                    if ($(target).is($(borderElem))) {
                        return false;
                    } else {
                        return true;
                    }
                }
            }
        }
        return true;
    });

    function load1() {
        localStorage.setItem("testGF", "true");
    }
    function load2() {
        localStorage.setItem("testGF", "false");
    }
</script>
<%-- Note This following script is to handle the what is this? link in group affiliations popup control. Soince it was not triggering document .ready() in the popup control, it is writtern in the containing control --%>
<script type="text/javascript">
    $(document).ready(function () {

        //Update Panel needs after async postback, else event handlers are lost.
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(setupHospitalAffiliationsHelpTexts);

    });

    function setupHospitalAffiliationsHelpTexts() {

        $("#divRequestedEffectivestartDate").hide();
        $(".what-is-this-link-startDate").mouseover(function () {
            $("#divRequestedEffectivestartDate").show();
        });

        $("#divRequestedEffectivestartDate").mouseleave(function () {
            $("#divRequestedEffectivestartDate").hide();
        });

        $("#divRequestedPrimaryFacility").hide();
        $(".what-is-this-link-primaryFacility").mouseover(function () {
            $("#divRequestedPrimaryFacility").show();
        });

        $("#divRequestedPrimaryFacility").mouseleave(function () {
            $("#divRequestedPrimaryFacility").hide();
        });

        $("#divRequestedMedicaidID").hide();
        $(".what-is-this-link-MedicaidID").mouseover(function () {
            $("#divRequestedMedicaidID").show();
        });

        $("#divRequestedMedicaidID").mouseleave(function () {
            $("#divRequestedMedicaidID").hide();
        });

        $("#divHTInpatientSetting").hide();
        $(".what-is-this-link-InpatientSetting").mouseover(function () {
            $("#divHTInpatientSetting").show();
        });

        $("#divHTInpatientSetting").mouseleave(function () {
            $("#divHTInpatientSetting").hide();
        });
    }
</script>
<script>
    function GoToHistory(event, redirectionType) {

        event.preventDefault()
        var urlpath = window.location.href.substring(0, window.location.href.lastIndexOf("/"));
        var regId = $("[id*=hdnRegId]").val();
        var redirectionUrl
        if (redirectionType == 'hospital') {
            redirectionUrl = urlpath + "/HistoryDetails.aspx?regId=" + regId + "&historyType=HospitalAffiliation";
        }
        if (redirectionType == 'group') {
            redirectionUrl = urlpath + "/HistoryDetails.aspx?regId=" + regId + "&historyType=GroupAffiliation";
        }
        if (redirectionType == 'credentialeddelegate') {
            redirectionUrl = urlpath + "/HistoryDetails.aspx?regId=" + regId + "&historyType=CredentialingDelegates"
        }
        window.location.href = redirectionUrl;
    }
</script>

<%-- NOTE: There is a bug in asp.net 4.0 that makes it so ImageButtons fail in IE10 when they are in an
    UpdatePanel. Do not uncomment this UpdatePanel without addressing this bug!  Best replacing ImageButtons
    with LinkButtons containing Images. --%>
<asp:UpdatePanel ID="upAff" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
         <div style ="width:1000px; height:50px" Class="error-message">If you are a provider working as a hospitalist or strictly inpatient only, 
                  Please click add new under hospital affiliations, and designate that you practice exclusively within the inpatient setting
          </div>
        <div>
            <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
            <asp:ValidationSummary ID="valGroupAndFacilityAffiliations" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valGroupAndFacilityAffiliations" />
        </div>
        <h2><span class="pageHeader">Pending Group Affiliations</span></h2>
        <div id="AffiliationInfo">
            <div class="divGrid">
                <div class="grid-hint-md" id="dvGMPHint" runat="server" style="text-align: left">Deleting your affiliation entry in this section will not delete your confirmed group affiliation.</div>
                <asp:GridView runat="server" Width="100%" ID="grdPendingGroupAffiliations" AutoGenerateColumns="False" CssClass="gridViewSmallFont" EmptyDataText="No pending affiliations found."
                    AllowSorting="true" AllowPaging="true" PageSize="10" ShowHeaderWhenEmpty="true" caption="<span style='display:none'> Pending Group Affiliations</span>"
                    DataKeyNames="REG_PENDING_AFFILIATION_ID"
                    OnRowCommand="grdPendingGroupAffiliations_RowCommand" OnPageIndexChanging="grdPendingGroupAffiliations_PageIndexChanging"
                    OnSorting="grdPendingGroupAffiliations_Sorting">
                    <Columns>
                        <asp:BoundField DataField="GroupName" HeaderText="Group Name" SortExpression="GroupName" />
                        <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
                        <asp:BoundField DataField="Medicaid_ID" HeaderText="Medicaid ID" SortExpression="Medicaid_ID" />
                        <asp:BoundField DataField="START_DATE" HeaderText="Start Date" SortExpression="StartDate" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="END_DATE" HeaderText="End Date" SortExpression="EndDate" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:TemplateField HeaderText="Affiliation Status">
                            <ItemTemplate>
                                Pending Approval
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Address">
                            <ItemTemplate>
                                <asp:Literal ID="litAddress" runat="server" Text='<%# FormatAddress(Eval("SERVICING_ADDRESS1"),Eval("SERVICING_ADDRESS2"),Eval("SERVICING_CITY"),Eval("SERVICING_STATE"),Eval("SERVICING_ZIP"),Eval("SERVICING_EXT_ZIP"),Eval("SERVICING_PHONE1")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="PendingGroupAffiliations" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    ToolTip="Edit">
                                    <asp:Image ID="imgEdit" ImageUrl="~/Images/edit.png" runat="server" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="PendingDeleteAffiliation" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    ToolTip="Delete Pending Association">
                                    <asp:Image ID="imgDel" ImageUrl="~/Images/cancel.png" runat="server" BorderStyle="None" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
            </div>
            <%--<i>Page <%=grdGroupAffiliations.PageIndex + 1%> of <%=grdGroupAffiliations.PageCount%></i>--%>
            <div class="divHistoryAndAdd">
                <asp:ImageButton ID="btnAdd" AlternateText="addnew" OnClientClick="load1()" runat="server" ImageUrl="~/Images/add.png" CommandName="PendingGroupAffiliations" OnCommand="btnAdd_Click" ToolTip="Add" />
            </div>
            <div class="btnBox">
            </div>
            <br />
            <div role="dialog" aria-labelledby="h2Group" aria-modal="true" aria-live="assertive">
            <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy" BehaviorID="mpecancel" BackgroundCssClass="modalBackground" />
            <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
                <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                    <div class="popTitle">
                         <h2 id="h2Group"><asp:Label ID="lblTitle" CssClass="bodyTextBold hospitalAffiliationsTitle" runat="server" Text="Title" /></h2>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlMain" runat="server">
                    <asp:MultiView ID="mltPopup" runat="server">
                        <asp:View ID="vwGroupAffiliations" runat="server">
                            <uc:PendingGroupAffiliations ID="ucPendingGroupAffiliations" runat="server" />
                        </asp:View>
                    </asp:MultiView>
                </asp:Panel>
            </asp:Panel>
            <asp:Button runat="server" ID="ButtonDummy" Text="Dummybutton" Style="display: none" />
            <h2><span class="pageHeader">Confirmed Group Affiliations</span></h2>
            <div id="Div1">
                <div class="divGrid">
                    <div class="grid-hint-md" id="Div3" runat="server" style="text-align: left">The grid above shows Groups where you are currently confirmed as a Group member (or have in the past been confirmed as a Group member)</div>
                    <asp:GridView runat="server" Width="100%" ID="grdConfirmedGroupAffiliations" AutoGenerateColumns="False" CssClass="gridViewSmallFont" EmptyDataText="No confirmed affiliations found."
                        AllowSorting="true" AllowPaging="true" PageSize="10" ShowHeaderWhenEmpty="true" caption="<span style='display:none'> Confirmed Group Affiliations</span>"
                        DataKeyNames=""
                        OnRowCommand="grdConfirmedGroupAffiliations_RowCommand" OnPageIndexChanging="grdConfirmedGroupAffiliations_PageIndexChanging" OnSorting="grdConfirmedGroupAffiliations_Sorting">
                        <Columns>
                            <asp:BoundField DataField="GroupName" HeaderText="Group Name" SortExpression="GroupName" />
                            <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
                            <asp:BoundField DataField="MedicaidID" HeaderText="Medicaid ID" SortExpression="MedicaidID" />
                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" SortExpression="StartDate" DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:BoundField DataField="EndDate" HeaderText="End Date" SortExpression="EndDate" DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:BoundField DataField="AffiliationStatus" HeaderText="Affiliation Status" SortExpression="AffiliationStatus" />
                            <asp:TemplateField HeaderText="Address">
                                <ItemTemplate>
                                    <asp:Literal ID="litAddress" runat="server" Text='<%# FormatAddress(Eval("ADDRESS1"),Eval("ADDRESS2"),Eval("CITY"),Eval("STATE"),Eval("ZIP"),Eval("EXT_ZIP"),Eval("PHONE1")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>
            </div>
                </div>
            <div id="hstryNewdiv" style="text-align: right; padding-top: 15px;">
                    <a href="javascript:void(0);" class="buttonBoxFocus hbtn-focus" onclick="GoToHistory(event, 'group');" title="History"><img src="../Images/HistoryIcon1.png" alt="History Icon" class="history-icon" />History</a>
            </div>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <h2><span class="pageHeader">Hospital Affiliations</span></h2>
            <div id="divHospitalAffiliations">
                <div class="divGrid">
                    <asp:GridView runat="server" Width="100%" ID="grdHealthCareAffiliates" AutoGenerateColumns="False" CssClass="gridViewSmallFont" EmptyDataText="No hospital affiliations found."
                        AllowSorting="true" AllowPaging="true" PageSize="10" ShowHeaderWhenEmpty="true" caption="<span style='display:none'> Hospital Affiliations</span>"
                        DataKeyNames="REG_HEALTH_CARE_FACILITY_AFFILIATION_ID"
                        OnRowCommand="grdHealthCareAffiliates_RowCommand" OnPageIndexChanging="grdHealthCareAffiliates_PageIndexChanging" OnRowDataBound="grdHealthCareAffiliates_RowDataBound"
                        OnSorting="grdHealthCareAffiliates_Sorting">
                        <Columns>
                            <asp:BoundField DataField="FacilityName" HeaderText="Facility Name" SortExpression="FacilityName" />
                            <asp:BoundField DataField="StatusofPrivileges" HeaderText="Staff Category" SortExpression="StatusofPrivileges" />
                            <asp:BoundField DataField="StaffCategory" HeaderText="Status of Privileges" SortExpression="StaffCategory" />
                            <asp:BoundField DataField="Is_Primary_Facility" HeaderText="Primary Facility" SortExpression="Is_Primary_Facility" />
                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" SortExpression="StartDate" DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:BoundField DataField="EndDate" HeaderText="End Date" SortExpression="EndDate" DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:TemplateField HeaderText="<span style='display:none'>Edit</span>">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="HealthCareAffiliations" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                        ToolTip="Edit">
                                        <asp:Image ID="imgEdit" ImageUrl="~/Images/edit.png" runat="server" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<span style='display:none'>Delete</span>">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteHEalthCareAffiliation" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                        ToolTip="Delete Pending Association">
                                        <asp:Image ID="imgDel" ImageUrl="~/Images/cancel.png" runat="server" BorderStyle="None" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>
                <%--<i>Page <%=grdGroupAffiliations.PageIndex + 1%> of <%=grdGroupAffiliations.PageCount%></i>--%>
                <div class="divHistoryAndAdd">
                    <asp:ImageButton ID="ImageButton1" AlternateText="addnew" OnClientClick="load2()"  runat="server" ImageUrl="~/Images/add.png" CommandName="HealthCareAffiliations" OnCommand="btnAdd_Click" ToolTip="Add" />
                </div>
                <div id="hstryNewdiv" style="text-align: right; padding-top: 15px;">
                    <asp:HiddenField ID="hdnRegId" runat="server" />
                    <a href="javascript:void(0);" class="buttonBoxFocus hbtn-focus" onclick="GoToHistory(event, 'hospital');" title="History"><img src="../Images/HistoryIcon1.png" alt="History Icon" class="history-icon" />History</a>
                </div>
            </div>
            <br />
            <h2><span class="pageHeader">Delegated Credentialing</span></h2>
            <hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
            <div id="divDelegateCredentialingProvider">
                 <table style="margin-left:inherit">
                     <tr>
                        <td><asp:CheckBox ID="credentialingDelegatesCheckBox" runat="server" OnCheckedChanged="chkVerifiedCredDelegate_CheckedChanged" AutoPostBack="true" Text ="Select this box if you have delegated credentialing that does not display below." Enabled ="false"/></td>
                     </tr>
                     <tr>
                          <td><label>Credentialing delegates are assigned by ODM Credentialing staff.</label></td>
                     </tr>
                 </table>
                <br />
                <asp:Panel ID="pnlDelegateCredentialingProvider" runat="server">
                    <div class="divGrid">
                        <div class="col-sm-7 col-lg-3 text-right">
                            <asp:Label ID="Label2" runat="server" CssClass="formLabel150">Assigned Delegates</asp:Label>
                        </div>
                        <div class="col-sm-5 col-lg-9">
                            <asp:GridView runat="server" ID="grdAssignedDelegates" AutoGenerateColumns="False" caption="<span style='display:none'>Delegated Credentialing</span>"  CssClass="gridViewSmallWidth" EmptyDataText="No delegates."
                                AllowSorting="true" AllowPaging="true" PageSize="4" ShowHeaderWhenEmpty="true" OnPageIndexChanging="grdAssignedDelegates_PageIndexChanging"
                                DataKeyNames="REG_DELEGATE_CREDENTIALING_ID" OnRowDataBound="grdAssignedDelegates_RowDataBound" OnRowCommand="grdAssignedDelegates_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="DELEGATES_NAME" HeaderText="Delegate Name" SortExpression="DELEGATES_NAME" />
                                    <asp:BoundField DataField="MEDICAID_ID" HeaderText="Delegate MED ID" SortExpression="MEDICAID_ID" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDeleteDelegate" runat="server" CommandName="DeleteAssignedDelegate" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                ToolTip="Delete Pending Association">
                                                <asp:Image ID="imgDel" ImageUrl="~/Images/cancel.png" runat="server" BorderStyle="None" />
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="gridViewHeader" />
                                <AlternatingRowStyle CssClass="gridViewAltRow" />
                                <RowStyle CssClass="gridViewRow" />
                                <FooterStyle CssClass="gridViewFooter" />
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>
                <div id="hstryNewdiv" style="text-align: right; padding-top: 15px;">
                    <a href="javascript:void(0);" class="buttonBoxFocus hbtn-focus" onclick="GoToHistory(event, 'credentialeddelegate');" title="History"><img src="../Images/HistoryIcon1.png" alt="History Icon" class="history-icon" />History</a>
                </div>

                <asp:Panel ID="pnlDelegateCredentialingODM" runat="server">
                    <div class="row" id="divDelegateCredentialingODM" runat="server">
                        <div class="col-sm-7 col-lg-4">
                            <asp:Label ID="lblODMCredentialingTxt" runat="server" CssClass="formLabel300">Does Provider participate in Delegated Credentialing?</asp:Label>
                        </div>
                        <div class="col-sm-5 col-lg-8 text-left">
                            <asp:RadioButtonList ID="rblDelegateCredential" runat="server" CssClass="formLabel150 rblYesNo" AutoPostBack="true"
                                OnSelectedIndexChanged="rblDelegateCredential_SelectedIndexChanged" TextAlign="Right" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1">Yes</asp:ListItem>
                                <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="valDelegatedCredentialingRequired" runat="server" ControlToValidate="rblDelegateCredential"
                                ValidationGroup="valGroupAndFacilityAffiliations" Display="Dynamic" Text="*"
                                ErrorMessage="* this is a required field"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row" id="div4" runat="server">
                        <div class="col-sm-4 text-right">
                            <asp:Label ID="Label1" runat="server" CssClass="formLabel150">Assign Delegates</asp:Label>
                        </div>
                        <div class="col-sm-8">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="rcbAssignDelegates" runat="server" CheckBoxes="true" AllowCustomText="true"
                                EnableCheckAllItemsCheckBox="true" Skin="PDMSModern" CssClass="setrcbBackgroundColor" Enabled="false">
                            </telerik:RadComboBox>
                        </div>
                    </div>
                    <div class="row" style="text-align: center">
                        <br />
                        <br />
                        <asp:Button ID="btnCredSave" runat="server" Text="Save" CssClass="buttonBox" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="valGroupAndFacilityAffiliations" />
                        <asp:Button ID="btnCredCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" />
                    </div>
                </asp:Panel>
            </div>
             <div role="dialog" aria-labelledby="h2hospital" aria-modal="true" aria-live="assertive">
            <cc1:ModalPopupExtender ID="mpe2" runat="server" PopupControlID="Panel1" TargetControlID="Button1" BehaviorID="mpe2cancel" BackgroundCssClass="modalBackground" />
            <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
                <asp:Panel ID="Panel2" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                    <div class="popTitle">
                        <h2 id="h2hospital"><asp:Label ID="lblTitle1" CssClass="bodyTextBold hospitalAffiliationsTitle" runat="server" Text="Title" /></h2>
                    </div>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server">
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="View1" runat="server">
                            <uc:HealthCareAffiliations ID="ucHealthCareAffiliations" runat="server" />
                        </asp:View>
                    </asp:MultiView>
                </asp:Panel>
            </asp:Panel>
            <asp:Button runat="server" ID="Button1" Text="Dummybutton" Style="display: none" />
                 </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="hidID" runat="server" />
