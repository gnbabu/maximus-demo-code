<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_ClosureNotice" Codebehind="ClosureNotice.ascx.cs" %>


<%@ Register Src="~/UserControls/FormField.ascx" TagPrefix="uc" TagName="FormField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>
<style type="text/css">
    .closureNoticePagerStyle
    {
        margin-right: 0;
    }
</style>

<div class="enrollment">

    <span class="boxLabel">Closure History</span><br />
	<asp:UpdatePanel ID="upGrd" runat="server" UpdateMode="Conditional">
		<ContentTemplate>

		<asp:GridView runat="server" Width="98%" ID="grd" AutoGenerateColumns="False"  HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No entries found.">
			<Columns>
				<asp:BoundField DataField="Risk_Alert_Date" HeaderText="Date of Risk Alert" SortExpression="Risk_Alert_Date" DataFormatString="{0:MM/dd/yyyy}" />
				<asp:BoundField DataField="Proposed_Effective_Date" HeaderText="Proposed Effective Date" SortExpression="Proposed_Effective_Date" DataFormatString="{0:MM/dd/yyyy}" />
				<asp:BoundField DataField="Review_Type" HeaderText="Review Type" SortExpression="Review_Type" />
				<asp:TemplateField HeaderText="Risk Alert Removed">
					<ItemTemplate>
						<asp:Label ID="lblIndicationOfSuccessor" runat="server" Text='<%# (Convert.ToBoolean(Eval("Risk_Alert_Removed")) == true) ? "Y" : "N" %>'></asp:Label>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:BoundField DataField="Closure_Effective_Date" HeaderText="Effective Date" SortExpression="Closure_Effective_Date" DataFormatString="{0:MM/dd/yyyy}" />
			</Columns>
			<HeaderStyle CssClass="gridViewHeader" Width="100px" />
			<AlternatingRowStyle CssClass="gridViewAltRow" />
			<RowStyle CssClass="gridViewRow" />
			<FooterStyle CssClass="gridViewFooter" />
		</asp:GridView>
		<asp:DataList ID="dlPager" CellPadding="5" RepeatDirection="Horizontal" runat="server" OnItemCommand="dlPager_ItemCommand" RepeatColumns="20" CssClass="closureNoticePagerStyle">
			<ItemStyle Wrap="true" />
			<ItemTemplate>
				<asp:LinkButton Enabled='<%#Eval("Enabled") %>' runat="server" ID="lnkPageNo" Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' CommandName="PageNo"></asp:LinkButton>
			</ItemTemplate>
		</asp:DataList>
		</ContentTemplate>
	</asp:UpdatePanel>

 <asp:Panel runat="server" ID="pnl45DayNoticeChangeOfOperator" >
    <uc1:SectHd runat="server" ID="sep45DayNoticeChangeOfOperator" Header="Closure Risk Alert" />
    </asp:Panel>
        <div id="closureNiticeInoutControls" runat="server">
                 <asp:Label ID="lblerrormsg" runat="server" CssClass="failureNotification" />
			   <div class="row completeFields" >
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblDateofRiskAlert" runat="server" Text="Date of Risk Alert *" CssClass="formLabel200" />
					</div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtDateofRiskAlert" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtDateofRiskAlert" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvdateRiskAlert" runat="server" ControlToValidate="txtDateofRiskAlert"
                                                ErrorMessage="* Date of Risk Alert is required." ToolTip="Date of Risk Alert is Required."
                                                ValidationGroup="valClosureNotice" SetFocusOnError="true">Required</asp:RequiredFieldValidator>     
                </div>
                </div>

              <div class="row completeFields" >
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblEffectiveDateCHOP" runat="server" Text="Proposed Effective Date *" CssClass="formLabel200" />
					</div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtEffectiveDateCHOP" runat="server" CssClass="formField"  /><ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="txtEffectiveDateCHOP" runat="server" />
                    <asp:RequiredFieldValidator ID="RfvProposedEffectiveDate" runat="server" ControlToValidate="txtEffectiveDateCHOP"
                                                ErrorMessage="* Proposed Effective Date is required." ToolTip="Proposed Effective Date is required."
                                                ValidationGroup="valClosureNotice" SetFocusOnError="true">Required</asp:RequiredFieldValidator> 
                      <asp:CustomValidator ID="cvProposedEffectiveDate" runat="server" OnServerValidate="cvProposedEffectiveDate_ServerValidate" ControlToValidate="txtEffectiveDateCHOP" Display="Static" ValidationGroup="valClosureNotice"
                                    ErrorMessage="* Proposted Effective Date must be less than one year from today and must be greater than enrollment efffective date." Text="*" />
                </div>
                </div>

             <div class="row completeFields" >
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblReviewType" runat="server" Text="Review Type *" CssClass="formLabel200" />
					</div>
                <div class="col-sm-8">
                      <asp:DropDownList ID="ddlReviewType" runat="server" CssClass="formField" >
                </asp:DropDownList>
                 <asp:RequiredFieldValidator ID="RfvReviewType" runat="server" ControlToValidate="ddlReviewType"
                                                ErrorMessage="* Review Type is required." ToolTip="Review Type is required."
                                                ValidationGroup="valClosureNotice" SetFocusOnError="true">Required</asp:RequiredFieldValidator>      
                </div>
                </div>

            <div class="row completeFields" >
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblEffectiveDate" runat="server" Text="Effective Date" CssClass="formLabel200" />
					</div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtEffectiveDate" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="CalendarExtender3" TargetControlID="txtEffectiveDate" runat="server" />
                     
                </div>
                </div>

		</div>
		

    <asp:PlaceHolder runat="server" ID="PlaceholderUploadClosureNotice" Visible="true"></asp:PlaceHolder>
<%--    <asp:Button ID="saveNotice" runat="server" OnClick="saveNotice_Click" />--%>
</div>



