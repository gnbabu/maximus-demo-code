<%@ Page Language="C#" AutoEventWireup="true" Inherits="Workflow_WF_TestMMISInterface" Codebehind="WF_TestMMISInterface.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Number of Service Locations to send to MMIS: <asp:TextBox ID="txtRecordCount" runat="server"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnProcess" runat="server" Text="Process" onclick="btnProcess_Click" />    
    </div>
    </form>
</body>
</html>
