<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ScreeningActivities" Codebehind="ScreeningActivities.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/AdverseActionList.ascx" TagName="AdverseActionList" TagPrefix="uc" %>
<div id="divScreeningDetails" style="display:inline-block;width:100%;">
    <br />
    <asp:Label runat="server" ID="lblName" CssClass="formFieldDisplay wdAll" />
    <br />
    <div class="row alignCenterBottom">
        <div class="col-sm-3 text-right"><span class="formLabelAuto">Screening Start:</span></div>
        <div class="col-sm-3 text-left"><span runat="server" id="spnScreeningStart" class="formFieldDisplay"></span></div>
        <div class="col-sm-3 text-right"><span class="formLabelAuto">Screening End:</span></div>
        <div class="col-sm-3 text-left"><span runat="server" id="spnScreeningEnd" class="formFieldDisplay"></span></div>
    </div>
    <div class="divGrid"  style="display:block;">
        <asp:GridView runat="server" Width="98%" ID="grdScreeningDetails" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No screening details found." DataKeyNames="SCREENING_ACTIVITY_ID,SCREENING_ACTIVITY_TYPE_ID,SCREENING_ACTIVITY_STATUS_ID" 
            OnRowDataBound="grdScreeningDetails_RowDataBound" OnSelectedIndexChanged="grdScreeningDetails_SelectedIndexChanged">
            <Columns>
                <asp:TemplateField ItemStyle-Width="18"  HeaderText="<span style='display:none'>Edit</span>">
                    <ItemTemplate>
                        <asp:Image runat="server" ID="imgStatus" alt="Status" Width="16" Height="16" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="SCREENING_ACTIVITY_TYPE_NAME" HeaderText="Activity" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:LinkButton ToolTip="Select" ID="btnSelectActivity" runat="server" CommandName="Select" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' 
                            Text='<%# Eval("SCREENING_ACTIVITY_STATUS_NAME") %>' />
                        <asp:Label ToolTip="Select" ID="lblActivityStatus" runat="server" Text='<%# Eval("SCREENING_ACTIVITY_STATUS_NAME") %>' Visible="false" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="SCREENING_METHOD_NAME" HeaderText="Method" />
                <asp:BoundField DataField="LAST_ACTION_DATE_TIME" HeaderText="Last Action" DataFormatString="{0:MM/dd/yy}" />
                <asp:TemplateField HeaderText="Notes">
                    <ItemTemplate>
                        <asp:ImageButton runat="server" ID="lnkNotes" ImageUrl="~/Images/notes.jpg" Width="16" Height="16" OnCommand="lnkNotes_Command" CommandArgument='<%# Eval("SCREENING_ACTIVITY_ID") %>'
                            Visible='<%# (bool)Eval("HAS_ADVERSE_ACTION") %>' />
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
</div>
<style type="text/css">
    .identModalPopup
    {
        background-color: #FFFFFF;
        border-width: 1px;
        border-style: solid;
        border-color: black;
        padding: 0px;
        width: auto !important;
        height: auto;
        top: 50% !important;
    }

</style>

<cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
    CancelControlID="btnClose" BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlHeader">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="identModalPopup" align="center" style="display:none">
    <asp:Panel ID="pnlHeader" CssClass="popupPanelHeader" runat="server" >
        <div>&nbsp;&nbsp;
            <asp:Label ID="Label2" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White">Screening Notes</asp:Label>
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