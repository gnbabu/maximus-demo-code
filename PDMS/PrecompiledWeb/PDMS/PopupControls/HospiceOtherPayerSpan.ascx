<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_HospiceOtherPayerSpan, App_Web_rqhgepvh" %>

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
        $("[id*=gvHospiceOtherPayerSpan] [id*=ftnAdd]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["fddlPayerType", "ftxtPayerName", "ftxtEffectiveDate", "ftxtEndDate"];
            var isValid = RequiredFieldsValidations(row, requiredControles);

            var ftxtEffectiveDate = $.trim(row.find("[id*=ftxtEffectiveDate]").val());
            var ftxtEndDate = $.trim(row.find("[id*=ftxtEndDate]").val());
            if (isValid === true) {
                isValid = DateValidations(ftxtEffectiveDate, ftxtEndDate, "PSErrorMessage");
            }
            return isValid;
        });
        $("[id*=gvHospiceOtherPayerSpan] [id*=btnUpdate]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["eddlPayerType", "etxtPayerName", "etxtEffectiveDate", "etxtEndDate"];
            var isValid = RequiredFieldsValidations(row, requiredControles);

            var ftxtEffectiveDate = $.trim(row.find("[id*=etxtEffectiveDate]").val());
            var ftxtEndDate = $.trim(row.find("[id*=etxtEndDate]").val());
            if (isValid === true) {
                isValid = DateValidations(ftxtEffectiveDate, ftxtEndDate, "PSErrorMessage");
            }
            return isValid;
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
    function BenefitSegmentIndicatorChange(ctrl) {
        var selectedItem = $(ctrl).find('option:selected').text();
        var hdCntrl = $(ctrl).closest('tr').find('td input[id*="hdnBenefitSegmentIndicator"]');
        hdCntrl.val(selectedItem);
    }

    function PayerTypeChange(ctrl) {
        var selectedItem = $(ctrl).find('option:selected').val();
        var hdCntrl = $(ctrl).closest('tr').find('td input[id*="hdnPayerType"]');
        hdCntrl.val(selectedItem);
    }
    //added review it
    function OtherPayerSpanDateValidations(effectiveDate, endDate) {
        if (Date.parse(effectiveDate) > Date.parse(endDate)) {
            $("[id*=PSErrorMessage]").text("Effective Date Should be less than or equal to End date.");
            return false;
        }
        if (Date.parse(endDate) < Date.parse(effectiveDate)) {
            $("[id*=PSErrorMessage]").text("EndDate Should be grater than or equal to EffectvieData.");
            return false;
        }

        return true;
    }
</script>
<ajax:Accordion ID="AccordionHospiceAttendingPhysician" runat="Server" SelectedIndex="-1" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionHospiceOtherPayerSpan" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblHospiceOtherPayerSpan" class="expandcollapse" runat="server" Text="+ HOSPICE OTHER PAYER SPAN"></asp:Label>
            </Header>
            <Content>

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvHospiceOtherPayerSpan" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvHospiceOtherPayerSpan_PageIndexChanging"
                        OnRowEditing="gvHospiceOtherPayerSpan_RowEditing" OnRowUpdating="gvHospiceOtherPayerSpan_RowUpdating" OnRowCancelingEdit="gvHospiceOtherPayerSpan_RowCancelingEdit"
                        OnRowDeleting="gvHospiceOtherPayerSpan_RowDeleting" OnRowDataBound="gvHospiceOtherPayerSpan_RowDataBound" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>
                           <%-- <asp:TemplateField HeaderText="Segment Indicator" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenefitSegmentIndicator" runat="server" Text='<%# Bind("SegmentIndicator") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:HiddenField ID="hdnBenefitSegmentIndicator" runat="server" Value='<%#Bind("SegmentIndicator") %>' />
                                    <asp:DropDownList ID="eddlBenefitSegmentIndicator" runat="server" onchange="javascript:BenefitSegmentIndicatorChange(this);"></asp:DropDownList>
                                    <span style="color:red; display:none"><br />Segment Indicator is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="fddlBenefitSegmentIndicator" runat="server"></asp:DropDownList>
                                    <span style="color:red; display:none"><br />Segment Indicator is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="Payer Type" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblPayerType" runat="server" Text='<%# Bind("PayerType") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:HiddenField ID="hdnPayerType" runat="server" Value='<%#Bind("PayerType") %>' />
                                    <asp:DropDownList ID="eddlPayerType" runat="server" onchange="javascript:PayerTypeChange(this);"></asp:DropDownList>
                                    <span style="color:red; display:none"><br />Payer type is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="fddlPayerType" runat="server"></asp:DropDownList>
                                    <span style="color:red; display:none"><br />Payer type is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Payer Name" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblPayerName" runat="server" Text='<%# Bind("PayerName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtPayerName" runat="server" Text='<%# Bind("PayerName") %>'></asp:TextBox>
                                    <span style="color:red; display:none"><br />Payer name is required</span>
                                    </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtPayerName" runat="server"></asp:TextBox>
                                      <span style="color:red; display:none"><br />Payer name is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>

                             <asp:TemplateField HeaderText="Effective Date" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Bind("PayerEffDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtEffectiveDate" autocomplete="off" runat="server" Text='<%# Bind("PayerEffDate", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                     <ajax:CalendarExtender ID="cleffDate" TargetControlID="etxtEffectiveDate" runat="server" EnabledOnClient="true" />
                                <span style="color:red; display:none"><br />Effective date is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtEffectiveDate" autocomplete="off" runat="server"></asp:TextBox>
                                     <ajax:CalendarExtender ID="clefffDate" TargetControlID="ftxtEffectiveDate" runat="server" EnabledOnClient="true" />
                                    <span style="color:red; display:none"><br />Effective date is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="End Date" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("PayerEndDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtEndDate" autocomplete="off" runat="server" Text='<%# Bind("PayerEndDate", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                     <ajax:CalendarExtender ID="cletxtEndDate" TargetControlID="etxtEndDate" runat="server" EnabledOnClient="true" />
                                    <span style="color:red; display:none"><br />End date is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtEndDate" autocomplete="off" runat="server"></asp:TextBox>
                                     <ajax:CalendarExtender ID="clftxtEndDate" TargetControlID="ftxtEndDate" runat="server" EnabledOnClient="true"/>
                                    <span style="color:red; display:none"><br />End date is required</span>
                                    <asp:CompareValidator ID="CompareValidatorEffEndDate" ValidationGroup="ComparingDate" ForeColor="Red" runat="server"
                                        ControlToValidate="ftxtEffectiveDate" ControlToCompare="ftxtEndDate" Operator="LessThan" Type="Date"
                                        ErrorMessage="End date must be Greater than Effective date."></asp:CompareValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CssClass="button" CausesValidation="false"  />
                                    &nbsp;
                <asp:Button ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" Visible='<%# Convert.ToBoolean(Eval("IsFromInquiry")) == true ? false : true %>'
                    CssClass="button" OnClientClick='return confirm("Are you sure you want to delete this record?");' CausesValidation="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button ID="btnUpdate" Text="Update" runat="server" CommandName="Update" CssClass="button" CausesValidation="true" />
                                    &nbsp;
                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" CssClass="button" CausesValidation="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="ftnAdd" runat="server" Text="Add New" OnClick="fbtnAdd_Click" CssClass="button" Style="width: auto !important;" CausesValidation="true" ValidationGroup="ComparingDate"/>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                     <div style="text-align: center">
                                <span id="PSErrorMessage" style="color: red;"></span>
                            </div>
                </div> 
            </Content>

        </ajax:AccordionPane>
    </Panes>

</ajax:Accordion>
