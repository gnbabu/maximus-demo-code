<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_PriorAuthDentalServiceDetails, App_Web_c4une0e1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


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
        $("[id*=DentserviceAdd]").click(function () {
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

<ajax:Accordion ID="PriorAuthDentalServicesDetails" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionPanePriorAuthDentalServicesDetails" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblPriorAuthDentalServicesDetails" class="expandcollapse" runat="server" Text="- SERVICE DETAIL DENTAL"></asp:Label>
            </Header>
            <Content>

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvPriorAuthDentalServicesDetails" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvPriorAuthDentalServicesDetails_PageIndexChanging"
                        PageSize="10" OnRowDeleting="gvPriorAuthDentalServicesDetails_RowDeleting" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>
                            <asp:BoundField DataField="DetailLineNumber" HeaderText="Line" />
                            <asp:BoundField DataField="TypeOfServiceCode" HeaderText="*Service Code Type" />
                            <%--<asp:BoundField DataField="ToothNumCode" HeaderText="Tooth Number" />
                            <asp:BoundField DataField="ToothQuadrant" HeaderText="Quadrant" />
                            <asp:BoundField DataField="RequestedUnits" HeaderText="*Requested Unit" />--%>
                            <asp:BoundField DataField="AuthorizedUnits" HeaderText="Authorized Units" />
                            <%--<asp:BoundField DataField="RequestedDollars" HeaderText="*Requested Dollars" />--%>
                            <asp:BoundField DataField="AuthorizedDollars" HeaderText="Authorized Dollars" />
                            <%--<asp:BoundField DataField="ServiceStartDate" HeaderText="*Requested FDOS" />
                            <asp:BoundField DataField="ServiceEndDate" HeaderText="*Requested TDOS" />--%>
                            <asp:BoundField DataField="" HeaderText="Authorized From DOS" />
                            <asp:BoundField DataField="" HeaderText="Authorized To DOS" />
                            <asp:BoundField DataField="ServiceStatusCode" HeaderText="Status" />
                            <%--<asp:BoundField DataField="AmountUsed" HeaderText="Total Fees" />--%>
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
                        <div class="row" id="lblServiceCodeType" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Service Code Type</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlServiceCodeType" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlServiceCodeType_SelectedIndexChanged">
                                        
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="reqServiceCodeType" SetFocusOnError="true"
                            ValidationGroup="valProviderInfoHeader" ControlToValidate="ddlServiceCodeType" ErrorMessage="*Recipient is covered by managed care program" Text="*" Display="Dynamic" InitialValue="0" />
                                </span>
                            </div>
                        </div>
                       
                    </div>
                      <div class="col-sm-6">
                        <div class="row" id="Servicecode" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Service Code</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtServicecode" runat="server" CssClass="formField" />
                                     <asp:RequiredFieldValidator ID="rfvServicecode" runat="server" ControlToValidate="txtServicecode" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                           <asp:LinkButton ID="lnkHcps" runat="server" ToolTip="Search" OnClick="lnkHcpsSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="toothNumber" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Tooth Number</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txttoothNumber" runat="server" CssClass="formField" />
                                     <asp:RequiredFieldValidator ID="rfvtoothNumber" runat="server" ControlToValidate="txttoothNumber" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-6">
                        <div class="row" id="Quadrant" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Quadrant</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtQuadrant" runat="server" CssClass="formField" />
                                     <asp:RequiredFieldValidator ID="rfvQuadrant" runat="server" ControlToValidate="txtQuadrant" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                     <div class="col-sm-6">
                        <div class="row" id="dentalStatus" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Status</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtdentalStatus" runat="server" CssClass="formField" />
                                     <asp:RequiredFieldValidator ID="rfvdentalStatus" runat="server" ControlToValidate="txtdentalStatus" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                     <div class="col-sm-6">
                        <div class="row" id="dentalAssociatedPANumber" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Associated PA Number</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtdentalAssociatedPANumber" runat="server" CssClass="formField" />
                                    <%-- <asp:RequiredFieldValidator ID="rfvdentalAssociatedPANumber" runat="server" ControlToValidate="txtdentalAssociatedPANumber" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>--%>
                                </span>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-6">
                        <div class="row" id="DentalRequestUnt" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Requested Units</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtDentalRequestUnt" runat="server" CssClass="formField" />
                                    <%-- <asp:RequiredFieldValidator ID="rfvDentalRequestUnt" runat="server" ControlToValidate="txtDentalRequestUnt" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>--%>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="RequestedDollars" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Requested Dollars</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtRequestedDollars" runat="server" CssClass="formField" />
                                    <%-- <asp:RequiredFieldValidator ID="rfvRequestedDollars" runat="server" ControlToValidate="txtRequestedDollars" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>--%>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="ReqFDOS" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: left">Requested FDOS</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtDentalReqFDOS" runat="server" CssClass="formField" />
                                     <ajax:CalendarExtender ID="ceReqFDOS" TargetControlID="txtDentalReqFDOS" runat="server" />
                                    <asp:CompareValidator ID="cvReqFDOS" runat="server" ValidationGroup="valProviderInfoHeader"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDentalReqFDOS"
                                        ErrorMessage="Select a valid smaller date than today" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="RequestedTDOS" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Requested TDOS</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtDentalReqTDOS" runat="server" CssClass="formField" />
                                      <ajax:CalendarExtender ID="ceRequestedTDOS" TargetControlID="txtDentalReqTDOS" runat="server" />
                                    <asp:CompareValidator ID="cvRequestedTDOS" runat="server" ValidationGroup="valProviderInfoHeader"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDentalReqTDOS"
                                        ErrorMessage="Smaller date than today and Requested DOS" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="AuthorizedUnits" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Authorized Units</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtAuthorizedUnits" runat="server" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true" />
                                    <%-- <asp:RequiredFieldValidator ID="rfvAuthorizedUnits" runat="server" ControlToValidate="txtAuthorizedUnits" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>--%>
                                </span>
                            </div>
                        </div>
                    </div>
                     <div class="col-sm-6">
                        <div class="row" id="AuthorizedDollars" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Authorized Dollars</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtAuthorizedDollars" runat="server" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true" />
                                    <%-- <asp:RequiredFieldValidator ID="rfvAuthorizedDollars" runat="server" ControlToValidate="txtdentalLine" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>--%>
                                </span>
                            </div>
                        </div>
                    </div>
                     <div class="col-sm-6">
                        <div class="row" id="AuthorizedToDOS" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Authorized TDOS</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtAuthorizedToDOS" runat="server" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true" />
                                    <ajax:CalendarExtender ID="ceAuthorizedToDOS" TargetControlID="txtAuthorizedToDOS" runat="server" />
                                    <asp:CompareValidator ID="cvAuthorizedToDOS" runat="server" ValidationGroup="valProviderInfoHeader"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtAuthorizedToDOS"
                                        ErrorMessage="" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="AuthorizedFromDOS" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Authorized FDOS</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtAuthorizedFromDOS" runat="server" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true"  />
                                    <ajax:CalendarExtender ID="ceAuthorizedFromDOS" TargetControlID="txtAuthorizedFromDOS" runat="server" />
                                    <asp:CompareValidator ID="cvAuthorizedFromDOS" runat="server" ValidationGroup="valProviderInfoHeader"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtAuthorizedFromDOS"
                                        ErrorMessage="" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 ">
                            <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="DentserviceAdd_Click"
                                CssClass="button" CausesValidation="true" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" CausesValidation="false" />
                        </div>
                    </div>
            </Content>

        </ajax:AccordionPane>
    </Panes>

</ajax:Accordion>








