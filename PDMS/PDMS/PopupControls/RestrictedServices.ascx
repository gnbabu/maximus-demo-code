<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_RestrictedServices" Codebehind="RestrictedServices.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/RestrictedService.ascx" TagPrefix="uc" TagName="RestrictedService" %>

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

    function removeDisabled() {
        $("#<%= btnCloseHistory.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    }  
</script>

<asp:panel id="upRSHistory" runat="server" style="min-width: 1200px; position: fixed; z-index: 2; left: 200px; top: 50px;">
    <cc1:modalpopupextender id="mpeRSHistory" runat="server" popupcontrolid="pRSHistory" targetcontrolid="ButtonDummy3"
        backgroundcssclass="modalBackground" popupdraghandlecontrolid="pRSHistory" CancelControlID="btnCloseHistory">
    </cc1:modalpopupextender>
    <asp:panel id="pRSHistory" runat="server" cssclass="modalPopup" style="display: none; padding: 20px; min-width: 1200px;">
        <div>
             <asp:panel id="pnlRSHistoryDetails" runat="server">
                <div>
                    <asp:GridView runat="server" Width="98%" ID="grd" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found."
                        AllowPaging="true" AllowSorting="True" PageSize="10"
                        OnPageIndexChanging="grd_PageIndexChanging" OnSorting="grd_Sorting">
                        <Columns>
                            <asp:BoundField DataField="OPERATION"           HeaderText="Operation"          SortExpression="Operation" />
                            <asp:BoundField DataField="IS_EXCLUDE"          HeaderText="Exclude/Include"    SortExpression="IS_EXCLUDE" />
                            <asp:BoundField DataField="REVIEWTYPE"          HeaderText="Review Type"        SortExpression="REVIEWTYPE" />
                            <asp:BoundField DataField="REVIEWREASON"        HeaderText="Review Reason"      SortExpression="REVIEWREASON" />
                            <asp:BoundField DataField="IS_RESTRICT"         HeaderText="Restrict"           SortExpression="IS_RESTRICT" />
                            <asp:BoundField DataField="EFFECTIVE_DATE"      HeaderText="Effective Date"     SortExpression="EFFECTIVE_DATE" DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:BoundField DataField="END_DATE"            HeaderText="End Date"           SortExpression="END_DATE" DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:BoundField DataField="STATUS"              HeaderText="Status"             SortExpression="STATUS" />
                            <asp:BoundField DataField="UserName" HeaderText="LastModifiedUser" SortExpression="UserName" />
                            <asp:BoundField DataField="DateOfAction" HeaderText="LastModifiedDateTime" SortExpression="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />
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
            <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkExcel_Click" />
        </div>
        <asp:button runat="server" id="ButtonDummy3" style="display: none" text="”ButtonDummy3”" />
    </asp:panel>
</asp:panel>
<asp:UpdatePanel ID="upRS_Main" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="AffiliationInfo">
            <div class="divGrid">
                <mms:SortablePagingGridView runat="server" Width="100%" ID="grdRestrcitedServices"
                    AutoGenerateSelectButton="false" AutoGenerateColumns="False" 
                    CssClass="gridViewSmallFont" PageSize="10"  
                    EmptyDataText="No Restricted Services found." 
                    GridViewSortColumn="REG_RESTRICTION_ID,IS_EXCLUDE,STATUS_CODE,EFFECTIVE_DATE,END_DATE,IS_RESTRICT,REVIEW_TYPE_ID,REVIEW_REASON_ID,SENT_TO_SI" GridViewSortDirection="Ascending" AllowPaging="true" 
                    ShowHeaderWhenEmpty="true" 
                    DataKeyNames="REG_RESTRICTION_ID,REG_ID"
                    OnSorting="grdRestrcitedServices_Sorting" OnRowDataBound="grdRestrcitedServices_RowDataBound"
                    OnRowCommand="grdRestrcitedServices_RowCommand" OnPageIndexChanging="grdRestrcitedServices_PageIndexChanging">
                    <Columns>
                        
                        <asp:BoundField DataField="REVIEWTYPE" HeaderText="Review Type" SortExpression="REVIEWTYPE"  />
                        <asp:BoundField DataField="REVIEWREASON" HeaderText="Review Reason" SortExpression="REVIEWREASON"/>
                        <asp:BoundField DataField="STATUS" HeaderText="Status" SortExpression="STATUS" />
                        <asp:BoundField DataField="EFFECTIVE_DATE" HeaderText="Effective Date" SortExpression="EFFECTIVE_DATE" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="END_DATE" HeaderText="End Date" SortExpression="END_DATE" DataFormatString="{0:d}" />
                        
                        <asp:TemplateField Headertext="Edit" >
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="RestrictedService" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    ToolTip="Edit">
                                    <asp:Image ID="imgEdit" ImageUrl="~/Images/edit.png" runat="server" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Headertext="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteRestrictedService" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                    ToolTip="Delete Restricted Service" OnClientClick="return confirm('Are you sure you want to delete?');">
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
                </mms:SortablePagingGridView >
            </div>
          
            <div class="divHistoryAndAdd" style="vertical-align: middle;">
                <asp:ImageButton ID="btnAdd" runat="server" ImageUrl="~/Images/add.png" CommandName="RestrictedService" OnCommand="btnAdd_Click" ToolTip="Add" Style="vertical-align: middle"/>
                <asp:LinkButton ID="btnHistory" runat="server" AlternateText="History" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" ToolTip="History" Text="History" Style="vertical-align: middle; color: white; text-decoration: none; padding: 4px;" />
            </div>
        </div>
        <br />
      
        <div id="divpopupRS">
            <cc1:ModalPopupExtender ID="mpeRS" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy" BackgroundCssClass="modalBackground"  />
            <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 800px; height: auto; width: auto;">
                <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
                    <div class="popTitle">
                        <asp:Label ID="lblTitle" CssClass="bodyTextBold" runat="server" Text="Add Restricted Service" /></div>
                </asp:Panel>
                <asp:MultiView ID="mltPopupRS" runat="server">
                    <asp:View ID="vwRestrictedService" runat="server">
                        <uc:RestrictedService ID="ucRestrictedService" runat="server" EnableViewState="true" />
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </div>
        <asp:button runat="server" id="ButtonDummy" style="display: none" text="”ButtonDummy”" />
    </ContentTemplate>
</asp:UpdatePanel>
<div style="width: 100%; text-align: right; display: none;">
    <telerik:RadGrid ID="grdHistoryExport" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
        AutoGenerateColumns="true" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
            <Excel Format="Biff" />
        </ExportSettings>
        <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
            DataKeyNames="INDEX, DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
            <Columns>
                <telerik:GridBoundColumn DataField="OPERATION"           HeaderText="Operation"          SortExpression="Operation" />
                <telerik:GridBoundColumn DataField="IS_EXCLUDE"          HeaderText="Exclude/Include"    SortExpression="IS_EXCLUDE" />
                <telerik:GridBoundColumn DataField="REVIEWTYPE"          HeaderText="Review Type"        SortExpression="REVIEWTYPE" />
                <telerik:GridBoundColumn DataField="REVIEWREASON"        HeaderText="Review Reason"      SortExpression="REVIEWREASON" />
                <telerik:GridBoundColumn DataField="IS_RESTRICT"         HeaderText="Restrict"           SortExpression="IS_RESTRICT" />
                <telerik:GridBoundColumn DataField="EFFECTIVE_DATE"      HeaderText="Effective Date"     SortExpression="EFFECTIVE_DATE" DataFormatString="{0:MM/dd/yyyy}" />
                <telerik:GridBoundColumn DataField="END_DATE"            HeaderText="End Date"           SortExpression="END_DATE" DataFormatString="{0:MM/dd/yyyy}" />
                <telerik:GridBoundColumn DataField="STATUS"              HeaderText="Status"             SortExpression="STATUS" />
                <telerik:GridBoundColumn DataField="UserName" HeaderText="LastModifiedUser" SortExpression="UserName" />
                <telerik:GridBoundColumn DataField="DateOfAction" HeaderText="LastModifiedDateTime" SortExpression="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>
