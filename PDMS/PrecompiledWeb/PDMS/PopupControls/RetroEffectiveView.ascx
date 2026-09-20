<%@ control language="C#" autoeventwireup="true" inherits="Views_RetroEffectiveView, App_Web_wenzyumt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ register src="NPIandMedIdControl.ascx" tagname="EnrollmentData" tagprefix="uc" %>
<script type="text/javascript">
 
  var isSubmitted = false;
 
  function preventMultipleSubmissions() {
 
  if (!isSubmitted) {
 
    isSubmitted = true;
 
    return true;
 
  }
 
  else {
 
    return false;
 
  }
 
    }

    function Verifydate() {
        var frmdate = document.getElementById("<%= lblExistingEffectiveDate.ClientID%>").innerText;
          var fromdate = new Date(frmdate);
          var todte = $("input[id*='txtNewEffectiveDate']").val();
          var todate = new Date(todte);
          var DiffInTime = todate.getTime() - fromdate.getTime();
          var Diffindays = DiffInTime / (1000 * 3600 * 24);
          if (Diffindays < -365) {
              $("input[id*='txtNewEffectiveDate']").val("");
              document.getElementById("<%= lblError.ClientID%>").innerHTML = "New effective date must be Less than 365 days in the Past";
         }
         else {
             document.getElementById("<%= lblError.ClientID%>").innerHTML = "";
        }
    }

</script>
<div style="padding:5px;">
    <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
    <asp:ValidationSummary ID="vsNewRequest" DisplayMode="List" runat="server"  ValidationGroup="RetroEffective" ShowSummary="true"  />
	
    <uc:enrollmentData id="ucEnrollmentData" runat="server" />
	
    <div class="pg-hint" style="padding-right:4px;">* Designates a required field</div><br /><br />
    <div style="width:auto;">
         <div class="row">
                <div class="col-sm-5 text-right"><span class="formLabel wd200">Current Effective Date</span></div>
                <div class="col-sm-6 text-left"><asp:TextBox ID="lblExistingEffectiveDate" runat="server" CssClass="formField formField" Enabled="false" /></div>
            </div>
            <div class="row">
                <div class="col-sm-5 text-right"><span class="formLabel wd200">New Effective Date*</span></div>
                <div class="col-sm-6 text-left">
                    
                     <asp:TextBox ID="txtNewEffectiveDate" OnChange="Verifydate()" runat="server" CssClass="formfieldDate" Style="height: 30px; width: 90px" MaxLength="10" />
                   
                    <ajax:CalendarExtender ID="ceEffectiveDate" CssClass="QstCalendarCSS" Format="MM/dd/yyyy" runat="server" TargetControlID="txtNewEffectiveDate" runat="server" />
                    <asp:RequiredFieldValidator ID="valEffectiveDateReqd" runat="server"  SetFocusOnError="true" ValidationGroup="RetroEffective" Text="*"
                        ControlToValidate="txtNewEffectiveDate" ErrorMessage="* Effective Date is required." Display="Dynamic"  Enabled="true" />
                    
                    <br /> <asp:Label  id="lblError" runat="server" ></asp:Label>
                    <asp:CompareValidator id="cvEffectiveDate" runat="server" ValidationGroup="RetroEffective"   
                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtNewEffectiveDate"   Enabled="true"
                        ErrorMessage="* A valid Effective Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                        SetFocusOnError="true"> 
                    </asp:CompareValidator>
                </div>

        </div>
        <div class="row">
            <div class="col-sm-5 text-right"><span class="formLabel wd200">Comments*</span></div>
            <div class="col-sm-6 text-left"><asp:TextBox ID="txtComments" runat="server" Rows="7" CssClass="formField wd350" TextMode="MultiLine" MaxLength="4000" />
                <asp:RequiredFieldValidator ID="valCommentReqd" runat="server" SetFocusOnError="true" ValidationGroup="RetroEffective" Text="*"
                    ControlToValidate="txtComments" ErrorMessage="* Comments are required." Display="Dynamic"  Enabled="true" />
            </div>
        </div>
    </div>
    <div class="btnBox" style="padding-top:10px;padding-right:10px;">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClientClick="return preventMultipleSubmissions();"  OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="RetroEffective" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
    </div>
    <br />
</div>