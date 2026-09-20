<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_EmailHistory" Codebehind="EmailHistory.ascx.cs" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="wc" %>



<div style="text-align: center;">
    <wc:GroupBox ID="EmailHistoryHeaderGroup" Caption="Mass Email History" CaptionStyle-CssClass="bodyTextBold" runat="server">

        <asp:Panel runat="server" ID="pnlHistorySearch">
            <div>
                <table class="grid contentInput" role="presentation">
                    <colgroup>
                        <col style="width: 50%" />
                        <col style="width: 50%" />
                    </colgroup>

                    <tr>
                        <td colspan="2">
                            <asp:Button CssClass="buttonBox" runat="server" ID="btnRefresh" Text="Refresh" OnClick="btnRefresh_Click" /></td>
                    </tr>
                </table>

            </div>
            <asp:GridView runat="server" ID="grdHistory" AllowPaging="True" AllowSorting="True" CssClass="RadGrid_PDMSModern"
                          AutoGenerateColumns="false" CellPadding="3" OnPageIndexChanging="grdHistory_PageIndexChanging">

                <HeaderStyle Font-Size="10pt" />
                <PagerStyle Font-Size="10pt" />
                <RowStyle Font-Size="10pt" ForeColor="Black" />
                <Columns>
                    <asp:BoundField DataField="EmailBatchId" HeaderText="Id" />
                    <asp:BoundField DataField="TotalRecords" HeaderText="Count"  />
                    <asp:BoundField DataField="TemplateId" HeaderText="Template"  />
                    <asp:BoundField DataField="EmailSubject" HeaderText="Subject"  />
                    <asp:BoundField DataField="CreateDateTime" HeaderText="Create Date"  />
                    <asp:BoundField DataField="EmailSendDateTime" HeaderText="Sent Date"  />
                    <asp:BoundField DataField="JobComplete" HeaderText="Is Complete"  />
                </Columns>
            </asp:GridView>

        </asp:Panel>





    </wc:GroupBox>

</div>
