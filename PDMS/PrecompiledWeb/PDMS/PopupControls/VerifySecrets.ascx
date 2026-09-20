<%@ control language="C#" autoeventwireup="true" inherits="UserControls_VerifySecrets, App_Web_rqhgepvh" %>
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

<asp:Panel ID="pnlVerifySecretsKeys" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div id="div1" runat="server" class="test-left" style="text-align: left;">
        <span class="pdspnlPATesting" id="Span1" runat="server"><b>Verify Secrets</b></span>
        <hr />
    </div>
    <div>
        <asp:ValidationSummary ID="valSummaryError" runat="server" DisplayMode="List" ForeColor="Red" ValidationGroup="VerifySecretsVS" ShowSummary="true" /><br />
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblRegionID" CssClass="ohio-field" AssociatedControlID="txtRegionID" runat="server">
                    <span class="ohio-field-label">Region*: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Enter the Region" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtRegionID" CssClass="ohio-field-input" runat="server" />            
                    <asp:RequiredFieldValidator runat="server" ID="rfvtxtRegionID" ValidationGroup="VerifySecretsVS"
                        ControlToValidate="txtRegionID" ErrorMessage="Enter Secrets Region." Text="*" Display="Dynamic" 
                        SetFocusOnError="true" />   
                </asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblDictionaryID" CssClass="ohio-field" AssociatedControlID="txtDictionaryID" runat="server">
                    <span class="ohio-field-label">Dictionary*: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Enter the Dictionary" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtDictionaryID" CssClass="ohio-field-input" runat="server" />            
                    <asp:RequiredFieldValidator runat="server" ID="rfvtxtDictionaryID" ValidationGroup="VerifySecretsVS"
                        ControlToValidate="txtDictionaryID" ErrorMessage="Enter Secrets Dictionary." Text="*" Display="Dynamic" 
                        SetFocusOnError="true" />   
                </asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblNameID" CssClass="ohio-field" AssociatedControlID="txtNameID" runat="server">
                    <span class="ohio-field-label">Name*: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Enter the Name" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtNameID" CssClass="ohio-field-input" runat="server" />            
                    <asp:RequiredFieldValidator runat="server" ID="rfvtxtNameID" ValidationGroup="VerifySecretsVS"
                        ControlToValidate="txtNameID" ErrorMessage="Enter Secrets Name." Text="*" Display="Dynamic" 
                        SetFocusOnError="true" />   
                </asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-6 col-md-4 col-lg-3 outerName">
                <asp:Label ID="lblSecretsValue" CssClass="ohio-field" AssociatedControlID="txtValueID" runat="server">
                    <span class="ohio-field-label">Value: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtValueID" CssClass="ohio-field-input" runat="server" Enabled="false" />
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
                        <asp:Button ID="btnGetSecretsKeyID" runat="server" Text="Get Secrets Value" OnClick="btnGetSecretsKey_Click" CssClass="buttonBoxFocus" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnGetSecretsKeyID" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Panel>
