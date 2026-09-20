<%@ control language="C#" autoeventwireup="true" inherits="UserControls.UserControls_EmailTemplateMaintenance, App_Web_p4ixifjm" %>
<%@ Register Src="~/PopupControls/Separator.ascx" TagName="Separator" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="SCS.WebControls.GroupBox" Namespace="SCS.WebControls" TagPrefix="wc" %>


<div style="text-align: center;">
    <wc:GroupBox ID="TemplateHeaderGroup" Caption="Setup Bulk Job" CaptionStyle-CssClass="bodyTextBold" runat="server">

        <asp:Panel runat="server" ID="step1">
            <div class="title-banner">
                <label>
                    <b>Step 1: Choose a template or Upload your own</b>
                </label>
            </div>
            <div class="title-banner-content">
                <table class="grid contentInput" role="presentation">
                    <%--                    <colgroup>
                        <col style="width: 50%" />
                        <col style="width: 50%" />
                    </colgroup>--%>

                    <tr>
                        <td>
                            <asp:Label ID="Label1" CssClass="formLabel150" runat="server" AssociatedControlID="txtTemplateName">Template name</asp:Label>
                            <asp:TextBox ID="txtTemplateName" runat="server" CssClass="formField" MaxLength="50"/>
                            <asp:RequiredFieldValidator ValidationGroup="groupTemplate" ID="TemplateNameValidator" runat="server" ControlToValidate="txtTemplateName" ErrorMessage="Template name is required"/>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnSingleEmail" Text="Create free form email" OnClick="btnSingleEmail_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDescription" CssClass="formLabel150" runat="server" AssociatedControlID="txtTemplateDescription">Template description</asp:Label>
                            <asp:TextBox ID="txtTemplateDescription" runat="server" CssClass="formField" MaxLength="200"/>
                            <asp:RequiredFieldValidator ValidationGroup="groupTemplate" ID="DescriptionValidator" runat="server" ControlToValidate="txtTemplateDescription" ErrorMessage="Template description is required"/>
                        </td>
                    </tr>

                    <tr>
                        <td>
                           
                            <asp:Label ID="lblUploadFile" CssClass="formLabel150" runat="server" AssociatedControlID="TemplateFileUpload">Select file to upload</asp:Label>
                            <asp:FileUpload ID="TemplateFileUpload" runat="server"/>
                            <asp:Button runat="server" CssClass="buttonBox" ID="btnTemplateSave" Text="Upload File" ValidationGroup="groupTemplate" OnClick="btnTemplateSave_Click"/>
                            <asp:RequiredFieldValidator ValidationGroup="groupTemplate" ID="UploadValidator" runat="server" ControlToValidate="TemplateFileUpload" ErrorMessage="Template file is required"/>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                </table>

                <%-- TEMPLATE LISTING --%>
                <asp:GridView
                    runat="server"
                    ID="grdTemplateList"
                    AllowPaging="True"
                    AllowSorting="True"
                    AutoGenerateColumns="false"
                    CellPadding="3"
                    DataKeyNames="TemplateId"
                    CssClass="RadGrid_PDMSModern"
                    OnPageIndexChanging="grdTemplateList_PageIndexChanging"
                    OnSelectedIndexChanged="grdTemplateList_SelectedIndexChanged">                    
                    <HeaderStyle Font-Size="12pt"/>
                    <PagerStyle Font-Size="12pt"/>
                    <RowStyle Font-Size="12pt" ForeColor="Black" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"/>
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="Button1" Text="Select" runat="server" OnClick="GridView_Button_Click"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="TemplateId" HeaderText="Id">

                        </asp:BoundField>
                        <asp:BoundField DataField="TemplateName" HeaderText="Name">

                        </asp:BoundField>
                        <asp:BoundField DataField="IsActive" HeaderText="IsActive">

                        </asp:BoundField>
                        <asp:BoundField DataField="TemplateNotes" HeaderText="Description"/>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDownload" Text="Download" runat="server" OnClick="GridView_Download_Click"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>


            </div>
        </asp:Panel>


        <asp:Panel runat="server" ID="step2">
            <div class="title-banner">
                <label>
                    <b>Step 2: Upload Provider List</b>
                </label>
            </div>

            <div class="title-banner-content">
                <table class="grid contentInput" role="presentation">
                    <colgroup>
                        <col style="width: 75%"/>
                        <col style="width: 25%"/>
                    </colgroup>

                    <tr>
                        <td>
                            <div style="padding-bottom: 10px;">
                                <asp:Label CssClass="formLabel300EM" runat="server" ID="lblCsvTemplateId" Font-Size="12pt"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblEmailSubject" CssClass="formLabel150EM" runat="server" AssociatedControlID="txtEmailSubject">Enter Email Subject</asp:Label>
                            <asp:TextBox ID="txtEmailSubject" runat="server" CssClass="formField" MaxLength="75" ValidationGroup="csvFileGroup"/>
                            <asp:RequiredFieldValidator ValidationGroup="csvFileGroup" ID="subjectValidator" runat="server" ControlToValidate="txtEmailSubject" ErrorMessage="Email subject is required"/>
                        </td>
                    </tr>

                    <tr id="csvRow1" runat="server">

                        <td colspan="2">
                            <div style="padding-bottom: 10px; padding-bottom: 10px;">
                                Use the Browse button to select the file name from your PC.<br/>
                                Please ensure the file is a csv file with a header row.
                            </div>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    
                        <tr id="csvRow2" runat="server">
                            <td>
                                <asp:Label ID="lblCsvFileUpload" CssClass="formLabel150EM" runat="server" AssociatedControlID="csvFileUpload">Select file to upload</asp:Label>
                                <asp:FileUpload ID="csvFileUpload" runat="server"/>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr id="csvRow3" runat="server">
                            <td colspan="2">
                                <asp:RequiredFieldValidator ValidationGroup="csvFileGroup" ID="TemplateIdValidator" runat="server" ControlToValidate="csvFileUpload" ErrorMessage="Csv file is required"/>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr id="csvRow4" runat="server">
                            <td>
                                <div style="padding: 10px;">
                                    <asp:Button CssClass="buttonBox" runat="server" ID="btnCancel" Text="Back" CausesValidation="false" OnClick="CSVUploadCancel_Click"/>
                                    <span style="margin-right: 50px;">&nbsp;</span>
                                    <asp:Button runat="server" CssClass="buttonBox" ID="Button2" Text="Upload File" ValidationGroup="csvFileGroup" OnClick="CSVUploadButton_Click"/>
                                </div>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr id="providerListRow1" runat="server">
                            <td>
                                <div style="padding: 10px;">
                                    <asp:Button CssClass="buttonBox" runat="server" ID="Button4" Text="Back" CausesValidation="false" OnClick="CSVUploadCancel_Click"/>
                                    <span style="margin-right: 50px;">&nbsp;</span>
                                    <asp:Button CssClass="buttonBox" runat="server" ID="Button3" Text="Load selected providers" CausesValidation="false" OnClick="btnProvider_Click"/>
                                </div>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                </table>
            </div>
        </asp:Panel>

        <asp:Panel runat="server" ID="singleEmail">
            <h2>Single Email</h2>
            <div class="row text-left">
                <asp:Label ID="lblCustomSubject" CssClass="formLabel150EM" runat="server" AssociatedControlID="txtCustomSubject">Enter Email Subject</asp:Label>
                <asp:TextBox ID="txtCustomSubject" runat="server" CssClass="formField" MaxLength="75" ValidationGroup="csvFileGroup" />
                <asp:RequiredFieldValidator ValidationGroup="CustomEmail" ID="valCustomEmailSubject" runat="server" ControlToValidate="txtCustomSubject" ErrorMessage="Email subject is required" />
            </div>
             <div class="row text-left">
                <asp:Label ID="Label2" CssClass="formLabel150EM" runat="server" AssociatedControlID="txtCustomSenders">Enter Email To</asp:Label>
                <asp:TextBox ID="txtCustomSenders" runat="server" CssClass="formField" MaxLength="75" />
                <asp:RequiredFieldValidator ValidationGroup="CustomEmail" ID="valCustomEmailTo" runat="server" ControlToValidate="txtCustomSenders" ErrorMessage="Email To is required" />
            </div>
            <div class="row text-left">
                <asp:RequiredFieldValidator ID="valCustomEmailText" runat="server" ErrorMessage="You must enter some text for the message" ValidationGroup="CustomEmail" ControlToValidate="edCustomEmail"></asp:RequiredFieldValidator>
                <telerik:RadEditor runat="server" ID="edCustomEmail" RenderMode="Lightweight" Width="900px" Height="600px" 
                    ContentFilters="DefaultFilters, PdfExportFilter"  EditModes="Design, Preview" DialogHandlerUrl="~/Telerik.Web.UI.DialogHandler.aspx" OnPreRender="edCustomEmail_PreRender" >
                 </telerik:RadEditor>
            </div>
            <div class="row">
                <div class="col-md-6" id="Tr1" runat="server">
                    <asp:Button CssClass="buttonBox" runat="server" ID="Button5" Text="Back" CausesValidation="false" OnClick="CSVUploadCancel_Click" />
                </div>
                <div class="col-md-6">
                    <asp:Button CssClass="buttonBox" runat="server" ID="btnSendCustomEmail" Text="Send Email" CausesValidation="true" ValidationGroup="CustomEmail" OnClick="btnSendCustomEmail_Click" />
                </div>
            </div>
        </asp:Panel>
    </wc:GroupBox>
</div>