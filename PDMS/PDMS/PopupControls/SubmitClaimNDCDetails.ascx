<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_SubmitClaimNDCDetails" Codebehind="SubmitClaimNDCDetails.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>


<ajax:Accordion ID="upNDCDetails" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="NDCDetails" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblNDCDetails" class="expandcollapse" runat="server" Text="- NDC DETAILS"></asp:Label>
            </Header>
            <Content>
                <%--<div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvNDCDetails" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvNDCDetails_PageIndexChanging"
                        PageSize="10" OnRowDeleting="gvTAdditionalProviderinfo_RowDeleting" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>

                            <asp:BoundField DataField="DetailLineNumber" HeaderText="Details" />
                            <asp:BoundField DataField="sak_short" HeaderText="NDC" />
                             <asp:TemplateField HeaderText="" ItemStyle-Width="100" ItemStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkNDC" runat="server" ToolTip="Search" Text='Search' CommandArgument='<%# Eval("DiagnosisCode") %>' OnClick="lnkNDCSearch_Click" Visible="true">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                            <asp:BoundField DataField="cde_unit_measure" HeaderText="Units of Measure" />
                            <asp:BoundField DataField="num_prescription_id" HeaderText="Prescription Number" />
                              <asp:BoundField DataField="amt_drug_unit_price" HeaderText="Drug Unit Price" />
                            <asp:BoundField DataField="qty_units_svc" HeaderText="Total Unit" />
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
                </div>--%>
                <div class="row" style="text-align: center;" runat="server" id="NDC">
                    <asp:ValidationSummary ID="vsAdditionalProviderserviceInfo" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="vgAdditionalProviderinfo" />
                    <div class="col-sm-6">
                        <div class="row" id="lblDetails" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Details</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlNdcDetails" EnableViewState="true" runat="server" AutoPostBack="true"
                                        AppendDataBoundItems="True" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true" OnSelectedIndexChanged="ddlDetails_SelectedIndexChanged">
                                    </asp:DropDownList>

                                </span>
                            </div>
                        </div>
                    </div>
                      <div class="col-sm-6">
                        <div class="row" id="lblProviderNPI" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">*NDC</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtNDC" runat="server" CssClass="formField" Style="height: 30px; width: 200px" />
                                     <asp:RequiredFieldValidator runat="server" ID="rfvNdc" SetFocusOnError="true"
                                ValidationGroup="valOwnerInfo" ControlToValidate="txtNDC" ErrorMessage="*NDC not in 11-digit format for detail N" Text="*" Display="Dynamic" InitialValue="0" />
                            
                                    </div>
                            </span>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lblUnitsOfMeasure" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">*Units of Measure</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlUnitsOfMeasure" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlUnitsOfMeasure_SelectedIndexChanged">
                                    </asp:DropDownList>
                                   <asp:RequiredFieldValidator runat="server" ID="rfvUnitsOfMeasure" SetFocusOnError="true"
                                ValidationGroup="valOwnerInfo" ControlToValidate="ddlUnitsOfMeasure" ErrorMessage="*Units of Measure is required" Text="*" Display="Dynamic" InitialValue="0" />
                                </span>
                            </div>
                        </div>
                    </div>
                  
                    <div class="col-sm-6">
                        <div class="row" id="lblPrescriptionNumber" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Prescription Number</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtPrescriptionNumber" runat="server" MaxLength="15" CssClass="formField" Style="height: 30px; width: 200px" />
                                     <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1"
                                ControlToValidate="txtPrescriptionNumber" ErrorMessage="*Prescription Number not reported for detail N" Text="*" Display="Dynamic"
                                SetFocusOnError="true" ValidationGroup="valPrescriptionNumber" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPrescriptionNumber" ErrorMessage="*Prescription number is required"
                                Text="*" Display="Dynamic" ValidationGroup="valPrescriptionNumber">
                            </asp:RequiredFieldValidator>
                            </div>
                            </span>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lblDrugUnitPrice" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Drug Unit Price</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtDrugUnitPrice" runat="server" MaxLength="25" CssClass="formField" Style="height: 30px; width: 200px" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvDrugUnitPrice1" SetFocusOnError="true"
                                ValidationGroup="valOwnerInfo" ControlToValidate="txtDrugUnitPrice" ErrorMessage="*Drug Unit Price not reported for detail N" Text="*" Display="Dynamic" InitialValue="0" />
                            <%-- <asp:RequiredFieldValidator ID="RfvDrugUnitPrice" runat="server" ControlToValidate="txtDrugUnitPrice" ErrorMessage="*Drug unit price is required"
                                 Text="*" Display="Dynamic" ValidationGroup="valDrugUnitPrice"></asp:RequiredFieldValidator>--%>
                                    </div>
                            </span>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lblTotalUnit" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Total Unit</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtTotalUnit" runat="server" MaxLength="11" CssClass="formField" Style="height: 30px; width: 200px" />
                             <asp:RequiredFieldValidator runat="server" ID="rfvTotalUnit1" SetFocusOnError="true"
                                ValidationGroup="valOwnerInfo" ControlToValidate="txtTotalUnit" ErrorMessage="*Total Unit not reported for detail N" Text="*" Display="Dynamic" InitialValue="0" />
                             <asp:RequiredFieldValidator ID="rfvTotalUnit" runat="server" ControlToValidate="txtTotalUnit" 
                                 ErrorMessage="*Total unit is required" Text="*" Display="Dynamic" 
                                 ValidationGroup="valTotalUnit"></asp:RequiredFieldValidator>
                                    </div>
                            </span>
                        </div>
                    </div>
               
                 <div class="col-sm-6 col-md-4 col-lg-3 ">
                    <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="NDCDetailsAdd_Click"
                        CssClass="button" CausesValidation="true" />
                    <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" CausesValidation="false" /> </div>
                </div>
                </div>
            </Content>
        </ajax:AccordionPane>
    </Panes>
</ajax:Accordion>

                  