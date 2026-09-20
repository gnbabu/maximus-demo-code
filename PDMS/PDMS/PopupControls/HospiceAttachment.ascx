<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_HospiceAttachment" Codebehind="HospiceAttachment.ascx.cs" %>
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
<script type="text/javascript">
    $(function () {
        $("[id*=gvHospiceAttachment] [id*=btnUpdate]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["eddlBenefitLineNo", "euAttachmentUpload", "eddlDocumentType"];

            var isValid = RequiredFieldsValidations(row, requiredControles);

            return isValid;
        });
    });
    function DocumentTypeChange(ctrl) {
        var selectedItem = $(ctrl).find('option:selected').text();
        var hdCntrl = $(ctrl).closest('tr').find('td input[id*="hdnDocumentType"]');
        hdCntrl.val(selectedItem);
    }
    function AttachementValidation(ctrl) {
        var row = $(ctrl).closest("tr");
        var requiredControles = ["fddlBenefitLineNo", "fuAttachmentUpload", "fddlDocumentType"];
        var isValid = RequiredFieldsValidations(row, requiredControles, "AttachmentErrorMessage");
        if (isValid === true) {
            var fuAttachmentUpload = row.find("[id*=fuAttachmentUpload]").val();
            var validFilesTypes = ["doc", "docx", "xlsx", "pdf", "jpeg", "zip", "jpg"];
            var isValidFile = false;
            var ext = fuAttachmentUpload.substring(fuAttachmentUpload.lastIndexOf(".") + 1, fuAttachmentUpload.length).toLowerCase();
            $.each(validFilesTypes, function (index, val) {
                if (ext === val) {
                    isValidFile = true;
                }
            });
            if (isValidFile === false) {
                $("[id*=AttachmentErrorMessage]").text("Unknown file type.");
                isValid = false;
            }
        }
        return isValid;
    }
    function AttachementUpdateValidation(ctrl) {
        var row = $(ctrl).closest("tr");
        var requiredControles = ["eddlBenefitLineNo", "euAttachmentUpload", "eddlDocumentType"];
        var isValid = RequiredFieldsValidations(row, requiredControles, "AttachmentErrorMessage");
        if (isValid === true) {
            var euAttachmentUpload = row.find("[id*=euAttachmentUpload]").val();
            var validFilesTypes = ["doc", "docx", "xlsx", "pdf", "jpeg", "zip", "jpg"];
            var isValidFile = false;
            var ext = euAttachmentUpload.substring(euAttachmentUpload.lastIndexOf(".") + 1, euAttachmentUpload.length).toLowerCase();
            $.each(validFilesTypes, function (index, val) {
                if (ext === val) {
                    isValidFile = true;
                }
            });
            if (isValidFile === false) {
                $("[id*=AttachmentErrorMessage]").text("Unknown file type.");
                isValid = false;
            }
        }
        return isValid;
    }
</script>
<ajax:Accordion ID="AccordionHospiceAttachment" runat="Server" SelectedIndex="-1" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false" style="display:none;"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionPaneHospiceAttachment" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblHospiceAttachment" class="expandcollapse" runat="server" Text="+ ATTACHMENT"></asp:Label>
            </Header>
            <Content>

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvHospiceAttachment" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvHospiceAttachment_PageIndexChanging"
                        OnRowEditing="gvHospiceAttachment_RowEditing" OnRowUpdating="gvHospiceAttachment_RowUpdating" OnRowCancelingEdit="gvHospiceAttachment_RowCancelingEdit"
                        OnRowDeleting="gvHospiceAttachment_RowDeleting" OnRowDataBound="gvHospiceAttachment_RowDataBound" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>
                            <asp:TemplateField HeaderText="Attachment" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:Image ID="AttachmentIcon" runat="server" ImageUrl="~/Images/file-icon.jpg" Height="20px" Width="20px" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:FileUpload ID="euAttachmentUpload" runat="server" CssClass="ohio-field-input" />
                                     <span style="color: red; display: none">
                                        <br />
                                        Document is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:FileUpload ID="fuAttachmentUpload" runat="server" CssClass="ohio-field-input" />
                                    <span style="color: red; display: none">
                                        <br />
                                        Document is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Benefit Line No" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenefitLineNo" runat="server" Text='<%# Bind("LineItem") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="hdnBenefitLineNo" runat="server" Value='<%#Bind("LineItem") %>' />
                                    <asp:DropDownList ID="eddlBenefitLineNo" runat="server" onchange="javascript:BenefitLineNumberChange(this,'Edit');"></asp:DropDownList>
                                    <span style="color: red; display: none">
                                        <br />
                                        Benefit Line No is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:HiddenField ID="hdnBenefitLineNo" runat="server" />
                                    <asp:DropDownList ID="fddlBenefitLineNo" runat="server" onchange="javascript:BenefitLineNumberChange(this,'Add');"></asp:DropDownList>
                                    <span style="color: red; display: none">
                                        <br />
                                        Benefit Line No is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Benefit Period Type" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenefitPeriodType" runat="server"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="elblSegmentBenefitType" runat="server"></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="flblSegmentBenefitType" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Benefit Period" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenefitPeriod" runat="server"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="elblDateBenefitPeriod" runat="server"></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="flblDateBenefitPeriod" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="LineItem" HeaderText="Line Item" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />--%>
                            <asp:BoundField DataField="DocumentID" HeaderText="Document ID" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            <%-- <asp:BoundField DataField="AttachType" HeaderText="Document Type" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />--%>
                            <asp:TemplateField HeaderText="Document Type" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblAttachType" runat="server" Text='<%# Bind("AttachType") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="hdnDocumentType" runat="server" Value='<%#Bind("AttachType") %>' />
                                    <asp:DropDownList ID="eddlDocumentType" runat="server" onchange="javascript:DocumentTypeChange(this);"></asp:DropDownList>
                                    <span style="color: red; display: none">
                                        <br />
                                        Document Type is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:HiddenField ID="hdnDocumentType" runat="server" />
                                    <asp:DropDownList ID="fddlDocumentType" runat="server" onchange="javascript:DocumentTypeChange(this);"></asp:DropDownList>
                                    <span style="color: red; display: none">
                                        <br />
                                        Document Type is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- <asp:BoundField DataField="Note" HeaderText="Note" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />--%>
                            <%--<asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Button ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" Visible='<%# Convert.ToBoolean(Eval("IsFromInquiry")) == true ? false : true %>'
                                        CssClass="button" OnClientClick='return confirm("Are you sure you want to delete this record?");' CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CssClass="button" CausesValidation="false" Visible='<%# Convert.ToBoolean(Eval("IsFromInquiry")) == true ? false : true %>' />
                                    &nbsp;
                <asp:Button ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" Visible='<%# Convert.ToBoolean(Eval("IsFromInquiry")) == true ? false : true %>'
                    CssClass="button" OnClientClick='return confirm("Are you sure you want to delete this record?");' CausesValidation="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button ID="btnUpdate" Text="Update" OnClientClick="return AttachementUpdateValidation(this)" runat="server" CommandName="Update" CssClass="button" />
                                    &nbsp;
                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" CssClass="button" CausesValidation="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="ftnAdd" runat="server" OnClientClick="return AttachementValidation(this)" Text="Add New" OnClick="fbtnAdd_Click" CssClass="button" Style="width: auto !important;" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                    <div style="text-align: center">
                        <span id="AttachmentErrorMessage" runat="server" style="color: red;"></span>
                    </div>
                </div>
            </Content>

        </ajax:AccordionPane>
    </Panes>

</ajax:Accordion>
