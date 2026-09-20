<%@ control language="C#" autoeventwireup="true" inherits="Controls_PowerAgentGrid" Codebehind="PowerAgentGrid.ascx.cs" %>
<%@ register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajax" %>

<style type="text/css">
    .power-agent-panel {
        margin-top: 25px;
        padding: 20px;
        background-color: #f8f9fa;
        border-radius: 4px;
    }

        .power-agent-panel h4 {
            color: #333;
            margin-bottom: 15px;
            font-size: 16px;
        }

    .grid-header {
        margin: 20px 0 15px 0;
        display: flex;
        justify-content: space-between;
        align-items: center;
    }

    .grid-header-left {
        display: flex;
        align-items: center;
        gap: 15px;
    }

    .grid-header-right {
        display: flex;
        align-items: center;
        gap: 10px;
    }

    .search-box {
        padding: 6px 10px;
        border: 1px solid #ced4da;
        border-radius: 4px;
        font-size: 14px;
        width: 200px;
    }

    .btn-add-new {
        background-color: #007bff;
        color: white;
        padding: 8px 16px;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        font-weight: 500;
        font-size: 14px;
        height: 36px;
    }

    .btn-search {
        background-color: #28a745;
        color: white;
        padding: 6px 16px;
        border: none;
        border-radius: 4px;
        cursor: pointer;
        font-weight: 500;
        font-size: 14px;
        height: 32px;
    }

    .power-agents-grid {
        width: 100%;
        border-collapse: separate;
        border-spacing: 0;
        border: 1px solid #dee2e6;
        border-radius: 4px;
        overflow: hidden;
        background-color: #ffffff;
    }

        .power-agents-grid th {
            background-color: #6c757d;
            color: white;
            padding: 12px 15px;
            text-align: left;
            border-right: 1px solid #5a6268;
            font-weight: 600;
            font-size: 14px;
        }

        .power-agents-grid td {
            padding: 12px 15px;
            border-bottom: 1px solid #dee2e6;
            border-right: 1px solid #dee2e6;
            font-size: 14px;
            vertical-align: middle;
        }

    .btn-grid-action {
        padding: 4px 12px;
        margin-right: 5px;
        border: none;
        border-radius: 3px;
        cursor: pointer;
        font-size: 12px;
        font-weight: 500;
        height: 28px;
    }

    .btn-edit {
        background-color: #28a745;
        color: white;
    }

    .btn-deactivate {
        background-color: #dc3545;
        color: white;
    }

    .modal-bg {
        background-color: rgba(0, 0, 0, 0.5);
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        z-index: 9999;
    }

    .modal-panel {
        background: white;
        padding: 20px;
        border-radius: 8px;
        width: 600px;
        max-height: 500px;
        overflow-y: auto;
    }


    .btn-save {
        background-color: #28a745;
        color: white;
    }

    .btn-cancel {
        background-color: #6c757d;
        color: white;
    }

    .editing-row {
        background-color: #fff3cd !important;
    }

    .modal-panel {
        background: white;
        padding: 20px;
        border-radius: 8px;
        width: 750px; /* Wider to accommodate horizontal layout */
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    }

        .modal-panel h3 {
            margin: 0 0 15px 0;
            color: #333;
            font-size: 18px;
        }

    .add-new-container {
        margin: 20px 0;
    }

    .add-new-table {
        width: 100%;
        border-collapse: separate;
        border-spacing: 0;
        border: 1px solid #dee2e6;
        border-radius: 4px;
        overflow: hidden;
    }

        .add-new-table thead th {
            background-color: #6c757d;
            color: white;
            padding: 10px;
            text-align: left;
            font-weight: 600;
            font-size: 14px;
            border-right: 1px solid #5a6268;
        }

            .add-new-table thead th:last-child {
                border-right: none;
            }

        .add-new-table tbody td {
            padding: 10px;
            background-color: #fff;
            border-right: 1px solid #dee2e6;
            vertical-align: middle;
        }

            .add-new-table tbody td:last-child {
                border-right: none;
            }

    .modal-input {
        width: 95%;
        padding: 6px 8px;
        border: 1px solid #ced4da;
        border-radius: 4px;
        font-size: 14px;
    }

        .modal-input:focus {
            outline: none;
            border-color: #80bdff;
            box-shadow: 0 0 0 0.2rem rgba(0,123,255,.25);
        }

    .modal-validator {
        color: #dc3545;
        font-size: 11px;
        display: block;
        margin-top: 2px;
    }

    .modal-buttons {
        text-align: center;
        margin-top: 20px;
        padding-top: 15px;
        border-top: 1px solid #dee2e6;
    }

    .modal-btn {
        padding: 8px 24px;
        margin: 0 5px;
        border: none;
        border-radius: 4px;
        font-size: 14px;
        font-weight: 500;
        cursor: pointer;
    }

    .btn-primary.modal-btn {
        background-color: #007bff;
        color: white;
    }

        .btn-primary.modal-btn:hover {
            background-color: #0056b3;
        }

    .btn-secondary.modal-btn {
        background-color: #6c757d;
        color: white;
    }

        .btn-secondary.modal-btn:hover {
            background-color: #5a6268;
        }

    .validation-messages {
        margin: 10px 0;
        min-height: 20px;
    }

    .error-message {
        color: #dc3545;
        font-size: 13px;
    }


    .message-container {
        margin: 10px 0;
        min-height: 20px;
    }

    .success-message {
        color: #28a745;
        font-size: 13px;
    }
    .ddl-Admin{
        min-width:200px!important;
    }
</style>

<asp:UpdatePanel ID="upPowerAgentGrid" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlPowerAgents" runat="server" CssClass="power-agent-panel">
            <h4>Power Agent Provisioning History</h4>
            <span class="help-text">Power Agent default functionality will include the same activities that an administrator 
                can perform, except Access Management. Access Management gives the ability to grant agent 
                access to all agents and the ability to add power agents.
            </span>

            <!-- Grid Header with Add New and Search -->
            <div class="grid-header">
                <div class="grid-header-left">
                    <asp:Button ID="btnAddNew" runat="server" Text="Add New"
                        CssClass="btn-add-new" OnClick="btnAddNew_Click" />
                </div>
                <div class="grid-header-right">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box"
                        placeholder="Search by OH ID, Email, or Name..." />
                    <asp:Button ID="btnSearch" runat="server" Text="Search"
                        CssClass="btn-search" OnClick="btnSearch_Click" />
                </div>
            </div>
            <div class="message-container">
                <asp:Label ID="lblError" runat="server" CssClass="error-message" Visible="false" />
                <asp:Label ID="lblSuccess" runat="server" CssClass="success-message" Visible="false" />
            </div>
            <!-- Power Agents Grid with Inline Editing -->
            <div>
                <asp:GridView ID="gvPowerAgents" runat="server"
                    AutoGenerateColumns="false"
                    CssClass="power-agents-grid"
                    DataKeyNames="Id"
                    OnRowCommand="gvPowerAgents_RowCommand"
                    OnRowDataBound="gvPowerAgents_RowDataBound"
                    GridLines="None"
                    BorderWidth="0"
                    EmptyDataText="No power agents found." AllowPaging="True" OnPageIndexChanging="gvPowerAgents_PageIndexChanging" PageSize="10" OnDataBound="gvPowerAgents_DataBound">
                    <Columns>
                        <asp:BoundField DataField="ProviderAdminOhId" HeaderText="Provider Administrator OHID" HeaderStyle-Width="200px" />
                        <asp:BoundField DataField="PowerAgentOhId" HeaderText="Power Agent OHID" HeaderStyle-Width="150px" />
                        <asp:BoundField DataField="PowerAgentEmail" HeaderText="Power Agent Email" HeaderStyle-Width="200px" />
                        <asp:BoundField DataField="PowerAgentUserName" HeaderText="Power Agent User Name" HeaderStyle-Width="200px" />
                        <asp:TemplateField HeaderText="Access Management" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <div style="text-align: center;">
                                    <asp:CheckBox ID="chkAccessManagement" runat="server"
                                        Checked='<%# Eval("HasAccessManagement") %>'
                                        Enabled="false" />
                                    <asp:HiddenField ID="hfOriginalValue" runat="server"
                                        Value='<%# Eval("HasAccessManagement") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <!-- Default state buttons -->
                                <asp:Panel ID="pnlViewMode" runat="server">
                                    <asp:Button ID="btnEdit" runat="server" Text="Edit"
                                        CssClass="btn-grid-action btn-edit"
                                        CommandName="EditAgent"
                                        CommandArgument='<%# Container.DataItemIndex %>' />
                                    <asp:Button ID="btnDeactivate" runat="server" Text="Deactivate"
                                        CssClass="btn-grid-action btn-deactivate"
                                        CommandName="DeactivateAgent"
                                        CommandArgument='<%# Eval("Id") %>'
                                        OnClientClick="return confirm('Are you sure you want to deactivate this Power agent?');" />
                                </asp:Panel>

                                <!-- Edit state buttons -->
                                <asp:Panel ID="pnlEditMode" runat="server" Visible="false">
                                    <asp:Button ID="btnSave" runat="server" Text="Save"
                                        CssClass="btn-grid-action btn-save"
                                        CommandName="SaveAgent"
                                        CommandArgument='<%# Container.DataItemIndex %>' />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel"
                                        CssClass="btn-grid-action btn-cancel"
                                        CommandName="CancelEdit"
                                        CommandArgument='<%# Container.DataItemIndex %>' />
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>

        <!-- Keep only the Add New modal, remove Edit modal -->
        <asp:Button ID="btnHiddenModalTarget" runat="server" Style="display: none" />

        <!-- Add New Power Agent Modal -->
        <ajax:modalpopupextender id="mpeAddNew" runat="server"
            targetcontrolid="btnHiddenModalTarget"
            popupcontrolid="pnlAddNewModal"
            backgroundcssclass="modal-bg" />

        <asp:Panel ID="pnlAddNewModal" runat="server" CssClass="modal-panel" Style="display: none;">
            <h3>Add New Power Agent</h3>
            <hr />

            <!-- Single row layout matching grid headers -->
          <div  id="divAddAgentTable" runat="server">
            <div class="add-new-container">
                <table class="add-new-table">
                    <!-- Header row -->
                    <thead>
                        <tr>
                            <th style="width: 200px;" runat="server" id="thPAdmin">Provider Administrator OHID</th>
                            <th style="width: 150px;">Access Management</th>
                            <th style="width: 150px;">Power Agent OHID</th>
                            <th style="width: 200px;">Power Agent Email</th>
                            <th style="width: 200px;">Power Agent User Name</th>
                        </tr>
                    </thead>
                    <!-- Data entry row -->
                    <tbody>
                        <tr>
                            <td runat="server" id="tdPAdmin">
                                <asp:DropDownList ID="ddlProviderAdmins" runat="server"  CssClass="ddl-Admin" />
                                <asp:RequiredFieldValidator
                                    ID="rfvProviderAdmin"
                                    runat="server"
                                    ControlToValidate="ddlProviderAdmins"
                                    InitialValue="0"
                                    ErrorMessage="Please select a Provider Admin."
                                    ValidationGroup="vgAddNew"
                                    CssClass="modal-validator"
                                    Display="Dynamic" />
                            </td>
                            <td style="text-align: center;">
                                <asp:CheckBox ID="chkNewAccessManagement" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtNewOhId" runat="server" CssClass="modal-input"
                                    placeholder="Enter OH ID" MaxLength="50" />
                                <asp:RequiredFieldValidator ID="rfvNewOhId" runat="server"
                                    ControlToValidate="txtNewOhId" Display="Dynamic"
                                    ErrorMessage="Required" ValidationGroup="vgAddNew"
                                    CssClass="modal-validator" />
                            </td>
                            <td>
                                <asp:Label ID="lblNewEmail" runat="server" Text="" />
                            </td>
                            <td>
                                <asp:Label ID="lblNewUserName" runat="server" Text="" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <!-- Validation messages area -->
            <div class="validation-messages">
                <asp:Label ID="lblValidationMessage" runat="server" CssClass="error-message" Visible="false" />
            </div>
            <!-- Buttons centered below -->
            <div class="modal-buttons">
                <asp:Button ID="btnSaveNew" runat="server" Text="Add"
                    CssClass="btn-primary modal-btn" OnClick="btnSaveNew_Click"
                    ValidationGroup="vgAddNew" />
                <asp:Button ID="btnCancelNew" runat="server" Text="Cancel"
                    CssClass="btn-secondary modal-btn" OnClick="btnCancelNew_Click" />
            </div>
          </div>
          <div id="divConfirmMsg" runat="server"  Visible="false">
              <div class="row" style="text-align: center">
                  <span>The agent selected will have previous agent access removed once provisioned as a power agent. The power agent functionality supersedes the previous agent assignments. Please confirm if you would like to proceed. </span>
              </div>
               <br />
             <div class="row">
                  <div class="modal-buttons">
                      <asp:Button ID="btnConfirm" runat="server" Text="Add"
                        CssClass="btn-primary modal-btn" OnClick="btnConfirm_Click"
                        ValidationGroup="vgAddNew" />
                    <asp:Button ID="btnCancelConfirm" runat="server" Text="Cancel"
                        CssClass="btn-secondary modal-btn" OnClick="btnCancelNew_Click" />
                 </div>
             </div>
          </div>
        </asp:Panel>

        <!-- Hidden field to track which row is being edited -->
        <asp:HiddenField ID="hfEditIndex" runat="server" Value="-1" />
    </ContentTemplate>
</asp:UpdatePanel>
