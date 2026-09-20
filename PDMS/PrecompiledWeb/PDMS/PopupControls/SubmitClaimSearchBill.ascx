<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SubmitClaimSearchBill, App_Web_wenzyumt" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<div>
    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="CheckEligibility" ShowSummary="true" />
</div>

<div class="row" style="text-align: center; margin-left:10px; margin-right:10px;" >
   <div class="row" style="text-align: center;">
        <div class="col-sm-6 col-md-4 col-lg-3 ">
            <span class="ohio-field-label"><b>Type of Bill</b>
                <asp:TextBox ID="txtCode" CssClass="ohio-field-input" runat="server"></asp:TextBox>
            </span>

        </div>

        <div class="col-sm-6 col-md-4 col-lg-6 ">
            <span class="ohio-field-label"><b>Type of Bill Description</b>
                <asp:TextBox ID="txtPlaceOfServiceName" CssClass="ohio-field-input" runat="server">
                </asp:TextBox>
                <asp:LinkButton ID="lnkPLaceServiceName" Text="Search" runat="server" ToolTip="Search" OnClick="LinkButton8_Click" Visible="true"></asp:LinkButton>
            </span>

        </div>

    <div class="col-sm-6 col-md-4 col-lg-3 " style="padding-top:12px;">
        <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="ProviderSearch" OnClientClick="showProgress()" Style="background-color: darkslateblue !important" />
    </div>
    </div>

</div>
<asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>
<div style="margin:20px;">
<mms:SortablePagingGridView
    ID="gvSubmitClaimSearchPop"
    runat="server"
    AutoGenerateColumns="False"
    CssClass="gridViewSmallFont" Width="100%"
    AllowSorting="true"
    EmptyDataText="No Providers found."
    OnPageIndexChanging="gvSubmitClaimSearchPop_PageIndexChanging"
    OnSorting="gvSubmitClaimSearchPop_Sorting"
    RowStyle-VerticalAlign="Top"
    AllowPaging="True"
    PageSize="15"
    GridViewSortColumn="Code" GridViewSortDirection="Ascending"
    DataKeyNames="TOB, TOBDesc">
    <Columns>

        <asp:BoundField DataField="TOB" HeaderText="Type of Bill" SortExpression="TOB" />
        <asp:BoundField DataField="TOBDesc" HeaderText="Type of Bill Description" SortExpression="TOBDesc" />
       
    </Columns>
</mms:SortablePagingGridView>
</div>
