<%@ Page Language="C#" AutoEventWireup="true" Inherits="Pages_ShowFiles" Codebehind="ShowFiles.aspx.cs" %>
<%--This is intentionaly blank--%>
<html lang="en">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        html, body {
            margin: 0;
            padding: 0;
            overflow:hidden;
        }
    </style>
</head>
<body>
    <iframe src="<%=viewUrl.Uri.ToString()%>" style="width:100%;height:100%"></iframe>
</body>
</html>
