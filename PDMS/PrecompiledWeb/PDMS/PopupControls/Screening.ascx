<%@ control language="C#" autoeventwireup="true" inherits="Pages_Screening, App_Web_tiu3g34i" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/ScreeningGroupAffiliations.ascx" TagName="ScreeningGroupAffiliations" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/ScreeningGroupOwners.ascx" TagName="ScreeningGroupOwners" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/ScreeningHouseholdMembers.ascx" TagName="ScreeningHHMbrs" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/ScreeningResult.ascx" TagName="ScreeningResult" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/ScreeningHistory.ascx" TagName="ScreeningHistory" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/ScreeningActivities.ascx" TagName="ScreeningActivities" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/AdverseAction.ascx" TagName="AdverseAction" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/NonScreeningResult.ascx" TagName="NonScreeningResult" TagPrefix="uc" %>

<asp:Panel runat="server" ID="pnlTakeAction" Visible="false" >
    <div style="float:right; padding-right: 10px;">
            <asp:Button id="btnTakeAction" runat="server" Text="Take Action" CssClass="buttonBoxFocus" Visible="true" ToolTip="Take action" OnClick="btnTakeAction_Click" />
    </div>
        <div style="clear:both;"></div>
    <br />
    <br />
</asp:Panel>
<asp:Panel ID="pnlScreeningHistory" runat="server">
    <uc1:Separator runat="server" ID="ucScreeningHistoryTitle" Header="Provider Screening History" />
    <uc:ScreeningHistory runat="server" ID="ucScreeningHistory" OnScreeningSelected="ucScreeningHistory_ScreeningSelected" />
    <br />
<br />
</asp:Panel>

<asp:Panel ID="pnlGroupAffiliations" runat="server">
    <uc1:Separator ID="Separator1" runat="server" Header="Provider Individual Member Summary" />
    <uc:ScreeningGroupAffiliations runat="server" ID="ucScreeningGroupAffiliations" OnAffiliationScreeningSelected="ucScreeningGroupAffiliations_AffiliationScreeningSelected" />
    <br />
    <br />
</asp:Panel>
<asp:Panel ID="pnlOwnerScreening" runat="server">
    <uc1:Separator ID="Separator2" runat="server" Header="Provider Owner Summary" />
    <uc:ScreeningGroupOwners runat="server" ID="ucScreeningGroupOwners" OnOwnerScreeningSelected="ucScreeningGroupOwners_OwnerScreeningSelected"  />
    <br />
    <br />
</asp:Panel>
<asp:Panel ID="pnlHouseholdScreening" runat="server">
    <uc1:Separator ID="Separator3" runat="server" Header="Provider Household Member Summary" />
    <uc:ScreeningHHMbrs runat="server" ID="ucScreeningHHMbrs"  OnHHMemberScreeningSelected="ucScreeningHHMbrs_HHMemberScreeningSelected" />
    <br />
    <br />
</asp:Panel>

<asp:Panel runat="server" ID="pnlScreeningActivities" Visible="false">
    <uc1:Separator ID="ucScreeningActivitiesTitle" runat="server" Header="Provider Screening Details" />
    <uc:ScreeningActivities runat="server" ID="ucScreeningActivities"   OnScreeningActivitySelected="ucScreeningActivities_ScreeningActivitySelected" 
            OnScreeningActivityDataBind="ucScreeningActivities_ScreeningActivityDataBind" IsReadOnly="false" />
    <br />
    <br />
</asp:Panel>
<asp:MultiView runat="server" ID="mvActivityDetails">
    <asp:View runat="server" ID="vwScreeningMatchDetails" >
        <uc1:Separator ID="Separator4" runat="server" Header="Screening Results" />
        <uc:ScreeningResult runat="server" ID="ucScreeningResult" OnCancel="ucScreeningResult_Cancel"  
            OnScreeningActivityUpdated="ucScreeningResult_ScreeningActivityUpdated" OnCreateAdverseAction="ucScreeningResult_CreateAdverseAction" />
    </asp:View>
    <asp:View runat="server" ID="vwNonScreeningDetails">
        <uc1:Separator ID="Separator5" runat="server" Header="Non-Screening Results" />
        <br />
        <uc:NonScreeningResult runat="server" ID="ucNonScreeningResult" OnCancel="ucNonScreeningResult_Cancel" OnScreeningActivityUpdated="ucNonScreeningResult_ScreeningActivityUpdated" />
    </asp:View>
</asp:MultiView>

    <!-- ModalPopupExtender copied from RegistrationNavigation for now-->
<asp:Button runat="server" ID="btnDummy" aria-Label="dummyButton" Style="visibility:hidden;" />

    <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="btnDummy"
    CancelControlID="btnCancelmpe" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlModal">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" align="center" style="display:none">
    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" >
         <div class="popTitle">Take Action</div>
    </asp:Panel>  
    <asp:Panel ID="pnlLabel" runat="server">
        <div style="text-align: left;padding: 15px">
            <div><asp:ValidationSummary ID="vsTakeAction" runat="server" DisplayMode="List" ValidationGroup="valTakeAction" /></div>
            <asp:RadioButtonList ID="rblScrReview" BorderStyle="None" CellPadding="0" CellSpacing="0" 
                RepeatDirection="Horizontal" runat="server" RepeatLayout="Table" CssClass="QstRadioList">  
                <asp:ListItem Value="1">Screening Complete</asp:ListItem>  
            </asp:RadioButtonList>
            <asp:Panel ID="pnlScreeningDone" runat="server">
                <br />
                <table border="0" cellpadding="2" cellspacing="2">
                    <tr >
                        <td><span class="formLabel">Comments</span></td>
                        <td><asp:TextBox ID="txtComments" runat="server" aria-label="Comments" MaxLength="1000" CssClass="formFieldLarge" TextMode="MultiLine" Columns="2000" Rows="7" /></td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </asp:Panel>
    <table border="0" >
        <tr>
            <td>
                <asp:Button id="btnMarkComplete" runat="server" Text="Save" CssClass="buttonBoxFocus" CausesValidation="true" ValidationGroup="valTakeAction" 
                    onclick="btnMarkComplete_Click" />
            </td>
            <td>
                <asp:Button id="btnCancelmpe"  runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancelmpe_Click" 
                    CausesValidation="false" />
            </td>
        </tr>
    </table>
    <br /> 
</asp:Panel>
