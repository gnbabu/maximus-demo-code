<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_SubmitClaimSearchPop" Codebehind="SubmitClaimSearchPop.ascx.cs" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>


<script type="text/javascript">
    function onlyNumbers(txt, event) {
        var charCode = (event.which) ? event.which : event.keyCode

        if (charCode < 48 || charCode > 57)
            return false;

        return true;
    }
</script>


        <div> <asp:Label ID="lblSResultPlaceofService" class="failureNotification" runat="server" Text="" ></asp:Label></div>
        <div class="row m-0 popUpSearch-Context d-FlexCenter" style="text-align: left;">
             

            <div class="col-sm-6 col-md-4 col-lg-3 ">

                <asp:TextBox ID="txtCode" CssClass="ohio-field-input" runat="server" onkeypress="return onlyNumbers(this,event);" MaxLength="2"></asp:TextBox>


            </div>

            <div class="col-sm-6 col-md-4 col-lg-6 " style="height: 26px; width: 1294px">

                <asp:TextBox ID="txtPlaceOfServiceName" CssClass="ohio-field-input" runat="server" MaxLength="100" Width="452px"></asp:TextBox>


            </div>

            <div class="col-sm-6 col-md-4 col-lg-3 " style="padding-top: 10px;">
                <asp:Button ID="btnSearch2" OnClientClick="return GetPlaceOfServiceCodeInfo()" runat="server" CssClass="buttonBox StepButton buttonBoxFocus"  Text="Search" CausesValidation="false" />


            </div>


        </div>
        <div class="search-Results">SEARCH RESULTS</div>
      

        <div  class="result-Container" style="overflow-y: scroll;padding-left: 20px;padding-right: 20px;">
             <div class="popupGridViewOnSearch">
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
                  DataKeyNames="PRIOR_AUTH_PLACE_OF_SERVICE_MMIS, PRIOR_AUTH_PLACE_OF_SERVICE_DESC">
                <Columns>

                    
                </Columns>
            </mms:SortablePagingGridView>
                 </div>
         <%--   <mms:SortablePagingGridView
                ID="gvSubmitClaimSearchPop"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="gridViewSmallFont" Width="100%"
                AllowSorting="false"
                EmptyDataText="No Place Of Service found."
                OnPageIndexChanging="gvSubmitClaimSearchPop_PageIndexChanging"
                OnSorting="gvSubmitClaimSearchPop_Sorting"
                RowStyle-VerticalAlign="Top"
                AllowPaging="True"
                PageSize="15"
                GridViewSortColumn="Code" GridViewSortDirection="Ascending"
                DataKeyNames="PRIOR_AUTH_PLACE_OF_SERVICE_MMIS, PRIOR_AUTH_PLACE_OF_SERVICE_DESC">
                <Columns>
                    <asp:TemplateField HeaderText="Code" ItemStyle-Width="100" ItemStyle-Wrap="true">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkCode" runat="server" Text='<%# Eval("PRIOR_AUTH_PLACE_OF_SERVICE_MMIS") %>' CausesValidation="false" 
                                CommandArgument='<%# Eval("PRIOR_AUTH_PLACE_OF_SERVICE_MMIS") %>' Font-Underline="false" OnClick="lnkCode_Click">
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PRIOR_AUTH_PLACE_OF_SERVICE_DESC" HeaderText="Place of Service Name" SortExpression="PRIOR_AUTH_PLACE_OF_SERVICE_DESC" />

                </Columns>
            </mms:SortablePagingGridView>--%>
        </div>
   