<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_AddPowerAgent" Codebehind="AddPowerAgent.ascx.cs" %>

<asp:Panel ID="pnl" runat="server" CssClass="modal-panel">
    <style type="text/css">
        .modal-panel {
            background-color: white;
            padding: 20px;
            border-radius: 5px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            width: 500px;
            margin: 0 auto;
        }

        .add-agent-table {
            width: 100%;
            border-collapse: collapse;
            border: 2px solid #6c757d;
            margin-bottom: 20px;
        }

        .add-agent-table th {
            background-color: #6c757d;
            color: white;
            padding: 10px;
            text-align: left;
            border: 1px solid #5a6268;
            font-weight: bold;
            font-size: 14px;
        }

        .add-agent-table td {
            padding: 8px 10px;
            border: 1px solid #6c757d;
            vertical-align: middle;
        }

        .checkbox-cell {
            text-align: center;
            width: 50px;
        }

        .ohid-input {
            width: 100px;
            padding: 4px 6px;
            border: 1px solid #ced4da;
            border-radius: 3px;
            font-size: 14px;
        }

        .ohid-input:focus {
            outline: none;
            border-color: #80bdff;
            box-shadow: 0 0 0 0.2rem rgba(0,123,255,.25);
        }

        .info-cell {
            font-size: 14px;
            color: #333;
            padding-left: 10px;
        }

        .button-row {
            text-align: right;
            margin-top: 15px;
        }

        .btn-add {
            background-color: #007bff;
            color: white;
            padding: 6px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-weight: 500;
            font-size: 14px;
            margin-right: 10px;
        }

        .btn-add:hover {
            background-color: #0056b3;
        }

        .btn-cancel {
            background-color: #6c757d;
            color: white;
            padding: 6px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-weight: 500;
            font-size: 14px;
        }

        .btn-cancel:hover {
            background-color: #5a6268;
        }

        .error-message {
            color: #dc3545;
            font-size: 12px;
            margin-top: 5px;
            display: block;
        }

        .help-text {
            color: #666666;
            font-size: 12px;
            font-style: italic;
            margin-top: 5px;
        }
    </style>

    <table class="add-agent-table">
        <thead>
            <tr>
                <th>Access Management</th>
                <th>OH ID</th>
                <th>OH ID Email</th>
                <th>OH ID User Name</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="checkbox-cell">
                    <asp:CheckBox ID="chkAccessManagement" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="txtOhId" runat="server" CssClass="ohid-input" MaxLength="50" 
                        placeholder="76589543" AutoPostBack="true" OnTextChanged="txtOhId_TextChanged" />
                    <asp:RequiredFieldValidator ID="rfvOhId" runat="server"
                        ControlToValidate="txtOhId" Display="Dynamic"
                        ErrorMessage="OH ID is required."
                        CssClass="error-message" ValidationGroup="vgAddAgent" />
                    <asp:CustomValidator ID="cvOhIdExists" runat="server"
                        ControlToValidate="txtOhId" Display="Dynamic"
                        OnServerValidate="cvOhIdExists_ServerValidate"
                        ErrorMessage="OH ID does not exist"
                        CssClass="error-message" ValidationGroup="vgAddAgent" />
                    <asp:CustomValidator ID="cvOhIdRole" runat="server"
                        ControlToValidate="txtOhId" Display="Dynamic"
                        OnServerValidate="cvOhIdRole_ServerValidate"
                        ErrorMessage="OH ID is not a provider agent"
                        CssClass="error-message" ValidationGroup="vgAddAgent" />
                    <asp:CustomValidator ID="cvOhIdNotDuplicate" runat="server"
                        ControlToValidate="txtOhId" Display="Dynamic"
                        OnServerValidate="cvOhIdNotDuplicate_ServerValidate"
                        ErrorMessage="OH ID is already provisioned as power agent to this administrator"
                        CssClass="error-message" ValidationGroup="vgAddAgent" />
                </td>
                <td class="info-cell">
                    <asp:Label ID="lblOhIdEmail" runat="server" Text="" />
                </td>
                <td class="info-cell">
                    <asp:Label ID="lblOhIdUserName" runat="server" Text="" />
                </td>
            </tr>
        </tbody>
    </table>

    <div class="help-text">
        Access Management gives ability to grant agent access to all agents and ability to add power agents
    </div>

    <div class="button-row">
        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn-add" 
            OnClick="btnAdd_Click" ValidationGroup="vgAddAgent" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-cancel" 
            CausesValidation="false" />
    </div>
</asp:Panel>