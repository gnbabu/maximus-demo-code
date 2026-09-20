<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_Reconsideration" Codebehind="Reconsideration.ascx.cs" %>


<%@ Register Src="~/UserControls/FormField.ascx" TagPrefix="uc" TagName="FormField" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="SectHd" TagPrefix="uc1" %>



<div class="enrollment">

    <asp:Panel runat="server" ID="pnlSepReconsideration">
        <uc1:SectHd runat="server" ID="sepReconsideration" Header="Reconsideration Request" />
    </asp:Panel>

    <asp:UpdatePanel ID="upReconsiderationRequest" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <div class="row completeFields">
                    <div class="col-sm-4 text-right">
                        <asp:Label ID="lblReconsiderationRequestDate" runat="server" Text="Date of Reconsideration Request" CssClass="formLabel200" />
                    </div>
                    <div class="col-sm-8">
                        <asp:TextBox ID="txtReconsidertionRequestDate" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="calStart" TargetControlID="txtReconsidertionRequestDate" runat="server" />
                    </div>
                </div>
            </div>
            <asp:PlaceHolder runat="server" ID="PlaceholderUploadIntegrity"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<asp:Panel ID="pnlProgramIntegrityMeeting" runat="server">
    <div class="enrollment">
        <asp:Panel runat="server" ID="pnlSepProgramIntegrityMeeting" Enabled="true"  Visible="true">
            <uc1:SectHd runat="server" ID="sepProgramIntegrityMeeting" Header="Reconsideration Meeting" />
        </asp:Panel>
        <asp:UpdatePanel ID="upProgramIntegrityMeeting" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div>
                    <div class="row completeFields">
                        <div class="col-sm-4 text-right">
                            <asp:Label ID="lblDateofMeeting" runat="server" Text="Date of Meeting" CssClass="formLabel200" />
                        </div>
                        <div class="col-sm-8">
                            <asp:TextBox ID="txtDateofMeeting" runat="server" CssClass="formField" /><ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtDateofMeeting" runat="server" />
                        </div>
                    </div>
                </div>
                <asp:PlaceHolder runat="server" ID="PlaceholderUploadNotesProgramIntegrityMeeting"></asp:PlaceHolder>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Panel>

<asp:Panel ID="pnlTerminationReconsider" runat="server" Enabled="false">
    <div class="row">
        <div class="row">
            <div class="row">
                <div class="col-sm-3  text-right">
                    <%-- <asp:Label ID="lblTerminationReason" Text="Termination Reason" runat="server" class="formLabel200" />--%>

                    <span class="formLabel wd170">Termination Reason* </span>
                </div>
                <div class="col-sm-9">
                    <asp:TextBox ID="txtTerminationReason" runat="server" CssClass="formField" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtTerminationReason" ValidationGroup="valReconsideration"></asp:RequiredFieldValidator>

                </div>
                <div style="display: none;"></div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel wd170">Effective  Date* </span></div>
                <div class="col-sm-9 text-left">
                    <asp:TextBox ID="txtEffectiveDate" runat="server" CssClass="formField" />
                    <ajax:CalendarExtender ID="CalendarExtenderReport" TargetControlID="txtEffectiveDate" runat="server" />
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtReportDate" ValidationGroup="valReconsideration"></asp:RequiredFieldValidator>--%>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel wd170">Enrollment Status Reason </span></div>
                <div class="col-sm-9 text-left">
                    <asp:DropDownList ID="ddlEnrollmentStatusReason" runat="server" CssClass="formField"  OnInit="ddlEnrollmentReasons_Init">
                    </asp:DropDownList>
                </div>
            </div>

        </div>
    </div>
</asp:Panel>
