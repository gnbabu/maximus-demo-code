<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_NursingFacilityVentilator, App_Web_yvhxe4ml" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<style type="text/css">
    #ctl00_MainContent_ucNursingFacilityVentilator_ucMessageModal_pnlModal
    {
        height:320px;
        width:785px;
    }
</style>
<div>
    <asp:ValidationSummary ID="valNursingFacilityVentilator" runat="server" DisplayMode="List" ValidationGroup="NursingFacilityVentilator" />
</div>
<asp:UpdatePanel ID="upProv" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="NursingFacilityVent" runat="server" style="width: 100%;">
            <div id="ParentTable" runat="server">
                <div class="row">
                    <div class="col-sm-7  text-left">
                        <asp:Label ID="lblName" AssociatedControlID="rblnewnfv" runat="server" CssClass="formLabel200" Text="Are you applying as a new nursing facility ventilator provider?"></asp:Label>
                    </div>
                    <div class="col-sm-5" runat="server">
                        <fieldset>
                            <legend>
                                  <asp:RadioButtonList ID="rblnewnfv" BorderStyle="None" CellPadding="0" CellSpacing="0"
                            RepeatDirection="Horizontal" runat="server" RepeatLayout="Table" CssClass="QstRadioList" OnSelectedIndexChanged="rblnewnfv_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Selected="False" Text="No" Value="0"></asp:ListItem>
                            <asp:ListItem Selected="False" Text="Yes" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                            </legend>
                        </fieldset>
                      
                        <asp:RequiredFieldValidator runat="server" ID="reqnewnfv"
                            ControlToValidate="rblnewnfv" ErrorMessage="* Select an Option" Text="*" Display="Dynamic"
                            SetFocusOnError="true" ValidationGroup="NursingFacilityVentilator" />
                    </div>
                 </div>
              </div>
              <div id="divVentilatorQuestions" runat="server" style="line-height: 1.5;">
                <uc1:Separator runat="server" ID="Separator1" Header="Ventilator Questions" style="color: #545487; font-size: 20px;
                    font-weight: bold; padding-bottom: 0; padding-left: 10px;" />
                <hr class="underLine" />

                <asp:Repeater ID="rptVentilatorQuestions" runat="server" OnItemDataBound="rptVentilatorQuestions_ItemDataBound">
                    <HeaderTemplate>
                        <div class="legend">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="row">
                            <div class="col-sm-12" style="padding: 0;">
                                <asp:Label runat="server" ID="lblQuestionV" Text='<%# DataBinder.Eval(Container.DataItem, "[QUESTION_TEXT]") %>' />
                                <asp:Label runat="server" ID="lblQuestionTypeIDV" Text='<%# DataBinder.Eval(Container.DataItem, "[QUESTION_TYPE_ID]") %>' Visible="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <asp:RadioButtonList ID="rblConfirmQuestionV" runat="server" RepeatDirection="Horizontal" CssClass="QstRadioList">
                                    <asp:ListItem Selected="False" Text="No" Value="0"></asp:ListItem>
                                    <asp:ListItem Selected="False" Text="Yes" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="repeaterDivider"></div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div id="divWeaningQuestions" runat="server" style="line-height: 1.5;">
                <uc1:Separator runat="server" ID="Separator2" Header="Weaning Questions" style="color: #545487; font-size: 20px;
                    font-weight: bold; padding-bottom: 0; padding-left: 10px;" />
                <hr class="underLine" />

                <asp:Repeater ID="rptWeaningQuestions" runat="server" OnItemDataBound="rptWeaningQuestions_ItemDataBound">
                    <HeaderTemplate>
                        <div class="legend">
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="row">
                            <div class="col-sm-12" style="padding: 0;">
                                <asp:Label runat="server" ID="lblQuestionW" Text='<%# DataBinder.Eval(Container.DataItem, "[QUESTION_TEXT]") %>' />
                                <asp:Label runat="server" ID="lblQuestionTypeIDW" Text='<%# DataBinder.Eval(Container.DataItem, "[QUESTION_TYPE_ID]") %>' Visible="false" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <asp:RadioButtonList ID="rblConfirmQuestionW" runat="server" RepeatDirection="Horizontal" CssClass="QstRadioList">
                                    <asp:ListItem Selected="False" Text="No" Value="0"></asp:ListItem>
                                    <asp:ListItem Selected="False" Text="Yes" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="row">
                            <div class="repeaterDivider"></div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        </div>
                    </FooterTemplate>
                </asp:Repeater>
            </div>

          </div>
     </ContentTemplate>
</asp:UpdatePanel>

<!-- ModalPopupExtender -->
<ajax:ModalPopupExtender ID="mpeWarningMsgNF" runat="server" PopupControlID="pnlWarningMsgNF" TargetControlID="ButtonDummy"
    CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlWarningMsgNF" runat="server" CssClass="modalPopup" style="display:none;width:50%;height:auto;">
    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server" >
        <div class="popTitle">
            <asp:Label ID="lbl_title"  runat="server" Text="Message"  />
        </div>
    </asp:Panel>  
     <asp:Panel ID="pnlMain" runat="server" Style="margin-right:10px">      
        <div style="text-align: left; padding: 15px; margin-left:30px !important" class="container-fluid">
         <div class="row">
                <asp:Label runat="server" ID="lblWarningMsg"></asp:Label>
         </div>
        </div>
    </asp:Panel>
    <table border="0" style="padding-bottom: 10px;">
        <tr>
           <td></td>
            <td>
                <asp:Button runat="server" ID="btnCancel" Text="OK" CssClass="buttonBox" />
            </td>
        </tr>
    </table> 
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy"/>