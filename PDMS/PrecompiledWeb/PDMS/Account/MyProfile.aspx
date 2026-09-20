<%@ page title="Change Password" language="C#" masterpagefile="~/Site.master" autoeventwireup="true" inherits="Account_MyProfile, App_Web_1rnu513f" enableEventValidation="false" stylesheettheme="Default" %>
<%@ Register Src="~/UserControls/SelectedControl.ascx" TagName="SelectedControl" TagPrefix="uc1" %>
<%@ Register Src="FormField.ascx" TagName="FormField" TagPrefix="uc" %>
<%@ Register Assembly="eWorld.UI" Namespace="eWorld.UI" TagPrefix="ew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ccuctf" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="PageLabelContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <style type="text/css">
        .fieldTable>tbody>tr>td:nth-child(1)
        {
            width:50%;
        }
        .formLabel
        {
            font-weight:bold;
        }
        .createUserForm
        {
            margin-left:auto;
            margin-right:auto;
        }
    </style>
    <script  type="text/javascript">
        function AnswerIsYes(rblId) {
            if (document.getElementById(rblId) != null) {
                var oElem = document.getElementById(rblId);
                var radio = oElem.getElementsByTagName("input");
                return radio[0].checked;
            }
            return false;
        }

        function TogglePanel(rbl1_Id, pnl, pnl2) {
            if (AnswerIsYes(rbl1_Id)) {
                document.getElementById(pnl).style.display = "block";
                document.getElementById(pnl2).style.display = "none";
            }
            else {
                document.getElementById(pnl).style.display = "none";
                document.getElementById(pnl2).style.display = "block";
            }
        }
     </script>
     <table class="fieldTable">
        <tr>
            <td><asp:Label AssociatedControlID="ProviderCategoryType" ID="lblComboBox" runat="server" CssClass="formLabel300" />Provider Category Type</td>
            <td><asp:DropDownList ID="ProviderCategoryType" AutoPostBack="false" runat="server" AppendDataBoundItems="true"></asp:DropDownList></td>
        </tr>
    </table>
     <table class="fieldTable">
        <tr>
            <td><asp:Label AssociatedControlID="ProviderType" ID="Label1" runat="server" CssClass="formLabel300" />Provider Type</td>
            <td><asp:DropDownList ID="ProviderType" AutoPostBack="false" runat="server" AppendDataBoundItems="true"></asp:DropDownList></td>
        </tr>
    </table>

</asp:Content>