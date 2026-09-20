<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ChangeOperatorInfo, App_Web_tiu3g34i" %>

<%@ Register Src="~/UserControls/FormField.ascx" TagPrefix="uc" TagName="FormField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>

<div class="enrollment">
 <asp:Panel runat="server" ID="pnlChangeOperatorInfo" >
    <uc1:SectHd runat="server" ID="sepChangeOperatorInfo" Header="Change of Operator Information" />
    </asp:Panel>

<asp:UpdatePanel ID="upChangeOperatorInfo" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <div>
                <div class="row completeFields" >
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblCHOPType" runat="server" Text="CHOP Type" CssClass="formLabel200" />
					</div>
                <div class="col-sm-8">
                      <asp:DropDownList ID="ddlCHOPType" runat="server" CssClass="formField" >
                </asp:DropDownList>
                           
                </div>
                </div>

			   <div class="row completeFields" >
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblExitingOperatorMedicaidID" runat="server" Text="Exiting Operator Medicaid ID" CssClass="formLabel200" />
					</div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtExitingOperatorMedicaidIDt" runat="server" CssClass="formField" ReadOnly="True" />
                </div>
                </div>

            <div class="row completeFields" >
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblPurchasePrice" runat="server" Text="Purchase Price " CssClass="formLabel200" />
					</div>
                <div class="col-sm-8">
                    <telerik:RadNumericTextBox ID="txtPurchasePrice" CssClass="formField" Skin="" runat="server" Culture="en-US" Type="Currency">
                    </telerik:RadNumericTextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtPurchasePrice" 
                        ErrorMessage="Please Enter Only Numbers" ForeColor="Red" ValidationExpression="(?<=^| )\d+(\.\d+)?(?=$| )"></asp:RegularExpressionValidator>
                </div>
                </div>

             <div class="row completeFields" >
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblSubLeaseAmount" runat="server" Text="Sub-lease Amount " CssClass="formLabel200" />
					</div>
                <div class="col-sm-8">
                    <telerik:RadNumericTextBox ID="txtSubLeaseAmount" CssClass="formField" Skin="" runat="server" Culture="en-US" Type="Currency">
                    </telerik:RadNumericTextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtSubLeaseAmount" 
                        ErrorMessage="Please Enter Only Numbers" ForeColor="Red" ValidationExpression="(?<=^| )\d+(\.\d+)?(?=$| )"></asp:RegularExpressionValidator>
                </div>
                </div>

               <div class="row completeFields" >
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lbltotalinitAnnualMasterAmount" runat="server" Text="Total Initial Annual Master Lease Amount" CssClass="formLabel300" />
					</div>
                <div class="col-sm-8">
                    <telerik:RadNumericTextBox ID="txtTotalInitAnnualMasterAmount" CssClass="formField" Skin="" runat="server" Culture="en-US" Type="Currency">
                    </telerik:RadNumericTextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtTotalInitAnnualMasterAmount" 
                        ErrorMessage="Please Enter Only Numbers" ForeColor="Red" ValidationExpression="(?<=^| )\d+(\.\d+)?(?=$| )"></asp:RegularExpressionValidator>
                </div>
                </div>

            <div class="row completeFields" >
                <div class="col-sm-4 text-right">
                    <asp:Label ID="lblEffectiveDateCHOP" runat="server" Text="Effective Date of CHOP" CssClass="formLabel200" />
					</div>
                <div class="col-sm-8">
                    <asp:TextBox ID="txtEffectiveDateCHOP" runat="server" CssClass="formField"  /><ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="txtEffectiveDateCHOP" runat="server" />
                           
                </div>
                </div>

		</div>
		<asp:HiddenField ID="hdnChopID" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

</div>




