<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_HospiceProviderServiceSpan, App_Web_l5y5araq" %>

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
        $("[id*=gvHospiceProviderServiceSpan] [id*=ftnAdd]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["fddlBenefitLineNo", "ftxtEffectiveDate", "ftxtEndDate"];
            var isValid = RequiredFieldsValidations(row, requiredControles);
            var effectvieData = $.trim(row.find("[id*=ftxtEffectiveDate]").val());
            var endData = $.trim(row.find("[id*=ftxtEndDate]").val());
            var segmentIndecator = $.trim(row.find("[id*=fddlBenefitLineNo] option:selected").text());
            if (isValid === true) {
                isValid = ServiceSpanDateValidations(effectvieData, endData);
            }
            if (isValid === true) {
                isValid = DateValidationsOverlap(effectvieData, endData, "gvHospiceProviderServiceSpan", "ServiceSpanErrorMessage");
            }
            if (isValid === true) {
                isValid = CompareWithBenefitPeriod(effectvieData, endData, segmentIndecator, "gvHospiceProviderServiceSpan", "ServiceSpanErrorMessage");
            }
            if (isValid === true) {
                row.find("[id*=ftxtEffectiveDate]").attr("disabled", false);
                row.find("[id*=ftxtEndDate]").attr("disabled", false);
            }
            return isValid;
        });
        $("[id*=gvHospiceProviderServiceSpan] [id*=btnUpdate]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["eddlBenefitLineNo", "etxtEffectiveDate", "etxtEndDate"];
            var isValid = RequiredFieldsValidations(row, requiredControles);
            var effectvieData = $.trim(row.find("[id*=etxtEffectiveDate]").val());
            var endData = $.trim(row.find("[id*=etxtEndDate]").val());
            var segmentIndecator = $.trim(row.find("[id*=eddlBenefitLineNo] option:selected").text());
            if (isValid == true) {
                isValid = ServiceSpanDateValidations(effectvieData, endData);
            }
            if (isValid === true) {
                isValid = CompareWithBenefitPeriod(effectvieData, endData, segmentIndecator, "gvHospiceProviderServiceSpan", "ServiceSpanErrorMessage");
            }
            if (isValid == true) {
                row.find("[id*=etxtEffectiveDate]").attr("disabled", false);
                row.find("[id*=etxtEndDate]").attr("disabled", false);
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
    function ServiceSpanDateValidations(effectiveDate, endDate) {
        if (Date.parse(effectiveDate) > Date.parse(endDate)) {
            $("[id*=ServiceSpanErrorMessage]").text("Effective Date Should be less than or equal to End date.");
            return false;
        }
        if (Date.parse(endDate) < Date.parse(effectiveDate)) {
            $("[id*=ServiceSpanErrorMessage]").text("EndDate Should be grater than or equal to EffectvieData.");
            return false;
        }

        return true;
    }
    function BenefitSegmentIndicatorChange(ctrl) {
        var selectedItem = $(ctrl).find('option:selected').text();
        var hdCntrl = $(ctrl).closest('tr').find('td input[id*="hdnBenefitSegmentIndicator"]');
        hdCntrl.val(selectedItem);
    }
    function BenefitLineNumberChangeProviderServicePanel(ctrl, action) {
        var selectedItem = $(ctrl).find('option:selected').text();
        var hdCntrl = $(ctrl).closest('tr').find('td input[id*="hdnBenefitLineNo"]');
        hdCntrl.val(selectedItem);
        var selectedVal = $(ctrl).find('option:selected').val();
        var benefit = selectedVal.split(';');
        var flblSegmentBenefitType = (action === 'Edit') ? $(ctrl).closest('tr').find("[id*=elblSegmentBenefitType]") : $(ctrl).closest('tr').find(" [id*=flblSegmentBenefitType]");
        var flblDateBenefitPeriod = (action === 'Edit') ? $(ctrl).closest('tr').find("[id*=elblDateBenefitPeriod]") : $(ctrl).closest('tr').find(" [id*=flblDateBenefitPeriod]");
        var txtEffectiveDate = (action === 'Edit') ? $(ctrl).closest('tr').find("[id*=etxtEffectiveDate]") : $(ctrl).closest('tr').find(" [id*=ftxtEffectiveDate]");
        var txtEndDate = (action === 'Edit') ? $(ctrl).closest('tr').find("[id*=etxtEndDate]") : $(ctrl).closest('tr').find(" [id*=ftxtEndDate]");
        if (benefit.length > 1) {
            flblSegmentBenefitType.html(benefit[0]);
            flblDateBenefitPeriod.html(benefit[1]);
            var benefitdates = benefit[1].split('-');
            txtEffectiveDate.val(benefitdates[0]);
            txtEndDate.val(benefitdates[1]);
            txtEffectiveDate.attr("disabled", true);
            txtEndDate.attr("disabled", true);
            var actionType = $("[id*=ddlHospiceApplicationType]").val();
            if (actionType === "CHGPR") {
                txtEffectiveDate.attr("disabled", false);
            }
            if (actionType === "CLSPR") {
                txtEndDate.attr("disabled", false);
            }
        }
        else {
            flblSegmentBenefitType.html("");
            flblDateBenefitPeriod.html("");
        }
    }
</script>
<asp:HiddenField ID="hdIsDateOverlapped" runat="server" />
<ajax:Accordion ID="AccordionHospiceProviderServiceSpan" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionHospiceProviderServiceSpanPane" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblHospiceOtherPayerSpan" class="expandcollapse" runat="server" Text="- * HOSPICE PROVIDER SERVICE SPAN"></asp:Label>
            </Header>
            <Content>

                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvHospiceProviderServiceSpan" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true" 
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvHospiceProviderServiceSpan_PageIndexChanging"
                        OnRowEditing="gvHospiceProviderServiceSpan_RowEditing" OnRowUpdating="gvHospiceProviderServiceSpan_RowUpdating" OnRowCancelingEdit="gvHospiceProviderServiceSpan_RowCancelingEdit" OnRowDataBound="gvHospiceProviderServiceSpan_RowDataBound"
                        OnRowDeleting="gvHospiceProviderServiceSpan_RowDeleting" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>
                            <asp:TemplateField HeaderText="Benefit Line No" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenefitLineNo" runat="server" Text='<%# Bind("BenPeriod") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="hdnBenefitLineNo" runat="server" Value='<%#Bind("BenPeriod") %>'/>
                                    <asp:DropDownList ID="eddlBenefitLineNo" runat="server" onchange="javascript:BenefitLineNumberChangeProviderServicePanel(this, 'Edit');"></asp:DropDownList>
                                     <span style="color:red; display:none"><br />Benefit Line No is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:HiddenField ID="hdnBenefitLineNo" runat="server"/>
                                    <asp:DropDownList ID="fddlBenefitLineNo" runat="server" onchange="BenefitLineNumberChangeProviderServicePanel(this, 'Add');"></asp:DropDownList>
                                     <span style="color:red; display:none"><br />Benefit Line No is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Benefit Period Type" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenefitPeriodType" runat="server" ></asp:Label>
                                </ItemTemplate>
                                 <EditItemTemplate>
                                   <asp:Label ID="elblSegmentBenefitType" runat="server" ></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                  <asp:Label ID="flblSegmentBenefitType" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Benefit Period" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenefitPeriod" runat="server"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                   <asp:Label ID="elblDateBenefitPeriod" runat="server" ></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                  <asp:Label ID="flblDateBenefitPeriod" runat="server"  ></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                             
                            <asp:TemplateField HeaderText="Provider Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="240px">
                                <ItemTemplate>
                                    <asp:Label ID="lblProviderName" runat="server" Text='<%# Bind("HospiceProvID") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="elblProviderName" runat="server"></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="flblProviderName" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Effective Date" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="130px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Bind("SpanEffDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtEffectiveDate" autocomplete="off" runat="server" Text='<%# Bind("SpanEffDate", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                    <ajax:CalendarExtender ID="eclEffDate" TargetControlID="etxtEffectiveDate" runat="server" EnabledOnClient="true"/>
                                    <span style="color:red; display:none"><br />Effective date is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtEffectiveDate" autocomplete="off" runat="server" Text=""></asp:TextBox>
                                  <ajax:CalendarExtender ID="clEffDate" TargetControlID="ftxtEffectiveDate" runat="server" EnabledOnClient="true" />
                                    <span style="color:red; display:none"><br />Effective date is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EndDate" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="130px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("SpanEndDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtEndDate" autocomplete="off" runat="server" Text='<%# Bind("SpanEndDate", "{0:MM/dd/yyyy}") %>' ></asp:TextBox> 
                                    <ajax:CalendarExtender ID="eclEndDate" TargetControlID="etxtEndDate" runat="server" EnabledOnClient="true"/>
                                    <span style="color:red; display:none"><br />End date is required</span></EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtEndDate" autocomplete="off" runat="server" Text=""></asp:TextBox>
                                    <ajax:CalendarExtender ID="clEndDate" TargetControlID="ftxtEndDate" runat="server" EnabledOnClient="true"/>
                                    <span style="color:red; display:none"><br />End date is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CssClass="button" CausesValidation="false" />
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
                                    <asp:Button ID="ftnAdd" runat="server" Text="Add New" OnClick="fbtnAdd_Click" CssClass="button" Style="width: auto !important;"  />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>  
                <div style="text-align: center">
                                <span id="ServiceSpanErrorMessage" style="color: red;"></span>
                            </div>
            </Content>

        </ajax:AccordionPane>
    </Panes>

</ajax:Accordion>
