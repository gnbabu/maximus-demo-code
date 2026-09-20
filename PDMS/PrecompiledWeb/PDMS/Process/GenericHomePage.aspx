<%@ page language="C#" autoeventwireup="true" masterpagefile="~/MasterPage.master" inherits="Process_GenericHomePage, App_Web_rnw0hezi" enableEventValidation="false" stylesheettheme="Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="PageLabelContent">
 
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
     <div class="WhiteBox">
         <br /> <br />
         <div class="row">
             <asp:Image runat="server" ID="sectionIcon" ImageUrl="~/Images/OHmedicaidlogo.jpg" CssClass="img-responsive" Style="margin-left: 30%;"></asp:Image>             
         </div><br /><br />
         <div class="row">
             <h1><span style="font-weight:bold;"> Welcome to Provider Network Management (PNM) System. Please use the Menu to navigate.</span> </h1>            
         </div>
     </div>
</asp:Content>
