<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ScreeningGroupAffiliations, App_Web_yvhxe4ml" %>
<%@ Register Src="~/PopupControls/ScreeningHistory.ascx" TagName="ScreeningHistory" TagPrefix="uc" %>
<div id="divScreeningHistory" class="" style="display:inline-block;width:100%;">
    <br />
    <div class="divGrid">
    <asp:GridView runat="server" Width="100%" ID="grdGroupAffiliations" DataKeyNames="REG_AFFILIATION_ID,SCREENING_ID,START_DATE_TIME,END_DATE_TIME,NAME,SCREENING_STATUS_ID" AutoGenerateColumns="False" 
        HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No group affiliations found." AllowSorting="false" 
        OnSelectedIndexChanged="grdGroupAffiliations_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="REG_AFFILIATION_ID" HeaderText="REG_AFFILIATION_ID" Visible="false" />
            <asp:BoundField DataField="WORKFLOW_LONG_NAME" HeaderText="Screening Type" />
            <asp:BoundField DataField="NAME" HeaderText="Name" SortExpression="NAME" />
            <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
            <asp:BoundField DataField="ProviderTypeName" HeaderText="Provider Type" SortExpression="ProviderTypeName" />
            <asp:BoundField DataField="PROVIDER_RISK_LEVEL_NAME" HeaderText="Risk Level" SortExpression="PROVIDER_RISK_LEVEL_NAME" />
            <asp:BoundField DataField="PDMSStatus" HeaderText="PDMS Status" SortExpression="PDMSStatus" />
            <asp:BoundField DataField="DESCRIPTION" HeaderText="Affiliation Status" SortExpression="DESCRIPTION" />
            <asp:BoundField DataField="START_DATE_TIME" HeaderText="Screening Start" DataFormatString="{0:MM/dd/yy}" />
            <asp:BoundField DataField="END_DATE_TIME" HeaderText="Screening End" DataFormatString="{0:MM/dd/yy}" />
            <asp:TemplateField HeaderText="Screening Status" SortExpression="SCREENING_STATUS_NAME">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnGASelect" CommandName="Select" CommandArgument='<%# Eval("SCREENING_ID") %>' Text='<%# Eval("SCREENING_STATUS_NAME") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="SCREENING_RESULT_NAME" HeaderText="Screening Result" SortExpression="SCREENING_RESULT_NAME" />
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
    </div>
</div>