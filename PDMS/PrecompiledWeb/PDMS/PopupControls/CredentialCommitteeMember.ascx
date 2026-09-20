<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_CredentialCommitteeMember, App_Web_rqhgepvh" %>
<div id="divCredentialMember" class="container-fluid">
    <asp:Panel runat="server" ID="pnlmember" Visible="true">
    <div class="row"> 
        <div class="col-sm-3 text-right"><span class="formLabel">Committee Member Name:</span></div>
        <div class="col-sm-3 text-left"><asp:Label runat="server" ID="lblMemberName" Text=""/></div>
        <div class="col-sm-3 text-right"><span class="formLabel">Committee Role:</span></div>
        <div class="col-sm-3 text-left"><asp:Label runat="server" ID="lblCommiteeRole" Text=""/></div>
    </div>
    <div class="row">
        <div class="col-sm-3 text-right"><span class="formLabel"><asp:Label ID="lblTaxIDLabel" runat="server" Text="Committee Member Action:"></asp:Label></span></div>
        <div class="col-sm-3 text-left"><asp:DropDownList runat="server" ID="ddlResult" CssClass="formDropDownSmall" OnSelectedIndexChanged="ddlResult_SelectedIndexChanged"  EnableViewState="true"/></div>
                            
        <div class="col-sm-3 text-right"></div>
        <div class="col-sm-3 text-left"></div>
    </div>       
     <div class="row">
        <div class="col-sm-3 text-right"><asp:Label runat="server" ID="lblactioncomments" CssClass="formLabel">Comments:</asp:Label></div>
        <div class="col-sm-9 text-left"><asp:TextBox ID="txtactioncomments" runat="server" MaxLength="1000" CssClass="formFieldMultiline" TextMode="MultiLine"  Rows="7" /></div>
    </div>
<div class="row btnBoxCenter">
    <br />
<asp:Button runat="server" ID="btnConfirm" CssClass="buttonBoxFocus" Text="Save"  EnableViewState="true" CausesValidation="true"  OnClick="btnConfirm_Click"/>
<asp:Button runat="server" ID="btnCancelScreeningResult" CssClass="buttonBox" Text="Cancel"  CausesValidation="false" />
</div>
    <asp:HiddenField ID="hdnCredentialingId" runat="server" value=""/>
        <asp:HiddenField ID="hdnCredentialAction" runat="server" value=""/>
        </asp:Panel>
    </div>
