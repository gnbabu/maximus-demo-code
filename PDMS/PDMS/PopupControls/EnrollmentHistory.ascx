<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_EnrollmentHistory" Codebehind="EnrollmentHistory.ascx.cs" %>
<asp:GridView runat="server" Width="98%" ID="grd" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No History found - Current Span is displayed on Provider Information Page.">
    <Columns>
        <asp:BoundField DataField="ENROLL_START_DATE_TIME" HeaderText="Effective Date"  DataFormatString="{0:d}"/>
        <asp:BoundField DataField="ENROLL_END_DATE_TIME" HeaderText="End Date"  DataFormatString="{0:d}"/>
        <asp:BoundField DataField="Enrollment_Status_code_description" HeaderText="Enrollment Status" />

    </Columns>
    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" />
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>
