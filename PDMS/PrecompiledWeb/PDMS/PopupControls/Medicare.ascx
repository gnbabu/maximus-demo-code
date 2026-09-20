<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_Medicare, App_Web_wbqq1lcm" %>
<%@ Register Src="~/UserControls/FormField.ascx" TagPrefix="uc" TagName="FormField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>

<script type="text/javascript">
    $(document).ready(function () {

        //Update Panel needs after async postback, else event handlers are lost.
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(setupMedicareEventHandlers);
    });
    var specialKeys = new Array();
    specialKeys.push(8);
    function IsNumeric(e) {
        var keyCode = e.which ? e.which : e.keyCode
        var ret = ((keyCode >= 48 && keyCode <= 57) || specialKeys.indexOf(keyCode) != -1);
        //document.getElementById("error").style.display = ret ? "none" : "inline";  
        return ret;
    }
    function setupMedicareEventHandlers() {

        if ($("#<%= ddlEnrollmentStatus.ClientID %>") != null) {
            $("#<%= vsMedicareSummary.ClientID %>").ValidationGroup = "Medicare";
            if ($('#<%=ddlEnrollmentStatus.ClientID %> option:selected').text().toLowerCase() == "completed") {
                $('.completeFields').show();
                $("#<%= vsMedicareSummary.ClientID %>").ValidationGroup = "MedicareComplete";
            }
            else if ($('#<%=ddlEnrollmentStatus.ClientID %> option:selected').text().toLowerCase() == "in process") {
                $('.completeFields').show();
            }
        }

        $("#<%= ddlEnrollmentStatus.ClientID %>").change(function (evt) {
            $("#<%= vsMedicareSummary.ClientID %>").ValidationGroup = "Medicare";
            if ($('#<%=ddlEnrollmentStatus.ClientID %> option:selected').text().toLowerCase() == "completed") {
                $('.completeFields').show();
                $("#<%= vsMedicareSummary.ClientID %>").ValidationGroup = "MedicareComplete";
            }
            else if ($('#<%=ddlEnrollmentStatus.ClientID %> option:selected').text().toLowerCase() == "in process") {
                $('.completeFields').show();
            }
        })
    }

    function removeDisabled() {
        $("#<%= btnCloseHistory.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    }
</script>
<style>
    .tooltipNew {
        position: relative;
        display: inline-block;
        border-bottom: 1px solid blue;
        font-size: 14pt;
        font-weight: bold;
        padding-left: 5px;
        padding-top: 2px;
        padding-bottom: 4px;
        text-align: left;
    }

        .tooltipNew .tooltiptext {
            visibility: hidden;
            width: 120px;
            background-color: black;
            color: #fff;
            border-radius: 6px;
            /* Position the tooltip */
            position: absolute;
            z-index: 1;
            padding: 5px;
            text-align: left;
        }

        .tooltipNew:hover .tooltiptext {
            visibility: visible;
            background-color: lemonchiffon;
            border: solid 1px #808080;
            font-size: 9pt;
            height: auto;
            width: auto;
            position: absolute;
            z-index: 9;
            color:black;
        }
</style>
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
                            <asp:BoundField DataField="MEDICARE_TYPE_NAME"           HeaderText="Type"          SortExpression="MEDICARE_TYPE_NAME" />
                            <asp:BoundField DataField="MEDICARE_NUMBER"           HeaderText="Medicare Number"          SortExpression="MEDICARE_NUMBER" />
                            <asp:BoundField DataField="MEDICARE_STATE"           HeaderText="State"          SortExpression="MEDICARE_STATE" />
                            <asp:BoundField DataField="ENROLLMENT_STATUS"           HeaderText="Status"          SortExpression="ENROLLMENT_STATUS" />
                            <asp:BoundField DataField="MEDICARE_EFF_DATE"           HeaderText="Eff Date"          SortExpression="MEDICARE_EFF_DATE"  DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:BoundField DataField="MEDICARE_END_DATE"           HeaderText="End Date"          SortExpression="MEDICARE_END_DATE"  DataFormatString="{0:MM/dd/yyyy}" />
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
<asp:Panel ID="pnlMedicare" runat="server" Style="display: inline-block; width: 100%;">
    <div class="divGrid">
        <asp:GridView runat="server" Width="98%" ID="grdMedicares"
            AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No records found" OnRowCommand="grdMedicares_RowCommand">
            <Columns>
                <asp:BoundField DataField="Medicare_Number" HeaderText="Medicare Number" />
                <asp:BoundField DataField="NPI" HeaderText="NPI" />
                <asp:BoundField DataField="Enrollment_Status_type_name" HeaderText="Medicare Enrollment Status" />
                <asp:BoundField DataField="Medicare_Eff_DATE" HeaderText="Medicare Enrollment Date"  DataFormatString="{0:MM/dd/yyyy}" />

                 <asp:TemplateField Visible="false">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdnMODIFIED_STATUS_TYPE_ID" runat="server" Value='<%# Eval("MODIFIED_STATUS_TYPE_ID")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField ItemStyle-Width="2%" HeaderText="<span style='display:none'>Edit</span>">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditMedicareRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/edit.png" ToolTip="Edit" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="<span style='display:none'>Delete</span>">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteMedicareRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" AlternateText="Delete"
                            Visible="<%# CanUserViewDelete(((GridViewRow) Container).RowIndex)  %>" />
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
        <asp:ImageButton ID="btnAddMedicare" runat="server" ImageUrl="~/Images/add.png" CommandName="Medicares" OnCommand="btnAddMedicare_Command" ToolTip="Add" AlternateText="Add New"  /><br />
    </div>
    <br />
</asp:Panel>
<asp:UpdatePanel ID="upMedicare" runat="server" UpdateMode="Conditional" Visible="false" ChildrenAsTriggers="false">
    <ContentTemplate>
        <asp:Label ID="lbl_ValidationMedicare" runat="server" Text="" CssClass="failureNotification"/>
        <asp:ValidationSummary ID="vsMedicareSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="MedicareComplete" />
        <div>
            <div class="row completeFields">

                <div class="col-sm-4  text-right">
                    <asp:Label ID="lblMedicareNumberType" runat="server" Text="Medicare Number Type" CssClass="formLabel200" />

                </div>
                <div class="col-sm-8">
                    <asp:RadioButtonList ID="rblMedicareNumberType" runat="server" CssClass="QstRadioList" RepeatDirection="Vertical"
                        AutoPostBack="true">
                        <asp:ListItem Value="CCN">CCN (CMS Certification Number)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp

                            <span id="helpCCN" Tabindex="0" class="tooltipNew">What is this?
                                <span class="tooltiptext">The CCN is used to verify Medicare/Medicaid certification for survey and certification, assessment-related activities and communications</span>
                            </span>

                          
                        </asp:ListItem>
                        <asp:ListItem Value="PTAN">PTAN (Provider Transaction Access Number)&nbsp;&nbsp;

                             <span id="helpPtan" Tabindex="0" class="tooltipNew">What is this?
                                <span class="tooltiptext">A PTAN is a Medicare-only number issued to providers by Medicare Administrative Contractors (MACs) upon enrollment to Medicare. MAC s issue an approval/notification letter, including PTAN information, when an enrollment is approved</span>
                            </span>

                        </asp:ListItem>
                    </asp:RadioButtonList>


                    <asp:RequiredFieldValidator ID="rfvMediCareNumberType" runat="server" ValidationGroup="MedicareComplete" ControlToValidate="rblMedicareNumberType" ErrorMessage="* Please select Medicare Number Type"
                        Enabled="true" SetFocusOnError="true" Text="*"
                        Display="Dynamic" />
                </div>
            </div>
            <div class="row completeFields">

                <div class="col-sm-4 text-right">
                    <asp:Label ID="lbl4MedicareNumber" runat="server" Text="Medicare Number*" CssClass="formLabel200" />
                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtMedicareNumber" runat="server" CssClass="formField" MaxLength="80" onKeyUp="javascript:alphanumericOnly(this);" />
                    <asp:RequiredFieldValidator ID="rfvMedicareNumber" runat="server" ValidationGroup="MedicareComplete" ControlToValidate="txtMedicareNumber" ErrorMessage="* Please enter Medicare Number"
                        Enabled="true" SetFocusOnError="true" Text="*"
                        Display="Dynamic" />
                </div>
                <div style="display: none;">
                    <asp:Label ID="lblCurrentMedicareNumber" runat="server" Text="" />
                </div>
            </div>

            <div class="row completeFields">
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lbl4NPI" runat="server" Text="Secondary NPI" CssClass="formLabel200" />
                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtNPI" runat="server" MaxLength="10" CssClass="formField" onkeypress="return IsNumeric(event);" OnTextChanged="txtNPI_TextChanged" AutoPostBack="true" />
                    <asp:RegularExpressionValidator ID="valNPIFormat" runat="server" ControlToValidate="txtNPI" ValidationExpression="^([1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$"
                        ErrorMessage="* Enter a 10 digit NPI that does not begin with 0." Enabled="true" SetFocusOnError="true" Text="*" ValidationGroup="MedicareComplete" Display="Dynamic" />
                </div>
                <div style="display: none;">
                    <asp:Label ID="lblCurrentNPI" runat="server" Text="" />
                </div>
            </div>
            <div class="row completeFields">
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblState" runat="server" Text="Medicare State*" CssClass="formLabel300" />
                </div>
                <div class="col-sm-8">
                    <asp:DropDownList ID="ddlProvState" AutoPostBack="true" ValidationGroup="Medicaid" runat="server" CssClass="formDropDown" OnSelectedIndexChanged="ddlProvState_SelectedIndexChanged"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvState" runat="server" ErrorMessage="* Please select state" ControlToValidate="ddlProvState" ValidationGroup="MedicareComplete" InitialValue=""
                        Enabled="true" SetFocusOnError="true" Text="*"
                        Display="Dynamic"></asp:RequiredFieldValidator>

                </div>
                <div style="display: none;">
                    <asp:Label ID="Label2" runat="server" Text="" />
                </div>
            </div>

            <div class="row">
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblMedicareEnrollmentStatus" runat="server" Text="Medicare Enrollment Status*" CssClass="formLabel200" />
                </div>
                <div class="col-sm-8">
                    <asp:DropDownList ID="ddlEnrollmentStatus" CssClass="formDropDown" ValidationGroup="MedicareComplete" runat="server" />
                    <asp:CompareValidator runat="server" ID="valStatusReqd" ControlToValidate="ddlEnrollmentStatus"
                        ValueToCompare="0" Type="Integer" ErrorMessage="* Status is required."
                        Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*"
                        ValidationGroup="Medicare" />

                </div>
                <div style="display: none;">
                    <asp:Label ID="lblCurrentEnrollmentStatus" runat="server" Text="" />
                </div>
            </div>
            <div class="row completeFields">
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lbl4EnrollmentDate" runat="server" Text="Medicare Enrollment Date" CssClass="formLabel200" />
                </div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtEnrollmentDate" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="calStart" TargetControlID="txtEnrollmentDate" runat="server" />

               
                </div>
                <div class="col-sm-offset-4 col-sm-8">
                    <asp:Label ID="lblCurrentEnrollmentDate" Visible="false" runat="server" Text="" />
                </div>
            </div>

        </div>
        <asp:UpdateProgress runat="server" ID="upSaveProgress" DisplayAfter="0" AssociatedUpdatePanelID="upMedicare">
            <ProgressTemplate>
                <div class="loading">
                    <asp:Image ID="imgWorking" AlternateText="Loading" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
        <asp:TextBox ID="hidID" runat="server" Visible="false" />
        <asp:PlaceHolder runat="server" ID="PlaceholderUploadMedicare" ></asp:PlaceHolder>
     
    </ContentTemplate>
</asp:UpdatePanel>

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
                    <telerik:GridBoundColumn DataField="MEDICARE_TYPE_NAME"           HeaderText="Type"          SortExpression="MEDICARE_TYPE_NAME" />
                    <telerik:GridBoundColumn DataField="MEDICARE_NUMBER"           HeaderText="Medicare Number"          SortExpression="MEDICARE_NUMBER" />
                    <telerik:GridBoundColumn DataField="MEDICARE_STATE"           HeaderText="State"          SortExpression="MEDICARE_STATE" />
                    <telerik:GridBoundColumn DataField="ENROLLMENT_STATUS"           HeaderText="Status"          SortExpression="ENROLLMENT_STATUS" />
                    <telerik:GridBoundColumn DataField="MEDICARE_EFF_DATE"           HeaderText="Eff Date"          SortExpression="MEDICARE_EFF_DATE"  DataFormatString="{0:MM/dd/yyyy}" />
                    <telerik:GridBoundColumn DataField="MEDICARE_END_DATE"           HeaderText="End Date"          SortExpression="MEDICARE_END_DATE"  DataFormatString="{0:MM/dd/yyyy}" />
                    <telerik:GridBoundColumn DataField="UserName" HeaderText="Username" SortExpression="UserName" />
                    <telerik:GridBoundColumn DataField="DateOfAction" HeaderText="DateOfAction" SortExpression="DateOfAction" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </div>

</div>