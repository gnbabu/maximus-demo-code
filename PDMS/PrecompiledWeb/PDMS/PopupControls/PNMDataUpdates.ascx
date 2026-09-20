<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_PNMDataUpdates, App_Web_wenzyumt" %>
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

<div>
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
            <span class="actionLabel">
                <asp:Label ID="actionLabel" runat="server" AssociatedControlID="actionDropDown" Text="Action" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:DropDownList runat="server" ID="actionDropDown" AutoPostBack="true" OnSelectedIndexChanged="actionDropDown_SelectedIndexChanged">
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
            <asp:DropDownList runat="server" ID="tableDropdownId" ValidationGroup="DataFix">
                <asp:ListItem Text="Select a Table" Value=""></asp:ListItem>
            </asp:DropDownList>
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
    <div class="row" style="margin-top: 10px; margin-left: 23%">
        <asp:Button ID="btnSearch" runat="server" CausesValidation="true" Text="OK" OnClick="btnSearch_Click" CssClass="buttonBoxFocus" ValidationGroup="DataFix" Width="20%" Enabled="false" />

        <asp:Button ID="btnSave" runat="server" CausesValidation="true" Text="Save" OnClick="table_dropdown_SelectedIndexChanged" CssClass="buttonBoxFocus" ValidationGroup="DataFix" Width="20%" />
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
