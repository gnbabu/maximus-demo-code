<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="Process_ResubmitTransaction" Codebehind="ResubmitTransaction.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td style="padding-left:160px;width:800px"><asp:Label ID="lblTitle" runat="server" Text="Resubmit Transaction" /></td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <style type="text/css">
        .gridview td
        {
            white-space:nowrap;
        }
        .Trans
        {
            font-size:8pt!important;
        }
    </style>
    <div class="boxPanel" style="width: 100% !important; height: 635px !important;">
        <div class="boxPanelHeader">Communications</div>
        <div class="boxPanelData" style="height: 600px; overflow-x: scroll; overflow-y: scroll;">
        <table>
            <tr>
                <td><span class="formLabel200">Transaction Type</span></td>
                <td><asp:DropDownList ID="ddlTransactionType" runat="server" CssClass="formDropDown" /></td>
                <td><span class="formLabel200">Process Start Date</span></td>
                <td>
                    <asp:TextBox ID="txtProcessStartDate" runat="server" CssClass="formField" />
                    <ajax:CalendarExtender ID="calProcessStartDate" TargetControlID="txtProcessStartDate" runat="server" /> 
                </td>
            </tr>
            <tr>
                <td><span class="formLabel200">Process End Date</span></td>
                <td>
                    <asp:TextBox ID="txtProcessEndDate" runat="server" CssClass="formField" />
                    <ajax:CalendarExtender ID="calProcessEndDate" TargetControlID="txtProcessEndDate" runat="server" /> 
                </td>
                <td><span class="formLabel200">Party ID</span></td>
                <td>
                    <asp:TextBox ID="txtPartyID" runat="server" CssClass="formField" />
                </td>
            </tr>
            <tr>
                <td><span class="formLabel200">Service Location ID</span></td>
                <td>
                    <asp:TextBox ID="txtServiceLocationID" runat="server" CssClass="formField" />
                </td>
                <td><span class="formLabel200">Medicaid ID</span></td>
                <td>
                    <asp:TextBox ID="txtMedicaidID" runat="server" CssClass="formField" />
                </td>
            </tr>
            <tr>
                <td><span class="formLabel200">CAQH ID</span></td>
                <td>
                    <asp:TextBox ID="txtCaqhID" runat="server" CssClass="formField" />
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="4" align="center"><asp:Button runat="server" ID="btnFilter" Text="Filter" OnClick="btnFilter_Click" /></td>
            </tr>
        </table>
        <br />
            <asp:UpdatePanel ID="upTransactions" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upTransactions">
                    <ProgressTemplate>
                        <h3 style="background-color:Gray; width:100%;">Processing...</h3>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                    <asp:GridView runat="server" Width="100%" ID="gvTrans" AutoGenerateColumns="False" CellSpacing="10" CellPadding="10"
                        OnRowCommand="gvTrans_RowCommand" OnRowDataBound="gvTrans_RowDataBound"
                        DataKeyNames="TransactionQueueId,TransactionTypeId,PartyId,CAQHID,ServiceLocationID"
                        HorizontalAlign="Center" CssClass="gridview" EmptyDataText="No transactions found." ShowHeader="true">
                        <Columns>
                            <%-- 0--%><asp:BoundField DataField="TransactionQueueId" HeaderText="TQID" ItemStyle-Width="150" ItemStyle-CssClass="Trans" />
                            <%-- 1--%><asp:BoundField DataField="TransactionType" HeaderText="TransactionType" ItemStyle-Width="150" ItemStyle-CssClass="Trans" />
                            <%-- 2--%><asp:BoundField DataField="PartyId" HeaderText="PartyId" ItemStyle-Width="50" ItemStyle-CssClass="Trans" />
                            <%-- 3--%><asp:BoundField DataField="PartyType" HeaderText="PartyType" ItemStyle-Width="50" ItemStyle-CssClass="Trans" />
                            <%-- 4--%><asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-Width="100"  ItemStyle-CssClass="Trans" />
                            <%-- 5--%><asp:BoundField DataField="CAQHID" HeaderText="CAQHID" ItemStyle-Width="50"  ItemStyle-CssClass="Trans" />
                            <%-- 6--%><asp:BoundField DataField="ServiceLocationID" HeaderText="ServiceLocationID" ItemStyle-Width="50"  ItemStyle-CssClass="Trans" />
                            <%-- 7--%><asp:BoundField DataField="ServiceLocationType" HeaderText="ServiceLocationType" ItemStyle-Width="50"  ItemStyle-CssClass="Trans" />
                            <%-- 8--%><asp:BoundField DataField="MedicaidId" HeaderText="MedicaidId" ItemStyle-Width="50"  ItemStyle-CssClass="Trans" />
                            <%-- 9--%><asp:BoundField DataField="CreateDateTime" HeaderText="CreateDateTime" ItemStyle-Width="50" DataFormatString="{0:MM/dd/yyyy}"  ItemStyle-CssClass="Trans" />
                            <%--10--%><asp:BoundField DataField="SubmitDateTime" HeaderText="SubmitDateTime" ItemStyle-Width="50" DataFormatString="{0:MM/dd/yyyy}"  ItemStyle-CssClass="Trans" />
                            <%--11--%><asp:BoundField DataField="ProcessDateTime" HeaderText="ProcessDateTime" ItemStyle-Width="50" DataFormatString="{0:MM/dd/yyyy}"  ItemStyle-CssClass="Trans" />
                            <%--12--%><asp:BoundField DataField="CancelDateTime" HeaderText="CancelDateTime" ItemStyle-Width="50" DataFormatString="{0:MM/dd/yyyy}"  ItemStyle-CssClass="Trans" />
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnCancelTransaction" Text="Cancel" runat="server" CommandName="CancelTransaction" CommandArgument='<%# Eval("TransactionQueueId") %>' CssClass="Trans" />
                                    <asp:LinkButton ID="lbtnResubmitTransaction" Text="Resubmit" runat="server" CommandName="ResubmitTransaction" CommandArgument='<%# Eval("TransactionQueueId") %>' CssClass="Trans" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />  
                        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" /> 
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>