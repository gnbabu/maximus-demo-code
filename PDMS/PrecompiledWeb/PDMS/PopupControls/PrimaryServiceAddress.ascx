<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_PrimaryServiceAddress, App_Web_av5ll3zk" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/UserControls/AddressValidation.ascx" TagName="AddressValidation" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/OfficeHoursServiceLocation.ascx" TagName="OfficeHours" TagPrefix="ohsl" %>
<%@ Register Src="~/UserControls/Address.ascx" TagName="Address" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/UploadDocument.ascx" TagName="UploadDocument" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/PrimaryServiceAddressHistory.ascx" TagPrefix="uc" TagName="PrimaryServiceAddressHistory" %>
<script src="Scripts/jquery-1.9.1.js" type="text/javascript"></script>
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
<span style="color:#D10000 ; font-size: 14pt !important; font-weight:100 !important" ;>An asterisk * indicates a required field</span>
<asp:ValidationSummary ID="vsPrimaryServiceAddress" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="PrimaryServiceAddress" />
<div class="divHistoryAndAdd">
    <asp:LinkButton TabIndex="0" ID="btnHistory" CommandName="History" runat="server" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" Text="History!" ToolTip="History" Style="color: white; text-decoration: none;">
        History</asp:LinkButton>
</div>
<asp:UpdatePanel ID="upProv" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="ParentTable" runat="server">
            <div class="row" id="prov_Services_Address" style="color: #C80000; font-weight: bold;" runat="server" visible="false">
                <div class="col-sm-6 text-right">
                    <asp:Label AssociatedControlID="prov_Services_Address" runat="server" class="formLabel200">Note: Please enter the address where members receive services.</asp:Label></div>
            </div>
             <div class="row">
                  <div class="col-sm-3  text-right"><span class="formLabel200">Override Address Validation</span></div>
                <div class="col-sm-9" id="div1" runat="server">
               <asp:CheckBox ID="prov_override" runat="server" Checked="false" aria-label="Override Address Validation" CssClass="formFieldCheckBox" OnCheckedChanged="prov_Override_CheckedChanged" AutoPostBack="true" style="border:none" />
                     </div>
                 </div>
            <div class="row" id="divProvidername" runat="server">
                <div class="col-sm-3  text-right"><span class="formLabel200">Provider Name</span></div>
                <div class="col-sm-9" id="divchkchanged" runat="server">
                    <asp:TextBox ID="txtProviderName" aria-label="Provider Name"  runat="server" CssClass="formField" MaxLength="50" />
                </div>
            </div>
            <uc:Address id="ucAddress" runat="server" ValidationGroup="PrimaryServiceAddress"></uc:Address>
            <ohsl:OfficeHours ID="OfficeHours" runat="server" ValidationGroup="PrimaryServiceAddress" SubControlValidationGroup="PrimaryServiceAddress" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
<asp:HiddenField ID="hdnPracticeChange" runat="server" />

<ajax:ModalPopupExtender ID="mpepoboxmsg" runat="server" PopupControlID="pnlModalPO" TargetControlID="ButtonDummy3"
    BackgroundCssClass="modalBackground"  CancelControlID="btnModalCancel">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlModalPO" runat="server" CssClass="modalPopup"  Style="display:none;width:50%;height:auto;">
    <div class="center">
        <br />
             The Primary Service Address should not be a Post Office Box unless you're a participant in Ohio Safe At Home with the Ohio Attorney General.  If you have a declaration, upload the document.
                If you do not have a declaration, cancel to return to data entry and update the address.
        <br />
      </div>
      
    <div class="btnBox" style="padding:10px">
        <asp:Button runat="server" ID="btnModalCancel" Text="Cancel" CssClass="buttonBox" CausesValidation="false" />
         <asp:Button runat="server" ID="btnUploadPOBox" Text="Upload" CssClass="buttonBox" CausesValidation="false" OnClick="btnUploadPOBox_Click" />
    </div>
</asp:Panel>
        <asp:Button runat="server" ID="ButtonDummy3" aria-Label="Dummy Button" Style="display: none" Text=”ButtonDummy3” />

<asp:Panel ID="upPrimaryServiceAddressHistory" runat="server">
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
                    <uc:PrimaryServiceAddressHistory ID="ucPrimaryServiceAddressHistory" runat="server" />
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
    <asp:Button runat="server" aria-Label="Dummy Button" ID="Button1" Style="display: none" Text=”ButtonDummy3” />
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
</div>