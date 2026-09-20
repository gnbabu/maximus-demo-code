<%@ control language="C#" autoeventwireup="true" inherits="Views_PaperDocumentView, App_Web_av5ll3zk" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/UploadDocument.ascx"  TagName="UploadDocument" TagPrefix="uc" %>

    <br /><uc:Separator ID="ucSep1" runat="server" Header="Uploaded Documents" /><br />
    <uc:UploadDocument ID="ucUploadDocument" runat="server" CssClassUploadButton="buttonBox"  DocumentSection=""
        ValidFileExtensions="doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt"  /><br />


