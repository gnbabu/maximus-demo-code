<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_CredentialActivities" Codebehind="CredentialActivities.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/AdverseActionList.ascx" TagName="AdverseActionList" TagPrefix="uc" %>

<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc" %>
<style type="text/css">
    #selectOpt{
        width:100px !important;
        max-width:100px;
    }
     #selectOpt option{
        width:100px;
    }
    .dropdownsmall {
          width:200px !important;
        max-width:250px;
        font-size:medium;
        overflow:hidden;
    }
</style>



<div id="divScreeningDetails" style="display:inline-block;width:100%;">
    <br />
    <asp:Label runat="server" ID="lblName" CssClass="formFieldDisplay wdAll" />
    <%--<br />
    <div class="row alignCenterBottom">
        <div class="col-sm-3 text-right"><span class="formLabelAuto"><asp:Label id="lblstartdate" text="Screening Start:" runat="server" /></span></div>
        <div class="col-sm-3 text-left"><span runat="server" id="spnScreeningStart" class="formFieldDisplay"></span></div>
        <div class="col-sm-3 text-right"><span class="formLabelAuto"><asp:Label id="lblenddate" text="Screening End:" runat="server" /></span></div>
        <div class="col-sm-3 text-left"><span runat="server" id="spnScreeningEnd" class="formFieldDisplay"></span></div>
    </div>--%>
    <div class="divGrid"  style="display:block;">
        <asp:GridView runat="server" Width="98%" ID="grdCredentialDetails" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No Credential details found." DataKeyNames="CREDENTIAL_ACTIVITY_ID, ACTIVITY_TYPE_ID,DATARANK_TYPE_ID" 
            OnRowDataBound="grdCredentialDetails_RowDataBound" OnSelectedIndexChanged="grdCredentialDetails_SelectedIndexChanged">
            <Columns>
                <asp:TemplateField ItemStyle-Width="18">
                    <ItemTemplate>
                        <asp:Image runat="server" ID="imgStatus" alt="Status" Width="16" Height="16" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CREDENTIAL_ACTIVITY_ID" HeaderText="Activity ID" Visible="false" />
                <asp:BoundField DataField="ACTIVITY_TYPE_ID" HeaderText="Activity Type" Visible="false" />
                <asp:BoundField DataField="DATARANK_TYPE_ID" HeaderText="Activity Rank" Visible="false" />
                <asp:BoundField DataField="SCREENING_ACTIVITY_TYPE_NAME" HeaderText="Credential Verification" ItemStyle-Wrap="true" ItemStyle-Width="300" />
                <asp:TemplateField HeaderText="Data Rank" ItemStyle-Width="100" ItemStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:LinkButton ToolTip="Select" ID="btnSelectActivity" runat="server" CommandName="Select" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' 
                            Text='<%# Eval("DATARANKNAME") %>' Width="20" />
                        
                    </ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Notes" ItemStyle-Wrap="true" ItemStyle-Width="300">
                    <ItemTemplate>
                       <asp:Label ID="lblNotes" runat="server" Text='<%# Eval("NOTES") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ORIGINAL_EFFECTIVE_DATE" HeaderText="Original Effective/Issue Date" DataFormatString="{0:MM/dd/yy}" ItemStyle-Width="100" ItemStyle-Wrap="true" />
                <asp:BoundField DataField="RENEWAL_DATE" HeaderText="Renewal Date" DataFormatString="{0:MM/dd/yy}" ItemStyle-Width="100" ItemStyle-Wrap="true" />
                <asp:BoundField DataField="EXPIRATION_DATE" HeaderText="Expiration Date" DataFormatString="{0:MM/dd/yy}" ItemStyle-Width="100" ItemStyle-Wrap="true" />
                <asp:BoundField DataField="VERIFICATION_SOURCE_DISPLAYNAME" HeaderText="Verification Source Used" ItemStyle-Wrap="true" ItemStyle-Width="100"  />
                <asp:BoundField DataField="VERIFICATION_DATE" HeaderText="Verification Date" DataFormatString="{0:MM/dd/yy}" ItemStyle-Width="100" ItemStyle-Wrap="true" />
                <asp:BoundField DataField="LAST_ACTION_DATE" HeaderText="Last Action Date" DataFormatString="{0:MM/dd/yy}" ItemStyle-Width="100" ItemStyle-Wrap="true" />
                <asp:BoundField DataField="VERIFIED_BY" HeaderText="Verified By" ItemStyle-Wrap="true" ItemStyle-Width="100"  />
               
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
</div>

<cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
    CancelControlID="btnClose" BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlHeader">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="identModalPopup" Width="720" align="center" style="display:none">
    <asp:Panel ID="pnlHeader" CssClass="popupPanelHeader" runat="server" >
        <div>&nbsp;&nbsp;
            <asp:Label ID="Label2" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White">Adverse Action - OIG Exclusion</asp:Label>
        </div>
    </asp:Panel>  
    <div style="padding: 5px;">
        <uc:AdverseActionList runat="server" id="ucAdverseActionList" />
        <br />
        <br />
        <asp:Button runat="server" ID="btnClose" CssClass="buttonBox" Text="Close" />
    </div>
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy"/>