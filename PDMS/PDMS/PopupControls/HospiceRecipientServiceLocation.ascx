<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_HospiceRecipientServiceLocation" Codebehind="HospiceRecipientServiceLocation.ascx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<link href="../Styles/jquery-ui.css" rel="Stylesheet" type="text/css" />
<script type="text/javascript">
    $(function () {

        $("[id*=gvRecipentServiceLocation] [id*=ftnAdd]").click(function () {
            var row = $(this).closest("tr");

            var requiredControles = ["fddlStateOfService", "fddlCountyofService", "fddlBenefitLineNo", "ftxtEffectiveDate", "ftxtEndDate"];
            if ($.trim(row.find("[id*=fddlStateOfService]").val()) !== "") {
                $(row.find("[id*=fddlCountyofService]")).removeAttr("disabled");
            }
            var isValid = RequiredFieldsValidations(row, requiredControles, "LocationErrorMessage");
            var effectvieData = $.trim(row.find("[id*=ftxtEffectiveDate]").val());
            var endData = $.trim(row.find("[id*=ftxtEndDate]").val());
            var segmentIndecator = $.trim(row.find("[id*=fddlBenefitLineNo] option:selected").text());
            if (isValid === true) {
                isValid = DateValidations(effectvieData, endData, "LocationErrorMessage");
            }
            if (isValid === true) {
                isValid = DateValidationsOverlap(effectvieData, endData, "gvRecipentServiceLocation", "LocationErrorMessage");
            }
            if (isValid === true) {
                var benfitLinoValue = $.trim(row.find("[id*=fddlBenefitLineNo] option:selected").val());
                var benefit = benfitLinoValue.split(';');
                isValid = CompareWithBenefitPeriodLineNo(effectvieData, endData, "LocationErrorMessage", benefit[1].split('-')[0], benefit[1].split('-')[1], segmentIndecator);
            }
            if (isValid === true) {
                isValid = CheckDaysGap(effectvieData, endData, "gvRecipentServiceLocation", "LocationErrorMessage", segmentIndecator, "A county must be assigned for every day within the benefit period");
            }
            return isValid;
        });
        $("[id*=gvRecipentServiceLocation] [id*=btnUpdate]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["eddlStateOfService", "eddlCountyofService", "eddlBenefitLineNo", "etxtEffectiveDate", "etxtEndDate"];
            if ($.trim(row.find("[id*=eddlStateOfService]").val()) != "") {
                if ($.trim(row.find("[id*=eddlCountyofService]").val()) === "") {
                    $(row.find("[id*=eddlCountyofService]")).attr("disabled", "disabled");
                }
            }

            var isValid = RequiredFieldsValidations(row, requiredControles, "LocationErrorMessage");
            var effectvieData = $.trim(row.find("[id*=etxtEffectiveDate]").val());
            var endData = $.trim(row.find("[id*=etxtEndDate]").val());
            var segmentIndecator = $.trim(row.find("[id*=eddlBenefitLineNo] option:selected").text());
            if (isValid === true) {
                isValid = DateValidations(effectvieData, endData, "LocationErrorMessage");
            }

            if (isValid == true) {
                var benfitLinoValue = $.trim(row.find("[id*=eddlBenefitLineNo] option:selected").val());
                var benefit = benfitLinoValue.split(';');
                isValid = CompareWithBenefitPeriodLineNo(effectvieData, endData, "LocationErrorMessage", benefit[1].split('-')[0], benefit[1].split('-')[1], segmentIndecator);
            }
            return isValid;
        });
    });
    function StateOfServiceChange(ctrl, action) {
        var selectedItem = $(ctrl).find('option:selected').val();
        var hdCntrl = $(ctrl).closest('tr').find('td input[id*="hdnstateOfService"]');
        hdCntrl.val(selectedItem);
        var countyDropdown = (action === 'Edit') ? $(ctrl).closest('tr').find("[id*=eddlCountyofService]") : $(ctrl).closest('tr').find(" [id*=fddlCountyofService]");
        if ($(ctrl).val() !== "") {
            $(countyDropdown).removeAttr("disabled");
        }
        else {
            $(countyDropdown).attr("disabled", "disabled");
        }
        GetCounty($(ctrl).val(), countyDropdown);
    }

    function CountyofServiceChange(ctrl) {
        var selectedItem = $(ctrl).find('option:selected').text();
        var hdCntrl = $(ctrl).closest('tr').find('td input[id*="hdncountyOfService"]');
        hdCntrl.val(selectedItem);
    }


    function CountyofServiceFChange(ctrl) {
        var selectedItem = $(ctrl).find('option:selected').text();
        var hdCntrl = $(ctrl).closest('tr').find('td input[id*="hdncountyOfServiceF"]');
        hdCntrl.val(selectedItem);
    }
    function BenefitSegmentIndicatorChange(ctrl) {
        var selectedItem = $(ctrl).find('option:selected').text();
        var hdCntrl = $(ctrl).closest('tr').find('td input[id*="hdnBenefitSegmentIndicator"]');
        hdCntrl.val(selectedItem);
    }
    function GetCounty(state, countyDropdown) {
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiHospice + "GetCountyData?state=" + state,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: 'application/json;charset=utf-8',
            dataType: 'json',
            success: function (data) {
                countyDropdown.empty();
                countyDropdown.append("<option value=''></option>");
                for (var i = 0; i < data.length; i++) {
                    countyDropdown.append($('<option></option>').attr("value", data[i].COUNTY_ID).text(data[i].COUNTY_NAME));
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('<p>status code: ' + jqXHR.status + '</p><p>errorThrown: ' + errorThrown + '</p><p>jqXHR.responseText:</p><div>' + jqXHR.responseText + '</div>');
            }
        });
    }
</script>

<style>
    .focus {
border: 2px solid red;
background-color: #FEFED5;
}
    select:disabled {
        background-color: gray !important;
        font-weight: bold;
        font-size: 12px;
    }

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

<ajax:Accordion ID="AccordionRecipentServiceLocation" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">
    <Panes>
        <ajax:AccordionPane ID="AccordionPaneRecipentServiceLocation" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent" >
            <Header>
                <asp:Label ID="lblRecipentServiceLocation" class="expandcollapse" runat="server" Text="- * COUNTY AND STATE OF RECIPIENT’S HOSPICE SERVICE LOCATION"></asp:Label>
            </Header>
            <Content>
                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                             <asp:AsyncPostBackTrigger ControlID="gvRecipentServiceLocation" EventName="RowUpdating" />
                        </Triggers>
                        <ContentTemplate>--%>
                    <asp:GridView ID="gvRecipentServiceLocation" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true" 
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvRecipentServiceLocation_PageIndexChanging"
                        OnRowEditing="gvRecipentServiceLocation_RowEditing" OnRowUpdating="gvRecipentServiceLocation_RowUpdating" OnRowCancelingEdit="gvRecipentServiceLocation_RowCancelingEdit"
                        OnRowDeleting="gvRecipentServiceLocation_RowDeleting"  OnRowDataBound="gvRecipentServiceLocation_RowDataBound" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>
                            <asp:TemplateField HeaderText="Benefit Line No" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenefitLineNo" runat="server" Text='<%#: Bind("BenPeriod") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="hdnBenefitLineNo" runat="server" Value='<%#: Bind("BenPeriod") %>'/>
                                    <asp:DropDownList ID="eddlBenefitLineNo" runat="server" onchange="javascript:BenefitLineNumberChange(this,'Edit');"></asp:DropDownList>
                                     <span style="color:red; display:none"><br />Benefit Line No is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:HiddenField ID="hdnBenefitLineNo" runat="server"/>
                                    <asp:DropDownList ID="fddlBenefitLineNo" runat="server" onchange="javascript:BenefitLineNumberChange(this,'Add');"></asp:DropDownList>
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
                                      <asp:HiddenField ID="hdnBFPeriodDates" runat="server"/>
                                </ItemTemplate>
                                <EditItemTemplate>
                                   <asp:Label ID="elblDateBenefitPeriod" runat="server" ></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                  <asp:Label ID="flblDateBenefitPeriod" runat="server"  ></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="State Of Service" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblStateOfService" runat="server" Text='<%#: Bind("State") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="hdnstateOfService" runat="server" Value='<%#: Bind("State") %>' />
                                    <asp:DropDownList ID="eddlStateOfService" runat="server" onchange="javascript:StateOfServiceChange(this, 'Edit');"></asp:DropDownList>
                                    <span style="color: red; display: none">
                                        <br />
                                        State of service is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="fddlStateOfService" runat="server" onchange="javascript:StateOfServiceChange(this, 'Add');"></asp:DropDownList>
                                    <span style="color: red; display: none">
                                        <br />
                                        State of service is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="County of Service" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblCountyofService" runat="server" Text='<%#: Bind("County") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="hdncountyOfService" runat="server" Value='<%#: Bind("County") %>' />
                                    <asp:DropDownList ID="eddlCountyofService" runat="server" onchange="javascript:CountyofServiceChange(this);"></asp:DropDownList>
                                    <span style="color: red; display: none">
                                        <br />
                                        County of service is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:HiddenField ID="hdncountyOfServiceF" runat="server" Value='<%#: Bind("County") %>' />
                                    <asp:DropDownList ID="fddlCountyofService" runat="server" disabled="disabled" onchange="javascript:CountyofServiceFChange(this);"></asp:DropDownList>
                                    <span style="color: red; display: none">
                                        <br />
                                        County of service is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                           <%-- <asp:TemplateField HeaderText="Benefit Segment Indicator" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenefitSegmentIndicator" runat="server" Text='<%# Bind("SegmentIndicator") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="hdnBenefitSegmentIndicator" runat="server" Value='<%#Bind("SegmentIndicator") %>' />
                                    <asp:DropDownList ID="eddlBenefitSegmentIndicator" runat="server" onchange="javascript:BenefitSegmentIndicatorChange(this);"></asp:DropDownList>
                                    <span style="color: red; display: none">
                                        <br />
                                        Benefit Segment Indicator is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="fddlBenefitSegmentIndicator" runat="server"></asp:DropDownList>
                                    <span style="color: red; display: none">
                                        <br />
                                        Benefit Segment Indicator is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Effective Date" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveDate" CssClass="* EffectiveDate" runat="server" Text='<%#: Bind("CountyEffDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtEffectiveDate" autocomplete="off" runat="server" Text='<%#: Bind("CountyEffDate", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                    <ajax:CalendarExtender ID="cleEffDate" TargetControlID="etxtEffectiveDate" runat="server"  />
                                    <span style="color: red; display: none">
                                        <br />
                                        Effective date is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtEffectiveDate" runat="server" autocomplete="off"></asp:TextBox>
                                    <ajax:CalendarExtender ID="clEffectiveDate" TargetControlID="ftxtEffectiveDate" runat="server" />

                                    <span style="color: red; display: none">
                                        <br />
                                        Effective date is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EndDate" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEndDate" CssClass="* EndDate" runat="server" Text='<%#: Bind("CountyEndDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtEndDate" runat="server" autocomplete="off" Text='<%#: Bind("CountyEndDate", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                    <ajax:CalendarExtender ID="cleEndDate" TargetControlID="etxtEndDate" runat="server" EnabledOnClient="true" Format="MM/dd/yyyy" />
                                    <span style="color: red; display: none">
                                        <br />
                                        End date is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtEndDate" runat="server" autocomplete="off"></asp:TextBox>
                                    <ajax:CalendarExtender ID="clfEndDate" TargetControlID="ftxtEndDate" runat="server"  />
                                    <span style="color: red; display: none;">
                                        <br />
                                        End date is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CssClass="button" CausesValidation="false" />
                                    &nbsp;
                <asp:Button ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" Visible='<%# Convert.ToBoolean(Eval("IsFromInquiry")) == true ? false : true %>'
                    CssClass="button" OnClientClick='return confirm("Are you sure you want to delete this record?");' CausesValidation="false" />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button ID="btnUpdate" Text="Update" runat="server" CommandName="Update" CssClass="button" CausesValidation="false" />
                                    &nbsp;
                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" CssClass="button" CausesValidation="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="ftnAdd" runat="server" Text="Add New" OnClick="fbtnAdd_Click" CssClass="button" Style="width: auto !important;" />
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
                        <span id="LocationErrorMessage" style="color: red;"></span>
                    </div>
                    <%--  </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
            </Content>
        </ajax:AccordionPane>
    </Panes>
</ajax:Accordion>

