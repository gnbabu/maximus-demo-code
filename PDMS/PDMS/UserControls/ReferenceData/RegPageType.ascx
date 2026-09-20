<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_RegPageType" Codebehind="RegPageType.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajx" %>
<style>
    caption {
        visibility: hidden !important
    }
</style>
<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
    <p style="text-align: center" class="page-main-header">
        <asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management: Reg Page Type "></asp:Literal>
    </p>

</asp:Panel>

<telerik:radgrid id="rgRegPageType" runat="server" rendermode="Lightweight" mastertableview-caption="RegPageType" allowpaging="True" allowsorting="True"
    onneeddatasource="rgRegPageType_NeedDataSource" allowfilteringbycolumn="False" skin="PDMSModern"
    cellspacing="0" gridlines="None" oninsertcommand="rgRegPageType_InsertCommand" onupdatecommand="rgRegPageType_UpdateCommand"
    autogeneratecolumns="false" autogenerateeditcolumn="False">
    <mastertableview commanditemdisplay="Top" gridlines="None"
        datakeynames="REG_PAGE_TYPE_ID">
        <columns>
            <telerik:grideditcommandcolumn>
            </telerik:grideditcommandcolumn>
            <telerik:gridboundcolumn datafield="REG_PAGE_NAME" headertext="Reg Page Name" uniquename="REG_PAGE_NAME">
            </telerik:gridboundcolumn>
            <telerik:gridboundcolumn datafield="SEQUENCE_ID" headertext="Sequence ID" uniquename="SEQUENCE_ID" datatype="System.Int32">
            </telerik:gridboundcolumn>
        </columns>
        <editformsettings editformtype="Template">
            <editcolumn uniquename="EditCol">
            </editcolumn>
            <formtemplate>
                <div class="WhiteBox">
                    <div class="gridEditTable">
                        <div class="row">
                            <div class="col-sm-3 text-right">Reg Page Name </div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox MaxLength="80" ID="txtRegPageName" aria-label="RegPageName" runat="server" Text='<%# Bind("REG_PAGE_NAME") %>' CssClass="formField">
                                </asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="* This field is required"
                                    ControlToValidate="txtRegPageName"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 text-right">Sequence ID</div>
                            <div class="col-sm-9 text-left">
                                <asp:TextBox ID="txtSequenceID" aria-label="SequenceID" runat="server" Text='<%# Bind("SEQUENCE_ID") %>' CssClass="formField">
                                </asp:TextBox>
                                <ajx:filteredtextboxextender id="ftbeTxtSequenceID" runat="server" enabled="True" targetcontrolid="txtSequenceID" filtertype="Numbers" filtermode="ValidChars">
                                </ajx:filteredtextboxextender>
                            </div>
                        </div>
                        <div class="row text-center">
                            <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CssClass="buttonBox"
                                CommandName="Cancel"></asp:Button>
                        </div>
                    </div>
                </div>
            </formtemplate>
            <popupsettings scrollbars="None" />
        </editformsettings>
    </mastertableview>
</telerik:radgrid>
