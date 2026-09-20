<%@ Page Language="C#" AutoEventWireup="true" Inherits="Process_MITSRedirect" Codebehind="MITSRedirect.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Redirecting to MITS</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin:auto;border:solid 2px black;">
            You are being redirected to MITS, if you are not automatically redirected, <a href="<%= this.RecipientEligibilityURL %>?AuthToken=<%= this.RecipientEligibilityToken %>&ProviderId=<%= this.RecipientEligibilityProviderId %>">Recipient Eligibility</a>
        </div>
    </form>
</body>
</html>
