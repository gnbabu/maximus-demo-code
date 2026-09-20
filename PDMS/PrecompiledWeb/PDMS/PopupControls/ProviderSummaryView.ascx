<%@ control language="C#" autoeventwireup="true" inherits="Views_ProviderSummaryView, App_Web_av5ll3zk" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
 <%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
    <div class="boxPanelFull2 wdAll" style="height:auto;min-height:100px;">
        <div class="boxPanelHeader">Provider Summary</div>
            <div class="group-hdg">                                        
                <span class="formLabel formLabelAuto">Tax ID:</span>
                <asp:Label ID="lblTaxID" runat="server" CssClass="formFieldReadOnly formFieldAuto" Text="" />
            </div>
        
            <div class="boxPanelData" >
                <div id="divReferrals" runat="server">
                <div  class="gridCaption">My Outstanding Program Service Referrals</div>
                <mms:SortablePagingGridView runat="server" Width="100%"  ID="gvReferrals" AutoGenerateSelectButton="false" AutoGenerateColumns="False" 
                    CssClass="gridViewSmallFont" EmptyDataText="No referrals found." ShowHeaderWhenEmpty="true"
                    GridViewSortColumn="ProviderName" GridViewSortDirection="Ascending"  AllowPaging="true"
                    OnSorting="gvReferrals_Sorting" OnRowDataBound="gvReferrals_RowDataBound"
                    OnPageIndexChanging="gvReferrals_PageIndexChanging" OnRowCommand="gvReferrals_RowCommand" 
                    DataKeyNames="RegID, PartyID,ReferralID,ReferralStatusID,ZipCode,ZipExt,ReferralType"  >
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <asp:LinkButton  ToolTip="Accept Referral" ID="btnAcceptReferral" runat="server" CommandName="AcceptReferral" 
                                    CommandArgument='<%# ((GridViewRow)Container).RowIndex  %>' Text="Select" />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="ProviderName" HeaderText="Provider" SortExpression="ProviderName" />
                        <asp:BoundField DataField="LocationZip" HeaderText="Location" SortExpression="LocationZip" />
                        <asp:BoundField DataField="ReferralProviderStatus" HeaderText="Status" SortExpression="ReferralProviderStatus" />
                        <asp:BoundField DataField="ReviewStatusName" HeaderText="Review Status" SortExpression="ReviewStatusName" />
                        <asp:BoundField DataField="EndDate" HeaderText="Re-Enrollment Due Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False"
                             SortExpression="EndDate" />
                    </Columns>
                </mms:SortablePagingGridView>
                <div class="grid-hint">Select from the list of available referrals to begin the registration process.</div>
                </div>
                <br />

            <div class="gridCaption">My Providers</div>
                <mms:SortablePagingGridView runat="server" Width="100%"  ID="gvMyProviders" AutoGenerateSelectButton="false" AutoGenerateColumns="False" 
                    EmptyDataText="No providers found." ShowHeaderWhenEmpty="true" CssClass="gridViewSmallFont"
                    GridViewSortColumn="ProviderName" GridViewSortDirection="Ascending"   AllowPaging="true"
                    OnSorting="gvMyProviders_Sorting" OnPageIndexChanging="gvMyProviders_PageIndexChanging"  
                    OnRowCommand="gvMyProviders_RowCommand" OnSelectedIndexChanged="gvMyProviders_SelectedIndexChanged" OnRowDataBound="gvMyProviders_RowDataBound"
                    DataKeyNames="UserID, RegID, PartyID"  >
                    <Columns>
                        <asp:TemplateField HeaderText="Provider"  SortExpression="ProviderName">
                            <ItemTemplate>
                                <asp:LinkButton ToolTip="Manage Provider" ID="btnManageProvider" runat="server" CommandName="ManageProvider" 
                                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' Text='<%# Eval("ProviderName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="RegistrationStatusType" HeaderText="Status" SortExpression="RegistrationStatusType" />
                        <asp:BoundField DataField="ProviderTypeName" HeaderText="Provider Type" SortExpression="ProviderTypeName" />
                        <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
                        <asp:BoundField DataField="MedicaidID" HeaderText="Medicaid ID" SortExpression="MedicaidID" />
                        <asp:BoundField DataField="SpecialtyTypeName" HeaderText="Specialty" SortExpression="SpecialtyTypeName" />
                        <asp:BoundField DataField="PracticeLocationZip" HeaderText="Location" SortExpression="PracticeLocationZip" />
                        <asp:BoundField DataField="EffectiveDateTime" HeaderText="Effective Date" DataFormatString="{0:MM/dd/yy}" SortExpression="EffectiveDateTime" />
                        <asp:BoundField DataField="SubmitDateTime" HeaderText="Submit Date" DataFormatString="{0:MM/dd/yy}" SortExpression="SubmitDateTime"  />
                        <asp:BoundField DataField="RevalidationDate" HeaderText="Re-Enrollment Due Date" DataFormatString="{0:MM/dd/yy}" SortExpression="RevalidationDate" />
                    </Columns>
                </mms:SortablePagingGridView>
                <asp:RadioButtonList ID="rblApplicationTypes" runat="server" RepeatDirection="Vertical" DataValueField="APPLICATION_TYPE_ID" DataTextField="APPLICATION_NAME_DESC">

                </asp:RadioButtonList>
	            <div class="btnBox"> 
                    <asp:RequiredFieldValidator runat="server" ID="rfvAddProvider"   ControlToValidate="rblApplicationTypes" ErrorMessage="* Please select an application type." SetFocusOnError="true"  ValidationGroup="valNewAppType"></asp:RequiredFieldValidator>

                    <asp:Button id="btnAddProvider" runat="server" Text="Begin New Enrollment" CssClass="buttonBox wd200" ToolTip="Begin New Enrollment" OnClick="btnAddProvider_Click"  ValidationGroup="valNewAppType"/>
                   
                </div>

                <br /><br />

                <div class="gridCaption" style="display:none;">
                <div class="gridCaption" style="display:none;">My Group Member Profiles</div>
                <mms:SortablePagingGridView runat="server" Width="100%"  ID="gvGroupMbr" AutoGenerateSelectButton="false" AutoGenerateColumns="False" 
                    CssClass="gridViewSmallFont" EmptyDataText="No group member profiles found." ShowHeaderWhenEmpty="true"
                    GridViewSortColumn="ProviderName" GridViewSortDirection="Ascending"  AllowPaging="true"
                    OnSorting="gvGroupMbr_Sorting" OnPageIndexChanging="gvGroupMbr_PageIndexChanging"  
                    OnRowCommand="gvGroupMbr_RowCommand" OnSelectedIndexChanged="gvGroupMbr_SelectedIndexChanged"
                    DataKeyNames="UserID, RegID, PartyID"  >
                    <Columns>
                        <asp:TemplateField HeaderText="Provider" SortExpression="ProviderName">
                            <ItemTemplate>
                                <asp:LinkButton ToolTip="Manage Provider" ID="btnManageProfile" runat="server" CommandName="ManageGroupMemberProfile" 
                                    CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' Text='<%# Eval("ProviderName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:BoundField DataField="RegistrationStatusType" HeaderText="Status" SortExpression="RegistrationStatusType" />
                        <asp:BoundField DataField="ProviderTypeName" HeaderText="Provider Type" SortExpression="ProviderTypeName" />
                        <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
                        <asp:BoundField DataField="SpecialtyTypeName" HeaderText="Specialty" SortExpression="SpecialtyTypeName" />
                        <asp:BoundField DataField="EffectiveDateTime" HeaderText="Effective Date" DataFormatString="{0:MM/dd/yy}" SortExpression="EffectiveDateTime" />
                        <asp:BoundField DataField="SubmitDateTime" HeaderText="Submit Date" DataFormatString="{0:MM/dd/yy}" SortExpression="SubmitDateTime" />
                    </Columns>
                </mms:SortablePagingGridView>
                <div class="grid-hint" id="dvGMPHint" runat="server">Create a Group Member Profile if you are or will be part of a Group Practice.</div>
	            <div class="btnBox">
                    <asp:Button id="btnAddGroupMember" runat="server" Text="Add Group Member Profile" CssClass="buttonBox wd200" ToolTip="Add Group Member Profile" OnClick="btnAddGroupMember_Click"  />
                </div>
                <br /><br />
            </div>
                <div id="divConvProviders" runat="server" style="padding-bottom:10px;">
                <div class="gridCaption">Other Providers with same TaxID</div>
                    <mms:SortablePagingGridView ID="gvConvertedProviders" runat="server" 
                        AutoGenerateSelectButton="false" AutoGenerateColumns="False"  
                        Width="100%" CssClass="gridViewSmallFont" AllowPaging="true"
                        PageSize="15" 
                        OnSelectedIndexChanging="gvConvertedProviders_SelectedIndexChanging"
                        OnPageIndexChanging="gvConvertedProviders_PageIndexChanging" OnRowCommand="gvConvertedProviders_RowCommand" 
                        OnSorting="gvConvertedProviders_Sorting" OnRowDataBound="gvConvertedProviders_RowDataBound" GridViewSortColumn="ProviderName" GridViewSortDirection="Ascending"
                        DataKeyNames="RegID,ProviderCategoryTypeId,IsPendingRegistration,AssignedUserId,AssignedUserName,ApplicationTypeID,EnrollmentStatusCode,AllowManage" EmptyDataText="No providers found." >
                        <Columns>
                            <asp:TemplateField HeaderText="Provider" SortExpression="ProviderName">
                                <ItemTemplate>
                                    <asp:LinkButton ToolTip="Manage Provider" ID="btnManageProvider" runat="server" CommandName="SelectProvider" 
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' Text='<%# Eval("ProviderName") %>' />
                                    <asp:Label ToolTip="Manage Provider" ID="lblManageProvider" runat="server"  Text='<%# Eval("ProviderName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
           
                            <asp:BoundField DataField="RegistrationStatusType" HeaderText="Status" SortExpression="RegistrationStatusType" />
<%--                            <asp:BoundField DataField="ProviderCategoryTypeName" HeaderText="Provider Category" />--%>
                            <asp:BoundField DataField="ProviderTypeName" HeaderText="Provider Type" SortExpression="ProviderTypeName" />
                            <asp:BoundField DataField="NPI" HeaderText="NPI" SortExpression="NPI" />
                            <asp:BoundField DataField="MedicaidID" HeaderText="Medicaid ID" SortExpression="MedicaidID" />
                            <asp:BoundField DataField="TaxonomyName" HeaderText="Taxonomy Code" SortExpression="TaxonomyName" />
                            <asp:BoundField DataField="PracticeLocationZip" HeaderText="Location" SortExpression="PracticeLocationZip" />
                            <asp:BoundField DataField="EndDate" HeaderText="Re-Enrollment Due Date" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" SortExpression="EndDate" />
                            <asp:BoundField DataField="AssignedUserName" HeaderText="Assigned User"  />
                                                        <asp:TemplateField HeaderText="" >
                                <ItemTemplate>
                                    <asp:LinkButton ToolTip="Manage Provider" ID="btnTransferProvider" runat="server" CommandName="TransferProvider" 
                                        CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' Text="Manage" />
                                </ItemTemplate>
                            </asp:TemplateField>  

                        </Columns>
                    </mms:SortablePagingGridView>
                    <div class="grid-hint">Select a provider to begin managing its registration.</div>
                                    <uc:MessageBox ID="MessageBox2" runat="server" />
                </div>
        </div>
            <ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlNoExceptions" TargetControlID="btnDummy"
        RepositionMode="RepositionOnWindowScroll" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlNoExceptions"  >
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlNoExceptions" runat="server" CssClass="modalPopup" Style="display: none; height: 140px; width:300px;">
        <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
            <div class="popTitle">Transfer Ownership</div>
        </asp:Panel>
            <div style="padding-top:10px;padding-left:4px;padding-right:4px;" >
                <asp:Label ID="lblNewReg" runat="server" Text="Please confirm if the registration should be transferred."></asp:Label>
                
                <br /><br />
                <div class="btnBox" >
                    <asp:Button ID="btnProcessYes" runat="server" Text="Yes" CssClass="buttonBox" OnClick="btnProcessYes_Click" CausesValidation="false"  />
                    <asp:Button ID="btnProcessNo" runat="server" Text="No" CssClass="buttonBox" OnClick="btnProcessNo_Click" CausesValidation="false" />
                </div>
            </div>
    </asp:Panel>
    <asp:Button runat="server" ID="btnDummy" Style="display: none" text="btnDummy" />
                        <asp:HiddenField ID="hidIndex" runat="server" />
    </div>


