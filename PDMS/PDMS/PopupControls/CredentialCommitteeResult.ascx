<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_CredentialCommitteeResult" Codebehind="CredentialCommitteeResult.ascx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="scs" %>

<script  type="text/javascript">
     

        function openLink(url) {
            window.open(url, 'newWindow');
    }
    function valiateTextboxLength(e, len) {
    if (e.value.length > len) {
        var temp = e.value.substring(0, len);
        $('#' + e.id).val(temp);
    }
}
</script>
<style type="text/css">
  
</style>


<asp:Panel ID="pnlCommittee" runat="server">
    <div id="divCredentialCommittee" class="container-fluid">
        <div class="row">
           <div class="col-sm-4 text-right"><span class="formLabel">Result * </span></div>
           <div class="col-sm-8 text-left">
               <asp:CustomValidator Id="RoleCustomValidator" runat="server" Enabled="true" ValidationGroup="valCredentialing" Text="" Display="None"
                   OnServerValidate="RoleCustomValidator_ServerValidate" ErrorMessage="This is a medium or high risk file and must be dispositioned by a Credentials Committee Quality Specialist." />

               <asp:DropDownList runat="server" ID="ddlCommResult" CssClass="formDropDown" 
                     AutoPostBack="true"
                     OnSelectedIndexChanged="ddlCommResult_SelectedIndexChanged" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlCommResult" Enabled="true" SetFocusOnError="true" 
                        Display="Dynamic" Text="*"  ValidationGroup="valCredentialing" ErrorMessage="* Result is required."></asp:RequiredFieldValidator>
           </div>
        </div>
         <div class="row">
           <div class="col-sm-4 text-right"><span class="formLabel">Decision Date * </span></div>
           <div class="col-sm-8 text-left"><asp:TextBox runat="server" ID="txtDecisionDate" CssClass="formField"/>
               <cc1:CalendarExtender ID="calDEAEffectiveDate" TargetControlID="txtDecisionDate" runat="server" />
               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDecisionDate" Enabled="true" SetFocusOnError="true" 
                        Display="Dynamic" Text="*"  ValidationGroup="valCredentialing" ErrorMessage="* Decision Date is required."></asp:RequiredFieldValidator>
               <asp:CompareValidator id="CompareValidator2" runat="server" ValidationGroup="valCredentialing"   
            Type="Date" Operator="DataTypeCheck" ControlToValidate="txtDecisionDate"  
            ErrorMessage="Select a valid Date of Effective" Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
            SetFocusOnError="true" /> 
           </div>
        </div>
         <div class="row">
           <div class="col-sm-4 text-right"><asp:Label runat="server" id="lblDenialTermReason" class="formLabel" text="Denial/Termination Reason" /></div>
           <div class="col-sm-8 text-left"><asp:TextBox runat="server" ID="txtDenialTermReason" onkeydown="valiateTextboxLength(this, 200)" CssClass="formFieldMultiline wd450" TextMode="MultiLine"/></div>
        </div>
         <div class="row">
           <div class="col-sm-4 text-right"><span class="formLabel">Summary</span></div>
           <div class="col-sm-8 text-left"><asp:TextBox runat="server" ID="txtSummary" onkeydown="valiateTextboxLength(this, 200)" CssClass="formFieldMultiline wd450" TextMode="MultiLine"/></div>
        </div>
         <div class="row">
           <div class="col-sm-4 text-right"><span class="formLabel300">Committee Discussion and Decision</span></div>
           <div class="col-sm-8 text-left"><asp:TextBox runat="server" ID="txtCommdiscuss" onkeydown="valiateTextboxLength(this, 500)" CssClass="formFieldMultiline wd450" TextMode="MultiLine" /></div>
        </div>
    </div>
    <div class="row btnBoxCenter">
        <br />
        <asp:Button runat="server" ID="btnConfirmComm" CssClass="buttonBoxFocus" Text="Save"  EnableViewState="true" ValidationGroup="valCredentialing" CausesValidation="true"  OnClick="btnConfirmComm_Click"/>
        <asp:Button runat="server" ID="btnCancelCommittee" CssClass="buttonBox" Text="Cancel"  CausesValidation="false" OnClick="btnCancelCommittee_Click" />
    </div>
    <%-- <asp:DataList ID="dtlCommittee" runat="server" DataKeyField="COMMITTEE_MEMBER_ID"
     EnableViewState="False" Width="100%" HorizontalAlign="Center" RepeatLayout="Table" CellPadding="5"
           CellSpacing="5" ItemStyle-Wrap="true" OnItemDataBound="dtlCommittee_ItemDataBound" >
    <ItemTemplate>       
        <table border="0" style="border-bottom-style:solid;border-bottom-color:grey;border-bottom-width: thin;table-layout:fixed;">
            <tr>
                <td class="formLabel text-right" style="width:250px;">Committee Member Name:</td>
                <td style="width:25%;"><asp:Label ID="lblCmtMemberName" runat="server" CssClass="text-left"
                    Text='<%# Eval("MEMBER_NAME") %>'  /></td>
                <td class="formLabel text-right" style="width:250px;">Committee Role:</td>
                <td style="width:25%;"><asp:Label ID="lblCmtRole" runat="server" CssClass="text-left"
                    Text='<%# Eval("ROLE") %>' /></td>
            </tr>
            <tr>
                <td class="formLabel text-right" style="width:250px;word-wrap:break-word">Action:</td>
                <td style="width:25%;"><asp:Label ID="lblCmtAction" runat="server"
                    Text='<%# Eval("COMMITTEE_ACTIVITY_STATUS_NAME") %>' CssClass="text-left" /></td>
                <td class="formLabel text-right" style="width:250px;">Date Of Action:</td>
                <td style="width:25%;"><asp:Label ID="lblDateOfAction" runat="server" CssClass="text-left"
                    Text='<%# Eval("ACTION_DATE", "{0:MM/dd/yy}") %>' /></td>
            </tr>
            <tr>
                <td class="formLabel text-right" style="width:250px;">Comments:</td>
                <td style="max-width:300px;word-wrap:break-word" colspan="3" ><asp:Label ID="lblComments" runat="server" CssClass="text-left"
                    Text='<%# Eval("COMMENTS") %>'  /></td>
               
            </tr>
        </table>
    </ItemTemplate>
</asp:DataList>--%>

</asp:Panel>