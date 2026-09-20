<%@ control language="C#" autoeventwireup="true" inherits="PopupControls_CredentialingReturntoProviderReasons, App_Web_l5y5araq" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:UpdatePanel ID="upRTP" runat="server" UpdateMode="Conditional">                
  <ContentTemplate>
    <div><asp:ValidationSummary ID="vsReturnToProviderReasons" runat="server" DisplayMode="List" ValidationGroup="valReturnToProviderReasons" /></div>
    <asp:Panel ID="pnlPSReview" runat="server" class="row">
        <div class="col-sm-12" id="divReturnReason" runat="server">
            <div class="row">
                <div class="col-sm-3 text-right" style="padding-right: 0px;"><span class="formLabel">Return Reason</span></div>
                <div class="col-sm-9 text-left">
                    <asp:Panel ID="Panel1" ScrollBars="Vertical" Height="200" runat="server" CssClass="fullWidth">
                        <asp:ListView ID="lsvReturnReasons" runat="server">
                            <LayoutTemplate>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div style="display: table-row;">
                                    <asp:CheckBox ID="chkReason" runat="server" Text=" " Style="margin-left: 5px;" /><asp:Label ID="lblReason" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ERROR_NAME") %>' />
                                    <asp:HiddenField ID="hdnErrorTypeID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ERROR_TYPE_ID") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right">
                    <span class="formLabel300" style="vertical-align: top">Notes To Provider</span>
                </div>
                <div class="col-sm-9 text-left">
                    <asp:TextBox ID="txtProviderNotes" runat="server" CssClass="formField" MaxLength="4000" />   
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtProviderNotes" ValidationExpression="^[a-zA-Z0-9\s,.;?()\-]*$"
                                    ErrorMessage="*" Enabled="true" SetFocusOnError="true" Text="*Invalid Input" Display="Dynamic" ForeColor="Red" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-3 text-right">
                    <span class="formLabel150" style="vertical-align: top">Internal Comments</span>
                </div>
                <div class="col-sm-9 text-left">
                    <asp:TextBox ID="txtInternalNotes" runat="server" CssClass="formField" MaxLength="4000"/>                  
                </div>
            </div>
        </div>
    </asp:Panel>
   </ContentTemplate>
</asp:UpdatePanel>