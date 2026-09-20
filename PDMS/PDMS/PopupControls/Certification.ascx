<%@ Control Language="C#" AutoEventWireup="true" Inherits="Pages_Certification" Codebehind="Certification.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/CertificationTransmittal.ascx" TagPrefix="uc" TagName="CertificationTransmittal" %>
<%@ Register Src="~/PopupControls/CertificationTransmittalHistory.ascx" TagPrefix="uc" TagName="CertificationTransmittalHistory" %>

<uc1:Separator ID="Separator5" runat="server" Header="Certification Transmittals" />
<div>
    <br />
    <div class="divGrid">
        <asp:GridView runat="server" Width="98%" ID="grdCertification" AutoGenerateColumns="False" HorizontalAlign="Left" 
            CssClass="gridview" EmptyDataText="No Certification Transmittal information found." OnRowCommand="grd_RowCommand">
            <Columns>
                <asp:BoundField DataField="SURVEY_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False"
                    HeaderText="Survey Date" />
                <asp:BoundField DataField="CERTIFICATION_EFFECTIVE_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False"
                    HeaderText="Cert Eff Date" />
                <asp:BoundField DataField="CERTIFICATION_END_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False"
                    HeaderText="Cert End Date" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ToolTip="Edit" ID="btnEdit" runat="server" CommandName="Certification" 
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/Images/edit.png" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    <div class="divHistoryAndAdd">
        <asp:ImageButton ToolTip="Add" ID="btnAddCertification" runat="server" ImageUrl="~/Images/add.png" 
            OnCommand="btnAdd_Click" CommandName="Certification" Visible="true" />
        <asp:ImageButton ToolTip="History" ID="btnHistoryCertification" runat="server" ImageUrl="~/Images/history_icon.jpg" 
            OnCommand="btnHistory_Click" CommandName="Certification" />
    </div>
</div>
<br />

<style type="text/css">
    .identModalBackground
    {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.2;
        height: auto;
    }
    .identModalPopup
    {
        background-color: #FFFFFF;
        border-width: 1px;
        border-style: solid;
        border-color: black;
        padding: 0px;
        width: auto;
        height: auto;
    }
</style>

<!-- ModalPopupExtender -->
<cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
    CancelControlID="btnCancel" BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlModal">
</cc1:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="identModalPopup" align="center" style="display:none">
    <asp:Panel ID="pnlHeader" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
        <div align="left">&nbsp;&nbsp;
            <asp:Label ID="lblTitle" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
        </div>
    </asp:Panel>  
    <asp:Panel ID="pnlMain" runat="server" Style="margin-right:10px" DefaultButton="btnSave">
        <asp:MultiView ID="mltPopup" runat="server">
            <asp:View ID="vwCertification" runat="server">
                <uc:CertificationTransmittal ID="ucCertification" runat="server" />
            </asp:View>
            <asp:View ID="vwCertificationHistory" runat="server">
                <uc:CertificationTransmittalHistory ID="ucCertificationHistory" runat="server" />
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
    <table border="0" cellpadding="0" cellspacing="5" align="center" style="padding-bottom: 10px">
        <tr>
            <td>
                <asp:Button id="btnSave"  runat="server" Text="Save" CssClass="buttonBox" onclick="btnSave_Click" CausesValidation="true" />
            </td>
            <td>
                <asp:Button id="btnCancel"  runat="server" Text="Cancel" CssClass="buttonBox" 
                    CausesValidation="false" />
            </td>
        </tr>
    </table> 
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy"/>
