<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_ReferToComplianceReasons, App_Web_l5y5araq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:UpdatePanel ID="upRTCompl" runat="server" UpdateMode="Conditional">                
  <ContentTemplate>
    <div><asp:ValidationSummary ID="vsReferToComplianceReasons" runat="server" DisplayMode="List" ValidationGroup="valReferToComplianceReasons" CssClass="failureNotification val-summary" /></div>
    <asp:Panel ID="pnlPSReview" runat="server" class="row">
        <div class="col-sm-12" id="divReturnReason" runat="server">
            <div class="row">
                <div class="col-sm-3 text-right">
                    <span class="formLabel150" style="vertical-align: top; padding-right:5px;"><b>Compliance Reason</b></span>
                </div>
                <div class="col-sm-9 text-left" style="vertical-align:bottom;padding-left:0px; ">
                    <fieldset>
                        <legend>
                         <label>
                             <asp:RadioButtonList ID="rblComplReason" runat="server" CssClass="radioButtonList" RepeatDirection="Vertical" />
                        </label>
                        </legend>
                    </fieldset>
                </div>
            </div>
           <div class="row">
                <div class="col-sm-3 text-right">
                    <span class="formLabel150" style="vertical-align: top; padding-right:5px"><b>Internal Comments</b></span>
                </div>
                <div class="col-sm-9 text-left">
                    <asp:TextBox ID="txtInternalNotes" runat="server" CssClass="formField" MaxLength="4000"/>                  
                </div>
            </div>
        </div>
    </asp:Panel>
   </ContentTemplate>
</asp:UpdatePanel>