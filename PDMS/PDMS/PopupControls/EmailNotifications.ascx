<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_EmailNotifications" Codebehind="EmailNotifications.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/PopupControls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<script type="text/javascript">    
//Generating Pop-up Print Preview page
function getPrint(print_area) {
    //Creating new page
    var pp = window.open();
    //Adding HTML opening tag with <HEAD> … </HEAD> portion 
    pp.document.writeln('<HTML><HEAD><title>Provider Email</title><LINK href=Styles.css  type="text/css" rel="stylesheet">')
    pp.document.writeln('<LINK href=PrintStyle.css  type="text/css" rel="stylesheet" media="print"><base target="_self"></HEAD>')
    //Adding Body Tag
    pp.document.writeln('<body MS_POSITIONING="GridLayout" bottomMargin="0" leftMargin="0" topMargin="0" rightMargin="0">');
    //Adding form Tag
    pp.document.writeln('<form  method="post">');

    //Writing print area of the calling page
    pp.document.writeln(document.getElementById(print_area).innerHTML);
    //Ending Tag of </form>, </body> and </HTML>
    pp.document.writeln('</form></body></HTML>');
    pp.document.close();
    setTimeout(function () {
        pp.print();
    }, 500);
}
</script>
<style type="text/css" media="print">  
    .nonPrintable
    {
    display: none;
    }
</style>
<style type="text/css">
    /*#tblPreview > tbody > tr > td:nth-child(1) {
        text-align: right;
        vertical-align: top;
        width: 110px;
    }

    #tblPreview > tbody > tr > td:nth-child(2) {
        text-align: left;
    }

    #tblPreview > tbody > tr > td:nth-child(1) > label.formLabel300,
    #tblPreview > tbody > tr > td:nth-child(1) > span.formLabel300 {
        width: 100px;
    }*/

    /*#tblPreview > tbody > tr > td:nth-child(2) > .formField, #ctl00_ucUserHeader_ucEmails_divBody {
        width: 700px;
    }

    #ctl00_ucUserHeader_ucEmails_divBody {
        border: solid 1px black;
        height: 300px;
        overflow-y: scroll;
    }*/

    .identModalBackground {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.2;
        height: auto;
    }

    .identModalPopup {
        background-color: #FFFFFF;
        border-width: 1px;
        border-style: solid;
        border-color: black;
        padding: 0px;
        width: auto;
        height: auto;
    }
    .fixedWidth {
        white-space:normal !important;
        word-break:break-all;
    }
</style>
<asp:UpdatePanel ID="upEmails" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
<div style="width:auto; padding:5px;">
        <asp:GridView ID="gvEmail" runat="server" AutoGenerateColumns="False" DataKeyNames="COMMUNICATION_EVENT_ID"
            CssClass="gridview" AllowSorting="true" OnSorting="gvEmail_Sorting" OnRowDataBound="gvEmail_RowDataBound" OnRowCommand="gvEmail_RowCommand"
            EmptyDataText="No Email Notifications found." OnRowCreated="gvEmail_RowCreated" RowStyle-VerticalAlign="Top"
            AllowPaging="True" PageSize="5" PagerSettings-Mode="NumericFirstLast" Width="100%" OnPageIndexChanging="gvEmail_PageIndexChanging">
            <Columns>
                <%-- 0--%><asp:BoundField DataField="LAST_MODIFIED_DATE_TIME" HeaderText="Date Sent" ItemStyle-Width="130" ItemStyle-VerticalAlign="Top" SortExpression="LAST_MODIFIED_DATE_TIME" />
                <%-- 1--%><asp:BoundField DataField="COMMUNICATION_EVENT_TYPE" HeaderText="Method" ItemStyle-Width="100" ItemStyle-VerticalAlign="Top" SortExpression="COMMUNICATION_EVENT_TYPE" />
                <%-- 2--%><asp:BoundField DataField="EMAIL_FROM" HeaderText="From" ItemStyle-Width="120" ItemStyle-VerticalAlign="Top" SortExpression="EMAIL_FROM" />
                <%-- 3--%><asp:BoundField DataField="EMAIL_TO" HeaderText="To" ItemStyle-Width="120" ItemStyle-VerticalAlign="Top" SortExpression="EMAIL_TO" ItemStyle-Wrap="true" ItemStyle-CssClass="fixedWidth" />
                <%-- 4--%><asp:BoundField DataField="SUBJECT" HeaderText="Subject" ItemStyle-Width="250" ItemStyle-VerticalAlign="Top" SortExpression="SUBJECT" />
                <%-- 5--%><asp:TemplateField ItemStyle-Width="40" HeaderText="Action">
                    <ItemTemplate>
                        <%-- NOTE! There is a bug in ASP.NET regarding use of ImageButtons inside UpdatePanels in IE 10, thats why this is here. --%>
                       <%-- <asp:ImageButton ID="btnAction" runat="server" CommandName="ViewDetails" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ImageUrl="~/Images/edit.png" />--%>
                        <asp:LinkButton ID="btnAction" runat="server" CommandName="ViewDetails" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" ><img src="../Images/edit.png" alt="Click Here" /></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <%-- 6--%><asp:BoundField DataField="COMMUNICATION_EVENT_ID" Visible="false" />
            </Columns>
            <PagerStyle cssClass="gridViewPager" HorizontalAlign="Right" />  
        <HeaderStyle CssClass="gridViewHeader" />
        <AlternatingRowStyle CssClass="gridViewAltRow" />
        <RowStyle CssClass="gridViewRow" /> 
        <FooterStyle CssClass="gridViewFooter" />
        </asp:GridView>
    </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:ValidationSummary ID="EmailValidationSummary" DisplayMode="List" runat="server" CssClass="failureNotification" ValidationGroup="Emails" />
<asp:UpdatePanel ID="upPreview" runat="server" UpdateMode="Conditional" style="width: 80%; margin-left: auto; margin-right: auto;">
    <ContentTemplate>
        <asp:Panel ID="pnlPreview" runat="server" Visible="false">
        <div id="tblPreview" style="text-align:left" role="presentation">
           <%-- <table width="100%" id="tblPreview" style="padding-top: 20px;">--%>
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <asp:Label ID="lblSubject" runat="server" CssClass="formLabel300" Text="Subject" AssociatedControlID="txtSubject" /></div>
                    <div class="col-sm-10 text-left">
                        <asp:TextBox ID="txtSubject" runat="server" CssClass="formField formField" ReadOnly="true" /></div>
                </div>
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <asp:Label ID="lblSendTo" runat="server" CssClass="formLabel300" Text="Send To" AssociatedControlID="lboxSendTo" /></div>
                    <div class="col-sm-10 text-left">
                        <asp:ListBox ID="lboxSendTo" runat="server" CssClass="formField formField" SelectionMode="Multiple" />
                        <asp:CustomValidator ID="cvSendTo" runat="server" ControlToValidate="lboxSendTo" OnServerValidate="SendToRequired" Display="Static" ValidationGroup="Emails" ErrorMessage="* Select at least one Send To email address." Text="*" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <asp:Label ID="lblBody" runat="server" CssClass="formLabel300" Text="Body" AssociatedControlID="txtBody" /></div>
                    <div class="col-sm-10 text-left">
                        <asp:TextBox ID="txtBody" runat="server" CssClass="formField formField" ReadOnly="true" TextMode="MultiLine" Columns="100" Rows="20" />
                        <div runat="server" id="divBody" style="overflow: auto;" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-2 text-right">
                        <asp:Label ID="lblAttach" runat="server" CssClass="formLabel300" Text="Attachment" /></div>
                    <div class="col-sm-10 text-left">
                        <asp:Panel ID="pnlAttachments" runat="server" />
                    </div>
                </div>
         <%--   </table>--%>
            <%--<div style="float: right;">
                
            </div>--%>
        </div>
        <asp:Button ID="btnResend" runat="server" Text="Resend Email" CssClass="buttonBoxFocus" OnClick="btnResend_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="buttonBox" OnClick="btnCancel_Click" />
        <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="buttonBoxFocus" OnClick="btnPrint_Click" />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<uc1:MessageBox ID="MessageBox2" runat="server" ErrorListCssClass="aligncenterPad" />
<ajax:ModalPopupExtender ID="mpeChangesSaved" runat="server" PopupControlID="pnlModal" TargetControlID="ButtonDummy" CancelControlID="btnModalCancel" BackgroundCssClass="identModalBackground" PopupDragHandleControlID="pnlModal">
</ajax:ModalPopupExtender>
<asp:Panel ID="pnlModal" runat="server" CssClass="identModalPopup" align="center" Style="display: none; padding: 20px; width: 200px;">
    <p>
        <asp:Label ID="lblModal" runat="server" />
    </p>
    <asp:Button runat="server" ID="btnModalCancel" Text="Close" CssClass="buttonBox" CausesValidation="false" />
</asp:Panel>
<asp:Button runat="server" ID="ButtonDummy" Style="display: none" Text="ButtonDummy" />

