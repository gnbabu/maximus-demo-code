<%@ control language="C#" autoeventwireup="true" inherits="UserControls_ReferenceData_NUBCCodeSets, App_Web_iq0r534d" %>
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
    visibility:hidden !important
      }
    </style>
   
<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
    
        <h1 style="text-align: center;font-size: 25px;font-weight: bold;"><asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management (RDM)"></asp:Literal></h1>
   
</asp:Panel>

<div class="row">
    <div class="col-sm-3 text-right">Codesets</div>
    <div class="col-sm-9 text-left">
        <asp:DropDownList ID="ddlNUBCCodeSet" runat="server" CssClass="formDropDown" OnInit="ddlNUBCCodeSet_Init" > </asp:DropDownList>
    </div>     
</div> 
<div class="row">
    <div class="col-sm-3 text-right">Approval Status</div>
    <div class="col-sm-9 text-left">
        <asp:literal ID="lblApprovalStatus" runat="server"   > </asp:literal>
    </div>     
</div> 
 <div class="row">
    <div class="col-sm-3 text-right">Last Load Date</div>
    <div class="col-sm-9 text-left">
        <asp:literal ID="lblLoadDate" runat="server"   > </asp:literal>
    </div>     
</div> 
<div class="row">
    <div class="col-sm-3 text-right">Review Date</div>
    <div class="col-sm-9 text-left">
        <asp:literal ID="lblReviewDate" runat="server"   > </asp:literal>
    </div>     
</div> 
<div class="row text-center">
    <asp:Button  ID="btnReview" Text="Review"
        runat="server"  CssClass="buttonBox buttonBoxFocus" OnClick="btnReview_Click" ></asp:Button>
    <asp:Button  ID="btnApprove" runat="server" Text="Approve Changes" runat="server" CssClass="buttonBox" OnClick="btnApprove_Click"></asp:Button>
    <asp:Button  ID="btnReject" runat="server" Text="Reject Changes" runat="server" CssClass="buttonBox" OnClick="btnReject_Click"></asp:Button>
</div> 
<div class="divGrid">
    <asp:GridView 
        runat="server" 
        Width="95%" 
        ID="gvNUBCCodeSets"
        AutoGenerateColumns="False" 
        HorizontalAlign="Center" 
        CssClass="gridview" 
        EmptyDataText="No data found"
        AllowPaging="true"
        PageSize = "100"
        OnPageIndexChanging="gvNUBCCodeSets_PageIndexChanging">
        <Columns>     
            <asp:BoundField ReadOnly="true" DataField="NUBC_Code" HeaderText="NUBC Code" />
            <asp:BoundField ReadOnly="true" DataField="NUBC_Description" HeaderText="NUBC Description" />
            <asp:BoundField ReadOnly="true" DataField="PNM_Code" HeaderText="PNM Code" />
            <asp:BoundField ReadOnly="true" DataField="PNM_Description" HeaderText="PNM Description" />
            <asp:BoundField ReadOnly="true" DataField="PNM_Status" HeaderText="PNM STATUS (Is Deleted?)" />
        </Columns>
    </asp:GridView>
</div>