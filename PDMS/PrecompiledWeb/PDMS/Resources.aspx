<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_Resources, App_Web_jwgsiblf" enableEventValidation="false" stylesheettheme="Default" %>
<asp:Content ContentPlaceHolderID="HtmlHead" runat="server">
    <style>
        li.heading{list-style-type:none;}
        .docList li {list-style-type:disc}
        main div h1{font-size:36px;}
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <h1>Provider Education & Training Resources</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="WhiteBox">
       

    </div>
    <br />
        <div id="divRender" runat="server">
         <asp:PlaceHolder ID="phAutoGenerate" runat="server"></asp:PlaceHolder>
    </div>
    <div class="resource-link-box"><a href="https://get.adobe.com/reader/" target="_blank">Download Acrobat reader here(opens new window)</a></div>
</asp:Content>
