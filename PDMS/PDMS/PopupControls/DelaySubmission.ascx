<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_SubmitClaimDelaySubmission" Codebehind="DelaySubmission.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>

 <asp:Panel ID="pnlDelayedSubReSubinfo_UC" runat="server" Style="min-height: 100px; min-width: 150px; height: auto; width: auto; max-width:98%;">
        <div class="row">
            <div class="col-sm-12">
                <span class="ohio-field" style="font-size: 17px;font-weight: bold;padding-left:20px;">Disclaimer: Documentation to justify the use of this panel and data entered must be retained for future audit purpose.</span>
            </div>
        </div>
            <div class="row" id="dvDentalReason" runat="server" visible="false">
                 <div class="col-md-2">
                        <span class="ohio-field" style="font-size: 15px;padding-left:20px;text-align:right;">Reason for Delay:</span>
                 </div>
                 <div class="col-md-10">
                        <asp:DropDownList ID="ddlReason" EnableViewState="true" runat="server" AppendDataBoundItems="True" Style="height: 30px; Width:200px">
                        </asp:DropDownList>
                       <%-- <asp:RequiredFieldValidator runat="server" ID="rfvReason" SetFocusOnError="true"
                             ValidationGroup="validateDelaySubmission" ControlToValidate="ddlReason" ErrorMessage="*Missing Reason for Delay" Text="*" Display="Dynamic" InitialValue="0" />--%>
                 </div>
            </div>
            <div class="row" id="dvProfReason" runat="server" visible="false">
            <div class="col-md-5">
                <div class="row">
                    <div class="col-md-4">
                        <span class="ohio-field" style="font-size: 15px;text-align:right;padding-left:20px;">Previously Denied ICN:</span>
                     </div>
                    <div class="col-md-8">
                       <asp:TextBox ID="txtPreviouslyDeniedICN" runat="server" MaxLength="50"/>
                    </div>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtPreviouslyDeniedICN" ValidationExpression="^[A-Za-z0-9?\.\d ,_-]+$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                </div>
            </div>
            <div class="col-md-7">
                <div class="row">
                     <div class="col-md-5">
                        <span class="ohio-field" style="font-size: 15px;text-align:right;">Reason for Delay:</span>
                     </div>
                     <div class="col-md-7">
                            <asp:DropDownList ID="ddlProfReason" EnableViewState="true" runat="server" AppendDataBoundItems="True" Style="height: 30px; Width:200px;">
                            </asp:DropDownList>
<%--                            <asp:RequiredFieldValidator runat="server" ID="rfvddlProfReason" SetFocusOnError="true"
                                 ValidationGroup="validateDelaySubmission" ControlToValidate="ddlProfReason" ErrorMessage="*Missing Reason for Delay" Text="*" Display="Dynamic" InitialValue="" />--%>
                     </div>
                </div>
            </div> 
            </div>
     <asp:HiddenField ID="hdnDelayReasonClaimID" runat="server" />
     <asp:HiddenField ID="hdnDelayReasonClaimType" runat="server" />
    </asp:Panel>
