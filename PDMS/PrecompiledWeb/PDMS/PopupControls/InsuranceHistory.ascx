<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_InsuranceHistory, App_Web_guw1elnn" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/WorkHistory.ascx" TagPrefix="uc" TagName="WorkHistory" %>
<%@ Register Src="~/UserControls/Address.ascx" TagName="Address" TagPrefix="uc" %>

<div id="pnlWorkHistoryList" runat="server">
  <div class="divGrid">
        <asp:GridView runat="server" ID="grdInsurance" AllowPaging="true" PageSize="10" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No records found" OnPageIndexChanging="grdInsuranceHistory_PageIndexChanging" OnSorting="grdInsuranceHistory_Sorting">
            <Columns>
                <asp:BoundField DataField="isMalpracticeClaimed"           HeaderText="Carrying malpractice insurance?"                    SortExpression="isMalpracticeClaimed" />
                <asp:BoundField DataField="IS_SELF_INSURED"                    HeaderText="Self Insured?"                                      SortExpression="IS_SELF_INSURED" />
                <asp:BoundField DataField="POLICY_NUMBER"                   HeaderText="Policy Number"                                      SortExpression="POLICY_NUMBER" />
                <asp:BoundField DataField="EFFECTIVE_DATE"                  HeaderText="Eff Date" DataFormatString="{0:MM/dd/yyyy}"         SortExpression="EFFECTIVE_DATE" />
                <asp:BoundField DataField="ORIGINAL_EFFECTIVE_DATE"                   HeaderText="Orig Eff Date" DataFormatString="{0:MM/dd/yyyy}"    SortExpression="ORIGINAL_EFFECTIVE_DATE"  />
                <asp:BoundField DataField="EXPIRATION_DATE"                 HeaderText="Expiration Date" DataFormatString="{0:MM/dd/yyyy}"  SortExpression="EXPIRATION_DATE" />
                <asp:BoundField DataField="TYPE_OF_COVERAGE_NAME"                   HeaderText="Type of Coverage"                                   SortExpression="TYPE_OF_COVERAGE_NAME" />
                <asp:BoundField DataField="IS_UNLIMITED_COVERAGE"           HeaderText="Do you have unlimited coverage?"                    SortExpression="IS_UNLIMITED_COVERAGE" />
                <asp:BoundField DataField="TAIL_NOSE_COVERAGE"                    HeaderText="Policy includes tail coverage?"                     SortExpression="TAIL_NOSE_COVERAGE" />
                <asp:BoundField DataField="CarrierName"                    HeaderText="Carrier or Self-Insured Name"                       SortExpression="CarrierName" />
                <asp:BoundField DataField="CarrierAddress"                 HeaderText="Carrier Address"                                    SortExpression="CarrierAddress" />
                <asp:BoundField DataField="PolicyHolder"                    HeaderText="Policy Holder"                                      SortExpression="PolicyHolder" />
                <asp:BoundField DataField="CoverageAmountPerOccurance"      HeaderText="Coverage Account Per Occurence"                     SortExpression="CoverageAmountPerOccurance" />
                <asp:BoundField DataField="CoverageAmountPerAggregate"      HeaderText="Coverage Account Per Aggregate"                     SortExpression="CoverageAmountPerAggregate" />
                <asp:BoundField DataField="MalpracticeInsuranceReason"      HeaderText="Explanation regarding malpractice insurance"        SortExpression="MalpracticeInsuranceReason" />
                <asp:BoundField DataField="UserName"                        HeaderText="User Name"                                          SortExpression="UserName" />
                <asp:BoundField DataField="DateOfAction"                    HeaderText="Update Date"                                        SortExpression="DateOfAction"   DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False"  />
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
</div>
