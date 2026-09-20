<%@ control language="C#" autoeventwireup="true" inherits="UserControls_SiteVisitAttempts, App_Web_p4ixifjm" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc" %>
<div style="display:inline-block;width:100%;">
    <uc:Separator runat="server" Header="Site Visit Attempts" Mode="1" />
    <br />
    <div class="divGrid"  style="display:block;">
        <asp:GridView runat="server" Width="98%" ID="grdSiteVisitAttempts" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No site visit attempts found." DataKeyNames="SITE_VISIT_ID, SITE_VISIT_ATTEMPT_ID, REQUIRED_DATE" 
            OnSelectedIndexChanged="grdSiteVisitAttempts_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="PERFORMED_DATE" HeaderText="Date Performed" DataFormatString="{0:MM/dd/yy}" />
                <asp:BoundField DataField="PERFORMED_BY_USERNAME" HeaderText="Performed By" />
                <asp:BoundField DataField="SITE_VISIT_RECOMMENDATION_NAME" HeaderText="Recommendation" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:LinkButton ToolTip="Select" ID="btnSelectSiteVisitAttempt" runat="server" CommandName="Select" 
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' Text='<%# Eval("SITE_VISIT_STATUS_NAME") %>' />
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