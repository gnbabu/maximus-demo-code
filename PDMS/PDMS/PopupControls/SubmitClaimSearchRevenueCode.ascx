<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_SubmitClaimSearchRevenueCode" Codebehind="SubmitClaimSearchRevenueCode.ascx.cs" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%--<div>
    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="CheckEligibility" ShowSummary="true" />
</div>--%>

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">


<script type="text/javascript">
    function loaderSerachRevenue() {
       
        document.getElementById('<%= txtCode.ClientID %>').disabled = true;
        document.getElementById('<%= txtPlaceOfServiceName.ClientID %>').disabled = true;
        document.getElementById('<%= btnloading.ClientID %>').style.display = 'block'; 
       <%-- document.getElementById('<%= btnSearch.ClientID %>').style.display = 'none';--%>
    }
    function onlyNumbers(txt, event) {
        var charCode = (event.which) ? event.which : event.keyCode

        if (charCode < 48 || charCode > 57)
            return false;

        return true;
    }

    function GetRevenueDetails() {

        var txtRevenueCode = $("#<%= txtCode.ClientID %>").first().val();
        var txtRevenuecodedesc = $("#<%= txtPlaceOfServiceName.ClientID %>").first().val();
        $("#<%=gvSubmitClaimSearchRevPop.ClientID %>").html("");
        $('#<%=gvSubmitClaimSearchRevPop.ClientID %>').remove();
        var APIToken = $("[id*=hdnAccessToken]").val();
         $.ajax({
             type: "GET",
             url: webApiClaims + "GetRevenueDetails?desc=" + txtRevenuecodedesc + "&&val=" + txtRevenueCode,
             //data: '{desc: "' + txtRevenuecodedesc + '" , val: "' + txtRevenueCode  + '" }',
             headers: {
                 "Access-Control-Allow-Origin": "*",
                 "Access-Control-Allow-Methods": "POST, GET, PUT, DELETE, OPTIONS",
                 "Access-Control-Allow-Headers": "Content-Type, Authorization, X-Requested-With",
                 "Authorization": "Bearer " + APIToken
             },
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             success: function (result) {
                
                 if (result.length == 0) {

                     $("#<%=gvSubmitClaimSearchRevPop.ClientID %>").append("<tr><td> No Records Found </td></tr>");
                  }
                  else {
                     $("#<%=gvSubmitClaimSearchRevPop.ClientID %>").append("<tr><th>REVENUE CODE </th><th>REVENUE CODE DESCRIPTION </th></tr>");
                      for (var i = 0; i < result.length; i++) {
                          $("#<%=gvSubmitClaimSearchRevPop.ClientID %>").append("<tr><td><a onClick='GetSelectedRow(this); return false;'>" + result[i].Reven_Code + "</a></td><td>" + result[i].Reven_Desc + "</td></tr>");
                     };
                 }
             },
             error: function (jqXHR, textStatus, errorThrown) {
                 $("[id*=lblmessage]").text('Revenue code not found.');
             }
         });
        //return false;
    }

    function GetSelectedRow(lnk) {
        $find("mperRevenueCode").hide();

        var gridindex = localStorage.getItem("index");

        if (!(gridindex == "")) {

            var row = lnk.parentNode.parentNode;
            var grid = document.getElementById("<%= gvSubmitClaimSearchRevPop.ClientID%>");
             var inputs = grid.rows[gridindex].getElementsByTagName("INPUT");
             inputs[1].innerText = row.cells[0].innerText;
             grid.rows[gridindex].cells[4].innerText = row.cells[1].innerHTML;
             return false;
         }
         else {
             var textboxrow = lnk.parentNode.parentNode;
            $("[id*=txtInstRevenueCode]").val(textboxrow.cells[0].innerText.trim());
             return false;
         }
     }
    function LoaderFunction() {
        document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';
    }

</script>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="row" style="text-align: center; margin-left: 10px; margin-right: 10px;">
            <div class="row m-0 popUpSearch-Context d-FlexCenter" style="text-align: center;">
                <div class="col-sm-6 col-md-4 col-lg-3 ">
                    
                        <asp:TextBox ID="txtCode" onkeypress="return onlyNumbers(this,event);" MaxLength="4" CssClass="ohio-field-input" runat="server" AutoPostBack="false"></asp:TextBox>
                    
                </div>

                <div class="col-sm-6 col-md-4 col-lg-6 ">
                        <asp:TextBox ID="txtPlaceOfServiceName" MaxLength="100" CssClass="ohio-field-input" runat="server" AutoPostBack="false">
                        </asp:TextBox>

                </div>

                <div class="col-sm-6 col-md-4 col-lg-3 " style="padding-top: 12px;">
                    
                   
                     <asp:Button ID="btnSearch2" OnClientClick="return GetServiceDetailsInfo()" runat="server" CssClass="buttonBox StepButton buttonBoxFocus"  Text="Search" CausesValidation="false" />


                  
                </div>
            </div>
            
                     <button id="btnloading" runat="server" style="display:none;"  CssClass="buttonBox StepButton buttonBoxFocus" ><i class="fa fa-spinner fa-spin"></i>Loading</button>

        </div>
        <div class="row m-0 popUpSearch-Context" style="text-align: left; padding: 1px">

            <div class="col-sm-6 col-md-4 col-lg-6">
                <asp:Label runat="server" ID="lblRevenueError" Text="" ForeColor="Red"  />
            </div>
        </div>
        <div class="search-Results">SEARCH RESULTS</div>
        <asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="" Visible="false"></asp:Label>
        <asp:HiddenField ID="hdnRevenueCode" runat="server" />
        <div  class="result-Container" style="overflow-y: scroll;padding-left: 20px;padding-right: 20px;">
            <div class="popupGridViewOnSearch">

              <mms:SortablePagingGridView
                ID="gvSubmitClaimSearchRevPop"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="gridViewSmallFont" Width="100%"
                AllowSorting="true"
                EmptyDataText="No Providers found."
                  OnPageIndexChanging="gvSubmitClaimSearchRevPop_PageIndexChanging"
                OnSorting="gvSubmitClaimSearchRevPop_Sorting"
                RowStyle-VerticalAlign="Top"
                AllowPaging="True"
                PageSize="15"
                GridViewSortColumn="Code" GridViewSortDirection="Ascending"
                  DataKeyNames="RevenueCode, RevenueDesc">
                <Columns>

                    
                </Columns>
            </mms:SortablePagingGridView>

            </div>
         <%--   <mms:SortablePagingGridView
                ID="gvSubmitClaimSearchRevPop"
                runat="server"
                AllowSorting="false"
                AutoGenerateColumns="False"
                CssClass="gridViewSmallFont" Width="100%"
                EmptyDataText="No Revenue codes found."
                OnPageIndexChanging="gvSubmitClaimSearchRevPop_PageIndexChanging"
                OnSorting="gvSubmitClaimSearchRevPop_Sorting"
                RowStyle-VerticalAlign="Top"
                AllowPaging="True"
                CellSpacing="10"
                PageSize="15"
                GridViewSortColumn="Code" GridViewSortDirection="Ascending"
                DataKeyNames="RevenueCode, RevenueDesc" CurrentPageIndex="0" PageIndexCount="1">
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <Columns>
                    <asp:TemplateField HeaderText="Revenue Code" ItemStyle-Width="100" ItemStyle-Wrap="true">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkRevCode" runat="server" Text='<%# Eval("RevenueCode") %>' CommandArgument='<%# Eval("RevenueCode") %>' Font-Underline="false" OnClick="lnkRevCode_Click"   OnClientClick="LoaderFunction()">
                            </asp:LinkButton>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" Wrap="True" />
                    </asp:TemplateField>

                    <asp:BoundField DataField="RevenueDesc" HeaderText="Revenue Code Description" SortExpression="RevenueDesc" />

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
    </ContentTemplate>
</asp:UpdatePanel>

