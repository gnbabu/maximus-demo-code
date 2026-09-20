<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SearchPriorAuthorization, App_Web_wenzyumt" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>
<%@ register src="~/PopupControls/MessageBox.ascx" tagname="MessageBox" tagprefix="cc2" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">

<script type="text/javascript">
    function DisableBackButton() {
        window.history.forward()
    }
    DisableBackButton();
    window.onload = DisableBackButton;
    window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
    window.onunload = function () { void (0) }

    function CollapseExpand(obj) {
        var sp = obj.getElementsByTagName('span')[1];
        var spText = sp.innerHTML;
        var plusIndex = spText.indexOf("+");

        if (plusIndex == 1)
            sp.innerHTML = spText.replace("+", "-");
        else
            sp.innerHTML = spText.replace("-", "+");
    }
    function resetAllControls() {
        $("#form1").find('input:text, input:password, input:file, select, textarea').val('');
    }

    function onlyNumbers(evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    }
    function checkDate(sender, args) {
        if (sender._selectedDate > new Date()) {
            //alert("You cannot select a day earlier than today!");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
        }
    }

    function checkDate_todate(sender, args) {
        // var fromdate = new date($("#<%=txtSubmissiondate.ClientID%>").val());
        var fromdate = document.getElementById("#<%=txtSubmissiondate.ClientID%>");
        console.log(fromdate);
        console.log(sender._selectedDate);
        if (sender._selectedDate > fromdate) {
            //alert("You cannot select a day earlier than From date!");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
        }
    }
    function checkDate_todate(sender, args) {
        // var fromdate = new date($("#<%=txtBirthDate.ClientID%>").val());
        var fromdate = document.getElementById("#<%=txtBirthDate.ClientID%>");
        console.log(fromdate);
        console.log(sender._selectedDate);
        if (sender._selectedDate > fromdate) {
            //alert("You cannot select a day earlier than From date!");
            sender._selectedDate = new Date();
            // set the date back to the current date
            sender._textbox.set_Value(sender._selectedDate.format(sender._format))
        }
    }
    function setCookie(c_name, value, exdays) {
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + exdays);
        var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
        document.cookie = c_name + "=" + c_value;
    }
    function setClientCookie() {
        var id = 10015;
        setCookie("selectedOption", id, 1);
    }
    function ValidatePASearch() {

        var errorMsg = "";
        document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';

        if ($('#ctl00_MainContent_uc2SearchPriorAuthorization_txtPriorAuthNumber').val() != ''
            && $('#ctl00_MainContent_uc2SearchPriorAuthorization_txtPriorAuthNumber').val().length < 10) {
            errorMsg = errorMsg + "<li> 10-digit Prior Auth Number is required </li> \n";
        }

        if ($('#ctl00_MainContent_uc2SearchPriorAuthorization_txtICDCode').val() != ''
            && $('#ctl00_MainContent_uc2SearchPriorAuthorization_txtICDCode').val().length < 7) {
            errorMsg = errorMsg + "<li> Please Enter ICD CODE 7 digits Alphanumeric is required </li> \n";
        }

        if ($('#ctl00_MainContent_uc2SearchPriorAuthorization_txtRevenuecode').val() != ''
            && $('#ctl00_MainContent_uc2SearchPriorAuthorization_txtRevenuecode').val().length < 3) {
            errorMsg = errorMsg + "<li> Revenue Code should be 3 characters </li> \n";
        }
        if ($('#ctl00_MainContent_uc2SearchPriorAuthorization_txtProcedureCode').val() != ''
            && $('#ctl00_MainContent_uc2SearchPriorAuthorization_txtProcedureCode').val().length < 5) {
            errorMsg = errorMsg + "<li>Procedure Code should be 5 characters </li> \n";
        }
        if ($('#ctl00_MainContent_uc2SearchPriorAuthorization_txtorderProvnpi').val() != ''
            && $('#ctl00_MainContent_uc2SearchPriorAuthorization_txtorderProvnpi').val().length < 10) {
            errorMsg = errorMsg + "<li> Ordering Provider NPI 10 digits number is required </li> \n";
        }

        if (errorMsg != "") {
            //$('#ErrorMessage').html(errorMsg);
            return false;
        }
        else {
            return true;
        }

    }

    function btnClearAllFields_Click() {
        $('#ErrorMessage').text('');
        $('#ErrorMessage').hide();
        $("#form1").find('input:text').val('');
        return true;
    }

    function validatePCText(osrc, args) {
        var textvalue = args.Value;
        if (textvalue != null || textvalue != "") {
            if (textvalue.length < 5) {
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
        }
        args.IsValid = true;
    }

    function validateRCText(osrc, args) {
        var textvalue = args.Value;
        if (textvalue != null || textvalue != "") {
            if (textvalue.length < 3) {
                args.IsValid = false;
                return;
            }
            args.IsValid = true;
        }
        args.IsValid = true;
    }
    function CollapseExpandPriorAuthSearch() {
        var button = document.getElementById("priorauthSearch").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("priorauthSearch").addClass("panelHeaderStyle");
        } else {
            button = "true"
            $("priorauthSearch").addClass("panelHeaderStyle");
        }
        document.getElementById("priorauthSearch").setAttribute("aria-expanded", button);
    };
    function CollapseExpandPriorAuthSearchResult() {
        var button = document.getElementById("priorauthSearchResult").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("priorauthSearchResult").addClass("panelHeaderStyle");
        } else {
            button = "true"
            $("priorauthSearchResult").addClass("panelHeaderStyle");
        }
        document.getElementById("priorauthSearchResult").setAttribute("aria-expanded", button);
    };

</script>

<style type="text/css">
    .formField {
        border: 1px solid #ccc;
        background-color: #FFFFFF;
        color: #000;
        width: 274px;
    }

    select {
        min-width: 273px;
        height: 41px;
        border: 1px solid #ccc;
    }

    .formLabel200 {
        width: 178px;
        /*width: 162px;*/
    }

    .col-lg-6 {
        /* width: 50%; */
    }

    .pnl-seach {
        min-height: 530px;
        min-width: 300px;
        height: auto;
        width: auto;
        max-width: 1200px;
    }

    .watermarked {
        background-color: #F7F6F3;
        border: solid 1px #808080;
        padding: 3px;
        color: #717171;
    }

    .unwatermarked {
        border: solid 1px #808080;
        padding: 3px;
        color: Gray;
    }

    .pagePnlSearchHeader {
        color: white;
        font-size: 24px;
        font-weight: bold;
    }

    .pagePnlTextField {
        font-size: 16px;
    }

    .mySearchButton {
        box-shadow: inset 0px 1px 0px 0px #9acc85;
        background: linear-gradient(to bottom, #288128 5%, #288128 100%);
        background-color: #288128;
        border: 1px solid #288128;
        display: inline-block;
        cursor: pointer;
        color: #ffffff;
        font-family: Arial;
        font-size: 18px;
        font-weight: bold;
        padding: 6px 12px;
        text-decoration: none;
    }

    .myCancelButton {
        box-shadow: inset 0px 1px 0px 0px #9acc85;
        background: linear-gradient(to bottom, #D03A3A 5%, #D03A3A 100%);
        background-color: #D03A3A;
        border: 1px solid #D03A3A;
        display: inline-block;
        cursor: pointer;
        color: #ffffff;
        font-family: Arial;
        font-size: 18px;
        font-weight: bold;
        padding: 6px 12px;
        text-decoration: none;
    }

    .collapsiblePanelContainer {
        height: 0;
        overflow: hidden;
    }

    .style3 {
        color: black;
        font-weight: bold;
    }

    td, h1, h2 {
        margin: 3px !important;
        padding-left: 5px !important;
    }

    .panelHeaderStyle {
        background-color: #2297bc;
        border-style: none;
    }

    .col-sm-3 {
        text-align: right;
    }
</style>
<asp:Label ID="lblGeneralErr" runat="server" Visible="false" CssClass="failureNotification" />
<div class="row col-sm-15" style="border: groove; margin-left: 10px; width: auto; height: auto">

    <ajax:collapsiblepanelextender id="cpeAuthSearch" runat="server" collapsed="false" targetcontrolid="pnlAuthSearch"
        expandcontrolid="pnlsepAuthSearch" collapsecontrolid="pnlsepAuthSearch"
        expandedtext="-" collapsedsize="0" scrollcontents="true" collapsedtext="+" expanddirection="Vertical"
        suppresspostback="true" textlabelid="lblsepContact" />
    <span style="color: #D33421; font-size: 14pt !important; font-weight: 100 !important">An asterisk * indicates a required field</span>
    <br />
    <asp:Label runat="server" Style="color: #D33421; font-size: 14pt !important; font-weight: 100 !important" ID="PASearchHelpTextID" />
    <asp:Panel runat="server" ID="pnlsepAuthSearch" class="CollapsingSeparator" Style="background-color: #2197bb;"
        ToolTip="Click to Expand/Collapse" CssClass="OwnerAuthSearch">

        <div class="pageHeader pH2">
            <table width="100%">
                <tr>
                    <td align="left" style="color: white">
                        <h1><span style="color: white; font-weight: bold;">
                            <button type="button" class="panelHeaderStyle" tabindex="0" id="priorauthSearch" aria-expanded="true" onclick="CollapseExpandPriorAuthSearch()">PRIOR AUTHORIZATION SEARCH</button></span></h1>
                        <asp:Label runat="server" ID="lblsepContact1" />
                    </td>
                    <td style="padding-right: 10px; margin: 30px; width: 35px">
                        <asp:Label runat="server" ID="lblsepContact" Style="color: white" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlAuthSearch" runat="server" CssClass="pnl-seach" ScrollBars="Auto">
        <div id="divErrorMessage" style="color: red; padding-left: 30px; margin: 10px;">
            <span id="ErrorMessage"></span>
            <asp:ValidationSummary ID="vsPriorAuthSearch" DisplayMode="List" ShowSummary="true" runat="server" CssClass="failureNotification" ValidationGroup="valProviderInfoheader" />
        </div>
        <div class="col-sm-12">
            <div class="d-flex justify-content-center text-primary">
                <button id="btnloading" runat="server" style="position: fixed; z-index: 9999; display: none; left: 45%; top: 50%; border: none; background-color: transparent">
                    <i class="fa fa-3x fa-spinner mr-2 fa-spin" area-hidden="true"></i>Loading</button>
            </div>
            <div class="row">
                <div class="col-lg-3 text-right">
                    <span style="font-size: 18px; text-align: right; font-weight: bold;">Prior Authorization Number : </span>
                </div>
                <div class="col-lg-3">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtPriorAuthNumber" aria-label="Prior Authorization Number" runat="server" CssClass="form-group form-control" MaxLength="50" Style="height: 30px;" />
                        <asp:RegularExpressionValidator runat="server" ID="revtxtPriorAuthNumber"
                            Display="Dynamic" ValidationGroup="valProviderInfoHeader"
                            ControlToValidate="txtPriorAuthNumber"
                            ValidationExpression="^[a-zA-Z0-9]*$"
                            ForeColor="Red" ErrorMessage="Please enter valid PA Number.">
                        </asp:RegularExpressionValidator>

                    </span>
                </div>
                <div class="col-lg-3 text-right">
                    <span style="font-size: 18px; text-align: right; font-weight: bold;">Patient Tracking Number : </span>
                </div>
                <div class="col-lg-3">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtPatientTrackingNumber" aria-label="Patient Tracking Number" runat="server" CssClass="form-group form-control" MaxLength="50" Style="height: 30px;" />
                    </span>
                </div>
            </div>
        </div>
        <div class="col-sm-12">
            <div class="row">
                <div class="col-lg-3 text-right">
                    <span style="font-size: 18px; text-align: right; font-weight: bold;">Medicaid Billing Number : </span>
                </div>
                <div class="col-lg-3">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtMedicaidBillingNumber" aria-label="Medicaid Billing Number" runat="server" CssClass="form-group form-control" MaxLength="12" Style="height: 30px;" />
                        <asp:RegularExpressionValidator runat="server" ID="revMedicaidBillingNumber"
                            Display="Dynamic" ValidationGroup="valProviderInfoHeader"
                            ControlToValidate="txtMedicaidBillingNumber"
                            ValidationExpression="^[0-9]{12}$"
                            ForeColor="Red" ErrorMessage="<br />12-digit Medicaid Billing Number is required.">
                        </asp:RegularExpressionValidator>
                    </span>
                </div>
                <div class="col-lg-3 text-right">
                    <span style="font-size: 18px; text-align: right; font-weight: bold;">ICD Procedure Code : </span>
                </div>
                <div class="col-lg-3">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtICDCode" aria-label="ICD Procedure Code" runat="server" CssClass="form-group form-control" MaxLength="7" Style="height: 30px;" />
                        <asp:RegularExpressionValidator ID="revICDCode" runat="server" ErrorMessage="Please Enter ICD CODE 7 digits Alphanumeric is required" Display="Dynamic"
                            ForeColor="Red" ControlToValidate="txtICDCode" ValidationGroup="valProviderInfoheader"
                            ValidationExpression="^[a-zA-Z0-9.@]{7}$"></asp:RegularExpressionValidator>
                    </span>
                </div>
            </div>
        </div>
        <div class="col-sm-12">
            <div class="row">
                <div class="col-lg-3 text-right">
                    <span style="font-size: 18px; text-align: right; font-weight: bold;">Date of Birth : </span>
                </div>
                <div class="col-lg-3">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtBirthDate" aria-label="Date of Birth" runat="server" CssClass="form-group form-control" Style="height: 30px;" MaxLength="10" />
                        <ajax:textboxwatermarkextender id="txtWater1" runat="server" targetcontrolid="txtBirthDate" watermarktext="mm/dd/yyyy"
                            watermarkcssclass="watermarked" />
                        <ajax:calendarextender id="ceDataofbirth" runat="server" format="MM/dd/yyyy" targetcontrolid="txtBirthDate"
                            popupposition="Right" />
                        <ajax:maskededitextender id="Ext1" runat="server" autocomplete="true" clearmaskonlostfocus="true"
                            culturedateformat="en-US" culturedateplaceholder=""
                            enabled="true" mask="99/99/9999" masktype="Date" targetcontrolid="txtBirthDate" userdateformat="MonthDayYear"
                            messagevalidatortip="true" />
                        <ajax:maskededitvalidator
                            id="MaskedEditValidator1"
                            runat="server"
                            controltovalidate="txtBirthDate"
                            controlextender="Ext1"
                            isvalidempty="true"
                            setfocusonerror="true"
                            emptyvaluemessage="Date of birth is empty"
                            invalidvaluemessage="Entered date not valid. Enter in mm/dd/yyyy format"
                            display="Dynamic" forecolor="Red" />
                        <asp:CompareValidator ID="cvBirthDate" runat="server" ValidationGroup="valOrgInfo"
                            Type="Date" Operator="LessThan" ControlToValidate="txtBirthDate"
                            ErrorMessage="Select a valid smaller date than today" ValueToCompare="MM/dd/yyyy"
                            SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:CompareValidator>
                    </span>
                </div>
                <div class="col-lg-3 text-right">
                    <span style="font-size: 18px; text-align: right; font-weight: bold;">Procedure Code : </span>
                </div>
                <div class="col-lg-3">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtProcedureCode" aria-label="Procedure Code" runat="server" CssClass="form-group form-control" MaxLength="5" Style="height: 30px;" />
                        <asp:RegularExpressionValidator ID="revProcdurecode" runat="server" ErrorMessage="Procedure Code should be 5 characters" Display="Dynamic"
                            ForeColor="Red" ControlToValidate="txtProcedureCode" ValidationGroup="valProviderInfoheader"
                            ValidationExpression="^[a-zA-Z0-9.@]{0,7}$"></asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="custxtProcedureCode" ValidateEmptyText="false" runat="server" ControlToValidate="txtProcedureCode"
                            Display="Dynamic" ValidationGroup="valProviderInfoheader" ForeColor="Red" OnServerValidate="ValidatetxtProcedureCode"
                            ErrorMessage="*" ShowSummary="true" EnableClientScript="true" ClientValidationFunction="validatePCText">Procedure Code should be 5 characters</asp:CustomValidator>
                    </span>
                </div>
            </div>
        </div>
        <div class="col-sm-12">
            <div class="row">
                <div class="col-lg-3 text-right">
                    <span style="font-size: 18px; text-align: right; font-weight: bold;">PA Submission Date : </span>
                </div>
                <div class="col-lg-3">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtSubmissiondate" aria-label="PA Submission Date" runat="server" CssClass="form-group form-control" Style="height: 30px;" MaxLength="10" />
                        <ajax:textboxwatermarkextender id="TextBoxWatermarkExtender1" runat="server" targetcontrolid="txtSubmissiondate" watermarktext="mm/dd/yyyy"
                            watermarkcssclass="watermarked" />
                        <ajax:calendarextender id="ceSubmissiondate" runat="server" format="MM/dd/yyyy" targetcontrolid="txtSubmissiondate"
                            popupposition="Right" />
                        <%--OnClientDateSelectionChanged="ValidateSubmissionDate"--%>
                        <ajax:maskededitextender id="MaskedEditExtender1" runat="server" autocomplete="true" clearmaskonlostfocus="true"
                            culturedateformat="en-US" culturedateplaceholder=""
                            enabled="true" mask="99/99/9999" masktype="Date" targetcontrolid="txtSubmissiondate" userdateformat="MonthDayYear" />
                        <ajax:maskededitvalidator
                            id="MaskedEditValidator2"
                            runat="server"
                            controltovalidate="txtSubmissiondate"
                            controlextender="MaskedEditExtender1"
                            isvalidempty="true"
                            setfocusonerror="true"
                            display="Dynamic"
                            invalidvaluemessage="Entered date not valid. Enter in mm/dd/yyyy format"
                            forecolor="Red" />
                        <asp:CompareValidator ID="cvtxtSubmissiondate" runat="server" ValidationGroup="valOrgInfo"
                            Type="Date" Operator="LessThan" ControlToValidate="txtSubmissiondate"
                            ErrorMessage="Select a valid smaller date than today" ValueToCompare="MM/dd/yyyy"
                            SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:CompareValidator>
                    </span>
                </div>
                <div class="col-lg-3 text-right">
                    <span style="font-size: 18px; text-align: right; font-weight: bold;">Revenue Code : </span>
                </div>
                <div class="col-lg-3">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtRevenuecode" aria-label="Revenue Code" runat="server" CssClass="form-group form-control" ValidationGroup="valProviderInfoheader" MaxLength="4" Style="height: 30px;" onkeypress="return onlyNumbers(event)" />
                        <asp:RegularExpressionValidator ID="revRevenueCode" runat="server" ErrorMessage="Revenue Code should be 4 characters" Display="Dynamic"
                            ForeColor="Red" ControlToValidate="txtRevenuecode" ValidationGroup="valProviderInfoheader"
                            ValidationExpression="^[a-zA-Z0-9.@]{0,4}$"></asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="custxtRevenuecode" ValidateEmptyText="false" runat="server" ControlToValidate="txtRevenuecode"
                            Display="Dynamic" ValidationGroup="valProviderInfoheader" ForeColor="Red" OnServerValidate="ValidatetxtRevenuecode"
                            ErrorMessage="*" ShowSummary="true" EnableClientScript="true" ClientValidationFunction="validateRCText">Revenue Code should be 4 characters</asp:CustomValidator>
                    </span>
                </div>
            </div>
        </div>
        <div class="col-sm-12">
            <div class="row">
                <div class="col-lg-3 text-right">
                    <span style="font-size: 18px; text-align: right; font-weight: bold;">Status : </span>
                </div>
                <div class="col-lg-3">
                    <span style="text-align: left;">
                        <asp:DropDownList ID="ddlStatus" aria-label="Status" CssClass="form-group form-control" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True" Style="height: 30px; min-width: 80px;">
                        </asp:DropDownList>
                    </span>
                </div>
                <div class="col-lg-3 text-right">
                    <span style="font-size: 18px; text-align: right; font-weight: bold;">Diagnosis Code : </span>
                </div>
                <div class="col-lg-3">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtDiagnoisCode" aria-label="Diagnosis Code" runat="server" CssClass="form-group form-control" MaxLength="7" Style="height: 30px;" />
                        <asp:RegularExpressionValidator ID="revDiagnoidcode" runat="server" ErrorMessage="Diagnosis Code 7 digits" Display="Dynamic"
                            ForeColor="Red" ControlToValidate="txtDiagnoisCode" ValidationGroup="valProviderInfoheader"
                            ValidationExpression="^[a-zA-Z0-9.@]{0,7}$"></asp:RegularExpressionValidator>
                    </span>
                </div>
            </div>
        </div>
        <div class="col-sm-12">
            <div class="row">
                <div class="col-lg-3 text-right">
                    <span style="font-size: 18px; text-align: right; font-weight: bold;">Ordering Provider NPI : </span>
                </div>
                <div class="col-lg-3">
                    <span style="text-align: left;">
                        <asp:TextBox ID="txtorderProvnpi" aria-label="Ordering Provider NPI" runat="server" CssClass="form-group form-control" MaxLength="10" Style="height: 30px;" />
                        <%--<asp:RegularExpressionValidator ID="revOrderingNPI" runat="server" ErrorMessage=" Ordering Provider NPI 10 digits number is required." Display="Dynamic"
                            ForeColor="Red" ControlToValidate="txtorderProvnpi" ValidationGroup="valProviderInfoheader"
                            ValidationExpression="^[0-9]{10}$"></asp:RegularExpressionValidator>--%>
                        <asp:RegularExpressionValidator ID="revOrderingNPI" runat="server" ErrorMessage="Ordering Provider NPI 10 digits number is required" Display="Dynamic"
                            ForeColor="Red" ControlToValidate="txtorderProvnpi" ValidationGroup="valProviderInfoheader"
                            ValidationExpression="^[0-9]{10}$"></asp:RegularExpressionValidator>
                    </span>
                </div>
                <div class="col-lg-3 text-right">
                    <span style="font-size: 18px; text-align: right; font-weight: bold;">PA Effective Date : </span>
                </div>
                <div class="col-lg-3">
                    <asp:TextBox ID="txtPAEffDate" aria-label="PA Effective Date" runat="server" CssClass="form-group form-control" Style="height: 30px;" MaxLength="10" />
                    <ajax:textboxwatermarkextender id="TextBoxWatermarkExtender2" runat="server" targetcontrolid="txtPAEffDate" watermarktext="mm/dd/yyyy"
                        watermarkcssclass="watermarked" />
                    <ajax:calendarextender id="cePAEffDate" runat="server" format="MM/dd/yyyy" targetcontrolid="txtPAEffDate"
                        popupposition="TopRight" />
                    <%--OnClientDateSelectionChanged="ValidatePAEffDate"--%>
                    <ajax:maskededitextender id="MaskedEditExtender2" runat="server" autocomplete="true" clearmaskonlostfocus="true"
                        culturedateformat="en-US" culturedateplaceholder=""
                        enabled="true" mask="99/99/9999" masktype="Date" targetcontrolid="txtPAEffDate" userdateformat="MonthDayYear" />

                    <ajax:maskededitvalidator
                        id="MaskedEditValidator3"
                        runat="server"
                        controltovalidate="txtPAEffDate"
                        controlextender="MaskedEditExtender2"
                        isvalidempty="true"
                        setfocusonerror="true"
                        display="Dynamic"
                        invalidvaluemessage="Entered date not valid. Enter in mm/dd/yyyy format"
                        forecolor="Red" />
                    <asp:CompareValidator ID="cvPAEffDate" runat="server" ValidationGroup="valOrgInfo"
                        Type="Date" Operator="LessThan" ControlToValidate="txtPAEffDate" ControlToCompare="txtPAExpDate"
                        ErrorMessage="PA Effective Date should be smaller than PA Expiry Date" ValueToCompare="MM/dd/yyyy"
                        SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:CompareValidator>
                    <%--  <asp:CompareValidator ID="cvPAEffDate1" runat="server" ValidationGroup="valOrgInfo"
                        Type="Date" Operator="GreaterThanEqual" ControlToValidate="txtPAEffDate"
                        ErrorMessage="Select Date greater than or equal to today" ValueToCompare="MM/dd/yyyy"
                        SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:CompareValidator>--%>
                </div>
            </div>
        </div>
        <div class="col-sm-12">
            <div class="row">
                <div class="col-lg-3 text-right">
                    <span style="font-size: 18px; text-align: right; font-weight: bold;"><span style="color: #e50000">* </span>Payer Name : </span>
                </div>
                <div class="col-lg-3">
                    <span style="text-align: left;">
                        <asp:DropDownList ID="ddlPayerName" aria-label="Payer Name" CssClass="form-group form-control" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True" Style="height: 30px; min-width: 80px;">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqPayerName" runat="server" ControlToValidate="ddlPayerName" ForeColor="Red"
                            ErrorMessage="* Payer Name is required" Text="Payer Name is required" Display="Dynamic" ValidationGroup="valProviderInfoHeader">
                        </asp:RequiredFieldValidator>
                    </span>
                </div>
                <div class="col-lg-3 text-right">
                    <span style="font-size: 18px; text-align: right; font-weight: bold;">PA Expiration Date : </span>
                </div>
                <div class="col-lg-3">
                    <asp:TextBox ID="txtPAExpDate" aria-label="PA Expiration Date" runat="server" CssClass="form-group form-control" Style="height: 30px;" MaxLength="10" />
                    <ajax:textboxwatermarkextender id="TextBoxWatermarkExtender3" runat="server" targetcontrolid="txtPAExpDate" watermarktext="mm/dd/yyyy"
                        watermarkcssclass="watermarked" />
                    <ajax:calendarextender id="cePAExpDate" runat="server" format="MM/dd/yyyy" targetcontrolid="txtPAExpDate"
                        popupposition="TopRight" />
                    <%--OnClientDateSelectionChanged="ValidatePAExpDate"--%>


                    <ajax:maskededitextender id="MaskedEditExtender3" runat="server" autocomplete="true" clearmaskonlostfocus="true"
                        culturedateformat="en-US" culturedateplaceholder=""
                        enabled="true" mask="99/99/9999" masktype="Date" targetcontrolid="txtPAExpDate" userdateformat="MonthDayYear" />
                    <ajax:maskededitvalidator
                        id="MaskedEditValidator4"
                        runat="server"
                        controltovalidate="txtPAExpDate"
                        controlextender="MaskedEditExtender3"
                        isvalidempty="true"
                        setfocusonerror="true"
                        display="Dynamic"
                        invalidvaluemessage="Entered date not valid. Enter in mm/dd/yyyy format"
                        forecolor="Red" />
                    <asp:CompareValidator ID="cvPAExpDate" runat="server" ValidationGroup="valOrgInfo"
                        Type="Date" Operator="GreaterThan" ControlToValidate="txtPAExpDate" ControlToCompare="txtPAEffDate"
                        ErrorMessage="PA Expiry Date should be greater than PA Effective Date" ValueToCompare="MM/dd/yyyy"
                        SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:CompareValidator>
                    <%--   <asp:CompareValidator ID="cvPAExpDate1" runat="server" ValidationGroup="valOrgInfo"
                        Type="Date" Operator="GreaterThanEqual" ControlToValidate="txtPAExpDate"
                        ErrorMessage="Select Date greater than or equal to today" ValueToCompare="MM/dd/yyyy"
                        SetFocusOnError="true" Display="Dynamic" ForeColor="Red"></asp:CompareValidator>--%>
                </div>
            </div>
        </div>
        <div class="col-sm-12">
            <div class="row">
                <div class="col-lg-3 text-right">
                    <span style="font-size: 18px; text-align: right; font-weight: bold;">Assignment Type : </span>
                </div>
                <div class="col-lg-3">
                    <span style="text-align: left;">
                        <asp:DropDownList ID="ddlAssignment" aria-label="Assignment Type" CssClass="form-group form-control" EnableViewState="true" runat="server"
                            AppendDataBoundItems="True" Style="height: 30px; min-width: 80px;">
                        </asp:DropDownList>
                    </span>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="col-sm-12">
            <div class="row">
                <div class="col-lg-6">
                </div>
                <div class="col-lg-3">
                    <div class="row">
                        <div class="col-sm-2"></div>
                        <div class="col-sm-5 text-right">
                            <span class="ohio-field" style="font-size: 18px; font-weight: bold">Max Records</span>
                        </div>
                        <div class="col-sm-5">
                            <span style="text-align: left;">
                                <asp:DropDownList ID="ddlPageSize" aria-label="Max Records" runat="server" AutoPostBack="false" OnSelectedIndexChanged="PageSize_Changed" Style="min-width: 100px; height: 30px">
                                    <asp:ListItem Text="5" Value="5" />
                                    <asp:ListItem Text="20" Value="20" />
                                    <%--<asp:ListItem Text="25" Value="25" />--%>
                                    <asp:ListItem Text="50" Value="50" />
                                    <asp:ListItem Text="100" Value="100" />
                                    <%--<asp:ListItem Text="200" Value="200" />--%> <%--OHPNM-9476Pagination UI - Remove Max records 200- Not in DSD.--%>
                                </asp:DropDownList>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-lg-3 text-center">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="return ValidatePASearch();"
                        ToolTip="Search data" class="mySearchButton" Font-Bold="True" ValidationGroup="valProviderInfoHeader" CausesValidation="True" Width="90px" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" OnClientClick="return btnClearAllFields_Click();" CssClass="myCancelButton"
                        Font-Bold="True" CausesValidation="True" Width="90px" />
                </div>
                <br />
            </div>
        </div>
    </asp:Panel>

    <ajax:collapsiblepanelextender id="cpePasearch" runat="server" collapsed="true" targetcontrolid="pnlPasearch"
        expandcontrolid="pnlsepPasearch" collapsecontrolid="pnlsepPasearch"
        expandedtext="-" collapsedsize="0" scrollcontents="true" collapsedtext="+" expanddirection="Vertical"
        suppresspostback="true" textlabelid="lblPriorAuthSearch" />


    <asp:Panel runat="server" ID="pnlsepPasearch" class="CollapsingSeparator" Style="background-color: #2197bb;"
        ToolTip="Click to Expand/Collapse" CssClass="OwnerAuthSearch">

        <div class="pageHeader pH2">
            <table width="100%">
                <tr>
                    <td align="left" style="color: white">
                        <h2><span style="color: white; font-weight: bold; font-size: 25px">
                            <button type="button" class="panelHeaderStyle" tabindex="0" id="priorauthSearchResult" aria-expanded="false" onclick="CollapseExpandPriorAuthSearchResult()">PRIOR AUTHORIZATION SEARCH RESULT</button></span></h2>
                        <asp:Label runat="server" ID="lblPriorAuthSearch1" />
                    </td>
                    <td style="padding-right: 10px; margin: 30px; width: 35px">
                        <asp:Label runat="server" ID="lblPriorAuthSearch" Style="color: white" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlPasearch" runat="server" CssClass="pnl-seach">
        <div class="divGrid">
            <asp:GridView ID="gvPasearch" runat="server" Style="width: 145%; border-collapse: collapse; margin-right: -539px;" AllowSorting="false"
                ShowHeaderWhenEmpty="true" EmptyDataText="No search results found" AutoGenerateColumns="false" HorizontalAlign="Left"
                AllowPaging="true" AllowCustomPaging="true" OnPageIndexChanging="gvPasearch_PageIndexChanging">

                <Columns>
                    <asp:TemplateField ShowHeader="True" HeaderText="PA Number">
                        <ItemTemplate>
                            <asp:LinkButton
                                ID="lnkPriorAuthId"
                                runat="server"
                                CausesValidation="false"
                                CommandArgument='<%# Eval("PriorAuthorizationID")+","+ Eval("PayorType") %>'
                                CommandName="ShowPriorAuthDetails"
                                Text='<%# Eval("PriorAuthorizationID") %>'
                                CssClass="gridLink" OnClick="lnkPriorAuthView_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PriorAuthorizationID" HeaderText="PA Number" Visible="false" />
                    <asp:BoundField DataField="PayorType" HeaderText="Payor Type" Visible="false" />
                    <asp:BoundField DataField="MemberID" HeaderText="Medicaid Billing number" />
                    <asp:BoundField DataField="PatientTrackingNumber" HeaderText="Patient Tracking Number" Visible="false" />
                    <asp:TemplateField ShowHeader="True" HeaderText="Patient Tracking Number">
                        <ItemTemplate>
                            <asp:LinkButton
                                ID="lnkPriorAuthPTId"
                                runat="server"
                                CausesValidation="false"
                                CommandArgument='<%# Eval("PatientTrackingNumber")+","+ Eval("PayorType") %>'
                                CommandName="ShowPriorAuthDetails"
                                Text='<%# Eval("PatientTrackingNumber") %>'
                                CssClass="gridLink" OnClick="lnkPriorAuthView2_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                    <asp:BoundField DataField="FirstMName" HeaderText="First Name, MI" />
                    <asp:BoundField DataField="StatusCode" HeaderText="Status" />
                    <asp:BoundField DataField="ICDProcedureCode" HeaderText="ICD Procedure Code" />
                    <asp:BoundField DataField="ProcedureCode" HeaderText="Procedure Code" />
                    <asp:BoundField DataField="DiagnosisCode" HeaderText="Diagnosis Code" />
                    <asp:BoundField DataField="RevenueCode" HeaderText="Revenue Code" />
                    <asp:BoundField DataField="AuthSubmissionDate" HeaderText="PA Effective Date" />
                    <asp:BoundField DataField="AuthorizationEndDate" HeaderText="PA Expiration Date" />
                    <asp:BoundField DataField="AssignmentType" HeaderText="Assignment Type" />
                    <asp:BoundField DataField="OrderingProviderID" HeaderText="Ordering Provider NPI" />
                    <asp:TemplateField ShowHeader="True" HeaderText="Attachment">
                        <ItemTemplate>
                            <asp:LinkButton
                                ID="lnkPriorAuthAttachment"
                                runat="server"
                                CausesValidation="false"
                                CommandArgument='<%# Eval("PriorAuthorizationID")+","+ Eval("MemberID") %>'
                                CommandName="UploadPriorAuthAttachment"
                                Text="Upload"
                                Visible='<%# Eval("StatusCode").ToString().ToLower() == "pending additional info"  || Eval("StatusCode").ToString().ToLower() == "pending addtl info" ? true: false %>'
                                CssClass="gridLink" OnClick="lnkPriorAuthAttachment_Click" OnClientClick="setClientCookie()" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Left" />
                <HeaderStyle BackColor="#cde1ec" BorderColor="Black" ForeColor="black" CssClass="style3" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
        <div class="divGrid">
            <asp:GridView ID="gvPasearchTracking" runat="server" Style="width: 145%; border-collapse: collapse; margin-right: -539px;" AllowSorting="false"
                ShowHeaderWhenEmpty="true" EmptyDataText="No search results found" AutoGenerateColumns="false" HorizontalAlign="Left">
                <Columns>
                    <asp:BoundField DataField="PA_NUMBER" HeaderText="PA Number" />
                    <asp:BoundField DataField="Billing_Number" HeaderText="Medicaid Billing Number" />
                    <asp:TemplateField ShowHeader="True" HeaderText="Patient Tracking Number">
                        <ItemTemplate>
                            <asp:LinkButton
                                ID="lnkPriortrackingnumberId"
                                runat="server" Visible="true"
                                CausesValidation="false"
                                CommandArgument='<%# Eval("Medicaid_ID") + ","  + Eval("TRACKING_NUMBER") %>'
                                CommandName="Tracking"
                                Text='<%# Eval("TRACKING_NUMBER") %>'
                                CssClass="gridLink" OnClick="lnkTrackingNumberView_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="LAST_NAME" HeaderText="Last Name" />
                    <asp:BoundField DataField="FIRST_NAME" HeaderText="FirstName, MI" />
                    <asp:BoundField DataField="STATUS_TYPE" HeaderText="Status" />
                    <asp:BoundField DataField="ICD_ProcedureCode" HeaderText="ICD Procedure Code" />
                    <asp:BoundField DataField="ProcedureCode" HeaderText="Procedure Code" />
                    <asp:BoundField DataField="DiagnosisCode" HeaderText="Diagnosis Code" />
                    <asp:BoundField DataField="RevenueCode" HeaderText="Revenue Code" />
                    <asp:BoundField DataField="PA_EffectiveDate" HeaderText="PA Effective Date" />
                    <asp:BoundField DataField="PA_Expiration_Date" HeaderText="PA Expiration Date" />
                    <asp:BoundField DataField="PRIOR_AUTH_Assignment_Type_DESC" HeaderText="Assignment Type" />
                    <asp:BoundField DataField="Ordering_ProviderNPI" HeaderText="Ordering Provider NPI" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle BackColor="#cde1ec" BorderColor="Black" ForeColor="black" CssClass="style3" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:Panel>
    <asp:HiddenField ID="hdnNPI" runat="server" />
</div>
<cc2:messagebox id="MessageBox2" runat="server" />
