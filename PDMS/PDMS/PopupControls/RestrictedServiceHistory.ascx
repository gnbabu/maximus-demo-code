<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_RestrictedServiceHistory" Codebehind="RestrictedServiceHistory.ascx.cs" %>
<br />
<asp:UpdatePanel ID="upGrdRestrcitedServceHistory" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    
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
    </ContentTemplate>
</asp:UpdatePanel>

