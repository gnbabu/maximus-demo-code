<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ScreeningHouseholdMembers" Codebehind="ScreeningHouseholdMembers.ascx.cs" %>
<%@ Register Src="~/PopupControls/ScreeningHistory.ascx" TagName="ScreeningHistory" TagPrefix="uc" %>
<div id="divScreeningHistory" class="" style="display:inline-block;width:100%;">
    <br />
    <div class="divGrid">
    <asp:GridView runat="server" Width="100%" ID="gvHHMembers" 
        DataKeyNames="REG_HOUSEHOLD_MEMBER_ID,SCREENING_ID,START_DATE_TIME,END_DATE_TIME,NAME,SCREENING_STATUS_ID" 
        AutoGenerateColumns="False" 
        HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No household members found." AllowSorting="false" 
        OnSelectedIndexChanged="gvHHMembers_SelectedIndexChanged">
        <Columns>
            <asp:BoundField DataField="REG_HOUSEHOLD_MEMBER_ID" HeaderText="REG_HOUSEHOLD_MEMBER_ID" Visible="false" />
            <asp:BoundField DataField="WORKFLOW_LONG_NAME" HeaderText="Screening Type" />
            <asp:BoundField DataField="NAME" HeaderText="Member Name" />
            <asp:BoundField DataField="RELATIONSHIP" HeaderText="Household Status" />
            <asp:BoundField DataField="START_DATE_TIME" HeaderText="Screening Start" DataFormatString="{0:MM/dd/yy}" />
            <asp:BoundField DataField="END_DATE_TIME" HeaderText="Screening End" DataFormatString="{0:MM/dd/yy}" />
            <asp:TemplateField HeaderText="Screening Status">
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