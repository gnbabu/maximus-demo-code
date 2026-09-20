<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_MalpracticeClaimHistory" Codebehind="MalpracticeClaimHistory.ascx.cs" %>
<asp:GridView runat="server" Width="100%" ID="grdMalPracticeClaimHistory" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found.">
    <Columns>
         <asp:BoundField DataField="DateOccurance" HeaderText="Date of Occurrence" DataFormatString="{0:d}" />
         <asp:BoundField DataField="Claim_Status_Name" HeaderText="Status Of Claim" />
         <asp:BoundField DataField="DateCLAIMFiled" HeaderText="Date Claim Filed" DataFormatString="{0:d}" />
         <asp:TemplateField HeaderText="Date Claim Settled">
            <ItemTemplate>
                <asp:Label ID="lblClaimSettleDate" runat="server" Text='<%# Helper.GetDisplayFormatDate(Eval("CLAIM_SETTLED_DATE")) %>' />
            </ItemTemplate>
         </asp:TemplateField>
         <asp:BoundField DataField="RESOLUTION_DESC" HeaderText="Method of Resolution" />
         <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
         <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" SortExpression="LAST_MODIFIED_DATE_TIME" HeaderText="Update Date" />
    </Columns>
    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>