<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SubmitClaimSearchReason, App_Web_wenzyumt" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">

<style type="text/css">
    .formLabel200 {
        
        
        text-align: center !important;
    }</style>
<script type="text/javascript">

    function loadSearchHeaderOtherPayerInstitutional() {
       
        document.getElementById('<%= txtCode.ClientID %>').disabled = true;
        document.getElementById('<%= txtPlaceOfServiceName.ClientID %>').disabled = true;
        document.getElementById('<%= btnSearch.ClientID %>').style.display = 'none';
        document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';
        
     }

</script>
<%--<div>
    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="CheckEligibility" ShowSummary="true" />
</div>--%>

<div class="row" style="text-align: center; margin-left: 10px; margin-right: 10px;">
    <div class="row m-0 popUpSearch-Context d-FlexCenter">
        <div class="col-sm-6 col-md-4 col-lg-3 ">

            <asp:TextBox ID="txtCode" CssClass="ohio-field-input" runat="server" MaxLength="5"></asp:TextBox>

        </div>

        <div class="col-sm-6 col-md-4 col-lg-6 ">

            <asp:TextBox ID="txtPlaceOfServiceName" CssClass="ohio-field-input" runat="server" MaxLength="100"></asp:TextBox>

        </div>

        <div class="col-sm-6 col-md-4 col-lg-3 " style="padding-top: 12px;">
             <button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                               <br />
            <asp:Button ID="btnSearch" runat="server" CausesValidation="false" Text="Search" CssClass="buttonBoxFocus"  OnClientClick="return GetHeaderAndOtherPayerDetails()"   Style="background-color: darkslateblue !important" />
             </div>
    </div>

</div>
<div class="row m-0 popUpSearch-Context" style="text-align: left; padding: 1px">

                <div class="col-sm-6 col-md-4 col-lg-6">
                    <asp:Label runat="server" ID="lblError" Text="" ForeColor="Red" Visible="false" />
                </div>
            </div>
<div class="search-Results">SEARCH RESULTS</div>

<%--<asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>--%>
<div class="result-Container" style="overflow-y: scroll;padding-left: 20px;padding-right: 20px;">
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
                 DataKeyNames="CLAIMS_REASON_CODE,CLAIMS_REASON_CODE_DESC">
                <Columns>

                    
                </Columns>
            </mms:SortablePagingGridView>

</div>
