<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_PrimaryContactAddress, App_Web_wenzyumt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/UserControls/Address.ascx" TagName="Address" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/PrimaryContactAddressHistory.ascx" TagPrefix="uc" TagName="PrimaryContactAddressHistory" %>

<script type="text/javascript">
    function CheckPhoneLength(sender, args) {
        var re = /\D/g; // Remove any characters that are not numbers
        var test = args.Value.replace(re, "");
        if (test === "") return true;

        var len = test.length;
        if (len !== 10)
            args.IsValid = false;
        if (test[0] === 0 || test[0] === 1 || test[3] === 0 || test[3] === 1)
            args.IsValid = false;
        return false;
    }

    function removeDisabled() {
        $("#<%= btnCloseHistory.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    }
</script>
<style>
    input[type=checkbox] {
        height: 21px;
        width: 21px;
    }
</style>
<div onmouseover="removeDisabled();">
<div>
    <asp:ValidationSummary ID="vsPrimaryContactAddress" runat="server" DisplayMode="List" ValidationGroup="PrimaryContactAddress" CssClass="failureNotification" />
</div>
<div class="divHistoryAndAdd">
    <asp:LinkButton TabIndex="0" ID="btnHistory" CommandName="History" runat="server" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" Text="History!" ToolTip="History" Style="color: white; text-decoration: none;">
        History</asp:LinkButton>
</div>
<asp:UpdatePanel ID="upProv" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="PrimaryContactAddress" runat="server" style="width: 100%;">
            <div id="ParentTable" runat="server">
           <span style="color:#df2012; font-size: 14pt !important"><b>An asterisk * indicates a required field</b></span>
                    <div class="row">
                  <div class="col-sm-3  text-right"><span class="formLabel200">Override Address Validation</span></div>
                <div class="col-sm-9" id="div1" runat="server">
               <asp:CheckBox ID="primaryContactAddress_override" Checked="false" runat="server" CssClass="formFieldCheckBox" OnCheckedChanged="prov_Override_CheckedChanged" AutoPostBack="true" style="border:none" />
                     </div>
                 </div>
                <div class="row">
                    <div class="col-sm-3  text-right">
                        <asp:Label ID="lblName" runat="server" CssClass="formLabel200" Text="Name*"></asp:Label>
                    </div>
                    <div id="divContactName" class="col-sm-9" runat="server">
                        <asp:TextBox ID="txtPrimaryContactName" runat="server" CssClass="formField" MaxLength="50" aria-label="Name" aria-required="true" />
                        <asp:RequiredFieldValidator runat="server" ID="reqPrimaryContactName"
                            ControlToValidate="txtPrimaryContactName" ErrorMessage="* Enter Primary Contact Name" Text="*" Display="Dynamic"
                            SetFocusOnError="true" ValidationGroup="PrimaryContactAddress" />
                    </div>
                    <div style="display: none;">
                        <asp:Label ID="lblPDMSPrimaryContactName" runat="server" CssClass="formFieldDisplayAuto" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3  text-right"></div>
                    <div class="col-sm-9">
                        <div class="pg-hint3Inherit">
                            <asp:Literal ID="PRIMARY_CONTACT_HELPTEXT" runat="server" Text="<%$ Resources:BrandingResource , PRIMARY_CONTACT_HELPTEXT %>" />
                        </div>
                    </div>
                </div>
                <uc:Address ID="ucAddress" runat="server" ValidationGroup="PrimaryContactAddress"></uc:Address>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Panel ID="upPrimaryContactAddressHistory" runat="server">
    <ajax:ModalPopupExtender ID="mpe" runat="server" BackgroundCssClass="modalBackground" CancelControlID="btnCloseHistory" PopupControlID="pnlModal" PopupDragHandleControlID="pnlModal" TargetControlID="ButtonDummy3" />
    <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 1000px;">
        <asp:Panel ID="pnlHeader" runat="server" CssClass="pnlHeader" HorizontalAlign="Left">
            <div align="left">
                &nbsp;&nbsp;<asp:Label ID="lblTitle" runat="server" CssClass="bodyTextBold" ForeColor="White" Text="Title" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px">
            <div class="container-fluid" style="text-align: left; padding: 15px;">
                <div class="row">
                    <uc:PrimaryContactAddressHistory ID="ucPrimaryContactAddressHistory" runat="server" />
                </div>
                <div class="row">
                    <div class="btnBox" style="text-align: right;">
                        <asp:Button ID="btnCloseHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="OK" />
                        <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkExcel_Click" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Button runat="server" aria-Label="Dummy Button" ID="ButtonDummy3" Style="display: none" Text=”ButtonDummy3” />
</asp:Panel>

<div style="width: 100%; text-align: right; display: none;">
    <asp:HiddenField ID="hdnRowCount" runat="server" />
    <asp:Button ID="lnkExcel" CommandName="Export" OnClick="lnkExcel_Click" runat="server" ToolTip="Excel" Visible="true" Text="Export"/>
    <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
    </div>
    <telerik:RadGrid ID="grd" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
        AutoGenerateColumns="true" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
            <Excel Format="Biff" />
        </ExportSettings>
        <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
            DataKeyNames="INDEX, DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
            <Columns>
                <telerik:GridBoundColumn DataField="OPERATION"           HeaderText="Operation"      SortExpression="Operation" />
                <telerik:GridBoundColumn DataField="NAME"                   HeaderText="Name"        SortExpression="NAME" />
                <telerik:GridBoundColumn DataField="ADDRESS1"            HeaderText="Address"        SortExpression="ADDRESS1" />
                <telerik:GridBoundColumn DataField="EMAIL1"              HeaderText="Email"          SortExpression="EMAIL1" />
                <telerik:GridBoundColumn DataField="PHONE1"              HeaderText="Phone"          SortExpression="PHONE1" />
                <telerik:GridBoundColumn DataField="ADR_EFFECTIVE_DATE"  HeaderText="Effective Date" SortExpression="ADR_EFFECTIVE_DATE" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
                <telerik:GridBoundColumn DataField="ADR_END_DATE"        HeaderText="End Date"       SortExpression="ADR_END_DATE"       DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" />
                <telerik:GridBoundColumn DataField="UserName"            HeaderText="User Name"      SortExpression="UserName" />
                <telerik:GridBoundColumn DataField="DateOfAction"        HeaderText="Update Date"    SortExpression="DateOfAction"       DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
</div>