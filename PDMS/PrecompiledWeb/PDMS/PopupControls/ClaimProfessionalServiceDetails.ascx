<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ClaimProfessionalServiceDetails, App_Web_rqhgepvh" %>
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
        $("[id*=ClaimDentserviceAdd]").click(function () {
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

<ajax:Accordion ID="ClaimProfessionalServiceDetails" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">
    <Panes>
        <ajax:AccordionPane ID="AccordionPaneClaimProfessionalServiceDetails" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblClaimProfessionalServiceDetails" class="expandcollapse" runat="server" Text="- PROFESSIONAL SERVICE DETAIL "></asp:Label>
            </Header>
            <Content>

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvClaimProfessionalServiceDetails" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvClaimProfessionalServiceDetails_PageIndexChanging"
                        PageSize="10" OnRowDeleting="gvClaimProfessionalServiceDetails_RowDeleting" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>

                            <asp:BoundField DataField="DetailLineNumber" HeaderText="Detail Line" />
                            <asp:BoundField DataField="" HeaderText="Procedure Code" />
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnksearch" Text="Search" runat="server" ToolTip="Search" OnClick="lnkProcedureCodeSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="" HeaderText="Procedure Modifier" />

                            <asp:BoundField DataField="" HeaderText="*Diagnosis Pointer" />
                            <asp:BoundField DataField="" HeaderText="Place of Service" />
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnksearch" Text="Search" runat="server" ToolTip="Search" OnClick="lnkPlaceofserviceSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="" HeaderText="Referral EPSDT Service/Family Planning" />
                            <asp:BoundField DataField="" HeaderText="*Billed Units" />
                            <asp:BoundField DataField="" HeaderText="*Units of measurement" />
                            <asp:BoundField DataField="" HeaderText="*Date of Service" />
                            <asp:BoundField DataField="" HeaderText="*Charges" />
                            <asp:BoundField DataField="" HeaderText="Paid Units" />
                            <asp:BoundField DataField="" HeaderText="Paid Amount" />
                            <asp:BoundField DataField="" HeaderText="Status" />
                            <asp:BoundField DataField="" HeaderText="Total charges" />
                            <asp:BoundField DataField="" HeaderText="Total Amount Paid" />
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
                        <div class="row" id="ProcedureCode" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">*Procedure Code</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtproffProcedureCode" runat="server" CssClass="formField" pattern="([^\s][A-z0-9À-ž\s]+)" Style="height: 30px; width: 200px" />

                                    <asp:RequiredFieldValidator ID="rfvProcedureCode" runat="server" ControlToValidate="txtproffProcedureCode" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                        <%--<asp:LinkButton ID="lnksearch" runat="server" ToolTip="Search" OnClick="lnkProcedureCodeSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>--%>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lblModifier" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Procedure Modifier</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtprofModifier" runat="server" pattern="([^\s][A-z0-9À-ž\s]+)" CssClass="formField" Style="height: 30px; width: 200px" />

                                    <asp:RequiredFieldValidator ID="rfvModifier" runat="server"
                                        ControlToValidate="txtprofModifier" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                        <%--<asp:LinkButton ID="lnksearch" runat="server" ToolTip="Search" OnClick="lnkProcedureCodeSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>--%>
                    </div>
                  <div class="col-sm-6">
                        <div class="row" id="lblDiagnosisPointer" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">*Diagnosis Pointer</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtproffDiagnosisPointer" runat="server" MaxLength="4" CssClass="formField" Style="height: 30px; width: 200px" />
                                    <asp:RequiredFieldValidator ID="rfvDiagnosisPointer" runat="server" ControlToValidate="txtproffDiagnosisPointer" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="PlaceofService" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Place Of Service</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtprofPlaceofserv" runat="server" CssClass="formField" Style="height: 30px; width: 200px" />
                                    <asp:RequiredFieldValidator ID="rfvPlaceofserv" runat="server" ControlToValidate="txtprofPlaceofserv" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                      <div class="col-sm-6">
                        <div class="row" id="lblReferralEPSDTService" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Referral EPSDT Service/Family Planning</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlProffReferralEPSDTService" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlProffReferralEPSDTService_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="reqReferralEPSDTService" SetFocusOnError="true"
                                        ValidationGroup="valProviderInfoHeader" ControlToValidate="ddlProffReferralEPSDTService" ErrorMessage="*Referral EPSDT Service/Family Planning" Text="*" Display="Dynamic" InitialValue="0" />
                                </span>
                            </div>
                        </div>
                    </div>
                     <div class="col-sm-6">
                        <div class="row" id="BilledUnits" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">*Billed Units</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtproffBilledUnits" runat="server" CssClass="formField" Style="height: 30px; width: 200px" MaxLength="1" />
                                    <asp:RequiredFieldValidator runat="server" ID="rfvBilledUnits" SetFocusOnError="true"
                                        ValidationGroup="vgBilledUnits" ControlToValidate="txtproffBilledUnits" ErrorMessage="*Billed unit not reported for detail N" Text="*" Display="Dynamic" InitialValue="1" />
                                </span>
                            </div>
                        </div>
                    </div>
                      <div class="col-sm-6">
                        <div class="row" id="lblUnitsMea" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">*Units of measurement</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlproffUnitsMea" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlproffUnitsMea_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvddlUnitsMea" SetFocusOnError="true"
                                        ValidationGroup="valProviderInfoHeader" ControlToValidate="ddlproffUnitsMea" ErrorMessage="*Units of measurement is required" Text="*" Display="Dynamic" InitialValue="0" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Dateofservice" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">*Date of Services</span>
                            </div>
                            <div class="col-sm-7">

                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtProffdateofservice" runat="server" CssClass="formField" Style="height: 30px; width: 200px" />

                                    <ajax:CalendarExtender ID="cedateofservice" TargetControlID="txtProffdateofservice" runat="server" />
                                    <asp:CompareValidator ID="cvdateofservice" runat="server" ValidationGroup="valOrgInfo"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtProffdateofservice"
                                        ErrorMessage="Select a valid smaller date than today" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                      <div class="col-sm-6">
                        <div class="row" id="lblcharges" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">*Charges</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtproffCharges" runat="server" CssClass="formField" Style="height: 30px; width: 200px" />
                                    <asp:RequiredFieldValidator runat="server" ID="rfvcharges" SetFocusOnError="true"
                                        ValidationGroup="vgCharges" ControlToValidate="txtproffCharges" ErrorMessage="*Allows $0.00" Text="*" Display="Dynamic" InitialValue="1" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="lblPaidUnit" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Paid Units</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtproffPaidunits" runat="server" CssClass="formField" Style="height: 30px; width: 200px" />
                                    <%-- <asp:RequiredFieldValidator ID="rfvpaidunits" runat="server" ControlToValidate="txtPaidunits" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>--%>
                                </span>
                            </div>
                        </div>
                    </div>
                     <div class="col-sm-6">
                        <div class="row" id="PaidAmount" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Paid Amount</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtproffPaidAmount" runat="server" CssClass="formField" Style="height: 30px; width: 200px" />
                                    <%-- <asp:RequiredFieldValidator ID="rfvPaidAmount" runat="server" ControlToValidate="txtPaidAmount" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>--%>
                                </span>
                            </div>
                        </div>
                    </div>
                  <div class="col-sm-6">
                        <div class="row" id="ClaimStatus" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Status</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtproffclaimsStatus" runat="server" CssClass="formField" Style="height: 30px; width: 200px" />
                                    <%-- <asp:RequiredFieldValidator ID="rfvclaimsStatus" runat="server" ControlToValidate="txtclaimsStatus" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>--%>
                                </span>
                            </div>
                        </div
>
                    </div>
                      <div class="col-sm-6">
                        <div class="row" id="TotalCharges" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Total Charges:</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtProfftotalCharges" runat="server" CssClass="formField" Style="height: 30px; width: 200px" />
                                    

                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="TotalAmountPaid" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Total Amount Paid</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtProfftotalamountpaid" runat="server" Style="background-color: lightgrey; height: 30px; width: 200px" CssClass="formField" ReadOnly="true" />

                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="ClaimProfessionalServiceDetailsAdd_Click"
                            CssClass="button" CausesValidation="true" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </div>
            </Content>
        </ajax:AccordionPane>
    </Panes>
</ajax:Accordion>
