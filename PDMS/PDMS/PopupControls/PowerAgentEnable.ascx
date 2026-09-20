<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_PowerAgentEnable" Codebehind="PowerAgentEnable.ascx.cs" %>

<style type="text/css">
    .power-agent-enable-section {
        margin: 20px 0;
    }
    
    .form-row {
        margin-bottom: 15px;
        display: flex;
        align-items: center;
        gap: 8rem;
    }
    
    .lbl-bold {
        font-weight: bold;
        font-size: 16px;
    }
    
    .radio-group {
        display: inline-flex;
        margin-left: 2rem;
        text-align: center;
        align-content: center;
        align-items: start;
    }
    
    .radio-group label {
        font-weight: normal;
        margin-left: 5px;
        cursor: pointer;
        font-size: 14px;
    }
    
    .radio-group input[type="radio"] {
        cursor: pointer;
        margin-right: 3px;
    }
    
    .radio-group label:first-of-type {
        margin-right: 40px !important;
    }
    
    .help-text {
        color: #666666;
        font-size: 13px;
        font-style: italic;
        display: block;
        margin-top: 3px;
    }
</style>

<asp:UpdatePanel ID="upPowerAgentEnable" runat="server">
    <ContentTemplate>
        <div class="power-agent-enable-section">
            <div class="form-row">
                <asp:Label ID="lblEnable" CssClass="lbl-bold" runat="server" 
                    Text="I would like to enable Power Agents:" />
                <asp:RadioButtonList ID="rblEnablePowerAgents" runat="server"
                    CssClass="radio-group" RepeatLayout="Flow" RepeatDirection="Horizontal"
                    AutoPostBack="true" OnSelectedIndexChanged="rblEnablePowerAgents_SelectedIndexChanged">
                    <asp:ListItem Text="No (default - 1 admin only)" Value="No" Selected="True" />
                    <asp:ListItem Text="Yes (will require at least 1 Power Agent)" Value="Yes" />
                </asp:RadioButtonList>
            </div>
            <%--<span class="help-text">
                Power Agent default functionality will include the same activities that an administrator 
                can perform, except Access Management. Access Management can be granted when provisioning 
                a new power agent or editing an existing power agent.
            </span>--%>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>