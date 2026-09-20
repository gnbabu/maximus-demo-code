<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_PriorAuthAttachment" Codebehind="PriorAuthAttachment.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagPrefix="uc" TagName="UploadSectionControl" %>--%>
<%--<%@ Register Src="~/PopupControls/OtherDocUploadSectionControl.ascx" TagPrefix="uc" TagName="OtherDocUploadSectionControl" %>--%>

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
        $("[id*=btnAdd]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["ddlDocumentTypeclaims"];
            var requiredControles = ["ddlDocumentType"];
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

<ajax:Accordion ID="PriorAuthAttachment" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionPanePriorAuthAttachment" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblPriorAuthAttachment" class="expandcollapse" runat="server" Text="- ATTACHMENT"></asp:Label>
            </Header>
            <Content>

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvPriorAuthAttachment" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvPriorAuthAttachment_PageIndexChanging"
                        PageSize="10" OnRowDeleting="gvPriorAuthAttachment_RowDeleting" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                <ItemTemplate>
                                    <asp:Image ID="AttachmentIcon" runat="server" ImageUrl="~/Images/file-icon.jpg" Height="20px" Width="20px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ODSAttachmentID" HeaderText="Line Item" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            <asp:BoundField DataField="AttachmentID" HeaderText="Document ID" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            <asp:BoundField DataField="AttachmentType" HeaderText="Document Type" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                          <%--  <asp:BoundField DataField="PRIOR_AUTH_Note" HeaderText="Note" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />--%>
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
                    <%--<div class="col-sm-6 col-md-4 col-lg-3">
                        <span class="ohio-field-label"><b>Line </b>
                            <asp:TextBox ID="txtline" runat="server" Style="background-color: lightgrey;" CssClass="formField" />
                        </span>
                           
                        </div>--%>
                       
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label"><span style="color: red">*</span> <b>Upload attachment: </b>
                            <asp:FileUpload ID="PriorAttachmentUpload" runat="server" CssClass="ohio-field-input" />
                        </span>

                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label"><span style="color: red">*</span><b>Document Type: </b>
                          <%--  <asp:Label ID="lblAttDocType" runat="server" Text="Document Type" CssClass="formLabel200" />--%>
                            <asp:DropDownList ID="ddlDocumentType" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                AppendDataBoundItems="True" OnSelectedIndexChanged="ddlDocumentType_SelectedIndexChanged">
                            </asp:DropDownList>
                           <%-- <asp:DropDownList ID="ddlDocumentType" runat="server" CssClass="formField" Width="260px">
                            </asp:DropDownList>--%>
                            <span style="color:red; display:none"><br />Document type is required</span>
                        </span>

                    </div>
                      <div id="DocumentTypeclaims" class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label"><span style="color: red">*</span><b>Document Type: </b>
                          <%--  <asp:Label ID="lblAttDocType" runat="server" Text="Document Type" CssClass="formLabel200" />--%>
                            <asp:DropDownList ID="ddlDocumentTypeclaims" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                AppendDataBoundItems="True" OnSelectedIndexChanged="ddlDocumentTypeclaims_SelectedIndexChanged">
                            </asp:DropDownList>
                           <%-- <asp:DropDownList ID="ddlDocumentType" runat="server" CssClass="formField" Width="260px">
                            </asp:DropDownList>--%>
                            <span style="color:red; display:none"><br />Document type is required</span>
                        </span>

                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label"><b>Note: </b>
                            <asp:TextBox ID="txtNote" CssClass="ohio-field-input" runat="server" />
                        </span>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="btnAdd_Click"
                            CssClass="button" CausesValidation="true" />
                        <br>
                        </br>
                        <asp:Button ID="brnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Click"
                            CssClass="button" CausesValidation="true" />
                        
                    </div>
                    
                </div>
            </Content>

        </ajax:AccordionPane>
    </Panes>

</ajax:Accordion>







<%--<asp:UpdatePanel ID="upPriorAuthAttachment" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:Panel ID="pnlPriorAuthAttachment" runat="server" DefaultButton="btnSave" Style="padding: 10px;">
            <div style="width: 1200px;">
                <asp:ValidationSummary ID="vsPriorAuthAttachment" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="PriorAuthServicesDetails" />
                <div class="wdAuto">
                   
                    <div class="row Attachment" runat="server">
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">

                            <asp:Label ID="lbluploadattachment" runat="server" Text="Upload Attachment" CssClass="formLabel200" />
                            <asp:DropDownList ID="ddluploadattachment" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                AppendDataBoundItems="True" OnSelectedIndexChanged="ddluploadattachment_SelectedIndexChanged">
                            </asp:DropDownList>
                          <uc:UploadSectionControl runat="server" ID="UploadSectionControl" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblAttDocType" runat="server" Text="Document Type" CssClass="formLabel200" />
                            <asp:DropDownList ID="ddlDocumentType" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                AppendDataBoundItems="True" OnSelectedIndexChanged="ddlDocumentType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="ccol-sm-6 col-md-4 col-lg-3 text-right">
                            <asp:Label ID="lblAttnote" runat="server" Text="Notes" CssClass="formLabel200" />
                            <asp:TextBox ID="txtAttNotes" runat="server" Rows="2" CssClass="formField wd650" TextMode="MultiLine" MaxLength="1000" />
                            <asp:RequiredFieldValidator ID="reqNotes" runat="server" ControlToValidate="txtAttNotes" ErrorMessage="* Required Attachment" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
                        </div>
                         <asp:UpdateProgress runat="server" ID="upPrAuthAttachment" DisplayAfter="0" AssociatedUpdatePanelID="upPriorAuthAttachment">
                <ProgressTemplate>
                    <div class="loading">
                        <asp:Image ID="imgSaving" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                    </div>
                </ProgressTemplate>--%>
            <%--</asp:UpdateProgress>
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="PriorAuthNotes" /></td>
                    <td>
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" /></td>
                </tr>
            </table>
                    </div>
                    <asp:PlaceHolder runat="server" ID="PriorAuthAttachment"></asp:PlaceHolder>
                    <uc:UploadSectionControl runat="server" ID="UploadSectionControl1" />
                    <div class="div AddDocUpload">
        <asp:ImageButton ID="btnDocUpload" runat="server" ImageUrl="~/Images/add.png" CommandName="Upload" OnCommand="btnAdd_Click" ToolTip="Add" />

    </div>
                     <ajax:ModalPopupExtender ID="upl" runat="server" PopupControlID="pnlDocUpload" TargetControlID="Button1" BackgroundCssClass="modalBackground" />
    <asp:Panel ID="pnlDocumentUpload" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
        <asp:Panel ID="pnlDocupload" CssClass="popHeader" runat="server" HorizontalAlign="Left">
            <div class="popTitle">
                <asp:Label ID="lblDocUpload" CssClass="bodyTextBold" runat="server" Text="Title" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnluploadDoc" runat="server">
            <asp:MultiView ID="mltUpload" runat="server">
                <asp:View ID="vwUploadDoc" runat="server">
                   <uc:OtherDocUploadSectionControl runat="server" ID="ucOtherDocUploadSectionControl" />
                </asp:View>
            </asp:MultiView>
        </asp:Panel>
        <asp:Button runat="server" ID="Button1" Style="display: none" Text="ButtonDummy6" />--%>
                   
       <%-- </asp:Panel>
                    </asp:Panel>
    </ContentTemplate>

</asp:UpdatePanel>--%>
