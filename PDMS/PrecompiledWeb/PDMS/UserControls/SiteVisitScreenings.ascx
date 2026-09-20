<%@ control language="C#" autoeventwireup="true" inherits="UserControls_SiteVisitScreenings, App_Web_p4ixifjm" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc" %>
<div style="display:inline-block;width:100%;">
    <uc:Separator runat="server" Header="Site Visit Screening Summary" />
    <br />
    <div class="divGrid"  style="display:block;">
        <asp:GridView runat="server" Width="98%" ID="grdSiteVisitScreenings" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No site visit screenings found." DataKeyNames="SITE_VISIT_SCREENING_ID,SCREENING_ACTIVITY_ID" 
            OnSelectedIndexChanged="grdSiteVisitScreenings_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="SITE_VISIT_SCREENING_TYPE_NAME" HeaderText="Site Visit Screening Type" />
                <asp:BoundField DataField="START_DATE" HeaderText="Start Date" DataFormatString="{0:MM/dd/yy}" />
                <asp:BoundField DataField="END_DATE" HeaderText="End Date" DataFormatString="{0:MM/dd/yy}" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:LinkButton ToolTip="Select" ID="btnSelectSiteVisitScreening" runat="server" CommandName="Select" 
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' Text='<%# Eval("SITE_VISIT_SCREENING_STATUS_NAME") %>' />
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