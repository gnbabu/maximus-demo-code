<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_MemberEligibilityTesting, App_Web_c4une0e1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<style type="text/css">
    select {
        min-width: 90%;
    }
</style>

<br />
<div style="border-top: 2px solid black; margin-bottom: -10px">
    <br />
</div>

<asp:Panel ID="pnlMemberEligibilityTesting" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div id="div1" runat="server" class="test-left" style="text-align: left;">
        <span class="pdspnlMemberEligibilityTesting" id="Span1" runat="server"><b>MemberEligibility Search Transaction</b></span>
        <hr />
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblMemberEligibilityTransactionType" class="ohio-select" AssociatedControlID="ddlMemberEligibilityTransactionType" runat="server">
                    <span class="ohio-select-label">Transaction Type:<span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover"
                        data-placement="right" data-content="Enter the Transaction Type"
                        aria-hidden="true"></span>
                    </span>
                    <span class="ohio-select-select fa">
                        <asp:DropDownList ID="ddlMemberEligibilityTransactionType" CssClass="ohio-select-select-el" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlMemberEligibilityTransactionType_SelectedIndexChanged">
                            <asp:ListItem Enabled="true" Text="Select Transaction Type" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Search MemberEligibility" Value="SearchEligibility"></asp:ListItem>
                            <asp:ListItem Text="Prior Auth MemberEligibility" Value="PriorAuthEligibility"></asp:ListItem>
                            <asp:ListItem Text="Claims MemberEligibility" Value="ClaimEligibility"></asp:ListItem>
                            <asp:ListItem Text="Hospice MemberEligibility" Value="HospiceEligibility"></asp:ListItem>
                        </asp:DropDownList>
                    </span>
                </asp:Label>
            </div>
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblMemberEligibilityTXNQID" CssClass="ohio-field" AssociatedControlID="txtMemberEligibilityTXNQID" runat="server">
                    <span class="ohio-field-label">SI Transaction Key: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Enter the SI Transaction Key" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtMemberEligibilityTXNQID" CssClass="ohio-field-input" runat="server" />
                </asp:Label>
            </div>
        </div>
    </div>
    <div id="divPFSearchHeader_id" class="RetrieveReportsSearchHeader" runat="server" visible="false">MemberEligibility Transaction Search Results</div>
    <asp:GridView
        ID="gvMemberEligibilityTransaction"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="gridViewSmallFont" Width="100%"
        AllowSorting="true"
        EmptyDataText="No Records Found"
        RowStyle-VerticalAlign="Top"
        AllowPaging="true"
        AllowCustomPaging="true"
        PageSize="5"
        GridViewSortDirection="Descending"
        PageIndexChanged="gvMemberEligibilityTransaction_PageIndexChanged"
        OnPageIndexChanging="gvMemberEligibilityTransaction_PageIndexChanging"
        DataKeyNames="OUTBOUND_MEMBER_ELIGIBILITY_SERVICE_REQ_RES_ID">
        <Columns>
            <asp:BoundField DataField="OUTBOUND_MEMBER_ELIGIBILITY_SERVICE_REQ_RES_ID" HeaderText="OUTBOUND MEMBER ELIGIBILITY REQ RES ID"  />
            <asp:BoundField DataField="REQUEST_TYPE" HeaderText="REQUEST TYPE" />
            <asp:BoundField DataField="SI_RESPONSE_CODE" HeaderText="RESPONSE CODE" />
            <asp:BoundField DataField="SI_RESPONSE_TYPE" HeaderText="RESPONSE TYPE" />
            <asp:BoundField DataField="SI_TRANSACTION_KEY" HeaderText="SI TRANSACTION KEY" />
            <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" HeaderText="CREATE DATE" />
        </Columns>
    </asp:GridView>
    <asp:HiddenField ID="hdnRowCount" runat="server" />
</asp:Panel>
<asp:Panel ID="Panel1" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div class="container-fluid">
        <div class="row">

            <div class="col-sm-6 col-md-4 col-lg-6 outerName">
                <asp:Label ID="lblMemberEligibilityGenXML" CssClass="ohio-field" AssociatedControlID="txtMemberEligibilityGenXML" runat="server">
                    <span class="ohio-field-label">Generated XML: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Generated XML for that transaction ID" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtMemberEligibilityGenXML" CssClass="ohio-field-input" runat="server" TextMode="MultiLine" />
                </asp:Label>
            </div>
            <div class="col-sm-6 col-md-4 col-lg-6 outerName">
                <asp:Label ID="lblMemberEligibilityResponse" CssClass="ohio-field" AssociatedControlID="txtMemberEligibilityResponse" runat="server">
                    <span class="ohio-field-label">SI Response: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Here you wil see Response From SI when Make Request is clicked" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtMemberEligibilityResponse" CssClass="ohio-field-input" runat="server" TextMode="MultiLine" />
                </asp:Label>
            </div>
        </div>
        <div class="row" style="padding-left: 1in">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <br />
                    <div class="row">
                        <asp:Button ID="btnGenXMLMemberEligibility" runat="server" Text="Generate MemberEligibility XML" OnClick="btnGenXMLMemberEligibility_Click" CssClass="buttonBoxFocus" />
                        <asp:Button ID="btnMakeMemberEligibility" runat="server" Text="Make MemberEligibility Call" OnClick="btnMakeMemberEligibility_Click" CssClass="buttonBoxFocus" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnGenXMLMemberEligibility" />
                    <asp:PostBackTrigger ControlID="btnMakeMemberEligibility" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Panel>
