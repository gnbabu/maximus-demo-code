<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_HospiceTerminalIllnessDiagnosis, App_Web_tiu3g34i" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/PopupControls/HospiceTerminalIllnessDiagnosisSearch.ascx" TagName="HospiceTerminalIllnessDiagnosisSearch" TagPrefix="uc1" %>
<%--<script type="text/javascript" src="../Scripts/jquery-1.4.1.min.js"></script>
<script type="text/javascript" src="../Scripts/jquery-1.4.1.js" ></script>--%>
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
        $("[id*=gvHospiceTerminalIllnessDiagnosis] [id*=ftnAdd]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["fddlBenefitLineNo", "ftxtPrimaryTerminalDiagnosis", "ftxtDiagnosisEffectiveDate", "ftxtDiagnosisEndDate"];
            var isValid = RequiredFieldsValidations(row, requiredControles);
            var effectvieData = $.trim(row.find("[id*=ftxtDiagnosisEffectiveDate]").val());
            var endData = $.trim(row.find("[id*=ftxtDiagnosisEndDate]").val());
            var segmentIndecator = $.trim(row.find("[id*=fddlBenefitLineNo] option:selected").text());

            if (isValid === true) {
                var primaryTD = "ftxtPrimaryTerminalDiagnosis";
                isValid = ValidateDiagnosisCode(row.find("[id*=" + primaryTD + "]"), "Primary Terminal Diagnosis code is invalid");
            }
            if (isValid === true) {
                var primaryTD2 = "ftxtTerminalDiagnosis2";
                if (row.find("[id*=" + primaryTD2 + "]").val() != "") {
                    isValid = ValidateDiagnosisCode(row.find("[id*=" + primaryTD2 + "]"), "Terminal Diagnosis 2 code is invalid");
                }
            }
            if (isValid === true) {
                var primaryTD3 = "ftxtTerminalDiagnosis3";
                if (row.find("[id*=" + primaryTD3 + "]").val() != "") {
                    isValid = ValidateDiagnosisCode(row.find("[id*=" + primaryTD3 + "]"), "Terminal Diagnosis 3 code is invalid");
                }
            }
            if (isValid === true) {
                isValid = DateValidations(effectvieData, endData, "TerminalErrorMessage");
            }
            if (isValid === true) {
                isValid = DateValidationsOverlap(effectvieData, endData, "gvHospiceTerminalIllnessDiagnosis", "TerminalErrorMessage");
            }
            if (isValid === true) {
                var benfitLinoValue = $.trim(row.find("[id*=fddlBenefitLineNo] option:selected").val());
                var benefit = benfitLinoValue.split(';');
                var segmentIndecator = $.trim(row.find("[id*=fddlBenefitLineNo] option:selected").text());
                isValid = CompareWithBenefitPeriodLineNo(effectvieData, endData, "TerminalErrorMessage", benefit[1].split('-')[0], benefit[1].split('-')[1], segmentIndecator);
            }
            if (isValid === true) {
                isValid = CheckDaysGap(effectvieData, endData, "gvHospiceTerminalIllnessDiagnosis", "TerminalErrorMessage", segmentIndecator, "Gaps of date spans are not allowed between the records associated with same benefit period.");
            }


            return isValid;
        });
        $("[id*=gvHospiceTerminalIllnessDiagnosis] [id*=btnUpdate]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["eddlBenefitLineNo", "etxtPrimaryTerminalDiagnosis", "etxtDiagnosisEffectiveDate", "etxtDiagnosisEndDate"];
            var isValid = RequiredFieldsValidations(row, requiredControles);
            var effectvieData = $.trim(row.find("[id*=etxtDiagnosisEffectiveDate]").val());
            var endData = $.trim(row.find("[id*=etxtDiagnosisEndDate]").val());
            var segmentIndecator = $.trim(row.find("[id*=eddlBenefitLineNo] option:selected").text());
            if (isValid === true) {
                var primaryTD = "etxtPrimaryTerminalDiagnosis";
                isValid = ValidateDiagnosisCode(row.find("[id*=" + primaryTD + "]"), "Primary Terminal Diagnosis code is invalid");
            }
            if (isValid === true) {
                var primaryTD2 = "etxtTerminalDiagnosis2";
                if (row.find("[id*=" + primaryTD2 + "]").val() != "") {
                    isValid = ValidateDiagnosisCode(row.find("[id*=" + primaryTD2 + "]"), "Terminal Diagnosis 2 code is invalid");
                }
            }
            if (isValid === true) {
                var primaryTD3 = "etxtTerminalDiagnosis3";
                if (row.find("[id*=" + primaryTD3 + "]").val() != "") {
                    isValid = ValidateDiagnosisCode(row.find("[id*=" + primaryTD3 + "]"), "Terminal Diagnosis 3 code is invalid");
                }
            }
            if (isValid === true) {
                isValid = DateValidations(effectvieData, endData, "TerminalErrorMessage");
            }
            //if (isValid == true) {
            //    isValid = TerminallDateValidations(effectvieData, endData);
            //}
            if (isValid == true) {
                var benfitLinoValue = $.trim(row.find("[id*=eddlBenefitLineNo] option:selected").val());
                var benefit = benfitLinoValue.split(';');
                var segmentIndecator = $.trim(row.find("[id*=eddlBenefitLineNo] option:selected").text());
                isValid = CompareWithBenefitPeriodLineNo(effectvieData, endData, "TerminalErrorMessage", benefit[1].split('-')[0], benefit[1].split('-')[1], segmentIndecator);
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
    function TerminallDateValidations(effectiveDate, endDate) {
        if (Date.parse(effectiveDate) > Date.parse(endDate)) {
            $("[id*=TerminalErrorMessage]").text("Effective Date Should be less than or equal to End date.");
            return false;
        }
        if (Date.parse(endDate) < Date.parse(effectiveDate)) {
            $("[id*=TerminalErrorMessage]").text("EndDate Should be grater than or equal to EffectvieData.");
            return false;
        }

        return true;
    }
    function BenefitSegmentIndicatorChange(ctrl) {
        var selectedItem = $(ctrl).find('option:selected').text();
        var hdCntrl = $(ctrl).closest('tr').find('td input[id*="hdnBenefitSegmentIndicator"]');
        hdCntrl.val(selectedItem);
    }
    function SearchDiagnosisinfo() {
        $('#gvHospiceTerminalIllnessDiagnosisSearch').empty();
        var code = $('#<%=txtDiagnosisCode.ClientID%>').val();
        var icdVersion = $('#<%=ddlICDVersion.ClientID%>').val();
        var diagnosisDes = $('#<%=txtDiagnosisCodeDescription.ClientID%>').val();
        //var post_data = JSON.stringify({ "code": code, "icdVersion": icdVersion, "diagnosisDes": diagnosisDes });
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            url: webApiPA + 'GetICDDiagnosis?code=' + code + "&&icdVersion=" + icdVersion + "&&diagnosisDes=" + diagnosisDes,
            type: 'GET',
            contentType: 'application/json;charset=utf-8',
            dataType: 'json',
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            //data: post_data,
            success: function (result) {
                if (result.length < 1) {
                    $("[id*=TerminalSearchErrorMessage]").text("No data found.");
                    $("[id*=gvHospiceTerminalIllnessDiagnosisSearch] tr").not($("[id*=gvHospiceTerminalIllnessDiagnosisSearch] tr:first-child")).css('visibility', 'hidden');
                }
                else {
                    $("[id*=gvHospiceTerminalIllnessDiagnosisSearch] tr").not($("[id*=gvHospiceTerminalIllnessDiagnosisSearch] tr:first-child")).css('visibility', 'visible');
                    var row = $("[id*=gvHospiceTerminalIllnessDiagnosisSearch] tr:last-child").clone(true);
                    $("[id*=gvHospiceTerminalIllnessDiagnosisSearch] tr").not($("[id*=gvHospiceTerminalIllnessDiagnosisSearch] tr:first-child")).remove();
                    if (result.length > 0) {
                        for (var i = 0; i < result.length; i++) {
                            $("td", row).eq(0).html(result[i].ICD10Diag);
                            $("td", row).eq(1).html(result[i].ICDVersion);
                            $("td", row).eq(2).html(result[i].DiagDesc);
                            $("[id*=gvHospiceTerminalIllnessDiagnosisSearch]").append(row);
                            row = $("[id*=gvHospiceTerminalIllnessDiagnosisSearch] tr:last-child").clone(true);
                        }
                        $("[id*=TerminalSearchErrorMessage]").text("");
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('<p>status code: ' + jqXHR.status + '</p><p>errorThrown: ' + errorThrown + '</p><p>jqXHR.responseText:</p><div>' + jqXHR.responseText + '</div>');
            }
        });

    }
    function FillSelectedDiagnosisInfo(ctrl) {
        var row = $(ctrl).closest("tr");
        var action = $('#hdSelectedSearchTDButtonGridRowId').val();
        var id = $('#hdSelectedSearchTDColumnId').val();
        var behaviorID = $('#hdSelectedSearchTDBehaviorID').val();
        if (action == 'Add') {
            var fillRow = $("[id*=gvHospiceTerminalIllnessDiagnosis] [id*=ftnAdd]").closest("tr");
            fillRow.find("[id*=" + id + "]").val(row.find("td").eq(0).html());
        }
        else {
            var fillRow = $("[id*=gvHospiceTerminalIllnessDiagnosis] [id*=btnUpdate]").closest("tr");
            fillRow.find("[id*=" + id + "]").val(row.find("td").eq(0).html());
        }
        $find("" + behaviorID + "").hide();
    }
    function DiagnosisSearchClickEvent(action, id, behaviorID) {
        $("[id*=TerminalSearchErrorMessage]").text("");
        $("[id*=gvHospiceTerminalIllnessDiagnosisSearch] tr").not($("[id*=gvHospiceTerminalIllnessDiagnosisSearch] tr:first-child")).css('visibility', 'hidden');
        $('#hdSelectedSearchTDButtonGridRowId').val(action);
        $('#hdSelectedSearchTDColumnId').val(id);
        $('#hdSelectedSearchTDBehaviorID').val(behaviorID);
        $('#<%=ddlICDVersion.ClientID%>').val("ICD 10");
    }

    function setTBBlankValue() {
        $('#<%=txtDiagnosisCode.ClientID%>').val("");
        $('#<%=ddlICDVersion.ClientID%>').val("");
        $('#<%=txtDiagnosisCodeDescription.ClientID%>').val("");
    }
    function ValidateDiagnosisCode(ctrl, errorMessage) {
        var DiagnosisCode = $(ctrl).val();
        var ddlICDVer = "ICD 10";
        var valid = true;
        var APIToken = $("[id*=hdnAccessToken]").val();
        $.ajax({
            type: "GET",
            async: false,
            url: webApiClaims + "GetDiagnosisCodeDescrption?Code=" + DiagnosisCode + "&&lICDVer=" + ddlICDVer,
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                "Authorization": "Bearer " + APIToken
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result === "") {
                    $("[id*=TerminalErrorMessage]").text(errorMessage);
                    valid = false;
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                $("[id*=TerminalErrorMessage]").text(errorMessage);
                valid = false;
            }
        });
        return valid;
    }
</script>
<ajax:Accordion ID="AccordionHospiceTerminalIllnessDiagnosis" runat="Server" SelectedIndex="0" EnableViewState="false"
    HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
    AutoSize="None" FadeTransitions="true" TransitionDuration="250" FramesPerSecond="40" RequireOpenedPane="false"
    SuppressHeaderPostbacks="true">

    <Panes>
        <ajax:AccordionPane ID="AccordionHospiceTerminalIllnessDiagnosisPane" runat="server" HeaderCssClass="accordionHeader" HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent">
            <Header>
                <asp:Label ID="lblHospiceOtherPayerSpan" class="expandcollapse" runat="server" Text="- * HOSPICE TERMINAL ILLNESS DIAGNOSIS"></asp:Label>
            </Header>
            <Content>
                 <asp:HiddenField ID="hdSelectedSearchTDButtonGridRowId" runat="server" ClientIDMode="Static" />
                 <asp:HiddenField ID="hdSelectedSearchTDColumnId" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdSelectedSearchTDBehaviorID" runat="server" ClientIDMode="Static" />
                <div class="row" style="text-align: center; width: 97%; margin-left: 1%">
                    <asp:GridView ID="gvHospiceTerminalIllnessDiagnosis" runat="server" ShowFooter="true" AutoGenerateColumns="False"
                        HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true" 
                        CssClass="gridViewSmallFont" EmptyDataText="No Data Found." OnPageIndexChanging="gvHospiceTerminalIllnessDiagnosis_PageIndexChanging"
                        OnRowEditing="gvHospiceTerminalIllnessDiagnosis_RowEditing" OnRowUpdating="gvHospiceTerminalIllnessDiagnosis_RowUpdating" OnRowCancelingEdit="gvHospiceTerminalIllnessDiagnosis_RowCancelingEdit"
                        OnRowDeleting="gvHospiceTerminalIllnessDiagnosis_RowDeleting"  OnRowDataBound="gvHospiceTerminalIllnessDiagnosis_RowDataBound" AlternatingRowStyle-BackColor="White" GridLines="Horizontal">
                        <Columns>
                            <asp:TemplateField HeaderText="Benefit Line No" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <asp:Label ID="lblBenefitLineNo" runat="server" Text='<%# Bind("BenPeriod") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:HiddenField ID="hdnBenefitLineNo" runat="server" Value='<%#Bind("BenPeriod") %>'/>
                                    <asp:DropDownList ID="eddlBenefitLineNo" runat="server" onchange="javascript:BenefitLineNumberChange(this,'Edit');"></asp:DropDownList>
                                     <span style="color:red; display:none"><br />Benefit Line No is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:HiddenField ID="hdnBenefitLineNo" runat="server"/>
                                    <asp:DropDownList ID="fddlBenefitLineNo" runat="server" onchange="javascript:BenefitLineNumberChange(this, 'Add');"></asp:DropDownList>
                                     <span style="color:red; display:none"><br />Benefit Line No is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Benefit Period Type" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
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
                            <asp:TemplateField HeaderText="Benefit Period" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
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
                           <%-- <asp:TemplateField HeaderText="Segment Indicator" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblSegmentIndicator" runat="server" Text='<%# Bind("SegmentIndicator") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:HiddenField ID="hdnBenefitSegmentIndicator" runat="server" Value='<%#Bind("SegmentIndicator") %>' />
                                    <asp:DropDownList ID="eddlSegmentIndicator" runat="server" onchange="javascript:BenefitSegmentIndicatorChange(this);"></asp:DropDownList>
                                    <span style="color:red; display:none"><br />Segment Indicator is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="fddlSegmentIndicator" runat="server"></asp:DropDownList>
                                    <span style="color:red; display:none"><br />Segment Indicator is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Primary Terminal Diagnosis" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="175px">
                                <ItemTemplate>
                                    <asp:Label ID="lblPrimaryTerminalDiagnosis" runat="server" Text='<%# Bind("PrimeTermDiag") %>'></asp:Label>&nbsp;
                                     
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtPrimaryTerminalDiagnosis" MaxLength="7" runat="server" Text='<%# Bind("PrimeTermDiag") %>'></asp:TextBox>
                                    <span style="color:red; display:none"><br />Primary Terminal Diagnosis code required</span>
                                     <asp:LinkButton ID="lnkSearchTD" runat="server" Text="Search" OnClientClick="DiagnosisSearchClickEvent('Update','etxtPrimaryTerminalDiagnosis','modelHLTCFProviderSearchTD')" />
                                    <ajax:ModalPopupExtender ID="DiaHLTCFProviderSearchTD" runat="server" BehaviorID="modelHLTCFProviderSearchTD" 
                                        PopupControlID="pnlHLTCFProviderSearchTD" TargetControlID="lnkSearchTD"
                                        BackgroundCssClass="modalBackground" CancelControlID="btnCloseTD"  />
                                    </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtPrimaryTerminalDiagnosis" MaxLength="7" runat="server"></asp:TextBox>
                                    <span style="color:red; display:none"><br />Primary Terminal Diagnosis code required</span>
                                    <asp:LinkButton ID="lnkSearchTD1" runat="server" Text="Search" OnClientClick="DiagnosisSearchClickEvent('Add','ftxtPrimaryTerminalDiagnosis','modelHLTCFProviderSearchTD1')" />
                                    <ajax:ModalPopupExtender ID="DiaHLTCFProviderSearchTD1" runat="server" BehaviorID="modelHLTCFProviderSearchTD1" 
                                        PopupControlID="pnlHLTCFProviderSearchTD" TargetControlID="lnkSearchTD1"
                                        BackgroundCssClass="modalBackground" CancelControlID="btnCloseTD"  />
                                    <span style="color:red; display:none"><br />Primary Terminal Diagnosis code required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Terminal Diagnosis 2" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="175px">
                                <ItemTemplate>
                                    <asp:Label ID="lblTerminalDiagnosis2" runat="server"  Text='<%# Bind("TermDiag2") %>'></asp:Label>&nbsp;
                                     </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtTerminalDiagnosis2" runat="server"  MaxLength="7" Text='<%# Bind("TermDiag2") %>'></asp:TextBox>                                    
                                <asp:LinkButton ID="lnkSearchTD2" runat="server" Text="Search" OnClientClick="DiagnosisSearchClickEvent('Update','etxtTerminalDiagnosis2','modelHLTCFProviderSearchTD2')" />
                                    <ajax:ModalPopupExtender ID="DiaHLTCFProviderSearchTD2" runat="server" BehaviorID="modelHLTCFProviderSearchTD2" 
                                        PopupControlID="pnlHLTCFProviderSearchTD" TargetControlID="lnkSearchTD2"
                                        BackgroundCssClass="modalBackground" CancelControlID="btnCloseTD"  />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtTerminalDiagnosis2" runat="server"  MaxLength="7"></asp:TextBox>  
                                    <asp:LinkButton ID="lnkSearchTD3" runat="server" Text="Search" OnClientClick="DiagnosisSearchClickEvent('Add','ftxtTerminalDiagnosis2','modelHLTCFProviderSearchTD3')" />
                                    <ajax:ModalPopupExtender ID="DiaHLTCFProviderSearchTD3" runat="server" BehaviorID="modelHLTCFProviderSearchTD3" 
                                        PopupControlID="pnlHLTCFProviderSearchTD" TargetControlID="lnkSearchTD3"
                                        BackgroundCssClass="modalBackground" CancelControlID="btnCloseTD"  />
                                </FooterTemplate>
                            </asp:TemplateField>
                                <asp:TemplateField HeaderText="Terminal Diagnosis 3" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="175px">
                                <ItemTemplate>
                                    <asp:Label ID="lblTerminalDiagnosis3" runat="server" Text='<%# Bind("TermDiag3") %>'></asp:Label>&nbsp;
                                    
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtTerminalDiagnosis3" runat="server" MaxLength="7"   Text='<%# Bind("TermDiag3") %>'></asp:TextBox>                                    
                                <asp:LinkButton ID="lnkSearchTD4" runat="server" Text="Search" OnClientClick="DiagnosisSearchClickEvent('Update','etxtTerminalDiagnosis3','modelHLTCFProviderSearchTD4')" />
                                    <ajax:ModalPopupExtender ID="DiaHLTCFProviderSearchTD4" runat="server" BehaviorID="modelHLTCFProviderSearchTD4" 
                                        PopupControlID="pnlHLTCFProviderSearchTD" TargetControlID="lnkSearchTD4"
                                        BackgroundCssClass="modalBackground" CancelControlID="btnCloseTD"  />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtTerminalDiagnosis3" runat="server" MaxLength="7"></asp:TextBox>  
                                    <asp:LinkButton ID="lnkSearchTD5" runat="server" Text="Search" OnClientClick="DiagnosisSearchClickEvent('Add','ftxtTerminalDiagnosis3','modelHLTCFProviderSearchTD5')" />
                                    <ajax:ModalPopupExtender ID="DiaHLTCFProviderSearchTD5" runat="server" BehaviorID="modelHLTCFProviderSearchTD5" 
                                        PopupControlID="pnlHLTCFProviderSearchTD" TargetControlID="lnkSearchTD5"
                                        BackgroundCssClass="modalBackground" CancelControlID="btnCloseTD"  />
                                </FooterTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Diagnosis Effective Date" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="175px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEffectiveDate" runat="server" Text='<%# Bind("DiagEffDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtDiagnosisEffectiveDate" autocomplete="off" runat="server" Text='<%# Bind("DiagEffDate", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                     <ajax:CalendarExtender ID="clDiagEffDate" TargetControlID="etxtDiagnosisEffectiveDate" runat="server" EnabledOnClient="true" />
                                    <span style="color:red; display:none"><br />Diagnosis effective date is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtDiagnosisEffectiveDate" autocomplete="off" runat="server"></asp:TextBox>
                                     <ajax:CalendarExtender ID="clDiagEffDate" TargetControlID="ftxtDiagnosisEffectiveDate" runat="server" EnabledOnClient="true" />
                                    <span style="color:red; display:none"><br />Diagnosis effective date is required</span>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Diagnosis EndDate" HeaderStyle-CssClass="GridviewHeaderAsterisk" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("DiagEndDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="etxtDiagnosisEndDate" autocomplete="off" runat="server" Text='<%# Bind("DiagEndDate", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                    <ajax:CalendarExtender ID="clDiagEndDate" TargetControlID="etxtDiagnosisEndDate" runat="server" EnabledOnClient="true" />
                                    <span style="color:red; display:none"><br />Diagnosis end date is required</span>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="ftxtDiagnosisEndDate" autocomplete="off" runat="server"></asp:TextBox>
                                    <ajax:CalendarExtender ID="clDiagEndDate" TargetControlID="ftxtDiagnosisEndDate" runat="server" EnabledOnClient="true"/>
                                    <span style="color:red; display:none"><br />Diagnosis end date is required</span>
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
                                    <asp:Button ID="btnUpdate" Text="Update" runat="server" CommandName="Update" CssClass="button"  />
                                    &nbsp;
                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" CssClass="button" CausesValidation="false" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="ftnAdd" runat="server" Text="Add New" OnClick="fbtnAdd_Click" CssClass="button" Style="width: auto !important;"
                                         />
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
                <asp:Panel ID="pnlHLTCFProviderSearchTD" runat="server" CssClass="modalPopup" Style="display: none; min-height: 250px; min-width: 610px; height: auto; width: auto;">
                    <asp:Panel ID="pnlCE1" runat="server">
                        <asp:Button runat="server" ID="btnCloseTD" Text="X" Style="float: right; background-image: none; border: 0px; border-radius: 0px;" CausesValidation="false" OnClientClick="javascript: setTBBlankValue();" />
                        <div style="text-align: left; padding: 42px; overflow-x: auto; overflow-y: auto" class="container-fluid">
                        
                            <div class="row" style="text-align: center;">
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label"><b>Diagnosis Code</b>
            <asp:TextBox ID="txtDiagnosisCode" MaxLength="7" CssClass="ohio-field-input" runat="server">
            </asp:TextBox>
        </span>

    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label"><b>ICD Version </b>
            <asp:DropDownList ID="ddlICDVersion" CssClass="ohio-field-label" runat="server">
                <asp:ListItem Value="ICD 10" Text="ICD 10" Selected="True"></asp:ListItem>               
            </asp:DropDownList>
        </span>
    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label"><b>Code Description</b>
            <asp:TextBox ID="txtDiagnosisCodeDescription" MaxLength="400" CssClass="ohio-field-input" runat="server">
            </asp:TextBox>
        </span>

    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <%--<asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus"  Style="background-color: darkslateblue !important" />--%>
        <input type="button" id="btnSearch"  onclick="SearchDiagnosisinfo();" value="Search" class="buttonBoxFocus" style="background-color: darkslateblue !important" />
    </div>
</div>
                             <div style="text-align: center">
                                <span id="TerminalSearchErrorMessage" style="color: red;"></span>
                            </div>
                             <div style="width: 100%; height: 400px; overflow: scroll">
                             <mms:SortablePagingGridView
    ID="gvHospiceTerminalIllnessDiagnosisSearch"
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
    GridViewSortColumn="ICD10Diag" GridViewSortDirection="Ascending">
    <Columns>

        <asp:BoundField DataField="ICD10Diag" HeaderText="Diagnosis Code" SortExpression="ICD10Diag" />
        <asp:BoundField DataField="ICDVersion" HeaderText="ICD Version" SortExpression="ICDVersion" />
        <asp:BoundField DataField="DiagDesc" HeaderText="Diagnosis Code Description" SortExpression="DiagDesc" />
        <asp:TemplateField>
                                        <ItemTemplate>
                                            <input type="button" id="lnkSelect" onclick="FillSelectedDiagnosisInfo(this); setTBBlankValue();" value="Select" class="buttonBoxFocus" style="background-color: darkslateblue !important" />
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
                                <span id="TerminalErrorMessage" style="color: red;"></span>
                            </div>
                </div> 
               
            </Content>

        </ajax:AccordionPane>
    </Panes>

</ajax:Accordion>