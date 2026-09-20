<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ProviderManagementView, App_Web_wbqq1lcm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/ProviderCommunicationsView.ascx" TagName="ProviderCommunicationsView" TagPrefix="viewComm" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/FormField.ascx" TagName="FormField" TagPrefix="uc" %>

<link href="<%# Page.ResolveClientUrl("~/App_Themes/Modernization/ToolTip.css") %>" rel="stylesheet" />
<style type="text/css">
    .fileControl {
        display: inline !important;
        width:333px;
    }

    .GridPosition {
        position: absolute;
        left: 120px;
        top: 150px;
    }

    .boxPanelDataLeft .formLabel {
        margin-right: 5px;
        text-align: right;
        width: 300px;
    }

    .spanWrap {
        display: table;
        white-space: break-spaces;
    }

    .reg-info-field {
        display: flex;
        flex-wrap: wrap;
    }

    .reg-info-controls {
        margin: 10px;
    }

    .formFieldReadOnly {
        width: auto;
        min-width: 450px;
    }


    @media only screen and (max-width: 990px) {
        .reg-info-field {
            display: flex;
            flex-direction: column;
            justify-content: space-between;
        }
    }

    @media only screen and (max-width: 760px) {
        .formFieldReadOnly {
            width: auto;
            min-width: 300px;
            height: 68px;
        }
    }
</style>

<script type="text/javascript">
    $(document).ready(function () {

        //Update Panel needs after async postback, else event handlers are lost.
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(setupOrgEventHandlers);
    });

    function setupOrgEventHandlers() {
        $(".help-enrollmentactions").mouseover(function () {
            // .position() uses position relative to the offset parent, 
            var pos = $(this).position();

            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();

            //show the menu directly over the placeholder
            $("#helpEnrollmentActionsIDInfo").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px"
            }).show();

        });
        $(".help-enrollmentactions").mouseleave(function () {
            $("#helpEnrollmentActionsIDInfo").hide();
        });

        $(".help-applications").mouseover(function () {
            // .position() uses position relative to the offset parent, 
            var pos = $(this).position();

            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();

            //show the menu directly over the placeholder
            $("#helpApplicationsIDInfo").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px"
            }).show();

        });
        $(".help-applications").mouseleave(function () {
            $("#helpApplicationsIDInfo").hide();
        });

        $(".help-UpdateCPCcontact").mouseover(function () {
            // .position() uses position relative to the offset parent, 
            var pos = $(this).position();

            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();

            //show the menu directly over the placeholder
            $("#helpUpdateCPCcontactInfo").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px"
            }).show();

        });
        $(".help-UpdateCPCcontact").mouseleave(function () {
            $("#helpUpdateCPCcontactInfo").hide();
        });

        $(".help-UpdateCPC").mouseover(function () {
            // .position() uses position relative to the offset parent, 
            var pos = $(this).position();

            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();

            //show the menu directly over the placeholder
            $("#helpUpdateCPCInfo").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px"
            }).show();

        });
        $(".help-UpdateCPC").mouseleave(function () {
            $("#helpUpdateCPCInfo").hide();
        });
        $(".help-UpdateCMCcontact").mouseover(function () {
            // .position() uses position relative to the offset parent, 
            var pos = $(this).position();

            // .outerWidth() takes into account border and padding.
            var width = $(this).outerWidth();

            //show the menu directly over the placeholder
            $("#helpUpdateCMCcontactInfo").css({
                position: "absolute",
                top: pos.top + "px",
                left: (pos.left + width) + "px"
            }).show();

        });
        $(".help-UpdateCMCcontact").mouseleave(function () {
            $("#helpUpdateCMCcontactInfo").hide();
        });

    }

    $(".lnkUpdateRegistration").mouseover(function () {
        // .position() uses position relative to the offset parent, 
        var pos = $(this).position();

        // .outerWidth() takes into account border and padding.
        var width = $(this).outerWidth();

        //show the menu directly over the placeholder
        $("#helpTaxIDInfo").css({
            position: "absolute",
            top: pos.top + "px",
            left: (pos.left + width) + "px"
        }).show();

    });

    $(".lnkUpdateRegistration").mouseleave(function () {
        $("#helpTaxIDInfo").hide();
    });

    function HideAndShowPopup() {
        document.getElementById('<%=btnSubmitMessagePopup.ClientID %>').click();
    }
	
	var isSubmitted = false;

	function preventMultipleSubmissions() {
        if (!isSubmitted) {
			isSubmitted = true;
			return true;
		}
        else {
            return false;
             }
        }
</script>

<div>
    <div>
        <asp:Label ID="LblRiskAlertStatus" runat="server" ForeColor="Red" Text="" Visible="false"></asp:Label>
    </div>
    <h4>Registration Information</h4>
      <div  class="col-sm-12 text-right">
           <div>
            <asp:Button runat="server" ID="btnPrevious" CssClass="buttonBox" Text="Previous Page" OnClick="btnPrevious_Click"  /> 
        </div> 
           </div>
    <svg width="100%" height="20px">
        <g fill="none" style="stroke: rgb(91,155,213); stroke-width: 2">
            <line x1="1" x2="100%" y1="10" y2="10" stroke-width="2" />
        </g>
    </svg>
    <div id="divWaiverErrorDisplay" runat="server" visible="false" class="row">
        <div>
            <span class="boxPanelHeader" style="color: red; text-shadow: 0em 0.15em 0.35em rgba(0, 0, 0, 0.35);">There was an issue with your registration. Please try again later
            </span>
        </div>
    </div>
    <div id="divCMCRedirectErrMsg" runat="server" visible="false" class="row">
        <div>
            <span class="boxPanelHeader" style="color: red; text-shadow: 0em 0.15em 0.35em rgba(0, 0, 0, 0.35);">Report/Dashboard service is not available, please try again later. 
            </span>
        </div>
    </div>

    <div id="divRegistrationInfo" class="reg-info-field" runat="server">
        <div class="reg-info-controls">
            <b>Provider Name</b><br />
            <asp:Label ID="lblProviderName" runat="server" CssClass="formFieldReadOnly" Text=""></asp:Label>
        </div>

        <div class="reg-info-controls">
            <b>Medicaid ID</b><br />
            <asp:Label ID="lblMedicaidID" runat="server" CssClass="formFieldReadOnly300" Style="width: 200px" />

        </div>

        <div class="reg-info-controls">
            <b>Effective Date</b><br />
            <asp:Label ID="lblEffectiveDate" runat="server" CssClass="formFieldReadOnly300" Style="width: 200px" />
        </div>

        <div class="reg-info-controls">
            <b>Revalidation Due Date</b><br />
            <asp:Label ID="lblRevalidationDate" runat="server" CssClass="formFieldReadOnly300" Style="width: 200px" />
        </div>

        <div class="reg-info-controls">
            <b>Term Date</b><br />
            <asp:Label ID="lblTermDate" runat="server" CssClass="formFieldReadOnly300" Style="width: 200px" />
        </div>
    </div>

    <div class="reg-info-field" id="divDODD" runat="server" visible="false">
        <div class="reg-info-controls" id="divDODDCertStartDate" runat="server" >
            <b>DODD Certification Start Date</b><br />
            <asp:Label ID="lblDODDCertStartDate" runat="server" CssClass="formFieldReadOnly300" Style="width: 200px" />

        </div>
        <div class="reg-info-controls" id="divDODDCertEndDate" runat="server">
            <b>DODD Certification End Date</b><br />
            <asp:Label ID="lblDODDCertEndDate" runat="server" CssClass="formFieldReadOnly300" Style="width: 200px" />

        </div>
        <div class="reg-info-controls">
            <b>DODD Contract Number</b><br />
            <asp:Label ID="lblDODDContractNumber" runat="server" CssClass="formFieldReadOnly300" Style="width: 200px" />

        </div>
    </div>

    <div style="display: none;">
        <div style="display: none" class="row">
            <span class="formLabel spanWrap col-sm-1">
                <asp:Literal ID="lblForRegStatus" runat="server" Text=" <%$ Resources:BrandingResource , REGISTRATION_STATUS_LABEL %>"></asp:Literal>
            </span>
            <asp:Label ID="lblRegistrationStatus" runat="server" CssClass="formFieldReadOnly300" />
        </div>
        <div id="divPseStatus" runat="server" visible="false" class="row">
            <span class="formLabel spanWrap col-sm-1">PSE Status</span>
            <asp:Label ID="lblPseStatus" runat="server" CssClass="formFieldReadOnly300" />
        </div>
        <div id="divApplicationStatus" runat="server" visible="false" class="row">
            <span class="formLabel spanWrap col-sm-1">Application Status</span>
            <asp:Label ID="lblApplicationStatus" runat="server" CssClass="formFieldReadOnly300" />
        </div>
        <div style="display: none" class="row">
            <span class="formLabel wd150 spanWrap col-sm-1">Enrollment Status</span>
            <asp:Label ID="lblEnrollmentStatus" runat="server" CssClass="formFieldReadOnly300" />
        </div>
    </div>
    <br />
    <h4>Manage Application</h4>
    <svg width="100%" height="20px">
        <g fill="none" style="stroke: rgb(91,155,213); stroke-width: 2">
            <line x1="1" x2="100%" y1="10" y2="10" stroke-width="2" />
        </g>
    </svg>
    <table style="width: 70%;">
        <tr>
            <td style="vertical-align: top; width: 15%;"><b>Enrollment Actions</b></td>
            <td>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 10px; vertical-align: top;"><span id="spanEnrollmentActionIcon" style="font-size: 30px; padding: 5px; color: mediumblue; cursor: pointer;">+</span></td>
                        <td>
                            <div style="background-color: lightgray; padding: 5px;">
                                <div style="vertical-align: top"><b>Enrollment Action Selections:</b></div>
                                <div id="divEnrollmentActionLinks" style="display: none;">
                                  <asp:Panel ID="pnlEnrollmentActionLinks" runat="server">
                                    <div class="vert-links">                                        
                                        <div id="divRevalHelpText" runat="server" visible="false">
                                            <span>
                                                <p style="color: red;">Once the application is approved, this will fulfill your Revalidation requirement.</p>
                                            </span>
                                        </div>
                                          <div id="divCPTHelpText" runat="server" visible="false">
                                            <span>
                                                <p style="color: red;">It appears you are currently in the middle of a workflow with a different Medicaid ID. Once that existing workflow is completed you can manage this Medicaid ID.</p>
                                            </span>
                                        </div>
                                        <div id="divDODDUpdate" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkDODDContinue" runat="server" Text="Continue DODD Enrollment Profile Update" CommandName="ContinueDODDUpdateRegistration" OnClick="lnkDODDContinue_Click" />
                                            <asp:LinkButton ID="lnkCancelUpdateDoddRegistration" runat="server" Text="Cancel Update" CommandName="CancelUpdateRegistration" OnClick="lnkCancelWorkflow_Click"
                                                OnClientClick="javascript:return confirm('Cancel update of registration. Are you sure?');" />
                                        </div>
                                        <div id="divODAUpdate" runat="server" class="vert-links" visible="false">
                                            <div id="divODAContinue" runat="server">
                                            <asp:LinkButton ID="lnkODAContinue" runat="server" Text="Continue ODA Enrollment Profile Update" CommandName="ContinueODAUpdateRegistration" OnClick="lnkODAContinue_Click" />
                                            </div>
                                            <div id="divCancelUpdateODA" runat="server">
                                            <asp:LinkButton ID="lnkCancelUpdateOdaRegistration" runat="server" Text="Cancel Update" CommandName="CancelUpdateRegistration" OnClick="lnkCancelWorkflow_Click"
                                                OnClientClick="javascript:return confirm('Cancel update of registration. Are you sure?');" />
                                            </div>
                                        </div>
                                        <div id="divOptRevalidationNeeded" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkBeginRevalidation" runat="server" Text="Begin Revalidation" CommandName="BeginRevalidation" OnClick="lnkBeginWorkflow_Click" />
                                        </div>
                                        <%--Group member profile--%>
                                        <div id="divOptNoActiveWorkflowGMP" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkUpdateGroupMemberProfile" runat="server" Text="Update Group Member Profile" CommandName="BeginUpdateGMPRegistration" OnClick="lnkBeginWorkflow_Click" PostBackUrl="~/Process/ProviderUpdateSummary.aspx" />
                                        </div>
                                        <div id="divOptNewGroupMbrProfile" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkContinueGroupMbrProfile" runat="server" Text="Continue Group Member Profile" CommandName="ContinueNewRegistration" OnClick="lnkContinueWorkflow_Click" PostBackUrl="~/Process/Registration.aspx" />
                                        </div>
                                        <div id="divOptNewRegistration" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkContinueRegistration" runat="server" Text="Continue Registration" CommandName="ContinueNewRegistration" OnClick="lnkContinueWorkflow_Click" PostBackUrl="~/Process/Registration.aspx" />
                                            <asp:LinkButton ID="lnkContinueDODDRegistration" runat="server" Text="Continue DODD Registration" CommandName="ContinueDODDRegistration" OnClick="lnkContinueWaiverWorkflow_Click" Visible="false" />
                                            <asp:LinkButton ID="lnkContinueODARegistration" runat="server" Text="Continue ODA Registration" CommandName="ContinueODARegistration" OnClick="lnkContinueWaiverWorkflow_Click" Visible="false" />
                                            <asp:LinkButton ID="lnkCancelNewRegistration" runat="server" Text="Cancel New Registration" CommandName="CancelNewRegistration" OnClick="lnkCancelWorkflow_Click"
                                                OnClientClick="javascript:return confirm('Cancel new registration. Are you sure?');" />
                                        </div>
                                        <div id="divOptUpdateProvider" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkContinueUpdateRegistration" runat="server" Text="Continue ODM Enrollment Profile Update" CommandName="ContinueUpdateRegistration" OnClick="lnkContinueUpdateWorkflow_Click" />
                                            <asp:LinkButton ID="lnkCancelUpdateRegistration" runat="server" Text="Cancel Update Registration" CommandName="CancelUpdateRegistration" OnClick="lnkCancelWorkflow_Click"
                                                OnClientClick="javascript:return confirm('Cancel update of registration. Are you sure?');" />
                                        </div>
                                        <div id="divReapplicationProvider" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkCntReapplicationProvider" runat="server" Text="Continue Reapplication" CommandName="ContinueReapplication" OnClick="lnkContinueWorkflow_Click" />
                                            <asp:LinkButton ID="lnkCancelReapplicationProvider" runat="server" Text="Cancel Reapplication " CommandName="CancelReapplication" OnClick="lnkCancelWorkflow_Click"
                                                OnClientClick="javascript:return confirm('Cancel update of registration. Are you sure?');" />
                                        </div>
                                        <div id="divOptUpdateOwnership" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkContinueUpdateOwnership" runat="server" Text="Continue Update Ownership" CommandName="ContinueUpdateOwnership" OnClick="lnkContinueWorkflow_Click" PostBackUrl="~/Process/Registration.aspx" />
                                            <asp:LinkButton ID="lnkCancelUpdateOwnership" runat="server" Text="Cancel Update Ownership" CommandName="CancelUpdateOwnership" OnClick="lnkCancelWorkflow_Click"
                                                OnClientClick="javascript:return confirm('Cancel update of registration. Are you sure?');" />
                                        </div>
                                        <div id="divOptAddAffiliation" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkContinueAddAffiliation" runat="server" Text="Continue Adding Group Members" CommandName="ContinueAddAffiliation" OnClick="lnkContinueWorkflow_Click" PostBackUrl="~/Process/Registration.aspx" />
                                            <asp:LinkButton ID="lnkCancelAddAffiliation" runat="server" Text="Cancel Adding Group Members" CommandName="CancelAddaffiliation" OnClick="lnkCancelWorkflow_Click"
                                                OnClientClick="javascript:return confirm('Cancel update of group members. Are you sure?');" />
                                        </div>
                                        <div id="divOptServicesReferral" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkContinueAddServices" runat="server" Text="Continue Services" CommandName="ContinueWaiverServices" OnClick="lnkContinueWorkflow_Click" PostBackUrl="~/Process/Registration.aspx" />
                                            <asp:LinkButton ID="lnkCancelAddServices" runat="server" Text="Cancel Services" CommandName="CancelWaiverServices" OnClick="lnkCancelWorkflow_Click"
                                                OnClientClick="javascript:return confirm('Cancel update of  services. Are you sure?');" />
                                        </div>
                                        <div id="divOptRevalidation" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkContinueRevalidation" runat="server" Text="Continue Revalidation" CommandName="ContinueRevalidation" OnClick="lnkContinueWorkflow_Click" />
                                            </div>
                                        <div id="divCancelConvertORP" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkCancelConvertORP" runat="server" Text="Cancel Convert from ORP" CommandName="CancelRevalidation" OnClick="lnkCancelWorkflow_Click"
                                                OnClientClick="javascript:return confirm('Cancel Convert from ORP. Are you sure?');" />
                                        </div>
                                        <div id="divOptReactivateEnrollment" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkContinueReactivateEnrollment" runat="server" Text="Continue Reactivate Enrollment" CommandName="ContinueReactivateEnrollment" OnClick="lnkContinueWorkflow_Click" PostBackUrl="~/Process/Registration.aspx" />
                                            <asp:LinkButton ID="lnkCancelReactivateEnrollment" runat="server" Text="Cancel Reactivate Enrollment" CommandName="CancelReactivateEnrollment" OnClick="lnkCancelWorkflow_Click"
                                                OnClientClick="javascript:return confirm('Cancel reactivate enrollment. Are you sure?');" />
                                        </div>
                                        <div id="divOptODMUpdate" runat="server" class="vert-links" visible="false">
											<asp:LinkButton ID="lnkUpdateRegistration" runat="server" Text="Begin ODM Enrollment Profile Update" CommandName="BeginUpdateRegistration" OnClick="lnkBeginUpdateWorkflow_Click" />
										</div>
                                        <div id="divOptNoActiveWorklowNonGMP" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkUpdateOwnershipInfo" runat="server" Text="Update Ownership Information" CommandName="BeginUpdateOwnership" OnClick="lnkBeginWorkflow_Click" PostBackUrl="~/Process/Registration.aspx" />
                                            <asp:LinkButton ID="lnkUpdateServices" runat="server" Text="Update Services Registration" CommandName="BeginUpdateServices" OnClick="lnkBeginWorkflow_Click" PostBackUrl="~/Process/Registration.aspx" />
                                            <asp:LinkButton ID="lnkAddAffiliation" runat="server" Text="Add a Group Member" CommandName="BeginAddAffiliation" OnClick="lnkBeginWorkflow_Click" PostBackUrl="~/Process/ProviderUpdateSummary.aspx" />
                                            <asp:LinkButton ID="lnkConvertFromORP" runat="server" Text="Convert from ORP" CommandName="ConvertfromORP" OnClick="lnkConvertfromORP_Click" PostBackUrl="~/Process/Registration.aspx" />
                                            <asp:LinkButton ID="lnkBeginReapplication" runat="server" Text="Begin Reapplication" CommandName="BeginReapplication" OnClick="lnkBeginWorkflow_Click" Visible="false" PostBackUrl="~/Process/Registration.aspx" />
                                            <asp:LinkButton ID="lnkBeginDODDUpdate" runat="server" Text="Begin DODD Enrollment Profile Update" Visible="false" OnClick="lnkBeginDODDUpdate_Click" OnClientClick="HideAndShowPopup()" />
                                            <asp:LinkButton ID="lnkAddODAServices" runat="server" Text="Add ODA Services" Visible="false" OnClick="lnkAddODAServices_Click" OnClientClick="HideAndShowPopup()" />
                                            <asp:LinkButton ID="lnkAddODMorODAMedSvc" runat="server" Text="Add ODM/ODA Medicaid Services" Visible="false" OnClick="lnkAddODMorODAMedSvc_Click" />
                                        </div>
                                          <div id="divOptProviderTypeChange" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkProviderTypeChange" runat="server" Text="Provider Type Change" CommandName="ProviderTypeChange" OnClick="lnkProviderTypeChange_Click" />
                                          </div>
                                          <div id="divOptContinueOrCancelProviderTypeChange" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkContinueProviderTypeChange" runat="server" Text="Continue Provider Type Change" CommandName="ContinueProviderTypeChange" OnClick="lnkContinueWorkflow_Click" />
                                             <asp:LinkButton ID="lnkCancelProviderTypeChange" runat="server" Text="Cancel Provider Type Change" CommandName="CancelProviderTypeChange" OnClick="lnkCancelProviderTypeChange_Click" />
                                          </div>
                                        <div id="divOptKeyFields" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkUpdateKeyFields" runat="server" Text="Edit Key Provider Identifiers" CommandName="UpdateKeyFields" OnClick="lnkUpdateKeyFields_Click" />
                                        </div>
                                        <div id="divLinkProvider" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkLinkProvider" runat="server" Text="Add Provider Type" CommandName="AddLinkProvider" OnClick="lnkLinkProvider_Click" />
                                        </div>
                                        <div id="divDisEnrollment" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkDisEnrollment" runat="server" Text="Request Disenrollment" CommandName="SaveDisEnrollment" OnClick="lnkDisEnrollment_Click" />
                                        </div>
                                        <div id="divRequestReconsideration" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkRequestReconsideration" runat="server" Text="Request Reconsideration" OnClick="lnkRequestReconsideration_Click" />
                                        </div>
                                        <div id="divInitiateCHOP" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="linkInitiateCHOP" runat="server" Text="Initiate CHOP" CommandName="InitiateCHOP" OnClick="lnkInitiateChop_Click" />
                                        </div>
                                        <div id="divSubmit90DayClosure" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="linkSubmit90DayClosure" runat="server" Text="Submit 90 Day Closure" CommandName="Submit90DayClosure" OnClick="linkSubmit90DayClosure_Click" />
                                        </div>
                                    </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
               <span id="helpEnrollmentActionsID" class="help-enrollmentactions" style="padding-left: 10px; cursor: pointer; display: inline-block;">
                    <asp:Image ID="imgHelpEnrollmentActionsID" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" /></span>
                    <span id="helpEnrollmentActionsIDInfo" class="infoBox" style="top: 0; right: 0;">
                    <span class="infoTitle">Enrollment Actions Help</span>
                    <span class="infoContent">
                        <asp:Literal ID="ltlEnrollmentActionsHelp" runat="server" Text="<%$ Resources:BrandingResource , ENROLLMENT_ACTIONS_HELP_TEXT %>"></asp:Literal>
                    </span>
                </span>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>

        <tr>
            <td style="vertical-align: top;width: 15%;"><b>Programs</b>
                                                                <div>
    <uc:MessageBox ID="ucMessageBox" runat="server" />
</div>
            </td>
            <td>
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 10px; vertical-align: top;"><span id="spanProgramIcon" style="font-size: 30px; padding: 5px; color: mediumblue; cursor: pointer;">+</span></td>
                        <td>
                            <div style="background-color: lightgray; padding: 5px;">
                              <p><b>Program Selections:</b></p>
                              <div id="divProgramSelectionLinks" style="display: none;">
                                <table style="width: 100%">
                                  <tr>
                                    <td style="vertical-align:top;" style="width: 100%">
                                            <div class="vert-links">
                                                <div id="divCreateCPCIndividual" runat="server" class="vert-links" visible="false">
                                                    <asp:LinkButton ID="lnkCreateCPCIndividual" runat="server" Text="Create CPC Individual" OnClick="lnkCreateCPCIndividual_Click" PostBackUrl="~/Process/Registration.aspx" />
                                                </div>
                                                <div id="divCreatePracticePartnership" runat="server" class="vert-links" visible="false">
                                                    <asp:LinkButton ID="lnkCreatePracticePartnership" runat="server" Text="Create a Practice Partnership" OnClick="lnkCreatePracticePartnership_Click" PostBackUrl="~/Process/Registration.aspx" />
                                                </div>
                                                <div id="divContinueCPCApplication" runat="server" class="vert-links" visible="false">
                                                    <asp:LinkButton ID="lnkContinueCPCApplication" runat="server" Text="Continue CPC Application" OnClick="lnkContinueCPCApplication_Click" />
                                                </div>
                                                <div id="divReattestCPCIndividualorPracticePartnership" runat="server" class="vert-links" visible="false">
                                                    <asp:LinkButton ID="lnkReattestCPCIndividualorPracticePartnership" runat="server" Text="Re-attest CPC Individual or Practice Partnership" OnClick="lnkReattestCPCIndividualorPracticePartnership_Click" PostBackUrl="~/Process/Registration.aspx" />
                                                </div>
                                                <div id="divBeginCPCEnrollmentUpdate" runat="server" class="vert-links" visible="false">
                                                    <asp:LinkButton ID="lnkBeginCPCEnrollmentUpdate" runat="server" Text="Request CPC Enrollment Change" OnClick="lnkBeginCPCEnrollmentUpdate_Click">
                                                        Request CPC Enrollment Change<span id="helpUpdateCPCID" class="help-UpdateCPC" style="padding-left: 10px; cursor: pointer; display: inline-block;">
                                                        <asp:Image ID="imgUpdateCPCID" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" /></span>
                                                        <span id="helpUpdateCPCInfo" class="infoBox" style="top: 0; right: 0;">
                                                           <span class="infoContent">
                                                              <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:BrandingResource , UPDATE_CPC_HELP_TEXT %>"></asp:Literal>
                                                           </span>
                                                        </span>
                                                    </asp:LinkButton>
                                                </div>
                                                <div id="divUpdateCPCcontact" runat="server" visible="false" class="vert-links">                                                 
                                                    <asp:LinkButton ID="lnkUpdateCPCcontact" runat="server" Text="Update CPC Contact" CommandName="" OnClick="lnkUpdateProviderFile_Click" >
                                                    Update CPC Contact<span id="helpUpdateCPCcontactID" class="help-UpdateCPCcontact" style="padding-left: 10px; cursor: pointer; display: inline-block;">
                                                        <asp:Image ID="imgUpdateCPCcontactID" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" /></span>
                                                        <span id="helpUpdateCPCcontactInfo" class="infoBox" style="top: 0; right: 0;">
                                                           <span class="infoContent">
                                                              <asp:Literal ID="ltlUpdateCPCcontactHelp" runat="server" Text="<%$ Resources:BrandingResource , UPDATE_CPC_CONTACT_HELP_TEXT %>"></asp:Literal>
                                                           </span>
                                                        </span>
                                                    </asp:LinkButton>     
                                                </div>
                                                 <div id="divCancelCPCApplication" runat="server" class="vert-links" visible="false">
                                                    <asp:LinkButton ID="lnkCancelCPCApplication" runat="server" Text="Cancel CPC Application" CommandName="CancelCPCRegistration" OnClick="lnkCancelWorkflow_Click" />
                                                </div>
                                                 <div id="divCancelCPCUpdate" runat="server" class="vert-links" visible="false">
                                                    <asp:LinkButton ID="lnkCancelCPCUpdate" runat="server" Text="Cancel CPC Update" CommandName="CancelCPCRegistration" OnClick="lnkCancelWorkflow_Click" />
                                                </div>
                                                 <div id="divCPCUpload" runat="server" class="vert-links" visible="false">                                                     
                                                    <span id="spanUploadIcon" style="font-size: 30px; padding: 5px; color: mediumblue; cursor: pointer;">+</span><span><b>Upload CPC Supplemental Clinical Data Files</b></span>
                                                   
                                                    <div id="divShowUpload" style="display: none;">
                                                          <div id="divShowRecordTypes" style="margin-left:10px;">
                                                          <div class="row">
                                                             <span style="margin-left:48px;margin-top:30px"><b>Record Types</b></span>   

                                                          </div>
                                                          <div class="row">
                                                            <span style="margin-left:70px;margin-top:30px;"><b>Please select a Record Type and upload a file in a format of CSV, TXT or EXCEL.</b></span>   

                                                         </div>
                                                      </div>
                                                        <div style="margin-left:68px;margin-top:10px; padding:20px;">
                                                       <%-- <p><b>Upload File:</b></p>--%>
                                                        <div class="row">
                                                            <div class="col-lg-4 col-sm-4 col-md-4" style="">
                                                                <span>Choose File:</span>
                                                                </div>
                                                            <div class="col-lg-3 col-sm-3 col-md-3" style="width:333px;">
                                                        <mms:EncryptedFileUpload runat="server" required ID="UploadAttachments" accept=".txt, .csv,.xlsx, .xls" aria-label="Upload attachment" ViewStateMode="Enabled" CssClass="fileControl"  />
                                                                </div>
                                                                </div>
                                                        <div class="row">
                                                                <div class="col-lg-4 col-sm-4 col-md-4" style="">
                                                                    <span>Record Type:</span>

                                                            <span id="helpRecordTypeID" runat="server"  data-tooltip-position="top"  class="help-recordtypeactions" data-tooltip="<%$ Resources:BrandingResource , ENROLLMENT_RECORDTYPE_HELP_TEXT %>" style="--width: 300px;cursor: pointer; display: inline-block;">
                                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" /></span>
                                                                     
                                                                </div>
                                                                <div class="col-lg-3 col-sm-3 col-md-3" style="">
                                                                     <asp:textbox ID="txtTypeID" CssClass="formField" EnableViewState="true" runat="server" 
                                                                        AppendDataBoundItems="True" required="true" >
                                                                      </asp:textbox>
                                                                </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-lg-4 col-sm-4 col-md-4" style="">
                                                                <span>Reporting Month (MM/YY):</span>
                                                            </div>
                                                            <div class="col-lg-3 col-sm-3 col-md-3" style="">
                                                                <asp:textbox ID="txtMonth" CssClass="formField" EnableViewState="true" runat="server"  
                                                                    AppendDataBoundItems="True" required="true">
                                                                </asp:textbox>
                                                                <div style="width:333px;">
                                                                   <asp:RegularExpressionValidator runat="server" ID="regMonthTextBox" ForeColor="red" ErrorMessage="Invalid Month Format" Display="Dynamic" ValidationExpression="^(0[1-9]|1[0-2])\/\d{2}$" ControlToValidate="txtMonth"  ></asp:RegularExpressionValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-lg-4 col-sm-4 col-md-4" style=""></div>
                                                            <div class="col-lg-3 col-sm-3 col-md-3" style="font-size:20px;">
                                                                <asp:button id="btnUpload" runat="server" onclick="btnUpload_Click" Text="Upload" />
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                                </div>
      
                                                <div id="divInitiateCMCEnroll" runat="server" class="vert-links" visible="false">
                                                    <asp:LinkButton ID="lnkInitiateCMCEnroll" runat="server" Text="Initiate CMC Enrollment" CommandName="" OnClick="btnInitiateCMCWorkflow_Click" PostBackUrl="~/Process/Registration.aspx" />
                                                </div>
                                                <div id="divContinueCMCApp" runat="server" class="vert-links" visible="false">
                                                    <asp:LinkButton ID="lnkContinueCMCApp" runat="server" Text="Continue CMC Application" CommandName="ContinueNewRegistration" OnClick="lnkContinueCMCApplication_Click" PostBackUrl="~/Process/Registration.aspx" />
                                                </div>
                                                <div id="divUpdateCMCContact" runat="server" class="vert-links" visible="false">
                                                    <asp:LinkButton ID="lnkUpdateCMCContact" runat="server" Text="Update CMC Contact" CommandName="" OnClick="lnkBeginCMCEnrollmentUpdate_Click">
                                                        Update CMC Contact<span id="helpUpdateCMCcontactID" class="help-UpdateCMCcontact" style="padding-left: 10px; cursor: pointer; display: inline-block;">
                                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" /></span>
                                                        <span id="helpUpdateCMCcontactInfo" class="infoBox" style="top: 0; right: 0;">
                                                           <span class="infoContent">
                                                              <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:BrandingResource , UPDATE_CPC_CONTACT_HELP_TEXT %>"></asp:Literal>
                                                           </span>
                                                        </span>
                                                    </asp:LinkButton>
                                                </div>
                                                <div id="divReattestCMCProvider" runat="server" class="vert-links" visible="false">
                                                    <asp:LinkButton ID="lnkReattestCMCProvider" runat="server" Text="Re-attest CMC Provider" CommandName="" OnClick="lnkReattestCMC_Click" PostBackUrl="~/Process/Registration.aspx" />
                                                </div>
                                                 <div id="divCancelCMCApplication" runat="server" class="vert-links" visible="false">
                                                    <asp:LinkButton ID="lnkCancelCMCApplication" runat="server" Text="Cancel CMC Application" CommandName="CancelNewRegistration" OnClick="lnkCancelWorkflow_Click" />
                                                </div>
                                                 <div id="divCancelCMCUpdate" runat="server" class="vert-links" visible="false">
                                                    <asp:LinkButton ID="lnkCancelCMCUpdate" runat="server" Text="Cancel CMC Update" CommandName="CancelNewRegistration" OnClick="lnkCancelWorkflow_Click" />
                                                </div>
                                            </div>
                                            <div id="divCPCInfolink" runat="server" visible="false">
                                                <span>
                                                    <p>
                                                        <a target="_blank" href="http://medicaid.ohio.gov/Provider/PaymentInnovation/CPC">Click here for Information on the CPC Program &raquo;</a>
                                                    </p>
                                                </span>
                                            </div>
                                            <div id="divCPCMessage" runat="server" visible="false">
                                                <span>
                                                    <p style="color: red;">The open enrollment application and attestation period for the Comprehensive Primary Care (CPC) program for the upcoming performance year has ended. If your practice or partnership was unable to enroll or re-attest, please contact Provider Assistance at 1-800-686-1516.</p>
                                                </span>
                                            </div>
                                            <div id="divCMCInfoLink" runat="server" visible="false">
                                                <span>
                                                    <p>
                                                        <a target="_blank" href="https://medicaid.ohio.gov/INITIATIVES/Maternal-and-Infant-Support/Mom-Baby-Bundle">Click here for information on the CMC Program &raquo;</a>
                                                    </p>
                                                </span>
                                            </div>
                                            <div id="divCMCMessage" runat="server" visible="false">
                                                <span>
                                                    <p style="color: red;">The open enrollment application and attestation period for the Comprehensive Maternal Care (CMC) program for the upcoming performance year has ended. If your practice was unable to enroll, please contact Provider Assistance at 1-800-686-1516.</p>
                                                </span>
                                            </div>
                                        
                                    </td>  <%-- end program--%>
                                  </tr>
                                  <tr>                                    
                                    <td  style="width: 100%">                                         
                                        <div id="divCPCCounts" runat="server" visible="false">
                                            <br/>
                                            <p><b>Enrollment attribution counts</b></p>
                                            <div id="divCPCEnrollmentCounts" runat="server" visible="false">
                                                <p><b>CPC</b></p>
                                                <hr style="height:2px;border-width:0;color:black;background-color:black">
                                                <div class="row">
                                                    <div class="col-sm-8 text-left">
                                                        <span>Total Attributed Member Count</span>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:label id="lblCPCMemcnt" runat="server" text="xx" CssClass="formLabelSmall" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-8 text-left">
                                                        <span>Total Attributed Pediatric Member Count</span>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:label id="lblCPCPedMemCnt" runat="server" text="xx" CssClass="formLabelSmall" />
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                            <div id="divCMCEnrollmentCounts" runat="server" visible="false">
                                                <p><b>CMC</b></p>
                                                <hr style="height:2px;border-width:0;color:black;background-color:black">
                                                <div class="row">
                                                    <div class="col-sm-8 text-left">
                                                        <span>Qualifying Enrollment Count</span>
                                                    </div>
                                                    <div class="col-sm-4">
                                                        <asp:label id="lblCMCcnt" runat="server" text="xx" CssClass="formLabelSmall" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                               </table>   
                              </div>  <%--divProgramSelectionLinks--%>
                            </div>
                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td style="vertical-align: top;"><b>Self Service</b></td>
            <td>
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 10px; vertical-align: top;"><span id="spanSelfServiceIcon" style="font-size: 30px; padding: 5px; color: mediumblue; cursor: pointer;">+</span></td>
                        <td>
                            <div style="background-color: lightgray; padding: 5px;">
                                <p><b>Self Service Selections:</b></p>
                                <div id="divSelfServiceSelectionLinks" style="display: none;">
                                    <div class="vert-links">
                                        <div id="divOptViewOnly" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkViewProviderFile" runat="server" Text="View Provider File" OnClick="lnkViewProviderFile_Click" PostBackUrl="~/Process/Registration.aspx" />
                                            <asp:LinkButton ID="lnkReactivateProvider" runat="server" Text="Reactivate Provider" CommandName="BeginReactivation" OnClick="lnkBeginWorkflow_Click" PostBackUrl="~/Process/Registration.aspx" />
                                        </div>
                                        <div id="divCorrespondence" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkBtnCorrespondence" runat="server" CssClass="add-row" OnCommand="BtnAdd_Correspondence">Provider Correspondence</asp:LinkButton>
                                        </div>
                                        <div id="divRemittanceRedirection" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkBtnRemittanceRedirection" runat="server" CssClass="add-row" OnCommand="BtnAdd_Remittance">Remittance Advice</asp:LinkButton>
                                        </div>
                                        <div id="divRecipientEligibility" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkRecipientEligibilityMITS" runat="server" CssClass="add-row" OnCommand="lnkNavigate_MemberEligibility">Recipient Eligibility</asp:LinkButton>
                                        </div>
                                        <div id="divClaims" runat="server" class="vert-links" visible="false">
                                           <%-- <asp:LinkButton ID="lnkClaims" runat="server" CssClass="add-row" OnCommand="lnkNavigateMITS_Command">Claims</asp:LinkButton>--%>
                                             <asp:LinkButton ID="lnkClaims" runat="server" CssClass="add-row" OnCommand="lnkNavigateMITS_Claims">Claims</asp:LinkButton>
                                        </div>
                                        <div id="divPriorAuthorization" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkPriorAuth" runat="server" CssClass="add-row" OnCommand="lnkNavigateMITS_PriorAuth">Prior Authorization</asp:LinkButton>
                                        </div>
                                        <div id="divCostReports" runat="server" class="vert-links" visible="false">
                                             <asp:LinkButton ID="lnkCostReports" runat="server" CssClass="add-row" OnCommand="lnkNavigateMAS_Command">Cost Reports and Rate Setting</asp:LinkButton>
                                        </div>
                                        <div id="divHospice" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkHospice" runat="server" CssClass="add-row" OnCommand="lnkBtn_Hospice">Hospice</asp:LinkButton>
                                        </div>
                                        <div id="divViewFinancials" runat="server" visible="false">
                                            <asp:LinkButton ID="lnkViewFinancials" runat="server" CssClass="add-row" OnCommand="BtnAdd_Financial">Provider Financial Self Services</asp:LinkButton></div>
                                        <div id="divPaymentInnovationReports" runat="server" class="vert-links" visible="false">
                                            <%--<a id="lnkPaymentInnovationReport" runat="server" href="PaymentInnovationReports.aspx?MedicaidId=<%= this.PaymentInnovationsProviderId%>">Alternative Payment Model Information</a></div>--%>
                                            <asp:LinkButton ID="lnkPaymentInnovationReport" runat="server" CssClass="add-row" OnCommand="lnkNavigate_PaymentInnovations">Alternative Payment Model Information</asp:LinkButton>
                                        </div>
                                        <div id="divProviderReports" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkBtnProviderReports" runat="server" CssClass="add-row" OnCommand="lnkBtn_ProviderReports">Provider Reports</asp:LinkButton>
                                        </div>
                                        <div id="divAttachments" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkBtnAttachments" runat="server" CssClass="add-row" OnCommand="lnkBtn_Attachments">Attachments</asp:LinkButton>
                                        </div>
                                        <div id="divORPSearch" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkBtnORPSearch" runat="server" CssClass="add-row" OnCommand="lnkBtn_ORPSearch">Ordering, Referring & Prescribing Search</asp:LinkButton>
                                        </div>
                                        <div id="divCMCDashboard" runat="server" class="vert-links" visible="false">
                                            <asp:LinkButton ID="lnkBtnCMCDashboard" runat="server" CssClass="add-row" OnCommand="lnkBtn_CMCDashboard">CMC Quality Metrics Dashboard</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <h4>
        My Current and Previous Applications 
            <span id="helpApplicationsID" class="help-applications" style="padding-left: 10px; cursor: pointer; display: inline-block;">
            <asp:Image ID="imgHelpApplicationsID" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" /></span>
            <span id="helpApplicationsIDInfo" class="infoBox" style="top: 0; right: 0;">
            <span class="infoContent">
                <table>
                 <tr>
                  <td><b>Providers Status</b></td>
                  <td><b>Providers Description</b></td>
                  <td><b>Providers Link</b></td>
                 </tr>
                 <tr><td colspan="3">---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------</td></tr>
                 <tr>
                  <td>Not Submitted</td>
                  <td>Provider Data Entry</td>
                  <td>Continue Link</td>
                 </tr>
                 <tr>
                  <td>Submitted</td>
                  <td>Screening, Review, Credentialing, Site Visit, BCII</td>
                  <td>No enrollment action links</td>
                 </tr>
                 <tr>
                  <td>Processing</td>
                  <td>Finalizing application</td>
                  <td>No enrollment action links</td>
                 </tr>
                 <tr>
                  <td>Denied</td>
                  <td>ODM has denied this application (initial applications)</td>
                  <td>Reconsideration Reapplication</td>
                 </tr>
                 <tr>
                  <td>Return to Provider</td>
                  <td>Application is returned to the provider to take
                  action</td>
                  <td>Continue link</td>
                 </tr>
                 <tr>
                  <td>Terminated</td>
                  <td>ODM has denied this application resulting in a
                  Termination (active provider)</td>
                  <td>Reconsideration Reapplication</td>
                 </tr>
                 <tr>
                  <td>Cancelled</td>
                  <td>Cancelled by Provider or aged out (10 days)</td>
                  <td>Begin update links</td>
                 </tr>
                 <tr>
                  <td>Approved / Complete</td>
                  <td>The provider is active and able to submit update</td>
                  <td>Begin update links</td>
                 </tr>
                 <tr>
                  <td>Disenrolled</td>
                  <td>The provider has voluntarily disenrolled</td>
                  <td>Reapplication</td>
                 </tr>
                 <tr>
                  <td>Suspended</td>
                  <td>The provider is suspended by ODM</td>
                  <td>Reconsideration</td>
                 </tr>
                 <tr>
                  <td>Pending Closure</td>
                  <td>Processing pending facility closure</td>
                  <td>Once submitted, no links available until complete</td>
                 </tr>
                 <tr>
                  <td>Pending CHOP</td>
                  <td>Processing pending change of operator</td>
                  <td>Once submitted, no links available until complete</td>
                 </tr>
                 <tr>
                  <td>Not Processed</td>
                  <td>Application is incomplete and provider must reapply</td>
                  <td>Reapplication</td>
                 </tr>
                </table>
            </span>
        </span>
    </h4>

    <svg width="100%" height="20px">
        <g fill="none" style="stroke: rgb(91,155,213); stroke-width: 2">
            <line x1="1" x2="100%" y1="10" y2="10" stroke-width="2" />
        </g>
    </svg>
    <asp:GridView runat="server" ID="grdCurrrentandPrev" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview" EmptyDataText="No Current and Previous Applications." DataKeyNames="ApplicationID">
        <Columns>
            <asp:BoundField DataField="REG_ID" HeaderText="Reg ID" />
            <asp:BoundField DataField="EnrollmentAction" HeaderText="Enrollment Action" />
            <asp:BoundField DataField="Program" HeaderText="Program" />
            <asp:BoundField DataField="ApplicationID" HeaderText="Application Id" />
            <asp:BoundField DataField="PNMApplicationStatus" HeaderText="PNM Application Status" />
            <asp:BoundField DataField="OtherAgencyApplicationStatus" HeaderText="Other Agency Application Status" />
            <asp:BoundField DataField="LegalApplicationStatus" HeaderText="DD Legal Status" />
            <asp:BoundField DataField="StatusDate" HeaderText="Status Date" DataFormatString="{0:MM/dd/yy}" />
            <asp:BoundField DataField="WorkflowComplete" HeaderText="Workflow Complete" />
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
    </asp:GridView>

    <div id="divMoratoriaInfo" runat="server" visible="false">
        <span class="formLabel wd150">Moratoria Begin Date</span>
        <asp:Label ID="lblMoratoriaBeginDate" runat="server" CssClass="formFieldReadOnly formFieldAuto" /><br />
        <span class="formLabel wd150">Moratoria EndDate Date</span>
        <asp:Label ID="lblMoratoriaEndDate" runat="server" CssClass="formFieldReadOnly formFieldAuto" />
    </div>
    <table border="0" style="padding-left: 10px; width: 100%; display: none;">
        <tr>
            <td>
                <viewComm:ProviderCommunicationsView ID="ucCommView" runat="server" />
            </td>
        </tr>
    </table>
</div>
<br />

<ajax:ModalPopupExtender ID="mpeUpdateRegistration" runat="server" PopupControlID="pnlUpdateModal" TargetControlID="ButtonDummyUpdate" BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlUpdateModal">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlUpdateModal" runat="server" CssClass="identModalPopup" align="center" Style="display: none; padding: 20px; width: 300px;">
    <p>
        <asp:Label ID="lblCREDays" runat="server" Text="" />
    </p>
    <asp:Button runat="server" ID="btnUpdateOk" Text="Ok" CssClass="buttonBox" OnClick="btnUpdateOk_Click" CausesValidation="false" />
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummyUpdate" Style="display: none" Text="ButtonDummy" />

<ajax:ModalPopupExtender ID="mpeError" runat="server" PopupControlID="pnlError" TargetControlID="btnError" BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlError">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlError" runat="server" CssClass="identModalPopup" align="center" Style="display: none; padding: 20px; width: 300px;">
    <p>
        <asp:Label ID="lblErrorNPI" runat="server" Text="" />
    </p>
    <asp:Button runat="server" ID="btnErrorOk" Text="Ok" CssClass="buttonBox" OnClick="btnErrorOk_Click" CausesValidation="false" />
</asp:Panel>
<asp:Button runat="server" ID="btnError" Style="display: none" Text="ButtonDummy" />


<ajax:ModalPopupExtender ID="mpeRequestReconsideration" runat="server" PopupControlID="pnlRequestReconsideration" TargetControlID="lnkRequestReconsideration"
    BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlRequestReconsideration">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlRequestReconsideration" runat="server" CssClass="ownerModalPopup" align="center" Style="display: none; height: 300px; width: 1000px">
    <asp:Panel ID="Panel1" CssClass="popHeader" runat="server">
        <div class="popTitle">
            <asp:Label ID="Label3" runat="server" Text="Request Reconsideration" />
        </div>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px">
        <div>
            <asp:ValidationSummary ID="vsReconsideration" runat="server" DisplayMode="List" ValidationGroup="ReconsiderationProvider" CssClass="text-left" />
        </div>
        <div class="row" id="trdateRecon" runat="server">
            <div class="col-sm-4  text-right">
                <span class="formLabel wd200">
                    <asp:Label ID="Label2" runat="server" Text="Date of Reconsideration Request*" CssClass="formLabel wd200" /></span>
            </div>
            <div class="col-sm-8">

                <span style="text-align: left;">
                    <asp:TextBox ID="txtDateofReconsidertaionRequest" runat="server" CssClass="formField wd200" Enabled="false" />
                    <ajax:CalendarExtender ID="CalendarExtender4" TargetControlID="txtDateofReconsidertaionRequest" runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" ValidationGroup="ReconsiderationProvider" Text="*"
                        ControlToValidate="txtDateofReconsidertaionRequest" ErrorMessage="* Reconsideration Effective Date is required." Display="Dynamic" Enabled="true" />
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="ReconsiderationProvider"
                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDateofReconsidertaionRequest" Enabled="true"
                        ErrorMessage="* A valid Reconsideration Effective Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                        SetFocusOnError="true"> 
                    </asp:CompareValidator>
                </span>
            </div>
        </div>

        <br />
        <div style="margin-left: 100px" class="col-sm-8">
            <asp:Panel ID="Panel2" CssClass="popHeader" runat="server">
                <div class="popTitle">
                    <asp:Label ID="Label1" runat="server" Text="Reconsideration Request" />
                </div>
            </asp:Panel>
            <br />
            <div class="row">
                <asp:Label ID="lblreconMsg" runat="server" Text="*Please select at least one reason for Disenrollment." Visible="false" CssClass="bodyTextRed"></asp:Label>
            </div>
            <div class="row">

                <mms:EncryptedFileUpload runat="server" ID="encRequestReconsiderationDoc" Width="400px" ViewStateMode="Enabled" CssClass="fileControl" />
            </div>
        </div>

        <div class="btnBox">
            <asp:Button runat="server" ID="btnReconSave" Text="Save" CssClass="buttonBoxFocus" OnClick="btnReconsiderationSave_Click" CausesValidation="true" />
            <asp:Button runat="server" ID="btnReconCancel" Text="Cancel" CssClass="buttonBox" OnClick="btnReconsiderationCancel_Click" CausesValidation="false" />
        </div>

    </asp:Panel>
    <br />

</asp:Panel>

<%--<asp:Panel ID="pnlRequestReconsideration" runat="server" CssClass="ownerModalPopup" align="center" Style="display: none;">

    <asp:Panel ID="Panel1" CssClass="popHeader" runat="server">
        <div class="popTitle">
            <asp:Label ID="Label3" runat="server" Text="Request Reconsideration" />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px" DefaultButton="btnOk">
        <div class="row" id="trMTEffectiveDate1" runat="server">
            <div class="col-sm-4  text-right">
                <span class="formLabel wd200">
                    <asp:Label ID="Label1" runat="server" Text="Date of Reconsideration Request*" CssClass="formLabel wd200" /></span>
            </div>
            <div class="col-sm-8">
                <span style="text-align: left;">
                    <asp:TextBox ID="txtDateofReconsidertaionRequest" runat="server" CssClass="formFieldReadOnly" />
                    <ajax:CalendarExtender ID="CalendarExtender4" TargetControlID="txtDateofReconsidertaionRequest" runat="server" />
                </span>
            </div>
        </div>
    </asp:Panel>
    <div class="row text-center" style="padding-right: 10px;">
        <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="buttonBoxFocus" />
    </div>
    <br />
</asp:Panel>--%>

<ajax:ModalPopupExtender ID="mpeContinueUpdate" runat="server" PopupControlID="pnlContinueModal" TargetControlID="lnkContinueUpdateRegistration" BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlContinueModal">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlContinueModal" runat="server" CssClass="identModalPopup" align="center" Style="display: none; padding: 20px; width: 300px;">
    <p>
        <asp:Label ID="lblCUEDays" runat="server" Text="" />
    </p>
    <asp:Button runat="server" ID="btnContinueUpdateOk" Text="Ok" CssClass="buttonBox" OnClick="btnContinueUpdateOk_Click" CausesValidation="false" />
</asp:Panel>

<ajax:ModalPopupExtender ID="mpeSaveDisEnrollement" runat="server" PopupControlID="pnlSaveDisEnrollement" TargetControlID="lnkDisEnrollment" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlSaveDisEnrollement">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlSaveDisEnrollement" runat="server" CssClass="modalPopup" Style="display: none; width: 800px;">
    <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
        <table style="width: 100%; cursor: pointer">
            <tr>
                <td>
                    <div class="popTitle">Request Disenrollment</div>
                </td>
                <td style="text-align: right">
                    <asp:ImageButton ID="imgClose" runat="server" ImageUrl="~/Images/cancel.png" OnClick="btnDisEnrollmentCancel_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>


    <asp:UpdatePanel ID="upPreview" runat="server" UpdateMode="Conditional" style="margin-left: auto; margin-right: auto;">
        <ContentTemplate>
            <div id="divPreview" runat="server" style="padding: 20px;">
                <asp:ValidationSummary ID="vsNewRequest" DisplayMode="List" runat="server"    ValidationGroup="DisenrollProvider" ShowSummary="true" style="text-align:left"   />
                <div class="row">
                    <div class="col-sm-4 text-left">
                        <asp:Label CssClass="formLabel wd200" runat="server" ID="lblDisenrollmentDate">Disenrollment Effective Date*</asp:Label>
                    </div>
                    <div class="col-sm-4 text-left">
                        <asp:TextBox ID="txtTermDate" runat="server" CssClass="formField wd200" MaxLength="10" CausesValidation="True" />
                        <ajax:CalendarExtender ID="ceTermDate" TargetControlID="txtTermDate" runat="server" />
                        <asp:RequiredFieldValidator ID="valTermDateReqd" runat="server" SetFocusOnError="true" ValidationGroup="DisenrollProvider" Text="*"
                            ControlToValidate="txtTermDate" ErrorMessage="* Disenrollment Effective Date is required." Display="Dynamic" Enabled="true" />
                        <asp:CompareValidator ID="cvTermDate" runat="server" ValidationGroup="DisenrollProvider"
                            Type="Date" Operator="DataTypeCheck" ControlToValidate="txtTermDate" Enabled="true"
                            ErrorMessage="* A valid Disenrollment Effective Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                            SetFocusOnError="true"> 
                        </asp:CompareValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4 text-left">
                        <p><strong>Indicate all that apply</strong></p>
                    </div>
                    <div class="col-sm-4 text-left">
                        <asp:Label ID="lblError" runat="server" Text="*Please select at least one reason for Disenrollment." Visible="false" CssClass="bodyTextRed"></asp:Label>
                        <div style="margin: 0 auto; width: 600px;">
                            <asp:CheckBoxList ID="disEnrollmentOptions" runat="server"></asp:CheckBoxList>
                        </div>
                    </div>
                </div>

                <div class="btnBox">
                    <asp:Button runat="server" ID="btnDisEnrollmentSave" Text="Save" CssClass="buttonBox" OnClick="btnDisEnrollmentSave_Click" CausesValidation="false" OnClientClick="this.value = 'Saving...'; return preventMultipleSubmissions();" />
                    <asp:Button runat="server" ID="btnDisEnrollmentCancel" Text="Cancel" CssClass="buttonBox" OnClick="btnDisEnrollmentCancel_Click" CausesValidation="false" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Panel>
<ajax:ModalPopupExtender ID="mpeClosureNotice" runat="server" PopupControlID="pnlClosureNotice" TargetControlID="linkSubmit90DayClosure" BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlClosureNotice">
</ajax:ModalPopupExtender>

<asp:Panel ID="pnlClosureNotice" runat="server" CssClass="modalPopup" Style="display: none; height: auto; width: auto;">
    <asp:Panel runat="server" ID="pnlClosureNoticeHeader">
        <span id="sepClosureNoticeHeader" runat="server" class="pageHeader">Initiate Closure Notice</span>
    </asp:Panel>
    <div>
        <mms:EncryptedFileUpload runat="server" ID="fileClosureNoticeUploadFile" Width="400px" ViewStateMode="Enabled" CssClass="fileControl" />
    </div>
    <div class="btnBox">
        <asp:Button runat="server" ID="btnClosureNoticeUpload" Text="Upload" CssClass="buttonBox" OnClick="btnClosureNoticeUpload_Click" CausesValidation="false" />
        <asp:Button runat="server" ID="btnClosureNoticeCancel" Text="Cancel" CssClass="buttonBox" OnClick="btnClosureNoticeCancel_Click" CausesValidation="false" />
    </div>
    <p>
        <b>
            <asp:Label ID="lblStatusMsg" runat="server" /></b>
    </p>
    <%-- <asp:Button runat="server" ID="Button1" Text="Ok" CssClass="buttonBox" OnClick="btnUpdateOk_Click" CausesValidation="false" PostBackUrl="~/Process/ProviderUpdateSummary.aspx" />--%>
</asp:Panel>

<ajax:ModalPopupExtender ID="mpeDaysNotice" runat="server" PopupControlID="pnlDaysNotice" TargetControlID="linkInitiateCHOP" BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlDaysNotice">
</ajax:ModalPopupExtender>

<asp:Panel ID="pnlDaysNotice" runat="server" CssClass="modalPopup" Style="display: none; height: auto; width: auto;">
    <asp:Panel runat="server" ID="pnlDaysNoticeHeader">
        <span id="sepDaysNoticeHeader" runat="server" class="pageHeader">Initiate 45 Days Notice</span>
    </asp:Panel>
    <div>
        <mms:EncryptedFileUpload runat="server" ID="fileDaysNoticeUploadFile" Width="400px" ViewStateMode="Enabled" CssClass="fileControl" />
    </div>
    <div class="btnBox">
        <asp:Button runat="server" ID="btnDaysNoticeUpload" Text="Upload" CssClass="buttonBox" OnClick="btnDaysNoticeUpload_Click" CausesValidation="false" />
        <asp:Button runat="server" ID="btnDaysNoticeCancel" Text="Cancel" CssClass="buttonBox" CausesValidation="false" />
    </div>
    <p>
        <b>
            <asp:Label ID="lblDaysStatusMsg" runat="server" /></b>
    </p>
    <%-- <asp:Button runat="server" ID="Button1" Text="Ok" CssClass="buttonBox" OnClick="btnUpdateOk_Click" CausesValidation="false" PostBackUrl="~/Process/ProviderUpdateSummary.aspx" />--%>
</asp:Panel>

<ajax:ModalPopupExtender ID="mpeMessage" runat="server" PopupControlID="pnlMessagepopup" TargetControlID="btnSubmitMessagePopup"
    CancelControlID="btnCancelMessagePopup" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlModal">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlMessagepopup" runat="server" CssClass="modalPopup" align="center" Style="display: none">
    <asp:Panel ID="Panel3" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
        <div style="text-align: left;">
            &nbsp;&nbsp;
           <asp:Label ID="Label4" CssClass="bodyTextBold" runat="server" Text="" ForeColor="White" />

        </div>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server" Style="margin-right: 10px">
    </asp:Panel>
    <table style="width: 700px; height: 250px; border-spacing: 10px 50px">
        <tr>
            <td>
                <br />
            </td>
        </tr>
        <tr style="align-content: center">
            <td style="padding-left: 92px;">
                <table border="1" style="width: 500px; height: 150px; align-content: center; border-spacing: 10px 50px;">
                    <tr>
                        <td><span style="font-size: large; align-content: center; text-align: left">Please wait while your information and session are being transferred to another Ohio Agency in order to complete your application.</span></td>
                    </tr>
                </table>
            </td>

        </tr>
    </table>
</asp:Panel>
<asp:Button runat="server" ID="btnSubmitMessagePopup" Style="display: none; visibility: hidden" />
<asp:Button runat="server" ID="btnCancelMessagePopup" Style="display: none; visibility: hidden" />

<script type="text/javascript">
    $('#spanEnrollmentActionIcon').on('click', function () {
        $('#divEnrollmentActionLinks').toggle(100);
        if ($(this).text() == '+') {
            $(this).text('-');
        }
        else {
            $(this).text('+');
        }
    });

    $('#spanProgramIcon').on('click', function () {
        $('#divProgramSelectionLinks').toggle(100);
        if ($(this).text() == '+') {
            $(this).text('-');
        }
        else {
            $(this).text('+');
        }
    });

    $('#spanUploadIcon').on('click', function () {
        $('#divShowUpload').toggle(100);
        if ($(this).text() == '+') {
            $(this).text('-');
        }
        else {
            $(this).text('+');
        }
    });

    $('#spanSelfServiceIcon').on('click', function () {
        $('#divSelfServiceSelectionLinks').toggle(100);
        if ($(this).text() == '+') {
            $(this).text('-');
        }
        else {
            $(this).text('+');
        }
    });
</script>
