<%@ control language="C#" autoeventwireup="true" inherits="Views_DisenrollmentView, App_Web_l5y5araq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<script>
    
    function ValidateModuleList(source, args)
    {
        var chkListModules = document.getElementById('<%= disEnrollmentOptions.ClientID %>');
        var chkListinputs = chkListModules.getElementsByTagName("input");
        for (var i = 0; i < chkListinputs.length; i++) {
            if (chkListinputs[i].checked) {
                args.IsValid = true;
                return;
            }
        }
        args.IsValid = false;
    }
</script>


<div style="padding:5px;" class="disenrollment-fields">
    <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
    <asp:ValidationSummary ID="vsNewRequest" DisplayMode="List" runat="server"    ValidationGroup="DisenrollProvider" ShowSummary="true" style="text-align:left"   />
    <div class="pg-hint" style="padding-right:4px;float:left !important">* Designates a required field</div><br /><br />
    <div style="width:auto;">
            <div class="row" id="divEffective" visible="true" runat="server">
                <div class="col-sm-6 adminPopUpLabelText text-right"><asp:Label CssClass="formLabelPopUp" runat="server" ID="lblEffctvDate">Effective Date</asp:Label></div>
                <div class="col-sm-6 text-left"><asp:Label ID="lblEffectiveDate" runat="server" CssClass="formFieldDisplay Wd100Percent" /></div>
            </div>
            <div class="row" id="divEnrollment" visible="true" runat="server">
                <div class="col-sm-6 adminPopUpLabelText text-right"><asp:Label CssClass="formLabelPopUp" runat="server" ID="lblEnrollStatus">Enrollment Status</asp:Label></div>
                <div class="col-sm-6 text-left"><asp:Label ID="lblEnrollmentStatus" runat="server" CssClass="formFieldDisplay Wd100Percent" /></div>  
            </div>
            <div class="row" id="divRevalidation" visible="true" runat="server">
                <div class="col-sm-6 adminPopUpLabelText text-right"><asp:Label CssClass="formLabelPopUp" runat="server" ID="lblRevalidationDueDate">Revalidation Due Date</asp:Label></div>
                <div class="col-sm-6 text-left"><asp:Label ID="lblRevalDueDate" runat="server" CssClass="formFieldDisplay Wd100Percent" /></div>
            </div>
            <div class="row">
                <div class="col-sm-6 adminPopUpLabelText text-right"><asp:Label Cssclass="disenrollmentLabelLeft formLabelPopUp" runat="server" ID="lblDisenrollEffectiveDate">Disenrollment Effective Date*</asp:Label></div>
                <div class="col-sm-6 text-left"><asp:TextBox ID="txtTermDate" aria-label="Disenrollment Effective Date" runat="server" CssClass="form-control" MaxLength="10" />
                    <ajax:CalendarExtender ID="ceTermDate" TargetControlID="txtTermDate" runat="server" />
                    <asp:RequiredFieldValidator ID="valTermDateReqd" runat="server" SetFocusOnError="true" ValidationGroup="DisenrollProvider" Text="*"
                        ControlToValidate="txtTermDate" ErrorMessage="* Disenrollment Effective Date is required." Display="Dynamic"  Enabled="true" />
                    <asp:CompareValidator id="cvTermDate" runat="server" ValidationGroup="DisenrollProvider"   
                        Type="Date" Operator="DataTypeCheck" ControlToValidate="txtTermDate"   Enabled="true"
                        ErrorMessage="* A valid Disenrollment Effective Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                        SetFocusOnError="true"> 
                    </asp:CompareValidator>
                </div>
        </div>
        <div id="divRadio" runat="server" style="width: auto; ">
            <div class="row">
                    <div class="col-sm-4 text-left">
                        <%--<p><strong>Indicate all that apply</strong></p>--%>
                    </div>
                    <div class="col-sm-4 text-left">
                        <asp:Label ID="lblError" runat="server" Text="*Please select at least one reason for Disenrollment." Visible="false" CssClass="bodyTextRed"></asp:Label>
                        <div style="margin: 0 auto; width: 600px;">
                            <asp:CheckBoxList ID="disEnrollmentOptions" runat="server"></asp:CheckBoxList>
                            <asp:CustomValidator runat="server" ID="cvmodulelist"
  ClientValidationFunction="ValidateModuleList"
  ErrorMessage="* Please Select Atleast one reason" ValidationGroup="DisenrollProvider" SetFocusOnError="true" Display="None"  Enabled="true"></asp:CustomValidator>
                        </div>
                    </div>
                </div>           

        </div>

        <div id="divsuspend" visible="true" class="row">
            <div class="col-sm-6 adminPopUpLabelText text-right"><asp:Label CssClass="disenrollmentLabelLeft formLabelPopUp" runat="server" ID="lblEnrollstatusreason">Enrollment Status Reason Code*</asp:Label></div>
            <div class="col-sm-6 text-left"><asp:DropDownList ID="ddlEnrollreason" aria-label="Enrollment Status Reason Code" runat="server" EnableViewState="true" CssClass="form-control"></asp:DropDownList></div>
           
        </div>

        <div class="row" id="divcomments" visible="true" runat="server">
            <div class="col-sm-6 adminPopUpLabelText text-right"><asp:Label CssClass="formLabelPopUp" runat="server" ID="lblCommnts">Comments*</asp:Label></div>
            <div class="col-sm-6 text-left">
<%--                <asp:TextBox ID="txtComments" runat="server"  CssClass="formField wd200" TextMode="MultiLine" MaxLength="4000" style=" white-space: pre-wrap; vertical-align:text-top"  Wrap="True"></asp:TextBox> --%>
                <textarea id="txtComments_area" aria-label="Comments" cols="20" rows="2" runat="server"></textarea>

                <asp:RequiredFieldValidator ID="valCommentReqd" runat="server" SetFocusOnError="true" ValidationGroup="DisenrollProvider" Text="*"
                    ControlToValidate="txtComments_area" ErrorMessage="* Comments are required." Display="Dynamic"  Enabled="true" />
            </div>
        </div>
    </div>
    <div class="btnBox" style="padding-top:10px;padding-right:10px;">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="DisenrollProvider" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
    </div>
    <br />
</div>