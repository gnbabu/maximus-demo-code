<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Maintenance_DesignStaticPages, App_Web_25ar0nw3" validaterequest="false" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc2" %>
<%@ Register Src="~/PopupControls/UploadDocumentControl.ascx" TagName="UploadDocumentControl" TagPrefix="uc" %>
<%@ Register Src="FormField.ascx" TagName="FormField" TagPrefix="uc" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="wc" %>
<script runat="server">

    protected void edCustomEmail_PreRender(object sender, EventArgs e)
    {

    }
</script>


<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    <b>Content Management</b>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
            <div class="WhiteBox" >
                <style type="text/css">

        .fieldTable>tbody>tr>td:nth-child(1)
        {
            width:50%;
        }
        .formLabel
        {
            font-weight:bold;
        }
    </style>
    <script type="text/javascript">
        

    </script>
                <style type="text/css">

#mainForm
{
    margin-left: 15%;  

}
.RightBox
{
    width: 100%;
}
.UserHeader
{
    
    width: 1120px;
    
}
  .reContent
 {
     max-height: 16.1428em;
 }
.reRemoveDOMElement{
     padding-left: 2% !important;
 }
</style>
                <script type="text/javascript">
    function disableButton(sender, group) {
        Page_ClientValidate(group);
        if (Page_IsValid) {
            sender.disabled = "disabled";
            __doPostBack(sender.name, '');
        }
    }
</script>
                <asp:Panel ID="pnlSearch" runat="server" >
                    <div>
                    </div>
                </asp:Panel>
                <br />
                <table>
                    <tr><td>
                        <asp:DropDownList ID="ddlpage" runat="server" OnSelectedIndexChanged="ddlpage_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
                            <asp:ListItem Text="Login" Value="Login"></asp:ListItem>
                            <asp:ListItem Text="Learning" Value="Learning"></asp:ListItem>
                        </asp:DropDownList>
                        </td></tr>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator ID="valCustomEmailText" runat="server" ErrorMessage="You must enter some text for the message" ValidationGroup="CustomEmail" ControlToValidate="edCustomEmail"></asp:RequiredFieldValidator>
                            <telerik:RadEditor runat="server" ID="edCustomEmail" RenderMode="Lightweight" Width="900px" Height="600px" EnableEmbeddedSkins="true" EnableAjaxSkinRendering="true" AjaxUrl="~/Telerik.Web.UI.SpellCheckHandler.axd"
                    ContentFilters="DefaultFilters, PdfExportFilter"  EditModes="Design, Preview" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.aspx" OnPreRender="edCustomEmail_PreRender" Skin="Office2010Blue" >
                                <CssFiles>
                                    <telerik:EditorCssFile Value="~/EditorContentArea.css" />
                                </CssFiles>
                                <DocumentManager  MaxUploadFileSize="7100000" />
                            </telerik:RadEditor>
                        </td>
                        <td>
                            <div id="divRender" runat="server">
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnPreview" text ="Preview" OnClick="btnPreview_Click" runat="server"/>
                <asp:Button ID="btnTest" text ="Save" OnClick="btnTest_Click" runat="server"/>
                <asp:HiddenField ID="hdnPagename" runat="server" />
            </div>
    
    <script type="text/javascript">
        function OnClientLoad(editor, args) {
            var style = editor.get_contentArea().style;
            /*style.backgroundImage = "none";
            style.backgroundColor = "black";
            style.color = "red";
            style.fontFamily = "Arial";
            style.fontSize = 15 + "px";*/
        }
    </script> 
</asp:Content>