<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_PracticePartnership" Codebehind="PracticePartnership.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/CPCGroupMember.ascx" TagPrefix="ucCPC" TagName="CPCGroupMember" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>


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
    function removeDisabled() {
        $("#<%= btnCloseHistory.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    }
</script>
<div onmouseover="removeDisabled();">
<asp:panel id="upHistory" runat="server" style="min-width: 1400px; position: fixed; z-index: 2; left: 200px; top: 50px;">
    <cc1:modalpopupextender id="mpeHistory" runat="server" popupcontrolid="pHistory" targetcontrolid="ButtonDummy3"
        backgroundcssclass="modalBackground" popupdraghandlecontrolid="pHistory" CancelControlID="btnCloseHistory">
    </cc1:modalpopupextender>
    <asp:panel id="pHistory" runat="server" cssclass="modalPopup" style="padding: 20px; min-width: 1400px;">
        <div>
             <asp:panel id="pnlHistoryDetails" runat="server">
                <div>
                    <asp:GridView runat="server" Width="98%" ID="grd" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found."
                        AllowPaging="true" AllowSorting="True" PageSize="10"
                        OnPageIndexChanging="grd_PageIndexChanging" OnSorting="grd_Sorting">
                        <Columns>
                            <asp:BoundField DataField="OPERATION"           HeaderText="Operation"          SortExpression="Operation" />
                            <asp:BoundField DataField="AffiliateName"           HeaderText="Name"          SortExpression="AffiliateName" />
                            <asp:BoundField DataField="AffiliateMedicaidID"           HeaderText="CPC ID"          SortExpression="AffiliateMedicaidID" />
                            <asp:BoundField DataField="GroupMedicaidID"           HeaderText="Group Medicaid ID"          SortExpression="GroupMedicaidID" />
                            <asp:BoundField DataField="StartDate"           HeaderText="Start Date"          SortExpression="StartDate"  DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:BoundField DataField="EndDate"           HeaderText="End Date"          SortExpression="EndDate"  DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:BoundField DataField="Member_Status"           HeaderText="Member Status"          SortExpression="Member_Status" />
                            <asp:BoundField DataField="UserName" HeaderText="Username" SortExpression="UserName" />
                            <asp:BoundField DataField="DateOfAction" HeaderText="DateOfAction" SortExpression="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>
            </asp:panel>
            <asp:Button id="btnCloseHistory"  runat="server" Text="OK" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
            <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkHistoryExcel_Click" />
        </div>
        <asp:button runat="server" id="ButtonDummy3" style="display: none" text="”ButtonDummy3”" />
    </asp:panel>
</asp:panel>
<div class="divHistoryAndAdd" style="vertical-align: middle;">
    <asp:LinkButton ID="btnHistory" runat="server" AlternateText="History" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" ToolTip="History" Text="History" Style="vertical-align: middle; color: white; text-decoration: none; padding: 4px;" />
</div>
<div style="width: 100%; text-align: right">
    <telerik:RadGrid ID="RadGridExportPP" runat="server" Visible="true">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true">
            <Pdf PageHeight="8.5in" PageWidth="11in" PageTitle="Affiliate Search">
                <PageFooter>
                    <RightCell Text="Page <?page-number?>" />
                </PageFooter>
            </Pdf>
        </ExportSettings>
        <MasterTableView AutoGenerateColumns="false" TableLayout="Auto">
            <Columns>
                <telerik:GridBoundColumn UniqueName="col1" HeaderText="Name" DataField="GroupName"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col2" HeaderText="NPI" DataField="MedicaidID"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col3" HeaderText="Provider Type" DataField="MedicaidID"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col5" HeaderText="Start Date" DataField="START_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col6" HeaderText="End Date" DataField="END_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="false"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn UniqueName="col7" HeaderText="Member Status" DataField="MEMBER_STATUS"></telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click" Visible="false"><img src="../Images/Excel_24x24.png" alt="XLS" /></asp:LinkButton>&nbsp;&nbsp;

    
</div>
<%-- NOTE: There is a bug in asp.net 4.0 that makes it so ImageButtons fail in IE10 when they are in an
    UpdatePanel. Do not uncomment this UpdatePanel without addressing this bug!  Best replacing ImageButtons
    with LinkButtons containing Images. --%>

<asp:UpdatePanel ID="upAff" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div>
            <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
            <asp:ValidationSummary ID="valGroupAndFacilityAffiliations" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valGroupAndFacilityAffiliations" />
        </div>
        <div class="popTitle">
            <asp:Label ID="Label3" CssClass="bodyTextBold groupMemberTitle" runat="server" Text="Practice Partnership" />
        </div>
        <span class="pageHeader">Practices in the Practice Partnership </span>
        <div id="AffiliationInfo">
            <div style="padding-top: 10px; padding-left: 10px;">
                <asp:Label runat="server" ID="lblStaticText" CssClass="error-message" />
                <%--<p style="color: red;">
                    Confirm existing members of your practice partnership by clicking on the green check mark or remove
members by clicking on the red X. Add members to the partnership by clicking the Add New button.
                </p>--%>
                <span></span>
            </div>
            <div class="divGrid">
           

                <asp:GridView runat="server" Width="100%" ID="grdConfirmedGroupAffiliations" AutoGenerateColumns="False" OnRowCommand="grdConfirmedGroupAffiliations_RowCommand" OnPageIndexChanging="grdConfirmedGroupAffiliations_PageIndexChanging"
                    CssClass="gridViewSmallFont" EmptyDataText="No confirmed affiliations found."
                    AllowPaging="true" PageSize="10" 
                    DataKeyNames="AffiliateReg_Id,REG_AFFILIATION_ID,StartDate,CanReattestMember,GROUP_AFFILIATION_STATUS_ID,CPC_PROGRAM_YEAR,TOTAL_ATTRIBUTED_MEMBERS,TOTAL_ATTRIBUTED_KIDS,MEMBER_STATUS"  OnRowDeleting="grdConfirmedGroupAffiliations_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="AffiliateName" HeaderText="Name" />
                        <asp:BoundField DataField="GroupMedicaidID" HeaderText="CPC ID" />
                        <asp:BoundField DataField="AffiliateMedicaidID" HeaderText="Medicaid ID" />
                        <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="EndDate" HeaderText="End Date"  DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="MEMBER_STATUS" HeaderText="Member Status" />

                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete"
                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    ToolTip="Delete Member" Visible="<%# CanDeleteMember( ((GridViewRow) Container).RowIndex)  %>">
                                    <asp:Image ID="imgDelete" ImageUrl="~/Images/cancel.png" runat="server" BorderStyle="None" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnReattest" runat="server" CommandName="Attest"
                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    ToolTip="Attest Member" Visible="<%# ExistingGroupMember( ((GridViewRow) Container).RowIndex)  %>">
                                    <asp:Image ID="imgReattest" ImageUrl="~/Images/Green_Circle_Checkbox.png" runat="server" BorderStyle="None" />
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
            <div class="divHistoryAndAdd">

                <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/Images/add.png" CommandName="CPCGroupMember" OnCommand="btnAdd_Click" ToolTip="Add" />
            </div>

            <div class="divHistoryAndAdd">
               
            </div>
            <div class="btnBox">
            </div>
            <br />
            <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy" BackgroundCssClass="modalBackground" />
            <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
                <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                    <div class="popTitle">
                        <asp:Label ID="lblTitle" CssClass="bodyTextBold hospitalAffiliationsTitle" runat="server" Text="Title" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlMain" runat="server">
                    <asp:MultiView ID="mltPopup" runat="server">
                        <asp:View ID="vwGroupAffiliations" runat="server">
                            <div style="text-align: left; padding: 15px" class="container-fluid">
                                <div class="row">
                                    <ucCPC:CPCGroupMember ID="ucCPCGroupMember" runat="server" />
                                </div>
                            </div>
                        </asp:View>

                    </asp:MultiView>
                </asp:Panel>
            </asp:Panel>
            <div>


                <asp:PlaceHolder runat="server" ID="PlaceholderUploadPracticePartnership"></asp:PlaceHolder>
            </div>
            <asp:Button runat="server" ID="ButtonDummy" Style="display: none" />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <cc1:ModalPopupExtender ID="mpe2" runat="server" PopupControlID="Panel1" TargetControlID="Button1" BackgroundCssClass="modalBackground" />
            <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
                <asp:Panel ID="Panel2" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                    <div class="popTitle">
                        <asp:Label ID="lblTitle1" CssClass="bodyTextBold hospitalAffiliationsTitle" runat="server" Text="Title" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server">
                    <asp:MultiView ID="MultiView1" runat="server">
                        <asp:View ID="View1" runat="server">
                           
             
                        </asp:View>
                    </asp:MultiView>
                </asp:Panel>
            </asp:Panel>
            <asp:Button runat="server" ID="Button1" Style="display: none" />
            <uc:MessageBox ID="ucMessageBox" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="hidID" runat="server" /> 
<div style="width: 100%; text-align: right">
    <asp:HiddenField ID="hdnRowCount" runat="server" />

</div>
    <div style="width: 100%; text-align: right; display: none;">
    <telerik:RadGrid ID="grdHistoryExport" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
        AutoGenerateColumns="true" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
            <Excel Format="Biff" />
        </ExportSettings>
        <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
            DataKeyNames="DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
            <Columns>
                <telerik:GridBoundColumn DataField="OPERATION"           HeaderText="Operation"          SortExpression="Operation" />
                <telerik:GridBoundColumn DataField="AffiliateName"           HeaderText="Name"          SortExpression="AffiliateName" />
                <telerik:GridBoundColumn DataField="AffiliateMedicaidID"           HeaderText="CPC ID"          SortExpression="AffiliateMedicaidID" />
                <telerik:GridBoundColumn DataField="GroupMedicaidID"           HeaderText="Group Medicaid ID"          SortExpression="GroupMedicaidID" />
                <telerik:GridBoundColumn DataField="StartDate"           HeaderText="Start Date"          SortExpression="StartDate"  DataFormatString="{0:MM/dd/yyyy}" />
                <telerik:GridBoundColumn DataField="EndDate"           HeaderText="End Date"          SortExpression="EndDate"  DataFormatString="{0:MM/dd/yyyy}" />
                <telerik:GridBoundColumn DataField="Member_Status"           HeaderText="Member Status"          SortExpression="Member_Status" />
                <telerik:GridBoundColumn DataField="UserName" HeaderText="Username" SortExpression="UserName" />
                <telerik:GridBoundColumn DataField="DateOfAction" HeaderText="DateOfAction" SortExpression="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>
</div>
