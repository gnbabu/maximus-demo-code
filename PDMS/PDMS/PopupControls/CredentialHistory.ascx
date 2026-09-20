<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_CredentialHistory" Codebehind="CredentialHistory.ascx.cs" %>
<asp:Panel ID="pnlPendingVerification" runat="server" Visible="false">
  <div class="btnBox">
    <asp:CheckBox ID="chkPendingVerification" runat="server" Text ="Pending Verification" CssClass="ChkBoxClass" OnCheckedChanged="chkPendingVerification_CheckedChanged" AutoPostBack="true" />
  </div>
</asp:Panel>
<div id="divScreeningHistory" style="display:inline-block;width:100%;">
    <br />
    <div class="divGrid">
        <asp:GridView runat="server" Width="98%" ID="grdCredentialHistory" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No Credential history found." DataKeyNames="credentialing_id" 
            OnSelectedIndexChanged="grdCredentialHistory_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="NAME" HeaderText="Credentialing Type" />
                <asp:BoundField DataField="START_DATE_TIME" HeaderText="Credentialing Start" DataFormatString="{0:MM/dd/yy}" />
                <asp:BoundField DataField="END_DATE_TIME" HeaderText="Credentialing End" DataFormatString="{0:MM/dd/yy}" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:LinkButton ToolTip="Select" ID="btnSelect" runat="server" CommandName="Select" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' 
                            Text='<%# Eval("CREDENTIALING_STATUS_NAME") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CREDENTIALING_RESULT_NAME" HeaderText="Result" />
                <asp:BoundField DataField="RiskLevelName" HeaderText="Credentialing Risk Level" />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
</div>