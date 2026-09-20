<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_PriorAuthServiceDetails, App_Web_l5y5araq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>


<script type="text/javascript">
    function checkDate(sender, args) {
        if (sender._selectedDate > new Date()) {
            alert("You cannot select a day earlier than today!");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
        }
    }
</script>
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
 <asp:UpdatePanel runat="server" ID="upAddnotes" UpdateMode="Always" EnableViewState="true"> 
          <ContentTemplate>   
<ajax:Accordion ID="PriorAuthServicesDetails" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true" >

    <Panes>
        <ajax:AccordionPane ID="AccordionPanePriorAuthServicesDetails" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
           

             <Header>
                <asp:Label ID="lblPriorAuthServicesDetails" class="expandcollapse" runat="server" Text="- SERVICE DETAIL"></asp:Label>
            </Header>
            <Content>
                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvPriorAuthServicesDetails" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvPriorAuthServicesDetails_PageIndexChanging"
                        PageSize="10" OnRowDeleting="gvPriorAuthServicesDetails_RowDeleting"  AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>

                            <asp:BoundField DataField="DetailLineNumber" HeaderText="Line" />
                            <asp:BoundField DataField="TypeOfServiceCode" HeaderText="Service Code Type" />
                            <asp:BoundField DataField="ServiceCode" HeaderText="Service Code" />
                            <%-- <asp:BoundField DataField="RequestedUnits" HeaderText="*Requested Unit" />--%>
                            <asp:BoundField DataField="AuthorizedUnits" HeaderText="Authorized Unit" />
                            <%--<asp:BoundField DataField="RequestedDollars" HeaderText="Requested Dollars" />--%>
                            <asp:BoundField DataField="" HeaderText="Authorized Dollars " />
                            <%--<asp:BoundField DataField="PRIOR_AUTH_SERVICE_DETAIL_REQUEST_UNITS_FEE" HeaderText="*Requested Unit Fees" />--%>
                            <%--<asp:BoundField DataField="ServiceStartDate" HeaderText="Requested_FDOS" />
                            <asp:BoundField DataField="ServiceEndDate" HeaderText="Requested TDOS" />--%>
                            <%-- getting error--%>
                             <%--<asp:BoundField DataField="AuthorizationDate" HeaderText="Authorized From DOS" />
                             <asp:BoundField DataField="AuthorizationEndDate" HeaderText="Authorized To DOS" />--%>
                            <%--<asp:BoundField DataField="AmountUsed" HeaderText="Total Fees" />--%>
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Button ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("DetailLineNumber")%>'
                                        CssClass="button" OnClick="btnDelete_Click" OnClientClick='return confirm("Are you sure you want to delete this record?");' CausesValidation="false" />
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
                        <div class="row" id="ServiceTypeCode" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">*Service Type Code</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlServiceTypeCode" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlServiceTypeCode_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <%--<asp:RequiredFieldValidator runat="server" ID="reqServiceCodeType" SetFocusOnError="true"
                            ValidationGroup="vgCarePlan" ControlToValidate="ddlServiceCodeType" ErrorMessage="*Recipient is covered by managed care program" Text="*" Display="Dynamic" InitialValue="0" />--%>
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
                                    <asp:TextBox ID="txtServiceCode" runat="server" CssClass="formField" />
                                     <asp:RequiredFieldValidator ID="rfvServiceCode" runat="server" ControlToValidate="txtServiceCode" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                        <asp:LinkButton ID="lnkHcps" runat="server" ToolTip="Search" OnClick="lnkServiceCodeSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="AssociatedPANumber" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Modifiers1</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtModifiers1" runat="server" CssClass="formField" MaxLength="2" />
                                     <asp:RequiredFieldValidator ID="rfvModifiers1" runat="server" ControlToValidate="txtAssociatedPANumber" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div2" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Modifiers2</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtModifiers2" runat="server" CssClass="formField" MaxLength="2" />
                                     <asp:RequiredFieldValidator ID="rfvModifiers2" runat="server" ControlToValidate="txtAssociatedPANumber" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div3" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Modifiers3</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtModifiers3" runat="server" CssClass="formField" MaxLength="2" />
                                     <asp:RequiredFieldValidator ID="rfvModifiers3" runat="server" ControlToValidate="txtAssociatedPANumber" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div4" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Modifiers4</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtModifiers4" runat="server" CssClass="formField" MaxLength="2" />
                                     <asp:RequiredFieldValidator ID="rfvModifiers4" runat="server" ControlToValidate="txtAssociatedPANumber" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div5" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">List Price</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtlistprice" runat="server" CssClass="formField" MaxLength="10" />
                                     <asp:RequiredFieldValidator ID="rfvlistprice" runat="server" ControlToValidate="txtAssociatedPANumber" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div6" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Pricing Formula</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:DropDownList ID="ddlPricingFormula" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                        AppendDataBoundItems="True" OnSelectedIndexChanged="ddlPricingFormula_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="reqPricingFormula" SetFocusOnError="true"
                            ValidationGroup="valProviderInfoHeader" ControlToValidate="ddlPricingFormula" ErrorMessage="*PricingFormula" Text="*" Display="Dynamic" InitialValue="0" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div7" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Number of Days</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtNumberdays" runat="server" CssClass="formField" />
                                     <asp:RequiredFieldValidator ID="rfvNumberdays" runat="server" ErrorMessage="*Number of Days" ControlToValidate="txtAssociatedPANumber" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div1" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Associated PA Number</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtAssociatedPAnum" runat="server" CssClass="formField" />
                                     <asp:RequiredFieldValidator ID="rfvAssociatedPAnum" runat="server" ControlToValidate="txtAssociatedPANumber" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="RequestUnt" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Requested Units</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtRequestUnt" runat="server" CssClass="formField" />
                                     <asp:RequiredFieldValidator ID="rfvRequestUnt" runat="server" ControlToValidate="txtRequestUnt" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
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
                                     <asp:RequiredFieldValidator ID="rfvAuthorizedUnits" runat="server" ControlToValidate="txtAuthorizedUnits" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
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
                                     <asp:RequiredFieldValidator ID="rfvRequestedDollars" runat="server" ControlToValidate="txtRequestedDollars" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
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
                                     <asp:RequiredFieldValidator ID="rfvAuthorizedDollars" runat="server" ControlToValidate="txtAuthorizedDollars" Text="*" Display="Dynamic" ValidationGroup="valProviderInfoHeader"></asp:RequiredFieldValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="ReqFDOS" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: left">*Requested FDOS</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtReqFDOS" runat="server" CssClass="formField" />
                                    <ajax:CalendarExtender ID="ceReqFDOS" TargetControlID="txtReqFDOS" runat="server" />
                                    <asp:CompareValidator ID="cvReqFDOS" runat="server" ValidationGroup="valOrgInfo"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtReqFDOS"
                                        ErrorMessage="Select a valid smaller date than today" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="RequestedTDOS" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">*Requested TDOS</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtReqTDOS" runat="server" CssClass="formField" />
                                    <ajax:CalendarExtender ID="ceRequestedTDOS" TargetControlID="txtReqTDOS" runat="server" />
                                    <asp:CompareValidator ID="cvRequestedTDOS" runat="server" ValidationGroup="valOrgInfo"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtReqTDOS"
                                        ErrorMessage="Smaller date than today and Requested DOS" Text="*" ValueToCompare="MM/dd/yyyy"
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
                                    <asp:TextBox ID="txtAuthorizedFromDOS" runat="server" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true" />
                                    <ajax:CalendarExtender ID="ceAuthorizedFromDOS" TargetControlID="txtAuthorizedFromDOS" runat="server" />
                                    <%-- <asp:CompareValidator ID="cvAuthorizedFromDOS" runat="server" ValidationGroup="valOrgInfo"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtAuthorizedFromDOS"
                                        ErrorMessage="" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>--%>
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
                                    <%-- <asp:CompareValidator ID="cvAuthorizedToDOS" runat="server" ValidationGroup="valOrgInfo"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtAuthorizedToDOS"
                                        ErrorMessage="" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>--%>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div8" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Billed Direct From Date</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtBilleddirFD" runat="server" CssClass="formField" />
                                    <ajax:CalendarExtender ID="cetBilleddirFD" TargetControlID="txtBilleddirFD" runat="server" />
                                    <%-- <asp:CompareValidator ID="cvtBilleddirFD" runat="server" ValidationGroup="valOrgInfo"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txttBilleddirFD"
                                        ErrorMessage="" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>--%>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div9" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Billed Direct To Date</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtBilleddirTD" runat="server" CssClass="formField" />
                                    <ajax:CalendarExtender ID="ceBilleddirTD" TargetControlID="txtBilleddirTD" runat="server" />
                                    <%-- <asp:CompareValidator ID="cvtBilleddirTD" runat="server" ValidationGroup="valOrgInfo"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtBilleddirTD"
                                        ErrorMessage="" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>--%>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div10" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Service/Rental From Date</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtservicerenFD" runat="server" CssClass="formField" />
                                    <ajax:CalendarExtender ID="ceservicerenFD" TargetControlID="txtservicerenFD" runat="server" />
                                    <%-- <asp:CompareValidator ID="cvtservicerenFD" runat="server" ValidationGroup="valOrgInfo"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtservicerenFD"
                                        ErrorMessage="" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>--%>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div11" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Service/Rental TO Date</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtservicerenTO" runat="server" CssClass="formField" />
                                    <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtservicerenTO" runat="server" />
                                    <%-- <asp:CompareValidator ID="cvtservicerenTO" runat="server" ValidationGroup="valOrgInfo"
                                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtservicerenTO"
                                        ErrorMessage="" Text="*" ValueToCompare="MM/dd/yyyy"
                                        SetFocusOnError="false"></asp:CompareValidator>--%>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="row" id="Div12" runat="server">
                            <div class="col-sm-5">
                                <span class="ohio-field" style="font-size: 15px; text-align: right">Total Fees</span>
                            </div>
                            <div class="col-sm-7">
                                <span style="text-align: left;">
                                    <asp:TextBox ID="txtTotalFees" runat="server" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true" />


                                </span>
                            </div>
                        </div>
                    </div>

                    <div class="col-sm-6 col-md-4 col-lg-3 ">
                        <asp:Button ID="btnAdd" Text="Add" runat="server" OnClick="serviceAdd_Click"
                            CssClass="button" CausesValidation="true" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </div>
 </Content>
         
           

        </ajax:AccordionPane>
    </Panes>

</ajax:Accordion>

              </ContentTemplate> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAdd"  EventName="Click"/>
        </Triggers>
    </asp:UpdatePanel>





