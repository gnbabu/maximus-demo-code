<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_SearchValueCodeInformation" Codebehind="SearchValueCodeInformation.ascx.cs" %>


<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<%--<%@ Register Src="~/PopupControls/SearchValueCodeInformation.ascx" TagPrefix="uc" TagName="SearchValueCodeInformation" %>--%>

<div>
    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="List" ValidationGroup="CheckEligibility" ShowSummary="true" />
</div>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <div class="row" style="text-align: center; margin-left: 10px; margin-right: 10px;">
            <div class="row m-0 popUpSearch-Context d-FlexCenter">
                <div class="col-sm-6 col-md-4 col-lg-3 ">
                    <span class="ohio-field-label"><b>VALUE CODE</b>

                        <asp:TextBox ID="txtCode" CssClass="ohio-field-input" runat="server"></asp:TextBox>
                    </span>

                </div>

                <div class="col-sm-6 col-md-4 col-lg-6 ">
                    <span class="ohio-field-label"><b>VALUE CODE DESCRIPTION</b>
                        <asp:TextBox ID="txtValueDesc" CssClass="ohio-field-input" runat="server">
                        </asp:TextBox>
                    </span>

                </div>


                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="col-sm-6 col-md-4 col-lg-3 " style="padding-top: 12px;">
                            <asp:Button ID="btnSearch2" OnClientClick="showProgress()" runat="server" CssClass="buttonBox StepButton buttonBoxFocus" OnClick="btnSearch_Click2" Text="Search" CausesValidation="false" />



                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
        <asp:Label ID="lblSResult" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>
       
         <div class="search-Results">SEARCH RESULTS</div>
                <asp:Label ID="Label1" class="expandcollapse" runat="server" Text="Search Result" Visible="false"></asp:Label>

        <div class="result-Container">
            <mms:SortablePagingGridView
                ID="gvValueCodeSearchPop"
                runat="server"
                AutoGenerateColumns="False"
                CssClass="gridViewSmallFont" Width="100%"
                AllowSorting="true"
                EmptyDataText="No Providers found."
                OnPageIndexChanging="gvValueCodeSearchPop_PageIndexChanging"
                OnSorting="gvValueCodeSearchPop_Sorting"
                RowStyle-VerticalAlign="Top"
                AllowPaging="True"
                PageSize="15"
                GridViewSortColumn="Code" GridViewSortDirection="Ascending"
                DataKeyNames="CLAIMS_VALUE_CODE, CLAIMS_VALUE_CODE_DESC">
                <Columns>

                    <asp:TemplateField HeaderText="VALUE CODE" ItemStyle-Width="100" ItemStyle-Wrap="true">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkValueCodeSpan" runat="server" ToolTip="Search" Text='<%# Eval("CLAIMS_VALUE_CODE") %>'
                                CommandArgument='<%# Eval("CLAIMS_VALUE_CODE") %>' OnClick="lnkValueCodeSpan_Click" CausesValidation="false">
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CLAIMS_VALUE_CODE_DESC"   HeaderText="VALUE CODE DESCRIPTION" SortExpression="CLAIMS_VALUE_CODE_DESC" />
                </Columns>
            </mms:SortablePagingGridView>
        </div>
               <asp:HiddenField ID="hdnValueCode" runat="server" />

        <asp:HiddenField ID="hdnValueCodeDesc" runat="server" />


    </ContentTemplate>
</asp:UpdatePanel>
