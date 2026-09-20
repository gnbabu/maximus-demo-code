<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_SubmitClaimSearchPage" Codebehind="SubmitClaimSearchPage.ascx.cs" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<div>
    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="CheckEligibility" ShowSummary="true" />
</div>

<div class="row" style="text-align: center;">
    <div class="col-sm-6 col-md-4 col-lg-2 ">
        <span class="ohio-field-label"> <b>NPI</b>
            <asp:TextBox ID="txtHCPCSCode" CssClass="ohio-field-input" runat="server">
            </asp:TextBox>
        </span>

    </div>
     
    <div class="col-sm-6 col-md-4 col-lg-2 ">
        <span class="ohio-field-label">  <b>Medicaid ID</b>
            <asp:TextBox ID="txtMedicaidID" CssClass="ohio-field-input" runat="server">
            </asp:TextBox>
        </span>

    </div>
    <div class="col-sm-6 col-md-4 col-lg-5 ">
        <span class="ohio-field-label"> <b>Business/Last Name</b>
            <asp:TextBox ID="txtBusinessLastName" CssClass="ohio-field-input" runat="server">
            </asp:TextBox>
        </span>

    </div>
     <div class="col-sm-6 col-md-4 col-lg-2 ">
        <span class="ohio-field-label"> <b>First Name</b>
            <asp:TextBox ID="txtFirstName" CssClass="ohio-field-input" runat="server">
            </asp:TextBox>
        </span>
    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 " style="text-align: left; margin-left:20px;">
        <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="ProviderSearch" OnClientClick="showProgress()" Style="background-color: darkslateblue !important" />
<%--        <asp:Button ID="brnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Click" CssClass="button" CausesValidation="true" />--%>
    </div>
</div>
<asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>

<div style="margin:20px;">
<mms:SortablePagingGridView
    ID="gvSubmitClaimSearchPage"
    runat="server"
    AutoGenerateColumns="False"
    CssClass="gridViewSmallFont" Width="100%"
    AllowSorting="true"
    EmptyDataText="No Providers found."
    OnPageIndexChanging="gvSubmitClaimSearchPage_PageIndexChanging"
    OnSorting="gvHSCPSCodeSearch_Sorting"
    RowStyle-VerticalAlign="Top"
    AllowPaging="True"
    PageSize="15"
    GridViewSortColumn="NPI" GridViewSortDirection="Ascending"
    DataKeyNames="MEDICAID_ID, LAST_NAME,FIRST_NAME">
    <Columns>

        <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
        <asp:BoundField DataField="MEDICAID_ID" HeaderText="Medicaid ID" SortExpression="MEDICAID_ID" />
        <asp:BoundField DataField="LAST_NAME" HeaderText="Business/Last Name"   SortExpression="LAST_NAME" />
        <asp:BoundField DataField="FIRST_NAME" HeaderText="First Name" SortExpression="FIRST_NAME" />
       
    </Columns>
</mms:SortablePagingGridView>
</div>
