<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_SearchEligibility, App_Web_l5y5araq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<div class="row">
    <div class="col-sm-19 col-md-16 col-lg-12 text-left">
        <asp:Label ID="lblProMedicaidID" class="formLabel200" Text="" runat="server" />
        <span class="formLabel200">Provider Medicaid ID:<span></span>
        </span>
        <asp:Label ID="lblProMedicaidID2" class="formLabelAuto" Text="" runat="server" />
        <span class="formLabelAuto"><span></span>
        </span>
        <asp:Label ID="lblPRONPI" class="formLabelAuto" Text="" runat="server" />
        <span class="formLabel200">Provider NPI:<span></span>
        </span>
        <asp:Label ID="lblPRONPI2" class="formLabelAuto" Text="" runat="server" />
        <span class="formLabelAuto"><span></span>
        </span>
        <asp:Label ID="lblProviderName" class="formLabelAuto" Text="" runat="server" />
        <span class="formLabel200">Provider Name:<span></span>
        </span>
        <asp:Label ID="lblProviderName2" class="formLabelAuto" Text="" runat="server" />
        <span class="formLabelAuto"><span></span>
        </span>
    </div>
</div>

<ajax:CollapsiblePanelExtender ID="cpeEligbilitysearch" runat="server" Collapsed="false" TargetControlID="pnlEligbilitysearch"
    ExpandControlID="pnlsepEligbilitysearch" CollapseControlID="pnlsepEligbilitysearch" />
<asp:Panel runat="server" ID="pnlsepEligbilitysearch" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerEligbilitysearch">
    <span id="sepEligbilitysearch" runat="server" class="pageHeader">-* Eligibility Search</span>
</asp:Panel>
<asp:Panel ID="pnlEligbilitysearch" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">

    <div class="row organization" runat="server">
        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
            <asp:Label ID="lblPriorAuthNumber" runat="server" Text=" Prior Authorization Number" CssClass="formLabel200" />
        </div>
        <div class="col-sm-9 text-left">
            <asp:TextBox ID="txtPriorAuthNumber" runat="server" CssClass="formField" MaxLength="12" />
            <asp:RequiredFieldValidator ID="rfvPriorAuthNumber" runat="server" ControlToValidate="txtPriorAuthNumber" ErrorMessage="* Enter PriorAuthNumber" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
        </div>
        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
            <asp:Label ID="lblMedicaidBillingNumber" runat="server" Text="Medicaid Billing Number" CssClass="formLabel200" />
        </div>
        <div class="col-sm-9 text-left">
            <asp:TextBox ID="txtMedicaidBillingNumber" runat="server" CssClass="formField" MaxLength="12" />
            <asp:RequiredFieldValidator ID="rfvMEDBillNum" runat="server" ControlToValidate="txtMedicaidBillingNumber" ErrorMessage="* Enter Medicaid Billing Number" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
        </div>
        <%-- <div class="col-sm-6 col-md-4 col-lg-3 text-right">
            <asp:Label ID="lblBirthDate" runat="server" Text="Date of Birth" CssClass="formLabel200" />
        </div>
        <div class="col-sm-9 text-left">
            <asp:TextBox ID="txtBirthDate" runat="server" CssClass="formField" />
            <ajax:CalendarExtender ID="ceBirthDate" runat="server" TargetControlID="txtBirthDate" />
          
            <asp:CompareValidator ID="cvBirthDate" runat="server" ControlToValidate="txtBirthDate" Display="Dynamic" ErrorMessage="Select a valid Date of Birth" Operator="DataTypeCheck" SetFocusOnError="true" Text="*" Type="Date" ValidationGroup="valOrgInfo" ValueToCompare="MM/dd/yyyy"> 
            </asp:CompareValidator>
        </div>--%>
        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
            <asp:Label ID="lblBirthDate" runat="server" Text="Date of Birth" CssClass="formLabel200" />
        </div>
        <div class="col-sm-9 text-left">
            <asp:TextBox ID="txtBirthDate" runat="server" CssClass="formField" />
            <ajax:CalendarExtender ID="calClaimAdjustdate" runat="server" Format="MM/dd/yyyy" TargetControlID="txtBirthDate"
                PopupPosition="BottomRight" CssClass="QstCalendarCSS" PopupButtonID="imgBirthDate" EnabledOnClient="true" />
            <asp:CompareValidator ID="cvBirthDate" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtBirthDate" ValidationGroup="VldGrpBirthDate"
                ErrorMessage="Select a valid To date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy" SetFocusOnError="true"> 
            </asp:CompareValidator>
            <asp:Image ID="imgBirthDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
            <asp:RequiredFieldValidator ID="reqBirthDate" runat="server" ControlToValidate="txtBirthDate"
                ErrorMessage="To Date is required" Text="*" Display="Dynamic" ValidationGroup="VldGrpBirthDate" />
            <asp:CustomValidator ID="ToDateRangeValidator" runat="server" ControlToValidate="txtBirthDate" ErrorMessage="Select a valid Date of Birth."
                Display="Dynamic" Text="*" ValidationGroup="VldBirthDate" OnServerValidate="ReportBirthDate_ServerValidate" />
        </div>

        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
            <asp:Label ID="lblLastName" runat="server" Text="Last Name" CssClass="formLabel200" />
        </div>
        <div class="col-sm-9 text-left">
            <asp:TextBox ID="txtLastName" runat="server" CssClass="formField" />
        </div>
        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
            <asp:Label ID="lblfrstmi" runat="server" Text="First Name, MI" CssClass="formLabel200" />
        </div>
        <div class="col-sm-9 text-left">
            <asp:TextBox ID="txtfrstmi" runat="server" CssClass="formField" />
        </div>
        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
            <asp:Label ID="lblfromDos" runat="server" Text="From DOS" CssClass="formLabel200" />
        </div>
        <div class="col-sm-9 text-left">
            <asp:TextBox ID="txtfromDos" runat="server" CssClass="formField" />
            <ajax:CalendarExtender ID="ceSubmissiondate" runat="server" TargetControlID="txtfromDos" />
            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBirthDate" Display="Dynamic" ErrorMessage="Enter Date of Birth" SetFocusOnError="true" Text="*" ValidationGroup="valOrgInfo" />--%>
            <asp:CompareValidator ID="cvfromDos" runat="server" ControlToValidate="txtfromDos" Display="Dynamic" ErrorMessage="Select a valid From DOS Date" Operator="DataTypeCheck" SetFocusOnError="true" Text="*" Type="Date" ValidationGroup="valOrgInfo" ValueToCompare="MM/dd/yyyy"> 
            </asp:CompareValidator>
        </div>

        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
            <asp:Label ID="lblToDos" runat="server" Text="TO DOS" CssClass="formLabel200" />
        </div>
        <div class="col-sm-9 text-left">
            <asp:TextBox ID="txtToDos" runat="server" CssClass="formField" />
            <ajax:CalendarExtender ID="ceToDos" runat="server" TargetControlID="txtToDos" />
            <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBirthDate" Display="Dynamic" ErrorMessage="Enter Date of Birth" SetFocusOnError="true" Text="*" ValidationGroup="valOrgInfo" />--%>
            <asp:CompareValidator ID="cvToDos" runat="server" ControlToValidate="txtToDos" Display="Dynamic" ErrorMessage="Select a valid TO DOS Date" Operator="DataTypeCheck" SetFocusOnError="true" Text="*" Type="Date" ValidationGroup="valOrgInfo" ValueToCompare="MM/dd/yyyy"> 
            </asp:CompareValidator>
        </div>
        <div class="col-sm-6 col-md-4 col-lg-3 text-right">
            <asp:Label ID="lblProcedureCode" runat="server" Text="Procedure Code" CssClass="formLabel200" />
        </div>
        <div class="col-sm-9 text-left">
            <asp:TextBox ID="txtProcedureCode" runat="server" CssClass="formField" MaxLength="5" />
            <asp:RequiredFieldValidator ID="rfvProcedureCode" runat="server" ControlToValidate="txtProcedureCode" Display="Dynamic" ErrorMessage="Enter 5 characters is required" SetFocusOnError="true" Text="*" ValidationGroup="valOrgInfo" />

        </div>
        <div style="text-align: center; padding-top: 4px; width: 95%;">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-sm-6 text-right" style="padding-right: 5px;">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click"
                            ToolTip="Search data" />
                        <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBoxFocus" OnClick="btnClear_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
<ajax:CollapsiblePanelExtender ID="cpeRecipientInfo" runat="server" Collapsed="false" TargetControlID="pnlRecipientInfo"
    ExpandControlID="pnlsepRecipientInfo" CollapseControlID="pnlsepRecipientInfo" />
<asp:Panel runat="server" ID="pnlsepRecipientInfo" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerRecipientInfo">
    <span id="sepRecipientInfo" runat="server" class="pageHeader">-Recipient Information</span>
</asp:Panel>
<asp:Panel ID="pnlRecipientInfo" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div class="col-sm-6 col-md-4 col-lg-3 text-right">
        <asp:Label ID="lblRecinfoMedicaidbillNumber" runat="server" Text="Medicaid Billing Number" CssClass="formLabel200" />
    </div>
    <div class="col-sm-9 text-left">
        <asp:TextBox ID="txtRecinfoMedicaidbillNumber" runat="server" CssClass="formField" MaxLength="12" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtRecinfoMedicaidbillNumber" ErrorMessage="* Enter Medicaid Billing Number" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 text-right">
        <asp:Label ID="lblLast" runat="server" Text="Last Name" CssClass="formLabel200" />
    </div>
    <div class="col-sm-9 text-left">
        <asp:TextBox ID="txtLast" runat="server" CssClass="formField" />
    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 text-right">
        <asp:Label ID="lblFirstName" runat="server" Text="First Name, MI" CssClass="formLabel200" />
    </div>
    <div class="col-sm-9 text-left">
        <asp:TextBox ID="txtFirstName" runat="server" CssClass="formField" />
    </div>
                <div class="col-sm-6 col-md-4 col-lg-3 text-right">
                <asp:Label ID="lblDOB" runat="server" Text="Date of Birth" CssClass="formLabel200" />
            </div>
            <div class="col-sm-9 text-left">
                <asp:TextBox ID="txtDOB" runat="server" CssClass="formField" />
                <ajax:CalendarExtender ID="CalendarExtender1" runat="server" Format="MM/dd/yyyy" TargetControlID="txtBirthDate"
                    PopupPosition="BottomRight" CssClass="QstCalendarCSS" PopupButtonID="imgBirthDate" EnabledOnClient="true" />
                <asp:CompareValidator ID="cvDOB1" runat="server" Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDOB" ValidationGroup="VldGrpBirthDate"
                    ErrorMessage="Select a valid To date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy" SetFocusOnError="true"> 
                </asp:CompareValidator>
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" AlternateText="Calendar Icon" />
                <asp:RequiredFieldValidator ID="rfvDOB" runat="server" ControlToValidate="txtDOB"
                    ErrorMessage="To Date is required" Text="*" Display="Dynamic" ValidationGroup="VldGrpBirthDate" />
                <asp:CustomValidator ID="cvDOB" runat="server" ControlToValidate="txtDOB" ErrorMessage="Select a valid Date of Birth."
                    Display="Dynamic" Text="*" ValidationGroup="VldBirthDate" OnServerValidate="ReportDOB_ServerValidate" />
            </div>

    <div class="col-sm-6 col-md-4 col-lg-3 text-right">
        <asp:Label ID="lblDOD" runat="server" Text="Date Of Death" CssClass="formLabel200" />
    </div>
    <div class="col-sm-9 text-left">
        <asp:TextBox ID="txtDOD" runat="server" CssClass="formField" />
        <ajax:CalendarExtender ID="ceDOD" runat="server" TargetControlID="txtDOB" />
        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBirthDate" Display="Dynamic" ErrorMessage="Enter Date of Birth" SetFocusOnError="true" Text="*" ValidationGroup="valOrgInfo" />--%>
        <asp:CompareValidator ID="cvDOD" runat="server" ControlToValidate="txtDOD" Display="Dynamic" ErrorMessage="Select a valid From Death of Date" Operator="DataTypeCheck" SetFocusOnError="true" Text="*" Type="Date" ValidationGroup="valOrgInfo" ValueToCompare="MM/dd/yyyy"> 
        </asp:CompareValidator>
    </div>
    <div class="col-sm-6 col-md-4 col-lg-3 text-right">
        <asp:Label ID="lblSSN" runat="server" Text="SSN" CssClass="formLabel200" />
    </div>
    <div class="col-sm-9 text-left">
        <asp:TextBox ID="txtSSN" runat="server" CssClass="formField" MaxLength="09" />
        <asp:RequiredFieldValidator ID="tfvSSn" runat="server" ControlToValidate="txtSSN" ErrorMessage="* Enter Medicaid Billing Number" Text="*" Display="Dynamic" ValidationGroup="valOwnerInfo"></asp:RequiredFieldValidator>
    </div>

</asp:Panel>
<ajax:CollapsiblePanelExtender ID="CPEBenefitsassplan" runat="server" Collapsed="false" TargetControlID="pnlBenefitsassplan"
    ExpandControlID="pnlsepBenefitsassplan" CollapseControlID="pnlsepBenefitsassplan" />
<asp:Panel runat="server" ID="pnlsepBenefitsassplan" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerBenefitsassplan">
    <span id="sepBenefitsassplan" runat="server" class="pageHeader">-Benefit/Assignment Plan(s)</span>
</asp:Panel>
<asp:Panel ID="pnlBenefitsassplan" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div class="divGrid" style="padding-top: 10px">

        <asp:GridView ID="gvBenefitsassplan" runat="server" Width="80%" AllowSorting="false" CssClass="gridview"
            EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdBenefitsassplan_SelectedIndexChanged" Style="margin-right: 200px">
            <Columns>
               
               
                <asp:BoundField DataField="Benefit_Assignment_Plan" HeaderText="Benefit/Assignment Plan"  />
                 <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date" />
                <asp:BoundField DataField="End_Date" HeaderText="End Date"/>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    </asp:Panel>
<ajax:CollapsiblePanelExtender ID="cpeManagedCarePlan" runat="server" Collapsed="false" TargetControlID="pnlManagedCarePlan"
    ExpandControlID="pnlsepManagedCarePlan" CollapseControlID="pnlsepManagedCarePlan" />
<asp:Panel runat="server" ID="pnlsepManagedCarePlan" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerManagedCarePlan">
    <span id="spnManagedCarePlan" runat="server" class="pageHeader">-Managed Care Plans</span>
</asp:Panel>
<asp:Panel ID="pnlManagedCarePlan" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div class="divManagedCarePlan" style="padding-top: 10px">

        <asp:GridView ID="gvManagedCarePlan" runat="server" Width="80%" AllowSorting="false" CssClass="gridview"
            EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdManagedCarePlan_SelectedIndexChanged" Style="margin-right: 200px">
            <Columns>
                <asp:BoundField DataField="Plan_Name " HeaderText="Plan Name "  />
                 <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date" />
                <asp:BoundField DataField="End_Date" HeaderText="End Date"/>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    </asp:Panel>
<ajax:CollapsiblePanelExtender ID="cpePrivateInsur" runat="server" Collapsed="false" TargetControlID="pnlPrivateInsur"
    ExpandControlID="pnlsepPrivateInsur" CollapseControlID="pnlsepPrivateInsur" />
<asp:Panel runat="server" ID="pnlsepPrivateInsur" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerPrivateInsur">
    <span id="spnPrivateInsur" runat="server" class="pageHeader">-Private Insurance </span>
</asp:Panel>
<asp:Panel ID="pnlPrivateInsur" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div class="divPrivateInsur" style="padding-top: 10px">

        <asp:GridView ID="gvPrivateInsur" runat="server" Width="80%" AllowSorting="false" CssClass="gridview"
            EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdPrivateInsur_SelectedIndexChanged" Style="margin-right: 200px">
            <Columns>
                <asp:BoundField DataField="Plan_Name " HeaderText="Plan Name "  />
                 <asp:BoundField DataField="Carrier_Number" HeaderText="Carrier Number" />
                <asp:BoundField DataField="Policy_Number" HeaderText="Policy Number"/>
                 <asp:BoundField DataField="Insured_Name" HeaderText="Insured’s Name"/>
                 <asp:BoundField DataField="Insured_Relationship_to_Recipient" HeaderText="Insured’s Relationship to Recipient"/>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    </asp:Panel>
<ajax:CollapsiblePanelExtender ID="cpeLockin" runat="server" Collapsed="false" TargetControlID="pnlLockin"
    ExpandControlID="pnlsepLockin" CollapseControlID="pnlsepLockin" />
<asp:Panel runat="server" ID="pnlsepLockin" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerLockin">
    <span id="spnLockin" runat="server" class="pageHeader">-Lock In </span>
</asp:Panel>
<asp:Panel ID="pnlLockin" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div class="divLockin" style="padding-top: 10px">

        <asp:GridView ID="gvLockin" runat="server" Width="80%" AllowSorting="false" CssClass="gridview"
            EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdLockin_SelectedIndexChanged" Style="margin-right: 200px">
            <Columns>
                <asp:BoundField DataField="Lock_In_Plan" HeaderText="Lock-In Plan"  />
                 <asp:BoundField DataField="Lock_In_Type" HeaderText="Lock-In Type" />
                <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date"/>
                 <asp:BoundField DataField="End_Date" HeaderText="End Date"/>
                 <asp:BoundField DataField="Provider_NPI" HeaderText="Provider NPI"/>
                 <asp:BoundField DataField="Provider_Name" HeaderText="Provider Name"/>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    </asp:Panel>
<ajax:CollapsiblePanelExtender ID="cpeMedicare" runat="server" Collapsed="false" TargetControlID="pnlMedicare"
    ExpandControlID="pnlsepMedicare" CollapseControlID="pnlsepMedicare" />
<asp:Panel runat="server" ID="pnlsepMedicare" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerMedicare">
    <span id="spnMedicare" runat="server" class="pageHeader">-Medicare </span>
</asp:Panel>
<asp:Panel ID="pnlMedicare" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div class="divMedicare" style="padding-top: 10px">

        <asp:GridView ID="gvMedicare" runat="server" Width="80%" AllowSorting="false" CssClass="gridview"
            EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdMedicare_SelectedIndexChanged" Style="margin-right: 200px">
            <Columns>
                <asp:BoundField DataField="Coverage" HeaderText="Coverage"  />
                 <asp:BoundField DataField="Lock_In_Type" HeaderText="Lock-In Type" />
                <asp:BoundField DataField="Effective_Date" HeaderText="Effective Date"/>
                 <asp:BoundField DataField="End_Date" HeaderText="End Date"/>
                 <asp:BoundField DataField="Plan_Name" HeaderText="Plan Name"/>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    </asp:Panel>
<ajax:CollapsiblePanelExtender ID="cpeAppealStatus" runat="server" Collapsed="false" TargetControlID="pnlAppealStatus"
    ExpandControlID="pnlsepMedicare" CollapseControlID="pnlsepAppealStatus" />
<asp:Panel runat="server" ID="pnlsepAppealStatus" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerAppealStatus">
    <span id="spnAppealStatus" runat="server" class="pageHeader">-Appeal Status </span>
</asp:Panel>
<asp:Panel ID="pnlAppealStatus" runat="server" Style="min-height: 340px; min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div class="divAppealStatus" style="padding-top: 10px">

        <asp:GridView ID="gvAppealStatus" runat="server" Width="80%" AllowSorting="false" CssClass="gridview"
            EmptyDataText="." AutoGenerateColumns="false" HorizontalAlign="Left" OnSelectedIndexChanged="grdAppealStatus_SelectedIndexChanged" Style="margin-right: 200px">
            <Columns>
                <asp:BoundField DataField="Appeal_Reference_Number" HeaderText="Appeal Reference Number"  />
                 <asp:BoundField DataField="Appeal_Date" HeaderText="Appeal Date" />
                <asp:BoundField DataField="Decision_Date" HeaderText="Decision Date"/>
                 <asp:BoundField DataField="Appeal_Status " HeaderText="Appeal Status "/>
                 <asp:BoundField DataField="Reason_for_Decision" HeaderText="Reason for Decision"/>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
            <HeaderStyle CssClass="gridViewHeader" Width="100px" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" />
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    </asp:Panel>