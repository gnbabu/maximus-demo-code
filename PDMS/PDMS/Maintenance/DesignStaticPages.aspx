<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Maintenance_DesignStaticPages" ValidateRequest="false" Codebehind="DesignStaticPages.aspx.cs" %>

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


<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="display: inline;" class="page-main-header">
        <br />
        <span style="text-align: center">Content Management</span>
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <style type="text/css">
        .page-main-header {
            font-family: 'Merriweather', serif !important;
            font-size: 28px !important;
        }

        .WhiteBox {
            font-family: 'Source Sans Pro', sans-serif !important;
        }

        .fieldTable > tbody > tr > td:nth-child(1) {
            width: 50%;
        }

        .formLabel {
            font-weight: bold;
        }

        #mainForm {
            margin-left: 15%;
        }

        .RightBox {
            width: 100%;
        }

        .UserHeader {
            width: 1120px;
        }

        .reContent {
            max-height: 16.1428em;
        }

        .reRemoveDOMElement {
            padding-left: 2% !important;
        }
    </style>
    <style>
        /* spacing */
        .form-group {
            margin-bottom: 18px;
        }

        .editor-panel {
            margin-top: 10px;
        }

        /* make the RadEditor responsive */


        /* bigger editor on larger screens */
        @media (min-width: 768px) {
            #edCustomEmail, .RadEditor {
                min-height: 480px;
            }
        }

        @media (min-width: 992px) {
            #edCustomEmail, .RadEditor {
                min-height: 600px;
            }
        }

        /* stack buttons on small screens while keeping them right-aligned on larger screens */
        @media (max-width: 479px) {
            #edCustomEmail, .RadEditor, .RadEditor .reContent {
                width: 100% !important;
                box-sizing: border-box;
                height: auto !important;
                min-height: 300px;
            }

            .action-row .btn {
                display: block;
                width: 100%;
                margin-bottom: 8px;
            }

            .action-row .text-right {
                text-align: left;
            }

            select[id$="ddlpage"],
            select[name$="ddlpage"] {
                display: block;
                width: 100% !important;
                max-width: 100% !important;
                box-sizing: border-box;
            }
        }



        .editor-panel {
            /*background: #fff;
            border: 1px solid #ddd;*/
            padding: 15px;
            border-radius: 4px;
        }

        /* Scrollable preview container */
        #divRender {
            max-height: 300px; /* change as required */
            overflow-y: auto;
            overflow-x: hidden;
            text-align: left !important;
            padding-right: 8px;
            box-sizing: border-box;
            word-wrap: break-word;
        }

        /* Align buttons to the right on small widths too */
        .action-row {
            margin-top: 15px;
        }

        .buttonBoxFocusBlue {
            margin-left: 8px;
        }

        .buttonBoxFocusGreen {
            color: #fff;
            background-color: #449d44 !important;
            font-weight: bold;
            padding: 5px 10px;
            line-height: 1.5 !important;
            border-radius: 3px !important;
            text-shadow: none !important;
            border: 1px solid transparent !important;
            width: auto;
            min-width: 100px;
            height: 40px;
        }
    </style>
    <div class="WhiteBox">
        <div class="row">
            <div class="col-xs-4">
                <div class="form-group">
                    <label for="<%= ddlpage.ClientID %>">Select Page</label>
                    <asp:DropDownList ID="ddlpage" runat="server" CssClass="form-control" Height="44px"
                        OnSelectedIndexChanged="ddlpage_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="--Select Page--" Value=""></asp:ListItem>
                        <asp:ListItem Text="Login" Value="Login"></asp:ListItem>
                        <asp:ListItem Text="Learning" Value="Learning"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="col-xs-12">
                <div class="form-group">
                    <asp:RequiredFieldValidator ID="valCustomEmailText" runat="server"
                        ErrorMessage="You must enter some text for the message" ValidationGroup="CustomEmail"
                        ControlToValidate="edCustomEmail" CssClass="text-danger">
                    </asp:RequiredFieldValidator>

                    <telerik:radeditor runat="server" id="edCustomEmail" rendermode="Lightweight"
                        width="100%" enableembeddedskins="true" enableajaxskinrendering="true"
                        ajaxurl="~/Telerik.Web.UI.SpellCheckHandler.axd"
                        contentfilters="DefaultFilters, PdfExportFilter" editmodes="Design, Preview"
                        dialoghandlerurl="~/Telerik.Web.UI.DialogHandler.aspx" onprerender="edCustomEmail_PreRender"
                        skin="Office2010Blue">
                        <cssfiles>
                            <telerik:editorcssfile value="~/EditorContentArea.css" />
                        </cssfiles>
                        <documentmanager maxuploadfilesize="7100000" />
                    </telerik:radeditor>
                </div>
            </div>

            <div class="col-xs-12">
                <div class="editor-panel">
                    <div class="row action-row">
                        <div class="col-xs-12 text-right">
                            <button id="btnPreview" class="buttonBoxFocusBlue" type="button">Preview</button>
                            <asp:Button ID="btnTest" runat="server" Text="Save" CssClass="buttonBoxFocusGreen" OnClick="btnTest_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="divRender" style="display: none; text-align: left">
        </div>
        <asp:HiddenField ID="hdnPagename" runat="server" />

    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnPreview").click(function (e) {
                e.preventDefault();
                var editor = $find("<%= edCustomEmail.ClientID %>");
                var content = editor.get_html();
                $("#divRender").html(content);

                $("#divRender").dialog({
                    modal: true,
                    width: 900,
                    height: 600,
                    title: "Preview",
                    close: function () {
                        $(this).dialog("destroy").hide().removeAttr("style");
                        $("#divRender").html('');
                    }
                });
            });
        });

        function disableButton(sender, group) {
            Page_ClientValidate(group);
            if (Page_IsValid) {
                sender.disabled = "disabled";
                __doPostBack(sender.name, '');
            }
        }

        function OnClientLoad(editor, args) {
            var style = editor.get_contentArea().style;
        }
    </script>
</asp:Content>
