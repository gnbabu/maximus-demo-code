<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_Medicaid, App_Web_qlfnt5yf" %>
<%@ Register Src="~/UserControls/FormField.ascx" TagPrefix="uc" TagName="FormField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<script type="text/javascript">
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
                            <asp:BoundField DataField="ISSUING_STATE"           HeaderText="State"          SortExpression="ISSUING_STATE" />
                            <asp:BoundField DataField="MEDICAID_NUMBER"           HeaderText="Medicaid Number"          SortExpression="MEDICAID_NUMBER" />
                            <asp:BoundField DataField="EFF_DATE"           HeaderText="Effective Date"          SortExpression="EFF_DATE" DataFormatString="{0:MM/dd/yyyy}" />
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
            <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkHistoryExcel_Click" />
        </div>
        <asp:button runat="server" id="ButtonDummy3" style="display: none" text="”ButtonDummy3”" />
    </asp:panel>
</asp:panel>
<div class="divHistoryAndAdd" style="vertical-align: middle;">
    <asp:LinkButton ID="btnHistory" runat="server" AlternateText="History" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" ToolTip="History" Text="History" Style="vertical-align: middle; color: white; text-decoration: none; padding: 4px;" />
</div>
<asp:Panel ID="pnlMedicaid" runat="server">
    <div class="divGrid">
        <asp:GridView runat="server" Width="98%" ID="grdMedicaid" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No Other State Medicaid Number found" OnRowCommand="grd_RowCommand"
            AllowSorting="true" AllowPaging="false" PageSize="2">
            <Columns>
                <asp:BoundField DataField="MEDICAID_STATE" HeaderText="Other State Medicaid Enrollment" SortExpression="MEDICAID_STATE" />
                <asp:BoundField DataField="MEDICAID_NUMBER" HeaderText="Medicaid Number" SortExpression="MEDICAID_NUMBER" />
                <asp:BoundField DataField="MEDICAID_EFF_DATE" HeaderText="Date Enrolled" SortExpression="MEDICAID_EFF_DATE" DataFormatString="{0:d}" />
                <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" DataFormatString="{0:d}" />
                <asp:TemplateField ItemStyle-Width="2%">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit"  alt="EditButton" runat="server" CommandName="EditMedicaidGridRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    <div class="divHistoryAndAdd">
        <asp:ImageButton ID="btnAddMedicaid" runat="server" ImageUrl="~/Images/add.png" AlternateText="Add New" CommandName="Medicaid" OnCommand="lbtnAdd_Click" ToolTip="Add" /><br />
    </div>
</asp:Panel>

<div id="medicaidDetail" runat="server" visible="false">
<asp:UpdatePanel ID="upSSN" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <script type="text/javascript">
            function makeBtnSaveEnabled(val) {
                if (val.options[val.selectedIndex].text == "")
                    $(".ToggleEnable").attr("disabled", "disabled");
                else
                    $(".ToggleEnable").removeAttr("disabled");
            }
        </script>
        <style type="text/css">
            .formLabel
            {
                width: 140px!important;
            }

            input[type='text']
            {
                border: solid 1px black!important;
                /*width: 250px;*/
            }

            input[type='checkbox']
            {
                float: left;
            }

            .modalPopup, #ctl00_MainContent_ucLicensesClassifications_ucMessageModal_pnlModal
            {
                height: 320px;
                width: 785px;
            }

            #labelTable > tbody > tr > td > span
            {
                width: 150px;
            }

            #provTable > tbody > tr > td > input[type='text'], #pdmsTable > tbody > tr > td > input[type='text']
            {
                width: 250px;
            }

            #provTable > tbody > tr > td > span
            {
                border: none;
            }

            .dataTable > tbody > tr > td
            {
                height: 25px;
            }

            .pdmsLabel
            {
                width: 250px;
            }

            table
            {
                text-align: left!important;
            }

            .failureNotification
            {
                text-align: left!important;
            }
        </style>

        <asp:ValidationSummary ID="OtherMiscValidationSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="Medicaid" />

        <div id="provTable" class="dataTable">
            <div class="row">
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblEnrollment_Status" runat="server" Text="Other State Medicaid Enrollment Status" CssClass="formLabel300" /></div>
                <div class="col-sm-8">
                    <asp:DropDownList ID="prov_Medicaid_Enrollment_Status" CssClass="formDropDown" AutoPostBack="true" AppendDataBoundItems="True" ValidationGroup="Medicaid" runat="server" onchange="makeBtnSaveEnabled(this)" OnSelectedIndexChanged="prov_Medicaid_Enrollment_Status_SelectedIndexChanged">
                        <asp:ListItem Value="" Text="" />
                        <asp:ListItem Value="1" Text="In Process" />
                        <asp:ListItem Value="2" Text="Completed" />
                    </asp:DropDownList>
                    <%--<asp:RequiredFieldValidator runat="server" ID="reqNumber" ControlToValidate="prov_Number" ErrorMessage="* Enter Number" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="Medicaid" />--%>
                </div>
            </div>
            <%--<tr>
                <td>
                    <asp:Label ID="lblInProgress" runat="server" Text="Other State Medicaid Enrollment In Process" CssClass="formLabel300" />
                    <asp:Label ID="lblCompleted" runat="server" Text="Other State Medicaid Enrollment Completed" CssClass="formLabel300" />
                </td>
                <td></td>
            </tr>--%>

            <div class="row">
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblState" runat="server" Text="State" CssClass="formLabel300" /></div>
                <div class="col-sm-8">
                    <asp:DropDownList ID="prov_State" AutoPostBack="true" ValidationGroup="Medicaid" runat="server" CssClass="formDropDown" OnSelectedIndexChanged="prov_State_SelectedIndexChanged"></asp:DropDownList>
                    <%--<asp:CustomValidator ID="CustVal_LC105" runat="server" ControlToValidate="prov_State" OnServerValidate="ValidateLC105" Display="Static" ValidationGroup="Medicaid" ErrorMessage="* State is required." Text="*" />--%>
                </div>
            </div>
            <div id="divMedicaidInfo" runat="server">
            <div class="row">
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblDateEnrolled" runat="server" Text="Date Enrolled" CssClass="formLabel300" /></div>
                <div class="col-sm-8">

                    <asp:TextBox ID="prov_Date_Enrolled" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="calStart" TargetControlID="prov_Date_Enrolled" runat="server" />
                    <asp:CompareValidator ID="dateValidator" runat="server" ValidationGroup="Medicaid"
                        Type="Date" Operator="DataTypeCheck" ControlToValidate="prov_Date_Enrolled"
                        ErrorMessage="Select a valid Date Enrolled" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                        SetFocusOnError="true" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblMedicaidNumber" runat="server" Text="Other State Medicaid ID Number" CssClass="formLabel300" /></div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtOtherStateMedicaidNumber" runat="server" MaxLength="11" CssClass="formField" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtOtherStateMedicaidNumber" ValidationExpression="^[0-9]*$"
                        ErrorMessage="* Enter numbers only for Other State Medicaid Number." Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="Medicaid" Display="Dynamic" />
                </div>
            </div>
             <div class="row">
                <div class="col-sm-2 text-right">
                    <asp:Label ID="lbldiffbillfornpi" runat="server" Text="Do you bill with a different NPI from what is listed in your Ohio application? *" CssClass="formLabel300" /></div>
		         <span />
		         <div style="padding-left: 400px">
			        <div class="col-sm-8">
				        <asp:RadioButtonList id="rbldiffbillfornpi" runat="server" RepeatDirection="Horizontal">
					        <asp:ListItem Value="Y">Yes</asp:ListItem>
					        <asp:ListItem Value="N">No</asp:ListItem>
				        </asp:RadioButtonList>
				        <asp:RequiredFieldValidator ID="rfvdiffbillfornpi" ControlToValidate="rbldiffbillfornpi" ErrorMessage="This field is required" ValidationGroup="Medicaid" runat="server" ></asp:RequiredFieldValidator>
			        </div>
		        </div>
            </div>
            <div class="row">
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblNPI" runat="server" Text="NPI" CssClass="formLabel300" /></div>
                <div class="col-sm-8">
                    <asp:TextBox ID="prov_NPI" runat="server" MaxLength="10" CssClass="formField" OnTextChanged="prov_NPI_TextChanged" />
                    <asp:RegularExpressionValidator ID="valNPIFormat" runat="server" ControlToValidate="prov_NPI" ValidationExpression="^([1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$"
                        ErrorMessage="* Enter a 10 digit NPI that does not begin with 0." Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="Medicaid" Display="Dynamic" />
                </div>
            </div>
                </div>
        </div>
  
        <asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
        <asp:TextBox ID="hidID" runat="server" Visible="false" />
        <asp:PlaceHolder runat="server" ID="PlaceholderUploadMedicaid"></asp:PlaceHolder>

    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="prov_Medicaid_Enrollment_Status" EventName="SelectedIndexChanged" />

    </Triggers>
</asp:UpdatePanel>
</div>
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
                    <telerik:GridBoundColumn DataField="ISSUING_STATE"           HeaderText="State"          SortExpression="ISSUING_STATE" />
                    <telerik:GridBoundColumn DataField="MEDICAID_NUMBER"           HeaderText="Medicaid Number"          SortExpression="MEDICAID_NUMBER" />
                    <telerik:GridBoundColumn DataField="EFF_DATE"           HeaderText="Effective Date"          SortExpression="EFF_DATE" DataFormatString="{0:MM/dd/yyyy}" />
                    <telerik:GridBoundColumn DataField="UserName" HeaderText="Username" SortExpression="UserName" />
                    <telerik:GridBoundColumn DataField="DateOfAction" HeaderText="DateOfAction" SortExpression="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>

</div>