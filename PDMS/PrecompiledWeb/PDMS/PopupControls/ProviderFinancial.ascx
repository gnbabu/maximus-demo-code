<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ProviderFinancial, App_Web_yvhxe4ml" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>
<%@ register src="~/PopupControls/Separator.ascx" tagname="SectHd" tagprefix="uc1" %>
<%@ register assembly="eWorld.UI" namespace="eWorld.UI" tagprefix="ew" %>

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
    <asp:label runat="server" style="color: #D33421; font-size: 14pt !important; font-weight: 100 !important" id="PASearchHelpTextID" />
    <asp:panel runat="server" id="pnlFSCPESearch" class="CollapsingSeparator" style="background-color: #2197bb;"
        tooltip="Click to Expand/Collapse" cssclass="OwnerAuthSearch">

        <div class="pageHeader pH2">
            <table width="100%">
                <tr>
                    <td align="left" style="color: white">
                        <h1><span style="color: white; font-weight: bold;">
                            <button type="button" class="panelHeaderStyle" tabindex="0" id="financialSearch" aria-expanded="true" onclick="CollapseExpandFinancialSearch()">FINANCIAL INFORMATION</button></span></h1>
                        <asp:label runat="server" id="lblsepContact1" />
                    </td>
                    <td style="padding-right: 10px; margin: 30px; width: 35px">
                        <asp:label runat="server" id="lblsepContact" style="color: white" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:panel>

    <asp:panel id="pnlFSSearch" runat="server" cssclass="pnl-seach" scrollbars="Auto">
        <br />
        <asp:panel runat="server" id="pnlInstructions">
            <span id="errorMessage" role="alert" aria-live="assertive">
                <asp:label id="lblErrPF" runat="server" visible="false" cssclass="failureNotification" />
            </span>
            <div class="row">
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <asp:label id="lblActivityType" text="Activity Type" runat="server" class="formLabelSmall" />
                    </div>
                    <div class="col-sm-2 text-left">
                        <asp:dropdownlist id="ddlActivityType" style="max-width: 40px !important" aria-label="Activity Type" runat="server" cssclass="formDropdownPF wd300" onselectedindexchanged="ddlActivityType_SelectedIndexChanged" autopostback="true">
                        </asp:dropdownlist>
                    </div>
                    <div class="col-sm-2 text-right">
                        <asp:label id="lblYear" runat="server" text="Year" cssclass="formLabelSmall" />
                    </div>
                    <div class="col-sm-2 text-left">
                        <asp:dropdownlist id="ddlYear" style="max-width: 40px !important" aria-label="Activity Type" runat="server" cssclass="formDropdownPF" enabled="true">
                        </asp:dropdownlist>
                    </div>
                    <div class="col-sm-2">
                        <asp:button runat="server" id="btnSearch" text="Search" class="mySearchButton" onclick="btnSearch_Click" causesvalidation="false" />
                    </div>
                </div>
            </div>
            <br />
        </asp:panel>
        <asp:panel runat="server" id="pnlTransHist" horizontalalign="Center" visible="false">
            <asp:panel id="pnlHeaderTrans" style="cursor: move; padding: 5px;" backcolor="#95c2e1" runat="server" horizontalalign="Left">
                <div style="text-align: left">
                    &nbsp;&nbsp;
                <asp:label id="lblTitle" cssclass="bodyTextBold" runat="server" style="font-weight: bold" text="Claim Activity Summary" forecolor="Black" />
                </div>
            </asp:panel>
            <br />
            <asp:label id="lblTransHistErr" runat="server" visible="false" cssclass="failureNotification" />
            <asp:panel id="pnlTransHistDetial" runat="server">
                <div style="text-align: center; margin-right: 10%;" class="tablepad">
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Number of Claims Paid in Current Month </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:label id="lbl1" runat="server" cssclass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Amount Paid in Current Month </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:label id="lbl2" runat="server" cssclass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Number of Claims Denied in Current Month </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:label id="lbl3" runat="server" cssclass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Number of Claims Paid in Past 12 Months </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:label id="lbl4" runat="server" cssclass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Amount Paid in Past 12 Months </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:label id="lbl5" runat="server" cssclass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Number of Claims Denied in Past 12 Months </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:label id="lbl6" runat="server" cssclass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Number of Suspended Claims </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:label id="lbl7" runat="server" cssclass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Number of Claims in Final Disposition </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:label id="lbl8" runat="server" cssclass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Date of Most Recent Payment </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:label id="lbl9" runat="server" cssclass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Type of Most Recent Payment </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:label id="lbl10" runat="server" cssclass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Amount of Most Recent Payment </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:label id="lbl11" runat="server" cssclass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Total Credit Balance Amount </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:label id="lbl12" runat="server" cssclass="formLabel" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6 text-right">
                            <span class="formLabel">Amount Applied Toward Credit Balance </span>
                        </div>
                        <div class="col-sm-4 text-left">
                            <asp:label id="lbl13" runat="server" cssclass="formLabel" />
                        </div>
                    </div>
                </div>
            </asp:panel>
        </asp:panel>
        <asp:panel runat="server" id="pnl1099" width="100%" horizontalalign="Center" visible="false">
            <asp:panel id="Panel1" style="cursor: move; padding: 5px;" backcolor="#95c2e1" runat="server" horizontalalign="Left">
                <div style="text-align: left">
                    &nbsp;&nbsp;
                <asp:label id="Label1" cssclass="bodyTextBold" style="font-weight: bold" runat="server" text="1099 Search Result" forecolor="Black" />
                </div>
            </asp:panel>
            <br />
            <asp:label id="lbl1099Err" runat="server" visible="false" cssclass="failureNotification" />
            <asp:panel id="pnl1099dtl" runat="server">
                <asp:gridview runat="server" id="grd1099" autogeneratecolumns="false" showheader="true" visible="true" width="100%"
    emptydatatext="No 1099 result received." showheaderwhenempty="true" cellpadding="5" showfooter="true" allowpaging="true"
    onrowdatabound="grd1099_RowDataBound" onpageindexchanging="grd1099_PageIndexChanging" OnRowCommand="grd1099_RowCommand" pagesize="10" pagersettings-mode="NumericFirstLast">
                    <columns>
                        <asp:boundfield datafield="TaxId" headertext="Tax ID" footertext="Page Totals: " />
                        <asp:templatefield headertext="Issued Date">
                            <itemtemplate>
                                <asp:literal runat="server" text='<%# Eval("IssuedDate") %>' />
                            </itemtemplate>
                        </asp:templatefield>
                        <asp:templatefield headertext="System Earning">
                            <itemtemplate>
                                $<%# Eval("SystemEarningsAmount")%>
                            </itemtemplate>
                        </asp:templatefield>
                        <asp:templatefield headertext="Manual Earning">
                            <itemtemplate>
                                $<%# Eval("ManualEarningsAmount")%>
                            </itemtemplate>
                        </asp:templatefield>
                        <asp:templatefield headertext="Claim Refunds">
                            <itemtemplate>
                                $<%# Eval("ClaimRefundAmount")%>
                            </itemtemplate>
                        </asp:templatefield>
                        <asp:templatefield headertext="Non-Claim Refunds">
                            <itemtemplate>
                                $<%# Eval("NonClaimRefundsAmount")%>
                            </itemtemplate>
                        </asp:templatefield>
                        <asp:templatefield headertext="Void Amount">
                            <itemtemplate>
                                $<%# Eval("VoidAmount")%>
                            </itemtemplate>
                        </asp:templatefield>
                        <asp:templatefield headertext="FICA Amount">
                            <itemtemplate>
                                $<%# Eval("FicaAmount")%>
                            </itemtemplate>
                        </asp:templatefield>
                        <asp:templatefield headertext="Backup Withholding Amount">
                            <itemtemplate>
                                $<%# Eval("BackupWithholdingAmount")%>
                            </itemtemplate>
                        </asp:templatefield>
                        <asp:templatefield headertext="Net Earning">
                            <itemtemplate>
                                $<%# Eval("NetEarningsAmount")%>
                            </itemtemplate>
                        </asp:templatefield>
                        <asp:boundfield datafield="AdjustReason" headertext="Adjust Reason" />
                        <asp:boundfield DataField="Payer_Name" HeaderText="Payer Name" />
                        <asp:templatefield HeaderText="Print">
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
                        </asp:templatefield>
                    </columns>
                    <pagerstyle cssclass="gridpager" horizontalalign="Right" />
                    <headerstyle backcolor="#cde1ec" bordercolor="Black" forecolor="black" cssclass="style3" width="100px" />
                    <alternatingrowstyle cssclass="gridViewAltRow" />
                    <rowstyle cssclass="gridViewRow" />
                    <footerstyle cssclass="gridViewFooter" />
                </asp:gridview>
            </asp:panel>
        </asp:panel>
    </asp:panel>
</div>
