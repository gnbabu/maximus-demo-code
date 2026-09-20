<%@ Control Language="C#" AutoEventWireup="true" Inherits="Pages_MMISTransactions" Codebehind="MMISTransactions.ascx.cs" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
                                    <table role="presentation"  role="presentation" >
                                        <tr>
                                             <td><span class="formLabel" style="width:auto">NPI:</span></td>
                                            <td style="text-align:left"><asp:Label ID="lblNPI" runat="server" /></td>
                                            <td><span class="formLabel" style="width:auto">Tax ID:</span></td>
                                    <td style="text-align:left"> <asp:Label ID="lblTaxId" runat="server" /> </td>
                                        </tr>
</table>
<br />
<uc1:separator id="ucMMIS" runat="server" header="Recent MMIS Transactions" />
<asp:GridView runat="server" Width="100%" ID="grdHistory"  AutoGenerateColumns="true" HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No event history found." AllowPaging="true" PageSize="20" OnPageIndexChanging="grdHistory_PageIndexChanging">
    <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />  
    <HeaderStyle CssClass="gridViewHeader" Width="100px" />
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <RowStyle CssClass="gridViewRow" /> 
    <FooterStyle CssClass="gridViewFooter" />
</asp:GridView>