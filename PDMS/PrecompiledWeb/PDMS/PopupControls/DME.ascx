<%@ control language="C#" autoeventwireup="true" inherits="Pages_DME, App_Web_qlfnt5yf" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/DMEBackgoundCheckProfessionalLicenses.ascx" TagPrefix="uc" TagName="DMEBackgoundCheckProfessionalLicenses" %>
<%@ Register Src="~/PopupControls/DMEProductsAndServices.ascx" TagPrefix="uc" TagName="DMEProductsAndServices" %>
<%@ Register Src="~/PopupControls/DMERegisteredAgent.ascx" TagPrefix="uc" TagName="DMERegisteredAgent" %>
<%@ Register Src="~/PopupControls/DMEProductsAndServicesHistory.ascx" TagPrefix="uc" TagName="DMEProductsAndServicesHistory" %>
<%@ Register Src="~/PopupControls/DMERegisteredAgentHistory.ascx" TagPrefix="uc" TagName="DMERegisteredAgentHistory" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<script type="text/javascript">
    function CollapseExpand(obj) {
        var sp = obj.getElementsByTagName('span')[0];
        var spText = sp.innerHTML;
        var plusIndex = spText.indexOf("+");

        if (plusIndex == 0)
            sp.innerHTML = spText.replace("+", "-");
        else
            sp.innerHTML = spText.replace("-", "+");
    }
    function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9]/g, '');
    }
    function numericOnly(obj) {
        obj.value = obj.value.replace(/[^0-9]/g, '');
    }
</script>
<%--<asp:ValidationSummary ID="valDME" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="DMEValidation" />--%>
<uc:Separator ID="Separator3" runat="server" Header="Registered Agent" />
<br />
<asp:Panel runat="server" ID="Panel2" class="panelContent">
    <div class="divGrid">
        <asp:GridView runat="server" Width="98%" ID="gvRegAgent" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No registered agents found" OnRowCommand="gvRegAgent_RowCommand" AllowSorting="true" DataKeyNames="REG_DME_REGISTERED_AGENT_ID">
            <Columns>
                <asp:BoundField DataField="AGENT_NAME" HeaderText="Registered Agent Name" HtmlEncode="False" />
                <asp:BoundField DataField="COMPANY_NAME" HeaderText="Company Name" />
                <asp:BoundField DataField="PHONE" HeaderText="Phone Number" />
                <asp:BoundField DataField="EMAIL_ADDRESS" HeaderText="Email Address" />
                <asp:TemplateField ItemStyle-Width="2%">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit"  alt="EditButton" runat="server" CommandName="EditDMERegAgentRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
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
        <asp:ImageButton ID="btnAddRegAgent" runat="server" ImageUrl="~/Images/add.png" CommandName="DMERegAgent" OnCommand="btnAddDMEBackgroundChcks_Command" ToolTip="Add" />
         <asp:ImageButton ID="btnAddRegAgentHistory" CommandName="RegAgentHistory" runat="server" ImageUrl="~/Images/history_icon.jpg"
            OnCommand="btnHistory_Click" ToolTip="History" />
    </div>
    <br />
</asp:Panel>
<uc:Separator ID="Separator2" runat="server" Header="Personnel" />
<asp:Panel runat="server" ID="pnlInstructions" class="panelContent">

    <b>Declaration of whether personnel have physical contact with beneficiaries</b>
    <b>Please answer YES or NO to the following questions:</b><br />
    Do (or will) any personnel providing services at this service site:
           <br />
    <br />
    <table id="ParentTable" >

        <tr>
            <td style="width:90%; vertical-align:top;">Enter beneficiaries’ homes to deliver DME products, to fit DME products to patients, or for other purposes?
           <%-- <td style="width:3px">&nbsp;</td>
            <td class="alignLeft" style="padding-bottom: 5px;">--%>
             <br />   <asp:RadioButtonList ID="rblYesNo" BorderStyle="None" CellPadding="0" CellSpacing="0"
                    RepeatDirection="Horizontal" runat="server" RepeatLayout="Table" CssClass="QstRadioList">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="valFNReqd" runat="server" ControlToValidate="rblYesNo" Enabled="true" SetFocusOnError="true"
                    Display="Dynamic" Text="*" ValidationGroup="valProviderInfoHeader" ErrorMessage="* Yes Or No Selection is required."></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>Have physical contact with beneficiaries to ensure the correct fit or teach appropriate use of prosthetics, wheelchairs, or other DME equipment; or have any other physical contact with patients beyond that physical contact routinely made by a cashier or other personnel performing administrative activities.
             <%--<td style="width:3px">&nbsp;</td>
            <td class="alignLeft">--%>
       <br />         <asp:RadioButtonList ID="rblPhysicallyContacted" BorderStyle="None" CellPadding="0" CellSpacing="0"
                    RepeatDirection="Horizontal" runat="server" RepeatLayout="Table" CssClass="QstRadioList">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="rblPhysicallyContacted" Enabled="true" SetFocusOnError="true"
                    Display="Dynamic" Text="*" ValidationGroup="valProviderInfoHeader" ErrorMessage="* Yes Or No Selection is required."></asp:RequiredFieldValidator>
            </td>

        </tr>
    </table>
    <br />
    If you answered "YES" to either of the above two questions, fill in the section below.
           <br />
    <%--<h5>List of personnel with physical contact with beneficiaries</h5>--%>
    Below, list every person in your company at this service location who will have contact with Medicaid recipients in any of the ways described above. For each such person:
           <ul style="list-style-type: disc; line-height: 1.5; ">
               <li>If he or she holds a current professional license as a health care provider, provide a copy of the license.</li>
               <li>If he or she does not currently possess a valid license as a health care provider, provide evidence of a background check.</li>
           </ul>
    <table>
        <tr>
            <td>State the number of copies of professional licenses that are included here</td>
            <td>
                <asp:TextBox ID="txtProfessionalLicenses" runat="server" MaxLength="35" CssClass="formField wd100" onKeyUp="javascript:numericOnly(this);" /></td>

        </tr>
        <tr>
            <td>State the number of criminal background checks that are included here
            </td>
            <td>
                <asp:TextBox ID="txtCriminalBackgroundChecks" runat="server" MaxLength="35" CssClass="formField wd100"  onKeyUp="javascript:numericOnly(this);" /></td>

        </tr>
    </table><br />
    <%--Please upload all the applicable Background Checks and Professional Licenses in the Upload Documents Section below
    <br />
    <br />--%>

   <%-- <div class="divGrid">
        <asp:GridView runat="server" Width="98%" ID="gvDMEBackgroundChcks" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No background checks found" OnRowCommand="gvDMEBackgroundChcks_RowCommand" AllowSorting="true" DataKeyNames="REG_DME_BACKGROUND_CHK_PROFESSIONAL_INFO_ID">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Name" HtmlEncode="False" />
                <asp:BoundField DataField="IsBackgroundCheck" HeaderText="Background Check" />
                <asp:BoundField DataField="IsProfessionalLicense" HeaderText="Professional License" />
                <asp:TemplateField ItemStyle-Width="2%">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit"  alt="EditButton" runat="server" CommandName="EditDMEBackgroundChcksRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
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
    </div>--%>
   
   <%-- <div class="divHistoryAndAdd">
        <asp:ImageButton ID="btnAddDMEBackgroundChcks" runat="server" ImageUrl="~/Images/add.png" CommandName="DMEBackgroundChcks" OnCommand="btnAddDMEBackgroundChcks_Command" ToolTip="Add" />
       
    </div>--%>
    <br />

</asp:Panel>
<br />
<uc:Separator ID="Separator1" runat="server" Header="Products And Services To Be Furnished By Provider" />
<br />
<asp:Panel runat="server" ID="Panel1" class="panelContent">
    <div class="divGrid">
        <asp:GridView runat="server" Width="98%" ID="gvProductsAndServices" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
            EmptyDataText="No Products and Services found" OnRowCommand="gvProductsAndServices_RowCommand" AllowSorting="true" DataKeyNames="REG_ID,DME_PRODUCT_SERVICE_CATEGORY_TYPE_ID">
            <Columns>
                <asp:BoundField DataField="DME_PRODUCT_SERVICE_CATEGORY_TYPE_NAME" HeaderText="Product/Service Category" HtmlEncode="False" />
                <asp:BoundField DataField="DME_PRODUCT_SERVICE_SUB_CATEGORY_TYPE_NAME" HeaderText="Product/Service SubCategories" />

                <asp:TemplateField ItemStyle-Width="2%">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit"  alt="EditButton" runat="server" CommandName="EditDMEProductsServicesRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
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
        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/add.png" CommandName="DMEProductsServices" OnCommand="btnAddDMEBackgroundChcks_Command" ToolTip="Add" />
         <asp:ImageButton ID="btnProductandServiceHistory" CommandName="ProductandServiceHistory" runat="server" ImageUrl="~/Images/history_icon.jpg"
            OnCommand="btnProductandServiceHistory_Click" ToolTip="History" />
    </div>
    <br />

</asp:Panel>
<br />
<uc:Separator ID="ucSep1" runat="server" Header="Qualifications of the DME/POS System Business" />
<br />
<b>Check agencies where any accreditation is held by your DME business:</b><br />
<p>
   <asp:CheckBoxList ID="cblAgencies" runat="server" CssClass="formCheckBox" OnDataBound="cblAgencies_DataBound" OnSelectedIndexChanged="cblAgencies_SelectedIndexChanged" AutoPostBack="true"/><br />
    <asp:TextBox ID="txtOtherReasons" runat="server" CssClass="formField wd100" Visible="false" onKeyUp="javascript:alphanumericOnly(this);" MaxLength="75"></asp:TextBox>
</p>
<ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
    CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" align="center" Style="display: none; width:60%; height:auto;">
    <asp:Panel ID="pnlHeader" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
        <div align="left">
            &nbsp;&nbsp;
            <asp:Label ID="lblTitle" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px" DefaultButton="btnSave">
        <asp:MultiView ID="mltPopup" runat="server">
            <asp:View ID="vwDMEBackgoundCheckProfessionalLicenses" runat="server">
                <div style="text-align: left; padding: 15px; margin-left:30px !important" class="container-fluid">
                 <div class="row">
                <uc:DMEBackgoundCheckProfessionalLicenses ID="ucDMEBackgoundCheckProfessionalLicenses" runat="server" />
                     </div>
                    </div>
            </asp:View>
            <asp:View ID="vwProductsAndServices" runat="server">
                <div style="text-align: left; padding: 15px; margin-left:30px !important" class="container-fluid">
                 <div class="row">
                <uc:DMEProductsAndServices ID="ucDMEProductsAndServices" runat="server" />
                     </div>
                    </div>
            </asp:View>
            <asp:View ID="vwRegAgent" runat="server">
                <div style="text-align: left; padding: 15px; margin-left:30px !important" class="container-fluid">
                 <div class="row">
                <uc:DMERegisteredAgent ID="ucDMERegAgent" runat="server" />
                     </div>
                    </div>
            </asp:View>
             <asp:View ID="vwRegAgentHistory" runat="server">
                 <div style="text-align: left; padding: 15px; margin-left:30px !important" class="container-fluid">
                 <div class="row">
                <uc:DMERegisteredAgentHistory ID="ucDMERegAgentHistory" runat="server" />
                     </div>
                     </div>
            </asp:View>
            <asp:View ID="vwProductandServiceHistory" runat="server">
                 <div style="text-align: left; padding: 15px; margin-left:30px !important" class="container-fluid">
                 <div class="row">
                <uc:DMEProductsAndServicesHistory ID="ucDMEProductsAndServicesHistory" runat="server" />
                     </div>
                     </div>
            </asp:View>
        </asp:MultiView>
    </asp:Panel>
    <table border="0" cellpadding="0" cellspacing="5" align="center" style="padding-bottom: 10px">
        <tr>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" />
            </td>
            <td>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox"
                    CausesValidation="false" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />
