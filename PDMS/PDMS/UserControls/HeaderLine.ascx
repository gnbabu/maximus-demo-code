<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_HeaderLine" Codebehind="HeaderLine.ascx.cs" %>

<style type="text/css"> 
    .headerLineText 
    { 
        float:left; 
        border: none; 
        background-color:White; 
        font-weight: bold;
        padding-right: 5px;
        padding-left: 5px; 
	    margin-left: 10px;
        height:20px; 
    } 
    .headerLineContainer 
    { 
        border-bottom:solid 1px #e3e3e3; 
        height:10px;
        margin-bottom: 5px; 
    } 
</style> 

<div class="headerLineContainer"><div class="headerLineText"><asp:Label ID="lblHeader" runat="server" /></div></div> 