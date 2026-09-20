<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_HospiceCheckEligibility, App_Web_wenzyumt" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<div>
    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="CheckEligibility" ShowSummary="true" />
</div>

<div class="row" style="text-align: center;">
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label"><span style="color: red">*</span> <b>Start Date</b>
            <asp:TextBox ID="txtStartDate" CssClass="ohio-field-input" runat="server">
            </asp:TextBox>
            <ajax:CalendarExtender ID="clStartDate" TargetControlID="txtStartDate" runat="server" />
            <asp:RequiredFieldValidator runat="server" ID="reStartDate" ControlToValidate="txtStartDate" ErrorMessage="* Start Date is required." Text="*" Display="Dynamic"
                SetFocusOnError="true" ValidationGroup="CheckEligibility" />
        </span>

    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label"><span style="color: red">*</span><b>End Date </b>
            <asp:TextBox ID="txtEndDate" CssClass="ohio-field-input" runat="server"></asp:TextBox>
            <ajax:CalendarExtender ID="clEndDate" TargetControlID="txtEndDate" runat="server" />
            <asp:RequiredFieldValidator runat="server" ID="reEndDate" ControlToValidate="txtEndDate" ErrorMessage="* End Date is required." Text="*" Display="Dynamic"
                SetFocusOnError="true" ValidationGroup="CheckEligibility" />
        </span>
    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="ProviderSearch" OnClientClick="showProgress()" Style="background-color: darkslateblue !important" />
    </div>
</div>
<asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>
<mms:SortablePagingGridView
    ID="gvHospiceCheckEligibility"
    runat="server"
    AutoGenerateColumns="False"
    CssClass="gridViewSmallFont" Width="100%"
    AllowSorting="true"
    EmptyDataText="No Providers found."
    OnPageIndexChanging="gvHospiceCheckEligibility_PageIndexChanging"
    OnSorting="gvHospiceCheckEligibility_Sorting"
    RowStyle-VerticalAlign="Top"
    AllowPaging="True"
    PageSize="15"
    GridViewSortColumn="Health_Plan" GridViewSortDirection="Ascending"
    DataKeyNames="Effective_Date, End_Date">
    <Columns>

        <asp:BoundField DataField="Health_Plan" HeaderText="Health/Assignment Plan" SortExpression="Health_Plan" />
        <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" SortExpression="Effective_Date" />
        <asp:BoundField DataField="End_Date" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" SortExpression="End_Date" />
    </Columns>
</mms:SortablePagingGridView>


