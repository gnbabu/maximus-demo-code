<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_SubmitClaimServiceDetails" Codebehind="SubmitClaimServiceDetails.ascx.cs" %>
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
   
<ajax:Accordion ID="SubmitClaimServiceDetails" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true" >

    <Panes>
        <ajax:AccordionPane ID="AccordionPaneSubmitClaimServiceDetails" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
           

             <Header>
                <asp:Label ID="lblSubmitClaimServiceDetails" class="expandcollapse" runat="server" Text="- SERVICE DETAIL"></asp:Label>
            </Header>
            <Content>
                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvSubmitClaimServiceDetails" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvSubmitClaimServiceDetails_PageIndexChanging"
                        PageSize="10" OnRowDeleting="gvSubmitClaimServiceDetails_RowDeleting"  AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>

                            <asp:BoundField DataField="PRIOR_AUTH_CLAIM_SERVICEDETAIL_DETAILITEM" HeaderText="Detail" />
                    <asp:BoundField DataField="PRIOR_AUTH_CLAIM_SERVICEDETAIL_PROCEDURECODE" HeaderText="*Procedure Code" />
                    <asp:BoundField DataField="PRIOR_AUTH_CLAIM_SERVICEDETAIL_PROCEDUREMODIFIER" HeaderText="Procedure Modifier" />
                    <asp:BoundField DataField="PRIOR_AUTH_CLAIM_SERVICEDETAIL_DIAGNOSISPOINTER" HeaderText="*Diagnosis Pointer" />
                    <asp:BoundField DataField="PRIOR_AUTH_CLAIM_SERVICEDETAIL_PLACEOFSERVICE" HeaderText="Place of Service" />
                    <asp:BoundField DataField="PRIOR_AUTH_CLAIM_EPSDT_ID" HeaderText="Referral EPSDT Service/Family Planning" />
                    <asp:BoundField DataField="PRIOR_AUTH_CLAIM_SERVICEDETAIL_BILLEDUNITS" HeaderText="Billed Units" />
                    <asp:BoundField DataField="PRIOR_AUTH_CLAIM_SERVICEDETAIL_UNITOFMEASUREMENT" HeaderText="Units of measurement" />
                    <asp:BoundField DataField="PRIOR_AUTH_CLAIM_SERVICEDETAIL_DATEOFSERVICE" HeaderText="Date of Service" />
                    <asp:BoundField DataField="PRIOR_AUTH_CLAIM_SERVICEDETAIL_CHARGES" HeaderText="Charges" />
                    <asp:BoundField DataField="PPRIOR_AUTH_CLAIM_SERVICEDETAIL_PAIDAMOUNT" HeaderText="Paid Amount" />
                    <asp:BoundField DataField="PRIOR_AUTH_CLAIM_SERVICEDETAIL_TOTALCHARGES" HeaderText="Total Charges" />
                    <asp:BoundField DataField="PRIOR_AUTH_CLAIM_SERVICEDETAIL_TOTALAMOUNTPAID" HeaderText="Total Amount Paid" />
                    <asp:BoundField DataField="PRIOR_AUTH_STATUS_ID" HeaderText="Status" />
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

 <div class="row serviceDetail" runat="server">
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblDetailItem" runat="server" Text="Detail Item" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-3 text-left">
                            <asp:TextBox ID="txtDetailItem" runat="server" Style="background-color: lightgrey;" CssClass="formField" ReadOnly="true" />

                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblRevenueCode" runat="server" Text="*Revenue Code" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-3 text-left">
                            <asp:TextBox ID="txtRevenueCode" runat="server" CssClass="formField" MaxLength="3" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvRevenueCode" SetFocusOnError="true"
                                ValidationGroup="vgRevenueCode" ControlToValidate="txtRevenueCode" ErrorMessage="*3 digit required" Text="*" Display="Dynamic" InitialValue="0" />
                            <asp:LinkButton ID="lnkRevenueCodeSearch" runat="server" Text="Search" ToolTip="Search" OnClick="lnkRevenueCodeSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>&nbsp;&nbsp;
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblProcedureType" runat="server" Text="*Procedure Type" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-3 text-left">
                            <asp:DropDownList ID="ddlProcedureType" CssClass="formField" EnableViewState="true" runat="server" AutoPostBack="true"
                                AppendDataBoundItems="True" OnSelectedIndexChanged="ddlProcedureType_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator runat="server" ID="rfvProcedureType" SetFocusOnError="true"
                                ValidationGroup="vgProcedureType" ControlToValidate="ddlProcedureType" ErrorMessage="*Procedure type is required" Text="*" Display="Dynamic" InitialValue="0" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblProcedureCode" runat="server" Text="*Procedure Code" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-3 text-left">
                            <asp:TextBox ID="txtServiceProcedureCode" runat="server" CssClass="formField" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvProcedureCode" SetFocusOnError="true"
                                ValidationGroup="vgProcedureCode" ControlToValidate="txtServiceProcedureCode" ErrorMessage="*Procedure code is required" Text="*" Display="Dynamic" InitialValue="0" />
                            <asp:LinkButton ID="lnkProcedureCodeSearch" runat="server" Text="Search" ToolTip="Search" OnClick="lnkProcedureCodeSearch_Click" OnClientClick="exportPopup();" Visible="true"></asp:LinkButton>&nbsp;&nbsp;
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblProcedureModifier" runat="server" Text="*Procedure Modifier" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-3 text-left">
                            <asp:TextBox ID="txtProcedureModifier" runat="server" CssClass="formField" />
                            <%--<asp:RequiredFieldValidator runat="server" ID="rfvProcedureModifier" SetFocusOnError="true"
                                ValidationGroup="vgProcedureModifier" ControlToValidate="txtProcedureModifier" ErrorMessage="*Procedure code is required" Text="*" Display="Dynamic" InitialValue="0" />--%>
                        </div>


                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblUnits" runat="server" Text="*Units" CssClass="formLabel200 " />
                        </div>
                        <div class="col-sm-3 text-left">
                            <asp:TextBox ID="txtUnits" runat="server" CssClass="formField" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblunitofMeasurment" runat="server" Text="Unit of Measurement" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-3 text-left">
                            <asp:TextBox ID="txtunitofMeasurment" runat="server" CssClass="formField" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblFromDos" runat="server" Text="*From DOS" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtFromDos" runat="server" CssClass="formField" />
                            <ajax:CalendarExtender ID="ceFromDate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtFromDos"
                                PopupPosition="BottomRight" CssClass="QstCalendarCSS" PopupButtonID="imgFromDate" EnabledOnClient="true" />
                            <asp:CompareValidator ID="cvFd" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtFromDos" ValidationGroup="VldGrpFromDos"
                                ErrorMessage="Select a valid To date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy" SetFocusOnError="true"> 
                            </asp:CompareValidator>
                            <asp:Image ID="imgFromDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                            <asp:RequiredFieldValidator ID="RFVFromDos" runat="server" ControlToValidate="txtFromDos"
                                ErrorMessage="From date is required" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo" />
                            <asp:CustomValidator ID="cvfd2" runat="server" ControlToValidate="txtFromDos" ErrorMessage="Select a From Dos is required."
                                Display="Dynamic" Text="*" ValidationGroup="VldFromDos" OnServerValidate="ReportFromDos_ServerValidate" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblToDos" runat="server" Text="*To DOS" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtToDos" runat="server" CssClass="formField" />
                            <ajax:CalendarExtender ID="ceToDos" runat="server" Format="MM/dd/yyyy" TargetControlID="txtToDos"
                                PopupPosition="BottomRight" CssClass="QstCalendarCSS" PopupButtonID="imgToDos" EnabledOnClient="true" />
                            <asp:CompareValidator ID="cvToDoss" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtToDos" ValidationGroup="VldGrpToDos"
                                ErrorMessage="Select a valid To date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy" SetFocusOnError="true"> 
                            </asp:CompareValidator>
                            <asp:Image ID="imgtoDos" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtToDos"
                                ErrorMessage="TO date is required" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo" />
                            <asp:CustomValidator ID="cvToDos" runat="server" ControlToValidate="txtToDos" ErrorMessage="Select a From Dos is required."
                                Display="Dynamic" Text="*" ValidationGroup="VldToDos" OnServerValidate="ReportToDos_ServerValidate" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                            <asp:Label ID="lblTotaCharges" Text="*Total Charges" runat="server" class="formLabel200" />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtTotaCharges" runat="server" CssClass="formField" MaxLength="15" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvAmount"
                                ControlToValidate="txtTotaCharges" ErrorMessage="*Enter TotaCharges" Text="*" Display="Dynamic"
                                SetFocusOnError="true" ValidationGroup="valTotaCharges" />
                            <asp:RegularExpressionValidator ID="revAmount" runat="server" ControlToValidate="txtTotaCharges"
                                ValidationExpression="^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$" ErrorMessage="*Enter TotaCharges" Text="*" Display="Dynamic"
                                ValidationGroup="valTotaCharges" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                            <asp:Label ID="lblNonCoveredCharges" Text="Non-Covered Charges" runat="server" class="formLabel200" />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtNonCoveredCharges" runat="server" CssClass="formField" MaxLength="15" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvNonCoveredCharges"
                                ControlToValidate="txtTotaCharges" ErrorMessage="*Enter NonCoveredCharges" Text="*" Display="Dynamic"
                                SetFocusOnError="true" ValidationGroup="valNonCoveredCharges" />
                            <asp:RegularExpressionValidator ID="revNonCoveredCharges" runat="server" ControlToValidate="txtNonCoveredCharges"
                                ValidationExpression="^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$" ErrorMessage="*Enter TotaCharges" Text="*" Display="Dynamic"
                                ValidationGroup="valNonCoveredCharges" />
                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                            <asp:Label ID="lblMedicaidAllowedAmount" Text=" Medicaid Allowed Amount" runat="server" class="formLabel200" />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtMedicaidAllowedAmount" runat="server" CssClass="formField" MaxLength="15" />
                            <asp:RequiredFieldValidator runat="server" ID="rfvMedicaidAllowedAmount"
                                ControlToValidate="txtMedicaidAllowedAmount" ErrorMessage="*Enter Medicaid Allowed Amount" Text="*" Display="Dynamic"
                                SetFocusOnError="true" ValidationGroup="valMedicaidAllowedAmount" />
                            <asp:RegularExpressionValidator ID="revMedicaidAllowedAmount" runat="server" ControlToValidate="txtMedicaidAllowedAmount"
                                ValidationExpression="^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$" ErrorMessage="*Enter TotaCharges" Text="*" Display="Dynamic"
                                ValidationGroup="valMedicaidAllowedAmount" />
                        </div>

                        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                            <asp:Label ID="lblPaidAmount" Text="Paid Amount" runat="server" class="formLabel200" />
                        </div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtServPaidAmount" runat="server" CssClass="formField" MaxLength="15" />
                            <%-- <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3"
                                ControlToValidate="txtPaidAmount" ErrorMessage="*Enter Paid Amount" Text="*" Display="Dynamic"
                                SetFocusOnError="true" ValidationGroup="valPaidAmount" />--%>
                            <asp:RegularExpressionValidator ID="revPaidAmount" runat="server" ControlToValidate="txtServPaidAmount"
                                ValidationExpression="^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$" ErrorMessage="*Enter Paid Amount" Text="*" Display="Dynamic"
                                ValidationGroup="valPaidAmount" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblclaimStatus" runat="server" Text="Status" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-3 text-left">
                            <asp:TextBox ID="txtClaimStatus" runat="server" CssClass="formField" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblToalCharges" runat="server" Text="Total charges:" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-3 text-left">
                            <asp:Label ID="lblTotalCharges2" runat="server" Text="" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-6 col-md-4 col-lg-3 text-left">
                            <asp:Label ID="lblTotalAmountPaid" runat="server" Text="Total Amount Paid" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-3 text-left">
                            <asp:Label ID="lblTotalAmountPaid2" runat="server" Text="" CssClass="formLabel200" />
                        </div>
               
            </div>
        </div>
 </Content>
         
           

        </ajax:AccordionPane>
    </Panes>

</ajax:Accordion>

              </ContentTemplate> 
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAdd"  EventName="Click"/> 
            <asp:AsyncPostBackTrigger  ControlID="btnCancel" EventName="Click"/> 
        </Triggers>
    </asp:UpdatePanel>



