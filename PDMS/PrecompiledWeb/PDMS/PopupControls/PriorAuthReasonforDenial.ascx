<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_PriorAuthReasonforDenial, App_Web_wenzyumt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
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
<script type="text/javascript">
    //$(function () {
    //    $("[id*=fbtnAdd]").click(function () {
    //        var row = $(this).closest("tr");
    //        var requiredControles = ["ddlDocumentType"];
    //        return RequiredFieldsValidations(row, requiredControles);
    //    });       
    //});

    function RequiredFieldsValidations(row, requiredControles) {
        var isValid = true;
        $.each(requiredControles, function (index, Id) {
            var label = row.find("[id*=" + Id + "]").next("SPAN");
            label.hide();
            if ($.trim(row.find("[id*=" + Id + "]").val()) === "") {
                label.show();
                isValid = false;
            }
        });

        return isValid;
    }

</script>


<ajax:Accordion ID="PriorAuthReasonforDenial" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionPanePriorAuthReasonforDenial" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblPriorAuthReasonforDenial" class="expandcollapse" runat="server" Text="- REASON FOR DENIAL "></asp:Label>
            </Header>
            <Content>

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvPriorAuthReasonforDenial" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvPriorAuthReasonforDenial_PageIndexChanging"
                        PageSize="10"  AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>                           
                          <%--  <asp:BoundField DataField="ODSProviderNoteID" HeaderText="Line" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />--%>
                            
                            <asp:BoundField DataField="ServiceStatusReasonCode" HeaderText="Reason Code" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
							 <asp:BoundField DataField="" HeaderText="Note" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>
                
            </Content>

        </ajax:AccordionPane>
    </Panes>

</ajax:Accordion>