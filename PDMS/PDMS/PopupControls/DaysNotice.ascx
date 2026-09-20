<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControlsDaysNotice" Codebehind="DaysNotice.ascx.cs" %>


<%@ Register Src="~/UserControls/FormField.ascx" TagPrefix="uc" TagName="FormField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<style type="text/css">
    .daysNoticePagerStyle
    {
        margin-right: 0;
    }
</style>

<div class="enrollment">

    <span class="boxLabel">CHOP History</span><br />
        <asp:UpdatePanel ID="upGrd" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

            <asp:GridView runat="server" Width="98%" ID="grd" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found.">
                <Columns>
                    <asp:BoundField DataField="Risk_Alert_Date" HeaderText="Date of Risk Alert" SortExpression="Risk_Alert_Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="Chop_Effective_Date" HeaderText="Effective Date of CHOP" SortExpression="Chop_Effective_Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="Review_Type" HeaderText="Review Type" SortExpression="Review_Type" />
                    <asp:BoundField DataField="Risk_Alert_Status_End_Date" HeaderText="End Date of Risk Alert Status" SortExpression="Risk_Alert_Status_End_Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="Entering_Provider_Name" HeaderText="Entering Provider Name" SortExpression="Entering_Provider_Name" />
                    <asp:BoundField DataField="Owner_Name_from_Cost_Report" HeaderText="Owner Name from Cost Report" SortExpression="Owner_Name_from_Cost_Report" />

                    <asp:TemplateField HeaderText="Indication of Successor Liability">
                        <ItemTemplate>
                            <asp:Label ID="lblIndicationOfSuccessor" runat="server" Text='<%# (Convert.ToBoolean(Eval("Indication_of_Successor_Liability")) == true) ? "Y" : "N" %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="CHOP Withdrawn">
                        <ItemTemplate>
                            <asp:Label ID="lblChopWithdrawn" runat="server" Text='<%# (Convert.ToBoolean(Eval("CHOP_Withdrawn")) == true) ? "Y" : "N" %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="CHOP_Final_Date" HeaderText="Final Date of CHOP" SortExpression="CHOP_Final_Date" DataFormatString="{0:MM/dd/yyyy}" />
                </Columns>
                <HeaderStyle CssClass="gridViewHeader" Width="100px" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
            <asp:DataList ID="dlPager" CellPadding="5" RepeatDirection="Horizontal" runat="server" OnItemCommand="dlPager_ItemCommand" RepeatColumns="20" CssClass="daysNoticePagerStyle">
                <ItemStyle Wrap="true" />
                <ItemTemplate>
                    <asp:LinkButton Enabled='<%#Eval("Enabled") %>' runat="server" ID="lnkPageNo" Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo"></asp:LinkButton>
                </ItemTemplate>
            </asp:DataList>
            </ContentTemplate>
        </asp:UpdatePanel>

    <asp:Panel runat="server" ID="pnl45DayNoticeChangeOfOperator">
        <uc1:SectHd runat="server" ID="sep45DayNoticeChangeOfOperator" Header="Upload 45 Day Notice of Change of Operator" />
    </asp:Panel>
    <div id="dayNoticeInputControls" runat="server">
        <div>
            <asp:ValidationSummary ID="vsdayNotice" runat="server" DisplayMode="List" ValidationGroup="valdayNotice" ForeColor="Red" />
        </div>
        <div class="row completeFields">
            <div class="col-sm-4 text-right">
                <asp:Label ID="lblDateofRiskAlert" runat="server" Text="Date of Risk Alert*" CssClass="formLabel200" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtDateofRiskAlert" runat="server" aria-label="Date of Risk" aria-Required="true" CssClass="formField" />
                <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtDateofRiskAlert" runat="server" />
                <asp:RequiredFieldValidator runat="server" ID="reqDateofRiskAlert" ControlToValidate="txtDateofRiskAlert" 
ErrorMessage="*Enter the Date of Risk Alert" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valdayNotice" Fore-Color="Red" />
                <asp:CompareValidator ID="CompareValidator2" runat="server" ValidationGroup="valdayNotice"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateofRiskAlert"
                    ErrorMessage="Select a valid Date of Risk Alert in the format mm/dd/yyy" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true" />
            </div>
        </div>

        <div class="row completeFields">
            <div class="col-sm-4 text-right">
                <asp:Label ID="lblEffectiveDateCHOP" runat="server" Text="Effective Date of CHOP*" CssClass="formLabel200" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtEffectiveDateCHOP" runat="server" aria-label="Effective Date" aria-required="true" CssClass="formField" />
                <ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="txtEffectiveDateCHOP" runat="server" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtEffectiveDateCHOP" ErrorMessage="*Enter the Effective Date of CHOP" Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valdayNotice" />
                <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="valdayNotice"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEffectiveDateCHOP"
                    ErrorMessage="Select a valid Effective Date of CHOP in the format mm/dd/yyy" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true" />

                <asp:CompareValidator ID="DateCompareValidator" runat="server" ControlToCompare="txtEffectiveDateCHOP" ValidationGroup="valdayNotice"
                    ControlToValidate="txtFinalChopDate" Operator="Equal" Type="Date" ValueToCompare="MM/dd/yyyy" Display="Dynamic" Text="*"
                    ErrorMessage="Effective Date of CHOP should be equal to Final Date of CHOP." SetFocusOnError="true" ></asp:CompareValidator>

            </div>
        </div>

        <div class="row completeFields">
            <div class="col-sm-4 text-right">
                <asp:Label ID="lblReviewType" runat="server" Text="Review Type*" CssClass="formLabel200" />
            </div>
            <div class="col-sm-8">
                <asp:DropDownList ID="ddlReviewType" runat="server" aria-label="Review Type" aria-required="true" CssClass="formField">
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="rfvReviewType" ValidationGroup="valdayNotice"
                    ControlToValidate="ddlReviewType" ErrorMessage="*Select a Review Type" Text="*" Display="Dynamic"
                    SetFocusOnError="true" InitialValue="" />
            </div>
        </div>

        <div class="row completeFields">
            <div class="col-sm-4 text-right">
                <asp:Label ID="lblEndDateRiskAlertStatus" runat="server" Text="End Date of Risk Alert Status*" CssClass="formLabel200" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtEndDateRiskAlertStatus" runat="server" aria-Label="End Date Risk Alert Status" aria-required="true" CssClass="formField" />
                <ajax:CalendarExtender ID="CalendarExtender3" TargetControlID="txtEndDateRiskAlertStatus" runat="server" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="txtEndDateRiskAlertStatus" ErrorMessage="*Enter the End Date of Risk Alert Status" 
                    Text="*" Display="Dynamic" SetFocusOnError="true" ValidationGroup="valdayNotice" />
                <asp:CompareValidator ID="CompareValidator3" runat="server" ValidationGroup="valdayNotice"
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDateRiskAlertStatus"
                    ErrorMessage="Select a valid End Date of Risk Alert Status in the format mm/dd/yyy" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true" />
            </div>
        </div>

        <div class="row completeFields">
            <div class="col-sm-4 text-right">
                <asp:Label ID="lblEnteringProviderName" runat="server" Text="Entering Provider Name*" CssClass="formLabel200" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtEnteringProviderName" aria-Label="Entering Provider Name" aria-Required="true" runat="server" CssClass="formField" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="valdayNotice"
                    ControlToValidate="txtEnteringProviderName" ErrorMessage="*Enter the Entering Provider Name" Text="*" Display="Dynamic"
                    SetFocusOnError="true" InitialValue="" />
            </div>
        </div>

        <div class="row completeFields">
            <div class="col-sm-4 text-right">
                <asp:Label ID="lblEnteringProviderEmail" runat="server" Text="Entering Provider Email*" CssClass="formLabel200" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtEnteringProviderEmail" aria-label="Entering Provider Email" aria-required="true"  runat="server"  CssClass="formField" />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid Email" 
                    ControlToValidate="txtEnteringProviderEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ValidationGroup="valdayNotice"
                    ControlToValidate="txtEnteringProviderEmail" ErrorMessage="*Enter the Entering Provider Email" Text="*" Display="Dynamic"
                    SetFocusOnError="true" InitialValue="" />
            </div>
        </div>

        <div class="row completeFields">
            <div class="col-sm-4 text-right">
                <asp:Label ID="lblOwnerNameFromCostReport" runat="server" Text="Owner Name From Cost Report*" CssClass="formLabel200" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtOwnerNameFromCostReport" aria-Label="Owner Name From Cost Report" aria-Required="true" runat="server" CssClass="formField" />
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ValidationGroup="valdayNotice"
                    ControlToValidate="txtOwnerNameFromCostReport" ErrorMessage="*Enter the Owner Name From Cost Report" Text="*" Display="Dynamic"
                    SetFocusOnError="true" InitialValue="" />
            </div>
        </div>


        <div class="row completeFields">
            <div class="col-sm-4 text-right">
                <asp:Label ID="lblIndicationofSuccessorLiability" AssociatedControlID="ChkBoxIndicationofSuccessorLiability" runat="server" Text="Indication of Successor Liability" CssClass="formLabel200" />
            </div>
            <div class="col-sm-8">
                <asp:CheckBox ID="ChkBoxIndicationofSuccessorLiability" runat="server" />
            </div>
        </div>

        <div class="row completeFields">
            <div class="col-sm-4 text-right">
                <asp:Label ID="lblCHOPWithdrawn" runat="server" AssociatedControlID="ChkBoxCHOPWithdrawn" Text="CHOP Withdrawn" CssClass="formLabel200" />
            </div>
            <div class="col-sm-8">
                <asp:CheckBox ID="ChkBoxCHOPWithdrawn" runat="server" OnCheckedChanged="ChkBoxCHOPWithdrawn_CheckedChanged" AutoPostBack="true" />
            </div>
        </div>
        <div class="row completeFields">
            <div class="col-sm-4 text-right">
                <asp:Label ID="lblFinalChopDate" runat="server" Text="Final Date of CHOP" CssClass="formLabel200" />
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtFinalChopDate" aria-label="Final Chop Date" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="CalendarExtender4" TargetControlID="txtFinalChopDate" runat="server" />

            </div>
        </div>


    </div>

    <%--<asp:UpdatePanel ID="up45DayNoticeChangeOfOperatorUpload" runat="server" UpdateMode="Conditional">
    <asp:PlaceHolder runat="server" ID="Placeholder1"></asp:PlaceHolder>
    </asp:UpdatePanel>--%>
    <asp:PlaceHolder runat="server" ID="PlaceholderUpload45DayNotice"></asp:PlaceHolder>
</div>



