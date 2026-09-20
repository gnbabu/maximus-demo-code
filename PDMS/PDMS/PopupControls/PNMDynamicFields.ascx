<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_PNMDynamicFields" Codebehind="PNMDynamicFields.ascx.cs" %>
<%@ register assembly="SCS.WebControls.GroupBox" namespace="SCS.WebControls" tagprefix="cc1" %>
<%@ register src="~/PopupControls/MessageBox.ascx" tagname="MessageBox" tagprefix="cc2" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>
<%@ register tagprefix="telerik" namespace="Telerik.Web.UI" assembly="Telerik.Web.UI" %>
<%@ register src="~/PopupControls/DynamicControlInfo.ascx" tagname="dynamicControlsInfo" tagprefix="uc1" %>





<style type="text/css">
    .DynamicControlFix {
        display: none;
    }

    .DynamicControlSelectValuesFix {
        display: none;
    }

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

    .redBoldText {
        color: red;
    }
</style>
<%--<script type="text/javascript">

    function hideControls() {
        value = $("#ddlControlTypeId").val();
        debugger;
        if (value == "") {
            $("#txtSelectedValues").hide();
            $("#txtOptions").hide();
        }
    }
</script>--%>

<div style="padding-top: 10px">
    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2">
        </div>
        <div class="col-sm-6 text-left">
            <span class="formLabel150">
                <asp:label id="lblError" runat="server" text="" visible="false" cssclass="redBoldText" />
            </span>
        </div>
    </div>
    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:label id="lblFieldName" runat="server" associatedcontrolid="txtFieldName" text="Field Name" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:textbox id="txtFieldName" runat="server" cssclass="textEntry" autopostback="false" maxlength="100" />
            <asp:requiredfieldvalidator id="RequiredFieldValidator3" runat="server" controltovalidate="txtFieldName"
                errormessage="* Enter a Field Name"
                enabled="true" setfocusonerror="true"
                validationgroup="DynamicControlFix" display="Dynamic" forecolor="Red" />
        </div>
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:label id="lblRegID" runat="server" associatedcontrolid="txtRegID" text="Registration ID" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:textbox id="txtRegID" runat="server" cssclass="textEntry" autopostback="false" maxlength="100" />
        </div>
    </div>
    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:label id="Label1" runat="server" associatedcontrolid="ddlSectionTypeId" text="Section Type" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:dropdownlist runat="server" id="ddlSectionTypeId" cssclass="formDropDown" autopostback="false">
                <asp:listitem text="Select an Action" value=""></asp:listitem>
            </asp:dropdownlist>
            <asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" controltovalidate="ddlSectionTypeId"
                errormessage="* Select a valid Section Type"
                enabled="true" setfocusonerror="true"
                validationgroup="DynamicControlFix" display="Dynamic" forecolor="Red" />
        </div>
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:label id="lblProvTypeId" runat="server" associatedcontrolid="ddlProvTypeId" text="MMIS Provider Type ID" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">

            <asp:dropdownlist id="ddlProvTypeId" runat="server" autopostback="false" cssclass="formDropDown">
                <asp:listitem text="Select an Action" value=""></asp:listitem>
            </asp:dropdownlist>


            <asp:requiredfieldvalidator id="RequiredFieldValidator4" runat="server" controltovalidate="ddlProvTypeId"
                errormessage="* Select a Provider Type"
                enabled="true" setfocusonerror="true"
                validationgroup="DynamicControlFix" display="Dynamic" forecolor="Red" />
        </div>
    </div>
    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:label id="lblControlType" runat="server" associatedcontrolid="ddlControlTypeId" text="Control Type" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">

            <asp:dropdownlist runat="server" id="ddlControlTypeId" cssclass="formDropDown" autopostback="true" onselectedindexchanged="ddlControlTypeId_SelectedIndexChanged">
                <asp:listitem text="Select an Action" value=""></asp:listitem>
            </asp:dropdownlist>
            <asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" controltovalidate="ddlControlTypeId"
                errormessage="* Select a valid Control Type"
                enabled="true" setfocusonerror="true"
                validationgroup="DynamicControlFix" display="Dynamic" forecolor="Red" />
        </div>
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:label id="lblDataTypeId" runat="server" associatedcontrolid="ddlDataTypeId" text="Data Type" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">

            <asp:dropdownlist runat="server" id="ddlDataTypeId" cssclass="formDropDown" autopostback="false">
                <asp:listitem text="Select an Action" value=""></asp:listitem>
            </asp:dropdownlist>
            <asp:requiredfieldvalidator id="valddlDataTypeId" runat="server" controltovalidate="ddlDataTypeId"
                errormessage="* Select a valid Data Type"
                enabled="true" setfocusonerror="true"
                validationgroup="DynamicControlFix" display="Dynamic" forecolor="Red" />
        </div>
    </div>
    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:label id="lblChildControl" runat="server" associatedcontrolid="ckbParentChildControl" text="Child Control?" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:checkbox runat="server" id="ckbParentChildControl" AutoPostBack="false">
            </asp:checkbox>
        </div>
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:label id="lblChildControlIDs" runat="server" associatedcontrolid="ddlChildControlIDs" text="Child Control" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <asp:dropdownlist runat="server" id="ddlChildControlIDs" cssclass="formDropDown" autopostback="false">
                <asp:listitem text="Select Child Control" value=""></asp:listitem>
            </asp:dropdownlist>
        </div>
    </div>
    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:label id="lblSelectedValues" runat="server" associatedcontrolid="txtSelectedValues" text="Select Values" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">

            <asp:textbox id="txtSelectedValues" runat="server" cssclass="textEntry" textmode="MultiLine" style="height: 200px;" enabled="false" rows="50" columns="75" />
        </div>
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:label id="lblMultiValueOptions" runat="server" associatedcontrolid="txtOptions" text="Options" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">

            <asp:textbox id="txtOptions" runat="server" cssclass="textEntry" maxlength="10" />
            <asp:requiredfieldvalidator id="RequiredFieldValidator5" runat="server" controltovalidate="txtOptions"
                errormessage="* Enter a value for selected control type"
                enabled="true" setfocusonerror="true"
                validationgroup="DynamicControlSelectValuesFix" display="Dynamic" forecolor="Red" />
        </div>
    </div>
    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:label id="Label4" runat="server" associatedcontrolid="lblApprovalStatus" text="Approval Status" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <div class="col-sm-2 text-right">
                <span class="formLabel150">
                    <asp:label id="lblApprovalStatus" runat="server" text="" />
                </span>&nbsp;&nbsp;
            </div>
        </div>
    </div>

    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:label id="Label2" runat="server" associatedcontrolid="lblApprovalDate" text="Approval Date" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <div class="col-sm-2 text-right">
                <span class="formLabel150">
                    <asp:label id="lblApprovalDate" runat="server" text="" />
                </span>&nbsp;&nbsp;
            </div>
        </div>
    </div>

    <div class="row" style="margin-top: 10px">
        <div class="col-sm-2 text-right">
            <span class="formLabel150">
                <asp:label id="Label3" runat="server" associatedcontrolid="lblReviewDate" text="Review Date" />
            </span>&nbsp;&nbsp;
        </div>
        <div class="col-sm-4 text-left">
            <div class="col-sm-2 text-right">
                <span class="formLabel150">
                    <asp:label id="lblReviewDate" runat="server" text="" />
                </span>&nbsp;&nbsp;
            </div>
        </div>
    </div>

    <br />
    <div class="row" style="margin-top: 10px; margin-left: 23%; text-align: center">
        <asp:button id="btnClearValues" runat="server" text="Clear Selected Values" cssclass="buttonBoxFocus" onclick="btnCelarValues_Click" />
        <asp:button id="btnAppendValues" runat="server" text="Append to Selected Values" cssclass="buttonBoxFocus" validationgroup="DynamicControlSelectValuesFix" onclick="btnAppendValues_Click" />
        <asp:button id="btnSave" runat="server" causesvalidation="true" text="Save" cssclass="buttonBoxFocus" validationgroup="DynamicControlFix"
            onclick="btnSave_Click" />
        <asp:button id="BtnClear" runat="server" causesvalidation="true" text="Clear" cssclass="buttonBoxFocus"
            onclick="btnClear_Click" />
    </div>
</div>
<div>
    <asp:validationsummary id="valSummaryDynamicControlFix" runat="server" displaymode="List" validationgroup="DynamicControlFix" cssclass="DynamicControlFix" />
</div>
<div>
    <asp:validationsummary id="ValSummaryDynamicControlSelectValues" runat="server" displaymode="List" validationgroup="DynamicControlSelectValuesFix" cssclass="DynamicControlSelectValuesFix" />
</div>

<br />
<div style="margin-top: 10px">

    <telerik:radgrid rendermode="Lightweight" runat="server" id="DynamicGrid1" allowpaging="true" cssclass="gridViewSmallFont grd-x-scroll"
        width="100%"
        autogeneratecolumns="true"
        autogeneratedeletecolumn="true"
        autogenerateeditcolumn="true"
        showstatusbar="True"
        allowfilteringbycolumn="true"
        skin="PDMSModern"
        pagesize="20" style="max-width: 100%;"
        allowdelete="true">

        <mastertableview commanditemdisplay="Top" editmode="InPlace" autogeneratecolumns="true" allowdelete="true">

            <pagerstyle mode="NextPrevAndNumeric" alwaysvisible="true" pagesizelabeltext="Page Size: " pagesizes="10,50,100,500,1000,10000" />
        </mastertableview>
        <filtermenu onclientshowing="MenuShowing" cssclass="gridviewFilter" />
    </telerik:radgrid>

</div>

<!--confirm pop-up modal -->
<asp:updatepanel id="upConfirmAdd" runat="server">
    <contenttemplate>
        <ajax:modalpopupextender id="mpeConfirmAdd" runat="server" popupcontrolid="pnlConfirmAdd" targetcontrolid="ButtonDummy1"
            backgroundcssclass="modalBackground" behaviorid="mpeConfirmAdd">
        </ajax:modalpopupextender>
        <asp:panel id="pnlConfirmAdd" runat="server" cssclass="modalPopup" style="display: none; width: 25%; height: auto;">
            <asp:panel id="pnlConfirmTitle" cssclass="popHeader" runat="server">
                <div class="popTitle">
                    <asp:label id="lbl_title" runat="server" text="Information" />
                </div>
            </asp:panel>
            <br />
            <asp:panel id="pnConfirmMsg" runat="server" style="margin-right: 10px">
                <div style="text-align: left; padding: 15px; margin-left: 30px !important" class="container-fluid">
                    <div class="row">
                        <p style="text-align: left; background-color: white; width: 90%; margin-left: 5%;">
                            <br />
                            <asp:label id="lblConfirm" runat="server" text="Your record has been saved successfully." />
                        </p>
                    </div>
                </div>
            </asp:panel>
            <br />
            <div class="btnBox btnBoxCenter" style="padding-top: 10px; margin-right: 15px; width: 93%;">
                <asp:button runat="server" id="btnOk" text="Ok" cssclass="buttonBox" onclick="btnOk_Click" />
            </div>
        </asp:panel>
        <asp:button runat="server" id="ButtonDummy1" style="display: none" text="”ButtonDummy1”" />
    </contenttemplate>
</asp:updatepanel>
<div id="divProviderFeed" runat="server">
    <uc1:dynamiccontrolsinfo id="dynamicControlsInfo" runat="server" mode="Grid" />
</div>


<asp:hiddenfield id="hdnWebAPIURLs" runat="server" />
<asp:hiddenfield id="hdnRegIds" runat="server" />

