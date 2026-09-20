<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_PriorAuthTesting" Codebehind="PriorAuthTesting.ascx.cs" %>
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

<asp:Panel ID="pnlPATesting" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div id="div1" runat="server" class="test-left" style="text-align: left;">
        <span class="pdspnlPATesting" id="Span1" runat="server"><b>Prior-Authorization Search Transaction</b></span>
        <hr />
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblTransactionType" class="ohio-select" AssociatedControlID="ddlTransactionType" runat="server">
                    <span class="ohio-select-label">Transaction Type:<span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover"
                        data-placement="right" data-content="Enter the Transaction Type"
                        aria-hidden="true"></span>
                    </span>
                    <span class="ohio-select-select fa">
                        <asp:DropDownList ID="ddlTransactionType" CssClass="ohio-select-select-el" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlTransactionType_SelectedIndexChanged">
                            <asp:ListItem Enabled="true" Text="Select Transaction Type" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Add/Update PA" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Inquiry PA" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Search PA" Value="6"></asp:ListItem>
                        </asp:DropDownList>
                    </span>
                </asp:Label>
            </div>
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblTXNQID" CssClass="ohio-field" AssociatedControlID="txtTXNQID" runat="server">
                    <span class="ohio-field-label">Transaction ID: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Enter the Transaction ID" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtTXNQID" CssClass="ohio-field-input" runat="server" />
                </asp:Label>
            </div>
        </div>
    </div>
    <div id="divRRSearchHeader_id" class="RetrieveReportsSearchHeader" runat="server" visible="false">PassThrough Transaction Search Results</div>
    <asp:GridView
        ID="gvQueryPassThroughTransaction"
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
        PageIndexChanged="gvQueryPassThroughTransaction_PageIndexChanged"
        OnPageIndexChanging="gvQueryPassThroughTransaction_PageIndexChanging"
        DataKeyNames="PASSTHROUGH_TRANSACTIONQUEUE_ID">
        <Columns>
            <asp:BoundField DataField="PASSTHROUGH_TRANSACTIONQUEUE_ID" HeaderText="PASSTHROUGH TRANSACTIONQUEUE ID" />
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
                <asp:Label ID="lblGenXML" CssClass="ohio-field" AssociatedControlID="txtGenXML" runat="server">
                    <span class="ohio-field-label">Generated XML: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Generated XML for that transaction ID" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtGenXML" CssClass="ohio-field-input" runat="server" TextMode="MultiLine" />
                </asp:Label>
            </div>
            <div class="col-sm-6 col-md-4 col-lg-6 outerName">
                <asp:Label ID="lblResponse" CssClass="ohio-field" AssociatedControlID="txtResponse" runat="server">
                    <span class="ohio-field-label">SI Response: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Here you wil see Response From SI when Make Request is clicked" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtResponse" CssClass="ohio-field-input" runat="server" TextMode="MultiLine" />
                </asp:Label>
            </div>
        </div>
        <div class="row" style="padding-left: 1in">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <br />
                    <div class="row">
                        <asp:Button ID="btnGenXMLPriorAuthAU" runat="server" Text="Generate PriorAuth XML" OnClick="btnGenXMLPriorAuthAU_Click" CssClass="buttonBoxFocus" />
                        <asp:Button ID="btnCreateAuth" runat="server" Text="Make AddUpdate PriorAuth Request" OnClick="btnCreateAuth_Click" CssClass="buttonBox" />
                        <asp:Button ID="btnInquireAuth" runat="server" Text="Make Inquire PriorAuth Request" OnClick="btnInquireAuth_Click" CssClass="buttonBox" />
                        <asp:Button ID="btnSearchAuth" runat="server" Text="Make Search PriorAuth Request" OnClick="btnSearchAuth_Click" CssClass="buttonBox" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnGenXMLPriorAuthAU" />
                    <asp:PostBackTrigger ControlID="btnCreateAuth" />
                    <asp:PostBackTrigger ControlID="btnInquireAuth" />
                    <asp:PostBackTrigger ControlID="btnSearchAuth" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Panel>
