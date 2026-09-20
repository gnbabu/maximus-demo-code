<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_TransactionQueueProvider" Codebehind="TransactionQueueProvider.ascx.cs" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<style type="text/css">
    select {
        min-width: 90%;
    }
</style>

<telerik:RadGrid ID="RadGridQ" runat="server" AllowFilteringByColumn="True" AutoGenerateColumns="False" EnableAriaSupport="true">
    <mastertableview Font_Name="Courier" role="presentation">
        <columns>
                <telerik:GridBoundColumn UniqueName="Col_0" HeaderText="Date" DataField="PROCESS_DATE_TIME" DataFormatString="{0:MM/dd/yy}" DataType="System.DateTime" HeaderStyle-Width="100px"/>
                <telerik:GridBoundColumn HeaderText="Reg ID" DataField="REG_ID" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn HeaderText="Medicaid ID" DataField="MEDICAID_ID" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn HeaderText="Provider Name" DataField="NAME" HeaderStyle-Width="270px" FilterControlWidth="300px"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn HeaderText="NPI" DataField="NPI"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn HeaderText="Update Type" DataField="TRANSACTION_TYPE"></telerik:GridBoundColumn>
                <telerik:GridBoundColumn HeaderText="DD Contract Number" DataField="DD_CONTRACT_NUMBER"></telerik:GridBoundColumn>
        </columns>
    </mastertableview>
</telerik:RadGrid>
