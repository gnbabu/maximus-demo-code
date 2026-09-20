<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_NonScreeningResult" Codebehind="NonScreeningResult.ascx.cs" %>
<asp:DropDownList runat="server" ID="ddlMatchResults" CssClass="formDropDown" ></asp:DropDownList><br />
<br />
<asp:Button runat="server" ID="btnUpdate" CssClass="buttonBox" Text="Update" OnClick="btnUpdate_Click" />

<asp:Button runat="server" ID="btnCancelNonScreeningResult" CssClass="buttonBox" Text="Cancel" OnClick="btnCancelNonScreeningResult_Click" />