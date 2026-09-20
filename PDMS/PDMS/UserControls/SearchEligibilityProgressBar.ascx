<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_SearchEligibilityProgressBarascx" Codebehind="SearchEligibilityProgressBar.ascx.cs" %>

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
        display: inline-block;
    }


    .needAttentionbg .rtbIcon {
        /*/*background :url('../Images/bullet-red.png') no-repeat;*/ */ background-position: 100% 0 !important;
        padding-right: 10px;
        content: '';
    }

    .completebg .rtbIcon {
        /*background :url('../Images/StepCheck.png') no-repeat;*/
        background-position: 100% 0 !important;
        content: '';
        padding-right: 20px;
    }

    .inProcessbg .rtbIcon {
        background: url('../Images/InProcess.png') no-repeat;
        background-position: 100% 0 !important;
        content: '';
        padding-right: 20px;
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
</style>


<script type="text/javascript">
    function OnClientLoad() {
        var toolBar = $find("<%= rrMenu.ClientID %>");

        var currentItem = document.getElementById("<%=hndCurrentItem.ClientID %>").value;
        if (currentItem != "") {
            var toolbarbutton = toolBar.findItemByValue(currentItem);
            toolbarbutton.focus();
        }

        var $elem = $('#divsearchEliginilityScroll');
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


    function ResizeRotator() {
        // the to dynamically set the size of the radrotator
        var windowSize = DivsearchEliginilityProgressBar.offsetWidth;
        // the available width is calculated and stored in a hidden field
        var availableSpace = windowSize;

        divsearchEliginilityScroll.offsetWidth = availableSpace - 16;
        return divsearchEliginilityScroll.offsetWidth;
    }
    function OnClientLoadHandler(sender) {
        sender.get_inputDomElement().readOnly = "readonly";
    }
    function ItemClick(id) {
        setCookie("selectedOption", id, 1);
		if (id == '10006') {
			setCookie("showResults", "false", 1);
        }
        if (id == '10017') {
            setCookie("showResults", "false", 1);
        }
    }
    function setCookie(c_name, value, exdays) {
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + exdays);
        var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
        document.cookie = c_name + "=" + c_value;
    }
    $(document).ready(function () {
        var $table = $(".RadComboBox").children("table");
        $table.removeAttr("summary");
        $table.attr("role", "presentation");

        // Get all elements with class 'rtbWrap'
        var rtbWrapElements = document.getElementsByClassName('rtbWrap');

        // Iterate through the elements
        for (var i = 0; i < rtbWrapElements.length; i++) {
            var rtbWrap = rtbWrapElements[i];

            // Check if the innerHTML of the 'rtbtext' element is empty
            var rtbTextElement = rtbWrap.querySelector('.rtbText');
            if (rtbTextElement.innerHTML.trim() === '') {

                // Remove the 'href' attribute from the 'rtbWrap' element
                rtbWrap.removeAttribute('href');
            }
        }
    });
</script>
<div id="DivsearchEliginilityProgressBar" class="searchEliginilityprogressbar">
    <div class="container">
        <div class="row">
            <div class="col-md-5 col-sm-5 text-right">
                <asp:Label ID="lblJumpTo" runat="server" CssClass="formLabel150" Text="Jump To:" />
            </div>


            <div class="col-md-6 col-sm-6">
                <telerik:RadComboBox
                    ID="RadJumpTo" runat="server" Width="450px"
                    MarkFirstMatch="true" EnableLoadOnDemand="true" EnableEmbeddedSkins="false" Skin="PDMSModern"
                    HighlightTemplatedItems="true" OnClientLoad="OnClientLoadHandler" OnClientItemsRequested="UpdateItemCountField" OnSelectedIndexChanged="RadJumpTo_SelectedIndexChanged"
                    OnDataBound="RadJumpTo_DataBound" OnItemDataBound="RadJumpTo_ItemDataBound" AutoPostBack="true" EnableAriaSupport="true" AriaSettings-Label="Jump To">

                    <%--<HeaderTemplate>

                        <ul>
                            <li class="col1">Section Name</li>
                           
                            <li class="col3">Status</li>
                        </ul>
                    </HeaderTemplate>--%>


                    <ItemTemplate>


                        <ul>

                            <li class="col1">

                                <%# DataBinder.Eval(Container.DataItem, "Text") %></li>



                            <%--<li class="col3" style="text-align:center;">

                                <asp:Image ImageUrl='<%# DataBinder.Eval(Container.DataItem, "Status") %>' ID="ImageStatus" runat="server" />--%>
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
    <div id="divsearchEliginilityScroll" class="progressScroll">
        <telerik:RadToolBar ID="rrMenu" runat="server" OnButtonClick="rrMenu_ButtonClick" AutoPostBack="true"
            Skin="PDMSModern" CausesValidation="false" EnableEmbeddedSkins="false" OnClientLoad="OnClientLoad">
        </telerik:RadToolBar>
    </div>

    <asp:HiddenField ID="hndCurrentItem" runat="server" />

</div>



<telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">

    <script type="text/javascript">

        function UpdateItemCountField(sender, args) {

            //Set the footer text.

            sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " items";

        }

    </script>

</telerik:RadScriptBlock>

