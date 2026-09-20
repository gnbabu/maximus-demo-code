<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_CredentialReviewSearch" Codebehind="CredentialReviewSearch.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<div style="padding: 5px;">
    <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
    
     <div style="text-align: center;">
        <asp:Panel ID="pnlFilter" runat="server" >
            <div style="width:100%;" class="table">
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">Provider Name</span>&nbsp;&nbsp;
                    </div>  
                    <div class="col-sm-4 text-left">
                        <asp:TextBox ID="txtGroupName" runat="server" CssClass="formField" MaxLength="30" />
                    </div>
                    <div class="col-sm-2">
                        <span class="formLabel150">Recommended Risk Level</span>&nbsp;&nbsp;
                    </div>  
                    <div class="col-sm-4 text-left">
                       <asp:DropDownList ID="ddlDataRank" EnableViewState="true" runat="server" CssClass="formDropDown">   
                           <asp:ListItem Text="" Value ="0" />
                           <asp:ListItem Text="Low" Value ="1" />
                           <asp:ListItem Text="Medium" Value ="2" />
                           <asp:ListItem Text="High" Value ="3" />
                       </asp:DropDownList>
                    </div>
                    </div>
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">Workflow</span>&nbsp;&nbsp;
                    </div>  
                    <div class="col-sm-4 text-left">
                        <asp:DropDownList ID="ddlworkflow" EnableViewState="true" runat="server" CssClass="formDropDown"> 
                            <asp:ListItem Text="" Value="0" />
                            <asp:ListItem Text="Registration - New" Value="1" />
                            <asp:ListItem Text="Registration - Recredentialing" Value ="18" />
                        </asp:DropDownList> 
                           
                    </div>
                    <div class="col-sm-2 text-right">
                        <span class="formLabel150">Provider Type</span>&nbsp;&nbsp;
                    </div>  
                    <div class="col-sm-4 text-left">
                       <asp:DropDownList ID="ddlProviderType" EnableViewState="true" runat="server" CssClass="formDropDown" />                        
                    </div>
                    </div>
                
                </div>

       </asp:Panel>



 </div>
</div>
