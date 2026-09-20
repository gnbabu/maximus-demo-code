<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_PriorAuthDocumentbyMail, App_Web_c4une0e1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<style>
    table.gridview td, .rgRow td, .rgAltRow td, table.gridViewSmallFont td {
        font-size: 14px;
    }

    select {
        min-width: 90%;
        height: 25px;
    }

    .gridViewFooter {
        background-color: white;
    }

    .gridViewSelected, .gridViewSelected td, .gridViewSelected tr {
        background-color: white;
    }
</style>

<ajax:Accordion ID="AccordionPriorAuthDocumentbyMail" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionPanePriorAuthDocumentbyMail" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblPriorAuthDocumentbyMail" class="expandcollapse" runat="server" Text="DOCUMENT BY MAIL"></asp:Label>
            </Header>
            <Content>
                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvPriorAuthDocumentbyMail" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true" DataKeyNames="PriorAuthDocumentbyMailId"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvPriorAuthDocumentbyMail_PageIndexChanging"
                        PageSize="10" OnRowDeleting="gvPriorAuthDocumentbyMail_RowDeleting" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>
                            <asp:BoundField DataField="LineItem" HeaderText="Line" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            <asp:BoundField DataField="DocumentType" HeaderText="Document Type" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            <asp:BoundField DataField="PRIOR_AUTH_Note_ID" HeaderText="Note" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnPrintCover" runat="server" ImageUrl="~/Images/PrintCoverPage.png" CommandName="PrintCoverpage" OnCommand="btnPrintCoverAdd_Click"  OnClientClick="javascript:window.open('../Documents/MITSEDMS_Cover_IT4.pdf'); return false;" ToolTip="PrintCover" Height="19px" Width="81px"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Button ID="btnDelete" Text="Delete" runat="server" CommandName="Delete"
                                        CssClass="button" OnClientClick='return confirm("Are you sure you want to delete this record?");' CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>

                <div class="row" style="text-align: center;">

                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label"><span style="color: red">*</span><b>Document Type: </b>
                             <asp:DropDownList ID="ddlDoctype" CssClass="formField"  runat="server"
                                 >

                            </asp:DropDownList>
                        </span>

                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label"><b>Note: </b>
                              <asp:TextBox ID="txtDocNote" runat="server" Rows="2" CssClass="formField wd650" TextMode="MultiLine" MaxLength="1000" />
                        </span>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="DocumentbyMailAdd_Click"
                            CssClass="button" CausesValidation="true" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </div>

            </Content>

        </ajax:AccordionPane>
    </Panes>

</ajax:Accordion>



<%--<asp:UpdatePanel ID="upPriorAuthDocumentbyMail" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlPriorAuthDocumentbyMail" runat="server" DefaultButton="btnSave" Style="padding: 10px;">
            <div style="width: 1200px;">
                <asp:ValidationSummary ID="vsPriorAuthDocumentbyMail" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="PriorAuthDocumentbyMail" />
                <div class="wdAuto">
                    <div class="row DocumentbyMail" runat="server">
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblDoctype" runat="server" Text="Document Type" CssClass="formLabel200" />
                            <asp:DropDownList ID="ddlDoctype" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                AppendDataBoundItems="True" OnSelectedIndexChanged="ddlDoctype_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="ccol-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblDocNote" runat="server" Text="Notes" CssClass="formLabel200" />

                            <asp:TextBox ID="txtDocNote" runat="server" Rows="2" CssClass="formField wd650" TextMode="MultiLine" MaxLength="1000" />

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDocNote" ErrorMessage="* Required Attachment" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <asp:UpdateProgress runat="server" ID="upPriorAuth" DisplayAfter="0" AssociatedUpdatePanelID="upPriorAuthDocumentbyMail">
                    <ProgressTemplate>
                        <div class="loading">
                            <asp:Image ID="imgSaving" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="PriorAuthDocumentbyMail" /></td>
                        <td>
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" /></td>
                    </tr>
                </table>
            </div>

        </asp:Panel>
    </ContentTemplate>

</asp:UpdatePanel>--%>
