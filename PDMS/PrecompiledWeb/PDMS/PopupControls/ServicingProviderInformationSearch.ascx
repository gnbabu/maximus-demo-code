<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ServicingProviderInformation, App_Web_av5ll3zk" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<div>
    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="CheckEligibility" ShowSummary="true" />
</div>

<div class="row" style="text-align: center;">
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label"> <b>NPI</b>
            <asp:TextBox ID="txtNPI" CssClass="ohio-field-input" runat="server">
            </asp:TextBox>
        </span>

    </div>
     
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label">  <b>Medicaid ID</b>
            <asp:TextBox ID="txtMedicaidID" CssClass="ohio-field-input" runat="server">
            </asp:TextBox>
        </span>

    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label"> <b>Business/Last Name</b>
            <asp:TextBox ID="txtBusinessLastName" CssClass="ohio-field-input" runat="server">
            </asp:TextBox>
        </span>

    </div>
     <div class="col-sm-6 col-md-4 col-lg-3 ">
        <span class="ohio-field-label"> <b>First Name</b>
            <asp:TextBox ID="txtFirstName" CssClass="ohio-field-input" runat="server">
            </asp:TextBox>
        </span>

    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 ">
        <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" ValidationGroup="ProviderSearch" OnClientClick="showProgress()" Style="background-color: darkslateblue !important" />
       <asp:Button ID="brnCancel" Text="Cancel" runat="server" OnClick="btnCancel_Click"
                            CssClass="button" CausesValidation="true" />
        </div>
</div>
<asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>
<mms:SortablePagingGridView
    ID="gvSerProviderInfoSearch"
    runat="server"
    AutoGenerateColumns="False"
    CssClass="gridViewSmallFont" Width="100%"
    AllowSorting="true"
    EmptyDataText="No Providers found."
    OnPageIndexChanging="gvSerProviderInfoSearch_PageIndexChanging"
    OnSorting="gvSerProviderInfoSearch_Sorting"
    RowStyle-VerticalAlign="Top"
    AllowPaging="True"
    PageSize="15"
    GridViewSortColumn="NPI" GridViewSortDirection="Ascending"
    DataKeyNames="MedicaidID, BusinessLastName,FirstName">
    <Columns>

        <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
        <asp:BoundField DataField="MedicaidID" HeaderText="Medicaid ID" SortExpression="MedicaidID" />
        <asp:BoundField DataField="BusinessLastName" HeaderText="Business/Last Name"   SortExpression="BusinessLastName" />
        <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
       
    </Columns>
</mms:SortablePagingGridView>