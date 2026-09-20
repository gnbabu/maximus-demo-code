<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_UploadSectionControl" Codebehind="UploadSectionControl.ascx.cs" %>
<%@ Register Src="~/PopupControls/UploadDocument.ascx" TagName="UploadDocument" TagPrefix="uc" %>

<script type="text/javascript"> 
    function openWinContentTemplate() {     
        var radWindow = $find("<%=Radwindow1.ClientID%>");
        radWindow.show();
    }
    function openWinNavigateUrl() {     
        var radWindow = $find("<%=Radwindow1.ClientID%>");
        radWindow.show();

    }

</script>
<style type="text/css">
    .RadWindow {
        background-color: rgba(247, 247, 247, 1) !important;
    }
</style>

<asp:UpdatePanel ID="upShowNames1" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <telerik:RadScriptBlock ID="radscript" runat="server">
          <script type="text/javascript">
              var updateProgress = null;
              var self = this;
            
              function postbackButtonClick() {
                
                  updateProgress = $find("<%=progress.ClientID%>");
                  self.window.setTimeout("updateProgress.set_visible(true)", updateProgress.get_displayAfter());

                  return true;
              }
              function openLink(url) {
                  self.window.open(url, 'newWindow');
              }
              function OnClientFilesUploaded(sender) {
                
                  var $ = $telerik.$;
                  $('#<%=BtnUpload.ClientID%>').click();
               }
             
              function setCustomPosition(sender, args) {
                  
                  if (typeof (sender) == 'undefined') {  return;}
                  
                  sender.moveTo(sender.getWindowBounds().x, 280);
              }

          </script>
        </telerik:RadScriptBlock>
        <div class="formLabelAuto">
            <asp:Label ID="lblUploadRequired" runat="server" Text="" />
        </div>
        <div class="pg-hint4" id="divUploadControlHint" runat="server" visible ="false">
            <span>Please upload a completed and signed ODI Standardized Credentialing Part B (found at: https://insurance.ohio.gov/static/Forms/Documents/INS5036.pdf). 
                   This application is required for credentialing organizations/facilities for the State of Ohio.</span>
        </div>
        <div style="width: 85%; margin: 0 auto; display: table;" id="div45DaysNotice" runat="server">
            <div style="border-style: solid; border-color: #000000; border-width: 0.03em;">
                <div style="background-color: #036; width: 100%;">
                    <span style="padding-left: 1px; text-align: center; font-weight: bold; color: #FFF;">
                        <asp:Label ID="uploadLabel" runat="server" Text="" />
                    </span>
                    <span style="text-align: center; font-style: italic; color: #FFF;">
                        <asp:Label ID="uploadInfo" runat="server" Text="" />
                    </span>
                </div>
                <div style="padding-top: 10px; padding-left: 10px;">
                    <span>
                        <asp:Label ID="LblFileName" Text="" runat="server" ForeColor="Green" CssClass="formLabel" Style="width:auto !important; text-align:left"></asp:Label>
                        &nbsp;
                    <asp:LinkButton ID="LnkButtonDownload" runat="server" OnClick="OnFileDownload" ForeColor="Blue" Visible="false" Style="display:inline-block;">Download</asp:LinkButton>
                        &nbsp;&nbsp;
                    <asp:LinkButton ID="LnkButtonView" runat="server" ForeColor="Blue" Visible="false" OnClientClick="openWinNavigateUrl(); return false;" Style="display:inline-block;">View</asp:LinkButton>
                        &nbsp;&nbsp;
                    <asp:LinkButton ID="LnkButtonDelete" runat="server" OnClick="OnFileRemove" ForeColor="Red" Visible="false" Style="display:inline-block;">Remove</asp:LinkButton>
                        &nbsp;&nbsp;
                    <asp:LinkButton ID="LnkButtonArchive" runat="server" OnClick="OnFileArchive" ForeColor="Red" Visible="false" Style="display:inline-block;">Archive Previous Document</asp:LinkButton>
                    </span>
                    <telerik:RadAsyncUpload ID="RadAsyncUpload1" runat="server" MultipleFileSelection="Disabled" MaxFileInputsCount="1"
                        OnFileUploaded="RadAsyncUpload1_FileUploaded" OnClientFileUploading="postbackButtonClick" OnClientFileSelected="postbackButtonClick" OnClientFileUploaded="OnClientFilesUploaded" EnableAriaSupport="true" EnableEmbeddedSkins="true" Skin="Default" InputSize="45" Localization-Select="Browse" EnableInlineProgress="true" EnableFileInputSkinning="true">
                        <AriaSettings Label="Upload Document" />
                    </telerik:RadAsyncUpload>
                    <asp:Label runat="server" ID="lblErrorMessages" CssClass="error-message" />
                    <asp:HiddenField ID="hdndocumentId" runat="server" />
                    <asp:HiddenField ID="hdnRowId" runat="server" />
                    <asp:HiddenField ID="hdnSectionName" runat="server" />
                </div>
                <asp:Button ID="BtnUpload" runat="server" Style="display: none;" Text="Upload" ValidationGroup="valUpload" CausesValidation="true" />
                <asp:CustomValidator runat="server" ID="CustomValidatorUpload" OnServerValidate="CustomValidator_ServerValidate" ValidationGroup="valUpload" ErrorMessage="Please upload a document" CssClass="failureNotification">
                </asp:CustomValidator>
            
            </div>
        </div>
        <br />
        <br />
        <triggers>
            <asp:AsyncPostBackTrigger ControlID="BtnUpload" EventName="Click" />
        </triggers>
        <telerik:RadWindow RenderMode="Lightweight" ID="Radwindow1" runat="server" Width="700px" Height="800px" VisibleStatusbar="false"
            Modal="true" OffsetElementID="main" OnClientShow="setCustomPosition" Skin="PDMSModern" NavigateUrl="~/Documents/Sligo_Creek_License_exp_12-1-2018.jpg" RestrictionZoneID="NavigateUrlZone">
        </telerik:RadWindow>
    </ContentTemplate>
</asp:UpdatePanel>
        <asp:UpdateProgress ID="progress" runat="server" DynamicLayout="true" DisplayAfter="0">
            <ProgressTemplate >
                <div class="ui-widget-overlay" >
                    <div id="dvLoading" style="position: absolute;top: 50%;left: 50%;margin: -50px 0px 0px -50px;">
                        <img id="imgProgress" alt="Loading" src="../Images/loader.gif" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
  