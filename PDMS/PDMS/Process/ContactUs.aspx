<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Process_ContactUs" Codebehind="ContactUs.aspx.cs" %>
<asp:Content ContentPlaceHolderID="htmlHead" runat="server" >
    <style>
        main ul {
            list-style-type:none;
            padding:0;
            margin:0 auto;
            text-align:left;
            display:inline-block;
        }
        main li{
            margin: 1.5rem;
        }
        main .infoPanel {
            text-align:center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    <h1>Contact Us</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Panel ID="pnlContactUs" runat="server" CssClass="infoPanel">
        <ul><li>            
              <strong>  State Provider Assistance and Enrollment <br />
               1-800-686-1516</strong>            
            </li>           
            <li><strong>State Waiver Providers <br />  
                1-877-908-1746</strong>
            </li>            
            <li><strong>State Support Center <br />
                    1-800-617-6733</strong>
            </li>            
            <li><strong>State Provider Certification<br />
                    1-614-779-0248</strong>
            </li>            
            <li><strong>General<br />
                    1-877-275-6364</strong>
            </li>
        </ul> 
    </asp:Panel>           
</asp:Content>

