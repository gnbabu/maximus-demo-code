<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_Notes" Codebehind="Notes.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>



<script type="text/javascript">
    function ShowPanel(pnlId, visible) {
        if (visible) document.getElementById(pnlId).style.display = "block";
        else document.getElementById(pnlId).style.display = "none";
    }
    $(document).ready(function () {
        $("#<%= ddlNoteType.ClientID %>").bind('change', function (ev) {
            //set the value to hiddenfield on change event of dropdownlist
            $("#<%= hdnSelectedNoteType.ClientID %>").val($(this).val());
        });
    });

</script>
<style type="text/css">
    .cpBody {
        width: 95%;
    }
</style>
<div style="width: auto; padding: 5px;">
    <asp:UpdatePanel ID="upEmails" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <asp:GridView ID="gvNotes" runat="server" AutoGenerateColumns="False"
                CssClass="gridview" AllowSorting="true" OnSorting="gvNotes_Sorting" OnRowDataBound="gvNotes_RowDataBound"
                EmptyDataText="No Notes found." OnRowCreated="gvNotes_RowCreated" RowStyle-VerticalAlign="Top"
                AllowPaging="True" PageSize="5" PagerSettings-Mode="NumericFirstLast" Width="100%" OnPageIndexChanging="gvNotes_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="NOTE_DATE_TIME" HeaderText="Date/Time" DataFormatString="{0:MM/dd/yyyy hh:mm tt}"
                        ItemStyle-VerticalAlign="Top" ItemStyle-Width="130" SortExpression="NOTE_DATE_TIME" />
                    <asp:BoundField DataField="Type" HeaderText="Type" ItemStyle-Width="100" ItemStyle-VerticalAlign="Top" SortExpression="Type" />
                    <asp:BoundField DataField="Username" HeaderText="User" ItemStyle-Width="120" ItemStyle-VerticalAlign="Top" SortExpression="Username" />
                    <asp:BoundField DataField="TASK_NAME" HeaderText="Workflow" ItemStyle-Width="120" SortExpression="TASK_NAME" />
                    <asp:BoundField DataField="REG_PAGE_NAME" HeaderText="Screen" ItemStyle-Width="100" SortExpression="REG_PAGE_NAME" />
                    <asp:BoundField DataField="DISPOSITION" HeaderText="Action" ItemStyle-Width="150" SortExpression="DISPOSITION" Visible="false" />
                    <asp:BoundField DataField="NOTE_TEXT" HeaderText="Notes" ItemStyle-Width="350" SortExpression="NOTE_TEXT" />
                    <%--            <asp:TemplateField HeaderText="Notes" SortExpression="NOTE_TEXT" ItemStyle-Width="350"  >
                <ItemTemplate>
                    <cc1:Accordion ID="accNotes" runat="Server" SelectedIndex="0" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                        AutoSize="None" FadeTransitions="true" TransitionDuration="100" FramesPerSecond="20" RequireOpenedPane="true"
                        SuppressHeaderPostbacks="true">
                        <Panes>
                            <cc1:AccordionPane ID="AccordionPane1" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                                ContentCssClass="accordionContent" BorderStyle="None">
                                <Header>
                                    <asp:LinkButton ID="lnkAccPreview" runat="server" Text="Preview" CssClass="QstLink"  />
                                </Header>
                                <Content>
                                    <asp:Label ID="lblNotePreview" runat="server" />
                                </Content>
                            </cc1:AccordionPane>        
                            <cc1:AccordionPane ID="AccordionPane2" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
                                ContentCssClass="accordionContent" BorderStyle="None">
                                <Header>
                                    <asp:LinkButton ID="lnkAccMore" runat="server" Text="Full" CssClass="QstLink" />
                                </Header>
                                <Content>
                                    <asp:Label ID="lblNoteFull" runat="server" />
                                </Content>
                            </cc1:AccordionPane>        
                        </Panes>            
                    </cc1:Accordion>
                </ItemTemplate> 
            </asp:TemplateField>--%>
                </Columns>
                <PagerStyle CssClass="gridViewPager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pHeader" runat="server" CssClass="cpHeader">
        <div style="text-align: right; padding-top: 10px; cursor: pointer;">
            <span tabindex="0">
            <asp:Image ID="imgAddNote" runat="server" ImageUrl="~/Images/add.png" AlternateText="Add a Note" />
                </span>
        </div>
        <asp:Label ID="lblText" runat="server" />
    </asp:Panel>
</div>

<asp:Panel ID="pBody" runat="server" CssClass="cpBody">
    <div style="text-align: left">
        <asp:ValidationSummary ID="vsNoteEntry" runat="server" DisplayMode="List" ValidationGroup="valNoteEntry" />
        <div border="0" cellpadding="0" cellspacing="0" style="text-align: left" role="presentation">
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabelSmall">Date</span></div>
                <div class="col-sm-9 text-left">
                    <asp:Label ID="lblLastActivity" runat="server" CssClass="formFieldDisplay wd450" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabelSmall">Type</span></div>
                <div class="col-sm-9 text-left">
                    <label id="lblNoteType" for="ddlNoteType" style="display:none;">Type</label>
                    <asp:DropDownList ID="ddlNoteType" aria-labelledby="lblNoteType" runat="server" CssClass="formDropDown" ToolTip="Type" />
                    <asp:RequiredFieldValidator runat="server" ID="reqType" ValidationGroup="valNoteEntry"
                        ControlToValidate="ddlNoteType" ErrorMessage="* Select a Type" Text="*" Display="Dynamic"
                        SetFocusOnError="true" InitialValue="" Enabled="false" />
                    <asp:HiddenField ID="hdnSelectedNoteType" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right">
                    <span class="formLabelSmall" style="vertical-align: top">Details</span>
                </div>
                <div class="col-sm-9 text-left">
                     <label id="lblDetails" for="txtDesc" style="display:none;">Details</label>
                    <asp:TextBox ID="txtDesc" aria-labelledby="lblDetails" runat="server" Rows="10" CssClass="formFieldMultiline" TextMode="MultiLine" MaxLength="4000"
                        ControlToValidate="txtDesc" ToolTip="Details" />
                    <asp:RequiredFieldValidator runat="server" ID="reqDesc" ValidationGroup="valNoteEntry"
                        ControlToValidate="txtDesc" ErrorMessage="* Enter Details" Text="*" Display="Dynamic"
                        SetFocusOnError="true" Enabled="false" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12 text-center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" ValidationGroup="valNoteEntry"
                        CausesValidation="true" OnClick="btnSave_Click" />
                    &nbsp;
                    <asp:Button ID="btnCancelNote" runat="server" Text="Cancel" CssClass="buttonBox" OnClientClick="return false;" />
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
<cc1:CollapsiblePanelExtender ID="cpeNotes" runat="server" TargetControlID="pBody" CollapseControlID="btnCancelNote" ExpandControlID="imgAddNote"
    Collapsed="true" CollapsedSize="0">
</cc1:CollapsiblePanelExtender>
<uc1:MessageBox ID="MessageBox2" runat="server" ErrorListCssClass="aligncenterPad" />

