<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" Inherits="Process_PowerAgentAccessMgmt" Codebehind="PowerAgentAccessMgmt.aspx.cs" %>

<%@ Register Src="~/PopupControls/GlobalAdminChange.ascx" TagName="GlobalAdminChange" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/PowerAgentEnable.ascx" TagName="PowerAgentEnable" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/PowerAgentGrid.ascx" TagName="PowerAgentGrid" TagPrefix="uc" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HtmlHead" runat="server">
    <style type="text/css">
        .power-agent-container {
            max-width: 100%;
            margin: 0;
            padding: 0;
            min-height: 500px;
        }
        
        .content-panel {
            padding: 25px 40px;
            border: none;
            margin: 0;
        }
        
        hr {
            border: none;
            border-top: 1px solid #dee2e6;
            margin: 25px 0;
        }
    </style>
</asp:Content>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="PageLabelContent">
    <h2>Power Agent Access Management</h2>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="power-agent-container">
        <div class="content-panel">
            
            <!-- Global Administrator Change Control -->
            <uc:GlobalAdminChange ID="ucGlobalAdminChange" runat="server" 
                OnAdminChanged="ucGlobalAdminChange_AdminChanged" />
            
            <hr />
            
            <!-- Power Agent Enable Control -->
            <uc:PowerAgentEnable ID="ucPowerAgentEnable" runat="server" 
                OnPowerAgentEnableChanged="ucPowerAgentEnable_PowerAgentEnableChanged" />
            
            <!-- Power Agent Grid Control -->
            <uc:PowerAgentGrid ID="ucPowerAgentGrid" runat="server" 
                OnPowerAgentAdded="ucPowerAgentGrid_PowerAgentAdded"
                OnPowerAgentUpdated="ucPowerAgentGrid_PowerAgentUpdated"
                OnPowerAgentDeactivated="ucPowerAgentGrid_PowerAgentDeactivated" />
        </div>
    </div>
</asp:Content>