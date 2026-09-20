<%@ Page Language="C#" AutoEventWireup="true" Inherits="Workflow_WF_EngineNew" Codebehind="WF_EngineNew.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        .pauseModalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.2;
            height: auto;
        }

        .pauseModalPopup {
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding: 0px;
            width: auto;
            height: auto;
        }
    </style>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <br />
            <br />
            <table style="width: 100%;">
                <asp:Button ID="btnRefresh" runat="server" Text="Refresh Grid" OnClick="btnRefresh_Click" CssClass="buttonBoxFocus" />&nbsp;
            <asp:Button ID="btnReturn" runat="server" Text="Return" OnClick="btnReturn_Click" CssClass="buttonBoxFocus" />&nbsp;
            <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" CssClass="buttonBoxFocus" />&nbsp;
            <br />
                <br />
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <tr>
                                <td>List of of processes whose current step is waiting for the Workflow Engine to take action.
                <span style="float: right; padding-right: 3%">Total:
                    <asp:Label ID="lblTotal" runat="server" /></span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                    <div>
                                        <asp:ValidationSummary ID="vsWorkflowEngine" runat="server" DisplayMode="List" ValidationGroup="valWorkflowEngine" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                </table>
                <div style="text-align: right; padding-right: 30px">
                    <span>
                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/Excel_24x24.png"
                            OnClick="ImageButton_Click" AlternateText="ExcelML" /></span>

                </div>
                <telerik:radgrid id="gvAwaitingAction"  runat="server" EnableAriaSupport="true" FilterMenu-AriaSettings-Label="WorkFlow" allowpaging="True" allowsorting="true" allowfilteringbycolumn="true" skin="PDMSModern">
                    <mastertableview autogeneratecolumns="False" datakeynames="">
                        <columns>
                            <telerik:gridtemplatecolumn headertext="" uniquename="Process" AllowFiltering="false">
                                <itemtemplate>
                                    <asp:LinkButton ToolTip="Process" ID="lnkEdit" runat="server" CommandName="ProcessRow" CausesValidation="false"
                                        OnCommand="lnkProcess_Click" CommandArgument='<%# Bind("PROCESS_ID") %>' Text="Process" CssClass="gridLink" />
                                </itemtemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn headertext="" AllowFiltering="false">
                                <itemtemplate>
                                    <asp:LinkButton ToolTip="Process" ID="lnkPause" runat="server" CommandName="PauseRow" CausesValidation="false"
                                        OnCommand="lnkPause_Click" CommandArgument='<%# Bind("PROCESS_ID") %>' Text="Pause" CssClass="gridLink" />
                                </itemtemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridboundcolumn datafield="WORKFLOW_ID" headertext="ID" visible="true" />
                            <telerik:gridboundcolumn datafield="WORKFLOW_NAME" headertext="Workflow" visible="true" />
                            <telerik:gridboundcolumn datafield="TASK_ID" headertext="Task ID" visible="true" />
                            <telerik:gridboundcolumn datafield="TASK_NAME" headertext="Task" visible="true" />
                            <telerik:gridboundcolumn datafield="PROCESS_ID" headertext="Process ID" visible="true" />
                            <telerik:gridboundcolumn datafield="STEP_ID" headertext="Step ID" visible="true" />
                            <telerik:gridboundcolumn datafield="REGISTRATION_ID" headertext="REG ID" visible="true" />
                            <telerik:gridboundcolumn datafield="NAME" headertext="Name" visible="true" />
                            <telerik:gridboundcolumn datafield="OtherAgencyAppID" headertext="Other Agency App ID" visible="true" />
                            <telerik:gridboundcolumn datafield="CREATE_DATE_TIME" headertext="Created Date" visible="true" />
                            <telerik:gridboundcolumn datafield="REG_PROGRAM_STATUS_TYPE_ID" headertext="Created Date" visible="false" />
                            <telerik:gridboundcolumn datafield="WORKFLOW_EVENT_TYPE_ID" headertext="Created Date" visible="false" />
                        </columns>
                        <pagerstyle mode="NextPrevAndNumeric" alwaysvisible="true" pagesizelabeltext="Page Size: " pagesizes="50,100,500,1000,10000" />
                    </mastertableview>
                </telerik:radgrid>


                <!-- ModalPopupExtender -->
                <ajax:modalpopupextender id="mpe" runat="server" popupcontrolid="pnlModal" targetcontrolid="ButtonDummy"
                    cancelcontrolid="btnCancel" backgroundcssclass="pauseModalBackground" popupdraghandlecontrolid="pnlModal">
                </ajax:modalpopupextender>
                <asp:Panel ID="pnlModal" runat="server" CssClass="pauseModalPopup" align="center" Style="display: none">
                    <asp:Panel ID="pnlHeaderMpe" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
                        <div align="left">
                            &nbsp;&nbsp;
                        <asp:Label ID="lblTitle" CssClass="bodyTextBold" runat="server" Text="Pause Workflow Process" ForeColor="White" />
                        </div>
                    </asp:Panel>
                    <div>
                        <asp:ValidationSummary ID="vsPauseInfo" runat="server" DisplayMode="List" ValidationGroup="valPauseInfo" />
                    </div>
                    <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px" DefaultButton="btnSave">
                        <table id="ParentTable" runat="server">
                            <colgroup>
                                <col width="50%" />
                                <col width="50%" align="left" />
                            </colgroup>
                            <tr>
                                <td><span class="formLabel">Pause Amount*</span></td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlMinutes" runat="server" CssClass="formDropDown">
                                        <asp:ListItem Text="5 Minutes" Value="5" />
                                        <asp:ListItem Text="10 Minutes" Value="10" />
                                        <asp:ListItem Text="15 Minutes" Value="15" />
                                        <asp:ListItem Text="30 Minutes" Value="30" />
                                        <asp:ListItem Text="1 Hour" Value="60" />
                                        <asp:ListItem Text="3 Hours" Value="180" />
                                        <asp:ListItem Text="6 Hours" Value="360" />
                                        <asp:ListItem Text="12 Hours" Value="720" />
                                        <asp:ListItem Text="1 Day" Value="1440" />
                                        <asp:ListItem Text="3 Days" Value="4320" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="reqMinutes" ValidationGroup="valPauseInfo"
                                        ControlToValidate="ddlMinutes" ErrorMessage="*Select a Pause Amount" Text="*" Display="Dynamic"
                                        SetFocusOnError="true" InitialValue="" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="0" cellpadding="0" cellspacing="5" align="center" style="padding-bottom: 10px">
                        <tr>
                            <td>
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBox" OnClick="btnSave_Click" CausesValidation="true" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox"
                                    CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />
        </div>
    </form>
</body>
</html>
