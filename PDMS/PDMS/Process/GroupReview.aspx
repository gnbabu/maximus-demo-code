<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" Inherits="Process_GroupReview" MaintainScrollPositionOnPostback="true" Codebehind="GroupReview.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/GroupReview.ascx" TagName="GroupReview" TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="display: inline;">
        <div class="page-main-header">
            <span style="text-align: center">
                <asp:Label ID="lblTitle" runat="server" Style="text-align: center; position: relative; top: 24px" Text="Provider Review" />
            </span>
        </div>
        <asp:Panel ID="pnlReturn" runat="server" CssClass="PanelReturn" Style="display: inline; float: right;">
            <asp:Button ID="btnReturn" runat="server" Text="Return" CssClass="buttonBox" OnClick="btnReturn_Click" CausesValidation="false" />
            <asp:Panel ID="pnlGA" runat="server" Visible="false">
                &nbsp;&nbsp;
                        <asp:Button ID="btnReturnToGroupAffiliations" runat="server" Text="Return to Group Affiliations" CssClass="buttonBox" OnClick="btnReturnToGroupAffiliations_Click" CausesValidation="false" />
            </asp:Panel>
        </asp:Panel>
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function doClick(buttonName, e) {
            // The purpose of this function is to allow the enter key to 
            // point to the correct button to click.
            var key;

            if (window.event) key = window.event.keyCode;   // IE
            else key = e.which;                             // Firefox

            if (key == 13) {
                //Get the button the user wants to have clicked
                var btn = document.getElementById(buttonName);
                if (btn != null) { //If we find the button click it
                    btn.click();
                    event.keyCode = 0
                }
            }
        }
    </script>
    <style type="text/css">
        .page-main-header {
            font-family: 'Merriweather', serif !important;
            font-size: 28px !important;
        }

        .group-review-container {
            font-family: 'Source Sans Pro', sans-serif !important;
        }

        .form-control {
            font-size: 17px;
            height: 44px;
            color: #000;
        }

        .ohio-field-input {
            height: 44px;
            color: #000;
        }
    </style>
    <div class="group-review-container">
        <uc1:groupreview id="ucGroupReview" runat="server" />
    </div>
</asp:Content>
