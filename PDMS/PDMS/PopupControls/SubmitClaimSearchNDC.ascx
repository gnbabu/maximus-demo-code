<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_SubmitClaimSearchNDC" Codebehind="SubmitClaimSearchNDC.ascx.cs" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">


<script type="text/javascript">
    function loaderSerachNDC() {

        document.getElementById('<%= txtCode.ClientID %>').disabled = true;
        document.getElementById('<%= txtTradeName.ClientID %>').disabled = true;
        document.getElementById('<%= btnloading.ClientID %>').style.display = 'block'; 
        document.getElementById('<%= btnSearchNDC.ClientID %>').style.display = 'none';
    }
    </script>
<%--<div>
    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="CheckEligibility" ShowSummary="true" />
</div>--%>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="row" style="text-align: center; margin-left: 10px; margin-right: 10px;">
            <div class="row m-0 popUpSearch-Context d-FlexCenter" style="text-align: center;">
                <div class="col-sm-6 col-md-4 col-lg-3 ">
                    <asp:TextBox ID="txtCode" CssClass="ohio-field-input" MaxLength="11" runat="server"></asp:TextBox>
                   <%-- <asp:RangeValidator runat="server" ControlToValidate="txtCode" ErrorMessage="11-digit number is required"
    Type="Integer" MinimumValue="11" ForeColor="Red"></asp:RangeValidator>--%>

                </div>

                <div class="col-sm-6 col-md-4 col-lg-6 ">
                    <asp:TextBox ID="txtTradeName" CssClass="ohio-field-input" runat="server" MaxLength="100">
                    </asp:TextBox>
                </div>

                <div class="col-sm-6 col-md-4 col-lg-3 " style="padding-top: 12px;">
                      <button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                    
                    <asp:Button ID="btnSearchNDC" OnClientClick="return GetNDCDetails()" runat="server" CssClass="buttonBox StepButton buttonBoxFocus"  Text="Search" CausesValidation="false" />

<%--                    <asp:Button ID="btnSearchNDC" runat="server" UseSubmitBehavior="false" CausesValidation="false" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" OnClientClick="loaderSerachNDC()" Style="background-color: darkslateblue !important" />--%>
                </div>
            </div>
            <div class="row m-0 popUpSearch-Context" style="text-align: left; padding: 1px">
                 <div class="col-sm-6 col-md-4 col-lg-6">
                    <asp:Label ID="fieldRequireError" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>

        </div>
        <div class="search-Results">SEARCH RESULTS</div>
        <asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>
            <div id="divSearchNDCDetails" runat="server" style="min-height: 200px; min-width: 800px;max-height:300px;max-width:900px;overflow-y:scroll" class="row m-0 popUpSearch-Context">
        <div class="row m-0 popUpSearch-Context" style="padding-left: 20px; padding-right: 20px;">
            <mms:SortablePagingGridView
                ID="gvSubmitClaimSearchNDCPop"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="gridViewSmallFont" Width="100%"
                AllowSorting="true"
                EmptyDataText="NDC is not Found."
                OnPageIndexChanging="gvSubmitClaimSearchNDCPop_PageIndexChanging"
                OnSorting="gvSubmitClaimSearchNDCPop_Sorting"
                RowStyle-VerticalAlign="Top"
                AllowPaging="True"
                PageSize="15"
                CellSpacing="10"
                GridViewSortColumn="Code" GridViewSortDirection="Ascending"
                DataKeyNames="CLAIMS_NDC_CODE, LAY_DESC"
                Style="overflow-y: scroll">
                <Columns>
                   
                </Columns>
            </mms:SortablePagingGridView>
        </div>
</div>
        <asp:HiddenField ID="hdnNDC_Code" runat="server" />
        <asp:HiddenField ID="hdnGridNDC_Code" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

