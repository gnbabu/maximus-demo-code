<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_AppSettings" Codebehind="AppSettings.ascx.cs" %>
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
        <asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management: App Settings"></asp:Literal>
    </p>
    <br />
</asp:Panel>
<script type="text/javascript">
    $(document).ready(function () {

        Input = $('.rgFilterRow').children("td").children("input");
        Input.attr('Title', 'Filter');

        pagesize = $('.rcbReadOnly').children("input");
        pagesize.attr('Title', 'Pagesize');

        var tablegriddata = document.getElementById("ctl00_MainContent_AppSettingsID_rgAppSettings_ctl00");
        // Remove the thead element
        var thead = tablegriddata.querySelector("thead");
        tablegriddata.removeChild(thead);
        var tablepager = document.getElementById("ctl00_MainContent_AppSettingsID_rgAppSettings_ctl00_Pager");
        // Remove the thead element
        var thead = tablepager.querySelector("thead");
        tablepager.removeChild(thead);
    });
</script>

<telerik:radgrid id="rgAppSettings" runat="server" rendermode="Lightweight" allowpaging="True" allowsorting="True"
    onneeddatasource="rgAppSettings_NeedDataSource" allowfilteringbycolumn="True" skin="PDMSModern"
    cellspacing="0" gridlines="None" oninsertcommand="rgAppSettings_InsertCommand" onupdatecommand="rgAppSettings_UpdateCommand"
    autogeneratecolumns="false" autogenerateeditcolumn="False">
    <clientsettings>

        <scrolling allowscroll="True" scrollheight="" usestaticheaders="True" savescrollposition="true"></scrolling>

    </clientsettings>
    <mastertableview commanditemdisplay="Top" gridlines="None">
        <columns>
            <telerik:grideditcommandcolumn headertext="<span style='display:none'>Edit</span>">
            </telerik:grideditcommandcolumn>
            <telerik:gridboundcolumn datafield="AppSettingsKey" headertext="App Settings Key" uniquename="AppSettingsKey">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="AppSettingsValue" headertext="App Settings Value" uniquename="AppSettingsValue">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="AppSettingsReadOnly" headertext="App Settings Read Only" uniquename="AppSettingsReadOnly" datatype="System.Boolean">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="AppSettingsNotes" headertext="App Settings Notes" uniquename="AppSettingsNotes">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="AppSettingsEnvironSpecific" headertext="App Settings Environment Specific" uniquename="AppSettingsEnvSpecific" datatype="System.Boolean">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="CanOverwrite" headertext="Disable App Settings Deployment Updates?" uniquename="CanOverwrite" datatype="System.Boolean">
            </telerik:gridboundcolumn>
        </columns>
        <editformsettings editformtype="Template">
            <editcolumn uniquename="EditCol">
            </editcolumn>
            <formtemplate>
                <div class="WhiteBox">
                    <div class="gridEditTable">
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                <span class="ohio-field">App Settings Key</span>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="150" ID="txtAppSettingsKey" runat="server" Text='<%# Bind("AppSettingsKey") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* App Settings Key is required"
                                    ControlToValidate="txtAppSettingsKey"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                <span class="ohio-field">App Settings Value</span>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1000" ID="txtAppSettingsValue" runat="server" Text='<%# Bind("AppSettingsValue") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="* App Settings Value is required"
                                    ControlToValidate="txtAppSettingsValue"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                <span class="ohio-field">Take Action Allowed</span>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="chkTakeActionAllowed" Checked='<%# Bind("AppSettingsReadOnly") %>' />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                <span class="ohio-field">App Settings Notes</span>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="1500" ID="TextBox1" runat="server" Text='<%# Bind("AppSettingsNotes") %>' CssClass="formFieldMultiline">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                <span class="ohio-field">App Settings Environment Specific</span>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="chkAppSettingsEnvironSpecific" Checked='<%# Bind("AppSettingsEnvironSpecific") %>' />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">
                                <span class="ohio-field">Disable App Settings Deployment Updates?</span>
                            </div>
                            <div class="col-sm-9 text-left">
                                <asp:CheckBox runat="server" ID="chkCanOverwrite" Checked='<%# Bind("CanOverwrite") %>' />
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
