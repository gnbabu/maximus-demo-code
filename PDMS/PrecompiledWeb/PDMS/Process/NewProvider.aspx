<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_NewProvider, App_Web_unbhbgmw" enableEventValidation="false" stylesheettheme="Default" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/ProviderManagementView.ascx" TagName="ProviderManagementView" TagPrefix="viewDetail" %>



<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript">
        function checkDate(sender, args) {
            if (sender._selectedDate > new Date()) {
                alert("You cannot select a future date!");
                sender._selectedDate = new Date();
                // set the date back to the current date
                sender._textbox.set_Value(sender._selectedDate.format(sender._format))
            }
        }

        function clientSideFunction() {
            window.open('https://dam.assets.ohio.gov/image/upload/medicaid.ohio.gov/Providers/Enrollment%20and%20Support/National-Provider-Identifier-Reference-Guide.pdf','NPI');
        }

    </script>

    <style>
        /*body {
            /*background-color:#eee;
            background-color:lightyellow;
        }
        .panel-clr {
            background-color: transparent;
        }

            .panel-clr.on {
                background-color: greenyellow;
            }*/

        .panel-info {
            border-color: #41719C;
        }

        .panel-heading {
            color: #fff !important;
            border-color: #41719C;
            font-size: 18pt;
        }

        .panelButton {
            width: 50%;
            color: #fff;
            font-weight: bold;
            border-radius: 6px;
            border: 0px;
            height: 40px;
        }

            .panelButton:hover {
                cursor: pointer;
                opacity: 0.5;
            }


        .row .col-sm-3:nth-child(1) .panel .panel-heading {
            background-color: #545487;
        }

        .row .col-sm-3:nth-child(1) .panel .panel-footer {
            background-color: #DCDDEB;
        }

        .row .col-sm-3:nth-child(1) .panelButton {
            background-color: #545487;
        }

        .row .col-sm-3:nth-child(2) .panel .panel-heading {
            background-color: #4E7B2F;
        }

        .row .col-sm-3:nth-child(2) .panel .panel-footer {
            background-color: #FDF0E4;
        }

        .row .col-sm-3:nth-child(2) .panel .panel-footer {
            background-color: #D9ECCD;
        }

        .row .col-sm-3:nth-child(2) .panelButton {
            background-color: #4E7B2F;
        }

        .row .col-sm-3:nth-child(3) .panel .panel-heading {
            background-color: #CA5C28;
        }

        .row .col-sm-3:nth-child(3) .panel .panel-footer {
            background-color: #FDF0E4;
        }

        .row .col-sm-3:nth-child(3) .panelButton {
            background-color: #CA5C28;
        }

        .row .col-sm-3:nth-child(4) .panel .panel-heading {
            background-color: #6B2F53;
        }

        .row .col-sm-3:nth-child(4) .panel .panel-footer {
            background-color: #FFE5EF;
        }

        .row .col-sm-3:nth-child(4) .panelButton {
            background-color: #6B2F53;
        }

        .tooltiptextAppType {
            visibility: hidden;
            width: auto;
            height: auto;
            background-color: lightyellow;
            color: black;
            text-align: center;
            border-radius: 6px;
            padding: 5px 0;
            /* Position the tooltip */
            position: absolute;
            z-index: 1;
        }

        .tooltipAppType:hover .tooltiptextAppType {
            visibility: visible;
        }
        .panelButton1 {
            width: 50%;
            color: #fff;
            font-weight: bold;
            border-radius: 6px;
            border: 0px;
            height: 40px;
            background-color: gray;
        }
    </style>
    <script>
        function HideAndShowPopup() {
            var workflowEventTypeId = document.getElementById('<%=workflowEventTypeId_hidden.ClientID %>').value;
            var id = document.getElementById('<%=txtWaiverTypeID.ClientID %>').value;
            if (id == 1) {
                document.getElementById('<%=btnSaveSubmitCancel.ClientID %>').click();
            }
            else {
                document.getElementById('<%=btnSaveSubmitCancel.ClientID %>').click();
                // if this is a NEW registration, let them know they are being passed to sister agency
                if (workflowEventTypeId == 1) {
                    document.getElementById('<%=btnSubmitMessagePopup.ClientID %>').click();
                }
            }
        }

        $("#applicationType").hide();
        setApplicationTypeChangeEditability();
        /* DisableApplicationTypeTile();*/
        function mevent(e) {


            var left = (e.clientX - 90) + "px";
            document.getElementById("helpTaxIDInfo").style.left = "140px";
            document.getElementById("helpTaxIDInfo").style.visibility = "visible";
            document.getElementById("helpTaxIDInfo").style.display = "block";


        }
        function meventleave() {

            document.getElementById("helpTaxIDInfo").style.visibility = "hidden";
        }


        $(function () {
            $(".help-taxid").mouseover(function () {
                // .position() uses position relative to the offset parent, 
                var pos = $(this).position();
                // .outerWidth() takes into account border and padding.
                var width = $(this).outerWidth();
                //show the menu directly over the placeholder
                $("#helpTaxIDInfo").css({
                    position: "absolute",
                    top: pos.top + "px",
                    left: (pos.left + width) + "px"
                }).show();
            });
            $("#helpTaxIDInfo").mouseleave(function () {
                $("#helpTaxIDInfo").hide();
            });

            $(".panel-clr").removeClass('on');
            $(".glyphicon").removeClass('glyphicon-ok');
            $("#CatDetails").hide();
            $("#ProvDetails").hide();


            var $appType = $("#<%= txtApplicationType.ClientID %>").val();

            if (undefined !== $appType) {
                if ($appType.length > 0) {
                    //checks if application type is selected
                    var $selAppType = $('div:contains("' + $appType + '")');
                    $selAppType.toggleClass('on');
                    if (!$selAppType.children('span').hasClass('glyphicon-ok')) {
                        $selAppType.children('span').addClass('glyphicon-ok');
                    }

                    //show disabled application textbox and category picker
                    $("#CatDetails").show();
                }
                else {
                    //show panel for application selector
                    $("#applicationType").show();

                    var $IsWaiver = $("#<%= hdnIsWaiver.ClientID %>").val();
                    if ($IsWaiver == "True") {

                        toggle($("#applicationTypeWaiver"));

                        $("#divapplicationTypeWaiver").show();
                    }

                    //if ($IsWaiver == "More") {

                    //    toggle($("#applicationTypeMore"));

                    //    $("#divapplicationTypeMore").show();
                    //}
                }
            }

            var $catType = $("#<%= txtCategory.ClientID %>").val();
            // alert($("#<%= txtCategory.ClientID %>").val());

            if (undefined !== $catType) {
                if ($catType.length > 0) {

                    var $selCatType = $('button:contains("' + $catType + '")');

                    if (!$selCatType.children('span').hasClass('glyphicon-ok')) {
                        $selCatType.children('span').addClass('glyphicon-ok');
                    }
                    //show the rest of the form
                    $("#ProvDetails").show();

                }
                else {
                    //show category panel
                    $("#trCategoryType").show();
                }
            }
        });

        $(function () {
            $('.panel-click').click(function () {

                var $this = $(this);

                $(".panel-clr").removeClass('on');
                $(".glyphicon").removeClass('glyphicon-ok');

                $this.find('div').toggleClass('on');

                if (!$this.find('span').hasClass('glyphicon-ok')) {
                    $this.find('span').addClass('glyphicon-ok');
                } else {
                    $this.find('span').removeClass('glyphicon-ok');
                }
                $("#<%= txtApplicationTypeID.ClientID %>").val($this.find('h3').text().trim());
                //__doPostBack('#<%= txtApplicationTypeID.ClientID %>', 'TextChanged');
                $("#<%= txtWaiverTypeID.ClientID %>").val($this.find('h2').text().trim());
                //__doPostBack('#<%= txtWaiverTypeID.ClientID %>', 'TextChanged');
                $("#<%= hdntxtWaiverType.ClientID %>").val($this.find('h2').text().trim());
                var postbackArray = ["'#<%= txtApplicationTypeID.ClientID %>'", "'#<%= txtWaiverTypeID.ClientID %>'"];
                var arrayLength = postbackArray.length;
                for (var i = 0; i < arrayLength; i++) {
                    __doPostBack(postbackArray[i], 'TextChanged');
                }
            })
        });

        $(function () {
            $('.buttonCategory').click(function () {
                var $this = $(this);
                $(this).parent("div").find(".glyphicon").removeClass('glyphicon-ok');
                if (!$this.find('span').hasClass('glyphicon-ok')) {
                    $this.find('span').addClass('glyphicon-ok');
                } else {
                    $this.find('span').removeClass('glyphicon-ok');
                }


                $("#<%= txtCategoryID.ClientID %>").val($this.find('h3').text().trim());
                __doPostBack('#<%= txtCategoryID.ClientID %>', 'TextChanged');

            })
        });
        function toggleCategory(lnk_obj) {
            $("#ProvDetails").hide();//OHPNM-3290 pschwarz hide when change by default
        }
        function toggleApplicationType(lnk_obj) {
            var $IsWaiver = $("#<%= hdnIsWaiver.ClientID %>").val();
            $("#applicationType").show();
            $("#ProvDetails").hide();//OHPNM-3290 pschwarz hide when change by default
            $("#CatDetails").hide();//OHPNM-3290 pschwarz hide when change by default

            if ($IsWaiver == "True") {
                //$("#applicationTypeMore").text('Less...');
                //$("#divapplicationTypeMore").show();
                $("#applicationTypeWaiver").text('Less...');
                $("#divapplicationTypeWaiver").show();
            }

            //if ($IsWaiver == "More") {
            //    $("#applicationTypeMore").text('Less...');
            //    $("#divapplicationTypeMore").show();
            //}
        }
        function toggle(lnk_obj) {
            if (lnk_obj.innerHTML == 'Less...') {
                lnk_obj.innerHTML = 'Click here for more application types...';
                $("#div" + lnk_obj.id).hide();
            }
            else {
                lnk_obj.innerHTML = 'Less...';
                $("#div" + lnk_obj.id).show();
            }
        }

        function setApplicationTypeChangeEditability() {
            var isLinkProvider = "<%=this.IsLinkProvider%>";
            var KeyFieldEditRequest = "<%=this.KeyFieldEditRequest%>";
            var isAddODM = "<%=this.IsAddODMorODAMedicaid%>";
            if (isLinkProvider == "True" || KeyFieldEditRequest == "True") {
                $("#lnkApplicationType").prop('disabled', true);
                $("#lnkApplicationType").css('color', '#DCDDEB');
            }
            else {
                $("#lnkApplicationType").prop('disabled', false);
                $("#lnkApplicationType").css('color', 'blue');
            }

            if (isLinkProvider == "True" || isAddODM == "True") {
                $("#lnkCategoryType").prop('disabled', true);
                $("#lnkCategoryType").css('color', '#DCDDEB');
            }
            else {
                $("#lnkCategoryType").prop('disabled', false);
                $("#lnkCategoryType").css('color', 'blue');
            }
            $('.buttonCategory').click(function () {
                var $this = $(this);
                $(this).parent("div").find(".glyphicon").removeClass('glyphicon-ok');
                if (!$this.find('span').hasClass('glyphicon-ok')) {
                    $this.find('span').addClass('glyphicon-ok');
                } else {
                    $this.find('span').removeClass('glyphicon-ok');
                }

                $("#<%= txtCategoryID.ClientID %>").val($this.find('h3').text().trim());
                __doPostBack('#<%= txtCategoryID.ClientID %>', 'TextChanged');

            })

            $('.panel-click').click(function () {

                var $this = $(this);

                $(".panel-clr").removeClass('on');
                $(".glyphicon").removeClass('glyphicon-ok');

                $this.find('div').toggleClass('on');

                if (!$this.find('span').hasClass('glyphicon-ok')) {
                    $this.find('span').addClass('glyphicon-ok');
                } else {
                    $this.find('span').removeClass('glyphicon-ok');
                }
                $("#<%= txtApplicationTypeID.ClientID %>").val($this.find('h3').text().trim());
                //__doPostBack('#<%= txtApplicationTypeID.ClientID %>', 'TextChanged');
                $("#<%= txtWaiverTypeID.ClientID %>").val($this.find('h2').text().trim());
                //__doPostBack('#<%= txtWaiverTypeID.ClientID %>', 'TextChanged');
                $("#<%= hdntxtWaiverType.ClientID %>").val($this.find('h2').text().trim());
                var postbackArray = ["'#<%= txtApplicationTypeID.ClientID %>'", "'#<%= txtWaiverTypeID.ClientID %>'"];
                var arrayLength = postbackArray.length;
                for (var i = 0; i < arrayLength; i++) {
                    __doPostBack(postbackArray[i], 'TextChanged');
                }
            })
            
        }
        <%--function DisableApplicationTypeTile() {
            var isAddMedSvc = "<%=this.IsAddODMorODAMedicaid%>";
            var appType = $(this).find('h3').text().trim();
            if (isAddMedSvc == "True") {
                /*if (appTypeID == "4") {*/
                $(this).find('divrptAppItem').removeClass();
                /*document.getElementById("divrptAppItem").addClass('panel panel-info');  */                 
                $(this).find('divrptAppItem').css('background-color', 'gray');
                $(this).find('SelectAppType').prop('disabled', true);
                /*document.getElementById("SelectAppType").disabled = true;*/
                /*}*/
            }
        } --%>

    </script>
    <asp:UpdatePanel ID="upAddNew" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="divNewProvider" runat="server">
                <asp:Panel ID="pnlPage" runat="server" DefaultButton="btnSave" Style="padding-left: 4px; padding-right: 4px; padding-bottom: 4px; padding-top: 0px;">
                    <ajax:ModalPopupExtender ID="mpeKFEVerify" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy"
                        CancelControlID="btnKFECancel" BackgroundCssClass="modalBackground" Drag="false">
                    </ajax:ModalPopupExtender>
                    <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; top: 0px; padding: 0px; width: 360px;">
                        <asp:Panel ID="pnlHeader" CssClass="popHeader" runat="server">
                            <div class="popTitle">
                                Confirm Update
                            </div>
                        </asp:Panel>
                        <div style="padding: 10px;">
                            <div class="center">
                                <asp:Literal ID="ltlKFEConfirm" runat="server" Text="<%$ Resources:BrandingResource , KEY_FIELD_EDIT_DELETE_CONFIRMATION_MESSAGE %>" />
                            </div>
                            <div class="btnBox">
                                <asp:Button runat="server" ID="btnKFEYes" Text="Yes" CssClass="buttonBox" OnClick="btnKFEYes_Click" CausesValidation="false" />
                                <asp:Button runat="server" ID="btnKFECancel" Text="Cancel" CssClass="buttonBox" OnClick="btnKFECancel_Click" CausesValidation="false" />
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Button runat="server" ID="ButtonDummy" Style="display: none;" Text="ButtonDummy" />
                    <asp:Button runat="server" ID="ButtonDummyCred" Style="display: none;" Text="ButtonDummyCred" />
                    <ajax:ModalPopupExtender ID="mpeCredential" runat="server" PopupControlID="pnlModelCred" TargetControlID="ButtonDummyCred"
                        CancelControlID="btnCredCancel" BackgroundCssClass="modalBackground" Drag="false">
                    </ajax:ModalPopupExtender>
                    <asp:Panel ID="pnlModelCred" runat="server" CssClass="modalPopup" Style="display: none; top: 0px; padding: 0px; width: 360px;">
                        <asp:Panel ID="Panel2" CssClass="popHeader" runat="server">
                            <div class="popTitle">
                                Provider Credentialing
                            </div>
                        </asp:Panel>
                        <div style="padding: 10px;">
                            <div class="center">
                                <asp:Literal ID="ltlCredConfirm" runat="server" Text="Provider is not yet credentialed, click continue to enter into the Credentialing flow" />
                            </div>
                            <div class="btnBox">
                                <asp:Button runat="server" ID="btnCredYes" Text="Continue" CssClass="buttonBox" OnClick="btnCredYes_Click" CausesValidation="false" />
                                <asp:Button runat="server" ID="btnCredCancel" Text="Cancel" CssClass="buttonBox" OnClick="btnCredCancel_Click" CausesValidation="false" />
                            </div>
                        </div>
                    </asp:Panel>
                    <div>
                        <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
                        <asp:ValidationSummary ID="vsNewProvider" DisplayMode="List" runat="server" ValidationGroup="AddNewProvider" ShowSummary="true" CssClass="error-message"/>
                    </div>
                    <div class="pg-hint"></div>
                    <br />
                    <br />
                    <div id="div10DayMessage" style="width: 90%; margin: auto;" runat="server">
                        <p style="color: indigo; font-size: 24pt;">
                            <q>Please note that you have <span style="color: red">10 days to complete your application</span>. After 10 days, your information will be removed and you will
                        have to re-start the process from the beginning of the application.</q>

                        </p>
                    </div>
                    <div id="applicationType" class="collapse">
                        <div class="row flex" data-toggle="collapse" data-target="#applicationType">
                            <asp:Repeater ID="rptApplication" runat="server" OnItemDataBound="rptApplication_ItemDataBound">
                                <ItemTemplate>
                                    <%-- <div class="col-sm-3" style="margin-left: auto"/>
                                        <div class="panel panel-info panel-click"/>
                                    <div class="col-sm-3" style="margin-left: auto" />
                                        <div class="panel panel-info panel-click" />--%>
                                    
                                    <div class="col-sm-3 tooltipAppType" style="margin-left: auto">


                                        <div id="divrptAppItem" class="panel panel-info panel-click" runat="server">
                                            <div id="divHeading" class="panel-heading panel-clr" runat="server">
                                                <div class="formLabelAuto">
                                                    <%# Eval("APPLICATION_TYPE_NAME") %><span id="autohide-true" class="pull-right glyphicon" aria-hidden="true"></span>
                                                </div>
                                                <h3 class="collapse"><%# Eval("APPLICATION_TYPE_ID") %></h3>
                                            </div>
                                            <div class="panel-body">
                                                <p>
                                                    <%# Eval("APPLICATION_TYPE_DESC").ToString() %>
                                                </p>
                                            </div>
                                            <div id="divFooter" class="panel-footer" runat="server">
                                                <button type="button" class="panelButton" id="SelectAppType" runat="server">
                                                    Select
                                                </button>

                                                <span id="spninfo" runat="server" class="glyphicon glyphicon-info-sign"></span>

                                            </div>
                                            <span id="spn" data-apptype='<%# Eval("APPLICATION_TYPE_ID") %>' runat="server" class="tooltiptextAppType"></span>
                                        </div>

                                    </div>
                                        
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <a id="applicationTypeWaiver" onclick="javascript:toggle(this);"
                            title="Click here for more application types" class="btn btn-lg btn-default">Click here for more application types...</a>
                        <%--  <a id="applicationTypeMore" onclick="javascript:toggle(this);"
                            title="Click here for more application types" class="btn btn-lg btn-default">Click here for more application types...</a>--%>
                    </div>
                    <%--      <div id="divapplicationTypeMore" class="collapse">
                        <br />
                        <div class="row flex" data-toggle="collapse" data-target="#divapplicationTypeMore">
                            <asp:Repeater ID="rptApplicationMore" runat="server">
                                <ItemTemplate>
                                    <div class="col-sm-3">
                                        <div class="panel panel-info panel-click">
                                            <div class="panel-heading panel-clr">
                                                <div class="formLabelAuto">
                                                    <%# Eval("APPLICATION_TYPE_NAME") %><span id="autohide-true" class="pull-right glyphicon" aria-hidden="true"></span>
                                                </div>
                                                <h3 class="collapse"><%# Eval("APPLICATION_TYPE_ID") %></h3>
                                            </div>
                                            <div class="panel-body">
                                                <p>
                                                    <%# Eval("APPLICATION_TYPE_DESC").ToString() %> 
                                                </p>
                                            </div>
                                            <div class="panel-footer">
                                                <button type="button" class="panelButton" id="SelectAppType">
                                                    Select
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                         <a id="applicationTypeWaiver" onclick="javascript:toggle(this);"
                            title="Click here for more application types" class="btn btn-lg btn-default">Click here for more application types...</a>
                    </div>--%>
                    <div id="divapplicationTypeWaiver" class="collapse">
                        <br />
                        <div class="row flex" data-toggle="collapse" data-target="#divapplicationTypeWaiver">
                            <asp:Repeater ID="rptApplicationWaiver" runat="server" OnItemDataBound="rptApplicationWaiver_ItemDataBound">
                                <ItemTemplate>
                                    <div class="col-sm-3">
                                        <div class="panel panel-info panel-click" id="divrptWaiver" runat="server">
                                            <div class="panel-heading panel-clr" id="divHeading1" runat="server">
                                                <div class="formLabelAuto">
                                                    <%# Eval("WAIVER_TYPE_NAME") %><span id="autohide-true" class="pull-right glyphicon" aria-hidden="true"></span>
                                                </div>
                                                <h2 class="collapse"><%# Eval("WAIVER_TYPE_ID") %></h2>
                                                <h3 class="collapse"><%# Eval("APPLICATION_TYPE_ID") %></h3>
                                            </div>
                                            <div class="panel-body">
                                                <p>
                                                    <%# Eval("WAIVER_TYPE_DESC").ToString() %><%--<%# Eval("APPLICATION_TYPE_DESC").ToString().Length > 50 ? Eval("APPLICATION_TYPE_DESC").ToString().Substring(0,50): Eval("APPLICATION_TYPE_DESC").ToString() %>--%>
                                                </p>
                                            </div>
                                            <div class="panel-footer" id="divfooter1" runat="server">
                                                <button type="button" class="panelButton" id="SelectAppType1" runat="server">
                                                    Select
                                                </button>
                                            </div>
                                            <span id="spnW" data-apptype='<%# Eval("WAIVER_TYPE_ID") %>' runat="server" class="tooltiptextAppType"></span>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="wdAuto" id="CatDetails">
                        <div class="row" id="trApplicationType" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Application Type</span>
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtApplicationType" runat="server" Enabled="false" CssClass="formFieldReadOnly"></asp:TextBox>
                                <asp:TextBox ID="txtApplicationTypeID" runat="server" AutoPostBack="true" OnTextChanged="txtApplicationTypeID_TextChanged" CssClass="collapse"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtWaiverType" runat="server" />
                                <asp:HiddenField ID="hdnIsWaiver" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvApplicationType" runat="server" ControlToValidate="txtApplicationType" Enabled="true" SetFocusOnError="true"
                                    Display="Dynamic" Text="*" ValidationGroup="AddNewProvider" ErrorMessage="* Application Type is required."></asp:RequiredFieldValidator>
                                <a href="#applicationType" onclick="javascript:toggleApplicationType(this);" id="lnkApplicationType" title="Change Application Type" runat="server">Change</a>
                            </div>
                        </div>
                        <div class="row" id="trWaiverType" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Waiver Type</span>
                            </div>
                            <div class="col-sm-9">

                                <asp:TextBox ID="txtWaiverType" runat="server" Enabled="false" CssClass="formFieldReadOnly"></asp:TextBox>
                                <asp:TextBox ID="txtWaiverTypeID" runat="server" AutoPostBack="true" OnTextChanged="txtWaiverTypeID_TextChanged" CssClass="collapse"></asp:TextBox>

                            </div>
                        </div>
                        <div class="row collapse" id="trCategoryType">
                            <div class="col-sm-12" style="text-align: center !important;">
                                <br />
                                <div data-toggle="collapse" data-target="#trCategoryType">
                                    <asp:Repeater ID="rptCategory" runat="server" OnItemDataBound="rptCategory_ItemDataBound">
                                        <ItemTemplate>
                                            <button id="btnCat" type="button" class="buttonCategory" runat="server">
                                                <img src='<%# Eval("IMAGE_SRC") %>' runat="server" />
                                                <%# Eval("PROVIDER_CATEGORY_TYPE_NAME") %><span id="autohide-true_category" class="pull-right glyphicon" aria-hidden="true"></span>
                                                <h3 class="collapse"><%# Eval("PROVIDER_CATEGORY_TYPE_ID") %></h3>

                                                <span id="spnProviderCategory" data-apptype='<%# Eval("PROVIDER_CATEGORY_TYPE_ID") %>' runat="server" class="tooltiptextAppType1"></span>
                                            </button>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <br />
                            </div>
                        </div>
                    </div>
                    <div id="ProvDetails">
                        <div>
                            <div class="row" id="trCategory" runat="server">
                                <div class="col-sm-3 text-right">
                                    <span class="formLabelAuto">Category*</span>
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtCategory" runat="server" Enabled="false" CssClass="formFieldReadOnly"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="txtCategory" Enabled="true" SetFocusOnError="true"
                                        Display="Dynamic" Text="*" ValidationGroup="AddNewProvider" ErrorMessage="* Category is required."></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtCategoryID" runat="server" AutoPostBack="True" OnTextChanged="txtCategoryID_TextChanged" CssClass="collapse" Width="0px"></asp:TextBox>
                                    <a href="#trCategoryType" data-toggle="collapse"  id="lnkCategoryType" runat="server">Change</a>
                                </div>
                            </div>
                            <div class="row" id="trEntityType" runat="server">
                                <div class="col-sm-3 text-right">
                                    <span class="formLabelAuto">Entity Type*</span>
                                </div>
                                <div class="col-sm-9">
                                    <asp:RadioButtonList ID="rblEntityType" runat="server" RepeatDirection="Horizontal" Style="padding: 0; margin: 0;" AutoPostBack="true" OnSelectedIndexChanged="rblEntityType_SelectedIndexChanged">
                                        <asp:ListItem Selected="False" Text="Individual" Value="Individual"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="Organization" Value="Organization"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="row" id="trProviderType" runat="server">
                                <div class="col-sm-3 text-right">
                                    <span class="formLabelAuto">Provider Type*</span>
                                </div>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddlProviderType" runat="server" AutoPostBack="true" AppendDataBoundItems="True" CssClass="formDropDownMedium" OnSelectedIndexChanged="ddlProviderType_SelectedIndexChanged" />
                                    <asp:CompareValidator runat="server" ID="valTypeCmp" ControlToValidate="ddlProviderType"
                                        ValueToCompare="0" Type="Integer" ErrorMessage="* Provider Type is required."
                                        Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*"
                                        ValidationGroup="AddNewProvider" />
                                </div>
                            </div>
                            <%--//akash --%>
                            <div class="row" id="divNursingLicense" runat="server">
                                <div class="col-sm-3 text-right">
                                    <span class="formLabelAuto" style="float: right">Are you a nurse with a valid nursing license? </span>
                                </div>
                                <div class="col-sm-9">
                                    <asp:RadioButtonList ID="RadiochkNursingLicense" runat="server" RepeatDirection="Horizontal" CssClass="QstRadioList">
                                        <asp:ListItem Selected="False" Text="Yes" Value="YES"></asp:ListItem>
                                        <asp:ListItem Selected="False" Text="No" Value="No"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <%--//akash --%>
                            <div class="row" id="trSpecialty" runat="server" style="display: none">
                                <div class="col-sm-3 text-right">
                                    <span class="formLabelAuto">Specialty*</span>
                                </div>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddlSpecialty" runat="server" AutoPostBack="true" AppendDataBoundItems="True" CssClass="formDropDownMedium" OnSelectedIndexChanged="ddlSpecialty_SelectedIndexChanged" />
                                </div>
                            </div>
                            
                            <div class="row" id="trPracticeType" runat="server" visible="false">
                                <div class="col-sm-3 text-right">
                                    <span class="formLabelAuto">Type of Practice*</span>
                                </div>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddlPracticeType" runat="server" AutoPostBack="false" ViewStateMode="Enabled" Enabled="false" CssClass="formDropDownMedium" />
                                    <asp:CompareValidator runat="server" ID="cvPracticeType" ControlToValidate="ddlPracticeType"
                                        ValueToCompare="0" Type="Integer" ErrorMessage="* Practice Type is required." Enabled="false"
                                        Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*"
                                        ValidationGroup="AddNewProvider" />
                                </div>
                            </div>
                            <div class="row group-provider" id="trOrgName" runat="server">
                                <div class="col-sm-3 text-right">
                                    <span class="formLabelAuto">Name of Business Entity*</span>
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtProviderName" runat="server" MaxLength="100" CssClass="formField"></asp:TextBox>
                                    <%-- akash--%>
                                    <asp:RequiredFieldValidator ID="valNameReqd" runat="server" ControlToValidate="txtProviderName" Enabled="true" SetFocusOnError="true"
                                        Display="Dynamic" Text="*" ValidationGroup="AddNewProvider" ErrorMessage="* Business Name is required."></asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="hdnName" runat="server" />
                                </div>
                            </div>


                        </div>
                        <%-- // akash --%>
                        <div class="row" id="trEPID" runat="server" visible="false">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Exiting Provider Medicaid ID	
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtExistingMedicaidId" runat="server" MaxLength="10" CssClass="formField "
                                    OnTextChanged ="txtProviderMedicID_TextChangedMedicID" AutoPostBack="true"/>
                                <asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="txtExistingMedicaidId" Enabled="true" SetFocusOnError="true"
                                    Display="Dynamic" Text="*" ValidationGroup="AddNewProvider" ErrorMessage="* Exiting Medicaid Id is required."></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row" id="trEPNPI" runat="server" visible="false">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Exiting Provider NPI	
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtExistingProviderNPI" runat="server" MaxLength="10" CssClass="formField " />
                            </div>
                        </div>
                        <div class="row" id="trEPName" runat="server" visible="false">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Exiting Provider Name	
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtExistingProviderName" runat="server" CssClass="formField " />
                            </div>
                            <%--<div class="col-sm-9">
                                <asp:TextBox ID="txtProviderName" runat="server" MaxLength="100" CssClass="formField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valNameReqd" runat="server" ControlToValidate="txtProviderName" Enabled="true" SetFocusOnError="true"
                                    Display="Dynamic" Text="*" ValidationGroup="AddNewProvider" ErrorMessage="* Business Name is required."></asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hdnName" runat="server" />
                            </div>--%>
                        </div>
                        <div class="row group-provider" id="trOrgNameHint" runat="server">
                            <div class="col-sm-9 pg-hint2" style="text-align: center !important;">
                                Business Name as it appears on your IRS Assignment letter
                            </div>
                        </div>
                        <div class="row indiv-provider" id="trFN" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">First Name*</span>
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtFirstName" runat="server" MaxLength="35" CssClass="formField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valFNReqd" runat="server" ControlToValidate="txtFirstName" Enabled="false" SetFocusOnError="true"
                                    Display="Dynamic" Text="*" ValidationGroup="AddNewProvider" ErrorMessage="* First Name is required."></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row indiv-provider" id="trMI" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Middle Name</span>
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtMI" runat="server" MaxLength="10" CssClass="formField"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revMI" runat="server" ControlToValidate="txtMI"
                                    ValidationExpression=".*[a-zA-Z]+.*" ErrorMessage="* Enter valid Middle Initial."
                                    Enabled="true" SetFocusOnError="true" Text="*"
                                    ValidationGroup="AddNewProvider" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="row indiv-provider" id="trLN" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Last Name*
                            </div>
                            <div class="col-sm-9 fieldValue">
                                <asp:TextBox ID="txtLastName" runat="server" MaxLength="35" CssClass="formField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valLNReqd" runat="server" ControlToValidate="txtLastName" Enabled="false" SetFocusOnError="true"
                                    Display="Dynamic" Text="*" ValidationGroup="AddNewProvider" ErrorMessage="* Last Name is required."></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row indiv-provider" id="trTaxIDType" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Tax ID Type*
                            </div>
                            <div class="col-sm-9">
                                <asp:RadioButtonList ID="rblTaxIDType"  runat="server" RepeatDirection="Horizontal" CssClass="QstRadioList" AutoPostBack="true" OnSelectedIndexChanged="rblTaxIDType_SelectedIndexChanged">
                                    <asp:ListItem Selected="False" Text="EIN" Value="16"></asp:ListItem>
                                    <asp:ListItem Selected="False" Text="SSN" Value="15"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="valTaxTypeReqd" runat="server" ControlToValidate="rblTaxIDType" Enabled="true" SetFocusOnError="true"
                                    Display="None" Text="*" ValidationGroup="AddNewProvider" ErrorMessage="* Tax ID Type is required."></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cvTaxType" runat="server" OnServerValidate="Validate_TaxIDType" ControlToValidate="rblTaxIDType"
                                    Display="Dynamic" ValidationGroup="AddNewProvider" ErrorMessage="* When enrolling or re-enrolling as Individual provider, you must provide your SSN. Please contact the MAXIMUS help desk." SetFocusOnError="true" Enabled="true" Text="*" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Tax ID*
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtTaxID" runat="server" CssClass="formField" MaxLength="9" Enabled="true"/>
                                <asp:RegularExpressionValidator ID="reTaxID" runat="server" ControlToValidate="txtTaxID"
                                    ValidationExpression="(?!078051120|219099999$)^(?!000|666)[0-8][0-9]{2}(?!00)[0-9]{2}(?!0000)[0-9]{4}$" ErrorMessage="* Enter a valid Tax ID."
                                    Enabled="false" SetFocusOnError="true" Text="*"
                                    ValidationGroup="AddNewProvider" Display="Dynamic"/>
                                  <asp:RegularExpressionValidator ID="reTaxIDEIN" runat="server" ControlToValidate="txtTaxID"
                                    ValidationExpression="^([0-9])+$" ErrorMessage="* Enter a valid Tax ID"
                                    Enabled="false" SetFocusOnError="true" Text="*"
                                    ValidationGroup="AddNewProvider" Display="Dynamic"/>
                            </div>
                        </div>
                        <div class="row" id="trOldNPI" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Old NPI
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtOldNPI" runat="server" CssClass="formFieldReadOnly" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="trOldNPIStartDate" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Old NPI Start Date
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtOldNPIStartDate" runat="server" CssClass="formFieldReadOnly" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row" id="trOldNPIEndDate" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Old NPI End Date*
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtOldNPIEndDate" runat="server" CssClass="formField" />
                                <ajax:CalendarExtender ID="ceOldNPIEndDate" TargetControlID="txtOldNPIEndDate" runat="server" />
                                <asp:RequiredFieldValidator ID="valOldNPIEndDate" runat="server" SetFocusOnError="true" ValidationGroup="AddNewProvider" Text="*"
                                    ControlToValidate="txtOldNPIEndDate" ErrorMessage="* Old NPI End Date is required." Display="Dynamic" Enabled="true" />
                                <asp:CompareValidator ID="cvOldNPIEndDate" runat="server" ValidationGroup="AddNewProvider"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtOldNPIEndDate" Enabled="true"
                                    ErrorMessage="* A valid Old NPI End Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>
                                <asp:CompareValidator ID="cvOldNPIEndDateGreater" runat="server" ValidationGroup="AddNewProvider"
                                    Type="Date" Operator="GreaterThan" ControlToValidate="txtOldNPIEndDate"
                                    ControlToCompare="txtOldNPIStartDate" Enabled="true"
                                    ErrorMessage="* Old NPI end date must be after old NPI start date."
                                    Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>
                                <asp:CompareValidator ID="cvOldNPIEndDateLessThan" runat="server" ValidationGroup="AddNewProvider"
                                    Type="Date" Operator="LessThan" ControlToValidate="txtOldNPIEndDate"
                                    ControlToCompare="txtNPIStartDate" Enabled="true"
                                    ErrorMessage="* Old NPI end must be before NPI start date."
                                    Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="false"> 
                                </asp:CompareValidator>
                            </div>
                        </div>

                        <div class="row" id="chkRetroCoverage" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Are you requesting retro coverage?	</span>
                            </div>
                            <%--   <div class="col-sm-9">
                                <asp:CheckBox ID="chkRetro" runat="server" OnCheckedChanged="chkRetro_CheckedChanged" />
                                <div id="retroHelpLink" class="bodyTextSmall what-is-this-link" style="color: blue; cursor: pointer; display: inline-block; text-decoration: underline" runat="server">
                                    <span class="ohio-field-label">What is this?	
                                         <span class="ohio-tooltip fa-info-circle" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="<asp:Literal ID='PROVIDER_SEARCH_POPUP_32' runat='server' Text='<%$ Resources:BrandingResource ,RETRO_COVERAGE_HELPTEXT %>' />" aria-hidden="true"></span>
                                    </span>
                                </div>
                            </div>--%>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox ID="chkRetro" runat="server" AutoPostBack="true" OnCheckedChanged="chkRetro_CheckedChanged" />
                                <span id="helpTaxID" class="help-taxid" style="cursor: pointer; display: inline-block;">What is this
                                        <asp:Image ID="imgHelpTaxID" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" onmouseout="meventleave()" onmouseover="mevent(event)" /></span>
                                <span id="helpTaxIDInfo" class="infoBox" style="top: 0; right: 0;">
                                    <span class="infoTitle">Retro Coverage Help</span>
                                    <span class="infoContent">
                                        <asp:Literal ID="ltlTaxIdHelp" runat="server" Text="<%$ Resources:BrandingResource , RETRO_COVERAGE_HELPTEXT %>"></asp:Literal>
                                    </span>
                                </span>
                                <div id="retroHelpLink" runat="server">
                                </div>

                            </div>
                        </div>
                        <div class="row" id="trNPI" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto" id="lblNPI" runat="server">NPI*</span>
                              </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtNPI" runat="server" MaxLength="10" CssClass="formField" AutoPostBack="true" OnTextChanged="txtNPI_TextChanged"/>
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="javascript:void(0);" OnClick="clientSideFunction();">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/help.jpg" CssClass="help-img" ImageAlign="Middle" ToolTip ="For Information and instructions for obtaining an NPI: https://dam.assets.ohio.gov/image/upload/medicaid.ohio.gov/Providers/Enrollment%20and%20Support/National-Provider-Identifier-Reference-Guide.pdf" />
                                    </asp:HyperLink>
                                <asp:HiddenField ID="hdnNPI" runat="server" />
                                <asp:RegularExpressionValidator ID="valNPI" runat="server" ControlToValidate="txtNPI"
                                    ValidationExpression="^([1-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9])$" ErrorMessage="* Enter a 10 digit NPI that does not begin with 0."
                                    Enabled="true" SetFocusOnError="true" Text="*"
                                    ValidationGroup="AddNewProvider" Display="Dynamic" />
                                <asp:CustomValidator ID="cvNPIUnchanged" runat="server" ControlToValidate="txtNPI" OnServerValidate="Validate_NPIEdit"
                                    Display="Dynamic" ValidationGroup="AddNewProvider" ErrorMessage="* New NPI must be different than old NPI." Text="*" />
                                <div id="divEditNPI" runat="server" class="bodyTextSmall what-is-this-link" style="color: blue; cursor: pointer; display: inline-block; text-decoration: underline">
                                    <asp:LinkButton ID="lnkEditNPI" runat="server" Text="Edit NPI" CommandName="Edit NPI" OnClick="lnkEditNPI_Click" />
                                </div>
                                <div id="divEditNPIEndDate" runat="server" class="bodyTextSmall what-is-this-link" style="color: blue; cursor: pointer; display: inline-block; text-decoration: underline">
                                    <asp:LinkButton ID="lnkEditEndDate" runat="server" Text="Edit End Date" CommandName="Edit End Date" OnClick="lnkEditEndDate_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="row" id="trNPIStartDate" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">NPI Start Date*
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtNPIStartDate" runat="server" CssClass="formField" />
                                <ajax:CalendarExtender ID="ceNPIStartDate" TargetControlID="txtNPIStartDate" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" SetFocusOnError="true" ValidationGroup="AddNewProvider" Text="*"
                                    ControlToValidate="txtNPIStartDate" ErrorMessage="* NPI Start Date is required." Display="Dynamic" Enabled="true" />
                                <asp:CompareValidator ID="valNPIStartDate" runat="server" ValidationGroup="AddNewProvider"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtNPIStartDate" Enabled="true"
                                    ErrorMessage="* A valid NPI Start Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>
                            </div>
                        </div>
                        <div class="row" id="trNPIEndDate" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">NPI End Date
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtNPIEndDate" runat="server" CssClass="formField" />
                                <ajax:CalendarExtender ID="ceNPIEndDate" TargetControlID="txtNPIEndDate" runat="server" />
                                <asp:CompareValidator ID="valNPIEndDate" runat="server" ValidationGroup="AddNewProvider"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtNPIEndDate" Enabled="true"
                                    ErrorMessage="* A valid NPI End Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>
                                <asp:CompareValidator ID="cvNPIEndDate" runat="server" ValidationGroup="AddNewProvider"
                                    Type="Date" Operator="GreaterThan" ControlToValidate="txtNPIEndDate"
                                    ControlToCompare="txtNPIStartDate" Enabled="true"
                                    ErrorMessage="* NPI end date must be after NPI start date."
                                    Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>
                            </div>
                        </div>
                        <div class="row" id="divDDContractNumber" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">DD Contract Number (If Applicable)
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtDDContractNumber" runat="server" MaxLength="10" CssClass="formField" />
                            </div>
                        </div>
                        <div class="row" id="divDDFacilityNumber" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">DD Facility Number* 
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtDDFacilityNumber" runat="server" MaxLength="10" CssClass="formField" />
                                <asp:RequiredFieldValidator ID="valtxtDDFacilityNumber" runat="server" SetFocusOnError="true" ValidationGroup="AddNewProvider" Text="*"
                                    ControlToValidate="txtDDFacilityNumber" ErrorMessage="* DD Facility Number is required." Display="Dynamic" Enabled="true" />
                            </div>
                        </div>
                        <div class="row" id="trRED" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Requested Effective Date*
                            </div>
                            <div class="col-sm-9 fieldValue">
                                <asp:TextBox ID="txtEffectiveDate" runat="server" CssClass="formField" />
                                <ajax:CalendarExtender ID="ceEffectiveDate" TargetControlID="txtEffectiveDate" runat="server" OnClientDateSelectionChanged="checkDate" />
                                <asp:RequiredFieldValidator ID="valEffectiveDateReqd" runat="server" SetFocusOnError="true" ValidationGroup="AddNewProvider" Text="*"
                                    ControlToValidate="txtEffectiveDate" ErrorMessage="* Requested Effective Date is required." Display="Dynamic" Enabled="true" />
                                <asp:CompareValidator ID="cvEffectiveDate" runat="server" ValidationGroup="AddNewProvider"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtEffectiveDate" Enabled="true"
                                    ErrorMessage="* A valid Requested Effective Date is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>
                                <%-- <div id="divWhatIsReqEffectiveDate" class="bodyTextSmall what-is-this-link" style="color: blue; cursor: pointer; display: inline-block; text-decoration: underline" runat="server">
                                    What is this?
                                </div>--%>
                                <div id="divRequestedEffectiveDateInfo" class="infoBox" style="left: 120px; top: 100px;">
                                    <div class="infoTitle">
                                        Requested Effective Date
                                    </div>
                                    <div class="infoContent">
                                        <asp:Literal ID="ltlRequestedEffectiveDateInfo" runat="server" Text="<%$ Resources:BrandingResource , REQUESTED_EFFECTIVE_DATE_HELPTEXT %>"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row indiv-provider" id="trGender" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Gender*
                            </div>
                            <div class="col-sm-9">
                                <asp:DropdownList ID="ddlGender" runat="server" CssClass="formDropDownMedium" style="min-width:333px">
                                </asp:DropdownList>
                                <asp:CompareValidator runat="server" ID="cvddlGender" ControlToValidate="ddlGender"
                                        ValueToCompare="" Type="String" ErrorMessage="* Gender is required."
                                        Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*"
                                        ValidationGroup="AddNewProvider" />
                            </div>
                        </div>
                        <div class="row indiv-provider" id="trDOB" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Date of Birth*
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtBirthDate" runat="server" CssClass="formField" />
                                <ajax:CalendarExtender ID="ceDOB" TargetControlID="txtBirthDate" runat="server" />
                                <asp:RequiredFieldValidator ID="valDOBReqd" runat="server" SetFocusOnError="true" ValidationGroup="AddNewProvider" Text="*"
                                    ControlToValidate="txtBirthDate" ErrorMessage="* Date of Birth is required." Display="Dynamic" Enabled="false" />
                                <asp:CompareValidator ID="cvDOBFormat" runat="server" ValidationGroup="AddNewProvider"
                                    Type="Date" Operator="DataTypeCheck" ControlToValidate="txtBirthDate" Enabled="false"
                                    ErrorMessage="* A valid Date of Birth is required (mm/dd/yyyy)." Text="*" Display="Dynamic" ValueToCompare="MM/dd/yyyy"
                                    SetFocusOnError="true"> 
                                </asp:CompareValidator>
                            </div>
                        </div>
                        <div class="row" id="trZipCode" runat="server" visible ="false"> 
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Zip Code*
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtZipCode" runat="server" MaxLength="5" CssClass="formField" />
                                <!--<asp:RequiredFieldValidator ID="valZipReqd" runat="server" ControlToValidate="txtZipCode"
                                    Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                                    ValidationGroup="AddNewProvider" ErrorMessage="* Zip Code is required."></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="valZipFormat" runat="server" Text="*"
                                    ErrorMessage="* Enter 5 digits for zip code" ControlToValidate="txtZipCode" SetFocusOnError="true"
                                    Display="Dynamic" ValidationExpression="(?!0{5})(?!9{5})\d{5}$" ValidationGroup="AddNewProvider" />-->
                            </div>
                        </div>
                        <div class="row" id="trZipExt" runat="server" visible="false">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Zip Code Extension*
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtZipCodeExt" runat="server" MaxLength="4" CssClass="formField" />
                                <!--<asp:RequiredFieldValidator ID="valZipExtRqd" runat="server" ControlToValidate="txtZipCodeExt"
                                    Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*"
                                    ValidationGroup="AddNewProvider" ErrorMessage="* Zip Code Extension is required."></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="valZipExtFormat" runat="server" Text="*"
                                    ErrorMessage="* Enter 4 digits for zip code extension" ControlToValidate="txtZipCodeExt" SetFocusOnError="true"
                                    Display="Dynamic" ValidationExpression="(?!0{4})(?!9{4})\d{4}$" ValidationGroup="AddNewProvider" />-->
                            </div>
                        </div>
                        <div class="row" id="trReferral" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">
                                    <asp:Literal ID="lbl4ReferralNumber" runat="server" Text="<%$ Resources:BrandingResource , REFERRAL_NUMBER_LABEL %>" />
                                    *
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtApplicationNumber" runat="server" MaxLength="20" CssClass="formField " />
                                <asp:RequiredFieldValidator ID="valAppNbrRqd" runat="server" ControlToValidate="txtApplicationNumber"
                                    Enabled="false" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="AddNewProvider" ErrorMessage="* Referral Number is required."></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="valApplicationNbr" runat="server" OnServerValidate="Validate_AppNumber" ControlToValidate="txtApplicationNumber"
                                    Display="Dynamic" ValidationGroup="AddNewProvider" ErrorMessage="* Referral Number not found" SetFocusOnError="true" Enabled="false" Text="*" />
                            </div>
                        </div>
                        <div class="row" id="trMedicaid" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Medicaid ID
                            </div>
                            <div class="col-sm-9">
                                <asp:TextBox ID="txtMedicaidID" runat="server" MaxLength="80" CssClass="formField " />
                            </div>
                        </div>
                        <div id="divMultipleMedicaidIds" runat="server" style="display: none;">
                            <div class="grid-hint">
                                Multiple Medicaid IDs were found. Please select the medicaid id to use for this registration.
                            </div>
                            <div class="fieldTable">
                                <div class="row">
                                    <div class="col-sm-3 text-right">
                                        <span class="formLabelAuto">Medicaid/Billing Numbers
                                    </div>
                                    <div class="col-sm-9">
                                        <asp:RadioButtonList ID="rblMedaidIds" runat="server" CssClass="formLabelAuto300 rbl_Vertical formField"
                                            RepeatDirection="Vertical" TextAlign="Right" />
                                        <asp:CompareValidator runat="server" ID="cvMedicaidIDs" ControlToValidate="rblMedaidIds" ValueToCompare="0" Type="Integer"
                                            ErrorMessage="* Multiple medicaid ids were found, selection of one is required." Enabled="false"
                                            Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="AddNewProvider" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" id="trTaxonomy" runat="server">
                                <div class="col-sm-3 text-right">
                                    <span class="formLabelAuto">Taxonomy*</span>
                                </div>
                                <div class="col-sm-9">
                                    <asp:DropDownList ID="ddlTaxonomy" runat="server" AutoPostBack="false" CssClass="formDropDownMedium" />
                                    <asp:CompareValidator runat="server" ID="valTaxonmyCmp" ControlToValidate="ddlTaxonomy" ValueToCompare="0" Type="Integer"
                                        ErrorMessage="* Taxonomy is required." Operator="NotEqual" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="AddNewProvider" />
                                </div>
                            </div>
                        <div class="row" id="trTaxonomyNPPES" runat="server">
                            <div class="col-sm-3 text-right">
                                <span class="formLabelAuto">Taxonomy*</span>
                            </div>
                            <div class="col-sm-9">
                                <asp:DropDownList ID="ddlTaxonomyNPPES" runat="server" AutoPostBack="false" CssClass="formDropDownMedium" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorTaxonomy" runat="server" InitialValue="0" ControlToValidate="ddlTaxonomyNPPES"
                                    Enabled="true" SetFocusOnError="true" Display="Dynamic" Text="*" ValidationGroup="AddNewProvider" ErrorMessage="* Taxonomy is required."></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <asp:UpdateProgress runat="server" ID="upAddProgress" DisplayAfter="0">
                            <ProgressTemplate>
                                <div class="loading">
                                    <asp:Image ID="imgSaving" runat="server" ImageUrl="~/Images/ajax-loader.gif" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <div id="divKeyFieldEditInfo" runat="server" class="row" visible="false">
                            <div class="col-sm-1">
                            </div>
                            <div class="col-sm-7">
                                <div class="pg-hint2">
                                    ***
                                    <asp:Literal ID="ltlKFE" runat="server" Text="<%$ Resources:BrandingResource , KEY_FIELD_EDIT_PAGE_INFO %>" />
                                    ***
                                </div>
                            </div>
                        </div>
                        <div class="btnBox" style="text-align: center">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBoxFocus" OnClick="btnSave_Click" CausesValidation="true" ValidationGroup="AddNewProvider" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" CausesValidation="false" />
                        </div>
                    </div>
                    <br />
                    <br />
                    </span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span></span>
                    </span></span></span></span></span></span>
                </asp:Panel>
            </div>
            <div id="divRegChoice" runat="server" visible="false">
                <div class="pageHeader" style="text-align: center">
                    How do you want to complete your registration?
                </div>
                <br />
                <br />
                <div class="row">
                    <div class="col-sm-3">
                    </div>
                    <div class="col-sm-3">
                        <div class="pageHeader2">
                            Guide me
                        </div>
                        <br />
                        <ul>
                            <li>Walks you through all the sections, one at a time</li>
                            <li>Best if you want us to take you through all situations</li>
                        </ul>
                        <br />
                        <button id="btnGuideme" class="btn btn-lg btn-info" onclick="document.location.href ='Registration.aspx'">
                            Walk me through everything
                        </button>
                    </div>
                    <div class="col-sm-3">
                        <div class="pageHeader2">
                            I'll explore on my own
                        </div>
                        <br />
                        <ul>
                            <li>Choose specific types of sections you want to work on </li>
                            <li>Best if you know which situations apply to you</li>
                        </ul>
                        <br />
                        <button id="btnExplore" class="btn btn-lg btn-info" onclick="document.location.href ='Navigation.aspx'">
                            I'll choose what I work on
                        </button>
                    </div>
                    <div class="col-sm-3">
                    </div>
                </div>
                <br />
            </div>
            <ajax:ModalPopupExtender ID="mpeSaveAndSubmit" runat="server" PopupControlID="pnlSaveSubmit" TargetControlID="btnSaveSubmit"
                CancelControlID="btnSaveSubmitCancel" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlModal">
            </ajax:ModalPopupExtender>
            <asp:Panel ID="pnlSaveSubmit" runat="server" CssClass="modalPopup" align="center" Style="display: none">
                <asp:Panel ID="pnlHeaderMpe" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
                    <div style="text-align: left;">
                        &nbsp;&nbsp;
           <asp:Label ID="lblTitle" CssClass="bodyTextBold" runat="server" Text="" ForeColor="White" />

                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px" DefaultButton="btnSave">
                </asp:Panel>
                <table style="width: 700px; height: 250px; border-spacing: 10px 50px">
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr style="align-content: center">
                        <td style="padding-left: 92px;">
                            <table border="1" style="width: 500px; height: 150px; align-content: center; border-spacing: 10px 50px;">
                                <tr>
                                    <td><span style="font-size: large; align-content: center; text-align: left">After your submission is completed and approved, you can apply for additional Waiver Services with other Agencies thru this registration in the Provider Network Module.</span></td>
                                </tr>
                            </table>
                        </td>

                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="align-content: center; text-align: center">
                            <asp:Button ID="Button1" runat="server" Text="Save And Submit" BackColor="Blue" CssClass="buttonBox" OnClick="btnSave_Click" OnClientClick="HideAndShowPopup()" CausesValidation="true" ValidationGroup="valAgreements" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnSaveSubmit" Style="display: none; visibility: hidden" />
                <asp:Button runat="server" ID="btnSaveSubmitCancel" Style="display: none; visibility: hidden" />
            </asp:Panel>

            <ajax:ModalPopupExtender ID="mpeMessage" runat="server" PopupControlID="pnlMessagepopup" TargetControlID="btnSubmitMessagePopup"
                CancelControlID="btnCancelMessagePopup" BackgroundCssClass="modalBackground" PopupDragHandleControlID="pnlModal">
            </ajax:ModalPopupExtender>
            <asp:Panel ID="pnlMessagepopup" runat="server" CssClass="modalPopup" align="center" Style="display: none">
                <asp:Panel ID="Panel3" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
                    <div style="text-align: left;">
                        &nbsp;&nbsp;
           <asp:Label ID="Label1" CssClass="bodyTextBold" runat="server" Text="" ForeColor="White" />

                    </div>
                </asp:Panel>
                <asp:Panel ID="Panel4" runat="server" Style="margin-right: 10px" DefaultButton="btnSave">
                </asp:Panel>
                <table style="width: 700px; height: 250px; border-spacing: 10px 50px">
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr style="align-content: center">
                        <td style="padding-left: 92px;">
                            <table border="1" style="width: 500px; height: 150px; align-content: center; border-spacing: 10px 50px;">
                                <tr>
                                    <td><span style="font-size: large; align-content: center; text-align: left">Please wait while your information and session are being transferred to another Ohio Agency in order to complete your application.</span></td>
                                </tr>
                            </table>
                        </td>

                    </tr>
                </table>
            </asp:Panel>
            <asp:Button runat="server" ID="btnSubmitMessagePopup" Style="display: none; visibility: hidden" />
            <asp:Button runat="server" ID="btnCancelMessagePopup" Style="display: none; visibility: hidden" />
			<asp:HiddenField ID="workflowEventTypeId_hidden" runat="server" />
        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>


