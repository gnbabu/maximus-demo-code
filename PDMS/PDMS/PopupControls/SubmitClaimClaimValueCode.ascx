<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_SubmitClaimClaimValueCode" Codebehind="SubmitClaimClaimValueCode.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<%--Caim Value Code--%>
<ajax:Accordion ID="SubmitClaimClaimValueCode" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="ClaimValueCode" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblClaimValueCode" class="expandcollapse" runat="server" Text="- Value Code"></asp:Label>
            </Header>
            <Content>
                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvClaimValueCode" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvClaimValueCode_PageIndexChanging"
                        PageSize="10" OnRowDeleting="gvClaimValueCode_RowDeleting" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>
                            <asp:BoundField DataField="" HeaderText="Items" />
                            <asp:BoundField DataField="" HeaderText="*Value Code" />
                             <asp:TemplateField HeaderText="" ItemStyle-Width="100" ItemStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkValuCode" runat="server" ToolTip="Search" Text='Search' CommandArgument='<%# Eval("DiagnosisCode") %>' OnClick="lnkValueCodeSearch_Click" Visible="true">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                            <asp:BoundField DataField="" HeaderText="*Monetary Amount" />
                            <asp:BoundField DataField="" HeaderText="Value Code Description" />
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
                    <asp:ValidationSummary ID="vsAdditionalProviderserviceInfo" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="vgAdditionalProviderinfo" />
                    <div class="col-sm-6">
                        <div class="row" id="lblClaimValueCodeItems" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Items</span>
                            </div>
                             <div class="col-sm-7">
                                <span style="text-align: left;">
                                     <asp:TextBox ID="txtClaimValueCodeItems" runat="server" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true"/>

                                </span>
                            </div>
                        </div>
                   </div>
                     <div class="col-sm-6">
                        <div class="row" id="lblValueCode" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">*Value Code</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtValueCode" runat="server" MaxLength="2"  CssClass ="formField" Style="height: 30px; width: 200px" />
                                     <asp:RequiredFieldValidator runat="server" ID="rfvValueCode" SetFocusOnError="true"
                                ValidationGroup="valOwnerInfo" ControlToValidate="txtValueCode" ErrorMessage="*Value code is required" Text="*" Display="Dynamic" InitialValue="0" />
                            
                                    </div>
                            </span>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lblMonetaryAmount" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Monetary Amount</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtMonetaryAmount" runat="server" MaxLength="15" CssClass="formField" Style="height: 30px; width: 200px" />
                                       <asp:RequiredFieldValidator runat="server" ID="rfvMonetaryAmount"
                                ControlToValidate="txtMonetaryAmount" ErrorMessage="*Monetary Amount" Text="*" Display="Dynamic"
                                SetFocusOnError="true" ValidationGroup="valMonetary Amount" />
                            <asp:RegularExpressionValidator ID="revMonetaryAmount" runat="server" ControlToValidate="txtMonetaryAmount"
                                ValidationExpression="^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$" ErrorMessage="*Monetary Amount" Text="*" Display="Dynamic"
                                ValidationGroup="valMonetaryAmount" />
                            </div>
                            </span>
                        </div>
                    </div>
                     <div class="col-sm-6">
                        <div class="row" id="lblValueCodeDesc" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Value Code Description</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtValueCodeDesc" runat="server" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true" />
                                   
                            </div>
                            </span>
                        </div>
                    </div>
                 <div class="col-sm-6 col-md-4 col-lg-3 ">
                    <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="ValueCodeAdd_Click"
                        CssClass="button" CausesValidation="true" />
                    <asp:Button ID="Button1" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" CausesValidation="false" />
                </div>

                </div>
            </Content>

           
        </ajax:AccordionPane>
    </Panes>
</ajax:Accordion>


                     
