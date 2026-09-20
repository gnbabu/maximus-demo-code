<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_ReferenceData_ApplicationFeePaymentType" Codebehind="ApplicationFeePaymentType.ascx.cs" %>
<asp:Panel ID="pnlPageHeader" runat="server" CssClass="pageHeader">
    <p style="text-align: center" class="page-main-header">
        <asp:Literal ID="pagelabel" runat="server" Text="Reference Data Management: Application Fee Payment Type"></asp:Literal>
    </p>
</asp:Panel>

<telerik:radgrid id="rgApplicationFeePaymentType" runat="server" rendermode="Lightweight" allowpaging="True" allowsorting="True"
    onneeddatasource="rgApplicationFeePaymentType_NeedDataSource" allowfilteringbycolumn="False" skin="PDMSModern"
    cellspacing="0" gridlines="None" oninsertcommand="rgApplicationFeePaymentType_InsertCommand" onupdatecommand="rgApplicationFeePaymentType_UpdateCommand"
    autogeneratecolumns="false" autogenerateeditcolumn="False">
    <mastertableview commanditemdisplay="Top" gridlines="None"
        datakeynames="PAYMENT_TYPE_ID">
        <columns>
            <telerik:grideditcommandcolumn>
            </telerik:grideditcommandcolumn>
            <telerik:gridboundcolumn datafield="PAYMENT_TYPE_NAME" headertext="Payment Type" uniquename="PAYMENT_TYPE_NAME">
            </telerik:gridboundcolumn>
        </columns>
        <editformsettings editformtype="Template">
            <editcolumn uniquename="EditCol">
            </editcolumn>
            <formtemplate>
                <div class="WhiteBox">
                    <table class="gridEditTable">
                        <tr>
                            <td>Payment Type Name</td>
                            <td>
                                <asp:TextBox MaxLength="100" ID="txtPaymentTypeName" runat="server" Text='<%# Bind("PAYMENT_TYPE_NAME") %>' Width="100%">
                                </asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btnUpdate" Text='<%# (Container is GridEditFormInsertItem) ? "Insert" : "Update" %>'
                                    runat="server" CommandName='<%# (Container is GridEditFormInsertItem) ? "PerformInsert" : "Update" %>' CssClass="buttonBox buttonBoxFocus"></asp:Button>
                                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CssClass="buttonBox"
                                    CommandName="Cancel"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </formtemplate>
            <popupsettings scrollbars="None" />
        </editformsettings>
    </mastertableview>
</telerik:radgrid>
</div>

