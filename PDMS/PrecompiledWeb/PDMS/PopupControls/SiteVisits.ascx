<%@ control language="C#" autoeventwireup="true" inherits="UserControls_SiteVisits, App_Web_av5ll3zk" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc" %>
<div style="display:inline-block;width:100%;">
    <asp:Label ID="lblOrgScrCompleteDate" runat ="server" CssClass="formLabel300" Text="Original Screening Complete Date " />
    <asp:Label ID="lblOrgScrCompleteDate1" runat ="server" CssClass="formFieldDisplayAuto" Text="" />
    <uc:Separator ID="sepSiteVisitSummary" runat="server" Header="Site Visit Summary" />
    <br />
    <div class="divGrid"  style="display:block;">
        <asp:GridView runat="server" Width="98%" ID="grdSiteVisits" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No site visits found." DataKeyNames="SITE_VISIT_ID,SITE_VISIT_ATTEMPT_ID,START_DATE,REQUIRED_DATE,DUE_BY,SITE_VISIT_TYPE_NAME,SITE_VISIT_ATTEMPT_STATUS_ID" 
            OnSelectedIndexChanged="grdSiteVisits_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="Attempt" HeaderText="Attempt" />
                <asp:BoundField DataField="DUE_BY" HeaderText="Due By" DataFormatString="{0:MM/dd/yy}" />
                <asp:BoundField DataField="COMPLETED_DATE" HeaderText="Date Completed" DataFormatString="{0:MM/dd/yy}" />
                <asp:BoundField DataField="SITE_VISIT_RECOMMENDATION_NAME" HeaderText="Recommendation" />
                <asp:BoundField DataField="SITE_VISIT_FINDINGS_NAME" HeaderText="Findings" />
                <asp:BoundField DataField="SITE_VISIT_ATTEMPT_STATUS_NAME" HeaderText="Status" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
</div>
