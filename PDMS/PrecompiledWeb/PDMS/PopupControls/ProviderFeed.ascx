<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ProviderFeed, App_Web_tiu3g34i" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="ms" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="mbox" %>
<%@ Register Src="~/PopupControls/AddProviderFeed.ascx" TagName="AddProviderFeed" TagPrefix="uc7" %>

<div id="divProviderFeed" runat="server">    
    <div class="row">
        <uc1:Separator runat="server" ID="SeparatorProviderFeed" Header="Provider Feed" /> 
        <span style="float: right">
            <asp:ImageButton ID="btnAdd"  AlternateText="Add New" runat="server" ImageUrl="~/Images/add.png" ToolTip="Add New" /> 
        </span>
    </div>
    <hr class="underLine" />
    <asp:Panel runat="server" ID="pnlProviderFeedGrid">
        <asp:GridView 
            ID="gvProviderFeed" 
            runat="server" 
            AutoGenerateColumns="False" 
            CssClass="gridViewSmallFont" 
            AllowSorting="true" 
            RowStyle-VerticalAlign="Top" 
            AllowPaging="True" 
            PageSize="5" 
            CellPadding="3" 
            PagerSettings-Mode="NumericFirstLast" 
            OnPageIndexChanging="gvProviderFeed_PageIndexChanging"
            OnRowCommand="gvProviderFeed_RowCommand"
            DataKeyNames="reg_provider_feed_id,HistoricTmNotes,reg_id, InitiatedByUserId, person_reviewed_by_userid, processid, ProviderFeedProcessId" >
            <Columns>
                <asp:BoundField DataField="Notes_Date" HeaderText="Date" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="InitiatedBy" HeaderText="OH ID for Initiater" />
                <asp:BoundField DataField="person_reviewed_by" HeaderText="OH ID for Last ODM Review" />
                <asp:BoundField DataField="Enrollment_Type" HeaderText="Enrollment Type" />
                <asp:BoundField DataField="Final_Disposition" HeaderText="Final Disposition" />                
                <asp:TemplateField HeaderText="New_Notes" ItemStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                        <asp:LinkButton 
                            ID="lbtnText" 
                            runat="server" 
                            CausesValidation="false" 
                            Text="Add Note" 
                            CommandName="AddNote"
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            CssClass="gridLink" Width="150"/> 
                    </ItemTemplate> 
                </asp:TemplateField>
                <asp:BoundField DataField="HistoricTmNotes" HeaderText="Historic TM Notes" />  
            </Columns>   
            <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />  
            <HeaderStyle CssClass="gridViewHeader" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" /> 
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
        </asp:Panel>
      <uc1:Separator runat="server" ID="SeparatorHistoricNotes" Header="Historic Notes(Prior to 12/31/2023)" />
    <hr class="underLine" />
    <asp:Panel runat="server" ID="pnlHistoricNotes">
        <asp:GridView 
            ID="gvHistoriceNotes" 
            runat="server" 
            AutoGenerateColumns="False" 
            CssClass="gridViewSmallFont" 
            AllowSorting="true" 
            EmptyDataText="No matching records found." 
            RowStyle-VerticalAlign="Top" 
            AllowPaging="True" 
            PageSize="2" 
            CellPadding="3" 
            PagerSettings-Mode="NumericFirstLast"
            OnPageIndexChanging="gvHistoriceNotes_PageIndexChanging"
            OnRowCommand="gvProviderFeed_RowCommand">
            <Columns>
                <asp:BoundField DataField="NOTE_DATE_TIME" HeaderText="Date" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="provider_note_type" HeaderText="Type" />
                <asp:BoundField DataField="UserName" HeaderText="User" />
                <asp:BoundField DataField="workflow_event_type" HeaderText="Workflow" />
                <asp:BoundField DataField="Screen" HeaderText="Screen" /> 
                <asp:BoundField DataField="NOTE_TEXT" HeaderText="Note" /> 
            </Columns>   
            <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />  
            <HeaderStyle CssClass="gridViewHeader" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" /> 
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </asp:Panel>

    <cc1:modalpopupextender id="modalAddProviderFeed" behaviorid="mpe" runat="server"
        popupcontrolid="pnlAdd" targetcontrolid="btnAdd" backgroundcssclass="modalBackground">
    </cc1:modalpopupextender>
    <asp:panel id="pnlAdd" runat="server" cssclass="modalPopup" style="padding: 20px; width: 50%;">
        <div class="center">
            <uc7:AddProviderFeed id="ucAddProviderFeed" runat="server" mode="Grid" />
        </div>
        <%--<div class="center">
            <asp:button runat="server" id="btnHide" text="Close" cssclass="buttonBox" />
        </div>--%>
    </asp:panel>

    <cc1:modalpopupextender id="modalAddNote" runat="server"
        popupcontrolid="pnlAddNote" targetcontrolid="btnProvFeedDummy" cancelcontrolid="btnHideAddNote" backgroundcssclass="modalBackground" >
    </cc1:modalpopupextender>
    
    <asp:panel id="pnlAddNote" runat="server" cssclass="modalPopup" style="display: none; min-height: 200px; min-width: 600px; height: auto; width: auto; position: fixed; top: 300px; left: 700px;">
       <uc1:Separator runat="server" ID="Separator1" Header="Notes" />
        <div class="center">
            <div class="row">
                <div class="col-sm-3 text-right" style="padding-right: 0px;"><span>Comments:*</span></div>
                <div class="col-sm-9 text-left">
                    <asp:textbox id="txtComments" runat="server" maxlength="1000" validationgroup="ValNotProcessed" textmode="MultiLine" columns="40" rows="6" />
                    <asp:requiredfieldvalidator id="valCommentRequired" runat="server" controltovalidate="txtComments"
                        errormessage="* Comments are required." display="Dynamic" text="*"
                        validationgroup="ValNotProcessed" />
                </div>
                <div class="row ">
                    <div class="center">
                        <asp:button runat="server" id="btnSaveNote" text="Save" cssclass="buttonBox" OnClick="btnSaveNote_Click" />
                        <asp:button runat="server" id="btnHideAddNote" text="Cancel" cssclass="buttonBox" 
                            CausesValidation="false" OnClick="btnHideAddNote_Click" />
                        <asp:hiddenField id ="hdnregProviderFeedID" runat="server" />
                        <asp:hiddenField id ="hdnHistoricNotes" runat="server" />
                        <asp:hiddenField id ="hdnregid" runat="server" />
                        <asp:hiddenField id ="hdnInitiatedByUserId" runat="server" />
                        <asp:hiddenField id ="hdnperson_reviewed_by_userid" runat="server" />
                        <asp:hiddenField id ="hdnprocessid" runat="server" />
                        <asp:hiddenField id ="hdnProviderFeedProcessId" runat="server" />
                        <asp:hiddenField id ="hdnNotes" runat="server" />
                        
                    </div>
                </div>
            </div>
        </div>
    </asp:panel>
    <asp:Button runat="server" ID="btnProvFeedDummy" aria-Label="dummybutton" Style="display: none" />
</div>
