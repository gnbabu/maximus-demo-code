<%@ Control Language="C#" AutoEventWireup="true" Inherits="Pages_MCOAffiliations" Codebehind="MCOAffiliations.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<%@ Register Src="~/UserControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>--%>
<%@ Register Src="~/PopupControls/MCOAffiliationsPopUp.ascx" TagPrefix="uc" TagName="MCOAffiliationsPopUp" %>

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

    function removeDisabled() {
        $("#<%= btnCloseHistory.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    }
</script>
<%-- Note This following script is to handle the what is this? link in group affiliations popup control. Soince it was not triggering document .ready() in the popup control, it is writtern in the containing control --%>
<script type="text/javascript">
    $(document).ready(function () {

        //Update Panel needs after async postback, else event handlers are lost.
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(setupStartDate);

    });

    function setupStartDate() {

        $("#divRequestedEffectiveDateInfo1").hide();
        $(".what-is-this-link1").mouseover(function () {
            $("#divRequestedEffectiveDateInfo1").show();
        });

        $("#divRequestedEffectiveDateInfo1").mouseleave(function () {
            $("#divRequestedEffectiveDateInfo1").hide();
        });
    }
</script>

<%-- NOTE: There is a bug in asp.net 4.0 that makes it so ImageButtons fail in IE10 when they are in an
    UpdatePanel. Do not uncomment this UpdatePanel without addressing this bug!  Best replacing ImageButtons
    with LinkButtons containing Images. --%>

<%--<uc1:Separator ID="Separator11" runat="server" Header="MCO Affiliation" Mode="1" />--%>

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
                            <asp:BoundField DataField="MANAGED_CARE_PLAN"           HeaderText="Managed Care Plan"          SortExpression="MANAGED_CARE_PLAN" />
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

<asp:UpdatePanel ID="upGA_Main" runat="server" UpdateMode="Conditional">

    <ContentTemplate>
        <div id="ParentTable" runat="server">
            <div class="row" id="trDateOccurence">
                <div class="col-sm-8"><span class="formLabel200">Are you interested in contracting with any of the State Medicaid Managed Care Plans?</span></div>
                <div class="col-sm-4 text-left">
                    <asp:RadioButtonList ID="rblIsContracting" runat="server" role="presentation" RepeatDirection="Horizontal" CssClass="QstRadioList" OnSelectedIndexChanged="rblIsContracting_SelectedIndexChanged" AutoPostback="true">
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="0">No</asp:ListItem>
                    </asp:RadioButtonList>

                </div>
            </div>
        </div>
        <div id="divplans" runat="server">
            <div class="row" id="trPossParticipation">
                <asp:label id="lblerrormsg" runat="server" CssClass="failureNotification"></asp:label>
                <div class="col-sm-12"><span class="formLabel200">Indicate your interested in possible participation with one or more State Medicaid Managed Care Plans</span></div>
                <div class="col-sm-12 text-left">
                        <asp:CheckBoxList runat="server" ID="chkPossibleParticipation" CausesValidation="true" role="presentation">
                        <asp:ListItem Text="AmeriHealth Caritas" Value="7"></asp:ListItem>
                        <asp:ListItem Text="Anthem Blue Cross" Value="8"></asp:ListItem>
                        <asp:ListItem Text="Aetna" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Buckeye" Value="2"></asp:ListItem>
                        <asp:ListItem Text="CareSource" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Humana" Value="9"></asp:ListItem>                            
                        <asp:ListItem Text="Molina" Value="4"></asp:ListItem>
                        <asp:ListItem Text="United Health Care" Value="5"></asp:ListItem>
                    </asp:CheckBoxList>
                </div>
            </div>
        </div>

        <div id="AffiliationInfo">


            <p id="paraVerifyIndividual" runat="server" visible="true">
                <b>Please Note: </b>
                <span style="color: #de2316;">This indication does not ensure a contract with the State Medicaid Managed Care Plans. Providers must still go thru the plan’s contracting process, if applicable</span>
                <br />

            </p>

            <div class="divHistoryAndAdd" runat="server" visible="false">
                <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/Images/add.png" CommandName="MCOAffiliations" OnCommand="btnAdd_Click" ToolTip="Add" />
            </div>
        </div>
        <br />


        <br />
        <br />
        <asp:UpdateProgress runat="server" ID="upGA_MainSaveProgress" DisplayAfter="0" AssociatedUpdatePanelID="upGA_Main">
            <ProgressTemplate>
                <div class="loading">
                    <asp:Image ID="imgSaving" runat="server" AlternateText="Saving" ImageUrl="~/Images/ajax-loader.gif" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy" BackgroundCssClass="modalBackground" />
        <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
            <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                <div class="popTitle">
                    <asp:Label ID="lblTitle" CssClass="bodyTextBold" runat="server"/>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlMain" runat="server">
                <asp:MultiView ID="mltPopup" runat="server">
                    <asp:View ID="vwGroupAffiliations" runat="server">
                        <uc:MCOAffiliationsPopUp ID="ucMCOAffiliationsPopUp" runat="server" />
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />
    </ContentTemplate>
</asp:UpdatePanel>
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
                    <telerik:GridBoundColumn DataField="MANAGED_CARE_PLAN"           HeaderText="Managed Care Plan"          SortExpression="MANAGED_CARE_PLAN" />
                    <telerik:GridBoundColumn DataField="UserName" HeaderText="Username" SortExpression="UserName" />
                    <telerik:GridBoundColumn DataField="DateOfAction" HeaderText="DateOfAction" SortExpression="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>

</div>