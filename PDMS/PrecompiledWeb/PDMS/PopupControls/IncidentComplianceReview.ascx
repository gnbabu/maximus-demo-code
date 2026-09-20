<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_IncidentComplianceReview, App_Web_wbqq1lcm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uss" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>

<style type="text/css">
    .seperatorLine {
        display: block;
        border: 1px solid #00008b;
        width: auto;
        margin: auto;
        margin-top: 1px;
        margin-bottom: 2px;        
    }

    
        .largerCheckbox { 
            width: 30px; 
            height: 30px; 
        } 
        .hideGridColumn {
              display: none;
             }

       
    
</style>

<%--<asp:UpdatePanel ID="upIncident" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
        <div>
            <asp:ValidationSummary ID="vsIncidentCompliance" runat="server" DisplayMode="List" ValidationGroup="valIncidentCompliance" />
             <asp:Label ID="lblError" runat="server" Visible="false" Text="" CssClass="failureNotification" Style="color: Red" />
        </div>
        <asp:Panel ID="pnlCaseSummary" runat="server">
            <span class="pageHeader">Case Summary</span>
            <hr class="seperatorLine" />
            <div class="divGrid">
                <div class="grid-hint-md" id="dvHint1" runat="server" style="text-align: left">Select Case Number hyperlink to view detail below.</div>        
                <asp:GridView runat="server" Width="15%" ID="grdCaseNumbers" AutoGenerateColumns="False" CssClass="gridViewSmallFont" EmptyDataText="No Case Numbers found."
                    ShowHeaderWhenEmpty="true" DataKeyNames="INCIDENT_CASE_NUMBER,CASE_STATUS,REG_INCIDENT_COMPLIANCE_CASE_XREF_ID" OnRowDataBound="grdCaseNumbers_RowDataBound" OnRowCommand="grdCaseNumbers_RowCommand" OnSelectedIndexChanged="grdCaseNumbers_SelectedIndexChanged">
                        <Columns>
                            <asp:TemplateField HeaderText="Case Number">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCaseNum" runat="server" CommandName="SelectCaseNumber" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                        ToolTip="Select Case Number" Text='<%# Eval("INCIDENT_CASE_NUMBER") %>'>
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
       </asp:Panel>
        <br />
    <asp:Panel ID="pnlIncidentdtls" runat="server">
        <span class="pageHeader">Incident Compliance Summary</span>
        <hr class="seperatorLine" />
        <div class="divGrid">
            <div class="row">
            <div class="col-md-6" id="dvHint2" runat="server" style="text-align: left">Select hyperlink in Incident Type to see more detail.</div> 
                <%--<div class="col-md-12" style="text-align: right"  ><asp:Button runat="server" ID="btnIssueNod" BackColor="Orange" CssClass="buttonBoxFocus" Text="Issue NOD"  EnableViewState="true" CausesValidation="true"   OnClick="btnIssueNOD_Click"/></div>--%>
                </div>
            <asp:GridView runat="server" Width="100%" ID="grdIncidentnumbers" AutoGenerateColumns="False" CssClass="gridViewSmallFont" EmptyDataText="No Incident Numbers found."
                ShowHeaderWhenEmpty="true" DataKeyNames="INCIDENT_CASE_NUMBER,INCIDENT_ID,NOD_ISSUED_DATE,NOD_REASON,CS_NOTES,NOD_NEEDED,REG_INCIDENT_COMPLIANCE_CASE_DETAIL_ID,IMS_ASSOCIATE_ID" OnRowCommand="grdIncidentnumbers_RowCommand" OnRowDataBound="grdIncidentnumbers_RowDataBound">
                    <Columns>    
                           <asp:BoundField DataField="REG_INCIDENT_COMPLIANCE_CASE_DETAIL_ID" HeaderText="Incident Type" ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn" />
                        <asp:TemplateField HeaderText ="NOD Needed">
                            <ItemTemplate>
                               <asp:RadioButtonList ID="rblnodNeeded" runat="server" SelectedValue='<%# Bind("NOD_NEEDED") == null ? "": Bind("NOD_NEEDED") %>' OnSelectedIndexChanged="SelectionChanged" AutoPostBack="true" RepeatDirection="Horizontal" CssClass="QstRadioList" BorderStyle="None" CellPadding="0" CellSpacing="0">
                                   <asp:ListItem Text="Yes" Value="True" />
                                   <asp:ListItem Text="No" Value="False" />
                                   <asp:ListItem Text="" Value="" style="display:none" />
                               </asp:RadioButtonList>
						
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:BoundField DataField="INCIDENT_CASE_NUMBER" HeaderText="Case Number" />
                        <asp:TemplateField HeaderText="Incident ID">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkIncidentNumber" runat="server" CommandName="SelectIncident" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    ToolTip="Select Case Number" Text='<%# Eval("INCIDENT_ID") %>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="INCIDENT_TYPE" HeaderText="Incident Type" />
                        <asp:BoundField DataField="INCIDENT_CATEGORY" HeaderText="Category" />
                        <asp:BoundField DataField="INCIDENT_SUB_CATEGORY" HeaderText ="Subcategory" />
                        <asp:TemplateField HeaderText="Reason for NOD">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkreasonNOD" runat="server" CommandName="SelectIncident" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  Text="...">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NOD_ISSUED_DATE" HeaderText="NOD Date" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="NOD_REFERRAL_DATE" HeaderText="Date from IMS" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:TemplateField HeaderText="CS Notes">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkcsNotes" runat="server" CommandName="SelectIncident" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  Text="...">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkIssueNOD" runat="server" CommandName="IssueNOD" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                        ToolTip="Issue NOD" Text="Issue NOD">
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
             <br />
            <asp:Panel ID="pnlIncidentnotes" runat="server">
             <div class="row">
                 <div class="col-sm-4">
                     <asp:Label ID="lblIncidentID" runat="server" Text="Incident ID" CssClass="formLabel300" />
                 </div>
                 <div class="col-sm-8">
                     <asp:TextBox ID="txtIncidentID" runat="server" CssClass="formField" Enabled="false" />
                 </div>
             </div>
            <div class="row">
                 <div class="col-sm-4">
                     <asp:Label ID="lblReason" runat="server" Text="Reason for NOD" CssClass="formLabel300" />
                 </div>
                 <div class="col-sm-8">
                     <asp:TextBox ID="txtReasonNOD" runat="server" CssClass="formFieldMultiline" Enabled="false" TextMode="MultiLine" />
                 </div>
             </div>
            <div class="row">
                 <div class="col-sm-4">
                     <asp:Label ID="lblNODdt" runat="server" Text="NOD Date" CssClass="formLabel300" />
                 </div>
                 <div class="col-sm-8">
                     <asp:TextBox ID="txtNODdate" runat="server" CssClass="formField" Enabled="false" ValidationGroup="valIncidentCompliance" CausesValidation="true" />
                     <ajax:CalendarExtender ID="calNODDate" TargetControlID="txtNODdate" runat="server" />
                     <asp:CompareValidator ID="cvDateOfProposedAdjudication" runat="server" ValidationGroup="valIncidentCompliance"
                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtNODdate" Enabled="true"
                        ErrorMessage="Select a valid Date for NOD date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                        SetFocusOnError="true" />
                 </div>
             </div>
             <div class="row">
                 <div class="col-sm-4">
                     <asp:Label ID="lblCSnotes" runat="server" Text="Compliance Specialist Notes" CssClass="formLabel300" />
                 </div>
                 <div class="col-sm-8">
                     <asp:TextBox ID="txtCSnotes" runat="server" CssClass="formFieldMultiline" Enabled="false" TextMode="MultiLine" MaxLength="1000" />
                 </div>
             </div>
            <div class="row btnBoxCenter">
              <br />
              <asp:Button runat="server" ID="btnSaveComments" CssClass="buttonBoxFocus" Text="Save"  EnableViewState="true" CausesValidation="true" Enabled="false" OnClick="btnSaveIncident_Click"/>
              <asp:Button runat="server" ID="btnCancelComments" CssClass="buttonBox" Text="Cancel"  CausesValidation="false" Enabled="false" OnClick="btnCancelIncident_Click" />
            </div>
                <div class="col-sm-8" visible="false">
                     <asp:TextBox ID="txtnodneed" visible="false" runat="server" CssClass="formFieldMultiline" Enabled="false" TextMode="MultiLine" MaxLength="1000" />
                 </div>
          </asp:Panel>
        </div>
      </asp:Panel>
      <asp:Panel ID="pnlPlanofCorrection" runat="server">
          <span class="pageHeader">Plan of Correction</span>
          <hr class="seperatorLine" />
          <div class="row">
            <div class="col-sm-4">
                <asp:Label ID="lblDateOfPOC" runat="server" CssClass="formLabel300" Text="Plan Of Correction Date" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtDateOfPOC" runat="server" CssClass="formField" Enabled="false" />
            </div>
        </div>
        <br />
        <br />
          <div class="row">
             <asp:PlaceHolder runat="server" ID="PlaceholderPOC"></asp:PlaceHolder>
          </div>                
        <br />
       <%--   <span class="pageHeader">Revised Plan of Correction</span>
          <hr class="seperatorLine" />--%>
          <div class="row">
            <div class="col-sm-4">
                <asp:Label ID="Label1" runat="server" CssClass="formLabel300" Text="Revised Plan Of Correction Date" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtDateRevPOC" runat="server" CssClass="formField" Enabled="false" />
            </div>
        </div>
          <br />  <br />
          <div class="row">
             <asp:PlaceHolder runat="server" ID="PlaceholderRevisedPOC"></asp:PlaceHolder>
          </div>
      </asp:Panel>
        <asp:Panel ID="pnlPOCByMail" runat="server">
            <div class="row">
                <div class="col-sm-4" id="dvMailId" runat="server" style="text-align:right"><b>I need to send Plan of Correction by Mail .</b></div> 
               <div class="col-sm-8 text-left">
             <span class="largerCheckbox"><asp:CheckBox runat="server"  ID="chkpocByMail" Checked= "false" AutoPostBack ="true" /></span>
              </div>
            </div>
            <br />
            <div class="row">
                <p style="text-align:left">Note: Check this box if you are mailing a hard copy of your plan of correction to ODM and then select submit for review</p>
            </div>
            <div class="row">
                   <strong>Mailing Address:</strong><br />
                    Ohio deparment of Medicaid<br />
                    PNM Compliance Unit<br />
                    PO Box 1461 <br />
                    Columbus, OH 43216-1461<br />                   
                </div>

            </asp:Panel>
          <div class="row btnBoxCenter">
              <br />
              <asp:Button runat="server" ID="btnSaveDoc" CssClass="buttonBoxFocus" Text="Save Documents"  EnableViewState="true" CausesValidation="true" OnClick="btnSaveDoc_Click"/>
              </div>
 <%--   </ContentTemplate>
</asp:UpdatePanel>--%>
<asp:HiddenField ID="hdnNodNeeded" runat="server" />
<asp:HiddenField ID="hdnCaseNumber" runat="server" />
<asp:Label ID="hdnIncidentCaseDtlID" runat="server" Visible="false" />
<asp:Label ID="lblRegCaseNum" runat="server" Visible="false" />
