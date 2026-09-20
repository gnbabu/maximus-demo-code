<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_WebAPITesting" Codebehind="WebAPITesting.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<style>
    .formDropDown {
        font-size: 17px;
        height: 44px;
        color: #000;
    }

    .formField {
        height: 44px;
        color: #000;
    }
</style>
<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
    <p style="text-align: center" class="page-main-header">
        <asp:Literal ID="pagelabel" runat="server" Text="Data Management: WebAPI Testing"></asp:Literal>
    </p>
    <br />
</asp:Panel>
<div>
    <asp:ValidationSummary ID="valSummaryWebAPITesting" runat="server" DisplayMode="List" ValidationGroup="WebAPITesting" CssClass="failureNotification" />
</div>
<telerik:radgrid id="rgWebAPITesting" runat="server" rendermode="Lightweight" allowpaging="True" allowsorting="True"
    onneeddatasource="rgWebAPITesting_NeedDataSource" allowfilteringbycolumn="True" skin="PDMSModern"
    cellspacing="0" gridlines="None" oninsertcommand="rgWebAPITesting_InsertCommand" onupdatecommand="rgWebAPITesting_UpdateCommand"
    autogeneratecolumns="false" autogenerateeditcolumn="False">
    <clientsettings>

        <scrolling allowscroll="True" scrollheight="" usestaticheaders="True" savescrollposition="true"></scrolling>

    </clientsettings>
    <mastertableview commanditemdisplay="Top" gridlines="None" datakeynames="ID">
        <columns>
            <telerik:grideditcommandcolumn headertext="<span style='display:none'>Edit</span>">
            </telerik:grideditcommandcolumn>
            <telerik:gridboundcolumn datafield="APIName" headertext="API Name" uniquename="APIName">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="APIDescp" headertext="API Descp" uniquename="APIDescp">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="APIXML" headertext="API Response" uniquename="APIXML" visible="false">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="APIEnabled" headertext="Enabled" uniquename="APIEnabled">
            </telerik:gridboundcolumn>
        </columns>
        <editformsettings editformtype="Template">
            <editcolumn uniquename="EditCol">
            </editcolumn>
            <formtemplate>
                <div class="WhiteBox">
                    <div class="gridEditTable">
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="ohio-field">API Name :</span> </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="150" ID="txtAPIName" runat="server" Text='<%# Bind("APIName") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAPIName" runat="server" ErrorMessage="* API Name is required"
                                    ControlToValidate="txtAPIName">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="ohio-field">XML Description :</span> </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="0" ID="txtAPIDescp" runat="server" Text='<%# Bind("APIDescp") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAPIDescp" runat="server" ErrorMessage="* XML Description is required"
                                    ControlToValidate="txtAPIDescp">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="ohio-field">XML :</span> </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="0" ID="txtAPIXML" runat="server" TextMode="multiline" Height="100px" Text='<%# Bind("APIXML") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAPIXML" runat="server" ErrorMessage="* XML Value is required"
                                    ControlToValidate="txtAPIXML">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right"><span class="ohio-field">Enabled : </span></div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="ckbAPIEnabled" Checked='<%# Bind("APIEnabled") %>' />
                            </div>
                        </div>
                        <div class="row" style="padding-top: 20px;">
                            <div class="col-sm-3 text-right">
                                <span class="ohio-field"></span>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                    runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CssClass="buttonBox"
                                    CommandName="Cancel"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </formtemplate>
            <popupsettings scrollbars="None" />
        </editformsettings>
    </mastertableview>
</telerik:radgrid>
