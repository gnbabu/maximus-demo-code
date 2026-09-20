<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SubmitClaimAdditionalProviderInformation_, App_Web_yvhxe4ml" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<ajax:Accordion ID="SubmitClaimAdditionalProviderInformation" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AdditionalProviderinfo" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblAdditionalProviderinfo" class="expandcollapse" runat="server" Text="- ADDITIONAL PROVIDER INFORMATION-SERVICE LEVEL"></asp:Label>
            </Header>
            <Content> 
                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvAdditionalProviderinfo" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvAdditionalProviderinfo_PageIndexChanging"
                        PageSize="10" OnRowDeleting="gvTAdditionalProviderinfo_RowDeleting" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>

                            <asp:BoundField DataField="DetailLineNumber" HeaderText="Detail Line" />
                            <asp:BoundField DataField="" HeaderText="Provider Type" />
                            <asp:BoundField DataField="" HeaderText="Provider NPI" />
                            <asp:BoundField DataField="" HeaderText="Last Name" />
                              <asp:BoundField DataField="" HeaderText="First Name, MI" />
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
                <div class="row" style="text-align: center; padding-left:20px; padding-right:20px;">
                    <asp:ValidationSummary ID="vsAdditionalProviderserviceInfo" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="vgAdditionalProviderinfo" />
               
                       <div class="col-sm-6">
                        <div class="row" id="lblDetails" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Details</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlDetails" EnableViewState="true" runat="server" AutoPostBack="true"
                                        AppendDataBoundItems="True" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true" OnSelectedIndexChanged="ddlDetails_SelectedIndexChanged">
                                    </asp:DropDownList>

                                </span>
                            </div>
                        </div>
                    </div>
                    
                    <div class="col-sm-6">
                        <div class="row" id="lblProviderType" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Provider Type</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlProviderType" CssClass="formField" EnableViewState="true" runat="server"
                                        AppendDataBoundItems="True">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rvProviderType" SetFocusOnError="true"
                                ValidationGroup="valProviderHeader" ControlToValidate="ddlProviderType" ErrorMessage="*Provider Type" Text="*" Display="Dynamic" InitialValue="0" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <%--    <div class="col-lg-2">
                        <span class="ohio-field" style="font-size: 15px; text-align: left">Details</span>
                        <div style="text-align: left;">
                            <asp:DropDownList ID="ddlDetails" EnableViewState="true" runat="server" AutoPostBack="true"
                                AppendDataBoundItems="True" Style="background-color: lightgrey; width: 150px; min-width:150px;" CssClass="formField" ReadOnly="true" OnSelectedIndexChanged="ddlDetails_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>--%>
                  <%--  <div class="col-lg-2">
                        <span class="ohio-field" style="font-size: 15px; text-align: left">Provider Type</span>
                        <div style="text-align: left;">
                            <asp:DropDownList ID="ddlProviderType" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                AppendDataBoundItems="True" OnSelectedIndexChanged="ddlProviderType_SelectedIndexChanged"  style="width: 150px; min-width:150px;">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rvProviderType" SetFocusOnError="true"
                                ValidationGroup="valProviderHeader" ControlToValidate="ddlProviderType" ErrorMessage="*Provider Type" Text="*" Display="Dynamic" InitialValue="0" />
                        </div>
                    </div>--%>
                    <div class="col-lg-2">
                             <span class="ohio-field" style="font-size: 15px; text-align: left">Provider NPI</span>
                            <div style="text-align: left;">
                                <asp:TextBox ID="txtProviderNPI" runat="server" CssClass="formField" Style="height: 30px; width: 150px; min-width:150px;" />
                                <asp:RequiredFieldValidator runat="server" ID="rfvProviderNPI1" SetFocusOnError="true"
                                    ValidationGroup="valOwnerInfo" ControlToValidate="txtProviderNPI" ErrorMessage="Incorrect NPI for detail N in additional provider Information" Text="*" Display="Dynamic" InitialValue="0" />
                                <asp:RequiredFieldValidator ID="rfvProviderNPI" runat="server" ControlToValidate="txtProviderNPI" ErrorMessage="Provider NPI required for detail N in additional provider panel" Text="*" Display="Dynamic" ValidationGroup="valDrugUnitPrice"></asp:RequiredFieldValidator>
                            </div>
                    </div>
                    <div class="col-lg-2">
                            <span class="ohio-field" style="font-size: 15px; text-align: left">Last Name</span>
                            <div style="text-align: left;">
                                <asp:TextBox ID="txtProviderLastName" runat="server" MaxLength="25" CssClass="formField" Style="height: 30px; width: 150px; min-width:150px;" />
                                <%--  <asp:RequiredFieldValidator runat="server" ID="rfvProviderLastName"
                                        ControlToValidate="txtPrescriptionNumber" ErrorMessage="*" Text="*" Display="Dynamic"
                                        SetFocusOnError="true" ValidationGroup="valPrescriptionNumber" />
                                    <asp:RequiredFieldValidator ID="rfvPrescriptionNumber" runat="server"
                                        ControlToValidate="txtPrescriptionNumber" ErrorMessage="*"
                                        Text="*" Display="Dynamic" ValidationGroup="valLast Name"></asp:RequiredFieldValidator>--%>
                                </div>
                    </div>
                    <div class="col-lg-2">
                        <span class="ohio-field" style="font-size: 15px; text-align: left">First Name</span>
                        <div style="text-align: left;">
                                <asp:TextBox ID="txtProviderFirstName" runat="server" MaxLength="25" CssClass="formField" Style="height: 30px; min-width: 150px;width: 150px;" />
                        </div>
                    </div>
                    <div class="col-lg-1">
                        <span class="ohio-field" style="font-size: 15px; text-align: left">MI</span>
                        <div style="text-align: left;">
                            <asp:TextBox ID="txtProviderMI" runat="server" MaxLength="25" CssClass="formField" Style="height: 30px; min-width: 50px; width: 50px" />
                        </div>
                    </div>
                    <div class="col-lg-1 ">
                        <span class="ohio-field" style="font-size: 15px; text-align: left">&nbsp;</span>
                        <div style="text-align: left;">
                            <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="AdditionalProviderinfoAdd_Click" CssClass="btn btn-primary" Width="60px" CausesValidation="true" />
                            <asp:Button ID="Button1" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" CausesValidation="false" Visible="false" />
                        </div>
                    </div>
                </div>
            </Content>
        </ajax:AccordionPane>
    </Panes>
</ajax:Accordion>


