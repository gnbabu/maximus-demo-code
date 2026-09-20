<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SubmitClaimConditionCode, App_Web_av5ll3zk" %>
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

<ajax:Accordion ID="SubmitClaimConditionCode" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionPaneSubmitClaimConditionCode" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblSubmitClaimConditionCode" class="expandcollapse" runat="server" Text="-  CONDITION CODE"></asp:Label>
            </Header>
            <Content>
                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvSubmitClaimConditionCode" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvSubmitClaimConditionCode_PageIndexChanging"
                        PageSize="10" OnRowDeleting="gvSubmitClaimConditionCode_RowDeleting" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>

                            <asp:BoundField DataField="" HeaderText="Detail Line" />
                            <asp:BoundField DataField="" HeaderText="Condition Code" />
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnksearch" Text="Search" runat="server" ToolTip="Search" 
                                        OnClick="lnkConditionCodeSearch_Click" OnClientClick="exportPopup();" 
                                        Visible="true"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="" HeaderText="Condition Code Description" />
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
                    <asp:ValidationSummary ID="valSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valProviderInfoHeader" />
                    <div class="col-sm-6">
                        <div class="row" id="lblConditionCode" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">*Condition Code</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtConditionCode" runat="server" MaxLength="2" CssClass="formField" Style="height: 30px; width: 200px" />

                                    <asp:RequiredFieldValidator ID="rfvConditionCode" runat="server" ControlToValidate="txtConditionCode" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                         <asp:LinkButton ID="lnksearch" Text="Search" runat="server" ToolTip="Search" 
                                        OnClick="lnkConditionCodeSearch_Click" OnClientClick="exportPopup();" 
                                        Visible="true"></asp:LinkButton>
                    </div>
                     <div class="col-sm-6">
                        <div class="row" id="lblConditionDesc" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Condition Description</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtConditionDesc" runat="server"  Rows="2" Style="background-color: lightgrey; height: 30px; width: 200px" CssClass="formField" ReadOnly="true" TextMode="MultiLine" MaxLength="1000" />
                                </span>
                            </div>
                        </div>
                    </div>
                      <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="SubmitClaimConditionCodeAdd_Click"
                            CssClass="button" CausesValidation="true" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>


                </div>
            </Content>
        </ajax:AccordionPane>
    </Panes>
</ajax:Accordion>




<%--Condition Code --%>
<%--<asp:UpdatePanel ID="upClaimConditionCode" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:Panel ID="pnlPage5" runat="server" DefaultButton="btnSave" Style="padding: 10px;">
            <div style="width: 600px;">
                <asp:ValidationSummary ID="ValidationSummary2" DisplayMode="List" runat="server" CssClass="failureClaimConditionCode" ValidationGroup="PriorAuthNotes" />
                <div class="wdAuto">
                    <div class="row ClaimConditionCode" runat="server">
            <%--            <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblConditioncodeItems" runat="server" Text="Items" CssClass="formLabel200 " />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtConditioncodeItems" runat="server" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true" />
                        </div>--%>
<%--  <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblConditionCode" runat="server" Text="Condition Code" CssClass="formLabel200 " />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtConditionCode" runat="server" CssClass="formField" MaxLength="2" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvConditionCode" SetFocusOnError="true"
                                ValidationGroup="valOwnerInfo" ControlToValidate="txtConditionCode" ErrorMessage="*Enter Condition Code" Text="*" Display="Dynamic" InitialValue="0" />
                        </div>
                        <asp:LinkButton ID="lnkConditionCode" runat="server" ToolTip="Search" OnClick="lnkConditionCodeSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>&nbsp;&nbsp;

                       
                        <div class="col-sm-3  text-left">
                            <asp:Label ID="lblConditionDesc" runat="server" Text="Condition Description" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-7 text-right">
                            <asp:TextBox ID="txtConditionDesc" runat="server" Rows="2" Style="background-color: lightgrey;" CssClass="formField wd650" ReadOnly="true" TextMode="MultiLine" MaxLength="1000" />--%>

<%-- <asp:RequiredFieldValidator ID="rfvConditionDesc" runat="server" ControlToValidate="txtConditionDesc" ErrorMessage=" Condition Description" Text="*" Display="Dynamic" ValidationGroup="valOccurenceDesc"></asp:RequiredFieldValidator>--%>
<%--</div>
                    </div>
                </div>

                <asp:UpdateProgress runat="server" ID="upSubmitClaim" DisplayAfter="0" AssociatedUpdatePanelID="upClaimConditionCode">
                    <ProgressTemplate>
                        <div class="loading">

                            <asp:Image ID="imgConditionDesc" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
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