<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_TypeOfBill" Codebehind="TypeOfBill.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">
<script type="text/javascript">
    function loaderTypeOfBill() {

        document.getElementById('<%= txtSearchTOBCode.ClientID %>').disabled = true;
        document.getElementById('<%= txtSearchTOBDesc.ClientID %>').disabled = true;
        document.getElementById('<%= Button30.ClientID %>').style.display = 'none';
        document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';

    }
</script>
<div class="row" style="text-align: center; margin-left: 10px; margin-right: 10px;">
    <div class="row m-0 popUpSearch-Context d-FlexCenter" style="text-align: left;">
        <div class="col-sm-3" style="width: 362px">
           
            <asp:TextBox ID="txtSearchTOBCode" CssClass="ohio-field-input" runat="server" Width="90px" MaxLength="4" onKeydown="return (!(event.keyCode>=65 && event.keyCode <=90) && event.keyCode!=32);"></asp:TextBox>
           
        </div>

        <div class="col-sm-7" style="width: 992px">
            <%--<span class="ohio-field-label"><b>&nbsp;Type of Bill Description</b>--%>
            <asp:TextBox ID="txtSearchTOBDesc" CssClass="ohio-field-input" runat="server" Width="369px" MaxLength="100"></asp:TextBox>
            <%--</span>--%>
        </div>

        <div class="col-sm-6 col-md-4 col-lg-3 " style="padding-top: 12px;">
             <button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                     
           
            
   <asp:Button ID="Button30" OnClientClick="return GetTypeOfBillInfo()" runat="server" CssClass="buttonBox StepButton buttonBoxFocus"  Text="Search" CausesValidation="false" />


        </div>
    </div>

</div>
<div class="row m-0 popUpSearch-Context" style="text-align: left; padding: 1px">

                <div class="col-sm-6 col-md-4 col-lg-6">
                    <asp:Label runat="server" ID="lblError" Text="" ForeColor="Red" Visible="false" />
                </div>
    </div>
    

<asp:Label ID="Label17" class="expandcollapse" runat="server" Text="Search Result"
    Visible="false"></asp:Label><div class="search-Results">SEARCH RESULTS</div>
<asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="" Visible="false"></asp:Label>
<div class="result-Container">
      <mms:SortablePagingGridView
                ID="gvSubmitClaimSearchPop"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="gridViewSmallFont" Width="100%"
                AllowSorting="true"
                EmptyDataText="No Providers found."
                OnPageIndexChanging="gvSubmitClaimSearchPop_PageIndexChanging"
               
                RowStyle-VerticalAlign="Top"
                AllowPaging="True"
                PageSize="15"
                GridViewSortColumn="Code" GridViewSortDirection="Ascending"
                 DataKeyNames="CLAIMS_TYPE_OF_BILL_Code,CLAIMS_TYPE_OF_BILL_DESC">
                <Columns>

                    
                </Columns>
            </mms:SortablePagingGridView>
   <%-- <asp:GridView
        ID="gvSubmitClaimSearchPop"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="gridViewSmallFont"
        AllowSorting="false"
        EmptyDataText="No Type of Bill found."
        RowStyle-VerticalAlign="Top"
        HeaderStyle-BackColor="#b7d5e9"
        AllowPaging="true"
        GridViewSortColumn="TOBCode"
        PageSize="10"
        GridViewSortDirection="Ascending"
        OnPageIndexChanging="gvSubmitClaimSearchPop_PageIndexChanging"
        DataKeyNames="CLAIMS_TYPE_OF_BILL_Code,CLAIMS_TYPE_OF_BILL_DESC" HeaderStyle-HorizontalAlign="Center"  Width="100%">
        <PagerSettings Mode="Numeric"  /> 
        <Columns>

            <asp:TemplateField HeaderText="Type of Bill" ItemStyle-Wrap="true" ItemStyle-Width="100px">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkTOB" runat="server" ToolTip="Search" Text='<%# Eval("CLAIMS_TYPE_OF_BILL_Code") %>' Font-Underline="false" 
                      OnClientClick="loaderTypeOfBill()"  CommandArgument='<%# Eval("CLAIMS_TYPE_OF_BILL_Code") %>' OnClick="lnkTOB_Click" CausesValidation="False"></asp:LinkButton> 
                </ItemTemplate>
            

                <ItemStyle Wrap="True" Width="100px"></ItemStyle>
            </asp:TemplateField>
            <asp:BoundField DataField="CLAIMS_TYPE_OF_BILL_DESC" HeaderText="Type of Bill Description" HeaderStyle-ForeColor="Black" SortExpression="CLAIMS_TYPE_OF_BILL_DESC"
                ItemStyle-Width="400px">

<HeaderStyle ForeColor="Black"></HeaderStyle>

                <ItemStyle Width="400px"></ItemStyle>
            </asp:BoundField>

        </Columns>
        
        
    </asp:GridView>--%>
</div>
