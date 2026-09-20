<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_AddProviderFeed, App_Web_av5ll3zk" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<div class="container">
    <uc1:Separator runat="server" ID="SeparatorProviderFeed" Header="Add Note" />
    <hr />
    <div class="row">
        <label for="txtDate" class="col-md-4 text-right">Date</label>
        <div class="col-md-8">
            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"/>
        </div>        
    </div>

    <div class="row">
        <label for="txtOHIDInitiator" class="col-md-4 text-right">OHID for Initiator</label>
        <div class="col-md-8">
            <asp:TextBox ID="txtOHIDInitiator" runat="server" CssClass="form-control"/>
        </div>        
    </div>

    <div class="row">
        <label for="txtOHIDReview" class="col-md-4 text-right">OHID for Last ODM Review</label>
        <div class="col-md-8">
            <asp:TextBox ID="txtOHIDReview" runat="server" CssClass="form-control"/>
        </div>        
    </div>

    <div class="row">
        <label for="txtEnrollmentType" class="col-md-4 text-right">Enrollment Type</label>
        <div class="col-md-8">
            <asp:TextBox ID="txtEnrollmentType" runat="server" CssClass="form-control"/>
        </div>        
    </div>

    <div class="row">
        <label for="txtFinalDisposition" class="col-md-4 text-right">Final Disposition</label>
        <div class="col-md-8">
            <asp:TextBox ID="txtFinalDisposition" runat="server" CssClass="form-control"/>
        </div>        
    </div>

    <div class="row">
        <label for="txtNewNote" class="col-md-4 text-right">New Note</label>
        <div class="col-md-8">
            <asp:TextBox ID="txtNewNote" runat="server" CssClass="form-control" 
                TextMode="MultiLine" Columns="100" Rows="4"/>
        </div>        
    </div>
    <div class="btnBox btnBoxCenter" style="padding-top: 10px; width: 93%;">
         <asp:Button runat="server" ID="btnCancelAdd" Text="Cancel" CssClass="buttonBox" OnClick="btnCancelAdd_Click" />
        <asp:button runat="server" id="btnSaveAdd" text="Save" cssclass="buttonBox" onclick="btnSaveAdd_Click" />
    </div>
</div>
