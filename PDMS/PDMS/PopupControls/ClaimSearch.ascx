<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ClaimSearch" Codebehind="ClaimSearch.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<link href="../Content/custom-style.css" rel="stylesheet" />
<style>
    .gridViewHeader a, .gridViewHeader a:link, .gridViewHeader a:active, .gridViewHeader a:hover, .gridViewHeader a:visited,
    .gridViewHeader th a, .gridViewHeader th a:link, .gridViewHeader th a:active, .gridViewHeader th a:hover, .gridViewHeader th a:visited,
    .rgHeader a, .rgHeader a:link, .rgHeader a:active, .rgHeader a:hover, .rgHeader a:visited,
    .rgHeader th a, .rgHeader th a:link, .rgHeader th a:active, .rgHeader th a:hover, .rgHeader th a:visited {
        color: #222222 !important;
        text-align: left;
    }
</style>
<script type="text/javascript">
    function getCookie(cname) {
        let name = cname + "=";
        let ca = document.cookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }

    $(document).ready(function () {
        localStorage.setItem("diagTable", "");
        localStorage.setItem("dentalServiceDetailTable", "");
        localStorage.setItem("OccSpanTable", "");
        localStorage.setItem("ICDProcCodeTable", "");
        localStorage.setItem("NDCDetailsTable", "");
        localStorage.setItem("instiServiceDetailTable", "");
        localStorage.setItem("ToothQuadrantInfoTable", "");
        localStorage.setItem("OccurrenceInfoTable", "");
        localStorage.setItem("OtherPayerAdjustmentTable", "");
        localStorage.setItem("providerNotesTable", "");
        localStorage.setItem("providerBillingNotesTable", "");
        localStorage.setItem("otherPayerInfoTableDental", "");
        localStorage.setItem("OPPAServiceDetailTable", "");
        if (getCookie('showResults') == 'false') {
            if (document.getElementById('ctl00_MainContent_uc6ClaimSearch_pnlClaimSearchResult') != null) {
                document.getElementById('ctl00_MainContent_uc6ClaimSearch_pnlClaimSearchResult').innerHTML = '<div></div>';
            }
            if (document.getElementById('ctl00_MainContent_uc6ClaimSearch_txtICNTCN') != null) {
                document.getElementById('ctl00_MainContent_uc6ClaimSearch_txtICNTCN').value = '';
            }
            if (document.getElementById('ctl00_MainContent_uc6ClaimSearch_txtMedicaidBillingNumber') != null) {
                document.getElementById('ctl00_MainContent_uc6ClaimSearch_txtMedicaidBillingNumber').value = '';
            }
            if (document.getElementById('ctl00_MainContent_uc6ClaimSearch_txtPatAccountNumber') != null) {
                document.getElementById('ctl00_MainContent_uc6ClaimSearch_txtPatAccountNumber').value = '';
            }
            if (document.getElementById('ctl00_MainContent_uc6ClaimSearch_txtRenderingProviderId') != null) {
                document.getElementById('ctl00_MainContent_uc6ClaimSearch_txtRenderingProviderId').value = '';
            }
            if (document.getElementById('ctl00_MainContent_uc6ClaimSearch_txtAmountBilled') != null) {
                document.getElementById('ctl00_MainContent_uc6ClaimSearch_txtAmountBilled').value = '';
            }
            if (document.getElementById('ctl00_MainContent_uc6ClaimSearch_txtPrescriptionNumber') != null) {
                document.getElementById('ctl00_MainContent_uc6ClaimSearch_txtPrescriptionNumber').value = '';
            }
            if (document.getElementById('ctl00_MainContent_uc6ClaimSearch_txtRaDate') != null) {
                document.getElementById('ctl00_MainContent_uc6ClaimSearch_txtRaDate').value = '';
            }
            if (document.getElementById('ctl00_MainContent_uc6ClaimSearch_ddlMangedCarePlan') != null) {
                document.getElementById('ctl00_MainContent_uc6ClaimSearch_ddlMangedCarePlan').selectedIndex = 0;
            }
            if (document.getElementById('ctl00_MainContent_uc6ClaimSearch_ddlClaimType') != null) {
                document.getElementById('ctl00_MainContent_uc6ClaimSearch_ddlClaimType').selectedIndex = 0;
            }
            if (document.getElementById('ctl00_MainContent_uc6ClaimSearch_ddlClaimStatus') != null) {
                document.getElementById('ctl00_MainContent_uc6ClaimSearch_ddlClaimStatus').selectedIndex = 0;
            }
        }
        const d = new Date();
        d.setTime(d.getTime() + (1 * 24 * 60 * 60 * 1000));
        let expires = "expires=" + d.toUTCString();
        document.cookie = 'showResults=false;' + expires + ";path=/";

    });

    function CollapseExpand(obj) {
        var sp = obj.getElementsByTagName('span')[0];
        var spText = sp.innerHTML;
        var plusIndex = spText.indexOf("+");

        if (plusIndex == 0)
            sp.innerHTML = spText.replace("+", "-");
        else
            sp.innerHTML = spText.replace("-", "+");
    }

    function isNumberKey(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }
    function btnSearch_Click() {
        var isDatabaseSearch = document.getElementById("ctl00_MainContent_uc6ClaimSearch_ddlClaimStatus").value;
        if (isDatabaseSearch != "1") {
            var selectedOption = document.getElementById("ctl00_MainContent_uc6ClaimSearch_ddlMangedCarePlan").value;
            if (selectedOption == "") {
                document.getElementById("errorMessageclaimsearch").innerHTML = '<div class="failureNotification">* Payor Name is Required</div>';
                document.getElementById("errorMessageclaimsearch").style.display = "block";
                return false; // Prevent form submission
            }
            else {
                document.getElementById("errorMessageclaimsearch").innerHTML = '<div></div>';
                document.getElementById("errorMessageclaimsearch").style.display = "none";
            }
        } else {
            document.getElementById("errorMessageclaimsearch").innerHTML = '<div></div>';
            document.getElementById("errorMessageclaimsearch").style.display = "none";
        }
        const d = new Date();
        d.setTime(d.getTime() + (1 * 24 * 60 * 60 * 1000));
        let expires = "expires=" + d.toUTCString();
        document.cookie = 'showResults=true;';
    }
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
    }

    @media only screen and (max-width: 1200px) {
        #fakeDiv1, #fakeDiv2 {
            display: none;
        }
    }

    .container {
        width: 605px;
    }

    .cssPager td {
        padding-left: 4px;
        padding-right: 4px;
    }

    .GroupBoxHeader {
        background: #7993ac !important;
        padding: 8px !important;
        margin-bottom: 4px !important
    }
</style>

<asp:Label ID="lblGeneralErr" runat="server" Visible="false" CssClass="failureNotification" />
<span id="errorMessageclaimsearch" role="alert" aria-live="assertive">
    <asp:ValidationSummary ID="valerrormess" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="ClaimSearch" />
</span>
<asp:HiddenField ID="hdnTotalCount" runat="server" />
<div class="row" style="border: groove; margin-left: 10px">
    <div class="row">
        <div class="col-sm-9">
            <asp:Label runat="server" Style="color: #D33421; font-size: 14pt !important; font-weight: bold" ID="SearchClaimHelpTxt" />
        </div>
    </div>
    <cc1:GroupBox ID="gbClaimSearch" Caption="CLAIM SEARCH" CaptionStyle-CssClass="GroupBoxHeader bodyTextBold" HorizontalAlign="Center" runat="server" CaptionStyle-BackColor="#0099cc" CaptionStyle-ForeColor="White" CaptionStyle-Font-Bold="true">
        <asp:Panel ID="pnlClaimSearch" runat="server" Style="min-width: 300px; width: auto; max-width: 1200px; margin-top: 0px;">
            <div class="table">
                <div class="col-sm-12">
                    <div class="row">

                        <div class="col-sm-3">
                            <span class="ohio-field" style="font-size: 15px; text-align: right; font-weight: bold">ICN</span>
                        </div>
                        <div class="col-sm-3">
                            <span style="text-align: left;">

                                <asp:TextBox ID="txtICNTCN" aria-label="ICN" runat="server" CssClass="formField" ValidationGroup="valOwnerInfo" MaxLength="50" Style="height: 30px; width: 200px" />
                            </span>
                        </div>
                        <div class="col-sm-3">
                            <span class="ohio-field" style="font-size: 15px; text-align: right; font-weight: bold">Claim Type</span>
                        </div>
                        <div class="col-sm-3">
                            <span style="text-align: left;">
                                <asp:DropDownList ID="ddlClaimType" aria-label="Claim Type" EnableViewState="true" CssClass="formField" AutoPostBack="false" runat="server" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlClaimType_SelectedIndexChanged" Style="min-width: 200px; height: 30px">
                                    <asp:ListItem Selected="False"></asp:ListItem>
                                    <asp:ListItem Selected="True" Text="Dental" Value="0"></asp:ListItem>
                                    <asp:ListItem Selected="False" Text="Institutional" Value="1"></asp:ListItem>
                                    <asp:ListItem Selected="False" Text="Professional" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </span>
                        </div>

                    </div>
                </div>
                <div class="col-sm-12">
                    <div class="row">

                        <div class="col-sm-3">
                            <span class="ohio-field" style="font-size: 15px; text-align: right; font-weight: bold">Medicaid Billing Number</span>
                        </div>
                        <div class="col-sm-3">
                            <span style="text-align: left;">
                                <asp:TextBox ID="txtMedicaidBillingNumber" aria-label="MedicaidBillingNumber" runat="server" CssClass="formField" ValidationGroup="valOwnerInfo" Style="height: 30px; width: 200px" MaxLength="12" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorBillingMedicaid" runat="server" ControlToValidate="txtMedicaidBillingNumber" ValidationExpression="^[0-9]{12}$"
                                    ErrorMessage="* 12-digit number is required." Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="ClaimSearch" Display="Dynamic" ForeColor="Red" />
                            </span>
                        </div>
                        <div class="col-sm-3">
                            <span class="ohio-field" style="font-size: 15px; text-align: right; font-weight: bold">Claim Status</span>
                        </div>
                        <div class="col-sm-3">
                            <span style="text-align: left;">
                                <asp:DropDownList ID="ddlClaimStatus" aria-label="Claim Status" CssClass="formField" EnableViewState="true" AutoPostBack="false" runat="server" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlClaimStatus_SelectedIndexChanged" Style="min-width: 200px; height: 30px"></asp:DropDownList>
                            </span>
                        </div>

                    </div>
                </div>

                <div class="col-sm-12">
                    <div class="row">
                        <div class="col-sm-3">
                            <span class="ohio-field" style="font-size: 15px; text-align: right; font-weight: bold">Patient Account Number</span>
                        </div>
                        <div class="col-sm-3">
                            <span style="text-align: left;">
                                <asp:TextBox ID="txtPatAccountNumber" aria-label="PatAccountNumber" runat="server" CssClass="formField" Style="height: 30px; width: 200px" MaxLength="38" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorPA" runat="server" ControlToValidate="txtPatAccountNumber" ValidationExpression="^[0-9A-Za-z]*$"
                                    ErrorMessage="* Please enter valid patient account number." Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="ClaimSearch" Display="Dynamic" ForeColor="Red" />
                            </span>
                        </div>
                        <div class="col-sm-3">
                            <span class="ohio-field" style="font-size: 15px; text-align: right; font-weight: bold;">RA Date</span>
                        </div>
                        <div class="col-sm-3">
                            <span style="text-align: left;">
                                <asp:TextBox ID="txtRaDate" aria-label="RA Date" runat="server" CssClass="formField" Style="height: 30px; min-width: 200px;" />
                                <ajax:CalendarExtender ID="ceRaDate" runat="server" TargetControlID="txtRaDate" />
                            </span>
                        </div>
                    </div>
                </div>
                <div class="col-sm-12">
                    <div class="row">
                        <div class="col-sm-3">
                            <span class="ohio-field" style="font-size: 15px; text-align: right; font-weight: bold">Rendering Provider ID</span>
                        </div>
                        <div class="col-sm-3">
                            <span style="text-align: left;">
                                <asp:TextBox ID="txtRenderingProviderId" aria-label="Rendering Provider ID" runat="server" CssClass="formField" ValidationGroup="valOwnerInfo" Style="height: 30px; width: 200px" MaxLength="10" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorpid" runat="server" ControlToValidate="txtRenderingProviderId" ValidationExpression="^[0-9]{10}$"
                                    ErrorMessage="* Please enter valid Rendering Provider ID." Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="ClaimSearch" Display="Dynamic" ForeColor="Red" />
                            </span>
                        </div>
                        <div class="col-sm-3">
                            <span class="ohio-field" style="font-size: 15px; text-align: right; font-weight: bold">Date of Service From</span>
                        </div>
                        <div class="col-sm-3">
                            <div class="row">
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtDateofServfrom" aria-label="Date of Service From" runat="server" CssClass="formField" Style="height: 30px; width: 90px" />
                                    <ajax:CalendarExtender ID="ceDateofServfrom" runat="server" TargetControlID="txtDateofServfrom" />
                                </div>
                                <div class="col-sm-2">
                                    <span style="font-size: 15px; text-align: right; font-weight: bold">To</span>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtDateofServto" aria-label="To" runat="server" CssClass="formField" Style="height: 30px; width: 90px;" />
                                    <ajax:CalendarExtender ID="ceDateofServTo" TargetControlID="txtDateofServTo" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-12">
                    <div class="row">
                        <div class="col-sm-3">
                            <span class="ohio-field" style="font-size: 15px; text-align: right; font-weight: bold">Amount Billed</span>
                        </div>
                        <div class="col-sm-3">
                            <span style="text-align: left;">
                                <asp:TextBox ID="txtAmountBilled" aria-label="Amount Billed" runat="server" CssClass="formField" onKeyPress="isNumberKey();" ValidationGroup="valOwnerInfo" Style="height: 30px; width: 200px" MaxLength="18" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtAmountBilled"
                                    ValidationExpression="^[+-]?[0-9]{1,3}(?:,?[0-9]{3})*(?:\.[0-9]{2})?$" ErrorMessage="*Enter Total Paid Amount TO" Text="*" Display="Dynamic"
                                    ValidationGroup="valTotPaidAmountTo" ForeColor="Red" />
                            </span>
                        </div>
                        <div class="col-sm-3">
                            <span class="ohio-field" style="font-size: 15px; text-align: right">Max Records</span>
                        </div>
                        <div class="col-sm-3">
                            <span style="text-align: left; width: 350px">
                                <asp:DropDownList ID="ddlPageSize" aria-label="Max Records" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageSize_Changed" Style="min-width: 80px; height: 30px">
                                    <asp:ListItem Text="5" Value="5" />
                                    <asp:ListItem Text="10" Value="10" />
                                    <asp:ListItem Text="20" Value="20" />
                                    <asp:ListItem Text="30" Value="30" />
                                    <asp:ListItem Text="40" Value="40" />
                                    <asp:ListItem Text="50" Value="50" />
                                </asp:DropDownList>
                            </span>
                        </div>
                    </div>
                </div>

                <div class="col-sm-12">
                    <div class="row">
                        <div class="col-sm-3">
                            <span class="ohio-field" style="font-size: 15px; text-align: right; font-weight: bold">Prescription Number</span>
                        </div>
                        <div class="col-sm-3">
                            <span style="text-align: left;">
                                <asp:TextBox ID="txtPrescriptionNumber" aria-label="Prescription Number" runat="server" CssClass="formField" ValidationGroup="valOwnerInfo" Style="height: 30px; width: 200px" MaxLength="12" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtPrescriptionNumber" ValidationExpression="^[a-zA-Z0-9]+$"
                                    ErrorMessage="* Please enter valid Prescription Number." Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="ClaimSearch" Display="Dynamic" ForeColor="Red" />
                            </span>
                        </div>
                        <div class="col-sm-3"></div>
                        <div class="col-sm-3" style="margin-left: 40px;">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="ClaimSearch"
                                ToolTip="Search data" CausesValidation="true" Width="90px" OnClientClick="return btnSearch_Click();" />

                            <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBoxFocusred" OnClick="btnClear_Click" Width="90px" />

                        </div>
                    </div>
                </div>

                <div class="col-sm-12">
                    <div class="row">
                        <div class="col-sm-3">
                            <span class="ohio-field" style="font-size: 15px; text-align: right; font-weight: bold">Payor Name *</span>
                        </div>
                        <div class="col-sm-3">
                            <span style="text-align: left;">
                                <asp:DropDownList ID="ddlMangedCarePlan" aria-label="Payor Name" EnableViewState="true" ValidationGroup="valMangedCarePlan" CssClass="formField" AutoPostBack="false" runat="server" AppendDataBoundItems="True" OnSelectedIndexChanged="ddlMangedCarePlan_SelectedIndexChanged" Style="height: 30px; width: 200px; min-width: 80px;">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="rvMangedCarePlan" SetFocusOnError="true"
                                    ValidationGroup="valMangedCarePlan" ControlToValidate="ddlMangedCarePlan" Text="Payor name is required"
                                    Display="Dynamic" CssClass="failureNotification" />
                            </span>
                        </div>
                    </div>
                </div>
                <%--<div class="col-sm-12">
                    <div class="row" id="PageSize" runat="server">
                        <div class="col-sm-7">
                            <span class="ohio-field" style="font-size: 15px; text-align: right">Max Records</span>
                        </div>
                        <div class="col-sm-5">
                            <span style="text-align: left;">
                                <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="PageSize_Changed" Style="min-width: 100px; height: 30px">
                                    <asp:ListItem Text="5" Value="5" />
                                    <asp:ListItem Text="10" Value="10" />
                                    <asp:ListItem Text="20" Value="20" />
                                    <asp:ListItem Text="30" Value="30" />
                                    <asp:ListItem Text="40" Value="40" />
                                    <asp:ListItem Text="50" Value="50" />
                                </asp:DropDownList>
                            </span>
                        </div>
                    </div>
                </div>--%>
            </div>
        </asp:Panel>
    </cc1:GroupBox>

    <cc1:GroupBox ID="gbSearchResult" Caption="CLAIM SEARCH RESULT" CaptionStyle-CssClass="GroupBoxHeader bodyTextBold" HorizontalAlign="Center" runat="server" CaptionStyle-ForeColor="White" CaptionStyle-BackColor="#0099cc" Font-Bold="true">
        <asp:Panel ID="pnlClaimSearchResult" runat="server" Style="min-height: 340px; min-width: 300px; width: auto; max-width: 100%;">
            <div class="ClaimSearchResult" style="padding-top: 1px; padding-left: 1px; padding-right: 1px; text-align: center">
                <%--<mms:SortablePagingGridView--%>
                <asp:GridView
                    ID="gvClaimSearchResult"
                    runat="server"
                    AllowPaging="True"
                    AllowCustomPaging="true"
                    AutoGenerateColumns="False"
                    CssClass="gridViewSmallFont" Width="100%"
                    AllowSorting="true"
                    OnSorting="gvClaimSearchResult_Sorting"
                    CurrentSortField="ICN"
                    CurrentSortDirection="DESC"
                    EmptyDataText="No results found."
                    RowStyle-VerticalAlign="Top"
                    OnRowDataBound="gvClaimSearchResult_RowDataBound"
                    OnRowCreated="gvClaimSearchResult_RowCreated"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" EditRowStyle-HorizontalAlign="Center" OnPageIndexChanging="gvClaimSearchResult_PageIndexChanging"
                    GridViewSortColumn="ICN,MemberId" GridViewSortDirection="Ascending" DataKeyNames="ICN,MemberId" OnRowCommand="grdClaimSearchResult_RowCommand">
                    <RowStyle HorizontalAlign="Center"></RowStyle>
                    <HeaderStyle HorizontalAlign="Center" />
                    <PagerStyle HorizontalAlign="Center" CssClass="cssPager" />
                    <Columns>
                        <asp:TemplateField ShowHeader="True" HeaderText="ICN" SortExpression="ICN">
                            <ItemTemplate>
                                <asp:LinkButton
                                    ID="lnkICN"
                                    runat="server"
                                    CausesValidation="false"
                                    CommandArgument='<%# Eval("ICN")+","+ Eval("MemberId") + ","  + Eval("ClaimType") %>'
                                    CommandName="ShowClaimDetails"
                                    Text='<%# Eval("ICN") %>'
                                    CssClass="gridLink" OnClick="lnkView_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="MemberId" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Medicaid Billing Number" />

                        <asp:TemplateField ShowHeader="True" HeaderText="Patient Account Number">
                            <ItemTemplate>
                                <asp:LinkButton
                                    ID="lnkPatientAccountNumber"
                                    runat="server"
                                    CausesValidation="false"
                                    CommandArgument='<%# Eval("ICN")+","+ Eval("MemberId") + ","  + Eval("ClaimType") + ","  + Eval("PayorType")+","+Eval("PatientAccountNumber") %>'
                                    CommandName="RedirectClaim"
                                    Text='<%# Eval("PatientAccountNumber") %>'
                                    CssClass="gridLink" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="TotalCharges" HeaderText="Billed Amount" />
                        <asp:BoundField DataField="TotalPaidAmount" HeaderText="Paid Amount" />
                        <asp:BoundField DataField="ClaimType" HeaderText="Claim Type" />
                        <asp:BoundField DataField="RemittanceAdviceDate" HeaderText="RA Date" />
                        <asp:BoundField DataField="FromDOS" HeaderText="From Date" />
                        <asp:BoundField DataField="ThruDOS" HeaderText="To Date" />
                        <asp:BoundField DataField="ClaimStatus" HeaderText="Status" />
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
    </cc1:GroupBox>
</div>