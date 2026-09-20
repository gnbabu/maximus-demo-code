<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_StandaloneUploadAttachments, App_Web_rnw0hezi" enableEventValidation="false" stylesheettheme="Default" %>

<%--<%@ MasterType TypeName="MasterWorkflowPage" %>--%>
<%@ PreviousPageType TypeName="RegistrationProvider" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/SelfServiceProgressBar.ascx" TagPrefix="uc7" TagName="SearchEligibilityProgressBar" %>
<%@ Register Src="~/PopupControls/UploadAttachments.ascx" TagPrefix="uc1" TagName="UploadAttachments" %>



<%--<%@ Register Src="~/PopupControls/HospiceEnrollSearch.ascx.cs" TagPrefix="uc8" TagName="HospiceEnrollSearch" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
    <div style="text-align: left !important"></div>
    <!--  <asp:Label id="lblPgTitle" runat="server" CssClass="pageHeader" />-->

</asp:Content>
<asp:Content ID="AUTH" ContentPlaceHolderID="MainContent" runat="Server">

    <%--    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
    <script type="text/javascript">
        $(document).ready(function () {

        });
        function CheckPhoneLength(sender, args) {
            var re = /\D/g; // Remove any characters that are not numbers
            var test = args.Value.replace(re, "");
            if (test == "") return true;

            var len = test.length;
            if (len != 10)
                args.IsValid = false;
            if (test[0] == 0 || test[0] == 1 || test[3] == 0 || test[3] == 1)
                args.IsValid = false;
            return;
        }

        function CheckSSNLength(sender, args) {
            var re = /\D/g; // Remove any characters that are not numbers
            var test = args.Value.replace(re, "");
            if (test == "") return true;

            var len = test.length;
            if (len != 9)
                args.IsValid = false;
            return;
        }

        var popUp;
        function OnClientShow(sender, eventArgs) {
            var myWidth = 0, myHeight = 0;
            if (typeof (window.innerWidth) == 'number') {

                //Non-IE
                myWidth = window.innerWidth;
                myHeight = window.innerHeight;
            } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                //IE 6+ in 'standards compliant mode'
                myWidth = document.documentElement.clientWidth;
                myHeight = document.documentElement.clientHeight;
            } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                //IE 4 compatible
                myWidth = document.body.clientWidth;
                myHeight = document.body.clientHeight;
            }

            popUp = eventArgs.get_popUp();
            var gridWidth = myWidth;
            var gridHeight = myHeight;
            var popUpWidth = popUp.style.width.substr(0, popUp.style.width.indexOf("px"));
            var popUpHeight = popUp.style.height.substr(0, popUp.style.height.indexOf("px"));
            popUp.style.left = ((gridWidth - popUpWidth) / 2) + "px";
            popUp.style.top = ((gridHeight - popUpHeight) / 2) + "px";
        }

      <%--  $(document).keypress(function (e) {
            if (e.which == 13) {
                $('#<%=ucRegistrationNavigation.FindControl("btnSave").ClientID%>').click();
                event.keyCode = 0;
            }--%>
        //});

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }


    </script>
    <style type="text/css">
        .divGrid {
            width: 100%;
        }

        .gridview {
            float: right;
        }

        .gridViewHeader > th > a {
            color: White;
        }

        .panelOwnerInfo {
            padding: 20px 20px 0px 20px;
        }

        td, th {
            padding: 4px !important;
        }

        .row {
            margin-top: 2px;
            margin-bottom: 2px;
        }

        .col-sm-3 {
            width: 23%;
        }

        .textAlignCentre {
            text-align: center;
        }

        .radioButtonList {
            list-style: none;
            margin: 0;
            padding: 0;
        }

            .radioButtonList.horizontal li {
                display: inline;
            }

            .radioButtonList label {
                display: inline;
            }

        .ph2 {
            padding-left: 10px;
            color: white;
        }
    </style>

    <div class="WhiteBox">
        <asp:Panel ID="pnlBillingAndotherservice" runat="server">
            <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="true" />
            <div class="row">
                <asp:HiddenField ID="HasUnsavedDataPA" runat="server" />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                        <asp:PostBackTrigger ControlID="ucRegProgressBar" />
                    </Triggers>
                    <ContentTemplate>
                        <div id="DivProgressBar" runat="server">
                            <uc7:SearchEligibilityProgressBar ID="ucRegProgressBar" runat="server" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <br />
            <br />


            <asp:PlaceHolder ID="sectionPH" runat="server" EnableViewState="false" Visible="true" />


            <div id="divUploadAttachments" runat="server">
                <div class="enrollment">
                </div>
                <asp:Panel ID="pnlUploadAttachments" runat="server">
                    <uc1:UploadAttachments runat="server" ID="UploadAttachments" />
                </asp:Panel>
            </div>
            <asp:HiddenField ID="RegIdTxt" runat="server" />
        </asp:Panel>
    </div>



</asp:Content>