<%@ Control Language="C#" AutoEventWireup="true" Inherits="Pages_OwnerInformation" Codebehind="OwnerInformation.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/OwnerInfo.ascx" TagPrefix="uc" TagName="OwnerInfo" %>
<%@ Register Src="~/PopupControls/OwnerInfoHistory.ascx" TagPrefix="uc" TagName="OwnerInfoHistory" %>
<%@ Register Src="~/PopupControls/RegistrationQuestion.ascx" TagName="Question" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/AdditionalAddresses.ascx" TagName="AdditionalAddresses" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/OwnerIdentifyingInfo.ascx" TagPrefix="uc" TagName="OwnerIdentifyingInfo" %>

<style type="text/css">  
    .divGrid
    {
        width: 100%;
    }
    .gridview
    {
        float: right;
    }
    .gridViewHeader>th>a
    {
        color:White!important;
    }
    .panelOwnerInfo
    {
        padding: 20px 20px 0px 20px;
    }
</style>
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
    function removeDisabled() {
        $("#<%= btnCloseHistory.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    }
</script>

<asp:HiddenField ID="hdnChanged" runat="server" />
<div onmouseover="removeDisabled();">
<div class="enrollment">
Click on the section header to expand or collapse the panel.<br/><br/>
<ajax:CollapsiblePanelExtender ID="cpeInstructions" runat="server" Collapsed="false" TargetControlID="pnlInstructions"
                               ExpandControlID="pnlSepInstructions" CollapseControlID="pnlSepInstructions"/>
<asp:Panel runat="server" ID="pnlSepInstructions" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse" CssClass="OwnerBackcolor">
    <span id="sepInstructions" runat="server" class="pageHeader">- Instructions</span>
    <br/>
</asp:Panel>
<asp:Panel runat="server" ID="pnlInstructions" class="OwnerBackground">
    Completion and submission of this form is a condition of participation, certification or recertification under any of the programs established by Titles V, XVIII, XIX and XX or as a
    condition of approval or renewal of a contractor agreement between the disclosing entity and the secretary of the appropriate state agency under any of the above-title programs.
    A full and accurate disclosure of ownership and financial interest is required. Direct or indirect ownership interest must be reported if it equates to an ownership interest of
    5 percent or more in the Provider. Failure to submit requested information may result in a refusal by the appropriate State agency to enter into an agreement or contract with any such
    institution or termination of existing agreements. This form must be submitted at the time a Provider is initially enrolling, or revalidating, or reenrolling, or whenever there is a
    change in ownership of a Provider, or a material change in the information required by this form and/or upon request by the Department of Health Care Finance (DHCF) or federal agencies.
    The following instructions are intended to clarify certain questions on the form. Instructions are listed in order of question for easy reference.
    <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
        <li>ITEM I</li>
        <li>a. If you are filling this out as a Provider Person, specify the name of the Provider Person. Do not include a name of a contact person. List the Provider Person's national provider identifier(s) (NPI), social security number (SSN) and Medicaid ID number.</li>
        <li>b. List the Provider Entities' doing business as (DBA) name, NPI(s), federal tax identification number(s) (TIN) and Medicaid ID number. This line is for the name of a Provider Entity (i.e. Family Medical Group of Anytown). This line would also be used for the DBA name of an Individual (i.e. John Smith Pediatrics P.C.).</li>
        <li>c. Specify whether your business is operated as: 1) an Individual by yourself; 2) in a group of Provider Persons at the same location or 3) in any other practice organization.</li>
        <li>d. Enter the address of both the Provider Person and the Provider Entity. P.O. Boxes are not acceptable addresses. All practice locations must be listed..</li>
    </ul>
    <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
        <li>ITEM II</li>
        <li>a. List the name, home address, date of birth (DOB), SSN and percentage owned for each person with a direct or indirect ownership or control interest of five (5) percent or more in the Provider Entity. If you are a Provider Person and own 100 percent of your practice, then you would just list yourself. In addition, list the same information for any Subcontractor in which the Provider Person or Provider Entity has direct or indirect ownership or control interest of 5 percent or more.</li>
        <li>b. List whether any of the persons named in II(a) is related to another as a spouse, parent, child or sibling; and</li>
        <li>c. List the name, address and TIN of any other Provider Entity in which a Person with an Ownership or Control Interest in the Provider Entity also has an Ownership or Control Interest. 42 C.F.R. §455.104</li>
    </ul>
    <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
        <li>ITEM III</li>
        <li>a. A Provider Entity must list the name, address, DOB, SSN and TIN for any Subcontractor with whom the Provider Entity has had singular business transactions totaling more than $25,000 during the 12-month period ending on the date of the request; and</li>
        <li>b. A Provider Entity must list any significant business transactions between the Provider Entity and any Subcontractor or Wholly Owned Supplier during the 5-year period ending on the date of the request. 42 C.F.R. §455.105</li>
    </ul>
    <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
        <li>ITEM IV (if you are a sole Provider you will fill out both parts of this item --- </li>
        <li>a) is about your employees </li>
        <li>b) is about yourself</li>
        <li>a. If you are filling out this form for Purpose 1 (i.e. on behalf of the Provider Entity) please list the following:</li>
        <li>1. List the name, home address, DOB and SSN of each Person with an Ownership or Control Interest in the Provider Entity or is an Agent or Managing Employee of the Provider Entity;</li>
        <li>2. Please list the name, home address, DOB and SSN of each Person with an Ownership or Control Interest in the Provider Entity that has been convicted of a criminal offense related to that person's involvement in any program under Medicare, Medicaid or the title XX services program since the inception of those programs. 42 C.F.R. §455.106 Provider Entities shall search the List of Excluded Individuals/Entities (LEIE) each month for the names of the Providers Entities' employees and contractors.</li>
        <li>b. If you are filling out this form for Purpose 2 (i.e. enrollment of a Provider Person) please fill out this section providing information only about yourself.</li>
    </ul>
    Signature: If this form is being completed for a Provider Entity, the signature below MUST be the written signature of a Responsible Party for the business. If the form is being filled out for a Provider Person the person must sign the form.

</asp:Panel>
<br/>
<ajax:CollapsiblePanelExtender ID="cpeDefinitions" runat="server" Collapsed="true" TargetControlID="pnlDefinitions" ExpandControlID="pnlSepDefinitions" CollapseControlID="pnlSepDefinitions"/>
<asp:Panel runat="server" ID="pnlSepDefinitions" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse">
    <span id="sepDefintions" runat="server" class="pageHeader">+ Definitions & Requirements</span>
</asp:Panel>
    <asp:Panel ID="pnlDefinitions" runat="server" class="OwnerBackground">
        <div runat="server" id="divPnlDefinitions">
            <ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
                <li>Federal and state regulations require all Medicaid providers to disclose full and complete information regarding individuals or entities that own, control, represent or manage them.
        This requirement applies to all provider types that are either enrolling or revalidation as an Ohio Medicaid provider — regardless of business structure (large corporation, partnership,
        non-profit or other type of business organization). Providers must disclose the information for owners (direct and indirect), members of Boards of Director and managing employees -
        this information includes an individual’s Social Security number and date of birth.
        Ohio Department of Medicaid disclosure requirements are outlined in <a href="http://codes.ohio.gov/oac/5160-1-17.3">OAC 5160-1-17-3</a>.
        Do not try to bypass this requirement.  Please note, willingly entering a false social security number is a violation of State and Federal law and could result in the rejection of the application. 
           </li></ul>
        </div>
    </asp:Panel>
<br/>
<ajax:CollapsiblePanelExtender ID="cpeOwnInfo" runat="server" Collapsed="true" TargetControlID="pnlOwnInfo" ExpandControlID="pnlSepOwnInfo" CollapseControlID="pnlSepOwnInfo"/>
<asp:Panel runat="server" ID="pnlSepOwnInfo" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse">
    <span id="sepOwnInfo" runat="server" class="pageHeader">+ Owner, Managing Employee and Controlling Interest Information</span>
</asp:Panel>
<asp:Panel runat="server" ID="pnlOwnInfo" CssClass="OwnerBackground">
    <div class="divGrid" style="padding-top: 10px">
        <asp:GridView runat="server" Width="98%" ID="grdOwnerInfo" AutoGenerateColumns="False" HorizontalAlign="Left"
                      CssClass="gridview" EmptyDataText="No owner information found." DataKeyNames="IS_SCREENED" OnRowCommand="grd_RowCommand" OnRowDataBound="grdOwnerInfo_RowDataBound">
            <Columns>
                <asp:BoundField DataField="OWNER_TYPE_NAME" HeaderText="Type"/>
                <asp:BoundField DataField="NAME" HeaderText="Name"/>
                <asp:BoundField DataField="TITLE" HeaderText="Title"/>
                <asp:BoundField DataField="PERCENTAGE_OF_OWNERSHIP" HeaderText="Percentage"/>
                 <asp:BoundField DataField="BEGIN_DATE" HeaderText="Start Date" DataFormatString = "{0:MM/dd/yyyy}"/>
                <asp:BoundField DataField="END_DATE" HeaderText="End Date" DataFormatString = "{0:MM/dd/yyyy}"/>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="OwnerInfo" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                         ImageUrl="~/Images/edit.png" ToolTip="Edit"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="server" CommandName="OwnerInfoDelete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                         ImageUrl="~/Images/cancel.png" ToolTip="Delete"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right"/>
            <HeaderStyle CssClass="gridViewHeader" Width="100px"/>
            <AlternatingRowStyle CssClass="gridViewAltRow"/>
            <RowStyle CssClass="gridViewRow"/>
            <FooterStyle CssClass="gridViewFooter"/>
        </asp:GridView>
    </div>
    <div class="divHistoryAndAdd">
        <asp:ImageButton ID="btnAddOwnerInfo" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="OwnerInfo" ToolTip="Add"/>
        <asp:LinkButton TabIndex="0" ID="btnHistory" CommandName="History" runat="server" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" Text="History" ToolTip="History" Style="color: white; text-decoration: none;">
            History</asp:LinkButton>
    </div>
    <div Class="panelContent">List the name, home address (no P.O. Box addresses), Date of Birth (DOB), Social Security Number (SSN) and percentage owned for each person with a direct or indirect ownership or control interest of 5 percent or more in the provider entity. In addition, list the same information for any subcontractor in which the provider entity has direct or indirect ownership or control interest of 5 percent or more. If you are an individual AND you are a solo practitioner and you own 100 percent of your practice then you would just list yourself as 100% owner.</div>
</asp:Panel>
<br/>

<div role="dialog" aria-live="assertive" aria-labelledby="dialog1Title" aria-hidden="true" id="divDialog1">
    <ajax:modalpopupextender id="mpeHistory" runat="server" backgroundcssclass="modalBackground" cancelcontrolid="btnCloseHistory" popupcontrolid="pnlModalHistory" targetcontrolid="ButtonDummy2" behaviorid="mpeSplHitory" />
    <asp:Panel ID="pnlModalHistory" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 60%;">
        <asp:Panel ID="pnlHeader" runat="server" CssClass="pnlHeader" HorizontalAlign="Left">
            <div align="left">
                &nbsp;&nbsp;
                <h2 id="dialog1Title">
                    <asp:Label ID="lblSpHistoryTitle" runat="server" CssClass="history" ForeColor="White" Text="Owner History" /></h2>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlModalHistoryData" runat="server" Style="margin-right: 10px">
            <div class="container-fluid" style="text-align: left; padding: 15px;">
                <div class="row">
                    <asp:GridView runat="server" Width="98%" ID="grdHistory" AutoGenerateColumns="False" HorizontalAlign="Left"  CssClass="gridview" 
                        EmptyDataText="No entries found."
                        AllowPaging="true" AllowSorting="true" PageSize="10"
                        OnPageIndexChanging="grdHistory_PageIndexChanging" OnSorting="grdHistory_Sorting">
                        <Columns>
                            <asp:BoundField DataField="Operation"           HeaderText="Operation"                    SortExpression="Operation" />
                            <asp:BoundField DataField="MITS_OWNER_ID"            HeaderText="Owner Number"                    SortExpression="MITS_OWNER_ID" />
                            <asp:BoundField DataField="OWNER_TYPE_NAME"           HeaderText="Type"                    SortExpression="OWNER_TYPE_NAME" />
                            <asp:BoundField DataField="provider_title_DESC"           HeaderText="Title"                    SortExpression="provider_title_DESC" />
                            <asp:BoundField DataField="AFFILIATION_TYPE_DESC"           HeaderText="Affiliation Type"                    SortExpression="AFFILIATION_TYPE_DESC" />
                            <asp:BoundField DataField="NAME"           HeaderText="Name"                    SortExpression="NAME" />
                            <asp:BoundField DataField="ADDRESS"           HeaderText="Address"                    SortExpression="ADDRESS" />
                            <asp:BoundField DataField="DOB"           HeaderText="Birth Date"                    SortExpression="DOB" />
                            <asp:BoundField DataField="TAX_ID"           HeaderText="Tax ID"                    SortExpression="TAX_ID" />
                            <asp:BoundField DataField="PERCENTAGE_OF_OWNERSHIP"           HeaderText="Percentage"                    SortExpression="PERCENTAGE_OF_OWNERSHIP" />
                            <asp:BoundField DataField="BEGIN_DATE"           HeaderText="Start Date"                    SortExpression="BEGIN_DATE" DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:BoundField DataField="END_DATE"           HeaderText="End Date"                    SortExpression="END_DATE" DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:BoundField DataField="Username"           HeaderText="Username"                    SortExpression="Username" />
                            <asp:BoundField DataField="DateOfAction"           HeaderText="DateOfAction"                    SortExpression="DateOfAction"  DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False" />
                        </Columns>
                        <PagerStyle             CssClass="gridpager"        HorizontalAlign="Right" />
                        <HeaderStyle            CssClass="gridViewHeader"   Width="100px" />
                        <AlternatingRowStyle    CssClass="gridViewAltRow" />
                        <RowStyle               CssClass="gridViewRow" />
                        <FooterStyle CssClass="gridViewFooter" />
                    </asp:GridView>
                </div>
                <div class="row">
                    <asp:Button ID="btnCloseHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="OK" />
                    <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="grdHistory_Export" />
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Button runat="server" ID="ButtonDummy2" Style="display: none" Text="”ButtonDummy2”" />
</div>
    
<ajax:CollapsiblePanelExtender ID="cpeRealEstate" runat="server" Collapsed="true" TargetControlID="pnlRealEstate" ExpandControlID="pnlSepRealEstate" CollapseControlID="pnlSepRealEstate"/>
<asp:Panel runat="server" ID="pnlSepRealEstate" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse">
    <span id="sepRealEstateInfo" runat="server" class="pageHeader">+ Real Estate Owners</span>
</asp:Panel>

<asp:Panel ID="pnlRealEstate" runat="server" class="OwnerBackground">
    <div class="divGrid" style="padding-top: 10px">
        <asp:GridView runat="server" Width="98%" ID="grdRealEstateOwnerInfo" AutoGenerateColumns="False" HorizontalAlign="Left"
                      CssClass="gridview" EmptyDataText="No Real Estate owner information found." OnRowCommand="grd_RowCommand" OnRowDataBound="grdRealEstateOwnerInfo_RowDataBound">
            <Columns>
                <asp:BoundField DataField="OWNER_TYPE_NAME" HeaderText="Type"/>
                <asp:BoundField DataField="NAME" HeaderText="Name"/>
                <%--<asp:BoundField DataField="TITLE" HeaderText="Title"/>--%>
                <asp:BoundField DataField="PERCENTAGE_OF_OWNERSHIP" HeaderText="Percentage"/>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEditRealEstate" runat="server" CommandName="RealEstateOwnerInfo" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                         ImageUrl="~/Images/edit.png" ToolTip="Edit"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                       <asp:ImageButton ID="btnDeleteRealEstate" runat="server" CommandName="RealEstateOwnerInfoDelete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                         ImageUrl="~/Images/cancel.png" ToolTip="Delete"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right"/>
            <HeaderStyle CssClass="gridViewHeader" Width="100px"/>
            <AlternatingRowStyle CssClass="gridViewAltRow"/>
            <RowStyle CssClass="gridViewRow"/>
            <FooterStyle CssClass="gridViewFooter"/>
        </asp:GridView>
    </div>
    <div class="divHistoryAndAdd">
        <asp:ImageButton ID="imgAddRealEstate" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="RealEstateOwnerInfo" ToolTip="Add"/>
      <%--  <asp:ImageButton ID="ImgHistRealEstate" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="RealEstateOwnerInfoHistory" ToolTip="History" Visible ="false"/>--%>
    </div>
    <div Class="panelContent">Add a new entry for all Real Estate Owners.</div>
</asp:Panel>

<br/>

<ajax:CollapsiblePanelExtender ID="cpeAdditionalDisclosure" runat="server" Collapsed="true" TargetControlID="pnlAdditionalDisclosure" ExpandControlID="pnlSepAdditionalDisclosure" CollapseControlID="pnlSepRealEstate"/>
<asp:Panel runat="server" ID="pnlSepAdditionalDisclosure" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse">
    <span id="sepAdditionalDisclosureInfo" runat="server" class="pageHeader">+ Additional Disclosure</span>
</asp:Panel>

<asp:Panel ID="pnlAdditionalDisclosure" runat="server" class="OwnerBackground">
    <div Class="panelContent">In this section, enter Individuals and Organizations that meet any of the following conditions: </div>
    <div Class="panelContent" style="text-align: left">
        <ol>
            <li>Any Subcontractors where you have had transactions totaling more than $25,000 within the past 12 months.</li>
            <li>Any Subcontractors or Wholly Owned Suppliers where you have had significant business transactions with the past 5 years.</li>
            <li>Any other Provider Entities where your owners also have an ownership or controlling interest. </li>
            <li>Other employees of your organization, not already listed as a Managing Employee that have :
            <ol type="a">    
                <li>been indicted or convicted of a criminal offense related to programs established by Titles XVII, XIX, or XX or </li>    
                <li>been indicted or convicted of a violation of State or Federal Law or</li>    
                <li> been sanctioned by the Medicare Program </li>  
            </ol>
            </li>
        </ol>
    </div>
    <div class="divGrid" style="padding-top: 10px">
        <asp:GridView runat="server" Width="98%" ID="grdAdditionalDisclosureInfo" AutoGenerateColumns="False" HorizontalAlign="Left"
                      CssClass="gridview" EmptyDataText="No Additional Disclosure information found." OnRowCommand="grd_RowCommand" OnRowDataBound="grdAdditionalDisclosureInfo_RowDataBound">
            <Columns>
                <asp:BoundField DataField="OWNER_TYPE_NAME" HeaderText="Type"/>
                <asp:BoundField DataField="NAME" HeaderText="Name"/>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEditAdditionalDisclosure" runat="server" CommandName="AdditionalDisclosureInfo" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                         ImageUrl="~/Images/edit.png" ToolTip="Edit"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                       <%-- <asp:ImageButton ID="btnDeleteRealEstate" runat="server" CommandName="AdditionalDisclosureInfoDelete" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                         ImageUrl="~/Images/cancel.png" ToolTip="Delete"/>--%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="gridpager" HorizontalAlign="Right"/>
            <HeaderStyle CssClass="gridViewHeader" Width="100px"/>
            <AlternatingRowStyle CssClass="gridViewAltRow"/>
            <RowStyle CssClass="gridViewRow"/>
            <FooterStyle CssClass="gridViewFooter"/>
        </asp:GridView>
    </div>
    <div class="divHistoryAndAdd">
        <asp:ImageButton ID="imgAddAdditionalDisclosure" runat="server" ImageUrl="~/Images/add.png" OnCommand="btnAdd_Click" CommandName="AdditionalDisclosureInfo" ToolTip="Add"/>
      <%--  <asp:ImageButton ID="ImgHistRealEstate" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnHistory_Click" CommandName="AdditionalDisclosureInfoHistory" ToolTip="History" Visible ="false"/>--%>
    </div>
</asp:Panel>

<br/>

<ajax:CollapsiblePanelExtender ID="cpeQuestions" runat="server" Collapsed="true" TargetControlID="pnlQuestions" ExpandControlID="pnlSepQuestions" CollapseControlID="pnlSepQuestions"/>
<asp:Panel runat="server" ID="pnlSepQuestions" class="CollapsingSeparator" onclick="javascript:CollapseExpand(this);" ToolTip="Click to Expand/Collapse">
    <span id="sepQuestions" runat="server" class="pageHeader">+ Questions</span>
</asp:Panel>
<asp:Panel ID="pnlQuestions" runat="server" CssClass="OwnerBackground">
    <div class="panelOwnerInfo">
        <uc:Question ID="ucQ01" runat="server" Option="1" QuestionTypeId="Q01" Section="Questions"/><br/>
        <uc:Question ID="ucQ02" runat="server" Option="2" QuestionTypeId="Q02" Section="Questions"/><br/>
        <uc:Question ID="ucQ04" runat="server" Option="4" QuestionTypeId="Q04" Section="Questions"/><br/>
        <uc:Question ID="ucQ05" runat="server" Option="5" QuestionTypeId="Q05" Section="Questions"/><br/>
        <uc:Question ID="ucQ06" runat="server" Option="6" QuestionTypeId="Q06" Section="Questions"/><br/>
        <uc:Question ID="ucQ07" runat="server" Option="7" QuestionTypeId="Q07" Section="Questions"/><br/>
        <uc:Question ID="ucQ09" runat="server" Option="9" QuestionTypeId="Q09" Section="Questions"/><br/>
        <uc:Question ID="ucQ13" runat="server" Option="13" QuestionTypeId="Q13" Section="Questions"/><br/>
    </div>
</asp:Panel>
</div>

<ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
    CancelControlID="btnCancel" BackgroundCssClass="modalBackground" >
</ajax:ModalPopupExtender>

<asp:Panel ID="pnlModal" runat="server" CssClass="ownerModalPopup" align="center" 
    style="display: none;">
    <asp:Panel ID="pnlHeaderMpe" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
        <div style="text-align: left";>&nbsp;&nbsp;
            <asp:Label ID="lblTitle" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" />
        </div>
    </asp:Panel>  
    <asp:Panel ID="pnlMain" runat="server" Style="margin-right:10px" DefaultButton="btnSave">
        <asp:MultiView ID="mltPopup" runat="server">
            <asp:View ID="vwOwnerInfo" runat="server">
              <div style="text-align: left; padding: 15px" class="container-fluid">
                 <div class="row">
                <uc:OwnerInfo ID="ucOwnerInfo" runat="server" />
                 </div>
              </div>
            </asp:View>
            <asp:View ID="vwOwnerInfoHistory" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                 <div class="row">
                <uc:OwnerInfoHistory ID="ucOwnerInfoHistory" runat="server" />
                     </div>
              </div>
            </asp:View>
            <asp:View ID="vwRealEstateInfo" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                    <div class="row">
                        <uc:OwnerInfo ID="ucRealEstateInfo" runat="server" />
                    </div>
                </div>
            </asp:View>         
            

            <asp:View ID="vwAdditionalAddress" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                 <div class="row">
                <uc:AdditionalAddresses ID="ucAdditionalAddresses" runat="server" />
                 </div>
              </div>
            </asp:View>
            <asp:View ID="vwAdditionalDisclosureInfo" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                    <div class="row">
                        <uc:OwnerInfo ID="ucAdditionalDisclosureInfo" runat="server" />
                    </div>
                </div>
            </asp:View>
            <asp:View ID ="vwOwnerIdentifyingInfo" runat="server">
                <div style="text-align: left; padding: 15px" class="container-fluid">
                 <div class="row">
                <uc:OwnerIdentifyingInfo ID="ucOwnerIdentifyingInfo" runat="server" />
                     </div>
              </div>
            </asp:View>
            
        </asp:MultiView>
    </asp:Panel>
    <div class="row text-center" style="padding-right: 10px;">
              <asp:Button id="btnSave" runat="server" Text="Save" CssClass="buttonBox" onclick="btnSave_Click" CausesValidation="true" />
                <asp:Button id="btnCancel"  runat="server" Text="Cancel" CssClass="buttonBox" 
                    CausesValidation="false" />
    </div>
    <br />    
</asp:Panel>

<div style="display: none;">
    <telerik:RadGrid ID="grdExportHistory" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
        AutoGenerateColumns="false" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
            <Excel Format="Biff" />
        </ExportSettings>
        <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
            DataKeyNames="INDEX, DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
            <Columns>
                <telerik:GridBoundColumn DataField="Operation"           HeaderText="Operation"                    SortExpression="Operation" />
                <telerik:GridBoundColumn DataField="MITS_OWNER_ID"            HeaderText="Owner Number"                    SortExpression="MITS_OWNER_ID" />
                <telerik:GridBoundColumn DataField="OWNER_TYPE_NAME"           HeaderText="Type"                    SortExpression="OWNER_TYPE_NAME" />
                <telerik:GridBoundColumn DataField="provider_title_DESC"           HeaderText="Title"                    SortExpression="provider_title_DESC" />
                <telerik:GridBoundColumn DataField="AFFILIATION_TYPE_DESC"           HeaderText="Affiliation Type"                    SortExpression="AFFILIATION_TYPE_DESC" />
                <telerik:GridBoundColumn DataField="NAME"           HeaderText="Name"                    SortExpression="NAME" />
                <telerik:GridBoundColumn DataField="ADDRESS"           HeaderText="Address"                    SortExpression="ADDRESS" />
                <telerik:GridBoundColumn DataField="DOB"           HeaderText="Birth Date"                    SortExpression="DOB" />
                <telerik:GridBoundColumn DataField="TAX_ID"           HeaderText="Tax ID"                    SortExpression="TAX_ID" />
                <telerik:GridBoundColumn DataField="PERCENTAGE_OF_OWNERSHIP"           HeaderText="Percentage"                    SortExpression="PERCENTAGE_OF_OWNERSHIP" />
                <telerik:GridBoundColumn DataField="BEGIN_DATE"           HeaderText="Start Date"                    SortExpression="BEGIN_DATE" DataFormatString="{0:MM/dd/yyyy}" />
                <telerik:GridBoundColumn DataField="END_DATE"           HeaderText="End Date"                    SortExpression="END_DATE" DataFormatString="{0:MM/dd/yyyy}" />
                <telerik:GridBoundColumn DataField="Username"           HeaderText="Username"                    SortExpression="Username" />
                <telerik:GridBoundColumn DataField="DateOfAction"           HeaderText="DateOfAction"                    SortExpression="DateOfAction"  DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>

<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy"/>
<asp:HiddenField ID="hdnOwnerType" runat="server" Value="" />


</div>