<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_CPCContactInformation, App_Web_c4une0e1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Src="~/UserControls/Address.ascx" TagName="Address" TagPrefix="uc" %>
<script src="../Scripts/jquery.inputmask.bundle.min.js"></script>
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
	$(document).ready(function () {
		$('.phone_number').inputmask('(999) 999-9999');
	}); 
	 function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9\x20]/g, '');
    }
    function numericOnly(obj) {
        obj.value = obj.value.replace(/[^0-9]/g, '');
	}
    function removeDisabled() {
        $("#<%= btnCloseHistory.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    } 
</script>
<div onmouseover="removeDisabled();">
<asp:panel id="upHistory" runat="server" style="min-width: 1400px; position: fixed; z-index: 2; left: 200px; top: 50px;">
    <ajax:modalpopupextender id="mpeHistory" runat="server" popupcontrolid="pHistory" targetcontrolid="ButtonDummy3"
        backgroundcssclass="modalBackground" popupdraghandlecontrolid="pHistory" CancelControlID="btnCloseHistory">
    </ajax:modalpopupextender>
    <asp:panel id="pHistory" runat="server" cssclass="modalPopup" style="padding: 20px; min-width: 1400px;">
        <div>
             <asp:panel id="pnlHistoryDetails" runat="server">
                <div>
                    <asp:GridView runat="server" Width="98%" ID="grd" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found."
                        AllowPaging="true" AllowSorting="True" PageSize="10"
                        OnPageIndexChanging="grd_PageIndexChanging" OnSorting="grd_Sorting">
                        <Columns>
                            <asp:BoundField DataField="OPERATION"           HeaderText="Operation"          SortExpression="Operation" />
                            <asp:BoundField DataField="NAME"           HeaderText="NAME"          SortExpression="NAME" />
                            <asp:BoundField DataField="PHONE"           HeaderText="PHONE"          SortExpression="PHONE" />
                            <asp:BoundField DataField="EMAIL"           HeaderText="EMAIL"          SortExpression="EMAIL" />
                            <asp:BoundField DataField="UserName" HeaderText="Username" SortExpression="UserName" />
                            <asp:BoundField DataField="DateOfAction" HeaderText="DateOfAction" SortExpression="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />
                        </Columns>
                        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                        <AlternatingRowStyle CssClass="gridViewAltRow" />
                        <RowStyle CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>
            </asp:panel>
            <asp:Button id="btnCloseHistory"  runat="server" Text="OK" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
            <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkExcel_Click" />
        </div>
        <asp:button runat="server" id="ButtonDummy3" style="display: none" text="”ButtonDummy3”" />
    </asp:panel>
</asp:panel>
<div>
	<asp:ValidationSummary ID="vsCPCContactInformation" runat="server" DisplayMode="List" CssClass="failureNotification" ValidationGroup="CPCContactInformation" />
</div>

<div class="popTitle">
	<asp:Label ID="lblTitle" CssClass="bodyTextBold groupMemberTitle" runat="server" Text="CPC Contact Information" />
</div>
<div class="divHistoryAndAdd" style="vertical-align: middle;">
    <asp:LinkButton ID="btnHistory" runat="server" AlternateText="History" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" ToolTip="History" Text="History" Style="vertical-align: middle; color: white; text-decoration: none; padding: 4px;" />
</div>
<asp:UpdatePanel ID="upProv" runat="server" UpdateMode="Conditional">

	<ContentTemplate>
		<div id="PrimaryContactAddress" runat="server" style="width: 100%;">
			<div id="ParentTable" runat="server">
				<div class="row">
					<div class="col-sm-3  text-right">
						<asp:Label ID="lblName" runat="server" CssClass="formLabel200" Text="Name*"></asp:Label>
					</div>
					<div id="divContactName" class="col-sm-9" runat="server">
						<asp:TextBox ID="txtPrimaryContactName" runat="server" CssClass="formField" MaxLength="50" onKeyUp="javascript:alphanumericOnly(this);" />
						<asp:RequiredFieldValidator runat="server" ID="reqPrimaryContactName"
							ControlToValidate="txtPrimaryContactName" ErrorMessage="* Enter Contact Name" Text="*" Display="Dynamic"
							SetFocusOnError="true" ValidationGroup="CPCContactInformation" />
					</div>
					<div style="display: none;">
						<asp:Label ID="lblPDMSPrimaryContactName" runat="server" CssClass="formFieldDisplayAuto" />
					</div>
				</div>

				<div class="row">
					<div class="col-sm-3  text-right"></div>
					<div class="col-sm-9">
						<div class="pg-hint3Inherit">
							<asp:Literal ID="PRIMARY_CONTACT_HELPTEXT" runat="server" Text="<%$ Resources:BrandingResource , CPC_CONTACT_HELPTEXT %>" />
						</div>
					</div>

				</div>

				<div class="row" id="divTitle" runat="server">
					<div class="col-sm-3  text-right">
						<asp:Label ID="Label5" runat="server" CssClass="formLabel200" Text="Title"></asp:Label>
					</div>
					<div class="col-sm-9">
						<asp:TextBox ID="txtTitle" runat="server" CssClass="formField" MaxLength="100" onKeyUp="javascript:alphanumericOnly(this);" />
					</div>
				</div>
				<div class="row" id="trPhone1" runat="server">
					<div class="col-sm-3  text-right">
						<asp:Label ID="lblPhone1" runat="server" CssClass="formLabel200" Text="Phone Number*"></asp:Label>
					</div>
					<div class="col-sm-9">
						<asp:TextBox ID="txtPhoneNo1" runat="server" CssClass="formField phone_number" />
						<asp:RequiredFieldValidator runat="server" ID="rfvPhone1" ValidationGroup="CPCContactInformation"
							ControlToValidate="txtPhoneNo1" ErrorMessage="* Enter Phone Number" Text="*" Display="Dynamic"
							SetFocusOnError="true" />
						<asp:CustomValidator ID="cvPhoneNum" runat="server" SetFocusOnError="True" Display="Dynamic"
							ControlToValidate="txtPhoneNo1" ClientValidationFunction="CheckPhoneLength" OnServerValidate="cvValidatePhoneLength"
							ErrorMessage="* Enter Valid Phone Number" Text="*" ValidationGroup="CPCContactInformation" />
					</div>
				</div>
				<div class="row" id="trPhoneExt1" runat="server">
					<div class="col-sm-3  text-right">
						<asp:Label ID="Label13" runat="server" CssClass="formLabel200" Text="Phone Extension"></asp:Label>
					</div>
					<div class="col-sm-9">
						<asp:TextBox ID="txtPhoneExt1" runat="server" CssClass="formField" MaxLength="100" onKeyUp="javascript:numericOnly(this);" />
					</div>
					<div style="display: none;">
						<asp:Label ID="Label14" runat="server" />
					</div>
				</div>
				<div class="row" id="radiobuttonText" runat="server">
					<div class="col-sm-3  text-right">
						<asp:Label ID="Label1" runat="server" CssClass="formLabel200" Text=""></asp:Label>
					</div>
				 <div class="row" style="margin-left: 100px;">
                 <asp:RadioButtonList ID="radioButtonTextSend" runat="server" CssClass="QstRadioList"  RepeatDirection="Horizontal"  >
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                </asp:RadioButtonList>
						<span class="pg-hint3Inherit">
							<asp:Literal ID="CPC_HELP_TEXT" runat="server" Text="<%$ Resources:BrandingResource ,CPC_CONTACTINFO_HELPTEXT_SEND_TEXT_MESSAGE %>" />
						</span>
                  </div>
					<%--<div style="display: none;">
						<asp:Label ID="Label2" runat="server" />
					</div>--%>
				</div>
				

                <div class="row" id="trEmail1" runat="server">
                    <div class="col-sm-3  text-right">
                        <asp:Label ID="lblEmail" runat="server" Text="Email Address*" class="formLabel200" />
                    </div>
                    <div class="col-sm-9">
                        <asp:TextBox ID="txtEmail1" CssClass="formField" runat="server" MaxLength="50" />
						<asp:RequiredFieldValidator runat="server" ID="rfvEmail1"
							ControlToValidate="txtEmail1" ErrorMessage="* Enter E-mail Address" Text="*" Display="Dynamic"
							SetFocusOnError="true" ValidationGroup="CPCContactInformation" />
						<asp:RegularExpressionValidator ID="regEmail1" runat="server" ControlToValidate="txtEmail1" Display="Dynamic" Text="*" ValidationGroup="CPCContactInformation"
							ErrorMessage="Enter valid E-mail" SetFocusOnError="true" ValidationExpression="^[A-Z'a-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}$" />
                    </div>
                    <div style="display: none;">
                        <asp:Label ID="lblPDMSEmail" runat="server" class="formFieldDisplayAuto" />
                    </div>
                </div>
				<br />
                <div class="row">
                    <div class="col-sm-9  text-right">
                    </div>
                    <div class="col-sm-3">
						<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" ValidationGroup="CPCContactInformation"
                            ToolTip="Save current screen data" />
                    </div>
                </div>


            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />

<div style="width: 100%; text-align: right; display: none;">
    <telerik:RadGrid ID="grdHistoryExport" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
        AutoGenerateColumns="true" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
            <Excel Format="Biff" />
        </ExportSettings>
        <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
            DataKeyNames="DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
            <Columns>
                <telerik:GridBoundColumn DataField="OPERATION"           HeaderText="Operation"          SortExpression="Operation" />
                <telerik:GridBoundColumn DataField="NAME"           HeaderText="NAME"          SortExpression="NAME" />
                <telerik:GridBoundColumn DataField="PHONE"           HeaderText="PHONE"          SortExpression="PHONE" />
                <telerik:GridBoundColumn DataField="EMAIL"           HeaderText="EMAIL"          SortExpression="EMAIL" />
                <telerik:GridBoundColumn DataField="UserName" HeaderText="Username" SortExpression="UserName" />
                <telerik:GridBoundColumn DataField="DateOfAction" HeaderText="DateOfAction" SortExpression="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>
</div>