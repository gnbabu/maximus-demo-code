<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_HospiceHLTCFProviderService" Codebehind="HospiceHLTCFProviderService.ascx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/PopupControls/HospiceHLTCFProviderSearch.ascx" TagName="HospiceHLTCFProviderSearch" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%--<script src="../Scripts/bootstrap.min.js"></script>--%>
<%--<script src="../Scripts/jquery-1.4.1.min.js"></script>
<script src="../Scripts/jquery-1.4.1.js"></script>
<link href="../Styles/jquery-ui.css" rel="Stylesheet" type="text/css" />--%>
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

    .gridViewAutoScroll {
        height: 300px;
        overflow: scroll;
    }
</style>

<script type="text/javascript">

    $(function () {
        $("[id*=gvHospiceHLTCFProviderService] [id*=ftxtProviderNPI]").blur(function () {
            if ($(this).val() !== "") {
                GetProvderInfoByNPI($(this).val(), 'Add');
            }
        });
        $("[id*=gvHospiceHLTCFProviderService] [id*=etxtProviderNPI]").blur(function () {
            if ($(this).val() !== "") {
                GetProvderInfoByNPI($(this).val(), 'Update');
            }
        });
        $("[id*=gvHospiceHLTCFProviderService] [id*=ftnAdd]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["fddlBenefitLineNo", "ftxtProviderNPI", "ftxtEffectiveDate", "ftxtEndDate"];
            var isValid = RequiredFieldsValidations(row, requiredControles);
            var effectvieData = $.trim(row.find("[id*=ftxtEffectiveDate]").val());
            var endData = $.trim(row.find("[id*=ftxtEndDate]").val());
            var segmentIndecator = $.trim(row.find("[id*=fddlBenefitLineNo] option:selected").text());
            if (isValid == true) {
                var ftxtNPI = $.trim(row.find("[id*=ftxtProviderNPI]").val());
                if (ftxtNPI.length != 10) {
                    $("[id*=HltcPhyErrorMessage]").text("NPI Should be 10 Digit number.");
                    isValid = false;
                }
            }
            if (isValid === true) {
                isValid = DateValidations(effectvieData, endData, "HltcPhyErrorMessage");
            }
            if (isValid === true) {
                isValid = DateValidationsOverlap(effectvieData, endData, "gvHospiceHLTCFProviderService", "HltcPhyErrorMessage");
            }
            if (isValid == true) {
                var benfitLinoValue = $.trim(row.find("[id*=fddlBenefitLineNo] option:selected").val());
                var benefit = benfitLinoValue.split(';');
                isValid = CompareWithBenefitPeriodLineNo(effectvieData, endData, "HltcPhyErrorMessage", benefit[1].split('-')[0], benefit[1].split('-')[1], segmentIndecator);
            }
            return isValid;
        });
        $("[id*=gvHospiceHLTCFProviderService] [id*=btnUpdate]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["eddlBenefitLineNo", "etxtProviderNPI", "etxtEffectiveDate", "etxtEndDate"];
            var segmentIndecator = $.trim(row.find("[id*=eddlBenefitLineNo] option:selected").text());
            var isValid = RequiredFieldsValidations(row, requiredControles);
            if (isValid == true) {
                var etxtProviderNPI = $.trim(row.find("[id*=etxtProviderNPI]").val());
                if (etxtProviderNPI.length != 10) {
                    $("[id*=HltcPhyErrorMessage]").text("NPI Should be 10 Digit number.");
                    isValid = false;
                }
            }
            var effectvieData = $.trim(row.find("[id*=etxtEffectiveDate]").val());
            var endData = $.trim(row.find("[id*=etxtEndDate]").val());
            if (isValid === true) {
                isValid = DateValidations(effectvieData, endData, "HltcPhyErrorMessage");
            }
            if (isValid == true) {
                var benfitLinoValue = $.trim(row.find("[id*=eddlBenefitLineNo] option:selected").val());
                var benefit = benfitLinoValue.split(';');
                isValid = CompareWithBenefitPeriodLineNo(effectvieData, endData, "HltcPhyErrorMessage", benefit[1].split('-')[0], benefit[1].split('-')[1], segmentIndecator);
            }
            return isValid;
        });
        <%--$('#<%=gvHospiceHLTCFProviderSearch.ClientID %>').Scrollable({
            ScrollHeight: 300,
            IsInUpdatePanel: true
        });--%>
    });
    function OpenNPIModal() {
        $('[id*=DiaHLTCFProviderSearch]').show();
    }
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
    function GetProvderInfoByNPI(npi, action) {        
        var medicaidid = "";
        var lastName = "";
        var firstName = "";

        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiHospice + "Search?npi=" + npi + "&&medicaidid=" + medicaidid + "&&lastName=" + lastName + "&&firstName=" + firstName,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: 'application/json;charset=utf-8',
            dataType: 'json',           
            success: function (result) {
                if (result === "") {
                    $("[id*=HltcPhyErrorMessage]").text("Invalid NPI.");
                }
                else {
                    if (result.length > 0 && result.length === 1) {
                        if (action == 'Add') {
                            console.log(result[0].MEDICAID_ID);
                            var fillRow = $("[id*=gvHospiceHLTCFProviderService] [id*=ftnAdd]").closest("tr");
                            fillRow.find("[id*=ftxtProviderMedicaidID]").val(result[0].MEDICAID_ID);
                            fillRow.find("[id*=hdnftxtProviderMedicaidID]").val(result[0].MEDICAID_ID);
                            fillRow.find("[id*=ftxtProviderName]").val(result[0].LAST_OR_BUSINESS_NAME + ' ' + result[0].FIRST_NAME);
                            $("[id*=HltcPhyErrorMessage]").text("");
                        }
                        else {
                            var fillRow = $("[id*=gvHospiceHLTCFProviderService] [id*=btnUpdate]").closest("tr");
                            fillRow.find("[id*=etxtProviderMedicaidID]").val(result[0].MEDICAID_ID);
                            fillRow.find("[id*=hdnetxtProviderMedicaidID]").val(result[0].MEDICAID_ID);
                            fillRow.find("[id*=etxtProviderName]").val(result[0].LAST_OR_BUSINESS_NAME + ' ' + result[0].FIRST_NAME);
                        }
                    }
                    else if (result.length > 1) {
                        $('#<%=txtNPI.ClientID%>').val(npi);
                        $('#hdSelectedSearchButtonGridRowId').val(action);
                        SearchProviderinfo();
                        var behaviorId = $('#hdSelectedSearchHLTCBehaviorId').val();
                        $find("" + behaviorId + "").show();
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
            }
        });
    }
    function SearchProviderinfo() {
        $('#gvHospiceHLTCFProviderSearch').empty();
        var npi = $('#<%=txtNPI.ClientID%>').val();
        var medicaidid = $('#<%=txtMedicaidID.ClientID%>').val();
        var lastName = $('#<%=txtBusinessLastName.ClientID%>').val();
        var firstName = $('#<%=txtFirstName.ClientID%>').val();
        var etxtProviderNPI = $.trim(npi);
        if (etxtProviderNPI === "" && medicaidid === "" && lastName === "" && firstName === "") {
            $("[id*=HltcPhySearchErrorMessage]").text("Please Enter at least one search criteria.");
            return;
        }
        if (etxtProviderNPI.length > 0 && etxtProviderNPI.length != 10) {
            $("[id*=HltcPhySearchErrorMessage]").text("NPI Should be 10 Digit number.");
            return;
        }
        if (medicaidid.length > 0 && medicaidid.length != 7) {
            $("[id*=HltcPhySearchErrorMessage]").text("7 digits Medicaid ID is required.");
            return;
        }
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            url: webApiHospice + "Search?npi=" + npi + "&&medicaidid=" + medicaidid + "&&lastName=" + lastName + "&&firstName=" + firstName,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: 'application/json;charset=utf-8',
            dataType: 'json',
            success: function (result) {
                if (result === "") {
                    $("[id*=HltcPhySearchErrorMessage]").text("No data found.");
                    $("[id*=gvHospiceHLTCFProviderSearch] tr").not($("[id*=gvHospiceHLTCFProviderSearch] tr:first-child")).css('visibility', 'hidden');
                }
                else {
                    $("[id*=gvHospiceHLTCFProviderSearch] tr").not($("[id*=gvHospiceHLTCFProviderSearch] tr:first-child")).css('visibility', 'visible');
                    var row = $("[id*=gvHospiceHLTCFProviderSearch] tr:last-child").clone(true);
                    $("[id*=gvHospiceHLTCFProviderSearch] tr").not($("[id*=gvHospiceHLTCFProviderSearch] tr:first-child")).remove();
                    if (result.length > 0) {
                        for (var i = 0; i < result.length; i++) {
                            $("td", row).eq(0).html(result[i].NPI);
                            $("td", row).eq(1).html(result[i].MEDICAID_ID);
                            $("td", row).eq(2).html(result[i].LAST_OR_BUSINESS_NAME);
                            $("td", row).eq(3).html(result[i].FIRST_NAME);
                            $("[id*=gvHospiceHLTCFProviderSearch]").append(row);
                            row = $("[id*=gvHospiceHLTCFProviderSearch] tr:last-child").clone(true);
                        }
                        $("[id*=HltcPhySearchErrorMessage]").text("");
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('<p>status code: ' + jqXHR.status + '</p><p>errorThrown: ' + errorThrown + '</p><p>jqXHR.responseText:</p><div>' + jqXHR.responseText + '</div>');
            }
        });

    }
    function FillSelectedProviderInfo(ctrl) {
        var row = $(ctrl).closest("tr");
        var action = $('#hdSelectedSearchButtonGridRowId').val();
        var behaviorId = $('#hdSelectedSearchHLTCBehaviorId').val();
        if (action == 'Add') {
            console.log(row.find("td").eq(1).html());
            var fillRow = $("[id*=gvHospiceHLTCFProviderService] [id*=ftnAdd]").closest("tr");
            fillRow.find("[id*=ftxtProviderNPI]").val(row.find("td").eq(0).html());
            fillRow.find("[id*=ftxtProviderMedicaidID]").val(row.find("td").eq(1).html());
            fillRow.find("[id*=hdnftxtProviderMedicaidID]").val(row.find("td").eq(1).html());
            fillRow.find("[id*=ftxtProviderName]").val(row.find("td").eq(2).html().trim() + ' ' + row.find("td").eq(3).html().trim());
        }
        else {
            var fillRow = $("[id*=gvHospiceHLTCFProviderService] [id*=btnUpdate]").closest("tr");
            fillRow.find("[id*=etxtProviderNPI]").val(row.find("td").eq(0).html());
            fillRow.find("[id*=etxtProviderMedicaidID]").val(row.find("td").eq(1).html());
            fillRow.find("[id*=hdnetxtProviderMedicaidID]").val(row.find("td").eq(1).html());
            fillRow.find("[id*=etxtProviderName]").val(row.find("td").eq(2).html().trim() + ' ' + row.find("td").eq(3).html().trim());
        }
        $find("" + behaviorId + "").hide();
    }
    function SearchClickEvent(action, behaviorId) {
        $("[id*=HltcPhySearchErrorMessage]").text("");
        $("[id*=gvHospiceHLTCFProviderSearch] tr").not($("[id*=gvHospiceHLTCFProviderSearch] tr:first-child")).css('visibility', 'hidden');
        $('#hdSelectedSearchButtonGridRowId').val(action);
        $('#hdSelectedSearchHLTCBehaviorId').val(behaviorId);
    }
    function setBlankValue() {
        $('#<%=txtNPI.ClientID%>').val("");
        $('#<%=txtMedicaidID.ClientID%>').val("");
        $('#<%=txtBusinessLastName.ClientID%>').val("");
        $('#<%=txtFirstName.ClientID%>').val("");
    }
</script>

<ajax:Accordion ID="AccordionHospiceHLTCFProviderService" runat="Server" SelectedIndex="-1" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">
    <Panes>

        <ajax:AccordionPane ID="AccordionPaneHospiceHLTCFProviderService" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblHospiceHLTCFProviderService" class="expandcollapse" runat="server" Text="+ HLTCF PROVIDER SERVICE"></asp:Label>
            </Header>
            <Content>
                <%--  <input type="hidden" id="hdSelectedSearchButtonGridRowId" name="hdSelectedSearchButtonGridRowId" value="" />--%>
                <asp:HiddenField ID="hdSelectedSearchButtonGridRowId" runat="server" ClientIDMode="Static" />
                 <asp:HiddenField ID="hdSelectedSearchHLTCBehaviorId" runat="server" ClientIDMode="Static" />
                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvHospiceHLTCFProviderService" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true"
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvHospiceHLTCFProviderService_PageIndexChanging"
                        OnRowEditing="gvHospiceHLTCFProviderService_RowEditing" OnRowUpdating="gvHospiceHLTCFProviderService_RowUpdating" OnRowCancelingEdit="gvHospiceHLTCFProviderService_RowCancelingEdit"
                        OnRowDeleting="gvHospiceHLTCFProviderService_RowDeleting" OnRowDataBound="gvHospiceHLTCFProviderService_RowDataBound" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>
                             <asp:TemplateField HeaderText="Benefit Line No" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenefitLineNo" runat="server" Text='<%# Bind("BenPeriod") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="hdnBenefitLineNo" runat="server" Value='<%#Bind("BenPeriod") %>'/>
                                    <asp:DropDownList ID="eddlBenefitLineNo" runat="server" onchange="javascript:BenefitLineNumberChange(this, 'Edit');"></asp:DropDownList>
                                     <span style="color:red; display:none"><br />Benefit Line No is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:HiddenField ID="hdnBenefitLineNo" runat="server"/>
                                    <asp:DropDownList ID="fddlBenefitLineNo" runat="server" onchange="javascript:BenefitLineNumberChange(this , 'Add');"></asp:DropDownList>
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
                            <asp:TemplateField HeaderText="Provider NPI" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblProviderNPI" runat="server"></asp:Label>&nbsp;
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtProviderNPI" runat="server" MaxLength="10"></asp:TextBox>
                                    <asp:LinkButton ID="lnkSearch" runat="server" Text="Search" OnClientClick="SearchClickEvent('Update','modelHLTCFProviderSearch')" />
                                    <ajax:ModalPopupExtender ID="DiaHLTCFProviderSearch" runat="server" BehaviorID="modelHLTCFProviderSearch"
                                        PopupControlID="pnlHLTCFProviderSearch" TargetControlID="lnkSearch"
                                        BackgroundCssClass="modalBackground" CancelControlID="btnCloseHLTCF" />
                                    <span style="color: red; display: none"><br />NPI is required</span>
                                    <asp:RegularExpressionValidator runat="server" ID="revetxtProviderNPI" ControlToValidate="etxtProviderNPI" ErrorMessage="10 Digit number required" ValidationExpression="^\d{10}$" ForeColor="Red"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtProviderNPI" runat="server" MaxLength="10"></asp:TextBox>
                                    <span style="color: red; display: none"><br />NPI is required</span>
                                    <asp:LinkButton ID="lnkSearch" runat="server" Text="Search" OnClientClick="SearchClickEvent('Add','modelHLTCFProviderSearch1')" />
                                    <ajax:ModalPopupExtender ID="DiaHLTCFProviderSearch" runat="server" PopupControlID="pnlHLTCFProviderSearch" BehaviorID="modelHLTCFProviderSearch1"
                                        TargetControlID="lnkSearch" BackgroundCssClass="modalBackground" CancelControlID="btnCloseHLTCF" />
                                    <asp:RegularExpressionValidator runat="server" ID="revftxtProviderNPI" ControlToValidate="ftxtProviderNPI" ErrorMessage="10 Digit number required" ValidationExpression="^\d{10}$" ForeColor="Red" ></asp:RegularExpressionValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Provider Medicaid ID" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="130px">
                                <ItemTemplate>
                                    <asp:Label ID="lblProviderMedicaidID" runat="server" Text='<%# Bind("HLTCFProvMedID") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="hdnetxtProviderMedicaidID" runat="server"/>
                                    <asp:TextBox ID="etxtProviderMedicaidID" runat="server" Text='<%# Bind("HLTCFProvMedID") %>' Enabled="false"></asp:TextBox>
                                    <asp:RegularExpressionValidator runat="server" ID="revetxtProviderMedicaidID" ControlToValidate="etxtProviderMedicaidID" ErrorMessage="7 Digit number required" ValidationExpression="^\d{7,}$" ForeColor="Red"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:HiddenField ID="hdnftxtProviderMedicaidID" runat="server"/>
                                    <asp:TextBox ID="ftxtProviderMedicaidID" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:RegularExpressionValidator runat="server" ID="revftxtProviderMedicaidID" ControlToValidate="ftxtProviderMedicaidID" ErrorMessage="7 Digit number required" ValidationExpression="^\d{7,}$" ForeColor="Red"></asp:RegularExpressionValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Provider Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="220px">
                                <ItemTemplate>
                                    <%--<asp:Label ID="lblProviderName" runat="server" Text='<%# Bind("HLTCFProviderName") %>'></asp:Label>--%>
                                    <asp:Label ID="lblHLTCProviderName" runat="server"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtProviderName" runat="server" Enabled="false" Width="100%"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtProviderName" runat="server" Enabled="false" Width="100%"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Effective Date" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Bind("HLTCFEffDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtEffectiveDate" autocomplete="off" runat="server" Text='<%# Bind("HLTCFEffDate", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                    <ajax:CalendarExtender ID="clEffDate" TargetControlID="etxtEffectiveDate" runat="server" EnabledOnClient="true" />
                                    <span style="color: red; display: none"><br />Effective date is required</span>
                                   </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtEffectiveDate" autocomplete="off" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="clEFffDate" TargetControlID="ftxtEffectiveDate" runat="server" EnabledOnClient="true" />
                                    <span style="color: red; display: none"><br />Effective date is required</span>
                                    </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EndDate" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("HLTCFEndDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtEndDate" autocomplete="off" runat="server" Text='<%# Bind("HLTCFEndDate", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                    <ajax:CalendarExtender ID="clEENDDate" TargetControlID="etxtEndDate" runat="server" EnabledOnClient="true" />
                                    <span style="color: red; display: none"><br />End date is required</span>
                                    </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtEndDate" autocomplete="off" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="clEndDate" TargetControlID="ftxtEndDate" runat="server"
                                        EnabledOnClient="true" />
                                    <span style="color: red; display: none"><br />End date is required</span>
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
                                    <asp:Button ID="btnUpdate" Text="Update" runat="server" CommandName="Update" CssClass="button" />
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
                </div>
                <asp:Panel ID="pnlHLTCFProviderSearch" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
                    <asp:Panel ID="pnlCE1" runat="server">
                        <asp:Button runat="server" ID="btnCloseHLTCF" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;" CausesValidation="false" OnClientClick="javascript: setBlankValue();" />
                        <div style="text-align: left; padding: 42px; overflow-x: auto; overflow-y: auto" class="container-fluid">
                            <%--<div class="row">
                                           <uc1:HospiceHLTCFProviderSearch ID="HospiceHLTCFProviderSearch1" runat="server" Visible="true"  />                                          
                                    </div>--%>
                            <div class="row" style="text-align: center;">
                                <div class="col-sm-6 col-md-4 col-lg-3 ">
                                    <span class="ohio-field-label"><b>NPI</b>
                                        <asp:TextBox ID="txtNPI" CssClass="ohio-field-input" runat="server" MaxLength="10">
                                        </asp:TextBox>
                                    </span>
                                    <asp:RegularExpressionValidator runat="server" ID="revtxtNPI" ControlToValidate="txtNPI" ErrorMessage="10 Digit number required" ValidationExpression="^\d{10}$" ForeColor="Red"></asp:RegularExpressionValidator>
                                </div>

                                <div class="col-sm-6 col-md-4 col-lg-3 ">
                                    <span class="ohio-field-label"><b>Medicaid ID</b>
                                        <asp:TextBox ID="txtMedicaidID" CssClass="ohio-field-input" runat="server" MaxLength="7">
                                        </asp:TextBox>
                                    </span>

                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-3 ">
                                    <span class="ohio-field-label"><b>Business/Last Name</b>
                                        <asp:TextBox ID="txtBusinessLastName" CssClass="ohio-field-input" runat="server" MaxLength="70">
                                        </asp:TextBox>
                                    </span>

                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-3 ">
                                    <span class="ohio-field-label"><b>First Name</b>
                                        <asp:TextBox ID="txtFirstName" CssClass="ohio-field-input" runat="server" MaxLength="35">
                                        </asp:TextBox>
                                    </span>

                                </div>
                                <div class="col-sm-6 col-md-4 col-lg-3 ">
                                    <%-- <asp:Button ID="btnSearch" runat="server" OnClientClick="javascript:SearchProviderinfo();" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" Style="background-color: darkslateblue !important" />--%>
                                    <input type="button" id="btnSearch" onclick="SearchProviderinfo();" value="Search" class="buttonBoxFocus" style="background-color: darkslateblue !important" />
                                </div>
                            </div>
                            <div style="text-align: center">
                                <span id="HltcPhySearchErrorMessage" runat="server" style="color: red;"></span>
                            </div>
                            <div style="width: 100%; height: 400px; overflow: scroll">
                                <mms:SortablePagingGridView
                                    ID="gvHospiceHLTCFProviderSearch"
                                    runat="server"
                                    AutoGenerateColumns="False"
                                    CssClass="gridViewSmallFont" Width="100%"
                                    AllowSorting="true"
                                    ShowHeaderWhenEmpty="true"
                                    EmptyDataText=" "
                                    RowStyle-VerticalAlign="Top"
                                    AlternatingRowStyle-BackColor="White" GridLines="Horizontal"
                                    AllowPaging="True"
                                    PageSize="15"
                                    GridViewSortColumn="NPI" GridViewSortDirection="Ascending">
                                    <Columns>
                                        <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
                                        <asp:BoundField DataField="MEDICAID_ID" HeaderText="Medicaid ID" SortExpression="MedicaidID" />
                                        <asp:BoundField DataField="LAST_OR_BUSINESS_NAME" HeaderText="Business/Last Name" SortExpression="BusinessLastName" />
                                        <asp:BoundField DataField="FIRST_NAME" HeaderText="First Name" SortExpression="FirstName" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <input type="button" id="lnkSelect" onclick="FillSelectedProviderInfo(this); setBlankValue();" value="Select" class="buttonBoxFocus" style="background-color: darkslateblue !important" />
                                                <%-- <asp:LinkButton Text="Select" runat="server" CommandName="Select" CommandArgument="<%# Container.DataItemIndex %>" />  --%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </mms:SortablePagingGridView>
                            </div>
                        </div>
                    </asp:Panel>
                </asp:Panel>

                <div style="text-align: center">
                    <span id="HltcPhyErrorMessage" runat="server" style="color: red;"></span>
                </div>
            </Content>
        </ajax:AccordionPane>

    </Panes>




</ajax:Accordion>

