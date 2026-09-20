<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_SiteVisitAttempt" Codebehind="SiteVisitAttempt.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/UploadDocument.ascx" TagName="UploadDocument" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc1" %>
<asp:Panel ID="pnlSiteVisitAttemptDetails" runat="server">
    <br />
    <br />
        <uc:Separator ID="sepLogSiteVisitAttempt" runat="server" Header="Log Site Visit Attempt" />
        <br />
        <div>
            <div class="row" id="dvOrganizationName" runat="server">
               <%--<div class="col-sm-3 text-right"><span class="formLabel170">Site Visit Type</span></div>
               <div class="col-sm-3"><asp:Label runat="server" id="lblSiteVisitType" CssClass="formFieldDisplay" /></div>--%>
               <div class="col-sm-3 text-right"><span class="formLabel170">Organization Name</span></div>
               <div class="col-sm-3"><asp:Label runat="server" CssClass="formFieldDisplayAuto" id="lblOrganizationName" /></div>
            </div>
            <div class="row" id="dvAttempt" runat="server">
               <div class="col-sm-3 text-right"><span class="formLabel170">Attempt</span></div>
               <div class="col-sm-9"><asp:Label runat="server" id="lblAttempt" CssClass="formFieldDisplay" /></div>
            </div>
            <div id="dvProvResponseDate" class="row" style="display:none;">
               <div class="col-sm-3 text-right"><span class="formLabel170">Provider Response Date</span></div>
               <div class="col-sm-3"><asp:TextBox ID="txtResponseDate" runat="server" CssClass="formField" />
                                    <ajax:CalendarExtender ID="calResponseDate" TargetControlID="txtResponseDate" runat="server" /></div>
            </div>
            <div class="row" id="dvVisitDate" runat="server">
                <div class="col-sm-3 text-right"><span class="formLabel170">Visit Date</span></div>
                <div id="dvtxtVisitDate" runat="server" class="col-sm-9"><asp:TextBox ID="txtVisitDate" runat="server" CssClass="formField" />
                <ajax:CalendarExtender ID="calVisitDate" TargetControlID="txtVisitDate" runat="server" />
                <asp:CompareValidator id="valVisitDate" runat="server" 
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtVisitDate" ValidationGroup="SiteVisitAttempt"
                    ErrorMessage="Select a valid Visit Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> 
                </asp:CompareValidator>
                <asp:RequiredFieldValidator runat="server" ID="valVisitDateRequired" ControlToValidate="txtVisitDate"
                    ErrorMessage="Visit Date is required." Text="*" Display="Dynamic" ValidationGroup="SiteVisitAttempt" /></div>
                <div id="dvlblVisitDate" runat="server" class="col-sm-9"><asp:Label runat="server" id="lblVisitDate" CssClass="formFieldDisplay" /></div>
                </div>
            <div class="row" id="dvSiteVisitProcess" runat="server">
                <div class="col-sm-3 text-right"><span class="formLabel170">Site Visit Process</span></div>
                <div id="dvddlMethod" class="col-sm-9" runat="server"><asp:DropDownList runat="server" ID="ddlMethod" CssClass="formDropDown"  AutoPostBack="false" /></div>
                <div id= "dvlblMethod" class="col-sm-9" runat="server"><asp:Label runat="server" id="lblMethod" CssClass="formFieldDisplay" /></div>
            </div>
            <div class="row" style="display:none;" id="dvPerformedBy" runat="server">
               <div class="col-sm-3 text-right"><span class="formLabel170">Performed By</span></div>
               <div class="col-sm-9"><asp:Label runat="server" CssClass="formFieldDisplay" id="lblPerformedBy" /></div>
            </div>
            <div class="row" id="dvRecommendation" runat="server">
                <div class="col-sm-3 text-right"><span class="formLabel170">Recommendation</span></div>
                <div id="dvddlRecommendation" class="col-sm-9" runat="server"><asp:DropDownList runat="server" ID="ddlRecommendation" CssClass="formDropDown"  AppendDataBoundItems="True" /></div>
                <div id= "dvlblRecommendation" class="col-sm-9" runat="server"><asp:Label runat="server" id="lblRecommendation" CssClass="formFieldDisplay" /></div>
            </div>
            <div id="dvddlFindings" class="row" runat="server">
                <div class="col-sm-3 text-right"><span class="formLabel170">Findings</span></div>
                <div  class="col-sm-9"><asp:DropDownList runat="server" ID="ddlfindings" CssClass="formDropDown"  AutoPostBack="true" AppendDataBoundItems="True"/></div>
            </div>
            <div  id="dvOpcomments" runat="server" class="row">
                <div class="col-sm-3 text-right"><span class="formLabel170">Comments</span></div>
                <div class="col-sm-9"><asp:TextBox runat="server" ID="txtComments" TextMode="MultiLine" Rows="4" CssClass="formField" /></div>
            </div>
            <div  id="dvComplianceComments" runat="server" class="row">
                <div class="col-sm-3 text-right"><span class="formLabel170">Comments</span></div>
                <div class="col-sm-9"><asp:TextBox runat="server" ID="txtComplianceComments" TextMode="MultiLine" Rows="4" CssClass="formField" /></div>
            </div>
            <br />
            <div id="dvplaceHolder" class="row" runat ="server">
    <br />
             <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel170">Notice Of Deficiency </span></div>
                 </div>
                <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel170">Issue Date </span></div>
                    <div runat="server" class="col-sm-9">
                        <asp:TextBox ID="txtIssueDate" runat="server" CssClass="formField" />
                <ajax:CalendarExtender ID="CalendarExtender1" TargetControlID="txtIssueDate" runat="server" />
                <asp:CompareValidator id="CompareValidator1" runat="server" 
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtIssueDate" ValidationGroup="SiteVisitAttempt"
                    ErrorMessage="Select a valid Issue Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> 
                </asp:CompareValidator>
                 </div>
                    </div>
                <div class="row">
                    <div class="col-sm-3 text-right"><span class="formLabel170">Services to be Removed? </span></div>
                    <div class="col-sm-3" ><input type="checkbox" ID="chkServiceRemoved" runat="server" style="width: 1em; height: 1em;" /></div>
                    </div>
                </div>
                <div class="row" id="dvPlaceHolderUpload" runat="server">
                <asp:PlaceHolder runat="server" 
                   ID="PlaceholderUploadSiteVisit"></asp:PlaceHolder>
                </div>
            <div id="dvPlanOfCorrection" class="row" runat ="server">
                <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel170">Plan Of Correction </span></div>
                 </div>
                <div class="row">
                <div class="col-sm-3 text-right"><span class="formLabel170">Date of Plan of Correction </span></div>
                    <div runat="server" class="col-sm-9"><asp:TextBox ID="txtplanofCorr" runat="server" CssClass="formField" />
                <ajax:CalendarExtender ID="CalendarExtender2" TargetControlID="txtplanofCorr" runat="server" />
                <asp:CompareValidator id="CompareValidator2" runat="server" 
                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtplanofCorr" ValidationGroup="SiteVisitAttempt"
                    ErrorMessage="Select a valid Plan of Correction Date" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                    SetFocusOnError="true"> 
                </asp:CompareValidator>
                 </div>
                    </div>
                </div>
                <div class="row" id="dvplaceHolder1Upload" runat="server">
                <asp:PlaceHolder runat="server" 
                   ID="PlaceholderUploadSiteVisit1"></asp:PlaceHolder>
                </div>
                
            <div class="bntBox" id="dvSave" runat="server">
                <asp:Button runat="server" ID="btnSave" CssClass="buttonBoxFocus" Text="Save" ValidationGroup="SiteVisitAttempt" CausesValidation="true" OnClick="btnSave_Click"/>
                <asp:Button runat="server" ID="btnCancelSiteVisit" CssClass="buttonBox" Text="Cancel" CausesValidation="false" OnClick="btnCancelSiteVisit_Click"/>
            </div>
            <br />
    </div>
    <asp:HiddenField ID="hdnchkValue" runat="server" ClientIDMode="Static"/>
</asp:Panel>

