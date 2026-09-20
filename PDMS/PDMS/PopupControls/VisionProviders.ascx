<%@ Control Language="C#" AutoEventWireup="true" Inherits="PopupControls_VisionProviders" Codebehind="VisionProviders.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

 <script src="//ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">

    function ValidateModuleList(source, args) {
        var chkListModules = document.getElementById('<%= ckPrescribe.ClientID %>');
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


        <div>
            <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
              </div>
        <div style="width:auto;">
                <div class="row">
                  <div class="col-sm-3"><asp:label ID="lblCertified" runat="server" Text ="Which of the following are you certified to use or prescribe?"  class="formLabel wd200"/></div>
                  <div class="col-sm-9">
                    <asp:CheckBoxList ID="ckPrescribe" runat="server">
                        <asp:ListItem Text="Topical Ocular Dignostic Pharmaceutical Agents" Value="Topical"></asp:ListItem>
                        <asp:ListItem Text="Therapeutic Pharmaceutical Agents" Value="Therapeutic"></asp:ListItem>
                        <asp:ListItem Text="Dignostic Pharmaceutical Agents" Value="Dignostic"></asp:ListItem>
                    </asp:CheckBoxList>
                     <asp:CustomValidator runat="server" ID="cvPrescribe" ErrorMessage="*Please Select" ClientValidationFunction="ValidateModuleList" ValidationGroup="vgVisionProviders"></asp:CustomValidator>
                  </div>            
                </div>
            <div class="row">
                <div class="col-sm-3"><span class="formLabel wd200">Does your office have an on-site lab?</span></div>
                <div class="col-sm-9">       
                <asp:RadioButtonList ID="rblOnSiteLab" runat="server" RepeatDirection="Horizontal" CssClass="QstRadioList">
                     <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                     <asp:ListItem Text="No" Value="False" Selected="True"></asp:ListItem>
                </asp:RadioButtonList></div>
            </div>
        </div>

           


<asp:TextBox ID="hidIsEdit" runat="server" Visible="false" />
<asp:TextBox ID="hidID" runat="server" Visible="false" />