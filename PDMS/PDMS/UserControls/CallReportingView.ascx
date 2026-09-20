<%@ Control Language="C#" AutoEventWireup="true" Inherits="Views_CallReportingView" Codebehind="CallReportingView.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

<script type="text/javascript">
    function maxZIndex() {
        var highest = -999;

        $("*").each(function () {
            var current = parseInt($(this).css("z-index"), 10);
            if (current && highest < current)
                highest = current;
        });

        return highest;
    }

    function closeDialog(btn) {
        var divName;
        if (btn.parentElement.id == "divRowCount")
            divName = "divRowCount";
        else if (btn.parentElement.id == "<%= divCallDetails.ClientID %>")
            divName = "<%= divCallDetails.ClientID %>";

        $("#" + divName).dialog("close");
    }

    function exportPopup() {
        var rowCount = $("#<%= hdnRowCount.ClientID %>").first().val();
        if (rowCount > 1000) {
            $("#divRowCount").dialog();
        }
    }
</script>
<style type="text/css">
    select {
        min-width: 90%;
    }
</style>

<div style="padding: 5px; height: auto;">
    <cc1:GroupBox ID="gbSearch" Caption="Search Criteria" CaptionStyle-CssClass="bodyTextBold" HorizontalAlign="Center" Width="98%" runat="server">
        <div style="text-align: left;">
            <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
            <asp:ValidationSummary ID="vsReportCriteria" DisplayMode="List" runat="server" ValidationGroup="ReportCriteriaVal" ShowSummary="true" Enabled="true" CssClass="error-message" />
        </div>
        <p style="text-align: center; font-weight: bold;">At least 1 search field must be entered.  Enter both Start and End Date to search a date range</p>
        <br />
        <br />
        <div>
            <div class="row">
                <div class="col-sm-2 text-right"><span class="formLabel wd120"><asp:Label ID="lblStartDate" runat="server" AssociatedControlID="txtStartDate" Text="Start Date*"/></span></div>
                <div class="col-sm-4 text-left">
                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="textEntry" />
                    <ajax:CalendarExtender ID="ceStartDate" TargetControlID="txtStartDate" runat="server" />
                    <asp:CompareValidator ID="cvStartDate" runat="server" ValidationGroup="ReportCriteriaVal"
                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtStartDate" Enabled="true"
                        ErrorMessage="* A valid Requested Start Date is required (mm/dd/yyyy)." Text="" Display="Dynamic"
                        ValueToCompare="MM/dd/yyyy" SetFocusOnError="true"> 
                    </asp:CompareValidator>
                </div>
                <div class="col-sm-2 text-right"><span class="formLabel wd120"><asp:Label ID="lblEndDate" runat="server" AssociatedControlID="txtEndDate" Text="End Date*"/></span></div>
                <div class="col-sm-4 text-left">
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="textEntry" />
                    <ajax:CalendarExtender ID="ceEndDate" TargetControlID="txtEndDate" runat="server" />
                    <asp:CompareValidator ID="cvEndDate" runat="server" ValidationGroup="ReportCriteriaVal"
                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate" Enabled="true"
                        ErrorMessage="* A valid End Date is required (mm/dd/yyyy)." Text="" Display="Dynamic"
                        ValueToCompare="MM/dd/yyyy" SetFocusOnError="true"> 
                    </asp:CompareValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2 text-right"><span class="formLabel wd120"><asp:Label ID="lblSource" runat="server" AssociatedControlID="ddlSource" Text="Caller Type"/></span></div>
                <div class="col-sm-4 text-left">
                    <asp:DropDownList ID="ddlSource" runat="server" CssClass="textEntry" /></div>
                <div class="col-sm-2 text-right"><span class="formLabel wd120"><asp:Label ID="lblReason" runat="server" AssociatedControlID="ddlReason" Text="Call Reason"/></span></div>
                <div class="col-sm-4 text-left">
                    <asp:DropDownList ID="ddlReason" runat="server" CssClass="textEntry" /></div>
            </div>
            <div class="row">
                <div class="col-sm-2 text-right"><span class="formLabel wd120"><asp:Label ID="lblResolution" runat="server" AssociatedControlID="ddlResolution" Text="Action Taken"/></span></div>
                <div class="col-sm-4 text-left">
                    <asp:DropDownList ID="ddlResolution" runat="server" CssClass="textEntry" /></div>
                <div class="col-sm-6"></div>
            </div>
            <div class="row">
                <div class="col-sm-2 text-right"><span class="formLabel wd120"><asp:Label ID="lblNPI" runat="server" AssociatedControlID="txtNPI" Text="NPI"/></span></div>
                <div class="col-sm-4 text-left">
                    <asp:TextBox ID="txtNPI" runat="server" CssClass="textEntry" MaxLength="20" />
                    <asp:RegularExpressionValidator ID="valNPI" runat="server" ControlToValidate="txtNPI" ErrorMessage="NPI must be 10 digits and start with 1 or 2."
                        ValidationGroup="ReportCriteriaVal" ValidationExpression="^([1-2][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$" Display="Dynamic"
                        SetFocusOnError="true" Text="*"></asp:RegularExpressionValidator>
                </div>
                <div class="col-sm-2 text-right"><span class="formLabel wd120"><asp:Label ID="lblMedicaidID" runat="server" AssociatedControlID="txtMedicaidID" Text="Medicaid ID"/></span></div>
                <div class="col-sm-4 text-left">
                    <asp:TextBox ID="txtMedicaidID" runat="server" CssClass="textEntry" MaxLength="20" />
                    <asp:RegularExpressionValidator ID="valMedicaidID" runat="server" ControlToValidate="txtMedicaidID" ErrorMessage="Medicaid ID must be 9 digits and start with 0."
                        ValidationGroup="ReportCriteriaVal" ValidationExpression="^([0][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$" Display="Dynamic"
                        SetFocusOnError="true" Text="*"></asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-2 text-right"><span class="formLabel wd120"><asp:Label ID="lblCallID" runat="server" AssociatedControlID="txtCallID" Text="Call ID"/></span></div>
                <div class="col-sm-4 text-left">
                    <ew:NumericBox ID="txtCallID" runat="server" CssClass="textEntry" CausesValidation="true" MaxLength="9" /></div>
                <div class="col-sm-6"></div>
            </div>
        </div>
        <div class="btnBox btnBoxCenter" style="padding-top: 10px; padding-right: 10px;">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonBox buttonBoxFocus" OnClick="btnSearch_Click"
                CausesValidation="true" ValidationGroup="ReportCriteriaVal" Enabled="true" />
            <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" />
        </div>
    </cc1:GroupBox>
    <br />

    <div style="width: 100%; text-align: right">
        <asp:HiddenField ID="hdnRowCount" runat="server" />
        <telerik:RadGrid ID="RadGridExport" runat="server" Visible="true">
            <ExportSettings IgnorePaging="true" OpenInNewWindow="true">
                <Pdf PageHeight="8.5in" PageWidth="11in" PageTitle="Provider Search">
                    <PageFooter>
                        <RightCell Text="Page <?page-number?>" />
                    </PageFooter>
                </Pdf>
            </ExportSettings>
            <MasterTableView AutoGenerateColumns="false">
                <Columns>
                    <telerik:GridBoundColumn DataField="CallID" HeaderText="Call ID" SortExpression="CallID" />
                    <telerik:GridBoundColumn DataField="RegID" HeaderText="Reg ID" SortExpression="RegID" />
                    <telerik:GridBoundColumn DataField="MedicaidID" HeaderText="Medicaid ID" SortExpression="MedicaidID" />
                    <telerik:GridBoundColumn DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
                    <telerik:GridBoundColumn DataField="SourceName" HeaderText="Caller Type" SortExpression="SourceName" />
                    <telerik:GridBoundColumn DataField="SubjectName" HeaderText="Call Reason" SortExpression="SubjectName" />
                    <telerik:GridBoundColumn DataField="StartTime" HeaderText="Started On" SortExpression="StartTime" />
                    <telerik:GridBoundColumn DataField="EndTime" HeaderText="Ended On" SortExpression="EndTime" />
                    <telerik:GridBoundColumn DataField="Duration" HeaderText="Duration" SortExpression="Duration" DataFormatString="{0:hh\:mm\:ss}" />
                    <telerik:GridBoundColumn DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                    <telerik:GridBoundColumn DataField="NextActionName" HeaderText="Action Taken" SortExpression="NextActionName" />
                    <telerik:GridBoundColumn DataField="CallDetails" HeaderText="Call Details" SortExpression="CallDetails" />
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        <asp:LinkButton ID="lnkExcel" runat="server" ToolTip="Excel" OnClick="lnkExcel_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/Excel_24x24.png" alt="PDF" /></asp:LinkButton>&nbsp;&nbsp;
        <asp:LinkButton ID="lnkPDF" runat="server" ToolTip="PDF" OnClick="lnkPDF_Click" OnClientClick="exportPopup();" Visible="false"><img src="../Images/PDF_24x24.png" alt="PDF" /></asp:LinkButton>
        <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
            <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 1000 results from the search will be exported.</p>
            <asp:Button ID="btnOKTopRows" class="buttonBox" Text="Ok" runat="server" OnClientClick="closeDialog(this);" />
        </div>
    </div>

    <%--    TODO:  Turn on paging and sorting--%>
    <div style="width: 100%; ">
    <telerik:RadGrid ID="gvCalls" runat="server" AllowSorting="false" AllowMultiRowSelection="false" BorderStyle="None"
        CssClass="gridView gridViewSmallFont" CellPadding="0" EnableEmbeddedSkins="false" ViewStateMode="Enabled"
        AllowPaging="false" Width="100%" RenderMode="Lightweight" GridLines="Horizontal" MasterTableView-NoDetailRecordsText="No records to display"
        AllowFilteringByColumn="false"
        OnSortCommand="gvCalls_SortCommand"
        OnItemDataBound="gvCalls_ItemDataBound"
        OnPageIndexChanged="gvCalls_PageIndexChanged"
        OnItemCommand="gvCalls_ItemCommand"
        OnSelectedIndexChanged="gvCalls_SelectedIndexChanged" Skin="PDMSModern" >
        <HeaderStyle CssClass="gridViewHeader" />
        <PagerStyle Mode="NumericPages" CssClass="gridViewPager" />
        <ItemStyle CssClass="gridViewRow" />
        <AlternatingItemStyle CssClass="gridViewAltRow" />
        <SelectedItemStyle CssClass="gridViewSelected" />
        <MasterTableView DataKeyNames="CallID"
            AutoGenerateColumns="false"
            AllowMultiColumnSorting="false"
            CommandItemDisplay="TopAndBottom">
            <CommandItemSettings ShowExportToExcelButton="false" ExportToExcelText="" ExportToExcelImageUrl="~/Images/export-excel.jpg" ShowAddNewRecordButton="false" ShowRefreshButton="false"  />
            <Columns>
                <telerik:GridBoundColumn DataField="CallID" HeaderText="Call ID" SortExpression="CallID" />
                <telerik:GridBoundColumn DataField="RegID" HeaderText="Reg ID" SortExpression="RegID" />
                <telerik:GridBoundColumn DataField="MedicaidID" HeaderText="Medicaid ID" SortExpression="MedicaidID" />
                <telerik:GridBoundColumn DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
                <telerik:GridBoundColumn DataField="SourceName" HeaderText="Caller Type" SortExpression="SourceName" />
                <telerik:GridBoundColumn DataField="SubjectName" HeaderText="Call Reason" SortExpression="SubjectName" />
                <telerik:GridBoundColumn DataField="StartTime" HeaderText="Started On" SortExpression="StartTime" />
                <telerik:GridBoundColumn DataField="EndTime" HeaderText="Ended On" SortExpression="EndTime" />
                <telerik:GridBoundColumn DataField="Duration" HeaderText="Duration" SortExpression="Duration" DataFormatString="{0:hh\:mm\:ss}" />
                <telerik:GridBoundColumn DataField="UserName" HeaderText="User Name" SortExpression="UserName" />
                <telerik:GridButtonColumn ButtonType="ImageButton" HeaderText="Call Details" ImageUrl="../Images/edit.png" CommandName="ViewCallDetails"></telerik:GridButtonColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    </div>
    <br />

    <div runat="server" id="divCallDetails" title="Call Details" style="display: none; text-align: center; width:800px;"><br />
        <div style="width:100%; text-align:left">
            <asp:Label ID="lblActionTakenLabel" runat="server" CssClass="formLabel wd120" Text="Action Taken:"></asp:Label>
            <asp:Label ID="lblActionTakenText" runat="server" MaxLength="20" /><br /><br />
        </div>
        <asp:TextBox ID="txtCallDetails" runat="server" Width="87%" TextMode="MultiLine" Rows="10" title="Call Action Details"></asp:TextBox><br /><br />
        <asp:Button ID="btnOKCallDetails" class="buttonBox" Text="Ok" runat="server" OnClientClick="closeDialog(this);" />
    </div>
    <br />
    <br />
</div>
