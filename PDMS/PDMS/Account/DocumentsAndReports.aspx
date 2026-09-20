<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Account_DocumentsAndReports"  Codebehind="DocumentsAndReports.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="wc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    Documents Selection
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<style type="text/css">
    .modal
    {
        position: fixed;
        top: 0;
        left: 0;
        background-color: black;
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }
    .loading
    {
        font-family: Arial;
        font-size: 10pt;
        width: 200px;
        height: 100px;
        display: none;
        position: fixed;
        z-index: 999;
    }
    .failureNotification
    {
        color:Red!important;
        text-align:left;
    }

    .buttonLinkBox 
{
    /* Old button background-color was #3d94f6 replaced with #07457E */
	-moz-box-shadow:inset 0px 1px 0px 0px #97c4fe;
	-webkit-box-shadow:inset 0px 1px 0px 0px #97c4fe;
	box-shadow:inset 0px 1px 0px 0px #97c4fe;
	background:-webkit-gradient( linear, left top, left bottom, color-stop(0.05, #07457E), color-stop(1, #1e62d0) );
	background:-moz-linear-gradient( center top, #07457E 5%, #1e62d0 100% );
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#07457E', endColorstr='#1e62d0');
	background-color: #07457E;
	-moz-border-radius:6px;
	-webkit-border-radius:6px;
	border-radius:6px;
	border:1px solid #337fed;
	display:inline-block;
	color:#ffffff;
	font-family:arial;
	font-size:13px;
	font-weight:bold;
	padding:1px 6px;
	text-decoration:none;
	text-shadow:1px 1px 0px #1570cd;
}

    #test a
    {
            color:#ffffff !important;
            text-decoration:none;
    }

        #test a:active 
        {
                    color:#ffffff !important;
                    text-decoration:none;
        }

        #test  a:visited 
        {
                    color:#ffffff !important;
                    text-decoration:none;
        }

        #test  a:hover { 
                    color:#ffffff !important;
                    text-decoration:none;
        }

</style>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript">
        function ShowProgress() {
            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        $('form').live("submit", function () {
        if(Page_IsValid)
            ShowProgress();
        });

        function numericOnly(obj) {
            obj.value = obj.value.replace(/[^0-9]/g, '');
        }

    </script>

            <ajax:TabContainer runat="server" ID="Tabs" ActiveTabIndex="2" >
            <ajax:TabPanel runat="server" ID="Panel1" HeaderText="e-Remmittance Advice">
                        <ContentTemplate>
    <center>
                            <wc:GroupBox ID="GroupBox1"  Caption="Selection" CaptionStyle-CssClass="bodyTextBold" Width="98%" runat="server">
            <div><asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" /></div>
            <table>
                <colgroup>
                    <col width="50%" />
                    <col width="50%" />
                </colgroup>
                <tr>
                    <td>
                        <span runat="server" id="spanMID" class="formLabel150">Medicaid ID:</span>
                        <asp:TextBox ID="txtMedicaidId" runat="server" CssClass="formField130" MaxLength="9" />
                                            <asp:RequiredFieldValidator ID="rvMedicaid" runat="server" ControlToValidate="txtMedicaidId" ErrorMessage="Medicaid ID is required" Text="*" Display="Dynamic" ValidationGroup="vGroupeRA"/>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <span class="formLabel150">From:</span>
                        <ajax:CalendarExtender 
                            ID="calFromDate" 
                            runat="server" 
                            Format="MM/dd/yyyy"  
                            TargetControlID="txtFromDate" 
                            PopupPosition="BottomRight"  
                            CssClass="QstCalendarCSS" 
                            PopupButtonID="imgFromDate"  
                            EnabledOnClient="true" />
                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="formField130" />
                        <asp:CompareValidator 
                            id="dateFromValidator" 
                            runat="server"  
                            Type="Date" 
                            Operator="DataTypeCheck" 
                            ControlToValidate="txtFromDate"  
                            ErrorMessage="Select a valid From date" 
                            Text="*" 
                            Display="Dynamic" 
                            ValueToCompare="MM/dd/yyyy"
                            SetFocusOnError="true"> 
                        </asp:CompareValidator>
                                            <asp:RequiredFieldValidator ID="rvFromDate" runat="server" ControlToValidate="txtFromDate" ErrorMessage="From Date is required" Text="*" Display="Dynamic" ValidationGroup="vGroupeRA"/>
                        <asp:Image ID="imgFromDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" />
                        <br /><asp:RangeValidator ID="FromRangeValidator" runat="server" ControlToValidate="txtFromDate" ErrorMessage="Select a proper date" Type="Date" />
                        <ajax:ValidatorCalloutExtender ID="FromRangeValidator_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="FromRangeValidator" />
                    </td>
                    <td>
                        <span class="formLabel150">To:</span>
                        <ajax:CalendarExtender 
                            ID="calToDate" 
                            runat="server" 
                            Format="MM/dd/yyyy"  
                            TargetControlID="txtToDate" 
                            PopupPosition="BottomRight"  
                            CssClass="QstCalendarCSS" 
                            PopupButtonID="imgToDate"  
                            EnabledOnClient="true" />
                        <asp:TextBox ID="txtToDate" runat="server" CssClass="formField130" />
                        <asp:CompareValidator 
                            id="dateToValidator" 
                            runat="server"  
                            Type="Date" 
                            Operator="DataTypeCheck" 
                            ControlToValidate="txtToDate"  
                            ErrorMessage="Select a valid To date" 
                            Text="*" 
                            Display="Dynamic" 
                            ValueToCompare="MM/dd/yyyy"
                            SetFocusOnError="true"> 
                        </asp:CompareValidator>
                                            <asp:RequiredFieldValidator ID="rvToDate" runat="server" ControlToValidate="txtToDate" ErrorMessage="To Date is required" Text="*" Display="Dynamic" ValidationGroup="vGroupeRA"/>
                        <asp:Image ID="imgToDate" runat="server" ImageUrl="~/Images/Calendar_scheduleHS.bmp" Height="16px" />
                        <br /><asp:RangeValidator ID="ToRangeValidator" runat="server" ControlToValidate="txtToDate" ErrorMessage="Select a proper date" Type="Date" />
                        <ajax:ValidatorCalloutExtender ID="ToRangeValidator_ValidatorCalloutExtender" runat="server" Enabled="True" TargetControlID="ToRangeValidator" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:center;padding:5px">
                                            <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="Search" CssClass="buttonBox" onclick="btnSearch_Click" validationGroup="vGroupeRA" />&nbsp;
                        <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBox" onclick="btnClear_Click" />
                    </td>
                </tr>
            </table>
        </wc:GroupBox>
        <asp:Label runat="server" id="ServerError" class="failureNotification" visible="False" />
        <asp:GridView 
            ID="gvDocs" 
            runat="server" 
            AutoGenerateColumns="False" 
            CssClass="gridViewSmallFont" 
            AllowSorting="true" 
            EmptyDataText="No matching records found." 
            RowStyle-VerticalAlign="Top" 
            AllowPaging="True" 
            PageSize="15" 
            CellPadding="3" 
            PagerSettings-Mode="NumericFirstLast" 
            OnRowCommand="gvDocs_RowCommand"
            DataKeyNames="DocId, DocTypeId, MedicaidID"
            OnPageIndexChanging="gvDocs_PageIndexChanging"
            OnSorting="myGridView_Sorting" >
            <Columns>
                <asp:BoundField DataField="DocType" HeaderText="Document Type" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="DocTypeId" Visible="false" />
                <asp:BoundField DataField="ReportDate" HeaderText="Document Date" SortExpression="ReportDate"  ItemStyle-HorizontalAlign="Center"  />
                <asp:BoundField DataField="MedicaidID" HeaderText="Medicaid ID" Visible="true" SortExpression="MedicaidID"  ItemStyle-HorizontalAlign="Center"  />
                <asp:BoundField DataField="DocId" HeaderText="Document ID" Visible="false" />
                <asp:BoundField DataField="ProviderId" Visible="false" />
                <asp:TemplateField HeaderText="Action" SortExpression=""  ItemStyle-HorizontalAlign="Center" >
                    <ItemTemplate>
                        <asp:LinkButton 
                            ID="lbtnText" 
                            runat="server" 
                            CausesValidation="false" 
                            Text="Download as Text" 
                            CommandName="GetReportText"
                            CommandArgument='<%# Container.DataItemIndex %>'
                            CssClass="gridLink" Width="150"/> 
                        <asp:LinkButton 
                            ID="lbtnPDF" 
                            runat="server" 
                            CausesValidation="false" 
                            Text="Download as PDF" 
                            CommandName="GetReportPDF"
                            CommandArgument='<%# Container.DataItemIndex %>'
                            CssClass="gridLink" Width="150"/>
                    </ItemTemplate> 
                </asp:TemplateField>
            </Columns>   
            <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />  
            <HeaderStyle CssClass="gridViewHeader" />
            <AlternatingRowStyle CssClass="gridViewAltRow" />
            <RowStyle CssClass="gridViewRow" /> 
            <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
        <div class="loading" align="center">
    <br />
    <br />
    <img src="../Images/loader.gif" alt=""  />
</div>

    </center>
                       </ContentTemplate>
            </ajax:TabPanel>
            
            <ajax:TabPanel runat="server" ID="Panel3" HeaderText="X12 eRA" >
                <ContentTemplate>
                    <center>
                        <wc:GroupBox ID="GroupBox2"  Caption="Selection" CaptionStyle-CssClass="bodyTextBold" Width="98%" runat="server">
                                                <span class="formLabel150">Medicaid Id:</span>
                                       <asp:TextBox ID="txtMedcaidIdx12" runat="server" CssClass="formField130" MaxLength="9" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMedcaidIdx12" ErrorMessage="Medicaid ID is required" Text="*" Display="Dynamic" ValidationGroup="vGroupex12"/>
                                        <asp:DropDownList ID="ddlx12eRAMedicaidIdList" runat="server" CssClass="formDropDown"  AppendDataBoundItems="true" />
                        </wc:GroupBox>
                        <p class="bodyTextRed">
                            <asp:Image ID="Warning" ImageUrl="~/Images/Warning-icon.png" runat="server" Height="24px" />  WARNING!!! Please save the document. Your X12 835 transactions will be deleted after they are downloaded
                        </p>
            <br />                        <%--<asp:Button ID="btnX12eRADownload" runat="server" CausesValidation="true" Text="Download" CssClass="buttonBox" onclick="btnX12eRADownload_Click"  validationGroup="vGroupex12" />--%>&nbsp;

                        <div id="test">
                                                <asp:LinkButton ID="btnX12eRADownload" runat="server" CausesValidation="true" Text="Download" Font-Underline="false"  CssClass="buttonLinkBox" Width="150"  onclick="btnX12eRADownload_Click" validationGroup="vGroupex12"  />
                            </div>
        <div class="loading" align="center">
    <br />
    <br />
    <img src="../Images/loader.gif" alt=""  />
</div>
                    </center>
                </ContentTemplate>
            </ajax:TabPanel>
        
            <ajax:TabPanel runat="server" ID="Panel2"  HeaderText="270/271">
                <ContentTemplate>
		<p> <br /> Coming Soon</p>
                </ContentTemplate>
            </ajax:TabPanel>
        </ajax:TabContainer>
</asp:Content>
