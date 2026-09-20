<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_PNMDataUpdates" Codebehind="PNMDataUpdates.ascx.cs" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="../UserControls/DataFix/DelegateAffiliationUploads.ascx" TagName="DelegateAffiliationUploads" TagPrefix="uc" %>


<style type="text/css">
    .rgEdit {
        width: 15px;
        height: 15px;
        display: inline-block;
        text-indent: -17px !important;
    }

    .rgDelIcon {
        width: 15px;
        height: 15px;
        display: inline-block;
        text-indent: -17px !important;
    }

    .RadCalendarPopup {
        background: #d2deef;
    }

    .RadCalendarPopupShadows {
        background: #d2deef;
    }

    .ChkBoxClass input {
        width: 25px;
        height: 25px;
        text-align: center;
        font-size: 14px;
        margin: 5px;
    }
</style>

<div style="padding-top: 40px">


    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:Label ID="actionLabel" runat="server" AssociatedControlID="actionDropDown" Text="Action" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:DropDownList runat="server" ID="actionDropDown" AutoPostBack="true" CssClass="DropDownList" OnSelectedIndexChanged="actionDropDown_SelectedIndexChanged">
                <asp:ListItem Text="Select an Action" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-sm-2 text-right">
        </div>
        <div class="col-sm-4 text-left">
            <asp:CheckBox ID="chkSendHistory" runat="server" Text="Send History" Visible="false" CssClass="ChkBoxClass"></asp:CheckBox>
        </div>
    </div>

    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:Label ID="tableDropDownLableID" runat="server" AssociatedControlID="tableDropdownId" Text="Page" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:DropDownList runat="server" ID="tableDropdownId" CssClass="DropDownList" AutoPostBack="true" OnSelectedIndexChanged="tableDropdownId_SelectedIndexChanged" ValidationGroup="DataFix">
                <asp:ListItem Text="Select a Table" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>
    </div>
    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:Label ID="lblRegId" runat="server" AssociatedControlID="txtRegId" Text="Reg ID" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">

            <asp:TextBox ID="txtRegId" runat="server" CssClass="textEntry" AutoPostBack="true" MaxLength="10" OnTextChanged="txtRegId_TextChanged" />

            <asp:RegularExpressionValidator ID="valtxtRegId" runat="server" ControlToValidate="txtRegId"
                ValidationExpression="\d{0,10}" ErrorMessage="* Enter a valid Reg ID Number"
                Enabled="true" SetFocusOnError="true"
                ValidationGroup="DataFix" Display="Dynamic" ForeColor="Red" />
        </div>

        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:Label ID="lblMedId" runat="server" AssociatedControlID="txtMedId" Text="MED ID" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:TextBox ID="txtMedId" runat="server" CssClass="textEntry" MaxLength="10" AutoPostBack="true" OnTextChanged="txtMedId_TextChanged" />

            <asp:RegularExpressionValidator ID="valtxtMedId" runat="server" ControlToValidate="txtMedId"
                ValidationExpression="\d{0,10}" ErrorMessage="* Enter a valid MED ID Number"
                Enabled="true" SetFocusOnError="true"
                ValidationGroup="DataFix" Display="Dynamic" ForeColor="Red" />
        </div>

    </div>

    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:Label ID="lblProcessID" runat="server" AssociatedControlID="txtProcessID" Text="Process ID" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">

            <asp:TextBox ID="txtProcessID" runat="server" CssClass="textEntry" AutoPostBack="true" />

            <asp:RegularExpressionValidator ID="valProcessID" runat="server" ControlToValidate="txtProcessID"
                ValidationExpression="\d{0,10}" ErrorMessage="* Enter a valid Process ID Number"
                Enabled="true" SetFocusOnError="true"
                ValidationGroup="DataFix" Display="Dynamic" ForeColor="Red" />
        </div>
    </div>
    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:Label ID="lblEndDate" runat="server" AssociatedControlID="txtEndDate" Text="End Date" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">

            <asp:TextBox ID="txtEndDate" runat="server" CssClass="textEntry" AutoPostBack="true" />
            <ajax:calendarextender id="CalendarExtender1" targetcontrolid="txtEndDate" runat="server" />
            <asp:CompareValidator ID="CompareValidator1" runat="server" ValidationGroup="DataFix"
                Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEndDate" Enabled="true"
                ErrorMessage="* A valid End Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                SetFocusOnError="true" />


        </div>
    </div>

    <asp:MultiView ID="mltDataFixes" runat="server" ActiveViewIndex="0" EnableViewState="true">
        <asp:View ID="vwDelegateAffiliations" runat="server">
            <uc:delegateaffiliationuploads id="ucDelegateAffiliations" runat="server" />
        </asp:View>
        <asp:View ID="View3" runat="server">
        </asp:View>
    </asp:MultiView>
    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:Label ID="lblTaskID" runat="server" AssociatedControlID="txtTaskID" Text="Task ID" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">

            <asp:TextBox ID="txtTaskID" runat="server" CssClass="textEntry" AutoPostBack="true" />

            <asp:RegularExpressionValidator ID="valTaskID" runat="server" ControlToValidate="txtTaskID"
                ValidationExpression="\d{0,10}" ErrorMessage="* Enter a valid Task ID Number"
                Enabled="true" SetFocusOnError="true"
                ValidationGroup="DataFix" Display="Dynamic" ForeColor="Red" />
        </div>
    </div>

    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:Label ID="lblApprovalStatusLabel" runat="server" AssociatedControlID="lblApprovalStatus" Text="Approval Status" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <div class="col-sm-2 text-right">
                <span class="formLabel150">
                    <asp:Label ID="lblApprovalStatus" runat="server" Text="" />
                </span>&nbsp;&nbsp;
            </div>
        </div>
    </div>

    <div class="row" style="margin-top: 10px;">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:Label ID="lblApprovalDateLabel" runat="server" AssociatedControlID="lblApprovalDate" Text="Approval Date" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <div class="col-sm-2 text-right">
                <span class="formLabel150">
                    <asp:Label ID="lblApprovalDate" runat="server" Text="" />
                </span>&nbsp;&nbsp;
            </div>
        </div>
    </div>

    <div class="row" style="margin-top: 10px;">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:Label ID="lblReviewDateLabel" runat="server" AssociatedControlID="lblReviewDate" Text="Review Date" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <div class="col-sm-2 text-right">
                <span class="formLabel150">
                    <asp:Label ID="lblReviewDate" runat="server" Text="" />
                </span>&nbsp;&nbsp;
            </div>
        </div>
    </div>

    <div class="btnBoxCenter">
        <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="OK" OnClick="btnSearch_Click" CssClass="buttonBoxFocus" ValidationGroup="DataFix" Enabled="false" />
        <asp:Button ID="btnSave" runat="server" CausesValidation="true" Text="Save" OnClick="table_dropdown_SelectedIndexChanged" CssClass="buttonBoxFocus" ValidationGroup="DataFix" />
        <asp:Button ID="btnClear" runat="server" CausesValidation="false" Text="Clear" CssClass="buttonBox" OnClick="btnClear_Click" />
    </div>


    <div style="text-align: right; padding-right: 30px">
        <span>
            <asp:ImageButton ID="lnkExcel" runat="server" ImageUrl="~/Images/Excel_24x24.png"
                OnClick="lnkExcel_Click" AlternateText="ExcelML" Visible="false" />
        </span>
    </div>



</div>
<div>
    <asp:ValidationSummary ID="valSummaryDataFix" runat="server" DisplayMode="List" ValidationGroup="DataFix" CssClass="failureNotification" />
</div>

<div style="margin-top: 10px">

    <telerik:radgrid rendermode="Lightweight" runat="server" id="DynamicGrid1" allowpaging="true" cssclass="gridViewSmallFont grd-x-scroll"
        width="100%"
        autogeneratecolumns="true"
        autogeneratedeletecolumn="true"
        oneditcommand="DynamicGrid1_EditCommand"
        oninsertcommand="DynamicGrid1_InsertCommand"
        autogenerateeditcolumn="true"
        showstatusbar="True"
        onitemdatabound="DynamicGrid1_ItemDataBound"
        onitemcommand="DynamicGrid1_ItemCommand"
        onupdatecommand="DynamicGrid1_UpdateCommand"
        ondeletecommand="DynamicGrid1_DeleteCommand"
        oncancelcommand="DynamicGrid1_CancelCommand"
        onpageindexchanged="DynamicGrid1_PageIndexChanged"
        oncolumncreated="DynamicGrid1_ColumnCreated"
        allowfilteringbycolumn="true"
        skin="PDMSModern"
        onpagesizechanged="DynamicGrid1_PageSizeChanged"
        pagesize="20" style="max-width: 100%;"
        allowdelete="true">

        <mastertableview commanditemdisplay="Top" editmode="InPlace" autogeneratecolumns="true" allowdelete="true">

            <pagerstyle mode="NextPrevAndNumeric" alwaysvisible="true" pagesizelabeltext="Page Size: " pagesizes="10,50,100,500,1000,10000" />
        </mastertableview>
        <filtermenu onclientshowing="MenuShowing" cssclass="gridviewFilter" />
    </telerik:radgrid>

</div>


<ajax:modalpopupextender id="mpeConfirmActions" runat="server" popupcontrolid="pnlConfirmActions" targetcontrolid="ButtonDummy1"
    backgroundcssclass="modalBackground" behaviorid="mpeConfirmActions">
</ajax:modalpopupextender>

<asp:Panel ID="pnlConfirmActions" runat="server" CssClass="modalPopup" Style="display: none; width: 35%; height: auto; font-family: 'Source Sans Pro', sans-serif !important; position: relative;">

    <asp:Panel ID="pnlConfirmActionsTitle" CssClass="popHeader" runat="server" Style="position: relative;">
        <div class="popConfirmActionTitle" style="text-align: center; padding-right: 30px; position: relative;">
            <asp:Label ID="lbl_confirm_title" runat="server" Style="color: white;" Text="Confirm Your Action" />
            <span id="btnCloseModal" title="Close"
                style="cursor: pointer; position: absolute; top: -2px; right: 5px; font-size: 28px; color: white; font-weight: bold;">&times;
            </span>
        </div>
    </asp:Panel>

    <br />

    <asp:Panel ID="pnConfirmActionMsg" runat="server">
        <div class="container-fluid">
            <div class="row">
                <p style="text-align: center; background-color: white; font-size: 20px;">
                    <br />
                    <asp:Label ID="lblConfirmAction" runat="server" Text="Please choose any of the below actions to proceed:" />
                </p>
            </div>
        </div>
    </asp:Panel>

    <br />

    <div class="btnBoxCenter" style="padding-top: 10px; padding-bottom: 40px;">
        <asp:Button runat="server" ID="btnCancel" Text="Cancel" Visible="false" CssClass="buttonBox" OnClick="btnCancel_Click" />
        <asp:Button runat="server" ID="btnapplyNow" Style="margin-left: 10px;" Text="Apply Changes Now" CssClass="buttonBox" OnClick="btnApplyNow_Click" />
        <asp:Button runat="server" ID="btnqueueProcessing" Style="margin-left: 10px;" Text="Queue for Processing" CssClass="buttonBoxFocus" OnClick="btnQueueProcessing_Click" />
    </div>

</asp:Panel>

<asp:Button runat="server" ID="ButtonDummy1" Style="display: none" Text="ButtonDummy1" />

<script>
    document.getElementById('btnCloseModal').addEventListener('click', function () {
        $find('mpeConfirmActions').hide();
    });
</script>
