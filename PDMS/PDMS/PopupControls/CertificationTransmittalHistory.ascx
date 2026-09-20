<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_CertificationTransmittalHistory" Codebehind="CertificationTransmittalHistory.ascx.cs" %>

<br />
<div style="padding-left: 20px">
    <asp:GridView runat="server" Width="98%" ID="grdCertificationTransmittalHistory" AutoGenerateColumns="False" HorizontalAlign="Left" 
        CssClass="gridview" EmptyDataText="No entries found.">
        <Columns>
            <asp:BoundField DataField="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" HtmlEncode="False"
                HeaderText="Last Modified Date" />
            <asp:BoundField DataField="SURVEY_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False"
                HeaderText="Survey Date" />
            <asp:BoundField DataField="CERTIFICATION_EFFECTIVE_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False"
                HeaderText="Cert Eff Date" />
            <asp:BoundField DataField="CERTIFICATION_END_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False"
                HeaderText="Cert End Date" />
            <asp:BoundField DataField="UserName" HeaderText="Last Modified User" />
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
</div>