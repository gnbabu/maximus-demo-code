<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ProviderRiskLevel, App_Web_l5y5araq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<style type="text/css">
    .formField, .formDropDown {
        width:300px !important;
    }
   select {
    min-width: 300px !important;
    }
    .col-sm-4 {
        width:45% !important;
    }
    .col-sm-8 {
        width:50% !important;
    }
</style> 
<asp:UpdatePanel ID="upSSN" runat="server" UpdateMode="Conditional">
                
                <ContentTemplate>
<div><asp:ValidationSummary ID="vsProviderRiskLevel" runat="server" DisplayMode="List" ValidationGroup="valProviderRiskLevel" /></div>
<div id="ParentTable" runat="server">
     <div class="row">
        <div class="col-sm-4 text-right"><asp:Label ID="lblProviderRiskLevel" runat="server" Text="Provider Risk Level" CssClass="formLabel150" /></div>
        <div class="col-sm-8"><asp:Label ID="lblCurrentProviderRiskLevel" runat="server" Text="" CssClass="formData wd300" />

        </div>
     </div>
    <div class="row">
        <div class="col-sm-4"><asp:Label ID="lblChangeProviderRiskLevel" runat="server" Text="Change Provider Risk Level To" CssClass="formLabel150" /></div>
        <div class="col-sm-8">
                       <asp:DropDownList ID="ddlRiskLevel" runat="server" AutoPostBack="true" AppendDataBoundItems="True" CssClass="formDropDown">
                       </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="rfvProviderRiskLevel" ControlToValidate="ddlRiskLevel" ErrorMessage="* Risk Level is required." Text="*" Display="Dynamic" InitialValue ="0" 
                        SetFocusOnError="true" ValidationGroup="valProviderRiskLevel" />
            

        </div>
        
    </div>   
<div class="row">
<div class="col-sm-4 text-right"><asp:Label ID="lblBumpUp" runat="server" Text="Bump-Up Reason" CssClass="formLabel150" /></div>
<div class="col-sm-8"><asp:DropDownList ID="ddlBumpUpReason" runat="server" AutoPostBack="true"  CssClass="formDropDown"  OnSelectedIndexChanged="ddlBumpUpReason_SelectedIndexChanged">
                                                           </asp:DropDownList>
    <asp:CustomValidator ID="ddl_validate" runat="server" 
        ErrorMessage="" ForeColor="Red"
        ControlToValidate="ddlBumpUpReason" Display="Dynamic" OnServerValidate="ddl_validate_ServerValidate" Text="*" ValidationGroup="valProviderRiskLevel" ValidateEmptyText="true"
        ></asp:CustomValidator>
</div>
</div> 
     <div class="row">
        <div class="col-sm-4 text-right"><span class="formLabel150 verticalAlignTop">Comments</span></div>
        <div class="col-sm-8">
            <asp:TextBox ID="txtComments" runat="server" MaxLength="1000" CssClass="formField" TextMode="MultiLine" Columns="2000" Rows="7" />
                                <asp:RequiredFieldValidator runat="server" ID="rfvComments" ControlToValidate="txtComments" ErrorMessage="* Comments is required." Text="*" Display="Dynamic" 
                        SetFocusOnError="true" ValidationGroup="valProviderRiskLevel" />

        </div>
         
    </div>
    <tr>
           
        
        </tr>
    </div>
                     </ContentTemplate>
</asp:UpdatePanel>
