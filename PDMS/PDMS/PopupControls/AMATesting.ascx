<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_AMATesting" Codebehind="AMATesting.ascx.cs" %>
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

<asp:Panel ID="pnlAMAServiceTesting" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div id="div1" runat="server" class="test-left" style="text-align: left;">
        <span class="pdspnlPATesting" id="Span1" runat="server"><b>AMA Service Testing</b></span>
        <hr />
    </div>
    <div>
        <asp:ValidationSummary ID="valSummaryError" runat="server" DisplayMode="List" ForeColor="Red" ValidationGroup="VerifyAMATesting" ShowSummary="true" />
        <br />
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblNPIID" CssClass="ohio-field" AssociatedControlID="txtNPIID" runat="server">
                    <span class="ohio-field-label">NPI*: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Enter the NPI" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtNPIID" CssClass="ohio-field-input" runat="server" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvtxtNPIID" ValidationGroup="VerifyAMATesting"
                        ControlToValidate="txtNPIID" ErrorMessage="Enter NPI." Text="*" Display="Dynamic"
                        SetFocusOnError="true" />
                </asp:Label>
            </div>
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblUserNameID" CssClass="ohio-field" AssociatedControlID="txtUserNameID" runat="server">
                    <span class="ohio-field-label">UserName*: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Enter the UserName" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtUserNameID" CssClass="ohio-field-input" runat="server" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvtxtUserNameID" ValidationGroup="VerifyAMATesting"
                        ControlToValidate="txtUserNameID" ErrorMessage="Enter UserName." Text="*" Display="Dynamic"
                        SetFocusOnError="true" />
                </asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6 col-md-4 col-lg-6 outerName">
                <asp:Label ID="lblResponse" CssClass="ohio-field" AssociatedControlID="txtAMAResponse" runat="server">
                    <span class="ohio-field-label">AMA Response: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Response from AMA" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtAMAResponse" CssClass="ohio-field-input" runat="server" TextMode="MultiLine" />
                </asp:Label>
            </div>
            <div class="col-sm-6 col-md-4 col-lg-6 outerName">
                <asp:Label ID="lblProfileResponse" CssClass="ohio-field" AssociatedControlID="txtAMAProfileResponse" runat="server">
                    <span class="ohio-field-label">AMA Full Profile Response: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Response from AMA Full Profile " aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtAMAProfileResponse" CssClass="ohio-field-input" runat="server" TextMode="MultiLine" />
                </asp:Label>
            </div>
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="Panel1" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div class="container-fluid">
        <div class="row" style="padding-left: 1in">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <br />
                    <div class="row">
                        <asp:Button ID="btnGetAMATestingID" runat="server" Text="Search AMA" OnClick="btnGetAMATestingID_Click" CssClass="buttonBoxFocus" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnGetAMATestingID" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Panel>
