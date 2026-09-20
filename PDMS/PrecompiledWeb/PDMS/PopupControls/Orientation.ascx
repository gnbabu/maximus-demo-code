<%@ control language="C#" autoeventwireup="true" inherits="Pages_Orientation, App_Web_wenzyumt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/OrientationInfo.ascx" TagPrefix="uc" TagName="OrgInfo" %>
<%@ Register Src="~/PopupControls/OrgInfoHistory.ascx" TagPrefix="uc" TagName="OrgInfoHistory" %>

<style type="text/css">
    .divGrid
    {
        width: 100%;
    }
    .gridview
    {
        float: right;
    }
    .gridViewHeader>th>a
    {
        color:White!important;
    }
    .panelOwnerInfo
    {
        padding: 20px 20px 0px 20px;
    }
</style>

<uc1:Separator ID="Separator5" runat="server" Header="Orientation Session Summary" />
<div id="OrgInfo">
    <br />
    <div class="divGrid">
        <asp:GridView runat="server" Width="98%" ID="grdOrientationInfo" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No organization information found." OnRowCommand="grd_RowCommand" 
            OnRowDataBound="grdOrientationInfo_RowDataBound">
            <Columns>
                <asp:BoundField DataField="PROVIDER_NAME" HeaderText="Name" />
                <asp:BoundField DataField="ORIENTATION_DUE_DATE" HeaderText="Due By" DataFormatString="{0:MM/dd/yyyy}"  />
                <asp:BoundField DataField="ORIENTATION_COMPLETED_DATE" HeaderText="Date Completed" DataFormatString="{0:MM/dd/yyyy}"  />
                <asp:BoundField DataField="ORIENTATION_RESPONSE_DATE" HeaderText="Provider Response Date" DataFormatString="{0:MM/dd/yyyy}"  />
                <asp:BoundField DataField="ORIENTATION_STATUS_TYPE" HeaderText="Status" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ToolTip="Edit" ID="btnEdit" runat="server" CommandName="OrientationInfo" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/Images/edit.png" />
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
    <div class="divHistoryAndAdd">
        <asp:ImageButton ToolTip="Add" ID="btnAddOrientationInfo" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OrientationInfo" />
        <asp:ImageButton ToolTip="History" ID="btnHistoryOrientationInfo" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="OrientationInfo" />
    </div>
</div>
<br />

<cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
    CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" style="display:none;width:50%;height:auto;">
    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" >
        <div class="popTitle">
            <asp:Label ID="lblTitle"  runat="server" Text="Title"  />
        </div>
    </asp:Panel>  
     <asp:Panel ID="pnlMain" runat="server" Style="margin-right:10px" DefaultButton="btnSave">
        <asp:MultiView ID="mltPopup" runat="server">
            <asp:View ID="vwOrientationInfo" runat="server">
                   <div style="text-align: left; padding: 15px; margin-left:30px !important" class="container-fluid">
                 <div class="row">
                <uc:OrgInfo ID="ucOrientationInfo" runat="server" />
                </div>
                </div>
            </asp:View>
            <asp:View ID="vwOrientationInfoHistory" runat="server">
                   <div style="text-align: left; padding: 15px; margin-left:30px !important" class="container-fluid">
                 <div class="row">
                <uc:OrgInfoHistory ID="ucOrientationInfoHistory" runat="server" />
                     </div></div>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
    <table border="0" cellpadding="0" cellspacing="5" align="center" style="padding-bottom: 10px">
        <tr>
            <td>
                <asp:Button id="btnSave"  runat="server" Text="Save" CssClass="buttonBoxFocus" onclick="btnSave_Click" CausesValidation="true" />
            </td>
            <td>
                <asp:Button id="btnCancel"  runat="server" Text="Cancel" CssClass="buttonBox" 
                    CausesValidation="false" />
            </td>
        </tr>
    </table> 
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />

