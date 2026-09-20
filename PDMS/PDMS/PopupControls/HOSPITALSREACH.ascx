<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_HOSPITALSREACH" Codebehind="HOSPITALSREACH.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<script type="text/javascript">

    function CollapseExpand(obj) {
        var sp = obj.getElementsByTagName('span')[0];
        var spText = sp.innerHTML;
        var plusIndex = spText.indexOf("+");

        if (plusIndex == 0)
            sp.innerHTML = spText.replace("+", "-");
        else
            sp.innerHTML = spText.replace("-", "+");
    }

</script>

<div class="row col-sm-12" style="border: groove; margin-left: 10px">

    <ajax:CollapsiblePanelExtender ID="cpeHospitalHistoryCRSearch" runat="server" Collapsed="false" TargetControlID="pnlHospitalHistoryCRSearch"
        ExpandControlID="pnlSepHospitalHistoryCRSearch" CollapseControlID="pnlSepHospitalHistoryCRSearch" />
    <asp:Panel runat="server" ID="pnlSepHospitalHistoryCRSearch" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: cornflowerblue;" ToolTip="Click to Expand/Collapse">
        <span id="sepHospitalHistoryCRSearch" runat="server" class="pageHeader">- HOSPITAL COST REPORT SUBMISSION HISTORY</span></asp:Panel>

    <asp:Panel runat="server" ID="pnlHospitalHistoryCRSearch" class="OwnerBackground" style="min-height:300px; min-width:150px; height:auto; width:auto; max-width:1200px;">
        <div class="row" style="margin-top:20px;">
            <div class="col-sm-2"><span class="ohio-field" style="font-size: 15px; text-align: right">Medicaid Provider ID</span></div>
            <div class="col-sm-2"><span style="text-align: left;"><asp:TextBox ID="txtMedicaidProviderId" runat="server" CssClass="formField" Style="height: 30px; width: 200px" /></span></div>
            <div class="col-sm-2"><span class="ohio-field" style="font-size: 15px; text-align: right">Settlement Type</span></div>
            <div class="col-sm-2"><span style="text-align: left;"><asp:DropDownList ID="ddlSettlementType" AutoPostBack="false" runat="server" Style="min-width: 250px; height: 30px"></asp:DropDownList></span></div>
            <div class="col-sm-4">&nbsp;</div>
        </div> 
        <div class="row">
            <div class="col-sm-2"><span class="ohio-field" style="font-size: 15px; text-align: right">Tracking Number</span></div>
            <div class="col-sm-2"><span style="text-align: left;"><asp:TextBox ID="TextBox1" runat="server" CssClass="formField" Style="height: 30px; width: 200px" /></span></div>
            <div class="col-sm-2"><span class="ohio-field" style="font-size: 15px; text-align: right">Settlement Amount</span></div>
            <div class="col-sm-2"><span style="text-align: left;"><asp:TextBox ID="TextBox2" runat="server" CssClass="formField" Style="height: 30px; width: 200px" /></span></div>
           
        </div>
       <div class="row">
            <div class="col-sm-2"><span class="ohio-field" style="font-size: 15px; text-align: right">Name</span></div>
            <div class="col-sm-3"><span style="text-align: left;"><asp:TextBox ID="txtName" runat="server" CssClass="formField" Style="height: 30px; width: 200px" /></span></div>
             <div class="col-sm-2"><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click"
                            ToolTip="Search data" BackColor="#66FF66" /></div>
            <div class="col-sm-1 "><asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBoxFocus" BackColor="#FF5050"  /></div>

        </div>
        <div class="row">
            <div class="col-sm-8"></div>
            <div class="col-sm-2"><span class="ohio-field" style="font-size: 15px; text-align: right">Max Records</span></div>
            <div class="col-sm-2">
               <span style="text-align: left;">
                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" Style="min-width: 100px; height: 30px">
                            <asp:ListItem Text="10" Value="10" />
                            <asp:ListItem Text="25" Value="25" />
                            <asp:ListItem Text="50" Value="50" />
                        </asp:DropDownList>
                   </span>
            </div>
        </div>

        
        
<asp:HiddenField ID="RegIdTxt" runat="server" />
    </asp:Panel>
    <ajax:CollapsiblePanelExtender ID="cpeOwnInfo" runat="server" Collapsed="false" TargetControlID="pnlOwnInfo1" ExpandControlID="pnlSepCRSearchResults" CollapseControlID="pnlSepCRSearchResults" />
    <asp:Panel runat="server" ID="pnlSepCRSearchResults" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: cornflowerblue;" ToolTip="Click to Expand/Collapse">
        <span id="sepCRSearchResults" runat="server" class="pageHeader">+ HOSPITAL COST REPORT SEARCH RESULTS</span>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlOwnInfo1" CssClass="OwnerBackground">
        <div class="divGrid" style="padding-top: 10px;min-height:150px;">
            <asp:GridView ID="gvRemittanceAdvicesearch" runat="server" Width="100%" AllowSorting="false" CssClass="auto-style2"
                EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Center" >
                <Columns>
                    <asp:BoundField HeaderText="Tracking Number" DataField="MITSTrackingNumber" />
                    <asp:BoundField HeaderText="Provider Medical ID" DataField="IdProvider" />
                    <asp:BoundField HeaderText="Provider NPI" DataField="NPI" />
                    <asp:BoundField HeaderText="Provider Name" DataField="ProviderName" />
                    <asp:BoundField HeaderText="Settlement Amount" DataField="SettlementAmt" />
                    <asp:BoundField HeaderText="Settlement Type" DataField="SettleType" />
                    <asp:BoundField HeaderText="Doc ID" DataField="CrDocumentNumber" />
                    <asp:BoundField HeaderText="Doc Type" DataField="DocType" />
                    <asp:BoundField HeaderText="Date Received" DataField="DateReceived" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:Panel>
</div>

<div class="row col-sm-12" style="border: groove; margin-left: 10px">

    <ajax:CollapsiblePanelExtender ID="cpeHospitalHistoryCRLetterSearch" runat="server" Collapsed="false" TargetControlID="pnlHospitalHistoryCRLetterSearch"
        ExpandControlID="pnlSepHospitalHistoryCRLetterSearch" CollapseControlID="pnlSepHospitalHistoryCRLetterSearch" />
    <asp:Panel runat="server" ID="pnlSepHospitalHistoryCRLetterSearch" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: cornflowerblue;" ToolTip="Click to Expand/Collapse">
        <span id="sepHospitalHistoryCRLetterSearch" runat="server" class="pageHeader">- HOSPITAL COST SETTLEMENT REPORT SEARCH</span></asp:Panel>

    <asp:Panel runat="server" ID="pnlHospitalHistoryCRLetterSearch" class="OwnerBackground" style="min-height:300px; min-width:150px; height:auto; width:auto; max-width:1200px;">
        <div class="row" style="margin-top:20px;">
            <div class="col-sm-2"><span class="ohio-field" style="font-size: 15px; text-align: right">Report</span></div>
            <div class="col-sm-4">
                <span style="text-align: left;">
                <asp:DropDownList ID="ddlReportType" runat="server"  CssClass="formField" Style="height: 30px; min-width: 250px"></asp:DropDownList>
                    </span>
            </div>
            <div class="col-sm-2"><span class="ohio-field" style="font-size: 15px; text-align: right">Period Type</span></div>
            <div class="col-sm-4">
                <span style="text-align: left;">
                    <asp:DropDownList ID="ddlPeriodType" runat="server"   CssClass="formField" Style="height: 30px;min-width: 250px"></asp:DropDownList>
                </span>
            </div>
        </div>
        <div class="row">
            
            <div class="col-sm-2 col-sm-offset-1"><asp:Button ID="btnSettelementSearch" runat="server" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSettelementSearch_Click"
                            ToolTip="Search data" BackColor="#66FF66" /></div>
            <div class="col-sm-2"><asp:Button ID="Button2" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBoxFocus" BackColor="#FF5050"  /></div>
            
        </div>
        <div class="row">
            
           
        </div>

    </asp:Panel>
    <ajax:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" Collapsed="true" TargetControlID="Panel1" ExpandControlID="pnlSepCRLetterSearchResults" CollapseControlID="pnlSepCRLetterSearchResults" />
    <asp:Panel runat="server" ID="pnlSepCRLetterSearchResults" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: cornflowerblue;" ToolTip="Click to Expand/Collapse">
        <span id="sepCRLetterSearchResults" runat="server" class="pageHeader">+ HOSPITAL COST SETTLEMENT SEARCH RESULTS</span>
    </asp:Panel>
    <asp:Panel runat="server" ID="Panel1" CssClass="OwnerBackground">
        <div class="divGrid" style="padding-top: 10px; min-height:150px;">
            <asp:GridView ID="gridHospitalSettlementSearchResults" runat="server" Width="100%" AllowSorting="false" CssClass="auto-style2"
                EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Center" OnRowCreated="gridHospitalSettlementSearchResults_RowCreated" OnRowCommand="gridHospitalSettlementSearchResults_RowCommand"  DataKeyNames="DOCUMENT_ID">
                <Columns>
                    <asp:BoundField HeaderText="Document ID" DataField="DOCUMENT_ID" />
                    <asp:BoundField HeaderText="Report Type" DataField="REPORT_NAME" />
                    <asp:BoundField HeaderText="Period Type" DataField="COST_REPORT_FISCAL_YEAR" />
                    <asp:BoundField HeaderText="Service Date From" DataField="COST_REPORT_FROM_DATE" />
                    <asp:BoundField HeaderText="Service Date Thru" DataField="COST_REPORT_TO_DATE" />
                    <asp:BoundField HeaderText="Date Available On Portal" DataField="DATE_AVIALBLE_ON_PORTAL" />
                    <asp:BoundField HeaderText="Date First Accessed" DataField="FIRST_ACCESS_DATE" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:Panel>
</div>

<div class="row col-sm-12" style="border: groove; margin-left: 10px">

    <ajax:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" Collapsed="false" TargetControlID="Panel3"
        ExpandControlID="pnlSepHospitalHistoryCRSettlementLetterSearch" CollapseControlID="pnlSepHospitalHistoryCRSettlementLetterSearch" />
    <asp:Panel runat="server" ID="pnlSepHospitalHistoryCRSettlementLetterSearch" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: cornflowerblue;" ToolTip="Click to Expand/Collapse">
        <span id="sepHospitalHistoryCRSettlementLetterSearch" runat="server" class="pageHeader">- HOSPITAL COST SETTLEMENT LETTER SEARCH</span></asp:Panel>

    <asp:Panel runat="server" ID="Panel3" class="OwnerBackground" style="min-height:300px; min-width:150px; height:auto; width:auto; max-width:1200px;">
        <div class="row" style="margin-top:20px;">
            <div class="col-sm-2"><span class="ohio-field" style="font-size: 15px; text-align: right">Letter Name</span></div>
            <div class="col-sm-4">
                <span style="text-align: left;">
                <asp:DropDownList ID="ddlLetterType" runat="server"  CssClass="formField" Style="height: 30px; min-width: 250px"></asp:DropDownList>
                    </span>
            </div>
            <div class="col-sm-2"><span class="ohio-field" style="font-size: 15px; text-align: right">Date Type</span></div>
            <div class="col-sm-4">
                <span style="text-align: left;">
                    <asp:DropDownList ID="ddlDateType" runat="server"   CssClass="formField" Style="height: 30px;min-width: 250px"></asp:DropDownList>
                </span>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-2"><span class="ohio-field" style="font-size: 15px; text-align: right">Date From</span></div>
            <div class="col-sm-4"><span style="text-align: left;"><asp:TextBox ID="TextBox3" runat="server" CssClass="formField" Style="height: 30px; width: 200px" /></span></div>
            <div class="col-sm-2"><span class="ohio-field" style="font-size: 15px; text-align: right">Date To</span></div>
            <div class="col-sm-4"><span style="text-align: left;"><asp:TextBox ID="TextBox4" runat="server" CssClass="formField" Style="height: 30px; width: 200px" /></span></div>
        </div>
        <div class="row">
            
            <div class="col-sm-2"><span class="ohio-field" style="font-size: 15px; text-align: right">Fiscal Year</span></div>
            <div class="col-sm-2"><span style="text-align: left;"><asp:TextBox ID="TextBox5" runat="server" CssClass="formField" Style="height: 30px; width: 200px" /></span></div>
            <div class="col-sm-2 col-sm-offset-1"><asp:Button ID="btnSettelementLetterSearch" runat="server" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSettelementLetterSearch_Click"
                            ToolTip="Search data" BackColor="#66FF66" /></div>
            <div class="col-sm-2"><asp:Button ID="Button4" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBoxFocus" BackColor="#FF5050"  /></div>
            
        </div>
        <div class="row">
            
           
        </div>

    </asp:Panel>
    <ajax:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" Collapsed="true" TargetControlID="pnlSepCRSettlementLetterSearchResults" ExpandControlID="pnlSepCRSettlementLetterSearchResults" CollapseControlID="pnlSepCRSettlementLetterSearchResults" />
    <asp:Panel runat="server" ID="pnlSepCRSettlementLetterSearchResults" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" Style="background-color: cornflowerblue;" ToolTip="Click to Expand/Collapse">
        <span id="sepCRSettlementLetterSearchResults" runat="server" class="pageHeader">+ HOSPITAL COST SETTLEMENT LETTERS SEARCH RESULTS</span>
    </asp:Panel>
    <asp:Panel runat="server" ID="Panel5" CssClass="OwnerBackground">
        <div class="divGrid" style="padding-top: 10px;min-height:150px;">
            <asp:GridView ID="gridHospitalSettelementLetterSearch" runat="server" Width="100%" AllowSorting="false" CssClass="auto-style2"
                EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Center" >
                <Columns>
                    <asp:BoundField HeaderText="Document ID" DataField="DocumentID" />
                    <asp:BoundField HeaderText="Letter Name" DataField="LetterName" />
                    <asp:BoundField HeaderText="Period Type" DataField="PeriodType" />
                    <asp:BoundField HeaderText="Date Sent" DataField="DateSent" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
        </div>
    </asp:Panel>
</div>


