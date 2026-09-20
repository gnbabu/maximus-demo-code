<%@ control language="C#" autoeventwireup="true" inherits="UserControls_RegEnrollment, App_Web_glma3lal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:ValidationSummary ID="vsRegEnrollment" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="NPIandMedIdControl" />
<asp:UpdatePanel ID="upProv" runat="server" UpdateMode="Conditional">
<ContentTemplate>
<h2><span class="pageHeader">NPI Span</span></h2>
<hr style="background-color: #205794; height: 4px; border: none; margin-top: 0;" />
<div>History of the NPI in PNM is listed below.</div>
<br>
<div id="divNote" style="font-weight: bold; margin-top: 5px; margin-bottom: 3px; font-style: italic">
	Note: Only dates with a calendar icon are editable for the NPI span by Provider Type.
</div>
<br>
<asp:RadioButtonList ID="rblEnrollmentSpanActions" runat="server"  RepeatDirection="Vertical" OnSelectedIndexChanged="rblEnrollmentSpanActions_SelectedIndexChanged" AutoPostBack="true" style="margin-left: 0"/>
<br><br>
<asp:Label ID="lblFailureMsg" runat="server" Text="" CssClass="failureNotification"/>
<asp:HiddenField ID="hdnEnrollIdORP" runat="server" />
<asp:HiddenField ID="hdnEnrollIdORPInactive" runat="server" />
<telerik:RadGrid ID="rgApplicationType" runat="server" RenderMode="Lightweight" MasterTableView-Caption="Enrollment Spans" AllowPaging="True" AllowSorting="False" 
				OnNeedDataSource="rgApplicationType_NeedDataSource" AllowFilteringByColumn="False" Skin="PDMSModern" 
				CellSpacing="0" GridLines="None" OnUpdateCommand="rgApplicationType_UpdateCommand"
				OnItemDataBound="rgApplicationType_ItemDataBound"
				AutoGenerateColumns="false" AutoGenerateEditColumn="False" OnPreRender="rgApplicationType_PreRender">

	<MasterTableView CommandItemDisplay="None" GridLines="None" DataKeyNames="REG_NPI_MEDID_ENROLLMENT_SPAN_ID"> 
		 <Columns> 

			<telerik:GridEditCommandColumn UniqueName="EditCommandColumn" ButtonType="ImageButton" EditText="Edit" UpdateText="Update" CancelText="Cancel" EditImageUrl="~/Images/edit.png" UpdateImageUrl="../App_Themes/Default/Grid/Update.gif" CancelImageUrl="../App_Themes/Default/Grid/Cancel.gif">
			</telerik:GridEditCommandColumn>
			
			<telerik:GridBoundColumn DataField="NPI" HeaderText="NPI Number       " UniqueName="EnrollmentNpiNumber">  
			</telerik:GridBoundColumn> 
			<telerik:GridBoundColumn DataField="MMIS_PROVIDER_TYPE_ID" HeaderText="PT Type      " UniqueName="EnrollmentPTType">  
			</telerik:GridBoundColumn> 
			<telerik:GridBoundColumn DataField="REG_ID" HeaderText="Reg ID" UniqueName="EnrollmentRegId">  
			</telerik:GridBoundColumn> 
			<telerik:GridBoundColumn DataField="MEDICAID_ID" HeaderText="Med ID" UniqueName="EnrollmentMedId" >  
			</telerik:GridBoundColumn>

            <telerik:GridTemplateColumn DataField="ENROLL_START_DATE_TIME" HeaderText="Provider Effective Date"  UniqueName="EnrollmentEffDate" DataType="System.DateTime">
                <ItemTemplate>
                   <asp:Image ID="imgEffDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="18px" AlternateText="Effective Date Calendar" />
				   <asp:Label ID="lblEffDate" runat="server" CssClass="formLabelGrid" Text='<%#Eval("ENROLL_START_DATE_TIME", "{0:MM/dd/yyyy}") %>'></asp:Label>
                </ItemTemplate>
            </telerik:GridTemplateColumn>

            <telerik:GridTemplateColumn DataField="ENROLL_END_DATE_TIME" HeaderText="End Date"  UniqueName="EnrollmentEndDate" DataType="System.DateTime">
                <ItemTemplate>
                   <asp:Image ID="imgEndDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="18px" AlternateText="End Date Calendar" />
				   <asp:Label ID="lblEndDate" runat="server" CssClass="formLabelGrid" Text='<%#Eval("ENROLL_END_DATE_TIME", "{0:MM/dd/yyyy}") %>'></asp:Label>
                </ItemTemplate>
            </telerik:GridTemplateColumn>

			<telerik:GridBoundColumn DataField="UserName" HeaderText="User/History" UniqueName="EnrollmentUsername">  
			</telerik:GridBoundColumn> 
			<telerik:GridBoundColumn DataField="ENROLL_STATUS_DESC" HeaderText="Active/Inactive" UniqueName="EnrollentStatus">  
			</telerik:GridBoundColumn> 
			<telerik:GridBoundColumn DataField="REG_NPI_MEDID_ENROLLMENT_SPAN_ID" UniqueName="REG_NPI_MEDID_ENROLLMENT_SPAN_ID" Display="false">  
			</telerik:GridBoundColumn> 
		</Columns> 
		<EditFormSettings EditFormType="Template">            
					<FormTemplate>
						<div class="WhiteBox">
							<div class="gridEditTable"> 
								<div class="row">
									<div class="col-sm-3 text-right">NPI</div>
									<div class="col-sm-9 text-left">
										 <asp:TextBox MaxLength="50" ID="txtNpi" aria-label="NPI" runat="server" Text='<%# Bind("NPI") %>' CssClass="formField"></asp:TextBox>
									</div>
								</div>
								<div class="row">
									<div class="col-sm-3 text-right">PT Type</div>
									<div class="col-sm-9 text-left">
										<asp:TextBox MaxLength="50" ID="txtPTType" aria-label="PT Type" runat="server" Text='<%# Bind("MMIS_PROVIDER_TYPE_ID") %>' CssClass="formField"></asp:TextBox>
									</div>
								</div>
								<div class="row">
									<div class="col-sm-3 text-right">Reg ID</div>
									<div class="col-sm-9 text-left">
										 <asp:TextBox MaxLength="50" ID="txtRegId" aria-label="Reg ID" runat="server" Text='<%# Bind("REG_ID") %>' CssClass="formField"></asp:TextBox>
									</div>
								</div>
								<div class="row">
									<div class="col-sm-3 text-right">Medicaid ID</div>
									<div class="col-sm-9 text-left">
										 <asp:TextBox MaxLength="50" ID="txtMedicaidId" aria-label="Medicaid ID" runat="server" Text='<%# Bind("MEDICAID_ID") %>' CssClass="formField"></asp:TextBox>
									</div>
								</div>
								<div class="row">
									<div class="col-sm-3 text-right">Provider Effective Date</div>
									<div class="col-sm-9 text-left">
										  <ajax:CalendarExtender ID="calProviderEffectiveDate" runat="server" 
											Format="MM/dd/yyyy"  TargetControlID="txtProviderEffectiveDate" PopupPosition="BottomRight"  
											CssClass="QstCalendarCSS" PopupButtonID="imgExpirationDate"  EnabledOnClient="true" />
										 <asp:TextBox ID="txtProviderEffectiveDate" runat="server" aria-label="Provider Effective Date" Text='<%# Bind("ENROLL_START_DATE_TIME","{0:MM/dd/yyyy}") %>' CssClass="formField"/>
									</div>
								</div> 
								<div class="row">
									<div class="col-sm-3 text-right">End Date</div>
									<div class="col-sm-9 text-left">
										  <ajax:CalendarExtender ID="calProviderEndDate" runat="server" 
											Format="MM/dd/yyyy"  TargetControlID="txtProviderEndDate" PopupPosition="BottomRight"  
											CssClass="QstCalendarCSS" PopupButtonID="imgExpirationDate"  EnabledOnClient="true" />
										 <asp:TextBox ID="txtProviderEndDate" runat="server" aria-label="End Date" Text='<%# Bind("ENROLL_END_DATE_TIME","{0:MM/dd/yyyy}") %>' CssClass="formField"/>
									</div>
								</div> 
								<div class="row">
									<div class="col-sm-3 text-right">User/History</div>
									<div class="col-sm-9 text-left">
										 <asp:TextBox MaxLength="50" ID="txtUsername" aria-label="Username" runat="server" Text='<%# Bind("UserName") %>' CssClass="formField"></asp:TextBox>
									</div>
								</div>
								<div class="row">
									<div class="col-sm-3 text-right">Active/Inactive</div>
									<div class="col-sm-9 text-left">
										 <asp:TextBox MaxLength="50" ID="txtStatus" aria-label="Active/Inactive" runat="server" Text='<%# Bind("ENROLL_STATUS_DESC") %>' CssClass="formField"></asp:TextBox>
									</div>
								</div>
								<div class="row" hidden>
									<div class="col-sm-3 text-right">Active/Inactive</div>
									<div class="col-sm-9 text-left">
										 <asp:TextBox MaxLength="50" ID="txtRegEnrollmentId" aria-label="Reg Enrollment ID" runat="server" Text='<%# Bind("REG_NPI_MEDID_ENROLLMENT_SPAN_ID") %>' CssClass="formField"></asp:TextBox>
									</div>
								</div>
                                <div class="row">
                                    <div class="col-sm-9"></div>
                                    <div class="col-sm-3 text-right">
                                        <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>' 
                                            runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBoxFocus"></asp:Button>&nbsp;
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                                        CommandName="Cancel" CssClass="buttonBox"></asp:Button>
                                    </div>
                                </div>
							</div>
						</div>                              
					</FormTemplate>
				<PopUpSettings ScrollBars="None" />
		</EditFormSettings>
	</MasterTableView>
</telerik:RadGrid> 
</ContentTemplate>
</asp:UpdatePanel>
