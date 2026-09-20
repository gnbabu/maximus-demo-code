<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_DMERegisteredAgent" Codebehind="DMERegisteredAgent.ascx.cs" %>
<%@ Register Src="~/UserControls/Address.ascx" TagName="Address" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<script type="text/javascript">
    function numericOnly(obj) {
        obj.value = obj.value.replace(/[^0-9]/g, '');
    }
    function alphabetsOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z]/g, '');
    }
</script>



<asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
<asp:ValidationSummary ID="vsDMERegisteredAgent" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valDMERegisteredAgent" />
<asp:UpdatePanel ID="upDMERegAgent" runat="server">
    <ContentTemplate>
        <asp:UpdateProgress runat="server" ID="upProgress" DisplayAfter="0">
            <ProgressTemplate>
                <div class="loading">
                    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/Images/ajax-loader.gif" />Loading...
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div id="ParentTable" style="table-layout: fixed;">
            <%--<tr>
                <td style="width: 40%"></td>
                <td style="width: 40%"></td>
                <td style="width: 20%"></td>
            </tr>--%>
            <div class="row text-center">
                <div class="panelContent">*PO Box Address Prohibited</div>                
            </div>
            <div class="row">
                <div class="col-sm-3 text-right">
                    <asp:Label ID="Label10" runat="server" Text="Name*" CssClass="formLabel200" /></div>
                <div class="col-sm-9 text-left">
                    <asp:TextBox ID="txtName" runat="server" MaxLength="50" CssClass="formField"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valFNReqd" runat="server" ControlToValidate="txtName" Enabled="true" SetFocusOnError="true"
                        Display="Dynamic" Text="*" ValidationGroup="valDMERegisteredAgent" ErrorMessage="* Name is required."></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right">
                    <asp:Label ID="Label1" runat="server" Text="Company Name*" CssClass="formLabel200" /></div>
                <div class="col-sm-9 text-left">
                    <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="50" CssClass="formField"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valCNReqd" runat="server" ControlToValidate="txtCompanyName" Enabled="true" SetFocusOnError="true"
                        Display="Dynamic" Text="*" ValidationGroup="valDMERegisteredAgent" ErrorMessage="* Company Name is required."></asp:RequiredFieldValidator>
                </div>
            </div>
            
         <uc:Address ID="ucAddress" runat="server" ValidationGroup="valDMERegisteredAgent" GetGeocode="true" />
            <div class="row">
                <div class="col-sm-3 text-right">
                    <asp:Label ID="Label2" runat="server" Text="Website" CssClass="formLabel200" /></div>
                <div class="col-sm-9 text-left">
                    <asp:TextBox ID="txtWebsite" runat="server" MaxLength="50" CssClass="formField"></asp:TextBox>
                    <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1"
                        ControlToValidate="txtWebsite" ValidationExpression="^(http(?:s)?\:\/\/[a-zA-Z0-9]+(?:(?:\.|\-)[a-zA-Z0-9]+)+(?:\:\d+)?(?:\/[\w\-]+)*(?:\/?|\/\w+\.[a-zA-Z]{2,4}(?:\?[\w]+\=[\w\-]+)?)?(?:\&[\w]+\=[\w\-]+)*)$"
                        ErrorMessage="* Enter valid website (format http://www.website.com)" Text="*" Display="Dynamic" SetFocusOnError="true"
                        ValidationGroup="valDMERegisteredAgent" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right">
                    <asp:Label ID="Label3" runat="server" Text="Email Address*" CssClass="formLabel200" /></div>
                <div class="col-sm-9 text-left">
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="80" CssClass="formField"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valEmail" runat="server" ControlToValidate="txtEmail" Enabled="true" SetFocusOnError="true"
                        Display="Dynamic" Text="*" ValidationGroup="valDMERegisteredAgent" ErrorMessage="* Email is required."></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="valEmailFormat"
                        ControlToValidate="txtEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        ErrorMessage="* Enter valid email address" Text="*" Display="Dynamic" SetFocusOnError="true"
                        ValidationGroup="valDMERegisteredAgent" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right">
                    <asp:Label ID="Label4" runat="server" Text="Phone*" CssClass="formLabel200" /></div>
                <div class="col-sm-9 text-left">
                    <asp:TextBox ID="txtPhone" runat="server" MaxLength="10" CssClass="formField" onKeyUp="javascript:numericOnly(this);"></asp:TextBox>
                    <ajax:MaskedEditExtender runat="server" ID="MaskedEditExtender1" AutoComplete="False"
                        ClearMaskOnLostFocus="False" TargetControlID="txtPhone"
                        MaskType="Number" Mask="(999) 999-9999" CultureAMPMPlaceholder=""
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                        CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvPhone" ValidationGroup="valDMERegisteredAgent"
                        ControlToValidate="txtPhone" ErrorMessage="* Enter Phone Number" Text="*" Display="Dynamic"
                        SetFocusOnError="true" InitialValue="(___) ___-____" />
                    <asp:CustomValidator ID="cvPhone" runat="server" SetFocusOnError="True" Display="Dynamic"
                        ControlToValidate="txtPhone" ClientValidationFunction="CheckPhoneLength" 
                        ErrorMessage="Enter valid Phone Number" Text="*" ValidationGroup="valDMERegisteredAgent" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right">
                    <asp:Label ID="Label5" runat="server" Text="Fax" CssClass="formLabel200" /></div>
                <div class="col-sm-9 text-left" >
                    <asp:TextBox ID="txtFaxNumber" runat="server" MaxLength="10" CssClass="formField" onKeyUp="javascript:numericOnly(this);"></asp:TextBox>
                    <ajax:MaskedEditExtender runat="server" ID="MaskedEditExtender2" AutoComplete="False"
                        ClearMaskOnLostFocus="False" TargetControlID="txtFaxNumber"
                        MaskType="Number" Mask="(999) 999-9999" CultureAMPMPlaceholder=""
                        CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                        CultureDatePlaceholder="" CultureDecimalPlaceholder=""
                        CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" />

                    <asp:CustomValidator ID="CustomValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                        ControlToValidate="txtFaxNumber" ClientValidationFunction="CheckPhoneLength"
                        ErrorMessage="Enter valid Fax Number" Text="*" ValidationGroup="valDMERegisteredAgent" />
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
