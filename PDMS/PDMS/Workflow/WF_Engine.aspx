<%@ Page Language="C#" AutoEventWireup="true" Inherits="WF_Engine" Codebehind="WF_Engine.aspx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow Engine</title>
    <style type="text/css">
        .pauseModalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.2;
            height: auto;
        }
        .pauseModalPopup
        {
            background-color: #FFFFFF;
            border-width: 1px;
            border-style: solid;
            border-color: black;
            padding: 0px;
            width: auto;
            height: auto;
        } 
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <%--<ajax:ToolkitScriptManager ID="ToolkitScriptManager1"  runat="server" ScriptMode="Release" />--%>
        <div>
            <br />
            <br />
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh Grid" onclick="btnRefresh_Click" CssClass="buttonBox" />&nbsp;
            <asp:Button ID="btnReturn" runat="server" Text="Return" onclick="btnReturn_Click" CssClass="buttonBox" />&nbsp;
            <asp:Button ID="btnLogout" runat="server" Text="Logout" onclick="btnLogout_Click" CssClass="buttonBox" />&nbsp;
            <br />
            <br />
            <table style="width:100%;">
            <tr>
            <td>
            List of of processes whose current step is waiting for the Workflow Engine to take action.
                <span style="float: right; padding-right: 3%">Total: <asp:Label ID="lblTotal" runat="server" /></span>
            </td>
            </tr>
            <tr>
                <td>
                    <div><asp:ValidationSummary ID="vsWorkflowEngine" runat="server" DisplayMode="List" ValidationGroup="valWorkflowEngine" /></div>
                </td>
            </tr>
            <tr><td>&nbsp;</td></tr>
            <tr>
            <td>
                <asp:GridView 
                    runat="server" 
                    Width="95%" 
                    ID="gvAwaitingAction"
                    AutoGenerateColumns="False" 
                    HorizontalAlign="Center" 
                    CssClass="gridview" 
                    EmptyDataText="No data found"                    
                    OnPageIndexChanging="gvAwaitingAction_PageIndexChanging"
                    onrowdatabound="gvAwaitingAction_RowDataBound"
                    AllowCustomPaging="true"
                    AllowPaging="true"
                    PageSize = "50">
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server" CausesValidation="false" 
                                    CommandArgument='<%# Bind("PROCESS_ID") %>' CommandName="ProcessRow" 
                                    OnCommand="lnkProcess_Click" Text="Process" CssClass="gridLink" />
                            </ItemTemplate>            
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPause" runat="server" CausesValidation="false" 
                                    CommandArgument='<%# Bind("PROCESS_ID") %>' CommandName="PauseRow" 
                                    OnCommand="lnkPause_Click" Text="Pause" CssClass="gridLink" />
                            </ItemTemplate>            
                        </asp:TemplateField>
                        <asp:BoundField ReadOnly="true" DataField="WORKFLOW_ID" HeaderText="ID" />
                        <asp:BoundField ReadOnly="true" DataField="WORKFLOW_NAME" HeaderText="Workflow" />
                        <asp:BoundField ReadOnly="true" DataField="TASK_ID" HeaderText="Task ID" />
                        <asp:BoundField ReadOnly="true" DataField="TASK_NAME" HeaderText="Task" />
                        <asp:BoundField ReadOnly="true" DataField="PROCESS_ID" HeaderText="Process ID" />
                        <asp:BoundField ReadOnly="true" DataField="STEP_ID" HeaderText="Step ID" />
                        <asp:BoundField ReadOnly="true" DataField="REGISTRATION_ID" HeaderText="REG ID" />
                        <asp:BoundField ReadOnly="true" DataField="NAME" HeaderText="Name" />
                        <asp:BoundField ReadOnly="true" DataField="CREATE_DATE_TIME" HeaderText="Created Date" />
                        <asp:TemplateField HeaderText="Advance Step" AccessibleHeaderText="Advance Step">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkAdvance" runat="server" />
                                <p style="display:none;">
                                <asp:Label ID="lblchkbox" runat="server" AssociatedControlID="chkAdvance" Text="Check Advance"></asp:Label>
                                    </p>
                            </ItemTemplate>            
                        </asp:TemplateField>
                        <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderText="">
                            <ItemTemplate>
                                <asp:HiddenField ID="hdnProcessID" runat="server" Value='<%# Eval("PROCESS_ID") %>' />
                                <asp:HiddenField ID="hdnStepID" runat="server" Value='<%# Eval("STEP_ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                    OnClick="btnDelete_Click" Text="Delete" />
                                <asp:LinkButton ID="btnCancel" runat="server" OnClientClick="return confirm('Are you sure you want to cancel this process?');"
                                    Text="Cancel" OnClick="btnCancel_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField Visible="false"  DataField="REG_PROGRAM_STATUS_TYPE_ID" HeaderText="Created Date" />
                        <asp:BoundField  Visible="false" DataField="WORKFLOW_EVENT_TYPE_ID" HeaderText="Created Date" />
                    </Columns>
                    <HeaderStyle CssClass="gridViewHeader" Width="100px" /> 
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" /> 
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
            </td>
            </tr>
            </table>
            <div style="padding-top: 20px">&nbsp;</div>
            Process ID (Leave blank to see everything): <asp:TextBox ID="txtProcessID" runat="server" />&nbsp;
                <asp:Button ID="btnGo" runat="server" Text="Go" onclick="btnGo_Click" CssClass="buttonBox" />&nbsp;
                <asp:Button ID="btnClear" runat="server" Text="Clear" onclick="btnClear_Click" CssClass="buttonBox" />&nbsp;
            <br /><br />
            <asp:GridView runat="server" Width="95%" ID="gvProcess" 
                    AutoGenerateColumns="False" 
                    HorizontalAlign="Center" 
                    CssClass="gridview" 
                    EmptyDataText="No data found">
                <Columns>
                    <asp:BoundField ReadOnly="true" DataField="PROCESS_ID" HeaderText="Process ID" />
                    <asp:BoundField ReadOnly="true" DataField="PROCESS_OWNER" HeaderText="Process Owner" />
                    <asp:BoundField ReadOnly="true" DataField="CURRENT_STEP_ID" HeaderText="Current Step ID" />
                    <asp:BoundField ReadOnly="true" DataField="STEP_OWNER" HeaderText="Step Owner" />
                    <asp:BoundField ReadOnly="true" DataField="WORKFLOW_ID" HeaderText="Workflow ID" />
                    <asp:BoundField ReadOnly="true" DataField="WORKFLOW_NAME" HeaderText="Workflow" />
                    <asp:BoundField ReadOnly="true" DataField="TASK_ID" HeaderText="Task ID" />
                    <asp:BoundField ReadOnly="true" DataField="TASK_NAME" HeaderText="Task" />
                    <asp:BoundField ReadOnly="true" DataField="CREATE_DATE_TIME" HeaderText="Created Date" />
                </Columns>
                <PagerStyle cssClass="gridpager" HorizontalAlign="Right" />  
                <HeaderStyle CssClass="gridViewHeader" Width="100px" /> 
                <AlternatingRowStyle CssClass="gridViewAltRow" />
                <RowStyle CssClass="gridViewRow" /> 
                <FooterStyle CssClass="gridViewFooter" />
            </asp:GridView>
            <!-- ModalPopupExtender -->
            <ajax:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
                CancelControlID="btnCancel" BackgroundCssClass="pauseModalBackground" PopupDragHandleControlID="pnlModal">
            </ajax:ModalPopupExtender>
            <asp:Panel ID="pnlModal" runat="server" CssClass="pauseModalPopup" align="center" style="display:none">
                <asp:Panel ID="pnlHeaderMpe" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
                    <div align="left">&nbsp;&nbsp;
                        <asp:Label ID="lblTitle" CssClass="bodyTextBold" runat="server" Text="Pause Workflow Process" ForeColor="White" />
                    </div>
                </asp:Panel>  
                <div><asp:ValidationSummary ID="vsPauseInfo" runat="server" DisplayMode="List" ValidationGroup="valPauseInfo" /></div>
                <asp:Panel ID="pnlMain" runat="server" Style="margin-right:10px" DefaultButton="btnSave">
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
                            <asp:Button id="btnSave"  runat="server" Text="Save" CssClass="buttonBox" onclick="btnSave_Click" CausesValidation="true" />
                        </td>
                        <td>
                            <asp:Button id="btnCancel"  runat="server" Text="Cancel" CssClass="buttonBox" 
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
