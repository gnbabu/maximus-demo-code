<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_HospiceDocumentsByMail_, App_Web_c4une0e1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register Src="~/PopupControls/HospiceHLTCFProviderSearch.ascx" TagName="HospiceHLTCFProviderSearch" TagPrefix="uc1" %>--%>
<%--<%--<script src="../Scripts/jquery-1.4.1.min.js"></script>--%>
<%--<script src="../Scripts/bootstrap.min.js"></script>
<link rel="stylesheet" href="../Content/bootstrap.min.css" />
<link rel="stylesheet" href="../Content/bootstrap-theme.min.css" />--%>


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

<ajax:Accordion ID="AccordionHospiceDocumentsByMail" runat="Server" SelectedIndex="-1" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionPaneHospiceDocumentsByMail" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblHospiceDocumentsByMail" class="expandcollapse" runat="server" Text="+ DOCUMENT BY MAIL"></asp:Label>
            </Header>
            <Content>
                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvHospiceDocumentsByMail" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true" DataKeyNames="LineItem"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvHospiceDocumentsByMail_PageIndexChanging"                     
                        OnRowDeleting="gvHospiceDocumentsByMail_RowDeleting" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>                             
                            <asp:BoundField DataField="LineItem" HeaderText="Line Item" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            <asp:BoundField DataField="DocumentType" HeaderText="Document Type" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            <asp:BoundField DataField="Note" HeaderText="Note" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                             <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Button ID="btnPrintCover" Text="PrintCover" runat="server" OnClientClick="javascript:window.open('../Documents/MITSEDMS_Cover_IT4.pdf'); return false;" CssClass="button"/>
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
                            <asp:DropDownList ID="ddlDocumentType" runat="server" CssClass="formField" Width="260px">
                            </asp:DropDownList>
                        </span>

                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label"><b>Note: </b>
                            <asp:TextBox ID="txtNote" CssClass="ohio-field-input" runat="server" />
                        </span>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="fbtnAdd_Click"
                            CssClass="button" CausesValidation="false" />
                    </div>
                </div>

                </Content>

        </ajax:AccordionPane>
    </Panes>

</ajax:Accordion>