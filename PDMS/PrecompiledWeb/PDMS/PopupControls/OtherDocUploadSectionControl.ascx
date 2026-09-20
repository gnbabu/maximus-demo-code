<%@ control language="C#" autoeventwireup="true" inherits="UserControls_OtherDocUploadSectionControl, App_Web_wbqq1lcm" %>
<%@ Register Src="~/PopupControls/UploadDocument.ascx" TagName="UploadDocument" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="us" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:UpdatePanel ID="upShowNames" runat="server"   UpdateMode="Always">
 <ContentTemplate>
        <div style="width: 85%;margin: 0 auto; display: table; ">
                   <div style="border-style: solid; border-color: #000000; border-width: 0.03em;">
                       <%-- <div style="background-color: #036; width: 100%;">
                            <span style="padding-left: 1px; text-align: center; font-weight: bold; color: #FFF;">
                                <asp:Label ID="uploadLabel" runat="server" Text="" />
                            </span>
                            <span style="text-align: center; font-style: italic; color: #FFF;">
                                <asp:Label ID="uploadInfo" runat="server" Text="" />
                            </span>
                        </div>--%>
                       <div style="padding-top: 10px;padding-left:10px;">
                            <p style="color:#B30000;">If you have additional documentation to provide that were not available for upload on other pages, upload those here. You may upload multiple documents and you will be able to view and delete documents after uploading.</p>
                            <span>
                            <asp:Label ID="LblFileName" Text="" runat="server" ForeColor="Green" CssClass="formLabel" Width="40%"></asp:Label>
                            &nbsp;
                            <asp:LinkButton ID="LnkButtonDownload" runat="server" OnClick="OnFileDownload" ForeColor="Blue" Visible="false">Download</asp:LinkButton>
                           &nbsp;&nbsp;
                            <asp:LinkButton ID="LnkButtonDelete" runat="server" OnClick="OnFileRemove" ForeColor="Red" Visible="false">Remove</asp:LinkButton>
                             </span>    
                           <telerik:RadAsyncUpload   ID="RadAsyncUpload1" runat="server" MultipleFileSelection="Disabled" MaxFileInputsCount="1" Visible="false"
                                OnFileUploaded="RadAsyncUpload1_FileUploaded" OnClientFileUploaded="OnClientFilesUploaded" EnableEmbeddedSkins="true" Skin="Default" InputSize="45" Localization-Select="Browse" EnableInlineProgress="true" EnableFileInputSkinning="true"   >
                            </telerik:RadAsyncUpload>
                            <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
                            <asp:HiddenField ID="hdndocumentId" runat="server" />
                            <asp:HiddenField ID="hdnRowId" runat="server" />
                            <asp:HiddenField ID="hdnSectionName" runat="server" />
                        </div>
                        <asp:Button ID="BtnUpload" runat="server" style="display:none;"  Text="Upload1"  ValidationGroup="valUpload" CausesValidation="true" />
                        <asp:CustomValidator runat="server" ID="CustomValidatorUpload" OnServerValidate="CustomValidator_ServerValidate"  ValidationGroup="valUpload" ErrorMessage="Please upload a document">
                        </asp:CustomValidator>
                        <script type="text/javascript">
                            function OnClientFilesUploaded(sender) {
                                //alert("Here");
                                var $ = $telerik.$;
                                $('#<%=BtnUpload.ClientID %>').click();

                            }
                        </script>
                 </div>
            <div>
                <span>You may also mail in additional documentation, which may result in a delay to process your application.  <br />
                   <strong>Mailing Address:<br /></strong>
                    Ohio Department of Medicaid <br />
                    Provider Enrollment Unit <br />
                    PO Box 1461<br />
                    Columbus, OH 43216-1461 

                </span>
            </div>
         </div>     
            
        <Triggers>
        <asp:AsyncPostBackTrigger ControlID="BtnUpload" EventName="Click" />
        </Triggers>

</ContentTemplate>

</asp:UpdatePanel>
<div style="width: 750px;">
	<asp:Label runat="server" ID="Label1" CssClass="error-message" />
	<asp:ValidationSummary ID="vsLimitedLiablityInsurance" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valLimitedLiablityInsurance" />
</div>
<br />
<us:Separator ID="sepEVV" runat="server" Header="EVV Training" />
<br />
<asp:Panel ID="pnlEVVTraining" runat="server">
	<table style="width: 100%;">
		<tr>
			<td>
				<div class="pg-hint2" style="float: left;">
					<table style="text-align: left;">
						<tr>
							<td>
								<asp:ValidationSummary ID="vsEVVTraining" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="valEVVTraining" />
							</td>
						</tr>

					</table>
				</div>
			</td>
		</tr>
	</table>
	<div class="row">
		<div class="col-sm-6 text-right">
			<asp:Label ID="lblChkEVVTrainingCompleted" AssociatedControlID="rblEVVTrainingCompleted" runat="server" CssClass="formLabel300">EVV Training or Attestation Completed</asp:Label>
		</div>
		<div class="col-sm-6">
			<asp:RadioButtonList ID="rblEVVTrainingCompleted" runat="server" Enabled="true" TextAlign="Left" CssClass="QstRadioList" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblEVVTrainingCompleted_OnSelectedIndexChanged" AutoPostBack="true">
				<asp:ListItem Text="Yes" Value="True"></asp:ListItem>
				<asp:ListItem Text="No" Value="False"></asp:ListItem>
			</asp:RadioButtonList>
		</div>
	</div>
	<div class="row">
		<div class="col-sm-6 text-right">
			<asp:Label ID="lblDateOfEVVTraining" AssociatedControlID="txtDateOfEVVTraining" runat="server" CssClass="formLabel300" Text="EVV Training or Attestation Completed Date"></asp:Label>
		</div>
		<div class="col-sm-6">
			<asp:TextBox ID="txtDateOfEVVTraining" runat="server" CssClass="formField" Enabled="true" ValidationGroup="valEVVTraining" CauseValidation="true" />
			<ajax:CalendarExtender ID="calDateOfProposedAdjudication" TargetControlID="txtDateOfEVVTraining" runat="server" />
			<asp:CompareValidator ID="cvDateOfEVVTraining" runat="server" ValidationGroup="valEVVTraining"
				Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateOfEVVTraining" Enabled="true"
				ErrorMessage="Select a valid Date for EVV Training" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
				SetFocusOnError="true" />
			<asp:RequiredFieldValidator ID="reqValtxtDateOfEVVTraining" runat="server" SetFocusOnError="true" ValidationGroup="valEVVTraining"
				Text="*" ControlToValidate="txtDateOfEVVTraining" ErrorMessage="Enter Date of EVV Training Completed" Display="Dynamic" Enabled="true" />
		</div>
	</div>
	<div class="row">
		<div class="col-sm-6 text-right">
			<asp:Label ID="evvProvLinkLabel" runat="server" CssClass="formLabel300">Agency Provider Training</asp:Label>
		</div>
		<div class="col-sm-6">
			<a href="https://medicaid.ohio.gov/resources-for-providers/special-programs-and-initiatives/electronic-visit-verification">EVV Agency Provider Training</a>
		</div>
	</div>
	<div class="row">
		<div class="col-sm-6 text-right">
			<asp:Label ID="evvIndProvLinkLabel"  runat="server" CssClass="formLabel300">Independent Provider Training</asp:Label>
		</div>
		<div class="col-sm-6">
			<a href="https://medicaid.ohio.gov/resources-for-providers/special-programs-and-initiatives/electronic-visit-verification">EVV Independent Provider Training</a>
		</div>
	</div>
	<br />
	<asp:PlaceHolder runat="server" ID="PlaceholderUploadEVVTraining"></asp:PlaceHolder>

</asp:Panel>
<us:Separator ID="sepLLI" runat="server" Header="Limited Liability Insurance" />
<br />
<asp:Panel ID="pnlLimitedLiabilityInsurance" runat="server">
	<table style="width: 100%;">
		<tr>
			<td>
				<div class="pg-hint2" style="float: left;">
					<table style="text-align: left;">
						<tr>
							<td><span style="color: red;">Your application cannot be completed until this section is satisfied with a Yes response.</span></td>
						</tr>

					</table>
				</div>
			</td>
		</tr>
	</table>
	<div class="row">
		<div class="col-sm-6 text-left">
			<ul style="list-style-type: none; line-height: 1.5; margin-left: 0px; padding: 0px;">
				<li style="width: 800px;">The provider maintains liability insurance coverage in the amount of not less than five hundred thousand dollars per occurrence and not less than five hundred thousand dollars in the aggregate, for any cause for which the provider would be liable. Include proof of insurance.
				</li>
			</ul>
		</div>
	</div>
	<div class="row">
		<div class="col-sm-6">
			<asp:RadioButtonList ID="rblLimitedLiabilityInsurance" runat="server" Enabled="true" TextAlign="Left" CssClass="QstRadioList" RepeatDirection="Horizontal">
				<asp:ListItem Text="Yes" Value="True"></asp:ListItem>
				<asp:ListItem Text="No" Value="False"></asp:ListItem>
			</asp:RadioButtonList>
		</div>
	</div>
	<br />
	<asp:PlaceHolder runat="server" ID="PlaceholderUploadLimitedLiabilityInsurance"></asp:PlaceHolder>
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" CssClass="UploadBox" role="Presentation">
</asp:Panel>
