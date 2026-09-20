<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_RegistrationProgressBar" Codebehind="RegistrationProgressBar.ascx.cs" %>

<style type="text/css">
    .RadComboBox_PDMSModern, .RadComboBox_PDMSModern .rcbInputCell .rcbInput, .RadComboBoxDropDown_PDMSModern {
        font: 18px arial, verdana, sans-serif;
    }

    .RadComboBoxDropDown_PDMSModern {
        font: 16px;
    }

    .rcbHeader ul,
    .rcbFooter ul,
    .rcbItem ul,
    .rcbHovered ul,
    .rcbDisabled ul {
        margin: 0;
        padding: 0;
        width: 100%;
        display: inline-block;
        list-style-type: none;
    }

    .rcbScroll {
        overflow: scroll !important;
        overflow-x: hidden !important;
    }

    .col2,
    .col3 {
        margin: 0;
        padding: 0 5px 0 0;
        width: 20%;
        line-height: 14px;
        float: left;
    }

    .col1 {
        width: 80%;
        margin: 0;
        padding: 0 5px 0 0;
        line-height: 14px;
        float: left;
        display:inline-block;
    }
     
     
    .needAttentionbg .rtbIcon
    {
       
        background :url('../Images/bullet-red.png') no-repeat;
       background-position: 100% 0 !important;
        padding-right:10px;
        content: '';
    }
    .completebg .rtbIcon
    {
        background :url('../Images/StepCheck.png') no-repeat;
        background-position: 100% 0 !important;
        content: '';
        padding-right:20px;

    }
    .inProcessbg .rtbIcon
    {
        background :url('../Images/InProcess.png') no-repeat;
        background-position: 100% 0 !important;
        content: '';
        padding-right:20px;
    }

    .progressScroll {
        margin-left: 8px;
        margin-right: 8px;
        overflow-y: hidden;
        overflow-x: scroll;
        scrollbar-face-color: #d2d2d2;
        scrollbar-highlight-color: #ebebeb;
        scrollbar-3dlight-color: #ebebeb;
        scrollbar-shadow-color: #d2d2d2;
        scrollbar-darkshadow-color: #000000;
        scrollbar-track-color: #e3e3f3;
        scrollbar-arrow-color: #545487;
    }
    .hidden-alt-text {
       visibility: hidden;
    }

</style>

<script type="text/javascript">
    function OnClientLoad() {
        var toolBar = $find("<%= rrMenu.ClientID %>");

          var currentItem = document.getElementById("<%=hndCurrentItem.ClientID %>").value;
          if (currentItem != "") {
              var toolbarbutton = toolBar.findItemByValue(currentItem);
              toolbarbutton.focus();
          }

          var $elem = $('#divScroll');
          var newScrollLeft = $elem.scrollLeft(),
           width = $elem.outerWidth(),
           scrollWidth = $elem.get(0).scrollWidth;
          if (scrollWidth - newScrollLeft - 100 <= width) {
              $('[id*=btnRight]').css('opacity', '0.5');

          }
          if (newScrollLeft == 0) {
              $('[id*=btnLeft]').css('opacity', '0.5');
          }
    }
    var onLeave = false;
    function OnClientButtonClicked(sender, args) {
        if (onLeave != 'true') {
            var button = args.get_item();
            var toolBar = $find("<%= rrMenu.ClientID %>");
            //alert('ToolBar: ' + toolBar);
            var currentItem = document.getElementById("<%=hndCurrentItem.ClientID %>").value;
            //alert('currentItem: ' + currentItem);
            if (currentItem != "") {
                var toolbarbutton = toolBar.findItemByValue(currentItem);
                if (button.get_text() != toolbarbutton.get_text()) {
                    BeforeLeavingBar(args);
                }
            }
        }
    }

    function BeforeLeavingBar(args) {
        var authenticated = '<%=HttpContext.Current.User.Identity.IsAuthenticated%>';
        if (authenticated == 'True') {
            var isProvAdmin = '<%=Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRole.ProviderAdministrator)%>';
            var isProvAgent = '<%=Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRole.ProviderAgent)%>';
            var storage = window.sessionStorage;
            var isSpecialityChanged = storage.getItem("isSpecialityChanged");
            var isSpecChanged;
            if (isSpecialityChanged != null && isSpecialityChanged != undefined && isSpecialityChanged != '' && isSpecialityChanged == 'True') {
                $('#ctl00_MainContent_HasUnsavedData').val('True');
            }
            
            //alert('HasUnsavedData: ' + $('#ctl00_MainContent_HasUnsavedData').val());
            if ($('#ctl00_MainContent_HasUnsavedData').val() == 'True' && (isProvAdmin == 'True' || isProvAgent == 'True')) {
                //if (event) event.preventDefault ? event.preventDefault() : event.returnValue = false;
                var dialogObj = $("#alertLeave");
                var dialog = dialogObj.dialog({
                    modal: true,
                    title: 'ALERT - Update not submitted',
                    width: '600px',
                    buttons: {
                        'Ok': function () {

                            var lclStorage = window.sessionStorage;
                            lclStorage.removeItem("isSpecialityChanged");

                            $(this).dialog('close');
                            //alert(args.get_item());
                            var button = args.get_item();
                            onLeave = "true";
                            button.click();
                        },
                        'Cancel': function () {
                            $(this).dialog('close');
                            args.set_cancel(true);

                            var toolBar = $find("<%= rrMenu.ClientID %>");
                            var currentItem = document.getElementById("<%=hndCurrentItem.ClientID %>").value;
                            if (currentItem != "") {
                                var toolbarbutton = toolBar.findItemByValue(currentItem);
                                toolbarbutton.focus();
                            }
                        }
                    },
                    open: function () {
                        $("#alertLeave").dialog('open');
                        args.set_cancel(true);
                    }
                });
            }
            else {
                args.set_cancel(false);
                return true;
            }
        }
        else {
            args.set_cancel(false);
            return true;
        }
    }

    function OnClientSelectionChanged(sender, args) {
        var button = args.get_item();
        var toolBar = $find("<%= rrMenu.ClientID %>");
        //alert('ToolBar: ' + toolBar);
        var currentItem = document.getElementById("<%=hndCurrentItem.ClientID %>").value;
        //alert('currentItem: ' + currentItem);
        if (currentItem != "") {
            var toolbarbutton = toolBar.findItemByValue(currentItem);
            if (button.get_text() != toolbarbutton.get_text()) {
                BeforeLeavingCombo(args);
            }
        }
    }

    function BeforeLeavingCombo(args) {
        var authenticated = '<%=HttpContext.Current.User.Identity.IsAuthenticated%>';
        if (authenticated == 'True') {
            var isProvAdmin = '<%=Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRole.ProviderAdministrator)%>';
            var isProvAgent = '<%=Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRole.ProviderAgent)%>';
            var storage = window.sessionStorage;
            var isSpecialityChanged = storage.getItem("isSpecialityChanged");
            var isSpecChanged;
            if (isSpecialityChanged != null && isSpecialityChanged != undefined && isSpecialityChanged != '' && isSpecialityChanged == 'True') {
                $('#ctl00_MainContent_HasUnsavedData').val('True');
            }
            
            //alert('HasUnsavedData: ' + $('#ctl00_MainContent_HasUnsavedData').val());
            if ($('#ctl00_MainContent_HasUnsavedData').val() == 'True' && (isProvAdmin == 'True' || isProvAgent == 'True')) {
                //if (event) event.preventDefault ? event.preventDefault() : event.returnValue = false;
                var dialogObj = $("#alertLeave");
                var dialog = dialogObj.dialog({
                    modal: true,
                    title: 'ALERT - Update not submitted',
                    width: '600px',
                    buttons: {
                        'Ok': function () {
                            var lclStorage = window.sessionStorage;
                            lclStorage.removeItem("isSpecialityChanged");

                            $(this).dialog('close');
                            //alert(args.get_item());
                            var button = args.get_item();
                            onLeave = "true";
                            //button.click();
                            __doPostBack("RadJumpTo", '{\"Command\" : \"Select\"}');
                        },
                        'Cancel': function () {
                            $(this).dialog('close');
                            args.set_cancel(true);

                            var toolBar = $find("<%= rrMenu.ClientID %>");
                            var currentItem = document.getElementById("<%=hndCurrentItem.ClientID %>").value;
                            if (currentItem != "") {
                                var toolbarbutton = toolBar.findItemByValue(currentItem);
                                toolbarbutton.focus();
                            }
                        }
                    },
                    open: function () {
                        $("#alertLeave").dialog('open');
                        args.set_cancel(true);
                    }
                });
            }
            else {
                __doPostBack("RadJumpTo", '{\"Command\" : \"Select\"}');
            }
        }
        else {
            __doPostBack("RadJumpTo", '{\"Command\" : \"Select\"}');
        }
    }

      function ResizeRotator() {
          // the to dynamically set the size of the radrotator
          var windowSize = DivProgressBar.offsetWidth;
          // the available width is calculated and stored in a hidden field
          var availableSpace = windowSize;

          divScroll.offsetWidth = availableSpace - 16;
          return divScroll.offsetWidth;
      }

    $(document).ready(function () {
        var $table = $(".RadComboBox").children("table");
        $table.removeAttr("summary");
        $table.attr("role", "presentation");
    });
</script>


<div id="DivProgressBar" class="progressbar">
    <div class="container">
        <div class="row">
            <div class="col-md-5 col-sm-5 text-right">
                <asp:Label ID="lblJumpTo" runat="server" CssClass="formLabel150" Text="Jump To:" />
            </div>
            <div class="col-md-6 col-sm-6">
                <telerik:RadComboBox ID="RadJumpTo" runat="server" Width="450px"  EnableAriaSupport="true" AriaSettings-Label="Jump To"
                    MarkFirstMatch="true" EnableLoadOnDemand="true" EnableEmbeddedSkins="false" Skin="PDMSModern"
                    HighlightTemplatedItems="true" OnClientItemsRequested="UpdateItemCountField" OnSelectedIndexChanged="RadJumpTo_SelectedIndexChanged" 
                    OnDataBound="RadJumpTo_DataBound" OnItemDataBound="RadJumpTo_ItemDataBound" AutoPostBack="true" OnClientSelectedIndexChanged="OnClientSelectionChanged">

                    <HeaderTemplate>

                        <ul>
                            <li class="col1">Section Name</li>
                           <%-- <li class="col2">Icon</li>--%>
                            <li class="col3">Status</li>
                        </ul>

                    </HeaderTemplate>


                    <ItemTemplate>
                        
                        <ul>

                            <li class="col1">

                             <%# DataBinder.Eval(Container.DataItem, "Text") %></li>

                            <%--<li class="col2">

                                <asp:Image ImageUrl='<%# DataBinder.Eval(Container.DataItem, "Icon") %>' ID="imageIcon" runat="server" /></li>--%>

                            <li class="col3" style="text-align:center;">

                                <asp:Image ImageUrl='<%# DataBinder.Eval(Container.DataItem, "Status") %>' ID="ImageStatus" AlternateText="status" runat="server" />
                            </li>

                        </ul>
                    </ItemTemplate>

                    <FooterTemplate>
                        A total of

                <asp:Literal runat="server" ID="RadComboItemsCount" />

                        items

                    </FooterTemplate>
                </telerik:RadComboBox>
            </div>
        </div>
    </div>
    <%--<div class="rightArrow">
        <asp:Image ImageUrl="~/Images/right.png" ID="btnRight" AlternateText="right" runat="server" />
    </div>
    <div class="leftArrow">
        <asp:Image ImageUrl="~/Images/left.png" ID="btnLeft" AlternateText="left" runat="server" />
    </div>--%>
    <div id="divScroll" class="progressScroll">
        <telerik:RadToolBar ID="rrMenu" runat="server" OnButtonClick="rrMenu_ButtonClick" OnClientButtonClicking="OnClientButtonClicked"
            Skin="PDMSModern" CausesValidation="false" EnableEmbeddedSkins="false" OnClientLoad="OnClientLoad">
        </telerik:RadToolBar>
    </div>

    <asp:HiddenField ID="hndCurrentItem" runat="server" />

</div>

<%--<script type="text/javascript">

    $(function () {

        var scrollLeft = 100;

        $('[id*=btnLeft]').click(function () {
            if (scrollLeft >= 100) {
                scrollLeft = $("#divScroll")[0].scrollLeft - 100;
            }
            else {
                scrollLeft = 0;
            }
            $("#divScroll").scrollLeft(scrollLeft);

            if (scrollLeft == 0)
                this.style.opacity = '0.5';


            $('[id*=btnRight]').css('opacity', '1');
            return false;
        });

        $('[id*=btnRight]').click(function () {
            //if (scrollLeft == 0) {
            //    scrollLeft = 50
            //}

            var $elem = $('#divScroll');
            var newScrollLeft = $elem.scrollLeft(),
             width = $elem.outerWidth(),
             scrollWidth = $elem.get(0).scrollWidth;
            if (scrollWidth - newScrollLeft == width) {
                this.style.opacity = '0.5';
            }
            else {

                scrollLeft = $("#divScroll")[0].scrollLeft + 100;
                $("#divScroll").scrollLeft(scrollLeft);
            }
            $('[id*=btnLeft]').css('opacity', '1');
            return false;
        });
    });
</script>--%>

<telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">

    <script type="text/javascript">

        function UpdateItemCountField(sender, args) {

            //Set the footer text.

            sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " items";

        }

    </script>

</telerik:RadScriptBlock>
