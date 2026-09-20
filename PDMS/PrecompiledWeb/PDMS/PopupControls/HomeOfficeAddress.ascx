<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_HomeOfficeAddress, App_Web_yvhxe4ml" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/UserControls/Address.ascx" TagName="Address" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/HomeOfficeAddressHistory.ascx" TagPrefix="sh" TagName="History" %>

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
<div onmouseover="removeDisabled();">
<asp:ValidationSummary ID="vsHomeOfficeAddress" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valHomeOfficeAddress" />
<asp:UpdatePanel ID="upHistory" runat="server">
    <ContentTemplate>
        <div role="dialog" aria-live="assertive" aria-labelledby="dialog1Title" aria-hidden="true" id="divDialog1">
            <ajax:modalpopupextender id="mpe" runat="server" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseHistory" popupcontrolid="pnlModal" targetcontrolid="ButtonDummy2" behaviorid="mpeSplHitory" />
            <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 60%;">
                <asp:Panel ID="pnlHeader" runat="server" CssClass="pnlHeader" HorizontalAlign="Left">
                    <div align="left">
                        &nbsp;&nbsp;
                     <h2 id="dialog1Title">
                         <asp:Label ID="lblSpHistoryTitle" runat="server" CssClass="history" ForeColor="White" Text="History" /></h2>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px">
                    <div class="container-fluid" style="text-align: left; padding: 15px;">
                        <div class="row">
                            <sh:history id="ucHomeOfficeAddressHistory" runat="server" />
                        </div>
                        <div class="row">
                            <div class="btnBox" style="text-align: right;">
                                <asp:Button ID="btnCloseHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="OK" /><br />
                                <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkExcel_Click" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <asp:Button runat="server" ID="ButtonDummy2" Style="display: none" Text="”ButtonDummy2”" />
        </div>

    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExportHistory" />
    </Triggers>
</asp:UpdatePanel>

<div class="divHistoryAndAdd">
    <span aria-label="History">
        <asp:LinkButton TabIndex="0" ID="btnHistory" CommandName="History" runat="server" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" Text="History" ToolTip="History" Style="color: white; text-decoration: none;">
        History</asp:LinkButton>
    </span>
</div>
<asp:UpdatePanel ID="upProv" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="ParentTable" runat="server">
            <div class="row" id="prov_Agency" runat="server" visible="false">
                <div class="col-sm-4  text-right">
                    <asp:Label AssociatedControlID="prov_Agency" style="color: #C80000; font-weight: bold;" runat="server" class="formLabel200">Note: Please enter your Agency Business Address.</asp:Label></div>
            </div>
            <div class="row">
                <div class="col-sm-4  text-right"><asp:Label AssociatedControlID="prov_Same" runat="server" class="formLabel200">Same as Practice Location <asp:CheckBox ID="prov_Same" runat="server" CssClass="formFieldCheckBox" OnCheckedChanged="prov_Same_CheckedChanged" AutoPostBack="true" style="border:none" /></asp:Label></div>
            </div>
             <div class="row">
                  <div class="col-sm-4  text-right"><asp:Label AssociatedControlID="prov_override" runat="server" class="formLabel200">Override Address Validation<asp:CheckBox ID="prov_override" runat="server" Checked="false" CssClass="formFieldCheckBox" OnCheckedChanged="prov_Override_CheckedChanged" AutoPostBack="true" style="border:none" /></asp:Label></div>
               
                 </div>
            <uc:Address id="ucAddress" runat="server" ValidationGroup="valHomeOfficeAddress"></uc:Address>                   
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

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
                <telerik:GridBoundColumn DataField="NAME"            HeaderText="Name"        SortExpression="NAME" />
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