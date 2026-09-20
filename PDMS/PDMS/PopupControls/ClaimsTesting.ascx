<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ClaimsTesting" Codebehind="ClaimsTesting.ascx.cs" %>
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

<asp:Panel ID="pnlClaimsTesting" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div id="div1" runat="server" class="test-left" style="text-align: left;">
        <span class="pdspnlClaimsTesting" id="Span1" runat="server"><b>Claims Search Transaction</b></span>
        <hr />
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblClaimsTransactionType" class="ohio-select" AssociatedControlID="ddlClaimsTransactionType" runat="server">
                    <span class="ohio-select-label">Transaction Type:<span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover"
                        data-placement="right" data-content="Enter the Transaction Type"
                        aria-hidden="true"></span>
                    </span>
                    <span class="ohio-select-select fa">
                        <asp:DropDownList ID="ddlClaimsTransactionType" CssClass="ohio-select-select-el" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlClaimsTransactionType_SelectedIndexChanged">
                            <asp:ListItem Enabled="true" Text="Select Transaction Type" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Add/Update Claims" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Inquiry Claims" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Search Claims" Value="3"></asp:ListItem>
                        </asp:DropDownList>
                    </span>
                </asp:Label>
            </div>
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblClaimsTXNQID" CssClass="ohio-field" AssociatedControlID="txtClaimsTXNQID" runat="server">
                    <span class="ohio-field-label">SI Transaction Key: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Enter the SI Transaction Key" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtClaimsTXNQID" CssClass="ohio-field-input" runat="server" />
                </asp:Label>
            </div>
        </div>
    </div>
    <div id="divPFSearchHeader_id" class="RetrieveReportsSearchHeader" runat="server" visible="false">Claims Transaction Search Results</div>
    <asp:GridView
        ID="gvClaimsTransaction"
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
        PageIndexChanged="gvClaimsTransaction_PageIndexChanged"
        OnPageIndexChanging="gvClaimsTransaction_PageIndexChanging"
        DataKeyNames="PASSTHROUGH_TRANSACTIONQUEUE_ID">
        <Columns>
            <asp:BoundField DataField="PASSTHROUGH_TRANSACTIONQUEUE_ID" HeaderText="PASSTHROUGH TRANSACTIONQUEUE ID"  />
            <asp:BoundField DataField="PASSTHROUGH_REQUEST_TYPE" HeaderText="PASSTHROUGH REQUEST TYPE" />
            <asp:BoundField DataField="RESPONSE_CODE" HeaderText="RESPONSE CODE" />
            <asp:BoundField DataField="RESPONSE_TYPE" HeaderText="RESPONSE TYPE" />
            <asp:BoundField DataField="RESPONSE_MESSAGE" HeaderText="RESPONSE MESSAGE" />
            <asp:BoundField DataField="RESPONSE_DETAILS" HeaderText="RESPONSE DETAILS" />
            <asp:BoundField DataField="CREATED_DATE_TIME" HeaderText="CREATE DATE" />
        </Columns>
    </asp:GridView>
    <asp:HiddenField ID="hdnRowCount" runat="server" />
</asp:Panel>
<asp:Panel ID="Panel1" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div class="container-fluid">
        <div class="row">

            <div class="col-sm-6 col-md-4 col-lg-6 outerName">
                <asp:Label ID="lblClaimsGenXML" CssClass="ohio-field" AssociatedControlID="txtClaimsGenXML" runat="server">
                    <span class="ohio-field-label">Generated XML: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Generated XML for that transaction ID" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtClaimsGenXML" CssClass="ohio-field-input" runat="server" TextMode="MultiLine" />
                </asp:Label>
            </div>
            <div class="col-sm-6 col-md-4 col-lg-6 outerName">
                <asp:Label ID="lblClaimsResponse" CssClass="ohio-field" AssociatedControlID="txtClaimsResponse" runat="server">
                    <span class="ohio-field-label">SI Response: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Here you wil see Response From SI when Make Request is clicked" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtClaimsResponse" CssClass="ohio-field-input" runat="server" TextMode="MultiLine" />
                </asp:Label>
            </div>
        </div>
        <div class="row" style="padding-left: 1in">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <br />
                    <div class="row">
                        <asp:Button ID="btnGenXMLClaims" runat="server" Text="Generate Claims XML" OnClick="btnGenXMLClaims_Click" CssClass="buttonBoxFocus" /> 
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnGenXMLClaims" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Panel>
