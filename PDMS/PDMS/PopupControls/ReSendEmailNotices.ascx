<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReSendEmailNotices" Codebehind="ReSendEmailNotices.ascx.cs" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<style type="text/css">
    select {
        min-width: 90%;
    }

    .form-row {
        display: flex;
        align-items: center;
        gap: 20px; /* spacing between dropdown and checkbox */
    }

    .form-group {
        display: flex;
        flex-direction: column;
    }

    .checkbox-group {
        display: flex;
        padding-left: 5%;
        gap: 8px;
    }

    .styled-dropdown {
        padding: 6px 12px;
        font-size: 16px;
    }

    .styled-checkbox input[type="checkbox"] {
        width: 20px;
        height: 20px;
        cursor: pointer;
    }

.rowREN {
  display: flex;
  justify-content: center;
  gap: 10px;
  flex-wrap: wrap; /* Allows wrapping on smaller screens */
}

@media (max-width: 600px) {
  .rowREN {
    flex-direction: column;
    align-items: center;
  }

  .usa-button {
    width: 100%;
    max-width: 300px; /* Optional: limits button width on mobile */
  }
}


/* Base button style */
.usa-button {
  display: inline-block;
  padding: 0.5rem 1.25rem;
  font-weight: 700;
  text-align: center;
  text-decoration: none;
  color: #ffffff;
  background-color: #005ea2;
  border: none;
  border-radius: 0.25rem;
  cursor: pointer;
  min-width: 250px; /* Ensures consistent width */
  height: 40px;      /* Ensures consistent height */
  line-height: 1.5;
  box-sizing: border-box;
}

.usa-button:hover {
  background-color: #1a4480;
}

.usa-button:focus {
  outline: 3px solid #ffbf47;
  outline-offset: 2px;
}

/* Secondary button */
.usa-button--secondary {
  background-color: #e6e6e6;
  color: #1a1a1a;
}

.usa-button--secondary:hover {
  background-color: #d9d9d9;
}

/* Danger button */
.usa-button--danger {
  background-color: #d54334;
  color: #ffffff;
}

.usa-button--danger:hover {
  background-color: #b7352d;
}

/* Outline button */
.usa-button--outline {
  background-color: transparent;
  color: #005ea2;
  border: 2px solid #005ea2;
}

.usa-button--outline:hover {
  background-color: #e6f2ff;
}

.checkbox-switch {
    cursor: pointer;
    display: inline-block;
    overflow: hidden;
    position: relative;
    text-align: left;
    width: 80px;
    height: 30px;
    -webkit-border-radius: 30px;
    border-radius: 30px;
    line-height: 1.2;
    font-size: 14px;
    margin-left: 5%;
}

.checkbox-switch input.input-checkbox {
	position: absolute;
	left: 0;
	top: 0;
	width: 80px;
	height: 30px;
	padding: 0;
	margin: 0;
	opacity: 0;
	z-index: 2;
	cursor: pointer;
}

.checkbox-switch .checkbox-animate {
    position: relative;
    width: 80px;
    height: 30px;
    background-color: #95a5a6;
    -webkit-transition: background 0.25s ease-out 0s;
    transition: background 0.25s ease-out 0s;
}

.checkbox-switch .checkbox-animate:before {
	content: "";
	display: block;
	position: absolute;
	width: 20px;
	height: 20px;
	border-radius: 10px;
	-webkit-border-radius: 10px;
	background-color: #7f8c8d;
	top: 5px;
	left: 5px;
	 -webkit-transition: left 0.3s ease-out 0s;
    transition: left 0.3s ease-out 0s;
    z-index: 10;
}

.checkbox-switch input.input-checkbox:checked + .checkbox-animate {
	background-color: #2ecc71;
}

.checkbox-switch input.input-checkbox:checked + .checkbox-animate:before {
	left: 55px;
	background-color: #27ae60;
}

.checkbox-switch .checkbox-off,
.checkbox-switch .checkbox-on {
	float: left;
	color: #fff;
	font-weight: 700;
	padding-top: 6px;
	 -webkit-transition: all 0.3s ease-out 0s;
    transition: all 0.3s ease-out 0s;
}

.checkbox-switch .checkbox-off {
	margin-left: 30px;
	opacity: 1;
}

.checkbox-switch .checkbox-on {
	display: none;
	float: right;
	margin-right: 35px;
	opacity: 0;
}

.checkbox-switch input.input-checkbox:checked + .checkbox-animate .checkbox-off {
	display: none;
	opacity: 0;
}

.checkbox-switch input.input-checkbox:checked + .checkbox-animate .checkbox-on {
	display: block;
	opacity: 1;
}

.gridViewSmallFont2 {
    font-size: 15px;
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    border-collapse: collapse;
    width: 100%;
}

.gridViewSmallFont2 th {
    font-size: 15px;
    background-color: #545486; /* Light gray or any solid color */
    color: #333;
    padding: 10px;
    border: 1px solid #ccc;
    text-align: left;
}


.gridViewSmallFont2 td {
    padding: 8px;
    border: 1px solid #ddd;
    vertical-align: top;
}

.gridViewSmallFont2 tr:nth-child(even) {
    background-color: #fafafa;
}

.gridViewSmallFont2 .aspNetDisabled {
    color: #999;
}

.gridViewSmallFont2 .pager,
.gridViewSmallFont2 td[align="right"] {
    background-color: #f4f4f4;
    color: #333;
    padding: 8px;
    border-top: 1px solid #ccc;
    text-align: right;
}

.gridViewSmallFont2 a {
    color: #007bff;
    text-decoration: none;
    margin: 0 5px;
}

.gridViewSmallFont2 a:hover {
    text-decoration: underline;
}

.gridViewPager td{
    background-color: #ffffff !important;
}

</style>
<br />
<div style="border-top: 2px solid black; margin-bottom: -10px">
    <br />
</div>

<asp:Panel ID="pnlReSendEmailNotices" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div id="div1" runat="server" class="test-left" style="text-align: left;">
        <span class="pdspnlReSendEmailNotices" id="Span1" runat="server"><b>Re-Send Email Notice Dashboard ( Job Scheduled for 6:15am EST )</b></span>
        <hr />
    </div>
    <div>
        <asp:ValidationSummary ID="valSummaryError" runat="server" DisplayMode="List" ForeColor="Red" ValidationGroup="ReSendNoticesVG" ShowSummary="true" />
        <br />
    </div>
</asp:Panel>
<asp:Panel ID="Panel2" runat="server" Style="min-width: 300px; height: auto; width: auto; max-width: 1200px;">
    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-12 col-md-12 col-lg-12 outerName">
                <asp:Label ID="lblGenXML" CssClass="ohio-field" AssociatedControlID="txtRegIdsToReSend" runat="server">
                    <span class="ohio-field-label">Registration IDs / IDs for Selection/Deletion: <span class="ohio-tooltip fa-info-circle" data-toggle="popover"
                        data-trigger="hover" data-placement="right" data-content="Enter comma-separated Registration IDs (e.g., 1,2,3,4) to select records, or enter system-generated IDs to delete the corresponding notices." aria-hidden="true"></span>
                    </span>
                    <asp:TextBox ID="txtRegIdsToReSend" CssClass="ohio-field-input" runat="server" TextMode="MultiLine" />
                </asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-9 col-md-9 col-lg-9 outerName">
                <asp:Label ID="lblNoticeTypesID" class="ohio-select" AssociatedControlID="ddlNoticeTypesID" runat="server">
                    <span class="ohio-select-label">Select Notice Type: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover"
                        data-placement="right" data-content="Choose the type of notice you want to view or resend. This may include options like Successfull, Revalidation, Termination, or General Communication. Selecting the correct notice type helps filter the results accordingly."
                        aria-hidden="true"></span>
                    </span>
                    <span class="ohio-select-select fa">
                        <asp:DropDownList ID="ddlNoticeTypesID" CssClass="ohio-select-select-el" runat="server" AutoPostBack="False">
                        </asp:DropDownList>
                    </span>
                </asp:Label>
            </div>
            <div class="col-sm-3 col-md-3 col-lg-3 outerName">
                <asp:Label ID="lblSendPaperNotice" class="ohio-select" runat="server">
                    <span class="ohio-select-label">Send Paper Notices: <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover"
                        data-placement="right" data-content="Select Send Paper Notices"
                        aria-hidden="true"></span>
                    </span>
                </asp:Label>
               <div class="checkbox-switch">
                   <input type="checkbox" onchange="T.toggleToobarStatus()" value="1" name="ckbSendPaperNoticeID" class="input-checkbox" id="toolbar-active">
                   <div class="checkbox-animate">
                      <span class="checkbox-off">OFF</span>
                      <span class="checkbox-on">ON</span>
                   </div>
                </div>
            </div>
        </div>
        <div class="rowREN" style="padding-left: 1in; ">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="rowREN" style="display: flex; justify-content: center; gap: 10px;">
                        <asp:Button ID="btnResendSelectedNotices" runat="server" Text="Resend Selected Notices" OnClick="btnResendSelectedNotices_Click" CssClass="usa-button" />
                        <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CssClass="usa-button usa-button--secondary usa-button--danger" />
                        <asp:Button ID="btnDeleteAll" runat="server" Text="Delete All" OnClick="btnDeleteAll_Click" CssClass="usa-button usa-button--outline usa-button--danger" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnResendSelectedNotices" />
                    <asp:PostBackTrigger ControlID="btnClear" />
                    <asp:PostBackTrigger ControlID="btnDeleteAll" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <br />
    <br />
    <asp:GridView
        ID="gvQueryResendSelectedNotices"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="gridViewSmallFont2" Width="100%"
        AllowSorting="true"
        EmptyDataText="No Records Found"
        RowStyle-VerticalAlign="Top"
        AllowPaging="true"
        AllowCustomPaging="true"
        PageSize="10"
        GridViewSortDirection="Descending"
        PageIndexChanged="gvQueryResendSelectedNotices_PageIndexChanged"
        OnPageIndexChanging="gvQueryResendSelectedNotices_PageIndexChanging"
        OnRowCommand="gvQueryResendSelectedNotices_RowCommand"
        DataKeyNames="ID">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID" />
            <asp:BoundField DataField="REG_ID" HeaderText="Registration ID" />
            <asp:BoundField DataField="NOTICES_TYPE" HeaderText="Notice Type" />
            <asp:BoundField DataField="NOTICE_SUBJECT" HeaderText="Notice Subject" />
            <asp:BoundField DataField="PAPER_NOTICE" HeaderText="Paper Notice?" />
            <asp:BoundField DataField="STATUS" HeaderText="Status" />
            <asp:BoundField DataField="LOAD_DATE" HeaderText="Load Date" />
            <asp:BoundField DataField="SENT_DATE" HeaderText="Sent Date" />
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteResendSelectedNoticesRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                        ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" AlternateText="Delete" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:HiddenField ID="hdnRowCount" runat="server" />
</asp:Panel>
