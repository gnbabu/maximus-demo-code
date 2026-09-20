<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_SearchOccurrenceSpan" Codebehind="SearchOccurrenceSpan.ascx.cs" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>


<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<link href="../Content/custom-style.css" rel="stylesheet" />


<script type="text/javascript">  
    function loadsearchOccuranceSpan() {
        document.getElementById('<%= btnSearch.ClientID %>').style.display = 'none';        
        document.getElementById('<%= txtCode.ClientID %>').disabled = true;
        document.getElementById('<%= txtCodeDescription.ClientID %>').disabled = true;
    }
</script>

     <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">

   
<%--<div>
    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="CheckEligibility" ShowSummary="true" />
</div>--%>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div style="background-color: #e7fff3">
            <asp:Label ID="lblSResult" class="failureNotification" runat="server" Text="Search Result" ForeColor="Red"></asp:Label>
            <div class="row m-0 popUpSearch-Context d-FlexCenter" >
                
               <%-- <div class="row" style="text-align: center;">--%>

                    <div class="col-sm-6 col-md-4 col-lg-2">
                         <span class="ohio-field-label" style="padding-left:10px;font-size:17px;" >
                            <asp:TextBox ID="txtCode" CssClass="ohio-field-input" runat="server" MaxLength="2"></asp:TextBox>
                        </span>                       

                    </div>
                  
                 <div class="col-sm-6 col-md-4 col-lg-8 ">
                        <span class="ohio-field-label">
                            <asp:TextBox ID="txtCodeDescription" CssClass="ohio-field-input" runat="server">

                            </asp:TextBox>
                        </span>
                    </div>
                    <div class="col-sm-2">
                         <div>
                             <%--<asp:Button ID="btnSearch" runat="server"  ValidationGroup="valSearchOccurrence" CausesValidation="false" Text="Search" CssClass="buttonBoxFocus"  OnClientClick="return GetOccurenceSpanDetails()" Style="background-color: darkslateblue !important" />--%>
                              <asp:Button ID="btnSearch" runat="server" CausesValidation="false" Text="Search" CssClass="buttonBox StepButton buttonBoxFocus"  ValidationGroup="valSearchOccurrence" OnClientClick="return GetOccurenceSpanDetails()"  />
                    
                         </div>
                    </div>
                <%--</div>--%>
            </div>
            <div class="search-Results">SEARCH RESULTS</div>
           
                <div class="result-Container">

                     <div class="popupGridViewOnSearch">
                  <mms:SortablePagingGridView
                ID="gvOccurenceCodeSpanSearchPop"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="gridViewSmallFont" Width="100%"
                AllowSorting="true"
                EmptyDataText="No Providers found."
                OnPageIndexChanging="gvOccurenceCodeSpanSearchPop_PageIndexChanging"
                OnSorting="gvOccurenceCodeSpanSearchPop_Sorting"
                RowStyle-VerticalAlign="Top"
                AllowPaging="True"
                PageSize="15"
                GridViewSortColumn="Code" GridViewSortDirection="Ascending"
                 DataKeyNames="CLAIMS_OCCURRENCE_CODE, CLAIMS_OCCURRENCE_CODE_DESC">
                <Columns>

                    
                </Columns>
            </mms:SortablePagingGridView>
               </div>
            </div>

            <asp:HiddenField ID="hdnOccurrenceSpanCode" runat="server" />

            <asp:HiddenField ID="hdnOccurrenceSpanCodeDec" runat="server" />
            <asp:HiddenField ID="hdnStatus" runat="server"/>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
