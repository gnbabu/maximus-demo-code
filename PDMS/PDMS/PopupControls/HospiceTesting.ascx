<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_HospiceTesting" Codebehind="HospiceTesting.ascx.cs" %>
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

<asp:Panel ID="pnlHospiceTesting" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div id="div1" runat="server" class="test-left" style="text-align: left;">
        <span class="pdspnlHospiceTesting" id="Span1" runat="server"><b>Hospice Search Transaction</b></span>
        <hr />
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblHospiceTransactionType" class="ohio-select" AssociatedControlID="ddlHospiceTransactionType" runat="server">
                    <span class="ohio-select-label">Transaction Type:<span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover"
                        data-placement="right" data-content="Enter the Transaction Type"
                        aria-hidden="true"></span>
                    </span>
                    <span class="ohio-select-select fa">
                        <asp:DropDownList ID="ddlHospiceTransactionType" CssClass="ohio-select-select-el" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlHospiceTransactionType_SelectedIndexChanged">
                            <asp:ListItem Enabled="true" Text="Select Transaction Type" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Add/Update Hospice" Value="AddUpdateHospice"></asp:ListItem>
                            <asp:ListItem Text="Inquiry Hospice" Value="InquireHospice"></asp:ListItem>
                            <asp:ListItem Text="Search Hospice" Value="SearchHospice"></asp:ListItem>
                            <asp:ListItem Text="Attachments Hospice" Value="SendAttachment"></asp:ListItem>
                        </asp:DropDownList>
                    </span>
                </asp:Label>
            </div>
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblHospiceTXNQID" CssClass="ohio-field" AssociatedControlID="txtHospiceTXNQID" runat="server">
                    <span class="ohio-field-label">SI Transaction Key: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Enter the SI Transaction Key" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtHospiceTXNQID" CssClass="ohio-field-input" runat="server" />
                </asp:Label>
            </div>
        </div>
    </div>
    <div id="divPFSearchHeader_id" class="RetrieveReportsSearchHeader" runat="server" visible="false">Hospice Transaction Search Results</div>
    <asp:GridView
        ID="gvHospiceTransaction"
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
        PageIndexChanged="gvHospiceTransaction_PageIndexChanged"
        OnPageIndexChanging="gvHospiceTransaction_PageIndexChanging"
        DataKeyNames="ID">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="OUTBOUND HOSPICE REQ RES ID"  />
            <asp:BoundField DataField="REQUEST_TYPE" HeaderText="REQUEST TYPE" />
            <asp:BoundField DataField="RESPONSE_CODE" HeaderText="RESPONSE CODE" />
            <asp:BoundField DataField="RESPONSE_TYPE" HeaderText="RESPONSE TYPE" />
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
                <asp:Label ID="lblHospiceGenXML" CssClass="ohio-field" AssociatedControlID="txtHospiceGenXML" runat="server">
                    <span class="ohio-field-label">Generated XML: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Generated XML for that transaction ID" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtHospiceGenXML" CssClass="ohio-field-input" runat="server" TextMode="MultiLine" />
                </asp:Label>
            </div>
            <div class="col-sm-6 col-md-4 col-lg-6 outerName">
                <asp:Label ID="lblHospiceResponse" CssClass="ohio-field" AssociatedControlID="txtHospiceResponse" runat="server">
                    <span class="ohio-field-label">SI Response: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Here you wil see Response From SI when Make Request is clicked" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtHospiceResponse" CssClass="ohio-field-input" runat="server" TextMode="MultiLine" />
                </asp:Label>
            </div>
        </div>
        <div class="row" style="padding-left: 1in">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <br />
                    <div class="row">
                        <asp:Button ID="btnGenXMLHospice" runat="server" Text="Generate Hospice XML" OnClick="btnGenXMLHospice_Click" CssClass="buttonBoxFocus" />
                        <asp:Button ID="btnSubmitHospice" runat="server" Text="Submit Hospice" OnClick="btnSubmitHospice_Click" CssClass="buttonBoxFocus" />
                        <asp:Button ID="btnInquiretHospice" runat="server" Text="Inquire Hospice" OnClick="btnInquiretHospice_Click" CssClass="buttonBoxFocus" />
                        <asp:Button ID="btnSearchHospice" runat="server" Text="Search Hospice" OnClick="btnSearchHospice_Click" CssClass="buttonBoxFocus" />
                        <asp:Button ID="btnSubmitAttachment" runat="server" Text="Submit Attachment" OnClick="btnSubmitAttachment_Click" CssClass="buttonBoxFocus" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnGenXMLHospice" />
                    <asp:PostBackTrigger ControlID="btnSubmitHospice" />
                    <asp:PostBackTrigger ControlID="btnInquiretHospice" />
                    <asp:PostBackTrigger ControlID="btnSearchHospice" />
                    <asp:PostBackTrigger ControlID="btnSubmitAttachment" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Panel>
