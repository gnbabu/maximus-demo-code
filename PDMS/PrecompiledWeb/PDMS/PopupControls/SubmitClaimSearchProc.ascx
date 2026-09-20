<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SubmitClaimSearchProc, App_Web_l5y5araq" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">


<script type="text/javascript">
    function loaderSerachProc() {
       
        document.getElementById('<%= txtCode.ClientID %>').disabled = true;
        document.getElementById('<%= txtPlaceOfServiceName.ClientID %>').disabled = true;
        document.getElementById('<%= btnloading1.ClientID %>').style.display = 'block'; 
        document.getElementById('<%= btnSearch1.ClientID %>').style.display = 'none';
    }
</script>
   <div class="row m-0 popUpSearch-Context d-FlexCenter" style="text-align: center;">
        <div class="col-sm-6 col-md-4 col-lg-3 ">
           
                <asp:TextBox ID="txtCode" CssClass="ohio-field-input" runat="server" MaxLength="5"></asp:TextBox>
                 <asp:Label ID="lbltxtCodeError" runat="server" ForeColor="Red"></asp:Label>
        </div>

        <div class="col-sm-6 col-md-4 col-lg-6 ">
                <asp:TextBox ID="txtPlaceOfServiceName" CssClass="ohio-field-input" runat="server" MaxLength="100"></asp:TextBox>
           
        </div>

        <div class="col-sm-6 col-md-4 col-lg-3 " style="padding-top:12px;">
             <button id="btnloading1" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>
                    <asp:Button ID="btnSearch1" OnClientClick="return GetProcedureCodeInfo()" runat="server" CssClass="buttonBox StepButton buttonBoxFocus"  Text="Search" CausesValidation="false" />
  
        </div>
    </div>

<%--</div>--%>

        <div class="row m-0 popUpSearch-Context" style="text-align: left; padding: 1px">
                
                <div class="col-sm-6 col-md-4 col-lg-6">
                    <asp:Label runat="server" ID="lblError" Text="" ForeColor="Red" Visible="false" />
                </div>
            </div>
<div class="search-Results">SEARCH RESULTS</div>
                <asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="" Visible="false"></asp:Label>
        <asp:HiddenField ID="hdnProcedureCode" runat="server" />
        <div  class="result-Container" style="overflow-y: scroll;padding-left: 20px;padding-right: 20px;">
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
                 DataKeyNames="PRIOR_AUTH_PROCEDURE_CODE_MMIS,PRIOR_AUTH_PROCEDURE_CODE_DESC">
                <Columns>

                    
                </Columns>
            </mms:SortablePagingGridView>
                </div>
<%--<mms:SortablePagingGridView
    ID="gvSubmitClaimSearchProcPop"
    runat="server"
    AllowSorting="false"
    AutoGenerateColumns="False"
    CssClass="gridViewSmallFont" Width="100%"
    EmptyDataText="No Procedure Code found."
    OnPageIndexChanging="gvSubmitClaimSearchPop_PageIndexChanging"
    OnSorting="gvSubmitClaimSearchPop_Sorting"
    RowStyle-VerticalAlign="Top"
    AllowPaging="True"
      CellSpacing="10"
    PageSize="15"
    GridViewSortColumn="Code" GridViewSortDirection="Ascending"
    DataKeyNames="PRIOR_AUTH_PROCEDURE_CODE_MMIS,PRIOR_AUTH_PROCEDURE_CODE_DESC" CurrentPageIndex="0" PageIndexCount="1">
    <AlternatingRowStyle CssClass="gridViewAltRow" />
    <Columns>
         <asp:TemplateField HeaderText="Procedure Code" ItemStyle-Width="100" ItemStyle-Wrap="true">
            <ItemTemplate>
                   <asp:LinkButton OnClientClick="loaderSerachProc()" ID="lnkProcCode" runat="server" Text='<%# Eval("PRIOR_AUTH_PROCEDURE_CODE_MMIS") %>' CausesValidation="false"
                       CommandArgument='<%# Eval("PRIOR_AUTH_PROCEDURE_CODE_MMIS") %>' Font-Underline="false"  OnClick ="lnkProcCode_Click">
                   </asp:LinkButton>
            </ItemTemplate>
             <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
             <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" Wrap="True" />
        </asp:TemplateField>
        <asp:BoundField DataField="PRIOR_AUTH_PROCEDURE_CODE_DESC" HeaderText="Procedure Code Description" SortExpression="PRIOR_AUTH_PROCEDURE_CODE_DESC" />
       
    </Columns>
    <EmptyDataRowStyle CssClass="gridViewEmptyRow" />
    <HeaderStyle CssClass="gridViewHeader" />
    <PagerSettings Mode="NumericFirstLast" />
    <PagerStyle CssClass="gridViewPager" />
    <RowStyle CssClass="gridViewRow" VerticalAlign="Top" />
    <SelectedRowStyle BackColor="Yellow" Font-Bold="False" />
    <SortedAscendingHeaderStyle CssClass="sort-asc" />
    <SortedDescendingHeaderStyle CssClass="sort-desc" />
</mms:SortablePagingGridView>--%>
</div>
<%-- </ContentTemplate>
</asp:UpdatePanel>--%>