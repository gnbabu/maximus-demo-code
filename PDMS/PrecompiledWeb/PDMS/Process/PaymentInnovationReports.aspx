<%@ page language="C#" autoeventwireup="true" inherits="Process_PaymentInnovationReports, App_Web_unbhbgmw" enableEventValidation="false" stylesheettheme="Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Redirecting to Payment Innovation Reports</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin:auto;border:solid 2px black;">
            You are being redirected to Payment Innovations, if you are not automatically redirected, <a href="<%= this.PaymentInnovationsURL %>?AuthToken=<%= this.PaymentInnovationsToken %>&ProviderId=<%= this.PaymentInnovationsProviderId %>">Payment Innovation Reports</a>
        </div>
    </form>
</body>
</html>
