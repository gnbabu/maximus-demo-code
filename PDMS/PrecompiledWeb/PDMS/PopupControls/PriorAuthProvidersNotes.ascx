<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_PriorAuthProvidersNotes, App_Web_av5ll3zk" %>

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
    $(function () {
        $("[id*=NotesAdd]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["txtProviderNotes"];
            return RequiredFieldsValidations(row, requiredControles);
        });       
    });

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

<ajax:Accordion ID="PriorAuthNotes" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionPanePriorAuthNotes" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblPriorAuthNotes" class="expandcollapse" runat="server" Text="- PROVIDER NOTES"></asp:Label>
            </Header>
            <Content>

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvPriorAuthNotes" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvPriorAuthNotes_PageIndexChanging"
                        PageSize="10" OnRowDeleting="gvPriorAuthNotes_RowDeleting" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>                           
                            <asp:BoundField DataField="ODSProviderNoteID" HeaderText="Line" HeaderStyle-Font-Size="18px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            
                            <asp:BoundField DataField="Note" HeaderText="Note" HeaderStyle-Font-Size="18px" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
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
                        <span class="ohio-field-label"><b>Note: </b>
                            <asp:TextBox ID="txtProviderNotes" CssClass="ohio-field-input" runat="server" />
                        </span>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="NotesAdd_Click"
                            CssClass="button" CausesValidation="true" />
                         <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" 
                             CausesValidation="false" />
                    </div>
                </div>
            </Content>

        </ajax:AccordionPane>
    </Panes>

</ajax:Accordion>



<%--<asp:UpdatePanel ID="upPriorAuthNotes" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:Panel ID="pnlPriorAuthNotes" runat="server" DefaultButton="btnSave" Style="padding: 10px;">
            <div style="width: 600px;">
                <asp:ValidationSummary ID="vsPriorAuthNotes" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="PriorAuthNotes" />
                <div class="wdAuto">

                    <div class="col-sm-3  text-left">
                        <asp:Label ID="lblProviderNote" runat="server" Text="*Note" CssClass="formLabel200" />
                    </div>
                    <div class="col-sm-7 text-right">
                        <asp:TextBox ID="txtProviderNotes" runat="server" Rows="2" CssClass="formField wd650" TextMode="MultiLine" MaxLength="1000" />
                        <%--<asp:TextBox ID="txtNotes" runat="server" CssClass="formField" />              --%>
                       <%-- <asp:RequiredFieldValidator ID="rfvtxtNote" runat="server" ControlToValidate="txtProviderNotes" ErrorMessage="* Required Note" Text="*" Display="Dynamic" ValidationGroup="valPriorAuthNotes"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <asp:UpdateProgress runat="server" ID="upPriorAuth" DisplayAfter="0" AssociatedUpdatePanelID="upPriorAuthNotes">
                    <ProgressTemplate>
                        <div class="loading">
                            <asp:Image ID="imgSaving" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="PriorAuthNotes" /></td>
                        <td>
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" /></td>
                    </tr>
                </table>
            </div>

        </asp:Panel>
    </ContentTemplate>


</asp:UpdatePanel>--%>


