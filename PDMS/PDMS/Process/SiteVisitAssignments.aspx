<%@ Page Language="C#" AutoEventWireup="true" Inherits="Process_SiteVisitAssignments" MasterPageFile="~/MasterPage.master" Title="Site Visit Assignments" Codebehind="SiteVisitAssignments.aspx.cs" %>

<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="display: inline;" class="page-main-header">
        <br />
        <span style="text-align: center">Site Visit Assignments</span>
    </div>
    <script type="text/javascript">
        function numericOnly(obj) {
            obj.value = obj.value.replace(/[^0-9]/g, '');


        }
        $(document).ready(function () {
            var $table = $("#ctl00_MainContent_gvPendingSiteVisits").find("table");
            $table.attr("role", "presentation");
        })
    </script>
    <style type="text/css">
        .page-main-header {
            font-family: 'Merriweather', serif !important;
            font-size: 28px !important;
        }

        .WhiteBox {
            font-family: 'Source Sans Pro', sans-serif !important;
        }

        .hidecol {
            display: none;
        }

        caption {
            visibility: hidden;
        }

        .td {
            background-color: #999999;
        }

        .display-search-fields {
            display: flex
        }

        .row {
            margin-top: 10px;
        }

        .form-control {
            font-size: 17px;
            height: 44px;
            color: #000;
            font-weight: normal;
        }

        @media only screen and (max-width: 760px) {
            .display-search-fields {
                display: flex;
                flex-direction: column;
            }

            input[type=text], input[type=password], textarea, select {
                width: 100% !important;
                min-width: 150px;
            }

            .WhiteBox {
                padding: 0px;
            }
        }

        .fieldLabel {
            text-align: left;
            float:left;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="WhiteBox">
        <div>
            <asp:ValidationSummary ID="vsSiteVisitAssignment" runat="server" DisplayMode="List" ValidationGroup="valSiteVisitAssignment" />
        </div>
        <asp:Panel ID="pnlMyDashBoard" runat="server">
            <div class="boxContainer">
                <asp:Label ID="Separator1" runat="server" Text="My DashBoard" CssClass="boxLabel" Visible="false" />
            </div>
            <br />
            <asp:GridView ID="grdMyDashBoard" runat="server" CssClass="gridViewSmallFont"
                title="Dashboard"
                role="presentation"
                EmptyDataText="No Providers found."
                HeaderStyle-HorizontalAlign="Left"
                RowStyle-VerticalAlign="Top"
                RowStyle-HorizontalAlign="Left"
                AllowPaging="True"
                PageSize="15"
                CellPadding="3"
                PagerSettings-Mode="NumericFirstLast"
                Width="100%">
            </asp:GridView>
        </asp:Panel>
        <br />
        <div class="col-sm-12 col-md-12 display-search-fields" style="margin-left: 0px">
            <div class="col-sm-12 col-md-12 col-lg-6">
                <%--                <td class="wd100 fieldLabel IncreaseTo200">
                        <asp:Label ID="lblVisitType" runat="server" Text="Visit Type:" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlVisitType" runat="server"   CssClass="DropDownList" />
                    </td>--%>

                <div class="row">
                    <div class="col-sm-3 text-right">
                        <asp:Label ID="lblAttempt" runat="server" CssClass="fieldLabel" Text="Attempt:" />
                    </div>
                    <div class="col-sm-8 text-left">
                        <asp:DropDownList ID="ddlAttempt" runat="server" aria-label="attempt" CssClass="form-control unsetPublicSearchDDLLength" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-3 text-right">
                        <asp:Label ID="lblRegistrationID" runat="server" CssClass="fieldLabel" Text="Registration ID:" />
                    </div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtRegID" runat="server" CssClass="form-control" />
                    </div>
                </div>
            </div>
            <div class="col-sm-12 col-md-12 col-lg-6">
                <div class="row">
                    <div class="col-sm-4 text-right">
                        <asp:Label ID="lblSearchAssignToUser" CssClass="fieldLabel" runat="server" Text="Assign To User" />
                    </div>
                    <div class="col-sm-8 text-left">
                        <asp:DropDownList ID="ddlSearchAssignToUser" runat="server" aria-label="search assign to user" CssClass="form-control unsetPublicSearchDDLLength" />
                    </div>
                </div>
            </div>
        </div>

        <div class="col-sm-12 col-md-12">
            <br />
            <br />
            <div class="row" style="text-align: center; margin-top: 10px">
                <asp:Button ID="btnSiteVisitNeeded" runat="server" Text="Yes-Site Visit Needed" CssClass="buttonBoxFocus" OnClick="btnSiteVisitNeeded_Click" />
                <asp:Button ID="btnSiteVisitNotNeeded" runat="server" Text="No-Site Visit Not Needed" CssClass="buttonBox" OnClick="btnSiteVisitNotNeeded_Click" />
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="buttonBoxFocus" OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" />

            </div>
            <div class="row" style="text-align: right; margin-top: 10px">
                <telerik:radgrid id="RadGridExportSVA" runat="server" visible="true" skin="PDMSModern" enableembeddedskins="false">
                    <exportsettings ignorepaging="true" openinnewwindow="true">
                        <pdf pageheight="8.5in" pagewidth="11in" pagetitle="Affiliate Search">
                            <pagefooter>
                                <rightcell text="Page <?page-number?>" />
                            </pagefooter>
                        </pdf>
                    </exportsettings>
                    <mastertableview autogeneratecolumns="false">
                        <columns>
                            <telerik:gridboundcolumn uniquename="col1" headertext="Reviewed By" datafield="REVIEWED_BY_USER"></telerik:gridboundcolumn>
                            <telerik:gridboundcolumn uniquename="col2" headertext="Site Visit Needed" datafield="SITE_VISIT_NEEDED"></telerik:gridboundcolumn>
                            <telerik:gridboundcolumn uniquename="col3" headertext="Reg ID" datafield="REG_ID"></telerik:gridboundcolumn>
                            <telerik:gridboundcolumn uniquename="col4" headertext="Name" datafield="NAME"></telerik:gridboundcolumn>
                            <telerik:gridboundcolumn uniquename="col5" headertext="Provider Type" datafield="PROVIDER_TYPE_NAME"></telerik:gridboundcolumn>
                            <telerik:gridboundcolumn uniquename="col6" headertext="Attempt" datafield="ATTEMPT"></telerik:gridboundcolumn>
                            <telerik:gridboundcolumn uniquename="col7" headertext="Assigned To" datafield="OWNER_ID_NAME"></telerik:gridboundcolumn>
                            <telerik:gridboundcolumn uniquename="col8" headertext="Notes" datafield="SITE_VISIT_DISPOSITION_COMMENTS"></telerik:gridboundcolumn>
                        </columns>
                    </mastertableview>
                </telerik:radgrid>
                <asp:LinkButton ID="lnkSVAExcel" runat="server" ToolTip="Excel" OnClick="lnkSVAExcel_Click" OnClientClick="return exportPopup();" Visible="false">
                    <img src="../Images/Excel_24x24.png" alt="XLS" /></asp:LinkButton>&nbsp;&nbsp;
            </div>
            <br />
            <br />
        </div>
        <asp:Panel ID="pnlPendingSitevisits" runat="server">
            <div class="boxContainer">
                <asp:Label ID="UMS01" runat="server" Text="Pending Site Visits" CssClass="boxLabel" Visible="false" />
            </div>
            <br />
            <asp:GridView
                ID="gvPendingSiteVisits"
                runat="server"
                role="presentation"
                title="Pending site visits"
                AutoGenerateColumns="False"
                CssClass="gridViewSmallFont"
                EmptyDataText="No Providers found."
                HeaderStyle-HorizontalAlign="Left"
                RowStyle-VerticalAlign="Top"
                RowStyle-HorizontalAlign="Left"
                AllowPaging="True"
                PageSize="15"
                CellPadding="3"
                PagerSettings-Mode="NumericFirstLast"
                Width="100%" OnPageIndexChanging="gvPendingSiteVisits_PageIndexChanging"
                OnRowDataBound="gvPendingSiteVisits_RowDataBound" OnRowCommand="gvPendingSiteVisits_RowCommand"
                DataKeyNames="REG_ID, SITE_VISIT_DISPOSITION_COMMENTS,PROCESS_ID">
                <Columns>
                    <asp:TemplateField ShowHeader="False" HeaderText="<span style='display:none'>SiteVisit</span>" SortExpression="">
                        <ItemTemplate>
                            <asp:CheckBox
                                ID="chkAssign" ToolTip="Select to assign"
                                runat="server"
                                CausesValidation="false"
                                CommandName="Reg_id" />
                            <p style="display: none;">
                                <asp:Label ID="lblcheckAssign" runat="server" AssociatedControlID="chkAssign" Text="Check Assign"></asp:Label>
                            </p>
                            <asp:HiddenField ID="lblregid" runat="server" Value='<%# Eval("REG_ID") %>' />
                            <asp:HiddenField ID="hdnstepid" runat="server" Value='<%# Eval("STEP_ID") %>' />
                            <asp:HiddenField ID="hdnpid" runat="server" Value='<%# Eval("PROCESS_ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="REVIEWED_BY_USER" HeaderText="Reviewed By" SortExpression="REVIEWED_BY_USER" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="SITE_VISIT_NEEDED" HeaderText="Site Visit Needed" SortExpression="SITE_VISIT_NEEDED" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="REG_ID" HeaderText="Reg ID" SortExpression="REG_ID" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="NAME" HeaderText="Name" SortExpression="NAME" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="PROVIDER_TYPE_NAME" HeaderText="Provider Type" SortExpression="PROVIDER_TYPE_NAME" HeaderStyle-HorizontalAlign="Left" />
                    <%--<asp:BoundField DataField="location" HeaderText="Location(Zip)" SortExpression="location" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Due_Date" HeaderText="Due By" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" SortExpression="Due_Date" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="site_visit_type_name" HeaderText="Visit Type" SortExpression="site_visit_type_name" Visible="false" HeaderStyle-HorizontalAlign="Left" />--%>
                    <asp:BoundField DataField="ATTEMPT" HeaderText="Attempt" SortExpression="ATTEMPT" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="OWNER_ID_NAME" HeaderText="Assigned To" SortExpression="OWNER_ID_NAME" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="SITE_VISIT_DISPOSITION_COMMENTS" HeaderText="Notes" HeaderStyle-HorizontalAlign="Left" />
                    <asp:TemplateField HeaderText="Add Note" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:LinkButton
                                ID="lbtnAddNote"
                                runat="server"
                                CausesValidation="false"
                                Text="Add Note"
                                CommandName="AddNote"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                CssClass="gridLink" Width="150" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TASK_NAME" HeaderText="TASK_NAME" HeaderStyle-HorizontalAlign="Left" Visible="false" />
                </Columns>
                <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="gridViewHeader" HorizontalAlign="Left" />
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" />
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
            <asp:Panel runat="server" ID="pnlSiteVisitAssign" Visible="false">
                <table class="TablePadding5" role="presentation">
                    <tr>
                        <td class="fieldLabel wd100 IncreaseTo250">
                            <asp:Label ID="lblAssignToUser" runat="server" Text="Assign To User" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSiteVisitOper" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnAssign" runat="server" Text="Assign" CssClass="buttonBox" OnClick="btnAssign_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <uc:messagebox id="MessageBox2" runat="server" />
        </asp:Panel>
        <asp:HiddenField ID="hdnRegisID" runat="server" />
        <asp:HiddenField ID="hdnProcessID" runat="server" />
        <ajax:modalpopupextender id="mpeSVAAddNote" runat="server"
            popupcontrolid="pnlSVAAddNote" targetcontrolid="btnSVANoteDummy" cancelcontrolid="btnHideNoteSVA" backgroundcssclass="modalBackground">
        </ajax:modalpopupextender>

        <asp:Panel ID="pnlSVAAddNote" runat="server" CssClass="modalPopup" Style="display: none; min-height: 200px; min-width: 600px; height: auto; width: auto; position: fixed; top: 300px; left: 700px;">
            <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
                <div class="popTitle">
                    <div>
                        <span style="text-align: left;">Add Notes</span>&nbsp;&nbsp;
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="plnTextArea" runat="server" Style="padding: 10px">
                <div class="center">
                    <div class="row">
                        <div class="col-sm-3 text-right" style="padding-right: 0px;"><span>Comments:*</span></div>
                        <div class="col-sm-9 text-left">
                            <asp:TextBox ID="txtSVAComments" runat="server" MaxLength="500" ValidationGroup="ValNotProcessed" TextMode="MultiLine" Columns="40" Rows="6" />
                            <asp:RequiredFieldValidator ID="valCommentRequired" runat="server" ControlToValidate="txtSVAComments"
                                ErrorMessage="* Comments are required." Display="Dynamic" Text="*"
                                ValidationGroup="ValNotProcessed" />
                        </div>
                        <div class="row ">
                            <div class="center">
                                <asp:Button runat="server" ID="btnSaveNoteSVA" Text="Save" CssClass="buttonBox" OnClick="btnSaveNoteSVA_Click" />
                                <asp:Button runat="server" ID="btnHideNoteSVA" Text="Cancel" CssClass="buttonBox"
                                    CausesValidation="false" OnClick="btnHideNoteSVA_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Button runat="server" ID="btnSVANoteDummy" aria-Label="dummybutton" Style="display: none" />
    </div>
</asp:Content>
