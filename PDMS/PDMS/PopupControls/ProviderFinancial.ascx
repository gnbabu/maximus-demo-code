<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ProviderFinancial" Codebehind="ProviderFinancial.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<style type="text/css">
    .formDropdownPF {
        border: 1px solid #ccc;
        background-color: #FFFFFF;
        color: #000;
        min-width: 130px;
        font-family: "Arial Narrow";
        font-size: 14pt !important;
        vertical-align: top;
        height: 40px;
    }

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
        background: linear-gradient(180deg,#ACCEFF 0%,#2E80FD 47.91%,#6BA5FF 97.92%,#3974CF 100% );
        border: 1px solid transparent;
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

    .myPrintButton {
        box-shadow: inset 0px 1px 0px 0px #9acc85;
        background: linear-gradient(to bottom, #438fff 5%, #438fff 100%);
        background-color: #438fff;
        border: 1px solid #438fff;
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
<script>
    function btnSearch_Click() {
        var selectedOption = document.getElementById("lblActivityType").value;
        if (selectedOption === "") {
            document.getElementById("errorMessage").style.display = "block";
            return false;
        }
    }

    function CollapseExpandFinancialSearch() {
        var button = document.getElementById("financialSearch").getAttribute("aria-expanded");
        if (button == "true") {
            button = "false"
            $("financialSearch").addClass("panelHeaderStyle");
        } else {
            button = "true"
            $("financialSearch").addClass("panelHeaderStyle");
        }
        document.getElementById("financialSearch").setAttribute("aria-expanded", button);
    };

</script>

<div class="WhiteBox">
    <h1>
        <uc1:secthd runat="server" id="sepInstructions" />
    </h1>
    <br />
    <ajax:collapsiblepanelextender id="cpeFinancialSearch" runat="server" collapsed="false" targetcontrolid="pnlFSSearch"
        expandcontrolid="pnlFSCPESearch" collapsecontrolid="pnlFSCPESearch"
        expandedtext="-" collapsedsize="0" scrollcontents="true" collapsedtext="+" expanddirection="Vertical"
        suppresspostback="true" textlabelid="lblsepContact" />
    <asp:Label runat="server" Style="color: #D33421; font-size: 14pt !important; font-weight: 100 !important" ID="PASearchHelpTextID" />
    <asp:Panel runat="server" ID="pnlFSCPESearch" class="CollapsingSeparator"
        ToolTip="Click to Expand/Collapse" CssClass="OwnerAuthSearch CollapsingSeparator">

        <div class="pageHeader pH2">
            <table width="100%;" class="collaps-table">
                <tr>
                    <td align="left" style="color: white">
                        <h1><span style="color: white; font-weight: bold;">
                            <button type="button" class="panelHeaderStyle" tabindex="0" id="financialSearch" aria-expanded="true" onclick="CollapseExpandFinancialSearch()">Financial Information</button></span></h1>
                        <asp:Label runat="server" ID="lblsepContact1" />
                    </td>
                    <td style="padding-right: 10px; margin: 30px; width: 35px">
                        <asp:Label runat="server" ID="lblsepContact" Style="color: white" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlFSSearch" runat="server" CssClass="pnl-seach" ScrollBars="Auto">
        <br />
        <asp:Panel runat="server" ID="pnlInstructions">
            <span id="errorMessage" role="alert" aria-live="assertive">
                <asp:Label ID="lblErrPF" runat="server" Visible="false" CssClass="failureNotification" />
            </span>
            <div class="col-sm-12">
                <div class="row">

                    <div class="col-sm-3 text-right">
                        <span style="font-size: 18px; text-align: right; font-weight: bold;"><span style="color: #e50000"></span>Activity Type : </span>
                    </div>
                    <div class="col-sm-3 text-left">
                        <asp:DropDownList ID="ddlActivityType" Style="max-width: 40px !important" aria-label="Activity Type" runat="server" CssClass="formDropdownPF wd300" OnSelectedIndexChanged="ddlActivityType_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-3 text-right">
                        <span style="font-size: 18px; text-align: right; font-weight: bold;"><span style="color: #e50000"></span>Year : </span>
                    </div>
                    <div class="col-sm-3 text-left">
                        <asp:DropDownList ID="ddlYear" Style="max-width: 40px !important" aria-label="Activity Type" runat="server" CssClass="formDropdownPF" Enabled="true">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="btnBox btnBoxCenter">
                <asp:Button runat="server" ID="btnSearch" Text="Search" class="buttonBoxFocusBlue" OnClick="btnSearch_Click" CausesValidation="false" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonBoxFocusred" CausesValidation="False" />
            </div>
            <br />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlTransHist" HorizontalAlign="Center" Visible="false">
            <asp:Panel ID="pnlHeaderTrans" Style="cursor: move; padding: 5px; margin-top: 100px;" CssClass="CollapsingSeparator" runat="server" HorizontalAlign="Left">
                <div style="text-align: left">
                    <div class="pageHeader pH2">
                        <table width="100%;" class="collaps-table">
                            <tr>
                                <td style="color: white;">
                                    <h1><span style="color: white; font-weight: bold;">Claim Activity Summary</span></h1>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </asp:Panel>
            <br />
            <asp:Label ID="lblTransHistErr" runat="server" Visible="false" CssClass="failureNotification" />
            <asp:Panel ID="pnlTransHistDetial" runat="server">
                <div style="text-align: center; margin-right: 10%;" class="tablepad">
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Number of Claims Paid in Current Month </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:Label ID="lbl1" runat="server" CssClass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Amount Paid in Current Month </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:Label ID="lbl2" runat="server" CssClass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Number of Claims Denied in Current Month </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:Label ID="lbl3" runat="server" CssClass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Number of Claims Paid in Past 12 Months </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:Label ID="lbl4" runat="server" CssClass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Amount Paid in Past 12 Months </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:Label ID="lbl5" runat="server" CssClass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Number of Claims Denied in Past 12 Months </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:Label ID="lbl6" runat="server" CssClass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Number of Suspended Claims </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:Label ID="lbl7" runat="server" CssClass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Number of Claims in Final Disposition </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:Label ID="lbl8" runat="server" CssClass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Date of Most Recent Payment </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:Label ID="lbl9" runat="server" CssClass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Type of Most Recent Payment </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:Label ID="lbl10" runat="server" CssClass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Amount of Most Recent Payment </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:Label ID="lbl11" runat="server" CssClass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Total Credit Balance Amount </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:Label ID="lbl12" runat="server" CssClass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Amount Applied Toward Credit Balance </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:Label ID="lbl13" runat="server" CssClass="formLabel" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnl1099" Width="100%" HorizontalAlign="Center" Visible="false">
            <asp:Panel ID="Panel1" Style="cursor: move; padding: 5px;" runat="server" HorizontalAlign="Left">
                <div style="text-align: left">
                    &nbsp;&nbsp;
                <asp:Label ID="Label1" CssClass="bodyTextBold" Style="font-weight: bold" runat="server" Text="1099 Search Result" ForeColor="Black" />
                </div>
            </asp:Panel>
            <br />
            <asp:Label ID="lbl1099Err" runat="server" Visible="false" CssClass="failureNotification" />
            <asp:Panel ID="pnl1099dtl" runat="server">
                <asp:GridView runat="server" ID="grd1099" AutoGenerateColumns="false" ShowHeader="true" Visible="true" Width="100%"
                    EmptyDataText="No 1099 result received." ShowHeaderWhenEmpty="true" CellPadding="5" ShowFooter="true" AllowPaging="true"
                    OnRowDataBound="grd1099_RowDataBound" OnPageIndexChanging="grd1099_PageIndexChanging" OnRowCommand="grd1099_RowCommand" PageSize="10" PagerSettings-Mode="NumericFirstLast">
                    <Columns>
                        <asp:BoundField DataField="TaxId" HeaderText="Tax ID" FooterText="Page Totals: " />
                        <asp:TemplateField HeaderText="Issued Date">
                            <ItemTemplate>
                                <asp:Literal runat="server" Text='<%# Eval("IssuedDate") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="System Earning">
                            <ItemTemplate>
                                $<%# Eval("SystemEarningsAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Manual Earning">
                            <ItemTemplate>
                                $<%# Eval("ManualEarningsAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Claim Refunds">
                            <ItemTemplate>
                                $<%# Eval("ClaimRefundAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Non-Claim Refunds">
                            <ItemTemplate>
                                $<%# Eval("NonClaimRefundsAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Void Amount">
                            <ItemTemplate>
                                $<%# Eval("VoidAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FICA Amount">
                            <ItemTemplate>
                                $<%# Eval("FicaAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Backup Withholding Amount">
                            <ItemTemplate>
                                $<%# Eval("BackupWithholdingAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Net Earning">
                            <ItemTemplate>
                                $<%# Eval("NetEarningsAmount")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="AdjustReason" HeaderText="Adjust Reason" />
                        <asp:BoundField DataField="Payer_Name" HeaderText="Payer Name" />
                        <asp:TemplateField HeaderText="Print">
                            <ItemTemplate>
                                <asp:LinkButton
                                    ID="lnkPrint1099"
                                    runat="Server"
                                    CommandArgument='<%#
                                        Eval("TaxId")
                                        + ";" + Eval("NetEarningsAmount")
                                        + ";" + Eval("FicaAmount") 
                                        + ";" + Eval("BackupWithholdingAmount")
                                        + ";" + Eval("Payer_Name")
                                        + ";" + Eval("Payer_Address1")
                                        + ";" + Eval("Payer_Address2")
                                        + ";" + Eval("Payer_City")
                                        + ";" + Eval("Payer_State")
                                        + ";" + Eval("Payer_Zip") %>'
                                    CausesValidation="false"
                                    CommandName="Print"
                                    Text="Print"
                                    CssClass="myPrintButton"
                                    Style="color: white; font-weight: bold;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle BackColor="#cde1ec" BorderColor="Black" ForeColor="black" CssClass="style3" Width="100px" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
</div>
