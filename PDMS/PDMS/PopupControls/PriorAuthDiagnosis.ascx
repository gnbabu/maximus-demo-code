<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_PriorAuthDiagnosis" Codebehind="PriorAuthDiagnosis.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%--<%@ Register Src="~/PopupControls/PriorAuthDiagnosisSeach.ascx" TagPrefix="uc1" TagName="PriorAuthDiagnosisSeach" %>--%>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>

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
        $("[id*=fbtnAdd]").click(function () {
            var row = $(this).closest("tr");
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
<ajax:Accordion ID="PriorAuthDiagnosis" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionPanePriorAuthDiagnosis" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblPriorAuthDiagnosis" class="expandcollapse" runat="server" Text="- Diagnosis"></asp:Label>
            </Header>
            <Content>

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvPriorAuthDiagnosis" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvPriorAuthDiagnosis_PageIndexChanging"
                        PageSize="10" OnRowDeleting="gvPriorAuthDiagnosis_RowDeleting" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>
                          
                            <asp:BoundField DataField="ODSDiagnosisID" HeaderText="Line" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            <asp:BoundField DataField="DiagnosisTypeCode" HeaderText="*Diagnosis Code Type" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                            <asp:BoundField DataField="DiagnosisCode" HeaderText="Document Type" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
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
                        <span class="ohio-field-label"><span style="color: red"></span> <b>Line </b>
                            
                            <asp:TextBox ID="txtDiagnosisline" runat="server" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true" />
                        </span>

                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label"><span style="color: red">*</span><b>Diagnosis Code Type </b>
                         <asp:DropDownList ID="ddlDiagnosisCodeType" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                AppendDataBoundItems="True" OnSelectedIndexChanged="ddlDiagnosisCodeType_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="reqDiagnosisCodeType" SetFocusOnError="true"
                                ValidationGroup="vgCarePlan" ControlToValidate="ddlDiagnosisCodeType" ErrorMessage="*Diagnosis Code Type" Text="*" Display="Dynamic" InitialValue="0" />
                            <span style="color:red; display:none"><br />Document type is required</span>
                        </span>

                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label"><span style="color: red"></span> <b>Diagnosis Code </b>
                           <asp:TextBox ID="txtDiagnosisCode" runat="server" CssClass="formField" />
                            <asp:LinkButton ID="lnkDiagnosisSearch" runat="server" ToolTip="Search" CommandName="DiagnosisSearch" OnClick="lnkDiagnosisSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>&nbsp;&nbsp;
                        </span>

                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <span class="ohio-field-label"><b>Diagnosis Code Description </b>
                            <asp:TextBox ID="txtDiagnosisCodeDesc" runat="server" Rows="2" CssClass="formField wd650" TextMode="MultiLine" MaxLength="1000" OnTextChanged="txtDiagnosisCodeDesc_TextChanged" />
                        </span>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="DiagnosisAdd_Click"
                            CssClass="button" CausesValidation="true" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" 
                            CausesValidation="false" />
                    </div>
                </div>
            </Content>

        </ajax:AccordionPane>
    </Panes>
   
</ajax:Accordion>
 <%--<asp:Panel ID="pnlDiag" runat="server">
        <asp:MultiView ID="mltDiag" runat="server">
            <asp:View ID="vwDiagnosis" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                    <div class="row">
                        <uc1:PriorAuthDiagnosisSeach runat="server" ID="ucPriorAuthDiagnosisSeach" />
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
</asp:Panel>--%>

<%--<ajax:ModalPopupExtender ID="Dia" runat="server" PopupControlID="pnlDiagnosissearch" TargetControlID="Button1" BackgroundCssClass="modalBackground" />
<asp:Panel ID="pnlDiagnosissearch" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
    <asp:Panel ID="pnlDiagnosisheader" CssClass="popHeader" runat="server" HorizontalAlign="Left">
        <div class="popTitle">
            <asp:Label ID="lblDiagnosissearch" CssClass="bodyTextBold" runat="server" Text="Title" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlDiag" runat="server">
        <asp:MultiView ID="mltDiag" runat="server">
            <asp:View ID="vwDiagnosis" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                    <div class="row">
                        <uc1:PriorAuthDiagnosisSeach runat="server" ID="ucPriorAuthDiagnosisSeach" />
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
</asp:Panel>--%>
<asp:Button runat="server" ID="Button1" Style="display: none" Text="ButtonDummy" />



<%-- Claim Diagnosis Code pannel (Institutional 4.11, Professional 4.12)--%>

<%--<asp:UpdatePanel ID="upClaimDiagnosisCode" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:Panel ID="pnlClaimDiagnosisCode" runat="server" DefaultButton="btnSave" Style="padding: 10px;">
            <div style="width: 1200px;">
                <asp:ValidationSummary ID="ValidationSummary1" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="PriorAuthServicesDetails" />
                <div class="wdAuto">

                    <div class="row Diagnosis" runat="server">
                        <div class="col-sm-3  text-left">
                            <asp:Label ID="lblSequence1" runat="server" Text="*Sequence" CssClass="formLabel200" />
                            <asp:DropDownList ID="ddlSequence" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                AppendDataBoundItems="True" OnSelectedIndexChanged="ddlSequence_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvSequence" SetFocusOnError="true"
                                ValidationGroup="vgddlSequence" ControlToValidate="ddlSequence" ErrorMessage="*Sequence" Text="*" Display="Dynamic" InitialValue="0" />
                        </div>
                        <div class="col-sm-3  text-left">
                            <asp:Label ID="lblclaimDiagnosisCode" runat="server" Text="Diagnosis Code" CssClass="formLabel200" />
                            <asp:TextBox ID="txtclaimDiagnosisCode" runat="server" CssClass="formField" MaxLength="7" OnTextChanged="txtclaimDiagnosisCode_TextChanged" />

                            <asp:RequiredFieldValidator runat="server" ID="rfvclaimDiagnosisCode" SetFocusOnError="true"
                                ValidationGroup="vgclaimDiagnosisCode" ControlToValidate="txtclaimDiagnosisCode" ErrorMessage="*Diagnosis Code is required" Text="*" Display="Dynamic" InitialValue="0" />
                        </div>

                        <div class="col-sm-3  text-left">
                            <asp:Label ID="lblPresentonAdmission" runat="server" Text="*Present on Admission" CssClass="formLabel200" />
                            <asp:DropDownList ID="ddlPresentonAdmission" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                AppendDataBoundItems="True" OnSelectedIndexChanged="ddlPresentonAdmission_SelectedIndexChanged">
                                <asp:ListItem Value="1" Text=""></asp:ListItem>
                                <asp:ListItem Value="2" Text="Yes"></asp:ListItem>
                                <asp:ListItem Value="3" Text="No" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" SetFocusOnError="true"
                                ValidationGroup="vgPresentonAdmission" ControlToValidate="ddlPresentonAdmission" ErrorMessage="*Present on Admission required for principal diagnosis code" Text="*" Display="Dynamic" InitialValue="0" />
                        </div>

                        <div class="col-sm-3  text-left">
                            <asp:Label ID="lblclaimDiagDesc" runat="server" Text="Diagnosis Code Description" CssClass="formLabel200" />
                            <asp:TextBox ID="txtclaimDiagDesc" runat="server" Rows="2" CssClass="formField wd650" TextMode="MultiLine" MaxLength="1000" />
                        </div>
                    </div>

                </div>
                <asp:UpdateProgress runat="server" ID="UpdateProgress1" DisplayAfter="0" AssociatedUpdatePanelID="upPriorAuthDiagnosis">
                    <ProgressTemplate>
                        <div class="loading">
                            <asp:Image ID="imgSaving2" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table>
                    <tr>
                        <td>
                           <%-- <asp:Button ID="Button1" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="PriorAuthNotes" /></td>--%>
<%--  <td>
                            <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" /></td>--%>
<%-- </tr>
                </table>
            </div>

        </asp:Panel>
    </ContentTemplate>

</asp:UpdatePanel>--%>
