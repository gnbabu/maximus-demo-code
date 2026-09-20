<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ProviderFinancialTesting, App_Web_rqhgepvh" %>
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

<asp:Panel ID="pnlPFTesting" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div id="div1" runat="server" class="test-left" style="text-align: left;">
        <span class="pdspnlPFTesting" id="Span1" runat="server"><b>Provider Financial Search Transaction</b></span>
        <hr />
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblPFTransactionType" class="ohio-select" AssociatedControlID="ddlPFTransactionType" runat="server">
                    <span class="ohio-select-label">Transaction Type:<span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover"
                        data-placement="right" data-content="Enter the Transaction Type"
                        aria-hidden="true"></span>
                    </span>
                    <span class="ohio-select-select fa">
                        <asp:DropDownList ID="ddlPFTransactionType" CssClass="ohio-select-select-el" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlPFTransactionType_SelectedIndexChanged">
                            <asp:ListItem Enabled="true" Text="Select Transaction Type" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="1099Inquire" Value="1099Inquire"></asp:ListItem>
                            <asp:ListItem Text="1099History" Value="1099History"></asp:ListItem>
                        </asp:DropDownList>
                    </span>
                </asp:Label>
            </div>
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblPFTXNQID" CssClass="ohio-field" AssociatedControlID="txtPFTXNQID" runat="server">
                    <span class="ohio-field-label">SI Transaction Key: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Enter the SI Transaction Key" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtPFTXNQID" CssClass="ohio-field-input" runat="server" />
                </asp:Label>
            </div>
        </div>
    </div>
    <div id="divPFSearchHeader_id" class="RetrieveReportsSearchHeader" runat="server" visible="false">Provider Financial Transaction Search Results</div>
    <asp:GridView
        ID="gvProviderFinancialTransaction"
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
        PageIndexChanged="gvProviderFinancialTransaction_PageIndexChanged"
        OnPageIndexChanging="gvProviderFinancialTransaction_PageIndexChanging"
        DataKeyNames="OUTBOUND_FINANCIAL_SERVICE_REQ_RES_ID">
        <Columns>
            <asp:BoundField DataField="OUTBOUND_FINANCIAL_SERVICE_REQ_RES_ID" HeaderText="OUTBOUND FINANCIAL_SERVICE REQ RES ID"  />
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
                <asp:Label ID="lblPFGenXML" CssClass="ohio-field" AssociatedControlID="txtPFGenXML" runat="server">
                    <span class="ohio-field-label">Generated XML: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Generated XML for that transaction ID" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtPFGenXML" CssClass="ohio-field-input" runat="server" TextMode="MultiLine" />
                </asp:Label>
            </div>
            <div class="col-sm-6 col-md-4 col-lg-6 outerName">
                <asp:Label ID="lblPFResponse" CssClass="ohio-field" AssociatedControlID="txtPFResponse" runat="server">
                    <span class="ohio-field-label">SI Response: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Here you wil see Response From SI when Make Request is clicked" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtPFResponse" CssClass="ohio-field-input" runat="server" TextMode="MultiLine" />
                </asp:Label>
            </div>
        </div>
        <div class="row" style="padding-left: 1in">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <br />
                    <div class="row">
                        <asp:Button ID="btnGenXMLProvFinancial" runat="server" Text="Generate Provider Financial XML" OnClick="btnGenXMLProvFinancial_Click" CssClass="buttonBoxFocus" />
                        <asp:Button ID="btn1099Inquiry" runat="server" Text="Make 1099 Inquiry Request" OnClick="btn1099Inquiry_Click" CssClass="buttonBox" />
                        <asp:Button ID="btn1099History" runat="server" Text="Make 1099 History Request" OnClick="btn1099History_Click" CssClass="buttonBox" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnGenXMLProvFinancial" />
                    <asp:PostBackTrigger ControlID="btn1099Inquiry" />
                    <asp:PostBackTrigger ControlID="btn1099History" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Panel>
