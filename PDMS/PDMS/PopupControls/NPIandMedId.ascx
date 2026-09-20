<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_NPIandMedId" Codebehind="NPIandMedId.ascx.cs" %>
<%@ register src="NPIandMedIdControl.ascx" tagname="EnrollmentData" tagprefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:ValidationSummary ID="vsNPIandMedId" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="NPIandMedId" />
<asp:UpdatePanel ID="upNpiAndMedId" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="ParentTable" runat="server">
            <uc:enrollmentData id="ucEnrollmentData" runat="server" />
		</div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />
