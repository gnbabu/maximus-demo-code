<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_C4CodeSets" Codebehind="C4CodeSets.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<style type="text/css">
    .RadGrid_PDMSModern .rgFilterBox {
        background-color: #fff !important;
        font-size: medium !important;
        color: #000;
        height: 30px !important;
        width: 70%;
    }

    caption {
        visibility: hidden !important
    }
</style>

<style type="text/css">
    .RadComboBox_PDMSModern, .RadComboBox_PDMSModern .rcbInputCell .rcbInput, .RadComboBoxDropDown_PDMSModern {
    }

    .RadComboBoxDropDown_PDMSModern {
        font: 16px;
    }

    .RadComboBox_PDMSModern .rcbArrowCell a {
        height: 44px !important;
        width: 35px !important;
    }

    .RadComboBox_PDMSModern td.rcbInputCell, .RadComboBox_PDMSModern .rcbInputCell .rcbInput {
        padding-top: 6px !important;
    }

    .rcbHeader ul,
    .rcbFooter ul,
    .rcbItem ul,
    .rcbHovered ul,
    .rcbDisabled ul {
        margin: 0;
        padding: 0;
        width: 100%;
        display: inline-block;
        list-style-type: none;
    }

    .rcbScroll {
        overflow: scroll !important;
        overflow-x: hidden !important;
    }

    .col2,
    .col3 {
        margin: 0;
        padding: 0 5px 0 0;
        width: 20%;
        line-height: 14px;
        float: left;
    }

    .col1 {
        width: 80%;
        margin: 0;
        padding: 0 5px 0 0;
        line-height: 14px;
        float: left;
        display: inline-block;
    }


    .needAttentionbg .rtbIcon {
        background: url('../Images/bullet-red.png') no-repeat;
        background-position: 100% 0 !important;
        padding-right: 10px;
        content: '';
    }

    .completebg .rtbIcon {
        background: url('../Images/StepCheck.png') no-repeat;
        background-position: 100% 0 !important;
        content: '';
        padding-right: 20px;
    }

    .inProcessbg .rtbIcon {
        background: url('../Images/InProcess.png') no-repeat;
        background-position: 100% 0 !important;
        content: '';
        padding-right: 20px;
    }

    .progressScroll {
        margin-left: 8px;
        margin-right: 8px;
        overflow-y: hidden;
        overflow-x: scroll;
        scrollbar-face-color: #d2d2d2;
        scrollbar-highlight-color: #ebebeb;
        scrollbar-3dlight-color: #ebebeb;
        scrollbar-shadow-color: #d2d2d2;
        scrollbar-darkshadow-color: #000000;
        scrollbar-track-color: #e3e3f3;
        scrollbar-arrow-color: #545487;
    }

    .hidden-alt-text {
        visibility: hidden;
    }
</style>

<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">

    <p style="text-align: center" class="page-main-header">
        <asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management (RDM)"></asp:Literal>
    </p>
    <br />
</asp:Panel>

<div class="row">
    <div class="col-sm-3 text-right">
        <span class="ohio-field">Codesets</span>
    </div>
    <div class="col-sm-9 text-left">
        <%--<asp:DropDownList ID="ddlC4CodeSet" runat="server" CssClass="formDropDown" OnInit="ddlC4CodeSet_Init" > </asp:DropDownList>--%>
        <telerik:radcombobox id="ddlC4CodeSet" runat="server" width="600px" enableariasupport="true" ariasettings-label="Jump To"
            markfirstmatch="true" enableloadondemand="false" enableembeddedskins="false" skin="PDMSModern"
            highlighttemplateditems="true" onselectedindexchanged="RadJumpTo_SelectedIndexChanged"
            onitemdatabound="RadJumpTo_ItemDataBound">

            <headertemplate>
                <ul>
                    <li class="col1">Code Name</li>
                    <li class="col3">Status</li>
                </ul>
            </headertemplate>

            <itemtemplate>
                <ul>
                    <li class="col1"><%# DataBinder.Eval(Container.DataItem, "Text") %></li>
                    <li class="col3" style="text-align: center;"
                        <%# string.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Status").ToString()) ? "style='display:none'" : "" %>>
                        <asp:Image ImageUrl='<%# DataBinder.Eval(Container.DataItem, "Status") %>' ID="ImageStatus" AlternateText="" runat="server" />
                    </li>
                </ul>
            </itemtemplate>
        </telerik:radcombobox>
    </div>
</div>
<div class="row row-gap">
    <div class="col-sm-3 text-right">
        <span class="ohio-field">Approval Status</span>
    </div>
    <div class="col-sm-9 text-left">
        <asp:Literal ID="lblApprovalStatus" runat="server"> </asp:Literal>
    </div>
</div>
<div class="row row-gap">
    <div class="col-sm-3 text-right">
        <span class="ohio-field">Last Load Date</span>
    </div>
    <div class="col-sm-9 text-left">
        <asp:Literal ID="lblLoadDate" runat="server"> </asp:Literal>
    </div>
</div>
<div class="row row-gap">
    <div class="col-sm-3 text-right">
        <span class="ohio-field">Review Date</span>
    </div>
    <div class="col-sm-9 text-left">
        <asp:Literal ID="lblReviewDate" runat="server"> </asp:Literal>
    </div>
</div>
<div class="row text-center" style="padding-top: 20px; padding-bottom: 20px;">
    <asp:Button ID="btnReview" Text="Review"
        runat="server" CssClass="buttonBox buttonBoxFocus" OnClick="btnReview_Click"></asp:Button>
    <asp:Button ID="btnApprove" runat="server" Text="Approve Changes" CssClass="buttonBox" OnClick="btnApprove_Click"></asp:Button>
    <asp:Button ID="btnReject" runat="server" Text="Reject Changes" CssClass="buttonBox" OnClick="btnReject_Click"></asp:Button>
</div>
<div class="divGrid">
    <asp:GridView
        runat="server"
        Width="95%"
        ID="gvC4CodeSets"
        AutoGenerateColumns="False"
        HorizontalAlign="Center"
        CssClass="gridview"
        EmptyDataText="No data found"
        AllowPaging="true"
        PageSize="100"
        OnPageIndexChanging="gvC4CodeSets_PageIndexChanging">
        <Columns>
            <asp:BoundField ReadOnly="true" DataField="C4_Code" HeaderText="C4 Code" />
            <asp:BoundField ReadOnly="true" DataField="C4_Description" HeaderText="C4 Description" />
            <asp:BoundField ReadOnly="true" DataField="PNM_Code" HeaderText="PNM Code" />
            <asp:BoundField ReadOnly="true" DataField="PNM_Description" HeaderText="PNM Description" />
            <asp:BoundField ReadOnly="true" DataField="PNM_Status" HeaderText="PNM STATUS (Is Deleted?)" />
        </Columns>
    </asp:GridView>
</div>
