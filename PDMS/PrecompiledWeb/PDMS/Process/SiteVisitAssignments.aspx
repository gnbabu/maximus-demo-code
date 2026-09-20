<%@ page language="C#" autoeventwireup="true" inherits="Process_SiteVisitAssignments, App_Web_qtcaivva" masterpagefile="~/MasterPage.master" title="Site Visit Assignments" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/MessageModal.ascx" TagName="MessageBox" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    <h1>Site Visit Assignments</h1>
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
        .hidecol {
            display: none;
        }

        caption {
            visibility:hidden;
}
        .td {
            background-color: #999999;
        }  
        
        .display-search-fields {
            display:flex
        }
        .row {
            margin-top: 10px;
        }

        @media only screen and (max-width: 760px){
            .display-search-fields {
                display:flex;
                flex-direction:column;
            }

            input[type=text], input[type=password], textarea, select {
                width: 100% !important;
                min-width: 150px;
            }
            .WhiteBox{
                padding: 0px;
            }

        }

        .fieldLabel {
            text-align: left;
        }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="WhiteBox">
        <div>
            <asp:ValidationSummary ID="vsSiteVisitAssignment" runat="server" DisplayMode="List" ValidationGroup="valSiteVisitAssignment" />
        </div>
        <asp:Panel ID="pnlMyDashBoard" runat="server">
               <div class="boxContainer"><asp:Label ID="Separator1" runat="server" Text="My DashBoard" CssClass="boxLabel" Visible="false" /></div>
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
    Width="100%"
                >
            </asp:GridView>
        </asp:Panel>
        <br />
        <div class="col-sm-12 col-md-12 display-search-fields" style="margin-left:0px">
            <div class="col-sm-12 col-md-12 col-lg-6">
<%--                <td class="wd100 fieldLabel IncreaseTo200">
                        <asp:Label ID="lblVisitType" runat="server" Text="Visit Type:" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlVisitType" runat="server"   CssClass="DropDownList" />
                    </td>--%>
                   
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <asp:Label ID="lblAttempt" runat="server" CssClass="fieldLabel" Text="Attempt:" />
                    </div>
                    <div class="col-sm-8 text-left">
                        <asp:DropDownList ID="ddlAttempt" runat="server" aria-label="attempt" CssClass="form-control unsetPublicSearchDDLLength" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <asp:Label ID="lblZip" CssClass="fieldLabel" runat="server" Text="Zip:" />
                    </div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtZip" CssClass="form-control" runat="server" MaxLength="5" aria-label="zip" onkeyup="return numericOnly(this)" CausesValidation="true"/>
                        <asp:RegularExpressionValidator ID="regexp1" runat="server" ValidationExpression="^\d{5}$"
                            ControlToValidate="txtZip" ErrorMessage="* Enter 5 digit Zip Code" Text="*This is a required field" Display="Dynamic"
                            SetFocusOnError="true" ValidationGroup="valSiteVisitAssignment" CssClass="bodyText" />
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-2 text-right">
                        <asp:Label ID="lblZipExt" CssClass="fieldLabel" runat="server" Text="Zip Ext:"  />
                    </div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtZipExt" CssClass="form-control" runat="server" aria-label="zip" MaxLength="4"  onkeyup="return numericOnly(this)"/>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="^\d{4}$"
                            ControlToValidate="txtZipExt" ErrorMessage="* Enter 4 digit Zip Code" Text="*This is a required field" Display="Dynamic"
                            SetFocusOnError="true" ValidationGroup="valSiteVisitAssignment" CssClass="bodyText"/>
                    </div>
                </div>
               
            </div>
            <div class="col-sm-12 col-md-12 col-lg-6">
                <div class="row">
                    <div class="col-sm-4 text-right">
                        <asp:Label ID="lblDueByFromDate" CssClass="fieldLabel" runat="server" Text="Due By From Date:" />
                    </div>
                    <div class="col-sm-8 text-left">
                         <asp:TextBox ID="txtDueByFromDate" runat="server" aria-label="due by from date" CssClass="form-control" />
                        <ajax:CalendarExtender ID="clrDueByFromDate" TargetControlID="txtDueByFromDate" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4 text-right">
                        <asp:Label ID="lblDueByToDate" CssClass="fieldLabel" runat="server" Text="Due By To Date:"  />
                    </div>
                    <div class="col-sm-8 text-left">
                        <asp:TextBox ID="txtDueByToDate" runat="server" aria-label="due by to date" CssClass="form-control" />
                        <ajax:CalendarExtender ID="clrDueByToDate" TargetControlID="txtDueByToDate" runat="server" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-4 text-right">
                        <asp:Label ID="lblSearchAssignToUser" CssClass="fieldLabel" runat="server" Text="Assign To User" />
                    </div>
                    <div class="col-sm-8 text-left">
                        <asp:DropDownList ID="ddlSearchAssignToUser" runat="server" aria-label="search assign to user"  CssClass="form-control unsetPublicSearchDDLLength" />
                    </div>
                </div>
            </div>
        </div>

        <div class="col-sm-12 col-md-12">
        <br />
        <br />
                <div class="row" style="text-align:center; margin-top: 10px">
                    <asp:Button ID="btnSearch" runat="server" Text="Search"  CssClass="buttonBox"  OnClick="btnSearch_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear"  CssClass="buttonBox"  OnClick="btnClear_Click"/>
                </div>
        <br />
        <br />         
        </div>
        <asp:Panel id="pnlPendingSitevisits" runat="server">
               <div class="boxContainer"><asp:Label ID="UMS01" runat="server" Text="Pending Site Visits" CssClass="boxLabel" Visible="false" /></div>
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
                OnRowDataBound="gvPendingSiteVisits_RowDataBound">
                <Columns>
                    <asp:TemplateField ShowHeader="False" HeaderText="<span style='display:none'>SiteVisit</span>" SortExpression="">
                        <ItemTemplate>
                            <asp:CheckBox 
                    ID="chkAssign" ToolTip="Select to assign" 
                    runat="server" 
                    CausesValidation="false" 
                    CommandName="Reg_id"
                   
                    
                    />
                            <p style="display:none;">
                            <asp:Label ID="lblcheckAssign" runat="server" AssociatedControlID="chkAssign" Text="Check Assign"></asp:Label></p>
                            <asp:HiddenField ID="lblregid" runat="server" Value='<%# Eval("reg_id") %>' />
                            <asp:HiddenField ID="hdnstepid" runat="server" Value='<%# Eval("step_id") %>' />
                            <asp:HiddenField ID="hdnpid" runat="server" Value='<%# Eval("process_id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="name" HeaderText="Name" SortExpression="Name" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="provider_type_name" HeaderText="Provider Type" SortExpression="provider_type_name" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="location" HeaderText="Location(Zip)" SortExpression="location" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="Due_Date" HeaderText="Due By" DataFormatString="{0:MM/dd/yyyy}" HtmlEncode="False" SortExpression="Due_Date" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="site_visit_type_name" HeaderText="Visit Type" SortExpression="site_visit_type_name" Visible="false" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="attempt" HeaderText="Attempt" SortExpression="attempt" HeaderStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="owner_id" HeaderText="Assigned To" SortExpression="owner_id" HeaderStyle-HorizontalAlign="Left" />
                </Columns>
                <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />
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
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox"/>
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <uc:MessageBox ID="MessageBox2" runat="server" />
        </asp:Panel>
    </div>
</asp:Content>