<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_SubmitClaimICD10ProcedureCodes" Codebehind="SubmitClaimICD10ProcedureCodes.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">
<script>
    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9- ]/g, '');
    }
</script>

<style>
    table.gridview td, .rgRow td, .rgAltRow td, table.gridViewSmallFont td {
        font-size: 14px;
    }

    select {
        min-width: 90%;
        height: 25px;
    }

    .gridViewFooter {
        background-color: white;
    }

    .gridViewSelected, .gridViewSelected td, .gridViewSelected tr {
        background-color: white;
    }
</style>
<script type="text/javascript">
    function loadSearchICDProcCode() {
        
        document.getElementById('<%= txtPlaceOfServiceName.ClientID %>').disabled = true;
          document.getElementById('<%= ddlICDPrCodeInstiutional.ClientID %>').disabled = true;
          document.getElementById('<%= btnSearch.ClientID %>').style.display = 'none';
        document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';
        document.getElementById('<%= txtCode.ClientID %>').disabled = true;
      }
    $(function () {
        $("[id*=ClaimDentserviceAdd]").click(function () {
            var row = $(this).closest("tr");
            var requiredControles = ["ddlDocumentType"];
            return RequiredFieldsValidations(row, requiredControles);
        });
    });

    function RequiredFieldsValidations(row, requiredControles) {
        var isValid = true;
        $.each(requiredControles, function (index, Id) {
            var label = row.find("[id*=" + Id + "]").next("SPAN");
            label.hide();
            if ($.trim(row.find("[id*=" + Id + "]").val()) === "") {
                label.show();
                isValid = false;
            }
        });

        return isValid;
    }

</script>
<%--<div>
    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="CheckEligibility" ShowSummary="true" />
</div>--%>

<div class="row" style="text-align: center; margin-left: 10px; margin-right: 10px;">
    <div class="row m-0 popUpSearch-Context d-FlexCenter" style="text-align: center;">
        <div class="col-sm-3">
            
            <div style="text-align: left;">
                <asp:TextBox ID="txtCode" CssClass="ohio-field-input" Style="height: 40px" Width="100px"   onKeyUp="javascript:alphanumericOnly(this);" runat="server" MaxLength="7"></asp:TextBox>
            </div>
        </div>
        <div class="col-sm-3">
            <div style="text-align: left;">
                <asp:DropDownList ID="ddlICDPrCodeInstiutional" Style=" height: 40px" Width="100px" CssClass="formField" runat="server">
                    <asp:ListItem Value="ICD 10" Text="ICD 10"></asp:ListItem>
                    <asp:ListItem Value="ICD 9" Text="ICD 9"></asp:ListItem>
                </asp:DropDownList>

            </div>
        </div>
        <div class="col-sm-4">
            <div style="text-align: left; width: 1079px;">
                <asp:TextBox ID="txtPlaceOfServiceName" CssClass="ohio-field-input" Width="325px" runat="server" MaxLength="100"></asp:TextBox>
                </div>
           
        </div>

        <div class="col-sm-2" style="padding-top: 0px;">
             <button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                     
                               <br />
            
       <asp:Button ID="btnSearch" OnClientClick="return GetIcdProcCodeDetails()" runat="server" CssClass="buttonBox StepButton buttonBoxFocus"  Text="Search" CausesValidation="false" />

            </div>
    </div>

</div>
<div class="row m-0 popUpSearch-Context" style="text-align: left; padding: 1px">

                <div class="col-sm-6 col-md-4 col-lg-6">
                    <asp:Label runat="server" ID="lblError" Text="" ForeColor="Red" Visible="false" />
                </div>
            </div>
<div class="search-Results">SEARCH RESULTS</div>
                <asp:Label ID="lblSResultICD" class="expandcollapse" runat="server" Text="" ForeColor="Red"></asp:Label>
<asp:HiddenField ID="hdnICD_PROCEDURE_CODE_ID" runat="server" />
<asp:HiddenField ID="hdnICD_Description" runat="server" />


  <div class="popupGridViewOnSearch">
      <mms:SortablePagingGridView
                ID="gvSubmitClaimSearchProcPop"
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
                 DataKeyNames="CLAIMS_ICD_PROCEDURE_CODE_ID">
                <Columns>

                    
                </Columns>
            </mms:SortablePagingGridView>
   
</div>
        
