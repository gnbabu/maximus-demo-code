<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ConditionCodeSearch, App_Web_wenzyumt" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
 <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
<meta name="viewport" content="width=device-width, initial-scale=1">
<%--<div>
    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="CheckEligibility" ShowSummary="true" />
</div>--%>
<script type="text/javascript">
 function loadConditionCodeInformation() {
        document.getElementById('<%= btnSearch.ClientID %>').style.display = 'none';
    <%-- document.getElementById('<%= btnloading.ClientID %>').style.display = 'block';--%>
     document.getElementById('<%= txtConditonCode.ClientID %>').disabled = true;
     document.getElementById('<%= txtConditionCodeDesc.ClientID %>').disabled = true;
    }
</script>
<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div style="background-color: #e7fff3">
             <asp:Label ID="fieldRequireError" runat="server" ForeColor="Red" class="failureNotification" ></asp:Label>   
            <div class="row m-0 popUpSearch-Context d-FlexCenter">
                
                <div class="col-sm-6 col-md-4 col-lg-3 ">
                     <span class="ohio-field-label" style="padding-left:10px;font-size:17px;" >
                    <asp:TextBox ID="txtConditonCode" CssClass="ohio-field-input" runat="server" MaxLength="2"></asp:TextBox>
                          </span>         
                </div>

                <div class="col-sm-6 col-md-4 col-lg-8 ">
                    <span class="ohio-field-label" style="padding-left:10px;font-size:17px;" >
                    <asp:TextBox ID="txtConditionCodeDesc" CssClass="ohio-field-input" runat="server" MaxLength="100">
                    </asp:TextBox>
                         </span>       
                </div>

                <div class="col-sm-2">
                               
                    <asp:Button ID="btnSearch" runat="server" CausesValidation="false" Text="Search" CssClass="buttonBox StepButton buttonBoxFocus"  ValidationGroup="ProviderSearch" OnClientClick="return GetConditionCodeDetails()"  />
                    
                </div>
            </div>

        </div>
        <div class="search-Results">SEARCH RESULTS</div>
        <%--<asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>--%>
        <div class="result-Container" style="overflow-y: scroll;">
             <div class="popupGridViewOnSearch">
              <mms:SortablePagingGridView
                ID="gvConditionCodeSearch"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="gridViewSmallFont" Width="100%"
                AllowSorting="true"
                EmptyDataText="No Providers found."
                OnPageIndexChanging="gvConditionCodeSearch_PageIndexChanging"
                OnSorting="gvConditionCodeSearch_Sorting"
                RowStyle-VerticalAlign="Top"
                AllowPaging="True"
                PageSize="15"
                GridViewSortColumn="Code" GridViewSortDirection="Ascending"
                 DataKeyNames="CLAIMS_CONDITION_CODE, CLAIMS_CONDITION_CODE_DESCRIPTION">
                <Columns>

                    
                </Columns>
            </mms:SortablePagingGridView>
         </div>
        </div>
        <asp:HiddenField ID="hdnCondition_Code" runat="server" />
        <asp:HiddenField ID="hdnCondition_Code_Description" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
