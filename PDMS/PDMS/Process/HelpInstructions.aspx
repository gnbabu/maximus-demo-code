<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Process_HelpInstructions" Codebehind="HelpInstructions.aspx.cs" %>
<asp:Content ContentPlaceHolderID="HtmlHead" runat="server">
    <style>
        li.heading{list-style-type:none;}
        .docList li {list-style-type:disc}
        main div h1{font-size:36px;}
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="WhiteBox">
       

    </div>
    <br />
        <div id="divRender" runat="server">
         <asp:PlaceHolder ID="sectionPH" runat="server"></asp:PlaceHolder>
    </div>
    
</asp:Content>
