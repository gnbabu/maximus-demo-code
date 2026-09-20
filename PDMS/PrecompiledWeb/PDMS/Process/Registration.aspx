<%@ page title="" language="C#" masterpagefile="~/MasterWorkflowPage.master" autoeventwireup="true" inherits="Process_Registration, App_Web_unbhbgmw" maintainscrollpositiononpostback="true" enableEventValidation="false" stylesheettheme="Default" %>


<%@ MasterType TypeName="MasterWorkflowPage" %>
<%@ PreviousPageType TypeName="RegistrationProvider" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/PopupControls/RegistrationNavigation.ascx" TagName="RegistrationNavigation" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/UploadSectionControl.ascx" TagName="UploadSectionControl" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/Specialties.ascx" TagName="UcSpeciality" TagPrefix="uc" %>

<%--<%@ Register Src="~/PopupControls/OrgInfoHistory.ascx" TagPrefix="uc" TagName="OrgInfoHistory" %>
<%@ Register Src="~/PopupControls/PrimaryContactInfoHistory.ascx" TagPrefix="uc" TagName="PrimaryContactInfoHistory" %>
<%@ Register Src="~/PopupControls/SpecialtiesHistory.ascx" TagPrefix="uc" TagName="SpecialtiesHistory" %>
<%@ Register Src="~/PopupControls/TaxonomiesHistory.ascx" TagPrefix="uc" TagName="TaxonomiesHistory" %>
<%@ Register Src="~/PopupControls/CertificationsHistory.ascx" TagPrefix="uc" TagName="DEACertificationHistory" %>
<%@ Register Src="~/PopupControls/CertSecondGridHistory.ascx" TagPrefix="uc" TagName="CLIACertificationHistory" %>
<%@ Register Src="~/PopupControls/LicensesHistory.ascx" TagPrefix="uc" TagName="LicensesHistory" %>
<%@ Register Src="~/PopupControls/MedicareHistory.ascx" TagPrefix="uc" TagName="MedicareHistory" %>
<%@ Register Src="~/PopupControls/PharmacyHistory.ascx" TagName="PharmacyHistory" TagPrefix="uc" %>
<%@ Register Src="~/PopupControls/MedicaidHistory.ascx" TagPrefix="uc" TagName="MedicaidHistory" %>
<%@ Register Src="~/PopupControls/SatellitePracticeLocationsHistory.ascx" TagPrefix="uc" TagName="SatellitePracticeLocationsHistory" %>--%>
<%@ Register Src="~/PopupControls/UploadDocument.ascx" TagName="UploadDocument" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageLabelContent" Runat="Server">
    <div style="text-align: left !important"></div>
    <!--  <asp:Label id="lblPgTitle" runat="server" CssClass="pageHeader" />-->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">


    <%--    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>--%>
    <script type="text/javascript">

        $(document).keydown(function (e) {
            // ESCAPE key pressed
            if (e.keyCode == 27) {
                $find("bhShowHistory").hide();
                if (document.getElementById("<%=btnHistory.ClientID %>") != null) {
                    document.getElementById("<%=btnHistory.ClientID %>").focus();
                }
                return false;
            }
            var target = e.target;
            var shiftPressed = e.shiftKey;
            // If TAB key pressed
            if (e.keyCode == 9) {
                // If inside a Modal dialog (determined by attribute role="dialog")
                if ($(target).parents('[role=dialog]').length) {
                    // Find first or last input element in the dialog parent (depending on whether Shift was pressed). 
                    // Input elements must be visible, and can be Input/Select/Button/Textarea.
                    var borderElem = shiftPressed ?
                        $(target).closest('[role=dialog]').find('input:visible,select:visible,button:visible,textarea:visible').first()
                        :
                        $(target).closest('[role=dialog]').find('input:visible,select:visible,button:visible,textarea:visible').last();
                    if ($(borderElem).length) {
                        if ($(target).is($(borderElem))) {
                            return false;
                        } else {
                            return true;
                        }
                    }
                }
            }
            return true;
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

        $(document).keypress(function (e) {
            if (e.which == 13) {
                $('#<%=ucRegistrationNavigation.FindControl("btnSave").ClientID%>').click();
                event.keyCode = 0;
            }
        });
        function openModalDialog() {
            var modalTitle = document.getElementById("h2History").textContent;
            document.getElementById("<%= divBoardHistory.ClientID %>").setAttribute("aria-live", "polite");
            document.getElementById("<%= divBoardHistory.ClientID %>").focus();
        }

    </script>
    <style type="text/css">
        .divGrid {
            width: 100%;
        }

        .gridview {
            float: right;
        }

        .gridViewHeader > th > a {
            color: White !important;
        }

        .panelOwnerInfo {
            padding: 20px 20px 0px 20px;
        }

        td, th {
            padding: 4px !important;
        }

        .row {
            margin-top: 4px;
            margin-bottom: 4px;
        }

        .gridViewSmallFont caption {
            color: #F7F7F7;
        }
    </style>


    <%--<uc:RegistrationNavigation ID="ucRegistrationNavigation" runat="server" />--%>
    <div id="DivDetails" runat="server">
        <div class="enrollment">
            <asp:HiddenField ID="HasUnsavedData" runat="server" />
            <%--<asp:Label id="lblPgTitle" runat="server" CssClass="pageHeader" />--%>
            <uc:registrationnavigation id="ucRegistrationNavigation" runat="server" />
            <asp:Panel ID="pnldetails" runat="server">
                <%--        Note:  these views must be in order of reg_page_type_id--%>
                <%--<uc:RegistrationNavigation ID="ucRegistrationNavigation" runat="server" />--%>
                <div class="row">
                    <div class="col-lg-3 col-sm-0 hidden-xs hidden-sm">
                        <asp:Image runat="server" ID="sectionIcon" ImageUrl="~/Images/ProviderInformation_Lg.png" Style="margin-left: 30%; margin-top: 50%;" alt="ProviderInformation..." CssClass="img-responsive"></asp:Image>

                    </div>

                    <div class="col-lg-9 col-sm-12">

                        <asp:Panel ID="Panel1" runat="server" Style="width: auto; height: auto;">
                            <asp:Panel ID="pnlHeader" runat="server" Visible="false">
                                <div class="popTitle">
                                    <asp:Label ID="lblTitle" runat="server" Text="Title" />
                                </div>
                            </asp:Panel>
                            <div style="text-align: right">
                                <asp:LinkButton ID="btnHistory" runat="server" ToolTip="History" CssClass="buttonBoxFocus" Visible="true" OnCommand="btnHistory_Click" Style="color: white; text-decoration: none;">
                                    <span aria-label="History Button" role="img" class="glyphicon glyphicon-book" style="padding-right:7px;"></span>History 
                                </asp:LinkButton>
                            </div>
                            <asp:Panel ID="pnlMain" runat="server" Style="margin-right: 10px">
                                <asp:PlaceHolder ID="sectionPH" runat="server" Visible="true" />
                            </asp:Panel>
                            <table border="0" cellpadding="0" cellspacing="5" align="center" style="padding-bottom: 10px">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="buttonBox" OnClick="btnSave_Click" CausesValidation="true" Style="display: none;" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox"
                                            CausesValidation="false" Style="display: none;" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>

            </asp:Panel>
            <asp:HiddenField ID="hdnRegStep" runat="server" />
        </div>

        <br />
        <asp:Panel ID="pnlUploadDocsfull" runat="server">
            <%-- TODO: EDV Do we need this usSep1 its always not visible --%>
            <div class="boxContainer" runat="server" id="dvboxcontainer">
                <asp:Label ID="ucSep1" runat="server" Text="Uploaded Documents" CssClass="boxLabel" Visible="false" /></div>
            <h2><span id="spPageHeader" runat="server" class="pageHeader">Uploaded Documents</span></h2>
            <asp:Panel ID="pnlUploadDocs" runat="server" CssClass="UploadBox">
                <uc1:uploaddocument id="ucUploadDocument" runat="server" cssclassuploadbutton="buttonBox" documentsection=""
                    validfileextensions="doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt" visible="false" />
                <asp:PlaceHolder runat="server"
                    ID="PlaceholderUploadSectionControl" OnPreRender="PlaceholderUploadSectionControl_PreRender"></asp:PlaceHolder>
            </asp:Panel>
        </asp:Panel>

        <div runat="server" id="convertedDocs" visible="false">
            <br />
            <h2><span class="pageHeader">Converted Documents</span></h2>
            <br />
            <asp:Panel ID="pnlConvertedDocs" runat="server" CssClass="WhiteBox">
                <mms:sortablepaginggridview id="gvConvertedDocs" runat="server" autogeneratecolumns="False" caption="<span style='display:none'>Converted Documents</span>"
                    horizontalalign="Center" width="100%" showheaderwhenempty="true"
                    cssclass="gridViewSmallFont" emptydatatext="No converted documents found."
                    onrowcommand="gvConvertedDocs_RowCommand"
                    onrowdatabound="gvConvertedDocs_RowDataBound" onpageindexchanging="grdvConvertedDocs_PageIndexChanging">
                    <columns>
                        <asp:BoundField DataField="DOC_TYPE" HeaderText="Doc Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="DTL_DOC_TYPE" HeaderText="Detail Doc Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="SUB_DOC_TYPE" HeaderText="Sub Doc Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="FILE_NAME" HeaderText="File Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="DOCUMENT_ID" HeaderText="Document Id" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                        <asp:TemplateField HeaderText="<span style='display:none'>Search</span>" ItemStyle-HorizontalAlign="Center" ShowHeader="false">
                            <itemtemplate>
                                <asp:ImageButton ID="imgView" ImageUrl="~/Images/search.png" alt="Search Button" runat="server" ToolTip="View" CommandName="Edit" />
                            </itemtemplate>
                        </asp:TemplateField>
                    </columns>
                    <pagerstyle cssclass="gridpager" horizontalalign="Right" />
                    <headerstyle cssclass="gridViewHeader" />
                    <alternatingrowstyle cssclass="gridViewAltRow" />
                    <rowstyle cssclass="gridViewRow" />
                    <footerstyle cssclass="gridViewFooter" />
                </mms:sortablepaginggridview>
            </asp:Panel>
            <br />
        </div>

        <div runat="server" id="paperDocs" visible="false">
            <br />
            <span class="pageHeader">Paper Uploaded Documents</span>
            <br />
            <asp:Panel ID="pnlPaperDocs" runat="server" CssClass="WhiteBox">
                <asp:GridView ID="gvPaperDocuments" runat="server" AutoGenerateColumns="False"
                    HorizontalAlign="Center" Width="100%" ShowHeaderWhenEmpty="true"
                    CssClass="gridViewSmallFont" EmptyDataText="No paper documents found."
                    OnRowCommand="gvPaperDocuments_RowCommand"
                    OnRowDataBound="gvPaperDocuments_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="DESCRIPTION" HeaderText="DESCRIPTION" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="UserName" HeaderText="UserName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="REG_PAGE_NAME" HeaderText="Page Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="ONBASE_DOCUMENT_ID" HeaderText="Onbase ID" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" ShowHeader="false">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgView" alt="Search Button" ImageUrl="~/Images/search.png" runat="server" ToolTip="View" CommandName="Edit" OnClick="imgView_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="gridpager" HorizontalAlign="Right" />
                    <HeaderStyle CssClass="gridViewHeader" />
                    <AlternatingRowStyle CssClass="gridViewAltRow" />
                    <RowStyle CssClass="gridViewRow" />
                    <FooterStyle CssClass="gridViewFooter" />
                </asp:GridView>
            </asp:Panel>
            <br />
        </div>
        <%--  <uc:FileUpload ID="ucFileUpload" runat ="server" CssClassUploadButton="buttonBox"  DocumentSection=""
        ValidFileExtensions="doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt"  />--%>

        <%--  <uc:UploadSectionControl ID="ucUploadNPI" runat="server" IsRequired="true" Title="test" 
         Description="(Required for Out of state except MD & VA)"  ValidFileExtensions="doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt" DestinationPath="C:\projects\temp"/> 
    <br /> <br />--%>
    </div>
    <%-- TODO: EDV Removing the bottom registration navigation to see the impact of view state --%>
    <uc:registrationnavigation id="ucRegistrationNavigationBottom" runat="server" visible="true" />
    <asp:UpdatePanel ID="upHistory" runat="server">
        <ContentTemplate>
            <div role="dialog" aria-labelledby="h2History" aria-modal="true" aria-live="assertive" id="divBoardHistory" runat="server">
                <ajax:modalpopupextender id="mpeShowHistory" okcontrolid="btnHistoryOk" runat="server" popupcontrolid="pnlHistory" targetcontrolid="ButtonDummy3"
                    backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnlHistory" behaviorid="bhShowHistory">
                </ajax:modalpopupextender>
                <asp:Panel ID="pnlHistory" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 1200px;">
                    <asp:Panel ID="pnlHistoryHeader" CssClass="pnlHeader" runat="server" HorizontalAlign="Left">
                        <div align="left">
                            &nbsp;&nbsp;
                            <h2 id="h2History" tabindex="0">
                                <asp:Label ID="lblHistoryTitle" CssClass="bodyTextBold" runat="server" Text="Title" ForeColor="White" /></h2>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlHistoryMain" runat="server" Style="margin-right: 10px">
                        <asp:PlaceHolder ID="phSectionHistory" runat="server" Visible="true" />
                    </asp:Panel>
                    <div class="btnBox">
                        <asp:Button runat="server" ID="btnHistoryOk" Text="OK" CssClass="buttonBox" CausesValidation="false" />
                    </div>
                </asp:Panel>

                <asp:Button runat="server" ID="ButtonDummy3" aria-Label="Dummy Button" Style="display: none" />
            </div>
        </ContentTemplate>




        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnHistory" />
        </Triggers>
    </asp:UpdatePanel>

   <%-- <ajax:modalpopupextender id="modalCMC" okcontrolid="Button2" runat="server" popupcontrolid="Panel2" targetcontrolid="ButtonDummy"
        backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnlModal" behaviorid="bhModalCMC">
    </ajax:modalpopupextender>
    <asp:Panel ID="Panel2" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 50%;">
        <div class="center">
            Your application for the CMC program has been submitted.
        <br />
            You will receive a Welcome letter upon approval
        </div>
        <div class="center">
            <asp:Button runat="server" ID="Button2" Text="OK" CssClass="buttonBox" CausesValidation="false" />
        </div>
    </asp:Panel>--%>

    <ajax:modalpopupextender id="mpeChangesSaved" okcontrolid="btnModalOk" runat="server" popupcontrolid="pnlModal" targetcontrolid="ButtonDummy"
        backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnlModal" behaviorid="bhChangesSaved">
    </ajax:modalpopupextender>
    <asp:Panel ID="pnlModal" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 50%;">
        <div class="center">
            Your application is complete and has been saved. Please take time to review your application prior to submission. You will be able to generate your completed application in PDF form prior to submitting your application. 
        <br />
            <br />
            Once your review is complete, <b>you must click 'Submit for Review' at the top of the Agreements page to submit your application.</b>
        </div>
        <div class="center">
            <asp:Button runat="server" ID="btnModalOk" Text="OK" CssClass="buttonBox" CausesValidation="false" />
        </div>
    </asp:Panel>
    <div style="display: none">
        <asp:Panel ID="Panel3" runat="server" CssClass="UploadBox">
            <uc:ucspeciality id="UcSpeciality1" runat="server" visible="false" />

        </asp:Panel>
    </div>
    <asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />

    <ajax:modalpopupextender id="mpeError" okcontrolid="btnErrorOk" runat="server" popupcontrolid="pnlFatalError" targetcontrolid="ButtonDummy2"
        backgroundcssclass="modalBackground" popupdraghandlecontrolid="pnlFatalError" behaviorid="bhError">
    </ajax:modalpopupextender>
    <asp:Panel ID="pnlFatalError" runat="server" CssClass="modalPopup" Style="display: none; padding: 20px; width: 200px;">
        <div class="center">The registration was not found.</div>
        <div class="btnBox">
            <asp:Button runat="server" ID="btnErrorOk" Text="OK" CssClass="buttonBox" CausesValidation="false" OnClick="btnErrorOk_Click" />
        </div>
    </asp:Panel>
    <asp:Button runat="server" ID="ButtonDummy2" Style="display: none" Text="ButtonDummy2" />

    <%--<ajax:ModalPopupExtender ID="mpeUpdatePopup" runat="server" PopupControlID="pnlContinueModal" TargetControlID="ButtonDummy4" 
        BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlContinueModal">
    </ajax:ModalPopupExtender>
    <asp:Panel ID="pnlContinueModal" runat="server" CssClass="identModalPopup" align="center" Style="display: none; padding: 20px; width: 300px;">
        <div class="center"><strong>ALERT - Update not submitted</strong><br/><br/></div>
        <div class ="left">
            You have started an update on this registration that has not been submitted for ODM processing.This update is not complete until changes are saved and the Submit for Review button is clicked.
        </div><br/><br/>
        <asp:Button runat="server" ID="btnContinueUpdateOk" Text="Ok" CssClass="buttonBox" OnClick="btnContinueUpdateOk_Click" CausesValidation="false" />
    </asp:Panel>
    <asp:Button runat="server" ID="ButtonDummy4" Style="display: none" Text="ButtonDummy4"  />--%>

    <asp:Button runat="server" ID="Button1" Style="display: none" Text="Button1" />
    <asp:HiddenField ID="hdnNERemove" ClientIDMode="Static" runat="server" />

    <script type="text/javascript">
        function BindPageEditFlag() {
            var storage = window.sessionStorage;
            //alert('In BindPageEditFlag');
            $('input[type="text"],input[type="file"],textarea').not(':disabled').on('input', function (e) {
                $('#<%= HasUnsavedData.ClientID %>').val('True');
                storage.setItem("isSpecialityChanged", "True");
            });

            $('input:file').not('.noprompt').on('change', function () {
                $('#<%= HasUnsavedData.ClientID %>').val('True');
                storage.setItem("isSpecialityChanged", "True");
            });

            $('select').not(':disabled').on('click', function () {
                $('#<%= HasUnsavedData.ClientID %>').val('True');
                storage.setItem("isSpecialityChanged", "True");
            });

            $('select').not(':disabled').on('change', function () {
                $('#<%= HasUnsavedData.ClientID %>').val('True');
                storage.setItem("isSpecialityChanged", "True");
            });

            $('input:checkbox,input:radio').not(':disabled').on('change', function () {
                $('#<%= HasUnsavedData.ClientID %>').val('True');
                storage.setItem("isSpecialityChanged", "True");
            });
            $('input[type="radio"]').on('click change', function (e) {
                storage.setItem("isSpecialityChanged", "True");
            });
        }

        BindPageEditFlag();

    </script>

</asp:Content>
