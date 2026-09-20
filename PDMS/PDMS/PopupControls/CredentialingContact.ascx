<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_CredentialingContact" Codebehind="CredentialingContact.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/CredentialingContactHistory.ascx" TagPrefix="uc" TagName="CredentialingContactHistory" %>
<script src="../Scripts/jquery.inputmask.bundle.min.js"></script>         

<script type="text/javascript">

     function CheckPhoneLength(sender, args) {
        var re = /\D/g; // Remove any characters that are not numbers
        var test = args.Value.replace(re, "");
        if (test === "") return true;

        var len = test.length;
        if (len !== 10)
            return false;
        if (test[0] === 0 || test[0] === 1 || test[3] === 0 || test[3] === 1)
            return false;

        return true;
    }
     function alphanumericOnly(obj) {
        obj.value = obj.value.replace(/[^a-zA-Z0-9]/g, '');
    }

    function isNumber(evt) {
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }

    $(document).ready(function(){
        $('.phone_number').inputmask('(999) 999-9999');
    });

    function removeDisabled() {
        $("#<%= btnCloseHistory.ClientID %>").removeAttr('disabled');
        $("#<%= btnExportHistory.ClientID %>").removeAttr('disabled');
    }
</script>
<div onmouseover="removeDisabled();">
<div><asp:ValidationSummary ID="vsCredentialContact" runat="server" DisplayMode="List" ValidationGroup="vgCredentialContact" CssClass="failureNotification"/></div>
<div class="divHistoryAndAdd">
    <asp:LinkButton TabIndex="0" ID="btnHistory" CommandName="History" runat="server" CssClass="buttonBoxFocus" OnCommand="btnHistory_Click" Text="History!" ToolTip="History" Style="color: white; text-decoration: none;">
        History</asp:LinkButton>
</div>

<asp:Panel runat="server" ID="pnlCredentialingContactGrid">
    <div>
     <br /> 
  <h2><span class="pageHeader">Add Contact</span>
            <br />  <br />
        </div>
<div class="divGrid">
    <asp:GridView runat="server" ID="grdCredentialingContact" AllowPaging="false" PageSize="1" AutoGenerateColumns="False" HorizontalAlign="Left" CssClass="gridview"
        EmptyDataText="No records found" OnRowCommand="grdCredentialingContact_RowCommand" >
        <Columns>
             <asp:BoundField DataField="CONTACT_NAME" HeaderText="Contact Name" />
             <asp:BoundField DataField="PRACTICE_NAME" HeaderText="Practice Name" />
             
             <asp:TemplateField HeaderText="Phone">
                <ItemTemplate>
                    <asp:Literal ID="litPhone" runat="server" Text='<%# FormatPhone(Eval("CONTACT_NUMBER")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
         <asp:BoundField DataField="EMAIL_ID" DataFormatString="<a href=mailto:{0}>{0}</a>" HtmlEncodeFormatString="false"  HeaderText="Email" SortExpression="Email" />
         <asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" HeaderText="Update Date" SortExpression="UpdatedDate" DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" />

             <asp:TemplateField ItemStyle-Width="2%">
             <ItemTemplate>
              <asp:ImageButton ID="btnEdit" alt="EditButton" runat="server" CommandName="EditCredentialingContact" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
              ImageUrl="~/Images/edit.png" ToolTip="Edit" />
             </ItemTemplate>                                       
             </asp:TemplateField>
             <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDelete" runat="server" CommandName="DeleteRow" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                            ImageUrl="~/Images/cancel.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete?');" AlternateText="Delete"/>
                    </ItemTemplate>
                </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
        <HeaderStyle CssClass="gridViewHeader" Width="100px" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" />
        <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>


</div>
<div class="divHistoryAndAdd">
       <asp:ImageButton ID="btnAddCredentialingContactItem" AlternateText="Add New" runat="server" ImageUrl="~/Images/add.png" CommandName="CredentialingContactAdd" OnCommand="btnAddCredentialingContactItem_Click" ToolTip="Add" />
       <%--<asp:ImageButton ID="btnCredentialingContactHistory" CommandName="CredentialingContactHistory" runat="server" ImageUrl="~/Images/history_icon.jpg" OnCommand="btnCredentialingContactHistory_Click" ToolTip="History" />--%>
</div>
<br />
</asp:Panel>
<asp:UpdatePanel ID="updPnlCredentialingContactEntry" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
<asp:Panel runat="server" ID="pnlCredentialingContactEntry" Visible="false">
<div style="width:100%;">
<span style="color:#de2316; font-size: 14pt !important"><b>An asterisk * indicates a required field</b></span>   
    <div class="row">
        <div class="col-sm-4  text-right">
            <asp:Label ID="lblSchool" runat="server" Text="*Contact Name" CssClass="formLabel wd200"></asp:Label>
            </div>
            <div class="col-sm-8">
                <asp:TextBox ID="txtContactName" runat="server" CssClass="formField" aria-label="Contact Name" aria-required="true" MaxLength="30"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvContactName" SetFocusOnError="true" 
            ValidationGroup="vgCredentialContact" ControlToValidate="txtContactName" ErrorMessage="*Enter Contact Name" Text="*" Display="Dynamic"  />
            </div>
    </div>
     <div class="row">
        <div class="col-sm-4  text-right">
            <asp:Label ID="Label1" runat="server" Text="*Practice Name" CssClass="formLabel wd200"></asp:Label>
            </div>
            <div class="col-sm-8">
            <asp:TextBox ID="txtPracticeName" runat="server" CssClass="formField" aria-label="Practice Name" aria-required="true" MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvPracticeName" SetFocusOnError="true" 
            ValidationGroup="vgCredentialContact" ControlToValidate="txtPracticeName" ErrorMessage="*Enter Practice Name" Text="*" Display="Dynamic"  />
            </div>
    </div>

     <div class="row">
        <div class="col-sm-4  text-right">
            <asp:Label ID="Label3" runat="server" Text="*Contact Phone No" CssClass="formLabel wd200"></asp:Label>
            </div>
            <div class="col-sm-8">
            <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="formField phone_number" aria-label="Phone number" aria-required="true"></asp:TextBox>                 
                <asp:RequiredFieldValidator runat="server" ID="rfvPhoneNumber" SetFocusOnError="true" 
            ValidationGroup="vgCredentialContact" ControlToValidate="txtPhoneNumber" ErrorMessage="*Enter Phone Number" Text="*" Display="Dynamic" InitialValue="(___) ___-____"  />
                 <asp:CustomValidator ID="cvPhoneNum" runat="server" SetFocusOnError="True" Display="Dynamic"
                        ControlToValidate="txtPhoneNumber" ClientValidationFunction="CheckPhoneLength"
                        ErrorMessage="Enter valid Contact Phone Number" Text="*" ValidationGroup="vgCredentialContact" />
            </div>
    </div>
    <div class="row">
        <div class="col-sm-4  text-right">
            <asp:Label ID="Label4" runat="server" Text="Contact Phone Extension" CssClass="formLabel wd200"></asp:Label>
            </div>
            <div class="col-sm-8">
            <asp:TextBox ID="txtExtension" runat="server" CssClass="formField" aria-label="extention" MaxLength="5" onkeypress="return isNumber(event)"></asp:TextBox>
            </div>
    </div>
    <div class="row">
        <div class="col-sm-4  text-right">
            <asp:Label ID="Label5" runat="server" Text="Contact Fax No" CssClass="formLabel wd200"></asp:Label>
            </div>
            <div class="col-sm-8">
            <asp:TextBox ID="txtFaxNo" runat="server" CssClass="formField phone_number" aria-label="fax Number" onKeyUp="javascript:alphanumericOnly(this);"></asp:TextBox>            
            
                 <asp:CustomValidator ID="CustomValidator1" runat="server" SetFocusOnError="True" Display="Dynamic"
                        ControlToValidate="txtFaxNo" ClientValidationFunction="CheckPhoneLength"
                        ErrorMessage="Enter valid Fax Number" Text="*" ValidationGroup="vgCredentialContact" />
            </div>
    </div>

    <div class="row">
        <div class="col-sm-4  text-right">
            <asp:Label ID="Label6" runat="server" Text="*Contact Email" CssClass="formLabel wd200"></asp:Label>
            </div>
            <div class="col-sm-8">
            <asp:TextBox ID="txtContactEmail" runat="server" aria-label="Contact Email" aria-required="true" CssClass="formField"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvContactEmail" SetFocusOnError="true" 
            ValidationGroup="vgCredentialContact" ControlToValidate="txtContactEmail" ErrorMessage="*Enter Contact Email" Text="*" Display="Dynamic"  />
                <asp:RegularExpressionValidator ID="regEmail1" runat="server" ControlToValidate="txtContactEmail" Display="Dynamic" Text="*" ValidationGroup="vgCredentialContact"
                    ErrorMessage="Enter valid E-mail" SetFocusOnError="true" ValidationExpression="^[A-Z'a-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}$" />
            </div>
    </div>
    

     <div class="row">
       <div class="col-sm-4  text-right">
            <asp:Label ID="Label7" runat="server" Text="Comments" CssClass="formLabel wd200"></asp:Label>
        </div>
        <div class="col-sm-8">
            <asp:TextBox ID="txtComments" runat="server" aria-label="comments" TextMode="MultiLine" CssClass="formField"></asp:TextBox>

        </div>
    </div>
</div>
        <asp:CustomValidator ID="cvCredentialContact"
        ControlToValidate=""
        OnServerValidate="cvCredentialContact_ServerValidate"
        Display="None"
        ErrorMessage=""
        ValidationGroup="vgCredentialContact"
        runat="server" />
    </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Panel ID="upCredentialingContactHistory" runat="server">
    <ajax:ModalPopupExtender ID="mpe" runat="server" BackgroundCssClass="modalBackground" CancelControlID="btnCloseHistory" PopupControlID="pnlModal" PopupDragHandleControlID="pnlModal" TargetControlID="ButtonDummy3" />
    <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: auto;">
        <asp:Panel ID="pnlHeader" runat="server" CssClass="pnlHeader" HorizontalAlign="Left">
            <div align="left">
                &nbsp;&nbsp;<asp:Label ID="lblTitle" runat="server" CssClass="bodyTextBold" ForeColor="White" Text="Title" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px">
            <div class="container-fluid" style="text-align: left; padding: 15px;">
                <div class="row">
                    <uc:CredentialingContactHistory ID="ucCredentialingContactHistory" runat="server" />
                </div>
                <div class="row">
                    <div class="btnBox" style="text-align: right;">
                        <asp:Button ID="btnCloseHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="OK" />
                        <asp:Button ID="btnExportHistory" runat="server" CausesValidation="false" CssClass="buttonBox" Text="Export" OnClick="lnkExcel_Click" />
                    </div>
                </div>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Button runat="server" aria-Label="Dummy Button" ID="ButtonDummy3" Style="display: none" Text=”ButtonDummy3” />
</asp:Panel>
    <asp:HiddenField ID="hdnRegCredentialingContactId" runat="server" />
<div style="width: 100%; text-align: right; display: none;">
    <asp:HiddenField ID="hdnRowCount" runat="server" />
    <asp:Button ID="lnkExcel" CommandName="Export" OnClick="lnkExcel_Click" runat="server" ToolTip="Excel" Visible="true" Text="Export"/>
    <div id="divRowCount" title="High Row Count" style="display: none; text-align: center">
        <p style="color: darkgreen" id="paraRowCount" runat="server">Only the first 10000 results from the search will be exported.</p>
    </div>
    <telerik:RadGrid ID="grd" runat="server" AllowCustomPaging="false" AllowSorting="false" Skin="PDMSModern" EnableEmbeddedSkins="false"
        AutoGenerateColumns="true" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-BackColor="#f7f7f7">
        <ExportSettings IgnorePaging="true" OpenInNewWindow="true" ExportOnlyData="true">
            <Excel Format="Biff" />
        </ExportSettings>
        <MasterTableView Width="100%" AllowSorting="false" AllowPaging="false" AutoGenerateColumns="false" TableLayout="Auto"
            DataKeyNames="INDEX, DateOfAction" EnableHeaderContextMenu="true" AllowMultiColumnSorting="false">
            <Columns>
                <telerik:GridBoundColumn DataField="OPERATION"           HeaderText="Operation"      SortExpression="Operation" />
                <telerik:GridBoundColumn DataField="CONTACT_NAME"    HeaderText="Contact Name"   SortExpression="CONTACT_NAME" />
                <telerik:GridBoundColumn DataField="PRACTICE_NAME"   HeaderText="Practice Name"  SortExpression="PRACTICE_NAME" />
                <telerik:GridBoundColumn DataField="CONTACT_NUMBER"  HeaderText="Phone"          SortExpression="CONTACT_NUMBER" />
                <telerik:GridBoundColumn DataField="EMAIL_ID"        HeaderText="Email"          SortExpression="EMAIL_ID" />
                <telerik:GridBoundColumn DataField="UserName"        HeaderText="User Name"      SortExpression="UserName" />
                <telerik:GridBoundColumn DataField="DateOfAction"    HeaderText="Update Date"    SortExpression="DateOfAction"   DataFormatString="{0:MM/dd/yyyy hh:mm:ss}" HtmlEncode="False"  />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</div>
</div>