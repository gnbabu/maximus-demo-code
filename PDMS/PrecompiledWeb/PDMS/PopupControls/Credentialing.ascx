<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_Credentialing, App_Web_2k5drnu4" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/CredentialResult.ascx" TagName="CredentialResult" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/CredentialHistory.ascx" TagName="CredentialHistory" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/CredentialActivities.ascx" TagName="CredentialActivities" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/AdverseAction.ascx" TagName="AdverseAction" TagPrefix="uc" %>

<%@ Register Src="~/PopupControls/CredentialCommitteeMember.ascx" TagName="CredentialCommitteeMember" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/CredentialCommitteeResult.ascx" TagName="CredentialCommitteeResult" TagPrefix="uc" %>

<%--<asp:Panel runat="server" ID="pnlTakeAction" Visible="false" >
    <div style="float:right; padding-right: 10px;">
            <asp:Button id="btnTakeAction" runat="server" Text="Take Action" CssClass="buttonBoxFocus" Visible="true" ToolTip="Take action" OnClick="btnTakeAction_Click" />
    </div>
        <div style="clear:both;"></div>
    <br />
    <br />
</asp:Panel>--%>
<asp:ValidationSummary ID="vsBoardCertifications" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valCredentialing" />
<asp:Panel ID="pnlCredReconsideration" runat="server" Visible="false">
   <h2> <uc1:Separator runat="server" ID="ucCredReconSep" Header="Reconsideration" /></h2>
   <div class="row btnBoxCenter">
      <br />
      <asp:Button runat="server" ID="btnUpheld" CssClass="buttonBoxFocus" Text="ODM Decision Upheld"  EnableViewState="true" CausesValidation="true"  OnClick="btnUpheld_Click"/>
      <asp:Button runat="server" ID="btnNotUpheld" CssClass="buttonBoxFocus" Text="ODM Decision Not Upheld"  EnableViewState="true" CausesValidation="true" OnClick="btnNotUpheld_Click" />
   </div>
</asp:Panel>

<asp:Panel ID="pnlCredentialingHistory" runat="server" Visible="true">
   <h2> <uc1:Separator runat="server" ID="ucScreeningHistoryTitle" Header="Provider Credential History" /></h2>
    <uc:CredentialHistory runat="server" ID="ucCredentialHistory"  OnCredentialSelected="ucCredentialHistory_ScreeningSelected" />
    <br />
<br />
</asp:Panel>

<asp:Panel ID="pnlComments" runat="server" Visible="true">   
   <h2><uc1:Separator runat="server" ID="sepCredComments" Header="Credential Comments" /> </h2> 
   <div id="divCredentialComments" class="container-fluid">
       <div class="row" style="text-decoration-color:red;">
           <asp:Label ID="lblCommentErr" runat="server" Visible="false" />
       </div>
       <div class ="row">
           <asp:DataList ID="dtlComments" runat="server" DataKeyField="REG_CREDENTIALING_COMMENTS_ID"
                 EnableViewState="False" Width="100%" HorizontalAlign="Center" RepeatLayout="Table" CellPadding="5"
                       CellSpacing="5" ItemStyle-Wrap="true" >
                <ItemTemplate>       
                    <table border="0" style="border-bottom-style:solid;border-bottom-color:grey;border-bottom-width: thin;table-layout:fixed;">
                        <tr>
                            <td class="formLabel text-right" style="width:250px;">CommentsBy:</td>
                            <td style="width:25%;"><asp:Label ID="lblCommentsBy" runat="server" CssClass="text-left"
                                Text='<%# Eval("USERNAME") %>'  /></td>
                            <td class="formLabel text-right" style="width:250px;">Role:</td>
                            <td style="width:25%;"><asp:Label ID="lblCmtRole" runat="server" CssClass="text-left"
                                Text='<%# Eval("ROLENAME") %>' /></td>
                        </tr>
                       <tr>
                            <td class="formLabel text-right" style="width:250px;">Comments:</td>
                            <td style="max-width:300px;word-wrap:break-word" colspan="3" ><asp:Label ID="lblComments" runat="server" CssClass="text-left"
                                Text='<%# Eval("COMMENTS") %>'  /></td>
               
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
       </div>
       <br />
       <asp:Panel ID ="pnlAddComments" runat="server">
           <div class="row">
            <div class="col-sm-3 text-right"><asp:Label runat="server" ID="lblactioncomments" CssClass="formLabel">Comments:</asp:Label></div>
            <div class="col-sm-9 text-left"><asp:TextBox ID="txtactioncomments" runat="server" MaxLength="1000" aria-label="Comments" CssClass="formFieldMultiline" TextMode="MultiLine"  Rows="7" /></div>
           </div>
          <div class="row btnBoxCenter">
            <br />
            <asp:Button runat="server" ID="btnSaveComments" CssClass="buttonBoxFocus" Text="Save"  EnableViewState="true" CausesValidation="true"  OnClick="btnSaveComments_Click"/>
            <asp:Button runat="server" ID="btnCancelComments" CssClass="buttonBox" Text="Cancel"  CausesValidation="false" OnClick="btnCancelComments_Click" />
          </div>
      </asp:Panel>
   </div>
</asp:Panel>
  
<asp:Panel runat="server" ID="pnlCommiteeResult" Visible="true">
<uc1:Separator runat="server" ID="ucCommittee" Header="Credential Committee" />
      <div style="padding: 5px;">
        <uc:CredentialCommitteeResult runat="server" id="CredentialCommitteeResult" /></div>
</asp:Panel>

<asp:Panel runat="server" ID="pnlCredentialActivities" Visible="true">
  <h2>  <uc1:Separator ID="ucCredentialActivitiesTitle" runat="server" Header="Provider Credentialing Details" /></h2>
    <uc:CredentialActivities runat="server" ID="ucCredentialActivities" OnCredentialActivitySelected="ucCredentialActivities_CredentialActivitySelected" IsReadOnly="false" OnCredentialActivityDataBind="ucCredentialActivities_CredentialActivityDataBind" />
    <br />
    <br />
</asp:Panel>
<asp:MultiView runat="server" ID="mvActivityDetails">
    <asp:View runat="server" ID="vwCredentialing" >
        <uc1:Separator ID="ucSeperatorResult" runat="server" Header="Credentialing Results" />
        <hr />
        <uc:CredentialResult runat="server" ID="ucCredentialResult" OnCancel="ucCredentialResult_Cancel"  
            OnCredentialActivityUpdated="ucCredentialResult_CredentialActivityUpdated" OnCreateAdverseAction="ucCredentialResult_CreateAdverseAction" />
    </asp:View>
    
</asp:MultiView>



    

    <!-- ModalPopupExtender copied from RegistrationNavigation for now-->
<%--<asp:Button runat="server" ID="btnDummy" Style="visibility:hidden;" />

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
                        <td><asp:TextBox ID="txtComments" runat="server" MaxLength="1000" CssClass="formFieldLarge" TextMode="MultiLine" Columns="2000" Rows="7" /></td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </asp:Panel>
    <table border="0" >
        <tr>
            <td>
                <asp:Button id="btnMarkComplete" runat="server" Text="Save" CssClass="buttonBoxFocus" CausesValidation="true" ValidationGroup="valTakeAction" 
                   />
            </td>
            <td>
                <asp:Button id="btnCancelmpe"  runat="server" Text="Cancel" CssClass="buttonBox"  
                    CausesValidation="false" />
            </td>
        </tr>
    </table>
    <br /> 
</asp:Panel>--%>
