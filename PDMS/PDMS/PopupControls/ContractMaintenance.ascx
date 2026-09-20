
<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ContractMaintenance" Codebehind="ContractMaintenance.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<div class="container">
    <div  ID="CMHeaderConvertedHistoricalData" runat="server">
        <div class ="pageHeader">
            <h2 style="text-align: center; font-weight: bold" > Converted Historical Contract Data</h2>
        </div>
        <div>
            <p class ="pageHeader3">All dates and status in the grid below are a picture in time at PNM Conversion from MITS.</p>
        </div>
    </div>
    <div  class ="pageHeader3"><span>NOTE: Any contract changes on or after 7/1/2022 will not display on this page </span></div>
   <div class="divGrid">
    <asp:GridView ID="ContractMaintananceGrid" runat="server"  Width="95%" AutoGenerateColumns="False" OnDataBound="OnDataBound" EmptyDataText="No Contract Maintenance details available." DataKeyNames="REG_CONTRACT_ID">
        <Columns>
            <asp:BoundField DataField="CONTRACT_TYPE_NAME" HeaderText="Contract Name"
                SortExpression="Contract Status" />
            <asp:BoundField DataField="CONTRACT_STATUS_NAME" HeaderText="Contract Status" SortExpression="Contract Status"></asp:BoundField>
            <asp:BoundField DataField="ENROLLMENT_STATUS_REASONS_DESC" HeaderText="Status Reason" SortExpression="Status Reason"></asp:BoundField>
            <asp:BoundField DataField="CONTRACT_START_DATE" HeaderText="Start Date" SortExpression="Start Date"></asp:BoundField>
            <asp:BoundField DataField="CONTRACT_END_DATE" HeaderText="End Date" SortExpression="End Date"></asp:BoundField>
            <%-- <asp:TemplateField>
            <ItemTemplate>
                <asp:CheckBox ID="chkRow" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>--%>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>
    </div>
    <br />
    <br />

</div>
